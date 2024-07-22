using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct AdmobileCfg : ISDKConfig
    {
        public GameObject pMainGO;
    }
    public class AdmobileUnity : ISDKAgent
    {
#if USE_ADMOBILE
        static AndroidJavaObject ms_Handle;
        AdmobileCfg m_Config;
        ISDKAgent m_pAgent = null;
        static bool ms_bInited = false;
#endif
        //------------------------------------------------------
        public static AdmobileUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_ADMOBILE
            ms_bInited = false;
            AdmobileUnity talkingData = new AdmobileUnity();
            if (talkingData.Init(config))
            {
                talkingData.SetCallback(callback);
                ms_bInited = true;
                return talkingData;
            }
            return null;
#else
            return null;
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {

#if USE_ADMOBILE
            m_Config = (AdmobileCfg)cfg;
            Debug.Log("Admobile begin init");
            if (m_Config.pMainGO == null)
            {
                Debug.LogWarning("unity go is null");
                return false;
            }
            ms_Handle = GameSDK.GetSDKHandler();
            if (ms_Handle == null)
            {
                Debug.LogWarning("unfind \"com.unity.sdks.SDKHandler\"");
                return false;
            }
            SDKCallback pCallback = m_Config.pMainGO.AddComponent<SDKCallback>();
            ms_Handle.CallStatic("SetListener", pCallback.gameObject.name);
            if(ms_Handle.CallStatic<bool>("Init", "admobile", ""))
                return true;
            GameObject.Destroy(pCallback);
            return false;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        public static void ShowAd(string type, string posId)
        {
#if USE_ADMOBILE
            if (ms_Handle!=null)
            {
                ms_Handle.CallStatic("Ads", "admobile", type, posId);
            }
#endif
        }
#if USE_ADMOBILE
        class SDKCallback : MonoBehaviour
        {
            void OnAdReceive(string msg)
            {

            }
            void OnAdExpose(string msg)
            {

            }
            void OnAdClick(string msg)
            {

            }
            void OnAdClose(string msg)
            {

            }
            void OnAdFailed(string msg)
            {

            }
            void OnAdComplete(string msg)
            {

            }
            void OnAdReward(string msg)
            {

            }
        }
#endif
    }
}