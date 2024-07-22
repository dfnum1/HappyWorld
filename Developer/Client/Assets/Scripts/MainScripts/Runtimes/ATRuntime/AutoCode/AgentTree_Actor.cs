//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("World/Actor",true)]
#endif
	public static class AgentTree_Actor
	{
#if UNITY_EDITOR
		[ATMonoFunc(636507301,"GetPropertyEffecter",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.PropertyEffecter),null)]
#endif
		public static bool AT_GetPropertyEffecter(Framework.Core.Actor pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetPropertyEffecter();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-403618621,"EnableSkill",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableBool),"bEnable",false, null,null)]
#endif
		public static bool AT_EnableSkill(Framework.Core.Actor pPointer, VariableBool bEnable)
		{
			if(bEnable== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.EnableSkill(bEnable.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1480893833,"GetLevelContextData",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.IContextData),null)]
#endif
		public static bool AT_GetLevelContextData(Framework.Core.Actor pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetLevelContextData();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1648251282,"GetQualityContextData",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.IContextData),null)]
#endif
		public static bool AT_GetQualityContextData(Framework.Core.Actor pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetQualityContextData();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(458799974,"GetTurnTime",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetTurnTime(Framework.Core.Actor pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetTurnTime();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1336095790,"SetDestrotyDelta",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableFloat),"fDelta",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
#endif
		public static bool AT_SetDestrotyDelta(Framework.Core.Actor pPointer, VariableFloat fDelta, VariableBool bAmount)
		{
			if(fDelta== null || bAmount== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetDestrotyDelta(fDelta.mValue,bAmount.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(131072779,"IsInvincible",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsInvincible(Framework.Core.Actor pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsInvincible();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1694231299,"SetInvincibleByDuration",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableFloat),"fDuration",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAbs",false, null,null)]
#endif
		public static bool AT_SetInvincibleByDuration(Framework.Core.Actor pPointer, VariableFloat fDuration, VariableBool bAbs)
		{
			if(fDuration== null || bAbs== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetInvincibleByDuration(fDuration.mValue,bAbs.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(46256047,"CanCancelState",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"toState",false, null,typeof(Framework.Core.EActionStateType))]
		[ATMethodArgv(typeof(VariableInt),"tag",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_CanCancelState(Framework.Core.Actor pPointer, VariableInt toState, VariableInt tag, VariableBool pReturn=null)
		{
			if(toState== null || tag== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CanCancelState((Framework.Core.EActionStateType)toState.mValue,(uint)tag.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2078301578,"CanCancelState_1",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableUser),"toState",false, typeof(Framework.Core.ActionState),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_CanCancelState_1(Framework.Core.Actor pPointer, VariableUser toState, VariableBool pReturn=null)
		{
			if(toState== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CanCancelState((Framework.Core.ActionState)toState.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1996489456,"ClearStateProperty",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
#endif
		public static bool AT_ClearStateProperty(Framework.Core.Actor pPointer)
		{
			pPointer.ClearStateProperty();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1649620682,"GetPetRiding",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.Actor),null)]
#endif
		public static bool AT_GetPetRiding(Framework.Core.Actor pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetPetRiding();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1072039686,"IsPetRide",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsPetRide(Framework.Core.Actor pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsPetRide();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-867953279,"ResetMomentum",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableBool),"bProperty",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bResetY",false, null,null)]
#endif
		public static bool AT_ResetMomentum(Framework.Core.Actor pPointer, VariableBool bProperty, VariableBool bResetY)
		{
			if(bProperty== null || bResetY== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ResetMomentum(bProperty.mValue,bResetY.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-267169915,"GetFraction",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetFraction(Framework.Core.Actor pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetFraction();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-99839765,"SetFraction",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableFloat),"fFraction",false, null,null)]
#endif
		public static bool AT_SetFraction(Framework.Core.Actor pPointer, VariableFloat fFraction)
		{
			if(fFraction== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetFraction(fFraction.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-204121276,"GetSpeed",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_GetSpeed(Framework.Core.Actor pPointer, VariableVector3 pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetSpeed();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2026832832,"SetSpeed",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableVector3),"vSpeed",false, null,null)]
#endif
		public static bool AT_SetSpeed(Framework.Core.Actor pPointer, VariableVector3 vSpeed)
		{
			if(vSpeed== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetSpeed(vSpeed.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1418438349,"SetSpeedXZ",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableVector3),"vSpeed",false, null,null)]
#endif
		public static bool AT_SetSpeedXZ(Framework.Core.Actor pPointer, VariableVector3 vSpeed)
		{
			if(vSpeed== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetSpeedXZ(vSpeed.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1888086346,"SetSpeedXZ_1",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableFloat),"fSpeedX",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fSpeedZ",false, null,null)]
#endif
		public static bool AT_SetSpeedXZ_1(Framework.Core.Actor pPointer, VariableFloat fSpeedX, VariableFloat fSpeedZ)
		{
			if(fSpeedX== null || fSpeedZ== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetSpeedXZ(fSpeedX.mValue,fSpeedZ.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1753493842,"SetSpeedY",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableFloat),"fSpeed",false, null,null)]
#endif
		public static bool AT_SetSpeedY(Framework.Core.Actor pPointer, VariableFloat fSpeed)
		{
			if(fSpeed== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetSpeedY(fSpeed.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1554835646,"GetJumpHorizenSpeed",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetJumpHorizenSpeed(Framework.Core.Actor pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetJumpHorizenSpeed();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(774670985,"GetJumpVerticalSpeed",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetJumpVerticalSpeed(Framework.Core.Actor pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetJumpVerticalSpeed();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1605685122,"GetFallingSpeed",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetFallingSpeed(Framework.Core.Actor pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetFallingSpeed();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(579145607,"GetMaxJumpHeight",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetMaxJumpHeight(Framework.Core.Actor pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetMaxJumpHeight();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1883859516,"GetGravity",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetGravity(Framework.Core.Actor pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetGravity();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1520964584,"SetGravity",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableFloat),"fGravity",false, null,null)]
#endif
		public static bool AT_SetGravity(Framework.Core.Actor pPointer, VariableFloat fGravity)
		{
			if(fGravity== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetGravity(fGravity.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-220446188,"EnableGravity",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableBool),"bGravity",false, null,null)]
#endif
		public static bool AT_EnableGravity(Framework.Core.Actor pPointer, VariableBool bGravity)
		{
			if(bGravity== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.EnableGravity(bGravity.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-465067774,"GetActorParameter",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.ActorParameter),null)]
#endif
		public static bool AT_GetActorParameter(Framework.Core.Actor pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetActorParameter();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1165429009,"GetActionStateGraph",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.ActionStateGraph),null)]
#endif
		public static bool AT_GetActionStateGraph(Framework.Core.Actor pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetActionStateGraph();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-178072086,"SetActorAgent",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableUser),"pActorAgent",false, typeof(Framework.Core.ActorAgent),null)]
#endif
		public static bool AT_SetActorAgent(Framework.Core.Actor pPointer, VariableUser pActorAgent)
		{
			if(pActorAgent== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetActorAgent((Framework.Core.ActorAgent)pActorAgent.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-780460258,"GetActorAgent",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.ActorAgent),null)]
#endif
		public static bool AT_GetActorAgent(Framework.Core.Actor pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetActorAgent();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1276637051,"SetPosition",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableVector3),"vPos",false, null,null)]
#endif
		public static bool AT_SetPosition(Framework.Core.Actor pPointer, VariableVector3 vPos)
		{
			if(vPos== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetPosition(vPos.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1917471407,"CanDoGroundAction",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"doActionType",false, null,typeof(Framework.Core.EActionStateType))]
		[ATMethodArgv(typeof(VariableInt),"tag",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_CanDoGroundAction(Framework.Core.Actor pPointer, VariableInt doActionType, VariableInt tag, VariableBool pReturn=null)
		{
			if(doActionType== null || tag== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CanDoGroundAction((Framework.Core.EActionStateType)doActionType.mValue,(uint)tag.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-722658812,"CanDoSlide",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableBool),"bIncludeSelf",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_CanDoSlide(Framework.Core.Actor pPointer, VariableBool bIncludeSelf, VariableBool pReturn=null)
		{
			if(bIncludeSelf== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CanDoSlide(bIncludeSelf.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(402565290,"CanDoJumpAction",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_CanDoJumpAction(Framework.Core.Actor pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CanDoJumpAction();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-538469385,"CanDoBuffStateAction",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_CanDoBuffStateAction(Framework.Core.Actor pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CanDoBuffStateAction();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1112719426,"CanDoCrouchAction",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_CanDoCrouchAction(Framework.Core.Actor pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CanDoCrouchAction();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-273803101,"StopBuffers",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"buffType",false, null,typeof(Framework.Core.EBuffType))]
		[ATMethodArgv(typeof(VariableInt),"count",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bTriggerEvent",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"dispelLevel",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"removeLayer",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_StopBuffers(Framework.Core.Actor pPointer, VariableInt buffType, VariableInt count, VariableBool bTriggerEvent, VariableInt dispelLevel, VariableInt removeLayer, VariableBool pReturn=null)
		{
			if(buffType== null || count== null || bTriggerEvent== null || dispelLevel== null || removeLayer== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.StopBuffers((Framework.Core.EBuffType)buffType.mValue,count.mValue,bTriggerEvent.mValue,(uint)dispelLevel.mValue,removeLayer.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1386079312,"StopEffectsBuffers",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"effectBits",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"count",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bTriggerEvent",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"dispelLevel",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"removeLayer",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_StopEffectsBuffers(Framework.Core.Actor pPointer, VariableInt effectBits, VariableInt count, VariableBool bTriggerEvent, VariableInt dispelLevel, VariableInt removeLayer, VariableBool pReturn=null)
		{
			if(effectBits== null || count== null || bTriggerEvent== null || dispelLevel== null || removeLayer== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.StopEffectsBuffers((uint)effectBits.mValue,count.mValue,bTriggerEvent.mValue,(uint)dispelLevel.mValue,removeLayer.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-367962996,"StopBufferByGroup",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"buffGroup",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"count",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bTriggerEvent",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"dispelLevel",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"removeLayer",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_StopBufferByGroup(Framework.Core.Actor pPointer, VariableInt buffGroup, VariableInt count, VariableBool bTriggerEvent, VariableInt dispelLevel, VariableInt removeLayer, VariableBool pReturn=null)
		{
			if(buffGroup== null || count== null || bTriggerEvent== null || dispelLevel== null || removeLayer== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.StopBufferByGroup(buffGroup.mValue,count.mValue,bTriggerEvent.mValue,(uint)dispelLevel.mValue,removeLayer.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(381308779,"HasBuffer",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"buffType",false, null,typeof(Framework.Core.EBuffType))]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_HasBuffer(Framework.Core.Actor pPointer, VariableInt buffType, VariableBool pReturn=null)
		{
			if(buffType== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.HasBuffer((Framework.Core.EBuffType)buffType.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1590420,"HasBuffEffect",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"buffEffect",false, null,typeof(Framework.Core.EBuffEffectBit))]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_HasBuffEffect(Framework.Core.Actor pPointer, VariableInt buffEffect, VariableBool pReturn=null)
		{
			if(buffEffect== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.HasBuffEffect((Framework.Core.EBuffEffectBit)buffEffect.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1960361843,"GetBuffer",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"dwBufferID",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.BufferState),null)]
#endif
		public static bool AT_GetBuffer(Framework.Core.Actor pPointer, VariableInt dwBufferID, VariableUser pReturn=null)
		{
			if(dwBufferID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetBuffer((uint)dwBufferID.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1768245781,"SetActionValue",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"nParamID",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fValue",false, null,null)]
#endif
		public static bool AT_SetActionValue(Framework.Core.Actor pPointer, VariableInt nParamID, VariableFloat fValue)
		{
			if(nParamID== null || fValue== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetActionValue(nParamID.mValue,fValue.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1487322123,"SetActionValue_1",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableString),"strParamID",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fValue",false, null,null)]
#endif
		public static bool AT_SetActionValue_1(Framework.Core.Actor pPointer, VariableString strParamID, VariableFloat fValue)
		{
			if(strParamID== null || fValue== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetActionValue(strParamID.mValue,fValue.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(257290566,"SetIdleType",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(Framework.Core.EActionStateType))]
#endif
		public static bool AT_SetIdleType(Framework.Core.Actor pPointer, VariableInt type)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetIdleType((Framework.Core.EActionStateType)type.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(453118092,"GetIdleType",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(Framework.Core.EActionStateType))]
#endif
		public static bool AT_GetIdleType(Framework.Core.Actor pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetIdleType();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1209311789,"SwitchActionType",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"eState",false, null,typeof(Framework.Core.EActionStateType))]
#endif
		public static bool AT_SwitchActionType(Framework.Core.Actor pPointer, VariableInt eState)
		{
			if(eState== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SwitchActionType((Framework.Core.EActionStateType)eState.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(955251209,"StartActionByType",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"pType",false, null,typeof(Framework.Core.EActionStateType))]
		[ATMethodArgv(typeof(VariableFloat),"fDeltaTime",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fSpeed",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bForceStart",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAnimatorTrigger",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bClearOnHitActorSet",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fAnimationOffset",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_StartActionByType(Framework.Core.Actor pPointer, VariableInt pType, VariableFloat fDeltaTime, VariableFloat fSpeed, VariableBool bForceStart, VariableBool bAnimatorTrigger, VariableBool bClearOnHitActorSet, VariableFloat fAnimationOffset, VariableBool pReturn=null)
		{
			if(pType== null || fDeltaTime== null || fSpeed== null || bForceStart== null || bAnimatorTrigger== null || bClearOnHitActorSet== null || fAnimationOffset== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.StartActionByType((Framework.Core.EActionStateType)pType.mValue,fDeltaTime.mValue,fSpeed.mValue,bForceStart.mValue,bAnimatorTrigger.mValue,bClearOnHitActorSet.mValue,fAnimationOffset.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(166713860,"StartActionByTag",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(Framework.Core.EActionStateType))]
		[ATMethodArgv(typeof(VariableInt),"nTag",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDeltaTime",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fSpeed",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bForceStart",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"param",false, typeof(Framework.Data.StateParam),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_StartActionByTag(Framework.Core.Actor pPointer, VariableInt type, VariableInt nTag, VariableFloat fDeltaTime, VariableFloat fSpeed, VariableBool bForceStart, VariableUser param, VariableBool pReturn=null)
		{
			if(type== null || nTag== null || fDeltaTime== null || fSpeed== null || bForceStart== null || param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.StartActionByTag((Framework.Core.EActionStateType)type.mValue,nTag.mValue,fDeltaTime.mValue,fSpeed.mValue,bForceStart.mValue,(Framework.Data.StateParam)param.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-116718839,"StartActionByName",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableString),"strName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDeltaTime",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fSpeed",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bForceStart",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"param",false, typeof(Framework.Data.StateParam),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_StartActionByName(Framework.Core.Actor pPointer, VariableString strName, VariableFloat fDeltaTime, VariableFloat fSpeed, VariableBool bForceStart, VariableUser param, VariableBool pReturn=null)
		{
			if(strName== null || fDeltaTime== null || fSpeed== null || bForceStart== null || param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.StartActionByName(strName.mValue,fDeltaTime.mValue,fSpeed.mValue,bForceStart.mValue,(Framework.Data.StateParam)param.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1080494372,"StartActionState",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableUser),"pState",false, typeof(Framework.Core.ActionState),null)]
		[ATMethodArgv(typeof(VariableFloat),"fDeltaTime",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"actionSpeed",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bForceStart",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAnimatorTrigger",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bClearOnHitActorSet",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fAnimationOffset",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"param",false, typeof(Framework.Data.StateParam),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_StartActionState(Framework.Core.Actor pPointer, VariableUser pState, VariableFloat fDeltaTime, VariableFloat actionSpeed, VariableBool bForceStart, VariableBool bAnimatorTrigger, VariableBool bClearOnHitActorSet, VariableFloat fAnimationOffset, VariableUser param, VariableBool pReturn=null)
		{
			if(pState== null || fDeltaTime== null || actionSpeed== null || bForceStart== null || bAnimatorTrigger== null || bClearOnHitActorSet== null || fAnimationOffset== null || param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.StartActionState((Framework.Core.ActionState)pState.mValue,fDeltaTime.mValue,actionSpeed.mValue,bForceStart.mValue,bAnimatorTrigger.mValue,bClearOnHitActorSet.mValue,fAnimationOffset.mValue,(Framework.Data.StateParam)param.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-847804814,"GetCurrentActionStateType",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(Framework.Core.EActionStateType))]
#endif
		public static bool AT_GetCurrentActionStateType(Framework.Core.Actor pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetCurrentActionStateType();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(783507463,"GetCurrentActionState",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.ActionState),null)]
#endif
		public static bool AT_GetCurrentActionState(Framework.Core.Actor pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentActionState();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1086133524,"GetActionStateDuration",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(Framework.Core.EActionStateType))]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetActionStateDuration(Framework.Core.Actor pPointer, VariableInt type, VariableFloat pReturn=null)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetActionStateDuration((Framework.Core.EActionStateType)type.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1623693629,"StopCurrentActionState",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
#endif
		public static bool AT_StopCurrentActionState(Framework.Core.Actor pPointer)
		{
			pPointer.StopCurrentActionState();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(118136581,"GetCurrentOverrideActionState",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.ActionState),null)]
#endif
		public static bool AT_GetCurrentOverrideActionState(Framework.Core.Actor pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCurrentOverrideActionState();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1664729584,"StopCurrentOverrideActionState",typeof(Framework.Core.Actor))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.Actor),null)]
#endif
		public static bool AT_StopCurrentOverrideActionState(Framework.Core.Actor pPointer)
		{
			pPointer.StopCurrentOverrideActionState();
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 636507301:
				{//Framework.Core.Actor->GetPropertyEffecter
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetPropertyEffecter(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -403618621:
				{//Framework.Core.Actor->EnableSkill
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_EnableSkill(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
				case 1480893833:
				{//Framework.Core.Actor->GetLevelContextData
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetLevelContextData(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 1648251282:
				{//Framework.Core.Actor->GetQualityContextData
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetQualityContextData(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 458799974:
				{//Framework.Core.Actor->GetTurnTime
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetTurnTime(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case -1336095790:
				{//Framework.Core.Actor->SetDestrotyDelta
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetDestrotyDelta(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case 131072779:
				{//Framework.Core.Actor->IsInvincible
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_IsInvincible(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 1694231299:
				{//Framework.Core.Actor->SetInvincibleByDuration
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetInvincibleByDuration(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case 46256047:
				{//Framework.Core.Actor->CanCancelState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_CanCancelState(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -2078301578:
				{//Framework.Core.Actor->CanCancelState_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_CanCancelState_1(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -1996489456:
				{//Framework.Core.Actor->ClearStateProperty
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_ClearStateProperty(pUserClass.mValue as Framework.Core.Actor);
				}
				case 1649620682:
				{//Framework.Core.Actor->GetPetRiding
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetPetRiding(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 1072039686:
				{//Framework.Core.Actor->IsPetRide
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_IsPetRide(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -867953279:
				{//Framework.Core.Actor->ResetMomentum
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_ResetMomentum(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case -267169915:
				{//Framework.Core.Actor->GetFraction
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetFraction(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case -99839765:
				{//Framework.Core.Actor->SetFraction
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetFraction(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case -204121276:
				{//Framework.Core.Actor->GetSpeed
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetSpeed(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -2026832832:
				{//Framework.Core.Actor->SetSpeed
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetSpeed(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask));
				}
				case 1418438349:
				{//Framework.Core.Actor->SetSpeedXZ
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetSpeedXZ(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask));
				}
				case 1888086346:
				{//Framework.Core.Actor->SetSpeedXZ_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetSpeedXZ_1(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask));
				}
				case -1753493842:
				{//Framework.Core.Actor->SetSpeedY
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetSpeedY(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case -1554835646:
				{//Framework.Core.Actor->GetJumpHorizenSpeed
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetJumpHorizenSpeed(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case 774670985:
				{//Framework.Core.Actor->GetJumpVerticalSpeed
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetJumpVerticalSpeed(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case -1605685122:
				{//Framework.Core.Actor->GetFallingSpeed
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetFallingSpeed(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case 579145607:
				{//Framework.Core.Actor->GetMaxJumpHeight
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetMaxJumpHeight(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case 1883859516:
				{//Framework.Core.Actor->GetGravity
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetGravity(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case -1520964584:
				{//Framework.Core.Actor->SetGravity
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetGravity(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case -220446188:
				{//Framework.Core.Actor->EnableGravity
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_EnableGravity(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
				case -465067774:
				{//Framework.Core.Actor->GetActorParameter
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetActorParameter(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 1165429009:
				{//Framework.Core.Actor->GetActionStateGraph
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetActionStateGraph(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -178072086:
				{//Framework.Core.Actor->SetActorAgent
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetActorAgent(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(1, pTask));
				}
				case -780460258:
				{//Framework.Core.Actor->GetActorAgent
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetActorAgent(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 1276637051:
				{//Framework.Core.Actor->SetPosition
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetPosition(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask));
				}
				case 1917471407:
				{//Framework.Core.Actor->CanDoGroundAction
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_CanDoGroundAction(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -722658812:
				{//Framework.Core.Actor->CanDoSlide
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_CanDoSlide(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 402565290:
				{//Framework.Core.Actor->CanDoJumpAction
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_CanDoJumpAction(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -538469385:
				{//Framework.Core.Actor->CanDoBuffStateAction
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_CanDoBuffStateAction(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 1112719426:
				{//Framework.Core.Actor->CanDoCrouchAction
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_CanDoCrouchAction(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -273803101:
				{//Framework.Core.Actor->StopBuffers
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_StopBuffers(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(5, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 1386079312:
				{//Framework.Core.Actor->StopEffectsBuffers
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_StopEffectsBuffers(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(5, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -367962996:
				{//Framework.Core.Actor->StopBufferByGroup
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_StopBufferByGroup(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(5, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 381308779:
				{//Framework.Core.Actor->HasBuffer
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_HasBuffer(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 1590420:
				{//Framework.Core.Actor->HasBuffEffect
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_HasBuffEffect(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -1960361843:
				{//Framework.Core.Actor->GetBuffer
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetBuffer(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -1768245781:
				{//Framework.Core.Actor->SetActionValue
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetActionValue(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask));
				}
				case -1487322123:
				{//Framework.Core.Actor->SetActionValue_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetActionValue_1(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask));
				}
				case 257290566:
				{//Framework.Core.Actor->SetIdleType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SetIdleType(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 453118092:
				{//Framework.Core.Actor->GetIdleType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetIdleType(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 1209311789:
				{//Framework.Core.Actor->SwitchActionType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_SwitchActionType(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 955251209:
				{//Framework.Core.Actor->StartActionByType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=7) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_StartActionByType(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(6, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(7, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 166713860:
				{//Framework.Core.Actor->StartActionByTag
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=6) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_StartActionByTag(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(6, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -116718839:
				{//Framework.Core.Actor->StartActionByName
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_StartActionByName(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(5, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -1080494372:
				{//Framework.Core.Actor->StartActionState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=8) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_StartActionState(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(6, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(7, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(8, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -847804814:
				{//Framework.Core.Actor->GetCurrentActionStateType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetCurrentActionStateType(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 783507463:
				{//Framework.Core.Actor->GetCurrentActionState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetCurrentActionState(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -1086133524:
				{//Framework.Core.Actor->GetActionStateDuration
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetActionStateDuration(pUserClass.mValue as Framework.Core.Actor, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case 1623693629:
				{//Framework.Core.Actor->StopCurrentActionState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_StopCurrentActionState(pUserClass.mValue as Framework.Core.Actor);
				}
				case 118136581:
				{//Framework.Core.Actor->GetCurrentOverrideActionState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_GetCurrentOverrideActionState(pUserClass.mValue as Framework.Core.Actor, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 1664729584:
				{//Framework.Core.Actor->StopCurrentOverrideActionState
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.Actor)) return true;
					return AgentTree_Actor.AT_StopCurrentOverrideActionState(pUserClass.mValue as Framework.Core.Actor);
				}
			}
			return AgentTree_AWorldNode.DoAction(pUserClass,pTask,pAction,functionId);
		}
	}
}
