#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Framework.Core;
using UnityEditor;

namespace TopGame.Core.Brush
{
    public class TerrainBrushEditorRuntime
    {
        public List<Foliage> m_Foliages = new List<Foliage>();
        public List<Foliage> GetFoliages()
        {
            return m_Foliages;
        }
        public int GetFoliagesCount()
        {
            return m_Foliages != null ? m_Foliages.Count : 0;
        }

        HashSet<long> m_Marks = new HashSet<long>();
        static Vector4[] ms_ExternCmds = new Vector4[1023];
        static Vector4[] ms_ExternParams = new Vector4[1023];
        static MaterialPropertyBlock ms_ComdProperty = null;
        bool m_bInstancingMat = true;

        TerrainBrush m_TerrainBrush = null;

        public bool useCommandBuffer = false;
        CommandBuffer m_CommandBuffer;

        private Dictionary<int, List<AInstanceAble>> m_vRuntimePrefabs = new Dictionary<int, List<AInstanceAble>>();
        private Dictionary<Brush, List<Foliage>>[] m_vRuntimes = new Dictionary<Brush, List<Foliage>>[(int)EBrushType.Count];
        //------------------------------------------------------
        public void Clear()
        {
            foreach (var db in m_vRuntimePrefabs)
            {
                if (db.Value == null) continue;
                for (int i = 0; i < db.Value.Count; ++i)
                {
                    FileSystemUtil.DeSpawnInstance(db.Value[i]);
                }
            }
            m_vRuntimePrefabs.Clear();
            for (int i = 0; i < (int)EBrushType.Count; ++i)
            {
                if (m_vRuntimes[i] != null) m_vRuntimes[i].Clear();
            }

            if (m_CommandBuffer != null) m_CommandBuffer.Clear();
        }
        //------------------------------------------------------
        public TerrainBrush GetTerrainBrush()
        {
            return m_TerrainBrush;
        }
        //------------------------------------------------------
        public void Init(TerrainBrush terrainBrush)
        {
            Clear();
            m_TerrainBrush = terrainBrush;
            if (terrainBrush == null) return;
            if (terrainBrush.Portals == null) return;
            m_Foliages = new List<Foliage>();
            for (int i = 0; i < m_TerrainBrush.Portals.Length; ++i)
            {
                if (m_TerrainBrush.Portals[i].foliages == null || m_TerrainBrush.Portals[i].foliages.Count <= 0) continue;
                m_Foliages.AddRange(m_TerrainBrush.Portals[i].foliages);
            }

            Refresh(false);
        }
        //------------------------------------------------------
        void Refresh(bool bClear = true)
        {
            if(bClear) Clear();
            if (m_Foliages == null) return;
            for (int i = 0; i < m_Foliages.Count; ++i)
            {
                Foliage foliage = m_Foliages[i];
                Brush brush = TerrainFoliageDatas.GetBrush(foliage.useIndex);
                if (brush == null) continue;
                foliage.brush = brush;
                AddRuntime(foliage, false, i);
            }
            m_bInstancingMat = TerrainFoliageDatas.IsInstancing();
        }
        //------------------------------------------------------
        public void SetFoliages(List<Foliage> foliages)
        {
            m_Foliages = foliages;
            Refresh();
        }
        //------------------------------------------------------
        public void Save()
        {
            if (m_TerrainBrush == null) return;
            HashSet<long> m_vMask = new HashSet<long>();
            //      if (brush.Portals == null || brush.Portals.Length <= 0)
            {
                m_TerrainBrush.Portals = new Portal[m_TerrainBrush.portalChunk * m_TerrainBrush.portalChunk];
                for (int i = 0; i < m_TerrainBrush.Portals.Length; ++i)
                {
                    m_TerrainBrush.Portals[i] = new Portal() { foliages = new List<Foliage>() };
                }
                for (int i = 0; i < m_Foliages.Count; ++i)
                {
                    Foliage foliage = m_Foliages[i];
                    Vector3 pos = foliage.position + m_TerrainBrush.transform.position;
                    long key = Mathf.RoundToInt(pos.x * 10) * 1000000000 + Mathf.RoundToInt(pos.y * 10) * 100000 + Mathf.RoundToInt(pos.z * 10);
                    if (m_vMask.Contains(key)) continue;
                    int index = GetPortalIndex(pos);
                    if (index >= 0)
                    {
                        m_TerrainBrush.Portals[index].foliages.Add(foliage);
                        m_vMask.Add(key);
                    }
                }
            }
            EditorUtility.SetDirty(m_TerrainBrush);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
        //------------------------------------------------------
        public void SetDirtyCmd()
        {

        }
        //------------------------------------------------------
        public bool HasFoliages(Vector3 curPos, float size, Brush brush = null)
        {
            if (m_Foliages == null || m_TerrainBrush == null) return false;
            for (int i = 0; i < m_Foliages.Count; ++i)
            {
                if ((m_Foliages[i].position + m_TerrainBrush.transform.position - curPos).magnitude <= size && (brush == null || brush.guid == m_Foliages[i].useIndex))
                {
                    return true;
                }
            }
            return false;
        }
        //------------------------------------------------------
        public bool RemoveFoliages(Vector3 curPos, float size, Brush brush = null, float removeRandom = 0)
        {
            if (m_Foliages == null || m_TerrainBrush == null) return false;
            bool bDirty = false;
            for (int i = 0; i < m_Foliages.Count;)
            {
                bool bInRange = false;
                Vector3 diff = m_Foliages[i].position + m_TerrainBrush.transform.position - curPos;
                diff.y = 0;
                bInRange = diff.magnitude <= size;
                if (bInRange && (brush == null || brush.guid == m_Foliages[i].useIndex))
                {
                    bool bRemoved = true;
                    if (removeRandom > 0)
                    {
                        if (UnityEngine.Random.Range(0, 100) > removeRandom)
                        {
                            bRemoved = false;
                        }
                    }
                    if (bRemoved)
                    {
                        bDirty = true;
                        long key = Mathf.RoundToInt(m_Foliages[i].position.x * 10) * 100000000 + Mathf.RoundToInt(m_Foliages[i].position.y * 10) * 10000 + Mathf.RoundToInt(m_Foliages[i].position.z * 10);
                        m_Marks.Remove(key);
                        m_Foliages.RemoveAt(i);
                    }
                    else
                        ++i;
                }
                else
                    ++i;
            }
            if (bDirty)
            {
                Refresh();
                return true;
            }
            return false;
        }
        //------------------------------------------------------
        public bool BrushColor(Vector3 curPos, float size, Color color, Brush brush = null)
        {
            if (m_Foliages == null || m_TerrainBrush == null) return false;
            bool bDirty = false;
            for (int i = 0; i < m_Foliages.Count; ++i)
            {
                Foliage foliage = m_Foliages[i];
                if ((foliage.position + m_TerrainBrush.transform.position - curPos).magnitude <= size && (brush == null || brush.guid == foliage.useIndex))
                {
                    bDirty = true;
                    foliage.color = new Vector3(color.r, color.g, color.b);
                    m_Foliages[i] = foliage;
                }
            }
            if (bDirty)
            {
                Refresh();
                return true;
            }
            return false;
        }
        //------------------------------------------------------
        public bool BrushSize(Vector3 curPos, float size, float sizeBegin, float sizeEnd, Brush brush = null)
        {
            if (m_Foliages == null || m_TerrainBrush == null) return false;
            bool bDirty = false;
            for (int i = 0; i < m_Foliages.Count; ++i)
            {
                Foliage foliage = m_Foliages[i];
                if ((foliage.position + m_TerrainBrush.transform.position - curPos).magnitude <= size && (brush == null || brush.guid == foliage.useIndex))
                {
                    bDirty = true;
                    foliage.scale = Vector3.one * UnityEngine.Random.Range(sizeBegin, sizeEnd);
                    m_Foliages[i] = foliage;
                }
            }
            if (bDirty)
            {
                Refresh();
                return true;
            }
            return false;
        }
        //------------------------------------------------------
        public bool AddRuntime(Foliage foliage, bool bAdd = true, int index = -1)
        {
            if (m_TerrainBrush == null || foliage.brush == null) return false;
            if (foliage.brush.type == EBrushType.Count || (int)foliage.brush.type >= m_vRuntimes.Length) return false;
            int typeIndex = (int)foliage.brush.type;

            long key = Mathf.RoundToInt(foliage.position.x * 10) * 100000000 + Mathf.RoundToInt(foliage.position.y * 10) * 10000 + Mathf.RoundToInt(foliage.position.z * 10);
            if (foliage.color.sqrMagnitude <= 0)
                foliage.color = Vector3.one;
            if (foliage.lightmap <= 0)
                foliage.lightmap = 1;
            if (bAdd)
            {
                if (m_Marks.Contains(key)) return false;
                if (m_Foliages == null) m_Foliages = new List<Foliage>();
                m_Foliages.Add(foliage);
                index = m_Foliages.Count - 1;
            }
            if (m_vRuntimes[typeIndex] == null)
                m_vRuntimes[typeIndex] = new Dictionary<Brush, List<Foliage>>();

            if (foliage.brush is BrushBuffer)
            {
                List<Foliage> vInstances = null;
                if (!m_vRuntimes[typeIndex].TryGetValue(foliage.brush, out vInstances))
                {
                    vInstances = new List<Foliage>();
                    m_vRuntimes[typeIndex].Add(foliage.brush, vInstances);
                }
                vInstances.Add(foliage);
            }
            else if (foliage.brush is BrushRes)
            {
                CreateInstanceFoliage(foliage);
            }
            m_Marks.Add(key);
            return bAdd;
        }
        //------------------------------------------------------
        void CreateInstanceFoliage(Foliage foliage)
        {
            if (m_TerrainBrush == null) return;
            GameObject pAble = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>((foliage.brush as BrushRes).strFile);
            if (pAble != null)
            {
                GameObject pInst = GameObject.Instantiate(pAble);
                AInstanceAble pPoolAble = pInst.GetComponent<AInstanceAble>();
                if (pPoolAble == null)
                    pPoolAble = pInst.AddComponent<AInstanceAble>();

                List<AInstanceAble> vAble;
                if (!m_vRuntimePrefabs.TryGetValue(foliage.useIndex, out vAble))
                {
                    vAble = new List<AInstanceAble>();
                    m_vRuntimePrefabs[foliage.useIndex] = vAble;
                }
                pPoolAble.SetParent(m_TerrainBrush.transform);
                pPoolAble.SetPosition(foliage.position + m_TerrainBrush.transform.position);
                pPoolAble.SetEulerAngle(foliage.eulerAngle + m_TerrainBrush.transform.eulerAngles);
                pPoolAble.SetScale(foliage.scale);
                vAble.Add(pPoolAble);
            }
        }
        //------------------------------------------------------
        bool IsSupportInstancing()
        {
            m_bInstancingMat = TerrainFoliageDatas.IsInstancing();
            return Framework.Base.ConfigUtil.IsSupportInstancing && m_bInstancingMat;
        }
        //------------------------------------------------------
        public bool GetPortalBound(int index, ref Bounds bounds)
        {
            if (m_TerrainBrush == null) return false;
            bounds.SetMinMax(Vector3.zero, Vector3.zero);
            if (index >= m_TerrainBrush.portalChunk * m_TerrainBrush.portalChunk) return false;
            int row = index / m_TerrainBrush.portalChunk;
            int col = index % m_TerrainBrush.portalChunk;
            Vector3 start = m_TerrainBrush.portalStart + m_TerrainBrush.transform.position;
            Vector3 min = new Vector3(start.x + col * m_TerrainBrush.portalSizeH, start.y, start.z + row * m_TerrainBrush.portalSizeV);
            Vector3 max = new Vector3(start.x + (col + 1) * m_TerrainBrush.portalSizeH, start.y + 100, start.z + (row + 1) * m_TerrainBrush.portalSizeV);
            bounds.SetMinMax(min, max);
            return true;
        }
        //------------------------------------------------------
        public int GetPortalIndex(Vector3 worldPos)
        {
            if (m_TerrainBrush == null) return -1;
            if (m_TerrainBrush.portalSizeH <= 0 || m_TerrainBrush.portalSizeV <= 0 || m_TerrainBrush.portalChunk <= 0) return -1;
            Vector3 start = m_TerrainBrush.portalStart + m_TerrainBrush.transform.position;
            Vector3 pos = worldPos - start;
            pos.x = Mathf.Clamp(pos.x, 0, m_TerrainBrush.portalChunk * m_TerrainBrush.portalSizeH);
            pos.z = Mathf.Clamp(pos.z, 0, m_TerrainBrush.portalChunk * m_TerrainBrush.portalSizeV);
            int col = Mathf.FloorToInt(pos.x / m_TerrainBrush.portalSizeH);
            int row = Mathf.FloorToInt(pos.z / m_TerrainBrush.portalSizeV);
            int index = row * m_TerrainBrush.portalChunk + col;
            if (index >= m_TerrainBrush.portalChunk * m_TerrainBrush.portalChunk) return -1;
            return index;
        }
        //------------------------------------------------------
        public void Draw(Camera camera, int layer = 0)
        {
            if (m_TerrainBrush == null) return;
            if (m_vRuntimes == null || camera == null) return;
            Matrix4x4 culling = camera.cullingMatrix;
            bool bCulling = true;
#if UNITY_EDITOR
            if (UnityEditor.SceneView.lastActiveSceneView && UnityEditor.SceneView.lastActiveSceneView.camera == camera)
                bCulling = false;
#endif
            for (int j = 0; j < m_vRuntimes.Length; ++j)
            {
                if (m_vRuntimes[j] == null) continue;
                Material material = TerrainFoliageDatas.GetMaterial((EBrushType)j);
                if (material == null) continue;
                foreach (var db in m_vRuntimes[j])
                {
                    if (db.Key is BrushBuffer)
                    {
                        FoliageLodMesh.Lod lod = db.Key.GetShareMesh(0);
                        if (lod == null) continue;
                        for (int i = 0; i < db.Value.Count; ++i)
                        {
                            if (bCulling)
                            {
                                if (!Framework.Base.IntersetionUtil.PositionInView(culling, db.Value[i].position + m_TerrainBrush.transform.position))
                                    continue;
                            }
                            Graphics.DrawMesh(lod.mesh, Matrix4x4.TRS(db.Value[i].position + m_TerrainBrush.transform.position, Quaternion.Euler(db.Value[i].eulerAngle + m_TerrainBrush.transform.eulerAngles), db.Value[i].scale),
                                material, layer, camera);
                        }
                    }
                }
            }

        }
        //------------------------------------------------------
        public void DrawInstance(Camera camera, int layer = 0)
        {
            if (m_TerrainBrush == null || m_vRuntimes == null || camera == null) return;

            if (!IsSupportInstancing())
            {
                Draw(camera, layer);
                return;
            }
            Matrix4x4 culling = camera.cullingMatrix;
            if (ms_ComdProperty == null) ms_ComdProperty = new MaterialPropertyBlock();

            if(useCommandBuffer)
            {
                if (m_CommandBuffer == null) m_CommandBuffer = new CommandBuffer();
                m_CommandBuffer.Clear();
            }

            for (int j = 0; j < m_vRuntimes.Length; ++j)
            {
                if (m_vRuntimes[j] == null) continue;
                Material material = TerrainFoliageDatas.GetMaterial((EBrushType)j);
                if (material == null) continue;
                foreach (var db in m_vRuntimes[j])
                {
                    if (db.Key is BrushBuffer)
                    {
                        FoliageLodMesh.Lod lod = db.Key.GetShareMesh(0);
                        if (lod == null || lod.mesh == null) continue;
                        int count = 0;
                        for (int i = 0; i < db.Value.Count; ++i)
                        {
                            ms_ExternParams[count] = new Vector4(db.Key.GetUvOffset().x, db.Key.GetUvOffset().y, 0, 0);
                            ms_ExternCmds[count] = new Vector4(db.Value[i].color.x * db.Value[i].lightmap, db.Value[i].color.y * db.Value[i].lightmap, db.Value[i].color.z * db.Value[i].lightmap, 0);
                            Framework.Core.CommonUtility.InstancingStacks[count] = Matrix4x4.TRS(db.Value[i].position + m_TerrainBrush.transform.position, Quaternion.Euler(db.Value[i].eulerAngle + m_TerrainBrush.transform.eulerAngles), db.Value[i].scale);

                            count++;
                            if (count >= 1023)
                            {
                                ms_ComdProperty.SetVectorArray("_ExternFactor", ms_ExternCmds);
                                ms_ComdProperty.SetVectorArray("_ExternParam", ms_ExternParams);
                                if (useCommandBuffer) m_CommandBuffer.DrawMeshInstanced(lod.mesh, 0, material, 0, Framework.Core.CommonUtility.InstancingStacks, count, ms_ComdProperty);
                                else Graphics.DrawMeshInstanced(lod.mesh, 0, material, Framework.Core.CommonUtility.InstancingStacks, count, ms_ComdProperty, UnityEngine.Rendering.ShadowCastingMode.Off, false, layer, camera);
                                count = 0;
                            }
                        }

                        if (count > 0)
                        {
                            ms_ComdProperty.SetVectorArray("_ExternFactor", ms_ExternCmds);
                            ms_ComdProperty.SetVectorArray("_ExternParam", ms_ExternParams);
                            if (useCommandBuffer) m_CommandBuffer.DrawMeshInstanced(lod.mesh, 0, material, 0, Framework.Core.CommonUtility.InstancingStacks, count, ms_ComdProperty);
                            else Graphics.DrawMeshInstanced(lod.mesh, 0, material, Framework.Core.CommonUtility.InstancingStacks, count, ms_ComdProperty, UnityEngine.Rendering.ShadowCastingMode.Off, false, layer, camera);
                            count = 0;
                        }
                    }
                }
            }
            if(useCommandBuffer)
            {
                Graphics.ExecuteCommandBuffer(m_CommandBuffer);
                m_CommandBuffer.Clear();
            }
        }
    }
}
#endif