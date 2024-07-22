#if USE_QUICKSDK
using quicksdk;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct QuickSDKCfg :ISDKConfig
    {
        public string productCode;
        public string productKey;
        public string assetFile;
        public GameObject pMainGO;
    }

    public class UnityQuickSDK : ISDKAgent
    {
#if USE_QUICKSDK
        QuickSdDKCallback m_Callback = null;
        QuickSDKCfg m_Config;
        static UnityQuickSDK ms_pQuickSDK = null;
        public QuickSDKCfg GetConfig(){ return m_Config; }
#endif
        //------------------------------------------------------
        public static UnityQuickSDK StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_QUICKSDK
            ms_pQuickSDK = null;
            UnityQuickSDK quickSDK = new UnityQuickSDK();
            if (quickSDK.Init(config))
            {
                quickSDK.SetCallback(callback);
                ms_pQuickSDK = quickSDK;
                return quickSDK;
            }
            return null;
#else
            return null;
#endif
        }
        //------------------------------------------------------
        public static void Login(string channel = null)
        {
#if USE_QUICKSDK
            if (ms_pQuickSDK != null)
            {
                if (string.IsNullOrEmpty(channel))
                    quicksdk.QuickSDK.getInstance().login();
                else if (channel.CompareTo("google") == 0)
                    quicksdk.QuickSDK.getInstance().googleLogin();
                else if (channel.CompareTo("free") == 0)
                    quicksdk.QuickSDK.getInstance().freeLogin();
            }
#endif
        }
        //------------------------------------------------------
        public static void LogOut()
        {
#if USE_QUICKSDK
            if (ms_pQuickSDK != null)
            {
                quicksdk.QuickSDK.getInstance().logout();
            }
#endif
        }
        //------------------------------------------------------
        public static void Exit()
        {
#if USE_QUICKSDK
            if (ms_pQuickSDK != null)
            {
//                 if (QuickSDK.getInstance().isChannelHasExitDialog())
//                 {
//                     quicksdk.QuickSDK.getInstance().exit();
//                 }
//                 else
                {
                    //mExitDialogCanvas.SetActive(true);
                    //游戏调用自身的退出对话框，点击“确定”后，再调用QuickSDK.getInstance().exit();
                    SDKCallbackParam param = new SDKCallbackParam();
                    param.action = ESDKActionType.Exit;
                    param.channel = "OutQuick";
                    ms_pQuickSDK.OnSDKAction(param);
                }
            }
#endif
        }
#if USE_QUICKSDK
        //------------------------------------------------------
        public static void EnterGame(GameRoleInfo param)
        {
            if (ms_pQuickSDK != null)
            {

//                     QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_REAL_NAME_REGISTER);
//                     quicksdk.QuickSDK.getInstance().enterGame(param);
            }
    }
        //------------------------------------------------------
        public static void CreateUser(GameRoleInfo param)
        {
            if (ms_pQuickSDK != null)
            {
//                     QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_REAL_NAME_REGISTER);
//                     quicksdk.QuickSDK.getInstance().createRole(param);
            }
    }
        //------------------------------------------------------
        public static void UpdateUser(GameRoleInfo param)
        {
            if (ms_pQuickSDK != null)
            {
                quicksdk.QuickSDK.getInstance().updateRole(param);
            }
        }
        //------------------------------------------------------
        public static void Share(ShareInfo param)
        {
            if (ms_pQuickSDK != null)
            {
                if(param.type.CompareTo("Ins") == 0)
                    quicksdk.QuickSDK.getInstance().shareToIns(param.imgPath);
                else if (param.type.CompareTo("TikTok") == 0)
                    quicksdk.QuickSDK.getInstance().shareToTikTok(param.imgPath);
                else if (param.type.CompareTo("fbImg") == 0)
                    quicksdk.QuickSDK.getInstance().fbShareImg(param.imgPath);
                else if (param.type.CompareTo("fbUrl") == 0)
                    quicksdk.QuickSDK.getInstance().fbShareUrl(param.url);
            }
        }
