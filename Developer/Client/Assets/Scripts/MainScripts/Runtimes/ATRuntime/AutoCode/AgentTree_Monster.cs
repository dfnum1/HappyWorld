//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("World/Monster",true)]
#endif
	public static class AgentTree_Monster
	{
#if UNITY_EDITOR
		[ATMonoFunc(206428463,"GetMonsterType",typeof(TopGame.Logic.Monster))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Logic.Monster),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(Framework.Base.EMonsterType))]
#endif
		public static bool AT_GetMonsterType(TopGame.Logic.Monster pPointer, VariableInt pReturn=null)
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
				case 206428463:
				{//TopGame.Logic.Monster->GetMonsterType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Logic.Monster)) return true;
					return AgentTree_Monster.AT_GetMonsterType(pUserClass.mValue as TopGame.Logic.Monster, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
			}
			return AgentTree_AMonster.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
