//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("World/Player",true)]
#endif
	public static class AgentTree_Player
	{
#if UNITY_EDITOR
		[ATMonoFunc(-229800794,"IsFrontRow",typeof(TopGame.Logic.Player))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Logic.Player),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsFrontRow(TopGame.Logic.Player pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsFrontRow();
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -229800794:
				{//TopGame.Logic.Player->IsFrontRow
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Logic.Player)) return true;
					return AgentTree_Player.AT_IsFrontRow(pUserClass.mValue as TopGame.Logic.Player, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
			}
			return AgentTree_APlayer.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
