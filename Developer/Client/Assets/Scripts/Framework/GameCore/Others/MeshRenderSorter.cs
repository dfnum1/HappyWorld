using UnityEngine;

namespace TopGame.Core
{
    [RequireComponent(typeof(Renderer))]
    public class MeshRenderSorter : MonoBehaviour
    {
        public int sortingOrder;
        Renderer m_MeshRenderer;
        void Awake()
        {
            m_MeshRenderer = GetComponent<Renderer>();
            if (m_MeshRenderer != null)
                m_MeshRenderer.sortingOrder = sortingOrder;
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (m_MeshRenderer != null)
                m_MeshRenderer.sortingOrder = sortingOrder;
        }
#endif
    }
}
