//auto generator
using UnityEngine;
namespace Framework.Plugin.Guide
{
	public class GuideWrapper
	{
		public static bool bDoing
		{
			get{return GuideSystem.getInstance().bDoing;}
		}
		public static void OnTouchBegin(TopGame.Core.TouchInput.TouchData touch)
		{
			GuideSystem.getInstance().OnTouchBegin(touch.touchID, touch.position, touch.deltaPosition);
		}
		public static void OnTouchMove(TopGame.Core.TouchInput.TouchData touch)
		{
			GuideSystem.getInstance().OnTouchMove(touch.touchID, touch.position, touch.deltaPosition);
		}
		public static void OnTouchEnd(TopGame.Core.TouchInput.TouchData touch)
		{
			GuideSystem.getInstance().OnTouchEnd(touch.touchID, touch.position, touch.deltaPosition);
		}
		public static void OnOptionStepCheck()
		{
			GuideSystem.getInstance().OverOptionState();
		}
		public static void OnUIWidgetTrigger(int widgetGuid, int listIndex, EUIWidgetTriggerType type, params Framework.Plugin.AT.IUserData[] argvs)
		{
			GuideSystem.getInstance().OnUIWidgetTrigger(widgetGuid, listIndex, type, argvs);
		}
		public static void OnCustomCallback(int customType, int userData, params Framework.Plugin.AT.IUserData[] argvs)
		{
			GuideSystem.getInstance().OnCustomCallback(customType, userData, argvs);
		}
		public static void DoGuide(int guid, int state, BaseNode pStartNode = null, bool bForce = false)
		{
			GuideSystem.getInstance().DoGuide(guid,state,pStartNode,bForce);
		}
		public static bool OnGuide(int state)
		{
		//Framework.Plugin.Guide.GuideTriggerType.Guide
			return GuideSystem.getInstance().OnTrigger(1, null, false, state);
		}
		public static bool OnOpenUI(int uiType,int state)
		{
		//Framework.Plugin.Guide.GuideTriggerType.OpenUI
			return GuideSystem.getInstance().OnTrigger(2, null, false, uiType, state);
		}
		public static bool OnEnterGameState(int state,int mode)
		{
		//Framework.Plugin.Guide.GuideTriggerType.EnterGameState
			return GuideSystem.getInstance().OnTrigger(3, null, false, state, mode);
		}
		public static bool OnStartTask(int taskID,int state)
		{
		//Framework.Plugin.Guide.GuideTriggerType.StartTask
			return GuideSystem.getInstance().OnTrigger(4, null, false, taskID, state);
		}
		public static bool OnCompleteTask(int taskID,int state)
		{
		//Framework.Plugin.Guide.GuideTriggerType.CompleteTask
			return GuideSystem.getInstance().OnTrigger(5, null, false, taskID, state);
		}
		public static bool OnCloseUI(int uiType,int state)
		{
		//Framework.Plugin.Guide.GuideTriggerType.CloseUI
			return GuideSystem.getInstance().OnTrigger(6, null, false, uiType, state);
		}
		public static bool OnCompleteGudie(int guideID,int state)
		{
		//Framework.Plugin.Guide.GuideTriggerType.CompleteGudie
			return GuideSystem.getInstance().OnTrigger(7, null, false, guideID, state);
		}
		public static bool OnHeroFullEnergy(int id)
		{
		//Framework.Plugin.Guide.GuideTriggerType.HeroFullEnergy
			return GuideSystem.getInstance().OnTrigger(8, null, false, id);
		}
	}
}
