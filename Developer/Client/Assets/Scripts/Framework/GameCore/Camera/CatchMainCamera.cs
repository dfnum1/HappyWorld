/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	RunCameraMode
作    者:	HappLI
描    述:	相机catch
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TopGame.Core
{
    public class CatchMainCameraUtil
    {
        public static Camera MainCamera;
        public static Transform MainTransform;
    }

    public class CatchMainCamera : MonoBehaviour
    {
        public Camera mainCamera;
        //------------------------------------------------------
        void Awake()
        {
            CatchMainCameraUtil.MainCamera = mainCamera;
            if (mainCamera)
                CatchMainCameraUtil.MainTransform = mainCamera.transform;
        }
        //------------------------------------------------------
        void OnEnable()
        {
            CatchMainCameraUtil.MainCamera = mainCamera;
            if (mainCamera)
                CatchMainCameraUtil.MainTransform = mainCamera.transform;
        }
        //------------------------------------------------------
        void OnDisable()
        {
            CatchMainCameraUtil.MainCamera = null;
            CatchMainCameraUtil.MainTransform = null;
        }
        //------------------------------------------------------
        void OnDestroy()
        {
            CatchMainCameraUtil.MainCamera = null;
            CatchMainCameraUtil.MainTransform = null;
        }
    }
}