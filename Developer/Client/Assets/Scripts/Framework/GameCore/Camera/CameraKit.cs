/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	CameraUtil
作    者:	HappLI
描    述:	相机工具
*********************************************************************/
using Framework.Core;
using UnityEngine;
namespace TopGame.Core
{
    public class CameraKit
    {
        public static System.Action<bool> OnCameraEnable = null;

        public static ICameraController cameraController;
        public delegate Camera Event_MainCamera();
        public delegate Matrix4x4 Event_MainCameraCulling();
        public delegate Vector3 Event_MainCameraVec3();
        public delegate float Event_MainCameraFOV();
        public delegate bool Event_MainCameraBool();
        public static Event_MainCamera getMainCamera;
        public static Event_MainCameraCulling getMainCameraCulling;
        public static Event_MainCameraVec3 getMainCameraPosition;
        public static Event_MainCameraVec3 getMainCameraEulerAngle;
        public static Event_MainCameraVec3 getMainCameraDirection;
        public static Event_MainCameraVec3 getMainCameraLookat;
        public static Event_MainCameraFOV getMainCameraFov;
        public static Event_MainCameraBool getMainCameraIsActived;
        //------------------------------------------------------
        public static bool IsInView(Vector3 position, float factor = 0.1f)
        {
            if (cameraController != null) return cameraController.IsInView(position, factor);
            return false;
        }
        //------------------------------------------------------
        public static Transform GetTransform()
        {
            if (cameraController != null ) return cameraController.GetTransform();
            return null;
        }
        //------------------------------------------------------
        public static CameraMode GetCurrentMode()
        {
            if (cameraController != null) return cameraController.GetCurrentMode();
            return null;
        }
        //------------------------------------------------------
        public static void SetLerpFormToMode(Vector3 transPosition, Vector3 eulerAngle, float fov, float fTime)
        {
            if (cameraController == null) return;
            cameraController.SetLerpFormToMode(transPosition, eulerAngle, fov, fTime);
        }
        //------------------------------------------------------
        public static void SetCameraSlot(ACameraSlots.Slot pSlot, float fTime = 0.5f)
        {
            if (cameraController == null) return;
            cameraController.SetCameraSlot(pSlot, fTime);
        }
        //------------------------------------------------------
        public static void ActiveRoot(bool bActive)
        {
            if (cameraController != null) cameraController.ActiveRoot(bActive);
        }
        //------------------------------------------------------
        public static void CloseCameraRef(bool close)
        {
            if (cameraController != null) cameraController.CloseCameraRef(close);
        }
        //------------------------------------------------------
        public static void Enable(bool bEnable)
        {
            if (cameraController == null) return;
            cameraController.Enable(bEnable);
        }
        //------------------------------------------------------
        public static void ForceUpdate()
        {
            if (cameraController == null) return;
            cameraController.ForceUpdate(0);
        }
        //------------------------------------------------------
        public static void RegisterCameraMode(string strMode, CameraMode pMode)
        {
            if (cameraController == null) return;
            cameraController.RegisterCameraMode(strMode, pMode);
        }
        //------------------------------------------------------
        public static T SwitchMode<T>(string strMode, bool bEnd = true) where T : CameraMode
        {
            if (cameraController == null) return null;
            return cameraController.SwitchMode(strMode, bEnd) as T;
        }
        //------------------------------------------------------
        public static CameraMode GetMode(string strMode)
        {
            if (cameraController == null) return null;
            return cameraController.GetMode(strMode);
        }
        //------------------------------------------------------
        public static T GetEffect<T>() where T : ACameraEffect, new()
        {
            if (cameraController == null) return null;
            return cameraController.GetEffect<T>();
        }
        //------------------------------------------------------
        public static void ActiveVolume(bool bVolumeActive)
        {
            if (cameraController != null) cameraController.ActiveVolume(bVolumeActive);
        }
        //------------------------------------------------------
        public static void UpdateActiveVolume()
        {
            if (cameraController != null) cameraController.UpdateActiveVolume();
        }
        //------------------------------------------------------
        public static bool IsActiveVolume()
        {
            if (cameraController != null) return cameraController.IsActiveVolume();
            return false;
        }
        //------------------------------------------------------
        public static UnityEngine.Rendering.Volume GetPostProcessVolume()
        {
            if (cameraController != null) return cameraController.GetPostProcessVolume();
            return null;
        }
        //------------------------------------------------------
        public static void AddCameraStack(Camera camera, ECameraType cameraType = ECameraType.Count, bool bAfter = true)
        {
            if (cameraController != null) cameraController.AddCameraStack(camera, cameraType, bAfter);
        }
        //------------------------------------------------------
        public static void RemoveCameraStack(Camera camera)
        {
            if (cameraController != null) cameraController.RemoveCameraStack(camera);
        }
        //------------------------------------------------------
        public static void SwapCamera(Camera camera, ECameraType typeCamera)
        {
            if (cameraController != null)
            {
                Camera camera1 = cameraController.GetCamera(typeCamera);
                SwapCamera(camera,camera1);
            }
        }
        //------------------------------------------------------
        public static void RePushCamera(string cameraTag, ECameraType typeCamera, bool bAfter = true)
        {
            if (cameraController != null && Framework.Module.ModuleManager.mainFramework!=null && !string.IsNullOrEmpty(cameraTag))
            {
                var catchObjects = Framework.Module.ModuleManager.mainFramework.baseShareParams.catchEngineObjs;
                catchObjects.Clear();
                var urpCamera = cameraController.GetURPCamera();
                if (urpCamera == null) return;
                var cameraStacks = urpCamera.cameraStack;
                for(int i =0; i < cameraStacks.Count; ++i)
                {
                    if(cameraTag.CompareTo(cameraStacks[i].tag) == 0)
                    {
                        catchObjects.Add(cameraStacks[i]);
                        CameraKit.RemoveCameraStack(cameraStacks[i]);
                    }
                }
                for(int i =0; i < catchObjects.Count; ++i)
                {
                    if (catchObjects[i] == null) continue;
                    Camera tempCamera = catchObjects[i] as Camera;
                    if (tempCamera)
                    {
                        cameraController.AddCameraStack(tempCamera, typeCamera, bAfter);
                    }
                }
                catchObjects.Clear();
            }
        }
        //------------------------------------------------------
        public static void SwapCamera(Camera camera, Camera typeCamera)
        {
            if (cameraController != null && camera && typeCamera)
            {
                cameraController.SwapCamera(camera, typeCamera);
            }
        }
        //------------------------------------------------------
        public static void SetCameraClearFlag(CameraClearFlags flags, Color color)
        {
            if (cameraController == null) return;
            cameraController.SetCameraClearFlag(flags, color);
        }
        //------------------------------------------------------
        public static void SetCameraNear(float fNear)
        {
            if (cameraController == null) return;
            cameraController.SetCameraNear(fNear);
        }
        //------------------------------------------------------
        public static void SetCameraFar(float fFar)
        {
            if (cameraController == null) return;
            cameraController.SetCameraFar(fFar);
        }
        //------------------------------------------------------
        public static void SetPostProcess(UnityEngine.Rendering.VolumeProfile profiler)
        {
            if (cameraController == null) return;
            cameraController.SetPostProcess(profiler);
        }
        //------------------------------------------------------
        public static void SetURPAsset(UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset)
        {
            if (cameraController == null) return;
            cameraController.SetURPAsset(urpAsset);
        }
        //------------------------------------------------------
        public static UnityEngine.Rendering.Universal.UniversalAdditionalCameraData GetURPCamera()
        {
            if (cameraController == null) return null;
            return cameraController.GetURPCamera();
        }
        //------------------------------------------------------
        public static void SetURPCamera(UnityEngine.Rendering.Universal.UniversalAdditionalCameraData camera)
        {
            if (cameraController == null) return;
            cameraController.SetURPCamera(camera);
        }
        //------------------------------------------------------
        public static void StopAllEffect()
        {
            if (cameraController == null) return;
            cameraController.StopAllEffect();
        }
        //------------------------------------------------------
        public static bool ScreenPointToRay(Vector3 screenPos, out Ray ray)
        {
            ray = new Ray(Vector3.zero, Vector3.zero);
            if (cameraController != null)
            {
                ray = cameraController.ScreenPointToRay(screenPos);
                return true;
            }
            return false;
        }
        //------------------------------------------------------
        static public Vector3 ScreenToWorldPos(Vector3 screen)
        {
            Camera camera = MainCamera;
            if (camera == null) return Vector3.zero;
            return camera.ScreenToWorldPoint(screen);
        }
        //------------------------------------------------------
        static public Vector3 ScreenRayWorldPos(Vector3 screen, float floorY = 0)
        {
            Camera camera = MainCamera;
            if (camera == null) return Vector3.zero;
            return CommonUtility.RayHitPos(camera.ScreenPointToRay(screen), floorY);
        }
        //------------------------------------------------------
        public static bool IsEditorMode
        {
            get
            {
#if UNITY_EDITOR
                if (cameraController != null) return cameraController.IsEditorMode();
#endif
                return false;
            }
        }
        //------------------------------------------------------
        public static Camera MainCamera
        {
            get
            {
                if (getMainCamera != null) return getMainCamera();
                return null;
            }
        }
        //------------------------------------------------------
        public static bool MainCameraIsActived
        {
            get
            {
                if (getMainCameraIsActived != null) return getMainCameraIsActived();
                return false;
            }
        }
        //------------------------------------------------------
        public static Matrix4x4 MainCameraCulling
        {
            get
            {
                if (getMainCameraCulling != null) return getMainCameraCulling();
                return Matrix4x4.identity;
            }
        }
        //------------------------------------------------------
        public static Vector3 MainCameraPosition
        {
            get
            {
                if (getMainCameraPosition != null) return getMainCameraPosition();
                return Vector3.zero;
            }
        }
        //------------------------------------------------------
        public static Vector3 MainCameraEulerAngle
        {
            get
            {
                if (getMainCameraEulerAngle != null) return getMainCameraEulerAngle();
                return Vector3.zero;
            }
        }
        //------------------------------------------------------
        public static Vector3 MainCameraDirection
        {
            get
            {
                if (getMainCameraDirection != null) return getMainCameraDirection();
                return Vector3.forward;
            }
        }
        //------------------------------------------------------
        public static Vector3 MainCameraLookAt
        {
            get
            {
                if (getMainCameraLookat != null) return getMainCameraLookat();
                return Vector3.zero;
            }
        }
    }
}