using System;
using System.Collections.Generic;
using UnityEngine;
#if USE_GAMEANALYTICS
using GameAnalyticsSDK;
#endif

namespace SDK
{
    [System.Serializable]
    public struct GameAnalyticsCfg : ISDKConfig
    {
        public string user;
        public string password;
        public GameObject pMainGO;
    }
    public class GameAnalyticsUnity : ISDKAgent
    {
#if USE_GAMEANALYTICS
        GameAnalyticsCfg m_Config;
        ISDKAgent m_pAgent = null;
        static Dictionary<string, object> ms_vEventTemp = null;
        static bool ms_bInited = false;
#endif
        //------------------------------------------------------
        public static GameAnalyticsUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_GAMEANALYTICS
            ms_bInited = false;
            GameAnalyticsUnity talkingData = new GameAnalyticsUnity();
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
#if USE_GAMEANALYTICS
            m_Config = (GameAnalyticsCfg)cfg;
            m_Config.pMainGO.AddComponent<GameAnalytics>();
            m_Config.pMainGO.AddComponent<GameAnalyticsSDK.Events.GA_SpecialEvents>();
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                SDKCallback listener = m_Config.pMainGO.AddComponent<SDKCallback>();
                GameAnalytics.RequestTrackingAuthorization(listener);
            }
            else
            {
                GameAnalytics.Initialize();
            }
            GameAnalytics.OnRemoteConfigsUpdatedEvent += OnRemoteConfigsUpdatedEvent;
            return false;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        internal static void SetUserId(string userId)
        {
#if USE_GAMEANALYTICS
            if (!ms_bInited) return;
            GameAnalytics.SetCustomId(userId);
#endif
        }
        //------------------------------------------------------
        internal static void PayEvent(string userId, string orderId, int amount, string currenncyType, string payType, string itemId = null, int itemCnt = 0)
        {
#if USE_GAMEANALYTICS
            GameAnalytics.NewBusinessEvent(currenncyType, amount, itemId, itemCnt.ToString(), payType);
#endif
        }
        //------------------------------------------------------
        internal static void LogEvent(string eventName)
        {
#if USE_GAMEANALYTICS
            var kvs = GameSDK.GetEventKVs();
            if (eventName.CompareTo("Ad") == 0)
            {
                GAAdType adType = GAAdType.Undefined;
                GAAdAction actionType = GAAdAction.Undefined;
                string action;
                if(kvs.TryGetValue("adAction", out action))
                {
                    if (action.CompareTo(AdCallbackParam.EAction.Clicked.ToString()) == 0) actionType = GAAdAction.Clicked;
                    else if (action.CompareTo(AdCallbackParam.EAction.Played.ToString()) == 0) actionType = GAAdAction.Show;
                    else if (action.CompareTo(AdCallbackParam.EAction.PlayFailed.ToString()) == 0) actionType = GAAdAction.FailedShow;
                    else if (action.CompareTo(AdCallbackParam.EAction.Reward.ToString()) == 0) actionType = GAAdAction.RewardReceived;
                    else actionType = GAAdAction.Undefined;
                }
                if (kvs.TryGetValue("adType", out action))
                {
                    if (action.CompareTo(EAdType.Reward.ToString()) == 0) adType = GAAdType.RewardedVideo;
                    else if (action.CompareTo(EAdType.Banner.ToString()) == 0) adType = GAAdType.Banner;
                    else if (action.CompareTo(EAdType.FullScreen.ToString()) == 0) adType = GAAdType.Interstitial;
                    else adType = GAAdType.Undefined;
                }
                if (!kvs.TryGetValue("posId", out action))
                {
                    action = "";
                }
                GameAnalytics.NewAdEvent(actionType, adType, "", action);
            }
            else
            {
                if (ms_vEventTemp == null) ms_vEventTemp = new Dictionary<string, object>(1);
                ms_vEventTemp.Clear();
                foreach (var db in kvs)
                    ms_vEventTemp[db.Key] = db.Value;
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, eventName, ms_vEventTemp);
                ms_vEventTemp.Clear();
            }
#endif
        }
        //------------------------------------------------------
        static void OnRemoteConfigsUpdatedEvent()
        {

        }
        //------------------------------------------------------
#if USE_GAMEANALYTICS
        class SDKCallback : MonoBehaviour, IGameAnalyticsATTListener
        {
            public void GameAnalyticsATTListenerNotDetermined()
            {
                GameAnalytics.Initialize();
            }
            public void GameAnalyticsATTListenerRestricted()
            {
                GameAnalytics.Initialize();
            }
            public void GameAnalyticsATTListenerDenied()
            {
                GameAnalytics.Initialize();
            }
            public void GameAnalyticsATTListenerAuthorized()
            {
                GameAnalytics.Initialize();
            }
        }
#endif
    }
}