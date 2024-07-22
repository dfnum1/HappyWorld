//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("逻辑状态",true)]
#endif
	public static class AgentTree_AState
	{
#if UNITY_EDITOR
		[ATMonoFunc(-380707247,"是否启用",typeof(TopGame.Logic.AState))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Logic.AState),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsEnable(TopGame.Logic.AState pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsEnable();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1578888436,"获取绑定场景",typeof(TopGame.Logic.AState))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Logic.AState),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetBindSceneID(TopGame.Logic.AState pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetBindSceneID();
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -380707247:
				{//TopGame.Logic.AState->IsEnable
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Logic.AState)) return true;
					return AgentTree_AState.AT_IsEnable(pUserClass.mValue as TopGame.Logic.AState, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -1578888436:
				{//TopGame.Logic.AState->GetBindSceneID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Logic.AState)) return true;
					return AgentTree_AState.AT_GetBindSceneID(pUserClass.mValue as TopGame.Logic.AState, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
			}
			return true;
		}
	}
}
