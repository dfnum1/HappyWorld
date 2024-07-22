/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	SDK
作    者:	HappLI
描    述:	SDK 参数
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace SDK
{
    [System.Serializable]
    public class SDKParam
    {
        [System.Serializable]
        public struct Param
        {
            public string label;
            public string strValue;
            public int nValue;
        }
        public string sdkName;
        public List<Param> Params;

        public int GetInt(string label)
        {
            if (string.IsNullOrEmpty(label)) return 0;
            for (int i=0; i < Params.Count; ++i)
            {
                if(label.CompareTo(Params[i].label) ==0)
                {
                    return Params[i].nValue;
                }
            }
            return 0;
        }
        public string GetString(string label, string defaultStr = null)
        {
            if (string.IsNullOrEmpty(label)) return defaultStr;
            for (int i = 0; i < Params.Count; ++i)
            {
                if (label.CompareTo(Params[i].label) == 0)
                {
                    return Params[i].strValue;
                }
            }
            return defaultStr;
        }

#if UNITY_EDITOR
        [System.NonSerialized]
        public bool bExpand;

        public bool SetString(string label, string value)
        {
            if (string.IsNullOrEmpty(label)) return false;
            for (int i = 0; i < Params.Count; ++i)
            {
                if (label.CompareTo(Params[i].label) == 0)
                {
                    Param param = Params[i];
                    param.strValue = value;
                    Params[i] = param;
                    return true;
                }
            }
            return false;
        }
        public bool SetInt(string label, int value)
        {
            if (string.IsNullOrEmpty(label)) return false;
            for (int i = 0; i < Params.Count; ++i)
            {
                if (label.CompareTo(Params[i].label) == 0)
                {
                    Param param = Params[i];
                    param.nValue = value;
                    Params[i] = param;
                    return true;
                }
            }
            return false;
        }
#endif
    }
    //------------------------------------------------------
    [CreateAssetMenu]
    public class SDKSetting : ScriptableObject
    {
        public List<SDKParam> Params;

        public SDKParam GetSDK(string name, bool ingoreCase = false)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (Params == null) return null;
            if (ingoreCase) name = name.ToLower();
            for(int i=0; i < Params.Count; ++i)
            {
                if(ingoreCase)
                {
                    if (name.CompareTo(Params[i].sdkName.ToLower()) == 0)
                    {
                        return Params[i];
                    }
                }
                else
                {
                    if (name.CompareTo(Params[i].sdkName) == 0)
                    {
                        return Params[i];
                    }
                }

            }
            return null;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(SDKSetting))]
    [CanEditMultipleObjects]
    public class SDKSettingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SDKSetting sdk = target as SDKSetting;
            serializedObject.Update();
            if (sdk.Params == null) sdk.Params = new List<SDKParam>();

            for (int i =0; i < sdk.Params.Count; ++i)
            {
                SDKParam param = sdk.Params[i];

                GUILayout.BeginHorizontal();
                param.bExpand = EditorGUILayout.Foldout(param.bExpand, param.sdkName);
                if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(60) }))
                {
                    sdk.Params.RemoveAt(i);
                    break;
                }
                GUILayout.EndHorizontal();
                if (param.bExpand)
                {
                    GUILayout.BeginHorizontal();
                    param.sdkName = EditorGUILayout.TextField("sdk名", param.sdkName);
                    if (GUILayout.Button("添加参数", new GUILayoutOption[] { GUILayout.Width(120) }))
                    {
                        if (param.Params == null) param.Params = new List<SDKParam.Param>();
                        param.Params.Add(new SDKParam.Param());
                    }
                    GUILayout.EndHorizontal();
                    EditorGUI.indentLevel++;
                    if (param.Params == null) param.Params = new List<SDKParam.Param>();
                    for(int j=0; j < param.Params.Count; ++j)
                    {
                        SDKParam.Param value = param.Params[j];
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("参数[" + (j+1) + "]");
                        if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20) }))
                        {
                            param.Params.RemoveAt(j);
                            break;
                        }
                        GUILayout.EndHorizontal();

                        EditorGUI.indentLevel++;
                        value.label = EditorGUILayout.TextField("属性标签", value.label);
                        value.strValue = EditorGUILayout.TextField("字符数据", value.strValue);
                        value.nValue = EditorGUILayout.IntField("值数据", value.nValue);
                        param.Params[j] = value;
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }
            }
            if(GUILayout.Button("添加SDK参数"))
            {
                sdk.Params.Add(new SDKParam());
            }
            if (GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
