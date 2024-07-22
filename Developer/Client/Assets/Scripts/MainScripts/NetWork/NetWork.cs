/********************************************************************
生成日期:	10:7:2019   11:49
类    名: 	NetWork
作    者:	HappLI
描    述:	网络消息
*********************************************************************/


using System;
using UnityEngine;
using TopGame.UI;
using System.Collections.Generic;
using TopGame.SvrData;

namespace TopGame.Net
{
    [Framework.Plugin.AT.ATExportMono("网络模块", Framework.Plugin.AT.EGlobalType.Single)]
    public partial class NetWork : Framework.Module.AModule, INetWork, Framework.Module.IStart, Framework.Module.ILateUpdate, ISessionCallback
    {
        struct TimerLockMid
        {
            public int mid;
            public int lastTime;
        }
        private static NetWork ms_Instance = null;
        const int REC_MAXBUFFERSIZE = 1024 * 1024*4;//4 Mb
        const int SND_MAXBUFFERSIZE = 1024 * 1024 * 2;//2 Mb

        System.Threading.Thread m_pReciveThread = null;
        System.Threading.Thread m_pSendThread = null;

        NetHandler m_pHandlers = null;
        List<AServerSession>[] m_Sessions = new List<AServerSession>[(int)ESessionType.Count];
    
        float m_fLastSendMessageTime = 0;
        Proto3.HeartRequest m_HeartRequest = new Proto3.HeartRequest();

        private HashSet<int> m_vLockMsgs = new HashSet<int>();
        private Dictionary<int, long> m_vTimerLockMsgs = new Dictionary<int, long>();

