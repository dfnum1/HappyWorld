/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Login
作    者:	HappLI
描    述:	登录
*********************************************************************/

using System;
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Data;
using TopGame.Net;
using UnityEngine;
using TopGame.Base;
using Framework.Core;
using TopGame.UI;

namespace TopGame.Logic
{
    public class VersionCheck
    {
        public enum EState
        {
            None,
            BeginLoadUpdateFile,    //开始下载热更内容文件
            LoadUpdateFiling,       //下载热更内容文件实时监控
            EndLoadUpdateFile,      //下载热更内容文件结束
            LoadUpdateFileFailed,   //下载热更内容文件失败

            BeginDonwloadAssetBundle,   //开始下载热更数据
            DonwloadAssetBundling,      //下载热更数据实时监控
            EndDonwloadAssetBundle,     //下载热更数据结束
            DonwloadAssetBundleFailed,  //下载热更数据失败

            BeginInitialize,            //开始初始化
            EndInitialize,              //初始化失败

            Cancel,                     //取消下载
            Abort,                      //中断下载

            Failed,                     //热更失败
            Succssed,                   //热更成功

        //    Overed,     //热更结束
        }
        EState m_eVersionState = EState.None;
        Dictionary<string, Package.UpdateItem> m_vLoadFiles = new Dictionary<string, Package.UpdateItem>();
        Core.FileDownload m_CurDownloadFile = null;

        Core.UpdateDownloader m_pDownload = null;

        string m_strServerCatchVersion = "";
        string m_strUpdateDir = "";
        string m_strDownloadCatch = "";
        HashSet<string> m_DownloadCatchs = null;

        UI.StartUpLoading m_UpdateLoading = null;

