using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct YingyongbaoCfg : ISDKConfig
    {
        public string assetFile;
        public MonoBehaviour monoBinder;
    }
    public class YingyongbaoUnity : ISDKAgent
    {
#if USE_YINGYONGBAO
        ISDKCallback m_pCallback;
        static AndroidJavaObject ms_Handle;
        YingyongbaoCfg m_Config;
        ISDKAgent m_pAgent = null;
        static bool ms_bInited = false;
        static Dictionary<string, object> ms_vEventTemp = null;
#endif
        //------------------------------------------------------
        public static YingyongbaoUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_YINGYONGBAO
            ms_bInited = false;
            YingyongbaoUnity agent = new YingyongbaoUnity();
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
        public static void Login(string platform="auto")
        {
#if USE_YINGYONGBAO
            if (ms_bInited && ms_Handle != null)
            {
                ms_Handle.Call("Login", "yingyongbao", platform);
            }
#endif
        }
        //------------------------------------------------------
        public static void Logout()
        {
#if USE_YINGYONGBAO
            if (ms_bInited && ms_Handle != null)
            {
                ms_Handle.Call("Logout", "yingyongbao");
            }
#endif
        }
#if USE_YINGYONGBAO
        //------------------------------------------------------
        public static void Pay(int amount, string googdsName, string goodsDes, string userId, string callback)
        {
#if USE_YINGYONGBAO
            if (ms_bInited && ms_Handle != null)
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
                ms_Handle.Call("Pay", "vivo", vivoSetting.ToString());
            }
#endif
        }
#endif
#if USE_YINGYONGBAO
        interface YingyongbaoCfg GetConfig()
        {
            return m_Config;
        }
#endif
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_YINGYONGBAO
            m_Config = (YingyongbaoCfg)cfg;
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
            YSDSDKCallback pCallback = m_Config.monoBinder.gameObject.AddComponent<YSDSDKCallback>();
            pCallback.Set(this);
            ms_Handle.Call("SetListener", pCallback.gameObject.name);

            var oppoSetting = Framework.Core.BaseUtil.stringBuilder;
            oppoSetting.AppendLine("{");
            oppoSetting.AppendLine("}");
            if(ms_Handle.CallStatic<bool>("Init", "vivo", oppoSetting.ToString()))
                return true;
            GameObject.Destroy(pCallback);
            return false;
#else
            return false;
#endif
        }
#if USE_YINGYONGBAO
        class YSDSDKCallback : MonoBehaviour
        {
            YingyongbaoUnity m_pAgent;
            public void Set(YingyongbaoUnity agent)
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
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.LoginSucces));
            }

            void OnLoginFail(string msg)
            {
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.LoginFail));
            }
            void OnExitGame(string msg)
            {
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.LogoutSucces));
            }
            void OnPaySuccess(string msg)
            {
                 m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.PaySucces));
            }
            void OnPayFail(string msg)
            {
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.PayFail));
            }
            void OnPayCancel(string msg)
            {
                m_pAgent.OnSDKAction( new SDKCallbackParam(ESDKActionType.PayCancel));
            }
            void OnRelationNotify(string msg)
            {
             //   m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.PayCancel));
            }
            void onWindowClose(string msg)
            {
                //   m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.PayCancel));
            }
        }
#endif
    }
}