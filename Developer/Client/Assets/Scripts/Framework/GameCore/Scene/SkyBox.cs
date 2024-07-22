/********************************************************************
生成日期:	3:17:2022  10:16
类    名: 	SkyBox
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
#endif
namespace TopGame.Core
{
    public class SkyBox : AInstanceAble
    {
        [System.Serializable]
        public struct SkyTheme
        {
            public string strName;
            [Framework.Data.StringViewGUI(typeof(Texture))]
            public string strTexture;
        }
        public SkyTheme[] skyThemes = null;
    }
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SkyBox), true)]
    public class SkyBoxEditor : Editor
    {
        List<SkyBox.SkyTheme> m_vThemes = new List<SkyBox.SkyTheme>();
        public void OnEnable()
        {
            SkyBox sky = target as SkyBox;
            if(sky.skyThemes!=null)
                m_vThemes = new List<SkyBox.SkyTheme>(sky.skyThemes);
        }
        public void OnDisable()
        {
            SkyBox sky = target as SkyBox;
            sky.skyThemes = m_vThemes.ToArray();
        }
        public override void OnInspectorGUI()
        {
            SkyBox sky = target as SkyBox;
            GUILayout.BeginHorizontal();
            GUILayout.Label("样式名", new GUILayoutOption[] { GUILayout.MinWidth(120) });
            GUILayout.Label("贴图", new GUILayoutOption[] { GUILayout.MinWidth(200) });
            GUILayout.Label("", new GUILayoutOption[] { GUILayout.MinWidth(50) });
            GUILayout.EndHorizontal();
            for(int i = 0; i< m_vThemes.Count; ++i)
            {
                SkyBox.SkyTheme theme = m_vThemes[i];
                GUILayout.BeginHorizontal();
                theme.strName = GUILayout.TextField(theme.strName, new GUILayoutOption[] { GUILayout.MinWidth(120) });
                Texture pTexture = EditorGUILayout.ObjectField(AssetDatabase.LoadAssetAtPath<Texture>(theme.strTexture), typeof(Texture), false, new GUILayoutOption[] { GUILayout.MinWidth(200) }) as Texture;
                if (pTexture != null)
                    theme.strTexture = AssetDatabase.GetAssetPath(pTexture);
                else theme.strTexture = "";
                if(string.IsNullOrEmpty(theme.strName) && !string.IsNullOrEmpty(theme.strTexture))
                {
                    theme.strName = System.IO.Path.GetFileNameWithoutExtension(theme.strTexture);
                }
                if(GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.MinWidth(50) }))
                {
                    m_vThemes.RemoveAt(i);
                    break;
                }
                if (!string.IsNullOrEmpty(theme.strTexture) && theme.strTexture.Contains("Assets/DatasRef"))
                {
                    EditorGUILayout.HelpBox("必须是Assets/Datas/目录下的资源", MessageType.Error);
                }
                GUILayout.EndHorizontal();
                m_vThemes[i] = theme;
            }
            if(GUILayout.Button("添加"))
            {
                m_vThemes.Add(new SkyBox.SkyTheme(){ strName ="", strTexture ="" });
            }
            if (GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
#endif
}
