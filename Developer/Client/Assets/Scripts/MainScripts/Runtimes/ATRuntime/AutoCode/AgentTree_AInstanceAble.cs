//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("实例/实例化句柄",true)]
#endif
	public static class AgentTree_AInstanceAble
	{
#if UNITY_EDITOR
		[ATMonoFunc(-1531199794,"获取Transform",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.Transform),null)]
#endif
		public static bool AT_GetTransorm(Framework.Core.AInstanceAble pPointer, VariableObject pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetTransorm();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1184135236,"SetPosition",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableVector3),"postion",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bLocal",false, null,null)]
#endif
		public static bool AT_SetPosition(Framework.Core.AInstanceAble pPointer, VariableVector3 postion, VariableBool bLocal)
		{
			if(postion== null || bLocal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetPosition(postion.mValue,bLocal.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1024259411,"SetRotation",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableQuaternion),"rot",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bLocal",false, null,null)]
#endif
		public static bool AT_SetRotation(Framework.Core.AInstanceAble pPointer, VariableQuaternion rot, VariableBool bLocal)
		{
			if(rot== null || bLocal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetRotation(rot.mValue,bLocal.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(372460568,"SetForward",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableVector3),"forward",false, null,null)]
#endif
		public static bool AT_SetForward(Framework.Core.AInstanceAble pPointer, VariableVector3 forward)
		{
			if(forward== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetForward(forward.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1194349079,"SetUp",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableVector3),"up",false, null,null)]
#endif
		public static bool AT_SetUp(Framework.Core.AInstanceAble pPointer, VariableVector3 up)
		{
			if(up== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetUp(up.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1326731487,"SetEulerAngle",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableVector3),"eulerAngles",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bLocal",false, null,null)]
#endif
		public static bool AT_SetEulerAngle(Framework.Core.AInstanceAble pPointer, VariableVector3 eulerAngles, VariableBool bLocal)
		{
			if(eulerAngles== null || bLocal== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetEulerAngle(eulerAngles.mValue,bLocal.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(802805407,"SetScale",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableVector3),"scale",false, null,null)]
#endif
		public static bool AT_SetScale(Framework.Core.AInstanceAble pPointer, VariableVector3 scale)
		{
			if(scale== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetScale(scale.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1859231129,"Reset",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableBool),"bScale",false, null,null)]
#endif
		public static bool AT_Reset(Framework.Core.AInstanceAble pPointer, VariableBool bScale)
		{
			if(bScale== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.Reset(bScale.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-703615783,"SetActive",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
#endif
		public static bool AT_SetActive(Framework.Core.AInstanceAble pPointer)
		{
			pPointer.SetActive();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(805507562,"SetUnActive",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
#endif
		public static bool AT_SetUnActive(Framework.Core.AInstanceAble pPointer)
		{
			pPointer.SetUnActive();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(501735087,"SetParent",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableObject),"pParent",false, typeof(UnityEngine.Transform),null)]
		[ATMethodArgv(typeof(VariableBool),"bFollowParentLayer",false, null,null)]
#endif
		public static bool AT_SetParent(Framework.Core.AInstanceAble pPointer, VariableObject pParent, VariableBool bFollowParentLayer)
		{
			if(pParent== null || bFollowParentLayer== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetParent(pParent.ToObject<UnityEngine.Transform>(),bFollowParentLayer.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-599780727,"ClearMaterialBlock",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
#endif
		public static bool AT_ClearMaterialBlock(Framework.Core.AInstanceAble pPointer)
		{
			pPointer.ClearMaterialBlock();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(880711736,"设置颜色",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableString),"propName",false, null,null)]
		[ATMethodArgv(typeof(VariableColor),"color",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetColor(Framework.Core.AInstanceAble pPointer, VariableString propName, VariableColor color, VariableBool bBlock, VariableBool bShare, VariableInt index)
		{
			if(propName== null || color== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetColor(propName.mValue,color.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1352012916,"设置向量属性",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableString),"propName",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4),"vec4",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetVector(Framework.Core.AInstanceAble pPointer, VariableString propName, VariableVector4 vec4, VariableBool bBlock, VariableBool bShare, VariableInt index)
		{
			if(propName== null || vec4== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetVector(propName.mValue,vec4.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2089642805,"设置浮点属性",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableString),"propName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fValue",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetFloat(Framework.Core.AInstanceAble pPointer, VariableString propName, VariableFloat fValue, VariableBool bBlock, VariableBool bShare, VariableInt index)
		{
			if(propName== null || fValue== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetFloat(propName.mValue,fValue.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(902137129,"设置整形属性",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableString),"propName",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"nValue",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetInt(Framework.Core.AInstanceAble pPointer, VariableString propName, VariableInt nValue, VariableBool bBlock, VariableBool bShare, VariableInt index)
		{
			if(propName== null || nValue== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetInt(propName.mValue,nValue.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1859986116,"设置贴图属性",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableString),"propName",false, null,null)]
		[ATMethodArgv(typeof(VariableObject),"texture",false, typeof(UnityEngine.Texture),null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetTexture(Framework.Core.AInstanceAble pPointer, VariableString propName, VariableObject texture, VariableBool bBlock, VariableBool bShare, VariableInt index)
		{
			if(propName== null || texture== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetTexture(propName.mValue,texture.ToObject<UnityEngine.Texture>(),bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1251211296,"设置材质",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableObject),"material",false, typeof(UnityEngine.Material),null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAutoDestroy",false, null,null)]
#endif
		public static bool AT_SetMaterial(Framework.Core.AInstanceAble pPointer, VariableObject material, VariableInt index, VariableBool bAutoDestroy)
		{
			if(material== null || index== null || bAutoDestroy== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetMaterial(material.ToObject<UnityEngine.Material>(),index.mValue,bAutoDestroy.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-958156853,"设置材质",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableObject),"material",false, typeof(UnityEngine.Material),null)]
		[ATMethodArgv(typeof(VariableFloat),"fLerpTime",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fKeepTime",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAutoDestroy",false, null,null)]
#endif
		public static bool AT_LerpToMaterial(Framework.Core.AInstanceAble pPointer, VariableObject material, VariableFloat fLerpTime, VariableInt index, VariableFloat fKeepTime, VariableString propertyName, VariableBool bAutoDestroy)
		{
			if(material== null || fLerpTime== null || index== null || fKeepTime== null || propertyName== null || bAutoDestroy== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.LerpToMaterial(material.ToObject<UnityEngine.Material>(),fLerpTime.mValue,index.mValue,fKeepTime.mValue,propertyName.mValue,bAutoDestroy.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1247696414,"删除材质-Index",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_RemoveMaterialByIndex(Framework.Core.AInstanceAble pPointer, VariableInt index)
		{
			if(index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.RemoveMaterialByIndex(index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1914014566,"删除材质-Material",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableObject),"pMaterial",false, typeof(UnityEngine.Material),null)]
#endif
		public static bool AT_RemoveMaterial(Framework.Core.AInstanceAble pPointer, VariableObject pMaterial)
		{
			if(pMaterial== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.RemoveMaterial(pMaterial.ToObject<UnityEngine.Material>());
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(522038616,"替换Shader",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableString),"name",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"materialIndex",false, null,null)]
#endif
		public static bool AT_ReplaceShader(Framework.Core.AInstanceAble pPointer, VariableString name, VariableInt materialIndex)
		{
			if(name== null || materialIndex== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ReplaceShader(name.mValue,materialIndex.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1791680667,"EnableKeyWorld",typeof(Framework.Core.AInstanceAble))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(Framework.Core.AInstanceAble),null)]
		[ATMethodArgv(typeof(VariableString),"keyWorld",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bEnable",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"materialIndex",false, null,null)]
#endif
		public static bool AT_EnableKeyWorld(Framework.Core.AInstanceAble pPointer, VariableString keyWorld, VariableBool bEnable, VariableInt materialIndex)
		{
			if(keyWorld== null || bEnable== null || materialIndex== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.EnableKeyWorld(keyWorld.mValue,bEnable.mValue,materialIndex.mValue);
			return true;
		}
		public static bool DoAction(VariableMonoScript pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -1531199794:
				{//Framework.Core.AInstanceAble->GetTransorm
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_GetTransorm(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case 1184135236:
				{//Framework.Core.AInstanceAble->SetPosition
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetPosition(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case 1024259411:
				{//Framework.Core.AInstanceAble->SetRotation
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetRotation(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableQuaternion>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case 372460568:
				{//Framework.Core.AInstanceAble->SetForward
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetForward(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask));
				}
				case 1194349079:
				{//Framework.Core.AInstanceAble->SetUp
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetUp(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask));
				}
				case -1326731487:
				{//Framework.Core.AInstanceAble->SetEulerAngle
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetEulerAngle(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case 802805407:
				{//Framework.Core.AInstanceAble->SetScale
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetScale(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask));
				}
				case -1859231129:
				{//Framework.Core.AInstanceAble->Reset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_Reset(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
				case -703615783:
				{//Framework.Core.AInstanceAble->SetActive
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetActive(pUserClass.mValue as Framework.Core.AInstanceAble);
				}
				case 805507562:
				{//Framework.Core.AInstanceAble->SetUnActive
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetUnActive(pUserClass.mValue as Framework.Core.AInstanceAble);
				}
				case 501735087:
				{//Framework.Core.AInstanceAble->SetParent
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetParent(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case -599780727:
				{//Framework.Core.AInstanceAble->ClearMaterialBlock
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_ClearMaterialBlock(pUserClass.mValue as Framework.Core.AInstanceAble);
				}
				case 880711736:
				{//Framework.Core.AInstanceAble->SetColor
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetColor(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(5, pTask));
				}
				case 1352012916:
				{//Framework.Core.AInstanceAble->SetVector
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetVector(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(5, pTask));
				}
				case -2089642805:
				{//Framework.Core.AInstanceAble->SetFloat
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetFloat(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(5, pTask));
				}
				case 902137129:
				{//Framework.Core.AInstanceAble->SetInt
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetInt(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(5, pTask));
				}
				case -1859986116:
				{//Framework.Core.AInstanceAble->SetTexture
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetTexture(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(5, pTask));
				}
				case -1251211296:
				{//Framework.Core.AInstanceAble->SetMaterial
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_SetMaterial(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask));
				}
				case -958156853:
				{//Framework.Core.AInstanceAble->LerpToMaterial
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=6) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_LerpToMaterial(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(6, pTask));
				}
				case 1247696414:
				{//Framework.Core.AInstanceAble->RemoveMaterialByIndex
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_RemoveMaterialByIndex(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case -1914014566:
				{//Framework.Core.AInstanceAble->RemoveMaterial
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_RemoveMaterial(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask));
				}
				case 522038616:
				{//Framework.Core.AInstanceAble->ReplaceShader
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_ReplaceShader(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask));
				}
				case -1791680667:
				{//Framework.Core.AInstanceAble->EnableKeyWorld
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(!(pUserClass.mValue is Framework.Core.AInstanceAble)) return true;
					return AgentTree_AInstanceAble.AT_EnableKeyWorld(pUserClass.mValue as Framework.Core.AInstanceAble, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask));
				}
			}
			return true;
		}
	}
}
