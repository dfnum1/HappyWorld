using UnityEngine;

namespace SDK
{
    public class Bugly : ISDKAgent
    {
        [System.Serializable]
        public struct BuglyConfig : ISDKConfig
        {
            public string BuglyAppIDForiOS;
            public string BuglyAppIDForAndroid;
        }
        //------------------------------------------------------
        public static Bugly StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_BUGLY
            Bugly bugly = new Bugly();
            bugly.SetCallback(callback);
            if(bugly.Init(config)) return bugly;
            return null;
#else
            return null;
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
            BuglyConfig config = (BuglyConfig)cfg;
#if USE_BUGLY
#if DEBUG
            BuglyAgent.ConfigDebugMode(false);
#endif
#if UNITY_IPHONE || UNITY_IOS
        BuglyAgent.InitWithAppId (config.BuglyAppIDForiOS);
#elif UNITY_ANDROID
            BuglyAgent.InitWithAppId(config.BuglyAppIDForAndroid);
#endif
            BuglyAgent.EnableExceptionHandler();

            BuglyAgent.PrintLog(LogSeverity.LogInfo, "Init the bugly sdk");
#endif
            return true;
        }
        //------------------------------------------------------
        public static void SetCurrentScene(int id)
        {
#if USE_BUGLY
            BuglyAgent.SetCurrentScene(id);
#endif
        }
        //------------------------------------------------------
        public static void SetCurrentUserID(string id)
        {
#if USE_BUGLY
             BuglyAgent.SetUserId(id);
#endif
        }
    }
}
