/********************************************************************
生成日期:	24:7:2019   11:12
类    名: 	AutoMarcros
作    者:	HappLI
描    述:	自动定义
*********************************************************************/
using System.Collections.Generic;
using System.IO;
using TopGame.Base;
using TopGame.Data;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public static class TextureNormalAOTool
    {
        public static void NormalAOChecker(string[] CheckPaths)
        {
            if (CheckPaths == null || CheckPaths.Length <= 0) return;
            List<string> resPaths = new List<string>();
            string[] materials = AssetDatabase.FindAssets("t:Material", CheckPaths);
            EditorUtility.DisplayProgressBar("AO Normal", "", 0);
            for (int i = 0; i < materials.Length; ++i)
            {
                EditorUtility.DisplayProgressBar("AO Normal", AssetDatabase.GUIDToAssetPath(materials[i]), (float)i / (float)materials.Length);
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(materials[i]));
                if (mat != null && mat.shader != null && mat.shader.name.Contains("SD_ActorMatcap"))
                {
                    int USE_NORMAL_AO = mat.GetInt("USE_NORMAL_AO");
                    if (USE_NORMAL_AO != 0) continue;
                    Texture2D _BumpMap = mat.GetTexture("_BumpMap") as Texture2D;
                    Texture2D _AOMap = mat.GetTexture("_AOMap") as Texture2D;
                    if (_BumpMap != null && _AOMap!=null)
                    {
                        string assetPath = AssetDatabase.GetAssetPath(_BumpMap);
                        TextureImporter textureImport = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(_BumpMap)) as TextureImporter;
                        bool mipmapEnabled = textureImport.mipmapEnabled;

                        Texture2D normalAO = null;
                        TextureConvertLogic.ConvertUnityNormalToNormalRG(_BumpMap, _AOMap, mipmapEnabled, ref normalAO);
                        assetPath = System.IO.Path.GetDirectoryName(assetPath).Replace("\\", "/") + "/" + System.IO.Path.GetFileNameWithoutExtension(assetPath) + "_AO.png";
                        System.IO.File.WriteAllBytes(assetPath, normalAO.EncodeToPNG());
                        resPaths.Add(assetPath);
                        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);


                        TextureImporter textureImporter = TextureImporter.GetAtPath(assetPath) as TextureImporter;
                        textureImporter.textureType = textureImport.textureType;


                        textureImporter.mipmapEnabled = textureImport.mipmapEnabled;
                        textureImporter.sRGBTexture = textureImport.sRGBTexture;
                        textureImporter.filterMode = textureImport.filterMode;
                        textureImporter.wrapMode = textureImport.wrapMode;

                        TextureImporterPlatformSettings android = textureImport.GetPlatformTextureSettings("Android");
                        if (android != null)
                            textureImporter.SetPlatformTextureSettings(android);

                        TextureImporterPlatformSettings iphone = textureImport.GetPlatformTextureSettings("iPhone");
                        if (iphone != null)
                            textureImporter.SetPlatformTextureSettings(iphone);
                        textureImporter.SaveAndReimport();

                        mat.SetInt("USE_NORMAL_AO", 1);
                        mat.EnableKeyword("USE_NORMAL_AO");
                        mat.SetTexture("_BumpMap", AssetDatabase.LoadAssetAtPath<Texture>(assetPath));
                        mat.SetTexture("_AOMap", null);
                        EditorUtility.SetDirty(mat);
                    }
                }
            }
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
    }
}

