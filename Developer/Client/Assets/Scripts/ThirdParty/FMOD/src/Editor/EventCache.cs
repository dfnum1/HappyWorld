#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity
{
    public class EventCache : ScriptableObject
    {
        public static int CurrentCacheVersion = 10;

        [SerializeField]
        public List<EditorBankRef> EditorBanks;
        [SerializeField]
        public List<EditorEventRef> EditorEvents;
        [SerializeField]
        public List<EditorParamRef> EditorParameters;
        [SerializeField]
        public List<EditorBankRef> MasterBanks;
        [SerializeField]
        public List<EditorBankRef> StringsBanks;
        [SerializeField]
        Int64 cacheTime;
        [SerializeField]
        public int cacheVersion;

        public DateTime CacheTime
        {
            get { return new DateTime(cacheTime); }
            set { cacheTime = value.Ticks; }
        }

        public EventCache()
        {
            EditorBanks = new List<EditorBankRef>();
            EditorEvents = new List<EditorEventRef>();
            EditorParameters = new List<EditorParamRef>();
            MasterBanks = new List<EditorBankRef>();
            StringsBanks = new List<EditorBankRef>();
            cacheTime = 0;
        }

        public static EditorEventRef GetEventRef(string eventPath)
        {
            EventCache eventCache = UnityEditor.AssetDatabase.LoadAssetAtPath(EventManager.CacheAssetFullName, typeof(EventCache)) as EventCache;
            if (eventCache == null) return null;
            for(int i =0; i < eventCache.EditorEvents.Count; ++i)
            {
                if(eventCache.EditorEvents[i].Path.CompareTo(eventPath)==0)
                {
                    return eventCache.EditorEvents[i];
                }
            }
            return null;
        }
    }
}
#endif