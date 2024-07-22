using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace TopGame.Core.Brush
{
    public class TerrainBrushRoot : MonoBehaviour
    {
        Transform m_pTransform;
        private void Awake()
        {
            m_pTransform = transform;
        }
        void Update()
        {
            TerrainManager.SetTerrainBrushRootPos(m_pTransform.position);
        }
    }
}



