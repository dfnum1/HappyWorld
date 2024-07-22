#if UNITY_EDITOR
/********************************************************************
生成日期:	2020-06-12
类    名: 	LocalizationText
作    者:	zdq
描    述:	多语言控制Text显示组件,使用方法,将此脚本挂载到显示的Text控件上
*********************************************************************/
using System;
using UnityEngine;
using UnityEngine.UI;
using TopGame.Data;
using UnityEditor;
namespace TopGame.Core
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LocalizationText))]
    class LocalizationTextEditor : Editor
    {
        void OnEnable()
        {
            if (target is LocalizationText)
            {
                LocalizationText text = (LocalizationText)target;
                if (text != null && text.m_text == null)
                {
                    text.m_text = text.GetComponent<Text>();
                    if (text.ID == 0)
                    {
                        GetIDByText();
                    }
                }
            }
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawToolBtns();

            serializedObject.ApplyModifiedProperties();
        }
        //------------------------------------------------------
        void DrawToolBtns()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("根据中文获取多语言ID"))
            {
                if (target is LocalizationText)
                {
                    var text = target as LocalizationText;
                    Undo.RecordObject(text, "localization");
                    GetIDByText();
                }
            }
            if (GUILayout.Button("根据多语言ID获取中文"))
            {
                if (target is LocalizationText)
                {
                    var text = target as LocalizationText;
                    Undo.RecordObject(text.GetComponent<Text>(), "localization");
                    GetChineseByID();
                }
            }
            if (GUILayout.Button("根据多语言ID获取英文"))
            {
                if (target is LocalizationText)
                {
                    var text = target as LocalizationText;
                    Undo.RecordObject(text.GetComponent<Text>(), "localization");
                    GetEnglishByID();
                }
            }

            EditorGUILayout.EndHorizontal();
        }
        //------------------------------------------------------
        [ContextMenu("根据中文获取多语言ID")]
        public void GetIDByText()
        {
            LocalizationText tagetText = target as LocalizationText;
            Text text = tagetText.GetComponent<Text>();
            if (text == null)
            {
                return;
            }

            CsvData_Text csv = null;
            DataManager dataManager = null;
            if (DataManager.getInstance() == null)
            {
                dataManager = new DataManager();
                csv = dataManager.Text;
            }
            else
            {
                dataManager = DataManager.getInstance();
                csv = dataManager.Text;
            }
            if (csv == null)
            {
                Framework.Plugin.Logger.Error("Csv数据为null");
                return;
            }
            var datas = csv.datas;

            //tagetText.ID = 0;
            foreach (var item in datas)
            {
                if (item.Value.textCN.Equals(text.text))
                {
                    tagetText.ID = item.Key;
                    break;
                }
            }

            UnityEditor.EditorUtility.SetDirty(target);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        //------------------------------------------------------
        [ContextMenu("根据多语言ID获取中文")]
        public void GetChineseByID()
        {
            LocalizationText tagetText = target as LocalizationText;
            Text text = tagetText.GetComponent<Text>();
            if (text == null)
            {
                return;
            }



            CsvData_Text csv = null;
            DataManager dataManager = null;
            if (DataManager.getInstance() == null)
            {
                dataManager = new DataManager();
                csv = dataManager.Text;
            }
            else
            {
                dataManager = DataManager.getInstance();
                csv = dataManager.Text;
            }
            if (csv == null)
            {
                Framework.Plugin.Logger.Error("Csv数据为null");
                return;
            }
            var datas = csv.datas;

            if (datas.ContainsKey(tagetText.ID))
            {
                text.text = datas[tagetText.ID].textCN;
            }

            UnityEditor.EditorUtility.SetDirty(target);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        //------------------------------------------------------
        [ContextMenu("根据多语言ID获取英文")]
        public void GetEnglishByID()
        {
            LocalizationText tagetText = target as LocalizationText;
            Text text = tagetText.GetComponent<Text>();

            if (text == null)
            {
                return;
            }



            CsvData_Text csv = null;
            DataManager dataManager = null;
            if (DataManager.getInstance() == null)
            {
                dataManager = new DataManager();
                csv = dataManager.Text;
            }
            else
            {
                dataManager = DataManager.getInstance();
                csv = dataManager.Text;
            }
            if (csv == null)
            {
                Framework.Plugin.Logger.Error("Csv数据为null");
                return;
            }
            var datas = csv.datas;

            foreach (var item in datas)
            {
                if (item.Key == tagetText.ID)
                {
                    text.text = item.Value.textEN;
                    break;
                }
            }

            UnityEditor.EditorUtility.SetDirty(target);
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }
}
#endif