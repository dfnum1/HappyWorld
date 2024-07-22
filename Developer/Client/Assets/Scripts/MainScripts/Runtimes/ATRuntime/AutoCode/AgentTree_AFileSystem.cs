//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("文件资源系统",true)]
#endif
	public static class AgentTree_AFileSystem
	{
#if UNITY_EDITOR
		[ATMonoFunc(1473547718,"清理预加载内容",typeof(Framework.Core.AFileSystem))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(Framework.Core.AFileSystem),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableFloat),"fDelay",false, null,null)]
#endif
		public static bool AT_ClearAllPreSpawn(Framework.Core.AFileSystem pPointer, VariableFloat fDelay)
		{
			if(fDelay== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ClearAllPreSpawn(fDelay.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1441835282,"预实例化池当前个数-(File)",typeof(Framework.Core.AFileSystem))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(Framework.Core.AFileSystem),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.GameObject))]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetPreSpawnStats(Framework.Core.AFileSystem pPointer, VariableString strFile, VariableInt pReturn=null)
		{
			if(strFile== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetPreSpawnStats(strFile.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(543942497,"预实例化池当前个数",typeof(Framework.Core.AFileSystem))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(Framework.Core.AFileSystem),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"pPrefab",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetPreSpawnStats_1(Framework.Core.AFileSystem pPointer, VariableObject pPrefab, VariableInt pReturn=null)
		{
			if(pPrefab== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetPreSpawnStats(pPrefab.ToObject<UnityEngine.GameObject>());
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(418262539,"预实例化对象-(File)",typeof(Framework.Core.AFileSystem))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(Framework.Core.AFileSystem),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.GameObject))]
		[ATMethodArgv(typeof(VariableBool),"bAsync",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bFrontQueue",false, null,null)]
#endif
		public static bool AT_PreSpawnInstance(Framework.Core.AFileSystem pPointer, VariableString strFile, VariableBool bAsync, VariableBool bFrontQueue)
		{
			if(strFile== null || bAsync== null || bFrontQueue== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.PreSpawnInstance(strFile.mValue,bAsync.mValue,bFrontQueue.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1846438530,"预实例化对象",typeof(Framework.Core.AFileSystem))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(Framework.Core.AFileSystem),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"pPrefab",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodArgv(typeof(VariableBool),"bFrontQueue",false, null,null)]
#endif
		public static bool AT_PreSpawnInstance_1(Framework.Core.AFileSystem pPointer, VariableObject pPrefab, VariableBool bFrontQueue)
		{
			if(pPrefab== null || bFrontQueue== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.PreSpawnInstance(pPrefab.ToObject<UnityEngine.GameObject>(),bFrontQueue.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1559568525,"实例化对象-(File)",typeof(Framework.Core.AFileSystem))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(Framework.Core.AFileSystem),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.GameObject))]
		[ATMethodArgv(typeof(VariableBool),"bAsync",false, null,null)]
#endif
		public static bool AT_SpawnInstance(Framework.Core.AFileSystem pPointer, VariableString strFile, VariableBool bAsync)
		{
			if(strFile== null || bAsync== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SpawnInstance(strFile.mValue,bAsync.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1671274047,"实例化对象",typeof(Framework.Core.AFileSystem))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(Framework.Core.AFileSystem),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"pAsset",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodArgv(typeof(VariableBool),"bAsync",false, null,null)]
#endif
		public static bool AT_SpawnInstance_1(Framework.Core.AFileSystem pPointer, VariableObject pAsset, VariableBool bAsync)
		{
			if(pAsset== null || bAsync== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SpawnInstance(pAsset.ToObject<UnityEngine.GameObject>(),bAsync.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1473547718:
				{//Framework.Core.AFileSystem->ClearAllPreSpawn
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = Framework.Core.FileSystemUtil.GetFileSystem();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.AFileSystem)) return true;
					return AgentTree_AFileSystem.AT_ClearAllPreSpawn(pUserClass.mValue as Framework.Core.AFileSystem, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case -1441835282:
				{//Framework.Core.AFileSystem->GetPreSpawnStats
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = Framework.Core.FileSystemUtil.GetFileSystem();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.AFileSystem)) return true;
					return AgentTree_AFileSystem.AT_GetPreSpawnStats(pUserClass.mValue as Framework.Core.AFileSystem, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 543942497:
				{//Framework.Core.AFileSystem->GetPreSpawnStats_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = Framework.Core.FileSystemUtil.GetFileSystem();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.AFileSystem)) return true;
					return AgentTree_AFileSystem.AT_GetPreSpawnStats_1(pUserClass.mValue as Framework.Core.AFileSystem, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 418262539:
				{//Framework.Core.AFileSystem->PreSpawnInstance
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = Framework.Core.FileSystemUtil.GetFileSystem();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(!(pUserClass.mValue is Framework.Core.AFileSystem)) return true;
					return AgentTree_AFileSystem.AT_PreSpawnInstance(pUserClass.mValue as Framework.Core.AFileSystem, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask));
				}
				case 1846438530:
				{//Framework.Core.AFileSystem->PreSpawnInstance_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = Framework.Core.FileSystemUtil.GetFileSystem();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.AFileSystem)) return true;
					return AgentTree_AFileSystem.AT_PreSpawnInstance_1(pUserClass.mValue as Framework.Core.AFileSystem, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case -1559568525:
				{//Framework.Core.AFileSystem->SpawnInstance
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = Framework.Core.FileSystemUtil.GetFileSystem();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.AFileSystem)) return true;
					return AgentTree_AFileSystem.AT_SpawnInstance(pUserClass.mValue as Framework.Core.AFileSystem, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case -1671274047:
				{//Framework.Core.AFileSystem->SpawnInstance_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = Framework.Core.FileSystemUtil.GetFileSystem();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.AFileSystem)) return true;
					return AgentTree_AFileSystem.AT_SpawnInstance_1(pUserClass.mValue as Framework.Core.AFileSystem, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
			}
			return true;
		}
	}
}
