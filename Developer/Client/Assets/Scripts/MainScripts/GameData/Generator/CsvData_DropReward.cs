/********************************************************************
类    名:   CsvData_DropReward
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
    public partial class CsvData_DropReward : Data_Base
    {
		public partial class DropRewardData : BaseData
		{
			public	uint				ID;
			public	uint				gourpId;
			public	uint				chapterId;
			public	uint[]				rewardId;
			public	uint				weight;
			public	float				fPickDistance;
			public	uint[]				showCount;
			public	string				dropSound;
			public	string				pickSound;
			public	string				effectModel;
			public	string				pickEffect;
			public	int				pickType;
			public	uint				goldDropEffect;

			//mapping data
			public	CsvData_GoldDropEffect.GoldDropEffectData GoldDropEffect_goldDropEffect_data;
		}
		Dictionary<uint, DropRewardData> m_vData = new Dictionary<uint, DropRewardData>();
		//-------------------------------------------
		public Dictionary<uint, DropRewardData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_DropReward()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public DropRewardData GetData(uint id)
		{
			DropRewardData outData;
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
				
				DropRewardData data = new DropRewardData();
				
				data.ID = csv[i]["ID"].Uint();
				data.gourpId = csv[i]["gourpId"].Uint();
				data.chapterId = csv[i]["chapterId"].Uint();
				data.rewardId = csv[i]["rewardId"].UintArray();
				data.weight = csv[i]["weight"].Uint();
				data.fPickDistance = csv[i]["fPickDistance"].Float();
				data.showCount = csv[i]["showCount"].UintArray();
				data.dropSound = csv[i]["dropSound"].String();
				data.pickSound = csv[i]["pickSound"].String();
				data.effectModel = csv[i]["effectModel"].String();
				data.pickEffect = csv[i]["pickEffect"].String();
				data.pickType = csv[i]["pickType"].Int();
				data.goldDropEffect = csv[i]["goldDropEffect"].Uint();
				
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
