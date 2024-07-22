/********************************************************************
生成日期:	6:3:2020 14:06
类    名: 	MultiSelectBtnBox
作    者:	JaydenHe
描    述:	允许ScrollRect子物体点击拖动
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif
namespace TopGame.UI
{
    [DisallowMultipleComponent]
    public class DragScrollRectBox : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public ListView DragListView;
        public ScrollRect DragScorll;
        public void Awake()
        {
            if (DragScorll == null)
                DragScorll = transform.GetComponentInParent<ScrollRect>();
            if(DragListView == null)
                DragListView = transform.GetComponentInParent<ListView>();
        }
        //------------------------------------------------------
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (DragScorll != null)
            {
                DragScorll.OnBeginDrag(eventData);
            }
            if (DragListView)
                DragListView.OnBeginDrag(eventData);
        }
        //------------------------------------------------------
        public void OnDrag(PointerEventData eventData)
        {
            if (DragScorll != null)
            {
                DragScorll.OnDrag(eventData);
            }
            if (DragListView)
                DragListView.OnDrag(eventData);
        }
        //------------------------------------------------------
        public void OnEndDrag(PointerEventData eventData)
        {
            if (DragScorll != null)
            {
                DragScorll.OnEndDrag(eventData);
            }
            if (DragListView)
                DragListView.OnEndDrag(eventData);
        }
        //------------------------------------------------------
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(DragScrollRectBox))]
    [CanEditMultipleObjects]
    public class DragScrollRectBoxEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            DragScrollRectBox evt = target as DragScrollRectBox;
            if (evt.DragScorll == null)
                evt.DragScorll = evt.transform.GetComponentInParent<ScrollRect>();
            if (evt.DragListView == null)
                evt.DragListView = evt.transform.GetComponentInParent<ListView>();
        }
    }
#endif
}

