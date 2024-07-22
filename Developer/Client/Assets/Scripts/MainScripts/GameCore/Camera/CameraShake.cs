/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	CameraShake
作    者:	HappLI
描    述:	相机抖动
*********************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
namespace TopGame.Core
{
    public class CameraShake : ACameraEffect
    {
        float m_fCameraShakeDuration = 0f;
        private Vector3 m_vShakeOffset = Vector3.zero;
        private bool m_bEnabled = false;

        float m_fCameraShakeDelta = 0f;
        Vector3 m_vShakeIntense = Vector3.zero;
        Vector3 m_vShakeHertz = Vector3.zero;
        Vector3 m_vInitPosition = Vector3.zero;
        AnimationCurve m_Damping = null;
        //--------------------------------------------------------------------------------------
        public override void Start(EType type)
        {
            if (m_fCameraShakeDelta > 0f)
                m_bEnabled = true;
            else
                m_bEnabled = false;
            base.Start(type);
        }
        //------------------------------------------------------
        public override bool CanDo()
        {
            return m_bEnabled && m_fCameraShakeDelta > 0f;
        }
        //------------------------------------------------------
        public void Shake(float fDuration, Vector3 vIntense, Vector3 vHertz, AnimationCurve damping= null)
        {
            m_bEnabled = true;
            m_fCameraShakeDuration = fDuration;
            m_fCameraShakeDelta = fDuration;
            m_vShakeIntense = vIntense;
            m_vShakeHertz = vHertz;
            m_Damping = damping;
        }
        //------------------------------------------------------
        public override void Stop()
        {
            m_fCameraShakeDuration = 0f;
            m_fCameraShakeDelta = 0f;
            m_vShakeIntense = Vector3.zero;
            m_vShakeHertz = Vector3.zero;
            m_vShakeOffset = Vector3.zero;
            m_Damping = null;

            m_bEnabled = false;

            base.Stop();
        }
        //------------------------------------------------------
        public Vector3 GetShakeOffset()
        {
            return m_vShakeOffset;
        }
        //------------------------------------------------------
        public override bool Update(ICameraController pController, float fFrame)
        {
#if UNITY_EDITOR
            if (CameraKit.IsEditorMode)
                return m_bEnabled;
#endif
            if (m_bEnabled)
            {
                if (m_fCameraShakeDelta > 0f)
                {
                    m_fCameraShakeDelta -= fFrame;
                    m_fCameraShakeDelta = Mathf.Max(0f, m_fCameraShakeDelta);

                    float fFade = Mathf.Clamp01(m_fCameraShakeDelta / m_fCameraShakeDuration);

                    float dampping = 1;
                    if (m_Damping != null && m_Damping.length>0)
                        dampping = m_Damping.Evaluate(1-fFade);

                    float fShakeX = m_vShakeIntense.x * ((float)Mathf.Sin(m_vShakeHertz.x * m_fCameraShakeDelta))* dampping;
                    float fShakeY = m_vShakeIntense.y * ((float)Mathf.Sin(m_vShakeHertz.y * m_fCameraShakeDelta)) * dampping;
                    float fShakeZ = m_vShakeIntense.z * ((float)Mathf.Sin(m_vShakeHertz.z * m_fCameraShakeDelta)) * dampping;

                    m_vShakeOffset = fShakeX * pController.GetDir() + fShakeY * pController.GetUp() + fShakeZ * pController.GetRight();
                    m_vShakeOffset *= fFade;

                }
                else
                {
                    m_vShakeOffset = Vector3.zero;
                    Stop();
                }
                if (m_vCallback != null)
                {
                    for (int i = 0; i < m_vCallback.Count; ++i)
                    {
                        m_vCallback[i].OnEffectPosition(m_vShakeOffset);
                        m_vCallback[i].OnEffectEulerAngle(m_vShakeOffset);
                    }
                }
                return m_bEnabled;
            }
            return m_bEnabled;
        }
    }
}