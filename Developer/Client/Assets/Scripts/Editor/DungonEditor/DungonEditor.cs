/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	DungonEditor
作    者:	HappLI
描    述:	关卡编辑器
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using TopGame.Data;
using System.Linq;

namespace TopGame.ED
{
    public class DungonEditor : EditorWindow
    {
        private static string ms_saveLevelPath = null;
        private static string ms_saveThemePath = null;
        private static string ms_EditorPath = null;
        private static string ms_EditorLevelAttrsPath = null;
        public static string EDITOR_PATH
        {
            get
            {
                if (string.IsNullOrEmpty(ms_EditorPath))
                {
                    ms_EditorPath = Application.dataPath + "/../EditorData/Runnings/";
                }
                return ms_EditorPath;
            }
        }
        public static string EDITOR_ATTR_PATH
        {
            get
            {
                if (string.IsNullOrEmpty(ms_EditorLevelAttrsPath))
                {
                    ms_EditorLevelAttrsPath = Application.dataPath + "/../EditorData/Runnings/Atts/";
                }
                return ms_EditorLevelAttrsPath;
            }
        }
        public static string LEVEL_PATH
        {
            get
            {
                if (string.IsNullOrEmpty(ms_saveLevelPath))
                {
                    DirectoryInfo dir = new DirectoryInfo(Application.dataPath);
                    ms_saveLevelPath = dir.Parent.FullName.Replace("\\", "/");
                    if (!ms_saveLevelPath.EndsWith("/")) ms_saveLevelPath += "/";
                    ms_saveLevelPath += "Binarys/Runnings/Levels/";
                }
                return ms_saveLevelPath;
            }
        }
        public static string THEMES_PATH
        {
            get
            {

                if (string.IsNullOrEmpty(ms_saveThemePath))
                {
                    DirectoryInfo dir = new DirectoryInfo(Application.dataPath);
                    ms_saveThemePath = dir.Parent.FullName.Replace("\\", "/");
                    if (!ms_saveThemePath.EndsWith("/")) ms_saveThemePath += "/";
                    ms_saveThemePath += "Binarys/Runnings/Themes/";
                }
                return ms_saveThemePath;
            }
        }
        EditorTimer m_pTimer = new EditorTimer();
        public static DungonEditor Instance { protected set; get; }

        public RenderTexture m_PreViewRT = null;

        public GUIStyle previewStyle;

        Vector2 m_layerPanelScrollPos = Vector2.zero;
        Vector2 m_inspectorPanelScrollPos = Vector2.zero;
        Vector2 m_framePanelScrollPos = Vector2.zero;

        public DataManager m_pDataMgr;

        static float GapTop = 50f;
        static float GapBottom = 4f;
        float m_fToolBarPosX = 0;

        public List<string> m_vBoneList = new List<string>();
        float m_StateFrameCnt = 60;

        private Rect previewRect = new Rect(0, 0, 0, 0);

        private float m_fPlayTime = 0f;

        Rect m_LayerSize = new Rect();
        Rect m_InspecSize = new Rect();
        public Rect LayerRect { get { return m_LayerSize; } }
        public Rect InspecRect { get { return m_InspecSize; } }

        public DungonEditorLogic m_Logic = new DungonEditorLogic();
        byte m_DragSplitGap = 0;

        SearcherMenu m_SearchMenu = new SearcherMenu();

        Core.GameModule m_pGameModuel=null;

        //-----------------------------------------------------
        [MenuItem("Tools/关卡编辑器")]
        private static void StartEditor()
        {
            if (Instance == null)
                EditorWindow.GetWindow<DungonEditor>();
            Instance.titleContent = new GUIContent("关卡编辑器");
        }
//         //-----------------------------------------------------
//         [MenuItem("Tools/关卡数据导出")]
//         private static void ExprotLevelData()
//         {
//             string strRoot = Application.dataPath + "/../EditorData/Config/";
//             if(Directory.Exists(strRoot))
//             {
//                 LevelExportWindow.Open(strRoot);
//             }
//         }
        //-----------------------------------------------------
        public Core.GameModule GetModule()
        {
            if (Framework.Module.ModuleManager.mainModule != null) return Framework.Module.ModuleManager.mainModule as Core.GameModule;
            if(m_pGameModuel == null)
            {
                m_pGameModuel = new EditorModule();
                (m_pGameModuel as EditorModule).StartUp();
            }
            return m_pGameModuel;
        }
        //-----------------------------------------------------
        public void AppEdiorSetup(Camera camera)
        {
            OnReLoad();
        }
        //      public override bool IsManaged() { return false; }

