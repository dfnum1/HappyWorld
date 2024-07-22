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
using TopGame.UI;
using Framework.Core;

namespace TopGame.Logic
{
    public class LogoSplash
    {
        bool m_bFadeinNext = false;
        float m_fReqSeverList = 0.5f;
        long m_nReqMemoryCheck = -1;
        float m_CheckMemoryDelay = 0.25f;
        bool m_bActive = false;
        FrameworkStartUp m_pStartUp = null;
        //------------------------------------------------------
        public LogoSplash(FrameworkStartUp startUp)
        {
            m_bActive = true;
            m_bFadeinNext = false;
            m_fReqSeverList = 0.5f;
            m_CheckMemoryDelay = 0.25f;
            m_pStartUp = startUp;
        }
        //------------------------------------------------------
        public void Start()
        {
            m_bActive = true;
            m_bFadeinNext = false;
            m_fReqSeverList = 0.5f;
            m_CheckMemoryDelay = 0.25f;
            CheckReqMemory();

            //! pre show version update panel
            UI.StartUpUpdateUI.Show();
            UI.StartUpUpdateUI.ShowBG(true);
            UI.StartUpUpdateUI.ShowProgressBar(false);
            UI.StartUpUpdateUI.StopScrollingText();
            UI.StartUpUpdateUI.SetProgressTip(80010229, TextAnchor.MiddleCenter);
            LogoUI.Play();
        }
        //------------------------------------------------------
        public void Shutdown()
        {

        }
        //------------------------------------------------------
        public void Exit()
        {
            m_bFadeinNext = false;
            m_fReqSeverList = 0.5f;
            m_nReqMemoryCheck = -1;
            m_CheckMemoryDelay = 0.25f;
        }
        //------------------------------------------------------
        public void Update(float fFrameTime)
        {
            if (!m_bActive) return;
            if (!m_bFadeinNext)
            {
                if (LogoUI.CanFadeInNext())
                {
                    m_bFadeinNext = true;

                    CheckReqMemory();
                }
            }
            else
            {
                if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2)
                {
                    TopGame.UI.UIPreTip.Show(null, 80010237);
                    return;
                }
                if(!VersionData.offline)
                {
                    if (Application.internetReachability == NetworkReachability.NotReachable)
                    {
                        TopGame.UI.UIPreTip.Show(ReqServerList, 80010201);
                        return;
                    }
                }
 
                if (m_CheckMemoryDelay > 0)
                {
                    m_CheckMemoryDelay -= fFrameTime;
                    if (m_CheckMemoryDelay <= 0)
                        CheckReqMemory();
                    return;
                }
                if (m_nReqMemoryCheck > 0 && m_nReqMemoryCheck < 83886080)  //<80mb
                {
                    TopGame.UI.UIPreTip.Show(ReCheckReqMemory, 80010236);
                    return;
                }
                if (m_fReqSeverList > 0)
                {
                    m_fReqSeverList -= Time.deltaTime;
                    if (m_fReqSeverList <= 0)
                    {
                        ReqServerList();
                    }
                }
            }
        }
        //------------------------------------------------------
        void ReqServerList()
        {
            SvrData.ServerList.ReqList(OnServerListBack);
        }
        //------------------------------------------------------
        void CheckReqMemory()
        {
            m_nReqMemoryCheck = JniPlugin.GetRemainMemory();
        }
        //------------------------------------------------------
        void ReCheckReqMemory()
        {
            m_nReqMemoryCheck = JniPlugin.GetRemainMemory();
            if (m_nReqMemoryCheck < 83886080) m_nReqMemoryCheck = -1;
        }
        //------------------------------------------------------
        public void OnServerListBack(string error, VariablePoolAble userParam)
        {
            if (VersionData.offline)
            {
                if (m_pStartUp != null)
                    m_pStartUp.SetSection(EStartUpSection.Version);
            }
            else
            {
                if (error.CompareTo("Fail") == 0)
                {
                    TopGame.UI.UIPreTip.Show(ReqServerList, 80010201);
                    return;
                }
                if (m_pStartUp != null)
                    m_pStartUp.SetSection(EStartUpSection.Version);
            }
        }
    }
}
