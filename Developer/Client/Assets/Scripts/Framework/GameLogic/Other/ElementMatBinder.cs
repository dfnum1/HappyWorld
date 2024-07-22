/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ElementMatBinder
作    者:	HappLI
描    述:	元素属性材质绑定器
*********************************************************************/
using UnityEngine;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif
namespace TopGame.Logic
{
    public class ElementMatBinder : MonoBehaviour
    {
        public Material[] elementMats = new Material[(int)EElementType.Count];
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ElementMatBinder))]
    [CanEditMultipleObjects]
    public class ElementMatBinderEditor : Editor
    {
        bool m_bExpandEle = false;
        //    UnityEditorInternal.ReorderableList m_TextureReorderableList;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ElementMatBinder agb = target as ElementMatBinder;

            System.Type enumType = typeof(EElementType);
            foreach (System.Enum v in System.Enum.GetValues(enumType))
            {
                FieldInfo fi = enumType.GetField(v.ToString());
                string strTemName = v.ToString();
                if (fi != null && fi.IsDefined(typeof(Framework.Plugin.DisableGUIAttribute)))
                    continue;
                if (fi != null && fi.IsDefined(typeof(Framework.Plugin.PluginDisplayAttribute)))
                {
                    strTemName = fi.GetCustomAttribute<Framework.Plugin.PluginDisplayAttribute>().displayName;
                }
                int val = System.Convert.ToInt32(v);
                agb.elementMats[val] = EditorGUILayout.ObjectField(strTemName, agb.elementMats[val], typeof(Material), false) as Material;
            }

            serializedObject.ApplyModifiedProperties();
            if (GUILayout.Button("刷新保存"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
#endif
}
