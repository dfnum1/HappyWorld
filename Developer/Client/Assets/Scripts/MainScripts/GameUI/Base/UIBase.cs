/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIBase
作    者:	HappLI
描    述:	UI操作类
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using TopGame.UI;
using UnityEngine;
using Framework.Module;
using System;
using Framework.Core;
using TopGame.SvrData;

namespace TopGame.UI
{
    [Framework.Plugin.AT.ATExportMono("UI系统/界面")]
    public abstract class UIBase : Framework.UI.UIHandle
    {
        public static System.Action<UIBase> OnGlobalAwakeUI;
        public static System.Action<UIBase> OnGlobalShowUI;
        public static System.Action<UIBase> OnGlobalHideUI;
        public static System.Action<UIBase> OnGlobalDestroyUI;
        public static System.Action<UIBase> OnGlobalMoveOutUI;
        public static System.Action<UIBase> OnGlobalMoveInUI;

        protected Framework.Core.AInstanceAble m_pAble = null;
        protected UISerialized m_UI = null;
        protected Canvas m_Canvas = null;
        protected RectTransform m_RootRect = null;
        protected UIRecord m_UIRecord = null;

        protected bool m_bVisible = false;
        protected bool m_bActive = false;
        protected bool m_bPermanent = false;
        protected bool m_bAlwayShow = false;
        private bool m_bDestroyed = false;
        protected bool m_bTackAble = false;
        protected byte m_nBackupAble = 0;

        protected float m_fDelayShow = 0;
        protected float m_fAutoHide = 0;

        private int m_nMoveCount = 0;

        protected bool m_bFullUI = false;
        protected int m_nOrder = 0;
        protected int m_nZValue = 0;
        protected ushort m_nUIType = 0;

        protected float m_openTime = 0;
        protected bool m_showLog = false;
        protected UIView m_pView = null;
        public System.Action<UIBase> OnCloseUI;
        public System.Action<UIBase> OnHideUI;
        [Framework.Plugin.AT.ATField("获取界面视图")]
        public UIView view
        {
            get { return m_pView; }
        }

        public Dictionary<int, UILogic> m_vLogic;

        public UISerialized ui { get { return m_UI; } }

        protected Framework.Plugin.AT.AgentTree m_pAT = null;
        public Framework.Plugin.AT.AgentTree uiAT
        {
            get { return m_pAT; }
        }

        Transform m_pParent = null;
        public System.Action m_pUIPendingAction;

        private Vector3 m_vMoveBeforePos;
        private float m_fTimer = 0;

