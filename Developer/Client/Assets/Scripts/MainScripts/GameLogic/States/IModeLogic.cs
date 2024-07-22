/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AModeLogic
作    者:	HappLI
描    述:	状态模式逻辑
*********************************************************************/
using Framework.BattlePlus;
using Framework.Core;
using Framework.Logic;
using Framework.Plugin.AT;

namespace TopGame.Logic
{
    public abstract class AModeLogic : Framework.Plugin.AT.IUserData
    {
        protected IMode m_pAMode;
        protected bool m_bActive = false;
        private bool m_bToggled = true;
        //------------------------------------------------------
        public AState GetState()
        {
            if (m_pAMode == null) return null;
            return m_pAMode.GetState();
        }
        //------------------------------------------------------
        public T GetState<T>() where T : AState
        {
            return GetState() as T;
        }
        //------------------------------------------------------
        public T GetMode<T>() where T : AbsMode
        {
            return m_pAMode as T;
        }
        //------------------------------------------------------
        public T GetModeLogic<T>() where T : AModeLogic
        {
            AModeLogic logic = null;
            if (m_pAMode == null) return logic as T;
            return m_pAMode.GetLogic<T>();
        }
        //------------------------------------------------------
        public void Enable(bool bEnable)
        {
            if(m_bToggled)
            {
                if (bEnable)
                    GetFramework().RegisterFunction(this);
                else
                    GetFramework().UnRegisterFunction(this);
            }
        }
        //------------------------------------------------------
        public Framework.Module.AFramework GetFramework()
        {
            if (m_pAMode == null) return null;
            return m_pAMode.GetFramework();
        }
        //------------------------------------------------------
        public World GetWorld()
        {
            if (m_pAMode == null) return null;
            return m_pAMode.GetWorld();
        }
        //------------------------------------------------------
        public BattleWorld GetBattleWorld()
        {
            if (m_pAMode == null) return null;
            return m_pAMode.GetBattleWorld();
        }
        //------------------------------------------------------
        public uint GetSceneTheme()
        {
            if (m_pAMode != null) return m_pAMode.GetSceneTheme();
            return 0;
        }
        //------------------------------------------------------
        public SvrData.User GetUser()
        {
#if USE_SERVER
            if (m_pAMode == null) return null;
            return m_pAMode.GetUser();
#else
            if (m_pAMode == null) return SvrData.UserManager.MySelf;
            return m_pAMode.GetUser();
#endif
        }
        //------------------------------------------------------
        public T GetBattleLogic<T>() where T : ABattleLogic
        {
            if (m_pAMode == null) return null;
            return m_pAMode.GetBattleLogic<T>();
        }
        //------------------------------------------------------
        public void EnableBattleLogic<T>(bool bEnable) where T : ABattleLogic
        {
            if (m_pAMode == null) return;
            m_pAMode.EnableBattleLogic<T>(bEnable);
        }
        //------------------------------------------------------
        public void Awake(IMode pBase)
        {
            m_pAMode = pBase;
            m_bActive = false;
            m_bToggled = true;
            if (m_pAMode!=null) OnAwake();
        }
        //------------------------------------------------------
        public void StateChange(AState pState)
        {
            if (m_pAMode == null) return;
            OnStateChange();
        }
        //------------------------------------------------------
        public void PreStart()
        {
            if(m_bToggled)
            {
                m_bActive = true;
                if (m_pAMode != null) OnPreStart();
            }
        }
        //------------------------------------------------------
        public void Start()
        {
            if (m_bToggled)
            {
                m_bActive = true;
                if (m_pAMode != null) OnStart();
            }
        }
        //------------------------------------------------------
        public void Destroy()
        {
            m_bActive = false;
            m_bToggled = false;
            if(GetFramework()!=null) GetFramework().UnRegisterFunction(this);
            Clear();
            OnDestroy();
        }
        //------------------------------------------------------
        public void Clear()
        {
            OnClear();
        }
        //------------------------------------------------------
        public void SetActive(bool bActive)
        {
            if (!m_bToggled) return;
            m_bActive = bActive;
        }
        //------------------------------------------------------
        public void Toggle(bool bEnable, bool bCheckActive = true)
        {
            if (m_bToggled == bEnable) return;
            m_bToggled = bEnable;
            if(bCheckActive)
            {
                if (m_bToggled)
                {
                    if (!m_bActive)
                    {
                        if (m_pAMode != null) Enable(m_pAMode.IsEnabled());
                        PreStart();
                        Start();
                    }
                }
                else
                {
                    Enable(false);
                }
            }
        }
        //------------------------------------------------------
        public bool isActived()
        {
            return m_bActive && m_bToggled;
        }
        //------------------------------------------------------
        public bool isToggled()
        {
            return m_bToggled;
        }
        //------------------------------------------------------
        public void Update(float fFrame)
        {
            if (!isActived() || m_pAMode == null) return;
            InnerUpdate(fFrame);
        }
        //------------------------------------------------------
        public virtual bool OnNetResponse(TopGame.Net.PacketBase msg) { return false; }
        //------------------------------------------------------
        public virtual void OnBattleWorldCallback(BattleWorld pWorld, EBattleWorldCallbackType type, VariablePoolAble takeData = null) 
        {
        }
        //------------------------------------------------------
        public virtual bool OnBattleResultCheckEvent(ref EBattleResultStatus current)
        {
            return true;
        }
        //------------------------------------------------------
        public virtual void OnActorAttrChange(Actor pActor, EAttrType type, float fValue, float fOriginal, bool bPlus)
        {

        }
        //------------------------------------------------------
        public bool ExecuteCustom(ushort enterType, IUserData pParam = null)
        {
            if (m_pAMode == null) return false;
            return m_pAMode.ExecuteCustom(enterType, pParam);
        }
        //------------------------------------------------------
        public bool ExecuteCustom(UnityEngine.GameObject pGo, IUserData pParam = null)
        {
            if (m_pAMode == null) return false;
            return m_pAMode.ExecuteCustom(pGo, pParam);
        }
        //------------------------------------------------------
        public bool ExecuteCustom(string strName, IUserData pParam = null)
        {
            if (m_pAMode == null) return false;
            return m_pAMode.ExecuteCustom(strName, pParam);
        }
        //------------------------------------------------------
        public bool ExecuteCustom(int nID, IUserData pParam = null)
        {
            if (m_pAMode == null) return false;
            return m_pAMode.ExecuteCustom(nID, pParam);
        }
        //------------------------------------------------------
        public virtual void DrawGizmos() { }
        protected virtual void OnAwake() { }
        protected virtual void OnPreStart() { }
        protected virtual void OnStart() { }
        protected virtual void InnerUpdate(float fFrame) { }
        protected virtual void OnDestroy() { }
        protected virtual void OnClear() { }

        protected virtual void OnStateChange() { }
        public virtual void OnRefreshPlayers(int group=0) { }

#if UNITY_EDITOR
        public virtual string Print() { return this.GetType().ToString()+":\r\n"; }
#endif
    }
}
