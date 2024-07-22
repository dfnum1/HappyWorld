using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Framework.Core;

namespace TopGame.Core.Brush
{

    [System.Serializable]
    public struct Foliage : VariablePoolAble
    {
        public int useIndex;
        public Vector3 position;
        public Vector3 eulerAngle;
        public Vector3 scale;
        public float lightmap;
        public Vector3 color;

        [System.NonSerialized]
        public Brush brush;

        public void Destroy()
        {
            if (brush != null) brush.Destroy();
        }
    }
    [System.Serializable]
    public class Portal
    {
        public List<Foliage> foliages;
    }
    public class TerrainBrush : MonoBehaviour
    {
        static HashSet<TerrainBrush> ms_AllBrushs = null;
        struct BrushInstanceRuntime
        {
            public AInstanceAble pAble;
            public void Destroy()
            {
                if (pAble != null)
                {
                    pAble.RecyleDestroy();
                    pAble = null;
                }
            }
        }
        public List<GameObject> Terrains = null;
        [Range(-500,500)]
        public float VisiableCheckDepth = 100;
        public bool VisiableOnlyDepth = true;
        public int InstaceMaxCount = 10;

        [Framework.Data.DisplayNameGUI("空间划分起始位置")]
        public Vector3 portalStart = new Vector3(-100, -10, 0);
        [Framework.Data.DisplayNameGUI("空间划分块数"),Range(4, 10)]
        public int      portalChunk = 4;
        [Framework.Data.DisplayNameGUI("空间划分横向单位大小"),Range(10, 100)]
        public int portalSizeH = 50;
        [Framework.Data.DisplayNameGUI("空间划分纵向单位大小"),Range(10, 100)]
        public int portalSizeV = 30;
   //     [HideInInspector]
        [SerializeField]
        public Portal[] Portals;

        private Vector3 m_LastPosition;
        private Vector3 m_LastEulerAngle;
        float m_fDelayEnabledTime = 0;

        private Transform m_pTransform;
        private void Start()
        {
            m_fDelayEnabledTime = 0.5f;
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            //如果是回收资源，需要等待所有逻辑数据执行完后再处理笔刷数据，否则位置信息是不对的
            m_fDelayEnabledTime = 0.5f;
            m_pTransform = this.transform;
            m_LastPosition = m_pTransform.position;
            m_LastPosition = m_pTransform.eulerAngles;
            if (ms_AllBrushs == null) ms_AllBrushs = new HashSet<TerrainBrush>();
            ms_AllBrushs.Add(this);
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            TerrainManager.OnDisableBrush(this);
            m_fDelayEnabledTime = 0;
            m_pTransform = null;

            if (ms_AllBrushs != null) ms_AllBrushs.Remove(this);
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            TerrainManager.OnRemoveBrush(this);
            m_fDelayEnabledTime = 0;
            m_pTransform = null;

            if (ms_AllBrushs != null) ms_AllBrushs.Remove(this);
        }
        //------------------------------------------------------
        private void Update()
        {
            if(m_fDelayEnabledTime>0)
            {
                m_fDelayEnabledTime -= Time.deltaTime;
                if (m_fDelayEnabledTime <= 0)
                {
                    TerrainManager.OnEnableBrush(this);
                    m_fDelayEnabledTime = 0;
                }
            }
            if (m_pTransform == null)
            {
                m_pTransform = this.transform;
                m_LastPosition = m_pTransform.position;
                m_LastEulerAngle = m_pTransform.eulerAngles;
            }

            if (!Framework.Core.CommonUtility.Equal(m_LastPosition, m_pTransform.position, 0.1f) || !Framework.Core.CommonUtility.Equal(m_LastEulerAngle, m_pTransform.eulerAngles, 0.1f))
            {
                TerrainManager.UpdateBrushDummy(this);
            }
            m_LastPosition = m_pTransform.position;
            m_LastEulerAngle = m_pTransform.eulerAngles;
        }
        //------------------------------------------------------
        public static void UpdateAllBrush(bool bForceUpdate = false)
        {
            if (ms_AllBrushs == null) return;
            foreach(var db in ms_AllBrushs)
                TerrainManager.UpdateBrushDummy(db);
            if (bForceUpdate)
                TerrainManager.UpdateDirtyBrush();
        }
    }
}



