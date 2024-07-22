//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("用户信息/用户",true)]
#endif
	public static class AgentTree_User
	{
#if UNITY_EDITOR
		[ATMonoFunc(837976054,"SetLocationState",typeof(TopGame.SvrData.User))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.SvrData.User),null)]
		[ATMethodArgv(typeof(VariableInt),"location",false, null,typeof(TopGame.SvrData.ELocationState))]
#endif
		public static bool AT_SetLocationState(TopGame.SvrData.User pPointer, VariableInt location)
		{
			if(location== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetLocationState((TopGame.SvrData.ELocationState)location.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1919035152,"GetLocationState",typeof(TopGame.SvrData.User))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.SvrData.User),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(TopGame.SvrData.ELocationState))]
#endif
		public static bool AT_GetLocationState(TopGame.SvrData.User pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetLocationState();
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 837976054:
				{//TopGame.SvrData.User->SetLocationState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.SvrData.UserManager.getInstance().mySelf;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.SvrData.User)) return true;
					return AgentTree_User.AT_SetLocationState(pUserClass.mValue as TopGame.SvrData.User, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 1919035152:
				{//TopGame.SvrData.User->GetLocationState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.SvrData.UserManager.getInstance().mySelf;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.SvrData.User)) return true;
					return AgentTree_User.AT_GetLocationState(pUserClass.mValue as TopGame.SvrData.User, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
			}
			return true;
		}
	}
}
