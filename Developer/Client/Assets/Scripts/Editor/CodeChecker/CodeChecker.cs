/********************************************************************
生成日期:	24:7:2019   11:12
类    名: 	CodeChecker
作    者:	HappLI
描    述:	代码方法体重写检测
*********************************************************************/
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopGame.Base;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class CodeChecker
    {
        public class CheckerCode
        {
            public System.Type baseType;
            public HashSet<string> methods = new HashSet<string>();
            public string CheckFieldClear = null;
            public bool CallBaseFuncCheck = true;

            public List<string> Clears = new List<string>();
            public bool CanCheckMethod(System.Reflection.MethodInfo method)
            {
                if (methods.Contains(method.Name))
                {
                    //      System.Reflection.MethodInfo baseMethod = baseType.GetMethod(method.Name);
                    //      if (baseMethod != null) return true;
                    return true;
                }
                return false;
            }

            public bool HasCallBase(System.Reflection.MethodInfo method)
            {
                try
                {
                    var reader = new ILReader(method);
                    foreach (var instr in reader)
                    {
                        var str = instr.ToString();
                        if (str.Contains("call " + baseType.FullName + "." + method.Name))
                            return true;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning(ex.StackTrace);
                    return false;
                }
                return false;
            }

            public void CheckClearField(System.Reflection.MethodInfo method, Dictionary<int, string> vFieldNames, HashSet<string> vFields)
            {
                string error = "";
                try
                {
                    var reader = new ILReader(method);
                    foreach(var field in vFieldNames)
                    {
                        foreach (var instr in reader)
                        {
                            var str = instr.ToString();
                            if (str.Contains("stfld " + field.Key))
                            {
                                vFields.Remove(field.Value);
                                break;
                            }
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning(ex.StackTrace);
                }
            }
        }
        //------------------------------------------------------
        static Dictionary<System.Type, CheckerCode> CheckerDefined()
        {
            Dictionary<System.Type, CheckerCode> vChecker = new Dictionary<System.Type, CheckerCode>();
            {//ui
                CheckerCode ui = new CheckerCode();
                ui.baseType = typeof(TopGame.UI.UIBase);
                ui.methods.Add("Awake");
                ui.methods.Add("Destroy");
                ui.methods.Add("Hide");
                ui.methods.Add("DoHide");
                ui.methods.Add("Show");
                ui.methods.Add("DoShow");
                ui.methods.Add("DoShowEnd");
                ui.methods.Add("Clear");
                ui.methods.Add("Close");
                ui.methods.Add("DoClose");

                ui.Clears.Add("Clear");
                ui.Clears.Add("Destroy");
                ui.Clears.Add("Hide");
                ui.Clears.Add("DoHide");
                ui.Clears.Add("Close");
                ui.Clears.Add("DoClose");

                vChecker[ui.baseType] = ui;
            }
            {//ui
                CheckerCode ui = new CheckerCode();
                ui.baseType = typeof(TopGame.UI.UIView);
                ui.methods.Add("Destroy");
                ui.methods.Add("Clear");
                ui.methods.Add("Init");

                ui.Clears.Add("Hide");
                ui.Clears.Add("Clear");
                ui.Clears.Add("Destroy");

                vChecker[ui.baseType] = ui;
            }
            {//ui
                CheckerCode ui = new CheckerCode();
                ui.CallBaseFuncCheck = false;
                ui.baseType = typeof(TopGame.UI.UILogic);

                ui.Clears.Add("OnClose");
                ui.Clears.Add("OnDestroy");
                ui.Clears.Add("OnHide");
                ui.Clears.Add("OnClear");
                ui.Clears.Add("Clear");

                vChecker[ui.baseType] = ui;
            }
//             {//ui
//                 CheckerCode ui = new CheckerCode();
//                 ui.baseType = typeof(TopGame.Data.AProxyDB);
//                 ui.CheckFieldClear = "Clear";
//                 vChecker[ui.baseType] = ui;
//             }
            return vChecker;
        }
        //------------------------------------------------------
        public static void CheckType(System.Type type, Dictionary<System.Type, CheckerCode> vChecker = null)
        {
            CheckerCode checkCode = null;
            if(vChecker == null) vChecker = CheckerDefined();

            checkCode = null;
            System.Type checkType = type.BaseType;
            while (checkType != null)
            {
                if (vChecker.TryGetValue(checkType, out checkCode))
                {
                    break;
                }
                checkType = checkType.BaseType;
            }
            if (checkCode != null)
            {
                checkType = type;
                if(checkCode.CallBaseFuncCheck)
                {
                    if (checkCode.methods.Count > 0)
                    {
                        try
                        {
                            System.Reflection.MethodInfo[] methods = checkType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                            for (int k = 0; k < methods.Length; ++k)
                            {
                                if (!methods[k].IsVirtual || !checkCode.CanCheckMethod(methods[k])) continue;
                                if (!checkCode.HasCallBase(methods[k]))
                                {
                                    Debug.LogError(checkType.FullName + "::" + methods[k].Name + "   没有调用父函数!!!");
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError("解析失败"+ ex.ToString());
                        }

                    }
                }

                if(checkCode.Clears.Count>0)
                {
                    HashSet<string> vField = new HashSet<string>();
                    Dictionary<int, string> vFieldNames = new Dictionary<int, string>();
                    System.Reflection.FieldInfo[] vFields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                    if (vFields != null)
                    {
                        for (int i = 0; i < vFields.Length; ++i)
                        {
                            if (vFields[i].IsStatic || vFields[i].IsInitOnly) continue;
                            if (vFields[i].FieldType.IsValueType) continue;
                            vFieldNames[vFields[i].MetadataToken] = vFields[i].Name;
                            vField.Add(vFields[i].Name);
                        }
                    }
                    vFields = type.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                    if (vFields != null)
                    {
                        for (int i = 0; i < vFields.Length; ++i)
                        {
                            if (vFields[i].IsStatic || vFields[i].IsInitOnly) continue;
                            if (vFields[i].FieldType.IsValueType) continue;
                            vFieldNames[vFields[i].MetadataToken] = vFields[i].Name;
                            vField.Add(vFields[i].Name);
                        }
                    }
                    for (int i = 0; i < checkCode.Clears.Count; ++i)
                    {
                        System.Reflection.MethodInfo clearMethod = checkType.GetMethod(checkCode.Clears[i], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                        if(clearMethod!=null)
                        {
                            if (!clearMethod.IsVirtual) return;
                             checkCode.CheckClearField(clearMethod, vFieldNames, vField);
                        }
                    }

                    if (vField.Count > 0)
                    {
                        foreach(var db in vField)
                            Debug.LogError(checkType.FullName + "::" + db + "  成员变量未清理!!!\n");
                    }
                }
            }
        }
        //------------------------------------------------------
        [MenuItem("Tools/代码/类接口重写检测")]
        static void OverrideChecker()
        {
            Dictionary<System.Type, CheckerCode> vChecker = CheckerDefined();
            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                System.Type[] types = assembly.GetTypes();
                for(int i = 0; i < types.Length; ++i)
                {
                    CheckType(types[i], vChecker);
                }
            }
        }
    }
}

