/********************************************************************
生成日期:	1:11:2020 13:07
类    名: 	GrassObject
作    者:	HappLI
描    述:	草渲染对象
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class GrassObject : MonoBehaviour
    {
        public bool bInstance = true;
          public IGrass m_pGrass = null;

        public GrassBladeData[] Data;

        MeshFilter m_MeshFiler;
        MeshRenderer m_pMeshRender;
        //------------------------------------------------------
        private void Awake()
        {
            m_pGrass.SetDatas(Data);
        }
        //------------------------------------------------------
        public void SetShareMesh(Mesh mesh)
        {
            if (m_MeshFiler == null) m_MeshFiler = GetComponent<MeshFilter>();
            if (m_MeshFiler == null) m_MeshFiler = gameObject.AddComponent<MeshFilter>();
            m_MeshFiler.sharedMesh = mesh;
        }
        //------------------------------------------------------
        public void SetShareMaterial(Material mat)
        {
            if (m_pMeshRender == null) m_pMeshRender = GetComponent<MeshRenderer>();
            if (m_pMeshRender == null) m_pMeshRender = gameObject.AddComponent<MeshRenderer>();
            m_pMeshRender.sharedMaterial = mat;
        }
        //------------------------------------------------------
        void Update()
        {
            m_pGrass.Update(CameraKit.MainCamera);
           // m_pGrass.Render();
        }
    }
}

