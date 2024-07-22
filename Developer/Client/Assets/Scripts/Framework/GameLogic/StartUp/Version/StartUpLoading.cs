/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	StartUpUpdateUI
作    者:	HappLI
描    述:	版本检测界面
*********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Net;
using UnityEngine;
using UnityEngine.UI;
using TopGame.Base;
using UnityEngine.Video;
using Framework.Core;

namespace TopGame.UI
{
    public class StartUpLoading
    {
        long m_lTotalProgress = 0;
        long m_lInnerToPogress = 0;
        long m_lExternProgress = 0;

        float m_fToProgress = 0;
        float m_fProgress = 0;
        float m_fWatingTime = 0;
        bool m_bShowTextTips = false;
        bool m_bVisible = false;
        static bool ms_bAutoHide = true;
        public void Show()
        {
            if (m_bVisible) return;
            m_bVisible = true;
            m_lInnerToPogress = 0;
            m_lTotalProgress = 0;
            m_lExternProgress = 0;
            m_fToProgress = 0;
            m_fProgress = 0;
            m_fWatingTime = 0;
            m_bShowTextTips = false;
            ms_bAutoHide = true;

            UI.StartUpUpdateUI.Show();

            UI.StartUpUpdateUI.OnUpdate = InnerUpdate;
        }
        //-------------------------------------------------
        public void Hide()
        {
            if (!m_bVisible) return;
            m_bVisible = false;
            UI.StartUpUpdateUI.Hide();
            UI.StartUpUpdateUI.OnUpdate = null;
            ms_bAutoHide = true;
        }
        //------------------------------------------------------
        public static void SetAutoHide(bool active)
        {
            ms_bAutoHide = active;
        }
        //-------------------------------------------------
        public void ShowLoadTexts(bool bShow)
        {
            m_bShowTextTips = bShow;
        }
        //-------------------------------------------------
        public void ShowBG(bool bShow)
        {
            UI.StartUpUpdateUI.ShowBG(true);
        }
        //-------------------------------------------------
        public void ShowProgressBar(bool bShow)
        {
            UI.StartUpUpdateUI.ShowProgressBar(true);
        }
        //-------------------------------------------------
        public void SetProgressTip(int id, TextAnchor anchor)
        {
            UI.StartUpUpdateUI.SetProgressTip(id, anchor);
        }
        //-------------------------------------------------
        public void StopScrollingText()
        {
            UI.StartUpUpdateUI.StopScrollingText();
        }
        //-------------------------------------------------
        internal void SetExternProgress(long externProgress)
        {
            m_lExternProgress = externProgress;
        }
        //-------------------------------------------------
        public void SetToProgress(long toProgress)
        {
            m_lInnerToPogress = toProgress;
        }
        //-------------------------------------------------
        public void SetTotalProgress(long totalProgress)
        {
            if (m_lTotalProgress < totalProgress)
            {
                m_lTotalProgress = totalProgress;
            }
            UI.StartUpUpdateUI.SetTotalSize(m_lTotalProgress, m_bShowTextTips);
        }
        //-------------------------------------------------
        protected void UpdateToProgress()
        {
            m_fToProgress = Mathf.Max(m_fToProgress, m_lInnerToPogress+ m_lExternProgress);
        }
        //-------------------------------------------------
        void InnerUpdate(float fFrame)
        {
            if (!m_bVisible) return;
            UpdateToProgress();
            if(m_lTotalProgress>0)
                m_fProgress = Mathf.Lerp(m_fProgress, m_fToProgress / m_lTotalProgress, Time.fixedDeltaTime * 2.5f);
            else
                m_fProgress = Mathf.Lerp(m_fProgress, m_fToProgress, Time.fixedDeltaTime * 2.5f);

            if (m_fWatingTime <= 0 && m_fProgress >= Base.GlobalUtil.PROGRESS_END_SNAP)
            {
                m_fWatingTime = 0.5f;
            }

            if (m_fWatingTime > 0 && ms_bAutoHide)
            {
                m_fWatingTime -= fFrame;
                if (m_fWatingTime <= 0)
                    m_fWatingTime = 0;
            }
            UI.StartUpUpdateUI.SetCurLoadSize((long)(m_fProgress * m_lTotalProgress), m_bShowTextTips);
            if (m_fProgress >= Base.GlobalUtil.PROGRESS_END_SNAP && m_fWatingTime <= 0)
                Hide();
        }
    }
}
