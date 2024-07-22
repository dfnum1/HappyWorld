using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace TopGame.Timeline
{
    public class PlayableTimeScaler : PlayableBehaviour
    {
        TimeScalerTrackAsset m_pTrackAsset;
        private AnimationCurve m_Curve;
        private float m_fOriginalTimeScale = 1;
        //------------------------------------------------------
        public void SetCurve(AnimationCurve curve)
        {
            m_Curve = curve;
        }
        //------------------------------------------------------
        public TimeScalerTrackAsset GetTrackAsset()
        {
            return m_pTrackAsset;
        }
        //------------------------------------------------------
        public void SetTrackAsset(TimeScalerTrackAsset pPointer)
        {
            m_pTrackAsset = pPointer;
        }
        //------------------------------------------------------
        public override void PrepareData(Playable playable, FrameData info)
        {
            base.PrepareData(playable, info);
            m_fOriginalTimeScale = Time.timeScale;
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public override void OnGraphStart(Playable playable)
        {
            Time.timeScale = 1;
        }
        //------------------------------------------------------
        public override void OnGraphStop(Playable playable)
        {
            Time.timeScale = 1;
        }
        //------------------------------------------------------
        public override void OnPlayableCreate(Playable playable)
        {
            Time.timeScale = 1;
        }
        //------------------------------------------------------
        public override void OnPlayableDestroy(Playable playable)
        {
            Time.timeScale = 1;
        }
        //------------------------------------------------------
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            Time.timeScale = 1;
        }
#endif
        //------------------------------------------------------
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            m_fOriginalTimeScale = Time.timeScale;
        }
        //------------------------------------------------------
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (m_pTrackAsset ==null  || m_Curve == null) return;
            double time = playable.GetTime();
            if (time <= 0)
                return;
            float toScale = m_Curve.Evaluate(Mathf.Clamp01((float)(time / playable.GetDuration())));
            Time.timeScale = toScale;// Mathf.Lerp(m_fOriginalTimeScale, toScale, 0.033f);
        }
    }
}