#define TD_GAME
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct AIHelpCfg : ISDKConfig
    {
        public string appKey;
        public string domain;
        public string android_appId;
        public string ios_appid;
        public string GetAppID()
        {
#if UNITY_ANDROID
            return android_appId;
#elif UNITY_IPHONE
            return ios_appid;
#else
            return null;
#endif
        }
    }
    public class AIHelpUnity : ISDKAgent
    {
#if USE_AIHELP
        AIHelpCfg m_Config;
        ISDKAgent m_pAgent = null;
        static bool ms_bInited = false;
#endif
        //------------------------------------------------------
        public static AIHelpUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_AIHELP
            ms_bInited = false;
            AIHelpUnity talkingData = new AIHelpUnity();
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
        public static void ShowHelp(string enter)
        {
#if USE_AIHELP
            if (ms_bInited)
            {
                AIHelp.AIHelpSupport.Show(enter);
            }
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_AIHELP
            m_Config = (AIHelpCfg)cfg;
            if (string.IsNullOrEmpty(m_Config.GetAppID()))
            {
                Debug.Log("AIHelp init failed, appid is null");
                return false;
            }
            Debug.Log("AIHelp begin init");
            AIHelp.AIHelpSupport.Init(m_Config.appKey, m_Config.domain, m_Config.GetAppID(), GetSystemLanuage());
            AIHelp.AIHelpSupport.SetOnAIHelpInitializedCallback(OnAIHelpInitializedCallback);
            return true;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        void OnAIHelpInitializedCallback()
        {
            Debug.Log("AIHelp init Ok");
        }
        //------------------------------------------------------
        public string GetSystemLanuage()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English: return "en";
                case SystemLanguage.Belarusian: return "ru"; //∂˚¬ﬁÀπ
                case SystemLanguage.Japanese: return "ja"; //»’±æ
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified: return "zh-CH";
                case SystemLanguage.ChineseTraditional: return "zh-TW";
                case SystemLanguage.Arabic: return "ar";
                case SystemLanguage.German: return "de";    //µ¬”Ô
                case SystemLanguage.French: return "fr";    //∑®”Ô
                case SystemLanguage.Korean: return "ko";    //∫´”Ô
                case SystemLanguage.Portuguese: return "pt";    //∆œÃ——¿”Ô
                case SystemLanguage.Thai: return "th";    //Ã©”Ô
                case SystemLanguage.Turkish: return "tr";    //Õ¡∂˙∆‰”Ô°£
                case SystemLanguage.Indonesian: return "id";    //”°∂»ƒ·Œ˜—«”Ô
                case SystemLanguage.Spanish: return "es";    //Œ˜∞‡—¿”Ô
                case SystemLanguage.Vietnamese: return "vi";    //‘Ωƒœ”Ô
                case SystemLanguage.Italian: return "it";    //“‚¥Û¿˚”Ô
                case SystemLanguage.Polish: return "pl";    //≤®¿º”Ô
                case SystemLanguage.Dutch: return "nl";    //∫…¿º”Ô
                case SystemLanguage.Faroese: return "fa";    //≤®Àπ”Ô
                case SystemLanguage.Romanian: return "ro";    //¬ﬁ¬Ìƒ·—«”Ô
                case SystemLanguage.Estonian: return "tl";    //∑∆¬…±ˆ”Ô
                case SystemLanguage.Czech: return "cs";    //Ω›øÀ”Ô
                case SystemLanguage.Greek: return "el";    //œ£¿∞”Ô
                case SystemLanguage.Hungarian: return "hu";    //–Ÿ—¿¿˚”Ô
                case SystemLanguage.Swedish: return "sv";    //»µ‰”Ô
                case SystemLanguage.Hebrew: return "hi";    //”°µÿ”Ô
                case SystemLanguage.Norwegian: return "nb";    //≈≤Õ˛”Ô
//              case SystemLanguage.xxx: return "te";    //Ã©¬¨πÃ”Ô
//              case SystemLanguage.xxx: return "bn";    //√œº”¿≠”Ô
//              case SystemLanguage.xxx: return "ta";    //Ã©√◊∂˚”Ô
//              case SystemLanguage.xx: return "ms";    //¬Ì¿¥”Ô
//              case SystemLanguage.Bulgarian: return "my";    //√ÂµÈ”Ô
            }
            return "en";
        }
    }
}