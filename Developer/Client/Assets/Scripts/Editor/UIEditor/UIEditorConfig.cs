using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public enum UIEditorPathType
    {
        SceneSave,
        OpenReferTo,//选择参考图时默认打开的文件夹路径
        ExportPrefab,//Prefab界面用的
    }
    public class UIEditorConfig
    {
        public static string UISystemPrefab = "Assets/DatasRef/System/UISystem.prefab";
        public static string UIScenesPath = "Assets/DatasRef/UI/UIScenes/";
        public static string UIWidgets = "Assets/DatasRef/UI/Widgets/";

        public static string GetLastPath(UIEditorPathType type)
        {
            return EditorPrefs.GetString("PathSaver_" + UIEditorConfig.ProjectUUID + "_" + type.ToString(), GeDefaultPath(type));
        }

        public static void SetLastPath(UIEditorPathType type, string path)
        {
            if (path == "")
                return;
            path = System.IO.Path.GetDirectoryName(path);
            EditorPrefs.SetString("PathSaver_" + UIEditorConfig.ProjectUUID + "_" + type.ToString(), path);
        }

        public static string GeDefaultPath(UIEditorPathType type)
        {
            return Application.dataPath;
        }

        static string projectUUID = string.Empty;
        public static string ProjectUUID
        {
            get
            {
#if UNITY_EDITOR
                if (projectUUID == string.Empty)
                {
                    projectUUID = UIEditorHelper.GenMD5String(Application.dataPath);
                }
#endif
                return projectUUID;
            }
        }
    }

}

