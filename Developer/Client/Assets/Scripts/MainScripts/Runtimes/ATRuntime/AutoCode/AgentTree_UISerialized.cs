//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("序列化/UI组件序列化器",true)]
#endif
	public static class AgentTree_UISerialized
	{
#if UNITY_EDITOR
		[ATMonoFunc(1324311230,"Visible",typeof(TopGame.UI.UISerialized))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.UI.UISerialized),null)]
#endif
		public static bool AT_Visible(TopGame.UI.UISerialized pPointer)
		{
			pPointer.Visible();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1443379720,"Hidden",typeof(TopGame.UI.UISerialized))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.UI.UISerialized),null)]
#endif
		public static bool AT_Hidden(TopGame.UI.UISerialized pPointer)
		{
			pPointer.Hidden();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-246436151,"获取引用对象",typeof(TopGame.UI.UISerialized))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.UI.UISerialized),null)]
		[ATMethodArgv(typeof(VariableString),"strName",false, null,null)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.GameObject),null)]
#endif
		public static bool AT_Find(TopGame.UI.UISerialized pPointer, VariableString strName, VariableObject pReturn=null)
		{
			if(strName== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.Find(strName.mValue);
			}	
			return true;
		}
		public static bool DoAction(VariableMonoScript pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1324311230:
				{//TopGame.UI.UISerialized->Visible
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UISerialized)) return true;
					return AgentTree_UISerialized.AT_Visible(pUserClass.mValue as TopGame.UI.UISerialized);
				}
				case 1443379720:
				{//TopGame.UI.UISerialized->Hidden
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UISerialized)) return true;
					return AgentTree_UISerialized.AT_Hidden(pUserClass.mValue as TopGame.UI.UISerialized);
				}
				case -246436151:
				{//TopGame.UI.UISerialized->Find
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UISerialized)) return true;
					return AgentTree_UISerialized.AT_Find(pUserClass.mValue as TopGame.UI.UISerialized, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
			}
			return AgentTree_ComSerialized.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
