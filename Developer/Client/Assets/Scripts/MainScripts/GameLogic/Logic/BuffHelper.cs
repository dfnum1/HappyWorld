/********************************************************************
生成日期:	3:17:2022  20:26
类    名: 	BuffUtil
作    者:	HappLI
描    述:	
*********************************************************************/
using TopGame.Data;
using TopGame.Core;
using Framework.Core;
using Framework.Base;
using System.Collections.Generic;
using ExternEngine;

namespace TopGame.Logic
{
    public class BuffHelper
    {
        public static BufferState CreateBuffer(Framework.Module.AFrameworkBase pFrame, Actor pActor, AWorldNode trigger, BufferState buff, uint dwBufferID, uint dwLevel, IContextData pData, IBuffParam param = null)
        {
            if (pFrame == null) return null;
            Framework.Module.AFramework pFramework = pFrame as Framework.Module.AFramework;
            if (pFramework == null) return null;

            ActorAgent pAgent = null;
            if (pActor != null)
            {
                pAgent = pActor.GetActorAgent();
            }
            Data.CsvData_Buff.BuffData buffData = pData as Data.CsvData_Buff.BuffData;
            if (buffData == null) buffData = Data.DataManager.getInstance().Buff.GetBuff(dwBufferID, dwLevel);
            if (buffData == null)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogWarning("buff组Id:" + dwBufferID + "   level:" + dwLevel + "   数据不存在");
#endif
                return null;
            }
            if(pActor!=null && buffData.limitCount>0)
            {
                AFrameworkModule frameworkMoudle = pFramework as AFrameworkModule;
                if(frameworkMoudle!=null)
                {
                    Framework.BattlePlus.BattleWorld battleWorld = frameworkMoudle.battleWorld as Framework.BattlePlus.BattleWorld;
                    if(battleWorld!=null)
                    {
                        Framework.BattlePlus.BattleStatus battleStatus = battleWorld.GetLogic<Framework.BattlePlus.BattleStatus>();
                        if(battleStatus!=null)
                        {
                            if (!battleStatus.OnCheckAddBuff(pActor, (int)dwBufferID))
                            {
#if UNITY_EDITOR
                                UnityEngine.Debug.LogWarning("buff组Id:" + dwBufferID + "   level:" + dwLevel + "   已达到上限");
#endif
                                return null;
                            }
                        }
                    }
                }
            }

            if (buffData.probability < 1f)
            {
                if (!pFramework.CheckerRandom(buffData.probability)) return null;
            }
            if(pAgent!=null)
            {
                if (pAgent.IsBuffEffect(EBuffEffectBit.ForbidBuff) && buffData.type == EBuffType.Buff)
                {
                    return null;
                }
                if (pAgent.IsBuffEffect(EBuffEffectBit.ForbidDeBuff) && buffData.type == EBuffType.Debuff)
                {
                    return null;
                }
            }

