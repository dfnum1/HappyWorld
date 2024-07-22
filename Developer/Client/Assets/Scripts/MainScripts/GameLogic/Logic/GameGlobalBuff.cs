/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GameGlobalBuffer
作    者:	HappLI
描    述:	游戏全局buff 
*********************************************************************/

using System.Collections.Generic;
using Framework.BattlePlus;
using Framework.Core;
using Framework.Logic;
using TopGame.Data;

namespace TopGame.Logic
{
    public class GameGlobalBuffer : GlobalBuff
    {
        //------------------------------------------------------
        public List<GlobalBufferState> GetGloablBuffs()
        {
            return m_Buffs;
        }
        //------------------------------------------------------
        public override bool AddBuff(uint buffId, Actor trigger = null, IBuffParam param = null)
        {
//             CsvData_GlobalBuff.GlobalBuffData buff = Data.DataManager.getInstance().GlobalBuff.GetData(buffId);
//             if (buff == null || buff.Buff_buffs_data == null || buff.Buff_buffs_data.Length<=0) return false;
//             
// 
//             GlobalBufferState globalBuff = FindGlobalBuff(buffId);
//             if(globalBuff == null)
//             {
//                 globalBuff = BuffUtil.NewGlobalBufferState(m_pFramework);
//                 globalBuff.id = buffId;
//                 globalBuff.nCooldown = (int)buff.coolingTime;
//                 globalBuff.quality = buff.quality;
//                 globalBuff.attackGroup = buff.attackGroup;
//                 globalBuff.SetMultiLayer(1);
//                 globalBuff.SetMatch(buff.conditionType, buff.conditionData);
//                 globalBuff.configData = buff;
//                 m_Buffs.Add(globalBuff);
//             }
//             for(int i = 0; i < buff.Buff_buffs_data.Length; ++i)
//             {
//                 CsvData_Buff.BuffData buffData = buff.Buff_buffs_data[i];
//                 BufferState buffState = globalBuff.FindBuff(buffData.id);
//                 BufferState returnBuff = BuffHelper.CreateBuffer(m_pFramework, trigger, null, buffState, buffId,1, buffData, param);
//                 if(returnBuff != buffState)
//                 {
//                     //! is new
//                     globalBuff.AddBuff(returnBuff);
//                 }
//             }
// 
//             if (CanMatch())
//             {
//                 CheckConditionMatch(globalBuff);
//                 if (globalBuff.isActived)
//                     globalBuff.Begin();
//             }
            return true;
        }
        //------------------------------------------------------
        protected override void DoDamage(Actor pActor, GlobalBufferState buffer)
        {
            if (buffer.GetBuffs() == null) return;
            BattleWorld battleWorld = m_pFramework.Get<BattleWorld>(false);
            if (battleWorld == null) return;
            BattleDamager damager = battleWorld.GetLogic<BattleDamager>();
            if (damager == null) return;
            for (int i = 0; i < buffer.GetBuffs().Count; ++i)
            {
                CsvData_Buff.BuffData buff = buffer.GetBuffs()[i].data as CsvData_Buff.BuffData;
                if(buff!=null)
                {
                    CsvData_SkillDamage.SkillDamageData pDamage = DataManager.getInstance().SkillDamage.GetData(buff.damage);
                    if (pDamage == null) continue;
                    if (damager != null)
                    {
                        ActorAttackData attack = ActorAttackData.DEFAULT;
                        attack.attacker_ptr = null;
                        attack.target_ptr = pActor;
                        attack.target_element_flags = pActor.GetElementFlags();
                        attack.target_id = pActor.GetInstanceID();
                        attack.target_config_id = (int)pActor.GetConfigID();
                        attack.target_type = pActor.GetActorType();
                        damager.AddTickDamage(pDamage, attack, 0);
                    }
                }
            }
        }
        //------------------------------------------------------
        protected override int CheckActorCareer(byte attackGroup, byte career)
        {
            if (!CanMatch()) return 0;
            return base.CheckActorCareer(attackGroup, career);
        }
        //------------------------------------------------------
        protected override bool CheckActorHp(byte attackGroup, int rate, bool bUpper)
        {
            if (!CanMatch()) return false;
            return base.CheckActorHp(attackGroup, rate, bUpper);
        }
        //------------------------------------------------------
        bool CanMatch()
        {
            AFrameworkModule frameModule = m_pFramework as AFrameworkModule;
            if (frameModule == null) return false;
            if (frameModule.battleWorld == null) return false;
            BattleWorld battleWorld = frameModule.battleWorld as BattleWorld;
            if (battleWorld == null) return false;
            return battleWorld.IsStarting();
        }
        //------------------------------------------------------
        public override void OnActiveActor(Actor pActor)
        {
            if (!CanMatch()) return;
            base.OnActiveActor(pActor);
        }
    }
}

