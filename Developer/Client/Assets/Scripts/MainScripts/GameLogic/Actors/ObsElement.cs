/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ObsElement
作    者:	HappLI
描    述:	场景元素对象
*********************************************************************/
using UnityEngine;
using Framework.Core;
using TopGame.Data;
using Framework.Logic;
using Framework.BattlePlus;
using ExternEngine;

namespace TopGame.Logic
{
    [Framework.Plugin.AT.ATExportMono("World/ObsElement")]
    public class ObsElement : AObsElement
    {
#if !USE_SERVER
        private AInstanceAble m_pOwnEffect;
#endif
        System.Collections.Generic.List<AInstanceAble> m_vTransferPoints = null;
        //------------------------------------------------------
        public ObsElement(AFrameworkModule pGame) : base(pGame) { }
        //------------------------------------------------------
        protected override void InnerCreated()
        {
#if !USE_SERVER
            if (m_pOwnEffect != null) m_pOwnEffect.RecyleDestroy(2);
            m_pOwnEffect = null;
#endif
            OnContextDataDirty();
            CsvData_BattleObject.BattleObjectData obs = m_pContextData as CsvData_BattleObject.BattleObjectData;
            if (obs == null) return;
            m_nElementFlags = obs.element;
            nCollisionCnt = obs.hitCount;
            SetScale(Vector3.one*obs.sacle);
            nElementCollisionCnt = obs.elementHitCount;
#if !USE_SERVER
            InstanceOperiaon pEffectCB = FileSystemUtil.SpawnInstance(obs.ownEffect, true);
            if (pEffectCB != null)
            {
                pEffectCB.OnCallback =OnOwnerSpawnEffect;
                pEffectCB.OnSign = OnOwnerSign;
                pEffectCB.SetAsync(true);
            }
#endif
        }
        //------------------------------------------------------
        protected override void OnContextDataDirty() 
        {
            CsvData_BattleObject.BattleObjectData obs = m_pContextData as CsvData_BattleObject.BattleObjectData;
            if (obs == null)
            {
                m_BoundBox.Clear();
                return;
            }
            SetScale(Vector3.one * obs.sacle);
            m_BoundBox.Set(obs.aabb_min, obs.aabb_max);
        }
        //------------------------------------------------------
        public override uint GetConfigID()
        {
            if (m_pContextData == null) return 0;
            CsvData_BattleObject.BattleObjectData obs = m_pContextData as CsvData_BattleObject.BattleObjectData;
            if (obs != null) return obs.id;
            return 0;
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (m_vTransferPoints != null)
            {
                for (int i = 0; i < m_vTransferPoints.Count; ++i)
                {
                    if (m_vTransferPoints[i]) m_vTransferPoints[i].Destroy();
                }
                m_vTransferPoints.Clear();
            }
            
#if !USE_SERVER
                if (m_pOwnEffect != null) m_pOwnEffect.RecyleDestroy(2);
            m_pOwnEffect = null;
#endif
        }
        //------------------------------------------------------
        public override bool IsTweening()
        {
#if !USE_SERVER
            if (m_pObjectAble == null) return false;
            TweenEffecter tween = m_pObjectAble as TweenEffecter;
            if (tween && tween.IsPlaying()) return true;
#endif
            return false;
        }
        //------------------------------------------------------
        protected override float GetCollisonGap()
        {
            CsvData_BattleObject.BattleObjectData battleObject = m_pContextData as CsvData_BattleObject.BattleObjectData;
            if (battleObject == null) return 1000;
            return battleObject.interval;
        }
        //------------------------------------------------------
        public override Framework.Base.EObstacleType GetObstacleType()
        {
            if (m_pContextData == null) return Framework.Base.EObstacleType.None;
            CsvData_BattleObject.BattleObjectData obs = m_pContextData as CsvData_BattleObject.BattleObjectData;
            if (obs != null) return obs.objectType;
            return Framework.Base.EObstacleType.None;
        }
        //------------------------------------------------------
        public override bool PlayState(string state)
        {
            if (string.IsNullOrEmpty(state)) return false;
            if (state.CompareTo(m_strCurAcitonName) == 0) return true;
            m_strCurAcitonName = state;
#if !USE_SERVER
            if (m_pObjectAble == null)
                return false;
            bool isPlayed = false;
            Animator pAnimator = m_pObjectAble.GetBehaviour<Animator>();
            if (pAnimator)
            {
                if (pAnimator.HasState(0, AnimatorUtil.ToHash(state)))
                {
                    AnimatorStateInfo stateInfo = pAnimator.GetCurrentAnimatorStateInfo(0);
                    if (!stateInfo.IsName(state))
                    {
                        isPlayed = true;
                        pAnimator.CrossFade(m_strCurAcitonName, 0);
                    }
                }
            }

            Core.VirtualActions actionCtl = m_pObjectAble as Core.VirtualActions;
            if (actionCtl)
            {
                if (actionCtl.Play(m_strCurAcitonName))
                    isPlayed = true;
            }
#endif
            return isPlayed;
        }
        //-------------------------------------------------
        public override bool IsCanLogic()
        {
            bool bCan = base.IsCanLogic() && m_BoundBox.GetBoundSizeSqr()>0;
#if !USE_SERVER
            if (bCan)
            {
                CsvData_BattleObject.BattleObjectData obs = m_pContextData as CsvData_BattleObject.BattleObjectData;
                if (obs != null && obs.collisionActions != null && obs.collisionActions.Length > 0)
                {
                    if (m_pObjectAble != null)
                    {
                        Animator pAnimator = m_pObjectAble.GetBehaviour<Animator>();
                        if (pAnimator)
                        {
                            AnimatorStateInfo stateInfo = pAnimator.GetCurrentAnimatorStateInfo(0);
                            for (int i = 0; i < obs.collisionActions.Length; ++i)
                            {
                                if (stateInfo.IsName(obs.collisionActions[i]))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    return false;
                }
            }
#endif
            return bCan;
        }
#if !USE_SERVER
        //------------------------------------------------------
        public void OnOwnerSign(InstanceOperiaon pCallback)
        {
            if (IsDestroy() || m_pContextData == null || IsKilled()) pCallback.SetUsed(false);
            if (pCallback.IsUsed())
                pCallback.SetByParent(RootsHandler.FindActorRoot((int)EActorType.Element));
        }
        //------------------------------------------------------
        public void OnOwnerSpawnEffect(InstanceOperiaon pCallback)
        {
            if (m_pOwnEffect != pCallback.GetAble())
            {
                if (m_pOwnEffect != null) m_pOwnEffect.RecyleDestroy(32);
            }
            m_pOwnEffect = pCallback.GetAble();
        }
#endif
        //-------------------------------------------------
        protected override void OnInnerSpawnObject()
        {
            base.OnInnerSpawnObject();
#if !USE_SERVER
            if (m_pObjectAble is Core.ParticleController)
            {
                Core.ParticleController parCtl = m_pObjectAble as Core.ParticleController;
                parCtl.SetAutoDestroy(false);
            }
            TweenEffecter tween = m_pObjectAble as TweenEffecter;
            if (tween != null)
                tween.Stop(false);
            if (m_pObjectAble != null && m_pObjectAble.GetParent()!=null)
                m_pObjectAble.SetRenderLayer(m_pObjectAble.GetParent().gameObject.layer);
#endif
            AdjustTerrainHeight();
        }
        //-------------------------------------------------
        protected override void OnFlagDirty(EWorldNodeFlag flag, bool IsUsed)
        {
            base.OnFlagDirty(flag, IsUsed);
            if(flag == EWorldNodeFlag.Killed)
            {
                if(IsUsed)
                {
                    PlayState("dead");
#if !USE_SERVER
                    if (m_pOwnEffect!=null) m_pOwnEffect.RecyleDestroy(32);
                    m_pOwnEffect = null;
#endif
                }
                return;
            }
        }
        //------------------------------------------------------
        protected override void OnActionActiveUpdate(FFloat fFrame)
        {
            CsvData_BattleObject.BattleObjectData battleObject = m_pContextData as CsvData_BattleObject.BattleObjectData;
            if (battleObject != null)
            {
                if (!m_bActiveAction)
                {
                    if (battleObject.activateDistance > 0 && GetGameModule() != null)
                    {
                        FVector3 diff = GetGameModule().GetPlayerPosition()- GetPosition();
                        diff.y = FFloat.zero;
                        if (diff.sqrMagnitude <= battleObject.activateDistance* battleObject.activateDistance)
                        {
                            m_bActiveAction = true;
                        }
                    }
                }
                else
                {
                    if (m_fActionSeqDelta >= 0)
                    {
                        m_fActionSeqDelta -= fFrame;
                        if (m_fActionSeqDelta <= 0)
                        {
                            if (battleObject.actionSequences != null && battleObject.actionSequences.Length > 0)
                            {
                                PlayState(battleObject.actionSequences[m_ActionSeqIndex % battleObject.actionSequences.Length]);
                            }
                            m_fActionSeqDelta = battleObject.intervalTime * 0.001f;
                            m_ActionSeqIndex++;
                        }
                    }
                }
            }
        }
    }
}

