using System;
using System.Collections.Generic;
using UnityEngine;
#if USE_FACEBOOK_AD
using AudienceNetwork.Utility;
#endif
namespace SDK
{
    [System.Serializable]
    public struct FacebookAdCfg
    {
        public string andoridID;
        public string iosID;
        public string getID()
        {
#if UNITY_EDITOR
            return andoridID;
#elif UNITY_ANDROID
                return andoridID;
#elif UNITY_IPHONE
                return iosID;
#else
                return null;
#endif
        }
    }
#if USE_FACEBOOK_AD
    public class FacebookAd : IPlatformAdSDK
    {
        FacebookAdCfg m_AdvConfig;

        Advertising.AdParam m_AdParam;
        ISDKAgent m_pAgent = null;

        AudienceNetwork.InterstitialAd m_interstitialFacebook;
        //------------------------------------------------------
        public bool Init(ISDKAgent agent, ISDKConfig config)
        {
            m_pAgent = agent;
            m_AdvConfig = ((AdvConfig)config).facebookAd;

            if (string.IsNullOrEmpty(m_AdvConfig.getID())) return false;

            m_interstitialFacebook = new AudienceNetwork.InterstitialAd(m_AdvConfig.getID());
            GameObject faceBookAd = new GameObject("FaceBookAdHandle");
            GameObject.DontDestroyOnLoad(faceBookAd);
            m_interstitialFacebook.Register(faceBookAd);

            m_interstitialFacebook.InterstitialAdDidLoad = HandleOnAdLoaded;
            m_interstitialFacebook.InterstitialAdDidFailWithError = HandleOnAdFailedToLoad;
            m_interstitialFacebook.interstitialAdDidClose = HandleOnAdClosed;

            m_interstitialFacebook.LoadAd();
            Debug.Log("unity Ads inited !!!");
            return true;
        }
        //------------------------------------------------------
        public bool AdCheck(Advertising.AdParam param)
        {
            if (m_interstitialFacebook == null || !m_interstitialFacebook.IsValid()) return false;
            if (param.adType == Advertising.EAdType.None) param.adType = Advertising.EAdType.Rewarded;
            m_AdParam = param;
            m_AdParam.action = Advertising.EAction.BeginVideo;

            return m_interstitialFacebook.Show();
        }
        //------------------------------------------------------
        public  bool IsChecking()
        {
            return false;
        }
        //------------------------------------------------------
        public void Update(float fTime)
        {
           
        }
        //------------------------------------------------------
        public void HandleOnAdLoaded()
        {
            if (m_pAgent != null) m_pAgent.OnSDKPauseGame();
        }
        //------------------------------------------------------
        public void HandleOnAdFailedToLoad(string error)
        {
            Debug.LogWarning("facebook ad load error:" + error);
            if (m_pAgent != null) m_pAgent.OnSDKResumeGame();
        }
        //------------------------------------------------------
        public void HandleOnAdClosed()
        {
            if (m_pAgent != null) m_pAgent.OnSDKResumeGame();
            m_AdParam.action = Advertising.EAction.EndVideo;
            if (m_pAgent != null)
            {
                m_pAgent.OnSDKAction(m_AdParam);
            }
        }
    }
#endif
}