/********************************************************************
生成日期:	6:15:2020 10:42
类    名: 	DragableMask
作    者:	JaydenHe
描    述:	
*********************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace TopGame.UI
{
    public class DragableMask : MonoBehaviour, IDragHandler
    {
        bool m_IsHorizontal = false;
        float m_Min = 0;
        float m_Max = 0;
        Vector3 m_tempPos;

        public void Init(bool isHorizontal,float min,float max)
        {
            m_IsHorizontal = isHorizontal;
            m_Min = min;
            m_Max = max;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (m_IsHorizontal)
            {
                m_tempPos.y = transform.localPosition.y;
                m_tempPos.x = Mathf.Clamp(transform.localPosition.x + eventData.delta.x, m_Min, m_Max);
            }
            else
            {
                m_tempPos.x = transform.localPosition.x;
                m_tempPos.y = Mathf.Clamp(transform.localPosition.y + eventData.delta.y, m_Min, m_Max);
            }

            transform.localPosition = m_tempPos;
        }
    }
}
