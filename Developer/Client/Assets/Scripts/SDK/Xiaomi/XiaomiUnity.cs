using System;
using System.Collections.Generic;
using UnityEngine;
#if USE_XIAOMI
using Xiaomi.GameSDK;
#endif

namespace SDK
{
    [System.Serializable]
    public struct XiaomiCfg : ISDKConfig
    {
        public string appId;
        public string appKey;
        public bool passPrivacy; // 隐私协议
        public string assetFile;
    }
    public class XiaomiUnity : ISDKAgent
    {
#if USE_XIAOMI
        static string ms_PayOrder = null;
        static MyCallback m_Callback = new MyCallback();
        static bool ms_bInited = false;
        XiaomiCfg m_Config;
        static Dictionary<string, object> ms_vEventTemp = null;
#endif
        //------------------------------------------------------
        public static XiaomiUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_XIAOMI
            ms_bInited = false;
            XiaomiUnity agent = new XiaomiUnity();
            if (agent.Init(config))
            {
                agent.SetCallback(callback);
                m_Callback.SetAgent(agent);
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
#if USE_XIAOMI
            m_Config = (XiaomiCfg)cfg;
            SDKAndroid.Instance.Init();

            if(m_Config.passPrivacy)
            {
                //此处应该显示游戏隐私协议，请游戏自行实现
                //同意隐私后，请调用以下方法通知SDK
                SDKAndroid.Instance.OnUserAgreed();
            }
            GameSDK.OnSDKAction(this, new SDKCallbackParam(ESDKActionType.InitSucces){ name = m_Config.assetFile});
            return true;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        public static void Login()
        {
#if USE_XIAOMI
            if (!ms_bInited) return;
            SDKAndroid.Instance.OnStartLogin(m_Callback);
#endif
        }
        //------------------------------------------------------
        public static void LogOut()
        {
#if USE_XIAOMI
            if (!ms_bInited) return;
            SDKAndroid.Instance.OnAppExit(m_Callback);
#endif
        }
#if USE_XIAOMI
        //------------------------------------------------------
        public static void Pay(int amount, string cpUserInfo)
        {
#if USE_XIAOMI
            if (!ms_bInited) return;
            if(string.IsNullOrEmpty(ms_PayOrder))
            {
                ms_PayOrder = System.Guid.NewGuid().ToString();
                SDKAndroid.Instance.OnAmountPay(amount, ms_PayOrder, cpUserInfo, m_Callback);
            }
            else
            {
                m_Callback.FinishPayProcess(-18005);
            }
#endif
        }
#endif
#if USE_XIAOMI
        public class MyCallback : IMiSDKLoginCallback, IMiSDKExitCallback, IMiSDKPayCallback
        {
            ISDKAgent m_pAgent;
            public void SetAgent(ISDKAgent agent)
            {
                m_pAgent = agent;
            }
            void IMiSDKLoginCallback.FinishLoginProcess(int code, MiAccountInfo var2)
            {
                if(code == 0)
                    m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.LoginSucces, var2.nikename, var2.uid));
                else
                    m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.LoginFail, var2.nikename, var2.uid));
            }

            public void OnExit(int code)
            {
                if (code == 10001) m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.LogoutSucces));
                else m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.LogoutFail));
            }

            public void FinishPayProcess(int code)
            {
                switch(code)
                {
                    case 0: m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.PaySucces, ms_PayOrder)); break;
                    case -12:
                    case -18004: //购买取消，不要发货
                        m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.PayCancel, ms_PayOrder)); break;
                    case -18005://重复购买
                        m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.RePay, ms_PayOrder)); break;
                    default:
                        m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.PayFail, ms_PayOrder)); break;
                }
                ms_PayOrder = null;
            }
        }
#endif
    }
}