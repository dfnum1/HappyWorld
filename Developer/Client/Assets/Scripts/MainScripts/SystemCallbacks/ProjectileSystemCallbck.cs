/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GameInstance
作    者:	HappLI
描    述:	游戏主线程
*********************************************************************/
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Data;
using UnityEngine;
using Framework.Plugin.AI;
using Framework.Logic;
using Framework.Core;
using Framework.Base;
using Framework.BattlePlus;
using ExternEngine;

namespace TopGame
{
    public partial class GameInstance
    {
        //------------------------------------------------------
        void StopProjectileAI(AProjectile pProjectile)
        {
            //! AI
            if (pProjectile == null || pProjectile.owner_actor == null) return;
            int hitCnt = (pProjectile.projectile!=null)?(pProjectile.projectile.hit_count - pProjectile.remain_hit_count):0;
            int skillId = -1;
            int skillType = -1;
            int actionType = (int)EActionStateType.None;
            int actionId = 0;
            int actionTag = 0;
            int targetActorId = 0;
            if (pProjectile.pTargetNode != null)
                targetActorId = pProjectile.pTargetNode.GetInstanceID();
            if (pProjectile.state_param != null && pProjectile.state_param is Skill)
            {
                Skill skill = pProjectile.state_param as Skill;
                skillId = (int)skill.nGuid;
                skillType = (int)skill.skillType;
                actionTag = (int)skill.Tag;
                Actor owner = pProjectile.owner_actor as Actor;
                ActionState pCur = owner != null ? owner.GetCurrentActionState() : null;
                if (pCur != null && (pCur.GetActionStateType() == EActionStateType.AttackGround || pCur.GetActionStateType() == EActionStateType.AttackAir))
                {
                    actionType = (int)pCur.GetActionStateType();
                    actionId = (int)pCur.GetCore().id;
                }
                else
                {
                    pCur = owner != null ? owner.GetCurrentOverrideActionState() : null;
                    if (pCur != null && (pCur.GetActionStateType() == EActionStateType.AttackGround || pCur.GetActionStateType() == EActionStateType.AttackAir))
                    {
                        actionType = (int)pCur.GetActionStateType();
                        actionId = (int)pCur.GetCore().id;
                    }
                }
            }
            AIWrapper.DoEndProjectileAI(pProjectile.GetAI(), (int)pProjectile.projectile.id, pProjectile.owner_actor.GetInstanceID(), skillId, skillType, actionType, actionId, actionTag, pProjectile.total_damage, hitCnt);
            pProjectile.total_damage = 0;
        }
        //------------------------------------------------------
        public override void OnStopProjectile(AProjectile pProjectile)
        {
            if (pProjectile.vHoldHoles != null)
            {
                Actor target;
                for (int i = 0; i < pProjectile.vHoldHoles.Count; ++i)
                {
                    target = pProjectile.vHoldHoles[i];
                    target.SetSpeedXZ(Vector3.zero);
                    RunerAgent agent = target.GetActorAgent() as RunerAgent;
                    if (agent != null) agent.EnableAILogic(true);
                }
            }
            base.OnStopProjectile(pProjectile);
            StopProjectileAI(pProjectile);
        }
        //------------------------------------------------------
        public override void OnDelayStopProjectile(AProjectile pProjectile)
        {
            if (pProjectile.vHoldHoles != null)
            {
                Actor target;
                for (int i = 0; i < pProjectile.vHoldHoles.Count; ++i)
                {
                    target = pProjectile.vHoldHoles[i];
                    target.SetSpeedXZ(Vector3.zero);
                    RunerAgent agent = target.GetActorAgent() as RunerAgent;
                    if (agent != null) agent.EnableAILogic(true);
                }
            }
            base.OnDelayStopProjectile(pProjectile);
            StopProjectileAI(pProjectile);
        }
        //------------------------------------------------------
        public override void OnProjectileUpdate(AProjectile pProjectile)
        {
            base.OnProjectileUpdate(pProjectile);
            if (pProjectile.projectile.type == EProjectileType.BlackHole)
            {
                if (pProjectile.vHoldHoles != null)
                {
                    Actor target;
                    for (int i = 0; i < pProjectile.vHoldHoles.Count;)
                    {
                        target = pProjectile.vHoldHoles[i];
                        if (target.IsFlag(EWorldNodeFlag.Killed) || target.HasBuffEffect(EBuffEffectBit.HardBody) || pProjectile.vHoldHoles[i].IsDestroy())
                        {
                            pProjectile.vHoldHoles.RemoveAt(i);
                            continue;
                        }
                        RunerAgent agent = target.GetActorAgent() as RunerAgent;
                        if (agent != null) agent.EnableAILogic(false);
                        if( (pProjectile.position - target.GetPosition()).sqrMagnitude<= 1)
                            target.SetPrefSpeedXZ(Vector3.zero);
                        else
                            target.SetPrefSpeedXZ((pProjectile.position - target.GetPosition()).normalized * target.GetRunSpeed());
                        ++i;
                    }
                }
            }

        }
        //------------------------------------------------------
        public override void OnLaunchProjectile(AProjectile pProj)
        {
            base.OnLaunchProjectile(pProj);
            Projectile pProjectile = pProj as Projectile;
#if !USE_SERVER
            //! waring test
            if (pProjectile.TestFinalDropPos())
            {
                InstanceOperiaon pOp = FileSystemUtil.SpawnInstance(pProjectile.projectile.waring_effect, true);
                if (pOp != null)
                {
                    pOp.SetByParent(RootsHandler.ParticlesRoot);
                    pOp.OnCallback =pProjectile.OnSpawnWaringCallback;
                    pOp.OnSign =pProjectile.OnWaringSign;
                    //          pOp.Refresh();
                }
                pProjectile.pWaringCallback = pOp;
            }

            if (!string.IsNullOrEmpty(pProjectile.launch_effect))
            {
                InstanceOperiaon pOp = FileSystemUtil.SpawnInstance(pProjectile.launch_effect, true);
                if (pOp != null)
                {
                    pOp.SetByParent(RootsHandler.ParticlesRoot);
                    pOp.OnCallback =pProjectile.OnSpawnCallback;
                    pOp.OnSign =pProjectile.OnSpawnSign;
          //          pOp.Refresh();
                }
                pProjectile.pCallback = pOp;
            }
            if (pProjectile.launch_sound_id>0)
            {
                AudioManager.PlayID(pProjectile.launch_sound_id);
            }
            else if (!string.IsNullOrEmpty(pProjectile.launch_sound))
            {
                AudioManager.PlayEffect(pProjectile.launch_sound);
            }
#endif
            //! AI
            if(pProjectile.owner_actor!=null)
            {
                int skillId = -1;
                int skillType = -1;
                int actionType = (int)EActionStateType.None;
                int actionId = 0;
                int actionTag = 0;
                int targetActorId = 0;
                if (pProjectile.pTargetNode!=null)
                targetActorId = pProjectile.pTargetNode.GetInstanceID();
                if(pProjectile.state_param !=null && pProjectile.state_param is Skill)
                {
                    Skill skill = pProjectile.state_param as Skill;
                    skillId = (int)skill.nGuid;
                    skillType = (int)skill.skillType;
                    actionTag = (int)skill.Tag;
                    Actor owner = pProjectile.owner_actor as Actor;
                    ActionState pCur = owner!=null? owner.GetCurrentActionState():null;
                    if(pCur!=null && (pCur.GetActionStateType() == EActionStateType.AttackGround || pCur.GetActionStateType() == EActionStateType.AttackAir))
                    {
                        actionType = (int)pCur.GetActionStateType();
                        actionId = (int)pCur.GetCore().id;
                    }
                    else
                    {
                        pCur = owner != null ? owner.GetCurrentOverrideActionState() : null;
                        if (pCur != null && (pCur.GetActionStateType() == EActionStateType.AttackGround || pCur.GetActionStateType() == EActionStateType.AttackAir))
                        {
                            actionType = (int)pCur.GetActionStateType();
                            actionId = (int)pCur.GetCore().id;
                        }
                    }
                }
                AIWrapper.DoLaunchProjectileAI(pProjectile.GetAI(), (int)pProjectile.projectile.id, pProjectile.owner_actor.GetInstanceID(), skillId, skillType, actionType, actionId, actionTag, targetActorId);
            }
        }
        //------------------------------------------------------
        public override void OnProjectileHit(AProjectile pProjectile, ActorAttackData attackData, bool bHitScene, bool bExplode)
        {
            bool bHasEffectPar = false;
//             if(attackData.elementEffect != null && attackData.elementEffect is CsvData_ElemetEffect.ElemetEffectData)
//             {
//                 bHasEffectPar = !string.IsNullOrEmpty((attackData.elementEffect as CsvData_ElemetEffect.ElemetEffectData).hitEffect);
//             }
            //! check element effect
            if (!bHitScene && !bHasEffectPar)
                base.OnProjectileHit(pProjectile, attackData, bHitScene, bExplode);
            else
            {
                if (bHitScene)
                    pProjectile.remain_hit_count = 0;
            }

            if(!bHitScene)
            {
                if (attackData.target_ptr != null)
                {
                    int attackActorID = 0;
                    int attackConfigID = 0;
                    int eAttackType = 0;
                    int atkElement = 0;
                    int targetActorID = 0;
                    int targetConfigID = 0;
                    int eTargetType = 0;
                    int tarElement = 0;
                    if (pProjectile.owner_actor != null)
                    {
                        attackActorID = pProjectile.owner_actor.GetInstanceID();
                        attackConfigID = (int)pProjectile.owner_actor.GetConfigID();
                        eAttackType = (int)pProjectile.owner_actor.GetActorType();
                    }
                    if (pProjectile.pTargetNode != null)
                    {
                        targetActorID = pProjectile.pTargetNode.GetInstanceID();
                        targetConfigID = (int)pProjectile.pTargetNode.GetConfigID();
                        eTargetType = (int)pProjectile.pTargetNode.GetActorType();
                    }

                    Framework.BattlePlus.AIWrapper.DoProjectileHitAI(attackData.target_ptr.GetAI(), (int)pProjectile.projectile.id, attackActorID, attackConfigID, eAttackType, atkElement, targetActorID, targetConfigID, eTargetType, tarElement);
                    if(pProjectile.owner_actor!=null)
                    {
                        Framework.BattlePlus.AIWrapper.DoProjectileHitAI(pProjectile.owner_actor.GetAI(), (int)pProjectile.projectile.id, attackActorID, attackConfigID, eAttackType, atkElement, targetActorID, targetConfigID, eTargetType, tarElement);
                    }
                }
            }

            if (!bHitScene && pProjectile.remain_bound_count>0 && pProjectile.projectile.bound_lock_type != Framework.Data.ELockHitType.None)
            {
                if (!pProjectile.IsKilled() && pProjectile.projectile.bound_lock_conditions != null)
                {
                    byte attackGroup = pProjectile.GetAttackGroup();
                    if (pProjectile.IsBoundInvert())
                        pProjectile.SetAttackGroup((byte)(1 - pProjectile.GetAttackGroup()));

                    for (int i = 0; i < pProjectile.projectile.bound_lock_conditions.Length; ++i)
                    {
                        short LockRode = GlobalDef.INVALID_RODE;
                        Vector3Int param = new Vector3Int();
                        if (pProjectile.projectile.bound_lock_param1 != null && i < pProjectile.projectile.bound_lock_param1.Length)
                            param.x = pProjectile.projectile.bound_lock_param1[i];
                        if (pProjectile.projectile.bound_lock_param2 != null && i < pProjectile.projectile.bound_lock_param2.Length)
                            param.y = pProjectile.projectile.bound_lock_param2[i];
                        if (pProjectile.projectile.bound_lock_param3 != null && i < pProjectile.projectile.bound_lock_param3.Length)
                            param.z = pProjectile.projectile.bound_lock_param3[i];
                        if (pProjectile.projectile.bound_lock_rode != null && i < pProjectile.projectile.bound_lock_rode.Length)
                            LockRode = pProjectile.projectile.bound_lock_rode[i];

                        Framework.Data.LockParam lockPara = new Framework.Data.LockParam() { condition = pProjectile.projectile.bound_lock_conditions[i], param = param, lockRode = LockRode };

                        int boundCnt = pProjectile.projectile.bound_count - pProjectile.remain_bound_count;
                        HashSet<int> vSets = shareParams.intCatchSet;
                        vSets.Clear();
                        if (attackData.attacker_ptr != null && !pProjectile.IsBoundInvert()) vSets.Add(attackData.attacker_ptr.GetInstanceID());
                        if (attackData.target_ptr != null) vSets.Add(attackData.target_ptr.GetInstanceID());
                        
                        List<AWorldNode> vLocks = lockSearcher.LockHitTarget(pProjectile, null, pProjectile.projectile.bound_lock_type, -1, lockPara, pProjectile.projectile.bound_lock_num, pProjectile.projectile.bound_lockHeight, pProjectile.projectile.bound_minLockHeight, pProjectile.projectile.bound_maxLockHeight, 0, vSets);
                        vSets.Clear();
                        if (vLocks!=null)
                        {
                            bool bBoundDiscardBouned = (pProjectile.projectile.bound_flag & ((int)EBoundFlag.BoundDiscardBounded)) != 0;
                            for (int j = 0; j < vLocks.Count; ++j)
                            {
                                if (vLocks[j] == attackData.target_ptr) continue;
                                if (!pProjectile.IsBoundInvert())
                                {
                                    if (vLocks[j] == attackData.attacker_ptr) continue;
                                }
                                if(bBoundDiscardBouned && pProjectile.IsBounded(vLocks[j]))
                                    continue;
                                pProjectile.SetKilled(true);
                                m_pProjectileManager.BoundLaunchProjectile(pProjectile, vLocks[j], attackData.target_ptr, boundCnt, pProjectile.GetAttackGroup(), false);
                            }
                            pProjectile.remain_bound_count--;
                            pProjectile.remain_life_time = 0;
                            vLocks.Clear();
                            break;
                        }
                    }
                    if (pProjectile.IsBoundInvert())
                        pProjectile.SetAttackGroup(attackGroup);
                }
            }
        }
    }
}
