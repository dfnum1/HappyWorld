//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("场景主题",true)]
#endif
	public static class AgentTree_ASceneTheme
	{
#if UNITY_EDITOR
		[ATMonoFunc(-1465110694,"UseTheme",typeof(TopGame.Core.ASceneTheme))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.ASceneTheme),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"theme",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bClear",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bLerpTo",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_UseTheme(TopGame.Core.ASceneTheme pPointer, VariableInt theme, VariableBool bClear, VariableBool bLerpTo, VariableBool pReturn=null)
		{
			if(theme== null || bClear== null || bLerpTo== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.UseTheme((uint)theme.mValue,bClear.mValue,bLerpTo.mValue);
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -1465110694:
				{//TopGame.Core.ASceneTheme->UseTheme
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.Core.SceneMgr.GetThemer();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.ASceneTheme)) return true;
					return AgentTree_ASceneTheme.AT_UseTheme(pUserClass.mValue as TopGame.Core.ASceneTheme, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
			}
			return true;
		}
	}
}
