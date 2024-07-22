using System;
using System.Collections.Generic;

#if USE_TAPTAP
using TapTap.Bootstrap;
using TapTap.Common;
using TapTap.AntiAddiction;
using TapTap.AntiAddiction.Model;
using LeanCloud.Storage;
using TapTap.TapDB;
//using TapTap.Payment;
#endif
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct TaptapCfg : ISDKConfig
    {
        public string clientID;
        public string clientToken;
        public string serverUrl;
        public bool testQualification;
        public string assetFile;
        public MonoBehaviour binderMono;
    }
    public class TaptapUnity : ISDKAgent
    {
#if USE_TAPTAP
        TaptapCfg m_Config;
        ISDKAgent m_pAgent = null;
        static bool ms_TestQualification = false;
        static bool ms_bInited = false;
        static Dictionary<string, object> ms_vEventTemp = null;
#endif
        //------------------------------------------------------
        public static TaptapUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_TAPTAP
            ms_bInited = false;
            TaptapUnity agent = new TaptapUnity();
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
#if USE_TAPTAP
        //------------------------------------------------------
        public TaptapCfg GetConfig()
        {
            return m_Config;
        }
#endif
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_TAPTAP
            m_Config = (TaptapCfg)cfg;
            var config = new TapConfig.Builder()
               .ClientID(m_Config.clientID) // 必须，开发者中心对应 Client ID
               .ClientToken(m_Config.clientToken) // 必须，开发者中心对应 Client Token
                .ServerURL(m_Config.serverUrl) // 必须，开发者中心 > 你的游戏 > 游戏服务 > 基本信息 > 域名配置 > API
                .RegionType(RegionType.CN) // 非必须，CN 表示中国大陆，IO 表示其他国家或地区
                .TapDBConfig(true,"taptap","", false)
                /*.TapPaymentConfig("CN","zh_CN","")*/
               .ConfigBuilder();
            TapBootstrap.Init(config);
            ms_TestQualification = m_Config.testQualification;
            Debug.Log("Taptap 初始化成功");

            AntiAddictionConfig antiCfg = new AntiAddictionConfig()
            {
                gameId = m_Config.clientID,
                useTapLogin = true,
                showSwitchAccount = false,
            };
            AntiAddictionUIKit.Init(antiCfg, OnAntiAddictionCallback);
            TapDB.ConfigAutoReportLogLevel( TapTap.TapDB.LogSeverity.LogError);
            TapDB.ConfigAutoQuitApplication(false);
       //     TapTap.Payment.TapPayment.Init(config);
            TapDB.RegisterLogCallback(logcallback);
            GameSDK.DoCallback(this, new SDKCallbackParam(ESDKActionType.InitSucces) { name = m_Config.assetFile });

            return true;
#else
            return false;
#endif
        }
#if USE_TAPTAP
        //------------------------------------------------------
        public void logcallback(string condition, string statckTrace, LogType type)
        {

        }
        //------------------------------------------------------
        void OnAntiAddictionCallback(int code, string error)
        {
            switch(code)
            {
                case 500: //玩家登录后判断当前玩家可以进行游戏
                    {
                        StartAntiAddiction();
                        if (m_pAgent != null) m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.Message) { msg= "已实名，可进入游戏" });
                    }
                    break;
                case 1000: //退出账号
                    {
                        //ExitAntiAddiction();
                        if (m_pAgent != null)
                        {
                            m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.Message) { msg = "已实名，可进入游戏" });
                            m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.LogoutSucces));
                        }
                    }
                    break;
                case 1001: //点击切换账号按钮
                    {
                        ExitAntiAddiction();
                        if (m_pAgent != null) m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.LogoutSucces));
                    }
                    break;
                case 1030: //未成年玩家当前无法进行游戏
                    {
                        if (m_pAgent != null)
                        {
                            m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.Message) { msg = "未成年玩家当前无法进行游戏" });
                            m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.LogoutSucces));
                        }
                    }
                    break;
                case 1095:
                    {
                        if (m_pAgent != null)
                        {
                            m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.Message) { msg = "未成年允许游戏弹窗" });
                            m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.LogoutSucces));
                        }
                    }
                    break;
                case 1050: //时长限制
                    {
                        if (m_pAgent != null) m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.LogoutSucces));
                    }
                    break;
                case 9002: //实名过程中点击了关闭实名窗
                    {
                        if (m_pAgent != null) m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.LogoutSucces));
                    }
                    break;
            }
        }
