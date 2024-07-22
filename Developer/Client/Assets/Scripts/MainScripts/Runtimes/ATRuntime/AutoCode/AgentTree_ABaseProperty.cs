//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("属性过渡管理器/操作句柄",true)]
#endif
	public static class AgentTree_ABaseProperty
	{
#if UNITY_EDITOR
		[ATMonoFunc(1674466214,"设置循环模式",typeof(Framework.Core.ABaseProperty))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ABaseProperty),null)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(Framework.Core.ABaseProperty.ELoopType))]
		[ATMethodArgv(typeof(VariableInt),"count",false, null,null)]
#endif
		public static bool AT_SetLoop(Framework.Core.ABaseProperty pPointer, VariableInt type, VariableInt count)
		{
			if(type== null || count== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetLoop((Framework.Core.ABaseProperty.ELoopType)type.mValue,count.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-284538173,"设置运行时间",typeof(Framework.Core.ABaseProperty))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ABaseProperty),null)]
		[ATMethodArgv(typeof(VariableFloat),"fTime",false, null,null)]
#endif
		public static bool AT_SetRuntimeTime(Framework.Core.ABaseProperty pPointer, VariableFloat fTime)
		{
			if(fTime== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetRuntimeTime(fTime.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1371160238,"设置延迟时间",typeof(Framework.Core.ABaseProperty))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ABaseProperty),null)]
		[ATMethodArgv(typeof(VariableFloat),"fDleta",false, null,null)]
#endif
		public static bool AT_SetDelayTime(Framework.Core.ABaseProperty pPointer, VariableFloat fDleta)
		{
			if(fDleta== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetDelayTime(fDleta.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1674466214:
				{//Framework.Core.ABaseProperty->SetLoop
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.ABaseProperty)) return true;
					return AgentTree_ABaseProperty.AT_SetLoop(pUserClass.mValue as Framework.Core.ABaseProperty, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask));
				}
				case -284538173:
				{//Framework.Core.ABaseProperty->SetRuntimeTime
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.ABaseProperty)) return true;
					return AgentTree_ABaseProperty.AT_SetRuntimeTime(pUserClass.mValue as Framework.Core.ABaseProperty, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case 1371160238:
				{//Framework.Core.ABaseProperty->SetDelayTime
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.ABaseProperty)) return true;
					return AgentTree_ABaseProperty.AT_SetDelayTime(pUserClass.mValue as Framework.Core.ABaseProperty, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
			}
			return true;
		}
	}
}
