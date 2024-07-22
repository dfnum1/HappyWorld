/********************************************************************
生成日期:	1:29:2021 10:06
类    名: 	ContentSizeFitterExpand
作    者:	zdq
描    述:	扩展 ContentSizeFitter 组件,添加最小尺寸设置功能
*********************************************************************/
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TopGame.UI
{
    public class ContentSizeFitterExpand : ContentSizeFitter
    {
        public float MinSize = 0f;

        [System.NonSerialized] private RectTransform m_Rect;
        protected RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        public override void SetLayoutHorizontal()
        {
            base.SetLayoutHorizontal();
            if (rectTransform == null)
            {
                return;
            }
            SetMinSize(0);
        }
        //------------------------------------------------------
        public override void SetLayoutVertical()
        {
            base.SetLayoutVertical();

            if (rectTransform == null)
            {
                return;
            }

            SetMinSize(1);
        }
        //------------------------------------------------------
        protected void SetMinSize(int axis)
        {
            FitMode fitting = (axis == 0 ? horizontalFit : verticalFit);
            if (fitting == FitMode.Unconstrained)
            {
                return;
            }

            Vector2 size = rectTransform.rect.size;
            if (size[axis] < MinSize)
            {
                size[axis] = MinSize;
                rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, MinSize);
            }
        }

    }


#if UNITY_EDITOR
    [CustomEditor(typeof(ContentSizeFitterExpand))]
    public class ContentSizeFitterExpandEditor : UnityEditor.UI.ContentSizeFitterEditor
    {
        SerializedProperty m_ShowType;
        protected override void OnEnable()
        {
            base.OnEnable();
            m_ShowType = serializedObject.FindProperty("MinSize");
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(m_ShowType);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}

