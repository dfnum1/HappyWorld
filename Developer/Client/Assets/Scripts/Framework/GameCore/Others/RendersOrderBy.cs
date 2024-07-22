/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	RendersOrderBy
作    者:	HappLI
描    述:	RenderOrderBy
*********************************************************************/
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace TopGame.Core
{
    public class RendersOrderBy : MonoBehaviour
    {
        [System.Serializable]
        public struct RenderOrder
        {
            public int offset;
            public Renderer render;
            public RenderOrder(Renderer render, int offset)
            {
                this.render = render;
                this.offset = offset;
            }
        }
        public Renderer OrderBy;
        public Canvas OrderByCanvas;
        public RenderOrder[] Renders;
        //------------------------------------------------------
        void Awake()
        {
            UpdateOrder();
        }
        //------------------------------------------------------
        void UpdateOrder()
        {
            if (Renders != null)
            {
                if (OrderBy)
                {
                    for (int i = 0; i < Renders.Length; ++i)
                    {
                        if (Renders[i].render)
                            Renders[i].render.sortingOrder = OrderBy.sortingOrder + Renders[i].offset;
                    }
                }
                else if (OrderByCanvas)
                {
                    for (int i = 0; i < Renders.Length; ++i)
                    {
                        if (Renders[i].render)
                            Renders[i].render.sortingOrder = OrderByCanvas.sortingOrder + Renders[i].offset;
                    }
                }
            }
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        private void Update()
        {
            UpdateOrder();
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(RendersOrderBy), true)]
    [CanEditMultipleObjects]
    public class RendersOrderByEditor : Editor
    {
        bool m_bExpand = false;
        System.Collections.Generic.List<RendersOrderBy.RenderOrder> m_vRenders;
        //------------------------------------------------------
        void OnEnable()
        {
            m_vRenders = new System.Collections.Generic.List<RendersOrderBy.RenderOrder>();
            RendersOrderBy order = target as RendersOrderBy;

            if(order.Renders!=null)
            {
                for (int i = 0; i < order.Renders.Length; ++i)
                {
                    if (order.Renders[i].render)
                        m_vRenders.Add(order.Renders[i]);
                }
            }
            if (m_vRenders.Count<=0)
            {
                Renderer[] renders = order.GetComponentsInChildren<Renderer>();
                int startOrder = 1000;
                for(int i = 0; i < renders.Length; ++i)
                {
                    startOrder = Mathf.Min(startOrder, renders[i].sortingOrder);

                }
                for(int i = 0; i < renders.Length; ++i)
                {
                    m_vRenders.Add(new RendersOrderBy.RenderOrder(renders[i], renders[i].sortingOrder- startOrder));
                }
            }
        }
        //------------------------------------------------------
        void OnDisable()
        {
            RendersOrderBy order = target as RendersOrderBy;
            order.Renders = m_vRenders.ToArray();
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            RendersOrderBy order = target as RendersOrderBy;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OrderBy"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OrderByCanvas"), true);
            EditorGUILayout.BeginHorizontal();
            m_bExpand = EditorGUILayout.Foldout(m_bExpand, "Renders");
            if(GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(40) }))
            {
                m_vRenders.Add( new RendersOrderBy.RenderOrder(null, 0) );
            }
            if (GUILayout.Button("刷新", new GUILayoutOption[] { GUILayout.Width(40) }))
            {
                m_vRenders.Clear();
                Renderer[] renders = order.GetComponentsInChildren<Renderer>();
                int startOrder = 1000;
                for (int i = 0; i < renders.Length; ++i)
                {
                    startOrder = Mathf.Min(startOrder, renders[i].sortingOrder);

                }
                for (int i = 0; i < renders.Length; ++i)
                {
                    m_vRenders.Add(new RendersOrderBy.RenderOrder(renders[i], renders[i].sortingOrder - startOrder));
                }
            }
            EditorGUILayout.EndHorizontal();
            if (m_bExpand)
            {
                for (int i = 0; i < m_vRenders.Count; ++i)
                {
                    RendersOrderBy.RenderOrder renderO = m_vRenders[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Foldout(true, "Element" + i);
                    if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(40) }))
                    {
                        m_vRenders.RemoveAt(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel++;
                    renderO.render = EditorGUILayout.ObjectField("Render", renderO.render, typeof(Renderer), true) as Renderer;
                    renderO.offset = EditorGUILayout.IntField("Offset", renderO.offset);
                    EditorGUI.indentLevel--;
                    m_vRenders[i] = renderO;
                }
            }
            order.Renders = m_vRenders.ToArray();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
