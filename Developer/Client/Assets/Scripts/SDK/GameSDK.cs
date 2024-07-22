/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	SDK
作    者:	HappLI
描    述:	SDK 初始化类
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    public class GameSDK : TopGame.Base.Singleton<GameSDK>
    {
        static AndroidJavaObject ms_SDKHandle = null;
        private static Dictionary<string, double> ms_vPushStatistickDatas = null;
        private static List<SDKStatisticRank> ms_vStatisticRanks = null;
        private static Dictionary<string,string> ms_LogEventKeyValues = null;
        private static bool ms_HasSDKLogEvent = false;
        private static bool ms_HasSDKLogin = false;
        private static bool ms_HasCloudStats = false;
        private static bool ms_HasAntiAddiction = false;
        private static ISDKCallback ms_pCallback = null;
        List<ISDKAgent> m_vSDKs = new List<ISDKAgent>();
        //------------------------------------------------------
        public static void SetCallback(ISDKCallback callback)
        {
            ms_pCallback = callback;
        }
#if UNITY_ANDROID
        //------------------------------------------------------
        internal static AndroidJavaObject GetSDKHandler()
        {
            return ms_SDKHandle;
        }
#endif
        //------------------------------------------------------
        public void Awake(SDKSetting setting, MonoBehaviour monoMain)
        {
            if (setting == null) return;
            try
            {
#if UNITY_ANDROID
                ms_SDKHandle = new AndroidJavaObject("com.unity.sdks.SDKHandler");
                if (ms_SDKHandle == null)
                {
                    Debug.LogWarning("unfind \"com.unity.sdks.SDKHandler\"");
                }
#endif
            }
            catch(System.Exception ex)
            {
                Debug.LogWarning(ex.ToString());
            }

            SDKParam bugly = setting.GetSDK("bugly");
            if (bugly != null)
            {
                Bugly.BuglyConfig cfg = new Bugly.BuglyConfig();
                cfg.BuglyAppIDForAndroid = bugly.GetString("android");
                cfg.BuglyAppIDForiOS = bugly.GetString("ios");
                Bugly sdk = Bugly.StartUp(cfg, ms_pCallback);
                if (sdk != null) m_vSDKs.Add(sdk);
            }

            SDKParam appsFlyer = setting.GetSDK("AppsFlyer");
            if (appsFlyer != null)
            {
                AppsFlyerCfg cfg = new AppsFlyerCfg();
                cfg.androidDevKey = appsFlyer.GetString("androidDevKey");
                cfg.iosDevKey = appsFlyer.GetString("iosDevKey");
                cfg.isDebug = appsFlyer.GetInt("isDebug") != 0;
                cfg.getConversionData = appsFlyer.GetInt("getConversionData") != 0;
                cfg.binderMono = monoMain;
                AppsFlyerUnity sdk = AppsFlyerUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
                    ms_HasSDKLogEvent = true;
                }
            }
            SDKParam fireBase = setting.GetSDK("FireBase");
            if (fireBase != null)
            {
                FireBaseCfg cfg = new FireBaseCfg();
                FireBaseUnity sdk = FireBaseUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
                    ms_HasSDKLogEvent = true;
                }
            }
            SDKParam talkingData = setting.GetSDK("TalkingData");
            if (talkingData != null)
            {
                TalkingDataCfg cfg = new TalkingDataCfg();
                cfg.appID = talkingData.GetString("TALKING_APPID");
                cfg.channel_ios = talkingData.GetString("Channel_IOS");
                cfg.channel_android = talkingData.GetString("Channel_Android");
                TalkingDataUnity sdk = TalkingDataUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
                    ms_HasSDKLogEvent = true;
                }
            }
            SDKParam gameAnalytics = setting.GetSDK("GameAnalytics");
            if (gameAnalytics != null)
            {
                GameAnalyticsCfg cfg = new GameAnalyticsCfg();
                cfg.user = gameAnalytics.GetString("user");
                cfg.password = gameAnalytics.GetString("password");
                cfg.pMainGO = monoMain.gameObject;
                GameAnalyticsUnity sdk = GameAnalyticsUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
                    ms_HasSDKLogEvent = true;
                }
            }



            SDKParam aiHelp = setting.GetSDK("AIHelp");
            if (aiHelp != null)
            {
                AIHelpCfg cfg = new AIHelpCfg();
                cfg.appKey = aiHelp.GetString("appKey");
                cfg.domain = aiHelp.GetString("domain");
                cfg.android_appId = aiHelp.GetString("android_appId");
                cfg.ios_appid = aiHelp.GetString("ios_appId");
                AIHelpUnity sdk = AIHelpUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null) m_vSDKs.Add(sdk);
            }

            SDKParam admobile = setting.GetSDK("Admobile");
            if (admobile != null)
            {
                AdmobileCfg cfg = new AdmobileCfg();
                cfg.pMainGO = monoMain.gameObject;
                AdmobileUnity sdk = AdmobileUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null) m_vSDKs.Add(sdk);
            }

            SDKParam toBid = setting.GetSDK("ToBid");
            if (toBid != null)
            {
                ToBidCfg cfg = new ToBidCfg();
                cfg.pMainGO = monoMain.gameObject;
                ToBidUnity sdk = ToBidUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null) m_vSDKs.Add(sdk);
            }

            {
                AdvConfig advCfg = new AdvConfig();
                SDKParam googleAd = setting.GetSDK("googleAd");
                if (googleAd != null)
                {
                    advCfg.googleAd = new GoogleAdCfg();
                    advCfg.googleAd.andoridID = googleAd.GetString("androidID");
                    advCfg.googleAd.rewardedAndroidID = googleAd.GetString("rewardedAndroidID");
                    advCfg.googleAd.iosID = googleAd.GetString("iosID");
                    advCfg.googleAd.rewardedIosID = googleAd.GetString("rewardedIosID");
                }

                SDKParam unityAd = setting.GetSDK("unityAd");
                if (unityAd != null)
                {
                    advCfg.unityAd = new UnityAdCfg();
                    advCfg.unityAd.andoridID = unityAd.GetString("androidID");
                    advCfg.unityAd.iosID = unityAd.GetString("iosID");
                    advCfg.unityAd.videoPlacement = unityAd.GetString("videoPlacement");
                    advCfg.unityAd.testMode = unityAd.GetInt("testMode") != 0;
                }

                SDKParam facebookAd = setting.GetSDK("facebookAd");
                if (facebookAd != null)
                {
                    advCfg.facebookAd = new FacebookAdCfg();
                    advCfg.facebookAd.andoridID = facebookAd.GetString("androidID");
                    advCfg.facebookAd.iosID = facebookAd.GetString("iosID");
                }
                Advertising sdk = Advertising.StartUp(advCfg, ms_pCallback);
                if (sdk != null) m_vSDKs.Add(sdk);
            }
        }
        //
        public void Start(SDKSetting setting, MonoBehaviour monoMain)
        {
            if(!TopGame.Core.VersionData.popPrivacy)
            {
                StartBegin(setting, monoMain);
                return;
            }
            List<SDKParam> Policys = new List<SDKParam>();
#if USE_QUICKSDK
            SDKParam quickSDK = setting.GetSDK("QuickSDK");
            if (quickSDK != null) Policys.Add(quickSDK);
#endif
#if USE_TALKINGDATA
            SDKParam TalkingData = setting.GetSDK("TalkingData");
            if (TalkingData != null) Policys.Add(TalkingData);
#endif
#if USE_INNER_QUICKSDK
            SDKParam innerQuickSDK = setting.GetSDK("InnerQuickSDK");
            if (innerQuickSDK != null) Policys.Add(innerQuickSDK);
#endif
#if USE_TAPTAP
            SDKParam taptap = setting.GetSDK("Taptap");
            if (taptap != null)Policys.Add(taptap);
#endif
#if USE_OPPO
            SDKParam oppo = setting.GetSDK("Oppo");
            if (oppo != null)Policys.Add(oppo);
#endif
#if USE_VIVO
            SDKParam vivo = setting.GetSDK("Vivo");
            if (vivo != null)Policys.Add(vivo);
#endif
#if USE_XIAOMI
            SDKParam xiaomi = setting.GetSDK("Xiaomi");
            if (xiaomi != null)Policys.Add(xiaomi);
#endif
#if USE_HUAWEI
            SDKParam huawei = setting.GetSDK("Huawei");
            if (huawei != null) Policys.Add(huawei);
#endif
#if USE_YINGYONGBAO
            SDKParam yingyongbao = setting.GetSDK("Yingyongbao");
            if (yingyongbao != null) Policys.Add(yingyongbao);
#endif
#if USE_APPLE
            SDKParam apple = setting.GetSDK("Apple");
            if (apple != null) Policys.Add(apple);
#endif
#if USE_LZT
            SDKParam LZT = setting.GetSDK("LZT");
            if (LZT != null) Policys.Add(LZT);
#endif
#if USE_ZSSDK
            SDKParam zssdk = setting.GetSDK("ZSSdk");
            if (zssdk != null) Policys.Add(zssdk);
#endif
#if USE_OPERATE
            SDKParam operate = setting.GetSDK("Operate");
            if (operate != null) Policys.Add(operate);
#endif
#if USE_233SDK
            SDKParam sdk233 = setting.GetSDK("233");
            if (sdk233 != null) Policys.Add(sdk233);
#endif
            System.Text.StringBuilder PolicySB = null;
            for (int i = 0; i < Policys.Count; ++i)
            {
                string Policy = Policys[i].GetString("Policy");
                if (!string.IsNullOrEmpty(Policy))
                {
                    var text = Resources.Load<TextAsset>(Policy);
                    if (text != null && !string.IsNullOrEmpty(text.text))
                    {
                        if (PolicySB == null) PolicySB = new System.Text.StringBuilder();
                        PolicySB.AppendLine(text.text);
                    }
                    if (text) Resources.UnloadAsset(text);
                }
            }
            if (PolicySB != null && PolicySB.Length > 0)
            {
                int PolicyFlag = PlayerPrefs.GetInt(Application.identifier + "_Policy", 0);
                if(PolicyFlag<=0)
                {

                    PolicyCallback callback = monoMain.gameObject.AddComponent<PolicyCallback>();
                    callback.setting = setting;
                    callback.monoMain = monoMain;
                    if(!TopGame.Core.JniPlugin.ShowAlertDialog("", PolicySB.ToString(), TopGame.Base.GlobalUtil.ToLocalization(80010239, "Yes"),
                        TopGame.Base.GlobalUtil.ToLocalization(80010240, "No"), callback.gameObject))
                        StartBegin(setting, monoMain);
                }
                else
                    StartBegin(setting, monoMain);
            }
            else
            {
                StartBegin(setting, monoMain);
            }
        }
        //------------------------------------------------------
        class PolicyCallback : MonoBehaviour
        {
            public SDKSetting setting;
            public MonoBehaviour monoMain;
            void OnAlertDlgCallback(string msg)
            {
                if("yes".CompareTo(msg) == 0)
                {
                    PlayerPrefs.SetInt(Application.identifier + "_Policy", 1);
                    GameSDK.getInstance().StartBegin(setting, monoMain);
                }
                else
                {
                    PlayerPrefs.SetInt(Application.identifier + "_Policy", 0);
                    Application.Quit();
                }
            }
        }
        //------------------------------------------------------
        public void StartBegin(SDKSetting setting, MonoBehaviour monoMain)
        {
            SDKParam quickSDK = setting.GetSDK("QuickSDK");
            if (quickSDK != null)
            {
                QuickSDKCfg cfg = new QuickSDKCfg();
                cfg.productCode = quickSDK.GetString("QUICK_PRODUCT_CODE");
                cfg.productKey = quickSDK.GetString("QUICK_PRODUCT_KEY");
                cfg.assetFile = quickSDK.GetString("Asset");
                cfg.pMainGO = monoMain.gameObject;
                UnityQuickSDK sdk = UnityQuickSDK.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
                    ms_HasSDKLogin = true;
                    ms_HasSDKLogEvent = true;
                }
            }

            SDKParam innerQuickSDK = setting.GetSDK("InnerQuickSDK");
            if (innerQuickSDK != null)
            {
                InnerQuickSDKCfg cfg = new InnerQuickSDKCfg();
                cfg.productCode = innerQuickSDK.GetString("QUICK_PRODUCT_CODE");
                cfg.productKey = innerQuickSDK.GetString("QUICK_PRODUCT_KEY");
                cfg.assetFile = innerQuickSDK.GetString("Asset");
                cfg.pMainGO = monoMain.gameObject;
                InnerQuickSDKUnity sdk = InnerQuickSDKUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
                    ms_HasSDKLogin = true;
                    ms_HasSDKLogEvent = true;
                }
            }

            SDKParam taptap = setting.GetSDK("Taptap");
            if (taptap != null)
            {
                TaptapCfg cfg = new TaptapCfg();
                cfg.clientID = taptap.GetString("ClientID");
                cfg.clientToken = taptap.GetString("ClientToken");
                cfg.serverUrl = taptap.GetString("ServerUrl");
                cfg.testQualification = taptap.GetInt("testQualification") !=0;
                cfg.assetFile = taptap.GetString("Asset");
                cfg.binderMono = monoMain;
                TaptapUnity sdk = TaptapUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
#if USE_TAPTAP
                    ms_HasSDKLogin = true;
#endif
                    ms_HasCloudStats = true;
                    ms_HasSDKLogEvent = true;
                    ms_HasAntiAddiction = true;
                }
            }

            SDKParam oppo = setting.GetSDK("Oppo");
            if (oppo != null)
            {
                OppoCfg cfg = new OppoCfg();
                cfg.appKey = oppo.GetString("appKey");
                cfg.appSecret = oppo.GetString("appSecret");
                cfg.assetFile = oppo.GetString("Asset");
                cfg.binderMono = monoMain;
                OppoUnity sdk = OppoUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
#if USE_OPPO
                    ms_HasSDKLogin = true;
#endif
                }
            }

            SDKParam vivo = setting.GetSDK("Vivo");
            if (vivo != null)
            {
                VivoCfg cfg = new VivoCfg();
                cfg.appID = vivo.GetString("appId");
                cfg.appKey = vivo.GetString("appKey");
                cfg.cpId = vivo.GetString("cpId");
                cfg.assetFile = vivo.GetString("Asset");
                cfg.passPrivacy = vivo.GetString("passPrivacy", "").CompareTo("1") == 0;
                cfg.monoBinder = monoMain;
                VivoUnity sdk = VivoUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
#if USE_VIVO
                    ms_HasSDKLogin = true;
#endif
                }
            }

            SDKParam xiaomi = setting.GetSDK("Xiaomi");
            if (xiaomi != null)
            {
                XiaomiCfg cfg = new XiaomiCfg();
                cfg.appKey = xiaomi.GetString("mi_game_appkey");
                cfg.appId = xiaomi.GetString("mi_game_appid");
                cfg.passPrivacy = xiaomi.GetString("passPrivacy","").CompareTo("1") == 0;
                cfg.assetFile = xiaomi.GetString("Asset");
                XiaomiUnity sdk = XiaomiUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
#if USE_XIAOMI
                    ms_HasSDKLogin = true;
#endif
                }
            }

            SDKParam huawei = setting.GetSDK("Huawei");
            if (huawei != null)
            {
                HuaweiCfg cfg = new HuaweiCfg();
                cfg.assetFile = huawei.GetString("Asset");
                cfg.binderMono = monoMain;
                HuaweiUnity sdk = HuaweiUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
#if USE_HUAWEI
                    ms_HasSDKLogin = true;
#endif
                }
            }

            SDKParam yingyongbao = setting.GetSDK("Yingyongbao");
            if (yingyongbao != null)
            {
                YingyongbaoCfg cfg = new YingyongbaoCfg();
                cfg.assetFile = yingyongbao.GetString("Asset");
                cfg.monoBinder = monoMain;
                YingyongbaoUnity sdk = YingyongbaoUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
#if USE_YINGYONGBAO
                    ms_HasSDKLogin = true;
#endif
                }
            }

            SDKParam apple = setting.GetSDK("Apple");
            if (apple != null)
            {
                AppleCfg cfg = new AppleCfg();
                cfg.monoObject = monoMain.gameObject;
                cfg.assetFile = apple.GetString("Asset");
                cfg.useAuth = apple.GetInt("useAuth") != 0;
                AppleUnity sdk = AppleUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
#if USE_APPLE
                    if(cfg.useAuth) ms_HasSDKLogin = true;
#endif
                }
            }

            SDKParam LZT = setting.GetSDK("LZT");
            if (LZT != null)
            {
                LZTCfg cfg = new LZTCfg();
                cfg.assetFile = LZT.GetString("Asset");
                cfg.appId = LZT.GetString("AppId");
                cfg.appKey = LZT.GetString("AppKey");
                cfg.monoBinder = monoMain;
                LZTUnity sdk = LZTUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
#if USE_LZT
                    ms_HasSDKLogin = true;
#endif
                }
            }

            SDKParam operate = setting.GetSDK("Operate");
            if (operate != null)
            {
                OperateCfg cfg = new OperateCfg();
                cfg.appKey = apple.GetString("appKey");
                cfg.assetFile = apple.GetString("Asset");
                cfg.binderMono = monoMain;
                OperateUnity sdk = OperateUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
#if USE_OPERATE
                    ms_HasSDKLogin = true;
#endif
                }
            }

            SDKParam zssdk = setting.GetSDK("ZSSdk");
            if (zssdk != null)
            {
                ZsCfg cfg = new ZsCfg();
                cfg.assetFile = apple.GetString("Asset");
                cfg.binderMono = monoMain;
                ZsUnity sdk = ZsUnity.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
#if USE_ZSSDK
                    ms_HasSDKLogin = true;
#endif
                }
            }

