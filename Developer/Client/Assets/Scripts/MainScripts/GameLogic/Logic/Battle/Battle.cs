/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Battle
作    者:	HappLI
描    述:	战斗状态
*********************************************************************/

using System.Collections.Generic;
using TopGame.Core;
using TopGame.Data;
using UnityEngine;
using Framework.Module;
using TopGame.SvrData;
using Framework.Core;
using Framework.Base;
using Framework.Logic;
using Framework.Data;
using Framework.BattlePlus;
using ExternEngine;
#if !USE_SERVER
using TopGame.UI;
#endif

namespace TopGame.Logic
{
    public enum EBttleExitType
    {
        None,
        Win = 1,
        Loss = 2,
        Exit = 3,
    }

    [State(EGameState.Battle, 3),StateClearFlag(EGameState.Battle, EGameState.Battle, EStateChangeFlag.Nothing| EStateChangeFlag.HideAllUI| EStateChangeFlag.SceneID)]
    [Framework.Plugin.AT.ATExportMono("逻辑状态/战斗")]
    public class Battle : AState, Framework.Module.IIMGUI, IBattleWorldCallback, IBattleDamagerCallback
    {
        EBttleExitType m_nNetResultType = EBttleExitType.None;
        BattleWorld m_pBattleWorld = null;

