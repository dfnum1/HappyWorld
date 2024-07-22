/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GameInstance
作    者:	HappLI
描    述:	游戏主线程
*********************************************************************/

using Framework.Base;
using Framework.Core;
using TopGame.Logic;
using TopGame.SvrData;

namespace TopGame
{
    public partial class GameInstance
    {
        //------------------------------------------------------
        public override void OnSkillInterruptState(Actor pActor, Skill skill, ActionState pState, bool bBegin)
        {
            if (pActor != null && skill != null && bBegin)
            {
                Framework.BattlePlus.AIWrapper.DoSkillInterruptAI(pActor.GetAI(), (int)skill.nGuid, (int)skill.Tag);
            }
        }
    }
}
