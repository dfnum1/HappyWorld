#define TD_GAME
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct TalkingDataCfg : ISDKConfig
    {
        public string appID;
        public string channel_ios;
        public string channel_android;
        public string GetChannelID()
        {
#if UNITY_ANDROID
            return channel_android;
#elif UNITY_IPHONE
            return channel_ios;
#else
            return null;
#endif
        }
    }
    public class TalkingDataUnity : ISDKAgent
    {
#if USE_TALKINGDATA
        TalkingDataCfg m_Config;
        ISDKAgent m_pAgent = null;
        static bool ms_bInited = false;
        static Dictionary<string, object> ms_vEventTemp = null;
#endif
        //------------------------------------------------------
        public static TalkingDataUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_TALKINGDATA
            ms_bInited = false;
            TalkingDataUnity talkingData = new TalkingDataUnity();
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
#if USE_TALKINGDATA
            m_Config = (TalkingDataCfg)cfg;
            string channel = m_Config.GetChannelID();
            if (string.IsNullOrEmpty(channel)) channel = "Unknown";
            Debug.Log("Begin TalkingData Init!");
            TalkingDataSDK.BackgroundSessionEnabled();
            return TalkingDataSDK.Init(m_Config.appID, channel, Application.platform.ToString());
#else
            return false;
#endif
        }
        //------------------------------------------------------
        internal static void PayEvent(string userId, string orderId, int amount , string currenncyType, string payType, string itemId = null, int itemCnt =0)
        {
#if USE_TALKINGDATA
            if (!ms_bInited) return;
#if TD_GAME
            TalkingDataSDK.OnPay(userId, orderId, amount, currenncyType, payType, itemId, itemCnt);
#endif
#endif
        }
        //------------------------------------------------------
        internal static void LogEvent(string eventName)
        {
#if USE_TALKINGDATA
            if (!ms_bInited) return;
            if (string.IsNullOrEmpty(eventName))
                return;
            if (eventName.CompareTo("Pause") == 0)
            {
                TalkingDataSDK.OnPause();
                return;
            }
            if (eventName.CompareTo("ReceiveDeepLink") == 0)
            {
                TalkingDataSDK.OnReceiveDeepLink(SDK.GameSDK.GetEventV("url"));
                return;
            }
            if (eventName.CompareTo("ShowUI") == 0 || eventName.CompareTo("showui") == 0)
            {
                TalkingDataSDK.OnPageBegin(SDK.GameSDK.GetEventV("ui_name"));
            }
            else if (eventName.CompareTo("HideUI") == 0 || eventName.CompareTo("closeui") == 0)
            {
                TalkingDataSDK.OnPageEnd(SDK.GameSDK.GetEventV("ui_name"));
            }
            else if (eventName.CompareTo("AdTracking") == 0 || eventName.CompareTo("Ad") == 0)
            {
                TalkingDataSDK.OnReceiveDeepLink(SDK.GameSDK.GetEventV("posId"));
            }
            else if (eventName.CompareTo("Register") == 0 || eventName.CompareTo("user_register") == 0)
            {
                TalkingDataProfile profile = TalkingDataProfile.CreateProfile();
                profile.SetName(SDK.GameSDK.GetEventV("name"));
                profile.SetType(TalkingDataProfileType.ANONYMOUS);
                profile.SetGender(TalkingDataGender.UNKNOWN);
                TalkingDataSDK.OnRegister(SDK.GameSDK.GetEventV("uid"), profile, "");
            }
            else if (eventName.CompareTo("Login") == 0 || eventName.CompareTo("user_login") == 0)
            {
                TalkingDataProfile profile = TalkingDataProfile.CreateProfile();
                profile.SetName(SDK.GameSDK.GetEventV("name"));
                TalkingDataSDK.OnLogin(SDK.GameSDK.GetEventV("uid"), profile);
            }
            else if (eventName.CompareTo("DaySign") == 0 || eventName.CompareTo("sign_7day") == 0)
            {
                TalkingDataSDK.OnPunch(SDK.GameSDK.GetEventV("uid"), SDK.GameSDK.GetEventV("sign_day"));
            }
            else if (eventName.CompareTo("ChapterEnd") == 0 || eventName.CompareTo("battle_result") == 0)
            {
                TalkingDataSDK.OnLevelPass(SDK.GameSDK.GetEventV("uid"), SDK.GameSDK.GetEventV("battle_chapter"));

                var kvs = GameSDK.GetEventKVs();
                if (kvs == null || kvs.Count <= 0) return;
                if (ms_vEventTemp == null) ms_vEventTemp = new Dictionary<string, object>(1);
                ms_vEventTemp.Clear();
                foreach (var db in kvs)
                    ms_vEventTemp[db.Key] = db.Value;
                TalkingDataSDK.OnEvent(eventName, ms_vEventTemp);
                ms_vEventTemp.Clear();
            }
            else if (eventName.CompareTo("NoviceGuidance") == 0)
            {
                TalkingDataSDK.OnGuideFinished(SDK.GameSDK.GetEventV("uid"), SDK.GameSDK.GetEventV("id"));
            }
            else if (eventName.CompareTo("Task") == 0 || eventName.CompareTo("hero_task_process") == 0)
            {
                //ÕËºÅID£¬Ç©µ½ÄÚÈÝ
                TalkingDataSDK.OnAchievementUnlock(SDK.GameSDK.GetEventV("uid"), SDK.GameSDK.GetEventV("task_id"));
            }
            else 
            {
                var kvs = GameSDK.GetEventKVs();
                if (kvs == null || kvs.Count <= 0) return;
                if (ms_vEventTemp == null) ms_vEventTemp = new Dictionary<string, object>(1);
                ms_vEventTemp.Clear();
                foreach (var db in kvs)
                    ms_vEventTemp[db.Key] = db.Value;
                TalkingDataSDK.OnEvent(eventName, ms_vEventTemp);
                ms_vEventTemp.Clear();
            }
#endif
        }
    }
}