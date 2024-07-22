/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Monster
作    者:	HappLI
描    述:	怪物
*********************************************************************/
using UnityEngine;
using Framework.Core;
using TopGame.Data;
using Framework.Logic;
using Framework.Base;
using Framework.BattlePlus;
using TopGame.SvrData;

namespace TopGame.Logic
{
    [Framework.Plugin.AT.ATExportMono("World/Monster")]
    public class Monster : AMonster
    {
        public Monster(AFrameworkModule pGameModuel) : base(pGameModuel)
        {

        }
        //------------------------------------------------------
        public override uint GetConfigID()
        {
            CsvData_Monster.MonsterData monster = m_pContextData as CsvData_Monster.MonsterData;
            if (monster == null) return 0;
            return monster.id;
        }
        //------------------------------------------------------
        protected override void InnerCreated()
        {
            base.InnerCreated();
            CsvData_Monster.MonsterData monster = m_pContextData as CsvData_Monster.MonsterData;
            if (monster == null) return;

            if(monster.object_scale>0)
            {
                SetScale(Vector3.one * monster.object_scale);
            }

//             if(!string.IsNullOrEmpty(monster.ownEffect))
//                 m_pActorAgent.CreateParticleExplicit(monster.ownEffect, Vector2.zero, Vector3.zero, FindBindSlot(monster.ownParentSlot), monster.scaleOwnEffect, (ESlotBindBit)monster.ownBindBite);
//             if (monster.dropReward!=null && GetGameModule()!=null && GetGameModule().dropManager!=null && UserManager.MySelf != null)
//             {
//                 for (int i = 0; i < monster.dropReward.Length; ++i)
//                 {
//                     if (rewardData != null)
//                     {
//                         GetGameModule().dropManager.AddDrop(GetActorType(), GetInstanceID(), rewardData.ID, 1);
//                     }
//                 }
//             }
        }
        //------------------------------------------------------
        public override IContextData GetDoDamageData()
        {
            if (m_pContextData == null) return null;
            return (m_pContextData as CsvData_Monster.MonsterData).SkillDamage_damageId_data;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public override EMonsterType GetMonsterType()
        {
            if (m_pContextData == null) return EMonsterType.Normal;
            CsvData_Monster.MonsterData monster = m_pContextData as CsvData_Monster.MonsterData;
            if (monster != null) return monster.monsterType;
            return EMonsterType.Normal;
        }
        //------------------------------------------------------
        public override byte GetCollisionFilter()
        {
            if (m_pContextData == null) return 0xff;
            return (m_pContextData as CsvData_Monster.MonsterData).collisionFilter;
        }

    }
}

