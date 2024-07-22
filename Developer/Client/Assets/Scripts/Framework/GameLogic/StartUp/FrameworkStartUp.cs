/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Login
作    者:	HappLI
描    述:	登录
*********************************************************************/

using System;
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Data;
using TopGame.Net;
using UnityEngine;
using TopGame.Base;
using TopGame.UI;
using Framework.Core;
using TopGame.Hot;

namespace TopGame.Logic
{
    public enum EStartUpSection
    {
        None =0,
        Logo,
        Version,
        AppAwake,
        AppStartUp,
    }
    public class FrameworkStartUp : Singleton<FrameworkStartUp>
    {
        public static System.Action<EStartUpSection> OnStartupSectionChange;
        LogoSplash m_pLogoSplash = null;
        VersionCheck m_pVersionCheck = null;
        HotScriptsModule m_pHotScripts = null;
        EStartUpSection m_eSection = EStartUpSection.None;
        FileSystem m_pFileSystem = null;
        Framework.IFrameworkCore m_pFrameworkMain = null;

        UI.StartUpLoading m_StartUpLoading = new UI.StartUpLoading();

        static float ms_fLoadProgress = 0;
        //------------------------------------------------------
        internal void Init(Framework.IFrameworkCore pMain)
        {
            ms_fLoadProgress = 0;
            m_pFrameworkMain = pMain;
            m_pHotScripts = new HotScriptsModule();
            m_pLogoSplash = new LogoSplash(this);
            m_pVersionCheck = new VersionCheck(this);

            //! 初始化
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            EFileSystemType type = pMain.GetFileStreamType();
            if (type == EFileSystemType.AssetData)
            {
#if UNITY_EDITOR
#else
#if UNITY_STANDALONE
                 type = EFileSystemType.AssetBundle; 
#elif UNITY_ANDROID
                type = EFileSystemType.AssetBundle;
                Application.targetFrameRate = 30;
                Shader.globalMaximumLOD = 300;
#elif UNITY_IOS
                type = EFileSystemType.AssetBundle;
                Application.targetFrameRate = 30;
                Shader.globalMaximumLOD = 300;
#else
                type = EFileSystemType.AssetBundle;
#endif
#endif
            }

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            VersionData.Parser(type, "version");

            if (VersionData.defaultLanguage != -1)
            {
                if (!PlayerPrefs.HasKey(TopGame.Core.ALocalizationManager.SD_LANGUAGE_KEY))
                {
                    PlayerPrefs.SetInt(TopGame.Core.ALocalizationManager.SD_LANGUAGE_KEY, VersionData.defaultLanguage);
                }
            }
            m_pFileSystem = new FileSystem();
            m_pFileSystem.PreBuild();
            m_pFileSystem.Init(type, "base_pkg", VersionData.version, VersionData.base_pack_cnt, VersionData.assetbundleEncryptKey, VersionData.sceneStreamAB);
        }
        //------------------------------------------------------
        public static FileSystem GetFileSystem()
        {
            return getInstance().m_pFileSystem;
        }
        //------------------------------------------------------
        public static Framework.IFrameworkCore GetFrameworkMain()
        {
            return getInstance().m_pFrameworkMain;
        }
        //------------------------------------------------------
        internal UI.StartUpLoading GetStartUpLoading()
        {
            return m_StartUpLoading;
        }
        //------------------------------------------------------
        public static void UpdateLoadProgress(float fProgress)
        {
            ms_fLoadProgress = fProgress;
        }
        //------------------------------------------------------
        internal void Shutdown()
        {
            if (m_pLogoSplash != null) m_pLogoSplash.Shutdown();
            m_pLogoSplash = null;
            if (m_pVersionCheck != null) m_pVersionCheck.Shutdown();
            m_pVersionCheck = null;
            Framework.Module.ModuleManager.getInstance().ShutDown();
            if (m_pFileSystem != null) m_pFileSystem.Destroy();
            m_pFileSystem = null;
            m_pFrameworkMain = null;
            OnStartupSectionChange = null;
        }
        //------------------------------------------------------
        internal void Pause(bool bPause)
        {
            if (m_eSection == EStartUpSection.AppStartUp)
                Framework.Module.ModuleManager.getInstance().OnApplicationPause(bPause);
        }
        //------------------------------------------------------
        internal void Focus(bool bFocus)
        {
            if (m_eSection == EStartUpSection.AppStartUp)
                Framework.Module.ModuleManager.getInstance().OnApplicationFocus(bFocus);
        }
        //------------------------------------------------------
        internal void GUI()
        {
#if UNITY_EDITOR
            if (m_eSection == EStartUpSection.AppStartUp)
                Framework.Module.ModuleManager.getInstance().OnGUI();
#endif
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        internal void OnDrawGizmos()
        {
            if(m_eSection == EStartUpSection.AppStartUp)
                Framework.Module.ModuleManager.getInstance().OnDrawGizmos();
        }
#endif
        //------------------------------------------------------
        public void SetSection(EStartUpSection section)
        {
            if (m_eSection == section) return;
            switch (m_eSection)
            {
                case EStartUpSection.Logo:
                    if (m_pLogoSplash != null) m_pLogoSplash.Exit();
                    break;
                case EStartUpSection.Version:
                    if (m_pVersionCheck != null) m_pVersionCheck.Exit();
                    break;
            }
            m_eSection = section;
            switch (m_eSection)
            {
                case EStartUpSection.Logo:
                    if (OnStartupSectionChange != null) OnStartupSectionChange.Invoke(EStartUpSection.Logo);
                    if (m_pLogoSplash != null) m_pLogoSplash.Start();
                    break;
                case EStartUpSection.Version:
                    if (OnStartupSectionChange != null) OnStartupSectionChange.Invoke(EStartUpSection.Version);
                    if (m_pVersionCheck != null) m_pVersionCheck.Start();
                    break;
                case EStartUpSection.AppAwake:
                    {
                        Debug.Log("awake game");
                        if (m_pFileSystem != null)
                        {
                            GameDelegate.StartUp(m_pFileSystem);
                        }
                        m_StartUpLoading.SetExternProgress(0);
                        m_StartUpLoading.SetTotalProgress(200);
                        m_pHotScripts.Awake(m_pFileSystem);
                        m_pHotScripts.Start();
                        if (OnStartupSectionChange != null) OnStartupSectionChange.Invoke(EStartUpSection.AppAwake);
                    }
                    break;
                case EStartUpSection.AppStartUp:
                    {
                        if (m_StartUpLoading != null) m_StartUpLoading.SetExternProgress(100);
                        Debug.Log("startup game");
                        if (OnStartupSectionChange != null) OnStartupSectionChange.Invoke(EStartUpSection.AppStartUp);
                    }
                    break;
            }
        }
        //------------------------------------------------------
        internal void Update()
        {
            float fTime = Time.deltaTime;
            switch (m_eSection)
            {
                case EStartUpSection.Logo:
                    if (m_pLogoSplash != null) m_pLogoSplash.Update(fTime);
                    break;
                case EStartUpSection.Version:
                    if (m_pVersionCheck != null) m_pVersionCheck.Update(fTime);
                    break;
                case EStartUpSection.AppAwake:
                    {
                        if (m_pFileSystem != null) m_pFileSystem.Update(fTime, true);
                        if (m_StartUpLoading != null) m_StartUpLoading.SetExternProgress((long)(ms_fLoadProgress * 100));
                    }
                    break;
                case EStartUpSection.AppStartUp:
                    {
                        Framework.Module.ModuleManager.getInstance().Update(fTime);
                    }
                    break;
            }
        }
        //------------------------------------------------------
        internal void FixedUpdate()
        {
            if (m_eSection == EStartUpSection.AppStartUp)
                Framework.Module.ModuleManager.getInstance().FixedUpdate(Time.fixedDeltaTime);
        }
        //------------------------------------------------------
        internal void LateUpdate()
        {
            if (m_eSection == EStartUpSection.AppStartUp)
                Framework.Module.ModuleManager.getInstance().LateUpdate(Time.deltaTime);
        }
    }
}
