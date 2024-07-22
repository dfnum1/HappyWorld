using System.Collections.Generic;
using Framework.Core;
using Framework.Module;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TopGame.Core
{
    public class CameraTween : ACameraEffect
    {
        struct CurveData
        {
            public float time;
            public Vector3 euler;
            public float distance;
            public Vector3 lookat;
            public float fov;
        }
        private bool m_bEnabled = false;

        List<CurveData> m_vCurveDatas = new List<CurveData>(2);

        float m_fDelta = 0;
        //------------------------------------------------------
        public override bool CanDo()
        {
            return m_bEnabled && m_vCurveDatas.Count > 0;
        }
        //------------------------------------------------------
        public override void Start(EType type)
        {
            m_bEnabled = m_vCurveDatas.Count > 0;
            base.Start(type);
        }
        //------------------------------------------------------
        public override void Stop()
        {
            m_fDelta = 0f;
            m_vCurveDatas.Clear();
            base.Stop();
        }
        //------------------------------------------------------
        public void AddFrame(float fTime, Vector3 euler, float distance, Vector3 lookat, float fov = 45)
        {
            CurveData curve = new CurveData();
            curve.time = fTime;
            curve.euler = euler;
            curve.distance = distance;
            curve.lookat = lookat;
            curve.fov = fov;
            m_vCurveDatas.Add(curve);
        }
        //------------------------------------------------------
        void SyncParam(CameraMode pMode, Vector3 euler, Vector3 lookat, float distance, float fov)
        {
            if (pMode == null) return;
            if (m_Type == ACameraEffect.EType.ModeSet)
            {
                pMode.SetFollowDistance(distance, true);
                pMode.SetCurrentEulerAngle(euler);
                pMode.SetCurrentLookAtOffset(lookat);
                if (m_bFov) pMode.SetCurrentFov(fov);
            }
            //                 else if (m_Type == ACameraEffect.EType.LockSet)
            //                 {
            //                     Vector3 camraPos = loakat - Framework.Core.CommonUtility.EulersAngleToDirection(euler) * distance;
            //                     pMode.SetLockCameraOffset(camraPos);
            //                     pMode.SetLockCameraLookAtOffset(loakat);
            //                     if (m_bFov) pMode.SetLockFovOffset(fov);
            //                 }
            //                 else
            //                 {
            //                     for (int i = 0; i < m_vCallback.Count; ++i)
            //                     {
            //                         m_vCallback[i].OnEffectPosition(camraPos);
            //                         m_vCallback[i].OnEffectEulerAngle( Framework.Core.CommonUtility.DirectionToEulersAngle((loakat-camraPos).normalized) );
            //                         if (m_bFov) m_vCallback[i].OnEffectFov(fov);
            //                     }
            //                 }
        }
        //------------------------------------------------------
        public override bool Update(ICameraController pController, float fFrame)
        {
            CameraMode pMode = pController.GetCurrentMode();
            if (pMode == null) return true;
            if (m_vCurveDatas.Count <= 0)
                m_bEnabled = false;
            if (m_bEnabled)
            {
                m_fDelta += fFrame;

                int __len = m_vCurveDatas.Count;
                int __half;
                int __middle;
                int __first = 0;
                while (__len > 0)
                {
                    __half = __len >> 1;
                    __middle = __first + __half;

                    if (m_fDelta < m_vCurveDatas[__middle].time)
                        __len = __half;
                    else
                    {
                        __first = __middle;
                        ++__first;
                        __len = __len - __half - 1;
                    }
                }

                int lhs = __first - 1;
                int rhs = Mathf.Min(m_vCurveDatas.Count - 1, __first);

                if (m_fDelta >= m_vCurveDatas[m_vCurveDatas.Count-1].time || lhs < 0 || lhs >= m_vCurveDatas.Count || rhs < 0 || rhs >= m_vCurveDatas.Count)
                {
                    CurveData last = m_vCurveDatas[m_vCurveDatas.Count - 1];
                    SyncParam(pMode, last.euler, last.lookat, last.distance, last.fov);
                    Stop();
                    return false;
                }

                CurveData lhsKey = m_vCurveDatas[lhs];
                CurveData rhsKey = m_vCurveDatas[rhs];

                float dx = rhsKey.time - lhsKey.time;
                Vector3 m1 = Vector3.zero, m2 = Vector3.zero;
                float t;
                if (dx != 0f)
                {
                    t = (m_fDelta - lhsKey.time) / dx;
                }
                else
                    t = 0;
                float distance = lhsKey.distance * (1 - t) + rhsKey.distance * t;
                Vector3 loakat = lhsKey.lookat * (1 - t) + rhsKey.lookat * t;
                Vector3 euler = lhsKey.euler * (1 - t) + rhsKey.euler * t;
                float fov = lhsKey.fov * (1-t) + rhsKey.fov * t;
                SyncParam(pMode, euler, loakat, distance, fov);
                return m_bEnabled;
            }
            return m_bEnabled;
        }
    }
}