/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	位置朝向曲线
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using Framework.Core;
namespace TopGame.Core
{
    public class TRSpline
    {
        [System.Serializable]
        public struct KeyFrame
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 outTan;
            public Vector3 inTan;
#if UNITY_EDITOR
            [System.NonSerialized]
            public bool expand;
#endif

            public static KeyFrame Epsilon = new KeyFrame() { position = Vector3.zero, rotation = Quaternion.identity, outTan = Vector3.zero, inTan = Vector3.zero };
        }

        private struct RuntimeFrame
        {
            public float time;
            public KeyFrame keyFrame;
        }
        //-----------------------------------------------------
        GameObject m_pTarget = null;

        private float m_fDelta = 0;
        private float m_fDuration = 0;
        List<RuntimeFrame> m_Keys = new List<RuntimeFrame>();
        //-----------------------------------------------------
        public bool IsValid()
        {
            return m_Keys.Count > 0;
        }
        //-----------------------------------------------------
        public void Clear()
        {
            m_pTarget = null;
            m_Keys.Clear();
            m_fDelta = 0;
            m_fDuration = 0;
        }
        //-----------------------------------------------------
        public void SetKeys(float speed, List<KeyFrame> keys)
        {
            if (keys == null || keys.Count <= 1 || speed<=0) return;
            m_Keys.Clear();

            RuntimeFrame frame = new RuntimeFrame();
            frame.keyFrame = keys[0];
            frame.time = 0;
            m_Keys.Add(frame);

            float fPathLength = 0f;
            for (int i = 1; i < keys.Count; i++)
            {
                KeyFrame vStart = keys[i - 1];
                KeyFrame vEnd = keys[i];
                float fPathSegmentLength = (vEnd.position - vStart.position).magnitude;
                if (fPathSegmentLength <= 0) continue;
                fPathLength += fPathSegmentLength;
                frame = new RuntimeFrame();
                frame.keyFrame = keys[i];
                frame.time = fPathLength / speed;
                m_Keys.Add(frame);
            }
            m_fDuration = fPathLength / speed;
        }
        //-----------------------------------------------------
        public KeyFrame GetLastKey()
        {
            if (m_Keys.Count <= 0) return KeyFrame.Epsilon;
            return m_Keys[m_Keys.Count - 1].keyFrame;
        }
        //-----------------------------------------------------
        public KeyFrame GetKeyByIndex(int index)
        {
            if (index < 0 || index >= m_Keys.Count) return KeyFrame.Epsilon;
            return m_Keys[index].keyFrame;
        }
        //-----------------------------------------------------
        public bool Update(Transform pTransform, float fTime)
        {
            if (m_Keys.Count <= 0 || pTransform== null) return false;
            m_fDelta += fTime;

            Vector3 retPos = pTransform.position;
            Quaternion retRotate = pTransform.rotation;
            if(Evaluate(m_fDelta,ref retPos, ref retRotate))
            {
                pTransform.position = retPos;
                pTransform.rotation = retRotate;
            }
            if (m_fDelta > m_fDuration)
                Clear();
            return m_fDelta <= m_fDuration;
        }
        //-----------------------------------------------------
        public bool Evaluate(float fTime, ref Vector3 retPos, ref Quaternion retRotate)
        {
            if (m_Keys.Count <= 0) return false;
            retPos = Vector3.zero;
            if (m_Keys.Count <= 0) return false;
            if (fTime <= m_Keys[0].time)
            {
                retRotate = m_Keys[0].keyFrame.rotation;
                retPos = m_Keys[0].keyFrame.position;
                return true;
            }
            if (fTime >= m_Keys[m_Keys.Count - 1].time)
            {
                retRotate = m_Keys[m_Keys.Count - 1].keyFrame.rotation;
                retPos = m_Keys[m_Keys.Count - 1].keyFrame.position;
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

                if (fTime < m_Keys[__middle].time)
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

            RuntimeFrame lhsKey = m_Keys[lhs];
            RuntimeFrame rhsKey = m_Keys[rhs];

            float dx = rhsKey.time - lhsKey.time;
            Vector3 m1 = Vector3.zero, m2 = Vector3.zero;
            float t;
            if (dx != 0f)
            {
                t = (fTime - lhsKey.time) / dx;
            }
            else
                t = 0;
            m1 = lhsKey.keyFrame.position + lhsKey.keyFrame.outTan;
            m2 = rhsKey.keyFrame.position + rhsKey.keyFrame.inTan;

            retPos = Framework.Core.BaseUtil.Bezier4(t, lhsKey.keyFrame.position, m1, m2, rhsKey.keyFrame.position);
            retRotate = Quaternion.Slerp(lhsKey.keyFrame.rotation, rhsKey.keyFrame.rotation, t);
#if UNITY_EDITOR
            if (UnityEditor.SceneView.lastActiveSceneView) UnityEditor.SceneView.lastActiveSceneView.Repaint();
#endif

            return true;
        }
#if UNITY_EDITOR
        //-----------------------------------------------------
        public void DrawLines()
        {
            if (m_Keys.Count<=0 || m_fDuration <=0) return;

            Color color = Handles.color;
            float width = Mathf.Max(1f, HandleUtility.GetHandleSize(Vector3.zero) * 0.1f);
            float fDelta = 0;

            Vector3 position = m_Keys[0].keyFrame.position;

            Handles.color = Color.yellow;
            while (fDelta <= m_fDuration)
            {
                Vector3 retPosition = Vector3.zero;
                Quaternion retRotation = Quaternion.identity;
                Evaluate(fDelta, ref retPosition, ref retRotation);
                Handles.DrawLine(position, retPosition);
                position = retPosition;
                fDelta += 0.033f;
            }
//             for (int i = 1; i < frames.Count; ++i)
//             {
//                 Handles.DrawBezier(frames[i - 1].position, frames[i].position, frames[i - 1].inTan, frames[i].inTan, Color.yellow, null, width);
//             }
            Handles.color = color;
        }
        //-----------------------------------------------------
        public void Editor(Transform pEditorTransform, List<KeyFrame> frames, float fSpeed)
        {
            if (pEditorTransform == null) return;
            UnityEngine.Event currentEvt = UnityEngine.Event.current;
            if (currentEvt != null)
            {
                GUILayout.BeginArea(new Rect(0, 0, Screen.width, 25));
                EditorGUI.BeginChangeCheck();
                m_fDelta = GUILayout.HorizontalSlider(m_fDelta, 0, m_fDuration);
                if(EditorGUI.EndChangeCheck())
                {
                    Vector3 retPosition = Vector3.zero;
                    Quaternion retRotation = Quaternion.identity;
                    if(Evaluate(m_fDelta, ref retPosition, ref retRotation))
                    {
                        pEditorTransform.position = retPosition;
                        pEditorTransform.rotation = retRotation;
                    }
                }
                if (GUILayout.Button("K帧", new GUILayoutOption[] { GUILayout.Width(80) }))
                {
                    KeyFrame newframe = new KeyFrame();
                    newframe.position = SceneView.lastActiveSceneView.camera.transform.position;
                    newframe.rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
                    pEditorTransform.position = newframe.position;
                    pEditorTransform.rotation = newframe.rotation;
                    frames.Add(newframe);
                    SetKeys(fSpeed,frames);
                }
                GUILayout.EndArea();
            }

            Color color = Handles.color;
            for (int i = 0; i < frames.Count; ++i)
            {
                KeyFrame frame = frames[i];

                bool bRayHit = true;
                Vector3 dir = Framework.Core.BaseUtil.EulersAngleToDirection(frame.rotation.eulerAngles);
                Vector3 lookatPos = Vector3.zero;
                float floorY = 0;
                if (Framework.Module.ModuleManager.mainModule != null)
                {
                    AFrameworkModule frameworkModule = Framework.Module.ModuleManager.GetMainFramework<AFrameworkModule>();
                    if(frameworkModule!=null) floorY = frameworkModule.GetPlayerPosition().y;
                }
                if (!Framework.Core.BaseUtil.RayInsectionFloor(out lookatPos, frame.position, dir, floorY))
                {
                    lookatPos = frame.position + dir * 20;
                    bRayHit = false;
                }

                Handles.color = Color.cyan;
                if (bRayHit)
                {
                    Handles.DrawLine(frame.position, lookatPos);
                    Handles.ArrowHandleCap(0, lookatPos, frame.rotation, 0.1f, EventType.Repaint);
                }
                Handles.color = color;

                if (currentEvt != null && currentEvt.control)
                {
                    if (bRayHit)
                    {
                        lookatPos = Handles.DoPositionHandle(lookatPos, Quaternion.identity);
                        Handles.Label(lookatPos, "视点[" + i.ToString() + "]");

                        EditorGUI.BeginChangeCheck();
                        frame.rotation = Quaternion.LookRotation((lookatPos - frame.position).normalized);
                        if (EditorGUI.EndChangeCheck())
                            SetKeys(fSpeed, frames);

                        pEditorTransform.position = frame.position;
                        pEditorTransform.rotation = frame.rotation;
                    }
                }
                else if (currentEvt != null && currentEvt.shift)
                {
                    //  HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                    EditorGUI.BeginChangeCheck();
                    frame.inTan = Handles.DoPositionHandle(frame.position + frame.inTan, Quaternion.identity) - frame.position;
                    frame.outTan = Handles.DoPositionHandle(frame.position + frame.outTan, Quaternion.identity) - frame.position;
                    if (EditorGUI.EndChangeCheck())
                        SetKeys(fSpeed, frames);

                    Handles.color = Color.red;
                    Handles.DrawLine(frame.position, frame.position + frame.inTan);
                    Handles.DrawLine(frame.position, frame.position + frame.outTan);
                    Handles.SphereHandleCap(0, frame.position + frame.inTan, Quaternion.identity, 0.1f, EventType.Repaint);
                    Handles.SphereHandleCap(0, frame.position + frame.outTan, Quaternion.identity, 0.1f, EventType.Repaint);
                    Handles.color = color;

                    Handles.SphereHandleCap(0, frame.position, Quaternion.identity, 0.2f, EventType.Repaint);
                }
                else
                {
                    EditorGUI.BeginChangeCheck();
                    frame.position = Handles.DoPositionHandle(frame.position, Quaternion.identity);
                    if(EditorGUI.EndChangeCheck())
                        SetKeys(fSpeed, frames);
                }


                Handles.Label(frame.position, "p[" + (i + 1) + "]");
                frames[i] = frame;
            }

            for (int i = 0; i < frames.Count; ++i)
            {
                KeyFrame frame = frames[i];
                Vector3 prevPos = frame.position;
                Vector3 nextPos = frame.position;
                Vector3 retPos = Vector3.zero;
                Quaternion retEuler = Quaternion.identity;
                if (i - 1 >= 0)
                {
                    prevPos = (frame.position + frames[i - 1].position) / 2;
                }
                if (i + 1 < frames.Count)
                {
                    nextPos = retPos;
                    nextPos = (frame.position + frames[i + 1].position) / 2;
                }

                Vector2 curuiPos = HandleUtility.WorldToGUIPoint(frame.position+Vector3.up*1);
                GUILayout.BeginArea(new Rect(curuiPos.x, curuiPos.y, 40, 25));
                if (GUILayout.Button("删除"))
                {
                    if (EditorUtility.DisplayDialog("提示", "删除这一帧", "删除", "取消"))
                    {
                        frames.RemoveAt(i);
                        SetKeys(fSpeed, frames);
                        break;
                    }
                }
                GUILayout.EndArea();

                if (i - 1 >= 0)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(prevPos + Vector3.up * 1);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y, 40, 25));
                    if (GUILayout.Button("插入"))
                    {
                        KeyFrame newframe = new KeyFrame();
                        newframe.position = (frame.position + frames[i - 1].position) / 2;
                        newframe.rotation = frames[i - 1].rotation;
                        frames.Add(newframe);
                        SetKeys(fSpeed, frames);
                        break;
                    }
                    GUILayout.EndArea();
                }
                if (i + 1 < frames.Count)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(nextPos + Vector3.up * 1);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y, 40, 25));
                    if (GUILayout.Button("插入"))
                    {
                        KeyFrame newframe = new KeyFrame();
                        newframe.position = (frame.position + frames[i + 1].position) / 2;
                        newframe.rotation = frames[i + 1].rotation;
                        frames.Add(newframe);
                        SetKeys(fSpeed, frames);
                    }
                    GUILayout.EndArea();
                }

                if (currentEvt != null && currentEvt.control)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(frame.position + Vector3.up * 1f);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y, 120, 50));
                    GUILayout.BeginVertical();
                    if (GUILayout.Button("节点参数覆盖到帧"))
                    {
                        frame.position = pEditorTransform.position;
                        frame.rotation = pEditorTransform.rotation;
                        frames[i] = frame;
                        SetKeys(fSpeed, frames);
                    }
                    if (GUILayout.Button("帧参数覆盖到节点"))
                    {
                        pEditorTransform.position = frame.position;
                        pEditorTransform.rotation = frame.rotation;
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndArea();
                }
            }
            Handles.color = color;
        }
#endif
    }
}