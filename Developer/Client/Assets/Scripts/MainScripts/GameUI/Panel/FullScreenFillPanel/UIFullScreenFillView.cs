/********************************************************************
生成日期:	2020-6-16
类    名: 	UIFullScreenFillView
作    者:	zdq
描    述:	UIFullScreenFillView界面
*********************************************************************/

using Framework.Plugin.AT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    [ATExportMono("UI系统/全屏填充界面/视图")]
    [UI((ushort)EUIType.FullScreenFillPanel, UI.EUIAttr.View)]
    public class UIFullScreenFillView : UIView
    {
        private UIFullScreenFillPanel m_pUI;
        private Image m_BG;

        //------------------------------------------------------
        public override void Init(UIBase pBase)
        {
            base.Init(pBase);
            m_pUI = pBase as UIFullScreenFillPanel;
        }
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            if (m_pUI != null && m_pUI.ui != null)
            {
                m_BG = m_pUI.ui.GetWidget<Image>("BG");
                //m_pUI.ui.Find("");
            }
        }
        //------------------------------------------------------
        public override void Clear(bool bUnloadDynamic = true)
        {
            base.Clear(bUnloadDynamic);
        }
        //------------------------------------------------------
        
        internal void SetFillColor(Color color)
        {
            if (m_BG)
            {
                m_BG.color = color;
            }
        }
        //------------------------------------------------------
        public void SetAlpha(float alpha)
        {
            if (m_BG)
            {
                m_BG.color = new Color(m_BG.color.r, m_BG.color.g, m_BG.color.b,alpha);
            }
        }
        //------------------------------------------------------
        public void CrossFadeAlpha(float a,float duration,bool ignoreScaleTime = true)
        {
            if (m_BG)
                m_BG.CrossFadeAlpha(a, duration, ignoreScaleTime);
        }
    }
}
