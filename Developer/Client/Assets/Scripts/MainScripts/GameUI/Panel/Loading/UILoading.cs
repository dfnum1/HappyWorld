/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UILoading
作    者:	HappLI
描    述:	加载界面
*********************************************************************/

using UnityEngine;

namespace TopGame.UI
{
    [Framework.Plugin.AT.ATExportMono("UI系统/Loading")]
    [UI((ushort)EUIType.Loading, UI.EUIAttr.UI)]
    public class UILoading : AUILoading
    {
        private UILoadingView m_view;
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            m_view = m_pView as UILoadingView;
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
        public override void Destroy()
        {
            base.Destroy();
            m_view = null;
        }
    }
}
