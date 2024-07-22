//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("ActorParamter",true)]
#endif
	public static class AgentTree_ActorParameter
	{
#if UNITY_EDITOR
		[ATMonoFunc(51290915,"GetSkill",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
#endif
		public static bool AT_GetSkill(Framework.Core.ActorParameter pPointer)
		{
			pPointer.GetSkill();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1157948153,"GetActorType",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,typeof(Framework.Core.EActorType))]
#endif
		public static bool AT_GetActorType(Framework.Core.ActorParameter pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetActorType();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1646839704,"SetActorID",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"guActorID",false, null,null)]
#endif
		public static bool AT_SetActorID(Framework.Core.ActorParameter pPointer, VariableInt guActorID)
		{
			if(guActorID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetActorID(guActorID.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2134776207,"GetActorID",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetActorID(Framework.Core.ActorParameter pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetActorID();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(755826881,"SetClassify",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableByte),"classify",false, null,null)]
#endif
		public static bool AT_SetClassify(Framework.Core.ActorParameter pPointer, VariableByte classify)
		{
			if(classify== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetClassify(classify.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1347125581,"GetClassify",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableByte),"pReturn",null,null)]
#endif
		public static bool AT_GetClassify(Framework.Core.ActorParameter pPointer, VariableByte pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetClassify();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-584972700,"IsElement",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"element",false, null,typeof(Framework.Core.EElementType))]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsElement(Framework.Core.ActorParameter pPointer, VariableInt element, VariableBool pReturn=null)
		{
			if(element== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsElement((Framework.Core.EElementType)element.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(237601304,"SetCareerID",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableByte),"dwCareerID",false, null,null)]
#endif
		public static bool AT_SetCareerID(Framework.Core.ActorParameter pPointer, VariableByte dwCareerID)
		{
			if(dwCareerID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetCareerID(dwCareerID.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1403348922,"GetCareerID",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableByte),"pReturn",null,null)]
#endif
		public static bool AT_GetCareerID(Framework.Core.ActorParameter pPointer, VariableByte pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetCareerID();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1560158775,"SetDutyID",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableByte),"dutyId",false, null,null)]
#endif
		public static bool AT_SetDutyID(Framework.Core.ActorParameter pPointer, VariableByte dutyId)
		{
			if(dutyId== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetDutyID(dutyId.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1981728411,"GetDutyID",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableByte),"pReturn",null,null)]
#endif
		public static bool AT_GetDutyID(Framework.Core.ActorParameter pPointer, VariableByte pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetDutyID();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(996557793,"SetCareerWeakness",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"dwWeakness",false, null,null)]
#endif
		public static bool AT_SetCareerWeakness(Framework.Core.ActorParameter pPointer, VariableInt dwWeakness)
		{
			if(dwWeakness== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetCareerWeakness((ushort)dwWeakness.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(540480927,"GetCareerWeakness",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetCareerWeakness(Framework.Core.ActorParameter pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetCareerWeakness();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1911841323,"SetRoleGUID",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableLong),"nRoleGUID",false, null,null)]
#endif
		public static bool AT_SetRoleGUID(Framework.Core.ActorParameter pPointer, VariableLong nRoleGUID)
		{
			if(nRoleGUID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetRoleGUID(nRoleGUID.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(241641066,"GetRoleGUID",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableLong),"pReturn",null,null)]
#endif
		public static bool AT_GetRoleGUID(Framework.Core.ActorParameter pPointer, VariableLong pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetRoleGUID();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(89875509,"GetQuality",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableByte),"pReturn",null,null)]
#endif
		public static bool AT_GetQuality(Framework.Core.ActorParameter pPointer, VariableByte pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetQuality();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1737352074,"GetMaxLevel",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetMaxLevel(Framework.Core.ActorParameter pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetMaxLevel();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-686842736,"GetLevel",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetLevel(Framework.Core.ActorParameter pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetLevel();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(949693102,"SetName",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableString),"strName",false, null,null)]
#endif
		public static bool AT_SetName(Framework.Core.ActorParameter pPointer, VariableString strName)
		{
			if(strName== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetName(strName.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1889457625,"GetName",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableString),"pReturn",null,null)]
#endif
		public static bool AT_GetName(Framework.Core.ActorParameter pPointer, VariableString pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetName();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(744227636,"IsShielded",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsShielded(Framework.Core.ActorParameter pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsShielded();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1543552952,"GetShieldedTime",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetShieldedTime(Framework.Core.ActorParameter pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetShieldedTime();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-206240772,"SetModelHeight",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableFloat),"fHeight",false, null,null)]
#endif
		public static bool AT_SetModelHeight(Framework.Core.ActorParameter pPointer, VariableFloat fHeight)
		{
			if(fHeight== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetModelHeight(fHeight.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1362761195,"GetModelHeight",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetModelHeight(Framework.Core.ActorParameter pPointer, VariableFloat pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetModelHeight();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(413264564,"IsBoydPartBroken",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"nBodyPart",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsBoydPartBroken(Framework.Core.ActorParameter pPointer, VariableInt nBodyPart, VariableBool pReturn=null)
		{
			if(nBodyPart== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsBoydPartBroken((uint)nBodyPart.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1985406151,"DoHpDrop",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"fHp",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"nBodyPartID",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bEvent",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"externVarial",false, typeof(Framework.Core.VariablePoolAble),null)]
		[ATMethodArgv(typeof(VariableBool),"bCheckKilled",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShielded",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_DoHpDrop(Framework.Core.ActorParameter pPointer, VariableInt fHp, VariableInt nBodyPartID, VariableBool bEvent, VariableUser externVarial, VariableBool bCheckKilled, VariableBool bShielded, VariableInt pReturn=null)
		{
			if(fHp== null || nBodyPartID== null || bEvent== null || externVarial== null || bCheckKilled== null || bShielded== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.DoHpDrop(fHp.mValue,(uint)nBodyPartID.mValue,bEvent.mValue,(Framework.Core.VariablePoolAble)externVarial.mValue,bCheckKilled.mValue,bShielded.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(233586886,"AppendHP",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"fValue",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bEvent",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"externVarial",false, typeof(Framework.Core.VariablePoolAble),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_AppendHP(Framework.Core.ActorParameter pPointer, VariableInt fValue, VariableBool bEvent, VariableUser externVarial, VariableInt pReturn=null)
		{
			if(fValue== null || bEvent== null || externVarial== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.AppendHP(fValue.mValue,bEvent.mValue,(Framework.Core.VariablePoolAble)externVarial.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(423197503,"ClearAttrStat",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(Framework.Core.EAttrType))]
		[ATMethodArgv(typeof(VariableBool),"bAdd",false, null,null)]
#endif
		public static bool AT_ClearAttrStat(Framework.Core.ActorParameter pPointer, VariableInt type, VariableBool bAdd)
		{
			if(type== null || bAdd== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ClearAttrStat((Framework.Core.EAttrType)type.mValue,bAdd.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-327976723,"GetStatAttr",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(Framework.Core.EAttrType))]
		[ATMethodArgv(typeof(VariableBool),"bAdd",false, null,null)]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetStatAttr(Framework.Core.ActorParameter pPointer, VariableInt type, VariableBool bAdd, VariableFloat pReturn=null)
		{
			if(type== null || bAdd== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetStatAttr((Framework.Core.EAttrType)type.mValue,bAdd.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(218770426,"AppendShield",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"fValue",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_AppendShield(Framework.Core.ActorParameter pPointer, VariableInt fValue, VariableInt pReturn=null)
		{
			if(fValue== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.AppendShield(fValue.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1248007065,"AppendShieldDuration",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableFloat),"fDuration",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAmount",false, null,null)]
#endif
		public static bool AT_AppendShieldDuration(Framework.Core.ActorParameter pPointer, VariableFloat fDuration, VariableBool bAmount)
		{
			if(fDuration== null || bAmount== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.AppendShieldDuration(fDuration.mValue,bAmount.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-274496393,"DoSpCost",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"fSp",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bEvent",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"externVarial",false, typeof(Framework.Core.VariablePoolAble),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_DoSpCost(Framework.Core.ActorParameter pPointer, VariableInt fSp, VariableBool bEvent, VariableUser externVarial, VariableInt pReturn=null)
		{
			if(fSp== null || bEvent== null || externVarial== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.DoSpCost(fSp.mValue,bEvent.mValue,(Framework.Core.VariablePoolAble)externVarial.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-118626230,"AppendSP",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"fValue",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bEvent",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"externVarial",false, typeof(Framework.Core.VariablePoolAble),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_AppendSP(Framework.Core.ActorParameter pPointer, VariableInt fValue, VariableBool bEvent, VariableUser externVarial, VariableInt pReturn=null)
		{
			if(fValue== null || bEvent== null || externVarial== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.AppendSP(fValue.mValue,bEvent.mValue,(Framework.Core.VariablePoolAble)externVarial.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1329042990,"是否有架势条",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_HasPosture(Framework.Core.ActorParameter pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.HasPosture();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(917025435,"是否虚落",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsExhaustioning(Framework.Core.ActorParameter pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsExhaustioning();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1865885123,"DropPosture",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"posuter",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bEvent",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"externVarial",false, typeof(Framework.Core.VariablePoolAble),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_DropPosture(Framework.Core.ActorParameter pPointer, VariableInt posuter, VariableBool bEvent, VariableUser externVarial, VariableInt pReturn=null)
		{
			if(posuter== null || bEvent== null || externVarial== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.DropPosture(posuter.mValue,bEvent.mValue,(Framework.Core.VariablePoolAble)externVarial.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(326899574,"AppendPosture",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"fValue",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bEvent",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"externVarial",false, typeof(Framework.Core.VariablePoolAble),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_AppendPosture(Framework.Core.ActorParameter pPointer, VariableInt fValue, VariableBool bEvent, VariableUser externVarial, VariableInt pReturn=null)
		{
			if(fValue== null || bEvent== null || externVarial== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.AppendPosture(fValue.mValue,bEvent.mValue,(Framework.Core.VariablePoolAble)externVarial.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1623966774,"GetBaseAttr",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"nType",false, null,typeof(Framework.Core.EAttrType))]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetBaseAttr(Framework.Core.ActorParameter pPointer, VariableInt nType, VariableFloat pReturn=null)
		{
			if(nType== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetBaseAttr((Framework.Core.EAttrType)nType.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1142089227,"GetExternAttr",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"nType",false, null,typeof(Framework.Core.EBuffAttrType))]
		[ATMethodReturn(typeof(VariableFloat),"pReturn",null,null)]
#endif
		public static bool AT_GetExternAttr(Framework.Core.ActorParameter pPointer, VariableInt nType, VariableFloat pReturn=null)
		{
			if(nType== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetExternAttr((Framework.Core.EBuffAttrType)nType.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-613243463,"UpdataAttr",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
#endif
		public static bool AT_UpdataAttr(Framework.Core.ActorParameter pPointer)
		{
			pPointer.UpdataAttr();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(146676832,"AddpendBaseAttr",typeof(Framework.Core.ActorParameter))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(Framework.Core.ActorParameter),null)]
		[ATMethodArgv(typeof(VariableInt),"Type",false, null,typeof(Framework.Core.EAttrType))]
		[ATMethodArgv(typeof(VariableFloat),"fValue",false, null,null)]
#endif
		public static bool AT_AddpendBaseAttr(Framework.Core.ActorParameter pPointer, VariableInt Type, VariableFloat fValue)
		{
			if(Type== null || fValue== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.AddpendBaseAttr((Framework.Core.EAttrType)Type.mValue,fValue.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 51290915:
				{//Framework.Core.ActorParameter->GetSkill
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetSkill(pUserClass.mValue as Framework.Core.ActorParameter);
				}
				case 1157948153:
				{//Framework.Core.ActorParameter->GetActorType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetActorType(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -1646839704:
				{//Framework.Core.ActorParameter->SetActorID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_SetActorID(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case -2134776207:
				{//Framework.Core.ActorParameter->GetActorID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetActorID(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 755826881:
				{//Framework.Core.ActorParameter->SetClassify
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_SetClassify(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableByte>(1, pTask));
				}
				case 1347125581:
				{//Framework.Core.ActorParameter->GetClassify
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetClassify(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableByte>(0, pTask));
				}
				case -584972700:
				{//Framework.Core.ActorParameter->IsElement
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_IsElement(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 237601304:
				{//Framework.Core.ActorParameter->SetCareerID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_SetCareerID(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableByte>(1, pTask));
				}
				case -1403348922:
				{//Framework.Core.ActorParameter->GetCareerID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetCareerID(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableByte>(0, pTask));
				}
				case -1560158775:
				{//Framework.Core.ActorParameter->SetDutyID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_SetDutyID(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableByte>(1, pTask));
				}
				case -1981728411:
				{//Framework.Core.ActorParameter->GetDutyID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetDutyID(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableByte>(0, pTask));
				}
				case 996557793:
				{//Framework.Core.ActorParameter->SetCareerWeakness
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_SetCareerWeakness(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 540480927:
				{//Framework.Core.ActorParameter->GetCareerWeakness
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetCareerWeakness(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -1911841323:
				{//Framework.Core.ActorParameter->SetRoleGUID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_SetRoleGUID(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableLong>(1, pTask));
				}
				case 241641066:
				{//Framework.Core.ActorParameter->GetRoleGUID
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetRoleGUID(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableLong>(0, pTask));
				}
				case 89875509:
				{//Framework.Core.ActorParameter->GetQuality
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetQuality(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableByte>(0, pTask));
				}
				case -1737352074:
				{//Framework.Core.ActorParameter->GetMaxLevel
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetMaxLevel(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -686842736:
				{//Framework.Core.ActorParameter->GetLevel
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetLevel(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 949693102:
				{//Framework.Core.ActorParameter->SetName
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_SetName(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask));
				}
				case 1889457625:
				{//Framework.Core.ActorParameter->GetName
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetName(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableString>(0, pTask));
				}
				case 744227636:
				{//Framework.Core.ActorParameter->IsShielded
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_IsShielded(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -1543552952:
				{//Framework.Core.ActorParameter->GetShieldedTime
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetShieldedTime(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case -206240772:
				{//Framework.Core.ActorParameter->SetModelHeight
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_SetModelHeight(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case 1362761195:
				{//Framework.Core.ActorParameter->GetModelHeight
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetModelHeight(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case 413264564:
				{//Framework.Core.ActorParameter->IsBoydPartBroken
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_IsBoydPartBroken(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 1985406151:
				{//Framework.Core.ActorParameter->DoHpDrop
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=6) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_DoHpDrop(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(6, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 233586886:
				{//Framework.Core.ActorParameter->AppendHP
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_AppendHP(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 423197503:
				{//Framework.Core.ActorParameter->ClearAttrStat
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_ClearAttrStat(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case -327976723:
				{//Framework.Core.ActorParameter->GetStatAttr
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetStatAttr(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case 218770426:
				{//Framework.Core.ActorParameter->AppendShield
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_AppendShield(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -1248007065:
				{//Framework.Core.ActorParameter->AppendShieldDuration
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_AppendShieldDuration(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case -274496393:
				{//Framework.Core.ActorParameter->DoSpCost
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_DoSpCost(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -118626230:
				{//Framework.Core.ActorParameter->AppendSP
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_AppendSP(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 1329042990:
				{//Framework.Core.ActorParameter->HasPosture
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_HasPosture(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 917025435:
				{//Framework.Core.ActorParameter->IsExhaustioning
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_IsExhaustioning(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 1865885123:
				{//Framework.Core.ActorParameter->DropPosture
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_DropPosture(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 326899574:
				{//Framework.Core.ActorParameter->AppendPosture
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_AppendPosture(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 1623966774:
				{//Framework.Core.ActorParameter->GetBaseAttr
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetBaseAttr(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case -1142089227:
				{//Framework.Core.ActorParameter->GetExternAttr
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_GetExternAttr(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableFloat>(0, pTask));
				}
				case -613243463:
				{//Framework.Core.ActorParameter->UpdataAttr
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_UpdataAttr(pUserClass.mValue as Framework.Core.ActorParameter);
				}
				case 146676832:
				{//Framework.Core.ActorParameter->AddpendBaseAttr
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is Framework.Core.ActorParameter)) return true;
					return AgentTree_ActorParameter.AT_AddpendBaseAttr(pUserClass.mValue as Framework.Core.ActorParameter, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask));
				}
			}
			return true;
		}
	}
}
