/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GameInstance
作    者:	HappLI
描    述:	游戏主线程
*********************************************************************/

using System.Collections.Generic;
using TopGame.Base;
using TopGame.Core;
using TopGame.Data;
using TopGame.Logic;
using Framework.Module;
using UnityEngine;
using TopGame.Net;
using System;
using Framework.Plugin.AT;
using Framework.Plugin.AI;
using Framework.Plugin.Guide;
using TopGame.UI;
using TopGame.SvrData;
using Framework.Core;
using Framework.Base;

namespace TopGame
{
    public partial class GameInstance
    {
        public Action<Actor, EAttrType, float, bool> OnBossAttrChangeCB;


        //------------------------------------------------------
        public override IDSoundData GetSoundByID(uint nId)
        {
            if (Data.DataManager.getInstance().Audio == null) return IDSoundData.NULL;
            List<Data.CsvData_Audio.AudioData> audioData = Data.DataManager.getInstance().Audio.GetData(nId);
            if (audioData != null && audioData.Count > 0)
            {
                var audio = audioData[UnityEngine.Random.Range(0, audioData.Count)];
                IDSoundData soundData = new IDSoundData();
                soundData.id = audio.id;
                soundData.group = audio.group;
                soundData.type = audio.type;
                soundData.location = audio.location;
                soundData.channel = audio.channel;
                soundData.volume = audio.volume;
                soundData.b3D = audio.b3D;
                return soundData;
            }
            return IDSoundData.NULL;
        }
        //------------------------------------------------------
        public override BodyPartParameter OnCreateBodyPart(Actor pActor, IBodyPart pBodyPart)
        {
            //             if (Data.DataManager.getInstance().Part == null) return null;
            //             CsvData_Part.PartData partData = Data.DataManager.getInstance().Part.GetData(pBodyPart.GetBodyID());
            //             if (partData == null) return null;
            //             BodyPartParameter part = BodyPartParameter.Malloc();
            //             part.Clear();
            //             part.pBodyPart = pBodyPart;
            //             part.hp_rate = (int)partData.nHPRate;
            //             part.max_hp_rate = part.hp_rate;
            //             part.damage_rate = partData.nDamageRate * Framework.Core.CommonUtility.WAN_RATE;
            //             part.rodeLines = partData.rodeLines;
            // 
            //             if (GlobalSetting.AlwayShowPartAimPoint)
            //             {
            //                 if(pBodyPart.GetState() == EBodyPartState.Enable)
            //                     part.UpdateAimPoint(PermanentAssetsUtil.GetAssetPath(EPathAssetType.PartAimPoint));
            //             }
            // 
            //             Framework.BattlePlus.AIWrapper.DoAddPartAI(pActor.GetAI(), (int)pBodyPart.GetBodyID());
            //            return part;
            return null;
        }
        //------------------------------------------------------
        public override void OnBodyPartState(Actor pActor, BodyPartParameter part)
        {
            if(part.IsBroken())
                Framework.BattlePlus.AIWrapper.DoBrokenPartAI(pActor.GetAI(), (int)part.body_id);

//             if (GlobalSetting.AlwayShowPartAimPoint)
//             {
//                 if (part.IsEnable())
//                 {
//                     part.UpdateAimPoint(PermanentAssetsUtil.GetAssetPath(EPathAssetType.PartAimPoint));
//                 }
//                 else
//                     part.UpdateAimPoint(null);
//             }


            base.OnBodyPartState(pActor, part);
        }
        //------------------------------------------------------
        public override void OnActorPopText(AWorldNode pActor, uint nCode, VariablePoolAble externVarial = null)
        {
            dynamicTexter.OnActorPopText(pActor, nCode, externVarial);
        }
        //------------------------------------------------------
        public override void OnActorAttrChange(Actor pActor, EAttrType type, int fValue, int fOriginal, bool bPlus, VariablePoolAble externVarial = null)
        {
            dynamicTexter.OnActorAttrChange(pActor, type, fOriginal, bPlus, externVarial);
            dynamicHudHper.OnActorAttrChange(pActor, type, fOriginal, bPlus);

            base.OnActorAttrChange(pActor, type, fValue, fOriginal, bPlus);
            if (type == EAttrType.MaxHp || type == EAttrType.HpRec || type == EAttrType.Crit)
            {
                int fMaxHp = pActor.GetActorParameter().GetRuntimeParam().max_hp;
                int addValue = 0;
                int dropValue = 0;

                int valueRate = 0;
                if (fMaxHp > 0)
                {
                    addValue = (int)(100 * pActor.GetActorParameter().GetStatAttr(EAttrType.MaxHp) / fMaxHp);
                    dropValue = (int)(100 * pActor.GetActorParameter().GetStatAttr(EAttrType.MaxHp, false) / fMaxHp);

                    valueRate = (int)(100 * (float)fValue / (float)fMaxHp);
                }
                int curRate = (int)(100*pActor.GetActorParameter().GetRuntimeParam().GetHpRate());
                if (bPlus)
                {
                    Framework.BattlePlus.AIWrapper.OnGlobalAppendHP(pActor.GetGameModule().aiSystem, null, pActor.GetInstanceID(), (int)pActor.GetActorParameter().GetRuntimeParam().hp, fValue, fMaxHp, curRate, dropValue, addValue, valueRate);
                    Framework.BattlePlus.AIWrapper.DoAppendHPAI(pActor.GetAI(), (int)pActor.GetActorParameter().GetRuntimeParam().hp, fValue, fMaxHp, curRate, dropValue, addValue, valueRate);
                }
                else
                {
                    Framework.BattlePlus.AIWrapper.OnGlobalDropHP(pActor.GetGameModule().aiSystem, null, pActor.GetInstanceID(), (int)pActor.GetActorParameter().GetRuntimeParam().hp, fValue, fMaxHp, curRate, dropValue, addValue, valueRate);
                    Framework.BattlePlus.AIWrapper.DoDropHPAI(pActor.GetAI(), (int)pActor.GetActorParameter().GetRuntimeParam().hp, fValue, fMaxHp, curRate, dropValue, addValue, valueRate);
                }
            }
            else if (type == EAttrType.MaxSp || type == EAttrType.SpRec)
            {
                int fMaxSp = pActor.GetActorParameter().GetRuntimeParam().max_sp;
                if (fMaxSp == 0) fMaxSp = 1;
                int addValue = 0;
                int dropValue = 0;
                if (fMaxSp>0)
                {
                    addValue = (int)(100 * pActor.GetActorParameter().GetStatAttr(EAttrType.MaxSp) / fMaxSp);
                    dropValue = (int)(100 * pActor.GetActorParameter().GetStatAttr(EAttrType.MaxSp, false) / fMaxSp);
                }
                int curRate = (int)(100 * pActor.GetActorParameter().GetRuntimeParam().GetSpRate());
                int valueRate = (int)(100 * (float)fValue / (float)fMaxSp);
                if (bPlus)
                {
                    Framework.BattlePlus.AIWrapper.DoAppendSPAI(pActor.GetAI(), (int)pActor.GetActorParameter().GetRuntimeParam().sp, fValue, fMaxSp, curRate, dropValue, addValue, valueRate);
                }
                else
                    Framework.BattlePlus.AIWrapper.DoDropSPAI(pActor.GetAI(), (int)pActor.GetActorParameter().GetRuntimeParam().sp, fValue, fMaxSp, curRate, dropValue, addValue, valueRate);
            }
            else if (type == EAttrType.Posture || type == EAttrType.PostureRec)
            {
                int MaxPosture = pActor.GetActorParameter().GetRuntimeParam().max_posture;
                int addValue = 0;
                int dropValue = 0;

                if (MaxPosture > 0)
                {
                    addValue = (int)(100 * pActor.GetActorParameter().GetStatAttr(EAttrType.Posture) / MaxPosture);
                    dropValue = (int)(100 * pActor.GetActorParameter().GetStatAttr(EAttrType.Posture, false) / MaxPosture);
                }
                int curRate = (int)(100 * pActor.GetActorParameter().GetRuntimeParam().GetPostureRate());
                int valueRate = (int)(100 * (float)fValue / (float)MaxPosture);
                int frenzy = (pActor.GetActorParameter().GetRuntimeParam().frenzy_trigger>1)?1:0;
                if (bPlus)
                {
                    Framework.BattlePlus.AIWrapper.DoAppendPostureAI(pActor.GetAI(), pActor.GetActorParameter().GetRuntimeParam().posture, fValue, MaxPosture, curRate, dropValue, addValue, valueRate, frenzy);
                }
                else
                    Framework.BattlePlus.AIWrapper.DoDropPostureAI(pActor.GetAI(), pActor.GetActorParameter().GetRuntimeParam().posture, fValue, MaxPosture, curRate, dropValue, addValue, valueRate, frenzy);
            }
            if (m_States!=null)
                m_States.OnActorAttrChange(pActor, type, fValue, fOriginal, bPlus);

            Logic.Monster monster = pActor as Logic.Monster;
            if (monster != null && monster.GetMonsterType() == EMonsterType.Boss)
            {
                OnBossAttrChangeCB?.Invoke(pActor, type, fValue, bPlus);
            }
        }
        //------------------------------------------------------
        public override void OnChangeElementFlag(AWorldNode pNode, uint nFlags)
        {
            if (pNode == null) return;
            Framework.Plugin.AT.AgentTreeManager.Execute((ushort)Base.EATEventType.UserDef, "ChangeElementFlags", pNode);
        }
        //------------------------------------------------------
        public override void OnTweenEffectCompleted(VariablePoolAble pAble)
        {
            if (pAble == null) return;
            if(pAble is PositionTween)
            {
                PositionTween posTween = pAble as PositionTween;
                Framework.Plugin.Guide.GuideWrapper.OnCustomCallback((int)Framework.Plugin.Guide.EGuideCustomType.WaitTweenCompleted, posTween.ID);
            }
        }
        //------------------------------------------------------
        public override bool OnUIWidgetTrigger(EventTriggerListener pTrigger, UnityEngine.EventSystems.BaseEventData eventData, Base.EUIEventType triggerType, int guid, int listIndex, params IUserData[] argvs)
        {
            Framework.Plugin.Guide.EUIWidgetTriggerType guideTriggerType = EUIWidgetTriggerType.None;
            switch (triggerType)
            {
                case Base.EUIEventType.onClick:
                    {
                        guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Click;
                        //用户行为收集
                        if (pTrigger.userActionData.IsValid)
                            AUserActionManager.AddRecordAction(pTrigger.userActionData);

                        //按钮解锁状态判断
                        if (pTrigger.btnUnLockData.isCheckLockState && pTrigger.btnUnLockData.listener != null && Core.DebugConfig.bEnableModuleLock)
                        {
                            if (!pTrigger.btnUnLockData.listener.OnClickBtn())
                            {
                                return true;   // skip 
                            }
                        }
                        if(guid <= 0)
                        {
                            Framework.Plugin.Guide.GuideWrapper.OnOptionStepCheck();    //非强制引导检测
                        }
                        break;
                    }
                case Base.EUIEventType.onDown: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Down; break;
                case Base.EUIEventType.onEnter: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Enter; break;
                case Base.EUIEventType.onExit: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Exit; break;
                case Base.EUIEventType.onUp: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Select; break;
                case Base.EUIEventType.onSelect: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Select; break;
                case Base.EUIEventType.onUpdateSelect: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.UpdateSelect; break;
                case Base.EUIEventType.onDrag: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Drag; break;
                case Base.EUIEventType.onDrop: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Drop; break;
                case Base.EUIEventType.onDeselect: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Deselect; break;
                case Base.EUIEventType.onScroll: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Scroll; break;
                case Base.EUIEventType.onBeginDrag: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.BeginDrag; break;
                case Base.EUIEventType.onEndDrag: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.EndDrag; break;
                case Base.EUIEventType.onSubmit: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Submit;break;
                case Base.EUIEventType.onCancel: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Cancel;break;
                case Base.EUIEventType.onMove: guideTriggerType = Framework.Plugin.Guide.EUIWidgetTriggerType.Move; break;
            }
            if (guid > 0 && guideTriggerType != EUIWidgetTriggerType.None)
                Framework.Plugin.Guide.GuideWrapper.OnUIWidgetTrigger(guid, listIndex, guideTriggerType, argvs);

            return false;
        }
    }
}
