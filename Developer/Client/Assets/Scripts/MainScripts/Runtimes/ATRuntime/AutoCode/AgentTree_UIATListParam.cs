//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI通用工具",true)]
#endif
	public static class AgentTree_UIATListParam
	{
#if UNITY_EDITOR
		[ATMonoFunc(194438000,"get_pSerialized",typeof(TopGame.UI.UIATListParam), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIATListParam),null)]
		[ATMethodReturn(typeof(VariableMonoScript),"pReturn",typeof(TopGame.UI.UISerialized),null)]
#endif
		public static bool AT_get_pSerialized(TopGame.UI.UIATListParam pPointer, VariableMonoScript pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.pSerialized;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-898839546,"get_pUserData",typeof(TopGame.UI.UIATListParam), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIATListParam),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Plugin.AT.IUserData),null)]
#endif
		public static bool AT_get_pUserData(TopGame.UI.UIATListParam pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.pUserData;
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 194438000:
				{//TopGame.UI.UIATListParam->pSerialized
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_UIATListParam.AT_get_pSerialized((TopGame.UI.UIATListParam)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(0, pTask));
				}
				case -898839546:
				{//TopGame.UI.UIATListParam->pUserData
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_UIATListParam.AT_get_pUserData((TopGame.UI.UIATListParam)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
			}
			return true;
		}
	}
}
