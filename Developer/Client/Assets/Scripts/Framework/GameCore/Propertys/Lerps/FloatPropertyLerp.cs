/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ColorProperty
作    者:	HappLI
描    述:	颜色过渡参数
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
namespace TopGame.Core
{
    public class FloatPropertyLerp : BasePropertyLerp
    {
        private static Stack<FloatPropertyLerp> ms_Pools = new Stack<FloatPropertyLerp>(16);
        public static FloatPropertyLerp Malloc()
        {
            if (ms_Pools.Count > 0) return ms_Pools.Pop();
            return new FloatPropertyLerp();
        }
        public static void Free(FloatPropertyLerp prop)
        {
            prop.Clear();
            if(ms_Pools.Count< 16) ms_Pools.Push(prop);
        }

        public AnimationCurve value;
        //------------------------------------------------------
        protected override void InnerUpdate()
        {
            float cur = 0;
            if (value != null && value.length > 0) cur = value.Evaluate(fLerp);

            MaterailBlockUtil.SetRendersFloat(renders, propertyName, cur, materialBlock, materialShare, materialIndex);
        }
        //------------------------------------------------------
        public override void Destroy()
        {
			base.Destroy();
            Free(this);
        }
    }
}

