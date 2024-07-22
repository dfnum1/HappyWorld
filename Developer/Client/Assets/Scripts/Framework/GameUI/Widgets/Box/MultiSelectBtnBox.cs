/********************************************************************
生成日期:	6:3:2020 10:06
类    名: 	MultiSelectBtnBox
作    者:	JaydenHe
描    述:	多个按钮选择数量有上限
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using TopGame.UI;
using UnityEngine;

namespace TopGame.UI
{
    public class MultiSelectBtnBox : MonoBehaviour
    {
        public List<MultiSelectBtnBox> Cells = null;
        public int MaxLimit = 4;
        public List<uint> CurSelected = new List<uint>();

        public System.Action<GameObject> SelectViewAction;
        public System.Action<GameObject> UnSelectViewAction;
        public bool IsSelected;
        public uint ID;
        //------------------------------------------------------
        public void Init(List<MultiSelectBtnBox> cells, int maxLimit, System.Action<GameObject> select, System.Action<GameObject> unselect)
        {
            Cells = cells;
            MaxLimit = maxLimit;
            SelectViewAction = select;
            UnSelectViewAction = unselect;
        }
        //------------------------------------------------------
        public void OnSelectCell()
        {
            int nSelectCnt = 0;
            for (int i = 0; i < Cells.Count; i++)
            {
                if (Cells[i].IsSelected) nSelectCnt++;
            }

            if (nSelectCnt < MaxLimit)
            {
                SelectViewAction(gameObject);
                IsSelected = true;
            }
        }
        //------------------------------------------------------
        public void OnUnSelectCell()
        {
            UnSelectViewAction(gameObject);
            IsSelected = false;
        }
        //------------------------------------------------------
        public void Boardcast()
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                Cells[i].CurSelected = CurSelected;
            }
        }
        //------------------------------------------------------
        public void OnClick(uint selectParam)
        {
            if (IsSelected)
            {
                if (CurSelected.Count <= 1) return;

                OnUnSelectCell();
                CurSelected.Remove(selectParam);
                Boardcast();
            }
            else
            {
                if (CurSelected.Count >= MaxLimit) return;
                OnSelectCell();
                CurSelected.Add(selectParam);
                Boardcast();
            }
        }
        //------------------------------------------------------
    }
}
