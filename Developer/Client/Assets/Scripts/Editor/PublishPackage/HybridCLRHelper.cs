/********************************************************************
生成日期:	25:7:2019   9:51
类    名: 	HybridCLRHelper
作    者:	HappLI
描    述:	
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
using System;

namespace TopGame.ED
{
    public class HybridCLRHelper
    {
        public static bool IsEnable
        {
            get
            {
#if USE_HYBRIDCLR && UNITY_EDITOR
                return HybridCLR.Editor.SettingsUtil.Enable;
#else
                return false;
#endif
            }
            set
            {
#if USE_HYBRIDCLR && UNITY_EDITOR
                HybridCLR.Editor.SettingsUtil.Enable = value;
#endif
            }
        }
#if USE_HYBRIDCLR && UNITY_EDITOR
        public static string LocalIl2CppDir
        {
            get
            {
                return HybridCLR.Editor.SettingsUtil.LocalIl2CppDir;
            }
        }
        public static string HybridCLRDataDir
        {
            get
            {
                return HybridCLR.Editor.SettingsUtil.HybridCLRDataDir;
            }
        }
        public static void ClearTemp()
        {
            string temp = HybridCLR.Editor.SettingsUtil.HybridCLRDataDir + "/StrippedAOTDllsTempProj";
            if (System.IO.Directory.Exists(temp)) System.IO.Directory.Delete(temp, true);
            temp = Application.dataPath + "/../HybridCLRData";
            if (System.IO.Directory.Exists(temp)) System.IO.Directory.Delete(temp, true);
        }
        public static string LocalUnityDataDir
        {
            get
            {
                return HybridCLR.Editor.SettingsUtil.LocalUnityDataDir;
            }
        }
        public static string IOSIl2CPPLibFile
        {
            get
            {
                return HybridCLR.Editor.SettingsUtil.HybridCLRDataDir + "/iOSBuild/build/libil2cpp.a";
            }
        }
        public static void BuildIOSIl2Cpp()
        {
            string file = HybridCLR.Editor.SettingsUtil.HybridCLRDataDir + "/iOSBuild/build.sh";
            if (File.Exists(file))
            {
                Debug.Log("libil2cpp compiler begin");
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "/bin/bash";
                process.StartInfo.Arguments = file;
                process.StartInfo.WorkingDirectory = HybridCLR.Editor.SettingsUtil.HybridCLRDataDir + "/iOSBuild";
                process.Start();
                process.WaitForExit();

                if (File.Exists(IOSIl2CPPLibFile))
                    Debug.Log("libil2cpp compiler ok");
                else Debug.Log("libil2cpp compiler fail");
            }
            else
            {
                Debug.Log("libil2cpp compiler file unfound:" + file);
            }
        }
#if UNITY_IOS
        public static void BuildIOSIl2Cpp(string targetGuid, UnityEditor.iOS.Xcode.PBXProject pbProject)
        {
            string file = HybridCLR.Editor.SettingsUtil.HybridCLRDataDir + "/iOSBuild/build.sh";
            if (File.Exists(file))
            {
                Debug.Log("libil2cpp compilering");
                var sbShellScript = new StringBuilder();
                sbShellScript.AppendLine("echo PreBuildI12CPP Start");
                sbShellScript.AppendLine("export CNAKE_ROOT=/Applications/CMake.app/Contents/bin");
                sbShellScript.AppendLine("export PATH=$CMAKE_ROOT :$PATH");
                sbShellScript.AppendLine("cd "+ HybridCLR.Editor.SettingsUtil.HybridCLRDataDir + "/iOSBuild/");
                sbShellScript.AppendLine("chmod +x build.sh");
                sbShellScript.AppendLine("sh build.sh");
                sbShellScript.AppendLine ("echo PreBuildI12CPP Finished");
                pbProject.InsertShellScriptBuildPhase(0, targetGuid,"PreBuildIl2CPP", "", sbShellScript.ToString());
            }
            else Debug.Log("libil2cpp compiler file unfound:" + file);
        }
#endif
        public static bool CheckUp()
        {
            var localIl2cppDir = HybridCLR.Editor.SettingsUtil.LocalIl2CppDir;
            if (!Directory.Exists(localIl2cppDir))
            {
                string hybridclr_repo = HybridCLR.Editor.SettingsUtil.HybridCLRDataDir + "/hybridclr_repo";
                string il2cpp_plus_repo = HybridCLR.Editor.SettingsUtil.HybridCLRDataDir + "/il2cpp_plus_repo";
                if (Directory.Exists(hybridclr_repo) && Directory.Exists(il2cpp_plus_repo))
                {
                    string excudeRoot = EditorApplication.applicationContentsPath.Replace("\\", "/");
                    if (excudeRoot[excudeRoot.Length - 1] != '/') excudeRoot += "/";
                    string il2cpp = excudeRoot + "/il2cpp";
                    string monobleeding = excudeRoot + "/MonoBleedingEdge";
                    if (Directory.Exists(il2cpp) && Directory.Exists(monobleeding))
                    {
                        CopyDir(il2cpp, HybridCLR.Editor.SettingsUtil.LocalIl2CppDir);
                        CopyDir(monobleeding, HybridCLR.Editor.SettingsUtil.LocalUnityDataDir + "/MonoBleedingEdge");

                        CopyDir(il2cpp_plus_repo + "/libil2cpp", HybridCLR.Editor.SettingsUtil.LocalIl2CppDir + "/libil2cpp");
                        CopyDir(hybridclr_repo + "/hybridclr", HybridCLR.Editor.SettingsUtil.LocalIl2CppDir + "/libil2cpp/hybridclr");
                    }
                    else
                    {
                        UnityEditor.EditorUtility.DisplayDialog("提示", il2cpp + "\r\n" + monobleeding + "\r\n目录不存在", "好的");
                        return false;
                    }
                }
                else
                {
                    //Debug.LogError($"本地il2cpp目录:{localIl2cppDir} 不存在，未安装本地il2cpp。请手动执行一次 {BuildConfig.HybridCLRDataDir} 目录下的 init_local_il2cpp_data.bat 或者 init_local_il2cpp_data.sh 文件");
                    UnityEditor.EditorUtility.DisplayDialog("提示", $"本地il2cpp目录:{localIl2cppDir} 不存在，未安装本地il2cpp。请手动执行一次 {HybridCLR.Editor.SettingsUtil.HybridCLRDataDir} 目录下的 init_local_il2cpp_data.bat 或者 init_local_il2cpp_data.sh 文件", "好的");
                    return false;
                }
            }
            Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", localIl2cppDir);
            return true;
        }
        public static void GenerateAll()
        {
          //  AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            HybridCLR.Editor.Commands.PrebuildCommand.GenerateAll();
        }

        public static List<string> AOTAssemblyNames
        {
            get { return HybridCLR.Editor.SettingsUtil.AOTAssemblyNames; }
        }

        public static List<string> HotUpdateAssemblys
        {
            get { return HybridCLR.Editor.SettingsUtil.HotUpdateAssemblyFilesExcludePreserved; }
        }

        public static string HotUpdateDllsOutputDir
        {
            get { return HybridCLR.Editor.SettingsUtil.GetHotUpdateDllsOutputDirByTarget(EditorUserBuildSettings.activeBuildTarget); }
        }

        public static string AssembliesPostIl2CppStripDir
        {
            get { return HybridCLR.Editor.SettingsUtil.GetAssembliesPostIl2CppStripDir(EditorUserBuildSettings.activeBuildTarget); }
        }

        //------------------------------------------------------
        static void CopyDir(string srcDir, string destDir, HashSet<string> vFilerExtension = null, HashSet<string> vIgoreExtension = null)
        {
            if (srcDir.Length <= 0 || destDir.Length < 0) return;

            try
            {
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destDir))
                {
                    System.IO.Directory.CreateDirectory(destDir);
                }
                string tile = "Copy:" + srcDir + "->" + destDir;
                EditorUtility.DisplayProgressBar(tile, "...", 0);
                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(srcDir);
                for (int i = 0; i < files.Length; ++i)
                {
                    string extension = Path.GetExtension(files[i]);
                    if (vIgoreExtension != null && vIgoreExtension.Contains(extension)) continue;
                    if (vFilerExtension != null && !vFilerExtension.Contains(extension)) continue;

                    string name = System.IO.Path.GetFileName(files[i]);
                    string dest = System.IO.Path.Combine(destDir, name);
                    System.IO.File.Copy(files[i], dest, true);//复制文件
                    EditorUtility.DisplayProgressBar(tile, files[i], (float)((float)i / (float)files.Length));
                }
                //得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(srcDir);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destDir, name);
                    CopyDir(folder, dest, vFilerExtension, vIgoreExtension);//构建目标路径,递归复制文件
                }
                EditorUtility.ClearProgressBar();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
#endif
    }
}