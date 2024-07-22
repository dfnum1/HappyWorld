using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
namespace TopGame.Timeline
{
    public class PlayableTimeScalerAsset : PlayableAsset
    {
        PlayableTimeScaler m_pBehaviour = null;
        public AnimationCurve mCurve;
        TimeScalerTrackAsset m_pTrackAssets;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
               var playable = ScriptPlayable<PlayableTimeScaler>.Create(graph);
            m_pBehaviour = playable.GetBehaviour();
            m_pBehaviour.SetCurve(mCurve);
            if (m_pBehaviour!=null) m_pBehaviour.SetTrackAsset(m_pTrackAssets);
            return playable;
        }
        //------------------------------------------------------
        public void Reset(TimeScalerTrackAsset pTrackAssets)
        {
            m_pTrackAssets = pTrackAssets;
            if (m_pBehaviour != null) m_pBehaviour.SetTrackAsset(m_pTrackAssets);
        }
    }
}