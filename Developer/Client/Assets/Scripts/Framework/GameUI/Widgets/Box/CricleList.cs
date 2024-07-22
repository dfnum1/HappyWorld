using System;
using System.Collections;
using System.Collections.Generic;
using TopGame.UI;
using UnityEngine;
using UnityEngine.UI;

public class CricleList : MonoBehaviour, IGuideScroll
{
    public GameObject Template;
    [Range(0, 3600)]
    public float radius = 1700;
    [Range(0, 360)]
    public float sectorAngle = 10;

    [Range(-360, 360)]
    public float startAngle = -90;

    public int OnePageNum = 5;

    [Range(1, 10)]
    public float MoveSpeed = 5;

    public int SlideStep = 1;

    [SerializeField]
    private Vector3 NormalScale = Vector3.one;
    [SerializeField]
    private Vector3 SelectScale = Vector3.one;

    private int m_OnePage = 0;

    private class ItemData : Framework.Plugin.IQuickSort<ItemData>
    {
        public UISerialized ui;
        public RectTransform R;
        public int sectorIndex;
        public int virtualIndex;
        public int actualIndex;
        public int depth;
        public bool startMove = false;
        public float delay = 0;

        public bool active;
        public bool renderable;

        public float ToG;
        public float CurG;

        public float fLerpScale;
        public bool isSelected;
        //------------------------------------------------------
        public int CompareTo(int type, ItemData other)
        {
            return depth - other.depth;
        }
        //------------------------------------------------------
        public void Destroy()
        {
        }
    }

    private List<ItemData> m_VirtualItems;    

    private bool m_bMoveing = false;
    private int m_nCurrentIndex = 0;
    private ItemData m_CurrentSelected = null;
    private float m_fDelayTime = 0;
    //------------------------------------------------------
    Queue<ItemData> m_vPools = new Queue<ItemData>();
    Dictionary<int, ItemData> m_vTmpContains = new Dictionary<int, ItemData>();

