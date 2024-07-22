//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI系统/TransitionLoading",true)]
#endif
	public static class AgentTree_TransitionPanel
	{
#if UNITY_EDITOR
		[ATMonoFunc(1997564683,"GetMode",typeof(TopGame.UI.TransitionPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.TransitionPanel),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(TopGame.Logic.EMode))]
#endif
		public static bool AT_GetMode(TopGame.UI.TransitionPanel pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetMode();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1953355092,"SetSprite",typeof(TopGame.UI.TransitionPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.TransitionPanel),null)]
		[ATMethodArgv(typeof(VariableObject),"pSprite",false, typeof(UnityEngine.Sprite),null)]
#endif
		public static bool AT_SetSprite(TopGame.UI.TransitionPanel pPointer, VariableObject pSprite)
		{
			if(pSprite== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetSprite(pSprite.ToObject<UnityEngine.Sprite>());
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1997564683:
				{//TopGame.UI.TransitionPanel->GetMode
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.TransitionPanel)) return true;
					return AgentTree_TransitionPanel.AT_GetMode(pUserClass.mValue as TopGame.UI.TransitionPanel, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 1953355092:
				{//TopGame.UI.TransitionPanel->SetSprite
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.TransitionPanel)) return true;
					return AgentTree_TransitionPanel.AT_SetSprite(pUserClass.mValue as TopGame.UI.TransitionPanel, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask));
				}
			}
			return true;
		}
	}
}
