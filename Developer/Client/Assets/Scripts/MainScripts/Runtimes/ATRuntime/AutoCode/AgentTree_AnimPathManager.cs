//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("路径动画系统",true)]
#endif
	public static class AgentTree_AnimPathManager
	{
#if UNITY_EDITOR
		[ATMonoFunc(955617704,"暂停/继续播放路径动画",typeof(TopGame.Core.AnimPathManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.AnimPathManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableBool),"bPause",false, null,null)]
#endif
		public static bool AT_PausePlay(TopGame.Core.AnimPathManager pPointer, VariableBool bPause)
		{
			if(bPause== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.PausePlay(bPause.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1665165138,"执行跳过事件",typeof(TopGame.Core.AnimPathManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.AnimPathManager),null, "", false, -1, true,false)]
#endif
		public static bool AT_DoSkipEvent(TopGame.Core.AnimPathManager pPointer)
		{
			pPointer.DoSkipEvent();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1072198510,"轨道绑定",typeof(TopGame.Core.AnimPathManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.AnimPathManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strTrackName",false, null,null)]
		[ATMethodArgv(typeof(VariableMonoScript),"pObject",false, typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableUser),"pUserData",false, typeof(Framework.Plugin.AT.IUserData),null)]
		[ATMethodArgv(typeof(VariableInt),"pathID",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bGenericBinding",false, null,null)]
#endif
		public static bool AT_TrackBind(TopGame.Core.AnimPathManager pPointer, VariableString strTrackName, VariableMonoScript pObject, VariableUser pUserData, VariableInt pathID, VariableBool bGenericBinding)
		{
			if(strTrackName== null || pObject== null || pUserData== null || pathID== null || bGenericBinding== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.TrackBind(strTrackName.mValue,pObject.ToObject<Framework.Core.AInstanceAble>(),(Framework.Plugin.AT.IUserData)pUserData.mValue,pathID.mValue,bGenericBinding.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-183764283,"是否已经绑定轨道上",typeof(TopGame.Core.AnimPathManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.AnimPathManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strTrackName",false, null,null)]
		[ATMethodArgv(typeof(VariableMonoScript),"pObject",false, typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableInt),"pathID",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_HadBindTrack(TopGame.Core.AnimPathManager pPointer, VariableString strTrackName, VariableMonoScript pObject, VariableInt pathID, VariableBool pReturn=null)
		{
			if(strTrackName== null || pObject== null || pathID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.HadBindTrack(strTrackName.mValue,pObject.ToObject<Framework.Core.AInstanceAble>(),pathID.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(220258364,"是否已经绑定轨道上-UserData",typeof(TopGame.Core.AnimPathManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.AnimPathManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strTrackName",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"pUserData",false, typeof(Framework.Plugin.AT.IUserData),null)]
		[ATMethodArgv(typeof(VariableInt),"pathID",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_HadBindTrackByUserData(TopGame.Core.AnimPathManager pPointer, VariableString strTrackName, VariableUser pUserData, VariableInt pathID, VariableBool pReturn=null)
		{
			if(strTrackName== null || pUserData== null || pathID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.HadBindTrackByUserData(strTrackName.mValue,(Framework.Plugin.AT.IUserData)pUserData.mValue,pathID.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(91311908,"跳过指定位置播放",typeof(TopGame.Core.AnimPathManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.AnimPathManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableFloat),"fSkipTime",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"pathID",false, null,null)]
#endif
		public static bool AT_SkipTo(TopGame.Core.AnimPathManager pPointer, VariableFloat fSkipTime, VariableInt pathID)
		{
			if(fSkipTime== null || pathID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SkipTo(fSkipTime.mValue,pathID.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1514673165,"事件开关(-1为全部)",typeof(TopGame.Core.AnimPathManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.AnimPathManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableBool),"bEvnet",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"pathId",false, null,null)]
#endif
		public static bool AT_EnableEvent(TopGame.Core.AnimPathManager pPointer, VariableBool bEvnet, VariableInt pathId)
		{
			if(bEvnet== null || pathId== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.EnableEvent(bEvnet.mValue,pathId.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1125653384,"停止路径动画(-1为全部)",typeof(TopGame.Core.AnimPathManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.AnimPathManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"pathID",false, null,null)]
#endif
		public static bool AT_Stop(TopGame.Core.AnimPathManager pPointer, VariableInt pathID)
		{
			if(pathID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.Stop(pathID.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1930804528,"是否正在播放镜头动画",typeof(TopGame.Core.AnimPathManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.AnimPathManager),null, "", false, -1, true,false)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsPlaying(TopGame.Core.AnimPathManager pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsPlaying();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(852744778,"是否播放路径动画",typeof(TopGame.Core.AnimPathManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Core.AnimPathManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"pathID",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsPlaying_1(TopGame.Core.AnimPathManager pPointer, VariableInt pathID, VariableBool pReturn=null)
		{
			if(pathID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsPlaying(pathID.mValue);
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 955617704:
				{//TopGame.Core.AnimPathManager->PausePlay
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().animPather;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathManager)) return true;
					return AgentTree_AnimPathManager.AT_PausePlay(pUserClass.mValue as TopGame.Core.AnimPathManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
				case -1665165138:
				{//TopGame.Core.AnimPathManager->DoSkipEvent
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().animPather;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathManager)) return true;
					return AgentTree_AnimPathManager.AT_DoSkipEvent(pUserClass.mValue as TopGame.Core.AnimPathManager);
				}
				case 1072198510:
				{//TopGame.Core.AnimPathManager->TrackBind
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().animPather;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathManager)) return true;
					return AgentTree_AnimPathManager.AT_TrackBind(pUserClass.mValue as TopGame.Core.AnimPathManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask));
				}
				case -183764283:
				{//TopGame.Core.AnimPathManager->HadBindTrack
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().animPather;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathManager)) return true;
					return AgentTree_AnimPathManager.AT_HadBindTrack(pUserClass.mValue as TopGame.Core.AnimPathManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 220258364:
				{//TopGame.Core.AnimPathManager->HadBindTrackByUserData
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().animPather;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathManager)) return true;
					return AgentTree_AnimPathManager.AT_HadBindTrackByUserData(pUserClass.mValue as TopGame.Core.AnimPathManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 91311908:
				{//TopGame.Core.AnimPathManager->SkipTo
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().animPather;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathManager)) return true;
					return AgentTree_AnimPathManager.AT_SkipTo(pUserClass.mValue as TopGame.Core.AnimPathManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask));
				}
				case -1514673165:
				{//TopGame.Core.AnimPathManager->EnableEvent
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().animPather;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathManager)) return true;
					return AgentTree_AnimPathManager.AT_EnableEvent(pUserClass.mValue as TopGame.Core.AnimPathManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask));
				}
				case 1125653384:
				{//TopGame.Core.AnimPathManager->Stop
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().animPather;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathManager)) return true;
					return AgentTree_AnimPathManager.AT_Stop(pUserClass.mValue as TopGame.Core.AnimPathManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 1930804528:
				{//TopGame.Core.AnimPathManager->IsPlaying
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().animPather;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathManager)) return true;
					return AgentTree_AnimPathManager.AT_IsPlaying(pUserClass.mValue as TopGame.Core.AnimPathManager, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 852744778:
				{//TopGame.Core.AnimPathManager->IsPlaying_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().animPather;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathManager)) return true;
					return AgentTree_AnimPathManager.AT_IsPlaying_1(pUserClass.mValue as TopGame.Core.AnimPathManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
			}
			return true;
		}
	}
}
