/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	曲线帧
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Framework.Core;

namespace TopGame.Core
{
    public class SplineCurve
    {
        [System.Serializable]
        public struct KeyFrame : Framework.Plugin.IQuickSort<KeyFrame>
        {
            public float time;
            public Vector3 inTan;
            public Vector3 outTan;
            public Vector3 position;
            public Vector3 eulerAngle;
            public bool bSet;
            public bool bLookAt;
            public Vector3 lookat;
            public float fov;

            public static KeyFrame Epsilon = new KeyFrame() { time = -1 };

            public void Init()
            {
                time = -1;
                fov = 0;
                inTan = outTan = position = eulerAngle = Vector3.zero;
                bSet = bLookAt = false;
            }

            public int CompareTo(int type, KeyFrame other)
            {
                if (time < other.time) return -1;
                else if (time > other.time) return 1;
                return 0;
            }

            public bool isValid
            {
                get { return time >= 0; }
            }
            //-----------------------------------------------------
            public void Destroy()
            {
            }
        }
        //-----------------------------------------------------
        public System.Action<BaseEventParameter, bool> OnTriggerEvent = null;
        GameObject m_pTarget = null;
        AnimationClip m_pClip = null;
        AnimationCurve m_pFovCurve = null;
        public AnimationClip clip
        {
            get { return m_pClip; }
        }
        public AnimationCurve FovCurve
        {
            get { return m_pFovCurve; }
            set { m_pFovCurve = value; }
        }

