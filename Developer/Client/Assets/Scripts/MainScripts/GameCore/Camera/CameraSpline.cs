using System.Collections.Generic;
using Framework.Base;
using Framework.Core;
using Framework.Module;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TopGame.Core
{
    public class CameraSpline : ACameraEffect
    {
        public enum EUsedFlag
        {
            Position = 1<<0,
            Rotation = 1<<1,
            Fov = 1<<2,
            LookAt=1<<3,
        }
//         [System.Serializable]
//         public struct CurveData: Framework.Plugin.IQuickSort<CurveData>
//         {
//             public float time;
//             public Vector3 position;
//             public Vector3 inTan;
//             public Vector3 outTan;
//             public Quaternion rotation;
//             public Vector3 lookat;
//             public float fov;
//             //-----------------------------------------------------
//             public int CompareTo(int type, CurveData other)
//             {
//                 if (time < other.time) return -1;
//                 if (time > other.time) return 1;
//                 return 0;
//             }
// #if UNITY_EDITOR
//             [System.NonSerialized]
//             public bool bExpand;
//             [System.NonSerialized]
//             public bool bSyncToCamera;
// 
//             [System.NonSerialized]
//             public Transform lookatTrans;
// 
//             [System.NonSerialized]
//             public float editorTime;
// 
//             //------------------------------------------------------
//             public void Write(ref TopGame.Data.BinaryUtil write)
//             {
//                 write.WriteFloat(time);
//                 write.WriteVector3(position);
//                 write.WriteVector3(inTan);
//                 write.WriteVector3(outTan);
//                 write.WriteQuaternion(rotation);
//                 write.WriteVector3(lookat);
//                 write.WriteFloat(fov);
//             }
// #endif
//             //------------------------------------------------------
//             public void Read(ref TopGame.Data.BinaryUtil write)
//             {
//                 time = write.ToFloat();
//                 position = write.ToVec3();
//                 inTan = write.ToVec3();
//                 outTan = write.ToVec3();
//                 rotation = write.ToQuaternion();
//                 lookat = write.ToVec3();
//                 fov = write.ToFloat();
//             }
//         }
        private bool m_bEnabled = false;

        [Framework.Data.DisplayEnumBitGUI(typeof(EUsedFlag))]
        private uint m_nFlags = 0;
        List<CurveData> m_vKeys = new List<CurveData>(4);
        private float m_fPlaySpeed = 1;
        private float m_fDuration = 0;
#if UNITY_EDITOR
        public List<CurveData> Frames
        {
            get { return m_vKeys; }
            set { m_vKeys = value; }
        }
        public uint usedFlag
        {
            get { return m_nFlags;  }
            set { m_nFlags = value; }
        }
        public bool bAbsCalc = false;
#endif

        float m_fDelta = 0;     
        //-----------------------------------------------------
        public bool IsEnable(EUsedFlag flag)
        {
            return (m_nFlags & (uint)flag) != 0;
        }
        //-----------------------------------------------------
        public void Enable(EUsedFlag flag, bool bEnable)
        {
            if (bEnable)
                m_nFlags |= (uint)flag;
            else
                m_nFlags &= (~((uint)flag));
        }
        //------------------------------------------------------
        public override bool CanDo()
        {
            return m_bEnabled && m_vKeys !=null && m_vKeys.Count > 0;
        }
        //------------------------------------------------------
        public override void Start(EType type)
        {
            if (m_vKeys.Count <= 0)
                return;
            m_bEnabled = m_vKeys[m_vKeys.Count - 1].time > 0;
            base.Start(type);
        }
        //------------------------------------------------------
        public override void Stop()
        {
            m_fDelta = 0f;
            m_vKeys = null;
            m_fPlaySpeed = 1;
            m_fDuration = 0;
            base.Stop();
        }
        //------------------------------------------------------
        public float GetMaxTime()
        {
            float fTime = 0;
            for(int i = 0; i < m_vKeys.Count; ++i)
            {
                fTime = Mathf.Max(m_vKeys[i].time, fTime);
            }
            return fTime;
        }
        //------------------------------------------------------
        public void PlayCurve(CameraSplineEventParameter param)
        {
            if (param.KeyFrames.Count <= 0) return;
            m_nFlags = param.useFlags;
            m_fDelta = 0;
            m_bFov = IsEnable(EUsedFlag.Fov);
            m_Type = (ACameraEffect.EType)param.splineType;
            m_vKeys = param.KeyFrames;
            m_fPlaySpeed = param.playSpeed;
            m_fDuration = GetMaxTime();
            m_bEnabled = m_fDuration > 0;
        }
        //------------------------------------------------------
        public bool Evaluate(float time, ref Vector3 retPos, ref Quaternion retEuler, ref float retFov)
        {
            if (m_vKeys.Count <= 0) return false;
            if (time <= m_vKeys[0].time)
            {
                retPos = m_vKeys[0].position;
                retEuler = m_vKeys[0].rotation;
                retFov = m_vKeys[0].fov;
                return true;
            }
            if (time >= m_vKeys[m_vKeys.Count - 1].time)
            {
                retPos = m_vKeys[m_vKeys.Count - 1].position;
                retEuler = m_vKeys[m_vKeys.Count - 1].rotation;
                retFov = m_vKeys[m_vKeys.Count - 1].fov;

                return true;
            }

            int __len = m_vKeys.Count;
            int __half;
            int __middle;
            int __first = 0;
            while (__len > 0)
            {
                __half = __len >> 1;
                __middle = __first + __half;

                if (time < m_vKeys[__middle].time)
                    __len = __half;
                else
                {
                    __first = __middle;
                    ++__first;
                    __len = __len - __half - 1;
                }
            }

            int lhs = __first - 1;
            int rhs = Mathf.Min(m_vKeys.Count - 1, __first);

            if (lhs < 0 || lhs >= m_vKeys.Count || rhs < 0 || rhs >= m_vKeys.Count)
                return false;

            CurveData lhsKey = m_vKeys[lhs];
            CurveData rhsKey = m_vKeys[rhs];

            float dx = rhsKey.time - lhsKey.time;
            Vector3 m1 = Vector3.zero, m2 = Vector3.zero;
            float t;
            if (dx != 0f)
            {
                t = (time - lhsKey.time) / dx;
                //  m1 = 
            }
            else
                t = 0;

            m1 = lhsKey.position + lhsKey.outTan;
            m2 = rhsKey.position + rhsKey.inTan;
            if(IsEnable(EUsedFlag.Position))
                retPos = Framework.Core.CommonUtility.Bezier4(t, lhsKey.position, m1, m2, rhsKey.position);
            if (IsEnable(EUsedFlag.LookAt))
            {
                Vector3 lookat = Vector3.Lerp(lhsKey.lookat, rhsKey.lookat, t);
                retEuler = Quaternion.Euler(Framework.Core.CommonUtility.DirectionToEulersAngle((lookat - retPos).normalized));
            }
            if (IsEnable(EUsedFlag.Rotation))
                retEuler = Quaternion.Lerp(lhsKey.rotation, rhsKey.rotation, t);
            retFov = Mathf.SmoothStep(lhsKey.fov, rhsKey.fov, t);
            return true;
        }
        //------------------------------------------------------
        public override bool Update(ICameraController pController, float fFrame)
        {
            CameraMode pMode = pController.GetCurrentMode();
            if (pMode == null) return true;
            if (m_bEnabled)
            {
                float fTime = 0;
                if(m_fPlaySpeed < 0)
                {
                    m_fDelta -= fFrame * m_fPlaySpeed;
                    fTime = m_fDuration - m_fDelta;
                }
                else
                {
                    m_fDelta += fFrame * m_fPlaySpeed;
                    fTime = m_fDelta;
                }

                Vector3 pos = Vector3.zero;
                Quaternion eulerAngle = Quaternion.identity;
                float fov = 0;
                if(Evaluate(fTime, ref pos, ref eulerAngle, ref fov))
                {
                    if (m_Type == EType.ModeSet)
                    {
                        if(IsEnable(EUsedFlag.Position) || IsEnable(EUsedFlag.LookAt))
                        {
                            Vector3 lookat = Framework.Core.CommonUtility.RayHitPos(pos, Framework.Core.CommonUtility.EulersAngleToDirection(eulerAngle.eulerAngles));
                            pMode.SetFollowDistance((pos - lookat).magnitude, true);
                            pMode.SetFollowLookAtPosition(lookat);
                        }
                        if(IsEnable(EUsedFlag.Rotation) || IsEnable(EUsedFlag.LookAt))
                            pMode.SetCurrentEulerAngle(eulerAngle.eulerAngles);
                        if (IsEnable(EUsedFlag.Fov)) pMode.SetCurrentFov(fov);
                    }
                    else if (m_Type == EType.LockSet)
                    {
                        if(IsEnable(EUsedFlag.Position))
                            pMode.SetExternOffset(pos);
                        if (IsEnable(EUsedFlag.Rotation) || IsEnable(EUsedFlag.LookAt))
                            pMode.SetExternEulerAngleOffset(eulerAngle.eulerAngles);
                        if (IsEnable(EUsedFlag.Fov)) pMode.SetLockFovOffset(fov);
                    }
                    else
                    {
                        for (int i = 0; i < m_vCallback.Count; ++i)
                        {
                            m_vCallback[i].OnEffectPosition(pos);
                            m_vCallback[i].OnEffectEulerAngle(eulerAngle.eulerAngles);
                            if (m_bFov) m_vCallback[i].OnEffectFov(fov);
                        }
                    }
                }
                if (fTime >= m_fDuration)
                {
                    Stop();
                    m_bEnabled = false;
                }
                return m_bEnabled;
            }
            return m_bEnabled;
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public void DrawDebug( int controller = 0, bool bEiditAble = true, float fArrowRadius = 6)
        {
            if (m_vKeys.Count <= 0) return;
            Vector3 prePos = Vector3.zero;
            float maxTime = GetMaxTime();
            Color bak = UnityEditor.Handles.color;
            prePos = m_vKeys[0].position;
            float time = 0f;
            while (time < maxTime)
            {
                Vector3 pos = Vector3.zero, lookat = Vector3.zero;
                Quaternion rot = Quaternion.identity;
                float f = 0f;
                Evaluate(time, ref pos, ref rot, ref f);

                UnityEditor.Handles.color = Color.blue;
                UnityEditor.Handles.DrawLine(prePos, pos);
                UnityEditor.Handles.color = bak;


                prePos = pos;
                time += 0.01f;
            }
        }
#endif
    }
}