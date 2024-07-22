//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("晃动系统/晃动组",true)]
#endif
	public static class AgentTree_TweenerGroup
	{
#if UNITY_EDITOR
		[ATMonoFunc(106153887,"isPlaying",typeof(TopGame.RtgTween.TweenerGroup))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.RtgTween.TweenerGroup),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_isPlaying(TopGame.RtgTween.TweenerGroup pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.isPlaying();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1659186380,"ForcePlay",typeof(TopGame.RtgTween.TweenerGroup))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.RtgTween.TweenerGroup),null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_ForcePlay(TopGame.RtgTween.TweenerGroup pPointer, VariableInt index)
		{
			if(index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ForcePlay((short)index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1687918740,"Play",typeof(TopGame.RtgTween.TweenerGroup))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.RtgTween.TweenerGroup),null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bForce",false, null,null)]
#endif
		public static bool AT_Play(TopGame.RtgTween.TweenerGroup pPointer, VariableInt index, VariableBool bForce)
		{
			if(index== null || bForce== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.Play((short)index.mValue,bForce.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1908055081,"Stop",typeof(TopGame.RtgTween.TweenerGroup))]
		[ATMethodArgv(typeof(VariableMonoScript),"pPointer",false,  typeof(TopGame.RtgTween.TweenerGroup),null)]
#endif
		public static bool AT_Stop(TopGame.RtgTween.TweenerGroup pPointer)
		{
			pPointer.Stop();
			return true;
		}
		public static bool DoAction(VariableMonoScript pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 106153887:
				{//TopGame.RtgTween.TweenerGroup->isPlaying
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.RtgTween.TweenerGroup)) return true;
					return AgentTree_TweenerGroup.AT_isPlaying(pUserClass.mValue as TopGame.RtgTween.TweenerGroup, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -1659186380:
				{//TopGame.RtgTween.TweenerGroup->ForcePlay
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.RtgTween.TweenerGroup)) return true;
					return AgentTree_TweenerGroup.AT_ForcePlay(pUserClass.mValue as TopGame.RtgTween.TweenerGroup, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case -1687918740:
				{//TopGame.RtgTween.TweenerGroup->Play
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is TopGame.RtgTween.TweenerGroup)) return true;
					return AgentTree_TweenerGroup.AT_Play(pUserClass.mValue as TopGame.RtgTween.TweenerGroup, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case 1908055081:
				{//TopGame.RtgTween.TweenerGroup->Stop
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.RtgTween.TweenerGroup)) return true;
					return AgentTree_TweenerGroup.AT_Stop(pUserClass.mValue as TopGame.RtgTween.TweenerGroup);
				}
			}
			return true;
		}
	}
}
