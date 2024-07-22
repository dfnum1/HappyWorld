/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GameFramework
作    者:	HappLI
描    述:	
*********************************************************************/
using System;
using UnityEngine;
using Framework.Module;
using System.Collections.Generic;
using Framework.Core;
using Framework.Base;
using Framework.Plugin.AT;
using Framework;

namespace TopGame.Core
{
    public abstract class GameFramework : AFrameworkModule, ISceneCallback,
        IParticleCallback, IAnimPathCallback, ITimelineCallback, ITweenEffectCallback
    {
        protected LODNodeManager m_pLodMgr = null;
        protected ARedDotManager m_pRedDotMgr = null;
        protected Logic.AUnLockManager m_pUnlockMgr = null;
        protected UI.IItemTweenManager m_pItemTweenMgr;
        protected ALocalizationManager m_pLocalizationMgr = null;
        protected AUserActionManager m_pUserActionMgr = null;
        //------------------------------------------------------
        public virtual LODNodeManager lodMgr
        {
            get { return m_pLodMgr; }
        }
        public ARedDotManager redDotManager
        {
            get { return m_pRedDotMgr; }
        }
        public Logic.AUnLockManager unlockMgr
        {
            get { return m_pUnlockMgr; }
        }
        public virtual UI.IItemTweenManager itemTweenMgr
        {
            get { return m_pItemTweenMgr; }
        }
        public virtual ALocalizationManager localizationMgr
        {
            get { return m_pLocalizationMgr; }
        }
        public virtual AUserActionManager userActionMgr
        {
            get { return m_pUserActionMgr; }
        }
        //------------------------------------------------------
        protected override bool OnAwake(IFrameworkCore plusSetting)
        {
            base.OnAwake(plusSetting);
            m_pLodMgr = RegisterModule<LODNodeManager>();
            m_pTerrainManager = RegisterModule<TerrainManager>();
            return true;
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (m_pLodMgr != null) m_pLodMgr.Destroy();
            m_pLodMgr = null;
        }
        //------------------------------------------------------
        public virtual void OnAnimPathBegin(IPlayableBase playAble)
        {
        }
        //------------------------------------------------------
        public virtual void OnAnimPathEnd(IPlayableBase playAble)
        {
        }
        //------------------------------------------------------
        public virtual void OnAnimPathUpdate(IPlayableBase playAble)
        {
        }
        //------------------------------------------------------
        public virtual void OnParticleStop(AInstanceAble pObject)
        {
        }
        //------------------------------------------------------
        public virtual void OnSceneCallback(SceneParam param)
        {
        }
        //------------------------------------------------------
        public virtual void OnTimelineStop(AInstanceAble pObject)
        {
        }
        //------------------------------------------------------
        public virtual void OnTweenEffectCompleted(VariablePoolAble pAble)
        {
        }
        //------------------------------------------------------
        public virtual bool OnUIWidgetTrigger(UI.EventTriggerListener pTrigger, UnityEngine.EventSystems.BaseEventData eventData, Base.EUIEventType triggerType, int guid, int listIndex, params Framework.Plugin.AT.IUserData[] argvs)
        {
            return false;
        }
        //------------------------------------------------------
        public virtual IDSoundData GetSoundByID(uint nId)
        {
            return IDSoundData.NULL;
        }
    }
}

