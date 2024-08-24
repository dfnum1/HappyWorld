/********************************************************************
生成日期:	3:23:2024  20:39
类    名: 	PVELevel
作    者:	HappLI
描    述:	PVE 关卡
*********************************************************************/
using ExternEngine;
using Framework.Core;
using System.Collections.Generic;
using System.Linq;
using TopGame.Core;
using UnityEngine;

namespace TopGame.Logic
{
    [ModeLogic(EMode.PVE)]
    public class PVELevel : AModeLogic
    {
        AInstanceAble m_pScene;
        List<Vector3> m_vSpawnPoints = new List<Vector3>();
        List<Vector3> m_vBossSpawnPoints = new List<Vector3>();
        List<AWorldNode> m_vMonster = new List<AWorldNode>();
        Dictionary<int,AWorldNode> m_vBossMonster = new Dictionary<int,AWorldNode>();
        List<uint> m_MonsterIDs = new List<uint>();
        List<uint> m_BossIDs = new List<uint>();
        protected override void OnPreStart()
        {
            base.OnPreStart();
            m_MonsterIDs.Add(1203202);
            m_MonsterIDs.Add(1200701);
            m_MonsterIDs.Add(1205801);
            m_MonsterIDs.Add(1201501);
            m_MonsterIDs.Add(1203801);

            m_BossIDs.Add(3200101);


            m_pScene = null;
            var opCall = FileSystemUtil.SpawnInstance("Assets/Datas/Objects/Scenes/Test/TestScene.prefab");
            if (opCall != null)
            {
                opCall.pByParent = ARootsHandler.ScenesRoot;
                opCall.OnSign += OnSpawnSign;
                opCall.OnCallback += OnSpawnScene;
            }
        }
        //------------------------------------------------------
        void OnSpawnScene(InstanceOperiaon callback)
        {
            m_pScene = callback.pPoolAble;
            if (m_pScene != null)
            {
                m_pScene.SetPosition(new Vector3(-25,0,-25));
                m_pScene.SetScale(Vector3.one);
                m_pScene.SetEulerAngle(Vector3.zero);

                Transform swapnPoints = m_pScene.GetTransorm().Find("SpwanPoints/Monsters");
                if(swapnPoints!=null)
                {
                    for (int i =0; i < swapnPoints.childCount; ++i)
                    {
                        m_vSpawnPoints.Add(swapnPoints.GetChild(i).position);
                    }
                }
                swapnPoints = m_pScene.GetTransorm().Find("SpwanPoints/Bosses");
                if (swapnPoints != null)
                {
                    for (int i = 0; i < swapnPoints.childCount; ++i)
                    {
                        m_vBossSpawnPoints.Add(swapnPoints.GetChild(i).position);
                    }
                }
                if (!Physics.autoSyncTransforms) Physics.SyncTransforms();
            }
        }
        //------------------------------------------------------
        void OnSpawnSign(InstanceOperiaon callback)
        {
            callback.bUsed = this.isActived();
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            base.InnerUpdate(fFrame);

#if UNITY_EDITOR
            if (CameraKit.IsEditorMode)
                return;
#endif

            if (m_vMonster.Count <= 1 && m_vSpawnPoints.Count > 0)
            {
                FVector3 playerPos = GetModeLogic<PVEPlayer>().GetPosition();
                for (int i = 0; i < 1; ++i)
                {
                    var data = Data.DataManager.getInstance().Monster.GetData(m_MonsterIDs[UnityEngine.Random.Range(0, m_MonsterIDs.Count)]);
                    if (data != null)
                    {
                        Monster monster = GetWorld().CreateNode<Monster>(EActorType.Monster, data);
                        if (monster != null)
                        {
                            monster.EnableAI(true);
                            monster.EnableLogic(true);
                            monster.EnableRVO(true);
                            monster.EnableSkill(true);
                            monster.SetAttackGroup(1);
                            monster.GetActorParameter().SetLevel((ushort)(1));
                            monster.StartActionByType(EActionStateType.Enter, 0, 1, true, false, true);
                            monster.SetFinalPosition(m_vSpawnPoints[GetFramework().GetRamdom(0, m_vSpawnPoints.Count)]);

                            Vector3 dir = playerPos - monster.GetPosition();
                            if (dir.sqrMagnitude > 0) monster.SetDirection(dir);
                            m_vMonster.Add(monster);
                        }
                    }
                }
            }
            if (m_vBossSpawnPoints.Count > 0)
            {
                FVector3 playerPos = GetModeLogic<PVEPlayer>().GetPosition();
                for (int i = 0; i < m_vBossSpawnPoints.Count; ++i)
                {
                    if (m_vBossMonster.TryGetValue(i,out var bossActor))
                    {
                        if (bossActor != null && !bossActor.IsKilled() && !bossActor.IsDestroy())
                            continue;

                        if(bossActor!=null) bossActor.Destroy();
                    }
                    var data = Data.DataManager.getInstance().Monster.GetData(m_BossIDs[UnityEngine.Random.Range(0, m_BossIDs.Count)]);
                    if (data != null)
                    {
                        Monster monster = GetWorld().CreateNode<Monster>(EActorType.Monster, data);
                        if (monster != null)
                        {
                            monster.EnableAI(true);
                            monster.EnableLogic(true);
                            monster.EnableRVO(true);
                            monster.EnableSkill(true);
                            monster.SetAttackGroup(1);
                            monster.GetActorParameter().SetLevel((ushort)(100));
                            monster.GetActorParameter().AddpendBaseAttr( EAttrType.MaxHp, 1000000);
                            monster.GetActorParameter().AppendHP(10000);
                            monster.StartActionByType(EActionStateType.Enter, 0, 1, true, false, true);
                            monster.SetFinalPosition(m_vBossSpawnPoints[i]);

                            Vector3 dir = playerPos - monster.GetPosition();
                            if (dir.sqrMagnitude > 0) monster.SetDirection(dir);
                            m_vBossMonster[i] =monster;
                        }
                    }
                }
            }
            for (int i = 0; i < m_vMonster.Count;)
            {
                if (m_vMonster[i].IsDestroy() || m_vMonster[i].IsKilled())
                {
                    m_vMonster.RemoveAt(i);
                    continue;
                }
                ++i;
            }
            foreach (var db in m_vBossMonster)
            {
                if (db.Value == null) continue;
                if (db.Value.IsDestroy() || db.Value.IsKilled())
                {
                    db.Value.Destroy();
                    m_vBossMonster[db.Key] = null;
                }
            }
        }
        //------------------------------------------------------
        protected override void OnClear()
        {
            base.OnClear();
            if (m_pScene != null) m_pScene.Destroy();
            m_pScene = null;

            foreach (var db in m_vMonster)
            {
                db.Destroy();
            }
            m_vMonster.Clear();

            foreach (var db in m_vBossMonster)
            {
                if(db.Value!=null)db.Value.Destroy();
            }
            m_vBossMonster.Clear();
        }
    }
}