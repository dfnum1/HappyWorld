using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class MessageBuilder
    {
        struct Message
        {
            public int nCode;
            public string ClassName;
        }
        static string PacketBuilderPath = Application.dataPath + "/Scripts/MainScripts/NetWork/AutoCode/PacketBuilder.cs";
        //------------------------------------------------------
        static bool IsGoogleIMessage(Type type)
        {
            if (!type.IsClass) return false;

            bool bMsg = false;
            Type[] interfaces = type.GetInterfaces();
            if (interfaces == null) return false;
            for(int i = 0; i < interfaces.Length; ++i)
            {
                if (interfaces[i] == typeof(Google.Protobuf.IMessage))
                {
                    bMsg = true;
                    break;
                }
            }

            if (bMsg /*type.IsSubclassOf(typeof(Google.Protobuf.IMessage))*/)
            {
                if (type.FullName.Contains("+Types")) return false;
                if (type.FullName.Contains("Proto3.") && (type.FullName.Contains("Request") || type.FullName.Contains("Response"))) return true;
            }
            return false;
        }
        //------------------------------------------------------
        [MenuItem("Tools/代码/消息协议")]
        public static void DoBuilder()
        {
            string root = System.IO.Path.GetDirectoryName(PacketBuilderPath);
            if (!Directory.Exists(root)) Directory.CreateDirectory(root);


            HashSet<Proto3.MID> vMids = new HashSet<Proto3.MID>();
            foreach (Proto3.MID o in Enum.GetValues(typeof(Proto3.MID)))
            {
                vMids.Add(o);
            }
            List<Message> vMessageClass = new List<Message>();
            Assembly assembly = null;
            foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                if (ass.GetName().Name == "MainScripts")
                {
                    assembly = ass;
                    Type[] types = assembly.GetTypes();
                    for (int i = 0; i < types.Length; ++i)
                    {
                        Type tp = types[i];
                        if (IsGoogleIMessage(tp))
                        {
                            Proto3.MID mid = Proto3.MID.None;
                            foreach (var db in vMids)
                            {
                                if (tp.FullName.ToLower().Contains(db.ToString().ToLower()))
                                {
                                    mid = db;
                                    break;
                                }
                            }
                            if (mid == Proto3.MID.None)
                            {
                                Debug.LogError("消息["+tp.Name+"] 对应消息码不存在,请按指定格式 xxxReq 或者 xxxRes");
                                continue;
                            }
                            else
                            {
                                vMessageClass.Add(new Message() { nCode = (int)mid, ClassName = tp.FullName });
                            }
                        }
                    }
                }
            }

            System.DateTime nowTime = System.DateTime.Now;
            string date_time = string.Format("{0}:{1}:{2}   {3}:{4}", nowTime.Day, nowTime.Month, nowTime.Year, nowTime.Hour, nowTime.Minute);

            string code = "";
            code += "/********************************************************************\n";
            code += "生成日期:	" + date_time + "\n";
            code += "作    者:	" + "自动生成" + "\n";
            code += "描    述:\n";
            code += "*********************************************************************/\n";

            code += "namespace TopGame.Net\n";
            code += "{\n";
            code += "\tpublic static class PacketBuilder\n";
            code += "\t{\n";
            code += "\t\tpublic static Google.Protobuf.IMessage newBuilder(int code, byte[] pDatas, int offset, int nLenth)\n";
            code += "\t\t{\n";
            code += "\t\t\tswitch(code)\n";
            code += "\t\t\t{\n";
            for (int i = 0; i < vMessageClass.Count; ++i)
            {
                code += "\t\t\tcase " + vMessageClass[i].nCode + ":\n";
                code += "\t\t\t{\n";
                // code += "\t\t\t\tGoogle.Protobuf.CodedInputStream pStream = new Google.Protobuf.CodedInputStream(pDatas, 0, nLenth);\n";
                // code += "\t\t\t\t" + vMessageClass[i].ClassName + " pMsg = new " + vMessageClass[i].ClassName + "();\n";
                // code += "\t\t\t\tpMsg.MergeFrom(pStream);\n";
                code += "\t\t\t\treturn " + vMessageClass[i].ClassName + ".Parser.ParseFrom(pDatas,offset, nLenth);\n";
                code += "\t\t\t}\n";
            }
            code += "\t\t\t}\n";
            code += "\t\t\treturn null;\n";
            code += "\t\t}\n";


            code += "\t\tpublic static Google.Protobuf.IMessage newMessageByCode(int code)\n";
            code += "\t\t{\n";
            code += "\t\t\tswitch(code)\n";
            code += "\t\t\t{\n";
            for (int i = 0; i < vMessageClass.Count; ++i)
            {
                code += "\t\t\tcase " + vMessageClass[i].nCode + ":\n";
                code += "\t\t\t{\n";
                 code += "\t\t\t\treturn new " + vMessageClass[i].ClassName + "();\n";
                code += "\t\t\t}\n";
            }
            code += "\t\t\t}\n";
            code += "\t\t\treturn null;\n";
            code += "\t\t}\n";

            code += "\t\tpublic static int getMessageCode(System.Type type)\n";
            code += "\t\t{\n";
            for (int i = 0; i < vMessageClass.Count; ++i)
            {
                code += "\t\t\tif(typeof(" + vMessageClass[i].ClassName + ") == type)";
                code += " return " + vMessageClass[i].nCode + ";\n";
            }
            code += "\t\t\treturn 0;\n";
            code += "\t\t}\n";

            code += "\t}\n";
            code += "}\n";

            if (File.Exists(PacketBuilderPath))
            {
                File.Delete(PacketBuilderPath);
            }
            FileStream fs = new FileStream(PacketBuilderPath, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(code);
            writer.Close();

            MessageHandler.DoBuilder();
        }
    }

}

