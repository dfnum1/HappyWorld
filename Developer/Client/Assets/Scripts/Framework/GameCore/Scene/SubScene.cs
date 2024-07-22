/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	SubScene
作    者:	HappLI
描    述:	场景中的子场景节点
*********************************************************************/
using Framework.Core;
using UnityEngine;
namespace TopGame.Core
{
    public class SubScene : MonoBehaviour
    {
        public float shadowDistance = 0;
        public bool ToggleEnableAndDisable = false;
        public UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset;
        public UnityEngine.Rendering.VolumeProfile volumeProfile;

        private float m_fBackupShadowDistance = -1;
        //------------------------------------------------------
        protected void Awake()
        {
            DyncmicTransformCollects.Collect(name, transform);
        }
        //------------------------------------------------------
        protected void OnDestroy()
        {
            DyncmicTransformCollects.UnCollect(name);
        }
        //------------------------------------------------------
        protected void OnEnable()
        {
            if (ToggleEnableAndDisable) UseProfile();
        }
        //------------------------------------------------------
        protected void OnDisable()
        {
            if (ToggleEnableAndDisable) RestoreProfile();
        }
        //------------------------------------------------------
        public float GetShadowDistance()
        {
            if (shadowDistance > 0) return shadowDistance;
            if(urpAsset!=null) return urpAsset.shadowDistance;
            return -1;
        }
        //------------------------------------------------------
        public void UseProfile()
        {
            if(volumeProfile!=null)
            {
                ICameraController ctl = CameraKit.cameraController;
                if (ctl != null)
                {
                    if(volumeProfile) ctl.SetPostProcess(volumeProfile);
                    if (urpAsset)
                    {
                         m_fBackupShadowDistance = urpAsset.shadowDistance;
                        if (shadowDistance > 0) urpAsset.shadowDistance = shadowDistance;
                        ctl.SetURPAsset(urpAsset);
                    }
                }
            }
        }
        //------------------------------------------------------
        public void RestoreProfile()
        {
            ICameraController ctl = CameraKit.cameraController;
            if (ctl != null)
            {
                if(volumeProfile) ctl.SetPostProcess(Data.GameQuality.volumeProfile);
                if (urpAsset)
                {
                    if (m_fBackupShadowDistance >= 0) urpAsset.shadowDistance = m_fBackupShadowDistance;
                    m_fBackupShadowDistance = -1;
                    ctl.SetURPAsset(Data.GameQuality.urpAsset);
                }
            }
        }
    }
}
