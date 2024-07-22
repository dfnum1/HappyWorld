#if UNITY_EDITOR
/********************************************************************
生成日期:	2020-06-12
类    名: 	LocalizationImage
作    者:	zdq
描    述:	多语言控制Image显示组件,使用方法,将此脚本挂载到显示的Image控件上
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using TopGame.Data;
using TopGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.Core
{
    [UnityEditor.CustomEditor(typeof(LocalizationRawImage), true)]
    [UnityEditor.CanEditMultipleObjects]
    public class LocalizationRawImageEditor : UnityEditor.Editor
    {
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("打印当前图片路径"))
            {
                PrintCurImagePath();
            }
            if (GUILayout.Button("加载中文图片"))
            {
                LoadChineseImage();
            }
            if (GUILayout.Button("加载英文图片"))
            {
                LoadEnglishImage();
            }
        }
        //------------------------------------------------------
        void OnEnable()
        {
            LocalizationRawImage rawImage = target as LocalizationRawImage;
            if (rawImage && rawImage.IsEmpty())
            {
                LoadChineseImage();
            }
            if (rawImage&& rawImage.m_RawImage ==null)
            {
                rawImage.m_RawImage = rawImage.GetComponent<RawImage>();
            }
        }
        //------------------------------------------------------
        void PrintCurImagePath()
        {
            LocalizationRawImage image = target as LocalizationRawImage;
            image.m_RawImage = image.GetComponent<RawImage>();
            if (image.m_RawImage == null)
            {
                return;
            }
            //m_image.sprite
            Debug.Log(UnityEditor.AssetDatabase.GetAssetPath(image.m_RawImage.texture));
        }
        //------------------------------------------------------
        public void LoadChineseImage()
        {
            LocalizationRawImage image = target as LocalizationRawImage;
            image.m_RawImage = image.GetComponent<RawImage>();
            if (image.m_RawImage == null)
            {
                return;
            }
            Debug.Log(1);
            CsvData_Text.TextData cfg = EditorGetLocalization(image.ID);
            if (cfg != null)
            {
                Debug.Log(2);
                Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(cfg.textCN);
                if (sprite != null)
                {
                    Debug.Log(3);
                    image.m_RawImage.texture = sprite.texture;

                    if (image.m_RawImage is RawImageEx)
                    {
                        ((RawImageEx)image.m_RawImage).texturePath = UnityEditor.AssetDatabase.GetAssetPath(sprite);
                    }
                    UnityEditor.EditorUtility.SetDirty(target);
                }
            }
        }
        //------------------------------------------------------
        void LoadEnglishImage()
        {
            LocalizationRawImage image = target as LocalizationRawImage;
            image.m_RawImage = image.GetComponent<RawImage>();
            if (image.m_RawImage == null)
            {
                return;
            }
            Debug.Log(1);
            CsvData_Text.TextData cfg = EditorGetLocalization(image.ID);
            if (cfg != null)
            {
                Debug.Log(2);
                Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(cfg.textEN);
                if (sprite != null)
                {
                    Debug.Log(3);
                    image.m_RawImage.texture = sprite.texture;

                    if (image.m_RawImage is RawImageEx)
                    {
                        ((RawImageEx)image.m_RawImage).texturePath = UnityEditor.AssetDatabase.GetAssetPath(sprite);
                    }
                    UnityEditor.EditorUtility.SetDirty(target);
                }
            }
        }
        //------------------------------------------------------
        CsvData_Text.TextData EditorGetLocalization(uint id)
        {
            DataManager dataManager = null;
            if (DataManager.getInstance() == null)
            {
                dataManager = new DataManager();
            }
            else
            {
                dataManager = DataManager.getInstance();
            }
            if (dataManager == null)
            {
                Framework.Plugin.Logger.Error("Csv数据为null");
                return null;
            }

            var cfg = dataManager.Text.GetData(id);
            if (cfg != null)
            {
                return cfg;
            }
            return null;
        }
    }
}
#endif