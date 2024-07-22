/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	IPostEffect
作    者:	HappLI
描    述:	后处理接口类,优化需求，统一走commandbuffer,对于urp 渲染管线，是无效果的，如果升级为urp ，需要修改
*********************************************************************/
using System.Collections;
using UnityEngine;
namespace TopGame.Post
{
    public abstract class APostEffect : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        protected Shader m_shader = null;

        protected Material m_material;
#if UNITY_EDITOR
        public Shader shader
        {
            get { return m_shader; }
            set { m_shader = value; }
        }
#endif
        //------------------------------------------------------
        private void Awake()
        {
            enabled = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline == null;
            if (enabled)
            {
                if (m_material == null && m_shader)
                {
                    m_material = new Material(m_shader);
                    m_material.hideFlags = HideFlags.DontSave;
                }
                OnAwake();
            }
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            DestroyImmediate(m_material);
            m_material = null;

            OnClear();
        }
        //------------------------------------------------------
        private void Update()
        {
            OnUpdate(Time.deltaTime);
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        void OnEnable()
        {
            if (UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline != null) return;
            if (m_material == null && m_shader)
            {
                m_material = new Material(shader);
                m_material.hideFlags = HideFlags.DontSave;
            }
        }
        //------------------------------------------------------
        void OnDisable()
        {
            DestroyImmediate(m_material);
            m_material = null;
        }
#endif
        //------------------------------------------------------
        protected virtual void OnAwake() { }
        protected virtual void OnClear() { }
        protected virtual void OnUpdate(float fTime) { }

    }
}
