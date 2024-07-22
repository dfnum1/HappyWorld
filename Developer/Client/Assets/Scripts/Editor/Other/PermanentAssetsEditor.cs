#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	PermanentAssetsEditor
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using Framework.Core;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace TopGame.Data
{
    [CustomEditor(typeof(PermanentAssets))]
    [CanEditMultipleObjects]
    public class PermanentAssetsEditor : Editor
    {
        bool m_bEpxandAssets = false;
        bool m_bExpandPaths = false;
        bool m_bExpandBuffMats = false;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 150;
            PermanentAssets assets = target as PermanentAssets;
            if (assets.Assets.Length != (int)EPermanentAssetType.Count)
            {
                List<Object> vAssets = assets.Assets!=null?new List<Object>(assets.Assets):new List<Object>();
                for (int i = vAssets.Count; i < (int)EPermanentAssetType.Count; ++i)
                    vAssets.Add(null);
                assets.Assets = vAssets.ToArray();
            }
            if (assets.Paths.Length != (int)EPathAssetType.Count)
            {
                List<PermanentAssets.PathAsset> vAssets = assets.Paths !=null?new List<PermanentAssets.PathAsset>(assets.Paths):new List<PermanentAssets.PathAsset>();
                for (int i = vAssets.Count; i < (int)EPathAssetType.Count; ++i)
                    vAssets.Add(new PermanentAssets.PathAsset() { asset = null, permanent = false });
                assets.Paths = vAssets.ToArray();
            }
            if (assets.BuffMaterials.Length != (int)EBuffEffectBit.Count)
            {
                List<PermanentAssets.BuffMaterial> vAssets = assets.BuffMaterials != null ? new List<PermanentAssets.BuffMaterial>(assets.BuffMaterials) : new List<PermanentAssets.BuffMaterial>();
                for (int i = vAssets.Count; i < (int)EPathAssetType.Count; ++i)
                    vAssets.Add(new PermanentAssets.BuffMaterial() { asset = null, propertyName = null });
                assets.BuffMaterials = vAssets.ToArray();
            }
            m_bEpxandAssets = EditorGUILayout.Foldout(m_bEpxandAssets, "资源对象");
            if(m_bEpxandAssets)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < assets.Assets.Length; ++i)
                {
                    if (!System.Enum.IsDefined(typeof(EPermanentAssetType), (EPermanentAssetType)i))
                        continue;
                    GUILayout.BeginHorizontal();
                    string strTemName = ((EPermanentAssetType)i).ToString();
                    FieldInfo fi = typeof(EPermanentAssetType).GetField(strTemName);
                    if (fi != null && fi.IsDefined(typeof(Framework.Plugin.AT.ATFieldAttribute)))
                    {
                        strTemName = fi.GetCustomAttribute<Framework.Plugin.AT.ATFieldAttribute>().DisplayName;
                    }
                    strTemName += "[" + i + "]";

                    System.Type type = typeof(UnityEngine.Object);
                    if (fi != null && fi.IsDefined(typeof(Framework.Data.ObjectTypeGUIAttribute)))
                    {
                        type = fi.GetCustomAttribute<Framework.Data.ObjectTypeGUIAttribute>().objType;
                    }
                    assets.Assets[i] = EditorGUILayout.ObjectField(strTemName, assets.Assets[i], type,false);
                    GUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }

            m_bExpandPaths = EditorGUILayout.Foldout(m_bExpandPaths, "路径资源对象");
            if (m_bExpandPaths)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < assets.Paths.Length; ++i)
                {
                    if (!System.Enum.IsDefined(typeof(EPathAssetType), (EPathAssetType)i))
                        continue;
                    PermanentAssets.PathAsset item = assets.Paths[i];

                    string strTemName = ((EPathAssetType)i).ToString();
                    FieldInfo fi = typeof(EPathAssetType).GetField(strTemName);
                    if (fi != null && fi.IsDefined(typeof(Framework.Plugin.AT.ATFieldAttribute)))
                    {
                        strTemName = fi.GetCustomAttribute<Framework.Plugin.AT.ATFieldAttribute>().DisplayName;
                    }
                    strTemName += "[" + i + "]";
                    GUILayout.BeginHorizontal();

                    System.Type type = typeof(UnityEngine.Object);
                    if (fi != null && fi.IsDefined(typeof(Framework.Data.ObjectTypeGUIAttribute)))
                    {
                        type = fi.GetCustomAttribute<Framework.Data.ObjectTypeGUIAttribute>().objType;
                    }
                    UnityEngine.Object obj = EditorGUILayout.ObjectField(strTemName, AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(item.asset) , type, false);
                    if (obj)
                        item.asset = AssetDatabase.GetAssetPath(obj);
                    item.permanent = EditorGUILayout.Toggle(item.permanent);
                    assets.Paths[i] = item;
                    GUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }

            m_bExpandBuffMats = EditorGUILayout.Foldout(m_bExpandBuffMats, "Buff材质对象");
            if (m_bExpandBuffMats)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < assets.BuffMaterials.Length; ++i)
                {
                    if (!System.Enum.IsDefined(typeof(EBuffEffectBit), (EBuffEffectBit)i))
                        continue;
                    PermanentAssets.BuffMaterial item = assets.BuffMaterials[i];

                    string strTemName = ((EBuffEffectBit)i).ToString();
                    FieldInfo fi = typeof(EBuffEffectBit).GetField(strTemName);
                    if (fi != null && fi.IsDefined(typeof(Framework.Plugin.AT.ATFieldAttribute)))
                    {
                        strTemName = fi.GetCustomAttribute<Framework.Plugin.AT.ATFieldAttribute>().DisplayName;
                    }
                    if (fi != null && fi.IsDefined(typeof(Framework.Plugin.PluginDisplayAttribute)))
                    {
                        strTemName = fi.GetCustomAttribute<Framework.Plugin.PluginDisplayAttribute>().displayName;
                    }
                    GUILayout.BeginHorizontal();

                    item.asset = EditorGUILayout.ObjectField(strTemName, item.asset, typeof(Material), false) as Material;
                    item.propertyName = EditorGUILayout.TextField(item.propertyName);
                    assets.BuffMaterials[i] = item;
                    GUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
            if (GUILayout.Button("刷新保存"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
            EditorGUIUtility.labelWidth = labelWidth;

        }
    }
}
#endif
