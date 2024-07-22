/********************************************************************
类    名:   CsvData_Buff
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
    public partial class CsvData_Buff : Data_Base
    {
        Dictionary<uint, List<BuffData>> m_vGroups = new Dictionary<uint, List<BuffData>>();
        //-------------------------------------------
        protected override void OnClearData()
        {
            base.OnClearData();
            if (m_vGroups != null) m_vGroups.Clear();
        }
        //-------------------------------------------
        protected override void OnAddData(Framework.Plugin.AT.IUserData baseData)
        {
            BuffData buff = baseData as BuffData;
            if (buff.groupID <= 0) return;
            List<BuffData> vBuffs;
            if (!m_vGroups.TryGetValue(buff.groupID, out vBuffs))
            {
                vBuffs = new List<BuffData>();
                m_vGroups.Add(buff.groupID, vBuffs);
            }
            vBuffs.Add(buff);
        }
        //-------------------------------------------
        public BuffData GetBuff(uint groupId, uint level)
        {
			if(level<=0)level = 1;
            List<BuffData> vBuffs;
            if (m_vGroups.TryGetValue(groupId, out vBuffs) && vBuffs.Count>0)
            {
                for (int i = vBuffs.Count - 1; i >= 0; --i)
                {
                    if (level >= vBuffs[i].level)
                        return vBuffs[i];
                }
				if( level >= vBuffs[0].level ) return vBuffs[0];
            }
            return null;
        }
        //------------------------------------------------------
        public static string[] GetValues(BuffData buffData)
        {
            string[] strings = new string[buffData.valueTypes.Length];
            for (int i = 0; i < buffData.valueTypes.Length; i++)
            {
                strings[i] = buffData.valueTypes[i] == 1
                    ? buffData.values[i].ToString()
                    : buffData.values[i] / 100 + "%";
            }

            return strings;
        }
    }
}
