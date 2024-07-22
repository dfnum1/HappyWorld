//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("相机控制器",true)]
#endif
	public static class AgentTree_CameraController
	{
#if UNITY_EDITOR
		[ATMonoFunc(1873846443,"获取主相机",typeof(TopGame.Core.CameraController))]
		[ATMethodReturn(typeof(VariableMonoScript),"pReturn",typeof(UnityEngine.Camera),null)]
#endif
		public static bool AT_GetCamera(TopGame.Core.CameraController pPointer, VariableMonoScript pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCamera();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(937004037,"获取相机",typeof(TopGame.Core.CameraController))]
		[ATMethodArgv(typeof(VariableInt),"eType",false, null,typeof(Framework.Core.ECameraType))]
		[ATMethodReturn(typeof(VariableMonoScript),"pReturn",typeof(UnityEngine.Camera),null)]
#endif
		public static bool AT_GetCamera_1(TopGame.Core.CameraController pPointer, VariableInt eType, VariableMonoScript pReturn=null)
		{
			if(eType== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCamera((Framework.Core.ECameraType)eType.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-83054029,"设置相机广角",typeof(TopGame.Core.CameraController))]
		[ATMethodArgv(typeof(VariableFloat),"fFov",false, null,null)]
#endif
		public static bool AT_UpdateFov(TopGame.Core.CameraController pPointer, VariableFloat fFov)
		{
			if(fFov== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.UpdateFov(fFov.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1796073115,"切换模式",typeof(TopGame.Core.CameraController))]
		[ATMethodArgv(typeof(VariableString),"strMode",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bEnd",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.CameraMode),null)]
#endif
		public static bool AT_SwitchMode(TopGame.Core.CameraController pPointer, VariableString strMode, VariableBool bEnd, VariableUser pReturn=null)
		{
			if(strMode== null || bEnd== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.SwitchMode(strMode.mValue,bEnd.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1592752874,"获取当前模式",typeof(TopGame.Core.CameraController))]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.CameraMode),null)]
#endif
		public static bool AT_GetCurrentMode(TopGame.Core.CameraController pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentMode();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(62086956,"GetDir",typeof(TopGame.Core.CameraController))]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetDir(TopGame.Core.CameraController pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetDir();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1869553528,"GetRight",typeof(TopGame.Core.CameraController))]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetRight(TopGame.Core.CameraController pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetRight();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(462930481,"GetUp",typeof(TopGame.Core.CameraController))]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetUp(TopGame.Core.CameraController pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetUp();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-176104711,"GetPosition",typeof(TopGame.Core.CameraController))]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetPosition(TopGame.Core.CameraController pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetPosition();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1152016935,"GetEulerAngle",typeof(TopGame.Core.CameraController))]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetEulerAngle(TopGame.Core.CameraController pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetEulerAngle();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(846133601,"GetCurrentLookAt",typeof(TopGame.Core.CameraController))]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetCurrentLookAt(TopGame.Core.CameraController pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentLookAt();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(574897287,"GetCurrentFollowLookAt",typeof(TopGame.Core.CameraController))]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetCurrentFollowLookAt(TopGame.Core.CameraController pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentFollowLookAt();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1585642957,"StopAllEffect",typeof(TopGame.Core.CameraController))]
#endif
		public static bool AT_StopAllEffect(TopGame.Core.CameraController pPointer)
		{
			pPointer.StopAllEffect();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(181856445,"相机震动",typeof(TopGame.Core.CameraController))]
		[ATMethodArgv(typeof(VariableFloat),"fDuration",false, null,null)]
		[ATMethodArgv(typeof(VariableVector3),"vIntense",false, null,null)]
		[ATMethodArgv(typeof(VariableVector3),"vHertz",false, null,null)]
		[ATMethodArgv(typeof(VariableCurve),"damping",false, null,null)]