        private Dictionary<string, Framework.Core.VariablePoolAble> m_vUserDatas = null;

#if UNITY_EDITOR
        private Framework.Base.ProfilerTicker m_pTimerLastTick = new Framework.Base.ProfilerTicker();
        public void SartProfiler()
        {
            m_pTimerLastTick.Start(this.GetType().Name);
        }
        //------------------------------------------------------
        public void EndProfiler()
        {
            m_pTimerLastTick.Stop();
        }
#endif
        //------------------------------------------------------
        public virtual void Awake()
        {
            m_bDestroyed = false;
            OnAwake();
            if (m_pView != null) m_pView.Awake();
            if(m_vLogic!=null)
            {
                foreach (var db in m_vLogic)
                    db.Value.Awake(this);
            }

            if (m_UI != null)
            {
                UserInterface ui = m_UI as UserInterface;
                if (ui)
                    m_pAT = Framework.Plugin.AT.AgentTreeManager.getInstance().LoadAT(ui.ATData);
                if(m_pAT!=null)
                {
                    m_pAT.AddOwnerClass(this);
                    if(m_pView!=null) m_pAT.AddOwnerClass(m_pView);
                    if (m_vLogic != null)
                    {
                        foreach (var db in m_vLogic)
                            m_pAT.AddOwnerClass(db.Value);
                    }
                    if (m_bVisible)
                    {
                        m_pAT.Enable(true);
                        m_pAT.Enter();
                    }
                }
            }
            if (OnGlobalAwakeUI != null) OnGlobalAwakeUI(this);

            if(m_bVisible)
            {
                OnFrameShow();
            }
        }
        //------------------------------------------------------
        protected virtual void OnAwake() { }
        //------------------------------------------------------
        internal int GeOrder()
        {
            return m_nOrder;
        }
        //------------------------------------------------------
        public GameObject GetRoot()
        {
            if (m_Canvas == null || m_Canvas.gameObject == null) return null;
            return m_Canvas.gameObject;
        }
        //------------------------------------------------------
        public void SetUIType(ushort uiType)
        {
            m_nUIType = uiType;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("界面类型ID")]
        public ushort GetUIType()
        {
            return m_nUIType;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("层级")]
        public int GetOrder()
        {
            return m_nOrder;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("设置层级")]
        public void SetOrder(int order)
        {
            m_nOrder = order;
            if(m_Canvas != null)
            {
                m_Canvas.overrideSorting = true;
                if (m_Canvas.sortingOrder != order)
                {
                    m_Canvas.sortingOrder = order;
                }
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("设置Z轴深度")]
        public void SetZDeepth(int zValue)
        {
            m_nZValue = zValue;
            if (m_Canvas != null&& zValue != 0)
            {
                Vector3 pos = m_RootRect.anchoredPosition3D;
                m_RootRect.anchoredPosition3D = new Vector3(pos.x,pos.y,zValue);
            }
        }
        //------------------------------------------------------
        public void SetFullUI(bool bFull)
        {
            m_bFullUI = bFull;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("设置为常驻")]
        public void SetPermanent(bool bPermanent)
        {
            m_bPermanent = bPermanent;
        }
        
        //------------------------------------------------------
        public void SetAlwayShow(bool bAlwayShow)
        {
            m_bAlwayShow = bAlwayShow;
        }
        //------------------------------------------------------
        public bool GetAlwayShow()
        {
            return m_bAlwayShow;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("是否常驻")]
        public bool GetPermanent()
        {
            return m_bPermanent;
        }
        //------------------------------------------------------
        public void SetParent(Transform pTrans)
        {
            if(m_pParent != pTrans)
            {
                if (m_pAble != null)
                {
                    m_pParent = pTrans;
                    RefreshInstanceAble();
                }
                else
                    m_pParent = pTrans;
            }
        }
        //------------------------------------------------------
        public void SetView(UIView pView)
        {
            m_pView = pView;
            if (m_pView != null)
                m_pView.Init(this);
        }
        //------------------------------------------------------
        public void ExecuteAT(Base.EUIEventType type, int nCustomID, Framework.Plugin.AT.IUserData paramData = null)
        {
            if (m_pAT == null) return;
            m_pAT.ExecuteEvent((ushort)type, nCustomID,paramData);
        }
        //------------------------------------------------------
        public void ExecuteAT(Base.EUIEventType type, string strCustom, Framework.Plugin.AT.IUserData paramData = null)
        {
            if (m_pAT == null) return;
            m_pAT.ExecuteEvent((ushort)type, strCustom,paramData);
        }
        //------------------------------------------------------
        public void ExecuteAT(Base.EUIEventType type, GameObject pObj, Framework.Plugin.AT.IUserData paramData = null)
        {
            if (m_pAT == null) return;
            m_pAT.ExecuteEvent((ushort)type, pObj, paramData);
        }
        //------------------------------------------------------
        public void ExecuteCustom(GameObject pGo, Framework.Plugin.AT.IUserData paramData = null)
        {
            if (m_pAT == null) return;
            m_pAT.ExecuteCustom(pGo, paramData);
        }
        //------------------------------------------------------
        public void ExecuteCustom(string strName, Framework.Plugin.AT.IUserData paramData = null)
        {
            if (m_pAT == null) return;
            m_pAT.ExecuteCustom(strName, paramData);
        }
        //------------------------------------------------------
        public void ExecuteCustom(int nID, Framework.Plugin.AT.IUserData paramData = null)
        {
            if (m_pAT == null) return;
            m_pAT.ExecuteCustom(nID, paramData);
        }
        //------------------------------------------------------
        public void ExecuteEvent(Base.EUIEventType enterType, Framework.Plugin.AT.IUserData paramData = null)
        {
            if (m_pAT == null) return;
            m_pAT.ExecuteCustom((ushort)enterType, paramData);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取UI逻辑脚本")]
        public UILogic FindLogic(System.Type type)
        {
            if (m_vLogic == null) return null;
            if (type == null) return null;
            int hashCode = type.GetHashCode();

            UILogic logic = null;
            if (m_vLogic.TryGetValue(hashCode, out logic))
                return logic;
            return null;
        }
        //------------------------------------------------------
        public T GetLogic<T>(bool bAutoNew = false) where T : UILogic, new()
        {
            int hashCode = typeof(T).GetHashCode();
            UILogic logic = null;
            if (m_vLogic!=null && m_vLogic.TryGetValue(hashCode, out logic))
                return logic as T;
            if(bAutoNew)
                return AddLogic<T>(hashCode);
            return null;
        }
        //------------------------------------------------------
        public void AddLogic(UILogic pLogic, int hashCode = 0)
        {
            if (hashCode == 0) hashCode = pLogic.GetType().GetHashCode();
            if (m_vLogic == null) m_vLogic = new Dictionary<int, UILogic>(4);
            if (m_vLogic.ContainsKey(hashCode))
                return;

            pLogic.OnInit(this);
            pLogic.Active(true);
            m_vLogic[hashCode] = pLogic;
        }
        //------------------------------------------------------
        public T AddLogic<T>(int hashCode = 0) where T : UILogic, new()
        {
            if(hashCode == 0) hashCode = typeof(T).GetHashCode();
            if (m_vLogic == null) m_vLogic = new Dictionary<int, UILogic>(4);
            UILogic logic = null;
            if (m_vLogic.TryGetValue(hashCode, out logic))
                return (T)logic;

            T newT = new T();
            newT.OnInit(this);
            newT.Active(true);
            if (m_pAble != null) newT.Awake(this);
            if (IsVisible())
            {
                newT.OnShow();
            }
            m_vLogic[hashCode] = newT;
            return newT;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("移除UI逻辑脚本")]
        public void RemoveLogic(UILogic pLogic, int hashCode = 0)
        {
            if (m_vLogic == null) return;
            if (hashCode == 0) hashCode = pLogic.GetType().GetHashCode();
            m_vLogic.Remove(hashCode);
            pLogic.Destroy();
        }
        //------------------------------------------------------
        public void RemoveLogic<T>(int hashCode = 0) where T : UILogic
        {
            if (m_vLogic == null) return;
            if(hashCode ==0 ) hashCode = typeof(T).GetHashCode();
            UILogic logic = null;
            if(m_vLogic.TryGetValue(hashCode, out logic))
            {
                logic.Destroy();
                m_vLogic.Remove(hashCode);
            }
        }
        //------------------------------------------------------
        public void Show(float fDelta)
        {
            if (m_bVisible) return;
            m_fDelayShow = 0;
            if (fDelta <=0)
            {
                Show();
                return;
            }
            m_bVisible = true;
            m_fDelayShow = fDelta;
        }
        //------------------------------------------------------
        void OnFrameShow()
        {
            if (!m_bVisible) return;
            m_fDelayShow = 0;
            //Debug.Log("显示UI:" + (EUIType)m_nUIType);
            GameInstance.getInstance().RegisterFunction(this);
            if (m_bVisible && m_pAble)
            {
                m_pAble.SetPosition(Vector3.zero, true);
            }
            if (m_pAT != null)
            {
                m_pAT.Enable(true);
                m_pAT.Enter();
            }
            
            SetZDeepth(m_nZValue);
            if (UIManager.GetInstance().IsLandscape())
            {
                SetLandscape();
            }
            else
            {
                SetPortrait();
            }
            if (m_pView != null) m_pView.Show();
            DoShow();

            if (m_UI != null && m_pAble)
            {
                if(m_UI.OnShow())
                {
                    return;
                }
            }
            DoShowEnd();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("显示")]
        public override void Show()
        {
            if (m_bVisible) return;
            m_bVisible = true;

            if (m_pAble == null) return;
            OnFrameShow();
            m_openTime = Time.time;
            if (m_showLog)
            {
                TopGame.Core.AUserActionManager.AddActionKV("ui_name", ((EUIType)m_nUIType).ToString());
                AddEventUtil.LogEvent("showui",true);
            }
        }
        //------------------------------------------------------
        protected virtual void DoShow()
        {
//             if (m_bFullUI)
//                 GameInstance.getInstance().cameraController.CloseCameraRef(true);
            if (m_vLogic != null)
            {
                foreach (var db in m_vLogic)
                {
                    if(GameInstance.getInstance()!=null) GameInstance.getInstance().RegisterFunction(db.Value);
                    db.Value.OnShow();
                }
            }
        }
        //------------------------------------------------------
        protected virtual void DoShowEnd()
        {
            ExecuteEvent(Base.EUIEventType.OnShowUIEnd, this);
            if (m_bFullUI)
                GameInstance.getInstance().cameraController.CloseCameraRef(true);
            if (m_vLogic != null)
            {
                foreach (var db in m_vLogic)
                    db.Value.OnShowEnd();
            }

            if (OnGlobalShowUI != null) OnGlobalShowUI(this);
        }
        //------------------------------------------------------
        protected virtual void OnMoveInside()
        {

        }
        //------------------------------------------------------
        protected virtual void OnMoveOutside()
        {

        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("关闭")]
        public override bool Close()
        {
            if (m_bPermanent)
            {
                Hide();
                return false;
            }
            Hide();
            UIEventLogic eventLogic = GetLogic<UIEventLogic>();
            if (eventLogic != null && eventLogic.IsEvnting())
            {
                m_bDestroyed = true;
                return true;
                //DoHide();
            }
            GameInstance.getInstance().UnRegisterFunction(this);
            DoClose();
            return true;
        }
        //------------------------------------------------------
        protected virtual void DoClose()
        {
            Destroy();
            if (m_vLogic != null)
            {
                foreach (var db in m_vLogic)
                    db.Value.OnClose();
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("移动屏幕外"),Framework.Plugin.AT.ATExportGUID(-1949292123)]
        public void MoveOutside()
        {
            if (m_bAlwayShow)
            {
                return;
            }
            if (m_nMoveCount == 0) m_vMoveBeforePos = m_pAble.transform.localPosition;
            if (m_nMoveCount == 0 && m_pAble)
            {
                if (OnGlobalMoveOutUI != null) OnGlobalMoveOutUI(this);
                bool bTween = false;
                if (m_UI != null)
                {
                    bTween = m_UI.OnMoveOut();
                }
                if(!bTween) m_pAble.SetPosition(Vector3.one * 8888, true);

                if (m_bFullUI)
                    GameInstance.getInstance().cameraController.CloseCameraRef(false);

                Framework.Plugin.Guide.GuideWrapper.OnCloseUI(m_nUIType, 1 << (int)GameInstance.getInstance().GetState());
                OnMoveOutside();
            }
            m_nMoveCount++;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("移进屏幕内"), Framework.Plugin.AT.ATExportGUID(-787964043)]
        public void MoveInside()
        {
            if (m_bAlwayShow)
            {
                return;
            }
            m_nMoveCount--;
            if (m_nMoveCount == 0 && m_pAble)
            {
                if (OnGlobalMoveInUI != null) OnGlobalMoveInUI(this);
                m_pAble.SetPosition(m_vMoveBeforePos, true);
                bool bTween = false;
                if (m_UI != null)
                {
                    bTween = m_UI.OnMoveIn();
                }

                if (m_bFullUI)
                    GameInstance.getInstance().cameraController.CloseCameraRef(true);

                Framework.Plugin.Guide.GuideWrapper.OnOpenUI(m_nUIType, 1 << (int)GameInstance.getInstance().GetState());
                OnMoveInside();
            }
        }
        //------------------------------------------------------
        public bool CanTrack()
        {
            return m_bTackAble;
        }
        //------------------------------------------------------
        public void SetTrackAble(bool able)
        {
            m_bTackAble = able;
        }
        //------------------------------------------------------
        public byte GetBackupFlags()
        {
            return m_nBackupAble;
        }
        //------------------------------------------------------
        public bool CanBackup()
        {
            return (m_nBackupAble&(int)EUIBackupFlag.Toggle)!=0;
        }
        //------------------------------------------------------
        public bool IsMoveOutSideBackup()
        {
            return (m_nBackupAble & (int)EUIBackupFlag.MoveOutside) != 0;
        }
        //------------------------------------------------------
        public bool IsBackupAllShowed()
        {
            return (m_nBackupAble & (int)EUIBackupFlag.BackupAllShow) != 0;
        }
        //------------------------------------------------------
        public bool CanInheritCommonBackup()
        {
            return (m_nBackupAble & (int)EUIBackupFlag.InheritCommon) != 0;
        }
        //------------------------------------------------------
        public void SetBackupAble(byte able)
        {
            m_nBackupAble = able;
        }
        //------------------------------------------------------
        public virtual bool CanHide()
        {
            return true;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("隐藏")]
        public override void Hide()
        {
            m_fDelayShow = 0;
            m_fAutoHide = 0;
            if (m_bAlwayShow) return;
            if (!m_bVisible) return;
            m_bVisible = false;
            m_nMoveCount = 0;
            if (OnGlobalHideUI != null) OnGlobalHideUI(this);
            if (m_UI != null && m_pAble)
            {
                if(m_UI.OnHide())
                {
                    if (m_showLog)
                    {
                        TopGame.Core.AUserActionManager.AddActionKV("ui_name", ((EUIType)m_nUIType).ToString());
                        TopGame.Core.AUserActionManager.AddActionKV("stay_time", Time.time - m_openTime);
                        AddEventUtil.LogEvent("closeui",true);
                    }
                    return;
                }
            }
            GameInstance.getInstance()?.UnRegisterFunction(this);
            DoHide();
            if (m_showLog)
            {
                TopGame.Core.AUserActionManager.AddActionKV("ui_name", ((EUIType)m_nUIType).ToString());
                TopGame.Core.AUserActionManager.AddActionKV("stay_time", Time.time - m_openTime);
                AddEventUtil.LogEvent("closeui",true);
            }
        }
        //------------------------------------------------------
        public void AutoHide(float fDelay)
        {
            m_fAutoHide = fDelay;
        }
        //------------------------------------------------------
        protected virtual void DoHide()
        {
            bool bHideEvent = false;

            if (!bHideEvent)
            {
                if (m_pAble) m_pAble.SetPosition(Vector3.one * 9999, true);
            }
            if (m_pView != null) m_pView.Hide();
            if (m_pView != null)
                m_pView.Clear(m_bPermanent);

            OnHideUI?.Invoke(this);
            OnHideUI = null;
            if (m_bFullUI)
                Core.CameraKit.CloseCameraRef(false);

            if (m_vLogic != null)
            {
                foreach (var db in m_vLogic)
                {
                    if (GameInstance.getInstance() != null) GameInstance.getInstance().UnRegisterFunction(db.Value);
                    db.Value.OnHide();
                }
            }

            if (m_pAT != null)
            {
                m_pAT.Exit();
                if(m_pAT!=null) m_pAT.Enable(false);
            }

            Core.RenderHudManager.getInstance().OnClearCallback(this);
            if (m_vUserDatas != null) m_vUserDatas.Clear();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("是否显示")]
        public override bool IsVisible()
        {
            return m_bVisible;
        }
        //------------------------------------------------------
        public override bool IsMoveOut()
        {
            return m_nMoveCount > 0;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("是否可以中断引导")]
        public bool CanBreakGuide()
        {
            return true;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取一个对象")]
        public GameObject FindOject(string strName)
        {
            if (m_UI == null) return null;
            return m_UI.Find(strName);
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            if (GameInstance.getInstance() == null || UserManager.Current == null)
            {
                return;
            }
            if (m_bFullUI && m_bVisible)
                Core.CameraKit.CloseCameraRef(false);
            Clear();
            if (m_pView != null)
            {
                m_pView.Destroy();
            }
            if (m_vLogic != null)
            {
                foreach (var db in m_vLogic)
                    db.Value.OnDestroy();
                m_vLogic.Clear();
            }
            if (m_pAble != null)
                m_pAble.RecyleDestroy(1);
            m_pAble = null;
            m_UI = null;
            m_bDestroyed = true;
            m_pParent = null;
            m_pView = null;
            if (m_pAT != null)
                Framework.Plugin.AT.AgentTreeManager.getInstance().UnloadAT(m_pAT);
            m_pAT = null;

            OnCloseUI?.Invoke(this);
            OnCloseUI = null;
            if (OnGlobalDestroyUI != null) OnGlobalDestroyUI(this);
            m_bVisible = false;
            GameInstance.getInstance().UnRegisterFunction(this);
            Core.RenderHudManager.getInstance().OnClearCallback(this);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("清理")]
        public override void Clear()
        {
            m_fDelayShow = 0;
            m_fAutoHide = 0;
            m_fTimer = 0;
            if (m_pView != null) m_pView.Clear();
            if (m_vLogic != null)
            {
                foreach (var db in m_vLogic)
                    db.Value.OnClear();
            }
            Core.RenderHudManager.getInstance().OnClearCallback(this);
            if (m_vUserDatas != null) m_vUserDatas.Clear();
        }
        //------------------------------------------------------
        public bool IsInstanced()
        {
            return m_pAble != null;
        }
        //------------------------------------------------------
        public Framework.Core.AInstanceAble GetInstanceAble()
        {
            return m_pAble;
        }
        //------------------------------------------------------
        void SetInstanceAble(AInstanceAble pAble)
        {
            m_pAble = pAble;
            if (m_pAble != null)
            {
                m_UI = m_pAble.GetComponent<UISerialized>();
                if (m_UI == null)
                {
                    Framework.Plugin.Logger.Error(((EUIType)m_nUIType).ToString() + ":" + m_pAble.name +  " ui null");
                }
                if (m_UI is UserInterface)
                {
                    UIEventLogic logic = new UIEventLogic();
                    m_UI.SetEventLogic(logic);
                    AddLogic(logic);
                }

                RefreshInstanceAble();
            }
            m_bActive = m_pAble != null;
            Awake();
#if UNITY_EDITOR
            EndProfiler();
#endif
        }
        //------------------------------------------------------
        void RefreshInstanceAble()
        {
            if (m_pAble == null) return;
            if(m_Canvas == null)
                m_Canvas = m_pAble.GetComponent<Canvas>();
            if (m_Canvas )
            {
                if (m_RootRect == null)
                    m_RootRect = m_Canvas.GetComponent<RectTransform>();
            }
            if (m_UIRecord == null)
            {
                m_UIRecord= m_Canvas.GetComponent<UIRecord>();
            }
            RectTransform rect = m_pAble.GetTransorm() as RectTransform;

            if (m_pParent != null)
                m_pAble.SetParent(m_pParent);
            if (rect == null) return;
            rect.localScale = Vector3.one;
            rect.anchoredPosition3D = Vector2.zero;
            rect.offsetMax = Vector3.zero;
            rect.offsetMin = Vector3.zero;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;

            if (m_bVisible)
            {
                m_pAble.SetPosition(Vector3.zero, true);
            }
            else
                m_pAble.SetPosition(Framework.Core.CommonUtility.INVAILD_POS, true);

            SetOrder(m_nOrder);
        }
        //------------------------------------------------------
        public override void OnEventHandle(VariablePoolAble param)
        {
            if(param !=null && param is Variable1)
            {
                UIEventType type = (UIEventType)((Variable1)param).intVal;
                switch(type)
                {
                    case UIEventType.Hide:
                        {
                            DoHide();
                            if (m_bDestroyed) DoClose();
                        }
                        break;
                    case UIEventType.Show:
                        {
                            DoShowEnd();
                        }
                        break;
                    case UIEventType.MoveOut:
                        {
                            m_pAble.SetPosition(Vector3.one * 8888, true);
                        }
                        break;
                    case UIEventType.MoveIn:
                        {
                            m_pAble.SetPosition(m_vMoveBeforePos, true);
                        }
                        break;
                }
            }

        }
        //------------------------------------------------------
        public void OnLoaded(InstanceOperiaon pOp)
        {
            UIManager.RemoveLoadingInstance(m_nUIType);
            if (pOp == null) return;
            m_nUIType = (ushort)pOp.GetUserData<Variable1>(0).intVal;
            SetInstanceAble(pOp.GetAble());
#if UNITY_EDITOR
            if (m_UI == null) return;      
            m_UI.PrefabAsset = pOp.GetFile();
            if (m_UI is UserInterface) ((UserInterface)m_UI).uiType = m_nUIType;
#endif
        }
        //------------------------------------------------------
        public void OnSign(InstanceOperiaon pOp)
        {
            pOp.SetUsed(!m_bDestroyed);
            if (!pOp.IsUsed()) UIManager.RemoveLoadingInstance(m_nUIType);
        }
        //------------------------------------------------------
        public override void Update(float fFrame)
        {
            if (!m_bActive || !m_bVisible) return;

            if (m_fDelayShow > 0)
            {
                if (m_pAble)
                {
                    m_fDelayShow -= Time.deltaTime;
                    if (m_fDelayShow <= 0)
                    {
                        m_bVisible = false;
                        Show();
                    }
                }
                return;
            }
            if(m_bVisible)
            {
                if(m_fAutoHide>0)
                {
                    m_fAutoHide -= Time.deltaTime;
                    if (m_fAutoHide<=0)
                    {
                        Hide();
                    }
                }
            }

            InnerUpdate(fFrame);
            if (m_pView != null) m_pView.Update(fFrame);
            if (m_vLogic != null)
            {
                foreach (var db in m_vLogic)
                    db.Value.Update(fFrame);
            }

            if (Time.realtimeSinceStartup - m_fTimer > 1)
            {
                SecondUpdate();
                if (m_vLogic != null)
                {
                    foreach (var db in m_vLogic)
                        db.Value.SecondUpdate();
                }

                m_fTimer = Time.realtimeSinceStartup;
            }
        }
        //------------------------------------------------------
        protected virtual void InnerUpdate(float fFrame)
        {

        }
        //------------------------------------------------------
        protected virtual void SecondUpdate()
        {

        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("资源加载设置")]
        public AssetOperiaon LoadObjectAsset(UnityEngine.Object pObj, string strPath, bool bPermanent = false, bool bAysnc = false, Sprite defaultSprite = null)
        {
            if (m_pView == null) return null;
            return m_pView.LoadObjectAsset(pObj, strPath, bPermanent, bAysnc, defaultSprite);
        }
        //------------------------------------------------------
        public InstanceOperiaon LoadInstance(string strFile, Transform pParent, bool bAsync = true, Action<InstanceOperiaon> OnCallback = null, Framework.Plugin.AT.IUserData userPtr = null)
        {
            if (m_pView == null) return null;
            return m_pView.LoadInstance(strFile, pParent, bAsync, OnCallback, userPtr);
        }
        //------------------------------------------------------
        public InstanceOperiaon LoadInstance(GameObject prefab, Transform pParent, bool bAsync = true, Action<InstanceOperiaon> OnCallback = null, Framework.Plugin.AT.IUserData userPtr = null)
        {
            if (m_pView == null) return null;
            return m_pView.LoadInstance(prefab, pParent, bAsync, OnCallback, userPtr);
        }
        //------------------------------------------------------
        public T GetWidget<T>(string name) where T : Component
        {
            if (m_UI == null) return null;
            return m_UI.GetWidget<T>(name);
        }
        //------------------------------------------------------
        /// <summary>
        /// 设置横屏
        /// </summary>
        public void SetLandscape()
        {
            if (m_UIRecord)
            {
                m_UIRecord.OnLandscape();
            }
        }
        //------------------------------------------------------
        /// <summary>
        /// 设置竖屏
        /// </summary>
        public void SetPortrait()
        {
            if (m_UIRecord)
            {
                m_UIRecord.OnPortrait();
            }
        }
        
        public void SetShowLog(bool isShow)
        {
            m_showLog = isShow;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public override void AddUserParam(string key, VariablePoolAble param)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (m_vUserDatas == null) m_vUserDatas = new Dictionary<string, VariablePoolAble>(2);
            m_vUserDatas[key.ToLower()] = param;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public void AddUserIntParam(string key, int param)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (m_vUserDatas == null) m_vUserDatas = new Dictionary<string, VariablePoolAble>(2);
            m_vUserDatas[key.ToLower()] = new Variable1() { intVal = param };
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public void AddUserStrParam(string key, string param)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (m_vUserDatas == null) m_vUserDatas = new Dictionary<string, VariablePoolAble>(2);
            m_vUserDatas[key.ToLower()] = new VariableString() { strValue = param };
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public override VariablePoolAble GetUserParam(string key)
        {
            if (string.IsNullOrEmpty(key) || m_vUserDatas == null) return null;
            VariablePoolAble result;
            if (m_vUserDatas.TryGetValue(key.ToLower(), out result)) return result;
            return null;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public int GetUserIntParam(string key)
        {
            if (string.IsNullOrEmpty(key) || m_vUserDatas == null) return 0;
            VariablePoolAble result;
            if (m_vUserDatas.TryGetValue(key.ToLower(), out result))
            {
                if(result is Variable1)
                    return ((Variable1)result).intVal;
                else if(result is VariableString)
                {
                    int temp;
                    if (int.TryParse(((VariableString)result).strValue, out temp))
                        return temp;
                }
            }
            return 0;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public string GetUserStrParam(string key)
        {
            if (string.IsNullOrEmpty(key) || m_vUserDatas == null) return null;
            VariablePoolAble result;
            if (m_vUserDatas.TryGetValue(key.ToLower(), out result) && result is VariableString) return ((VariableString)result).strValue;
            return null;
        }
    }
}
