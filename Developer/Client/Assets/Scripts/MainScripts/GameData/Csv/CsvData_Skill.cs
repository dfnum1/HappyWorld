/********************************************************************
类    名:   CsvData_Skill
作    者:	Happli
描    述:	
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.Data;
using Framework.Base;

namespace TopGame.Data
{
    public partial class CsvData_Skill : Data_Base
    {
        //-------------------------------------------
        public static bool IsValidData(CsvData_SkillDamage.SkillDamageData skillData)
        {
            if (skillData == null) return false;
            if (skillData.ValueTarget == null || skillData.ValueTarget.Length<=0) return false;
            int lenth = skillData.ValueTarget.Length;
            if (skillData.valueTypes == null || skillData.valueTypes.Length != lenth) return false;
            if (skillData.values == null || skillData.values.Length != lenth) return false;
            if (skillData.effectTypes == null || skillData.effectTypes.Length != lenth) return false;
            return true;
        }
        //-------------------------------------------
        public SkillData GetSkill(uint groupId, uint level)
        {
            if (groupId == 0) return null;
            if (level <= 0) level = 1;
            List<SkillData> vSkills;
            if (m_vData.TryGetValue(groupId, out vSkills) && vSkills.Count>0)
            {
                for (int i = vSkills.Count - 1; i >= 0; --i)
                {
                    if (level >= vSkills[i].level)
                        return vSkills[i];
                }
                if(level >= vSkills[0].level) return vSkills[0];
            }
            return null;
        }
        //-------------------------------------------
        public SkillData GetUnlockSkill(uint groupId, uint level)
        {
            if (groupId == 0) return null;
            if (level <= 0) level = 1;
            List<SkillData> vSkills;
            if (m_vData.TryGetValue(groupId, out vSkills) && vSkills.Count > 0)
            {
                for (int i = vSkills.Count - 1; i >= 0; --i)
                {
                    if (level >= vSkills[i].unLock)
                        return vSkills[i];
                }
                if (level >= vSkills[0].unLock) return vSkills[0];
            }
            return null;
        }
    }
}
