#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using TopGame.Data;
using UnityEditor.IMGUI.Controls;
using System.Reflection;
using Framework.Data;

namespace TopGame.ED
{
    [Framework.Plugin.PluginEditorWindow("FormWindow", "OpenForm")]
    public class FormWindow : EditorWindow
    {
        public class ListItem : FormAssetView.ItemData
        {
            public object pData;
            public override Color itemColor()
            {
                return Color.white;
            }
        }

        UnityEditor.IMGUI.Controls.TreeViewState m_TreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_AssetListMCHState;
        FormAssetView m_pTreeView = null;

        Data_Base m_dataBase;
        string m_strField = "";
        public List<object> BindDatas = new List<object>();
        FieldInfo[] m_Fileds = null;
        //-----------------------------------------------------
        public static FormWindow OpenForm(Data_Base dataBase, string strField="", List<object> vBindDatas = null)
        {
            FormWindow window = ScriptableObject.CreateInstance(typeof(FormWindow)) as FormWindow;
            window.ShowUtility();
            window.m_strField = strField;
            window.m_dataBase = dataBase;
            window.InitTable();
            if (vBindDatas != null)
                window.BindDatas = vBindDatas;
            return window;
        }
        //-----------------------------------------------------
        public static FormWindow OpenForm(System.Type tableType, string strField = "", List<object> vBindDatas = null)
        {
            if (tableType == null) return null;
            FormWindow window = ScriptableObject.CreateInstance(typeof(FormWindow)) as FormWindow;
            window.ShowUtility();
            window.m_strField = strField;
            Data.DataManager.StartInit();
            window.m_dataBase =  DataEditorUtil.GetTable<Data_Base>(tableType,false);
            window.InitTable();
            if (vBindDatas != null)
                window.BindDatas = vBindDatas;
            return window;
        }
        //-----------------------------------------------------
        void InitTable()
        {
            if (m_dataBase != null)
            {
                m_Fileds = CsvDataFormer.GetDataType(m_dataBase.GetType()).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (m_Fileds == null || m_Fileds.Length <= 0) return;

                float argvW = minSize.x / m_Fileds.Length;

                MultiColumnHeaderState.Column[] colums = new MultiColumnHeaderState.Column[m_Fileds.Length];
                for (int i = 0; i < m_Fileds.Length; ++i)
                {
                    colums[i] = new MultiColumnHeaderState.Column();
                    colums[i].headerContent = new GUIContent(m_Fileds[i].Name, "");
                    colums[i].canSort = false;
                    colums[i].minWidth = argvW;
                    colums[i].width = argvW;
                    colums[i].maxWidth = position.width;
                    colums[i].headerTextAlignment = TextAlignment.Center;
                    colums[i].canSort = false;
                    colums[i].autoResize = true;
                }
                var headerState = new MultiColumnHeaderState(colums);
                m_AssetListMCHState = headerState;

                m_TreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pTreeView = new FormAssetView(m_TreeState, m_AssetListMCHState);
                m_pTreeView.Reload();

                m_pTreeView.OnItemDoubleClick = OnSelect;
                m_pTreeView.OnCellDraw += OnCellGUI;

                RefreshList();
            }
        }
        //-----------------------------------------------------
        protected void OnEnable()
        {
            minSize = new Vector2(1280, 720);
            
        }
        //-----------------------------------------------------
        void RefreshList()
        {
            List<object> vDatas = CsvDataFormer.FormTree(m_dataBase);
            m_pTreeView.BeginTreeData();
            for (int i = 0; i < vDatas.Count; ++i)
            {
                ListItem item = new ListItem();
                item.id = i;
                item.name = m_Fileds[0].GetValue(vDatas[i]).ToString();
                item.pData = vDatas[i];
                m_pTreeView.AddData(item);
            }
            m_pTreeView.EndTreeData();
        }
        //-----------------------------------------------------
        private void OnGUI()
        {
            if (m_pTreeView == null) return;
            m_pTreeView.searchString = EditorGUILayout.TextField("过滤", m_pTreeView.searchString);
            m_pTreeView.OnGUI(new Rect(0,20, position.width, position.height-20));
        }
        //-----------------------------------------------------
        bool OnCellGUI(Rect cellRect, FormAssetView.TreeItemData item, int column, bool bSelected, bool focused)
        {
            if (column < 0 || column >= m_Fileds.Length) return false;

            ListItem list = item.data as ListItem;
            item.displayName = list.id.ToString();

            var val = m_Fileds[column].GetValue(list.pData);

            GUI.Label( cellRect, val!=null? val.ToString():"");
            
            return true;
        }
        //-----------------------------------------------------
        void OnSelect(FormAssetView.ItemData data)
        {
            ListItem list = data as ListItem;
            if(m_dataBase !=null && BindDatas != null && BindDatas.Count== 2)
            {
                FieldInfo field = BindDatas[1] as FieldInfo;
                if(field!=null)
                {
                    for (int i = 0; i < m_Fileds.Length; ++i)
                    {
                        if (m_Fileds[i].Name.ToLower().CompareTo(m_strField.ToLower()) == 0)
                        {
                            try
                            {
                                if (field.FieldType == m_Fileds[i].FieldType)
                                    field.SetValue(BindDatas[0], m_Fileds[i].GetValue(list.pData));
                                else
                                {
                                    string valStr = m_Fileds[i].GetValue(list.pData).ToString();
                                    if (field.FieldType == typeof(int))
                                    {
                                        int temp = 0;
                                        if(int.TryParse(valStr, out temp)) field.SetValue(BindDatas[0], temp);
                                    }
                                    else if (field.FieldType == typeof(uint))
                                    {
                                        uint temp = 0;
                                        if (uint.TryParse(valStr, out temp)) field.SetValue(BindDatas[0], temp);
                                    }
                                    else if (field.FieldType == typeof(byte))
                                    {
                                        byte temp = 0;
                                        if (byte.TryParse(valStr, out temp)) field.SetValue(BindDatas[0], temp);
                                    }
                                    else if (field.FieldType == typeof(short))
                                    {
                                        short temp = 0;
                                        if (short.TryParse(valStr, out temp)) field.SetValue(BindDatas[0], temp);
                                    }
                                    else if (field.FieldType == typeof(ushort))
                                    {
                                        ushort temp = 0;
                                        if (ushort.TryParse(valStr, out temp)) field.SetValue(BindDatas[0], temp);
                                    }
                                    else if (field.FieldType == typeof(long))
                                    {
                                        long temp = 0;
                                        if (long.TryParse(valStr, out temp)) field.SetValue(BindDatas[0], temp);
                                    }
                                    else if (field.FieldType == typeof(ulong))
                                    {
                                        ulong temp = 0;
                                        if (ulong.TryParse(valStr, out temp)) field.SetValue(BindDatas[0], temp);
                                    }
                                    else if (field.FieldType == typeof(float))
                                    {
                                        float temp = 0;
                                        if (float.TryParse(valStr, out temp)) field.SetValue(BindDatas[0], temp);
                                    }
                                    else if (field.FieldType == typeof(string))
                                    {
                                        field.SetValue(BindDatas[0], valStr);
                                    }
                                }
                                
                            }
                            catch (System.Exception ex)
                            {
                                Debug.LogError(ex.ToString());
                                EditorUtility.DisplayDialog("提示", "请将报错信息截图发给程序排查!!", "好的");
                            }
                            break;
                        }
                    }
                }
  
            }
            Close();
        }
        //------------------------------------------------------
        public static void BuildFormerDatas()
        {
            if(EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("提示", "请等编译完成后，再进行操作!", "好的");
                return;
            }
            string classPath = Application.dataPath + "/Scripts/MainScripts/GameData/FormWindow/CsvDataFormer.cs";
            if (File.Exists(classPath))
                File.Delete(classPath);

            List<System.Type> vTypes = new List<System.Type>();
            List<string> vTypeDatas = new List<string>();
            List<string> vTablePointers = new List<string>();
            Assembly assembly = null;
            foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                if (ass.GetName().Name == "MainScripts" || ass.GetName().Name.Contains("FrameworkPlus"))
                {
                    assembly = ass;
                    System.Type[] types = assembly.GetTypes();
                    for (int i = 0; i < types.Length; ++i)
                    {
                        System.Type tp = types[i];
                        if (tp.IsSubclassOf(typeof(Data_Base)) && !tp.IsDefined(typeof(Framework.Plugin.EditorCodeAttribute)))
                        {
                            PropertyInfo dataPI = tp.GetProperty("datas", BindingFlags.Public|BindingFlags.Instance);
                            if(dataPI != null)
                            {
                                System.Type propType = dataPI.PropertyType;
                                if (propType.IsGenericType && propType.GenericTypeArguments.Length == 2)
                                {
                                    string str = propType.GenericTypeArguments[1].FullName;
                                    if(propType.GenericTypeArguments[1].IsGenericType && propType.GenericTypeArguments[1].GenericTypeArguments.Length == 1)
                                    {
                                        str = propType.GenericTypeArguments[1].GenericTypeArguments[0].FullName;
                                    }
                                    if(str.Length > 0)
                                    {
                                        vTypes.Add(tp);
                                        vTypeDatas.Add(str.Replace('+', '.'));
                                        vTablePointers.Add("Data.DataManager.getInstance()." + tp.Name.Replace("CsvData_", ""));
                                    }

                                }
                            }
                        }
                    }
                }
            }
            string code = "";
            code += "#if UNITY_EDITOR\r\n";
            code += "//auto generator code\r\n";
            code += "using Framework.Core;\r\n";
            code += "using System.Collections.Generic;\r\n";
            code += "using Framework.Data;\r\n";
            code += "namespace TopGame.ED\r\n";
            code += "{\r\n";
            code += "\t[TableMapping]\r\n";
            code += "\tpublic partial class CsvDataFormer\r\n";
            code += "\t{\r\n";
            code += "\t\tpublic static System.Type GetDataType(System.Type type)\r\n";
            code += "\t\t{\r\n";
            for (int i = 0; i < vTypes.Count; ++i)
            {
                code += "\t\t\tif(type == typeof(" + vTypes[i].FullName + ")) return typeof(" + vTypeDatas[i] + ");\r\n";
            }
            code += "\t\t\treturn null;\r\n";
            code += "\t\t}\r\n";

