/********************************************************************
生成日期:	2020-6-16
类    名: 	BuildingTransform
作    者:	Happli
描    述:	建筑配置挂点
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
namespace TopGame.Logic
{
    public class BuildingTransform : ABuildingInstance
    {
        public BuildingCrowdInstance ownerCrowd;
        public float upgrageScaler = 1;
        [Framework.Data.StringViewGUI(typeof(GameObject))]
        [Framework.Data.DisplayNameGUI("升级脚手架模型")]
        public string upgrageModel = "";
        [Framework.Data.DisplayNameGUI("升级时隐藏的物体")]
        public List<GameObject> vHideGoWithUpgrade;
        //-------------------------------------------
        public override BuildingCrowdInstance GetOwnerCrowd()
        {
            return ownerCrowd;
        }
    }
}