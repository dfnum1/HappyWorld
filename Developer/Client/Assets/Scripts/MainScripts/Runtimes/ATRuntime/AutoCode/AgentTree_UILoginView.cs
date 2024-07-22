//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI系统/登录/视图",true)]
#endif
	public static class AgentTree_UILoginView
	{
#if UNITY_EDITOR
		[ATMonoFunc(-1867741274,"是否有缓存账号",typeof(TopGame.UI.UILoginView))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UILoginView),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_HasAccountCache(TopGame.UI.UILoginView pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.HasAccountCache();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2145244606,"点击进入游戏或者登录",typeof(TopGame.UI.UILoginView))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UILoginView),null)]
#endif
		public static bool AT_ReqLogin(TopGame.UI.UILoginView pPointer)
		{
			pPointer.ReqLogin();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-220747138,"是否离线",typeof(TopGame.UI.UILoginView))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UILoginView),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsOffline(TopGame.UI.UILoginView pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsOffline();
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -1867741274:
				{//TopGame.UI.UILoginView->HasAccountCache
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UILoginView)) return true;
					return AgentTree_UILoginView.AT_HasAccountCache(pUserClass.mValue as TopGame.UI.UILoginView, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -2145244606:
				{//TopGame.UI.UILoginView->ReqLogin
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UILoginView)) return true;
					return AgentTree_UILoginView.AT_ReqLogin(pUserClass.mValue as TopGame.UI.UILoginView);
				}
				case -220747138:
				{//TopGame.UI.UILoginView->IsOffline
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UILoginView)) return true;
					return AgentTree_UILoginView.AT_IsOffline(pUserClass.mValue as TopGame.UI.UILoginView, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
			}
			return AgentTree_UIView.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
