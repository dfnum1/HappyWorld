/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	UpdateDownloader
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core
{
    public class UpdateDownloader
    {
        public enum ErrorCode
        {
            None = 0,
            ParameterError,//参数错误
            TimeOut,//超时
            PreprocessError,//预处理错误

            InvalidURL = 1001,//未能识别url服务器
            ServerNoResponse = 1002,//服务器未响应
            DownloadFailed = 1003,//下载失败
            DownloadMainConfigFileFailed = 1004,//主配置文件下载失败
            DonwloadAssetBundleFailed = 1005,//AB下载失败
        }

        //并发下载
        public const int CONCURRENCE_DOWNLOAD_MAX = 1;

        public string url;
        public string path;

        public bool isDone { get; private set; }
        public ErrorCode errorCode { get; private set; }
        public bool isFailed { get { return errorCode != ErrorCode.None; } }
        public long completedSize { get; private set; }
        public long totalSize { get; private set; }
        public Dictionary<string, Package.UpdateItem> unCompleteDownloads { get; private set; }//需要下载的资源列表
        public List<Package.UpdateItem> completeDownloads { get; private set; }//已下载资源
        public List<Package.UpdateItem> failedDownloads { get; private set; }//下载失败的资源

        private List<HttpAsyDownload> m_Downloads = new List<HttpAsyDownload>();//

        private object m_Lock = new object();

        public UpdateDownloader(string url)
        {
            this.url = url;
            isDone = false;
            errorCode = ErrorCode.None;
            completedSize = 0;
            totalSize = 0;
            unCompleteDownloads = new Dictionary<string, Package.UpdateItem>();
              completeDownloads = new List<Package.UpdateItem>();
            failedDownloads = new List<Package.UpdateItem>();

            System.Net.ServicePointManager.DefaultConnectionLimit = CONCURRENCE_DOWNLOAD_MAX;
        }
        //------------------------------------------------------
        public bool Start(string path, Dictionary<string, Package.UpdateItem> vDownloadFiles)
        {
            Abort();

            OnInitializeDownload(path, vDownloadFiles);
            OnUpdateState();
            OnDownloadAll();
            return true;
        }
        //------------------------------------------------------
        public void Cancel()
        {
            foreach (var item in m_Downloads)
                item.Cancel();
        }
        //------------------------------------------------------
        public void Abort()
        {
            foreach (var item in m_Downloads)
                item.Abort();
        }
        //------------------------------------------------------
        void OnInitializeDownload(string path, Dictionary<string, Package.UpdateItem> vDownloadFiles)
        {
            this.path = path;
            unCompleteDownloads = vDownloadFiles;

            isDone = false;
            errorCode = ErrorCode.None;
            completeDownloads.Clear();
            failedDownloads.Clear();

            if (unCompleteDownloads == null) unCompleteDownloads = new Dictionary<string, Package.UpdateItem>();

            totalSize = 0;
            completedSize = 0;

            foreach (var item in unCompleteDownloads)
            {
                totalSize += item.Value.size;
            }
        }
        //------------------------------------------------------
        public bool IsDownLoading(string fileName)
        {
            var ab = m_Downloads.Find((d) => { return d.localName == fileName; });
            return ab != null;
        }
        //------------------------------------------------------
        HttpAsyDownload GetFreeDownload()
        {
            lock (m_Lock)
            {
                foreach (var item in m_Downloads)
                    if (item.isDone)
                        return item;

                if (m_Downloads.Count < System.Net.ServicePointManager.DefaultConnectionLimit)
                {
                    var item = new HttpAsyDownload(url);
                    m_Downloads.Add(item);
                    return item;
                }
                return null;
            }
        }
        //------------------------------------------------------
        /// <summary>
        /// 下载所有资源
        /// </summary>
        void OnDownloadAll()
        {
            lock (m_Lock)
            {
                foreach (var item in unCompleteDownloads)
                    if (!OnDownload(item.Value))
                        break;
            }
        }
        //------------------------------------------------------
        /// <summary>
        /// 更新状态
        /// </summary>
        void OnUpdateState()
        {
            isDone = unCompleteDownloads.Count == 0;
            errorCode = failedDownloads.Count > 0 ? ErrorCode.DownloadFailed : errorCode;
        }
        //------------------------------------------------------
        /// <summary>
        /// 启动下载
        /// </summary>
        bool OnDownload(Package.UpdateItem assetbundleName)
        {
            lock (m_Lock)
            {
                string fileName = assetbundleName.abName;
                if (!IsDownLoading(fileName))
                {
                    var d = GetFreeDownload();
                    if (null == d)
                        return false;
                    d.Start(path, assetbundleName.remotePath, fileName, OnDownloadNotify, OnDownloadError);
                }
                return true;
            }
        }
        //------------------------------------------------------
        void OnDownloadSuccess(string fileName)
        {
            lock (m_Lock)
            {
                Package.UpdateItem item;
                if(unCompleteDownloads.TryGetValue(fileName, out item))
                {
                    completeDownloads.Add(item);
                    unCompleteDownloads.Remove(fileName);
                }
            }
        }
        //------------------------------------------------------
        void OnDownloadNotify(HttpAsyDownload d, long size)
        {
            lock (m_Lock)
            {
                if (d.isDone)
                {
                    OnDownloadSuccess(d.localName);
                    OnDownloadAll();
                }

                completedSize += size;
                OnUpdateState();
            }
        }
        //------------------------------------------------------
        void OnDownloadError(HttpAsyDownload d)
        {
            lock (m_Lock)
            {
                Package.UpdateItem item;
                if (unCompleteDownloads.TryGetValue(d.localName, out item))
                {
                    failedDownloads.Add(item);
                }
                OnDownloadAll();
                OnUpdateState();
            }
        }
        //------------------------------------------------------
        /// <summary>
        /// 重试
        /// </summary>
        public void OnPartialDownload()
        {
            if (m_Downloads != null)
            {
                foreach (var item in m_Downloads)
                {
                    item.Abort();
                }
                m_Downloads.Clear();
            }
        }
    }
}