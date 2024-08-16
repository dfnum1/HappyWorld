/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GameInstance
作    者:	HappLI
描    述:	游戏主线程
*********************************************************************/
using TopGame.Data;
using UnityEngine;
using Framework.Core;

namespace TopGame
{
    public partial class GameInstance
    {
        //------------------------------------------------------
        protected override IEventParameter OnMallocEventParameter(int eventType)
        {
            return Framework.Core.EventMallocHandler.NewEvent(eventType);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("触发指定ID事件-触发者")]
        public override void TriggertEvent(int nEventID, Framework.Plugin.AT.IUserData pTrigger, Framework.Plugin.AT.IUserData pTarget)
        {
            if (m_pEventTrigger == null) return;
            m_pEventTrigger.Begin();
            m_pEventTrigger.ATuserData = pTarget;
            m_pEventTrigger.TriggerEventPos = GetPlayerPosition();
            m_pEventTrigger.TriggerEventRealPos = GetPlayerPosition();
            if(pTrigger!=null && pTrigger is AWorldNode)
                m_pEventTrigger.pParentNode = pTrigger as AWorldNode;
            if (pTarget != null)
            {
                if (pTarget is AWorldNode)
                {
                    AWorldNode pActor = (pTarget as AWorldNode);
                    m_pEventTrigger.TriggerEventPos = pActor.GetPosition();
                    m_pEventTrigger.TriggerEventRealPos = m_pEventTrigger.TriggerEventPos;
                    m_pEventTrigger.TriggerActorDir = pActor.GetDirection();
                }
            }
            OnTriggerEvent(nEventID, false);
            m_pEventTrigger.End();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("触发指定ID事件")]
        public override void TriggertEvent(int nEventID, Framework.Plugin.AT.IUserData pUseData)
        {
            if (m_pEventTrigger == null) return;
            m_pEventTrigger.Begin();
            m_pEventTrigger.ATuserData = pUseData;
            m_pEventTrigger.TriggerEventPos = GetPlayerPosition();
            m_pEventTrigger.TriggerEventRealPos = GetPlayerPosition();
            if(pUseData!=null)
            {
                if (pUseData is AWorldNode)
                {
                    AWorldNode pActor = (pUseData as AWorldNode);
                    m_pEventTrigger.TriggerEventPos = pActor.GetPosition();
                    m_pEventTrigger.TriggerEventRealPos = m_pEventTrigger.TriggerEventPos;
                    m_pEventTrigger.TriggerActorDir = pActor.GetDirection();
                }
            }
            OnTriggerEvent(nEventID, false);
            m_pEventTrigger.End();
        }
        //------------------------------------------------------
        public override void OnTriggerEvent(int nEventID, bool bAutoClear = true)
        {
            if (m_pEventTrigger == null) return;
            if (DataManager.getInstance() == null) return;
            if (bAutoClear)
            {
				m_pEventTrigger.Begin();
                m_pEventTrigger.TriggerEventPos = GetPlayerPosition();
                m_pEventTrigger.TriggerEventRealPos = GetPlayerPosition();
            }
            EventData pEvent = DataManager.getInstance().EventDatas.GetData(nEventID);
            if (pEvent == null || pEvent.events ==null) return;

            Vector3 TriggerEventPos = m_pEventTrigger.TriggerEventPos;
            Vector3 TriggerEventRealPos = m_pEventTrigger.TriggerEventRealPos;
            if(pEvent.groupRandom)
            {
                BaseEventParameter evt =   pEvent.GetRandomGroup(this);
                if (evt!=null && CheckerRandom(evt.triggerRate))
                    OnTriggerEvent(evt, false);
            }
            else
            {
                for (int i = 0; i < pEvent.events.Count; ++i)
                {
                    m_pEventTrigger.TriggerEventPos = TriggerEventPos;
                    m_pEventTrigger.TriggerEventRealPos = TriggerEventRealPos;
                    if (CheckerRandom(pEvent.events[i].triggerRate))
                        OnTriggerEvent(pEvent.events[i], false);
                }
            }

            if (bAutoClear) m_pEventTrigger.End();
        }
    }
}
