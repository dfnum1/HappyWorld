/********************************************************************
生成日期:	2020-6-16
类    名: 	BuildingCrowdAble
作    者:	Happli
描    述:	建筑群实体对象
*********************************************************************/
using System.Collections.Generic;

using UnityEngine;

namespace TopGame.Logic
{
    public class BuildingCrowdInstance : ABuildingInstance
    {
        public List<Vector3> polygonRegion = new List<Vector3>();
        [SerializeField]
        List<ABuildingInstance> m_buildingNodes = new List<ABuildingInstance>();
        public List<ABuildingInstance> buildingNodes
        {
            get { return m_buildingNodes; }
            set
            {
                m_buildingNodes = value;
            }
        }
    }
}