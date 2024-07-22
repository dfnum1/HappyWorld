/********************************************************************
生成日期:	24:7:2019   11:12
类    名: 	AutoMarcros
作    者:	HappLI
描    述:	自动定义
*********************************************************************/
using System.Collections.Generic;
using System.IO;
using TopGame.Base;
using TopGame.Data;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public static class AutoMarcros
    {
        public static string[] VersionMacros = new string[]
        {
            "USE_FMOD",
            "UNITY_TIMELINE_EXIST",
            //"USE_PICO",
            //"USE_VR",
          //  "USE_DIYLEVEL"
        //   "USE_HOTCODE",
       //     "UNITY_POST_PROCESSING_STACK_V2",
        };
        //------------------------------------------------------
        public static void SetMacros(string[] macros, PublishPanel.PublishSetting setting = null)
        {
            string defineCommand = "";

            HashSet<string> vSets = new HashSet<string>();
            for (int i = 0; i < AutoMarcros.VersionMacros.Length; i++)
            {
                defineCommand += AutoMarcros.VersionMacros[i] + ";";
                vSets.Add(AutoMarcros.VersionMacros[i].ToLower());
            }
            if(macros!=null)
            {
                for(int i= 0; i < macros.Length; ++i)
                {
                    if (!string.IsNullOrEmpty(macros[i]))
                    {
                        defineCommand += macros[i] + ";";
                        vSets.Add(macros[i].ToLower());
                    }
                }
            }
            if(setting == null) setting = PublishPanel.LoadSetting();
            if(setting!=null)
            {
                PublishPanel.PublishSetting.Platform platform = setting.GetPlatform(EditorUserBuildSettings.activeBuildTarget);
                if(platform!=null)
                {
                    if (platform.copySdks != null)
                    {
                        string strToRoot = Application.dataPath + "/";
                        for (int i = 0; i < platform.copySdks.Count; ++i)
                        {
                            if (!platform.copySdks[i].bUsed || string.IsNullOrEmpty(platform.copySdks[i].sdkMarco)) continue;

                            string strSrcRoot = Application.dataPath + platform.copySdks[i].pluginDatas + "/";

                            bool bRealUsed = false;
                            if (vSets.Contains(platform.copySdks[i].sdkMarco.ToLower())) continue;
                            if (Directory.Exists(strSrcRoot) && Directory.Exists(strToRoot))
                            {
                                string[] files = Directory.GetFiles(strSrcRoot, "*.*", SearchOption.AllDirectories);
                                if(files.Length>0)
                                {
                                    int totalCnt = 0;
                                    int existCnt = 0;
                                    for (int j = 0; j < files.Length; ++j)
                                    {
                                        string strTem = files[j].Replace("\\", "/").Substring(strSrcRoot.Length);
                                        if (strTem.EndsWith(".gradleImp")|| strTem.EndsWith(".propertiesImp") || strTem.EndsWith(".xmlManifestImp")) continue;
                                        if (PublishPanel.IsIngoreSdkRoot(strTem)) continue;
                                        totalCnt++;
                                        if (File.Exists(strToRoot + strTem))
                                        {
                                            existCnt++;
                                        }
                                    }
                                    if (existCnt >= totalCnt*3/4)
                                        bRealUsed = true;
                                }
                            }
                            if(bRealUsed)
                            {
                                defineCommand += platform.copySdks[i].sdkMarco + ";";
                                vSets.Add(platform.copySdks[i].sdkMarco.ToLower());
                            }
                        }
                    }
                }
                
                if(setting.showFps)
                {
                    if (!vSets.Contains("USE_SHOWFPS"))
                    {
                        defineCommand += "USE_SHOWFPS" + ";";
                        vSets.Add("USE_SHOWFPS");
                    }
                }
                if (setting.reportView)
                {
                    if (!vSets.Contains("USE_REPORTVIEW"))
                    {
                        defineCommand += "USE_REPORTVIEW" + ";";
                        vSets.Add("USE_REPORTVIEW");
                    }
                }
                if (setting.gmPanel)
                {
                    if (!vSets.Contains("USE_GMCONSOLE"))
                    {
                        defineCommand += "USE_GMCONSOLE" + ";";
                        vSets.Add("USE_GMCONSOLE");
                    }
                }
                if (setting.antiAddiction)
                {
                    if (!vSets.Contains("USE_ANTIADDICTION"))
                    {
                        defineCommand += "USE_ANTIADDICTION" + ";";
                        vSets.Add("USE_ANTIADDICTION");
                    }
                }
                if (setting.hotType == PublishPanel.EHotType.InjectFix)
                {
                    if (!vSets.Contains("USE_INJECTFIX"))
                    {
                        defineCommand += "USE_INJECTFIX" + ";";
                        vSets.Add("USE_INJECTFIX");
                    }
                }
#if UNITY_2020_3_OR_NEWER
                else if(setting.hotType == PublishPanel.EHotType.HybridCLR)
                {
                    PlayerSettings.SetApiCompatibilityLevel(EditorUserBuildSettings.selectedBuildTargetGroup, ApiCompatibilityLevel.NET_4_6);
                    if (!vSets.Contains("USE_HYBRIDCLR"))
                    {
                        defineCommand += "USE_HYBRIDCLR" + ";";
                        vSets.Add("USE_HYBRIDCLR");
                    }
                }
#endif
                else if (setting.hotType == PublishPanel.EHotType.ILRuntime)
                {
                    if (!vSets.Contains("USE_ILRUNTIME"))
                    {
                        defineCommand += "USE_ILRUNTIME" + ";";
                        vSets.Add("USE_ILRUNTIME");
                    }
                }

                if (setting.useVR)
                {
                    if (!vSets.Contains("USE_VR"))
                    {
                        defineCommand += "USE_VR" + ";";
                        vSets.Add("USE_VR");
                        ApplayMarco("USE_VR");
                    }
                }
            }
            bool bRefresh = false;
            string curDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if(curDefine.CompareTo(defineCommand) != 0)
            {
                if (setting != null)
                {
                    PublishPanel.CopyMarcoToProject(setting, vSets);
                }
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defineCommand);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, defineCommand);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defineCommand);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defineCommand);

                bRefresh = true;
            }

            if(EditorUserBuildSettings.selectedBuildTargetGroup == BuildTargetGroup.iOS)
            {
                var assets = AssetDatabase.FindAssets("t:GameQuality");
                if (assets != null && assets.Length > 0)
                {
                    Data.GameQuality quality = AssetDatabase.LoadAssetAtPath<Data.GameQuality>(AssetDatabase.GUIDToAssetPath(assets[0]));
                    int index = (int)Data.EGameQulity.High;
                    if (quality.Configs != null && index < quality.Configs.Length)
                    {
                        Data.QualityConfig cfg = quality.Configs[index];
                        cfg.MSAA = false;
                        cfg.AntiAliasing = Data.QualityConfig.EAntiSamplingType.Disable;
                        if (cfg.urpAsset)
                        {
                            if (cfg.urpAsset.msaaSampleCount != 0 || cfg.urpAsset.supportsCameraDepthTexture == true)
                            {
                                cfg.urpAsset.supportsCameraDepthTexture = false;
                                cfg.urpAsset.msaaSampleCount = 0;
                                EditorUtility.SetDirty(cfg.urpAsset);
                            }
                        }
                        EditorUtility.SetDirty(quality);
                    }
                    bRefresh = true;
                }
            }
            if(bRefresh)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
        public static void ApplayMarco(string marco)
        {
            if (marco.CompareTo("USE_VR") == 0)
            {
                string asmdefFile = Application.dataPath + "/Scripts/MainScripts/MainScripts.asmdef";
                if (File.Exists(asmdefFile))
                {
                    string strContext = File.ReadAllText(asmdefFile, System.Text.Encoding.UTF8);
                    if (strContext.Contains("\"Framework\","))
                    {
                        bool bDirty = false;
                        string strRefs = "\"Framework\",\r\n";
                        if (!strContext.Contains("Unity.XR.Interaction.Toolkit"))
                        {
                            strRefs += "\"Unity.XR.Interaction.Toolkit\",\r\n";
                            bDirty = true;
                        }
                        if (bDirty)
                        {
                            strContext = strContext.Replace("\"Framework\",", strRefs);
                            File.WriteAllText(asmdefFile, strContext, System.Text.Encoding.UTF8);
                        }
                    }
                }
            }
            if(marco.CompareTo("USE_PICO") == 0)
            {
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    var PackageManagerWindow = assembly.GetType("UnityEditor.PackageManager.UI.PackageManagerWindow");
                    if (PackageManagerWindow != null)
                    {
                        var realType = assembly.GetType("UnityEditor.PackageManager.UI.PackageDatabase");
                        if(realType!=null)
                        {
                            var propertyN = realType.GetMethod("get_instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
                            if (propertyN != null)
                            {
                                var obj = propertyN.Invoke(null, null);
                                var AddByPath = realType.GetMethod("InstallFromPath", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                                if (AddByPath != null)
                                {
                                    //  AddByPath.Invoke(obj, );
                                }

                                var PackageManagerWindowAnalytics = assembly.GetType("UnityEditor.PackageManager.UI.PackageManagerWindowAnalytics");
                                if (PackageManagerWindowAnalytics != null)
                                {
                                    var SendEvent = PackageManagerWindowAnalytics.GetMethod("SendEvent", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
                                    //      if (SendEvent != null) SendEvent.Invoke(null, new System.Object[] { "addFromDisk", null });
                                }
                            }
                        }
                        
                        break;
                    }
                }
            }
        }
    }
}

