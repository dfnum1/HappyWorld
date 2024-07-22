/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	CameraController
作    者:	HappLI
描    述:	相机控制
*********************************************************************/
using System;
using System.Collections.Generic;
using TopGame.Post;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Framework.Core;

namespace TopGame.Core
{
    [Framework.Plugin.AT.ATExportMono("相机控制器", Framework.Plugin.AT.EGlobalType.Single)]
    public class CameraController : ICameraController, ICameraEffectCallback
    {
        struct CameraSlot
        {
            public float fTime;
            public float fDuration;
            public CameraSlots.Slot pSlot;
            public bool isExit;
            public static CameraSlot DEFAULT = new CameraSlot() { pSlot = CameraSlots.Slot.DEFAULT , fTime = 0, fDuration = 0 };
            public void Clear()
            {
                fTime = 0;
                fDuration = 0;
                pSlot = CameraSlots.Slot.DEFAULT;
                isExit = false;
            }
            public bool IsValid()
            {
                return pSlot.IsValid();
            }
        }
        struct LerpFromToMode
        {
            public Vector3 tansformPos;
            public Vector3 eulerAngle;
            public float fov;
            public float time;
            public float duration;
            public void Clear()
            {
                time = 0;
                duration = 0;
            }
            public bool IsValid()
            {
                return duration>0 && time < duration;
            }
        }
        static CameraController ms_pInstnace = null;

        CameraSetting m_pCameraSetting = null;
        Transform m_URPCameraTranfrom = null;
        private int[] m_arCameraCullingMask = new int[(int)ECameraType.Count];

        private Transform m_pCameaRoot = null;

        private CameraSlot m_pCameraSlot = CameraSlot.DEFAULT;
        private LerpFromToMode m_LerpFromToMode = new LerpFromToMode();
        private CameraMode m_pCurrentMode = null;
        private Dictionary<string, CameraMode> m_vCameraModes;

        private Vector3 m_EffectPos = Vector3.zero;
        private Vector3 m_EffectEulerAngle = Vector3.zero;
        private Vector3 m_EffectLookAt = Vector3.zero;
        private float m_EffectFov = 0;

        CameraAnimPath m_pAnimPathEffector = null;

        List<ACameraEffect> m_vCameraEffect = null;

        List<APostEffect> m_vPostEffect = null;

        Texture2D m_pCaptureScreenTexture = null;
#if UNITY_EDITOR
        [System.NonSerialized]
        public float m_fYaw =0;
        [System.NonSerialized]
        public float m_fPitch = 0;
        [System.NonSerialized]
        public bool m_bEditor = false;
        [System.NonSerialized]
        Vector3 m_EditorPosition = Vector3.zero;
        [System.NonSerialized]
        public float m_zoomSpeed = 25.0f;
        [System.NonSerialized]
        public float m_dragSpeed = 8.0f;
        [System.NonSerialized]
        public float m_rotateSpeed = 5.0f;
        [System.NonSerialized]
        Vector3 m_lastMousePos;

