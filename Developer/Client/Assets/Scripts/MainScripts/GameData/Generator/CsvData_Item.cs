/********************************************************************
类    名:   CsvData_Item
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
    public partial class CsvData_Item : Data_Base
    {
		public partial class ItemData : BaseData
		{
			public	uint				ID;
			public	byte				type;
			public	uint				name;
			public	uint				des;
			public	byte				quality;
			public	uint				qualityFrame;
			public	string				qualityColor;
			public	uint				icon;
			public	uint				rank;
			public	bool				isShow;
			public	bool				isStack;
			public	uint				stackNum;
			public	uint				maxNum;
			public	bool				isSale;
			public	uint				price;
			public	uint				jumpUI;
			public	uint				sourceText;
			public	uint				param;
			public	uint				ranReward;

			//mapping data
			public	CsvData_Text.TextData Text_name_data;
			public	CsvData_Text.TextData Text_des_data;
			public	CsvData_Text.TextData Text_qualityFrame_data;
			public	CsvData_Text.TextData Text_icon_data;
			public	CsvData_Text.TextData Text_sourceText_data;
		}
		Dictionary<uint, ItemData> m_vData = new Dictionary<uint, ItemData>();
		//-------------------------------------------
		public Dictionary<uint, ItemData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Item()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public ItemData GetData(uint id)
		{
			ItemData outData;
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
				
				ItemData data = new ItemData();
				
				data.ID = csv[i]["ID"].Uint();
				data.type = csv[i]["type"].Byte();
				data.name = csv[i]["name"].Uint();
				data.des = csv[i]["des"].Uint();
				data.quality = csv[i]["quality"].Byte();
				data.qualityFrame = csv[i]["qualityFrame"].Uint();
				data.qualityColor = csv[i]["qualityColor"].String();
				data.icon = csv[i]["icon"].Uint();
				data.rank = csv[i]["rank"].Uint();
				data.isShow = csv[i]["isShow"].Bool();
				data.isStack = csv[i]["isStack"].Bool();
				data.stackNum = csv[i]["stackNum"].Uint();
				data.maxNum = csv[i]["maxNum"].Uint();
				data.isSale = csv[i]["isSale"].Bool();
				data.price = csv[i]["price"].Uint();
				data.jumpUI = csv[i]["jumpUI"].Uint();
				data.sourceText = csv[i]["sourceText"].Uint();
				data.param = csv[i]["param"].Uint();
				data.ranReward = csv[i]["ranReward"].Uint();
				
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
