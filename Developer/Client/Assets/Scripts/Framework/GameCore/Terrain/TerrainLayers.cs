/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	Terrain
作    者:	HappLI
描    述:	
*********************************************************************/
using ExternEngine;
using Framework.Base;
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
#if !USE_SERVER
using UnityEngine.AI;
#endif

namespace TopGame.Core
{
    public class TerrainLayers : ATerrainLayers
    {
#if !USE_SERVER
        protected NavMeshHit m_NavHit;
        protected List<TerrainLayerElement> m_LayerElements = new List<TerrainLayerElement>(64);
#endif
        //------------------------------------------------------
        public override EPhyTerrain GetHeight(FVector3 curPos, FVector3 curUp, ref FVector3 terrain, ref FVector3 normal, float maxDistance = 1f, float stepHeight = -1, AInstanceAble pIngore = null)
        {
            terrain = curPos;
            normal = curUp;
            if (maxDistance < 0) maxDistance = 10000;
#if !USE_SERVER
            if (m_LayerElements.Count > 0)
            {
                if (!Physics.autoSyncTransforms) Physics.SyncTransforms();
            }
            bool bEleHit = false;
            float mostheight = -1000f;
            Vector3 mostNormal = Vector3.up;
            for (int i = 0; i < m_LayerElements.Count; ++i)
            {
                if (!m_LayerElements[i]) continue;
                int result = m_LayerElements[i].Raycast(curPos, ref m_PhysicHit, maxDistance, pIngore);
                if (result > 0)
                {
                    if (m_PhysicHit.point.y > mostheight)
                    {
                        mostheight = m_PhysicHit.point.y;
                        mostNormal = m_PhysicHit.normal;
                    }
                    bEleHit = true;
                }
            }
            if (bEleHit)
            {
                normal = mostNormal;
                terrain.y = mostheight;
                return EPhyTerrain.Hit;
            }
#endif
            return base.GetHeight(curPos, curUp, ref terrain, ref normal, maxDistance, stepHeight, pIngore);
            // return false;
        }
        //------------------------------------------------------
        public override EPhyTerrain GetHeight(FVector3 curPos, FVector3 curUp, ref FVector3 terrain, ref FVector3 normal, ref int mask, float maxDistance = 1f, float stepHeight= -1, AInstanceAble pIngore = null)
        {
            terrain = curPos;
            normal = curUp;
            if (maxDistance < 0) maxDistance = 10000;
#if !USE_SERVER
            if (m_LayerElements.Count > 0)
            {
                if (!Physics.autoSyncTransforms) Physics.SyncTransforms();
            }
            bool bEleHit = false;
            float mostheight = -1000f;
            Vector3 mostNormal = Vector3.up;
            for (int i = 0; i < m_LayerElements.Count; ++i)
            {
                if (!m_LayerElements[i]) continue;
                int result = m_LayerElements[i].Raycast(curPos, ref m_PhysicHit, maxDistance, pIngore);
                if (result > 0)
                {
                    if (m_PhysicHit.point.y > mostheight)
                    {
                        mostheight = m_PhysicHit.point.y;
                        mostNormal = m_PhysicHit.normal;
                    }
                    bEleHit = true;
                }
            }
            if (bEleHit)
            {
                mask = m_nTerrainLayer;
                normal = mostNormal;
                terrain.y = mostheight;
                return EPhyTerrain.Hit;
            }
#endif
            return base.GetHeight(curPos, curUp, ref terrain, ref normal, ref mask, maxDistance, stepHeight, pIngore);
        }
        //------------------------------------------------------
        public override void Clear()
        {
            base.Clear();
        }
        //------------------------------------------------------
        protected override void OnAddTerrainZoom(int guid, List<Vector3> vPolygon, Vector3 centerPos, bool bClear)
        {
            if (vPolygon == null || vPolygon.Count < 3) return;
            int index = -1;
            Zoom zooms;
            for (int i = 0; i < m_LayerZooms.Count; ++i)
            {
                if (m_LayerZooms[i].guid == guid)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                zooms = new Zoom();
                zooms.polygons = new List<Polygon>(2);
                zooms.start = Vector3.one * 10000;
                zooms.end = Vector3.zero;
            }
            else
                zooms = m_LayerZooms[index];
            if (bClear)
            {
                if (zooms.polygons != null) zooms.polygons.Clear();
            }
            Polygon poly = new Polygon();
            poly.points = new List<Vector3>(vPolygon.Count);
            for (int i = 0; i < vPolygon.Count; ++i)
            {
                poly.points.Add(vPolygon[i] + centerPos);
                zooms.start = Vector3.Min(zooms.start, vPolygon[i] + centerPos);
                zooms.end = Vector3.Max(zooms.end, vPolygon[i] + centerPos);
            }
            zooms.polygons.Add(poly);
            if (index >= 0) m_LayerZooms[index] = zooms;
            else m_LayerZooms.Add(zooms);
        }
        //------------------------------------------------------
        protected override void OnRemoveTerrainZoom(int guid)
        {
            for (int i = 0; i < m_LayerZooms.Count; ++i)
            {
                if (m_LayerZooms[i].guid == guid)
                {
                    m_LayerZooms.RemoveAt(i);
                    break;
                }
            }
        }
#if !USE_SERVER
        //------------------------------------------------------
#if UNITY_EDITOR
        protected static bool ms_ShowNavMesh = false;
        protected static FFloat ms_CalculateDelta = 0;
        protected static Mesh ms_pDebugMesh = new Mesh();
        public void DebugDraw()
        {
            Color color = Gizmos.color;
            Gizmos.color = Color.red;
            Zoom zoom;
            for (int z = 0; z < m_LayerZooms.Count; ++z)
            {
                zoom = m_LayerZooms[z];
                for (int i = 0; i < zoom.polygons.Count; ++i)
                {
                    Polygon poly = zoom.polygons[i];
                    for (int j = 0; j < poly.points.Count; ++j)
                    {
                        Gizmos.DrawLine(poly.points[j], poly.points[(j + 1) % poly.points.Count]);
                    }
                }
            }
            if (DebugConfig.bShowGridMap)
            {
                if(Framework.Module.ModuleManager.mainFramework!=null) ATerrainLayers.DebugGridMap(Framework.Module.ModuleManager.mainModule);
            }
        }
#endif
        protected override void OnInnerUpdate(float fFrameTime)
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha9))
                ms_ShowNavMesh = !ms_ShowNavMesh;
            if (ms_ShowNavMesh)
            {
                ms_CalculateDelta -= fFrameTime;
                if (ms_CalculateDelta <= 0)
                {
                    NavMeshTriangulation tris = NavMesh.CalculateTriangulation();
                    if (tris.vertices != null)
                    {
                        ms_pDebugMesh.Clear();
                        Color[] colors = new Color[tris.vertices.Length];
                        for (int i = 0; i < ms_pDebugMesh.vertices.Length; ++i)
                            colors[i] = new Color(0, 1, 0, 0.25f);
                        ms_pDebugMesh.vertices = tris.vertices;
                        ms_pDebugMesh.colors = colors;
                        ms_pDebugMesh.triangles = tris.indices;
                    }
                    ms_CalculateDelta = 1;
                }
                Graphics.DrawMesh(ms_pDebugMesh, Matrix4x4.Translate(Vector3.up * 0.5f), Framework.Core.CommonUtility.debugMaterial, LayerMask.NameToLayer(GlobalDef.ms_foregroundLayerName));
            }
#endif
        }
        //------------------------------------------------------
        public NavMeshDataInstance AddNav(Vector3 pos, Quaternion rotation, NavMeshData pData)
        {
            return NavMesh.AddNavMeshData(pData, pos, rotation);
        }
        //------------------------------------------------------
        public void RemoveNav(NavMeshDataInstance pIns)
        {
            pIns.Remove();
        }
        //------------------------------------------------------
        public void AddLayerElement(TerrainLayerElement layerElement)
        {
            if (m_LayerElements.Contains(layerElement)) return;
            m_LayerElements.Add(layerElement);
        }
        //------------------------------------------------------
        public void RemoveLayerElement(TerrainLayerElement layerElement)
        {
            m_LayerElements.Remove(layerElement);
        }
#endif
    }
}