        public void SetEditor(bool bEditor)
        {
            if(m_pCameaRoot!=null)
            {
                m_fYaw = -m_pCameaRoot.eulerAngles.x;
                m_fPitch = m_pCameaRoot.eulerAngles.y;
                m_EditorPosition = m_pCameaRoot.position;
            }
            m_bEditor = bEditor;
        }
        public void SyncEdit(Vector3 pos, Vector3 eulerAngle)
        {
            if(m_pCameaRoot!=null)
            {
                m_pCameaRoot.position = pos;
                m_pCameaRoot.eulerAngles = eulerAngle;
            }
            m_fYaw = -eulerAngle.x;
            m_fPitch = eulerAngle.y;
            m_EditorPosition = pos;
        }
        public void SyncEditPos(Vector3 pos)
        {
            if (m_pCameaRoot != null)
            {
                m_pCameaRoot.position = pos;
            }
            m_EditorPosition = pos;
        }
        public void SyncEditEuler(Vector3 eulerAngle)
        {
            if (m_pCameaRoot != null)
            {
                m_pCameaRoot.eulerAngles = eulerAngle;
            }
            m_fYaw = -eulerAngle.x;
            m_fPitch = eulerAngle.y;
        }
        public bool IsEditorMode()
        {
            return m_bEditor;
        }
#endif
        //------------------------------------------------------
        public static Camera GetGameMainCamera(ECameraType type = ECameraType.Force)
        {
            if (getInstance() != null)
            {
                return getInstance().GetCamera(ECameraType.Force);
            }
            return null;
        }
        //------------------------------------------------------
        public static bool IsGameMainCamera
        {
            get
            {
                if(getInstance() != null)
                {
                    if (CatchMainCameraUtil.MainCamera == null) return true;
                    return CatchMainCameraUtil.MainCamera == getInstance().GetCamera(ECameraType.Force);
                }
                return false;
            }
        }
        //------------------------------------------------------
        static Camera _GetMainCamera()
        {
            if (getInstance() != null && getInstance().IsEnable())
            {
                Camera camera = getInstance().GetCamera();
                if (camera) return camera;
                return CatchMainCameraUtil.MainCamera;
            }
            else if (CatchMainCameraUtil.MainCamera)
                return CatchMainCameraUtil.MainCamera;

            if (getInstance() != null && getInstance().GetCamera())
                return getInstance().GetCamera();

#if UNITY_EDITOR
            return Camera.main;
#else
            return null;
#endif
        }
        public static Camera MainCamera
        {
            get
            {
                return _GetMainCamera();
            }
        }
        //------------------------------------------------------
        static Vector3 _GetMainCameraPosition()
        {
            if (getInstance() != null && getInstance().IsEnable())
            {
                return getInstance().GetPosition();
            }
            else if (CatchMainCameraUtil.MainTransform) return CatchMainCameraUtil.MainTransform.position;

            if (getInstance() != null && getInstance().GetCamera())
                return getInstance().GetCamera().transform.position;
#if UNITY_EDITOR
            if (Camera.main)
                return Camera.main.transform.position;
            return Vector3.zero;
#else
                return Vector3.zero;
#endif
        }
        public static Vector3 MainCameraPoition
        {
            get { return _GetMainCameraPosition(); }
        }
        //------------------------------------------------------
        static Vector3 _GetMainCameraLookAt()
        {
            if (getInstance() != null && getInstance().IsEnable())
            {
                return getInstance().GetCurrentLookAt();
            }
            else if (CatchMainCameraUtil.MainTransform)
            {
                Vector3 curLookAt = Framework.Core.CommonUtility.RayHitPos(CatchMainCameraUtil.MainTransform.position, CatchMainCameraUtil.MainTransform.forward, 0);
                if (Vector3.Dot(CatchMainCameraUtil.MainTransform.forward, curLookAt - CatchMainCameraUtil.MainTransform.position) < 0)
                {
                    float dist = Vector3.Distance(curLookAt, CatchMainCameraUtil.MainTransform.position);
                    curLookAt = CatchMainCameraUtil.MainTransform.position + (CatchMainCameraUtil.MainTransform.position - curLookAt).normalized * dist;
                }
                return curLookAt;
            }

            if (getInstance() != null)
                return getInstance().GetCurrentLookAt();
#if UNITY_EDITOR
            if (Camera.main)
            {
                Vector3 curLookAt = Framework.Core.CommonUtility.RayHitPos(Camera.main.transform.position, Camera.main.transform.forward, 0);
                if (Vector3.Dot(Camera.main.transform.forward, curLookAt - Camera.main.transform.position) < 0)
                {
                    float dist = Vector3.Distance(curLookAt, Camera.main.transform.position);
                    curLookAt = Camera.main.transform.position + (Camera.main.transform.position - curLookAt).normalized * dist;
                }
                return curLookAt;
            }
            return Vector3.zero;
#else
                return Vector3.zero;
#endif
        }
        public static Vector3 MainCameraLookAt
        {
            get { return _GetMainCameraLookAt(); }
        }
        //------------------------------------------------------
        static Matrix4x4 _GetMainCameraCulling()
        {
            if (getInstance() != null && getInstance().IsEnable())
            {
                return getInstance().GetCamera().cullingMatrix;
            }
            else if (CatchMainCameraUtil.MainCamera) return CatchMainCameraUtil.MainCamera.cullingMatrix;
            if (getInstance() != null && getInstance().GetCamera())
                return getInstance().GetCamera().cullingMatrix;

#if UNITY_EDITOR
            if (Camera.main)
                return Camera.main.cullingMatrix;
            return Matrix4x4.identity;
#else
                return Matrix4x4.identity;
#endif
        }
        public static Matrix4x4 MainCameraCulling
        {
            get { return _GetMainCameraCulling(); }
        }
        //------------------------------------------------------
        static Vector3 _GetMainCameraEulerAngle()
        {
            if (getInstance() != null && getInstance().IsEnable())
            {
                return getInstance().GetEulerAngle();
            }
            else if (CatchMainCameraUtil.MainCamera) return CatchMainCameraUtil.MainTransform.eulerAngles;

            if (getInstance() != null && getInstance().GetCamera())
                return getInstance().GetCamera().transform.eulerAngles;

#if UNITY_EDITOR
            if (Camera.main)
                return Camera.main.transform.eulerAngles;
            return Vector3.zero;
#else
                return Vector3.zero;
#endif
        }
        public static Vector3 MainCameraEulerAngle
        {
            get { return _GetMainCameraEulerAngle(); }
        }
        //------------------------------------------------------
        static Vector3 _GetMainCameraDirection()
        {
            if (getInstance() != null && getInstance().IsEnable())
            {
                return getInstance().GetDir();
            }
            else if (CatchMainCameraUtil.MainCamera) return CatchMainCameraUtil.MainTransform.forward;

            if (getInstance() != null && getInstance().GetCamera())
                return getInstance().GetCamera().transform.forward;

#if UNITY_EDITOR
            if (Camera.main)
                return Camera.main.transform.forward;
            return Vector3.forward;
#else
                return Vector3.forward;
#endif       
        }
        public static Vector3 MainCameraDirection
        {
            get { return _GetMainCameraDirection(); }
        }
        //------------------------------------------------------
        static float _GetMainCameraFOV()
        {
            if (getInstance() != null && getInstance().IsEnable())
            {
                return getInstance().GetFov();
            }
            else if (CatchMainCameraUtil.MainCamera) return CatchMainCameraUtil.MainCamera.fieldOfView;

            if (getInstance() != null && getInstance().GetCamera())
                return getInstance().GetCamera().fieldOfView;

#if UNITY_EDITOR
            if (Camera.main)
                return Camera.main.fieldOfView;
            return 45;
#else
                return 45;
#endif
        }
        public static float MainCameraFOV
        {
            get { return _GetMainCameraFOV(); }
        }
        //------------------------------------------------------
        static bool _GetMainCameraIsActived() 
        {
#if UNITY_EDITOR
            if (getInstance() != null && !(getInstance() is CameraController)) return false;
#endif
            if (getInstance() != null) return (getInstance() as CameraController).IsEnable();
            return false;
        }
        public static bool MainCameraIsActived
        {
            get { return _GetMainCameraIsActived(); }
        }
        //------------------------------------------------------
        public static ICameraController getInstance()
        {
            return ms_pInstnace;
        }
        //------------------------------------------------------
        protected int m_nCloseCameraRef = 0;
        protected int m_bActiveRef = 1;
        protected int m_nActiveVolumeRef = 1;
        private bool m_bEnable = true;
        private bool m_bClosed = false;

        UnityEngine.Rendering.Universal.UniversalAdditionalCameraData m_BaseURPCameraData = null;

