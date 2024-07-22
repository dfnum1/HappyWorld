using UnityEngine;
using System;
using System.Collections.Generic;
using Framework.Core;

namespace TopGame.Core
{
    public class LODNodeManager : Framework.Module.AModule, Framework.Module.IUpdate
    {
        private List<LODRank> m_LodRanks = null;

        private bool m_bForceLowerLOD = false;

        Vector3 m_PreCameraPos = Vector3.zero;
        Vector3 m_PreCameraRot = Vector3.zero;
        //------------------------------------------------------
        protected override void Awake()
        {
            SetLODS(Data.GlobalSetting.LODS);

            m_PreCameraPos = Vector3.zero;
            m_PreCameraRot = Vector3.zero;
        }
        //------------------------------------------------------
        public void SetLODS(LODRank[] lods)
        {
            if (lods != null)
            {
                if (m_LodRanks == null)
                    m_LodRanks = new List<LODRank>(lods.Length);
                else
                    m_LodRanks.Clear();
                for (int i = 0; i < lods.Length; ++i)
                {
                    LODRank rank = new LODRank();
                    rank.index = lods[i].index;
                    rank.distance = lods[i].distance * lods[i].distance;
                    m_LodRanks.Add(rank);
                }
                Framework.Plugin.SortUtility.QuickSortDown(ref m_LodRanks);
            }
            else
                m_LodRanks = null;
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            m_LodRanks = null;
        }
        //------------------------------------------------------
        public void Update(float fFrame)
        {
            if (CameraKit.cameraController == null) return;
            Vector3 curPos = CameraKit.MainCameraPosition;
            Vector3 curAngle = CameraKit.MainCameraEulerAngle;
            if(!Framework.Core.CommonUtility.Equal(curPos, m_PreCameraPos, 0.1f) || !Framework.Core.CommonUtility.Equal(curAngle, m_PreCameraRot, 0.1f))
            {
                UpdateLOD();
                m_PreCameraPos = curPos;
                m_PreCameraRot = curAngle;
            }
        }
        //------------------------------------------------------
        public void ForceLowerLOD(bool bForce)
        {
            if(m_bForceLowerLOD!= bForce)
            {
                m_bForceLowerLOD = bForce;

                if(m_bForceLowerLOD)
                {
                    UpdateLOD();
                }
            }
        }
        //------------------------------------------------------
        int GetLodIndexFromDistance(float distance)
        {
            if (m_LodRanks == null || m_LodRanks.Count<=0)
                return 0;

            if (m_bForceLowerLOD)
            {
                LODRank nodeInfo = m_LodRanks[m_LodRanks.Count - 1];
                return nodeInfo.index;
            }
            for(int i = 0; i < m_LodRanks.Count; ++i)
            {
                if (distance >= m_LodRanks[i].distance) return m_LodRanks[i].index;
            }
            return 0;
        }
        //------------------------------------------------------
        void UpdateLOD()
        {
            LODNode node = LODNode.Header as LODNode;

            float sqrDist;
            for (; node != null; node = node.Next as LODNode)
            {
                if (!node.isActiveAndEnabled) continue;
                sqrDist = (node.GetPosition() - m_PreCameraPos).sqrMagnitude;
                node.OnUpdateLOD(GetLodIndexFromDistance(sqrDist));
            }
        }
    }
}