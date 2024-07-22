/********************************************************************
类    名:   CsvData_PayList
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
    public partial class CsvData_PayList : Data_Base
    {
		public partial class PayListData : BaseData
		{
			public	uint				id;
			public	string				channel;
			public	string				type;
			public	uint				money;
			public	uint				diamond;
			public	uint[]				rewardId;
			public	uint[]				num;
			public	uint				name;
			public	uint				desc;
			public	uint				payid;
			public	uint				icon;
			public	string				goodsId;

			//mapping data
			public	CsvData_Text.TextData Text_name_data;
			public	CsvData_Text.TextData Text_desc_data;
			public	CsvData_Text.TextData Text_icon_data;
		}
		Dictionary<uint, PayListData> m_vData = new Dictionary<uint, PayListData>();
		//-------------------------------------------
		public Dictionary<uint, PayListData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_PayList()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public PayListData GetData(uint id)
		{
			PayListData outData;
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
				
				PayListData data = new PayListData();
				
				data.id = csv[i]["id"].Uint();
				data.channel = csv[i]["channel"].String();
				data.type = csv[i]["type"].String();
				data.money = csv[i]["money"].Uint();
				data.diamond = csv[i]["diamond"].Uint();
				data.rewardId = csv[i]["rewardId"].UintArray();
				data.num = csv[i]["num"].UintArray();
				data.name = csv[i]["name"].Uint();
				data.desc = csv[i]["desc"].Uint();
				data.payid = csv[i]["payid"].Uint();
				data.icon = csv[i]["icon"].Uint();
				data.goodsId = csv[i]["goodsId"].String();
				
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
