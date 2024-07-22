/********************************************************************
生成日期:	2021/7/28
类    名: 	GridListBox
作    者:	ChenFenHui
描    述:	滚动窗口动态子物件管理
*********************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TopGame.Core;
using System;
using TopGame.Base;
using DG.Tweening;

namespace TopGame.UI
{
    

    public enum e_Direction
    {
        Horizontal,
        Vertical
    }


    public class GridListBox : MonoBehaviour
    {

        public e_Direction m_Direction = e_Direction.Horizontal;

        public int m_Row = 1;

        public float m_Spacing = 0f; //间距
        public GameObject m_CellGameObject; //指定的cell

        protected Action<GameObject, int> m_FuncCallBackFunc;
        protected Action<GameObject, int> m_FuncOnClickCallBack;
        protected Action<int, bool, GameObject> m_FuncOnButtonClickCallBack;

        protected RectTransform rectTrans;

        protected float m_PlaneWidth;
        protected float m_PlaneHeight;

        protected float m_ContentWidth;
        protected float m_ContentHeight;

        protected float m_CellObjectWidth;
        protected float m_CellObjectHeight;

        protected GameObject m_Content;
        protected RectTransform m_ContentRectTrans;

        private bool m_isInited = false;

        //记录 物体的坐标 和 物体 
        public struct CellInfo
        {
            public Vector3 pos;
            public GameObject obj;
            public bool needHide;
        };
        protected CellInfo[] m_CellInfos;

        public CellInfo[] CellInfos
        {
            get { return m_CellInfos; }
        }

        protected bool m_IsInited = false;

        protected ScrollRect m_ScrollRect;

        protected int m_MaxCount = -1; //列表数量

        protected int m_MinIndex = -1;
        protected int m_MaxIndex = -1;

        protected bool m_IsClearList = false; //是否清空列表
        public bool IsClearList
        {
            set { m_IsClearList = value; }
        }

        private bool isMoveAnim = false;
        public void Init(Action<GameObject, int> callBack)
        {
            Init(callBack, null);
        }

        public void Init(Action<GameObject, int> callBack, Action<GameObject, int> onClickCallBack)
        {

            DisposeAll();

            m_FuncCallBackFunc = callBack;

            if (onClickCallBack != null)
            {
                m_FuncOnClickCallBack = onClickCallBack;
            }

            if (m_isInited)
                return;


            m_Content = this.GetComponent<ScrollRect>().content.gameObject;

            if (m_CellGameObject == null)
            {
                m_CellGameObject = m_Content.transform.GetChild(0).gameObject;
            }
            /* Cell 处理 */
            //m_CellGameObject.transform.SetParent(m_Content.transform.parent, false);
            SetPoolsObj(m_CellGameObject);

            RectTransform cellRectTrans = m_CellGameObject.GetComponent<RectTransform>();
            cellRectTrans.pivot = new Vector2(0.5f, 0.5f);
            CheckAnchor(cellRectTrans);
            cellRectTrans.anchoredPosition = Vector2.zero;

            //记录 Cell 信息
            m_CellObjectHeight = cellRectTrans.rect.height;
            m_CellObjectWidth = cellRectTrans.rect.width;

            //记录 Plane 信息
            rectTrans = GetComponent<RectTransform>();
            Rect planeRect = rectTrans.rect;
            m_PlaneHeight = planeRect.height;
            m_PlaneWidth = planeRect.width;

            //记录 Content 信息
            m_ContentRectTrans = m_Content.GetComponent<RectTransform>();
            Rect contentRect = m_ContentRectTrans.rect;
            m_ContentHeight = contentRect.height;
            m_ContentWidth = contentRect.width;

            m_ContentRectTrans.pivot = new Vector2(0f, 1f);
            //m_ContentRectTrans.sizeDelta = new Vector2 (planeRect.width, planeRect.height);
            //m_ContentRectTrans.anchoredPosition = Vector2.zero;
            CheckAnchor(m_ContentRectTrans);

            m_ScrollRect = this.GetComponent<ScrollRect>();

            m_ScrollRect.onValueChanged.RemoveAllListeners();
            //添加滑动事件
            m_ScrollRect.onValueChanged.AddListener(delegate (Vector2 value) { ScrollRectListener(value); });





            m_isInited = true;

        }
        //检查 Anchor 是否正确
        private void CheckAnchor(RectTransform rectTrans)
        {
            if (m_Direction == e_Direction.Vertical)
            {
                if (!((rectTrans.anchorMin == new Vector2(0, 1) && rectTrans.anchorMax == new Vector2(0, 1)) ||
                         (rectTrans.anchorMin == new Vector2(0, 1) && rectTrans.anchorMax == new Vector2(1, 1))))
                {
                    rectTrans.anchorMin = new Vector2(0, 1);
                    rectTrans.anchorMax = new Vector2(1, 1);
                }
            }
            else
            {
                if (!((rectTrans.anchorMin == new Vector2(0, 1) && rectTrans.anchorMax == new Vector2(0, 1)) ||
                         (rectTrans.anchorMin == new Vector2(0, 0) && rectTrans.anchorMax == new Vector2(0, 1))))
                {
                    rectTrans.anchorMin = new Vector2(0, 0);
                    rectTrans.anchorMax = new Vector2(0, 1);
                }
            }
        }

        //实时刷新列表时用
        public void UpdateList()
        {
            for (int i = 0, length = m_CellInfos.Length; i < length; i++)
            {
                CellInfo cellInfo = m_CellInfos[i];
                if (cellInfo.obj != null)
                {
                    float rangePos = m_Direction == e_Direction.Vertical ? cellInfo.pos.y : cellInfo.pos.x;
                    if (!IsOutRange(rangePos))
                    {
                        Func(m_FuncCallBackFunc, cellInfo.obj, true);
                    }
                }
            }
        }

        //刷新某一项
        public void UpdateCell(int index)
        {
            CellInfo cellInfo = m_CellInfos[index - 1];
            if (cellInfo.obj != null)
            {
                float rangePos = m_Direction == e_Direction.Vertical ? cellInfo.pos.y : cellInfo.pos.x;
                if (!IsOutRange(rangePos))
                {
                    Func(m_FuncCallBackFunc, cellInfo.obj);
                }
            }
        }

        public void ShowList(int num)
        {
            m_MinIndex = -1;
            m_MaxIndex = -1;

            //-> 计算 Content 尺寸
            if (m_Direction == e_Direction.Vertical)
            {
                float contentSize = (m_Spacing + m_CellObjectHeight) * Mathf.CeilToInt((float)num / m_Row);
                m_ContentHeight = contentSize;
                m_ContentWidth = m_ContentRectTrans.sizeDelta.x;
                contentSize = contentSize < rectTrans.rect.height ? rectTrans.rect.height : contentSize;
                m_ContentRectTrans.sizeDelta = new Vector2(m_ContentWidth, contentSize);
                if (num != m_MaxCount)
                {
                    m_ContentRectTrans.anchoredPosition = new Vector2(m_ContentRectTrans.anchoredPosition.x, 0);
                }
            }
            else
            {
                float contentSize = (m_Spacing + m_CellObjectWidth) * Mathf.CeilToInt((float)num / m_Row);
                m_ContentWidth = contentSize;
                m_ContentHeight = m_ContentRectTrans.sizeDelta.x;
                contentSize = contentSize < rectTrans.rect.width ? rectTrans.rect.width : contentSize;
                m_ContentRectTrans.sizeDelta = new Vector2(contentSize, m_ContentHeight);
                if (num != m_MaxCount)
                {
                    m_ContentRectTrans.anchoredPosition = new Vector2(0, m_ContentRectTrans.anchoredPosition.y);
                }
            }

            //-> 计算 开始索引
            int lastEndIndex = 0;

            //-> 过多的物体 扔到对象池 ( 首次调 ShowList函数时 则无效 )
            if (m_IsInited)
            {
                lastEndIndex = num - m_MaxCount > 0 ? m_MaxCount : num;
                lastEndIndex = m_IsClearList ? 0 : lastEndIndex;

                int count = m_IsClearList ? m_CellInfos.Length : m_MaxCount;
                for (int i = lastEndIndex; i < count; i++)
                {
                    if (m_CellInfos[i].obj != null || m_CellInfos[i].needHide)
                    {
                        SetPoolsObj(m_CellInfos[i].obj);
                        m_CellInfos[i].obj = null;
                        m_CellInfos[i].needHide = false;
                    }
                }
                              
            }

            //-> 以下四行代码 在for循环所用
            CellInfo[] tempCellInfos = m_CellInfos;
            m_CellInfos = new CellInfo[num];

            //-> 1: 计算 每个Cell坐标并存储 2: 显示范围内的 Cell
            for (int i = 0; i < num; i++)
            {
                // * -> 存储 已有的数据 ( 首次调 ShowList函数时 则无效 )
                if (m_MaxCount != -1 && i < lastEndIndex)
                {
                    CellInfo tempCellInfo = tempCellInfos[i];
                    //-> 计算是否超出范围
                    float rPos = m_Direction == e_Direction.Vertical ? tempCellInfo.pos.y : tempCellInfo.pos.x;
                    if (!IsOutRange(rPos))
                    {
                        //-> 记录显示范围中的 首位index 和 末尾index
                        m_MinIndex = m_MinIndex == -1 ? i : m_MinIndex; //首位index
                        m_MaxIndex = i; // 末尾index

                        if (tempCellInfo.obj == null)
                        {
                            tempCellInfo.obj = GetPoolsObj();
                        }
                        tempCellInfo.obj.transform.GetComponent<RectTransform>().anchoredPosition = tempCellInfo.pos;
                        tempCellInfo.obj.name = i.ToString();
                        tempCellInfo.obj.SetActive(true);

                        //动画
                        TweenAnim(i, tempCellInfo.obj);

                        Func(m_FuncCallBackFunc, tempCellInfo.obj);
                    }
                    else
                    {
                        SetPoolsObj(tempCellInfo.obj);
                        tempCellInfo.obj = null;
                    }
                    m_CellInfos[i] = tempCellInfo;
                    continue;
                }

                CellInfo cellInfo = new CellInfo();

                float pos = 0;  //坐标( isVertical ? 记录Y : 记录X )
                float rowPos = 0; //计算每排里面的cell 坐标

                // * -> 计算每个Cell坐标
                if (m_Direction == e_Direction.Vertical)
                {
                    pos = m_CellObjectHeight * Mathf.FloorToInt(i / m_Row) + m_Spacing * Mathf.FloorToInt(i / m_Row) + 0.5f * m_CellObjectHeight;
                    rowPos = m_CellObjectWidth * (i % m_Row) + m_Spacing * (i % m_Row) + 0.5f * m_CellObjectWidth;
                    cellInfo.pos = new Vector3(rowPos, -pos, 0);
                }
                else
                {
                    pos = m_CellObjectWidth * Mathf.FloorToInt(i / m_Row) + m_Spacing * Mathf.FloorToInt(i / m_Row) + 0.5f * m_CellObjectWidth;
                    rowPos = m_CellObjectHeight * (i % m_Row) + m_Spacing * (i % m_Row) + 0.5f * m_CellObjectHeight;
                    cellInfo.pos = new Vector3(pos, -rowPos, 0);
                }

                //-> 计算是否超出范围
                float cellPos = m_Direction == e_Direction.Vertical ? cellInfo.pos.y : cellInfo.pos.x;
                if (IsOutRange(cellPos))
                {
                    cellInfo.obj = null;
                    cellInfo.needHide = false;
                    m_CellInfos[i] = cellInfo;
                    continue;
                }

                //-> 记录显示范围中的 首位index 和 末尾index
                m_MinIndex = m_MinIndex == -1 ? i : m_MinIndex; //首位index
                m_MaxIndex = i; // 末尾index

                //-> 取或创建 Cell
                GameObject cell = GetPoolsObj();
                cell.transform.GetComponent<RectTransform>().anchoredPosition = cellInfo.pos;
                cell.gameObject.name = i.ToString();

                //-> 存数据
                cellInfo.obj = cell;
                cellInfo.needHide = false;
                m_CellInfos[i] = cellInfo;


                TweenAnim(i, cell);

               
                //-> 回调  函数
                Func(m_FuncCallBackFunc, cell);
            }

            m_MaxCount = num;
            m_IsInited = true;


        }

        /// <summary>
        /// 动画
        /// </summary>
        /// <param name="i"></param>
        /// <param name="cell"></param>
        private void TweenAnim(int i, GameObject cell)
        {
            ScaleTween scaleTween = cell.GetComponent<ScaleTween>();
            scaleTween?.EnablePlay();
            scaleTween.index = i;
            scaleTween.DelayTime = 0.15f;
            scaleTween.Play();
            scaleTween.OnComplete = () =>
            {
                cell.GetComponent<EventTriggerListener>()?.RefreshScale();
            };
        }


        private int GetIndexByPosition(Vector3 pos)
        {
            for (int i = 0; i < CellInfos.Length; i++)
            {
                if (pos == CellInfos[i].pos)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 排序移动
        /// </summary>
        /// <param name="infos"></param>
        public void SortMove(int start,TweenCallback callback)
        {
            ScrollToCell(start,()=> {
                List<CellInfo> sortList = new List<CellInfo>();
                for (int i = start; i < m_CellInfos.Length; i++)
                {
                    if (!m_CellInfos[i].needHide)
                    {
                        sortList.Add(m_CellInfos[i]);
                    }
                }

                int length = sortList.Count < 25 ? sortList.Count : 25;
                isMoveAnim = length>=0?true:false;
                if (length > 0)
                {
                    for (int i = 0; i < length; i++)
                    {
                        int index = GetIndexByPosition(sortList[i].pos);
                        if (CellInfos[index].obj == null && index != -1)
                        {
                            ShowAndSetCell(index);
                            Func(m_FuncCallBackFunc, CellInfos[index].obj);
                        }

                        Tweener tween = CellInfos[index].obj.GetComponent<RectTransform>().DOAnchorPos3D(CellInfos[start + i].pos, 0.8f);

                        if (i == length - 1)
                        {
                            tween.OnComplete(() =>
                            {
                                isMoveAnim = false;
                                callback();
                            });
                        }

                    }
                }
                else
                {
                    isMoveAnim = false;
                    callback();
                }
                
            });
            
        }

        /// <summary>
        /// 显示对应index的cell
        /// </summary>
        /// <param name="index"></param>
        public void ShowAndSetCell(int index)
        {
            CellInfo cellInfo = m_CellInfos[index];
            GameObject cell = GetPoolsObj();
            cell.transform.GetComponent<RectTransform>().anchoredPosition = cellInfo.pos;
            cell.gameObject.name = index.ToString();
            cellInfo.obj = cell;
            m_CellInfos[index] = cellInfo;
        }

        /// <summary>
        /// 移动到对应的cell位置
        /// </summary>
        /// <param name="index"></param>
        public void ScrollToCell(int index,TweenCallback callback = null)
        {
            CellInfo cellInfo = CellInfos[index];
            float cellPos = m_Direction == e_Direction.Vertical ? cellInfo.pos.y : cellInfo.pos.x;
            if (IsOutRange(cellPos))
            {
                isMoveAnim = true;
                if (m_Direction == e_Direction.Vertical)
                    m_ContentRectTrans.DOAnchorPosY(Mathf.Abs(cellPos), 0f).OnComplete(() => {
                        isMoveAnim = false;
                        UpdateCheck();
                        callback?.Invoke(); });
                else
                    m_ContentRectTrans.DOAnchorPosX(cellPos, 0f).OnComplete(() => {
                        isMoveAnim = false;
                        UpdateCheck();
                        callback?.Invoke(); });
            }
            else
            {
                callback?.Invoke();
            }
        }

        // 更新滚动区域的大小
        public void UpdateSize()
        {
            Rect rect = GetComponent<RectTransform>().rect;
            m_PlaneHeight = rect.height;
            m_PlaneWidth = rect.width;
        }

        //滑动事件
        protected void ScrollRectListener(Vector2 value)
        {
            if (isMoveAnim)
                return;
            UpdateCheck();
        }

        private void UpdateCheck()
        {
            if (m_CellInfos == null)
                return;

            //检查超出范围
            for (int i = 0, length = m_CellInfos.Length; i < length; i++)
            {
                CellInfo cellInfo = m_CellInfos[i];
                GameObject obj = cellInfo.obj;
                Vector3 pos = cellInfo.pos;

                float rangePos = m_Direction == e_Direction.Vertical ? pos.y : pos.x;
                //判断是否超出显示范围
                if (IsOutRange(rangePos) || cellInfo.needHide)
                {
                    //把超出范围的cell 扔进 poolsObj里
                    if (obj != null)
                    {
                        SetPoolsObj(obj);

                        m_CellInfos[i].obj = null;
                    }
                }
                else
                {
                    if (obj == null)
                    {
                        //优先从 poolsObj中 取出 （poolsObj为空则返回 实例化的cell）
                        GameObject cell = GetPoolsObj();
                        cell.transform.GetComponent<RectTransform>().anchoredPosition = pos;
                        cell.gameObject.name = i.ToString();
                        m_CellInfos[i].obj = cell;
                        Func(m_FuncCallBackFunc, cell);
                    }
                }
            }
        }

        //判断是否超出显示范围
        protected bool IsOutRange(float pos)
        {
            Vector3 listP = m_ContentRectTrans.anchoredPosition;
            if (m_Direction == e_Direction.Vertical)
            {
                if (pos + listP.y > m_CellObjectHeight || pos + listP.y + 0.5f * m_CellObjectHeight < -rectTrans.rect.height)
                {
                    return true;
                }
            }
            else
            {
                if (pos + listP.x < -m_CellObjectWidth || pos + listP.x + 0.5f * m_CellObjectWidth > rectTrans.rect.width)
                {
                    return true;
                }
            }
            return false;
        }

        //对象池 机制  (存入， 取出) cell
        protected Stack<GameObject> poolsObj = new Stack<GameObject>();
        int allCount = 0;
        //取出 cell
        protected GameObject GetPoolsObj()
        {
            GameObject cell = null;
            if (poolsObj.Count > 0)
            {
                cell = poolsObj.Pop();
            }

            if (cell == null)
            {
                cell = Instantiate(m_CellGameObject) as GameObject;
                allCount++;
            }
            cell.transform.SetParent(m_Content.transform);
            cell.transform.localScale = Vector3.one;
            cell.SetActive(true);

            return cell;
        }
        //存入 cell
        public void SetPoolsObj(GameObject cell)
        {
            if (cell != null)
            {
                poolsObj.Push(cell);
                cell.SetActive(false);
            }
        }

        //回调
        protected void Func(Action<GameObject, int> func, GameObject selectObject, bool isUpdate = false)
        {
            int num = int.Parse(selectObject.name) + 1;
            if (func != null)
            {
                func(selectObject, num);
            }

        }

        public void DisposeAll()
        {
            if (m_FuncCallBackFunc != null)
            {
                m_FuncCallBackFunc = null;
            }
            if (m_FuncOnClickCallBack != null)
            {
                m_FuncOnClickCallBack = null;
            }
        }

        protected void OnDestroy()
        {
            DisposeAll();
        }

    }
}