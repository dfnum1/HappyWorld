using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TopGame.Timeline
{
    [UnityEngine.Timeline.TrackClipType(typeof(PlayableCameraFovAsset))]
    [UnityEngine.Timeline.TrackBindingType(typeof(Transform))]
    public class CameraFovTrackAsset : UnityEngine.Timeline.TrackAsset
    {
        //------------------------------------------------------
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            if (clip.asset is PlayableCameraFovAsset)
            {
                (clip.asset as PlayableCameraFovAsset).Reset(this);
            }
            return base.CreatePlayable(graph, gameObject, clip);
        }
    }
}