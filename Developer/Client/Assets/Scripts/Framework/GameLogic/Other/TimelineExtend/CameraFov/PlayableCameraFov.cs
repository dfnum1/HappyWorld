using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace TopGame.Timeline
{
    public class PlayableCameraFov : PlayableBehaviour
    {
        CameraFovTrackAsset m_pTrackAsset;
        private AnimationCurve m_Curve;
        //------------------------------------------------------
        public void SetCurve(AnimationCurve curve)
        {
            m_Curve = curve;
        }
        //------------------------------------------------------
        public CameraFovTrackAsset GetTrackAsset()
        {
            return m_pTrackAsset;
        }
        //------------------------------------------------------
        public void SetTrackAsset(CameraFovTrackAsset pPointer)
        {
            m_pTrackAsset = pPointer;
        }
        //------------------------------------------------------
        public override void PrepareData(Playable playable, FrameData info)
        {
            base.PrepareData(playable, info);
            Time.timeScale = 1;
        }
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
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            Time.timeScale = 1;
        }
        //------------------------------------------------------
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
#if UNITY_EDITOR
            Time.timeScale = 1;
#endif
        }
        //------------------------------------------------------
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (m_pTrackAsset ==null  || m_Curve == null) return;
#if UNITY_EDITOR
            Camera camera = playerData as Camera;
            if(camera == null)
            {
                if (playerData is Transform)
                {
                    camera = (playerData as Transform).GetComponent<Camera>();
                    if (camera == null)
                        camera = (playerData as Transform).GetComponentInChildren<Camera>();
                }
            }
            if (camera)
            {
                camera.fieldOfView = m_Curve.Evaluate(Mathf.Clamp01((float)(playable.GetTime() / (m_pTrackAsset.end - m_pTrackAsset.start))));
                return;
            }
#endif
            Core.ICameraController ctl = Core.CameraKit.cameraController;
            if (ctl != null)
                ctl.UpdateFov(m_Curve.Evaluate(Mathf.Clamp01((float)(playable.GetTime() / (m_pTrackAsset.end - m_pTrackAsset.start)))));
        }
    }
}