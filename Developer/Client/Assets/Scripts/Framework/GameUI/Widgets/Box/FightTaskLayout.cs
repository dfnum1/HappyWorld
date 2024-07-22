/********************************************************************
生成日期:	9:30:2020 17:06
类    名: 	FightTaskLayout
作    者:	JaydenHe
描    述:	战令任务界面布局
*********************************************************************/

using Framework.Plugin.Guide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TopGame.UI
{
    [UIWidgetExport]
    public class FightTaskLayout : MonoBehaviour
    {
        public RectTransform MainTaskTopBar;
        public RectTransform[] MainTaskSubBar;
        public RectTransform MainTaskTimeBar;
        public RectTransform[] FightTaskSubBar;
        public RectTransform FightTaskTimeBar;
        public Image FightTaskBg;
        public Image MainTaskBg;

        public RectTransform EmptyMainTask;
        public RectTransform EmptyFightTask;


        public GameObject PoolRoot;
        private float CurrentHeight = 100;
        private float Spacing = 5;
        private float MainBarSpace = 30;
        private float FightTaskSpace = 30;
        private float MiddleSpace = 50;

        public float TimeOffset = 20;
     
        public void UpdateUI(int mainTaskCnt, int fightTaskCnt, System.Action<GameObject> mainTaskShow, System.Action<GameObject, int> dailyTaskShow, System.Action<GameObject, int> fightTaskShow, System.Action<GameObject> fightTaskBg, System.Action<GameObject> dailyTimeShow, System.Action<GameObject> fightTaskTimeShow)
        {
            for (int i = 0; i < MainTaskSubBar.Length; i++)
            {
                if(MainTaskSubBar[i].gameObject.activeSelf)
                MainTaskSubBar[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < FightTaskSubBar.Length; i++)
            {
                if (FightTaskSubBar[i].gameObject.activeSelf)
                    FightTaskSubBar[i].gameObject.SetActive(false);
            }

            InitUI(mainTaskCnt, fightTaskCnt, mainTaskShow, dailyTaskShow, fightTaskShow, fightTaskBg, dailyTimeShow, fightTaskTimeShow);
        }

        public void InitUI(int mainTaskCnt, int fightTaskCnt, System.Action<GameObject> mainTaskShow, System.Action<GameObject, int> dailyTaskShow, System.Action<GameObject, int> fightTaskShow, System.Action<GameObject> fightTaskBg, System.Action<GameObject> dailyTimeShow, System.Action<GameObject> fightTaskTimeShow)
        {
            //头部
            MainTaskTopBar.transform.SetParent(transform);
            MainTaskTopBar.localScale = Vector2.one;
            CurrentHeight = MainTaskTopBar.sizeDelta.y / 2;
            MainTaskTopBar.anchoredPosition = new Vector2(0, -CurrentHeight);
            CurrentHeight += MainTaskTopBar.rect.height + Spacing;
            if (mainTaskShow != null) mainTaskShow(MainTaskTopBar.gameObject);

            //主线任务背景高度
            float mainTaskBgHeight = (MainTaskSubBar[0].rect.height + Spacing) * mainTaskCnt
                +(mainTaskCnt == 0 ? EmptyMainTask.rect.height + Spacing : 0) 
                + MainBarSpace + MainTaskSubBar[0].rect.height / 2 + MainTaskTimeBar.rect.height;
            CurrentHeight += mainTaskBgHeight / 2;

            MainTaskBg.transform.SetParent(transform);
            RectTransform mainRect = MainTaskBg.GetComponent<RectTransform>();
            mainRect.localScale = Vector2.one;
            mainRect.anchoredPosition = new Vector2(0, -CurrentHeight);
            mainRect.sizeDelta = new Vector2(mainRect.sizeDelta.x, mainTaskBgHeight);

            //子物体离背景顶部距离
            float tempMainHeight = MainTaskSubBar[0].rect.height/2 + MainBarSpace*2;
            for (int i = 0; i < mainTaskCnt; i++)
            {
                RectTransform sub = GetAvaliable(MainTaskSubBar);
                sub.gameObject.SetActive(true);
                sub.transform.SetParent(MainTaskBg.transform);

                RectTransform rectSub = sub.GetComponent<RectTransform>();
                rectSub.localScale = Vector2.one;
                rectSub.anchoredPosition = new Vector2(0, -tempMainHeight);

                if (i == mainTaskCnt - 1)
                {
                    //末尾时间
                    GameObject subMainTime = MainTaskTimeBar.gameObject;
                    subMainTime.transform.SetParent(MainTaskBg.transform);
                    RectTransform subMainTimeRect = subMainTime.GetComponent<RectTransform>();
                    subMainTimeRect.localScale = Vector2.one;
                    tempMainHeight += subMainTimeRect.rect.height + Spacing + MainTaskSubBar[0].rect.height / 2;
                    subMainTimeRect.anchoredPosition = new Vector2(0, -tempMainHeight + TimeOffset);
                    dailyTimeShow(subMainTime);
                }
                else
                    tempMainHeight += rectSub.rect.height + Spacing;

                if (dailyTaskShow != null) dailyTaskShow(rectSub.gameObject,i);
            }

            if (mainTaskCnt == 0)
            {
                //空提示
                GameObject emptyTip = EmptyMainTask.gameObject;
                emptyTip.transform.SetParent(MainTaskBg.transform);
                RectTransform subMainEmptyRect = emptyTip.GetComponent<RectTransform>();
                subMainEmptyRect.localScale = Vector2.one;
                subMainEmptyRect.anchoredPosition = new Vector2(0, -tempMainHeight);
                tempMainHeight += subMainEmptyRect.rect.height + Spacing;

                //末尾时间
                GameObject subMainTime = MainTaskTimeBar.gameObject;
                subMainTime.transform.SetParent(MainTaskBg.transform);
                RectTransform subMainTimeRect = subMainTime.GetComponent<RectTransform>();
                subMainTimeRect.localScale = Vector2.one;
                subMainTimeRect.anchoredPosition = new Vector2(0, -tempMainHeight+ subMainTimeRect.rect.height);
                dailyTimeShow(subMainTime);
            }

            CurrentHeight += mainTaskBgHeight / 2 + MiddleSpace;
            //战令任务背景高度
            float fightTaskBgHeight = (FightTaskSubBar[0].rect.height + Spacing) * fightTaskCnt
                +(fightTaskCnt == 0 ? EmptyFightTask.rect.height + Spacing : 0) 
                + FightTaskSpace 
                + FightTaskSubBar[0].rect.height / 2
                + FightTaskTimeBar.rect.height;

            CurrentHeight += fightTaskBgHeight / 2;

            FightTaskBg.transform.SetParent(transform);
            RectTransform fightRect = FightTaskBg.GetComponent<RectTransform>();
            fightRect.localScale = Vector2.one;
            fightRect.anchoredPosition = new Vector2(0, -CurrentHeight);
            fightRect.sizeDelta = new Vector2(fightRect.sizeDelta.x, fightTaskBgHeight);
            fightTaskBg(FightTaskBg.gameObject);

            //子物体离背景顶部距离
            float tempHeight = FightTaskSubBar[0].rect.height / 2 + FightTaskSpace*2;
            for (int j = 0; j < fightTaskCnt; j++)
            {
                RectTransform sub = GetAvaliable(FightTaskSubBar);
                sub.gameObject.SetActive(true);
                sub.transform.SetParent(FightTaskBg.transform);

                RectTransform rectSub = sub.GetComponent<RectTransform>();
                rectSub.localScale = Vector2.one;
                rectSub.anchoredPosition = new Vector2(0, -tempHeight);

                if (j == fightTaskCnt - 1)
                {
                    //末尾时间
                    FightTaskTimeBar.transform.SetParent(FightTaskBg.transform);
                    FightTaskTimeBar.localScale = Vector2.one;
                    tempHeight += FightTaskTimeBar.rect.height+Spacing+ FightTaskSubBar[0].rect.height / 2;
                    FightTaskTimeBar.anchoredPosition = new Vector2(0, -tempHeight+ TimeOffset);
                    fightTaskTimeShow(FightTaskTimeBar.gameObject);
                }

                tempHeight += rectSub.rect.height + Spacing;

                if (fightTaskShow != null) fightTaskShow(rectSub.gameObject, j);
            }

            if (fightTaskCnt == 0)
            {
                //空提示
                GameObject emptyTip = EmptyFightTask.gameObject;
                emptyTip.transform.SetParent(FightTaskBg.transform);
                RectTransform subFightEmptyRect = emptyTip.GetComponent<RectTransform>();
                subFightEmptyRect.localScale = Vector2.one;
                subFightEmptyRect.anchoredPosition = new Vector2(0, -tempHeight);
                tempHeight += subFightEmptyRect.rect.height + Spacing;

                //末尾时间
                FightTaskTimeBar.transform.SetParent(FightTaskBg.transform);
                FightTaskTimeBar.localScale = Vector2.one;
                FightTaskTimeBar.anchoredPosition = new Vector2(0, -tempHeight+ FightTaskTimeBar.rect.height);
                fightTaskTimeShow(FightTaskTimeBar.gameObject);
            }
            RectTransform content = transform.GetComponent<RectTransform>();
            float finalContentSize = CurrentHeight + fightTaskBgHeight / 2 + FightTaskSubBar[0].rect.height / 2 + FightTaskSpace;
            content.sizeDelta = new Vector2(content.sizeDelta.x, finalContentSize);
        }

        public RectTransform GetAvaliable(RectTransform[] pools)
        {
            for (int i=0;i<pools.Length;i++)
            {
                if (!pools[i].gameObject.activeSelf)
                    return pools[i];
            }

            return null;
        }
    }
}