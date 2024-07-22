#if !UNITY_EDITOR && UNITY_IOS && USE_TOBID
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace SDK
{
    public class ToBidIOS
    {
        [DllImport("__Internal")]
        private static extern void toBidListener(string targetName);
        public static void txSetListener(string name)
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                toBidListener(name);
            }
        }

        [DllImport("__Internal")]
        private static extern void toBidInit(string appId, string appKey);
        public static void txInit(string appId, string appKey)
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                toBidInit(eventName, eventMsg);
            }
        }
		
		[DllImport("__Internal")]
        private static extern void setRuntimeData(string key, string strValue);
        public static void txSetRuntimeData(string key, string strValue)
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                setRuntimeData(key, strValue);
            }
        }
		
		[DllImport("__Internal")]
        private static extern void showAds(string type, string posID);
        public static void txShowAds(string type, string posID)
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                setRuntimeData(type, posID);
            }
        }
    }
}
#endif