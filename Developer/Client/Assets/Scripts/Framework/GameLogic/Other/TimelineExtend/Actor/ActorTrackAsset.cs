using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TopGame.Timeline
{
    [UnityEngine.Timeline.TrackClipType(typeof(ActorPlayableAsset))]
    [UnityEngine.Timeline.TrackBindingType(typeof(Transform))]
    public class ActorTrackAsset : UnityEngine.Timeline.TrackAsset, IUserTrackAsset
    {
        [System.NonSerialized]
        private Framework.Plugin.AT.IUserData m_pUserPointer = null;
        private Transform m_pBinder;
        private Animator m_pAnimator;

        private PlayableDirector m_Director;
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            m_Director = director;
            Reset(director);
        }
        //------------------------------------------------------
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            if (clip.asset is ActorPlayableAsset)
            {
                (clip.asset as ActorPlayableAsset).Reset(this);
                if(!string.IsNullOrEmpty((clip.asset as ActorPlayableAsset).displayName))
                    clip.displayName = (clip.asset as ActorPlayableAsset).displayName;
            }
            return base.CreatePlayable(graph, gameObject, clip);
        }
        //------------------------------------------------------
        public void Reset(PlayableDirector director)
        {
            m_pBinder = director.GetGenericBinding(this) as Transform;
            foreach (var db in GetClips())
            {
                (db.asset as ActorPlayableAsset).Reset(this);
            }
            if (m_pBinder)
                m_pAnimator = m_pBinder.GetComponent<Animator>();
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
            m_pBinder = transform;
            if (m_pBinder)
                m_pAnimator = m_pBinder.GetComponent<Animator>();
        }
        //------------------------------------------------------
        public Transform GetBinder()
        {
            return m_pBinder;
        }
        //------------------------------------------------------
        public Animator GetAnimator()
        {
            return m_pAnimator;
        }
    }
}