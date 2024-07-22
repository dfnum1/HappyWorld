/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	Projectile
作    者:	HappLI
描    述:	飞行道具
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
using ExternEngine;

namespace TopGame.Core
{
    //------------------------------------------------------
    public class Projectile : AProjectile
    {
#if !USE_SERVER
        public AInstanceAble particle_waring_data = null;

        public InstanceOperiaon pCallback = null;
        public InstanceOperiaon pWaringCallback = null;
#endif
        //------------------------------------------------------
        public Projectile(AFrameworkModule pGame) : base(pGame) { }
#if !USE_SERVER
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            if (particle_waring_data != null) particle_waring_data.RecyleDestroy(2);
            particle_waring_data = null;
            base.OnDestroy();
        }
        //------------------------------------------------------
        public override void Reset()
        {
            if (pCallback != null) pCallback.Earse();
            pCallback = null;

            if (pWaringCallback != null) pWaringCallback.Earse();
            pWaringCallback = null;

            base.Reset();
        }
        //------------------------------------------------------
        protected override void OnInnerSpawnObject()
        {
            if (m_pObjectAble != null)
            {
                if (launch_effect_speed > 0)
                {
                    if (m_pObjectAble is SpeedLifeParticleController)
                    {
                        SpeedLifeParticleController speedLife = m_pObjectAble as SpeedLifeParticleController;
                        speedLife.SetLaunchSpeed(launch_effect_speed);
                        speedLife.SetLifeTime(projectile.life_time + delta);
                    }
                    else if (m_pObjectAble is ParticleController)
                    {
                        if (launch_effect_speed > 0)
                            (m_pObjectAble as ParticleController).SetSpeed(launch_effect_speed);
                    }
                }
                else
                {
                    if (m_pObjectAble is SpeedLifeParticleController)
                    {
                        SpeedLifeParticleController speedLife = m_pObjectAble as SpeedLifeParticleController;
                        speedLife.SetLaunchSpeed(launch_effect_speed);
                        speedLife.SetLifeTime(projectile.life_time + delta);
                    }
                }
            }

            if (pCallback != null) pCallback.Earse();
            pCallback = null;
            base.OnInnerSpawnObject();
        }
        //------------------------------------------------------
        public void OnWaringSign(InstanceOperiaon pCb)
        {
            pCb.SetUsed(!IsFlag(EWorldNodeFlag.Killed) && !IsFlag(EWorldNodeFlag.Destroy) && remain_life_time > 0 && this.waring_duration > 0);
        }
        //------------------------------------------------------
        public void OnSpawnWaringCallback(InstanceOperiaon pCb)
        {
            particle_waring_data = pCb.GetAble();
            if (particle_waring_data == null) return;
            particle_waring_data.SetPosition(final_drop_pos);
            particle_waring_data.SetEulerAngle(Vector3.zero);
            if (m_nRenderlayer > 0 && particle_waring_data != null) particle_waring_data.SetRenderLayer(m_nRenderlayer);

            if (pWaringCallback != null) pWaringCallback.Earse();
            pWaringCallback = null;
        }
        //------------------------------------------------------
        protected override void InnerUpdate(FFloat fFrameTime)
        {
            if (waring_duration > 0)
            {
                waring_duration -= fFrameTime;
                if (waring_duration <= 0)
                {
                    if (particle_waring_data != null)
                    {
                        particle_waring_data.RecyleDestroy(2);
                        particle_waring_data = null;
                    }
                }
                else
                {
                    if (particle_waring_data != null)
                    {
                        FVector3 drop = init_position;
                        if (has_speed)
                        {
                            FVector3 externSpeed = FVector3.zero;
                            if (delta > 0 || projectile.externLogicSpeed)
                                externSpeed += GetGameModule().GetCurrentRunSpeed();
                            if (delta <= 0)
                                externSpeed += extern_speed;
                            final_drop_pos.x += externSpeed.x * fFrameTime;
                            final_drop_pos.z += externSpeed.z * fFrameTime;
                        }
                        drop += final_drop_pos;
                        Core.TerrainLayers.AdjustTerrainHeight(GetGameModule(), ref drop, GetUp(), ETerrainHeightType.TerrainY);
                        particle_waring_data.SetPosition(drop);
                    }
                }
            }
            base.InnerUpdate(fFrameTime);
        }
        //------------------------------------------------------
        public override void SetRenderLayer(int layer)
        {
            base.SetRenderLayer(layer);
            if (particle_waring_data != null)
            {
                if (layer == -1) particle_waring_data.ResetLayer();
                else particle_waring_data.SetRenderLayer(layer);
            }
        }
#endif
    }
}

