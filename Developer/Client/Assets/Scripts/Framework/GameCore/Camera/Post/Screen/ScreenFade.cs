/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ScreenFade
作    者:	HappLI
描    述:	屏幕效果
*********************************************************************/
using UnityEngine;
namespace TopGame.Post
{
    public class ScreenFade : APostEffect
    {
        public float lerpTime = 0.5f;
        public Color fadeColor = new Color(0.01f, 0.01f, 0.01f, 1.0f);
        private Material m_FadeMaterial = null;

        AnimationCurve m_FadeCurve;
        private bool m_bFading = false;
        float m_fDurationTime = 0;
        float m_fMaxTime = 0;
        //------------------------------------------------------
        protected override void OnAwake()
        {
            m_FadeMaterial = new Material(Shader.Find("SD/Post/SD_ScreenFade"));
            m_bFading = false;
            m_FadeCurve = null;
            m_fDurationTime = 0;
            m_fMaxTime = 0;
        }
        //------------------------------------------------------
        protected override void OnClear()
        {
            if (m_FadeMaterial != null)
            {
                Destroy(m_FadeMaterial);
            }
        }
        //------------------------------------------------------
        public void BeginFade(Color fadeColor, AnimationCurve curve)
        {
            m_FadeCurve = curve;
            m_fMaxTime = Framework.Core.BaseUtil.GetCurveMaxTime(curve);
            m_fDurationTime = 0;
            this.fadeColor = fadeColor;
            m_bFading = curve!=null && m_fMaxTime > 0 && m_FadeMaterial!=null;
        }
        //------------------------------------------------------
        protected override void OnUpdate(float fTime)
        {
            if(m_bFading)
            {
                m_fDurationTime += fTime;

                float alpha = m_FadeCurve.Evaluate(m_fDurationTime);
                fadeColor.a = Mathf.Lerp(fadeColor.a, alpha, lerpTime);
                m_FadeMaterial.color = fadeColor;

                if( Mathf.Abs(fadeColor.a-alpha) <= 0.1f  && m_fDurationTime >= m_fMaxTime)
                    m_bFading = false;
            }
        }
        //------------------------------------------------------
        void OnPostRender()
        {
            if (m_bFading && m_FadeMaterial)
            {
                m_FadeMaterial.SetPass(0);
                GL.PushMatrix();
                GL.LoadOrtho();
                GL.Color(m_FadeMaterial.color);
                GL.Begin(GL.QUADS);
                GL.Vertex3(0f, 0f, -12f);
                GL.Vertex3(0f, 1f, -12f);
                GL.Vertex3(1f, 1f, -12f);
                GL.Vertex3(1f, 0f, -12f);
                GL.End();
                GL.PopMatrix();
            }
        }
    }
}
