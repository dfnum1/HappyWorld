/********************************************************************
生成日期:	7:10:2020 18:16
类    名: 	CountDownTimer
作    者:	JaydenHe
描    述:	倒计时
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TopGame.Base;
using Framework.Core;

namespace TopGame.UI
{
    [UIWidgetExport]
    public class CountDownTimer : MonoBehaviour
    {
        public Text UIText;
        public Action OnTimerEnd;
        private float m_endTime;
        private string m_prefix;
        private string m_after;
        string m_final;
        private bool m_IsEndActionFinish = false;
        public bool IsDayFormat = false;
        private bool m_IsStrFormat = false;
        private uint m_key = 0;
        private string m_StrFormat = null;

        private Action<VariablePoolAble> m_UpdateAction = null;
        private VariablePoolAble m_Data = null;

        private Slider m_pSlider = null;
        private Image m_pImage = null;
        private int m_cdTime = 0;
        public void InitSlider(Slider slider = null,Image image = null, int cdTime = 0)
        {
            m_pSlider = slider;
            m_cdTime = cdTime;
            m_pImage = image;
        }

        private void SetSliderValue()
        {
            if (m_pSlider == null) return;
            float value = (m_cdTime - m_endTime) / (float)m_cdTime;
            value = Mathf.Clamp(value, 0, 1);
            m_pSlider.value = value;
        }

        private void SetImageValue()
        {
            if (m_pImage == null) return;
            float value = (m_cdTime - m_endTime) / (float)m_cdTime;
            value = Mathf.Clamp(value, 0, 1);
            m_pImage.fillAmount = value;
        }

        public bool IsTimeEnd()
        {
            return m_endTime <= 0;
        }

        public void SetEndTime(long time,string prefix = "",string after = "")
        {
            if (UIText)
            {
                UIText.gameObject.SetActive(true);
                UIText.text = prefix + " 00:00:00" + after;
            }
            
            m_endTime = time;
            m_prefix = prefix;
            m_after = after;
            m_IsEndActionFinish = false;
            SetSliderValue();
            SetImageValue();
        }

        public void Stop()
        {
            m_endTime = -1;
            m_IsEndActionFinish = true;
            OnTimerEnd = null;
            m_IsStrFormat = false;
            IsDayFormat = false;
            m_StrFormat = null;
            m_UpdateAction = null;
            m_Data = null;
        }
        //------------------------------------------------------
        public void SetStrFormat(uint key)
        {
            m_key = key;
            m_IsStrFormat = true;
            m_StrFormat = Core.ALocalizationManager.ToLocalization(m_key);
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(m_StrFormat) || m_StrFormat.Contains("{0}") == false)
            {
                Debug.LogError("多语言id格式有问题,请检查配置 id:" + key);
            }
#endif
        }
        //------------------------------------------------------
        public void SetUpdateDate(VariablePoolAble data,Action<VariablePoolAble> updateAction)
        {
            m_UpdateAction = updateAction;
            m_Data = data;
        }

        void Update()
        {
            if (UIText == null) return;
            if (m_endTime == -1) return;

            if (m_endTime <= 0 && !m_IsEndActionFinish)
            {
                if (UIText.gameObject.activeSelf)
                {
                    UIText.gameObject.SetActive(false);
                    OnTimerEnd?.Invoke();
                    m_IsEndActionFinish = true;
                } 
                return;
            }

            m_endTime -= Time.deltaTime;
            m_final = IsDayFormat ? Base.TimerUtil.GetTimeForStringDayFormat((long)m_endTime): Base.TimerUtil.GetTimeForString((int)m_endTime);
            if (m_IsStrFormat && m_StrFormat.Contains("{0}"))
            {
                m_final = string.Format(m_StrFormat, m_final);
            }
            UIText.text = Framework.Core.BaseUtil.stringBuilder.Append(m_prefix).Append(m_final).Append(m_after).ToString();
            SetSliderValue();
            SetImageValue();

            if (m_UpdateAction != null)
            {
                m_UpdateAction.Invoke(m_Data);
            }
        }
    }
}