#if USE_233SDK
            SDKParam sdk233 = setting.GetSDK("233");
            if (sdk233 != null)
            {
                ZsCfg cfg = new ZsCfg();
                cfg.assetFile = apple.GetString("Asset");
                cfg.binderMono = monoMain;
                Unity233 sdk = Unity233.StartUp(cfg, ms_pCallback);
                if (sdk != null)
                {
                    m_vSDKs.Add(sdk);
                    ms_HasSDKLogin = true;
                }
            }
#endif
            Debug.Log("sdk channel:" + GetPayChannel());
        }
        //------------------------------------------------------
        public void Update()
        {
            float fTime = Time.deltaTime;
            for (int i = 0; i < m_vSDKs.Count; ++i)
            {
                m_vSDKs[i].Update(fTime);
            }
        }
        //------------------------------------------------------
        internal static void DoCallback(ISDKAgent agent, ISDKParam callParam)
        {
            if(callParam is SDKCallbackParam)
            {
                var temp = (SDKCallbackParam)callParam;
                if (temp.action == ESDKActionType.StatisticsRank)
                {
                    temp.statisticRanks = ms_vStatisticRanks;
                    callParam = temp;
                }
            }

            if (ms_pCallback != null)
                ms_pCallback.OnSDKAction(agent, callParam);
            if (callParam is SDKCallbackParam)
            {
                var temp = (SDKCallbackParam)callParam;
                if (temp.action == ESDKActionType.StatisticsRankUpdate)
                {
                    if (ms_vPushStatistickDatas != null) ms_vPushStatistickDatas.Clear();
                }
            }

        }
        //------------------------------------------------------
        public static void AddEventKV(string key, string value, bool bClear = false)
        {
            // if (!ms_HasSDKLogEvent || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) return;
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) return;
            if (ms_LogEventKeyValues == null) ms_LogEventKeyValues = new Dictionary<string, string>();
            if (bClear) ms_LogEventKeyValues.Clear();
            ms_LogEventKeyValues[key] = value;
        }
        //------------------------------------------------------
        public static string GetEventV(string key)
        {
            if (!ms_HasSDKLogEvent || ms_LogEventKeyValues == null) return string.Empty;
            string value;
            if (ms_LogEventKeyValues.TryGetValue(key, out value)) return value;
            return string.Empty;
        }
        //------------------------------------------------------
        internal static Dictionary<string,string> GetEventKVs()
        {
            return ms_LogEventKeyValues;
        }
        //------------------------------------------------------
        internal static void AddStatisticRank(string uid, string name, double value, long rank =0, bool bClear = false)
        {
            if (ms_vStatisticRanks == null) ms_vStatisticRanks = new List<SDKStatisticRank>();
            if(bClear) ms_vStatisticRanks.Clear();
            if (rank == 0) rank = ms_vStatisticRanks.Count+1;
            ms_vStatisticRanks.Add(new SDKStatisticRank(uid,name, value, rank));
        }
        //------------------------------------------------------
        public static bool HasLogEvent()
        {
#if USE_FIREBASE || USE_QUICKSDK || USE_TALKINGDATA || USE_TAPTAP || USE_GAMEANALYTICS
            return ms_HasSDKLogEvent;
#endif
            return false;
        }
        //------------------------------------------------------
        public static void LogEvent(string eventName)
        {
            if (!HasLogEvent())
            {
                //没有接入sdk时走输出
                if (ms_LogEventKeyValues != null)
                {
                    string str = "";
                    foreach (KeyValuePair<string,string> msLogEventKeyValue in ms_LogEventKeyValues)
                    {
                        str +=  msLogEventKeyValue.Key + ":" + msLogEventKeyValue.Value + "\n";
                    }
                    Framework.Plugin.Logger.Warning("AddEventUtil:eventName: " + eventName + "\n" + str);
                    ms_LogEventKeyValues.Clear();
                }
                return;
            }
#if USE_FIREBASE
            if (!ms_bFirebaseInitialized) return;
            FireBaseUnity.LogEvent(eventName);
#endif
#if USE_QUICKSDK
            UnityQuickSDK.LogEvent(eventName);
#endif
#if USE_TALKINGDATA
            TalkingDataUnity.LogEvent(eventName);
#endif
#if USE_THINKINGDATA
            ThinkingDataUnity.LogEvent(eventName);
#endif
#if USE_TAPTAP
            TaptapUnity.LogEvent(eventName);
#endif
#if USE_GAMEANALYTICS
            GameAnalyticsUnity.LogEvent(eventName);
#endif
            if (ms_LogEventKeyValues !=null) ms_LogEventKeyValues.Clear();
        }
        //------------------------------------------------------
        public static string GetPayChannel(bool appendDevice = true)
        {
            var channel = "";
#if USE_OPERATE
            channel = "operate";
#elif USE_INNER_QUICKSDK
            channel = InnerQuickSDKUnity.GetChannelName();
// #elif USE_TAPTAP
//             channel = "taptap";
#elif USE_LZT
            channel = "lzt";
#elif USE_ZSSDK
            channel = "zssdk";
#elif USE_233SDK
            channel = "233";
#elif USE_APPLE
			channel = "apple";
#else
            channel = "default";
#endif
            if(appendDevice)
            {
#if UNITY_ANDROID
                channel += "_android";
#elif UNITY_IOS   
            channel += "_ios";
#endif
            }

            return channel;
        }
        //------------------------------------------------------
        public static void ClearBuying()
        {
#if USE_APPLE
            AppleUnity.ClearApplePayInfo();
#endif
        }
        //------------------------------------------------------
        public static bool IsBuying()
        {
#if USE_APPLE
            return AppleUnity.IsBuying();
#else
            return false;
#endif
        }
        //------------------------------------------------------
        public static bool HasPaySDK()
        {
#if (USE_OPERATE || USE_INNER_QUICKSDK || USE_LZT || USE_ZSSDK || USE_233SDK || USE_APPLE)
            return true;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        public static bool Pay(string userId, string orderId, double amount, string goodsId, string goodsName, string googdsDesc, string externParam, string callbackUrl)
        {
#if USE_OPERATE
            return OperateUnity.Pay(orderId, amount, goodsName, externParam,callbackUrl);
#elif USE_INNER_QUICKSDK
            return InnerQuickSDKUnity.Pay(userId, orderId, amount, goodsId, goodsName, googdsDesc, externParam, callbackUrl);
#elif USE_TAPTAP
            return TaptapUnity.Pay(userId, orderId, amount, goodsId, goodsName, googdsDesc, externParam, callbackUrl);
#elif USE_LZT
            return LZTUnity.Pay(orderId, goodsId, amount, goodsName, "1", "个", externParam, callbackUrl);
#elif USE_ZSSDK
            return ZsUnity.Pay(userId,orderId,goodsId,amount,goodsName,externParam,callbackUrl);
#elif USE_233SDK
            return Unity233.Pay(userId,orderId,goodsId,amount,goodsName,externParam,callbackUrl);
#elif USE_APPLE
			return AppleUnity.Pay(userId,orderId,goodsId,amount,goodsName,externParam,callbackUrl);
#else
            return false;
#endif
        }
		//------------------------------------------------------
		public static void InitProdocts(List<string> products)
		{
#if USE_APPLE
			AppleUnity.InitProdocts(products);
#endif			
		}
        //------------------------------------------------------
        public static void PayLog(string userId, string orderId, int amount, string currenncyType, string payType, string itemId = null, int itemCnt = 0)
        {
#if USE_TALKINGDATA
            TalkingDataUnity.PayEvent(userId, orderId, amount, currenncyType, payType, itemId, itemCnt);
#endif
#if USE_GAMEANALYTICS
            GameAnalyticsUnity.PayEvent(userId, orderId, amount, currenncyType, payType, itemId, itemCnt);
#endif
        }
        //------------------------------------------------------
        public static bool HasLoginSDK()
        {
            return ms_HasSDKLogin;
        }
        //------------------------------------------------------
        public static bool Login(string platform="auto")
        {
            if (!ms_HasSDKLogin) return false;
#if USE_TAPTAP
            TaptapUnity.Login(platform);
            return true;
#elif USE_OPPO
            OppoUnity.Login();
            return true;
#elif USE_VIVO
            VivoUnity.Login();
            return true;
#elif USE_XIAOMI
            XiaomiUnity.Login();
            return true;
#elif USE_YINGYONGBAO
            YingyongbaoUnity.Login(platform);
            return true;
#elif USE_HUAWEI
            HuaweiUnity.Login();
            return true;
#elif USE_OPERATE
            OperateUnity.Login();
            return true;
#elif USE_INNER_QUICKSDK
            InnerQuickSDKUnity.Login();
            return true;
#elif USE_LZT
            LZTUnity.Login();
            return true;
#elif USE_ZSSDK
            ZsUnity.Login();
            return true;
#elif USE_233SDK
            Unity233.Login();
            return true;
#elif USE_APPLE
            AppleUnity.Login();
            return true;			
#endif
            return false;
        }
        //------------------------------------------------------
        public static bool Logout()
        {
            if (!ms_HasSDKLogin) return false;
#if USE_TAPTAP
            TaptapUnity.Logout();
              return true;
#elif USE_OPPO
            OppoUnity.Logout();
            return true;
#elif USE_VIVO
            VivoUnity.Logout();
            return true;
#elif USE_XIAOMI
            XiaomiUnity.Logout();
            return true;
#elif USE_YINGYONGBAO
            YingyongbaoUnity.Logout();
            return true;
#elif USE_HUAWEI
            HuaweiUnity.Logout();
            return true;
#elif USE_OPERATE
            OperateUnity.Logout();
            return true;
#elif USE_INNER_QUICKSDK
            InnerQuickSDKUnity.Logout();
            return true;
#elif USE_LZT
            LZTUnity.Logout();
            return true;
#elif USE_ZSSDK
            ZsUnity.Logout();
            return true;
#elif USE_233SDK
            Unity233.Logout();
            return true;
#elif USE_APPLE
            AppleUnity.Logout();
            return true;			
#endif
            return false;
        }
        //------------------------------------------------------
        public static bool Exit()
        {
#if USE_TAPTAP
            TaptapUnity.Logout();
              return true;
#elif USE_OPPO
            OppoUnity.Logout();
            return true;
#elif USE_VIVO
            VivoUnity.Logout();
            return true;
#elif USE_XIAOMI
            XiaomiUnity.Logout();
            return true;
#elif USE_YINGYONGBAO
            YingyongbaoUnity.Logout();
            return true;
#elif USE_HUAWEI
            HuaweiUnity.Logout();
            return true;
#elif USE_OPERATE
            OperateUnity.Logout();
            return true;
#elif USE_INNER_QUICKSDK
            InnerQuickSDKUnity.Exit();
            return true;
#elif USE_ZSSDK
            ZsUnity.Exit();
            return true;
#elif USE_233SDK
            Unity233.Exit();
            return true;
#endif
            return false;
        }
        //------------------------------------------------------
        public static void UpdateGameData(string userId, string nickName, int level, string serverName, bool bCreate, bool bEnter)
        {
            if (string.IsNullOrEmpty(serverName)) serverName = "0";
            if (string.IsNullOrEmpty(nickName)) nickName = userId;
#if USE_INNER_QUICKSDK
                if(bCreate) SDK.InnerQuickSDKUnity.CreateUser(userId, nickName, level, serverName);
                else SDK.InnerQuickSDKUnity.UpdateUser(userId, nickName, level, serverName);
                if (bEnter) SDK.InnerQuickSDKUnity.EnterGame(userId.ToString(), nickName, level, serverName);
#endif
#if USE_LZT
                if(bCreate || bEnter) SDK.LZTUnity.SetRoleInfo("0", serverName, userId, nickName,level.ToString());
                else  SDK.LZTUnity.UpdateRole(userId.ToString(), level.ToString());
#endif
#if USE_ZSSDK
                SDK.ZsUnity.UpdateRole(userId, nickName, serverName, "0", level.ToString(), bCreate);
#endif
#if USE_TAPTAP
            SDK.TaptapUnity.UpdateRole("0", serverName, userId, nickName,level.ToString(), bCreate);
#endif
#if USE_FIREBASE
            FireBaseUnity.SetUserId(userId);
#endif
#if USE_GAMEANALYTICS
            GameAnalyticsUnity.SetUserId(userId);
#endif
        }
        //------------------------------------------------------
        public static void ShowAds(EAdType type, string posId, string userId)
        {
            SetRuntimeData("user_id", userId);
#if USE_ADMOBILE
            AdmobileUnity.ShowAd(type.ToString(), posId);
#elif USE_TOBID
            ToBidUnity.ShowAd(type.ToString(), posId);
#elif USE_233SDK
            Unity233.ShowAd(type.ToString(), posId);
#endif
        }
        //------------------------------------------------------
        public static bool HasHasAntiAddiction()
        {
            return ms_HasAntiAddiction;
        }
        //------------------------------------------------------
        public static void StartAntiAddiction()
        {
#if USE_TAPTAP
            TaptapUnity.StartAntiAddiction();
#endif
        }
        //------------------------------------------------------
        public static void LeaveAntiAddiction()
        {
#if USE_TAPTAP
            TaptapUnity.LeaveAntiAddiction();
#endif
        }
        //------------------------------------------------------
        public static void ExitAntiAddiction()
        {
#if USE_TAPTAP
            TaptapUnity.ExitAntiAddiction();
#endif
        }
        //------------------------------------------------------
        public static int GetAntiAddictionReaminningTime()
        {
#if USE_TAPTAP
            return TaptapUnity.GetAntiAddictionReaminningTime();
#endif
            return 0;
        }
        //------------------------------------------------------
        public static void AntiAddictionIdentifier(string userIdentifier)
        {
#if USE_TAPTAP
            TaptapUnity.AntiAddictionIdentifier(userIdentifier);
#endif
        }
        //------------------------------------------------------
        public static bool HasCloudStatistics()
        {
            return ms_HasCloudStats;
        }
        //------------------------------------------------------
        public static void CloudStatistics(string group, int rankTopCount = 10)
        {
#if USE_TAPTAP
            TaptapUnity.CloudStatistics(group, rankTopCount);
#endif
        }
        //------------------------------------------------------
        public static void SearchCloudStatistics(string uid, string group)
        {
#if USE_TAPTAP
            TaptapUnity.SearchCloudStatistics(uid, group);
#endif
        }
        //------------------------------------------------------
        public static void AddStatisticData(string group, double value, bool bClear =true)
        {
            if (!ms_HasCloudStats) return;
            if (ms_vPushStatistickDatas == null) ms_vPushStatistickDatas = new Dictionary<string, double>();
            if (bClear) ms_vPushStatistickDatas.Clear();
             ms_vPushStatistickDatas[group] = value;
        }
        //------------------------------------------------------
        public static void UpdateStatistic(string uid)
        {
            if (!ms_HasCloudStats) return;
            if (ms_vPushStatistickDatas == null || ms_vPushStatistickDatas.Count <= 0) return;
#if USE_TAPTAP
            TaptapUnity.UpdateStatistic(uid, ms_vPushStatistickDatas);
#endif
        }
        //------------------------------------------------------
        public static void SetRuntimeData(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;
#if UNITY_ANDROID
            if(ms_SDKHandle!=null)
            {
                ms_SDKHandle.CallStatic("SetRuntimeData", key, value);
            }
#endif
        }
        //------------------------------------------------------
        public static void ShowFloatMenu(bool bShow)
        {

#if USE_INNER_QUICKSDK
            InnerQuickSDKUnity.ShowFloatMenu(bShow);
#endif
#if UNITY_ANDROID
            if (ms_SDKHandle != null)
            {
#if USE_OPPO
            ms_SDKHandle.CallStatic("ShowFloatMenu", "oppo", bShow);
#elif USE_VIVO
            ms_SDKHandle.CallStatic("ShowFloatMenu", "vivo", bShow);
#elif USE_XIAOMI
            ms_SDKHandle.CallStatic("ShowFloatMenu", "xiaomi", bShow);
#elif USE_YINGYONGBAO
            ms_SDKHandle.CallStatic("ShowFloatMenu", "yingyongbao", bShow);
#elif USE_HUAWEI
            ms_SDKHandle.CallStatic("ShowFloatMenu", "huawei", bShow);
#elif USE_OPERATE
            ms_SDKHandle.CallStatic("ShowFloatMenu", "operate", bShow);
#elif USE_ZSSDK
            //ms_SDKHandle.CallStatic("ShowFloatMenu", "zs", bShow);
#elif USE_233SDK
            ms_SDKHandle.CallStatic("ShowFloatMenu", "233", bShow);
#endif
            }
#endif
        }
    }
}
