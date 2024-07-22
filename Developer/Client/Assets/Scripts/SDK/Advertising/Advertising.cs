using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct AdvConfig : ISDKConfig
    {
        public GoogleAdCfg googleAd;
        public UnityAdCfg unityAd;
        public FacebookAdCfg facebookAd;
    }
    //------------------------------------------------------
    public class Advertising : ISDKAgent
    {
        public enum EAdType
        {
            None = 0,
            Interstitial,
            Rewarded,
            Banner,
        }
        public enum EAction
        {
            None = 0,
            Loading,
            Failed,
            BeginVideo,
            EndVideo,
        }
        public struct AdParam : ISDKParam
        {
            public EAction action;
            public string pubID;
            public int advID;
            public int funcType;
            public int externID;
            public int subID;
            public void Clear()
            {
                pubID = null;
                advID = -1;
                funcType = -1;
                externID = 0;
                subID = 0;
            }
            public bool IsValid()
            {
                return advID >= 0 && funcType>=0;
            }

        }
        //------------------------------------------------------
        public static Advertising StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
            Advertising adv = new Advertising();
            adv.SetCallback(callback);
            if (adv.Init(config))
            {
                adv.SetCallback(callback);
                return adv;
            }
            return null;
        }
        //------------------------------------------------------
        public static Advertising This
        {
            get { return ms_Agent as Advertising; }
        }

        List<IPlatformAdSDK> m_vAdSdks = null;

        //------------------------------------------------------
        protected override bool Init(ISDKConfig config)
        {
            if (m_pCallback != null) m_pCallback.OnStartUp(this);

#if USE_GOOGLE_ADMOB
            GoogleAdMob google = new GoogleAdMob();
            if (google.Init(this, config))
            {
                if (m_vAdSdks == null) m_vAdSdks = new List<IPlatformAdSDK>(4);
                m_vAdSdks.Add(google);
            }
#endif
#if USE_FACEBOOK_AD
            FacebookAd facebookAd = new FacebookAd();
            if (facebookAd.Init(this, config))
            {
                if (m_vAdSdks == null) m_vAdSdks = new List<IPlatformAdSDK>(4);
                m_vAdSdks.Add(facebookAd);
            }
#endif
#if USE_UNITY_AD
            UnityAd unityAd = new UnityAd();
            if (unityAd.Init(this, config))
            {
                if (m_vAdSdks == null) m_vAdSdks = new List<IPlatformAdSDK>(4);
                m_vAdSdks.Add(unityAd);
            }
#endif
            return true;
        }
        //------------------------------------------------------
        public static bool IsCheckingAds()
        {
            if (This == null) return false;
            if (This.m_vAdSdks == null || This.m_vAdSdks.Count <= 0)
            {
                for (int i = 0; i < This.m_vAdSdks.Count; ++i)
                {
                    if (This.m_vAdSdks[i] !=null && This.m_vAdSdks[i].IsChecking())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //------------------------------------------------------
        public static bool CheckVideo(AdParam param)
        {
            if (This == null) return false;
            param.action = EAction.None;
            if (This.m_vAdSdks == null || This.m_vAdSdks.Count<=0)
            {
                param.action = EAction.EndVideo;
                if (This.m_pCallback != null)
                {
                    This.m_pCallback.OnSDKAction(This, param);               
                }

                return false;
            }
            param.action = EAction.BeginVideo;
            for (int i =0; i < This.m_vAdSdks.Count;++i)
            {
                if(This.m_vAdSdks[i] != null && This.m_vAdSdks[i].AdCheck(param))
                {
                    return true;
                }
            }

            param.action = EAction.EndVideo;
            if (This.m_pCallback != null)
            {
                This.m_pCallback.OnSDKAction(This, param);
            }
            return false;
        }
        //------------------------------------------------------
        public override void Update(float fTime)
        {
            if(m_vAdSdks!=null)
            {
                for(int i = 0; i < m_vAdSdks.Count; ++i)
                {
                    if(m_vAdSdks[i]!=null) m_vAdSdks[i].Update(fTime);
                }
            }
        }
    }
}