            code += "\t\t[TableMapping(\"tabletype\")]\r\n";
            code += "\t\tpublic static System.Type GetTableType(string tableName)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\ttableName = tableName.ToLower();\r\n";
            code += "\t\t\tif (tableName.StartsWith(\"data.\")) tableName = tableName.Replace(\"data.\", \"\");\r\n";
            code += "\t\t\tif (!tableName.StartsWith(\"csvdata_\")) tableName = \"csvdata_\" + tableName;\r\n";
            code += "\t\t\tint hash = UnityEngine.Animator.StringToHash(tableName);\r\n";
            code += "\t\t\tswitch(hash)\r\n";
            code += "\t\t\t{\r\n";
            for (int i = 0; i < vTypes.Count; ++i)
            {
                code += "\t\t\t\tcase "+Animator.StringToHash(vTypes[i].Name.ToLower()) +": return typeof(" + vTypes[i].FullName + ");\r\n";
            }
            code += "\t\t\t}\r\n";
            code += "\t\t\treturn null;\r\n";
            code += "\t\t}\r\n";

            code += "\t\tpublic static List<object> FormTree(Data_Base dataBase)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tSystem.Type type = dataBase.GetType();\r\n";
            code += "\t\t\tList<object> vData = new List<object>();\r\n";
            for (int i = 0; i < vTypes.Count; ++i)
            {
                code += "\t\t\tif(type == typeof(" + vTypes[i].FullName + "))\r\n";
                code += "\t\t\t{\r\n";
                code += "\t\t\t\tforeach(var db in (dataBase as " + vTypes[i].FullName + ").datas)\r\n";
                System.Type propType = vTypes[i].GetProperty("datas").PropertyType;
                if (propType.IsGenericType && propType.GenericTypeArguments.Length == 2 && propType.GenericTypeArguments[1].IsGenericType)
                {
                    code += "\t\t\t\t\tforeach (var sub in db.Value)\r\n";
                    code += "\t\t\t\t\t\tvData.Add(sub);\r\n";
                }
                else
                    code += "\t\t\t\t\tvData.Add(db.Value);\r\n";
                code += "\t\t\t}\r\n";
            }
            code += "\t\t\treturn vData;\r\n";
            code += "\t\t}\r\n";

            code += "\t}\r\n";
            code += "}\r\n";
            code += "#endif\r\n";
            FileStream fs = new FileStream(classPath, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(code);
            writer.Close();
        }
    }
}
#endif