using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace TopGame.ED
{
    public class ParticleSortLayerSetterEditor : EditorWindowBase
    {
        public static ParticleSortLayerSetterEditor Instance { protected set; get; }


        public ParticleSortLayerSetterEditorLogic m_Logic;

        private GameObject m_pRoot = null;

        public Rect previewRect = new Rect(0, 0, 0, 0);
        string m_strMsgHelp = ""; 
        //-----------------------------------------------------
        [MenuItem("Tools/特效排序编辑器")]
        private static void Init()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("Waring", "Please wating compile over do it", "ok");
                return;
            }
            if (ParticleSortLayerSetterEditor.Instance == null)
            {
                ParticleSortLayerSetterEditor window = EditorWindow.GetWindow<ParticleSortLayerSetterEditor>();
                window.titleContent.text = "特效排序编辑器";
            }
        }
        //-----------------------------------------------------
        protected override void OnInnerDisable()
        {
            if (m_Logic != null) m_Logic.OnDisable();
        }
        //-----------------------------------------------------
        protected override void OnInnerEnable()
        {
            Instance = this;
            base.minSize = new Vector2(850f, 320f);

            m_Logic = new ParticleSortLayerSetterEditorLogic();

            if (m_Logic != null)
                m_Logic.OnEnable();
            m_strMsgHelp = "1.选择需要排序的特效文件的根目录\r\n2.选择排序层最低层级的单元\r\n3.点击排序，排序结束后，会自动计算新的最低层级\r\n4.特效层级有最大上限不能超过32767"; 
        }
        //-----------------------------------------------------
        public void setTargets(UnityEngine.Object[] targets)
        {
        }
        //-----------------------------------------------------
        private void OnSelectionChange()
        {

        }
        //------------------------------------------------------
        protected override void Update()
        {
            base.Update();
        }
        //-----------------------------------------------------
        private void OnGUI()
        {
            if (m_Logic == null)
            {
                base.ShowNotification(new GUIContent("Init Failed."));
                return;
            }

            base.RemoveNotification();
            if (m_Logic != null && Event.current != null)
            {
                m_Logic.OnEvent(Event.current);
            }
            GUILayout.BeginVertical("Box", new GUILayoutOption[0]);
            DrawHelpBox();
            m_Logic.OnGUI();
            GUILayout.EndVertical();
        }
        //-----------------------------------------------------
        private void OnInspectorUpdate()
        {
            base.Repaint();
        }
        //-----------------------------------------------------
        private void OnReLoad()
        {
            if (m_Logic != null)
            {
                m_Logic.SaveData();
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
        private void DrawHelpBox()
        {
            Color color = GUI.color;
            GUI.color = Color.green;
            EditorGUILayout.HelpBox(m_strMsgHelp, MessageType.None);
            GUI.color = color;
        }
    }
}
