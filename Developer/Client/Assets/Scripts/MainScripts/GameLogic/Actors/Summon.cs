/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Summon
作    者:	HappLI
描    述:	召唤者
*********************************************************************/
using UnityEngine;
using TopGame.Data;
using Framework.Logic;
using Framework.Core;
using Framework.BattlePlus;

namespace TopGame.Logic
{
    [Framework.Plugin.AT.ATExportMono("World/Summon")]
    public class Summon : ASummon
    {
        Vector3 m_FollowOffset;
        Vector3 m_FollowEulerOffset;
        Actor m_pFollow = null;
        //------------------------------------------------------
        public Summon(AFrameworkModule pGameModuel) : base(pGameModuel)
        {
            m_FollowOffset = Vector3.zero;
            m_FollowEulerOffset = Vector3.zero;
            m_pFollow = null;
        }
        //------------------------------------------------------
        public override uint GetConfigID()
        {
            CsvData_Summon.SummonData summonData = m_pContextData as CsvData_Summon.SummonData;
            if (summonData == null) return 0;
            return summonData.id;
        }
        //------------------------------------------------------
        protected override void OnInheritFollowAttr(Actor follow)
        {
            if (follow == null) return;
            ActorParameter trigger = follow.GetActorParameter();
            CsvData_Summon.SummonData summonData = m_pContextData as CsvData_Summon.SummonData;
            if(summonData == null)
            {
                return;
            }

            if(summonData.inheritAttrTypes!=null && summonData.inheritValues!=null && summonData.inheritAttrTypes.Length == summonData.inheritValues.Length)
            {
                for(int i =0; i < summonData.inheritAttrTypes.Length; ++i)
                    m_pActorParameter.AddpendBaseAttr((EAttrType)summonData.inheritAttrTypes[i], trigger.GetBaseAttr((EAttrType)summonData.inheritAttrTypes[i]) * summonData.inheritValues[i] * Framework.Core.CommonUtility.WAN_RATE);
            }
            m_pActorParameter.SetClassify(trigger.GetClassify());
            
            if(follow.GetAttackGroup() != 0)
                BattleKits.ApplayExpandChapterAttr(this, false);
            BattleKits.ApplayAdjustAttrCoefficientFormula(this, false);

            m_pActorParameter.ResetRunTimeParameter();
            m_pActorParameter.AppendSP((int)summonData.baseSp);
        }
    }
}

