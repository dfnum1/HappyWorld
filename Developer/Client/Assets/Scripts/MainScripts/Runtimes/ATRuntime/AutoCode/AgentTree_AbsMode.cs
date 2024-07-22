//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("游戏状态/模式",true)]
#endif
	public static class AgentTree_AbsMode
	{
#if UNITY_EDITOR
		[ATMonoFunc(-78806146,"RefreshPlayers",typeof(TopGame.Logic.AbsMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Logic.AbsMode),null)]
		[ATMethodArgv(typeof(VariableInt),"group",false, null,null)]
#endif
		public static bool AT_RefreshPlayers(TopGame.Logic.AbsMode pPointer, VariableInt group)
		{
			if(group== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.RefreshPlayers(group.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -78806146:
				{//TopGame.Logic.AbsMode->RefreshPlayers
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.Logic.AbsMode)) return true;
					return AgentTree_AbsMode.AT_RefreshPlayers(pUserClass.mValue as TopGame.Logic.AbsMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
			}
			return true;
		}
	}
}
