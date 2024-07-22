#if USE_UNITY_AD
using UnityEngine.Advertisements;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct UnityAdCfg
    {
        public string andoridID;
        public string iosID;
        public string videoPlacement;
        public bool testMode;
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
#if USE_UNITY_AD
    public class UnityAd : IPlatformAdSDK, IUnityAdsListener
    {
        UnityAdCfg m_AdvConfig;

        Advertising.AdParam m_AdParam;
        ISDKAgent m_pAgent = null;
        //------------------------------------------------------
        public bool Init(ISDKAgent agent, ISDKConfig config)
        {
            m_pAgent = agent;
            m_AdvConfig = ((AdvConfig)config).unityAd;

            if (string.IsNullOrEmpty(m_AdvConfig.getID())) return false;
            if (!Advertisement.isSupported)
            {
                Debug.Log("unity Ads un supported !!!");
                return false;
            }
            if (Advertisement.isInitialized)
            {
                Debug.Log("unity Ads is already inited !!!");
                return true;
            }
            Advertisement.Initialize(m_AdvConfig.getID(), m_AdvConfig.testMode);

            Advertisement.AddListener(this);

            Debug.Log("unity Ads inited !!!");
            return true;
        }
        //------------------------------------------------------
        public bool IsReady(string zoomId = null)
        {
            if (!Advertisement.isInitialized) return false;
            if (string.IsNullOrEmpty(zoomId)) zoomId = null;
            return Advertisement.IsReady(zoomId);
        }
        //------------------------------------------------------
        public  bool IsChecking()
        {
            return false;
        }
        //------------------------------------------------------
        public bool AdCheck(Advertising.AdParam param)
        {
            if (!Advertisement.isInitialized) return false;
            if (param.adType == Advertising.EAdType.None) param.adType = Advertising.EAdType.Rewarded;
            m_AdParam = param;
            m_AdParam.action = Advertising.EAction.BeginVideo;

            string videName = string.IsNullOrEmpty(m_AdvConfig.videoPlacement)? param.adType.ToString(): m_AdvConfig.videoPlacement;
            if (Advertisement.IsReady(videName))
            {
                Advertisement.Show(videName);
                return true;
            }
            return false;
        }
        //------------------------------------------------------
        public void Update(float fTime)
        {
            if (!Advertisement.isInitialized) return;
        }
        //------------------------------------------------------
        public void OnUnityAdsReady(string placementId)
        {
        }
        //------------------------------------------------------
        public void OnUnityAdsDidError(string message)
        {
        }
        //------------------------------------------------------
        public void OnUnityAdsDidStart(string placementId)
        {
            if (m_pAgent != null) m_pAgent.OnSDKPauseGame();
            m_AdParam.action = Advertising.EAction.BeginVideo;
            if (m_pAgent != null)
            {
                m_pAgent.OnSDKAction(m_AdParam);
            }
        }
        //------------------------------------------------------
        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (m_pAgent != null) m_pAgent.OnSDKResumeGame();

            if(showResult == ShowResult.Finished)
            {
                m_AdParam.action = Advertising.EAction.EndVideo;
                if (m_pAgent != null)
                {
                    m_pAgent.OnSDKAction(m_AdParam);
                }
            }
        }
    }
#endif
}