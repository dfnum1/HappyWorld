/********************************************************************
类    名:   CsvData_EventDatas
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
    public partial class CsvData_EventDatas : Data_Base
    {
		Dictionary<int, TopGame.Data.EventData> m_vData = new Dictionary<int, TopGame.Data.EventData>();
		//-------------------------------------------
		public Dictionary<int, TopGame.Data.EventData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_EventDatas()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public TopGame.Data.EventData GetData(int id)
		{
			TopGame.Data.EventData outData;
			if(m_vData.TryGetValue(id, out outData))
				return outData;
			return null;
		}
        //-------------------------------------------
        public override bool LoadJson(string strContext)
        {
			m_vData.Clear();
			try
			{
	#if USE_SERVER
				TopGame.Data.EventDatas datas = ExternEngine.JsonUtility.FromJson<TopGame.Data.EventDatas>(strContext);
	#else
				TopGame.Data.EventDatas datas = UnityEngine.JsonUtility.FromJson<TopGame.Data.EventDatas>(strContext);
	#endif
				
				if(datas == null) return false;
				if(datas.datas == null || datas.datas.Length == 0) return true;
				for(int i= 0; i < datas.datas.Length; i++)
				{				
					m_vData.Add(datas.datas[i].nID, datas.datas[i]);
					OnAddData(datas.datas[i]);
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogError(ex.ToString());
			}			
			OnLoadCompleted();
            return true;
        }
	#if UNITY_EDITOR
        //-------------------------------------------
        public override void Save(string strPath=null)
        {
			if(string.IsNullOrEmpty(strPath)) strPath = strFilePath;
		    if (System.IO.File.Exists(strPath))
                System.IO.File.Delete(strPath);	
			List<TopGame.Data.EventData> saves = new List<TopGame.Data.EventData>();
			foreach(var item in m_vData)
				saves.Add(item.Value);
			TopGame.Data.EventDatas db = new TopGame.Data.EventDatas();	
			db.datas = saves.ToArray();
#if USE_SERVER
			string strJson = ExternEngine.JsonUtility.ToJson(db,true);
#else
			string strJson = UnityEngine.JsonUtility.ToJson(db,true);
#endif
			System.IO.FileStream fs = new System.IO.FileStream(strPath, System.IO.FileMode.OpenOrCreate);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
            sw.Write(strJson);
            sw.Close();
        }
	#endif
        //-------------------------------------------
        public override void ClearData()
        {
			m_vData.Clear();
			base.ClearData();
        }
    }
}
