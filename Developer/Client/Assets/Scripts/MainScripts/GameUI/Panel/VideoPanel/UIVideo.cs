/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIVideo
作    者:	HappLI
描    述:	视频界面
*********************************************************************/

using Framework.Plugin.Media;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace TopGame.UI
{
    [UI((ushort)EUIType.VideoPanel, UI.EUIAttr.UI)]
    public class UIVideo : UIBase
    {
        private UIVideoView m_view;

        private string m_strVideoPath = null;
        private bool m_bLoop = false;
        private float m_fDelayPlay;
        private IMediaPlayer m_VideoPlayer;

        private float m_fPrepareDuration = 0;
        private float m_FadeInAlphaFactor = 0;
        System.Action<MediaPlayerEvent.EventType> m_pCallback = null;
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            m_view = m_pView as UIVideoView;
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
            m_pCallback = null;
        }
        //------------------------------------------------------
        protected override void DoShow()
        {
            base.DoShow();
            m_FadeInAlphaFactor = 0;
            m_fPrepareDuration = 0;
            if (m_view != null)
            {
                m_view.ShowPrepare(false);
                m_view.SetPrepareAlpha(1, 0);
            }
        }
        //------------------------------------------------------
        protected override void DoHide()
        {
            base.DoHide();
            Core.VideoController.getInstance().StopVideo(m_VideoPlayer);
            m_fDelayPlay = 0;
            m_VideoPlayer = null;
            m_strVideoPath = null;
            m_pCallback = null;
            m_bLoop = false;
            m_FadeInAlphaFactor = 0;
        }
        //------------------------------------------------------
        public override bool CanHide()
        {
            return (m_VideoPlayer==null || !m_VideoPlayer.IsPlaying())&& m_fDelayPlay<=0;
        }
        //------------------------------------------------------
        public IMediaPlayer GetVideoPlayer()
        {
            return m_VideoPlayer;
        }
        //------------------------------------------------------
        public void SetPrepareDuration(float fDuration)
        {
            m_fPrepareDuration = fDuration;
            if(fDuration <=0)
            {
                if (m_view != null)
                {
                    m_view.ShowPrepare(false);
                    m_view.SetPrepareAlpha(1, 0);
                }
            }
            else
            {
                if (m_view != null)
                {
                    m_view.ShowPrepare(true);
                    m_view.SetPrepareAlpha(1, 0.2f);
                }
            }
        }
        //------------------------------------------------------
        public void ClearVideoPlayer(bool bOnlyClear = false)
        {
            if (bOnlyClear)
                m_VideoPlayer = null;
            else
            {
                Core.VideoController.getInstance().StopVideo(m_VideoPlayer);
                m_VideoPlayer = null;
                Hide();
            }
        }
        //------------------------------------------------------
        public void SetDefaultTexture(string path)
        {
            if (m_view != null)
                m_view.SetDefaultTexture(path);
        }
        //------------------------------------------------------
        public bool Play(VideoClip videoClip , bool bLoop = false, System.Action<MediaPlayerEvent.EventType> pCallback = null)
        {
            if (videoClip == null)
            {
                if (pCallback != null) pCallback(MediaPlayerEvent.EventType.Error);
                return false;
            }
            m_fDelayPlay = 0;
            m_bLoop = bLoop;
            m_pCallback = pCallback;
            if (m_VideoPlayer != null)
            {
                if (m_VideoPlayer.GetVideoPath().CompareTo(videoClip.name) == 0) return true;
                Core.VideoController.getInstance().StopVideo(m_VideoPlayer);
            }
            m_VideoPlayer = Core.VideoController.getInstance().PlayVideo(videoClip);
            if (m_VideoPlayer != null)
            {
                m_VideoPlayer.SetLooping(bLoop);
                m_VideoPlayer.AddListener(OnMediaListener);
                Show();
                return true;
            }
            else
            {
                if (m_pCallback != null) m_pCallback(MediaPlayerEvent.EventType.Error);
                m_pCallback = null;
            }
            return false;
        }
        //------------------------------------------------------
        public bool Play(string video, bool bLoop = false, float fDelayPlay = 0, System.Action<MediaPlayerEvent.EventType> pCallback = null)
        {
            if (string.IsNullOrEmpty(video))
            {
                if (pCallback != null) pCallback(MediaPlayerEvent.EventType.Error);
                return false;
            }
            if(video.CompareTo(m_strVideoPath) == 0)
            {
                return true;
            }
            m_bLoop = bLoop;
            m_strVideoPath = video;
            m_fDelayPlay = fDelayPlay;
            m_pCallback = pCallback;
            Show();
            return true;
        }
        //------------------------------------------------------
        bool DelayPlay()
        {
            if (string.IsNullOrEmpty(m_strVideoPath)) return false;
            if (m_VideoPlayer != null)
            {
                if (m_VideoPlayer.GetVideoPath().CompareTo(m_strVideoPath) == 0) return true;
                Core.VideoController.getInstance().StopVideo(m_VideoPlayer);
            }
            m_VideoPlayer = Core.VideoController.getInstance().PlayVideo(m_strVideoPath);
            if (m_VideoPlayer != null)
            {
                m_VideoPlayer.SetLooping(m_bLoop);
                m_VideoPlayer.AddListener(OnMediaListener);
                SetDefaultTexture(Data.DefaultResources.DefaultLoading);
                Show();
                return true;
            }
            else
            {
                if (m_pCallback != null) m_pCallback(MediaPlayerEvent.EventType.Error);
                m_pCallback = null;
            }
            return false;
        }
        //------------------------------------------------------
        void OnMediaListener(IMediaPlayer player, MediaPlayerEvent.EventType type, ErrorCode code)
        {
            if (m_VideoPlayer != player) return;
            if (m_pCallback != null) m_pCallback(type);
            if (type == MediaPlayerEvent.EventType.Closing ||
                type == MediaPlayerEvent.EventType.Error ||
                type == MediaPlayerEvent.EventType.FinishedPlaying)
            {
                m_VideoPlayer = null;
                Hide();
            }
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            base.InnerUpdate(fFrame);
            if(m_fDelayPlay>=0)
            {
                m_fDelayPlay -= fFrame;
                if(m_fDelayPlay<=0)
                {
                    DelayPlay();
                    m_fDelayPlay = -1;
                }
            }
            if (m_VideoPlayer == null) return;
            m_FadeInAlphaFactor += fFrame*5;
            m_FadeInAlphaFactor = Mathf.Clamp01(m_FadeInAlphaFactor);
            if(m_view!=null) m_view.SyncTexture(m_VideoPlayer, m_FadeInAlphaFactor);
            if (m_VideoPlayer.IsFinished())
                Hide();

            if(m_fPrepareDuration>0)
            {
                m_fPrepareDuration -= fFrame;
                if(m_fPrepareDuration<=0)
                {
                    m_fPrepareDuration = -1;
                    if (m_view != null) m_view.SetPrepareAlpha(0);
                }
            }
        }
    }
}
