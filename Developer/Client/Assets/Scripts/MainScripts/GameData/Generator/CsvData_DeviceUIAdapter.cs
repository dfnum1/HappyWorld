/********************************************************************
类    名:   CsvData_DeviceUIAdapter
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
    public partial class CsvData_DeviceUIAdapter : Data_Base
    {
		public partial class DeviceUIAdapterData : BaseData
		{
			public	string				id;
			public	float				left;
			public	float				top;
			public	float				right;
			public	float				bottom;
			public	float				posZ;

			//mapping data
		}
		Dictionary<string, DeviceUIAdapterData> m_vData = new Dictionary<string, DeviceUIAdapterData>();
		//-------------------------------------------
		public Dictionary<string, DeviceUIAdapterData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_DeviceUIAdapter()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public DeviceUIAdapterData GetData(string id)
		{
			DeviceUIAdapterData outData;
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
				
				DeviceUIAdapterData data = new DeviceUIAdapterData();
				
				data.id = csv[i]["id"].String();
				data.left = csv[i]["left"].Float();
				data.top = csv[i]["top"].Float();
				data.right = csv[i]["right"].Float();
				data.bottom = csv[i]["bottom"].Float();
				data.posZ = csv[i]["posZ"].Float();
				
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
