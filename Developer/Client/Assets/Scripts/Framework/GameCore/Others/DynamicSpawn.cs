/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	DynamicSpawn
作    者:	HappLI
描    述:	DynamicSpawn
*********************************************************************/
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

namespace TopGame.Core
{
    public class DynamicSpawn : MonoBehaviour
    {
        public GameObject[] Prefabs;

        private List<GameObject> m_vInstnace = null;
        void Awake()
        {
            if(Prefabs != null && Prefabs.Length>0)
            {
                m_vInstnace = new List<GameObject>(Prefabs.Length);
                for (int i = 0; i < Prefabs.Length; ++i)
                {
                    if(Prefabs[i])
                    {
                        GameObject pIns = GameObject.Instantiate(Prefabs[i]);
                        pIns.name = Prefabs[i].name;
                        m_vInstnace.Add(pIns);
                    }
                }
            }
        }
        //------------------------------------------------------
        void OnDestroy()
        {
            if (m_vInstnace == null) return;
            for(int i = 0; i < m_vInstnace.Count; ++i)
            {
                GameObject.Destroy(m_vInstnace[i]);
            }
            m_vInstnace = null;
        }
    }
}