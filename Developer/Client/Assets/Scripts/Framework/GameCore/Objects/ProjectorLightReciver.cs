/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ProjectorLightReciver
作    者:	HappLI
描    述:	探照灯接受器
*********************************************************************/
using Framework.Core;
using UnityEngine;

namespace TopGame.Core
{
    public class ProjectorLightReciver : MonoBehaviour
    {
        public static float fProjectorLightCheckGap = 1;
        public bool bUsedBlock = true;
        public float fLerpSpeed= 10;
        float m_fProjectorLastTime = 0;
        Renderer[] m_pRenders;
        bool m_bReciver = false;
        Transform m_pTransform;

        bool m_bProjectoring = false;
        bool m_bDirtyColor = false;
        Color m_Color = Color.white;
        Color m_ToColor = Color.white;
        private void Awake()
        {
            m_pTransform = transform;
            m_pRenders = GetComponentsInChildren<Renderer>();
            m_bReciver = m_pRenders != null && m_pRenders.Length > 0;
            if(m_bReciver)
            {
                bool bHasProjectorDef = false;
                for(int i = 0; i < m_pRenders.Length; ++i)
                {
                    if(m_pRenders[i].sharedMaterial && m_pRenders[i].sharedMaterial.HasProperty(Framework.Core.MaterailBlockUtil.ProjectorColorID))
                    {
                        bHasProjectorDef = true;
                        break;
                    }
                }
                if (!bHasProjectorDef)
                    m_bReciver = false;
            }
            enabled = m_bReciver;
        }
        //------------------------------------------------------
        private void Update()
        {
            if (!m_bReciver) return;
            if(Time.time - m_fProjectorLastTime > fProjectorLightCheckGap)
            {
                if(Framework.Base.IntersetionUtil.PositionInView(CameraKit.MainCameraCulling, m_pTransform.position, 1))
                {
                    Color color = m_ToColor;
                    bool bChecking = ProjectorLightCatch.ScanProjector(m_pTransform.position, out color);
                    if (bChecking != m_bProjectoring || !Framework.Core.CommonUtility.Equal(m_Color, color, 0.01f))
                    {
                        m_bProjectoring = bChecking;
                        m_ToColor = color;
                        m_bDirtyColor = true;
                    }
                }

                m_fProjectorLastTime = Time.time;
            }

            if(m_bDirtyColor)
            {
                if (fLerpSpeed > 0)
                    m_Color = Color.Lerp(m_Color, m_ToColor, Time.deltaTime * fLerpSpeed);
                else
                    m_Color = m_ToColor;

                if (bUsedBlock)
                    MaterailBlockUtil.SetRendersColor(m_pRenders, "_ProjectorColor", m_Color);
                else
                {
                    for (int i = 0; i < m_pRenders.Length; ++i)
                    {
                        if (m_pRenders[i])
                            m_pRenders[i].sharedMaterial.SetColor(MaterailBlockUtil.ProjectorColorID, m_Color);
                    }
                }

                if(Framework.Core.CommonUtility.Equal(m_Color, m_ToColor, 0.01f))
                {
                    m_bDirtyColor = false;
                }
            }

        }
    }
}
