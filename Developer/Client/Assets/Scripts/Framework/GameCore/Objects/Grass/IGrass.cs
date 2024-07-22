/********************************************************************
生成日期:	1:11:2020 13:07
类    名: 	Grass
作    者:	HappLI
描    述:	草
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core
{
    [System.Serializable]
    public struct GrassBladeData
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector3 tangent;
        public Vector3 scale;
        public byte texRow;
        public byte texCol;
    }
    public interface IGrass
    {
        void Clear();
        void Update(Camera camera);
        void Destroy();
        void SetDatas(GrassBladeData[] datas);
        void SetMaterial(Material material);
#if UNITY_EDITOR
        void GetMeshs(List<Mesh> vMeshs);
#endif
        void Render(Camera camera);
        void SetTransform(Matrix4x4 t);
    }
}

