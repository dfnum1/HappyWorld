
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;
using UnityEngine.Playables;
using TopGame.Data;
using TopGame.Core;
using TopGame.Base;
using Framework.Module;
using System.IO;
using Framework.Core;
using Framework.ED;
using Framework.Data;

namespace TopGame.ED
{
    public class AnimPathEditorLogic
    {
        private ushort m_nAddID = 0;
        public CsvData_TargetPaths m_pPathAsset = null;
        Vector2 m_layerPanelScrollPos;
        Vector2 m_inspectorPanelScrollPos;
        List<GameObject> m_SceneGameObject = new List<GameObject>();

        IPlayableBase m_PlayingAble = null;
        public bool m_bPlayPath = false;
        private GameObject m_pCameraRoot = null;
        private Camera m_pCamera = null;
        public bool bPreviewGame = true;

        public bool bGameControllMode = false;

        RenderTexture m_pPreviewTex = null;
        RenderTexture m_pPreviewGameTex = null;

        private int m_hotEventKey;
        private float m_tempPreviewPlaybackTime;
        public float m_fMaxKeyTime = 60f;
        private float m_fPlayTime = 0f;
        private Core.SplineCurve.KeyFrame m_CurKey = Core.SplineCurve.KeyFrame.Epsilon;
        private BaseEventParameter m_CurEvent = null;
        TimelinePanel m_pTimePanel = new TimelinePanel();

        List<string> m_vTrackSlots = new List<string>();
        List<string> m_vTrackBinds = new List<string>();

        AnimationClip m_AnimClip = null;
        Vector3 m_AdjustPos = Vector3.zero;
        Vector3 m_AdjustRot = Vector3.zero;

        Vector2 m_EventScroll = Vector2.zero;
        bool m_bExpandEvent = false;
        List<string> m_SettingTargets = new List<string>();

        AnimationClip m_SeleAnimation = null;
        Data.TargetPathData m_SelePathData = null;
        private SplineCurve m_Curve = new SplineCurve();
        public List<Vector3> m_CurvePaths = new List<Vector3>();
        TargetPreview m_Preivew = null;

        bool m_bEditorUpdateView = false;
        bool m_bMirrorTest = false;

        GameObject m_pEmpty = null;
        private Rect m_CurRegion = new Rect(0, 0, 150f, 150f);
        int m_PathSelectIndex = -1;

        GameObject m_SceneRoot = null;
        List<GameObject> m_vChildAnimator = new List<GameObject>();

        float m_fDeltaScale = 1f;

        List<string> m_PathPops = new List<string>();
        List<string> m_ScenePops = new List<string>();

        List<BaseEventParameter> m_vCopyEvents = new List<BaseEventParameter>();

        GameObject m_pTimeline = null;
        PlayableDirector m_DirectorAsset = null;
        PlayableDirector m_DirectorAssetInstance = null;
        List<string> m_TimelineBinds = new List<string>();
        List<Camera> m_TimelineCamera = new List<Camera>();
        Camera m_pTimelineCamea = null;

