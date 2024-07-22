using System;
using System.Collections.Generic;
using TopGame.Base;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace TopGame.ED
{
    public class TextureCliperEditor : EditorWindow
    {
        public static string CLIP_PATH = "Assets/Datas/Config/TextureClipAssets.asset";
        public static TextureCliperEditor Instance { protected set; get; }

        public TextureCliperEditorLogic m_Logic;

        public float LeftWidth = 400;
        public float GapTop = 50f;
        public float GapBottom = 2;
        string m_strMsgHelp = "";
        //-----------------------------------------------------
        [MenuItem("Tools/图片裁切器", false, 10)]
        private static void Init()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            if (TextureCliperEditor.Instance == null)
            {
                TextureCliperEditor window = EditorWindow.GetWindow<TextureCliperEditor>();
                window.title = "图片裁切器";
            }
        }
        //-----------------------------------------------------
        protected void OnDisable()
        {
            if (m_Logic != null) m_Logic.OnDisable();
        }
        //-----------------------------------------------------
        protected void OnEnable()
        {
            if (Instance == null)
                EditorKits.NewScene();
            m_strMsgHelp = "鼠标中键拖动\r\n";
            m_strMsgHelp += "鼠标左键选中框体拖点进行移动\r\n";
            m_strMsgHelp += "鼠标左键+shift重新选框\r\n";

            Instance = this;
            base.minSize = new Vector2(850f, 320f);

            m_Logic = new TextureCliperEditorLogic();

            m_Logic.InitAsset();

            if (m_Logic != null)
                m_Logic.OnEnable(this);
        }
        //-----------------------------------------------------
        private void OnSelectionChange()
        {

        }
        //-----------------------------------------------------
        protected void Update()
        {
            if (m_Logic != null)
            {
                m_Logic.Update(0);
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

            base.RemoveNotification();
            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            DrawToolPanel();
            DrawLayerPanel();
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

        }
        //-----------------------------------------------------
        public void RemoveObject(UnityEngine.Object obj)
        {
            if (obj)
                GameObject.DestroyImmediate(obj);
        }
        //-----------------------------------------------------
        private void DrawLayerPanel()
        {
            float bottomHeight = GapBottom;
            float height = position.height - bottomHeight - GapTop;
            GUILayout.BeginArea(new Rect(0, GapTop, LeftWidth, height));
            GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(LeftWidth), GUILayout.Height(height) });

            GUILayout.BeginVertical("Box", new GUILayoutOption[0]);
            m_Logic.OnDrawLayerPanel(new Rect(0, 0, LeftWidth, height));
            GUILayout.EndVertical();

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        private void DrawInspectorPanel()
        {
            float height = position.height - GapBottom - GapTop;
            m_Logic.DrawInspectorPanel(new Rect(LeftWidth + 5, GapTop, position.width - LeftWidth - 5, height));
        }
        //-----------------------------------------------------
        private void DrawToolPanel()
        {
            if (GUILayout.Button("保存", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                if (m_Logic != null)
                    m_Logic.SaveData();
            }
            if (GUILayout.Button("重新加载", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                OnReLoad();
            }
            if (GUILayout.Button("重置", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                m_Logic.Reset();
            }
            if ( GUILayout.Button("关联文件", new GUILayoutOption[] { GUILayout.Width(100), GUILayout.Height(45f) }))
            {
                ED.EditorKits.OpenPathInExplorer(CLIP_PATH);
            }
            if (GUILayout.Button("关闭", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                if (m_Logic != null)
                    m_Logic.SaveData();

                base.Close();
            }
            GUILayout.TextArea(m_strMsgHelp, new GUILayoutOption[] { GUILayout.Height(45f) });
            if (Event.current != null && Event.current.isKey && Event.current.keyCode == KeyCode.Return)
            {
                GUI.FocusControl("");
            }
        }
    }

}

