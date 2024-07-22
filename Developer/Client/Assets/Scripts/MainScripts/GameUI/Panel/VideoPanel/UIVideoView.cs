/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIVideoView
作    者:	HappLI
描    述:	视频界面
*********************************************************************/

using UnityEngine;

namespace TopGame.UI
{
    [UI((ushort)EUIType.VideoPanel, UI.EUIAttr.View)]
    public class UIVideoView : UIView
    {
        UIVideo m_pUI = null;

        Color m_BlackColor = Color.black;
        UnityEngine.UI.RawImage m_pBG = null;
        UnityEngine.UI.RawImage m_pPrepareBG = null;
        //------------------------------------------------------
        public override void Init(UIBase pBase)
        {
            base.Init(pBase);
            m_pUI = pBase as UIVideo;
        }
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            if (m_pUI == null) return;
            if (m_pUI.ui)
            {
                m_pBG = m_pUI.ui.GetWidget<UnityEngine.UI.RawImage>("bg");
                m_pPrepareBG = m_pUI.ui.GetWidget<UnityEngine.UI.RawImage>("prepare");
            }
        }
        //------------------------------------------------------
        public void SetDefaultTexture(string strPath)
        {
            if (m_pBG)
            {
                LoadObjectAsset(m_pBG, strPath, false, false);
            }
        }
        //------------------------------------------------------
        public override void Clear(bool bUnloadDynamic = true)
        {
            base.Clear(true);
            if (m_pBG) m_pBG.texture = null;
            if (m_pPrepareBG) m_pPrepareBG.texture = null;
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
        }
        //------------------------------------------------------
        public void SetPrepareTexture(string strPath)
        {
            if (m_pPrepareBG)
            {
                LoadObjectAsset(m_pPrepareBG, strPath, false, false);
            }
        }
        //------------------------------------------------------
        public void ShowPrepare(bool bShow)
        {
            if (m_pPrepareBG) m_pPrepareBG.gameObject.SetActive(bShow);
        }
        //------------------------------------------------------
        public void SetPrepareAlpha(float toAlpha, float Duration = 1)
        {
            if (m_pPrepareBG)m_pPrepareBG.CrossFadeAlpha(toAlpha, Duration, true);
        }
        //------------------------------------------------------
        public void SyncTexture(Framework.Plugin.Media.IMediaPlayer player, float alphaFactor = 1)
        {
            if (m_pBG == null ) return;
            m_pBG.texture = player.GetTexture();
            if (m_pBG.texture == null)
                m_BlackColor.r = m_BlackColor.g = m_BlackColor.b = 0;
            else
                m_BlackColor.r = m_BlackColor.g = m_BlackColor.b = 1;
            m_BlackColor.a = player.GetAlhpa(true)* alphaFactor;
            m_pBG.color = m_BlackColor;
        }
    }
}
