/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	TerrainManager
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Base;
using Framework.Core;
using System;
using System.Collections.Generic;
#if !USE_SERVER
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
#endif
namespace TopGame.Core
{
#if !USE_SERVER
    struct FroceWind
    {
        public Transform transform;
        public float distance;
        public Vector3 position;
        public float force;
        public float damping;
    }
#endif

    public class TerrainManager : ATerrainManager
    {
#if !USE_SERVER
        public struct BrushDummy
        {
            public int instanceID;
            public Vector3 startPos;
            public Vector3 eulerAngle;
            public float visibleCheck;
            public Vector3 portalStart;
            public int portalSizeH;
            public int portalSizeV;
            public int portalChunk;
            public Bounds bounds;
            public Brush.Portal[] Portals;
            public bool bOnlyDepth;
#if UNITY_EDITOR
            public MonoBehaviour binder;
#endif
            public bool InView(Vector3 rootPos, Plane[] frustum)
            {
                Vector3 minBounds = portalStart + GetStartPosition() + rootPos;
                Vector3 maxBounds = minBounds + new Vector3(portalSizeH * portalChunk, 100, portalSizeV * portalChunk);
                bounds.SetMinMax(minBounds, maxBounds);
                return Framework.Core.BaseUtil.BoundInView(frustum, bounds);
            }

            public bool InView(Vector3 rootPos, Plane[] frustum, int portalIndex)
            {
                if (portalIndex >= portalChunk * portalChunk) return false;
                int row = portalIndex / portalChunk;
                int col = portalIndex % portalChunk;
                Vector3 start = portalStart + GetStartPosition() + rootPos;
                Vector3 minBounds = new Vector3(start.x + col * portalSizeH, start.y, start.z + row * portalSizeV);
                Vector3 maxBounds = new Vector3(start.x + (col + 1) * portalSizeH, start.y + 100, start.z + (row + 1) * portalSizeV);
                bounds.SetMinMax(minBounds, maxBounds);
                return Framework.Core.BaseUtil.BoundInView(frustum, bounds);
            }

            public Vector3 GetStartPosition()
            {
                return startPos;
            }
            public Vector3 GetStartEulerAngle()
            {
                return eulerAngle;
            }
        }

        //! aysnc thread parameters
        byte m_bDirtyBrush = 0;
        List<BrushDummy> m_vBrushing = new List<BrushDummy>();
        HashSet<int> m_vRemoveBrushing = new HashSet<int>();

        private List<Brush.Foliage> m_vStandInstances = new List<Brush.Foliage>();

        private Dictionary<int, List<AInstanceAble>> m_vRuntimePrefabs = new Dictionary<int, List<AInstanceAble>>();
        private List<BrushCmdStack>[] m_vRuntimeBrushs = new List<BrushCmdStack>[(int)Brush.EBrushType.Count];

        int m_nLayer = 0;
        int m_bDirtyCmd = 0;
        CommandBuffer m_cmdBuff = null;
        bool m_bInstancingMat = true;

        static int ms_ExternFactorID = 0;
        static MaterialPropertyBlock ms_ComdProperty = null;

        bool m_bCullingCheck = true;
        float m_fCullingFactor = 1.1f;
        Camera m_pCurCamera = null;
        private Plane[] m_Frustum = new Plane[(int)EFrustumType.kPlaneFrustumNum];
        private Matrix4x4 m_CullingMatrix = Matrix4x4.identity;
        Vector3 m_LastCameraPos = Vector3.zero;
        Vector3 m_LastCaemraEulerAngle = Vector3.zero;
        Vector3 m_LastCaemraDirection = Vector3.forward;

        Vector3 m_TerrainBrushRootPos = Vector3.zero;

        List<FroceWind> m_vForceWinds = new List<FroceWind>(4);
#endif

