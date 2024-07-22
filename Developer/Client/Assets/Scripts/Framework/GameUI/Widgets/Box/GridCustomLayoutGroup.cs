/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GridCustomLayoutGroup
作    者:	HappLI
描    述:	根据数量自定义布局
*********************************************************************/

using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.UI
{
    [UIWidgetExport]
    public class GridCustomLayoutGroup : GridLayoutGroup
    {
        [System.Serializable]
        public struct Custom
        {
            public int gridCount;
            public int constraintCount;
            public Vector2 cellSize;
            public Vector2 spacing;
        }
        public Custom[] customs;
        
        public void SetGridCount(int cnt)
        {
            if (customs == null) return;
            for(int i = 0; i < customs.Length; ++i)
            {
                if(customs[i].gridCount == cnt)
                {
                    this.constraintCount = customs[i].constraintCount;
                    this.cellSize = customs[i].cellSize;
                    this.spacing = customs[i].spacing;
                    break;
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GridCustomLayoutGroup), true)]
    [CanEditMultipleObjects]
    public class GridCustomLayoutGroupEditor : UnityEditor.UI.GridLayoutGroupEditor
    {
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GridCustomLayoutGroup image = target as GridCustomLayoutGroup;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("customs"), true);
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
#endif
}