        List<Vector3> m_vPoints = new List<Vector3>();
        List<KeyFrame> m_Keys = new List<KeyFrame>();
        List<BaseEventParameter> m_vEvents = new List<BaseEventParameter>();
        public List<KeyFrame> keys
        {
            get { return m_Keys; }
            set { m_Keys = value; }
        }
        //-----------------------------------------------------
        public List<BaseEventParameter> events
        {
            get { return m_vEvents; }
            set { m_vEvents = value; }
        }
        //-----------------------------------------------------
        public float GetMaxTime()
        {
            if (m_pClip != null) return m_pClip.length;
            if (m_Keys.Count > 0) return m_Keys[m_Keys.Count - 1].time;
            return 0;
        }
        //-----------------------------------------------------
        public void AddKey(float t, Vector3 pos)
        {
            for (int i = 0; i < m_Keys.Count; ++i)
            {
                if (Mathf.Abs(m_Keys[i].time - t) <= 0.01f)
                {
                    m_Keys[i] = new KeyFrame() { time = t, position = pos };
                    return;
                }
            }
            m_Keys.Add(new KeyFrame() { time = t, position = pos });
            Framework.Plugin.SortUtility.QuickSortUp<KeyFrame>(ref m_Keys);
        }
        //-----------------------------------------------------
        public void AddKey(float t, Vector3 pos, Vector3 rot)
        {
            for (int i = 0; i < m_Keys.Count; ++i)
            {
                if (Mathf.Abs(m_Keys[i].time - t) <= 0.01f)
                {
                    m_Keys[i] = new KeyFrame() { time = t, position = pos, eulerAngle = rot};
                    return;
                }
            }
            m_Keys.Add(new KeyFrame() { time = t, position =pos, eulerAngle  = rot});
            Framework.Plugin.SortUtility.QuickSortUp<KeyFrame>(ref m_Keys);
        }
        //-----------------------------------------------------
        public void AddKey(float t, Vector3 pos, Vector3 rot, float fieldFov)
        {
            for (int i = 0; i < m_Keys.Count; ++i)
            {
                if (Mathf.Abs(m_Keys[i].time - t) <= 0.01f)
                {
                    m_Keys[i] = new KeyFrame() { time = t, position = pos, eulerAngle = rot, fov = fieldFov };
                    return;
                }
            }
            m_Keys.Add(new KeyFrame() { time = t, position = pos, eulerAngle = rot, fov = fieldFov });
            Framework.Plugin.SortUtility.QuickSortUp<KeyFrame>(ref m_Keys);
        }
        //-----------------------------------------------------
        public void AddKey(KeyFrame key)
        {
            for(int i = 0; i < m_Keys.Count;++i)
            {
                if( Mathf.Abs(m_Keys[i].time-key.time) <= 0.01f )
                {
                    m_Keys[i] = key;
                    return;
                }
            }
            m_Keys.Add(key);
            Framework.Plugin.SortUtility.QuickSortUp<KeyFrame>(ref m_Keys);
        }
        //-----------------------------------------------------
        public void AddEvent(BaseEventParameter evt)
        {
            m_vEvents.Add(evt);
            Framework.Plugin.SortUtility.QuickSortUp<BaseEventParameter>(ref m_vEvents);
        }
        //-----------------------------------------------------
        public bool IsValid()
        {
            if (m_pClip) return m_pClip.length>0;
            return m_Keys.Count > 0;
        }
        //-----------------------------------------------------
        public void SetClip(GameObject target, AnimationClip clip, KeyFrame[] Fovs, BaseEventParameter[] events = null)
        {
            Clear();
            m_pClip = clip;
            m_pTarget = target;
            m_pFovCurve = null;
            if(Fovs !=null && Fovs.Length>1)
            {
                m_pFovCurve = new AnimationCurve();
                for (int i = 0; i < Fovs.Length; ++i)
                    m_pFovCurve.AddKey(new Keyframe() { time = Fovs[i].time, inTangent = Fovs[i].inTan.x, outTangent = Fovs[i].outTan.x, tangentMode = (int)Fovs[i].inTan.y, value = Fovs[i].fov });
            }
            if (events != null)
            {
                for (int i = 0; i < events.Length; ++i)
                {
                    m_vEvents.Add(events[i]);
                }
                Framework.Plugin.SortUtility.QuickSortUp<BaseEventParameter>(ref m_vEvents);
            }
        }
        //-----------------------------------------------------
        public void AddEventCores(KeyFrame[] keys, ActionEventCore pEventCore)
        {
            Clear();
            if (keys == null) return;
            for(int i = 0; i < keys.Length; ++i)
            {
                m_Keys.Add(keys[i]);
            }
            Framework.Plugin.SortUtility.QuickSortUp<KeyFrame>(ref m_Keys);
            pEventCore.BuildEvent(null, m_vEvents);
            Framework.Plugin.SortUtility.QuickSortUp<BaseEventParameter>(ref m_vEvents);
        }
        //-----------------------------------------------------
        public void AddKeys(KeyFrame[] keys, BaseEventParameter[] events)
        {
            Clear();
            if (keys == null) return;
            for (int i = 0; i < keys.Length; ++i)
            {
                m_Keys.Add(keys[i]);
            }
            Framework.Plugin.SortUtility.QuickSortUp<KeyFrame>(ref m_Keys);
            if(events!=null)
            {
                for (int i = 0; i < events.Length; ++i)
                    m_vEvents.Add(events[i]);
                Framework.Plugin.SortUtility.QuickSortUp<BaseEventParameter>(ref m_vEvents);
            }
        }
        //-----------------------------------------------------
        public void Clear()
        {
            m_pFovCurve = null;
            m_pClip = null;
            m_pTarget = null;
            m_Keys.Clear();
            m_vEvents.Clear();
        }
        //-----------------------------------------------------
        public KeyFrame GetLastKey()
        {
            if(m_pClip != null)
            {
                if(m_pTarget)
                {
                    KeyFrame frame = new KeyFrame();
                    m_pClip.SampleAnimation(m_pTarget, m_pClip.length);
                    frame.position = m_pTarget.transform.position;
                    frame.eulerAngle = m_pTarget.transform.eulerAngles;
                    if (m_pFovCurve != null && m_pFovCurve.length>1)
                        frame.fov = m_pFovCurve.Evaluate(m_pClip.length);
                    return frame;
                }
                return KeyFrame.Epsilon;
            }
            if (m_Keys.Count <= 0) return KeyFrame.Epsilon;
            return m_Keys[m_Keys.Count - 1];
        }
        //-----------------------------------------------------
        public KeyFrame GetKey(float time)
        {
            for (int i = 0; i < m_Keys.Count; ++i)
            {
                if (Mathf.Abs(m_Keys[i].time - time) <= 0.01)
                {
                    return m_Keys[i];
                }
            }
            return KeyFrame.Epsilon;
        }
        //-----------------------------------------------------
        public KeyFrame GetKeyByIndex(int index)
        {
            if (index < 0 || index >= m_Keys.Count) return KeyFrame.Epsilon;
            return m_Keys[index];
        }
        //-----------------------------------------------------
        public void RemoveKey(KeyFrame frame)
        {
            if (m_Keys.Contains(frame))
                m_Keys.Remove(frame);
        }
        //-----------------------------------------------------
        public void RemoveKeys(float time)
        {
            for(int i = 0; i < m_Keys.Count;)
            {
                if (m_Keys[i].time < time)
                {
                    m_Keys.RemoveAt(i);
                }
                else ++i;
            }
        }
        //-----------------------------------------------------
        public bool Evaluate(float time, ref Vector3 retPos, ref Vector3 retEuler, ref float retFov, ref bool bLookAt, ref Vector3 lookAtPos, bool bEvent = true, bool bEditor = false)
        {
            if (m_pClip != null && m_pTarget)
            {
                m_pClip.SampleAnimation(m_pTarget, time);
                Transform trans = m_pTarget.transform;
                retPos = trans.position;
                retEuler = trans.eulerAngles;
                bLookAt = false;
                if (m_pFovCurve!=null && m_pFovCurve.length>1)
                {
                    retFov = m_pFovCurve.Evaluate(time);
                }
                if (!bEditor && time > m_pClip.length)
                {
                    if(bEvent)
                    {
                        if (OnTriggerEvent != null)
                        {
                            for (int i = 0; i < events.Count; ++i)
                            {
                                OnTriggerEvent(events[i], false);
                            }
                        }
                    }

                    events.Clear();
                    m_pTarget = null;
                    m_pClip = null;
                    m_pFovCurve = null;
                }
            }
            else
            {
                if (m_Keys.Count <= 0) return false;
                if (time <= m_Keys[0].time)
                {
                    retPos = m_Keys[0].position;
                    retEuler = m_Keys[0].eulerAngle;
                    retFov = m_Keys[0].fov;
                    bLookAt = m_Keys[0].bLookAt;
                    lookAtPos = m_Keys[0].lookat;
                    return true;
                }
                if (time >= m_Keys[m_Keys.Count - 1].time)
                {
                    retPos = m_Keys[m_Keys.Count - 1].position;
                    retEuler = m_Keys[m_Keys.Count - 1].eulerAngle;
                    retFov = m_Keys[m_Keys.Count - 1].fov;
                    bLookAt = m_Keys[m_Keys.Count - 1].bLookAt;
                    lookAtPos = m_Keys[m_Keys.Count - 1].lookat;

                    if (OnTriggerEvent != null)
                    {
                        for (int i = 0; i < events.Count; ++i)
                        {
                            OnTriggerEvent(events[i],false);
                        }
                    }
                    events.Clear();
                    return true;
                }

                int __len = m_Keys.Count;
                int __half;
                int __middle;
                int __first = 0;
                while (__len > 0)
                {
                    __half = __len >> 1;
                    __middle = __first + __half;

                    if (time < m_Keys[__middle].time)
                        __len = __half;
                    else
                    {
                        __first = __middle;
                        ++__first;
                        __len = __len - __half - 1;
                    }
                }

                int lhs = __first - 1;
                int rhs = Mathf.Min(m_Keys.Count - 1, __first);

                if (lhs < 0 || lhs >= m_Keys.Count || rhs < 0 || rhs >= m_Keys.Count)
                    return false;

                KeyFrame lhsKey = m_Keys[lhs];
                KeyFrame rhsKey = m_Keys[rhs];

                if (rhsKey.bSet)
                {
                    retPos = rhsKey.position;
                    retEuler = rhsKey.eulerAngle;
                    retFov = rhsKey.fov;

                    bLookAt = lhsKey.bLookAt;
                    lookAtPos = rhsKey.lookat;
                    return true;
                }

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
                retPos = Bezier4(t, lhsKey.position, m1, m2, rhsKey.position);
                retEuler = Vector3.Slerp(lhsKey.eulerAngle, rhsKey.eulerAngle, t);
                retFov = lhsKey.fov * t + rhsKey.fov * (1 - t);

                bLookAt = lhsKey.bLookAt;
                lookAtPos = Vector3.Lerp(lhsKey.lookat, rhsKey.lookat, t);
            }

            if (bEvent && events != null && OnTriggerEvent != null)
            {
                while (events.Count > 0 && time >= events[0].triggetTime)
                {
                    OnTriggerEvent(events[0],false);
                    events.RemoveAt(0);
                }
            }

#if UNITY_EDITOR
            if (UnityEditor.SceneView.lastActiveSceneView) UnityEditor.SceneView.lastActiveSceneView.Repaint();
#endif
            return true;
        }
        //-----------------------------------------------------
        public bool Evaluate(float time, ref Vector3 retPos, ref Vector3 retEuler, bool bEvent = true)
        {
            if (m_pClip != null && m_pTarget)
            {
                m_pClip.SampleAnimation(m_pTarget, time);
                Transform trans = m_pTarget.transform;
                retPos = trans.position;
                retEuler = trans.eulerAngles;
                if (time >= m_pClip.length)
                {
                    if (OnTriggerEvent != null)
                    {
                        for (int i = 0; i < events.Count; ++i)
                        {
                            OnTriggerEvent(events[i],false);
                        }
                    }
                    events.Clear();
                    m_pTarget = null;
                    m_pClip = null;
                    m_pFovCurve = null;
                }
            }
            else
            {
                if (m_Keys.Count <= 0) return false;
                if (time <= m_Keys[0].time)
                {
                    retPos = m_Keys[0].position;
                    retEuler = m_Keys[0].eulerAngle;
                    return true;
                }
                if (time >= m_Keys[m_Keys.Count - 1].time)
                {
                    retPos = m_Keys[m_Keys.Count - 1].position;
                    retEuler = m_Keys[m_Keys.Count - 1].eulerAngle;

                    if (OnTriggerEvent != null)
                    {
                        for (int i = 0; i < events.Count; ++i)
                        {
                            OnTriggerEvent(events[i],false);
                        }
                    }
                    events.Clear();
                    return false;
                }

                int __len = m_Keys.Count;
                int __half;
                int __middle;
                int __first = 0;
                while (__len > 0)
                {
                    __half = __len >> 1;
                    __middle = __first + __half;

                    if (time < m_Keys[__middle].time)
                        __len = __half;
                    else
                    {
                        __first = __middle;
                        ++__first;
                        __len = __len - __half - 1;
                    }
                }

                int lhs = __first - 1;
                int rhs = Mathf.Min(m_Keys.Count - 1, __first);

                if (lhs < 0 || lhs >= m_Keys.Count || rhs < 0 || rhs >= m_Keys.Count)
                    return false;

                KeyFrame lhsKey = m_Keys[lhs];
                KeyFrame rhsKey = m_Keys[rhs];

                if (rhsKey.bSet)
                {
                    retPos = rhsKey.position;
                    retEuler = rhsKey.eulerAngle;
                    return true;
                }

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
                retPos = lhsKey.position * (1 - t) + rhsKey.position * t;
               //  retPos = Bezier4(t, lhsKey.position, m1, m2, rhsKey.position);
                retEuler = Vector3.Slerp(lhsKey.eulerAngle, rhsKey.eulerAngle, t);
            }

            if (bEvent && events != null && OnTriggerEvent != null)
            {
                if (events.Count > 0)
                {
                    if (time >= events[0].triggetTime)
                    {
                        OnTriggerEvent(events[0],false);
                        events.RemoveAt(0);
                    }
                }
            }

#if UNITY_EDITOR
            if (UnityEditor.SceneView.lastActiveSceneView) UnityEditor.SceneView.lastActiveSceneView.Repaint();
#endif
            return true;
        }
        //-----------------------------------------------------
        public static Vector3 Bezier2(float t, Vector3 p1, Vector3 p2)
        {
            Vector3 val = p1 + t * (p2 - p1);

            return val;
        }
        //-----------------------------------------------------
        public static Vector3 Bezier3(float t, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 lbStart = Bezier2(t, p1, p2);
            Vector3 lbEnd = Bezier2(t, p2, p3);
            Vector3 val = lbStart + (lbEnd - lbStart) * t;
            return val;
        }
        //-----------------------------------------------------
        public static Vector3 Bezier4(float t, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            Vector3 lbStart = Bezier3(t, p1, p2, p3);
            Vector3 lbEnd = Bezier3(t, p2, p3, p4);
            Vector3 val = lbStart + (lbEnd - lbStart) * t;

            return val;
        }
#if UNITY_EDITOR
        //-----------------------------------------------------
        public void DrawDebug(int controller, int maxDuration = 100, bool bShowTanArrow = true, float radius = 6, bool bEditAble = true)
        {
            if (m_pClip != null)
            {
                return;
            }
            if (m_Keys.Count>0)
            {
                Vector3 prePos = Vector3.zero;
                float maxTime = GetMaxTime();       
                Color bak = UnityEditor.Handles.color;
                if(bEditAble)
                {
                    for (int i = 0; i < m_Keys.Count; ++i)
                    {
                        Quaternion rot = Quaternion.identity;
                        rot.eulerAngles = m_Keys[i].eulerAngle;
                        UnityEditor.Handles.color = Color.red;
                        UnityEditor.Handles.ArrowHandleCap(controller, m_Keys[i].position, rot, 20f, EventType.Repaint);
                        UnityEditor.Handles.color = bak;
                        UnityEditor.Handles.CubeHandleCap(controller, m_Keys[i].position, rot, radius, EventType.Repaint);
                        maxTime = Mathf.Max(m_Keys[i].time, maxTime);

                        if(m_Keys[i].bLookAt)
                        {
                            UnityEditor.Handles.color = Color.red;
                            UnityEditor.Handles.CubeHandleCap(controller, m_Keys[i].lookat, rot, radius, EventType.Repaint);
                            UnityEditor.Handles.color = bak;
                        }

                        if (bShowTanArrow)
                        {
                            if (i == 0)
                            {
                                UnityEditor.Handles.color = Color.green;
                                if (m_Keys[i].outTan.magnitude != 0)
                                    UnityEditor.Handles.ArrowHandleCap(controller, m_Keys[i].position, Quaternion.LookRotation(m_Keys[i].outTan), 15f, EventType.Repaint);
                            }
                            else if (i == m_Keys.Count - 1)
                            {
                                UnityEditor.Handles.color = Color.red;
                                if (m_Keys[i].inTan.magnitude != 0)
                                    UnityEditor.Handles.ArrowHandleCap(controller, m_Keys[i].position, Quaternion.LookRotation(m_Keys[i].inTan), 15f, EventType.Repaint);

                            }
                            else
                            {
                                UnityEditor.Handles.color = Color.red;
                                if (m_Keys[i].inTan.magnitude != 0)
                                    UnityEditor.Handles.ArrowHandleCap(controller, m_Keys[i].position, Quaternion.LookRotation(m_Keys[i].inTan), 15f, EventType.Repaint);
                                UnityEditor.Handles.color = Color.green;
                                if (m_Keys[i].outTan.magnitude != 0)
                                    UnityEditor.Handles.ArrowHandleCap(controller, m_Keys[i].position, Quaternion.LookRotation(m_Keys[i].outTan), 15f, EventType.Repaint);
                            }
                        }

                        UnityEditor.Handles.color = bak;
                    }
                }
                
                prePos = m_Keys[0].position;
                Vector3 preLookatPos = m_Keys[0].lookat;
                float time = 0f;
                while (time < maxTime)
                {
                    Vector3 pos = Vector3.zero, rot = Vector3.zero, lookat = Vector3.zero;
                    float f = 0f;
                    bool blookAt = false;
                    Evaluate(time, ref pos, ref rot, ref f, ref blookAt, ref lookat, false);

                    UnityEditor.Handles.color = Color.yellow;
                    UnityEditor.Handles.DrawLine(prePos, pos);
                    UnityEditor.Handles.color = bak;

                    if(blookAt)
                    {
                        UnityEditor.Handles.color = Color.red;
                        UnityEditor.Handles.DrawLine(preLookatPos, lookat);
                        UnityEditor.Handles.color = bak;
                    }


                    prePos = pos;
                    preLookatPos = lookat;
                    time += 0.1f;
                }
            }

        }
        //-----------------------------------------------------
        public void DrawGizmos(float radius = 6, bool bEditAble = true)
        {
            if (m_pClip != null)
            {
                return;
            }
            if (m_Keys.Count > 0)
            {
                Vector3 prePos = Vector3.zero;
                float maxTime = GetMaxTime();
                Color bak = Gizmos.color;
                if (bEditAble)
                {
                    for (int i = 0; i < m_Keys.Count; ++i)
                    {
                        Quaternion rot = Quaternion.identity;
                        rot.eulerAngles = m_Keys[i].eulerAngle;
                        Gizmos.DrawWireCube(m_Keys[i].position, Vector3.one*radius);
                        maxTime = Mathf.Max(m_Keys[i].time, maxTime);

                        if (m_Keys[i].bLookAt)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawWireCube(m_Keys[i].lookat, Vector3.one * radius);
                            Gizmos.color = bak;
                        }
                        

                        Gizmos.color = bak;
                    }
                }

                prePos = m_Keys[0].position;
                Vector3 preLookatPos = m_Keys[0].lookat;
                float time = 0f;
                while (time < maxTime)
                {
                    Vector3 pos = Vector3.zero, rot = Vector3.zero, lookat = Vector3.zero;
                    float f = 0f;
                    bool blookAt = false;
                    Evaluate(time, ref pos, ref rot, ref f, ref blookAt, ref lookat, false);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(prePos, pos);
                    Gizmos.color = bak;

                    if (blookAt)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(preLookatPos, lookat);
                        Gizmos.color = bak;
                    }


                    prePos = pos;
                    preLookatPos = lookat;
                    time += 0.1f;
                }
            }

        }
#endif
    }
}