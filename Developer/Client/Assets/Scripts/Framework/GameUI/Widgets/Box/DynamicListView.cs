/********************************************************************
生成日期:	7:10:2020 18:16
类    名: 	DynamicListView
作    者:	zdq
描    述:	列表加载组件
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TopGame.Base;

namespace TopGame.UI
{
    [UIWidgetExport]
    public class DynamicListView : ScrollRect, IGuideScroll
    {
        List<GameObject> m_vInstantiateItems = null;
        bool m_bInit = false;
        //------------------------------------------------------
        public void Clear()
        {
            if (content == null)
            {
                return;
            }

            for (int i = content.transform.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(content.transform.GetChild(i).gameObject);//todo:目前只是将加载和销毁移到到这边进行管理,待优化
            }

            if (m_vInstantiateItems != null)
            {
                m_vInstantiateItems.Clear();
            }
            m_bInit = false;
        }
        //------------------------------------------------------
        public T Instantiate<T>(UISerialized original, Transform parent) where T : UnityEngine.Object
        {
            if (m_vInstantiateItems == null)
            {
                m_vInstantiateItems = new List<GameObject>();
            }
            GameObject item = GameObject.Instantiate(original.gameObject, parent) as GameObject;//todo:目前只是将加载和销毁移到到这边进行管理,待优化
            m_vInstantiateItems.Add(item);
            return item.GetComponent<T>();
        }
        //------------------------------------------------------
        public void InitCompleted()
        {
            m_bInit = true;
        }

        #region IGuideScroll
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

            return m_vInstantiateItems[index] .transform;
        }
        #endregion
    }
}