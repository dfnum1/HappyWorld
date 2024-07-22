using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct ToBidCfg : ISDKConfig
    {
        public string appId;
        public string appKey;
        public GameObject pMainGO;
    }
    public class ToBidUnity : ISDKAgent
    {
#if USE_TOBID
#if UNITY_ANDROID
        static AndroidJavaObject ms_Handle;
#endif
        ToBidCfg m_Config;
        static bool ms_bInited = false;
#endif
        //------------------------------------------------------
        public static ToBidUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_TOBID
            ms_bInited = false;
            ToBidUnity toBid = new ToBidUnity();
            if (toBid.Init(config))
            {
                toBid.SetCallback(callback);
                ms_bInited = true;
                return toBid;
            }
            return null;
#else
            return null;
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_TOBID
            m_Config = (ToBidCfg)cfg;
            Debug.Log("TopOn begin init");
            if (m_Config.pMainGO == null)
            {
                Debug.LogWarning("unity go is null");
                return false;
            }
#if UNITY_ANDROID
            ms_Handle = GameSDK.GetSDKHandler();
            if(ms_Handle == null)
            {
                Debug.LogWarning("unfind \"com.unity.sdks.SDKHandler\"");
                return false;
            }
            SDKCallback pCallback = m_Config.pMainGO.gameObject.AddComponent<SDKCallback>();
            pCallback.Set(this);
            ms_Handle.CallStatic("SetListener", m_Config.pMainGO.gameObject.name);
            if (ms_Handle.CallStatic<bool>("Init", "tobid"))
                return true;
            GameObject.Destroy(pCallback);
#elif UNITY_IOS
            SDK.ToBidIOS.toBidInit( m_Config.appId, m_Config.appKey);
            SDKCallback pCallback = m_Config.pMainGO.gameObject.AddComponent<SDKCallback>();
            pCallback.Set(this);
            SDK.ToBidIOS.txSetListener(m_Config.pMainGO.gameObject.name);
            return true;
#endif

            return false;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        public static void ShowAd(string type, string posId)
        {
#if USE_TOBID
            if (!ms_bInited) return;
#if UNITY_ANDROID
            if (ms_Handle != null)
            {
                ms_Handle.CallStatic("Ads", "tobid", type, posId);
            }
#elif UNITY_IOS
                SDK.ToBidIOS.showAds(type, posId);
#endif
#endif
        }
#if USE_TOBID
        class SDKCallback : MonoBehaviour
        {
            ToBidUnity m_pAgent;
            public void Set(ToBidUnity agent)
            {
                m_pAgent = agent;
            }
            void OnAdPreLoadSuccess(string msg)
            {
                AdCallbackParam callParam = new AdCallbackParam();
                try
                {
                    JsonUtility.FromJsonOverwrite(msg, callParam);
                }
                catch(Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                callParam.action = AdCallbackParam.EAction.Loaded;
                GameSDK.DoCallback(m_pAgent, callParam);
            }
            void OnAdPreLoadFail(string msg)
            {
                AdCallbackParam callParam = new AdCallbackParam();
                try
                {
                    JsonUtility.FromJsonOverwrite(msg, callParam);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                callParam.action = AdCallbackParam.EAction.LoadFailed;
                GameSDK.DoCallback(m_pAgent, callParam);
            }
            void OnAdPlayEnd(string msg)
            {
                AdCallbackParam callParam = new AdCallbackParam();
                try
                {
                    JsonUtility.FromJsonOverwrite(msg, callParam);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                callParam.action = AdCallbackParam.EAction.PlayEnd;
                GameSDK.DoCallback(m_pAgent, callParam);
            }
            void OnAdPlayStart(string msg)
            {
                AdCallbackParam callParam = new AdCallbackParam();
                try
                {
                    JsonUtility.FromJsonOverwrite(msg, callParam);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                callParam.action = AdCallbackParam.EAction.Played;
                GameSDK.DoCallback(m_pAgent, callParam);
            }
            void OnAdClicked(string msg)
            {
                AdCallbackParam callParam = new AdCallbackParam();
                try
                {
                    JsonUtility.FromJsonOverwrite(msg, callParam);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                callParam.action = AdCallbackParam.EAction.Clicked;
                GameSDK.DoCallback(m_pAgent, callParam);
            }
            void OnAdSkiped(string msg)
            {
                AdCallbackParam callParam = new AdCallbackParam();
                try
                {
                    JsonUtility.FromJsonOverwrite(msg, callParam);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                callParam.action = AdCallbackParam.EAction.Clicked;
                GameSDK.DoCallback(m_pAgent, callParam);
            }
            void OnAdClosed(string msg)
            {
                AdCallbackParam callParam = new AdCallbackParam();
                try
                {
                    JsonUtility.FromJsonOverwrite(msg, callParam);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                callParam.action = AdCallbackParam.EAction.Closed;
                GameSDK.DoCallback(m_pAgent, callParam);
            }
            void OnAdRewarded(string msg)
            {
                AdCallbackParam callParam = new AdCallbackParam();
                try
                {
                    JsonUtility.FromJsonOverwrite(msg, callParam);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                callParam.action = AdCallbackParam.EAction.Reward;
                GameSDK.DoCallback(m_pAgent, callParam);
            }
            void OnAdLoadError(string msg)
            {
                AdCallbackParam callParam = new AdCallbackParam();
                try
                {
                    JsonUtility.FromJsonOverwrite(msg, callParam);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                callParam.action = AdCallbackParam.EAction.LoadFailed;
                GameSDK.DoCallback(m_pAgent, callParam);
            }
            void OnAdPlayError(string msg)
            {
                AdCallbackParam callParam = new AdCallbackParam();
                try
                {
                    JsonUtility.FromJsonOverwrite(msg, callParam);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                callParam.action = AdCallbackParam.EAction.PlayFailed;
                GameSDK.DoCallback(m_pAgent, callParam);
            }
        }
#endif
    }
}