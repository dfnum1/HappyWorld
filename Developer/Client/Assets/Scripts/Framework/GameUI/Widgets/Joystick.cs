/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	Joystick
作    者:	HappLI
描    述:	摇杆控件
*********************************************************************/
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace TopGame.UI
{
    [UIWidgetExport]
    public class Joystick : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [System.Serializable]
        public class JoystickEvent : UnityEvent<Vector3> { }

        public Transform        content;

        [System.NonSerialized] public System.Action       beginControl;
        [System.NonSerialized] public System.Action<Vector3> controlling;
        [System.NonSerialized] public System.Action<PointerEventData> endControl;

        //------------------------------------------------------
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(this.beginControl!=null)
                this.beginControl();
        }
        //------------------------------------------------------
        public void OnDrag(PointerEventData eventData)
        {
            if (this.controlling!=null && this.content)
            {
                this.controlling(this.content.localPosition.normalized);
            }
        }
        //------------------------------------------------------
        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.endControl != null) this.endControl(eventData);
        }
    }
}
