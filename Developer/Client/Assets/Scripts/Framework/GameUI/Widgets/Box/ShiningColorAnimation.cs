/********************************************************************
生成日期:	2021-8-11
类    名: 	ShiningColorAnimation
作    者:	jaydenhe
描    述:	霓虹灯闪烁效果
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    public class ShiningColorAnimation : MonoBehaviour
    {
        public Color FromColor = Color.white;
        public Color ToColor = Color.black;
        public float Speed = 1;

        private bool m_IsEnable = true;
        private float m_PlayTimer = 0f;
        private float m_FactorValue = 1;

        public Graphic[] graphics;

        public AnimationCurve ColorCurve;
        //------------------------------------------------------
        void Awake()
        {

        }
        //------------------------------------------------------
        void Update()
        {
            if ( ColorCurve == null || graphics == null) return;

            m_PlayTimer += Time.deltaTime * Speed;
            m_FactorValue = ColorCurve.Evaluate(m_PlayTimer);
            for (int i = 0; i < graphics.Length; ++i)
            {
                if(graphics[i])
                    graphics[i].color = Color.Lerp(FromColor, ToColor, m_FactorValue);
            }
        }
    }
}
