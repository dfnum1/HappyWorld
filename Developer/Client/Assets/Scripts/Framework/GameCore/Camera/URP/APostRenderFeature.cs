/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	APostRenderFeature
作    者:	HappLI
描    述:	URP
*********************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TopGame.URP
{
    [System.Serializable]
    public class CustomSettings
    {
        [SerializeField]
        bool bEnabled = true;
        public Framework.Core.ECameraType cameraType;
        public LayerMask layerMask = -1;
        public string passName = "";

        public bool IsOpaque = true;
        public string[] ShaderTags;

        public virtual bool isEnabled()
        {
            return bEnabled;
        }
    }

    [System.Serializable]
    public class PassSettings : CustomSettings
    {
        public RenderPassEvent renderEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }
    public abstract class APostRenderFeature : ScriptableRendererFeature
    {
        protected void OnCreatePass(APostRenderPass renderPass)
        {

        }
    }
}
