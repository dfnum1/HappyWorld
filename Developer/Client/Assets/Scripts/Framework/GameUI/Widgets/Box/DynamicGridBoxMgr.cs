/********************************************************************
生成日期:	6:8:2020 17:06
类    名: 	DynamicGridBoxMgr
作    者:	JaydenHe
描    述:	滚动窗口不规则混合动态子物件管理
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TopGame.Base;
namespace TopGame.UI
{
    public class CellData
    {
        public int GroupId;
        public int CellId;
        public RectTransform Rect;
        public bool IsInView;
        public bool IsAvaliable;
        public bool IsInited;
        public Transform PoolObj;
        public CellData(RectTransform rect)
        {
            Rect = rect;
        }

    }

    [UIWidgetExport]
    public class DynamicGridBoxMgr : MonoBehaviour
    {
        /// <summary>
        /// 标题模板
        /// </summary>
        public Transform TitleTemplate;
        /// <summary>
        /// 内容模板
        /// </summary>
        public Transform ContentTemplate;
        /// <summary>
        /// 格子模板
        /// </summary>
        public Transform CellTemplate;
        /// <summary>
        /// 格子实体内存池
        /// </summary>
        public GameObject PoolObj;
        /// <summary>
        /// 内存池挂载点
        /// </summary>
        public Transform PoolRoot;
        /// <summary>
        /// 设置Item内容的委托
        /// </summary>
        /// <param name="item">Item对象</param>
        /// <param name="wrapIndex">当前所在组idx</param>
        /// <param name="realIndex">当前组idx</param>
        public delegate void OnShowItemDelegate(GameObject item, int wrapIndex, int groupIdx);
        /// <summary>
        /// 设置Item的委托
        /// </summary>
        public OnShowItemDelegate OnShowItem;
        /// <summary>
        /// 设置Title的委托
        /// </summary>
        public OnShowItemDelegate OnShowTitleItem;

        /// <summary>
        /// 标题高度
        /// </summary>
        float Title_h = 0;

        /// <summary>
        /// 格子大小
        /// </summary>
        float Cell_w = 0, Cell_h = 0;
        /// <summary>
        /// 行总宽度
        /// </summary>
        float m_fLineWidth;
        /// <summary>
        /// 动态格子大小
        /// </summary>
        float m_fCellRealWidth;
        /// <summary>
        /// 格子真实高度
        /// </summary>
        float m_fCellRealHeight;
        /// <summary>
        /// 每行显示个数
        /// </summary>
        int m_nLineShowCnt;

        /// <summary>
        /// 格子布局
        /// </summary>
        GridLayoutGroup m_GridLayout;
        /// <summary>
        /// 当前RectTransform对象
        /// </summary>
        RectTransform m_rTrans;

        /// <summary>
        /// ScrollRect
        /// </summary>
        ScrollRect m_Scroll;
        /// <summary>
        /// ScrollRect
        /// </summary>
        RectTransform m_ScrollRect;

        /// <summary>
        /// 各组cell个数
        /// </summary>
        List<int> m_SetCount;
        /// <summary>
        /// 对象列表字典
        /// </summary>
        Dictionary<int, List<CellData>> m_CellMap;
        /// <summary>
        /// 滚动窗口四角坐标
        /// </summary>
        Vector3[] m_SRWorldConner = new Vector3[4];
        /// <summary>
        /// 格子四角坐标
        /// </summary>
        Vector3[] m_TempConner = new Vector3[4];
        /// <summary>
        /// 内存池
        /// </summary>
        Stack<Transform> m_Pools = new Stack<Transform>();
        /// <summary>
        /// 内存池容量
        /// </summary>
        int m_PoolSize = 25;
        /// <summary>
        /// 每帧创建个数
        /// </summary>
        public int m_SpawnPerFrame = 5;
        /// <summary>
        /// 内存池是否初始化
        /// </summary>
        bool m_PoolInited = false;
        public bool m_IsMgrInited = false;
        Vector2 SR_size = Vector2.zero;//SrollRect的尺寸
        //------------------------------------------------------
        public void RefreshScrollHeight()
        {
            float fHeight = 0;
            foreach (RectTransform child in m_rTrans)
            {
                fHeight += child.rect.height;
            }
            m_rTrans.sizeDelta = new Vector2(m_rTrans.sizeDelta.x, fHeight);
        }
        //------------------------------------------------------
        public void GetScrollWorldConner()
        {
            m_ScrollRect.GetWorldCorners(m_SRWorldConner);
        }
        //------------------------------------------------------
        void Update()
        {
            if (!m_IsMgrInited || m_PoolInited) return;

            for (int i = 0; i < m_SpawnPerFrame; i++)
            {
                if (m_Pools.Count < m_PoolSize)
                {
                    GameObject obj = Object.Instantiate(PoolObj, PoolRoot);
                    obj.transform.localPosition = Vector3.zero;
                    m_Pools.Push(obj.transform);
                }
                else
                {
                    m_PoolInited = true;
                    WrapContent();
                    break;
                }
            }
        }

        //------------------------------------------------------
        public void Refresh()
        {
            WrapContent();
        }
        //------------------------------------------------------
        public Transform GetOne()
        {
            if (m_Pools.Count > 0)
            {
                return m_Pools.Pop();
            }

            return null;
        }

        //------------------------------------------------------
        public bool IsInView(RectTransform go)
        {
            go.GetWorldCorners(m_TempConner);
            float x01 = m_SRWorldConner[0].x;
            float x02 = m_SRWorldConner[2].x;
            float y01 = m_SRWorldConner[0].y;
            float y02 = m_SRWorldConner[2].y;

            float x11 = m_TempConner[0].x;
            float x12 = m_TempConner[2].x;
            float y11 = m_TempConner[0].y;
            float y12 = m_TempConner[2].y;

            float zx = Mathf.Abs(x01 + x02 - x11 - x12);
            float x = Mathf.Abs(x01 - x02) + Mathf.Abs(x11 - x12);
            float zy = Mathf.Abs(y01 + y02 - y11 - y12);
            float y = Mathf.Abs(y01 - y02) + Mathf.Abs(y11 - y12);

            return zx <= x && zy <= y;
        }
        //------------------------------------------------------
        public void Init(List<int> setCount, OnShowItemDelegate showItem, OnShowItemDelegate showTitleItem)
        {
            m_SetCount = setCount;
            OnShowItem = showItem;
            OnShowTitleItem = showTitleItem;
            SR_size = m_Scroll.GetComponent<RectTransform>().rect.size;

            m_ScrollRect = m_Scroll.GetComponent<RectTransform>();
            m_CellMap = new Dictionary<int, List<CellData>>();
            Transform item;
            for (int i = 0; i < m_SetCount.Count; i++)
            {
                m_CellMap.Add(i, new List<CellData>());
                Transform title = Object.Instantiate(TitleTemplate, transform);
                title.localScale = Vector3.one;

                UpdateTitleItem(title,i,i);

                Transform content = Object.Instantiate(ContentTemplate,transform);
                content.localScale = Vector3.one;
                float hegiht = GetContentHeight(m_SetCount[i]);
                RectTransform contentRect = content.GetComponent<RectTransform>();
                contentRect.sizeDelta = new Vector2(m_rTrans.rect.width, hegiht);

                for (int j = 0; j < m_SetCount[i]; j++)
                {                   
                    item = Object.Instantiate(CellTemplate, content);
                    item.localScale = Vector3.one;
                    item.name = "Cell" + j;
                    CellData data = new CellData(item.GetComponent<RectTransform>());
                    data.GroupId = i;
                    data.CellId = j;
                    m_CellMap[i].Add(data);
                }
            }

            TitleTemplate.gameObject.SetActive(false);
            CellTemplate.gameObject.SetActive(false);
            ContentTemplate.gameObject.SetActive(false);
            PoolObj.gameObject.SetActive(false);
            m_Scroll.onValueChanged.RemoveAllListeners();
            m_Scroll.onValueChanged.AddListener(delegate { WrapContent(); });//添加滚动事件回调
            m_IsMgrInited = true;
        }
        //------------------------------------------------------
        void WrapContent()
        {
            foreach (var row in m_CellMap)
            {
                for (int i = 0; i < row.Value.Count; i++)
                {
                    if (IsInView(row.Value[i].Rect))
                    {
                        if (!row.Value[i].IsInited)
                        {
                            UpdateItem(row.Value[i].Rect.transform, row.Value[i].CellId, row.Value[i].GroupId);
                            row.Value[i].IsInited = true;
                        }
                    }
                    else
                    {
                        row.Value[i].IsInited = false;
                        //recycle
                        if (row.Value[i].PoolObj)
                        {
                            row.Value[i].PoolObj.transform.SetParent(PoolRoot);
                            row.Value[i].PoolObj.transform.localPosition = Vector3.zero;

                            if (!m_Pools.Contains(row.Value[i].PoolObj))
                            m_Pools.Push(row.Value[i].PoolObj);

                            row.Value[i].PoolObj = null;
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        void UpdateItem(Transform cellItem, int index, int groupIdx)
        {
            Transform poolObj = GetOne();
            if (poolObj != null && OnShowItem != null)
            {
                poolObj.SetParent(cellItem);
                poolObj.localScale = Vector3.one;
                poolObj.localPosition = Vector3.zero;
                if (!poolObj.gameObject.activeSelf)
                    poolObj.gameObject.SetActive(true);
                m_CellMap[groupIdx][index].PoolObj = poolObj;
                OnShowItem(poolObj.gameObject, index, groupIdx);
            }
        }
        //------------------------------------------------------
        void UpdateTitleItem(Transform cellItem, int index, int groupIdx)
        {
            if (OnShowTitleItem != null)
            {
                OnShowTitleItem(cellItem.gameObject, index, groupIdx);
            }
        }

        //------------------------------------------------------
        private float GetContentHeight(int cellCnt)
        {
            return Mathf.CeilToInt(cellCnt / m_nLineShowCnt) * m_fCellRealHeight;
        }
        //------------------------------------------------------
        private void Awake()
        {
            if (TitleTemplate == null)
            {
                Framework.Plugin.Logger.Warning("滚动窗口：" + transform.name + " 没有指定Title模板");
                return;
            }

            if (CellTemplate == null)
            {
                Framework.Plugin.Logger.Warning("滚动窗口：" + transform.name + " 没有指定Cell模板");
                return;
            }

            if (ContentTemplate == null)
            {
                Framework.Plugin.Logger.Warning("滚动窗口：" + transform.name + " 没有指定Content模板");
                return;
            }

            if (PoolObj == null)
            {
                Framework.Plugin.Logger.Warning("滚动窗口：" + transform.name + " 没有指定内存池实例模板");
                return;
            }

            if (PoolRoot == null)
            {
                Framework.Plugin.Logger.Warning("滚动窗口：" + transform.name + " 没有指定内存池根节点");
                return;
            }

            RectTransform titleRect = TitleTemplate.GetComponent<RectTransform>();
            Title_h = titleRect.rect.size.y * titleRect.localScale.y;

            RectTransform rect = CellTemplate.GetComponent<RectTransform>();
            Cell_w = rect.rect.size.x * rect.localScale.x;
            Cell_h = rect.rect.size.y * rect.localScale.y;

            m_rTrans = transform.GetComponent<RectTransform>();
            m_Scroll = transform.parent.parent.GetComponent<ScrollRect>();
            m_fLineWidth = m_rTrans.rect.width;
            m_nLineShowCnt = Mathf.FloorToInt(m_fLineWidth / Cell_w);
            int nRowShowCnt = Mathf.CeilToInt(m_rTrans.rect.height / Cell_h);
            m_PoolSize = Mathf.CeilToInt(m_nLineShowCnt * nRowShowCnt * 2f);
            m_GridLayout = ContentTemplate.GetComponent<GridLayoutGroup>();
            m_fCellRealWidth = m_fLineWidth / m_nLineShowCnt;
            m_fCellRealHeight = Cell_h;
            m_GridLayout.cellSize = new Vector2(m_fCellRealWidth, m_fCellRealHeight);
        }
    }
}
