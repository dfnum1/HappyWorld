/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GameInstance
作    者:	HappLI
描    述:	游戏主线程
*********************************************************************/
using TopGame.Core;
using TopGame.Data;
using Framework.Core;
using UnityEngine;
namespace TopGame
{
    public partial class GameInstance
    {
        //------------------------------------------------------
        public override void OnCreateDropItem(DropItem pItem)
        {
            CsvData_DropReward.DropRewardData pDropData = DataManager.getInstance().DropReward.GetData(pItem.nDropId);
            if (pDropData == null) return;
            pItem.pData = pDropData;
            pItem.fPickDistance = pDropData.fPickDistance * pDropData.fPickDistance;
            if (pDropData.fPickDistance <= 0) pItem.bAutoPick = true;
#if !USE_SERVER
            if (!string.IsNullOrEmpty(pDropData.effectModel))
            {
                InstanceOperiaon pCallback = FileSystemUtil.SpawnInstance(pDropData.effectModel, true);
                if(pCallback != null)
                {
                    pCallback.pByParent = RootsHandler.FindActorRoot((int)EActorType.Element);
                    pCallback.OnSign = ((DropItem)pItem).OnSpawnSign;
                    pCallback.OnCallback = ((DropItem)pItem).OnSpawnCallback;
                }
            }
            if (!string.IsNullOrEmpty(pDropData.dropSound))
            {
                AudioManager.PlayEffect(pDropData.dropSound);
            }
#endif
        }
        //------------------------------------------------------
        public override void OnReadyToPick(DropItem pItem)
        {
        }
        //------------------------------------------------------
        public override void OnPickedDrop(DropItem pItem)
        {
#if !USE_SERVER
            if (pItem.pData == null) return;
            CsvData_DropReward.DropRewardData pDropData = (CsvData_DropReward.DropRewardData)pItem.pData;
            if (!string.IsNullOrEmpty(pDropData.pickSound))
            {
                AudioManager.PlayEffect(pDropData.pickSound);
            }

            if(!string.IsNullOrEmpty(pDropData.pickEffect))
            {
                
            }
#endif
        }
    }
}
