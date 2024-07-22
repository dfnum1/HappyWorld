/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GameSystem
作    者:	HappLI
描    述:	
*********************************************************************/
using System;
using TopGame.Base;
using UnityEngine;
using Framework.Module;
using System.Collections.Generic;
using Framework.Core;
using Framework.Base;
using Framework.Plugin.AT;
using Framework.BattlePlus;
using ExternEngine;

namespace TopGame.Core
{
    public abstract class GameModule : GameFramework
    {
        protected AnimPathManager m_pAnimPathMgr = null;

        protected bool m_bEnableSliderTouch = true;

        protected Core.SceneMgr m_SceneManager =null;
        public SceneMgr sceneManager { get { return m_SceneManager; } }

        //------------------------------------------------------
        public virtual AnimPathManager animPather
        {
            get { return m_pAnimPathMgr; }
        }

        protected RootsLayerer m_pRootsLayerer = null;
        public virtual RootsLayerer rootsLayerer
        {
            get { return m_pRootsLayerer; }
        }
        //------------------------------------------------------
        protected override bool OnAwake(Framework.IFrameworkCore plusSetting)
        {
            base.OnAwake(plusSetting);

            if(m_pDataMgr == null)
                m_pDataMgr = RegisterModule<Data.DataManager>();
            
            if (m_pFileSystem == null)
                m_pFileSystem = RegisterModule<FileSystem>();

            m_URPWorker = RegisterModule<URP.URPPostWorker>();

            m_pDropManager = RegisterModule<DropItemManager>();
            m_pDropManager.Register(this);
            m_pWorld.RegisterCallback(m_pDropManager);

            if (m_pEventTrigger == null)
                m_pEventTrigger = RegisterModule<EventSystemTrigger>();

            m_pAnimPathMgr = RegisterModule<AnimPathManager>();
            m_pAnimPathMgr.RegisterCallback(this);

            m_SceneManager = RegisterModule<Core.SceneMgr>();
            m_SceneMgr = m_SceneManager;
            m_SceneMgr.Register(this);

            m_pRootsLayerer = RegisterModule<RootsLayerer>();

            m_pBattleWorld = RegisterModule<Framework.BattlePlus.BattleWorld>();

            TargetFrameRate = 30;

            return true;
        }
        //------------------------------------------------------
        protected override void OnStart(Framework.IFrameworkCore plusSetting)
        {
            //! set frameworkplus configData
            Data.CsvData_SystemConfig.SystemConfigData sysConfig = Data.CsvData_SystemConfig.sysConfig;
            if(sysConfig!=null)
            {
                //ConfigUtil.ConversionRatio = sysConfig.ConversionRatio;
                //ConfigUtil.runSkillSections = sysConfig.runSkillSection;
                //ConfigUtil.globalCommandSkillCD = sysConfig.globalCommandSkillCD;
                //ConfigUtil.globalTeamSoulFlagDefer = (int)sysConfig.teamSoulFlagDefer;
                //ConfigUtil.ConstMinHurt = sysConfig.ConstMinHurt;
                //ConfigUtil.ConstHurt1 = sysConfig.ConstHurt1;
                //ConfigUtil.ConstHurt2 = sysConfig.ConstHurt2;
                //ConfigUtil.ConstDef1 = sysConfig.ConstDef1;
                //ConfigUtil.ConstDef2 = sysConfig.ConstDef2;
            }
            ConfigUtil.timeHorizon = Data.GlobalSetting.fRVOTimeHorizon;
            ConfigUtil.timeHorizonObst = Data.GlobalSetting.fRVOTimeHorizonObst;
            ConfigUtil.bitObstacleIgnoreFilter =  CollisionFilters.GetObstacleFilters();
            ConfigUtil.magentFlilter = CollisionFilters.GetMagentFilter();
            ConfigUtil.bProfilerDebug = DebugConfig.bProfilerTick;
       //     ConfigUtil.DefaultBattleRegionWidth = Data.GlobalSetting.DefaultBattleRegionWidth;
            ConfigUtil.lowerBattleSpeed = Data.GlobalSetting.fLowerBattleSpeed;

            ConfigUtil.ImprisonLinkEffect = Data.PermanentAssetsUtil.GetAssetPath( Data.EPathAssetType.ImprisonLinkEffect);
            ConfigUtil.LinkEffect = Data.PermanentAssetsUtil.GetAssetPath(Data.EPathAssetType.BuffLinkEffect);
            ConfigUtil.PartAimPoint = Data.PermanentAssetsUtil.GetAssetPath(Data.EPathAssetType.PartAimPoint);
            ConfigUtil.bDamageDebug = DebugConfig.bDamageDebug;
            ConfigUtil.bSkillDebug = DebugConfig.bSkillDebug;
            ConfigUtil.bShowSpatial = DebugConfig.bShowSpatial;
            ConfigUtil.bShowNodeDebugFrame = DebugConfig.bShowNodeDebugFrame;
            ConfigUtil.bEventTriggerDebug = DebugConfig.eventDebug;
            ConfigUtil.bDamageDebug = DebugConfig.bDamageDebug;
#if UNITY_EDITOR
            ConfigUtil.bBattleFameWriteLogFile = DebugConfig.bBatlleReport;
#else
            ConfigUtil.bBattleFameWriteLogFile = false;
#endif
            ConfigUtil.OnBuffEffectFunc = Data.PermanentAssetsUtil.GetBuffEffectMaterial;
            
            Data.GlobalDefaultResources.UIGrayMat = Data.PermanentAssetsUtil.GetAsset<Material>(Data.EPermanentAssetType.GrayMat);
            base.OnStart(plusSetting);
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            if (m_pAnimPathMgr != null)
                m_pAnimPathMgr.Destroy();
            m_pAnimPathMgr = null;

            if (m_SceneMgr != null)
                m_SceneMgr.Destroy();
            m_SceneMgr = null;

            if (m_pRootsLayerer != null) m_pRootsLayerer.Destroy();
            m_pFileSystem = null;

            m_SceneManager = null;
            base.OnDestroy();
        }
        //------------------------------------------------------
        public override void SetPlayerPosition(FVector3 pos, Actor pActor = null)
        {
            if (m_PlayerPosition == pos)
                return;
            base.SetPlayerPosition(pos, pActor);
            if (Base.GlobalShaderController.Instance)
                Base.GlobalShaderController.Instance.SetCurvePivotPointX(m_PlayerPosition.z);
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrameTime)
        {
            base.InnerUpdate(fFrameTime);
            if (m_pAnimPathMgr != null) m_pAnimPathMgr.Update(fFrameTime);
        }
        //------------------------------------------------------
        public override void OnParticleStop(AInstanceAble pObject)
        {
            if (m_pProjectileManager == null) return;
            System.Collections.Generic.Dictionary<int, AProjectile> vMap = m_pProjectileManager.GetRunningProjectile();
            foreach (var db in vMap)
            {
                if (db.Value.GetObjectAble() == pObject)
                {
                    db.Value.remain_life_time = 0;
                }
            }
        }
        //------------------------------------------------------
        public override void OnTimelineStop(AInstanceAble playable)
        {
            if (m_pAnimPathMgr != null)
            {
                m_pAnimPathMgr.OnTimelineStop(playable);
            }
            System.Collections.Generic.Dictionary<int, AProjectile> vMap = m_pProjectileManager.GetRunningProjectile();
            foreach (var db in vMap)
            {
                if (db.Value.GetObjectAble() == playable)
                {
                    db.Value.remain_life_time = 0;
                }
            }
        }
        //------------------------------------------------------
        public override void OnLaunchProjectile(AProjectile pProjectile)
        {
        }
        //------------------------------------------------------
        public override void OnProjectileHit(AProjectile pProjectile, ActorAttackData attackData, bool bHitScene, bool bExplode)
        {
            if (bHitScene)
                pProjectile.remain_hit_count = 0;

            string strEffect = pProjectile.hit_target_effect;
            if(bExplode)
            {
                strEffect = pProjectile.projectile.explode_effect;
            }
            if(!string.IsNullOrEmpty(strEffect))
            {
                InstanceOperiaon pOp = FileSystemUtil.SpawnInstance(strEffect, true);
                if(pOp != null)
                {
                    pOp.SetByParent(RootsHandler.ParticlesRoot);
                    pOp.SetUserData(0, new Variable3()
                    {
                        floatVal0 = attackData.hit_position.x,
                        floatVal1 = attackData.hit_position.y,
                        floatVal2 = attackData.hit_position.z
                    });
                    if ((pProjectile.projectile.target_hit_flag & (byte)(ETargetHitFlag.HitEffectUseDirection)) != 0)
                        pOp.SetUserData(1, new Variable3() { floatVal0 = pProjectile.direction.x, floatVal1 = pProjectile.direction.y, floatVal2 = pProjectile.direction.z });
                    else
                        pOp.SetUserData(1, null);
                    pOp.SetUserData(2, new Variable3() { floatVal0 = pProjectile.projectile.target_effect_hit_scale, floatVal1 = pProjectile.projectile.target_effect_hit_scale, floatVal2 = pProjectile.projectile.target_effect_hit_scale });
                    pOp.SetUserData(3, new Variable1() { floatVal = 5 });
                    pOp.OnCallback =ProjectileHitUtil.OnProjectHitCallback;
                }
            }

            if(pProjectile.hit_target_sound_id>0)
            {
                AudioManager.PlayID(pProjectile.hit_target_sound_id, pProjectile.position);
            }
            else if(!string.IsNullOrEmpty(pProjectile.hit_target_sound))
            {
                AudioManager.PlayEffect(pProjectile.hit_target_sound);
            }
        }
        //------------------------------------------------------
        public override void OnProjectileUpdate(AProjectile pProjectile)
        {
            if (pProjectile.IsObjected())
            {
                pProjectile.SetPosition(pProjectile.position);
                uint launch_flag = pProjectile.projectile.launch_flag;
                if (pProjectile.offsetEulerAngle.sqrMagnitude>0 || pProjectile.selfRotate.sqrMagnitude>0)
                {
                    Vector3 eulerAngles = Framework.Core.CommonUtility.DirectionToEulersAngle(pProjectile.direction) + pProjectile.offsetEulerAngle;
                    if (pProjectile.projectile.life_time > 0)
                    {
                        eulerAngles += pProjectile.selfRotate* ExternEngine.FMath.Clamp01(FFloat.one-pProjectile.remain_life_time/pProjectile.projectile.life_time);
                    }
                    Vector3 finalEuler = Vector3.zero;
                    if ((launch_flag & (int)ELaunchFlag.DirX) != 0) finalEuler.x = eulerAngles.x;
                    if ((launch_flag & (int)ELaunchFlag.DirY) != 0) finalEuler.y = eulerAngles.y;
                    if ((launch_flag & (int)ELaunchFlag.DirZ) != 0) finalEuler.z = eulerAngles.z;
                    pProjectile.SetEulerAngle(finalEuler);
                }
                else
                {
                    if((launch_flag&(int)ELaunchFlag.AllDir) != 0 )
                        pProjectile.SetDirection(pProjectile.direction);
                    else
                    {
                        Vector3 eulerAngles = Framework.Core.CommonUtility.DirectionToEulersAngle(pProjectile.direction);
                        Vector3 finalEuler = Vector3.zero;
                        if (pProjectile.IsLaunchFlaged(ELaunchFlag.DirX)) finalEuler.x = eulerAngles.x;
                        if (pProjectile.IsLaunchFlaged(ELaunchFlag.DirY)) finalEuler.y = eulerAngles.y;
                        if (pProjectile.IsLaunchFlaged(ELaunchFlag.DirZ)) finalEuler.z = eulerAngles.z;
                        pProjectile.SetEulerAngle(finalEuler);
                    }
                }

                if(pProjectile.projectile.life_time>0 && pProjectile.GetObjectAble() is ParticleController)
                {
                    ParticleController parCtl = pProjectile.GetObjectAble() as ParticleController;
                    parCtl.SetPlayLifeTimeScale(ExternEngine.FMath.Clamp01(FFloat.one - pProjectile.remain_life_time / pProjectile.projectile.life_time), pProjectile.GetScale());
                }

                if(pProjectile.launch_effect_speed > 0)
                {
                    if (pProjectile.GetObjectAble() is SpeedLifeParticleController)
                    {
                        SpeedLifeParticleController speedLife = pProjectile.GetObjectAble() as SpeedLifeParticleController;
                        speedLife.SetLaunchSpeed(pProjectile.speed.magnitude* pProjectile.launch_effect_speed);
                    }
                }

                if(ProjectileData.IsTrack(pProjectile.projectile.type) && pProjectile.owner_actor!=null && pProjectile.pTargetNode!=null)
                {
                    ChainLightning chainLightning = pProjectile.GetObjectAble() as ChainLightning;
                    if (chainLightning)
                    {
                        Vector3 offset_slot = Vector3.zero;

                        //! start
                        if (pProjectile.bound_start_actor != null && !pProjectile.bound_start_actor.IsFlag(EWorldNodeFlag.Killed))
                        {
                            Transform tranSlot = pProjectile.bound_start_actor.FindBindSlot(pProjectile.owner_bind_slot);
                            if (tranSlot != null) chainLightning.SetStart(tranSlot.position + offset_slot, null);
                            else chainLightning.SetStart(pProjectile.bound_start_actor.GetPosition(), null);
                        }
                        else if(pProjectile.is_bound_projectile)
                        {
                            chainLightning.SetStart(pProjectile.init_position, null);
                        }
                        else
                        {
                            Transform tranSlot = pProjectile.owner_actor.FindBindSlot(pProjectile.owner_bind_slot);
                            if (tranSlot != null) chainLightning.SetStart(tranSlot.position + offset_slot, null);
                            else chainLightning.SetStart(pProjectile.owner_actor.GetPosition(), null);
                        }

                        //! end
                        chainLightning.SetEnd(pProjectile.targetPosition, null);
                    }
                }

             }
        }
        //------------------------------------------------------
        public override void OnGameQuality(int quality, Framework.Data.AConfig config)
        {
            if (config == null || !(config is TopGame.Data.QualityConfig) ) return;
            FileSystem fileSystem = m_pFileSystem as FileSystem;
            TopGame.Data.QualityConfig qualityCfg = (TopGame.Data.QualityConfig)config;
            ProjectorLightReciver.fProjectorLightCheckGap = qualityCfg.ProjectorLightCheckGap;
            if (fileSystem!=null) fileSystem.SetCapability(qualityCfg.OneFrameCost, qualityCfg.MaxInstanceCount, qualityCfg.DestroyDelayTime);
            if (m_pLodMgr != null)
                m_pLodMgr.ForceLowerLOD(qualityCfg.bForceLOWLOD);

            ICameraController cameraCtl = m_pCameraController as ICameraController;
            if (cameraCtl != null)
            {
                cameraCtl.SetPostProcess(qualityCfg.postProcess);
                cameraCtl.SetURPAsset(qualityCfg.urpAsset);
            }
            URP.URPPostWorker.SetPassFlags(qualityCfg.nURPPassFlags);
            m_pFileSystem.SetSreamReadBufferSize(qualityCfg.ReadAbStreamBuffSize);

            TargetFrameRate = qualityCfg.TargetFrameRate;
        }
        //------------------------------------------------------
        public override void OnAnimPathBegin(IPlayableBase playAble)
        {
        }
        //------------------------------------------------------
        public override void OnAnimPathEnd(IPlayableBase playAble)
        {
        }
        //------------------------------------------------------
        public override void OnAnimPathUpdate(IPlayableBase playAble)
        {
        }
        //------------------------------------------------------
        protected override void OnInnerApplicationFocus(bool bFocus)
        {
            TargetFrameRate = Data.GameQuality.targetFrameRate;
            if(bFocus)
                Data.GameQuality.OnAppResume();
            
            TopGame.Core.AUserActionManager.AddActionKV("switch_type", bFocus?2:1);
            AddEventUtil.LogEvent("switch_game",true,true);
        }
        //------------------------------------------------------
        public override BufferState OnCreateBuffer(Actor pActor, AWorldNode trigger, BufferState buff, uint dwBufferID, uint dwLevel, IContextData pData, IBuffParam param = null)
        {
            return Logic.BuffHelper.CreateBuffer(this, pActor, trigger, buff, dwBufferID, dwLevel, pData, param);
        }
        //------------------------------------------------------
        public override void OnSceneCallback( SceneParam sceParam)
        {

        }
        //------------------------------------------------------
        public override long GetReaminMemory()
        {
            return JniPlugin.GetRemainMemory();
        }
        //------------------------------------------------------
        public virtual void OnSessionState(Net.AServerSession session, Net.ESessionState state) { }
        //------------------------------------------------------
        public virtual void OnReConnect(Framework.Plugin.AT.IUserData takeData = null) { }
        //------------------------------------------------------
        public virtual void OnCheckNetPackage(int msgCode, bool bRevice, Framework.Plugin.AT.IUserData pParam = null) { }
        //------------------------------------------------------
        public virtual void OnNetWorkHeart(float intervalTime, Net.AServerSession pSession, int lastSendCode, Google.Protobuf.IMessage lastSendMessage) { }
        //------------------------------------------------------
#region 无限跑酷滑动开关
        public bool GetEnableSlider()
        {
            return m_bEnableSliderTouch;
        }
        //------------------------------------------------------
        public void SetEnableSlider(bool enable)
        {
            m_bEnableSliderTouch = enable;
        }
#endregion
    }
}

