//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("World/Monster",true)]
#endif
	public static class AgentTree_AMonster
	{
#if UNITY_EDITOR
		[ATMonoFunc(696251494,"GetMonsterType",typeof(Framework.BattlePlus.AMonster))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.AMonster),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(Framework.Base.EMonsterType))]
#endif
		public static bool AT_GetMonsterType(Framework.BattlePlus.AMonster pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetMonsterType();
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 696251494:
				{//Framework.BattlePlus.AMonster->GetMonsterType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.BattlePlus.AMonster)) return true;
					return AgentTree_AMonster.AT_GetMonsterType(pUserClass.mValue as Framework.BattlePlus.AMonster, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
			}
			return AgentTree_Actor.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
