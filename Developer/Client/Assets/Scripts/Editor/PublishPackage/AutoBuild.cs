/********************************************************************
生成日期:	25:7:2019   9:51
类    名: 	AutoBuild
作    者:	zdq
描    述:	自动打包脚本
*********************************************************************/
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static TopGame.ED.PublishPackage;

using UnityEditor.Callbacks;
using System;
#if UNITY_EDITOR_OSX
using UnityEditor.iOS.Xcode;
#endif

namespace TopGame.ED
{
    public class AutoBuild
    {
        static PublishPanel.PublishSetting m_BuildSetting = new PublishPanel.PublishSetting();

        public static string APKPath;
        public static string GetCurrentPlublishTag()
        {
            string curTagFile = Application.dataPath + "/../Publishs/Temps/publishTag.txt";
            if (System.IO.File.Exists(curTagFile))
                return System.IO.File.ReadAllText(curTagFile).Trim();
            return "";
        }

        static Dictionary<string, string> ParseCmd(string[] argvs)
        {
            Dictionary<string, string> keyValue = new Dictionary<string, string>();
            string externCmd = null;
            for (int i = 0; i < argvs.Length; ++i)
            {
                Debug.Log("argv:" + argvs[i]);
                if (argvs[i].Contains("externCmd"))
                {
                    externCmd = argvs[i];
                }
                else if (argvs[i].Contains("--out"))
                {
                    keyValue["outPutPkg"] = argvs[i].Replace("--out=", "").Trim();
                }
            }
            if (!string.IsNullOrEmpty(externCmd))
            {
                keyValue["externCmd"] = externCmd;
                externCmd = externCmd.Trim().Replace("--externCmd=", "");
                string[] cmds = externCmd.Split('/');
                for (int i = 0; i < cmds.Length; ++i)
                {
                    if (cmds[i].Contains("settingtag"))
                    {
                        string[] kv = cmds[i].Split('=');
                        if (kv.Length == 2)
                            keyValue["settingTag"] = kv[1];
                    }
                    else if (cmds[i].Contains("onlyab"))
                    {
                        string[] kv = cmds[i].Split('=');
                        if (kv.Length == 2)
                            keyValue["onlyBuildAB"] = kv[1].ToLower();
                    }
                    else if (cmds[i].Contains("hotup"))
                    {
                        string[] kv = cmds[i].Split('=');
                        if (kv.Length == 2)
                            keyValue["hotup"] = kv[1].ToLower();
                    }
                    else if (cmds[i].Contains("argvCmd"))
                    {
                        string[] kv = cmds[i].Split('=');
                        if (kv.Length == 2)
                            keyValue["argvCmd"] = kv[1].ToLower();
                    }
                }
            }
            return keyValue;
        }

