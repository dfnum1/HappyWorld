/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GameInstance
作    者:	HappLI
描    述:	游戏主线程
*********************************************************************/

using System.Collections.Generic;
using TopGame.Base;
using TopGame.Core;
using TopGame.Data;
using TopGame.Logic;
using Framework.Module;
using UnityEngine;
using TopGame.Net;
using System;
using Framework.Plugin.AT;
using Framework.Plugin.AI;
using Framework.Plugin.Guide;
using TopGame.UI;
using TopGame.SvrData;
using Framework.Core;
using Framework.Base;
using ExternEngine;

namespace TopGame
{
    [Framework.Plugin.AT.ATExportMono("游戏主体", Framework.Plugin.AT.EGlobalType.Single)]
    public partial class GameInstance : Core.GameModule
    {
        SvrData.UserManager m_pUserManager = null;
        public SvrData.UserManager userManager
        {
            get { return m_pUserManager; }
        }

        TouchInput m_MulTouchs = null;
        public TouchInput touchInput
        {
            get { return m_MulTouchs; }
        }

        bool m_bBeingVirtualLoading = false;
        float m_fVirtualLoading = 0;
        int m_nPoolLoadingCount = 0;
        Hot.HotScriptsModule m_HotScriptModule = null;
        public Hot.HotScriptsModule hotScriptModule
        {
            get { return m_HotScriptModule; }
        }
        protected UI.UIManager m_UIMgr = null;
        public UI.UIManager uiManager
        {
            get { return m_UIMgr; }
        }
        StateFactory m_States = null;
        public StateFactory statesFactory
        {
            get { return m_States; }
        }

        UI.DynamicTextManager m_pDynamicTexter = null;
        public UI.DynamicTextManager dynamicTexter
        {
            get
            {
                if (m_pDynamicTexter == null)
                    m_pDynamicTexter = new UI.DynamicTextManager();

                return m_pDynamicTexter;
            }
        }

        UI.DynamicHudHpManager m_pDynamicHudHper = null;
        public UI.DynamicHudHpManager dynamicHudHper
        {
            get
            {
                if (m_pDynamicHudHper == null)
                    m_pDynamicHudHper = new UI.DynamicHudHpManager();

                return m_pDynamicHudHper;
            }
        }

        UI.ObjectBubbleTalkManager m_pObjBubbleTalkMgr = null;
        public UI.ObjectBubbleTalkManager objectBubbleTalkMgr
        {
            get
            {
                if (m_pObjBubbleTalkMgr == null)
                    m_pObjBubbleTalkMgr = new UI.ObjectBubbleTalkManager();

                return m_pObjBubbleTalkMgr;
            }
        }

        public override UI.IItemTweenManager itemTweenMgr
        {
            get
            {
                if (m_pItemTweenMgr == null)
                    m_pItemTweenMgr = new UI.ItemTweenManager();

                return m_pItemTweenMgr;
            }
        }

        /// <summary>
        /// 是否离线状态
        /// </summary>
        public bool IsOffline = true;

        private AnimationCurve m_TimeScaleCurve = null;
        private float m_fTimeScaleCurveDelta = 0;
        private float m_fTimeScaleCurveDuration = 0;

        protected static GameInstance sm_pInstance;

        protected Framework.Plugin.AT.AgentTree m_pAgentTree = null;
        public AgentTree mainAT
        {
            get { return m_pAgentTree; }
        }

