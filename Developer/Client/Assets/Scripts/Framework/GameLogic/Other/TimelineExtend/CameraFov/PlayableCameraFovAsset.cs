using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
namespace TopGame.Timeline
{
    public class PlayableCameraFovAsset : PlayableAsset
    {
        PlayableCameraFov m_pBehaviour = null;
        public AnimationCurve mCurve;
        CameraFovTrackAsset m_pTrackAssets;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<PlayableCameraFov>.Create(graph);
            m_pBehaviour = playable.GetBehaviour();
            m_pBehaviour.SetCurve(mCurve);
            if (m_pBehaviour!=null) m_pBehaviour.SetTrackAsset(m_pTrackAssets);
            return playable;
        }
        //------------------------------------------------------
        public void Reset(CameraFovTrackAsset pTrackAssets)
        {
            m_pTrackAssets = pTrackAssets;
            if (m_pBehaviour != null) m_pBehaviour.SetTrackAsset(m_pTrackAssets);
        }
    }
}