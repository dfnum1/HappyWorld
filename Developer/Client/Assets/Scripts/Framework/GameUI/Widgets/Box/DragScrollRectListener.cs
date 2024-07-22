/********************************************************************
生成日期:	6:3:2020 14:06
类    名: 	MultiSelectBtnBox
作    者:	JaydenHe
描    述:	允许ScrollRect子物体点击拖动
*********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TopGame.UI
{
    public class DragScrollRectListener : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        public ScrollRect DragScorll;

        public System.Action OnScrollBeginRoll;
        public System.Action OnScrollEndRoll;
        private Vector2 m_PreVelocity;
        private bool m_IsBeginDrag = false;
        private bool m_IsEndDrag = false;

        public float m_EndSpeed = 0.1f;
        public void Awake()
        {
            if (DragScorll == null)
            {
                Debug.LogError("滚动窗口不存在");
                return;
            }

            DragScorll.onValueChanged.AddListener(OnValueChange);
        }
        //------------------------------------------------------
        public void OnValueChange(Vector2 pos)
        {
            Vector2 curDelta = DragScorll.velocity - m_PreVelocity;
            if (Mathf.Abs(curDelta.x)  <= m_EndSpeed && Mathf.Abs(curDelta.y) <= m_EndSpeed && m_IsBeginDrag && m_IsEndDrag)
            {
                OnScrollEndRoll?.Invoke();
                m_IsBeginDrag = false;
                return;
            }

            m_PreVelocity = DragScorll.velocity;
        }

        //------------------------------------------------------
        public void OnPointerDown(PointerEventData eventData)
        {
            if (DragScorll != null)
            {
                m_IsBeginDrag = true;
                m_IsEndDrag = false;
                OnScrollBeginRoll?.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_IsEndDrag = true;
        }
        //------------------------------------------------------
    }
}

