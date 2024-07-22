/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ColorProperty
作    者:	HappLI
描    述:	颜色过渡参数
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
namespace TopGame.Core
{
    public class ColorPropertyLerp : BasePropertyLerp
    {
        private static Stack<ColorPropertyLerp> ms_Pools = new Stack<ColorPropertyLerp>(16);
        public static ColorPropertyLerp Malloc()
        {
            if (ms_Pools.Count > 0) return ms_Pools.Pop();
            return new ColorPropertyLerp();
        }
        public static void Free(ColorPropertyLerp prop)
        {
            prop.Clear();
            if(ms_Pools.Count< 16) ms_Pools.Push(prop);
        }

        public AnimationCurve r;
        public AnimationCurve g;
        public AnimationCurve b;
        public AnimationCurve a;
        //------------------------------------------------------
        protected override void InnerUpdate()
        {
            Color color = Color.white;
            if (r != null && r.length > 0) color.r = r.Evaluate(fLerp);
            if (g != null && g.length > 0) color.g = g.Evaluate(fLerp);
            if (b != null && b.length > 0) color.b = b.Evaluate(fLerp);
            if (a != null && a.length > 0) color.a = a.Evaluate(fLerp);

            MaterailBlockUtil.SetRendersColor(renders, propertyName, color, materialBlock, materialShare, materialIndex);
        }
        //------------------------------------------------------
        public override void Destroy()
        {
			base.Destroy();
            Free(this);
        }
    }
}

