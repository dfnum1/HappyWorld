using Framework.Plugin.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace TopGame.UI
{
    public class LogoUI : MonoBehaviour
    {
        public AnimationCurve alphaCurve;
        public RawImage videoImage;
        public float fadeInNext;

        Framework.Plugin.Media.IMediaPlayer m_VideoPlayer;
        Color m_BlackColor = Color.black;

        static UI.LogoUI ms_LogoUI;
        //------------------------------------------------------
        private void Awake()
        {
            ms_LogoUI = this;
          //  GameObject.DontDestroyOnLoad(this);
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_LogoUI = null;
        }
        //------------------------------------------------------
        void SetVideoPlayer(Framework.Plugin.Media.IMediaPlayer videoPlayer)
        {
            if (m_VideoPlayer != null) m_VideoPlayer.Stop();
             m_VideoPlayer = videoPlayer;
            if(m_VideoPlayer!=null) m_VideoPlayer.AddListener(OnVideoEvent);
        }
        //------------------------------------------------------
        void OnVideoEvent(IMediaPlayer player, MediaPlayerEvent.EventType type, ErrorCode code)
        {
            if (type == MediaPlayerEvent.EventType.Closing ||
               type == MediaPlayerEvent.EventType.Error ||
               type == MediaPlayerEvent.EventType.FinishedPlaying)
            {
                if (player == m_VideoPlayer)
                {
                    gameObject.SetActive(false);
                    GameObject.Destroy(gameObject);
                    m_VideoPlayer = null;
                }
            }
        }
        //------------------------------------------------------
        public static bool Play()
        {
            if (ms_LogoUI == null) return false;
            ms_LogoUI.SetVideoPlayer(Core.VideoController.getInstance().PlayVideo("Video/logo.mp4"));
            if (ms_LogoUI.m_VideoPlayer != null)
                ms_LogoUI.m_VideoPlayer.SetCurveAlpha(ms_LogoUI.alphaCurve);
            bool bOpen = ms_LogoUI.m_VideoPlayer != null;
            if(!bOpen)
            {
                GameObject.Destroy(ms_LogoUI.gameObject);
            }
            return bOpen;
        }
        //------------------------------------------------------
        public static void Stop()
        {
            if (ms_LogoUI == null) return;
            if (ms_LogoUI.m_VideoPlayer != null)
                Core.VideoController.getInstance().StopVideo(ms_LogoUI.m_VideoPlayer);
            ms_LogoUI.m_VideoPlayer = null;
            GameObject.Destroy(ms_LogoUI.gameObject);
            ms_LogoUI = null;
        }
        //------------------------------------------------------
        public static bool CanFadeInNext()
        {
            if (ms_LogoUI == null) return true;
            if (ms_LogoUI.m_VideoPlayer == null || ms_LogoUI.m_VideoPlayer.IsFinished()) return true;
            if(ms_LogoUI.fadeInNext>0)
                return ms_LogoUI.m_VideoPlayer.GetCurrentTimeMs() >= ms_LogoUI.fadeInNext * 1000;
            return false;
        }
        //------------------------------------------------------
        private void Update()
        {
            if (m_VideoPlayer == null || videoImage == null)
                return;
            videoImage.texture = m_VideoPlayer.GetTexture();
            if(videoImage.texture == null)
                m_BlackColor.r = m_BlackColor.g = m_BlackColor.b = 0;
            else
                m_BlackColor.r = m_BlackColor.g = m_BlackColor.b = 1;
            m_BlackColor.a = m_VideoPlayer.GetAlhpa(true);
            videoImage.color = m_BlackColor;

            if (m_VideoPlayer.IsFinished())
            {
                GameObject.Destroy(gameObject);
                m_VideoPlayer = null;
            }
        }
    }
}