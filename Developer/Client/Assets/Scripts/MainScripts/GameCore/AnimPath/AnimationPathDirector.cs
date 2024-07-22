/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	animation 播放器
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using Framework.Core;

namespace TopGame.Core
{
    public class AnimationPathDirector
    {
        public ushort bAnimPathUseEnd = 0;
        private bool m_bAnimPathLookAt = false;
        private Vector3 m_AnimPathLookAtPos = Vector3.zero;
        private SplineCurve m_pCurve = null;
        private Asset m_pAsset = null;
        private Vector3 m_MirrorReference = Vector3.zero;
        private Transform m_LookatToTarget = null;
        private AnimationCurve m_pSpeedCurve = null;

        private float m_fEnterTweenDelta = 0f;
        public float fTweenTime
        {
            get { return m_fEnterTweenDelta; }
        }
        private bool m_bPlayEnterTween = false;
        private bool m_bEnterTweenTime = false;

        Transform m_pTrigger;
        Transform m_pTarget = null;
        Transform m_pFollow = null;
        Camera m_pCamera = null;
        public bool bMirror = false;

        public bool bCanSkip = false;
        public float fSkipToTime = 0;

        Vector3 m_OffsetEulerAngle = Vector3.zero;
        Vector3 m_OffsetPosition = Vector3.zero;

        private List<IPlayableCallback> m_vCallback;
        AnimPathData m_pAnimPath = null;

