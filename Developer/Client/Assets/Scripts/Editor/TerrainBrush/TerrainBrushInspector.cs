#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TopGame.Core.Brush
{
    [CustomEditor(typeof(TerrainBrush))]
    public class TerrainBrushInspector : Editor
    {
        private TerrainBrushEditor m_pEditor;
        private void OnEnable()
        {
            m_pEditor = new TerrainBrushEditor(target as TerrainBrush,true, serializedObject);
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            m_pEditor.Destroy();
            m_pEditor = null;
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
         //   base.OnInspectorGUI();
            m_pEditor.OnInspectorGUI();
        }
        //------------------------------------------------------
        public void OnSceneGUI()
        {
            Camera cam = UnityEditor.SceneView.lastActiveSceneView.camera;
            Vector2 mousePos = Event.current.mousePosition;
            mousePos.y = UnityEditor.SceneView.lastActiveSceneView.position.height - mousePos.y - 40;
            Ray aimRay = cam.ScreenPointToRay(mousePos);
            m_pEditor.OnSceneGUI(cam, aimRay, Event.current);
        }
    }
}
#endif