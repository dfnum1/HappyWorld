/********************************************************************
类    名:   CsvData_Dialog
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
	[Framework.Plugin.AT.ATExportMono("配置数据/Dialog", "TopGame.Data.DataManager.getInstance().Dialog")]
    public partial class CsvData_Dialog : Data_Base
    {
		[Framework.Plugin.AT.ATExportMono("配置数据/Dialog/DialogData")]
		public partial class DialogData : BaseData
		{
			[Framework.Plugin.AT.ATField("",null,"",1)]public	uint				ID;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	uint				nNextID;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	uint				nContext;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	uint[]				beiginEvents;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	uint[]				endEvents;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	TopGame.Logic.EDialogTriggerType				eTriggerType;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	uint[]				nTriggerParams;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	string[]				strTriggerParams;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	uint				nSyaName;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	uint				nDesName;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	uint				nModel;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	string				playAction;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	Vector3				vModelCameraOffset;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	float				fModelRotate;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	Vector2Int				vHudSize;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	string				strSound;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	float				fSeat;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	bool				bPause;

			//mapping data
			[Framework.Plugin.AT.ATField("",null,"",1)]public	CsvData_Text.TextData Text_nContext_data;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	CsvData_Text.TextData Text_nSyaName_data;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	CsvData_Text.TextData Text_nDesName_data;
			[Framework.Plugin.AT.ATField("",null,"",1)]public	CsvData_Models.ModelsData Models_nModel_data;
		}
		Dictionary<uint, DialogData> m_vData = new Dictionary<uint, DialogData>();
		//-------------------------------------------
		[Framework.Plugin.AT.ATField("数据列表")]
		public Dictionary<uint, DialogData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Dialog()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		[Framework.Plugin.AT.ATMethod("查找数据")]
		public DialogData GetData(uint id)
		{
			DialogData outData;
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
				
				DialogData data = new DialogData();
				
				data.ID = csv[i]["ID"].Uint();
				data.nNextID = csv[i]["nNextID"].Uint();
				data.nContext = csv[i]["nContext"].Uint();
				data.beiginEvents = csv[i]["beiginEvents"].UintArray();
				data.endEvents = csv[i]["endEvents"].UintArray();
				data.eTriggerType = (TopGame.Logic.EDialogTriggerType)csv[i]["eTriggerType"].Int();
				data.nTriggerParams = csv[i]["nTriggerParams"].UintArray();
				data.strTriggerParams = csv[i]["strTriggerParams"].StringArray();
				data.nSyaName = csv[i]["nSyaName"].Uint();
				data.nDesName = csv[i]["nDesName"].Uint();
				data.nModel = csv[i]["nModel"].Uint();
				data.playAction = csv[i]["playAction"].String();
				data.vModelCameraOffset = csv[i]["vModelCameraOffset"].Vec3();
				data.fModelRotate = csv[i]["fModelRotate"].Float();
				data.vHudSize = csv[i]["vHudSize"].Vec2int();
				data.strSound = csv[i]["strSound"].String();
				data.fSeat = csv[i]["fSeat"].Float();
				data.bPause = csv[i]["bPause"].Bool();
				
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
