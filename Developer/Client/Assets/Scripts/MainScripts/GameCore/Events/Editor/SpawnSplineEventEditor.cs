#if UNITY_EDITOR
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Framework.Core;
using Framework.Logic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Core
{
    [Framework.Plugin.PluginEditorWindow("SpawnSplineEventEditor", "Edit")]
    public class SpawnSplineEventEditor : EditorWindow
    {
        public static SpawnSplineEventEditor current { get; private set; }
        //     [MenuItem("Tools/相机曲线K帧")]
        public static void EditFrame(Transform pTransform)
        {
            if (current != null)
            {
                current.SetEditorTarget(pTransform);
                current.OnEnable();
                return;
            }
            SpawnSplineEventEditor window = EditorWindow.GetWindow<SpawnSplineEventEditor>("曲线编辑", true);
            if (current != null) current.Close();
            current = window;
            current.m_SrcObject = null;
            current.m_ObjectFieldSpawnData = null;
            current.SetEditorTarget(pTransform);
            GUI.FocusControl("ClearAllFocus");
        }
        public static void Edit(SpawnSplineData spawnData, System.Object pData, System.Reflection.FieldInfo fieldData, Transform pTransform = null)
        {
            if (pData == null) return;
            SpawnSplineEventEditor window = EditorWindow.GetWindow<SpawnSplineEventEditor>("曲线编辑", true);
            if (current != null) current.Close();
            current = window;
            current.m_SpawnData = spawnData;
            current.m_SrcObject = pData;
            current.m_ObjectFieldSpawnData = fieldData;
            current.SetFrames(spawnData.Frames);
            current.SetEditorTarget(pTransform);
            GUI.FocusControl("ClearAllFocus");
        }

        System.Object m_SrcObject = null;
        System.Reflection.FieldInfo m_ObjectFieldSpawnData;

        SpawnSplineData m_SpawnData = new SpawnSplineData();

        List<SpawnSplineData.KeyFrame> m_vFrames = new List<SpawnSplineData.KeyFrame>();
        Vector2 m_Scroll = Vector2.zero;
        float m_fTestTime = 0;
        float m_fDurationTime = 0;
        bool m_bPlaying = false;

        private float m_PreviousTime;
        public float deltaTime = 0.02f;
        public float fixedDeltaTime = 0.02f;
        protected float m_fDeltaTime = 0f;
        protected float m_currentSnap = 1f;

        Transform m_pTarget = null;
        Vector3 m_BackupPosition = Vector3.zero;
        Quaternion m_BackupRotation = Quaternion.identity;
        Vector3 m_BackupScale = Vector3.one;

        int m_nCopyFrameId = -1;
        SpawnSplineData.KeyFrame m_pCopyFrame;
        Vector3 m_TargetPosition = Vector3.zero;
        //------------------------------------------------------
        public static Transform GetTarget()
        {
            if (current == null) return null;
            return current.m_pTarget;
        }
        //------------------------------------------------------
        public float GetMaxTime()
        {
            float fMax = 0;
            for (int i = 0; i < m_vFrames.Count; ++i)
                fMax = Mathf.Max(fMax, m_vFrames[i].time);
            return fMax;
        }
        //------------------------------------------------------
        void SetFrames(SpawnSplineData.KeyFrame[] frames)
        {
            m_vFrames.Clear();
            if (frames == null) return;
            for (int i = 0; i < frames.Length; ++i)
            {
                SpawnSplineData.KeyFrame kF = frames[i];
                kF.editorTime = frames[i].time;
                m_vFrames.Add(kF);
            }
            Framework.Plugin.SortUtility.QuickSortUp<SpawnSplineData.KeyFrame>(ref m_vFrames, 0);
        }
        //------------------------------------------------------
        void SetEditorTarget(Transform pTransform)
        {
            if (pTransform == m_pTarget) return;
            if (m_pTarget != null)
            {
                m_pTarget.position = m_BackupPosition;
                m_pTarget.rotation = m_BackupRotation;
                m_pTarget.localScale = m_BackupScale;
            }
            m_pTarget = pTransform;
            if (m_pTarget)
            {
                m_BackupPosition = m_pTarget.position;
                m_BackupRotation = m_pTarget.rotation;
                m_BackupScale = m_pTarget.localScale;
            }
        }
        //------------------------------------------------------
        SpawnSplineData SaveCatch(ref SpawnSplineData data)
        {
            string strTempFile = Application.dataPath + "/../EditorData/TempSplineTrack.json";
            if (!Directory.Exists(Application.dataPath + "/../EditorData/"))
                Directory.CreateDirectory(Application.dataPath + "/../EditorData");
            if (File.Exists(strTempFile))
                File.Delete(strTempFile);
            if (m_vFrames.Count > 0)
            {
                data.Frames = m_vFrames.ToArray();
            }


            FileStream fs = new FileStream(strTempFile, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(JsonUtility.ToJson(data, true));
            writer.Close();
            return data;
        }
        //------------------------------------------------------
        public static bool CatchCopyCatchSplineDatas()
        {
            string strTempFile = Application.dataPath + "/../EditorData/TempSplineTrack.json";
            return File.Exists(strTempFile);
        }
        //------------------------------------------------------
        public static bool CopyCatchSplineDatas(ref SpawnSplineData data)
        {
            string strTempFile = Application.dataPath + "/../EditorData/TempSplineTrack.json";
            if (!File.Exists(strTempFile)) return false;
            try
            {
                data = JsonUtility.FromJson<SpawnSplineData>(File.ReadAllText(strTempFile));
                return true;

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
                return false;
            }
        }
        //------------------------------------------------------
        public static SpawnSplineEventParameter BulidCopyCatchSplineEvent()
        {
            SpawnSplineData data = new SpawnSplineData();
            if (CopyCatchSplineDatas(ref data)) return null;
            SpawnSplineEventParameter evet = new SpawnSplineEventParameter();
            evet.spawnData = data;
            return evet;
        }
        //------------------------------------------------------
        private void OnGUI()
        {
            m_SpawnData.startAction = EditorGUILayout.TextField("出生动作", m_SpawnData.startAction);
            m_SpawnData.loopAction = EditorGUILayout.TextField("循环动作", m_SpawnData.loopAction);
            m_SpawnData.endAction = EditorGUILayout.TextField("结束动作", m_SpawnData.endAction);
            if (m_SpawnData.speedCurve == null) m_SpawnData.speedCurve = new AnimationCurve();
            m_SpawnData.speedCurve = EditorGUILayout.CurveField("速度曲线", m_SpawnData.speedCurve);

            Transform editorTarget = EditorGUILayout.ObjectField("作用目标", m_pTarget, typeof(Transform), true) as Transform;
            SetEditorTarget(editorTarget);
            GUILayout.BeginHorizontal();
            if (m_pTarget != null)
            {
                m_fTestTime = EditorGUILayout.FloatField("时间", m_fTestTime);
                if (GUILayout.Button("K帧", new GUILayoutOption[] { GUILayout.Width(120) }))
                {
                    for (int i = 0; i < m_vFrames.Count; ++i)
                    {
                        if (Mathf.Abs(m_vFrames[i].time - m_fTestTime) <= 0.001f)
                        {
                            m_vFrames.RemoveAt(i);
                            break;
                        }
                    }

                    SpawnSplineData.KeyFrame frame = new SpawnSplineData.KeyFrame();
                    frame.time = m_fTestTime;
                    frame.editorTime = m_fTestTime;
                    frame.position = m_pTarget.position - m_BackupPosition;
                    frame.eulerAngle = m_pTarget.eulerAngles;
                    frame.scale = m_pTarget.localScale;
                    m_vFrames.Add(frame);
                    Framework.Plugin.SortUtility.QuickSortUp<SpawnSplineData.KeyFrame>(ref m_vFrames);
                }
                if (m_SrcObject != null && m_ObjectFieldSpawnData != null)
                {
                    if (m_vFrames.Count > 1 && GUILayout.Button("应用", new GUILayoutOption[] { GUILayout.Width(50) }))
                    {
                        SaveCatch(ref m_SpawnData);
                        m_ObjectFieldSpawnData.SetValue(m_SrcObject, m_SpawnData);
                    }
                }

                if (CatchCopyCatchSplineDatas() && GUILayout.Button("黏贴", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    SpawnSplineData spawnData = new SpawnSplineData();
                    if (CopyCatchSplineDatas(ref spawnData))
                    {
                        SetFrames(spawnData.Frames);
                    }
                }
                if (GUILayout.Button("逆序", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    SpawnSplineData.KeyFrame[] keys = m_vFrames.ToArray();
                    m_vFrames.Reverse();
                    for (int i = 0; i < keys.Length; ++i)
                    {
                        SpawnSplineData.KeyFrame f = m_vFrames[i];
                        f.time = keys[i].time;
                        m_vFrames[i] = f;
                    }
                }
            }
            GUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();
            m_fDurationTime = EditorGUILayout.Slider(m_fDurationTime, 0, GetMaxTime());
            if (GUILayout.Button(m_bPlaying ? "停止" : "播放"))
            {
                m_bPlaying = !m_bPlaying;
                if (!m_bPlaying) m_fDurationTime = 0;
            }
            GUILayout.EndHorizontal();
            if (!m_bPlaying && EditorGUI.EndChangeCheck())
            {
                m_fTestTime = m_fDurationTime;
            }
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);

            for (int i = 0; i < m_vFrames.Count; ++i)
            {
                SpawnSplineData.KeyFrame frame = m_vFrames[i];
                GUILayout.BeginHorizontal();
                frame.bExpand = EditorGUILayout.Foldout(frame.bExpand, "第" + (i + 1) + "帧[" + m_vFrames[i].time + "]");
                if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    m_vFrames.RemoveAt(i);
                    break;
                }
                if (GUILayout.Button("复制", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    m_nCopyFrameId = i;
                    m_pCopyFrame = frame;
                }
                GUILayout.EndHorizontal();
                if (frame.bExpand)
                {
                    GUILayout.BeginHorizontal();
                    frame.editorTime = EditorGUILayout.FloatField(frame.editorTime);
                    if (frame.editorTime != frame.time && GUILayout.Button("应用", new GUILayoutOption[] { GUILayout.Width(40) }))
                    {
                        float fGap = frame.editorTime - frame.time;
                        frame.time = frame.editorTime;
                        for (int f = i; f < m_vFrames.Count; ++f)
                        {
                            SpawnSplineData.KeyFrame afterF = m_vFrames[f];
                            afterF.time += fGap;
                            m_vFrames[f] = afterF;
                        }
                    }
                    GUILayout.EndHorizontal();
                    frame.position = EditorGUILayout.Vector3Field("位置", frame.position);
                    frame.eulerAngle = EditorGUILayout.Vector3Field("朝向", frame.eulerAngle);
                    frame.scale = EditorGUILayout.Vector3Field("缩放", frame.scale);
                }
                m_vFrames[i] = frame;
            }

            EditorGUILayout.EndScrollView();
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            SetEditorTarget(null);
            SceneView.duringSceneGui -= OnSceneFunc;
            EditorApplication.update -= EditorUpdate;
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneFunc;
            EditorApplication.update += EditorUpdate;
        }
        //-----------------------------------------------------
        private void OnSceneGUI(SceneView sceneView)
        {
            if (m_vFrames == null) return;
            DrawDebug(m_BackupPosition);
            Color color = Handles.color;
            UnityEngine.Event currentEvt = UnityEngine.Event.current;
            for (int i = 0; i < m_vFrames.Count; ++i)
            {
                SpawnSplineData.KeyFrame frame = m_vFrames[i];

                if (currentEvt != null && currentEvt.shift && currentEvt.control)
                {
                    //  HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                    frame.inTan = Handles.DoPositionHandle(frame.position + m_BackupPosition + frame.inTan, Quaternion.identity) - frame.position - m_BackupPosition;
                    frame.outTan = Handles.DoPositionHandle(frame.position + m_BackupPosition + frame.outTan, Quaternion.identity) - frame.position - m_BackupPosition;

                    Handles.color = Color.red;
                    Handles.SphereHandleCap(0, frame.position + m_BackupPosition + frame.inTan, Quaternion.identity, 0.1f, EventType.Repaint);
                    Handles.SphereHandleCap(0, frame.position + m_BackupPosition + frame.outTan, Quaternion.identity, 0.1f, EventType.Repaint);
                    Handles.color = color;

                    Handles.SphereHandleCap(0, frame.position + m_BackupPosition, Quaternion.identity, 0.2f, EventType.Repaint);
                }
                else if (currentEvt != null && currentEvt.shift)
                {
                    if (Tools.current == Tool.Move)
                        frame.position = Handles.DoPositionHandle(frame.position + m_BackupPosition, Quaternion.identity) - m_BackupPosition;
                    else if (Tools.current == Tool.Rotate)
                        frame.eulerAngle = Handles.DoRotationHandle(Quaternion.Euler(frame.eulerAngle), frame.position + m_BackupPosition).eulerAngles;
                }

                Handles.Label(frame.position + m_BackupPosition, i.ToString() + "[" + frame.time + "]");
                m_vFrames[i] = frame;
            }

            if (m_pTarget)
            {
                if (currentEvt == null || !currentEvt.shift)
                {
                    if (Tools.current == Tool.Move)
                        m_pTarget.position = Handles.DoPositionHandle(m_pTarget.position, Quaternion.identity);
                    else if (Tools.current == Tool.Rotate)
                        m_pTarget.rotation = Handles.DoRotationHandle(m_pTarget.rotation, m_pTarget.position);
                    else if (Tools.current == Tool.Scale)
                        m_pTarget.localScale = Handles.DoScaleHandle(m_pTarget.localScale, m_pTarget.position, m_pTarget.rotation, HandleUtility.GetHandleSize(m_pTarget.position));
                }
            }



            for (int i = 0; i < m_vFrames.Count; ++i)
            {
                SpawnSplineData.KeyFrame frame = m_vFrames[i];
                Vector3 prevPos = frame.position;
                Vector3 nextPos = frame.position;
                Vector3 retPos = Vector3.zero;
                Vector3 retEuler = Vector3.zero;
                Vector3 retScale = Vector3.one;
                if (i - 1 >= 0 && Evaluate((frame.time + m_vFrames[i - 1].time) / 2, ref retPos, ref retEuler, ref retScale))
                {
                    prevPos = retPos;
                }
                if (i + 1 < m_vFrames.Count && Evaluate((frame.time + m_vFrames[i + 1].time) / 2, ref retPos, ref retEuler, ref retScale))
                {
                    nextPos = retPos;
                }

                Vector2 curuiPos = HandleUtility.WorldToGUIPoint(frame.position + m_BackupPosition);
                GUILayout.BeginArea(new Rect(curuiPos.x, curuiPos.y, 40, 25));
                if (GUILayout.Button("删除"))
                {
                    if (EditorUtility.DisplayDialog("提示", "删除这一帧", "删除", "取消"))
                    {
                        m_vFrames.RemoveAt(i);
                        break;
                    }
                }
                GUILayout.EndArea();

                if (i - 1 >= 0 && currentEvt != null && currentEvt.shift)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(prevPos + m_BackupPosition);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y, 40, 25));
                    if (GUILayout.Button("插入"))
                    {
                        float insert = (frame.time + m_vFrames[i - 1].time) / 2;
                        if (Evaluate(insert, ref retPos, ref retEuler, ref retScale))
                        {
                            SpawnSplineData.KeyFrame newframe = new SpawnSplineData.KeyFrame();
                            newframe.time = insert;
                            newframe.editorTime = insert;
                            newframe.position = retPos;
                            newframe.eulerAngle = retEuler;
                            m_vFrames.Insert(i, newframe);

                            m_fDurationTime = insert;
                            m_fTestTime = insert;
                            break;
                        }
                    }
                    GUILayout.EndArea();
                }
                if (i + 1 < m_vFrames.Count && currentEvt != null && currentEvt.shift)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(nextPos + m_BackupPosition);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y + 25, 40, 25));
                    if (GUILayout.Button("插入"))
                    {
                        float insert = (frame.time + m_vFrames[i + 1].time) / 2;
                        if (Evaluate(insert, ref retPos, ref retEuler, ref retScale))
                        {
                            SpawnSplineData.KeyFrame newframe = new SpawnSplineData.KeyFrame();
                            newframe.time = insert;
                            newframe.editorTime = insert;
                            newframe.position = retPos;
                            newframe.eulerAngle = retEuler;
                            m_vFrames.Insert(i + 1, newframe);

                            m_fDurationTime = insert;
                            m_fTestTime = insert;
                            break;
                        }
                    }
                    GUILayout.EndArea();
                }

                if (currentEvt != null && currentEvt.control)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(frame.position + Vector3.up * 1f + m_BackupPosition);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y, 120, 50));
                    if (GUILayout.Button("目标同步帧"))
                    {
                        frame.position = m_pTarget.position - m_BackupPosition;
                        frame.eulerAngle = m_pTarget.eulerAngles;
                        frame.scale = m_pTarget.localScale;
                        m_vFrames[i] = frame;
                    }
                    if (GUILayout.Button("帧同步目标"))
                    {
                        m_pTarget.position = frame.position + m_BackupPosition;
                        m_pTarget.eulerAngles = frame.eulerAngle;
                        m_pTarget.localScale = frame.scale;
                    }
                    GUILayout.EndArea();
                }
            }
        }
        //------------------------------------------------------
        private void Update()
        {

        }
        //------------------------------------------------------
        void DrawDebug(Vector3 position)
        {
            if (m_vFrames == null || m_vFrames.Count <= 0) return;
            Vector3 prePos = Vector3.zero;
            float maxTime = GetMaxTime();
            Color bak = UnityEditor.Handles.color;
            prePos = m_vFrames[0].position;
            float time = 0f;
            while (time < maxTime)
            {
                Vector3 pos = Vector3.zero;
                Vector3 rot = Vector3.zero;
                Vector3 scale = Vector3.one;
                Evaluate(time, ref pos, ref rot, ref scale);

                UnityEditor.Handles.color = Color.blue;
                UnityEditor.Handles.DrawLine(prePos + position, pos + position);
                UnityEditor.Handles.color = bak;

                prePos = pos;
                time += 0.01f;
            }
        }
        //------------------------------------------------------
        bool Evaluate(float fTime, ref Vector3 retPos, ref Vector3 retEuler, ref Vector3 retScale)
        {
            if (m_vFrames == null || m_vFrames.Count <= 0) return false;
            if (fTime <= m_vFrames[0].time)
            {
                retPos = m_vFrames[0].position;
                retEuler = m_vFrames[0].eulerAngle;
                return true;
            }
            if (fTime >= m_vFrames[m_vFrames.Count - 1].time)
            {
                retPos = m_vFrames[m_vFrames.Count - 1].position;
                retEuler = m_vFrames[m_vFrames.Count - 1].eulerAngle;
                return true;
            }
            int __len = m_vFrames.Count;
            int __half;
            int __middle;
            int __first = 0;
            while (__len > 0)
            {
                __half = __len >> 1;
                __middle = __first + __half;

                if (fTime < m_vFrames[__middle].time)
                    __len = __half;
                else
                {
                    __first = __middle;
                    ++__first;
                    __len = __len - __half - 1;
                }
            }

            int lhs = __first - 1;
            int rhs = Mathf.Min(m_vFrames.Count - 1, __first);

            if (lhs < 0 || lhs >= m_vFrames.Count || rhs < 0 || rhs >= m_vFrames.Count)
                return false;

            SpawnSplineData.KeyFrame lhsKey = m_vFrames[lhs];
            SpawnSplineData.KeyFrame rhsKey = m_vFrames[rhs];

            float dx = rhsKey.time - lhsKey.time;
            Vector3 m1 = Vector3.zero, m2 = Vector3.zero;
            float t;
            if (dx != 0f)
            {
                t = (fTime - lhsKey.time) / dx;
            }
            else
                t = 0;

            m1 = lhsKey.position + lhsKey.outTan;
            m2 = rhsKey.position + rhsKey.inTan;
            retPos = Framework.Core.CommonUtility.Bezier4(t, lhsKey.position, m1, m2, rhsKey.position);
            retEuler = Vector3.Slerp(lhsKey.eulerAngle, rhsKey.eulerAngle, t);
            retScale = Vector3.Slerp(lhsKey.eulerAngle, rhsKey.eulerAngle, t);
            return true;
        }
        //------------------------------------------------------
        void UpdateTransform(float fTime)
        {
            if (m_pTarget)
            {
                Vector3 retPos = Vector3.zero;
                Vector3 retEuler = Vector3.zero;
                Vector3 retScale = Vector3.one;
                if (Evaluate(fTime, ref retPos, ref retEuler, ref retScale))
                {
                    m_pTarget.position = retPos + m_BackupPosition;
                    m_pTarget.eulerAngles = retEuler;
                    m_pTarget.localScale = retScale;
                    if (SceneView.lastActiveSceneView)
                        SceneView.lastActiveSceneView.Repaint();
                }
            }
        }
        //------------------------------------------------------
        void EditorUpdate()
        {
            if (Application.isPlaying)
            {
                Application.targetFrameRate = 30;
                deltaTime = Time.deltaTime;
                m_fDeltaTime = (float)(deltaTime * m_currentSnap);
            }
            else
            {
                float curTime = Time.realtimeSinceStartup;
                m_PreviousTime = Mathf.Min(m_PreviousTime, curTime);//very important!!!

                deltaTime = curTime - m_PreviousTime;
                m_fDeltaTime = (float)(deltaTime * m_currentSnap);
            }

            m_PreviousTime = Time.realtimeSinceStartup;

            if (m_bPlaying)
            {
                float fSpeed = 1;
                if (m_SpawnData.speedCurve != null && Framework.Core.CommonUtility.IsValidCurve(m_SpawnData.speedCurve))
                    fSpeed = m_SpawnData.speedCurve.Evaluate(m_fDurationTime);

                m_fDurationTime += deltaTime * fSpeed;
                UpdateTransform(m_fDurationTime);
                if (m_fDurationTime >= GetMaxTime())
                {
                    m_fDurationTime = 0;
                    m_bPlaying = false;
                }
            }
            this.Repaint();
        }
        //-----------------------------------------------------
        static public void OnSceneFunc(SceneView sceneView)
        {
            if (current != null)
                current.OnSceneGUI(sceneView);
        }
    }
}

#endif