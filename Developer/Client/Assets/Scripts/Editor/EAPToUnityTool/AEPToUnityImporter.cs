#if UNITY_EDITOR
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TopGame.AEPToUnity
{

    public class AEPToUnityImporter
    {
     //   [MenuItem("Window/AEPToUnity/AEPToUnity")]
        public static void ImportAEPToUnity()
        {
            string fileName = "AEPToUnity.importer";
            string fileDll = "AEPToUnity.dll";
            string currentPath = Application.dataPath + "/Scripts/Editor";
            //Debug.LogError(currentPath);
            string[] fullPath = Directory.GetFiles(currentPath + "/EAPToUnityTool", fileName, SearchOption.AllDirectories);
            string pathFile = "";
            if (fullPath.Length > 0)
            {
                pathFile = fullPath[0];
            }
            if (pathFile.Length > 0)
            {
                ChangeFileType(fileName, pathFile);
            }
            else
            {
                fullPath = Directory.GetFiles(currentPath, fileName, SearchOption.AllDirectories);
                if (fullPath.Length > 0)
                {
                    pathFile = fullPath[0];
                }
                if (pathFile.Length > 0)
                {
                    ChangeFileType(fileName, pathFile);
                }
                else
                {
                    string strInfo = "Unity";
                    fullPath = Directory.GetFiles(currentPath + "/EAPToUnityTool", fileDll, SearchOption.AllDirectories);
                    if (fullPath.Length < 0)
                    {
                        fullPath = Directory.GetFiles(currentPath, fileDll, SearchOption.AllDirectories);
                        if (fullPath.Length < 0)
                        {
                            EditorUtility.DisplayDialog("Import Error", "Can not find file \"" + fileName + "\" for importing !", "OK");
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Already imported", "To using AEP Converter press\nWindow->AEPToUnity->" + strInfo, "OK");
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Already imported", "To using AEP Converter press\nWindow->AEPToUnity->" + strInfo, "OK");
                    }
                }
            }
        }
        static private void ChangeFileType(string filename, string path)
        {
            string str = Path.ChangeExtension(path, ".dll");
            if (File.Exists(str))
                return;
            File.Move(path, str);
            AssetDatabase.Refresh();

            string strInfo = "Unity";
            EditorUtility.DisplayDialog("Import Finish", "import finish, To using AEP Converter press\nWindow->AEPToUnity->" + strInfo, "OK");

        }

    }
}
#endif