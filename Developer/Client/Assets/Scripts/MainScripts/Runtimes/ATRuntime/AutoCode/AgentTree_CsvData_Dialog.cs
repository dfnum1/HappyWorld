//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("配置数据/Dialog",true)]
#endif
	public static class AgentTree_CsvData_Dialog
	{
#if UNITY_EDITOR
		[ATMonoFunc(1941524615,"查找数据",typeof(TopGame.Data.CsvData_Dialog))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Data.CsvData_Dialog),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"id",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Data.CsvData_Dialog.DialogData),null)]
#endif
		public static bool AT_GetData(TopGame.Data.CsvData_Dialog pPointer, VariableInt id, VariableUser pReturn=null)
		{
			if(id== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetData((uint)id.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1382775926,"get_datas",typeof(TopGame.Data.CsvData_Dialog), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Data.CsvData_Dialog),null, "", false, -1, true,false)]
#endif
		public static bool AT_get_datas(TopGame.Data.CsvData_Dialog pPointer)
		{
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1941524615:
				{//TopGame.Data.CsvData_Dialog->GetData
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.Data.DataManager.getInstance().Dialog;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog)) return true;
					return AgentTree_CsvData_Dialog.AT_GetData(pUserClass.mValue as TopGame.Data.CsvData_Dialog, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -1382775926:
				{//TopGame.Data.CsvData_Dialog->datas
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.Data.DataManager.getInstance().Dialog;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.Data.CsvData_Dialog)) return true;
					return AgentTree_CsvData_Dialog.AT_get_datas(pUserClass.mValue as TopGame.Data.CsvData_Dialog);
				}
			}
			return true;
		}
	}
}
