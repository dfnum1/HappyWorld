/********************************************************************
生成日期:	12:9:2021 10:06
类    名: 	BanerScroll
作    者:	cfh
描    述:	BanerScroll
*********************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TopGame.UI
{
    public class BanerScroll : MonoBehaviour,IPointerUpHandler,IPointerDownHandler,IPointerClickHandler
    {
        private float beginX;
        private bool isScroll = false;
        public System.Action<bool> ScrollAction;
        public System.Action ClickAction;
        private float sensitive = 20f;
        public void OnPointerUp(PointerEventData eventData)
        {
            //向前
            if (beginX - sensitive > eventData.position.x)
            {
                if (ScrollAction != null)
                {
                    ScrollAction.Invoke(true);
                }
                isScroll = true;
            }
            else if (beginX + sensitive < eventData.position.x)
            {
                if (ScrollAction != null)
                {
                    ScrollAction.Invoke(false);
                }
                isScroll = true;
            }
            else
            {
                isScroll = false;
            }
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            beginX = eventData.position.x;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isScroll)
                return;
            ClickAction?.Invoke();
        }
    }
}
