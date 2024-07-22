/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UILoading
作    者:	HappLI
描    述:	加载界面
*********************************************************************/

using Framework.Core;
using Framework.Plugin.AT;
using System;
using System.Collections;
using System.Collections.Generic;
using TopGame.Base;
using TopGame.Core;
using TopGame.Data;
using TopGame.Logic;
using TopGame.Net;
using TopGame.SvrData;
using UnityEngine;

namespace TopGame.UI
{
    [Framework.Plugin.AT.ATExportMono("UI系统/登录/界面")]
    [UI((ushort)EUIType.Login, UI.EUIAttr.UI)]
    public class UILogin : UIBase
    {
        bool m_bCheckNet = false;
        bool m_bRecheck = false;
        float m_fCheckTime = 0f;
        UILoginView m_view = null;
        //------------------------------------------------------
        protected override void DoShow()
        {
            base.DoShow();
            Logic.Login.SetLoginFlag(false);
            StartUpLoading.SetAutoHide(true);
            if (Data.LocalAccountCatch.HasAccountCache())
            {
                if(m_view!=null) m_view.SetPanel(true);
            }
            else
            {
                //无缓存
                if (!DebugConfig.AutoAccountRegister)
                {
                    if(m_view!=null) m_view.NotCachedAccount();
                }
            }
        }
        //------------------------------------------------------
        public override void Clear()
        {
            base.Clear();
            Logic.Login.SetLoginFlag(false);
            m_bCheckNet = false;
            m_fCheckTime = 0f;
        }
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            m_view = m_pView as UILoginView;
            SetCheckTime(0);
        }
        //------------------------------------------------------
        public void OnLoginClick(GameObject go, params VariablePoolAble[] param)
        {
            Login();
//             Variable1 loginParam = new Variable1() { intVal = (int)Proto3.LoginType.Account };
//             SvrData.ServerList.CheckNeedReqList(ReqServerListEndLogin, loginParam);
        }
        //------------------------------------------------------
        void ReqServerListEndLogin(string msg, VariablePoolAble userParam)
        {
            Login();
        }
        //------------------------------------------------------
        [ATMethod]
        public void Login()
        {
            if (SDK.GameSDK.HasLoginSDK())
                SDK.GameSDK.Login();
            else
            {
                if(m_view.IsOffline())
                    GameInstance.getInstance().OnLogin(m_view.GetUsrInput(), m_view.GetUsrInput(), m_view.GetPwdInput(), m_view.IsOffline());
                else 
                {
                    if(string.IsNullOrEmpty(m_view.GetPath()))
                    {
                        UI.TipsUtil.ShowCommonTip(ETipType.Yes, 80010235);
                    }
                    else
                    {
                        GameInstance.getInstance().IsOffline = false;
                        Net.NetWork.getInstance().Disconnect(ESessionType.Count);
                        Proto3.LoginVerifyRequest loginVer = new Proto3.LoginVerifyRequest();
                        loginVer.Account = m_view.GetUsrInput();
                        loginVer.Password = m_view.GetPwdInput();
                        loginVer.LoginType = Proto3.LoginType.Account;
                        loginVer.AutoCreate = true;
                        loginVer.DeviceGUID = GameInstance.getInstance().GetDeviceUDID();
                        loginVer.OsInfo = UnityEngine.SystemInfo.operatingSystem;
                        loginVer.DeviceType = UnityEngine.SystemInfo.deviceModel;
                        loginVer.DeviceModel = UnityEngine.SystemInfo.deviceName;
                        if (!string.IsNullOrEmpty(loginVer.Account)) loginVer.NickName = loginVer.Account;
                        Net.WebHandler.PosHttp(m_view.GetPath(), MessagePool.ToJson(Proto3.MID.LoginVerifyReq, loginVer));
                    }
                }
            }
        }
        //         //------------------------------------------------------
        //         public void Login(Proto3.LoginType loginType)
        //         {
        //             Debug.LogWarning("Login Request");
        //             Variable1 loginParam = new Variable1() { intVal = (int)loginType };
        //             SvrData.ServerList.CheckNeedReqList(ReqServerListEndLogin, loginParam);
        //         }
        ////------------------------------------------------------
        //public void DoLogin(bool bOffline, Proto3.LoginType loginType)
        //{
        //    UILoginView loginView = m_pView as UILoginView;
        //    if( string.IsNullOrEmpty(loginView.GetPath()) )
        //    {
        //        loginView.SetListData();
        //    }
        //    GameInstance.getInstance().IsOffline = loginView.IsOffline();
        //    Logic.Login.SetLoginFlag(false);

