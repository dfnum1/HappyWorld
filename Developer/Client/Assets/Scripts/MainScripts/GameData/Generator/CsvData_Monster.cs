/********************************************************************
类    名:   CsvData_Monster
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
    public partial class CsvData_Monster : Data_Base
    {
		public partial class MonsterData : BaseData
		{
			public	uint				id;
			public	string				desc;
			public	uint				quality;
			public	uint				heroStar;
			public	uint				nameText;
			public	uint				descText;
			public	string				icon;
			public	uint				frame;
			public	uint				modelId;
			public	float				object_scale;
			public	byte				subType;
			public	EMonsterType				monsterType;
			public	uint[]				sceneThemes;
			public	uint[]				skills;
			public	int				atkSpCoefficient;
			public	int				defSpCoefficient;
			public	uint				deadSpValue;
			public	uint				deadPoint;
			public	string				actionGraph;
			public	uint				postureRageNum;
			public	uint[]				postureRageBuffId;
			public	uint				bossFrenzyTime;
			public	uint[]				attributeTypes;
			public	uint[]				valueTypes;
			public	float[]				baseValues;
			public	float[]				growValues;
			public	uint				element;
			public	byte				collisionFilter;
			public	byte				hitCount;
			public	uint				buffId;
			public	uint				damageId;
			public	uint				ballisticType;
			public	uint[]				hitDropReward;
			public	uint[]				dropReward;
			public	byte				ownBindBite;
			public	string				ownParentSlot;
			public	string				ownEffect;
			public	float				scaleOwnEffect;
			public	uint				deadAudioId;
			public	ushort				genreWeakness;
			public	float				basePosture;
			public	float				growPosture;
			public	float				postureReduction;
			public	float				postureTime;
			public	uint[]				postureTimeBuffId;
			public	uint				postureRecoverTime;
			public	float				moveSpeed;
			public	byte				camp;
			public	byte				damageType;

			//mapping data
			public	CsvData_Text.TextData Text_nameText_data;
			public	CsvData_Text.TextData Text_descText_data;
			public	CsvData_Models.ModelsData Models_modelId_data;
			public	CsvData_Buff.BuffData Buff_buffId_data;
			public	CsvData_SkillDamage.SkillDamageData SkillDamage_damageId_data;
			public	CsvData_DropReward.DropRewardData[] DropReward_hitDropReward_data;
			public	CsvData_DropReward.DropRewardData[] DropReward_dropReward_data;
		}
		Dictionary<uint, MonsterData> m_vData = new Dictionary<uint, MonsterData>();
		//-------------------------------------------
		public Dictionary<uint, MonsterData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Monster()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public MonsterData GetData(uint id)
		{
			MonsterData outData;
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
				
				MonsterData data = new MonsterData();
				
				data.id = csv[i]["id"].Uint();
				data.desc = csv[i]["desc"].String();
				data.quality = csv[i]["quality"].Uint();
				data.heroStar = csv[i]["heroStar"].Uint();
				data.nameText = csv[i]["nameText"].Uint();
				data.descText = csv[i]["descText"].Uint();
				data.icon = csv[i]["icon"].String();
				data.frame = csv[i]["frame"].Uint();
				data.modelId = csv[i]["modelId"].Uint();
				data.object_scale = csv[i]["object_scale"].Float();
				data.subType = csv[i]["subType"].Byte();
				data.monsterType = (EMonsterType)csv[i]["monsterType"].Int();
				data.sceneThemes = csv[i]["sceneThemes"].UintArray();
				data.skills = csv[i]["skills"].UintArray();
				data.atkSpCoefficient = csv[i]["atkSpCoefficient"].Int();
				data.defSpCoefficient = csv[i]["defSpCoefficient"].Int();
				data.deadSpValue = csv[i]["deadSpValue"].Uint();
				data.deadPoint = csv[i]["deadPoint"].Uint();
				data.actionGraph = csv[i]["actionGraph"].String();
				data.postureRageNum = csv[i]["postureRageNum"].Uint();
				data.postureRageBuffId = csv[i]["postureRageBuffId"].UintArray();
				data.bossFrenzyTime = csv[i]["bossFrenzyTime"].Uint();
				data.attributeTypes = csv[i]["attributeTypes"].UintArray();
				data.valueTypes = csv[i]["valueTypes"].UintArray();
				data.baseValues = csv[i]["baseValues"].FloatArray();
				data.growValues = csv[i]["growValues"].FloatArray();
				data.element = csv[i]["element"].Uint();
				data.collisionFilter = csv[i]["collisionFilter"].Byte();
				data.hitCount = csv[i]["hitCount"].Byte();
				data.buffId = csv[i]["buffId"].Uint();
				data.damageId = csv[i]["damageId"].Uint();
				data.ballisticType = csv[i]["ballisticType"].Uint();
				data.hitDropReward = csv[i]["hitDropReward"].UintArray();
				data.dropReward = csv[i]["dropReward"].UintArray();
				data.ownBindBite = csv[i]["ownBindBite"].Byte();
				data.ownParentSlot = csv[i]["ownParentSlot"].String();
				data.ownEffect = csv[i]["ownEffect"].String();
				data.scaleOwnEffect = csv[i]["scaleOwnEffect"].Float();
				data.deadAudioId = csv[i]["deadAudioId"].Uint();
				data.genreWeakness = csv[i]["genreWeakness"].Ushort();
				data.basePosture = csv[i]["basePosture"].Float();
				data.growPosture = csv[i]["growPosture"].Float();
				data.postureReduction = csv[i]["postureReduction"].Float();
				data.postureTime = csv[i]["postureTime"].Float();
				data.postureTimeBuffId = csv[i]["postureTimeBuffId"].UintArray();
				data.postureRecoverTime = csv[i]["postureRecoverTime"].Uint();
				data.moveSpeed = csv[i]["moveSpeed"].Float();
				data.camp = csv[i]["camp"].Byte();
				data.damageType = csv[i]["damageType"].Byte();
				
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
