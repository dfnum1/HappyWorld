using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class UISpriteUseChecker
    {
        class PrefabRef
        {
            public string atlasName;
            public List<string> prefabs = new List<string>();
        }
        static Dictionary<string, PrefabRef> m_vGUIDRefs = new Dictionary<string, PrefabRef>();
        //------------------------------------------------------
        [MenuItem("Tools/UI/精灵图片使用情况")]
        public static void CheckDependces()
        {
            m_vGUIDRefs.Clear();
            GUI.FocusControl("");
            EditorUtility.DisplayProgressBar("收集工程内所有的ui图片", "", 0);
            string[] textureAssets= AssetDatabase.FindAssets("t:texture", new string[] { "Assets/DatasRef/UI", "Assets/Datas/Texture" });
            for(int i = 0; i < textureAssets.Length; ++i)
            {
                string path = AssetDatabase.GUIDToAssetPath(textureAssets[i]);
                EditorUtility.DisplayProgressBar("收集工程内所有的ui图片", path, (float)i/ (float)textureAssets.Length);
                PrefabRef refP = new PrefabRef();
                AssetImporter import = AssetImporter.GetAtPath(path);
                if (import != null && import is TextureImporter)
                {
                    if((import as TextureImporter).textureType == TextureImporterType.Sprite)
                    {
                        string[] splitText = path.Split('/');
                        if (splitText.Length > 0)
                        {
                            string atlasName = System.IO.Path.GetDirectoryName(path).Replace("\\", "/");
                            if (splitText.Length > 3)
                                atlasName = splitText[splitText.Length - 3] + "_" + splitText[splitText.Length - 2] + "_" + splitText[splitText.Length - 1];
                            else if (splitText.Length > 2)
                                atlasName = splitText[splitText.Length - 2] + "_" + splitText[splitText.Length - 1];
                            else
                                atlasName = splitText[splitText.Length - 1];
                            refP.atlasName = atlasName;
                        }
                    }
                }
                m_vGUIDRefs[path] = refP;
            }
            EditorUtility.ClearProgressBar();

            string mathcPattern = "(?<=guid:).*?(?=,)";
            string mathcTextPattern = "(?<=\").*?(?=\")";

            string[] unitys = AssetDatabase.FindAssets("t:unity");
            EditorUtility.DisplayProgressBar("分析工程所有场景", "", 0);
            for (int i = 0; i < unitys.Length; ++i)
            {
                string path = AssetDatabase.GUIDToAssetPath(unitys[i]);
                EditorUtility.DisplayProgressBar("分析工程所有场景", path, (float)i / (float)unitys.Length);
                string content = File.ReadAllText(path);
                MatchCollection mc = Regex.Matches(content, mathcPattern);
                foreach (Match m in mc)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(m.Value.Trim());
                    if (string.IsNullOrEmpty(assetPath)) continue;
                    AssetImporter import = AssetImporter.GetAtPath(assetPath);
                    if (import != null && import is TextureImporter)
                    {
                        PrefabRef refNew;
                        if (m_vGUIDRefs.TryGetValue(assetPath, out refNew))
                        {
                            refNew.prefabs.Add(path);
                        }
                    }
                }
                mc = Regex.Matches(content, mathcTextPattern);
                foreach (Match m in mc)
                {
                    string assetPath = m.Value.Trim();
                    if (string.IsNullOrEmpty(assetPath) || !assetPath.StartsWith("Assets/")) continue;
                    foreach (var db in m_vGUIDRefs)
                    {
                        PrefabRef refP = db.Value;
                        if (db.Key.Contains(assetPath))
                        {
                            refP.prefabs.Add(path);
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();

            string[] prefabs = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Datas/uis" });
            EditorUtility.DisplayProgressBar("分析工程所有ui", "", 0);
            for (int i = 0; i < prefabs.Length; ++i)
            {
                string path= AssetDatabase.GUIDToAssetPath(prefabs[i]);
                EditorUtility.DisplayProgressBar("分析工程所有ui", path, (float)i / (float)prefabs.Length);
                string content = File.ReadAllText(path);
                MatchCollection mc = Regex.Matches(content, mathcPattern);
                foreach (Match m in mc)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(m.Value.Trim());
                    if (string.IsNullOrEmpty(assetPath)) continue;
                    AssetImporter import = AssetImporter.GetAtPath(assetPath);
                    if (import != null && import is TextureImporter)
                    {
                        PrefabRef refNew;
                        if (m_vGUIDRefs.TryGetValue(assetPath, out refNew))
                        {
                            refNew.prefabs.Add(path);
                        }
                    }
                }
                mc = Regex.Matches(content, mathcTextPattern);
                foreach (Match m in mc)
                {
                    string assetPath = m.Value.Trim();
                    if (string.IsNullOrEmpty(assetPath) || !assetPath.StartsWith("Assets/")) continue;
                    foreach (var db in m_vGUIDRefs)
                    {
                        PrefabRef refP = db.Value;
                        if (db.Key.Contains(assetPath))
                        {
                            refP.prefabs.Add(path);
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();
            
            string[] soDatas = AssetDatabase.FindAssets("t:ScriptableObject");
            EditorUtility.DisplayProgressBar("分析工程所有SO", "", 0);
            for (int i = 0; i < soDatas.Length; ++i)
            {
                string path = AssetDatabase.GUIDToAssetPath(soDatas[i]);
                EditorUtility.DisplayProgressBar("分析工程所有SO", path, (float)i / (float)soDatas.Length);
                string content = File.ReadAllText(path);
                MatchCollection mc = Regex.Matches(content, mathcPattern);
                foreach (Match m in mc)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(m.Value.Trim());
                    if (string.IsNullOrEmpty(assetPath)) continue;
                    AssetImporter import = AssetImporter.GetAtPath(assetPath);
                    if (import != null && import is TextureImporter)
                    {
                        PrefabRef refNew;
                        if (m_vGUIDRefs.TryGetValue(assetPath, out refNew))
                        {
                            refNew.prefabs.Add(path);
                        }
                    }
                }

                mc = Regex.Matches(content, mathcTextPattern);
                foreach (Match m in mc)
                {
                    string assetPath = m.Value.Trim();
                    if (string.IsNullOrEmpty(assetPath) || !assetPath.StartsWith("Assets/")) continue;
                    foreach (var db in m_vGUIDRefs)
                    {
                        PrefabRef refP = db.Value;
                        if (db.Key.Contains(assetPath))
                        {
                            refP.prefabs.Add(path);
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();
            string[] textAssets = AssetDatabase.FindAssets("t:TextAsset",new string[]{ "Assets/Datas/Config", "Assets/DataRefs/Config" });
            EditorUtility.DisplayProgressBar("分析工程所有文本", "", 0);
            for (int i = 0; i < textAssets.Length; ++i)
            {
                string path = AssetDatabase.GUIDToAssetPath(textAssets[i]);
                EditorUtility.DisplayProgressBar("分析工程所有配置文本", path, (float)i / (float)textAssets.Length);
                string content = File.ReadAllText(path);
                MatchCollection mc = Regex.Matches(content, mathcTextPattern);
                foreach (Match m in mc)
                {
                    string assetPath = m.Value.Trim();
                    foreach (var db in m_vGUIDRefs)
                    {
                        PrefabRef refP = db.Value;
                        if(db.Key.Contains(assetPath))
                        {
                            refP.prefabs.Add(path);
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();

            string[] scriptAssets = AssetDatabase.FindAssets("t:MonoScript", new string[] { "Assets/Scripts" });
            EditorUtility.DisplayProgressBar("分析工程所有脚本", "", 0);
            for (int i = 0; i < scriptAssets.Length; ++i)
            {
                string path = AssetDatabase.GUIDToAssetPath(scriptAssets[i]);
                if(path.EndsWith(".mdb") || path.EndsWith(".dll")) continue;
                EditorUtility.DisplayProgressBar("分析工程所有脚本", path, (float)i / (float)scriptAssets.Length);
                string content = File.ReadAllText(path);
                MatchCollection mc = Regex.Matches(content, mathcTextPattern);
                foreach (Match m in mc)
                {
                    string assetPath = m.Value.Trim();
                    foreach (var db in m_vGUIDRefs)
                    {
                        PrefabRef refP = db.Value;
                        if (db.Key.Contains(assetPath))
                        {
                            refP.prefabs.Add(path);
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();

            string strExportCSv = Application.dataPath + "/../Local";
            if(!System.IO.Directory.Exists(strExportCSv))
            {
                System.IO.Directory.CreateDirectory(strExportCSv);
            }
            strExportCSv += "/TextureRefs.csv";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("图片,使用情况");
            foreach (var db in m_vGUIDRefs)
            {
                string str = "";
                for(int i =0; i < db.Value.prefabs.Count; ++i)
                {
                    str += db.Value.prefabs[i] + "\r\n";
                }
                sb.AppendFormat("{0},{1}\r\n", db.Key, str);
            }
            System.IO.FileStream fs = new System.IO.FileStream(strExportCSv, System.IO.FileMode.OpenOrCreate);
            System.IO.BinaryWriter sw = new System.IO.BinaryWriter(fs, System.Text.Encoding.UTF8);
            sw.BaseStream.SetLength(0);
            sw.BaseStream.Position = 0;
            sw.Write(sb.ToString());
            sw.Close();
        }
    }

}