        //    Logic.Login.sbConnectedAuoLogin = false;
        //    if (loginView.IsOffline())
        //    {
        //        SvrData.LocalServerData.Test();//添加离线3个默认英雄
        //        BattleDB battleDb = TopGame.SvrData.UserManager.getInstance().mySelf.ProxyDB<BattleDB>(Data.EDBType.Battle);
        //        foreach (var item in DataManager.getInstance().Chapter.datas)
        //        {
        //            battleDb.SetCurrentLevel(Proto3.LevelTypeCode.Default, item.Value.id);
        //            break;
        //        }
        //        GameInstance.getInstance().ChangeState(Logic.EGameState.Battle, Logic.EMode.Runer, ELoadingType.Loading,true);
        //    }
        //    else
        //    {
        //        if (Application.internetReachability == NetworkReachability.NotReachable)
        //        {
        //            Base.Util.ShowCommonTip(UI.ETipType.Yes, 80010201);
        //            return;
        //        }
        //        if(Net.NetWork.getInstance().GetSessionState() != ESessionState.Connected)
        //        {
        //            Logic.Login.sbConnectedAuoLogin = true;
        //        }
        //        string user = loginView.GetUsrInput();
        //        string password = loginView.GetPwdInput();
        //        string name = loginView.GetLoginServerName();
        //        if (loginView.GetPath() == null)
        //        {
        //            System.Action confirm = () =>
        //            {
        //                Application.Quit();
        //            };
        //            Base.Util.ShowCommonTip(UI.ETipType.Yes, 10100009, confirm, null, null);//获取不到服务器地址的情况下,提示服务器链接失败
        //        }
        //        if (string.IsNullOrEmpty(user) && loginType == Proto3.LoginType.FastLogin)
        //        {
        //            if(Net.NetWork.getInstance().GetSessionState() == ESessionState.Connected)
        //                Logic.Login.SetLoginFlag(true);
        //            else
        //                GameInstance.getInstance().OnGateIpTest(loginView.GetPath());
        //        }
        //        else
        //        {
        //            if (CheckUserAccount(user) && CheckUserPassword(password))//判断输入账号和密码长度是否小于最小值是否符合规则
        //            {
        //                if (Net.NetWork.getInstance().GetSessionState() == ESessionState.Connected)
        //                    Logic.Login.SetLoginFlag(true);
        //                else
        //                    GameInstance.getInstance().OnGateIpTest(loginView.GetPath());
        //                //Net.NetWork.getInstance().ReqHttp(loginView.GetPath(), GameInstance.getInstance().OnGateIpTest);
        //            }
        //        }

        //    }
        //}
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            if(Logic.Login.CanLoginReq())
            {
                Req_Login(false);
            }
            m_fCheckTime -= fFrame;

