/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ColorProperty
作    者:	HappLI
描    述:	颜色过渡参数
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core
{
    public class ActionStatePropertyLerp : BasePropertyLerp
    {
        private static Stack<ActionStatePropertyLerp> ms_Pools = new Stack<ActionStatePropertyLerp>(16);
        public static ActionStatePropertyLerp Malloc()
        {
            if (ms_Pools.Count > 0) return ms_Pools.Pop();
            return new ActionStatePropertyLerp();
        }
        public static void Free(ActionStatePropertyLerp prop)
        {
            prop.Clear();
            if(ms_Pools.Count< 16) ms_Pools.Push(prop);
        }

        SplineTracker m_pSplineTracker = null;
        //------------------------------------------------------
        public bool SetFrames(SplineTracker.KeyFrame[] frames)
        {
            if (m_pSplineTracker == null) m_pSplineTracker = new SplineTracker();
            return m_pSplineTracker.SetFrames(frames);
        }
        //------------------------------------------------------
        public override bool Update(float fFrameTime)
        {
            if (binderObject == null || m_pSplineTracker == null) return false;
            fLerp += fFrameTime;
            InnerUpdate();
            if (fLerp >= fDuration)
            {
                Clear();
                return false;
            }
            return true;
        }
        //------------------------------------------------------
        protected override void InnerUpdate()
        {

        }
        //------------------------------------------------------
        public override void Destroy()
        {
            if (m_pSplineTracker != null) m_pSplineTracker.Clear();
            base.Destroy();
            Free(this);
        }
    }
}

