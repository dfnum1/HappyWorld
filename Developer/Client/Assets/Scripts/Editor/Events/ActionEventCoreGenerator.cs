// #if UNITY_EDITOR
// /********************************************************************
// 生成日期:	1:11:2020 10:06
// 类    名: 	ActionEventCoreGenerator
// 作    者:	HappLI
// 描    述:	事件参数序列化生成器
// *********************************************************************/
// 
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using System.Reflection;
// using System;
// using System.IO;
// 
// namespace TopGame.Core
// {
//     public class ActionEventCoreGenerator
//     {
//         public struct EventType
//         {
//             public int property;
//             public Type type;
//             public string FullName;
//             public string Name;
//             public EventDeclarationAttribute attri;
//             public List<FieldInfo> vFields;
// 
//             public List<string> vCppIncludes;
//             public string Code;
//             public string CppCode;
// 
//             public string xmlName;
//         }
//         public static string GeneratorEditorCode = "/Scripts/MainScripts/GameCore/Events/Editor/DrawEventCore.cs";
//         public static string GeneratorCode = "/Scripts/MainScripts/GameCore/Events/ActionEventCore.cs";
//         public static string CPP_GeneratorCode = "/GamePlus/CorePlus/Source/Logic/Events";
//         public static string CS_BinaryGeneratorCode = "/Scripts/MainScripts/AutoSerializeds/Events/";
//         [MenuItem("Tools/代码/事件系统自动化代码")]
//         public static void Build()
//         {
//             List<EventType> vEvents = new List<EventType>();
//             Assembly assembly = null;
//             foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
//             {
//                 if (ass.GetName().Name == "MainScripts")
//                 {
//                     assembly = ass;
//                     Type[] types = assembly.GetTypes();
//                     for (int i = 0; i < types.Length; ++i)
//                     {
//                         Type tp = types[i];
//                         if (tp.IsSubclassOf(typeof(BaseEventParameter)))
//                         {
//                             EventType evTyp = new EventType();
//                             evTyp.type = tp;
//                             evTyp.vCppIncludes = new List<string>();
//                             evTyp.FullName = tp.FullName;
//                             evTyp.Name = tp.Name;
//                             evTyp.xmlName = "ele";
//                             evTyp.attri = evTyp.type.GetCustomAttribute<EventDeclarationAttribute>();
//                             evTyp.property = 0;
//                             if (evTyp.attri != null) evTyp.property = evTyp.attri.nProprity;
//                             evTyp.vFields = new List<FieldInfo>();
//                             FieldInfo[] fiels = tp.GetFields(BindingFlags.Public | BindingFlags.Instance);
//                             for (int j = 0; j < fiels.Length; ++j)
//                             {
//                                 if(fiels[j].IsNotSerialized) continue;
//                                 if (fiels[j].DeclaringType != tp) continue;
//                                 evTyp.vFields.Add(fiels[j]);
//                             }
//                             EventNativeIncludeAttribute[] includes = (EventNativeIncludeAttribute[])evTyp.type.GetCustomAttributes<EventNativeIncludeAttribute>();
//                             if(includes!=null )
//                             {
//                                 for (int j = 0; j < includes.Length; ++j)
//                                     evTyp.vCppIncludes.Add(includes[j].includePath);
//                             }
//                             
//                             vEvents.Add(evTyp);
//                         }
//                     }
//                 }
//             }
//             vEvents.Sort((EventType e0, EventType e1) => { return e1.property - e0.property; });
// 
//             BuildCode(Application.dataPath + GeneratorCode, vEvents, "TopGame.Core");
//             BuildDrawCode(Application.dataPath + GeneratorEditorCode, vEvents);
//             BuildCppCode(Application.dataPath + "/../" + CPP_GeneratorCode, vEvents);
//             BuildBinaryCsCode(Application.dataPath + CS_BinaryGeneratorCode, vEvents, "TopGame.Core");
//          //   BuildEventSerializedCode(Application.dataPath + "/Scripts/MainScripts/AutoSerializeds/Events/", vEvents);
//         }
//         //------------------------------------------------------
//         static void BuildCppCode(string strDir, List<EventType> vTypes, string nameSpace = "TopPlus")
//         {
//             string includeDir = "";
//             int strIndex = strDir.IndexOf("/Source/");
//             if(strIndex > 0)
//             {
//                 includeDir = strDir.Substring(strIndex+ "/Source/".Length);
//                 if (!includeDir.EndsWith("/")) includeDir += "/";
//             }
// 
//             string strEventTypeCode = "";
//             strEventTypeCode+="/* Copyright (C) 2019-2019, happli. All rights reserved.\r\n";
//             strEventTypeCode+="*\r\n";
//             strEventTypeCode+="* File:    EEventType.h\r\n";
//             strEventTypeCode+= "* Desc:    EEventType\r\n";
//             strEventTypeCode+="* Version: 1.0\r\n";
//             strEventTypeCode+="* Author: auto generator\r\n";
//             strEventTypeCode+="*\r\n";
//             strEventTypeCode+="* This file is part of the game.\r\n";
//             strEventTypeCode+="*/\r\n";
//             strEventTypeCode+= "#pragma once\r\n";
//             strEventTypeCode+="namespace " + nameSpace + "\r\n";
//             strEventTypeCode += "{\r\n";
//             strEventTypeCode += "\tenum EEventType //: tInt16\r\n";
//             strEventTypeCode += "\t{\r\n";
//             for (int i = 0; i < vTypes.Count; ++i)
//             {
//                 strEventTypeCode += "\t\tEEventType_" + vTypes[i].attri.eType + "=" + (int)(vTypes[i].attri.eType) + ",";
//                 if(!string.IsNullOrEmpty(vTypes[i].attri.EventName))
//                 {
//                     strEventTypeCode += "//" + vTypes[i].attri.EventName;
//                 }
//                 strEventTypeCode += "\r\n";
//             }
//             strEventTypeCode += "\t};\r\n";
//             strEventTypeCode += "}\r\n";
// 
//             bool bOk = true;
// 
//             for (int i = 0; i < vTypes.Count; ++i)
//             {
//                 EventType evType = vTypes[i];
//                 List<string> strCode = new List<string>();
//                 List<string> strCppCode = new List<string>();
// 
//                 strCppCode.Add("#include \"stdafx.h\"\r\n");
//                 strCppCode.Add("#include \""+ includeDir + evType.Name + ".h\"\r\n");
//                 strCppCode.Add("#include \"" + includeDir + "EEventType.h\"\r\n");
//                 strCppCode.Add("namespace " + nameSpace + "\r\n");
//                 strCppCode.Add("{\r\n");
// 
//                 strCode.Add( "/* Copyright (C) 2019-2019, happli. All rights reserved.\r\n");
//                 strCode.Add( "*\r\n");
//                 strCode.Add( "* File:    "+ evType.Name + ".h\r\n");
//                 strCode.Add( "* Desc:    "+ evType.Name + "\r\n");
//                 strCode.Add( "* Version: 1.0\r\n");
//                 strCode.Add( "* Author: auto generator\r\n");
//                 strCode.Add( "*\r\n");
//                 strCode.Add( "* This file is part of the game.\r\n");
//                 strCode.Add( "*/\r\n");
//                 strCode.Add( "#ifndef __"+ evType.Name.ToUpper() + "_H__\r\n");
//                 strCode.Add( "#define __" + evType.Name.ToUpper() + "_H__\r\n");
//                 strCode.Add("#include \"Core/Event/BaseEventParam.h\"\r\n");
//                 for(int j =0; j < evType.vCppIncludes.Count;++j)
//                     strCode.Add("#include \""+ evType.vCppIncludes[j]+ "\"\r\n");
// 
//                 strCode.Add( "namespace "+ nameSpace + "\r\n");
//                 strCode.Add( "{\r\n");
//                 strCode.Add( "\tstruct " + evType.Name + ": public ABaseEventParam\r\n");
//                 strCode.Add( "\t{\r\n");
//                 string strReadString = "";
//                 string strWriteString = "";
// 
//                 strReadString += "\tbool "+ evType.Name + "::InnerRead(Serializer* seralizer, tByte version)\r\n";
//                 strReadString += "\t{\r\n";
// 
//                 strWriteString += "\tvoid "+ evType.Name + "::InnerWrite(Serializer* seralizer, tByte version)\r\n";
//                 strWriteString += "\t{\r\n";
// 
//                 string strCopy = "";
//                 string strConstruction="";
//                 string strDestruction="";
//                 if (!ActionEventCoreGeneratorSerializerField.BuildFieldCpp(evType, ref strConstruction, ref strDestruction, ref strCopy, ref strCode, ref strReadString, ref strWriteString))
//                 {
//                     bOk = false;
//                     break;
//                 }
// 
//                 strReadString += "\t\treturn true;\r\n";
//                 strReadString += "\t}\r\n";
//                 strWriteString += "\t}\r\n";
// 
//                 strCode.Add("\r\n");
//                 strCode.Add("\t\t" + evType.Name + "();\r\n");
//                 strCode.Add("\t\t~" + evType.Name + "();\r\n");
//                 strCode.Add( "\t\tstatic tInt16 StaticType();\r\n");
//                 strCppCode.Add("\ttInt16  " + evType.Name + "::StaticType() { return (tInt16)EEventType_" + evType.attri.eType + "; }\r\n");
// 
// 
//                 strCode.Add( "\t\tvirtual void Copy(ABaseEventParam* evtParam);\r\n");
// 
//                 strCppCode.Add("\t" + evType.Name + "::"+ evType.Name + "()\r\n");
//                 strCppCode.Add("\t\t:ABaseEventParam()\r\n");
//                 strCppCode.Add("\t{\r\n");
//                 strCppCode.Add(strConstruction);
//                 strCppCode.Add("\t}\r\n");
// 
//                 strCppCode.Add("\t" + evType.Name + "::~" + evType.Name + "()\r\n");
//                 strCppCode.Add("\t{\r\n");
//                 strCppCode.Add(strDestruction);
//                 strCppCode.Add("\t}\r\n");
// 
//                 strCppCode.Add("\tvoid " + evType.Name + "::Copy(ABaseEventParam* evtParam)\r\n");
//                 strCppCode.Add("\t{\r\n");
//                 strCppCode.Add("\t\tABaseEventParam::Copy(evtParam);\r\n");
//                 strCppCode.Add("\t\t" + evType.Name + "* oth = CastEvent<" + evType.Name + ">(evtParam);\r\n");
//                 strCppCode.Add("\t\tif (oth == NULL) return;\r\n");
//                 strCppCode.Add(strCopy);
//                 strCppCode.Add("\t}\r\n");
// 
//                 strCode.Add( "\tprotected:\r\n");
//                 strCode.Add("\t\tvirtual bool InnerRead(Serializer* seralizer, tByte version);\r\n");
//                 strCode.Add("\t\tvirtual void InnerWrite(Serializer* seralizer, tByte version);\r\n");
// 
//                 strCppCode.Add(strReadString);
//                 strCppCode.Add(strWriteString);
// 
//                 strCode.Add( "\t};\r\n");
//                 strCode.Add( "}\r\n");
//                 strCode.Add("#endif\r\n");
// 
//                 strCppCode.Add("}\r\n");
// 
//                 for (int j = 0; j < strCode.Count; ++j)
//                     evType.Code += strCode[j];
//                 for (int j = 0; j < strCppCode.Count; ++j)
//                     evType.CppCode += strCppCode[j];
// 
//                 vTypes[i] = evType;
//             }
// 
//             if (!bOk)
//             {
//                 EditorUtility.DisplayDialog("提示", "序列化错误，请查看控制台输出", "好的");
//                 return;
//             }
// 
//             strDir = strDir.Replace("\\", "/");
//             if (Directory.Exists(strDir))
//                 Directory.Delete(strDir, true);
//                 
//             Directory.CreateDirectory(strDir);
// 
//             if (!strDir.EndsWith("/")) strDir += "/";
//             {
//                 string strPath = strDir + "EEventType.h";
//                 FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
//                 StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
//                 writer.BaseStream.Position = 0;
//                 writer.BaseStream.SetLength(0);
//                 writer.BaseStream.Flush();
//                 writer.Write(strEventTypeCode);
//                 writer.Close();
//             }
//             for (int i = 0; i < vTypes.Count; ++i)
//             {
//                 EventType evType = vTypes[i];
//                 {
//                     string strPath = strDir + evType.Name + ".h";
//                     FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
//                     StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
//                     writer.BaseStream.Position = 0;
//                     writer.BaseStream.SetLength(0);
//                     writer.BaseStream.Flush();
//                     writer.Write(evType.Code);
//                     writer.Close();
//                 }
//                 if(!string.IsNullOrEmpty(evType.CppCode) )
//                 {
//                     string strPath = strDir + evType.Name + ".cpp";
//                     FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
//                     StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
//                     writer.BaseStream.Position = 0;
//                     writer.BaseStream.SetLength(0);
//                     writer.BaseStream.Flush();
//                     writer.Write(evType.CppCode);
//                     writer.Close();
//                 }
//             }
// 
//             {
//                 //new .h
//                 {
//                     string strNewEvenTypeCode = "";
//                     strNewEvenTypeCode += "/* Copyright (C) 2019-2019, happli. All rights reserved.\r\n";
//                     strNewEvenTypeCode += "*\r\n";
//                     strNewEvenTypeCode += "* File:    EEventType.h\r\n";
//                     strNewEvenTypeCode += "* Desc:    EEventType\r\n";
//                     strNewEvenTypeCode += "* Version: 1.0\r\n";
//                     strNewEvenTypeCode += "* Author: auto generator\r\n";
//                     strNewEvenTypeCode += "*\r\n";
//                     strNewEvenTypeCode += "* This file is part of the game.\r\n";
//                     strNewEvenTypeCode += "*/\r\n";
//                     strNewEvenTypeCode += "#pragma once\r\n";
//                     strNewEvenTypeCode += "namespace " + nameSpace + "\r\n";
//                     strNewEvenTypeCode += "{\r\n";
//                     strNewEvenTypeCode += "\tstruct ABaseEventParam;\r\n";
//                     strNewEvenTypeCode += "\textern ABaseEventParam* CreateEventParamFromType(short type);\r\n";
//                     strNewEvenTypeCode += "}\r\n";
// 
//                     string strPath = strDir + "EventUtil.h";
//                     FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
//                     StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
//                     writer.BaseStream.Position = 0;
//                     writer.BaseStream.SetLength(0);
//                     writer.BaseStream.Flush();
//                     writer.Write(strNewEvenTypeCode);
//                     writer.Close();
//                 }
//                 //new cpp
//                 {
//                     string strNewEvenTypeCode = "";
//                     strNewEvenTypeCode += "#include \"stdafx.h\"\r\n";
//                     strNewEvenTypeCode += ("#include \"" + includeDir + "EventUtil.h\"\r\n");
//                     strNewEvenTypeCode += ("#include \"Core/Event/BaseEventParam.h\"\r\n");
//                     strNewEvenTypeCode += ("#include \"" + includeDir + "EEventType.h\"\r\n");
//                     for (int i = 0; i < vTypes.Count; ++i)
//                     {
//                         strNewEvenTypeCode += "#include \"" + includeDir + vTypes[i].Name + ".h\"\r\n";
//                     }
//                     strNewEvenTypeCode += "namespace " + nameSpace + "\r\n";
//                     strNewEvenTypeCode += "{\r\n";
//                     strNewEvenTypeCode += "\tABaseEventParam* CreateEventParamFromType(short type)\r\n";
//                     strNewEvenTypeCode += "\t{\r\n";
//                     strNewEvenTypeCode += "\t\tswitch(type)\r\n";
//                     strNewEvenTypeCode += "\t\t{\r\n";
//                     for (int i = 0; i < vTypes.Count; ++i)
//                     {
//                         strNewEvenTypeCode += "\t\t\tcase (short)EEventType_" + vTypes[i].attri.eType + ": return new " + vTypes[i].Name + "();\r\n";
//                     }
//                     strNewEvenTypeCode += "\t\t\tdefault: LogError(\"undefine event type[%d]\", type);\r\n";
//                     strNewEvenTypeCode += "\t\t}\r\n";
//                     strNewEvenTypeCode += "\t\treturn NULL;\r\n";
//                     strNewEvenTypeCode += "\t}\r\n";
// 
//                     strNewEvenTypeCode += "}\r\n";
// 
//                     string strPath = strDir + "EventUtil.cpp";
//                     FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
//                     StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
//                     writer.BaseStream.Position = 0;
//                     writer.BaseStream.SetLength(0);
//                     writer.BaseStream.Flush();
//                     writer.Write(strNewEvenTypeCode);
//                     writer.Close();
//                 }
//             }
//         }
//        //------------------------------------------------------
//         static void BuildBinaryCsCode(string strDir, List<EventType> vTypes, string nameSpace = "TopPlus")
//         {
//             strDir = strDir.Replace("\\", "/");
//             if (Directory.Exists(strDir))
//                 Directory.Delete(strDir, true);
// 
//             Directory.CreateDirectory(strDir);
// 
//             for (int i =0; i < vTypes.Count; ++i)
//             {
//                 EventType evtType = vTypes[i];
//                 string strReadCode = "";
//                 string strWriteCode = "";
//                 string strCopy = "";
//                 List<string> strCode = new List<string>();
// 
//                 strCode.Add("//auto generator\r\n");
//                 strCode.Add("namespace " + nameSpace + "\r\n");
//                 strCode.Add("{\r\n");
//                 strCode.Add("\tpublic partial class " + evtType.Name + "\r\n");
//                 strCode.Add("\t{\r\n");
//                 strCode.Add("\t\tpublic override bool Read(ref TopGame.Data.BinaryUtil reader)\r\n");
//                 strCode.Add("\t\t{\r\n");
//                 strCode.Add("\t\t\tbase.Read(ref reader);\r\n");
//                 if (!ActionEventCoreGeneratorSerializerField.BuildBinaryFieldCs(evtType, ref strCopy, ref strCode, ref strReadCode, ref strWriteCode))
//                 {
//                     continue;
//                 }
// 
// 
//                 strCode.Add(strReadCode);
//                 strCode.Add("\t\t\treturn true;\r\n");
//                 strCode.Add("\t\t}\r\n");
//                 strCode.Add("\t\t#if UNITY_EDITOR\r\n");
//                 strCode.Add("\t\tpublic override void Write(ref TopGame.Data.BinaryUtil writer)\r\n");
//                 strCode.Add("\t\t{\r\n");
//                 strCode.Add("\t\t\tbase.Write(ref writer);\r\n");
//                 strCode.Add(strWriteCode);
//                 strCode.Add("\t\t}\r\n");
//                 strCode.Add("\t\t#endif\r\n");
//                 strCode.Add("\t}\r\n");
//                 strCode.Add("}\r\n");
// 
//                 string strFile = strDir + evtType.Name + ".cs";
//                 FileStream fs = new FileStream(strFile, FileMode.OpenOrCreate);
//                 StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
//                 writer.BaseStream.Position = 0;
//                 writer.BaseStream.SetLength(0);
//                 writer.BaseStream.Flush();
//                 for (int j = 0; j < strCode.Count; ++j)
//                     writer.Write(strCode[j]);
//                 writer.Close();
//             }
// 
//         }
//         //------------------------------------------------------
//         static void BuildEventSerializedCode(string strDir, List<EventType> vTypes)
//         {
//             bool bOk = true;
//             for (int i = 0; i < vTypes.Count; ++i)
//             {
//                 EventType evType = vTypes[i];
//                 string strCode = "";
// 
//                 strCode += "//auto generator\r\n";
//                 strCode += "using System;\r\n";
//                 strCode += "using UnityEngine;\r\n";
//                 strCode += "using System.IO;\r\n";
//                 strCode += "using TopGame.Data;\r\n";
//                 strCode += "using TopGame.Core;\r\n";
//                 strCode += "namespace TopGame.Core\r\n";
//                 strCode += "{\r\n";
//                 strCode += "\tpublic partial class " + evType.Name + "\r\n";
//                 strCode += "\t{\r\n";
//                 string strReadString = "";
//                 string strWriteString = "";
//                 string strReadXml = "";
//                 string strWriteXml = "";
//                 string strReadBinary = "";
//                 string strWriteBinary = "";
// 
//                 strReadString += "\t\tpublic override bool ReadCmd(string strParam)\r\n";
//                 strReadString += "\t\t{\r\n";
//                 strReadString += "\t\t\tif(!base.ReadCmd(strParam)) return false;\r\n";
// 
//                 strWriteString += "\t\tpublic override string WriteCmd()\r\n";
//                 strWriteString += "\t\t{\r\n";
//                 strWriteString += "\t\t\tstring strParam = base.WriteCmd() + \",\";\r\n";
// 
//                 if (!ActionEventCoreGeneratorSerializerField.BuildFieldSerializer(evType, "", ref strReadString, ref strWriteString, ref strReadXml, ref strWriteXml, ref strReadBinary, ref strWriteBinary))
//                 {
//                     bOk = false;
//                     break;
//                 }
// 
//                 strReadString += "\t\t}\r\n";
//                 strWriteString += "\t\t}\r\n";
// 
//                 strCode += strReadString;
//                 strCode += strWriteString;
// 
//                 strCode += "\t\t}\r\n";
//                 strCode += "\t}\r\n";
//                 strCode += "}\r\n";
// 
//                 evType.Code = strCode;
//                 vTypes[i] = evType;
//             }
// 
//             if (!bOk)
//             {
//                 EditorUtility.DisplayDialog("提示", "序列化错误，请查看控制台输出", "好的");
//                 return;
//             }
// 
//             if (!Directory.Exists(strDir))
//                 Directory.CreateDirectory(strDir);
// 
//             for (int i = 0; i < vTypes.Count; ++i)
//             {
//                 EventType evType = vTypes[i];
//                 string strPath = strDir + evType.Name + "_Serializer.cs";
//                 FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
//                 StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
//                 writer.BaseStream.Position = 0;
//                 writer.BaseStream.SetLength(0);
//                 writer.BaseStream.Flush();
//                 writer.Write(evType.Code);
//                 writer.Close();
//             }
//         }
//         //------------------------------------------------------
//         static void BuildDrawCode(string strPath, List<EventType> vTypes)
//         {
//             if (File.Exists(strPath))
//             {
//                 File.Delete(strPath);
//             }
//             string code = "#if UNITY_EDITOR\r\n";
//             code += "//auto generator\r\n";
//             code += "using System;\r\n";
//             code += "using System.Collections.Generic;\r\n";
//             code += "using UnityEditor;\r\n";
//             code += "using UnityEngine;\r\n";
//             code += "using System.IO;\r\n";
//             code += "using TopGame.Data;\r\n";
//             code += "using TopGame.Core;\r\n";
//             code += "namespace TopGame.ED\r\n";
//             code += "{\r\n";
//             code += "\t[Framework.Plugin.PluginEventDraw]\r\n";
//             code += "\tpublic class DrawEventCore\r\n";
//             code += "\t{\r\n";
//             code += "\t\tstatic List<BaseEventParameter> ms_Events = new List<BaseEventParameter>();\r\n";
//             code += "\t\tstatic List<string> ms_IngoreNoActionFiled = new List<string>();\r\n";
//             code += "\t\tpublic static Dictionary<EEventType, BaseEventParameter> vCopyParameters = new Dictionary<EEventType, BaseEventParameter>();\r\n";
//             code += "\t\tpublic static List<string> GetNoActionIngoreField()\r\n";
//             code += "\t\t{\r\n";
//             code += "\t\t\tif(ms_IngoreNoActionFiled == null || ms_IngoreNoActionFiled.Count<=0)\r\n";
//             code += "\t\t\t{\r\n";
//             code += "\t\t\t\tms_IngoreNoActionFiled.Add(\"totalTriggertCount\");\r\n";
//             code += "\t\t\t\tms_IngoreNoActionFiled.Add(\"actionWithBit\");\r\n";
//             code += "\t\t\t\tms_IngoreNoActionFiled.Add(\"canTriggerAfterKilled\");\r\n";
//             code += "\t\t\t\tms_IngoreNoActionFiled.Add(\"triggetTime\");\r\n";
//             code += "\t\t\t}\r\n";
//             code += "\t\t\treturn ms_IngoreNoActionFiled;\r\n";
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic static void CheckCopyByClipBoard()\r\n";
//             code += "\t\t{\r\n";
//             code += "\t\t\tif (string.IsNullOrEmpty(GUIUtility.systemCopyBuffer)) return;\r\n";
//             code += "\t\t\t\tBaseEventParameter copyData = BaseEventParameter.NewEvent(GUIUtility.systemCopyBuffer);\r\n";
//             code += "\t\t\tif(copyData != null)\r\n";
//             code += "\t\t\t{\r\n";
//             code += "\t\t\t\tAddCopyEvent(copyData);\r\n";
//             code += "\t\t\t\tGUIUtility.systemCopyBuffer = null;\r\n";
//             code += "\t\t\t}\r\n";
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic static void AddCopyEvent(BaseEventParameter evt)\r\n";
//             code += "\t\t{\r\n";
//             code += "\t\t\tif(evt == null) return;\r\n";
//             code += "\t\t\t\tvCopyParameters[evt.GetEventType()] = evt;\r\n";
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic static BaseEventParameter GetCopyEvent(EEventType type)\r\n";
//             code += "\t\t{\r\n";
//             code += "\t\t\tBaseEventParameter evet;\r\n";
//             code += "\t\t\tif (vCopyParameters.TryGetValue(type, out evet))\r\n";
//             code += "\t\t\t\treturn evet;\r\n";
//             code += "\t\t\treturn null;\r\n";
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic static bool CopyEvent(BaseEventParameter evt, bool bRemove = true)\r\n";
//             code += "\t\t{\r\n";
//             code += "\t\t\tBaseEventParameter cpy;\r\n";
//             code += "\t\t\tif (vCopyParameters.TryGetValue(evt.GetEventType(), out cpy))\r\n";
//             code += "\t\t\t{\r\n";
//             code += "\t\t\t\tevt.Copy(cpy);\r\n";
//             code += "\t\t\t\tif (bRemove) vCopyParameters.Remove(evt.GetEventType());\r\n";
//             code += "\t\t\t\treturn true;\r\n";
//             code += "\t\t\t}\r\n";
//             code += "\t\t\treturn false;\r\n";
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic static bool CanCopyEvent(BaseEventParameter evt)\r\n";
//             code += "\t\t{\r\n";
//             code += "\t\t\tif (evt == null) return false;\r\n";
//             code += "\t\t\tBaseEventParameter cpy;\r\n";
//             code += "\t\t\tif (vCopyParameters.TryGetValue(evt.GetEventType(), out cpy) && evt != cpy)\r\n";
//             code += "\t\t\t\treturn true;\r\n";
//             code += "\t\t\treturn false;\r\n";
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic static BaseEventParameter DrawUnAction(BaseEventParameter evt, List<string> Slots = null)\r\n";
//             code += "\t\t{\r\n";
//             code += "\t\t\tif (Slots != null)\r\n";
//             code += "\t\t\t\tHandleUtilityWrapper.BindSlots = Slots;\r\n";
//             code += "\t\t\tevt = (BaseEventParameter)HandleUtilityWrapper.DrawProperty(evt, GetNoActionIngoreField());\r\n";
//             code += "\t\t\tHandleUtilityWrapper.BindSlots=null;\r\n";
//             code += "\t\t\treturn evt;\r\n";
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic static void Draw(ActionEventCore pCore, List<string> Slots = null, System.Action<BaseEventParameter, string> OnDrawEvent = null)\r\n";
//             code += "\t\t{\r\n";
//             code += "\t\t\tif(Slots != null)\r\n";
//             code += "\t\t\t\tHandleUtilityWrapper.BindSlots = Slots;\r\n";
//             code += "\t\t\tms_Events.Clear();\r\n";
//             for(int i = 0; i < vTypes.Count; ++i)
//             {
//                 string fieldName = vTypes[i].Name.Replace("EventParameter", "s").Replace("Parameter", "s");
//                 code += "\t\t\tif(pCore." + fieldName + " !=null)\r\n";
//                 code += "\t\t\t{\r\n";
//                 code += "\t\t\t\tfor(int i =0; i < pCore." + fieldName + ".Count; ++i) ms_Events.Add(pCore." + fieldName + "[i]);\r\n";
//                 code += "\t\t\t}\r\n";
//             }
//             code += "\t\t\tfor(int i=0; i < ms_Events.Count; ++i)\r\n";
//             code += "\t\t\t{\r\n";
//             code += "\t\t\t\tGUILayout.BeginHorizontal();\r\n";
//             code += "\t\t\t\tif (OnDrawEvent != null)\r\n";
//             code += "\t\t\t\t{\r\n";
//             code += "\t\t\t\t    OnDrawEvent(ms_Events[i], null);\r\n";
//             code += "\t\t\t\t }\r\n";
//             code += "\t\t\t\tif (ms_Events[i].OnEdit(true) && GUILayout.Button(\"编辑\", new GUILayoutOption[] { GUILayout.Width(30) }))\r\n";
//             code += "\t\t\t\t {\r\n";
//             code += "\t\t\t\t     ms_Events[i].OnEdit(false);\r\n";
//             code += "\t\t\t\t }\r\n";
//             code += "\t\t\t\tif (ms_Events[i].OnPreview(true) && GUILayout.Button(\"预览\", new GUILayoutOption[] { GUILayout.Width(30) }))\r\n";
//             code += "\t\t\t\t {\r\n";
//             code += "\t\t\t\t     ms_Events[i].OnPreview(false);\r\n";
//             code += "\t\t\t\t }\r\n";
// 
//             code += "\t\t\t\tif(GUILayout.Button(\"Del\", new GUILayoutOption[] { GUILayout.Width(30) }))\r\n";
//             code += "\t\t\t\t{\r\n";
//             code += "\t\t\t\t\tif (EditorUtility.DisplayDialog(\"提示\", \"确定删除 ? \", \"确定\", \"取消\"))\r\n";
//             code += "\t\t\t\t\t{\r\n";
//             code += "\t\t\t\t\t\tpCore.DelEvent(ms_Events[i]);\r\n";
//             code += "\t\t\t\t\t\tbreak;\r\n";
//             code += "\t\t\t\t\t}\r\n";
//             code += "\t\t\t\t}\r\n";
//             code += "\t\t\t\tif (GUILayout.Button(\"Copy\", new GUILayoutOption[] { GUILayout.Width(50) }))\r\n";
//             code += "\t\t\t\t{\r\n";
//             code += "\t\t\t\t    AddCopyEvent(ms_Events[i]);\r\n";
//             code += "\t\t\t\t}\r\n";
//             code += "\t\t\t\tif(CanCopyEvent(ms_Events[i]) && GUILayout.Button(\"Parse\", new GUILayoutOption[] { GUILayout.Width(50) }))\r\n";
//             code += "\t\t\t\t{\r\n";
//             code += "\t\t\t\t    CopyEvent(ms_Events[i]);\r\n";
//             code += "\t\t\t\t}\r\n";
// 
//             code += "\t\t\t\tms_Events[i].bExpand = EditorGUILayout.Foldout(ms_Events[i].bExpand, \"Event[\" + ms_Events[i].GetEventType() + \"]\");\r\n";
//             code += "\t\t\t\tGUILayout.EndHorizontal();\r\n";
//             code += "\t\t\t\tif(ms_Events[i].bExpand)\r\n";
//             code += "\t\t\t\t{\r\n";
//             code += "\t\t\t\t\tEditorGUI.indentLevel++;\r\n";
//             code += "\t\t\t\t\tms_Events[i]= (BaseEventParameter)HandleUtilityWrapper.DrawProperty(ms_Events[i], null, (string field) =>\r\n";
//             code += "\t\t\t\t\t{\r\n";
//             code += "\t\t\t\t\t if (OnDrawEvent != null)\r\n";
//             code += "\t\t\t\t\t   {\r\n";
//             code += "\t\t\t\t\t      OnDrawEvent(ms_Events[i], field);\r\n";
//             code += "\t\t\t\t\t   }\r\n";
//             code += "\t\t\t\t\t  });\r\n";
//             code += "\t\t\t\t\tEditorGUI.indentLevel--;\r\n";
//             code += "\t\t\t\t}\r\n";
//             code += "\t\t\t}\r\n";
//             code += "\t\t\tHandleUtilityWrapper.BindSlots = null;\r\n";
// 
//             code += "\t\t}\r\n";
//             code += "\t}\r\n";
//             code += "}\r\n";
//             code += "#endif\r\n";
//             FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
//             StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
//             writer.Write(code);
//             writer.Close();
//         }
//         //------------------------------------------------------
//         static void BuildCode(string strPath, List<EventType> vTypes, string strNameSpace)
//         {
//             if (File.Exists(strPath))
//             {
//                 File.Delete(strPath);
//             }
//             string code = "//auto generator\r\n";
//             code += "using UnityEngine;\r\n";
//             code += "using System.Collections.Generic;\r\n";
//             code += "namespace " + strNameSpace + "\r\n";
//             code += "{\r\n";
//             code += "\tpublic interface IBaseEvent\r\n";
//             code += "\t{\r\n";
//             code += "\t\tvoid Reset(BaseEventParameter parameter);\r\n";
//             code += "\t}\r\n";
//             code += "\t[System.Serializable]\r\n";
//             code += "\t[Framework.Plugin.PluginEventCore]\r\n";
//             code += "\tpublic struct ActionEventCore\r\n";
//             code += "\t{\r\n";
//             for (int i = 0; i < vTypes.Count; ++i)
//                 code += "\t\tpublic List<" + vTypes[i].FullName + "> " + vTypes[i].Name.Replace("EventParameter", "s").Replace("Parameter", "s") + ";\r\n";
// 
//             code += "\t\tpublic int BuildActionEvent(ref List<LogActionEvent> vEvent)\r\n";
//             code += "\t\t{\r\n";
//             for (int i = 0; i < vTypes.Count; ++i)
//             {
//                 string fieldName = vTypes[i].Name.Replace("EventParameter", "s").Replace("Parameter", "s");
//                 code += "\t\t\tif(" + fieldName + "!=null)\r\n";
//                 code += "\t\t\t{\r\n";
//                 code += "\t\t\t\tfor(int i =0; i < " + fieldName + ".Count; ++i)\r\n";
//                 code += "\t\t\t\t{\r\n";
//                 code += "\t\t\t\t\tActionEvent newT = ActionNewPool.MallocActionEvent(); newT.Reset(" + fieldName + "[i]);\r\n";
//                 code += "\t\t\t\t\tvEvent.Add(newT);\r\n";
//                 code += "\t\t\t\t}\r\n";
//                 code += "\t\t\t}\r\n";
//             }
//             code += "\t\t\treturn vEvent.Count;\r\n";
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic int BuildEvent<T>(ref List<T> vEvent) where T : IBaseEvent, new()\r\n";
//             code += "\t\t{\r\n";
//             code += "\t\t\tint index = 0;\r\n";
//             code += "\t\t\tint preCnt = vEvent.Count;\r\n";
//             for (int i = 0; i < vTypes.Count; ++i)
//             {
//                 string fieldName = vTypes[i].Name.Replace("EventParameter", "s").Replace("Parameter", "s");
//                 code += "\t\t\tif(" + fieldName + "!=null)\r\n";
//                 code += "\t\t\t{\r\n";
//                 code += "\t\t\t\tfor(int i =0; i < " + fieldName + ".Count; ++i)\r\n";
//                 code += "\t\t\t\t{\r\n";
//                 code += "\t\t\t\t\tif(index < preCnt)\r\n";
//                 code += "\t\t\t\t\t{\r\n";
//                 code += "\t\t\t\t\t\tvEvent[index].Reset(" + fieldName + "[i]);\r\n";
//                 code += "\t\t\t\t\t\t++index;\r\n";
//                 code += "\t\t\t\t\t}\r\n";
//                 code += "\t\t\t\t\telse\r\n";
//                 code += "\t\t\t\t\t{\r\n";
//                 code += "\t\t\t\t\t\tT newT = new T(); newT.Reset(" + fieldName + "[i]);\r\n";
//                 code += "\t\t\t\t\t\tvEvent.Add(newT);\r\n";
//                 code += "\t\t\t\t\t}\r\n";
//                 code += "\t\t\t\t}\r\n";
//                 code += "\t\t\t}\r\n";
//             }
//             code += "\t\t\treturn index;\r\n";
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic void BuildEvent(List<BaseEventParameter> vEvent)\r\n";
//             code += "\t\t{\r\n";
//             for (int i = 0; i < vTypes.Count; ++i)
//             {
//                 string fieldName = vTypes[i].Name.Replace("EventParameter", "s").Replace("Parameter", "s");
//                 code += "\t\t\tif(" + fieldName + "!=null)\r\n";
//                 code += "\t\t\t{\r\n";
//                 code += "\t\t\t\tfor(int i =0; i < " + fieldName + ".Count; ++i)\r\n";
//                 code += "\t\t\t\t{\r\n";
//                 code += "\t\t\t\t\tvEvent.Add(" + fieldName + "[i]);\r\n";
//                 code += "\t\t\t\t}\r\n";
//                 code += "\t\t\t}\r\n";
//             }
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic void AddEvent(BaseEventParameter param)\r\n";
//             code += "\t\t{\r\n";
//             for(int i = 0; i < vTypes.Count; ++i)
//             {
//                 string fieldName = vTypes[i].Name.Replace("EventParameter", "s").Replace("Parameter", "s");
//                 code += "\t\t\tif(param is " + vTypes[i].FullName + ")\r\n";
//                 code += "\t\t\t{\r\n";
//                 code += "\t\t\t\tif(" + fieldName + "==null) " + fieldName + "= new List<" + vTypes[i].FullName + ">();\r\n";
//                 code += "\t\t\t\t" + fieldName + ".Add(param as " + vTypes[i].FullName + ");\r\n";
//                 code += "\t\t\t\treturn;\r\n";
//                 code += "\t\t\t}\r\n";
//             }
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic void DelEvent(BaseEventParameter param)\r\n";
//             code += "\t\t{\r\n";
//             for (int i = 0; i < vTypes.Count; ++i)
//             {
//                 string fieldName = vTypes[i].Name.Replace("EventParameter", "s").Replace("Parameter", "s");
//                 code += "\t\t\tif(param is " + vTypes[i].FullName + ")\r\n";
//                 code += "\t\t\t{\r\n";
//                 code += "\t\t\t\tif(" + fieldName + "!=null) " + fieldName + ".Remove(param as " + vTypes[i].FullName + ");\r\n";
//                 code += "\t\t\t\treturn;\r\n";
//                 code += "\t\t\t}\r\n";
//             }
//             code += "\t\t}\r\n";
// 
//             code += "\t\tprivate static Dictionary<EEventType, Stack<BaseEventParameter>> gs_Pools = new Dictionary<EEventType, Stack<BaseEventParameter>>();\r\n";
// 
//             code += "\t\tpublic static void ReleaseEvent(BaseEventParameter eventParam)\r\n";
//             code += "\t\t{\r\n";
//             code += "\t\t\tif(eventParam == null ) return;\r\n";
//             code += "\t\t\tEEventType evtType = eventParam.GetEventType();\r\n";
//             code += "\t\t\tif (evtType == EEventType.Base || evtType == EEventType.Count) return;\r\n";
//             code += "\t\t\tStack<BaseEventParameter> stacks;\r\n";
//             code += "\t\t\tif(!gs_Pools.TryGetValue(evtType, out stacks))\r\n";
//             code += "\t\t\t{\r\n";
//             code += "\t\t\t\tstacks = new Stack<BaseEventParameter>(2);\r\n";
//             code += "\t\t\t\tgs_Pools.Add(evtType, stacks);\r\n";
//             code += "\t\t\t}\r\n";
//             code += "\t\t\tif(stacks.Count >= 2) return;\r\n";
//             code += "\t\t\tstacks.Push(eventParam);\r\n";
//             code += "\t\t}\r\n";
// 
//             code += "\t\tpublic static BaseEventParameter NewEvent(EEventType type)\r\n";
//             code += "\t\t{\r\n";
//             code += "\t\t\tStack<BaseEventParameter> stacks;\r\n";
//             code += "\t\t\tif(gs_Pools.TryGetValue(type, out stacks) && stacks != null && stacks.Count > 0) return stacks.Pop();\r\n";
//             for (int i = 0; i < vTypes.Count; ++i)
//             {
//                 EventDeclarationAttribute dec = vTypes[i].attri;
//                 if (dec == null) continue;
//                 code += "\t\t\tif(type == EEventType." + dec.eType + ") ";
//                 code += "return new " + vTypes[i].FullName + "();\r\n";
//             }
//             code += "\t\t\treturn null;\r\n";
//             code += "\t\t}\r\n";
// 
// 
//             code += "\t\tpublic void Init()\r\n";
//             code += "\t\t{\r\n";
//             for (int i = 0; i < vTypes.Count; ++i)
//             {
//                 string fieldName = vTypes[i].Name.Replace("EventParameter", "s").Replace("Parameter", "s");
//                 code += "\t\t\tif(" + fieldName + "!=null)\r\n";
//                 code += "\t\t\t{\r\n";
//                 code += "\t\t\t\tfor(int i =0; i < " + fieldName + ".Count; ++i)\r\n";
//                 code += "\t\t\t\t{\r\n";
//                 code += "\t\t\t\t\tif("+ fieldName + "[i]!=null) " + fieldName + "[i].FillParams();\r\n";
//                 code += "\t\t\t\t}\r\n";
//                 code += "\t\t\t}\r\n";
//             }
//             code += "\t\t}\r\n";
// 
//             code += "\t}\r\n";
//             code += "}\r\n";
// 
//             FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
//             StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
//             writer.Write(code);
//             writer.Close();
//         }
//     }
// }
// #endif