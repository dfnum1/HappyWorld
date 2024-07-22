#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	DBTypeAutoCode
作    者:	HappLI
描    述:	db 数据索引类型代码自动生成
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.IO;
using TopGame.Data;

namespace TopGame.Data
{
    public class DBTypeAutoCode
    {
        class ClassBinaryCode
        {
            public System.Type classType = null;
            public DBTypeAttribute attr;
        }
        public static string GeneratorCode = "/Scripts/MainScripts/GameData/ServerData/DBTypeMapping.cs";
        public static void Build()
        {
            List<ClassBinaryCode> ClassCodeMapping = new List<ClassBinaryCode>();
            Assembly assembly = null;
            foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                assembly = ass;
                Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; ++i)
                {
                    Type tp = types[i];
                    if (tp.IsDefined(typeof(DBTypeAttribute), false))
                    {
                        ClassCodeMapping.Add(new ClassBinaryCode() { classType = tp, attr = tp.GetCustomAttribute<DBTypeAttribute>() });
                    }
                }
            }
            BuildCode(Application.dataPath + GeneratorCode, ClassCodeMapping);
        }
        //------------------------------------------------------
        static void BuildCode(string strPath, List<ClassBinaryCode> types)
        {
            if (File.Exists(strPath))
            {
                File.Delete(strPath);
            }

            string code = "//auto generator\r\n";
            code += "namespace TopGame.Data\r\n";
            code += "{\r\n";
            code += "\tpublic class DBTypeMapping \r\n";
            code += "\t{\r\n";

            code += "\t\tpublic static int" + " GetTypeIndex(System.Type type)\r\n";
            code += "\t\t{\r\n";
            foreach (var db in types)
            {
                code += "\t\t\tif(typeof(" + db.classType + ") == type) return (int)TopGame.Data.EDBType." + db.attr.type + ";\r\n";
            }
            code += "\t\t\treturn -1;\r\n";
            code += "\t\t}\r\n";

            code += "\t\tpublic static System.Type GetDBType( EDBType type)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tswitch(type)\r\n";
            code += "\t\t\t{\r\n";
            foreach (var db in types)
                code += "\t\t\t\tcase TopGame.Data.EDBType." + db.attr.type + ": return typeof(" + db.classType + ");\r\n";
            code += "\t\t\t}\r\n";
            code += "\t\t\treturn null;\r\n";
            code += "\t\t}\r\n";

            code += "\t\tpublic static AProxyDB NewProxyDB(EDBType type, SvrData.User user)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tAProxyDB proxy = null;\r\n";
            code += "\t\t\tswitch(type)\r\n";
            code += "\t\t\t{\r\n";
            foreach (var db in types)
                code += "\t\t\t\tcase TopGame.Data.EDBType." + db.attr.type + ": proxy = new " + db.classType + "(); break;\r\n";
            code += "\t\t\t}\r\n";
            code += "\t\t\tif(proxy!=null) proxy.Init(user);\r\n";
            code += "\t\t\treturn proxy;\r\n";
            code += "\t\t}\r\n";

            code += "\t}\r\n";
            code += "}\r\n";

            FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(code);
            writer.Close();
        }
    }
}
#endif