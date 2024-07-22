#if USE_FIREBASE
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct FireBaseCfg : ISDKConfig
    {
        public string androidDevKey;
        public string iosDevKey;

        public bool isDebug;
        public bool getConversionData;

        public string getAppID()
        {
            return Application.identifier;
        }
        public string getDevKey()
        {
#if USE_FIREBASE
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
    public class FireBaseUnity : ISDKAgent
    {
#if USE_FIREBASE
        FireBaseCfg m_Config;

        DependencyStatus m_dependencyStatus = DependencyStatus.UnavailableOther;
        static protected bool ms_bFirebaseInitialized = false;
        ISDKAgent m_pAgent = null;
#endif
        //------------------------------------------------------
        public static FireBaseUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_FIREBASE
            ms_bFirebaseInitialized = false;
            FireBaseUnity fireBase = new FireBaseUnity();
            fireBase.SetCallback(callback);
            if (fireBase.Init(config))
            {
                fireBase.SetCallback(callback);
                return fireBase;
            }
            return null;
#else
            return null;
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_FIREBASE
            Debug.Log("Begin Google FireBase Analytics Init!");
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                m_dependencyStatus = task.Result;
                if (m_dependencyStatus == DependencyStatus.Available)
                {
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod,"StupidDogStudios");
                    // Set the user ID.
                //    FirebaseAnalytics.SetUserId("uber_user_510");
                    // Set default session duration values.
                    FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
                    ms_bFirebaseInitialized = true;

                    SetUserProperty("DeviceType", UnityEngine.SystemInfo.deviceModel);
                    SetUserProperty("DeviceModel", UnityEngine.SystemInfo.deviceName);
                    SetUserProperty("OperatingSystem", UnityEngine.SystemInfo.operatingSystem);

                    Debug.Log("Google FireBase Analytics Startup!");
                }
                else
                {
                    Debug.LogError(
                      "Could not resolve all Firebase dependencies: " + m_dependencyStatus);
                }
            });
            return true;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        internal static void SetUserId(string userId)
        {
#if USE_FIREBASE
            if (!ms_bFirebaseInitialized) return;
            FirebaseAnalytics.SetUserId(userId);
#endif
        }
        //------------------------------------------------------
        internal static void SetUserProperty(string name, string property)
        {
#if USE_FIREBASE
            if (!ms_bFirebaseInitialized) return;
            FirebaseAnalytics.SetUserProperty(name, property);
#endif
        }
        //------------------------------------------------------
        internal static void LogEvent(string eventName, string parameterName, string parameterValue, string parameterProp0=null)
        {
#if USE_FIREBASE
            if (!ms_bFirebaseInitialized) return;
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
#endif
        }
    }
}