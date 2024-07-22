/********************************************************************
生成日期:	27:6:2023   17:39
类    名: 	CsvData_DropReward
作    者:	Ywm
描    述:	CsvData_DropReward
*********************************************************************/

using System;
using System.Collections.Generic;
using Framework.Data;
using TopGame.Base;

namespace TopGame.Data
{
    public partial class CsvData_DropReward : Data_Base
    {
        Dictionary<uint, Dictionary<uint,DropRewardData>> m_vGroups = new Dictionary<uint, Dictionary<uint,DropRewardData>>();
        
        //-------------------------------------------
        protected override void OnClearData()
        {
            base.OnClearData();
            if (m_vGroups != null) m_vGroups.Clear();
        }
        
        
        //-------------------------------------------
        protected override void OnAddData(Framework.Plugin.AT.IUserData baseData)
        {
            base.OnAddData(baseData);
            DropRewardData dropRewardData = baseData as DropRewardData;
            Dictionary<uint,DropRewardData> dropGourp;
            if (!m_vGroups.TryGetValue(dropRewardData.gourpId, out dropGourp))
            {
                dropGourp = new Dictionary<uint,DropRewardData>();
                m_vGroups.Add(dropRewardData.gourpId, dropGourp);
            }
            dropGourp.Add(dropRewardData.chapterId,dropRewardData);
        }

        //------------------------------------------------------
        public DropRewardData GetDropRewardByGP(uint groupId, uint chapterId)
        {
            DropRewardData dropRewardData;
            Dictionary<uint,DropRewardData> dropGourp;
            if (m_vGroups.TryGetValue(groupId, out dropGourp))
            {
                if (dropGourp.TryGetValue(chapterId, out dropRewardData))
                {
                    return Util.RandomNumber(0, 10000) < dropRewardData.weight ? dropRewardData : null;
                }
                //chapter 0 为通用
                if (dropGourp.TryGetValue(0, out dropRewardData))
                {
                    return Util.RandomNumber(0, 10000) < dropRewardData.weight ? dropRewardData : null;
                }
            }
            return null;
        }
    }
}