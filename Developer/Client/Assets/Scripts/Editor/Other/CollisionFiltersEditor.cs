#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	CollisionConfig
作    者:	HappLI
描    述:	碰撞标志
*********************************************************************/
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using System.IO;
using Framework.Core;
using Framework.Base;
#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif
#if USE_SERVER
using ScriptableObject = ExternEngine.ScriptableObject;
#endif
namespace TopGame.Core
{
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(CollisionFilters), true)]
    public class CollisionConfigEditor : UnityEditor.Editor
    {
        struct DrawTemp
        {
            [Framework.Data.DisplayNameGUI("飞行道具碰撞过滤")]
            [Framework.Data.DisplayEnumBitGUI(typeof(EObstacleType), true)]
            public ushort bitObstacleIgnoreFilter;
        }
        EActorType m_curType = EActorType.Player;
        DrawTemp m_DrawTemp = new DrawTemp();
        bool m_bExpandMagent = false;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CollisionFilters controller = target as CollisionFilters;
            if(controller.bitObstacleIgnoreFilter == null || controller.bitObstacleIgnoreFilter.Length!= (int)EActorType.Count)
            {
                List<ushort> vAssets = new List<ushort>();
                for (int i = vAssets.Count; i < (int)EActorType.Count; ++i)
                {
                    if (controller.bitObstacleIgnoreFilter != null && i < controller.bitObstacleIgnoreFilter.Length)
                        vAssets.Add(controller.bitObstacleIgnoreFilter[i]);
                    else vAssets.Add(0);
                }
                controller.bitObstacleIgnoreFilter = vAssets.ToArray();
            }
            m_curType = (EActorType)Framework.ED.EditorEnumPop.PopEnum("过滤障碍碰撞", m_curType);
            int typeIndex = (int)m_curType;
            if (typeIndex>=0 && typeIndex< controller.bitObstacleIgnoreFilter.Length)
            {
                m_DrawTemp.bitObstacleIgnoreFilter = controller.bitObstacleIgnoreFilter[typeIndex];
                m_DrawTemp = (DrawTemp)Framework.ED.HandleUtilityWrapper.DrawProperty(m_DrawTemp, null);
                controller.bitObstacleIgnoreFilter[typeIndex] = m_DrawTemp.bitObstacleIgnoreFilter;
            }

            m_bExpandMagent = EditorGUILayout.Foldout(m_bExpandMagent, "吸铁石");
            if(m_bExpandMagent)
            {
                EditorGUI.indentLevel++;
                Framework.ED.HandleUtilityWrapper.DrawPropertyByFieldName(controller, "magentFlilter", null);
                EditorGUI.indentLevel--;
            }

            if (GUILayout.Button("刷新"))
            {
                UnityEditor.EditorUtility.SetDirty(target);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
}
#endif