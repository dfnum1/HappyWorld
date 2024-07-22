//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("逻辑状态",true)]
#endif
	public static class AgentTree_StateFactory
	{
#if UNITY_EDITOR
		[ATMonoFunc(1613771299,"获取状态",typeof(TopGame.Logic.StateFactory))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Logic.StateFactory),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"state",false, null,typeof(TopGame.Logic.EGameState))]
		[ATMethodArgv(typeof(VariableBool),"bAutoNode",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Logic.AState),null)]
#endif
		public static bool AT_GetState(TopGame.Logic.StateFactory pPointer, VariableInt state, VariableBool bAutoNode, VariableUser pReturn=null)
		{
			if(state== null || bAutoNode== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetState((TopGame.Logic.EGameState)state.mValue,bAutoNode.mValue);
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1613771299:
				{//TopGame.Logic.StateFactory->GetState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().statesFactory;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Logic.StateFactory)) return true;
					return AgentTree_StateFactory.AT_GetState(pUserClass.mValue as TopGame.Logic.StateFactory, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
			}
			return true;
		}
	}
}
