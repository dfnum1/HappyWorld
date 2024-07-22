/********************************************************************
类    名:   CsvData_Summon
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
    public partial class CsvData_Summon : Data_Base
    {
		public partial class SummonData : BaseData
		{
			public	uint				id;
			public	uint				groupId;
			public	ushort				level;
			public	uint				nModelID;
			public	string				strName;
			public	byte				hurtDisplayType;
			public	uint				battleIcon;
			public	uint				battleName;
			public	uint				battleDesc;
			public	byte				element;
			public	byte				damageType;
			public	int				atkSpCoefficient;
			public	int				defSpCoefficient;
			public	uint				deadSpValue;
			public	uint				duration;
			public	byte[]				slot;
			public	uint[]				skills;
			public	string				actionGraph;
			public	uint[]				baseAttributeTypes;
			public	uint[]				baseTypes;
			public	float[]				baseValues;
			public	float[]				growValues;
			public	uint[]				inheritAttrTypes;
			public	float[]				inheritValues;
			public	uint				baseSp;

			//mapping data
			public	CsvData_Models.ModelsData Models_nModelID_data;
			public	CsvData_Text.TextData Text_battleIcon_data;
			public	CsvData_Text.TextData Text_battleName_data;
			public	CsvData_Text.TextData Text_battleDesc_data;
		}
		Dictionary<uint, SummonData> m_vData = new Dictionary<uint, SummonData>();
		//-------------------------------------------
		public Dictionary<uint, SummonData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Summon()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public SummonData GetData(uint id)
		{
			SummonData outData;
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
				
				SummonData data = new SummonData();
				
				data.id = csv[i]["id"].Uint();
				data.groupId = csv[i]["groupId"].Uint();
				data.level = csv[i]["level"].Ushort();
				data.nModelID = csv[i]["nModelID"].Uint();
				data.strName = csv[i]["strName"].String();
				data.hurtDisplayType = csv[i]["hurtDisplayType"].Byte();
				data.battleIcon = csv[i]["battleIcon"].Uint();
				data.battleName = csv[i]["battleName"].Uint();
				data.battleDesc = csv[i]["battleDesc"].Uint();
				data.element = csv[i]["element"].Byte();
				data.damageType = csv[i]["damageType"].Byte();
				data.atkSpCoefficient = csv[i]["atkSpCoefficient"].Int();
				data.defSpCoefficient = csv[i]["defSpCoefficient"].Int();
				data.deadSpValue = csv[i]["deadSpValue"].Uint();
				data.duration = csv[i]["duration"].Uint();
				data.slot = csv[i]["slot"].ByteArray();
				data.skills = csv[i]["skills"].UintArray();
				data.actionGraph = csv[i]["actionGraph"].String();
				data.baseAttributeTypes = csv[i]["baseAttributeTypes"].UintArray();
				data.baseTypes = csv[i]["baseTypes"].UintArray();
				data.baseValues = csv[i]["baseValues"].FloatArray();
				data.growValues = csv[i]["growValues"].FloatArray();
				data.inheritAttrTypes = csv[i]["inheritAttrTypes"].UintArray();
				data.inheritValues = csv[i]["inheritValues"].FloatArray();
				data.baseSp = csv[i]["baseSp"].Uint();
				
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
