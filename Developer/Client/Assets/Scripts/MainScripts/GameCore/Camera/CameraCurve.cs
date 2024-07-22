using System.Collections.Generic;
using Framework.Core;
using Framework.Module;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TopGame.Core
{
    public class CameraCurve : ACameraEffect
    {
        public struct CurveData
        {
            public AnimationCurve PositionX;
            public AnimationCurve PositionY;
            public AnimationCurve PositionZ;

            public AnimationCurve LookAtX;
            public AnimationCurve LookAtY;
            public AnimationCurve LookAtZ;
            public bool bUseFov;
            public AnimationCurve Fov;
            public bool bAbs;

            public float fDuration;
        }
        private bool m_bEnabled = false;

        CurveData m_CurCurveData = new CurveData() { fDuration = 0 };
        List<CurveData> m_vCurveDatas = new List<CurveData>(4);

        float m_fDelta = 0;
        //------------------------------------------------------
        public override bool CanDo()
        {
            return m_bEnabled && m_CurCurveData.fDuration > 0;
        }
        //------------------------------------------------------
        public override void Start(EType type)
        {
            m_bEnabled = m_CurCurveData.fDuration > 0;
            base.Start(type);
        }
        //------------------------------------------------------
        public override void Stop()
        {
            m_fDelta = 0f;
            m_CurCurveData.fDuration = 0;
            if (m_vCurveDatas.Count > 0)
            {
                DoCurve(m_vCurveDatas[0]);
                m_vCurveDatas.RemoveAt(0);
            }
            m_bEnabled = m_CurCurveData.fDuration > 0;
            base.Stop();
        }
        //------------------------------------------------------
        void DoCurve(CurveData curve)
        {
            bool bAbs = false;
            if (m_CurCurveData.fDuration > 0) bAbs = m_CurCurveData.bAbs;
            m_CurCurveData = curve;
            m_fDelta = 0;
            if (m_CurCurveData.fDuration <= 0)
                Stop();
            else
            {
                m_bEnabled = true;
            }
        }
        //------------------------------------------------------
        public void PlayCurve(CurveData data)
        {
            if (data.fDuration <= 0) return;
            m_vCurveDatas.Clear();
            m_fDelta = 0;
            DoCurve(data);
        }
        //------------------------------------------------------
        public void PlayCurve(CameraCurveEventParameter curveParam)
        {
            CurveData curve = new CurveData();
            curve.fDuration = 0;
            curve.PositionX = curveParam.PositionX;
            curve.fDuration = Mathf.Max(curve.fDuration, Framework.Core.CommonUtility.GetCurveMaxTime(curveParam.PositionX));
            curve.PositionY = curveParam.PositionY;
            curve.fDuration = Mathf.Max(curve.fDuration, Framework.Core.CommonUtility.GetCurveMaxTime(curveParam.PositionY));
            curve.PositionZ = curveParam.PositionZ;
            curve.fDuration = Mathf.Max(curve.fDuration, Framework.Core.CommonUtility.GetCurveMaxTime(curveParam.PositionZ));

            curve.LookAtX = curveParam.LookAtX;
            curve.fDuration = Mathf.Max(curve.fDuration, Framework.Core.CommonUtility.GetCurveMaxTime(curveParam.LookAtX));
            curve.LookAtY = curveParam.LookAtY;
            curve.fDuration = Mathf.Max(curve.fDuration, Framework.Core.CommonUtility.GetCurveMaxTime(curveParam.LookAtY));
            curve.LookAtZ = curveParam.LookAtZ;
            curve.fDuration = Mathf.Max(curve.fDuration, Framework.Core.CommonUtility.GetCurveMaxTime(curveParam.LookAtZ));
            curve.bUseFov = curveParam.bUseFov;
            if (curve.bUseFov)
            {
                curve.Fov = curveParam.Fov;
                curve.fDuration = Mathf.Max(curve.fDuration, Framework.Core.CommonUtility.GetCurveMaxTime(curveParam.Fov));
            }
            curve.bAbs = curveParam.bAbs;

            if (curve.fDuration <= 0) return;

            if(curveParam.playType == ECameraCurveType.StopAll)
            {
                m_vCurveDatas.Clear();
                m_fDelta = 0;
                DoCurve(curve);          
            }
            else if (curveParam.playType == ECameraCurveType.StopCurrent)
            {
                m_fDelta = 0;
                DoCurve(curve);
            }
            else if (curveParam.playType == ECameraCurveType.WaitTime)
            {
                if(m_CurCurveData.fDuration<=0)
                {
                    m_fDelta = 0;
                    DoCurve(curve);
                }
                else
                    m_vCurveDatas.Add(curve);
            }
            else if (curveParam.playType == ECameraCurveType.CurrentEndPlay)
            {
                if (m_CurCurveData.fDuration <= 0)
                {
                    m_fDelta = 0;
                    DoCurve(curve);
                }
                else
                {
                    m_vCurveDatas.Insert(1,curve);
                }
            }
        }
        //------------------------------------------------------
        public override bool Update(ICameraController pController, float fFrame)
        {
            CameraMode pMode = pController.GetCurrentMode();
            if (pMode == null) return true;
            if (m_bEnabled)
            {
                m_fDelta += fFrame;
                if(m_fDelta <= m_CurCurveData.fDuration)
                {
                    Vector3 camraPos = Vector3.zero;
                    Vector3 loakat = Vector3.zero;
                    float fov = 0;
                    if (m_CurCurveData.PositionX != null) camraPos.x = m_CurCurveData.PositionX.Evaluate(m_fDelta);
                    if (m_CurCurveData.PositionY != null) camraPos.y = m_CurCurveData.PositionY.Evaluate(m_fDelta);
                    if (m_CurCurveData.PositionZ != null) camraPos.z = m_CurCurveData.PositionZ.Evaluate(m_fDelta);
                    if (m_CurCurveData.LookAtX != null) loakat.x = m_CurCurveData.LookAtX.Evaluate(m_fDelta);
                    if (m_CurCurveData.LookAtY != null) loakat.y = m_CurCurveData.LookAtY.Evaluate(m_fDelta);
                    if (m_CurCurveData.LookAtZ != null) loakat.z = m_CurCurveData.LookAtZ.Evaluate(m_fDelta);
                    if (m_CurCurveData.Fov !=null) fov = m_CurCurveData.Fov.Evaluate(m_fDelta);

                    if(m_CurCurveData.bAbs)
                    {
                        pMode.SetLockCameraOffset(camraPos);
                        pMode.SetLockCameraLookAtOffset(loakat);
                        if (m_CurCurveData.bUseFov) pMode.SetCurrentFov(fov);
                    }
                    else
                    {
                        for(int i = 0; i < m_vCallback.Count; ++i)
                        {
                            m_vCallback[i].OnEffectPosition(camraPos);
                            m_vCallback[i].OnEffectLookAt(loakat);
                            if (m_CurCurveData.bUseFov) m_vCallback[i].OnEffectFov(fov);
                        }
                    }
                }
                else
                {
                    Stop();
                }
                return m_bEnabled;
            }
            return m_bEnabled;
        }
    }
}