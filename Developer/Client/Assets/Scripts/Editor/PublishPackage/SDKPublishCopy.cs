/********************************************************************
生成日期:	25:7:2019   9:51
类    名: 	SDKPublishCopy
作    者:	HappLI
描    述:	发布包体 SDK 配置
*********************************************************************/

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEditor.Build;
using System.Text;
using TopGame.Core;
using UnityEngine.Rendering;
using UnityEngine.U2D;
using UnityEditor.U2D;
using System.Linq;
using Framework.Core;
using static TopGame.ED.PublishPanel;

namespace TopGame.ED
{
    public class SDKPublishCopy
    {
        static System.Text.UTF8Encoding UTF8WhitoutBom = new System.Text.UTF8Encoding(false);
        internal static void CopySDKToPluginAssets(PublishSetting setting)
        {
            string ExternalDependencyManager = "/ExternalDependencyManager";
            PublishSetting.Platform platform = setting.GetPlatform(setting.buildTarget);
            var defaultSetting = LoadSetting();
            if (defaultSetting == null) defaultSetting = setting;
            var defaultPlaform = defaultSetting.GetPlatform(setting.buildTarget);
            List<string> vMarcs = new List<string>();
            float progress = 0;
            float total = defaultPlaform.copySdks.Count * 2;
            EditorUtility.DisplayProgressBar("检测SDK", "检测sdk", progress);
            //check
            for (int i = 0; i < defaultPlaform.copySdks.Count; ++i)
            {
                if (string.IsNullOrEmpty(defaultPlaform.copySdks[i].pluginDatas)) continue;
                string strSrcRoot = Application.dataPath + defaultPlaform.copySdks[i].pluginDatas + "/";
                string strToRoot = Application.dataPath;
                strSrcRoot = strSrcRoot.Replace("\\", "/");
                strToRoot = strToRoot.Replace("\\", "/");
                string strToDir = strToRoot + "/";
                if (Directory.Exists(strSrcRoot) && Directory.Exists(strToDir))
                {
                    string[] files = Directory.GetFiles(strSrcRoot, "*.*", SearchOption.AllDirectories);
                    List<string> fileSets = new List<string>();
                    for(int j = 0; j < files.Length; ++j)
                    {
                        string strTem = files[j].Replace("\\", "/").Substring(strSrcRoot.Length);
                        fileSets.Add(strToDir + strTem);
                    }
                    for (int j = 0; j < fileSets.Count; ++j)
                    {
                        if (File.Exists(fileSets[j]))
                            File.Delete(fileSets[j]);
                    }
                }
                progress++;
                EditorUtility.DisplayProgressBar("检测去除SDK", defaultPlaform.copySdks[i].sdkMarco, progress / total);
            }
            EditorKits.DeleteDirectory(Application.dataPath + ExternalDependencyManager);

            SDK.SDKSetting sdkSetting = AssetDatabase.LoadAssetAtPath<SDK.SDKSetting>("Assets/DatasRef/System/SDKSetting.asset");
            total = platform.copySdks.Count * 2;

            //copy to
            bool isUseGradleSrc = false;
            List<string> vAndroidMainTemplateGradles = new List<string>();
            List<string> vAndroidGradleTemplatePropsGradles = new List<string>();
            List<string[]> vAndroidManifestImps = new List<string[]>();
            bool bNeedExternDep = false;
            bool bNeedUnitySDK = false;
            Dictionary<string, SDKData> vMarcoSDKs = new Dictionary<string, SDKData>();
            for (int i = 0; i < platform.copySdks.Count; ++i)
            {
                if (!platform.copySdks[i].bUsed) continue;
                vMarcoSDKs[platform.copySdks[i].sdkMarco] = platform.copySdks[i];
                if (!string.IsNullOrEmpty(platform.copySdks[i].pluginDatas))
                {
                    string strSrcRoot = Application.dataPath + platform.copySdks[i].pluginDatas + "/";
                    string strToRoot = Application.dataPath;
                    strSrcRoot = strSrcRoot.Replace("\\", "/");
                    strToRoot = strToRoot.Replace("\\", "/");
                    string strToDir = strToRoot + "/";
                    if (Directory.Exists(strSrcRoot))
                    {
                        List<SDKData.KeyValue> vKeyValues = platform.copySdks[i].keyValues!=null?(new List<SDKData.KeyValue>(platform.copySdks[i].keyValues.ToArray())):new List<SDKData.KeyValue>();
                        vKeyValues.Add(new SDKData.KeyValue() { key = "GAME_APP_ID", value = platform.bundleName });
                        vKeyValues.Add(new SDKData.KeyValue() { key = "GAME_APP_NAME", value = platform.projectName });
                        if(setting.portait)
                            vKeyValues.Add(new SDKData.KeyValue() { key = "GAME_ORIENTATION", value = "portrait" });
                        else
                            vKeyValues.Add(new SDKData.KeyValue() { key = "GAME_ORIENTATION", value = "landscape" });
                        SDK.SDKParam sdk = null;
                        if(sdkSetting!=null) sdk =  sdkSetting.GetSDK(platform.copySdks[i].sdkMarco.Replace("USE_", ""), true);

                        string[] files = Directory.GetFiles(strSrcRoot, "*.*", SearchOption.AllDirectories);
                        for (int j = 0; j < files.Length; ++j)
                        {
                            string srcFile = files[j].Replace("\\", "/");
                            string strTem = srcFile.Substring(strSrcRoot.Length);
                            string strFile = strToDir + strTem;
                            if (IsIngoreSdkRoot(strFile)) continue;
                            string strDir = System.IO.Path.GetDirectoryName(strFile).Replace("\\", "/");
                            if (!Directory.Exists(strDir))
                                Directory.CreateDirectory(strDir);
                            if (File.Exists(strFile)) File.Delete(strFile);

                            if(srcFile.EndsWith(".gradleImp"))
                            {
                                vAndroidMainTemplateGradles.Add(srcFile);
                            }
                            else if (srcFile.EndsWith(".propertiesImp"))
                            {
                                vAndroidGradleTemplatePropsGradles.Add(srcFile);
                            }
                            else if (srcFile.EndsWith(".xmlManifestImp"))
                            {
                                string[] strContent = File.ReadAllLines(srcFile, UTF8WhitoutBom);
                                for(int c =0; c < strContent.Length; ++c)
                                {
                                    for (int k = 0; k < vKeyValues.Count; ++k)
                                    {
                                        SDKData.KeyValue keyValue = vKeyValues[k];
                                        if (string.IsNullOrEmpty(keyValue.key)) continue;
                                        string key = "$(" + keyValue.key + ")";
                                        if (strContent[c].Contains(key))
                                        {
                                            strContent[c] = strContent[c].Replace(key, keyValue.value);
                                        }
                                        else
                                        {
                                            key = "${" + keyValue.key + "}";
                                            if (strContent[c].Contains(key))
                                            {
                                                strContent[c] = strContent[c].Replace(key, keyValue.value);
                                            }
                                        }
                                    }
                                }

                                vAndroidManifestImps.Add(strContent);
                            }
                            else
                            {
                                if (srcFile.EndsWith(".gradle"))
                                {
                                    isUseGradleSrc = true;
                                }
                                File.Copy(srcFile, strFile);
                                if (File.Exists(strFile) && vKeyValues.Count > 0)
                                {
                                    string strExtension = System.IO.Path.GetExtension(strFile);
                                    if (!string.IsNullOrEmpty(strExtension))
                                    {
                                        strExtension = strExtension.ToLower();
                                        if (!strExtension.StartsWith(".")) strExtension = "." + strExtension;
                                        if (strExtension.Contains(".xml") || strExtension.Contains(".plist") || strExtension.Contains(".asset") || strExtension.Contains(".ini") || strExtension.Contains(".mm") || strExtension.Contains(".h") || strExtension.Contains(".json") || strExtension.Contains(".text") || strExtension.Contains(".txt"))
                                        {
                                            string strContent = File.ReadAllText(strFile, UTF8WhitoutBom);
                                            bool bDirty = false;
                                            for (int k = 0; k < vKeyValues.Count; ++k)
                                            {
                                                SDKData.KeyValue keyValue = vKeyValues[k];
                                                if (string.IsNullOrEmpty(keyValue.key)) continue;
                                                string key = "$(" + keyValue.key + ")";
                                                if (strContent.Contains(key))
                                                {
                                                    bDirty = true;
                                                    strContent = strContent.Replace(key, keyValue.value);
                                                }
                                                else
                                                {
                                                    key = "${" + keyValue.key + "}";
                                                    if (strContent.Contains(key))
                                                    {
                                                        bDirty = true;
                                                        strContent = strContent.Replace(key, keyValue.value);
                                                    }
                                                }
                                            }
                                            if (bDirty)
                                            {
                                                File.WriteAllText(strFile, strContent, UTF8WhitoutBom);
                                            }
                                        }
                                    }
                                }
                                if(sdkSetting!=null)
                                {
                                    if(sdk!=null)
                                    {
                                        for(int p =0; p < sdk.Params.Count; ++p)
                                        {
                                            var param = sdk.Params[p];
                                            bool bDo = false;
                                            for (int k = 0; k < vKeyValues.Count; ++k)
                                            {
                                                if (param.label.ToLower() == vKeyValues[k].key.ToLower())
                                                {
                                                    if (param.strValue == null || param.strValue.CompareTo(vKeyValues[k].value) != 0)
                                                    {
                                                        param.strValue = vKeyValues[k].value;
                                                        int nValue = 0;
                                                        if (int.TryParse(vKeyValues[k].value, out nValue))
                                                            param.nValue = nValue;
                                                        bDo = true;
                                                    }
                                                    break;
                                                }
                                            }
                                            if (bDo)
                                            {
                                                sdk.Params[p] = param;
                                                EditorUtility.SetDirty(sdkSetting);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!bNeedExternDep && platform.copySdks[i].needExternalDependency) bNeedExternDep = platform.copySdks[i].needExternalDependency;
                    if (!bNeedUnitySDK && platform.copySdks[i].needUnitySDK) bNeedUnitySDK = platform.copySdks[i].needUnitySDK;
                }

                if(EditorUtility.IsDirty(sdkSetting))
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                }

                if (!string.IsNullOrEmpty(platform.copySdks[i].sdkMarco))
                    vMarcs.Add(platform.copySdks[i].sdkMarco);

                progress++;
                EditorUtility.DisplayProgressBar("检测添加使用SDK", platform.copySdks[i].sdkMarco, progress / total);
            }

            if (vAndroidMainTemplateGradles.Count > 0)
            {
                string strFile;
                if (isUseGradleSrc)
                    strFile = Application.dataPath + "/Plugins/Android/mainTemplate.gradle";
                else strFile = Application.dataPath + "/../Publishs/Android/config/mainTemplate.gradle";
                if (File.Exists(strFile))
                {
                    string strContent = File.ReadAllText(strFile, UTF8WhitoutBom);

                    string strOptionAdds = "";
                    string strApplyPlugin = "";
                    string strClassPath = "";
                    string strLineImps = "";
                    Dictionary<string, string> vReplaceKeys = new Dictionary<string, string>();
                    for (int i = 0; i < vAndroidMainTemplateGradles.Count; ++i)
                    {
                        var lines = File.ReadAllLines(vAndroidMainTemplateGradles[i], UTF8WhitoutBom);

                        string preCommLine = "";
                        List<string> strRangeLining = new List<string>();
                        string beginRangeLiningEnd = "";
                        for (int j = 0; j < lines.Length; ++j)
                        {
                            if (string.IsNullOrEmpty(lines[j].Trim())) continue;
                            if(!string.IsNullOrEmpty(beginRangeLiningEnd))
                            {
                                if (lines[j].Contains(beginRangeLiningEnd))
                                {
                                    if (beginRangeLiningEnd.Contains("//android-options-"))
                                    {
                                        for(int o =0; o < strRangeLining.Count; ++o)
                                        {
                                            if (o == 0) strOptionAdds += strRangeLining[o] + "\r\n";
                                            else strOptionAdds = "\t\t" + strRangeLining[o] + "\r\n";
                                        }
                                    }
                                    else if (beginRangeLiningEnd.Contains("//key-replace-"))
                                    {
                                        for (int o = 0; o < strRangeLining.Count; ++o)
                                        {
                                            string temp = strRangeLining[o].Trim();
                                            if (!string.IsNullOrEmpty(temp))
                                            {
                                                int splitIndex = temp.IndexOf("->");
                                                if (splitIndex > 0 && splitIndex +2 < temp.Length)
                                                {
                                                    vReplaceKeys[temp.Substring(0, splitIndex)] = temp.Substring(splitIndex + 2);
                                                }
                                                splitIndex = temp.IndexOf("=");
                                                if (splitIndex > 0 && splitIndex + 1 < temp.Length)
                                                {
                                                    vReplaceKeys[temp.Substring(0, splitIndex)] = temp.Substring(splitIndex +1);
                                                }
                                            }
                                        }
                                    }
                                    beginRangeLiningEnd = "";
                                }
                                else
                                {
                                    strRangeLining.Add(lines[j].Trim());
                                 //   else strRangeLining += "\t\t" + lines[j].Trim() + "\r\n";
                                }
                                continue;
                            }
                            if(lines[j].Contains("//android-options-begin"))
                            {
                                strRangeLining.Clear();
                                beginRangeLiningEnd = "//android-options-end";
                            }
                            else if (lines[j].Contains("//key-replace-begin"))
                            {
                                strRangeLining.Clear();
                                beginRangeLiningEnd = "//key-replace-end";
                            }
                            else if (lines[j].Contains("implementation "))
                            {
                                if (!string.IsNullOrEmpty(preCommLine)) strLineImps += preCommLine + "\r\n";
                                preCommLine = "";
                                strLineImps += lines[j];
                                strLineImps += "\r\n";
                            }
                            else if (lines[j].Contains("classpath "))
                            {
                                if (!string.IsNullOrEmpty(preCommLine)) strClassPath += preCommLine + "\r\n";
                                preCommLine = "";
                                strClassPath += lines[j];
                                strClassPath += "\r\n";
                            }
                            else if (lines[j].Contains("apply plugin:"))
                            {
                                if (!string.IsNullOrEmpty(preCommLine)) strApplyPlugin += preCommLine + "\r\n";
                                preCommLine = "";
                                strApplyPlugin += lines[j];
                                strApplyPlugin += "\r\n";
                            }
                            else if (lines[j].Trim().StartsWith("//"))
                                preCommLine = lines[j];
                        }
                    }
                    if(!string.IsNullOrEmpty(strLineImps))
                        strContent = strContent.Replace("//IMP_ADDS", strLineImps);
                    if (!string.IsNullOrEmpty(strLineImps))
                        strContent = strContent.Replace("//CLASSPATH", strClassPath);
                    if (!string.IsNullOrEmpty(strApplyPlugin))
                        strContent = strContent.Replace("//APPLYPLUGINS", strApplyPlugin);
                    if (!string.IsNullOrEmpty(strOptionAdds))
                        strContent = strContent.Replace("//OPT_ADDS", strOptionAdds);

                    foreach(var db in vReplaceKeys)
                    {
                        if(strContent.Contains(db.Key))
                        {
                            strContent = strContent.Replace(db.Key, db.Value);
                        }
                    }

                    string toFile = Application.dataPath + "/Plugins/Android/mainTemplate.gradle";
                    File.WriteAllText(toFile, strContent, UTF8WhitoutBom);
                }
            }
            else
            {
                if(!isUseGradleSrc)
                {
                    string toFile = Application.dataPath + "/Plugins/Android/mainTemplate.gradle";
                    if (File.Exists(toFile))
                        File.Delete(toFile);
                }
            }

            if (vAndroidGradleTemplatePropsGradles.Count > 0)
            {
                string strFile = Application.dataPath + "/../Publishs/Android/config/gradleTemplate.properties";
                if (File.Exists(strFile))
                {
                    string strContent = File.ReadAllText(strFile, UTF8WhitoutBom);
                    strContent += "\r\n";
                    for (int i = 0; i < vAndroidGradleTemplatePropsGradles.Count; ++i)
                    {
                        string[] lines = File.ReadAllLines(vAndroidGradleTemplatePropsGradles[i], UTF8WhitoutBom);
                        for (int k = 0; k < lines.Length; ++k)
                        {
                            if (strContent.Contains(lines[k].Trim()))
                                continue;
                            strContent += lines[k] + "\r\n";
                        }
                    }
                    string toFile = Application.dataPath + "/Plugins/Android/gradleTemplate.properties";
                    File.WriteAllText(toFile, strContent, UTF8WhitoutBom);
                }
            }
            else
            {
                string toFile = Application.dataPath + "/Plugins/Android/config/gradleTemplate.properties";
                if (File.Exists(toFile))
                    File.Delete(toFile);
            }

            {
                string strManifestFile = Application.dataPath + "/../Publishs/Android/config/AndroidManifest.xml";
                if (File.Exists(strManifestFile))
                {
                    string[] strContentLines = File.ReadAllLines(strManifestFile, UTF8WhitoutBom);
                    List<string> vUsersPermissions = new List<string>();
                    List<string> vMetaDataDefs = new List<string>();
                    List<string> vApplications = new List<string>();
                    List<string> vActivitys = new List<string>();

                    int findCheck = 0;
                    for (int i =0; i < vAndroidManifestImps.Count; ++i)
                    {
                        for(int k =0; k < vAndroidManifestImps[i].Length; ++k)
                        {
                            string line = vAndroidManifestImps[i][k];
                            switch (findCheck)
                            {
                                case 0: // users permission
                                    {
                                        if (line.Contains("//uses-permission-begin"))
                                        {
                                            findCheck = 1;
                                        }
                                        else if (line.Contains("//meta-data-begin"))
                                        {
                                            findCheck = 2;
                                        }
                                        else if (line.Contains("//application-begin"))
                                        {
                                            findCheck = 3;
                                        }
                                        else if (line.Contains("//activity-begin"))
                                        {
                                            findCheck = 4;
                                        }
                                    }
                                    break;
                                case 1: // users permission
                                    {
                                        if (line.Contains("//uses-permission-end"))
                                        {
                                            findCheck = 0;
                                        }
                                        else if(line.Contains("<uses-permission"))
                                        {
                                            bool bHas = false;
                                            for(int x =0; x < vUsersPermissions.Count; ++x)
                                            {
                                                if(vUsersPermissions[x].Contains(line.Trim()))
                                                {
                                                    bHas = true;
                                                    break;
                                                }
                                            }
                                            if(!bHas) vUsersPermissions.Add("\t" + line);
                                        }
                                    }
                                    break;
                                case 2: // meta data
                                    {
                                        if (line.Contains("//meta-data-end"))
                                        {
                                            findCheck = 0;
                                        }
                                        else if (line.Contains("<meta-data"))
                                        {
                                            bool bHas = false;
                                            for (int x = 0; x < vMetaDataDefs.Count; ++x)
                                            {
                                                if (vMetaDataDefs[x].Contains(line.Trim()))
                                                {
                                                    bHas = true;
                                                    break;
                                                }
                                            }
                                            if (!bHas)
                                                vMetaDataDefs.Add("\t" +line);
                                        }
                                    }
                                    break;
                                case 3: // applcaition
                                    {
                                        if (line.Contains("//application-end"))
                                        {
                                            findCheck = 0;
                                        }
                                        else
                                        {
                                            vApplications.Add("\t" + line);
                                        }
                                    }
                                    break;
                                case 4: // activity
                                    {
                                        if (line.Contains("//activity-end"))
                                        {
                                            findCheck = 0;
                                        }
                                        else
                                            vActivitys.Add("\t" + line);
                                    }
                                    break;
                            }
                        }
                    }

                    List<string> vLines = new List<string>();
                    if(vApplications.Count>0)
                    {
                        int bApplication = 0;
                        for(int i =0; i < strContentLines.Length; ++i)
                        {
                            if (bApplication == 1)
                            {
                                if (strContentLines[i].Contains("<!-- application end -->"))
                                {
                                    vLines.AddRange(vApplications);
                                    vLines.Add(strContentLines[i]);
                                    bApplication = 0;
                                }
                                continue;
                            }
                              
                            if (strContentLines[i].Contains("<!-- application begin -->"))
                            {
                                vLines.Add(strContentLines[i]);
                                bApplication = 1;
                            }
                            else if(bApplication == 0)
                                vLines.Add(strContentLines[i]);
                        }
                    }
                    else
                    {
                        vLines = new List<string>(strContentLines);
                    }
                    //! check commons
                    for(int i =0; i < vLines.Count; ++i)
                    {
                        if(vLines[i].Contains("<uses-permission"))
                        {
                            for (int j = 0; j < vUsersPermissions.Count;)
                            {
                                if (vUsersPermissions[j].Contains(vLines[i].Trim()))
                                {
                                    vUsersPermissions.RemoveAt(j);
                                }
                                else ++j;
                            }
                        }
                        if (vLines[i].Contains("<meta-data"))
                        {
                            for (int j = 0; j < vMetaDataDefs.Count;)
                            {
                                if (vMetaDataDefs[j].Contains(vLines[i].Trim()))
                                {
                                    vMetaDataDefs.RemoveAt(j);
                                }
                                else ++j;
                            }
                        }
                    }
                    
                    //! add permissions
                    if(vUsersPermissions.Count>0)
                    {
                        for (int i = 0; i < vLines.Count; ++i)
                        {
                            if (vLines[i].Contains("<!-- uses-permission add -->"))
                            {
                                vLines.InsertRange(i, vUsersPermissions);
                                break;
                            }
                        }
                    }

                    //! add activity
                    if (vActivitys.Count > 0)
                    {
                        for (int i = 0; i < vLines.Count; ++i)
                        {
                            if (vLines[i].Contains("<!-- activity-add -->"))
                            {
                                vLines.InsertRange(i, vActivitys);
                                break;
                            }
                        }
                    }

                    //! add meta data
                    if (vMetaDataDefs.Count > 0)
                    {
                        for (int i = 0; i < vLines.Count; ++i)
                        {
                            if (vLines[i].Contains("<!-- meta-data-add -->"))
                            {
                                vLines.InsertRange(i, vMetaDataDefs);
                                break;
                            }
                        }
                    }

                    string strContent = "";
                    for (int i = 0; i < vLines.Count; ++i) strContent += vLines[i] + "\r\n";
                    string toFile = Application.dataPath + "/Plugins/Android/AndroidManifest.xml";
                    File.WriteAllText(toFile, strContent, UTF8WhitoutBom);
                }
            }

            if (bNeedExternDep)
            {
                string strSrcRoot = Application.dataPath  + "/../Publishs/SDK/ExternalDependencyManager";
                string strToRoot = Application.dataPath;
                string strToDir = strToRoot + "/";
                if (Directory.Exists(strSrcRoot))
                {
                    string[] files = Directory.GetFiles(strSrcRoot, "*.*", SearchOption.AllDirectories);
                    for (int j = 0; j < files.Length; ++j)
                    {
                        string srcFile = files[j].Replace("\\", "/");
                        string strTem = srcFile.Substring(strSrcRoot.Length);
                        string strFile = strToDir + "/ExternalDependencyManager/" + strTem;
                        string strDir = System.IO.Path.GetDirectoryName(strFile).Replace("\\", "/");
                        if (!Directory.Exists(strDir))
                            Directory.CreateDirectory(strDir);
                        if (File.Exists(strFile))
                            continue;
                        File.Copy(srcFile, strFile);
                    }
                }
            }
            if(bNeedUnitySDK)
            {
                string strSrcFile = Application.dataPath + "/../Publishs/Android/config/UnitySDKs.jar";
                string strToFile = Application.dataPath + "/Plugins/Android/UnitySDKs.jar";
                if (File.Exists(strSrcFile))
                {
                    File.Copy(strSrcFile, strToFile,true);
                }
            }
            else
            {
                string strToFile = Application.dataPath + "/Plugins/Android/UnitySDKs.jar";
                if (File.Exists(strToFile))
                {
                    File.Delete(strToFile);
                }
            }
            EditorUtility.ClearProgressBar();
            if (setting != null)
            {
                SDKData sdkData;
                if (vMarcoSDKs.TryGetValue("USE_PICO", out sdkData))
                {
                    if (setting.buildTarget == BuildTarget.Android)
                    {
                        setting.bIL2CPP = true;
                        setting.armv64 = true;
                        setting.armv7 = false;
                        if (setting.androidSdkVersion < AndroidSdkVersions.AndroidApiLevel26)
                            setting.androidSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
                    }

                    //! 设置MSAA
                    var assets = AssetDatabase.FindAssets("t:GameQuality");
                    if(assets!=null && assets.Length>0)
                    {
                        Data.GameQuality quality = AssetDatabase.LoadAssetAtPath<Data.GameQuality>(AssetDatabase.GUIDToAssetPath(assets[0]));
                        int index = (int)Data.EGameQulity.High;
                        if (quality.Configs != null && index < quality.Configs.Length)
                        {
                            Data.QualityConfig cfg = quality.Configs[index];
                            cfg.MSAA = true;
                            cfg.AntiAliasing = Data.QualityConfig.EAntiSamplingType.Samplingx4;
                            if (cfg.urpAsset)
                                cfg.urpAsset.msaaSampleCount = 4;
                        }
                    }

                    GameObject cameraSytemPrefab = null;
                    var cameraSystems = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/DatasRef/System/CameraSystems" });
                    if(cameraSystems!=null && cameraSystems.Length>0)
                    {
                        for(int i =0; i < cameraSystems.Length; ++i)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(cameraSystems[i]);
                            if(path.Contains("PICOVR"))
                            {
                                cameraSytemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                                break;
                            }
                        }
                    }
                    if(cameraSytemPrefab == null)
                    {
                        Debug.LogError("PICO VR 相机找不到, 请在\"Assets/DatasRef/System/CameraSystems\" 这个目录下添加名为PICOVRCameraSystem.prefab 预制体");
                    }
                    else
                    {
                        var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Startup.unity");
                        if (scene.IsValid())
                        {
                            bool bDirtyScene = false;
                            var roots = scene.GetRootGameObjects();
                            for (int i = 0; i < roots.Length; ++i)
                            {
                                FrameworkMain frameworkMain = roots[i].GetComponent<FrameworkMain>();
                                if (frameworkMain == null) frameworkMain = roots[i].GetComponentInChildren<FrameworkMain>();
                                if (frameworkMain != null)
                                {
                                    if (frameworkMain.VRCameraPrefab != cameraSytemPrefab)
                                    {
                                        frameworkMain.VRCameraPrefab = cameraSytemPrefab;
                                        UnityEditor.EditorUtility.SetDirty(frameworkMain);
                                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
                                        bDirtyScene = true;
                                    }
                                    break;
                                }
                            }
                            if(bDirtyScene)
                                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
                        }
                    }
                }
            }
                
            if (setting!=null && setting.useVR)
            {
                if (!vMarcs.Contains("USE_VR")) vMarcs.Add("USE_VR");
            }

            GameObject BaseEventSystem = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/DatasRef/System/EventSystem/EventSystem.prefab");
            GameObject VREventSystem = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/DatasRef/System/EventSystem/VREventSystem.prefab");
            if (VREventSystem == null)
            {
                Debug.LogError("PICO VR 相机找不到, 请在\"Assets/DatasRef/System/EventSystem\" 这个目录下添加名为VREventSystem.prefab 预制体");
            }
            else
            {
                var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Startup.unity");
                if (scene.IsValid())
                {
                    bool bDirtyScene = false;
                    var roots = scene.GetRootGameObjects();
                    for (int i = 0; i < roots.Length; ++i)
                    {
                        FrameworkMain frameworkMain = roots[i].GetComponent<FrameworkMain>();
                        if (frameworkMain == null) frameworkMain = roots[i].GetComponentInChildren<FrameworkMain>();
                        if (frameworkMain != null)
                        {
                            if(setting!=null && setting.useVR)
                            {
                                if (frameworkMain.eventSystem != VREventSystem)
                                {
                                    frameworkMain.eventSystem = VREventSystem;
                                    bDirtyScene = true;
                                }
                            }
                            else
                            {
                                if (frameworkMain.eventSystem != BaseEventSystem)
                                {
                                    frameworkMain.eventSystem = BaseEventSystem;
                                    bDirtyScene = true;
                                }
                            }
                            if(bDirtyScene)
                            {
                                UnityEditor.EditorUtility.SetDirty(frameworkMain);
                                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
                            }
                            break;
                        }
                    }
                    if (bDirtyScene)
                        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
                }
            }
            AutoMarcros.SetMacros(vMarcs.ToArray(), setting);
        }
    }
}
