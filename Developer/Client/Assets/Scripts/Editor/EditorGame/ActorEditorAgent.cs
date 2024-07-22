//auto generator
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using TopGame.Data;
using TopGame.Core;
using System.Reflection;
using Framework.Core;
using Framework.Logic;
using Framework.BattlePlus;
using ExternEngine;
using Framework.ED;

namespace TopGame.ED
{
    [Framework.Plugin.PluginDisiableExport]
	public class ActorEditorAgent : GameActorAgent
    {
        public bool bUseActionDo = false;

        private Actor m_DummyTarget= null;
        TargetPreview m_pPreview = null;

        List<AWorldNode> m_vLockTarget = new List<AWorldNode>();
        //------------------------------------------------------
        public Vector3 GetTeamPosition()
        {
            return m_pActor.GetPosition();
        }
        //------------------------------------------------------
        public override void OnTriggerEvent(ActionState pState, ActionEvent pEvent)
        {
            if (!m_pActor.GetGameModule().CheckerRandom(pEvent.m_pCore.triggerRate))
                return;
            AFrameworkModule pGameModule = m_pActor.GetGameModule() as AFrameworkModule;
            if (pGameModule == null) return;
            if(m_DummyTarget != null)
                AddLockTarget(m_DummyTarget, true);
            
            if (pEvent.m_pCore is SoundEventParameter)
            {
                pEvent.bSkip = true;
                SoundEventParameter param = pEvent.m_pCore as SoundEventParameter;
                if (param.soundType == ESoundType.StopEffect)
                    Framework.Core.AudioUtil.StopEffect(param.strFile);
                else if (param.soundType == ESoundType.StopBG)
                    Framework.Core.AudioUtil.StopBG(param.strFile);
                else
                {
                    Framework.Core.AudioUtil.PlayEffect(param.strFile);
                }
            }
            else if (pEvent.m_pCore is TakeSlotSkillEventParameter)
            {
                pEvent.bSkip = true;
            }
            else if(pEvent.m_pCore is CameraEventParameter)
            {
                pEvent.bSkip = true;
                pEvent.m_pCore.PlayPreview(true);
                Framework.ED.DrawEventCore.AddEditEvent(pEvent.m_pCore);
            }
            else if (pEvent.m_pCore is CameraCloseUpEventParameter)
            {
                pEvent.bSkip = true;
                pEvent.m_pCore.PlayPreview(true);
                Framework.ED.DrawEventCore.AddEditEvent(pEvent.m_pCore);
            }
            else if (pEvent.m_pCore is CameraShakeParameter)
            {
                pEvent.bSkip = true;
                pEvent.m_pCore.PlayPreview(true);
                Framework.ED.DrawEventCore.AddEditEvent(pEvent.m_pCore);
            }
            base.OnTriggerEvent(pState, pEvent);

            if (pEvent.pObject != null && m_pPreview != null)
                m_pPreview.AddSingleObject(pEvent.pObject.GetObject());
        }
        //------------------------------------------------------
        public override void OnStopEvent(ActionState pState, ActionEvent pEvent)
        {
            base.OnStopEvent(pState, pEvent);
        }
        //------------------------------------------------------
        public override void Reset()
        {
            base.Reset();
        }
        //------------------------------------------------------
        protected override void InnerUpdate(FFloat fTime)
        {
            base.InnerUpdate(fTime);
            m_pActor.SetDirection(Vector3.forward);
        }
        //------------------------------------------------------
        protected override void PlayAction(ActionState pAction)
        {
            if (bUseActionDo) base.PlayAction(pAction);
        }
        //------------------------------------------------------
        public void SyncPreview(TargetPreview preview)
        {
            m_pPreview = preview;
        }
        //------------------------------------------------------
        public void SetDummyTarget(Actor dummyTarget)
        {
            m_DummyTarget = dummyTarget;
        }
        //------------------------------------------------------
        public override void AddLockTarget(AWorldNode pActor, bool bClear = false, bool bLogic =true)
        {
            if(bClear) m_vLockTarget.Clear();
            m_vLockTarget.Add(pActor);
        }
        //------------------------------------------------------
        public override List<AWorldNode> GetLockTargets(bool isEmptyReLock=true)
        {
            return m_vLockTarget;
        }
    }
}
