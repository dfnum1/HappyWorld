/********************************************************************
生成日期:	25:7:2019   10:28
类    名: 	CompileAssetBundles
作    者:	HappLI
描    述:	
*********************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TopGame.ED
{
    public class CompileAssetBundles
    {
        public delegate string OnPrepareMarkABNameEvent(string strAssetFile);
        public class FileInfoData
        {
            public FileInfo fileInfo = null;
            public DirectoryInfo directoryInfo = null;
            public string path = "";
            public string subFile = "";
            public bool bMark;
        }
        public static OnPrepareMarkABNameEvent OnPrepareMarkAbName = null;
        public static System.Action<string, string> OnMarkAbName = null;
        public static System.Action<PublishPanel.PublishSetting, bool, bool, bool, bool> OnABBuildCallback = null;

        public class BuidDir
        {
            public string strDir;
            public bool bMark;
        }
        //------------------------------------------------------
        static int GetNameToID(string strPath)
        {
            //crc32
            return Animator.StringToHash(strPath);
        }
        //------------------------------------------------------
        public static bool MarkAssetBundleAysnByDir(PublishPanel.PublishSetting setting, string strDir, bool bMark, bool bMarkEndAction = true, bool bBuildPackage = false, bool bOpenExplorer = true, bool bHotUpdate = false)
        {
            List<BuidDir> vDir = new List<BuidDir>();
            vDir.Add(new BuidDir() { strDir = strDir, bMark = bMark });
            return MarkAssetBundleAysn(setting, bMark, vDir, bMarkEndAction, bBuildPackage, bOpenExplorer);
        }
        //------------------------------------------------------
        public static bool MarkAssetBundleAysn(PublishPanel.PublishSetting setting, bool bMark, List<BuidDir> vDirRoot, bool bMarkEndAction = true, bool bBuildPackage = false, bool bOpenExplorer = true, bool bHotUpdate = false)
        {
            if (vDirRoot.Count <= 0)
            {
                EditorUtility.DisplayDialog("Tips", "请输入一个有效的目录", "好的");
                return false;
            }
            PublishPanel.WritePublishProgress("MarkAssetBundleAysn-Begin");
            List<FileInfoData> fileInfoList = null;
            for (int i = 0; i < vDirRoot.Count; ++i)
            {
                string strDir = vDirRoot[i].strDir;
                if (strDir[strDir.Length - 1] == '/')
                    vDirRoot[i].strDir = strDir.Substring(0, strDir.Length - 1);
            }
            for (int i = 0; i < vDirRoot.Count; ++i)
            {
                GetFileList(vDirRoot[i].strDir, ref fileInfoList, vDirRoot[i].bMark);
            }

            if (fileInfoList == null || fileInfoList.Count == 0)
                return false;

            HashSet<string> vAllScriptPathRef = new HashSet<string>();
            HashSet<string> vAllUnUsedAbs = new HashSet<string>();
            if (bMark)
            {
                string[] objects = AssetDatabase.FindAssets("t:Object");
                for (int j = 0; j < objects.Length; ++j)
                {
                    AssetImporter import = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(objects[j]));
                    if (import != null && !string.IsNullOrEmpty(import.assetBundleName))
                    {
                        vAllUnUsedAbs.Add(AssetDatabase.GUIDToAssetPath(objects[j]));
                    }
                }
                vAllScriptPathRef = SearchScriptPathRefDB.Search("Assets/Datas");
            }

            Dictionary<string, string[]> vCheckDepends = new Dictionary<string, string[]>();
            Dictionary<string, int> vDepends = new Dictionary<string, int>();
            bool isCancel = false;
            int startIndex = 0;
            while (startIndex < fileInfoList.Count || isCancel)
            {
                var data = fileInfoList[startIndex];

                isCancel = EditorUtility.DisplayCancelableProgressBar("标记资源中", data.fileInfo.FullName, (float)startIndex / (float)fileInfoList.Count);

                string assetPath = string.Format("{0}/{1}", data.directoryInfo, data.fileInfo.Name);

                AssetImporter import = AssetImporter.GetAtPath(assetPath);

                if (data.fileInfo.FullName.EndsWith("meta") || import == null)
                {

                }
                else
                {
                    bool bMarkAb = data.bMark;
                    if (bMarkAb) setting.CanMarker(assetPath, ref bMarkAb);
                    else
                    {
                        if (bMark)
                            setting.CanMarker(assetPath, ref bMarkAb);
                    }
                    if (bMarkAb)
                    {
                        bool bFilePathUseAB = setting.singleFileAB;
                        if (bFilePathUseAB)
                        {
                            if (setting.useEncrptyPak)
                            {
                                for (int i = 0; i < setting.buildFileRootPathAbs.Count; ++i)
                                {
                                    if (import.assetPath.Contains(setting.buildFileRootPathAbs[i]))
                                    {
                                        bFilePathUseAB = false;
                                        break;
                                    }
                                }
                            }
                        }

                        bool bSingleAb = false;
                        if (!bFilePathUseAB)
                        {
                            for (int i = 0; i < setting.buildSingleAbs.Count; ++i)
                            {
                                if (import.assetPath.Contains(setting.buildSingleAbs[i]))
                                {
                                    bSingleAb = true;
                                    bFilePathUseAB = true;
                                    break;
                                }
                            }
                        }

                        string strAssignAb = "";
                        string abName = "";
                        if (bFilePathUseAB)
                        {
                            strAssignAb = data.subFile.ToLower();
                            if (OnPrepareMarkAbName != null)
                            {
                                string ab = OnPrepareMarkAbName(assetPath);
                                if (!string.IsNullOrEmpty(ab))
                                    strAssignAb = ab;
                            }
                            if (setting.abUseIndex) strAssignAb = GetNameToID(strAssignAb).ToString();
                        }
                        else
                        {
                            strAssignAb = data.path.ToLower();
                            if (OnPrepareMarkAbName != null)
                            {
                                string ab = OnPrepareMarkAbName(assetPath);
                                if (!string.IsNullOrEmpty(ab))
                                    strAssignAb = ab;
                            }
                            if (setting.abUseIndex) strAssignAb = GetNameToID(strAssignAb).ToString();
                        }
                        if (!bSingleAb)
                        {
                            for (int i = 0; i < setting.buildCombineAbsGroup.Count; ++i)
                            {
                                if (import.assetPath.Contains(setting.buildCombineAbsGroup[i].pathFile))
                                {
                                    strAssignAb = setting.buildCombineAbsGroup[i].strAbName;
                                    break;
                                }
                            }
                        }

                        strAssignAb = strAssignAb.Replace("\\", "/");

                        if (strAssignAb.CompareTo(import.assetBundleName) != 0 || "pkg".CompareTo(import.assetBundleVariant) != 0)
                        {
                            import.assetBundleName = strAssignAb;
                            import.assetBundleVariant = "pkg";
                            import.SaveAndReimport();
                        }
                        abName = import.assetBundleName + "." + import.assetBundleVariant;
                        if (OnMarkAbName != null) OnMarkAbName(assetPath, abName);

                        vAllUnUsedAbs.Remove(assetPath);
                        if (setting.dependMarkAB)
                        {
                            //                             string[] depthAssets;
                            //                             if (!vCheckDepends.TryGetValue(assetPath, out depthAssets))
                            //                             {
                            //                                 depthAssets = FindAssetsReferenceTool.CheckDependice(assetPath, true);
                            //                                 vCheckDepends.Add(assetPath, depthAssets);
                            //                             }
                            string[] depthAssets = AssetDatabase.GetDependencies(assetPath, true);
                            for (int k = 0; k < depthAssets.Length; ++k)
                            {
                                if (depthAssets[k].EndsWith(".cs") || depthAssets[k].StartsWith("Packages/") || depthAssets[k].Contains("/Editor/") || depthAssets[k].Contains("/ThirdParty/")) continue;
                                if (setting.CanDepMarker(depthAssets[k]))
                                {
                                    int cnt = 0;
                                    if (!vDepends.TryGetValue(depthAssets[k], out cnt))
                                    {
                                        vDepends[depthAssets[k]] = 1;
                                    }
                                    else
                                    {
                                        vDepends[depthAssets[k]] = cnt + 1;
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(import.assetBundleVariant) || !string.IsNullOrEmpty(import.assetBundleName))
                        {
                            import.assetBundleVariant = null;
                            import.assetBundleName = null;
                            import.SaveAndReimport();
                        }
                    }
                }

                ++startIndex;
                if (isCancel || startIndex >= fileInfoList.Count)
                {

                    if (vDepends.Count > 0)
                    {
                        int depIndex = 0;
                        foreach (var db in vDepends)
                        {
                            depIndex++;
                            EditorUtility.DisplayProgressBar("依赖资源标记中", db.Key, (float)depIndex / (float)vDepends.Count);
                            if (db.Value > 1)
                            {
                                BuildMark(setting, vAllUnUsedAbs, vDirRoot, db.Key, bMark, false);
                            }
                        }
                    }

                    int scrpRef = 0;
                    foreach (var db in vAllScriptPathRef)
                    {
                        EditorUtility.DisplayProgressBar("脚本路径引用资源标记", db, (float)scrpRef / (float)vAllScriptPathRef.Count);
                        BuildMark(setting, vAllUnUsedAbs, vDirRoot, db, bMark, true);
                    }
                    EditorUtility.ClearProgressBar();

                    //! 去除已经没用的依赖 ab 
                    if (bMark)
                    {
                        EditorUtility.DisplayProgressBar("去除已经没用文件的AB标志", "", 0);
                        int progress = 0;
                        foreach (var db in vAllUnUsedAbs)
                        {
                            progress++;
                            EditorUtility.DisplayProgressBar("去除已经没用文件的AB标志", db, (float)progress / (float)vAllUnUsedAbs.Count);
                            import = AssetImporter.GetAtPath(db);
                            if (!string.IsNullOrEmpty(import.assetBundleVariant) || !string.IsNullOrEmpty(import.assetBundleName))
                            {
                                import.assetBundleVariant = null;
                                import.assetBundleName = null;
                                import.SaveAndReimport();
                            }
                        }
                        EditorUtility.ClearProgressBar();
                    }

                    startIndex = 0;
                    AssetDatabase.RemoveUnusedAssetBundleNames();
                    Debug.Log("标记结束:");
                    PublishPanel.WritePublishProgress("MarkAssetBundleAysn-End");
                    if (bMarkEndAction && !isCancel && OnABBuildCallback != null)
                        OnABBuildCallback(setting, bBuildPackage, bOpenExplorer, true, bHotUpdate);
                    break;
                }
            }
            if (bMark && vDepends.Count > 0)
            {
                string mapingFile = setting.GetBuildTargetOutputDir() + "/" + "dep_mapping.txt";
                string strContext = "";
                foreach (var db in vDepends)
                {
                    if (db.Value > 1) strContext += db.Key + "     " + db.Value + "\r\n";
                }
                StreamWriter sw;
                if (!File.Exists(mapingFile)) sw = File.CreateText(mapingFile);
                else sw = new StreamWriter(File.Open(mapingFile, FileMode.OpenOrCreate, FileAccess.Write));

                sw.BaseStream.Position = 0;
                sw.BaseStream.SetLength(0);
                sw.BaseStream.Flush();
                sw.Write(strContext);
                sw.Close();
            }

            return true;
        }
        //------------------------------------------------------
        public static AssetBundleManifest BuildAB(string strOutputDir, BuildAssetBundleOptions option, BuildTarget target, string strEncryptKey = null)
        {
#if UNITY_2019_4_2
            BuildPipeline.SetAssetBundleEncryptKey(strEncryptKey);
#endif
            //使每个object具有唯一不变的hashID, 可用于增量式发布AssetBundle
            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(strOutputDir, option | BuildAssetBundleOptions.DeterministicAssetBundle, target);
            AssetDatabase.RemoveUnusedAssetBundleNames();

            return manifest;
        }
        static void BuildMark(PublishPanel.PublishSetting setting, HashSet<string> vAllUnUsedAbs, List<BuidDir> vDirRoot, string path, bool bMark, bool bCollectMark=true)
        {
            var import = AssetImporter.GetAtPath(path);
            if (import)
            {
                bool bDepMark = bMark;
                if (bDepMark)
                {
                    for (int i = 0; i < vDirRoot.Count; ++i)
                    {
                        if (path.Contains(vDirRoot[i].strDir))
                        {
                            if (!vDirRoot[i].bMark)
                                bDepMark = false;
                            break;
                        }
                    }
                }

                if (bDepMark)
                {
                    bool bFilePathUseAB = setting.singleFileAB;
                    if (bFilePathUseAB)
                    {
                        if (setting.useEncrptyPak)
                        {
                            for (int i = 0; i < setting.buildFileRootPathAbs.Count; ++i)
                            {
                                if (import.assetPath.Contains(setting.buildFileRootPathAbs[i]))
                                {
                                    bFilePathUseAB = false;
                                    break;
                                }
                            }
                        }
                    }
                    bool bSingleAb = false;
                    if (!bFilePathUseAB)
                    {
                        for (int i = 0; i < setting.buildSingleAbs.Count; ++i)
                        {
                            if (import.assetPath.Contains(setting.buildSingleAbs[i]))
                            {
                                bFilePathUseAB = true;
                                bSingleAb = true;
                                break;
                            }
                        }
                    }

                    string strAssignAb = "";
                    if (bFilePathUseAB)
                    {
                        strAssignAb = path.ToLower();
                        if (OnPrepareMarkAbName != null)
                        {
                            string ab = OnPrepareMarkAbName(path);
                            if (!string.IsNullOrEmpty(ab))
                                strAssignAb = ab;
                        }
                    }
                    else
                    {

                        strAssignAb = System.IO.Path.GetDirectoryName(path).ToLower();
                        if (OnPrepareMarkAbName != null)
                        {
                            string ab = OnPrepareMarkAbName(path);
                            if (!string.IsNullOrEmpty(ab))
                                strAssignAb = ab;
                        }
                    }
                    if (!bSingleAb)
                    {
                        for (int i = 0; i < setting.buildCombineAbsGroup.Count; ++i)
                        {
                            if (import.assetPath.Contains(setting.buildCombineAbsGroup[i].pathFile))
                            {
                                strAssignAb = setting.buildCombineAbsGroup[i].strAbName;
                                break;
                            }
                        }
                    }
                    strAssignAb = strAssignAb.Replace("\\", "/");
                    if (setting.abUseIndex) strAssignAb = GetNameToID(strAssignAb).ToString();
                    if (strAssignAb.CompareTo(import.assetBundleName) != 0 || "pkg".CompareTo(import.assetBundleVariant) != 0)
                    {
                        import.assetBundleName = strAssignAb;
                        import.assetBundleVariant = "pkg";
                        import.SaveAndReimport();
                    }
                    vAllUnUsedAbs.Remove(path);
                    if(bCollectMark)
                    {
                        string abName = import.assetBundleName + "." + import.assetBundleVariant;
                        if (OnMarkAbName != null) OnMarkAbName(path, abName);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(import.assetBundleVariant) || !string.IsNullOrEmpty(import.assetBundleName))
                    {
                        import.assetBundleVariant = null;
                        import.assetBundleName = null;
                        import.SaveAndReimport();
                    }
                }
            }
        }
        //------------------------------------------------------
        public static void GetFileList(string basePath, ref List<FileInfoData> fileInfoList, bool bMark)
        {
            if (string.IsNullOrEmpty(basePath))
                return;

            if (!Directory.Exists(basePath))
                return;

            if (fileInfoList == null)
            {
                fileInfoList = new List<FileInfoData>();
            }

            basePath = basePath.Replace("\\", "/");
            if (basePath[basePath.Length - 1] == '/')
                basePath = basePath.Substring(0, basePath.Length - 1);

            var curDirectoryInfo = new DirectoryInfo(basePath);

            var FilesInfo = curDirectoryInfo.GetFiles();

            foreach (var info in FilesInfo)
            {
                if (info.FullName.EndsWith("meta")) continue;
                FileInfoData data = new FileInfoData();
                data.fileInfo = info;
                data.directoryInfo = curDirectoryInfo;
                data.path = basePath;
                data.bMark = bMark;
                data.subFile = basePath + "/" + info.Name;
                fileInfoList.Add(data);
            }

            var childrenDirectories = curDirectoryInfo.GetDirectories();

            for (int i = 0; i < childrenDirectories.Length; ++i)
            {
                var directory = childrenDirectories[i];

                string childPath = string.Format("{0}/{1}", basePath, directory.Name);
                GetFileList(childPath, ref fileInfoList, bMark);
            }
        }
    }
}
