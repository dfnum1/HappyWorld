/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	PreviewPlayer
作    者:	HappLI
描    述:	展示角色
*********************************************************************/
using UnityEngine;
using TopGame.Core;
using TopGame.Data;
using Framework.Logic;
using Framework.Core;

namespace TopGame.Logic
{
    [Framework.Plugin.AT.ATExportMono("World/PreviewActor")]
    public class PreviewActor : APreviewActor
    {
        public PreviewActor(AFrameworkModule pGameModuel) : base(pGameModuel)
        {
        }
        //------------------------------------------------------
        public override uint GetConfigID()
        {
            CsvData_Player.PlayerData player = m_pContextData as CsvData_Player.PlayerData;
            if (player == null) return 0;
            return player.ID;
        }
    }
}