#endif
        //------------------------------------------------------
        public static async void Login(string type)
        {
#if USE_TAPTAP
            if(!string.IsNullOrEmpty(type))
            {
                if (type.Contains("youke"))
                {
                    try
                    {
                        var tdsUser = await TDSUser.LoginAnonymously();
                        LoginAfter(tdsUser);
                    }
                    catch (Exception e)
                    {
                        LoginExecption(e);
                    }
                    return;
                }
                else if (type.Contains("other"))
                {
                    type = type.Replace("other", "");
                    Dictionary<string, object> thirdPartyData = new Dictionary<string, object> {
                    // 必须
                    { "openid", "OPENID" },
                    { "access_token", "ACCESS_TOKEN" },
                    { "expires_in", 7200 },

                    // 可选
                    { "refresh_token", "REFRESH_TOKEN" },
                    { "scope", "SCOPE" }
                    };
                    try
                    {
                        TDSUser tdsUser = await TDSUser.LoginWithAuthData(thirdPartyData, type);
                       // UnityNativeToastsHelper.ShowShortText("使用微信登录成功：" + currentUser.ToString());
                    }
                    catch (Exception e)
                    {
                        LoginExecption(e, "第三方账户登录异常");
                    }
                    return;
                }
            }
  
            var currentUser = await TDSUser.GetCurrent();
            if (null == currentUser)
            {
                Debug.Log("当前未登录");
                // 开始登录
                try
                {
                    var tdsUser = await TDSUser.LoginWithTapTap();
                    LoginAfter(tdsUser);
                }
                catch (Exception e)
                {
                    LoginExecption(e);
                }
            }
            else
            {
                LoginAfter(currentUser);
            }
#endif
        }
#if USE_TAPTAP
        //------------------------------------------------------
        static void LoginExecption(Exception e, string msg = null)
        {
            SDKCallbackParam param = new SDKCallbackParam();
            param.action = ESDKActionType.LoginFail;
            if (e is TapException tapError)
            {
                param.msg = tapError.message;
                Debug.Log($"encounter exception:{tapError.code} message:{tapError.message}");
                if (tapError.code == (int)TapErrorCode.ERROR_CODE_BIND_CANCEL) // 取消登录
                {
                    Debug.Log("登录取消");
                }
            }
            else if(string.IsNullOrEmpty(msg)) msg = e.Message;
            param.msg = msg;
            GameSDK.DoCallback(null, param);
        }
        //------------------------------------------------------
        static async void LoginAfter(TDSUser user)
        {
            if (ms_TestQualification)
            {
                SDKCallbackParam msg = new SDKCallbackParam();
                msg.action = ESDKActionType.Message;
                var test = await TapTap.Login.TapLogin.GetTestQualification();
                if (test)
                {
                    msg.msg = "该玩家具备篝火测试资格";
                    GameSDK.DoCallback(null, msg);
                }
                else
                {
                    msg.msg = "该玩家不具备篝火测试资格";
                    GameSDK.DoCallback(null, msg);

                    SDKCallbackParam callparam = new SDKCallbackParam();
                    callparam.action = ESDKActionType.LoginFail;
                    GameSDK.DoCallback(null, callparam);

                    return;
                }
            }

            SDKCallbackParam param = new SDKCallbackParam();
            param.action = ESDKActionType.LoginSucces;
            param.name = user["nickname"].ToString();
            param.uid = user.ObjectId;
            param.channel = "Taptap";
            GameSDK.DoCallback(null, param);
            TapTap.TapDB.TapDB.SetUser(user.ObjectId);
            TapTap.TapDB.TapDB.SetName(param.name);
            
            Debug.Log("Taptap 登录:" + param.ToString());
        }
