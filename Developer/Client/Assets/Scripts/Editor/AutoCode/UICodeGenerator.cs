#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UICodeGenerator
作    者:	HappLI
描    述:	界面代码关联绑定
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.IO;

namespace TopGame.UI
{
    public class UICodeGenerator
    {
        public static string UIGeneratorCode = "/Scripts/MainScripts/GameUI/Generator/";
        class UIGene
        {
            public List<Type> Logics = new List<Type>();
            public Type UI = null;
            public Type View = null;
        }
        public static void Build()
        {
            Assembly assembly = null;
            foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                if (ass.GetName().Name == "MainScripts")
                {
                    assembly = ass;
                    Dictionary<ushort, UIGene> vTypes = new Dictionary<ushort, UIGene>();
                    Type[] types = assembly.GetTypes();
                    for (int i = 0; i < types.Length; ++i)
                    {
                        Type tp = types[i];
                        if (tp.IsDefined(typeof(UIAttribute), false))
                        {
                            UIAttribute attr = (UIAttribute)tp.GetCustomAttribute(typeof(UIAttribute));
                            if(attr.uiType > 0)
                            {
                                UIGene pui;
                                if(!vTypes.TryGetValue(attr.uiType, out pui))
                                {
                                    pui = new UIGene();
                                    vTypes.Add(attr.uiType, pui);
                                }
                                if (attr.attri == EUIAttr.UI)
                                {
                                    if (pui.UI != null)
                                        Debug.LogError("UI操作实体类已绑定!");
                                    pui.UI = tp;
                                }
                                else if (attr.attri == EUIAttr.Logic)
                                {
                                    if(tp.GetCustomAttribute<UIAttribute>().isAuto)
                                        pui.Logics.Add(tp);
                                }
                                else if (attr.attri == EUIAttr.View)
                                {
                                    if (pui.View != null)
                                        Debug.LogError("存在相同属性类型的视图操作句柄!");
                                    pui.View = tp;
                                }
                                else
                                    Debug.LogError("未定义的界面属性类型");
                            }
                        }
                    }

                    string genePath = Application.dataPath + "/" + UIGeneratorCode;
                    BuildCode(genePath, vTypes, "TopGame.UI");
                }
                //else if (ass.GetName().Name == "MainScripts")
                //{
                //    assembly = ass;
                //    Dictionary<ushort, UIGene> vTypes = new Dictionary<ushort, UIGene>();
                //    Type[] types = assembly.GetTypes();
                //    for (int i = 0; i < types.Length; ++i)
                //    {
                //        Type tp = types[i];
                //        if (tp.IsDefined(typeof(Hot.UI.UIAttribute), false))
                //        {
                //            UIAttribute attr = (UIAttribute)tp.GetCustomAttribute(typeof(UIAttribute));
                //            if (attr.uiType > 0)
                //            {
                //                UIGene pui;
                //                if (!vTypes.TryGetValue(attr.uiType, out pui))
                //                {
                //                    pui = new UIGene();
                //                    vTypes.Add(attr.uiType, pui);
                //                }
                //                if (attr.attri == EUIAttr.Logic)
                //                {
                //                    if (pui.Logic != null)
                //                        Debug.LogError("存在相同属性类型的逻辑操作句柄!");
                //                    pui.Logic = tp;
                //                }
                //                else if (attr.attri == EUIAttr.View)
                //                {
                //                    if (pui.View != null)
                //                        Debug.LogError("存在相同属性类型的视图操作句柄!");
                //                    pui.View = tp;
                //                }
                //                else
                //                    Debug.LogError("未定义的界面属性类型");
                //            }
                //        }
                //    }

                //    string genePath = Application.dataPath + "/" + UIGeneratorCode;
                //    BuildCode(genePath, vTypes, "Hot.UI");
                //}
            }
        }
        //------------------------------------------------------
        static void BuildCode(string strPath, Dictionary<ushort, UIGene> vGenes, string strNameSpace)
        {
            if (Directory.Exists(strPath))
                Directory.Delete(strPath, true);
            Directory.CreateDirectory(strPath);
            string strFile = strPath + "UIBinderBuilder.cs";
            if (File.Exists(strFile))
            {
                File.Delete(strFile);
            }
            string code = "//auto generator\r\n";
            code += "namespace " + strNameSpace + "\r\n";
            code += "{\r\n";
            code += "\tpublic partial class UIBinderBuilder\r\n";
            code += "\t{\r\n";
            code += "\t\tpublic static UIBase Create(ushort type)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tswitch(type)\r\n";
            code += "\t\t\t{\r\n";
            foreach (var db in vGenes)
            {
                if(db.Value.UI == null)
                {
                    Debug.LogError(db.Key + ": 该类型的操作实体界面没有进行绑定!");
                    continue;
                }
                code += "\t\t\t\tcase " + db.Key + ":\r\n";
                code += "\t\t\t\t{\r\n";
                code += "\t\t\t\t\tUIBase pUI = new " + db.Value.UI.FullName + "();\r\n";
                for(int i = 0; i < db.Value.Logics.Count; ++i)
                {
                    if (db.Value.Logics[i] != null)
                    {
                        code += "\t\t\t\t\t{\r\n";
                        code += "\t\t\t\t\t\t" + db.Value.Logics[i].FullName + " pLogic = new " + db.Value.Logics[i].FullName + "();\r\n";
                        code += "\t\t\t\t\t\tpUI.AddLogic( pLogic );\r\n";
                        code += "\t\t\t\t\t}\r\n";
                    }
                }

                if (db.Value.View != null)
                {
                    code += "\t\t\t\t\t" + db.Value.View.FullName + " pView = new " + db.Value.View.FullName + "();\r\n";
                    code += "\t\t\t\t\tpUI.SetView( pView );\r\n";
                }
                code += "\t\t\t\t\treturn pUI;\r\n";
                code += "\t\t\t\t}\r\n";
            }
            code += "\t\t\t}\r\n";
            code += "\t\t\treturn null;\r\n";
            code += "\t\t}\r\n";

            code += "\t\tstatic System.Collections.Generic.Dictionary<System.Type, int> ms_vUITypeMaps = null;\r\n";
            code += "\t\tstatic void CheckTypeMapping()\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tif(ms_vUITypeMaps == null)\r\n";
            code += "\t\t\t{\r\n";
            code += "\t\t\t\tms_vUITypeMaps = new System.Collections.Generic.Dictionary<System.Type, int>("+ vGenes.Count+ ");\r\n";
            foreach (var db in vGenes)
            {
                code += "\t\t\t\tms_vUITypeMaps[typeof(" + db.Value.UI.FullName + ")] = " + db.Key + ";\r\n";
            }
            code += "\t\t\t}\r\n";

            code += "\t\t}\r\n";


            code += "\t\tpublic static int GetTypeToUIType(System.Type type)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tCheckTypeMapping();\r\n";
            code += "\t\t\tint result =0;\r\n";
            code += "\t\t\tif(ms_vUITypeMaps.TryGetValue(type, out result)) return result;\r\n";
            code += "\t\t\treturn 0;\r\n";
            code += "\t\t}\r\n";

            code += "\t}\r\n";
            code += "}\r\n";

            FileStream fs = new FileStream(strFile, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(code);
            writer.Close();
        }
    }
}
#endif