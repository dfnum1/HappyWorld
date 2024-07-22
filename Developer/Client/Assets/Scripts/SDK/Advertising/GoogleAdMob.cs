using System;
using System.Collections.Generic;
using UnityEngine;

#if USE_GOOGLE_ADMOB
using GoogleMobileAds.Api;
#endif

namespace SDK
{
    [System.Serializable]
    public struct GoogleAdCfg
    {
        public string andoridID;
        public string iosID;

//         public string bannerAndroidID;
//         public string bannerIosID;
// 
//         public string interstitialAndroidID;
//         public string interstitialIosID;

        public string rewardedAndroidID;
        public string rewardedIosID;

        public string getAppID()
        {
#if UNITY_EDITOR
#if USE_GOOGLE_ADMOB
            return AdRequest.TestDeviceSimulator;
#else
                return null;
#endif
#elif UNITY_ANDROID
                return andoridID;
#elif UNITY_IPHONE
                return iosID;
#else
                return null;
#endif
        }

//        public string getBannerID()
//        {
//#if UNITY_EDITOR
//            return bannerAndroidID;
//#elif UNITY_ANDROID
//                return bannerAndroidID;
//#elif UNITY_IPHONE
//                return bannerIosID;
//#else
//                return null;
//#endif
//        }

//        public string getInterstitialID()
//        {
//#if UNITY_EDITOR
//            return interstitialAndroidID;
//#elif UNITY_ANDROID
//                return interstitialAndroidID;
//#elif UNITY_IPHONE
//                return interstitialIosID;
//#else
//                return null;
//#endif
//        }

        public string getRewardedID()
        {
#if UNITY_EDITOR
            return "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_ANDROID
                if(string.IsNullOrEmpty(rewardedAndroidID)) return "ca-app-pub-3940256099942544/5224354917";
                return rewardedAndroidID;
#elif UNITY_IPHONE
            if(string.IsNullOrEmpty(rewardedIosID))  return "ca-app-pub-3940256099942544/1712485313";
                return rewardedIosID;
#else
                return null;
#endif
        }
    }

#if USE_GOOGLE_ADMOB
    public class GoogleAdMob : IPlatformAdSDK
    {
        string m_strCurrentID = "";
        GoogleAdCfg m_AdvConfig;

        RewardedAd m_RewaredAd;

        Advertising.AdParam m_AdParam;
        ISDKAgent m_pAgent = null;

        bool m_bReqCreate = false;
        bool m_bReqAds = false;
        string m_strPubID = null;

