/********************************************************************
类    名:   CsvData_Skill
作    者:	自动生成
描    述:	
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.Data;
using Framework.Core;
using Framework.Base;
namespace TopGame.Data
{
    public partial class CsvData_Skill : Data_Base
    {
		public partial class SkillData : BaseData
		{
			public	uint				id;
			public	uint				level;
			public	uint				unLock;
			public	ESkillType				skillType;
			public	uint				tag;
			public	uint				abnormalLimit;
			public	byte[]				triggerType;
			public	bool				canFaceToTarget;
			public	int[]				triggerParams;
			public	byte				triggerTimes;
			public	bool				triggerClear;
			public	byte[]				triggerProbability;
			public	uint				distance;
			public	float				minAttack;
			public	float				maxAttack;
			public	short[]				lockRode;
			public	bool				canLockHitFlyTarget;
			public	uint				lockFilterFlags;
			public	bool				canLockBehind;
			public	ELockHitCondition[]				targetCondition;
			public	int[]				targetParameter1;
			public	int[]				targetParameter2;
			public	int[]				targetParameter3;
			public	ELockHitType				targetType;
			public	byte				targetNumbers;
			public	byte[]				costType;
			public	uint[]				costValue;
			public	byte[]				gainSpType;
			public	int[]				GainSpValue;
			public	uint				coolDown;
			public	uint				initialCoolDown;
			public	uint				globalCoolDown;
			public	uint				name;
			public	uint				upgradeDescID;
			public	uint				skillDamageID;
			public	string				qualityFrame;
			public	bool				lockHeight;
			public	float				minLockHeight;
			public	float				maxLockHeight;
			public	uint[]				beginEventId;
			public	uint[]				attrBuffs;
			public	string				icon;
			public	bool				isFloating;
			public	uint				skillFloating;
			public	uint				interruptTime;
			public	bool				targetReset;
			public	uint				skillTriggerCoolDown;
			public	uint[]				upgradeCostType;
			public	uint[]				upgradeCostValue;
			public	uint				fightPower;

			//mapping data
			public	CsvData_Text.TextData Text_name_data;
			public	CsvData_Text.TextData Text_upgradeDescID_data;
			public	CsvData_SkillDamage.SkillDamageData SkillDamage_skillDamageID_data;
			public	CsvData_Text.TextData Text_skillFloating_data;
		}
		Dictionary<uint, List<SkillData>> m_vData = new Dictionary<uint, List<SkillData>>();
		//-------------------------------------------
		public Dictionary<uint, List<SkillData>> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Skill()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public List<SkillData> GetData(uint id)
		{
			List<SkillData> outData;
			if(m_vData.TryGetValue(id, out outData))
				return outData;
			return null;
		}
        //-------------------------------------------
        public override bool LoadData(string strContext,CsvParser csv = null)
        {
			if(csv == null) csv = new CsvParser();
			if(!csv.LoadTableString(strContext))
				return false;
			
			ClearData();
			
			int i = csv.GetTitleLine();
			if(i < 0) return false;
			
			int nLineCnt = csv.GetLineCount();
			for(i++; i < nLineCnt; i++)
			{
				if(!csv[i]["id"].IsValid()) continue;
				
				SkillData data = new SkillData();
				
				data.id = csv[i]["id"].Uint();
				data.level = csv[i]["level"].Uint();
				data.unLock = csv[i]["unLock"].Uint();
				data.skillType = (ESkillType)csv[i]["skillType"].Int();
				data.tag = csv[i]["tag"].Uint();
				data.abnormalLimit = csv[i]["abnormalLimit"].Uint();
				data.triggerType = csv[i]["triggerType"].ByteArray();
				data.canFaceToTarget = csv[i]["canFaceToTarget"].Bool();
				data.triggerParams = csv[i]["triggerParams"].IntArray();
				data.triggerTimes = csv[i]["triggerTimes"].Byte();
				data.triggerClear = csv[i]["triggerClear"].Bool();
				data.triggerProbability = csv[i]["triggerProbability"].ByteArray();
				data.distance = csv[i]["distance"].Uint();
				data.minAttack = csv[i]["minAttack"].Float();
				data.maxAttack = csv[i]["maxAttack"].Float();
				data.lockRode = csv[i]["lockRode"].ShortArray();
				data.canLockHitFlyTarget = csv[i]["canLockHitFlyTarget"].Bool();
				data.lockFilterFlags = csv[i]["lockFilterFlags"].Uint();
				data.canLockBehind = csv[i]["canLockBehind"].Bool();
				{
					int[] temps = csv[i]["targetCondition"].IntArray();
					if(temps !=null)
					{
						data.targetCondition= new ELockHitCondition[temps.Length];
						for(int e = 0; e < temps.Length; ++e)
							data.targetCondition[e] = (ELockHitCondition)temps[e];
					}
				}
				data.targetParameter1 = csv[i]["targetParameter1"].IntArray();
				data.targetParameter2 = csv[i]["targetParameter2"].IntArray();
				data.targetParameter3 = csv[i]["targetParameter3"].IntArray();
				data.targetType = (ELockHitType)csv[i]["targetType"].Int();
				data.targetNumbers = csv[i]["targetNumbers"].Byte();
				data.costType = csv[i]["costType"].ByteArray();
				data.costValue = csv[i]["costValue"].UintArray();
				data.gainSpType = csv[i]["gainSpType"].ByteArray();
				data.GainSpValue = csv[i]["GainSpValue"].IntArray();
				data.coolDown = csv[i]["coolDown"].Uint();
				data.initialCoolDown = csv[i]["initialCoolDown"].Uint();
				data.globalCoolDown = csv[i]["globalCoolDown"].Uint();
				data.name = csv[i]["name"].Uint();
				data.upgradeDescID = csv[i]["upgradeDescID"].Uint();
				data.skillDamageID = csv[i]["skillDamageID"].Uint();
				data.qualityFrame = csv[i]["qualityFrame"].String();
				data.lockHeight = csv[i]["lockHeight"].Bool();
				data.minLockHeight = csv[i]["minLockHeight"].Float();
				data.maxLockHeight = csv[i]["maxLockHeight"].Float();
				data.beginEventId = csv[i]["beginEventId"].UintArray();
				data.attrBuffs = csv[i]["attrBuffs"].UintArray();
				data.icon = csv[i]["icon"].String();
				data.isFloating = csv[i]["isFloating"].Bool();
				data.skillFloating = csv[i]["skillFloating"].Uint();
				data.interruptTime = csv[i]["interruptTime"].Uint();
				data.targetReset = csv[i]["targetReset"].Bool();
				data.skillTriggerCoolDown = csv[i]["skillTriggerCoolDown"].Uint();
				data.upgradeCostType = csv[i]["upgradeCostType"].UintArray();
				data.upgradeCostValue = csv[i]["upgradeCostValue"].UintArray();
				data.fightPower = csv[i]["fightPower"].Uint();
				
				List<SkillData> vList;
				if(!m_vData.TryGetValue(data.id, out vList))
				{
					vList = new List<SkillData>();
					m_vData.Add(data.id, vList);
				}
				vList.Add(data);
				OnAddData(data);
			}
			OnLoadCompleted();
            return true;
        }
        //-------------------------------------------
        public override void ClearData()
        {
			m_vData.Clear();
			base.ClearData();
        }
    }
}
