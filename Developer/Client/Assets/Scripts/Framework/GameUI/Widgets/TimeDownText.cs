/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	AddNewItemAni
作    者:	zdq
描    述:	控制物品缩放动画
*********************************************************************/
using Framework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    [UIWidgetExport]
    public class TimeDownText : Text
    {
        private float m_fEndTime = 0;
        int m_nTime = 0;
        System.Action<VariablePoolAble> m_pOnEnd = null;
        VariablePoolAble m_pVar = null;
        bool m_bForamt = true;
        //------------------------------------------------------
        private void LateUpdate()
        {
            if (m_fEndTime <= 0) return;
            float fTime = m_fEndTime - Time.realtimeSinceStartup;
            SetText((int)fTime);
            if (fTime < 0)
            {
                if (m_pOnEnd != null) m_pOnEnd(m_pVar);
                m_pVar = null;
                m_pOnEnd = null;
                 m_fEndTime = 0;
            }
        }
        //------------------------------------------------------
        public void SetParam(float endTime, bool bFormat = true, System.Action<VariablePoolAble> OnEnd = null, VariablePoolAble var = null)
        {
            m_fEndTime = endTime;
            m_pOnEnd = OnEnd;
            m_pVar = var;
            m_bForamt = bFormat;
        }
        //------------------------------------------------------
        void SetText(int time)
        {
            if (time < 0) time = 0;
            if (m_nTime == time) return;
            m_nTime = time;
            if(m_bForamt) this.text = Base.TimerUtil.GetTimeForString(m_nTime);
            else this.text = m_nTime.ToString();
        }
    }
}
