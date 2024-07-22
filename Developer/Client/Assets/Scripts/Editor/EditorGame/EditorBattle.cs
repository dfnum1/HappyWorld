/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	EditorBattle
作    者:	HappLI
描    述:	战斗状态
*********************************************************************/
using Framework.BattlePlus;
using Framework.Core;
using Framework.Data;
using Framework.Logic;
using System.Linq;
using TopGame.Core;
using TopGame.Data;
using UnityEngine;

namespace TopGame.Logic
{
    public class EditorBattle : IBattleDamagerCallback
    {
        BattleWorld m_pBattleWorld = null;
        //------------------------------------------------------
        public void Awake(Framework.Module.AFramework pFramework)
        {
            m_pBattleWorld = pFramework.Get<BattleWorld>(true);
        }
        //------------------------------------------------------
        public void Start()
        {
            if (m_pBattleWorld != null)
            {
                m_pBattleWorld.Init();
                m_pBattleWorld.OnBattleResultCheckEvent = OnBattleResultCheckEvent;
                BattleDamager pDamager = m_pBattleWorld.GetLogic<BattleDamager>();
                if (pDamager != null) pDamager.RegisterCallback(this);
            }
            if (m_pBattleWorld != null)
            {
                m_pBattleWorld.Prepare();
                m_pBattleWorld.Start();
                m_pBattleWorld.Active(true);
            }
        }
        //------------------------------------------------------
        public void Exit()
        {
            if (m_pBattleWorld != null)
                m_pBattleWorld.Exit();
            m_pBattleWorld = null;
        }
        //------------------------------------------------------
        public bool OnBattleWorldCallback(BattleWorld pWorld, EBattleWorldCallbackType type, VariablePoolAble takeData = null)
        {
            return false;
        }
        //------------------------------------------------------
        EBattleResultStatus OnBattleResultCheckEvent(EBattleResultStatus current)
        {
            return current;
        }
        //------------------------------------------------------
        public bool OnBatleFrameCollision(AWorldNode attacker, AWorldNode target, ref CollisonParam param)
        {
            return true;
        }
        //------------------------------------------------------
        public bool OnBattleDamageValue(EDamageType type, Actor pAttacker, Actor pTarget, ref int value)
        {
            return true;
        }
        //------------------------------------------------------
        public bool OnBattleDoActorDamageCheck(ref ActorAttackData attackData)
        {
            CsvData_SkillDamage.SkillDamageData pDamange = attackData.skill_damage_data as CsvData_SkillDamage.SkillDamageData;
            if (pDamange == null)
            {
                CsvData_Skill.SkillData skillData = null;
                if (attackData.skill_data != null)
                    skillData = attackData.skill_data as CsvData_Skill.SkillData;
                if (skillData != null)
                    attackData.skill_damage_data = skillData.SkillDamage_skillDamageID_data;
                if (attackData.skill_damage_data == null)
                    pDamange = DataManager.getInstance().SkillDamage.GetData(attackData.damage_id);
            }
            if (attackData.skill_damage_data == null)
            {
                if(DataManager.getInstance().SkillDamage.datas.Count>0)
                {
                    attackData.skill_damage_data = DataManager.getInstance().SkillDamage.datas.ElementAt(0).Value;
                }
            }
            return true;
        }
        //------------------------------------------------------
        public bool OnBattleFillFrameDamage(ref BattleFrameDamage damageParam, IContextData pData, ref ActorAttackData attackData)
        {
            if(pData!=null && pData is CsvData_SkillDamage.SkillDamageData)
            {
                CsvData_SkillDamage.SkillDamageData pDamage = pData as CsvData_SkillDamage.SkillDamageData;
                if (pDamage != null)
                {
                    damageParam.damageID = pDamage.id;
                    if (damageParam.skillLevel <= 0) damageParam.skillLevel = 1;
                    damageParam.eventID = pDamage.eventID;
                    damageParam.damageType = pDamage.damageType;
                    damageParam.damageElements = pDamage.element;

                    damageParam.effectDamageLevel = pDamage.damageLevel;

                    damageParam.damage_fire = pDamage.damage_fire;
                    damageParam.damage_fire_rate = pDamage.damage_fire_rate;
                    damageParam.citical_fire = pDamage.citical_fire;

                    damageParam.attackGainSpType = pDamage.ourGainSpType;
                    damageParam.attackGainSpValue = pDamage.ourGainSpValue;
                    damageParam.targetGainSpType = pDamage.enemyGainSpType;
                    damageParam.targetGainSpValue = pDamage.enemyGainSpValue;

                    damageParam.damageGroups = pDamage.ValueTarget;
                    damageParam.attrTypes = pDamage.effectTypes;
                    damageParam.valueTypes = pDamage.valueTypes;
                    damageParam.fixedDamage = pDamage.fixedDamage;

                    damageParam.limitDamageGroups = pDamage.ValueTargetMax;
                    damageParam.limitAttrTypes = pDamage.effectTypesMax;
                    damageParam.limitValueTypes = pDamage.valueTypesMax;
                    damageParam.limitFixedDamage = pDamage.fixedDamageMax;

                    damageParam.buffs = pDamage.buffID;
                    damageParam.buffsProbability = pDamage.buffProbability;
                    damageParam.propertValues = pDamage.values;
                    damageParam.effectDamageLevel = pDamage.damageLevel;
                    if (pDamage.secondDamageTypes == attackData.hitType && pDamage.declineRatio != null && pDamage.declineRatio.Length > 0)
                    {
                        switch (attackData.hitType)
                        {
                            case EHitType.Bound:
                            case EHitType.MutiHit:
                                {
                                    int index = 0;
                                    if (attackData.hit_type_user_data != null && attackData.hit_type_user_data is Variable1)
                                    {
                                        index = ((Variable1)attackData.hit_type_user_data).intVal;
                                        index = Mathf.Clamp(index, 0, pDamage.declineRatio.Length - 1);
                                    }
#if UNITY_EDITOR
                                    if (DebugConfig.bDamageDebug)
                                    {
                                        Framework.Plugin.Logger.Info("伤害递减:" + pDamage.declineRatio[index]);
                                    }
#endif
                                    attackData.damage_power += pDamage.declineRatio[index] * Framework.Core.CommonUtility.WAN_RATE;
                                }
                                break;
                            case EHitType.Explode:
                                {
#if UNITY_EDITOR
                                    if (DebugConfig.bDamageDebug)
                                    {
                                        Framework.Plugin.Logger.Info("伤害递减:" + pDamage.declineRatio[0]);
                                    }
#endif
                                    attackData.damage_power += pDamage.declineRatio[0] * Framework.Core.CommonUtility.WAN_RATE;
                                }
                                break;
                        }
                    }

                }
            }
            return true;
        }
        //------------------------------------------------------
        public bool OnBattleApplayElementReactionEffect(ref BattleFrameDamage damageParam, ref ActorAttackData attackData, ref ElementReactionParam param)
        {
            return true;
        }
        //------------------------------------------------------
        public bool OnBattleApplayDamageBuff(Actor pAttacker, Actor pTarget, uint[] buffs, uint[] buffProbability, uint buffLevel = 1,int effectHitRate =0)
        {
            return true;
        }
        //------------------------------------------------------
        public void OnBattleFrameDamage(BattleDamage battleDamage)
        {
        }
    }
}
