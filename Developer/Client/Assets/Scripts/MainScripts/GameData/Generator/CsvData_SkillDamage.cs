/********************************************************************
类    名:   CsvData_SkillDamage
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
    public partial class CsvData_SkillDamage : Data_Base
    {
		public partial class SkillDamageData : BaseData
		{
			public	uint				id;
			public	uint				groupID;
			public	EDamageType				damageType;
			public	uint				element;
			public	EDamageValueType[]				ValueTarget;
			public	uint[]				effectTypes;
			public	uint[]				valueTypes;
			public	int[]				values;
			public	int				fixedDamage;
			public	EHitType				secondDamageTypes;
			public	int[]				declineRatio;
			public	uint				shieldDamageCoefficient;
			public	bool				shieldDamageDisregard;
			public	bool				ingoreInvincible;
			public	uint[]				searchTypeGroupID;
			public	int[]				extraDamageChange;
			public	uint[]				extraBuffId;
			public	uint[]				extrabuffProbability;
			public	EDamageValueType[]				ValueTargetMax;
			public	uint[]				effectTypesMax;
			public	uint[]				valueTypesMax;
			public	int[]				valuesMax;
			public	int				fixedDamageMax;
			public	byte[]				ourGainSpType;
			public	int[]				ourGainSpValue;
			public	byte[]				enemyGainSpType;
			public	int[]				enemyGainSpValue;
			public	uint[]				buffID;
			public	uint[]				buffProbability;
			public	uint				eventID;
			public	byte				damageLevel;
			public	bool				damage_fire;
			public	bool				citical_fire;
			public	int				damage_fire_rate;

			//mapping data
			public	CsvData_Buff.BuffData[] Buff_searchTypeGroupID_data;
			public	CsvData_Buff.BuffData[] Buff_extraBuffId_data;
		}
		Dictionary<uint, SkillDamageData> m_vData = new Dictionary<uint, SkillDamageData>();
		//-------------------------------------------
		public Dictionary<uint, SkillDamageData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_SkillDamage()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public SkillDamageData GetData(uint id)
		{
			SkillDamageData outData;
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
				
				SkillDamageData data = new SkillDamageData();
				
				data.id = csv[i]["id"].Uint();
				data.groupID = csv[i]["groupID"].Uint();
				data.damageType = (EDamageType)csv[i]["damageType"].Int();
				data.element = csv[i]["element"].Uint();
				{
					int[] temps = csv[i]["ValueTarget"].IntArray();
					if(temps !=null)
					{
						data.ValueTarget= new EDamageValueType[temps.Length];
						for(int e = 0; e < temps.Length; ++e)
							data.ValueTarget[e] = (EDamageValueType)temps[e];
					}
				}
				data.effectTypes = csv[i]["effectTypes"].UintArray();
				data.valueTypes = csv[i]["valueTypes"].UintArray();
				data.values = csv[i]["values"].IntArray();
				data.fixedDamage = csv[i]["fixedDamage"].Int();
				data.secondDamageTypes = (EHitType)csv[i]["secondDamageTypes"].Int();
				data.declineRatio = csv[i]["declineRatio"].IntArray();
				data.shieldDamageCoefficient = csv[i]["shieldDamageCoefficient"].Uint();
				data.shieldDamageDisregard = csv[i]["shieldDamageDisregard"].Bool();
				data.ingoreInvincible = csv[i]["ingoreInvincible"].Bool();
				data.searchTypeGroupID = csv[i]["searchTypeGroupID"].UintArray();
				data.extraDamageChange = csv[i]["extraDamageChange"].IntArray();
				data.extraBuffId = csv[i]["extraBuffId"].UintArray();
				data.extrabuffProbability = csv[i]["extrabuffProbability"].UintArray();
				{
					int[] temps = csv[i]["ValueTargetMax"].IntArray();
					if(temps !=null)
					{
						data.ValueTargetMax= new EDamageValueType[temps.Length];
						for(int e = 0; e < temps.Length; ++e)
							data.ValueTargetMax[e] = (EDamageValueType)temps[e];
					}
				}
				data.effectTypesMax = csv[i]["effectTypesMax"].UintArray();
				data.valueTypesMax = csv[i]["valueTypesMax"].UintArray();
				data.valuesMax = csv[i]["valuesMax"].IntArray();
				data.fixedDamageMax = csv[i]["fixedDamageMax"].Int();
				data.ourGainSpType = csv[i]["ourGainSpType"].ByteArray();
				data.ourGainSpValue = csv[i]["ourGainSpValue"].IntArray();
				data.enemyGainSpType = csv[i]["enemyGainSpType"].ByteArray();
				data.enemyGainSpValue = csv[i]["enemyGainSpValue"].IntArray();
				data.buffID = csv[i]["buffID"].UintArray();
				data.buffProbability = csv[i]["buffProbability"].UintArray();
				data.eventID = csv[i]["eventID"].Uint();
				data.damageLevel = csv[i]["damageLevel"].Byte();
				data.damage_fire = csv[i]["damage_fire"].Bool();
				data.citical_fire = csv[i]["citical_fire"].Bool();
				data.damage_fire_rate = csv[i]["damage_fire_rate"].Int();
				
				m_vData.Add(data.id, data);
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
