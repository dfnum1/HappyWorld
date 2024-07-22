#if UNITY_EDITOR
/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	UIEditorTemper
作    者:	HappLI
描    述:	UI 编辑器临时数据，预制体中不存在该挂件
*********************************************************************/

using Framework.ED;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TopGame.UI;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    [CustomEditor(typeof(UIEditorTemper), true)]
    public class UIEditorTemperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            UIEditorTemper edit = target as UIEditorTemper;
            edit.eUIType = HandleUtilityWrapper.PopEnum("UI Type", edit.eUIType, "TopGame.UI.EUIType");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SaveToPrefab"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ReferToPic"), true);
            if (GUILayout.Button("加载参考图"))
            {
                UIEditorHelper.CreateReferToPic(edit);
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Desc"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("strAuthor"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isFullUI"), true);
            if(edit != null)
            {
                UISerialized serialize = edit.GetComponent<UISerialized>();
                if(serialize)
                {
                    if (GUILayout.Button("导出保存"))
                    {
                        var record = edit.GetComponent<UIRecord>();
                        if (record != null)
                        {
                            record.OnExportUI();
                        }

                        UIEditorHelper.ExportUIPrefab(edit.GetComponent<UISerialized>());

                        UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
                        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
                    }
                    if (GUILayout.Button("导出保存并生成代码框架"))
                    {
                        UIEditorHelper.ExportUIPrefab(edit.GetComponent<UISerialized>());
                        string panel = edit.transform.name;
                        if (panel.EndsWith("Panel"))
                            panel = panel.Replace("Panel", "");

                        string rootName = panel + "Panel";
                        string strFile = Application.dataPath + "/Scripts/MainScripts/GameUI/Panel/" + rootName;
                        if (!System.IO.Directory.Exists(strFile))
                        {
                            CreateScript(edit, edit.isFullUI, panel, strFile);
                        }
                        else
                            ED.EditorKits.OpenPathInExplorer(strFile);

                        UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
                        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
                    }
                }

            }

            serializedObject.ApplyModifiedProperties();
        }
        //------------------------------------------------------
        void CreateScript(UIEditorTemper editor, bool bFullUI, string baseName, string strRoot)
        {
            if(string.IsNullOrEmpty(editor.SaveToPrefab) || AssetDatabase.LoadAssetAtPath<GameObject>(editor.SaveToPrefab) == null)
            {
                EditorUtility.DisplayDialog("提示", "请先将界面导成预制体!", "好的");
                return;
            }
            if(editor.eUIType ==0)
            {
                EditorUtility.DisplayDialog("提示", "请先设置UI类型,如果是新界面，请在enum EUIType 中添加枚举!", "好的");
                return;
            }
            if(string.IsNullOrEmpty(editor.strAuthor))
            {
                EditorUtility.DisplayDialog("提示", "请先设置开发作者！", "好的");
                return;
            }

            //! uiconfig asset fit
            UIConfig uiConfig = AssetDatabase.LoadAssetAtPath<UIConfig>("Assets/Datas/Config/UIConfig.asset");
            if(uiConfig!=null)
            {
                for (int i = 0; i < uiConfig.UIS.Length; ++i)
                {
                    if(uiConfig.UIS[i].type == (ushort)editor.eUIType)
                    {
                        editor.eUIType = uiConfig.UIS[i].type;
                        EditorUtility.DisplayDialog("提示", "UI 类型可能重复,如果是新界面，请在enum EUIType 中添加枚举!", "好的");
                        return; 
                    }
                }
            }

            string strPanelCs = strRoot + "/" + baseName + "Panel.cs";
            string strViewCs = strRoot + "/" + baseName + "View.cs";
            if (System.IO.File.Exists(strPanelCs) || System.IO.File.Exists(strViewCs))
                return;

            if (!System.IO.Directory.Exists(strRoot))
                System.IO.Directory.CreateDirectory(strRoot);
            System.DateTime nowTime = System.DateTime.Now;
            string date_time = string.Format("{0}:{1}:{2}   {3}:{4}", nowTime.Day, nowTime.Month, nowTime.Year, nowTime.Hour, nowTime.Minute);

            string serializeCode = "";
            string serializeFied = "";
            UISerialized serialzer = editor.GetComponent<UISerialized>();
            if(serialzer && serialzer.Widgets != null)
            {
                HashSet<string> vHashed = new HashSet<string>();
                for(int i = 0; i < serialzer.Widgets.Length; ++i)
                {
                    if (serialzer.Widgets[i].widget == null) continue;
                    string field = serialzer.Widgets[i].widget.name;
                    if (!string.IsNullOrEmpty(serialzer.Widgets[i].fastName))
                        field = serialzer.Widgets[i].fastName;

                    string typeName = serialzer.Widgets[i].widget.GetType().FullName.Replace("+", ".");
                    serializeFied+= "\t\t//" + typeName + "\tm_" + field + ";\r\n";

                    serializeCode += "\t\t\t\t" + "//m_"+ field + " = m_pBaseUI.GetWidget<" + typeName + ">(\"" + field + "\");\r\n";
                }
            }

            //Panel
            {
                string file = Application.dataPath + "/Scripts/Editor/UIEditor/tml/UITmlPanel.txt";
                if (File.Exists(file))
                {
                    List<string> tmlCode = new List<string>();
                    FileStream fs = new FileStream(file, FileMode.Open);
                    StreamReader reader = new StreamReader(fs, System.Text.Encoding.UTF8);
                    string sStuName = string.Empty;
                    while ((sStuName = reader.ReadLine()) != null)
                    {
                        tmlCode.Add(sStuName + "\r\n");
                    }
                    reader.Close();
                    fs.Close();

                    file = strPanelCs;


                    ParseKeyFiled(tmlCode, "#DATE_TIME#", date_time);
                    ParseKeyFiled(tmlCode, "#AUTHOR#", editor.strAuthor);
                    ParseKeyFiled(tmlCode, "#DESC#", editor.Desc);
                    ParseKeyFiled(tmlCode, "#CLASSNAME#", baseName + "Panel");
                    ParseKeyFiled(tmlCode, "#TYPE_NAME#", ((TopGame.UI.EUIType)editor.eUIType).ToString());
                    ParseKeyFiled(tmlCode, "#SERIALIZED_FIELD#", serializeFied);
                    ParseKeyFiled(tmlCode, "#SERIALIZED_CODE#", serializeCode);

                    if (File.Exists(file))
                        File.Delete(file);

                    StreamWriter writer = new StreamWriter(file, false, System.Text.Encoding.UTF8);
                    for (int i = 0; i < tmlCode.Count; ++i)
                        writer.Write(tmlCode[i]);
                    writer.Close();
                }
            }
            //View
            {
                string file = Application.dataPath + "/Scripts/Editor/UIEditor/tml/UITmlView.txt";
                if (File.Exists(file))
                {
                    List<string> tmlCode = new List<string>();
                    FileStream fs = new FileStream(file, FileMode.Open);
                    StreamReader reader = new StreamReader(fs, System.Text.Encoding.UTF8);
                    string sStuName = string.Empty;
                    while ((sStuName = reader.ReadLine()) != null)
                    {
                        tmlCode.Add(sStuName+"\r\n");
                    }
                    reader.Close();
                    fs.Close();

                    ParseKeyFiled(tmlCode, "#DATE_TIME#", date_time);
                    ParseKeyFiled(tmlCode, "#AUTHOR#", editor.strAuthor);
                    ParseKeyFiled(tmlCode, "#DESC#", editor.Desc);
                    ParseKeyFiled(tmlCode, "#CLASSNAME#", baseName + "View");
                    ParseKeyFiled(tmlCode, "#TYPE_NAME#", ((TopGame.UI.EUIType)editor.eUIType).ToString());
                    ParseKeyFiled(tmlCode, "#SERIALIZED_FIELD#", serializeFied);
                    ParseKeyFiled(tmlCode, "#SERIALIZED_CODE#", serializeCode);


                    file = strViewCs;
                    if (File.Exists(file))
                        File.Delete(file);

                    StreamWriter writer = new StreamWriter(file, false, System.Text.Encoding.UTF8);
                    for (int i = 0; i < tmlCode.Count; ++i)
                        writer.Write(tmlCode[i]);
                    writer.Close();
                }
            }

            if(uiConfig!=null)
            {
                List<UIConfig.UI> uIs = uiConfig.UIS!=null? new List<UIConfig.UI>(uiConfig.UIS):new List<UIConfig.UI>();
                UIConfig.UI uI = new UIConfig.UI();
                uI.type = (ushort)editor.eUIType;
                uI.prefab = editor.SaveToPrefab;
                uI.permanent = false;
                uI.alwayShow = false;
                uI.Order = editor.uiOrder;
                uI.fullUI = bFullUI;
                uI.uiZValue = 0;
                uI.backups = null;
                uI.canBackupFlag = 0;
                uI.trackAble = editor.trackAble;
                uIs.Add(uI);
                uiConfig.UIS = uIs.ToArray();

                EditorUtility.SetDirty(uiConfig);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            //! 
            UICodeGenerator.Build();

            ED.EditorKits.OpenPathInExplorer(strRoot);
        }
        //------------------------------------------------------
        static void ParseKeyFiled(List<string> vLine, string strKey, string strCode)
        {
            for (int i = 0; i < vLine.Count; ++i)
            {
                if (vLine[i].Trim().Contains(strKey))
                {
                    vLine[i] = vLine[i].Replace(strKey, strCode);
                }
            }
        }
    }
}
#endif
