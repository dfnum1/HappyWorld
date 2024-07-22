//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("序列化/组件序列化器",true)]
#endif
	public static class AgentTree_ComSerialized
	{
#if UNITY_EDITOR
		[ATMonoFunc(-1798633666,"获取组件",typeof(TopGame.Core.ComSerialized))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.Core.ComSerialized),null)]
		[ATMethodArgv(typeof(VariableString),"name",false, null,null)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",null,null)]
#endif
		public static bool AT_GetWidget(TopGame.Core.ComSerialized pPointer, VariableString name, VariableObject pReturn=null)
		{
			if(name== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetWidget(name.mValue);
			}	
			return true;
		}
		public static bool DoAction(VariableMonoScript pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -1798633666:
				{//TopGame.Core.ComSerialized->GetWidget
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.ComSerialized)) return true;
					return AgentTree_ComSerialized.AT_GetWidget(pUserClass.mValue as TopGame.Core.ComSerialized, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
			}
			return true;
		}
	}
}
