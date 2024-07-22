using Framework.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TopGame.Timeline
{
    [UnityEngine.Timeline.TrackClipType(typeof(EventPlayableAsset))]
    [UnityEngine.Timeline.TrackBindingType(typeof(Transform))]
    public class EventTrackAsset : UnityEngine.Timeline.TrackAsset, IUserTrackAsset, IInstanceSpawner
    {
        public Transform m_pBind;

        private Animator m_pAnimator;

        [System.NonSerialized]
        private Framework.Plugin.AT.IUserData m_pUserPointer = null;

        PlayableDirector m_Director;

        private List<EventEmitter> m_vEvents = null;
        List<AInstanceAble> m_vInstances = null;

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            Reset(director);
        }
        //------------------------------------------------------
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            DestortInstance();
            if (clip.asset is EventPlayableAsset)
            {
                (clip.asset as EventPlayableAsset).pTrackAssets = this;
            }
            if(m_vEvents!=null) m_vEvents.Clear();
            int markCount = GetMarkerCount();
            for(int i =0; i < markCount; ++i)
            {
                IMarker marker = GetMarker(i);
                if(marker is EventEmitter)
                {
                    (marker as EventEmitter).InitEvent();
                    if (m_vEvents == null) m_vEvents = new List<EventEmitter>(markCount);
                    m_vEvents.Add((EventEmitter)marker);
                }
            }
            return base.CreatePlayable(graph, gameObject, clip);
        }
        //------------------------------------------------------
        public PlayableDirector GetDirector()
        {
            return m_Director;
        }
        //------------------------------------------------------
        public void SetUserPointer(Framework.Plugin.AT.IUserData pBehavior)
        {
            m_pUserPointer = pBehavior;
        }
        //------------------------------------------------------
        public Framework.Plugin.AT.IUserData GetUserPointer()
        {
            return m_pUserPointer;
        }
        //------------------------------------------------------
        public void SetBinder(Transform transform)
        {
            if (transform == null) return;
            m_pBind = transform;
            if (m_pBind)
                m_pAnimator = m_pBind.GetComponent<Animator>();
        }
        //------------------------------------------------------
        public Transform GetBinder()
        {
            return m_pBind;
        }
        //------------------------------------------------------
        public Animator GetAnimator()
        {
            return m_pAnimator;
        }
        //------------------------------------------------------
        public void Reset(PlayableDirector director)
        {
            DestortInstance();
            m_Director = director;
            m_pBind = director.GetGenericBinding(this) as Transform;
            foreach (var db in GetClips())
            {
                (db.asset as EventPlayableAsset).pTrackAssets = this;
            }
            if (m_pBind)
                m_pAnimator = m_pBind.GetComponent<Animator>();

            if (m_vEvents != null) m_vEvents.Clear();
            int markCount = GetMarkerCount();
            for (int i = 0; i < markCount; ++i)
            {
                IMarker marker = GetMarker(i);
                if (marker is EventEmitter)
                {
                    (marker as EventEmitter).InitEvent();
                    if (m_vEvents == null) m_vEvents = new List<EventEmitter>(markCount);
                    m_vEvents.Add((EventEmitter)marker);
                }
            }
        }
        //------------------------------------------------------
        public void ProcessFrame(Playable playable, FrameData frameData)
        {
            if (m_vEvents == null || m_pBind == null) return;
            double time = playable.GetTime();
            for(int i =0; i < m_vEvents.Count;)
            {
                if (m_vEvents[i].time <= time)
                {
                    m_vEvents[i].Emitter(m_pBind.gameObject, this);
                    m_vEvents.RemoveAt(i);
                }
                else ++i;
            }
        }
        //------------------------------------------------------
        public void Spawn(string strFile, bool bAbs, Vector3 offset, Vector3 euler, Transform pParent = null)
        {
            InstanceOperiaon pCallback = FileSystemUtil.SpawnInstance(strFile, true);
            if (pCallback != null)
            {
                pCallback.OnCallback = OnSpawnCallback;
                if (bAbs)
                {
                    pCallback.pByParent = RootsHandler.ScenesRoot;
                    pCallback.userData0 = new Variable3() { floatVal0 = offset.x, floatVal1 = offset.y, floatVal2 = offset.z };
                    pCallback.userData1 = new Variable3() { floatVal0 = euler.x, floatVal1 = euler.y, floatVal2 = euler.z };
                    pCallback.userData2 = new Variable1() { boolVal = bAbs };
                }
                else
                {
                    pCallback.pByParent = pParent;
                    pCallback.userData0 = new Variable3() { floatVal0 = offset.x, floatVal1 = offset.y, floatVal2 = offset.z };
                    pCallback.userData1 = new Variable3() { floatVal0 = euler.x, floatVal1 = euler.y, floatVal2 = euler.z };
                    pCallback.userData2 = new Variable1() { boolVal = bAbs };
                }
            }
        }
        //------------------------------------------------------
        void OnSpawnCallback(InstanceOperiaon pCallback)
        {
            if (pCallback.pPoolAble != null)
            {
                Vector3 pos = ((Variable3)pCallback.userData0).ToVector3();
                Vector3 euler = ((Variable3)pCallback.userData1).ToVector3();
                bool bAbs = ((Variable1)pCallback.userData2).boolVal;
                pCallback.pPoolAble.SetEulerAngle(pos, bAbs);
                pCallback.pPoolAble.SetEulerAngle(euler, bAbs);
                if (m_vInstances == null) m_vInstances = new List<AInstanceAble>();
                m_vInstances.Add(pCallback.pPoolAble);
            }
        }
        //------------------------------------------------------
        void DestortInstance()
        {
            if (m_vInstances != null)
            {
                for (int i = 0; i < m_vInstances.Count; ++i)
                {
                    m_vInstances[i].RecyleDestroy();
                }
                m_vInstances.Clear();
            }
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            DestortInstance();
        }
    }
}