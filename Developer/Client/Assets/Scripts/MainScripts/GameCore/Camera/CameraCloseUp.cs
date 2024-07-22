/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	CameraCloseUp
作    者:	HappLI
描    述:	相机特写
*********************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
namespace TopGame.Core
{
    public class CameraCloseUp : ACameraEffect
    {
        private Vector3 m_vDuration;
        private Vector3 m_vLookAtOffset;
        private Vector3 m_vTranslationOffset;
        private float m_fToFov;

        //runing time data
        private bool m_bEnable = false;
        private Vector3 m_vDurationDelta;
        private Vector3 m_vCloseUpTransOffset;
        private Vector3 m_vCloseUpDirOffset;
        private Vector3 m_vCurrentTrans;
        private Vector3 m_vCurrentLookAt;
        private float m_fCurrentFov;

        Transform m_pTarget = null;
        //--------------------------------------------------------------------------------------
        public override void Start(EType type)
        {
            type = EType.Direct;
            m_bEnable = m_vDuration.x > 0;
            base.Start(type);
        }
        //------------------------------------------------------
        public override bool CanDo()
        {
            return m_bEnable;
        }
        //------------------------------------------------------
        public void CloseUp(Transform pTarget, Vector3 vDuration, Vector3 vLookat, Vector3 vTranslate ,float fFov)
        {
            m_vDuration = vDuration;
            m_vLookAtOffset = vLookat;
            m_vTranslationOffset = vTranslate;
            m_fToFov = fFov;

            m_pTarget = pTarget;

            if (!m_bEnable)
            {
                m_vCurrentTrans = CameraController.MainCameraPoition;
                m_vCurrentLookAt = CameraController.MainCameraLookAt;
                m_fCurrentFov = CameraController.MainCameraFOV;
            }
            if (m_fToFov <= 0) m_fToFov = m_fCurrentFov;
            m_bEnable = true;
            m_vDurationDelta = m_vDuration;
        }
        //------------------------------------------------------
        public override void Stop()
        {
            m_vDuration = Vector3.zero;
            m_bEnable = false;
            base.Stop();
        }
        //------------------------------------------------------
        void GetRuntimeParameter(ref Vector3 vTargetTranslation, ref Vector3 vTargetEulerAngle, ref float fFov, float fFrameTime)
        {
            if (m_pTarget)
            {
                Vector3 vLookAt = m_pTarget.position
                                                 + m_vLookAtOffset.x * m_pTarget.right
                                                 + m_vLookAtOffset.y * m_pTarget.up
                                                 + m_vLookAtOffset.z * m_pTarget.forward;

                Vector3 vTrans = m_pTarget.position
                                 + m_vTranslationOffset.x * m_pTarget.right
                                 + m_vTranslationOffset.y * m_pTarget.up
                                 + m_vTranslationOffset.z * m_pTarget.forward;

                if (m_vDurationDelta.x > 0.0f)
                {
                    m_vDurationDelta.x -= fFrameTime;
                    m_vDurationDelta.x = Mathf.Max(m_vDurationDelta.x, 0);
                    float fFactor = m_vDurationDelta.x / m_vDuration.x;
                    fFov = m_fToFov * (1.0f - fFactor) + m_fCurrentFov * fFactor;
                    vTargetTranslation = vTrans * (1.0f - fFactor) + (m_vCurrentTrans + m_vCloseUpTransOffset) * fFactor;
                    Vector3 lookat = vLookAt * (1.0f - fFactor) + (m_vCurrentLookAt + m_vCloseUpDirOffset) * fFactor;
                    vTargetEulerAngle = Framework.Core.CommonUtility.LookRotation(lookat,vTargetTranslation).eulerAngles;
                }
                else if (m_vDurationDelta.y > 0.0f)
                {
                    m_vDurationDelta.y -= fFrameTime;
                    m_vDurationDelta.y = Mathf.Max(m_vDurationDelta.y, 0.0f);
                    vTargetTranslation = vTrans;
                    vTargetEulerAngle = Framework.Core.CommonUtility.LookRotation(vLookAt,vTargetTranslation).eulerAngles;
                }
                else if (m_vDurationDelta.z >= 0.0f)
                {
                    m_vDurationDelta.z -= fFrameTime;
                    m_vDurationDelta.z = Mathf.Max(m_vDurationDelta.z, 0.0f);
                    if (m_vDurationDelta.z <= 0.0f)
                        m_bEnable = false;
                    float fFactor = 0;
                    if (m_vDuration.z > 0)
                        fFactor = m_vDurationDelta.z / m_vDuration.z;
                    fFov = m_fToFov * fFactor + m_fCurrentFov * (1.0f - fFactor);
                    vTargetTranslation = vTrans * fFactor + m_vCurrentTrans * (1.0f - fFactor);
                    Vector3 lookat = vLookAt * fFactor + m_vCurrentLookAt * (1.0f - fFactor);
                    vTargetEulerAngle = Framework.Core.CommonUtility.LookRotation(lookat,vTargetTranslation).eulerAngles;
                }
            }
            else m_bEnable = false;
        }
        //------------------------------------------------------
        public override bool Update(ICameraController pController, float fFrame)
        {
            if(m_bEnable)
            {
                Vector3 position = Vector3.zero;
                Vector3 eulerAngle = Vector3.zero;
                float fov = 45;
                GetRuntimeParameter(ref position, ref eulerAngle, ref fov, fFrame);
                for (int i = 0; i < m_vCallback.Count; ++i)
                {
                    m_vCallback[i].OnEffectPosition(position);
                    m_vCallback[i].OnEffectEulerAngle(eulerAngle);
                    m_vCallback[i].OnEffectFov(fov);
                }
            }
            return m_bEnable;
        }
    }
}