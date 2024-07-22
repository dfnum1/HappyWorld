/********************************************************************
类    名:   CsvData_Player
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
    public partial class CsvData_Player : Data_Base
    {
		public partial class PlayerData : BaseData
		{
			public	uint				ID;
			public	uint				quality;
			public	uint				nextId;
			public	uint				nModelID;
			public	uint				hModelID;
			public	Vector3				modelPosition;
			public	Vector3				modelRotation;
			public	string				strName;
			public	uint				nameText;
			public	uint				descText;
			public	uint				spDescText;
			public	uint				icon;
			public	uint				playerIcon;
			public	string				cardIcon;
			public	uint[]				skills;
			public	uint[]				pvpSkills;
			public	uint[]				equips;
			public	uint				ballisticType;
			public	uint				storyText;
			public	string				actionGraph;
			public	string				showActionGraph;
			public	uint				element;
			public	byte				camp;
			public	byte				damageType;
			public	byte				genre;
			public	uint				duty;
			public	uint				equipType;
			public	uint				heroStar;
			public	int				atkSpCoefficient;
			public	int				defSpCoefficient;
			public	uint				deadSpValue;
			public	uint				heroNoumenon;
			public	bool				isUnlock;
			public	uint				nameResource;
			public	uint[]				attributeTypes;
			public	uint[]				valueTypes;
			public	float[]				baseValues;
			public	float[]				growValues;
			public	uint[]				starEffectTypes;
			public	uint[]				starValueTypes;
			public	int[]				starValues;
			public	uint				totalScoreAddition;
			public	uint[]				resurrectionBuffs;

			//mapping data
			public	CsvData_Text.TextData Text_quality_data;
			public	CsvData_Models.ModelsData Models_nModelID_data;
			public	CsvData_Models.ModelsData Models_hModelID_data;
			public	CsvData_Text.TextData Text_nameText_data;
			public	CsvData_Text.TextData Text_descText_data;
			public	CsvData_Text.TextData Text_spDescText_data;
			public	CsvData_Text.TextData Text_icon_data;
			public	CsvData_Text.TextData Text_playerIcon_data;
			public	CsvData_Text.TextData Text_storyText_data;
			public	CsvData_Text.TextData Text_nameResource_data;
			public	CsvData_Buff.BuffData[] Buff_resurrectionBuffs_data;
		}
		Dictionary<uint, PlayerData> m_vData = new Dictionary<uint, PlayerData>();
		//-------------------------------------------
		public Dictionary<uint, PlayerData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Player()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public PlayerData GetData(uint id)
		{
			PlayerData outData;
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
				if(!csv[i]["ID"].IsValid()) continue;
				
				PlayerData data = new PlayerData();
				
				data.ID = csv[i]["ID"].Uint();
				data.quality = csv[i]["quality"].Uint();
				data.nextId = csv[i]["nextId"].Uint();
				data.nModelID = csv[i]["nModelID"].Uint();
				data.hModelID = csv[i]["hModelID"].Uint();
				data.modelPosition = csv[i]["modelPosition"].Vec3();
				data.modelRotation = csv[i]["modelRotation"].Vec3();
				data.strName = csv[i]["strName"].String();
				data.nameText = csv[i]["nameText"].Uint();
				data.descText = csv[i]["descText"].Uint();
				data.spDescText = csv[i]["spDescText"].Uint();
				data.icon = csv[i]["icon"].Uint();
				data.playerIcon = csv[i]["playerIcon"].Uint();
				data.cardIcon = csv[i]["cardIcon"].String();
				data.skills = csv[i]["skills"].UintArray();
				data.pvpSkills = csv[i]["pvpSkills"].UintArray();
				data.equips = csv[i]["equips"].UintArray();
				data.ballisticType = csv[i]["ballisticType"].Uint();
				data.storyText = csv[i]["storyText"].Uint();
				data.actionGraph = csv[i]["actionGraph"].String();
				data.showActionGraph = csv[i]["showActionGraph"].String();
				data.element = csv[i]["element"].Uint();
				data.camp = csv[i]["camp"].Byte();
				data.damageType = csv[i]["damageType"].Byte();
				data.genre = csv[i]["genre"].Byte();
				data.duty = csv[i]["duty"].Uint();
				data.equipType = csv[i]["equipType"].Uint();
				data.heroStar = csv[i]["heroStar"].Uint();
				data.atkSpCoefficient = csv[i]["atkSpCoefficient"].Int();
				data.defSpCoefficient = csv[i]["defSpCoefficient"].Int();
				data.deadSpValue = csv[i]["deadSpValue"].Uint();
				data.heroNoumenon = csv[i]["heroNoumenon"].Uint();
				data.isUnlock = csv[i]["isUnlock"].Bool();
				data.nameResource = csv[i]["nameResource"].Uint();
				data.attributeTypes = csv[i]["attributeTypes"].UintArray();
				data.valueTypes = csv[i]["valueTypes"].UintArray();
				data.baseValues = csv[i]["baseValues"].FloatArray();
				data.growValues = csv[i]["growValues"].FloatArray();
				data.starEffectTypes = csv[i]["starEffectTypes"].UintArray();
				data.starValueTypes = csv[i]["starValueTypes"].UintArray();
				data.starValues = csv[i]["starValues"].IntArray();
				data.totalScoreAddition = csv[i]["totalScoreAddition"].Uint();
				data.resurrectionBuffs = csv[i]["resurrectionBuffs"].UintArray();
				
				m_vData.Add(data.ID, data);
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
