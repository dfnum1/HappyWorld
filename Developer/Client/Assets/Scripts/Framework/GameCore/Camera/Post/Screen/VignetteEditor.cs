#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Assertions;

namespace TopGame.Post
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Vignette))]
    public class VignetteEditor : APostEditor
    {
        SerializedProperty m_shader;
        SerializedProperty m_Mode;
        SerializedProperty m_Color;
        SerializedProperty m_Center;
        SerializedProperty m_Intensity;
        SerializedProperty m_Smoothness;
        SerializedProperty m_Roundness;
        SerializedProperty m_Rounded;

        SerializedProperty m_Mask;
        SerializedProperty m_Opacity;

        void OnEnable()
        {
            Vignette bloom = target as Vignette;
            if (bloom.shader == null)
                bloom.shader = Shader.Find("SD/Post/SD_Vignette");
            m_shader = serializedObject.FindProperty("m_shader");
            m_Mode = serializedObject.FindProperty("mode");
            m_Color = serializedObject.FindProperty("color");
            m_Center = serializedObject.FindProperty("center");
            m_Intensity = serializedObject.FindProperty("intensity");
            m_Smoothness = serializedObject.FindProperty("smoothness");
            m_Roundness = serializedObject.FindProperty("roundness");
            m_Rounded = serializedObject.FindProperty("rounded");
            m_Mask = serializedObject.FindProperty("mask");
            m_Opacity = serializedObject.FindProperty("opacity");
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_shader);
            EditorGUILayout.PropertyField(m_Mode);
            EditorGUILayout.PropertyField(m_Color);
            if (m_Mode.intValue == (int)Vignette.VignetteMode.Classic)
            {
                EditorGUILayout.PropertyField(m_Center);
                EditorGUILayout.PropertyField(m_Intensity);
                EditorGUILayout.PropertyField(m_Smoothness);
                EditorGUILayout.PropertyField(m_Roundness);
                EditorGUILayout.PropertyField(m_Rounded);
            }
            else
            {
                EditorGUILayout.PropertyField(m_Mask);

                var mask = (target as Vignette).mask;

                // Checks import settings on the mask
                if (mask != null)
                {
                    var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(mask)) as TextureImporter;

                    // Fails when using an internal texture as you can't change import settings on
                    // builtin resources, thus the check for null
                    if (importer != null)
                    {
                        bool valid = importer.anisoLevel == 0
                            && importer.mipmapEnabled == false
                            && importer.alphaSource == TextureImporterAlphaSource.FromGrayScale
                            && importer.textureCompression == TextureImporterCompression.Uncompressed
                            && importer.wrapMode == TextureWrapMode.Clamp;

                        if (!valid)
                            DrawFixMeBox("Invalid mask import settings.", () => SetMaskImportSettings(importer));
                    }
                }

                EditorGUILayout.PropertyField(m_Opacity);
            }

            serializedObject.ApplyModifiedProperties();
        }
        //------------------------------------------------------
        void SetMaskImportSettings(TextureImporter importer)
        {
            importer.textureType = TextureImporterType.SingleChannel;
            importer.alphaSource = TextureImporterAlphaSource.FromGrayScale;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.anisoLevel = 0;
            importer.mipmapEnabled = false;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.SaveAndReimport();
            AssetDatabase.Refresh();
        }
    }
}
#endif