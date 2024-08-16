//auto generator
using System;
namespace Framework.Plugin.AT
{
	public static class AgentTreeUserClass_Func
	{
		public static bool DoInerAction(AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			if(pAction.inArgvs == null || pAction.inArgvs.Length < 1) return true;
			Variable pUserPointer = pAction.GetInVariableByIndex<Variable>(0, pTask);
			Variable pOriUserPointer = pAction.GetInVariable(0); // get ori port
			int classHashCode= 0;
			if (pOriUserPointer != null) classHashCode = pOriUserPointer.GetClassHashCode();
			else if(pUserPointer != null ) classHashCode = pUserPointer.GetClassHashCode();
			if(classHashCode == 0 )
			{
				VariableInt pUserHash = pAction.GetInVariableByIndex<VariableInt>(0, pTask);
				if(pUserHash!=null)classHashCode = pUserHash.mValue;
			}
			if(classHashCode == 0) return true;
			switch(classHashCode)
			{
				case 1265268240:
				{//TopGame.RtgTween.Tweener
					return AgentTree_Tweener.DoAction(pUserPointer as VariableMonoScript, pTask, pAction, functionId);
				}
				case -730277061:
				{//TopGame.RtgTween.TweenerGroup
					return AgentTree_TweenerGroup.DoAction(pUserPointer as VariableMonoScript, pTask, pAction, functionId);
				}
				case 2026484859:
				{//TopGame.UI.UISerialized
					return AgentTree_UISerialized.DoAction(pUserPointer as VariableMonoScript, pTask, pAction, functionId);
				}
				case 2115626524:
				{//TopGame.UI.SelectToggleGroup
					return AgentTree_SelectToggleGroup.DoAction(pUserPointer as VariableMonoScript, pTask, pAction, functionId);
				}
				case -329504808:
				{//TopGame.Core.AudioManager
					return AgentTree_AudioManager.DoAction(pUserPointer as VariableMonoScript, pTask, pAction, functionId);
				}
				case -1826999285:
				{//TopGame.Core.ComSerialized
					return AgentTree_ComSerialized.DoAction(pUserPointer as VariableMonoScript, pTask, pAction, functionId);
				}
				case -1107943145:
				{//TopGame.Core.ASceneTheme
					return AgentTree_ASceneTheme.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 1501439831:
				{//TopGame.GameInstance
					return AgentTree_GameInstance.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1984467271:
				{//TopGame.Net.NetWork
					return AgentTree_NetWork.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 1271664188:
				{//TopGame.Net.AServerSession
					return AgentTree_AServerSession.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -109978949:
				{//TopGame.Logic.Monster
					return AgentTree_Monster.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1166410742:
				{//TopGame.Logic.Player
					return AgentTree_Player.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -2140108889:
				{//TopGame.Logic.AbsMode
					return AgentTree_AbsMode.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -193269295:
				{//TopGame.Logic.AState
					return AgentTree_AState.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 60260985:
				{//TopGame.Logic.StateFactory
					return AgentTree_StateFactory.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -113451244:
				{//TopGame.SvrData.User
					return AgentTree_User.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -622917976:
				{//TopGame.SvrData.UserManager
					return AgentTree_UserManager.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 2137942569:
				{//TopGame.Data.DataManager
					return AgentTree_DataManager.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1275882281:
				{//TopGame.Data.CsvData_Dialog
					return AgentTree_CsvData_Dialog.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -157950560:
				{//TopGame.Data.PermanentAssetsUtil
					return AgentTree_PermanentAssetsUtil.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1458859266:
				{//TopGame.UI.UIBase
					return AgentTree_UIBase.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1757231343:
				{//TopGame.UI.UIView
					return AgentTree_UIView.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 492517252:
				{//TopGame.UI.DialogPanel
					return AgentTree_DialogPanel.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1419252118:
				{//TopGame.UI.UIFullScreenFillPanel
					return AgentTree_UIFullScreenFillPanel.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1590900061:
				{//TopGame.UI.UIGMPanel
					return AgentTree_UIGMPanel.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -892208471:
				{//TopGame.UI.UILogin
					return AgentTree_UILogin.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 896367561:
				{//TopGame.UI.UILoginView
					return AgentTree_UILoginView.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 1512056826:
				{//TopGame.UI.TransitionPanel
					return AgentTree_TransitionPanel.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -519742740:
				{//TopGame.UI.UIATListParam
					return AgentTree_UIATListParam.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1627565909:
				{//TopGame.UI.UIManager
					return AgentTree_UIManager.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1210458564:
				{//TopGame.UI.UIUtil
					return AgentTree_UIUtil.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 87329578:
				{//TopGame.Core.AnimPathData
					return AgentTree_AnimPathData.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 1737045939:
				{//TopGame.Core.AnimPathManager
					return AgentTree_AnimPathManager.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1311249602:
				{//TopGame.Core.TimelineData
					return AgentTree_TimelineData.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -525529666:
				{//TopGame.Core.CameraController
					return AgentTree_CameraController.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -847446270:
				{//TopGame.Core.DropItemManager
					return AgentTree_DropItemManager.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 948022496:
				{//TopGame.Base.Util
					return AgentTree_Util.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 519029197:
				{//TopGame.Data.CsvData_Dialog.DialogData
					return AgentTree_DialogData.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1651345459:
				{//Framework.Core.AFileSystem
					return AgentTree_AFileSystem.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 1980077048:
				{//Framework.Core.AInstanceAble
					return AgentTree_AInstanceAble.DoAction(pUserPointer as VariableMonoScript, pTask, pAction, functionId);
				}
				case -1089217254:
				{//Framework.Core.InstanceOperiaon
					return AgentTree_InstanceOperiaon.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 2138846980:
				{//Framework.Core.MaterailBlockUtil
					return AgentTree_MaterailBlockUtil.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 661331526:
				{//Framework.Core.DyncmicTransformCollects
					return AgentTree_DyncmicTransformCollects.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 149187860:
				{//Framework.BattlePlus.AMonster
					return AgentTree_AMonster.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -80997925:
				{//Framework.BattlePlus.APlayer
					return AgentTree_APlayer.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -1879118545:
				{//Framework.BattlePlus.BattleDamage
					return AgentTree_BattleDamage.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 378229250:
				{//Framework.Logic.APreviewActor
					return AgentTree_APreviewActor.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 1498978502:
				{//Framework.Core.CameraMode
					return AgentTree_CameraMode.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 85319012:
				{//Framework.Core.ABaseProperty
					return AgentTree_ABaseProperty.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -460919082:
				{//Framework.Core.PropertyEffecter
					return AgentTree_PropertyEffecter.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 1089212927:
				{//Framework.Core.Actor
					return AgentTree_Actor.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 660158659:
				{//Framework.Core.ActorAgent
					return AgentTree_ActorAgent.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 457018166:
				{//Framework.Core.ActorParameter
					return AgentTree_ActorParameter.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case -2131304428:
				{//Framework.Core.BodyPartParameter
					return AgentTree_BodyPartParameter.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 811637270:
				{//Framework.Core.SkillInformation
					return AgentTree_SkillInformation.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
				case 55777259:
				{//Framework.Core.AWorldNode
					return AgentTree_AWorldNode.DoAction(pUserPointer as VariableUser, pTask, pAction, functionId);
				}
			}
			return true;
		}
	}
}
