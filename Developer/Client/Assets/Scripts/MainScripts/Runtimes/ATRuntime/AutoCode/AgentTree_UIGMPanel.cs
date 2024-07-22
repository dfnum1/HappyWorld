//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI系统/GM面板/UIBase",true)]
#endif
	public static class AgentTree_UIGMPanel
	{
#if UNITY_EDITOR
		[ATMonoFunc(-413614972,"添加随机装备",typeof(TopGame.UI.UIGMPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIGMPanel),null)]
#endif
		public static bool AT_OnClickAddEquip(TopGame.UI.UIGMPanel pPointer)
		{
			pPointer.OnClickAddEquip();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2043298343,"添加随机元素装备",typeof(TopGame.UI.UIGMPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIGMPanel),null)]
#endif
		public static bool AT_OnClickAddElementEquip(TopGame.UI.UIGMPanel pPointer)
		{
			pPointer.OnClickAddElementEquip();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1736797148,"输入代码提交",typeof(TopGame.UI.UIGMPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIGMPanel),null)]
		[ATMethodArgv(typeof(VariableString),"code",false, null,null)]
#endif
		public static bool AT_OnCommitInputCode(TopGame.UI.UIGMPanel pPointer, VariableString code)
		{
			if(code== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.OnCommitInputCode(code.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -413614972:
				{//TopGame.UI.UIGMPanel->OnClickAddEquip
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIGMPanel)) return true;
					return AgentTree_UIGMPanel.AT_OnClickAddEquip(pUserClass.mValue as TopGame.UI.UIGMPanel);
				}
				case -2043298343:
				{//TopGame.UI.UIGMPanel->OnClickAddElementEquip
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIGMPanel)) return true;
					return AgentTree_UIGMPanel.AT_OnClickAddElementEquip(pUserClass.mValue as TopGame.UI.UIGMPanel);
				}
				case 1736797148:
				{//TopGame.UI.UIGMPanel->OnCommitInputCode
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIGMPanel)) return true;
					return AgentTree_UIGMPanel.AT_OnCommitInputCode(pUserClass.mValue as TopGame.UI.UIGMPanel, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask));
				}
			}
			return AgentTree_UIBase.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
