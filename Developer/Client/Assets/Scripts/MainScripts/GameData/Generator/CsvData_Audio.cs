/********************************************************************
类    名:   CsvData_Audio
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
    public partial class CsvData_Audio : Data_Base
    {
		public partial class AudioData : BaseData
		{
			public	uint				id;
			public	byte				group;
			public	byte				type;
			public	string				location;
			public	string				channel;
			public	Vector2				volume;
			public	bool				b3D;

			//mapping data
		}
		Dictionary<uint, List<AudioData>> m_vData = new Dictionary<uint, List<AudioData>>();
		//-------------------------------------------
		public Dictionary<uint, List<AudioData>> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Audio()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public List<AudioData> GetData(uint id)
		{
			List<AudioData> outData;
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
				
				AudioData data = new AudioData();
				
				data.id = csv[i]["id"].Uint();
				data.group = csv[i]["group"].Byte();
				data.type = csv[i]["type"].Byte();
				data.location = csv[i]["location"].String();
				data.channel = csv[i]["channel"].String();
				data.volume = csv[i]["volume"].Vec2();
				data.b3D = csv[i]["b3D"].Bool();
				
				List<AudioData> vList;
				if(!m_vData.TryGetValue(data.id, out vList))
				{
					vList = new List<AudioData>();
					m_vData.Add(data.id, vList);
				}
				vList.Add(data);
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
