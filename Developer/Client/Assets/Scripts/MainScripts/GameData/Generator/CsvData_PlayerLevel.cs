/********************************************************************
类    名:   CsvData_PlayerLevel
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
    public partial class CsvData_PlayerLevel : Data_Base
    {
		public partial class PlayerLevelData : BaseData
		{
			public	uint				id;
			public	uint				exp;
			public	uint				diamond;
			public	uint				actionPoint;

			//mapping data
		}
		Dictionary<uint, PlayerLevelData> m_vData = new Dictionary<uint, PlayerLevelData>();
		//-------------------------------------------
		public Dictionary<uint, PlayerLevelData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_PlayerLevel()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public PlayerLevelData GetData(uint id)
		{
			PlayerLevelData outData;
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
				
				PlayerLevelData data = new PlayerLevelData();
				
				data.id = csv[i]["id"].Uint();
				data.exp = csv[i]["exp"].Uint();
				data.diamond = csv[i]["diamond"].Uint();
				data.actionPoint = csv[i]["actionPoint"].Uint();
				
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