        private float m_fBackupShadowDistance = -1;
        EGameQulity m_BackupQuality = EGameQulity.None;
        private uint attackerCfg;
        //------------------------------------------------------
        public override BattleWorld GetBattleWorld()
        {
            if(m_pBattleWorld == null)
                m_pBattleWorld = m_pFrameWork.Get<BattleWorld>(true);
            return m_pBattleWorld;
        }
        //------------------------------------------------------
        public override void OnAwake()
        {
            base.OnAwake();
            m_pBattleWorld = m_pFrameWork.Get<BattleWorld>(true);
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public override string Print()
        {
            string print = base.Print();
            return print;
        }
#endif
        //------------------------------------------------------
        public override void OnInit(EGameState state)
        {
            base.OnInit(state);
            m_nNetResultType = EBttleExitType.None;
        }
        //------------------------------------------------------
        public override void OnStart()
        {
            base.OnStart();
#if !USE_SERVER
            ICameraController ctl = CameraKit.cameraController;
            if(ctl!=null)
                ctl.Enable(true);
            m_nNetResultType = EBttleExitType.None;

            Framework.Core.DyncmicTransformCollects.ActiveNodeByName("battlescene", true,false);

            MagicaCloth.MagicaPhysicsManager.Enable(false);
#endif
        }
        //------------------------------------------------------
        public override void OnPreStart()
        {
            m_BackupQuality = GameQuality.Qulity;
            m_fBackupShadowDistance = GameQuality.ShadowDistance;
            ATerrainManager.EnableTerrainLowerLimit(GetFramework(), false);
            m_nNetResultType = EBttleExitType.None;
            if (m_pBattleWorld != null)
            {
                m_pBattleWorld.RegisterCallback(this);
                m_pBattleWorld.Init();
                m_pBattleWorld.OnBattleResultCheckEvent = OnBattleResultCheckEvent;
                BattleDamager pDamager = GetBattleLogic<BattleDamager>();
                if (pDamager != null) pDamager.RegisterCallback(this);
            }

            base.OnPreStart();

            if (m_pBattleWorld != null)
                m_pBattleWorld.Prepare();
        }
        //------------------------------------------------------
        protected override void OnReDo()
        {
            SvrData.User backupUser = m_pUser;
            m_nNetResultType = EBttleExitType.None;
            if (m_pBattleWorld != null)
            {
                m_pBattleWorld.Exit();
            }
            m_pFrameWork.Resume();
            if (m_pActivedMode == null)
            {
                return;
            }
            bool bEnable = m_pActivedMode.IsEnabled();

            foreach (var db in m_vLogic)
            {
                db.Value.Clear();
            }
            m_pActivedMode.Destroy();

            if (m_pBattleWorld != null)
            {
                m_pBattleWorld.Exit();
            }
			if(GetWorld()!=null) GetWorld().ClearWorld();

            m_pUser = backupUser;

            m_pActivedMode.BindState(this);
            m_pActivedMode.Awake();

            if (m_pBattleWorld != null)
            {
                m_pBattleWorld.RegisterCallback(this);
                m_pBattleWorld.Init();
                m_pBattleWorld.OnBattleResultCheckEvent = OnBattleResultCheckEvent;
                BattleDamager pDamager = GetBattleLogic<BattleDamager>();
                if (pDamager != null) pDamager.RegisterCallback(this);
            }

            foreach (var db in m_vLogic)
                db.Value.PreStart();
            m_pActivedMode.PreStart();

            if (m_pBattleWorld != null)
                m_pBattleWorld.Prepare();

            foreach (var db in m_vLogic)
                db.Value.Start();
            m_pActivedMode.Start();
            m_pActivedMode.Enable(bEnable);
        }
        //------------------------------------------------------
        public override void OnExit()
        {
            base.OnExit();
            if (m_BackupQuality != EGameQulity.None)
                GameQuality.Qulity = m_BackupQuality;
            m_BackupQuality = EGameQulity.None;
            if (m_fBackupShadowDistance >= 0)
                GameQuality.ShadowDistance = m_fBackupShadowDistance;
            m_fBackupShadowDistance = -1;
#if !USE_SERVER
            Framework.Core.DyncmicTransformCollects.ActiveNodeByName("battlescene", false, false);
#endif
            m_nNetResultType = EBttleExitType.None;
            if (m_pBattleWorld != null)
                m_pBattleWorld.Exit();
            m_pBattleWorld = null;

            CameraKit.StopAllEffect();
        }
        //------------------------------------------------------
        public override float GetLowerFpsCheckerThreshold()
        {
            return 28;//if less 28 ,do Demotion Quality
        }
        //------------------------------------------------------
        public EBttleExitType GetNetBattleResult()
        {
            return m_nNetResultType;
        }
        //------------------------------------------------------
        public void SetNetBattleResult(EBttleExitType result)
        {
            m_nNetResultType = result;
        }
        //------------------------------------------------------
        public Actor AddPlayer(byte attackGroup, VariablePoolAble svrData, bool bCheckExist = true)
        {
            if (this.m_pActivedMode == null) return null;
            return this.m_pActivedMode.AddPlayer(attackGroup, svrData, bCheckExist);
        }
        //------------------------------------------------------
        public override void OnUpdate(float fFrameTime)
        {
//             fFrameTime = Framework.Core.BaseUtil.GetFixedFrame(GetFramework(), true);
            base.OnUpdate(fFrameTime);
        }
        //------------------------------------------------------
        public void RefreshBatleData()
        {
            if (m_pBattleWorld == null || GetUser() == null) return;
            VariablePoolAble userData = m_pActivedMode.GetUserData();

            UserManager userMgr = GetFramework().Get<UserManager>();

            BattleSvrData svrData = GetBattleLogic<BattleSvrData>();
            if(svrData!=null)
            {
                svrData.SetChapterData(userData);
                svrData.SetChapterLevel(1);

                //! attackgroup attr
                svrData.ClearAdjustAttrCoefficientFormula(0xff);
                svrData.ClearAttackGroupAttrParam(0xff);

                //attackgroup
                svrData.ClearAttackGroupBuffs(0xff);
            }
            if (userData == null) return;

            PassCondition passCondition = GetBattleLogic<PassCondition>();
            if (passCondition == null) return;
            if(!m_pActivedMode.OnFillPassData(passCondition, userData))
            {
            }
        }
        //------------------------------------------------------
        public override VariablePoolAble GetUserData()
        {
            if (m_pActivedMode == null) return null;
            return m_pActivedMode.GetUserData();
        }
        //------------------------------------------------------
        public uint GetWeatherType()
        {
            return 0;
        }
        //------------------------------------------------------
        public bool OnBattleWorldCallback(BattleWorld pWorld, EBattleWorldCallbackType type, VariablePoolAble takeData = null)
        {
            base.OnBattleCallback(pWorld,type, takeData);
            if (type == EBattleWorldCallbackType.AwakeBattle)
                RefreshBatleData();
            else if (type == EBattleWorldCallbackType.StartBattle)
            {
                //! global buff applay
            }
            else if(type == EBattleWorldCallbackType.DefeatedBattle || type == EBattleWorldCallbackType.VictoryBattle)
            {
                if (SDK.GameSDK.HasLogEvent())
                {
                    var stats = GetBattleLogic<BattleStats>();
                    //  if(type == EBattleWorldCallbackType.VictoryBattle)
                    {
                        SDK.GameSDK.AddStatisticData("score", stats.GetScore(), true);
                        SDK.GameSDK.UpdateStatistic(GetUser().GetSdkUid());
                    }
                }
            }
            if (m_pActivedMode!=null)
            {
                m_pActivedMode.OnBattleWorldCallback(pWorld,type, takeData);
            }
            return false;
        }
        //------------------------------------------------------
        EBattleResultStatus OnBattleResultCheckEvent(EBattleResultStatus current)
        {
            if (m_pActivedMode != null)
            {
                m_pActivedMode.OnBattleResultCheckEvent(ref current);
            }
            return current;
        }
        //------------------------------------------------------
        public bool OnBatleFrameCollision(AWorldNode attacker, AWorldNode target, ref CollisonParam param)
        {
            BattleStatus pStatus = GetBattleLogic<BattleStatus>();
            if (target != null && attacker != null && !BattleStatus.bInvincible_Player &&
                target.GetActorType() == EActorType.Player && target.GetAttackGroup() == 0 && !pStatus.IsInvincible(target.GetActorType()))
            {
                if (attacker is ObsElement)
                {
                    if (attacker.GetContextData() is CsvData_BattleObject.BattleObjectData)
                    {
                        if (((CsvData_BattleObject.BattleObjectData)attacker.GetContextData()).skillDamageID != 0)
                        {
                            attackerCfg = attacker.GetConfigID();
                        }
                    }
                }
            }
            
            if (target != null && attacker is Actor && target is Actor && param.userData is AttackFrameParameter)
            {
                AttackFrameParameter actionFrame = param.userData as AttackFrameParameter;
                CsvData_SkillDamage.SkillDamageData pDamageData = null;
                if (actionFrame.damage != 0)
                {
                    pDamageData = DataManager.getInstance().SkillDamage.GetData(actionFrame.damage);
                }
                if (pDamageData == null)
                {
                    IContextData pUserData = attacker.GetContextData();
                    if (pUserData == null) return true;
                    if (pUserData is CsvData_Monster.MonsterData)
                    {
                        if (pDamageData == null)
                            pDamageData = (pUserData as CsvData_Monster.MonsterData).SkillDamage_damageId_data;
                    }
                }
                param.pDoDamage = pDamageData;
                return true;
            }
            ObsElement pEle = attacker as ObsElement;
            if (pEle == null) return true;

            bool bCanTriggerEvent = true;// (param.collisonFlag & (int)ECollisionBit.EventTrigget) != 0;
            CsvData_BattleObject.BattleObjectData battleObject = pEle.GetContextData() as CsvData_BattleObject.BattleObjectData;
            if (battleObject != null)
            {
                Framework.Core.AudioUtil.PlayEffect(battleObject.hitAudio);
                param.pDoDamage = battleObject.SkillDamage_skillDamageID_data;
                param.nDoTargetBuffId = battleObject.buffId;

                param.arrTargetEvenntIDs = battleObject.eventID;

                var goldUpNum = battleObject.goldUp;
                var foodUpNum = battleObject.foodUp;
                param.extarnVar = new Variable3() { intVal0 = (int)goldUpNum, intVal1  = (int)battleObject.pointUp, intVal2 = (int)foodUpNum };
#if !USE_SERVER
                if (bCanTriggerEvent && !pEle.IsCanPick())
                {
                    BattleDamage damage = new BattleDamage();
                    damage.Set(BattleFrameDamage.DEF, attacker as Actor, target, target as Actor, false, false, 1, pEle.GetPosition());
                    Framework.Plugin.AT.AgentTreeManager.Execute((ushort)Base.EATEventType.OnElementCollision, 1, damage);
                }
#endif
            }
            return true;
        }
        //------------------------------------------------------
        public bool OnBattleDamageValue(EDamageType type, Actor pAttacker, Actor pTarget, ref int value)
        {
            BattleStatus pStatus = GetBattleLogic<BattleStatus>();
            
            if (pTarget != null && pTarget.GetActorType() == EActorType.Player && !BattleStatus.bInvincible_Player
                                && pTarget.GetAttackGroup() == 0 && !pStatus.IsInvincible(pTarget.GetActorType()))
            {
                if (pAttacker is Monster)
                {
                    TopGame.Core.AUserActionManager.AddActionKV("damage_type", 2);
                    TopGame.Core.AUserActionManager.AddActionKV("moster_id", pAttacker.GetConfigID());
                    TopGame.Core.AUserActionManager.AddActionKV("plater_id", 0);
                    TopGame.Core.AUserActionManager.AddActionKV("object_id", 0);
                }
                else if (pAttacker is Player)
                {
                    TopGame.Core.AUserActionManager.AddActionKV("damage_type", 3);
                    TopGame.Core.AUserActionManager.AddActionKV("moster_id", 0);
                    TopGame.Core.AUserActionManager.AddActionKV("plater_id", pAttacker.GetConfigID());
                    TopGame.Core.AUserActionManager.AddActionKV("object_id", 0);
                }
                else
                {
                    TopGame.Core.AUserActionManager.AddActionKV("damage_type", 1);
                    TopGame.Core.AUserActionManager.AddActionKV("moster_id", 0);
                    TopGame.Core.AUserActionManager.AddActionKV("plater_id", 0);
                    TopGame.Core.AUserActionManager.AddActionKV("object_id", attackerCfg);
                }
                TopGame.Core.AUserActionManager.AddActionKV("hp_max", pTarget.GetActorParameter().GetRuntimeParam().max_hp);
                TopGame.Core.AUserActionManager.AddActionKV("damage_type", type.ToString());
                if (type == EDamageType.Cure || type == EDamageType.Shield)
                {
                    TopGame.Core.AUserActionManager.AddActionKV("hp_now", pTarget.GetActorParameter().GetRuntimeParam().hp + value);
                    TopGame.Core.AUserActionManager.AddActionKV("damage", value);
                }
                else
                {
                    TopGame.Core.AUserActionManager.AddActionKV("hp_now", pTarget.GetActorParameter().GetRuntimeParam().hp - value > 0?
                        pTarget.GetActorParameter().GetRuntimeParam().hp - value:0);
                    TopGame.Core.AUserActionManager.AddActionKV("damage", -value);
                }

                AddEventUtil.LogEvent("hp_lose",true,false,true);
            }
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
                {
                    pDamange = DataManager.getInstance().SkillDamage.GetData(attackData.damage_id);
                    if (pDamange != null)
                    {
                        attackData.skill_damage_data = pDamange;
                    }
                }
            }
            else
            {
                if(pDamange.id != attackData.damage_id)
                {
                    pDamange = DataManager.getInstance().SkillDamage.GetData(attackData.damage_id);
                    if(pDamange!=null)
                    {
                        attackData.skill_damage_data = pDamange;
                    }
                }
            }
            return true;
        }
        //------------------------------------------------------
        public bool OnBattleFillFrameDamage(ref BattleFrameDamage damageParam, IContextData pData, ref ActorAttackData attackData)
        {
            if (GetBattleWorld().IsOvered())
                return false;
            if(pData!=null && pData is CsvData_SkillDamage.SkillDamageData)
            {
                CsvData_SkillDamage.SkillDamageData pDamage = pData as CsvData_SkillDamage.SkillDamageData;
                if (pDamage != null)
                {
                    damageParam.damageID = pDamage.id;
                    if (damageParam.skillLevel <= 0) damageParam.skillLevel = 1;
                    damageParam.damageGroupID = pDamage.groupID;
                    damageParam.eventID = pDamage.eventID;
                    damageParam.damageType = pDamage.damageType;
                    damageParam.damageElements = pDamage.element;

                    damageParam.effectDamageLevel = pDamage.damageLevel;

                    damageParam.damage_fire = pDamage.damage_fire;
                    damageParam.damage_fire_rate = pDamage.damage_fire_rate;
                    damageParam.citical_fire = pDamage.citical_fire;
                    damageParam.bIngoreInvincible = pDamage.ingoreInvincible;

                    damageParam.attackGainSpType = pDamage.ourGainSpType;
                    damageParam.attackGainSpValue = pDamage.ourGainSpValue;
                    damageParam.targetGainSpType = pDamage.enemyGainSpType;
                    damageParam.targetGainSpValue = pDamage.enemyGainSpValue;

                    damageParam.searchTypeGroupIDs = pDamage.searchTypeGroupID;
                    damageParam.extraDamageChanges = pDamage.extraDamageChange;
                    damageParam.extraBuffIds = pDamage.extraBuffId;
                    damageParam.extraBuffProbabilitys = pDamage.extrabuffProbability;

                    damageParam.damageGroups = pDamage.ValueTarget;
                    damageParam.attrTypes = pDamage.effectTypes;
                    damageParam.valueTypes = pDamage.valueTypes;
                    damageParam.fixedDamage = pDamage.fixedDamage;
                    damageParam.shieldDamageDisregard = pDamage.shieldDamageDisregard;

                    damageParam.shieldDamageCoefficient = (int)pDamage.shieldDamageCoefficient;

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

                    ////! 阵营克制检测
                    //if(attackData.attacker_ptr!=null && attackData.target_ptr!=null && attackData.target_ptr!= attackData.attacker_ptr)
                    //{
                    //    Actor pAttacker = attackData.attacker_ptr as Actor;
                    //    Actor pTarger = attackData.target_ptr as Actor;
                    //    if(pAttacker!=null && pTarger!=null && pAttacker.CanAttackGroup(pTarger.GetAttackGroup()) &&
                    //        pAttacker.GetActorParameter().GetCamp() != pTarger.GetActorParameter().GetCamp())
                    //    {
                    //        var campData = Data.DataManager.getInstance().CampRelation.GetData(pAttacker.GetActorParameter().GetCamp(), pTarger.GetActorParameter().GetCamp());
                    //        if(campData!=null)
                    //        {
                    //            damageParam.campReactionValueTypes = campData.valueTypes;
                    //            damageParam.campReactionValues = campData.values;
                    //        }
                    //    }
                    //}

#if UNITY_EDITOR
                    if(!damageParam.isValid())
                    {
                        Debug.LogError("伤害Id[" +pDamage.id + "]:ValueTarget effectTypes valueTypes 数组个数不匹配!");
                    }
#endif
                }
            }
            return true;
        }
        //------------------------------------------------------
        public bool OnBattleApplayElementReactionEffect(ref BattleFrameDamage damageParam, ref ActorAttackData attackData, ref ElementReactionParam param)
        {
//            uint weatherType = 0;
//            param.weatherType = GetWeatherType();

//            CsvData_ElemetEffect.ElemetEffectData effectData = Data.DataManager.getInstance().ElemetEffect.MatchEffect(attackData.attacker_type, attackData.attacker_config_id, param.attacker_element_flags, attackData.target_type, attackData.target_config_id, param.target_element_flags, weatherType);
//            attackData.elementEffect = effectData;
//#if UNITY_EDITOR
//            if (ConfigUtil.bDamageDebug)
//            {
//                if(effectData!=null)
//                    Framework.Plugin.Logger.Info("元素反应:" + effectData.id + "    value:" + effectData.value);
//            }
//#endif
//            if(effectData!=null)
//            {
//                param.targetBuffID = effectData.targetBuffID;
//                param.targetEventID = effectData.targetEventID;
//                param.targetBuffLevel = damageParam.skillLevel;

//                param.atkBuffID = effectData.atkBuffID;
//                param.atkEventID = effectData.atkEventID;
//                param.atkBuffLevel = damageParam.skillLevel;

//                param.reactionType = effectData.reactionType;
//                param.valueType = effectData.valueType;
//                param.value = effectData.value;

//#if !USE_SERVER
//                float scale = effectData.scaleEffect;
//                if (attackData.target_type == EActorType.Monster)
//                {
//                    Monster pTarget = attackData.target_ptr as Monster;
//                    if (pTarget != null && pTarget.GetMonsterType() == EMonsterType.Boss) scale = effectData.scaleEffectBoss;
//                }
//                string hitEffect = effectData.hitEffect;
//                if (!string.IsNullOrEmpty(hitEffect))
//                {
//                    InstanceOperiaon pOp = FileSystemUtil.SpawnInstance(hitEffect, true);
//                    if (pOp != null)
//                    {
//                        pOp.pByParent = RootsHandler.ParticlesRoot;
//                        pOp.userData0 = new Variable3()
//                        {
//                            floatVal0 = attackData.hit_position.x,
//                            floatVal1 = attackData.hit_position.y,
//                            floatVal2 = attackData.hit_position.z
//                        };
//                        pOp.userData1 = new Variable3() { floatVal0 = attackData.hit_direction.x, floatVal1 = attackData.hit_direction.y, floatVal2 = attackData.hit_direction.z };
//                        pOp.userData2 = new Variable3() { floatVal0 = scale, floatVal1 = scale, floatVal2 = scale };
//                        pOp.OnCallback = ProjectileHitUtil.OnProjectHitCallback;
//                    }
//                }
//                string hitSound = effectData.hitSound;
//                if (!string.IsNullOrEmpty(hitSound))
//                {
//                    Framework.Core.AudioUtil.PlayEffect(hitSound);
//                }
//#endif
//            }
            return true;
        }
        //------------------------------------------------------
        public bool OnBattleApplayDamageBuff(Actor pAttacker, Actor pTarget, uint[] buffs, uint[] buffProbability, uint buffLevel, int effectHitRate)
        {
            bool isEnemy = pAttacker.CanAttackGroup(pTarget.GetAttackGroup());
            BattleStatus pBattleStatus = null;
            CsvData_Buff.BuffData buffData;
            for (int i = 0; i < buffs.Length; ++i)
            {
                buffData = Data.DataManager.getInstance().Buff.GetBuff(buffs[i], buffLevel);
                if (buffData == null) continue;
                if(!buffData.trigger_fire && isEnemy)
                {
                    int appendProbability = 0;
                    if(pAttacker!=null)
                    {
                        if (pBattleStatus == null) pBattleStatus = GetBattleLogic<BattleStatus>();
                        if (pBattleStatus != null)
                        {
                            appendProbability = pBattleStatus.GetBuffTriggerProbability(pAttacker.GetInstanceID(), buffData.type, (int)buffData.groupID);
                        }
                    }

                    if (buffData.type == EBuffType.Debuff)
                    {
                        if (buffProbability != null && i < buffProbability.Length)
                        {
                            if (!GetFramework().CheckerRandom((int)buffProbability[i]+ appendProbability, 0, CommonUtility.WAN_VALUE))
                                continue;
                        }
                    }
                    else
                    {
                        if (buffProbability != null && i < buffProbability.Length)
                        {
                            if (!GetFramework().CheckerRandom((int)(buffProbability[i] + effectHitRate+ appendProbability), 0, CommonUtility.WAN_VALUE))
                                continue;
                        }
                    }
                }
                switch (buffData.targetType)
                {
                    case EBuffGroupType.Attacker:
                        {
                            if (pAttacker != null)
                                pAttacker.BeginBuffer(buffs[i], buffLevel, pAttacker, null);
                        }
                        break;
                    case EBuffGroupType.Targeter:
                        {
                            if (pTarget != null)
                            {
                                pTarget.BeginBuffer(buffs[i], buffLevel, pAttacker, null);
                            }
                        }
                        break;
                    case EBuffGroupType.Friender:
                        {
                            if(pAttacker != null)
                            {
                                AWorldNode pNode = GetWorld().GetRootNode();
                                while (pNode != null)
                                {
                                    Actor pActor = pNode as Actor;
                                    pNode = pNode.GetNext();
                                    if (pActor != null && pActor.IsCanLogic() && pActor.GetAttackGroup() == pAttacker.GetAttackGroup())
                                    {
                                        pActor.BeginBuffer(buffs[i], buffLevel, pAttacker, null);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            return true;
        }
        //------------------------------------------------------
        public void OnBattleFrameDamage(BattleDamage frameDamage)
        {
            if(frameDamage.pActor!=null && frameDamage.bKilled)
            {
                if(frameDamage.pActor.GetActorType() == EActorType.Monster)
                {
                    CsvData_Monster.MonsterData monstarData = frameDamage.pActor.GetContextData() as CsvData_Monster.MonsterData;
                    if (monstarData != null && monstarData.deadPoint != 0)
                    {
                        GetBattleLogic<BattleStats>().OnScore((int)monstarData.deadPoint, frameDamage.pAttacker);
                    }
                }
            }
        }
        //------------------------------------------------------
        public void OnGUI()
        {
#if UNITY_EDITOR
            if (m_pBattleWorld == null) return;
            BattleStatus pStatus = GetBattleLogic<BattleStatus>();
            if (pStatus == null) return;
            if (pStatus.IsInvincible(EActorType.Player) || (BattleStatus.bInvincible_Player))
            {
                Color color = GUI.color;
                GUI.color = Color.black;
                GUI.Label(new Rect(Screen.width - 100, 0, 100, 30), "无敌");
                GUI.color = color;
            }
#endif
        }
    }
}
