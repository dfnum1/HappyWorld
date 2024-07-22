//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI系统/组件/选择组组件",true)]
#endif
	public static class AgentTree_SelectToggleGroup
	{
#if UNITY_EDITOR
		[ATMonoFunc(988097153,"SetSelect",typeof(TopGame.UI.SelectToggleGroup))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.UI.SelectToggleGroup),null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetSelect(TopGame.UI.SelectToggleGroup pPointer, VariableInt index)
		{
			if(index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetSelect(index.mValue);
			return true;
		}
		public static bool DoAction(VariableMonoScript pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 988097153:
				{//TopGame.UI.SelectToggleGroup->SetSelect
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.SelectToggleGroup)) return true;
					return AgentTree_SelectToggleGroup.AT_SetSelect(pUserClass.mValue as TopGame.UI.SelectToggleGroup, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
			}
			return true;
		}
	}
}
