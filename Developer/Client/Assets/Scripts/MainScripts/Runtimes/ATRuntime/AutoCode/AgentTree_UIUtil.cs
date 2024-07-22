//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI通用工具",true)]
#endif
	public static class AgentTree_UIUtil
	{
#if UNITY_EDITOR
		[ATMonoFunc(665131453,"SetSerializedUIVisible",typeof(TopGame.UI.UIUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.UI.UIUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableMonoScript),"serializer",false, typeof(TopGame.UI.UISerialized),null)]
		[ATMethodArgv(typeof(VariableBool),"bVisible",false, null,null)]
#endif
		public static bool AT_SetSerializedUIVisible(VariableMonoScript serializer,VariableBool bVisible)
		{
			if(serializer== null || bVisible== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.UI.UIUtil.SetSerializedUIVisible(serializer.ToObject<TopGame.UI.UISerialized>(),bVisible.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1711646046,"SetGraphicColor",typeof(TopGame.UI.UIUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.UI.UIUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableMonoScript),"graphic",false, typeof(UnityEngine.UI.Graphic),null)]
		[ATMethodArgv(typeof(VariableColor),"color",false, null,null)]
#endif
		public static bool AT_SetGraphicColor(VariableMonoScript graphic,VariableColor color)
		{
			if(graphic== null || color== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.UI.UIUtil.SetGraphicColor(graphic.ToObject<UnityEngine.UI.Graphic>(),color.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(2104630191,"OpenUI",typeof(TopGame.UI.UIUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.UI.UIUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"uiType",false, null,typeof(TopGame.UI.EUIType))]
		[ATMethodArgv(typeof(VariableBool),"bTips",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_OpenUI(VariableInt uiType,VariableBool bTips,VariableBool pReturn=null)
		{
			if(uiType== null || bTips== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.UI.UIUtil.OpenUI((TopGame.UI.EUIType)uiType.mValue,bTips.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-586476807,"ReplayPlayableDirector",typeof(TopGame.UI.UIUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.UI.UIUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableMonoScript),"playable",false, typeof(UnityEngine.Playables.PlayableDirector),null)]
		[ATMethodArgv(typeof(VariableBool),"rebuild",false, null,null)]
#endif
		public static bool AT_ReplayPlayableDirector(VariableMonoScript playable,VariableBool rebuild)
		{
			if(playable== null || rebuild== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.UI.UIUtil.ReplayPlayableDirector(playable.ToObject<UnityEngine.Playables.PlayableDirector>(),rebuild.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1173665642,"SetLabel",typeof(TopGame.UI.UIUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.UI.UIUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableMonoScript),"text",false, typeof(UnityEngine.UI.Text),null)]
		[ATMethodArgv(typeof(VariableInt),"textId",false, null,null)]
#endif
		public static bool AT_SetLabel(VariableMonoScript text,VariableInt textId)
		{
			if(text== null || textId== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.UI.UIUtil.SetLabel(text.ToObject<UnityEngine.UI.Text>(),(uint)textId.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1397948150,"SetLabel_1",typeof(TopGame.UI.UIUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.UI.UIUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableMonoScript),"text",false, typeof(UnityEngine.UI.Text),null)]
		[ATMethodArgv(typeof(VariableString),"str",false, null,null)]
#endif
		public static bool AT_SetLabel_1(VariableMonoScript text,VariableString str)
		{
			if(text== null || str== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.UI.UIUtil.SetLabel(text.ToObject<UnityEngine.UI.Text>(),str.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1848696326,"PlayTween",typeof(TopGame.UI.UIUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.UI.UIUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableMonoScript),"tween",false, typeof(UnityEngine.MonoBehaviour),null)]
		[ATMethodArgv(typeof(VariableBool),"bRewind",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_PlayTween(VariableMonoScript tween,VariableBool bRewind,VariableInt index)
		{
			if(tween== null || bRewind== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.UI.UIUtil.PlayTween(tween.ToObject<UnityEngine.MonoBehaviour>(),bRewind.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(872842540,"RebuildLayout",typeof(TopGame.UI.UIUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.UI.UIUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"transform",false, typeof(UnityEngine.RectTransform),null)]
#endif
		public static bool AT_RebuildLayout(VariableObject transform)
		{
			if(transform== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.UI.UIUtil.RebuildLayout(transform.ToObject<UnityEngine.RectTransform>());
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 665131453:
				{//TopGame.UI.UIUtil->SetSerializedUIVisible
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_UIUtil.AT_SetSerializedUIVisible(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case 1711646046:
				{//TopGame.UI.UIUtil->SetGraphicColor
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_UIUtil.AT_SetGraphicColor(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(2, pTask));
				}
				case 2104630191:
				{//TopGame.UI.UIUtil->OpenUI
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_UIUtil.AT_OpenUI(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -586476807:
				{//TopGame.UI.UIUtil->ReplayPlayableDirector
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_UIUtil.AT_ReplayPlayableDirector(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask));
				}
				case 1173665642:
				{//TopGame.UI.UIUtil->SetLabel
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_UIUtil.AT_SetLabel(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask));
				}
				case 1397948150:
				{//TopGame.UI.UIUtil->SetLabel_1
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_UIUtil.AT_SetLabel_1(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask));
				}
				case -1848696326:
				{//TopGame.UI.UIUtil->PlayTween
					if(pAction.inArgvs.Length <=3) return true;
					return AgentTree_UIUtil.AT_PlayTween(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask));
				}
				case 872842540:
				{//TopGame.UI.UIUtil->RebuildLayout
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_UIUtil.AT_RebuildLayout(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask));
				}
			}
			return true;
		}
	}
}
