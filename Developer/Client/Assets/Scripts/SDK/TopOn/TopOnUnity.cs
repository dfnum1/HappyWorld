using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct TopOnCfg : ISDKConfig
    {
        public string appId;
        public string appKey;
        public GameObject pMainGO;
    }
    public class TopOnUnity : ISDKAgent
    {
#if USE_TOPON
        TopOnCfg m_Config;
        static bool ms_bInited = false;
#endif
        //------------------------------------------------------
        public static AdmobileUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_TOPON
            ms_bInited = false;
            AdmobileUnity talkingData = new AdmobileUnity();
            if (talkingData.Init(config))
            {
                talkingData.SetCallback(callback);
                ms_bInited = true;
                return talkingData;
            }
            return null;
#else
            return null;
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_TOPON
            m_Config = (TopOnCfg)cfg;
            Debug.Log("TopOn begin init");
            if (m_Config.pMainGO == null)
            {
                Debug.LogWarning("unity go is null");
                return false;
            }
//（可选配置）设置自定义的Map信息，可匹配后台配置的广告商顺序的列表（App纬度）
//注意：调用此方法会清除setChannel()、setSubChannel()方法设置的信息，如果有设置这些信息，请在调用此方法后重新设置
ATSDKAPI.initCustomMap(new Dictionary<string, string> { { "unity3d_data", "test_data" } }); 

//（可选配置）设置自定义的Map信息，可匹配后台配置的广告商顺序的列表（Placement纬度）
ATSDKAPI.setCustomDataForPlacementID(new Dictionary<string, string> { { "unity3d_data_pl", "test_data_pl" } } ,placementId);

//（可选配置）设置渠道的信息，开发者可以通过该渠道信息在后台来区分看各个渠道的广告数据
//注意：如果有使用initCustomMap()方法，必须在initCustomMap()方法之后调用此方法
ATSDKAPI.setChannel("unity3d_test_channel"); 

//（可选配置）设置子渠道的信息，开发者可以通过该渠道信息在后台来区分看各个渠道的子渠道广告数据
//注意：如果有使用initCustomMap()方法，必须在initCustomMap()方法之后调用此方法
ATSDKAPI.setSubChannel("unity3d_test_subchannel"); 

//设置开启Debug日志（强烈建议测试阶段开启，方便排查问题）
ATSDKAPI.setLogDebug(true);

//（必须配置）SDK的初始化
ATSDKAPI.initSDK(m_Config.appId, m_Config.appKey);//Use your own app_id & app_key here
            return true;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        public static void ShowAd(string type, string posId)
        {
#if USE_TOPON
            if (!ms_bInited) return;
            if(type.CompareTo("rewardAd") == 0)
            {
                ATRewardedVideo.Instance.client.onAdLoadEvent += onAdLoad; 
                ATRewardedVideo.Instance.client.onAdLoadFailureEvent += onAdLoadFail;
                ATRewardedVideo.Instance.client.onAdVideoStartEvent  += onAdVideoStartEvent;
                ATRewardedVideo.Instance.client.onAdVideoEndEvent  += onAdVideoEndEvent;
                ATRewardedVideo.Instance.client.onAdVideoFailureEvent += onAdVideoPlayFail;
                ATRewardedVideo.Instance.client.onAdClickEvent += onAdClick;
                ATRewardedVideo.Instance.client.onRewardEvent += onReward;
                ATRewardedVideo.Instance.client.onAdVideoCloseEvent += onAdVideoClosedEvent;

                Dictionary<string,string> jsonmap = new Dictionary<string,string>();
                //如果需要通过开发者的服务器进行奖励的下发（部分广告平台支持此服务器激励），则需要传递下面两个key
                //ATConst.USERID_KEY必传，用于标识每个用户;ATConst.USER_EXTRA_DATA为可选参数，传入后将透传到开发者的服务器
                jsonmap.Add(ATConst.USERID_KEY, "test_user_id");
                jsonmap.Add(ATConst.USER_EXTRA_DATA, "test_user_extra_data");

                ATRewardedVideo.Instance.loadVideoAd(mPlacementId_rewardvideo_all,jsonmap);
            }
#endif
        }
#if USE_TOPON
        class SDKCallback : MonoBehaviour
        {
            void OnAdReceive(string msg)
            {

            }
            void OnAdExpose(string msg)
            {

            }
            void OnAdClick(string msg)
            {

            }
            void OnAdClose(string msg)
            {

            }
            void OnAdFailed(string msg)
            {

            }
            void OnAdComplete(string msg)
            {

            }
            void OnAdReward(string msg)
            {

            }
        }
#endif
    }
}