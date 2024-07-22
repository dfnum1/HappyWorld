/********************************************************************
生成日期:	2020-11-24
类    名: 	RedDotManager
作    者:	JaydenHe
描    述:	红点管理器
*********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using TopGame.Core;
using Framework.Module;
using TopGame.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Framework.Core;

namespace TopGame.Logic
{
    public enum RedDotType
    {
        None = 0,
        /// <summary>
        /// 大厅角色养成按钮
        /// </summary>
        Hall_PlayerBtn,
        Hall_EggBtn,
        Hall_HeadBtn,
        Hall_ShopBtn,
        Hall_TaskAchieveBtn,
        Hall_SignInBtn,
        HeroDetailMenuBar_Talent,
        HeroDetailMenuBar_Equip,
        HeroEquip_Weapon,
        HeroEquip_Equip1,
        HeroEquip_Equip2,
        HeroEquip_Equip3,
        HeroEquip_Equip4,
        HeroEquip_Equip5,
    }

    public class RedDotManager : Core.ARedDotManager, ITouchInput
    {
        int m_hashCode;
        List<RaycastResult> results = new List<RaycastResult>();
        public class RedDotInstance
        {
            public bool IsSatisfied;
            public bool IsClicked = false;
            public bool IsShow;
            public bool IsIgnoreClick = false; //是否点过就消失 如旧任务做完点消失 新任务又出现
            public IsSatisfyCondition Condition;
            public List<GameObject> Obj;

            public void SetShow()
            {
                IsSatisfied = Condition();
                IsShow = IsIgnoreClick ? IsSatisfied : (IsClicked ? false : IsSatisfied);
            }
        }

        private Dictionary<EUIType, Dictionary<RedDotType, RedDotInstance>> RedDotCache = new Dictionary<EUIType, Dictionary<RedDotType, RedDotInstance>>();

        /// <summary>
        /// 更新回调
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="type"></param>
        public override void UpdateRedDot(int ui, int type)
        {
            EUIType uiType = (EUIType)ui;
            RedDotType redDotType = (RedDotType)type;
            if (!RedDotCache.ContainsKey(uiType)) return;
            if (!RedDotCache[uiType].ContainsKey(redDotType)) return;

            if (CheckDataFilled(RedDotCache[uiType][redDotType]))
            {
                RedDotCache[uiType][redDotType].SetShow();
                UpdateSingleRedDotUI(uiType, redDotType);
            }
        }

        public void UpdateViewRedDot(EUIType ui)
        {
            foreach (var child in RedDotCache[ui])
            {
                UpdateRedDot((int)ui, (int)child.Key);
            }
        }

        public override void OnRedDotClickedCB(int ui, int type)
        {
            EUIType uiType = (EUIType)ui;
            RedDotType redDotType = (RedDotType)type;
            if (uiType >= EUIType.Count) return;
            RedDotCache[uiType][redDotType].IsClicked = true;
            UpdateRedDot(ui, type);
        }

        public override void BindRedDotGameObject(int ui, int type,GameObject reddot)
        {
            if (!reddot)
            {
                Framework.Plugin.Logger.Error("UI:" + ((EUIType)ui) + " RedDotType:" + ((RedDotType)type) + " 红点为空绑定失败");
                return;
            }
            EUIType uiType = (EUIType)ui;
            RedDotType redDotType = (RedDotType)type;

            if (!RedDotCache.ContainsKey(uiType))
                RedDotCache.Add(uiType, new Dictionary<RedDotType, RedDotInstance>());

            if (!RedDotCache[uiType].ContainsKey(redDotType))
                RedDotCache[uiType].Add(redDotType, new RedDotInstance());

            if(RedDotCache[uiType][redDotType].Obj == null)
                RedDotCache[uiType][redDotType].Obj = new List<GameObject>();
            RedDotCache[uiType][redDotType].Obj.Add(reddot);

            if (CheckDataFilled(RedDotCache[uiType][redDotType]))
                UpdateSingleRedDotUI(uiType, redDotType);
        }

        public override void UnBindRedDotGameObject(int uiType, int redDotType,GameObject reddot)
        {
            EUIType ui = (EUIType)uiType;
            RedDotType type = (RedDotType)redDotType;
            if (!reddot)
            {
                Framework.Plugin.Logger.Error("UI:" + ui + " RedDotType:" + type + " 红点为空绑定失败");
                return;
            }

            if (!RedDotCache.ContainsKey(ui))
                return;

            if (!RedDotCache[ui].ContainsKey(type))
                return;

            if(RedDotCache[ui][type].Obj == null)
                return;
            RedDotCache[ui][type].Obj.Remove(reddot);
        }

        public bool CheckDataFilled(RedDotInstance instance)
        {
            return instance.Condition != null && instance.Obj != null;
        }

        public override void Register(int uiType, int redDotType, IsSatisfyCondition condition,bool isIgnoreClick = false)
        {
            EUIType ui = (EUIType)uiType;
            RedDotType type = (RedDotType)redDotType;

            if (!RedDotCache.ContainsKey(ui))
                RedDotCache.Add(ui, new Dictionary<RedDotType, RedDotInstance>());

            if (!RedDotCache[ui].ContainsKey(type))
                RedDotCache[ui].Add(type, new RedDotInstance());

            RedDotCache[ui][type].Condition = condition;
            RedDotCache[ui][type].IsIgnoreClick = isIgnoreClick;
            RedDotCache[ui][type].SetShow();

            if (CheckDataFilled(RedDotCache[ui][type]))
                UpdateSingleRedDotUI(ui, type);
        }

        public void UpdateSingleRedDotUI(EUIType ui, RedDotType type)
        {
            for (int i = 0; i < RedDotCache[ui][type].Obj.Count; i++)
            {
                GameObject redDot = RedDotCache[ui][type].Obj[i];
                TopGame.Base.Util.SetActive(redDot,RedDotCache[ui][type].IsShow);
            }
        }

        public void SetRedDotClickState(EUIType ui, RedDotType type,bool isClick)
        {
            if (!RedDotCache.ContainsKey(ui)) return;
            if (!RedDotCache[ui].ContainsKey(type)) return;
            RedDotCache[ui][type].IsClicked = isClick;
        }

        protected override void Awake()
        {

        }

        protected override void OnDestroy()
        {
            RedDotCache.Clear();
        }

        public void OnTouchBegin(ATouchInput.TouchData touch)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = touch.position;

            results.Clear();
            if (EventSystem.current == null) return;
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            for (int i = 0; i < results.Count; i++)
            {
                RedDotListener listener = results[i].gameObject.GetComponent<RedDotListener>();
                if (listener != null)
                {
                    OnRedDotClickedCB(listener.UIType, listener.RedType);
                    break;
                }
            }
        }

        public void OnTouchMove(ATouchInput.TouchData touch)
        {

        }

        public void OnTouchWheel(float wheel, Vector2 mouse)
        {

        }

        public void OnTouchEnd(ATouchInput.TouchData touch)
        {

        }
    }
}