using Framework.Core;
using System;
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Logic;
using UnityEngine;
using UnityEngine.Playables;

namespace TopGame.Timeline
{
    public class EventPlayableBehavior : PlayableBehaviour, Framework.Plugin.AT.IUserData
    {
        struct UpdateAble
        {
            public AInstanceAble pAble;
            public Transform pSlot;
            public Vector3 offset;
            public Vector3 euler;
        }
        private EventTrackAsset m_pTrackAsset = null;
        private bool m_bDestroyed = false;
        private Transform m_pBinder = null;
        private List<BaseEventParameter> m_vEvents = new List<BaseEventParameter>();
        private List<UpdateAble> m_vUpdateLists = null;
        //------------------------------------------------------
        public void AddEvent(BaseEventParameter pEvent, bool bClear = false)
        {
            if (pEvent == null) return;
            if (bClear) m_vEvents.Clear();
            m_vEvents.Add(pEvent);
        }
        //------------------------------------------------------
        public void SetBinder(Transform pBinder)
        {
            m_pBinder = pBinder;
        }
        //------------------------------------------------------
        public EventTrackAsset GetTrackAsset()
        {
            return m_pTrackAsset;
        }
        //------------------------------------------------------
        public void SetTrackAsset(EventTrackAsset pPointer)
        {
            m_pTrackAsset = pPointer;
        }
        //------------------------------------------------------
        public void Destroy()
        {
            if (m_vUpdateLists != null)
            {
                for (int i = 0; i < m_vUpdateLists.Count; ++i)
                {
                    FileSystemUtil.DeSpawnInstance(m_vUpdateLists[i].pAble);
                }
                m_vUpdateLists.Clear();
            }
            m_bDestroyed = true;
        }
        //------------------------------------------------------
        public override void OnPlayableCreate(Playable playable)
        {
            base.OnPlayableCreate(playable);
            Destroy();
        }
        //------------------------------------------------------
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            Destroy();
            m_bDestroyed = false;
            base.OnBehaviourPlay(playable, info);
            AEventSystemTrigger trigger = null;
            if (Framework.Module.ModuleManager.mainModule != null)
            {
                Framework.Module.AFramework framework = Framework.Module.ModuleManager.GetMainFramework<Framework.Module.AFramework>();
                if(framework!=null) trigger = framework.eventSystem;
            }
            if (trigger == null) return;
            trigger.Begin();
            trigger.ATuserData = this;
            if (m_pBinder != null)
            {
                trigger.TriggerEventPos = m_pBinder.position;
                trigger.TriggerEventRealPos = m_pBinder.position;
            }

            if(trigger.GetFramework()!=null)
            {
                for (int i = 0; i < m_vEvents.Count; ++i)
                {
                    if ( trigger.GetFramework().CheckerRandom(m_vEvents[i].triggerRate))
                    {
                        trigger.OnTriggerEvent(m_vEvents[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_vEvents.Count; ++i)
                {
                    if (m_vEvents[i].triggerRate >= UnityEngine.Random.Range(0, 1))
                    {
                        trigger.OnTriggerEvent(m_vEvents[i]);
                    }
                }
            }

            trigger.End();
#if UNITY_EDITOR
            if(!Framework.Module.ModuleManager.startUpGame)
                FileSystemUtil.Update(0, false);
#endif
        }
        //------------------------------------------------------
        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
            Destroy();
        }
        //------------------------------------------------------
        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
            Destroy();
        }
        //------------------------------------------------------
        public override void OnGraphStop(Playable playable)
        {
            base.OnGraphStop(playable);
            Destroy();
        }
        //------------------------------------------------------
        public void OnSpawnSign(InstanceOperiaon pCb)
        {
            pCb.bUsed = !m_bDestroyed;
        }
        //------------------------------------------------------
        public void OnSpawnBehaviour(InstanceOperiaon pCb)
        {
            if (pCb.pPoolAble == null) return;
            if (m_vUpdateLists == null) m_vUpdateLists = new List<UpdateAble>(2);
            UpdateAble able = new UpdateAble();


             Variable3 offsetPos = (Variable3)pCb.userData0;
            Variable3 offsetEuler = (Variable3)pCb.userData1;
            VariableString strSlot = (VariableString)pCb.userData2;

            able.euler = offsetEuler.ToVector3();
            able.offset = offsetPos.ToVector3();

            Transform pBind = m_pBinder;
            if (!string.IsNullOrEmpty(strSlot.strValue))
            {
                pBind = DyncmicTransformCollects.FindTransformByName(strSlot.strValue);
            }
            able.pSlot = pBind;
            able.pAble = pCb.pPoolAble;
            able.pAble.SetPosition(pBind!=null?(pBind.position+able.offset):able.offset);
            able.pAble.SetEulerAngle(pBind != null ? (pBind.eulerAngles + able.euler) : able.euler);
            m_vUpdateLists.Add(able);
        }
        //------------------------------------------------------
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if(m_pBinder == null)
            {
                m_pBinder = playerData as Transform;
                if (m_pTrackAsset && m_pTrackAsset.GetBinder() == null)
                    m_pTrackAsset.SetBinder(m_pBinder);
            }
            if(m_pTrackAsset!=null) m_pTrackAsset.ProcessFrame(playable, info);

            if (m_vUpdateLists!=null)
            {
                if (m_pTrackAsset.GetBinder() == null)
                    m_pTrackAsset.SetBinder(playerData as Transform);

                for (int i = 0; i < m_vUpdateLists.Count; ++i)
                {
                    UpdateAble able = m_vUpdateLists[i];
                    if(able.pAble == null) continue;
                    able.pAble.SetPosition(able.offset);
                    able.pAble.SetEulerAngle(able.euler);

                    if (able.pSlot == null) continue;
                    able.pAble.SetPosition(able.pSlot.position+able.offset);
                    able.pAble.SetEulerAngle(able.pSlot.eulerAngles + able.euler);
                    able.pAble.SetScale(able.pSlot.lossyScale);
                }
            }
            base.ProcessFrame(playable, info, playerData);
        }
    }
}