using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class UIDependChecker : EditorWindow
    {
        static UIDependChecker ms_instance = null;

        [MenuItem("Assets/查看依赖资源")]
        public static void CheckDependces()
        {
            if (Selection.activeGameObject == null) return;
            if (ms_instance == null)
            {
                ms_instance = EditorWindow.GetWindow<UIDependChecker>();
                ms_instance.titleContent = new GUIContent("查看依赖资源");
                ms_instance.Show();
            }
            ms_instance.Check(Selection.activeGameObject);
        }
        class SpritePak
        {
            public UnityEngine.Object sprite;
            public UnityEngine.Object replace_sprite;
            public int usedCnt;
            public string pakTag;
            public bool replace;
        }

        string m_UseTips = "";
        string m_prefabPath = "";
        string m_bindScreenPath = "";
        Dictionary<string, int> m_vUsedPakTag = new Dictionary<string, int>();
        List<SpritePak> m_vDepSprites = new List<SpritePak>();
        List<UnityEngine.Object> m_Deps = new List<Object>();
        GameObject m_pPrefab = null;
        Vector2 m_Scroll = Vector2.zero;
        //------------------------------------------------------
        private void OnEnable()
        {
            Check(Selection.activeGameObject);
        }
        //------------------------------------------------------
        string FindBindScene(string path)
        {
            List<FileInfo> vFiles = new List<FileInfo>();
            EditorKits.FindDirFiles("Assets/DatasRef/UI/UIScenes", vFiles);
            for(int i = 0; i < vFiles.Count; ++i)
            {
                string strContext = File.ReadAllText(vFiles[i].FullName);
                if(strContext.Contains(path))
                    return vFiles[i].FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
            }
            return null;
        }
        //------------------------------------------------------
        void Check(GameObject pObj)
        {
            GUI.FocusControl("");
            m_bindScreenPath = "";
            m_vDepSprites.Clear();
            m_Deps.Clear();
            m_vUsedPakTag.Clear();
            m_UseTips = "总共使用精灵图集:\r\n";

            m_pPrefab = pObj;
            if (m_pPrefab == null) return;
            m_prefabPath = AssetDatabase.GetAssetPath(pObj);
            m_bindScreenPath = FindBindScene(m_prefabPath);
            // string[] assets = AssetDatabase.GetDependencies(m_prefabPath);
            string mathcPattern = "(?<=guid:).*?(?=,)";
            string content = File.ReadAllText(m_prefabPath);
            Dictionary<string, int> assets = new Dictionary<string, int>();
            MatchCollection mc = Regex.Matches(content, mathcPattern);
            foreach (Match m in mc)
            {
                string guid = AssetDatabase.GUIDToAssetPath(m.Value.Trim());
                int cnt = 0;
                if (!assets.TryGetValue(guid, out cnt))
                {
                    assets.Add(guid, 1);
                }
                else
                    assets[guid] = cnt + 1;
            }

            foreach (var db in assets)
            {
                UnityEngine.Object pRet = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(db.Key);
                if (pRet == null) continue;
                AssetImporter import = AssetImporter.GetAtPath(db.Key);
                if (import != null && import is TextureImporter && (import as TextureImporter).textureType == TextureImporterType.Sprite)
                {
                    string dir = System.IO.Path.GetDirectoryName(db.Key).Replace("\\", "/");
                    if (string.IsNullOrEmpty(dir)) continue;
                    string[] splitText = dir.Split('/');
                    if (splitText.Length > 0)
                    {
                        string atlasName = dir;
                        if (splitText.Length > 3)
                            atlasName = splitText[splitText.Length - 3] + "_" + splitText[splitText.Length - 2] + "_" + splitText[splitText.Length - 1];
                        else if (splitText.Length > 2)
                            atlasName = splitText[splitText.Length - 2] + "_" + splitText[splitText.Length - 1];
                        else
                            atlasName = splitText[splitText.Length - 1];

                        SpritePak pak = new SpritePak();
                        pak.sprite = pRet;
                        pak.pakTag = atlasName;
                        pak.usedCnt = db.Value;
                        m_vDepSprites.Add(pak);
                        int useTotal = 0;
                        if (!m_vUsedPakTag.TryGetValue(atlasName, out useTotal))
                        {
                            m_vUsedPakTag.Add(atlasName, 1);
                        }
                        else
                            m_vUsedPakTag[atlasName] = useTotal + 1;
                        continue;
                    }

                }
                if (!m_Deps.Contains(pRet))
                {
                    m_Deps.Add(pRet);
                }
            }
            if (m_vUsedPakTag.Count > 0)
            {
                foreach (var db in m_vUsedPakTag)
                {
                    m_UseTips += db.Key + "  使用次数:" + db.Value + "\r\n";
                }
                if (m_vUsedPakTag.Count > 2)
                    m_UseTips += "  请务必检查！\r\n";
            }
        }
        //------------------------------------------------------
        void OnGUI()
        {
            if (m_vUsedPakTag.Count > 0)
            {
                EditorGUILayout.HelpBox(m_UseTips, m_vUsedPakTag.Count > 2 ? MessageType.Warning : MessageType.Info);
            }
            EditorGUILayout.ObjectField(m_pPrefab, typeof(GameObject), false);
            m_bindScreenPath = EditorGUILayout.TextField("场景绑定", m_bindScreenPath);
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll, new GUILayoutOption[] { GUILayout.Width(position.width) });
            int index = 0;
            bool bReplaceBtn = false;
            for (int i = 0; i < m_vDepSprites.Count; ++i)
            {
                EditorGUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(position.width - 10) });
                EditorGUILayout.LabelField("tag:" + m_vDepSprites[i].pakTag + "  " + m_vDepSprites[i].usedCnt);
                EditorGUILayout.ObjectField(m_vDepSprites[i].sprite, typeof(Sprite), false, new GUILayoutOption[] { GUILayout.Width(120) });
                EditorGUILayout.LabelField("替换为", new GUILayoutOption[] { GUILayout.Width(60) });
                m_vDepSprites[i].replace_sprite = EditorGUILayout.ObjectField(m_vDepSprites[i].replace_sprite, typeof(Sprite), false, new GUILayoutOption[] { GUILayout.Width(120) }) as Sprite;
                m_vDepSprites[i].replace = EditorGUILayout.Toggle(m_vDepSprites[i].replace);
                EditorGUILayout.EndHorizontal();

                if (m_vDepSprites[i].replace) bReplaceBtn = true;
            }
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;
            for (int i = 0; i < m_Deps.Count; ++i)
            {
                EditorGUILayout.ObjectField((index++).ToString(), m_Deps[i], typeof(UnityEngine.Object), false, new GUILayoutOption[] { GUILayout.Width(position.width-20) });
            }
            EditorGUILayout.EndScrollView();
            EditorGUIUtility.labelWidth = labelWidth;

            if (bReplaceBtn && GUILayout.Button("执行替换"))
            {
                string sceneContent = null;
                if (!string.IsNullOrEmpty(m_bindScreenPath))
                    sceneContent = File.ReadAllText(m_bindScreenPath);
                string content = File.ReadAllText(m_prefabPath);
                bool bDirty = false;
                for (int i = 0; i < m_vDepSprites.Count; ++i)
                {
                    if (m_vDepSprites[i].replace && m_vDepSprites[i].sprite)
                    {
                        string src = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(m_vDepSprites[i].sprite));
                        string to = "";
                        if (m_vDepSprites[i].replace_sprite!=null)
                            to = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(m_vDepSprites[i].replace_sprite));
                        if (src.CompareTo(to) != 0)
                        {
                            bDirty = true;
                            content = content.Replace(src, to);
                            if (!string.IsNullOrEmpty(sceneContent))
                                sceneContent = sceneContent.Replace(src, to);
                            m_vDepSprites[i].sprite = m_vDepSprites[i].replace_sprite;
                            m_vDepSprites[i].replace_sprite = null;
                            m_vDepSprites[i].replace = false;
                        }
                    }
                }
                if (bDirty)
                {
                    File.WriteAllText(m_prefabPath, content);
                    EditorUtility.SetDirty(m_pPrefab);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    Check(m_pPrefab);
                }
            }
        }
    }

}

