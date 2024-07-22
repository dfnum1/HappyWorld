using UnityEngine;

namespace TopGame.Core
{
    [ExecuteAlways]
    [RequireComponent(typeof(Renderer))]
    public class RenderSortByDistance : MonoBehaviour
    {
        public Transform distanceCamera;
        public Renderer sortRenderer;
        public int appendSort = 0;
        //------------------------------------------------------
        private void Update()
        {
            if (distanceCamera == null) return;
#if UNITY_EDITOR
            if (sortRenderer == null)
                sortRenderer = GetComponent<Renderer>();
#endif
            if (sortRenderer != null)
                sortRenderer.sortingOrder = Mathf.Clamp(16383+ appendSort - (int)((distanceCamera.position-transform.position).magnitude), -32768, 32767);
        }
#if UNITY_EDITOR
        private void OnEnable()
        {
            if(sortRenderer == null)
                sortRenderer = GetComponent<Renderer>();
        }
#endif
    }
}
