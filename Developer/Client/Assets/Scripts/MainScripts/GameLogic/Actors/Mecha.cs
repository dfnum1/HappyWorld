/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Mecha
作    者:	HappLI
描    述:	机甲
*********************************************************************/
using UnityEngine;
using Framework.Core;
using TopGame.Data;
using System.Collections.Generic;
using Framework.Logic;
using Framework.Data;
using Framework.BattlePlus;
using ExternEngine;

namespace TopGame.Logic
{
    [Framework.Plugin.AT.ATExportMono("World/Mecha")]
    public class Mecha : AMecha
    {
        public Mecha(AFrameworkModule pGameModuel) : base(pGameModuel)
        {

        }
        //------------------------------------------------------
        public override uint GetConfigID()
        {
            return 0;
        }
        //------------------------------------------------------
        public override void InheritActors(List<Actor> vPlayers)
        {
//             CsvData_Mecha.MechaData mechData = m_pContextData as CsvData_Mecha.MechaData;
//             if (mechData == null) return;
//             if(vPlayers!=null && vPlayers.Count>0 && mechData.inheritTypes!=null && mechData.inheritValues!=null && mechData.inheritTypes.Length == mechData.inheritValues.Length)
//             {
//                 m_pActorParameter.UpdataAttr();
//                 FFloat[] attrs = GetShareFrameParams().arrAttrs;
//                 System.Array.Clear(attrs, 0, attrs.Length);
//                 for (int i = 0; i < vPlayers.Count; ++i)
//                 {
//                     for (int j = 0; j < mechData.inheritTypes.Length; ++j)
//                     {
//                         if (mechData.inheritTypes[j] >= 0 && mechData.inheritTypes[j] < (int)EAttrType.Num)
//                         {
//                             attrs[(int)mechData.inheritTypes[j]] += vPlayers[i].GetActorParameter().GetBaseAttr((EAttrType)mechData.inheritTypes[j]);
//                         }
//                     }
//                 }
//                 for (int i = 0; i < mechData.inheritTypes.Length; ++i)
//                 {
//                     if(mechData.inheritTypes[i]>=0 && mechData.inheritTypes[i]< (int)EAttrType.Num)
//                         m_pActorParameter.AddpendBaseAttr((EAttrType)mechData.inheritTypes[i], attrs[(int)mechData.inheritTypes[i]] * (mechData.inheritValues[i] * Framework.Core.CommonUtility.WAN_RATE));
//                 }
//             }

 //           m_pActorParameter.ResetRunTimeParameter();
        }
    }
}

