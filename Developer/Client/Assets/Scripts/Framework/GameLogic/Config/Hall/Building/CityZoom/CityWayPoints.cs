/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	CityWayPoints
作    者:	HappLI
描    述:	主城寻路驻点
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.Core;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TopGame.Logic
{
    public class CityWayPoints : MonoBehaviour
    {
        public static Action<CityWayPoints> OnRefreshWayPoint;
        [System.Serializable]
        public struct WayPoint
        {
            public byte group;
            public Vector3 start;
            public Vector3 end;
            // public Vector3 position;
      //      public Vector3 eulerAngle;
        }
        public List<WayPoint> wayPoints;
        public bool bAutoAdd = true;

        static CityWayPoints ms_Instnace = null;
        //------------------------------------------------------
        private void Awake()
        {
            ms_Instnace = this;
        }
        //------------------------------------------------------
        public void OnEnable()
        {
            if (bAutoAdd)
            {
                if(OnRefreshWayPoint!=null) OnRefreshWayPoint(this);
            }
        }
        //------------------------------------------------------
        private void Start()
        {
            if(bAutoAdd)
            {
                if (OnRefreshWayPoint != null) OnRefreshWayPoint(this);
            }
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_Instnace = null;
        }
        
    }
}

