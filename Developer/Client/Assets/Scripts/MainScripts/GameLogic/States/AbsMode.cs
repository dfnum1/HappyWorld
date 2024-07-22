/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ILogic
作    者:	HappLI
描    述:	状态模式
*********************************************************************/

using Framework.Core;
using Framework.Logic;
using Framework.Plugin.AT;
using Framework.BattlePlus;
using System;
using System.Collections.Generic;
using Framework.Data;
using UnityEngine;

namespace TopGame.Logic
{
    [Framework.Plugin.AT.ATExportMono("游戏状态/模式")]
    public abstract class AbsMode : AAPICallFlag, IMode
    {
        protected AState m_pState;
        private bool m_bLocked = false;
        protected bool m_bEnabled = false;

        protected EMode m_pMode = EMode.None;
        private List<AModeLogic> m_vLogic = null;
        //------------------------------------------------------
        public EMode GetMode()
        {
            return m_pMode;
        }
        //------------------------------------------------------
        public void SetMode(EMode mode)
        {
            if (m_pMode != mode)
                resetAPICalled();
            m_pMode = mode;
        }
        //------------------------------------------------------
        public void BindState(AState pState)
        {
            if (m_pState == pState) return;
            m_pState = pState;
            OnStateChange();
        }
        //------------------------------------------------------
        protected virtual void OnStateChange()
        {
            if (m_vLogic != null)
            {
                for (int i = 0; i < m_vLogic.Count;)
                {
                    AModeLogic logic = m_vLogic[i];
                    if (logic == null || !CheckLogicRegister(logic.GetType()))
                    {
                        logic.Destroy();
                        m_vLogic.RemoveAt(i);
                    }
                    else
                    {
                        logic.StateChange(m_pState);
                        ++i;
                    }
                }
            }
        }
        //------------------------------------------------------
        public AState GetState()
        {
            return m_pState;
        }
        //------------------------------------------------------
        public void Enable(bool bEnable)
        {
            if (m_bEnabled == bEnable) return;
            m_bEnabled = bEnable;
            if (bEnable)
                GetFramework().RegisterFunction(this);
            else
                GetFramework().UnRegisterFunction(this);
            m_bLocked = false;
            OnEnabled(bEnable);
            if (m_vLogic != null)
            {
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    m_vLogic[i].Enable(bEnable);
                }
            }
        }
        protected virtual void OnEnabled(bool bEnable) { }
        //------------------------------------------------------
        public bool IsEnabled()
        {
            return m_bEnabled;
        }
        //------------------------------------------------------
        public void SetLocked(bool bLock)
        {
            m_bLocked = bLock;
        }
        //------------------------------------------------------
        public bool IsLocked()
        {
            return m_bLocked;
        }
        //------------------------------------------------------
        public void Awake()
        {
            if (m_bLocked) return;
            if (isAPICalled(EAPICallType.Awake)) return;
            m_bEnabled = true;
            resetAPICalled();
            setAPICalled(EAPICallType.Awake);

            if (m_vLogic != null)
            {
                AModeLogic logic;
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    logic = m_vLogic[i];
                    logic.Toggle(true, false);
                }
            }
            OnAwake();
            if (m_vLogic != null)
            {
                AModeLogic logic;
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    logic = m_vLogic[i];
                    if (logic.isToggled()) logic.Awake(this);
                }
            }
            m_bEnabled = false;
        }
        //------------------------------------------------------
        protected virtual void OnAwake() { }
        //------------------------------------------------------
        public void Start()
        {
            if (m_bLocked) return;
            if (isAPICalled(EAPICallType.Start)) return;
            setAPICalled(EAPICallType.Start);
            m_bEnabled = true;
            OnStart();
            if (m_vLogic != null)
            {
                AModeLogic logic;
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    logic = m_vLogic[i];
                    if (logic.isToggled()) logic.Start();
                }
            }
            m_bEnabled = false;
        }
        //------------------------------------------------------
        public void PreStart()
        {
            if (m_bLocked) return;
            if (isAPICalled(EAPICallType.PreStart)) return;
            setAPICalled(EAPICallType.PreStart);
            OnPreStart();
            if (m_vLogic != null)
            {
                AModeLogic logic;
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    logic = m_vLogic[i];
                    if (logic.isToggled()) logic.PreStart();
                }
            }
        }
        //------------------------------------------------------
        protected virtual void OnPreStart() { }
        protected virtual void OnStart() { }
        protected virtual void OnDestroy() { }

        //------------------------------------------------------
        public void Destroy()
        {
            if (m_bLocked) return;
            if (isAPICalled(EAPICallType.Destroy)) return;

            m_bEnabled = false;
            m_bLocked = false;
            if (m_vLogic != null)
            {
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    m_vLogic[i].Destroy();
                }
            }
            GetFramework().UnRegisterFunction(this);
            OnDestroy();
            m_pState = null;
            resetAPICalled();
            setAPICalled(EAPICallType.Destroy);
        }
        //------------------------------------------------------
        public void Update(float fFrame)
        {
            if (!m_bEnabled) return;
            OnUpdate(fFrame);
            if (m_vLogic == null) return;
            AModeLogic logic;
            for (int i = 0; i < m_vLogic.Count; ++i)
            {
                logic = m_vLogic[i];
                if (logic.isToggled()) logic.Update(fFrame);
            }
        }
        //------------------------------------------------------
        protected virtual void OnUpdate(float fFrame) { }
        //------------------------------------------------------
        public T RegisterLogic<T>() where T : AModeLogic, new()
        {
            Type classType = typeof(T);
            if (!CheckLogicRegister(classType))
            {
                if (m_vLogic != null)
                {
                    for (int i = 0; i < m_vLogic.Count;)
                    {
                        T temp = m_vLogic[i] as T;
                        if (temp != null)
                        {
                            temp.Destroy();
                            m_vLogic.RemoveAt(i);
                        }
                        else
                            ++i;
                    }
                }

                return null;
            }

            T mode = GetLogic<T>();
            if (mode != null) return mode;

            int hash = classType.GetHashCode();
            if (m_vLogic == null) m_vLogic = new List<AModeLogic>(8);
            else
            {
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    mode = m_vLogic[i] as T;
                    if (mode != null) return mode;
                }
            }

            mode = new T();
            RegisterLogic(mode);
            return mode;
        }
        //------------------------------------------------------
        public Framework.Module.AFramework GetFramework()
        {
            if (m_pState == null) return null;
            return m_pState.GetFramework();
        }
        //------------------------------------------------------
        public Framework.Plugin.AI.AISystem GetAISystem()
        {
            return m_pState != null ? m_pState.GetAISystem() : null;
        }
        //------------------------------------------------------
        public World GetWorld()
        {
            if (m_pState == null) return null;
            return m_pState.GetWorld();
        }
        //------------------------------------------------------
        public SvrData.User GetUser()
        {
#if USE_SERVER
            if (m_pState == null) return null;
            return m_pState.GetUser();
#else
            if (m_pState == null) return SvrData.UserManager.MySelf;
            return m_pState.GetUser();
#endif
        }
        //------------------------------------------------------
        public BattleWorld GetBattleWorld()
        {
            if (m_pState == null) return null;
            return m_pState.GetBattleWorld();
        }
        //------------------------------------------------------
        public T GetBattleLogic<T>() where T : ABattleLogic
        {
            if (m_pState == null) return null;
            return m_pState.GetBattleLogic<T>();
        }
        //------------------------------------------------------
        public void EnableBattleLogic<T>(bool bEnable) where T : ABattleLogic
        {
            if (m_pState == null) return;
            m_pState.EnableBattleLogic<T>(bEnable);
        }
        //------------------------------------------------------
        protected virtual bool CheckLogicRegister(Type logicType)
        {
            return true;
        }
        //------------------------------------------------------
        public virtual Wingman GetWingman(int index = 0)
        {
            return null;
        }
        //------------------------------------------------------
        public virtual Wingman GetArtifact(int index = 0)
        {
            return null;
        }
        //------------------------------------------------------
        public virtual List<Actor> GetPlayers(int index = 0)
        {
            return null;
        }
        //------------------------------------------------------
        public virtual Actor GetCurrentPlayer()
        {
            return null;
        }
        //------------------------------------------------------
        public virtual bool AddPlayer(uint configId, ushort level)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual Actor AddPlayer(byte attackGroup, VariablePoolAble contextData, bool bCheckExist = true)
        {
            return null;
        }
        //------------------------------------------------------
        public void Preload(string strFile)
        {
            if (m_pState == null) return;
            m_pState.Preload(strFile);
        }
        //------------------------------------------------------
        public void PreloadInstance(string strFile, int insCnt = 1)
        {
            if (m_pState == null) return;
            m_pState.PreloadInstance(strFile, insCnt);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public virtual void RefreshPlayers(int group = 0)
        {
            if (m_vLogic != null)
            {
                AModeLogic logic = null;
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    logic = m_vLogic[i];
                    if (logic.isToggled()) logic.OnRefreshPlayers(group);
                }
            }
        }
        //------------------------------------------------------
        public virtual bool OnFillPassData(PassCondition passCondition, VariablePoolAble userData)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual VariablePoolAble GetUserData()
        {
            return null;
        }
        //------------------------------------------------------
        public virtual uint GetSceneTheme()
        {
#if USE_SERVER
            return 0;
#else
            Core.ASceneTheme themer = Core.SceneMgr.GetThemer();
            if (themer == null) return 0;
            return (uint)themer.GetCurTheme();
#endif
        }
        //------------------------------------------------------
        public virtual bool OnNetResponse(TopGame.Net.PacketBase msg)
        {
            int nDeal = 0;
            if (m_vLogic != null)
            {
                AModeLogic logic = null;
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    logic = m_vLogic[i];
                    if (logic.isToggled() && logic.OnNetResponse(msg)) nDeal++;
                }
            }
            return nDeal > 0;
        }
        //------------------------------------------------------
        public virtual void OnBattleWorldCallback(BattleWorld pWorld, EBattleWorldCallbackType type, VariablePoolAble takeData = null)
        {
            if (m_vLogic != null)
            {
                AModeLogic logic = null;
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    logic = m_vLogic[i];
                    if(logic.isToggled()) logic.OnBattleWorldCallback(pWorld,type, takeData);
                }
            }
        }
        //------------------------------------------------------
        public virtual void OnBattleResultCheckEvent(ref EBattleResultStatus current)
        {
            if (m_vLogic != null)
            {
                AModeLogic logic = null;
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    logic = m_vLogic[i];
                    if (logic.isToggled() && logic.OnBattleResultCheckEvent(ref current))
                        break;
                }
            }
        }
        //------------------------------------------------------
        public virtual void OnActorAttrChange(Actor pActor, EAttrType type, float fValue, float fOriginal, bool bPlus)
        {
            if (m_vLogic != null)
            {
                AModeLogic logic = null;
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    logic = m_vLogic[i];
                    if (logic.isToggled())
                        logic.OnActorAttrChange(pActor, type, fValue, fOriginal, bPlus);
                }
            }
        }
        //------------------------------------------------------
        public virtual bool OnLockHitTargetCondition(AWorldNode pTrigger, AWorldNode pTarget, ELockHitType hitType, ELockHitCondition condition, Vector3Int conditionParam)
        {
            return true;
        }
        //------------------------------------------------------
        public void RegisterLogic(AModeLogic logic)
        {
            if (logic == null) return;
            if (m_vLogic == null) m_vLogic = new List<AModeLogic>(8);
            if (m_vLogic.Contains(logic)) return;
            m_vLogic.Add(logic);
            logic.Toggle(true, false);
            if (isAPICalled(EAPICallType.Awake)) logic.Awake(this);
            if (isAPICalled(EAPICallType.PreStart)) logic.PreStart();
            if (isAPICalled(EAPICallType.Start)) logic.Start();
        }
        //------------------------------------------------------
        public void UnRegisterLogic(AModeLogic logic)
        {
            if (m_vLogic == null || logic == null) return;
            logic.Destroy();
            m_vLogic.Remove(logic);
        }
        //------------------------------------------------------
        public T GetLogic<T>() where T : AModeLogic
        {
            AModeLogic logic = null;
            if (m_vLogic == null) return logic as T;
            for(int i = 0;i < m_vLogic.Count; ++i)
            {
                logic = m_vLogic[i] as T;
                if (logic != null) return logic as T;
            }
            return logic as T;
        }
        //------------------------------------------------------
        public void ToggleLogic<T>(bool bEnable) where T : AModeLogic
        {
            if (m_vLogic == null) return;
            AModeLogic logic = null;
            for (int i = 0; i < m_vLogic.Count; ++i)
            {
                logic = m_vLogic[i] as T;
                if (logic != null)
                {
                    logic.Toggle(bEnable);
                }
            }
        }
        //------------------------------------------------------
        public int GetLogicCount()
        {
            if (m_vLogic == null) return 0;
            return m_vLogic.Count;
        }
        //------------------------------------------------------
        public AModeLogic GetLogic(int index)
        {
            if (m_vLogic == null || index<0 || index >= m_vLogic.Count) return null;
            return m_vLogic[index];
        }
        //------------------------------------------------------
        public bool ExecuteCustom(ushort enterType, IUserData pParam = null)
        {
            if (m_pState == null) return false;
            return m_pState.ExecuteCustom(enterType, pParam);
        }
        //------------------------------------------------------
        public bool ExecuteCustom(UnityEngine.GameObject pGo, IUserData pParam = null)
        {
            if (m_pState == null) return false;
            return m_pState.ExecuteCustom(pGo, pParam);
        }
        //------------------------------------------------------
        public bool ExecuteCustom(string strName, IUserData pParam = null)
        {
            if (m_pState == null) return false;
            return m_pState.ExecuteCustom(strName, pParam);
        }
        //------------------------------------------------------
        public bool ExecuteCustom(int nID, IUserData pParam = null)
        {
            if (m_pState == null) return false;
            return m_pState.ExecuteCustom(nID, pParam);
        }
        //------------------------------------------------------
        public virtual void DrawGizmos()
        {
            if (m_vLogic != null)
            {
                AModeLogic logic = null;
                for (int i = 0; i < m_vLogic.Count; ++i)
                {
                    logic = m_vLogic[i];
                    if(logic!=null && logic.isToggled()) logic.DrawGizmos();
                }
            }
        }
        //------------------------------------------------------
#if UNITY_EDITOR
        public virtual string Print() { return this.GetType().ToString()+":\r\n"; }
#endif
    }
}
