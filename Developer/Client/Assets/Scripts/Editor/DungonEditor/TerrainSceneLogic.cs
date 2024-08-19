/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	DungonEditorLogic
作    者:	HappLI
描    述:	关卡编辑器
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;
using System.IO;
using TopGame.Data;
using TopGame.Core;
using UnityEditor.IMGUI.Controls;
using System.Reflection;
using Framework.Core;
using Framework.Base;
using Framework.ED;
using UnityEditor.PackageManager.UI;
using static Framework.ED.EditorUtil;
using TopGame.UI;
using DG.Tweening.Plugins.Core.PathCore;

namespace TopGame.ED
{
    public class TerrainSceneLogic
    {
        CsvData_BattleObject.BattleObjectData m_pCurItem;

        GridWireBounds m_pGridWireBox = new GridWireBounds();
        GameObject m_pPreveObject = null;

        TargetPreview m_preview;
        GUIStyle m_previewStyle;

        int m_nScanSize = 1;
        int m_nCellSize = 1;
        Vector3Int m_SceneSize = new Vector3Int(30, 30, 30);

        Vector3 m_CurrentWorldPos = Vector3.zero;

        List<List<Vector2Int>> m_vPaths = new List<List<Vector2Int>>();
        Mesh m_PathMesh = null;
        Material m_PathMaterial = null;

