/********************************************************************
生成日期:	8:10:2022 11:16
类    名: 	ReverseScroll
作    者:	hjc
描    述:	反向滚动组件
*********************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    public class ReverseScroll : MonoBehaviour
    {
        public ListView listView;
        public ScrollRect scrollRect;
        public float scrollSpeed = 1;

        RectTransform m_Transform;
        Vector2  m_nContentPos;

        //------------------------------------------------------
        public void Awake()
        {
            if (listView)
            {
                listView.onValueChanged.AddListener(OnListScroll);
                m_nContentPos = listView.content.anchoredPosition;
            }
            if (scrollRect)
            {
                scrollRect.onValueChanged.AddListener(OnListScroll);
                m_nContentPos = scrollRect.content.anchoredPosition;
            }
            m_Transform = this.transform as RectTransform;
        }
        //------------------------------------------------------
        private void OnListScroll(Vector2 pos)
        {
            float deltaX = 0;
            float deltaY = 0;
            if (listView)
            {
                if (listView.motionType == ListView.MotionType.Horizontal)
                {
                    deltaX = (listView.content.anchoredPosition.x - m_nContentPos.x) * scrollSpeed;
                }
                else
                {
                    deltaY = (listView.content.anchoredPosition.y - m_nContentPos.y) * scrollSpeed;
                }
                m_nContentPos = listView.content.anchoredPosition;
            }
            if (scrollRect)
            {
                if (scrollRect.horizontal)
                {
                    deltaX = (scrollRect.content.anchoredPosition.x - m_nContentPos.x) * scrollSpeed;
                }
                if (scrollRect.vertical)
                {
                    deltaY = (scrollRect.content.anchoredPosition.y - m_nContentPos.y) * scrollSpeed;
                }
                m_nContentPos = scrollRect.content.anchoredPosition;
            }
            m_Transform.anchoredPosition = new Vector2(m_Transform.anchoredPosition.x - deltaX, m_Transform.anchoredPosition.y - deltaY);
        }
    }
}