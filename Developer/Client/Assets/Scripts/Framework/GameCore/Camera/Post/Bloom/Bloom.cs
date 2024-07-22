using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Post
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("PostEffect/Bloom")]
    public class Bloom : APostEffect
    {
        struct PropertyID
        {
            int m_BaseTex;
            int m_Threshold;
            int m_Curve;
            int m_PrefilterOffs;
            int m_SampleScale;
            int m_Intensity;

            public int _BaseTex
            {
                get { if (m_BaseTex == 0) m_BaseTex = Shader.PropertyToID("_BaseTex"); return m_BaseTex; }
            }
            public int _Threshold
            {
                get { if (m_Threshold == 0) m_Threshold = Shader.PropertyToID("_Threshold"); return m_Threshold; }
            }
            public int _Curve
            {
                get { if (m_Curve == 0) m_Curve = Shader.PropertyToID("_Curve"); return m_Curve; }
            }
            public int _PrefilterOffs
            {
                get { if (m_PrefilterOffs == 0) m_PrefilterOffs = Shader.PropertyToID("_PrefilterOffs"); return m_PrefilterOffs; }
            }
            public int _SampleScale
            {
                get { if (m_SampleScale == 0) m_SampleScale = Shader.PropertyToID("_SampleScale"); return m_SampleScale; }
            }
            public int _Intensity
            {
                get { if (m_Intensity == 0) m_Intensity = Shader.PropertyToID("_Intensity"); return m_Intensity; }
            }
        }
        PropertyID m_Property = new PropertyID();

        #region Public Properties

        /// Prefilter threshold (gamma-encoded)
        /// Filters out pixels under this level of brightness.
        public float thresholdGamma
        {
            get { return Mathf.Max(_threshold, 0); }
            set { _threshold = value; }
        }

        /// Prefilter threshold (linearly-encoded)
        /// Filters out pixels under this level of brightness.
        public float thresholdLinear
        {
            get { return GammaToLinear(thresholdGamma); }
            set { _threshold = LinearToGamma(value); }
        }

        [SerializeField]
        [Tooltip("Filters out pixels under this level of brightness.")]
        float _threshold = 0.8f;

        /// Soft-knee coefficient
        /// Makes transition between under/over-threshold gradual.
        public float softKnee
        {
            get { return _softKnee; }
            set { _softKnee = value; }
        }

        [SerializeField, Range(0, 1)]
        [Tooltip("Makes transition between under/over-threshold gradual.")]
        float _softKnee = 0.5f;

        /// Bloom radius
        /// Changes extent of veiling effects in a screen
        /// resolution-independent fashion.
        public float radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        [SerializeField, Range(1, 7)]
        [Tooltip("Changes extent of veiling effects\n" +
                 "in a screen resolution-independent fashion.")]
        float _radius = 2.5f;

        /// Bloom intensity
        /// Blend factor of the result image.
        public float intensity
        {
            get { return Mathf.Max(_intensity, 0); }
            set { _intensity = value; }
        }

        [SerializeField]
        [Tooltip("Blend factor of the result image.")]
        float _intensity = 0.8f;

        /// High quality mode
        /// Controls filter quality and buffer resolution.
        public bool highQuality
        {
            get { return _highQuality; }
            set { _highQuality = value; }
        }

        [SerializeField]
        [Tooltip("Controls filter quality and buffer resolution.")]
        bool _highQuality = true;

        /// Anti-flicker filter
        /// Reduces flashing noise with an additional filter.
        [SerializeField]
        [Tooltip("Reduces flashing noise with an additional filter.")]
        bool _antiFlicker = true;

        public bool antiFlicker
        {
            get { return _antiFlicker; }
            set { _antiFlicker = value; }
        }

        #endregion

        #region Private Members

        const int kMaxIterations = 16;
        RenderTexture[] _blurBuffer1 = new RenderTexture[kMaxIterations];
        RenderTexture[] _blurBuffer2 = new RenderTexture[kMaxIterations];

        float LinearToGamma(float x)
        {
#if UNITY_5_3_OR_NEWER
            return Mathf.LinearToGammaSpace(x);
#else
            if (x <= 0.0031308f)
                return 12.92f * x;
            else
                return 1.055f * Mathf.Pow(x, 1 / 2.4f) - 0.055f;
#endif
        }

        float GammaToLinear(float x)
        {
#if UNITY_5_3_OR_NEWER
            return Mathf.GammaToLinearSpace(x);
#else
            if (x <= 0.04045f)
                return x / 12.92f;
            else
                return Mathf.Pow((x + 0.055f) / 1.055f, 2.4f);
#endif
        }

        #endregion

        #region MonoBehaviour Functions
        //------------------------------------------------------
        protected override void OnAwake()
        {
            if (m_material == null)
            {
                var shader = m_shader ? m_shader : Shader.Find("SD/Post/SD_Bloom");
                m_material = new Material(shader);
                m_material.hideFlags = HideFlags.DontSave;
            }
        }
        //------------------------------------------------------
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (m_material == null)
            {
                Graphics.Blit(source, destination);
                return;
            }
            var useRGBM = Application.isMobilePlatform;

            // source texture size
            var tw = source.width;
            var th = source.height;

            // halve the texture size for the low quality mode
            if (!_highQuality)
            {
                tw /= 4;
                th /= 4;
            }

            // blur buffer format
            var rtFormat = useRGBM ?
                RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;

            // determine the iteration count
            var logh = Mathf.Log(th, 2) + _radius - 8;
            var logh_i = (int)logh;
            var iterations = Mathf.Clamp(logh_i, 1, kMaxIterations);

            // update the shader properties
            var lthresh = thresholdLinear;
            m_material.SetFloat(m_Property._Threshold, lthresh);

            var knee = lthresh * _softKnee + 1e-5f;
            var curve = new Vector3(lthresh - knee, knee * 2, 0.25f / knee);
            m_material.SetVector(m_Property._Curve, curve);

            var pfo = !_highQuality && _antiFlicker;
            m_material.SetFloat(m_Property._PrefilterOffs, pfo ? -0.5f : 0.0f);

            m_material.SetFloat(m_Property._SampleScale, 0.5f + logh - logh_i);
            m_material.SetFloat(m_Property._Intensity, intensity);

            // prefilter pass
            var prefiltered = RenderTexture.GetTemporary(tw, th, 0, rtFormat);
            var pass = _antiFlicker ? 1 : 0;
            Graphics.Blit(source, prefiltered, m_material, pass);

            // construct a mip pyramid
            var last = prefiltered;
            for (var level = 0; level < iterations; level++)
            {
                _blurBuffer1[level] = RenderTexture.GetTemporary(
                    last.width / 2, last.height / 2, 0, rtFormat
                );

                pass = (level == 0) ? (_antiFlicker ? 3 : 2) : 4;
                Graphics.Blit(last, _blurBuffer1[level], m_material, pass);

                last = _blurBuffer1[level];
            }

            // upsample and combine loop
            for (var level = iterations - 2; level >= 0; level--)
            {
                var basetex = _blurBuffer1[level];
                m_material.SetTexture(m_Property._BaseTex, basetex);

                _blurBuffer2[level] = RenderTexture.GetTemporary(
                    basetex.width, basetex.height, 0, rtFormat
                );

                pass = _highQuality ? 6 : 5;
                Graphics.Blit(last, _blurBuffer2[level], m_material, pass);
                last = _blurBuffer2[level];
            }

            // finish process
            m_material.SetTexture(m_Property._BaseTex, source);
            pass = _highQuality ? 8 : 7;
            Graphics.Blit(last, destination, m_material, pass);

            // release the temporary buffers
            for (var i = 0; i < kMaxIterations; i++)
            {
                if (_blurBuffer1[i] != null)
                    RenderTexture.ReleaseTemporary(_blurBuffer1[i]);

                if (_blurBuffer2[i] != null)
                    RenderTexture.ReleaseTemporary(_blurBuffer2[i]);

                _blurBuffer1[i] = null;
                _blurBuffer2[i] = null;
            }

            RenderTexture.ReleaseTemporary(prefiltered);
        }
        #endregion
    }
}