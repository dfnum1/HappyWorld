//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("游戏主体",true)]
#endif
	public static class AgentTree_GameInstance
	{
#if UNITY_EDITOR
		[ATMonoFunc(-2055845484,"PreloadInstances",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableStringList),"vPreLoad",false, null,null)]
#endif
		public static bool AT_PreloadInstances(TopGame.GameInstance pPointer, VariableStringList vPreLoad)
		{
			if(vPreLoad== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_String_Catch_0 == null) ms_String_Catch_0= new System.Collections.Generic.List<System.String>();
				for(int i = 0; i < vPreLoad.GetList().Count; ++i)
				{
					if(vPreLoad.mValue[i]!=null)
						ms_String_Catch_0.Add( (System.String)vPreLoad.mValue[i]);
				}
			}
			pPointer.PreloadInstances(ms_String_Catch_0);
			if(ms_String_Catch_0!=null) ms_String_Catch_0.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-816304263,"PreloadInstance",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"instance",false, null,typeof(UnityEngine.GameObject))]
		[ATMethodArgv(typeof(VariableInt),"cnt",false, null,null)]
#endif
		public static bool AT_PreloadInstance(TopGame.GameInstance pPointer, VariableString instance, VariableInt cnt)
		{
			if(instance== null || cnt== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.PreloadInstance(instance.mValue,cnt.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2109360973,"PreloadAsset",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strPreLoad",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAsync",false, null,null)]
#endif
		public static bool AT_PreloadAsset(TopGame.GameInstance pPointer, VariableString strPreLoad, VariableBool bAsync)
		{
			if(strPreLoad== null || bAsync== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.PreloadAsset(strPreLoad.mValue,bAsync.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1166592248,"手机震动",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(Framework.Plugin.NiceVibrations.HapticTypes))]
#endif
		public static bool AT_Vibrate(TopGame.GameInstance pPointer, VariableInt type)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.Vibrate((Framework.Plugin.NiceVibrations.HapticTypes)type.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(2092791991,"实例化渲染开关",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableBool),"bEnable",false, null,null)]
#endif
		public static bool AT_EnableSupportInstancing(TopGame.GameInstance pPointer, VariableBool bEnable)
		{
			if(bEnable== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.EnableSupportInstancing(bEnable.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2089067694,"获取当前状态",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(TopGame.Logic.EGameState))]
#endif
		public static bool AT_GetState(TopGame.GameInstance pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetState();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(155236381,"获取之前状态",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(TopGame.Logic.EGameState))]
#endif
		public static bool AT_GetPreState(TopGame.GameInstance pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetPreState();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(68567540,"切换游戏坐标状态",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"location",false, null,typeof(TopGame.SvrData.ELocationState))]
		[ATMethodArgv(typeof(VariableInt),"loadingType",false, null,typeof(TopGame.Base.ELoadingType))]
#endif
		public static bool AT_ChangeLocation(TopGame.GameInstance pPointer, VariableInt location, VariableInt loadingType)
		{
			if(location== null || loadingType== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ChangeLocation((TopGame.SvrData.ELocationState)location.mValue,(TopGame.Base.ELoadingType)loadingType.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(118210848,"切换游戏状态",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"eState",false, null,typeof(TopGame.Logic.EGameState))]
		[ATMethodArgv(typeof(VariableInt),"mode",false, null,typeof(TopGame.Logic.EMode))]
		[ATMethodArgv(typeof(VariableInt),"loadingType",false, null,typeof(TopGame.Base.ELoadingType))]
		[ATMethodArgv(typeof(VariableBool),"bForce",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Logic.IMode),null)]
#endif
		public static bool AT_ChangeState(TopGame.GameInstance pPointer, VariableInt eState, VariableInt mode, VariableInt loadingType, VariableBool bForce, VariableUser pReturn=null)
		{
			if(eState== null || mode== null || loadingType== null || bForce== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.ChangeState((TopGame.Logic.EGameState)eState.mValue,(TopGame.Logic.EMode)mode.mValue,(TopGame.Base.ELoadingType)loadingType.mValue,bForce.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-303002616,"同步服务器时间",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableLong),"lTime",false, null,null)]
#endif
		public static bool AT_SynchronousTime(TopGame.GameInstance pPointer, VariableLong lTime)
		{
			if(lTime== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SynchronousTime(lTime.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1043919141,"获取当前版本",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodReturn(typeof(VariableString),"pReturn",null,null)]
#endif
		public static bool AT_GetVersion(TopGame.GameInstance pPointer, VariableString pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetVersion();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(285777891,"获取是否显示激励视频按钮",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_GetIsShowADBtn(TopGame.GameInstance pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetIsShowADBtn();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(485988348,"触发指定ID事件-触发者-作用者",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nEventID",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"pTrigger",false, typeof(Framework.Plugin.AT.IUserData),null)]
		[ATMethodArgv(typeof(VariableUser),"pTarget",false, typeof(Framework.Plugin.AT.IUserData),null)]
#endif
		public static bool AT_TriggertEvent(TopGame.GameInstance pPointer, VariableInt nEventID, VariableUser pTrigger, VariableUser pTarget)
		{
			if(nEventID== null || pTrigger== null || pTarget== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.TriggertEvent(nEventID.mValue,(Framework.Plugin.AT.IUserData)pTrigger.mValue,(Framework.Plugin.AT.IUserData)pTarget.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1790642020,"触发指定ID事件",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nEventID",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"pUseData",false, typeof(Framework.Plugin.AT.IUserData),null)]
#endif
		public static bool AT_TriggertEvent_1(TopGame.GameInstance pPointer, VariableInt nEventID, VariableUser pUseData)
		{
			if(nEventID== null || pUseData== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.TriggertEvent(nEventID.mValue,(Framework.Plugin.AT.IUserData)pUseData.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1458745467,"SDKLogin",typeof(TopGame.GameInstance))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.GameInstance),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"param",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_SDKLogin(VariableString param,VariableBool pReturn=null)
		{
			if(param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.GameInstance.SDKLogin(param.mValue);
			}	
			return true;
		}
		private static System.Collections.Generic.List<System.String> ms_String_Catch_0;
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -2055845484:
				{//TopGame.GameInstance->PreloadInstances
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_PreloadInstances(pUserClass.mValue as TopGame.GameInstance, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableStringList>(1, pTask));
				}
				case -816304263:
				{//TopGame.GameInstance->PreloadInstance
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_PreloadInstance(pUserClass.mValue as TopGame.GameInstance, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask));
				}
				case -2109360973:
				{//TopGame.GameInstance->PreloadAsset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_PreloadAsset(pUserClass.mValue as TopGame.GameInstance, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case 1166592248:
				{//TopGame.GameInstance->Vibrate
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_Vibrate(pUserClass.mValue as TopGame.GameInstance, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 2092791991:
				{//TopGame.GameInstance->EnableSupportInstancing
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_EnableSupportInstancing(pUserClass.mValue as TopGame.GameInstance, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
				case -2089067694:
				{//TopGame.GameInstance->GetState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_GetState(pUserClass.mValue as TopGame.GameInstance, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 155236381:
				{//TopGame.GameInstance->GetPreState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_GetPreState(pUserClass.mValue as TopGame.GameInstance, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 68567540:
				{//TopGame.GameInstance->ChangeLocation
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_ChangeLocation(pUserClass.mValue as TopGame.GameInstance, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask));
				}
				case 118210848:
				{//TopGame.GameInstance->ChangeState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_ChangeState(pUserClass.mValue as TopGame.GameInstance, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -303002616:
				{//TopGame.GameInstance->SynchronousTime
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_SynchronousTime(pUserClass.mValue as TopGame.GameInstance, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableLong>(1, pTask));
				}
				case 1043919141:
				{//TopGame.GameInstance->GetVersion
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_GetVersion(pUserClass.mValue as TopGame.GameInstance, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableString>(0, pTask));
				}
				case 285777891:
				{//TopGame.GameInstance->GetIsShowADBtn
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_GetIsShowADBtn(pUserClass.mValue as TopGame.GameInstance, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 485988348:
				{//TopGame.GameInstance->TriggertEvent
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_TriggertEvent(pUserClass.mValue as TopGame.GameInstance, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(3, pTask));
				}
				case 1790642020:
				{//TopGame.GameInstance->TriggertEvent_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is TopGame.GameInstance)) return true;
					return AgentTree_GameInstance.AT_TriggertEvent_1(pUserClass.mValue as TopGame.GameInstance, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(2, pTask));
				}
				case -1458745467:
				{//TopGame.GameInstance->SDKLogin
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_GameInstance.AT_SDKLogin(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
			}
			return true;
		}
	}
}
