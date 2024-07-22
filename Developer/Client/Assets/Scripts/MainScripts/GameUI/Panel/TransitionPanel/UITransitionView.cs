/********************************************************************
生成日期:	18:1:2022   11:10
类    名: 	TransitionView
作    者:	hjc
描    述:	战斗过渡界面
*********************************************************************/

using TopGame.Base;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Framework.Core;

namespace TopGame.UI
{
    [UI((ushort)EUIType.TransitionPanel, UI.EUIAttr.View)]
    public class TransitionView : UIView
    {
        AUILoading m_pUI;
        Image m_pBack = null;
        Image m_pFront = null;
        private RawImage m_pBlackMask = null;
        UnityEngine.UI.RawImage m_pBG = null;
        Text m_Tips;
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            if (m_pBaseUI == null) return;
            m_pUI = m_pBaseUI as AUILoading;
            m_pBG = m_pUI.GetWidget<UnityEngine.UI.RawImage>("bg");
            m_pBlackMask = m_pUI.GetWidget<UnityEngine.UI.RawImage>("BlackMask");
            m_pBack = m_pUI.GetWidget<Image>("back");
            m_pFront = m_pUI.GetWidget<Image>("front");
            m_Tips = m_pUI.GetWidget<Text>("tips");
        }
        //------------------------------------------------------
        public override void Show()
        {
            base.Show();
            RtgTween.Tweener tweener = m_pUI.GetWidget<RtgTween.Tweener>("back"); 
            if (m_pBack != null) m_pBack.color = Color.white;
            if (tweener) tweener.ForcePlayTween();
        }
        //------------------------------------------------------
        public override void Hide()
        {
            base.Hide();
            SetSprite(null);
        }
        //------------------------------------------------------
        public void SetBGTexture(string strTexturePath)
        {
            if (m_pBG && m_pBlackMask)
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
        public void SetGBAlpha(float fAlpha)
        {
            fAlpha = Mathf.Clamp01(fAlpha);
            if (m_pBG)
            {
                Color color = m_pBG.color;
                color.a = fAlpha;
                m_pBG.color = color;
            }
            if (m_pBlackMask)
            {
                Color color = m_pBlackMask.color;
                color.a = fAlpha;
                m_pBlackMask.color = color;
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public void SetSprite(Sprite pSprite)
        {
            if (m_pFront)
            {
                m_pFront.enabled = pSprite != null;
                m_pFront.sprite = pSprite;
            }
        }
        //------------------------------------------------------
        public override void Update(float fFrame)
        {
            if (m_pBack != null && m_pUI!=null)
            {
                m_pBack.fillAmount = m_pUI.Progress;
            }
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
            m_pBlackMask = null;
            m_pBG = null;
            m_Tips = null;
            m_pBack = null;
            m_pFront = null;
        }
    }
}
