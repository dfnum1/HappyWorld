//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("文件资源系统/实例化操作",true)]
#endif
	public static class AgentTree_InstanceOperiaon
	{
#if UNITY_EDITOR
		[ATMonoFunc(-1641981117,"刷新",typeof(Framework.Core.InstanceOperiaon))]
#endif
		public static bool AT_Refresh(Framework.Core.InstanceOperiaon pPointer)
		{
			pPointer.Refresh();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-533014188,"get_strFile",typeof(Framework.Core.InstanceOperiaon), true)]
		[ATMethodReturn(typeof(VariableString),"pReturn",null,null)]
#endif
		public static bool AT_get_strFile(Framework.Core.InstanceOperiaon pPointer, VariableString pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.strFile;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-915724092,"get_pAssetPrefab",typeof(Framework.Core.InstanceOperiaon), true)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.GameObject),null)]
#endif
		public static bool AT_get_pAssetPrefab(Framework.Core.InstanceOperiaon pPointer, VariableObject pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.pAssetPrefab;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1769842198,"get_pPoolAble",typeof(Framework.Core.InstanceOperiaon), true)]
		[ATMethodReturn(typeof(VariableMonoScript),"pReturn",typeof(Framework.Core.AInstanceAble),null)]
#endif
		public static bool AT_get_pPoolAble(Framework.Core.InstanceOperiaon pPointer, VariableMonoScript pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (UnityEngine.Behaviour)pPointer.pPoolAble;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-243243470,"get_userPointer",typeof(Framework.Core.InstanceOperiaon), true)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",null,null)]
#endif
		public static bool AT_get_userPointer(Framework.Core.InstanceOperiaon pPointer, VariableObject pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.userPointer;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1067359262,"set_userPointer",typeof(Framework.Core.InstanceOperiaon))]
		[ATMethodArgv(typeof(VariableObject),"自定义参数-Object",false, null,null)]
#endif
		public static bool AT_set_userPointer(Framework.Core.InstanceOperiaon pPointer, VariableObject userPointer)
		{
			if(userPointer== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.userPointer = userPointer.mValue;
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(74264770,"get_bUsed",typeof(Framework.Core.InstanceOperiaon), true)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_get_bUsed(Framework.Core.InstanceOperiaon pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.bUsed;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-180280385,"set_bUsed",typeof(Framework.Core.InstanceOperiaon))]
		[ATMethodArgv(typeof(VariableBool),"bUsed",false, null,null)]
#endif
		public static bool AT_set_bUsed(Framework.Core.InstanceOperiaon pPointer, VariableBool bUsed)
		{
			if(bUsed== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.bUsed = bUsed.mValue;
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1090322877,"get_bScene",typeof(Framework.Core.InstanceOperiaon), true)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_get_bScene(Framework.Core.InstanceOperiaon pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.bScene;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1857560623,"set_bScene",typeof(Framework.Core.InstanceOperiaon))]
		[ATMethodArgv(typeof(VariableBool),"bScene",false, null,null)]
#endif
		public static bool AT_set_bScene(Framework.Core.InstanceOperiaon pPointer, VariableBool bScene)
		{
			if(bScene== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.bScene = bScene.mValue;
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(157585951,"get_bAsync",typeof(Framework.Core.InstanceOperiaon), true)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_get_bAsync(Framework.Core.InstanceOperiaon pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.bAsync;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(656520077,"set_bAsync",typeof(Framework.Core.InstanceOperiaon))]
		[ATMethodArgv(typeof(VariableBool),"bAsync",false, null,null)]
#endif
		public static bool AT_set_bAsync(Framework.Core.InstanceOperiaon pPointer, VariableBool bAsync)
		{
			if(bAsync== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.bAsync = bAsync.mValue;
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -1641981117:
				{//Framework.Core.InstanceOperiaon->Refresh
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_Refresh(pUserClass.mValue as Framework.Core.InstanceOperiaon);
				}
				case -533014188:
				{//Framework.Core.InstanceOperiaon->strFile
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_get_strFile(pUserClass.mValue as Framework.Core.InstanceOperiaon, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableString>(0, pTask));
				}
				case -915724092:
				{//Framework.Core.InstanceOperiaon->pAssetPrefab
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_get_pAssetPrefab(pUserClass.mValue as Framework.Core.InstanceOperiaon, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case 1769842198:
				{//Framework.Core.InstanceOperiaon->pPoolAble
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_get_pPoolAble(pUserClass.mValue as Framework.Core.InstanceOperiaon, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(0, pTask));
				}
				case -243243470:
				{//Framework.Core.InstanceOperiaon->userPointer
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_get_userPointer(pUserClass.mValue as Framework.Core.InstanceOperiaon, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case 1067359262:
				{//Framework.Core.InstanceOperiaon->userPointer
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_set_userPointer(pUserClass.mValue as Framework.Core.InstanceOperiaon, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask));
				}
				case 74264770:
				{//Framework.Core.InstanceOperiaon->bUsed
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_get_bUsed(pUserClass.mValue as Framework.Core.InstanceOperiaon, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -180280385:
				{//Framework.Core.InstanceOperiaon->bUsed
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_set_bUsed(pUserClass.mValue as Framework.Core.InstanceOperiaon, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
				case -1090322877:
				{//Framework.Core.InstanceOperiaon->bScene
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_get_bScene(pUserClass.mValue as Framework.Core.InstanceOperiaon, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -1857560623:
				{//Framework.Core.InstanceOperiaon->bScene
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_set_bScene(pUserClass.mValue as Framework.Core.InstanceOperiaon, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
				case 157585951:
				{//Framework.Core.InstanceOperiaon->bAsync
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_get_bAsync(pUserClass.mValue as Framework.Core.InstanceOperiaon, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 656520077:
				{//Framework.Core.InstanceOperiaon->bAsync
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.InstanceOperiaon)) return true;
					return AgentTree_InstanceOperiaon.AT_set_bAsync(pUserClass.mValue as Framework.Core.InstanceOperiaon, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
			}
			return true;
		}
	}
}
