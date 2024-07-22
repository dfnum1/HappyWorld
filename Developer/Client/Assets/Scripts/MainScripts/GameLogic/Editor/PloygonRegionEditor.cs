#if UNITY_EDITOR
/********************************************************************
生成日期:	5:24:2022  13:32
类    名: 	CityZoom
作    者:	HappLI
描    述:	主城区域组件
*********************************************************************/
using UnityEngine;
using Framework.Core;
using TopGame.Core;
using System.Collections.Generic;
using UnityEditor;

namespace TopGame.Logic
{
    public class PloygonRegionEditor : Editor
    {
        bool m_bMouseDownAdd = false;
        Vector3 m_RayHitPoint = Vector3.zero;
        //------------------------------------------------------
        public void OnEditorPolygon(Transform transform, ref List<Vector3> vPolygonRegion)
        {
            MonoBehaviour pTransMon = this.target as MonoBehaviour;
            if (pTransMon == null) return;
            if (vPolygonRegion == null) vPolygonRegion = new List<Vector3>();
            Handles.Label(pTransMon.transform.position, "中心");
            pTransMon.transform.position = Handles.DoPositionHandle(pTransMon.transform.position, Quaternion.identity);
            Vector3 center = pTransMon.transform.position + Vector3.up * 5;
            if (vPolygonRegion.Count > 1)
            {
                for (int i = 0; i < vPolygonRegion.Count; ++i)
                {
                    Vector3 point = vPolygonRegion[i] + center;
                    Handles.DrawLine(point, vPolygonRegion[(i + 1) % vPolygonRegion.Count] + center);
                }
            }
            if (!Event.current.shift)
            {
                for (int i = 0; i < vPolygonRegion.Count; ++i)
                {
                    Vector3 point = vPolygonRegion[i] + center;
                    vPolygonRegion[i] = Handles.PositionHandle(point, Quaternion.identity) - center;
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(point);
                    {
                        GUILayout.BeginArea(new Rect(position2.x, position2.y, 100, 25));
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("point[" + i + "]");
                        if (GUILayout.Button("移除"))
                        {
                            vPolygonRegion.RemoveAt(i);
                            break;
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.EndArea();
                    }
                    if (vPolygonRegion.Count > 1)
                    {
                        Vector3 insert = (vPolygonRegion[(i + 1) % vPolygonRegion.Count] + vPolygonRegion[i]) / 2 + center;
                        {
                            Vector2 insertgui = HandleUtility.WorldToGUIPoint(insert);
                            GUILayout.BeginArea(new Rect(insertgui.x, insertgui.y, 40, 25));
                            if (GUILayout.Button("插入"))
                            {
                                vPolygonRegion.Insert((i + 1) % vPolygonRegion.Count, insert - center);
                                break;
                            }
                            GUILayout.EndArea();
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < vPolygonRegion.Count; ++i)
                {
                    Vector3 point = vPolygonRegion[i] + center;
                    Handles.Label(point, "point[" + i + "]");
                    vPolygonRegion[i] = Handles.PositionHandle(point, Quaternion.identity) - center;
                }
            }


            Camera cam = UnityEditor.SceneView.lastActiveSceneView.camera;
            if (cam == null) return;

            if (Event.current != null && Event.current.shift)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                if (Event.current.button == 0)
                {
                    Vector3 mousePosition = Event.current.mousePosition;
                    float mult = 1;
#if UNITY_5_4_OR_NEWER
                    mult = EditorGUIUtility.pixelsPerPoint;
#endif
                    mousePosition.y = cam.pixelHeight - mousePosition.y * mult;
                    mousePosition.x *= mult;
                    if (Event.current.type == EventType.MouseDown)
                    {
                        m_bMouseDownAdd = true;
                        Ray aimRay = cam.ScreenPointToRay(mousePosition);
                        m_RayHitPoint = Framework.Core.CommonUtility.RayHitPos(aimRay);
                        Handles.DrawWireCube(m_RayHitPoint, Vector3.one * HandleUtility.GetHandleSize(m_RayHitPoint));
                    }
                    else if (Event.current.type == EventType.MouseUp)
                    {
                        if (m_bMouseDownAdd)
                        {
                            Ray aimRay = cam.ScreenPointToRay(mousePosition);
                            Vector3 pos = Framework.Core.CommonUtility.RayHitPos(aimRay);
                            if ((pos - m_RayHitPoint).sqrMagnitude <= 1f)
                            {
                                bool bEqual = false;
                                for (int i = 0; i < vPolygonRegion.Count; ++i)
                                {
                                    if ((vPolygonRegion[i] + center - pos).sqrMagnitude <= 1)
                                    {
                                        bEqual = true;
                                        vPolygonRegion[i] = pos - center;
                                        break;
                                    }
                                }
                                if (!bEqual)
                                    vPolygonRegion.Add(pos - center);
                            }
                        }
                        m_bMouseDownAdd = false;
                    }
                }
            }
        }
    }
}
#endif