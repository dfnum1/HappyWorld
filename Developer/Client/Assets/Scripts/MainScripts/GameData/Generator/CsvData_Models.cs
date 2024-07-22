/********************************************************************
类    名:   CsvData_Models
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
    public partial class CsvData_Models : Data_Base
    {
		public partial class ModelsData : BaseData
		{
			public	uint				ID;
			public	string				strName;
			public	float				fHeight;
			public	string				strFile;
			public	Vector3				modelPosition;
			public	Vector3				modelRotation;
			public	Vector3				modelScale;

			//mapping data
		}
		Dictionary<uint, ModelsData> m_vData = new Dictionary<uint, ModelsData>();
		//-------------------------------------------
		public Dictionary<uint, ModelsData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Models()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public ModelsData GetData(uint id)
		{
			ModelsData outData;
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
				
				ModelsData data = new ModelsData();
				
				data.ID = csv[i]["ID"].Uint();
				data.strName = csv[i]["strName"].String();
				data.fHeight = csv[i]["fHeight"].Float();
				data.strFile = csv[i]["strFile"].String();
				data.modelPosition = csv[i]["modelPosition"].Vec3();
				data.modelRotation = csv[i]["modelRotation"].Vec3();
				data.modelScale = csv[i]["modelScale"].Vec3();
				
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
