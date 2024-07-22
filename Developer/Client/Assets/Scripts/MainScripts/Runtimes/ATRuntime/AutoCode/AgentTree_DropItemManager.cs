//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("掉落系统",true)]
#endif
	public static class AgentTree_DropItemManager
	{
#if UNITY_EDITOR
		[ATMonoFunc(539189205,"添加掉落物",typeof(TopGame.Core.DropItemManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.DropItemManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"eType",false, null,typeof(Framework.Core.EActorType))]
		[ATMethodArgv(typeof(VariableInt),"nId",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"nDropId",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"nCount",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"effectID",false, null,null)]
#endif
		public static bool AT_AddDrop(TopGame.Core.DropItemManager pPointer, VariableInt eType, VariableInt nId, VariableInt nDropId, VariableInt nCount, VariableInt effectID)
		{
			if(eType== null || nId== null || nDropId== null || nCount== null || effectID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.AddDrop((Framework.Core.EActorType)eType.mValue,nId.mValue,(uint)nDropId.mValue,(uint)nCount.mValue,effectID.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 539189205:
				{//TopGame.Core.DropItemManager->AddDrop
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().dropManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(!(pUserClass.mValue is TopGame.Core.DropItemManager)) return true;
					return AgentTree_DropItemManager.AT_AddDrop(pUserClass.mValue as TopGame.Core.DropItemManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(5, pTask));
				}
			}
			return true;
		}
	}
}
