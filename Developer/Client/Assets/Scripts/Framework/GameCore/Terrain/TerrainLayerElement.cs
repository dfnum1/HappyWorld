/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	TerrainLayerElement
作    者:	HappLI
描    述:	地表多层元素
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TopGame.Core
{
    public class TerrainLayerElement : AInstanceAble
    {
        static Ray ms_RayHit = new Ray();
        [SerializeField]
        Collider[] m_Collider;
        [SerializeField]
        float layerHeight = 0;

        private float m_fSizeSqr = -1;
        private float m_fMinZ = -1;
        private float m_fMaxZ = -1;
#if UNITY_EDITOR
        //------------------------------------------------------
        public void SetCollider(Collider[] collider, float layerHeight)
        {
            m_Collider = collider;
            this.layerHeight = layerHeight;
            m_fSizeSqr = -1;
            m_fMinZ = -1;
            m_fMaxZ = -1;
        }
#endif
        //------------------------------------------------------
        public float GetLayerHeight()
        {
            return layerHeight;
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            if (Framework.Module.ModuleManager.mainModule == null) return;
            if (m_Collider == null || m_Collider.Length <= 0 || layerHeight <=0) return;
            TerrainLayers terrainLayers = TerrainManager.GetTerrainLayers(Framework.Module.ModuleManager.mainModule) as TerrainLayers;
            if (terrainLayers != null) terrainLayers.RemoveLayerElement(this);
            base.OnDestroy();
        }
        //------------------------------------------------------
        public override void OnRecyle()
        {
            base.OnRecyle();
            if (Framework.Module.ModuleManager.mainModule == null) return;
            if (m_Collider == null || m_Collider.Length <= 0 || layerHeight <= 0) return;
            TerrainLayers terrainLayers = TerrainManager.GetTerrainLayers(Framework.Module.ModuleManager.mainModule) as TerrainLayers;
            if(terrainLayers!=null) terrainLayers.RemoveLayerElement(this);
        }
        //------------------------------------------------------
        public override void OnPoolStart()
        {
            base.OnPoolStart();
            if (Framework.Module.ModuleManager.mainModule == null) return;
            if (m_Collider == null || m_Collider.Length <= 0) return;
            TerrainLayers terrainLayers = TerrainManager.GetTerrainLayers(Framework.Module.ModuleManager.mainModule) as TerrainLayers;
            if (terrainLayers != null) terrainLayers.AddLayerElement(this);
        }
        //------------------------------------------------------
        public int Raycast(Vector3 curPos, ref RaycastHit rayHit, float maxDistance = 1f, AInstanceAble pIngore = null)
        {
            if (layerHeight <= 0 || m_Collider == null || m_Collider.Length <= 0 || m_pTransform == null) return 0;
            //     if (curPos.y < layerHeight/2) return false;
            float halfHeight = layerHeight / 2 +m_pTransform.position.y;
            float sqrDiff = Vector3.SqrMagnitude(curPos - m_pTransform.position);
            if (m_fSizeSqr<0)
            {
                m_fMinZ = float.MaxValue;
                m_fMaxZ = float.MinValue;
                m_fSizeSqr = 0;
                for (int i = 0; i < m_Collider.Length; ++i)
                {
                    Bounds bound = m_Collider[i].bounds;
                    m_fMinZ = Mathf.Min(m_fMinZ, bound.min.z - bound.center.z);
                    m_fMaxZ = Mathf.Max(m_fMaxZ, bound.max.z - bound.center.z);
                    m_fSizeSqr = Mathf.Max(m_fSizeSqr, bound.size.x* bound.size.x * 4);
                    m_fSizeSqr = Mathf.Max(m_fSizeSqr, bound.size.z * bound.size.z * 4);
                    m_fSizeSqr = Mathf.Max(m_fSizeSqr, bound.size.y * bound.size.y * 4);
                }
            }
            if (sqrDiff > m_fSizeSqr || curPos.z < (m_fMinZ + m_pTransform.position.z) || curPos.z> (m_fMaxZ +m_pTransform.position.z)) return 0;
            if(pIngore!=null)
            {
                if (pIngore.GetTransorm() == m_pTransform) return 0;
            }

            ms_RayHit.origin = curPos + Vector3.up*(Framework.Core.CommonUtility.JUMP_HEIGHT_LOWER);
            ms_RayHit.direction = Vector3.down;
            for (int i = 0; i < m_Collider.Length; ++i)
            {
                if (m_Collider[i] == null || !m_Collider[i].enabled) continue;

                if(m_Collider[i].Raycast(ms_RayHit, out rayHit, maxDistance))
                {
                    if(Mathf.Abs(Vector3.Dot(rayHit.normal, Vector3.up)) >= 0.99f)
                    {
                        if (curPos.y < halfHeight) return -1;
                        else return 3;
                    }
                    return 2;
                }
            }

            return 0;
        }
    }

    //------------------------------------------------------
#if UNITY_EDITOR
    [CustomEditor(typeof(TerrainLayerElement))]
    [CanEditMultipleObjects]
    public class TerrainLayerElementEditor : UnityEditor.Editor
    {
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            TerrainLayerElement test = target as TerrainLayerElement;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Collider"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("layerHeight"), true);
            if(test.GetLayerHeight()<= 0)
            {
                EditorGUILayout.HelpBox("没有高度值", MessageType.Warning);
            }
            if (GUILayout.Button("刷新"))
            {
                float layerHeight = 0;
                Collider[] cols = GetCollider(ref layerHeight);
                test.SetCollider(cols, layerHeight);
            }
        }
        //------------------------------------------------------
        Collider[] GetCollider(ref float layerHeight)
        {
            TerrainLayerElement test = target as TerrainLayerElement;
            Collider curCol = test.GetComponent<Collider>();
            Collider[] colliders = test.gameObject.GetComponentsInChildren<Collider>();
            List<Collider> vCol = new List<Collider>();
            if (curCol != null) vCol.Add(curCol);
            for(int i = 0; i < colliders.Length; ++i)
            {
                if(!vCol.Contains(colliders[i]))
                    vCol.Add(colliders[i]);
            }
            for(int i = 0; i < vCol.Count; ++i)
            {
                Bounds bounds = vCol[i].bounds;
                layerHeight = Mathf.Max(layerHeight, bounds.max.y-bounds.min.y);
            }
            return vCol.ToArray();
        }
    }
#endif
}