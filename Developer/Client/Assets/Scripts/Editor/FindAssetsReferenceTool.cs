/********************************************************************
生成日期:	9:28:2020   14:35
类    名: 	FindAssetsReferenceTool
作    者:	JaydenHe
描    述:	寻找索引工具
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using TopGame.ED;
using UnityEditor;
using UnityEngine;
namespace TopGame.ED
{
    public class FindAssetsReferenceTool : EditorWindow
    {
        public static FindAssetsReferenceTool Instance { protected set; get; }
        static string[] assetGUIDs;
        static string[] assetPaths;
        static string[] allAssetPaths;
        static Thread thread;
        static List<string> logInfo = new List<string>();

        static string util_code_path;
        Vector2 scrollPosition;
        static string searchingState = "搜索中...";
        [MenuItem("Assets/查找资源引用", false, 1)]
        static void FindAssetReferencesMenu()
        {
            assetPaths = null;
            assetGUIDs = null;
            assetPaths = null;
            allAssetPaths = null;
            searchingState = "搜索中...";
            util_code_path = Application.dataPath + "/Scripts/MainScripts/GameBase/Util.cs";

            Rect rect = new Rect(Vector2.zero, new Vector2(720, 720));
            EditorWindow.GetWindowWithRect(typeof(FindAssetsReferenceTool), rect);

            if (Selection.assetGUIDs.Length == 0)
            {
                Debug.LogError("请先选择任意一个组件，再右键点击此菜单");
                return;
            }

            assetGUIDs = Selection.assetGUIDs;
            assetPaths = new string[assetGUIDs.Length];

            for (int i = 0; i < assetGUIDs.Length; i++)
            {
                assetPaths[i] = AssetDatabase.GUIDToAssetPath(assetGUIDs[0]);
            }

            allAssetPaths = AssetDatabase.GetAllAssetPaths();

            thread = new Thread(new ThreadStart(FindAssetReferences));
            thread.Start();
        }

        void OnGUI()
        {
            GUILayout.Label(searchingState);
            GUI.skin.label.fontSize = 24;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            scrollPosition = GUILayout.BeginScrollView(
                scrollPosition, GUILayout.Width(720), GUILayout.Height(720));

            for (int i = 0; i < logInfo.Count; i++)
            {
                if (GUILayout.Button(logInfo[i]))
                {
                    if(logInfo[i].Contains("/"))
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath(logInfo[i], typeof(Object));
                }
            }

            GUILayout.EndScrollView();

        }

        static void FindAssetReferences()
        {
            logInfo.Clear();
            string path;
            string log;
            for (int i = 0; i < allAssetPaths.Length; i++)
            {
                path = allAssetPaths[i];

                if (path.EndsWith(".prefab") || path.EndsWith(".unity") || path.EndsWith(".playable") || path.EndsWith(".mat") || path.EndsWith(".asset")
                    || path.EndsWith(".csv") || path.EndsWith(".json"))
                {
                    string content = File.ReadAllText(path);
                    if (content == null)
                    {
                        continue;
                    }

                    for (int j = 0; j < assetGUIDs.Length; j++)
                    {
                        if (content.IndexOf(assetGUIDs[j]) > 0)
                        {
                            log = string.Format(path);
                            logInfo.Add(log);
                        }
                    }

                    if(path.EndsWith(".csv") || path.EndsWith(".json"))
                    {
                        for (int j = 0; j < assetPaths.Length; j++)
                        {
                            if (content.IndexOf(assetPaths[j]) > 0)
                            {
                                log = string.Format(path);
                                logInfo.Add(log);
                            }
                        }
                    }
                }
            }

            HashSet<string> vDynamicProgramCall = new HashSet<string>();
            if (File.Exists(util_code_path))
            {
                string matchCode = "(?<=\"Assets/).*?(?=\")";
                MatchCollection mc1 = Regex.Matches(File.ReadAllText(util_code_path), matchCode);
                foreach (Match m in mc1)
                {
                    string assetPath = "Assets/" + m.Value.Trim();
                    vDynamicProgramCall.Add(assetPath);
                }
            }

            for (int j = 0; j < assetPaths.Length; j++)
            {
                foreach(var db in vDynamicProgramCall)
                {
                    if(assetPaths[j].Contains(db))
                    {
                        logInfo.Add("由程序代码动态构建加载");
                        break;
                    }
                }
            }

            Debug.Log("选择对象引用数量：" + logInfo.Count);

            Debug.Log("查找完成");
            searchingState = "查找完成";
        }
        //------------------------------------------------------
        public static void CheckDependice(string assetPath, Dictionary<string, int> vDepends, bool bAssetPath = true)
        {
            if(File.Exists(assetPath))
            {
                string mathcPattern = "(?<=guid:).*?(?=,)";
                string content = File.ReadAllText(assetPath);
                MatchCollection mc = Regex.Matches(content, mathcPattern);
                foreach (Match m in mc)
                {
                    string guid = bAssetPath?AssetDatabase.GUIDToAssetPath(m.Value.Trim()): m.Value.Trim();
                    int cnt = 0;
                    if (!vDepends.TryGetValue(guid, out cnt))
                    {
                        vDepends.Add(guid, 1);
                    }
                    else
                        vDepends[guid] = cnt + 1;
                }
            }
        }
        //------------------------------------------------------
        public static string[] CheckDependice(string assetPath, bool bAssetPath = true)
        {
            List<string> vDep = new List<string>();
            if (File.Exists(assetPath))
            {
                string mathcPattern = "(?<=guid:).*?(?=,)";
                string content = File.ReadAllText(assetPath);
                MatchCollection mc = Regex.Matches(content, mathcPattern);
                foreach (Match m in mc)
                {
                    string guid = bAssetPath ? AssetDatabase.GUIDToAssetPath(m.Value.Trim()) : m.Value.Trim();
                    if (!vDep.Contains(guid))
                    {
                        vDep.Add(guid);
                    }
                }
            }
            return vDep.ToArray();
        }
        //------------------------------------------------------
        static Dictionary<string, HashSet<string>> ms_vFindCatch = null;
        public static List<string> FindByUsedList(string assetFile)
        {
            if(ms_vFindCatch == null || ms_vFindCatch.Count<=0)
            {
                if (ms_vFindCatch == null) ms_vFindCatch = new Dictionary<string, HashSet<string>>();

                HashSet<string> vDynamicProgramCall = new HashSet<string>();
                string util_code_path = Application.dataPath + "/Scripts/MainScripts/GameBase/Util.cs";
                if (File.Exists(util_code_path))
                {
                    string matchCode = "(?<=\"Assets/).*?(?=\")";
                    MatchCollection mc1 = Regex.Matches(File.ReadAllText(util_code_path), matchCode);
                    foreach (Match m in mc1)
                    {
                        string assetPath = "Assets/" + m.Value.Trim();
                        vDynamicProgramCall.Add(assetPath);
                    }
                }

                EditorUtility.DisplayProgressBar("检测索引", "", 0);
                allAssetPaths = AssetDatabase.GetAllAssetPaths();
                for (int i = 0; i < allAssetPaths.Length; ++i)
                {
                    HashSet<string> vAssets;
                    if(!ms_vFindCatch.TryGetValue(allAssetPaths[i], out vAssets))
                    {
                        vAssets = new HashSet<string>();
                        ms_vFindCatch.Add(allAssetPaths[i], vAssets);
                    }
                    string[] deps = AssetDatabase.GetDependencies(allAssetPaths[i],true);
                    for(int j =0; j < deps.Length; ++j)
                    {
                        vAssets.Add(deps[j]);
                    }

                    if (vDynamicProgramCall.Contains(allAssetPaths[i]))
                        vDynamicProgramCall.Add("由程序动态调用");
                }
                EditorUtility.ClearProgressBar();
            }
            List<string> vUsed = new List<string>();
            foreach(var db in ms_vFindCatch)
            {
                if(db.Value.Contains(assetFile))
                {
                    vUsed.Add(db.Key);
                }
            }
            return vUsed;
        }
    }
}