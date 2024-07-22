/********************************************************************
生成日期:	4:28:2021 18:16
类    名: 	MaskCountDown
作    者:	JaydenHe
描    述:	遮罩倒计时
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TopGame.Base;

namespace TopGame.UI
{
    [UIWidgetExport]
    public class MaskCountDown : MonoBehaviour
    {
        public Image UIMask;
        public Action OnTimerEnd;
        private float m_leftTime;
        private float m_duration;

        private bool m_IsFinished = true;
        public void SetEndTime(float leftTimeInSec,float duration)
        {
            m_leftTime = leftTimeInSec;
            m_duration = duration;
            m_IsFinished = false;
        }

        void Update()
        {
            if (!UIMask) return;
            if (m_leftTime == -1 || m_IsFinished) return;
            if (m_leftTime > 0)
            {
                m_leftTime -= Time.deltaTime;
                UIMask.fillAmount = m_leftTime / m_duration;
            }
            else
            {
                m_IsFinished = true;
                OnTimerEnd?.Invoke();
            }           
        }
    }
}