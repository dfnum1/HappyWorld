/********************************************************************
生成日期:	25:7:2019   9:51
类    名: 	PublishPostProcess
作    者:	HappLI
描    述:	发布回调
*********************************************************************/
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Reflection;
using System.IO;
using System;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Il2Cpp;
using System.Collections.Generic;
#if UNITY_EDITOR_OSX
using UnityEditor.iOS.Xcode;
#endif
namespace TopGame.ED
{
    public class BuildProcessor_2020_1_OR_NEWER : IPreprocessBuildWithReport
#if UNITY_ANDROID
        , IPostGenerateGradleAndroidProject
#endif
    {
        public int callbackOrder => 0;
        public void OnPreprocessBuild(BuildReport report)
        {

        }
#if UNITY_ANDROID
        public void OnPostGenerateGradleAndroidProject(string path)
        {
            OnBuildPublish(path);
        }
#endif
        public void OnPostprocessBuild(BuildReport report)
        {
#if !UNITY_ANDROID
            OnBuildPublish(report.summary.outputPath);
#endif
        }

        public static void OnBuildPublish(string path)
        {
            Debug.Log("build:" + path);
            var setting = PublishPanel.LoadSetting(AutoBuild.GetCurrentPlublishTag());
            if(setting !=null)
            {
                if (PlayerSettings.GetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup) == ScriptingImplementation.IL2CPP)
                {
                    // encrypt global meta
                    path = path.Replace('\\', '/');
#if UNITY_ANDROID
                    if (setting.globalMetaDataEncrypt)
                        OnEncryptMetadataProcess(path + "/src/main/assets/bin/Data/Managed/Metadata/global-metadata.dat");
#elif UNITY_IPHONE || UNITY_IOS
                    if(setting.globalMetaDataEncrypt) OnEncryptMetadataProcess(Path.Combine(path, "Data", "Managed", "Metadata", "global-metadata.dat"));
#if USE_HYBRIDCLR
                        //! copy libil2cpp.a
                        string strFile = HybridCLRHelper.IOSIl2CPPLibFile;
                        if (File.Exists(strFile))
                        {
                            string toFile = Path.Combine(path, "Libraries", "libil2cpp.a");
                            if (File.Exists(toFile))
                            {
                                File.Copy(strFile, toFile, true);
                                Debug.Log("COPY libil2cpp.a");
                            }
                        }
#endif
#elif !UNITY_ANDROID && !UNITY_IPHONE
                    if(setting.globalMetaDataEncrypt)
                    {
                        var file = Directory.GetFiles(path, "global-metadata.dat", SearchOption.AllDirectories);
                        for (int i = 0; i < file.Length; ++i)
                        {
                            OnEncryptMetadataProcess(file[i]);
                        }
                    }
#endif
                }
            }
        }
        private static void OnEncryptMetadataProcess(string rootPath = null)
        {
            var srcPath = rootPath;
            if (File.Exists(srcPath))
            {
                Debug.Log(srcPath + "\r\nglobal-meta.data 加密");
                EditorUtility.DisplayProgressBar("global-meta.data 加密", srcPath, 0);
                byte[] bytes = File.ReadAllBytes(srcPath);
                if (CoreDllUtil.CorePlus_EncryptBuffer(ref bytes[0], bytes.Length, null, 1))
                {
                    byte[] newBytes = new byte[bytes.Length + 4];
                    Buffer.BlockCopy(bytes, 0, newBytes, 4, bytes.Length);
                    newBytes[0] = BitConverter.GetBytes('e')[0];
                    newBytes[1] = BitConverter.GetBytes('n')[0];
                    newBytes[2] = BitConverter.GetBytes('c')[0];
                    newBytes[3] = BitConverter.GetBytes('r')[0];
                    File.WriteAllBytes(srcPath, newBytes);
                }
                EditorUtility.ClearProgressBar();
            }
            else
                Debug.LogWarning(rootPath + " no found!");
        }

        class CoreDllUtil
        {

#if !UNITY_EDITOR && UNITY_IPHONE
        const string CoreDLL = "__Internal";
#else
            const string CoreDLL = "CorePlus";
#endif

