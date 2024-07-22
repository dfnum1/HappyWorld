//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI系统/登录/界面",true)]
#endif
	public static class AgentTree_UILogin
	{
#if UNITY_EDITOR
		[ATMonoFunc(-43411419,"Login",typeof(TopGame.UI.UILogin))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UILogin),null)]
#endif
		public static bool AT_Login(TopGame.UI.UILogin pPointer)
		{
			pPointer.Login();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(239707427,"修复",typeof(TopGame.UI.UILogin))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UILogin),null)]
#endif
		public static bool AT_RepairGame(TopGame.UI.UILogin pPointer)
		{
			pPointer.RepairGame();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1780598287,"客服",typeof(TopGame.UI.UILogin))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UILogin),null)]
		[ATMethodArgv(typeof(VariableString),"enter",false, null,null)]
#endif
		public static bool AT_CustomerService(TopGame.UI.UILogin pPointer, VariableString enter)
		{
			if(enter== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.CustomerService(enter.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-710969690,"注销",typeof(TopGame.UI.UILogin))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UILogin),null)]
#endif
		public static bool AT_LogOut(TopGame.UI.UILogin pPointer)
		{
			pPointer.LogOut();
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -43411419:
				{//TopGame.UI.UILogin->Login
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UILogin)) return true;
					return AgentTree_UILogin.AT_Login(pUserClass.mValue as TopGame.UI.UILogin);
				}
				case 239707427:
				{//TopGame.UI.UILogin->RepairGame
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UILogin)) return true;
					return AgentTree_UILogin.AT_RepairGame(pUserClass.mValue as TopGame.UI.UILogin);
				}
				case -1780598287:
				{//TopGame.UI.UILogin->CustomerService
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UILogin)) return true;
					return AgentTree_UILogin.AT_CustomerService(pUserClass.mValue as TopGame.UI.UILogin, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask));
				}
				case -710969690:
				{//TopGame.UI.UILogin->LogOut
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UILogin)) return true;
					return AgentTree_UILogin.AT_LogOut(pUserClass.mValue as TopGame.UI.UILogin);
				}
			}
			return AgentTree_UIBase.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