            if (m_fCheckTime <= 0 && m_bCheckNet)
            {
                m_bCheckNet = false;
                CheckNet();
            }
        }
        //------------------------------------------------------
        void SetCheckTime(float time)
        {
            m_fCheckTime = time;
            m_bCheckNet = true;
        }
        //------------------------------------------------------
        void CheckNet()
        {
            //switch (Application.internetReachability)
            //{
            //    case NetworkReachability.NotReachable:
            //        Framework.Plugin.Logger.Warning("没网络");
            //        //无网络时,显示重连提示
            //        Action confirm = () =>
            //        {
            //            //再次检查网络
            //            var cfg = Data.CsvData_SystemConfig.sysConfig;
            //            if (cfg != null)
            //            {
            //                m_bRecheck = true;
            //                SetCheckTime(cfg.netConnectionTime1);//有个检查延时 systemConfig netConnectionTime1 
            //            }
            //        };

            //        Action cancel = () =>
            //        {
            //            Application.Quit();
            //        };

            //        Base.Util.ShowCommonTip(ETipType.Yes_No, 80010201, confirm, cancel,null, 80010203, 80010014);
            //        break;
            //    case NetworkReachability.ReachableViaCarrierDataNetwork:
            //        Framework.Plugin.Logger.Warning("使用数据");
            //        //重新获取服务器列表
            //        if (m_bRecheck)
            //        {
            //            SvrData.ServerList.ReqList();
            //            m_bRecheck = false;
            //        }

            //        break;
            //    case NetworkReachability.ReachableViaLocalAreaNetwork:
            //        Framework.Plugin.Logger.Warning("使用wifi或电缆");
            //        //重新获取服务器列表
            //        if (m_bRecheck)
            //        {
            //            SvrData.ServerList.ReqList();
            //            m_bRecheck = false;
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("修复")]
        public void RepairGame()
        {
            //Action confirm = () =>
            //{
            //    //:删除热更下载的资源
            //    if (System.IO.Directory.Exists(FileSystemUtil.UpdateDataPath))
            //    {
            //        System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(FileSystemUtil.UpdateDataPath);
            //        var directories = directoryInfo.GetDirectories();
            //        var files = directoryInfo.GetFiles();

            //        foreach (var directory in directories)
            //        {
            //            //Debug.Log("directory.FullName: " + directory.FullName);
            //            System.IO.Directory.Delete(directory.FullName,true);
            //        }

            //        foreach (var file in files)
            //        {
            //            //Debug.Log("file: " + file.FullName);
            //            System.IO.File.Delete(file.FullName);
            //        }
            //    }

            //    Util.ShowCommonTip(ETipType.AutoHide, 80020333);
            //};
            //Base.Util.ShowCommonTip(ETipType.Yes_No, 10014005,80010041, confirm, null,null);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("客服")]
        public void CustomerService(string enter)
        {
//             if(!Base.Util.ShowAIHelp(enter))
//                 Base.Util.ShowCommonTip(ETipType.AutoHide, 10100002);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("注销")]
        public void LogOut()
        {
            if (SDK.GameSDK.HasLoginSDK())
            {
                SDK.GameSDK.Logout();
                return;
            }
            if (m_view != null)
            {
                m_view.NotCachedAccount();
                m_view.bHasCacheAccount = false;
            }
        }
        //------------------------------------------------------
        bool CheckUserAccount(string account)
        {
            //var cfg = CsvData_SystemConfig.sysConfig;
            //if (cfg == null)
            //{
            //    return true;
            //}

            //int minLength = (int)cfg.accountCharacterNum[0];
            //int length = Util.GetStrLength(account);

            //if (length < minLength)
            //{
            //    Util.ShowCommonTip(ETipType.AutoHide, 80020356);
            //    return false;
            //}

            ////检测是否有非法字符,只能是数字和字母组成
            //if (!Util.IsOnlyNumAndLetters(account))
            //{
            //    Util.ShowCommonTip(ETipType.AutoHide, 80020358);
            //    return false;
            //}

            return true;
        }
        //------------------------------------------------------
        bool CheckUserPassword(string password)
        {
            //var cfg = CsvData_SystemConfig.sysConfig;
            //if (cfg == null)
            //{
            //    return true;
            //}

            //int minLength = (int)cfg.passwordCharacterNum[0];
            //int length = Util.GetStrLength(password);

            //if (length < minLength)
            //{
            //    Util.ShowCommonTip(ETipType.AutoHide, 80020355);
            //    return false;
            //}

            ////检测是否有非法字符,密码只能是数字和字母组成
            //if (!Util.IsOnlyNumAndLetters(password))
            //{
            //    Util.ShowCommonTip(ETipType.AutoHide, 80020357);
            //    return false;
            //}

            return true;
        }
        //------------------------------------------------------
        public void Req_Login(bool isMandatory, bool isReconect = false)
        {
            if (GameInstance.getInstance() != null)
            {
                UILoginView loginView = m_pView as UILoginView;
                string user = loginView.GetUsrInput();
                string password = loginView.GetPwdInput();
                GameInstance.getInstance().Req_Login(isMandatory, isReconect, user, password);
            }
        }
        //------------------------------------------------------
        public void OnChangeAccount()
        {
            if (m_view != null)
            {
                m_view.NotCachedAccount();
            }
        }
        //------------------------------------------------------
        public string GetUsrInput()
        {
            return m_view.GetUsrInput();
        }
    }
}
