/********************************************************************
生成日期:	25:7:2019   9:51
类    名: 	PackagePanel
作    者:	HappLI
描    述:	包体信息
*********************************************************************/

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEditor.Build;
using System.Text;
using TopGame.Core;
using UnityEditor.IMGUI.Controls;
using Excel;

namespace TopGame.ED
{
    public class ExportAssetToCsv
    {
        //------------------------------------------------------
        class TabInfo
        {
            public string file;
            public long size;
            public bool rw;
            public bool mipmap;
            public ModelImporterMeshCompression mehsCopression;
            public bool optimizePolygon;
            public bool optimizeVertex;
            public UnityEngine.Object asset;
        }
        //------------------------------------------------------
        static TabInfo BuildTabInfo(Dictionary<string, List<string>> vHashed, string parent, string file)
        {
            List<string> usedBy;
            if (!vHashed.TryGetValue(file, out usedBy))
            {
                usedBy = new List<string>();
                vHashed.Add(file, usedBy);
                UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(file);

                TabInfo tab = new TabInfo();
                tab.file = file;
                tab.asset = asset;
                tab.size = EditorKits.GetStorgeMemory(asset);
                tab.mipmap = EditorKits.IsTextureMipMap(asset);
                tab.rw = EditorKits.IsReadWriteAble(asset);
                tab.optimizeVertex = EditorKits.IsOptimizeMeshVertices(asset);
                tab.optimizePolygon = EditorKits.IsOptimizeMeshPolygon(asset);
                tab.mehsCopression = EditorKits.CompressMesh(asset);

                return tab;
            }
            if(!string.IsNullOrEmpty(parent))
                usedBy.Add(parent);
            return null;
        }
        //------------------------------------------------------
        public static void ExportAssets(List<TreeAssetView.ItemData> vDatas)
        {
            string strDir = EditorUtility.SaveFolderPanel("导出目录", Application.dataPath+"/../../", "AssetExports");
            List<TabInfo> vTextures = new List<TabInfo>();
            List<TabInfo> vMeshs = new List<TabInfo>();
            List<TabInfo> vAudios = new List<TabInfo>();
            List<TabInfo> vClips = new List<TabInfo>();
            EditorUtility.DisplayProgressBar("导出", "", 0);

            Dictionary<string, List<string>> vHashed = new Dictionary<string, List<string>>();
            for(int i = 0; i < vDatas.Count; ++i)
            {
                EditorUtility.DisplayProgressBar("导出", "", (float)i/(float)vDatas.Count);
                PackagePanel.FileItem item = vDatas[i] as PackagePanel.FileItem;
                if (item.itemType == PackagePanel.EItemType.File)
                {
                    if (item.assetBundle == null)
                    {
                        item.assetBundle = PackagePanel.FindAssetBunlde(item.path, item.shortFile);
                    }
                    if(item.assetBundle)
                    {
                        string[] assets = item.assetBundle.GetAllAssetNames();
                        for (int j = 0; j < assets.Length; ++j)
                        {
                            string[] depths = AssetDatabase.GetDependencies(assets[j]);
                            if (depths != null)
                            {
                                for (int k = 0; k < depths.Length; ++k)
                                {
                                    TabInfo tab = BuildTabInfo(vHashed, assets[j], depths[k]);
                                    if(tab!=null)
                                    {
                                        {
                                            Texture convertObj = tab.asset as Texture;
                                            if (convertObj != null)
                                            {
                                                vTextures.Add(tab);
                                            }
                                        }
                                        {
                                            AudioClip convertObj = tab.asset as AudioClip;
                                            if (convertObj != null)
                                            {
                                                vAudios.Add(tab);
                                            }
                                        }
                                        {
                                            AnimationClip convertObj = tab.asset as AnimationClip;
                                            if (convertObj != null)
                                            {
                                                vClips.Add(tab);
                                            }
                                        }
                                        {
                                            string convertObj = tab.file.ToLower();
                                            if (convertObj.Contains(".fbx"))
                                            {
                                                vMeshs.Add(tab);
                                            }
                                        }
                                    }
                                }
                            }

                            {
                                TabInfo tab = BuildTabInfo(vHashed, null, assets[j]);
                                if (tab != null)
                                {
                                    {
                                        Texture convertObj = tab.asset as Texture;
                                        if (convertObj != null)
                                        {
                                            vTextures.Add(tab);
                                        }
                                    }
                                    {
                                        AudioClip convertObj = tab.asset as AudioClip;
                                        if (convertObj != null)
                                        {
                                            vAudios.Add(tab);
                                        }
                                    }
                                    {
                                        AnimationClip convertObj = tab.asset as AnimationClip;
                                        if (convertObj != null)
                                        {
                                            vClips.Add(tab);
                                        }
                                    }
                                    {
                                        string convertObj = tab.file.ToLower();
                                        if (convertObj.Contains(".fbx"))
                                        {
                                            vMeshs.Add(tab);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();


            if (!Directory.Exists(strDir))
            {
                Directory.CreateDirectory(strDir);
            }

            //texture
            {
                EditorUtility.DisplayProgressBar("导出所有纹理", "", 0);
                string textures = strDir + "/textures.csv";
                if (File.Exists(textures)) File.Delete(textures);
                vTextures.Sort((TabInfo t0, TabInfo t1) => { return (int)(t1.size - t0.size); });
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("文件路径,大小,mipmap,read/write,被使用次数,详情");
                for(int i = 0; i <vTextures.Count; ++i)
                {
                    List<string> vUsed;
                    vHashed.TryGetValue(vTextures[i].file, out vUsed);
                    if (vUsed == null) vUsed = new List<string>();
                    List<string> findUsed = FindAssetsReferenceTool.FindByUsedList(vTextures[i].file);
                    if (findUsed != null) vUsed.AddRange(findUsed);

                    int usedBy = vUsed != null ? vUsed.Count : 0;
                    string strDetial = "\"";
                    for(int j = 0; j < vUsed.Count; ++j)
                    {
                        strDetial += vUsed[j] + "\r\n";
                    }
                    strDetial += "\"";
                    sb.AppendFormat("{0},{1},{2},{3},{4},{5}\r\n", vTextures[i].file, Base.Util.FormBytes(vTextures[i].size), vTextures[i].mipmap, vTextures[i].rw, usedBy, strDetial);
                }
                if (System.IO.File.Exists(textures))
                    System.IO.File.Delete(textures);
                System.IO.FileStream fs = new System.IO.FileStream(textures, System.IO.FileMode.OpenOrCreate);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.Write(sb.ToString());
                sw.Close();
            }

            //audio
            {
                EditorUtility.DisplayProgressBar("导出所有音效", "", 0);
                string textures = strDir + "/audios.csv";
                if (File.Exists(textures)) File.Delete(textures);
                vAudios.Sort((TabInfo t0, TabInfo t1) => { return (int)(t1.size - t0.size); });
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("文件路径,大小,被使用次数,详情");
                for (int i = 0; i < vAudios.Count; ++i)
                {
                    List<string> vUsed;
                    vHashed.TryGetValue(vAudios[i].file, out vUsed);
                    if (vUsed == null) vUsed = new List<string>();
                    List<string> findUsed = FindAssetsReferenceTool.FindByUsedList(vAudios[i].file);
                    if (findUsed != null) vUsed.AddRange(findUsed);
                    int usedBy = vUsed != null ? vUsed.Count : 0;

                    string strDetial = "\"";
                    for (int j = 0; j < vUsed.Count; ++j)
                    {
                        strDetial += vUsed[j] + "\r\n";
                    }
                    strDetial += "\"";
                    sb.AppendFormat("{0},{1},{2},{3}\r\n", vAudios[i].file, Base.Util.FormBytes(vAudios[i].size), usedBy, strDetial);
                }
                if (System.IO.File.Exists(textures))
                    System.IO.File.Delete(textures);
                System.IO.FileStream fs = new System.IO.FileStream(textures, System.IO.FileMode.OpenOrCreate);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.Write(sb.ToString());
                sw.Close();
            }

            //mesh
            {
                EditorUtility.DisplayProgressBar("导出所有Mesh", "", 0);
                string textures = strDir + "/meshs.csv";
                if (File.Exists(textures)) File.Delete(textures);
                vMeshs.Sort((TabInfo t0, TabInfo t1) => { return (int)(t1.size - t0.size); });
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("文件路径,大小,read/write,mehsCopression,optimizePolygon,optimizeVertex,被使用次数,详情");
                for (int i = 0; i < vMeshs.Count; ++i)
                {
                    List<string> vUsed;
                    vHashed.TryGetValue(vMeshs[i].file, out vUsed);
                    if (vUsed == null) vUsed = new List<string>();
                    List<string> findUsed = FindAssetsReferenceTool.FindByUsedList(vMeshs[i].file);
                    if (findUsed != null) vUsed.AddRange(findUsed);
                    int usedBy = vUsed != null ? vUsed.Count : 0;
                    string strDetial = "\"";
                    for (int j = 0; j < vUsed.Count; ++j)
                    {
                        strDetial += vUsed[j] + "\r\n";
                    }
                    strDetial += "\"";
                    sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7}\r\n", vMeshs[i].file, Base.Util.FormBytes(vMeshs[i].size), vMeshs[i].rw, vMeshs[i].mehsCopression, vMeshs[i].optimizePolygon, vMeshs[i].optimizeVertex, usedBy, strDetial);
                }
                if (System.IO.File.Exists(textures))
                    System.IO.File.Delete(textures);
                System.IO.FileStream fs = new System.IO.FileStream(textures, System.IO.FileMode.OpenOrCreate);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.Write(sb.ToString());
                sw.Close();
            }

            //animclip
            {
                EditorUtility.DisplayProgressBar("导出所有Clip", "", 0);
                string textures = strDir + "/clips.csv";
                if (File.Exists(textures)) File.Delete(textures);
                vClips.Sort((TabInfo t0, TabInfo t1) => { return (int)(t1.size - t0.size); });
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("文件路径,大小,被使用次数,详情");
                for (int i = 0; i < vClips.Count; ++i)
                {
                    List<string> vUsed;
                    vHashed.TryGetValue(vClips[i].file, out vUsed);
                    if (vUsed == null) vUsed = new List<string>();
                    List<string> findUsed = FindAssetsReferenceTool.FindByUsedList(vClips[i].file);
                    if (findUsed != null) vUsed.AddRange(findUsed);
                    int usedBy = vUsed != null ? vUsed.Count : 0;
                    string strDetial = "\"";
                    for (int j = 0; j < vUsed.Count; ++j)
                    {
                        strDetial += vUsed[j] + "\r\n";
                    }
                    strDetial += "\"";
                    sb.AppendFormat("{0},{1},{2},{3}\r\n", vClips[i].file, Base.Util.FormBytes(vClips[i].size), usedBy, strDetial);
                }
                if (System.IO.File.Exists(textures))
                    System.IO.File.Delete(textures);
                System.IO.FileStream fs = new System.IO.FileStream(textures, System.IO.FileMode.OpenOrCreate);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.Write(sb.ToString());
                sw.Close();
            }
            EditorUtility.ClearProgressBar();
        }
    }
}