#endif
        //------------------------------------------------------
        public static void UserCenter()
        {
#if USE_QUICKSDK
            if (ms_pQuickSDK != null)
            {
                quicksdk.QuickSDK.getInstance().userCenter();
            }
#endif
        }
#if USE_QUICKSDK
        //------------------------------------------------------
        public static void Play(GameRoleInfo user, OrderInfo order)
        {
            if (ms_pQuickSDK != null)
            {
                quicksdk.QuickSDK.getInstance().pay(order, user);
            }
    }
#endif
        //------------------------------------------------------
        static Dictionary<string,string> ms_vDirMaps = new System.Collections.Generic.Dictionary<string, string>();
        internal static void LogEvent(string eventName, string parameterName, string parameterValue)
        {
#if USE_QUICKSDK
            if (string.IsNullOrEmpty(parameterValue) || string.IsNullOrEmpty(parameterName) || string.IsNullOrEmpty(eventName)) return;
            if (ms_pQuickSDK != null)
            {
                string strToken = Framework.Core.BaseUtil.stringBuilder.Append(eventName).Append(":").Append(parameterName).Append("-").Append(parameterValue).ToString();
                quicksdk.QuickSDK.getInstance().adjustEvent(strToken);

                if (ms_vDirMaps == null) ms_vDirMaps = new Dictionary<string, string>(1);
                ms_vDirMaps.Clear();
                ms_vDirMaps[parameterName] = parameterValue;
                quicksdk.QuickSDK.getInstance().appsFlyerEvent(eventName, ms_vDirMaps);

                quicksdk.QuickSDK.getInstance().firebaseEvent(eventName, ms_vDirMaps);
            }
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_QUICKSDK
            Debug.Log("Begin Quick SDK Init!");
            QuickSDKCfg quickCfg = (QuickSDKCfg)cfg;
            m_Callback = quickCfg.pMainGO.AddComponent<QuickSdDKCallback>();
            m_Callback.Set(this);
            quicksdk.QuickSDK.getInstance().setListener(m_Callback);
            quicksdk.QuickSDK.getInstance().init();
            return true;
#else
            return false;
#endif
        }
#region SDKCALLBACK
        //callback