#endif
		public static bool AT_Shake(TopGame.Core.CameraController pPointer, VariableFloat fDuration, VariableVector3 vIntense, VariableVector3 vHertz, VariableCurve damping)
		{
			if(fDuration== null || vIntense== null || vHertz== null || damping== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.Shake(fDuration.mValue,vIntense.mValue,vHertz.mValue,damping.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1131141808,"镜头特写",typeof(TopGame.Core.CameraController))]
		[ATMethodArgv(typeof(VariableObject),"pTarget",false, typeof(UnityEngine.Transform),null)]
		[ATMethodArgv(typeof(VariableVector3),"vDuration",false, null,null)]
		[ATMethodArgv(typeof(VariableVector3),"vLookat",false, null,null)]
		[ATMethodArgv(typeof(VariableVector3),"vTranslate",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fFov",false, null,null)]
#endif
		public static bool AT_CloseUp(TopGame.Core.CameraController pPointer, VariableObject pTarget, VariableVector3 vDuration, VariableVector3 vLookat, VariableVector3 vTranslate, VariableFloat fFov)
		{
			if(pTarget== null || vDuration== null || vLookat== null || vTranslate== null || fFov== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.CloseUp(pTarget.ToObject<UnityEngine.Transform>(),vDuration.mValue,vLookat.mValue,vTranslate.mValue,fFov.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-577737157,"播放路径动画",typeof(TopGame.Core.CameraController))]
		[ATMethodArgv(typeof(VariableInt),"nId",false, null,null)]
		[ATMethodArgv(typeof(VariableObject),"pTrigger",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodArgv(typeof(VariableBool),"bAbs",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"offsetX",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"offsetY",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"offsetZ",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Core.IPlayableBase),null)]
#endif
		public static bool AT_PlayAniPath(TopGame.Core.CameraController pPointer, VariableInt nId, VariableObject pTrigger, VariableBool bAbs, VariableFloat offsetX, VariableFloat offsetY, VariableFloat offsetZ, VariableUser pReturn=null)
		{
			if(nId== null || pTrigger== null || bAbs== null || offsetX== null || offsetY== null || offsetZ== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.PlayAniPath(nId.mValue,pTrigger.ToObject<UnityEngine.GameObject>(),bAbs.mValue,offsetX.mValue,offsetY.mValue,offsetZ.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(653244172,"点是否在视野内",typeof(TopGame.Core.CameraController))]
		[ATMethodArgv(typeof(VariableVector3),"pos",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"factor",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsInView(TopGame.Core.CameraController pPointer, VariableVector3 pos, VariableFloat factor, VariableBool pReturn=null)
		{
			if(pos== null || factor== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsInView(pos.mValue,factor.mValue);
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1873846443:
				{//TopGame.Core.CameraController->GetCamera
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_GetCamera(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(0, pTask));
				}
				case 937004037:
				{//TopGame.Core.CameraController->GetCamera_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_GetCamera_1(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(0, pTask));
				}
				case -83054029:
				{//TopGame.Core.CameraController->UpdateFov
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_UpdateFov(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case 1796073115:
				{//TopGame.Core.CameraController->SwitchMode
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_SwitchMode(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -1592752874:
				{//TopGame.Core.CameraController->GetCurrentMode
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_GetCurrentMode(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 62086956:
				{//TopGame.Core.CameraController->GetDir
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_GetDir(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 1869553528:
				{//TopGame.Core.CameraController->GetRight
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_GetRight(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 462930481:
				{//TopGame.Core.CameraController->GetUp
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_GetUp(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -176104711:
				{//TopGame.Core.CameraController->GetPosition
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_GetPosition(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 1152016935:
				{//TopGame.Core.CameraController->GetEulerAngle
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_GetEulerAngle(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 846133601:
				{//TopGame.Core.CameraController->GetCurrentLookAt
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_GetCurrentLookAt(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 574897287:
				{//TopGame.Core.CameraController->GetCurrentFollowLookAt
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_GetCurrentFollowLookAt(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 1585642957:
				{//TopGame.Core.CameraController->StopAllEffect
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_StopAllEffect(pUserClass.mValue as TopGame.Core.CameraController);
				}
				case 181856445:
				{//TopGame.Core.CameraController->Shake
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_Shake(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableCurve>(4, pTask));
				}
				case -1131141808:
				{//TopGame.Core.CameraController->CloseUp
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_CloseUp(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(5, pTask));
				}
				case -577737157:
				{//TopGame.Core.CameraController->PlayAniPath
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=6) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_PlayAniPath(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(6, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 653244172:
				{//TopGame.Core.CameraController->IsInView
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Core.CameraController)) return true;
					return AgentTree_CameraController.AT_IsInView(pUserClass.mValue as TopGame.Core.CameraController, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
			}
			return true;
		}
	}
}
