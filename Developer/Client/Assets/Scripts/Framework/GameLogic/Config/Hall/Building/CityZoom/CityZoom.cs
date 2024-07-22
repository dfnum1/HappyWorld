/********************************************************************
生成日期:	5:24:2022  13:32
类    名: 	CityZoom
作    者:	HappLI
描    述:	主城区域组件
*********************************************************************/
using UnityEngine;
using Framework.Core;
using TopGame.Core;
using System.Collections.Generic;
namespace TopGame.Logic
{
    public class CityZoom : MonoBehaviour
    {
        [System.Serializable]
        public class FixdeCamera
        {
            public Vector3 lookAtPos;
            public Vector3 cameraEulerAngle;
            public float distance;
            public float maxDistance;
            public float fov;
            public float absortLowerFactor;
            public float absortUpperFactor;

            public float markerLOD = -1;

            public bool IsAbsort(float fFactor)
            {
                return fFactor >= absortLowerFactor && fFactor <= absortUpperFactor;
            }
            [System.NonSerialized]
            private float m_fRuntimeDistance;
            public float runtimeDistance
            {
                get
                {
                    if (m_fRuntimeDistance <= 0) m_fRuntimeDistance = distance;
                    return m_fRuntimeDistance;
                }
                set
                {
                    if (distance < maxDistance)
                        m_fRuntimeDistance = Mathf.Clamp(value, distance, maxDistance);
                    else m_fRuntimeDistance = distance;
                }
            }
        }

        public int zoomID = 0;
        public List<Vector3> polygonRegion;
        public FixdeCamera fixedCamera = new FixdeCamera();
        Transform m_pTransform = null;
        private void Awake()
        {
            m_pTransform = transform;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            m_pTransform = null;
        }
        //------------------------------------------------------
        public bool IsInRegion(Vector3 point)
        {
            if (polygonRegion == null || polygonRegion.Count < 3 || m_pTransform == null) return false;
            int plyCnt = polygonRegion.Count;
            Vector3 center = m_pTransform.position;
            int crossNum = 0;
            for (int i = 0; i < plyCnt; ++i)
            {
                Vector3 v1 = polygonRegion[i] + center;
                Vector3 v2 = polygonRegion[(i + 1) % plyCnt] + center;
                if (((v1.z <= point.z) && (v2.z > point.z)) || ((v1.z > point.z) && (v2.z <= point.z)))
                {
                    if (point.x < v1.x + (point.z - v1.z) / (v2.z - v1.z) * (v2.x - v1.x))
                        crossNum += 1;
                }
            }
            if (crossNum % 2 == 0) return false;
            return true;
        }
    }
}