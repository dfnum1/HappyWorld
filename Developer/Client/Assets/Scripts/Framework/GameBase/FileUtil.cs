/********************************************************************
生成日期:	17:9:2019   16:19
类    名: 	FileUtil
作    者:	HappLI
描    述:	通用文件工具集
*********************************************************************/
using Framework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TopGame.Base
{
    public static class FileUtil
    {//------------------------------------------------------
        public static string ReadFile(string path)
        {
            StreamReader sr = new StreamReader(path);
            string content = "";
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                content += str + "\n";
            }

            sr.Close();

            return content;
        }

        //------------------------------------------------------
        public static List<string> GetDirectoryFilesName(string path)
        {
            List<string> fileNames = new List<string>();
            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);
                foreach (FileInfo f in root.GetFiles())
                {
                    fileNames.Add(f.Name);
                }
            }
            return fileNames;
        }
        //------------------------------------------------------
        public static void WriteFile(string sFilePath, string sFileName, string content, string ex)
        {
            sFileName = sFilePath + "/" + sFileName + "." + ex; //文件的绝对路径
            if (!Directory.Exists(sFilePath))//验证路径是否存在
            {
                Directory.CreateDirectory(sFilePath);
                //不存在则创建
            }
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))
            //验证文件是否存在，有则追加，无则创建
            {
                File.Delete(sFileName);
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            sw.WriteLine(content);
            sw.Close();
            fs.Close();

            Debug.LogError("模板导出成功:" + sFileName);
        }
    }
}