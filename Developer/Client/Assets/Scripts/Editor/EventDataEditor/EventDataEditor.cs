using System;
using System.Collections.Generic;
using TopGame.Base;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace TopGame.ED
{
    public class EventDataEditor : EditorWindow
    {
        public static EventDataEditor Instance { protected set; get; }

        public EventDataEditorLogic m_Logic;

        public float LeftWidth = 400;
        public float GapTop = 50f;
        public float GapBottom = 2;
        string m_strMsgHelp = "";
        //-----------------------------------------------------
        [MenuItem("Tools/事件编辑器")]
        private static void Init()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            if (EventDataEditor.Instance == null)
            {
                EventDataEditor window = EditorWindow.GetWindow<EventDataEditor>();
                window.title = "事件编辑器";
            }
        }
        //-----------------------------------------------------
        protected void OnDisable()
        {
//             if(EditorKits.PopMessageBox("提示", "是否保存后关闭", "保存", "不保存"))
//             {
//                 if (m_Logic != null)
//                     m_Logic.SaveData();
//             }
            if (m_Logic != null) m_Logic.OnDisable();
        }
        //-----------------------------------------------------
        protected void OnEnable()
        {
            if (Instance == null)
                EditorKits.NewScene();
     //       EditorModule moude = new EditorModule();
      //      moude.StartUp();

            Instance = this;
            base.minSize = new Vector2(850f, 320f);

            m_Logic = new EventDataEditorLogic();
            if (!EditorApplication.isPlaying)
            {

                m_Logic.OnReLoadAssetData();
            }
            else
            {
                if (Framework.Module.ModuleManager.mainModule == GameInstance.getInstance())
                    m_Logic.InitAsset();
            }

            if (m_Logic != null)
                m_Logic.OnEnable(this, null);
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
            GUILayout.BeginArea(new Rect(LeftWidth+5, GapTop, position.width - LeftWidth-5, height));

            GUILayout.BeginVertical("Box", new GUILayoutOption[0]);

            m_Logic.DrawInspectorPanel(new Vector2(position.width - LeftWidth - 5, height));
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        private void DrawToolPanel()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                if (m_Logic != null)
                    m_Logic.SaveData();
            }
            if (Framework.Module.ModuleManager.mainModule != GameInstance.getInstance() && GUILayout.Button("ReLoad", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                OnReLoad();
            }
            if (m_Logic.m_pCsvData != null && m_Logic.m_pCsvData.strFilePath!=null &&
                GUILayout.Button("关联文件", new GUILayoutOption[] { GUILayout.Width(100), GUILayout.Height(45f) }))
            {
                ED.EditorKits.OpenPathInExplorer(m_Logic.m_pCsvData.strFilePath);
            }
            if (GUILayout.Button("检测循环触发", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(45f) }))
            {
                if (m_Logic != null)
                    m_Logic.CheckLoopRecyleEvent();
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
            GUILayout.EndHorizontal();
        }
    }

}

