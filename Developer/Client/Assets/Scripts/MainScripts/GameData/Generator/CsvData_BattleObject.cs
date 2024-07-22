/********************************************************************
类    名:   CsvData_BattleObject
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
    public partial class CsvData_BattleObject : Data_Base
    {
		public partial class BattleObjectData : BaseData
		{
			public	uint				id;
			public	string				desc;
			public	uint				modelId;
			public	uint[]				sceneThemes;
			public	uint				nameText;
			public	byte				subType;
			public	EObstacleType				objectType;
			public	byte[]				objectLabel;
			public	EActorCollisionType				boxType;
			public	Vector3				aabb_min;
			public	Vector3				aabb_max;
			public	ushort				boxDirection;
			public	short				hitCount;
			public	short				elementHitCount;
			public	string[]				collisionActions;
			public	string[]				actionSequences;
			public	uint				intervalTime;
			public	float				activateDistance;
			public	uint				goldUp;
			public	uint				pointUp;
			public	uint				foodUp;
			public	uint[]				dropReward;
			public	uint				buffId;
			public	uint				interval;
			public	uint				skillDamageID;
			public	uint[]				eventID;
			public	uint				level;
			public	uint				element;
			public	bool				targetPlayer;
			public	float				baseHp;
			public	float				growHp;
			public	float				sacle;
			public	byte				bindBite;
			public	string				ownEffect;
			public	float				scaleOwnEffect;
			public	string				effects;
			public	float				scaleEffect;
			public	string				bindSlot;
			public	uint				disappearTime;
			public	uint				activateAudioId;
			public	uint				loopAudioId;
			public	uint				loopAudioTime;
			public	string				hitAudio;
			public	string				actionGraph;
			public	byte				isReplace;

			//mapping data
			public	CsvData_Models.ModelsData Models_modelId_data;
			public	CsvData_Text.TextData Text_nameText_data;
			public	CsvData_DropReward.DropRewardData[] DropReward_dropReward_data;
			public	CsvData_Buff.BuffData Buff_buffId_data;
			public	CsvData_SkillDamage.SkillDamageData SkillDamage_skillDamageID_data;
		}
		Dictionary<uint, BattleObjectData> m_vData = new Dictionary<uint, BattleObjectData>();
		//-------------------------------------------
		public Dictionary<uint, BattleObjectData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_BattleObject()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public BattleObjectData GetData(uint id)
		{
			BattleObjectData outData;
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
				
				BattleObjectData data = new BattleObjectData();
				
				data.id = csv[i]["id"].Uint();
				data.desc = csv[i]["desc"].String();
				data.modelId = csv[i]["modelId"].Uint();
				data.sceneThemes = csv[i]["sceneThemes"].UintArray();
				data.nameText = csv[i]["nameText"].Uint();
				data.subType = csv[i]["subType"].Byte();
				data.objectType = (EObstacleType)csv[i]["objectType"].Int();
				data.objectLabel = csv[i]["objectLabel"].ByteArray();
				data.boxType = (EActorCollisionType)csv[i]["boxType"].Int();
				data.aabb_min = csv[i]["aabb_min"].Vec3();
				data.aabb_max = csv[i]["aabb_max"].Vec3();
				data.boxDirection = csv[i]["boxDirection"].Ushort();
				data.hitCount = csv[i]["hitCount"].Short();
				data.elementHitCount = csv[i]["elementHitCount"].Short();
				data.collisionActions = csv[i]["collisionActions"].StringArray();
				data.actionSequences = csv[i]["actionSequences"].StringArray();
				data.intervalTime = csv[i]["intervalTime"].Uint();
				data.activateDistance = csv[i]["activateDistance"].Float();
				data.goldUp = csv[i]["goldUp"].Uint();
				data.pointUp = csv[i]["pointUp"].Uint();
				data.foodUp = csv[i]["foodUp"].Uint();
				data.dropReward = csv[i]["dropReward"].UintArray();
				data.buffId = csv[i]["buffId"].Uint();
				data.interval = csv[i]["interval"].Uint();
				data.skillDamageID = csv[i]["skillDamageID"].Uint();
				data.eventID = csv[i]["eventID"].UintArray();
				data.level = csv[i]["level"].Uint();
				data.element = csv[i]["element"].Uint();
				data.targetPlayer = csv[i]["targetPlayer"].Bool();
				data.baseHp = csv[i]["baseHp"].Float();
				data.growHp = csv[i]["growHp"].Float();
				data.sacle = csv[i]["sacle"].Float();
				data.bindBite = csv[i]["bindBite"].Byte();
				data.ownEffect = csv[i]["ownEffect"].String();
				data.scaleOwnEffect = csv[i]["scaleOwnEffect"].Float();
				data.effects = csv[i]["effects"].String();
				data.scaleEffect = csv[i]["scaleEffect"].Float();
				data.bindSlot = csv[i]["bindSlot"].String();
				data.disappearTime = csv[i]["disappearTime"].Uint();
				data.activateAudioId = csv[i]["activateAudioId"].Uint();
				data.loopAudioId = csv[i]["loopAudioId"].Uint();
				data.loopAudioTime = csv[i]["loopAudioTime"].Uint();
				data.hitAudio = csv[i]["hitAudio"].String();
				data.actionGraph = csv[i]["actionGraph"].String();
				data.isReplace = csv[i]["isReplace"].Byte();
				
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
