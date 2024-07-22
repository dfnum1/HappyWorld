/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	DungonTheme
作    者:	HappLI
描    述:   关卡主题数据
*********************************************************************/
using Framework.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Data
{
    [System.Serializable]
    public class DungonTheme : SceneThemeData
    {
    }

    [DataBinderType("DungonThemes", "int")]
    [FieldMapTable("Mapping(GetFramework())")]
    [System.Serializable]
    public class DungonThemes
    {
        public DungonTheme[] datas;
#if UNITY_EDITOR
        static string ms_FilePath = null;
        public static string FilePath
        {
            get
            {
                if(string.IsNullOrEmpty(ms_FilePath))
                    ms_FilePath = Application.dataPath + "/DatasRef/Config/Other/DungonThemes.json";
                return ms_FilePath;
            }
        }
        public static List<string> ms_vThemeNames = null;
        public static List<int> ms_vThemes = null;
        public List<int> ThemePops
        {
            get
            {
                CheckThemes();
                return ms_vThemes;
            }
        }
        public List<string> ThemeNamePops
        {
            get
            {
                CheckThemes();
                return ms_vThemeNames;
            }
        }
        static void CheckThemes()
        {
            if (System.IO.File.Exists(FilePath))
            {
                DungonThemes themes = JsonUtility.FromJson<DungonThemes>(System.IO.File.ReadAllText(FilePath));
                if (ms_vThemeNames == null || (ms_vThemeNames.Count-1) != themes.datas.Length)
                {
                    ms_vThemeNames = new List<string>();
                    ms_vThemes = new List<int>();
                    ms_vThemes.Add(0);
                    ms_vThemeNames.Add("无");
                    for (int i =0; i < themes.datas.Length; ++i)
                    {
                        ms_vThemeNames.Add(themes.datas[i].strName);
                        ms_vThemes.Add(themes.datas[i].nID);
                    }
                }
            }
        }
        public static int OnGUITheme(string label, int id)
        {
            CheckThemes();
            if (ms_vThemes == null)
            {
                id = UnityEditor.EditorGUILayout.IntField(label, id);
                return id;
            }
            int select = ms_vThemes.IndexOf(id);
            select = UnityEditor.EditorGUILayout.Popup(label, select, ms_vThemeNames.ToArray());
            if (select >= 0 && select < ms_vThemes.Count)
                id = ms_vThemes[select];
            return id;
        }
#endif
    }
}

