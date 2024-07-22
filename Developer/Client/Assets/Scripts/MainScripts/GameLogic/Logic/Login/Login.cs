/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Login
作    者:	HappLI
描    述:	登录
*********************************************************************/

using System;
using TopGame.Core;
using TopGame.Net;
using TopGame.UI;
using UnityEngine;

namespace TopGame.Logic
{
    [State(EGameState.Login,1)]
    [StateClearFlag(EGameState.VersionCheck, EGameState.Login)]
    public class Login : AState, UI.IUICallback, ISessionCallback
    {
        static bool ms_bReqLoginFlag = false;
        static bool ms_bShowNotice = true;
        static bool ms_bOnChangeAccount = false;
        public static bool sbConnectedAuoLogin = false;
#if !UNITY_EDITOR
        private float m_fSDKDelayLoginShow = -1;
#endif
        //------------------------------------------------------
        public override void OnInit(EGameState state)
        {
            base.OnInit(state);
            SvrData.UserManager.getInstance().ClearUser();
        }
        //------------------------------------------------------
        public override void OnPreStart()
        {
            base.OnPreStart();

            Framework.Plugin.Guide.GuideSystem.getInstance().Clear();
            //if(Framework.Plugin.Guide.GuideSystem.getInstance()!=null) Framework.Plugin.Guide.GuideSystem.getInstance().Clear();
            Core.RenderHudManager.getInstance().Shutdown();
            CameraKit.SetCameraClearFlag(UnityEngine.CameraClearFlags.Skybox, Color.black);
#if !UNITY_EDITOR
           m_fSDKDelayLoginShow = -1;
#endif
            ApplayCamera(GetBindSceneID(), false);
        }
        //------------------------------------------------------
        public void ApplayCamera(ushort sceneCamera, bool bTween = true)
        {
            ICameraController cameraCtl = CameraController.getInstance();
            cameraCtl.Enable(true);
            FreeCameraMode cameraMode = cameraCtl.SwitchMode("free") as FreeCameraMode;

            if (cameraMode == null) return;
            cameraMode.Reset();
            cameraMode.SetCurrentEulerAngle(new Vector3(15.5f, 2.5f, 0), false, 0);
            cameraMode.SetCurrentTransOffset(new Vector3(-1.0f, 16.9f, -9.2f), false, 0);
            cameraCtl.ForceUpdate(0);
            Data.CsvData_Scene.SceneData scene = Data.DataManager.getInstance().Scene.GetData(sceneCamera);
            if (scene != null)
            {
                float fLerp = bTween ? scene.fLerp : 0;

                cameraMode.SetCurrentLookAtOffset(scene.LookAtOffset, false, fLerp);
                cameraMode.SetCurrentEulerAngle(scene.EulerAngles, false, fLerp);
                cameraMode.SetCurrentTransOffset(scene.PositionOffset, false, fLerp);
                cameraMode.SetCurrentFov(scene.FOV, false, fLerp);
                cameraMode.AppendFollowDistance(scene.FollowDistance, true, false, fLerp);
                if (fLerp <= 0)
                {
                    cameraMode.Start();
                    cameraMode.Blance();
                    CameraController.getInstance().ForceUpdate(0);
                }
            }
            cameraMode.SetLookFocusScatter(Vector3.zero, 0, 0);
            cameraMode.ReStartFocusScatter();
        }
        //------------------------------------------------------
        void PlayVideo()
        {
            UIVideo viedoPanel =  UIKits.CastGetUI<UIVideo>();
            if (viedoPanel != null)
            {
                if (viedoPanel.Play("Video/login.mp4", true))
                {
                    //    viedoPanel.SetDefaultTexture(Data.DefaultResources.DefaultLoading);
                    viedoPanel.SetOrder(-1);
                }
            }
        }
        //------------------------------------------------------
        public override void OnStart()
        {
            base.OnStart();
            if(GetFramework()!=null) GetFramework().ResetRuntime();
            CameraKit.SetCameraClearFlag(UnityEngine.CameraClearFlags.Skybox, Color.black);
            Net.NetWork.getInstance().RegisterSessionCallback(this);
            GameInstance.getInstance().uiManager.RegisterCallback(this);
#if USE_QUICKSDK && !UNITY_EDITOR
            m_fSDKDelayLoginShow = 1;
            UI.UIKits.HideUI((int)UI.EUIType.Login);
#else
            Framework.UI.UIHandle login = UI.UIKits.ShowUI((int)UI.EUIType.Login);
            if (login != null)
            {
                login.Show();
                if (ms_bOnChangeAccount)
                {
                    UILogin uILogin = login as UILogin;
                    if (uILogin != null)
                    {
                        uILogin.OnChangeAccount();
                    }
                }
            }
#endif
            if (GameInstance.getInstance() != null && 
                (GameInstance.getInstance().GetPreState() <= EGameState.Login || GameInstance.getInstance().GetPreState() == EGameState.Count))
            {
                var str = SvrData.ServerList.GetLoginNoticeArray();
                if (str != null)
                {
                    //公告
                    UIManager.ShowUI(EUIType.NoticeTip);
                } 
            }
        }
#if USE_QUICKSDK && !UNITY_EDITOR
        //------------------------------------------------------
        public override void OnUpdate(float fFrameTime)
        {
            base.OnUpdate(fFrameTime);
            if (m_fSDKDelayLoginShow > 0)
            {
                m_fSDKDelayLoginShow -= Time.deltaTime;
                if(m_fSDKDelayLoginShow<=0)
                {
                    m_fSDKDelayLoginShow = -1;
                    SDK.UnityQuickSDK.Login();
                }
            }
        }
#endif
        //------------------------------------------------------
        public override void OnExit()
        {
            base.OnExit();
#if USE_QUICKSDK && !UNITY_EDITOR
            m_fSDKDelayLoginShow = -1;
#endif
            if (GameInstance.getInstance()== null)
            {
                return;
            }
            GameInstance.getInstance().uiManager.UnRegisterCallback(this);
            UILogin login = UIKits.CastGetUI<UILogin>(false);
            if (login != null)
            {
                login.Close();
            }
            ms_bOnChangeAccount = false;
        }
        //------------------------------------------------------
        public void OnUIAwake(UIBase pBase)
        {
            if (!pBase.IsVisible()) return;
        }
        //------------------------------------------------------
        public void OnUIShow(UIBase pBase)
        {
            if (!pBase.IsInstanced()) return;
        }
        //------------------------------------------------------
        public void OnUIHide(UIBase pBase)
        {

        }
        //------------------------------------------------------
        public void OnSessionState(AServerSession serverSession, ESessionState eState)
        {
            if (eState == ESessionState.Connected)
            {
                Framework.Plugin.Logger.Info("服务器连接成功");
                if(sbConnectedAuoLogin) SetLoginFlag(true);
                sbConnectedAuoLogin = false;
            }
        }
        //------------------------------------------------------
        public static bool CanLoginReq()
        {
            return ms_bReqLoginFlag;
        }
        //------------------------------------------------------
        public static void SetLoginFlag(bool bReq = true)
        {
            ms_bReqLoginFlag = bReq;
        }
        //------------------------------------------------------
        public static void SetShowNotice(bool isShow)
        {
            ms_bShowNotice = isShow;
        }
        //------------------------------------------------------
        public static void OnChangeAccount(bool changeAccount)
        {
            ms_bOnChangeAccount = changeAccount;
        }
    }
}
