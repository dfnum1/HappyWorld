/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	AnimPathData
作    者:	HappLI
描    述:   路径动画数据
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopGame.Core;
using Framework.Core;
using Framework.Data;

namespace TopGame.Data
{
    [System.Serializable]
    public class EventData : BaseData
    {
        public int nID = 0;
#if UNITY_EDITOR
        public string desc;
#endif
        [SerializeField]
        string[] eventDatas;
        public bool groupRandom = false;

        [System.NonSerialized]
        public List<BaseEventParameter> events;

        public void Mapping(Framework.Module.AFrameworkBase pFramework)
        {
            if (eventDatas != null)
            {
                events = new List<BaseEventParameter>(eventDatas.Length);
                for (int i = 0; i < eventDatas.Length; ++i)
                {
                    BaseEventParameter evt = BaseEventParameter.NewEvent(pFramework, eventDatas[i]);
                    if (evt != null) events.Add(evt);
                }
            }
        }

        public BaseEventParameter GetRandomGroup(Framework.Module.AFramework pFramework)
        {
            if (!groupRandom || events ==null || events.Count<=0) return null;

            return events[pFramework.GetRamdom(0, events.Count)];
        }

        public void PreloadCollect(Framework.Module.AFramework pFramework,List<string> vFiles, HashSet<string> vAssets)
        {
            if (events != null)
            {
                for (int i = 0; i < events.Count; ++i)
                {
                    if (events[i] != null) events[i].CollectPreload(pFramework,vFiles, vAssets);
                }
            }
        }

#if UNITY_EDITOR
        public void Save()
        {
            if(events!=null)
            {
                eventDatas = new string[events.Count];
                for (int i =0; i < events.Count; ++i)
                {
                    eventDatas[i] = events[i].WriteCmd();
                }
            }
        }
        public void Copy(EventData pData)
        {
            if (pData == null) return;
            desc = pData.desc;
            groupRandom = pData.groupRandom;
            if(pData.events != null)
            {
                events = new List<BaseEventParameter>(pData.events.Count);
                eventDatas = new string[events.Count];
                for (int i = 0; i < events.Count; ++i)
                {
                    eventDatas[i] = pData.events[i].WriteCmd();
                    events.Add( BaseEventParameter.NewEvent(null, eventDatas[i]) );
                }
            }
        }
#endif
    }

    [DataBinderType("EventDatas", "int")]
    [FieldMapTable("Mapping(GetFramework())")]
    [System.Serializable]
    public class EventDatas
    {
        public EventData[] datas;
    }
}

