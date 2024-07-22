/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIManager
作    者:	HappLI
描    述:	UI管理器
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.Module;
using UnityEngine.EventSystems;
using TopGame.Core;
using Framework.Plugin.AT;
using TopGame.SvrData;
using System.Linq;
using UnityEngine.Rendering.Universal;
using System;
using Framework.UI;
using Framework.Core;

namespace TopGame.UI
{
    public interface IBuffCallBack : Framework.Plugin.AT.IUserData
    {
        //------------------------------------------------------
        void OnBeginBuff(Actor pActor, BufferState buff);
        //------------------------------------------------------
        void OnEndBuff(Actor pActor, BufferState buff);
    }

    public interface IUICallback
    {
        void OnUIAwake(UIBase pBase);
        void OnUIShow(UIBase pBase);
        void OnUIHide(UIBase pBase);
    }

    [Framework.Plugin.AT.ATExportMono("UI系统", "TopGame.GameInstance.getInstance().uiManager")]
    public class UIManager : Framework.Module.AModule, UIFramework
    {
        public static Action<UIBase> OnHideUIAction;
        
        public static Vector3[] ms_contersArray = new Vector3[4];
        static Vector3[] ms_contersArray1 = new Vector3[4];
        private readonly float DELTA_DESTROY_TIME = 30;
        struct DeltaDestroy
        {
            public ushort uiType;
            public float lastTime;
        }
        bool m_bInited = false;
        List<ushort> m_vTemp = null;

        bool m_bDirtyMaps = false;
        bool m_bLockRemoved = false;
        List<UIBase> m_vUIFraming = null;
        Dictionary<ushort, UIBase> m_vUIs = null;

        List<UIBase> m_vPreInits = null;
        UIConfig m_UIConfig = null;

        static HashSet<ushort> ms_vLoading = new HashSet<ushort>();

        Transform m_pRoot = null;
        Canvas m_UIRootCanvas = null;
        RectTransform[] m_DynamicUIRootRT = null;
        RectTransform m_UIRootRT = null;
        Transform m_UIRoot = null;
        CanvasScaler m_CanvasScaler = null;
        Camera m_pUICamera = null;
        GameObject m_EventSystem;

        List<Transform> m_Roots = new List<Transform>();

        private bool m_bLockDestroy = false;
        private List<DeltaDestroy> m_vDestroying = null;

        private List<IUICallback> m_vCallbacks = null;
        private Dictionary<UIBase,IBuffCallBack> m_vBuffCallbacks = null;
        protected Framework.Plugin.AT.AgentTree m_pAgentTree = null;

        private UISignalTrackSystem m_SignalSlots = null;

