/********************************************************************
生成日期:	12:28:2020 13:16
类    名: 	FitImage
作    者:	Happli
描    述:	图片适配
*********************************************************************/

using UnityEngine.UI;
using UnityEngine;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif

namespace TopGame.UI
{
	[UIWidgetExport]
    [ExecuteAlways]
    public class FitImage : Image
    {
        public UI.EFitScreenType screenFitType = EFitScreenType.AutoNearly;
        private Vector2 m_backupSize = Vector2.zero;

        RectTransform m_RectTransform;
        //------------------------------------------------------
        protected override void Awake()
        {
            base.Awake();
            m_RectTransform = transform as RectTransform;
            this.RegisterDirtyMaterialCallback(DirtyFit);
            if (sprite)
            {
                DirtyFit();
            }
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            this.UnregisterDirtyMaterialCallback(DirtyFit);
            base.OnDestroy();
        }
        //------------------------------------------------------
        public void SetDirtyFit()
        {
            m_backupSize = Vector2.zero;
            DirtyFit();
        }
        //------------------------------------------------------
        protected virtual void DirtyFit()
        {
            if (sprite && m_RectTransform)
            {
                Vector2 size = sprite.bounds.size * sprite.pixelsPerUnit;
                if(!Framework.Core.BaseUtil.Equal(m_backupSize, size, 1))
                {
                    m_backupSize = size;
                    UI.UIKits.ScaleWithScreenSize(screenFitType, ref size);
                    Vector3 scale = Vector3.one;
                    UI.UIKits.ScaleWithScreenScale(ref scale);
                    m_RectTransform.localScale = scale;
                    m_RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    m_RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    m_RectTransform.offsetMin = Vector2.zero;
                    m_RectTransform.offsetMax = Vector2.zero;
                    m_RectTransform.sizeDelta = size;
                    
                }
            }
        }
#if UNITY_EDITOR
        Vector2Int m_Screen = new Vector2Int(Screen.width, Screen.height);
        private void Update()
        {
            if(m_Screen.x != Screen.width || m_Screen.y != Screen.height)
            {
                m_Screen = new Vector2Int(Screen.width, Screen.height);
                m_backupSize = Vector2.zero;
                DirtyFit();
            }
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(FitImage), true)]
    [CanEditMultipleObjects]
    public class FitImageEditor : ImageEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            FitImage image = target as FitImage;

            EditorGUI.BeginChangeCheck();
            image.screenFitType = (UI.EFitScreenType)EditorGUILayout.EnumPopup("Screen Fit Type", image.screenFitType);
            if (EditorGUI.EndChangeCheck())
                image.SetDirtyFit();

            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
#endif
}