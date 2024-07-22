/********************************************************************
生成日期:	24:7:2019   11:12
类    名: 	TimelinePanel
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.ED;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class GridZoomView
    {
        EditorWindow m_pWindow = null;

        public float minZoom = 1f;
        public float maxZoom = 1f;

        private bool m_bPressInView = false;
        public static bool isPanning { get; private set; }

        public Vector2 panOffset { get { return _panOffset; } set { _panOffset = value; Repaint(); } }
        private Vector2 _panOffset;
        public float zoom { get { return _zoom; } set { _zoom = Mathf.Clamp(value, minZoom, maxZoom); Repaint(); } }
        private float _zoom = 1;

        Rect m_Preivew;
        //------------------------------------------------------
        public GridZoomView(EditorWindow window)
        {
            m_pWindow = window;
        }
        //------------------------------------------------------
        public void OnEvent(Event e)
        {
            switch (e.type)
            {
                case EventType.ScrollWheel:
                    {
                        if( m_Preivew.Contains(e.mousePosition) )
                        {
                            float oldZoom = zoom;
                            if (e.delta.y > 0) zoom += 0.1f * zoom;
                            else zoom -= 0.1f * zoom;
                            panOffset += (1 - oldZoom / zoom) * (WindowToGridPosition(e.mousePosition) + panOffset);
                        }
                    }
                    break;
                case EventType.MouseDown:
                    {
                        m_bPressInView = m_Preivew.Contains(e.mousePosition);
                    }
                    break;
                case EventType.MouseUp:
                    {
                        m_bPressInView = false;
                    }
                    break;
                case EventType.MouseDrag:
                    {
                        if (m_bPressInView)
                        {
                            if (e.button == 1 || e.button == 2)
                            {
                                MovePan(e.delta);
                                isPanning = true;
                            }
                        }

                    }
                    break;
            }
        }
        //------------------------------------------------------
        public void MovePan(Vector2 delta)
        {
            panOffset += delta * zoom;
        }
        //------------------------------------------------------
        public void SetPanOffset(Vector2 offset)
        {
            panOffset = offset;
        }
        //------------------------------------------------------
        public void Draw(Rect preview)
        {
            m_Preivew = preview;
            GUILayout.BeginArea(m_Preivew);
            EditorUtil.DrawGrid(new Rect(0, 0, m_Preivew.width, m_Preivew.height), zoom, panOffset);
            GUILayout.EndArea();
        }
        //------------------------------------------------------
        public bool IsContains(Vector2 mouse)
        {
            return m_Preivew.Contains(mouse);
        }
        //------------------------------------------------------
        void Repaint()
        {
            if (m_pWindow) m_pWindow.Repaint();
        }
        //------------------------------------------------------
        public Vector2 WindowToGridPosition(Vector2 windowPosition)
        {
            return (windowPosition - m_Preivew.position - (panOffset / zoom)) * zoom;
        }
        //------------------------------------------------------
        public Vector2 GridToWindowPosition(Vector2 gridPosition)
        {
            return (panOffset / zoom) + (gridPosition / zoom) + m_Preivew.position;
        }
        //------------------------------------------------------
        public Vector2 GridToViewPosition(Vector2 gridPosition)
        {
            return (panOffset / zoom) + (gridPosition / zoom);
        }
    }
}
