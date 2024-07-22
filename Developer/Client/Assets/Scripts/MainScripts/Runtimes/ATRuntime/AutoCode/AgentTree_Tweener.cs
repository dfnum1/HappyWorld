//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("晃动系统/晃动组件",true)]
#endif
	public static class AgentTree_Tweener
	{
#if UNITY_EDITOR
		[ATMonoFunc(823841711,"IsPlaying",typeof(TopGame.RtgTween.Tweener))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.RtgTween.Tweener),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsPlaying(TopGame.RtgTween.Tweener pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsPlaying();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1425730982,"PlayTween",typeof(TopGame.RtgTween.Tweener))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.RtgTween.Tweener),null)]
		[ATMethodArgv(typeof(VariableFloat),"fDelay",false, null,null)]
#endif
		public static bool AT_PlayTween(TopGame.RtgTween.Tweener pPointer, VariableFloat fDelay)
		{
			if(fDelay== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.PlayTween(fDelay.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1544577910,"ForcePlayTween",typeof(TopGame.RtgTween.Tweener))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.RtgTween.Tweener),null)]
		[ATMethodArgv(typeof(VariableFloat),"fDelay",false, null,null)]
#endif
		public static bool AT_ForcePlayTween(TopGame.RtgTween.Tweener pPointer, VariableFloat fDelay)
		{
			if(fDelay== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ForcePlayTween(fDelay.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(362508259,"StopTween",typeof(TopGame.RtgTween.Tweener))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.RtgTween.Tweener),null)]
#endif
		public static bool AT_StopTween(TopGame.RtgTween.Tweener pPointer)
		{
			pPointer.StopTween();
			return true;
		}
		public static bool DoAction(VariableMonoScript pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 823841711:
				{//TopGame.RtgTween.Tweener->IsPlaying
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.RtgTween.Tweener)) return true;
					return AgentTree_Tweener.AT_IsPlaying(pUserClass.mValue as TopGame.RtgTween.Tweener, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 1425730982:
				{//TopGame.RtgTween.Tweener->PlayTween
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.RtgTween.Tweener)) return true;
					return AgentTree_Tweener.AT_PlayTween(pUserClass.mValue as TopGame.RtgTween.Tweener, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case 1544577910:
				{//TopGame.RtgTween.Tweener->ForcePlayTween
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.RtgTween.Tweener)) return true;
					return AgentTree_Tweener.AT_ForcePlayTween(pUserClass.mValue as TopGame.RtgTween.Tweener, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
				case 362508259:
				{//TopGame.RtgTween.Tweener->StopTween
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.RtgTween.Tweener)) return true;
					return AgentTree_Tweener.AT_StopTween(pUserClass.mValue as TopGame.RtgTween.Tweener);
				}
			}
			return true;
		}
	}
}