        private static UIManager ms_pUIManager = null;
        public static UIManager GetInstance()
        {
            return ms_pUIManager;
        }
        //------------------------------------------------------
        public RectTransform GetDynamicUIRoot(int order =0) 
        {
            if (m_DynamicUIRootRT == null || m_DynamicUIRootRT.Length<=0) return m_UIRootRT;
            if (order < 0) order = 0;
            if (order >= m_DynamicUIRootRT.Length) order = m_DynamicUIRootRT.Length - 1;
            return m_DynamicUIRootRT[order]; 
        }
        //------------------------------------------------------
        public RectTransform GetStaticUIRoot() { return m_UIRootRT; }
        //------------------------------------------------------
        public Canvas GetUICanvasRoot() { return m_UIRootCanvas; }
        //------------------------------------------------------
        public CanvasScaler GetUICanvasScalerRoot() { return m_CanvasScaler; }
        //------------------------------------------------------
        public MonoBehaviour GetUICanvasScaler() { return m_CanvasScaler; }
        //------------------------------------------------------
        public Camera GetUICamera() { return m_pUICamera; }
        //------------------------------------------------------
        protected override void Awake()
        {
            m_bInited = false;
            m_vPreInits = new List<UIBase>(4);
            m_vDestroying = new List<DeltaDestroy>(64);
            m_vUIs = new Dictionary<ushort, UIBase>(256);
            m_vUIFraming = new List<UIBase>(4);
            m_vBuffCallbacks = new Dictionary<UIBase,IBuffCallBack>(8);
            m_SignalSlots = new UISignalTrackSystem(this);
            m_bLockRemoved = false;
            m_bLockDestroy = false;
            ms_pUIManager = this;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("清理所有")]
        public void Clear()
        {
            if (m_vTemp == null) m_vTemp = new List<ushort>();
            m_bLockRemoved = true;
            foreach (var db in m_vUIs)
            {
                if (db.Value.GetPermanent()) continue;
                db.Value.Destroy();
                m_vTemp.Add(db.Key);
            }
            m_bLockRemoved = false;
            for (int i = 0; i < m_vTemp.Count; ++i)
            {
                m_vUIs.Remove(m_vTemp[i]);
            }
            m_vTemp.Clear();
            if(m_SignalSlots!=null)m_SignalSlots.Clear();
            if (ms_vLoading != null) ms_vLoading.Clear();

            m_bDirtyMaps = true;
            UIUtil.ResetDynamicMakerRef();
        }
        //------------------------------------------------------
        public void RegisterCallback(IUICallback callback)
        {
            if (m_vCallbacks == null) m_vCallbacks = new List<IUICallback>(2);
            if (m_vCallbacks.Contains(callback)) return;
            m_vCallbacks.Add(callback);
        }
        //------------------------------------------------------
        public void UnRegisterCallback(IUICallback callback)
        {
            if (m_vCallbacks == null) return;
            m_vCallbacks.Remove(callback);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取设备型号")]
        public string GetPhoneType()
        {
            //Framework.Plugin.Logger.Info("设备型号:" + SystemInfo.deviceModel);
            return SystemInfo.deviceModel;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("设置适配边缘偏移")]
        public static void SetCanvasBorderOffset(float left,float top,float right,float bottom,float posZ)
        {
            Debug.Log(Framework.Core.CommonUtility.stringBuilder.AppendFormat("Adpter:{0},{1},{2},{3},{4}", left,top, right, bottom, posZ).ToString());
            UIAdapter.AdapterLeft = left;
            UIAdapter.AdapterTop = top;
            UIAdapter.AdapterPosZ = posZ;
            UIAdapter.AdapterRight = right;
            UIAdapter.AdapterBottom = bottom;
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            if (GameInstance.getInstance() == null) return;
            m_bLockRemoved = true;
            foreach (var db in m_vUIs)
            {
                db.Value.Clear();
                db.Value.Destroy();
            }
            m_bLockRemoved = false;
            m_vUIs.Clear();
            m_bDirtyMaps = true;

            m_bLockDestroy = false;
            m_vDestroying.Clear();
            if(m_SignalSlots!=null) m_SignalSlots.Clear();
            m_vCallbacks = null;
            m_vBuffCallbacks = null;
            TopGameUIExecuteEvents.OnEventExcude -= OnEventExcude;

            UIBase.OnGlobalAwakeUI -= OnAwakeUI;
            UIBase.OnGlobalShowUI -= OnShowUI;
            UIBase.OnGlobalHideUI -= OnHideUI;
            UIBase.OnGlobalDestroyUI -= OnDestroyUI;
            if (m_pAgentTree != null)
            {
                m_pAgentTree.Destroy();
                Framework.Plugin.AT.AgentTreeManager.getInstance().UnloadAT(m_pAgentTree);
                m_pAgentTree = null;
            }
            Font.textureRebuilt -= OnFontRebuild;

            ms_pUIManager = null;
        }
        //------------------------------------------------------
        void OnDestroyUI(UIBase ui)
        {
            if (ui == null || GameInstance.getInstance() == null) return;
            if (!m_bLockRemoved)
            {
                m_vUIs.Remove(ui.GetUIType());
                m_bDirtyMaps = true;
            }
            if(!m_bLockDestroy)
            {
                for (int i = 0; i < m_vDestroying.Count; ++i)
                {
                    if (m_vDestroying[i].uiType == ui.GetUIType())
                    {
                        m_vDestroying.RemoveAt(i);
                        break;
                    }
                }
            }
            if (m_SignalSlots != null) m_SignalSlots.RemoveSignalTrack(ui, m_UIConfig);
            //Framework.Plugin.Guide.GuideWrapper.OnOptionStepCheck();
            if (ui.GetUIType() != (ushort)EUIType.GuidePanel)
            {
                Framework.Plugin.Guide.GuideWrapper.OnCloseUI(ui.GetUIType(), 1 << (int)GameInstance.getInstance().GetState());
                if (ui.IsInstanced())
                    Framework.Plugin.Guide.GuideWrapper.OnCustomCallback((int)Framework.Plugin.Guide.EGuideCustomType.WaitPanelClose, ui.GetUIType());
            }
            ui = null;
        }
        //------------------------------------------------------
        public void OnAwakeUI(UIBase ui)
        {
            if (ui == null) return;
            if (m_vCallbacks != null)
            {
                for (int i = 0; i < m_vCallbacks.Count; ++i)
                {
                    m_vCallbacks[i].OnUIAwake(ui);
                }
            }
            if (ui.IsVisible() && ui.IsInstanced())
                Framework.Plugin.Guide.GuideWrapper.OnCustomCallback((int)Framework.Plugin.Guide.EGuideCustomType.WaitPanelOpen, ui.GetUIType());

            if (ui as IBuffCallBack != null && !m_vBuffCallbacks.ContainsKey(ui))
                m_vBuffCallbacks.Add(ui, ui as IBuffCallBack);
        }
        //------------------------------------------------------
        public UIBase AddUI(ushort type, UserInterface ui, bool bPermanent, int order)
        {
            if (ui == null ) return null;

            UIBase pUI = GetUI(type, false);
            if (pUI != null) return pUI;

            pUI = UIBinderBuilder.Create(type);
            if (pUI == null) return null;

            if(m_UIRoot!=null) pUI.SetParent(m_UIRoot);
            pUI.SetFullUI(true);
            pUI.SetOrder(order);
            pUI.SetTrackAble(false);
            pUI.SetAlwayShow(false);
            pUI.SetPermanent(bPermanent);
            pUI.Hide();

            //             if(bPermanent)
            //                 GameObject.DontDestroyOnLoad(ui.gameObject);
            InstanceOperiaon pInstCB = InstanceOperiaon.Malloc();
            pInstCB.userData0 = new Variable1() { intVal = type };
            pInstCB.pPoolAble = ui.GetComponent<AInstanceAble>();
            CanvasScaler scaler = ui.GetComponent<CanvasScaler>();
            if (scaler)
                GameObject.Destroy(scaler);
            if(pInstCB.pPoolAble == null) pInstCB.pPoolAble = ui.gameObject.AddComponent<AInstanceAble>();
            pUI.OnLoaded(pInstCB);
            pUI.SetZDeepth(0);
            InstanceOperiaon.Free(pInstCB);
            m_vUIs.Add(type, pUI);
            //Debug.Log($"AddUI add {(EUIType)type}");
            m_bDirtyMaps = true;
            if(!m_bInited)
            {
                m_vPreInits.Add(pUI);
            }
            return pUI;
        }
        //------------------------------------------------------
        void OnShowUI(UIBase ui)
        {
            if (ui == null) return;
            if (m_vCallbacks != null)
            {
                for (int i = 0; i < m_vCallbacks.Count; ++i)
                {
                    m_vCallbacks[i].OnUIShow(ui);
                }
            }
            if (m_pAgentTree != null) m_pAgentTree.ExecuteEvent((ushort)Base.EUIEventType.OnShowUI, ui.GetUIType());

            if (m_UIConfig == null) return;

            if (ui.GetUIType() != (ushort)EUIType.GuidePanel)
            {
                //Framework.Plugin.Guide.GuideWrapper.OnOptionStepCheck();
                Framework.Plugin.Guide.GuideWrapper.OnOpenUI(ui.GetUIType(), 1 << (int)GameInstance.getInstance().GetState());
                if (ui.IsInstanced())
                    Framework.Plugin.Guide.GuideWrapper.OnCustomCallback((int)Framework.Plugin.Guide.EGuideCustomType.WaitPanelOpen, ui.GetUIType());
            }

            // backup restore ui
            if (m_SignalSlots!=null)
                m_SignalSlots.AddSignalTrack(ui, m_UIConfig);
        }
        //------------------------------------------------------
        void OnHideUI(UIBase ui)
        {
            if (ui == null) return;
            RemoveLoadingInstance(ui.GetUIType());
            if (m_vCallbacks != null)
            {
                for (int i = 0; i < m_vCallbacks.Count; ++i)
                {
                    m_vCallbacks[i].OnUIHide(ui);
                }
            }

            //Framework.Plugin.Guide.GuideWrapper.OnOptionStepCheck();
            if (m_pAgentTree != null) m_pAgentTree.ExecuteEvent((ushort)Base.EUIEventType.OnHideUI, ui.GetUIType());
            if (ui.GetUIType() <= (int)EUIType.Loading) return;

            if (ui.GetUIType() != (ushort)EUIType.GuidePanel && GameInstance.getInstance() != null)
            {
                Framework.Plugin.Guide.GuideWrapper.OnCloseUI(ui.GetUIType(), 1 << (int)GameInstance.getInstance().GetState());
                if (ui.IsInstanced())
                    Framework.Plugin.Guide.GuideWrapper.OnCustomCallback((int)Framework.Plugin.Guide.EGuideCustomType.WaitPanelClose, ui.GetUIType());
            }


            // close when hide restore ui
            if (m_SignalSlots != null) m_SignalSlots.RemoveSignalTrack(ui, m_UIConfig);
            OnHideUIAction?.Invoke(ui);
            if (!ui.GetPermanent())
            {
                DeltaDestroy deltaUI ;
                for(int i = 0; i < m_vDestroying.Count; ++i)
                {
                    deltaUI = m_vDestroying[i];
                    if (deltaUI.uiType == ui.GetUIType())
                    {
                        deltaUI.lastTime = Time.time;
                        m_vDestroying[i] = deltaUI;
                        return;
                    }
                }
                deltaUI = new DeltaDestroy();
                deltaUI.uiType = ui.GetUIType();
                deltaUI.lastTime = Time.time;
                m_vDestroying.Add(deltaUI);
            }
        }
        //------------------------------------------------------
        public void Init(UISystem uiSystem, bool bEditor)
        {
            if (m_bInited) return;
            if (uiSystem == null)
                return;
            m_bInited = true;
            Asset pAsset = FileSystemUtil.LoadAsset(uiSystem.uiConfig, false, true, true);
            if (pAsset == null || pAsset.GetOrigin() == null)
            {
                Framework.Plugin.Logger.Break("请检查"+ uiSystem.uiConfig + ",是否存在!");
                return;
            }
            m_UIConfig = pAsset.GetOrigin() as UIConfig;

            if (m_UIConfig != null && m_UIConfig.uiAnimators)
                UIAnimatorFactory.getInstance().Init(m_UIConfig.uiAnimators, true);

            GameObject pUIRoot = GameObject.Instantiate(uiSystem.gameObject);
            uiSystem = pUIRoot.GetComponent<UISystem>();
            m_pRoot = pUIRoot.transform;
            m_pRoot.position = new Vector3(0,10000,0);
            m_UIRootCanvas = uiSystem.rootCanvas;
            m_UIRootRT = m_UIRootCanvas.transform as RectTransform;
            m_DynamicUIRootRT = uiSystem.DynamicRoot;
            m_UIRoot = uiSystem.Root;
            m_CanvasScaler = uiSystem.contentScaler;
            if (Screen.width > Screen.height)
            {
                m_CanvasScaler.referenceResolution = new Vector2(1334, 750);
            }
            if(m_DynamicUIRootRT!=null)
            {
                for (int i = 0; i < m_DynamicUIRootRT.Length; ++i)
                {
                    m_Roots.Add(m_DynamicUIRootRT[i]);
                }
            }
            m_Roots.Add(m_UIRoot);

            SetUIOffset();

            GameObject.DontDestroyOnLoad(pUIRoot);

            if(!bEditor)
            {
                UIBase pIU = GetUI((ushort)EUIType.GameInfo);
                if (pIU != null) pIU.Show();
                
            }

            m_pUICamera = uiSystem.uiCamera;
            if (m_pUICamera == null) m_pUICamera = pUIRoot.GetComponentInChildren<Camera>();

            if (UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline != null)
            {
                m_pUICamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
                CameraKit.AddCameraStack(m_pUICamera);
            }

            TopGameUIExecuteEvents.OnEventExcude += OnEventExcude;

            m_bLockRemoved = false;
            m_bLockDestroy = false;

            UIBase.OnGlobalAwakeUI += OnAwakeUI;
            UIBase.OnGlobalShowUI += OnShowUI;
            UIBase.OnGlobalHideUI += OnHideUI;
            UIBase.OnGlobalDestroyUI += OnDestroyUI;

            Font.textureRebuilt += OnFontRebuild;

            for(int i = 0; i < m_vPreInits.Count; ++i)
            {
                if(m_vPreInits[i].GetInstanceAble()!=null)
                {
                    m_vPreInits[i].SetParent(m_UIRoot);
                }
            }
            m_vPreInits.Clear();

            if (GameInstance.getInstance().unlockMgr != null)
            {
                GameInstance.getInstance().unlockMgr.Init();
            }
#if USE_VR
            m_UIRootCanvas.renderMode = RenderMode.WorldSpace;
            m_UIRootCanvas.worldCamera = m_pUICamera;
            m_pUICamera.cameraType = CameraType.VR;
            m_pUICamera.orthographic = false;
            m_pUICamera.fieldOfView = 80;
            m_pUICamera.nearClipPlane = 0.1f;
            m_pUICamera.farClipPlane = 1000.0f;
            m_pUICamera.transform.SetParent(RootsHandler.CameraSystemRoot);
            m_pRoot.transform.position += Vector3.forward * 30;
            m_pRoot.gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.UI.TrackedDeviceGraphicRaycaster>();
#endif

            if (m_CanvasScaler)
            {
                Debug.Log(Base.Util.stringBuilder.Append("CanvasScaler:").Append(m_CanvasScaler.referenceResolution).ToString());
            }
            Debug.Log(Base.Util.stringBuilder.Append("实际分辨率:").Append(GetReallyResolution()).ToString());
            Debug.Log(Base.Util.stringBuilder.Append(Screen.width).Append(",").Append(Screen.height).ToString());
        }
        //------------------------------------------------------
        public void PreferParent(Transform transNode)
        {
            if (transNode == null) return;
            RectTransform rect = transNode as RectTransform;

            if (m_UIRoot != null)
                transNode.SetParent(m_UIRoot);
            if (rect == null) return;
            rect.localScale = Vector3.one;
            rect.anchoredPosition3D = Vector2.zero;
            rect.offsetMax = Vector3.zero;
            rect.offsetMin = Vector3.zero;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
        }
        //------------------------------------------------------
        public void StartUp()
        {
            m_pAgentTree = Framework.Plugin.AT.AgentTreeManager.getInstance().LoadAT(Data.ATModuleSetting.UIMgrAT);
            if (m_pAgentTree != null)
            {
                m_pAgentTree.Enter();
            }
        }
        //------------------------------------------------------
        void OnFontRebuild(Font font)
        {
            //Framework.Plugin.Logger.Info(font.name + "---------------rebuild");
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("UI根节点")]
        public Transform GetRoot()
        {
            return m_UIRoot;
        }
        //------------------------------------------------------
        public static Transform GetAutoUIRoot(int order =0)
        {
            UIManager pThis = null;
            if (ModuleManager.mainModule is GameInstance)
            {
                GameInstance pGameInstance = ModuleManager.mainModule as GameInstance;
                if (pGameInstance != null)
                    pThis = pGameInstance.uiManager;
                if (pThis != null)
                {
                    var root = pThis.GetDynamicUIRoot(order);
                    if (root) return root;
                    return pThis.GetRoot();
                }
            }
            return null;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("跟节点")]
        public Transform GetRoot3D()
        {
            return m_pRoot;
        }
        //------------------------------------------------------
        public Dictionary<ushort, UIBase> GetUIS()
        {
            return m_vUIs;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("界面是否显示", new System.Type[] { typeof(EUIType) })]
        public bool IsWinShow(ushort type)
        {
            UIBase pUI = GetUI(type, false);
            if (pUI == null) return false;
            return pUI.IsVisible();
        }

        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("显示所有")]
        public void ShowAll()
        {
            if (m_vTemp == null) m_vTemp = new List<ushort>();
            m_vTemp.Clear();
            foreach (var db in m_vUIs)
            {
                m_vTemp.Add(db.Key);
            }
            UIBase ui;
            for (int i = 0; i < m_vTemp.Count; ++i)
            {
                if (m_vUIs.TryGetValue(m_vTemp[i], out ui))
                {
                    ui.Show();
                }
            }
            m_vTemp.Clear();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("显示一个UI", new System.Type[] { typeof(EUIType) }), ATExportGUID(-2073083593)]
        public UIHandle ShowUI(ushort type)
        {
          //  Framework.Plugin.Logger.Info("ShowUI type:" + type);
            UIBase pUI = GetUI(type, true);
            if (pUI != null)
            {
                pUI.Show();
            }
            return pUI;
        }
        //------------------------------------------------------
        public static T ShowUI<T>(EUIType uiType) where T : UIHandle
        {
            var frameWork = UI.UIKits.GetUIFramework();
            if (frameWork == null) return null;
            return frameWork.CastShowUI<T>((ushort)uiType);
        }
        //------------------------------------------------------
        public static UIBase ShowUI(EUIType uiType)
        {
            var frameWork = UI.UIKits.GetUIFramework();
            if (frameWork == null) return null;
            return (UIBase)frameWork.ShowUI((ushort)uiType);
        }
        //------------------------------------------------------
        public static void HideUI(EUIType uiType)
        {
            var frameWork = UI.UIKits.GetUIFramework();
            if (frameWork == null) return;
            frameWork.HideUI((ushort)uiType);
        }
        //------------------------------------------------------
        public T CastShowUI<T>(ushort type = 0) where T : UIHandle
        {
            if (type == 0) type = (ushort)UIBinderBuilder.GetTypeToUIType(typeof(T));
            if (type == 0) return null;
            UIHandle pUI = ShowUI(type);
            return pUI as T;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("隐藏一个UI", new System.Type[] { typeof(EUIType) })]
        public void HideUI(ushort type)
        {
            UIBase pUI = GetUI(type, false);
            if (pUI != null)
                pUI.Hide();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("隐藏所有")]
        public void HideAll()
        {
            HideAllIngores(null);
        }
        //------------------------------------------------------
        public void HideAllIngores(HashSet<int> vIngores = null)
        {
            m_SignalSlots.Clear();
            if (m_vTemp == null) m_vTemp = new List<ushort>();
            m_vTemp.Clear();
            foreach (var db in m_vUIs)
            {
                if (vIngores!=null && vIngores.Contains(db.Key)) continue;
                if (db.Value.CanHide() && db.Value.IsVisible())
                    m_vTemp.Add(db.Key);
            }
            UIBase ui;
            for (int i = 0; i < m_vTemp.Count; ++i)
            {
                if (m_vUIs.TryGetValue(m_vTemp[i], out ui))
                {
                    ui.Hide();
                }
            }
            m_vTemp.Clear();
        }
        //------------------------------------------------------
        public void HideAllIngore(ushort uiType)
        {
            m_SignalSlots.Clear();
            if (m_vTemp == null) m_vTemp = new List<ushort>();
            m_vTemp.Clear();
            foreach (var db in m_vUIs)
            {
                if (uiType ==  db.Key) continue;
                if (db.Value.CanHide() && db.Value.IsVisible())
                    m_vTemp.Add(db.Key);
            }
            UIBase ui;
            for (int i = 0; i < m_vTemp.Count; ++i)
            {
                if (m_vUIs.TryGetValue(m_vTemp[i], out ui))
                {
                    ui.Hide();
                }
            }
            m_vTemp.Clear();
        }
        //------------------------------------------------------
        public void ShowRoot(bool bShow)
        {
            if(bShow)
            {
                if(m_Roots!=null)
                {
                    for(int i = 0;i< m_Roots.Count; ++i)
                    {
                        if (m_Roots[i] != null) m_Roots[i].localScale = Vector3.one;
                    }
                }
            }
            else
            {
                if (m_Roots != null)
                {
                    for (int i = 0; i < m_Roots.Count; ++i)
                    {
                        if (m_Roots[i] != null) m_Roots[i].localScale = Vector3.zero;
                    }
                }
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("移动一个UI到屏幕外", new System.Type[] { typeof(EUIType) })]
        public void MoveOutside(EUIType type)
        {
            UIBase ui = GetUI((ushort)type, false);
            if (ui == null) return;
            ui.MoveOutside();
        }
        //------------------------------------------------------
        public void MoveOutsideAll(EUIType ingoreType = EUIType.None)
        {
            if (m_vTemp == null) m_vTemp = new List<ushort>();
            m_vTemp.Clear();
            foreach (var db in m_vUIs)
            {
                if (db.Value.IsVisible() && ingoreType != (EUIType)db.Key)
                    m_vTemp.Add(db.Key);
            }
            UIBase ui;
            for(int i = 0; i < m_vTemp.Count; ++i)
            {
                if(m_vUIs.TryGetValue(m_vTemp[i], out ui))
                {
                    ui.MoveOutside();
                }
            }
            m_vTemp.Clear();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("移动一个UI到屏幕内", new System.Type[] { typeof(EUIType) })]
        public void MoveInside(EUIType type)
        {
            UIBase ui = GetUI((ushort)type, false);
            if (ui == null) return;
            ui.MoveInside();
        }
        //------------------------------------------------------
        public void MoveInsideAll(EUIType ingoreType = EUIType.None)
        {
            if (m_vTemp == null) m_vTemp = new List<ushort>();
            m_vTemp.Clear();
            foreach (var db in m_vUIs)
            {
                if (db.Value.IsVisible() && ingoreType != (EUIType)db.Key)
                    m_vTemp.Add(db.Key);
            }
            UIBase ui;
            for (int i = 0; i < m_vTemp.Count; ++i)
            {
                if (m_vUIs.TryGetValue(m_vTemp[i], out ui))
                {
                    ui.MoveInside();
                }
            }
            m_vTemp.Clear();
        }

        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("关闭一个UI", new System.Type[] { typeof(EUIType) })]
        public void CloseUI(ushort type)
        {
            UIBase pUI = GetUI(type, false);
            if (pUI != null)
            {
                if (pUI.Close())
                {
                    if (!m_bLockRemoved)
                    {
                        m_vUIs.Remove(type);
                        m_bDirtyMaps = true;
                    }
                    if(!m_bLockDestroy)
                    {
                        for (int i = 0; i < m_vDestroying.Count; ++i)
                        {
                            if (m_vDestroying[i].uiType == type)
                            {
                                m_vDestroying.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("关闭所有")]
        public void CloseAll()
        {
            if (m_vTemp == null) m_vTemp = new List<ushort>();
            m_vTemp.Clear();
            foreach (var db in m_vUIs)
                m_vTemp.Add(db.Key);
            for (int i =0; i < m_vTemp.Count; ++i)
            {
                UIBase uiBase;
                if(m_vUIs.TryGetValue(m_vTemp[i], out uiBase) && uiBase.Close())
                    m_vUIs.Remove(m_vTemp[i]);
            }
            m_vTemp.Clear();
            if (m_SignalSlots != null) m_SignalSlots.Clear();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取一个UI", new System.Type[] { typeof(EUIType) })]
        public UIBase GetUI(ushort type, bool bAuto = true)
        {
            if (m_vUIs == null) return null;
            UIBase pUI;
            if (m_vUIs.TryGetValue(type, out pUI))
                return pUI;
            if (bAuto)
            {
                return (UIBase)CreateUI(type, false);
            }
            return null;
        }
        //------------------------------------------------------
        public static T CastUI<T>(EUIType type = 0, bool bAuto = true) where T : UIHandle
        {
            var frameWork = UI.UIKits.GetUIFramework();
            if (frameWork == null) return null;
            return frameWork.CastGetUI<T>((ushort)type, bAuto);
        }
        //------------------------------------------------------
        public T CastGetUI<T>(ushort type = 0, bool bAuto = true) where T : UIHandle
        {
            if (m_vUIs == null) return null;
            if(type == 0)
            {
                type = (ushort)UIBinderBuilder.GetTypeToUIType(typeof(T));
                if (type == 0) return null;
            }
            UIBase pUI;
            if (m_vUIs.TryGetValue(type, out pUI))
                return pUI as T;
            if (bAuto)
            {
                return CreateUI(type, false) as T;
            }
            return null;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("创建一个UI", new System.Type[] { typeof(EUIType) }),ATExportGUID(207487781)]
        public UIHandle CreateUI(ushort type, bool bShow = false)
        {
            UIBase pUI = GetUI(type, false);
            if (pUI != null) return pUI;
            if (m_UIConfig == null) return null;
            UIConfig.UI uiData = m_UIConfig.GetUI(type);
            if (uiData == null) return null;

            pUI = UIBinderBuilder.Create(type);
            if (pUI == null) return null;

            if (bShow) pUI.Show();
            pUI.SetUIType(type);
            pUI.SetParent(m_UIRoot);
            pUI.SetFullUI(uiData.fullUI);
            pUI.SetOrder(uiData.Order);

            pUI.SetAlwayShow(uiData.alwayShow);
            pUI.SetTrackAble(uiData.trackAble);
            pUI.SetBackupAble(uiData.canBackupFlag);
            pUI.SetPermanent(uiData.permanent);
            pUI.SetShowLog(uiData.showLog);
            InstanceOperiaon pInstCB = FileSystemUtil.SpawnInstance(uiData.prefab, false);
            if (pInstCB != null)
            {
#if UNITY_EDITOR
                pUI.SartProfiler();
#endif
                ms_vLoading.Add(type);
                pInstCB.SetLimitCheckCnt(1);
                pInstCB.OnCallback =pUI.OnLoaded;
                pInstCB.OnSign =pUI.OnSign;
                pInstCB.SetUserData(0, new Variable1() { intVal = type });
                pInstCB.Refresh();
            }
            pUI.SetZDeepth(uiData.uiZValue);
            m_vUIs.Add(type, pUI);
            //Debug.Log($"CreateUI add {(EUIType)type}");
            m_bDirtyMaps = true;

            if (!bShow && !uiData.permanent)
            {
                DeltaDestroy deltaUI = new DeltaDestroy();
                deltaUI.uiType = pUI.GetUIType();
                deltaUI.lastTime = Time.time;
                m_vDestroying.Add(deltaUI);
            }
            return pUI;
        }
        //------------------------------------------------------
        public static void RemoveLoadingInstance(ushort uiType)
        {
            ms_vLoading.Remove(uiType);
        }
        //------------------------------------------------------
        public static bool HasLodingInstnaceUI()
        {
            return ms_vLoading.Count > 0;
        }
        //------------------------------------------------------
        public static bool IsShowUI(EUIType uiType)
        {
            var frameWork = UI.UIKits.GetUIFramework();
            if (frameWork == null) return false;
            return frameWork.IsShow((ushort)uiType);
        }
        //------------------------------------------------------
        public bool IsShow(ushort type)
        {
            if (type == 0) return false;
            UIBase pUI = GetUI(type, false);
            if (pUI != null)
                return pUI.IsVisible() && pUI.IsMoveOut() == false;
            return false;
        }
        //------------------------------------------------------
        public void SetScreenOrientation(ScreenOrientation orientation)
        {
            if (Screen.orientation == orientation)
                return;
            Screen.orientation = orientation;
            if (orientation == ScreenOrientation.Portrait || orientation == ScreenOrientation.PortraitUpsideDown)
            {
               // int width = Mathf.Min(Screen.width, Screen.height);
               // int height = Mathf.Max(Screen.width, Screen.height);
              //  Screen.SetResolution(width, height, true);

                if (m_CanvasScaler)
                {
                    m_CanvasScaler.referenceResolution = new Vector2(750, 1334);
                    m_CanvasScaler.matchWidthOrHeight = 0;
                }
#if UNITY_EDITOR
                Framework.ED.EditorUtil.SetGameViewTargetSize(750, 1334);
#endif
            }
            else
            {
               // int width = Mathf.Max(Screen.width, Screen.height);
               // int height = Mathf.Min(Screen.width, Screen.height);
               // Screen.SetResolution(width, height, true);

                if (m_CanvasScaler)
                {
                    m_CanvasScaler.referenceResolution = new Vector2(1334, 750);
                    m_CanvasScaler.matchWidthOrHeight = 1;
                }
#if UNITY_EDITOR
                Framework.ED.EditorUtil.SetGameViewTargetSize(1334, 750);
#endif
            }
        }
        //------------------------------------------------------
        public void Update(float fFrame)
        {
            fFrame = Time.deltaTime;
#if UNITY_EDITOR
            if (m_CanvasScaler)
            {
                if (Screen.width > Screen.height)
                {
                    m_CanvasScaler.referenceResolution = new Vector2(1334, 750);
                }
                else
                {
                    m_CanvasScaler.referenceResolution = new Vector2(750, 1334);
                }
            }

#endif
            m_bDirtyMaps = false;
            foreach (var db in m_vUIs)
            {
                db.Value.Update(fFrame);
                if (m_bDirtyMaps)
                    break;
            }
            m_bDirtyMaps = false;

            if (m_vDestroying.Count>0)
            {
                DeltaDestroy uiDelta;
                float curTime = Time.time;
                for (int i = 0; i < m_vDestroying.Count;)
                {
                    uiDelta = m_vDestroying[i];
                    if (curTime - uiDelta.lastTime >= DELTA_DESTROY_TIME)
                    {
                        UIBase uiBase = GetUI(uiDelta.uiType, false);
                        if (uiBase == null || uiBase.IsVisible())
                        {
                            m_vDestroying.RemoveAt(i);
                            continue;
                        }
                        m_bLockDestroy = true;
                        uiBase.Destroy();
                        m_bLockDestroy = false;
                        m_vDestroying.RemoveAt(i);
                        m_vUIs.Remove(uiDelta.uiType);
                    }
                    else
                        ++i;
                }
            }
        }
        //------------------------------------------------------
        public static void Free()
        {
            UIUtil.ResetDynamicMakerRef();
            UIManager pThis = null;
            if (ModuleManager.mainModule is GameInstance)
            {
                GameInstance pGameInstance = ModuleManager.mainModule as GameInstance;
                if (pGameInstance != null)
                    pThis = pGameInstance.uiManager;
            }
            if (pThis == null) return;
            DeltaDestroy uiDelta;
            float curTime = Time.time;
            for (int i = 0; i < pThis.m_vDestroying.Count;++i)
            {
                uiDelta = pThis.m_vDestroying[i];
                {
                    UIBase uiBase = pThis.GetUI(uiDelta.uiType, false);
                    if (uiBase != null && !uiBase.IsVisible())
                    {
                        pThis.m_bLockDestroy = true;
                        uiBase.Destroy();
                        pThis.m_bLockDestroy = false;
                        pThis.m_vDestroying.RemoveAt(i);
                        pThis.m_vUIs.Remove(uiDelta.uiType);
                    }
                }
            }
            pThis.m_vDestroying.Clear();
        }
        //------------------------------------------------------
        public bool IsInView(Transform trans)
        {
#if USE_FAIRYGUI
            if (m_pUICamera == null) return false;
            float factor = 0.1f;
            Vector3 viewPos = m_pUICamera.WorldToViewportPoint(trans.position);
            Vector3 dir = (trans.position - m_pRoot.position).normalized;
            float dot = Vector3.Dot(m_pRoot.forward, dir);
            if (dot > 0 && viewPos.x >= -factor && viewPos.x <= 1 + factor && viewPos.y >= -factor && viewPos.y <= 1 + factor)
                return true;
            return false;
#else
            if (!(trans is RectTransform))
                return CameraKit.IsInView(trans.position);
            //lb,lt,rt,rb
            m_UIRootRT.GetWorldCorners(ms_contersArray);
            (trans as RectTransform).GetWorldCorners(ms_contersArray1);
            Bounds rootBd = new Bounds();
            rootBd.min = new Vector3(ms_contersArray[0].x, ms_contersArray[0].y, 0);//忽略旋转
            rootBd.max = new Vector3(ms_contersArray[2].x, ms_contersArray[2].y, 0);

            Bounds tranBd = new Bounds();
            tranBd.min = new Vector3(ms_contersArray1[0].x, ms_contersArray1[0].y,0);//忽略旋转
            tranBd.max = new Vector3(ms_contersArray1[2].x, ms_contersArray1[2].y, 0);
            return tranBd.Intersects(rootBd);
#endif
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("UGUI坐标转屏幕坐标")]
        public Vector3 ConvertUIPosToScreen(Vector3 uiWorldPos)
        {
            return RectTransformUtility.WorldToScreenPoint(m_pUICamera, uiWorldPos);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("屏幕坐标转UGUI坐标")]
        public bool ConvertScreenToUIPos(Vector3 screenPos, bool bLocal, ref Vector3 point)
        {
            if (bLocal)
            {
                Vector2 local_temp = Vector2.zero;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_UIRoot as RectTransform, screenPos, m_pUICamera, out local_temp))
                {
                    point.x = local_temp.x;
                    point.y = local_temp.y;
                    return true;
                }
            }
            else
            {
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_UIRoot as RectTransform, screenPos, m_pUICamera, out point))
                {
                    return true;
                }
            }
            return false;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("世界坐标转UGUI坐标")]
        public bool ConvertWorldPosToUIPos(Vector3 worldPos, bool bLocal, ref Vector3 point, Camera cam = null)
        {
            if (cam == null) cam = CameraController.MainCamera;
            if (cam == null) return false;
            Vector2 screenPos = cam.WorldToScreenPoint(worldPos);
            if (bLocal)
            {
                Vector2 local_temp = Vector2.zero;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_UIRoot as RectTransform, screenPos, m_pUICamera, out local_temp))
                {
                    point.x = local_temp.x;
                    point.y = local_temp.y;
                    return true;
                }
            }
            else
            {
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_UIRoot as RectTransform, screenPos, m_pUICamera, out point))
                {
                    return true;
                }
            }
            return false;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("UGUI坐标转世界坐标")]
        public Vector3 UGUIPosToWorldPos(Camera cam, Vector3 uiguiPos, float distance = 0)
        {
            if (cam == null)
            {
                return Vector3.zero;
            }
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(m_pUICamera, uiguiPos);
            Ray ray = cam.ScreenPointToRay(screenPoint);
            return ray.GetPoint(distance<=0?cam.farClipPlane: distance);
            //  return Base.Util.RayHitPos(ray.origin, ray.direction, 0);
        }
        //------------------------------------------------------
        public Vector3 ScreenPosToWorldPos(Vector3 screenPos)
        {
            if (m_pUICamera == null)
            {
                return Vector3.zero;
            }

            return m_pUICamera.ScreenToWorldPoint(screenPos);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("连续打开界面")]
        public void SequenceOpenUI(List<ushort> uis)
        {
            if (uis == null || uis.Count < 2)
            {
                Framework.Plugin.Logger.Error("连续打开界面数据错误");
                return;
            }

            List<Variable1> used = new List<Variable1>();
            for (int i=0;i<uis.Count;i++)
            {
                if (i != uis.Count - 1)
                {
                    UIBase baseUI = (UIBase)(i==0? ShowUI(uis[i]):GetUI(uis[i]));
                    Variable1 var1 = new Variable1();
                    var1.intVal = uis[i + 1];
                    used.Add(var1);

                    baseUI.OnCloseUI += (baseui) =>
                    {
                        ShowUI((ushort)var1.intVal);
                    };
                }
                else
                {
                    UIBase baseUI = GetUI(uis[i]);
                    List<Variable1> resetList = used;
                    baseUI.OnCloseUI += (baseui) =>
                    {
                        for (int j = 0; j < resetList.Count; j++)
                        {
                            UIBase subui = GetUI((ushort)resetList[j].intVal);
                            subui.OnCloseUI = null;
                        }
                    };                  
                }               
            }
        }


        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("隐藏GameObject")]
        public void HideGameObject(GameObject Go)
        {
            if (Go)
                Go.gameObject.SetActive(false);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("显示GameObject")]
        public void ShowGameObject(GameObject Go)
        {
            if (Go)
                Go.gameObject.SetActive(true);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("显示GameObjects")]
        public void ShowGameObjects(List<GameObject> param)
        {
            if (param == null) return;

            for (int i = 0; i < param.Count; i++)
            {
                GameObject go = param[i];
                if (go)
                    go.SetActive(true);
            }

        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("隐藏GameObjects")]
        public void HideGameObjects(List<GameObject> param)
        {
            if (param == null) return;

            for (int i = 0; i < param.Count; i++)
            {
                GameObject go = param[i];
                if (go)
                    go.SetActive(false);
            }

        }

        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("GameObject是否显示")]
        public bool IsGameObjectShow(GameObject Go)
        {
            if (Go)
                return Go.activeSelf;
            return false;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取UI资源文件", new System.Type[] { typeof(EUIType) })]
        public string GetUIAssetFile(ushort type)
        {
            if (m_UIConfig == null) return null;
            UIConfig.UI uiData = m_UIConfig.GetUI(type);
            if (uiData == null) return null;
            return uiData.prefab;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("预实例化UI资源", new System.Type[] { typeof(EUIType) })]
        public void PreSpawnUI(List<ushort> uis, bool bAsync = true, bool bFrontQueue = true)
        {
            if (uis == null) return;
            for(int i = 0; i < uis.Count; ++i)
            {
                if (m_vUIs.ContainsKey(uis[i]))
                    continue;
                UIConfig.UI uiData = m_UIConfig.GetUI(uis[i]);
                if (uiData == null) continue;
                if(FileSystemUtil.GetPreSpawnStats(uiData.prefab) <=0)
                {
                    FileSystemUtil.PreSpawnInstance(uiData.prefab, bAsync, bFrontQueue);
                }
            }
        }
        //------------------------------------------------------
        public bool OnEventExcude(IEventSystemHandler hander, BaseEventData pData)
        {
            //             if (hander == null || pData == null) return false;
            //             if(hander is IUpdateSelectedHandler)
            //             {
            //                 return false;
            //             }
            //             if (hander is IPointerClickHandler)
            //             {
            //                 return Framework.Plugin.AT.AgentTreeManager.getInstance().ExecuteEvent(pData.selectedObject);
            //             }
            return false;
        }

        //------------------------------------------------------
        public void OnBeginBuff(Actor pActor, BufferState buff)
        {
            if (m_vBuffCallbacks == null) return;
            foreach (var child in m_vBuffCallbacks)
            {
                child.Value.OnBeginBuff(pActor,buff);
            }
        }
        //------------------------------------------------------
        public void OnEndBuff(Actor pActor, BufferState buff)
        {
            if (m_vBuffCallbacks == null) return;

            foreach (var child in m_vBuffCallbacks)
            {
                child.Value.OnEndBuff(pActor, buff);
            }
        }
        //------------------------------------------------------
        public Vector2 GetReallyResolution()
        {
            if (m_UIRootRT == null)
            {
                if (m_CanvasScaler != null)
                {
                    return m_CanvasScaler.referenceResolution;
                }
                return new Vector2(1334, 750);
            }
            //Framework.Plugin.Logger.Info("canvas 分辨率:" + m_UIRootRT.sizeDelta);
            //Framework.Plugin.Logger.Info("手机分辨率:" + Screen.width + "," + Screen.height);
            return m_UIRootRT.sizeDelta;
        }
        //------------------------------------------------------
        public Vector2 GetReallyResolutionRatio()
        {
            if (m_UIRootRT == null)
            {
                return new Vector2(1, 1);
            }
            return new Vector2(m_UIRootRT.sizeDelta.x / 1334.0f, m_UIRootRT.sizeDelta.y / 750.0f);
        }
        //------------------------------------------------------
        public bool IsLandscape()
        {
            var resolution = GetReallyResolution();
            return resolution.x > resolution.y;
        }
        //------------------------------------------------------
        void SetUIOffset()
        {
            string deviceModel = GetPhoneType();
            float left = 0, top = 0, right = 0, bottom = 0, posZ = 0;
            //获取配置表,是否能正确获取到配置表
            //如果存在id跟这个一致的配置
            //读取偏移
            var cfg = Data.DataManager.getInstance().DeviceUIAdapter.GetData(deviceModel);
            if (cfg != null)
            {
                left = cfg.left;
                top = cfg.top;
                right = cfg.right;
                bottom = cfg.bottom;
                posZ = cfg.posZ;
            }
            //left = 20;
            SetCanvasBorderOffset(left, top, right, bottom, posZ);
        }
        //------------------------------------------------------
        public void SetEventSystem(GameObject eventSystem)
        {
            m_EventSystem = eventSystem;
        }
        //------------------------------------------------------ 获取当前UI显示个数
        public int GetShowUINum()
        {
            int num = 0;
            foreach (KeyValuePair<ushort,UIBase> pUI in m_vUIs)
            {
                if (pUI.Value.IsVisible() && pUI.Value.IsMoveOut() == false)
                {
                    num++;
                }
            }
            return num;
        }
    }
}
