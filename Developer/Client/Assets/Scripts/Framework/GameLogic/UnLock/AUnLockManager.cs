/********************************************************************
生成日期:	2020-6-16
类    名: 	UnLockManager
作    者:	zdq
描    述:	控制解锁功能的判断和解锁过渡状态的判断,管理所有挂载在功能按钮上的mono
*********************************************************************/

namespace TopGame.Logic
{
    public abstract class AUnLockManager : Framework.Module.AModule
    {
        protected bool m_bEnable = true;
        //------------------------------------------------------
        public bool IsEnable()
        {
            return m_bEnable;
        }
        //------------------------------------------------------
        public void SetEnable(bool enable)
        {
            if (!Core.DebugConfig.bEnableModuleLock)
            {
                return;
            }
            m_bEnable = enable;
        }
        //------------------------------------------------------
        protected override void Awake()
        {
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
        }
        //------------------------------------------------------
        public abstract void Init();
        //------------------------------------------------------
        public abstract void Clear();
        public abstract void OnChangeAccount();
        //------------------------------------------------------
        public abstract void RefreshState(UnLockListener listener);
        //------------------------------------------------------
        public abstract void PopUnlockTipPanel(UnLockListener listener);
        //------------------------------------------------------
        public abstract bool IsPlayingUnLockTipsTween();
        //------------------------------------------------------
        public abstract Framework.Core.IContextData GetUnlockData(uint id);
        public abstract void AddUnLockUIItem(UnLockListener listener);
        public abstract void RemoveUnlockListener(UnLockListener listener);
        public abstract bool IsLockAndTip(uint id, Framework.Core.IContextData cfg = null);
        public abstract bool IsLocked(uint id);
        public abstract bool IsUILockAndTip(uint uiType);
        public abstract bool IsUILocked(uint uiType);
        public abstract void SetUnLockState(UnLockListener listener, EModuleLockState state = EModuleLockState.Unlock);
        //------------------------------------------------------
        public virtual bool CanShowTweenEffect() { return true; }
#if UNITY_EDITOR
        //------------------------------------------------------
        public virtual void PrintDebug()
        {
        }
#endif
    }
}


