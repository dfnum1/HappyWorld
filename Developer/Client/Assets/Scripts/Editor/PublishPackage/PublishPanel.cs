/********************************************************************
生成日期:	25:7:2019   9:51
类    名: 	PublishPackage
作    者:	HappLI
描    述:	发布包体
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

namespace TopGame.ED
{
    public class PublishPanel
    {
        public static string[] INGORE_SDK_ROOT = { "Tools", "Documents", "OthConfigs" };
        public static bool IsIngoreSdkRoot(string path)
        {
            for (int i = 0; i < INGORE_SDK_ROOT.Length; ++i)
            {
                if (path.Contains("/" + INGORE_SDK_ROOT[i] + "/"))
                    return true;
            }
            return false;
        }
        struct SplitPath
        {
            public string split0;
            public string split1;
        }
        public enum EHotType
        {
            [Framework.Plugin.PluginDisplay("支持图形脚本热更")] Default = 0,
            [Framework.Plugin.PluginDisplay("支持代码注入热更")] InjectFix,
            [Framework.Plugin.PluginDisplay("ILRuntime")] ILRuntime,
#if UNITY_2020_3_OR_NEWER
            [Framework.Plugin.PluginDisplay("程序集dll热更")] HybridCLR,
#endif
        }
        public enum ESpriteAtlasType
        {
            Packer_Tag,
            Sprite_Atlas,
        }
        [System.Serializable]
        public struct SDKData
        {
            [System.Serializable]
            public struct KeyValue
            {
                public string key;
                public string value;
                public string desc; //说明
            }
            public string sdkMarco;
            public string pluginDatas;
            public bool bUsed;
            public bool needExternalDependency;
            public bool needUnitySDK;
            [System.NonSerialized]
            public bool bExpand;
            public List<KeyValue> keyValues;
        }

        [System.Serializable]
        public struct MarcoData
        {
            public string marco;
            public string pluginDatas;
        }

        [System.Serializable]
        public struct PakData
        {
            public string srcDir;
            public string toDir;
            public string pakTag;
        }

        [System.Serializable]
        public struct PathFormTo
        {
            public string from;
            public string to;
        }

        [System.Serializable]
        public class A_BConfig
        {
            public string name;
            public struct ABMap
            {
                public string srcFile;
                public string strMapTo;
            }
            public List<ABMap> mapping = new List<ABMap>();
        }

        [System.Serializable]
        public class AbGroup
        {
            public string strAbName;
            public string pathFile;
        }

        [System.Serializable]
        public class PublishSetting
        {
            [System.NonSerialized]
            public string settingTag = "";

            [System.Serializable]
            public class Platform
            {
                public BuildTarget target = BuildTarget.Android;
                public string projectName = "JSJ";
                public string projectExportName = "JSJ";
                public string bundleName = "com.stupiddog.studio.jsj";
                public string version = "1.0.0";
                public int versionCode = 1;

                public string provisioning_profiler_appkey = "94afe52f-4af0-4b3f-9135-49f689e7a2dd";
                public string provisioning_profiler_name = "stupiddog_run_dev";
                public string development_teamKey = "8J9G7947T9";

                public SystemLanguage defaultLanguage = SystemLanguage.Unknown;

                public int package_size_kb = 16384;
                public List<string> ignore_path = new List<string>();
                public List<string> ignore_suffix = new List<string>();
                public List<string> ignore_file = new List<string>();
                public List<SDKData> copySdks = new List<SDKData>();

                public List<string> shadervariants = new List<string>();

                public bool IsUsedSDK(string sdk)
                {
                    for (int i = 0; i < copySdks.Count; ++i)
                    {
                        if (copySdks[i].sdkMarco.CompareTo(sdk) == 0)
                            return copySdks[i].bUsed;
                    }
                    return false;
                }

                public string GetKeyValue(string sdkMarco, string key, string strDefualt = null)
                {
                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(sdkMarco)) return strDefualt;
                    for (int i = 0; i < copySdks.Count; ++i)
                    {
                        if (copySdks[i].bUsed && copySdks[i].sdkMarco.Contains(sdkMarco))
                        {
                            for (int j = 0; j < copySdks[i].keyValues.Count; ++j)
                            {
                                if (copySdks[i].keyValues[j].key.Contains(key))
                                    return copySdks[i].keyValues[j].value;
                            }
                            break;
                        }
                    }
                    return strDefualt;
                }
            }
            [System.Serializable]
            public class ServerUrl
            {
                public string name;
                public string sreverUrl;
                public string uploadUrl;
                public bool bUsed;
            }
            public List<string> legacyFlagSets = new List<string>();
            public List<string> particleSets = new List<string>();
            public List<string> buildDirs = new List<string>();
            public List<string> unbuildDirs = new List<string>();
            public List<string> buildDllPakDirs = new List<string>();
            public List<PakData> copyToStreamDirs = new List<PakData>();

            public List<string> buildSingleAbs = new List<string>();
            public List<string> spriteUnPak = new List<string>();
            public List<string> includeShaders = new List<string>();
            public List<string> buildFileRootPathAbs = new List<string>();
            public List<AbGroup> buildCombineAbsGroup = new List<AbGroup>();

            public bool debug = true;
            public bool profiler = true;
            public bool depthPrifiling = false;
            public bool scriptDebug = false;
            public bool waitForManagerDebuger = false;
            public bool autoConnectProfiler = false;
            public bool offline = true;
            public bool delPrePackageDir = false;
            public bool bIL2CPP = false;
            public bool armv64 = true;
            public bool armv7 = true;
            public AndroidSdkVersions androidSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
            public bool obb = false;
            public bool dateName = false;
            public bool portait = true;
            public bool versionCheck = false;
            public bool useEncrptyPak = false;
            public bool convertASTC = false;
            public bool sceneStreamAb = false;
            public bool streamingMipmaps = true;
            public bool globalMetaDataEncrypt = false;
            public bool popPrivacy = false;
            public string channelLabel = "";
            public bool useVR = false;

            public List<MarcoData> marcoDatas = new List<MarcoData>();

            public EHotType hotType = EHotType.Default;

            public bool showFps = true;
            public bool reportView = true;
            public bool gmPanel = true;
            public bool antiAddiction = false;

            public ESpriteAtlasType spritePacker = ESpriteAtlasType.Packer_Tag;

            public bool abUseIndex = false;
            public bool dependMarkAB = false;
            public bool singleFileAB = false;

            public List<ServerUrl> serverUrlList = new List<ServerUrl>();
            //public string projectName="JSJ";
            //public string bundleName = "com.stupiddog.studio.jsj";
            //public string version = "1.0.0";
            //public int versionCode = 1;

            public string versionServerAddress = "";

            public string strEncryptKey = "jsj123^*456{][sd";
            public BuildTarget buildTarget;
            public BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.ChunkBasedCompression;
            public string bundleExportDir = "/../Publishs/Packages";

            public bool copyToStreamAsset = false;
            public bool buildAutoRun = false;

            public List<Platform> platforms = new List<Platform>();
            public int[] pak_encrtpys = new int[] { 10, 20, 50, 40 };
            public List<string> unEncryptSuffix = new List<string>();

            public List<A_BConfig> A_BAssets = new List<A_BConfig>();

            [System.NonSerialized]
            bool m_bInited = false;
            Dictionary<BuildTarget, Platform> m_vPlatforms = new Dictionary<BuildTarget, Platform>();

            public void Init()
            {
                if (m_bInited) return;
                m_bInited = true;
                for (int i = 0; i < platforms.Count; ++i)
                {
                    m_vPlatforms[platforms[i].target] = platforms[i];
                }
            }

            public void CanMarker(string strPath, ref bool bMarkAb)
            {
                if (bMarkAb)
                {
                    if (unbuildDirs != null)
                    {
                        bool bInAbForce = false;
                        if (buildDirs != null)
                        {
                            for (int i = 0; i < buildDirs.Count; ++i)
                            {
                                int index = buildDirs[i].IndexOf("/xxx/");
                                if (index > 0)
                                {
                                    int after = index + "/xxx/".Length;
                                    string split0 = buildDirs[i].Substring(0, index) + "/";
                                    string split1 = "/" + buildDirs[i].Substring(after, buildDirs[i].Length - after);
                                    if (strPath.Contains(split0) && strPath.Contains(split1))
                                    {
                                        bInAbForce = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (strPath.Contains(buildDirs[i]))
                                    {
                                        bInAbForce = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (!bInAbForce)
                        {
                            for (int i = 0; i < unbuildDirs.Count; ++i)
                            {
                                if (strPath.Contains(unbuildDirs[i]))
                                {
                                    bMarkAb = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (buildDirs != null)
                    {
                        for (int i = 0; i < buildDirs.Count; ++i)
                        {
                            if (strPath.Contains(buildDirs[i]))
                            {
                                bMarkAb = true;
                                break;
                            }
                        }
                    }
                }
            }

            public bool CanDepMarker(string strPath)
            {
                if (unbuildDirs != null)
                {
                    for (int i = 0; i < unbuildDirs.Count; ++i)
                    {
                        if (strPath.Contains(unbuildDirs[i]))
                        {
                            if (buildDirs != null)
                            {
                                for (int j = 0; j < buildDirs.Count; ++j)
                                {
                                    if (strPath.Contains(buildDirs[j]))
                                    {
                                        return true;
                                    }
                                }
                            }
                            return false;
                        }
                    }
                }
                return true;
            }

            public Platform GetPlatform(BuildTarget target)
            {
                Init();
                if (!m_vPlatforms.ContainsKey(target))
                {
                    m_vPlatforms.Add(target, new Platform() { target = target });
                    platforms.Clear();
                    foreach (var db in m_vPlatforms)
                    {
                        platforms.Add(db.Value);
                    }
                }

                return m_vPlatforms[target];
            }

            public string GetTag(string defaultTag = "default")
            {
                if (string.IsNullOrEmpty(settingTag)) return defaultTag;
                return settingTag;
            }

            public string GetBuildOutputRoot(BuildTarget target = BuildTarget.NoTarget)
            {
                if (target == BuildTarget.NoTarget) target = buildTarget;
                string outPut = Application.dataPath + bundleExportDir.Replace("\\", "/");
                if (outPut[outPut.Length - 1] == '/') outPut += buildTarget;
                else outPut += "/" + buildTarget;

                string tempTag = GetTag();
                if (string.IsNullOrEmpty(tempTag)) tempTag = "default";
                outPut += "/" + tempTag;
                return outPut;
            }

            public string GetBuildTargetOutputDir(BuildTarget target = BuildTarget.NoTarget)
            {
                if (target == BuildTarget.NoTarget) target = buildTarget;
                string outPut = Application.dataPath + bundleExportDir.Replace("\\", "/");

                string tempTag = GetTag();
                if (string.IsNullOrEmpty(tempTag))
                {
                    if (outPut[outPut.Length - 1] == '/') outPut += buildTarget + "/base_pkg";
                    else outPut += "/" + buildTarget + "/base_pkg";
                }
                else
                {
                    if (outPut[outPut.Length - 1] == '/') outPut += buildTarget + "/" + tempTag + "/base_pkg";
                    else outPut += "/" + buildTarget + "/" + tempTag + "/base_pkg";
                }
                return outPut;
            }

            public string GetBuildTargetEncrtpyDir(BuildTarget target = BuildTarget.NoTarget)
            {
                if (target == BuildTarget.NoTarget) target = buildTarget;
                string outPut = Application.dataPath + bundleExportDir.Replace("\\", "/");
                string tempTag = GetTag();
                if (string.IsNullOrEmpty(tempTag))
                {
                    if (outPut[outPut.Length - 1] == '/') outPut += buildTarget + "/encrpty_packages";
                    else outPut += "/" + buildTarget + "/encrpty_packages";
                }
                else
                {
                    if (outPut[outPut.Length - 1] == '/') outPut += buildTarget + "/" + tempTag + "/encrpty_packages";
                    else outPut += "/" + buildTarget + "/" + tempTag + "/encrpty_packages";
                }

                return outPut;
            }

            public string GetEncrtpyPackSuffix(BuildTarget target = BuildTarget.NoTarget)
            {
                //    if (target == BuildTarget.Android) return "pak";
                return "pak";
            }

            public string GetBuildTargetExtractDir(BuildTarget target = BuildTarget.NoTarget)
            {
                if (target == BuildTarget.NoTarget) target = buildTarget;
                string outPut = Application.dataPath + bundleExportDir.Replace("\\", "/");
                string tempTag = GetTag();
                if (string.IsNullOrEmpty(tempTag))
                {
                    if (outPut[outPut.Length - 1] == '/') outPut += buildTarget + "/extract_res";
                    else outPut += "/" + buildTarget + "/extract_res";
                }
                else
                {
                    if (outPut[outPut.Length - 1] == '/') outPut += buildTarget + "/" + tempTag + "/extract_res";
                    else outPut += "/" + buildTarget + "/" + tempTag + "/extract_res";
                }

                return outPut;
            }

            public uint GetBuildTargetVersion(BuildTarget target = BuildTarget.NoTarget)
            {
                Platform plaform = GetPlatform(target);
                string versionStr = plaform.version.Replace(".", "") + plaform.versionCode;
                uint version = 0;
                if (uint.TryParse(versionStr, out version))
                {
                    return version;
                }
                return 0;
            }
        }
        string m_strUpdateCheckVersion = "1.0.0";

        string[] m_vSetingTagPops = null;
        List<string> m_vSettingTags = new List<string>();
        PublishSetting m_BuildSetting = new PublishSetting();
        Vector2 m_ScrollPos = Vector2.zero;

        static Dictionary<string, string> m_vMarkAbs = new Dictionary<string, string>();
        bool m_bUnExportOtherPackageDir = false;
        bool m_bExportOtherPackageDir = false;
        bool m_bExportOtherLegacyDir = false;
        bool m_bExportParticlesDir = false;
        bool m_bCopyDirToStreamAssetDir = false;
        bool m_bCopySDKDirToPlugin = false;
        bool m_bCopyMarcoToProject = false;
        bool m_bExpandServerUrl = false;
        bool m_bExpandBuildDllPaks = false;
        bool m_bIncludeShaders = false;
        bool m_bBuildSingleAb = false;
        bool m_bBuildFileRootPathAb = false;
        bool m_bBuildCombineGpAb = false;
        bool m_bSpriteUnPak = false;
        bool m_bExpandShaderVariants = false;
        //------------------------------------------------------
        public void OnDisable()
        {
            Save();
        }
        //------------------------------------------------------
        public void OnEnable()
        {
            m_BuildSetting = LoadSetting();
            HybridCLRHelper.IsEnable = m_BuildSetting.hotType == EHotType.HybridCLR;
            m_vMarkAbs.Clear();
            CompileAssetBundles.OnABBuildCallback = BuildAB;
            CompileAssetBundles.OnMarkAbName = OnMarkABName;
            CompileAssetBundles.OnPrepareMarkAbName = PublishPanel.OnPrepareMarkAbName;
            RefreshTags();
        }
        //------------------------------------------------------
        void RefreshTags()
        {
            m_vSetingTagPops = null;
            List<string> vPop = new List<string>();
            m_vSettingTags.Clear();
            m_vSettingTags.Add("");
            vPop.Add("缺省");
            string[] settings = System.IO.Directory.GetFiles(Application.dataPath + "/../Publishs/", "*.json");
            for (int i = 0; i < settings.Length; ++i)
            {
                string tag = System.IO.Path.GetFileNameWithoutExtension(settings[i]).Replace("Setting", "");
                if (string.IsNullOrEmpty(tag)) continue;
                if (tag.StartsWith("_")) tag = tag.Substring(1);
                m_vSettingTags.Add(tag);
                vPop.Add(tag);
            }
            m_vSetingTagPops = vPop.ToArray();
        }
        //------------------------------------------------------
        public string GetUpdateVersion()
        {
            return m_strUpdateCheckVersion;
        }
        //------------------------------------------------------
        public void Save()
        {
            SaveSetting(m_BuildSetting);
        }
        //------------------------------------------------------
        public PublishSetting getBuildSetting()
        {
            return m_BuildSetting;
        }
        //------------------------------------------------------
        public static PublishSetting LoadSetting(string strTag = "")
        {
            PublishSetting setting = null;
            string strPath = Application.dataPath + "/../Publishs/Setting_" + strTag + ".json";
            if (!System.IO.File.Exists(strPath))
            {
                Debug.LogWarning(strTag + "   setting file unfind!");
                strPath = Application.dataPath + "/../Publishs/Setting.json";
            }
            //Debug.Log("setingpath:" + strPath);
            if (System.IO.File.Exists(strPath))
            {
                try
                {
                    string strCode = File.ReadAllText(strPath, System.Text.Encoding.Default);
                    setting = JsonUtility.FromJson<PublishSetting>(strCode);
                    setting.Init();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.StackTrace);
                    setting = new PublishSetting();
                }
            }
            if (setting == null) setting = new PublishSetting();

            if (string.IsNullOrEmpty(setting.bundleExportDir)) setting.bundleExportDir = "/../Publishs/Packages";
            setting.buildTarget = EditorUserBuildSettings.activeBuildTarget;
            setting.settingTag = strTag;
            return setting;
        }
        //------------------------------------------------------
        static void SaveSetting(PublishSetting setting)
        {
            if (setting == null) return;
            string strPath = Application.dataPath + "/../Publishs/Setting.json";
            if (!string.IsNullOrEmpty(setting.settingTag) && setting.settingTag.CompareTo("default") != 0)
                strPath = Application.dataPath + "/../Publishs/Setting_" + setting.settingTag + ".json";

            if (System.IO.File.Exists(strPath))
                System.IO.File.Delete(strPath);
            string strJson = JsonUtility.ToJson(setting, true);
            System.IO.FileStream fs = new System.IO.FileStream(strPath, System.IO.FileMode.OpenOrCreate);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
            sw.Write(strJson);
            sw.Close();
        }
        //------------------------------------------------------
        public void OnGUI(Rect position)
        {
            int preSelectTag = m_vSettingTags.IndexOf(m_BuildSetting.settingTag);
            int selectTag = EditorGUILayout.Popup("选择配置Tag", preSelectTag, m_vSetingTagPops);
            if (preSelectTag != selectTag && selectTag >= 0 && selectTag < m_vSettingTags.Count)
            {
                string settingFile = "";
                if (string.IsNullOrEmpty(m_vSettingTags[selectTag])) settingFile = Application.dataPath + "/../Publishs/Setting.json";
                else settingFile = Application.dataPath + "/../Publishs/Setting_" + m_vSettingTags[selectTag] + ".json";


                if (File.Exists(settingFile))
                {
                    try
                    {
                        PublishSetting settingPublish = JsonUtility.FromJson<PublishSetting>(File.ReadAllText(settingFile, System.Text.Encoding.Default));
                        if (settingPublish != null)
                        {
                            settingPublish.Init();
                            settingPublish.buildTarget = m_BuildSetting.buildTarget;
                            settingPublish.settingTag = m_vSettingTags[selectTag];

                            m_BuildSetting = settingPublish;
                        }
                    }
                    catch
                    {

                    }
                }
            }

            float layoutWidth = position.width - 20;
            GUILayoutOption[] ops = null;// new GUILayoutOption[] { GUILayout.Width(layoutWidth) };
            m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, false, true, new GUILayoutOption[] { GUILayout.Width(position.width - 10) });

            EditorGUI.BeginChangeCheck();
            DrawTile("调试信息", Color.yellow, layoutWidth);
            {
                m_BuildSetting.debug = EditorGUILayout.Toggle("Debug版本", m_BuildSetting.debug, ops);
                m_BuildSetting.profiler = EditorGUILayout.Toggle("分析调试", m_BuildSetting.profiler, ops);
                m_BuildSetting.depthPrifiling = EditorGUILayout.Toggle("深度调试", m_BuildSetting.depthPrifiling, ops);
                m_BuildSetting.scriptDebug = EditorGUILayout.Toggle("脚本调试", m_BuildSetting.scriptDebug, ops);
                //      m_BuildSetting.waitForManagerDebuger = EditorGUILayout.Toggle("分析调试", m_BuildSetting.waitForManagerDebuger, ops);
                m_BuildSetting.autoConnectProfiler = EditorGUILayout.Toggle("自动连接调试器", m_BuildSetting.autoConnectProfiler, ops);
            }

            PublishSetting.Platform platform = m_BuildSetting.GetPlatform(m_BuildSetting.buildTarget);

            DrawTile("打包操作选项", Color.red, layoutWidth);
            {
                m_BuildSetting.offline = EditorGUILayout.Toggle("单机", m_BuildSetting.offline, ops);
                m_BuildSetting.abUseIndex = EditorGUILayout.Toggle("ab名使用id", m_BuildSetting.abUseIndex, ops);
                m_BuildSetting.dependMarkAB = EditorGUILayout.Toggle("依赖包打成ab", m_BuildSetting.dependMarkAB, ops);
                m_BuildSetting.singleFileAB = EditorGUILayout.Toggle("单资源ab", m_BuildSetting.singleFileAB, ops);
                m_BuildSetting.portait = EditorGUILayout.Toggle("竖屏", m_BuildSetting.portait, ops);
                m_BuildSetting.buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("平台", m_BuildSetting.buildTarget, ops);
                m_BuildSetting.popPrivacy = EditorGUILayout.Toggle("隐私政策弹框", m_BuildSetting.popPrivacy, ops);
                m_BuildSetting.channelLabel = EditorGUILayout.TextField("渠道标识", m_BuildSetting.channelLabel, ops);

                EditorGUILayout.BeginHorizontal();
                EHotType preHot = m_BuildSetting.hotType;
                m_BuildSetting.hotType = (EHotType)EditorGUILayout.EnumPopup("热更方案", m_BuildSetting.hotType, ops);
                if (preHot != m_BuildSetting.hotType)
                {
                    HybridCLRHelper.IsEnable = m_BuildSetting.hotType == EHotType.HybridCLR;
                    AutoMarcros.SetMacros(null, m_BuildSetting);
                }
#if USE_HYBRIDCLR
                if (m_BuildSetting.hotType == EHotType.HybridCLR && GUILayout.Button("Check"))
                {
                    if(HybridCLRHelper.IsEnable) HybridCLRHelper.CheckUp();
                }
#endif
                EditorGUILayout.EndHorizontal();
#if !UNITY_5_1
                m_BuildSetting.buildOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField("Bundle编译选项", m_BuildSetting.buildOptions);
#endif
                if (m_BuildSetting.buildTarget == BuildTarget.Android)
                {
                    m_BuildSetting.androidSdkVersion = (AndroidSdkVersions)EditorGUILayout.EnumPopup("API Level", m_BuildSetting.androidSdkVersion, ops);
                    m_BuildSetting.obb = EditorGUILayout.Toggle("分包", m_BuildSetting.obb, ops);
                }
                m_BuildSetting.bIL2CPP = EditorGUILayout.Toggle("使用IL2CPP", m_BuildSetting.bIL2CPP, ops);
                if (m_BuildSetting.bIL2CPP)
                {
                    m_BuildSetting.globalMetaDataEncrypt = EditorGUILayout.Toggle("global-metadata加密", m_BuildSetting.globalMetaDataEncrypt, ops);
                    m_BuildSetting.armv7 = EditorGUILayout.Toggle("Armv7", m_BuildSetting.armv7, ops);
                    m_BuildSetting.armv64 = EditorGUILayout.Toggle("Armv64", m_BuildSetting.armv64, ops);
                }
                m_BuildSetting.useEncrptyPak = EditorGUILayout.Toggle("资源加固", m_BuildSetting.useEncrptyPak, ops);
                m_BuildSetting.versionCheck = EditorGUILayout.Toggle("版本检测", m_BuildSetting.versionCheck, ops);
                m_BuildSetting.versionServerAddress = EditorGUILayout.TextField("版本检测服务器地址", m_BuildSetting.versionServerAddress, ops);

                m_BuildSetting.convertASTC = EditorGUILayout.Toggle("图片转ASTC", m_BuildSetting.convertASTC, ops);
                m_BuildSetting.streamingMipmaps = EditorGUILayout.Toggle("StreamingMipmaps", m_BuildSetting.streamingMipmaps, ops);
                m_BuildSetting.dateName = EditorGUILayout.Toggle("包名加入时间", m_BuildSetting.dateName, ops);

                m_BuildSetting.delPrePackageDir = EditorGUILayout.Toggle("是否删除原有包体数据", m_BuildSetting.delPrePackageDir, ops);
                m_BuildSetting.copyToStreamAsset = EditorGUILayout.Toggle("是否拷贝到StreamAsset 目录下", m_BuildSetting.copyToStreamAsset, ops);

                m_BuildSetting.spritePacker = (ESpriteAtlasType)EditorGUILayout.EnumPopup("图集打包方式", m_BuildSetting.spritePacker, ops);
                m_BuildSetting.bundleExportDir = EditorGUILayout.TextField("AssetBundle 生成目录", m_BuildSetting.bundleExportDir);

                m_BuildSetting.showFps = EditorGUILayout.Toggle("显示Fps", m_BuildSetting.showFps, ops);
                m_BuildSetting.reportView = EditorGUILayout.Toggle("DebugView", m_BuildSetting.reportView, ops);
                m_BuildSetting.gmPanel = EditorGUILayout.Toggle("GM控制台", m_BuildSetting.gmPanel, ops);
                m_BuildSetting.antiAddiction = EditorGUILayout.Toggle("防沉迷", m_BuildSetting.antiAddiction, ops);

                m_BuildSetting.useVR = EditorGUILayout.Toggle("VR项目", m_BuildSetting.useVR, ops);

                GUILayout.BeginHorizontal();
                m_bExpandShaderVariants = EditorGUILayout.Foldout(m_bExpandShaderVariants, "Shader 变体");
                if (m_bExpandShaderVariants && GUILayout.Button("添加"))
                {
                    platform.shadervariants.Add("");
                }
                GUILayout.EndHorizontal();


                if (m_bExpandShaderVariants)
                {
                    for (int i = 0; i < platform.shadervariants.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        ShaderVariantCollection svc = EditorGUILayout.ObjectField("变体[" + i + "]", AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(platform.shadervariants[i]), typeof(ShaderVariantCollection), false, ops) as ShaderVariantCollection;
                        if (svc)
                            platform.shadervariants[i] = AssetDatabase.GetAssetPath(svc);
                        else
                            platform.shadervariants[i] = null;
                        if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(100) }))
                        {
                            platform.shadervariants.RemoveAt(i);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }

                }

                m_bExportOtherPackageDir = EditorGUILayout.Foldout(m_bExportOtherPackageDir, "自定义打包目录");
                if (m_bExportOtherPackageDir)
                {
                    EditorGUILayout.BeginVertical(ops);
                    for (int i = 0; i < m_BuildSetting.buildDirs.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        m_BuildSetting.buildDirs[i] = EditorGUILayout.TextField("[" + (i + 1).ToString() + "]", m_BuildSetting.buildDirs[i]);
                        if (GUILayout.Button("移除"))
                        {
                            m_BuildSetting.buildDirs.RemoveAt(i);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("添加打包目录"))
                    {
                        m_BuildSetting.buildDirs.Add("");
                    }
                    EditorGUILayout.EndVertical();
                }

                GUILayout.BeginHorizontal();
                m_bIncludeShaders = EditorGUILayout.Foldout(m_bIncludeShaders, "Included Shaders");
                if (GUILayout.Button("执行"))
                {
                    SetShaderIncludes(m_BuildSetting);
                }
                GUILayout.EndHorizontal();
                if (m_bIncludeShaders)
                {
                    EditorGUILayout.BeginVertical(ops);
                    for (int i = 0; i < m_BuildSetting.includeShaders.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        Shader shader = EditorGUILayout.ObjectField("[" + (i + 1).ToString() + "]", Shader.Find(m_BuildSetting.includeShaders[i]), typeof(Shader), false) as Shader;
                        if (shader)
                        {
                            m_BuildSetting.includeShaders[i] = shader.name;
                        }
                        else
                            m_BuildSetting.includeShaders[i] = "";
                        if (GUILayout.Button("移除"))
                        {
                            m_BuildSetting.includeShaders.RemoveAt(i);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("添加"))
                    {
                        m_BuildSetting.includeShaders.Add("");
                    }
                    EditorGUILayout.EndVertical();
                }

                m_bUnExportOtherPackageDir = EditorGUILayout.Foldout(m_bUnExportOtherPackageDir, "忽略自定义打包目录");
                if (m_bUnExportOtherPackageDir)
                {
                    EditorGUILayout.BeginVertical(ops);
                    for (int i = 0; i < m_BuildSetting.unbuildDirs.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        m_BuildSetting.unbuildDirs[i] = EditorGUILayout.TextField("[" + (i + 1).ToString() + "]", m_BuildSetting.unbuildDirs[i]);
                        if (GUILayout.Button("移除"))
                        {
                            m_BuildSetting.unbuildDirs.RemoveAt(i);
                            break;
                        }
                        if (GUILayout.Button("清除标记"))
                        {
                            CompileAssetBundles.MarkAssetBundleAysnByDir(m_BuildSetting, m_BuildSetting.unbuildDirs[i], false, false, false, false);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("添加"))
                    {
                        m_BuildSetting.unbuildDirs.Add("");
                    }
                    EditorGUILayout.EndVertical();
                }

                GUILayout.BeginHorizontal();
                m_bBuildSingleAb = EditorGUILayout.Foldout(m_bBuildSingleAb, "单包配置");
                GUILayout.EndHorizontal();
                if (m_bBuildSingleAb)
                {
                    EditorGUILayout.BeginVertical(ops);
                    for (int i = 0; i < m_BuildSetting.buildSingleAbs.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        m_BuildSetting.buildSingleAbs[i] = EditorGUILayout.TextField("[" + (i + 1).ToString() + "]", m_BuildSetting.buildSingleAbs[i]);
                        if (GUILayout.Button("移除"))
                        {
                            m_BuildSetting.buildSingleAbs.RemoveAt(i);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("添加"))
                    {
                        m_BuildSetting.buildSingleAbs.Add("");
                    }
                    EditorGUILayout.EndVertical();
                }

                if (m_BuildSetting.singleFileAB)
                {
                    GUILayout.BeginHorizontal();
                    m_bBuildFileRootPathAb = EditorGUILayout.Foldout(m_bBuildFileRootPathAb, "根据目录打AB");
                    GUILayout.EndHorizontal();
                    if (m_bBuildFileRootPathAb)
                    {
                        EditorGUILayout.BeginVertical(ops);
                        for (int i = 0; i < m_BuildSetting.buildFileRootPathAbs.Count; ++i)
                        {
                            GUILayout.BeginHorizontal();
                            m_BuildSetting.buildFileRootPathAbs[i] = EditorGUILayout.TextField("[" + (i + 1).ToString() + "]", m_BuildSetting.buildFileRootPathAbs[i]);
                            if (GUILayout.Button("移除"))
                            {
                                m_BuildSetting.buildFileRootPathAbs.RemoveAt(i);
                                break;
                            }
                            GUILayout.EndHorizontal();
                        }
                        if (GUILayout.Button("添加"))
                        {
                            m_BuildSetting.buildFileRootPathAbs.Add("");
                        }
                        EditorGUILayout.EndVertical();
                    }
                }

                GUILayout.BeginHorizontal();
                m_bBuildCombineGpAb = EditorGUILayout.Foldout(m_bBuildCombineGpAb, "打组AB");
                if (GUILayout.Button("添加"))
                {
                    AbGroup abGp = new AbGroup();
                    abGp.strAbName = "";
                    abGp.pathFile = "";
                    m_BuildSetting.buildCombineAbsGroup.Add(abGp);
                }
                GUILayout.EndHorizontal();
                if (m_bBuildCombineGpAb)
                {
                    GUILayoutOption[] opTiles = new GUILayoutOption[] { GUILayout.Width((position.width - 10 - 50) / 2) };
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("文件或路径", opTiles);
                    GUILayout.Label("AB组名", opTiles);
                    GUILayout.Label("", new GUILayoutOption[] { GUILayout.Width(50) });
                    EditorGUILayout.EndHorizontal();
                    for (int i = 0; i < m_BuildSetting.buildCombineAbsGroup.Count; ++i)
                    {
                        AbGroup abGp = m_BuildSetting.buildCombineAbsGroup[i];
                        EditorGUILayout.BeginHorizontal();
                        abGp.pathFile = EditorGUILayout.TextField(abGp.pathFile, opTiles);
                        abGp.strAbName = EditorGUILayout.TextField(abGp.strAbName, opTiles);
                        if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(50) }))
                        {
                            m_BuildSetting.buildCombineAbsGroup.RemoveAt(i);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }

                GUILayout.BeginHorizontal();
                m_bSpriteUnPak = EditorGUILayout.Foldout(m_bSpriteUnPak, "图集UnPak");
                GUILayout.EndHorizontal();
                if (m_bSpriteUnPak)
                {
                    EditorGUILayout.BeginVertical(ops);
                    for (int i = 0; i < m_BuildSetting.spriteUnPak.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        m_BuildSetting.spriteUnPak[i] = EditorGUILayout.TextField("[" + (i + 1).ToString() + "]", m_BuildSetting.spriteUnPak[i]);
                        if (GUILayout.Button("移除"))
                        {
                            m_BuildSetting.spriteUnPak.RemoveAt(i);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("添加"))
                    {
                        m_BuildSetting.spriteUnPak.Add("");
                    }
                    EditorGUILayout.EndVertical();
                }

                GUILayout.BeginHorizontal();
                m_bExportParticlesDir = EditorGUILayout.Foldout(m_bExportParticlesDir, "特效资源检测");
                if (GUILayout.Button("检测"))
                {
                    CheckParticles(m_BuildSetting);
                }
                GUILayout.EndHorizontal();
                if (m_bExportParticlesDir)
                {
                    EditorGUILayout.BeginVertical(ops);
                    for (int i = 0; i < m_BuildSetting.particleSets.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        m_BuildSetting.particleSets[i] = EditorGUILayout.TextField("[" + (i + 1).ToString() + "]", m_BuildSetting.particleSets[i]);
                        if (GUILayout.Button("移除"))
                        {
                            m_BuildSetting.particleSets.RemoveAt(i);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("添加"))
                    {
                        m_BuildSetting.particleSets.Add("");
                    }
                    EditorGUILayout.EndVertical();
                }

                GUILayout.BeginHorizontal();
                m_bExportOtherLegacyDir = EditorGUILayout.Foldout(m_bExportOtherLegacyDir, "动画剪辑资源检测");
                if (GUILayout.Button("检测"))
                {
                    CheckLegacyFlag(m_BuildSetting);
                }
                GUILayout.EndHorizontal();
                if (m_bExportOtherLegacyDir)
                {
                    EditorGUILayout.BeginVertical(ops);
                    for (int i = 0; i < m_BuildSetting.legacyFlagSets.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        m_BuildSetting.legacyFlagSets[i] = EditorGUILayout.TextField("[" + (i + 1).ToString() + "]", m_BuildSetting.legacyFlagSets[i]);
                        if (GUILayout.Button("移除"))
                        {
                            m_BuildSetting.legacyFlagSets.RemoveAt(i);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("添加"))
                    {
                        m_BuildSetting.legacyFlagSets.Add("");
                    }
                    EditorGUILayout.EndVertical();
                }
                m_bCopyDirToStreamAssetDir = EditorGUILayout.Foldout(m_bCopyDirToStreamAssetDir, "拷贝目录(文件)SteamAssets目录下");
                if (m_bCopyDirToStreamAssetDir)
                {
                    EditorGUILayout.BeginVertical(ops);
                    float tileWidth = (layoutWidth - 10) / 4;
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(layoutWidth) });
                    GUILayout.Label("原目录/文件", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("StreamAssets子目录", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("pakTag", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.EndHorizontal();
                    for (int i = 0; i < m_BuildSetting.copyToStreamDirs.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        PakData copy = m_BuildSetting.copyToStreamDirs[i];
                        copy.srcDir = EditorGUILayout.TextField(copy.srcDir, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        copy.toDir = EditorGUILayout.TextField(copy.toDir, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        copy.pakTag = EditorGUILayout.TextField(copy.pakTag, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(tileWidth) }))
                        {
                            m_BuildSetting.copyToStreamDirs.RemoveAt(i);
                            break;
                        }
                        m_BuildSetting.copyToStreamDirs[i] = copy;
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("添加打包目录"))
                    {
                        m_BuildSetting.copyToStreamDirs.Add(new PakData() { srcDir = "", toDir = "" });
                    }
                    EditorGUILayout.EndVertical();
                }

                m_bCopySDKDirToPlugin = EditorGUILayout.Foldout(m_bCopySDKDirToPlugin, "SDK相关配置");
                if (m_bCopySDKDirToPlugin)
                {
                    EditorGUILayout.BeginVertical(ops);
                    int argv = 5;
                    if (m_BuildSetting.buildTarget == BuildTarget.Android) argv++;
                    float tileWidth = (layoutWidth - 10) / argv;
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(layoutWidth) });
                    GUILayout.Label("", new GUILayoutOption[] { GUILayout.Width(45) });
                    GUILayout.Label("宏开关", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("相关配置数据目录", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("是否使用", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("ExternalDependency", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    if (m_BuildSetting.buildTarget == BuildTarget.Android)
                        GUILayout.Label("UnitySDK", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.EndHorizontal();
                    for (int i = 0; i < platform.copySdks.Count; ++i)
                    {
                        GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(layoutWidth) });
                        SDKData copy = platform.copySdks[i];
                        copy.bExpand = EditorGUILayout.Foldout(copy.bExpand, "");
                        copy.sdkMarco = EditorGUILayout.TextField(copy.sdkMarco, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        copy.pluginDatas = EditorGUILayout.TextField(copy.pluginDatas, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        copy.bUsed = EditorGUILayout.Toggle(copy.bUsed, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        copy.needExternalDependency = EditorGUILayout.Toggle(copy.needExternalDependency, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        if (m_BuildSetting.buildTarget == BuildTarget.Android)
                            copy.needUnitySDK = EditorGUILayout.Toggle(copy.needUnitySDK, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(tileWidth / 2) }))
                        {
                            platform.copySdks.RemoveAt(i);
                            break;
                        }
                        if (GUILayout.Button("添加KeyValue", new GUILayoutOption[] { GUILayout.MaxWidth(tileWidth / 2) }))
                        {
                            if (copy.keyValues == null) copy.keyValues = new List<SDKData.KeyValue>();
                            copy.keyValues.Add(new SDKData.KeyValue() { key = "", value = "" });
                        }
                        GUILayout.EndHorizontal();

                        if (copy.bExpand)
                        {
                            float subWidth = (layoutWidth - 60) / 4;
                            EditorGUI.indentLevel++;
                            GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(subWidth) });
                            GUILayout.Label("Key", new GUILayoutOption[] { GUILayout.Width(subWidth) });
                            GUILayout.Label("Value", new GUILayoutOption[] { GUILayout.Width(subWidth) });
                            GUILayout.Label("说明", new GUILayoutOption[] { GUILayout.Width(subWidth) });
                            GUILayout.Label("", new GUILayoutOption[] { GUILayout.Width(subWidth) });
                            GUILayout.EndHorizontal();

                            if (copy.keyValues != null)
                            {
                                for (int j = 0; j < copy.keyValues.Count; ++j)
                                {
                                    SDKData.KeyValue keyValue = copy.keyValues[j];
                                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(subWidth) });
                                    keyValue.key = EditorGUILayout.TextField(keyValue.key, new GUILayoutOption[] { GUILayout.Width(subWidth) });
                                    keyValue.value = EditorGUILayout.TextField(keyValue.value, new GUILayoutOption[] { GUILayout.Width(subWidth) });
                                    keyValue.desc = EditorGUILayout.TextField(keyValue.desc, new GUILayoutOption[] { GUILayout.Width(subWidth) });
                                    if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(subWidth) }))
                                    {
                                        copy.keyValues.RemoveAt(j);
                                        break;
                                    }
                                    GUILayout.EndHorizontal();
                                    copy.keyValues[j] = keyValue;
                                }
                            }

                            EditorGUI.indentLevel--;
                        }
                        platform.copySdks[i] = copy;
                    }
                    if (GUILayout.Button("添加SDK"))
                    {
                        platform.copySdks.Add(new SDKData() { sdkMarco = "", pluginDatas = "", bUsed = false, needExternalDependency = false, needUnitySDK = false });
                    }
                    EditorGUILayout.EndVertical();
                }
                m_bCopyMarcoToProject = EditorGUILayout.Foldout(m_bCopyMarcoToProject, "宏相关资源拷贝");
                if (m_bCopyMarcoToProject)
                {
                    EditorGUILayout.BeginVertical(ops);
                    float tileWidth = (layoutWidth - 10) / 3;
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(layoutWidth) });
                    GUILayout.Label("宏开关", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("相关配置数据目录", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.EndHorizontal();
                    for (int i = 0; i < m_BuildSetting.marcoDatas.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        MarcoData copy = m_BuildSetting.marcoDatas[i];
                        copy.marco = EditorGUILayout.TextField(copy.marco, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        copy.pluginDatas = EditorGUILayout.TextField(copy.pluginDatas, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(tileWidth) }))
                        {
                            m_BuildSetting.marcoDatas.RemoveAt(i);
                            break;
                        }
                        m_BuildSetting.marcoDatas[i] = copy;
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("添加宏配置"))
                    {
                        m_BuildSetting.marcoDatas.Add(new MarcoData() { marco = "", pluginDatas = "" });
                    }
                    EditorGUILayout.EndVertical();
                }

                m_bExpandBuildDllPaks = EditorGUILayout.Foldout(m_bExpandBuildDllPaks, "dll资源包");
                if (m_bExpandBuildDllPaks)
                {
                    EditorGUILayout.BeginVertical(ops);
                    for (int i = 0; i < m_BuildSetting.buildDllPakDirs.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        m_BuildSetting.buildDllPakDirs[i] = EditorGUILayout.TextField("[" + (i + 1).ToString() + "]", m_BuildSetting.buildDllPakDirs[i]);
                        if (GUILayout.Button("移除"))
                        {
                            m_BuildSetting.buildDllPakDirs.RemoveAt(i);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("添加"))
                    {
                        m_BuildSetting.buildDllPakDirs.Add("");
                    }
                    EditorGUILayout.EndVertical();
                }
            }

            DrawTile("版本信息", Color.red, layoutWidth);
            {
                m_bExpandServerUrl = EditorGUILayout.Foldout(m_bExpandServerUrl, "服务器列表");
                if (m_bExpandServerUrl)
                {
                    EditorGUILayout.BeginVertical(ops);
                    float tileWidth = (layoutWidth - 30) / 5;
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(layoutWidth) });
                    GUILayout.Label("服务器名", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("地址", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("热更地址", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("使用", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.Label("", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                    GUILayout.EndHorizontal();
                    for (int i = 0; i < m_BuildSetting.serverUrlList.Count; ++i)
                    {
                        PublishSetting.ServerUrl serverList = m_BuildSetting.serverUrlList[i];
                        GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(layoutWidth) });
                        serverList.name = GUILayout.TextField(serverList.name, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        serverList.sreverUrl = GUILayout.TextField(serverList.sreverUrl, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        serverList.uploadUrl = GUILayout.TextField(serverList.uploadUrl, new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        serverList.bUsed = GUILayout.Toggle(serverList.bUsed, "", new GUILayoutOption[] { GUILayout.Width(tileWidth) });
                        if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(tileWidth) }))
                        {
                            if (EditorUtility.DisplayDialog("提示", "是否确定去除？", "去除", "取消"))
                            {
                                m_BuildSetting.serverUrlList.RemoveAt(i);
                                break;
                            }
                        }
                        GUILayout.EndHorizontal();

                        m_BuildSetting.serverUrlList[i] = serverList;
                    }
                    if (GUILayout.Button("新增", new GUILayoutOption[] { GUILayout.Width(layoutWidth) }))
                    {
                        m_BuildSetting.serverUrlList.Add(new PublishSetting.ServerUrl());
                    }
                    EditorGUILayout.EndVertical();
                }
                platform.projectExportName = EditorGUILayout.TextField("工程导出目录名", platform.projectExportName, ops);
                platform.projectName = EditorGUILayout.TextField("工程名", platform.projectName, ops);
                platform.bundleName = EditorGUILayout.TextField("包名", platform.bundleName, ops);
                platform.version = EditorGUILayout.TextField("版本号", platform.version, ops);
                //     if (m_BuildSetting.buildTarget == BuildTarget.Android)
                platform.versionCode = EditorGUILayout.IntField("迭代更新版本号", platform.versionCode, ops);

                platform.defaultLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("缺省语言", platform.defaultLanguage);
                if (m_BuildSetting.buildTarget == BuildTarget.iOS)
                {
                    platform.provisioning_profiler_appkey = EditorGUILayout.TextField("ProfilgerKey", platform.provisioning_profiler_appkey, ops);
                    platform.provisioning_profiler_name = EditorGUILayout.TextField("证书名", platform.provisioning_profiler_name, ops);
                    platform.development_teamKey = EditorGUILayout.TextField("DevelopmentTeam", platform.development_teamKey, ops);
                }

                GUILayout.BeginHorizontal();
                m_strUpdateCheckVersion = EditorGUILayout.TextField("版本更新包", m_strUpdateCheckVersion);
                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(m_strUpdateCheckVersion));
                if (GUILayout.Button("检测", new GUILayoutOption[] { GUILayout.Width(80) }))
                {
                    CopyToStreamRawAssets(m_BuildSetting);
                    FilterUpdateAssetBundle(m_BuildSetting, "resinfo.json", platform.version, m_strUpdateCheckVersion);
                }
                if (GUILayout.Button("上传", new GUILayoutOption[] { GUILayout.Width(80) }))
                {
                    string outPut = m_BuildSetting.GetBuildOutputRoot(m_BuildSetting.buildTarget);

                    UploadAssets upLoad = new UploadAssets();
                    upLoad.Upload("http://192.168.100.210:35560/upload/UpLoadFile.jsp", outPut + "/" + m_strUpdateCheckVersion, m_BuildSetting.buildTarget + "/" + m_strUpdateCheckVersion, new List<string>(new[] { "resinfo.json" }));
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck())
            {
                Save();
            }

            EditorGUILayout.EndScrollView();

            if (m_BuildSetting.bundleExportDir.Length > 0)
            {
                string outPut = m_BuildSetting.GetBuildTargetOutputDir();

                //   int btnCnt = 7;
                // if (System.IO.Directory.Exists(m_BuildSetting.GetBuildTargetOutputDir())) btnCnt++;
                int btnCnt = 9;

                float gapW = (position.width - 42) / btnCnt;
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80) });
                if (GUILayout.Button("打包AB", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80 / 2) }))
                {
                    Save();
                    BuildAndMarkAssetBundleAysn(m_BuildSetting, false);
                }
                if (GUILayout.Button("清除所有AB标签", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80 / 2) }))
                {
                    UnMarkAssetBundleAysn(m_BuildSetting);
                }
                GUILayout.EndVertical();
                if (GUILayout.Button("Copy配置项到StreamAssets", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80) }))
                {
                    CopyToStreamRawAssets(m_BuildSetting);
                }
                if (GUILayout.Button("加固包", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80) }))
                {
                    CopyToStreamRawAssets(m_BuildSetting);
                    PackagePanel.BuildEncrptyPak(m_BuildSetting, null);
                }
                if (GUILayout.Button("检测SDK配置", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80) }))
                {
                    CopySDKToPluginAssets(m_BuildSetting);
                }
                GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80) });
                if (GUILayout.Button("ui 图集", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80 / 3) }))
                {
                    BuildUITextureAtlas(m_BuildSetting, true);
                }
                if (GUILayout.Button("图片ASTC", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80 / 3) }))
                {
                    ConvertTextureChecker(m_BuildSetting);
                }
                if (GUILayout.Button("图片规格", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80 / 3) }))
                {
                    ConvertTextureChecker(m_BuildSetting, false);
                }
                GUILayout.EndVertical();
                GUILayout.Box("", new GUILayoutOption[] { GUILayout.Width(5), GUILayout.Height(80) });
                if (GUILayout.Button("Build", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80) }))
                {
#if USE_HYBRIDCLR
                    if ( HybridCLRHelper.IsEnable && m_BuildSetting.hotType == EHotType.HybridCLR && !HybridCLRHelper.CheckUp())
                        return;
#endif
                    m_BuildSetting.buildAutoRun = false;
                    Save();
                    BuildAndMarkAssetBundleAysn(m_BuildSetting, true);
                }
                //      if(System.IO.Directory.Exists(m_BuildSetting.GetBuildTargetOutputDir()) )
                {
                    if (GUILayout.Button("Quick Build", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80) }))
                    {
                        if (!System.IO.Directory.Exists(m_BuildSetting.GetBuildTargetOutputDir()))
                        {
                            if (!EditorUtility.DisplayDialog("提示", "还未打AB,确认直接出包？", "确认", "取消"))
                                return;
                        }
#if USE_HYBRIDCLR
                        if (HybridCLRHelper.IsEnable && m_BuildSetting.hotType == EHotType.HybridCLR && !HybridCLRHelper.CheckUp())
                            return;
#endif
                        m_BuildSetting.buildAutoRun = false;
                        Save();
                        CopySDKToPluginAssets(m_BuildSetting);
                        BuildAB(m_BuildSetting, true, true, false);
                    }
                    if (GUILayout.Button("Quick Build And Run", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80) }))
                    {
                        if (!System.IO.Directory.Exists(m_BuildSetting.GetBuildTargetOutputDir()))
                        {
                            if (!EditorUtility.DisplayDialog("提示", "还未打AB,确认直接出包？", "确认", "取消"))
                                return;
                        }
#if USE_HYBRIDCLR
                        if (HybridCLRHelper.IsEnable && m_BuildSetting.hotType == EHotType.HybridCLR && !HybridCLRHelper.CheckUp())
                            return;
#endif
                        m_BuildSetting.buildAutoRun = true;
                        Save();
                        CopySDKToPluginAssets(m_BuildSetting);
                        BuildAB(m_BuildSetting, true, true, false);
                    }
                }

                if (GUILayout.Button("Build And Run", new GUILayoutOption[] { GUILayout.Width(gapW), GUILayout.Height(80) }))
                {
#if USE_HYBRIDCLR
                    if (HybridCLRHelper.IsEnable && !HybridCLRHelper.CheckUp())
                        return;
#endif
                    m_BuildSetting.buildAutoRun = true;
                    Save();
                    BuildAndMarkAssetBundleAysn(m_BuildSetting, true);
                }
                GUILayout.EndHorizontal();
            }

        }
        //------------------------------------------------------
        public static bool UnMarkAssetBundleAysn(PublishSetting setting)
        {
            m_vMarkAbs.Clear();
            if (setting.buildDirs != null)
            {
                for (int i = 0; i < setting.buildDirs.Count; ++i)
                {
                    string strPath = setting.buildDirs[i];
                    if (!string.IsNullOrEmpty(strPath))
                        CompileAssetBundles.MarkAssetBundleAysnByDir(setting, strPath, false, false, false, false);
                }
            }
            bool bBuild = CompileAssetBundles.MarkAssetBundleAysnByDir(setting, "Assets/Datas", false, false, false, false);
            if (setting.unbuildDirs != null)
            {
                for (int i = 0; i < setting.unbuildDirs.Count; ++i)
                {
                    string strPath = setting.unbuildDirs[i];
                    if (!string.IsNullOrEmpty(strPath))
                        CompileAssetBundles.MarkAssetBundleAysnByDir(setting, strPath, false, false, false, false);
                }
            }
            return bBuild;
        }
        //------------------------------------------------------
        public static void CheckLegacyFlag(PublishSetting setting)
        {
            if (setting.legacyFlagSets != null)
            {
                bool bDirty = false;
                string[] assets = AssetDatabase.FindAssets("t:AnimationClip", setting.legacyFlagSets.ToArray());
                if (assets != null)
                {
                    EditorUtility.DisplayProgressBar("设置动画Legacy标志", "", 0);
                    for (int i = 0; i < assets.Length; ++i)
                    {
                        string clippath = AssetDatabase.GUIDToAssetPath(assets[i]);
                        if (System.IO.Path.GetExtension(clippath).CompareTo(".anim") == 0 && !clippath.Contains("Assets/DatasRef/"))
                        {
                            EditorUtility.DisplayProgressBar("设置动画Legacy标志", clippath, ((float)i) / ((float)assets.Length));
                            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clippath);
                            if (clip && !clip.legacy)
                            {
                                clip.legacy = true;
                                EditorUtility.SetDirty(clip);
                                bDirty = true;
                            }
                        }
                    }
                    EditorUtility.ClearProgressBar();
                }
                if (bDirty)
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                }
            }
        }
        //------------------------------------------------------
        public static void CheckParticles(PublishSetting setting)
        {
            if (setting.particleSets != null)
            {
                bool bDirty = false;
                string[] assets = AssetDatabase.FindAssets("t:prefab", setting.particleSets.ToArray());
                if (assets != null)
                {
                    EditorUtility.DisplayProgressBar("检测特效引用mesh", "", 0);
                    for (int a = 0; a < assets.Length; ++a)
                    {
                        string parPath = AssetDatabase.GUIDToAssetPath(assets[a]);
                        Debug.Log("检测特效:" + parPath);
                        GameObject par = AssetDatabase.LoadAssetAtPath<GameObject>(parPath);
                        if (par)
                        {
                            EditorUtility.DisplayProgressBar("检测特效引用mesh", parPath, ((float)a) / ((float)assets.Length));
                            ParticleSystem[] emitters = par.GetComponentsInChildren<ParticleSystem>();
                            for (int i = 0; i < emitters.Length; ++i)
                            {
                                ParticleSystem.ShapeModule shapeModule = emitters[i].shape;
                                if (!shapeModule.enabled)
                                {
                                    bDirty = true;
                                    shapeModule.mesh = null;
                                    shapeModule.meshRenderer = null;
                                    shapeModule.skinnedMeshRenderer = null;

                                    EditorUtility.SetDirty(par);
                                    bDirty = true;
                                }

                                ParticleSystemRenderer parRender = emitters[i].GetComponent<ParticleSystemRenderer>();
                                if (parRender)
                                {
                                    if (!parRender.enabled)
                                    {
                                        bDirty = parRender.mesh != null;
                                        parRender.mesh = null;
                                        bDirty = true;
                                    }
                                    else
                                    {
                                        if (parRender.mesh != null)
                                        {
                                            string meshPath = AssetDatabase.GetAssetPath(parRender.mesh);
                                            if (!string.IsNullOrEmpty(meshPath))
                                            {
                                                ModelImporter modelImport = AssetImporter.GetAtPath(meshPath) as ModelImporter;
                                                if (modelImport != null)
                                                {
                                                    bool bReimport = false;
                                                    if (!modelImport.isReadable)
                                                    {
                                                        modelImport.isReadable = true;
                                                        bReimport = true;
                                                    }
                                                    if (!modelImport.optimizeMeshPolygons)
                                                    {
                                                        modelImport.optimizeMeshPolygons = true;
                                                        bReimport = true;
                                                    }
                                                    if (!modelImport.optimizeMeshVertices)
                                                    {
                                                        modelImport.optimizeMeshVertices = true;
                                                        bReimport = true;
                                                    }
                                                    if (!modelImport.optimizeGameObjects)
                                                    {
                                                        modelImport.optimizeGameObjects = true;
                                                        bReimport = true;
                                                    }
                                                    if (modelImport.meshOptimizationFlags != MeshOptimizationFlags.Everything)
                                                    {
                                                        bReimport = true;
                                                        modelImport.meshOptimizationFlags = MeshOptimizationFlags.Everything;
                                                    }
                                                    if (bReimport) modelImport.SaveAndReimport();
                                                }
                                            }
                                        }
                                    }

                                    if (bDirty)
                                        EditorUtility.SetDirty(par);
                                }
                            }
                        }
                    }
                    EditorUtility.ClearProgressBar();
                }
                if (bDirty)
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                }
            }
        }
        //------------------------------------------------------
        public static void ConvertTextureChecker(PublishSetting setting, bool bCheckASTC = true)
        {
            string[] textureAssets = AssetDatabase.FindAssets("t:Texture2D");

            if (textureAssets == null)
                return;
            List<string> sprites = new List<string>();
            for (int i = 0; i < textureAssets.Length; ++i)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(textureAssets[i]);
                if (assetPath.Contains("Packages/")) continue;
                if (assetPath.Contains("/Editor/")) continue;
                if (assetPath.Contains("/Editor Default Resources/")) continue;
                if (assetPath.Contains("/ThirdParty/")) continue;
                if (assetPath.Contains("/Scripts/")) continue;
                sprites.Add(assetPath);
            }
            string strTexPlaform = "";
            if (setting.buildTarget == BuildTarget.iOS)
            {
                strTexPlaform = "iPhone";
            }
            else if (setting.buildTarget == BuildTarget.Android)
            {
                strTexPlaform = "Android";
            }
            if (string.IsNullOrEmpty(strTexPlaform)) return;

            //无Alpha通道的贴图建议压缩格式为ASTC 8x8。
            //贴图为法线贴图，建议压缩格式为ASTC 5x5。
            //有更高要求的贴图（比如面部、场景地面），可以设置压缩格式为ASTC 6x6，法线贴图为ASTC 4x4。
            int startIndex = 0;
            for (; startIndex < sprites.Count; ++startIndex)
            {
                string data = sprites[startIndex];

                EditorUtility.DisplayProgressBar("图片转ASTC格式  " + startIndex.ToString() + "/" + sprites.Count.ToString(), data, (float)startIndex / (float)sprites.Count);

                string assetPath = data;
                AssetImporter import = AssetImporter.GetAtPath(assetPath);

                if (import != null && import is TextureImporter)
                {
                    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
                    TextureImporter texurtImport = import as TextureImporter;
                    bool bReimport = false;
                    TextureImporterPlatformSettings platformSetting = texurtImport.GetPlatformTextureSettings(strTexPlaform);
                    if (platformSetting == null)
                    {
                        platformSetting = texurtImport.GetDefaultPlatformTextureSettings();
                    }
                    if (platformSetting != null)
                    {
                        if (bCheckASTC && setting.convertASTC)
                        {
                            if (platformSetting.format.ToString().Contains("Automatic") || platformSetting.format.ToString().Contains("HDR") || platformSetting.format.ToString().Contains("PVRTC_") || platformSetting.format.ToString().Contains("ETC2_") || platformSetting.format.ToString().Contains("ETC_"))
                            {
                                if (assetPath.ToLower().Contains("lightmap"))
                                    platformSetting.format = TextureImporterFormat.ASTC_8x8;
                                else
                                    platformSetting.format = TextureImporterFormat.ASTC_6x6;
                                platformSetting.overridden = true;
                                texurtImport.SetPlatformTextureSettings(platformSetting);
                                bReimport = true;
                            }
                        }

                        if (setting.buildTarget == BuildTarget.Android)
                        {
                            if (platformSetting.androidETC2FallbackOverride != AndroidETC2FallbackOverride.Quality16Bit)
                            {
                                platformSetting.androidETC2FallbackOverride = AndroidETC2FallbackOverride.Quality16Bit;
                                platformSetting.overridden = true;
                                texurtImport.SetPlatformTextureSettings(platformSetting);
                                bReimport = true;
                            }
                        }
                        if (texurtImport.mipmapEnabled)
                        {
                            if (assetPath.ToLower().Contains("/fonts") || assetPath.ToLower().Contains("/uis/") || assetPath.ToLower().Contains("/UI/"))
                            {
                                texurtImport.mipmapEnabled = false;
                                bReimport = true;
                            }
                        }
                        else
                        {
                            if (assetPath.ToLower().Contains("/Scenes/"))
                            {
                                texurtImport.mipmapEnabled = true;
                                bReimport = true;
                            }
                        }
                        if (texurtImport.mipmapEnabled)
                        {
                            if (!texurtImport.streamingMipmaps)
                            {
                                bReimport = true;
                                texurtImport.streamingMipmaps = true;
                            }
                        }

                        if (texture.width >= 2048 || texture.height >= 2048)
                        {
                            if (platformSetting.maxTextureSize > 2048)
                            {
                                platformSetting.maxTextureSize = 1024;
                                platformSetting.overridden = true;
                                texurtImport.SetPlatformTextureSettings(platformSetting);
                                bReimport = true;
                            }
                        }
                    }
                    if (bReimport)
                        texurtImport.SaveAndReimport();
                }
            }
            EditorUtility.ClearProgressBar();
        }
        //------------------------------------------------------
        public static bool BuildAndMarkAssetBundleAysn(PublishSetting setting, bool bBuildPackage, bool hotUpdate = false)
        {
            try
            {
                WritePublishProgress("BuildAndMarkAssetBundleAysn-Begin");
                ConvertTextureChecker(setting);
                TextureCompresser.Compresser();
                BuildUITextureAtlas(setting, true);
                m_vMarkAbs.Clear();
                List<SplitPath> vSplitPaths = new List<SplitPath>();
                List<CompileAssetBundles.BuidDir> vDirs = new List<CompileAssetBundles.BuidDir>();
                if (setting.buildDirs != null)
                {
                    for (int i = 0; i < setting.buildDirs.Count; ++i)
                    {
                        string strPath = setting.buildDirs[i];
                        if (!string.IsNullOrEmpty(strPath))
                        {
                            int index = strPath.IndexOf("/xxx/");
                            if (index > 0)
                            {
                                int after = index + "/xxx/".Length;
                                SplitPath splitPath = new SplitPath();
                                splitPath.split0 = strPath.Substring(0, index) + "/";
                                splitPath.split1 = "/" + strPath.Substring(after, strPath.Length - after);
                                vSplitPaths.Add(splitPath);
                            }
                            else
                                vDirs.Add(new CompileAssetBundles.BuidDir() { strDir = strPath, bMark = true });
                        }
                    }
                }
                for (int i = 0; i < vSplitPaths.Count; ++i)
                {
                    var dirs = Directory.GetDirectories(vSplitPaths[i].split0, "*.*", SearchOption.AllDirectories);
                    for (int j = 0; j < dirs.Length; ++j)
                    {
                        string strDir = dirs[j].Replace("\\", "/").Replace(Application.dataPath + "/", "");
                        if (strDir.Contains(vSplitPaths[i].split0) && strDir.Contains(vSplitPaths[i].split1))
                        {
                            vDirs.Add(new CompileAssetBundles.BuidDir() { strDir = strDir, bMark = true });
                        }
                    }
                }

                CheckLegacyFlag(setting);
                CheckParticles(setting);

                vDirs.Add(new CompileAssetBundles.BuidDir() { strDir = "Assets/Datas", bMark = true });
                if (setting.unbuildDirs != null)
                {
                    for (int i = 0; i < setting.unbuildDirs.Count; ++i)
                    {
                        string strPath = setting.unbuildDirs[i];
                        if (!string.IsNullOrEmpty(strPath))
                        {
                            vDirs.Add(new CompileAssetBundles.BuidDir() { strDir = strPath, bMark = false });
                        }
                    }
                }
                WritePublishProgress("BuildAndMarkAssetBundleAysn-End");
                return CompileAssetBundles.MarkAssetBundleAysn(setting, true, vDirs, true, bBuildPackage, !bBuildPackage, hotUpdate);
            }
            catch(System.Exception ex)
            {
                WritePublishProgress("BuildAndMarkAssetBundleAysn:" + ex.StackTrace);
                Debug.LogError(ex.StackTrace);
            }
            return false;
        }
        //------------------------------------------------------
        static void BuildUITextureAtlas(PublishSetting setting, bool bMark)
        {
            string[] spriteAssets = AssetDatabase.FindAssets("t:Sprite");

            bool bPakTagingMode = setting.spritePacker == ESpriteAtlasType.Packer_Tag;
            if (setting.spritePacker == ESpriteAtlasType.Packer_Tag)
                EditorSettings.spritePackerMode = SpritePackerMode.AlwaysOn;
            else
                EditorSettings.spritePackerMode = SpritePackerMode.AlwaysOnAtlas;

            if (spriteAssets == null)
                return;
            List<string> sprites = new List<string>();
            for (int i = 0; i < spriteAssets.Length; ++i)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(spriteAssets[i]);
                if (assetPath.Contains("Packages/")) continue;
                if (assetPath.Contains("/Editor/")) continue;
                if (assetPath.Contains("/ThirdParty/")) continue;
                if (assetPath.Contains("/Scripts/")) continue;
                sprites.Add(assetPath);
            }
            string strTexPlaform = "";
            if (setting.buildTarget == BuildTarget.iOS)
            {
                strTexPlaform = "iPhone";
            }
            else if (setting.buildTarget == BuildTarget.Android)
            {
                strTexPlaform = "Android";
            }
            else if (setting.buildTarget == BuildTarget.WebGL)
            {
                strTexPlaform = "WebGL";
            }

            //无Alpha通道的贴图建议压缩格式为ASTC 8x8。
            //贴图为法线贴图，建议压缩格式为ASTC 5x5。
            //有更高要求的贴图（比如面部、场景地面），可以设置压缩格式为ASTC 6x6，法线贴图为ASTC 4x4。

            if (bPakTagingMode)
            {
                string rootPath = "Assets/Datas/SpriteAtlas";
                if (Directory.Exists(rootPath))
                {
                    EditorKits.DeleteDirectory(rootPath);
                }
                int startIndex = 0;
                for (; startIndex < sprites.Count; ++startIndex)
                {
                    string data = sprites[startIndex];

                    EditorUtility.DisplayProgressBar("UI图片标记资源中  " + startIndex.ToString() + "/" + sprites.Count.ToString(), data, (float)startIndex / (float)sprites.Count);

                    string assetPath = data;
                    AssetImporter import = AssetImporter.GetAtPath(assetPath);

                    if (import == null || !(import is TextureImporter))
                    {
                    }
                    else
                    {
                        bool bMarking = bMark;
                        if (assetPath.Contains("/bg/"))
                            bMarking = false;
                        if (bMarking)
                        {
                            for (int i = 0; i < setting.spriteUnPak.Count; ++i)
                            {
                                if (assetPath.Contains(setting.spriteUnPak[i]))
                                {
                                    bMarking = false;
                                    break;
                                }
                            }
                        }

                        string dir = System.IO.Path.GetDirectoryName(assetPath).Replace("\\", "/");
                        if (string.IsNullOrEmpty(dir)) continue;
                        string[] splitText = dir.Split('/');
                        if (splitText.Length <= 0) continue;
                        string atlasName = dir;
                        if (splitText.Length > 3)
                            atlasName = splitText[splitText.Length - 3] + "_" + splitText[splitText.Length - 2] + "_" + splitText[splitText.Length - 1];
                        else if (splitText.Length > 2)
                            atlasName = splitText[splitText.Length - 2] + "_" + splitText[splitText.Length - 1];
                        else
                            atlasName = splitText[splitText.Length - 1];
                        if (string.IsNullOrEmpty(dir)) continue;
                        TextureImporter texurtImport = import as TextureImporter;
                        bool bReimport = false;
                        if (texurtImport.mipmapEnabled != false)
                        {
                            texurtImport.mipmapEnabled = false;
                            bReimport = true;
                        }

                        if (!string.IsNullOrEmpty(strTexPlaform))
                        {
                            TextureImporterPlatformSettings platformSetting = texurtImport.GetPlatformTextureSettings(strTexPlaform);
                            if (platformSetting == null)
                            {
                                platformSetting = texurtImport.GetDefaultPlatformTextureSettings();
                            }
                            if (platformSetting != null)
                            {
                                if (platformSetting.format.ToString().Contains("Automatic") || platformSetting.format.ToString().Contains("PVRTC_") || platformSetting.format.ToString().Contains("ETC2_") || platformSetting.format.ToString().Contains("ETC_"))
                                {
                                    platformSetting.format = TextureImporterFormat.ASTC_6x6;
                                    platformSetting.overridden = true;
                                    texurtImport.SetPlatformTextureSettings(platformSetting);
                                    bReimport = true;
                                }
                            }
                        }

                        TextureImporterSettings tex_setting = new TextureImporterSettings();
                        texurtImport.ReadTextureSettings(tex_setting);
                        if (setting != null)
                        {
                            bool bDirtySprite = false;
                            if (tex_setting.spriteGenerateFallbackPhysicsShape)
                            {
                                tex_setting.spriteGenerateFallbackPhysicsShape = false;
                                bDirtySprite = true;
                            }
                            if (tex_setting.spriteMeshType != SpriteMeshType.FullRect)
                            {
                                tex_setting.spriteMeshType = SpriteMeshType.FullRect;
                                bDirtySprite = true;
                            }
                            if (bDirtySprite)
                            {
                                texurtImport.SetTextureSettings(tex_setting);
                                bReimport = true;
                            }
                        }

                        if (bMarking)
                        {
                            if (texurtImport.spritePackingTag != atlasName)
                            {
                                //[TIGHT]atlasName [RECT]atlasName
                                texurtImport.spritePackingTag = atlasName;
                                bReimport = true;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(texurtImport.spritePackingTag))
                            {
                                texurtImport.spritePackingTag = "";
                                bReimport = true;
                            }
                        }
                        if (bReimport)
                            texurtImport.SaveAndReimport();
                    }
                }
                EditorUtility.ClearProgressBar();
            }
            else
            {
                string rootPath = "Assets/Datas/SpriteAtlas";
                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);

                Dictionary<string, SpriteAtlas> spriteAtlas = new Dictionary<string, SpriteAtlas>();
                List<FileInfo> vFiles = new List<FileInfo>();
                string[] atlas = AssetDatabase.FindAssets("t:SpriteAtlas", new string[] { rootPath });
                for (int i = 0; i < atlas.Length; ++i)
                {
                    SpriteAtlas spriteAtls = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(AssetDatabase.GUIDToAssetPath(atlas[i]));
                    if (spriteAtls)
                    {
                        SpriteAtlasExtensions.Remove(spriteAtls, SpriteAtlasExtensions.GetPackables(spriteAtls));
                        spriteAtlas[spriteAtls.name] = spriteAtls;
                        EditorUtility.SetDirty(spriteAtls);
                    }
                }

                int startIndex = 0;
                for (; startIndex < sprites.Count; ++startIndex)
                {
                    string data = sprites[startIndex];

                    EditorUtility.DisplayProgressBar("UI图片标记资源中  " + startIndex.ToString() + "/" + sprites.Count.ToString(), data, (float)startIndex / (float)sprites.Count);
                    string assetPath = data;
                    AssetImporter import = AssetImporter.GetAtPath(assetPath);

                    if (import != null || (import is TextureImporter))
                    {
                        bool bMarking = true;
                        for (int i = 0; i < setting.spriteUnPak.Count; ++i)
                        {
                            if (assetPath.Contains(setting.spriteUnPak[i]))
                            {
                                bMarking = false;
                            }
                        }
                        if (!bMarking) continue;
                        string dir = System.IO.Path.GetDirectoryName(assetPath).Replace("\\", "/");
                        if (string.IsNullOrEmpty(dir)) continue;
                        string[] splitText = dir.Split('/');
                        if (splitText.Length <= 0) continue;
                        string atlasName = dir;
                        if (splitText.Length > 3)
                            atlasName = splitText[splitText.Length - 3] + "_" + splitText[splitText.Length - 2] + "_" + splitText[splitText.Length - 1];
                        else if (splitText.Length > 2)
                            atlasName = splitText[splitText.Length - 2] + "_" + splitText[splitText.Length - 1];
                        else
                            atlasName = splitText[splitText.Length - 1];
                        if (string.IsNullOrEmpty(atlasName)) continue;

                        TextureImporter texurtImport = import as TextureImporter;
                        bool bReimport = false;
                        if (texurtImport.mipmapEnabled != false)
                        {
                            texurtImport.mipmapEnabled = false;
                            bReimport = true;
                        }

                        if (!string.IsNullOrEmpty(strTexPlaform))
                        {
                            TextureImporterPlatformSettings platformSetting = texurtImport.GetPlatformTextureSettings(strTexPlaform);
                            if (platformSetting == null)
                            {
                                platformSetting = texurtImport.GetDefaultPlatformTextureSettings();
                            }
                            if (platformSetting != null)
                            {
                                if (platformSetting.format.ToString().Contains("PVRTC_") || platformSetting.format.ToString().Contains("ETC2_") || platformSetting.format.ToString().Contains("ETC_"))
                                {
                                    platformSetting.format = TextureImporterFormat.ASTC_6x6;
                                    texurtImport.SetPlatformTextureSettings(platformSetting);
                                    bReimport = true;
                                }
                            }
                        }

                        TextureImporterSettings tex_setting = new TextureImporterSettings();
                        texurtImport.ReadTextureSettings(tex_setting);
                        if (setting != null)
                        {
                            bool bDirtySprite = false;
                            if (tex_setting.spriteGenerateFallbackPhysicsShape)
                            {
                                tex_setting.spriteGenerateFallbackPhysicsShape = false;
                                bDirtySprite = true;
                            }
                            if (tex_setting.spriteMeshType != SpriteMeshType.FullRect)
                            {
                                tex_setting.spriteMeshType = SpriteMeshType.FullRect;
                                bDirtySprite = true;
                            }
                            if (bDirtySprite)
                            {
                                texurtImport.SetTextureSettings(tex_setting);
                                bReimport = true;
                            }
                        }

                        if (!string.IsNullOrEmpty(texurtImport.spritePackingTag))
                        {
                            texurtImport.spritePackingTag = "";
                            bReimport = true;
                        }
                        if (bReimport)
                            texurtImport.SaveAndReimport();
                        if (assetPath.Contains("Assets/DatasRef/"))
                            atlasName += "_Ref";

                        string savePath = rootPath + "/" + atlasName + ".spriteatlas";
                        SpriteAtlas atlasSprites = null;
                        if (!spriteAtlas.TryGetValue(atlasName, out atlasSprites))
                        {
                            atlasSprites = new SpriteAtlas();
                            atlasSprites.name = atlasName;
                            SpriteAtlasPackingSettings packSet = new SpriteAtlasPackingSettings()
                            {
                                blockOffset = 1,
                                enableRotation = true,
                                enableTightPacking = true,
                                padding = 4,
                            };
                            atlasSprites.SetPackingSettings(packSet);
                            SpriteAtlasTextureSettings textureSet = new SpriteAtlasTextureSettings()
                            {
                                readable = false,
                                generateMipMaps = false,
                                sRGB = true,
                                filterMode = FilterMode.Bilinear,
                            };
                            atlasSprites.SetTextureSettings(textureSet);

                            AssetDatabase.CreateAsset(atlasSprites, savePath);

                            spriteAtlas.Add(atlasName, atlasSprites);

                            AssetImporter importTexture = AssetImporter.GetAtPath(savePath);
                            if (importTexture)
                            {
                                importTexture.assetBundleName = atlasName;
                                importTexture.assetBundleVariant = "pkg";
                                EditorUtility.SetDirty(importTexture);
                            }
                        }
                        SpriteAtlasExtensions.Add(atlasSprites, new Object[] { AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath) });
                        EditorUtility.SetDirty(atlasSprites);
                    }
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                foreach (var db in spriteAtlas)
                {
                    SpriteAtlas obj = db.Value;
                    UnityEngine.Object[] paks = SpriteAtlasExtensions.GetPackables(obj);
                    if (paks == null || paks.Length <= 0)
                    {
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(obj));
                    }
                }

                EditorUtility.ClearProgressBar();
            }

            Debug.Log("标记结束");
        }
        //------------------------------------------------------
        public static void OnMarkABName(string strAssetPath, string strAbName)
        {
            if (strAssetPath.EndsWith(".spriteatlas")) strAssetPath = System.IO.Path.GetFileNameWithoutExtension(strAssetPath);
            m_vMarkAbs[strAssetPath] = strAbName;
        }
        //------------------------------------------------------
        public static string OnPrepareMarkAbName(string strAssetPath)
        {
            return null;
        }
        //------------------------------------------------------
        public static void BuildAB(PublishSetting setting, bool bBuildPackage, bool bOpenExplorer, bool bBuildAB = true, bool versionCheck = false)
        {
            if (setting.bundleExportDir.Length <= 0)
            {
                return;
            }
            PublishPanel.WritePublishProgress("BuildAB-Begin");
            string outPut = setting.GetBuildTargetOutputDir();
            if (setting.delPrePackageDir)
            {
                EditorKits.DeleteDirectory(outPut);
            }
            if (!Directory.Exists(outPut))
                Directory.CreateDirectory(outPut);

            //! build asset mapping ab
            AbMapping abMapping = BuildAssetMappingAB(setting);

            PublishSetting.Platform platform = setting.GetPlatform(setting.buildTarget);

            BuildVersionFile(setting, "version.ver", platform.version);
            CopyToStreamRawAssets(setting);


            //for (int i = 0; i < setting.unbuildDirs.Count; ++i)
            //{
            //    CompileAssetBundles.MarkAssetBundleAysnByDir(setting, setting.unbuildDirs[i], false, false, false, false);
            //}

            RenderPipelineAsset urpAsset = AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>("Assets/DatasRef/Config/RenderURP/Default/UniversalRenderPipelineAsset.asset");
            if (QualitySettings.renderPipeline == null)
                QualitySettings.renderPipeline = urpAsset;

            if (UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset == null)
            {
                UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset = urpAsset;
            }

            if (setting.buildTarget == BuildTarget.iOS)
            {
                SerializedObject graphicsSettings = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0]);
                SerializedProperty it = graphicsSettings.GetIterator();
                while (it.NextVisible(true))
                {
                    if (it.name == "m_InstancingStripping")
                    {
                        it.intValue = 2;//Kepp All
                        graphicsSettings.ApplyModifiedProperties();
                        break;
                    }
                }
            }

            bool fog = RenderSettings.fog;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogDensity = 1;
            RenderSettings.fogStartDistance = 0;
            RenderSettings.fogEndDistance = 100;
            AssetBundleManifest manifest = null;
            if (bBuildAB)
                manifest = CompileAssetBundles.BuildAB(outPut, setting.buildOptions, setting.buildTarget, setting.strEncryptKey);

            RenderSettings.fog = fog;
            if (setting.copyToStreamAsset)
            {
                EditorKits.DeleteDirectory(Application.dataPath + "/StreamingAssets/packages/");
                HashSet<string> ignoreSet = new HashSet<string>();
                ignoreSet.Add(".manifest");
                ignoreSet.Add(".txt");
                ignoreSet.Add(".ver");
                EditorKits.CopyDir(outPut, Application.dataPath + "/StreamingAssets/packages/", null, ignoreSet);
            }

            AssetVersionInfo versionInfo = BuildAssetBundleMD5(setting, manifest, "resInfo.json", abMapping);
            if (setting.versionCheck || versionCheck)
            {
                int version = (int)setting.GetBuildTargetVersion(setting.buildTarget);
                //! hot check
                switch (setting.hotType)
                {
                    case EHotType.InjectFix:
                        {
#if USE_INJECTFIX
                            if (setting.buildTarget == BuildTarget.Android)
                                IFix.Editor.IFixEditor.CompileToAndroid(version);
                            else if (setting.buildTarget == BuildTarget.iOS)
                                IFix.Editor.IFixEditor.CompileToIOS(version);
                            else if (setting.buildTarget == BuildTarget.StandaloneWindows)
                                IFix.Editor.IFixEditor.Patch(version);
#endif
                        }
                        break;
#if UNITY_2020_3_OR_NEWER
                    case EHotType.HybridCLR:
                        {
#if USE_HYBRIDCLR
                            if (HybridCLRHelper.IsEnable && !HybridCLRHelper.CheckUp())
                                return;
#endif
                        }
                        break;
#endif
                }
                FilterUpdateAssetBundle(setting, "resinfo.json", platform.version, platform.version, false, versionInfo);
            }

            if (setting.useEncrptyPak)
            {
                PackagePanel.ClearEncrptyPaks(setting);
                PackagePanel.BuildEncrptyPak(setting, null);
            }
            PublishPanel.WritePublishProgress("BuildAB-End");
            if (bBuildPackage)
            {
                BuildPlayer(setting);
            }
            else
            {
                if (bOpenExplorer)
                {
#if !UNITY_EDITOR_OSX
                    EditorKits.OpenPathInExplorer(outPut);
#endif
                }
            }
        }
        //------------------------------------------------------
        void DrawTile(string label, Color color, float width = 0, float height = 20)
        {
            Color back = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUILayout.Box(label, new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) });
            GUI.backgroundColor = back;
        }
        //------------------------------------------------------
        public static void WritePublishProgress(string text)
        {
            if (!Directory.Exists(Application.dataPath + "/../Publishs/Temps"))
                Directory.CreateDirectory(Application.dataPath + "/../Publishs/Temps");
            string publishingName = Application.dataPath + "/../Publishs/Temps/publish_progress.txt";
            if (File.Exists(publishingName)) File.Delete(publishingName);
            File.WriteAllText(publishingName, text);
        }
        //------------------------------------------------------
        public static void WriteBuildIL2Cpp(string copyTo)
        {
            if (!Directory.Exists(Application.dataPath + "/../Publishs/Temps"))
                Directory.CreateDirectory(Application.dataPath + "/../Publishs/Temps");
            string publishingName = Application.dataPath + "/../Publishs/Temps/il2cppbuild.txt";
            if (File.Exists(publishingName)) File.Delete(publishingName);
            File.WriteAllText(publishingName, copyTo);
        }
        //------------------------------------------------------
        public static void DelBuildIL2CppFile()
        {
            string publishingName = Application.dataPath + "/../Publishs/Temps/il2cppbuild.txt";
            if (File.Exists(publishingName)) File.Delete(publishingName);
        }
        //------------------------------------------------------
        public static void WritePublishingNameFile(PublishSetting setting)
        {
            if (!Directory.Exists(Application.dataPath + "/../Publishs/Temps"))
                Directory.CreateDirectory(Application.dataPath + "/../Publishs/Temps");
            string pkgName = PublishPanel.ConvertProjectName(setting.GetPlatform(setting.buildTarget).projectExportName);
            string publishingName = Application.dataPath + "/../Publishs/Temps/publishingName.txt";
            if (File.Exists(publishingName)) File.Delete(publishingName);
            File.WriteAllText(publishingName, pkgName);
        }
        //------------------------------------------------------
        public static void WriteProductFile(string file)
        {
            if (!Directory.Exists(Application.dataPath + "/../Publishs/Temps"))
                Directory.CreateDirectory(Application.dataPath + "/../Publishs/Temps");
            string publishingName = Application.dataPath + "/../Publishs/Temps/publishingProcudt.txt";
            if (File.Exists(publishingName)) File.Delete(publishingName);
            File.WriteAllText(publishingName, file);
        }
        //------------------------------------------------------
        public static void DelProductFile()
        {
            string publishingName = Application.dataPath + "/../Publishs/Temps/publishingProcudt.txt";
            if (File.Exists(publishingName)) File.Delete(publishingName);
        }
        //------------------------------------------------------
        static void WritePublishSettingInfo(PublishSetting setting)
        {
            if (!Directory.Exists(Application.dataPath + "/../Publishs/Temps"))
                Directory.CreateDirectory(Application.dataPath + "/../Publishs/Temps");

            string strSettingInfo = Application.dataPath + "/../Publishs/Temps/settingInfo.txt";
            if (File.Exists(strSettingInfo)) File.Delete(strSettingInfo);
            var plaform = setting.GetPlatform(setting.buildTarget);
            string info = "版本：" + plaform.version + "." + plaform.versionCode + "\r\n";
            info += "Debug版本：" + setting.debug + "\r\n";
            info += "分析调试：" + setting.profiler + "\r\n";
            info += "脚本调试：" + setting.scriptDebug + "\r\n";
            info += "深度调试：" + setting.depthPrifiling + "\r\n";
            info += "自动检测热更资源：" + setting.versionCheck + "\r\n";
            info += "代码热更方案：" + TopGame.ED.EditorHelp.GetDisplayName(setting.hotType);
#if USE_HYBRIDCLR
            if (setting.hotType == EHotType.HybridCLR)
            {
                info += "(";
                for (int i =0; i < HybridCLRHelper.HotUpdateAssemblys.Count; ++i)
                {
                    info += HybridCLRHelper.HotUpdateAssemblys[i];
                    if (i < HybridCLRHelper.HotUpdateAssemblys.Count - 1) info += ",";
                }
                info += ")\r\n";
            }
            else info += "\r\n";
#else
                info += "\r\n";
#endif

            if (plaform.defaultLanguage != SystemLanguage.Unknown)
            {
                info += "默认语言：" + plaform.defaultLanguage + "\r\n";
            }
            info += "global-metadata.dat 加密：" + setting.globalMetaDataEncrypt + "\r\n";
            info += "资源加固：" + setting.useEncrptyPak + "\r\n";
            info += "分包obb打包方式：" + setting.obb + "\r\n";
            info += "显示FPS：" + setting.showFps + "\r\n";
            info += "包含GM控制台/调试信息界面：" + setting.gmPanel + "\r\n";
            info += "防沉迷：" + setting.antiAddiction + "\r\n";
            for (int i = 0; i < setting.serverUrlList.Count; ++i)
            {
                if (setting.serverUrlList[i].bUsed)
                {
                    info += "链接的服务器：" + setting.serverUrlList[i].name + "\r\n";
                    break;
                }
            }
            info += "包含SDK：";
            for (int i = 0; i < plaform.copySdks.Count; ++i)
            {
                if (plaform.copySdks[i].bUsed)
                {
                    info += plaform.copySdks[i].sdkMarco + "  ";
                }
            }

            File.WriteAllText(strSettingInfo, info, System.Text.Encoding.UTF8);
        }
        //------------------------------------------------------
        static void CopyLocalIL2Cpp(PublishSetting setting)
        {
            if (setting.globalMetaDataEncrypt)
            {
                string strLocalRoot = "";
                string strLocalIL2cpp = "";
#if USE_HYBRIDCLR
                if (setting.hotType == EHotType.HybridCLR && HybridCLRHelper.IsEnable)
                {
                    strLocalIL2cpp = HybridCLRHelper.LocalIl2CppDir;
                    strLocalRoot = HybridCLRHelper.LocalUnityDataDir;
                }
                else
#endif
                {
                    strLocalRoot = Application.dataPath + "/../Publishs/Temps/LocalIl2CppData";
                    if (Directory.Exists(strLocalIL2cpp))
                        Directory.Delete(strLocalIL2cpp, true);
                    string excudeRoot = EditorApplication.applicationContentsPath.Replace("\\", "/");
                    if (excudeRoot[excudeRoot.Length - 1] != '/') excudeRoot += "/";
                    string il2cpp = excudeRoot + "/il2cpp";
                    string monobleeding = excudeRoot + "/MonoBleedingEdge";
                    if (!Directory.Exists(il2cpp) || !Directory.Exists(monobleeding))
                    {
                        System.Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", null);
                        return;
                    }
                    ED.EditorKits.CopyDir(il2cpp, strLocalRoot + "/il2cpp");
                    ED.EditorKits.CopyDir(monobleeding, strLocalRoot + "/MonoBleedingEdge");
                    strLocalIL2cpp = strLocalRoot + "/il2cpp";
                }


                if (!string.IsNullOrEmpty(strLocalRoot) && Directory.Exists(strLocalRoot))
                {
                    string strDirTemp = Application.dataPath + "/../Tools/HybridCLRData/Copys";
                    string version = Application.unityVersion;
                    string[] files = Directory.GetFiles(strDirTemp, "*.*", SearchOption.AllDirectories);
                    for (int i = 0; i < files.Length; ++i)
                    {
                        string tempFile = files[i].Replace("\\", "/");
                        string toFile = tempFile.Replace(strDirTemp, strLocalRoot);
                        File.Copy(tempFile, toFile, true);
                    }
                }
                if (!string.IsNullOrEmpty(strLocalIL2cpp) && Directory.Exists(strLocalIL2cpp))
                {
                    System.Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", strLocalIL2cpp);
                }
                else
                    System.Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", null);
            }
            else
            {
#if USE_HYBRIDCLR
                string strDir = EditorApplication.applicationContentsPath.Replace("\\", "/");
                if (strDir[strDir.Length - 1] != '/') strDir += "/";
                string strFile = strDir + "il2cpp/libil2cpp/vm/MetadataLoader.cpp";
                if(File.Exists(strFile))
                {
                    string strLocalIL2cpp = "";
                    strLocalIL2cpp = HybridCLRHelper.LocalIl2CppDir + "/libil2cpp/vm/MetadataLoader.cpp";
                    if(File.Exists(strLocalIL2cpp))
                        File.Copy(strFile, strLocalIL2cpp, true);
                }
#endif
                DeleteLocalIL2Cpp();
            }
        }
        //------------------------------------------------------
        public static void DeleteLocalIL2Cpp()
        {
            string strLocalIL2cpp = Application.dataPath + "/../Publishs/Temps/LocalIl2CppData";
            if (Directory.Exists(strLocalIL2cpp))
                Directory.Delete(strLocalIL2cpp, true);
            System.Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", null);
        }
        //------------------------------------------------------
        static string ConvertProjectName(string name)
        {
            string ret = "";
            for (int i = 0; i < name.Length; ++i)
            {
                if ((name[i] >= 0 && name[i] <= 9) || (name[i] >= 'a' && name[i] <= 'z') || (name[i] >= 'A' && name[i] <= 'Z'))
                    ret += name[i];
                else
                    ret += "_";
            }
            return ret;
        }
        //------------------------------------------------------
        static void OnActiveBuildTargetChanged(PublishSetting setting)
        {
            Debug.Log("Switched build target to " + setting.buildTarget);
            PublishPanel.WritePublishProgress("OnActiveBuildTargetChanged-Begin");
            PublishSetting.Platform platform = setting.GetPlatform(setting.buildTarget);

            List<string> Scenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                Scenes.Add(scene.path);
            }
            if (setting.portait)
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            else
                PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;

            BuildTargetGroup buildGroup = BuildTargetGroup.Unknown;
            bool bCheckObb = false;
            string buildTargetDir = "";
            string projectName = platform.projectName;
            string pkgName = ConvertProjectName(platform.projectExportName);
            WritePublishingNameFile(setting);
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    if (setting.dateName)
                        pkgName += string.Format("[{0}][{1}_{2}_{3}_{4}_{5}_{6}]", platform.version + "." + platform.versionCode, System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second);
                    buildTargetDir = Application.dataPath.Replace("/Assets", "/Assets/../Publishs/Android/") + pkgName + ".apk";
                    WriteProductFile(buildTargetDir);
                    AutoBuild.APKPath = buildTargetDir;
                    PlayerSettings.applicationIdentifier = platform.bundleName;
                    EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
                    buildGroup = BuildTargetGroup.Android;
                    bCheckObb = true;
                    if (setting.bIL2CPP)
                    {
                        PlayerSettings.SetScriptingBackend(buildGroup, ScriptingImplementation.IL2CPP);
                        AndroidArchitecture aac = AndroidArchitecture.ARM64;
                        if (setting.armv7) aac |= AndroidArchitecture.ARMv7;
                        PlayerSettings.Android.targetArchitectures = aac;
                    }
                    else
                    {
                        PlayerSettings.SetScriptingBackend(buildGroup, ScriptingImplementation.Mono2x);
                        AndroidArchitecture aac = AndroidArchitecture.ARMv7;// | AndroidArchitecture.ARM64;
                        PlayerSettings.Android.targetArchitectures = aac;
                    }

                    if (PlayerSettings.Android.useCustomKeystore)
                    {
                        PlayerSettings.Android.keystorePass = "happyland12345678sd";
                        PlayerSettings.Android.keyaliasPass = "happyland12345678sd";
                    }

                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier1);
                        tierSetting.hdr = false;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.Low;
                    }

                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier2);
                        tierSetting.hdr = true;
                        tierSetting.hdrMode = CameraHDRMode.FP16;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.Medium;
                    }
                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier3);
                        tierSetting.hdr = true;
                        tierSetting.hdrMode = CameraHDRMode.R11G11B10;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.Medium;
                    }
                    PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.Auto;
                    PlayerSettings.Android.minSdkVersion = setting.androidSdkVersion;
                    PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
#if USE_ZSSDK
                    PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
#endif

                    UnityEditor.EditorSettings.externalVersionControl = "Disabled";
                    break;

                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:

                    buildTargetDir = Application.dataPath.Replace("/Assets", "/Assets/../Publishs/Window/") + pkgName + ".exe";
                    WriteProductFile(buildTargetDir);
                    buildGroup = BuildTargetGroup.Standalone;
                    if (setting.bIL2CPP)
                        PlayerSettings.SetScriptingBackend(buildGroup, ScriptingImplementation.IL2CPP);
                    else
                        PlayerSettings.SetScriptingBackend(buildGroup, ScriptingImplementation.Mono2x);
                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier1);
                        tierSetting.hdr = false;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.Low;
                    }

                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier2);
                        tierSetting.hdr = true;
                        tierSetting.hdrMode = CameraHDRMode.FP16;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.High;
                    }
                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier3);
                        tierSetting.hdr = true;
                        tierSetting.hdrMode = CameraHDRMode.R11G11B10;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.High;
                    }
                    UnityEditor.PlayerSettings.defaultScreenWidth = 1280;
                    UnityEditor.PlayerSettings.defaultScreenHeight = 720;
                    UnityEditor.PlayerSettings.forceSingleInstance = true;
                    UnityEditor.PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
                    break;

                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXIntel:

                    buildTargetDir = Application.dataPath.Replace("/Assets", "/Assets/../Publishs/OSX/") + pkgName;
                    buildGroup = BuildTargetGroup.Standalone;
                    if (setting.bIL2CPP)
                        PlayerSettings.SetScriptingBackend(buildGroup, ScriptingImplementation.IL2CPP);
                    else
                        PlayerSettings.SetScriptingBackend(buildGroup, ScriptingImplementation.Mono2x);
                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier1);
                        tierSetting.hdr = false;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.Low;
                    }

                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier2);
                        tierSetting.hdr = true;
                        tierSetting.hdrMode = CameraHDRMode.FP16;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.High;
                    }
                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier3);
                        tierSetting.hdr = true;
                        tierSetting.hdrMode = CameraHDRMode.R11G11B10;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.High;
                    }
                    UnityEditor.PlayerSettings.defaultScreenWidth = 1280;
                    UnityEditor.PlayerSettings.defaultScreenHeight = 720;
                    UnityEditor.PlayerSettings.forceSingleInstance = true;
                    UnityEditor.PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
                    break;

                case BuildTarget.iOS:
                    projectName = platform.projectExportName;
                    buildTargetDir = Application.dataPath.Replace("/Assets", "/Assets/../Publishs/IOS/") + pkgName;
                    WriteProductFile(Application.dataPath.Replace("/Assets", "/Assets/../Publishs/IOS/") + "build/"+ projectName + ".ipa");
                    buildGroup = BuildTargetGroup.iOS;
                    PlayerSettings.iOS.applicationDisplayName = platform.projectName;
                    PlayerSettings.iOS.buildNumber = platform.versionCode.ToString();
                    PlayerSettings.SetScriptingBackend(buildGroup, ScriptingImplementation.IL2CPP);
                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier1);
                        tierSetting.hdr = false;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.Low;
                    }

                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier2);
                        tierSetting.hdr = true;
                        tierSetting.hdrMode = CameraHDRMode.FP16;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.Medium;
                    }
                    {
                        UnityEditor.Rendering.TierSettings tierSetting = UnityEditor.Rendering.EditorGraphicsSettings.GetTierSettings(buildGroup, GraphicsTier.Tier3);
                        tierSetting.hdr = true;
                        tierSetting.hdrMode = CameraHDRMode.R11G11B10;
                        tierSetting.standardShaderQuality = UnityEditor.Rendering.ShaderQuality.Medium;
                    }
                    break;

                case BuildTarget.WebGL:
                    {
                        PlayerSettings.WebGL.decompressionFallback = true;

                        projectName = platform.projectExportName;
                        buildTargetDir = Application.dataPath.Replace("/Assets", "/Assets/../Publishs/WebGL/") + pkgName;
                        WriteProductFile(Application.dataPath.Replace("/Assets", "/Assets/../Publishs/WebGL/") + "build/" + projectName + ".html");
                        buildGroup = BuildTargetGroup.WebGL;
                    }
                    break;

                default:
                    EditorUtility.DisplayDialog("提示", "暂未支持该平台发包,如果确认需要，请用unity 内置的发包系统!", "好的");
                    return;
            }
            DelBuildIL2CppFile();

            PlayerSettings.graphicsJobs = true;
            PlayerSettings.MTRendering = true;
            PlayerSettings.bundleVersion = platform.version;

            PlayerSettings.applicationIdentifier = platform.bundleName;
            UnityEditor.PlayerSettings.productName = projectName;
            if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
#if USE_LZT
                PlayerSettings.iOS.cameraUsageDescription = "保存用户名密码需要使用到您的相册";
#endif
                PlayerSettings.iOS.applicationDisplayName = platform.projectName;
            }

            //! splash 
            PlayerSettings.SplashScreen.show = false;
            PlayerSettings.SplashScreen.showUnityLogo = false;

            //! shader variants
            for (int i = 0; i < platform.shadervariants.Count; ++i)
            {
                if (string.IsNullOrEmpty(platform.shadervariants[i])) continue;
                ShaderVariantCollection svc = AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(platform.shadervariants[i]);
                if (svc != null)
                {
                    UnityEngine.Object[] preloadshaders = PlayerSettings.GetPreloadedAssets();
                    List<UnityEngine.Object> vList = preloadshaders != null ? new List<UnityEngine.Object>(preloadshaders) : new List<UnityEngine.Object>();
                    if (!vList.Contains(svc))
                    {
                        vList.Add(svc);
                        PlayerSettings.SetPreloadedAssets(vList.ToArray());
                    }
                }
            }

            SetShaderIncludes(setting);

            string outPut = setting.GetBuildTargetOutputDir();

            if (!setting.copyToStreamAsset)
            {
                EditorKits.DeleteDirectory(Application.dataPath + "/StreamingAssets/packages/");

                if (setting.useEncrptyPak)
                {
                    EditorKits.CopyDir(setting.GetBuildTargetEncrtpyDir(), Application.dataPath + "/StreamingAssets/packages/", null);
                }
                else
                {
                    HashSet<string> ignoreSet = new HashSet<string>();
                    ignoreSet.Add(".manifest");
                    ignoreSet.Add(".ver");
                    ignoreSet.Add(".txt");
                    EditorKits.CopyDir(outPut, Application.dataPath + "/StreamingAssets/packages/", null, ignoreSet);
                }
            }
            if (File.Exists(outPut + "/version.ver"))
            {
                VersionData.Config config = JsonUtility.FromJson<VersionData.Config>(File.ReadAllText(outPut + "/version.ver"));
                if (File.Exists(Application.dataPath + "/Resources/version.txt")) File.Delete(Application.dataPath + "/Resources/version.txt");//old file suffix
                config.Save(Application.dataPath + "/Resources/version.bytes");
            }
            //EditorKits.CopyFile(outPut + "/version.ver", Application.dataPath + "/Resources/version.txt");
            if (!setting.useEncrptyPak)
                EditorKits.CopyFile(outPut + "/mapping.txt", Application.dataPath + "/Resources/mapping.txt");

            CopySDKToPluginAssets(setting);
#if UNITY_2020_3_OR_NEWER
            if (setting.hotType == EHotType.HybridCLR)
            {
#if USE_HYBRIDCLR
                if (!HybridCLRHelper.CheckUp())
                    return;
                HybridCLR.Editor.SettingsUtil.Enable = true;
                PlayerSettings.gcIncremental = false;
                PlayerSettings.SetApiCompatibilityLevel(buildGroup, ApiCompatibilityLevel.NET_4_6);
                WriteBuildIL2Cpp(Path.Combine(buildTargetDir, "Libraries", "libil2cpp.a"));
#endif
            }
            else
            {
#if USE_HYBRIDCLR
                HybridCLR.Editor.SettingsUtil.Enable = false;
#endif
                //! copy local il2cpp

#if UNITY_WEBGL
#else
                PlayerSettings.SetApiCompatibilityLevel(buildGroup, ApiCompatibilityLevel.NET_Standard_2_0);
#endif
            }
#endif
                if (bCheckObb)
            {
                if (setting.obb)
                    PlayerSettings.Android.useAPKExpansionFiles = true;
                else
                    PlayerSettings.Android.useAPKExpansionFiles = false;
            }

            //! hot check
            switch (setting.hotType)
            {
                case EHotType.InjectFix:
                    {
#if USE_INJECTFIX
                        IFix.Editor.IFixEditor.AutoInjectAssemblys();
#endif
                    }
                    break;
            }

            CopyLocalIL2Cpp(setting);
            WritePublishSettingInfo(setting);

            Debug.Log("Start Build " + EditorUserBuildSettings.activeBuildTarget);
            BuildOptions buildOps = BuildOptions.None;// BuildOptions.AcceptExternalModificationsToPlayer;
            if (setting.sceneStreamAb) buildOps |= BuildOptions.BuildAdditionalStreamedScenes;
            if (setting.buildAutoRun) buildOps |= BuildOptions.AutoRunPlayer;
            if (setting.profiler || setting.debug) buildOps |= BuildOptions.Development;
            if (setting.autoConnectProfiler) buildOps |= BuildOptions.ConnectWithProfiler;
            if (setting.scriptDebug) buildOps |= BuildOptions.AllowDebugging;
            EditorUserBuildSettings.buildWithDeepProfilingSupport = setting.depthPrifiling;
            EditorUserBuildSettings.waitForManagedDebugger = setting.waitForManagerDebuger;
            PublishPanel.WritePublishProgress("BuildPipeline.BuildPlayer-Begin");
            UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(Scenes.ToArray(), buildTargetDir, EditorUserBuildSettings.activeBuildTarget, buildOps);
            PublishPanel.WritePublishProgress("BuildPipeline.BuildPlayer-End:" + report.summary.result);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.LogError("BuildPlayer failure: " + report.summary.result);
                DelProductFile();
            }
            else
            {
#if USE_HYBRIDCLR
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
                {
                    //! copy libil2cpp.a
                    string strFile = HybridCLRHelper.IOSIl2CPPLibFile;
                    if (File.Exists(strFile))
                    {
                        string toFile = Path.Combine(buildTargetDir, "Libraries", "libil2cpp.a");
                        if (File.Exists(toFile))
                        {
                            File.Copy(strFile, toFile, true);
                            Debug.Log("COPY libil2cpp.a");
                        }
                    }
                }
#endif
                platform.versionCode++;
                SaveSetting(setting);
#if !UNITY_EDITOR_OSX
                EditorKits.OpenPathInExplorer(report.summary.outputPath);
#endif
            }
            //! delete packages
            EditorKits.DeleteDirectory(Application.dataPath + "/StreamingAssets/packages/");
            DeleteLocalIL2Cpp();
        }
        //------------------------------------------------------
        static void SetShaderIncludes(PublishSetting setting)
        {
            //include shader
            if (setting.includeShaders.Count > 0)
            {
                SerializedObject graphicsSettings = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0]);
                SerializedProperty it = graphicsSettings.GetIterator();
                SerializedProperty dataPoint;
                while (it.NextVisible(true))
                {
                    if (it.name == "m_AlwaysIncludedShaders")
                    {
                        for (int i = 0; i < setting.includeShaders.Count; i++)
                        {
                            var shader = Shader.Find(setting.includeShaders[i]);
                            if (!shader)
                            {
                                continue;
                            }
                            int index = -1;
                            bool bExisted = false;
                            for (int j = 0; j < it.arraySize; ++j)
                            {
                                SerializedProperty oldData = it.GetArrayElementAtIndex(j);
                                if (oldData != null)
                                {
                                    if (oldData.objectReferenceValue == shader)
                                    {
                                        bExisted = true;
                                        break;
                                    }
                                    else if (oldData.objectReferenceValue == null)
                                    {
                                        index = j;
                                    }
                                }
                            }
                            if (bExisted) continue;
                            if (index == -1)
                            {
                                index = it.arraySize;
                                it.InsertArrayElementAtIndex(index);
                            }
                            dataPoint = it.GetArrayElementAtIndex(index);

                            dataPoint.objectReferenceValue = shader;
                        }
                        graphicsSettings.ApplyModifiedProperties();
                        UnityEngine.Debug.Log("RefreshShaders 完成");

                        break;
                    }
                }
            }
        }
        //------------------------------------------------------
        static void CopyToStreamRawAssets(PublishSetting setting)
        {
            string outPut = setting.GetBuildTargetOutputDir() + "/raws/";

#if USE_HYBRIDCLR
            if (HybridCLRHelper.IsEnable && HybridCLRHelper.CheckUp())
            {
                HybridCLR.Editor.SettingsUtil.Enable = true;
                PublishPanel.WritePublishProgress("HybridCLRHelper.GenerateAll-Begin");
                HybridCLRHelper.GenerateAll();
                PublishPanel.WritePublishProgress("HybridCLRHelper.GenerateAll-End");
            }
#endif

            for (int i = 0; i < setting.copyToStreamDirs.Count; ++i)
            {
                string strToDir = outPut + setting.copyToStreamDirs[i].toDir;
                EditorKits.DeleteDirectory(strToDir);
            }
            string toDll = Application.dataPath + "/../Binarys/HotScripts/";
            ED.EditorKits.DeleteDirectory(toDll);
            //! copy HYBRIDCLR hot dll
#if USE_HYBRIDCLR
            if(HybridCLRHelper.IsEnable)
            {
                Hot.HotScriptsModule.HyBridHots hotBrids = new Hot.HotScriptsModule.HyBridHots();
                hotBrids.aotDlls = new List<string>();
                hotBrids.hotDlls = new List<string>();
                hotBrids.diffDlls = new List<string>();
                if (!Directory.Exists(toDll))
                    Directory.CreateDirectory(toDll);
                string tempDll = HybridCLRHelper.HotUpdateDllsOutputDir;
                string tempDll1 = HybridCLRHelper.AssembliesPostIl2CppStripDir;
              //  HybridCLRHelper.GenerateAll();
                for (int i = 0; i < HybridCLRHelper.AOTAssemblyNames.Count; ++i)
                {
                    string file = tempDll1 + "/" + HybridCLRHelper.AOTAssemblyNames[i];
                    if (File.Exists(file))
                    {
                        File.Copy(file, toDll + HybridCLRHelper.AOTAssemblyNames[i]);
                        hotBrids.aotDlls.Add(HybridCLRHelper.AOTAssemblyNames[i]);
                    }
                    else
                    {
                        file = tempDll + "/" + HybridCLRHelper.AOTAssemblyNames[i];
                        if (File.Exists(file))
                        {
                            File.Copy(file, toDll + HybridCLRHelper.AOTAssemblyNames[i]);
                            hotBrids.aotDlls.Add(HybridCLRHelper.AOTAssemblyNames[i]);
                        }
                    }
                }
                for (int i = 0; i < HybridCLRHelper.HotUpdateAssemblys.Count; ++i)
                {
                    string file = tempDll1 + "/" + HybridCLRHelper.HotUpdateAssemblys[i];
                    if (File.Exists(file))
                    {
                        File.Copy(file, toDll + HybridCLRHelper.HotUpdateAssemblys[i]);
                        hotBrids.hotDlls.Add(HybridCLRHelper.HotUpdateAssemblys[i]);
                    }
                    else
                    {
                        file = tempDll + "/" + HybridCLRHelper.HotUpdateAssemblys[i];
                        if (File.Exists(file))
                        {
                            File.Copy(file, toDll + HybridCLRHelper.HotUpdateAssemblys[i]);
                            hotBrids.hotDlls.Add(HybridCLRHelper.HotUpdateAssemblys[i]);
                        }
                    }
                }
                if (!Directory.Exists(Application.dataPath + "/Resources"))
                    Directory.CreateDirectory(Application.dataPath + "/Resources");
                string hotBridCrlFile = Application.dataPath + "/Resources/hotdlls.txt";
                if (hotBrids.aotDlls.Count > 0 || hotBrids.hotDlls.Count > 0 || hotBrids.diffDlls.Count > 0)
                {
                    string hotBridContents = JsonUtility.ToJson(hotBrids, true);
                    File.WriteAllText(hotBridCrlFile, hotBridContents, Encoding.UTF8);
                }
                else if (File.Exists(hotBridCrlFile))
                    File.Delete(hotBridCrlFile);
            }
            else
            {
                string hotBridCrlFile = Application.dataPath + "/Resources/hotdlls.txt";
                if (File.Exists(hotBridCrlFile))
                    File.Delete(hotBridCrlFile);
            }
#endif

            for (int i = 0; i < setting.copyToStreamDirs.Count; ++i)
            {
                string strDir = Application.dataPath;
                string start = setting.copyToStreamDirs[i].srcDir;
                if (start.StartsWith("Assets/"))
                {
                    strDir += start.Substring("Assets".Length);
                }
                else
                {
                    if (start.StartsWith("/")) strDir += start;
                    else strDir += "/" + start;
                }
                string strToDir = outPut + setting.copyToStreamDirs[i].toDir;
                if (!Directory.Exists(strToDir))
                    Directory.CreateDirectory(strToDir);
                if (strToDir[strToDir.Length - 1] != '/') strToDir += "/";
                if (File.Exists(strDir))
                {
                    string strFileName = Path.GetFileName(strDir);
                    string strToFile = strToDir + strFileName;
                    EditorKits.CopyFile(strDir, strToFile);
                }
                else if (Directory.Exists(strDir))
                {
                    HashSet<string> vIngore = new HashSet<string>();
                    vIngore.Add(".meta");
                    EditorKits.CopyDir(strDir, strToDir, null, vIngore);
                }
            }
        }
        //------------------------------------------------------
        public static void CopyMarcoToProject(PublishSetting setting, HashSet<string> vSets)
        {
            if (setting.marcoDatas != null)
            {
                for (int i = 0; i < setting.marcoDatas.Count; ++i)
                {
                    MarcoData marco = setting.marcoDatas[i];
                    if (string.IsNullOrEmpty(marco.marco))
                        continue;
                    if (vSets.Contains(marco.marco))
                    {
                        if (!string.IsNullOrEmpty(marco.pluginDatas))
                        {
                            string strSrcRoot = Application.dataPath + marco.pluginDatas + "/";
                            string strToRoot = Application.dataPath;
                            string strToDir = strToRoot + "/";
                            if (Directory.Exists(strSrcRoot))
                            {
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
                                    if (File.Exists(strFile))
                                        continue;
                                    File.Copy(srcFile, strFile);
                                }
                            }
                        }
                    }
                    else
                    {
                        string strSrcRoot = Application.dataPath + marco.pluginDatas + "/";
                        string strToRoot = Application.dataPath;
                        string strToDir = strToRoot + "/";
                        if (Directory.Exists(strSrcRoot) && Directory.Exists(strToDir))
                        {
                            string[] files = Directory.GetFiles(strSrcRoot, "*.*", SearchOption.AllDirectories);
                            List<string> fileSets = new List<string>();
                            for (int j = 0; j < files.Length; ++j)
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
                    }
                }
            }
        }
        //------------------------------------------------------
        static void CopySDKToPluginAssets(PublishSetting setting)
        {
            PublishPanel.WritePublishProgress("CopySDKToPluginAssets-Begin");
            SDKPublishCopy.CopySDKToPluginAssets(setting);
            PublishPanel.WritePublishProgress("CopySDKToPluginAssets-End");
        }
        //------------------------------------------------------
        static void BuildPlayer(PublishSetting setting)
        {
            if (EditorUserBuildSettings.activeBuildTarget != setting.buildTarget)
            {
                if (setting.buildTarget == BuildTarget.StandaloneWindows || setting.buildTarget == BuildTarget.StandaloneWindows64)
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, setting.buildTarget);
                else if (setting.buildTarget == BuildTarget.Android)
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, setting.buildTarget);
                else if (setting.buildTarget == BuildTarget.iOS)
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, setting.buildTarget);
                else
                    return;
            }
            OnActiveBuildTargetChanged(setting);
        }
        //------------------------------------------------------
        [System.Serializable]
        public class AssetVersionInfo
        {
            [System.Serializable]
            public struct AssetData
            {
                public string name;
                public string md5;
                public long size;
                public int version;
                public string[] depends;
            }
            [System.Serializable]
            public struct RawAsset
            {
                public string name;
                public string md5;
                public long size;
                public int version;
            }
            public List<RawAsset> rawAssets = new List<RawAsset>();

            public List<AssetData> resInfos = new List<AssetData>();

            public AbMapping abMapping;
        }
        //------------------------------------------------------
        public static void FilterUpdateAssetBundle(PublishSetting setting, string strResInfoFile, string curVersion, string version, bool bUploadVersionServer = false, AssetVersionInfo curVersionInfo = null)
        {
            //             if(version.CompareTo(curVersion) == 0)
            //             {
            //                 EditorUtility.DisplayDialog("提示", "检测[" + version + "]" + "<=" + "当前[" + curVersion + "]", "Ok");
            //                 return;
            //             }
            if (setting.useEncrptyPak)
            {

                EditorUtility.DisplayProgressBar("当前[" + curVersion + "]->检测[" + version + "]", "", 0);
                string outPut = setting.GetBuildOutputRoot(setting.buildTarget);

                string strInfoFile = outPut + "/" + version + "/updates.json";
                string strVersionBasePkgDir = outPut + "/" + version + "/encrpty_packages";
                string strUpdatePkgDir = outPut + "/" + version + "/updates";
                string updatepkgresInfo = outPut + "/" + strResInfoFile;
                string updatepkgresVerisonInfo = outPut + "/" + version + "/" + strResInfoFile;
                if (!Directory.Exists(outPut + "/" + version))
                {
                    Directory.CreateDirectory(outPut + "/" + version);
                }
                bool isFirstUpdatePkg = false;
                if (!Directory.Exists(strVersionBasePkgDir))
                {
                    isFirstUpdatePkg = true;
                    Directory.CreateDirectory(strVersionBasePkgDir);
                    EditorKits.CopyDir(outPut + "/encrpty_packages", strVersionBasePkgDir, null, null);
                    EditorKits.CopyFile(updatepkgresInfo, updatepkgresVerisonInfo);
                }
                if (File.Exists(updatepkgresInfo))
                {
                    try
                    {
                        if(!isFirstUpdatePkg)
                        {
                            if(Directory.Exists(outPut + "/encrpty_packages") && Directory.Exists(strVersionBasePkgDir))
                            {
                                if(Directory.GetFiles(strVersionBasePkgDir).Length<=0)
                                {
                                    EditorKits.CopyDir(outPut + "/encrpty_packages", strVersionBasePkgDir, null, null);
                                }
                            }

                            AssetVersionInfo assetData = JsonUtility.FromJson<AssetVersionInfo>(File.ReadAllText(updatepkgresInfo));

                            Package.PakUpdateFiles pakUpdateFile = new Package.PakUpdateFiles();
                            pakUpdateFile.datas = new List<Package.PakUpdateFiles.PakData>();
                            List<string> vPakDirs = new List<string>();
                            vPakDirs.Add(strVersionBasePkgDir);
                            if (!Directory.Exists(strUpdatePkgDir))
                            {
                                Directory.CreateDirectory(strUpdatePkgDir);
                            }
                            else
                                vPakDirs.Add(strUpdatePkgDir);

                            int versionCode = setting.GetPlatform(setting.buildTarget).versionCode;
                            if (File.Exists(strInfoFile))
                            {
                                Package.PakUpdateFiles oldUpdateFiles = JsonUtility.FromJson<Package.PakUpdateFiles>(File.ReadAllText(strInfoFile));
                                versionCode = oldUpdateFiles.versionCode;
                            }
                            versionCode++;

                            PackagePanel.BuildEncrptyPak(setting, null, setting.buildTarget, false, vPakDirs, strUpdatePkgDir, version + "." + versionCode);

                            GameDelegate.DeleteAllPackages();
                            GameDelegate.EnableCatchHandle(false);
                            GameDelegate.EnablePackage(true);
                            string suffixName = setting.GetEncrtpyPackSuffix(setting.buildTarget);
                            string[] encrtpy_packages = Directory.GetFiles(strUpdatePkgDir, "*." + suffixName, SearchOption.AllDirectories);
                            for (int i = 0; i < encrtpy_packages.Length; ++i)
                            {
                                System.IntPtr pEnterHandler = GameDelegate.LoadPackage(encrtpy_packages[i]);
                                if (pEnterHandler == System.IntPtr.Zero)
                                {
                                    EditorUtility.DisplayDialog("提示", encrtpy_packages[i] + "  \r\n无效包", "Ok");
                                    break;
                                }
                                Package.PakUpdateFiles.PakData pakData = new Package.PakUpdateFiles.PakData();
                                pakData.remotePath = "updates/" + Path.GetFileName(encrtpy_packages[i]);
                                pakData.size = GetFileSize(encrtpy_packages[i]);
                                byte[] nameByes = System.IO.File.ReadAllBytes(encrtpy_packages[i]); //System.Text.Encoding.ASCII.GetBytes(System.IO.Path.GetFileName(encrtpy_packages[i]));
                                pakData.md5 = GameDelegate.Md5(nameByes, (int)nameByes.Length);
                                pakUpdateFile.datas.Add(pakData);
                            }
                            GameDelegate.DeleteAllPackages();

                            pakUpdateFile.version = version;
                            pakUpdateFile.versionCode = versionCode;

                            if (File.Exists(strInfoFile)) File.Delete(strInfoFile);
                            StreamWriter updatesw = File.CreateText(strInfoFile);
                            updatesw.Write(UnityEngine.JsonUtility.ToJson(pakUpdateFile, true));
                            updatesw.Close();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        EditorUtility.DisplayDialog("提示", ex.ToString(), "Ok");
                        return;
                    }
                }
            }
            else
            {
                EditorUtility.DisplayProgressBar("当前[" + curVersion + "]->检测[" + version + "]", "", 0);
                string outPut = setting.GetBuildOutputRoot(setting.buildTarget);

                string strInfoFile = outPut + "/" + strResInfoFile;


                string strVersionBasePkgDir = outPut + "/" + version + "/base_pkg";
                if (!Directory.Exists(outPut + "/" + version))
                {
                    Directory.CreateDirectory(outPut + "/" + version);
                }
                string updatepkgresInfo = outPut + "/" + version + "/resinfo.json";

                Dictionary<string, string> fileAbMapping = new Dictionary<string, string>();
                Dictionary<string, string> vCurDatas = new Dictionary<string, string>();
                Dictionary<string, string> vCurRawAssets = new Dictionary<string, string>();
                if (!File.Exists(updatepkgresInfo))
                {
                    Debug.Log("该版本[" + version + "]还未打过热更包");
                    if (!EditorUtility.DisplayDialog("提示", "该版本还未打过包!是否进行母包拷贝？", "是", "否"))
                    {
                        EditorUtility.ClearProgressBar();
                        File.Copy(strInfoFile, updatepkgresInfo);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        string strCode = File.ReadAllText(updatepkgresInfo, System.Text.Encoding.Default);
                        AssetVersionInfo assetData = JsonUtility.FromJson<AssetVersionInfo>(strCode);
                        for (int i = 0; i < assetData.resInfos.Count; ++i)
                        {
                            vCurDatas[assetData.resInfos[i].name] = assetData.resInfos[i].md5;
                        }
                        for (int i = 0; i < assetData.rawAssets.Count; ++i)
                        {
                            vCurRawAssets[assetData.rawAssets[i].name] = assetData.rawAssets[i].md5;
                        }

                        if (assetData.abMapping != null && assetData.abMapping.mapping != null)
                        {
                            for (int i = 0; i < assetData.abMapping.mapping.Length; ++i)
                            {
                                fileAbMapping[assetData.abMapping.mapping[i].file] = assetData.abMapping.mapping[i].abName;
                            }
                        }

                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.StackTrace);
                        EditorUtility.DisplayDialog("提示", "检测更新包失败!", "确定");
                        EditorUtility.ClearProgressBar();
                        return;
                    }
                }
                EditorUtility.DisplayProgressBar("当前[" + curVersion + "]->检测[" + version + "]", "", 0.2f);
                if (System.IO.File.Exists(strInfoFile))
                {
                    List<Package.UpdateData> vUpdateFiles = new List<Package.UpdateData>();
                    string[] dir = Directory.GetDirectories(outPut + "/" + version);
                    int update_id = 0;
                    for (int i = 0; i < dir.Length; ++i)
                    {
                        string dirName = Path.GetFileName(dir[i]).ToLower();
                        dirName = dirName.Replace("update_", "");
                        int curId = 0;
                        if (int.TryParse(dirName, out curId))
                        {
                            update_id = Mathf.Max(update_id, curId);
                            try
                            {
                                string strCode = File.ReadAllText(dir[i] + "/resource.json");
                                Package.UpdateData updata = JsonUtility.FromJson<Package.UpdateData>(strCode);
                                vUpdateFiles.Add(updata);
                            }
                            catch (System.Exception ex)
                            {
                                Debug.LogError(ex.StackTrace);
                                EditorUtility.DisplayDialog("提示", "检测更新包失败!", "确定");
                                EditorUtility.ClearProgressBar();
                                return;
                            }
                        }
                    }
                    EditorUtility.DisplayProgressBar("当前[" + curVersion + "]->检测[" + version + "]", "", 0.6f);
                    update_id++;
                    try
                    {
                        if (vCurDatas.Count <= 0)
                        {
                            EditorKits.DeleteDirectory(strVersionBasePkgDir);
                            HashSet<string> ignoreSet = new HashSet<string>();
                            EditorKits.CopyDir(outPut + "/base_pkg", strVersionBasePkgDir, null, ignoreSet);
                            File.Copy(strInfoFile, outPut + "/" + version + "/resInfo.json");
                        }
                        else
                        {
                            List<AbMapping.FileAbName> veUpMappingData = new List<AbMapping.FileAbName>();
                            Dictionary<string, AssetVersionInfo.AssetData> veUpData = new Dictionary<string, AssetVersionInfo.AssetData>();
                            Dictionary<string, AssetVersionInfo.RawAsset> veUpRawAsset = new Dictionary<string, AssetVersionInfo.RawAsset>();
                            try
                            {
                                if (curVersionInfo == null)
                                {
                                    string strCode = File.ReadAllText(strInfoFile, System.Text.Encoding.Default);
                                    curVersionInfo = JsonUtility.FromJson<AssetVersionInfo>(strCode);

                                }
                                if (curVersionInfo != null)
                                {
                                    for (int i = 0; i < curVersionInfo.resInfos.Count; ++i)
                                    {
                                        string name = curVersionInfo.resInfos[i].name;
                                        string md5 = curVersionInfo.resInfos[i].md5;
                                        if (!vCurDatas.ContainsKey(name) || vCurDatas[name].CompareTo(md5) != 0)
                                        {
                                            veUpData[name] = curVersionInfo.resInfos[i];
                                        }
                                    }

                                    for (int i = 0; i < curVersionInfo.rawAssets.Count; ++i)
                                    {
                                        string name = curVersionInfo.rawAssets[i].name;
                                        string md5 = curVersionInfo.rawAssets[i].md5;
                                        if (!vCurRawAssets.ContainsKey(name) || vCurRawAssets[name].CompareTo(md5) != 0)
                                        {
                                            veUpRawAsset[name] = curVersionInfo.rawAssets[i];
                                        }
                                    }

                                    if (curVersionInfo.abMapping != null && curVersionInfo.abMapping.mapping != null)
                                    {
                                        for (int i = 0; i < curVersionInfo.abMapping.mapping.Length; ++i)
                                        {
                                            string name = curVersionInfo.abMapping.mapping[i].file;
                                            string ab = curVersionInfo.abMapping.mapping[i].abName;
                                            if (!fileAbMapping.ContainsKey(name) || fileAbMapping[name].CompareTo(ab) != 0)
                                            {
                                                veUpMappingData.Add(new AbMapping.FileAbName() { abName = ab, file = name });
                                            }
                                        }
                                    }


                                    if (veUpData.Count > 0 || veUpMappingData.Count > 0 || veUpRawAsset.Count > 0)
                                    {
                                        // copy update ab
                                        string updateDir = outPut + "/" + version + "/update_" + update_id;
                                        if (Directory.Exists(updateDir))
                                        {
                                            EditorKits.DeleteDirectory(updateDir);
                                        }
                                        else
                                            Directory.CreateDirectory(updateDir);

                                        //        UploadAssets upload = new UploadAssets("192.168.100.210", "21", "root", "12345678sd");
                                        //      upload.Connect();
                                        bool bUplodServer = bUploadVersionServer && !string.IsNullOrEmpty(setting.versionServerAddress);
                                        foreach (var db in veUpData)
                                        {
                                            string strSrc = outPut + "/base_pkg/" + db.Key;
                                            string strDst = updateDir + "/" + db.Key;
                                            string strDstDir = Path.GetDirectoryName(strDst);
                                            if (!Directory.Exists(strDstDir)) Directory.CreateDirectory(strDstDir);
                                            EditorKits.CopyFile(strSrc, strDst);
                                            if (bUplodServer)
                                            {

                                            }
                                        }

                                        foreach (var db in veUpRawAsset)
                                        {
                                            string strSrc = outPut + "/base_pkg/raws/" + db.Key;
                                            string strDst = updateDir + "/" + db.Key;
                                            string strDstDir = Path.GetDirectoryName(strDst);
                                            if (!Directory.Exists(strDstDir)) Directory.CreateDirectory(strDstDir);
                                            EditorKits.CopyFile(strSrc, strDst);
                                            if (bUplodServer)
                                            {

                                            }
                                        }

                                        // build update info
                                        Package.UpdateData updateData = new Package.UpdateData();
                                        string updateFile = updateDir + "/resource.json";

                                        updateData.version = version;
                                        updateData.rootDir = "update_" + update_id;
                                        if (veUpData.Count > 0 || veUpRawAsset.Count > 0)
                                        {
                                            updateData.datas = new Package.UpdateItem[veUpData.Count];
                                            int i = 0;
                                            long totalSize = 0;
                                            foreach (var db in veUpData)
                                            {
                                                Package.UpdateItem item = new Package.UpdateItem();
                                                item.abName = db.Key;
                                                item.md5 = db.Value.md5;
                                                item.size = db.Value.size;
                                                item.depends = db.Value.depends;
                                                updateData.datas[i++] = item;
                                                totalSize += db.Value.size;

                                                for (int j = 0; j < vUpdateFiles.Count;)
                                                {
                                                    Package.UpdateData upDta = vUpdateFiles[j];
                                                    if (upDta.IsExist(item.abName))
                                                    {
                                                        upDta.RemoveItem(item.abName);
                                                        if ((upDta.rawDatas != null && upDta.rawDatas.Length > 0) || (upDta.datas != null && upDta.datas.Length > 0))
                                                        {
                                                            vUpdateFiles[j] = upDta;
                                                            ++j;
                                                        }
                                                        else
                                                        {
                                                            vUpdateFiles.RemoveAt(j);
                                                        }
                                                    }
                                                    else
                                                        ++j;
                                                }
                                            }

                                            updateData.rawDatas = new Package.UpdateItem[veUpRawAsset.Count];
                                            i = 0;
                                            foreach (var db in veUpRawAsset)
                                            {
                                                Package.UpdateItem item = new Package.UpdateItem();
                                                item.abName = db.Key;
                                                item.md5 = db.Value.md5;
                                                item.size = db.Value.size;
                                                updateData.rawDatas[i++] = item;
                                                totalSize += db.Value.size;

                                                for (int j = 0; j < vUpdateFiles.Count;)
                                                {
                                                    Package.UpdateData upDta = vUpdateFiles[j];
                                                    if (upDta.IsRawExist(item.abName))
                                                    {
                                                        upDta.RemoveRawItem(item.abName);
                                                        if ((upDta.rawDatas != null && upDta.rawDatas.Length > 0) || (upDta.datas != null && upDta.datas.Length > 0))
                                                        {
                                                            vUpdateFiles[j] = upDta;
                                                            ++j;
                                                        }
                                                        else
                                                        {
                                                            vUpdateFiles.RemoveAt(j);
                                                        }
                                                    }
                                                    else
                                                        ++j;
                                                }
                                            }
                                            updateData.totalSize = totalSize;
                                        }
                                        else
                                        {
                                            updateData.totalSize = 0;
                                        }

                                        string strContext = UnityEngine.JsonUtility.ToJson(updateData, true);

                                        if (File.Exists(updateFile))
                                            File.Delete(updateFile);

                                        StreamWriter sw = File.CreateText(updateFile);
                                        sw.Write(strContext);
                                        sw.Close();

                                        vUpdateFiles.Add(updateData);

                                        Package.UpdateFiles updateFiles = new Package.UpdateFiles();
                                        updateFiles.datas = vUpdateFiles;
                                        updateFiles.version = curVersion;

                                        if (veUpMappingData.Count > 0)
                                        {
                                            updateFiles.mapping = new AbMapping();
                                            updateFiles.mapping.mapping = veUpMappingData.ToArray();
                                        }
                                        updateFiles.Sort();

                                        string strTotalUpdateFile = outPut + "/" + version + "/updates.json";
                                        if (File.Exists(strTotalUpdateFile)) File.Delete(strTotalUpdateFile);
                                        StreamWriter updatesw = File.CreateText(strTotalUpdateFile);
                                        updatesw.Write(UnityEngine.JsonUtility.ToJson(updateFiles, true));
                                        updatesw.Close();
                                    }

                                    File.Copy(strInfoFile, outPut + "/" + version + "/resInfo.json", true);
                                }
                            }
                            catch (System.Exception ex)
                            {
                                Debug.LogError(ex.StackTrace);
                                EditorUtility.DisplayDialog("提示", "检测更新包失败!", "确定");
                            }
                        }
                        EditorUtility.DisplayProgressBar("当前[" + curVersion + "]->检测[" + version + "]", "", 1f);
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
                EditorUtility.ClearProgressBar();
            }
        }
        //------------------------------------------------------
        static AbMapping BuildAssetMappingAB(PublishSetting setting)
        {
            string mapingFile = setting.GetBuildTargetOutputDir() + "/" + "mapping.txt";
            if (File.Exists(mapingFile)) File.Delete(mapingFile);

            AbMapping mapping = new AbMapping();
            List<AbMapping.FileAbName> data = new List<AbMapping.FileAbName>();
            foreach (var db in m_vMarkAbs)
            {
                data.Add(new AbMapping.FileAbName() { file = db.Key, abName = db.Value });
            }
            mapping.mapping = data.ToArray();

            string strContext = JsonUtility.ToJson(mapping);

            StreamWriter sw;
            if (!File.Exists(mapingFile)) sw = File.CreateText(mapingFile);
            else sw = new StreamWriter(File.Open(mapingFile, FileMode.OpenOrCreate, FileAccess.Write));

            sw.BaseStream.Position = 0;
            sw.BaseStream.SetLength(0);
            sw.BaseStream.Flush();
            sw.Write(strContext);
            sw.Close();

            return mapping;
        }
        //------------------------------------------------------
        static void BuildVersionFile(PublishSetting setting, string strFile, string strVersion)
        {
            string outPut = setting.GetBuildTargetOutputDir();

            VersionData.Config config = new VersionData.Config();
            config.sceneStreamAB = setting.sceneStreamAb;
            config.version = strVersion;
            config.packCount = 0;
            config.defaultLanguage = (short)setting.GetPlatform(setting.buildTarget).defaultLanguage;
            config.offline = setting.offline;
            config.assetbundleEncryptKey = setting.strEncryptKey;
            config.popPrivacy = setting.popPrivacy;
            config.channel = setting.channelLabel;
            List<VersionData.Server> svrs = new List<VersionData.Server>();

            for (int i = 0; i < setting.serverUrlList.Count; ++i)
            {
                if (setting.serverUrlList[i].bUsed)
                {
                    VersionData.Server svr = new VersionData.Server();
                    svr.name = setting.serverUrlList[i].name;
                    svr.url = setting.serverUrlList[i].sreverUrl;
                    svrs.Add(svr);
                }
            }
            if (svrs.Count <= 0 && setting.serverUrlList.Count > 0)
            {
                VersionData.Server svr = new VersionData.Server();
                svr.name = setting.serverUrlList[0].name;
                svr.url = setting.serverUrlList[0].sreverUrl;
                svrs.Add(svr);
            }
            config.serverUrls = svrs.ToArray();
            if (svrs.Count <= 0)
            {
                EditorUtility.DisplayDialog("提示", "没有服务器配置数据", "请配置");
                Debug.LogError("没有服务器配置数据");
            }

            string strContext = JsonUtility.ToJson(config, true);

            strFile = outPut + "/" + strFile;
            StreamWriter sw;
            if (!File.Exists(strFile))
                sw = File.CreateText(strFile);
            else
            {
                sw = new StreamWriter(File.Open(strFile, FileMode.OpenOrCreate, FileAccess.Write));
            }
            sw.BaseStream.Position = 0;
            sw.BaseStream.SetLength(0);
            sw.BaseStream.Flush();
            sw.Write(strContext);
            sw.Close();
        }
        //------------------------------------------------------
        static AssetVersionInfo BuildAssetBundleMD5(PublishSetting setting, AssetBundleManifest mainfest, string strResInfoFile, AbMapping abMapping = null)
        {
            if(!string.IsNullOrEmpty(AutoBuild.GetCurrentPlublishTag()))
            {
                setting.settingTag = AutoBuild.GetCurrentPlublishTag();
            }
            string outPut = setting.GetBuildOutputRoot(setting.buildTarget);
            if (!Directory.Exists(outPut))
                Directory.CreateDirectory(outPut);

            if (abMapping == null)
            {
                string mapingFile = setting.GetBuildTargetOutputDir() + "/" + "mapping.txt";
                if (File.Exists(mapingFile))
                {
                    try
                    {
                        abMapping = JsonUtility.FromJson<AbMapping>(File.ReadAllText(mapingFile));
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
            }

            string strInfoFile = outPut + "/" + strResInfoFile;

            PublishSetting.Platform platform = setting.GetPlatform(setting.buildTarget);

            if (mainfest == null)
            {
                AssetBundle assetData = AssetBundle.LoadFromFile(outPut + "/base_pkg/base_pkg");
                if (assetData != null)
                    mainfest = assetData.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            if (mainfest == null)
                return null;
            AssetVersionInfo verData = new AssetVersionInfo();
            var names = AssetDatabase.GetAllAssetBundleNames();
            foreach (var name in names)
            {
                string path = outPut + "/base_pkg/" + name;
                string md5 = mainfest.GetAssetBundleHash(name).ToString(); // BuildFileMd5(path);
                if (string.IsNullOrEmpty(md5))
                    continue;

                AssetVersionInfo.AssetData data = new AssetVersionInfo.AssetData();
                data.md5 = md5;
                data.name = name;
                int.TryParse(platform.version, out data.version);
                data.size = GetFileSize(path);
                data.depends = mainfest.GetDirectDependencies(name);
                verData.resInfos.Add(data);
            }
            //! streamdir copy
            if (setting.copyToStreamDirs != null)
            {
                for (int i = 0; i < setting.copyToStreamDirs.Count; ++i)
                {
                    string strToDir = setting.copyToStreamDirs[i].toDir;
                    if (!strToDir.EndsWith("/")) strToDir += "/";
                    string strDir = Application.dataPath;
                    string start = setting.copyToStreamDirs[i].srcDir;
                    if (start.StartsWith("Assets/"))
                    {
                        strDir += start.Substring("Assets".Length);
                    }
                    else
                    {
                        if (start.StartsWith("/")) strDir += start;
                        else strDir += "/" + start;
                    }
                    if (File.Exists(strDir))
                    {
                        AssetVersionInfo.RawAsset data = new AssetVersionInfo.RawAsset();
                        data.md5 = BuildFileMd5(strDir);

                        string strFileName = Path.GetFileName(strDir);
                        string strToFile = strToDir + strFileName;
                        data.name = strToDir + strFileName;

                        int.TryParse(platform.version, out data.version);
                        data.size = GetFileSize(strDir);

                        verData.rawAssets.Add(data);
                    }
                    else if (Directory.Exists(strDir))
                    {
                        List<FileInfo> vFiles = new List<FileInfo>();
                        EditorKits.FindDirFiles(strDir, vFiles);
                        for (int j = 0; j < vFiles.Count; ++j)
                        {
                            if (vFiles[j].FullName.EndsWith(".meta")) continue;
                            string strFileName = Path.GetFileName(vFiles[j].FullName);

                            AssetVersionInfo.RawAsset data = new AssetVersionInfo.RawAsset();
                            data.md5 = BuildFileMd5(vFiles[j].FullName);
                            string strToFile = strToDir + strFileName;
                            data.name = strToDir + strFileName;

                            int.TryParse(platform.version, out data.version);
                            data.size = vFiles[j].Length;
                            verData.rawAssets.Add(data);
                        }
                    }
                }
            }
            verData.abMapping = abMapping;
            string strContext = UnityEngine.JsonUtility.ToJson(verData, true);

            StreamWriter sw;
            if (!File.Exists(strInfoFile))
                sw = File.CreateText(strInfoFile);
            else
            {
                string backupDir = outPut + "/backup";
                if (!Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }
                string strBackupReseInfo = backupDir + "/resinfo_" + string.Format("[{0}{1}{2}{3}{4}{5}]", System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second) + ".json";
                string strOldData = File.ReadAllText(strInfoFile);
                if (!string.IsNullOrEmpty(strOldData))
                {
                    StreamWriter old_sw = new StreamWriter(File.Open(strBackupReseInfo, FileMode.OpenOrCreate, FileAccess.ReadWrite));
                    old_sw.Write(strContext);
                    old_sw.Close();
                }

                if (File.Exists(strInfoFile))
                    File.Delete(strInfoFile);

                sw = File.CreateText(strInfoFile);
            }
            sw.Write(strContext);
            sw.Close();

            return verData;
        }
        //------------------------------------------------------
        public static long GetFileSize(string fileName)
        {
            long size = 0;
            if (!File.Exists(fileName))
            {
                return size;
            }
            else
            {
                var info = new FileInfo(fileName);
                return info.Length;
            }
        }
        //------------------------------------------------------
        public static string BuildFileMd5(string filename)
        {
            string filemd5 = null;
            try
            {
                var fileStream = File.OpenRead(filename);
                var md5 = System.Security.Cryptography.MD5.Create();
                var fileMD5Bytes = md5.ComputeHash(fileStream);
                fileStream.Close();
                filemd5 = System.BitConverter.ToString(fileMD5Bytes).Replace("-", "").ToLower();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }
            return filemd5;
        }
    }
}
