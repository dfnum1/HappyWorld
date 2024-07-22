// #if UNITY_EDITOR
// /********************************************************************
// 生成日期:	1:11:2020 10:06
// 类    名: 	ActionEventCoreGeneratorSerializerArray
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
//     public class ActionEventCoreGeneratorSerializerArray
//     {
//         //------------------------------------------------------
//         public static bool BuildBinaryArrayCs(FieldInfo field, System.Type fieldType, string strDecName, ref string strRead, ref string strWrite)
//         {
//             if (fieldType == null) return false;
//             if (string.IsNullOrEmpty(strDecName)) strDecName = field.Name;
// 
//             strRead += "\t\t\t{\r\n";
//             strRead += "\t\t\t\tint cnt = (int)reader.ToUshort();\r\n";
//             strRead += "\t\t\t\tif(cnt>0)\r\n";
//             strRead += "\t\t\t\t{\r\n";
//             strRead += "\t\t\t\t\t" + field.Name + "= new " + strDecName + "[cnt];\r\n";
//             strRead += "\t\t\t\t\tfor(int i =0; i < cnt; ++i)\r\n";
//             strRead += "\t\t\t\t\t{\r\n";
// 
//             strWrite += "\t\t\t{\r\n";
//             strWrite += "\t\t\t\twriter.WriteUshort((ushort)" + field.Name + ".Length);\r\n";
//             strWrite += "\t\t\t\tif (" + field.Name + ".Length > 0)\r\n";
//             strWrite += "\t\t\t\t{\r\n";
//             strWrite += "\t\t\t\t\tfor(int i =0; i <" + field.Name + ".Length; ++i)\r\n";
// 
//             string strReadString = "";
//             string strWriteString = "";
//             if (fieldType == typeof(bool))
//             {
//                 strReadString += field.Name + "[i] =reader.ToBool();\r\n";
//                 strWriteString += "writer.WriteBool(db);\r\n";
//             }
//             else if (fieldType == typeof(byte))
//             {
//                 strReadString += field.Name + "[i] =reader.ToByte();\r\n";
//                 strWriteString += "writer.WriteByte(db);\r\n";
//             }
//             else if (fieldType == typeof(short))
//             {
//                 strReadString += field.Name + "[i] =reader.ToShort();\r\n";
//                 strWriteString += "writer.WriteShort(db);\r\n";
//             }
//             else if (fieldType == typeof(ushort))
//             {
//                 strReadString += field.Name + "[i] =reader.ToUshort();\r\n";
//                 strWriteString += "writer.WriteUshort(db);\r\n";
//             }
//             else if (fieldType == typeof(int))
//             {
//                 strReadString += field.Name + "[i] =reader.ToInt32();\r\n";
//                 strWriteString += "writer.WriteInt32(db);\r\n";
//             }
//             else if (fieldType == typeof(uint))
//             {
//                 strReadString += field.Name + "[i] =reader.ToUint32();\r\n";
//                 strWriteString += "writer.WriteUint32(db);\r\n";
//             }
//             else if (fieldType == typeof(float))
//             {
//                 strReadString += field.Name + "[i] =reader.ToFloat();\r\n";
//                 strWriteString += "writer.WriteFloat(db);\r\n";
//             }
//             else if (fieldType == typeof(double))
//             {
//                 strReadString += field.Name + "[i] =reader.ToDouble());\r\n";
//                 strWriteString += "writer.WriteDouble(db);\r\n";
//             }
//             else if (fieldType == typeof(long))
//             {
//                 strReadString += field.Name + "[i] =reader.ToInt64();\r\n";
//                 strWriteString += "writer.WriteInt64(db);\r\n";
//             }
//             else if (fieldType == typeof(ulong))
//             {
//                 strReadString += field.Name + "[i] =reader.ToUint64();\r\n";
//                 strWriteString += "writer.WriteUint64(val);\r\n";
//             }
//             else if (fieldType.IsEnum)
//             {
//                 string enumTypeName = fieldType.FullName.Replace("+", ".");
// 
//                 strReadString += field.Name + "[i] =(" + enumTypeName + ")reader.ToShort();\r\n";
//                 strWriteString += "writer.WriteShort((short)db);\r\n";
//             }
//             else if (fieldType == typeof(string))
//             {
//                 strReadString += field.Name + "[i] =reader.ToString();\r\n";
//                 strWriteString += "writer.WriteString(db);\r\n";
//             }
//             else if (fieldType == typeof(Vector2))
//             {
//                 strReadString += field.Name + "[i] =reader.ToVec2();\r\n";
//                 strWriteString += "writer.WriteVector2(db);\r\n";
//             }
//             else if (fieldType == typeof(Vector2Int))
//             {
//                 strReadString += field.Name + "[i] =reader.ToVec2Int();\r\n";
//                 strWriteString += "writer.WriteVector2Int(db);\r\n";
//             }
//             else if (fieldType == typeof(Vector3))
//             {
//                 strReadString += field.Name + "[i] =reader.ToVec3();\r\n";
//                 strWriteString += "writer.WriteVector3(db);\r\n";
//             }
//             else if (fieldType == typeof(Vector3Int))
//             {
//                 strReadString += field.Name + "[i] =reader.ToVec3Int();\r\n";
//                 strWriteString += "writer.WriteVector3Int(db);\r\n";
//             }
//             else if (fieldType == typeof(Vector4))
//             {
//                 strReadString += field.Name + "[i] =reader.ToVec4();\r\n";
//                 strWriteString += "writer.WriteVector4(db);\r\n";
//             }
//             else if (fieldType == typeof(Color))
//             {
//                 strReadString += field.Name + "[i] =reader.ToColor();\r\n";
//                 strWriteString += "writer.WriteColor(db);\r\n";
//             }
//             else if (fieldType == typeof(Quaternion))
//             {
//                 strReadString += field.Name + "[i] =reader.ToQuaternion();\r\n";
//                 strWriteString += "writer.WriteQuaternion(db);\r\n";
//             }
//             else if (fieldType == typeof(CameraSpline.CurveData))
//             {
//                 strReadString += "{CameraSpline.CurveData temp = new CameraSpline.CurveData(); temp.Read(ref reader); " + field.Name + "[i] =temp;}\r\n";
//                 strWriteString += "db.Write(ref writer);\r\n";
//             }
//             else if (fieldType == typeof(AnimationCurve))
//             {
//                 strReadString += field.Name + "[i] =reader.ToCurve();\r\n";
//                 strWriteString += "writer.WriteCurve(db);\r\n";
//             }
//             else if (fieldType == typeof(ActionStatePropertyData))
//             {
//                 strReadString += "{ActionStatePropertyData temp = new ActionStatePropertyData(); temp.Read(ref reader); " + field.Name + "[i] =temp;}\r\n";
//                 strWriteString += "db.Write(ref writer);\r\n";
//             }
//             else if (fieldType == typeof(TopGame.Logic.SpawnSplineData))
//             {
//                 strRead += "{TopGame.Logic.SpawnSplineData temp = new TopGame.Logic.SpawnSplineData(); temp.Read(ref reader); " + field.Name + "[i] =temp;}\r\n";
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
// 
//         //------------------------------------------------------
//         public static bool BuildArrayCpp(FieldInfo field, System.Type type, string strDecName, ref string strRead, ref string strWrite)
//         {
//             if (type == null) return false;
//             if(string.IsNullOrEmpty(strDecName)) strDecName =  field.Name;
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
//         public static bool BuildArraySerializer(FieldInfo field, System.Type type, string xmlEle, ref string strRead, ref string strWrite, ref string xmlRead, ref string xmlWrite, ref string binaryRead, ref string binaryWrite)
//         {
//             if (type == null) return false;
//             if (type == typeof(bool))
//             {
//                 strRead += "\t\t\t{\r\n";
//                 strRead += "\t\t\t\tif(ms_vData.Count<=0) return false;\r\n";
//                 strRead += "\t\t\t\tstring[] vals = ms_vData[0].Split('@');\r\n";
//                 strRead += "\t\t\t\tms_vData.RemoveAt(0);\r\n";
//                 strRead += "\t\t\t\tif(vals!=null && vals.Length>0)\r\n";
//                 strRead += "\t\t\t\t{\r\n";
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = vals[a].CompareTo(\"1\") == 0;\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a]?\"1\":\"0\";\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = vals[a].CompareTo(\"1\") == 0;\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a]?\"1\":\"0\";\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\""+ field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] = reader.ReadBoolean();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
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
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = byte.Parse(vals[a]);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = byte.Parse(vals[a]);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
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
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = short.Parse(vals[a]);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = short.Parse(vals[a]);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] = reader.ReadInt16();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
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
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = ushort.Parse(vals[a]);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = ushort.Parse(vals[a]);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] = reader.ReadUInt16();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
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
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = int.Parse(vals[a]);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = int.Parse(vals[a]);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] = reader.ReadInt32();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
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
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = uint.Parse(vals[a]);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = uint.Parse(vals[a]);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] = reader.ReadUInt32();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
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
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = float.Parse(vals[a]);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = float.Parse(vals[a]);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] = reader.ReadSingle();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
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
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = double.Parse(vals[a]);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = double.Parse(vals[a]);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] = reader.ReadDouble();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
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
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = long.Parse(vals[a]);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = long.Parse(vals[a]);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] = reader.ReadInt64();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
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
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = ulong.Parse(vals[a]);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = unlong.Parse(vals[a]);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] = reader.ReadUInt64();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
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
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = (" + type.FullName.Replace("+", ".") + ")int.Parse(vals[a]);\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += ((int)" + field.Name + "[a]);\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = (" + type.FullName.Replace("+", ".") + ")int.Parse(vals[a]);\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += ((int)" + field.Name + "[a]);\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] =(" + type.FullName.Replace("+", ".") + ") reader.ReadInt16();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
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
//                 strRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 strRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 strRead += "\t\t\t\t\t\t" + field.Name + "[a] = vals[a];\r\n";
//                 strRead += "\t\t\t\t}\r\n";
//                 strRead += "\t\t\t}\r\n";
// 
//                 strWrite += "\t\t\t{\r\n";
//                 strWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 strWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 strWrite += "\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 strWrite += "\t\t\t\t\t{\r\n";
//                 strWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 strWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 strWrite += "\t\t\t\t\t}\r\n";
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
//                 xmlRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[vals.Length];\r\n";
//                 xmlRead += "\t\t\t\t\tfor(int a =0; a < vals.Length; ++a)\r\n";
//                 xmlRead += "\t\t\t\t\t\t" + field.Name + "[a] = vals[a];\r\n";
//                 xmlRead += "\t\t\t\t}\r\n";
//                 xmlRead += "\t\t\t}\r\n";
// 
//                 xmlWrite += "\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\tstring strRet = \"\";\r\n";
//                 xmlWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 xmlWrite += "\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 xmlWrite += "\t\t\t\t\t{\r\n";
//                 xmlWrite += "\t\t\t\t\t\tstrRet += " + field.Name + "[a];\r\n";
//                 xmlWrite += "\t\t\t\t\t\tif(a < " + field.Name + ".Length-1) strRet += \"@\";\r\n";
//                 xmlWrite += "\t\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\t}\r\n";
//                 xmlWrite += "\t\t\t\tele.SetAttribute(\"" + field.Name + "\", strRet );\r\n";
//                 xmlWrite += "\t\t\t}\r\n";
// 
//                 binaryRead += "\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\tbyte size = reader.ReadByte();\r\n";
//                 binaryRead += "\t\t\t\tif(size>0)\r\n";
//                 binaryRead += "\t\t\t\t{\r\n";
//                 binaryRead += "\t\t\t\t\t" + field.Name + " = new " + type.FullName + "[size];\r\n";
//                 binaryRead += "\t\t\t\t\tfor(int a =0; a < size; ++a)\r\n";
//                 binaryRead += "\t\t\t\t\t\t" + field.Name + "[a] = reader.ReadString();\r\n";
//                 binaryRead += "\t\t\t\t}\r\n";
//                 binaryRead += "\t\t\t}\r\n";
// 
//                 binaryWrite += "\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\twriter.Write((byte)(" + field.Name + " != null ? " + field.Name + ".Length:0))\r\n";
//                 binaryWrite += "\t\t\t\tif(" + field.Name + "!=null)\r\n";
//                 binaryWrite += "\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\tfor(int a = 0; a < " + field.Name + ".Length; ++a)\r\n";
//                 binaryWrite += "\t\t\t\t\t{\r\n";
//                 binaryWrite += "\t\t\t\t\t\twriter.Write(" + field.Name + "[a]);\r\n";
//                 binaryWrite += "\t\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t\t}\r\n";
//                 binaryWrite += "\t\t\t}\r\n";
//             }
//             else
//                 return false;
//             return true;
//         }
//         //------------------------------------------------------
//         public static bool bArray(Type paramType, out Type argType)
//         {
//             argType = null;
//             if (paramType == null) return false;
//             if (paramType.IsArray && paramType.FullName.Contains("[]"))
//             {
//                 argType = paramType.Assembly.GetType(paramType.FullName.Replace("+", ".").Replace("[]", ""));
//                 return true;
//             }
//             return false;
//         }
//     }
// }
// #endif