        //-----------------------------------------------------
        public static DataManager GetDataer()
        {
            if (Instance == null) return null;
            return Instance.m_pDataMgr;
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
                m_Logic.OnSceneGUI(sceneView);
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

            if (GetModule() != null)
            {
                if (Framework.Module.ModuleManager.mainModule != GetModule())
                    GetModule().Destroy();
            }


            //if (EditorApplication.isPlaying)
            //    EditorApplication.isPlaying = false;

            SceneView.duringSceneGui -= OnSceneFunc;

        //    BattleObjectBatchEventPopup.Hide();

       //     RunDungonsLayout.ClearBuild();
        }
        //-----------------------------------------------------
        protected void OnEnable()
        {
            Instance = this;
            m_pDataMgr = new DataManager();
            DataManager.SetEditor(m_pDataMgr);
            Framework.Data.DataEditorUtil.ClearTables();

            base.minSize = new Vector2(850f, 320f);
            m_Logic.OnEnable(this);

            RenderSettings.ambientSkyColor = Color.white;

            SceneView.duringSceneGui += OnSceneFunc;

            ED.EditorKits.CheckEventCopy();
          //  BattleObjectBatchEventPopup.Hide();
        }
        //-----------------------------------------------------
        public void OnDraw(int controllerId, Camera camera)
        {

        }
        //-----------------------------------------------------
        protected void Update()
        {
            m_pTimer.Update();
            if (m_pGameModuel != null)
                Framework.Module.ModuleManager.UpdateSingle(m_pGameModuel, m_pTimer.deltaTime);
            m_Logic.Update(m_pTimer.deltaTime);
            if (m_SearchMenu != null)
            {
                m_SearchMenu.Update(m_pTimer.deltaTime);
            }
            this.Repaint();
        }
        //-----------------------------------------------------
        private void OnGUI()
        {
            float width = position.width / 3;
            m_LayerSize = new Rect(0, GapTop, width, position.height - GapTop - GapBottom);
            m_InspecSize = new Rect(m_LayerSize.xMax+5, GapTop, position.width- width, position.height - GapTop - GapBottom);

            EditorGUI.BeginDisabledGroup(m_SearchMenu.isOpen());

            GUILayout.BeginHorizontal();

            GUILayout.BeginHorizontal();
            DrawToolPanel();
            GUILayout.EndHorizontal();

            DrawLayerPanel();
            DrawInspectorPanel();

            m_Logic.OnGUI();

            GUILayout.Space(5f);
            GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.MaxHeight(GapBottom) });


            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();

            if (Event.current != null)
                OnEvent(Event.current);

            if (m_SearchMenu != null)
            {
                BeginWindows();
                m_SearchMenu.OnDraw();
                EndWindows();
            }

            this.Repaint();
        }
        //-----------------------------------------------------
        private void OnEvent(Event evt)
        {
            if(!m_SearchMenu.isOpen())
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
                if(evt.control) GUI.FocusControl("");
                m_DragSplitGap = 0;
            }
            else if(evt.type == EventType.KeyDown)
            {
                if(evt.keyCode == KeyCode.Escape)
                {
                    m_SearchMenu.Close();
                }
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
        private void DrawToolPanel()
        {
            if (GUILayout.Button("刷新", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                Stop();
                m_Logic.Realod();
            }
            if (GUILayout.Button("检测", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                m_Logic.Check();
            }
            if (GUILayout.Button("保存", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                m_Logic.SaveData();
            }
            if (GUILayout.Button("关联文件", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                EditorKits.OpenPathInExplorer(Application.dataPath + "/../Binarys/Runnings/" );
            }
            if (GUILayout.Button("一键提交", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                List<string> vPath = new List<string>();
                vPath.Add(EDITOR_PATH);
                vPath.Add(THEMES_PATH);
                vPath.Add(LEVEL_PATH);
                vPath.Add(EDITOR_ATTR_PATH);
                UnitySVN.SVNCommit(vPath.ToArray());
            }
            if (m_Logic.HasCtlTips() && GUILayout.Button("操作说明", new GUILayoutOption[] { GUILayout.Width(100f), GUILayout.Height(45f) }))
            {
                m_Logic.ShowCtlTips();
            }
            if (GUILayout.Button("退出", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                base.Close();
            }
            m_fToolBarPosX = Mathf.Max(m_fToolBarPosX, GUILayoutUtility.GetLastRect().xMax+10 );
            GUILayout.BeginArea(new Rect(m_fToolBarPosX, 0, this.position.width- m_fToolBarPosX, 45f));
            m_Logic.OnToolBarGUI();
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
        //-----------------------------------------------------
        public static SearcherMenu OpenSearchMenu(float x =0 ,float y = 0)
        {
            if (Instance == null) return null;
            Rect rect = new Rect();
            if (x!=0 || y != 0)
                rect = new Rect(x,y, 350, 400);
            else
                rect = new Rect((Instance.position.width - 350) / 2,(Instance.position.height - 400) / 2, 350, 400);
            Instance.m_SearchMenu.Open(rect);
            return Instance.m_SearchMenu;
        }

        //------------------------------------------------------
        public void OnSelectionChange()
        {
            if (Selection.activeTransform == null) return;
            Transform trans = Selection.activeTransform;
//             SceneEditObject pData = trans.GetComponentInParent<SceneEditObject>();
//             if (pData != null)
//                 Selection.activeTransform = pData.transform;
        }
    }
}
