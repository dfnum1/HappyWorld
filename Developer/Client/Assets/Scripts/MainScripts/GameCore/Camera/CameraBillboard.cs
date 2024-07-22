using System.Collections.Generic;
using Framework.Base;
using Framework.Core;
using Framework.Module;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TopGame.Core
{
    public class CameraBillboard : MonoBehaviour
    {
        Transform m_pTransform;
        public Vector3 offsetEuler;
        public float cameraDistance;
        private void Awake()
        {
            m_pTransform = transform;
        }
        private void LateUpdate()
        {
            if (Framework.Module.ModuleManager.startUpGame)
            {
                m_pTransform.eulerAngles = CameraController.MainCameraEulerAngle;
                m_pTransform.localEulerAngles =  offsetEuler;
                m_pTransform.position = CameraController.MainCameraPoition + CameraController.MainCameraDirection * cameraDistance;
            }
        }
    }
}