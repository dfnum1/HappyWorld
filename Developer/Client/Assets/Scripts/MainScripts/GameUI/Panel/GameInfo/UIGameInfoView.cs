#if UNITY_EDITOR
#define USE_SHOWFPS
#endif
/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIBase
作    者:	HappLI
描    述:	UI管理器
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using TopGame.Base;
using TopGame.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    [UI((ushort)EUIType.GameInfo, UI.EUIAttr.View)]
    public class UIGameInfoView : UIView
    {
        UIBase m_pUI = null;
        UnityEngine.UI.Text m_pFps = null;
        private Text m_version;
        int m_nFps;
        //------------------------------------------------------
        public override void Init(UIBase pBase)
        {
            base.Init(pBase);
            m_pUI = pBase;
            Base.FpsStat.OnFps = OnFps;
        }
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            if (m_pUI == null) return;
            if (m_pUI.ui)
            {
#if USE_SHOWFPS
                m_pFps = m_pUI.ui.GetWidget<UnityEngine.UI.Text>("fps");
                m_pFps.color = Color.white;
                m_pFps.fontSize = 24;
#else
                m_pFps = m_pUI.ui.GetWidget<UnityEngine.UI.Text>("fps");
                if (m_pFps)
                {
                    m_pFps.gameObject.SetActive(false);
                    m_pFps.text = "";
                }
#endif
                m_version = m_pUI.ui.GetWidget<UnityEngine.UI.Text>("version");
                if (m_version)
                {
                    UIUtil.SetLabel(m_version, VersionData.version);
                }
            }
        }
        //------------------------------------------------------
        void OnFps(float fps)
        {
#if USE_SHOWFPS
            if (m_pFps == null) return;
            int fps_temp = Mathf.CeilToInt(fps);
            if (m_nFps != fps_temp)
            {
                m_nFps = fps_temp;
                UIUtil.SetLabel(m_pFps, m_nFps.ToString());
            }
#endif
        }
    }
}
