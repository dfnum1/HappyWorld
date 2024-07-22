/********************************************************************
类    名:   CsvData_Buff
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
    public partial class CsvData_Buff : Data_Base
    {
		public partial class BuffData : BaseData
		{
			public	uint				id;
			public	uint				groupID;
			public	uint				typeGroupID;
			public	int				priority;
			public	uint				qualityFrame;
			public	string				name;
			public	EBuffType				type;
			public	EBuffGroupType				targetType;
			public	uint				effectType;
			public	float				probability;
			public	string				icon;
			public	bool				isShow;
			public	uint				positionWeight;
			public	uint				time;
			public	uint				interval;
			public	bool				instant;
			public	uint[]				starEvent;
			public	uint[]				intervalEvent;
			public	uint[]				endEvent;
			public	byte				layers;
			public	int				limitCount;
			public	uint[]				layersMaxEvent;
			public	string				parentSlot;
			public	byte				bindBite;
			public	string				beginEffect;
			public	float				scaleEffect;
			public	float				scaleEffectBoss;
			public	string				effectMaterial;
			public	string				stepEffect;
			public	string				endEffect;
			public	string				beginSound;
			public	string				stepSound;
			public	string				endSound;
			public	byte				hitFinish;
			public	byte				BeHitFinish;
			public	uint				damage;
			public	uint[]				attrTypes;
			public	uint[]				valueTypes;
			public	int[]				values;
			public	ushort				skillType;
			public	byte				diedKeep;
			public	bool				trigger_fire;
			public	uint				nameId;
			public	uint				desc;
			public	uint				floatNameId;
			public	byte				s_superposition;
			public	byte				d_superposition;
			public	uint[]				callBuff;
			public	byte[]				triggerLimit;
			public	int[]				triggerParams;
			public	uint				dispelLevel;
			public	uint				element;
			public	uint				forbiddenSkillType;
			public	uint				level;
			public	uint[]				endEventByTrigger;
			public	uint				buffEffectApplayFlag;
			public	int				buffEffectApplayFinish;

			//mapping data
			public	CsvData_Text.TextData Text_qualityFrame_data;
			public	CsvData_SkillDamage.SkillDamageData SkillDamage_damage_data;
			public	CsvData_Text.TextData Text_nameId_data;
			public	CsvData_Text.TextData Text_desc_data;
			public	CsvData_Text.TextData Text_floatNameId_data;
		}
		Dictionary<uint, BuffData> m_vData = new Dictionary<uint, BuffData>();
		//-------------------------------------------
		public Dictionary<uint, BuffData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Buff()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public BuffData GetData(uint id)
		{
			BuffData outData;
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
				
				BuffData data = new BuffData();
				
				data.id = csv[i]["id"].Uint();
				data.groupID = csv[i]["groupID"].Uint();
				data.typeGroupID = csv[i]["typeGroupID"].Uint();
				data.priority = csv[i]["priority"].Int();
				data.qualityFrame = csv[i]["qualityFrame"].Uint();
				data.name = csv[i]["name"].String();
				data.type = (EBuffType)csv[i]["type"].Int();
				data.targetType = (EBuffGroupType)csv[i]["targetType"].Int();
				data.effectType = csv[i]["effectType"].Uint();
				data.probability = csv[i]["probability"].Float();
				data.icon = csv[i]["icon"].String();
				data.isShow = csv[i]["isShow"].Bool();
				data.positionWeight = csv[i]["positionWeight"].Uint();
				data.time = csv[i]["time"].Uint();
				data.interval = csv[i]["interval"].Uint();
				data.instant = csv[i]["instant"].Bool();
				data.starEvent = csv[i]["starEvent"].UintArray();
				data.intervalEvent = csv[i]["intervalEvent"].UintArray();
				data.endEvent = csv[i]["endEvent"].UintArray();
				data.layers = csv[i]["layers"].Byte();
				data.limitCount = csv[i]["limitCount"].Int();
				data.layersMaxEvent = csv[i]["layersMaxEvent"].UintArray();
				data.parentSlot = csv[i]["parentSlot"].String();
				data.bindBite = csv[i]["bindBite"].Byte();
				data.beginEffect = csv[i]["beginEffect"].String();
				data.scaleEffect = csv[i]["scaleEffect"].Float();
				data.scaleEffectBoss = csv[i]["scaleEffectBoss"].Float();
				data.effectMaterial = csv[i]["effectMaterial"].String();
				data.stepEffect = csv[i]["stepEffect"].String();
				data.endEffect = csv[i]["endEffect"].String();
				data.beginSound = csv[i]["beginSound"].String();
				data.stepSound = csv[i]["stepSound"].String();
				data.endSound = csv[i]["endSound"].String();
				data.hitFinish = csv[i]["hitFinish"].Byte();
				data.BeHitFinish = csv[i]["BeHitFinish"].Byte();
				data.damage = csv[i]["damage"].Uint();
				data.attrTypes = csv[i]["attrTypes"].UintArray();
				data.valueTypes = csv[i]["valueTypes"].UintArray();
				data.values = csv[i]["values"].IntArray();
				data.skillType = csv[i]["skillType"].Ushort();
				data.diedKeep = csv[i]["diedKeep"].Byte();
				data.trigger_fire = csv[i]["trigger_fire"].Bool();
				data.nameId = csv[i]["nameId"].Uint();
				data.desc = csv[i]["desc"].Uint();
				data.floatNameId = csv[i]["floatNameId"].Uint();
				data.s_superposition = csv[i]["s_superposition"].Byte();
				data.d_superposition = csv[i]["d_superposition"].Byte();
				data.callBuff = csv[i]["callBuff"].UintArray();
				data.triggerLimit = csv[i]["triggerLimit"].ByteArray();
				data.triggerParams = csv[i]["triggerParams"].IntArray();
				data.dispelLevel = csv[i]["dispelLevel"].Uint();
				data.element = csv[i]["element"].Uint();
				data.forbiddenSkillType = csv[i]["forbiddenSkillType"].Uint();
				data.level = csv[i]["level"].Uint();
				data.endEventByTrigger = csv[i]["endEventByTrigger"].UintArray();
				data.buffEffectApplayFlag = csv[i]["buffEffectApplayFlag"].Uint();
				data.buffEffectApplayFinish = csv[i]["buffEffectApplayFinish"].Int();
				
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