    public Action<UISerialized, int,int> CallBack = null;
    public Action<UISerialized, int, bool> SelectedChange = null;
    bool m_isInited = false;
    int m_nDataNum = 0;
    List<int> m_vUserData = null;
    //------------------------------------------------------
    void Awake()
    {
        Init(0);
    }
    //------------------------------------------------------
    public void Init(List<int> userDatas, int select)
    {
        m_vUserData = userDatas;
        Init(select);
    }
    //------------------------------------------------------
    public void Init(int dataNum, int select)
    {
        m_nDataNum = dataNum;
        Init(select);
    }
    //------------------------------------------------------
    void Init(int select)
    {
        int sectorIndex = OnePageNum / 2;
        int instNum = OnePageNum;
        if (OnePageNum % 2 == 0) instNum++;
        m_OnePage = instNum;
        if (m_VirtualItems == null || m_VirtualItems.Count != m_OnePage)
        {
            if(m_VirtualItems!=null)
            {
                for(int i = m_VirtualItems.Count; i < m_OnePage; ++i)
                {
                    ItemData itemdata = CreateItemData(i, sectorIndex--);
                    m_VirtualItems.Add(itemdata);
                }
                while(m_VirtualItems.Count > m_OnePage)
                {
                    ItemData itemData = m_VirtualItems[m_VirtualItems.Count - 1];
                    GameObject.DestroyImmediate(itemData.R.gameObject);
                    m_VirtualItems.RemoveAt(m_VirtualItems.Count -1);
                }
            }
            else
            {
                m_VirtualItems = new List<ItemData>();
                for (int i = 0; i < m_OnePage; i++)
                {
                    ItemData itemdata = CreateItemData(i, sectorIndex--);
                    m_VirtualItems.Add(itemdata);
                }
            }
            SetDep();
            m_bMoveing = true;
        }
        m_CurrentSelected = null;
        m_nCurrentIndex = 0;
        StartCoroutine(SetSelecePos(select,true));
        m_isInited = true;
    }
    //------------------------------------------------------
    public void Refresh()
    {
        ItemData node;
        for (int i =0; i < m_VirtualItems.Count; ++i)
        {
            node = m_VirtualItems[i];
            CallBack?.Invoke(node.ui, node.virtualIndex, node.actualIndex);
        }
    }
    //------------------------------------------------------
    public void RefreshID(int id)
    {
        ItemData node;
        for (int i = 0; i < m_VirtualItems.Count; ++i)
        {
            node = m_VirtualItems[i];
            if( GetUserDataID(node.actualIndex) == id )
            {
                CallBack?.Invoke(node.ui, node.virtualIndex, node.actualIndex);
            }
        }
    }
    //------------------------------------------------------
    IEnumerator SetSelecePos(int select, bool bForce)
    {
        yield return new WaitForSeconds(0.2f);
        SetSelected(select, bForce);
    }
    //------------------------------------------------------
    private ItemData CreateItemData(int i, int sectorIndex)
    {
        GameObject obj = GameObject.Instantiate(Template.gameObject, transform);
        obj.SetActive(true);
        UISerialized ui = obj.GetComponent<UISerialized>();
        if (ui == null)
        {
            ui = obj.AddComponent<UISerialized>();
#if UNITY_EDITOR
            Debug.LogWarning("模版控件需要加 UISerialized 组件");
#endif
        }
        ItemData itemdata = new ItemData();
        itemdata.R = ui.transform as RectTransform;
        itemdata.ui = ui;
        itemdata.sectorIndex = sectorIndex;
        if(GetDataCount()> 0)
        {
            int actual = sectorIndex % GetDataCount();
            if (actual < 0) actual = GetDataCount() + sectorIndex;
            itemdata.actualIndex = actual;
        }
        itemdata.virtualIndex = i;
        itemdata.CurG = getG(0);
        itemdata.ToG = getG(sectorIndex);
        itemdata.delay = i * m_fDelayTime;
        itemdata.startMove = true;
        itemdata.fLerpScale = 1;

        if (itemdata.sectorIndex < 0)
        {
            itemdata.depth = m_OnePage/2 + itemdata.sectorIndex;
        }
        else if (itemdata.sectorIndex == 0)
        {
            itemdata.depth = m_OnePage;
        }
        else
            itemdata.depth = m_OnePage - itemdata.sectorIndex;
        return itemdata;
    }
    //------------------------------------------------------
    public int CurrentSelectIndex
    {
        get { return m_CurrentSelected!=null? m_CurrentSelected.actualIndex:-1; }
    }
    //------------------------------------------------------
    public int CurrentSelectID
    {
        get
        {
            return GetUserDataID(CurrentSelectIndex);
        }
    }
    //------------------------------------------------------
    public int CurrentListIndex
    {
        get
        {
            return m_CurrentSelected != null ? m_CurrentSelected.virtualIndex : 0;
        }
    }
    //------------------------------------------------------
    public int GetUserDataID(int index)
    {
        if (m_vUserData != null)
        {
            if (index < 0 || index >= m_vUserData.Count) return -1;
            return m_vUserData[index];
        }
        return index;
    }
    //------------------------------------------------------
    int GetDataCount()
    {
        if (m_vUserData != null) return m_vUserData.Count;
        return m_nDataNum;
    }
    //------------------------------------------------------
    public void ScrollTo(int virIndex)
    {
        int moveStep = 0;
        for (int i = 0; i < m_VirtualItems.Count; ++i)
        {
            if( m_VirtualItems[i].virtualIndex == virIndex)
            {
                moveStep = m_VirtualItems[i].sectorIndex;
                break;
            }
        }
        if (moveStep == 0) return;
        m_nCurrentIndex = 0;
        SetSelected(-moveStep);
    }
    //------------------------------------------------------
    public void ScrollTo(bool bNext)
    {
        if (m_OnePage <= 1) return;
        int index = m_nCurrentIndex;
        if (bNext)
            index+= SlideStep;
        else
            index-= SlideStep;
        SetSelected(index);
    }
    //------------------------------------------------------
    private void SetDep()
    {
        Framework.Plugin.SortUtility.QuickSortUp<ItemData>(ref m_VirtualItems);
        ItemData node;
        for (int i = 0; i < m_VirtualItems.Count; ++i)
        {
            node = m_VirtualItems[i];
            if(node.R == null) continue;
            node.R.transform.SetSiblingIndex(node.depth);           
        }
    }
    //------------------------------------------------------
    private void SetSelected(int index, bool bForce=false)
    {
     //   if (m_nCurrentIndex == index) return;
        int dataNum = GetDataCount();
        if (dataNum <= 0) return;
        int moveCnt = index - m_nCurrentIndex;
        m_nCurrentIndex = index;

        int centerIndex = CurrentSelectIndex+moveCnt;
        if (centerIndex < 0) centerIndex = m_OnePage - 1;
        if(centerIndex >= m_OnePage) centerIndex = 0;

           index = index % dataNum;
        if (index < 0) index = dataNum + index;

        int splitPage = m_OnePage / 2;
        int start_index = centerIndex - splitPage;
        int end_index = centerIndex + splitPage;
        int depth = m_VirtualItems.Count;

        m_CurrentSelected = null;

        int sectorIndex = splitPage;
        m_vPools.Clear();
        m_vTmpContains.Clear();
        ItemData item;
        for (int i = 0; i < m_VirtualItems.Count; i++)
        {
            item = m_VirtualItems[i];
            if (item.virtualIndex < start_index || item.virtualIndex > end_index)
            {
                m_vPools.Enqueue(item);
                item.active = false;
            }
            else
            {
                m_vTmpContains.Add(item.virtualIndex, item);
                item.active = true;
            }
            bool isSelected = item.isSelected;
            item.isSelected = false;
            if (isSelected)
                SelectedChange?.Invoke(item.ui, item.actualIndex, false);
            item.renderable = false;
        }
        ItemData selectNode = null;
        for (int i = start_index; i <= end_index; ++i)
        {
            if (!m_vTmpContains.TryGetValue(i, out item))
            {
                item = m_vPools.Dequeue();
                item.active = true;
                item.renderable = true;
            }
            else
            {
                item.active = true;
            }

            if (moveCnt != 0)
            {
                item.sectorIndex += moveCnt;
                bool bCricle = false;
                item.fLerpScale = 1;
                if (item.sectorIndex > splitPage)
                {
                    item.sectorIndex = (item.sectorIndex + splitPage) % m_OnePage - splitPage;
                    bCricle = true;
                    item.fLerpScale = 1.5f;
                }
                else if (item.sectorIndex < -splitPage)
                {
                    item.sectorIndex = (item.sectorIndex - splitPage) % m_OnePage + splitPage;
                    bCricle = true;
                    item.fLerpScale = 1.5f;
                }

                item.ToG = getG(item.sectorIndex);
                if (bCricle)
                {
                    if (item.ToG < 0) item.CurG = item.CurG - 360;
                    else item.CurG = 360 - Mathf.Abs(item.CurG);
                }
                item.startMove = true;
            }
            int actual = i% dataNum;
            if (actual < 0) actual = dataNum + actual;
            item.actualIndex = actual;

            if (item.sectorIndex < 0)
            {
                item.depth = splitPage + item.sectorIndex;
            }
            else if (item.sectorIndex == 0)
            {
                item.depth = m_OnePage;
            }
            else
                item.depth = m_OnePage - item.sectorIndex;

            if (item.sectorIndex ==0)
                selectNode = item;
            item.virtualIndex = i;// item.sectorIndex;
#if UNITY_EDITOR
            item.R.transform.name = item.sectorIndex.ToString() + "   dep=" + item.depth + "   vir=" + item.virtualIndex;
#endif
        }
        m_CurrentSelected = selectNode;
        if (selectNode != null)
        {
            m_CurrentSelected = selectNode;
            m_CurrentSelected.isSelected = true;
        }
        for (int i = 0; i < m_VirtualItems.Count; i++)
        {
            item = m_VirtualItems[i];
            if (item.R.gameObject.activeSelf != item.active)
                item.R.gameObject.SetActive(item.active);
            if(item.renderable || bForce)
            {
                CallBack?.Invoke(item.ui, item.virtualIndex, item.actualIndex);
                item.renderable = false;
            }
        }
        if (m_CurrentSelected != null)
            SelectedChange?.Invoke(m_CurrentSelected.ui, m_CurrentSelected.actualIndex, true);
        SetDep();
        m_bMoveing = true;
    }
    //------------------------------------------------------
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //    ScrollTo(false);
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //    ScrollTo(true);
        if (!m_bMoveing || m_VirtualItems == null || m_VirtualItems.Count <= 0) return;
        ItemData node;
        bool bo = false;
        for(int i =0; i < m_VirtualItems.Count; ++i)
        {
            node = m_VirtualItems[i];
        //    if (node.startMove)
            {
                bo = true;
                node.delay -= Time.deltaTime;
                if (node.delay <= 0)
                {
                    node.CurG = Mathf.Lerp(node.CurG,node.ToG, Time.deltaTime * MoveSpeed*node.fLerpScale);
                    node.R.anchoredPosition = getPos(node.CurG);

                    node.R.rotation = Quaternion.Euler(0, 0, node.CurG);
                }
            }
            if (node.isSelected)
                node.R.localScale = Vector2.Lerp(node.R.localScale, SelectScale, Time.deltaTime * MoveSpeed * node.fLerpScale);
            else
                node.R.localScale = Vector2.Lerp(node.R.localScale, NormalScale, Time.deltaTime * MoveSpeed * node.fLerpScale);
        }
        m_bMoveing = bo;        
    }
    //------------------------------------------------------
    private void Reset()
    {
        if (m_VirtualItems == null) return;
        int sectorIndex = OnePageNum / 2;
        ItemData node;
        for(int i=0; i< m_VirtualItems.Count; ++i)
        {
            node = m_VirtualItems[i];
            node.startMove = false;
            node.sectorIndex = sectorIndex--;
            node.virtualIndex = sectorIndex;
            node.CurG = getG(node.sectorIndex);
            node.ToG = node.CurG;
            node.fLerpScale = 1;

            Vector2 v = getPos(node.CurG);
            node.R.anchoredPosition = v;
            node.R.rotation = Quaternion.Euler(0, 0, node.CurG - startAngle);
            node.R.localScale = NormalScale;
        }
        m_bMoveing = false;
        return;
    }
    //------------------------------------------------------
    private float getG(int posId)
    {
        float angle = sectorAngle / m_OnePage * posId;
        return angle;
    }
    //------------------------------------------------------
    private Vector2 getPos(float g)
    {
        g = (g + startAngle) * Mathf.Deg2Rad;
        float x = Mathf.Cos(g) * radius;
        float y = Mathf.Sin(g) * radius;
        return new Vector2(x, y);
    }
    //------------------------------------------------------
    private void OnDestroy()
    {
        Clear();
    }
    //------------------------------------------------------
    public void Clear()
    {
        m_isInited = false;
        m_CurrentSelected = null;
        m_nCurrentIndex = 0;
        m_vUserData = null;
        m_nDataNum = 0;
        if(m_VirtualItems!=null)
        {
            ItemData node;
            for (int i = 0; i < m_VirtualItems.Count; ++i)
            {
                node = m_VirtualItems[i];
                GameObject.DestroyImmediate(node.R.gameObject);
            }
            m_VirtualItems.Clear();
            m_VirtualItems = null;
        }
    }
    //------------------------------------------------------
    ItemData FindNode(int id)
    {
        if (m_VirtualItems == null) return null;
        for (int i = 0; i < m_VirtualItems.Count; ++i)
        {
            if (m_VirtualItems[i].actualIndex == id) return m_VirtualItems[i];
        }
        return null;
    }
    //------------------------------------------------------
    ItemData FindIndexNode(int index)
    {
        if (m_VirtualItems == null || index < 0 || m_VirtualItems.Count <= 0) return null;
        for (int i = 0; i < m_VirtualItems.Count; ++i)
        {
            if (m_VirtualItems[i].virtualIndex == index) return m_VirtualItems[i];
        }
        return null;
    }
    //------------------------------------------------------
    public Transform GetItem(int id)
    {
        if (m_VirtualItems == null) return null;
        if (m_vUserData != null) id = m_vUserData.IndexOf(id);
        return GetItemByIndex(id);
    }
    //------------------------------------------------------
    public UISerialized GetItemUI(int id)
    {
        if (m_VirtualItems == null) return null;
        if (m_vUserData != null) id = m_vUserData.IndexOf(id);
        for (int i = 0; i < m_VirtualItems.Count; ++i)
        {
            if (m_VirtualItems[i].actualIndex == id) return m_VirtualItems[i].ui;
        }
        return null;
    }
    //------------------------------------------------------
    public Transform GetItemByIndex(int index)
    {
        if (m_VirtualItems == null) return null;
        ItemData node;
        for (int i =0; i < m_VirtualItems.Count; ++i)
        {
            node = m_VirtualItems[i];
            if (node.actualIndex == index)
                return node.R;
        }
        return null;
    }
    //------------------------------------------------------
    public int GetIndexByItem(GameObject go)
    {
        if (m_VirtualItems == null) return -1;
        ItemData node;
        for (int i = 0; i < m_VirtualItems.Count; ++i)
        {
            node = m_VirtualItems[i];
            if (node.R && node.R.gameObject == go)
                return node.actualIndex;
        }
        return -1;
    }
    //------------------------------------------------------
    public bool GetIsLoadCompleted()
    {
        return m_isInited;
    }
}
