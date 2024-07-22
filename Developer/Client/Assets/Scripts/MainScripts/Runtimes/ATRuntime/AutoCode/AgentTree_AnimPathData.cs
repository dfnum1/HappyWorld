//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("路径动画系统/动画剪辑",true)]
#endif
	public static class AgentTree_AnimPathData
	{
#if UNITY_EDITOR
		[ATMonoFunc(1234380764,"获取相机",typeof(TopGame.Core.AnimPathData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.AnimPathData),null)]
		[ATMethodReturn(typeof(VariableMonoScript),"pReturn",typeof(UnityEngine.Camera),null)]
#endif
		public static bool AT_GetCamera(TopGame.Core.AnimPathData pPointer, VariableMonoScript pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCamera();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1785411771,"获取绑点",typeof(TopGame.Core.AnimPathData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.AnimPathData),null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.Transform),null)]
#endif
		public static bool AT_GetSlot(TopGame.Core.AnimPathData pPointer, VariableInt index, VariableObject pReturn=null)
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
		[ATMonoFunc(1895479964,"获取绑点-Name",typeof(TopGame.Core.AnimPathData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.AnimPathData),null)]
		[ATMethodArgv(typeof(VariableString),"strName",false, null,null)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.Transform),null)]
#endif
		public static bool AT_GetSlot_1(TopGame.Core.AnimPathData pPointer, VariableString strName, VariableObject pReturn=null)
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
		[ATMonoFunc(373338554,"绑定轨道对象数据",typeof(TopGame.Core.AnimPathData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.AnimPathData),null)]
		[ATMethodArgv(typeof(VariableString),"trackName",false, null,null)]
		[ATMethodArgv(typeof(VariableMonoScript),"pObj",false, typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableBool),"bGenericBinding",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"pAT",false, typeof(Framework.Plugin.AT.IUserData),null)]
#endif
		public static bool AT_BindTrackObject(TopGame.Core.AnimPathData pPointer, VariableString trackName, VariableMonoScript pObj, VariableBool bGenericBinding, VariableUser pAT)
		{
			if(trackName== null || pObj== null || bGenericBinding== null || pAT== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.BindTrackObject(trackName.mValue,pObj.ToObject<Framework.Core.AInstanceAble>(),bGenericBinding.mValue,(Framework.Plugin.AT.IUserData)pAT.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1922817012,"当前位置(GetPosition)",typeof(TopGame.Core.AnimPathData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.AnimPathData),null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetPosition(TopGame.Core.AnimPathData pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetPosition();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2046579737,"当前角度(GetEulerAngle)",typeof(TopGame.Core.AnimPathData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.AnimPathData),null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetEulerAngle(TopGame.Core.AnimPathData pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetEulerAngle();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(430030540,"当前视点(GetLookAt)",typeof(TopGame.Core.AnimPathData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.AnimPathData),null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetLookAt(TopGame.Core.AnimPathData pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetLookAt();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1934461359,"当前广角(GetFov)",typeof(TopGame.Core.AnimPathData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.AnimPathData),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetFov(TopGame.Core.AnimPathData pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetFov();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-606313989,"跳指定位置开始播放(SkipTo)",typeof(TopGame.Core.AnimPathData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.AnimPathData),null)]
		[ATMethodArgv(typeof(VariableFloat),"fSkipTime",false, null,null)]
#endif
		public static bool AT_SkipTo(TopGame.Core.AnimPathData pPointer, VariableFloat fSkipTime)
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
		[ATMonoFunc(-1945101661,"跳位置播放(SkipDo)",typeof(TopGame.Core.AnimPathData))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.Core.AnimPathData),null)]
#endif
		public static bool AT_SkipDo(TopGame.Core.AnimPathData pPointer)
		{
			pPointer.SkipDo();
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1234380764:
				{//TopGame.Core.AnimPathData->GetCamera
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathData)) return true;
					return AgentTree_AnimPathData.AT_GetCamera(pUserClass.mValue as TopGame.Core.AnimPathData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(0, pTask));
				}
				case -1785411771:
				{//TopGame.Core.AnimPathData->GetSlot
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathData)) return true;
					return AgentTree_AnimPathData.AT_GetSlot(pUserClass.mValue as TopGame.Core.AnimPathData, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case 1895479964:
				{//TopGame.Core.AnimPathData->GetSlot_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathData)) return true;
					return AgentTree_AnimPathData.AT_GetSlot_1(pUserClass.mValue as TopGame.Core.AnimPathData, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case 373338554:
				{//TopGame.Core.AnimPathData->BindTrackObject
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathData)) return true;
					return AgentTree_AnimPathData.AT_BindTrackObject(pUserClass.mValue as TopGame.Core.AnimPathData, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(4, pTask));
				}
				case 1922817012:
				{//TopGame.Core.AnimPathData->GetPosition
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathData)) return true;
					return AgentTree_AnimPathData.AT_GetPosition(pUserClass.mValue as TopGame.Core.AnimPathData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -2046579737:
				{//TopGame.Core.AnimPathData->GetEulerAngle
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathData)) return true;
					return AgentTree_AnimPathData.AT_GetEulerAngle(pUserClass.mValue as TopGame.Core.AnimPathData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 430030540:
				{//TopGame.Core.AnimPathData->GetLookAt
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathData)) return true;
					return AgentTree_AnimPathData.AT_GetLookAt(pUserClass.mValue as TopGame.Core.AnimPathData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 1934461359:
				{//TopGame.Core.AnimPathData->GetFov
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathData)) return true;
					return AgentTree_AnimPathData.AT_GetFov(pUserClass.mValue as TopGame.Core.AnimPathData, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case -606313989:
				{//TopGame.Core.AnimPathData->SkipTo
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathData)) return true;
					return AgentTree_AnimPathData.AT_SkipTo(pUserClass.mValue as TopGame.Core.AnimPathData, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case -1945101661:
				{//TopGame.Core.AnimPathData->SkipDo
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.Core.AnimPathData)) return true;
					return AgentTree_AnimPathData.AT_SkipDo(pUserClass.mValue as TopGame.Core.AnimPathData);
				}
			}
			return true;
		}
	}
}
