/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ActionEditor
作    者:	HappLI
描    述:	动作编辑器
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using TopGame.Data;
using Framework.Core;
using System.Reflection;

namespace TopGame.ED
{
    public class ActionEditor : EditorWindowBase
    {
        public static ActionEditor Instance { protected set; get; }

        public RenderTexture m_PreViewRT = null;

        public GUIStyle previewStyle;

        Vector2 m_layerPanelScrollPos = Vector2.zero;
        Vector2 m_inspectorPanelScrollPos = Vector2.zero;
        Vector2 m_framePanelScrollPos = Vector2.zero;

        static float GapTop = 50f;
        static float GapBottom = 5;

        public List<string> m_vBoneList = new List<string>();
        float m_StateFrameCnt = 60;

        private Rect previewRect = new Rect(0, 0, 0, 0);

        Rect m_LayerSize = new Rect();
        Rect m_InspecSize = new Rect();
        Rect m_FrameSize = new Rect();

        public int EditorEventMask = -1;
        public struct EventLayerPop
        {
            public EEventBit eventLayerBit;
            public string strDisplay;
        }
        public List<EventLayerPop> m_vEventLayers = new List<EventLayerPop>();
        ActionEditorLogic m_Logic = new ActionEditorLogic();
        public ActionEditorLogic logic
        {
            get { return m_Logic; }
        }
        byte m_DragSplitGap = 0;

        ActionTimelinePanel m_Timeline = new ActionTimelinePanel();

        //-----------------------------------------------------
        [MenuItem("Tools/角色动作编辑器")]
        private static void StartActionEditor()
        {
            if (Instance == null)
                EditorWindow.GetWindow<ActionEditor>();
            Instance.titleContent = new GUIContent("角色动作编辑器");
        }
        //-----------------------------------------------------
        public void AppEdiorSetup(Camera camera)
        {
            OnReLoad();
        }
        //-----------------------------------------------------
        static public void OnSceneFunc(SceneView sceneView)
        {
            if(Instance) Instance.OnSceneGUI(sceneView);
        }
        //-----------------------------------------------------
        private void OnSceneGUI(SceneView sceneView)
        {
            if (m_Logic != null)
                m_Logic.OnSceneGUI(0, sceneView.camera);
        }
        //-----------------------------------------------------
        public void SetEditorTarget(UnityEngine.Object value, bool bInspectorOPen = true)
        {
            if (value == null) return;
        }
        //-----------------------------------------------------
        private void RefreshChidAnimator(Transform root, bool bClear = true)
        {
            
        }
        //-----------------------------------------------------
        private void BuildObjectClips()
        {
           
        }
        //-----------------------------------------------------
        private static void Init()
        {
            EditorWindow.GetWindow<ActionEditor>();
        }
        //-----------------------------------------------------
        protected override void OnInnerDisable()
        {
            Instance = null;

            m_Logic.OnDisable();

            if (EditorApplication.isPlaying)
                EditorApplication.isPlaying = false;

            SceneView.duringSceneGui -= OnSceneFunc;
        }
        //-----------------------------------------------------
        protected override void OnInnerEnable()
        {
            Instance = this;

            m_vEventLayers.Clear();
            foreach (Enum v in Enum.GetValues(typeof(EEventBit)))
            {
                if (v.ToString().StartsWith("EventLayer"))
                {
                    System.Reflection.FieldInfo fi = typeof(EEventBit).GetField(v.ToString());
                    EventLayerPop evtPop = new EventLayerPop();
                    evtPop.eventLayerBit = (EEventBit)v;
                    evtPop.strDisplay = v.ToString();
                    if (fi != null && fi.IsDefined(typeof(Framework.Plugin.PluginDisplayAttribute)))
                    {
                        evtPop.strDisplay = fi.GetCustomAttribute<Framework.Plugin.PluginDisplayAttribute>().displayName;
                    }
                    m_vEventLayers.Add(evtPop);
                }
            }

            base.minSize = new Vector2(1000f, 320f);
            EditorModule moude = new EditorModule();
            moude.StartUp();
            m_Logic.OnEnable(this, moude);
            EditorKits.NewScene();
            EditorKits.CheckEventCopy();

            SceneView.duringSceneGui += OnSceneFunc;

            m_LayerSize = new Rect(0, GapTop, 320, position.height-GapTop-GapBottom);
            m_InspecSize = new Rect(position.width-400, GapTop, 600, position.height - GapTop - GapBottom);

            CheckPlayingAssetData();
        }
        //-----------------------------------------------------
        public void OnDraw(int controllerId, Camera camera)
        {

        }
        //-----------------------------------------------------
        protected override void Update()
        {
            base.Update();
            m_Logic.Update(m_fDeltaTime);

            this.Repaint();
        }
        //-----------------------------------------------------
        private void OnGUI()
        {
            float width = position.width / 2 - 5 * 2;
            m_LayerSize.x = 0;
            m_LayerSize.y = GapTop;
            m_LayerSize.height = position.height - GapTop - GapBottom;

            m_InspecSize.x = position.width- m_InspecSize.width;
            m_InspecSize.y = GapTop;
            m_InspecSize.height = m_LayerSize.height;

            m_FrameSize.x = m_LayerSize.xMax;
            m_FrameSize.y = GapTop;
            m_FrameSize.width = position.width - m_FrameSize.x - m_InspecSize.width;
            m_FrameSize.height = m_LayerSize.height;

            GUILayout.BeginHorizontal();

            GUILayout.BeginHorizontal();
            DrawToolPanel();
            GUILayout.EndHorizontal();

            DrawLayerPanel();
            DrawInspectorPanel();
            DrawFramePanel();

            GUILayout.Space(5f);
            GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.MaxHeight(GapBottom) });


