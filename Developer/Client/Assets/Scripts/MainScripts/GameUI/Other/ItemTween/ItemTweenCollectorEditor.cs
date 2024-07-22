#if UNITY_EDITOR
/********************************************************************
生成日期:	5:31:2022 14:45
类    名: 	ItemTweenCollector
作    者:	hjc
描    述:	道具收集目标
*********************************************************************/
using Framework.Core;
using TopGame.Core;
using UnityEngine;
using Framework.ED;
using UnityEditor;

namespace TopGame.UI
{
    [CustomEditor(typeof(ItemTweenCollector), true)]
    public class ItemTweenCollectorEditor : Editor
    {
        void Awake()
        {
            ItemTweenCollector collector = target as ItemTweenCollector;
            if (collector.UIType == 0)
            {
                UIEditorTemper ui = collector.GetComponentInParent<UIEditorTemper>();
                if (ui) collector.UIType = ui.eUIType;
            }
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ItemTweenCollector collector = target as ItemTweenCollector;
            collector.ItemId = UnityEditor.EditorGUILayout.IntField("道具ID", collector.ItemId);
            collector.ShowRoot = UnityEditor.EditorGUILayout.ObjectField("根节点", collector.ShowRoot, typeof(Transform), true) as Transform;
            collector.UIType = HandleUtilityWrapper.PopEnum("绑定UI", collector.UIType, "TopGame.UI.EUIType");
            collector.Priority = UnityEditor.EditorGUILayout.IntField("优先级", collector.Priority);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif