using System;
using System.Collections.Generic;
using UnityEngine;
#if USE_INNER_QUICKSDK
using quicksdk;
#endif
namespace SDK
{
    [System.Serializable]
    public struct InnerQuickSDKCfg :ISDKConfig
    {
        public string productCode;
        public string productKey;
        public string assetFile;
        public GameObject pMainGO;
    }

    public class InnerQuickSDKUnity : ISDKAgent
    {
#if USE_INNER_QUICKSDK
        QuickSdDKCallback m_Callback = null;
        InnerQuickSDKCfg m_Config;
        static InnerQuickSDKUnity ms_pQuickSDK = null;
        public InnerQuickSDKCfg GetConfig()
        {
            return m_Config;
        }
#endif
        //------------------------------------------------------
        public static InnerQuickSDKUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_INNER_QUICKSDK
            ms_pQuickSDK = null;
            InnerQuickSDKUnity quickSDK = new InnerQuickSDKUnity();
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
#if USE_INNER_QUICKSDK
            if (ms_pQuickSDK != null)
            {
                QuickSDK.getInstance().login();
            }
#endif
        }
        //------------------------------------------------------
        public static void Logout()
        {
#if USE_INNER_QUICKSDK
            if (ms_pQuickSDK != null)
            {
                QuickSDK.getInstance().logout();
            }
#endif
        }
        //------------------------------------------------------
        public static void Exit()
        {
#if USE_INNER_QUICKSDK
            if (ms_pQuickSDK != null)
            {
                if (QuickSDK.getInstance().isChannelHasExitDialog())
                {
                    QuickSDK.getInstance().exit();
                }
                else
                {
                    //游戏调用自身的退出对话框，点击确定后，调用QuickSDK的exit()方法
                    //                     SDKCallbackParam param = new SDKCallbackParam();
                    //                     param.action = ESDKActionType.Exit;
                    //                     param.channel = "Quick";
                    //                     GameSDK.DoCallback(null, param);
                    QuickSDK.getInstance().exit();
                }
            }
#endif
        }
        //------------------------------------------------------
        public static void EnterGame(string userId, string nickName, int level, string serverName)
        {
#if USE_INNER_QUICKSDK
            if (ms_pQuickSDK != null)
            {
                QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_REAL_NAME_REGISTER);
                //注：GameRoleInfo的字段，如果游戏有的参数必须传，没有则不用传
                GameRoleInfo gameRoleInfo = new GameRoleInfo();
                if (string.IsNullOrEmpty(serverName)) serverName = "0";
                gameRoleInfo.gameRoleBalance = "0";
                gameRoleInfo.gameRoleID = userId;
                gameRoleInfo.gameRoleLevel = level.ToString();
                gameRoleInfo.gameRoleName = nickName;
//                gameRoleInfo.partyName = "同济会";
                gameRoleInfo.serverID = "1";
                gameRoleInfo.serverName = serverName;
                gameRoleInfo.vipLevel = "1";
                gameRoleInfo.roleCreateTime = DateTime.Now.Ticks.ToString();//UC与1881渠道必传，值为10位数时间戳

//                 gameRoleInfo.gameRoleGender = "男";//360渠道参数
//                 gameRoleInfo.gameRolePower = "38";//360渠道参数，设置角色战力，必须为整型字符串
//                 gameRoleInfo.partyId = "1100";//360渠道参数，设置帮派id，必须为整型字符串
// 
//                 gameRoleInfo.professionId = "11";//360渠道参数，设置角色职业id，必须为整型字符串
//                 gameRoleInfo.profession = "法师";//360渠道参数，设置角色职业名称
//                 gameRoleInfo.partyRoleId = "1";//360渠道参数，设置角色在帮派中的id
//                 gameRoleInfo.partyRoleName = "帮主"; //360渠道参数，设置角色在帮派中的名称
//                 gameRoleInfo.friendlist = "无";//360渠道参数，设置好友关系列表，格式请参考：http://open.quicksdk.net/help/detail/aid/190


                QuickSDK.getInstance().enterGame(gameRoleInfo);//开始游戏
            }
#endif
        }
        //------------------------------------------------------
        public static void CreateUser(string userId, string nickName, int level, string serverName)
        {
#if USE_INNER_QUICKSDK
            if (ms_pQuickSDK != null)
            {
                if (string.IsNullOrEmpty(serverName)) serverName = "0";
                if (m_GameRoleInfo == null) m_GameRoleInfo = new GameRoleInfo();
                if (string.IsNullOrEmpty(nickName)) nickName = userId;
                m_GameRoleInfo.gameRoleBalance = "0";
                m_GameRoleInfo.gameRoleID = userId;
                m_GameRoleInfo.gameRoleLevel = level.ToString();
                m_GameRoleInfo.gameRoleName = nickName;
                m_GameRoleInfo.partyName = "test";
                m_GameRoleInfo.serverID = "1";
                m_GameRoleInfo.serverName = serverName;
                m_GameRoleInfo.vipLevel = "1";
                m_GameRoleInfo.roleCreateTime = DateTime.Now.Ticks.ToString();//UC与1881渠道必传，值为10位数时间戳

                //                 gameRoleInfo.gameRoleGender = "男";//360渠道参数
                //                 gameRoleInfo.gameRolePower = "38";//360渠道参数，设置角色战力，必须为整型字符串
                //                 gameRoleInfo.partyId = "1100";//360渠道参数，设置帮派id，必须为整型字符串
                // 
                //                 gameRoleInfo.professionId = "11";//360渠道参数，设置角色职业id，必须为整型字符串
                //                 gameRoleInfo.profession = "法师";//360渠道参数，设置角色职业名称
                //                 gameRoleInfo.partyRoleId = "1";//360渠道参数，设置角色在帮派中的id
                //                 gameRoleInfo.partyRoleName = "帮主"; //360渠道参数，设置角色在帮派中的名称
                //                 gameRoleInfo.friendlist = "无";//360渠道参数，设置好友关系列表，格式请参考：http://open.quicksdk.net/help/detail/aid/190


                QuickSDK.getInstance().createRole(m_GameRoleInfo);//创建角色
            }
#endif
        }
        //------------------------------------------------------
        public static void ShowFloatMenu(bool bShow)
        {
#if USE_INNER_QUICKSDK
            if(ms_pQuickSDK!=null)
            {
                if (bShow) QuickSDK.getInstance().showToolBar(ToolbarPlace.QUICK_SDK_TOOLBAR_MID_LEFT);
                else QuickSDK.getInstance().hideToolBar();
            }
#endif
        }
        //------------------------------------------------------
#if USE_INNER_QUICKSDK
        static GameRoleInfo m_GameRoleInfo = null;
#endif
        public static void UpdateUser(string userId, string nickName, int level, string serverName)
        {
#if USE_INNER_QUICKSDK
            if (ms_pQuickSDK != null)
            {
                if (string.IsNullOrEmpty(serverName)) serverName = "0";
                //注：GameRoleInfo的字段，如果游戏有的参数必须传，没有则不用传
                if (m_GameRoleInfo == null) m_GameRoleInfo = new GameRoleInfo();
                if (string.IsNullOrEmpty(nickName)) nickName = userId;
                m_GameRoleInfo.gameRoleBalance = "0";
                m_GameRoleInfo.gameRoleID = userId;
                m_GameRoleInfo.gameRoleLevel = level.ToString();
                m_GameRoleInfo.gameRoleName = nickName;
                m_GameRoleInfo.partyName = "test";
                m_GameRoleInfo.serverID = "1";
                m_GameRoleInfo.serverName = serverName;
                m_GameRoleInfo.vipLevel = "1";
                m_GameRoleInfo.roleCreateTime = DateTime.Now.Ticks.ToString();//UC与1881渠道必传，值为10位数时间戳

//                 gameRoleInfo.gameRoleGender = "男";//360渠道参数
//                 gameRoleInfo.gameRolePower = "38";//360渠道参数，设置角色战力，必须为整型字符串
//                 gameRoleInfo.partyId = "1100";//360渠道参数，设置帮派id，必须为整型字符串
// 
//                 gameRoleInfo.professionId = "11";//360渠道参数，设置角色职业id，必须为整型字符串
//                 gameRoleInfo.profession = "法师";//360渠道参数，设置角色职业名称
//                 gameRoleInfo.partyRoleId = "1";//360渠道参数，设置角色在帮派中的id
//                 gameRoleInfo.partyRoleName = "帮主"; //360渠道参数，设置角色在帮派中的名称
//                 gameRoleInfo.friendlist = "无";//360渠道参数，设置好友关系列表，格式请参考：http://open.quicksdk.net/help/detail/aid/190

                QuickSDK.getInstance().updateRole(m_GameRoleInfo);
            }
#endif
        }
        //------------------------------------------------------
        public static void Share(ShareParam param)
        {
#if USE_INNER_QUICKSDK
            ShareInfo shareInfo = new ShareInfo();

            shareInfo.title = param.title;
            shareInfo.content = param.content;
            shareInfo.imgPath = param.imgPath;
            shareInfo.imgUrl = param.imgUrl;
            shareInfo.url = param.url;
            shareInfo.type = param.type;
            shareInfo.shareTo = param.shareTo;
            shareInfo.extenal = param.extenal;
            QuickSDK.getInstance().callSDKShare(shareInfo);
#endif
        }
        //------------------------------------------------------
        public static string GetChannelName()
        {
#if USE_INNER_QUICKSDK
            try
            {
                if (string.IsNullOrEmpty(QuickSDK.getInstance().channelName())) return "quick";
                return "quick_" + QuickSDK.getInstance().channelName().ToLower();
            }
            catch(Exception e)
            {
                Debug.LogWarning(e.ToString());
            }     
#endif
            return "quick";
        }
        //------------------------------------------------------
        public static void UserCenter()
        {
#if USE_INNER_QUICKSDK
            if (ms_pQuickSDK != null)
            {
                QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_ENTER_USER_CENTER);
            }
