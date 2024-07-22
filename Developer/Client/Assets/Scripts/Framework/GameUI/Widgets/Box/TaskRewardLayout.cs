/********************************************************************
生成日期:	9:30:2020 17:06
类    名: 	TaskRewardLayout
作    者:	JaydenHe
描    述:	战令任务奖励界面布局
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TopGame.UI
{
    public enum TaskRewardRowType
    {
        TitleRow,
        NormalRow,
        DivLineRow,
        ExtraRewardDivLineRow,
        ExtraRewardItemRow,
        ExtraCertainLevelRewardRow,
    }
    [UIWidgetExport]
    public class TaskRewardLayout : MonoBehaviour
    {
        public RectTransform TitleRow;
        public RectTransform[] NormalRow;
        public RectTransform[] DivLineRow;
        public RectTransform ExtraRewardDivLineRow;
        public RectTransform ExtraRewardItemRow;
        public RectTransform ExtraCertainLevelRewardRow;

        public RectTransform MiddleYellowLine;
        public RectTransform MiddleBlueLine;

        System.Action<GameObject, int> OnShowTaskRewardAction;

        private float CurrentHeight = 100;
        public float Spacing = 10;
        public float RewardSpacing = 30;

        List<TaskRewardRowType> showList = new List<TaskRewardRowType>();

        public void UpdateUI(System.Action<GameObject, int> taskRewardShow, int unlockNormalRow, List<TaskRewardRowType> showList, System.Action<GameObject> showExtraReward, System.Action<GameObject> divShow, System.Action<GameObject> showCertainReward)
        {
            for (int i = 0; i < NormalRow.Length; i++)
            {
                if (NormalRow[i].gameObject.activeSelf)
                    NormalRow[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < DivLineRow.Length; i++)
            {
                if (DivLineRow[i].gameObject.activeSelf)
                    DivLineRow[i].gameObject.SetActive(false);
            }

            CurrentHeight = 100;
            InitUI(taskRewardShow, unlockNormalRow, showList, showExtraReward, divShow, showCertainReward);
        }

        public void InitUI(System.Action<GameObject, int> taskRewardShow,int unlockNormalRow,List<TaskRewardRowType> showList,System.Action<GameObject> showExtraReward, System.Action<GameObject> divShow, System.Action<GameObject> showCertainReward)
        {
            float unlockHeight = 0;
            int cnt = 0;
            for (int i = 0; i < showList.Count; i++)
            {
                RectTransform rect = GetItems(showList[i]);
                if (!rect.gameObject.activeSelf)
                    rect.gameObject.SetActive(true);
                if (rect.parent != transform)
                    rect.transform.SetParent(transform);

                rect.localScale = Vector2.one;
                if (showList[i] == TaskRewardRowType.ExtraRewardItemRow)
                {
                    CurrentHeight += RewardSpacing;
                    showExtraReward(rect.gameObject);
                }

                if (showList[i] == TaskRewardRowType.ExtraCertainLevelRewardRow)
                {
                   // CurrentHeight += RewardSpacing;
                    showCertainReward(rect.gameObject);
                }

                if (showList[i] == TaskRewardRowType.DivLineRow)
                {
                    divShow(rect.gameObject);
                }

               rect.anchoredPosition = new Vector2(0, -CurrentHeight);
                CurrentHeight += rect.rect.height + Spacing;

                if (showList[i] == TaskRewardRowType.NormalRow)
                {
                    taskRewardShow(rect.gameObject, cnt);
                    cnt++;
                }

                if (cnt >= unlockNormalRow && unlockHeight == 0)
                    unlockHeight = CurrentHeight;
            }

            RectTransform blueLine = MiddleBlueLine;
            if(blueLine.parent != transform)
                blueLine.transform.SetParent(transform);
            blueLine.localScale = Vector2.one;
            blueLine.sizeDelta = new Vector2(MiddleBlueLine.sizeDelta.x, CurrentHeight);
            blueLine.anchoredPosition = new Vector2(0, -CurrentHeight / 2);
          
            RectTransform yellowLine = MiddleYellowLine;
            if (yellowLine.parent != transform)
                yellowLine.transform.SetParent(transform);
            yellowLine.localScale = Vector2.one;
            yellowLine.sizeDelta = new Vector2(yellowLine.sizeDelta.x, unlockHeight);
            yellowLine.anchoredPosition = new Vector2(0, -unlockHeight / 2);
   
            blueLine.SetSiblingIndex(0);
            yellowLine.SetSiblingIndex(1);

            RectTransform content = transform.GetComponent<RectTransform>();
            content.sizeDelta = new Vector2(content.sizeDelta.x, CurrentHeight);
        }

        public RectTransform GetItems(TaskRewardRowType type)
        {
            switch (type)
            {
                case TaskRewardRowType.TitleRow:
                    return TitleRow;
                case TaskRewardRowType.NormalRow:
                    return GetAvaliable(NormalRow);
                case TaskRewardRowType.DivLineRow:
                    return GetAvaliable(DivLineRow);
                case TaskRewardRowType.ExtraRewardDivLineRow:
                    return ExtraRewardDivLineRow;
                case TaskRewardRowType.ExtraRewardItemRow:
                    return ExtraRewardItemRow;
                case TaskRewardRowType.ExtraCertainLevelRewardRow:
                    return ExtraCertainLevelRewardRow;
            }

            return null;
        }

        public RectTransform GetAvaliable(RectTransform[] pools)
        {
            for (int i = 0; i < pools.Length; i++)
            {
                if (!pools[i].gameObject.activeSelf)
                    return pools[i];
            }

            return null;
        }
    }
}