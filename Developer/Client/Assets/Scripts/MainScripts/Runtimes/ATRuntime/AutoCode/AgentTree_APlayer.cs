//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("World/Player",true)]
#endif
	public static class AgentTree_APlayer
	{
#if UNITY_EDITOR
		[ATMonoFunc(1699782104,"IsFrontRow",typeof(Framework.BattlePlus.APlayer))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.APlayer),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsFrontRow(Framework.BattlePlus.APlayer pPointer, VariableBool pReturn=null)
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
				case 1699782104:
				{//Framework.BattlePlus.APlayer->IsFrontRow
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.BattlePlus.APlayer)) return true;
					return AgentTree_APlayer.AT_IsFrontRow(pUserClass.mValue as Framework.BattlePlus.APlayer, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
			}
			return AgentTree_Actor.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
