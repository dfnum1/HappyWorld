//auto generator
using System;
using System.Collections.Generic;
namespace Framework.Plugin.AT
{
	public static class AgentTree_ClassTypes
	{
		static Dictionary<System.Type, int> ms_vClassTypeToHash = new Dictionary<System.Type, int>();
		public static int ClassTypeToHash(System.Type classType)
		{
			#region MappingHash
			if(ms_vClassTypeToHash.Count <=0)
			{
				ms_vClassTypeToHash[typeof(Framework.Plugin.AT.IUserData)]= -110543973;
				ms_vClassTypeToHash[typeof(CommonTitleMenuSerialize)]= 1467265108;
				ms_vClassTypeToHash[typeof(UIElementCrystalUISerialized)]= 162237095;
				ms_vClassTypeToHash[typeof(UIFriendSerialized)]= -1899455692;
				ms_vClassTypeToHash[typeof(TopGame.UI.UISerialized)]= 2026484859;
				ms_vClassTypeToHash[typeof(TopGame.UI.UserInterface)]= 574168041;
				ms_vClassTypeToHash[typeof(TopGame.UI.ObjectHudHp)]= -1355461763;
				ms_vClassTypeToHash[typeof(TopGame.UI.ArtifactPanelSerialized)]= -1470547262;
				ms_vClassTypeToHash[typeof(TopGame.UI.DialogUserInterface)]= -794119849;
				ms_vClassTypeToHash[typeof(TopGame.UI.ExChanegUISerialized)]= -620562519;
				ms_vClassTypeToHash[typeof(TopGame.UI.UIGuideSerialize)]= -1691144711;
				ms_vClassTypeToHash[typeof(TopGame.UI.InstitutePanelSerialized)]= 660279042;
				ms_vClassTypeToHash[typeof(TopGame.UI.UILoadingSerialize)]= 156821818;
				ms_vClassTypeToHash[typeof(TopGame.UI.UILoginSerialize)]= -2145996026;
				ms_vClassTypeToHash[typeof(TopGame.UI.MainAdventurePanelSerialize)]= -513799404;
				ms_vClassTypeToHash[typeof(TopGame.UI.MainLineTaskSerialized)]= 738696663;
				ms_vClassTypeToHash[typeof(TopGame.UI.SelectModeUISerialize)]= -1446013808;
				ms_vClassTypeToHash[typeof(TopGame.UI.UnlimitParkourPanelSerialized)]= -274558275;
				ms_vClassTypeToHash[typeof(TopGame.UI.VideoSerialize)]= -1094919257;
				ms_vClassTypeToHash[typeof(TopGame.UI.WebViewPanel)]= 261520028;
				ms_vClassTypeToHash[typeof(TopGame.Logic.DoubleFireLevel)]= -1059238273;
				ms_vClassTypeToHash[typeof(TopGame.Logic.IslandInstanceAble)]= -2140299791;
				ms_vClassTypeToHash[typeof(TopGame.Logic.IslandMaskFogRoot)]= 787644096;
				ms_vClassTypeToHash[typeof(TopGame.Logic.SnakerLevel)]= 1000916782;
				ms_vClassTypeToHash[typeof(TopGame.Logic.BuildingAble)]= -1349519079;
				ms_vClassTypeToHash[typeof(TopGame.Logic.BuildingStateMarker)]= 189980108;
				ms_vClassTypeToHash[typeof(TopGame.Logic.DIYBlock)]= -1445518181;
				ms_vClassTypeToHash[typeof(TopGame.Logic.DIYTerrainPrefab)]= -350958494;
				ms_vClassTypeToHash[typeof(TopGame.Logic.BloodTweenEffecter)]= 778351003;
				ms_vClassTypeToHash[typeof(TopGame.Logic.RotationTweenEffecter)]= -1281209584;
				ms_vClassTypeToHash[typeof(TopGame.Logic.TweenEffecter)]= 1536686621;
				ms_vClassTypeToHash[typeof(TopGame.Logic.WorldUITweenEffecter)]= 1902611992;
				ms_vClassTypeToHash[typeof(TopGame.Core.ActionGraphBinder)]= -1380385372;
				ms_vClassTypeToHash[typeof(TopGame.Core.TimelineController)]= -1776368755;
				ms_vClassTypeToHash[typeof(TopGame.Core.FileSystem)]= -243819703;
				ms_vClassTypeToHash[typeof(TopGame.Core.DummyInstanceAble)]= 1059001602;
				ms_vClassTypeToHash[typeof(TopGame.Core.InstanceAbleHandler)]= 1246674300;
				ms_vClassTypeToHash[typeof(TopGame.Core.VirtualActions)]= -153299491;
				ms_vClassTypeToHash[typeof(TopGame.Core.ChainLightning)]= 1988465925;
				ms_vClassTypeToHash[typeof(TopGame.Core.ParticleController)]= -1137323838;
				ms_vClassTypeToHash[typeof(TopGame.Core.ParticleControllerWithAction)]= 1809223743;
				ms_vClassTypeToHash[typeof(TopGame.Core.SpeedLifeParticleController)]= -558023452;
				ms_vClassTypeToHash[typeof(TopGame.Core.WaringParticleController)]= -1254121026;
				ms_vClassTypeToHash[typeof(TopGame.Core.LevelScene)]= 865441931;
				ms_vClassTypeToHash[typeof(TopGame.Core.SkyBox)]= -496788420;
				ms_vClassTypeToHash[typeof(TopGame.Core.TerrainLayerElement)]= -1318659070;
				ms_vClassTypeToHash[typeof(TopGame.Net.KcpServerSession)]= -670594113;
				ms_vClassTypeToHash[typeof(TopGame.Net.TcpServerSession)]= -1328885221;
				ms_vClassTypeToHash[typeof(TopGame.Logic.Mecha)]= 350769278;
				ms_vClassTypeToHash[typeof(TopGame.Logic.Monster)]= -109978949;
				ms_vClassTypeToHash[typeof(TopGame.Logic.ObsElement)]= -1227023835;
				ms_vClassTypeToHash[typeof(TopGame.Logic.Player)]= -1166410742;
				ms_vClassTypeToHash[typeof(TopGame.Logic.PreviewActor)]= -908709959;
				ms_vClassTypeToHash[typeof(TopGame.Logic.Summon)]= -301302906;
				ms_vClassTypeToHash[typeof(TopGame.Logic.Wingman)]= 1348942682;
				ms_vClassTypeToHash[typeof(TopGame.Logic.WorldSceneNode)]= -943621745;
				ms_vClassTypeToHash[typeof(TopGame.Logic.Battle)]= 838439259;
				ms_vClassTypeToHash[typeof(TopGame.Logic.PVE)]= 955251439;
				ms_vClassTypeToHash[typeof(TopGame.Logic.Hall)]= 1447346531;
				ms_vClassTypeToHash[typeof(TopGame.Logic.Login)]= -929555034;
				ms_vClassTypeToHash[typeof(TopGame.Logic.SceneTheme)]= 643266871;
				ms_vClassTypeToHash[typeof(TopGame.ED.BattleEditorInstance)]= 127321349;
				ms_vClassTypeToHash[typeof(TopGame.UI.UICommonTip)]= -1012335406;
				ms_vClassTypeToHash[typeof(TopGame.UI.UICommonTipView)]= 184507133;
				ms_vClassTypeToHash[typeof(TopGame.UI.DialogPanel)]= 492517252;
				ms_vClassTypeToHash[typeof(TopGame.UI.DialogView)]= -663210307;
				ms_vClassTypeToHash[typeof(TopGame.UI.UIFullScreenFillPanel)]= -1419252118;
				ms_vClassTypeToHash[typeof(TopGame.UI.UIFullScreenFillView)]= -2104695422;
				ms_vClassTypeToHash[typeof(TopGame.UI.UIGameInfo)]= -833143701;
				ms_vClassTypeToHash[typeof(TopGame.UI.UIGameInfoView)]= 381635682;
				ms_vClassTypeToHash[typeof(TopGame.UI.UIGMPanel)]= -1590900061;
				ms_vClassTypeToHash[typeof(TopGame.UI.UIGMPanelView)]= 97081629;
				ms_vClassTypeToHash[typeof(TopGame.UI.GuidePanel)]= -906153592;
				ms_vClassTypeToHash[typeof(TopGame.UI.UIGuideView)]= -1053650587;
				ms_vClassTypeToHash[typeof(TopGame.UI.AUILoading)]= 945521253;
				ms_vClassTypeToHash[typeof(TopGame.UI.UILoading)]= 867782054;
				ms_vClassTypeToHash[typeof(TopGame.UI.UILoadingView)]= -1000724451;
				ms_vClassTypeToHash[typeof(TopGame.UI.UILogin)]= -892208471;
				ms_vClassTypeToHash[typeof(TopGame.UI.UILoginView)]= 896367561;
				ms_vClassTypeToHash[typeof(TopGame.UI.TransitionPanel)]= 1512056826;
				ms_vClassTypeToHash[typeof(TopGame.UI.TransitionView)]= 296171347;
				ms_vClassTypeToHash[typeof(TopGame.UI.UIVideo)]= 471788437;
				ms_vClassTypeToHash[typeof(TopGame.UI.UIVideoView)]= -1314502153;
				ms_vClassTypeToHash[typeof(TopGame.UI.WaitPanel)]= 431918946;
				ms_vClassTypeToHash[typeof(TopGame.UI.WaitView)]= -421083845;
				ms_vClassTypeToHash[typeof(TopGame.Core.BattleCameraMode)]= 43277010;
				ms_vClassTypeToHash[typeof(TopGame.Core.EditorCameraMode)]= -302951517;
				ms_vClassTypeToHash[typeof(TopGame.Core.FreeCameraMode)]= 118271136;
				ms_vClassTypeToHash[typeof(TopGame.Core.HallCameraMode)]= 599668512;
				ms_vClassTypeToHash[typeof(TopGame.Core.Projectile)]= 601264434;
				ms_vClassTypeToHash[typeof(Framework.Core.ATweenEffecter)]= 825531184;
				ms_vClassTypeToHash[typeof(Framework.Core.AActionGraphBinder)]= 564390056;
				ms_vClassTypeToHash[typeof(Framework.Core.AParticleController)]= -1257273348;
				ms_vClassTypeToHash[typeof(Framework.BattlePlus.GameActorAgent)]= 1946692654;
				ms_vClassTypeToHash[typeof(Framework.BattlePlus.MechaAgent)]= 1941383935;
				ms_vClassTypeToHash[typeof(Framework.BattlePlus.MonsterAgent)]= -1802823783;
				ms_vClassTypeToHash[typeof(Framework.BattlePlus.RunerAgent)]= 1984083663;
				ms_vClassTypeToHash[typeof(Framework.BattlePlus.SummonAgent)]= 220361335;
				ms_vClassTypeToHash[typeof(Framework.BattlePlus.AMecha)]= 1166008789;
				ms_vClassTypeToHash[typeof(Framework.BattlePlus.AMonster)]= 149187860;
				ms_vClassTypeToHash[typeof(Framework.BattlePlus.AObsElement)]= 1326997309;
				ms_vClassTypeToHash[typeof(Framework.BattlePlus.APlayer)]= -80997925;
				ms_vClassTypeToHash[typeof(Framework.BattlePlus.ASummon)]= -1352690089;
				ms_vClassTypeToHash[typeof(Framework.BattlePlus.AWingman)]= -1577939723;
				ms_vClassTypeToHash[typeof(Framework.Logic.APreviewActor)]= 378229250;
				ms_vClassTypeToHash[typeof(Framework.Core.GraphicActorAgent)]= -1512580597;
				ms_vClassTypeToHash[typeof(Framework.Core.ColorProperty)]= 1040517172;
				ms_vClassTypeToHash[typeof(Framework.Core.FloatProperty)]= -1601878289;
				ms_vClassTypeToHash[typeof(Framework.Core.TextureProperty)]= -869663978;
				ms_vClassTypeToHash[typeof(Framework.Core.VectorProperty)]= 406635084;
				ms_vClassTypeToHash[typeof(Framework.Core.Actor)]= 1089212927;
				ms_vClassTypeToHash[typeof(Framework.Core.AProjectile)]= 1328774582;
			}
			#endregion
			int hash = 0;
			if(ms_vClassTypeToHash.TryGetValue(classType, out hash)) return hash;
			return 0;
		}
		public static System.Type HashToClassType(int hashCode)
		{
			if(hashCode == 0) return null;
			switch(hashCode)
			{
				case -110543973: return typeof(Framework.Plugin.AT.IUserData);
				case 1467265108: return typeof(CommonTitleMenuSerialize);
				case 162237095: return typeof(UIElementCrystalUISerialized);
				case -1899455692: return typeof(UIFriendSerialized);
				case 2026484859: return typeof(TopGame.UI.UISerialized);
				case 574168041: return typeof(TopGame.UI.UserInterface);
				case -1355461763: return typeof(TopGame.UI.ObjectHudHp);
				case -1470547262: return typeof(TopGame.UI.ArtifactPanelSerialized);
				case -794119849: return typeof(TopGame.UI.DialogUserInterface);
				case -620562519: return typeof(TopGame.UI.ExChanegUISerialized);
				case -1691144711: return typeof(TopGame.UI.UIGuideSerialize);
				case 660279042: return typeof(TopGame.UI.InstitutePanelSerialized);
				case 156821818: return typeof(TopGame.UI.UILoadingSerialize);
				case -2145996026: return typeof(TopGame.UI.UILoginSerialize);
				case -513799404: return typeof(TopGame.UI.MainAdventurePanelSerialize);
				case 738696663: return typeof(TopGame.UI.MainLineTaskSerialized);
				case -1446013808: return typeof(TopGame.UI.SelectModeUISerialize);
				case -274558275: return typeof(TopGame.UI.UnlimitParkourPanelSerialized);
				case -1094919257: return typeof(TopGame.UI.VideoSerialize);
				case 261520028: return typeof(TopGame.UI.WebViewPanel);
				case -1059238273: return typeof(TopGame.Logic.DoubleFireLevel);
				case -2140299791: return typeof(TopGame.Logic.IslandInstanceAble);
				case 787644096: return typeof(TopGame.Logic.IslandMaskFogRoot);
				case 1000916782: return typeof(TopGame.Logic.SnakerLevel);
				case -1349519079: return typeof(TopGame.Logic.BuildingAble);
				case 189980108: return typeof(TopGame.Logic.BuildingStateMarker);
				case -1445518181: return typeof(TopGame.Logic.DIYBlock);
				case -350958494: return typeof(TopGame.Logic.DIYTerrainPrefab);
				case 778351003: return typeof(TopGame.Logic.BloodTweenEffecter);
				case -1281209584: return typeof(TopGame.Logic.RotationTweenEffecter);
				case 1536686621: return typeof(TopGame.Logic.TweenEffecter);
				case 1902611992: return typeof(TopGame.Logic.WorldUITweenEffecter);
				case -1380385372: return typeof(TopGame.Core.ActionGraphBinder);
				case -1776368755: return typeof(TopGame.Core.TimelineController);
				case -243819703: return typeof(TopGame.Core.FileSystem);
				case 1059001602: return typeof(TopGame.Core.DummyInstanceAble);
				case 1246674300: return typeof(TopGame.Core.InstanceAbleHandler);
				case -153299491: return typeof(TopGame.Core.VirtualActions);
				case 1988465925: return typeof(TopGame.Core.ChainLightning);
				case -1137323838: return typeof(TopGame.Core.ParticleController);
				case 1809223743: return typeof(TopGame.Core.ParticleControllerWithAction);
				case -558023452: return typeof(TopGame.Core.SpeedLifeParticleController);
				case -1254121026: return typeof(TopGame.Core.WaringParticleController);
				case 865441931: return typeof(TopGame.Core.LevelScene);
				case -496788420: return typeof(TopGame.Core.SkyBox);
				case -1318659070: return typeof(TopGame.Core.TerrainLayerElement);
				case -670594113: return typeof(TopGame.Net.KcpServerSession);
				case -1328885221: return typeof(TopGame.Net.TcpServerSession);
				case 350769278: return typeof(TopGame.Logic.Mecha);
				case -109978949: return typeof(TopGame.Logic.Monster);
				case -1227023835: return typeof(TopGame.Logic.ObsElement);
				case -1166410742: return typeof(TopGame.Logic.Player);
				case -908709959: return typeof(TopGame.Logic.PreviewActor);
				case -301302906: return typeof(TopGame.Logic.Summon);
				case 1348942682: return typeof(TopGame.Logic.Wingman);
				case -943621745: return typeof(TopGame.Logic.WorldSceneNode);
				case 838439259: return typeof(TopGame.Logic.Battle);
				case 955251439: return typeof(TopGame.Logic.PVE);
				case 1447346531: return typeof(TopGame.Logic.Hall);
				case -929555034: return typeof(TopGame.Logic.Login);
				case 643266871: return typeof(TopGame.Logic.SceneTheme);
				case 127321349: return typeof(TopGame.ED.BattleEditorInstance);
				case -1012335406: return typeof(TopGame.UI.UICommonTip);
				case 184507133: return typeof(TopGame.UI.UICommonTipView);
				case 492517252: return typeof(TopGame.UI.DialogPanel);
				case -663210307: return typeof(TopGame.UI.DialogView);
				case -1419252118: return typeof(TopGame.UI.UIFullScreenFillPanel);
				case -2104695422: return typeof(TopGame.UI.UIFullScreenFillView);
				case -833143701: return typeof(TopGame.UI.UIGameInfo);
				case 381635682: return typeof(TopGame.UI.UIGameInfoView);
				case -1590900061: return typeof(TopGame.UI.UIGMPanel);
				case 97081629: return typeof(TopGame.UI.UIGMPanelView);
				case -906153592: return typeof(TopGame.UI.GuidePanel);
				case -1053650587: return typeof(TopGame.UI.UIGuideView);
				case 945521253: return typeof(TopGame.UI.AUILoading);
				case 867782054: return typeof(TopGame.UI.UILoading);
				case -1000724451: return typeof(TopGame.UI.UILoadingView);
				case -892208471: return typeof(TopGame.UI.UILogin);
				case 896367561: return typeof(TopGame.UI.UILoginView);
				case 1512056826: return typeof(TopGame.UI.TransitionPanel);
				case 296171347: return typeof(TopGame.UI.TransitionView);
				case 471788437: return typeof(TopGame.UI.UIVideo);
				case -1314502153: return typeof(TopGame.UI.UIVideoView);
				case 431918946: return typeof(TopGame.UI.WaitPanel);
				case -421083845: return typeof(TopGame.UI.WaitView);
				case 43277010: return typeof(TopGame.Core.BattleCameraMode);
				case -302951517: return typeof(TopGame.Core.EditorCameraMode);
				case 118271136: return typeof(TopGame.Core.FreeCameraMode);
				case 599668512: return typeof(TopGame.Core.HallCameraMode);
				case 601264434: return typeof(TopGame.Core.Projectile);
				case 825531184: return typeof(Framework.Core.ATweenEffecter);
				case 564390056: return typeof(Framework.Core.AActionGraphBinder);
				case -1257273348: return typeof(Framework.Core.AParticleController);
				case 1946692654: return typeof(Framework.BattlePlus.GameActorAgent);
				case 1941383935: return typeof(Framework.BattlePlus.MechaAgent);
				case -1802823783: return typeof(Framework.BattlePlus.MonsterAgent);
				case 1984083663: return typeof(Framework.BattlePlus.RunerAgent);
				case 220361335: return typeof(Framework.BattlePlus.SummonAgent);
				case 1166008789: return typeof(Framework.BattlePlus.AMecha);
				case 149187860: return typeof(Framework.BattlePlus.AMonster);
				case 1326997309: return typeof(Framework.BattlePlus.AObsElement);
				case -80997925: return typeof(Framework.BattlePlus.APlayer);
				case -1352690089: return typeof(Framework.BattlePlus.ASummon);
				case -1577939723: return typeof(Framework.BattlePlus.AWingman);
				case 378229250: return typeof(Framework.Logic.APreviewActor);
				case -1512580597: return typeof(Framework.Core.GraphicActorAgent);
				case 1040517172: return typeof(Framework.Core.ColorProperty);
				case -1601878289: return typeof(Framework.Core.FloatProperty);
				case -869663978: return typeof(Framework.Core.TextureProperty);
				case 406635084: return typeof(Framework.Core.VectorProperty);
				case 1089212927: return typeof(Framework.Core.Actor);
				case 1328774582: return typeof(Framework.Core.AProjectile);
			}
			return null;
		}
		public static int HashToParentHash(int hashCode)
		{
			if(hashCode == 0) return 0;
			switch(hashCode)
			{
				case 1467265108: return 2026484859;//CommonTitleMenuSerialize->TopGame.UI.UISerialized
				case 162237095: return 2026484859;//UIElementCrystalUISerialized->TopGame.UI.UISerialized
				case -1899455692: return 2026484859;//UIFriendSerialized->TopGame.UI.UISerialized
				case 2026484859: return -1826999285;//TopGame.UI.UISerialized->TopGame.Core.ComSerialized
				case 574168041: return 2026484859;//TopGame.UI.UserInterface->TopGame.UI.UISerialized
				case -1355461763: return 1980077048;//TopGame.UI.ObjectHudHp->Framework.Core.AInstanceAble
				case -1470547262: return 2026484859;//TopGame.UI.ArtifactPanelSerialized->TopGame.UI.UISerialized
				case -794119849: return 2026484859;//TopGame.UI.DialogUserInterface->TopGame.UI.UISerialized
				case -620562519: return 2026484859;//TopGame.UI.ExChanegUISerialized->TopGame.UI.UISerialized
				case -1691144711: return 2026484859;//TopGame.UI.UIGuideSerialize->TopGame.UI.UISerialized
				case 660279042: return 2026484859;//TopGame.UI.InstitutePanelSerialized->TopGame.UI.UISerialized
				case 156821818: return 2026484859;//TopGame.UI.UILoadingSerialize->TopGame.UI.UISerialized
				case -2145996026: return 2026484859;//TopGame.UI.UILoginSerialize->TopGame.UI.UISerialized
				case -513799404: return 2026484859;//TopGame.UI.MainAdventurePanelSerialize->TopGame.UI.UISerialized
				case 738696663: return 2026484859;//TopGame.UI.MainLineTaskSerialized->TopGame.UI.UISerialized
				case -1446013808: return 2026484859;//TopGame.UI.SelectModeUISerialize->TopGame.UI.UISerialized
				case -274558275: return 2026484859;//TopGame.UI.UnlimitParkourPanelSerialized->TopGame.UI.UISerialized
				case -1094919257: return 2026484859;//TopGame.UI.VideoSerialize->TopGame.UI.UISerialized
				case 261520028: return 2026484859;//TopGame.UI.WebViewPanel->TopGame.UI.UISerialized
				case -1059238273: return 1980077048;//TopGame.Logic.DoubleFireLevel->Framework.Core.AInstanceAble
				case -2140299791: return 1980077048;//TopGame.Logic.IslandInstanceAble->Framework.Core.AInstanceAble
				case 787644096: return -1826999285;//TopGame.Logic.IslandMaskFogRoot->TopGame.Core.ComSerialized
				case 1000916782: return 1980077048;//TopGame.Logic.SnakerLevel->Framework.Core.AInstanceAble
				case -1349519079: return 1980077048;//TopGame.Logic.BuildingAble->Framework.Core.AInstanceAble
				case 189980108: return 1980077048;//TopGame.Logic.BuildingStateMarker->Framework.Core.AInstanceAble
				case -1445518181: return 1980077048;//TopGame.Logic.DIYBlock->Framework.Core.AInstanceAble
				case -350958494: return 1980077048;//TopGame.Logic.DIYTerrainPrefab->Framework.Core.AInstanceAble
				case 778351003: return 1980077048;//TopGame.Logic.BloodTweenEffecter->Framework.Core.AInstanceAble
				case -1281209584: return 1980077048;//TopGame.Logic.RotationTweenEffecter->Framework.Core.AInstanceAble
				case 1536686621: return 1980077048;//TopGame.Logic.TweenEffecter->Framework.Core.AInstanceAble
				case 1902611992: return 1980077048;//TopGame.Logic.WorldUITweenEffecter->Framework.Core.AInstanceAble
				case -1380385372: return 1980077048;//TopGame.Core.ActionGraphBinder->Framework.Core.AInstanceAble
				case -1776368755: return 1980077048;//TopGame.Core.TimelineController->Framework.Core.AInstanceAble
				case -243819703: return -1651345459;//TopGame.Core.FileSystem->Framework.Core.AFileSystem
				case 1059001602: return 1980077048;//TopGame.Core.DummyInstanceAble->Framework.Core.AInstanceAble
				case 1246674300: return 1980077048;//TopGame.Core.InstanceAbleHandler->Framework.Core.AInstanceAble
				case -153299491: return 1980077048;//TopGame.Core.VirtualActions->Framework.Core.AInstanceAble
				case 1988465925: return 1980077048;//TopGame.Core.ChainLightning->Framework.Core.AInstanceAble
				case -1137323838: return 1980077048;//TopGame.Core.ParticleController->Framework.Core.AInstanceAble
				case 1809223743: return 1980077048;//TopGame.Core.ParticleControllerWithAction->Framework.Core.AInstanceAble
				case -558023452: return 1980077048;//TopGame.Core.SpeedLifeParticleController->Framework.Core.AInstanceAble
				case -1254121026: return 1980077048;//TopGame.Core.WaringParticleController->Framework.Core.AInstanceAble
				case 865441931: return 1980077048;//TopGame.Core.LevelScene->Framework.Core.AInstanceAble
				case -496788420: return 1980077048;//TopGame.Core.SkyBox->Framework.Core.AInstanceAble
				case -1318659070: return 1980077048;//TopGame.Core.TerrainLayerElement->Framework.Core.AInstanceAble
				case -670594113: return 1271664188;//TopGame.Net.KcpServerSession->TopGame.Net.AServerSession
				case -1328885221: return 1271664188;//TopGame.Net.TcpServerSession->TopGame.Net.AServerSession
				case 350769278: return 1089212927;//TopGame.Logic.Mecha->Framework.Core.Actor
				case -109978949: return 149187860;//TopGame.Logic.Monster->Framework.BattlePlus.AMonster
				case -1227023835: return 55777259;//TopGame.Logic.ObsElement->Framework.Core.AWorldNode
				case -1166410742: return -80997925;//TopGame.Logic.Player->Framework.BattlePlus.APlayer
				case -908709959: return 378229250;//TopGame.Logic.PreviewActor->Framework.Logic.APreviewActor
				case -301302906: return 1089212927;//TopGame.Logic.Summon->Framework.Core.Actor
				case 1348942682: return -80997925;//TopGame.Logic.Wingman->Framework.BattlePlus.APlayer
				case -943621745: return 55777259;//TopGame.Logic.WorldSceneNode->Framework.Core.AWorldNode
				case 838439259: return -193269295;//TopGame.Logic.Battle->TopGame.Logic.AState
				case 955251439: return -2140108889;//TopGame.Logic.PVE->TopGame.Logic.AbsMode
				case 1447346531: return -193269295;//TopGame.Logic.Hall->TopGame.Logic.AState
				case -929555034: return -193269295;//TopGame.Logic.Login->TopGame.Logic.AState
				case 643266871: return -1107943145;//TopGame.Logic.SceneTheme->TopGame.Core.ASceneTheme
				case 127321349: return 1501439831;//TopGame.ED.BattleEditorInstance->TopGame.GameInstance
				case -1012335406: return -1458859266;//TopGame.UI.UICommonTip->TopGame.UI.UIBase
				case 184507133: return -1757231343;//TopGame.UI.UICommonTipView->TopGame.UI.UIView
				case 492517252: return -1458859266;//TopGame.UI.DialogPanel->TopGame.UI.UIBase
				case -663210307: return -1757231343;//TopGame.UI.DialogView->TopGame.UI.UIView
				case -1419252118: return -1458859266;//TopGame.UI.UIFullScreenFillPanel->TopGame.UI.UIBase
				case -2104695422: return -1757231343;//TopGame.UI.UIFullScreenFillView->TopGame.UI.UIView
				case -833143701: return -1458859266;//TopGame.UI.UIGameInfo->TopGame.UI.UIBase
				case 381635682: return -1757231343;//TopGame.UI.UIGameInfoView->TopGame.UI.UIView
				case -1590900061: return -1458859266;//TopGame.UI.UIGMPanel->TopGame.UI.UIBase
				case 97081629: return -1757231343;//TopGame.UI.UIGMPanelView->TopGame.UI.UIView
				case -906153592: return -1458859266;//TopGame.UI.GuidePanel->TopGame.UI.UIBase
				case -1053650587: return -1757231343;//TopGame.UI.UIGuideView->TopGame.UI.UIView
				case 945521253: return -1458859266;//TopGame.UI.AUILoading->TopGame.UI.UIBase
				case 867782054: return -1458859266;//TopGame.UI.UILoading->TopGame.UI.UIBase
				case -1000724451: return -1757231343;//TopGame.UI.UILoadingView->TopGame.UI.UIView
				case -892208471: return -1458859266;//TopGame.UI.UILogin->TopGame.UI.UIBase
				case 896367561: return -1757231343;//TopGame.UI.UILoginView->TopGame.UI.UIView
				case 1512056826: return -1458859266;//TopGame.UI.TransitionPanel->TopGame.UI.UIBase
				case 296171347: return -1757231343;//TopGame.UI.TransitionView->TopGame.UI.UIView
				case 471788437: return -1458859266;//TopGame.UI.UIVideo->TopGame.UI.UIBase
				case -1314502153: return -1757231343;//TopGame.UI.UIVideoView->TopGame.UI.UIView
				case 431918946: return -1458859266;//TopGame.UI.WaitPanel->TopGame.UI.UIBase
				case -421083845: return -1757231343;//TopGame.UI.WaitView->TopGame.UI.UIView
				case 43277010: return 1498978502;//TopGame.Core.BattleCameraMode->Framework.Core.CameraMode
				case -302951517: return 1498978502;//TopGame.Core.EditorCameraMode->Framework.Core.CameraMode
				case 118271136: return 1498978502;//TopGame.Core.FreeCameraMode->Framework.Core.CameraMode
				case 599668512: return 1498978502;//TopGame.Core.HallCameraMode->Framework.Core.CameraMode
				case 601264434: return 55777259;//TopGame.Core.Projectile->Framework.Core.AWorldNode
				case 825531184: return 1980077048;//Framework.Core.ATweenEffecter->Framework.Core.AInstanceAble
				case 564390056: return 1980077048;//Framework.Core.AActionGraphBinder->Framework.Core.AInstanceAble
				case -1257273348: return 1980077048;//Framework.Core.AParticleController->Framework.Core.AInstanceAble
				case 1946692654: return 660158659;//Framework.BattlePlus.GameActorAgent->Framework.Core.ActorAgent
				case 1941383935: return 660158659;//Framework.BattlePlus.MechaAgent->Framework.Core.ActorAgent
				case -1802823783: return 660158659;//Framework.BattlePlus.MonsterAgent->Framework.Core.ActorAgent
				case 1984083663: return 660158659;//Framework.BattlePlus.RunerAgent->Framework.Core.ActorAgent
				case 220361335: return 660158659;//Framework.BattlePlus.SummonAgent->Framework.Core.ActorAgent
				case 1166008789: return 1089212927;//Framework.BattlePlus.AMecha->Framework.Core.Actor
				case 149187860: return 1089212927;//Framework.BattlePlus.AMonster->Framework.Core.Actor
				case 1326997309: return 55777259;//Framework.BattlePlus.AObsElement->Framework.Core.AWorldNode
				case -80997925: return 1089212927;//Framework.BattlePlus.APlayer->Framework.Core.Actor
				case -1352690089: return 1089212927;//Framework.BattlePlus.ASummon->Framework.Core.Actor
				case -1577939723: return -80997925;//Framework.BattlePlus.AWingman->Framework.BattlePlus.APlayer
				case 378229250: return 1089212927;//Framework.Logic.APreviewActor->Framework.Core.Actor
				case -1512580597: return 660158659;//Framework.Core.GraphicActorAgent->Framework.Core.ActorAgent
				case 1040517172: return 85319012;//Framework.Core.ColorProperty->Framework.Core.ABaseProperty
				case -1601878289: return 85319012;//Framework.Core.FloatProperty->Framework.Core.ABaseProperty
				case -869663978: return 85319012;//Framework.Core.TextureProperty->Framework.Core.ABaseProperty
				case 406635084: return 85319012;//Framework.Core.VectorProperty->Framework.Core.ABaseProperty
				case 1089212927: return 55777259;//Framework.Core.Actor->Framework.Core.AWorldNode
				case 1328774582: return 55777259;//Framework.Core.AProjectile->Framework.Core.AWorldNode
			}
			return 0;
		}
	}
}
