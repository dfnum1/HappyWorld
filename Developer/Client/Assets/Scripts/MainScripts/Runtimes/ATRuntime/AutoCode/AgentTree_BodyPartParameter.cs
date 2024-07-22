//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("Actor/部位",true)]
#endif
	public static class AgentTree_BodyPartParameter
	{
#if UNITY_EDITOR
		[ATMonoFunc(-69107422,"获取准心对象",typeof(Framework.Core.BodyPartParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.BodyPartParameter),null)]
		[ATMethodReturn(typeof(VariableMonoScript),"pReturn",typeof(Framework.Core.AInstanceAble),null)]
#endif
		public static bool AT_GetAmiPoint(Framework.Core.BodyPartParameter pPointer, VariableMonoScript pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (UnityEngine.Behaviour)pPointer.GetAmiPoint();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1293021367,"表现效果",typeof(Framework.Core.BodyPartParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.BodyPartParameter),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDuration",false, null,null)]
		[ATMethodArgv(typeof(VariableCurve),"R",false, null,null)]
		[ATMethodArgv(typeof(VariableCurve),"G",false, null,null)]
		[ATMethodArgv(typeof(VariableCurve),"B",false, null,null)]
		[ATMethodArgv(typeof(VariableCurve),"A",false, null,null)]
#endif
		public static bool AT_ApplayEffect(Framework.Core.BodyPartParameter pPointer, VariableString propertyName, VariableFloat fDuration, VariableCurve R, VariableCurve G, VariableCurve B, VariableCurve A)
		{
			if(propertyName== null || fDuration== null || R== null || G== null || B== null || A== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ApplayEffect(propertyName.mValue,fDuration.mValue,R.mValue,G.mValue,B.mValue,A.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(398127439,"表现Rim效果",typeof(Framework.Core.BodyPartParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.BodyPartParameter),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDuration",false, null,null)]
		[ATMethodArgv(typeof(VariableCurve),"Rim",false, null,null)]
#endif
		public static bool AT_ApplayRimEffect(Framework.Core.BodyPartParameter pPointer, VariableString propertyName, VariableFloat fDuration, VariableCurve Rim)
		{
			if(propertyName== null || fDuration== null || Rim== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ApplayRimEffect(propertyName.mValue,fDuration.mValue,Rim.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(255764216,"SetPropertyColor",typeof(Framework.Core.BodyPartParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.BodyPartParameter),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableColor),"color",false, null,null)]
#endif
		public static bool AT_SetPropertyColor(Framework.Core.BodyPartParameter pPointer, VariableString propertyName, VariableColor color)
		{
			if(propertyName== null || color== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetPropertyColor(propertyName.mValue,color.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-391628369,"SetPropertyFloat",typeof(Framework.Core.BodyPartParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.BodyPartParameter),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fValue",false, null,null)]
#endif
		public static bool AT_SetPropertyFloat(Framework.Core.BodyPartParameter pPointer, VariableString propertyName, VariableFloat fValue)
		{
			if(propertyName== null || fValue== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetPropertyFloat(propertyName.mValue,fValue.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -69107422:
				{//Framework.Core.BodyPartParameter->GetAmiPoint
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.BodyPartParameter)) return true;
					return AgentTree_BodyPartParameter.AT_GetAmiPoint(pUserClass.mValue as Framework.Core.BodyPartParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(0, pTask));
				}
				case -1293021367:
				{//Framework.Core.BodyPartParameter->ApplayEffect
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=6) return true;
					if(!(pUserClass.mValue is Framework.Core.BodyPartParameter)) return true;
					return AgentTree_BodyPartParameter.AT_ApplayEffect(pUserClass.mValue as Framework.Core.BodyPartParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableCurve>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableCurve>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableCurve>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableCurve>(6, pTask));
				}
				case 398127439:
				{//Framework.Core.BodyPartParameter->ApplayRimEffect
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(!(pUserClass.mValue is Framework.Core.BodyPartParameter)) return true;
					return AgentTree_BodyPartParameter.AT_ApplayRimEffect(pUserClass.mValue as Framework.Core.BodyPartParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableCurve>(3, pTask));
				}
				case 255764216:
				{//Framework.Core.BodyPartParameter->SetPropertyColor
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.BodyPartParameter)) return true;
					return AgentTree_BodyPartParameter.AT_SetPropertyColor(pUserClass.mValue as Framework.Core.BodyPartParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(2, pTask));
				}
				case -391628369:
				{//Framework.Core.BodyPartParameter->SetPropertyFloat
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.BodyPartParameter)) return true;
					return AgentTree_BodyPartParameter.AT_SetPropertyFloat(pUserClass.mValue as Framework.Core.BodyPartParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask));
				}
			}
			return true;
		}
	}
}
