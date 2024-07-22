#if UNITY_EDITOR
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Framework.Core;
using Framework.Base;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Core
{
    public class CameraSplineEventEditor : EditorWindow
    {
        [System.Serializable]
        public class CameraSplineCatchData
        {
            public uint useFlag;
            public CameraSplineEventParameter.EType splineType;
            public List<CurveData> datas;
        }
        public static CameraSplineEventEditor current { get; private set; }
        static CameraSplineEventParameter mCurrentData;
        static CameraSplineEventParameter mpEventSrcData = null;
        //     [MenuItem("Tools/相机曲线K帧")]
        public static void EditFrame(Camera pCamera)
        {
            if (current != null)
            {
                if (pCamera)
                {
                    current.m_LockCameraPosition = pCamera.transform.position;
                    current.m_LockCameraRotation = pCamera.transform.rotation;
                    current.m_LockFov = pCamera.fieldOfView;
                }
                current.OnEnable();
                return;
            }
            mCurrentData = new CameraSplineEventParameter();
            mpEventSrcData = null;
            CameraSplineEventEditor window = EditorWindow.GetWindow<CameraSplineEventEditor>("相机曲线编辑", true);
            if (current != null) current.Close();
            current = window;
            if (pCamera)
            {
                current.m_LockCameraPosition = pCamera.transform.position;
                current.m_LockCameraRotation = pCamera.transform.rotation;
                current.m_LockFov = pCamera.fieldOfView;
            }
            GUI.FocusControl("ClearAllFocus");
        }
        public static void Edit(CameraSplineEventParameter pData)
        {
            if (pData == null) return;
            mCurrentData = new CameraSplineEventParameter();
            mCurrentData.Copy(pData);
            mpEventSrcData = pData;
            CameraSplineEventEditor window = EditorWindow.GetWindow<CameraSplineEventEditor>("相机曲线编辑", true);
            if (current != null) current.Close();
            current = window;
            current.m_pSpline.usedFlag = pData.useFlags;
            current.m_pSpline.bAbsCalc = (pData.splineType == CameraSplineEventParameter.EType.ModeSet);
            if (CameraController.getInstance() != null)
            {
                current.m_pCamera = CameraController.getInstance().GetCamera();
                if (current.m_pCamera)
                {
                    current.m_LockCameraPosition = current.m_pCamera.transform.position;
                    current.m_LockCameraRotation = current.m_pCamera.transform.rotation;
                    current.m_LockFov = current.m_pCamera.fieldOfView;

                    if (mCurrentData.splineType == CameraSplineEventParameter.EType.ModeSet)
                    {
                        for (int i = 0; i < mCurrentData.KeyFrames.Count; ++i)
                        {
                            CurveData data = mCurrentData.KeyFrames[i];
                            data.position += current.m_LockCameraPosition;
                            data.rotation = Quaternion.Euler(data.rotation.eulerAngles + current.m_LockCameraRotation.eulerAngles);
                            data.fov += current.m_LockFov;
                            mCurrentData.KeyFrames[i] = data;
                        }
                    }
                }
            }
            GUI.FocusControl("ClearAllFocus");
        }

        CameraSpline m_pSpline = new CameraSpline();
        Vector2 m_Scroll = Vector2.zero;
        Camera m_pCamera = null;
        Transform m_Lookat = null;
        float m_fTestTime = 0;
        float m_fDurationTime = 0;
        bool m_bPlaying = false;


        private float m_PreviousTime;
        public float deltaTime = 0.02f;
        public float fixedDeltaTime = 0.02f;
        protected float m_fDeltaTime = 0f;
        protected float m_currentSnap = 1f;

        Vector3 m_LockCameraPosition = Vector3.zero;
        Quaternion m_LockCameraRotation = Quaternion.identity;
        float m_LockFov = 0;

        int m_nCopyFrameId = -1;
        CurveData m_pCopyFrame;
        Vector3 m_TargetPosition = Vector3.zero;
        //------------------------------------------------------
        CameraSplineCatchData SaveCatch(CameraSplineEventParameter.EType splineType)
        {
            if (m_pSpline.Frames.Count <= 0) return null;
            string strTempFile = Application.dataPath + "/../EditorData/TempCameraSpline.json";
            if (!Directory.Exists(Application.dataPath + "/../EditorData/"))
                Directory.CreateDirectory(Application.dataPath + "/../EditorData");
            if (File.Exists(strTempFile))
                File.Delete(strTempFile);
            CameraSplineCatchData ff = new CameraSplineCatchData();
            ff.useFlag = m_pSpline.usedFlag;
            ff.splineType = splineType;
            ff.datas = new List<CurveData>(m_pSpline.Frames.ToArray());
            if (splineType != CameraSplineEventParameter.EType.ModeSet)
            {
                for (int i = 0; i < ff.datas.Count; ++i)
                {
                    CurveData frame = ff.datas[i];
                    frame.position -= m_LockCameraPosition;
                    frame.rotation = Quaternion.Euler(frame.rotation.eulerAngles - m_LockCameraRotation.eulerAngles);
                    frame.fov -= m_LockFov;
                    ff.datas[i] = frame;
                }
            }

            FileStream fs = new FileStream(strTempFile, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(JsonUtility.ToJson(ff, true));
            writer.Close();
            return ff;
        }
        //------------------------------------------------------
        public static bool CatchCopyCatchSplineDatas()
        {
            string strTempFile = Application.dataPath + "/../EditorData/TempCameraSpline.json";
            return File.Exists(strTempFile);
        }
        //------------------------------------------------------
        public static CameraSplineCatchData CopyCatchSplineDatas()
        {
            string strTempFile = Application.dataPath + "/../EditorData/TempCameraSpline.json";
            if (!File.Exists(strTempFile)) return null;
            try
            {
                CameraSplineCatchData data = JsonUtility.FromJson<CameraSplineCatchData>(File.ReadAllText(strTempFile));
                return data;

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
                return null;
            }
        }
        //------------------------------------------------------
        public static CameraSplineEventParameter BulidCopyCatchSplineEvent()
        {
            CameraSplineCatchData catchData = CopyCatchSplineDatas();
            if (catchData == null || catchData.datas == null || catchData.datas.Count <= 0) return null;
            CameraSplineEventParameter evet = new CameraSplineEventParameter();
            evet.KeyFrames = catchData.datas;
            evet.useFlags = (ushort)catchData.useFlag;
            evet.splineType = (CameraSplineEventParameter.EType)catchData.splineType;
            return evet;
        }
        //------------------------------------------------------
        private void OnGUI()
        {
            System.Reflection.FieldInfo filed = m_pSpline.GetType().GetField("m_nFlags", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            // Data.DrawHelper.DrawProperty("使用标志位", m_pSpline, filed);
            Framework.ED.HandleUtilityWrapper.DrawPropertyField(m_pSpline, filed);

            m_pCamera = EditorGUILayout.ObjectField("K帧相机", m_pCamera, typeof(Camera)) as Camera;
            if (m_pCamera == null)
            {
                if (GameInstance.getInstance().cameraController != null)
                    m_pCamera = GameInstance.getInstance().cameraController.GetCamera();
                if (m_pCamera == null) m_pCamera = GameObject.FindObjectOfType<Camera>();
            }
            m_Lookat = EditorGUILayout.ObjectField("指定目标点", m_Lookat, typeof(Transform)) as Transform;
            GUILayout.BeginHorizontal();
            if (m_pCamera != null)
            {
                m_fTestTime = EditorGUILayout.FloatField("时间", m_fTestTime);
                if (GUILayout.Button("K帧", new GUILayoutOption[] { GUILayout.Width(120) }))
                {
                    for (int i = 0; i < m_pSpline.Frames.Count; ++i)
                    {
                        if (Mathf.Abs(m_pSpline.Frames[i].time - m_fTestTime) <= 0.001f)
                        {
                            m_pSpline.Frames.RemoveAt(i);
                            break;
                        }
                    }
                    if (m_Lookat)
                    {
                        m_pCamera.transform.LookAt(m_Lookat);
                    }
                    CurveData frame = new CurveData();
                    frame.time = m_fTestTime;
                    frame.position = m_pCamera.transform.position;
                    frame.rotation = m_pCamera.transform.rotation;
                    frame.fov = m_pCamera.fieldOfView;
                    m_pSpline.Frames.Add(frame);
                    List<CurveData> vList = m_pSpline.Frames;
                    Framework.Plugin.SortUtility.QuickSortUp<CurveData>(ref vList);
                }
                if (mpEventSrcData != null)
                {
                    if (m_pSpline.Frames.Count > 1 && GUILayout.Button("应用", new GUILayoutOption[] { GUILayout.Width(50) }))
                    {
                        CameraSplineCatchData catchData = SaveCatch(mpEventSrcData.splineType);
                        if (catchData != null)
                        {
                            mpEventSrcData.KeyFrames = new List<CurveData>(catchData.datas.ToArray());
                            mpEventSrcData.useFlags = (ushort)catchData.useFlag;
                            mpEventSrcData.splineType = (CameraSplineEventParameter.EType)catchData.splineType;
                        }
                    }
                }
                else
                {
                    if (m_pSpline.Frames.Count > 1 && GUILayout.Button("绝对复制", new GUILayoutOption[] { GUILayout.Width(100) }))
                    {
                        SaveCatch(CameraSplineEventParameter.EType.ModeSet);
                    }
                    if (m_pSpline.Frames.Count > 1 && GUILayout.Button("相对复制", new GUILayoutOption[] { GUILayout.Width(100) }))
                    {
                        SaveCatch(CameraSplineEventParameter.EType.LockSet);
                    }
                }

                if (mCurrentData != null && CatchCopyCatchSplineDatas() && GUILayout.Button("黏贴", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    CameraSplineCatchData catchData = CopyCatchSplineDatas();
                    if (catchData != null)
                    {
                        mCurrentData.KeyFrames = catchData.datas;
                        m_pSpline.usedFlag = catchData.useFlag;
                        m_pSpline.bAbsCalc = catchData.splineType == CameraSplineEventParameter.EType.ModeSet;
                        mCurrentData.splineType = catchData.splineType;
                        if (!m_pSpline.bAbsCalc)
                        {
                            for (int i = 0; i < mCurrentData.KeyFrames.Count; ++i)
                            {
                                CurveData data = mCurrentData.KeyFrames[i];
                                data.position += current.m_LockCameraPosition;
                                data.rotation = Quaternion.Euler(data.rotation.eulerAngles + current.m_LockCameraRotation.eulerAngles);
                                data.fov += current.m_LockFov;
                                mCurrentData.KeyFrames[i] = data;
                            }
                        }
                    }
                    m_pSpline.Frames = mCurrentData.KeyFrames;
                }
            }
            GUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();
            m_fDurationTime = EditorGUILayout.Slider(m_fDurationTime, 0, m_pSpline.GetMaxTime());
            if (GUILayout.Button(m_bPlaying ? "停止" : "播放"))
            {
                m_bPlaying = !m_bPlaying;
                if (!m_bPlaying) m_fDurationTime = 0;
            }
            GUILayout.EndHorizontal();
            if (!m_bPlaying && EditorGUI.EndChangeCheck())
            {
                m_fTestTime = m_fDurationTime;
                UpdateCamera(m_fDurationTime);
            }
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);

            for (int i = 0; i < m_pSpline.Frames.Count; ++i)
            {
                CurveData frame = m_pSpline.Frames[i];
                GUILayout.BeginHorizontal();
                frame.bExpand = EditorGUILayout.Foldout(frame.bExpand, "第" + (i + 1) + "帧[" + m_pSpline.Frames[i].time + "]");
                if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    m_pSpline.Frames.RemoveAt(i);
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
                        for (int f = i; f < m_pSpline.Frames.Count; ++f)
                        {
                            CurveData afterF = m_pSpline.Frames[f];
                            afterF.time += fGap;
                            m_pSpline.Frames[f] = afterF;
                        }
                    }
                    GUILayout.EndHorizontal();
                    EditorGUI.indentLevel++;
                    if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.Position))
                    {
                        GUILayout.BeginHorizontal();
                        frame.position = EditorGUILayout.Vector3Field("位置", frame.position);
                        if (m_nCopyFrameId != i && GUILayout.Button("黏贴"))
                        {
                            frame.position = m_pCopyFrame.position;
                        }
                        GUILayout.EndHorizontal();
                    }


                    if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.LookAt))
                    {
                        GUILayout.BeginHorizontal();
                        frame.lookatTrans = EditorGUILayout.ObjectField("lookat", frame.lookatTrans, typeof(Transform)) as Transform;
                        if (m_nCopyFrameId != i && GUILayout.Button("黏贴"))
                        {
                            frame.lookatTrans = m_pCopyFrame.lookatTrans;
                        }
                        GUILayout.EndHorizontal();

                        if (frame.lookatTrans)
                        {
                            frame.rotation.eulerAngles = Framework.Core.CommonUtility.DirectionToEulersAngle((frame.lookatTrans.position - frame.position).normalized);
                            frame.lookat = frame.lookatTrans.position;
                        }
                    }
                    else if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.Rotation))
                    {
                        GUILayout.BeginHorizontal();
                        frame.rotation.eulerAngles = EditorGUILayout.Vector3Field("朝向", frame.rotation.eulerAngles);
                        if (m_nCopyFrameId != i && GUILayout.Button("黏贴"))
                        {
                            frame.rotation = m_pCopyFrame.rotation;
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.Fov))
                        frame.fov = EditorGUILayout.FloatField("广角", frame.fov);
                    EditorGUI.indentLevel--;
                }
                m_pSpline.Frames[i] = frame;
            }

            EditorGUILayout.EndScrollView();
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneFunc;
            EditorApplication.update -= EditorUpdate;
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            if (mCurrentData != null)
                m_pSpline.Frames = new List<CurveData>(mCurrentData.KeyFrames.ToArray());
            else
                m_pSpline.Frames = new List<CurveData>();
            SceneView.duringSceneGui += OnSceneFunc;

            EditorApplication.update += EditorUpdate;
        }
        //-----------------------------------------------------
        private void OnSceneGUI(SceneView sceneView)
        {
            if (m_pSpline.Frames == null) return;
            m_pSpline.DrawDebug();
            Color color = Handles.color;
            UnityEngine.Event currentEvt = UnityEngine.Event.current;
            for (int i = 0; i < m_pSpline.Frames.Count; ++i)
            {
                CurveData frame = m_pSpline.Frames[i];

                bool bRayHit = true;
                Vector3 dir = Framework.Core.CommonUtility.EulersAngleToDirection(frame.rotation.eulerAngles);
                Vector3 lookatPos = Vector3.zero;
                if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.LookAt))
                {
                    lookatPos = frame.lookat;
                }
                else
                {
                    float floorY = 0;
                    if (Framework.Module.ModuleManager.mainModule != null)
                    {
                        AFrameworkModule framework = Framework.Module.ModuleManager.GetMainFramework<AFrameworkModule>();
                        if(framework!=null) floorY = framework.GetPlayerPosition().y;
                    }
                    if (!Framework.Core.CommonUtility.RayInsectionFloor(out lookatPos, frame.position, dir, floorY))
                    {
                        lookatPos = frame.position + dir * 20;
                        bRayHit = false;
                    }
                }

                Handles.color = Color.cyan;
                Handles.DrawLine(frame.position, lookatPos);
                Handles.ArrowHandleCap(0, lookatPos, frame.rotation, 0.1f, EventType.Repaint);
                Handles.color = color;

                if (currentEvt != null && currentEvt.control)
                {
                    //HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                    if (bRayHit)
                    {
                        lookatPos = Handles.DoPositionHandle(lookatPos, Quaternion.identity);
                        Handles.Label(lookatPos, "视点[" + i.ToString() + "]");
                        frame.rotation = Quaternion.LookRotation((lookatPos - frame.position).normalized);

                        if (frame.bSyncToCamera && m_pCamera != null)
                        {
                            if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.Position) || m_pSpline.IsEnable(CameraSpline.EUsedFlag.LookAt))
                                m_pCamera.transform.position = frame.position;
                            if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.Rotation) || m_pSpline.IsEnable(CameraSpline.EUsedFlag.LookAt))
                                m_pCamera.transform.rotation = frame.rotation;
                        }
                    }
                }
                else if (currentEvt != null && currentEvt.shift)
                {
                    //  HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                    frame.inTan = Handles.DoPositionHandle(frame.position + frame.inTan, Quaternion.identity) - frame.position;
                    frame.outTan = Handles.DoPositionHandle(frame.position + frame.outTan, Quaternion.identity) - frame.position;

                    Handles.color = Color.red;
                    Handles.SphereHandleCap(0, frame.position + frame.inTan, Quaternion.identity, 0.1f, EventType.Repaint);
                    Handles.SphereHandleCap(0, frame.position + frame.outTan, Quaternion.identity, 0.1f, EventType.Repaint);
                    Handles.color = color;

                    Handles.SphereHandleCap(0, frame.position, Quaternion.identity, 0.2f, EventType.Repaint);
                }
                else
                {
                    frame.position = Handles.DoPositionHandle(frame.position, Quaternion.identity);
                }


                Handles.Label(frame.position, i.ToString() + "[" + frame.time + "]");
                m_pSpline.Frames[i] = frame;
            }


            for (int i = 0; i < m_pSpline.Frames.Count; ++i)
            {
                CurveData frame = m_pSpline.Frames[i];
                Vector3 prevPos = frame.position;
                Vector3 nextPos = frame.position;
                Vector3 retPos = Vector3.zero;
                Quaternion retEuler = Quaternion.identity;
                float fov = 0;
                if (i - 1 >= 0 && m_pSpline.Evaluate((frame.time + m_pSpline.Frames[i - 1].time) / 2, ref retPos, ref retEuler, ref fov))
                {
                    prevPos = retPos;
                }
                if (i + 1 < m_pSpline.Frames.Count && m_pSpline.Evaluate((frame.time + m_pSpline.Frames[i + 1].time) / 2, ref retPos, ref retEuler, ref fov))
                {
                    nextPos = retPos;
                }

                Vector2 curuiPos = HandleUtility.WorldToGUIPoint(frame.position);
                GUILayout.BeginArea(new Rect(curuiPos.x, curuiPos.y, 40, 25));
                if (GUILayout.Button("删除"))
                {
                    if (EditorUtility.DisplayDialog("提示", "删除这一帧", "删除", "取消"))
                    {
                        m_pSpline.Frames.RemoveAt(i);
                        break;
                    }
                }
                GUILayout.EndArea();

                if (i - 1 >= 0)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(prevPos);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y, 40, 25));
                    if (GUILayout.Button("插入"))
                    {
                        float insert = (frame.time + m_pSpline.Frames[i - 1].time) / 2;
                        if (m_pSpline.Evaluate(insert, ref retPos, ref retEuler, ref fov))
                        {
                            CurveData newframe = new CurveData();
                            newframe.time = insert;
                            newframe.position = retPos;
                            newframe.rotation = retEuler;
                            newframe.fov = fov;
                            if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.LookAt))
                            {
                                newframe.lookat = frame.lookat;
                                newframe.lookatTrans = frame.lookatTrans;
                            }
                            m_pSpline.Frames.Insert(i, newframe);

                            m_fDurationTime = insert;
                            m_fTestTime = insert;
                            break;
                        }
                    }
                    GUILayout.EndArea();
                }
                if (i + 1 < m_pSpline.Frames.Count)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(nextPos);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y, 40, 25));
                    if (GUILayout.Button("插入"))
                    {
                        float insert = (frame.time + m_pSpline.Frames[i + 1].time) / 2;
                        if (m_pSpline.Evaluate(insert, ref retPos, ref retEuler, ref fov))
                        {
                            CurveData newframe = new CurveData();
                            newframe.time = insert;
                            newframe.position = retPos;
                            newframe.rotation = retEuler;
                            newframe.fov = fov;
                            if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.LookAt))
                            {
                                newframe.lookat = frame.lookat;
                                newframe.lookatTrans = frame.lookatTrans;
                            }
                            m_pSpline.Frames.Insert(i + 1, newframe);

                            m_fDurationTime = insert;
                            m_fTestTime = insert;
                            break;
                        }
                    }
                    GUILayout.EndArea();
                }

                if (currentEvt != null && currentEvt.control)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(frame.position + Vector3.up * 1f);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y, 120, 25));
                    if (GUILayout.Button("相机参数覆盖到帧"))
                    {
                        frame.position = m_pCamera.transform.position;
                        frame.rotation = m_pCamera.transform.rotation;
                        m_pSpline.Frames[i] = frame;
                    }
                    GUILayout.EndArea();

                    float floorY = 0;
                    if (Framework.Module.ModuleManager.mainModule != null)
                    {
                        AFrameworkModule framework = Framework.Module.ModuleManager.GetMainFramework<AFrameworkModule>();
                        if(framework!=null) floorY = framework.GetPlayerPosition().y;
                    }
                    Vector3 dir = Framework.Core.CommonUtility.EulersAngleToDirection(frame.rotation.eulerAngles);
                    Vector3 lookatPos = Vector3.zero;
                    if (Framework.Core.CommonUtility.RayInsectionFloor(out lookatPos, frame.position, dir, floorY))
                    {
                        position2 = HandleUtility.WorldToGUIPoint(lookatPos + Vector3.up * 1f);
                        GUILayout.BeginArea(new Rect(position2.x, position2.y, 120, 25));
                        if (GUILayout.Button(frame.bSyncToCamera ? "取消相机同步" : "同步相机"))
                        {
                            frame.bSyncToCamera = !frame.bSyncToCamera;
                            m_pSpline.Frames[i] = frame;
                        }
                        GUILayout.EndArea();
                    }
                }
            }
        }
        //------------------------------------------------------
        private void Update()
        {

        }
        //------------------------------------------------------
        void UpdateCamera(float fTime)
        {
            CameraController ctl = CameraController.getInstance() as CameraController;
            if (ctl != null)
            {
                Vector3 retPos = Vector3.zero;
                Quaternion retEuler = Quaternion.identity;
                float fov = 0;
                if (m_pSpline.Evaluate(fTime, ref retPos, ref retEuler, ref fov))
                {
                    if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.Rotation) || m_pSpline.IsEnable(CameraSpline.EUsedFlag.LookAt))
                        ctl.SyncEditEuler(retEuler.eulerAngles);
                    if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.Position) || m_pSpline.IsEnable(CameraSpline.EUsedFlag.LookAt))
                        ctl.SyncEditPos(retPos);
                    if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.Fov))
                        ctl.UpdateFov(fov);
                }
            }
            if (m_pCamera)
            {
                Vector3 retPos = Vector3.zero;
                Quaternion retEuler = Quaternion.identity;
                float fov = 0;
                if (m_pSpline.Evaluate(fTime, ref retPos, ref retEuler, ref fov))
                {
                    if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.Rotation) || m_pSpline.IsEnable(CameraSpline.EUsedFlag.LookAt))
                        m_pCamera.transform.position = retPos;
                    if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.Position) || m_pSpline.IsEnable(CameraSpline.EUsedFlag.LookAt))
                        m_pCamera.transform.rotation = retEuler;
                    if (m_pSpline.IsEnable(CameraSpline.EUsedFlag.Fov))
                        m_pCamera.fieldOfView = fov;
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

            if (m_bPlaying && m_pSpline != null)
            {
                m_fDurationTime += deltaTime;
                UpdateCamera(m_fDurationTime);
                if (m_fDurationTime >= m_pSpline.GetMaxTime())
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