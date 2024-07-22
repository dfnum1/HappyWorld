/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIBase
作    者:	HappLI
描    述:	UI管理器
*********************************************************************/

using Framework.Core;
using System.Collections;
using System.Collections.Generic;
using TopGame.Base;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace TopGame.UI
{
    [UI((ushort)EUIType.Loading, UI.EUIAttr.View)]
    public class UILoadingView : UIView
    {
        UILoading m_pUI = null;

        UnityEngine.UI.Image m_pProgress = null;
        UnityEngine.UI.RawImage m_pBG = null;
        private RawImage m_pBlackMask;
        RectTransform m_Dog;
        Text m_Text;

        //------------------------------------------------------
        public override void Init(UIBase pBase)
        {
            base.Init(pBase);
            m_pUI = pBase as UILoading;
            if(m_pBG)
                m_pBG.texture = null;
        }
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            Debug.Log("机型:"+SystemInfo.deviceModel);
            if (m_pUI == null) return;
            if (m_pUI.ui)
            {
                m_pBG = m_pUI.ui.GetWidget<UnityEngine.UI.RawImage>("bg");
                m_pBlackMask = m_pUI.ui.GetWidget<UnityEngine.UI.RawImage>("BlackMask");
                m_pProgress = m_pUI.ui.GetWidget<UnityEngine.UI.Image>("handler");
                m_Dog = m_pUI.ui.GetWidget<UnityEngine.RectTransform>("Dog");
                m_Text = m_pUI.ui.GetWidget<Text>("Text");
            }
        }
        //------------------------------------------------------
        public override void Clear(bool bUnloadDynamic = true)
        {
            bUnloadDynamic = true;
            base.Clear(bUnloadDynamic);
        }
        //------------------------------------------------------
        public void SetBGTexture(string strTexturePath)
        {
            if(m_pBG && m_pBlackMask)
            {
                if (string.IsNullOrEmpty(strTexturePath))
                {
                    m_pBG.enabled = false;
                    m_pBlackMask.enabled = false;
                    return;
                }
                m_pBG.enabled = true;
                AssetOperiaon assetOp = LoadObjectAsset(m_pBG, strTexturePath, false, false);
                if (assetOp != null) assetOp.Refresh();
                m_pBlackMask.enabled = true;
            }
        }
        //------------------------------------------------------
        public void SyncBGTexture(Texture pTexture)
        {
            if (m_pBG && m_pBlackMask)
            {
                if (pTexture)
                {
                    m_pBG.enabled = true;
                    m_pBlackMask.enabled = true;
                }
                else
                {
                    m_pBG.enabled = false;
                    m_pBlackMask.enabled = false;
                }
                
                m_pBG.texture = pTexture;
            }
        }
        //------------------------------------------------------
        public void SetGBAlpha(float alpha)
        {
            alpha = Mathf.Clamp01(alpha);
            if (m_pBG)
            {
                Color color = m_pBG.color;
                color.a = alpha;
                m_pBG.color = color;
            }
            if (m_pBlackMask)
            {
                Color color = m_pBlackMask.color;
                color.a = alpha;
                m_pBlackMask.color = color;
            }
        }
        //------------------------------------------------------
        public override void Update(float fFrame)
        {
            if (m_pProgress != null&& m_Text)
            {
                m_pProgress.fillAmount = m_pUI.Progress;
                m_Text.text =Util.stringBuilder.Append((m_pProgress.fillAmount * 100).ToString("f0")).Append("%").ToString();
                if (m_Dog)
                {
                    m_Dog.anchoredPosition = new Vector2(m_pProgress.fillAmount * 590 - 280, m_Dog.anchoredPosition.y);
                }
            }
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
        }
    }
}
