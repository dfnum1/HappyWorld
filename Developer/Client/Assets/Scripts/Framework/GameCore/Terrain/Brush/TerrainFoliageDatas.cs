using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Framework.Core;

namespace TopGame.Core.Brush
{
    public enum EBrushType : byte
    {
        Grass = 0,
        Tree = 1,
        ResIns = 2,
        Count,
    }

    [System.Serializable]
    public abstract class Brush
    {
        public EBrushType type = EBrushType.Grass;
        public string name;
        public int guid;
        public abstract void Destroy();

        public abstract FoliageLodMesh.Lod GetShareMesh(float distance);
        public abstract Vector2 GetUvOffset();
        public abstract bool IsBillboard();
#if UNITY_EDITOR
        public abstract float GetSize();
#endif
    }
    [System.Serializable]
    public class BrushBuffer : Brush
    {
        public int shareMesh = 0;
        public bool bBillboard;
        public Vector2 usOffset = Vector2.zero;
        public override Vector2 GetUvOffset()
        {
            return usOffset;
        }
        public override FoliageLodMesh.Lod GetShareMesh(float distance)
        {
            return TerrainFoliageDatas.GetMesh(shareMesh, distance);
        }
        public override void Destroy()
        {
        }

        public override bool IsBillboard()
        {
            return bBillboard;
        }

#if UNITY_EDITOR
        [System.NonSerialized]
        float m_fSize = -1;
        public override float GetSize()
        {
            if(m_fSize <0)
            {
                FoliageLodMesh.Lod mesh = GetShareMesh(0);
                if (mesh == null || mesh.mesh == null) return 1;
                m_fSize = mesh.mesh.bounds.size.magnitude;
            }
            return m_fSize;
        }
#endif
    }
    [System.Serializable]
    public class BrushRes : Brush
    {
        public string strFile;

        //------------------------------------------------------
        public override Vector2 GetUvOffset()
        {
            return Vector2.zero;
        }
        //------------------------------------------------------
        public override FoliageLodMesh.Lod GetShareMesh(float distance)
        {
            return null;
        }
        //------------------------------------------------------
        public override bool IsBillboard()
        {
            return false;
        }
        //------------------------------------------------------
        public override void Destroy()
        {

        }

#if UNITY_EDITOR
        [System.NonSerialized]
        float m_fSize = -1;
        public override float GetSize()
        {
            if (m_fSize < 0)
            {
                GameObject pInst = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(strFile);
                if (pInst != null)
                {
                    Renderer render = pInst.GetComponent<Renderer>();
                    if (render)
                    {
                        m_fSize = new Vector2(render.bounds.size.x, render.bounds.size.z).magnitude / 2;
                    }
                    else
                    {
                        Vector2 min = Vector2.zero;
                        Vector2 max = Vector2.zero;
                        Renderer[] renders = pInst.GetComponentsInChildren<Renderer>();
                        for (int i = 0; i < renders.Length; ++i)
                        {
                            min.x = Mathf.Min(min.x, renders[i].bounds.min.x);
                            min.y = Mathf.Min(min.y, renders[i].bounds.min.z);
                            max.x = Mathf.Max(max.x, renders[i].bounds.max.x);
                            max.y = Mathf.Max(max.y, renders[i].bounds.max.z);
                        }
                        m_fSize = (max - min).magnitude / 2;
                    }
                }
                else
                    m_fSize = 0.1f;
            }

            return m_fSize;
        }
#endif
    }
    [CreateAssetMenu]
    public class TerrainFoliageDatas : ScriptableObject
    {
        public List<FoliageLodMesh> shareMeshs;
        public Material foliageMatrial;
        public Material treeMaterial;
        public List<BrushRes> brushRes;
        public List<BrushBuffer> brushBuffers;

