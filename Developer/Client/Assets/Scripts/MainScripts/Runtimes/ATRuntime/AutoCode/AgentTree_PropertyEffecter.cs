//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("属性过渡管理器",true)]
#endif
	public static class AgentTree_PropertyEffecter
	{
#if UNITY_EDITOR
		[ATMonoFunc(-2058834526,"SetCreateDelay",typeof(Framework.Core.PropertyEffecter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.PropertyEffecter),null)]
		[ATMethodArgv(typeof(VariableFloat),"fDelay",false, null,null)]
#endif
		public static bool AT_SetCreateDelay(Framework.Core.PropertyEffecter pPointer, VariableFloat fDelay)
		{
			if(fDelay== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetCreateDelay(fDelay.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-723254497,"添加浮点属性",typeof(Framework.Core.PropertyEffecter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.PropertyEffecter),null)]
		[ATMethodArgv(typeof(VariableString),"strProperty",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDuration",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"srcValue",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"dstValue",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDelay",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"materialIndex",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.ABaseProperty),null)]
#endif
		public static bool AT_AddFloat(Framework.Core.PropertyEffecter pPointer, VariableString strProperty, VariableFloat fDuration, VariableFloat srcValue, VariableFloat dstValue, VariableFloat fDelay, VariableBool bBlock, VariableBool bShare, VariableInt materialIndex, VariableUser pReturn=null)
		{
			if(strProperty== null || fDuration== null || srcValue== null || dstValue== null || fDelay== null || bBlock== null || bShare== null || materialIndex== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.AddFloat(strProperty.mValue,fDuration.mValue,srcValue.mValue,dstValue.mValue,fDelay.mValue,bBlock.mValue,bShare.mValue,materialIndex.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1712569318,"添加浮点属性-曲线",typeof(Framework.Core.PropertyEffecter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.PropertyEffecter),null)]
		[ATMethodArgv(typeof(VariableString),"strProperty",false, null,null)]
		[ATMethodArgv(typeof(VariableCurve),"curve",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"srcValue",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"dstValue",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDelay",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"materialIndex",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.ABaseProperty),null)]
#endif
		public static bool AT_AddFloatByCurve(Framework.Core.PropertyEffecter pPointer, VariableString strProperty, VariableCurve curve, VariableFloat srcValue, VariableFloat dstValue, VariableFloat fDelay, VariableBool bBlock, VariableBool bShare, VariableInt materialIndex, VariableUser pReturn=null)
		{
			if(strProperty== null || curve== null || srcValue== null || dstValue== null || fDelay== null || bBlock== null || bShare== null || materialIndex== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.AddFloatByCurve(strProperty.mValue,curve.mValue,srcValue.mValue,dstValue.mValue,fDelay.mValue,bBlock.mValue,bShare.mValue,materialIndex.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1333876188,"添加向量属性",typeof(Framework.Core.PropertyEffecter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.PropertyEffecter),null)]
		[ATMethodArgv(typeof(VariableString),"strProperty",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDuration",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4),"srcValue",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4),"dstValue",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDelay",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"materialIndex",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.ABaseProperty),null)]
#endif
		public static bool AT_AddVector(Framework.Core.PropertyEffecter pPointer, VariableString strProperty, VariableFloat fDuration, VariableVector4 srcValue, VariableVector4 dstValue, VariableFloat fDelay, VariableBool bBlock, VariableBool bShare, VariableInt materialIndex, VariableUser pReturn=null)
		{
			if(strProperty== null || fDuration== null || srcValue== null || dstValue== null || fDelay== null || bBlock== null || bShare== null || materialIndex== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.AddVector(strProperty.mValue,fDuration.mValue,srcValue.mValue,dstValue.mValue,fDelay.mValue,bBlock.mValue,bShare.mValue,materialIndex.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(505460657,"添加向量属性-曲线",typeof(Framework.Core.PropertyEffecter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.PropertyEffecter),null)]
		[ATMethodArgv(typeof(VariableString),"strProperty",false, null,null)]
		[ATMethodArgv(typeof(VariableCurve),"curve",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4),"srcValue",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4),"dstValue",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDelay",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"materialIndex",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.ABaseProperty),null)]
#endif
		public static bool AT_AddVectorByCurve(Framework.Core.PropertyEffecter pPointer, VariableString strProperty, VariableCurve curve, VariableVector4 srcValue, VariableVector4 dstValue, VariableFloat fDelay, VariableBool bBlock, VariableBool bShare, VariableInt materialIndex, VariableUser pReturn=null)
		{
			if(strProperty== null || curve== null || srcValue== null || dstValue== null || fDelay== null || bBlock== null || bShare== null || materialIndex== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.AddVectorByCurve(strProperty.mValue,curve.mValue,srcValue.mValue,dstValue.mValue,fDelay.mValue,bBlock.mValue,bShare.mValue,materialIndex.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(257887938,"添加颜色属性",typeof(Framework.Core.PropertyEffecter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.PropertyEffecter),null)]
		[ATMethodArgv(typeof(VariableString),"strProperty",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDuration",false, null,null)]
		[ATMethodArgv(typeof(VariableColor),"srcColor",false, null,null)]
		[ATMethodArgv(typeof(VariableColor),"dstColor",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDelay",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"materialIndex",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.ABaseProperty),null)]
#endif
		public static bool AT_AddColor(Framework.Core.PropertyEffecter pPointer, VariableString strProperty, VariableFloat fDuration, VariableColor srcColor, VariableColor dstColor, VariableFloat fDelay, VariableBool bBlock, VariableBool bShare, VariableInt materialIndex, VariableUser pReturn=null)
		{
			if(strProperty== null || fDuration== null || srcColor== null || dstColor== null || fDelay== null || bBlock== null || bShare== null || materialIndex== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.AddColor(strProperty.mValue,fDuration.mValue,srcColor.mValue,dstColor.mValue,fDelay.mValue,bBlock.mValue,bShare.mValue,materialIndex.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-457316731,"添加颜色属性-曲线",typeof(Framework.Core.PropertyEffecter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.PropertyEffecter),null)]
		[ATMethodArgv(typeof(VariableString),"strProperty",false, null,null)]
		[ATMethodArgv(typeof(VariableCurve),"curve",false, null,null)]
		[ATMethodArgv(typeof(VariableColor),"srcColor",false, null,null)]
		[ATMethodArgv(typeof(VariableColor),"dstColor",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fDelay",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"materialIndex",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.ABaseProperty),null)]
#endif
		public static bool AT_AddColorByCurve(Framework.Core.PropertyEffecter pPointer, VariableString strProperty, VariableCurve curve, VariableColor srcColor, VariableColor dstColor, VariableFloat fDelay, VariableBool bBlock, VariableBool bShare, VariableInt materialIndex, VariableUser pReturn=null)
		{
			if(strProperty== null || curve== null || srcColor== null || dstColor== null || fDelay== null || bBlock== null || bShare== null || materialIndex== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.AddColorByCurve(strProperty.mValue,curve.mValue,srcColor.mValue,dstColor.mValue,fDelay.mValue,bBlock.mValue,bShare.mValue,materialIndex.mValue);
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -2058834526:
				{//Framework.Core.PropertyEffecter->SetCreateDelay
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.PropertyEffecter)) return true;
					return AgentTree_PropertyEffecter.AT_SetCreateDelay(pUserClass.mValue as Framework.Core.PropertyEffecter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case -723254497:
				{//Framework.Core.PropertyEffecter->AddFloat
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=8) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.PropertyEffecter)) return true;
					return AgentTree_PropertyEffecter.AT_AddFloat(pUserClass.mValue as Framework.Core.PropertyEffecter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(6, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(7, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(8, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 1712569318:
				{//Framework.Core.PropertyEffecter->AddFloatByCurve
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=8) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.PropertyEffecter)) return true;
					return AgentTree_PropertyEffecter.AT_AddFloatByCurve(pUserClass.mValue as Framework.Core.PropertyEffecter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableCurve>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(6, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(7, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(8, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 1333876188:
				{//Framework.Core.PropertyEffecter->AddVector
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=8) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.PropertyEffecter)) return true;
					return AgentTree_PropertyEffecter.AT_AddVector(pUserClass.mValue as Framework.Core.PropertyEffecter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(6, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(7, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(8, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 505460657:
				{//Framework.Core.PropertyEffecter->AddVectorByCurve
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=8) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.PropertyEffecter)) return true;
					return AgentTree_PropertyEffecter.AT_AddVectorByCurve(pUserClass.mValue as Framework.Core.PropertyEffecter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableCurve>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(6, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(7, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(8, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 257887938:
				{//Framework.Core.PropertyEffecter->AddColor
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=8) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.PropertyEffecter)) return true;
					return AgentTree_PropertyEffecter.AT_AddColor(pUserClass.mValue as Framework.Core.PropertyEffecter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(6, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(7, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(8, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -457316731:
				{//Framework.Core.PropertyEffecter->AddColorByCurve
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=8) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.PropertyEffecter)) return true;
					return AgentTree_PropertyEffecter.AT_AddColorByCurve(pUserClass.mValue as Framework.Core.PropertyEffecter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableCurve>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(6, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(7, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(8, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
			}
			return true;
		}
	}
}
