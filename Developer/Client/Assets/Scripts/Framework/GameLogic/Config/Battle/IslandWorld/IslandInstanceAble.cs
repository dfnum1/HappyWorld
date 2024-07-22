/********************************************************************
生成日期:	2020-6-16
类    名: 	IslandInstanceAble
作    者:	happli
描    述:	岛屿对象
*********************************************************************/

using UnityEngine;
using System.Collections.Generic;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Logic
{
    public class IslandInstanceAble : Framework.Core.AInstanceAble
    {
        public List<Vector3Int> polygonRegion;
    }
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(IslandInstanceAble), true)]
    public class IslandInstanceAbleEditor : Editor
    {
        public bool m_IsEditoring = false;
        int m_nCellSize = 1;
        bool m_bMouseDownAdd;
        Vector3 m_RayHitPoint;
        public void OnSceneGUI()
        {
            IslandInstanceAble building = this.target as IslandInstanceAble;
            if (building == null) return;
            Vector3 center = building.transform.position;
            if (building.polygonRegion != null)
            {
                if (building.polygonRegion.Count > 1)
                {
                    for (int i = 0; i < building.polygonRegion.Count; ++i)
                    {
                        Vector3 point = building.polygonRegion[i] + center;
                        Handles.DrawLine(point, building.polygonRegion[(i + 1) % building.polygonRegion.Count] + center);
                    }
                }
                if(m_IsEditoring)
                {
                    if (!Event.current.shift)
                    {
                        for (int i = 0; i < building.polygonRegion.Count; ++i)
                        {
                            Vector3 point = building.polygonRegion[i] + center;
                            point = Handles.PositionHandle(point, Quaternion.identity) - center;
                            point.x = Mathf.FloorToInt(point.x / m_nCellSize) * m_nCellSize;
                            point.y = Mathf.FloorToInt(point.y / m_nCellSize) * m_nCellSize;
                            point.z = Mathf.FloorToInt(point.z / m_nCellSize) * m_nCellSize;
                            building.polygonRegion[i] = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), Mathf.FloorToInt(point.z));
                            Vector2 position2 = HandleUtility.WorldToGUIPoint(point + center);
                            {
                                GUILayout.BeginArea(new Rect(position2.x, position2.y, 100, 25));
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("point[" + i + "]");
                                if (GUILayout.Button("移除"))
                                {
                                    building.polygonRegion.RemoveAt(i);
                                    break;
                                }
                                GUILayout.EndHorizontal();
                                GUILayout.EndArea();
                            }
                            if (building.polygonRegion.Count > 1)
                            {
                                Vector3 insert = (building.polygonRegion[(i + 1) % building.polygonRegion.Count] + building.polygonRegion[i]) / 2 + center;
                                {
                                    Vector2 insertgui = HandleUtility.WorldToGUIPoint(insert);
                                    GUILayout.BeginArea(new Rect(insertgui.x, insertgui.y, 40, 25));
                                    if (GUILayout.Button("插入"))
                                    {
                                        point = insert - center;
                                        point.x = Mathf.FloorToInt(point.x / m_nCellSize) * m_nCellSize;
                                        point.y = Mathf.FloorToInt(point.y / m_nCellSize) * m_nCellSize;
                                        point.z = Mathf.FloorToInt(point.z / m_nCellSize) * m_nCellSize;
                                        building.polygonRegion.Insert((i + 1) % building.polygonRegion.Count, new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), Mathf.FloorToInt(point.z)));
                                        break;
                                    }
                                    GUILayout.EndArea();
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < building.polygonRegion.Count; ++i)
                        {
                            Vector3 point = building.polygonRegion[i] + center;
                            Handles.Label(point, "point[" + i + "]");
                            point = Handles.PositionHandle(point, Quaternion.identity) - center;
                            point.x = Mathf.FloorToInt(point.x / m_nCellSize) * m_nCellSize;
                            point.y = Mathf.FloorToInt(point.y / m_nCellSize) * m_nCellSize;
                            point.z = Mathf.FloorToInt(point.z / m_nCellSize) * m_nCellSize;
                            building.polygonRegion[i] = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), Mathf.FloorToInt(point.z));
                        }
                    }
                }
            }

            Camera cam = UnityEditor.SceneView.lastActiveSceneView.camera;
            if (cam == null) return;

            if (m_IsEditoring && Event.current!=null && Event.current.shift)
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
                        m_RayHitPoint = Framework.Core.BaseUtil.RayHitPos(aimRay);
                        Handles.DrawWireCube(m_RayHitPoint, Vector3.one * HandleUtility.GetHandleSize(m_RayHitPoint));
                    }
                    else if (Event.current.type == EventType.MouseUp)
                    {
                        if(m_bMouseDownAdd)
                        {
                            if (building.polygonRegion == null) building.polygonRegion = new List<Vector3Int>();
                            Ray aimRay = cam.ScreenPointToRay(mousePosition);
                            Vector3 pos = Framework.Core.BaseUtil.RayHitPos(aimRay);
                            if ((pos - m_RayHitPoint).sqrMagnitude <= 1f)
                            {
                                bool bEqual = false;
                                for(int i =0; i < building.polygonRegion.Count; ++i)
                                {
                                    if((building.polygonRegion[i]+center- pos).sqrMagnitude<=1)
                                    {
                                        bEqual = true;
                                        Vector3 point = pos - center;
                                        point.x = Mathf.FloorToInt(point.x / m_nCellSize) * m_nCellSize;
                                        point.y = Mathf.FloorToInt(point.y / m_nCellSize) * m_nCellSize;
                                        point.z = Mathf.FloorToInt(point.z / m_nCellSize) * m_nCellSize;
                                        building.polygonRegion[i] = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), Mathf.FloorToInt(point.z));
                                        break;
                                    }
                                }
                                if (!bEqual)
                                {
                                    Vector3 point = pos - center;
                                    point.x = Mathf.FloorToInt(point.x / m_nCellSize) * m_nCellSize;
                                    point.y = Mathf.FloorToInt(point.y / m_nCellSize) * m_nCellSize;
                                    point.z = Mathf.FloorToInt(point.z / m_nCellSize) * m_nCellSize;
                                    building.polygonRegion.Add(new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), Mathf.FloorToInt(point.z)));
                                }
                            }
                        }
                        m_bMouseDownAdd = false;
                    }
                }
            }
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.HelpBox("按shift 进行加点", MessageType.Info);
            m_IsEditoring = EditorGUILayout.Toggle("编辑", m_IsEditoring);
            m_nCellSize = EditorGUILayout.IntField("单位大小", m_nCellSize);
            IslandInstanceAble building = this.target as IslandInstanceAble;
            if (GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }

#endif
}