        //shell 脚本调用
        static void Build()
        {
            if (Directory.Exists(Application.dataPath + "/../Publishs/Temps"))
            {
                Directory.Delete(Application.dataPath + "/../Publishs/Temps", true);
            }
            Directory.CreateDirectory(Application.dataPath + "/../Publishs/Temps");
#if USE_HYBRIDCLR
            HybridCLRHelper.ClearTemp();
#endif

            var cmdKeyValue = ParseCmd(System.Environment.GetCommandLineArgs());
            bool hotUpdate = false;
            bool onlyBuildAB = false;
            string settingTag = "";
            string externCmd = "";
            string outPutPkg = "";
            string argvCmd = "";
            if (cmdKeyValue.ContainsKey("onlyBuildAB")) onlyBuildAB = cmdKeyValue["onlyBuildAB"].CompareTo("1") == 0 || cmdKeyValue["onlyBuildAB"].CompareTo("true") == 0;
            if (cmdKeyValue.ContainsKey("settingTag")) settingTag = cmdKeyValue["settingTag"];
            if (cmdKeyValue.ContainsKey("externCmd")) externCmd = cmdKeyValue["externCmd"];
            if (cmdKeyValue.ContainsKey("outPutPkg")) outPutPkg = cmdKeyValue["outPutPkg"];
            if (cmdKeyValue.ContainsKey("hotup")) hotUpdate = cmdKeyValue["hotup"].CompareTo("1") == 0 || cmdKeyValue["hotup"].CompareTo("true") == 0;
            if (cmdKeyValue.ContainsKey("argvCmd")) argvCmd = cmdKeyValue["argvCmd"];

            Debug.Log("externCmd:" + externCmd);
            Debug.Log("settingTag:" + settingTag);
            Debug.Log("onlyBuildAB:" + onlyBuildAB);
            Debug.Log("hotup:" + hotUpdate);
            Debug.Log("argvCmd:" + argvCmd);

            string curTagFile = Application.dataPath + "/../Publishs/Temps/";
            if (!System.IO.Directory.Exists(curTagFile))
                System.IO.Directory.CreateDirectory(curTagFile);
            curTagFile += "/publishTag.txt";
            if (System.IO.File.Exists(curTagFile)) System.IO.File.Delete(curTagFile);
            System.IO.File.WriteAllText(curTagFile, settingTag);


            m_BuildSetting = PublishPanel.LoadSetting(settingTag);

            SwitchPlatform();

            Debug.Log("output:" + outPutPkg);

            Dictionary<string, string> vCmdKV = new Dictionary<string, string>();
            if(!string.IsNullOrEmpty(argvCmd))
            {
                if(argvCmd.Contains("@"))
                {
                    string[] cmds = argvCmd.Split('@');
                    for (int i = 0; i < cmds.Length; ++i)
                    {
                        if (cmds[i].Contains("="))
                        {
                            string[] kv = cmds[i].Split('=');
                            if (kv.Length == 2)
                                vCmdKV[kv[0].ToLower()] = kv[1];
                            else vCmdKV[kv[0].ToLower()] = "";
                        }
                        else vCmdKV[cmds[i].ToLower()] = "";
                    }
                }
                else
                {
                    if (argvCmd.Contains("="))
                    {
                        string[] kv = argvCmd.Split('=');
                        if (kv.Length == 2)
                            vCmdKV[kv[0].ToLower()] = kv[1];
                        else vCmdKV[kv[0].ToLower()] = "";
                    }
                    else vCmdKV[argvCmd.ToLower()] = "";
                }

            }

            if(vCmdKV.ContainsKey("delhotup"))
            {
                Debug.Log("清除热更数据");
                try
                {
                    string outPut = m_BuildSetting.GetBuildOutputRoot(m_BuildSetting.buildTarget);
                    string strUpdate = outPut + "/" + m_BuildSetting.GetPlatform(m_BuildSetting.buildTarget).version;
                    if(Directory.Exists(strUpdate))
                    {
                        Directory.Delete(strUpdate, true);
                    }
                }
                catch { }
            }

            int savePkgCnt = 5;
            //! packge old deldete
            if(System.IO.Directory.Exists(outPutPkg))
            {
                string[] dires = System.IO.Directory.GetDirectories(outPutPkg);
                for(int i =0; i < dires.Length; ++i)
                {
                    if (dires[i].Contains("BurstDebugInformation_DoNotShip"))
                        Directory.Delete(dires[i], true);
                }

                if(!hotUpdate)
                {
                    string[] files = System.IO.Directory.GetFiles(outPutPkg, "*.apk");
                    Debug.Log("cur android packageCnt:" + files.Length);
                    if (files.Length > savePkgCnt)
                    {
                        List<System.IO.FileInfo> vFiles = new List<FileInfo>();
                        for (int i = 0; i < files.Length; ++i)
                        {
                            System.IO.FileInfo var = new System.IO.FileInfo(files[i]);
                            vFiles.Add(var);
                        }
                        vFiles.Sort((FileInfo o1, FileInfo o2) => { return (int)(o1.CreationTime.Ticks - o2.CreationTime.Ticks); });
                        while (vFiles.Count > savePkgCnt)
                        {
                            if (vFiles[0].Exists) File.Delete(vFiles[0].FullName);
                            vFiles.RemoveAt(0);
                        }
                    }
                    files = System.IO.Directory.GetFiles(outPutPkg, "*.ipa");
                    Debug.Log("cur ios packageCnt:" + files.Length);
                    if (files.Length > savePkgCnt)
                    {
                        List<System.IO.FileInfo> vFiles = new List<FileInfo>();
                        for (int i = 0; i < files.Length; ++i)
                        {
                            System.IO.FileInfo var = new System.IO.FileInfo(files[i]);
                            vFiles.Add(var);
                        }
                        vFiles.Sort((FileInfo o1, FileInfo o2) => { return (int)(o1.CreationTime.Ticks - o2.CreationTime.Ticks); });
                        while (vFiles.Count > savePkgCnt)
                        {
                            if (vFiles[0].Exists) File.Delete(vFiles[0].FullName);
                            vFiles.RemoveAt(0);
                        }
                    }
                }
                
            }
            if(m_BuildSetting!=null)
                HybridCLRHelper.IsEnable = m_BuildSetting.hotType == PublishPanel.EHotType.HybridCLR;

            PublishPanel.WritePublishingNameFile(m_BuildSetting);
            CompileAssetBundles.OnABBuildCallback = PublishPanel.BuildAB;
            CompileAssetBundles.OnMarkAbName = PublishPanel.OnMarkABName;
            CompileAssetBundles.OnPrepareMarkAbName = PublishPanel.OnPrepareMarkAbName;

            if(hotUpdate)
            {
                string version = m_BuildSetting.GetPlatform(m_BuildSetting.buildTarget).version;
                if (onlyBuildAB)
                    PublishPanel.BuildAndMarkAssetBundleAysn(m_BuildSetting, false, true);
                else
                    PublishPanel.FilterUpdateAssetBundle(m_BuildSetting, "resinfo.json", version, version);
            }
            else
            {
                PublishPanel.BuildAndMarkAssetBundleAysn(m_BuildSetting, !onlyBuildAB,false);
            }
        }
        //------------------------------------------------------
        static bool GetIsOnlyBuildAssetbundle()
        {
            bool onlyBuildAB = false;
            Dictionary<string, string> args = GetArgs("TopGame.ED.AutoBuild.Build");//根据打开时传递的函数名去找后面的参数
            foreach (var item in args)
            {
                Debug.Log("key:" + item.Key + ",value=" + item.Value);
            }
            if (args.ContainsKey("onlyBuildAB"))
            {
                string value = args["onlyBuildAB"];
                if (value.Contains("true"))
                {
                    onlyBuildAB = true;
                }
            }
            Debug.Log("onlyBuildAB:" + onlyBuildAB);
            return onlyBuildAB;
        }
        //------------------------------------------------------
        static void SwitchPlatform()
        {
            Dictionary<string, string> args = GetArgs("TopGame.ED.AutoBuild.Build");//根据打开时传递的函数名去找后面的参数
            foreach (var item in args)
            {
                Debug.Log("key:" + item.Key + ",value=" + item.Value);
            }
            if (args.ContainsKey("out"))
            {
                string value = args["out"];
                if (value.Contains("Android"))
                {
                    m_BuildSetting.buildTarget = BuildTarget.Android;
                }
                else if (value.Contains("Window"))
                {
                    m_BuildSetting.buildTarget = BuildTarget.StandaloneWindows;
                }
                else
                {
                    m_BuildSetting.buildTarget = BuildTarget.iOS;
                }
            }
            Debug.Log("当前选择的平台：" + m_BuildSetting.buildTarget);
        }
        //------------------------------------------------------
        /// <summary>
        /// 获得传入参数中，在方法名后的--开头的参数
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        static Dictionary<string,string> GetArgs(string methodName)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            bool isArg = false;
            foreach (var arg in System.Environment.GetCommandLineArgs())
            {
                if (isArg)
                {
                    if (arg.StartsWith("--"))
                    {
                        int splitIndex = arg.IndexOf("=");
                        if (splitIndex > 0)
                        {
                            args.Add(arg.Substring(2, splitIndex - 2), arg.Substring(splitIndex + 1));
                        }
                        else
                        {
                            args.Add(arg.Substring(2), "true");
                        }
                    }
                }
                else if(arg == methodName)
                {
                    isArg = true;
                }
            }

            return args;
        }
        //------------------------------------------------------
        /// <summary>
        /// 调用shell指令
        /// </summary>
        /// <param name="shell"></param>
        /// <param name="args"></param>
        static void CallShell(string shell,params string[] args)
        {
            string shellPath = Path.Combine(Application.dataPath + "/../Shell/" + shell);
            Debug.Log("shellPath=" + shellPath);
            string command = shellPath;
            foreach (var arg in args)
            {
                command += " " + arg;
            }
            Debug.Log("command=" + command);
            System.Diagnostics.Process.Start("/bin/bash",command);
            
        }
    }
}

