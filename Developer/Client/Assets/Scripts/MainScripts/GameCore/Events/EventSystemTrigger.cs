/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	EventSystemTrigger
作    者:	HappLI
描    述:	
*********************************************************************/
#define USE_FMOD
using UnityEngine;
using ExternEngine;
#if !USE_SERVER
using TopGame.URP;
#else
using Transform = ExternEngine.Transform;
#endif
using Framework.Core;
using Framework.BattlePlus;
namespace TopGame.Core
{
    public class EventSystemTrigger : ABattleEventSystemTrigger
    {
        //------------------------------------------------------
        protected override void OnSound(SoundEventParameter param)
        {
#if !USE_SERVER
            Actor actor = GetTriggerActor();
            if (actor != null)
            {
                if ((!actor.IsVisible() || (!param.canTriggerAfterKilled && actor.IsFlag(EWorldNodeFlag.Killed)) || actor.IsDestroy())) return;
            }
            else if(param.soundType == ESoundType.StopEffect)
            {
                Framework.Core.AudioUtil.StopEffect(param.strFile);
                return;
            }
            else if (param.soundType == ESoundType.StopBG)
            {
                Framework.Core.AudioUtil.StopBG(param.strFile);
                return;
            }
            int snd = 0;
            if (param.audioId != 0)
            {
                Transform pTrans = GetTriggerTransform();
                if (param.use3D)
                    snd = AudioManager.PlayID(param.audioId, pTrans, param.mixBG);
                else
                    snd = AudioManager.PlayID(param.audioId, pTrans ? pTrans.position : (Vector3)TriggerEventRealPos, param.mixBG);
            }
            else
            {
                if (param.soundType == ESoundType.BG) 
                {
                    if (param.mixBG) snd = AudioManager.MixBG(param.strFile, param.Fade, true, null, param.mixGroup);
                    else snd = AudioManager.PlayBG(param.strFile, param.Fade);
                }
                else if (param.soundType == ESoundType.Effect)
                {
                    if (param.use3D)
                    {
                        Transform pTrans = GetTriggerTransform();
                        snd = AudioManager.Play3DEffectVolume(param.strFile, UnityEngine.Random.Range(Mathf.Min(param.minVolume, param.maxVolume), Mathf.Max(param.minVolume, param.maxVolume)), pTrans);
                    }
                    else
                        snd = AudioManager.PlayEffectVolume(param.strFile, UnityEngine.Random.Range(Mathf.Min(param.minVolume, param.maxVolume), Mathf.Max(param.minVolume, param.maxVolume)));
                }
            }

            if (snd != 0 && pActionEvent != null)
            {
                pActionEvent.sound = (uint)snd;
                pActionEvent.sound3D = param.use3D;
            }
#endif
        }
        //------------------------------------------------------
        protected override void OnRenderCurve(RenderCurveEventParameter param)
        {
#if !USE_SERVER
            if (Base.GlobalShaderController.Instance == null) return;
            TriggerEventPos.z += param.PivotZ;
            Base.GlobalShaderController.SetCurve(param.Angle, param.Radius, param.PivotPointY, param.PivotX, TriggerEventPos, param.bAbs);
#endif
        }
        //------------------------------------------------------
        protected override void OnRenderBlend(RenderBlendEventParameter param)
        {
#if !USE_SERVER
            if (Base.GlobalShaderController.Instance == null) return;
            Base.GlobalShaderController.SetBlend(param.AxisX, param.AxisY, param.PivotZ, param.bAbs, param.bImmediately);
#endif
        }
        //------------------------------------------------------
        protected override void OnCameraSpline(CameraSplineEventParameter param)
        {
#if !USE_SERVER
            if (Base.GlobalShaderController.Instance == null || CameraController.getInstance() == null) return;
            CameraController.getInstance().GetEffect<CameraSpline>().PlayCurve(param);
#endif
        }
        //------------------------------------------------------
        protected override void OnCameraOffset(CameraOffsetEventParameter param)
        {
#if !USE_SERVER
            AFrameworkModule pGameMoudle = m_pFramework as AFrameworkModule;
            if (pGameMoudle == null) return;
            if (pGameMoudle.cameraController == null) return;
            CameraMode curMode = pGameMoudle.cameraController.GetCurrentMode();
            if (curMode == null) return;
            if ((param.useBit & (int)CameraOffsetEventParameter.EType.Distance) != 0)
            {
                if (param.force || !curMode.IsLockOffsetDistanceLerping())
                    curMode.SetLockOffsetDistance(param.distance, param.bAmount, param.fLerp, param.pingpong);
            }
            if ((param.useBit & (int)CameraOffsetEventParameter.EType.Fov) != 0)
            {
                if (param.force || curMode.GetLockFovOffsetLerp().IsArrived())
                    curMode.SetLockFovOffset(param.fov, param.bAmount, param.fLerp, param.pingpong);
            }
            if ((param.useBit & (int)CameraOffsetEventParameter.EType.Yaw) != 0)
            {
                if (param.force || curMode.GetLockEulerAngleOffsetLerp().IsArrived(0))
                    curMode.SetLockYawOffset(param.yaw, param.bAmount, param.fLerp, param.pingpong);
            }
            if ((param.useBit & (int)CameraOffsetEventParameter.EType.Pitch) != 0)
            {
                if (param.force || curMode.GetLockEulerAngleOffsetLerp().IsArrived(1))
                    curMode.SetLockPitchOffset(param.pitch, param.bAmount, param.fLerp, param.pingpong);
            }
            if ((param.useBit & (int)CameraOffsetEventParameter.EType.Roll) != 0)
            {
                if (param.force || curMode.GetLockEulerAngleOffsetLerp().IsArrived(2))
                    curMode.SetLockRollOffset(param.roll, param.bAmount, param.fLerp, param.pingpong);
            }
            if ((param.useBit & (int)CameraOffsetEventParameter.EType.LookAtOffset) != 0)
            {
                if (param.force || curMode.GetLockCameraLookAtOffset().IsArrived())
                    curMode.SetLockCameraLookAtOffset(param.lookOffset, param.bAmount, param.fLerp, param.pingpong);
            }
#endif
        }
        //------------------------------------------------------
        protected override void OnUIEvent(UIEventParameter param)
        {
#if !USE_SERVER
            UI.UIManager uiMgr = m_pFramework.Get<UI.UIManager>();
            if (uiMgr == null) return;
            if (param.type == UIEventParameter.EType.UIRootHide)
            {
                if (uiMgr != null) uiMgr.ShowRoot(false);
                return;
            }
            if (param.type == UIEventParameter.EType.UIRootShow)
            {
                if (uiMgr != null) uiMgr.ShowRoot(true);
                return;
            }
            if (param.bAllUI)
            {
                switch (param.type)
                {
                    case UIEventParameter.EType.Close: uiMgr.CloseAll(); break;
                    case UIEventParameter.EType.Hide: uiMgr.HideAll(); break;
                    case UIEventParameter.EType.Show: uiMgr.ShowAll(); break;
                }
            }
            else
            {
                if (param.uiType <= 0) return;
                switch (param.type)
                {
                    case UIEventParameter.EType.Close: uiMgr.CloseUI(param.uiType); break;
                    case UIEventParameter.EType.Hide: uiMgr.HideUI(param.uiType); break;
                    case UIEventParameter.EType.Show: 
                        {
                            if(param.Params!=null && param.Params.Count>0)
                            {
                                var handle = uiMgr.CastGetUI<UI.UIBase>(param.uiType, true);
                                if (handle != null)
                                {
                                    for(int i =0; i < param.Params.Count; ++i)
                                    {
                                        if (string.IsNullOrEmpty(param.Params[i].key)) continue;
                                        handle.AddUserParam(param.Params[i].key, new VariableString() { strValue = param.Params[i].value });
                                    }
                                    handle.Show();
                                }
                            }
                            else
                            {
                                uiMgr.ShowUI(param.uiType);
                            }
                        }
                        break;
                }
            }
#endif
        }
        //------------------------------------------------------
        protected override void OnTrigger(TriggerEventParameter param)
        {
            switch (param.dataType)
            {
#if !USE_SERVER
                case TriggerEventParameter.EDataType.ShowAllActors:
                    {
                        if (RootsHandler.ActorsRoot)
                        {
                            Framework.Core.CommonUtility.SetActive(RootsHandler.ActorsRoot, true);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.HideAllActors:
                    {
                        if (RootsHandler.ActorsRoot)
                        {
                            Framework.Core.CommonUtility.SetActive(RootsHandler.ActorsRoot, false);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.ShowTypeActors:
                    {
                        Transform root = RootsHandler.FindActorRoot(param.idValue);
                        if (root != null)
                        {
                            Framework.Core.CommonUtility.SetActive(root, true);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.HideTypeActors:
                    {
                        Transform root = RootsHandler.FindActorRoot(param.idValue);
                        if (root != null)
                        {
                            Framework.Core.CommonUtility.SetActive(root, false);
                        }
                        return;
                    }
#endif
                case TriggerEventParameter.EDataType.ShowPlayerActor:
                    {
                        Framework.Module.AFramework framework = m_pFramework as Framework.Module.AFramework;
                        if(framework!=null)
                        {
                            Actor pActor = framework.world.FindNodeByConfig<Actor>(EActorType.Player, (uint)param.idValue);
                            if (pActor != null) pActor.SetFlag(EWorldNodeFlag.Visible, true);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.HidePlayerActor:
                    {
                        Framework.Module.AFramework framework = m_pFramework as Framework.Module.AFramework;
                        if(framework!=null)
                        {
                            Actor pActor = framework.world.FindNodeByConfig<Actor>(EActorType.Player, (uint)param.idValue);
                            if (pActor != null) pActor.SetVisible(false);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.ShowNpcActor:
                    {
                        Framework.Module.AFramework framework = m_pFramework as Framework.Module.AFramework;
                        if(framework!=null)
                        {
                            Actor pActor = framework.world.FindNodeByConfig<Actor>(EActorType.Monster, (uint)param.idValue);
                            if (pActor != null) pActor.SetFlag(EWorldNodeFlag.Visible, true);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.HideNpcActor:
                    {
                        Framework.Module.AFramework framework = m_pFramework as Framework.Module.AFramework;
                        if (framework != null)
                        {
                            Actor pActor = framework.world.FindNodeByConfig<Actor>(EActorType.Monster, (uint)param.idValue);
                            if (pActor != null) pActor.SetVisible(false);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.ShowSummonActor:
                    {
                        Framework.Module.AFramework framework = m_pFramework as Framework.Module.AFramework;
                        if (framework != null)
                        {
                            Actor pActor = framework.world.FindNodeByConfig<Actor>(EActorType.Summon, (uint)param.idValue);
                            if (pActor != null) pActor.SetFlag(EWorldNodeFlag.Visible, true);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.HideSummonActor:
                    {
                        Framework.Module.AFramework framework = m_pFramework as Framework.Module.AFramework;
                        if (framework != null)
                        {
                            Actor pActor = framework.world.FindNodeByConfig<Actor>(EActorType.Summon, (uint)param.idValue);
                            if (pActor != null) pActor.SetVisible(false);
                        }
                        return;
                    }
#if !USE_SERVER
                case TriggerEventParameter.EDataType.ShowGameRoot:
                    {
                        if (!string.IsNullOrEmpty(param.stringValue))
                        {
                            AWorldNode node = GetTriggerNode();
                            bool bDeal = false;
                            if (node != null)
                            {
                                Transform slotNode = node.FindBindSlot(param.stringValue);
                                if (slotNode)
                                {
                                    bDeal = true;
                                    slotNode.gameObject.SetActive(true);
                                }
                            }
                            if (!bDeal)
                                DyncmicTransformCollects.ActiveNodeByName(param.stringValue, true);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.HideGameRoot:
                    {
                        if (!string.IsNullOrEmpty(param.stringValue))
                        {
                            AWorldNode node = GetTriggerNode();
                            bool bDeal = false;
                            if (node != null)
                            {
                                Transform slotNode = node.FindBindSlot(param.stringValue);
                                if (slotNode)
                                {
                                    bDeal = true;
                                    slotNode.gameObject.SetActive(false);
                                }
                            }
                            if (!bDeal)
                                DyncmicTransformCollects.ActiveNodeByName(param.stringValue, false);
                        }
                        return;
                    }
#endif
                case TriggerEventParameter.EDataType.StopAllProjectile:
                    {
                        Framework.Module.AFramework framework = m_pFramework as Framework.Module.AFramework;
                        if (framework != null)
                        {
                            framework.GetProjectileManager().StopAllProjectiles();
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.EnableBodyPart:
                    {
                        Actor actor = GetTriggerActor();
                        if (actor != null)
                        {
                            actor.GetActorParameter().EnableBodyPart((uint)param.idValue, true);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.DisableBodyPart:
                    {
                        Actor actor = GetTriggerActor();
                        if (actor != null)
                        {
                            actor.GetActorParameter().EnableBodyPart((uint)param.idValue, false);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.SlotAvatar:
                    {
                        Actor actor = GetTriggerActor();
                        if (actor != null && actor.GetActorAgent() != null)
                        {
                            actor.GetActorAgent().EnableAvatar((int)param.idValue, param.stringValue.CompareTo("1") == 0);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.TriggerIDEvent:
                    {
                        AFrameworkModule pGameMoudle = m_pFramework as AFrameworkModule;
                        if (pGameMoudle != null)
                        {
                            pGameMoudle.OnTriggerEvent(param.idValue, false);
                        }
                        return;
                    }
            }
            base.OnTrigger(param);
        }
        //------------------------------------------------------
        protected override void OnTargetPath(TargetPathEventParameter param)
        {
#if !USE_SERVER
            CameraAnimPath path = CameraController.getInstance().GetEffect<CameraAnimPath>();
            if (path != null)
            {
                FFloat fTerrainHeight = FFloat.zero;
                if (TerrainManager.SampleTerrainHeight(m_pFramework as AFrameworkModule, TriggerEventRealPos, Vector3.up, ref fTerrainHeight, 1000))
                {
                    TriggerEventRealPos.y = fTerrainHeight;
                }

                if (param.ctlType == TargetPathEventParameter.EType.Play)
                {
                    path.SetPathID((ushort)param.idValue, param.bAbsolute, null, false);
                    path.SetTriggerPosition(TriggerEventRealPos + m_EventOffset);
                    path.Start(param.bAbsolute ? ACameraEffect.EType.LockSet : ACameraEffect.EType.Offset);
                    if (path.playable != null)
                    {
                        path.playable.SetMirror(false);
                        if (param.mirrorDirector && Vector3.Dot(TriggerActorDir, Vector3.forward) < 0)
                            path.playable.SetMirror(true);
                    }
                }
                else if (param.ctlType == TargetPathEventParameter.EType.ForcePlay)
                {
                    path.SetPathID((ushort)param.idValue, param.bAbsolute, null, true);
                    path.SetTriggerPosition(TriggerEventRealPos + m_EventOffset);
                    path.Start(param.bAbsolute ? ACameraEffect.EType.LockSet : ACameraEffect.EType.Offset);

                    if (path.playable != null)
                    {
                        path.playable.SetMirror(false);
                        if (param.mirrorDirector && Vector3.Dot(TriggerActorDir, Vector3.forward) < 0)
                            path.playable.SetMirror(true);
                    }
                }
                else if (param.ctlType == TargetPathEventParameter.EType.Stop)
                {
                    GameInstance.getInstance().animPather.Stop(param.idValue);
                }
                else if (param.ctlType == TargetPathEventParameter.EType.Skip)
                {
                    float skipTime = 0;
                    if (float.TryParse(param.userParam, out skipTime))
                        GameInstance.getInstance().animPather.SkipTo(skipTime, param.idValue);
                }
            }
#endif
        }
        //------------------------------------------------------
        protected override void OnCameraToggle(CameraToggleEventParameter param)
        {
#if !USE_SERVER
            AFrameworkModule pGameMoudle = m_pFramework as AFrameworkModule;
            if (pGameMoudle == null) return;
            switch (param.toggle)
            {
                case CameraToggleEventParameter.EToggle.Enable:
                    {
                        pGameMoudle.cameraController.Enable(true);

                    }
                    break;
                case CameraToggleEventParameter.EToggle.Disable:
                    {
                        pGameMoudle.cameraController.Enable(false);
                    }
                    break;
                case CameraToggleEventParameter.EToggle.Open:
                    {
                        pGameMoudle.cameraController.CloseCameraRef(false);
                    }
                    break;
                case CameraToggleEventParameter.EToggle.Close:
                    {
                        pGameMoudle.cameraController.CloseCameraRef(true);
                    }
                    break;
                case CameraToggleEventParameter.EToggle.Active:
                    {
                        pGameMoudle.cameraController.ActiveRoot(true);
                    }
                    break;
                case CameraToggleEventParameter.EToggle.UnActive:
                    {
                        pGameMoudle.cameraController.ActiveRoot(false);
                    }
                    break;
            }
#endif
        }
        //------------------------------------------------------
        protected override void OnTrackBind(TrackBindEventParameter param)
        {
#if !USE_SERVER
            if (param.valueID <= 0 || string.IsNullOrEmpty(param.trackName)) return;
            AFrameworkModule pGameMoudle = m_pFramework as AFrameworkModule;
            if (pGameMoudle == null) return;

            Vector3 lookPos = pGameMoudle.cameraController.GetCurrentLookAt();
            if (param.idType == TrackBindEventParameter.EIDType.Projectile)
            {
                float nearlyDist = float.MaxValue;
                AProjectile nearly = null;
                GameInstance.getInstance().animPather.CollectTrackBind(pGameMoudle.shareParams.catchUserDataSet, null, param.targetPath);
                foreach (var db in pGameMoudle.GetProjectileManager().GetRunningProjectile())
                {
                    if (db.Value.projectile == null || db.Value.projectile.id != param.valueID) continue;
                    if (pGameMoudle.shareParams.catchUserDataSet.Contains(db.Value)) continue;
                    float dst = (db.Value.position - lookPos).sqrMagnitude;
                    if (dst <= nearlyDist)
                    {
                        nearlyDist = dst;
                        nearly = db.Value;
                    }
                }
                if (nearly != null && nearly.GetObjectAble() != null)
                {
                    GameInstance.getInstance().animPather.TrackBind(param.trackName, nearly.GetObjectAble() as AInstanceAble, nearly, param.targetPath, param.bGenericBinding);
                }
                pGameMoudle.shareParams.catchUserDataSet.Clear();
                return;
            }
            Actor pActor = null;
            if (param.idType == TrackBindEventParameter.EIDType.Player)
            {
                GameInstance.getInstance().animPather.CollectTrackBind(pGameMoudle.shareParams.catchUserDataSet, null, param.targetPath);
                pActor = pGameMoudle.world.FindNearlyNodeByConfig<Actor>(lookPos, EActorType.Player, param.valueID, pGameMoudle.shareParams.catchUserDataSet);
            }
            else if (param.idType == TrackBindEventParameter.EIDType.Npc)
            {
                GameInstance.getInstance().animPather.CollectTrackBind(pGameMoudle.shareParams.catchUserDataSet, null, param.targetPath);
                pActor = pGameMoudle.world.FindNearlyNodeByConfig<Actor>(lookPos, EActorType.Monster, param.valueID, pGameMoudle.shareParams.catchUserDataSet);
            }
            else if (param.idType == TrackBindEventParameter.EIDType.Summon)
            {
                GameInstance.getInstance().animPather.CollectTrackBind(pGameMoudle.shareParams.catchUserDataSet, null, param.targetPath);
                pActor = pGameMoudle.world.FindNearlyNodeByConfig<Actor>(lookPos, EActorType.Summon, param.valueID, pGameMoudle.shareParams.catchUserDataSet);
            }

            pGameMoudle.shareParams.catchUserDataSet.Clear();
            if (pActor == null || pActor.GetObjectAble() == null) return;
            GameInstance.getInstance().animPather.TrackBind(param.trackName, pActor.GetObjectAble() as AInstanceAble, pActor, param.targetPath, param.bGenericBinding);
#endif
        }
        //------------------------------------------------------
        protected override void OnPostRender(PostRenderEventParameter param)
        {
#if !USE_SERVER
            switch (param.postType)
            {
                case PostRenderEventParameter.EType.ScreenFade:
                    ScreenFadeLogic.Fade(param.GetFadeColor(), param.curve);
                    break;
            }
#endif
        }
        //------------------------------------------------------
        protected override void OnInstance(InstanceEventParameter param)
        {
#if !USE_SERVER
            if(InstanceSpawner!=null)
            {
                Transform parent = null;
                if (pGameObject) parent = pGameObject.transform;
                if (parent == null && pInstnaceAble != null)
                    parent = pInstnaceAble.GetTransorm();

                InstanceSpawner.Spawn(param.strFile, param.bAbs, m_EventOffset, param.euler, parent);
            }
            else
            {
                FileSystemUtil.SpawnInstance(param.strFile);
            }
#endif
        }
        //------------------------------------------------------
        protected override void OnRunerSpeed(RunerSpeedEventParameter param)
        {
        }
    }
}

