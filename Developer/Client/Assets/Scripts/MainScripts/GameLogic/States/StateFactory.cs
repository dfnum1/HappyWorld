/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	StateFactory
作    者:	HappLI
描    述:	状态工厂
*********************************************************************/

using Framework.Core;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;

namespace TopGame.Logic
{
    [Framework.Plugin.AT.ATExportMono("逻辑状态", "TopGame.GameInstance.getInstance().statesFactory")]
    public partial class StateFactory : Framework.Module.AModule, Framework.Module.IUpdate, Framework.Module.IStartUp
#if UNITY_EDITOR
        ,Framework.Module.IDrawGizmos
#endif
    {
        // IState[] m_arStates = new IState[(int)EGameState.Count];
        Dictionary<int, AState> m_vStates = null;
        private Dictionary<EMode, AbsMode> m_vModes = null;

        private Dictionary<int, AStateShareResource> m_vShareResources = new Dictionary<int, AStateShareResource>(2);

        EGameState m_eState = EGameState.Count;
        EGameState m_eStating = EGameState.Count;
        EGameState m_ePreState = EGameState.Count;
        ushort m_nSceneID = 0;
        EMode m_eMode = EMode.None;
        uint m_nStateChangeFlag = (uint)EStateChangeFlag.All;
        float m_fDeltaChangeState = 0;
//         int m_nAsyncUploadTimeSlice = -1;
//         int m_nAsyncUploadBuffSize =-1;
        int m_BackgroundLoadingPriority = -1;
        ushort m_nLoadingUI = 0;
        bool m_bHideAllUIDelay = false;

        private AState m_pCurState = null;
        static StateFactory ms_pInstance = null;
#if UNITY_EDITOR
        private Framework.Base.ProfilerTicker m_pTimerLastTick = new Framework.Base.ProfilerTicker();
#endif
        //------------------------------------------------------
        public EGameState curState
        {
            get { return m_eState; }
        }
        //------------------------------------------------------
        public EGameState stating
        {
            get { return m_eStating; }
        }
        //------------------------------------------------------
        public EGameState preState
        {
            get { return m_ePreState; }
        }
        //------------------------------------------------------
        public EMode curMode
        {
            get { return m_eMode; }
        }
        //------------------------------------------------------
        public AState GetCurrentState()
        {
            return m_pCurState;
        }
        //------------------------------------------------------
        protected override void Awake()
        {
            ms_pInstance = this;
            m_vModes = new Dictionary<EMode, AbsMode>((int)EMode.Count);
            m_vStates = new Dictionary<int, AState>((int)EGameState.Count);
        }
        //------------------------------------------------------
        public void StartUp()
        {

        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            ms_pInstance = null;
            foreach (var state in m_vStates)
            {
                if (state.Value != null)
                    state.Value.OnShutdown();
            }
            m_vStates.Clear();
        }
        //------------------------------------------------------
        T GetShareResource<T>(bool bAutoCreate = true) where T : AStateShareResource, new()
        {
            int hashCode = typeof(T).GetHashCode();
            AStateShareResource res = null;
            if (m_vShareResources.TryGetValue(hashCode, out res))
                return (T)res;
            if(bAutoCreate)
            {
                res = new T();
                m_vShareResources.Add(hashCode, res);
            }
            return (T)res;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取状态")]
        public AState GetState(EGameState state, bool bAutoNode=true)
        {
            System.Type type = GetType(state);
            if (type == null) return null;

            int hash = type.GetHashCode();
            AState pState = null;
            if (m_vStates.TryGetValue(hash, out pState))
            {
                pState.SetFramework(m_pFramework);
                return pState;
            }
            if(bAutoNode)
            {
                pState = NewState(state);
                m_vStates.Add(hash, pState);
            }
            if (pState != null)
            {
                pState.SetFramework(m_pFramework);
            }

            return pState;
        }
        //------------------------------------------------------
        public T GetState<T>(bool bAutoNew =true) where T : AState, new()
        {
            int hash = typeof(T).GetHashCode();
            AState pState = null;
            if (m_vStates.TryGetValue(hash, out pState))
            {
                return (T)pState;
            }
            if (!bAutoNew) return null;

            pState = new T();
            m_vStates.Add(hash, pState);
            return (T)pState;
        }
        //------------------------------------------------------
        public void RemoveState<T>() where T : AState
        {
            int hash = typeof(T).GetHashCode();
            AState pState = null;
            if (m_vStates.TryGetValue(hash, out pState))
            {
                pState.OnExit();
                m_vStates.Remove(hash);
            }
        }
        //------------------------------------------------------
        public static AState CurrentState()
        {
            Logic.StateFactory state = GameInstance.getInstance().Get<Logic.StateFactory>();
            if (state == null) return null;
            return state.m_pCurState;
        }
        //------------------------------------------------------
        public static T CurrentState<T>() where T : AState
        {
            if (ms_pInstance == null) return null;
            return ms_pInstance.m_pCurState as T;
        }
        //------------------------------------------------------
        public static T Get<T>(bool bAutoNew = true) where T : AState, new()
        {
            if (ms_pInstance == null) return null;
            return ms_pInstance.GetState<T>(bAutoNew);
        }
        //------------------------------------------------------
        public static T ShareResource<T>(bool bAutoNew = true) where T : AStateShareResource, new()
        {
            if (ms_pInstance == null) return null;
            return ms_pInstance.GetShareResource<T>(bAutoNew);
        }
        //------------------------------------------------------
        public IMode SwitchState(EGameState eState, Base.ELoadingType loadingType = Base.ELoadingType.Loading, EMode mode = EMode.None, bool bForce = false, uint clearFlag = 0)
        {
            if(GetFramework()!=null)
            {
                if (eState == EGameState.Battle)
                {
#if UNITY_IOS
//                     if (Data.GameQuality.Qulity >= Data.EGameQulity.High)
//                     {
//                         Data.GameQuality.Qulity = Data.EGameQulity.Middle;
//                         Data.GameQuality.Qulity = Data.EGameQulity.High;
//                     }
                    GetFramework().TargetFrameRate = 60;
#else
                    GetFramework().TargetFrameRate = 60;
#endif
                }
                else GetFramework().TargetFrameRate = TopGame.Data.GameQuality.targetFrameRate;
            }

            if (!bForce && eState == m_eState)
            {
                if (m_pCurState != null)
                {
                    if (m_eMode != mode || m_pCurState.GetActivedMode() == null)
                    {
                        AbsMode pMode = CreateMode(mode);
                        if (pMode != null)
                        {
                            m_pCurState.SwitchMode(pMode);
                        }
                        m_eMode = mode;
                    }
                    return m_pCurState.GetActivedMode();
                }
            }
#if UNITY_EDITOR
            m_pTimerLastTick.Start("ChangeState");
#endif
            m_nStateChangeFlag = clearFlag;
            if (m_nStateChangeFlag == 0) m_nStateChangeFlag = GetClearFlags(m_eState, eState);

            Framework.UI.UIBaseFramework uiFramework = UI.UIKits.GetUIFramework();

            m_ePreState = m_eState;
            m_eState = eState;
            if(m_nLoadingUI!=0)
            {
                if (uiFramework != null)
                {
                    UI.UIBase loading = uiFramework.CastGetUI<UI.UIBase>(m_nLoadingUI, false);
                    if (loading != null)
                    {
                        loading.Hide();
                    }
                }
            }
            m_nLoadingUI = 0;
            m_bHideAllUIDelay = false;
//             if(m_nAsyncUploadTimeSlice<0)
//                 m_nAsyncUploadTimeSlice = QualitySettings.asyncUploadTimeSlice;
//             if (m_nAsyncUploadBuffSize < 0)
//                 m_nAsyncUploadBuffSize = QualitySettings.asyncUploadBufferSize;
            if (m_BackgroundLoadingPriority < 0)
                m_BackgroundLoadingPriority = (int)Application.backgroundLoadingPriority;
         //   QualitySettings.asyncUploadTimeSlice = 2;
         //   QualitySettings.asyncUploadBufferSize = 16;
            Application.backgroundLoadingPriority = ThreadPriority.High;
         //   FileSystemUtil.SetCapability(3000, 3000, -1);
            m_fDeltaChangeState = ClearFlagUtil.GetDetaChangeTime(m_nStateChangeFlag);
            if (loadingType == Base.ELoadingType.BlackFull)
                m_fDeltaChangeState = Mathf.Max(m_fDeltaChangeState, 0.5f);
            Framework.Module.ModuleManager.mainModule.ResetRuntime(m_nStateChangeFlag);

            m_eStating = Logic.EGameState.Loading;
            m_nSceneID = 0;
            m_eMode = mode;

            AState pState = GetState(m_eState);

            if (!StateFactory.DeltaChangeState(eState) || m_fDeltaChangeState<=0)
            {
             //   m_eStating = eState;
                m_fDeltaChangeState = 0;
                CheckLoding(pState, loadingType, uiFramework);
                ChangeState();
            }
            else
            {
                CheckLoding(pState, loadingType, uiFramework);
            }
            
            return pState.GetActivedMode();
        }
        //------------------------------------------------------
        public void BeginStateLoding(Base.ELoadingType loadingType)
        {
            if (m_pCurState == null || m_nLoadingUI!=0) return;
            CheckLoding(m_pCurState, loadingType);
        }
        //------------------------------------------------------
        void CheckLoding(AState pState, Base.ELoadingType loadingType, Framework.UI.UIBaseFramework uiFramework = null)
        {
            if(uiFramework == null)
            {
                uiFramework = UI.UIKits.GetUIFramework();
            }
            if (uiFramework == null) return;

            m_bHideAllUIDelay = false;
            if (loadingType == Base.ELoadingType.Loading)
            {
                m_bHideAllUIDelay = false;
                m_nLoadingUI = (ushort)UI.EUIType.Loading;
                UI.UILoading loading = uiFramework.CastGetUI<UI.UILoading>((ushort)UI.EUIType.Loading, true);
                if (loading != null)
                {
                    Framework.Plugin.Media.IMediaPlayer mediaPlayer = null;
                    string loadingbg = null;
                    UI.UIVideo video = uiFramework.CastGetUI<UI.UIVideo>((ushort)UI.EUIType.VideoPanel);
                    if (video != null && video.IsVisible())
                    {
                        loading.SetBGTexture(null);
                        if (m_eState > EGameState.Login)
                        {
                            mediaPlayer = video.GetVideoPlayer();
                            video.ClearVideoPlayer(true);
                            video.Hide();
                        }
                    }
                    else
                    {
                        loadingbg = Data.DefaultResources.DefaultLoading;
                        if (pState != null) loadingbg = pState.GetLoadingBG();
                    }

                    if (ClearFlagUtil.StateChangeFlag(m_nStateChangeFlag, EStateChangeFlag.HideAllUI))
                    {
                        if (uiFramework != null) uiFramework.HideAll();
                    }
                    if (mediaPlayer != null)
                        loading.SetVideoBG(mediaPlayer);
                    else
                    {
                        loading.SetBGTexture(loadingbg);
                    }
                    loading.Show();
                }
            }
            else if (loadingType == Base.ELoadingType.BlackFull)
            {
                UI.UIFullScreenFillPanel loading = uiFramework.CastGetUI<UI.UIFullScreenFillPanel>((ushort)UI.EUIType.FullScreenFillPanel, true);
                if (loading != null)
                {
                    m_bHideAllUIDelay = true;
                    m_nLoadingUI = (ushort)UI.EUIType.FullScreenFillPanel;

                    UI.UIVideo video = uiFramework.CastGetUI<UI.UIVideo>((ushort)UI.EUIType.VideoPanel);
                    if (video != null && video.IsVisible())
                    {
                        video.ClearVideoPlayer(true);
                        video.Hide();
                    }
                    loading.Show();
                    loading.SetBeginTween(0.5f);
                }
                else
                {
                    CheckLoding(pState, Base.ELoadingType.Loading, uiFramework);
                }
            }
            else if (loadingType == Base.ELoadingType.ModeTransition)
            {
                UI.TransitionPanel loading = uiFramework.CastGetUI<UI.TransitionPanel>((ushort)UI.EUIType.TransitionPanel, true);
                if (loading != null)
                {
                    m_nLoadingUI = (ushort)UI.EUIType.TransitionPanel;

                    Framework.Plugin.Media.IMediaPlayer mediaPlayer = null;
                    string loadingbg = null;
                    UI.UIVideo video = uiFramework.CastGetUI<UI.UIVideo>((ushort)UI.EUIType.VideoPanel);
                    if (video != null && video.IsVisible())
                    {
                        loading.SetBGTexture(null);
                        if (m_eState > EGameState.Login)
                        {
                            mediaPlayer = video.GetVideoPlayer();
                            video.ClearVideoPlayer(true);
                            video.Hide();
                        }
                    }
                    else
                    {
                        loadingbg = Data.DefaultResources.DefaultLoading;
                        if (pState != null) loadingbg = pState.GetLoadingBG();
                    }

                    if (ClearFlagUtil.StateChangeFlag(m_nStateChangeFlag, EStateChangeFlag.HideAllUI))
                    {
                        if (uiFramework != null) uiFramework.HideAll();
                    }

                    if (mediaPlayer != null)
                        loading.SetVideoBG(mediaPlayer);
                    else
                    {
                        loading.SetBGTexture(loadingbg);
                    }
                    loading.Show(m_eMode);
                }
                else
                {
                    CheckLoding(pState, Base.ELoadingType.Loading, uiFramework);
                }
            }
        }
        //------------------------------------------------------
        public void EndStateLoding()
        {
            if (m_nLoadingUI == 0) return;
            Framework.UI.UIBaseFramework uiFramework = UI.UIKits.GetUIFramework();
            if (uiFramework != null)
            {
                UI.UIBase loading = uiFramework.CastGetUI<UI.UIBase>(m_nLoadingUI, false);
                if (loading != null)
                {
                    loading.Hide();
                }
            }
            m_nLoadingUI = 0;
        }
        //------------------------------------------------------
        public void OnSceneLoaded(ushort sceneID, bool bCompleted)
        {
     //       Framework.Module.ModuleManager.getInstance().ResumeJobs();
            if (bCompleted)
            {
                AState pState = GetState(m_eState);
                if (pState != null) pState.OnPreStart();
                GameDelegate.OnChangeState((uint)m_eState, EStateStatus.PreStart, 0);
            }
        }
        //------------------------------------------------------
        public void OnEnterState()
        {
            m_eStating = m_eState;
            AState pState = GetState(m_eState);
            if (pState != null) pState.OnStart();
            GameDelegate.OnChangeState((uint)m_eState, EStateStatus.Start, 0);
            EndStateLoding();
            m_fDeltaChangeState = 0;
            m_nStateChangeFlag = (uint)EStateChangeFlag.All;

//            FileSystemUtil.SetCapability(Data.GameQuality.OneFrameCost, Data.GameQuality.MaxInstanceCount, -1);
//             if (m_nAsyncUploadTimeSlice > 0) QualitySettings.asyncUploadTimeSlice = m_nAsyncUploadTimeSlice;
//             if (m_nAsyncUploadBuffSize > 0) QualitySettings.asyncUploadBufferSize = m_nAsyncUploadBuffSize;
            if(m_BackgroundLoadingPriority>=0) Application.backgroundLoadingPriority = (UnityEngine.ThreadPriority)m_BackgroundLoadingPriority;
            m_BackgroundLoadingPriority = -1;
//             m_nAsyncUploadBuffSize = -1;
//            m_nAsyncUploadTimeSlice = -1;

#if UNITY_EDITOR
            m_pTimerLastTick.Stop();
#endif
        }
        //------------------------------------------------------
        void ChangeState()
        {
            AState preAState = m_pCurState;
            if(preAState == null) preAState = GetState(m_eState);
            if (preAState != null)
            {
                preAState.OnExit();
            }
            GameDelegate.OnChangeState((uint)m_eState, EStateStatus.Init, 0);
            if (m_nSceneID <= 0 && ClearFlagUtil.StateChangeFlag(m_nStateChangeFlag, EStateChangeFlag.SceneID))
                m_nSceneID = GetSceneID(m_eState, m_eMode);

            if(m_bHideAllUIDelay)
            {
                Framework.UI.UIBaseFramework uiFramework = UI.UIKits.GetUIFramework();
                if (uiFramework != null)
                {
                    uiFramework.HideAll();
                }
            }
            m_bHideAllUIDelay = false;

            ClearFlagUtil.Clear(m_ePreState, m_eState, m_nStateChangeFlag);

            AState pState = GetState(m_eState);
            if (pState != null)
            {
                m_pCurState = pState;
                RegisterStateLogic(pState, m_eState);
                RegisterMode(pState, m_eMode);

                //! if common state ,exit before
                if (m_eMode !=  EMode.None && m_pCurState.GetActivedMode() == null)
                {
                    AbsMode pMode = CreateMode(m_eMode);
                    if (pMode != null)
                    {
                        m_pCurState.SwitchMode(pMode);
                    }
                }

                pState.OnInit(m_eState);
                pState.BindSceneID(m_nSceneID);
                pState.OnAwake();



                if (m_nSceneID != 0)
                {
                    SDK.Bugly.SetCurrentScene((int)m_eState);
                    if (GameInstance.getInstance().sceneManager != null)
                    {
                        if(m_eState > EGameState.Login && m_ePreState < EGameState.Count && m_ePreState > EGameState.Login)
                        {
                            long reaminMemory = GetFramework().GetReaminMemory() / 1024;
                            if (Data.GameQuality.ThresholdSystemMemory > 0 && reaminMemory >= Data.GameQuality.ThresholdSystemMemory)
                                GameInstance.getInstance().sceneManager.LoadScene(m_nSceneID, 600);
                            else
                                GameInstance.getInstance().sceneManager.LoadScene(m_nSceneID, 180);
                        }
                        else
                        {
                            GameInstance.getInstance().sceneManager.LoadScene(m_nSceneID, 0);
                        }
                    }
                    else
                    {
                        Framework.Plugin.Logger.Error("sceneMgr == null");
                    }
                }
                else
                {
                    GameInstance.getInstance().OnStateNoSceneChangeEnd();
                }
            }
        }
        //------------------------------------------------------
        public void OnActorAttrChange(Actor pActor, EAttrType type, float fValue, float fOriginal, bool bPlus)
        {
            if (m_pCurState == null) return;
            m_pCurState.OnActorAttrChange(pActor, type, fValue, fOriginal, bPlus);
        }
        //------------------------------------------------------
        public void Update(float fFrame)
        {
            if(m_eStating == EGameState.Loading)
            {
                if (m_fDeltaChangeState > 0)
                {
                    m_fDeltaChangeState -= fFrame;
                    if (m_fDeltaChangeState < 0)
                        ChangeState();
                    return;
                }
                m_fDeltaChangeState = -1;
                return;
            }
            if (m_pCurState == null || !m_pCurState.IsEnable()) return;
            m_pCurState.OnUpdate(fFrame);
            foreach(var db in m_vShareResources)
            {
                db.Value.Update(m_pCurState, fFrame);
            }
        }
        //------------------------------------------------------
        public float GetDeltaChangeState()
        {
            return m_fDeltaChangeState;
        }
        //------------------------------------------------------
        public void DrawGizmos()
        {
            if (m_pCurState == null) return;
            m_pCurState.OnDrawGizmos();
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public string Print()
        {
            if (m_pCurState == null) return "not state";
            return m_pCurState.Print();
        }
#endif
    }

}
