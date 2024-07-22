using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TopGame.Timeline
{
    public interface IUserTrackAsset
    {
        void Reset(PlayableDirector director);
        void SetUserPointer(Framework.Plugin.AT.IUserData pBehavior);
        Framework.Plugin.AT.IUserData GetUserPointer();

        PlayableDirector GetDirector();

        void SetBinder(Transform transform);
        Transform GetBinder();
        Animator GetAnimator();
    }
}