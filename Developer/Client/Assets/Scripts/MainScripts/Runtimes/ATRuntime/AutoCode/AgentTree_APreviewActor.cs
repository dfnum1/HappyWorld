//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("World/PreviewActor",true)]
#endif
	public static class AgentTree_APreviewActor
	{
#if UNITY_EDITOR
		[ATMonoFunc(-150469370,"IsFrontRow",typeof(Framework.Logic.APreviewActor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Logic.APreviewActor),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsFrontRow(Framework.Logic.APreviewActor pPointer, VariableBool pReturn=null)
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
				case -150469370:
				{//Framework.Logic.APreviewActor->IsFrontRow
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Logic.APreviewActor)) return true;
					return AgentTree_APreviewActor.AT_IsFrontRow(pUserClass.mValue as Framework.Logic.APreviewActor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
			}
			return AgentTree_Actor.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
