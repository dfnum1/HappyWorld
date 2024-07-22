// #if UNITY_EDITOR
// /********************************************************************
// 生成日期:	1:11:2020 10:06
// 类    名: 	ActionEventCoreGeneratorSerializerHashSet
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
// using Framework.Core;
// 
// namespace TopGame.Core
// {
//     public class ActionEventCoreGeneratorSerializerHashSet
//     {
//         //------------------------------------------------------
//         public static bool BuildBinaryHashSetCs(FieldInfo field, System.Type fieldType, string strDecName, ref string strRead, ref string strWrite)
//         {
//             if (fieldType == null) return false;
//             if (string.IsNullOrEmpty(strDecName)) strDecName = field.Name;
// 
//             strRead += "\t\t\t{\r\n";
//             strRead += "\t\t\t\tint cnt = (int)reader.ToUshort();\r\n";
//             strRead += "\t\t\t\tif(cnt>0)\r\n";
//             strRead += "\t\t\t\t{\r\n";
//             strRead += "\t\t\t\t\t" + field.Name + "= new System.Collections.Generic.HashSet<" + strDecName + ">();\r\n";
//             strRead += "\t\t\t\t\tfor(int i =0; i < cnt; ++i)\r\n";
//             strRead += "\t\t\t\t\t{\r\n";
// 
//             strWrite += "\t\t\t{\r\n";
//             strWrite += "\t\t\t\twriter.WriteUshort((ushort)" + field.Name + ".Count);\r\n";
//             strWrite += "\t\t\t\tif (" + field.Name + ".Count > 0)\r\n";
//             strWrite += "\t\t\t\t{\r\n";
//             strWrite += "\t\t\t\t\tforeach(var db in " + field.Name + ")\r\n";
// 
//             string strReadString = "";
//             string strWriteString = "";
//             if (fieldType == typeof(bool))
//             {
//                 strReadString += field.Name + ".Add(reader.ToBool());\r\n";
//                 strWriteString += "writer.WriteBool(db);\r\n";
//             }
//             else if (fieldType == typeof(byte))
//             {
//                 strReadString += field.Name + ".Add(reader.ToByte());\r\n";
//                 strWriteString += "writer.WriteByte(db);\r\n";
//             }
//             else if (fieldType == typeof(short))
//             {
//                 strReadString += field.Name + ".Add(reader.ToShort());\r\n";
//                 strWriteString += "writer.WriteShort(db);\r\n";
//             }
//             else if (fieldType == typeof(ushort))
//             {
//                 strReadString += field.Name + ".Add(reader.ToUshort());\r\n";
//                 strWriteString += "writer.WriteUshort(db);\r\n";
//             }
//             else if (fieldType == typeof(int))
//             {
//                 strReadString += field.Name + ".Add(reader.ToInt32());\r\n";
//                 strWriteString += "writer.WriteInt32(db);\r\n";
//             }
//             else if (fieldType == typeof(uint))
//             {
//                 strReadString += field.Name + ".Add(reader.ToUint32());\r\n";
//                 strWriteString += "writer.WriteUint32(db);\r\n";
//             }
//             else if (fieldType == typeof(float))
//             {
//                 strReadString += field.Name + ".Add(reader.ToFloat());\r\n";
//                 strWriteString += "writer.WriteFloat(db);\r\n";
//             }
//             else if (fieldType == typeof(double))
//             {
//                 strReadString += field.Name + ".Add(reader.ToDouble());\r\n";
//                 strWriteString += "writer.WriteDouble(db);\r\n";
//             }
//             else if (fieldType == typeof(long))
//             {
//                 strReadString += field.Name + ".Add(reader.ToInt64());\r\n";
//                 strWriteString += "writer.WriteInt64(db);\r\n";
//             }
//             else if (fieldType == typeof(ulong))
//             {
//                 strReadString += field.Name + ".Add(reader.ToUint64());\r\n";
//                 strWriteString += "writer.WriteUint64(val);\r\n";
//             }
//             else if (fieldType.IsEnum)
//             {
//                 string enumTypeName = fieldType.FullName.Replace("+", ".");
// 
//                 strReadString += field.Name + ".Add((" + enumTypeName + ")reader.ToShort());\r\n";
//                 strWriteString += "writer.WriteShort((short)db);\r\n";
//             }
//             else if (fieldType == typeof(string))
//             {
//                 strReadString += field.Name + ".Add(reader.ToString());\r\n";
//                 strWriteString += "writer.WriteString(db);\r\n";
//             }
//             else if (fieldType == typeof(Vector2))
//             {
//                 strReadString += field.Name + ".Add(reader.ToVec2());\r\n";
//                 strWriteString += "writer.WriteVector2(db);\r\n";
//             }
//             else if (fieldType == typeof(Vector2Int))
//             {
//                 strReadString += field.Name + ".Add(reader.ToVec2Int());\r\n";
//                 strWriteString += "writer.WriteVector2Int(db);\r\n";
//             }
//             else if (fieldType == typeof(Vector3))
//             {
//                 strReadString += field.Name + ".Add(reader.ToVec3());\r\n";
//                 strWriteString += "writer.WriteVector3(db);\r\n";
//             }
//             else if (fieldType == typeof(Vector3Int))
//             {
//                 strReadString += field.Name + ".Add(reader.ToVec3Int());\r\n";
//                 strWriteString += "writer.WriteVector3Int(db);\r\n";
//             }
//             else if (fieldType == typeof(Vector4))
//             {
//                 strReadString += field.Name + ".Add(reader.ToVec4());\r\n";
//                 strWriteString += "writer.WriteVector4(db);\r\n";
//             }
//             else if (fieldType == typeof(Color))
//             {
//                 strReadString += field.Name + ".Add(reader.ToColor());\r\n";
//                 strWriteString += "writer.WriteColor(db);\r\n";
//             }
//             else if (fieldType == typeof(Quaternion))
//             {
//                 strReadString += field.Name + ".Add(reader.ToQuaternion());\r\n";
//                 strWriteString += "writer.WriteQuaternion(db);\r\n";
//             }
//             else if (fieldType == typeof(CurveData))
//             {
//                 strReadString += "{CameraSpline.CurveData temp = new CameraSpline.CurveData(); temp.Read(ref reader); " + field.Name + ".Add(temp);}\r\n";
//                 strWriteString += "db.Write(ref writer);\r\n";
//             }
//             else if (fieldType == typeof(AnimationCurve))
//             {
//                 strReadString += field.Name + ".Add(reader.ToCurve());\r\n";
//                 strWriteString += "writer.WriteCurve(db);\r\n";
//             }
//             else if (fieldType == typeof(ActionStatePropertyData))
//             {
//                 strReadString += "{ActionStatePropertyData temp = new ActionStatePropertyData(); temp.Read(ref reader); " + field.Name + ".Add(temp);}\r\n";
//                 strWriteString += "db.Write(ref writer);\r\n";
//             }
//             else if (fieldType == typeof(SpawnSplineData))
//             {
//                 strRead += "{TopGame.Logic.SpawnSplineData temp = new TopGame.Logic.SpawnSplineData(); temp.Read(ref reader); " + field.Name + ".Add(temp);}\r\n";
//                 strWriteString += "db.Write(ref writer);\r\n";
//             }
//             else
//             {
//                 Debug.LogError(fieldType.Name + " 类成员：" + fieldType.Name + " 不支持序列化");
//                 return false;
//             }
// 
//             strRead += "\t\t\t\t\t\t" + strReadString + "\r\n";
//             strRead += "\t\t\t\t\t}\r\n";
//             strRead += "\t\t\t\t}\r\n";
//             strRead += "\t\t\t}\r\n";
// 
// 
//             strWrite += "\t\t\t\t\t" + strWriteString + "\r\n";
//             strWrite += "\t\t\t\t}\r\n";
//             strWrite += "\t\t\t}\r\n";
// 
//             return true;
//         }
//         //------------------------------------------------------
//         public static bool BuildHashSetCpp(FieldInfo field, System.Type type, string strDecName, ref string strRead, ref string strWrite)
//         {
//             if (type == null) return false;
//             if (string.IsNullOrEmpty(strDecName)) strDecName = field.Name;
// 
//             strRead += "\t\t{\r\n";
//             strRead += "\t\t\ttUint16 cnt = seralizer->readUShort();\r\n";
//             strRead += "\t\t\tif(cnt>0)\r\n";
//             strRead += "\t\t\t{\r\n";
//             strRead += "\t\t\t\t" + field.Name + ".resize(cnt);\r\n";
//             strRead += "\t\t\t\tsize_t dataLen = cnt*sizeof(" + strDecName + ");\r\n";
//             strRead += "\t\t\t\tmemcpy(" + field.Name + ".get(), seralizer->getCurPtr(), dataLen);\r\n";
//             strRead += "\t\t\t\tseralizer->seek((long)dataLen, SEEK_CUR);\r\n";
//             strRead += "\t\t\t}\r\n";
//             strRead += "\t\t}\r\n";
// 
//             strWrite += "\t\t{\r\n";
//             strWrite += "\t\t\tseralizer->writeUShort((tUint16)" + field.Name + ".size());\r\n";
//             strWrite += "\t\t\tif (" + field.Name + ".size() > 0)\r\n";
//             strWrite += "\t\t\t{\r\n";
//             strWrite += "\t\t\t\tseralizer->writeUCharArray((const tUint8*)" + field.Name + ".get(), " + field.Name + ".size()*sizeof(" + strDecName + "));\r\n";
//             strWrite += "\t\t\t}\r\n";
//             strWrite += "\t\t}\r\n";
//             return true;
//         }
//         //------------------------------------------------------
//         public static bool BuildHashSetSerializer(FieldInfo field, System.Type type, string xmlEle, ref string strRead, ref string strWrite, ref string xmlRead, ref string xmlWrite, ref string binaryRead, ref string binaryWrite)
//         {
//             if (type == null) return false;
//             string typeFullName = type.FullName.Replace("+", ".");
//             if (type == typeof(bool))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(vals[a].CompareTo(\"1\") == 0);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "a?\"1\":\"0\";\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif("+ xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add(vals[a].CompareTo(\"1\") == 0);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "a?\"1\":\"0\";\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(reader.ReadBoolean());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else if (type == typeof(byte))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(byte.Parse(vals[a]));\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif(" + xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add(byte.Parse(vals[a]));\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(reader.ReadByte());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else if (type == typeof(short))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(short.Parse(vals[a]));\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif(" + xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add(short.Parse(vals[a]));\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(reader.ReadInt16());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else if (type == typeof(ushort))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(ushort.Parse(vals[a]));\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif(" + xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add(ushort.Parse(vals[a]));\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(reader.ReadUInt16());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else if (type == typeof(int))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(int.Parse(vals[a]));\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif(" + xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add(int.Parse(vals[a]));\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(reader.ReadInt32());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else if (type == typeof(uint))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(uint.Parse(vals[a]));\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif(" + xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add(uint.Parse(vals[a]));\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(reader.ReadUInt32());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else if (type == typeof(float))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(float.Parse(vals[a]));\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif(" + xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add(float.Parse(vals[a]));\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(reader.ReadSingle());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else if (type == typeof(double))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(double.Parse(vals[a]));\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif(" + xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add(double.Parse(vals[a]));\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(reader.ReadDouble());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else if (type == typeof(long))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(long.Parse(vals[a]));\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif(" + xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add(long.Parse(vals[a]));\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(reader.ReadInt64());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else if (type == typeof(ulong))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(ulong.Parse(vals[a]));\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif(" + xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add(ulong.Parse(vals[a]));\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(reader.ReadUInt64());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else if (type.IsEnum)
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(("+ typeFullName + ")int.Parse(vals[a]));\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif(" + xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add((" + typeFullName + ")int.Parse(vals[a]));\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += ((int)a);\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(("+typeFullName+")reader.ReadInt16());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write((short)a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else if (type == typeof(string))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + ".Add(vals[a]);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 strWrite += "\t\t\t\t}\r\n";
//                 strWrite += "\t\t\t\tstrParam += strRet + \",\";\r\n";
//                 strWrite += "\t\t\t}\r\n";
// 
//                 xmlRead += "\t\t\tif(" + xmlEle + ".HasAttribute(\"" + field.Name + "\")\r\n";
//                 xmlRead += "\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\tstring[] vals = " + xmlEle + ".GetAttribute(\"" + field.Name + "\").Split('@');\r\n";
//                 xmlRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 xmlRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 xmlRead += "\t\t\t\t{\r\n";
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + ".Add(vals[a]);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += a;\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t\tif(strRet.Length>0)strRet = strRet.Substring(0,strRet.Length-1) \r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new System.Collections.Generic.HashSet<" + typeFullName + ">();\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + ".Add(reader.ReadString());\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Count:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tforeach(var a in " + field.Name + ")\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(a);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else
//                 return false;
//             return true;
//         }
//         //------------------------------------------------------
//         public static bool bHashSet(Type paramType, out Type argType)
//         {
//             argType = null;
//             if (paramType == null) return false;
//             if (paramType.IsGenericType)
//             {
//                 if (paramType.Name.Contains("HashSet`1") && paramType.GenericTypeArguments != null && paramType.GenericTypeArguments.Length == 1)
//                 {
//                     argType = paramType.GenericTypeArguments[0];
//                     return true;
//                 }
//             }
//             return false;
//         }
//     }
// }
// #endif