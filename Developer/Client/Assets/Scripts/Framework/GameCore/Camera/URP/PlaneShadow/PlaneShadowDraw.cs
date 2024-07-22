/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	APostRenderPass
作    者:	HappLI
描    述:	URP
*********************************************************************/
using Framework.URP;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TopGame.URP
{
    public class PlaneShadowDraw : ARenderObjects
    {
        public override EPostPassType GetPostPassType()
        {
            return EPostPassType.PlaneShadow;
        }
    }
}