        public CameraController()
        {
            m_bEnable = true;
            m_bActiveRef = 1;
            m_bClosed = false;
            m_vCameraModes = new Dictionary<string, CameraMode>(4);
            m_pCurrentMode = null;
        }
        //------------------------------------------------------
        public void StartUp()
        {
            if (m_pCameraSetting && m_pCameraSetting.URPCamera)
            {
                m_pCameraSetting.URPCamera.enabled = true;
                m_URPCameraTranfrom = m_pCameraSetting.URPCamera.transform;
            }
        }
        //------------------------------------------------------
        public bool IsEnable()
        {
            return m_bEnable && m_bActiveRef>0 && !m_bClosed && m_nCloseCameraRef<=0;
        }
        //------------------------------------------------------
        public bool IsClosed()
        {
            return m_bClosed && m_bActiveRef>0 && m_nCloseCameraRef>0;
        }
        //------------------------------------------------------
        public void Enable(bool bEnable)
        {
            if (m_bEnable == bEnable) return;
            m_bEnable = bEnable;
            if (m_bEnable)
            {
                if (m_nCloseCameraRef > 0 || m_bClosed)
                    CloseCamera(false);
                m_nCloseCameraRef = 0;
                m_bActiveRef = 0;
                m_nActiveVolumeRef = 0;
                ActiveRoot(true);
                ActiveVolume(true);
            }
            if(CameraKit.OnCameraEnable!=null) CameraKit.OnCameraEnable(m_bEnable);
        }
        //------------------------------------------------------
        public void ActiveRoot(bool bActive)
        {
            if (bActive) m_bActiveRef++;
            else
            {
                m_bActiveRef--;
                //if (m_bActiveRef < 0) m_bActiveRef = 0;
            }
            if (m_pCameaRoot)
            {
                m_pCameaRoot.gameObject.SetActive(m_bActiveRef>0);
            }
            if (m_pCameraSetting && m_pCameraSetting.URPCamera) m_pCameraSetting.URPCamera.enabled = m_bActiveRef>0;
        }
        //------------------------------------------------------    
        public void CloseCamera(bool bClose)
        {
            if (m_pCameraSetting == null || m_pCameraSetting.arrCameras == null) return;
            int lenth = m_pCameraSetting.arrCameras.Length;
            for (int i = 0; i < lenth; ++i)
            {
                Camera camera = m_pCameraSetting.arrCameras[i];
                if (camera == null) continue;
                if(m_pCameraSetting.URPCamera != null)
                {
                    camera.enabled = !bClose;
                }
                else
                {
                    camera.cullingMask = bClose ? 0 : m_arCameraCullingMask[i];
                }
            }
            m_bClosed = bClose;
            if (!bClose)
                m_nCloseCameraRef = 0;
        }
        //------------------------------------------------------
        public void CloseCameraRef(bool bClose)
        {
            if (m_pCameraSetting == null || m_pCameraSetting.arrCameras == null) return;
            if (bClose) m_nCloseCameraRef++;
            else m_nCloseCameraRef--;
            if(m_nCloseCameraRef>0)
            {
                int lenth = m_pCameraSetting.arrCameras.Length;
                for (int i = 0; i < lenth; ++i)
                {
                    Camera camera = m_pCameraSetting.arrCameras[i];
                    if (camera == null) continue;
                    if (m_pCameraSetting.URPCamera != null)
                    {
                        camera.enabled = false;
                    }
                    else
                    {
                        if (i == (int)ECameraType.Background)
                        {
                            camera.cullingMask = 0;
                        }
                        else
                            camera.enabled = false;
                    }

                }
            }
            else
            {
                m_nCloseCameraRef = 0;
                int lenth = m_pCameraSetting.arrCameras.Length;
                for (int i = 0; i < (int)ECameraType.Count; ++i)
                {
                    Camera camera = m_pCameraSetting.arrCameras[i];
                    if (camera == null) continue;
                    if (m_pCameraSetting.URPCamera != null)
                    {
                        camera.enabled = !m_bClosed;
                    }
                    else
                    {
                        camera.cullingMask = m_bClosed ? 0 : m_arCameraCullingMask[i];
                    }

                }
            }
        }
        //------------------------------------------------------
        void OnDestroy()
        {
            ms_pInstnace = null;
        }
        //------------------------------------------------------
        public bool Init(GameObject root)
        {
            if (root == null) return false;
            CameraSetting cameraSetting = root.GetComponentInChildren<CameraSetting>();
            if (cameraSetting == null || cameraSetting.arrCameras == null) return false;
            ms_pInstnace = this;
            root.name = "CameraSystem";
            m_pCameraSetting = cameraSetting;
            m_pCameaRoot = cameraSetting.transform;
            GameObject.DontDestroyOnLoad(root);
            m_vCameraEffect = new List<ACameraEffect>(4);
            m_pAnimPathEffector = null;
            int cameraCnt = cameraSetting.arrCameras.Length;
            for (int i = 0; i < cameraCnt; ++i)
            {
                if (cameraSetting.arrCameras[i]) m_arCameraCullingMask[i] = cameraSetting.arrCameras[i].cullingMask;
            }

            CameraKit.cameraController = this;
            CameraKit.getMainCamera = _GetMainCamera;
            CameraKit.getMainCameraCulling = _GetMainCameraCulling;
            CameraKit.getMainCameraPosition = _GetMainCameraPosition;
            CameraKit.getMainCameraEulerAngle = _GetMainCameraEulerAngle;
            CameraKit.getMainCameraDirection = _GetMainCameraDirection;
            CameraKit.getMainCameraLookat = _GetMainCameraLookAt;
            CameraKit.getMainCameraFov = _GetMainCameraFOV;
            CameraKit.getMainCameraIsActived = _GetMainCameraIsActived;

            DyncmicTransformCollects.Collect("CameraRoot", m_pCameaRoot);

            m_vPostEffect = new List<APostEffect>();
            APostEffect[] effects =  root.GetComponentsInChildren<APostEffect>();
            for(int i= 0; i < effects.Length; ++i)
            {
                m_vPostEffect.Add(effects[i]);
            }

            m_nCloseCameraRef = 0;

            if (cameraSetting.URPCamera)
            {
#if UNITY_EDITOR
                if (UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline == null)
                    UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline = UnityEditor.AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>("Assets/DatasRef/Config/RenderURP/Default/UniversalRenderPipelineAsset.asset");
#endif
                SetURPCamera(cameraSetting.URPCamera.GetUniversalAdditionalCameraData());
                cameraSetting.URPCamera.clearFlags = CameraClearFlags.SolidColor;
                cameraSetting.URPCamera.backgroundColor = Color.black;
                m_URPCameraTranfrom = cameraSetting.URPCamera.transform;
            }
#if UNITY_EDITOR
            else
            {
                UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline = null;
            }
#endif

            return true;
        }
        //------------------------------------------------------
        public Transform GetTransform()
        {
            return m_pCameaRoot;
        }
        //------------------------------------------------------
        public UnityEngine.Rendering.Universal.UniversalAdditionalCameraData GetURPCamera()
        {
            return m_BaseURPCameraData;
        }
        //------------------------------------------------------
        public void SetCameraClearFlag(CameraClearFlags flags, Color color)
        {
            if (m_pCameraSetting == null) return;
            if(m_pCameraSetting.URPCamera)
            {
                m_pCameraSetting.URPCamera.clearFlags = flags;
                m_pCameraSetting.URPCamera.backgroundColor = color;
            }
            else
            {
                Camera[] arrCameras = m_pCameraSetting.arrCameras;
                if(arrCameras!=null)
                {
                    for (int i = 0; i < arrCameras.Length; ++i)
                    {
                        if (arrCameras[i])
                        {
                            arrCameras[i].clearFlags = flags;
                            arrCameras[i].backgroundColor = color;
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        public void SetCameraNear(float fNear)
        {
            if (m_pCameraSetting.URPCamera)
            {
                m_pCameraSetting.URPCamera.nearClipPlane = fNear;
            }
            Camera[] arrCameras = m_pCameraSetting.arrCameras;
            if (arrCameras != null)
            {
                for (int i = 0; i < arrCameras.Length; ++i)
                {
                    if (arrCameras[i])
                    {
                        arrCameras[i].nearClipPlane = fNear;
                    }
                }
            }
        }
        //------------------------------------------------------
        public void SetCameraFar(float fFar)
        {
            if (m_pCameraSetting.URPCamera)
            {
                m_pCameraSetting.URPCamera.farClipPlane = fFar;
            }
            Camera[] arrCameras = m_pCameraSetting.arrCameras;
            if (arrCameras != null)
            {
                for (int i = 0; i < arrCameras.Length; ++i)
                {
                    if (arrCameras[i])
                    {
                        arrCameras[i].farClipPlane = fFar;
                    }
                }
            }
        }
        //------------------------------------------------------
        public UnityEngine.Rendering.Volume GetPostProcessVolume()
        {
            if (m_pCameraSetting == null) return null;
            return m_pCameraSetting.postProcessVolume;
        }
        //------------------------------------------------------
        public void SetPostProcess(UnityEngine.Rendering.VolumeProfile profiler)
        {
            if (m_pCameraSetting == null) return;
            if (m_pCameraSetting.postProcessVolume && profiler) m_pCameraSetting.postProcessVolume.sharedProfile = profiler;
        }
        //------------------------------------------------------
        public void ActiveVolume(bool bActive)
        {
            if (bActive) m_nActiveVolumeRef++;
            else m_nActiveVolumeRef--;
            if (m_nActiveVolumeRef < 0) m_nActiveVolumeRef = 0;
            UpdateActiveVolume();
        }
        //------------------------------------------------------
        public bool IsActiveVolume()
        {
            if (m_pCameraSetting == null) return false;
            return m_nActiveVolumeRef > 0;
        }
        //------------------------------------------------------
        public void UpdateActiveVolume()
        {
            if (m_pCameraSetting == null) return;
            if (m_pCameraSetting.postProcessVolume) m_pCameraSetting.postProcessVolume.enabled = m_nActiveVolumeRef > 0;
        }
        //------------------------------------------------------
        public void SetURPAsset(UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset)
        {
            if (m_pCameraSetting == null) return;
            if (m_pCameraSetting.URPCamera)
            {
                if(urpAsset!=null)
                {
                    QualitySettings.renderPipeline = urpAsset;
                    UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset = urpAsset;
                    Camera camera = m_pCameraSetting.GetCamera(ECameraType.Force);
                    if (camera && camera.GetUniversalAdditionalCameraData())
                        camera.GetUniversalAdditionalCameraData().renderShadows = urpAsset.supportsMainLightShadows;
                }
            }
        }
        //------------------------------------------------------
        public void SetURPCamera(UnityEngine.Rendering.Universal.UniversalAdditionalCameraData camera)
        {
            m_BaseURPCameraData = camera;
        }
        //------------------------------------------------------
        public void SwapCamera(Camera pCamera0, Camera pCamera1)
        {
            if (m_BaseURPCameraData == null || pCamera1 == null || pCamera0 == null || pCamera1 == pCamera0) return;
            List<Camera> vStacks = m_BaseURPCameraData.cameraStack;
            int camera0 = vStacks.IndexOf(pCamera0);
            int camera1 = vStacks.IndexOf(pCamera1);
            if (camera0 == camera1) return;
            vStacks[camera1] = pCamera0;
            vStacks[camera0] = pCamera1;
        }
        //------------------------------------------------------
        public void AddCameraStack(Camera pCamera, ECameraType type = ECameraType.Count, bool bAfter = true)
        {
            if (m_BaseURPCameraData == null) return;

            UnityEngine.Rendering.Universal.UniversalAdditionalCameraData cameraData = pCamera.GetUniversalAdditionalCameraData();
            if(cameraData!=null)
            {
                if (cameraData.renderType != CameraRenderType.Overlay)
                    cameraData.renderType = CameraRenderType.Overlay;
            }

            List<Camera> vStacks = m_BaseURPCameraData.cameraStack;
            if (vStacks.Contains(pCamera)) return;
            if (type == ECameraType.Count)
            {
                vStacks.Add(pCamera);
            }
            else if(m_pCameraSetting!=null)
            {
                int stackCnt = vStacks.Count;
                Camera camera = m_pCameraSetting.GetCamera(type);
                for (int i = 0; i < stackCnt; ++i)
                {
                    if (vStacks[i] == camera)
                    {
                        if (bAfter)vStacks.Insert(i + 1, pCamera);
                        else vStacks.Insert(0, pCamera);
                        break;
                    }
                }
            }
        }
        //------------------------------------------------------
        public void RemoveCameraStack(Camera pCamera)
        {
            if (m_BaseURPCameraData == null) return;
            m_BaseURPCameraData.cameraStack.Remove(pCamera);
        }
        //------------------------------------------------------
        public void SetCamera(ECameraType eType, Camera pCamera)
        {
            if (m_pCameraSetting == null) return;
            m_pCameraSetting.SetCamera(eType, pCamera);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取主相机")]
        public Camera GetCamera()
        {
            if (m_pAnimPathEffector != null && m_pAnimPathEffector.IsPlaying())
            {
                Camera pCamera = m_pAnimPathEffector.GetCamera();
                if (pCamera != null) return pCamera;
            }
            if (!IsEnable()) return null;
            return GetCamera(ECameraType.Force);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取相机")]
        public Camera GetCamera( ECameraType eType = ECameraType.Force )
        {
            if (m_pCameraSetting == null) return null;
            return m_pCameraSetting.GetCamera(eType);
        }
        //------------------------------------------------------
        public  void SetCameraCullingMask(ECameraType type, int cullingMask)
        {
            Camera camera = GetCamera(type);
            if (camera)
                camera.cullingMask = cullingMask;
        }
        //------------------------------------------------------
        public void RestoreCameraCullingMask(ECameraType type = ECameraType.Count)
        {
            if (m_pCameraSetting == null) return;
            if(type == ECameraType.Count)
            {
                if(m_pCameraSetting.arrCameras!=null)
                {
                    int cameraLenth = m_pCameraSetting.arrCameras.Length;
                    for (int i = 0; i < cameraLenth; ++i)
                    {
                        if (m_pCameraSetting.arrCameras[i]) m_pCameraSetting.arrCameras[i].cullingMask = m_arCameraCullingMask[i];
                    }
                }
                return;
            }
            Camera camera = GetCamera(type);
            if (camera) camera.cullingMask = m_arCameraCullingMask[(int)type];
        }
        //------------------------------------------------------
        public int GetCameraCullingMask(ECameraType type)
        {
            Camera camera = GetCamera(type);
            if (camera) return camera.cullingMask;
            return 0;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("设置相机广角")]
        public void UpdateFov(float fFov)
        {
            if (m_pCameraSetting == null || m_pCameraSetting.arrCameras == null) return;
            for (int i = 0; i < m_pCameraSetting.arrCameras.Length; ++i)
            {
                if(m_pCameraSetting.arrCameras[i] == null) continue;
                m_pCameraSetting.arrCameras[i].fieldOfView = fFov;
            }
            if (m_pCameraSetting.URPCamera) m_pCameraSetting.URPCamera.fieldOfView = fFov;
        }
        //------------------------------------------------------
        public void RegisterCameraMode(string strMode, CameraMode pMode)
        {
            pMode.SetController(this);
            if (m_vCameraModes.ContainsKey(strMode)) return;
            m_vCameraModes.Add(strMode, pMode);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("切换模式")]
        public CameraMode SwitchMode(string strMode, bool bEnd = true)
        {
            CameraMode pMode;
            if(m_vCameraModes.TryGetValue(strMode, out pMode))
            {
                if (m_pCurrentMode == pMode)
                {
                    return m_pCurrentMode;
                }
                if (bEnd && m_pCurrentMode != null) m_pCurrentMode.End();
                m_pCurrentMode = pMode;
                m_pCurrentMode.Start();
            }
            return m_pCurrentMode;
        }
        //------------------------------------------------------
        public void SwitchMode(CameraMode pMode, bool bEnd = true)
        {
            if (m_pCurrentMode == pMode)
            {
                return;
            }
            if (bEnd && m_pCurrentMode != null) m_pCurrentMode.End();
            m_pCurrentMode = pMode;
            m_pCurrentMode.Start();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取当前模式")]
        public CameraMode GetCurrentMode()
        {
            return m_pCurrentMode;
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
        public Ray ScreenPointToRay(Vector3 mousePosition)
        {
            Camera camera = GetCamera();
            if (camera == null) return new Ray();
            return camera.ScreenPointToRay(mousePosition);
        }
        //------------------------------------------------------
        public float GetFov()
        {
            if (m_pCameraSetting == null) return 45;
            if (m_pCameraSetting.URPCamera) return m_pCameraSetting.URPCamera.fieldOfView;
            Camera camera = GetCamera();
            if (camera == null) return 45;
            return camera.fieldOfView;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("GetDir")]
        public Vector3 GetDir()
        {
            if (m_pAnimPathEffector != null && m_pAnimPathEffector.IsPlaying())
            {
                if (m_pAnimPathEffector.GetCamera() != null)
                    return m_pAnimPathEffector.GetCamera().transform.forward;
            }
            if (m_pCameaRoot == null)
                return Vector3.forward;
            return m_pCameaRoot.forward;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("GetRight")]
        public Vector3 GetRight()
        {
            if (m_pAnimPathEffector != null && m_pAnimPathEffector.IsPlaying())
            {
                if (m_pAnimPathEffector.GetCamera() != null)
                    return m_pAnimPathEffector.GetCamera().transform.right;
            }
            if (m_pCameaRoot == null)
                return Vector3.right;
            return m_pCameaRoot.right;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("GetUp")]
        public Vector3 GetUp()
        {
            if (m_pAnimPathEffector != null && m_pAnimPathEffector.IsPlaying())
            {
                if (m_pAnimPathEffector.GetCamera() != null)
                    return m_pAnimPathEffector.GetCamera().transform.up;
            }
            if (m_pCameaRoot == null)
                return Vector3.up;
            return m_pCameaRoot.up;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("GetPosition")]
        public Vector3 GetPosition()
        {
            if (m_pAnimPathEffector != null && m_pAnimPathEffector.IsPlaying())
            {
                if (m_pAnimPathEffector.GetCamera() != null)
                    return m_pAnimPathEffector.GetCamera().transform.position;
            }
            if (m_pCameaRoot == null)
                return Vector3.zero;
            return m_pCameaRoot.position;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("GetEulerAngle")]
        public Vector3 GetEulerAngle()
        {
            if (m_pAnimPathEffector != null && m_pAnimPathEffector.IsPlaying())
            {
                if (m_pAnimPathEffector.GetCamera() != null)
                    return m_pAnimPathEffector.GetCamera().transform.eulerAngles;
            }
            if (m_pCameaRoot == null)
                return Vector3.zero;
            return m_pCameaRoot.eulerAngles;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("GetCurrentLookAt")]
        public Vector3 GetCurrentLookAt()
        {
            if (m_pCurrentMode == null) return Vector3.zero;
            return m_pCurrentMode.GetCurrentLookAt();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("GetCurrentFollowLookAt")]
        public Vector3 GetCurrentFollowLookAt()
        {
            if (m_pCurrentMode == null) return Vector3.zero;
            return m_pCurrentMode.GetFollowLookAtPosition();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("StopAllEffect")]
        public void StopAllEffect()
        {
            for (int i = 0; i < m_vCameraEffect.Count; ++i)
            {
                m_vCameraEffect[i].Stop();
            }
            m_LerpFromToMode.Clear();
            m_pCameraSlot = CameraSlot.DEFAULT;
        }
        //------------------------------------------------------
        public T GetEffect<T>() where T : ACameraEffect, new()
        {
            for(int i = 0; i < m_vCameraEffect.Count; ++i)
            {
                if (m_vCameraEffect[i].GetType() == typeof(T))
                    return (T)m_vCameraEffect[i];
            }
            T newEffect = new T();
            newEffect.Register(this);
            m_vCameraEffect.Add(newEffect);
            if (newEffect is CameraAnimPath)
                m_pAnimPathEffector = newEffect as CameraAnimPath;
            return newEffect;
        }
        //------------------------------------------------------
        public bool IsTweenEffecting(float fFactorError =0.1f)
        {
            for(int i = 0; i < m_vCameraEffect.Count; ++i)
            {
                if (m_vCameraEffect[i] == null) continue;
                if (m_vCameraEffect[i] is CameraShake) continue;
                if (m_vCameraEffect[i].CanDo()) return true;
            }
            if(m_bEnable && m_pCurrentMode!=null)
            {
                if (m_pCurrentMode.IsLockOffsetDistanceLerping(fFactorError)) return true;
                if (!m_pCurrentMode.GetLockFovOffsetLerp().IsArrived(fFactorError)) return true;
                if (!m_pCurrentMode.GetLockEulerAngleOffsetLerp().IsArrived(-1,fFactorError)) return true;
            }
            return false;
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
        [Framework.Plugin.AT.ATMethod("相机震动")]
        public void Shake(float fDuration, Vector3 vIntense, Vector3 vHertz, AnimationCurve damping = null)
        {
            CameraShake cameraShake = GetEffect<CameraShake>();
            cameraShake.Shake(fDuration, vIntense, vHertz, damping);
            cameraShake.Start(ACameraEffect.EType.Offset);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("镜头特写")]
        public void CloseUp(Transform pTarget, Vector3 vDuration, Vector3 vLookat, Vector3 vTranslate, float fFov)
        {
            CameraCloseUp cameraShake = GetEffect<CameraCloseUp>();
            cameraShake.CloseUp(pTarget, vDuration, vLookat, vTranslate, fFov);
            cameraShake.Start(ACameraEffect.EType.Direct);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("播放路径动画")]
        public IPlayableBase PlayAniPath(int nId, GameObject pTrigger = null, bool bAbs = true, float offsetX = 0, float offsetY = 0, float offsetZ = 0)
        {
            CameraAnimPath cameraPath = GetEffect<CameraAnimPath>();
            cameraPath.SetPathID((ushort)nId, bAbs, pTrigger);
            cameraPath.SetTriggerPosition(new Vector3(offsetX, offsetY, offsetZ));
            cameraPath.Start(bAbs?ACameraEffect.EType.LockSet: ACameraEffect.EType.Offset);
            return cameraPath.playable;
        }
        //-------------------------------------------
        public bool IsAnimPathPlaying(int nId)
        {
            if (m_pAnimPathEffector == null) return false;
            if (m_pAnimPathEffector.playable != null && !m_pAnimPathEffector.playable.isOver())
            {
                if(nId==-1 || m_pAnimPathEffector.playable.GetGuid() == nId)
                    return true;
            }
            return false;
        }
        //-------------------------------------------
        [Framework.Plugin.AT.ATMethod("点是否在视野内")]
        public bool IsInView(Vector3 pos, float factor = 0.1f)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && m_bEditor) return true;
#endif
            Camera mainCam = MainCamera;
            if (mainCam == null) mainCam = GetCamera(ECameraType.Force);
            if (mainCam == null) return false;
            Vector2 viewPos = mainCam.WorldToViewportPoint(pos);
            Vector3 dir = (pos - GetPosition()).normalized;
            float dot = Vector3.Dot(GetDir(), dir);
            if (dot > 0 && viewPos.x >= -factor && viewPos.x <= 1 + factor && viewPos.y >= -factor && viewPos.y <= 1 + factor)
                return true;
            return false;
        }
        //-------------------------------------------
        public bool IsInView(Vector3 pos, Rect viewRc)
        {
#if UNITY_EDITOR
            if (m_bEditor) return false;
#endif
            Camera mainCam = MainCamera;
            if (mainCam == null) mainCam = GetCamera(ECameraType.Force);
            if (mainCam == null) return false;
            Vector2 viewPos = mainCam.WorldToViewportPoint(pos);
            Vector3 dir = (pos - GetPosition()).normalized;
            float dot = Vector3.Dot(GetDir(), dir);
            if (dot > 0 && viewPos.x >= viewRc.xMin && viewPos.x <= viewRc.xMax && viewPos.y >= viewRc.yMin && viewPos.y <= viewRc.yMax)
                return true;
            return false;
        }
        //------------------------------------------------------
        public void SetLerpFormToMode(Vector3 transPosition, Vector3 eulerAngle, float fov, float fTime)
        {
            m_LerpFromToMode.duration = fTime;
            m_LerpFromToMode.time = 0;
            m_LerpFromToMode.fov = fov;
            m_LerpFromToMode.tansformPos = transPosition;
            m_LerpFromToMode.eulerAngle = eulerAngle;
            ForceUpdate(0);
        }
        //------------------------------------------------------
        public void SetCameraSlot(ACameraSlots.Slot slot, float fLerpTime = 0.5f)
        {
            if (!slot.IsValid())
            {
                if(m_pCameraSlot.IsValid())
                {
                    m_pCameraSlot.pSlot.position = m_pCameaRoot.position;
                    m_pCameraSlot.pSlot.eulerAngle = m_pCameaRoot.eulerAngles;
                    m_pCameraSlot.pSlot.fov = GetFov();

                    m_pCameraSlot.fTime = 0;
                    m_pCameraSlot.fDuration = fLerpTime;
                    m_pCameraSlot.isExit = true;
                }
                return;
            }
            m_pCameraSlot.isExit = false;
            m_pCameraSlot.fDuration = fLerpTime;
            m_pCameraSlot.fTime = 0;
            m_pCameraSlot.pSlot = slot;
            ForceUpdate(0);
        }
        //------------------------------------------------------
        public void ForceUpdate(float fFrame)
        {
            fFrame = Time.deltaTime;
            if (m_pCameaRoot == null) return;
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
            if(hasDirectEffect)
            {
                m_pCameaRoot.position = m_EffectPos;
                m_pCameaRoot.eulerAngles = m_EffectEulerAngle;
                UpdateFov(m_EffectFov);

                if (m_URPCameraTranfrom)
                {
                    m_URPCameraTranfrom.position = m_pCameaRoot.position;
                    m_URPCameraTranfrom.eulerAngles = m_pCameaRoot.eulerAngles;
                }
#if UNITY_EDITOR
                m_EditorPosition = m_pCameaRoot.position;
#endif
                return;
            }
#if UNITY_EDITOR
            if (m_bEditor)
            {
                if (m_pCameaRoot == null) return;
                if(Event.current!=null && (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown))
                {

                }
                else
                {
                    if (Input.GetMouseButton(1))
                    {
                        m_fYaw += Input.GetAxis("Mouse Y") * m_rotateSpeed;
                        m_fPitch += Input.GetAxis("Mouse X") * m_rotateSpeed;
                    }
                    m_pCameaRoot.eulerAngles = new Vector3(Framework.Core.CommonUtility.ClampAngle(-m_fYaw), Framework.Core.CommonUtility.ClampAngle(m_fPitch), m_pCameaRoot.eulerAngles.z) + m_EffectEulerAngle;
                    if (Input.GetKey(KeyCode.W))
                    {
                        m_EditorPosition += m_pCameaRoot.forward * fFrame * m_dragSpeed;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        m_EditorPosition -= m_pCameaRoot.forward * fFrame * m_dragSpeed;
                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        m_EditorPosition -= m_pCameaRoot.right * fFrame * m_dragSpeed;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        m_EditorPosition += m_pCameaRoot.right * fFrame * m_dragSpeed;
                    }

                    if (Input.GetMouseButton(2))
                    {
                        m_EditorPosition -= m_pCameaRoot.up * Input.GetAxis("Mouse Y") * 0.5f * m_dragSpeed;
                    }
                    if (Input.GetMouseButton(2))
                    {
                        m_EditorPosition -= m_pCameaRoot.right * Input.GetAxis("Mouse X") * 0.5f * m_dragSpeed;
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") != 0)
                    {
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                            m_EditorPosition += m_pCameaRoot.forward * Input.GetAxis("Mouse ScrollWheel") * 2 * m_zoomSpeed;
                        else
                            m_EditorPosition += m_pCameaRoot.forward * Input.GetAxis("Mouse ScrollWheel") * m_zoomSpeed;
                    }

                    if (IsEnable())
                    {
                        m_pCameaRoot.position = m_EditorPosition + m_EffectPos;
                        if (m_pCurrentMode != null)
                            UpdateFov(m_pCurrentMode.GetCurrentFov() + m_EffectFov);
                    }
                }
                
                return;
            }
#endif

            if (IsEnable())
            {
                if(m_pCurrentMode!=null)
                {
             //       Framework.Core.CommonUtility.AjustUpdatePostitionFrame(ref fFrame);
                    m_pCurrentMode.Update(fFrame);
                    if(m_LerpFromToMode.IsValid())
                    {
                        m_LerpFromToMode.time += fFrame;
                        float fFactor = Mathf.Clamp01(m_LerpFromToMode.time / m_LerpFromToMode.duration);
                        m_pCameaRoot.position = Vector3.Lerp(m_LerpFromToMode.tansformPos, m_pCurrentMode.GetCurrentTrans() + m_EffectPos, fFactor) ;
                        m_pCameaRoot.rotation = Quaternion.Slerp(Quaternion.Euler(m_LerpFromToMode.eulerAngle), Quaternion.Euler(m_pCurrentMode.GetCurrentEulerAngle() + m_EffectEulerAngle), fFactor);
                        UpdateFov(Mathf.Lerp(m_LerpFromToMode.fov, m_pCurrentMode.GetCurrentFov() + m_EffectFov, fFactor));
                    }
                    else if(m_pCameraSlot.IsValid())
                    {
                        m_pCameraSlot.fTime += fFrame;
                        if(m_pCameraSlot.isExit)
                        {
                            float fFactor = Mathf.Clamp01(m_pCameraSlot.fTime / m_pCameraSlot.fDuration);
                            m_pCameaRoot.position = Vector3.Lerp(m_pCameraSlot.pSlot.position, m_pCurrentMode.GetCurrentTrans() + m_EffectPos, fFactor);
                            m_pCameaRoot.rotation = Quaternion.Slerp(Quaternion.Euler(m_pCameraSlot.pSlot.eulerAngle), Quaternion.Euler(m_pCurrentMode.GetCurrentEulerAngle() + m_EffectEulerAngle), fFactor);
                            UpdateFov(Mathf.Lerp(m_pCameraSlot.pSlot.fov, m_pCurrentMode.GetCurrentFov() + m_EffectFov, fFactor));
                            if(fFactor>=1)
                            {
                                m_pCameraSlot.Clear();
                            }
                        }
                        else
                        {
                            float fFactor = 1 - Mathf.Clamp01(m_pCameraSlot.fTime / m_pCameraSlot.fDuration);
                            m_pCameaRoot.position = Vector3.Lerp(m_pCameraSlot.pSlot.position, m_pCurrentMode.GetCurrentTrans() + m_EffectPos, fFactor);
                            m_pCameaRoot.rotation = Quaternion.Slerp(Quaternion.Euler(m_pCameraSlot.pSlot.eulerAngle), Quaternion.Euler(m_pCurrentMode.GetCurrentEulerAngle() + m_EffectEulerAngle), fFactor);
                            UpdateFov(Mathf.Lerp(m_pCameraSlot.pSlot.fov, m_pCurrentMode.GetCurrentFov() + m_EffectFov, fFactor));
                        }
                    }
                    else
                    {
                        m_pCameaRoot.position = m_pCurrentMode.GetCurrentTrans() + m_EffectPos;
                        m_pCameaRoot.eulerAngles = m_pCurrentMode.GetCurrentEulerAngle() + m_EffectEulerAngle;
                        UpdateFov(m_pCurrentMode.GetCurrentFov() + m_EffectFov);

                    }

                    if (m_URPCameraTranfrom)
                    {
                        m_URPCameraTranfrom.position = m_pCameaRoot.position;
                        m_URPCameraTranfrom.eulerAngles = m_pCameaRoot.eulerAngles;
                    }
                }
            }
#if UNITY_EDITOR
            m_EditorPosition = m_pCameaRoot.position;
#endif
        }
        //------------------------------------------------------
        public void OnEffectPosition(Vector3 pos)
        {
            m_EffectPos += pos;
        }
        //------------------------------------------------------
        public void OnEffectEulerAngle(Vector3 pos)
        {
            m_EffectEulerAngle += pos;
        }
        //------------------------------------------------------
        public void OnEffectLookAt(Vector3 pos)
        {
            m_EffectLookAt += pos;
        }
        //------------------------------------------------------
        public void OnEffectFov(float fov)
        {
            m_EffectFov += fov;
        }
        //------------------------------------------------------
        public void OnFollowPosition(Vector3 pos)
        {

        }
        //-------------------------------------------
        public Texture CaptureScreenTexture(int sw = -1, int sh = -1, RenderTextureFormat format = RenderTextureFormat.ARGB32)
        {
            if (m_pCameraSetting == null || m_pCameraSetting.arrCameras == null) return null;
            if (sw < 0) sw = Screen.width;
            if (sh < 0) sh = Screen.height;
            if (!SystemInfo.SupportsRenderTextureFormat(format))
                format = RenderTextureFormat.ARGB32;
            RenderTexture RT = RenderTexture.GetTemporary(sw, sh, 24, format);
            if (m_pCaptureScreenTexture != null && (m_pCaptureScreenTexture.width != sw || m_pCaptureScreenTexture.height != sh))
            {
                UnityEngine.Object.Destroy(m_pCaptureScreenTexture);
                m_pCaptureScreenTexture = null;
            }
            if (m_pCaptureScreenTexture == null)
            {
                m_pCaptureScreenTexture = new Texture2D(sw, sh, TextureFormat.ARGB32, false);
            }

            int cameraLen = m_pCameraSetting.arrCameras.Length;
            for (int i = 0; i < cameraLen; ++i)
            {
                Camera camera = m_pCameraSetting.arrCameras[i];
                if (camera == null) continue;
                RenderTexture rt1 = camera.targetTexture;

                camera.targetTexture = RT;
                camera.Render();
                camera.targetTexture = rt1;
            }

            RenderTexture rt = RenderTexture.active;
            RenderTexture.active = RT;
            m_pCaptureScreenTexture.ReadPixels(new Rect(0, 0, RT.width, RT.height), 0, 0);
            m_pCaptureScreenTexture.Apply();
            RenderTexture.active = rt;

            RenderTexture.ReleaseTemporary(RT);

            return m_pCaptureScreenTexture;
        }
    }
}