        bool m_bActive = false;
        FrameworkStartUp m_pStartUp = null;
        //------------------------------------------------------
        public VersionCheck(FrameworkStartUp startUp)
        {
            m_bActive = false;
            m_pStartUp = startUp;
            m_UpdateLoading = startUp.GetStartUpLoading();
        }
        //------------------------------------------------------
        public void Start()
        {
            m_bActive = true;
            FileSystem pFileSystem = GetFileSystem();
            if(pFileSystem == null)
            {
                Debug.LogError("filesytem no create!");
                return;
            }
            string versionsUrl = SvrData.ServerList.GetHotAddress();
            bool isUpdate = SvrData.ServerList.GetVersionIsUpdate();
            Framework.Plugin.Logger.Info("Is Hot Update:" + isUpdate);
            if (string.IsNullOrEmpty(versionsUrl))
            {
                isUpdate = false;
            }
            m_UpdateLoading.Show();
            m_UpdateLoading.ShowLoadTexts(true);
            m_UpdateLoading.ShowBG(true);
            m_UpdateLoading.ShowProgressBar(true);
            m_UpdateLoading.SetProgressTip(80010238, TextAnchor.MiddleRight);
            m_vLoadFiles.Clear();
            m_strServerCatchVersion = "";
            if (isUpdate)
            {
                StartUpLoading.SetAutoHide(false);
                if (versionsUrl != null && versionsUrl[versionsUrl.Length - 1] != '/')
                    versionsUrl += "/";
                Framework.Plugin.Logger.Info("versionsUrl:" + versionsUrl);

                m_strDownloadCatch = Framework.Core.BaseUtil.stringBuilder.Append(pFileSystem.UpdateDataPath).Append("download.catch").ToString();
                m_strUpdateDir = Framework.Core.BaseUtil.stringBuilder.
                    Append(versionsUrl).
                    Append(Application.platform.ToString().ToLower()).Append("/").
                    Append(pFileSystem.PlublishVersion).Append("/").ToString();
                Framework.Plugin.Logger.Info("Update Dir:" + m_strUpdateDir);

                
                if (isUpdate && pFileSystem.GetStreamType() >= EFileSystemType.AssetBundle)
                {
                    m_pDownload = new UpdateDownloader(m_strUpdateDir);
                    m_eVersionState = EState.BeginLoadUpdateFile;
                    m_DownloadCatchs = new HashSet<string>();
                    OnStateChange();
                }
                else
                {
                    m_eVersionState = EState.Succssed;
                    OnStateChange();
                    //    m_eVersionState = EState.Overed;
                }
            }
            else
            {
                m_UpdateLoading.Show();
                m_UpdateLoading.SetToProgress(100);
                m_eVersionState = EState.Succssed;
                OnStateChange();
                StartUpLoading.SetAutoHide(true);
            }
            
        }
        //------------------------------------------------------
        public void Exit()
        {
            m_UpdateLoading.SetToProgress(100);
            m_bActive = false;
        }
        //------------------------------------------------------
        void OnStateChange()
        {
            if (m_eVersionState == EState.Succssed)
            {
                m_UpdateLoading.ShowLoadTexts(false);
                m_UpdateLoading.StopScrollingText();
                m_UpdateLoading.ShowProgressBar(true);
                UI.StartUpUpdateUI.SetProgressTip("");
                m_UpdateLoading.SetToProgress((long)(100));
                if (m_pStartUp != null)
                    m_pStartUp.SetSection(EStartUpSection.AppAwake);
            }
        }
        //------------------------------------------------------
        public void Update(float fFrameTime)
        {
            if (!m_bActive) return;
            FileSystem pFileSystem = GetFileSystem();
            if (pFileSystem == null)
                return;
            switch (m_eVersionState)
            {
                case EState.BeginLoadUpdateFile:
                    {
                        m_eVersionState = EState.LoadUpdateFiling;
                        if (m_CurDownloadFile == null) m_CurDownloadFile = new Core.FileDownload(m_strUpdateDir, pFileSystem.UpdateDataPath, "updates.json");
                        else m_CurDownloadFile.Reset(m_strUpdateDir, pFileSystem.UpdateDataPath, "updates.json");
                        m_CurDownloadFile.Start();
                        UI.StartUpUpdateUI.BeginDownloadBaseFile();
                        OnStateChange();
                    }
                    break;
                case EState.LoadUpdateFiling:
                    {
                        if(m_CurDownloadFile == null || m_CurDownloadFile.isFailed)
                        {
                            m_eVersionState = EState.LoadUpdateFileFailed;
                            OnStateChange();

                            m_CurDownloadFile.Abort();
                            m_CurDownloadFile = null;
                        }
                        else if(m_CurDownloadFile.isDone)
                        {
                            m_eVersionState = EState.EndLoadUpdateFile;

                            OnStateChange();
                            m_CurDownloadFile.Abort();
                            m_CurDownloadFile = null;
                        }
                    }
                    break;
                case EState.LoadUpdateFileFailed:
                    {
                        m_eVersionState = EState.Failed;
                        OnStateChange();
                    }
                    break;
                case EState.EndLoadUpdateFile:
                    {
                        CheckDownLoadDatas();
                        if (m_vLoadFiles.Count > 0)
                        {
                            //这边开始下载资源,下载资源前要判断一下资源大小提示框,还有网络类型判断

                            System.Action confirm = null;
                            System.Action cancel = null;
                            switch (Application.internetReachability)
                            {
                                case NetworkReachability.NotReachable://无网络
                                    confirm = () =>
                                    {
                                        //重新下载
                                        OnPartialDownload();
                                    };
                                    cancel = () =>
                                    {
                                        Application.Quit();
                                    };

                                    UI.StartUpUpdateUI.ShowTips(true, 80010234, confirm, cancel);
                                    break;
                                case NetworkReachability.ReachableViaCarrierDataNetwork://使用数据
                                    long size = GetTotalDownloadSize(m_vLoadFiles);//获取到文件大小,单位字节
                                    size /= 1024;//字节转kb 
                                    long tipSize = UI.StartUpUpdateUI.GetDownloadTipsSize();
                                    if (size >= tipSize)
                                    {
                                        //弹窗提示是否下载
                                        confirm = () =>
                                        {
                                            BeginDonwloadAssetBundle();
                                        };
                                        cancel = () =>
                                        {
                                            Application.Quit();
                                        };
                                        string tips = string.Format(ALocalizationManager.ToLocalization(80010210),GlobalUtil.FormBytes(size * 1024));
                                        UI.StartUpUpdateUI.ShowTips(true, tips, confirm, cancel);
                                        break;
                                    }

                                    BeginDonwloadAssetBundle();

                                    break;
                                case NetworkReachability.ReachableViaLocalAreaNetwork://wifi或者电缆
                                    BeginDonwloadAssetBundle();
                                    break;
                                default:
                                    break;
                            }
                            
                            
                        }
                        else
                        {
                            m_eVersionState = EState.Succssed;
                            OnStateChange();
                        }
                    }
                    break;
                case EState.BeginDonwloadAssetBundle:
                    {
                        m_eVersionState = EState.DonwloadAssetBundling;
                        OnStateChange();
                    }
                    break;
                case EState.DonwloadAssetBundling:
                    {
                        //这边是下载中,如果网络中断就提示,后面要测试一下从wifi中断到数据,或者直接关掉数据会有什么表现

                        m_UpdateLoading.SetToProgress(m_pDownload.completedSize);

                        if (m_pDownload.isDone)
                        {
                            m_eVersionState = EState.EndDonwloadAssetBundle;
                            OnStateChange();
                        }
                        if (m_pDownload.isFailed)
                        {
                            m_eVersionState = EState.DonwloadAssetBundleFailed;
                            Framework.Plugin.Logger.Error("下载热更新资源失败,当前网络状态:" + Application.internetReachability);
                            //这边进行弹窗
                            //如果网络下载中断,会跳转到这边
                            Action confirm = null;
                            Action cancel = null;
                            switch (Application.internetReachability)
                            {
                                case NetworkReachability.NotReachable:
                                    //提示断网
                                    confirm = () =>
                                    {
                                        //重新下载
                                        OnPartialDownload();
                                    };
                                    cancel = () =>
                                    {
                                        Application.Quit();
                                    };
                                    UI.StartUpUpdateUI.ShowTips(true, 80010234, confirm, cancel);

                                    Framework.Plugin.Logger.Warning("下载失败,当前没有网络");
                                    break;
                                case NetworkReachability.ReachableViaCarrierDataNetwork:
                                    //提示数据
                                    confirm = () =>
                                    {
                                        //重新下载
                                        OnPartialDownload();
                                    };
                                    cancel = () =>
                                    {
                                        Application.Quit();
                                    };
                                    if (m_pDownload == null)
                                    {
                                        return;
                                    }
                                    //判断大小 
                                    long size = m_pDownload.totalSize;
                                    size /= 1024;//字节转kb 
                                    long tipSize = UI.StartUpUpdateUI.GetDownloadTipsSize();
                                    if (size >= tipSize)
                                    {
                                        string tips = string.Format(ALocalizationManager.ToLocalization(80010210), GlobalUtil.FormBytes(size * 1024));
                                        UI.StartUpUpdateUI.ShowTips(true, tips, confirm, cancel);
                                    }
                                    else
                                    {
                                        OnPartialDownload();
                                    }
                                    //UI.StartUpUpdateUI.ShowTips(true, 80010211, confirm, cancel);

                                    Framework.Plugin.Logger.Warning("下载失败,当前为数据状态");
                                    break;
                                case NetworkReachability.ReachableViaLocalAreaNetwork:
                                    //继续下载,这边可能是从数据改到wifi
                                    OnPartialDownload();
                                    //提示数据
                                    //confirm = () =>
                                    //{
                                    //    //重新下载
                                    //    OnPartialDownload();
                                    //};
                                    //cancel = () =>
                                    //{
                                    //    Application.Quit();
                                    //};
                                    //UI.StartUpUpdateUI.ShowTips(true, 80010211, confirm, cancel);

                                    Framework.Plugin.Logger.Warning("下载失败,当前为wifi状态,直接下载");
                                    break;
                                default:
                                    break;
                            }
                            OnStateChange();
                        }
                    }
                    break;
                case EState.EndDonwloadAssetBundle:
                    {
                        SaveDownloadCacheData();
                        m_eVersionState = EState.BeginInitialize;
                        OnStateChange();
                    }
                    break;
                case EState.DonwloadAssetBundleFailed:
                    {
                        
                    }
                    break;
                case EState.BeginInitialize:
                    {
                        m_eVersionState = EState.EndInitialize;
                        OnStateChange();
                    }
                    break;
                case EState.EndInitialize:
                    {
                        m_eVersionState = EState.Succssed;
                        OnStateChange();
                    }
                    break;
                case EState.Cancel:
                    {
                        m_eVersionState = EState.None;
                        SaveDownloadCacheData();
                        if (m_pDownload != null)
                        {
                            m_pDownload.Cancel();
                            m_pDownload = null;
                        }
                        if(m_CurDownloadFile != null)
                        {
                            m_CurDownloadFile.Cancel();
                            m_CurDownloadFile = null;
                        }
                        OnStateChange();
                    }
                    break;
                case EState.Abort:
                    {
                        m_eVersionState = EState.None;
                        SaveDownloadCacheData();
                        if (m_pDownload != null)
                        {
                            m_pDownload.Abort();
                            m_pDownload = null;
                        }
                        if (m_CurDownloadFile != null)
                        {
                            m_CurDownloadFile.Abort();
                            m_CurDownloadFile = null;
                        }
                        OnStateChange();
                    }
                    break;
                case EState.Succssed:
                    {
                        SaveDownloadCacheData();
                        SaveLocalVersion();
                 //       m_eVersionState = EState.Overed;
                        m_UpdateLoading.StopScrollingText();
                    }
                    break;
                case EState.Failed:
                    {
                        SaveDownloadCacheData();
                        // m_eVersionState = EState.None;
                        //先暂时可进入游戏，之后做提示
                        m_eVersionState = EState.Succssed;
                        OnStateChange();
                //        m_eVersionState = EState.Overed;
                    }
                    break;
//                 case EState.Overed:
//                     {
//                         m_UpdateLoading.ShowLoadTexts(false);
//                         m_UpdateLoading.SetToProgress((long)(Data.DataManager.getInstance().Progress*100));
//                         m_UpdateLoading.SetTotalProgress(200);
//                     }
//                     break;
            }
        }
        //------------------------------------------------------
        public void Shutdown()
        {
            Abort();
            Update(0);
        }
        //------------------------------------------------------
        public void Cancel()
        {
            if(m_eVersionState != EState.Cancel)
                m_eVersionState = EState.Cancel;
        }
        //------------------------------------------------------
        public void Abort()
        {
            if (m_eVersionState != EState.Abort)
                m_eVersionState = EState.Abort;
        }
        //------------------------------------------------------
        void SaveDownloadCacheData()
        {
            if (m_pDownload == null) return;
            bool bDirty = false;
            if (m_pDownload.completeDownloads != null && m_pDownload.completeDownloads.Count > 0)
            {
                foreach (var item in m_pDownload.completeDownloads)
                {
                    var hashCode = item.md5;
                    if (!m_DownloadCatchs.Contains(hashCode))
                    {
                        m_DownloadCatchs.Add(hashCode);
                        bDirty = true;
                    }
                }
            }
            if(bDirty)
            {
                if (System.IO.File.Exists(m_strDownloadCatch))
                {
                    try
                    {
                        string[] lines = System.IO.File.ReadAllLines(m_strDownloadCatch);
                        if (lines != null)
                        {
                            for (int i = 0; i < lines.Length; ++i)
                            {
                                m_DownloadCatchs.Add(lines[i]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning("parse download catch file error!");
                    }
                }
                try
                {
                    if (System.IO.File.Exists(m_strDownloadCatch))
                        System.IO.File.Delete(m_strDownloadCatch);
                    System.IO.FileStream fs = new System.IO.FileStream(m_strDownloadCatch, System.IO.FileMode.OpenOrCreate);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
                    foreach (var db in m_DownloadCatchs)
                    {
                        sw.WriteLine(db);
                    }
                    sw.Close();
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("save download catch failed:" + ex.StackTrace);
                }
            }
        }
        //------------------------------------------------------
        void SaveLocalVersion()
        {
            FileSystem pFileSystem = GetFileSystem();
            if (pFileSystem == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(m_strServerCatchVersion) || 
                m_strServerCatchVersion.CompareTo(pFileSystem.PlublishVersion) ==0) return;
            try
            {
                if (System.IO.File.Exists(pFileSystem.LocalVersionFile))
                    System.IO.File.Delete(pFileSystem.LocalVersionFile);
                System.IO.FileStream fs = new System.IO.FileStream(pFileSystem.LocalVersionFile, System.IO.FileMode.OpenOrCreate);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.Write(m_strServerCatchVersion);
                sw.Close();
            }
            catch (Exception ex)
            {
                Debug.LogWarning("save local version failed:" + ex.StackTrace);
            }
        }
        //------------------------------------------------------
        void CheckDownLoadDatas()
        {
            FileSystem pFileSystem = GetFileSystem();
            if (pFileSystem == null)
            {
                return;
            }
            long curVersion = ConverVersion(pFileSystem.Version);
            string file = pFileSystem.LocalUpdateFile;
            //Debug.LogError("检测下载文件: " + file);
            if (System.IO.File.Exists(file))
            {
                m_DownloadCatchs.Clear();
                if (System.IO.File.Exists(m_strDownloadCatch))
                {
                    try
                    {
                        string[] lines = System.IO.File.ReadAllLines(m_strDownloadCatch);
                        if (lines != null)
                        {
                            for (int i = 0; i < lines.Length; ++i)
                            {
                                m_DownloadCatchs.Add(lines[i]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning("parse download catch file error!");
                    }
                }

                try
                {
                    long totalSize = 0;
                    if (pFileSystem.GetStreamType() == EFileSystemType.EncrptyPak )
                    {
                        string strCode = System.IO.File.ReadAllText(file);
                        Package.PakUpdateFiles updataData = JsonUtility.FromJson<Package.PakUpdateFiles>(strCode);
                        string strRoot = pFileSystem.UpdateDataPath;
                        if (updataData.datas!=null)
                        {
                            if (ConverVersion(updataData.version) >= curVersion)
                            {
                                m_strServerCatchVersion = updataData.version;
                                for (int i = 0; i < updataData.datas.Count; ++i)
                                {
                                    Package.PakUpdateFiles.PakData item = updataData.datas[i];
                                    if (!m_DownloadCatchs.Contains(item.md5))
                                    {
                                        string filePath = Framework.Core.BaseUtil.stringBuilder.Append(strRoot).Append(item.remotePath).ToString();
                                        if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                                        {
                                            Package.UpdateItem uItem = new Package.UpdateItem();
                                            uItem.remotePath = item.remotePath;
                                            uItem.abName = item.remotePath;//localfile
                                            uItem.md5 = item.md5;
                                            m_vLoadFiles[uItem.abName] = uItem;
                                            totalSize += item.size;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string strCode = System.IO.File.ReadAllText(file);
                        Package.UpdateFiles updataData = JsonUtility.FromJson<Package.UpdateFiles>(strCode);

                        string strRoot = pFileSystem.UpdateDataPath;
                        if (updataData.datas != null)
                        {
                            m_strServerCatchVersion = updataData.version;
                            updataData.Sort();
                            for (int i = 0; i < updataData.datas.Count; ++i)
                            {
                                if (updataData.datas[i].datas == null) continue;
                                if (ConverVersion(updataData.datas[i].version) < curVersion)
                                    continue;
                                for (int j = 0; j < updataData.datas[i].datas.Length; ++j)
                                {
                                    Package.UpdateItem item = updataData.datas[i].datas[j];
                                    Package.EnterData pData = pFileSystem.FindEnterData(item.abName);
                                    bool bDownload = false;
                                    if (pData == null || !pData.isValid() || pData.md5.CompareTo(item.md5) != 0)
                                    {
                                        bDownload = true;
                                    }

                                    if (bDownload)
                                    {
                                        string filePath = Framework.Core.BaseUtil.stringBuilder.Append(strRoot).Append(item.abName).ToString();
                                        pFileSystem.AddFile(item.abName, filePath, item.md5, item.depends);

                                        if (!m_DownloadCatchs.Contains(item.md5) || !System.IO.File.Exists(filePath))
                                        {
                                            item.remotePath = Framework.Core.BaseUtil.stringBuilder.Append(updataData.datas[i].rootDir).Append("/").Append(item.abName).ToString();
                                            m_vLoadFiles[item.abName] = item;
                                            totalSize += item.size;
                                        }
                                    }
                                }
                                for (int j = 0; j < updataData.datas[i].rawDatas.Length; ++j)
                                {
                                    Package.UpdateItem item = updataData.datas[i].rawDatas[j];
                                    if (!m_DownloadCatchs.Contains(item.md5))
                                    {
                                        string filePath = Framework.Core.BaseUtil.stringBuilder.Append(strRoot).Append(item.abName).ToString();
                                        if (!m_DownloadCatchs.Contains(item.md5) || !System.IO.File.Exists(filePath))
                                        {
                                            item.remotePath = Framework.Core.BaseUtil.stringBuilder.Append(updataData.datas[i].rootDir).Append("/").Append(item.abName).ToString();
                                            m_vLoadFiles[item.abName] = item;
                                            totalSize += item.size;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                    m_UpdateLoading.SetTotalProgress(totalSize);
                }
                catch (System.Exception ex)
                {

                }
            }
        }
        //------------------------------------------------------
        static long ConverVersion(string version)
        {
            long result = 0;
            long.TryParse(version.Replace(".",""), out result);
            return result;
        }
        //------------------------------------------------------
        long GetTotalDownloadSize(Dictionary<string, Package.UpdateItem> vDownloadFiles)
        {
            long size = 0;
            foreach (var item in vDownloadFiles)
            {
                size += item.Value.size;
            }
            return size;
        }
        //------------------------------------------------------
        void BeginDonwloadAssetBundle()
        {
            FileSystem pFileSystem = GetFileSystem();
            if (pFileSystem == null)
            {
                return;
            }
            if (m_pDownload.Start(pFileSystem.UpdateDataPath, m_vLoadFiles))
            {
                m_eVersionState = EState.BeginDonwloadAssetBundle;
            }
            else
                m_eVersionState = EState.DonwloadAssetBundleFailed;
            OnStateChange();
        }
        //------------------------------------------------------
        void OnPartialDownload()
        {
            if (m_pDownload == null)
            {
                return;
            }
            m_pDownload.OnPartialDownload();
            BeginDonwloadAssetBundle();
            if (m_pDownload != null)
            {
                m_UpdateLoading.SetTotalProgress(m_pDownload.totalSize);
            }
        }
        //------------------------------------------------------
        FileSystem GetFileSystem()
        {
            return FrameworkStartUp.GetFileSystem();
        }
        //------------------------------------------------------
        UI.StartUpLoading GetStartUpLoading()
        {
            if (m_pStartUp == null) return null;
            return m_pStartUp.GetStartUpLoading();
        }
    }
}