#endif
        }
        //------------------------------------------------------
        public static bool Pay(string userId, string orderId, double amount, string goodsId, string goodsName, string googdsDesc = null, string externParam = null, string callbackUrl = null)
        {
#if USE_INNER_QUICKSDK
            if (ms_pQuickSDK != null)
            {
                if (googdsDesc == null) googdsDesc = "";
                if (externParam == null) externParam = "";
                if (callbackUrl == null) callbackUrl = "";
                if (string.IsNullOrEmpty(goodsName)) goodsName = goodsId;
                if (string.IsNullOrEmpty(googdsDesc)) googdsDesc = goodsId;

                OrderInfo orderInfo = new OrderInfo();

                orderInfo.goodsID = goodsId;
                orderInfo.goodsName = goodsName;
                orderInfo.goodsDesc = googdsDesc;
                orderInfo.quantifier = "个";
                orderInfo.extrasParams = externParam;
                orderInfo.count = 1;
                orderInfo.amount = amount;
                orderInfo.price = amount;
                orderInfo.cpOrderID = orderId;
                orderInfo.callbackUrl = callbackUrl;

                if (m_GameRoleInfo == null)
                {
                    m_GameRoleInfo.gameRoleID = userId;
                    m_GameRoleInfo.gameRoleName = userId;
                    m_GameRoleInfo.serverID = "0";
                    m_GameRoleInfo.serverName = "test";
                    m_GameRoleInfo.roleCreateTime = DateTime.Now.Ticks.ToString();
                    m_GameRoleInfo.vipLevel = "1";
                    m_GameRoleInfo.partyName = "test";
                    m_GameRoleInfo = new GameRoleInfo();
                }

                quicksdk.QuickSDK.getInstance().pay(orderInfo, m_GameRoleInfo);
                return true;
            }
#endif
            return false;
        }
        //------------------------------------------------------
        static Dictionary<string,string> ms_vDirMaps = new System.Collections.Generic.Dictionary<string, string>();
        internal static void LogEvent(string eventName, string parameterName, string parameterValue)
        {
#if USE_INNER_QUICKSDK
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_INNER_QUICKSDK
            Debug.Log("Begin Quick SDK Init!");
            InnerQuickSDKCfg quickCfg = (InnerQuickSDKCfg)cfg;
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
#if USE_INNER_QUICKSDK
        class QuickSdDKCallback : QuickSDKListener
        {
            InnerQuickSDKUnity m_pAgent;
            public void Set(InnerQuickSDKUnity agent)
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
                param.channel = InnerQuickSDKUnity.GetChannelName();
                m_pAgent.OnSDKAction(param);
            }
            //------------------------------------------------------
            public override void onLoginSuccess(quicksdk.UserInfo userInfo)
            {
                //登录成功的回调
                Debug.Log("onLoginSuccess  " + "uid: " + userInfo.uid + " ,username: " + userInfo.userName + " ,userToken: " + userInfo.token + ", msg: " + userInfo.errMsg);
                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.LoginSucces;
                param.uid = userInfo.uid;
                param.name = userInfo.userName;
                param.channel = InnerQuickSDKUnity.GetChannelName();
                m_pAgent.OnSDKAction(param);

                GameSDK.ShowFloatMenu(true);
            }
            //------------------------------------------------------
            public override void onSwitchAccountSuccess(quicksdk.UserInfo userInfo)
            {
                //切换账号成功的回调
                //一些渠道在悬浮框有切换账号的功能，此回调即切换成功后的回调。游戏应清除当前的游戏角色信息。在切换账号成功后回到选择服务器界面，请不要再次调用登录接口。
                Debug.Log("onSwitchAccountSuccess   " + "uid: " + userInfo.uid + " ,username: " + userInfo.userName + " ,userToken: " + userInfo.token + ", msg: " + userInfo.errMsg);

                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.SwitchAccount;
                param.uid = userInfo.uid;
                param.name = userInfo.userName;
                param.channel = InnerQuickSDKUnity.GetChannelName();
                m_pAgent.OnSDKAction(param);
            }
            //------------------------------------------------------
            public override void onLoginFailed(quicksdk.ErrorMsg errMsg)
            {
                //登录失败的回调
                //如果游戏没有登录按钮，应在这里再次调用登录接口
                Debug.Log("onLoginFailed   " + "msg: " + errMsg.errMsg);

                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.LoginFail;
                param.msg = errMsg.errMsg;
                param.channel = InnerQuickSDKUnity.GetChannelName();
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
                param.channel = InnerQuickSDKUnity.GetChannelName();
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
                param.cpOrderId = payResult.cpOrderId;
                param.orderId = payResult.orderId;
                param.msg = payResult.extraParam;
                param.channel = InnerQuickSDKUnity.GetChannelName();
                m_pAgent.OnSDKAction(param);
            }
            //------------------------------------------------------
            public override void onPayCancel(PayResult payResult)
            {
                //支付取消的回调
                Debug.Log("onPayCancel   " + "orderId: " + payResult.orderId  + " ,extraParam" + payResult.extraParam);

                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.PayCancel;
                param.cpOrderId = payResult.cpOrderId;
                param.orderId = payResult.orderId;
                param.msg = payResult.extraParam;
                param.channel = InnerQuickSDKUnity.GetChannelName();
                m_pAgent.OnSDKAction(param);
            }
            //------------------------------------------------------
            public override void onPayFailed(PayResult payResult)
            {
                //支付失败的回调
                Debug.Log("onPayFailed  " + "orderId: " + payResult.orderId + " ,extraParam" + payResult.extraParam);
                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.PayFail;
                param.cpOrderId = payResult.cpOrderId;
                param.orderId = payResult.orderId;
                param.msg = payResult.extraParam;
                param.channel = InnerQuickSDKUnity.GetChannelName();
                m_pAgent.OnSDKAction(param);
            }
            //------------------------------------------------------
            public override void onExitSuccess()
            {
                Debug.Log("onExitSuccess");
                //SDK退出成功的回调
                //在此处调用QuickSDK.getInstance().exitGame()函数即可实现退出游戏，杀进程。为避免与渠道发生冲突，请不要使用Application.Quit()函数
                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.Exit;
                param.channel = InnerQuickSDKUnity.GetChannelName();
                m_pAgent.OnSDKAction(param);
                QuickSDK.getInstance().exitGame();
            }
            //------------------------------------------------------
            public override void onSucceed(string infos)
            {
                Debug.Log("onSucceed:" + infos);
//                 SDKCallbackParam param = new SDKCallbackParam();
//                 param.action = ESDKActionType.Message;
//                 param.msg = infos;
//                 param.channel = InnerQuickSDKUnity.GetChannelName();
//                 m_pAgent.OnSDKAction(param);
            }
            //------------------------------------------------------
            public override void onFailed(string message)
            {
                Debug.Log("onFailed  " + "msg: " + message);
                SDKCallbackParam param = new SDKCallbackParam();
                param.action = ESDKActionType.Message;
                param.msg = message;
                param.channel = InnerQuickSDKUnity.GetChannelName();
                m_pAgent.OnSDKAction(param);
            }
        }
#endif
#endregion
    }
}