            bool bNew = true;
            if (buff != null)
            {
                bNew = false;
                if(buff.groupType!=0 && buff.groupType == buffData.typeGroupID)
                {
                    if(buffData.priority < buff.priority)
                    {
                        return buff;
                    }
                }
                if (buff.GetLayer() >= buffData.layers)
                {
                    buff.step_duration = (buffData.interval * 0.001f);
                    buff.running_delta = 0;
                    buff.running_duration = (buffData.time * 0.001f);
                    return buff;
                }
                int overlapType = buffData.d_superposition;
                if (trigger != null)
                {
                    if (buff.triggerActor == trigger.GetInstanceID() && buff.triggerType == trigger.GetActorType())
                    {
                        overlapType = buffData.s_superposition;
                    }
                    buff.triggerActor = trigger.GetInstanceID();
                    buff.triggerType = trigger.GetActorType();
                }
                else
                {
                    buff.triggerActor = 0;
                    buff.triggerType = EActorType.None;
                }
                buff.hitFinish = buffData.BeHitFinish;
                buff.attackFinish = buffData.hitFinish;
                buff.applayOverEffectBit = buffData.buffEffectApplayFlag;
                buff.applayOverEffectFinish = buffData.buffEffectApplayFinish;
                switch (overlapType)
                {
                    case 0:  //重置
                        {
                            buff.step_duration = (buffData.interval * 0.001f);
                            buff.running_delta = 0;
                            buff.running_duration = (buffData.time * 0.001f);
                            buff.end = false;
                            buff.layer = 1;
                        //    if(!buff.isActived()) buff.SetActiveCondition(buffData.triggerLimit, buffData.triggerParams);
                        }
                        break;
                    case 1: // 重置，且叠加
                        {
                            buff.step_duration = (buffData.interval * 0.001f);
                            buff.running_delta = 0;
                            buff.running_duration = (buffData.time * 0.001f);
                            buff.end = false;
                            buff.layer++;
                         //   if (!buff.isActived()) buff.SetActiveCondition(buffData.triggerLimit, buffData.triggerParams);
                        }
                        break;
                    case 2:
                        return buff;
                }
                buff.type = buffData.type;
                buff.end = false;

                if (buff.GetLayer() >= buffData.layers && buffData.layersMaxEvent != null)
                {
                    AEventSystemTrigger evtSystem = pFramework.eventSystem;
                    evtSystem.Begin();
                    if(pActor!=null)
                    {
                        evtSystem.ATuserData = pActor;
                        evtSystem.TriggerEventPos = pActor.GetPosition();
                        evtSystem.TriggerEventRealPos = pActor.GetPosition();
                        evtSystem.TriggerActorDir = pActor.GetDirection();
                    }
                    for (int i = 0; i < buffData.layersMaxEvent.Length; ++i)
                        pFramework.OnTriggerEvent((int)buffData.layersMaxEvent[i], false);
                    evtSystem.End();
                }
                //          buff.step_delta = 0;
                buff.die_keep = buffData.diedKeep != 0;
                buff.bindBit = buffData.bindBite;
                if (param != null && param is BuffOffsetParam)
                {
                    BuffOffsetParam offsetParam = (BuffOffsetParam)param;
                    buff.offset = offsetParam.offset;
                    buff.rotateOffset = offsetParam.rotateOffset;
                    buff.bindBit = offsetParam.bindBit;
                    buff.triggerEvent = offsetParam.bTriggerEvent;
                    buff.die_keep = offsetParam.die_keep;
                }
                else
                    buff.triggerEvent = true;
                if (buff.bindBit == 0) buff.bindBit = buffData.bindBite;
                if (buffData.instant || buff.step_duration <= 0)
                {
                    if(buff.tick_count>0) buff.tick_count--;
                    if(pAgent!=null) pAgent.BufferTick(buff);
                }
            }

