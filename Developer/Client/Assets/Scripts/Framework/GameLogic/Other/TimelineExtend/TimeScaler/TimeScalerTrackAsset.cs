using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TopGame.Timeline
{
    [UnityEngine.Timeline.TrackClipType(typeof(PlayableTimeScalerAsset))]
    [UnityEngine.Timeline.TrackBindingType(typeof(Transform))]
    public class TimeScalerTrackAsset : UnityEngine.Timeline.TrackAsset
    {
        //------------------------------------------------------
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            if (clip.asset is PlayableTimeScalerAsset)
            {
                (clip.asset as PlayableTimeScalerAsset).Reset(this);
            }
            return base.CreatePlayable(graph, gameObject, clip);
        }
    }
}