        TerrainGenerator m_pGenerator = new TerrainGenerator();
        DungonEditorLogic m_pLogic;
        //-----------------------------------------------------
        public void Enable(DungonEditorLogic pEditor)
        {
            m_pLogic = pEditor;
            m_pGenerator.Enable(this);
            setUpPreview();
            m_pGridWireBox.Init(Color.red);

            if (m_PathMaterial == null)
            {
                m_PathMaterial = new Material(Shader.Find("SD/Particles/SD_Alpha_Blended"));
                m_PathMaterial.hideFlags = HideFlags.DontSave;
            }
            if(m_PathMesh == null)
            {
                m_PathMesh = new Mesh();
                m_PathMesh.hideFlags = HideFlags.DontSave;
                m_PathMesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 0) };
                m_PathMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
                m_PathMesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
            }
        }
        //-----------------------------------------------------
        public void ShowNotification(string content, float showTime = 1)
        {
            m_pLogic.ShowNotification(content, showTime);
        }
        //-----------------------------------------------------
        public void setUpPreview()
        {
            GameObject[] roots = new GameObject[1];
            roots[0] = new GameObject("EditorRoot");

            if (m_preview == null)
                m_preview = new TargetPreview(m_pLogic.Editor);
            m_preview.AddPreview(roots[0]);

            TargetPreview.PreviewCullingLayer = 0;
            m_preview.SetCamera(0.01f, 10000f, 60f);
            m_preview.Initialize(roots);
            m_preview.SetPreviewInstance(roots[0]);

            m_preview.OnDrawAfterCB = OnDrawPreview;
            m_preview.OnMoseHitCB = this.OnMouseHit;
            m_preview.OnMoseMoveCB = this.PreviewMouseMove;
            m_preview.OnMosueUpCB = this.OnMosueUp;

        }
        //-----------------------------------------------------
        public void Disable()
        {
            Clear();
            m_pGenerator.Disable();

            if (m_preview != null)
                m_preview.Destroy();

            m_pGridWireBox.Destroy();

            if(m_PathMesh != null)EditorUtil.Destroy(m_PathMesh);
            if (m_PathMaterial) EditorUtil.Destroy(m_PathMaterial);
        }
        //-----------------------------------------------------
        public void Clear()
        {
            ClearTarget();
            m_pGenerator.Clear();
        }
        //-----------------------------------------------------
        void ClearTarget()
        {
            if (m_pPreveObject != null) GameObject.DestroyImmediate(m_pPreveObject);
            m_pPreveObject = null;
        }
        //-----------------------------------------------------
        public void Update(float fFrameTime)
        {
            m_pGenerator.Update(fFrameTime);
        }
        //-----------------------------------------------------
        public void reload()
        {
            m_pGenerator.reload();
        }
        //-----------------------------------------------------
        public void OnGUI()
        {          
        }
        //-----------------------------------------------------
        public void OnSceneGUI(SceneView view)
        {

        }
        //-----------------------------------------------------
        public void OnEvent(Event evt)
        {
            if(evt.type == EventType.KeyUp)
            {
                if(evt.keyCode == KeyCode.Escape)
                {
                    if(m_vPaths.Count>0)
                    {
                        m_vPaths.RemoveAt(m_vPaths.Count - 1);
                    }
                }
            }
        }
        //-----------------------------------------------------
        void OnDrawPreview(int controll, Camera camera, Event evt)
        {
            Handles.DrawWireCube(new Vector3(m_SceneSize.x*m_nCellSize * 0.5f, m_SceneSize.y * m_nCellSize * 0.5f, m_SceneSize.z * m_nCellSize * 0.5f), new Vector3(m_SceneSize.x * m_nCellSize, m_SceneSize.y * m_nCellSize, m_SceneSize.z * m_nCellSize));
            using (new HandlesColor(new Color(1, 1, 1, 0.25f)))
            {
                for (int x = 0; x < m_SceneSize.x * m_nCellSize; x += m_nCellSize)
                {
                    Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, 0, m_SceneSize.z* m_nCellSize));
                }
                for (int z = 0; z < m_SceneSize.z* m_nCellSize; z += m_nCellSize)
                {
                    Handles.DrawLine(new Vector3(0, 0, z), new Vector3(m_SceneSize.x * m_nCellSize, 0, z));
                }
            }
            if (m_pCurItem != null)
            {
                Quaternion rot = Quaternion.identity;
                Vector3 pos = Vector3.zero;
                if (m_pPreveObject != null)
                {
                    pos = m_pPreveObject.transform.position;
                    m_pPreveObject.transform.localScale = Vector3.one * m_pCurItem.sacle;
                    rot = m_pPreveObject.transform.rotation;
                }

                Color color = Handles.color;
                Handles.color = EditorKits.GetVolumeToColor(EVolumeType.Target);
                if (m_pCurItem.boxType == EActorCollisionType.BOX)
                {
                    Vector3 vMin = m_pCurItem.aabb_min;
                    Vector3 vMax = m_pCurItem.aabb_max;
                    Vector3 vCenter;
                    Vector3 vHalf;
                    vCenter = (vMax + vMin) * 0.5f;
                    vHalf = vMax - vCenter;
                    Framework.Core.CommonUtility.DrawBoundingBox(vCenter, vHalf, Matrix4x4.TRS(pos, rot, Vector3.one), EditorKits.GetVolumeToColor(EVolumeType.Target), false);

                    //     m_pGridWireBox.AddBox(m_pPreveObject.transform, vHalf, Vector3.zero);
                    //      m_pGridWireBox.RenderToPos(vCenter,camera);
                }
                else if (m_pCurItem.boxType == EActorCollisionType.CAPSULE)
                {
                    EditorKits.DrawWireSphere(pos, m_pCurItem.aabb_max.x, EditorKits.GetVolumeToColor(EVolumeType.Target), controll);
                }
                Handles.color = color;
            }

            for (int j = 0; j < m_vPaths.Count; ++j)
            {
                var path = m_vPaths[j];
                float colorF = ((j + 1) / m_vPaths.Count);
                using (new HandlesColor(new Color(colorF, 0, 0, 1)))
                {
                    for (int i = 0; i < path.Count - 1; ++i)
                    {
                      //  EditorUtil.DrawLine(ConvertWorld(path[i], true), ConvertWorld(path[i + 1], true), 1);
                        Handles.SphereHandleCap(controll, ConvertWorld(path[i], true), Quaternion.identity, m_nCellSize * 0.5f, EventType.Repaint);
                    }
                }
                if (path.Count > 0)
                {
                    using (new HandlesColor(Color.yellow))
                    {
                        Handles.SphereHandleCap(controll, ConvertWorld(path[path.Count - 1], true), Quaternion.identity, m_nCellSize * 0.5f, EventType.Repaint);
                    }
                }
            }

            var statusPoints = m_pGenerator.GetStautsPoints(0);
            if(statusPoints!=null)
            {
                using (new HandlesColor(new Color(1, 0, 0, 1)))
                {
                    foreach (var db in statusPoints)
                    {
                        Handles.SphereHandleCap(controll, ConvertWorld(m_pGenerator.IndexToGrid(db), true), Quaternion.identity, m_nCellSize * 0.5f, EventType.Repaint);
                    }
                }
            }
            statusPoints = m_pGenerator.GetStautsPoints(1);
            if (statusPoints != null)
            {
                using (new HandlesColor(new Color(0, 1, 0, 1)))
                {
                    foreach (var db in statusPoints)
                    {
                        Handles.SphereHandleCap(controll, ConvertWorld(m_pGenerator.IndexToGrid(db), true), Quaternion.identity, m_nCellSize * 0.5f, EventType.Repaint);
                    }
                }
            }
            statusPoints = m_pGenerator.GetStautsPoints(2);
            if (statusPoints != null)
            {
                using (new HandlesColor(new Color(0, 0, 1, 1)))
                {
                    foreach (var db in statusPoints)
                    {
                        Handles.SphereHandleCap(controll, ConvertWorld(m_pGenerator.IndexToGrid(db), true), Quaternion.identity, m_nCellSize * 0.5f, EventType.Repaint);
                    }
                }
            }

            statusPoints = m_pGenerator.GetStautsPoints(3);
            if (statusPoints != null)
            {
                using (new HandlesColor(new Color(0, 1, 1, 1)))
                {
                    foreach (var db in statusPoints)
                    {
                        Handles.SphereHandleCap(controll, ConvertWorld(m_pGenerator.IndexToGrid(db), true), Quaternion.identity, m_nCellSize * 0.5f, EventType.Repaint);
                    }
                }
            }

            var pathPoints = m_pGenerator.GetPaths();
            if (pathPoints != null)
            {
                using (new HandlesColor(new Color(1, 0, 1, 1)))
                {
                    foreach (var db in pathPoints)
                    {
                        Handles.SphereHandleCap(controll, ConvertWorld(m_pGenerator.IndexToGrid(db), true), Quaternion.identity, m_nCellSize * 0.5f, EventType.Repaint);
                    }
                }
            }
        }
        //-----------------------------------------------------
        public void OnDrawInspecPanel(Vector2 size)
        {
            DrawPreview(new Rect(0, 0, size.x, size.y));
        }
        //-----------------------------------------------------
        private void DrawPreview(Rect previewRect)
        {
            if (m_preview != null)
            {
                if (m_previewStyle == null)
                    m_previewStyle = new GUIStyle(EditorStyles.textField);
                m_preview.OnPreviewGUI(previewRect, m_previewStyle);
            }
        }
        //-----------------------------------------------------
        public void OnDrawLayerPanel(Vector2 size)
        {
            m_nCellSize = Math.Max(1, EditorGUILayout.IntField("单元大小", m_nCellSize));
            m_SceneSize = EditorGUILayout.Vector3IntField("地图大小", m_SceneSize);
            int preScanSize = m_nScanSize;
            m_nScanSize = EditorGUILayout.IntSlider("路径扩散", m_nScanSize, 1, Math.Max(m_SceneSize.x, m_SceneSize.z));
            if (preScanSize != m_nScanSize)
                m_pGenerator.RefreshScan(m_nScanSize);
            if (GUILayout.Button("随机地图"))
            {
                m_pGenerator.Generator(m_SceneSize, 1, m_nScanSize);
            }
        }
        //-----------------------------------------------------
        void OnMosueUp(Ray ray, Vector3 vWorldPos, Event evt)
        {
        }
        //-----------------------------------------------------
        void PreviewMouseMove(Ray ray, Vector3 vWorldPos, Event evt)
        {
            m_CurrentWorldPos = vWorldPos;
            if (evt.type == EventType.MouseDrag)
            {
                ray.direction = vWorldPos - ray.origin;
                GUI.FocusControl("");

                if(evt.button == 0)
                {
                }
            }
        }
        //-----------------------------------------------------
        void OnMouseHit(Ray ray, Vector3 hitPos, Event evt)
        {
        }
        //-----------------------------------------------------
        public Vector3 ConvertWorld(Vector2Int grid, bool center = false)
        {
            if(center)
                return new Vector3(grid.x * m_nCellSize + m_nCellSize*0.5f, 0, grid.y * m_nCellSize + m_nCellSize * 0.5f);

            return new Vector3(grid.x * m_nCellSize, 0, grid.y * m_nCellSize);
        }
        //-----------------------------------------------------
        public Vector2Int ConvertGrid(Vector3 world)
        {
            return new Vector2Int(Math.Max(0, Mathf.RoundToInt(world.x / m_nCellSize)), Math.Max(0, Mathf.RoundToInt(world.z / m_nCellSize)));
        }
        //-----------------------------------------------------
        public Vector3 AdjustWorld(Vector3 grid, bool center = false)
        {
            return ConvertWorld(ConvertGrid(grid));
        }
    }
}