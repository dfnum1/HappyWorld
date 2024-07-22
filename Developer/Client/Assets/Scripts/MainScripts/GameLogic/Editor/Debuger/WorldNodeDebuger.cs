#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	WorldNodeDebuger
作    者:	HappLI
描    述:	调试数据
*********************************************************************/
using UnityEditor;
using Framework.Core;
using UnityEngine;
using System.Text;

namespace TopGame.Logic
{
    public class WorldNodeDebuger : AWorldNodeDebuger
    {
    }
    public interface IWorldNodeDebugerEditor
    {
        void OnEnable(AWorldNode pNode);
        void OnDisable(AWorldNode pNode);
        void OnInspectorGUI(AWorldNode pNode);
        void OnSceneGUI(AWorldNode pNode);
    }
    //------------------------------------------------------
    [CustomEditor(typeof(WorldNodeDebuger))]
    [CanEditMultipleObjects]
    public class WorldNodeDebugerEditor : Editor
    {
        struct ExternActorPlayaleDraw : Framework.GraphVisualizer.IUserPlayableExternDraw
        {
            public Actor pActor;

            public void OnDrawInfo(ref StringBuilder sb)
            {
                if(pActor !=null)
                {
                    ActionState pState = pActor.GetCurrentActionState();
                    if(pState!=null)
                    {
                        sb.AppendLine("当前逻辑层播放动作信息");
                        sb.AppendLine("动作:" + pState.GetAnimation(pState.GetIndex()));
                        sb.AppendLine("播放时长:" + pState.GetDelta());
                    }
                }
            }
        }
        IWorldNodeDebugerEditor m_pDebuger =null;
        UnityEngine.Material m_pTestMateral = null;

        ExternActorPlayaleDraw m_pExternDraw = new ExternActorPlayaleDraw();
        private void OnEnable()
        {
            WorldNodeDebuger trans = target as WorldNodeDebuger;
            if (trans.pNode == null) return;
            if (trans.pNode is Actor)
                m_pDebuger = new ActorDebugTransormEditor();
            else if (trans.pNode is AProjectile)
                m_pDebuger = new ProjectileDebugEditor();
            else if (trans.pNode is WorldSceneNode)
                m_pDebuger = new SceneDebugInfoEditor();
            if(m_pDebuger!=null) m_pDebuger.OnEnable(trans.pNode);
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            WorldNodeDebuger trans = target as WorldNodeDebuger;
            if (trans.pNode == null) return;
            if (m_pDebuger != null) m_pDebuger.OnDisable(trans.pNode);
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            WorldNodeDebuger trans = target as WorldNodeDebuger;
            if (trans.pNode == null) return;
            EditorGUILayout.LabelField("ConfigID", trans.pNode.GetConfigID().ToString());
            EditorGUILayout.LabelField("ID", trans.pNode.GetInstanceID().ToString());
            EditorGUILayout.LabelField("空间树索引", trans.pNode.nSpatialCellIndex.ToString());
            EditorGUILayout.LabelField("AI状态:" + trans.pNode.IsEnableAI());
            if(trans.pNode.GetBounds().GetBoundSizeSqr()<=0)
            {
                EditorGUILayout.HelpBox("没有包围盒，不能被索敌", MessageType.Warning);
            }
            if(GraphPlayableUtil.IsGraphPlayable(trans.gameObject))
            {
                if (UnityEngine.GUILayout.Button("动画调试器"))
                {
                    m_pExternDraw.pActor = trans.pNode as Actor;
                    GraphPlayableUtil.DebugPlayable(trans.gameObject, m_pExternDraw);
                }
            }
            if (m_pDebuger != null) m_pDebuger.OnInspectorGUI(trans.pNode);

            GUILayout.BeginHorizontal();
            m_pTestMateral = EditorGUILayout.ObjectField("LerpMat", m_pTestMateral, typeof(Material), false) as Material;
            if (GUILayout.Button("设置"))
            {
                AInstanceAble pAble = trans.GetComponent<AInstanceAble>();
                if(pAble) pAble.LerpToMaterial(m_pTestMateral, 0.8f, 1000, 2, "_Alpha", false);
            }
            if (GUILayout.Button("FadeOut设置"))
            {
                AInstanceAble pAble = trans.GetComponent<AInstanceAble>();
                if(pAble) pAble.FadeoutMaterial();
            }
            GUILayout.EndHorizontal();
        }
        //------------------------------------------------------
        private void OnSceneGUI()
        {
            WorldNodeDebuger trans = target as WorldNodeDebuger;
            if (trans.pNode == null) return;
            if (m_pDebuger != null) m_pDebuger.OnSceneGUI(trans.pNode);
        }
    }
}
#endif