        //------------------------------------------------------
        public static NetWork getInstance()
        {
            if (ms_Instance == null)
            {
                Framework.Plugin.Logger.Break("请先注册网络模块");
            }
            return ms_Instance;
        }
        //------------------------------------------------------
        protected override void Awake()
        {
            ms_Instance = this;
            for (int i = 0; i < m_Sessions.Length; ++i)
                m_Sessions[i] = new List<AServerSession>();
            m_pHandlers = new NetHandler(this);
            WebHandler.SetPosCallback(OnTextPackage);
            NetHandlerRegister.Init(m_pHandlers);
            m_fLastSendMessageTime = 0;
            m_vTimerLockMsgs.Clear();
        }
        //------------------------------------------------------
        public void Start()
        {
            m_fLastSendMessageTime = 0;
            m_vLockMsgs.Clear();
            m_vTimerLockMsgs.Clear();
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            Disconnect(ESessionType.Count);
            m_vLockMsgs.Clear();
        }
        //------------------------------------------------------
        AServerSession CreateSession(ESessionType type)
        {
            switch (type)
            {
                case ESessionType.Kcp:
                    {
                        KcpServerSession pKcpSession = new KcpServerSession(this, REC_MAXBUFFERSIZE, SND_MAXBUFFERSIZE, 64, 10, m_Sessions[(int)type].Count);
                        pKcpSession.Register(this);
                        pKcpSession.SetConnectTimeout(10000);
                        m_Sessions[(int)type].Add(pKcpSession);
                        return pKcpSession;
                    }
                case ESessionType.Tcp:
                    {

                        TcpServerSession pTcpSession = new TcpServerSession(this, REC_MAXBUFFERSIZE, SND_MAXBUFFERSIZE, 64, 10, m_Sessions[(int)type].Count);
                        pTcpSession.Register(this);
                        pTcpSession.SetConnectTimeout(10000);
                        m_Sessions[(int)type].Add(pTcpSession);
                        return pTcpSession;
                    }
            }
            return null;
        }
        //------------------------------------------------------
        public AServerSession GetSession(string ip, int port)
        {
            for (int i = 0; i < m_Sessions.Length; ++i)
            {
                for (int j = 0; j < m_Sessions[i].Count; ++j)
                {
                    var session = m_Sessions[i][j];
                    if (session.GetPort() == port && session.GetIp().CompareTo(ip) == 0)
                        return session;
                }
            }
            return null;
        }
        //------------------------------------------------------
        public AServerSession GetSession(string strAddress)
        {
            for (int i = 0; i < m_Sessions.Length; ++i)
            {
                for (int j = 0; j < m_Sessions[i].Count; ++j)
                {
                    var session = m_Sessions[i][j];
                    if (session.GetAddress().CompareTo(strAddress) == 0)
                        return session;
                }
            }
            return null;
        }
        //------------------------------------------------------
        public AServerSession GetSession(ESessionType type, int index=0)
        {
            int tpIndex = (int)type;
            if (tpIndex < 0 || tpIndex >= m_Sessions.Length) return null;
            if (index < 0 || m_Sessions[tpIndex] ==null || index >= m_Sessions[tpIndex].Count) return null;
            return m_Sessions[tpIndex][index];
        }
        //------------------------------------------------------
        public void RegisterSessionCallback(ISessionCallback pCallback, int index=-1)
        {
            if(index < 0)
            {
                List<AServerSession> sessions;
                for (int i = 0; i < m_Sessions.Length; ++i)
                {
                    sessions = m_Sessions[i];
                    if (sessions == null) continue;
                    for(int j =0; j < sessions.Count; ++j)
                        sessions[j].Register(pCallback);
                }
            }
            else
            {
                for (int i = 0; i < m_Sessions.Length; ++i)
                {
                    if (index >= 0 && index < m_Sessions[i].Count)
                    {
                        m_Sessions[i][index].Register(pCallback);
                    }
                }
            }

        }
        //------------------------------------------------------
        public void UnRegisterSessionCallback(ISessionCallback pCallback, int index = -1)
        {
            if (index < 0)
            {
                List<AServerSession> sessions;
                for (int i = 0; i < m_Sessions.Length; ++i)
                {
                    sessions = m_Sessions[i];
                    if (sessions == null) continue;
                    for (int j = 0; j < sessions.Count; ++j)
                        sessions[j].UnRegister(pCallback);
                }
            }
            else
            {
                for (int i = 0; i < m_Sessions.Length; ++i)
                {
                    if (index >= 0 && index < m_Sessions[i].Count)
                    {
                        m_Sessions[i][index].UnRegister(pCallback);
                    }
                }
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public AServerSession Connect(ESessionType type,string strIp, int nPort, System.Action<AServerSession> onCallback = null, long localConnID =0)
        {
            if (string.IsNullOrEmpty(strIp) || strIp.CompareTo("0.0.0.0") == 0 || strIp.CompareTo("127.0.0.1") == 0)
            {
                var ipAdress = Framework.Base.NetworkHelper.GetHostAddress(System.Net.Dns.GetHostName());
                strIp = ipAdress.ToString();
            }

            m_vLockMsgs.Clear();
            var session = GetSession(strIp, nPort);
            if (session == null) session = CreateSession(type);
            session.SetLocalConnID(localConnID);
            if (session != null) session.Connect(strIp, nPort, onCallback);
            return session;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("ConnectAddress")]
        public AServerSession Connect(ESessionType type, string strAddress, System.Action<AServerSession> onCallback = null, long localConnID = 0)
        {
            if (string.IsNullOrEmpty(strAddress))
            {
                if (onCallback != null) onCallback(null);
                Debug.LogError("address is null !");
                return null;
            }
            int ipPort = strAddress.LastIndexOf(":");
            if (ipPort <= 0)
            {
                if (onCallback != null) onCallback(null);
                Debug.LogError(strAddress + " address parser error, not port!");
                return null;
            }
            int port = 0;
            if (!int.TryParse(strAddress.Substring(ipPort + 1, strAddress.Length - ipPort - 1), out port))
            {
                if (onCallback != null) onCallback(null);
                Debug.LogError(strAddress + " address port invalid");
                return null;
            }
            string ip = strAddress.Substring(0, ipPort);
            return Connect(type, ip, port, onCallback, localConnID);
        }
        //------------------------------------------------------
        public void Disconnect(ESessionType type,int index =0)
        {
            if(type == ESessionType.Count)
            {
                for(int i =0; i < m_Sessions.Length; ++i)
                {
                    for (int j = 0; j < m_Sessions[i].Count; ++j)
                        m_Sessions[i][j].Disconnect();
                    m_Sessions[i].Clear();
                }
            }
            else
            {
                if(index<0)
                {
                    for (int i = 0; i < m_Sessions[(int)type].Count; ++i)
                    {
                        m_Sessions[(int)type][i].Disconnect();
                    }
                    m_Sessions[(int)type].Clear();
                }
                else if(index>=0 && index< m_Sessions[(int)type].Count)
                {
                    m_Sessions[(int)type][index].Disconnect();
                }
            }
            m_vLockMsgs.Clear();
            m_vTimerLockMsgs.Clear();
        }
        //------------------------------------------------------
        public void ReqHttp(string url, System.Action<string> onCallback, byte nRepeat = 6, int timeout = 3)
        {
            WebHandler.ReqHttp(url, onCallback, nRepeat, timeout);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("发送消息包")]
        public bool SendMessage(PacketBase pMsg)
        {
            if (pMsg.GetCode() <= 0) return false;
            if (IsTimerLock(pMsg.GetCode())) return false;
            var session = GetSession(pMsg.GetSessionType(), pMsg.GetSessionIndex());
            if (session == null) return false;
            bool bSend = session.Send(pMsg);
            if (bSend)
            {
                m_fLastSendMessageTime = Framework.Base.TimeHelper.ClientNow();
            }
            LockPacket(pMsg.GetCode());
            if (GetFramework() != null)
            {
                TopGame.Core.GameModule pMoudle = GetFramework() as TopGame.Core.GameModule;
                if (pMoudle != null) pMoudle.OnCheckNetPackage(pMsg.GetCode(), false);
            }
            return bSend;
        }
        //------------------------------------------------------
        public bool SendMessage(ESessionType type, int nCode, Google.Protobuf.IMessage pMsg,bool isCheckNet = true)
        {
            var session = GetSession(type);
            if (session == null) return false;
            if (IsTimerLock(nCode)) return false;
            bool bSend = session.Send(nCode, pMsg);
            if (bSend && isCheckNet)
            {
                m_fLastSendMessageTime = Framework.Base.TimeHelper.ClientNow();
            }
            LockPacket(nCode);
            if (GetFramework() != null)
            {
                TopGame.Core.GameModule pMoudle = GetFramework() as TopGame.Core.GameModule;
                if (pMoudle != null) pMoudle.OnCheckNetPackage(nCode, false);
            }
            return bSend;
        }
        //------------------------------------------------------
        public void SendMessage(int nCode, Google.Protobuf.IMessage pMsg, bool isCheckNet = true)
        {
            if (IsTimerLock(nCode)) return;
            var session = GetSession(ESessionType.Tcp);
            if (session == null) session = GetSession(ESessionType.Kcp);
            if (session == null) return;
            if (session.Send(nCode, pMsg) && isCheckNet)
            {
                m_fLastSendMessageTime = Framework.Base.TimeHelper.ClientNow();
            }
            LockPacket(nCode);
            if (GetFramework() != null)
            {
                TopGame.Core.GameModule pMoudle = GetFramework() as TopGame.Core.GameModule;
                if (pMoudle != null) pMoudle.OnCheckNetPackage(nCode, false);
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("发送心跳包")]
        public void SendHeart(float fHeartGap)
        {
            //if (GetMessageInterval() < fHeartGap) return;
            var session = GetSession(ESessionType.Tcp);
            if (session !=null)
            {
                if (session.GetState() == ESessionState.Connected)
                {
                    SendMessage(ESessionType.Tcp, (int)Proto3.MID.HeartReq, m_HeartRequest);
                }
            }
            session = GetSession(ESessionType.Kcp);
            if (session != null)
            {
                if (session.GetState() == ESessionState.Connected)
                {
                    SendMessage(ESessionType.Kcp, (int)Proto3.MID.HeartReq, m_HeartRequest);
                }
            }
        }
        //------------------------------------------------------
        public void OnSessionState(AServerSession session, ESessionState eState)
        {
            Framework.Plugin.Logger.Info(eState.ToString());
            if (GetFramework() != null)
            {
                TopGame.Core.GameModule pMoudle = GetFramework() as TopGame.Core.GameModule;
                if (pMoudle != null) pMoudle.OnSessionState(session, eState);
            }
            if (eState == ESessionState.Connected)
            {
                if(m_pSendThread==null || !m_pSendThread.IsAlive)
                {
                    m_pSendThread = new System.Threading.Thread(OnProgressSend);
                    m_pSendThread.Start();
                }
                if(m_pReciveThread == null || !m_pReciveThread.IsAlive)
                {
                    m_pReciveThread = new System.Threading.Thread(OnProgressRecive);
                    m_pReciveThread.Start();
                }
            }
            else if (eState == ESessionState.Disconnected)
            {
                if(!HasSessionConnected())
                {
                    if (m_pSendThread != null) m_pSendThread.Abort();
                    m_pSendThread = null;
                    if (m_pReciveThread != null) m_pReciveThread.Abort();
                    m_pReciveThread = null;
                }
            }
        }
        //------------------------------------------------------
        bool HasSessionConnected()
        {
            for (int i = 0; i < m_Sessions.Length; ++i)
            {
                for (int j = 0; j < m_Sessions[i].Count; ++j)
                {
                    if (m_Sessions[i][j] != null && m_Sessions[i][j].GetState() == ESessionState.Connected)
                        return true;
                }
            }
            return false;
        }
        //------------------------------------------------------
        public ESessionState GetSessionState(ESessionType type,int index=0)
        {
            var session = GetSession(type,index);
            if (session == null) return ESessionState.None;
            return session.GetState();
        }
        //------------------------------------------------------
        public bool IsRegisterHandler(int nCode)
        {
            return true;
        }
        //------------------------------------------------------
        public void Register(int code, OnRevicePacketMsg onHandler)
        {
            m_pHandlers.Register(code, onHandler);
        }
        //------------------------------------------------------
        public void UnRegister(int code)
        {
            m_pHandlers.UnRegister(code);
        }
        //------------------------------------------------------
        public void OnPackage(PacketBase pMsg)
        {
            UnLockPacket(pMsg.GetCode());
            if(GetFramework()!=null)
            {
                TopGame.Core.GameModule pMoudle = GetFramework() as TopGame.Core.GameModule;
                if (pMoudle != null) pMoudle.OnCheckNetPackage(pMsg.GetCode(), true, pMsg);
            }
            m_pHandlers.OnPakckage(pMsg);
        }
        //------------------------------------------------------
        void OnTextPackage(string msgJson)
        {
            PacketBase pkg = MessagePool.ToPackage(msgJson);
            if (pkg.GetCode() > 0) OnPackage(pkg);
            else Debug.LogWarning("pb code is zero!");
        }
        //------------------------------------------------------
        public void LateUpdate(float fFrame)
        {
            for(int i =0; i < m_Sessions.Length; ++i)
            {
                for (int j =0; j < m_Sessions[i].Count; ++j)
                {
                    m_Sessions[i][j].OnMainUpdate(fFrame);
                }
            }

            
        }
        //------------------------------------------------------
        public void ResetReceiveMessageTime(float fReceiveMessageTime)
        { 
            m_fLastSendMessageTime = fReceiveMessageTime;
        }
        //------------------------------------------------------
        public float GetMessageInterval()
        {
            if (m_fLastSendMessageTime == 0)
                return 0;
            else
            {
                return Framework.Base.TimeHelper.ClientNow() - m_fLastSendMessageTime;
            }
        }
        //------------------------------------------------------
        public void SetSeed(uint nSeed)
        {
            for (int i = 0; i < m_Sessions.Length; ++i)
            {
                for(int j =0; j < m_Sessions[i].Count; ++j)
                    m_Sessions[i][j].SetSeed(nSeed);
            }
        }
        //------------------------------------------------------
        public void OnReConnect(Framework.Core.VariablePoolAble userData = null)
        {
            if (GetFramework() != null)
            {
                TopGame.Core.GameModule pMoudle = GetFramework() as TopGame.Core.GameModule;
                if (pMoudle != null) pMoudle.OnReConnect(userData);
            }
            m_vLockMsgs.Clear();
        }
        //------------------------------------------------------
        private void OnProgressRecive()
        {
            int job = 1;
            while (job > 0)
            {
                try
                {
                    job = 0;
                    for (int i = 0; i < m_Sessions.Length; ++i)
                    {
                        for (int j = 0; j < m_Sessions[i].Count; ++j)
                        {
                            if (m_Sessions[i][j].OnNetThreadUpdate(1))
                                job++;
                        }
                    }
                    if (job <= 0) break;
                }
                catch (Exception ex)
                {
                    job = 1;
                }
                System.Threading.Thread.Sleep(1);
            }
        }
        //------------------------------------------------------
        private void OnProgressSend()
        {
            int job = 1;
            while(job>0)
            {
                try
                {
                    job = 0;
                    for (int i = 0; i < m_Sessions.Length; ++i)
                    {
                        for (int j = 0; j < m_Sessions[i].Count; ++j)
                        {
                            if (m_Sessions[i][j] != null && m_Sessions[i][j].OnNetThreadUpdate(0))
                                job++;
                        }
                    }
                    if (job <= 0) break;
                }
                catch (Exception ex)
                {
                    job = 1;
                }

                System.Threading.Thread.Sleep(1);
            }
        }
        //------------------------------------------------------
        public bool IsMsgLock(int mid)
        {
            return m_vLockMsgs.Contains(mid);
        }
        //------------------------------------------------------
        public void SetUserID(long userID)
        {
            m_HeartRequest.UserID = userID;
        }
    }
}