        public Vector3 curLookat { get; set; }
        public Vector3 curPosition { get; set; }
        public Vector3 curEulerAngle { get; set; }
        public float curFov { get; set; }
        //---------------------------------------------
        public System.Action<Vector3, Vector3, float, bool> OnPathEnd;
        public void SetPlayAble(AnimPathData aniPath)
        {
            m_pAnimPath = aniPath;
        }
        //---------------------------------------------
        public void Clear()
        {
            m_pTrigger = null;
            bMirror = false;
            m_pTarget = null;
            m_pCamera = null;
            m_pFollow = null;
            m_bEnterTweenTime = false;
            m_bPlayEnterTween = false;
            if (m_pCurve != null) m_pCurve.Clear();
            bAnimPathUseEnd = 0;
            m_bAnimPathLookAt = false;
            m_MirrorReference = Vector3.zero;
            m_LookatToTarget = null;
            m_pSpeedCurve = null;
            m_fEnterTweenDelta = 0;

            bCanSkip = false;
            fSkipToTime = 0;
            m_pAsset = null;

            m_OffsetEulerAngle = Vector3.zero;
            m_OffsetPosition = Vector3.zero;

            if (m_vCallback != null) m_vCallback.Clear();
            m_pAnimPath = null;
        }
        //---------------------------------------------
        void SetPosition(Vector3 vPos)
        {
            if (m_pFollow != null)
                vPos = m_pFollow.position;
            curPosition = vPos+ m_OffsetPosition;
            if(m_vCallback != null)
            {
                for(int i = 0; i < m_vCallback.Count;)
                {
                    if (m_vCallback[i].OnTimelineCallback(m_pAnimPath, EPlayableCallbackType.Position, new Variable3(curPosition)))
                        ++i;
                    else m_vCallback.RemoveAt(i);
                }
            }
            if (m_pTarget != null)
            {
                m_pTarget.position = curPosition;
                return;
            }
        }
        //---------------------------------------------
        void SetEulerRotation(Vector3 vEuler)
        {
            if (m_pFollow != null)
                vEuler = m_pFollow.eulerAngles;
            curEulerAngle = vEuler + m_OffsetEulerAngle;
            if (m_vCallback != null)
            {
                for (int i = 0; i < m_vCallback.Count;)
                {
                    if (m_vCallback[i].OnTimelineCallback(m_pAnimPath, EPlayableCallbackType.EuleraAngle, new Variable3(curEulerAngle)))
                        ++i;
                    else m_vCallback.RemoveAt(i);
                }
            }
            if (m_pTarget != null)
            {
                m_pTarget.eulerAngles = curEulerAngle;
                return;
            }
        }
        //---------------------------------------------
        void SetFov(float fFov)
        {
            curFov = fFov;
            if (m_vCallback != null)
            {
                for (int i = 0; i < m_vCallback.Count;)
                {
                    if (m_vCallback[i].OnTimelineCallback(m_pAnimPath, EPlayableCallbackType.FOV, new Variable3() { floatVal0 = curFov })) ++i;
                    else m_vCallback.RemoveAt(i);
                }
            }
        }
        //---------------------------------------------
        void SetLookAtPos(Vector3 vPos)
        {
            curLookat = vPos;
            if (m_vCallback != null)
            {
                for (int i = 0; i < m_vCallback.Count;)
                {
                    if (m_vCallback[i].OnTimelineCallback(m_pAnimPath, EPlayableCallbackType.Lookat, new Variable3(curLookat))) ++i;
                    else m_vCallback.RemoveAt(i);
                }
            }
            if (m_pTarget)
            {
                m_pTarget.LookAt(vPos);
                return;
            }
        }
        //---------------------------------------------
        public bool HasPlaying(int guid)
        {
            return false;
        }
        //---------------------------------------------
        public void SetCamera(Camera pCamera)
        {
            m_pCamera = pCamera;
        }
        //---------------------------------------------
        public Camera GetCamera()
        {
            return m_pCamera;
        }
        //---------------------------------------------
        public void SetFollow(Transform pFollow)
        {
            m_pFollow = pFollow;
        }
        //---------------------------------------------
        public Transform GetFollow()
        {
            return m_pFollow;
        }
        //---------------------------------------------
        public Transform GetTarget()
        {
            return m_pTarget;
        }
        //---------------------------------------------
        public void SetTraget(Transform pTarget)
        {
            m_pTarget = pTarget;
        }
        //---------------------------------------------
        public Transform GetTrigger()
        {
            return m_pTrigger;
        }
        //---------------------------------------------
        public void SetTrigger(Transform pTarget)
        {
            m_pTrigger = pTarget;
        }
        //---------------------------------------------
        public void Register(IPlayableCallback callback)
        {
            if (m_vCallback == null) m_vCallback = new List<IPlayableCallback>(2);
            if (!m_vCallback.Contains(callback))
                m_vCallback.Add(callback);
        }
        //---------------------------------------------
        public void UnRegister(IPlayableCallback callback)
        {
            if (m_vCallback == null) return;
            m_vCallback.Remove(callback);
        }
        //---------------------------------------------
        public SplineCurve.KeyFrame GetSplineLasyKey()
        {
            if (m_pCurve != null && m_pCurve.IsValid())
                return m_pCurve.GetLastKey();
            return SplineCurve.KeyFrame.Epsilon;
        }
        //---------------------------------------------
        public void SetOffsetPosition(Vector3 pos)
        {
            m_OffsetPosition = pos;
        }
        //---------------------------------------------
        public void SetOffsetEulerAngle(Vector3 eulerAngle)
        {
            m_OffsetEulerAngle = eulerAngle;
        }
        //------------------------------------------------------
        public Vector3 GetOffsetPosition()
        {
            return m_OffsetPosition;
        }
        //------------------------------------------------------
        public Vector3 GetOffsetEulerAngle()
        {
            return m_OffsetEulerAngle;
        }
        //---------------------------------------------
        public KeyFrame GetLasyKey()
        {
            if (m_pCurve != null && m_pCurve.IsValid())
            {
                KeyFrame kf = new KeyFrame();
                kf.time = m_pCurve.GetLastKey().time;
                kf.position = m_pCurve.GetLastKey().position;
                kf.eulerAngle = m_pCurve.GetLastKey().eulerAngle;
                kf.fov = m_pCurve.GetLastKey().fov;
                return kf;
            }
            return KeyFrame.Epsilon;
        }
        //------------------------------------------------------
        public float GetDuration()
        {
            if (m_pCurve == null || !m_pCurve.IsValid()) return 0;
            return m_pCurve.GetMaxTime();
        }
        //---------------------------------------------
        public List<BaseEventParameter> GetEvents()
        {
            if (m_pCurve != null && m_pCurve.IsValid())
                return m_pCurve.events;
            return null;
        }
        //-------------------------------------------
        public void TriggerEvent()
        {

        }
        //-------------------------------------------
        private bool PlayEnterTween(float fFrameTime, bool bEvent = true)
        {
            if (m_pCurve != null && m_pCurve.IsValid())
            {
                if (!m_bPlayEnterTween) return true;
                Transform controller = m_pTarget;
                float speedTween = 1f;
                float maxTime = GetDuration();
                if (maxTime > 0.01f)
                {
                    if (m_pSpeedCurve != null) speedTween = m_pSpeedCurve.Evaluate(m_fEnterTweenDelta / maxTime);
                }
                m_fEnterTweenDelta += fFrameTime * speedTween;
                Vector3 pos = Vector3.zero, rot = Vector3.zero, lookAtPos = Vector3.zero;
                Vector3 LookatPos = Vector3.zero;
                float fov = 0;
                bool bLookAt = false;
                if (m_pCurve.Evaluate(m_fEnterTweenDelta, ref pos, ref rot, ref fov, ref bLookAt, ref lookAtPos, bEvent))
                {
                    if (m_pFollow != null)
                    {
                        pos = m_pFollow.position;
                        rot = m_pFollow.eulerAngles;
                    }
                    if (bMirror)
                    {
                        float posY = pos.y;
                        pos = m_MirrorReference - pos + m_MirrorReference;
                        pos.y = posY;
                    }

                    SetPosition(pos);
                    if (m_LookatToTarget && bLookAt)
                    {
                        if (bMirror)
                            LookatPos = m_MirrorReference - m_LookatToTarget.position + m_MirrorReference;
                        else
                            LookatPos = m_LookatToTarget.position;
                        SetLookAtPos(LookatPos);
                        if (bMirror) controller.eulerAngles -= Vector3.up * 180f;
                        SetEulerRotation(controller.eulerAngles);
                    }
                    else if (m_bAnimPathLookAt)
                    {
                        if (bMirror)
                        {
                            LookatPos = m_MirrorReference - m_AnimPathLookAtPos + m_MirrorReference;
                            LookatPos.y = m_AnimPathLookAtPos.y;
                        }
                        else
                            LookatPos = m_AnimPathLookAtPos;
                        SetLookAtPos(m_AnimPathLookAtPos);
                        if (bMirror) controller.eulerAngles -= Vector3.up * 180f;
                        SetEulerRotation(controller.eulerAngles);
                    }
                    else if (bLookAt)
                    {
                        if (bMirror)
                        {
                            LookatPos = m_MirrorReference - lookAtPos + m_MirrorReference;
                            LookatPos.y = lookAtPos.y;
                        }
                        else
                            LookatPos = lookAtPos;
                        SetLookAtPos(lookAtPos);
                        if (bMirror) controller.eulerAngles -= Vector3.up * 180f;
                        SetEulerRotation(controller.eulerAngles);
                    }
                    else
                    {
                        if (bMirror) rot -= Vector3.up * 180f;
                        SetEulerRotation(rot);
                    }
                    ICameraController ctl = (ICameraController)CameraController.getInstance();
                    if (ctl != null)
                    {
                        if (ctl.IsEnable() && ctl.GetCurrentMode() != null)
                            ctl.GetCurrentMode().SetCurrentFov(fov);
                        else ctl.UpdateFov(fov);
                    }
                    if (m_fEnterTweenDelta >= maxTime)
                    {
                        OnEnd(pos, rot, fov, bAnimPathUseEnd);
                        m_bPlayEnterTween = false;
                    }
                    return m_fEnterTweenDelta <= maxTime;
                }
            }
            else
            {
                m_bPlayEnterTween = false;
            }
            return false;
        }
        //---------------------------------------------
        public void PausePlay(bool bPause)
        {
            m_bPlayEnterTween = !bPause;
        }
        //---------------------------------------------
        public void SkipTo(float fSkipTime)
        {
            if (m_pCurve == null || !m_pCurve.IsValid()) return;
            if (fSkipTime < 0) fSkipTime = m_pCurve.GetMaxTime()-Time.deltaTime;
            m_fEnterTweenDelta = Mathf.Clamp(fSkipTime, 0, m_pCurve.GetMaxTime());
        }
        //---------------------------------------------
        public void Stop()
        {
            if (m_pCurve == null || !m_pCurve.IsValid()) return;
            m_fEnterTweenDelta = m_pCurve.GetMaxTime();
            PlayEnterTween(0, true);
            if (m_pAsset != null) m_pAsset.Release();
            m_pAsset = null;
        }
        //---------------------------------------------
        public bool IsPlaying()
        {
            return m_bPlayEnterTween && m_pCurve != null && m_pCurve.IsValid();
        }
        //---------------------------------------------
        void OnEnd(Vector3 pos, Vector3 rot, float fov, ushort bUseEnd)
        {
            if(m_vCallback != null)
            {
                for(int i = 0; i < m_vCallback.Count;)
                {
                    if (m_vCallback[i].OnTimelineCallback(m_pAnimPath, EPlayableCallbackType.End, new Variable3())) ++i;
                    else m_vCallback.RemoveAt(i);
                }
            }
        }
        //---------------------------------------------
        public void PlayAnimPath(GameObject pTrigger, GameObject targetObj, Asset clip, SplineCurve.KeyFrame[] frames, BaseEventParameter[] events, AnimationCurve speedCurve, bool bLookAt,
            Vector3 lookatPos, Vector3 MirrorReference, ushort bUseEnd = 0, GameObject lookatTarget = null)
        {
            if (clip == null || targetObj == null) return;
            AnimationClip assetClip = clip.GetOrigin<AnimationClip>();
            if(assetClip == null)
            {
                clip.Release();
                return;
            }
            GameObject sampleTarget = targetObj;
            if (pTrigger != null) sampleTarget = pTrigger;
            if (sampleTarget == null) return;
            if (m_pAsset != null) m_pAsset.Release();
            m_pAsset = null;

            if (m_pCurve == null)
            {
                m_pCurve = new SplineCurve();
                AFrameworkModule framework = Framework.Module.ModuleManager.GetMainFramework<AFrameworkModule>();
                if(framework!=null) m_pCurve.OnTriggerEvent = framework.OnTriggerEvent;
            }
            m_pAsset = clip;
            m_pCurve.Clear();
            m_pCurve.SetClip(sampleTarget, assetClip, frames, null);
            m_pTarget = targetObj ? targetObj.transform : null;
            m_LookatToTarget = lookatTarget ? lookatTarget.transform : null;
            m_MirrorReference = MirrorReference;
            m_pSpeedCurve = speedCurve;
            m_bAnimPathLookAt = bLookAt;
            bAnimPathUseEnd = bUseEnd;
            m_AnimPathLookAtPos = lookatPos;
            m_fEnterTweenDelta = 0;
            m_bEnterTweenTime = assetClip.length > 0;

            bCanSkip = false;
            BuildEvent(events);

            m_pTrigger = pTrigger ? pTrigger.transform : null;
            m_bPlayEnterTween = true;
            PlayEnterTween(0, false);
        }
        //---------------------------------------------
        public void PlayAnimPath(GameObject pTrigger, GameObject target, SplineCurve.KeyFrame[] framekeys, BaseEventParameter[] events, AnimationCurve speedCurve, bool bLookAt, Vector3 lookatPos, Vector3 MirrorReference,
            ushort bUseEnd = 0, GameObject lookatTarget = null)
        {
            if (framekeys == null || target == null) return;
            if (m_pAsset != null) m_pAsset.Release();
            m_pAsset = null;
            if (m_pCurve == null)
            {
                m_pCurve = new SplineCurve();
                AFrameworkModule framework = Framework.Module.ModuleManager.GetMainFramework<AFrameworkModule>();
                if(framework!=null) m_pCurve.OnTriggerEvent = framework.OnTriggerEvent;
            }
            m_pCurve.Clear();
            m_pCurve.AddKeys(framekeys, null);
            bCanSkip = false;
            BuildEvent(events);
            m_MirrorReference = MirrorReference;
            m_pTarget = target.transform;
            m_LookatToTarget = lookatTarget ? lookatTarget.transform : null;
            m_pSpeedCurve = speedCurve;
            m_bAnimPathLookAt = bLookAt;
            bAnimPathUseEnd = bUseEnd;
            m_AnimPathLookAtPos = lookatPos;
            m_fEnterTweenDelta = 0;
            m_bEnterTweenTime = framekeys.Length > 1;
            m_pTrigger = pTrigger ? pTrigger.transform : null;
            if (m_pCurve == null) m_pCurve = new SplineCurve();
            m_bPlayEnterTween = true;
            PlayEnterTween(0, false);
        }
        //---------------------------------------------
        void BuildEvent(BaseEventParameter[] events)
        {
            bCanSkip = false;
            if (events == null || m_pCurve == null) return;

            for (int i = 0; i < events.Length; ++i)
            {
                //if ((events[i].Event is CameraPathAniEventParameter) && (events[i].Event as CameraPathAniEventParameter).status == CameraPathAniEventParameter.Status.Skip)
                //{
                //    bCanSkip = true;
                //    float.TryParse((events[i].Event as CameraPathAniEventParameter).commonMatToTarget, out fSkipToTime);
                //    continue;
                //}
                m_pCurve.AddEvent(events[i]);
            }
        }
        //---------------------------------------------
        public bool ForceUpdate(float fFrametime, bool bEventTrigger)
        {
            if (!m_bEnterTweenTime) return false;
            if(m_vCallback!=null)
            {
                for (int i = 0; i < m_vCallback.Count;)
                {
                    if (m_vCallback[i].OnTimelineCallback(m_pAnimPath, EPlayableCallbackType.Tick, new Variable3())) ++i;
                    else m_vCallback.RemoveAt(i);
                }
            }

            return PlayEnterTween(fFrametime, bEventTrigger);
        }
    }
}