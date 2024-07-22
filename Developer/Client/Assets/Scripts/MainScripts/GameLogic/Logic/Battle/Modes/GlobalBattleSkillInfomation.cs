/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GlobalBattleSkillInfomation
作    者:	HappLI
描    述:	全局技能
*********************************************************************/
using System.Collections.Generic;
using TopGame.Data;
using TopGame.Core;
using Framework.Core;
using Framework.Base;
using ExternEngine;

namespace TopGame.Logic
{
    public class GlobalBattleSkillInfomation : GlobalSkillInfomation
    {
        protected override void OnAddSkill(byte attackGroup, uint skillLevel, IContextData baseData) 
        {
            if (baseData == null) return;
            Data.CsvData_Skill.SkillData skillData = baseData as Data.CsvData_Skill.SkillData;
            if (skillData == null) return;

            List<Skill> vSkill;
            if (m_vSkills.TryGetValue(attackGroup, out vSkill))
            {
//                 //! limit times
//                 uint[] limitSkills = CsvData_SystemConfig.sysConfig.runSkillSection;
//                 if (limitSkills != null && limitSkills.Length == 3)
//                 {
//                     if (skillData.id >= limitSkills[0] && skillData.id <= limitSkills[1])
//                     {
//                         int cnt = 0;
//                         Skill dbSkill;
//                         for (int i = 0; i < vSkill.Count; ++i)
//                         {
//                             dbSkill = vSkill[i];
//                             if (dbSkill.configId >= limitSkills[0] && dbSkill.configId <= limitSkills[1])
//                             {
//                                 if (dbSkill.triggerTimes > 0) cnt += dbSkill.triggerTimes;
//                                 else cnt++;
//                             }
//                         }
//                         if (cnt >= limitSkills[2])
//                         {
//                             return;
//                         }
//                     }
//                 }
                Skill checkSkill;
                for(int i = 0; i < vSkill.Count; ++i)
                {
                    checkSkill = vSkill[i];
                    if (checkSkill.configId == skillData.id)
                    {
                        if (checkSkill.triggerTimes > 0)
                        {
                            checkSkill.triggerTimes++;
                            return;
                        }
                        FreeSkill(checkSkill);
                        vSkill.Remove(checkSkill);
                        break;
                    }
                }
            }

            Skill skill = MallocSkill();
            skill.nGuid = skillData.id;
            skill.configId = skillData.id;
            skill.bLearned = true;// heroLevel >= skillData.unLock;
            skill.skillData = skillData;
            skill.Tag = skillData.tag;
            skill.skillType = (byte)skillData.skillType;
            skill.skillLevel = (ushort)skillData.level;
            skill.abnormalLimit = skillData.abnormalLimit;

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

            if(vSkill == null)
            {
                if (!m_vSkills.TryGetValue(attackGroup, out vSkill))
                {
                    vSkill = new List<Skill>(2);
                    m_vSkills.Add(attackGroup, vSkill);
                }
            }

            vSkill.Add(skill);
        }
    }
}