            GUILayout.EndHorizontal();

            if (Event.current != null)
                OnEvent(Event.current);

            GUILayout.EndHorizontal();

            this.Repaint();
        }
        //-----------------------------------------------------
        private void OnEvent(Event evt)
        {
            m_Logic.OnEvent(evt);
            if(evt.type == EventType.MouseDown)
            {
                if(m_DragSplitGap == 0)
                {
                    if (new Rect(position.x, GapTop, m_LayerSize.x, m_LayerSize.y).Contains(evt.mousePosition))
                        m_DragSplitGap = 1;
                    else if (new Rect(position.x- m_InspecSize.x, GapTop, m_InspecSize.x, m_InspecSize.y).Contains(evt.mousePosition))
                        m_DragSplitGap = 2;
                }
            }
            else if(evt.type == EventType.MouseDrag)
            {
                if(m_DragSplitGap == 1)
                {
                    m_LayerSize.x += evt.delta.x;
                    EditorGUIUtility.AddCursorRect(position, MouseCursor.SlideArrow);
                }
                else if (m_DragSplitGap == 2)
                {
                    m_InspecSize.x += evt.delta.x;
                    EditorGUIUtility.AddCursorRect(position, MouseCursor.SlideArrow);
                }
            }
            else if(evt.type == EventType.MouseUp)
            {
                m_DragSplitGap = 0;
            }
        }
        //-----------------------------------------------------
        public void CheckPlayingAssetData()
        {
            m_Logic.OnReLoadAssetData();
        }
        //-----------------------------------------------------
        public void OnReLoadAssetData()
        {
            m_Logic.OnReLoadAssetData();
        }
        //-----------------------------------------------------
        private void OnReLoad()
        {
            OnReLoadAssetData();
        }
        //-----------------------------------------------------
        public void OnDestroy()
        {
            m_Logic.Clear();
        }
        //-----------------------------------------------------
        private void DrawLayerPanel()
        {
            GUILayout.BeginArea(m_LayerSize);
            GUILayout.BeginVertical();

            this.m_layerPanelScrollPos = GUILayout.BeginScrollView(this.m_layerPanelScrollPos, new GUILayoutOption[] { GUILayout.Height(m_LayerSize.height) });
            m_Logic.OnDrawLayerPanel(m_LayerSize.size);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        private void DrawInspectorPanel()
        {
            GUILayout.BeginArea(m_InspecSize);

            GUILayout.BeginVertical("Box");

           
            this.m_inspectorPanelScrollPos = GUILayout.BeginScrollView(this.m_inspectorPanelScrollPos, new GUILayoutOption[] { GUILayout.Height(m_InspecSize.height) });
            m_Logic.OnDrawInspecPanel(m_InspecSize.size);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        private void DrawFramePanel()
        {
            Rect timeline = new Rect(m_LayerSize.xMax + 5, GapTop, position.width - m_LayerSize.width - m_InspecSize.width - 10, position.height-GapTop-GapBottom);
            if(m_Logic.isPlay())
                m_Timeline.SetCurTime(m_Logic.GetPlayTime());
            else
                m_Logic.SetPlayTime(m_Timeline.GetCurTime());
            m_Timeline.OnGUI(this, timeline);

            m_Logic.OnDrawFramePanel(m_FrameSize);
        }
        //-----------------------------------------------------
        private void DrawToolPanel()
        {
            //             if (GUILayout.Button("选择动作脚本", new GUILayoutOption[] { GUILayout.Width(120), GUILayout.Height(45f) }))
            //             {
            //                 Stop();
            //                 m_Logic.Load();
            //             }
            if (GUILayout.Button("新建动作脚本", new GUILayoutOption[] { GUILayout.Width(120), GUILayout.Height(45f) }))
            {
                   m_Logic.New();
            }
            //if (GUILayout.Button("PlayGraph", new GUILayoutOption[] { GUILayout.Width(120), GUILayout.Height(45f) }))
            //{
            //    m_Logic.BuildPlayableGraph();
            //}
            if (GUILayout.Button("重新加载csv", new GUILayoutOption[] { GUILayout.Width(120), GUILayout.Height(45f) }))
            {
                ReInit();
            }
            if (GUILayout.Button("播放", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                Play();
            }
            if (GUILayout.Button("停止", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                Stop();
            }
            if (GUILayout.Button("保存", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                m_Logic.SaveData();
            }
            if (GUILayout.Button("批量保存", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                m_Logic.SaveAlls();
            }
            if (GUILayout.Button("关联目录", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                EditorKits.OpenPathInExplorer(Application.dataPath + "/Datas/Config/ActionGraphs/");
                EditorKits.OpenPathInExplorer(Application.dataPath + "/Datas/Role/");
                EditorKits.OpenPathInExplorer(Application.dataPath + ActionStateManager.SAVE_PATH_ROOT);
            }
            if (GUILayout.Button("Close", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                base.Close();
            }

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 60;
            GUILayout.BeginArea(new Rect(760, 0, position.width-200- 760, 45));
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical();
                    {
                        for (int i = 0; i < m_vEventLayers.Count; ++i)
                        {
                            bool toggle = EditorGUILayout.Toggle(m_vEventLayers[i].strDisplay, (EditorEventMask & (int)m_vEventLayers[i].eventLayerBit) != 0, new GUILayoutOption[] { GUILayout.MaxWidth(90) });
                            if (toggle) EditorEventMask |= (int)m_vEventLayers[i].eventLayerBit;
                            else EditorEventMask &= ~(int)m_vEventLayers[i].eventLayerBit;
                            if ((i + 1) % 2 == 0)
                            {
                                GUILayout.EndVertical();
                                GUILayout.BeginVertical();
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    {
                        m_Logic.fActorEulerAngle = EditorGUILayout.Slider("模拟角度", m_Logic.fActorEulerAngle, -180, 180);
                        m_Logic.DrawPlaySwitch();
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();

            EditorGUIUtility.labelWidth = labelWidth;

            GUILayout.BeginArea(new Rect(position.width - 200,0,200,45));
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("SpeedSacle:", new GUILayoutOption[] { GUILayout.Width(80) });
                m_currentSnap = GUILayout.HorizontalSlider(m_currentSnap, 0.15f, 6f, new GUILayoutOption[] { GUILayout.Width(80f) });
                m_currentSnap = Mathf.Clamp(EditorGUILayout.FloatField(m_currentSnap, new GUILayoutOption[] { GUILayout.Width(20) }), 0.15f, 6);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        public float GetCurrentFrameScale()
        {
            return m_currentSnap;
        }
        //-----------------------------------------------------
        public void Play()
        {
            m_Logic.Play(true);
        }
        //-----------------------------------------------------
        public void Stop()
        {
            if (m_Logic.isPlay())
                m_Timeline.SetCurTime( m_Logic.GetPlayTime());
            else
                m_Timeline.SetCurTime(0);

            m_Logic.Play(false);
        }
        //-----------------------------------------------------
        void ReInit()
        {
        }
        //-----------------------------------------------------
        public void SetStateFrameCount(int cnt)
        {
            m_StateFrameCnt = cnt;
        }
    }
}
