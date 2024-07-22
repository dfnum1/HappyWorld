using Framework.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TopGame.Timeline
{
    [Serializable]
    [CustomStyle("EventEmitter")]
    public class EventEmitter : Marker, INotification, INotificationOptionProvider
    {
        [SerializeField]
        string[] Events = null;

        private List<BaseEventParameter> m_vEvents = null;
        public List<BaseEventParameter> GetEvents()
        {
#if UNITY_EDITOR
            if (m_vEvents == null)
            {
                InitEvent();
                if(m_vEvents == null) m_vEvents = new List<BaseEventParameter>();
            }
#endif
            return m_vEvents;
        }
#if UNITY_EDITOR
        public void SyncEvents()
        {
            Events = new string[m_vEvents.Count];
            for(int i = 0; i < m_vEvents.Count; ++i)
            {
                Events[i] = m_vEvents[i].ToString();
            }
        }
#endif
        public bool emitOnce { get; set; }
        public PropertyName id
        {
            get { return new PropertyName("EventEmitter"); }
        }

        public override void OnInitialize(TrackAsset aPent)
        {
            base.OnInitialize(aPent);
            InitEvent();
        }

        public void InitEvent()
        {
            if (m_vEvents != null) return;
            if (Events != null && Events.Length > 0)
            {
                m_vEvents = new List<BaseEventParameter>(Events.Length);
                for (int i = 0; i < Events.Length; ++i)
                {
                    BaseEventParameter evt = BaseEventParameter.NewEvent(Framework.Module.ModuleManager.mainModule, Events[i]);
                    if (evt == null) continue;
                    m_vEvents.Add(evt);
                }
            }
        }

        NotificationFlags INotificationOptionProvider.flags
        {
            get
            {
                return NotificationFlags.TriggerOnce;// (emitOnce ? NotificationFlags.TriggerOnce : default(NotificationFlags));
            }
        }

        public void Emitter(GameObject pBinder, IInstanceSpawner spanwer)
        {
            Framework.Plugin.AT.IUserData pUserPointer = null;
            if (this.parent is IUserTrackAsset)
            {
                IUserTrackAsset tackAsset = this.parent as IUserTrackAsset;
                pUserPointer = tackAsset.GetUserPointer();
            }
            if (Framework.Module.ModuleManager.mainModule != null)
            {
                Framework.Core.AFrameworkModule mainFramework = Framework.Module.ModuleManager.GetMainFramework<Framework.Core.AFrameworkModule>();
                if (mainFramework != null)
                {
                    mainFramework.eventSystem.Begin();
                    mainFramework.eventSystem.InstanceSpawner = spanwer;
                    mainFramework.eventSystem.ATuserData = pUserPointer;
                    mainFramework.eventSystem.pGameObject = pBinder;
                    mainFramework.eventSystem.TriggerEventPos = mainFramework.GetPlayerPosition();
                    mainFramework.eventSystem.TriggerEventRealPos = mainFramework.GetPlayerPosition();
                    mainFramework.OnTriggerEvent(GetEvents(), false);
                    mainFramework.eventSystem.End();
                }
                else
                {
#if UNITY_EDITOR
                    Vector3 transPos = Vector3.zero;
                    if (pBinder) transPos = pBinder.transform.position;
                    List<BaseEventParameter> vEvents = GetEvents();
                    for (int i = 0; i < GetEvents().Count; ++i)
                    {
                        if (vEvents[i].GetEventType() == EEventType.Sound)
                        {
                            SoundEventParameter soundParam = vEvents[i] as SoundEventParameter;
                            if (soundParam.soundType == ESoundType.Effect || soundParam.soundType == ESoundType.BG)
                            {
                                if (soundParam.use3D)
                                {
                                    if (pBinder) Framework.Core.AudioUtil.Play3DEffectVolume(soundParam.strFile, 1, pBinder.transform);
                                    else Framework.Core.AudioUtil.PlayEffectVolume(soundParam.strFile, 1, transPos);
                                }
                                else
                                    Framework.Core.AudioUtil.PlayEffect(soundParam.strFile);
                            }
                            else if (soundParam.soundType == ESoundType.StopEffect)
                            {
                                Framework.Core.AudioUtil.StopEffect(soundParam.strFile);
                            }
                            else if (soundParam.soundType == ESoundType.StopBG)
                            {
                                Framework.Core.AudioUtil.StopBG(soundParam.strFile);
                            }
                        }
                    }
#endif
                }
            }
        }
    }
}