/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ScalePropertyLerp
作    者:	HappLI
描    述:	缩放过渡参数
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
namespace TopGame.Core
{
    public class ScalePropertyLerp : BasePropertyLerp
    {
        private static Stack<ScalePropertyLerp> ms_Pools = new Stack<ScalePropertyLerp>(16);
        public static ScalePropertyLerp Malloc()
        {
            if (ms_Pools.Count > 0) return ms_Pools.Pop();
            return new ScalePropertyLerp();
        }
        public static void Free(ScalePropertyLerp prop)
        {
            prop.Clear();
            if(ms_Pools.Count< 16) ms_Pools.Push(prop);
        }

        public AnimationCurve scale;
        //------------------------------------------------------
        protected override void InnerUpdate()
        {
            if (scale != null && scale.length > 0)
            {
                float scaleFactor = scale.Evaluate(fLerp);
                if (m_pAble)
                    m_pAble.SetScale(Vector3.one * scaleFactor);
            }
        }
        //------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
            Free(this);
        }
    }
}

