/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	FileDownload
作    者:	HappLI
描    述:	文件下载器
*********************************************************************/
namespace TopGame.Core
{
    public class FileDownload
    {
        public string url { get; private set; }
        public string path { get; private set; }
        public string fileName { get; private set; }
        public string remoteFileName { get; private set; }
        public bool isDone { get; private set; }
        public HttpAsyDownload.ErrorCode errorCode { get; private set; }
        public bool isFailed { get { return errorCode != HttpAsyDownload.ErrorCode.None; } }

        public long completeSize { get; private set; }//已下载文件大小
        public long totalSize { get; private set; }//总文件大小

        public Framework.Core.VariablePoolAble userCallData = null;
        public System.Action<FileDownload> OnDownloaded = null;
        private HttpAsyDownload m_Download;
        //------------------------------------------------------
        public FileDownload(string url,string path,string fileName, string remoteFileName="")
        {
            Reset(url, path, fileName, remoteFileName);
        }
        //------------------------------------------------------
        public void Reset(string url, string path, string fileName, string remoteFileName="")
        {
            if (m_Download !=null && this.url.CompareTo(url) != 0)
            {
                m_Download.Abort();
                m_Download = null;
            }
            this.url = url;
            this.path = path;
            this.fileName = fileName;
            if(string.IsNullOrEmpty(remoteFileName))
                this.remoteFileName = fileName;
            isDone = false;
            errorCode = HttpAsyDownload.ErrorCode.None;
            completeSize = 0;
            totalSize = 0;
            userCallData = null;
            OnDownloaded = null;
        }
        //------------------------------------------------------
        /// <summary>
        /// 开始下载
        /// </summary>
        public void Start()
        {
            //统计数据
            totalSize = 0;
            completeSize = 0;
            OnUpdateState();
            //下载
            OnDownload(fileName, remoteFileName);
        }
        //------------------------------------------------------
        /// <summary>
        /// 取消下载
        /// </summary>
        public void Cancel()
        {
            if (m_Download != null)
                m_Download.Cancel();
        }
        //------------------------------------------------------
        /// <summary>
        /// 终止下载
        /// </summary>
        public void Abort()
        {
            if (m_Download != null)
                m_Download.Abort();
        }
        //------------------------------------------------------
        private void OnUpdateState()
        {
            if(totalSize > 0)
            {
                isDone = m_Download != null && m_Download.isDone;
            }
        }
        //------------------------------------------------------
        private void OnDownload(string fileName, string remoteFileName)
        {
            if(m_Download == null) m_Download = new HttpAsyDownload(url);
            m_Download.Start(path, remoteFileName, fileName, OnDownloadnotifyCallback, OnDownloadErrorCallback);
        }
        //------------------------------------------------------
        void OnDownloadnotifyCallback(HttpAsyDownload l,long size)
        {
            completeSize = l.completeLength;
            totalSize = l.length;
            OnUpdateState();
            if (OnDownloaded != null) OnDownloaded(this);
        }
        //------------------------------------------------------
        void OnDownloadErrorCallback(HttpAsyDownload l)
        {
            isDone = false;
            errorCode = l.errorCode;
            if (OnDownloaded != null) OnDownloaded(this);
        }
    }
}