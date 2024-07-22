using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct VivoCfg : ISDKConfig
    {
        public string appID;
        public string appKey;
        public string cpId;
        public bool passPrivacy;
        public string assetFile;
        public MonoBehaviour monoBinder;
    }
    public class VivoUnity : ISDKAgent
    {
#if USE_VIVO
        static AndroidJavaObject ms_Handle;
        VivoCfg m_Config;
        ISDKAgent m_pAgent = null;
        static bool ms_bInited = false;
        static Dictionary<string, object> ms_vEventTemp = null;
#endif
        //------------------------------------------------------
        public static VivoUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_VIVO
            ms_bInited = false;
            VivoUnity agent = new VivoUnity();
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
                ms_Handle.CallStatic("Login", "vivo","");
            }
#endif
        }
        //------------------------------------------------------
        public static void Logout()
        {
#if USE_OPPO
            if(ms_bInited && ms_Handle!=null)
            {
                ms_Handle.CallStatic("Logout", "vivo");
            }
#endif
        }
#if USE_OPPO
        //------------------------------------------------------
        public static void Pay(int amount, string googdsName, string goodsDes, string userId, string callback)
        {
#if USE_OPPO
            if(ms_bInited && ms_Handle!=null)
            {
                var vivoSetting = Framework.Core.BaseUtil.stringBuilder;
                vivoSetting.AppendLine("{");
                vivoSetting.Append("\t\"amount\"").Append(":").Append("\"").Append(amount).AppendLine("\",");
                vivoSetting.Append("\t\"goodsName\"").Append(":").Append("\"").Append(googdsName).AppendLine("\",");
                vivoSetting.Append("\t\"goodsDes\"").Append(":").Append("\"").Append(goodsDes).AppendLine("\",");
                vivoSetting.Append("\t\"userId\"").Append(":").Append("\"").Append(userId).AppendLine("\",");
                vivoSetting.Append("\t\"order\"").Append(":").Append("\"").Append("").AppendLine("\",");
                vivoSetting.Append("\t\"callback\"").Append(":").Append("\"").Append(callback).AppendLine("\"");
                vivoSetting.AppendLine("}");
                ms_Handle.CallStatic("Pay", "vivo", vivoSetting.ToString());
            }
#endif
        }
#endif
#if USE_VIVO
        internal VivoCfg GetConfig()
        {
            return m_Config;
        }
#endif
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_VIVO
            m_Config = (VivoCfg)cfg;
            if (m_Config.monoBinder == null)
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
            VivoSDKCallback pCallback = m_Config.monoBinder.gameObject.AddComponent<VivoSDKCallback>();
            pCallback.Set(this);
            ms_Handle.CallStatic("SetListener", pCallback.gameObject.name);

            var oppoSetting = Framework.Core.BaseUtil.stringBuilder;
            oppoSetting.AppendLine("{");
            oppoSetting.Append("\t\"appId\"").Append(":").Append("\"").Append(m_Config.appID).AppendLine("\",");
            oppoSetting.Append("\t\"appKey\"").Append(":").Append("\"").Append(m_Config.appKey).AppendLine("\",");
            oppoSetting.Append("\t\"cpId\"").Append(":").Append("\"").Append(m_Config.cpId).AppendLine("\",");
            oppoSetting.Append("\t\"order\"").Append(":").Append("\"").Append("").AppendLine("\",");
            oppoSetting.Append("\t\"passPrivacy\"").Append(":").Append("\"").Append(m_Config.passPrivacy).AppendLine("\"");
            oppoSetting.AppendLine("}");
            if(ms_Handle.CallStatic<bool>("Init", "vivo", oppoSetting.ToString()))
                return true;
            GameObject.Destroy(pCallback);
            return false;
#else
            return false;
#endif
        }
#if USE_VIVO
        class VivoSDKCallback : MonoBehaviour
        {
            VivoUnity m_pAgent;
            public void Set(VivoUnity agent)
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
                SDKCallbackParam msg = new SDKCallbackParam(ESDKActionType.LoginSucces);
                m_pAgent.OnSDKAction(msg);
            }

            void OnLoginFail(string msg)
            {
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.LoginFail,msg));
            }
            void OnExitGame(string msg)
            {
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.LogoutSucces,msg));
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
        }
#endif
    }
}