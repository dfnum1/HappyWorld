/********************************************************************
类    名:   CsvData_Text
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
    public partial class CsvData_Text : Data_Base
    {
		public partial class TextData : BaseData
		{
			public	uint				id;
			public	string				textCN;
			public	string				textEN;

			//mapping data
		}
		Dictionary<uint, TextData> m_vData = new Dictionary<uint, TextData>();
		//-------------------------------------------
		public Dictionary<uint, TextData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Text()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public TextData GetData(uint id)
		{
			TextData outData;
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
				
				TextData data = new TextData();
				
				data.id = csv[i]["id"].Uint();
				data.textCN = csv[i]["textCN"].String();
				data.textEN = csv[i]["textEN"].String();
				
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