        Advertising.EAction m_ListenAction = Advertising.EAction.None;
        //------------------------------------------------------
        public bool Init(ISDKAgent agent, ISDKConfig config)
        {
            m_bReqCreate = false;
            m_strPubID = null;
            m_pAgent = agent;
            m_ListenAction = Advertising.EAction.None;
            m_AdvConfig = ((AdvConfig)config).googleAd;
#if UNITY_ANDROID
            m_strCurrentID = m_AdvConfig.andoridID;
#elif UNITY_IPHONE
            m_strCurrentID = m_AdvConfig.iosID;
#else
            m_strCurrentID = null;
#endif
            if (string.IsNullOrEmpty(m_strCurrentID)) return false;

            List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

            m_AdParam.Clear();
#if UNITY_EDITOR
#elif UNITY_IPHONE
             deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
             deviceIds.Add("F77D06D0E98FB8A462EB5BD6A12F0E70");
#endif
            RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            //     .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            .SetTestDeviceIds(deviceIds)
            .build();
            MobileAds.SetRequestConfiguration(requestConfiguration);

            MobileAds.Initialize((initStatus) =>
            {
                Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
                foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
                {
                    string className = keyValuePair.Key;
                    AdapterStatus status = keyValuePair.Value;
                    switch (status.InitializationState)
                    {
                        case AdapterState.NotReady:
                            // The adapter initialization did not complete.
                            MonoBehaviour.print("Adapter: " + className + " not ready.");
                            break;
                        case AdapterState.Ready:
                            // The adapter was successfully initialized.
                            MonoBehaviour.print("Adapter: " + className + " is initialized.");
                            break;
                    }
                }

                CreateReward();
                m_RewaredAd.LoadAd(CreateAdRequest());
            });
            return true;
        }
        //------------------------------------------------------
        void CreateReward(string id = null)
        {
            if (string.IsNullOrEmpty(id))
                id = m_AdvConfig.getRewardedID();
            //! 激励广告
            if (!string.IsNullOrEmpty(id))
            {
                m_RewaredAd = new RewardedAd(id);
                m_RewaredAd.OnAdLoaded += HandleOnAdLoaded;
                m_RewaredAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
                m_RewaredAd.OnAdOpening += HandleOnAdOpened;
                m_RewaredAd.OnAdFailedToShow += HandleOnAdFailedToShow;
                m_RewaredAd.OnAdClosed += HandleOnAdClosed;
                m_RewaredAd.OnUserEarnedReward += HandleOnUserEarnedReward;
            }
            m_strPubID = id;
        }
        //------------------------------------------------------
        private AdRequest CreateAdRequest(string context= "codesd:running")
        {
            return new AdRequest.Builder()
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("F77D06D0E98FB8A462EB5BD6A12F0E70")
                .AddKeyword(context)
                .SetBirthday(new DateTime(1985,1,1))
//                .SetGender(Gender.Male) 性别
                .AddExtra("color_bg", "9B30FF")
                .TagForChildDirectedTreatment(false)//儿童在线隐私保护法
                .Build();
        }
        //------------------------------------------------------
        public bool IsChecking()
        {
            return m_AdParam.IsValid();
        }
        //------------------------------------------------------
        public bool AdCheck(Advertising.AdParam param)
        {
            string pubId = param.pubID;
            if (string.IsNullOrEmpty(pubId))
                pubId = m_AdvConfig.getRewardedID();
            if (string.IsNullOrEmpty(pubId)) return false;

            m_AdParam = param;
            m_AdParam.action = Advertising.EAction.Loading;
            m_ListenAction = Advertising.EAction.Loading;
            if (pubId.CompareTo(m_strPubID) != 0)
            {
                CreateReward(pubId);
            }
            if (m_RewaredAd == null) return false;

            m_bReqAds = true;
            if(!m_RewaredAd.IsLoaded())
                m_RewaredAd.LoadAd(CreateAdRequest());

            m_ListenAction = Advertising.EAction.BeginVideo;
            return true;
        }
        //------------------------------------------------------
        public void HandleOnAdLoaded(object sender, EventArgs args)
        {
            Debug.LogWarning("HandleOnAdLoaded:" + m_RewaredAd.GetResponseInfo().GetMediationAdapterClassName());
            //do this when ad loads
        }
        //------------------------------------------------------
        public void HandleOnAdFailedToLoad(object sender, EventArgs args)
        {
           // m_RewaredAd.LoadAd(CreateAdRequest());
            m_ListenAction = Advertising.EAction.Failed;
           // m_bReqCreate = true;
            Debug.LogWarning("HandleOnAdFailedToLoad");
        }
        //------------------------------------------------------
        public void HandleOnAdFailedToShow(object sender, EventArgs args)
        {
            m_RewaredAd.LoadAd(CreateAdRequest());
            m_ListenAction = Advertising.EAction.Failed;
            m_bReqCreate = true;
            Debug.LogWarning("HandleOnAdFailedToShow");
        }
        //------------------------------------------------------
        public void HandleOnAdLeavingApplication(object sender, EventArgs args)
        {
            if (!m_RewaredAd.IsLoaded())
                m_RewaredAd.LoadAd(CreateAdRequest());
            Debug.LogWarning("HandleOnAdLeavingApplication");
        }
        //------------------------------------------------------
        public void HandleOnAdOpened(object sender, EventArgs args)
        {
            m_ListenAction = Advertising.EAction.BeginVideo;
            Debug.LogWarning("HandleOnAdOpened");
        }
        //------------------------------------------------------
        public void HandleOnAdClosed(object sender, EventArgs args)
        {
            m_ListenAction = Advertising.EAction.EndVideo;

            Debug.LogWarning("HandleOnAdClosed");
            m_bReqAds = false;
            m_bReqCreate = true;
        }
        //------------------------------------------------------
        public void HandleOnUserEarnedReward(object sender, EventArgs args)
        {
            m_ListenAction = Advertising.EAction.EndVideo;
            Debug.LogWarning("HandleOnUserEarnedReward");
            m_bReqAds = false;
            m_bReqCreate = true;
        }
        //------------------------------------------------------
        public void Update(float fTime)
        {
            if(m_ListenAction == Advertising.EAction.Failed)
            {
                if(m_AdParam.IsValid() &&m_AdParam.action != Advertising.EAction.Failed)
                {
                    m_AdParam.action = Advertising.EAction.Failed;
                    m_pAgent.OnSDKAction(m_AdParam);
                }
            }
            else if(m_ListenAction == Advertising.EAction.BeginVideo)
            {
                if(m_AdParam.IsValid() &&m_AdParam.action != Advertising.EAction.BeginVideo)
                {
                    m_AdParam.action = Advertising.EAction.BeginVideo;
                    m_pAgent.OnSDKAction(m_AdParam);
                }
            }
            else if(m_ListenAction == Advertising.EAction.EndVideo)
            {
                if(m_AdParam.IsValid() && m_AdParam.action != Advertising.EAction.EndVideo)
                {
                    m_AdParam.action = Advertising.EAction.EndVideo;
                    m_pAgent.OnSDKAction(m_AdParam);
                    m_AdParam.Clear();
                }
            }
            else if(m_ListenAction == Advertising.EAction.Loading)
            {
                if(m_AdParam.IsValid() &&m_AdParam.action != Advertising.EAction.Loading)
                {
                    m_AdParam.action = Advertising.EAction.Loading;
                    m_pAgent.OnSDKAction(m_AdParam);
                }
            }
            if (m_bReqAds)
            {
                if(m_RewaredAd.IsLoaded())
                {
                    m_RewaredAd.Show();
                    m_bReqAds = false;
                }
            }
            else
            {
                if (m_bReqCreate)
                {
                    m_bReqCreate = false;
                    CreateReward();
                    m_RewaredAd.LoadAd(CreateAdRequest());
                }
            }
        }
    }
#endif
}