/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	MessageSimulationEditorLogic
作    者:	HappLI
描    述:	消息模拟逻辑
*********************************************************************/
using System.Collections.Generic;
using TopGame.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace TopGame.ED
{
    public class SimulationNetWork : Framework.Module.AModule,INetWork, ISessionCallback
    {
        class ReqData : System.IComparable<ReqData>
        {
            public float reqSleep;
            public byte repeatTimes;
            public System.Action<string> onCallback;
            public string url;

            public UnityEngine.Networking.UnityWebRequest request;

            public int CompareTo(ReqData other)
            {
                return url.CompareTo(other.url);
            }
        }
        List<ReqData> m_vCallback = new List<ReqData>();

        [System.Serializable]
        public struct SvrItem
        {
            public int id;
            public string name;
            public string ip;
            public string port;
        }
        class Server
        {
            public List<SvrItem> list = new List<SvrItem>();
        }
        List<SvrItem> m_ServerList = new List<SvrItem>();

        private static SimulationNetWork ms_Instance = null;
        const int REC_MAXBUFFERSIZE = 1024 * 1024*4;//1 Mb
        const int SND_MAXBUFFERSIZE = 1024 * 1024 * 4;//1 Mb

        TcpServerSession m_pSession = null;

        OnRevicePacketMsg m_pPackageMsg = null;
        float m_fLastSendMessageTime = 0;
    //    Proto3.HeartRequest m_HeartRequest = new Proto3.HeartRequest();

        private HashSet<int> m_vLockMsgs = new HashSet<int>();

        int m_LastSendCode = 0;
        Google.Protobuf.IMessage m_LastSendMessage = null;
        //------------------------------------------------------
        public static SimulationNetWork getInstance()
        {
            if (ms_Instance == null)
            {
                Framework.Plugin.Logger.Break("请先注册网络模块");
            }
            return ms_Instance;
        }
        //------------------------------------------------------
        public SimulationNetWork()
        {
            ms_Instance = this;
            m_pSession = new TcpServerSession(this, REC_MAXBUFFERSIZE, SND_MAXBUFFERSIZE, 64, 10);
            m_pSession.Register(this);
            m_pSession.SetConnectTimeout(10000);
            m_fLastSendMessageTime = 0;
        }
        //------------------------------------------------------
        protected override void Awake()
        {

        }
        //------------------------------------------------------
        public void Start()
        {
            m_fLastSendMessageTime = 0;
            m_vLockMsgs.Clear();
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            Disconnect(ESessionType.Count);
            m_vLockMsgs.Clear();
        }
        //------------------------------------------------------
        public void RegisterSessionCallback(ISessionCallback pCallback)
        {
            if (m_pSession != null) m_pSession.Register(pCallback);
        }
        //------------------------------------------------------
        public void UnRegisterSessionCallback(ISessionCallback pCallback)
        {
            if (m_pSession != null) m_pSession.UnRegister(pCallback);
        }
        //------------------------------------------------------
        public AServerSession Connect(ESessionType type, string strIp, int nPort, System.Action<AServerSession> onCallback =null, long localConn = 0)
        {
            m_vLockMsgs.Clear();
            if (m_pSession != null) m_pSession.Connect(strIp, nPort, onCallback);
            return m_pSession;
        }
        //------------------------------------------------------
        public void Disconnect( ESessionType type, int index= 0)
        {
            if (m_pSession != null) m_pSession.Disconnect();
            m_vLockMsgs.Clear();
        }
        //------------------------------------------------------
        public void ReqHttp(string url, System.Action<string> onCallback, byte nRepeat = 6)
        {
            if (string.IsNullOrEmpty(url))
            {
                Framework.Plugin.Logger.Warning("request http url is null");
                if (onCallback != null)
                    onCallback("Faild");
                return;
            }
            ReqData reqData = new ReqData();
            reqData.url = url;
            if (m_vCallback.Contains(reqData))
            {
                return;
            }

            reqData.onCallback = onCallback;
            reqData.repeatTimes = nRepeat;
            reqData.request = null;
            reqData.reqSleep = 0;
            m_vCallback.Add(reqData);
        }
        //------------------------------------------------------
        public bool SendMessage(PacketBase pMsg)
        {
            if (m_pSession != null && m_pSession.Send(pMsg))
            {
                m_fLastSendMessageTime = Time.unscaledTime;
                return true;
            }
            return false;
        }
        //------------------------------------------------------
        public bool SendMessage(ESessionType type, int nCode, Google.Protobuf.IMessage pMsg, bool isCheckNet = true)
        {
            if (m_pSession != null && m_pSession.Send(nCode, pMsg) && isCheckNet)
            {
                m_fLastSendMessageTime = Time.unscaledTime;
                return true;
            }
            m_LastSendCode = nCode;
            m_LastSendMessage = pMsg;
            return false;
        }
        //------------------------------------------------------
        public void SendHeart(float fHeartGap)
        {
            if (Time.unscaledTime - m_fLastSendMessageTime < fHeartGap) return;
        //    SendMessage((int)Proto3.MID.HeartReq, m_HeartRequest);
        }
        //------------------------------------------------------
        public void OnSessionState(AServerSession session, ESessionState eState)
        {
            if(GameInstance.getInstance()!=null) GameInstance.getInstance().OnSessionState(session, eState);
        }
        //------------------------------------------------------
        public ESessionState GetSessionState(ESessionType type, int index = 0)
        {
            if (m_pSession != null)
                return m_pSession.GetState();
            return ESessionState.None;
        }
        //------------------------------------------------------
        public bool IsRegisterHandler(int nCode)
        {
            return true;
        }
        //------------------------------------------------------
        public void Register(int code, OnRevicePacketMsg onHandler)
        {
            m_pPackageMsg = onHandler;
        }
        //------------------------------------------------------
        public void UnRegister(int code)
        {

        }
        //------------------------------------------------------
        public void OnPackage(PacketBase pMsg)
        {
            if(m_pPackageMsg!=null)
            m_pPackageMsg(pMsg);
        }
        //------------------------------------------------------
        public void LateUpdate(float fFrame)
        {
            if (m_pSession != null)
            {
                m_pSession.OnMainUpdate(fFrame);
                m_pSession.OnNetThreadUpdate(0);
                m_pSession.OnNetThreadUpdate(1);

                if (GameInstance.getInstance()!=null) GameInstance.getInstance().OnNetWorkHeart(GetMessageInterval(), m_pSession, m_LastSendCode, m_LastSendMessage);
            }
            if(m_vCallback.Count>0)
            {
                ReqData reqData = m_vCallback[0];
                reqData.reqSleep -= fFrame;
                if (reqData.reqSleep > 0) return;
                if(reqData.request == null)
                {
                    reqData.request = UnityWebRequest.Get(reqData.url);
                    reqData.request.SendWebRequest();
                }

                if (reqData.request.isDone)
                {
                    if (!string.IsNullOrEmpty(reqData.request.error) || (reqData.request.downloadedBytes <= 0))
                    {
                        if (!string.IsNullOrEmpty(reqData.request.error))
                        {
                            Debug.LogWarning("http request error:[" + reqData.url + "] error:" + reqData.request.error);
                            //         reqData.onCallback("Fail");
                        }
                        else
                        {
                            Debug.LogWarning("can't not request http:" + reqData.url);
                            //        reqData.onCallback("Fail");
                        }
                        reqData.request.Dispose();
                        reqData.request = null;
                        reqData.reqSleep = 1;
                        reqData.repeatTimes--;
                    }
                    else
                    {
                        OnServerList(reqData.request.downloadHandler.text);
                        if (reqData.onCallback != null)
                            reqData.onCallback(reqData.request.downloadHandler.text);
                        reqData.request.Dispose();
                        m_vCallback.RemoveAt(0);
                    }
                }
                if (reqData.repeatTimes<=0)
                {
                    if(reqData.onCallback!=null)
                        reqData.onCallback("Fail");
                    reqData.request.Dispose();
                    m_vCallback.RemoveAt(0);
                }
            }
        }
        //------------------------------------------------------
        public void ResetReceiveMessageTime(float fLastReceiveMessageTime)
        {
            m_fLastSendMessageTime = 0;
        }
        //------------------------------------------------------
        public float GetMessageInterval()
        {
            if (m_fLastSendMessageTime == 0)
                return 0;
            else
            {
                //Framework.Plugin.Logger.Error("间隔时间:" + (Time.unscaledTime - m_fLastSendMessageTime));
                return Time.unscaledTime - m_fLastSendMessageTime;
            }
        }
        //------------------------------------------------------
        public void SetSeed(uint nSeed)
        {
            if (m_pSession != null) m_pSession.SetSeed(nSeed);
        }
        //------------------------------------------------------
        public List<SvrItem> GetServerList()
        {
            return m_ServerList;
        }
        //------------------------------------------------------
        void OnServerList(string strValue)
        {
            if(strValue.CompareTo("Fail") == 0)
            {
                EditorUtility.DisplayDialog("提示", "请求服务器列表失败", "Ok");
            }
            try
            {
                string[] arr = strValue.Split('|');

                if (arr.Length >= 1)
                {
                    strValue = "{ \"list\": " + arr[0] + "}";
                    Server studentList = JsonUtility.FromJson<Server>(strValue);
                    m_ServerList = studentList.list;
                }
            }
            catch
            {
                EditorUtility.DisplayDialog("提示", "解析失败", "Ok");
            }

        }

        public void OnReConnect(Framework.Core.VariablePoolAble talkeData= null)
        {
            if(GameInstance.getInstance()!=null) GameInstance.getInstance().OnReConnect(talkeData);
        }
    }
}