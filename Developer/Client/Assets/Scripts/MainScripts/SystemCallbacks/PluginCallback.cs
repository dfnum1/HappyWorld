/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GameInstance
作    者:	HappLI
描    述:	游戏主线程
*********************************************************************/

using TopGame.Core;
using TopGame.Data;
using UnityEngine;
using System;
using Framework.Plugin.AI;
using Framework.Plugin.AT;
using Proto3;
#if !USE_SERVER
using Framework.Plugin.Guide;
using TopGame.UI;
#endif
using Framework.Core;

namespace TopGame
{
    public partial class GameInstance :
         Framework.Plugin.AI.AISystemCallback
#if !USE_SERVER
        , Framework.Plugin.AT.AgentTreeDoActionCallback
        , Framework.Plugin.Guide.IGuideSystemCallback
#endif
    {
        //------------------------------------------------------
        public int OnGetAITypeArgvCount(int type, bool bMustReq = false)
        {
            return Framework.Plugin.AI.AIUtil.GetTypeArgvCount(type, bMustReq);
        }
        //------------------------------------------------------
        public IUserData OnBuildEvent(string strEvent)
        {
            if (string.IsNullOrEmpty(strEvent)) return null;
            return BaseEventParameter.NewEvent(this, strEvent);
        }
        //------------------------------------------------------
        public bool OnTriggerAIEvent(AAIProcesser aiAgent, IUserData pTrigger, IUserData pEvent, int triggeGUID=0)
        {
            if (pEvent == null ) return false;
            BaseEventParameter eventParam = pEvent as BaseEventParameter;
            if (eventParam == null) return false;
            if (!CheckerRandom(eventParam.triggerRate)) return false;
            if (triggeGUID != 0) pTrigger = m_pWorld.FindNode<AWorldNode>(triggeGUID);
            m_pEventTrigger.Begin();
            m_pEventTrigger.ATuserData = pTrigger;
            if (pTrigger is AWorldNode)
            {
                AWorldNode pActor = pTrigger as AWorldNode;
                m_pEventTrigger.TriggerEventPos = pActor.GetPosition();
                m_pEventTrigger.TriggerEventRealPos = pActor.GetPosition();
                m_pEventTrigger.TriggerActorDir = pActor.GetDirection();
            }

            IUserData catchData = aiAgent.GetCatchData(typeof(Framework.Data.StateParam));
            if (catchData != null)
                m_pEventTrigger.TriggerActorActionStateParam = catchData as Framework.Data.StateParam;
            catchData = aiAgent.GetCatchData(typeof(Framework.Core.ActionState));
            if (catchData != null)
                m_pEventTrigger.TriggerActorActionState = catchData as Framework.Core.ActionState;

            m_pEventTrigger.OnTriggerEvent(pEvent as BaseEventParameter);
            m_pEventTrigger.End();
            return true;
        }
        //------------------------------------------------------
        public bool OnExcudeAINode(AAIProcesser pAgent, AIBase pNode)
        {
            if (pNode is AITimer)
                return Framework.BattlePlus.AITimerExcude.OnExcude(this, pAgent, pNode as AITimer);
            else if (pNode is AIExcude)
                return Framework.BattlePlus.AIExcuderHandler.OnExcude(this, pAgent, pNode as AIExcude);
            else if (pNode is AINode)
            {
                if (pAgent.IsBindTrigger<Actor>())
                {
                    Actor pActor = pAgent.GetBindTrigger() as Actor;
                    pAgent.AddCatchData(pActor.GetStateParam());
                    pAgent.AddCatchData(pActor.GetCurrentActionState());
                }
                return Framework.BattlePlus.AINodeExcude.OnExcude(this, pAgent, pNode as AINode);
            }
            else if (pNode is AITask) return true;
            else if (pNode is AIEvent)
            {
                return true;
            }
            return false;
        }
        //------------------------------------------------------
        public void OnExcudeAINodeEnd(AAIProcesser pAgent, AIBase pNode)
        {
        }
        //------------------------------------------------------
        public void OnAIAgentActived(AIAgent pAgent, bool bActive)
        {
            Framework.BattlePlus.AIWrapper.DoActiveAIAI(pAgent, bActive?1:0);
        }
#if !USE_SERVER
        //------------------------------------------------------
        public bool OnAction(AgentTreeTask pTask, ActionNode pNode, int nFuncId)
        {
            bool bOk = Framework.Plugin.AT.AgentTreeUserClass_Func.DoInerAction(pTask, pNode, nFuncId);
#if UNITY_EDITOR
            if (!bOk)
            {
                Debug.LogError("AT:Function:" + pNode.strName + "   不存在!");
            }
#endif
            return bOk;
        }
        //------------------------------------------------------
        public bool OnFillCustomVariable(AgentTreeTask pTask, Variable pCustomVariable, IUserData pData)
        {
            return Framework.Plugin.AT.AgentTreeFillVariable_Func.OnFillCustomVariable(pTask, pCustomVariable, pData);
        }
        //------------------------------------------------------
        public int GetClassHashCode(Type classType)
        {
            return Framework.Plugin.AT.AgentTree_ClassTypes.ClassTypeToHash(classType);
        }
        //------------------------------------------------------
        public int GetParentHashCode(int hashCode)
        {
            return Framework.Plugin.AT.AgentTree_ClassTypes.HashToParentHash(hashCode);
        }
        //------------------------------------------------------
        public System.Type GetHashCodeClass(int hashCode)
        {
            return Framework.Plugin.AT.AgentTree_ClassTypes.HashToClassType(hashCode);
        }
        //------------------------------------------------------
        public bool OnTriggerGuideEvent(IUserData pEvent, IUserData pTrigger)
        {
            if (pEvent == null) return false;
            BaseEventParameter eventParam = pEvent as BaseEventParameter;
            if (eventParam == null) return false;
            if (!CheckerRandom(eventParam.triggerRate)) return false;

            m_pEventTrigger.Begin();
            m_pEventTrigger.ATuserData = pTrigger;
            m_pEventTrigger.TriggerEventPos = GetPlayerPosition();
            m_pEventTrigger.TriggerEventRealPos = GetPlayerPosition();
            m_pEventTrigger.OnTriggerEvent(pEvent as BaseEventParameter);
            m_pEventTrigger.End();
            return true;
        }
        //------------------------------------------------------
        public bool OnGuideSuccssedListener(Framework.Plugin.Guide.BaseNode pNode)
        {
            if (pNode is Framework.Plugin.Guide.StepNode)
                return Framework.Plugin.Guide.GuideStepHandler.OnGuideSuccssedListener(pNode as Framework.Plugin.Guide.StepNode);
            return true;
        }
        //------------------------------------------------------
        public void OnGuideStatus(int guid, bool bDoing)
        {
            UIBase uIBase = uiManager.GetUI((ushort)EUIType.GuidePanel);
            if (uIBase == null)
            {
                return;
            }
            GuidePanel guidePanel = uIBase as GuidePanel;
            if (bDoing)
            {
                guidePanel.ResetData();//引导结束时清空缓存
                guidePanel.Show();
            //    AUserActionManager.AddRecordAction((int)UserActionTypeCode.NoviceGuidance, guid, 1, 1, true);
            }
            else
            {
                guidePanel.bDoing = false;
                guidePanel.Hide();
            //    AUserActionManager.AddRecordAction((int)UserActionTypeCode.NoviceGuidance, guid, 1, 2, true);

                //引导结束触发检测,自己的引导结束不触发引导结束触发器,记录最后一次触发的引导组id,作为比较
                if (GuideSystem.getInstance().LastGroupGuid != guid)
                {
                    GuideSystem.getInstance().LastGroupGuid = guid;
                    Framework.Plugin.Guide.GuideWrapper.OnCompleteGudie(guid, 1 << (int)GetState());//在执行新的触发器的时候,会进行判断,如果当前有正在执行的触发器,会调用一次结束引导组的操作,就会导致递归
                }
                
            }

            if (DebugConfig.bGuideLogEnable)
            {
                Framework.Plugin.Logger.Warning("OnGuideStatus:" + guid + ",status=" + bDoing);
            }
        }
        //------------------------------------------------------
        public bool OnGuideSign(Framework.Plugin.Guide.BaseNode pNode, Framework.Plugin.Guide.CallbackParam param)
        {
            if (pNode is Framework.Plugin.Guide.StepNode)
                return Framework.Plugin.Guide.GuideStepHandler.OnGuideSign(pNode as Framework.Plugin.Guide.StepNode, param);
            if (pNode is Framework.Plugin.Guide.TriggerNode)
                return Framework.Plugin.Guide.GuideTriggerHandler.OnGuideSign(pNode as Framework.Plugin.Guide.TriggerNode, param);
            if (pNode is Framework.Plugin.Guide.ExcudeNode)
                return Framework.Plugin.Guide.GuideExcudesHandler.OnGuideSign(pNode as Framework.Plugin.Guide.ExcudeNode, param);
            return false;
        }
        //------------------------------------------------------
        public void OnGuideNode(Framework.Plugin.Guide.BaseNode pNode)
        {
            UIBase uIBase = uiManager.GetUI((ushort)EUIType.GuidePanel);
            
            if (pNode is Framework.Plugin.Guide.StepNode)
            {
                GuidePanel guidePanel = uIBase as GuidePanel;
                if (guidePanel != null)
                {
                    guidePanel.ClearWidget();
                }
                Framework.Plugin.Guide.GuideStepHandler.OnGuideNode(pNode as Framework.Plugin.Guide.StepNode);
            }
            if (pNode is Framework.Plugin.Guide.TriggerNode)
                Framework.Plugin.Guide.GuideTriggerHandler.OnGuideNode(pNode as Framework.Plugin.Guide.TriggerNode);
            if (pNode is Framework.Plugin.Guide.ExcudeNode)
                Framework.Plugin.Guide.GuideExcudesHandler.OnGuideNode(pNode as Framework.Plugin.Guide.ExcudeNode);
        }
        //------------------------------------------------------
        public void OnNodeAutoNext(Framework.Plugin.Guide.BaseNode pNode)
        {
            if (pNode is Framework.Plugin.Guide.StepNode)
                Framework.Plugin.Guide.GuideStepHandler.OnNodeAutoNext(pNode as Framework.Plugin.Guide.StepNode);
            if (pNode is Framework.Plugin.Guide.TriggerNode)
                Framework.Plugin.Guide.GuideTriggerHandler.OnNodeAutoNext(pNode as Framework.Plugin.Guide.TriggerNode);
            if (pNode is Framework.Plugin.Guide.ExcudeNode)
                Framework.Plugin.Guide.GuideExcudesHandler.OnNodeAutoNext(pNode as Framework.Plugin.Guide.ExcudeNode);
        }
#endif
    }
}