        static TerrainFoliageDatas ms_Instance = null;
        [System.NonSerialized]
        Dictionary<int, Brush> m_vBrushs = null;
        private void OnEnable()
        {
            ms_Instance = this;

            bool bRebuildShader = false;
#if !UNITY_EDITOR
            bRebuildShader = true;
#else
            if (FileSystemUtil.GetStreamType() != EFileSystemType.AssetData)
                bRebuildShader = true;
#endif
            if (Data.GlobalDefaultResources.DefaultMaterials != null && Data.GlobalDefaultResources.DefaultMaterials.Length>0)
            {
                foliageMatrial = Data.GlobalDefaultResources.DefaultMaterials[0];
                treeMaterial = Data.GlobalDefaultResources.DefaultMaterials[1];
                bRebuildShader = false;
            }

            if (bRebuildShader)
            {
                Shader foliage = Shader.Find("SD/Environment/SD_Foliages");
                if (foliage)
                    foliageMatrial.shader = foliage;

                Shader tree = Shader.Find("SD/Environment/SD/Environment/SD_IslandTree");
                if (tree)
                    treeMaterial.shader = tree;

            }
            Refresh();
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_Instance = null;
            m_vBrushs = null;
        }
        //------------------------------------------------------
        public void Refresh()
        {
            m_vBrushs = new Dictionary<int, Brush>();
            if (brushRes != null)
            {
                foreach (var db in brushRes)
                {
                    m_vBrushs[db.guid] = db;
                }
            }
            if (brushBuffers != null)
            {
                foreach (var db in brushBuffers)
                {
                    m_vBrushs[db.guid] = db;
                }
            }
        }
        //------------------------------------------------------
        public Dictionary<int, Brush> GetBrushs()
        {
            return m_vBrushs;
        }
        //------------------------------------------------------
        public Material FindMaterial(EBrushType brushType)
        {
            if (ms_Instance == null) return null;
            switch (brushType)
            {
                case EBrushType.Grass: return ms_Instance.foliageMatrial;
                case EBrushType.Tree: return ms_Instance.treeMaterial;
            }
            return null;
        }
        //------------------------------------------------------
        public static Brush GetBrush(int brush)
        {
            if (ms_Instance == null) return null;
            if (ms_Instance.m_vBrushs == null) return null;
            Brush retBrush = null;
            if (ms_Instance.m_vBrushs.TryGetValue(brush, out retBrush))
                return retBrush;
            return null;
        }
        //------------------------------------------------------
        public static FoliageLodMesh.Lod GetMesh(int index, float distanceSqr=0)
        {
            if (ms_Instance == null) return null;
            if (ms_Instance.shareMeshs == null || index<0 || index >= ms_Instance.shareMeshs.Count) return null;
            FoliageLodMesh lodMesh = ms_Instance.shareMeshs[index];
            if (lodMesh.Lods.Count <= 0) return null;
            for(int i = 0; i < lodMesh.Lods.Count; ++i)
            {
                FoliageLodMesh.Lod lod = lodMesh.Lods[i];
                if (distanceSqr <= lod.distance * lod.distance)
                {
                    return lod;
                }
            }
            return lodMesh.Lods[lodMesh.Lods.Count-1];
        }
        //------------------------------------------------------
        public static Material FoliageMatrial
        {
            get
            {
                if (ms_Instance == null) return null;
                return ms_Instance.foliageMatrial;
            }
        }
        //------------------------------------------------------
        public static Material GetMaterial(EBrushType brushType)
        {
            if (ms_Instance == null) return null;
            return ms_Instance.FindMaterial(brushType);
        }
        //------------------------------------------------------
        public static bool IsInstancing()
        {
            if (ms_Instance == null) return false;
            if (ms_Instance.foliageMatrial && !ms_Instance.foliageMatrial.enableInstancing) return false;
            if (ms_Instance.treeMaterial && !ms_Instance.treeMaterial.enableInstancing) return false;
            return true;
        }
        //------------------------------------------------------
        public static Dictionary<int, Brush> Brushs
        {
            get
            {
                if (ms_Instance == null) return null;
                return ms_Instance.m_vBrushs;
            }
        }
    }
}


