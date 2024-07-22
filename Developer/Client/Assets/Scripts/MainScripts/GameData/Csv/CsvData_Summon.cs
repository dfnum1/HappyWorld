/********************************************************************
类    名:   CsvData_Summon
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.Data;
using Framework.Core;
using Framework.Base;
namespace TopGame.Data
{
    public partial class CsvData_Summon : Data_Base
    {
        Dictionary<uint, List<SummonData>> m_vGroups = new Dictionary<uint, List<SummonData>>();
        //-------------------------------------------
        protected override void OnClearData()
        {
            base.OnClearData();
            m_vGroups?.Clear();
        }
        //-------------------------------------------
        protected override void OnAddData(Framework.Plugin.AT.IUserData baseData)
        {
            SummonData buff = baseData as SummonData;
            if (buff.groupId <= 0) return;
            List<SummonData> vBuffs;
            if (!m_vGroups.TryGetValue(buff.groupId, out vBuffs))
            {
                vBuffs = new List<SummonData>();
                m_vGroups.Add(buff.groupId, vBuffs);
            }
            vBuffs.Add(buff);
        }
        //-------------------------------------------
        public SummonData GetSummon(uint groupId, ushort level)
        {
            List<SummonData> vSummons;
            if (m_vGroups.TryGetValue(groupId, out vSummons) && vSummons.Count>0)
            {
                for (int i = vSummons.Count - 1; i >= 0; --i)
                {
                    if (level >= vSummons[i].level)
                        return vSummons[i];
                }
                return vSummons[0];
            }
            return null;
        }
    }
}
