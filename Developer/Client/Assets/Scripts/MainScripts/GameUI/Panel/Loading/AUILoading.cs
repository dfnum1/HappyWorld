/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AUILoading
作    者:	HappLI
描    述:	加载界面 基础句柄
*********************************************************************/

using Framework.Plugin.Media;
using UnityEngine;

namespace TopGame.UI
{
    public abstract class AUILoading : UIBase
    {
        static  AUILoading ms_LoadingHandler = null;
        protected float m_fProgress = 0;
        protected float m_fToProgress = 0;
        protected float m_fWatingTime = 0;
        private bool m_bFadeOuted = false;
        protected float m_fFadeAlphaHide = -1;

        bool m_bCloseCamera = false;
        IMediaPlayer m_MediaPlayer;
        //------------------------------------------------------
        public float Progress
        {
            get { return m_fProgress; }
        }
        //------------------------------------------------------
        public static float ShowLoading
        {
            get
            {
                if (ms_LoadingHandler == null || !ms_LoadingHandler.IsVisible()) return GameInstance.getInstance().GetProgress();
                return ms_LoadingHandler.m_fProgress;
            }
        }
        //------------------------------------------------------
        public static bool IsLoadingEnd
        {
            get
            {
                if (ms_LoadingHandler == null) return ShowLoading >= Base.GlobalUtil.PROGRESS_END_SNAP;
                if (ShowLoading >= Base.GlobalUtil.PROGRESS_END_SNAP && ms_LoadingHandler.m_fWatingTime <= 0) return true;
                return false;
            }
        }
        //------------------------------------------------------
        public static AUILoading loadingHandler
        {
            get
            {
                return ms_LoadingHandler;
            }
        }
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            
            m_fWatingTime = 0;
            m_bFadeOuted = false;
            m_fFadeAlphaHide = -1;
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
            if (ms_LoadingHandler == this)
                ms_LoadingHandler = null;
            m_fWatingTime = 0;
            m_bFadeOuted = false;
            m_fFadeAlphaHide = 1;
            if(Core.VideoController.getInstance()!=null) Core.VideoController.getInstance().StopVideo(m_MediaPlayer);
            m_MediaPlayer = null;
        }
        //------------------------------------------------------
        protected override void DoShow()
        {
            base.DoShow();
            m_bCloseCamera = false;
            ms_LoadingHandler = this;
            m_fProgress = 0;
            m_fToProgress = 0;
            m_bFadeOuted = false;
            m_fWatingTime = 0;
            m_fFadeAlphaHide = 1;
            SetGBAlpha(1);
            Update(0);
        }
        //------------------------------------------------------
        protected override void DoShowEnd()
        {
            base.DoShowEnd();
            m_bCloseCamera = true;
            Core.CameraKit.CloseCameraRef(true);
        }
        //------------------------------------------------------
        protected override void DoHide()
        {
            base.DoHide();
            if (m_bCloseCamera)
            {
                Core.CameraKit.CloseCameraRef(false);
                m_bCloseCamera = false;
            }
            m_fToProgress = 0;
            m_fProgress = 0;
            m_bFadeOuted = false;
            m_fWatingTime = 0;
            m_fFadeAlphaHide = 1;
            Core.VideoController.getInstance().StopVideo(m_MediaPlayer);
            m_MediaPlayer = null;
            SetGBAlpha(1);
            if (ms_LoadingHandler == this)
                ms_LoadingHandler = null;
        }
        //------------------------------------------------------
        public void SetVideoBG(IMediaPlayer player)
        {
            if (m_MediaPlayer != null && player != m_MediaPlayer)
            {
                Core.VideoController.getInstance().StopVideo(m_MediaPlayer);
            }
            m_MediaPlayer = player;
            Update(0);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public void SetVideoBG(string strVideo)
        {
            if (m_MediaPlayer != null && strVideo.CompareTo(m_MediaPlayer.GetVideoPath()) == 0) return;
            m_MediaPlayer = Core.VideoController.getInstance().PlayVideo(strVideo);
            if (m_MediaPlayer != null)
                m_MediaPlayer.SetLooping(true);
        }
        //------------------------------------------------------
        protected virtual void UpdateToProgress()
        {
            m_fToProgress = Mathf.Max(GameInstance.getInstance().GetProgress(), m_fToProgress);
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            UpdateToProgress();
            m_fProgress = Mathf.Lerp(m_fProgress, m_fToProgress, Time.fixedDeltaTime*5f);

            if(m_fWatingTime<=0 && m_fProgress >= Base.GlobalUtil.PROGRESS_END_SNAP)
            {
                m_fWatingTime = 0.1f;
            }

            if(m_fWatingTime>0)
            {
                m_fWatingTime -= fFrame;
                if (m_fWatingTime <= 0)
                    m_fWatingTime = 0;
            }
            if(m_fProgress >= Base.GlobalUtil.PROGRESS_END_SNAP)
                CheckVideoFade();

            if (m_bFadeOuted)
            {
                if (m_fFadeAlphaHide > 0)
                {
                    m_fWatingTime = 0.01f;
                    m_fFadeAlphaHide -= fFrame*20;
                    if (m_fFadeAlphaHide < 0)
                    {
                        m_fWatingTime = 0;
                    }
                    SetGBAlpha(m_fFadeAlphaHide);
                }
            }

            if (m_MediaPlayer != null && !m_MediaPlayer.IsFinished())
            {
                SyncBGTexture(m_MediaPlayer.GetTexture());
            }
        }
        //------------------------------------------------------
        void CheckVideoFade()
        {
            if (!m_bFadeOuted)
            {
                UIVideo video = GameInstance.getInstance().uiFramework.CastGetUI<UIVideo>((ushort)EUIType.VideoPanel, false);
                if (video != null && video.IsVisible() && video.GetVideoPlayer() != null)
                {
                    if (!video.GetVideoPlayer().IsPrepared() || video.GetVideoPlayer().GetTexture() == null)
                    {
                        m_fWatingTime = 0.1f;
                    }
                    else
                    {
                        m_bFadeOuted = true;
                        m_fFadeAlphaHide = 1;
                    }
                }
            }
        }
        //------------------------------------------------------
        public void StartFadeOut()
        {
            if (m_bFadeOuted) return;
            if (m_bCloseCamera)
            {
                GameInstance.getInstance().cameraController.CloseCameraRef(false);
                m_bCloseCamera = false;
            }
        }
        //------------------------------------------------------
        public abstract void SetGBAlpha(float fAlpha);
        //------------------------------------------------------
        public abstract void SetBGTexture(string strTexturePath);
        //------------------------------------------------------
        public abstract void SyncBGTexture(Texture pTexture);
    }
}
