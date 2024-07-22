/********************************************************************
生成日期:	12:28:2020 13:16
类    名: 	UIGrabPasser
作    者:	Happli
描    述:   UIGrabPasser 开关
*********************************************************************/

using UnityEngine.UI;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.URP
{
  //  [RequireComponent(typeof(Image))]
    public class  UIGrabPasser: GrabPasser
    {
        public Image showImage;
        int m_nInnerLastFrame = 0;

        protected override void OnPassed(bool bPassed)
        {
            base.OnPassed(bPassed);
#if UNITY_EDITOR
            if (!Framework.Module.ModuleManager.startUpGame)
                return;
#endif
            if (this.IsUpdateGrab())
            {
                if(showImage) showImage.enabled = true;
                return;
            }
            if (showImage)
            {
                showImage.enabled = false;
            }
        }
        //-------------------------------------------------
        protected override void InnerUpdate()
        {
#if UNITY_EDITOR
            if (!Framework.Module.ModuleManager.startUpGame)
                return;
#endif
            if (this.IsUpdateGrab()) return;
            if (Time.frameCount < m_nInnerLastFrame) return;
            m_nInnerLastFrame = Time.frameCount + 10;
            if (GrabPassFeature.GrabStatMaskFlag == 0)
            {
                if (showImage) showImage.enabled = true; 
            }
            else if (showImage) showImage.enabled = false;
        }
        protected override bool ViewCheck() { return true; }

    }
#if UNITY_EDITOR
    [CustomEditor(typeof(UIGrabPasser), true)]
    public class UIGrabPasserEditor : GrabPasserEditor
    {
        private void OnEnable()
        {
            UIGrabPasser passer = target as UIGrabPasser;
            passer.showImage = passer.GetComponent<Image>();
            if (passer.showImage)
            {
                if (passer.showImage.material == null || !passer.showImage.material.shader.name.Contains("SD/UI/BackBlur"))
                    passer.showImage.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/DatasRef/UI/Materials/UIBlackBlur.mat");
            }
        }
        private void OnDisable()
        {
            UIGrabPasser passer = target as UIGrabPasser;
            passer.showImage = passer.GetComponent<Image>();
            if (passer.showImage)
            {
                if (passer.showImage.material == null || !passer.showImage.material.shader.name.Contains("SD/UI/BackBlur"))
                    passer.showImage.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/DatasRef/UI/Materials/UIBlackBlur.mat");
            }
        }
        public override void OnInspectorGUI()
        {
            UIGrabPasser passer = target as UIGrabPasser;
            passer.showImage = EditorGUILayout.ObjectField("Image", passer.showImage, typeof(Image), true) as Image;
            base.OnInspectorGUI();
        }
    }
#endif
}