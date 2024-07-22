//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("基础通用工具",true)]
#endif
	public static class AgentTree_Util
	{
#if UNITY_EDITOR
		[ATMonoFunc(-212781080,"SetActive",typeof(TopGame.Base.Util))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.Base.Util),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"pObj",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodArgv(typeof(VariableBool),"bActive",false, null,null)]
#endif
		public static bool AT_SetActive(VariableObject pObj,VariableBool bActive)
		{
			if(pObj== null || bActive== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Base.Util.SetActive(pObj.ToObject<UnityEngine.GameObject>(),bActive.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(435770816,"显示帮助",typeof(TopGame.Base.Util))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.Base.Util),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"enter",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_ShowAIHelp(VariableString enter,VariableBool pReturn=null)
		{
			if(enter== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Base.Util.ShowAIHelp(enter.mValue);
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -212781080:
				{//TopGame.Base.Util->SetActive
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_Util.AT_SetActive(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case 435770816:
				{//TopGame.Base.Util->ShowAIHelp
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_Util.AT_ShowAIHelp(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
			}
			return true;
		}
	}
}
