/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Player
作    者:	HappLI
描    述:	玩家对象
*********************************************************************/
using UnityEngine;
using TopGame.Data;
using Framework.Logic;
using Framework.Core;
using Framework.BattlePlus;

namespace TopGame.Logic
{
    [Framework.Plugin.AT.ATExportMono("World/Player")]
    public class Player : APlayer
    {
        public Player(AFrameworkModule pGameModuel) : base(pGameModuel)
        {

        }
        //------------------------------------------------------
        public override uint GetConfigID()
        {
            CsvData_Player.PlayerData player = m_pContextData as CsvData_Player.PlayerData;
            if (player == null) return 0;
            return player.ID;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public override bool IsFrontRow()
        {
            return true;
        }
    }
}

