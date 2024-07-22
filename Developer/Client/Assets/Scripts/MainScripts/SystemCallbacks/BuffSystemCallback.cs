/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GameInstance
作    者:	HappLI
描    述:	游戏主线程
*********************************************************************/
using TopGame.Data;
using Framework.Core;
using Framework.Base;

namespace TopGame
{
    public partial class GameInstance
    {
        public override BufferState OnCreateBuffer(Actor pActor, AWorldNode trigger, BufferState buff, uint dwBufferID, uint dwLevel, IContextData pData, IBuffParam param = null)
        {
            
            if (m_pDynamicHudHper != null && m_pBattleWorld != null)
            {
                var battleState = ((Framework.BattlePlus.BattleWorld)m_pBattleWorld).GetStatus();
                
                if (battleState > Framework.BattlePlus.EBattleResultStatus.Prepare)
                {
                    //UnityEngine.Debug.LogError($"OnCreateBuffer,battleState:{battleState}");
                    m_pDynamicHudHper.OnBeginBuff(pActor, buff);
                }
            }
            return base.OnCreateBuffer(pActor, trigger, buff, dwBufferID, dwLevel, pData, param);
        }
        //------------------------------------------------------
        public override void OnBeginBuff(Actor pActor, BufferState buff)
        {
            uint flag = (buff.data as CsvData_Buff.BuffData).effectType;
            Framework.BattlePlus.AIWrapper.DoAddBuffAI(pActor.GetAI(), (int)buff.id, (int)buff.type, (int)flag, buff.GetLayer(), (int)buff.groupType, (int)buff.dispelLevel);
            base.OnBeginBuff(pActor, buff);
#if !USE_SERVER
            if (uiManager != null)
                uiManager.OnBeginBuff(pActor, buff);
            if (m_pDynamicHudHper != null && m_pBattleWorld != null)
            {
                var battleState = ((Framework.BattlePlus.BattleWorld)m_pBattleWorld).GetStatus();

                if (battleState > Framework.BattlePlus.EBattleResultStatus.Prepare)
                {
                    m_pDynamicHudHper.OnBeginBuff(pActor, buff);
                }
            }
                
#endif
        }
        //------------------------------------------------------
        public override void OnEndBuff(Actor pActor, BufferState buff)
        {
            uint flag = (buff.data as CsvData_Buff.BuffData).effectType;
            int triggerActorAG = -1;
            if(buff.triggerActor !=0)
            {
                AWorldNode triggerActor = world.FindNode<AWorldNode>(buff.triggerActor);
                if (triggerActor != null) triggerActorAG = triggerActor.GetAttackGroup();
            }
            int reaminTime = (int)((buff.running_duration - buff.running_delta) * 1000);
            if (reaminTime < 0) reaminTime = 0;
            Framework.BattlePlus.AIWrapper.DoRemoveBuffAI(pActor.GetAI(), (int)buff.id, (int)buff.type,(int)flag, reaminTime, buff.triggerActor, triggerActorAG);

            //! AI 全局回调buff 
            Framework.BattlePlus.AIWrapper.OnGlobalRemoveBuff(pActor.GetGameModule().aiSystem, null, pActor.GetInstanceID(), buff.triggerActor, (int)buff.id, (int)buff.type, (int)flag, reaminTime);
            base.OnEndBuff(pActor, buff);
#if !USE_SERVER
            if (uiManager != null)
                uiManager.OnEndBuff(pActor, buff);
            if (m_pDynamicHudHper != null)
                m_pDynamicHudHper.OnEndBuff(pActor, buff);

#endif
        }
    }
}
