using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core.Brush
{
    [System.Serializable]
    public struct FoliageBuffer
    {
        public Vector3[] vertices;
        public Vector2[] uvs;
   //     public Vector2[] lightmap;
        public int[] triangles;
    }

    [System.Serializable]
    public class FoliageLodMesh
    {
        [System.Serializable]
        public class Lod
        {
            public float distance;
            public bool billboard;
            public Mesh mesh;
        }
        public List<Lod> Lods;
#if UNITY_EDITOR
        [System.NonSerialized]
        public bool bExpand;
#endif
    }
}



