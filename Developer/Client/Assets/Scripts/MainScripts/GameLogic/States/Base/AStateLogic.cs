/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	EGameState
作    者:	HappLI
描    述:	游戏状态逻辑
*********************************************************************/
using Framework.BattlePlus;
using Framework.Core;
using Framework.Logic;

namespace TopGame.Logic
{
    public abstract class AStateLogic : Framework.Plugin.AT.IUserData
    {
        protected AState m_pState;
        //------------------------------------------------------
        public void Awake(AState state) { m_pState = state; OnAwake(); }
        protected virtual void OnAwake() { }
        //------------------------------------------------------
        public void Start() { OnStart(); }
        protected virtual void OnStart() { }
        //------------------------------------------------------
        public void PreStart() { OnPreStart(); }
        protected virtual void OnPreStart() { }
        //------------------------------------------------------
        public void Exit() { Clear(); OnExit(); }
        protected virtual void OnExit() { }
        //------------------------------------------------------
        public virtual void Enable(bool bEnable) { }
        //------------------------------------------------------
        public void Update(float fFrameTime) 
        {
            if (m_pState == null) return;
            InnerUpdate(fFrameTime);
        }
        protected virtual void InnerUpdate(float fFrameTime) { }
        //------------------------------------------------------
        public void Clear() 
        {
            OnClear();
        }
        protected virtual void OnClear() { }
        //------------------------------------------------------
        public T GetCurrentMode<T>() where T : AbsMode
        {
            if (m_pState == null) return null;
            return m_pState.GetCurrentMode<T>();
        }
        //------------------------------------------------------
        public T GetCurrentModeLogic<T>() where T : AModeLogic
        {
            if (m_pState == null) return null;
            return m_pState.GetCurrentModeLogic<T>();
        }
        //------------------------------------------------------
        public virtual void OnBattleWorldCallback(EBattleWorldCallbackType type, VariablePoolAble takeData = null)
        {

        }
        public virtual bool OnNetResponse(Net.PacketBase msg) { return false; }
        //------------------------------------------------------
        public Framework.Module.AFramework GetFramework()
        {
            if (m_pState != null) return m_pState.GetFramework();
            return null;
        }
        //------------------------------------------------------
        public World GetWorld()
        {
            if (m_pState != null) return m_pState.GetWorld();
            return null;
        }
        //------------------------------------------------------
        public SvrData.User GetUser()
        {
#if USE_SERVER
            if (m_pState == null) return null;
            return m_pState.GetUser();
#else
            if (m_pState != null) return SvrData.UserManager.MySelf;
            return m_pState.GetUser();
#endif
        }
        //------------------------------------------------------
        public BattleWorld GetBattleWorld()
        {
            if (m_pState != null) return m_pState.GetBattleWorld();
            return null;
        }
        //------------------------------------------------------
        public T GetBattleLogic<T>() where T : ABattleLogic
        {
            if (m_pState != null) return m_pState.GetBattleLogic<T>();
            return null;
        }
        //------------------------------------------------------
        public void EnableBattleLogic<T>(bool bEnable) where T : ABattleLogic
        {
            if (m_pState != null) m_pState.EnableBattleLogic<T>(bEnable);
        }
        //------------------------------------------------------
        public void Destroy() { }
        //------------------------------------------------------
#if UNITY_EDITOR
        public virtual void DrawGizmos() { }
        public virtual string Print() { return ""; }

#endif

    }
}
