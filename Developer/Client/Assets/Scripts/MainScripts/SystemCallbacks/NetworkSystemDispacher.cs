/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GameInstance
作    者:	HappLI
描    述:	游戏主线程
*********************************************************************/
using TopGame.Core;
using TopGame.Data;
using TopGame.Logic;
using UnityEngine;
using TopGame.Net;
using System;
using Framework.Plugin.Guide;
using TopGame.UI;
using TopGame.SvrData;
using Proto3;

namespace TopGame
{
    public partial class GameInstance
    {
        float m_SendHeartTimer;
        const float m_SendHeartInterval = 30f;
        const float m_SendHeartInterval_Battle = 30f;
        bool m_bChangeAccountDisconnected = false;
        float m_IntervalTime = 0;
        /// <summary>
        /// 显示等待界面时间
        /// </summary>
        float m_ShowWaitTime = 6f;
        /// <summary>
        /// 等待界面展示持续时间
        /// </summary>
        float m_WaitTime = 8f;
        int m_LastReachablility = -1;
        //------------------------------------------------------
        public void Req_Login(bool isMandatory, bool isReconect = false, string strUsr = null, string strPassword = null)
        {
//             Logic.Login.SetLoginFlag(false);
//             if(string.IsNullOrEmpty(strUsr)) strUsr = Data.LocalAccountCatch.GetAccount();
//             if(string.IsNullOrEmpty(strPassword)) strPassword = Data.LocalAccountCatch.GetPassword();
//             if(DebugConfig.AutoAccountRegister && string.IsNullOrEmpty(strUsr) && string.IsNullOrEmpty(strPassword))
//                 LoginHandler.Req_Login(Proto3.LoginType.FastLogin, strUsr, strPassword, isMandatory, isReconect);
//             else
//                 LoginHandler.Req_Login(Proto3.LoginType.Account, strUsr, strPassword, isMandatory, isReconect);
        }
        //------------------------------------------------------
        public void OnChangeAccount()
        {
            if(IsOffline) SvrData.LocalServerData.SaveUserData();
        }
        //------------------------------------------------------
        public void OnLogin(string accountId, string userName, string password, bool bOffline =false, string token =null, string loginSDK = null)
        {
            this.IsOffline = bOffline;
            if(bOffline)
            {
                if (string.IsNullOrEmpty(userName))
                {
                    UI.TipsUtil.ShowCommonTip(ETipType.Yes, 90000072);
                    return;
                }
                Data.LocalAccountCatch.SetAccount(accountId);
                Data.LocalAccountCatch.SetUserName(userName);
                ChangeLocation(ELocationState.Pve, Base.ELoadingType.Loading);
            }
            else
            {
                if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(userName))
                {
                    UI.TipsUtil.ShowCommonTip(ETipType.Yes, 90000010);
                    return;
                }
                //   Net.NetLoginHandler.ReqLoginVerify(accountId, userName, password, token, loginSDK);
            }
        }
        //------------------------------------------------------
        public override void OnReConnect(Framework.Plugin.AT.IUserData takeData = null)
        {
        }
        //------------------------------------------------------
        public override void OnSessionState(AServerSession session, ESessionState state)
        {
            if (state == ESessionState.Disconnected)
            {
                UIManager.HideUI(EUIType.WaitPanel);
            }
        }
        //------------------------------------------------------
        public override void OnCheckNetPackage(int msgCode, bool bRevice, Framework.Plugin.AT.IUserData pParam = null)
        {
            //发送时开始计时
            //接收到对应消息后,结束计时
            if (bRevice)
            {
                Framework.Plugin.Guide.GuideWrapper.OnCustomCallback((int)Framework.Plugin.Guide.EGuideCustomType.NetCallback, msgCode);

                //    if (pParam != null)
                //        Framework.Plugin.AT.AgentTreeManager.getInstance().ExecuteEvent((ushort)Base.EATEventType.TCPCallback, msgCode, pParam);

                m_IntervalTime = 0;

                if (GuideSystem.getInstance().bGuideLogEnable)
                {
                    Debug.Log("最后收到消息 msgCode:" + (MID)msgCode);
                }
                
            }
            else
            {
                bool isLock = NetWork.getInstance().IsMsgLock(msgCode);
                if (isLock)
                {
                    m_IntervalTime = Time.realtimeSinceStartup;
                }

                if (GuideSystem.getInstance().bGuideLogEnable)
                {
                    Debug.Log("最后发送消息 msgCode:" + (MID)msgCode);
                }
                
            }
        }
        //------------------------------------------------------
        public void OnNetWorkUpdate()
        {
            //联网状态判断
            if (m_LastReachablility == -1)
            {
                m_LastReachablility = (int)Application.internetReachability;
            }
            else
            {
                if (GetState() > EGameState.Logo)
                {
                    int net = (int)Application.internetReachability;
                    if (m_LastReachablility != net)
                    {
                        if (net == (int)NetworkReachability.NotReachable)
                        {
                            UIManager.ShowUI(EUIType.WaitPanel);
                        }
                        else if (net == (int)NetworkReachability.ReachableViaCarrierDataNetwork || net == (int)NetworkReachability.ReachableViaLocalAreaNetwork)
                        {
                            UIManager.HideUI(EUIType.WaitPanel);
                        }
                        m_LastReachablility = net;
                    }
                }
            }

            //断网或者登陆状态过滤
            if (GetState() <= EGameState.Logo || m_LastReachablility == (int)NetworkReachability.NotReachable)
                return;

            ////战斗状态不显示弹窗
            
            if (GetState() == EGameState.Battle && m_pBattleWorld != null && m_pBattleWorld is Framework.BattlePlus.BattleWorld)
            {
                if (((Framework.BattlePlus.BattleWorld)m_pBattleWorld).IsStarting())
                {
                    return;
                }
            }

            if (m_IntervalTime == 0)
            {
                UIManager.HideUI(EUIType.WaitPanel);
                return;
            }

            //发送等待回包间隔判断
            float time = Time.realtimeSinceStartup - m_IntervalTime;

            //if (m_IntervalTime > 0)
            //{
            //    Debug.Log($"intervalTime:{time},state:{pSession.GetState()}");
            //}

            if (time > m_ShowWaitTime && !UIManager.IsShowUI(EUIType.WaitPanel) && time < (m_ShowWaitTime + m_WaitTime))
            {
                UIManager.ShowUI(EUIType.WaitPanel);

                if (Framework.Plugin.Guide.GuideSystem.getInstance().bDoing)//引导状态下断网,关闭引导
                {
                    GuideSystem.getInstance().OverGuide();
                }
            }
            else if (time > (m_ShowWaitTime + m_WaitTime))
            {
                UIManager.HideUI(EUIType.WaitPanel);
                var battleState = ((Framework.BattlePlus.BattleWorld)m_pBattleWorld).GetStatus();
                uint tips = 80010238;
                if (battleState >=  Framework.BattlePlus.EBattleResultStatus.Victory)
                {
                    tips = 90000005;
                }
                TipsUtil.ShowCommonTip(ETipType.Yes, tips, OnNetwotError);
            }

        }
        //------------------------------------------------------
        private void OnNetwotError(ETipAction action)
        {
            m_IntervalTime = 0;
       //     NetLoginHandler.ReqLogout();
        }

        //------------------------------------------------------
        void SendHeart()
        {
            if(Net.NetWork.getInstance()!=null) Net.NetWork.getInstance().SendHeart(m_SendHeartInterval);
        }
    }
}