            [System.Runtime.InteropServices.DllImport(CoreDLL)]
            public static extern bool CorePlus_EncryptBuffer(ref byte pInBuffer, int nBuffSize, int[] arrKeys, int cipherRemains);
        }
    }

    public class PublishPostProcess
    {
        [PostProcessBuild(700)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            Debug.Log("build:" + pathToBuiltProject);
            if (target == BuildTarget.Android)
            {
                ///Users/stupiddog/Desktop/work/JSJProject_Android/Client/Assets/../Publishs/Android/JSJ[1.0.0][2020_12_31_17_17_1].apk

#if UNITY_EDITOR_OSX
                //生成通知文件
                string apkInfoPath = "/Users/stupiddog/.jenkins/workspace/Project_JSJ_Android";
                string apkInfoFile = apkInfoPath + "/info.txt";
                string workspaceAPKPath = "/Users/stupiddog/.jenkins/workspace/Project_JSJ_Android";
                string apkName = workspaceAPKPath + "/JSJ_Android.apk";

                if (!Directory.Exists(apkInfoPath))
                {
                    Directory.CreateDirectory(apkInfoPath);
                }
                if (File.Exists(apkInfoFile))
                {
                    File.Delete(apkInfoFile);
                }
                long size = 0;
                if (File.Exists(apkName))
                {
                    FileInfo fileInfo = new FileInfo(apkName);
                    if (fileInfo != null)
                    {
                        size = fileInfo.Length;
                    }
                }
                //编辑apk信息,用于后面通知到企业微信使用
                var write = File.CreateText(apkInfoFile);
                write.WriteLine(apkName + " : " + Base.Util.FormBytes(size));
                write.Close();

                //得到apk路径,移动apk到workspace

                Debug.Log("APK_path:" + pathToBuiltProject + ",move_apk_path:" + apkName + ",apk_info_path:" + apkInfoFile);
                if (!Directory.Exists(workspaceAPKPath))
                {
                    Directory.CreateDirectory(workspaceAPKPath);
                }
                //File.Move(pathToBuiltProject, apkName);//路径中带 .. 符号,不能正确移动文件,走shell进行移动apk
#endif

                return;
            }
            if (target == BuildTarget.iOS)
            {
                Debug.Log("Begin PostProcessBuild path:" + pathToBuiltProject);
#if UNITY_EDITOR_OSX
               string provisioning_profiler_appkey = "94afe52f-4af0-4b3f-9135-49f689e7a2dd";
                string provisioning_profiler_name = "stupiddog_run_dev";
                string development_teamKey = "8J9G7947T9";
                string bundid = "com.stupiddog.codesd.running";
                string productName = "HappyLand";
                string publishTag = AutoBuild.GetCurrentPlublishTag();
                Debug.Log("Begin PostProcessBuild Tag:" + publishTag);
                var setting = PublishPanel.LoadSetting(publishTag);
                PublishPanel.PublishSetting.Platform plushPlaform = null;
                if (setting != null)
                {
                    plushPlaform = setting.GetPlatform(target);
                    if (plushPlaform != null)
                    {
                        provisioning_profiler_appkey = plushPlaform.provisioning_profiler_appkey;
                        provisioning_profiler_name = plushPlaform.provisioning_profiler_name;
                        development_teamKey = plushPlaform.development_teamKey;
                        bundid = plushPlaform.bundleName;
                        productName = plushPlaform.projectExportName;
                    }
                }
                string exportOps = Application.dataPath + "/../Publishs/IOS/ExportOptions.plist";
                if (File.Exists(exportOps))
                {
                    string exportOpContext = File.ReadAllText(exportOps, System.Text.Encoding.UTF8);
                    exportOpContext = exportOpContext.Replace("com.stupiddog.codesd.running", bundid);
                    exportOpContext = exportOpContext.Replace("stupiddog_run_dev", provisioning_profiler_name);
                    exportOpContext = exportOpContext.Replace("8J9G7947T9", development_teamKey);
                    if (!Directory.Exists(Application.dataPath + "/../Publishs/Temps"))
                        Directory.CreateDirectory(Application.dataPath + "/../Publishs/Temps");
                    File.WriteAllText(Application.dataPath + "/../Publishs/Temps/ExportOptions.plist", exportOpContext, System.Text.Encoding.UTF8);
                }
                var projPath =pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
                var proj = new PBXProject();
                proj.ReadFromFile(projPath);
            
                proj.SetBuildProperty(proj.ProjectGuid(), "ENABLE_BITCODE", "NO");
            //    proj.SetBuildProperty(proj.ProjectGuid(), "ENABLE_OBJC_EXCEPTIONS", "YES");
                proj.SetBuildProperty(proj.ProjectGuid(), "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
             
                string targetUnityIphone = proj.GetUnityFrameworkTargetGuid();


                Debug.Log("proj.ProjectGuid():" + proj.ProjectGuid());
                Debug.Log("proj.GetUnityMainTargetGuid():" + proj.GetUnityMainTargetGuid());

                

                var targetGUID = proj.GetUnityMainTargetGuid();
                proj.SetBuildProperty(targetGUID, "PRODUCT_NAME_APP", productName);
                proj.SetBuildProperty(targetGUID, "ENABLE_BITCODE", "NO");
               // proj.SetBuildProperty(targetGUID, "ENABLE_OBJC_EXCEPTIONS", "YES");
                proj.SetBuildProperty(targetGUID, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
                proj.SetBuildProperty(targetGUID, "ITSAppUsesNonExemptEncryption", "NO");

                //设置证书
                proj.SetBuildProperty(targetGUID, "PROVISIONING_PROFILE_APP", provisioning_profiler_appkey);//<key>UUID</key>
                proj.SetBuildProperty(targetGUID, "PROVISIONING_PROFILE_SPECIFIER",provisioning_profiler_name);//<key>Name</key>
                proj.SetBuildProperty(targetGUID, "DEVELOPMENT_TEAM", development_teamKey);//<key>TeamIdentifier</key>
    //#if USE_BUGLY
               // proj.AddFrameworkToProject(targetGUID, Utf8string("libz.tbd"), true); 
                proj.AddBuildProperty(targetUnityIphone, "OTHER_LDFLAGS", "-lz");
    //#endif
                //proj.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ObjC");
                //proj.AddFrameworkToProject(targetGUID, "libresolv.9.tbd", false);
                //proj.AddFrameworkToProject(targetGUID, "WebKit.framework", false);

                if(plushPlaform!=null)
                {
                    if(plushPlaform.IsUsedSDK("USE_QUICKSDK"))ConfigSDKIOS(target, "USE_QUICKSDK", pathToBuiltProject,plushPlaform,proj);
                    if(plushPlaform.IsUsedSDK("USE_TALKINGDATA"))ConfigSDKIOS(target, "USE_TALKINGDATA", pathToBuiltProject,plushPlaform,proj);
                    if(plushPlaform.IsUsedSDK("USE_APPLE"))ConfigSDKIOS(target, "USE_APPLE", pathToBuiltProject,plushPlaform,proj);
                    if(plushPlaform.IsUsedSDK("USE_TOBID"))ConfigSDKIOS(target, "USE_TOBID", pathToBuiltProject,plushPlaform,proj);
                    if(plushPlaform.IsUsedSDK("USE_LZT"))ConfigSDKIOS(target, "USE_LZT", pathToBuiltProject,plushPlaform,proj); 
                }

                BuildProcessor_2020_1_OR_NEWER.OnBuildPublish(pathToBuiltProject);
                if(plushPlaform!=null)
                {
                    if(plushPlaform.IsUsedSDK("USE_QUICKSDK")) CoinfigSDKAfter(target, "USE_QUICKSDK",pathToBuiltProject,plushPlaform);
                    if(plushPlaform.IsUsedSDK("USE_APPLE")) CoinfigSDKAfter(target, "USE_APPLE",pathToBuiltProject,plushPlaform);
                }
#if USE_HYBRIDCLR
            //    HybridCLRHelper.BuildIOSIl2Cpp(targetGUID, proj);
                        //! copy libil2cpp.a
                string strFile = HybridCLRHelper.IOSIl2CPPLibFile;
                Debug.Log("COPY libil2cpp.a");
                if (File.Exists(strFile))
                {
                    string toFile = Path.Combine(pathToBuiltProject, "Libraries", "libil2cpp.a");
                    if (File.Exists(toFile))
                    {
                        File.Copy(strFile, toFile, true);
                        Debug.Log("COPY libil2cpp.a");
                    }
                    else 
                        Debug.Log(toFile + " no found");
                }
                else
                    Debug.Log(strFile + " no found");
#endif
                proj.WriteToFile(projPath);
#endif
            }
        }

        static string Utf8string(string s)
        {
               System.Text.UTF8Encoding.UTF8.GetString(System.Text.UTF8Encoding.UTF8.GetBytes(s));
            return s;
        }
#if UNITY_EDITOR_OSX
        public static void CoinfigSDKAfter(BuildTarget target, string sdk, string pathToBuiltProject, PublishPanel.PublishSetting.Platform publishPlatoform = null)
        {
            bool bDirtyFile = false;
            string projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            UnityEditor.iOS.Xcode.ProjectCapabilityManager projectCapabilityManager = new UnityEditor.iOS.Xcode.ProjectCapabilityManager(projectPath, "XXX.entitlements", "Unity-iPhone");
#if USE_QUICKSDK
            if (sdk.CompareTo("USE_QUICKSDK") == 0)
            {
                bDirtyFile = true;
                projectCapabilityManager.AddSignInWithApple();  //app登录
                projectCapabilityManager.AddInAppPurchase();    //内购
             //   projectCapabilityManager.AddGameCenter();       //gamecenter
             //   projectCapabilityManager.AddPushNotifications(true);    //推送
                
            }
#endif
#if USE_APPLE   
            if (sdk.CompareTo("USE_APPLE") == 0)
            {
                bDirtyFile = true;
                if(publishPlatoform!=null)
                {
                    string useAuth = publishPlatoform.GetKeyValue(sdk, "useAuth");
                    if(useAuth!=null && useAuth != "0") projectCapabilityManager.AddSignInWithApple();  //app登录
                }
                projectCapabilityManager.AddInAppPurchase();    //内购
                projectCapabilityManager.AddGameCenter();       //gamecenter
             //   projectCapabilityManager.AddPushNotifications(true);    //推送
                
            }
#endif
            if(bDirtyFile) projectCapabilityManager.WriteToFile();
        }
        public static void ConfigSDKIOS(BuildTarget target, string sdk, string pathToBuiltProject, PublishPanel.PublishSetting.Platform publishPlatoform = null, UnityEditor.iOS.Xcode.PBXProject proj = null)
        {
            pathToBuiltProject = System.IO.Path.GetFullPath(pathToBuiltProject).Replace("\\", "/");
            string plistFile = pathToBuiltProject + "/Info.plist";
            if (!File.Exists(plistFile))
            {
                return;
            }
            string targetGUID = "";
            string targetUnityFrameGUID = "";
            if(proj!=null)
            {
                targetGUID= proj.GetUnityMainTargetGuid();
                targetUnityFrameGUID= proj.GetUnityFrameworkTargetGuid();
            }

            if(publishPlatoform == null)
            {
                var setting = PublishPanel.LoadSetting(AutoBuild.GetCurrentPlublishTag());
                if (setting != null)
                {
                    publishPlatoform = setting.GetPlatform(target);
                }
            }
            if(publishPlatoform == null) return;

            UnityEditor.iOS.Xcode.PlistDocument dd = new UnityEditor.iOS.Xcode.PlistDocument();
            dd.ReadFromFile(plistFile);
            bool bDirtyInfoList = false;
#if USE_LZT
            if (sdk.CompareTo("USE_LZT") == 0)
            {
                dd.root.SetString("UIUserInterfaceStyle", "Light");
                dd.root.SetString("Appearance", "Light");
                dd.root.SetString("NSPhotoLibraryUsageDescription", "保存用户名密码需要使用到您的相册");
                string pluginDir = pathToBuiltProject + "/Frameworks/Plugins/iOS/";
                if(proj!=null && (File.Exists(pluginDir+ "LZTRES.bundle") || System.IO.Directory.Exists(pluginDir+ "LZTRES.bundle")))
                {
                    Debug.Log("Copy LZTRES.bundle");
                    proj.AddFileToBuild(targetGUID, proj.AddFile(pluginDir + "LZTRES.bundle", "LZTRES.bundle", UnityEditor.iOS.Xcode.PBXSourceTree.Source));
                    proj.AddFileToBuild(targetUnityFrameGUID, proj.AddFile(pluginDir + "LZTRES.bundle", "LZTRES.bundle", UnityEditor.iOS.Xcode.PBXSourceTree.Source));
                }
                else 
                    Debug.Log("Copy " + pluginDir+ "LZTRES.bundle" + "  not found");

                bDirtyInfoList = true;
            }  
#endif

#if USE_QUICKSDK
            if(sdk.CompareTo("USE_QUICKSDK") == 0)
            {
                bDirtyInfoList = true;
                dd.root.SetString("NSCameraUsageDescription", publishPlatoform.GetKeyValue(sdk,"NSCameraUsageDescription","上传头像等场景需要使用到您的相机"));
                dd.root.SetString("NSPhotoLibraryUsageDescription", publishPlatoform.GetKeyValue(sdk,"NSCameraUsageDescription","上传头像等场景需要使用到您的相机"));
                dd.root.SetString("NSUserTrackingUsageDescription", publishPlatoform.GetKeyValue(sdk,"NSUserTrackingUsageDescription","自定义获取idfa权限说明文案应使用与应用本地化语言一致的语言"));

                var urlTypes = dd.root.CreateArray("CFBundleURLTypes");

                string pluginDir = pathToBuiltProject + "/Frameworks/Plugins/iOS/";
                if(proj!=null && File.Exists(pluginDir+ "SSBundle.bundle"))
                {
                    proj.AddFileToBuild(targetGUID, proj.AddFile(pluginDir + "SSBundle.bundle", "SSBundle.bundle", UnityEditor.iOS.Xcode.PBXSourceTree.Source));
                }

//                 proj.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ObjC");
//                 proj.AddFrameworkToProject(targetGUID, "libc++.tbd", false);
//                 proj.AddFrameworkToProject(targetGUID, "Foundation.framework", false);

                //! appstore sign in
                {
                    if(proj != null)
                    {
                        proj.AddCapability(targetGUID, UnityEditor.iOS.Xcode.PBXCapabilityType.SignInWithApple);
                        proj.AddFrameworkToProject(targetGUID, "AuthenticationServices.framework", false);
                        proj.AddFrameworkToProject(targetGUID, "Accelerate.framework", false);

                        proj.AddFrameworkToProject(targetUnityFrameGUID, "AuthenticationServices.framework", false);
                        proj.AddFrameworkToProject(targetUnityFrameGUID, "Accelerate.framework", false);
                    }
                }

                //! google
                {
                    string googleKey = publishPlatoform.GetKeyValue(sdk, "GOOGLE_SIGNIN_CLIENT_ID");
                    if (!string.IsNullOrEmpty(googleKey))
                    {
                        if(proj!=null)
                        {
//                          proj.AddFrameworkToProject(targetGUID, "AppAuth.framework", false);
//                          proj.AddFrameworkToProject(targetGUID, "GoogleSignIn.framework", false);
//                          proj.AddFrameworkToProject(targetGUID, "GTMAppAuth.framework", false);
//                          proj.AddFrameworkToProject(targetGUID, "GTMSessionFetcher.framework", false);
                            if (proj != null && File.Exists(pluginDir + "GoogleSignIn.bundle"))
                            {
                                proj.AddFileToBuild(targetGUID, proj.AddFile(pluginDir + "GoogleSignIn.bundle", "GoogleSignIn.bundle",UnityEditor.iOS.Xcode.PBXSourceTree.Absolute));
                            }
                        }
                        dd.root.SetString("GoogleClientID", googleKey);
                        var googleSchemes = urlTypes.AddDict();
                        googleSchemes.SetString("CFBundleTypeRole", "Editor");
                        var items = googleSchemes.CreateArray("CFBundleURLSchemes");

                        string schemeskey = "";
                        int dotIndex = 0;
                        for (int i = 0; i < googleKey.Length; ++i)
                        {
                            if (googleKey[i] == '.')
                            {
                                dotIndex = i;
                                break;
                            }
                            schemeskey += googleKey[i];
                        }
                        if (dotIndex > 0 && dotIndex < googleKey.Length)
                        {
                            string keyTrail = googleKey.Substring(dotIndex, googleKey.Length - dotIndex);
                            List<string> keyTail = new List<string>(keyTrail.Split('.'));
                            keyTail.Reverse();
                            for (int i = 0; i < keyTail.Count; ++i)
                                schemeskey = keyTail[i] + "." + schemeskey;
                        }

                        items.AddString(schemeskey);
                    }
                }


                //! facebook
                {
                    string FacebookAppID = publishPlatoform.GetKeyValue(sdk, "FACEBOOK_APP_ID");
                    if(!string.IsNullOrEmpty(FacebookAppID))
                    {
                        // swift ???
                        string othConfigDir = Application.dataPath + "/../Publishs/SDK/QuickSDK/OthConfigs/";
                         if(proj!=null && File.Exists(othConfigDir+ "File.swift")&& File.Exists(othConfigDir+ "Unity-iPhone-Bridging-Header.h"))
                         {
                             File.Copy(othConfigDir+ "File.swift", pathToBuiltProject + "/File.swift");
                             File.Copy(othConfigDir+ "Unity-iPhone-Bridging-Header.h", pathToBuiltProject + "/Unity-iPhone-Bridging-Header.h");
                             if(!proj.ContainsFileByProjectPath("File.swift"))
                             {
                                string swiftFile = proj.AddFile(pathToBuiltProject + "/File.swift", "File.swift", UnityEditor.iOS.Xcode.PBXSourceTree.Source);
                                proj.AddFileToBuild(targetGUID, swiftFile);
                                proj.AddFileToBuildSection(targetGUID, proj.GetSourcesBuildPhaseByTarget(targetGUID), swiftFile);
                                proj.AddFileToBuildSection(targetGUID, proj.GetSourcesBuildPhaseByTarget(targetUnityFrameGUID), swiftFile);
                             }
                             if(!proj.ContainsFileByProjectPath("Unity-iPhone-Bridging-Header.h"))
                             {
                                string bridgingHeader = proj.AddFile(pathToBuiltProject + "/Unity-iPhone-Bridging-Header.h", "Unity-iPhone-Bridging-Header.h", UnityEditor.iOS.Xcode.PBXSourceTree.Source);      
//                              proj.AddFileToBuild(targetGUID, bridgingHeader);
                             }

                             
                             proj.AddBuildProperty(targetGUID, "SWIFT_OBJC_BRIDGING_HEADER", "Unity-iPhone-Bridging-Header.h");
                             proj.AddBuildProperty(targetGUID, "SWIFT_VERSION", "5.0");
                             proj.AddBuildProperty(targetGUID, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
                             proj.AddBuildProperty(targetUnityFrameGUID, "SWIFT_VERSION", "5.0");
                             proj.AddBuildProperty(targetUnityFrameGUID, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

                           //   proj.AddFrameworkToProject(targetGUID, "Combine.framework", true);
                           //   proj.AddFrameworkToProject(targetGUID, "libswiftCore.tbd", true);
                           //   proj.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ObjC");
                         }
//                         if (proj != null)
//                         {
//                             proj.AddFrameworkToProject(targetGUID, "FBSDKCoreKit.framework", false);
//                             proj.AddFrameworkToProject(targetGUID, "FBAEMKit.framework", false);
//                             proj.AddFrameworkToProject(targetGUID, "FBSDKCoreKit_Basics.framework", false);
//                             proj.AddFrameworkToProject(targetGUID, "FBSDKLoginKit.framework", false);
//                         }

                        string FacebookClientToken = publishPlatoform.GetKeyValue(sdk, "FACEBOOK_CLIENT_TOKEN");
                        if (string.IsNullOrEmpty(FacebookClientToken)) FacebookClientToken = "a82ccfbc9856bf38bb41f227e3962d79";

                        string FacebookDisplayName = publishPlatoform.GetKeyValue(sdk, "FACEBOOK_DISPLAY_NAME");
                        if (string.IsNullOrEmpty(FacebookClientToken)) FacebookDisplayName = "CodeSD:Running";

                        dd.root.SetString("FacebookAppID", FacebookAppID);
                        dd.root.SetString("FacebookClientToken", FacebookClientToken);
                        dd.root.SetString("FacebookDisplayName", FacebookDisplayName);

                        //! 白名单
                        var LSApplicationQueriesSchemes = dd.root.CreateArray("LSApplicationQueriesSchemes");
                        LSApplicationQueriesSchemes.AddString("twitter");
                        LSApplicationQueriesSchemes.AddString("twitterauth");
                        LSApplicationQueriesSchemes.AddString("lineauth");
                        LSApplicationQueriesSchemes.AddString("lineauth2");
                        LSApplicationQueriesSchemes.AddString("fbapi");
                        LSApplicationQueriesSchemes.AddString("fb-messenger-api");
                        LSApplicationQueriesSchemes.AddString("fbauth2");
                        LSApplicationQueriesSchemes.AddString("fbshareextension");
                        LSApplicationQueriesSchemes.AddString("fb-messenger-share-api");

                        string fbloginProtocalScheme = publishPlatoform.GetKeyValue(sdk, "FACEBOOK_LOGIN_PROTOCAL_SCHEME");
                        if (string.IsNullOrEmpty(fbloginProtocalScheme)) fbloginProtocalScheme = "fb4304524946296678";

                        var fabcebookSchemes = urlTypes.AddDict();
                        fabcebookSchemes.SetString("CFBundleTypeRole", "Editor");
                        var items = fabcebookSchemes.CreateArray("CFBundleURLSchemes");
                        items.AddString(fbloginProtocalScheme);
                    }
                }

                //! appsflyer
                {
                    string appsFlyerId = publishPlatoform.GetKeyValue(sdk, "APPSFLYER_KEYCODE");
                    if (!string.IsNullOrEmpty(appsFlyerId) && proj != null)
                    {
//                         proj.AddFrameworkToProject(targetGUID, "AppsFlyerLib.framework", false);
                         proj.AddFrameworkToProject(targetGUID, "AdSupport.framework", false);
                         proj.AddFrameworkToProject(targetGUID, "iAd.framework", false);
                    }
                }
            }
#endif
#if USE_TALKINGDATA
            if(sdk.CompareTo("USE_TALKINGDATA") == 0)
            {
                if(!proj.ContainsFramework(targetUnityFrameGUID,"AdServices.framework")) {bDirtyInfoList = true; proj.AddFrameworkToProject(targetUnityFrameGUID, "AdServices.framework", false);}
                if(!proj.ContainsFramework(targetUnityFrameGUID,"iAd.framework")) {bDirtyInfoList = true; proj.AddFrameworkToProject(targetUnityFrameGUID, "iAd.framework", false);}
                if(!proj.ContainsFramework(targetUnityFrameGUID,"StoreKit.framework")){bDirtyInfoList = true; proj.AddFrameworkToProject(targetUnityFrameGUID, "StoreKit.framework", false);}
                if(!proj.ContainsFramework(targetUnityFrameGUID,"AppTrackingTransparency.framework")) {bDirtyInfoList = true;proj.AddFrameworkToProject(targetUnityFrameGUID, "AppTrackingTransparency.framework", false);}
                if(!proj.ContainsFramework(targetUnityFrameGUID,"AdSupport.framework")){bDirtyInfoList = true; proj.AddFrameworkToProject(targetUnityFrameGUID, "AdSupport.framework", false);}
                if(!proj.ContainsFramework(targetUnityFrameGUID,"CoreTelephony.framework")){bDirtyInfoList = true; proj.AddFrameworkToProject(targetUnityFrameGUID, "CoreTelephony.framework", false);}
                if(!proj.ContainsFramework(targetUnityFrameGUID,"Security.framework")){bDirtyInfoList = true; proj.AddFrameworkToProject(targetUnityFrameGUID, "Security.framework", false);}
                if(!proj.ContainsFramework(targetUnityFrameGUID,"SystemConfiguration.framework")){bDirtyInfoList = true; proj.AddFrameworkToProject(targetUnityFrameGUID, "SystemConfiguration.framework", false);}
                if(!proj.ContainsFramework(targetUnityFrameGUID,"libc++.tbd")){bDirtyInfoList = true; proj.AddFrameworkToProject(targetUnityFrameGUID, "libc++.tbd", false);}
                if(!proj.ContainsFramework(targetUnityFrameGUID,"libz.tbd")){bDirtyInfoList = true; proj.AddFrameworkToProject(targetUnityFrameGUID, "libz.tbd", false);}
            }
#endif
#if USE_APPLE
            if(sdk.CompareTo("USE_APPLE") == 0)
            {
                if(proj != null)
                {
                    string useAuth = publishPlatoform.GetKeyValue(sdk, "useAuth");
                    if(useAuth!=null && useAuth != "0")
                    {
                    //    proj.AddCapability(targetUnityFrameGUID, UnityEditor.iOS.Xcode.PBXCapabilityType.SignInWithApple);
                        proj.AddCapability(targetGUID, UnityEditor.iOS.Xcode.PBXCapabilityType.SignInWithApple);
                    }
                  //  proj.AddCapability(targetUnityFrameGUID, UnityEditor.iOS.Xcode.PBXCapabilityType.InAppPurchase);
                    proj.AddCapability(targetGUID, UnityEditor.iOS.Xcode.PBXCapabilityType.InAppPurchase);

                    if(!proj.ContainsFramework(targetUnityFrameGUID,"AuthenticationServices.framework")) proj.AddFrameworkToProject(targetUnityFrameGUID, "AuthenticationServices.framework", false);
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"Accelerate.framework")) proj.AddFrameworkToProject(targetUnityFrameGUID, "Accelerate.framework", false);
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"StoreKit.framework")) proj.AddFrameworkToProject(targetUnityFrameGUID, "StoreKit.framework", false);

                  //  proj.AddFrameworkToProject(targetGUID, "AuthenticationServices.framework", false);
                 //   proj.AddFrameworkToProject(targetGUID, "Accelerate.framework", false);

                    bDirtyInfoList = true;
                }            
            }
#endif
       
#if USE_TOBID
            if(sdk.CompareTo("USE_TOBID") == 0)
            {
                if(proj != null)
                {
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"StoreKit.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "StoreKit.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"CFNetwork.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "CFNetwork.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"CoreMedia.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "CoreMedia.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"AdSupport.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "AdSupport.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"CoreGraphics.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "CoreGraphics.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"AVFoundation.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "AVFoundation.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"CoreLocation.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "CoreLocation.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"CoreTelephony.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "CoreTelephony.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"SafariServices.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "SafariServices.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"MobileCoreServices.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "MobileCoreServices.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"WebKit.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "WebKit.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"SystemConfiguration.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "SystemConfiguration.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"ImageIO.framework")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "ImageIO.framework", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"libz.tbd")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "libz.tbd", false);}
                    if(!proj.ContainsFramework(targetUnityFrameGUID,"libsqlite3.tbd")){bDirtyInfoList = true;  proj.AddFrameworkToProject(targetUnityFrameGUID, "libsqlite3.tbd", false);}
                }            
            }
#endif

            if(bDirtyInfoList)
                File.WriteAllText(plistFile, dd.WriteToString(), System.Text.Encoding.UTF8);
            return;
        }
#endif
    }
}
#endif