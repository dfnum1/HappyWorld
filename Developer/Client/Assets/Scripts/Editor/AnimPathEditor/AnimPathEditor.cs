using Framework.ED;
using System;
using System.Collections.Generic;
using TopGame.Base;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace TopGame.ED
{
    public class AnimPathEditor : EditorWindow
    {
        public static AnimPathEditor Instance { protected set; get; }

        //   public FormWindow               m_FormView = null;
        private bool m_bPreivew = true;
        public TargetPreview m_preview;
        public GUIStyle previewStyle;

        public AnimPathEditorLogic m_Logic = null;

        public float LeftWidth = 350f;
        public float RightWidth = 350f;
        public float GapTop = 50f;
        public float GapBottom = 40f;

        private float m_PreviousTime;
        public float deltaTime = 0.02f;
        public float fixedDeltaTime = 0.02f;
        protected float m_fDeltaTime = 0f;
        protected float m_currentSnap = 1f;

        public GameObject m_pRoot = null;

        public Rect previewRect = new Rect(0, 0, 0, 0);
        string m_strMsgHelp = "";
        //-----------------------------------------------------
        [MenuItem("Tools/路径动画编辑器")]
        private static void Init()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            if (AnimPathEditor.Instance != null)
                AnimPathEditor.Instance.Close();
                AnimPathEditor window = EditorWindow.GetWindow<AnimPathEditor>();
                window.title = "镜头动画编辑器";
        }
        //-----------------------------------------------------
        protected void OnDisable()
        {
            if (m_Logic != null) m_Logic.OnDisable();
        }
        //-----------------------------------------------------
        protected void OnEnable()
        {
            if(Instance == null)
                EditorKits.NewScene();


            Instance = this;
            base.minSize = new Vector2(850f, 320f);
            m_Logic = new AnimPathEditorLogic();
            m_Logic.OnReLoadAssetData();

            setTargets();

            if (m_Logic != null)
                m_Logic.OnEnable(this,m_preview);

            m_strMsgHelp = "ESC  键：清除选择 \n";
            m_strMsgHelp += "F1   键：切换视图 \n";
            m_strMsgHelp += "F2   键：播放/停止 \n";
            m_strMsgHelp += "Ctl  键：选中移动 \n";
            m_strMsgHelp += "K    键：K帧 \n";
            m_strMsgHelp += "Del  键：删除选中帧 \n";

        }
        //-----------------------------------------------------
        public void setTargets()
        {
            if (m_preview == null)
                m_preview = new TargetPreview(this);

            GameObject[] roots = new GameObject[1];
            roots[0] = new GameObject("EditorRoot");
            Util.ResetGameObject(roots[0], EResetType.All);

            m_preview.AddPreview(roots[0]);

            TargetPreview.PreviewCullingLayer = 0;
            m_preview.SetCamera(0.01f, 10000f, 60f);
            m_preview.Initialize(roots);
            m_preview.SetPreviewInstance(roots[0] as GameObject);
            m_pRoot = (roots[0] as GameObject);


            m_preview.bLeftMouseForbidMove = true;
            m_preview.OnDrawAfterCB = this.OnDraw;
            m_preview.OnMoseHitCB = this.OnMouseHit;
            m_preview.OnMoseMoveCB = this.PreviewMouseMove;
            m_preview.OnMosueUpCB = this.OnMosueUp;
        }
        //-----------------------------------------------------
        private void OnSelectionChange()
        {

        }
        //-----------------------------------------------------
        public void OnDraw(int controllerId, Camera camera, Event evt)
        {
            if (m_Logic != null)
                m_Logic.OnDraw(controllerId, camera, TargetPreview.PreviewCullingLayer);
        }
        //-----------------------------------------------------
        protected void Update()
        {
            if (Application.isPlaying)
            {
                deltaTime = Time.deltaTime;
                m_fDeltaTime = (float)(deltaTime * m_currentSnap);
            }
            else
            {
                float curTime = Time.realtimeSinceStartup;
                m_PreviousTime = Mathf.Min(m_PreviousTime, curTime);//very important!!!

                deltaTime = curTime - m_PreviousTime;
                m_fDeltaTime = (float)(deltaTime * m_currentSnap * 0.5f);
            }

            m_PreviousTime = Time.realtimeSinceStartup;
            this.Repaint();
            if (m_preview != null)
            {
                m_preview.Update(m_fDeltaTime);
            }
            if (m_Logic != null)
            {
                m_Logic.Update(m_fDeltaTime);
            }
            this.Repaint();
        }
        //-----------------------------------------------------
        private void OnGUI()
        {
            if (m_Logic == null)
            {
                this.Reset();
                base.ShowNotification(new GUIContent("Init Failed."));
                return;
            }

            if (m_bPreivew && m_preview == null)
            {
                m_preview = new TargetPreview(this);
            }

            base.RemoveNotification();
            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            DrawToolPanel();
            DrawLayerPanel();
            if (m_bPreivew)
            {
                DrawPreview();
            }
            DrawInspectorPanel();

            if (m_Logic != null && Event.current != null)
            {
                m_Logic.OnEvent(Event.current);
            }

            m_Logic.OnGUI();
        }
        //-----------------------------------------------------
        static public void OnSceneFunc(SceneView sceneView)
        {
            //  DungeonEditor.Instance.OnSceneGUI(sceneView);
        }
        //-----------------------------------------------------
        private void OnSceneGUI(SceneView sceneView)
        {
            // Handles.BeginGUI();
            // Handles.PositionHandle(m_HitPos+Vector3.up, Quaternion.identity);
            // Handles.EndGUI();
        }
        //-----------------------------------------------------
        private void OnInspectorUpdate()
        {
            base.Repaint();
        }
        //-----------------------------------------------------
        private void Reset()
        {

        }
        //-----------------------------------------------------
        private void OnReLoad()
        {
            if (m_Logic != null)
            {
                m_Logic.SaveData();
                m_Logic.OnReLoadAssetData();
            }
        }
        //-----------------------------------------------------
        public void OnDestroy()
        {
            if (m_preview != null)
                m_preview.Destroy();
        }
        //-----------------------------------------------------
        public void RemoveObject(UnityEngine.Object obj)
        {
            if (obj)
                GameObject.DestroyImmediate(obj);
        }
        //-----------------------------------------------------
        private void DrawHelpBox()
        {
            Color color = GUI.color;
            GUI.color = Color.green;
            EditorGUILayout.HelpBox(m_strMsgHelp, MessageType.None);
            GUI.color = color;
        }
        //-----------------------------------------------------
        private void DrawLayerPanel()
        {
            float bottomHeight = 60 + GapBottom;
            GUILayout.BeginArea(new Rect(0, GapTop, LeftWidth, previewRect.height - bottomHeight));
            GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(LeftWidth), GUILayout.Height(previewRect.height - bottomHeight) });

            GUILayout.BeginVertical("Box", new GUILayoutOption[0]);

            DrawHelpBox();

            m_Logic.OnDrawLayerPanel(new Rect(0, 0, LeftWidth, previewRect.height - bottomHeight));
            GUILayout.EndVertical();

            GUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(0, position.height - bottomHeight, LeftWidth, bottomHeight));
            if (m_Logic.m_pPathAsset != null && GUILayout.Button("路径存储文件"))
                EditorKits.OpenPathInExplorer(m_Logic.m_pPathAsset.strFilePath);
            if (m_Logic.m_pPathAsset != null && GUILayout.Button("动画文件"))
                EditorKits.OpenPathInExplorer("Assets/Datas/AnimPaths/");
            GUILayout.EndArea();

        }
        //-----------------------------------------------------
        private void DrawPreview()
        {
            GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(previewRect.width), GUILayout.Height(previewRect.height) });
            if (m_preview != null)
            {

                previewRect.x = LeftWidth;
                previewRect.y = GapTop;
                previewRect.width = position.width - LeftWidth - RightWidth;
                previewRect.height = position.height - GapBottom - GapTop;

                if (this.previewStyle == null)
                    this.previewStyle = new GUIStyle(EditorStyles.textField);

                if (m_Logic == null || !m_Logic.bPreviewGame)
                    m_preview.OnPreviewGUI(previewRect, this.previewStyle);

                if (m_Logic != null)
                    m_Logic.OnDrawPreivew(previewRect);
            }
            GUILayout.EndVertical();
        }
        //-----------------------------------------------------
        private void DrawInspectorPanel()
        {
            GUILayout.BeginArea(new Rect(position.width - RightWidth, GapTop, RightWidth, previewRect.height));

            GUILayout.BeginVertical("Box", new GUILayoutOption[0]);

            m_Logic.DrawInspectorPanel(new Vector2(RightWidth, previewRect.height));
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        private void DrawToolPanel()
        {
            if (GUILayout.Button("LoadScene", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                if (m_Logic != null)
                    m_Logic.LoadScene();
            }
            if (GUILayout.Button("Save", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                if (m_Logic != null)
                    m_Logic.SaveData();
            }
            if (GUILayout.Button("ReLoad", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                OnReLoad();
            }
            if (m_Logic != null)
            {
                if (GUILayout.Button(m_Logic.m_bPlayPath ? "Stop" : "Play", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
                {
                    if (m_Logic != null)
                        m_Logic.Play();
                }
            }

            if (GUILayout.Button("Close", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                if (m_Logic != null)
                    m_Logic.SaveData();

                base.Close();
            }


            if (Event.current != null && Event.current.isKey && Event.current.keyCode == KeyCode.Return)
            {
                GUI.FocusControl("");
            }
        }
        //-----------------------------------------------------
        public void OnMosueUp(Ray ray, Vector3 vWorldPos, Event evt)
        {
            m_Logic.OnMouseUp(evt.button);
        }
        //-----------------------------------------------------
        public void PreviewMouseMove(Ray ray, Vector3 vWorldPos, Event evt)
        {
            if (evt.type == EventType.MouseDown)
            {
                ray.direction = vWorldPos - ray.origin;
                GUI.FocusControl("");
                m_Logic.OnMouseMove(vWorldPos, ray);
            }
        }
        //-----------------------------------------------------
        public void OnMouseHit(Ray ray, Vector3 hitPos, Event evt)
        {
            if (evt.button == 0)
            {
                ray.direction = hitPos - ray.origin;
                m_Logic.OnMouseHit(hitPos, ref ray);
            }
        }
        //-----------------------------------------------------
        private void SaveState()
        {

        }
    }

}

