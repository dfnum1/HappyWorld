/********************************************************************
生成日期:	7:10:2020 18:16
类    名: 	DynamicListView
作    者:	zdq
描    述:	引导加载物体查找组件
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    public class GuideScroll : MonoBehaviour, IGuideScroll
    {
        List<GameObject> m_vInstantiateItems = null;
        bool m_bInit = false;

        /// <summary>
        /// 在列表所有物品都加载完毕后,调用改函数进行初始化,即可接入引导查找
        /// </summary>
        public void Init()
        {
            if (m_vInstantiateItems == null)
            {
                m_vInstantiateItems = new List<GameObject>();
            }
            m_vInstantiateItems.Clear();

            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform child = transform.GetChild(i);
                m_vInstantiateItems.Add(child.gameObject);
            }

            m_bInit = true;
        }
        //------------------------------------------------------
        public void Clear()
        {
            m_bInit = false;
            if (m_vInstantiateItems != null)
            {
                m_vInstantiateItems.Clear();
                m_vInstantiateItems = null;
            }
            //该脚本只是获取挂载物体的所有子物体,并记录,清理时不进行物体销毁
        }
        //------------------------------------------------------
        public int GetIndexByItem(GameObject go)
        {
            if (m_vInstantiateItems == null)
            {
                return -1;
            }

            GameObject item = null;
            for (int i = 0; i < m_vInstantiateItems.Count; i++)
            {
                item = m_vInstantiateItems[i];
                if (item != null && go == item)
                {
                    return i;
                }
            }
            return -1;
        }
        //------------------------------------------------------
        public bool GetIsLoadCompleted()
        {
            return m_bInit;
        }
        //------------------------------------------------------
        public Transform GetItemByIndex(int index)
        {
            if (m_vInstantiateItems == null || index >= m_vInstantiateItems.Count)
            {
                return null;
            }

            return m_vInstantiateItems[index].transform;
        }
    }
}