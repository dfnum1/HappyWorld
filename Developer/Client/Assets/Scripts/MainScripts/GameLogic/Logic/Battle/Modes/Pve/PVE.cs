/********************************************************************
生成日期:	3:23:2024  20:31
类    名: 	Pve
作    者:	HappLI
描    述:	PVE 副本
*********************************************************************/
using UnityEngine;
using TopGame.Core;
using TopGame.Data;
using Framework.Base;
using Framework.Logic;
using Framework.Core;
using Framework.BattlePlus;

namespace TopGame.Logic
{
    [Mode(Logic.EGameState.Battle, Logic.EMode.PVE, 100)]
    public class PVE : AbsMode
    {
        protected override void OnPreStart()
        {
            base.OnPreStart();

            EnableBattleLogic<BattlePVP>(false);
            EnableBattleLogic<BattlePlayers>(false);
            EnableBattleLogic<BattleRegions>(false);


            //Test
            AudioUtil.PlayID(100);
        }
        //------------------------------------------------------
        protected override void OnStart()
        {
            base.OnStart();
            GetBattleWorld().Start();
            GetBattleWorld().Active(true);
            GetWorld().SetLimitZoom(ExternEngine.FVector3.min, ExternEngine.FVector3.max);
        }
    }
}