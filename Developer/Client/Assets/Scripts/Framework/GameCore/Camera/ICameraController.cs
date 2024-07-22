/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	CameraController
作    者:	HappLI
描    述:	相机控制
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using TopGame.Post;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TopGame.Core
{
    public interface ICameraController : Framework.Core.IBaseCameraController
    {
        UnityEngine.Rendering.Volume GetPostProcessVolume();
        void SetPostProcess(UnityEngine.Rendering.VolumeProfile profiler);
        void UpdateActiveVolume();
        void SetURPAsset(UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset);
        UnityEngine.Rendering.Universal.UniversalAdditionalCameraData GetURPCamera();
        void SetURPCamera(UnityEngine.Rendering.Universal.UniversalAdditionalCameraData camera);

        T GetEffect<T>() where T : ACameraEffect, new();
        T GetPost<T>() where T : APostEffect, new();

        void SetCameraClearFlag(CameraClearFlags flags, Color color);
        void AddCameraStack(Camera pCamera, ECameraType type = ECameraType.Count, bool bAfter = true);
        void RemoveCameraStack(Camera pCamera);

        void SwapCamera(Camera camera, Camera typeCamera);

        IPlayableBase PlayAniPath(int nId, GameObject pTrigger = null, bool bAbs = true, float offsetX = 0, float offsetY = 0, float offsetZ = 0);
    }
}