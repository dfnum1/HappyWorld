/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	SDKCallback
作    者:	HappLI
描    述:	SDK 回调
*********************************************************************/

using System.Collections.Generic;
using Proto3;
using SDK;
using TopGame.Data;
using TopGame.Net;
using TopGame.SvrData;
using TopGame.UI;
using UnityEngine;

namespace TopGame
{
    public partial class GameInstance : SDK.ISDKCallback
    {
        private bool m_bSDKPause = false;
        [Framework.Plugin.AT.ATMethod]
        public static bool SDKLogin(string param)
        {
            if (!GameSDK.HasLoginSDK())
                return false;
            return GameSDK.Login(param);
        }
        //------------------------------------------------------
        public void OnStartUp(ISDKAgent agent)
        {
        }
        //------------------------------------------------------
        public void OnSDKPauseGame()
        {
            if (m_bSDKPause) return;
            m_bSDKPause = true;
            LogicLock(m_bSDKPause);
            TouchLock(m_bSDKPause);
            SkillLock(m_bSDKPause);
        }
        //------------------------------------------------------
        public void OnSDKResumeGame()
        {
            if (!m_bSDKPause) return;
            m_bSDKPause = false;
            LogicLock(m_bSDKPause);
            TouchLock(m_bSDKPause);
            SkillLock(m_bSDKPause);
        }
        //------------------------------------------------------
        public void OnSDKAction(ISDKAgent agent, ISDKParam param)
        {
            if(param!=null && param is SDKCallbackParam)
            {
                SDKCallbackParam sdkParam = (SDKCallbackParam)param;
                switch(sdkParam.action)
                {
                    case ESDKActionType.InitSucces:
                        {
                            UI.UILoginSDKLogic.AddSDKAssets(sdkParam.name);
							if("apple".CompareTo(sdkParam.channel) == 0)
							{
								var products = DataManager.getInstance().PayList.GetPayListByChannel("apple");
								if(products!=null && products.Count>0)
								{
									List<string> temps = new List<string>(products.Count);
									foreach(var db in products)
									{
										if(!string.IsNullOrEmpty(db.Value.goodsId)) temps.Add(db.Value.goodsId);
									}
									SDK.GameSDK.InitProdocts(temps);
								}
							}
                        }
                        break;
                    case ESDKActionType.LoginSucces:
                        if (string.IsNullOrEmpty(sdkParam.name)) sdkParam.name = sdkParam.uid;
                        OnLogin(sdkParam.uid, sdkParam.name, "", false, null, sdkParam.channel);
                        break;
                    case ESDKActionType.SwitchAccount:
                        //Net.NetLoginHandler.ReqLogout();
                        OnLogin(sdkParam.uid, sdkParam.name, "", false, null, sdkParam.channel);
                        break;
                    case ESDKActionType.LogoutSucces:
                        //Net.NetLoginHandler.ReqLogout();
                        break;
                    case ESDKActionType.Message:
                        UI.TipsUtil.ShowCommonTip(ETipType.Yes, sdkParam.msg);
                        break;
                    case ESDKActionType.PayBegin:
                        {
                            if ("apple".CompareTo(sdkParam.channel) == 0) UIManager.ShowUI(EUIType.CommonWaitPanel);
                        }
                        break;
                    case ESDKActionType.PaySucces:
                        {
                           // Net.NetPayHandler.PayAction(sdkParam.cpOrderId, sdkParam.channel, PaySDKAction.PayVerify);
                            if ("apple".CompareTo(sdkParam.channel) == 0) UIManager.HideUI(EUIType.CommonWaitPanel);
                        }
                        break;
                    case ESDKActionType.PayFail:
                        {
                          //  Net.NetPayHandler.PayAction(sdkParam.cpOrderId, sdkParam.channel, PaySDKAction.PayFail);
                            if ("apple".CompareTo(sdkParam.channel) == 0) UIManager.HideUI(EUIType.CommonWaitPanel);
                        }
                        break;
                    case ESDKActionType.PayCancel:
                        {
                          //  Net.NetPayHandler.PayAction(sdkParam.cpOrderId, sdkParam.channel, PaySDKAction.PayCancel);
                            if ("apple".CompareTo(sdkParam.channel) == 0) UIManager.HideUI(EUIType.CommonWaitPanel);
                        }
                        break;
                    case ESDKActionType.Exit:
                        break;
                    case ESDKActionType.FCMOut:
                        {
                         //   Net.NetLoginHandler.ReqLogout();
                        }
                        break;
                }
                return;
            }
            if (param is SDK.AdCallbackParam)
            {
                SDK.AdCallbackParam callback = (SDK.AdCallbackParam)param;
                TopGame.Core.AUserActionManager.AddActionKV("adAction", callback.action.ToString());
                TopGame.Core.AUserActionManager.AddActionKV("adType", callback.type.ToString());
                TopGame.Core.AUserActionManager.AddActionKV("posId", callback.posId);
                AddEventUtil.LogEvent("Ad", true);

                switch (callback.action)
                {
                    case AdCallbackParam.EAction.Clicked:
                        {
                        }
                        break;
                    case AdCallbackParam.EAction.Loaded:
                    case AdCallbackParam.EAction.Played:
                        OnSDKPauseGame();
                        break;
                    case AdCallbackParam.EAction.Closed:
                        OnSDKResumeGame();
                        break;
                }
            }
            if (agent!=null && agent is SDK.Advertising)
            {
                //判断正确后发送看广告消息
                Advertising.AdParam video = (Advertising.AdParam)param;
                Debug.Log(video.action + "->" + "adID:" + video.advID + ",id:" + video.externID + ",subID:" + video.subID + ",code:" + video.funcType + " pubid:" + video.pubID);
                switch (video.action)
                {
                    case Advertising.EAction.BeginVideo:
                    case Advertising.EAction.Loading:
                        {
                            OnSDKPauseGame();
                            //! TODO 添加转圈loding
                       //     TopGame.Base.Util.ShowWaittingMask(true, true);
                        }
                        break;
                    case Advertising.EAction.Failed:
                        {
                            OnSDKResumeGame();
                            //! 关闭转圈loading
                       //     TopGame.Base.Util.ShowWaittingMask(false);
                            //如果无限跑酷广告观看失败,直接结算
                        }
                        break;
                    case Advertising.EAction.EndVideo:
                        {
                            OnSDKResumeGame();
                            RequestAD(video.funcType, video.advID, video.externID, video.subID);

                            //! 关闭转圈loading
                     //       TopGame.Base.Util.ShowWaittingMask(false);
                        }
                        break;
                }
            }
//             else if(agent is SDK.UnityQuickSDK)
//             {
//                 SDK.QuickSDKParam quickSDK = (SDK.QuickSDKParam)param;
//                 switch(quickSDK.actionType)
//                 {
//                 }
//             }
        }

        //------------------------------------------------------
        public void RequestAD(int ADType, int ADID, int ExternId, int SubID)
        {
        }
    }
}
