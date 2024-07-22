/********************************************************************
类    名:   CsvData_Guide
作    者:	Happli
描    述:	
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.Data;
namespace TopGame.Data
{
    public partial class CsvData_Player : Data_Base
    {
        Dictionary<uint, List<PlayerData>> m_vMainDatas = new Dictionary<uint, List<PlayerData>>();
        Dictionary<uint, PlayerData> m_vEquipDatas = new Dictionary<uint, PlayerData>();
        //-------------------------------------------
        protected override void OnClearData()
        {
            base.OnClearData();
            if (m_vMainDatas != null) m_vMainDatas.Clear();
            if (m_vEquipDatas != null) m_vEquipDatas.Clear();
        }
        //-------------------------------------------
        protected override void OnAddData(Framework.Plugin.AT.IUserData baseData) 
        {
            PlayerData player = baseData as PlayerData;
            if (player.heroNoumenon > 0)
            {
                List<PlayerData> vPlayers;
                if(!m_vMainDatas.TryGetValue(player.heroNoumenon, out vPlayers))
                {
                    vPlayers = new List<PlayerData>();
                    m_vMainDatas.Add(player.heroNoumenon, vPlayers);
                }
                vPlayers.Add(player);
            }

            if (player.equips != null)
            {
                foreach (var equip in player.equips)
                {
                    if (!m_vEquipDatas.TryGetValue(equip, out PlayerData pl))
                    {
                        m_vEquipDatas.Add(equip,player);
                    }
                }
            }
        }
        //-------------------------------------------
        public List<PlayerData> GetDataByNoumenons(uint heroNoumenon)
        {
            if (heroNoumenon == 0) return null;
            List<PlayerData> vDatas;
            if(m_vMainDatas.TryGetValue(heroNoumenon, out vDatas))
            {
                return vDatas;
            }
            return null;
        }
        //-------------------------------------------
        public PlayerData GetDataByNoumenon(uint heroNoumenon)
        {
            if (heroNoumenon == 0) return null;
            List<PlayerData> vDatas;
            if (m_vMainDatas.TryGetValue(heroNoumenon, out vDatas) && vDatas.Count>0)
            {
                return vDatas[0];
            }
            return null;
        }

        public PlayerData GetDataByEquip(uint equipId)
        {
            if (equipId <= 0) return null;
            PlayerData player;
            if (m_vEquipDatas.TryGetValue(equipId, out player))
            {
                return player;
            }

            return null;
        }
    }
}
