/********************************************************************
生成日期:	1:29:2021 10:06
类    名: 	ImageFillAmountTween
作    者:	zdq
描    述:	用户imgage组件的fillamount 平滑过渡
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    public class ImageFillAmountTween : MonoBehaviour
    {
        public float Max;
        public float Cur;
        public float Value;
        /// <summary>
        /// 走完一圈的时间,单位秒
        /// </summary>
        public float Speed = 1;

        public Image img;
        bool m_Enable = false;

        public System.Action OnCompletedCallback;
        public System.Action<float,float> OnTweeningCallback;

        bool m_IsTweenAdd = false;

        private void Awake()
        {
            if (img == null)
            {
                img = GetComponent<Image>();
            }
        }
        //------------------------------------------------------
        private void Update()
        {
            if (!m_Enable || img == null || Max <= 0)
            {
                return;
            }
            float value = Max / (Speed / Time.deltaTime);//(一圈总数值) / (一圈几秒/这一帧的时间) = 每帧减少的数值
            if (m_IsTweenAdd)
            {
                Value += value;
                if (Value >= Cur)
                {
                    Value = Cur;
                    OnCompleted();
                }
            }
            else
            {
                Value -= value;
                if (Value <= 0)
                {
                    Value = 0;
                    OnCompleted();
                }
            }
            img.fillAmount = Value / Max;
            OnTweeningCallback?.Invoke(Value, Max);
        }
        //------------------------------------------------------
        public void SetEnable(bool enable)
        {
            m_Enable = enable;
        }
        //------------------------------------------------------
        public void SetTweenMode(bool isAdd)
        {
            m_IsTweenAdd = isAdd;
        }
        //------------------------------------------------------
        public void Clear()
        {
            m_Enable = false;
            m_IsTweenAdd = false;
        }
        //------------------------------------------------------
        public void OnCompleted()
        {
            OnCompletedCallback?.Invoke();
            Clear();
        }
    }
}