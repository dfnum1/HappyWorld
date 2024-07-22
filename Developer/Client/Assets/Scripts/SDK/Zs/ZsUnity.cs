using System;
using System.Collections.Generic;
using UnityEngine;
namespace SDK
{
    [System.Serializable]
    public struct ZsCfg : ISDKConfig
    {
        public string assetFile;
        public MonoBehaviour binderMono;
    }
    public class ZsUnity : ISDKAgent
    {
#if USE_ZSSDK
        [System.Serializable]
        struct Player
        {
            public string uid;
            public string token;
            public string nickName;
            public string device;
        }
        ISDKCallback m_pCallback;
        ZsCfg m_Config;    
        static bool ms_bInited = false;
        static ZsUnity ms_pAgent = null;
        static AndroidJavaObject ms_Handle;
#endif
        //------------------------------------------------------
        public static ZsUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_ZSSDK
            ms_bInited = false;
            ZsUnity agent = new ZsUnity();
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
#if USE_ZSSDK
            if (ms_bInited)
            {
                if (!ms_bInited || ms_Handle == null)
                {
                    return;
                }
                ms_Handle.CallStatic("Login", "zs", "");
            }
#endif
        }
        //------------------------------------------------------
        public static void Logout()
        {
#if USE_ZSSDK
            if (ms_bInited)
            {
                if (!ms_bInited || ms_Handle == null)
                {
                    return;
                }
                ms_Handle.CallStatic("Logout", "zs");
            }
#endif
        }
#if USE_ZSSDK
        //------------------------------------------------------
        public static bool Pay(string userId, string orderId, string goodsId, double amount, string googdsName, string externParam, string callback)
        {
#if USE_ZSSDK
            if (ms_bInited && ms_Handle!=null)
            {
                var oppoSetting = Framework.Core.BaseUtil.stringBuilder;
                oppoSetting.AppendLine("{");
                oppoSetting.Append("\t\"order\"").Append(":").Append("\"").Append(orderId).AppendLine("\",");
                oppoSetting.Append("\t\"goodsId\"").Append(":").Append("\"").Append(goodsId).AppendLine("\",");
                oppoSetting.Append("\t\"price\"").Append(":").Append("\"").Append(amount.ToString("F2")).AppendLine("\",");
                oppoSetting.Append("\t\"userId\"").Append(":").Append("\"").Append(userId).AppendLine("\",");
                oppoSetting.Append("\t\"count\"").Append(":").Append("\"").Append("1").AppendLine("\",");
                oppoSetting.Append("\t\"goodsName\"").Append(":").Append("\"").Append(googdsName).AppendLine("\",");
                oppoSetting.Append("\t\"extInfo\"").Append(":").Append("\"").Append(externParam).AppendLine("\",");
                oppoSetting.Append("\t\"callback\"").Append(":").Append("\"").Append(callback).AppendLine("\"");
                oppoSetting.AppendLine("}");
                ms_Handle.CallStatic("Pay", "zs", oppoSetting.ToString());
                return true;
            }
#endif
            return false;
        }
#endif
#if USE_ZSSDK
        internal ZsCfg GetConfig()
        {
            return m_Config;
        }
#endif
        //------------------------------------------------------
        public static void UpdateRole(string userId, string nickName, string sevrName, string svrId, string level, bool isCreate)
        {
#if USE_ZSSDK
            if (ms_bInited)
            {
                if (!ms_bInited || ms_Handle == null)
                {
                    return;
                }
                var upSetting = Framework.Core.BaseUtil.stringBuilder;
                upSetting.AppendLine("{");
                upSetting.Append("\t\"userId\"").Append(":").Append("\"").Append(userId).AppendLine("\",");
                upSetting.Append("\t\"userName\"").Append(":").Append("\"").Append(nickName).AppendLine("\",");
                upSetting.Append("\t\"svrName\"").Append(":").Append("\"").Append(sevrName).AppendLine("\",");
                upSetting.Append("\t\"svrId\"").Append(":").Append("\"").Append(svrId).AppendLine("\",");
                upSetting.Append("\t\"level\"").Append(":").Append("\"").Append(level).AppendLine("\",");
                upSetting.Append("\t\"isNewCreate\"").Append(":").Append(isCreate?"true":"false").AppendLine();
                upSetting.AppendLine("}");
                ms_Handle.CallStatic("UpdateRole", "zs", upSetting.ToString());
            }
#endif
        }
        //------------------------------------------------------
        public static void Exit()
        {
#if USE_ZSSDK
            if (ms_bInited)
            {
                if (!ms_bInited || ms_Handle == null)
                {
                    return;
                }
            }
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_ZSSDK
            m_Config = (ZsCfg)cfg;
            if (m_Config.binderMono == null)
            {
                Debug.LogWarning("unity go is null");
                return false;
            }
            ms_Handle = GameSDK.GetSDKHandler();
            if (ms_Handle == null)
            {
                Debug.LogWarning("unfind \"com.unity.sdks.SDKHandler\"");
                return false;
            }
            SDKCallback pCallback = m_Config.binderMono.gameObject.AddComponent<SDKCallback>();
            pCallback.Set(this);
            ms_Handle.CallStatic("SetListener", m_Config.binderMono.gameObject.name);
            Debug.Log("ZS SDK Begin Init");
            if (ms_Handle.CallStatic<bool>("Init", "zs", ""))
            {
                return true;
            }
            Debug.Log("ZS SDK Init Fail");
            GameObject.Destroy(pCallback);
            return true;
#else
            return false;
#endif
        }
#if USE_ZSSDK
        class SDKCallback : MonoBehaviour
        {
            ZsUnity m_pAgent = null;
            public void Set(ZsUnity agent)
            {
                m_pAgent = agent;
            }

