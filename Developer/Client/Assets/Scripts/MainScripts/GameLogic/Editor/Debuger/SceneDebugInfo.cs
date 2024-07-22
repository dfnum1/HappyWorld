#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ActorDebugTransorm
作    者:	HappLI
描    述:	调试数据
*********************************************************************/
using Framework.Core;

namespace TopGame.Logic
{
    public class SceneDebugInfoEditor : IWorldNodeDebugerEditor
    {
        public void OnEnable(AWorldNode sceneNode) { }
        public void OnDisable(AWorldNode sceneNode) { }
        public void OnInspectorGUI(AWorldNode sceneNode)
        {
            if (sceneNode == null) return;
//             EditorGUILayout.Vector3Field("位置", sce.sceneNode.curPos);
//             EditorGUILayout.Vector3Field("下一段对接位置", sce.sceneNode.nextPos);
//             if(sce.sceneNode.partData != null)
//             {
//                 EditorGUILayout.LabelField(sce.sceneNode.partData.name + "[" + sce.sceneNode.partData.nID + "]");
// 
//             }
        }
        public void OnSceneGUI(AWorldNode sceneNode)
        {

        }
    }
}
#endif
