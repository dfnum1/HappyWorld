/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	DynamicHudHpManager
作    者:	HappLI
描    述:	对象头顶血条管理模块
*********************************************************************/

using System;
using TopGame.Core;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TopGame.Data;
using Framework.Core;
using Framework.Logic;
using Framework.Base;
using TopGame.Base;
using Framework.BattlePlus;
using TopGame.Logic;
using TopGame.SvrData;

namespace TopGame.UI
{
    public class DynamicHudHpManager
    {
        private readonly int POOL_MAX = 4;

        Dictionary<int, HudBar> m_vData;
        Dictionary<int, BufferState> m_vLoadingBuffer;

        List<int> m_vDestroying = null;
        Dictionary<byte, Queue<HudBar>> m_vRecycle = null;
        ObjectPool<HudBar> m_vPools = null;

        private bool m_bEnable = true;
        private uint m_nHudEnableFlags = 0xffffffff;
        private uint m_nHudEnableAttackGroupFlags = 0xffffffff;
        public DynamicHudHpManager()
        {
            m_vPools = new ObjectPool<HudBar>(16);
            m_vData = new Dictionary<int, HudBar>(16);
            m_vRecycle = new Dictionary<byte, Queue<HudBar>>(2);
            m_vDestroying = new List<int>(16);
            m_vLoadingBuffer = new Dictionary<int, BufferState>(16);
        }
        //------------------------------------------------------
        public void Enable(bool bEnable)
        {
            m_bEnable = bEnable;
            if (bEnable)
            {
                m_nHudEnableFlags = 0xffffffff;
                m_nHudEnableAttackGroupFlags = 0xffffffff;
            }
            if (!m_bEnable)
            {
                Destroy();
            }
        }
        //------------------------------------------------------
        public void EnableAttackGroupHud(byte attackGroup, bool bEnable)
        {
            if (bEnable) m_nHudEnableAttackGroupFlags |= (uint)((1 << (int)attackGroup));
            else m_nHudEnableAttackGroupFlags &= ~(uint)((1 << (int)attackGroup));
        }
        //------------------------------------------------------
        public bool IsEnableAttackGroupHud(byte attackGroup)
        {
            if (!m_bEnable) return false;
            return (m_nHudEnableAttackGroupFlags & (uint)((1 << (int)attackGroup))) != 0;
        }
        //------------------------------------------------------
        public void EnableHud(EActorType actorType, bool bEnable)
        {
            if (bEnable) m_nHudEnableFlags |= (uint)((1 << (int)actorType));
            else m_nHudEnableFlags &= ~(uint)((1 << (int)actorType));
        }
        //------------------------------------------------------
        public bool IsEnableHud(EActorType actorType)
        {
            if (actorType == EActorType.None || actorType == EActorType.Wingman) return false;
            if (!m_bEnable) return false;
            return (m_nHudEnableFlags & (uint)((1 << (int)actorType))) != 0;
        }
        //------------------------------------------------------
        public void OnBeginBuff(Actor pActor, BufferState buff)
        {
            HudBar hud;
            if (m_vData.TryGetValue(pActor.GetInstanceID(), out hud))
            {
                if (hud != null)
                {
                    hud.OnBeginBuff(pActor, buff);
                }
                else
                {
                    m_vLoadingBuffer[pActor.GetInstanceID()] = buff;
                }
            }
            else
            {
                m_vLoadingBuffer[pActor.GetInstanceID()] = buff;
                SpawnHeroHudHp(pActor);
            }
                
        }
        //------------------------------------------------------
        public void OnEndBuff(Actor pActor, BufferState buff)
        {
            HudBar hud;
            if (m_vData.TryGetValue(pActor.GetInstanceID(), out hud))
            {
                if (hud != null)
                {
                    hud.OnEndBuff(pActor, buff);
                }
            }
        }
        //------------------------------------------------------
        public void OnActorAttrChange(Actor pActor, EAttrType type, float fValue, bool bPlus, VariablePoolAble externVarial = null)
        {
            if (!IsEnableHud(pActor.GetActorType()) || !IsEnableAttackGroupHud(pActor.GetAttackGroup())) return;
            //if (!m_bEnable) return;
            if (externVarial != null && externVarial is BattleDamage)
            {
                if (!((BattleDamage)externVarial).CanPost(EDamagePostBit.PopHud))
                    return;
            }
            //Logic.Monster monster = pActor as Logic.Monster;
            //if(monster!=null && monster.GetMonsterType() == EMonsterType.Boss)
            //{
            //    return;
            //}

            if (Mathf.Abs(fValue) <= 0.01f) return;
            if (IsRefreshHudBar(type))
            {
                HudBar hud;
                if (m_vData.TryGetValue(pActor.GetInstanceID(), out hud))
                {
                    if (hud!=null)
                    {
                        if (bPlus)
                        {
                            hud.Show(-fValue);
                        }
                        else
                        {
                            hud.Show(fValue);
                        }
                    }
                    return;
                }

                //默认敌方普通怪物血条
                EPermanentAssetType assetType = EPermanentAssetType.HudHp;
                //判断玩家血条
                if (pActor.GetActorType() == EActorType.Player && pActor.GetAttackGroup() == 0)
                    assetType = EPermanentAssetType.HeroHudHp;
                //判断精英怪物血条
                //if (monster != null && monster.GetMonsterType() == EMonsterType.Elite)
                //{
                //    assetType = EPermanentAssetType.EliteHudHp;
                //}
                //判断召唤物血条
                var actorType = pActor.GetActorType();
                if (actorType == EActorType.Summon && pActor.GetAttackGroup() == 0)
                {
                    assetType = EPermanentAssetType.SummonHudHp;
                }
                else if (actorType == EActorType.Summon && pActor.GetAttackGroup() != 0)
                {
                    assetType = EPermanentAssetType.EnemySummonHudHp;
                }

                Queue<HudBar> vPools;
                if (m_vRecycle.TryGetValue((byte)assetType, out vPools) && vPools.Count>0)
                {
                    hud = vPools.Dequeue();
                    hud.SetHudType((byte)assetType);
                    hud.SetActor(pActor);
                    if (bPlus)
                    {
                        hud.Show(-fValue);
                    }
                    else
                    {
                        hud.Show(fValue);
                    }
                    if (m_vLoadingBuffer.TryGetValue(hud.GetInstanceID(), out BufferState bufferState))
                        hud.OnBeginBuff(hud.GetActor(), bufferState);

                    m_vData.Add(pActor.GetInstanceID(), hud);
                }
                else
                {
                    GameObject pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(assetType);
                    if (pAsset == null) return;

                    if (pActor == null) return;
                    HudBar hudBar;
                    if (!m_vData.TryGetValue(pActor.GetInstanceID(), out hudBar))
                    {
                        hudBar = MallocHudBar();
                        m_vData.Add(pActor.GetInstanceID(), hudBar);
                    }
                    if (hudBar.pInstanceOperiaon != null)
                        hudBar.pInstanceOperiaon.Earse();

                    hudBar.pInstanceOperiaon = FileSystemUtil.SpawnInstance(pAsset);
                    if (hudBar.pInstanceOperiaon == null) return;
                    hudBar.SetHudType((byte)assetType);
                    hudBar.SetActor(pActor);
                    hudBar.pInstanceOperiaon.userData0 = hudBar;
                    hudBar.pInstanceOperiaon.OnSign = OnCallSign;
                    hudBar.pInstanceOperiaon.OnCallback = OnSpawnCallback;
                }

            }
        }
        //------------------------------------------------------
        bool IsRefreshHudBar(EAttrType type)
        {
            //血量,暴击类型,护盾,都刷新血条UI
            return type == EAttrType.MaxHp || type == EAttrType.Crit || (int)type == (int)EBuffAttrType.ShieldDuration;
        }
        //------------------------------------------------------
        public void SpawnHeroHudHp(Actor pActor)
        {
            OnActorAttrChange(pActor, EAttrType.MaxHp, 1, true);
        }
        //------------------------------------------------------
        public void Preload(int cnt)
        {
            GameObject pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.HudHp);
            if (pAsset)
            {
                for(int i = 0; i < cnt; ++i)
                    FileSystemUtil.PreSpawnInstance(pAsset);
            }
            pAsset = Data.PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.HeroHudHp);
            if (pAsset)
            {
                for (int i = 0; i < cnt; ++i)
                    FileSystemUtil.PreSpawnInstance(pAsset);
            }
        }
        //------------------------------------------------------
        void OnCallSign(InstanceOperiaon pCb)
        {
            pCb.bUsed = pCb.userData0 != null;

            HudBar pHudBar = pCb.userData0 as HudBar;
            if(pCb.bUsed)
            {
                Actor pActor = pHudBar.GetActor();
                if (pActor != null)
                {
                    if (pCb.bUsed)
                    {
                        pCb.bUsed = !pActor.IsDestroy() && !pActor.IsFlag(EWorldNodeFlag.Killed) && !pActor.IsInvincible() && pActor.IsActived();
                    }
                }
                else pCb.bUsed = false;
            }
            if (!m_bEnable)
                pCb.bUsed = false;
            if (!pCb.bUsed)
            {
                pHudBar.pInstanceOperiaon = null;
                m_vData.Remove(pHudBar.GetInstanceID());
                pHudBar.Destroy();
                FreeHudBar(pHudBar);
            }
        }
        //------------------------------------------------------
        void OnSpawnCallback(InstanceOperiaon pCb)
        {
            if(pCb.pPoolAble != null)
            {
                HudBar hudBar = pCb.userData0 as HudBar;
                ObjectHudHp hud = pCb.pPoolAble as ObjectHudHp;
                if(hudBar!=null)
                    hudBar.pInstanceOperiaon = null;
                if (hudBar == null || hudBar.GetActor() == null || hud == null || pCb.userData0 == null)
                {
                    if(hudBar!=null)
                    {
                        hudBar.pInstanceOperiaon = null;
                        m_vData.Remove(hudBar.GetInstanceID());
                        hudBar.Destroy();
                        FreeHudBar(hudBar);
                    }

                    pCb.pPoolAble.RecyleDestroy();
                    return;
                }
                hud.SetParent(UI.UIManager.GetAutoUIRoot());
                hud.SetScale(Vector3.one);
                hudBar.Awake(hud);
                hudBar.Show(0);
                if (m_vLoadingBuffer.TryGetValue(hudBar.GetInstanceID(), out BufferState bufferState))
                    hudBar.OnBeginBuff(hudBar.GetActor(), bufferState);
            }
        }
        //------------------------------------------------------
        public void OnBattleWorldCallback(BattleWorld pWorld, EBattleWorldCallbackType type, VariablePoolAble takeData = null)
        {

        }
        //------------------------------------------------------
        public void Destroy()
        {
            foreach (var db in m_vData)
            {
                db.Value.Destroy();
                FreeHudBar(db.Value);
            }
            m_vData.Clear();

            if (m_vLoadingBuffer != null)
            {
                m_vLoadingBuffer.Clear();
            }

            foreach (var db in m_vRecycle)
            {
                foreach (var hud in db.Value) hud.Destroy();
            }
            m_vRecycle.Clear();
            m_vDestroying.Clear();
            m_nHudEnableFlags = 0xffffffff;
        }
        //------------------------------------------------------
        void FreeHudBar(HudBar hudBar)
        {
            hudBar.Destroy();
            m_vPools.Release(hudBar);
        }
        //------------------------------------------------------
        HudBar MallocHudBar()
        {
            return m_vPools.Get();
        }
        //------------------------------------------------------
        public void Update(float fFrame)
        {
            if (!m_bEnable) return;
            bool bRecyle = false;
            HudBar hudHp;
            foreach (var db in m_vData)
            {
                hudHp = db.Value;
                if (hudHp == null)
                {
                    continue;
                }
                if (!hudHp.isInited())
                    continue;
                hudHp.Update();
                if (hudHp.IsEnd() || !IsEnableHud(hudHp.GetBindActorType()) || !IsEnableAttackGroupHud(hudHp.GetAttackGroup()))
                {
                    bRecyle = false;
                    if(hudHp.GetHudType() != 0xff)
                    {
                        Queue<HudBar> vPools;
                        if (!m_vRecycle.TryGetValue(hudHp.GetHudType(), out vPools))
                        {
                            vPools = new Queue<HudBar>(POOL_MAX);
                            m_vRecycle.Add(hudHp.GetHudType(), vPools);
                        }
                        if (vPools.Count <= POOL_MAX)
                        {
                            vPools.Enqueue(hudHp);
                            bRecyle = true;
                        }
                    }

                    hudHp.Clear();
                    if(!bRecyle)
                    {
                        hudHp.Destroy();
                        FreeHudBar(hudHp);
                    }
                    m_vDestroying.Add(db.Key);
                }
            }
            for(int i = 0; i < m_vDestroying.Count; ++i)
            {
                m_vData.Remove(m_vDestroying[i]);
            }
            m_vDestroying.Clear();
        }
    }
}
