/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ProjectileEditor
作    者:	HappLI
描    述:	飞行道具编辑器
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using TopGame.Data;
using Framework.Data;

namespace TopGame.ED
{
    public class ProjectileEditor : EditorWindow
    {
        EditorTimer m_pTimer = new EditorTimer();
        public static ProjectileEditor Instance { protected set; get; }

        public RenderTexture m_PreViewRT = null;

        public GUIStyle previewStyle;

        Vector2 m_layerPanelScrollPos = Vector2.zero;
        Vector2 m_inspectorPanelScrollPos = Vector2.zero;
        Vector2 m_framePanelScrollPos = Vector2.zero;

        static float GapTop = 50f;
        static float GapBottom = 4f;

        public List<string> m_vBoneList = new List<string>();
        float m_StateFrameCnt = 60;

        private Rect previewRect = new Rect(0, 0, 0, 0);

        private float m_fPlayTime = 0f;

        Rect m_LayerSize = new Rect();
        Rect m_InspecSize = new Rect();

        EditorModule m_pModuel = null;

        ProjectileEditorLogic m_Logic = new ProjectileEditorLogic();
        byte m_DragSplitGap = 0;
        TimelinePanel m_Timeline = new TimelinePanel();
        //-----------------------------------------------------
        [MenuItem("Tools/飞行道具编辑器 _F7")]
        private static void StartEditor()
        {
            if (Instance == null)
                EditorWindow.GetWindow<ProjectileEditor>();
            Instance.titleContent = new GUIContent("飞行道具编辑器");
        }
        //-----------------------------------------------------
        public void AppEdiorSetup(Camera camera)
        {
            OnReLoad();
        }
        //-----------------------------------------------------
        static public void OnSceneFunc(SceneView sceneView)
        {
            Instance.OnSceneGUI(sceneView);
        }
        //-----------------------------------------------------
        private void OnSceneGUI(SceneView sceneView)
        {
            if (m_Logic != null)
                m_Logic.OnSceneGUI();
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
            EditorWindow.GetWindow<ProjectileEditor>();
        }
        //-----------------------------------------------------
        protected void OnDisable()
        {
            Instance = null;

            m_Logic.OnDisable();

//             if (EditorApplication.isPlaying)
//                 EditorApplication.isPlaying = false;

            SceneView.duringSceneGui -= OnSceneFunc;
        }
        //-----------------------------------------------------
        protected void OnEnable()
        {
            Instance = this;
            EditorKits.NewScene();
            m_pModuel = new EditorModule();
            m_pModuel.StartUp();

            base.minSize = new Vector2(850f, 320f);
            m_Logic.OnEnable(this, m_pModuel);

            SceneView.duringSceneGui += OnSceneFunc;
        }
        //-----------------------------------------------------
        public void OnDraw(int controllerId, Camera camera)
        {

        }
        //-----------------------------------------------------
        protected void Update()
        {
            m_pTimer.Update();
            if (m_pModuel != null) Framework.Module.ModuleManager.UpdateSingle(m_pModuel, m_pTimer.deltaTime);
            m_Logic.Update(m_pTimer.deltaTime);
            this.Repaint();
        }
        //-----------------------------------------------------
        private void OnGUI()
        {

            float width = position.width / 3- 5 * 2;
            m_LayerSize = new Rect(0, GapTop, width, position.height - GapTop - GapBottom);
            m_InspecSize = new Rect(m_LayerSize.xMax+5, GapTop, position.width- (m_LayerSize.xMax + 5), position.height - GapTop - GapBottom);

            GUILayout.BeginHorizontal();

            GUILayout.BeginHorizontal();
            DrawToolPanel();
            GUILayout.EndHorizontal();

            DrawLayerPanel();
            DrawInspectorPanel();

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
        public void OnReLoadAssetData()
        {

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

            float LinkHeight = 20f;
            this.m_layerPanelScrollPos = GUILayout.BeginScrollView(this.m_layerPanelScrollPos, new GUILayoutOption[] { GUILayout.Height(m_LayerSize.height- LinkHeight) });
            m_Logic.OnDrawLayerPanel(m_LayerSize.size-new Vector2(0, LinkHeight));
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            if(GUILayout.Button("关联表格"))
            {
                CsvData_Projectile csvProj = DataEditorUtil.GetTable<CsvData_Projectile>();
                if(csvProj !=null)
                    EditorKits.OpenPathInExplorer(csvProj.strFilePath);
            }

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
        private void DrawToolPanel()
        {
            if (GUILayout.Button("刷新", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                Stop();
                m_Logic.Realod();
            }
            if (GUILayout.Button("模拟", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                Play();
            }
            if (GUILayout.Button("停止", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                Stop();
            }
            if (GUILayout.Button("新建", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                m_Logic.New();
            }
            if (GUILayout.Button("保存", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                m_Logic.SaveData();
            }
            if (GUILayout.Button("退出", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                base.Close();
            }
            GUILayout.BeginArea(new Rect(position.width - 200,0,200,45));
            GUILayout.BeginHorizontal();
            GUILayout.Label("SpeedSacle:", new GUILayoutOption[] { GUILayout.Width(80) });
            m_pTimer.m_currentSnap = GUILayout.HorizontalSlider(m_pTimer.m_currentSnap, 0.15f, 2f, new GUILayoutOption[] { GUILayout.Width(80f) });
            m_pTimer.m_currentSnap = Mathf.Clamp(EditorGUILayout.FloatField(m_pTimer.m_currentSnap, new GUILayoutOption[] { GUILayout.Width(20) }), 0.15f, 2);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        public void Play()
        {
            m_Logic.Play(true);
        }
        //-----------------------------------------------------
        public void Stop()
        {
            m_Logic.Play(false);
        }
        //-----------------------------------------------------
        public void SetStateFrameCount(int cnt)
        {
            m_StateFrameCnt = cnt;
        }
        //-----------------------------------------------------
        public void SetPlayTime(float time)
        {
            m_fPlayTime = time;
        }
    }
}
