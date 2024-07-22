//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("路径动画系统/Timeline",true)]
#endif
	public static class AgentTree_TimelineData
	{
#if UNITY_EDITOR
		[ATMonoFunc(-462637657,"当前位置(GetPosition)",typeof(TopGame.Core.TimelineData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.TimelineData),null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetPosition(TopGame.Core.TimelineData pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetPosition();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1113620520,"当前角度(GetEulerAngle)",typeof(TopGame.Core.TimelineData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.TimelineData),null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetEulerAngle(TopGame.Core.TimelineData pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetEulerAngle();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(191332948,"当前视点(GetLookAt)",typeof(TopGame.Core.TimelineData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.TimelineData),null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetLookAt(TopGame.Core.TimelineData pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetLookAt();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-755417953,"当前广角(GetFov)",typeof(TopGame.Core.TimelineData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.TimelineData),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetFov(TopGame.Core.TimelineData pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetFov();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(944172933,"获取相机",typeof(TopGame.Core.TimelineData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.TimelineData),null)]
		[ATMethodReturn(typeof(VariableMonoScript),"pReturn",typeof(UnityEngine.Camera),null)]
#endif
		public static bool AT_GetCamera(TopGame.Core.TimelineData pPointer, VariableMonoScript pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCamera();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(600405676,"获取绑点",typeof(TopGame.Core.TimelineData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.TimelineData),null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.Transform),null)]
#endif
		public static bool AT_GetSlot(TopGame.Core.TimelineData pPointer, VariableInt index, VariableObject pReturn=null)
		{
			if(index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetSlot(index.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1053680565,"获取绑点-Name",typeof(TopGame.Core.TimelineData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.TimelineData),null)]
		[ATMethodArgv(typeof(VariableString),"strName",false, null,null)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.Transform),null)]
#endif
		public static bool AT_GetSlot_1(TopGame.Core.TimelineData pPointer, VariableString strName, VariableObject pReturn=null)
		{
			if(strName== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetSlot(strName.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-921019037,"跳指定位置开始播放(SkipTo)",typeof(TopGame.Core.TimelineData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.TimelineData),null)]
		[ATMethodArgv(typeof(VariableFloat),"fSkipTime",false, null,null)]
#endif
		public static bool AT_SkipTo(TopGame.Core.TimelineData pPointer, VariableFloat fSkipTime)
		{
			if(fSkipTime== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SkipTo(fSkipTime.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(445087227,"跳位置播放(SkipDo)",typeof(TopGame.Core.TimelineData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.TimelineData),null)]
#endif
		public static bool AT_SkipDo(TopGame.Core.TimelineData pPointer)
		{
			pPointer.SkipDo();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1002257048,"绑定轨道对象数据",typeof(TopGame.Core.TimelineData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.TimelineData),null)]
		[ATMethodArgv(typeof(VariableString),"trackName",false, null,null)]
		[ATMethodArgv(typeof(VariableMonoScript),"pObj",false, typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableBool),"bGenericBinding",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"pAT",false, typeof(Framework.Plugin.AT.IUserData),null)]
#endif
		public static bool AT_BindTrackObject(TopGame.Core.TimelineData pPointer, VariableString trackName, VariableMonoScript pObj, VariableBool bGenericBinding, VariableUser pAT)
		{
			if(trackName== null || pObj== null || bGenericBinding== null || pAT== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.BindTrackObject(trackName.mValue,pObj.ToObject<Framework.Core.AInstanceAble>(),bGenericBinding.mValue,(Framework.Plugin.AT.IUserData)pAT.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -462637657:
				{//TopGame.Core.TimelineData->GetPosition
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.TimelineData)) return true;
					return AgentTree_TimelineData.AT_GetPosition(pUserClass.mValue as TopGame.Core.TimelineData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -1113620520:
				{//TopGame.Core.TimelineData->GetEulerAngle
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.TimelineData)) return true;
					return AgentTree_TimelineData.AT_GetEulerAngle(pUserClass.mValue as TopGame.Core.TimelineData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 191332948:
				{//TopGame.Core.TimelineData->GetLookAt
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.TimelineData)) return true;
					return AgentTree_TimelineData.AT_GetLookAt(pUserClass.mValue as TopGame.Core.TimelineData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -755417953:
				{//TopGame.Core.TimelineData->GetFov
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.TimelineData)) return true;
					return AgentTree_TimelineData.AT_GetFov(pUserClass.mValue as TopGame.Core.TimelineData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case 944172933:
				{//TopGame.Core.TimelineData->GetCamera
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.TimelineData)) return true;
					return AgentTree_TimelineData.AT_GetCamera(pUserClass.mValue as TopGame.Core.TimelineData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(0, pTask));
				}
				case 600405676:
				{//TopGame.Core.TimelineData->GetSlot
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.TimelineData)) return true;
					return AgentTree_TimelineData.AT_GetSlot(pUserClass.mValue as TopGame.Core.TimelineData, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case 1053680565:
				{//TopGame.Core.TimelineData->GetSlot_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.TimelineData)) return true;
					return AgentTree_TimelineData.AT_GetSlot_1(pUserClass.mValue as TopGame.Core.TimelineData, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case -921019037:
				{//TopGame.Core.TimelineData->SkipTo
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.Core.TimelineData)) return true;
					return AgentTree_TimelineData.AT_SkipTo(pUserClass.mValue as TopGame.Core.TimelineData, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case 445087227:
				{//TopGame.Core.TimelineData->SkipDo
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.Core.TimelineData)) return true;
					return AgentTree_TimelineData.AT_SkipDo(pUserClass.mValue as TopGame.Core.TimelineData);
				}
				case 1002257048:
				{//TopGame.Core.TimelineData->BindTrackObject
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is TopGame.Core.TimelineData)) return true;
					return AgentTree_TimelineData.AT_BindTrackObject(pUserClass.mValue as TopGame.Core.TimelineData, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(4, pTask));
				}
			}
			return true;
		}
	}
}