            void onInitSuccess()
            {
                Debug.Log("ZS SDK Inited");
                ms_bInited = true;
                m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.InitSucces) { name = m_pAgent.GetConfig().assetFile, channel = "ZSSdk" });
            }
            void onInitFailure(string msg)
            {
                m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.InitFail) {msg = msg, channel = "ZSSdk" });
            }

            void onLoginSuccess(string msg)
            {
                try
                {
                    Player player = JsonUtility.FromJson<Player>(msg);
                    SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LoginSucces);
                    sdkParam.uid = player.uid;
                    sdkParam.channel = "ZSSdk";
                    if(string.IsNullOrEmpty(player.nickName)) player.nickName = player.uid;
                    sdkParam.name = player.nickName;
                    m_pAgent.OnSDKAction(sdkParam);
                    GameSDK.ShowFloatMenu(true);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
            void onLoginFailure(string msg)
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LoginFail);
                sdkParam.channel = "ZSSdk";
                sdkParam.msg = msg;
                m_pAgent.OnSDKAction(sdkParam);

            }

            void onLogoutSuccess()
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LogoutSucces);
                sdkParam.channel = "ZSSdk";
                m_pAgent.OnSDKAction(sdkParam);
            }
            void onLogoutFailure(string msg)
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LogoutFail);
                sdkParam.channel = "ZSSdk";
                m_pAgent.OnSDKAction(sdkParam);
            }
            void onExitSuccess()
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.Exit);
                sdkParam.channel = "ZSSdk";
                m_pAgent.OnSDKAction(sdkParam);
            }
            void onExitError(string msg)
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.Message);
                sdkParam.channel = "ZSSdk";
                sdkParam.msg = msg;
                m_pAgent.OnSDKAction(sdkParam);
            }

            void onPayEnd(string msg)
            {
                Debug.Log("Zs Pay:" + msg);
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.PaySucces);
                sdkParam.channel = "ZSSdk";
                sdkParam.cpOrderId = msg;
                m_pAgent.OnSDKAction(sdkParam);
            }
            void onPayFailure(string msg)
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.PayFail);
                sdkParam.channel = "ZSSdk";
                sdkParam.msg = msg;
                m_pAgent.OnSDKAction(sdkParam);
            }
        }
#endif
    }
}