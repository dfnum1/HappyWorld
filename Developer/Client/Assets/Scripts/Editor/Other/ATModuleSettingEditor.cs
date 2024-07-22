/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	图形脚本模块配置
作    者:	HappLI
描    述:	
*********************************************************************/
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace TopGame.Data
{
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(ATModuleSetting), true)]
    public class ATModuleSettingEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ATModuleSetting setting = target as ATModuleSetting;

            GUILayout.BeginHorizontal();
            setting.mainAT = UnityEditor.EditorGUILayout.ObjectField("主线模块", setting.mainAT, typeof(Framework.Plugin.AT.AgentTreeData), false) as Framework.Plugin.AT.AgentTreeData;
            if (setting.mainAT !=null && GUILayout.Button("编辑"))
            {
                Framework.Plugin.AT.AgentTreeEditor.Editor(setting.mainAT, target);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            setting.uiMgrAT = UnityEditor.EditorGUILayout.ObjectField("UI模块", setting.uiMgrAT, typeof(Framework.Plugin.AT.AgentTreeData), false) as Framework.Plugin.AT.AgentTreeData;
            if (setting.uiMgrAT != null && GUILayout.Button("编辑"))
            {
                Framework.Plugin.AT.AgentTreeEditor.Editor(setting.uiMgrAT, target);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            setting.soundAT = UnityEditor.EditorGUILayout.ObjectField("声音模块", setting.soundAT, typeof(Framework.Plugin.AT.AgentTreeData), false) as Framework.Plugin.AT.AgentTreeData;
            if (setting.soundAT != null && GUILayout.Button("编辑"))
            {
                Framework.Plugin.AT.AgentTreeEditor.Editor(setting.soundAT, target);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            setting.dialogSystemAT = UnityEditor.EditorGUILayout.ObjectField("对话系统", setting.dialogSystemAT, typeof(Framework.Plugin.AT.AgentTreeData), false) as Framework.Plugin.AT.AgentTreeData;
            if (setting.dialogSystemAT != null && GUILayout.Button("编辑"))
            {
                Framework.Plugin.AT.AgentTreeEditor.Editor(setting.dialogSystemAT, target);
            }
            GUILayout.EndHorizontal();

            if (setting.stateATs == null || setting.stateATs.Length != (int)Logic.EGameState.Count)
            {
                List<Framework.Plugin.AT.AgentTreeData> datas = (setting.stateATs == null)?(new List<Framework.Plugin.AT.AgentTreeData>()):new List<Framework.Plugin.AT.AgentTreeData>(setting.stateATs);
                for (int i = datas.Count; i < (int)Logic.EGameState.Count; ++i)
                    datas.Add(null);
                while(datas.Count > (int)Logic.EGameState.Count)
                {
                    datas.RemoveAt(datas.Count - 1);
                }
                setting.stateATs = datas.ToArray();
            }

            for(int i = 0; i < (int)Logic.EGameState.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                setting.stateATs[i] = UnityEditor.EditorGUILayout.ObjectField(((Logic.EGameState)i).ToString(), setting.stateATs[i], typeof(Framework.Plugin.AT.AgentTreeData), true) as Framework.Plugin.AT.AgentTreeData;
                if (setting.stateATs[i]!=null && GUILayout.Button("编辑"))
                {
                    Framework.Plugin.AT.AgentTreeEditor.Editor(setting.stateATs[i], target);
                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("刷新"))
            {
                UnityEditor.EditorUtility.SetDirty(target);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.ForceSynchronousImport);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
