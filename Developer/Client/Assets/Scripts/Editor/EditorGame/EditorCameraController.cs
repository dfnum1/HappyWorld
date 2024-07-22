/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	EditorModule
作    者:	HappLI
描    述:	编辑器主模块
*********************************************************************/
using Framework.Core;
using System;
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Post;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class EditorCameraController : ICameraController, Core.ICameraEffectCallback
    {
        private Camera m_Camera = null;
        private float m_fTweenDeltaScale = 10;

        private Transform m_pCameaRoot = null;
        private CameraMode m_pCurrentMode = null;
        private Dictionary<string, CameraMode> m_vCameraModes;

        List<ACameraEffect> m_vCameraEffect = new List<ACameraEffect>();
        List<APostEffect> m_vPostEffect = new List<APostEffect>();
        private bool m_bEnable = true;

        private Vector3 m_EffectPos = Vector3.zero;
        private Vector3 m_EffectEulerAngle = Vector3.zero;
        private Vector3 m_EffectLookAt = Vector3.zero;
        private float m_EffectFov = 0;
        public EditorCameraController()
        {
            m_bEnable = true;
            m_vCameraModes = new Dictionary<string, CameraMode>(4);
            m_pCurrentMode = null;
        }
        public bool IsEditorMode()
        {
            return false;
        }
        //------------------------------------------------------
        public void SetEditor(bool bEditor)
        {

        }
        //------------------------------------------------------
        public void StartUp()
        {

        }
        //------------------------------------------------------
        public bool IsEnable()
        {
            return m_bEnable;
        }
        //------------------------------------------------------
        public void Enable(bool bEnable)
        {
            m_bEnable = bEnable;
        }
        //------------------------------------------------------    
        public bool IsClosed()
        {
            return false;
        }
        //------------------------------------------------------    
        public void SetLerpFormToMode(Vector3 transPosition, Vector3 eulerAngle, float fov, float fTime) { }
        //------------------------------------------------------    
        public void SetCameraSlot(ACameraSlots.Slot pSlot, float fLerpTime = 0.5F) { }
        //------------------------------------------------------
        public UnityEngine.Rendering.Universal.UniversalAdditionalCameraData GetURPCamera()
        {
            return null;
        }
        //------------------------------------------------------
        public void SetURPCamera(UnityEngine.Rendering.Universal.UniversalAdditionalCameraData camera)
        {
        }
        //------------------------------------------------------
        public void SetPostProcess(UnityEngine.Rendering.VolumeProfile profiler)
        {
        }
        //------------------------------------------------------
        public void SetURPAsset(UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset)
        {
        }
        //------------------------------------------------------
        public void AddCameraStack(Camera pCamera, ECameraType type = ECameraType.Count, bool bAfter = true)
        {
        }
        //------------------------------------------------------
        public void RemoveCameraStack(Camera pCamera)
        {
        }
        //------------------------------------------------------
        public void SwapCamera(Camera pCamera0, Camera pCamera1)
        {

        }
        //------------------------------------------------------    
        public void CloseCamera(bool bClose)
        {

        }
        //------------------------------------------------------
        public void CloseCameraRef(bool bClose)
        {

        }
        //------------------------------------------------------
        public void ActiveRoot(bool bActive)
        {

        }
        //------------------------------------------------------
        public void ActiveVolume(bool bActive)
        {

        }
        //------------------------------------------------------
        public bool IsActiveVolume()
        {
            return true;
        }
        //------------------------------------------------------
        public UnityEngine.Rendering.Volume GetPostProcessVolume()
        {
            return null;
        }
        //------------------------------------------------------
        public void UpdateActiveVolume()
        {

        }
        //------------------------------------------------------
        public bool Init(Camera root)
        {
            m_pCameaRoot = root.transform;
            m_vCameraEffect = new List<ACameraEffect>(4);
            m_vPostEffect = new List<APostEffect>();
            APostEffect[] effects = root.GetComponentsInChildren<APostEffect>();
            for (int i = 0; i < effects.Length; ++i)
            {
                m_vPostEffect.Add(effects[i]);
            }
            return true;
        }
        //------------------------------------------------------
        public void StopAllEffect()
        {
            for (int i = 0; i < m_vCameraEffect.Count; ++i)
            {
                m_vCameraEffect[i].Stop();
            }
        }
        //------------------------------------------------------
        public bool IsTweenEffecting(float fFactorError = 0.1f) { return false; }
        //------------------------------------------------------
        public T GetEffect<T>() where T : ACameraEffect, new()
        {
            for (int i = 0; i < m_vCameraEffect.Count; ++i)
            {
                if (m_vCameraEffect[i].GetType() == typeof(T))
                    return (T)m_vCameraEffect[i];
            }
            T newEffect = new T();
            newEffect.Register(this);
            m_vCameraEffect.Add(newEffect);
            return newEffect;
        }
        //------------------------------------------------------
        public T GetPost<T>() where T : APostEffect, new()
        {
            for (int i = 0; i < m_vPostEffect.Count; ++i)
            {
                if (m_vPostEffect[i].GetType() == typeof(T))
                    return (T)m_vPostEffect[i];
            }
            return null;
        }
        //------------------------------------------------------
        public void SetCamera(Camera pCamera)
        {
            m_Camera = pCamera;
            if(pCamera!=null)
            m_pCameaRoot = pCamera.transform;
        }
        //------------------------------------------------------
        public float GetTweenScale()
        {
            return m_fTweenDeltaScale;
        }
        //------------------------------------------------------
        public void UpdateFov(float fFov)
        {
            if (m_Camera == null) return;
            m_Camera.fieldOfView = fFov;
        }
        //------------------------------------------------------
        public float GetFov()
        {
            if (m_Camera == null) return 45;
            return m_Camera.fieldOfView;
        }
        //------------------------------------------------------
        public Ray ScreenPointToRay(Vector3 mousePosition)
        {
            return GetCamera().ScreenPointToRay(mousePosition);
        }
        //------------------------------------------------------
        public void RegisterCameraMode(string strMode, CameraMode pMode)
        {
            pMode.SetController(this);
            if (m_vCameraModes.ContainsKey(strMode)) return;
            m_vCameraModes.Add(strMode, pMode);
        }
        //------------------------------------------------------
        public CameraMode SwitchMode(string strMode, bool bEnd = true)
        {
            CameraMode pMode;
            if (m_vCameraModes.TryGetValue(strMode, out pMode))
            {
                if (bEnd && m_pCurrentMode != null) m_pCurrentMode.End();
                m_pCurrentMode = pMode;
                m_pCurrentMode.Start();
            }
            return m_pCurrentMode;
        }
        //------------------------------------------------------
        public void SwitchMode(CameraMode pMode, bool bEnd = true)
        {
            if (m_pCurrentMode == pMode) return;
            if (bEnd && m_pCurrentMode != null) m_pCurrentMode.End();
            m_pCurrentMode = pMode;
            m_pCurrentMode.Start();
        }
        //------------------------------------------------------
        public CameraMode GetCurrentMode()
        {
            return m_pCurrentMode;
        }
        //------------------------------------------------------
        public Vector3 GetCurrentLookAt()
        {
            if (m_pCurrentMode == null) return Vector3.zero;
            return m_pCurrentMode.GetCurrentLookAt();
        }
        //------------------------------------------------------
        public Vector3 GetCurrentFollowLookAt()
        {
            if (m_pCurrentMode == null) return Vector3.zero;
            return m_pCurrentMode.GetFollowLookAtPosition();
        }
        //------------------------------------------------------
        public CameraMode GetMode(string strMode)
        {
            CameraMode pMode;
            if (m_vCameraModes.TryGetValue(strMode, out pMode))
            {
                return pMode;
            }
            return null;
        }
        //------------------------------------------------------
        public Vector3 GetDir()
        {
            if (m_pCameaRoot == null) return Vector3.forward;
            return m_pCameaRoot.forward;
        }
        //------------------------------------------------------
        public Vector3 GetRight()
        {
            if (m_pCameaRoot == null) return Vector3.right;
            return m_pCameaRoot.right;
        }
        //------------------------------------------------------
        public Vector3 GetUp()
        {
            if (m_pCameaRoot == null) return Vector3.up;
            return m_pCameaRoot.up;
        }
        //------------------------------------------------------
        public Vector3 GetPosition()
        {
            if (m_pCameaRoot == null) return Vector3.zero;
            return m_pCameaRoot.position;
        }
        //------------------------------------------------------
        public Vector3 GetEulerAngle()
        {
            if (m_pCameaRoot == null) return Vector3.zero;
            return m_pCameaRoot.eulerAngles;
        }
        //------------------------------------------------------
        public Transform GetTransform()
        {
            return m_pCameaRoot;
        }
        //-------------------------------------------
        public bool IsInView(Vector3 pos, float factor = 0.1f)
        {
            if (m_Camera == null) return false;
            Vector2 viewPos = m_Camera.WorldToViewportPoint(pos);
            Vector3 dir = (pos - GetPosition()).normalized;
            float dot = Vector3.Dot(GetDir(), dir);
            if (dot > 0 && viewPos.x >= -factor && viewPos.x <= 1 + factor && viewPos.y >= -factor && viewPos.y <= 1 + factor)
                return true;
            return false;
        }
        //-------------------------------------------
        public bool IsInView(Vector3 pos, Rect viewRc)
        {
            if (m_Camera == null) return false;
            Vector2 viewPos = m_Camera.WorldToViewportPoint(pos);
            Vector3 dir = (pos - GetPosition()).normalized;
            float dot = Vector3.Dot(GetDir(), dir);
            if (dot > 0 && viewPos.x >= viewRc.xMin && viewPos.x <= viewRc.xMax && viewPos.y >= viewRc.yMin && viewPos.y <= viewRc.yMax)
                return true;
            return false;
        }
        //------------------------------------------------------
        public void ForceUpdate(float fFrame)
        {
            Base.GlobalShaderController.TestUpdate();
            if (!m_bEnable || m_pCurrentMode == null || m_pCameaRoot == null) return;
            m_EffectPos = Vector3.zero;
            m_EffectEulerAngle = Vector3.zero;
            m_EffectLookAt = Vector3.zero;
            m_EffectFov = 0;

            bool hasDirectEffect = false;
            for (int i = 0; i < m_vCameraEffect.Count; ++i)
            {
                if (m_vCameraEffect[i] != null && m_vCameraEffect[i].CanDo())
                {
                    m_vCameraEffect[i].Update(this, fFrame);
                    if (!hasDirectEffect && m_vCameraEffect[i].GetModeType() == ACameraEffect.EType.Direct)
                        hasDirectEffect = true;
                }
            }
            if (hasDirectEffect)
            {
                m_pCameaRoot.position = m_EffectPos;
                m_pCameaRoot.eulerAngles = m_EffectEulerAngle;
                UpdateFov(m_EffectFov);
                return;
            }

            m_pCurrentMode.Update(fFrame);

            m_pCameaRoot.position = m_pCurrentMode.GetCurrentTrans() + m_pCurrentMode.GetLockCameraOffset() + m_EffectPos;
            Vector3 dir = m_pCurrentMode.GetCurrentLookAt() + m_pCurrentMode.GetFinalLookAtOffset() + m_EffectLookAt - m_pCameaRoot.position;
            if (!Framework.Core.CommonUtility.Equal(dir, Vector3.zero))
            {
                Vector3 eulerAngles = Quaternion.LookRotation(dir, m_pCurrentMode.GetCurrentUp() + m_pCurrentMode.GetLockUpOffset()).eulerAngles;
                m_pCameaRoot.eulerAngles = eulerAngles + m_pCurrentMode.GetLockEulerAngleOffset() + m_EffectEulerAngle;
            }

            UpdateFov(m_pCurrentMode.GetCurrentFov() + m_pCurrentMode.GetLockFovOffset()+ m_EffectFov);
        }
        //------------------------------------------------------
        public void Shake(float fDuration, Vector3 vIntense, Vector3 vHertz, AnimationCurve dampping = null)
        {
            CameraShake cameraShake = GetEffect<CameraShake>();
            cameraShake.Shake(fDuration, vIntense, vHertz, dampping);
            cameraShake.Start(ACameraEffect.EType.Offset);
        }
        //------------------------------------------------------
        public void CloseUp(Transform pTarget, Vector3 vDuration, Vector3 vLookat, Vector3 vTranslate, float fFov)
        {
            CameraCloseUp cameraShake = GetEffect<CameraCloseUp>();
            cameraShake.CloseUp(pTarget, vDuration, vLookat, vTranslate, fFov);
            cameraShake.Start(ACameraEffect.EType.Direct);
        }
        //------------------------------------------------------
        public Core.IPlayableBase PlayAniPath(int nId, GameObject pTarget = null, bool bAbs = true, float offsetX = 0, float offsetY = 0, float offsetZ = 0)
        {
            CameraAnimPath cameraPath = GetEffect<CameraAnimPath>();
            cameraPath.SetPathID((ushort)nId, bAbs, pTarget);
            cameraPath.SetTriggerPosition(new Vector3(offsetX, offsetY, offsetZ));
            cameraPath.Start(bAbs?ACameraEffect.EType.LockSet:ACameraEffect.EType.Offset);
            return cameraPath.playable;
        }
        //-------------------------------------------
        public bool IsAnimPathPlaying(int nId)
        {
            CameraAnimPath cameraPath = GetEffect<CameraAnimPath>();
            if (cameraPath == null) return false;
            if (cameraPath.playable != null && !cameraPath.playable.isOver())
            {
                if (nId == -1 || cameraPath.playable.GetGuid() == nId)
                    return true;
            }
            return false;
        }
        //------------------------------------------------------
        public Camera GetCamera()
        {
            return m_Camera;
        }

        public void SetCameraClearFlag(CameraClearFlags flag, Color color)
        {

        }


        public Camera GetCamera(ECameraType type = ECameraType.Force)
        {
            return m_Camera;
        }

        public void OnEffectPosition(Vector3 pos)
        {
          //  m_EffectPos += pos;
        }

        public void OnEffectEulerAngle(Vector3 eulerAngle)
        {
         //   m_EffectEulerAngle += eulerAngle;
        }

        public void OnEffectLookAt(Vector3 lookAt)
        {
          //  m_EffectLookAt += lookAt;
        }

        public void OnEffectFov(float fov)
        {
          //  m_EffectFov += fov;
        }

        public void OnFollowPosition(Vector3 pos)
        {
            //  m_EffectPos += pos;
        }

        public Texture CaptureScreenTexture(int sw = -1, int sh = -1, RenderTextureFormat format = RenderTextureFormat.ARGB32)
        {
            return null;
        }
        //------------------------------------------------------
        public void SetCameraCullingMask(ECameraType type, int cullingMask)
        {
        }
        //------------------------------------------------------
        public void RestoreCameraCullingMask(ECameraType type = ECameraType.Count)
        {
        }
        //------------------------------------------------------
        public int GetCameraCullingMask(ECameraType type)
        {
            return -1;
        }

        public void SetCameraNear(float fNear)
        {
        }

        public void SetCameraFar(float fFar)
        {
        }
    }
}
