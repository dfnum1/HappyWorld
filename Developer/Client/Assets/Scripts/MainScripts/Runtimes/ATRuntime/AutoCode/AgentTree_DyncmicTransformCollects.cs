//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("游戏动态引用绑点集",true)]
#endif
	public static class AgentTree_DyncmicTransformCollects
	{
#if UNITY_EDITOR
		[ATMonoFunc(-694021954,"根据标识名获取绑点",typeof(Framework.Core.DyncmicTransformCollects))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.DyncmicTransformCollects),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strName",false, null,null)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.Transform),null)]
#endif
		public static bool AT_FindTransformByName(VariableString strName,VariableObject pReturn=null)
		{
			if(strName== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = Framework.Core.DyncmicTransformCollects.FindTransformByName(strName.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-667608332,"根据唯一GUID获取绑点",typeof(Framework.Core.DyncmicTransformCollects))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.DyncmicTransformCollects),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"Guid",false, null,null)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.Transform),null)]
#endif
		public static bool AT_FindTransformByGUID(VariableInt Guid,VariableObject pReturn=null)
		{
			if(Guid== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = Framework.Core.DyncmicTransformCollects.FindTransformByGUID(Guid.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1782924016,"显示隐藏节点",typeof(Framework.Core.DyncmicTransformCollects))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.DyncmicTransformCollects),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"Guid",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bActive",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bPosActive",false, null,null)]
#endif
		public static bool AT_ActiveNodeByGUID(VariableInt Guid,VariableBool bActive,VariableBool bPosActive)
		{
			if(Guid== null || bActive== null || bPosActive== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.DyncmicTransformCollects.ActiveNodeByGUID(Guid.mValue,bActive.mValue,bPosActive.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-311412660,"显示隐藏节点",typeof(Framework.Core.DyncmicTransformCollects))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.DyncmicTransformCollects),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strName",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bActive",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bPosActive",false, null,null)]
#endif
		public static bool AT_ActiveNodeByName(VariableString strName,VariableBool bActive,VariableBool bPosActive)
		{
			if(strName== null || bActive== null || bPosActive== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.DyncmicTransformCollects.ActiveNodeByName(strName.mValue,bActive.mValue,bPosActive.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -694021954:
				{//Framework.Core.DyncmicTransformCollects->FindTransformByName
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_DyncmicTransformCollects.AT_FindTransformByName(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case -667608332:
				{//Framework.Core.DyncmicTransformCollects->FindTransformByGUID
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_DyncmicTransformCollects.AT_FindTransformByGUID(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case 1782924016:
				{//Framework.Core.DyncmicTransformCollects->ActiveNodeByGUID
					if(pAction.inArgvs.Length <=3) return true;
					return AgentTree_DyncmicTransformCollects.AT_ActiveNodeByGUID(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask));
				}
				case -311412660:
				{//Framework.Core.DyncmicTransformCollects->ActiveNodeByName
					if(pAction.inArgvs.Length <=3) return true;
					return AgentTree_DyncmicTransformCollects.AT_ActiveNodeByName(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask));
				}
			}
			return true;
		}
	}
}
