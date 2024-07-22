//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("相机系统/基础模式",true)]
#endif
	public static class AgentTree_CameraMode
	{
#if UNITY_EDITOR
		[ATMonoFunc(-1933908172,"启用启用",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableBool),"bTween",false, null,null)]
#endif
		public static bool AT_EnableTween(Framework.Core.CameraMode pPointer, VariableBool bTween)
		{
			if(bTween== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.EnableTween(bTween.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1546534097,"SetCurrentTrans",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableVector3),"vCurrentTrans",false, null,null)]
#endif
		public static bool AT_SetCurrentTrans(Framework.Core.CameraMode pPointer, VariableVector3 vCurrentTrans)
		{
			if(vCurrentTrans== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetCurrentTrans(vCurrentTrans.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1570325651,"SetCurrentTransOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableVector3),"vCurrentTrans",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetCurrentTransOffset(Framework.Core.CameraMode pPointer, VariableVector3 vCurrentTrans, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(vCurrentTrans== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetCurrentTransOffset(vCurrentTrans.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1256346679,"GetCurrentTransOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableBool),"bFinal",false, null,null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetCurrentTransOffset(Framework.Core.CameraMode pPointer, VariableBool bFinal, VariableVector3 pReturn=null)
		{
			if(bFinal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentTransOffset(bFinal.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1527417033,"SetCurrentEulerAngle",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableVector3),"vEulerAngle",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetCurrentEulerAngle(Framework.Core.CameraMode pPointer, VariableVector3 vEulerAngle, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(vEulerAngle== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetCurrentEulerAngle(vEulerAngle.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1270046768,"SetCurrentUp",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableVector3),"vUp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetCurrentUp(Framework.Core.CameraMode pPointer, VariableVector3 vUp, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(vUp== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetCurrentUp(vUp.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2010337506,"SetCurrentFov",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableFloat),"fFov",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetCurrentFov(Framework.Core.CameraMode pPointer, VariableFloat fFov, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(fFov== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetCurrentFov(fFov.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1446188677,"GetCurrentFov",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableBool),"bFinal",false, null,null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetCurrentFov(Framework.Core.CameraMode pPointer, VariableBool bFinal, VariableFloat pReturn=null)
		{
			if(bFinal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentFov(bFinal.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(521301320,"GetCurrentTrans",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableBool),"bFinal",false, null,null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetCurrentTrans(Framework.Core.CameraMode pPointer, VariableBool bFinal, VariableVector3 pReturn=null)
		{
			if(bFinal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentTrans(bFinal.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1412126936,"GetCurrentTrans",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableBool),"bFinal",false, null,null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetCurrentLookAt(Framework.Core.CameraMode pPointer, VariableBool bFinal, VariableVector3 pReturn=null)
		{
			if(bFinal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentLookAt(bFinal.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-810966904,"GetCurrentLookAtOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableBool),"bFinal",false, null,null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetCurrentLookAtOffset(Framework.Core.CameraMode pPointer, VariableBool bFinal, VariableVector3 pReturn=null)
		{
			if(bFinal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentLookAtOffset(bFinal.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-528433498,"SetCurrentLookAtOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableVector3),"offset",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetCurrentLookAtOffset(Framework.Core.CameraMode pPointer, VariableVector3 offset, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(offset== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetCurrentLookAtOffset(offset.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1845877909,"GetCurrentEulerAngle",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableBool),"bExternAdd",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bFinal",false, null,null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetCurrentEulerAngle(Framework.Core.CameraMode pPointer, VariableBool bExternAdd, VariableBool bFinal, VariableVector3 pReturn=null)
		{
			if(bExternAdd== null || bFinal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentEulerAngle(bExternAdd.mValue,bFinal.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-608368519,"GetCurrentUp",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableBool),"bFinal",false, null,null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetCurrentUp(Framework.Core.CameraMode pPointer, VariableBool bFinal, VariableVector3 pReturn=null)
		{
			if(bFinal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentUp(bFinal.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1794590326,"SetLockCameraOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableVector3),"vOffset",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetLockCameraOffset(Framework.Core.CameraMode pPointer, VariableVector3 vOffset, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(vOffset== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetLockCameraOffset(vOffset.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(854309168,"GetLockCameraOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableBool),"bFinal",false, null,null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetLockCameraOffset(Framework.Core.CameraMode pPointer, VariableBool bFinal, VariableVector3 pReturn=null)
		{
			if(bFinal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetLockCameraOffset(bFinal.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-450170215,"SetLockCameraLookAtOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableVector3),"vOffset",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetLockCameraLookAtOffset(Framework.Core.CameraMode pPointer, VariableVector3 vOffset, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(vOffset== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetLockCameraLookAtOffset(vOffset.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1038271813,"GetLockCameraLookAtOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableBool),"bFinal",false, null,null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetLockCameraLookAtOffset(Framework.Core.CameraMode pPointer, VariableBool bFinal, VariableVector3 pReturn=null)
		{
			if(bFinal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetLockCameraLookAtOffset(bFinal.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(960115007,"SetLockEulerAngleOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableVector3),"vOffset",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetLockEulerAngleOffset(Framework.Core.CameraMode pPointer, VariableVector3 vOffset, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(vOffset== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetLockEulerAngleOffset(vOffset.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-175369712,"SetLockPitchOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableFloat),"fOffset",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetLockPitchOffset(Framework.Core.CameraMode pPointer, VariableFloat fOffset, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(fOffset== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetLockPitchOffset(fOffset.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1993292537,"SetLockYawOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableFloat),"fOffset",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetLockYawOffset(Framework.Core.CameraMode pPointer, VariableFloat fOffset, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(fOffset== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetLockYawOffset(fOffset.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-615199528,"SetLockRollOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableFloat),"fOffset",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetLockRollOffset(Framework.Core.CameraMode pPointer, VariableFloat fOffset, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(fOffset== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetLockRollOffset(fOffset.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1133485919,"GetLockEulerAngleOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetLockEulerAngleOffset(Framework.Core.CameraMode pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetLockEulerAngleOffset();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(336495130,"SetLockUpOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableVector3),"vUp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetLockUpOffset(Framework.Core.CameraMode pPointer, VariableVector3 vUp, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(vUp== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetLockUpOffset(vUp.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(717048848,"GetLockUpOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetLockUpOffset(Framework.Core.CameraMode pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetLockUpOffset();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-361570703,"SetLockFovOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableFloat),"fOffset",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetLockFovOffset(Framework.Core.CameraMode pPointer, VariableFloat fOffset, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(fOffset== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetLockFovOffset(fOffset.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1150305740,"GetLockFovOffset",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetLockFovOffset(Framework.Core.CameraMode pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetLockFovOffset();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(2036714775,"Start",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
#endif
		public static bool AT_Start(Framework.Core.CameraMode pPointer)
		{
			pPointer.Start();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2025916725,"End",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
#endif
		public static bool AT_End(Framework.Core.CameraMode pPointer)
		{
			pPointer.End();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-424039333,"GetLookAtActor",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.Actor),null)]
#endif
		public static bool AT_GetFollowLookAtActor(Framework.Core.CameraMode pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetFollowLookAtActor();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1475730809,"GetFollowLookAtPosition",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetFollowLookAtPosition(Framework.Core.CameraMode pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetFollowLookAtPosition();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-122089869,"SetFollowLookAtPosition",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableVector3),"pos",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bForce",false, null,null)]
#endif
		public static bool AT_SetFollowLookAtPosition(Framework.Core.CameraMode pPointer, VariableVector3 pos, VariableBool bForce)
		{
			if(pos== null || bForce== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetFollowLookAtPosition(pos.mValue,bForce.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1486491035,"SetFollowDistance",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableFloat),"fDistance",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bReMinMax",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
#endif
		public static bool AT_SetFollowDistance(Framework.Core.CameraMode pPointer, VariableFloat fDistance, VariableBool bReMinMax, VariableBool bAmount)
		{
			if(fDistance== null || bReMinMax== null || bAmount== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetFollowDistance(fDistance.mValue,bReMinMax.mValue,bAmount.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(127891292,"GetFollowDistance",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableBool),"bFinal",false, null,null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetFollowDistance(Framework.Core.CameraMode pPointer, VariableBool bFinal, VariableFloat pReturn=null)
		{
			if(bFinal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetFollowDistance(bFinal.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1303181960,"AppendFollowDistance",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableFloat),"fDistance",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bReMinMax",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bClamp",false, null,null)]
#endif
		public static bool AT_AppendFollowDistance(Framework.Core.CameraMode pPointer, VariableFloat fDistance, VariableBool bReMinMax, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong, VariableBool bClamp)
		{
			if(fDistance== null || bReMinMax== null || bAmount== null || fLerp== null || pingpong== null || bClamp== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.AppendFollowDistance(fDistance.mValue,bReMinMax.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue,bClamp.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1049004742,"SetLockOffsetDistance",typeof(Framework.Core.CameraMode))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.CameraMode),null)]
		[ATMethodArgv(typeof(VariableFloat),"fDistance",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"pingpong",false, null,null)]
#endif
		public static bool AT_SetLockOffsetDistance(Framework.Core.CameraMode pPointer, VariableFloat fDistance, VariableBool bAmount, VariableFloat fLerp, VariableBool pingpong)
		{
			if(fDistance== null || bAmount== null || fLerp== null || pingpong== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetLockOffsetDistance(fDistance.mValue,bAmount.mValue,fLerp.mValue,pingpong.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -1933908172:
				{//Framework.Core.CameraMode->EnableTween
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_EnableTween(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
				case 1546534097:
				{//Framework.Core.CameraMode->SetCurrentTrans
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetCurrentTrans(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask));
				}
				case 1570325651:
				{//Framework.Core.CameraMode->SetCurrentTransOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetCurrentTransOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case 1256346679:
				{//Framework.Core.CameraMode->GetCurrentTransOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetCurrentTransOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 1527417033:
				{//Framework.Core.CameraMode->SetCurrentEulerAngle
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetCurrentEulerAngle(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case 1270046768:
				{//Framework.Core.CameraMode->SetCurrentUp
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetCurrentUp(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case -2010337506:
				{//Framework.Core.CameraMode->SetCurrentFov
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetCurrentFov(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case -1446188677:
				{//Framework.Core.CameraMode->GetCurrentFov
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetCurrentFov(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case 521301320:
				{//Framework.Core.CameraMode->GetCurrentTrans
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetCurrentTrans(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 1412126936:
				{//Framework.Core.CameraMode->GetCurrentLookAt
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetCurrentLookAt(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -810966904:
				{//Framework.Core.CameraMode->GetCurrentLookAtOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetCurrentLookAtOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -528433498:
				{//Framework.Core.CameraMode->SetCurrentLookAtOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetCurrentLookAtOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case 1845877909:
				{//Framework.Core.CameraMode->GetCurrentEulerAngle
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetCurrentEulerAngle(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -608368519:
				{//Framework.Core.CameraMode->GetCurrentUp
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetCurrentUp(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -1794590326:
				{//Framework.Core.CameraMode->SetLockCameraOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetLockCameraOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case 854309168:
				{//Framework.Core.CameraMode->GetLockCameraOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetLockCameraOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -450170215:
				{//Framework.Core.CameraMode->SetLockCameraLookAtOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetLockCameraLookAtOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case 1038271813:
				{//Framework.Core.CameraMode->GetLockCameraLookAtOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetLockCameraLookAtOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 960115007:
				{//Framework.Core.CameraMode->SetLockEulerAngleOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetLockEulerAngleOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case -175369712:
				{//Framework.Core.CameraMode->SetLockPitchOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetLockPitchOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case 1993292537:
				{//Framework.Core.CameraMode->SetLockYawOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetLockYawOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case -615199528:
				{//Framework.Core.CameraMode->SetLockRollOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetLockRollOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case -1133485919:
				{//Framework.Core.CameraMode->GetLockEulerAngleOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetLockEulerAngleOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case 336495130:
				{//Framework.Core.CameraMode->SetLockUpOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetLockUpOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case 717048848:
				{//Framework.Core.CameraMode->GetLockUpOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetLockUpOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -361570703:
				{//Framework.Core.CameraMode->SetLockFovOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetLockFovOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
				case 1150305740:
				{//Framework.Core.CameraMode->GetLockFovOffset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetLockFovOffset(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case 2036714775:
				{//Framework.Core.CameraMode->Start
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_Start(pUserClass.mValue as Framework.Core.CameraMode);
				}
				case -2025916725:
				{//Framework.Core.CameraMode->End
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_End(pUserClass.mValue as Framework.Core.CameraMode);
				}
				case -424039333:
				{//Framework.Core.CameraMode->GetFollowLookAtActor
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetFollowLookAtActor(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 1475730809:
				{//Framework.Core.CameraMode->GetFollowLookAtPosition
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetFollowLookAtPosition(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -122089869:
				{//Framework.Core.CameraMode->SetFollowLookAtPosition
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetFollowLookAtPosition(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case -1486491035:
				{//Framework.Core.CameraMode->SetFollowDistance
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetFollowDistance(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask));
				}
				case 127891292:
				{//Framework.Core.CameraMode->GetFollowDistance
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_GetFollowDistance(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case -1303181960:
				{//Framework.Core.CameraMode->AppendFollowDistance
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=6) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_AppendFollowDistance(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(6, pTask));
				}
				case -1049004742:
				{//Framework.Core.CameraMode->SetLockOffsetDistance
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(!(pUserClass.mValue is Framework.Core.CameraMode)) return true;
					return AgentTree_CameraMode.AT_SetLockOffsetDistance(pUserClass.mValue as Framework.Core.CameraMode, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask));
				}
			}
			return true;
		}
	}
}
