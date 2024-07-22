using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
namespace SDK
{
    struct LZTCfg : ISDKConfig
    {
        public string appId;
        public string appKey;
        public string assetFile;
        public MonoBehaviour monoBinder;
    }
    public class LZTUnity : ISDKAgent
    {
#if USE_LZT
#if !UNITY_EDITOR && UNITY_IOS
        [DllImport("__Internal")]
        private static extern void LZT_setListener(string text);

        [DllImport("__Internal")]
        private static extern void LZT_init(long appId, string appKey);

        [DllImport("__Internal")]
        private static extern void LZT_setServId(string serverId, string serverName, string userid, string userName, string level);

        [DllImport("__Internal")]
        private static extern void LZT_setRole(string userId, string level);

        [DllImport("__Internal")]
        private static extern void LZT_login();

        [DllImport("__Internal")]
        private static extern void LZT_logout();

        [DllImport("__Internal")]
        private static extern void LZT_pay(string goodsId, string orderId, string goodsName, string goodsCount, string goodsPrice, string goodsUnit, string externData, string callbackUrl);

        [DllImport("__Internal")]
        private static extern void LZT_payTest(string goodsId, string orderId, string goodsName, string goodsCount, string goodsPrice, string goodsUnit, string externData, string callbackUrl);
#endif
#endif

#if USE_LZT
        static bool ms_bInited = false;
#endif
        //------------------------------------------------------
        public static LZTUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_LZT
            ms_bInited = false;
            LZTUnity agent = new LZTUnity();
            if (agent.Init(config))
            {
                agent.SetCallback(callback);
                ms_bInited = true;
                return agent;
            }
            return null;
#else
            return null;
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {

#if USE_LZT
            LZTCfg appleCfg = (LZTCfg)cfg;

#if !UNITY_EDITOR && UNITY_IOS
            LZTCallback listener = appleCfg.monoBinder.gameObject.AddComponent<LZTCallback>();
            listener.Init(this);
            LZT_setListener(listener.gameObject.name);
            LZT_init(long.Parse(appleCfg.appId),appleCfg.appKey);
            OnSDKAction(new SDKCallbackParam(ESDKActionType.InitSucces) { name = appleCfg.assetFile, channel = "ZSSdk" });
            return true;
#else
            return false;
#endif
#else
            return false;
#endif
        }
        //------------------------------------------------------
        public static void Login()
        {
#if USE_LZT
            if (!ms_bInited) return;
#if !UNITY_EDITOR && UNITY_IOS
            LZT_login();
#endif
#endif
        }
        //------------------------------------------------------
        public static void Logout()
        {
#if USE_LZT
            if (!ms_bInited) return;
#if !UNITY_EDITOR && UNITY_IOS
            LZT_logout();
#endif
#endif
        }
        //------------------------------------------------------
        public static void SetRoleInfo(string serverId, string serverName, string userId, string userName, string level)
        {
#if USE_LZT
#if !UNITY_EDITOR && UNITY_IOS
            LZT_setServId(serverId, serverName, userId, userName, level);
#endif
#endif
        }
        //------------------------------------------------------
        public static void UpdateRole(string userid, string level)
        {
#if USE_LZT
#if !UNITY_EDITOR && UNITY_IOS
            LZT_setRole(userid, level);
#endif
#endif
        }
#if USE_LZT
        //------------------------------------------------------
        public static bool Pay(string orderId, string goodsId, double amount, string goodsName, string goodsCount, string goodsUnit, string externParam, string callback)
        {
#if USE_LZT
            if (!ms_bInited) return false;
#if !UNITY_EDITOR && UNITY_IOS
            LZT_pay(goodsId, orderId, goodsName, goodsCount, amount.ToString("F2"), goodsUnit, externParam, callback);
            return true;
#else
            return false;
#endif
#else
            return false;
#endif
        }
#endif
#if USE_LZT
        class LZTCallback : MonoBehaviour
        {
            [System.Serializable]
            struct LoginParam
            {
                public string LZT_USERID;
                public string LZT_USERTOKEN;
            }
            LZTUnity m_pAgent = null;
            public void Init(LZTUnity agent)
            {
                m_pAgent = agent;
            }
            void onLogin(string message)
            {
                Debug.Log("lzt login: " + message);
                try
                {
                    LoginParam player = JsonUtility.FromJson<LoginParam>(message);
                    SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LoginSucces);
                    sdkParam.uid = player.LZT_USERID;
                    sdkParam.channel = "ZSSdk";
                    sdkParam.name = player.LZT_USERID;
                    m_pAgent.OnSDKAction(sdkParam);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
            void onLogOut(string message)
            {
                SDKCallbackParam param = new SDKCallbackParam( ESDKActionType.LogoutSucces);
                param.channel = "ZSSdk";
                m_pAgent.OnSDKAction(param);
            }
            void onPaySuccess(string message)
            {
//                 SDKCallbackParam param = new SDKCallbackParam(ESDKActionType.PaySucces);
//                 param.channel = "LZT";
//                 m_pAgent.OnSDKAction(param);
            }
            void onPayFinish(string message)
            {
                SDKCallbackParam param = new SDKCallbackParam(ESDKActionType.PaySucces);
                param.channel = "LZT";
                m_pAgent.OnSDKAction(param);
            }
            void onPayFail(string message)
            {
                SDKCallbackParam param = new SDKCallbackParam(ESDKActionType.PayFail);
                param.channel = "LZT";
                m_pAgent.OnSDKAction(param);
            }
        }
#endif
    }
}