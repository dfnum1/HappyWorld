using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct OppoCfg : ISDKConfig
    {
        public string appKey;
        public string appSecret;
        public string assetFile;
        public MonoBehaviour binderMono;
    }
    public class OppoUnity : ISDKAgent
    {
#if USE_OPPO
        ISDKCallback m_pCallback;
        static AndroidJavaObject ms_Handle;
        OppoCfg m_Config;    
        ISDKAgent m_pAgent = null;
        static bool ms_bInited = false;
#endif
        //------------------------------------------------------
        public static OppoUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_OPPO
            ms_bInited = false;
            OppoUnity agent = new OppoUnity();
            agent.m_pCallback = callback;
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
        public static void Login()
        {
#if USE_OPPO
            if(ms_bInited && ms_Handle!=null)
            {
                ms_Handle.CallStatic("Login", "oppo","");
            }
#endif
        }
        //------------------------------------------------------
        public static void Logout()
        {
#if USE_OPPO
            if(ms_bInited && ms_Handle!=null)
            {
                ms_Handle.CallStatic("Logout", "oppo");
            }
#endif
        }
        //------------------------------------------------------
        public static void Pay(int amount, string googdsName, string goodsDes, string userId, string callback)
        {
#if USE_OPPO
            if(ms_bInited && ms_Handle!=null)
            {
                var oppoSetting = Framework.Core.BaseUtil.stringBuilder;
                oppoSetting.AppendLine("{");
                oppoSetting.Append("\t\"amount\"").Append(":").Append("\"").Append(amount).AppendLine("\",");
                oppoSetting.Append("\t\"goodsName\"").Append(":").Append("\"").Append(googdsName).AppendLine("\",");
                oppoSetting.Append("\t\"goodsDes\"").Append(":").Append("\"").Append(goodsDes).AppendLine("\",");
                oppoSetting.Append("\t\"userId\"").Append(":").Append("\"").Append(userId).AppendLine("\",");
                oppoSetting.Append("\t\"order\"").Append(":").Append("\"").Append("").AppendLine("\",");
                oppoSetting.Append("\t\"callback\"").Append(":").Append("\"").Append(callback).AppendLine("\"");
                oppoSetting.AppendLine("}");
                ms_Handle.CallStatic("Pay", "oppo", oppoSetting.ToString());
            }
#endif
        }
#if USE_OPPO
        internal OppoCfg GetConfig()
        {
            return m_Config;
        }
#endif
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_OPPO
            m_Config = (OppoCfg)cfg;
            if(m_Config.binderMono == null)
            {
                Debug.LogWarning("unity go is null");
                return false;
            }
            ms_Handle = GameSDK.GetSDKHandler();
            if(ms_Handle == null)
            {
                Debug.LogWarning("unfind \"com.unity.sdks.SDKHandler\"");
                return false;
            }
            OppoSDKCallback pCallback = m_Config.binderMono.gameObject.AddComponent<OppoSDKCallback>();
            pCallback.Set(this);
            ms_Handle.CallStatic("SetListener", m_Config.binderMono.gameObject.name);

            var oppoSetting = Framework.Core.BaseUtil.stringBuilder;
            oppoSetting.AppendLine("{");
            oppoSetting.Append("\t\"appSecret\"").Append(":").Append("\"").Append(m_Config.appSecret).AppendLine("\"");
            oppoSetting.AppendLine("}");
            if(ms_Handle.CallStatic<bool>("Init", "oppo",oppoSetting.ToString()))
                return true;
            GameObject.Destroy(pCallback);
            return false;
#else
            return false;
#endif
        }
#if USE_OPPO
        class OppoSDKCallback : MonoBehaviour
        {
           OppoUnity m_pAgent;
            public void Set(OppoUnity agent)
            {
                m_pAgent = agent;
            }
            void OnInitSuccess(string msg)
            {
                ms_bInited = true;
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.InitSucces){ name = m_pAgent.GetConfig().assetFile});
            }
            void OnLoginSuccess(string msg)
            {
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.LoginSucces,msg));
            }

            void OnLoginFail(string msg)
            {
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.LoginFail,msg));
            }
            void OnExitGame(string msg)
            {
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.LogoutSucces,msg));
            }
            void OnGetUserInfoSuccess(string msg)
            {

            }
            void OnSendUserInfoSuccess(string msg)
            {

            }
            void OnSendUserInfoFail(string msg)
            {

            }
            void OnPaySuccess(string msg)
            {
                 m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.PaySucces,msg));
            }
            void OnPayFail(string msg)
            {
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.PayFail,msg));
            }
            void OnPayCancel(string msg)
            {
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.PayCancel,msg));
            }
            void onCallCarrierPay(string msg)
            {

            }
        }
#endif
    }
}