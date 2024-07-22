/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	FrameworkMain
作    者:	HappLI
描    述:	入口
*********************************************************************/

using UnityEngine;
using TopGame.Data;
using Framework.Core;
using System.Collections;

namespace TopGame
{
    public class FrameworkMain : MonoBehaviour, Framework.IFrameworkCore
    {
        public GameObject eventSystemPrefab;
        public Net.WebHandler webHandler;
        public GameQuality gameQuality;
        public SDK.SDKSetting SDKConfig;
        public GameObject CameraPrefab;
        public GameObject VRCameraPrefab;
        public UI.UISystem UISystem;
        public GameObject SoundSystem;
        public GameObject CustomLightSystem;
        public RootsHandler rootHandler;

        public int maxThread = 4;
        public int GetMaxThread() { return maxThread; }
        public ActionSystemConfig GetActionSystemConfig() { return new ActionSystemConfig(); }

        public Material[] initMaterials;

        public UI.StartUpUpdateUI versionUpdateUI;

        public UI.LogoUI logo;
        public UI.VideoSerialize videoPanel;
        public UI.UIPreTip preTipPanel;
        public UI.WebViewPanel webViewPanel;
        public EFileSystemType eFileStreamType = EFileSystemType.AssetData;
        public EFileSystemType GetFileStreamType() { return eFileStreamType; }

        protected Logic.FrameworkStartUp m_pStartUp = null;

        [System.NonSerialized]
        public GameObject eventSystem;

        public System.Action OnGUIAction = null;
        //------------------------------------------------------
        public virtual bool IsEditor()
        {
            return false;
        }
        //------------------------------------------------------
        protected virtual void Awake()
        {
            GameObject.DontDestroyOnLoad(this);
            gameObject.name = "System";
            eventSystem = null;
            if (eventSystemPrefab)
            {
                eventSystem = GameObject.Instantiate(eventSystemPrefab);
            }
            else
            {
                eventSystem = new GameObject();
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
            eventSystem.transform.position = Vector3.zero;
            eventSystem.transform.eulerAngles = Vector3.zero;
            eventSystem.transform.localScale = Vector3.one;
            eventSystem.transform.SetParent(this.transform);

            Core.VideoController.getInstance().Init();

            if (webHandler) Net.WebHandler.Init(webHandler);
            UI.UIPreTip.InitInstance(preTipPanel);
            UI.WebViewPanel.InitInstance(webViewPanel);
            Data.GlobalDefaultResources.DefaultMaterials = initMaterials;
        }
        //------------------------------------------------------
        protected virtual void Start()
        {
            m_pStartUp = Logic.FrameworkStartUp.getInstance();
            m_pStartUp.Init(this);
            m_pStartUp.SetSection(Logic.EStartUpSection.Logo);
        }
        //------------------------------------------------------
        public Coroutine BeginCoroutine(IEnumerator coroutine)
        {
            return this.StartCoroutine(coroutine);
        }
        //------------------------------------------------------
        public void EndCoroutine(Coroutine coroutine)
        {
            this.StopCoroutine(coroutine);
        }
        //------------------------------------------------------
        public void EndCoroutine(IEnumerator coroutine)
        {
            this.StopCoroutine(coroutine);
        }
        //------------------------------------------------------
        public void EndAllCoroutine()
        {
            this.StopAllCoroutines();
        }
        //------------------------------------------------------
        protected virtual void Update()
        {
            Base.FpsStat.getInstance().Update();
            if(m_pStartUp!=null) m_pStartUp.Update();
        }
        //------------------------------------------------------
        protected virtual void OnDestroy()
        {
            if (m_pStartUp != null) m_pStartUp.Shutdown();
            m_pStartUp = null;
        }
        //------------------------------------------------------
        protected virtual void OnApplicationQuit()
        {
            if (m_pStartUp != null) m_pStartUp.Shutdown();
            m_pStartUp = null;
        }
        //------------------------------------------------------
        protected virtual void LateUpdate()
        {
            if (m_pStartUp != null) m_pStartUp.LateUpdate();
        }
        //------------------------------------------------------
        protected virtual void FixedUpdate()
        {
            if (m_pStartUp != null) m_pStartUp.FixedUpdate();
        }
        //------------------------------------------------------
        private void OnGUI()
        {
            if (m_pStartUp != null) m_pStartUp.GUI();
            if (OnGUIAction != null) OnGUIAction();
        }
        //------------------------------------------------------
        protected virtual void OnApplicationPause(bool pause)
        {
            if (m_pStartUp != null) m_pStartUp.Pause(pause);
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        protected virtual void OnDrawGizmos()
        {
            if (m_pStartUp != null) m_pStartUp.OnDrawGizmos();
        }
#endif
    }

}

