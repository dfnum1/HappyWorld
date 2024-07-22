/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Wingman
作    者:	HappLI
描    述:	僚机
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;
using TopGame.Core;
using TopGame.Data;
using Framework.Logic;
using Framework.Core;
using Framework.Data;
using Framework.BattlePlus;
using TopGame.SvrData;

namespace TopGame.Logic
{
    [Framework.Plugin.AT.ATExportMono("World/Wingman")]
    public class Wingman : AWingman
    {
        uint[] m_arrPowerSkillNeedElements = null;
        //------------------------------------------------------
        public Wingman(AFrameworkModule pGameModuel) : base(pGameModuel)
        {

        }
        //------------------------------------------------------
        protected override void InnerCreated()
        {
            base.InnerCreated();
            m_arrPowerSkillNeedElements = null;
        }
        //------------------------------------------------------
        public override uint GetConfigID()
        {
            CsvData_Summon.SummonData summonData = m_pContextData as CsvData_Summon.SummonData;
            if (summonData == null) return 0;
            return summonData.id;
        }
        //------------------------------------------------------
        public uint GetPetConfigID()
        {
            CsvData_Summon.SummonData summonData = m_pContextData as CsvData_Summon.SummonData;
            if (summonData == null) return 0;
            return summonData.groupId;
        }
        
        //------------------------------------------------------
        public override bool IsFrontRow()
        {
            return true;
        }
        //-------------------------------------------------
        protected override void OnExternDataDirty(BaseData pExtern)
        {
            m_arrPowerSkillNeedElements = null;
            if (pExtern == null) return;
        }
        //------------------------------------------------------
        public override uint[] GetTriggerSkillNeedElements()
        {
            return m_arrPowerSkillNeedElements;
        }
        //------------------------------------------------------
        public uint GetGrounpID()
        {
            CsvData_Summon.SummonData summonData = m_pContextData as CsvData_Summon.SummonData;
            if (summonData == null) return 0;
            return summonData.groupId;
        }
        //------------------------------------------------------
        public int GetLevel()
        {
            CsvData_Summon.SummonData summonData = m_pContextData as CsvData_Summon.SummonData;
            if (summonData == null) return 0;
            return summonData.level;
        }

        //------------------------------------------------------
        public void AddBuffs()
        {
        }
    }
}

