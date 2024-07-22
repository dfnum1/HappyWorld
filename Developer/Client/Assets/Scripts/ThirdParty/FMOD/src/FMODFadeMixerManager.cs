using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity
{
    public class FMODFadeMixerManager
    {
        struct MixerChannel
        {
            public FMOD.Studio.EventInstance eventInstance;
            public float toFade;
            public float fromFade;
            public float time;
        }
        class GroupChannel
        {
            public string channel;
            public List<MixerChannel> vMixers;
        }
        List<GroupChannel> m_vMixGroup = null;
        internal void AddFadeMixer(string group, FMOD.Studio.EventInstance eventInstance, float toFade)
        {
            if (string.IsNullOrEmpty(group) || !eventInstance.isValid()) return;
            if (m_vMixGroup == null) m_vMixGroup = new List<GroupChannel>(2);

            GroupChannel channel = null;
            for (int i = 0; i < m_vMixGroup.Count; ++i)
            {
                if (group.CompareTo(m_vMixGroup[i].channel) == 0)
                {
                    channel = m_vMixGroup[i];
                    break;
                }
            }
            if (channel == null)
            {
                channel = new GroupChannel();
                channel.vMixers = new List<MixerChannel>(2);
                channel.channel = group;
                m_vMixGroup.Add(channel);
            }
            for(int i =0; i < channel.vMixers.Count; ++i)
            {
                if(channel.vMixers[i].eventInstance.handle == eventInstance.handle)
                {
                    var tempMix = channel.vMixers[i];
                    tempMix.toFade = toFade;
                    channel.vMixers[i] = tempMix;
                    return;
                }
            }
            MixerChannel mixGroup = new MixerChannel();
            mixGroup.eventInstance = eventInstance;
            mixGroup.toFade = toFade;
            eventInstance.getVolume(out mixGroup.fromFade);
            mixGroup.time = Time.realtimeSinceStartup;
            channel.vMixers.Add(mixGroup);
        }
        //------------------------------------------------------
        internal void Update()
        {
            if (m_vMixGroup == null) return;
            float time = Time.realtimeSinceStartup;
            GroupChannel channel;
            for (int i = 0; i < m_vMixGroup.Count; ++i)
            {
                channel = m_vMixGroup[i];
                if (channel.vMixers.Count <= 0)
                    continue;

                for (int j = 0; j < channel.vMixers.Count;)
                {
                    MixerChannel mixer = channel.vMixers[j];
                    if (!mixer.eventInstance.isValid())
                    {
                        channel.vMixers.RemoveAt(j);
                        continue;
                    }
                    mixer.fromFade = Mathf.Lerp(mixer.fromFade, mixer.toFade, Time.fixedDeltaTime * 10);
                    mixer.eventInstance.setVolume(mixer.fromFade);
                    bool bStoped = false;
                    if (Mathf.Abs(mixer.fromFade - mixer.toFade) <= 0.01f)
                    {
                        mixer.eventInstance.setVolume(mixer.toFade);
                        if (mixer.toFade <= 0.01f)
                        {
                            mixer.eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                            mixer.eventInstance.release();
                            mixer.eventInstance.clearHandle();
                            bStoped = true;
                        }
                    }
                    if (bStoped)
                    {
                        channel.vMixers.RemoveAt(j);
                        continue;
                    }
                    channel.vMixers[j] = mixer;
                    ++j;
                }
            }
        }
    }
}