#if USE_QUICKSDK
        class QuickSdDKCallback : QuickSDKListener
        {
            UnityQuickSDK m_pAgent;
            public void Set(UnityQuickSDK agent)
            {
                m_pAgent = agent;
            }
            //------------------------------------------------------
            public override void onInitSuccess()
            {
                //初始化成功的回调
                Debug.Log("QuickSDK Init success");
                GameSDK.DoCallback(m_pAgent, new SDKCallbackParam(ESDKActionType.InitSucces){ name = m_pAgent.GetConfig().assetFile});
            }
            //------------------------------------------------------
            public override void onInitFailed(ErrorMsg errMsg)
            {
                //初始化失败的回调
                Debug.Log("QuickSDK init failed  " + "msg: " + errMsg.errMsg);
                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.InitFail;
                param.msg = errMsg.errMsg;
                param.channel = "OutQuick";
                m_pAgent.OnSDKAction(param);
            }
            //------------------------------------------------------
            public override void onLoginSuccess(UserInfo userInfo)
            {
                //登录成功的回调
                Debug.Log("onLoginSuccess  " + "uid: " + userInfo.uid + " ,username: " + userInfo.userName + " ,userToken: " + userInfo.token + ", msg: " + userInfo.errMsg);
                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.LoginSucces;
                param.uid = userInfo.uid;
                param.name = userInfo.userName;
                param.msg = userInfo.errMsg;
                param.channel = "OutQuick";
                m_pAgent.OnSDKAction(param);
            }
//             //------------------------------------------------------
//             public override void onSwitchAccountSuccess(UserInfo userInfo)
//             {
//                 //切换账号成功的回调
//                 //一些渠道在悬浮框有切换账号的功能，此回调即切换成功后的回调。游戏应清除当前的游戏角色信息。在切换账号成功后回到选择服务器界面，请不要再次调用登录接口。
//                 Debug.Log("onLoginSuccess   " + "uid: " + userInfo.uid + " ,username: " + userInfo.userName + " ,userToken: " + userInfo.token + ", msg: " + userInfo.errMsg);
// 
//                 QuickSDKParam param = new QuickSDKParam();
//                 param.actionType = EQuickSDKActionType.SwitchAccount;
//                 param.userParam = userInfo;
//                 m_pAgent.OnSDKAction(param);
//             }
            //------------------------------------------------------
            public override void onLoginFailed(ErrorMsg errMsg)
            {
                //登录失败的回调
                //如果游戏没有登录按钮，应在这里再次调用登录接口
                Debug.Log("onLoginFailed   " + "msg: " + errMsg.errMsg);

                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.LoginFail;
                param.msg = errMsg.errMsg;
                param.channel = "OutQuick";
                m_pAgent.OnSDKAction(param);
            }
            //------------------------------------------------------
            public override void onLogoutSuccess()
            {
                //注销成功的回调
                //游戏应该清除当前角色信息，回到登陆界面，并自动调用一次登录接口
                Debug.Log("onLogoutSuccess");

                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.LogoutSucces;
                param.channel = "OutQuick";
                m_pAgent.OnSDKAction(param);
            }
            //------------------------------------------------------
            public override void onPaySuccess(PayResult payResult)
            {
                //支付成功的回调
                //一些渠道支付成功的通知并不准确，因此客户端的通知仅供参考，游戏发货请以服务端通知为准，不能以客户端的通知为准
                Debug.Log("onPaySuccess   " + "orderId: " + payResult.orderId + " ,extraParam" + payResult.extraParam);
                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.PaySucces;
                param.cpOrderId = payResult.orderId;
                param.msg = payResult.extraParam;
                param.channel = "OutQuick";
                m_pAgent.OnSDKAction(param);
            }
            //------------------------------------------------------
            public override void onPayCancel(PayResult payResult)
            {
                //支付取消的回调
                Debug.Log("onPayCancel   " + "orderId: " + payResult.orderId  + " ,extraParam" + payResult.extraParam);

                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.PayCancel;
                param.cpOrderId = payResult.orderId;
                param.msg = payResult.extraParam;
                param.channel = "OutQuick";
                m_pAgent.OnSDKAction(param);
            }
            //------------------------------------------------------
            public override void onPayFailed(PayResult payResult)
            {
                //支付失败的回调
                Debug.Log("onPayFailed  " + "orderId: " + payResult.orderId + " ,extraParam" + payResult.extraParam);
                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.PayFail;
                param.cpOrderId = payResult.orderId;
                param.msg = payResult.extraParam;
                param.channel = "OutQuick";
                m_pAgent.OnSDKAction(param);
            }
            //             //------------------------------------------------------
            //             public override void onExitSuccess()
            //             {
            //                 Debug.Log("onExitSuccess");
            //                 QuickSDK.getInstance().exitGame();
            //                 QuickSDKParam param = new QuickSDKParam();
            //                 param.actionType = EQuickSDKActionType.ExitGame;
            //                 m_pAgent.OnSDKAction(param);
            //                 //SDK退出成功的回调
            //                 //在此处调用QuickSDK.getInstance().exitGame()函数即可实现退出游戏，杀进程。为避免与渠道发生冲突，请不要使用Application.Quit()函数
            //             }
            //             //------------------------------------------------------
            //             public override void onSucceed(string infos)
            //             {
            //                 Debug.Log("onSucceed:" + infos);
            //             }
            //             //------------------------------------------------------
            //             public override void onFailed(string message)
            //             {
            //                 Debug.Log("onFailed  " + "msg: " + message);
            //             }
            //------------------------------------------------------
            public override void onSubsSkuCallback(string skuId, string qkOrderId)
            {
            }
        }
#endif
#endregion
    }
}