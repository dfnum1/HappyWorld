/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	EGameState
作    者:	HappLI
描    述:	游戏状态逻辑
            一个状态可以包含多个模式（IMode）
*********************************************************************/

using Framework.Plugin.AT;
using System.Collections.Generic;
using TopGame.Core;
using Framework.Core;
using Framework.Logic;
using Framework.BattlePlus;
using Framework.Data;
using UnityEngine;

namespace TopGame.Logic
{
    [Framework.Plugin.AT.ATExportMono("逻辑状态")]
    public abstract class AState : AAPICallFlag, Framework.Plugin.AT.IUserData
    {
        struct ReDoCallback : Framework.Core.VariablePoolAble
        {
            public System.Action onCallback;
            public void Destroy()
            {
            }
        }

        protected ushort m_nSceneID = 0;
        protected SvrData.User m_pUser = null;
        protected IMode m_pActivedMode = null;

        protected Dictionary<int, AStateLogic> m_vLogic = new Dictionary<int, AStateLogic>(2);
        
        protected bool m_bActive = false;
        private bool m_bInited = false;

        protected EGameState m_eState = EGameState.Count;

        protected Framework.Module.AFrameworkBase m_pFrameWork = null;
#if !USE_SERVER
        protected AgentTree m_pAgentTree = null;
        public AgentTree AT
        {
            get { return m_pAgentTree; }
            set
            {
                if(m_pAgentTree != null)
                {
                    AgentTreeManager.getInstance().UnloadAT(m_pAgentTree);
                }
                m_pAgentTree = value;
            }
        }
#endif
        //------------------------------------------------------
        public EGameState gameState
        {
            get { return m_eState; }
        }
        //------------------------------------------------------
        ~AState()
        {
#if !USE_SERVER
            if (m_pAgentTree != null)
            {
                AgentTreeManager.getInstance().UnloadAT(m_pAgentTree);
            }
            m_pAgentTree = null;
#endif
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public virtual string Print()
        {
            string str = this.GetType().Name + ":\r\nSceneID:" + m_nSceneID.ToString() + "\r\n";

            if (m_pActivedMode != null) return m_pActivedMode.Print();

            return str;
        }
#endif
        //------------------------------------------------------
        public void SetFramework(Framework.Module.AFrameworkBase pFramework)
        {
            m_pFrameWork = pFramework;
        }
        //------------------------------------------------------
        public virtual void OnInit(EGameState state)
        {
            if (m_bInited) return;
            m_bInited = true;

            resetAPICalled();

            GetFramework().RegisterFunction(this);
            m_eState = state;
            m_nSceneID = 0;
            m_bActive = true;
#if !USE_SERVER
            if (m_pAgentTree == null)
            {
                m_pAgentTree = Framework.Plugin.AT.AgentTreeManager.getInstance().LoadAT(Data.ATModuleSetting.GetStateAT((int)state));
                if (m_pAgentTree != null)
                {
                    m_pAgentTree.AddOwnerClass(this);
                    m_pAgentTree.Enable(false);
                }
            }
#endif
        }
        //------------------------------------------------------
        public virtual void OnAwake()
        {
            if (isAPICalled(EAPICallType.Awake)) return;
            resetAPICalled();
            setAPICalled(EAPICallType.Awake);
            if (m_pActivedMode != null)
            {
                m_pActivedMode.BindState(this);
                m_pActivedMode.Awake();
                m_pActivedMode.Enable(true);
            }

            foreach (var db in m_vLogic)
            {
                GetFramework().RegisterFunction(db.Value);
                db.Value.Awake(this);
            }
            if (m_pActivedMode != null) m_pActivedMode.Awake();

            m_bActive = false;
        }
        //------------------------------------------------------
        public virtual void OnStart()
        {
            if (isAPICalled(EAPICallType.Start)) return;
            setAPICalled(EAPICallType.Start);

            foreach (var db in m_vLogic)
                db.Value.Start();

            if (m_pActivedMode != null)
            {
                m_pActivedMode.Start();
                m_pActivedMode.Enable(true);
            }
            m_bActive = true;
        }
        //------------------------------------------------------
        public virtual void OnPreStart()
        {
            if (isAPICalled(EAPICallType.PreStart)) return;
            setAPICalled(EAPICallType.PreStart);

#if !USE_SERVER
            //! set theme
            if (GetBindSceneID() > 0 && Data.DataManager.getInstance() != null)
            {
                Data.CsvData_Scene.SceneData sceneData = Data.DataManager.getInstance().Scene.GetData(GetBindSceneID());
                if (sceneData != null && sceneData.nTheme > 0)
                {
                    ASceneTheme themer = Core.SceneMgr.GetThemer();
                    if (themer != null)
                        themer.UseTheme(sceneData.nTheme, false, false);
                }
            }
#endif

            foreach (var db in m_vLogic)
                db.Value.PreStart();
               
            if (m_pActivedMode != null) m_pActivedMode.PreStart();
            m_bActive = true;
#if !USE_SERVER
            if (m_pAgentTree != null)
            {
                m_pAgentTree.Enable(true);
                m_pAgentTree.Enter();
            }
#endif
        }
        //------------------------------------------------------
        public virtual void OnExit()
        {
            if (isAPICalled(EAPICallType.Destroy) || GetFramework() == null) return;
            setAPICalled(EAPICallType.Destroy);

            if (GetFramework() != null)
            {
                GetFramework().ResetRuntime();
            }

            GetFramework().UnRegisterFunction(this);
            m_nSceneID = 0;
            foreach (var db in m_vLogic)
            {
                db.Value.Exit();
                GetFramework().UnRegisterFunction(db.Value);
            }
            if (m_pActivedMode != null) m_pActivedMode.Destroy();
            m_pActivedMode = null;
#if !USE_SERVER
            if (m_pAgentTree != null)
            {
                m_pAgentTree.Exit();
                m_pAgentTree.Enable(false);
            }
#endif
            m_bActive = false;
            m_bInited = false;
            resetAPICalled();
            setAPICalled(EAPICallType.Destroy);

            TimerManager.UnRegisterTimer(GetFramework(), "OnAStateReDoTimer");

            //  m_pFrameWork = null;
            m_pUser = null;
        }
        //------------------------------------------------------
        public void OnBattleCallback(BattleWorld pWorld, EBattleWorldCallbackType type, VariablePoolAble takeData = null)
        {
            foreach (var db in m_vLogic)
            {
                db.Value.OnBattleWorldCallback(type, takeData);
            }
        }
        //------------------------------------------------------
        public void ReDo(bool bLoading= true, System.Action onCallback = null)
        {
#if !USE_SERVER
            UI.UIFullScreenFillPanel loading = UI.UIKits.CastGetUI<UI.UIFullScreenFillPanel>(true);
            if (loading != null)
            {
                loading.Show();
                loading.SetBeginTween(0.1f);
                loading.AutoHide(0.2f);

                ReDoCallback call = new ReDoCallback();
                call.onCallback = onCallback;
                TimerManager.RegisterTimer(GetFramework(), "OnAStateReDoTimer", OnAStateReDoTimer, 100, 1, false, true, call);
            }
            else
            {
                OnReDo();
            }
#else
             OnReDo();
#endif
        }
        //------------------------------------------------------
        private bool OnAStateReDoTimer(int tickHandle, IUserData param)
        {
            OnReDo();
            if(param is ReDoCallback)
            {
                ReDoCallback call =((ReDoCallback)param);
               if(call.onCallback!=null) call.onCallback();
            }
            return false;
        }
        //------------------------------------------------------
        protected virtual void OnReDo()
        {
            if (m_pActivedMode == null) return;
            bool bEnable = m_pActivedMode.IsEnabled();

            foreach (var db in m_vLogic)
            {
                db.Value.Clear();
            }

            m_pActivedMode.Destroy();
            GetFramework().Resume();

            m_pActivedMode.BindState(this);
            m_pActivedMode.Awake();
            foreach (var db in m_vLogic)
                db.Value.PreStart();
            m_pActivedMode.PreStart();

            foreach (var db in m_vLogic)
                db.Value.Start();
            m_pActivedMode.Start();
            m_pActivedMode.Enable(bEnable);
        }
        //------------------------------------------------------
        public virtual void OnShutdown()
        {
            OnExit();
            m_pActivedMode = null;
        }
        //------------------------------------------------------
        public Framework.Module.AFramework GetFramework()
        {
            if (m_pFrameWork == null) return null;
            return m_pFrameWork as Framework.Module.AFramework;
        }
        //------------------------------------------------------
        public Framework.Plugin.AI.AISystem GetAISystem()
        {
            var framework = GetFramework();
            if (framework == null) return null;
            return framework.aiSystem;
        }
        //------------------------------------------------------
        public World GetWorld()
        {
            var framework = GetFramework();
            if (framework == null) return null;
            return framework.world;
        }
        //------------------------------------------------------
        public SvrData.User GetUser()
        {
            if (m_pUser != null) return m_pUser;
#if !USE_SERVER
            return SvrData.UserManager.MySelf;
#else
            return null;
#endif
        }
        //------------------------------------------------------
        public void SetUser(SvrData.User user)
        {
            m_pUser = user;
        }
        //------------------------------------------------------
        public virtual BattleWorld GetBattleWorld()
        {
            return null;
        }
        //------------------------------------------------------
        public void Preload(string strFile)
        {
#if !USE_SERVER
            if (m_pFrameWork == null || !(m_pFrameWork is GameInstance)) return;
            (m_pFrameWork as GameInstance).PreloadAsset(strFile);
#endif
        }
        //------------------------------------------------------
        public void PreloadInstance(string strFile, int insCnt= 1)
        {
#if !USE_SERVER
            if (m_pFrameWork == null || !(m_pFrameWork is GameInstance)) return;
            int hadCnt = FileSystemUtil.StatsInstanceCount(strFile);
            if (hadCnt < insCnt)
            {
                (m_pFrameWork as GameInstance).PreloadInstance(strFile, insCnt- hadCnt);
            }
#endif
        }
        //------------------------------------------------------
        public T GetBattleLogic<T>() where T : ABattleLogic
        {
            BattleWorld bw = GetBattleWorld();
            if (bw == null) return null;
            return bw.GetLogic<T>();
        }
        //------------------------------------------------------
        public void EnableBattleLogic<T>(bool bEnable) where T : ABattleLogic
        {
            BattleWorld bw = GetBattleWorld();
            if (bw == null) return;
            bw.EnableLogic<T>(bEnable);
        }
        //------------------------------------------------------
        public void Active(bool bActive)
        {
            if (m_bActive == bActive) return;
            m_bActive = bActive;

            if (m_pActivedMode != null) m_pActivedMode.Enable(bActive);

            foreach (var db in m_vLogic)
                db.Value.Enable(bActive);
#if !USE_SERVER
            if (m_pAgentTree != null) m_pAgentTree.Enable(bActive);
#endif
        }
        //------------------------------------------------------
        public virtual void Clear()
        {
            foreach(var db in m_vLogic)
            {
                db.Value.Clear();
            }
        }
        //------------------------------------------------------
        public T FindLogic<T>() where T : AStateLogic
        {
            int hashCode = typeof(T).GetHashCode();
            AStateLogic res = null;
            if (m_vLogic.TryGetValue(hashCode, out res))
                return res as T;
            return res as T;
        }
        //------------------------------------------------------
        public T GetLogic<T>(bool bAutoNew = true) where T : AStateLogic, new()
        {
            int hashCode = typeof(T).GetHashCode();
            AStateLogic res = null;
            if (m_vLogic.TryGetValue(hashCode, out res))
                return res as T;

            res = new T();
            m_vLogic.Add(hashCode, res);
            return res as T;
        }
        //------------------------------------------------------
        public bool SwitchMode(IMode mode)
        {
            if (mode == null || m_pActivedMode == mode) return false;
            if (m_pActivedMode != null)
                m_pActivedMode.Destroy();
            m_pActivedMode = mode;
            m_pActivedMode.BindState(this);

            if (isAPICalled(EAPICallType.Awake)) m_pActivedMode.Awake();
            if (isAPICalled(EAPICallType.PreStart)) m_pActivedMode.PreStart();
            if (isAPICalled(EAPICallType.Start)) m_pActivedMode.Start();

            return true;
        }
        //------------------------------------------------------
        public T GetCurrentMode<T>() where T : AbsMode
        {
            return m_pActivedMode as T;
        }
        //------------------------------------------------------
        public IMode GetActivedMode()
        {
            return m_pActivedMode;
        }
        //------------------------------------------------------
        public T GetCurrentModeLogic<T>() where T : AModeLogic
        {
            AModeLogic logic = null;
            if (m_pActivedMode != null) return m_pActivedMode.GetLogic<T>();
            return logic as T;
        }
        //------------------------------------------------------
        public virtual VariablePoolAble GetUserData()
        {
            return null;
        }
        //------------------------------------------------------
        public uint GetSceneTheme()
        {
#if USE_SERVER
            return 0;
#else
            if (m_pActivedMode != null) return m_pActivedMode.GetSceneTheme();
            Core.ASceneTheme themer = Core.SceneMgr.GetThemer();
            if (themer == null) return 0;
            return (uint)themer.GetCurTheme();
#endif
        }
        //------------------------------------------------------
        public Actor GetCurrentActor()
        {
            if (m_pActivedMode == null) return null;
            return m_pActivedMode.GetCurrentPlayer();
        }
        //------------------------------------------------------
        public virtual List<Actor> GetPlayers(int index=0)
        {
            if (m_pActivedMode == null) return null;
            return m_pActivedMode.GetPlayers(index);
        }
        //------------------------------------------------------
        public virtual Actor GetWingman(int index = 0)
        {
            if (m_pActivedMode == null) return null;
            return m_pActivedMode.GetWingman(index);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("是否启用")]
        public bool IsEnable()
        {
            return m_bActive;
        }
        //------------------------------------------------------
        public virtual void OnUpdate(float fFrameTime)
        {
            if (!m_bActive) return;
            if (m_pActivedMode != null)
                m_pActivedMode.Update(fFrameTime);
            foreach (var db in m_vLogic)
                db.Value.Update(fFrameTime);
        }
        //------------------------------------------------------
        public virtual void OnDrawGizmos()
        {
            if (!m_bActive) return;
#if UNITY_EDITOR
            foreach (var db in m_vLogic)
                db.Value.DrawGizmos();
#endif
            if (m_pActivedMode != null)
                m_pActivedMode.DrawGizmos();
        }
        //------------------------------------------------------
        public bool OnNetResponse(Net.PacketBase msg)
        {
            int cnt = 0;
            if (OnInnerNetResponse(msg)) cnt++;
            if(m_vLogic!=null)
            {
                foreach (var db in m_vLogic)
                {
                    if (db.Value!=null && db.Value.OnNetResponse(msg))
                        cnt++;
                }
            }
            if (m_pActivedMode!=null)
            {
                if (m_pActivedMode.OnNetResponse(msg)) cnt++;
            }
            return cnt>0;
        }
        //------------------------------------------------------
        protected virtual bool OnInnerNetResponse(Net.PacketBase msg) { return false; }
        //------------------------------------------------------
        public virtual void OnActorAttrChange(Actor pActor, EAttrType type, float fValue, float fOriginal, bool bPlus)
        {
            if(m_pActivedMode!=null)
            {
                m_pActivedMode.OnActorAttrChange(pActor,type, fValue, fOriginal, bPlus);
            }
        }
        //------------------------------------------------------
        public bool OnLockHitTargetCondition(AWorldNode pTrigger, AWorldNode pTarget, ELockHitType hitType, ELockHitCondition condition, Vector3Int conditionParam)
        {
            if (m_pActivedMode != null)
            {
                if (!m_pActivedMode.OnLockHitTargetCondition(pTrigger, pTarget, hitType, condition, conditionParam))
                    return false;
            }
            return true;
        }
        //------------------------------------------------------
        public bool ExecuteCustom(ushort enterType, IUserData pParam = null)
        {
#if USE_SERVER
            return false;
#else
            if (m_pAgentTree == null) return false;
            return m_pAgentTree.ExecuteCustom(enterType, pParam);
#endif
        }
        //------------------------------------------------------
        public bool ExecuteCustom(UnityEngine.GameObject pGo, IUserData pParam = null)
        {
#if USE_SERVER
            return false;
#else
            if (m_pAgentTree == null) return false;
            return m_pAgentTree.ExecuteCustom(pGo, pParam);
#endif
        }
        //------------------------------------------------------
        public bool ExecuteCustom(string strName, IUserData pParam = null)
        {
#if USE_SERVER
            return false;
#else
            if (m_pAgentTree == null) return false;
            return m_pAgentTree.ExecuteCustom(strName, pParam);
#endif
        }
        //------------------------------------------------------
        public bool ExecuteCustom(int nID, IUserData pParam = null)
        {
#if USE_SERVER
            return false;
#else
            if (m_pAgentTree == null) return false;
            return m_pAgentTree.ExecuteCustom(nID, pParam);
#endif
        }
        //------------------------------------------------------
        public void BindSceneID(ushort nSceneID)
        {
            m_nSceneID = nSceneID;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取绑定场景")]
        public ushort GetBindSceneID()
        {
            return m_nSceneID;
        }
        //------------------------------------------------------
        public virtual string GetLoadingBG()
        {
#if USE_SERVER
            return null;
#else
            return Data.DefaultResources.DefaultLoading;
#endif
        }
        //------------------------------------------------------
        public virtual float GetLowerFpsCheckerThreshold()
        {
            return -1;
        }
        //------------------------------------------------------
        public void Destroy()
        {
#if !USE_SERVER
            if (m_pAgentTree != null) m_pAgentTree.Destroy();
#endif
        }
#if !USE_SERVER
        //------------------------------------------------------
        static StateFactory GetStateFactory(Framework.Module.AFrameworkBase pFramework = null)
        {
            if (pFramework == null) pFramework = Framework.Module.ModuleManager.mainModule;
            if (pFramework == null) return null;
            return pFramework.Get<StateFactory>();
        }
        //------------------------------------------------------
        public static T CastCurrentMode<T>(Framework.Module.AFrameworkBase pFramework = null) where T : AbsMode
        {
            StateFactory pStateFactory = GetStateFactory(pFramework);
            if (pStateFactory == null) return null;
            AState pState = pStateFactory.GetCurrentState();
            if (pState == null) return null;
            return pState.GetCurrentMode<T>();
        }
        //------------------------------------------------------
        public static T CastCurrentMode<T>(EGameState state, Framework.Module.AFrameworkBase pFramework = null) where T : AbsMode
        {
            StateFactory pStateFactory = GetStateFactory(pFramework);
            if (pStateFactory == null) return null;
            AState pState = pStateFactory.GetState(state, false);
            if (pState == null) return null;
            return pState.GetCurrentMode<T>();
        }
        //------------------------------------------------------
        public static T CastCurrentModeLogic<T>(Framework.Module.AFrameworkBase pFramework = null) where T : AModeLogic
        {
            StateFactory pStateFactory = GetStateFactory(pFramework);
            if (pStateFactory == null) return null;
            AState pState = pStateFactory.GetCurrentState();
            if (pState == null) return null;
            IMode pMode = pState.GetActivedMode();
            if (pMode == null) return null;
            return pMode.GetLogic<T>();
        }
        //------------------------------------------------------
        public static T CastCurrentLogic<T>(Framework.Module.AFrameworkBase pFramework = null) where T : AStateLogic
        {
            StateFactory pStateFactory = GetStateFactory(pFramework);
            if (pStateFactory == null) return null;
            AState pState = pStateFactory.GetCurrentState();
            if (pState == null) return null;
            return pState.FindLogic<T>();
        }
        //------------------------------------------------------
        public static T CastLogic<T>(EGameState state, bool bAutoNew = true, Framework.Module.AFrameworkBase pFramework = null) where T : AStateLogic, new()
        {
            StateFactory pStateFactory = GetStateFactory(pFramework);
            if (pStateFactory == null) return null;
            AState pState = pStateFactory.GetState(state, bAutoNew);
            if (pState == null) return null;
            return pState.GetLogic<T>(bAutoNew);
        }
#endif
    }
}
