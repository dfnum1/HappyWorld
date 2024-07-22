/********************************************************************
生成日期:	6:17:2020 15:16
类    名: 	UISwapEvent
作    者:	JaydenHe
描    述:	UI左右滑动触发事件
*********************************************************************/
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace TopGame.UI
{
    [UIWidgetExport]
    public class UISwapEvent : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [System.NonSerialized]
        public System.Action OnSwapLeft;
        [System.NonSerialized]
        public System.Action OnSwapRight;

        private Vector2 m_vBeginPoint;
        //------------------------------------------------------
        public void OnBeginDrag(PointerEventData eventData)
        {
            m_vBeginPoint = eventData.position;
        }
        //------------------------------------------------------
        public void OnDrag(PointerEventData eventData)
        {
        }
        //------------------------------------------------------
        public void OnEndDrag(PointerEventData eventData)
        {
            bool bLeft = eventData.position.x < m_vBeginPoint.x;
            bool bRight = eventData.position.x > m_vBeginPoint.x ;

            if (bLeft) { OnSwapLeft?.Invoke(); return; }
            if (bRight) {OnSwapRight?.Invoke(); return; }       
        }
    }
}
