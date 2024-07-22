//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI系统/对话界面",true)]
#endif
	public static class AgentTree_DialogPanel
	{
#if UNITY_EDITOR
		[ATMonoFunc(900571167,"GetCurDialogData",typeof(TopGame.UI.DialogPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.DialogPanel),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
#endif
		public static bool AT_GetCurDialogData(TopGame.UI.DialogPanel pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurDialogData();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(745733841,"GoNextDialog",typeof(TopGame.UI.DialogPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.DialogPanel),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_GoNextDialog(TopGame.UI.DialogPanel pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GoNextDialog();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(401653215,"SetPlayerHud",typeof(TopGame.UI.DialogPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.DialogPanel),null)]
#endif
		public static bool AT_SetPlayerHud(TopGame.UI.DialogPanel pPointer)
		{
			pPointer.SetPlayerHud();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1754166831,"显示对话",typeof(TopGame.UI.DialogPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.DialogPanel),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"dialog",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"delay",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_ShowDialog(VariableInt dialog,VariableFloat delay,VariableBool pReturn=null)
		{
			if(dialog== null || delay== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.UI.DialogPanel.ShowDialog((uint)dialog.mValue,delay.mValue);
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 900571167:
				{//TopGame.UI.DialogPanel->GetCurDialogData
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.DialogPanel)) return true;
					return AgentTree_DialogPanel.AT_GetCurDialogData(pUserClass.mValue as TopGame.UI.DialogPanel, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 745733841:
				{//TopGame.UI.DialogPanel->GoNextDialog
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.DialogPanel)) return true;
					return AgentTree_DialogPanel.AT_GoNextDialog(pUserClass.mValue as TopGame.UI.DialogPanel, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 401653215:
				{//TopGame.UI.DialogPanel->SetPlayerHud
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.DialogPanel)) return true;
					return AgentTree_DialogPanel.AT_SetPlayerHud(pUserClass.mValue as TopGame.UI.DialogPanel);
				}
				case -1754166831:
				{//TopGame.UI.DialogPanel->ShowDialog
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_DialogPanel.AT_ShowDialog(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
			}
			return AgentTree_UIBase.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
