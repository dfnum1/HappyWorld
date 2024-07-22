//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("网络模块/AServerSession",true)]
#endif
	public static class AgentTree_AServerSession
	{
#if UNITY_EDITOR
		[ATMonoFunc(-1578242505,"GetIndex",typeof(TopGame.Net.AServerSession))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Net.AServerSession),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetIndex(TopGame.Net.AServerSession pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetIndex();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(560171941,"GetIp",typeof(TopGame.Net.AServerSession))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Net.AServerSession),null)]
		[ATMethodReturn(typeof(VariableString),"pReturn",null,null)]
#endif
		public static bool AT_GetIp(TopGame.Net.AServerSession pPointer, VariableString pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetIp();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1782044709,"GetPort",typeof(TopGame.Net.AServerSession))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Net.AServerSession),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetPort(TopGame.Net.AServerSession pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetPort();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-425397646,"GetAddress",typeof(TopGame.Net.AServerSession))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Net.AServerSession),null)]
		[ATMethodReturn(typeof(VariableString),"pReturn",null,null)]
#endif
		public static bool AT_GetAddress(TopGame.Net.AServerSession pPointer, VariableString pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetAddress();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(68655038,"GetState",typeof(TopGame.Net.AServerSession))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Net.AServerSession),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(TopGame.Net.ESessionState))]
#endif
		public static bool AT_GetState(TopGame.Net.AServerSession pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetState();
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -1578242505:
				{//TopGame.Net.AServerSession->GetIndex
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Net.AServerSession)) return true;
					return AgentTree_AServerSession.AT_GetIndex(pUserClass.mValue as TopGame.Net.AServerSession, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 560171941:
				{//TopGame.Net.AServerSession->GetIp
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Net.AServerSession)) return true;
					return AgentTree_AServerSession.AT_GetIp(pUserClass.mValue as TopGame.Net.AServerSession, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableString>(0, pTask));
				}
				case -1782044709:
				{//TopGame.Net.AServerSession->GetPort
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Net.AServerSession)) return true;
					return AgentTree_AServerSession.AT_GetPort(pUserClass.mValue as TopGame.Net.AServerSession, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -425397646:
				{//TopGame.Net.AServerSession->GetAddress
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Net.AServerSession)) return true;
					return AgentTree_AServerSession.AT_GetAddress(pUserClass.mValue as TopGame.Net.AServerSession, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableString>(0, pTask));
				}
				case 68655038:
				{//TopGame.Net.AServerSession->GetState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Net.AServerSession)) return true;
					return AgentTree_AServerSession.AT_GetState(pUserClass.mValue as TopGame.Net.AServerSession, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
			}
			return true;
		}
	}
}
