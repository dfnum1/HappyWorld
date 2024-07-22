//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("技能系统",true)]
#endif
	public static class AgentTree_SkillInformation
	{
#if UNITY_EDITOR
		[ATMonoFunc(896849859,"GetSkillByTag",typeof(Framework.Core.SkillInformation))]
		[ATMethodArgv(typeof(VariableInt),"tag",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.Skill),null)]
#endif
		public static bool AT_GetSkillByTag(Framework.Core.SkillInformation pPointer, VariableInt tag, VariableUser pReturn=null)
		{
			if(tag== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetSkillByTag((uint)tag.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-564122994,"EnableSkill",typeof(Framework.Core.SkillInformation))]
		[ATMethodArgv(typeof(VariableBool),"bEnable",false, null,null)]
#endif
		public static bool AT_EnableSkill(Framework.Core.SkillInformation pPointer, VariableBool bEnable)
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
		[ATMonoFunc(550137578,"CanInterrupt",typeof(Framework.Core.SkillInformation))]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_CanInterrupt(Framework.Core.SkillInformation pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CanInterrupt();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(328911757,"GetInterruptFactor",typeof(Framework.Core.SkillInformation))]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetInterruptFactor(Framework.Core.SkillInformation pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetInterruptFactor();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(467988297,"ClearSkill",typeof(Framework.Core.SkillInformation))]
#endif
		public static bool AT_ClearSkill(Framework.Core.SkillInformation pPointer)
		{
			pPointer.ClearSkill();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-936478076,"CanSkill",typeof(Framework.Core.SkillInformation))]
		[ATMethodArgv(typeof(VariableInt),"skillId",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAddCommand",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bHead",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_CanSkill(Framework.Core.SkillInformation pPointer, VariableInt skillId, VariableBool bAddCommand, VariableBool bHead, VariableBool pReturn=null)
		{
			if(skillId== null || bAddCommand== null || bHead== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CanSkill((uint)skillId.mValue,bAddCommand.mValue,bHead.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1034254774,"CanSkillByTag",typeof(Framework.Core.SkillInformation))]
		[ATMethodArgv(typeof(VariableInt),"nTag",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAddCommand",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bHead",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_CanSkillByTag(Framework.Core.SkillInformation pPointer, VariableInt nTag, VariableBool bAddCommand, VariableBool bHead, VariableBool pReturn=null)
		{
			if(nTag== null || bAddCommand== null || bHead== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CanSkillByTag((uint)nTag.mValue,bAddCommand.mValue,bHead.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(862262881,"ForceDoSkill",typeof(Framework.Core.SkillInformation))]
		[ATMethodArgv(typeof(VariableInt),"nSkill",false, null,null)]
#endif
		public static bool AT_ForceDoSkill(Framework.Core.SkillInformation pPointer, VariableInt nSkill)
		{
			if(nSkill== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ForceDoSkill((uint)nSkill.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1272877384,"ForceDoSkillByTag",typeof(Framework.Core.SkillInformation))]
		[ATMethodArgv(typeof(VariableInt),"nTag",false, null,null)]
#endif
		public static bool AT_ForceDoSkillByTag(Framework.Core.SkillInformation pPointer, VariableInt nTag)
		{
			if(nTag== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ForceDoSkillByTag((uint)nTag.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 896849859:
				{//Framework.Core.SkillInformation->GetSkillByTag
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.SkillInformation)) return true;
					return AgentTree_SkillInformation.AT_GetSkillByTag(pUserClass.mValue as Framework.Core.SkillInformation, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -564122994:
				{//Framework.Core.SkillInformation->EnableSkill
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.SkillInformation)) return true;
					return AgentTree_SkillInformation.AT_EnableSkill(pUserClass.mValue as Framework.Core.SkillInformation, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
				case 550137578:
				{//Framework.Core.SkillInformation->CanInterrupt
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.SkillInformation)) return true;
					return AgentTree_SkillInformation.AT_CanInterrupt(pUserClass.mValue as Framework.Core.SkillInformation, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 328911757:
				{//Framework.Core.SkillInformation->GetInterruptFactor
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.SkillInformation)) return true;
					return AgentTree_SkillInformation.AT_GetInterruptFactor(pUserClass.mValue as Framework.Core.SkillInformation, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case 467988297:
				{//Framework.Core.SkillInformation->ClearSkill
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.SkillInformation)) return true;
					return AgentTree_SkillInformation.AT_ClearSkill(pUserClass.mValue as Framework.Core.SkillInformation);
				}
				case -936478076:
				{//Framework.Core.SkillInformation->CanSkill
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.SkillInformation)) return true;
					return AgentTree_SkillInformation.AT_CanSkill(pUserClass.mValue as Framework.Core.SkillInformation, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -1034254774:
				{//Framework.Core.SkillInformation->CanSkillByTag
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.SkillInformation)) return true;
					return AgentTree_SkillInformation.AT_CanSkillByTag(pUserClass.mValue as Framework.Core.SkillInformation, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 862262881:
				{//Framework.Core.SkillInformation->ForceDoSkill
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.SkillInformation)) return true;
					return AgentTree_SkillInformation.AT_ForceDoSkill(pUserClass.mValue as Framework.Core.SkillInformation, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 1272877384:
				{//Framework.Core.SkillInformation->ForceDoSkillByTag
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.SkillInformation)) return true;
					return AgentTree_SkillInformation.AT_ForceDoSkillByTag(pUserClass.mValue as Framework.Core.SkillInformation, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
			}
			return true;
		}
	}
}
