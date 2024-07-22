//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("World/WorldNode",true)]
#endif
	public static class AgentTree_AWorldNode
	{
#if UNITY_EDITOR
		[ATMonoFunc(699506516,"GetConfigID",typeof(Framework.Core.AWorldNode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.AWorldNode),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetConfigID(Framework.Core.AWorldNode pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetConfigID();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1485093526,"GetElementFlags",typeof(Framework.Core.AWorldNode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.AWorldNode),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetElementFlags(Framework.Core.AWorldNode pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetElementFlags();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2041391782,"GetActorType",typeof(Framework.Core.AWorldNode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.AWorldNode),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(Framework.Core.EActorType))]
#endif
		public static bool AT_GetActorType(Framework.Core.AWorldNode pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetActorType();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(265337666,"GetAttackGroup",typeof(Framework.Core.AWorldNode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.AWorldNode),null)]
		[ATMethodReturn(typeof(VariableByte),"pReturn",null,null)]
#endif
		public static bool AT_GetAttackGroup(Framework.Core.AWorldNode pPointer, VariableByte pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetAttackGroup();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1974416911,"GetObjectAble",typeof(Framework.Core.AWorldNode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.AWorldNode),null)]
		[ATMethodReturn(typeof(VariableMonoScript),"pReturn",typeof(Framework.Core.AInstanceAble),null)]
#endif
		public static bool AT_GetObjectAble(Framework.Core.AWorldNode pPointer, VariableMonoScript pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (UnityEngine.Behaviour)pPointer.GetObjectAble();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(7096612,"GetInstanceID",typeof(Framework.Core.AWorldNode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.AWorldNode),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetInstanceID(Framework.Core.AWorldNode pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetInstanceID();
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 699506516:
				{//Framework.Core.AWorldNode->GetConfigID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.AWorldNode)) return true;
					return AgentTree_AWorldNode.AT_GetConfigID(pUserClass.mValue as Framework.Core.AWorldNode, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -1485093526:
				{//Framework.Core.AWorldNode->GetElementFlags
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.AWorldNode)) return true;
					return AgentTree_AWorldNode.AT_GetElementFlags(pUserClass.mValue as Framework.Core.AWorldNode, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -2041391782:
				{//Framework.Core.AWorldNode->GetActorType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.AWorldNode)) return true;
					return AgentTree_AWorldNode.AT_GetActorType(pUserClass.mValue as Framework.Core.AWorldNode, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 265337666:
				{//Framework.Core.AWorldNode->GetAttackGroup
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.AWorldNode)) return true;
					return AgentTree_AWorldNode.AT_GetAttackGroup(pUserClass.mValue as Framework.Core.AWorldNode, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableByte>(0, pTask));
				}
				case 1974416911:
				{//Framework.Core.AWorldNode->GetObjectAble
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.AWorldNode)) return true;
					return AgentTree_AWorldNode.AT_GetObjectAble(pUserClass.mValue as Framework.Core.AWorldNode, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(0, pTask));
				}
				case 7096612:
				{//Framework.Core.AWorldNode->GetInstanceID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.AWorldNode)) return true;
					return AgentTree_AWorldNode.AT_GetInstanceID(pUserClass.mValue as Framework.Core.AWorldNode, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
			}
			return true;
		}
	}
}
