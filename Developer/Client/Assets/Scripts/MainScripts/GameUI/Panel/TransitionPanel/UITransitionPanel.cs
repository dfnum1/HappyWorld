/********************************************************************
生成日期:	18:1:2022   11:10
类    名: 	TransitionPanel
作    者:	hjc
描    述:	战斗过渡界面
*********************************************************************/

using TopGame.Core;
using UnityEngine;

namespace TopGame.UI
{
    [Framework.Plugin.AT.ATExportMono("UI系统/TransitionLoading")]
    [UI((ushort)EUIType.TransitionPanel, UI.EUIAttr.UI)]
    public class TransitionPanel : AUILoading
    {
        TransitionView m_view;
        Logic.EMode m_eMode = Logic.EMode.None;
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            m_view = m_pView as TransitionView;
        }
        //------------------------------------------------------
        public override void Hide()
        {
            base.Hide();
            RectTransform LoadingFrm = GetWidget<RectTransform>("LoadingFrm");
            if (LoadingFrm) LoadingFrm.gameObject.SetActive(false);
             m_eMode = Logic.EMode.None;
        }
        //------------------------------------------------------
        public override void Show()
        {
            base.Show();
            RectTransform LoadingFrm = GetWidget<RectTransform>("LoadingFrm");
            if (LoadingFrm) LoadingFrm.gameObject.SetActive(true);
        }
        //------------------------------------------------------
        public bool Show(Logic.EMode mode)
        {
            m_eMode = mode;
            Show();
            return true;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public Logic.EMode GetMode()
        {
            return m_eMode;
        }
        //------------------------------------------------------
        public override void SetBGTexture(string strTexturePath)
        {
            if (m_view != null) m_view.SetBGTexture(strTexturePath);
        }
        //------------------------------------------------------
        public override void SetGBAlpha(float fAlpha)
        {
            if (m_view != null) m_view.SetGBAlpha(fAlpha);
        }
        //------------------------------------------------------
        public override void SyncBGTexture(Texture pTexture)
        {
            if (m_view != null) m_view.SyncBGTexture(pTexture);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public void SetSprite(Sprite pSprite)
        {
            if (m_view != null) m_view.SetSprite(pSprite);
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
            m_view = null;
        }
    }
}
