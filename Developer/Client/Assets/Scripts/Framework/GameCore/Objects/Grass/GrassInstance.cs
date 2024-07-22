/********************************************************************
生成日期:	1:11:2020 13:07
类    名: 	GrassInstance
作    者:	HappLI
描    述:	实例化渲染草
*********************************************************************/
using Framework.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core
{
    public class GrassInstance : IGrass
    {
        public class DrawInstace
        {
            public List<Matrix4x4> running_matrices = new List<Matrix4x4>();
            public List<Vector4> running_probs = new List<Vector4>();
            public List<float> running_heights = new List<float>();

            public void Clear()
            {
                running_matrices.Clear();
                running_probs.Clear();
                running_heights.Clear();
            }
        }
        public class Instance
        {
            public byte texCol = 0;
            public byte texRow = 0;
            public float height = 0;
            public List<Matrix4x4> matrices = new List<Matrix4x4>();
            public List<Vector4> probs = new List<Vector4>();
            public List<float> heights = new List<float>();

            public List<Matrix4x4> running_matrices = new List<Matrix4x4>();
            public List<Vector4> running_probs = new List<Vector4>();
            public List<float> running_heights = new List<float>();

            public Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            public int frame  =0;
            public void Clear()
            {
                matrices.Clear();
            }
        }
        Mesh m_pBaseMesh = null;
        List<Instance> m_vInstances = new List<Instance>();
        List<DrawInstace> m_vDraws = new List<DrawInstace>();
        Dictionary<int, int> m_vMappingCatch = new Dictionary<int, int>();

        List<Matrix4x4> m_vTransofrm = new List<Matrix4x4>();
        Matrix4x4 m_ParentTasnform = Matrix4x4.identity;
        Material m_pMaterial;
        MaterialPropertyBlock m_pPropertyBlock = new MaterialPropertyBlock();

        int m_Instance_Grass_UV = 0;
        int m_Instance_Grass_Height = 0;
        //------------------------------------------------------
        public GrassInstance()
        {
            m_Instance_Grass_UV = Framework.Core.MaterailBlockUtil.BuildPropertyID("Instance_Grass_UV");
            m_Instance_Grass_Height = Framework.Core.MaterailBlockUtil.BuildPropertyID("Instance_Grass_Height");
        }
        //------------------------------------------------------
        public void SetDatas(GrassBladeData[] Datas)
        {

        }
        //------------------------------------------------------
        public void SetDatas( List<GrassBladeData> vDatas )
        {
            if(m_pBaseMesh == null)
            {
                m_pBaseMesh = new Mesh();
                m_pBaseMesh.vertices = new Vector3[] {
                    new Vector3(0.5f,0f,0f),
                    new Vector3(-0.5f,0,0f),
                    new Vector3(-0.5f,1f,0f),
                    new Vector3(0.5f,1f,0f)
                    };
                m_pBaseMesh.SetUVs(0, new Vector2[] {
                    new Vector2(0,0),
                    new Vector2(1,0),
                    new Vector2(1,1),
                    new Vector2(0,1)
                    });
                m_pBaseMesh.triangles = new int[] { 0, 1, 2, 2, 3, 0 };

            }
            m_pPropertyBlock.Clear();
            m_vTransofrm.Clear();
            m_vInstances.Clear();
            m_vMappingCatch.Clear();
            m_vDraws.Clear();
            for (int i = 0; i < vDatas.Count; ++i)
            {
                int key = vDatas[i].texCol << 16 | vDatas[1].texRow;
                Instance pIns = null;
                int arrIndex = -1;
                bool bNew = true;
                if (m_vMappingCatch.TryGetValue(key, out arrIndex))
                {
                    pIns = m_vInstances[arrIndex];
                    if (pIns.matrices.Count < 1023)
                    {
                        bNew = false;
                    }
                }
                if(bNew)
                {
                    pIns = new Instance();
                    pIns.texCol = vDatas[i].texCol;
                    pIns.texRow = vDatas[i].texRow;
                    pIns.height = vDatas[i].scale.y;
                    pIns.bounds.SetMinMax(Vector3.one * float.MaxValue, Vector3.one * float.MinValue);
                    pIns.frame = 0;
                    m_vInstances.Add(pIns);
                    m_vMappingCatch[key] = m_vInstances.Count - 1;

                    m_vDraws.Add(new DrawInstace());
                }

                Vector3 pos = m_ParentTasnform.MultiplyPoint(vDatas[i].position);
                Quaternion rot = Quaternion.LookRotation(vDatas[i].tangent,vDatas[i].normal)*Quaternion.Euler(0,-90,0);

                pIns.matrices.Add(Matrix4x4.TRS(pos, rot, vDatas[i].scale));
                pIns.probs.Add(new Vector4(0, 0, pIns.texCol, pIns.texRow));
                pIns.heights.Add(vDatas[i].scale.y*0.1f);
                Vector3 min = Vector3.Min(pIns.bounds.min, vDatas[i].position);
                Vector3 max = Vector3.Max(pIns.bounds.max, vDatas[i].position);
                pIns.bounds.SetMinMax(min, max);
            }
        }
        //------------------------------------------------------
        public void Clear()
        {
            m_pPropertyBlock.Clear();
        }
        //------------------------------------------------------
        public void Destroy()
        {
            foreach (var db in m_vInstances)
            {
                db.Clear();
            }
            if (m_pBaseMesh != null) m_pBaseMesh.Clear(false);
            m_pPropertyBlock.Clear();
            m_vDraws.Clear();
            m_vDraws = null;
            m_vInstances = null;
            m_pMaterial = null;
        }
        //------------------------------------------------------
        public void GetMeshs(List<Mesh> vMeshs)
        {
            vMeshs.Add(m_pBaseMesh);
        }
        //------------------------------------------------------
        public void Render(Camera camera)
        {
            if (m_vDraws.Count<=0 || m_pBaseMesh == null || m_pMaterial == null) return;
            foreach (var db in m_vDraws)
            {
                if (db.running_matrices.Count <= 0) continue;
                m_pPropertyBlock.SetVectorArray(m_Instance_Grass_UV, db.running_probs);
                m_pPropertyBlock.SetFloatArray(m_Instance_Grass_Height, db.running_heights);
                Graphics.DrawMeshInstanced(m_pBaseMesh, 0, m_pMaterial, db.running_matrices, m_pPropertyBlock, UnityEngine.Rendering.ShadowCastingMode.Off, false);
            }
        }
        //------------------------------------------------------
        void CullView(Camera pCamera, Instance pGrass, DrawInstace pDraw, int delayFrame = 10)
        {
            if (pGrass.frame > Time.frameCount) return;

            pDraw.Clear();
            pGrass.frame = Time.frameCount+delayFrame;
            if(!Framework.Base.IntersetionUtil.BoundInView(pCamera.cullingMatrix, pGrass.bounds))
            {
                return;
            }

            Matrix4x4 culling = pCamera.cullingMatrix;
            for (int i = 0; i < pGrass.matrices.Count; ++i)
            {
                if(Framework.Base.IntersetionUtil.PositionInView(culling, pGrass.matrices[i].GetColumn(3)))
                {
                    pDraw.running_matrices.Add(pGrass.matrices[i]);
                    pDraw.running_probs.Add(pGrass.probs[i]);
                    pDraw.running_heights.Add(pGrass.heights[i]);
                }
            }
        }
        //------------------------------------------------------
        public void SetMaterial(Material material)
        {
            m_pMaterial = material;
        }
        //------------------------------------------------------
        public void SetTransform(Matrix4x4 t)
        {
            m_ParentTasnform = t;
        }
        //------------------------------------------------------
        public void Update(Camera camera)
        {
            if (m_vInstances.Count <= 0 || m_pBaseMesh == null || m_pMaterial == null) return;
            m_vMappingCatch.Clear();
            for (int i = 0; i < m_vInstances.Count; ++i)
            {
                CullView(camera, m_vInstances[i], m_vDraws[i]);
            }
        }
    }
}

