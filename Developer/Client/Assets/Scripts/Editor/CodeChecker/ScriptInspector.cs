/********************************************************************
生成日期:	24:7:2019   11:12
类    名: 	CodeChecker
作    者:	HappLI
描    述:	代码方法体重写检测
*********************************************************************/
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TopGame.Base;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    [CustomEditor(typeof(MonoImporter))]
#if UNITY_2020_1_OR_NEWER
    public class ScriptInspector : UnityEditor.AssetImporters.AssetImporterEditor
#else
    public class ScriptInspector : UnityEditor.Experimental.AssetImporters.AssetImporterEditor
#endif
    {
        public override void OnInspectorGUI()
        {
            MonoImporter monoImporter = base.target as MonoImporter;
            MonoScript script = monoImporter.GetScript();
            System.Type classType = script.GetClass();

            bool flag = script;
            if (flag)
            {
                bool flag2 = !UnityEditorInternal.InternalEditorUtility.IsInEditorFolder(monoImporter.assetPath);
                if (flag2)
                {
                    bool flag3 = !IsTypeCompatible(classType);
                    if (flag3)
                    {
                        EditorGUILayout.HelpBox("No MonoBehaviour scripts in the file, or their names do not match the file name.", MessageType.Info);
                    }
                }
                List<string> list = new List<string>();
                List<UnityEngine.Object> list2 = new List<UnityEngine.Object>();
                bool flag4 = false;
                using (new EditorGUIUtility.IconSizeScope(new Vector2(16f, 16f)))
                {
                    this.ShowFieldInfo(classType, monoImporter, list, list2, ref flag4);
                }
                bool flag5 = list2.Count != 0;
                if (flag5)
                {
                    EditorGUILayout.HelpBox("Default references will only be applied in edit mode.", MessageType.Info);
                }
                bool flag6 = flag4;
                if (flag6)
                {
                    monoImporter.SetDefaultReferences(list.ToArray(), list2.ToArray());
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(monoImporter));
                }
            }
            string path = AssetDatabase.GetAssetPath(target);
            if(path.EndsWith(".cs") &&  GUILayout.Button("检测"))
            {
                CodeChecker.CheckType(script.GetClass() );
            }
            base.ApplyRevertGUI();
        }
        //------------------------------------------------------
        private static bool IsTypeCompatible(System.Type type)
        {
            bool flag = type == null || (!type.IsSubclassOf(typeof(MonoBehaviour)) && !type.IsSubclassOf(typeof(ScriptableObject)));
            return !flag;
        }
        //------------------------------------------------------
        private void ShowFieldInfo(System.Type type, MonoImporter importer, List<string> names, List<UnityEngine.Object> objects, ref bool didModify)
        {
            bool flag = !IsTypeCompatible(type);
            if (!flag)
            {
                this.ShowFieldInfo(type.BaseType, importer, names, objects, ref didModify);
                FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                FieldInfo[] array = fields;
                int i = 0;
                while (i < array.Length)
                {
                    FieldInfo fieldInfo = array[i];
                    bool flag2 = !fieldInfo.IsPublic;
                    if (!flag2)
                    {
                        goto IL_7D;
                    }
                    object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(SerializeField), true);
                    bool flag3 = customAttributes == null || customAttributes.Length == 0;
                    if (!flag3)
                    {
                        goto IL_7D;
                    }
                    IL_111:
                    i++;
                    continue;
                    IL_7D:
                    bool flag4 = fieldInfo.FieldType.IsSubclassOf(typeof(UnityEngine.Object)) || fieldInfo.FieldType == typeof(UnityEngine.Object);
                    if (flag4)
                    {
                        UnityEngine.Object defaultReference = importer.GetDefaultReference(fieldInfo.Name);
                        UnityEngine.Object @object = EditorGUILayout.ObjectField(ObjectNames.NicifyVariableName(fieldInfo.Name), defaultReference, fieldInfo.FieldType, false, new GUILayoutOption[0]);
                        names.Add(fieldInfo.Name);
                        objects.Add(@object);
                        bool flag5 = defaultReference != @object;
                        if (flag5)
                        {
                            didModify = true;
                        }
                    }
                    goto IL_111;
                }
            }
        }
    }
}