        AnimPathEditor m_pEditor;
        EEventType m_AddEventType = EEventType.Count;
        //-----------------------------------------------------
        public void OnEnable(AnimPathEditor pEditor, TargetPreview preview)
        {
            m_pEditor = pEditor;
            m_SceneGameObject.Clear();
            // LoadScene("Assets/Scene/Homeland.unity");
            m_Preivew = preview;
            bPreviewGame = false;
            LoadCamera();
            m_pEmpty = GameObject.CreatePrimitive(PrimitiveType.Cube);

            m_Curve.OnTriggerEvent = OnTrigger;
            

            CollectObjects(false);
        }
        //-----------------------------------------------------
        public void OnDisable()
        {
            foreach (var db in m_SceneGameObject)
            {
                Util.DestroyImmediate(db);
            }
            m_SceneGameObject.Clear();
            Clear();
            Util.DestroyImmediate(m_SceneRoot);
            Util.DestroyImmediate(m_pTimeline);
            m_DirectorAsset = null;
            m_DirectorAssetInstance = null;
            m_pTimelineCamea = null;
            m_TimelineBinds.Clear();
            m_TimelineCamera.Clear();
            if (m_Preivew != null) m_Preivew.Destroy();
        }
        public void OnTrigger(BaseEventParameter param, bool bAutoClear)
        {
            if (param == null) return;
            
        }
        //-----------------------------------------------------
        void RefreshPathsPop()
        {
            m_PathPops.Clear();
            if (m_pPathAsset != null)
            {
                foreach (var db in m_pPathAsset.datas)
                {
                    m_PathPops.Add(db.Value.strName + "[" + db.Key + "]");
                }
            }

        }
        //-----------------------------------------------------
        void RefreshScenesPop()
        {
            m_ScenePops.Clear();
        }
        //-----------------------------------------------------
        void LoadCamera()
        {
            if (Application.isPlaying) return;
            if (m_pCamera)
                Util.DestroyImmediate(m_pCamera.gameObject);

            m_pCameraRoot = new GameObject("CameraRoot");
            Util.ResetGameObject(m_pCameraRoot, EResetType.All);
            GameObject camera = new GameObject("Camera");
            Util.ResetGameObject(camera, EResetType.All);
            camera.transform.SetParent(m_pCameraRoot.transform);

            m_pCamera = camera.AddComponent<Camera>();
            m_pCamera.clearFlags = CameraClearFlags.Skybox;
            m_pCamera.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.02f);
            m_pCamera.cullingMask = 1 << TargetPreview.PreviewCullingLayer;
            m_pCamera.useOcclusionCulling = false;
            m_pCamera.allowHDR = false;
            m_pCamera.allowMSAA = false;
            m_pCamera.fieldOfView = 55.0f;
            m_pCamera.nearClipPlane = 0.01f;
            m_pCamera.farClipPlane = 1000.0f;
            m_pCamera.transparencySortMode = TransparencySortMode.Perspective;
        }
        //-----------------------------------------------------
        private Vector3 DoPositionHandle(Vector3 position, Quaternion rotation)
        {
            Color temp = Handles.color;

            Handles.color = Handles.xAxisColor; position = Handles.Slider(position, rotation * Vector3.right);
            Handles.color = Handles.yAxisColor; position = Handles.Slider(position, rotation * Vector3.up);
            Handles.color = Handles.zAxisColor; position = Handles.Slider(position, rotation * Vector3.forward);

            Handles.color = temp;

            return position;
        }
        //-----------------------------------------------------
        public void Clear()
        {
            if (m_pCameraRoot)
                Util.DestroyImmediate(m_pCameraRoot);
            if (m_pPreviewTex)
                Util.DestroyImmediate(m_pPreviewTex);
            if (m_pPreviewGameTex)
                Util.DestroyImmediate(m_pPreviewGameTex);
            m_pPreviewTex = null;
            m_pPreviewGameTex = null;

            m_PathSelectIndex = -1;
            Util.DestroyImmediate(m_pEmpty);
            Util.DestroyImmediate(m_pTimeline);
            m_DirectorAsset = null;
            m_DirectorAssetInstance = null;
            m_vTrackBinds.Clear();
            m_vTrackSlots.Clear();
            m_pTimelineCamea = null;
            m_TimelineBinds.Clear();
            m_TimelineCamera.Clear();
        }
        //-----------------------------------------------------
        private void UpdateFrame()
        {

        }
        //-----------------------------------------------------
        public void Update(float fFrameTime)
        {
            if (ModuleManager.editorModule == null) return;
            fFrameTime = m_fDeltaScale * fFrameTime;
            if (bGameControllMode)
            {
                bPreviewGame = false;

                return;
            }

            return;
            if (m_bPlayPath)
            {
                UpdateCamera(m_fPlayTime);
                m_fPlayTime += fFrameTime;

                if (m_fPlayTime >= m_Curve.GetMaxTime())
                    m_fPlayTime = m_Curve.GetMaxTime();
            }
            else
            {
                m_fDeltaScale = 1f;
                float time = m_fPlayTime;
                UpdateCamera(time);
                AFrameworkModule framework = ModuleManager.editorModule as AFrameworkModule;
                ICameraController cameraCtl = (ICameraController)(framework!=null? framework.cameraController:null);
                if (cameraCtl != null && cameraCtl.GetTransform())
                {
                    Vector3 pos = cameraCtl.GetPosition();
                    Vector3 euler = cameraCtl.GetTransform().eulerAngles;
                    Vector3 LookAtPos = Vector3.zero;
                    float fov = cameraCtl.GetCamera().fieldOfView;
                    bool bLookAt = false;
                    m_Curve.Evaluate(time, ref pos, ref euler, ref fov, ref bLookAt, ref LookAtPos, m_bPlayPath, true);
                    if (m_SelePathData != null && bLookAt)
                    {
                        cameraCtl.GetTransform().transform.LookAt(LookAtPos);
                    }
                    else
                    {
                        cameraCtl.GetTransform().transform.eulerAngles = euler;
                    }
                    cameraCtl.GetTransform().transform.position = pos;
                    cameraCtl.UpdateFov(fov);
                    for (int i = 0; i < m_Curve.events.Count; ++i)
                    {
                        if (Mathf.Abs(time - m_Curve.events[i].triggetTime) <= 0.1f)
                        {
                            if (!m_Curve.events[i].stopWithAction)
                            {
                                m_Curve.events[i].stopWithAction = true;
                                OnTrigger(m_Curve.events[i], false);
                            }
                        }
                    }
                }
            }
        }
        //-----------------------------------------------------
        void UpdateCamera(float time)
        {
            if (m_pCameraRoot)
            {
                Vector3 pos = m_pCameraRoot.transform.position;
                Vector3 euler = m_pCameraRoot.transform.eulerAngles;
                Vector3 LookAtPos = Vector3.zero;
                float fov = m_pCamera.fieldOfView;
                bool bLookAt = false;
                if (m_DirectorAssetInstance && m_TimelineCamera.Count > 0)
                {
                    m_pTimelineCamea = null;
                    for (int i = 0; i < m_TimelineCamera.Count; ++i)
                    {
                        if (m_TimelineCamera[i].gameObject.activeSelf)
                        {
                            m_pTimelineCamea = m_TimelineCamera[i];
                            break;
                        }
                    }

                    if (m_pTimelineCamea != null)
                    {
                        m_DirectorAssetInstance.time = time;
                        m_DirectorAssetInstance.Evaluate();
                        pos = m_pTimelineCamea.transform.position;
                        m_pCameraRoot.transform.position = m_pTimelineCamea.transform.position;
                        m_pCameraRoot.transform.eulerAngles = m_pTimelineCamea.transform.eulerAngles;
                        fov = m_pTimelineCamea.fieldOfView;
                    }
                }
                else
                {
                    m_Curve.Evaluate(time, ref pos, ref euler, ref fov, ref bLookAt, ref LookAtPos, m_bPlayPath, true);
                    if (m_SelePathData != null && bLookAt)
                    {
                        m_pCameraRoot.transform.LookAt(LookAtPos);
                        if (m_bMirrorTest)
                        {
                            m_pCameraRoot.transform.eulerAngles -= Vector3.up * 180f;
                        }
                    }
                    else
                    {
                        m_pCameraRoot.transform.eulerAngles = euler;
                        if (m_bMirrorTest) m_pCameraRoot.transform.eulerAngles -= Vector3.up * 180f;
                    }
                    if (m_bMirrorTest && m_SelePathData != null)
                    {
                        float posY = pos.y;
                        pos = m_SelePathData.MirrorReference - pos + m_SelePathData.MirrorReference;
                        pos.y = posY;
                    }
                }

                m_pCameraRoot.transform.position = pos;
                m_pCamera.fieldOfView = fov;

                if (m_bPlayPath || m_bEditorUpdateView)
                {
                    m_Preivew.SetCameraPositionAndEulerAngle(m_pCameraRoot.transform.position, m_pCameraRoot.transform.eulerAngles);
                    m_Preivew.SetCameraFov(fov);

                    // if(SceneView.lastActiveSceneView!=null)
                    // {
                    //     SceneView.lastActiveSceneView.camera.transform.position = m_pCameraRoot.transform.position;
                    //     SceneView.lastActiveSceneView.camera.transform.eulerAngles = m_pCameraRoot.transform.eulerAngles;
                    //     SceneView.lastActiveSceneView.camera.fieldOfView = fov;
                    //     SceneView.lastActiveSceneView.Repaint();
                    // }
                }
            }
        }
        //-----------------------------------------------------
        public void OnEvent(Event evt)
        {
            if (evt.type == EventType.KeyDown)
            {
                if (evt.keyCode == KeyCode.F1)
                {
                    bPreviewGame = !bPreviewGame;
                }
                else if (evt.keyCode == KeyCode.F2)
                {
                    m_fPlayTime = 0;
                    Play();
                }
                else if (evt.keyCode == KeyCode.K)
                {
                    AddKey(m_fPlayTime);
                }
                else if (evt.keyCode == KeyCode.Delete)
                {
                    if (m_CurKey.isValid)
                        RemoveKey(m_CurKey);
                    m_CurKey.time = -1;
                }
            }
        }
        //-----------------------------------------------------
        void ChangeGameControllerMode()
        {
            if (!bGameControllMode) return;

            SplineCurve.KeyFrame frame = m_Curve.GetKeyByIndex(m_Curve.keys.Count - 1);
            if (m_Preivew != null)
            {
                if (frame.isValid)
                {
                    m_Preivew.SetCameraFov(frame.fov);
                    m_Preivew.SetCameraPositionAndEulerAngle(frame.position, frame.eulerAngle);
                }
            }
        }
        //-----------------------------------------------------
        private void AddKey(float time)
        {
            time *= m_fMaxKeyTime;
            bool bHas = false;

            SplineCurve.KeyFrame frame = m_Curve.GetKey(time);
            if (frame.isValid)
            {
                frame.position = m_Preivew.GetCamera().transform.position;
                frame.eulerAngle = m_Preivew.GetCamera().transform.eulerAngles;
                frame.fov = m_Preivew.GetCameraFov();
                return;
            }

            frame = new SplineCurve.KeyFrame();
            frame.time = time;
            frame.fov = m_Preivew.GetCameraFov();
            frame.position = m_Preivew.GetCamera().transform.position;
            frame.eulerAngle = m_Preivew.GetCamera().transform.eulerAngles;
            m_Curve.AddKey(frame);
        }
        //-----------------------------------------------------
        public void RemoveKey(SplineCurve.KeyFrame key)
        {
            m_Curve.RemoveKey(key);
        }
        //-----------------------------------------------------
        private void SelectKey(SplineCurve.KeyFrame keyTime, bool bSetCamera = true)
        {
            m_CurKey = keyTime;
            m_fPlayTime = keyTime.time;

            if (!bSetCamera) return;
            if (!m_bPlayPath)
            {
                if (m_pCameraRoot != null)
                {
                    m_pCameraRoot.transform.position = m_CurKey.position;
                    m_pCameraRoot.transform.eulerAngles = m_CurKey.eulerAngle;
                    m_pCamera.fieldOfView = m_CurKey.fov;
                }

                if (m_Preivew != null)
                {
                    m_Preivew.SetCameraFov(m_CurKey.fov);
                    m_Preivew.SetCameraPositionAndEulerAngle(m_CurKey.position, m_CurKey.eulerAngle);

                }
            }
        }
        //-----------------------------------------------------
        private void SelectEvent(BaseEventParameter keyTime)
        {
            m_CurEvent = keyTime;
            if (m_CurEvent == null) return;
            m_fPlayTime = m_CurEvent.triggetTime;
        }
        //-----------------------------------------------------
        public void SaveData()
        {
            foreach (var db in m_pPathAsset.datas)
            {
                db.Value.Save();
            }
            m_pPathAsset.Save();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
        //-----------------------------------------------------
        public void LoadScene()
        {
            if (Application.isPlaying) return;
            string path = Application.dataPath;
            path = EditorUtility.OpenFilePanel("选择场景", path, "unity");
            if (Path.GetExtension(path) != "unity")
            {
                EditorUtility.DisplayDialog("error", "check path error!", "ok");
                return;
            }
            path = "Assets/" + path.Substring(Application.dataPath.Length, path.Length - Application.dataPath.Length);
            LoadScene(path);
        }
        //-----------------------------------------------------
        private void LoadScene(string path)
        {
            if (Application.isPlaying) return;

            bPreviewGame = false;
            foreach (var db in m_SceneGameObject)
            {
                Util.DestroyImmediate(db);
            }
            m_SceneGameObject.Clear();
            Clear();
            //EditorApplication.OpenScene(path);
            UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path);

            CollectObjects(true);
            LoadCamera();
        }
        //-----------------------------------------------------
        void CollectObjects(bool bAddSceneList)
        {
            m_SceneGameObject.Clear();
            Util.DestroyImmediate(m_SceneRoot);
            foreach (GameObject rootObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                HideFlags pre = rootObj.hideFlags;
                if(!Application.isPlaying) m_Preivew.AddPreview(rootObj, HideFlags.None);
                rootObj.hideFlags = pre;
                m_SceneGameObject.Add(rootObj);
            }

            m_vChildAnimator.Clear();
            for (int i = 0; i < m_SceneGameObject.Count; ++i)
            {
                Transform tm = m_SceneGameObject[i].transform;
                Animator temp = tm.GetComponent<Animator>();
                if (temp != null && !m_vChildAnimator.Contains(tm.gameObject))
                    m_vChildAnimator.Add(tm.gameObject);
                for (int j = 0; j < tm.childCount; ++i)
                {
                    tm = tm.GetChild(j);
                    temp = tm.GetComponent<Animator>();
                    if (temp != null && !m_vChildAnimator.Contains(tm.gameObject))
                        m_vChildAnimator.Add(tm.gameObject);
                }
            }

            m_SettingTargets.Clear();

            if (!bAddSceneList)
                m_SceneGameObject.Clear();


        }
        //-----------------------------------------------------
        public void OnGUI()
        {
            if (m_pEditor == null) return;
            m_fPlayTime = m_pTimePanel.Draw(new Rect(10, m_pEditor.position.height - m_pEditor.GapBottom, m_pEditor.position.width - 20f, m_pEditor.GapBottom), m_fPlayTime, m_fMaxKeyTime, m_fMaxKeyTime * 0.01f);
            EditorGUI.DropShadowLabel(new Rect(Screen.width - 200, 20, 200f, 20f), "当前时间: " + m_fPlayTime.ToString());
        }
        //-----------------------------------------------------
        public void OnDraw(int controllerId, Camera camera, int cullLayer)
        {
            if (m_pCamera != null)
            {
                Handles.CubeHandleCap(controllerId, m_pCamera.transform.position, Quaternion.identity, 1, EventType.Repaint);

                float width = m_pEditor.position.width - m_pEditor.LeftWidth - m_pEditor.RightWidth;
                float height = m_pEditor.position.height - m_pEditor.GapTop - m_pEditor.GapBottom;
                EditorUtil.DrawDebugCamera(m_pCamera, (int)width, (int)height, new Color(233f / 255f, 211f / 255f, 233f / 255f, 128f / 255f));
            }
            Color bak = Handles.color;
            
            Handles.color = bak;
            m_Curve.DrawDebug(controllerId, 100, true, m_SeleAnimation ? 3 : 6, m_SeleAnimation == null);

            if (m_SelePathData != null)
            {
                float size = Mathf.Min(10, HandleUtility.GetHandleSize(m_SelePathData.MirrorReference));
                Handles.color = Color.yellow;
                Handles.CubeHandleCap(controllerId, m_SelePathData.MirrorReference, Quaternion.identity, size, EventType.Repaint);
            }
            Handles.color = bak;
        }
        //-----------------------------------------------------
        void LoadPath(Data.TargetPathData data)
        {
            if (data == null) return;
            bPreviewGame = false;
            m_SelePathData = data;
            m_Curve.Clear();
            m_DirectorAsset = null;
            m_AnimClip = null;
            m_vTrackSlots.Clear();
            m_vTrackBinds.Clear();

            Util.DestroyImmediate(m_pTimeline);
            m_DirectorAssetInstance = null;
            m_pTimelineCamea = null;
            m_TimelineBinds.Clear();
            m_TimelineCamera.Clear();

            if (m_SelePathData.animClip.Length > 0)
            {
                m_AnimClip = AssetDatabase.LoadAssetAtPath<UnityEngine.AnimationClip>(m_SelePathData.animClip);
                m_Curve.SetClip(m_pCameraRoot, m_AnimClip, m_SelePathData.keys, m_SelePathData.events);
            }
            else if (m_SelePathData.timeline.Length > 0)
            {
                GameObject pAsset = AssetDatabase.LoadAssetAtPath<GameObject>(m_SelePathData.timeline);
                if (pAsset)
                {
                    m_DirectorAsset = pAsset.GetComponent<PlayableDirector>();
                    m_pTimeline = GameObject.Instantiate(pAsset);
                    m_Preivew.AddPreview(m_pTimeline);
                    m_DirectorAssetInstance = m_pTimeline.GetComponent<PlayableDirector>();
                    if (m_DirectorAssetInstance)
                    {
                        for (int i = 0; i < m_DirectorAssetInstance.transform.childCount; ++i)
                        {
                            if (!m_TimelineBinds.Contains(m_DirectorAssetInstance.transform.GetChild(i).name))
                                m_TimelineBinds.Add(m_DirectorAssetInstance.transform.GetChild(i).name);
                        }

                        m_TimelineCamera = new List<Camera>(m_DirectorAssetInstance.transform.GetComponentsInChildren<Camera>());
                    }
                }

            }
            else
                m_Curve.AddKeys(m_SelePathData.keys, m_SelePathData.events);

        }
        //-----------------------------------------------------
        public void DrawInspectorPanel(Vector2 size)
        {            
            if (m_SelePathData != null)
            {
                GUILayout.Box("事件列表", new GUILayoutOption[] { GUILayout.MinWidth(size.x - 20) });
                GUILayout.BeginHorizontal();
                if (m_SelePathData.events != null && m_SelePathData.events.Length > 0 && GUILayout.Button("复制"))
                {
                    m_vCopyEvents = new List<BaseEventParameter>(m_SelePathData.events);
                }
                if (m_vCopyEvents.Count > 0 && GUILayout.Button("粘贴"))
                {
                    m_SelePathData.events = m_vCopyEvents.ToArray();
                    m_vCopyEvents.Clear();
                }
                GUILayout.EndHorizontal();
                if (Framework.Module.ModuleManager.editorModule != null && m_SelePathData.events != null && m_SelePathData.events.Length > 0)
                {
                    AFrameworkModule framework = ModuleManager.editorModule as AFrameworkModule;
                    if (framework!=null && framework.cameraController != null && GUILayout.Button("清除已触发事件"))
                    {
                        for (int i = 0; i < m_Curve.events.Count; ++i)
                        {
                            if (m_Curve.events[i].stopWithAction)
                                m_Curve.events[i].stopWithAction = false;
                        }
                    }
                }

                List<BaseEventParameter> events = null;
                if (m_SelePathData.events != null)
                {
                    events = new List<BaseEventParameter>(m_SelePathData.events);
                }
                else
                    events = new List<BaseEventParameter>();

                m_EventScroll = EditorGUILayout.BeginScrollView(m_EventScroll);

                for (int i = 0; i < events.Count; ++i)
                {
                    if (events[i] != null)
                    {
                        GUILayout.BeginHorizontal();
                        string tag = string.Format("event#[{0}]->{1}", events[i].GetEventType(), events[i].triggetTime.ToString());
                        m_bExpandEvent = EditorGUILayout.Foldout(m_CurEvent == events[i], tag);
                        if (GUILayout.Button("Del", new GUILayoutOption[] { GUILayout.Width(40) }))
                        {
                            events.RemoveAt(i);
                            m_SelePathData.events = events.ToArray();
                            m_Curve.events = new List<BaseEventParameter>(events.ToArray());
                            return;
                        }
                        if (GUILayout.Button("复制", new GUILayoutOption[] { GUILayout.Width(40) }))
                        {
                            DrawEventCore.AddCopyEvent(events[i]);
                        }
                        if (DrawEventCore.CanCopyEvent(events[i]) &&
                            GUILayout.Button("黏贴", new GUILayoutOption[] { GUILayout.Width(40) }))
                        {
                            DrawEventCore.CopyEvent(events[i]);
                        }
                        GUILayout.EndHorizontal();
                        if (m_bExpandEvent)
                        {
                            m_CurEvent = events[i];
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(15);
                            GUILayout.BeginVertical();
                            float preTime = events[i].triggetTime;
                            events[i].triggetTime = EditorGUILayout.FloatField("Time", events[i].triggetTime);
                            //    HandleUtilityWrapper.DrawProperty(events[i], null);
                            List<string> vSlots = m_vTrackSlots;
                            if(events[i].GetEventType() == EEventType.TrackBind && (events[i] as TrackBindEventParameter).bGenericBinding)
                            {
                                vSlots = m_vTrackBinds;
                            }
                            events[i] =DrawEventCore.DrawUnAction(events[i], vSlots);
                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            if (preTime != events[i].triggetTime)
                            {
                                Framework.Plugin.SortUtility.QuickSortUp<BaseEventParameter>(ref events);
                            }
                        }
                        else
                        {
                            if (m_CurEvent == events[i]) m_CurEvent = null;
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
                GUILayout.BeginHorizontal();
                m_AddEventType = EventPopDatas.DrawEventPop(m_AddEventType, "");
                EditorGUI.BeginDisabledGroup(m_AddEventType == EEventType.Base || m_AddEventType == EEventType.Count);
                if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    BaseEventParameter parame = BuildEventUtl.BuildEventByType(null, m_AddEventType);
                    if(parame!=null)
                    {
                        parame.triggetTime = m_fPlayTime;
                        List<BaseEventParameter> vEvent = m_SelePathData.events != null ? new List<BaseEventParameter>(m_SelePathData.events) : new List<BaseEventParameter>();
                        vEvent.Add(parame);
                        m_SelePathData.events = vEvent.ToArray();
                        m_AddEventType = EEventType.Count;
                    }


                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }

        }
        //-----------------------------------------------------
        public void OnDrawLayerPanel(Rect rc)
        {
            GUILayoutOption[] op = new GUILayoutOption[] { GUILayout.Width(rc.width-10) };
            m_fMaxKeyTime = EditorGUILayout.FloatField("最大时间(s)", m_fMaxKeyTime, op);
            if (m_Preivew != null)
                m_Preivew.SetCameraFov(EditorGUILayout.Slider("FOV", m_Preivew.GetCameraFov(), 0, 180));

            if (!m_bPlayPath)
                m_bEditorUpdateView = EditorGUILayout.Toggle("是否同时呈现镜头画面", m_bEditorUpdateView, op);
            m_bMirrorTest = EditorGUILayout.Toggle("是否镜像测试", m_bMirrorTest, op);

            if (m_SelePathData != null)
            {
                if (GUILayout.Button("ApplyFov", new GUILayoutOption[] { GUILayout.Width(m_pEditor.LeftWidth) }))
                {
                    List<SplineCurve.KeyFrame> Keys = m_Curve.keys;
                    for (int i = 0; i < Keys.Count; ++i)
                    {
                        SplineCurve.KeyFrame keyFrame = Keys[i];
                        keyFrame.fov = m_Preivew.GetCameraFov();
                        Keys[i] = keyFrame;
                    }
                    m_SelePathData.keys = Keys.ToArray();

                    return;
                }
            }

            int prePth = m_PathSelectIndex;
            m_PathSelectIndex = EditorGUILayout.Popup("路径动画", m_PathSelectIndex, m_PathPops.ToArray(), op);
            if (prePth != m_PathSelectIndex)
            {
                if (m_PathSelectIndex >= 0 && m_PathSelectIndex < m_pPathAsset.datas.Count)
                {
                    Data.TargetPathData data = m_pPathAsset.datas.ElementAt(m_PathSelectIndex).Value;
                    LoadPath(data);
                }
            }

            if (m_SelePathData != null)
            {
                m_SelePathData.strName = EditorGUILayout.TextField("名称描述", m_SelePathData.strName, op);
                m_SelePathData.isUIPath = EditorGUILayout.Toggle("UI 路径动画", m_SelePathData.isUIPath, op);
                HandleUtilityWrapper.DrawPropertyByFieldName(m_SelePathData, "useEndType");
                if (m_SelePathData.timeline.Length <= 0)
                {
                    m_SelePathData.MirrorReference = EditorGUILayout.Vector3Field("镜像参考点", m_SelePathData.MirrorReference, op);
                    m_SelePathData.OffsetPosition = EditorGUILayout.Vector3Field("镜头偏移", m_SelePathData.OffsetPosition, op);
                    m_SelePathData.OffsetRotate = EditorGUILayout.Vector3Field("镜头角度偏移", m_SelePathData.OffsetRotate, op);
                }

                m_AnimClip = EditorGUILayout.ObjectField("动画剪辑", m_AnimClip, typeof(AnimationClip), op) as AnimationClip;
                if (m_Curve.clip != m_AnimClip)
                {
                    m_Curve.SetClip(m_pCameraRoot, m_AnimClip, m_SelePathData.keys, m_SelePathData.events);
                    if (m_AnimClip != null)
                    {
                        m_SelePathData.animClip = AssetDatabase.GetAssetPath(m_AnimClip);
                    }
                    else
                    {
                        m_SelePathData.animClip = "";
                    }
                    m_DirectorAsset = null;
                    m_SelePathData.timeline = "";
                }
                if (m_Curve.clip != null)
                    m_fMaxKeyTime = m_Curve.GetMaxTime();

                m_DirectorAsset = EditorGUILayout.ObjectField("Timeline", m_DirectorAsset, typeof(PlayableDirector), op) as PlayableDirector;
                if (m_DirectorAsset != null)
                {
                    m_SelePathData.timeline = AssetDatabase.GetAssetPath(m_DirectorAsset);
                    m_SelePathData.animClip = "";
                    m_AnimClip = null;
                    m_fMaxKeyTime = (float)m_DirectorAsset.duration;

                    m_TimelineBinds.Clear();
                    for (int i = 0; i < m_DirectorAsset.transform.childCount; ++i)
                    {
                        if (!m_TimelineBinds.Contains(m_DirectorAsset.transform.GetChild(i).name))
                            m_TimelineBinds.Add(m_DirectorAsset.transform.GetChild(i).name);
                    }

                    m_vTrackBinds.Clear();
                    m_vTrackSlots.Clear();
                    if(m_DirectorAsset.playableAsset!=null && m_DirectorAsset.playableAsset.outputs!=null)
                    {
                        foreach (var bind in m_DirectorAsset.playableAsset.outputs)
                        {
                            m_vTrackBinds.Add(bind.streamName);
                        }
                    }

                    TimelineController binderSlots = m_DirectorAsset.gameObject.GetComponent<TimelineController>();
                    if(binderSlots != null && binderSlots.slots != null)
                    {
                        for(int i = 0; i < binderSlots.slots.Length; ++i)
                        {
                            if (binderSlots.slots[i] == null) continue;
                            m_vTrackSlots.Add(binderSlots.slots[i].name);
                        }
                    }
                }

                GUILayout.Box("动画帧", new GUILayoutOption[] { GUILayout.MinWidth(m_pEditor.LeftWidth - 20) });
                this.m_layerPanelScrollPos = GUILayout.BeginScrollView(this.m_layerPanelScrollPos, op);
                GUILayout.BeginVertical();

                if (m_AnimClip != null)
                {
                    if (m_Curve.FovCurve == null) m_Curve.FovCurve = new AnimationCurve();
                    if (m_Curve.FovCurve != null)
                    {
                        GUILayout.BeginHorizontal();
                        m_Curve.FovCurve = EditorGUILayout.CurveField("Fov插曲线", m_Curve.FovCurve);
                        if (m_Curve.FovCurve.length > 0)
                        {
                            m_SelePathData.keys = new SplineCurve.KeyFrame[m_Curve.FovCurve.length];
                            for (int i = 0; i < m_Curve.FovCurve.length; ++i)
                            {
                                SplineCurve.KeyFrame frame = new SplineCurve.KeyFrame();
                                frame.time = m_Curve.FovCurve.keys[i].time;
                                frame.inTan.x = m_Curve.FovCurve.keys[i].inTangent;
                                frame.outTan.x = m_Curve.FovCurve.keys[i].outTangent;
                                frame.inTan.y = m_Curve.FovCurve.keys[i].tangentMode;
                                frame.fov = m_Curve.FovCurve.keys[i].value;
                                m_SelePathData.keys[i] = frame;
                            }
                        }
                        else
                        {
                            m_SelePathData.keys = null;
                        }
                        if (GUILayout.Button("烘焙", new GUILayoutOption[] { GUILayout.Width(50) }))
                        {
                            m_Curve.FovCurve = new AnimationCurve();
                            m_Curve.FovCurve.AddKey(new Keyframe(0, m_Preivew.GetCameraFov()));
                            m_Curve.FovCurve.AddKey(new Keyframe(m_AnimClip.length, m_Preivew.GetCameraFov()));
                        }
                        if (GUILayout.Button("应用", new GUILayoutOption[] { GUILayout.Width(50) }))
                        {
                            m_Curve.SetClip(m_pCameraRoot, m_AnimClip, m_SelePathData.keys, m_SelePathData.events);
                        }
                        GUILayout.EndHorizontal();


                    }
                }
                else if (m_DirectorAsset != null)
                {
                    m_SelePathData.bCloseGameCamera = EditorGUILayout.Toggle("是否关闭游戏镜头", m_SelePathData.bCloseGameCamera);
                }

                if (m_AnimClip == null)
                {
                    Color bak_col = GUI.color;
                    List<SplineCurve.KeyFrame> Keys = m_Curve.keys;
                    for (int i = 0; i < Keys.Count; ++i)
                    {
                        SplineCurve.KeyFrame keyFrame = Keys[i];
                        if (i == Keys.Count - 1)
                        {
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("[" + i.ToString() + "]Key@" + Keys[i].time.ToString());
                            GUILayout.EndHorizontal();
                        }
                        else
                            EditorGUILayout.LabelField("[" + i.ToString() + "]Key@" + Keys[i].time.ToString());
                        GUI.color = bak_col;
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        GUILayout.BeginVertical();
                        if (i == 0)
                        {
                            keyFrame.outTan = EditorGUILayout.Vector3Field("OutTan", Keys[i].outTan);
                        }
                        else if (i == Keys.Count - 1)
                        {
                            keyFrame.inTan = EditorGUILayout.Vector3Field("InTan", Keys[i].inTan);
                        }
                        else
                        {
                            keyFrame.inTan = EditorGUILayout.Vector3Field("InTan", Keys[i].inTan);
                            keyFrame.outTan = EditorGUILayout.Vector3Field("OutTan", Keys[i].outTan);
                        }

                        keyFrame.position = EditorGUILayout.Vector3Field("Pos", Keys[i].position);
                        keyFrame.eulerAngle = EditorGUILayout.Vector3Field("EulerAngle", Keys[i].eulerAngle);
                        keyFrame.fov = EditorGUILayout.FloatField("Fov", Keys[i].fov);


                        keyFrame.bLookAt = EditorGUILayout.Toggle("bLookat", Keys[i].bLookAt);
                        if (Keys[i].bLookAt)
                            keyFrame.lookat = EditorGUILayout.Vector3Field("LookAtPos", Keys[i].lookat);

                        keyFrame.bSet = EditorGUILayout.Toggle("bSet", Keys[i].bSet);
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        Keys[i] = keyFrame;
                    }

                    m_SelePathData.keys = Keys.ToArray();
                }

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            }

            GUILayout.BeginHorizontal();
            m_nAddID = (ushort)EditorGUILayout.IntField(m_nAddID);
            if (GUILayout.Button("NewPath"))
            {
                ushort id = m_nAddID;
                if(id == 0)
                {
                    foreach (var db in m_pPathAsset.datas)
                    {
                        id = (ushort)Mathf.Max(db.Key, id);
                    }
                    id++;
                }
                else
                {
                    if(m_pPathAsset.datas.ContainsKey(m_nAddID))
                    {
                        EditorUtility.DisplayDialog("提示", "该ID 已存在!!!", "好的");
                        return;
                    }
                }
  
                Data.TargetPathData ne = new Data.TargetPathData();
                ne.nID = id;
                m_pPathAsset.datas.Add(ne.nID, ne);

                m_nAddID = 0;
                RefreshPathsPop();

                m_SelePathData = ne;
                LoadPath(ne);
            }
            GUILayout.EndHorizontal();
            if (m_SelePathData != null)
            {
                if (GUILayout.Button("RemovePath"))
                {
                    m_pPathAsset.datas.Remove(m_SelePathData.nID);
                    m_SelePathData = null;
                    RefreshPathsPop();
                }
            }
        }
        //-----------------------------------------------------
        public void OnDrawPreivew(Rect rc)
        {
#if (UNITY_5_6 || UNITY_5_1)
        if (m_pCamera != null)
        {
            if(bPreviewGame)
            {
                if (m_pPreviewGameTex == null)
                    m_pPreviewGameTex = new RenderTexture((int)rc.width, (int)rc.height, 24, RenderTextureFormat.ARGB32);
                m_pCamera.targetTexture = m_pPreviewGameTex;
                m_pCamera.Render();
                GUI.DrawTexture(rc, m_pCamera.targetTexture);
            }
            else
            {
                if (m_pPreviewTex == null)
                    m_pPreviewTex = new RenderTexture(300, 200, 24, RenderTextureFormat.ARGB32);
                m_pCamera.targetTexture = m_pPreviewTex;
                m_pCamera.Render();
                Color bak = GUI.color;
                GUI.color = Color.white;
                GUI.Box(new Rect(rc.right - 305, rc.bottom - 205, 300, 200), "");
                GUI.color = bak;
                GUI.DrawTexture(new Rect(rc.right- 300, rc.bottom-200, 300, 200), m_pCamera.targetTexture);
            }
        }
#else
#endif
        }
        //-----------------------------------------------------
        public void OnMouseMove(Vector3 hitPos, Ray ray)
        {
            if (bGameControllMode)
            {
                return;
            }
        }
        //-----------------------------------------------------
        public void OnMouseHit(Vector3 hitPos, ref Ray ray)
        {
            if (Event.current == null)
                return;
            Bounds bound = new Bounds();
            for (int i = 0; i < m_Curve.keys.Count; ++i)
            {
                bound.center = m_Curve.keys[i].position;
                bound.size = Vector3.one * 6;
                if (bound.IntersectRay(ray))
                {
                    SelectKey(m_Curve.keys[i], false);
                    break;
                }

            }
            // m_CurSelect = HandleUtility.PickGameObject(hitPos, true);
        }
        //-----------------------------------------------------
        public void OnMouseUp(int button)
        {
        }
        //-----------------------------------------------------
        public void Play()
        {
            if (m_bPlayPath)
            {
                if (m_pCamera != null)
                {
                    m_pCamera.transform.localPosition = Vector3.zero;
                    m_pCamera.transform.localEulerAngles = Vector3.zero;
                }
                m_bPlayPath = false;
                return;
            }
            
            m_bPlayPath = true;
            m_fPlayTime = 0f;
            if(m_SelePathData != null && CameraController.getInstance()!=null)
            {
                if (m_bPlayPath)
                {
                    IPlayableBase playale = CameraController.getInstance().PlayAniPath(m_SelePathData.nID);
                    if (playale != null)
                        playale.SetOffsetPosition(GameInstance.getInstance().GetPlayerPosition());
                }
                else
                    CameraController.getInstance().StopAllEffect();
            }
            return;
            if (m_SelePathData != null && m_SelePathData.events != null)
            {
                m_Curve.events.Clear();
                for (int i = 0; i < m_SelePathData.events.Length; ++i)
                {
                    m_Curve.AddEvent(m_SelePathData.events[i]);
                }
            }

            if (m_pCamera != null)
            {
                m_pCamera.transform.localPosition = m_SelePathData.OffsetPosition;
                m_pCamera.transform.localEulerAngles = m_SelePathData.OffsetRotate;
            }
            SplineCurve.KeyFrame frame = m_Curve.GetKeyByIndex(0);
            if (frame.isValid)
            {
                if (m_pCameraRoot != null)
                {
                    m_pCameraRoot.transform.position = frame.position;
                    m_pCameraRoot.transform.eulerAngles = frame.eulerAngle ;
                }
                if (m_pCamera) m_pCamera.fieldOfView = frame.fov;
            }
            else
            {
                if (m_Preivew != null)
                {
                    if (m_pCameraRoot)
                    {
                        m_pCameraRoot.transform.position = m_Preivew.GetCamera().transform.position;
                        m_pCameraRoot.transform.eulerAngles = m_Preivew.GetCamera().transform.eulerAngles;
                        m_pCamera.fieldOfView = m_Preivew.GetCameraFov();
                    }

                }
            }
        }
        //-----------------------------------------------------
        public void OnReLoadAssetData()
        {
            InitAsset();
        }
        //-----------------------------------------------------
        public void InitAsset()
        {
            m_pPathAsset = DataEditorUtil.GetTable<CsvData_TargetPaths>(true);
            DataEditorUtil.MappingTable(m_pPathAsset);
            RefreshPathsPop();
            RefreshScenesPop();
        }
    }
}