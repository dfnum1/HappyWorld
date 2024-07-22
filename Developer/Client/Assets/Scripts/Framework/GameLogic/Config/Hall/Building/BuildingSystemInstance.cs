using System.Collections.Generic;
using TopGame.UI;
using UnityEngine;
namespace TopGame.Logic
{
    public enum TipCondition
    {
        DB,
        ItemCount,
    }
   
    [System.Serializable]
    public class TipParam
    {
        public TipCondition condition;
        public int arg1;
        public int arg2;
        [Framework.Data.StringViewGUI(typeof(Texture2D))]
        public string iconStr;
    }

    public class BuildingSystemInstance : ABuildingInstance
    {
#if UNITY_EDITOR
        [Framework.ED.DisplayDrawType("TopGame.UI.EUIType")]
#endif
        public int bindUI = 0;
        public BuildingCrowdInstance ownerCrowd;
        [HideInInspector]
        public List<TipParam> tipParam;
        //-------------------------------------------
        public override BuildingCrowdInstance GetOwnerCrowd()
        {
            return ownerCrowd;
        }
    }
}