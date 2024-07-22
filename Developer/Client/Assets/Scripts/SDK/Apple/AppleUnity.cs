using System;
using System.Collections.Generic;
using UnityEngine;
#if USE_APPLE
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
#endif

namespace SDK
{
    struct AppleCfg : ISDKConfig
    {
        public string assetFile;
        public bool useAuth;
        public GameObject monoObject;
    }
    public class AppleUnity : ISDKAgent
    {
#if USE_APPLE
        static bool ms_bInited = false;
        static AppleAuthManager ms_AppleAuthMgr;
		static ApplePay.IOSIAPMgr ms_IAPMgr;
#endif
        //------------------------------------------------------
        public static AppleUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_APPLE
            ms_bInited = false;
            AppleUnity agent = new AppleUnity();
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
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_APPLE
            if(!AppleAuth.AppleAuthManager.IsCurrentPlatformSupported)
                return false;
            AppleCfg appleCfg = (AppleCfg)cfg;
            ms_IAPMgr = appleCfg.monoObject.AddComponent<ApplePay.IOSIAPMgr>();
            if(appleCfg.useAuth)
            {
                var deserializer = new PayloadDeserializer();
                ms_AppleAuthMgr = new AppleAuthManager(deserializer);
                GameSDK.DoCallback(this, new SDKCallbackParam(ESDKActionType.InitSucces) { name = appleCfg.assetFile, channel = "apple" });
            }
            else
                GameSDK.DoCallback(this, new SDKCallbackParam(ESDKActionType.InitSucces) { channel = "apple" });

            return true;
#else
            return false;
#endif
        }
#if USE_APPLE
        //------------------------------------------------------
        public override void Update(float fTime)
        {
            base.Update(fTime);
            if (ms_AppleAuthMgr != null) ms_AppleAuthMgr.Update();
        }
#endif
        //------------------------------------------------------
        public static void Login()
        {
#if USE_APPLE
            ClearApplePayInfo();
            if (!ms_bInited || ms_AppleAuthMgr == null) return;
            var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);
            ms_AppleAuthMgr.LoginWithAppleId(
                loginArgs, 
                credential =>
                {
                    SDKCallbackParam callParam = new SDKCallbackParam();
                    callParam.action = ESDKActionType.LoginSucces;
                    callParam.uid = credential.User;
                    callParam.name = credential.User;
					callParam.channel = "apple";
                    GameSDK.DoCallback(null, callParam);
                },
                error =>
                {
                    var authorizationErrorCode = error.GetAuthorizationErrorCode();
                    SDKCallbackParam callParam = new SDKCallbackParam();
                    callParam.action = ESDKActionType.LoginFail;
                    callParam.name = authorizationErrorCode.ToString();
                    if (authorizationErrorCode == AuthorizationErrorCode.Canceled) callParam.msg = "he user canceled the authorization attempt";
                    else if (authorizationErrorCode == AuthorizationErrorCode.InvalidResponse) callParam.msg = "The authorization request received an invalid response";
                    else if (authorizationErrorCode == AuthorizationErrorCode.NotHandled) callParam.msg = "The authorization request wasn't handled";
                    else if (authorizationErrorCode == AuthorizationErrorCode.Failed) callParam.msg = "The authorization attempt failed"; ;
					callParam.channel = "apple";
                    GameSDK.DoCallback(null, callParam);
                });
#endif
        }
        //------------------------------------------------------
        public static void Logout()
        {
#if USE_APPLE
            ClearApplePayInfo();
            if (!ms_bInited || ms_AppleAuthMgr == null) return;
            SDKCallbackParam callParam = new SDKCallbackParam();
            callParam.action = ESDKActionType.LogoutSucces;
			callParam.channel = "apple";
            GameSDK.DoCallback(null, callParam);
#endif
        }
        //------------------------------------------------------
        public static void InitProdocts(List<string> products)
		{
#if USE_APPLE
			if(ms_IAPMgr!=null) ms_IAPMgr.InitPay(products);
#endif			
		}
#if USE_APPLE
        static string ms_apple_goodsId = null;
        static string ms_apple_order_id = null;
        internal static void ClearApplePayInfo()
        {
            ms_apple_goodsId = null;
            ms_apple_order_id = null;
        }
        //------------------------------------------------------
        internal static bool IsBuying()
        {
            if (!string.IsNullOrEmpty(ms_apple_goodsId))
            {
                return true;
            }
            if (ms_IAPMgr != null) return ms_IAPMgr.isBuying();
            return false;
        }
        //------------------------------------------------------
        public static bool Pay(string userId, string orderId, string goodsId, double amount, string googdsName, string externParam, string callback)
        {
            if(!string.IsNullOrEmpty(ms_apple_goodsId))
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.Message);
                sdkParam.channel = "apple";
                sdkParam.msg = "正在购买中，请稍后！";
                GameSDK.DoCallback(null, sdkParam);
                return false;
            }
            if (ms_IAPMgr == null)
            {
                ClearApplePayInfo();
                return false;
            }
            if (!ms_IAPMgr.IsProductVailable())
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.Message);
                sdkParam.channel = "apple";
                sdkParam.msg = "不支持购买支付行为！";
                GameSDK.DoCallback(null, sdkParam);
                ClearApplePayInfo();
                return false;
            }
            if (ms_IAPMgr.isBuying())
            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.Message);
                sdkParam.channel = "apple";
                sdkParam.msg = "正在购买中，请稍后！";
                GameSDK.DoCallback(null, sdkParam);
                return false;
            }
			ms_IAPMgr.OnPaySucessed = OnPaySucessed;
			ms_IAPMgr.OnPayFailed = OnPayFailed;
            ms_IAPMgr.OnPayError = OnPayError;
            ms_IAPMgr.OnPayCancel = OnPayCancel;
            ms_IAPMgr.Buy(goodsId, true);
            ms_apple_goodsId = goodsId;
            ms_apple_order_id = orderId;


            {
                SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.PayBegin);
                sdkParam.channel = "apple";
                GameSDK.DoCallback(null, sdkParam);
            }
            return true;
        }
		//------------------------------------------------------	
		static void OnPaySucessed(string goodsId)
		{
			Debug.Log("Apple Buy OnPaySucessed:" + goodsId);
            SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.PaySucces);
            sdkParam.channel = "apple";
            sdkParam.cpOrderId = ms_apple_order_id;
            GameSDK.DoCallback(null, sdkParam);
            ClearApplePayInfo();
        }
		//------------------------------------------------------	
		static void OnPayFailed(string str)
		{
			Debug.Log("Apple Buy OnPayFailed:" + str);
            SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.PayFail);
            sdkParam.channel = "apple";
            sdkParam.cpOrderId = ms_apple_order_id;
            GameSDK.DoCallback(null, sdkParam);
            ClearApplePayInfo();
        }
        //------------------------------------------------------	
        static void OnPayError(string str)
        {
            Debug.Log("Apple Buy OnPayFailed:" + str);
            SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.PayFail);
            sdkParam.channel = "apple";
            sdkParam.cpOrderId = ms_apple_order_id;
            GameSDK.DoCallback(null, sdkParam);
            ms_apple_goodsId = null;
        }
        //------------------------------------------------------	
        static void OnPayCancel(string str)
        {
            Debug.Log("Apple Buy OnPayFailed:" + str);
            SDKCallbackParam sdkParam = new SDKCallbackParam(ESDKActionType.PayCancel);
            sdkParam.channel = "apple";
            sdkParam.cpOrderId = ms_apple_order_id;
            GameSDK.DoCallback(null, sdkParam);
            ClearApplePayInfo();
        }
#endif
    }
}