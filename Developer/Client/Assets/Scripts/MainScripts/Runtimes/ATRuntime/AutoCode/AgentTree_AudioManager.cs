//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("音乐系统",true)]
#endif
	public static class AgentTree_AudioManager
	{
#if UNITY_EDITOR
		[ATMonoFunc(998138295,"停止音效",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nID",false, null,null)]
#endif
		public static bool AT_StopEffect(VariableInt nID)
		{
			if(nID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.StopEffect(nID.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-635419129,"FadeOut音效",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nID",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fFadeTime",false, null,null)]
#endif
		public static bool AT_FadeOutEffect(VariableInt nID,VariableFloat fFadeTime)
		{
			if(nID== null || fFadeTime== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.FadeOutEffect(nID.mValue,fFadeTime.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-745221383,"暂停音效",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nID",false, null,null)]
#endif
		public static bool AT_PauseEffect(VariableInt nID)
		{
			if(nID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.PauseEffect(nID.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1151116777,"恢复音效",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nID",false, null,null)]
#endif
		public static bool AT_ResumeEffect(VariableInt nID)
		{
			if(nID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.ResumeEffect(nID.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1438782140,"播放FMOD声音-3D",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strEvent",false, null,null)]
		[ATMethodArgv(typeof(VariableObject),"pGo",false, typeof(UnityEngine.GameObject),null)]
#endif
		public static bool AT_PlayFMOD3D(VariableString strEvent,VariableObject pGo)
		{
			if(strEvent== null || pGo== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.PlayFMOD3D(strEvent.mValue,pGo.ToObject<UnityEngine.GameObject>());
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1401625004,"播放音效",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.AudioClip))]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_PlayEffect(VariableString strFile,VariableInt pReturn=null)
		{
			if(strFile== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.PlayEffect(strFile.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-913102846,"播放音效",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.AudioClip))]
		[ATMethodArgv(typeof(VariableObject),"pTrans",false, typeof(UnityEngine.Transform),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_Play3DEffect(VariableString strFile,VariableObject pTrans,VariableInt pReturn=null)
		{
			if(strFile== null || pTrans== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.Play3DEffect(strFile.mValue,pTrans.ToObject<UnityEngine.Transform>());
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(2005546597,"播放音效-Volume",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.AudioClip))]
		[ATMethodArgv(typeof(VariableFloat),"fVolume",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_PlayEffectVolume(VariableString strFile,VariableFloat fVolume,VariableInt pReturn=null)
		{
			if(strFile== null || fVolume== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.PlayEffectVolume(strFile.mValue,fVolume.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1757583559,"播放3D音效-Transform",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.AudioClip))]
		[ATMethodArgv(typeof(VariableFloat),"fVolume",false, null,null)]
		[ATMethodArgv(typeof(VariableObject),"pTrans",false, typeof(UnityEngine.Transform),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_Play3DEffectVolume(VariableString strFile,VariableFloat fVolume,VariableObject pTrans,VariableInt pReturn=null)
		{
			if(strFile== null || fVolume== null || pTrans== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.Play3DEffectVolume(strFile.mValue,fVolume.mValue,pTrans.ToObject<UnityEngine.Transform>());
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(303985431,"播放3D音效-Position",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.AudioClip))]
		[ATMethodArgv(typeof(VariableFloat),"fVolume",false, null,null)]
		[ATMethodArgv(typeof(VariableVector3),"position",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_PlayEffectVolume_1(VariableString strFile,VariableFloat fVolume,VariableVector3 position,VariableInt pReturn=null)
		{
			if(strFile== null || fVolume== null || position== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.PlayEffectVolume(strFile.mValue,fVolume.mValue,position.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2027702129,"播放音效-Transform",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nId",false, null,typeof(UnityEngine.AudioClip))]
		[ATMethodArgv(typeof(VariableObject),"pTrans",false, typeof(UnityEngine.Transform),null)]
		[ATMethodArgv(typeof(VariableBool),"bMix",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_PlayID(VariableInt nId,VariableObject pTrans,VariableBool bMix,VariableInt pReturn=null)
		{
			if(nId== null || pTrans== null || bMix== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.PlayID((uint)nId.mValue,pTrans.ToObject<UnityEngine.Transform>(),bMix.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(953814572,"播放音效-Position",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nId",false, null,typeof(UnityEngine.AudioClip))]
		[ATMethodArgv(typeof(VariableVector3),"position",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bMix",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_PlayID_1(VariableInt nId,VariableVector3 position,VariableBool bMix,VariableInt pReturn=null)
		{
			if(nId== null || position== null || bMix== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.PlayID((uint)nId.mValue,position.mValue,bMix.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(826377619,"播放音效(clip)",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"clip",false, typeof(UnityEngine.AudioClip),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_PlayEffect_1(VariableObject clip,VariableInt pReturn=null)
		{
			if(clip== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.PlayEffect(clip.ToObject<UnityEngine.AudioClip>());
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(527768922,"停止所有",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableBool),"bBG",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bEffect",false, null,null)]
#endif
		public static bool AT_StopAll(VariableBool bBG,VariableBool bEffect)
		{
			if(bBG== null || bEffect== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.StopAll(bBG.mValue,bEffect.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-554106939,"FadeOut所有",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableFloat),"fadeOutBG",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fadeOutEffect",false, null,null)]
#endif
		public static bool AT_FadeOutAll(VariableFloat fadeOutBG,VariableFloat fadeOutEffect)
		{
			if(fadeOutBG== null || fadeOutEffect== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.FadeOutAll(fadeOutBG.mValue,fadeOutEffect.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1806843610,"停止背景音乐",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nID",false, null,null)]
#endif
		public static bool AT_StopBG(VariableInt nID)
		{
			if(nID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.StopBG(nID.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1108765933,"FadeOut背景音乐",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nID",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fFadeTime",false, null,null)]
#endif
		public static bool AT_FadeOutBG(VariableInt nID,VariableFloat fFadeTime)
		{
			if(nID== null || fFadeTime== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.FadeOutBG(nID.mValue,fFadeTime.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-970270962,"FadeOut(Name)背景音乐",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fFadeTime",false, null,null)]
#endif
		public static bool AT_FadeOutBGByName(VariableString strName,VariableFloat fFadeTime)
		{
			if(strName== null || fFadeTime== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.FadeOutBGByName(strName.mValue,fFadeTime.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1557254816,"暂停背景音乐",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nID",false, null,null)]
#endif
		public static bool AT_PauseBG(VariableInt nID)
		{
			if(nID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.PauseBG(nID.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1734329653,"恢复背景音乐",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"nID",false, null,null)]
#endif
		public static bool AT_ResumeBG(VariableInt nID)
		{
			if(nID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.ResumeBG(nID.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-109158738,"播放背景音乐(clip)",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"clip",false, typeof(UnityEngine.AudioClip),null)]
		[ATMethodArgv(typeof(VariableBool),"bStopAllBG",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bLoop",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"fastName",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_PlayBG(VariableObject clip,VariableBool bStopAllBG,VariableBool bLoop,VariableString fastName,VariableInt pReturn=null)
		{
			if(clip== null || bStopAllBG== null || bLoop== null || fastName== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.PlayBG(clip.ToObject<UnityEngine.AudioClip>(),bStopAllBG.mValue,bLoop.mValue,fastName.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1206360201,"播放背景音乐",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.AudioClip))]
		[ATMethodArgv(typeof(VariableBool),"bStopAllBG",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bLoop",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"fastName",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_PlayBG_1(VariableString strFile,VariableBool bStopAllBG,VariableBool bLoop,VariableString fastName,VariableInt pReturn=null)
		{
			if(strFile== null || bStopAllBG== null || bLoop== null || fastName== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.PlayBG(strFile.mValue,bStopAllBG.mValue,bLoop.mValue,fastName.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(703450180,"混合背景音乐",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.AudioClip))]
		[ATMethodArgv(typeof(VariableBool),"bLoop",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"fastName",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"mixGroup",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_MixBG(VariableString strFile,VariableBool bLoop,VariableString fastName,VariableInt mixGroup,VariableInt pReturn=null)
		{
			if(strFile== null || bLoop== null || fastName== null || mixGroup== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.MixBG(strFile.mValue,bLoop.mValue,fastName.mValue,mixGroup.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(661362780,"播放背景音乐(Fade)",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.AudioClip))]
		[ATMethodArgv(typeof(VariableCurve),"fadeCurve",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAllBGFadeStop",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bLoop",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"fastName",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_PlayBG_2(VariableString strFile,VariableCurve fadeCurve,VariableBool bAllBGFadeStop,VariableBool bLoop,VariableString fastName,VariableInt pReturn=null)
		{
			if(strFile== null || fadeCurve== null || bAllBGFadeStop== null || bLoop== null || fastName== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.PlayBG(strFile.mValue,fadeCurve.mValue,bAllBGFadeStop.mValue,bLoop.mValue,fastName.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-74490072,"混和背景音乐(Fade)",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"strFile",false, null,typeof(UnityEngine.AudioClip))]
		[ATMethodArgv(typeof(VariableCurve),"fadeCurve",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bLoop",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"fastName",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"mixGroup",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_MixBG_1(VariableString strFile,VariableCurve fadeCurve,VariableBool bLoop,VariableString fastName,VariableInt mixGroup,VariableInt pReturn=null)
		{
			if(strFile== null || fadeCurve== null || bLoop== null || fastName== null || mixGroup== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Core.AudioManager.MixBG(strFile.mValue,fadeCurve.mValue,bLoop.mValue,fastName.mValue,mixGroup.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(32219195,"设置背景音乐参数",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"parameter",false, null,null)]
#endif
		public static bool AT_SetBGParameter(VariableInt parameter)
		{
			if(parameter== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.SetBGParameter(parameter.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1943526142,"设置背景音乐参数-Label",typeof(TopGame.Core.AudioManager))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false, typeof(TopGame.Core.AudioManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"parameter",false, null,null)]
#endif
		public static bool AT_SetBGParameterLabel(VariableString parameter)
		{
			if(parameter== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.Core.AudioManager.SetBGParameterLabel(parameter.mValue);
			return true;
		}
		public static bool DoAction(VariableMonoScript pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 998138295:
				{//TopGame.Core.AudioManager->StopEffect
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_AudioManager.AT_StopEffect(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case -635419129:
				{//TopGame.Core.AudioManager->FadeOutEffect
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_AudioManager.AT_FadeOutEffect(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask));
				}
				case -745221383:
				{//TopGame.Core.AudioManager->PauseEffect
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_AudioManager.AT_PauseEffect(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 1151116777:
				{//TopGame.Core.AudioManager->ResumeEffect
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_AudioManager.AT_ResumeEffect(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case -1438782140:
				{//TopGame.Core.AudioManager->PlayFMOD3D
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_AudioManager.AT_PlayFMOD3D(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(2, pTask));
				}
				case 1401625004:
				{//TopGame.Core.AudioManager->PlayEffect
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_PlayEffect(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -913102846:
				{//TopGame.Core.AudioManager->Play3DEffect
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_Play3DEffect(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(2, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 2005546597:
				{//TopGame.Core.AudioManager->PlayEffectVolume
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_PlayEffectVolume(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -1757583559:
				{//TopGame.Core.AudioManager->Play3DEffectVolume
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_Play3DEffectVolume(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(3, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 303985431:
				{//TopGame.Core.AudioManager->PlayEffectVolume_1
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_PlayEffectVolume_1(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(3, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -2027702129:
				{//TopGame.Core.AudioManager->PlayID
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_PlayID(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 953814572:
				{//TopGame.Core.AudioManager->PlayID_1
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_PlayID_1(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 826377619:
				{//TopGame.Core.AudioManager->PlayEffect_1
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_PlayEffect_1(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 527768922:
				{//TopGame.Core.AudioManager->StopAll
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_AudioManager.AT_StopAll(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case -554106939:
				{//TopGame.Core.AudioManager->FadeOutAll
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_AudioManager.AT_FadeOutAll(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask));
				}
				case -1806843610:
				{//TopGame.Core.AudioManager->StopBG
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_AudioManager.AT_StopBG(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case -1108765933:
				{//TopGame.Core.AudioManager->FadeOutBG
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_AudioManager.AT_FadeOutBG(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask));
				}
				case -970270962:
				{//TopGame.Core.AudioManager->FadeOutBGByName
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_AudioManager.AT_FadeOutBGByName(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask));
				}
				case -1557254816:
				{//TopGame.Core.AudioManager->PauseBG
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_AudioManager.AT_PauseBG(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case -1734329653:
				{//TopGame.Core.AudioManager->ResumeBG
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_AudioManager.AT_ResumeBG(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case -109158738:
				{//TopGame.Core.AudioManager->PlayBG
					if(pAction.inArgvs.Length <=4) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_PlayBG(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(4, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -1206360201:
				{//TopGame.Core.AudioManager->PlayBG_1
					if(pAction.inArgvs.Length <=4) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_PlayBG_1(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(4, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 703450180:
				{//TopGame.Core.AudioManager->MixBG
					if(pAction.inArgvs.Length <=4) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_MixBG(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(4, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 661362780:
				{//TopGame.Core.AudioManager->PlayBG_2
					if(pAction.inArgvs.Length <=5) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_PlayBG_2(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableCurve>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(5, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -74490072:
				{//TopGame.Core.AudioManager->MixBG_1
					if(pAction.inArgvs.Length <=5) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_AudioManager.AT_MixBG_1(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableCurve>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(5, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 32219195:
				{//TopGame.Core.AudioManager->SetBGParameter
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_AudioManager.AT_SetBGParameter(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 1943526142:
				{//TopGame.Core.AudioManager->SetBGParameterLabel
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_AudioManager.AT_SetBGParameterLabel(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask));
				}
			}
			return true;
		}
	}
}
