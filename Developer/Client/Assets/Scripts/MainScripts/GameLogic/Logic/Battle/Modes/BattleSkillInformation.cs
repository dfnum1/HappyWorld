/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	SkillInformation
作    者:	HappLI
描    述:	技能
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using TopGame.Core;
using TopGame.Data;
using Framework.Core;
using Framework.Base;
using Framework.Logic;
using Framework.BattlePlus;
using ExternEngine;
using TopGame.SvrData;
using System;

namespace TopGame.Logic
{
    public class BattleSkillInformations : SkillInformation
    {
        protected FFloat m_fGlobalLastCDTime = 0;

        protected FFloat m_fLockInterrupTime = 0;
        protected FFloat m_fSkillActionDurationTime = 0;
        //------------------------------------------------------
        public override bool CanInterrupt()
        {
            if (m_pCurrentSkill == null || m_pCurrentSkill.interruptTime <= 0) return true;
            if (m_fLockInterrupTime > 0)
            {
                if (m_fLockInterrupTime > 0)
                {
                    return true;
                }
                else if (m_fLockInterrupTime <= 0 &&  m_fSkillActionDurationTime>0)
                {
                    return false;
                }
            }
            return true;
        }
        //------------------------------------------------------
        public override FFloat GetInterruptFactor()
        {
            if (m_pCurrentSkill == null || m_pCurrentSkill.interruptTime <= 0) return 0;
            return Mathf.Clamp01(m_fLockInterrupTime / m_pCurrentSkill.interruptTime);
        }
        //------------------------------------------------------
        public override void ResetSkills()
        {
            base.ResetSkills();
            if (m_pActor == null) return;
            m_fGlobalLastCDTime = 0;
            OnAddSkill(m_pActor);
        }
        //------------------------------------------------------
        public override void ClearSkill()
        {
            base.ClearSkill();
        }
        //------------------------------------------------------
        public override void CreateSkill(Actor pActor)
        {
            if (pActor == null) return;
            Clear();
            m_fGlobalLastCDTime = 0;

            base.CreateSkill(pActor);
            OnAddSkill(pActor);
        }
        //------------------------------------------------------
        void OnAddSkill(Actor pActor)
        {
            if (pActor == null) return;
            IContextData pData = pActor.GetContextData();
            if (pData == null) return;
            bool isPvp = (GetFramework().battleWorld!=null)?GetFramework().battleWorld.IsPvpMode():false;

            uint level = pActor.GetActorParameter().GetLevel();
            List<Framework.Data.SvrSkillData> heroSkills = null;
            uint[] skills = null;
            if (pData is CsvData_Player.PlayerData)
            {
                CsvData_Player.PlayerData player = pData as CsvData_Player.PlayerData;
                if (isPvp) skills = player.pvpSkills;
                else skills = player.skills;
            }
            else if (pData is CsvData_Monster.MonsterData)
            {
                CsvData_Monster.MonsterData player = pData as CsvData_Monster.MonsterData;
                skills = player.skills;
            }
            else if (pData is CsvData_Summon.SummonData)
            {
                CsvData_Summon.SummonData player = pData as CsvData_Summon.SummonData;
                skills = player.skills;
            }
            TopGame.SvrData.ISvrHero heroData = pActor.GetActorParameter().GetSvrData() as TopGame.SvrData.ISvrHero;
            if (heroData != null)
            {
                heroSkills = heroData.GetSkills();
            }

            if (heroSkills != null)
            {
                if (m_vSkillByTags == null) m_vSkillByTags = new Dictionary<uint, Skill>();
                if (m_vSkills == null) m_vSkills = new Dictionary<uint, Skill>(skills.Length);
                if (m_vLearnedSkills == null) m_vLearnedSkills = new List<Skill>();
                for (int i = 0; i < heroSkills.Count; ++i)
                {
                    if (heroSkills[i].configData == null) continue;
                    Skill newSkill = AddSkill(heroSkills[i].skillId, heroSkills[i].level, heroSkills[i].configData);
#if UNITY_EDITOR
                    if (newSkill != null)
                        newSkill.gotLabel = heroSkills[i].gotLabel;
#endif
                }
            }
            else if (skills != null)
            {
                if (m_vSkillByTags == null) m_vSkillByTags = new Dictionary<uint, Skill>();
                if (m_vSkills == null) m_vSkills = new Dictionary<uint, Skill>(skills.Length);
                if (m_vLearnedSkills == null) m_vLearnedSkills = new List<Skill>();
                for (int i = 0; i < skills.Length; ++i)
                {
                    if (skills[i] == 0) continue;
                    var skillData = DataManager.getInstance().Skill.GetUnlockSkill(skills[i], level);
                    if (skillData == null)
                    {
#if UNITY_EDITOR
                        Debug.LogWarning(m_pActor.GetConfigID() + " 的技能：" + skills[i] + "  Lv:" + level + "  找不到数据");
#endif
                        continue;
                    }
                    AddSkill(skills[i], level, skillData);
                }
            }
        }
        //------------------------------------------------------
        public override Skill AddSkill(uint skillGroup, uint skillLevel, IContextData pData = null)
        {
            if (m_vSkills == null) return null;
            CsvData_Skill.SkillData skillData = null;
            if (pData != null) skillData = pData as CsvData_Skill.SkillData;
            if (skillData == null)
                skillData = Data.DataManager.getInstance().Skill.GetSkill(skillGroup, skillLevel);
            if (skillData != null)
            {
#if UNITY_EDITOR

                if (skillData.SkillDamage_skillDamageID_data == null)
                {
                    Log("技能[" + skillGroup + "   lv:" + skillLevel + "  为无效技能，没有具体的伤害!");
          //          return;
                }
#endif
                if (skillData.skillType == ESkillType.TeamSkill)
                {
                    GlobalSkillInfomation.AddSkill(m_pActor.GetGameModule(), m_pActor.GetAttackGroup(), skillLevel, pData);
                    return null;
                }
                
//                 //! limit times
//                 uint[] limitSkills = CsvData_SystemConfig.sysConfig.runSkillSection;
//                 if (limitSkills != null && limitSkills.Length == 3)
//                 {
//                     if (skillData.id >= limitSkills[0] && skillData.id <= limitSkills[1])
//                     {
//                         int cnt = 0;
//                         Skill dbSkill;
//                         foreach(var db in m_vSkills)
//                         {
//                             dbSkill = db.Value;
//                             if (dbSkill.configId >= limitSkills[0] && dbSkill.configId <= limitSkills[1])
//                             {
//                                 if (dbSkill.triggerTimes > 0) cnt +=dbSkill.triggerTimes;
//                                 else cnt++;
//                             }
//                         }
//                         if (cnt >= limitSkills[2])
//                         {
//                             return null;
//                         }
//                     }
//                 }

                Skill skill;
                if (m_vSkills.TryGetValue(skillGroup, out skill))
                {
                    if (skill.triggerTimes > 0)
                    {
                        skill.triggerTimes++;
                        if (m_vCallback != null)
                        {
                            for (int i = 0; i < m_vCallback.Count; ++i)
                            {
                                m_vCallback[i].OnSkillAddAndRemove(m_pActor, skill, true);
                            }
                        }
                        return skill;
                    }
                    FreeSkill(skill);
                    m_vSkills.Remove(skillGroup);
                }


                skill = MallocSkill();
                skill.nGuid = skillGroup;
                skill.configId = skillData.id;
                skill.bLearned = true;// heroLevel >= skillData.unLock;
                skill.skillData = skillData;
                skill.Tag = skillData.tag;
                skill.skillType = (byte)skillData.skillType;
                skill.skillLevel = (ushort)skillData.level;
                skill.abnormalLimit = skillData.abnormalLimit;
                skill.skillDamage = skillData.SkillDamage_skillDamageID_data;
                skill.skillDamageID = skillData.skillDamageID;
                skill.costType = skillData.costType;
                skill.costValue = skillData.costValue;
                skill.lockReset = skillData.targetReset;
                skill.canFaceToTarget = skillData.canFaceToTarget;
                skill.canLockHitFlyTarget = skillData.canLockHitFlyTarget;
                skill.gainSpType = skillData.gainSpType;
                skill.gainSpValue = skillData.GainSpValue;

                if (skillData.triggerTimes <= 0) skill.triggerTimes = -1;
                else skill.triggerTimes = skillData.triggerTimes;
                skill.triggerTimesClear = skillData.triggerClear;
                if (skillData.triggerType != null && skillData.triggerProbability != null && skillData.triggerType.Length == skillData.triggerProbability.Length)
                {
                    if (skill.nTriggerParam == null) skill.nTriggerParam = new Dictionary<ESkillTriggerType, Skill.TriggerParam>(skillData.triggerType.Length);
                    for (int t = 0; t < skillData.triggerType.Length; ++t)
                    {
                        Skill.TriggerParam param = new Skill.TriggerParam();
                        param.nRate = (byte)skillData.triggerProbability[t];
                        if (skillData.triggerParams != null && t < skillData.triggerParams.Length)
                            param.nParam = skillData.triggerParams[t];
                        skill.nTriggerParam.Add((ESkillTriggerType)(int)skillData.triggerType[t], param);
                        skill.nTriggerBit |= (uint)(1 << skillData.triggerType[t]);
                    }
                }
                else if (skillData.triggerType != null)
                {
                    for (int t = 0; t < skillData.triggerType.Length; ++t)
                    {
                        skill.nTriggerBit |= (uint)(1 << skillData.triggerType[t]);
                    }
                }
                skill.cd = (skillData.coolDown * 0.001f);
                skill.init_cd = (skillData.initialCoolDown * 0.001f);
                skill.init_config_cd = skill.init_cd;
                skill.runingCD = 0;

                skill.trigger_config_cd = (skillData.skillTriggerCoolDown * 0.001f);
                skill.trigger_cd = 0;

                skill.minAttack = (skillData.minAttack);
                skill.maxAttack = (skillData.maxAttack);
                skill.maxTarget = (skillData.distance);

                if (skill.abnormalLimit != 0) skill.interruptTime = (skillData.interruptTime * 0.001f);
                else skill.interruptTime = 0;

                short lockNum = skillData.targetNumbers;
                Framework.Data.ELockHitType lockType = skillData.targetType;
                if (lockType != Framework.Data.ELockHitType.None && lockNum > 0)
                {
                    FFloat lockDist = skillData.distance;
                    if (lockDist <= 0) lockDist = m_pActor.GetMaxTargetDistance();
                    if (skill.SetLockHitType(lockType, lockDist, lockNum, skillData.lockHeight, skillData.minLockHeight, skillData.maxLockHeight, skillData.lockFilterFlags))
                    {
                        if (skillData.targetCondition != null)
                        {
                            for (int i = 0; i < skillData.targetCondition.Length; ++i)
                            {
                                short LockRode = GlobalDef.INVALID_RODE;
                                Vector3Int param = new Vector3Int();
                                if (skillData.targetParameter1 != null && i < skillData.targetParameter1.Length)
                                    param.x = skillData.targetParameter1[i];
                                if (skillData.targetParameter2 != null && i < skillData.targetParameter2.Length)
                                    param.y = skillData.targetParameter2[i];
                                if (skillData.targetParameter3 != null && i < skillData.targetParameter3.Length)
                                    param.z = skillData.targetParameter3[i];
                                if (skillData.lockRode != null && i < skillData.lockRode.Length)
                                    LockRode = skillData.lockRode[i];
                                skill.AddLockCondition(skillData.targetCondition[i], param, LockRode);
                            }
                        }
                        else
                        {
                            skill.AddLockCondition(Framework.Data.ELockHitCondition.NearDistance, Vector3Int.zero);
                        }
                    }
                }

                OnAddSkill(skill);
                return skill;
            }
#if UNITY_EDITOR
            else
            {
                UnityEngine.Debug.LogWarning("技能添加失败:group:" + skillGroup + "  Lv:" + skillLevel);
            }
#endif
            return null;
        }
        //------------------------------------------------------
        protected override bool OnPreareAddCommand(Skill skill)
        {
            if (m_pActor == null) return false;
            switch ((ESkillType)skill.skillType)
            {
                case ESkillType.PowerSkill:
                    {
                        if (m_bAutoAllSkill)
                        {
                            CsvData_Player.PlayerData playerData = m_pActor.GetContextData() as CsvData_Player.PlayerData;
                            if (GlobalSkillInfomation.AddQueueSkill(m_pActor.GetGameModule(), m_pActor.GetAttackGroup(), this, skill.nGuid, 0))
                            {
                                return false;
                            }
                        }
                    }
                    break;
            }

            return true;
        }
        //------------------------------------------------------
        protected override void OnChangeCurrentSkill(Skill currentSkill, Skill preSkill)
        {
            base.OnChangeCurrentSkill(currentSkill, preSkill);
            if(preSkill!=null && m_pActor!=null)
            {
                if(m_pActor.GetAI()!=null)
                    m_pActor.GetAI().AddCatchData(currentSkill!=null? currentSkill:preSkill);
                Framework.BattlePlus.AIWrapper.DoSkillDoEndAI(m_pActor.GetAI(), (int)preSkill.nGuid, (int)preSkill.Tag, preSkill.skillType);
                Framework.BattlePlus.AIWrapper.OnGlobalSkillDoEnd(m_pActor.GetGameModule().aiSystem, null, m_pActor.GetInstanceID(), m_pActor.GetAttackGroup(), (int)preSkill.nGuid, (int)preSkill.Tag, preSkill.skillType);
            }
        }
        //------------------------------------------------------
        protected override bool InnerCheckSkill(Skill skill, ESkillTriggerType type = ESkillTriggerType.Count, int nParam = 0, bool bLessCheck = true)
        {
            if (m_fGlobalLastCDTime>0)
                return false;

            if (skill.skillType == (byte)ESkillType.Attack)
            {
                if (!BuffUtil.CanDoAttack(m_pActor))
                {
#if UNITY_EDITOR
                    Log("技能:" + skill.nGuid + " 有机制释放Buff");
#endif
                    return false;
                }
            }
            else
            {
                if (!BuffUtil.CanDoSkill(m_pActor))
                {
#if UNITY_EDITOR
                    Log("技能:" + skill.nGuid + " 有机制释放Buff");
#endif
                    return false;
                }
            }

            if (m_pActor.GetActorType() == EActorType.Player)
            {
                BattleStatus pStatus = BattleKits.GetBattleLogic<BattleStatus>(m_pActor.GetGameModule());
                if (pStatus != null)
                {
                    if (pStatus.PlayerSkillIsLock())
                    {
#if UNITY_EDITOR
                        Log("技能:" + skill.nGuid + " 技能被全局锁锁住");
#endif
                        return false;
                    }
                    if (skill.skillType == (byte)ESkillType.PowerSkill && pStatus.PlayerPowerSkillIsLock())
                    {
#if UNITY_EDITOR
                        Log("技能:" + skill.nGuid + " 技能被全局锁锁住");
#endif
                        return false;
                    }
                }
            }
            else if (m_pActor.GetActorType() == EActorType.Monster)
            {
                BattleStatus pStatus = BattleKits.GetBattleLogic<BattleStatus>(m_pActor.GetGameModule());
                if (pStatus != null)
                {
                    if (pStatus.MonsterSkillIsLock())
                    {
#if UNITY_EDITOR
                        Log("技能:" + skill.nGuid + " 技能被全局锁锁住");
#endif
                        return false;
                    }
                    if (skill.skillType == (byte)ESkillType.PowerSkill && pStatus.MonsterPowerSkillIsLock())
                    {
#if UNITY_EDITOR
                        Log("技能:" + skill.nGuid + " 技能被全局锁锁住");
#endif
                        return false;
                    }
                }
            }
            return base.InnerCheckSkill(skill, type, nParam, bLessCheck);
        }
        //------------------------------------------------------
        public override bool IsCanDriveSkill(Skill skill)
        {
            if (!base.IsCanDriveSkill(skill)) return false;
            RunerAgent agent = m_pActor.GetActorAgent() as RunerAgent;
            if (agent != null && agent.IsAILogic())
            {
                ESkillType skillType = ESkillType.Attack;
                if (skill != null) skillType = (ESkillType)skill.skillType;
                if (skillType == ESkillType.PassivitySkill) return true;
#if UNITY_EDITOR
                if (agent.GetAIState() > EAILogicType.Attack)
                {
                    if (DebugConfig.bSkillDebug)
                        Framework.Plugin.Logger.Warning("AI 状态:"+ agent.GetAIState() + "  不允许释放技能!");
                }
#endif
                return agent.GetAIState() <= EAILogicType.Attack;
            }
            return true;
        }
        //------------------------------------------------------
        public override void OnActionStateBegin(ActionState state)
        {
            if (m_fLockInterrupTime > 0 && m_pCurrentState != state)
            {
                if (m_pCurrentSkill != null && m_pCurrentState != null)
                {
                    if (m_vCallback != null)
                    {
                        foreach (var db in m_vCallback)
                            db.OnSkillInterruptState(m_pActor, m_pCurrentSkill, m_pCurrentState, false);
                    }
                }
            }
            base.OnActionStateBegin(state);
        }
        //------------------------------------------------------
        public override void OnActionStateEnd(ActionState state)
        {
            base.OnActionStateEnd(state);
        }
        //------------------------------------------------------
        protected override int DoPrepareSkill(Skill skill)
        {
            CsvData_Skill.SkillData skillData = skill.skillData as CsvData_Skill.SkillData;
            if (skillData == null) return  1;
            RunerAgent pAgent = m_pActor.GetActorAgent() as RunerAgent;
            if (pAgent == null) return 1;
            return base.DoPrepareSkill(skill);
        }
        //------------------------------------------------------
        protected override int OnSkillActionBefore(Skill skill, ActionState pState)
        {
            int result = base.OnSkillActionBefore(skill, pState);
            if (result == 0)
                return 0;
            if(skill.GetLockHitType() != Framework.Data.ELockHitType.None && skill.GetLockHitType() != Framework.Data.ELockHitType.Self)
            {
                if(skill.skillType != (byte)ESkillType.PassivitySkill)
                {
                    RunerAgent pAIAgent = m_pActor.GetActorAgent() as RunerAgent;
                    if (!m_pActor.IsAICanDoAttacking())
                    {
                        pAIAgent.PauseRunAlongPathPoint();
                        if (pAIAgent != null)
                            pAIAgent.UpdateAI(0);
                        pAIAgent.ResumeRunAlongPathPoint();
                    }
                    if (!m_pActor.IsAICanDoAttacking())
                    {
#if UNITY_EDITOR
                        if (DebugConfig.bSkillDebug)
                        {
                            if (skill.skillDamage != null)
                                Framework.Plugin.Logger.Warning(m_pActor.GetConfigID() + " 技能:" + (skill.skillData as CsvData_Skill.SkillData).Text_name_data?.textCN + "  不在攻击距离范围内");
                        }
#endif
                        if (pAIAgent != null && pAIAgent.GetAIState() == EAILogicType.GoTarget) return 0;
                        return 1;
                    }
                }
            }

            bool bOk = false;
            CsvData_Skill.SkillData skillData = skill.skillData as CsvData_Skill.SkillData;
            if (skillData == null) return 1;
            RunerAgent pAgent = m_pActor.GetActorAgent() as RunerAgent;
            if (pAgent == null) return 1;
            if (bOk && skill.skillType == (byte)ESkillType.PowerSkill)
            {
                AWorldNode pNode = m_pActor.GetWorld().GetRootNode();
                while (pNode != null)
                {
                    Actor pActor = pNode as Actor;
                    pNode = pNode.GetNext();
                    if (pActor != null && pActor != m_pActor && pActor.GetAttackGroup() == m_pActor.GetAttackGroup())
                    {
                        SkillInformation skill_system = pActor.GetSkill();
                        if (skill_system == null) continue;
                        skill_system.UseSkillByTrigger(ESkillTriggerType.TeamSkillPower);
                    }
                }
            }
            if (skill.skillData != null && skill.skillData is CsvData_Skill.SkillData)
                m_fGlobalLastCDTime = (skill.skillData as CsvData_Skill.SkillData).globalCoolDown * 0.001f;

//             if (m_fGlobalLastCDTime > 0)
//             {
//                 m_vCommandList.Clear();
//             }

            if (pState != null)
            {
                m_fSkillActionDurationTime = pState.GetDuration();
                if (skill.interruptTime > 0)
                {
                    m_fLockInterrupTime = skill.interruptTime;
                    if (m_vCallback != null)
                    {
                        foreach (var db in m_vCallback)
                            db.OnSkillInterruptState(m_pActor, skill, pState, true);
                    }
                }
                else
                {
                    if (m_fLockInterrupTime > 0 && m_pCurrentSkill != null && m_pCurrentState != null)
                    {
                        if (m_vCallback != null)
                        {
                            foreach (var db in m_vCallback)
                                db.OnSkillInterruptState(m_pActor, m_pCurrentSkill, m_pCurrentState, false);
                        }
                    }
                    m_fLockInterrupTime = 0;
                }
            }
            else
            {
                if (m_fLockInterrupTime > 0 && m_pCurrentSkill != null && m_pCurrentState != null)
                {
                    if (m_vCallback != null)
                    {
                        foreach (var db in m_vCallback)
                            db.OnSkillInterruptState(m_pActor, m_pCurrentSkill, m_pCurrentState, false);
                    }
                }
                m_fLockInterrupTime = 0;
            }

            if (bOk)
            {
                if (skillData.beginEventId != null)
                {
                    AEventSystemTrigger eventSystem = m_pActor.GetGameModule().eventSystem;
                    eventSystem.Begin();
                    eventSystem.ATuserData = m_pActor;
                    for (int i = 0; i < skillData.beginEventId.Length; ++i)
                    {
                        m_pActor.GetGameModule().OnTriggerEvent((int)skillData.beginEventId[i], false);
                    }
                    eventSystem.End();
                }
            }

            if (bOk && pState == null)
            {
                List<AWorldNode> vTarget = skill.GetLockTargets(m_pActor, false);
                if (vTarget != null && vTarget.Count > 0)
                {
                    BattleDamager damager = BattleKits.GetBattleLogic<BattleDamager>(m_pActor.GetGameModule());
                    if (damager != null)
                    {
                        ActorAttackData attack = ActorAttackData.DEFAULT;
                        attack.attacker_ptr = m_pActor;
                        attack.attacker_id = m_pActor.GetInstanceID();
                        attack.attacker_config_id = (int)m_pActor.GetConfigID();
                        attack.attacker_element_flags = m_pActor.GetElementFlags();
                        attack.attacker_type = m_pActor.GetActorType();
                        for (int i = 0; i < vTarget.Count; ++i)
                        {
                            attack.target_ptr = vTarget[i];
                            attack.target_type = vTarget[i].GetActorType();
                            attack.target_id = vTarget[i].GetInstanceID();
                            attack.target_config_id = (int)vTarget[i].GetConfigID();
                            attack.target_element_flags = vTarget[i].GetElementFlags();
                            attack.hit_position = vTarget[i].GetPosition();
                            damager.AddTickDamage(skill.skillDamage, attack);
                        }
                    }
                }
                else
                {
#if UNITY_EDITOR
                    if (DebugConfig.bSkillDebug)
                    {
                        Framework.Plugin.Logger.Warning("技能id[" + skill.nGuid + "]  + 索敌目标为空，触发失败");
                    }
#endif
                }
            }

            return 2;
        }
        //------------------------------------------------------
        protected override void OnSkillActionAfter(Skill skill, ActionState pState)
        {
            base.OnSkillActionAfter(skill, pState);
            m_pCurrentState = pState;

            if (m_pActor.GetAI() != null)
            {
                m_pActor.GetAI().AddCatchData(skill);
                m_pActor.GetAI().AddCatchData(pState);
            }
            //     if (skill.curTakingSlot == 0) return;
            //     if (!pState.HasEvent(EEventType.TakeSoltSkill)) return;
        }
        //------------------------------------------------------
        public override void AutoSkill(bool bAuto)
        {
            if (m_bAutoAllSkill == bAuto) return;
            m_bAutoAllSkill = bAuto;
            if (!bAuto)
            {
                GlobalSkillInfomation.RemoveQueueSkill(m_pActor.GetGameModule(), m_pActor.GetAttackGroup(), this);
            }
        }
        //------------------------------------------------------
        public FFloat GetGlobalCD()
        {
            return m_fGlobalLastCDTime;
        }
        //------------------------------------------------------
        protected override void OnInnerUpdate(FFloat fFrame)
        {
            base.OnInnerUpdate(fFrame);
            if(m_fGlobalLastCDTime>0)
            {
                m_fGlobalLastCDTime -= fFrame;
                if (m_fGlobalLastCDTime < 0) m_fGlobalLastCDTime = 0;
            }
            if(m_fLockInterrupTime>0)
            {
                m_fLockInterrupTime -= fFrame;
                if (m_fLockInterrupTime < 0) m_fLockInterrupTime = 0;
            }
            if (m_fSkillActionDurationTime > 0)
            {
                m_fSkillActionDurationTime -= fFrame;
                if (m_fSkillActionDurationTime < 0) m_fSkillActionDurationTime = 0;
            }
        }
    }
}

