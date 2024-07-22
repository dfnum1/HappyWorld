/********************************************************************
生成日期:	6:23:2020 10:06
类    名: 	LongPressTrigger
作    者:	JaydenHe
描    述:	长按触发事件
*********************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TopGame.UI
{
    [UIWidgetExport]
    public class LongPressTrigger : EventTriggerListener
    {
        public uint unlockID = 0;
        public float IntervalTime = 0.3f;
        public System.Action<GameObject> OnLongpress;
        public System.Action<GameObject> OnNormalClick;

        private bool m_bIsPointDown = false;
        private float m_fLastInvokeTime;
        private float m_fCumulateTime = 0;
        public void Reset()
        {
            m_bIsPointDown = false;
            m_fCumulateTime = 0;
        }
        //------------------------------------------------------
        void Update()
        {
            if (m_bIsPointDown)
            {
                if (Framework.Plugin.Guide.GuideSystem.getInstance().bDoing)
                {
                    return;
                }
                m_fCumulateTime += Time.deltaTime;
                if (Time.time - m_fLastInvokeTime > IntervalTime)
                {
                    OnLongpress?.Invoke(gameObject);
                    m_fLastInvokeTime = Time.time;
                }
            }
        }
        //------------------------------------------------------
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (Framework.Module.ModuleManager.mainFramework == null)
            {
                base.OnPointerDown(eventData);
                return;
            }
            Core.GameFramework pGameFrame = Framework.Module.ModuleManager.mainFramework as Core.GameFramework;
            if(pGameFrame == null || pGameFrame.unlockMgr == null)
            {
                base.OnPointerDown(eventData);
                return;
            }
            if (unlockID != 0 && pGameFrame.unlockMgr.IsLocked(unlockID))
                return;

            m_fCumulateTime = 0;
            m_bIsPointDown = true;
            m_fLastInvokeTime = Time.time;
            base.OnPointerDown(eventData);
        }
        //------------------------------------------------------
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (Framework.Module.ModuleManager.mainFramework == null)
            {
                base.OnPointerDown(eventData);
                return;
            }
            Core.GameFramework pGameFrame = Framework.Module.ModuleManager.mainFramework as Core.GameFramework;
            if (pGameFrame == null || pGameFrame.unlockMgr == null)
            {
                base.OnPointerDown(eventData);
                return;
            }
            if (unlockID != 0 && pGameFrame.unlockMgr.IsLocked(unlockID))
                return;
            m_bIsPointDown = false;
            if (m_fCumulateTime < IntervalTime)
            {
                if (eventData.dragging)
                {
                    Framework.Plugin.Logger.Warning("过滤点击按钮后,拖拽的操作");
                    return;
                }

                OnNormalClick?.Invoke(gameObject);
                isPlayCommonClickSound = false;
                base.OnPointerClick(eventData);
            }
            base.OnPointerClick(eventData);
        }
        //------------------------------------------------------
        public override void OnPointerExit(PointerEventData eventData)
        {
            m_bIsPointDown = false;
            base.OnPointerExit(eventData);
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            Reset();//长按状态,如果直接隐藏按钮,不会触发 OnPointerUp 和 OnPointerExit,导致按钮状态异常,所以在Disable进行重置
        }
    }
}