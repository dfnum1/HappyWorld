using System;
using System.Collections.Generic;
using UnityEngine;
namespace SDK
{
    public class Unity233 : ISDKAgent
    {
#if USE_233SDK
        [System.Serializable]
        struct Player
        {
            public string uid;
            public string token;
            public string nickName;
            public string bindPhone;
			public string age;
			public bool isGuest;
			public string avatar;
        }
        ISDKCallback m_pCallback;
        ZsCfg m_Config;    
        static bool ms_bInited = false;
        static Unity233 ms_pAgent = null;
        static AndroidJavaObject ms_Handle;
#endif
        //------------------------------------------------------
        public static Unity233 StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_233SDK
            ms_bInited = false;
            Unity233 agent = new Unity233();
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
#if USE_233SDK
            if (ms_bInited)
            {
                if (!ms_bInited || ms_Handle == null)
                {
                    return;
                }
                ms_Handle.CallStatic("Login", "233", "");
            }
#endif
        }
        //------------------------------------------------------
        public static void Logout()
        {
#if USE_233SDK
            if (ms_bInited)
            {
                if (!ms_bInited || ms_Handle == null)
                {
                    return;
                }
                ms_Handle.CallStatic("Logout", "233", "");
            }
#endif
        }
#if USE_233SDK
        //------------------------------------------------------
        public static bool Pay(string userId, string orderId, string goodsId, double amount, string googdsName, string externParam, string callback)
        {
#if USE_233SDK
            if (ms_bInited && ms_Handle!=null)
            {
                var oppoSetting = Framework.Core.BaseUtil.stringBuilder;
                oppoSetting.AppendLine("{");
                oppoSetting.Append("\t\"order\"").Append(":").Append("\"").Append(orderId).AppendLine("\",");
                oppoSetting.Append("\t\"goodsId\"").Append(":").Append("\"").Append(goodsId).AppendLine("\",");
                oppoSetting.Append("\t\"price\"").Append(":").Append("\"").Append((int)(amount*100)).AppendLine("\",");
                oppoSetting.Append("\t\"userId\"").Append(":").Append("\"").Append(userId).AppendLine("\",");
                oppoSetting.Append("\t\"count\"").Append(":").Append("\"").Append("1").AppendLine("\",");
                oppoSetting.Append("\t\"goodsName\"").Append(":").Append("\"").Append(googdsName).AppendLine("\",");
                oppoSetting.Append("\t\"extInfo\"").Append(":").Append("\"").Append(externParam).AppendLine("\",");
                oppoSetting.Append("\t\"callback\"").Append(":").Append("\"").Append(callback).AppendLine("\"");
                oppoSetting.AppendLine("}");
                ms_Handle.CallStatic("Pay", "233", oppoSetting.ToString());
                return true;
            }
#endif
            return false;
        }
#endif
#if USE_233SDK
        internal ZsCfg GetConfig()
        {
            return m_Config;
        }
#endif
        //------------------------------------------------------
        public static void Exit()
        {
#if USE_233SDK
            if (ms_bInited && ms_Handle != null) ms_Handle.CallStatic("Exit", "233", "");
#endif
        }
		//------------------------------------------------------
		public static void ShowAd(string type, string posId)
		{
#if USE_233SDK
			if (ms_bInited && ms_Handle != null)
            {
				try{
					int posInt;
					if(int.TryParse(posId, out posInt))
						ms_Handle.CallStatic("Ads", "233", "type", posInt);
				}
				catch(System.Exception e)
				{
					Debug.LogError(e.ToString());
				}
            }
#endif
		}
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_233SDK
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
            Debug.Log("233 SDK Begin Init");
            if (ms_Handle.CallStatic<bool>("Init", "233", ""))
            {
                return true;
            }
            Debug.Log("233 SDK Init Fail");
            GameObject.Destroy(pCallback);
            return true;
#else
            return false;
#endif
        }
#if USE_233SDK
        class SDKCallback : MonoBehaviour
        {
            Unity233 m_pAgent = null;
            public void Set(Unity233 agent)
            {
                m_pAgent = agent;
            }

            void onInitSuccess()
            {
				Debug.Log("233 SDK Inited");
                ms_bInited = true;
                m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.InitSucces) { name = m_pAgent.GetConfig().assetFile, channel = "233" });
            }
            void onInitFailure(string msg)
            {
                m_pAgent.OnSDKAction(new SDKCallbackParam(ESDKActionType.InitFail) {msg = msg, channel = "233" });
            }

            void onLoginSuccess(string msg)
            {
                try
                {
                    Player player = JsonUtility.FromJson<Player>(msg);
                    SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LoginSucces);
                    sdkParam.uid = player.uid;
                    sdkParam.channel = "233";
                    sdkParam.name = player.nickName;
                    m_pAgent.OnSDKAction(sdkParam);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
            void onLoginFailure(string msg)
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LoginFail);
                sdkParam.channel = "233";
                sdkParam.msg = msg;
                m_pAgent.OnSDKAction(sdkParam);

            }
			void onSiwtchAccount(string msg)
			{
				try
                {
                    Player player = JsonUtility.FromJson<Player>(msg);
                    SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.SwitchAccount);
                    sdkParam.uid = player.uid;
                    sdkParam.channel = "233";
                    sdkParam.name = player.nickName;
                    m_pAgent.OnSDKAction(sdkParam);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }		
			}

            void onLogoutSuccess()
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LogoutSucces);
                sdkParam.channel = "233";
                m_pAgent.OnSDKAction(sdkParam);
            }
            void onLogoutFailure(string msg)
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.LogoutFail);
                sdkParam.channel = "233";
                m_pAgent.OnSDKAction(sdkParam);
            }
            void onExitSuccess()
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.Exit);
                sdkParam.channel = "233";
                m_pAgent.OnSDKAction(sdkParam);
            }

            void OnPaySuccess(string msg)
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.PaySucces);
                sdkParam.channel = "233";
                sdkParam.msg = msg;
                m_pAgent.OnSDKAction(sdkParam);
            }
            void OnPayFail(string msg)
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.PayFail);
                sdkParam.channel = "233";
                sdkParam.msg = msg;
                m_pAgent.OnSDKAction(sdkParam);
            }
			void onAdShow(string ads)
			{
				
			}		
			void onAdShowFailed(string ads)
			{
				
			}
			void onAdClick(string ads)
			{
				
			}
			void onAdClose(string ads)
			{
				
			}
			void onAdClickSkip(string ads)
			{
				
			}
			void onAdReward(string ads)
			{
				
			}
        }
#endif
    }
}