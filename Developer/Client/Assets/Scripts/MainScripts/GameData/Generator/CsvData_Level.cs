/********************************************************************
类    名:   CsvData_Level
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
    public partial class CsvData_Level : Data_Base
    {
		public partial class LevelData : BaseData
		{
			public	uint				level;
			public	uint				exp;
			public	uint				gold;
			public	uint				powder;
			public	uint				levelMax;
			public	uint				crystalNeedLv;
			public	float				monsterGrowAbilityRate;
			public	float				summonGrowAbilityRate;
			public	float				growAbilityRate;

			//mapping data
		}
		Dictionary<uint, LevelData> m_vData = new Dictionary<uint, LevelData>();
		//-------------------------------------------
		public Dictionary<uint, LevelData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Level()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public LevelData GetData(uint id)
		{
			LevelData outData;
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
				if(!csv[i]["level"].IsValid()) continue;
				
				LevelData data = new LevelData();
				
				data.level = csv[i]["level"].Uint();
				data.exp = csv[i]["exp"].Uint();
				data.gold = csv[i]["gold"].Uint();
				data.powder = csv[i]["powder"].Uint();
				data.levelMax = csv[i]["levelMax"].Uint();
				data.crystalNeedLv = csv[i]["crystalNeedLv"].Uint();
				data.monsterGrowAbilityRate = csv[i]["monsterGrowAbilityRate"].Float();
				data.summonGrowAbilityRate = csv[i]["summonGrowAbilityRate"].Float();
				data.growAbilityRate = csv[i]["growAbilityRate"].Float();
				
				m_vData.Add(data.level, data);
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
