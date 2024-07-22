using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct AppsFlyerCfg : ISDKConfig
    {
        public string androidDevKey;
        public string iosDevKey;

        public bool isDebug;
        public bool getConversionData;

        public MonoBehaviour binderMono;

        public string getAppID()
        {
            return Application.identifier;
        }
        public string getDevKey()
        {
#if USE_APPSFLYER
#if UNITY_ANDROID
            return androidDevKey;
#elif UNITY_IPHONE
                        return iosDevKey;
#else
                        return null;
#endif
#else
            return null;
#endif
        }
    }
    public class AppsFlyerUnity : ISDKAgent
    {
        AppsFlyerCfg m_AdvConfig;

        Advertising.AdParam m_AdParam;
        ISDKAgent m_pAgent = null;
        //------------------------------------------------------
        public static AppsFlyerUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_APPSFLYER
            AppsFlyerUnity appsFlyer = new AppsFlyerUnity();
            appsFlyer.SetCallback(callback);
            if (appsFlyer.Init(config)) return appsFlyer;
            return null;
#else
            return null;
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_APPSFLYER
            AppsFlyerCfg config = (AppsFlyerCfg)cfg;
            string strAppId = config.getAppID();
            string strDevKey = config.getDevKey();
            if (string.IsNullOrEmpty(strAppId) || string.IsNullOrEmpty(strDevKey))
            {
                Debug.LogWarning("AppId or DevKey is null !");
                return false;
            }
            // These fields are set from the editor so do not modify!
            //******************************//
            AppsFlyerSDK.AppsFlyer.setIsDebug(config.isDebug);
            AppsFlyerSDK.AppsFlyer.initSDK(strDevKey, strAppId, config.getConversionData ? config.binderMono : null);
            AppsFlyerSDK.AppsFlyer.startSDK();
            Debug.Log("AppsFlyer Startup!");
            return true;
#else
            return false;
#endif
        }
    }
}