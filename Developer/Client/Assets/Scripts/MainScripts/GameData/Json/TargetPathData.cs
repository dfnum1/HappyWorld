/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	TargetPathData
作    者:	HappLI
描    述:   路径动画数据
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Data;
using Framework.Core;

namespace TopGame.Data
{
    [System.Serializable]
    public class TargetPathData : BaseData
    {
        public int nID = 0;
        public string strName = "";
        public Vector3 OffsetRotate = Vector3.zero;
        public Vector3 OffsetPosition = Vector3.zero;
        public Vector3 MirrorReference = Vector3.zero;
        [Framework.Data.DisplayNameGUI("最后一帧视角作为游戏视角")]
        [Framework.Data.DisplayEnumBitGUI(typeof(Core.EPathUseEndType))]
        public ushort useEndType = 0;
        public string animClip = "";
        public string timeline = "";
        public bool bCloseGameCamera = false;
        public bool isUIPath = false;
        public Core.SplineCurve.KeyFrame[] keys = null;
        [SerializeField]
        string[] eventDatas;
        [System.NonSerialized]
        public BaseEventParameter[] events;

        public void Mapping(Framework.Module.AFrameworkBase pFramework)
        {
            if (eventDatas != null)
            {
                events = new BaseEventParameter[eventDatas.Length];
                for (int i = 0; i < eventDatas.Length; ++i)
                {
                    events[i] = BaseEventParameter.NewEvent(pFramework, eventDatas[i]);
                }
            }
        }
#if UNITY_EDITOR
        public void Save()
        {
            if (events != null)
            {
                eventDatas = new string[events.Length];
                for (int i = 0; i < events.Length; ++i)
                {
                    if (events[i] != null)
                    {
                        eventDatas[i] = events[i].WriteCmd();
                    }
                }
            }
        }
        public void Copy(TargetPathData pData)
        {
            if (pData == null) return;
            strName = pData.strName;
            OffsetRotate = pData.OffsetRotate;
           OffsetPosition = pData.OffsetPosition;
            MirrorReference = pData.MirrorReference;
            useEndType = pData.useEndType;
            animClip = pData.animClip;
            timeline = pData.timeline;
            bCloseGameCamera = pData.bCloseGameCamera;
            keys = null;
            if (pData.events != null)
            {
                events = new BaseEventParameter[pData.events.Length];
                eventDatas = new string[events.Length];
                for (int i = 0; i < events.Length; ++i)
                {
                    eventDatas[i] = pData.events[i].WriteCmd();
                    events[i] = BaseEventParameter.NewEvent(null, eventDatas[i]);
                }
            }

            if(pData.keys != null)
            {
                keys = new List<Core.SplineCurve.KeyFrame>(pData.keys).ToArray();
            }
        }
#endif
    }

    [DataBinderType("TargetPaths", "int")]
    [FieldMapTable("Mapping(GetFramework())")]
    [System.Serializable]
    public class TargetPathDatas
    {
        public TargetPathData[] datas;
    }
}

