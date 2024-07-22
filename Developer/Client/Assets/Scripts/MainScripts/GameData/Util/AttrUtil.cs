// ********************************************************************
// 生成日期:	1:11:2020 13:16
// 类    名: 	CalcActorAtribute
// 作    者:	HappLI
// 描    述:	属性计算
// *********************************************************************/
using Framework.Core;
using Framework.Data;
using System.Collections.Generic;

namespace TopGame.Data
{
    public static class AttrUtil
    {
        //! 属性转战力API
        public static int GetExternAttrFightPower(ExternAttrParam externAttr)
        {
            if (!externAttr.IsValid()) return 0;
            int fightPower = 0;
//             Dictionary<uint, CsvData_Power.PowerData> powerDatas = Data.DataManager.getInstance().Power.datas;
//             if (powerDatas != null)
//             {
// 
//                 for (int i = 0; i < externAttr.valueTypes.Length; i++)
//                 {
//                     if (powerDatas.ContainsKey(externAttr.attrTypes[i]))
//                     {
//                         if (externAttr.values[i] > 0)
//                             fightPower += (int)((externAttr.values[i] + powerDatas[externAttr.attrTypes[i]].powerReset) * powerDatas[externAttr.attrTypes[i]].powerParam);
//                     }
//                 }
//             }
            return fightPower;
        }
        //-------------------------------------------------
        public static BaseAttrArrayParam BuildBaseAttrParam(IContextData pData, float growAbilityRate = 1)
        {
            if (pData == null) return BaseAttrArrayParam.DEF;
            BaseAttrArrayParam bassAttrParam = new BaseAttrArrayParam();
            if (pData is CsvData_Player.PlayerData)
            {
                CsvData_Player.PlayerData playData = pData as CsvData_Player.PlayerData;
                bassAttrParam.elementFlag = playData.element;
                bassAttrParam.baseAttrTypes = playData.attributeTypes;
                bassAttrParam.baseValueTypes = playData.valueTypes;
                bassAttrParam.baseValues = playData.baseValues;
                bassAttrParam.growValues = playData.growValues;
                bassAttrParam.growAbilityRate = growAbilityRate;
            }
            else if (pData is CsvData_Monster.MonsterData)
            {
                CsvData_Monster.MonsterData playData = pData as CsvData_Monster.MonsterData;
                bassAttrParam.elementFlag = playData.element;
                bassAttrParam.baseAttrTypes = playData.attributeTypes;
                bassAttrParam.baseValueTypes = playData.valueTypes;
                bassAttrParam.baseValues = playData.baseValues;
                bassAttrParam.growValues = playData.growValues;
                bassAttrParam.growAbilityRate = growAbilityRate;
            }
            else if (pData is CsvData_Summon.SummonData)
            {
                CsvData_Summon.SummonData playData = pData as CsvData_Summon.SummonData;
                bassAttrParam.elementFlag = playData.element;
                bassAttrParam.baseAttrTypes = playData.baseAttributeTypes;
                bassAttrParam.baseValueTypes = playData.baseTypes;
                bassAttrParam.baseValues = playData.baseValues;
                bassAttrParam.growValues = playData.growValues;
                bassAttrParam.growAbilityRate = growAbilityRate;
            }
            else
                bassAttrParam.Clear();
            return bassAttrParam;
        }
    }
}

