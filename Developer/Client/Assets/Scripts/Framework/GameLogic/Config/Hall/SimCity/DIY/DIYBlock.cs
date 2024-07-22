/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	DiyConfig
作    者:	HappLI
描    述:	DIY配置
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using System.IO;
using TopGame.Core;
using UnityEngine;
//using Framework.Nav;
using Framework.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TopGame.Logic
{
    public class DIYBlock : AInstanceAble//, INavigationPoly
    {
        [System.Serializable]
        public struct Plygon
        {
            public List<Vector3> points;
            public void AddPoint(Vector3 pt)
            {
                if (points == null) points = new List<Vector3>();
                points.Add(pt);
            }
#if UNITY_EDITOR
            public void CheckCCW()
            {
                if (points == null || points.Count < 3) return;
                int i1, i2;
                i1 = i2 = 0;
                float area = 0f;
                for (; i1 < points.Count; ++i1)
                {
                    i2 = i1 + 1;
                    if (i2 == points.Count) i2 = 0;
                    area += points[i1].x * points[i2].z - points[i1].z * points[i2].x;
                }
                if(area <0) //CW
                {
                    points.Reverse();
                }
            }
#endif
        }
        public enum EUseType
        {
            [DisplayNameGUI("地面")] Land = 0,
            [DisplayNameGUI("阻挡")] Obstacle,
            [DisplayNameGUI("道路")] Road,
            [DisplayNameGUI("装饰")] Decorate,
        }
        public int useFlag = 1;
        public bool canRayHit = false;
        public List<Plygon> polygons;
        public Vector2Int Size;
        public GameObject Top;
        public GameObject Bottom;

        public bool IsType(EUseType type)
        {
            return (useFlag & 1<<(int)type)!=0;
        }

//         public void GetPolygons(ref List<NavPolyon> vTiles, ref List<NavPolyon> vObs)
//         {
//             if(polygons!=null && polygons.Count>0)
//             {
//                 for(int i =0; i < polygons.Count; ++i)
//                 {
//                     if (polygons[i].points == null || polygons[i].points.Count < 3)
//                         continue;
//                     NavPolyon navPly = new NavPolyon();
//                     navPly.offset = this.transform.position;
//                     navPly.points = polygons[i].points;
// 
//                     if (IsType(EUseType.Obstacle)) vObs.Add(navPly);
//                     else if (IsType(EUseType.Land) || IsType(EUseType.Road)) vTiles.Add(navPly);
//                 }
//             }
//             else
//             {
//                 if(Size.sqrMagnitude>0)
//                 {
//                     Vector2Int cellSize = Data.DIYBlockDatas.GetCellSize();
//                     int cellX = cellSize.x;
//                     int cellZ = cellSize.y;
//                     int sizeX = Size.x * cellSize.x;
//                     int sizeZ = Size.y * cellSize.y;
//                     NavPolyon navPly = new NavPolyon();
//                     navPly.offset = this.transform.position;
//                     navPly.size = new Vector2(sizeX, sizeZ);
//                     if (IsType(EUseType.Obstacle)) vObs.Add(navPly);
//                     else if (IsType(EUseType.Land) || IsType(EUseType.Road)) vTiles.Add(navPly);
// 
//                 }
//             }
//         }
    }
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DIYBlock), true)]
    public class DIYBlockEditor : Editor
    {
        Material m_pNavMaterial;
        Mesh m_pNavMesh;

        bool m_bExpandPolygon = false;
        bool m_bMouseDownAdd = false;
        Vector3 m_RayHitPoint = Vector3.zero;
        List<MeshCollider> m_vMeshColliders = new List<MeshCollider>();
        private void OnEnable()
        {
            if (m_pNavMesh == null)
                m_pNavMesh = new Mesh();
            if (m_pNavMaterial == null)
            {
                m_pNavMaterial = new Material(Shader.Find("SD/Editor/SD_NavEditor"));
                m_pNavMaterial.SetColor("_Color", new Color(0f, 1.0f, 0f, 0.6f));
            }
            DIYBlock block = target as DIYBlock;
            Renderer[] renders = block.GetComponentsInChildren<Renderer>();
            for(int i =0; i <renders.Length; ++i)
            {
                MeshCollider collider= renders[i].GetComponent<MeshCollider>();
                if (collider == null)
                {
                    collider = renders[i].gameObject.AddComponent<MeshCollider>();
                    m_vMeshColliders.Add(collider);
                }
                collider.hideFlags |= HideFlags.DontSaveInEditor; 
            }
            RefreshNavMesh();
        }
        private void OnDisable()
        {
            try
            {
                if(m_pNavMesh) UnityEngine.Object.DestroyImmediate(m_pNavMesh);
                m_pNavMesh = null;
                if(m_pNavMaterial) UnityEngine.Object.DestroyImmediate(m_pNavMaterial);
                m_pNavMaterial = null;
                for (int i = 0; i < m_vMeshColliders.Count; ++i)
                {
                    if (m_vMeshColliders[i]) UnityEngine.Object.DestroyImmediate(m_vMeshColliders[i]);
                }
            }
            catch (System.Exception ex)
            {
            	
            }
            m_vMeshColliders.Clear();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DIYBlock block = target as DIYBlock;
            EditorGUILayout.HelpBox("shift鼠标左键添加多边形导航区域", MessageType.Info);
            EditorGUI.BeginChangeCheck();
            block.useFlag = Framework.ED.EditorEnumPop.PopEnumBit("用途", block.useFlag, typeof(DIYBlock.EUseType), null, true);
            if (EditorGUI.EndChangeCheck()) SyncProperty("useFlag");

            EditorGUI.BeginChangeCheck();
            block.canRayHit = EditorGUILayout.Toggle("可点击", block.canRayHit);
            if (EditorGUI.EndChangeCheck()) SyncProperty("canRayHit");

            EditorGUI.BeginChangeCheck();
            block.Size = EditorGUILayout.Vector2IntField("Size", block.Size);
            if (EditorGUI.EndChangeCheck()) SyncProperty("Size");

            block.Top = EditorGUILayout.ObjectField("Top", block.Top, typeof(GameObject), true) as GameObject;
            block.Bottom = EditorGUILayout.ObjectField("Bottom", block.Bottom, typeof(GameObject), true) as GameObject;

            EditorGUILayout.BeginHorizontal();
            m_bExpandPolygon = EditorGUILayout.Foldout(m_bExpandPolygon, "导航多边形区域");
            
            if (GUILayout.Button("新建导航多边形区"))
            {
                if (block.polygons == null) block.polygons = new List<DIYBlock.Plygon>();
                block.polygons.Add(new DIYBlock.Plygon());
            }
            if (m_bExpandPolygon && block.polygons!=null)
            {
                EditorGUI.indentLevel++;
                for(int i =0; i < block.polygons.Count; ++i)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("区域" + (i + 1));
                    if(GUILayout.Button("-", new GUILayoutOption[] { GUILayout.MaxWidth(20) }))
                    {
                        block.polygons.RemoveAt(i);
                        break;
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("刷新"))
            {
                if(targets!=null)
                {
                    for (int i = 0; i < targets.Length; ++i)
                    {
                        EditorUtility.SetDirty(targets[i]);
                    }
                }

                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
            serializedObject.ApplyModifiedProperties();
        }
        //-----------------------------------------------------
        void SyncProperty(string field)
        {
            if(targets!=null)
            {
                for(int i =0; i < targets.Length; ++i)
                {
                    if (targets[i] == target) continue;
                    DIYBlock block = targets[i] as DIYBlock;
                    var fieldInfo = block.GetType().GetField(field, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if(fieldInfo!=null)
                    {
                        fieldInfo.SetValue(targets[i], fieldInfo.GetValue(target));
                    }
                }
            }
        }
        //-----------------------------------------------------
        void RefreshNavMesh()
        {
            m_pNavMesh.Clear();
            DIYBlock block = target as DIYBlock;
            if (block.polygons == null) return;
            Vector3 center = block.transform.position + Vector3.up*0.01f;

            List<int> tris = new List<int>();
            List<Vector3> vertices = new List<Vector3>();
            for (int j = 0; j < block.polygons.Count; ++j)
            {
                DIYBlock.Plygon ply = block.polygons[j];
                if (ply.points == null || ply.points.Count<3) continue;
                ply.CheckCCW();
                int curIndex = vertices.Count;
                for (int i = 0; i < ply.points.Count; ++i)
                {
                    Vector3 point = ply.points[i] + center;
                    vertices.Add(point);
                }
                for (int i =0; i < ply.points.Count-2; i++)
                {
                    tris.Add(curIndex);
                    tris.Add(i +1 + curIndex);
                    tris.Add(i + 2 + curIndex);
                }
            }
            m_pNavMesh.vertices = vertices.ToArray();
            m_pNavMesh.triangles = tris.ToArray();

        }
        //-----------------------------------------------------
        private void OnSceneGUI()
        {
            DIYBlock block = target as DIYBlock;
            DrawGrid(block.transform, block.Size, Data.DIYBlockDatas.GetCellSize());
            Camera cam = UnityEditor.SceneView.lastActiveSceneView.camera;
            if (cam == null) return;

            if (Event.current!=null)
            {
                Event evt = Event.current;
                if(evt.shift)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                }
                if (evt.type == EventType.MouseDown)
                {
                    if (evt.shift)
                        m_bMouseDownAdd = true;
                    else m_bMouseDownAdd = false;
                }
                else if(evt.type == EventType.MouseMove)
                {
                    Ray aimRay = cam.ScreenPointToRay(evt.mousePosition);
                    m_RayHitPoint = Framework.Core.CommonUtility.RayHitPos(aimRay);
                    Handles.DrawWireCube(m_RayHitPoint, Vector3.one * HandleUtility.GetHandleSize(m_RayHitPoint));
                }
                else if (evt.type == EventType.MouseUp)
                {
                    if(evt.shift)
                    {
                        if(m_bMouseDownAdd)
                        {
                            Ray ray = ED.EditorHelp.MouseToRay(evt.mousePosition, cam);
                            Vector3 mousePosition = evt.mousePosition;

                            bool bHit = false;
                            for(int i =0; i < m_vMeshColliders.Count; ++i)
                            {
                                RaycastHit hit;
                                if (m_vMeshColliders[i].Raycast(ray, out hit, 10000))
                                {
                                    bHit = true;
                                    Vector3 hitpos = hit.point;
                                    if (hitpos.y < 0) hitpos.y = 0;

                                    if (block.polygons == null) block.polygons = new List<DIYBlock.Plygon>();
                                    if (block.polygons.Count <= 0)
                                        block.polygons.Add(new DIYBlock.Plygon());

                                    DIYBlock.Plygon ply = block.polygons[block.polygons.Count - 1];
                                    ply.AddPoint(hitpos);
                                    block.polygons[block.polygons.Count - 1] = ply;
                                    RefreshNavMesh();
                                    if (UnityEditor.SceneView.lastActiveSceneView) UnityEditor.SceneView.lastActiveSceneView.Repaint();
                                    break;
                                }
                            }
                            if(!bHit)
                            {
                                if (block.polygons == null) block.polygons = new List<DIYBlock.Plygon>();
                                if (block.polygons.Count <= 0)
                                    block.polygons.Add(new DIYBlock.Plygon());

                                DIYBlock.Plygon ply = block.polygons[block.polygons.Count - 1];
                                ply.AddPoint(Framework.Core.BaseUtil.RayHitPos(ray));
                                block.polygons[block.polygons.Count - 1] = ply;
                                RefreshNavMesh();
                                if (UnityEditor.SceneView.lastActiveSceneView) UnityEditor.SceneView.lastActiveSceneView.Repaint();
                            }
                            m_bMouseDownAdd = false;
                        }
                    }
                }
            }
            if (block.polygons == null) return;

            if (m_pNavMesh != null && m_pNavMaterial != null)
                Graphics.DrawMesh(m_pNavMesh, Matrix4x4.identity, m_pNavMaterial, 0, cam, 0);

            Vector3 center = block.transform.position;
            if (!Event.current.shift)
            {
                for(int j =0; j < block.polygons.Count; ++j)
                {
                    DIYBlock.Plygon ply = block.polygons[j];
                    if (ply.points == null) continue;
                    for (int i = 0; i < ply.points.Count; ++i)
                    {
                        Vector3 point = ply.points[i] + center;
                        ply.points[i] = Handles.PositionHandle(point, Quaternion.identity) - center;
                        Vector2 position2 = HandleUtility.WorldToGUIPoint(point);
                        {
                            GUILayout.BeginArea(new Rect(position2.x, position2.y, 100, 25));
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("point[" + i + "]");
                            if (GUILayout.Button("移除"))
                            {
                                ply.points.RemoveAt(i);
                                RefreshNavMesh();
                                break;
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.EndArea();
                        }
                        if (ply.points.Count > 1)
                        {
                            Vector3 insert = (ply.points[(i + 1) % ply.points.Count] + ply.points[i]) / 2 + center;
                            {
                                Vector2 insertgui = HandleUtility.WorldToGUIPoint(insert);
                                GUILayout.BeginArea(new Rect(insertgui.x, insertgui.y, 40, 25));
                                if (GUILayout.Button("插入"))
                                {
                                    ply.points.Insert((i + 1) % ply.points.Count, insert - center);
                                    RefreshNavMesh();
                                    break;
                                }
                                GUILayout.EndArea();
                            }
                        }
                    }
                    block.polygons[j] = ply;
                }

                for (int j = 0; j < block.polygons.Count; ++j)
                {
                    DIYBlock.Plygon ply = block.polygons[j];
                    if (ply.points == null) continue;
                    for (int i = 0; i < ply.points.Count; ++i)
                    {
                        Vector3 point = ply.points[i] + center;
                        Handles.Label(point, "point[" + i + "]");
                        Vector3 prePt = ply.points[i];
                        ply.points[i] = Handles.PositionHandle(point, Quaternion.identity) - center;
                        if(!Framework.Core.CommonUtility.Equal(prePt, ply.points[i]))
                        {
                            RefreshNavMesh();
                        }
                        Handles.DrawLine(ply.points[i] + center, ply.points[(i + 1) % ply.points.Count] + center);
                    }
                    block.polygons[j] = ply;
                }

            }
            else
            {
                for (int j = 0; j < block.polygons.Count; ++j)
                {
                    DIYBlock.Plygon ply = block.polygons[j];
                    if (ply.points == null) continue;
                    for (int i = 0; i < ply.points.Count; ++i)
                    {
                        Vector3 point = ply.points[i] + center;
                        Handles.Label(point, "point[" + i + "]");
                        Handles.DrawLine(ply.points[i] + center, ply.points[(i + 1) % ply.points.Count] + center);
                    }
                }
            }
        }
        //-----------------------------------------------------
        public static void DrawGrid(Transform transform, Vector2Int Size, Vector2Int cellSize, bool bCenterOffset = false)
        {
            int cellX = cellSize.x;
            int cellZ = cellSize.y;
            int sizeX = Size.x * cellSize.x;
            int sizeZ = Size.y * cellSize.y;
            Vector3 halfSize = new Vector3(sizeX, 0, sizeZ) /2;
            Vector3 center = transform.position;
            if(bCenterOffset) center -= transform.forward* halfSize.z + transform.right* halfSize.x;
            for (int x = 0; x <= sizeX; x += cellX)
            {
                Handles.DrawLine(center + transform.right * x, center + transform.right * x + transform.forward * sizeZ);
            }

            for (int y = 0; y <= sizeZ; y += cellZ)
            {
                Handles.DrawLine(center + transform.forward*y, center + transform.forward*y + transform.right* sizeX);
            }
        }
    }
#endif
}

