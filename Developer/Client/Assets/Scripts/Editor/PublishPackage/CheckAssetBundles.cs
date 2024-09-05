#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
namespace TopGame.ED
{
    class CheckAssetBundles
    {
        //------------------------------------------------------
        public static bool CheckABInfiniteDeps(PublishPanel.PublishSetting setting, AssetBundleManifest mainfest)
        {
            if (mainfest == null)
            {
                string outPut = setting.GetBuildOutputRoot(setting.buildTarget);
                AssetBundle assetData = AssetBundle.LoadFromFile(outPut + "/base_pkg/base_pkg");
                if (assetData != null)
                    mainfest = assetData.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            if (mainfest == null)
                return false;

            List<string> vLogs = new List<string>();
            Dictionary<string, HashSet<string>> vDeps = new Dictionary<string, HashSet<string>>();
            var names = AssetDatabase.GetAllAssetBundleNames();
            foreach (var name in names)
            {
                HashSet<string> vHashed = new HashSet<string>();
                CheckDep(mainfest, name, vLogs, vHashed, null);
            }
            foreach (var db in vLogs)
            {
                Debug.LogError(db);
            }
            return vLogs.Count > 0;
        }
        //--------------------------------------------------------
        static void CheckDep(AssetBundleManifest mainfest, string name, List<string> logs, HashSet<string> vHashed, string parentName = null)
        {
            var depends = mainfest.GetDirectDependencies(name);
            if (depends == null || depends.Length<=0) return;

            if(vHashed.Contains(name))
            {
                if(parentName!=null)
                {
                    logs.Add(parentName + "无限递归引用:" + name);
                }
                return;
            }

            vHashed.Add(name);

            for (int i = 0; i < depends.Length; ++i)
            {
                CheckDep(mainfest, depends[i], logs, vHashed, name);
            }
        }
    }
}

#endif