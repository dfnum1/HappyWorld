/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Vignette
作    者:	HappLI
描    述:	暗角
*********************************************************************/
using UnityEngine;
namespace TopGame.Post
{
    public class Vignette : APostEffect
    {
        internal static readonly int Vignette_Color = Shader.PropertyToID("_Vignette_Color");
        internal static readonly int Vignette_Center = Shader.PropertyToID("_Vignette_Center");
        internal static readonly int Vignette_Settings = Shader.PropertyToID("_Vignette_Settings");
        internal static readonly int Vignette_Mask = Shader.PropertyToID("_Vignette_Mask");
        internal static readonly int Vignette_Opacity = Shader.PropertyToID("_Vignette_Opacity");
        internal static readonly int Vignette_Mode = Shader.PropertyToID("_Vignette_Mode");

        public enum VignetteMode
        {
            /// <summary>
            /// This mode offers parametric controls for the position, shape and intensity of the Vignette.
            /// </summary>
            Classic,

            /// <summary>
            /// This mode multiplies a custom texture mask over the screen to create a Vignette effect.
            /// </summary>
            Masked
        }
        [Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
        public VignetteMode mode = VignetteMode.Classic;

        /// <summary>
        /// The color to use to tint the vignette.
        /// </summary>
        [Tooltip("Vignette color.")]
        public Color color = new Color(0f, 0f, 0f, 1f);

        /// <summary>
        /// Sets the vignette center point (screen center is <c>[0.5,0.5]</c>).
        /// </summary>
        [Tooltip("Sets the vignette center point (screen center is [0.5, 0.5]).")]
        public Vector2 center = new Vector2(0.5f, 0.5f);

        /// <summary>
        /// The amount of vignetting on screen.
        /// </summary>
        [Range(0f, 1f), Tooltip("Amount of vignetting on screen.")]
        public float intensity = 0;

        /// <summary>
        /// The smoothness of the vignette borders.
        /// </summary>
        [Range(0.01f, 1f), Tooltip("Smoothness of the vignette borders.")]
        public float smoothness = 0.2f;

        /// <summary>
        /// Lower values will make a square-ish vignette.
        /// </summary>
        [Range(0f, 1f), Tooltip("Lower values will make a square-ish vignette.")]
        public float roundness = 1;

        /// <summary>
        /// Should the vignette be perfectly round or be dependent on the current aspect ratio?
        /// </summary>
        [Tooltip("Set to true to mark the vignette to be perfectly round. False will make its shape dependent on the current aspect ratio.")]
        public bool rounded = false;

        /// <summary>
        /// A black and white mask to use as a vignette.
        /// </summary>
        [Tooltip("A black and white mask to use as a vignette.")]
        public Texture mask = null;

        /// <summary>
        /// Mask opacity.
        /// </summary>
        [Range(0f, 1f), Tooltip("Mask opacity.")]
        public float opacity =  1f;

        //------------------------------------------------------
        public bool IsEnabledAndSupported()
        {
            return enabled
                && ((mode == VignetteMode.Classic && intensity > 0f)
                || (mode == VignetteMode.Masked && opacity > 0f && mask != null)) &&
                m_material;
        }
        //------------------------------------------------------
        protected override void OnAwake()
        {
            if (m_material == null)
            {
                var shader = m_shader ? m_shader : Shader.Find("SD/Post/SD_Vignette");
                m_material = new Material(shader);
                m_material.hideFlags = HideFlags.DontSave;
            }
        }
        //------------------------------------------------------
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!IsEnabledAndSupported())
            {
                Graphics.Blit(source, destination);
                return;
            }
            m_material.SetColor(Vignette_Color, color);

            if (mode == VignetteMode.Classic)
            {
                m_material.SetFloat(Vignette_Mode, 0f);
                m_material.SetVector(Vignette_Center, center);
                float round_ness = (1f - roundness) * 6f + roundness;
                m_material.SetVector(Vignette_Settings, new Vector4(intensity * 3f, smoothness * 5f, round_ness, rounded ? 1f : 0f));
            }
            else // Masked
            {
                m_material.SetFloat(Vignette_Mode, 1f);
                m_material.SetTexture(Vignette_Mask, mask);
                m_material.SetFloat(Vignette_Opacity, Mathf.Clamp01(opacity));
            }
            Graphics.Blit(source, destination, m_material);
        }
    }
}
