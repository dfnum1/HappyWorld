//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI系统/界面视图",true)]
#endif
	public static class AgentTree_UIView
	{
#if UNITY_EDITOR
		[ATMonoFunc(-1707741647,"清理",typeof(TopGame.UI.UIView))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIView),null)]
		[ATMethodArgv(typeof(VariableBool),"bUnloadDynamic",false, null,null)]
#endif
		public static bool AT_Clear(TopGame.UI.UIView pPointer, VariableBool bUnloadDynamic)
		{
			if(bUnloadDynamic== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.Clear(bUnloadDynamic.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -1707741647:
				{//TopGame.UI.UIView->Clear
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIView)) return true;
					return AgentTree_UIView.AT_Clear(pUserClass.mValue as TopGame.UI.UIView, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
			}
			return true;
		}
	}
}
