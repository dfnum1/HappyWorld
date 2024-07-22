/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	TouchInput
作    者:	HappLI
描    述:	掉落系统
*********************************************************************/
using System;
using System.Collections.Generic;
using TopGame.Base;
using TopGame.Data;
using TopGame.Logic;
using Framework.Module;
using UnityEngine;
using Framework.Core;
using Framework.Base;
using ExternEngine;
using TopGame.SvrData;

namespace TopGame.Core
{

    [Framework.Plugin.AT.ATExportMono("掉落系统", "TopGame.GameInstance.getInstance().dropManager")]
    public class DropItemManager : Framework.Core.ADropItemManager
    {
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("添加掉落物")]
        public override void AddDrop(EActorType eType, int nId, uint nDropId, uint nCount, int effectID=-1)
        {
            if (nCount <= 0) return;
            CsvData_DropReward.DropRewardData dropData = DataManager.getInstance().DropReward.GetData(nDropId);
            if (dropData == null) return;
            if (effectID == -1)
            {
                effectID = (int)dropData.goldDropEffect;
            }
            AddDrop(eType, nId, nDropId, dropData, nCount, effectID);
        }
        //------------------------------------------------------
        protected override void OnAddDrop(ref DropData drop, int effectId)
        {
            if(drop.pData == null)
                drop.pData = DataManager.getInstance().DropReward.GetData(drop.nDropId);
#if !USE_SERVER
            if (effectId>=0)
                drop.pEffectData = DataManager.getInstance().GoldDropEffect.GetData((uint)effectId);
#endif
        }
        //------------------------------------------------------
        protected override bool OnDropItemWeightAdjust(DropData drop, ref List<ItemParam> vParams)
        {
            return false;
        }
        //------------------------------------------------------
        protected override void OnDropItemEffectParamFill(DropData drop, ItemParam itemParam, ref DropItem item)
        {
#if !USE_SERVER
            CsvData_GoldDropEffect.GoldDropEffectData effectData = null;
            if(drop.pEffectData!=null)
                effectData = drop.pEffectData as CsvData_GoldDropEffect.GoldDropEffectData;
            if(effectData == null && drop.pEffectData!=null)
            {
                effectData = drop.pEffectData as CsvData_GoldDropEffect.GoldDropEffectData;
            }
            item.nBounceCount = UnityEngine.Random.Range(2, 5);
            item.vSpeed.x = UnityEngine.Random.Range(-2, 2);
            item.vSpeed.z = UnityEngine.Random.Range(-2, 2);
            item.vSpeed.y = UnityEngine.Random.Range(2, 10);
            item.pickScale = 1;

            if (effectData != null)
            {
                item.pickScale = effectData.pickScale;
                item.pickSlot = effectData.pickSlot;
                item.fPickFinishDuration = effectData.pickDuration;
                if (effectData.randomRadius!=null && effectData.randomRadius.Length ==2)
                {
                    Vector3 vPos = item.GetPosition();
                    vPos.x += UnityEngine.Random.Range(effectData.randomRadius[0], effectData.randomRadius[1]);
                    vPos.z += UnityEngine.Random.Range(effectData.randomRadius[0], effectData.randomRadius[1]);
                    item.SetPosition(vPos);
                }

                if(effectData.dropSpeedX!=null && effectData.dropSpeedX.Length ==2) item.vSpeed.x = m_pFramework.GetRamdom(effectData.dropSpeedX[0], effectData.dropSpeedX[1]);
                if (effectData.dropSpeedZ != null && effectData.dropSpeedZ.Length == 2) item.vSpeed.z = m_pFramework.GetRamdom(effectData.dropSpeedZ[0], effectData.dropSpeedZ[1]);
                if (effectData.dropSpeedY != null && effectData.dropSpeedY.Length == 2) item.vSpeed.y = m_pFramework.GetRamdom(effectData.dropSpeedY[0], effectData.dropSpeedY[1]);
                if(effectData.IdleTime!=null && effectData.IdleTime.Length == 2) item.dropIdle = UnityEngine.Random.Range(effectData.IdleTime[0], effectData.IdleTime[1]);
                if (effectData.bounceCount != null && effectData.bounceCount.Length == 2) item.nBounceCount = UnityEngine.Random.Range(effectData.bounceCount[0], effectData.bounceCount[1]);
            }
            else
            {
                item.dropIdle = 0;
            }
            if (item.pData != null && item.pData is CsvData_DropReward.DropRewardData)
                item.pickType = (EPickType)(item.pData as CsvData_DropReward.DropRewardData).pickType;
            else
                item.pickType = EPickType.FlyUI;
#endif
        }
        //-------------------------------------------------
        protected override bool OnDraopFlyUIEffect(DropItem data)
        {
#if !USE_SERVER
            DropItem drop = data as DropItem;
            ATweenEffecter tween = drop.GetWorldTween();
            if (!drop.bIsFlyingToUI)
            {
                if (tween != null)
                {
                    //飞向UI
                    if (drop.tweenFlyUIPos.sqrMagnitude > 0)
                    {
                        Vector3 pos = drop.tweenFlyUIPos;
                        if (pos.sqrMagnitude > 0)
                        {
                            if (tween is WorldUITweenEffecter)
                            {
                                WorldUITweenEffecter worldEffect = tween as WorldUITweenEffecter;
                                worldEffect.RayDistance = drop.RayDistance;
                            }
                            drop.GetWorldTween().Param = drop;
                            drop.GetWorldTween().OnTweenFinishCB = OnDropFlyUIFinish;
                            drop.GetWorldTween().Play(drop.GetPosition(), pos);
                            drop.bIsFlyingToUI = true;
                        }
                        else
                        {
                            drop.bIsReachedUI = true;
                        }
                    }
                    else
                        drop.bIsReachedUI = true;
                }
                else
                {
                    drop.bIsReachedUI = true;
                }
            }
            if (tween != null && tween.IsPlaying())
            {
                return false;
            }
            return true;
#else
            return false;
#endif
        }
#if !USE_SERVER
        //-------------------------------------------------
        void OnDropFlyUIFinish(VariablePoolAble param)
        {
            DropItem item = (DropItem)param;
            item.bIsFlyingToUI = false;
            item.bIsReachedUI = true;
            OnFlyToUIFinishCB?.Invoke();
        }
#endif
    }
}