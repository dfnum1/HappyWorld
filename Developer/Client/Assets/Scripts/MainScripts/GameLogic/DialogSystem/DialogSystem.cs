/********************************************************************
生成日期:	2020-06-12
类    名: 	DialogSystem
作    者:	happli
描    述:	对话系统
*********************************************************************/
using UnityEngine;

namespace TopGame.Logic
{
    public enum EDialogTriggerType
    {
        ShowUI = 1, //打开什么界面
        HideUI = 2, //关闭什么界面

        EnterPVE = 10, //进入副本，关卡ID

        EnterStateMode = 100,   //进入某种模式，比如roguelite,islandworld
    }
    public class DialogSystem : Framework.Module.AModule, Framework.Module.IStartUp, Framework.Module.IUpdate
    {
        static DialogSystem ms_pDialogSystem = null;
        Framework.Plugin.AT.AgentTree m_pAgentTree = null;
        //------------------------------------------------------
        public static DialogSystem getInstance()
        {
            return ms_pDialogSystem;
        }
        //------------------------------------------------------
        protected override void Awake()
        {
            ms_pDialogSystem = this;
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            ms_pDialogSystem = null;
        }
        //------------------------------------------------------
        public void StartUp()
        {
            ms_pDialogSystem = this;
            if (m_pAgentTree != null)
                Framework.Plugin.AT.AgentTreeManager.getInstance().UnloadAT(m_pAgentTree);
            m_pAgentTree = Framework.Plugin.AT.AgentTreeManager.getInstance().LoadAT(Data.ATModuleSetting.DialogSystemAT);
            if (m_pAgentTree != null)
            {
                m_pAgentTree.AddOwnerClass(this);
                m_pAgentTree.Enable(true);
                m_pAgentTree.Enter();
            }
        }
        //------------------------------------------------------
        public void Update(float fFrame)
        {

        }
        //------------------------------------------------------
        public static void OnSoundATEvent(string evtName, Framework.Plugin.AT.IUserData pParam = null)
        {
            if (ms_pDialogSystem == null) return;
            if (ms_pDialogSystem.m_pAgentTree == null) return;
            ms_pDialogSystem.m_pAgentTree.ExecuteCustom(evtName, pParam);
        }
        //------------------------------------------------------
        public static void OnSoundATEvent(int evtID, Framework.Plugin.AT.IUserData pParam = null)
        {
            if (ms_pDialogSystem == null) return;
            if (ms_pDialogSystem.m_pAgentTree == null) return;
            ms_pDialogSystem.m_pAgentTree.ExecuteCustom(evtID, pParam);
        }
        //------------------------------------------------------
        public static void OnSoundATEvent(GameObject pGO, Framework.Plugin.AT.IUserData pParam = null)
        {
            if(ms_pDialogSystem == null) return;
            if (ms_pDialogSystem.m_pAgentTree == null) return;
            ms_pDialogSystem.m_pAgentTree.ExecuteCustom(pGO, pParam);
        }
    }
}

