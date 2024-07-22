//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("用户信息",true)]
#endif
	public static class AgentTree_UserManager
	{
#if UNITY_EDITOR
		[ATMonoFunc(-1768429031,"获取用户",typeof(TopGame.SvrData.UserManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.SvrData.UserManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableLong),"userID",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.SvrData.User),null)]
#endif
		public static bool AT_GetUser(TopGame.SvrData.UserManager pPointer, VariableLong userID, VariableUser pReturn=null)
		{
			if(userID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetUser(userID.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-78302368,"获取当前用户",typeof(TopGame.SvrData.UserManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.SvrData.UserManager),null, "", false, -1, true,false)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.SvrData.User),null)]
#endif
		public static bool AT_GetCurUser(TopGame.SvrData.UserManager pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurUser();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-609659953,"添加用户",typeof(TopGame.SvrData.UserManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.SvrData.UserManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableLong),"userID",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.SvrData.User),null)]
#endif
		public static bool AT_AddUser(TopGame.SvrData.UserManager pPointer, VariableLong userID, VariableUser pReturn=null)
		{
			if(userID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.AddUser(userID.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2035959315,"清理用户列表",typeof(TopGame.SvrData.UserManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.SvrData.UserManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableBool),"bIncludeMyself",false, null,null)]
#endif
		public static bool AT_ClearUser(TopGame.SvrData.UserManager pPointer, VariableBool bIncludeMyself)
		{
			if(bIncludeMyself== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ClearUser(bIncludeMyself.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-119579179,"get_mySelf",typeof(TopGame.SvrData.UserManager), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.SvrData.UserManager),null, "", false, -1, true,false)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.SvrData.User),null)]
#endif
		public static bool AT_get_mySelf(TopGame.SvrData.UserManager pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.mySelf;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-694518713,"set_mySelf",typeof(TopGame.SvrData.UserManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.SvrData.UserManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableUser),"我自己",false, typeof(TopGame.SvrData.User),null)]
#endif
		public static bool AT_set_mySelf(TopGame.SvrData.UserManager pPointer, VariableUser mySelf)
		{
			if(mySelf== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.mySelf = (TopGame.SvrData.User)mySelf.mValue;
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -1768429031:
				{//TopGame.SvrData.UserManager->GetUser
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().userManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.SvrData.UserManager)) return true;
					return AgentTree_UserManager.AT_GetUser(pUserClass.mValue as TopGame.SvrData.UserManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableLong>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -78302368:
				{//TopGame.SvrData.UserManager->GetCurUser
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().userManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.SvrData.UserManager)) return true;
					return AgentTree_UserManager.AT_GetCurUser(pUserClass.mValue as TopGame.SvrData.UserManager, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -609659953:
				{//TopGame.SvrData.UserManager->AddUser
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().userManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.SvrData.UserManager)) return true;
					return AgentTree_UserManager.AT_AddUser(pUserClass.mValue as TopGame.SvrData.UserManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableLong>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -2035959315:
				{//TopGame.SvrData.UserManager->ClearUser
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().userManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.SvrData.UserManager)) return true;
					return AgentTree_UserManager.AT_ClearUser(pUserClass.mValue as TopGame.SvrData.UserManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
				case -119579179:
				{//TopGame.SvrData.UserManager->mySelf
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().userManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.SvrData.UserManager)) return true;
					return AgentTree_UserManager.AT_get_mySelf(pUserClass.mValue as TopGame.SvrData.UserManager, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -694518713:
				{//TopGame.SvrData.UserManager->mySelf
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().userManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.SvrData.UserManager)) return true;
					return AgentTree_UserManager.AT_set_mySelf(pUserClass.mValue as TopGame.SvrData.UserManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(1, pTask));
				}
			}
			return true;
		}
	}
}
