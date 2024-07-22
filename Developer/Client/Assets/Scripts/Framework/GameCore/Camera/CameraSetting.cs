/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	CameraController
作    者:	HappLI
描    述:	相机控制
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Framework.Core;

namespace TopGame.Core
{
    public class CameraSetting : MonoBehaviour
    {
        public Volume postProcessVolume;
        public Camera URPCamera = null;
        [SerializeField]
        private Camera[] m_arCamera = new Camera[(int)ECameraType.Count];

        public Camera[] arrCameras
        {
            get { return m_arCamera; }
        }
        public Camera GetCamera(ECameraType type)
        {
            int index = (int)type;
            if (index < 0 || m_arCamera == null || index >= m_arCamera.Length) return null;
            return m_arCamera[index];
        }
        public void SetCamera(ECameraType type, Camera camera)
        {
            int index = (int)type;
            if (index < 0 || m_arCamera == null || index >= m_arCamera.Length) return;
            m_arCamera[index] = camera;
        }
    }
}