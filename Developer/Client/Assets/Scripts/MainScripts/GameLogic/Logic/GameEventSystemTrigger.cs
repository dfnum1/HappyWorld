/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GameEventSystemTrigger
作    者:	HappLI
描    述:	
*********************************************************************/

using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using Framework.Core;
using Framework.Logic;
using Framework.Base;
using Framework.Module;
using Framework.BattlePlus;
using ExternEngine;
using TopGame.Data;
using TopGame.SvrData;

namespace TopGame.Logic
{
    public class GameEventSystemTrigger : EventSystemTrigger
    {
        //------------------------------------------------------
        T GetState<T>() where T : AState
        {
#if USE_SERVER
             if(m_pFramework is GameInstance) return ((GameInstance)m_pFramework).GetBattleState() as T;
             return null;
#else
            StateFactory stateFactory = m_pFramework.Get<StateFactory>();
            if (stateFactory == null || stateFactory.GetCurrentState() == null) return null;
            return stateFactory.GetCurrentState() as T;
#endif
        }
        //------------------------------------------------------
        T GetStateLogic<T>() where T : AStateLogic
        {
#if USE_SERVER
             AState state = GetState<AState>();
             if(state!=null) return state.FindLogic<T>();
             return null;
#else
            return AState.CastCurrentLogic<T>(m_pFramework);
#endif
        }
        //------------------------------------------------------
        T GetStateMode<T>() where T : AbsMode
        {
#if USE_SERVER
             AState state = GetState<AState>();
             if(state!=null) return state.GetCurrentMode<T>();
             return null;
#else
            return AState.CastCurrentMode<T>(m_pFramework);
#endif
        }
        //-------------------------------------------------
        bool ConvertRodeToPosX(short rode, out FFloat rodeX)
        {
            rodeX = 0;
            AFrameworkModule frameModule = GetFrameWorkMoudle();
            if (frameModule == null) return false;
            if (frameModule.RodeToPosX(rode, out rodeX))
            {
                return true;
            }
            return false;
        }
        //-------------------------------------------------

#if !USE_SERVER
        //------------------------------------------------------
        public override void CollectPreload(BaseEventParameter param, List<string> vFiles, HashSet<string> vAssets)
        {
            AFramework pFramework = m_pFramework as AFramework;
            if (pFramework == null) return;
            base.CollectPreload(param, vFiles, vAssets);
            switch(param.GetEventType())
            {
                case EEventType.Projectile:
                    {
                        if (pFramework.GetProjectileManager() == null) return;
                        ProjectileEventParameter proj = param as ProjectileEventParameter;
                        if (proj == null) return;
                        if (proj.projectileID > 0 && ModuleManager.mainModule != null)
                        {
                            ProjectileData projData = pFramework.GetProjectileManager().GetProjectileData(proj.projectileID);
                            if (projData != null)
                            {
                                vFiles.Add(projData.effect);
                                if (projData.HitEventID > 0)
                                {
                                    Data.EventData pEvent = Data.DataManager.getInstance().EventDatas.GetData(projData.HitEventID);
                                    if (pEvent != null)
                                    {
                                        pEvent.PreloadCollect(pFramework, vFiles, vAssets);
                                    }
                                }
                                if (projData.AttackEventID > 0)
                                {
                                    Data.EventData pEvent = Data.DataManager.getInstance().EventDatas.GetData(projData.AttackEventID);
                                    if (pEvent != null)
                                    {
                                        pEvent.PreloadCollect(pFramework, vFiles, vAssets);
                                    }
                                }
                                if (projData.OverEventID > 0)
                                {
                                    Data.EventData pEvent = Data.DataManager.getInstance().EventDatas.GetData(projData.OverEventID);
                                    if (pEvent != null)
                                    {
                                        pEvent.PreloadCollect(pFramework, vFiles, vAssets);
                                    }
                                }
                                if (projData.StepEventID > 0)
                                {
                                    Data.EventData pEvent = Data.DataManager.getInstance().EventDatas.GetData(projData.StepEventID);
                                    if (pEvent != null)
                                    {
                                        pEvent.PreloadCollect(pFramework, vFiles, vAssets);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case EEventType.Sound:
                    {
                        SoundEventParameter sound = param as SoundEventParameter;
                        if (sound == null) return;
                        if (!string.IsNullOrEmpty(sound.strFile))
                            vAssets.Add(sound.strFile);
                        else if (sound.audioId > 0)
                        {
                            List<Data.CsvData_Audio.AudioData> audios = Data.DataManager.getInstance().Audio.GetData(sound.audioId);
                            if (audios != null)
                            {
                                for (int i = 0; i < audios.Count; ++i)
                                {
                                    if (string.IsNullOrEmpty(audios[i].location) || audios[i].location.StartsWith("event:/"))
                                        continue;
                                    vAssets.Add(audios[i].location);
                                }
                            }
                        }
                    }
                    break;
                case EEventType.TargetPath:
                    {
                        TargetPathEventParameter targetPath = param as TargetPathEventParameter;
                        if (targetPath.ctlType == TargetPathEventParameter.EType.Play || targetPath.ctlType == TargetPathEventParameter.EType.ForcePlay)
                        {
                             var targetPathData = Data.DataManager.getInstance().TargetPaths.GetData(targetPath.idValue);
                            if(targetPathData!=null)
                            {
                                if (!string.IsNullOrEmpty(targetPathData.animClip)) vAssets.Add(targetPathData.animClip);
                                if (!string.IsNullOrEmpty(targetPathData.timeline)) vAssets.Add(targetPathData.timeline);
                                if (targetPathData.events != null)
                                {
                                    for (int i = 0; i < targetPathData.events.Length; ++i)
                                    {
                                        CollectPreload(targetPathData.events[i], vFiles, vAssets);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case EEventType.Trigger:
                    {
                        TriggerEventParameter triggerEvt = param as TriggerEventParameter;
                        if (triggerEvt == null) return;
                        switch(triggerEvt.dataType)
                        {
                            case TriggerEventParameter.EDataType.Dialog:
                                {
                                    var dialogData = Data.DataManager.getInstance().Dialog.GetData((uint)triggerEvt.idValue);
                                    if (dialogData != null)
                                    {
                                        int stackLoop = 100;
                                        while(dialogData!=null && stackLoop>0)
                                        {
                                            stackLoop--;
                                            if (dialogData.Models_nModel_data != null)
                                                vAssets.Add(dialogData.Models_nModel_data.strFile);
                                            dialogData = Data.DataManager.getInstance().Dialog.GetData(dialogData.nNextID);
                                        }
                                    }
                                }
                                break;
                            case TriggerEventParameter.EDataType.Guide:
                                {
                                    var guideData = Framework.Plugin.Guide.GuideDatas.GetGuideGroup(triggerEvt.idValue);
                                    if(guideData!=null && guideData.GetAllNodes()!=null)
                                    {
                                        var nodes = guideData.GetAllNodes();
                                        foreach (var db in nodes)
                                        {
                                            List<Framework.Plugin.AT.IUserData> vEvents = db.Value.GetBeginEvents();
                                            if(vEvents!=null)
                                            {
                                                for(int j = 0; j < vEvents.Count; ++j)
                                                {
                                                    if (vEvents[j] != null && vEvents[j]!= param) (vEvents[j] as BaseEventParameter).CollectPreload(pFramework, vFiles, vAssets);
                                                }
                                            }
                                            vEvents = db.Value.GetEndEvents();
                                            if (vEvents != null)
                                            {
                                                for (int j = 0; j < vEvents.Count; ++j)
                                                {
                                                    if (vEvents[j] != null && vEvents[j] != param) (vEvents[j] as BaseEventParameter).CollectPreload(pFramework, vFiles, vAssets);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case TriggerEventParameter.EDataType.TriggerIDEvent:
                                {
                                    var eventData = Data.DataManager.getInstance().EventDatas.GetData(triggerEvt.idValue);
                                    if (eventData != null)
                                    {
                                        eventData.PreloadCollect(pFramework, vFiles, vAssets);
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }
        }
#endif
        //------------------------------------------------------
        protected override void PrepareEventDatas(BaseEventParameter pEvent)
        {
            base.PrepareEventDatas(pEvent);
            Actor TriggerActor = GetTriggerActor();
            if (TriggerActorActionState!=null && TriggerActor !=null && TriggerActor.GetActorAgent()!=null)
            {
                if ((pEvent.triggerBit & ((int)EEventBit.LockTeamMember)) != 0)
                {
                    AFrameworkModule moudle = GetFrameWorkMoudle();
                    if(moudle != null && moudle.battleWorld!=null)
                    {
                        BattleWorld battleWorld= moudle.battleWorld as BattleWorld;
                        if (battleWorld == null) return;
                        List<Actor> vMembers = battleWorld.GetPlayers(0);
                        if (vMembers == null) return;
                        for (int i = 0; i < vMembers.Count; ++i)
                        {
                            if (vMembers[i] != null)
                                TriggerActor.AddLockTarget(vMembers[i], i == 0);
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        public Actor GetTeamFirstTriggerActor()
        {
            AFrameworkModule moudle = GetFrameWorkMoudle();
            if (moudle == null) return null;
            BattleWorld battleWorld = moudle.battleWorld as BattleWorld;
            if (battleWorld == null || battleWorld.GetPlayers() == null || battleWorld.GetPlayers().Count<=0) return null;
            return battleWorld.GetPlayers()[0];
        }
#if !USE_SERVER
        //------------------------------------------------------
        public override bool CanTrigger(BaseEventParameter param)
        {
            if ((param.triggerBit & ((int)EEventBit.OnlyWorld)) != 0)
            {
                if (GameInstance.getInstance().GetState() == EGameState.Hall)
                    return true;
                else
                    return false;
            }
            return true;
        }
#endif
        //------------------------------------------------------
        protected override void OnProjectile(ProjectileEventParameter projParam)
        {
            if (projParam.eBornType == ProjectileEventParameter.EBornType.RandomRode && (projParam.nRode <= -1 || projParam.nRode >= 1))
            {
                short rode = (short)m_pFramework.GetRamdom(-Mathf.Abs(projParam.nRode), Mathf.Abs(projParam.nRode) + 1);
                FFloat roadX;
                if (ConvertRodeToPosX(rode, out roadX))
                {
                    TriggerEventPos.x = roadX;
                    TriggerEventRealPos.x = roadX;
                }
            }
            else if (projParam.eBornType == ProjectileEventParameter.EBornType.SpawnTeamX)
            {
                AFrameworkModule frameworkMoudle = GetFrameWorkMoudle();
                if (frameworkMoudle != null)
                {
                    FFloat roadX;
                    if(ConvertRodeToPosX(frameworkMoudle.GetCurrentRode(), out roadX))
                    {
                        TriggerEventPos.x = roadX;
                        TriggerEventRealPos.x = TriggerEventPos.x;
                    }
                }
            }
            else if (projParam.eBornType == ProjectileEventParameter.EBornType.AssignRode && Mathf.Abs(projParam.nRode) < 255)
            {
                FFloat roadX ;
                if (ConvertRodeToPosX(projParam.nRode, out roadX))
                {
                    TriggerEventPos.x = roadX;
                    TriggerEventRealPos.x = roadX;
                }
            }
            base.OnProjectile(projParam);
        }
#if !USE_SERVER
        //------------------------------------------------------
        protected override void OnParticle(ParticleEventParameter param)
        {
            if (ATuserData != null && ATuserData is Timeline.EventPlayableBehavior)
            {
                InstanceOperiaon pOp = FileSystemUtil.SpawnInstance(param.partile, true);
                if (pOp != null)
                {
                    Vector3 pos = m_EventOffset;
                    Vector3 euler = param.euler;
                    pOp.pByParent = RootsHandler.ParticlesRoot;
                    pOp.OnCallback = (ATuserData as Timeline.EventPlayableBehavior).OnSpawnBehaviour;
                    pOp.OnSign = (ATuserData as Timeline.EventPlayableBehavior).OnSpawnSign;
                    pOp.userData0 = new Variable3() { floatVal0 = pos.x, floatVal1 = pos.y, floatVal2 = pos.z };
                    pOp.userData1 = new Variable3() { floatVal0 = euler.x, floatVal1 = euler.y, floatVal2 = euler.z };
                    pOp.userData2 = new VariableString() { strValue = param.parent_slot };
                }
                return;
            }
            base.OnParticle(param);
        }
#endif
        //------------------------------------------------------
        protected override void OnInvincible(InvincibleEventParameter param)
        {
            if(param.groupBit == 0)
            {
                base.OnInvincible(param);
                return;
            }
            BattleStatus status = BattleKits.GetBattleLogic<BattleStatus>(m_pFramework);
            if(status != null)
                status.ApplayInvincible(param.groupBit, param.fDuration, param.bAbs);
        }
        //------------------------------------------------------
        protected override void OnSummon(SummonEventParameter param)
        {
            AFramework pFramework = m_pFramework as AFramework;
            if (pFramework == null) return;

            bool bRodeCheckOk = false;
            FFloat roadX = FFloat.zero;
            if (param.rodeType != SummonEventParameter.ERodeType.None)
            {
                if (param.rodeType == SummonEventParameter.ERodeType.AssignRode)
                {
                    if (ConvertRodeToPosX(param.nRode, out roadX))
                    {
                        roadX += param.position.x;
                        bRodeCheckOk = true;
                    }
                }
                else if (param.rodeType == SummonEventParameter.ERodeType.RandomRode)
                {
                    short rode = (short)m_pFramework.GetRamdom(-Mathf.Abs(param.nRode), Mathf.Abs(param.nRode) + 1);
                    ConvertRodeToPosX(rode, out roadX);
                    {
                        roadX += param.position.x;
                        bRodeCheckOk = true;
                    }
                }
            }

            int level = 0;
            if ((param.levelType & (1 << (int)SummonEventParameter.ELevelType.AddChapter)) != 0)
            {
                if (GetBattleWorld() != null)
                {
                    BattleSvrData svrData = GetBattleWorld().GetLogic<BattleSvrData>();
                    level += (svrData!=null)? svrData.GetChapterLevel():0;
                }
            }
            if ((param.levelType & (1 << (int)SummonEventParameter.ELevelType.AddTrigger)) != 0)
            {
                Actor triggerActor = GetTriggerActor();
                if (triggerActor != null) level += triggerActor.GetActorParameter().GetLevel();
            }
            if ((param.levelType & (1 << (int)SummonEventParameter.ELevelType.StateLevel)) != 0)
            {
                if (TriggerActorActionStateParam != null)
                {
                    if (TriggerActorActionStateParam is Skill)
                        level += (int)((Skill)TriggerActorActionStateParam).skillLevel;
                    else if (TriggerActorActionStateParam is BufferState)
                        level += (int)((BufferState)TriggerActorActionStateParam).level;
                }
            }
            if ((param.levelType & (1 << (int)SummonEventParameter.ELevelType.Add)) != 0)
            {
                level += param.externLevel;
            }
            if ((param.levelType & (1 << (int)SummonEventParameter.ELevelType.Mul)) != 0)
            {
                level *= param.externLevel;
            }
            if ((param.levelType & (1 << (int)SummonEventParameter.ELevelType.Equal)) != 0)
            {
                level = param.externLevel;
            }

            if (param.idType ==  SummonEventParameter.EIDType.Projectile)
            {
               ProjectileData projData = pFramework.GetProjectileManager().GetProjectileData(param.valueID);
                if (projData == null || ProjectileData.IsTrack(projData.type)) return;

                FVector3 up = FVector3.up;
                AWorldNode TriggerActor = GetTriggerNode();
                if (TriggerActor == null)
                {
                    TriggerActor = GetTeamFirstTriggerActor();
                    if (TriggerActor != null)
                    {
                        TriggerActorDir = TriggerActor.GetDirection();
                        if(GetFrameWorkMoudle()!=null)
                            TriggerEventRealPos = GetFrameWorkMoudle().GetPlayerPosition();
                    }
                }
                if (TriggerActor != null) up = TriggerActor.GetUp();

                FVector3 triggerDir = TriggerActorDir;
                FVector3 triggerPos = TriggerEventRealPos;
                FQuaternion dirAngle = FQuaternion.Euler(param.eulerAngle);
                if (param.bLocation)
                {
                    triggerPos += param.position;
                    triggerDir += dirAngle * Vector3.forward;
                }
                else
                {
                    triggerPos= param.position;
                    triggerDir= dirAngle * Vector3.forward;
                }
                if (bRodeCheckOk) triggerPos.x = roadX;
                ATerrainLayers.AdjustTerrainHeight(pFramework, ref triggerPos, up, param.heightType);

                List<Framework.Plugin.AT.IUserData> vCatchs = pFramework.shareParams.catchUserDataList;
                int begin = vCatchs.Count;
                pFramework.GetProjectileManager().LaunchProjectile(param.valueID, TriggerActor, TriggerActorActionState, TriggerActorActionStateParam, 0xffffffff, triggerPos, triggerDir, null, 0, 0, null, vCatchs);
                for (int i = 1; i < param.nCount; ++i)
                    pFramework.GetProjectileManager().LaunchProjectile(param.valueID, TriggerActor, TriggerActorActionState, TriggerActorActionStateParam, 0xffffffff, triggerPos, triggerDir, null, 0, param.fStepDuration * i, null, vCatchs);
                for(int i =begin; i < vCatchs.Count;++i)
                {
                    Projectile projectile = vCatchs[i] as Projectile;
                    if (param.dieKeep)
                        projectile.launch_flag |= (int)ELaunchFlag.DieKeep;
                    projectile.damage_id_offset = param.externLevel;
                    projectile.targetPosRode = param.GetTargetRode(pFramework);
                }
                vCatchs.RemoveRange(begin, vCatchs.Count - begin);
            }
            else if(param.idType == SummonEventParameter.EIDType.Player)
            {
                BattleWorld battleWorld = GetBattleWorld();
                if(battleWorld != null)
                {
                    if(!battleWorld.AddPlayer(0,Data.DataManager.getInstance().Player.GetData((uint)param.valueID), param.nCount, level))
                        Framework.Plugin.Logger.Error("召唤添加角色失败");
                }
            }
            else if (param.idType == SummonEventParameter.EIDType.BattleObject)
            {
                uint sceneTheme = 0;
                AbsMode pState = GetStateMode<AbsMode>();
                if (pState != null) sceneTheme = pState.GetSceneTheme();
                Data.CsvData_BattleObject.BattleObjectData obsData = Data.DataManager.getInstance().BattleObject.GetData(sceneTheme, (uint)param.valueID);
                if (obsData != null)
                {
                    if(param.limitUpper>0)
                    {
                        int cnt = pFramework.world.StatNodeByConfigID((int)param.valueID, 1 << (int)EActorType.Element);
                        if (cnt >= param.limitUpper)
                        {
#if UNITY_EDITOR
                            Framework.Plugin.Logger.Error("已达数量上线");
#endif
                            return;
                        }
                    }

                    Vector3 triggerPos = param.position + TriggerEventRealPos;
                    if (bRodeCheckOk) triggerPos.x = roadX;
                    ObsElement monster = pFramework.world.CreateNode<ObsElement>(EActorType.Element, obsData);
                    if (monster != null)
                    {
                        monster.SetActived(true);
                        monster.SetVisible(true);
                        monster.SetSpatial(true);
                        monster.EnableLogic(true);
                        monster.SetCollectAble(true);
                        monster.SetAttackGroup((int)EActorType.Monster);
                        monster.SetPosition(triggerPos);
                        monster.SetEulerAngle(param.eulerAngle);
                        monster.SetTerrainType(param.heightType);
                        monster.PlayState("enter");
                    }
                }
                else
                {
                    Framework.Plugin.Logger.Error("召唤怪物[" + param.valueID + "]不存在，请确认");
                }
            }
            else if (param.idType == SummonEventParameter.EIDType.Monster)
            {
                Data.CsvData_Monster.MonsterData monsterData =  Data.DataManager.getInstance().Monster.GetData((uint)param.valueID);
                if(monsterData!=null)
                {
                    if (param.limitUpper > 0)
                    {
                        int cnt = pFramework.world.StatNodeByConfigID((int)param.valueID, 1 << (int)EActorType.Monster);
                        if (cnt >= param.limitUpper)
                        {
#if UNITY_EDITOR
                            Framework.Plugin.Logger.Error("已达数量上线");
#endif
                            return;
                        }
                    }
                    Monster monster = pFramework.world.CreateNode<Monster>(EActorType.Monster, monsterData);
                    if (monster != null)
                    {
                        Vector3 triggerPos = param.position + TriggerEventRealPos;
                        if (bRodeCheckOk) triggerPos.x = roadX;
                        monster.SetActived(true);
                        monster.SetVisible(true);
                        monster.SetSpatial(true);
                        monster.EnableLogic(true);
                        monster.SetCollectAble(true);
                        monster.SetAttackGroup((int)EActorType.Monster);
                        monster.SetPosition(triggerPos);
                        monster.SetEulerAngleImmediately(param.eulerAngle);
                        monster.SetTerrainType(param.heightType);
                        monster.EnableSkill(true);
                        if (monster.GetActorParameter() != null)
                            monster.GetActorParameter().SetLevel((ushort)Mathf.Max(1, level));
                        
                        BattleKits.ApplayExpandChapterAttr(monster, false, null, false);
                        BattleKits.ApplayAdjustAttrCoefficientFormula(monster, true);
                        BattleKits.ApplayExpandChapterBuff(monster);

                        monster.GetActorParameter().ResetRunTimeParameter(true, true);

                        monster.StartActionByType(EActionStateType.Enter, 0, 1, true, false, true);
                    }
                }
                else
                {
                    Framework.Plugin.Logger.Error("召唤怪物[" + param.valueID + "]不存在，请确认");
                }
            }
            else if (param.idType == SummonEventParameter.EIDType.Summon)
            {
                Actor TriggerActor = GetTriggerActor();
                if (TriggerActor != null)
                {
                    for(int i = 0; i < param.nCount; ++i)
                    {
                        Data.CsvData_Summon.SummonData pSummon = Data.DataManager.getInstance().Summon.GetSummon(param.valueID, (ushort)level);
                        if (pSummon != null)
                        {
                            if (param.limitUpper > 0)
                            {
                                int cnt = pFramework.world.StatNodeByConfigID((int)param.valueID, 1 << (int)EActorType.Summon);
                                if (cnt >= param.limitUpper)
                                {
#if UNITY_EDITOR
                                    Framework.Plugin.Logger.Error("已达数量上线");
#endif
                                    return;
                                }
                            }
                            int useSlot = -1;
                            Vector3 followOffset = param.position;
                            if (param.userFlag != 0)
                            {
                                uint flags = TriggerActor.GetSummonPositionFlags();
                                Data.SummonSlotFormation.TypeData typeData = Data.SummonSlotFormation.GetFormation(EActorType.Summon);
                                if (!typeData.isValid)
                                {
#if UNITY_EDITOR
                                    Framework.Plugin.Logger.Error("没有可用的召唤数据");
#endif
                                    return;
                                }
                                followOffset += typeData.offset;
                                for(int j =0; j < typeData.format.Length; ++j)
                                {
                                    if( (flags & (1<<j)) == 0)
                                    {
                                        useSlot = j;
                                        followOffset = typeData.offset + typeData.format[j].slot;
                                        break;
                                    }
                                }
                                if (useSlot < 0)
                                {
                                    Framework.Plugin.Logger.Error("没有可用的召唤槽位");
                                    break;
                                }
                            }
                            else
                            {
                                followOffset += this.TriggerEventRealPos;
                            }

                            Summon actor = pFramework.world.CreateNode<Summon>(EActorType.Summon, pSummon);
                            if (actor != null)
                            {
                                float fDuration = 0;
                                if (pSummon.duration > 0) fDuration = pSummon.duration * 0.001f;
                                else fDuration = param.externParam * 0.001f;

                                actor.GetActorParameter().SetLevel((ushort)level);
                                if (bRodeCheckOk) followOffset.x = roadX;
                                actor.SetFollow(TriggerActor, param.externParam1!=0, followOffset, param.eulerAngle, 0, useSlot);
                                actor.SetLifeTime(fDuration);
                                actor.StartActionByType(EActionStateType.Enter, 0, 1, true, false, true);

                                if (param.userFlag!=0)
                                {
                                    TriggerActor.SetSummonPosition(useSlot, true);
                                }
                                BattleWorld battleWorld = GetBattleWorld();
                                if(battleWorld!=null && battleWorld.IsStartingAndActive())
                                {
                                    actor.EnableAI(true);
                                    actor.EnableSkill(true);
                                    actor.EnableLogic(true);
                                    if (actor.GetSkill()!=null)
                                    {
                                        actor.GetSkill().AutoSkill(true);
                                        actor.GetSkill().EnableCommand(true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Framework.Plugin.Logger.Error("召唤[" + param.valueID + "]不存在，请确认");
                        }
                    }
                }
                else
                {
                    Framework.Plugin.Logger.Error("没有触发者，请确认");
                }
            }
            else if (param.idType == SummonEventParameter.EIDType.Wingman)
            {
                for (int i = 0; i < param.nCount; ++i)
                {
                    Data.CsvData_Summon.SummonData pSummon = Data.DataManager.getInstance().Summon.GetData(param.valueID);
                    if (pSummon != null)
                    {
                        if (param.limitUpper > 0)
                        {
                            int cnt = pFramework.world.StatNodeByConfigID((int)param.valueID, 1 << (int)EActorType.Wingman);
                            if (cnt >= param.limitUpper)
                            {
#if UNITY_EDITOR
                                Framework.Plugin.Logger.Error("已达数量上线");
#endif
                                return;
                            }
                        }
                        Vector3 followOffset = param.position;
                        followOffset += this.TriggerEventRealPos;

                        Wingman actor = pFramework.world.CreateNode<Wingman>(EActorType.Wingman, pSummon);
                        if (actor != null)
                        {
                            float fDuration = 0;
                            if (pSummon.duration > 0) fDuration = pSummon.duration * 0.001f;
                            else fDuration = param.externParam * 0.001f;

                            AWorldNode pTriggerNode = GetTriggerNode();
                            if(pTriggerNode!=null)
                                actor.SetAttackGroup(pTriggerNode.GetAttackGroup());
                            actor.GetActorParameter().SetLevel((ushort)level);
                            if (bRodeCheckOk) followOffset.x = roadX;
                            if (param.externParam1 != 0)
                            {
                                //跟随
                            }
                            actor.StartActionByType(EActionStateType.Enter, 0, 1, true, false, true);

                            BattleWorld battleWorld = GetBattleWorld();
                            if (battleWorld != null && battleWorld.IsStartingAndActive())
                            {
                                actor.EnableAI(true);
                                actor.EnableSkill(true);
                                actor.EnableLogic(true);
                                if (actor.GetSkill() != null)
                                {
                                    actor.GetSkill().AutoSkill(true);
                                    actor.GetSkill().EnableCommand(true);
                                }
                            }
                            
       
                            //添加buff
                            actor.AddBuffs();
                        }
                    }
                    else
                    {
                        Framework.Plugin.Logger.Error("召唤[" + param.valueID + "]不存在，请确认");
                    }
                }
            }
        }
        //------------------------------------------------------
        protected override void OnLockEvent(LockEventParameter param)
        {
            base.OnLockEvent(param);

            if ((param.nLockBit & ((ushort)LockEventParameter.ELockType.PlayerSkill)) != 0)
            {
                BattleStatus pStatus = BattleKits.GetBattleLogic<BattleStatus>(m_pFramework);
                if (pStatus != null)
                    pStatus.LockPlayerSkill(param.bApplay);
#if UNITY_EDITOR
                if (DebugConfig.lockDebug)
                {
                    Framework.Plugin.Logger.Info(LockEventParameter.ELockType.PlayerSkill.ToString() + (param.bApplay ? "   加锁" : "   解锁"));
                }
#endif
            }
            if ((param.nLockBit & ((ushort)LockEventParameter.ELockType.MonsterSkill)) != 0)
            {
                BattleStatus pStatus = BattleKits.GetBattleLogic<BattleStatus>(m_pFramework);
                if (pStatus != null)
                    pStatus.LockMonsterSkill(param.bApplay);
#if UNITY_EDITOR
                if (DebugConfig.lockDebug)
                {
                    Framework.Plugin.Logger.Info(LockEventParameter.ELockType.MonsterSkill.ToString() + (param.bApplay ? "   加锁" : "   解锁"));
                }
#endif
            }
            if ((param.nLockBit & ((ushort)LockEventParameter.ELockType.PlayerPowerSkill)) != 0)
            {
                BattleStatus pStatus = BattleKits.GetBattleLogic<BattleStatus>(m_pFramework);
                if (pStatus != null)
                    pStatus.LockPlayerPowerSkill(param.bApplay);
#if UNITY_EDITOR
                if (DebugConfig.lockDebug)
                {
                    Framework.Plugin.Logger.Info(LockEventParameter.ELockType.PlayerPowerSkill.ToString() + (param.bApplay ? "   加锁" : "   解锁"));
                }
#endif
            }
            if ((param.nLockBit & ((ushort)LockEventParameter.ELockType.MonsterPowerSkill)) != 0)
            {
                BattleStatus pStatus = BattleKits.GetBattleLogic<BattleStatus>(m_pFramework);
                if (pStatus != null)
                    pStatus.LockMonsterPowerSkill(param.bApplay);
#if UNITY_EDITOR
                if (DebugConfig.lockDebug)
                {
                    Framework.Plugin.Logger.Info(LockEventParameter.ELockType.MonsterPowerSkill.ToString() + (param.bApplay ? "   加锁" : "   解锁"));
                }
#endif
            }
        }
        //------------------------------------------------------
        protected override void OnTimeScale(TimeScaleEventParamenter param)
        {
#if !USE_SERVER
            if (param.bUsedCurve)
            {
                GameInstance.getInstance().ApplayTimeScaleByCurve(param.timeCurve);
            }
            else
                base.OnTimeScale(param);
#endif
        }
        //------------------------------------------------------
        protected override void OnTrigger(TriggerEventParameter param)
        {
            AFramework pFramework = m_pFramework as AFramework;
            if (pFramework == null) return;

            switch (param.dataType)
            {
#if !USE_SERVER
                case TriggerEventParameter.EDataType.Guide:
                    {
                        Framework.Plugin.Guide.GuideWrapper.DoGuide(param.idValue, 1 << (int)GameInstance.getInstance().GetState(), null, param.stringValue.CompareTo("1") ==0);
                        return;
                    }
                case TriggerEventParameter.EDataType.Dialog:
                    {
                        float delay = 0;
                        if(float.TryParse(param.stringValue, out delay))
                        UI.DialogPanel.ShowDialog((uint)param.idValue, delay);
                        return;
                    }
#endif
                case TriggerEventParameter.EDataType.KillPlayer:
                    {
                        if(param.idValue <= 0)
                        {
                            if(ATuserData != null)
                            {
                                if(ATuserData is Actor)
                                {
                                    (ATuserData as Actor).SetFlag(EWorldNodeFlag.Killed, true);
                                }
                                else if(ATuserData is ObsElement)
                                {
                                    (ATuserData as ObsElement).SetFlag(EWorldNodeFlag.Killed, true);
                                }
                                else if (ATuserData is Projectile)
                                {
                                    (ATuserData as Projectile).remain_life_time = 0;
                                }
                            }
                            return;
                        }
#if !USE_SERVER
                        GameInstance.getInstance().animPather.CollectTrackBind(pFramework.shareParams.catchUserDataSet, null);
                        bool bHasPath = false;
                        foreach (var db in pFramework.shareParams.catchUserDataSet)
                        {
                            if(db is Actor)
                            {
                                Actor actor = db as Actor;
                                if(actor.GetActorType() == EActorType.Player && actor.GetConfigID() == param.idValue)
                                {
                                    actor.SetFlag(EWorldNodeFlag.Killed, true);
                                    bHasPath = true;
                                }
                            }
                        }
                        pFramework.shareParams.catchUserDataSet.Clear();
                        if (bHasPath) return;
#endif
                        List<AWorldNode> vNodes = pFramework.world.CatchNodeList;
                        pFramework.world.CollectNodes(ref vNodes);
                        for(int i =0; i < vNodes.Count; ++i)
                        {
                            if(vNodes[i].GetActorType() == EActorType.Player && vNodes[i].GetConfigID() == param.idValue)
                            {
                                vNodes[i].SetKilled(true);
                            }
                        }
                        vNodes.Clear();
                        return;
                    }
                case TriggerEventParameter.EDataType.DestroyPlayer:
                    {
                        if (param.idValue <= 0)
                        {
                            if (ATuserData != null)
                            {
                                if (ATuserData is Actor && (ATuserData as Actor).GetActorType() == EActorType.Player)
                                {
                                    (ATuserData as Actor).Destroy();
                                }
                                else if (ATuserData is ObsElement)
                                {
                                    (ATuserData as ObsElement).Destroy();
                                }
                                else if (ATuserData is Projectile)
                                {
                                    (ATuserData as Projectile).remain_life_time = 0;
                                }
                            }
                            return;
                        }
#if !USE_SERVER
                        GameInstance.getInstance().animPather.CollectTrackBind(pFramework.shareParams.catchUserDataSet, null);
                        bool bHasPath = false;
                        foreach (var db in pFramework.shareParams.catchUserDataSet)
                        {
                            if (db is Actor)
                            {
                                Actor actor = db as Actor;
                                if (actor.GetActorType() == EActorType.Player && actor.GetConfigID() == param.idValue)
                                {
                                    actor.Destroy();
                                    bHasPath = true;
                                }
                            }
                        }
                        pFramework.shareParams.catchUserDataSet.Clear();
                        if (bHasPath) return;
#endif
                        List<AWorldNode> vNodes = pFramework.world.CatchNodeList;
                        pFramework.world.CollectNodes(ref vNodes);
                        for (int i = 0; i < vNodes.Count; ++i)
                        {
                            if (vNodes[i].GetActorType() == EActorType.Player && vNodes[i].GetConfigID() == param.idValue)
                            {
                                vNodes[i].SetDestroy();
                            }
                        }
                        vNodes.Clear();
                        return;
                    }
                case TriggerEventParameter.EDataType.KillMonster:
                    {
                        if (param.idValue <= 0)
                        {
                            if (ATuserData != null)
                            {
                                if (ATuserData is Actor && (ATuserData as Actor).GetActorType() == EActorType.Monster)
                                {
                                    (ATuserData as Actor).SetFlag(EWorldNodeFlag.Killed, true);
                                }
                            }
                            return;
                        }
#if !USE_SERVER
                        GameInstance.getInstance().animPather.CollectTrackBind(pFramework.shareParams.catchUserDataSet, null);
                        bool bHasPath = false;
                        foreach (var db in pFramework.shareParams.catchUserDataSet)
                        {
                            if (db is Actor)
                            {
                                Actor actor = db as Actor;
                                if (actor.GetActorType() == EActorType.Monster && actor.GetConfigID() == param.idValue)
                                {
                                    actor.SetFlag(EWorldNodeFlag.Killed, true);
                                    bHasPath = true;
                                }
                            }
                        }
                        pFramework.shareParams.catchUserDataSet.Clear();
                        if (bHasPath) return;
#endif
                        List<AWorldNode> vNodes = pFramework.world.CatchNodeList;
                        pFramework.world.CollectNodes(ref vNodes);
                        for (int i = 0; i < vNodes.Count; ++i)
                        {
                            if (vNodes[i].GetActorType() == EActorType.Monster && vNodes[i].GetConfigID() == param.idValue)
                            {
                                vNodes[i].SetKilled(true);
                            }
                        }
                        vNodes.Clear();
                        return;
                    }
                case TriggerEventParameter.EDataType.DestroyMonster:
                    {
                        if (param.idValue <= 0)
                        {
                            if (ATuserData != null)
                            {
                                if (ATuserData is Actor && (ATuserData as Actor).GetActorType() == EActorType.Monster)
                                {
                                    (ATuserData as Actor).Destroy();
                                }
                            }
                            return;
                        }
#if !USE_SERVER
                        GameInstance.getInstance().animPather.CollectTrackBind(pFramework.shareParams.catchUserDataSet, null);
                        bool bHasPath = false;
                        foreach (var db in pFramework.shareParams.catchUserDataSet)
                        {
                            if (db is Actor)
                            {
                                Actor actor = db as Actor;
                                if (actor.GetActorType() == EActorType.Monster && actor.GetConfigID() == param.idValue)
                                {
                                    actor.Destroy();
                                    bHasPath = true;
                                }
                            }
                        }
                        pFramework.shareParams.catchUserDataSet.Clear();
                        if (bHasPath) return;
#endif
                        List<AWorldNode> vNodes = pFramework.world.CatchNodeList;
                        pFramework.world.CollectNodes(ref vNodes);
                        for (int i = 0; i < vNodes.Count; ++i)
                        {
                            if (vNodes[i].GetActorType() == EActorType.Monster && vNodes[i].GetConfigID() == param.idValue)
                            {
                                vNodes[i].SetDestroy();
                            }
                        }
                        vNodes.Clear();
                        return;
                    }
                case TriggerEventParameter.EDataType.KillSummon:
                    {
                        if (param.idValue <= 0)
                        {
                            if (ATuserData != null)
                            {
                                if (ATuserData is Actor && (ATuserData as Actor).GetActorType() == EActorType.Summon)
                                {
                                    (ATuserData as Actor).SetFlag(EWorldNodeFlag.Killed, true);
                                }
                            }
                            return;
                        }
#if !USE_SERVER
                        GameInstance.getInstance().animPather.CollectTrackBind(pFramework.shareParams.catchUserDataSet, null);
                        bool bHasPath = false;
                        foreach (var db in pFramework.shareParams.catchUserDataSet)
                        {
                            if (db is Actor)
                            {
                                Actor actor = db as Actor;
                                if (actor.GetActorType() == EActorType.Summon && actor.GetConfigID() == param.idValue)
                                {
                                    actor.SetFlag(EWorldNodeFlag.Killed, true);
                                    bHasPath = true;
                                }
                            }
                        }
                        pFramework.shareParams.catchUserDataSet.Clear();
                        if (bHasPath) return;
#endif
                        List<AWorldNode> vNodes = pFramework.world.CatchNodeList;
                        pFramework.world.CollectNodes(ref vNodes);
                        for (int i = 0; i < vNodes.Count; ++i)
                        {
                            if (vNodes[i].GetActorType() == EActorType.Summon && vNodes[i].GetConfigID() == param.idValue)
                            {
                                vNodes[i].SetKilled(true);
                            }
                        }
                        vNodes.Clear();
                        return;
                    }
                case TriggerEventParameter.EDataType.DestroySummon:
                    {
                        if (param.idValue <= 0)
                        {
                            if (ATuserData != null)
                            {
                                if (ATuserData is Actor && (ATuserData as Actor).GetActorType() == EActorType.Summon)
                                {
                                    (ATuserData as Actor).Destroy();
                                }
                            }
                            return;
                        }
#if !USE_SERVER
                        GameInstance.getInstance().animPather.CollectTrackBind(pFramework.shareParams.catchUserDataSet, null);
                        bool bHasPath = false;
                        foreach (var db in pFramework.shareParams.catchUserDataSet)
                        {
                            if (db is Actor)
                            {
                                Actor actor = db as Actor;
                                if (actor.GetActorType() == EActorType.Summon && actor.GetConfigID() == param.idValue)
                                {
                                    actor.Destroy();
                                    bHasPath = true;
                                }
                            }
                        }
                        pFramework.shareParams.catchUserDataSet.Clear();
                        if (bHasPath) return;
#endif
                        List<AWorldNode> vNodes = pFramework.world.CatchNodeList;
                        pFramework.world.CollectNodes(ref vNodes);
                        for (int i = 0; i < vNodes.Count; ++i)
                        {
                            if (vNodes[i].GetActorType() == EActorType.Summon && vNodes[i].GetConfigID() == param.idValue)
                            {
                                vNodes[i].SetDestroy();
                            }
                        }
                        vNodes.Clear();
                        return;
                    }
                case TriggerEventParameter.EDataType.ForceVictory:
                    {
                        BattleWorld battleWorld = BattleKits.GetBattle(m_pFramework);
                        if (battleWorld != null)
                        {
                            battleWorld.ForceVictory();
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.ForceDefeated:
                    {
                        BattleWorld battleWorld = BattleKits.GetBattle(m_pFramework);
                        if (battleWorld != null)
                        {
                            battleWorld.ForceDefeated();
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.ATEvent:
                    {
#if !USE_SERVER
                        if (param.idValue!=0)
                            Framework.Plugin.AT.AgentTreeManager.getInstance().ExecuteEvent((ushort)Base.EATEventType.UserDef, param.idValue );
                        else if(!string.IsNullOrEmpty(param.stringValue))
                            Framework.Plugin.AT.AgentTreeManager.getInstance().ExecuteEvent((ushort)Base.EATEventType.UserDef, param.stringValue);
#endif
                        return;
                    }
                case TriggerEventParameter.EDataType.AnimatorState:
                    {
#if !USE_SERVER
                        if (!string.IsNullOrEmpty(param.stringValue))
                        {
                            bool bOk = false;
                            Actor TriggerActor = GetTriggerActor();
                            if (TriggerActor!=null)
                            {
                                bOk = TriggerActor.StartActionByName(param.stringValue, 0, 1, true);
                            }
                            else
                            {
                                ObsElement pSceneNode = GetTriggerNode() as ObsElement;
                                if (pSceneNode != null)
                                {
                                    pSceneNode.PlayState(param.stringValue);
                                    bOk = true;
                                }
                            }
                            if(!bOk )
                            {
                                Animator pAnimator = null;
                                if (pInstnaceAble != null) pAnimator = pInstnaceAble.GetBehaviour<Animator>();
                                else if (pGameObject != null) pAnimator = pGameObject.GetComponent<Animator>();
                                if (pAnimator!=null)
                                {
//                                     if(ATuserData != null && pAnimator.runtimeAnimatorController && ATuserData is ObsElement)
//                                     {
//                                         ObsElement ele = ATuserData as ObsElement;
//                                         AnimationClip[] clips = pAnimator.runtimeAnimatorController.animationClips;
//                                         for (int i = 0; i < clips.Length; ++i)
//                                         {
//                                             if (param.stringValue.CompareTo(clips[i].name) == 0)
//                                             {
//                                                 ele.fKillDelayTime = clips[i].averageDuration;
//                                                 break;
//                                             }
//                                         }
//                                     }
                                        pAnimator.speed = 1;

                                    pAnimator.PlayInFixedTime(param.stringValue, 0, 0);
                                }
                            }
                        }
#endif
                        return;
                    }
                case TriggerEventParameter.EDataType.KillHeroSlot:
                    {
                        if(GetBattleWorld()!=null)
                        {
                            List<Actor> players = GetBattleWorld().GetPlayers();
                            if (players != null)
                            {
                                int index = param.idValue - 1;
                                if (index >= 0 && index < players.Count)
                                    players[index].SetKilled(true);
                            }
                        }

                        return;
                    }
                case TriggerEventParameter.EDataType.KillMySelf:
                    {
                        AWorldNode TriggerActor = GetTriggerNode();
                        if (TriggerActor!=null)
                        {
                            TriggerActor.SetKilled(true);
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.DestroyMySelf:
                    {
                        AWorldNode TriggerActor = GetTriggerNode();
                        if (TriggerActor != null)
                        {
                            TriggerActor.SetDestroy();
                        }
                        return;
                    }
                case TriggerEventParameter.EDataType.AutoElude:
                    {
                        //先去除
                        //                         Actor TriggerActor = GetTriggerActor();
                        //                         if (TriggerActor != null)
                        //                         {
                        //                             AutoEluder pEluder = AState.CastCurrentModeLogic<AutoEluder>(m_pFramework);
                        //                             if (pEluder != null)
                        //                             {
                        //                                 if (param.idValue != 0) pEluder.AddEluder(TriggerActor);
                        //                                 else pEluder.RemoveEluder(TriggerActor);
                        //                             }
                        //                         }
                        return;
                    }
                default:
                    break;
            }
            base.OnTrigger(param);
        }
        //------------------------------------------------------
        protected override void OnLevelChange(LevelChangeEventParameter param)
        {
        }
        //------------------------------------------------------
        protected override void OnSkillCD(SkillCDEventParameter param)
        {
            Actor TriggerActor = GetTriggerActor();
            if (TriggerActor == null) return;
            if(param.ctlType == SkillCDEventParameter.ECtlType.CdRate)
            {
                SkillInformation skill = TriggerActor.GetActorParameter().GetSkill();
                if (skill == null) return;
                List<Skill> vSkills = skill.GetLearned();
                if (vSkills == null) return;
                for(int i =0; i < vSkills.Count; ++i)
                {
                    if(vSkills[i].runingCD > 0 && 
                        ( (param.skillSets != null && param.skillSets.Contains(vSkills[i].nGuid)) || (param.skillTypes&(ushort)1<<vSkills[i].skillType)!=0 ) )
                    {
                        vSkills[i].runingCD += param.nCDRate * vSkills[i].cd* 0.01f;
#if UNITY_EDITOR
                        if (DebugConfig.bSkillDebug)
                        {
                            Framework.Plugin.Logger.Info("SkillCDEvent:" + TriggerActor.ToString());
                            Framework.Plugin.Logger.Info("SkillGroup:" + vSkills[i].nGuid);
                            Framework.Plugin.Logger.Info("CDRate:" + param.nCDRate * 0.01f);
                            Framework.Plugin.Logger.Info("config cd:" + vSkills[i].cd);
                            Framework.Plugin.Logger.Info("final cd:" + vSkills[i].runingCD);
                        }
#endif
                    }
                }
            }
        }
        //------------------------------------------------------
        protected override void OnTrackBind(TrackBindEventParameter param)
        {
#if !USE_SERVER
            if (param.valueID <= 0 || string.IsNullOrEmpty(param.trackName)) return;
            
            base.OnTrackBind(param);
#endif
        }
        //------------------------------------------------------
        protected override void OnVariable(VariableEventParameter param)
        {
            if (param.varType == VariableEventParameter.EVarType.None) return;
            AFramework pFramework = m_pFramework as AFramework;
            if (pFramework == null) return;

            switch (param.varType)
            {
                case VariableEventParameter.EVarType.RenderCurveToggle:
                    {
#if !USE_SERVER
                        Base.GlobalShaderController.EnableCurve(param.var0!=0);
#endif
                    }
                    return;
                case VariableEventParameter.EVarType.ReviveLatelyDieTeamActor:
                    {
                        if (param.var1 <= 0) return;
                        int attackGroup = this.nAttackGroup;
                        if (param.var0 >= 0) attackGroup = param.var0;
                        Framework.BattlePlus.BattleWorld battleWorld = m_pFramework.Get<Framework.BattlePlus.BattleWorld>();
                        if (battleWorld == null) return;

                        System.Collections.Generic.Stack<Framework.Data.ISvrActorData> vSvrDatas = battleWorld.GetDieStack((byte)attackGroup);
                        if (vSvrDatas != null && vSvrDatas.Count > 0)
                        {
                            Battle battle = GetState<Battle>();
                            if (battle == null) return;
                            Framework.Data.ISvrActorData svrData = vSvrDatas.Pop();
                            Actor pActor = battle.AddPlayer((byte)attackGroup, svrData, true);
                            if(pActor!=null)
                            {
                                pActor.GetActorParameter().GetRuntimeParam().hp = (int)(pActor.GetActorParameter().GetAttr(EAttrType.MaxHp) * param.var1 * CommonUtility.WAN_RATE);
                                pActor.StartActionByType(EActionStateType.Revive, 0, 1, true, false, true);
                            }
                        }
                        return;
                    }
                case VariableEventParameter.EVarType.UpdatePlayerLevel:
                    {
                        if(param.var0>0 && param.var1>0)
                        {
                            List<AWorldNode> vNodes = pFramework.world.CatchNodeList;
                            pFramework.world.CollectNodes(ref vNodes);
                            for (int i = 0; i < vNodes.Count; ++i)
                            {
                                if (vNodes[i].GetActorType() == EActorType.Player && vNodes[i].GetConfigID() == param.var0)
                                {
                                    Actor pActor = vNodes[i] as Actor;
                                    if(pActor!=null)
                                    {
                                        if (pActor.GetActorParameter().GetLevel() != param.var1)
                                        {
                                            pActor.GetActorParameter().SetLevel((ushort)param.var1);
                                            pActor.GetActorParameter().ResetRunTimeParameter(false);
                                        }
                                    }
                                }
                            }
                            vNodes.Clear();
                        }
                    }
                    return;
                case VariableEventParameter.EVarType.ReplaceElement:
                    {
                        if (param.var0 > 0 && param.var1 != 0)
                        {
                            uint sceneTheme = 0;
                            AbsMode pState = GetStateMode<AbsMode>();
                            if (pState != null) sceneTheme = pState.GetSceneTheme();
                            Data.CsvData_BattleObject.BattleObjectData obsData = Data.DataManager.getInstance().BattleObject.GetData(sceneTheme,(uint)param.var1);
                            if (obsData != null)
                            {
                                List<AWorldNode> vNodes = pFramework.shareParams.catchNodeList;
                                vNodes.Clear();
                                pFramework.world.CollectNodes<ObsElement>(ref vNodes);
                                float sqrtDist = param.var0 * 0.01f;
                                sqrtDist *= sqrtDist;
                                for (int i = 0; i < vNodes.Count; ++i)
                                {
                                    ObsElement obsEle = vNodes[i] as ObsElement;
                                    if (obsEle != null && obsEle.GetContextData() != obsData && obsEle.GetContextData() != null)
                                    {
                                        Data.CsvData_BattleObject.BattleObjectData battleObjData = obsEle.GetContextData() as Data.CsvData_BattleObject.BattleObjectData;
                                        if(battleObjData!=null && battleObjData.isReplace != 0 && (obsEle.GetPosition()- TriggerEventPos).sqrMagnitude<= sqrtDist)
                                        {
#if USE_SERVER

                                            obsEle.SetContextData(obsData);

#else
                                            InstanceOperiaon pCB = FileSystemUtil.SpawnInstance(obsData.Models_modelId_data.strFile, true);
                                            if (pCB != null)
                                            {
                                                pCB.OnCallback = obsEle.OnSpawnCallback;
                                                pCB.OnSign = obsEle.OnSpawnSign;
                                                pCB.bAsync = true;
                                                obsEle.SetContextData(obsData);
                                            }
                                            InstanceOperiaon pEffectCB = FileSystemUtil.SpawnInstance(obsData.ownEffect, true);
                                            if (pEffectCB != null)
                                            {
                                                pEffectCB.OnCallback = obsEle.OnOwnerSpawnEffect;
                                                pEffectCB.OnSign = obsEle.OnOwnerSign;
                                                pEffectCB.bAsync = true;
                                            }
#endif
                                        }
                                    }
                                }
                                vNodes.Clear();
                            }
                        }
                    }
                    return;
                case VariableEventParameter.EVarType.ReplaceMonster:
                    {
                        if (param.var0 > 0 && param.var1 != 0)
                        {
                            Data.CsvData_Monster.MonsterData monsterData = Data.DataManager.getInstance().Monster.GetData((uint)param.var1);
                            if (monsterData != null)
                            {
                                List<AWorldNode> vNodes = pFramework.shareParams.catchNodeList;
                                vNodes.Clear();
                                pFramework.world.CollectNodes<Monster>(ref vNodes);
                                float sqrtDist = param.var0 * 0.01f;
                                sqrtDist *= sqrtDist;
                                for (int i = 0; i < vNodes.Count; ++i)
                                {
                                    Monster obsEle = vNodes[i] as Monster;
                                    if (obsEle != null && obsEle.GetConfigID() != monsterData.id && (obsEle.GetPosition() - TriggerEventPos).sqrMagnitude <= sqrtDist)
                                    {
                                        Monster pActor = pFramework.world.CreateNode<Monster>(EActorType.Monster, monsterData);
                                        if (pActor != null)
                                        {
                                            pActor.SetPosition(obsEle.GetPosition());
                                            pActor.SetDirection(obsEle.GetDirection());
                                            pActor.SetScale(obsEle.GetScale());
                                            pActor.SetVisible(true);
                                            pActor.SetActived(obsEle.IsActived());
                                            pActor.SetSpatial(true);
                                            pActor.EnableLogic(true);
                                            pActor.EnableGravity(true);
                                            pActor.SetAttackGroup(obsEle.GetAttackGroup());
                                            pActor.GetActorParameter().SetLevel(obsEle.GetActorParameter().GetLevel());
                                            pActor.EnableSkill(true);
                                            ActorAgent pAgent = pActor.GetActorAgent();
                                            if (pActor.IsActived())
                                                pActor.StartActionByType(EActionStateType.Enter, 0, 1, true, false, true);
                                        }
                                        obsEle.SetDestroy();
                                    }
                                }
                                vNodes.Clear();
                            }
                        }
                    }
                    return;
                case VariableEventParameter.EVarType.ClearObstacles:
                    {
                        if (param.var0 > 0)
                        {
                            List<AWorldNode> vNodes = pFramework.world.CollectNodes<ObsElement>();
                            if(vNodes!=null)
                            {
                                float dist = param.var1 * 0.01f;
                                dist *= dist;
                                for (int i =0; i < vNodes.Count; ++i)
                                {
                                    ObsElement obs = vNodes[i] as ObsElement;
                                    if (obs == null) continue;
                                    if (((uint)param.var0 & ((1 << (int)obs.GetObstacleType()))) != 0 && (dist<=0 ||(vNodes[i].GetPosition() - TriggerEventPos).sqrMagnitude<= dist))
                                    {
                                        vNodes[i].SetDestroy();
                                    }
                                }
                            }
                        }
                    }
                    return;
                case VariableEventParameter.EVarType.HpConvertShield:
                    {
                        Actor pActor = GetTriggerActor();
                        if (pActor != null)
                        {
                            float rate = (float)param.var0 * Framework.Core.CommonUtility.WAN_RATE;
                            rate *= param.var1 * 0.01f;
                            int convertHp = (int)(pActor.GetActorParameter().GetRuntimeParam().hp * rate);
                            if (convertHp > 0)
                                pActor.GetActorParameter().AppendShieldExtern(convertHp);
                        }
                    }
                    return;
                case VariableEventParameter.EVarType.StartDungonTimeDown:
                    {
                        BattleStats pStats = BattleKits.GetBattleLogic<BattleStats>(m_pFramework);
                        if(pStats!=null)
                        {
                            pStats.StartGlobalTimeDown(param.var0, param.var1!=0);
                        }
                    }
                    return;
                case VariableEventParameter.EVarType.DelayShowBattleResult:
                    {
                    }
                    return;
                case VariableEventParameter.EVarType.PauseResumeGame:
                    {
                        if(param.var0!=0) m_pFramework.Pause();
                        else m_pFramework.Resume();
                    }
                    return;
                case VariableEventParameter.EVarType.NodeFreezed:
                    {
                        AWorldNode pNode = GetTriggerNode();
                        if (pNode == null) return;
                        if ((param.var1 & (int)PropertyEffectParameter.EApplayFlag.MySelf) != 0)
                        {
                            pNode.Freezed(param.var0!=0, (param.var2*0.001f));
                        }
                        if ((param.var1 & (int)PropertyEffectParameter.EApplayFlag.Friend) != 0 ||
                            (param.var1 & (int)PropertyEffectParameter.EApplayFlag.Enemy) != 0)
                        {
                            Actor pGroup0;
                            AWorldNode pRoot = pFramework.world.GetRootNode();
                            while (pRoot != null)
                            {
                                pGroup0 = pRoot as Actor;
                                pRoot = pRoot.GetNext();
                                if (pGroup0 == pNode) continue;
                                if (pGroup0 != null)
                                {
                                    if (((param.var1 & (int)PropertyEffectParameter.EApplayFlag.Friend) != 0 && pNode.GetAttackGroup() == pGroup0.GetAttackGroup()) ||
                                        ((param.var1 & (int)PropertyEffectParameter.EApplayFlag.Enemy) != 0 && pNode.GetAttackGroup() != pGroup0.GetAttackGroup()))
                                    {
                                        pGroup0.Freezed(param.var0 != 0,(param.var2 * 0.001f));
                                    }
                                }
                            }
                        }
                    }
                    return;
                case VariableEventParameter.EVarType.InBattleCameraID:
                    {
#if !USE_SERVER
                        BattleCameraLogic cameraLogic = AState.CastCurrentModeLogic<BattleCameraLogic>(m_pFramework);
                        if (cameraLogic != null)
                        {
                            if(Data.DataManager.getInstance().Scene.GetData((ushort)param.var1)!=null)
                            {
                                cameraLogic.SetOverrideCameraID((BattleCameraLogic.EStateType)param.var0, (ushort)param.var1);
                            }
                        }
#endif
                    }
                    return;
            }
            base.OnVariable(param);
        }
        //------------------------------------------------------
        protected override void OnActionStateProperty(ActionStatePropertyEventParameter param)
        {
            Actor TriggerActor = GetTriggerActor();
            if (TriggerActor != null)
            {
                base.OnActionStateProperty(param);
                return;
            }
        }
        //------------------------------------------------------
        protected override void OnObjScale(ObjScaleEventParameter param)
        {
            AWorldNode actor = GetTriggerNode();
            if (actor != null && GetFrameWorkMoudle()!=null)
            {
                GetFrameWorkMoudle().objScaler.Add(actor, param.curve, param.bKeep);
            }
        }
        //------------------------------------------------------
        protected override void OnSpawnSpline(SpawnSplineEventParameter param)
        {
            SpawnSplineManager.AddSpawnSpline(GetTriggerNode(), param.spawnData);
        }
        //------------------------------------------------------
        protected override void OnCameraEvent(CameraEventParameter param)
        {
#if !USE_SERVER
            BattleCameraLogic cameraLogic = AState.CastCurrentModeLogic<BattleCameraLogic>();
            if (cameraLogic == null) return;
            cameraLogic.ApplayEventCamera(param.level, param.lookOffset, param.transOffset, param.eulerAngle, param.distance, param.fov, param.fLerp);
#endif
        }

        //------------------------------------------------------
        public override void OnStopEvent(AWorldNode pTrigger, BaseEventParameter param)
        {
            base.OnStopEvent(pTrigger, param);
#if !USE_SERVER
            switch (param.GetEventType())
            {
                case EEventType.RenderLayer:
                    {
                        if (pTrigger!=null && GameInstance.getInstance().rootsLayerer != null && pTrigger.GetObjectAble() != null)
                        {
                            RenderLayerEventParameter layerParam = param as RenderLayerEventParameter;
                            if (layerParam != null && layerParam.clear)
                                GameInstance.getInstance().rootsLayerer.Clear(pTrigger.GetObjectAble().gameObject);
                        }
                    }
                    break;
            }
#endif
        }
    }
}

