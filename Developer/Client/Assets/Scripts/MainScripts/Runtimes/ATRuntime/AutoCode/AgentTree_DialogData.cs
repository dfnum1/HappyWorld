//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("配置数据/Dialog/DialogData",true)]
#endif
	public static class AgentTree_DialogData
	{
#if UNITY_EDITOR
		[ATMonoFunc(-1302729340,"get_ID",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_get_ID(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.ID;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1155876060,"get_nNextID",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_get_nNextID(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.nNextID;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1728301036,"get_nContext",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_get_nContext(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.nContext;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-266632684,"get_beiginEvents",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
#endif
		public static bool AT_get_beiginEvents(TopGame.Data.CsvData_Dialog.DialogData pPointer)
		{
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1609868904,"get_endEvents",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
#endif
		public static bool AT_get_endEvents(TopGame.Data.CsvData_Dialog.DialogData pPointer)
		{
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-913051463,"get_eTriggerType",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(TopGame.Logic.EDialogTriggerType))]
#endif
		public static bool AT_get_eTriggerType(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.eTriggerType;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-117060606,"get_nTriggerParams",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
#endif
		public static bool AT_get_nTriggerParams(TopGame.Data.CsvData_Dialog.DialogData pPointer)
		{
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1029166286,"get_strTriggerParams",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
#endif
		public static bool AT_get_strTriggerParams(TopGame.Data.CsvData_Dialog.DialogData pPointer)
		{
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1835508854,"get_nSyaName",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_get_nSyaName(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.nSyaName;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1591082952,"get_nDesName",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_get_nDesName(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.nDesName;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-872421127,"get_nModel",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_get_nModel(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.nModel;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1468136122,"get_playAction",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableString),"pReturn",null,null)]
#endif
		public static bool AT_get_playAction(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableString pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.playAction;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(364523703,"get_vModelCameraOffset",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_get_vModelCameraOffset(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.vModelCameraOffset;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1609538139,"get_fModelRotate",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_get_fModelRotate(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.fModelRotate;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1323215943,"get_vHudSize",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableVector2Int),"pReturn",null,null)]
#endif
		public static bool AT_get_vHudSize(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableVector2Int pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.vHudSize;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1852718107,"get_strSound",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableString),"pReturn",null,null)]
#endif
		public static bool AT_get_strSound(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableString pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.strSound;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(198655990,"get_fSeat",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_get_fSeat(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.fSeat;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1137522506,"get_bPause",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_get_bPause(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.bPause;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(209428047,"get_Text_nContext_data",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Data.CsvData_Text.TextData),null)]
#endif
		public static bool AT_get_Text_nContext_data(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.Text_nContext_data;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1964114007,"get_Text_nSyaName_data",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Data.CsvData_Text.TextData),null)]
#endif
		public static bool AT_get_Text_nSyaName_data(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.Text_nSyaName_data;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1977285635,"get_Text_nDesName_data",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Data.CsvData_Text.TextData),null)]
#endif
		public static bool AT_get_Text_nDesName_data(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.Text_nDesName_data;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(397569405,"get_Models_nModel_data",typeof(TopGame.Data.CsvData_Dialog.DialogData), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Data.CsvData_Models.ModelsData),null)]
#endif
		public static bool AT_get_Models_nModel_data(TopGame.Data.CsvData_Dialog.DialogData pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.Models_nModel_data;
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -1302729340:
				{//TopGame.Data.CsvData_Dialog.DialogData->ID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_ID(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 1155876060:
				{//TopGame.Data.CsvData_Dialog.DialogData->nNextID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_nNextID(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 1728301036:
				{//TopGame.Data.CsvData_Dialog.DialogData->nContext
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_nContext(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -266632684:
				{//TopGame.Data.CsvData_Dialog.DialogData->beiginEvents
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_beiginEvents(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData);
				}
				case 1609868904:
				{//TopGame.Data.CsvData_Dialog.DialogData->endEvents
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_endEvents(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData);
				}
				case -913051463:
				{//TopGame.Data.CsvData_Dialog.DialogData->eTriggerType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_eTriggerType(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -117060606:
				{//TopGame.Data.CsvData_Dialog.DialogData->nTriggerParams
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_nTriggerParams(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData);
				}
				case 1029166286:
				{//TopGame.Data.CsvData_Dialog.DialogData->strTriggerParams
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_strTriggerParams(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData);
				}
				case -1835508854:
				{//TopGame.Data.CsvData_Dialog.DialogData->nSyaName
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_nSyaName(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 1591082952:
				{//TopGame.Data.CsvData_Dialog.DialogData->nDesName
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_nDesName(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -872421127:
				{//TopGame.Data.CsvData_Dialog.DialogData->nModel
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_nModel(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -1468136122:
				{//TopGame.Data.CsvData_Dialog.DialogData->playAction
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_playAction(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableString>(0, pTask));
				}
				case 364523703:
				{//TopGame.Data.CsvData_Dialog.DialogData->vModelCameraOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_vModelCameraOffset(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 1609538139:
				{//TopGame.Data.CsvData_Dialog.DialogData->fModelRotate
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_fModelRotate(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case 1323215943:
				{//TopGame.Data.CsvData_Dialog.DialogData->vHudSize
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_vHudSize(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector2Int>(0, pTask));
				}
				case -1852718107:
				{//TopGame.Data.CsvData_Dialog.DialogData->strSound
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_strSound(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableString>(0, pTask));
				}
				case 198655990:
				{//TopGame.Data.CsvData_Dialog.DialogData->fSeat
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_fSeat(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case -1137522506:
				{//TopGame.Data.CsvData_Dialog.DialogData->bPause
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_bPause(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 209428047:
				{//TopGame.Data.CsvData_Dialog.DialogData->Text_nContext_data
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_Text_nContext_data(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 1964114007:
				{//TopGame.Data.CsvData_Dialog.DialogData->Text_nSyaName_data
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_Text_nSyaName_data(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -1977285635:
				{//TopGame.Data.CsvData_Dialog.DialogData->Text_nDesName_data
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_Text_nDesName_data(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 397569405:
				{//TopGame.Data.CsvData_Dialog.DialogData->Models_nModel_data
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog.DialogData)) return true;
					return AgentTree_DialogData.AT_get_Models_nModel_data(pUserClass.mValue as TopGame.Data.CsvData_Dialog.DialogData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
			}
			return true;
		}
	}
}
