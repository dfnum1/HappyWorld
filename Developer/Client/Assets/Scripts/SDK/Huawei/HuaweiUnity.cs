using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct HuaweiCfg : ISDKConfig
    {
        public string assetFile;
        public MonoBehaviour binderMono;
    }
    public class HuaweiUnity : ISDKAgent
    {
#if USE_HUAWEI
        [System.Serializable]
        struct Player
        {
            public string uid;
            public string token;
            public string nickName;
            public string unionId;
            public string openId;
        }
        [System.Serializable]
        struct Account
        {
            public string uid;
            public string token;
            public string nickName;
            public string unionId;
            public string openId;
            public string age;
            public string gender;
            public string homezone;
            public string email;
        }
        [System.Serializable]
        struct Error
        {
            public int error;
            public string msg;
        }
        HuaweiCfg m_Config;
        ISDKAgent m_pAgent = null;
        static bool ms_bInited = false;
        static AndroidJavaObject ms_Handle;
        //------------------------------------------------------
        public HuaweiCfg GetConfig()
        {
            return m_Config;
        }
#endif
        //------------------------------------------------------
        public static HuaweiUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_HUAWEI
            ms_bInited = false;
            HuaweiUnity agent = new HuaweiUnity();
            if (agent.Init(config))
            {
                agent.SetCallback(callback);
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
#if USE_HUAWEI
            m_Config = (HuaweiCfg)cfg;
            if (m_Config.binderMono == null)
            {
                Debug.LogWarning("unity go is null");
                return false;
            }
            ms_Handle =GameSDK.GetSDKHandler();
            if (ms_Handle == null)
            {
                Debug.LogWarning("unfind \"com.unity.sdks.SDKHandler\"");
                return false;
            }
            HuaweiSDKCallback pCallback = m_Config.binderMono.gameObject.AddComponent<HuaweiSDKCallback>();
            pCallback.Set(this);
            ms_Handle.CallStatic("SetListener", m_Config.binderMono.gameObject.name);
            Debug.Log("HUAWEI SDK Begin Init");
            if(ms_Handle.CallStatic<bool>("Init", "huawei",""))
            {
                return true;
            }
            Debug.Log("HUAWEI SDK Init Fail");
            GameObject.Destroy(pCallback);
            return false;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        public static void Login()
        {
#if USE_HUAWEI
            if(!ms_bInited || ms_Handle == null)
            {
                return;
            }
            ms_Handle.CallStatic("Login", "huawei", "");
#endif
        }
        //------------------------------------------------------
        public static void Logout()
        {
#if USE_HUAWEI
            if (!ms_bInited || ms_Handle == null)
            {
                return;
            }
            ms_Handle.CallStatic("Login", "huawei");
#endif
        }
#if USE_HUAWEI
        //------------------------------------------------------
        // HuaweiSDKCallback
        //------------------------------------------------------
        class HuaweiSDKCallback : MonoBehaviour
        {
            HuaweiUnity m_pAgent = null;
            public void Set(HuaweiUnity agent)
            {
                m_pAgent = agent;
            }
            void OnInitSuccess()
            {
                Debug.Log("HUAWEI SDK Init OK");
                ms_bInited = true;
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.InitSucces);
                sdkParam.name = m_pAgent.GetConfig().assetFile;
                m_pAgent.OnSDKAction(sdkParam);
            }
            void OnInitFailure(string code)
            {
                 Debug.Log("HUAWEI SDK Init FAIL");
                ms_bInited = false;
                if (code == null) return;
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.InitFail);
                if (code.CompareTo("7401") == 0)
                {
                    //用户未同意华为联运隐私协议
                    sdkParam.name = "用户未同意华为联运隐私协议";
                    m_pAgent.OnSDKAction(sdkParam);
                    return;
                }
                if (code.CompareTo("7002") == 0)
                {
                    //网络异常
                    //此处您可提示玩家检查网络，请不要重复调用init接口，否则断网情况下可能会造成手机高耗电。
                    sdkParam.name = "网络异常";
                    m_pAgent.OnSDKAction(sdkParam);
                    return;
                }
                if (code.CompareTo("907135003") == 0)
                {
                    //表示玩家取消HMS Core升级或组件升级
                    return;
                }
            }

            void OnAccountData(string accountJson)
            {
                Debug.Log("HUAWEI SDK OnAccountData:" + accountJson);
            }
            void OnLoginSuccess(string accountJson)
            {
                Debug.Log("HUAWEI SDK OnLoginSuccess:" + accountJson);
                try
                {
                    Player player = JsonUtility.FromJson<Player>(accountJson);
                    SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LoginSucces);
                    sdkParam.uid = player.uid;
                    sdkParam.channel = "Huawei";
                    sdkParam.name = player.nickName;
                    m_pAgent.OnSDKAction(sdkParam);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
            void OnLoginFail(string msg)
            {
                try
                {
                    Error player = JsonUtility.FromJson<Error>(msg);
                    SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LoginFail);
                    sdkParam.msg = player.msg;
                    m_pAgent.OnSDKAction(sdkParam);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }

            void OnLogoutSuccess(string msg)
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LogoutSucces);
                m_pAgent.OnSDKAction(sdkParam);
            }

            void OnLogoutFail(string msg)
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LogoutFail);
                m_pAgent.OnSDKAction(sdkParam);
            }

            void OnAntiAddictionExit(string msg)
            {
                   // 该回调会在如下两种情况下返回:
                // 1.未成年人实名帐号在白天登录游戏，华为会弹框提示玩家不允许游戏，玩家点击“确定”，华为返回回调
                // 2.未成年实名帐号在国家允许的时间登录游戏，到晚上9点，华为会弹框提示玩家已到时间，玩家点击“知道了”，华为返回回调
                // 您可在此处实现游戏防沉迷功能，如保存游戏、调用帐号退出接口或直接游戏进程退出(如System.exit(0))            
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LogoutSucces);
                m_pAgent.OnSDKAction(sdkParam);                
            }

            void OnPopMessage(string code)
            {
                if (code == null) return;
                if (code.CompareTo("1") == 0)
                {
                    //! 请先初始化后再登录
                    return;
                }
                if (code.CompareTo("2") == 0)
                {
                    //! 登录成功，但账号信息解析失败
                    return;
                }
                if (code.CompareTo("3") == 0)
                {
                    //! SignIn result is empty
                    return;
                }
            }

            void OnPaySuccess(string code)
            {
                Debug.Log(code);
                SDKCallbackParam param = new SDKCallbackParam(ESDKActionType.PaySucces);
                param.channel = "Huawei";
                m_pAgent.OnSDKAction(param);
                //                                         	String msg = "{";
                //                                 msg += "\"payment\":\"" +result.getPaymentData() + "\",";
                //                                 msg += "\"signature\":\"" +result.getPaymentSignature() + "\"";
                //                                 msg += "}";
            }

            void OnPayFail(string code)
            {
                Debug.Log(code);
                SDKCallbackParam param = new SDKCallbackParam(ESDKActionType.PayFail);
                param.channel = "Huawei";
                m_pAgent.OnSDKAction(param);
            }
        }
#endif
    }
}