#endif
        //------------------------------------------------------
        public static async void Logout()
        {
#if USE_TAPTAP
            await TDSUser.Logout();
            ExitAntiAddiction();
            GameSDK.DoCallback(null, new SDKCallbackParam( ESDKActionType.LogoutSucces));
#endif
        }
        //------------------------------------------------------
        public static void UpdateRole(string userId, string nickName, string sevrName, string svrId, string level, bool isCreate)
        {

        }
#if USE_TAPTAP
        //------------------------------------------------------
        public static bool Pay(string userId, string orderId, double amount, string goodsId, string goodsName, string googdsDesc = null, string externParam = null, string callbackUrl = null)
        {
            return false;
        }
#endif
        //------------------------------------------------------
        public static void LogEvent(string logEvent)
        {
#if USE_TAPTAP
            if (logEvent.CompareTo("Login") == 0)
            {
                TapTap.TapDB.TapDB.SetUser(GameSDK.GetEventV("uid"));
                TapTap.TapDB.TapDB.SetUser(GameSDK.GetEventV("name"));
                TapTap.TapDB.TapDB.SetUser(GameSDK.GetEventV("level"));
                return;
            }
            var kvs = GameSDK.GetEventKVs();
            if (kvs == null || kvs.Count <= 0) return;

            var text= Framework.Core.BaseUtil.stringBuilder;
            text.Append("{");
            foreach(var db in kvs)
            {
                text.AppendFormat("\"#{0}\":\"{1}\",", db.Key, db.Value);
            }
            if (text.Length > 0 && text[text.Length - 1] == ',') text.Length -= 1;
            text.Append("}");
            TapTap.TapDB.TapDB.TrackEvent("#" +logEvent, text.ToString());
#endif
        }
        //------------------------------------------------------
        public static void StartAntiAddiction()
        {
#if USE_TAPTAP
            AntiAddictionUIKit.EnterGame();
#endif
        }
        //------------------------------------------------------
        public static void LeaveAntiAddiction()
        {
#if USE_TAPTAP
            AntiAddictionUIKit.LeaveGame();
#endif
        }
        //------------------------------------------------------
        public static void ExitAntiAddiction()
        {
#if USE_TAPTAP
            AntiAddictionUIKit.Exit();
#endif
        }
        //------------------------------------------------------
        public static int GetAntiAddictionReaminningTime()
        {
#if USE_TAPTAP
            return AntiAddictionUIKit.RemainingTime;
#endif
            return 0;
        }
        //------------------------------------------------------
        public static void AntiAddictionIdentifier(string userIdentifier)
        {
#if USE_TAPTAP
            TaptapUnity.AntiAddictionIdentifier(userIdentifier);
#endif
        }
        //------------------------------------------------------
        public static async void CloudStatistics(string group, int rankTopCount = 10)
        {
#if USE_TAPTAP
            var leaderboard = LCLeaderboard.CreateWithoutData(group);

            var rankings = await leaderboard.GetResults(limit: rankTopCount);
            int index = 0;
            foreach (var db in rankings)
            {
                GameSDK.AddStatisticRank(db.Object.ObjectId, db.User["nickname"].ToString(), db.Value, 0, index == 0);
                index++;
            }
            GameSDK.DoCallback(null, new SDKCallbackParam(ESDKActionType.StatisticsRank) { name = group });
#endif
        }
        //------------------------------------------------------
        public static async void SearchCloudStatistics(string uid, string group)
        {
#if USE_TAPTAP
            var statistics = await LCLeaderboard.GetStatistics(uid,new List<string> { group });
            foreach (var db in statistics)
            {
                GameSDK.AddStatisticRank(db.Object.ObjectId, db.User["nickname"].ToString(), db.Value);
            }
            GameSDK.DoCallback(null, new SDKCallbackParam(ESDKActionType.StatisticsRank) { name = group, uid = uid });
#endif
        }
        //------------------------------------------------------
        public static async void UpdateStatistic(string uid, Dictionary<string, double> statistic)
        {
#if USE_TAPTAP
            await LCLeaderboard.UpdateStatistics(uid, statistic, overwrite:true);
            GameSDK.DoCallback(null, new SDKCallbackParam(ESDKActionType.StatisticsRankUpdate) { uid = uid });
#endif
        }
    }
}