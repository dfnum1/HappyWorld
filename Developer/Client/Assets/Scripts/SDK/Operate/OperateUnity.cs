using System;
using System.Collections.Generic;
using UnityEngine;
#if USE_OPERATE
using com.m3839.sdk;
using com.m3839.sdk.login;
using com.m3839.sdk.login.bean;
using com.m3839.sdk.login.listener;
using com.m3839.sdk.pay.listener;
using com.m3839.sdk.pay.bean;
using com.m3839.sdk.pay;
using com.m3839.sdk.single;
#endif
namespace SDK
{
    [System.Serializable]
    public struct OperateCfg : ISDKConfig
    {
        public string appKey;
        public string assetFile;
        public MonoBehaviour binderMono;
    }
    public class OperateUnity : ISDKAgent
    {
#if USE_OPERATE
        ISDKCallback m_pCallback;
        OperateCfg m_Config;    
        static bool ms_bInited = false;
        static OperateUnity ms_pAgent = null;
#endif
        //------------------------------------------------------
        public static OperateUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_OPERATE
            ms_bInited = false;
            OperateUnity agent = new OperateUnity();
            agent.m_pCallback = callback;
            if (agent.Init(config))
            {
                agent.SetCallback(callback);
                ms_pAgent = agent;
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
#if USE_OPERATE
            if(ms_bInited)
            {
                HykbUser user = HykbLogin.GetUser();
                if (user != null)
                {
                    var param = new SDKCallbackParam(ESDKActionType.LoginSucces);
                    param.channel = "Operate";
                    param.name = user.getNick();
                    param.uid = user.getUserId();
                    GameSDK.DoCallback(ms_pAgent, param);

                    return;
                }

                HykbLogin.Login();
            }
#endif
        }
        //------------------------------------------------------
        public static void Logout()
        {
#if USE_OPERATE
            if(ms_bInited)
            {
                HykbLogin.Logout();
                var param = new SDKCallbackParam(ESDKActionType.LogoutSucces);
                HykbUser user = HykbLogin.GetUser();
                if(user!=null)
                {
                    param.channel = "Operate";
                    param.name = user.getNick();
                    param.uid = user.getUserId();
                }
                GameSDK.DoCallback(ms_pAgent, param);
            }
#endif
        }
        //------------------------------------------------------
        public static bool Pay(string orderId, double amount, string googdsName, string externParam, string callback)
        {
#if USE_OPERATE
            if (ms_bInited)
            {
                HykbUser user = HykbLogin.GetUser();
                if(user!=null)
                {
                    HykbPayInfo payInfo = new HykbPayInfo(googdsName, (int)amount, 0, orderId, externParam);
                    PayListenerPoroxy proxy = new PayListenerPoroxy(ms_pAgent);
                    HykbPay.Pay(payInfo, proxy);
                    return true;
                }
                else
                {
                    GameSDK.DoCallback(null, new SDKCallbackParam(ESDKActionType.Message) { msg = "unlogin" });
                }
            }
#endif
            return false;
        }
#if USE_OPERATE
        internal OperateCfg GetConfig()
        {
            return m_Config;
        }
#endif
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_OPERATE
            m_Config = (OperateCfg)cfg;
            int orientation = 0;
            if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
                orientation = HykbContext.SCREEN_PORTRAIT;
            else orientation = HykbContext.SCREEN_LANDSCAPE;

            InitListenerProxy proxy = new InitListenerProxy(this);
            HykbLogin.Init(m_Config.appKey, orientation, proxy);
            HykbLogin.SetUserListener(new LoginListenerProxy(this));
            UnionFcmSDK.Init(m_Config.appKey, orientation, new FCListenerProxy(this));
            return true;
#else
            return false;
#endif
        }
#if USE_OPERATE
        class FCListenerProxy : UnionV2FcmListener
        {
            OperateUnity m_pInstance;

            public FCListenerProxy(OperateUnity instance)
            {
                this.m_pInstance = instance;
            }
            public override void OnFailed(int code, string message)
            {
                if (2005 == code || 2003 == code)
                {
                    var param = new SDKCallbackParam(ESDKActionType.FCMOut);
                    param.channel = "Operate";
                    param.msg = message;
                    m_pInstance.OnSDKAction(param);
                    //Application.Quit();
                }
                else
                {
                    var param = new SDKCallbackParam(ESDKActionType.Message);
                    param.channel = "Operate";
                    param.msg = message;
                    m_pInstance.OnSDKAction(param);
                }
            }

            public override void OnSucceed(UnionFcmUser user)
            {
                if (user != null)
                {
                    //µÇÂ¼³É¹¦
                  //  demo.ShowText.text = user.toString();
                }
            }
        }
        class PayListenerPoroxy : HykbV2PayListener
        {
            OperateUnity m_pInstance;

            public PayListenerPoroxy(OperateUnity instance)
            {
                this.m_pInstance = instance;
            }
            public override void OnFailed(HykbPayInfo payInfo, int code, string message)
            {
                var param = new SDKCallbackParam(ESDKActionType.PayFail); 
                param.channel = "Operate";
                param.cpOrderId = payInfo.cpOrderId;
                param.name = payInfo.goodsName;
                m_pInstance.OnSDKAction(param);
            }

            public override void OnSucceed(HykbPayInfo payInfo)
            {
                var param = new SDKCallbackParam(ESDKActionType.PaySucces);
                param.channel = "Operate";
                param.cpOrderId = payInfo.cpOrderId;
                param.name = payInfo.goodsName;
                m_pInstance.OnSDKAction(param);
            }
        }

        public class LoginListenerProxy : HykbUserListener
        {
            OperateUnity m_pInstance;

            public LoginListenerProxy(OperateUnity instance)
            {
                this.m_pInstance = instance;
            }

            public override void OnLoginFailed(int code, string message)
            {
                m_pInstance.OnSDKAction(new SDKCallbackParam(ESDKActionType.LoginFail) { msg = message, channel = "Operate" });
            }

            public override void OnLoginSucceed(HykbUser user)
            {
                var param = new SDKCallbackParam(ESDKActionType.LoginSucces);
                param.channel = "Operate";
                param.name = user.getNick();
                param.uid = user.getUserId();
                m_pInstance.OnSDKAction(param);
            }

            public override void OnSwitchUser(HykbUser user)
            {
                var param = new SDKCallbackParam(ESDKActionType.SwitchAccount);
                param.channel = "Operate";
                param.name = user.getNick();
                param.uid = user.getUserId();
                m_pInstance.OnSDKAction(param);
            }
        }

        public class InitListenerProxy : HykbV2InitListener
        {
            OperateUnity m_pInstance;
            public InitListenerProxy(OperateUnity instance)
            {
                this.m_pInstance = instance;
            }

            public override void OnSucceed()
            {
                ms_bInited = true;
                m_pInstance.OnSDKAction(new SDKCallbackParam(ESDKActionType.InitSucces) { name = m_pInstance.GetConfig().assetFile, channel = "Operate" });
            }

            public override void OnFailed(int code, string message)
            {
                m_pInstance.OnSDKAction(new SDKCallbackParam(ESDKActionType.InitFail) { msg = message, channel = "Operate" });
            }
        }
#endif
    }
}