/********************************************************************
类    名:   CsvData_GoldDropEffect
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
    public partial class CsvData_GoldDropEffect : Data_Base
    {
		public partial class GoldDropEffectData : BaseData
		{
			public	uint				id;
			public	float[]				dropSpeedY;
			public	float[]				dropSpeedX;
			public	float[]				dropSpeedZ;
			public	int[]				bounceCount;
			public	float				pickDuration;
			public	float[]				IdleTime;
			public	int[]				RoundCnt;
			public	float[]				randomRadius;
			public	string				pickSlot;
			public	float				pickScale;

			//mapping data
		}
		Dictionary<uint, GoldDropEffectData> m_vData = new Dictionary<uint, GoldDropEffectData>();
		//-------------------------------------------
		public Dictionary<uint, GoldDropEffectData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_GoldDropEffect()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public GoldDropEffectData GetData(uint id)
		{
			GoldDropEffectData outData;
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
				
				GoldDropEffectData data = new GoldDropEffectData();
				
				data.id = csv[i]["id"].Uint();
				data.dropSpeedY = csv[i]["dropSpeedY"].FloatArray();
				data.dropSpeedX = csv[i]["dropSpeedX"].FloatArray();
				data.dropSpeedZ = csv[i]["dropSpeedZ"].FloatArray();
				data.bounceCount = csv[i]["bounceCount"].IntArray();
				data.pickDuration = csv[i]["pickDuration"].Float();
				data.IdleTime = csv[i]["IdleTime"].FloatArray();
				data.RoundCnt = csv[i]["RoundCnt"].IntArray();
				data.randomRadius = csv[i]["randomRadius"].FloatArray();
				data.pickSlot = csv[i]["pickSlot"].String();
				data.pickScale = csv[i]["pickScale"].Float();
				
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
