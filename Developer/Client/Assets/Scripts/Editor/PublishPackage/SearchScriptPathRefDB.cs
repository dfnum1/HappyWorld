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
    public class SearchScriptPathRefDB
    {
        public static HashSet<string> Search(string dir)
        {
            HashSet<string> vSearchs = new HashSet<string>();

            string[] assets =string.IsNullOrEmpty(dir)? AssetDatabase.FindAssets("t:GameObject"): AssetDatabase.FindAssets("t:GameObject", new string[] { dir });
            EditorUtility.DisplayProgressBar("搜索预制体中脚本使用路径引用资源", "", 0);
            for (int i =0; i < assets.Length; ++i)
            {
                EditorUtility.DisplayProgressBar("搜索预制体中脚本使用路径引用资源", "", (float)i/(float)assets.Length);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assets[i]));
                if(prefab !=null)
                {
                    CollectActionGraphBinder(vSearchs,prefab.GetComponent<Framework.Core.AActionGraphBinder>());
                }
            }
            EditorUtility.ClearProgressBar();
            return vSearchs;
        }
        //------------------------------------------------------
        static void CollectActionGraphBinder(HashSet<string> vCollect, Framework.Core.AActionGraphBinder binder)
        {
            if (binder == null) return;
            if(binder.playables!=null)
            {
                for(int i =0; i < binder.playables.Length; ++i)
                {
                    var able = binder.playables[i];
                    if(able.clipTracks!=null)
                    {
                        for(int j =0; j < able.clipTracks.Length; ++j)
                        {
                            if(!string.IsNullOrEmpty(able.clipTracks[j].clipFile) )
                            {
                                vCollect.Add(able.clipTracks[j].clipFile);
                            }
                        }
                    }
                }
            }
        }
    }
}
