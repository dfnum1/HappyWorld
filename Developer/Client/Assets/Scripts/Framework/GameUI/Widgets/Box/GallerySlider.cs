/********************************************************************
生成日期:	7:10:2020 18:16
类    名: 	GallerySlider
作    者:	JaydenHe
描    述:	滚动图片展
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using System;
using TopGame.Base;

namespace TopGame.UI
{
    [UIWidgetExport]
    public class GallerySlider : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler
    {
        System.Action<int> m_OnShowItem;
        Vector2 m_vBeginPoint;
        public bool m_IsDraging = false;

        public LayoutElement[] m_CachePools;
        int m_TotalSlices; 
        int m_CurrentIdx;
        List<LayoutElement> m_CurrentDotCells = new List<LayoutElement>();
        //------------------------------------------------------
        public void RefreshDotCell(int idx)
        {
            for (int i = 0; i < m_CurrentDotCells.Count; i++)
            {
                m_CurrentDotCells[i].preferredWidth = 25;
                m_CurrentDotCells[i].transform.Find("UnSelect").gameObject.SetActive(true);
                m_CurrentDotCells[i].transform.Find("Select").gameObject.SetActive(false);
            }

            m_CurrentDotCells[idx].preferredWidth = 60;
            m_CurrentDotCells[idx].transform.Find("UnSelect").gameObject.SetActive(false);
            m_CurrentDotCells[idx].transform.Find("Select").gameObject.SetActive(true);
        }
        //------------------------------------------------------
        public void Init(int count, System.Action<int> onShowItem)
        {
            if (count <= 0 || onShowItem == null || m_CachePools.Length <= 0) return;
            m_CurrentDotCells.Clear();
            for (int i=0;i< m_CachePools.Length; i++)
            {
                if (i <= count - 1)
                    m_CurrentDotCells.Add(m_CachePools[i]);
                else
                    m_CachePools[i].gameObject.SetActive(false);
            }

            m_TotalSlices = count;
            m_CurrentIdx = 0;
            m_OnShowItem = onShowItem;

            DoChangePage(0);
        }
        //------------------------------------------------------
        public void DoChangePage(int idx)
        {
            m_OnShowItem(idx);
            RefreshDotCell(idx);
        }
        //------------------------------------------------------
        public void OnBeginDrag(PointerEventData eventData)
        {
            m_vBeginPoint = eventData.position;
            m_IsDraging = true;
        }
        //------------------------------------------------------
        public void OnDrag(PointerEventData eventData)
        {
        }
        //------------------------------------------------------
        public void OnEndDrag(PointerEventData eventData)
        {
            bool bLeft = eventData.position.x < m_vBeginPoint.x;
            bool bRight = eventData.position.x > m_vBeginPoint.x;
            m_IsDraging = false;
            if (bLeft) { SlideLeft(); return; }
            if (bRight) { SlideRight(); ; return; }
        }
        //------------------------------------------------------
        public void SlideLeft()
        {
            if (m_CurrentIdx >= m_TotalSlices - 1) return;

            m_CurrentIdx = m_CurrentIdx + 1;

            DoChangePage(m_CurrentIdx);
        }

        //------------------------------------------------------
        public void SlideRight()
        {
            if (m_CurrentIdx <= 0) return;

            m_CurrentIdx = m_CurrentIdx - 1;

            DoChangePage(m_CurrentIdx);
        }

        //------------------------------------------------------

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_IsDraging) return;
            if (eventData.pointerEnter&&eventData.pointerEnter.transform.localPosition.x < 0)
                    SlideRight();
            if (eventData.pointerEnter && eventData.pointerEnter.transform.localPosition.x > 0)
                    SlideLeft();

        }
        //------------------------------------------------------

    }
}