            if (bNew)
            {
                buff = BuffUtil.NewBufferState(pFramework);
                buff.groupType = buffData.typeGroupID;
                buff.priority = buffData.priority;
                buff.step_duration = (buffData.interval * 0.001f);
                buff.running_delta = 0;
                buff.step_delta = 0;
                buff.running_duration = (buffData.time * 0.001f);
                buff.end = false;
                buff.instant = buffData.instant;
                if (trigger != null)
                {
                    buff.triggerActor = trigger.GetInstanceID();
                    buff.triggerType = trigger.GetActorType();
                }
                buff.data = buffData;
                buff.id = dwBufferID;
                buff.layer = 1;
                buff.level = (ushort)buffData.level;
                buff.extern_layer = 0;
                buff.max_layer = buffData.layers;
                buff.layersMaxEvent = buffData.layersMaxEvent;
                buff.hitFinish = buffData.BeHitFinish;
                buff.attackFinish = buffData.hitFinish;
                buff.applayOverEffectBit = buffData.buffEffectApplayFlag;
                buff.applayOverEffectFinish = buffData.buffEffectApplayFinish;
                buff.die_keep = buffData.diedKeep != 0;
                buff.type = buffData.type;
                buff.dispelLevel = buffData.dispelLevel;

                buff.limitCnt = buffData.limitCount;

                buff.parent_slot = buffData.parentSlot;

                buff.begin_effect = buffData.beginEffect;
                buff.begin_sound = buffData.beginSound;
                buff.begin_scale = (pActor!=null && pActor.GetExternType() == (int)(EMonsterType.Boss)) ? buffData.scaleEffectBoss : buffData.scaleEffect;

                buff.step_effect = buffData.stepEffect;
                buff.step_sound = buffData.stepSound;
                buff.end_effect = buffData.endEffect;
                buff.end_sound = buffData.endSound;

                buff.effectMaterial = buffData.effectMaterial;

                buff.SetActiveCondition(buffData.triggerLimit, buffData.triggerParams);

                buff.targetGroupType = buffData.targetType;
                buff.starEvent = buffData.starEvent;
                buff.intervalEvent = buffData.intervalEvent;
                buff.endEvent = buffData.endEvent;
                buff.endEventByTrigger = buffData.endEventByTrigger;
                buff.tickDamage = Data.DataManager.getInstance().SkillDamage.GetData(buffData.damage);
                buff.tickDamageID = buffData.damage;
                if (buff.tickDamage != null)
                    buff.tick_damage_element = buffData.element;
#if UNITY_EDITOR
                else if (buffData.damage > 0)
                {
                    UnityEngine.Debug.LogError("Buff 伤害数据" + buffData.damage + "   等级" + buffData.level + "  不存在!");
                }
#endif
                else buff.tick_damage_element = 0;

                buff.configAttrTypes = buffData.attrTypes;
                buff.configValueTypes = buffData.valueTypes;
                buff.configValues = buffData.values;

                if (buffData.attrTypes!=null && buffData.valueTypes!=null && buffData.values!=null && 
                    buffData.attrTypes.Length == buffData.valueTypes.Length &&
                    buffData.valueTypes.Length == buffData.values.Length)
                {
                    if (buff.attrTypes == null) buff.attrTypes = new System.Collections.Generic.List<uint>(buffData.attrTypes);
                    else
                    {
                        buff.attrTypes.Clear();
                        for (int i = 0; i < buffData.attrTypes.Length; ++i) buff.attrTypes.Add(buffData.attrTypes[i]);
                    }
                    if (buff.valueTypes == null) buff.valueTypes = new System.Collections.Generic.List<uint>(buffData.valueTypes);
                    else
                    {
                        buff.valueTypes.Clear();
                        for (int i = 0; i < buffData.valueTypes.Length; ++i) buff.valueTypes.Add(buffData.valueTypes[i]);
                    }
                    if (buff.values == null) buff.values = new System.Collections.Generic.List<int>(buffData.values);
                    else
                    {
                        buff.values.Clear();
                        for (int i = 0; i < buffData.values.Length; ++i) buff.values.Add(buffData.values[i]);
                    }
                }

                buff.effectTypeFlag = buffData.effectType;
                buff.skillTypeCDFlag = buffData.skillType;
                buff.skillForbidFlags = buffData.forbiddenSkillType;
                buff.buffElementFlag = buffData.element;

                buff.bindBit = buffData.bindBite;
                if (param != null && param is BuffOffsetParam)
                {
                    BuffOffsetParam offsetParam = (BuffOffsetParam)param;
                    buff.offset = offsetParam.offset;
                    buff.rotateOffset = offsetParam.rotateOffset;
                    buff.bindBit = offsetParam.bindBit;
                    buff.triggerEvent = offsetParam.bTriggerEvent;
                    buff.die_keep = offsetParam.die_keep || buffData.diedKeep != 0;
                }
                else
                    buff.triggerEvent = true;
                if (buff.bindBit == 0) buff.bindBit = buffData.bindBite;
            }
            return buff;
        }
    }
}
