//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("配置数据",true)]
#endif
	public static class AgentTree_DataManager
	{
#if UNITY_EDITOR
		[ATMonoFunc(1900266958,"get_Dialog",typeof(TopGame.Data.DataManager), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Data.DataManager),null, "", false, -1, true,false)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Data.CsvData_Dialog),null)]
#endif
		public static bool AT_get_Dialog(TopGame.Data.DataManager pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.Dialog;
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1900266958:
				{//TopGame.Data.DataManager->Dialog
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.Data.DataManager.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.DataManager)) return true;
					return AgentTree_DataManager.AT_get_Dialog(pUserClass.mValue as TopGame.Data.DataManager, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
			}
			return true;
		}
	}
}