        static TerrainManager ms_Instance = null;
#if !USE_SERVER
        //------------------------------------------------------
        public static void OnAddBrushDummy(BrushDummy brushDummy)
        {
            if (ms_Instance == null) return;
            ms_Instance.m_vRemoveBrushing.Remove(brushDummy.instanceID);
            for (int i = 0; i < ms_Instance.m_vBrushing.Count; ++i)
            {
                if (ms_Instance.m_vBrushing[i].instanceID == brushDummy.instanceID)
                {
                    return;
                }
            }
            brushDummy.bounds = new Bounds();
            ms_Instance.m_vBrushing.Add(brushDummy);
            if (ms_Instance.m_bDirtyBrush <= 0)
                ms_Instance.m_bDirtyBrush = 2;
        }
        //------------------------------------------------------
        public static void OnRemoveBrushDummy(int instanceID)
        {
            if (ms_Instance == null) return;
            if (ms_Instance.m_vRemoveBrushing.Contains(instanceID)) return;
            ms_Instance.m_vRemoveBrushing.Add(instanceID);
            if (ms_Instance.m_bDirtyBrush <= 0)
                ms_Instance.m_bDirtyBrush = 2;
        }
        //------------------------------------------------------
        public static void UpdateBrushDummy(Brush.TerrainBrush terrainBrush)
        {
            if (ms_Instance == null) return;
            int instanceID = terrainBrush.GetInstanceID();
            BrushDummy brushDummy;
            for (int i = 0; i < ms_Instance.m_vBrushing.Count; ++i)
            {
                brushDummy = ms_Instance.m_vBrushing[i];
                if (brushDummy.instanceID == instanceID)
                {
                    brushDummy.startPos = terrainBrush.transform.position;
                    brushDummy.eulerAngle = terrainBrush.transform.eulerAngles;
                    ms_Instance.m_vBrushing[i] = brushDummy;
                    if (ms_Instance.m_bDirtyBrush <= 0)
                        ms_Instance.m_bDirtyBrush = 2;
                    return;
                }
            }
        }
        //------------------------------------------------------
        public static void UpdateBrushDummy(int instanceId, Vector3 position, Vector3 eulerAngle)
        {
            if (ms_Instance == null) return;
            BrushDummy brushDummy;
            for (int i = 0; i < ms_Instance.m_vBrushing.Count; ++i)
            {
                brushDummy = ms_Instance.m_vBrushing[i];
                if (brushDummy.instanceID == instanceId)
                {
                    brushDummy.startPos = position;
                    brushDummy.eulerAngle = eulerAngle;
                    ms_Instance.m_vBrushing[i] = brushDummy;
                    if (ms_Instance.m_bDirtyBrush <= 0)
                        ms_Instance.m_bDirtyBrush = 2;
                    return;
                }
            }
        }
        //------------------------------------------------------
        public static void OnAddBrush(Brush.TerrainBrush pBrush)
        {
            if (ms_Instance == null) return;
            //      lock(ms_Instance.m_BrushLock)
            {
                int instanceID = pBrush.GetInstanceID();
                BrushDummy dummy = new BrushDummy();
                dummy.instanceID = instanceID;
                dummy.Portals = pBrush.Portals;
                dummy.visibleCheck = pBrush.VisiableCheckDepth;
                dummy.bOnlyDepth = pBrush.VisiableOnlyDepth;
                dummy.eulerAngle = pBrush.transform.eulerAngles;
                dummy.startPos = pBrush.transform.position;
                dummy.portalStart = pBrush.portalStart;
                dummy.portalSizeH = pBrush.portalSizeH;
                dummy.portalSizeV = pBrush.portalSizeV;
                dummy.portalChunk = pBrush.portalChunk;
#if UNITY_EDITOR
                dummy.binder = pBrush;
#endif
                OnAddBrushDummy(dummy);
            }
        }
        //------------------------------------------------------
        public static void OnRemoveBrush(Brush.TerrainBrush pBrush)
        {
            OnRemoveBrushDummy(pBrush.GetInstanceID());
        }
        //------------------------------------------------------
        public static void OnEnableBrush(Brush.TerrainBrush pBrush)
        {
            if (ms_Instance == null) return;
            OnAddBrush(pBrush);
        }
        //------------------------------------------------------
        public static void OnDisableBrush(Brush.TerrainBrush pBrush)
        {
            if (ms_Instance == null) return;
            OnRemoveBrush(pBrush);
        }
        //------------------------------------------------------
        public static void SetTerrainBrushRootPos(Vector3 pos)
        {
            if (ms_Instance == null) return;
            //    lock (ms_Instance.m_BrushLock)
            {
                ms_Instance.m_TerrainBrushRootPos = pos;
                if (ms_Instance.m_bDirtyBrush <= 0)
                    ms_Instance.m_bDirtyBrush = 2;
            }
        }
        //------------------------------------------------------
        public static void DirtyBrush()
        {
            if (ms_Instance == null) return;
            //     lock (ms_Instance.m_BrushLock)
            {
                if (ms_Instance.m_bDirtyBrush <= 0)
                    ms_Instance.m_bDirtyBrush = 2;
            }
        }
        //------------------------------------------------------
        public static void UpdateDirtyBrush()
        {
            if (ms_Instance == null) return;
            if(ms_Instance.m_bDirtyBrush!=0)
            {
                ms_Instance.Update(0);
                ms_Instance.JobUpdate(0);
            }
        }
        //------------------------------------------------------
        public static void OffsetScene(Vector3 offset)
        {
            if (ms_Instance == null) return;
#if !USE_SERVER
            if(ms_Instance.m_vBrushing != null)
            {
                for(int i =0; i < ms_Instance.m_vBrushing.Count; ++i)
                {
                    BrushDummy dummy = ms_Instance.m_vBrushing[i];
                    dummy.startPos += offset;
                    ms_Instance.m_vBrushing[i] = dummy;
                }
            }
            if (ms_Instance.m_vRuntimePrefabs != null)
            {
                foreach (var db in ms_Instance.m_vRuntimePrefabs)
                {
                    for(int i =0; i < db.Value.Count; ++i)
                    {
                        db.Value[i].SetPosition(db.Value[i].GetPosition() + offset);
                    }
                }
            }
            ms_Instance.m_bDirtyCmd = 1;
            ms_Instance.m_bDirtyBrush = 2;
#endif
        }
    //------------------------------------------------------
    public static void RemoveForceWind(Transform instance)
        {
            if (ms_Instance == null) return;
            //    lock (ms_Instance.m_BrushLock)
            {
                for (int i = 0; i < ms_Instance.m_vForceWinds.Count; ++i)
                {
                    if (ms_Instance.m_vForceWinds[i].transform == instance)
                    {
                        ms_Instance.m_vForceWinds.RemoveAt(i);

                        if (ms_Instance.m_bDirtyBrush <= 0)
                            ms_Instance.m_bDirtyBrush = 2;
                        return;
                    }
                }
            }
        }
        //------------------------------------------------------
        public static void AddForceWind(Transform instance, float fForce, float distance, float fDamping, bool bClear = false)
        {
            if (ms_Instance == null || distance <= 0 || instance == null) return;
            //      lock (ms_Instance.m_BrushLock)
            {
                if (bClear)
                {
                    ms_Instance.m_vForceWinds.Clear();
                }
                else
                {
                    for (int i = 0; i < ms_Instance.m_vForceWinds.Count; ++i)
                    {
                        if (ms_Instance.m_vForceWinds[i].transform == instance)
                        {
                            FroceWind forceWind = ms_Instance.m_vForceWinds[i];
                            if (Mathf.Abs(forceWind.distance - distance) > 0.5f || Mathf.Abs(forceWind.force - fForce) > 0.5f)
                            {
                                forceWind.distance = distance * distance;
                                forceWind.force = fForce;
                                ms_Instance.m_vForceWinds[i] = forceWind;

                                if (ms_Instance.m_bDirtyBrush <= 0)
                                    ms_Instance.m_bDirtyBrush = 2;
                            }
                            return;
                        }
                    }
                }

                FroceWind wind = new FroceWind();
                wind.transform = instance;
                wind.force = fForce;
                wind.position = instance.position;
                wind.damping = fDamping;
                wind.distance = distance * distance;
                ms_Instance.m_vForceWinds.Add(wind);
                if (ms_Instance.m_bDirtyBrush <= 0)
                    ms_Instance.m_bDirtyBrush = 2;
            }
        }
#endif
            //------------------------------------------------------
        protected override void OnAwake()
        {
            ms_Instance = this;
            m_Terrains = new TerrainLayers();
        }
#if !USE_SERVER
        //------------------------------------------------------
        protected override void OnStartUp()
        {
            m_bInstancingMat = Brush.TerrainFoliageDatas.IsInstancing();
            m_nLayer = LayerMask.NameToLayer(GlobalDef.ms_foregroundLayerName);
            ms_ExternFactorID = Framework.Core.MaterailBlockUtil.BuildPropertyID("_ExternFactor");
            for (int i = 0; i < m_Frustum.Length; ++i)
                m_Frustum[i] = new Plane();
            m_pCurCamera = null;
            //      m_BatchRender = new UnityEngine.Rendering.BatchRendererGroup(this.OnPerformCulling);
        }
#endif
        //------------------------------------------------------
        protected override void OnDitryClear()
        {
            base.OnDitryClear();
            OnClear();
        }
        //------------------------------------------------------
        protected override void OnClear()
        {
#if !USE_SERVER
            m_TerrainBrushRootPos = Vector3.zero;
            m_bDirtyBrush = 0;
            m_LastCameraPos = Vector3.zero;
            m_LastCaemraEulerAngle = Vector3.zero;
            m_LastCaemraDirection = Vector3.forward;

            foreach (var db in m_vRuntimePrefabs)
            {
                for (int i = 0; i < db.Value.Count; ++i)
                    db.Value[i].RecyleDestroy();
            }
            m_vRuntimePrefabs.Clear();

            m_vForceWinds.Clear();
            //      lock (m_BrushLock)
            {
                m_vRemoveBrushing.Clear();
                m_bDirtyBrush = 0;
                m_vBrushing.Clear();
                m_vStandInstances.Clear();
                for (int i = 0; i < m_vRuntimeBrushs.Length; ++i)
                {
                    if (m_vRuntimeBrushs[i] != null)
                        m_vRuntimeBrushs[i].Clear();
                }
            }
            TerrainRenderFoliages foliages = URP.URPPostWorker.CastPostPass<TerrainRenderFoliages>(Framework.URP.EPostPassType.ForceCustomOpaque);
            if (foliages != null)
            {
                for (int i = 0; i < m_vRuntimeBrushs.Length; ++i)
                {
                    if (m_vRuntimeBrushs[i] != null)
                    {
                        foliages.UpdateFoliages(m_vRuntimeBrushs[(int)Brush.EBrushType.Grass], IsSupportInstancing(), Brush.TerrainFoliageDatas.GetMaterial((Brush.EBrushType)i));
                    }
                }
                m_bDirtyCmd = 0;
            }
#endif
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            ms_Instance = null;
            base.OnDestroy();
        }
#if !USE_SERVER
        //------------------------------------------------------
        bool IsSupportInstancing()
        {
#if UNITY_EDITOR
            m_bInstancingMat = Brush.TerrainFoliageDatas.IsInstancing();
#endif
            return Framework.Base.ConfigUtil.IsSupportInstancing && m_bInstancingMat;
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            Camera camera = CameraKit.MainCamera;
            if (camera == null) return;
            m_CullingMatrix = camera.cullingMatrix;
            Framework.Core.CommonUtility.CalculateFrustumPlanes(m_CullingMatrix, ref m_Frustum);
            m_bCullingCheck = CameraKit.MainCameraIsActived;
            if (Vector3.Dot(CameraKit.MainCameraDirection, Vector3.forward) < 0)
                m_fCullingFactor = 2.5f;
            else
                m_fCullingFactor = 1.2f;

            Vector3 pos = CameraKit.MainCameraPosition;
            Vector3 euler = CameraKit.MainCameraEulerAngle;
            if (!Framework.Core.BaseUtil.Equal(pos, m_LastCameraPos, 0.1f) || !Framework.Core.BaseUtil.Equal(euler, m_LastCaemraEulerAngle, 0.1f))
            {
                m_bDirtyBrush = 2;
                m_LastCameraPos = pos;
                m_LastCaemraEulerAngle = euler;
                m_LastCaemraDirection = CameraKit.MainCameraDirection;
            }

            if (m_vForceWinds.Count > 0)
            {
                for (int i = 0; i < m_vForceWinds.Count;)
                {
                    FroceWind wind = m_vForceWinds[i];
                    if (wind.transform == null)
                    {
                        m_vForceWinds.RemoveAt(i);
                        continue;
                    }
                    if (wind.damping <= 0)
                    {
                        ++i;
                        continue;
                    }
                    wind.position = wind.transform.position;
                    wind.force = Mathf.Lerp(wind.force, 0, fFrame * wind.damping);
                    if (wind.force <= 0.001f)
                    {
                        m_vForceWinds.RemoveAt(i);
                        continue;
                    }
                    m_vForceWinds[i] = wind;
                    ++i;
                }
                if (m_vForceWinds.Count > 0 && m_bDirtyBrush <= 0)
                {
                    m_bDirtyBrush = 2;
                }
            }

            if (m_bDirtyCmd != 0 && Data.GameQuality.bInstanceBrush)
            {
                TerrainRenderFoliages foliages = URP.URPPostWorker.CastPostPass<TerrainRenderFoliages>(Framework.URP.EPostPassType.ForceCustomOpaque);
                if (foliages != null)
                {
                    for (int i = 0; i < m_vRuntimeBrushs.Length; ++i)
                    {
                        if (m_vRuntimeBrushs[i] != null)
                        {
                            foliages.UpdateFoliages(m_vRuntimeBrushs[i], IsSupportInstancing(), Brush.TerrainFoliageDatas.GetMaterial((Brush.EBrushType)i));
                        }
                    }
                    m_bDirtyCmd = 0;
                }
            }
        }
        //------------------------------------------------------
        protected override void OnJobUpdate()
        {
            if (m_bDirtyBrush != 0 && Data.GameQuality.bInstanceBrush)
            {
                Brush.Portal portal;
                Brush.Foliage foliage;
                BrushDummy dummy;

                if (m_vRemoveBrushing.Count > 0)
                {
                    foreach (var db in m_vRemoveBrushing)
                    {
                        int id = db;
                        for (int f = 0; f < m_vBrushing.Count;)
                        {
                            dummy = m_vBrushing[f];
                            if (dummy.instanceID == id)
                            {
                                m_vBrushing.RemoveAt(f);
                                m_bDirtyBrush = 2;
                            }
                            else
                                ++f;
                        }
                    }

                    m_vRemoveBrushing.Clear();
                }

                if (m_bDirtyBrush == 2)
                {
                    BrushCmdStack stack;
                    for (int j = 0; j < m_vRuntimeBrushs.Length; ++j)
                    {
                        List<BrushCmdStack> cmdStrack = m_vRuntimeBrushs[j];
                        if (cmdStrack == null) continue;
                        for (int i = 0; i < cmdStrack.Count; ++i)
                        {
                            stack = cmdStrack[i];
                            stack.asyncStackCount = 0;
                            stack.distanceSqr = -1;
                            cmdStrack[i] = stack;
                        }
                    }

                    int curIndex = 0;
                    Vector3 startPos = Vector3.zero;
                    for (int i = 0; i < m_vBrushing.Count; ++i)
                    {
                        dummy = m_vBrushing[i];
                        if (dummy.Portals == null) continue;
                        Quaternion brushRotation = Quaternion.Euler(dummy.eulerAngle);
                        startPos = dummy.GetStartPosition() + m_TerrainBrushRootPos;
                        float depth = startPos.z;
                        float visibleDst = (dummy.visibleCheck + Data.GameQuality.TerrainBrushCheckVisible);
                        if (dummy.bOnlyDepth)
                        {
                            if (depth - m_LastCameraPos.z >= visibleDst)
                                continue;
                        }
                        else
                        {
                            if ((startPos - m_LastCameraPos).sqrMagnitude >= visibleDst * visibleDst)
                                continue;
                        }
                        if (!dummy.InView(m_TerrainBrushRootPos, m_Frustum)) continue;
                        for (int p = 0; p < dummy.Portals.Length; ++p)
                        {
                            portal = dummy.Portals[p];
                            if (dummy.InView(m_TerrainBrushRootPos, m_Frustum, p))
                            {
                                if (dummy.Portals[p].foliages == null) continue;
                                for (int j = 0; j < dummy.Portals[p].foliages.Count; ++j)
                                {
                                    foliage = dummy.Portals[p].foliages[j];
                                    Vector3 position = Framework.Core.CommonUtility.RoateAround(Vector3.zero, foliage.position, brushRotation);
                                    if (dummy.bOnlyDepth)
                                    {
                                        depth = (position + startPos).z;
                                        if (depth - m_LastCameraPos.z >= visibleDst)
                                            continue;
                                    }
                                    else
                                    {
                                        depth = (position + dummy.GetStartPosition() + m_TerrainBrushRootPos - m_LastCameraPos).sqrMagnitude;

                                        if (depth >= visibleDst * visibleDst)
                                            continue;
                                    }

                                    if (m_bCullingCheck && !Framework.Base.IntersetionUtil.PositionInView(m_CullingMatrix, position + startPos, m_fCullingFactor))
                                        continue;
                                    if (foliage.brush == null)
                                        foliage.brush = Brush.TerrainFoliageDatas.GetBrush(foliage.useIndex);
                                    if (foliage.brush == null) continue;
                                    if (foliage.brush is Brush.BrushRes)
                                    {
                                        //  m_vStandInstances.Add(foliage);
                                        continue;
                                    }

                                    List<BrushCmdStack> cmdStacks = GetBrushCmdStack(foliage.brush.type);
                                    if (cmdStacks == null) continue;

                                    stack = GetStack(foliage.brush, out curIndex);
                                    if (!stack.IsValid || curIndex < 0)
                                    {
                                        continue;
                                    }
                                    if (dummy.bOnlyDepth)
                                    {
                                        stack.distanceSqr = 0;
                                    }
                                    else
                                    {
                                        if (stack.distanceSqr >= 0)
                                            stack.distanceSqr = Mathf.Min(stack.distanceSqr, (startPos - m_LastCameraPos).sqrMagnitude);
                                        else stack.distanceSqr = (startPos - m_LastCameraPos).sqrMagnitude;
                                    }
                                    Brush.FoliageLodMesh.Lod lodMesh = stack.getMesh(stack.distanceSqr);
                                    if (lodMesh == null) continue;

                                    //! wind effect
                                    float factor = 0;
                                    Quaternion rot = CalcForceWind(foliage.eulerAngle + dummy.GetStartEulerAngle(), position, ref factor);
                                    if (lodMesh.billboard)
                                    {
                                        m_LastCaemraDirection.y = 0;
                                        rot *= Quaternion.LookRotation(m_LastCaemraDirection);
                                    }
                                    stack.vMatrixs[stack.asyncStackCount] = Matrix4x4.TRS(position + dummy.GetStartPosition() + m_TerrainBrushRootPos, rot, foliage.scale);
                                    stack.vExterns[stack.asyncStackCount] = new Vector4(foliage.color.x * foliage.lightmap, foliage.color.y * foliage.lightmap, foliage.color.z * foliage.lightmap, factor);
                                    Vector2 uvOffset = foliage.brush.GetUvOffset();
                                    stack.vParams[stack.asyncStackCount] = new Vector4(uvOffset.x, uvOffset.y, 0, 0);
                                    stack.asyncStackCount++;
                                    cmdStacks[curIndex] = stack;
                                }
                            }
                        }
                    }


                    for (int j = 0; j < m_vRuntimeBrushs.Length; ++j)
                    {
                        List<BrushCmdStack> cmdStrack = m_vRuntimeBrushs[j];
                        if (cmdStrack == null) continue;
                        for (int i = 0; i < cmdStrack.Count; ++i)
                        {
                            stack = cmdStrack[i];
                            stack.stackCount = stack.asyncStackCount;
                            cmdStrack[i] = stack;
                        }
                    }
                    m_bDirtyCmd = 1;
                }
                m_bDirtyBrush = 0;
            }
        }
        //------------------------------------------------------
        List<BrushCmdStack> GetBrushCmdStack(Brush.EBrushType brushType)
        {
            if (brushType == Brush.EBrushType.Count) return null;
            int index = (int)brushType;
            if (index >= m_vRuntimeBrushs.Length) return null;
            if (m_vRuntimeBrushs[index] == null) m_vRuntimeBrushs[index] = new List<BrushCmdStack>(2);
            return m_vRuntimeBrushs[index];
        }
        //------------------------------------------------------
        Quaternion CalcForceWind(Vector3 eulerAngle, Vector3 pos, ref float fFactor)
        {
            fFactor = 0;
            Vector3 selfDir = Framework.Core.BaseUtil.EulersAngleToDirection(eulerAngle);
            Quaternion rot = Quaternion.Euler(eulerAngle);
            if (m_vForceWinds.Count > 0)
            {
                float factor = 0;
                Vector3 dir = Vector3.zero;

                int windCnt = m_vForceWinds.Count;
                FroceWind wind;
                float invCnt = 1f / windCnt;
                for (int i = 0; i < windCnt; ++i)
                {
                    if (i >= m_vForceWinds.Count) break;
                    wind = m_vForceWinds[i];
                    Vector3 tempDir = pos - wind.position;
                    float dist = tempDir.sqrMagnitude;
                    factor += Mathf.Clamp01((wind.distance - dist) / wind.distance) * wind.force;
                    tempDir.y = 0;
                    dir += tempDir;
                    if (m_vForceWinds.Count != windCnt) break;
                }
                dir *= invCnt;
                factor *= invCnt;
                dir.Normalize();
                fFactor = factor;
                Vector3 finalUp = (Vector3.up + dir * factor).normalized;
                rot = Quaternion.LookRotation(rot * Vector3.forward, finalUp);
            }


            return rot;
        }
        //------------------------------------------------------
        BrushCmdStack GetStack(Brush.Brush brush, out int index)
        {
            List<BrushCmdStack> brushStack = GetBrushCmdStack(brush.type);
            index = -1;
            if (brushStack == null) return BrushCmdStack.DEF;
            for (int i = 0; i < brushStack.Count; ++i)
            {
                if (brushStack[i].asyncStackCount < 1023)
                {
                    index = i;
                    return brushStack[i];
                }
            }

            BrushCmdStack stack = new BrushCmdStack(brush);
            brushStack.Add(stack);
            index = brushStack.Count - 1;
            return stack;
        }
        //------------------------------------------------------
        public override void DrawGizmos()
        {
#if UNITY_EDITOR
            if (m_Terrains != null && m_Terrains is TerrainLayers)
                (m_Terrains as TerrainLayers).DebugDraw();
#endif
        }
#endif
    }
}