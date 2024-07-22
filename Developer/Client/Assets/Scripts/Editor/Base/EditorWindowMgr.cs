/********************************************************************
生成日期:	17:9:2019   10:09
类    名: 	EditorWindowMgr
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Module;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    //-------------------------------------------
    //! EditorWindowBase
    //-------------------------------------------
    public abstract class EditorWindowBase : EditorWindow
    {
        private bool m_bRuntimingOpened = false;
        void OnEnable()
        {
            m_bRuntimingOpened = Framework.Module.ModuleManager.startUpGame;
            EditorWindowMgr.RegisterWindow(this);
            if (Framework.Module.ModuleManager.mainModule == null || !(Framework.Module.ModuleManager.mainModule is GameInstance))
                Core.FileSystem.bEditorMode = true;
            OnInnerEnable();
            EditorKits.StopAllAudioClips();

        }
        void OnDisable()
        {
            EditorWindowMgr.UnRegisterWindow(this);
            OnInnerDisable();
            if(IsManaged() && !m_bRuntimingOpened)
            {
                try
                {
                    Framework.Module.ModuleManager.getInstance().ShutDown();
                    EditorApplication.isPlaying = false;
                }
                catch (System.Exception ex)
                {

                }
            }
            m_bRuntimingOpened = false;
        }

        protected virtual void OnInnerEnable() { }
        protected virtual void OnInnerDisable() { }

        public virtual int GetPriority() { return 0; }
        public virtual bool IsManaged() { return true; }
        public virtual bool IsRuntimeOpen() { return m_bRuntimingOpened; }

        private float m_PreviousTime;
        public float deltaTime = 0.02f;
        public float fixedDeltaTime = 0.02f;
        protected float m_fDeltaTime = 0f;
        protected float m_currentSnap = 1f;
        //-----------------------------------------------------
        protected virtual void Update()
        {
            if (Application.isPlaying)
            {
                Application.targetFrameRate = 30;
                deltaTime = Time.deltaTime;
                m_fDeltaTime = (float)(deltaTime * m_currentSnap);
            }
            else
            {
                float curTime = Time.realtimeSinceStartup;
                m_PreviousTime = Mathf.Min(m_PreviousTime, curTime);//very important!!!

                deltaTime = curTime - m_PreviousTime;
                m_fDeltaTime = (float)(deltaTime * m_currentSnap);
            }

           // if (m_fDeltaTime >= 1 / 30f) m_fDeltaTime = 1 / 30f;

            m_PreviousTime = Time.realtimeSinceStartup;
            ModuleManager.UpdateSingle(ModuleManager.editorModule, deltaTime);
            this.Repaint();
        }
    }
    //-------------------------------------------
    public class SingleEditorWindow<T> : EditorWindowBase where T : EditorWindow
    {
        public static T Instance;

        public static void Open(string title, Vector2 size)
        {
            Instance = EditorWindow.GetWindow<T>();
            Instance.Show();

            if (Instance.position.x < 0 || Instance.position.y < 0
                || Instance.position.x > 1920 || Instance.position.y > 1080)
            {
                Instance.position = new Rect(0, 0, size.x, size.y);
            }

            //Instance.ReadPathFile();
            Instance.titleContent.text = title;
            Instance.minSize = size;
        }
    }
    //-------------------------------------------
    //! EditorWindowMgr
    //-------------------------------------------
    public class EditorWindowMgr
    {
        static List<EditorWindowBase> m_vList = new List<EditorWindowBase>();
        static List<EditorWindowBase> m_vRuntimeOpenList = new List<EditorWindowBase>();
        //-------------------------------------------
        static public void RegisterWindow(EditorWindowBase window)
        {
            if (!window.IsManaged()) return;
            if(window.IsRuntimeOpen())
            {
                if (m_vRuntimeOpenList.Contains(window)) return;
                m_vRuntimeOpenList.Add(window);
                return;
            }
            if (m_vList.Contains(window)) return;
            m_vList.Add(window);
        }
        //-------------------------------------------
        static public void UnRegisterWindow(EditorWindowBase window)
        {
            if (ms_bCloseLock) return;
            m_vList.Remove(window);
            m_vRuntimeOpenList.Remove(window);
        }
        //-------------------------------------------
        static bool ms_bCloseLock = false;
        static public bool CloseAll()
        {
            if(m_vList.Count>0)
            {
                if(!EditorUtility.DisplayDialog("提示", "当前有开着的编辑器，是否确认保存，再继续？", "继续", "保存后再继续吧"))
                {
                    return false;
                }

                ms_bCloseLock = true;
                for (int i = 0; i < m_vList.Count; ++i)
                {
                    if (m_vList[i]) m_vList[i].Close();
                }
                ms_bCloseLock = false;
                m_vList.Clear();
            }
            if (m_vRuntimeOpenList.Count > 0)
            {
                ms_bCloseLock = true;
                for (int i = 0; i < m_vRuntimeOpenList.Count; ++i)
                {
                    if (m_vRuntimeOpenList[i]) m_vRuntimeOpenList[i].Close();
                }
                ms_bCloseLock = false;
                m_vRuntimeOpenList.Clear();
            }
            return true;
        }
    }
}
