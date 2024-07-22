//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("逻辑状态/战斗/战斗伤害数据",true)]
#endif
	public static class AgentTree_BattleDamage
	{
#if UNITY_EDITOR
		[ATMonoFunc(325760087,"get_pActor",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.Actor),null)]
#endif
		public static bool AT_get_pActor(Framework.BattlePlus.BattleDamage pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.pActor;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1399618266,"get_pAttacker",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.Actor),null)]
#endif
		public static bool AT_get_pAttacker(Framework.BattlePlus.BattleDamage pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.pAttacker;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(202197688,"get_finalValue",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_get_finalValue(Framework.BattlePlus.BattleDamage pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.finalValue;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-725834923,"get_damage_id",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_get_damage_id(Framework.BattlePlus.BattleDamage pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.damage_id;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-412087707,"get_damageLevel",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_get_damageLevel(Framework.BattlePlus.BattleDamage pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.damageLevel;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1058052478,"get_bodyPart",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.BodyPartParameter),null)]
#endif
		public static bool AT_get_bodyPart(Framework.BattlePlus.BattleDamage pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.bodyPart;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(605191648,"get_actorType",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(Framework.Core.EActorType))]
#endif
		public static bool AT_get_actorType(Framework.BattlePlus.BattleDamage pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.actorType;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1067317187,"get_monsterType",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(Framework.Base.EMonsterType))]
#endif
		public static bool AT_get_monsterType(Framework.BattlePlus.BattleDamage pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.monsterType;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1024554556,"get_bCril",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_get_bCril(Framework.BattlePlus.BattleDamage pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.bCril;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1709517284,"get_bBlock",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_get_bBlock(Framework.BattlePlus.BattleDamage pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.bBlock;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-456976464,"get_bKilled",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_get_bKilled(Framework.BattlePlus.BattleDamage pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.bKilled;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1974570053,"get_bDamageExport",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_get_bDamageExport(Framework.BattlePlus.BattleDamage pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.bDamageExport;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1675270675,"get_bCareerWeaknees",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_get_bCareerWeaknees(Framework.BattlePlus.BattleDamage pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.bCareerWeaknees;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1968435665,"get_bCampReaction",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_get_bCampReaction(Framework.BattlePlus.BattleDamage pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.bCampReaction;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(14559046,"get_pInstanceAble",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableMonoScript),"pReturn",typeof(Framework.Core.AInstanceAble),null)]
#endif
		public static bool AT_get_pInstanceAble(Framework.BattlePlus.BattleDamage pPointer, VariableMonoScript pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (UnityEngine.Behaviour)pPointer.pInstanceAble;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1235412740,"get_damagePostBit",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableByte),"pReturn",null,null)]
#endif
		public static bool AT_get_damagePostBit(Framework.BattlePlus.BattleDamage pPointer, VariableByte pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.damagePostBit;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-775942494,"get_elementEffect",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.IContextData),null)]
#endif
		public static bool AT_get_elementEffect(Framework.BattlePlus.BattleDamage pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.elementEffect;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1895646541,"get_attackElementType",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(Framework.Core.EElementType))]
#endif
		public static bool AT_get_attackElementType(Framework.BattlePlus.BattleDamage pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.attackElementType;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(187556128,"get_targetElementType",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(Framework.Core.EElementType))]
#endif
		public static bool AT_get_targetElementType(Framework.BattlePlus.BattleDamage pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.targetElementType;
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1599318743,"get_attackSkillType",typeof(Framework.BattlePlus.BattleDamage), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.BattlePlus.BattleDamage),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(Framework.Base.ESkillType))]
#endif
		public static bool AT_get_attackSkillType(Framework.BattlePlus.BattleDamage pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.attackSkillType;
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 325760087:
				{//Framework.BattlePlus.BattleDamage->pActor
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_pActor((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -1399618266:
				{//Framework.BattlePlus.BattleDamage->pAttacker
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_pAttacker((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 202197688:
				{//Framework.BattlePlus.BattleDamage->finalValue
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_finalValue((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -725834923:
				{//Framework.BattlePlus.BattleDamage->damage_id
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_damage_id((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -412087707:
				{//Framework.BattlePlus.BattleDamage->damageLevel
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_damageLevel((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -1058052478:
				{//Framework.BattlePlus.BattleDamage->bodyPart
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_bodyPart((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 605191648:
				{//Framework.BattlePlus.BattleDamage->actorType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_actorType((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -1067317187:
				{//Framework.BattlePlus.BattleDamage->monsterType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_monsterType((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -1024554556:
				{//Framework.BattlePlus.BattleDamage->bCril
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_bCril((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -1709517284:
				{//Framework.BattlePlus.BattleDamage->bBlock
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_bBlock((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -456976464:
				{//Framework.BattlePlus.BattleDamage->bKilled
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_bKilled((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 1974570053:
				{//Framework.BattlePlus.BattleDamage->bDamageExport
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_bDamageExport((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case -1675270675:
				{//Framework.BattlePlus.BattleDamage->bCareerWeaknees
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_bCareerWeaknees((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 1968435665:
				{//Framework.BattlePlus.BattleDamage->bCampReaction
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_bCampReaction((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 14559046:
				{//Framework.BattlePlus.BattleDamage->pInstanceAble
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_pInstanceAble((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(0, pTask));
				}
				case 1235412740:
				{//Framework.BattlePlus.BattleDamage->damagePostBit
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_damagePostBit((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableByte>(0, pTask));
				}
				case -775942494:
				{//Framework.BattlePlus.BattleDamage->elementEffect
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_elementEffect((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 1895646541:
				{//Framework.BattlePlus.BattleDamage->attackElementType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_attackElementType((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 187556128:
				{//Framework.BattlePlus.BattleDamage->targetElementType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_targetElementType((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -1599318743:
				{//Framework.BattlePlus.BattleDamage->attackSkillType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_BattleDamage.AT_get_attackSkillType((Framework.BattlePlus.BattleDamage)pUserClass.mValue, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
			}
			return true;
		}
	}
}