// 
// using System.Collections.Generic;
// using TopGame.Core;
// using UnityEngine;
// #if UNITY_EDITOR
// using UnityEditor;
// #endif
// namespace TopGame.Data
// {
// #if UNITY_EDITOR
//     public class CalcAttrDebuger
//     {
//         public Framework.Plugin.AT.IUserData userData;
//         public Vector2[] vBases = new Vector2[(int)EBuffAttrType.Count];
//         public Vector2[] vElementEquips = new Vector2[(int)EBuffAttrType.Count];
//         public Vector2[] SynthesisEquips = new Vector2[(int)EBuffAttrType.Count];
//         public Vector2[] StartEles = new Vector2[(int)EBuffAttrType.Count];
//         public Vector2[] guildExtern = new Vector2[(int)EBuffAttrType.Count];
//         public Vector2[] bondAtts = new Vector2[(int)EBuffAttrType.Count];
//         public Vector2[] skillDatas = new Vector2[(int)EBuffAttrType.Count];
//         public Vector2[] soulAttrs = new Vector2[(int)EBuffAttrType.Count];
//         public Vector2[] chapterAttrs = new Vector2[(int)EBuffAttrType.Count];
//         public void Draw()
//         {
//             for(int i = 0; i < (int)EBuffAttrType.Count; ++i)
//             {
//                 EditorGUILayout.LabelField(((EBuffAttrType)i).ToString());
//                 EditorGUI.indentLevel++;
//                 EditorGUILayout.LabelField("Base:" + vBases[i].x + "   rate:" + vBases[i].y);
//                 EditorGUILayout.LabelField("ElementEquips:" + vElementEquips[i].x + "   rate:" + vElementEquips[i].y);
//                 EditorGUILayout.LabelField("SynthesisEquips:" + SynthesisEquips[i].x + "   rate:" + SynthesisEquips[i].y);
//                 EditorGUILayout.LabelField("StartEles:" + StartEles[i].x + "   rate:" + StartEles[i].y);
//                 EditorGUILayout.LabelField("guildExtern:" + guildExtern[i].x + "   rate:" + guildExtern[i].y);
//                 EditorGUILayout.LabelField("bondAtts:" + bondAtts[i].x + "   rate:" + bondAtts[i].y);
//                 EditorGUILayout.LabelField("skillDatas:" + skillDatas[i].x + "   rate:" + skillDatas[i].y);
//                 EditorGUILayout.LabelField("soulAttrs(战斗时计算):" + soulAttrs[i].x + "   rate:" + soulAttrs[i].y);
//                 EditorGUILayout.LabelField("chapterAttrs(战斗时计算):" + chapterAttrs[i].x + "   rate:" + chapterAttrs[i].y);
//                 EditorGUI.indentLevel--;
//             }
//         }
//         public void Clear()
//         {
//             userData = null;
//             System.Array.Clear(vElementEquips, 0, vElementEquips.Length);
//             System.Array.Clear(SynthesisEquips, 0, SynthesisEquips.Length);
//             System.Array.Clear(StartEles, 0, StartEles.Length);
//             System.Array.Clear(guildExtern, 0, guildExtern.Length);
//             System.Array.Clear(bondAtts, 0, bondAtts.Length);
//             System.Array.Clear(skillDatas, 0, skillDatas.Length);
//             System.Array.Clear(soulAttrs, 0, soulAttrs.Length);
//             System.Array.Clear(chapterAttrs, 0, chapterAttrs.Length);
//         }
//         static CalcAttrDebuger ms_pCurrentDebuger = null;
//         public static void SetCurrentDebuger(CalcAttrDebuger debuger)
//         {
//              ms_pCurrentDebuger = debuger;
//         }
//         public static CalcAttrDebuger CurrentDebuger
//         {
//             get { return ms_pCurrentDebuger; }
//         }
//     }
// #endif
//     public struct CalcExternAttrParam
//     {
//         private bool m_bTeamCatch;
//         //
//         public SvrData.ElementEquip[] vElementEquips; //count=AVATAR_SLOT_COUNT
//         public List<SvrData.Equip> SynthesisEquips;
//         public List<SvrData.StarSkyDB.StarElement> StartEles;
//         public CsvData_Buff.BuffData guildExtern;
//         public List<CsvData_Buff.BuffData> bondAtts;
//         public List<CsvData_Skill.SkillData> skillDatas
//         {
//             get
//             {
//                 return CalcActorAtribute.TempSkillList;
//             }
//         }
//         public void ClearSkills()
//         {
//             skillDatas.Clear();
//         }
//         public void AddSkill(CsvData_Skill.SkillData skillData, bool bClear = false)
//         {
//             if (bClear) skillDatas.Clear();
//             if (skillData == null || skillData.Buff_attrBuffs_data == null) return;
//             skillDatas.Add(skillData);
//         }
// 
//         public void Clear()
//         {
//             if (!m_bTeamCatch)
//             {
//                 ClearSkills();
//                 if (SynthesisEquips != null) SynthesisEquips.Clear();
//                 return;
//             }
//             if(vElementEquips!=null)
//                 System.Array.Clear(vElementEquips,0, vElementEquips.Length);
//             if (SynthesisEquips != null) SynthesisEquips.Clear();
//             if (StartEles != null) StartEles.Clear();
//             guildExtern = null;
//             
//             if (bondAtts != null) bondAtts.Clear();
//         }
//         static CalcExternAttrParam ms_TempParam =new CalcExternAttrParam() { vElementEquips = new SvrData.ElementEquip[SvrData.Hero.AVATAR_SLOT_COUNT], m_bTeamCatch = true };
//         public static CalcExternAttrParam TempParam
//         {
//             get
//             {
//                 ms_TempParam.Clear();
//                 return ms_TempParam;
//             }
//         }
//     }
//     public class CalcActorAtribute
//     {
//         private static List<CsvData_Skill.SkillData> ms_TempSkillList = new List<CsvData_Skill.SkillData>();
//         protected static float[] ms_TempAttr = new float[(int)Core.EAttrType.Num];
//         public static List<CsvData_Skill.SkillData> TempSkillList
//         {
//             get { return ms_TempSkillList; }
//         }
//         //------------------------------------------------------
//         public static float[] TempAttr
//         {
//             get { return ms_TempAttr; }
//         }
//         //------------------------------------------------------
//         public static float[] CalcAttribute(IContextData pData, uint level, float growAbilityRate, CalcExternAttrParam externParam, Framework.Plugin.AT.IUserData pPointer = null)
//         {
//             if (pData == null) return null;
// #if UNITY_EDITOR
//             if (CalcAttrDebuger.CurrentDebuger != null)
//             {
//                 CalcAttrDebuger.CurrentDebuger.Clear();
//                 CalcAttrDebuger.CurrentDebuger.userData = pPointer;
//             }
// #endif
//             System.Array.Clear(ms_TempAttr, 0, ms_TempAttr.Length);
//             for (EAttrType i = EAttrType.MaxHp; i < EAttrType.Num; ++i)
//                 ms_TempAttr[(int)i] = CalcAttribute(i, pData, level, growAbilityRate, externParam, pPointer);
//             return ms_TempAttr;
//         }
//         //------------------------------------------------------
//         public static CsvData_Skill.SkillData[] GetDataSkills(IContextData pData)
//         {
//             if (pData == null) return null;
//             if (pData is CsvData_Player.PlayerData) return (pData as CsvData_Player.PlayerData).Skill_skills_data;
//             if (pData is CsvData_Mecha.MechaData) return (pData as CsvData_Mecha.MechaData).Skill_skills_data;
//             if (pData is CsvData_Monster.MonsterData) return (pData as CsvData_Monster.MonsterData).Skill_skills_data;
//             if (pData is CsvData_Summon.SummonData) return (pData as CsvData_Summon.SummonData).Skill_skills_data;
//             return null;
//         }
//         //------------------------------------------------------
//         public static float CalcAttribute(EAttrType type, IContextData pData, uint level, float growAbilityRate, CalcExternAttrParam externParam, Framework.Plugin.AT.IUserData pPointer = null)
//         {
//             float fEquipAttr = GetEquipAttr(type, externParam.vElementEquips, pPointer);
//             float fSynthesisEquipAttr = GetSynthesisEquipAttr(type, externParam.SynthesisEquips);
// 
//             float appendGuildValue = 0;
//             float appendGuildRate = 0;
//             GetBuffExternAttr(type, ref appendGuildValue, ref appendGuildRate, externParam.guildExtern);
// 
//             float appendBondValue = 0;
//             float appendBondRate = 0;
//             GetBondAttr(type, ref appendBondValue, ref appendBondRate, externParam.bondAtts);
// 
//             float appendSkillValue = 0;
//             float appendSkillRate = 0;
//             GetSkillExternAttr(type, ref appendSkillValue, ref appendSkillRate, externParam.skillDatas);
//             float appendRate = appendGuildRate + appendBondRate + appendSkillRate;
//             float baseValue = 0;
//             uint elementFlag = 0;
//             if (pData is CsvData_Player.PlayerData)
//             {
//                 CsvData_Player.PlayerData playData = pData as CsvData_Player.PlayerData;
//                 elementFlag = playData.element;
//                 switch (type)
//                 {
//                     case EAttrType.MaxHp: baseValue += playData.baseHp + playData.growHp * growAbilityRate; break;
//                     case EAttrType.MaxSp: baseValue += playData.baseSp; break;
//                     case EAttrType.HpRec: baseValue += playData.baseHpRegeneration + playData.growHpRegeneration * growAbilityRate; break;
//                     case EAttrType.SpRec: baseValue += playData.baseSpRegeneration + playData.growSpRegeneration * growAbilityRate; break;
//                     case EAttrType.Attack: baseValue += playData.baseAttack  + playData.growAttack * growAbilityRate; break;
//                     case EAttrType.Defense: baseValue += playData.baseDefense  + playData.growDefense * growAbilityRate; break;
//                     case EAttrType.Crit: baseValue += playData.baseCritDamage ; break;
//                     case EAttrType.CritRate: baseValue += playData.baseCrit ; break;
//                     case EAttrType.AttackSpeedRate: baseValue += playData.baseAttackSpeed ; break;
//                     case EAttrType.Thorn: baseValue += playData.baseThorn  + playData.growThorn * growAbilityRate; break;
//                 }
//             }
//             else if (pData is CsvData_Mecha.MechaData)
//             {
//                 CsvData_Mecha.MechaData playData = pData as CsvData_Mecha.MechaData;
//                 switch (type)
//                 {
//                     case EAttrType.MaxHp: baseValue += playData.baseHp + playData.growHp * growAbilityRate ; break;
//                     case EAttrType.MaxSp: baseValue +=  fEquipAttr; break;
//                     case EAttrType.HpRec: baseValue += playData.baseHpRegeneration  + playData.growHpRegeneration * growAbilityRate; break;
//                     case EAttrType.SpRec: baseValue += fEquipAttr; break;
//                     case EAttrType.Attack: baseValue += playData.baseAttack  + playData.growAttack * growAbilityRate; break;
//                     case EAttrType.Defense: baseValue += playData.baseDefense  + playData.growDefense * growAbilityRate; break;
//                     case EAttrType.Crit: baseValue += playData.baseCritDamage ; break;
//                     case EAttrType.CritRate: baseValue += playData.baseCrit ; break;
//                     case EAttrType.AttackSpeedRate: baseValue += playData.baseAttackSpeed ; break;
//                     case EAttrType.Thorn: baseValue += playData.baseThorn  + playData.growThorn * growAbilityRate; break;
//                 }
//             }
//             else if (pData is CsvData_Monster.MonsterData)
//             {
//                 CsvData_Monster.MonsterData playData = pData as CsvData_Monster.MonsterData;
//                 elementFlag = playData.element;
//                 switch (type)
//                 {
//                     case EAttrType.MaxHp: baseValue += playData.baseHp  + playData.growHp * growAbilityRate; break;
//                     case EAttrType.MaxSp: baseValue += playData.baseSp ; break;
//                     case EAttrType.HpRec: baseValue += playData.baseHpRegeneration  + playData.growHpRegeneration * growAbilityRate; break;
//                     case EAttrType.SpRec: baseValue += playData.baseSpRegeneration  + playData.growSpRegeneration * growAbilityRate; break;
//                     case EAttrType.Attack: baseValue += playData.baseAttack  + playData.growAttack * growAbilityRate; break;
//                     case EAttrType.Defense: baseValue += playData.baseDefense  + playData.growDefense * growAbilityRate; break;
//                     case EAttrType.Crit: baseValue += playData.baseCritDamage ; break;
//                     case EAttrType.CritRate: baseValue += playData.baseCrit ; break;
//                     case EAttrType.AttackSpeedRate: baseValue += playData.baseAttackSpeed ; break;
//                     case EAttrType.Thorn: baseValue += playData.baseThorn  + playData.growThorn * growAbilityRate; break;
//                     case EAttrType.Posture: baseValue += playData.basePosture  + playData.growPosture * growAbilityRate; break;
//                     case EAttrType.PostureRec: baseValue += playData.postureRecoverTime; break;
//                 }
//             }
//             else if (pData is CsvData_Summon.SummonData)
//             {
//                 CsvData_Summon.SummonData playData = pData as CsvData_Summon.SummonData;
//                 elementFlag = playData.element;
//                 switch (type)
//                 {
//                     case EAttrType.MaxHp: baseValue += playData.baseHp ; break;
//                     case EAttrType.MaxSp: baseValue += playData.baseSp ; break;
//                     case EAttrType.HpRec: baseValue += playData.baseHpRegeneration ; break;
//                     case EAttrType.SpRec: baseValue += playData.baseSpRegeneration ; break;
//                     case EAttrType.Attack: baseValue += playData.baseAttack ; break;
//                     case EAttrType.Defense: baseValue += playData.baseDefense ; break;
//                     case EAttrType.Crit: baseValue += playData.baseCritDamage ; break;
//                     case EAttrType.CritRate: baseValue += playData.baseCrit ; break;
//                     case EAttrType.AttackSpeedRate: baseValue += playData.attackSpeed ; break;
//                     case EAttrType.Thorn: baseValue += playData.baseThorn ; break;
//                 }
//             }
//             float fStartEleAttr = 0;
//             if(elementFlag!=0) fStartEleAttr = GetStartEleAttr(type, externParam.StartEles, elementFlag);
// 
// #if UNITY_EDITOR
//             if (CalcAttrDebuger.CurrentDebuger!=null)
//             {
//                 CalcAttrDebuger.CurrentDebuger.vBases[(int)type].x = baseValue;
//                 CalcAttrDebuger.CurrentDebuger.vElementEquips[(int)type].x = fEquipAttr;
//                 CalcAttrDebuger.CurrentDebuger.SynthesisEquips[(int)type].x = fSynthesisEquipAttr;
//                 CalcAttrDebuger.CurrentDebuger.StartEles[(int)type].x = fStartEleAttr;
//                 CalcAttrDebuger.CurrentDebuger.guildExtern[(int)type].x = appendGuildValue;
//                 CalcAttrDebuger.CurrentDebuger.bondAtts[(int)type].x = appendBondValue;
//                 CalcAttrDebuger.CurrentDebuger.skillDatas[(int)type].x = appendSkillValue;
// 
//                 CalcAttrDebuger.CurrentDebuger.guildExtern[(int)type].y = appendGuildRate;
//                 CalcAttrDebuger.CurrentDebuger.bondAtts[(int)type].y = appendBondRate;
//                 CalcAttrDebuger.CurrentDebuger.skillDatas[(int)type].y = appendSkillRate;
//             }
// #endif
//             float finalValue = baseValue + appendGuildValue + appendBondValue + appendSkillValue + fEquipAttr + fSynthesisEquipAttr + fStartEleAttr;
//             return finalValue + finalValue* appendRate* Framework.Core.CommonUtility.WAN_RATE;
//         }
//         //------------------------------------------------------
//         public static void CalcExternAttr(Framework.Plugin.AT.IUserData pPointer, CalcExternAttrParam externParam)
//         {
//             if (pPointer == null) return;
//             ActorParameter actorParam = pPointer as ActorParameter;
//             if (actorParam == null) return;
//             uint elementFlag = actorParam.GetElementFlags();
//             actorParam.GetAttr().ResetExtern();
//             ExternAttr[] externAttr = actorParam.GetAttr().arExternAttr;
//             if (externParam.vElementEquips != null)
//             {
//                 CsvData_ElementEquip.ElementEquipData equipData = null;
//                 for (int i = 0; i < externParam.vElementEquips.Length; ++i)
//                 {
//                     if (externParam.vElementEquips[i] == null) continue;
//                     equipData = externParam.vElementEquips[i].equipData;
//                     if (equipData == null) continue;
// 
//                     uint[] attritype = equipData.effectTypes;
//                     int[] values = equipData.values;
//                     uint[] valuetype = equipData.valueTypes;
// 
//                     if (!(attritype.Length == values.Length && values.Length == valuetype.Length)) continue;
//                     for (int j = 0; j < attritype.Length; j++)
//                     {
//                         int currentType = (int)attritype[j];
// 
//                         //! base attribute
//                         if (currentType < (int)EAttrType.Num || currentType >= externAttr.Length) continue;
//                         if (valuetype[j] == (int)EValueType.Value) externAttr[currentType].value += values[j];
//                         else externAttr[currentType].rate += values[j] * Framework.Core.CommonUtility.WAN_RATE;
//                     }
// 
//                     CsvData_ElementEquipAttr.ElementEquipAttrData randData;
//                     for (int k = 0; k < externParam.vElementEquips[i].randomAttributes.Count; k++)
//                     {
//                         randData = externParam.vElementEquips[i].randomAttributes[k].attrData;
//                         if (randData == null) continue;
//                         int typevalue = externParam.vElementEquips[i].randomAttributes[k].value;
//                         int currentType = (int)randData.effectTypes;
//                         if (currentType >= externAttr.Length) continue;
// 
//                         if (randData.element > 1)
//                         {
//                             float fTemp = 0;
//                             if (randData.valueTypes == (int)EValueType.Value) fTemp += typevalue;
//                             else
//                             {
//                                 fTemp += typevalue * Framework.Core.CommonUtility.WAN_RATE;
//                             }
// 
//                             for (Base.EElementType ele = Base.EElementType.None + 1; ele < Base.EElementType.Count; ++ele)
//                             {
//                                 if ((randData.element & (1 << (int)ele)) != 0)
//                                 {
//                                     if (randData.valueTypes == (int)EValueType.Value)
//                                         externAttr[currentType].element[(int)ele].x += fTemp;
//                                     else
//                                         externAttr[currentType].element[(int)ele].y += fTemp;
//                                 }
//                             }
//                             continue;
//                         }
// 
//                         //! base attribute
//                         if (currentType <= (int)EAttrType.Num) continue;
//                         if (randData.valueTypes == (int)EValueType.Value) externAttr[currentType].value += typevalue;
//                         else
//                         {
//                             externAttr[currentType].rate += typevalue * Framework.Core.CommonUtility.WAN_RATE;
//                         }
//                     }
//                 }
//             }
//             if(externParam.guildExtern !=null)
//             {
//                 CalcBuffAppendExternAttr(ref externAttr, externParam.guildExtern);    
//             }
//             if (externParam.bondAtts != null)
//             {
//                 for(int j =0; j < externParam.bondAtts.Count; ++j)
//                 {
//                     CalcBuffAppendExternAttr(ref externAttr, externParam.bondAtts[j]);
//                 }
//             }
//             if (elementFlag !=0 && externParam.StartEles != null)
//             {
//                 CsvData_ConstellationLv.ConstellationLvData lvData = null;
//                 for (int i = 0; i < externParam.StartEles.Count; ++i)
//                 {
//                     lvData = externParam.StartEles[i].Config;
//                     if (lvData == null || lvData.attrTypes == null || lvData.valueTypes == null || lvData.values == null) continue;
//                     if (lvData.attrTypes.Length != lvData.valueTypes.Length || lvData.valueTypes.Length != lvData.values.Length) continue;
//                     if ( (lvData.element & elementFlag) == 0) continue;
//                     for (int j = 0; j < lvData.attrTypes.Length; ++j)
//                     {
//                         if (lvData.attrTypes[j] < (uint)EAttrType.Num)
//                             continue;
// 
//                         if (lvData.valueTypes[i] == (uint)EValueType.Value)
//                             externAttr[lvData.attrTypes[i]].value += lvData.values[i];
//                         else if (lvData.valueTypes[i] == (uint)EValueType.Rate)
//                             externAttr[lvData.attrTypes[i]].value += lvData.values[i] * Framework.Core.CommonUtility.WAN_RATE;
//                     }
//                 }
//             }
//             if(externParam.skillDatas!=null)
//             {
//                 CsvData_Skill.SkillData skillData;
//                 for (int k = 0; k < externParam.skillDatas.Count; ++k)
//                 {
//                     skillData = externParam.skillDatas[k];
//                     if(skillData == null) continue;
//                     if (skillData.Buff_attrBuffs_data == null) continue;
//                     for (int j = 0; j < skillData.Buff_attrBuffs_data.Length; ++j)
//                     {
//                         CalcBuffAppendExternAttr(ref externAttr, skillData.Buff_attrBuffs_data[j]);
//                     }
//                 }
//             }
//         }
//         //------------------------------------------------------
//         static void CalcBuffAppendExternAttr(ref ExternAttr[] externAttr, CsvData_Buff.BuffData extern_attr)
//         {
//             if (extern_attr == null) return;
//             if (extern_attr.attrTypes != null && extern_attr.values != null && extern_attr.valueTypes != null)
//             {
//                 if (extern_attr.attrTypes.Length == extern_attr.values.Length && extern_attr.values.Length == extern_attr.valueTypes.Length)
//                 {
//                     for (int i = 0; i < extern_attr.attrTypes.Length; ++i)
//                     {
//                         if (extern_attr.attrTypes[i] < (uint)EAttrType.Num)
//                             continue;
// 
//                         if (extern_attr.valueTypes[i] == (uint)EValueType.Value)
//                             externAttr[extern_attr.attrTypes[i]].value += extern_attr.values[i];
//                         else if (extern_attr.valueTypes[i] == (uint)EValueType.Rate)
//                             externAttr[extern_attr.attrTypes[i]].value += extern_attr.values[i] * Framework.Core.CommonUtility.WAN_RATE;
//                     }
//                 }
//             }
//         }
//         //------------------------------------------------------
//         public static float GetEquipAttr(EAttrType type, SvrData.ElementEquip[] vEquips, Framework.Plugin.AT.IUserData pPointer = null)
//         {
//             if (vEquips == null) return 0;
//     
//             float fCalc = 0;
//             CsvData_ElementEquip.ElementEquipData equipData = null;
//             for(int i = 0; i < vEquips.Length; ++i)
//             {
//                 float fTemp = 0;
//                 if (vEquips[i] == null) continue;
//                 equipData = vEquips[i].equipData;
//                 if (equipData == null) continue;
// 
//                 uint[] attritype = equipData.effectTypes;
//                 int[] values = equipData.values;
//                 uint[] valuetype = equipData.valueTypes;
// 
//                 if(!(attritype.Length == values.Length && values.Length == valuetype.Length)) continue;
//                 for (int j=0;j<attritype.Length;j++)
//                 {
//                     EAttrType currentType = (EAttrType)attritype[j];
//                     if (currentType == type)
//                     {
//                         if (valuetype[j] == (int)EValueType.Value) fTemp += values[j];
//                         else
//                         {
//                             fTemp += values[j] * Framework.Core.CommonUtility.WAN_RATE;
//                         }
//                     }
//                 }
//                 fCalc += fTemp;
//             }
//             for (int i = 0; i < vEquips.Length; ++i)
//             {
//                 float fTemp = 0;
//                 if (vEquips[i] == null || vEquips[i].randomAttributes == null) continue;
// 
//                 for (int k = 0; k < vEquips[i].randomAttributes.Count; k++)
//                 {
//                     int typevalue = vEquips[i].randomAttributes[k].value;
//                     CsvData_ElementEquipAttr.ElementEquipAttrData randData = vEquips[i].randomAttributes[k].attrData;
//                     if (randData == null) continue;
//                     EAttrType currentType = (EAttrType)randData.effectTypes;
//                     if (currentType == type)
//                     {
//                         //不属于元素属性
//                         if (randData.element <= 1)
//                         {
//                             if (randData.valueTypes == (int)EValueType.Value) fTemp += typevalue;
//                             else
//                             {
//                                 float convertData = typevalue * Framework.Core.CommonUtility.WAN_RATE;
//                                 if (currentType < EAttrType.Num)
//                                 {
//                                     if (IsBaseAttr((int)currentType))
//                                         fTemp += fTemp * convertData;
//                                     else
//                                     {
//                                         fTemp += convertData;
//                                     }
//                                 }
//                                 else
//                                 {
//                                     fTemp += convertData;
//                                 }
//                             }
//                         }
//                     }
//                 }
//                 fCalc += fTemp;
//             }
// 
//             return fCalc;
//         }
//         //------------------------------------------------------
//         static float GetSynthesisEquipAttr(EAttrType type, List<SvrData.Equip> SynthesisEquips = null)
//         {
//             if (SynthesisEquips == null) return 0;
// 
//             float fCalc = 0;
//             CsvData_EquipSynthesis.EquipSynthesisData equipData = null;
//             for (int i = 0; i < SynthesisEquips.Count; ++i)
//             {
//                 if (SynthesisEquips[i] == null) continue;
//                 equipData = SynthesisEquips[i].equipSynthesisData;
//                 if (equipData == null || equipData.attributeType == null || equipData.valueTypes ==null || equipData.attributeValue == null) continue;
//                 if (equipData.attributeType.Length != equipData.valueTypes.Length || equipData.attributeType.Length != equipData.attributeValue.Length) continue;
//                 for (int j = 0; j < equipData.attributeType.Length; ++j)
//                 {
//                     if(equipData.attributeType[j] == (uint)type)
//                     {
//                         if(equipData.valueTypes[j] == (uint)EValueType.Value)
//                             fCalc += equipData.attributeValue[j];
//                         else if (equipData.valueTypes[j] == (uint)EValueType.Rate)
//                             fCalc += equipData.attributeValue[j] *Framework.Core.CommonUtility.WAN_RATE;
//                         break;
//                     }
//                 }
//             }
//             return fCalc;
//         }
//         //------------------------------------------------------
//         static void GetBuffExternAttr(EAttrType type, ref float baseValue, ref float valueRate, CsvData_Buff.BuffData externData = null)
//         {
//             if (externData == null) return;
//             if (externData.attrTypes == null || externData.values == null || externData.valueTypes == null)
//                 return;
//             if (externData.attrTypes.Length != externData.values.Length || externData.values.Length != externData.valueTypes.Length)
//                 return;
// 
//             for (int i = 0; i < externData.attrTypes.Length; ++i)
//             {
//                 if (externData.attrTypes[i] != (uint)type) continue;
//                 if (externData.valueTypes[i] == (uint)EValueType.Value)
//                     baseValue += externData.values[i];
//                 else if (externData.valueTypes[i] == (uint)EValueType.Rate)
//                     valueRate += externData.values[i];
//             }
//         }
//         //------------------------------------------------------
//         static void GetBondAttr(EAttrType type, ref float baseValue, ref float valueRate, List<CsvData_Buff.BuffData> attrs = null)
//         {
//             baseValue = 0;
//             valueRate = 0;
//             if (attrs == null) return;
//             for(int i =0; i < attrs.Count; ++i)
//             {
//                 GetBuffExternAttr(type, ref baseValue, ref valueRate, attrs[i]);
//             }
//         }
//         //------------------------------------------------------
//         static void GetSkillExternAttr(EAttrType type, ref float baseValue, ref float valueRate, List<CsvData_Skill.SkillData> skills = null)
//         {
//             if (skills == null) return;
//             CsvData_Buff buffDater = Data.DataManager.getInstance().Buff;
//             for (int i = 0; i < skills.Count; ++i)
//             {
//                 if (skills[i].attrBuffs == null) continue;
//                 for(int j =0; j < skills[i].Buff_attrBuffs_data.Length; ++j)
//                 {
//                     GetBuffExternAttr(type, ref baseValue, ref valueRate, skills[i].Buff_attrBuffs_data[j]);
//                 }
//             }
//         }
//         //------------------------------------------------------
//         static float GetStartEleAttr(EAttrType type, List<SvrData.StarSkyDB.StarElement> StarElements = null, uint elementFlag =0)
//         {
//             if (StarElements == null) return 0;
//             float fCalc = 0;
//             CsvData_ConstellationLv.ConstellationLvData lvData = null;
//             for (int i = 0; i < StarElements.Count; ++i)
//             {
//                 lvData = StarElements[i].Config;
//                 if (lvData == null || lvData.attrTypes == null || lvData.valueTypes == null || lvData.values == null) continue;
//                 if (lvData.attrTypes.Length != lvData.valueTypes.Length || lvData.valueTypes.Length != lvData.values.Length) continue;
//                 if ((elementFlag & lvData.element) == 0) continue;
//                 for (int j = 0; j < lvData.attrTypes.Length; ++j)
//                 {
//                     if (lvData.attrTypes[j] == (uint)type)
//                     {
//                         if (lvData.valueTypes[j] == (uint)EValueType.Value)
//                             fCalc += lvData.values[j];
//                         else if (lvData.valueTypes[j] == (uint)EValueType.Rate)
//                             fCalc += lvData.values[j] * Framework.Core.CommonUtility.WAN_RATE;
//                         break;
//                     }
//                 }
//             }
//             return fCalc;
//         }
//         //------------------------------------------------------
//         static bool IsBaseAttr(int type)
//         {
//             if (type == (int)EAttrType.MaxHp) return true;
//             if (type == (int)EAttrType.Attack) return true;
//             if (type == (int)EAttrType.Defense) return true;
//             if (type == (int)EAttrType.Thorn) return true;
//             return false;
//         }
//     }
// }
// 
