//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("Actor系统/ActorAgent",true)]
#endif
	public static class AgentTree_ActorAgent
	{
#if UNITY_EDITOR
		[ATMonoFunc(-205682262,"CreateParticle",typeof(Framework.Core.ActorAgent))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorAgent),null)]
		[ATMethodArgv(typeof(VariableString),"strParticle",false, null,typeof(UnityEngine.GameObject))]
		[ATMethodArgv(typeof(VariableVector3),"offset",false, null,null)]
		[ATMethodArgv(typeof(VariableVector3),"rotateOffset",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fScale",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"strSlot",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"bit",false, null,typeof(Framework.Core.ESlotBindBit))]
		[ATMethodArgv(typeof(VariableFloat),"lifeTime",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fParticleSpeed",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bVisibleSyncOwner",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_CreateParticle(Framework.Core.ActorAgent pPointer, VariableString strParticle, VariableVector3 offset, VariableVector3 rotateOffset, VariableFloat fScale, VariableString strSlot, VariableInt bit, VariableFloat lifeTime, VariableFloat fParticleSpeed, VariableBool bVisibleSyncOwner, VariableInt pReturn=null)
		{
			if(strParticle== null || offset== null || rotateOffset== null || fScale== null || strSlot== null || bit== null || lifeTime== null || fParticleSpeed== null || bVisibleSyncOwner== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CreateParticle(strParticle.mValue,offset.mValue,rotateOffset.mValue,fScale.mValue,strSlot.mValue,(Framework.Core.ESlotBindBit)bit.mValue,lifeTime.mValue,fParticleSpeed.mValue,bVisibleSyncOwner.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1900216873,"CreateParticleExplicit",typeof(Framework.Core.ActorAgent))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorAgent),null)]
		[ATMethodArgv(typeof(VariableString),"strParticle",false, null,typeof(UnityEngine.GameObject))]
		[ATMethodArgv(typeof(VariableVector3),"offset",false, null,null)]
		[ATMethodArgv(typeof(VariableVector3),"rotateOffset",false, null,null)]
		[ATMethodArgv(typeof(VariableObject),"pSlot",false, typeof(UnityEngine.Transform),null)]
		[ATMethodArgv(typeof(VariableFloat),"fScale",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"bit",false, null,typeof(Framework.Core.ESlotBindBit))]
		[ATMethodArgv(typeof(VariableFloat),"lifeTime",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fParticleSpeed",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bVisibleSyncOwner",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_CreateParticleExplicit(Framework.Core.ActorAgent pPointer, VariableString strParticle, VariableVector3 offset, VariableVector3 rotateOffset, VariableObject pSlot, VariableFloat fScale, VariableInt bit, VariableFloat lifeTime, VariableFloat fParticleSpeed, VariableBool bVisibleSyncOwner, VariableInt pReturn=null)
		{
			if(strParticle== null || offset== null || rotateOffset== null || pSlot== null || fScale== null || bit== null || lifeTime== null || fParticleSpeed== null || bVisibleSyncOwner== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CreateParticleExplicit(strParticle.mValue,offset.mValue,rotateOffset.mValue,pSlot.ToObject<UnityEngine.Transform>(),fScale.mValue,(Framework.Core.ESlotBindBit)bit.mValue,lifeTime.mValue,fParticleSpeed.mValue,bVisibleSyncOwner.mValue);
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -205682262:
				{//Framework.Core.ActorAgent->CreateParticle
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=9) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorAgent)) return true;
					return AgentTree_ActorAgent.AT_CreateParticle(pUserClass.mValue as Framework.Core.ActorAgent, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(7, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(8, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(9, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 1900216873:
				{//Framework.Core.ActorAgent->CreateParticleExplicit
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=9) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorAgent)) return true;
					return AgentTree_ActorAgent.AT_CreateParticleExplicit(pUserClass.mValue as Framework.Core.ActorAgent, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(7, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(8, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(9, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
			}
			return true;
		}
	}
}