        bool m_bFirstLoginShowRewardFlag = false;
        public bool FisrtLoginShowRewardFlag
        {
            get
            {
                return m_bFirstLoginShowRewardFlag;
            }
            set
            {
                m_bFirstLoginShowRewardFlag = value;
            }
        }
        public System.Action<bool> GamePauseCB;
        //------------------------------------------------------
        public static GameInstance getInstance()
        {
            return sm_pInstance;
        }
        //------------------------------------------------------
        public GameInstance()
        {
            sm_pInstance = this;
            ModuleManager.mainModule = this;
            SetRandomSeed((uint)DateTime.Now.Ticks);
            SDK.GameSDK.SetCallback(this);
        }
        //------------------------------------------------------
        protected override bool OnAwake(Framework.IFrameworkCore plusSetting)
        {
            sm_pInstance = this;
            FrameworkMain pMain = plusSetting as FrameworkMain;
            ModuleManager.startUpGame = true;
            SDK.GameSDK.getInstance().Awake(pMain.SDKConfig, pMain);

            //pMain.OnGUIAction += TestController.OnGUI;

            m_pUserManager = RegisterModule<UserManager>();
            m_pFileSystem = RegisterModule<FileSystem>(Logic.FrameworkStartUp.GetFileSystem());
            m_pDataMgr = RegisterModule<Data.DataManager>();

            //! start up dll
            GameQuality.Init();

            //! 相机控制
            {
#if USE_PICO && USE_VR
                if (pMain.VRCameraPrefab == null)
                {
                    Framework.Plugin.Logger.Error("camera prefab miss...");
                    Framework.Plugin.Logger.Break("camera prefab miss...");
                    return false;
                }
                GameObject pCameraCtrl = GameObject.Instantiate<GameObject>(pMain.VRCameraPrefab);
#else
                if (pMain.CameraPrefab == null)
                {
                    Framework.Plugin.Logger.Break("camera prefab miss...");
                    return false;
                }
                GameObject pCameraCtrl = GameObject.Instantiate<GameObject>(pMain.CameraPrefab);
#endif
                CameraController cameraControll = new CameraController();
                if (!cameraControll.Init(pCameraCtrl))
                {
                    Framework.Plugin.Logger.Error("camera prefab miss...");
                    Framework.Plugin.Logger.Break("camera prefab miss...");
                    return false;
                }
                m_pCameraController = cameraControll;
                m_pCameraController.Enable(false);
                m_pCameraController.CloseCamera(true);
                m_pCameraController.ActiveRoot(false);
            }
            //! 声音系统
            {
                if (pMain.SoundSystem == null)
                {
                    Framework.Plugin.Logger.Break("sound prefab miss...");
                }
                else
                {
                    GameObject pSound = GameObject.Instantiate<GameObject>(pMain.SoundSystem);
                    m_pAudioManager = pSound.GetComponent<AudioManager>();
                    if (m_pAudioManager == null)
                    {
                        Framework.Plugin.Logger.Break("sound prefab is error...");
                        return false;
                    }
                }
            }
            //! 光照系统
            {
                if (pMain.CustomLightSystem == null)
                {
                    Framework.Plugin.Logger.Break("custom light prefab miss...");
                }
                else
                {
                    GameObject pCustomLight = GameObject.Instantiate<GameObject>(pMain.CustomLightSystem);
                }
            }
            //! 游戏状态注册，确保在update 列表第一位
            m_States =RegisterModule<StateFactory>();

            m_UIMgr = RegisterModule<UI.UIManager>();
            m_UIMgr.SetEventSystem(pMain.eventSystem);
            m_UIFramework = m_UIMgr;
            m_pRedDotMgr = RegisterModule<RedDotManager>();
            m_pEventTrigger = RegisterModule<GameEventSystemTrigger>();
            m_pGlobalBuff = RegisterModule<GameGlobalBuffer>();

            //! 逻辑模块注册
            m_pLocalizationMgr = RegisterModule<LocalizationManager>();
            m_pUserActionMgr = RegisterModule<UserActionManager>();
      //      m_pUnlockMgr = RegisterModule<UnLockManager>();
            Net.NetWork pNet = RegisterModule<Net.NetWork>();
            RegisterModule<Logic.DialogSystem>();
            m_pGlobalSkillInfomation = RegisterModule<Logic.GlobalBattleSkillInfomation>();

            Application.lowMemory += LowMemoryCallback;
            Physics.autoSimulation = false;
            //Physics.autoSyncTransforms = false;

            base.OnAwake(plusSetting);

            if(m_SceneManager != null)
                m_SceneManager.SetThemer(new SceneTheme());

            //! 注册回调
            aiSystem.Register(this);
            Framework.Plugin.Guide.GuideSystem.getInstance().Register(this);

            if (pMain.videoPanel)
                m_UIMgr.AddUI((ushort)EUIType.VideoPanel, pMain.videoPanel, true, pMain.videoPanel.defalutOrder);

            PositionTween.OnGlobalTweenEffectCompleted = this.OnTweenEffectCompleted;

            Reporter.OnGameController = OnGameGMController;
            SRDebug.OnGameController = OnGameGMController;

            return true;
        }
        //------------------------------------------------------
        protected override bool OnInitResource(Framework.IFrameworkCore plusSetting)
        {
            GameQuality.Init();
            bool bSucceed = base.OnInitResource(plusSetting);
            if (!bSucceed) return false;

            GetProjectileManager().SetDatas(Data.DataManager.getInstance().Projectile.datas);

            FrameworkMain pMain = plusSetting as FrameworkMain;
            //! 相机控制
            {
                m_pCameraController.RegisterCameraMode("hall", new HallCameraMode());
                m_pCameraController.RegisterCameraMode("free", new FreeCameraMode());
                m_pCameraController.RegisterCameraMode("battle", new BattleCameraMode());
            }

            AgentTreeManager.getInstance().Register(this);

            m_UIMgr.Init(pMain.UISystem, plusSetting.IsEditor());
            if (pMain.preTipPanel) m_UIMgr.PreferParent(pMain.preTipPanel.transform);
            if (pMain.versionUpdateUI) m_UIMgr.PreferParent(pMain.versionUpdateUI.transform);
            if (pMain.webViewPanel) m_UIMgr.PreferParent(pMain.webViewPanel.transform);

            //! 注册模块
            m_MulTouchs = RegisterModule<Core.TouchInput>();
            //m_HotScriptModule = ModuleManager.getInstance().RegisterModule<Hot.HotScriptsModule>();

            m_MulTouchs.OnMouseDown += Framework.Plugin.Guide.GuideWrapper.OnTouchBegin;
            m_MulTouchs.OnMouseMove += Framework.Plugin.Guide.GuideWrapper.OnTouchMove;
            m_MulTouchs.OnMouseUp += Framework.Plugin.Guide.GuideWrapper.OnTouchEnd;

            return true;
        }
        //------------------------------------------------------
        protected override void OnStart(Framework.IFrameworkCore plusSetting)
        {
            base.OnStart(plusSetting);

            FrameworkMain pMain = plusSetting as FrameworkMain;
            SDK.GameSDK.getInstance().Start(pMain.SDKConfig, pMain);

            if (m_pCameraController != null) m_pCameraController.StartUp();
            if (m_UIMgr != null) m_UIMgr.StartUp();
            if (m_pAudioManager != null)
            {
                m_pAudioManager.StartUp();
                AudioManager audioMgr = m_pAudioManager as AudioManager;
                if(audioMgr!=null)
                {
                    var data = SystemSettingManager.getInstance().GetSystemSettingData();
                    if(data!=null) audioMgr.SyncConfig(data.BGM, data.SoundEffect, data.BGVolumn, data.SoundEffectVolumn);
                }
            }

            m_UIMgr.CreateUI((ushort)UI.EUIType.GuidePanel);

            if(m_pAgentTree != null)
                AgentTreeManager.getInstance().UnloadAT(m_pAgentTree);
            m_pAgentTree = AgentTreeManager.getInstance().LoadAT(ATModuleSetting.MainAT);
            if(m_pAgentTree != null)
            {
                m_pAgentTree.AddOwnerClass(this);
                m_pAgentTree.Enable(true);
                m_pAgentTree.Enter();
            }
            //! 图形脚本注册回调
            AgentTreeManager.getInstance().RegisterClass(this);

            aiSystem.Init(AIDatas.AllDatas, AIDatas.GlobalVariables);
            aiSystem.EnableLog(DebugConfig.bAIDebug);

            GuideDatas.InitData(true);
            Framework.Plugin.Guide.GuideSystem.getInstance().datas = GuideDatas.AllDatas;
            Framework.Plugin.Guide.GuideSystem.getInstance().Enable(DebugConfig.bEnableGuide);
            Framework.Plugin.Guide.GuideSystem.getInstance().EnableSkip(DebugConfig.bEnableGuideSkip);
            Framework.Plugin.Guide.GuideSystem.getInstance().bGuideLogEnable = DebugConfig.bGuideLogEnable;
            Framework.Plugin.Guide.GuideGuidUtl.bGuideGuidLog = DebugConfig.bGuideGuidLog;


            Framework.Plugin.Logger.getInstance().SetFlag(DebugConfig.logLevel);

            if(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2)
            {
                EnableSupportInstancing(false);
            }
            else
                Framework.Base.ConfigUtil.EnableSupportInstancing(SystemInfo.supportsInstancing);
            if(Framework.Base.ConfigUtil.IsSupportInstancing && m_pAgentTree!=null)
            {
                AgentTreeManager.getInstance().ExecuteEvent((ushort)Base.EATEventType.UserDef, "InstancingCheck", new Framework.Core.VariableString() { strValue = SystemInfo.graphicsDeviceName });
            }
            if(!Framework.Base.ConfigUtil.IsSupportInstancing)
                EnableSupportInstancing(false);

            if (m_pDropManager!=null)
                m_pDropManager.SetDropSpeed(GlobalSetting.DropSpeed);

            //! 进入登录
            if(!plusSetting.IsEditor())
                ChangeState(EGameState.Login, EMode.None, ELoadingType.Nonthing);
        }
        //------------------------------------------------------
        public bool IsLoading()
        {
            if (m_UIMgr == null) return false;
            UI.UIBase loading = m_UIMgr.GetUI((ushort)UI.EUIType.Loading, false);
            if (loading == null) return false;
            return loading.IsVisible();
        }
        //------------------------------------------------------
        public float GetProgress()
        {
            if (m_SceneMgr == null) return 0;
            if (m_nPoolLoadingCount == 0)
            {
                if(m_bBeingVirtualLoading)
                    return m_SceneMgr.Progress * 0.3f + m_fVirtualLoading * 0.7f;
                else
                    return m_SceneMgr.Progress * 0.3f + m_fVirtualLoading * 0.5f;
            }
            else
                return m_SceneMgr.Progress * 0.3f + (1 - (float)m_pFileSystem.GetCurLoadingCount() / (float)m_nPoolLoadingCount) * 0.2f + m_fVirtualLoading * 0.5f;
        }
        //------------------------------------------------------
        void OnGameGMController()
        {
            UI.UIManager.ShowUI(EUIType.GMPanel);
        }
        //------------------------------------------------------
        protected override void OnPause(bool bPause)
        {
            GamePauseCB?.Invoke(bPause);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public void PreloadInstances(List<string> vPreLoad)
        {
            if (vPreLoad.Count <= 0) return;
            Dictionary<string, int> vStats =shareParams.stringCatchStatsMap;
            vStats.Clear();
            for (int i = 0; i < vPreLoad.Count; ++i)
            {
                int stat = 0;
                if (!vStats.TryGetValue(vPreLoad[i], out stat))
                {
                    vStats.Add(vPreLoad[i], 1);
                }
                else if (stat < 3)
                {
                    vStats[vPreLoad[i]] = stat + 1;
                }
            }
            foreach (var db in vStats)
            {
                int cnt = db.Value - FileSystemUtil.StatsInstanceCount(db.Key);
                if (cnt < 0) continue;
                PreloadInstance(db.Key, cnt);
            }
            vStats.Clear();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("", new Type[] { typeof(GameObject) })]
        public void PreloadInstance(string instance, int cnt)
        {
            for(int i = 0; i < cnt; ++i)
                FileSystemUtil.PreSpawnInstance(instance, true, false);
        }
        //------------------------------------------------------
        public void PreloadAssets(HashSet<string> vPreLoad, bool bAsync = true)
        {
            if (vPreLoad.Count <= 0) return;
            foreach (var db in vPreLoad)
            {
                PreloadAsset(db, bAsync);
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("")]
        public void PreloadAsset(string strPreLoad, bool bAsync = true)
        {
            if (string.IsNullOrEmpty(strPreLoad)) return;
            if (!FileSystemUtil.IsInLoadQueue(strPreLoad))
            {
                if(bAsync || IsLoading())
                    FileSystemUtil.AsyncReadFile(strPreLoad);
                else
                    FileSystemUtil.ReadFile(strPreLoad);
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("手机震动")]
        public void Vibrate(Framework.Plugin.NiceVibrations.HapticTypes type)
        {
            var data = SystemSettingManager.getInstance().GetSystemSettingData();
            if (data != null && data.bVibrateDevice <= 0) return;
            Framework.Plugin.NiceVibrations.MMVibrationManager.Haptic(type);
#if UNITY_EDITOR
            cameraController.Shake(0.2f, new Vector3(0.1f, 0.25f, 0f), new Vector3(60, 50, 1));
#endif
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("实例化渲染开关")]
        public void EnableSupportInstancing(bool bEnable)
        {
            Framework.Base.ConfigUtil.EnableSupportInstancing(bEnable);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取当前状态", null,  typeof(Logic.EGameState))]
        public EGameState GetState()
        {
            if (m_States == null) return EGameState.Count;
            return m_States.curState;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取之前状态", null, typeof(Logic.EGameState))]
        public EGameState GetPreState()
        {
            if (m_States == null) return EGameState.Count;
            return m_States.preState;
        }
        //------------------------------------------------------
        public EGameState GetStating()
        {
            if (m_States == null) return EGameState.Count;
            return m_States.stating;
        }
        //------------------------------------------------------
        public override void ResetRuntime( uint nResetFlags = 0xffffffff)
        {
            base.ResetRuntime(nResetFlags);

#if UNITY_EDITOR
            if (cameraController != null) cameraController.SetEditor(false);
#endif

            ClearTimeScaleCurve();
            ResetTimeScale();

            Framework.Plugin.Guide.GuideWrapper.OnOptionStepCheck();
            if (Framework.Plugin.Guide.GuideSystem.getInstance().IsCanChangeStateBreak())
                Framework.Plugin.Guide.GuideSystem.getInstance().OverGuide(false);

            Base.GlobalShaderController.SetBlend(Vector3.zero, Vector3.zero);
        //    Framework.Module.ModuleManager.getInstance().PauseJobs();

            m_vCurrentRunSpeed = Vector3.zero;
            m_PlayerPosition = Vector3.zero;
            m_PlayerDirection = Vector3.forward;
            m_nPoolLoadingCount = 0;
            m_bBeingVirtualLoading = false;
            m_fVirtualLoading = 0;

            if (m_pDynamicTexter != null) m_pDynamicTexter.Destroy();
            if (m_pDynamicHudHper != null) m_pDynamicHudHper.Destroy();
            if (m_pObjBubbleTalkMgr != null) m_pObjBubbleTalkMgr.Destroy();
            if(m_pProjectileManager!=null) m_pProjectileManager.StopAllProjectiles();
            AudioManager.FadeOutAll(1,1);

            Framework.Core.CommonUtility.ClearCatchs();
            if(dropManager!=null) dropManager.Reset();

            if (cameraController != null)
            {
                if (cameraController.GetCurrentMode() != null)
                    cameraController.GetCurrentMode().End();

                cameraController.Enable(false);
            }
            SetCurrentRode(0xff);
            SetPlayerPosition(Vector3.zero);

            Base.GlobalShaderController.EnableCurve(false);
            Base.GlobalShaderController.SetBlend(Vector3.zero, Vector3.zero);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("切换游戏坐标状态")]
        public void ChangeLocation(SvrData.ELocationState location, ELoadingType loadingType)
        {
            if(IsOffline) SvrData.LocalServerData.SaveUserData();

            AState pState = m_States.GetCurrentState();
            EMode currentMode = EMode.None;
            ELocationState preLocationState = m_pUserManager.mySelf.GetLocationState();
            m_pUserManager.mySelf.SetLocationState(location);
            if (pState != null && pState.GetActivedMode() != null) currentMode = pState.GetActivedMode().GetMode();
            if (location <=  ELocationState.InCity )
            {
                if(location == ELocationState.InCity) currentMode = EMode.City;
                ChangeState(EGameState.Hall, currentMode, loadingType);
            }
            else
            {
                bool bLoadingNothing = false;
                var preMode = m_States.curMode;
                EGameState preState = GetState();
                currentMode = (EMode)location;
                bool bForce = preState != EGameState.Battle || preMode != currentMode;// || battleDb.IsDirtyBigChapter();
                if (currentMode != EMode.None)
                    loadingType = StateFactory.GetModeLoadingType(currentMode);
                if (preState <= EGameState.Login) loadingType = ELoadingType.Loading;
                if (!bForce)
                {
                    if (bLoadingNothing) loadingType = ELoadingType.Nonthing;
                    else loadingType = ELoadingType.BlackFull;
                }
                ChangeState(EGameState.Battle, currentMode, loadingType, true);
            }
            
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("切换游戏状态", new System.Type[] { typeof(Logic.EGameState), typeof(Logic.EMode) })]
        public IMode ChangeState(Logic.EGameState eState, EMode mode = EMode.None,ELoadingType loadingType = ELoadingType.Loading, bool bForce = false)
        {
            if(m_States.curState > EGameState.Login && m_States.preState <= EGameState.Login)
                Framework.Module.ModuleManager.StartWatchPlayTime();
            return m_States.SwitchState(eState, loadingType, mode, bForce);
        }
        //------------------------------------------------------
        public void OnStateNoSceneChangeEnd()
        {
            SceneParam sceParam = SceneParam.DEF;
            sceParam.sceneID = 0;
            sceParam.isCompled = true;
            OnSceneCallback(sceParam);
            if (GetStating() != EGameState.Loading)
                OnEnterState();
        }
        //------------------------------------------------------
        public override void OnSceneCallback(Framework.Core.SceneParam sceParam)
        {
            if (m_States != null) m_States.OnSceneLoaded((ushort)sceParam.sceneID, sceParam.isCompled);
            if (sceParam.isCompled)
            {
                m_nPoolLoadingCount = m_pFileSystem.GetCurLoadingCount();

                m_bBeingVirtualLoading = true;
                m_fVirtualLoading = 0;
            }
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
        //------------------------------------------------------
        void OnEnterState()
        {
            if (m_States!=null) m_States.OnEnterState();
            m_bBeingVirtualLoading = false;
            m_fVirtualLoading = 1;
     //       if (m_pUnlockMgr != null) ((UnLockManager)m_pUnlockMgr).OnChangeState(GetPreState(), GetState());
            int modeValue = 0;
            AState pCurState = StateFactory.CurrentState();
            if(pCurState!=null)
            {
                IMode gameMode = pCurState.GetActivedMode();
                if (gameMode != null) modeValue = (int)gameMode.GetMode();
            }
            Framework.Plugin.Guide.GuideWrapper.OnEnterGameState((int)GameInstance.getInstance().GetState(), modeValue);
      //      Framework.Plugin.Guide.GuideWrapper.OnCompleteTask(Net.FightTaskHandler.CompletedTaskID, 1 << (int)GameInstance.getInstance().GetState());

            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrameTime)
        {
#if UNITY_EDITOR
            DebugConfig.Update();
#endif
            SDK.GameSDK.getInstance().Update();
            NetWork.getInstance().LateUpdate(InvTargetFrameRate);
            if (IsPause())
            {
                if (m_pFileSystem != null) m_pFileSystem.Update(fFrameTime, false);
                if (m_pTimerMgr != null) m_pTimerMgr.Update(GetRunTime(), GetRunUnScaleTime(), true);
                
                TopGame.RtgTween.RtgTweenerManager.getInstance().update((long)(InvTargetFrameRate * 1000));
                UIAnimatorFactory.getInstance().Update((long)(InvTargetFrameRate * 1000));
                Time.timeScale = 0;

                Framework.Plugin.Guide.GuideSystem.getInstance().Update(fFrameTime);
                OnNetWorkUpdate();
                CheckHeart();
                return;
            }
            else if (fFrameTime <= 0)
                fFrameTime = Time.unscaledDeltaTime;

            TopGame.RtgTween.RtgTweenerManager.getInstance().update((long)(fFrameTime * 1000));
            UIAnimatorFactory.getInstance().Update((long)(fFrameTime * 1000));
//             if (!m_bStartup)
//             {
//                 if (m_pFileSystem != null) m_pFileSystem.Update(fFrameTime, false);
//                 if (m_States != null) m_States.Update(fFrameTime);
//                 base.InnerUpdate(fFrameTime);
//                 return;
//             }

            base.InnerUpdate(fFrameTime);
            if(m_pFileSystem!=null) m_pFileSystem.Update(fFrameTime, GetStating() != EGameState.Loading);

            Framework.Plugin.AT.AgentTreeManager.getInstance().Update(fFrameTime);

            if (GetStating() == EGameState.Loading)
            {
                float fDeltaChangeState = m_States.GetDeltaChangeState();
                if (fDeltaChangeState > 0)
                    return;
                if (m_bBeingVirtualLoading && fDeltaChangeState <= 0)
                {
                    int curLoding = 0;
                    if (m_pFileSystem != null) curLoding = m_pFileSystem.GetCurLoadingCount();
                    m_nPoolLoadingCount = Mathf.Max(m_nPoolLoadingCount, curLoding);
                    if (curLoding <= 0)
                        m_fVirtualLoading = Mathf.Clamp01(Mathf.Lerp(m_fVirtualLoading, 2f, Time.fixedDeltaTime * 10f));
                    else
                        m_fVirtualLoading = Mathf.Clamp01(Mathf.Lerp(m_fVirtualLoading, 1.1f, 0.01f));
                }
                FrameworkStartUp.UpdateLoadProgress(GetProgress());


                if (UI.UILoading.IsLoadingEnd /*&& NetWork.getInstance().GetLockSize() <=0*/)
                {
                    OnEnterState();
                    return;
                }
                return;
            }
            if (m_pDynamicHudHper != null) m_pDynamicHudHper.Update(fFrameTime);
            if (m_pCameraController!=null) m_pCameraController.ForceUpdate(fFrameTime);
            if (m_pObjBubbleTalkMgr != null) m_pObjBubbleTalkMgr.Update(fFrameTime);
            if (m_pItemTweenMgr != null) m_pItemTweenMgr.Update(fFrameTime);

            UpdateTimeScaleCurve(Time.unscaledDeltaTime);

            GameDelegate.GameRun(fFrameTime);
            Core.RenderHudManager.getInstance().Update(fFrameTime);

            Framework.Plugin.Guide.GuideSystem.getInstance().Update(fFrameTime);
            OnNetWorkUpdate();

            CheckHeart();

#if USE_ANTIADDICTION
            UpdateAntiAddiction();
#endif
        }
        //------------------------------------------------------
        void CheckHeart()
        {
            var state = GetState();
            var interval = m_SendHeartInterval;
            if (state == EGameState.Battle)
            {
                interval = m_SendHeartInterval_Battle;
            }
            if (GetState() >= EGameState.Hall)
            {
                if ((Time.unscaledTime - m_SendHeartTimer) >= interval)
                {
                    SendHeart();
                    m_SendHeartTimer = Time.unscaledTime;
                }
            }
        }
        //------------------------------------------------------
        protected override void InnerLateUpdate(float fFrame)
        {
            if (m_MulTouchs != null) m_MulTouchs.Update(fFrame);
            if (m_pDynamicTexter != null) m_pDynamicTexter.Update(fFrame);
            if (IsPause())
                return;
            if (!IsLoading() && GetState() > EGameState.Login)
            {
                var currentStatte = m_States.GetCurrentState();
                GameQuality.Update(fFrame, currentStatte!=null? currentStatte.GetLowerFpsCheckerThreshold():-1);
            }
        }
        //------------------------------------------------------
        void LowMemoryCallback()
        {
            Debug.LogWarning("LowMemoryCallback");
            UI.UIManager.Free();
            m_pFileSystem.Free();
            GameDelegate.OnStatus(EAppStatus.LowMemory);

            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("同步服务器时间")]
        public override void SynchronousTime(long lTime)
        {
            base.SynchronousTime(lTime);
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
          //  if (UserManager.MySelf != null) Net.NetLoginHandler.ReqLogout( false);
            if (IsOffline) SvrData.LocalServerData.SaveUserData();

            ModuleManager.startUpGame = false;
            if (m_MulTouchs!=null)
            {
                m_MulTouchs.OnMouseDown -= Framework.Plugin.Guide.GuideWrapper.OnTouchBegin;
                m_MulTouchs.OnMouseMove -= Framework.Plugin.Guide.GuideWrapper.OnTouchMove;
                m_MulTouchs.OnMouseUp -= Framework.Plugin.Guide.GuideWrapper.OnTouchEnd;
            }        
            if (m_pLocalizationMgr != null)
            {
                m_pLocalizationMgr.Destroy();
                m_pLocalizationMgr = null;
            }

            if (m_pAgentTree != null)
            {
                m_pAgentTree.Exit();
                m_pAgentTree.Enable(false);
				AgentTreeManager.getInstance().UnloadAT(m_pAgentTree);
            }
            m_pAgentTree = null;

            Application.lowMemory -= LowMemoryCallback;

            GameDelegate.OnStatus(EAppStatus.Exit);

            AgentTreeManager.getInstance().Shutdown();

            if (m_pDynamicHudHper != null) m_pDynamicHudHper.Destroy();
            m_pDynamicHudHper = null;
            if (m_pObjBubbleTalkMgr != null) m_pObjBubbleTalkMgr.Destroy();
            m_pObjBubbleTalkMgr = null;
            if (m_pItemTweenMgr != null) m_pItemTweenMgr.Destroy();
            m_pItemTweenMgr = null;
            base.OnDestroy();

            //! AI 系统取消注册回调
#if UNITY_EDITOR
            if(aiSystem!=null)
            {
                aiSystem.Clear();
                aiSystem.UnRegister(this);
            }
            Framework.Plugin.Guide.GuideSystem.getInstance().Clear();
            Framework.Plugin.Guide.GuideSystem.getInstance().UnRegister(this);
#else
            Framework.Plugin.Guide.GuideSystem.getInstance().Destroy();
#endif

            Resources.UnloadUnusedAssets();
            GC.Collect();

            sm_pInstance = null;
        }
        //------------------------------------------------------
        public Vector3 GetCurveWorldPosition(Vector3 worldPos, bool bStandard = false)
        {
            if (GlobalShaderController.Instance == null) return worldPos;
            Camera camera = cameraController.GetCamera();
            if(camera == null)return worldPos;

            Vector3 clipPos = worldPos;
            clipPos -= GlobalShaderController.Instance.GetCWPivotPointPosition();

            Vector3 bendAxis = GlobalShaderController.Instance.GetCWBendAxis();
            Vector3 bendOffset = GlobalShaderController.Instance.GetCWBendOffset();
            Vector2 xyOff = Vector2.Max(Vector2.zero, new Vector2(clipPos.z- bendOffset.x, clipPos.z- bendOffset.y));
            xyOff *= xyOff;

            clipPos.x = bendAxis.x * xyOff.x * 0.001f * GlobalShaderController.Instance.GetCWCurve();
            clipPos.y = -bendAxis.y * xyOff.y * 0.001f * GlobalShaderController.Instance.GetCWCurve();
            clipPos.z = 0;
            worldPos += clipPos;
            return worldPos;
        }
        //-------------------------------------------------
        public void ApplayTimeScaleByCurve(AnimationCurve curve)
        {
            if (curve == null)
                return;
            m_fTimeScaleCurveDuration = Framework.Core.CommonUtility.GetCurveMaxTime(curve);
            if (m_fTimeScaleCurveDuration <= 0) return;
            m_fTimeScaleCurveDelta = 0;
            m_TimeScaleCurve = curve;
            TimeScaleDuration = 0;
        }
        //------------------------------------------------------
        void UpdateTimeScaleCurve(float fFrame)
        {
            if(m_fTimeScaleCurveDuration>0 && m_TimeScaleCurve!=null)
            {
                m_fTimeScaleCurveDelta += fFrame;
                TimeScaleFactor = m_TimeScaleCurve.Evaluate(m_fTimeScaleCurveDelta / m_fTimeScaleCurveDuration);
                if(m_fTimeScaleCurveDelta >= m_fTimeScaleCurveDuration)
                {
                    m_fTimeScaleCurveDuration = 0;
                    m_TimeScaleCurve = null;
                }
            }
        }
        //------------------------------------------------------
        public void ClearTimeScaleCurve()
        {
            m_fTimeScaleCurveDelta = 0;
            m_fTimeScaleCurveDuration = 0;
            m_TimeScaleCurve = null;
            ResetTimeScale();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取当前版本")]
        public string GetVersion()
        {
            return Core.LocalizationManager.ToLocalization(80010036, "版本号：") + VersionData.version;
        }
        //------------------------------------------------------
        public int GetPlatform()
        {
            int platform = 0;
#if UNITY_ANDROID
            platform = 2;
#elif UNITY_IOS
            platform = 1;
#else
            platform = 3;
#endif
            return platform;
        }
        //------------------------------------------------------
        public string GetDeviceUDID()
        {
            string udid = SystemInfo.deviceUniqueIdentifier;
            if (!string.IsNullOrEmpty(udid)) return udid;
            return JniPlugin.GetUDID();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取是否显示激励视频按钮")]
        public bool GetIsShowADBtn()
        {
            return GlobalSetting.Instance.bAlwayShowADBtn;
        }
#if USE_ANTIADDICTION
        private int m_LastUpdateRemainingTime;
        private int m_UnderageRemainingTime = 0;
        public void UpdateUnderageRemainingTime(int value)
        {
            m_UnderageRemainingTime = value;
            m_LastUpdateRemainingTime = (int)Time.realtimeSinceStartup;
        }
        //------------------------------------------------------
        private void UpdateAntiAddiction()
        {
            //if (m_UnderageRemainingTime == 0) return;
            //if (Time.realtimeSinceStartup - m_LastUpdateRemainingTime > m_UnderageRemainingTime)
            //{
            //    var panel = UIManager.ShowUI<AntiAddictionPanel>(EUIType.AntiAddictionPanel);
            //    panel.SetTips(80025012);
            //    m_UnderageRemainingTime = 0;
            //}
        }
#endif
    }
}
