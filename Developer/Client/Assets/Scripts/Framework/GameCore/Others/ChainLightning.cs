/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ChainLightning
作    者:	HappLI
描    述:	闪电链
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TopGame.Core
{
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    [ExecuteAlways]
    public class ChainLightning : Framework.Core.AInstanceAble, Framework.Core.IChainLightning
    {
        [Tooltip("增加后，线条数量会减少，每个线条会更长。")]
        public float lineSize = 0.1f;

        [Tooltip("增加后，线条数量会减少，每个线条会更长。")]
        public float detail = 1;

        [Tooltip("位移量，也就是线条数值方向偏移的最大值")]
        public float displacement = 15;

        public ChainLightning[] childChains;

        [System.NonSerialized]
        Transform m_target;
        [System.NonSerialized]
        Transform m_start;

        [System.NonSerialized]
        Vector3 m_targetPosition = Vector3.zero;
        [System.NonSerialized]
        Vector3 m_startPositon = Vector3.zero;

        [SerializeField]
        private LineRenderer m_lineRender;
        private List<Vector3> m_linePosList;
#if UNITY_EDITOR
      //  [System.NonSerialized]
        public Transform target;
     //   [System.NonSerialized]
        public Transform start;
        //------------------------------------------------------
        protected override void OnEnable()
        {
            base.OnEnable();
            m_lineRender = GetComponent<LineRenderer>();
        }
#endif
        //------------------------------------------------------
        protected override void Awake()
        {
            base.Awake();
            if(m_lineRender == null) m_lineRender = GetComponent<LineRenderer>();
            m_linePosList = new List<Vector3>();
        }
        //------------------------------------------------------
        public void SetStart(Vector3 position, Transform transform = null)
        {
            m_targetPosition = position;
            m_target = transform;
            if (childChains == null) return;
            for (int i = 0; i < childChains.Length; ++i)
            {
                if (childChains[i])
                    childChains[i].SetStart(position, transform);
            }
        }
        //------------------------------------------------------
        public void SetEnd(Vector3 position, Transform transform = null)
        {
            m_startPositon = position;
            m_start = transform;
            if (childChains == null) return;
            for(int i = 0; i < childChains.Length; ++i)
            {
                if (childChains[i])
                    childChains[i].SetEnd(position, transform);
            }
        }
        //------------------------------------------------------
        private void Update()
        {
#if UNITY_EDITOR
            if (target) m_target = target;
            if (start) m_start = start;
#endif
            if(m_lineRender != null && Time.timeScale != 0)
            {
                if (displacement > detail && detail>0)
                {
                    m_linePosList.Clear();
                    Vector3 startPos = m_startPositon;
                    Vector3 endPos = m_targetPosition;
                    if (m_target != null)
                    {
                        endPos = m_target.position;
                    }
                    if (m_start != null)
                    {
                        startPos = m_start.position;
                    }

                    if ((endPos - startPos).sqrMagnitude <= 1) return;

                    CollectLinPos(startPos, endPos, displacement);
                    m_linePosList.Add(endPos);

                    m_lineRender.widthMultiplier = lineSize * transform.localScale.x;
                    m_lineRender.positionCount = m_linePosList.Count;
                    for (int i = 0, n = m_linePosList.Count; i < n; i++)
                    {
                        m_lineRender.SetPosition(i, m_linePosList[i]);
                    }
                }
                else
                {
                    m_linePosList.Clear();
                    Vector3 startPos = m_startPositon;
                    Vector3 endPos = m_targetPosition;
                    if (m_target != null)
                    {
                        endPos = m_target.position;
                    }
                    if (m_start != null)
                    {
                        startPos = m_start.position;
                    }

                    if ((endPos - startPos).sqrMagnitude <= 1) return;

                    m_linePosList.Add(startPos);
                    m_linePosList.Add(endPos);
                    m_lineRender.widthMultiplier = lineSize * transform.localScale.x;
                    m_lineRender.positionCount = m_linePosList.Count;
                    for (int i = 0, n = m_linePosList.Count; i < n; i++)
                    {
                        m_lineRender.SetPosition(i, m_linePosList[i]);
                    }
                }
            }
        }
        //------------------------------------------------------
        private void CollectLinPos(Vector3 startPos, Vector3 destPos, float displace)
        {
            if (displace < detail)
            {
                m_linePosList.Add(startPos);
            }
            else
            {
                float midX = (startPos.x + destPos.x) / 2;
                float midY = (startPos.y + destPos.y) / 2;
                float midZ = (startPos.z + destPos.z) / 2;

                midX += (float)(UnityEngine.Random.value - 0.5) * displace;
                midY += (float)(UnityEngine.Random.value - 0.5) * displace;
                midZ += (float)(UnityEngine.Random.value - 0.5) * displace;

                Vector3 midPos = new Vector3(midX, midY, midZ);

                CollectLinPos(startPos, midPos, displace / 2);
                CollectLinPos(midPos, destPos, displace / 2);
            }
        }
    }
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ChainLightning), true)]
    class ChainLightningEditor : Editor
    {
        private void OnEnable()
        {
            Check();
        }
        private void OnDisable()
        {
            Check();
        }

        void Check()
        {
            List<ChainLightning> chiains = new List<ChainLightning>();
            ChainLightning pointer = target as ChainLightning;
            ChainLightning[] chains = pointer.GetComponentsInChildren<ChainLightning>();
            if (chains != null)
            {
                for (int i = 0; i < chains.Length; ++i)
                {
                    if (chains[i] == pointer) continue;
                    chiains.Add(chains[i]);
                }
            }
            pointer.childChains = chiains.ToArray();
            EditorUtility.SetDirty(target);
        }
    }
#endif
}