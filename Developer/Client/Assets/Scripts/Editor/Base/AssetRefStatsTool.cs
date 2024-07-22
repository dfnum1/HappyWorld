using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class AssetRefStatsTool
    {
        //------------------------------------------------------
        [MenuItem("Tools/导出所有资源引用情况", false,5)]
        public static void ExportAllAssetStats()
        {
            string strDir = EditorUtility.OpenFolderPanel("导出", Application.dataPath + "/../../", "assetStats");

            HashSet<string> vDynamicProgramCall = new HashSet<string>();
            Dictionary<string, HashSet<string>> GuidAssets = new Dictionary<string, HashSet<string>>();
            ParseProjectAssets(ref GuidAssets, ref vDynamicProgramCall);

            ExportFilterAsset(GuidAssets, vDynamicProgramCall, "t:texture", strDir + "/texturesRefs.csv", "贴图");
            ExportFilterAsset(GuidAssets, vDynamicProgramCall, "t:audioclip", strDir + "/audioRefs.csv", "音频");
            ExportFilterAsset(GuidAssets, vDynamicProgramCall, "t:mesh", strDir + "/meshRefs.csv", "网格");
            ExportFilterAsset(GuidAssets, vDynamicProgramCall, "t:shader", strDir + "/shaderRefs.csv", "Shader");
            ExportFilterAsset(GuidAssets, vDynamicProgramCall, "t:material", strDir + "/materialRefs.csv", "材质");
            ExportFilterAsset(GuidAssets, vDynamicProgramCall, "t:prefab", strDir + "/prefabRefs.csv", "预制体");
            ExportFilterAsset(GuidAssets, vDynamicProgramCall, "t:prefab", strDir + "/prefabRefs.csv", "预制体");
            EditorKits.OpenPathInExplorer(strDir);
        }
        //------------------------------------------------------
        public static void ParseProjectAssets(ref Dictionary<string, HashSet<string>> GuidAssets, ref HashSet<string> vDynamicProgramCall)
        {
            if(vDynamicProgramCall == null) vDynamicProgramCall = new HashSet<string>();
            vDynamicProgramCall.Clear();
            string strUtil = Application.dataPath + "/Scripts/MainScripts/GameBase/Util.cs";
            if (File.Exists(strUtil))
            {
                string matchCode = "(?<=\"Assets/).*?(?=\")";
                MatchCollection mc1 = Regex.Matches(File.ReadAllText(strUtil), matchCode);
                foreach (Match m in mc1)
                {
                    string assetPath = "Assets/" + m.Value.Trim();
                    vDynamicProgramCall.Add(assetPath);
                }
            }
            if(GuidAssets == null)
                GuidAssets = new Dictionary<string, HashSet<string>>();
            GuidAssets.Clear();
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            string path;
            string mathcPattern = "(?<=guid:).*?(?=,)";
            string mathcPattern1 = "(?<=Assets/)+(.*?)\\.(prefab|asset|fbx|FBX|csv|mat|png|jpg|jpeg|tga|mp3|mp4|wav|ogg)";
            EditorUtility.DisplayProgressBar("导出", "解析游戏资源", 0);
            for (int i = 0; i < allAssetPaths.Length; i++)
            {
                path = allAssetPaths[i];
                string parentGuid = AssetDatabase.AssetPathToGUID(path);
                if(!string.IsNullOrEmpty(parentGuid))
                {
                    HashSet<string> vParentList;
                    if (!GuidAssets.TryGetValue(parentGuid, out vParentList))
                    {
                        vParentList = new HashSet<string>();
                        GuidAssets.Add(parentGuid, vParentList);
                    }
                }

                if (path.EndsWith(".prefab") || path.EndsWith(".unity") || path.EndsWith(".mat") || path.EndsWith(".asset")
                    || path.EndsWith(".csv") || path.EndsWith(".json"))
                {
                    string content = File.ReadAllText(path);
                    MatchCollection mc = Regex.Matches(content, mathcPattern);
                    foreach (Match m in mc)
                    {
                        string guid = m.Value.Trim();
                        HashSet<string> vList;
                        if (!GuidAssets.TryGetValue(guid, out vList))
                        {
                            vList = new HashSet<string>();
                            GuidAssets.Add(guid, vList);
                        }
                        vList.Add(path);
                    }
                    if (content.Contains("Assets/Datas/"))
                    {
                        MatchCollection mc1 = Regex.Matches(content, mathcPattern1);
                        foreach (Match m in mc1)
                        {
                            string assetPath = "Assets/" + m.Value.Trim();
                            string guid = AssetDatabase.AssetPathToGUID(assetPath);
                            if (string.IsNullOrEmpty(guid)) continue;
                            HashSet<string> vList;
                            if (!GuidAssets.TryGetValue(guid, out vList))
                            {
                                vList = new HashSet<string>();
                                GuidAssets.Add(guid, vList);
                            }
                            vList.Add(path);
                        }
                    }

                    // if (i %10==0)
                    EditorUtility.DisplayProgressBar("导出", "解析游戏资源[" + i.ToString() + "/" + allAssetPaths.Length + "]", (float)i / (float)allAssetPaths.Length);
                }
            }
            EditorUtility.ClearProgressBar();
        }
        //------------------------------------------------------
        static void ExportFilterAsset(Dictionary<string, HashSet<string>> GuidAssets, HashSet<string> vDynamicProgramCall,string filterKey, string exportFile, string displayName)
        {
            StringBuilder csvBuiler = new StringBuilder();
            string barName = "统计所有" + displayName + "资源";
            EditorUtility.DisplayProgressBar("导出", barName, 0);
            csvBuiler.AppendLine("资源路径,使用次数,详情");
            string[] assets = AssetDatabase.FindAssets(filterKey);
            string strDetial = "";
            for (int i = 0; i < assets.Length; ++i)
            {
                EditorUtility.DisplayProgressBar("导出", barName, (float)i / (float)assets.Length);
                string assetPath = AssetDatabase.GUIDToAssetPath(assets[i]);
                if (assetPath.StartsWith("Packages/")) continue;
                strDetial = "\"";
                int cnt = 0;
                HashSet<string> vList;
                if (GuidAssets.TryGetValue(assets[i], out vList))
                {
                    foreach (var db in vList)
                    {
                        strDetial += db + "\r\n";
                    }
                    cnt = vList.Count;
                }
                foreach (var db in vDynamicProgramCall)
                {
                    if (assetPath.Contains(db))
                    {
                        strDetial += "由程序动态拼接调用\r\n";
                        cnt++;
                        break;
                    }
                }

                strDetial += "\"";
                csvBuiler.AppendFormat("{0},{1},{2}\r\n", assetPath, cnt.ToString(), strDetial);
            }
            EditorUtility.ClearProgressBar();
            if (System.IO.File.Exists(exportFile))
                System.IO.File.Delete(exportFile);
            System.IO.FileStream fs = new System.IO.FileStream(exportFile, System.IO.FileMode.OpenOrCreate);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
            sw.Write(csvBuiler.ToString());
            sw.Close();
        }
    }

}

