/********************************************************************
生成日期:	2020-6-16
类    名: 	BuildingAble
作    者:	Happli
描    述:	建筑实体
*********************************************************************/
using UnityEngine;
namespace TopGame.Logic
{
    public class BuildingAble : Framework.Core.AInstanceAble
    {
        public float cameraLerp = 0;
        public Vector3 cameraPosition = Vector3.zero;
        public Vector3 cameraLookAt = Vector3.zero;

        public float ModelMarkerTopOffset = 0;
        public float ModelMarkerBottomOffset = 0;
        public float ModelMarkerZOffset = 0;
        public float ModelMarkerXOffset = 0;
        public float ModelTopMarkerZOffset = 0;
        public float ModelTopMarkerXOffset = 0;
        public Vector3[] ModelSurroundPoint;
        public Vector3[] ModelSurroundPathPoint;

#if UNITY_EDITOR
        public Framework.Core.VariablePoolAble runtimeBuilding;
#endif
    }
}