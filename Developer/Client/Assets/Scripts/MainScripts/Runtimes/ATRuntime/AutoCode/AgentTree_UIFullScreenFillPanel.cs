//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI系统/全屏填充界面/界面",true)]
#endif
	public static class AgentTree_UIFullScreenFillPanel
	{
#if UNITY_EDITOR
		[ATMonoFunc(1294942679,"设置渐变颜色",typeof(TopGame.UI.UIFullScreenFillPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIFullScreenFillPanel),null)]
		[ATMethodArgv(typeof(VariableColor),"color",false, null,null)]
#endif
		public static bool AT_SetFillColor(TopGame.UI.UIFullScreenFillPanel pPointer, VariableColor color)
		{
			if(color== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetFillColor(color.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(566349986,"开始渐变",typeof(TopGame.UI.UIFullScreenFillPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIFullScreenFillPanel),null)]
#endif
		public static bool AT_SetBeginTween(TopGame.UI.UIFullScreenFillPanel pPointer)
		{
			pPointer.SetBeginTween();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(205230711,"开始渐变(指定时间)",typeof(TopGame.UI.UIFullScreenFillPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIFullScreenFillPanel),null)]
		[ATMethodArgv(typeof(VariableFloat),"time",false, null,null)]
#endif
		public static bool AT_SetBeginTween_1(TopGame.UI.UIFullScreenFillPanel pPointer, VariableFloat time)
		{
			if(time== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetBeginTween(time.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-978133061,"结束渐变",typeof(TopGame.UI.UIFullScreenFillPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIFullScreenFillPanel),null)]
#endif
		public static bool AT_SetEndTween(TopGame.UI.UIFullScreenFillPanel pPointer)
		{
			pPointer.SetEndTween();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-916072722,"结束渐变(指定时间)",typeof(TopGame.UI.UIFullScreenFillPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIFullScreenFillPanel),null)]
		[ATMethodArgv(typeof(VariableFloat),"time",false, null,null)]
#endif
		public static bool AT_SetEndTween_1(TopGame.UI.UIFullScreenFillPanel pPointer, VariableFloat time)
		{
			if(time== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetEndTween(time.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(2065444057,"获取当前状态",typeof(TopGame.UI.UIFullScreenFillPanel))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIFullScreenFillPanel),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(TopGame.UI.UIFullScreenFillPanel.State))]
#endif
		public static bool AT_GetState(TopGame.UI.UIFullScreenFillPanel pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetState();
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1294942679:
				{//TopGame.UI.UIFullScreenFillPanel->SetFillColor
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIFullScreenFillPanel)) return true;
					return AgentTree_UIFullScreenFillPanel.AT_SetFillColor(pUserClass.mValue as TopGame.UI.UIFullScreenFillPanel, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(1, pTask));
				}
				case 566349986:
				{//TopGame.UI.UIFullScreenFillPanel->SetBeginTween
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIFullScreenFillPanel)) return true;
					return AgentTree_UIFullScreenFillPanel.AT_SetBeginTween(pUserClass.mValue as TopGame.UI.UIFullScreenFillPanel);
				}
				case 205230711:
				{//TopGame.UI.UIFullScreenFillPanel->SetBeginTween_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIFullScreenFillPanel)) return true;
					return AgentTree_UIFullScreenFillPanel.AT_SetBeginTween_1(pUserClass.mValue as TopGame.UI.UIFullScreenFillPanel, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case -978133061:
				{//TopGame.UI.UIFullScreenFillPanel->SetEndTween
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIFullScreenFillPanel)) return true;
					return AgentTree_UIFullScreenFillPanel.AT_SetEndTween(pUserClass.mValue as TopGame.UI.UIFullScreenFillPanel);
				}
				case -916072722:
				{//TopGame.UI.UIFullScreenFillPanel->SetEndTween_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIFullScreenFillPanel)) return true;
					return AgentTree_UIFullScreenFillPanel.AT_SetEndTween_1(pUserClass.mValue as TopGame.UI.UIFullScreenFillPanel, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case 2065444057:
				{//TopGame.UI.UIFullScreenFillPanel->GetState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIFullScreenFillPanel)) return true;
					return AgentTree_UIFullScreenFillPanel.AT_GetState(pUserClass.mValue as TopGame.UI.UIFullScreenFillPanel, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
			}
			return AgentTree_UIBase.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
