/********************************************************************
生成日期:	6:8:2020 10:42
类    名: 	UICommonTip
作    者:	zdq
描    述:	通用提示选择框
*********************************************************************/

using Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TopGame.UI
{
    [UI((ushort)EUIType.CommonTip, UI.EUIAttr.UI)]
    public class UICommonTip : UIBase
    {
        UICommonTipView m_CommontipView = null;

        ETipType m_type;
        string m_content;
        string m_title;
        Action<ETipAction> m_Action;
        string m_yesBtnText;
        string m_noBtnText;

        Action<ETipType, string, string, Action<ETipAction>, string, string> m_ComTipPendingAction;

        string m_resContent;
        string m_resTitle;
        bool m_resIsShowDimond;
        int m_resDimondCnt;

        bool m_IsNetTips = false;

        bool m_IsDisableMaskClick = false;

        Action<string, string, Action<ETipAction>, bool, int> m_ResTipPendingAction;
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            m_CommontipView = m_pView as UICommonTipView;

            m_ComTipPendingAction?.Invoke(m_type, m_content, m_title, m_Action, m_yesBtnText, m_noBtnText);
            m_ComTipPendingAction = null;

            m_ResTipPendingAction?.Invoke(m_resTitle, m_resContent, m_Action, m_resIsShowDimond, m_resDimondCnt);
            m_ResTipPendingAction = null;
        }
        //------------------------------------------------------
        protected override void DoHide()
        {
            base.DoHide();
            m_CommontipView.OnHide();

            m_Action = null;

            m_IsNetTips = false;
            m_IsDisableMaskClick = false;
        }
        //------------------------------------------------------
        public void AddAction(Action<ETipAction> action)
        {
            m_Action += action;
        }
        //------------------------------------------------------
        public void DoShowTipDimond(string title, string content, System.Action<ETipAction> confirmAction, bool isShowDimond = false, int dimondCnt = 0)
        {
            Show();
            m_Action = null;
            m_CommontipView.ShowTip(title, content, confirmAction, isShowDimond, dimondCnt);
        }
        //------------------------------------------------------
        public void ShowTip_Dimond(string title, string content, System.Action<ETipAction> confirmAction, bool isShowDimond = false, int dimondCnt = 0)
        {
            if (m_CommontipView == null)
            {
                m_resContent = content;
                m_resTitle = title;
                m_Action = confirmAction;
                m_resIsShowDimond = isShowDimond;
                m_resDimondCnt = dimondCnt;
                m_ResTipPendingAction = DoShowTipDimond;
            }
            else
            {
                DoShowTipDimond(title, content, confirmAction, isShowDimond, dimondCnt);
            }
        }
        //------------------------------------------------------
        public void DoShowTip(ETipType type, string content, string title, System.Action<ETipAction> action = null, string yesBtnText = "确定", string noBtnText = "取消")
        {
            Show();

            this.m_Action = action;

            switch (type)
            {
                case ETipType.Yes:
                    m_CommontipView.ShowTip_Yes(content, title, yesBtnText);
                    break;
                case ETipType.Yes_No:
                    m_CommontipView.ShowTip_Yes_No(content, title, yesBtnText, noBtnText);
                    break;
                case ETipType.Yes_No_Close:
                    m_CommontipView.ShowTip_Yes_No_Close(content, title, yesBtnText, noBtnText);
                    break;
                case ETipType.Yes_Close:
                    m_CommontipView.ShowTip_Yes_Close(content, title, yesBtnText);
                    break;
                case ETipType.Close:
                    m_CommontipView.ShowTip_Close(content, title);
                    break;
                default:
                    break;
            }
        }

        //------------------------------------------------------
        public void ShowTip(ETipType type,string content, string title, System.Action<ETipAction> action = null,string yesBtnText = "确定",string noBtnText = "取消")
        {
            if (m_CommontipView == null)
            {
                m_type = type;
                m_content = content;
                m_title = title;
                m_Action = action;
                m_yesBtnText =yesBtnText;
                m_noBtnText =noBtnText;
                m_ComTipPendingAction = DoShowTip;
            }
            else
            {
                DoShowTip(type, content, title, action, yesBtnText, noBtnText);
            }

            
        }
        //------------------------------------------------------
        internal void OnClickm_MaskBtn(GameObject go, VariablePoolAble[] param)
        {
            if (m_IsDisableMaskClick)
            {
                return;
            }
            this.m_Action?.Invoke(ETipAction.Cancel);//点击遮罩和点击取消按钮一样,没有取消按钮的,直接隐藏
            this.Hide();

        }

        //------------------------------------------------------
        public void OnClickCancelBtn(GameObject go, params VariablePoolAble[] param)
        {
            this.Hide();
            this.m_Action?.Invoke(ETipAction.Cancel);
        }
        //------------------------------------------------------
        public void OnClickConfirmBtn(GameObject go, params VariablePoolAble[] param)
        {
            Hide();
            this.m_Action?.Invoke(ETipAction.Confirm);
        }
        //------------------------------------------------------
        public void OnClickCloseBtn(GameObject go, params VariablePoolAble[] param)
        {
            Hide();
            this.m_Action?.Invoke(ETipAction.Close);
        }
        //------------------------------------------------------
        public bool GetIsNetTips()
        {
            return m_IsNetTips;
        }
        //------------------------------------------------------
        public void SetIsNetTips(bool isNetTips)
        {
            m_IsNetTips = isNetTips;
        }
        //------------------------------------------------------
        public void SetADIconActive(bool active)
        {
            if (m_CommontipView != null)
            {
                m_CommontipView.SetADIconActive(active);
            }
        }
        //------------------------------------------------------
        public void SetDisableMaskClick(bool isDisable)
        {
            m_IsDisableMaskClick = isDisable;
        }
    }
}
