/********************************************************************
生成日期:	6:8:2020 10:42
类    名: 	UICommonTipView
作    者:	zdq
描    述:	通用提示选择框
*********************************************************************/


using System;
using System.Collections.Generic;
using TopGame.Base;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TopGame.UI
{
    [UI((ushort)EUIType.CommonTip, UI.EUIAttr.View)]
    public class UICommonTipView : UIView
    {
        UICommonTip m_pUI = null;
        Button m_YesBtn = null;
        Button m_NoBtn = null;
        Text m_Content = null;
        Text m_Title = null;
        Text m_Num = null;
        private Button m_CloseBtn;

        /// <summary>
        /// 记录关闭界面时要隐藏的物体
        /// </summary>
        List<GameObject> ResetGoList = new List<GameObject>();
        private Text m_ConfirmBtn_Text;
        private GameObject m_ADIcon;
        int m_lineCount = 0;

        //------------------------------------------------------
        public override void Init(UIBase pBase)
        {
            base.Init(pBase);
            m_pUI = pBase as UICommonTip;
        }
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            if (m_pUI == null) return;
            if (m_pUI.ui != null)
            {
                m_YesBtn = m_pUI.ui.GetWidget<Button>("YesBtn");
                if (m_YesBtn) EventTriggerListener.Get(m_YesBtn.gameObject).onClick = m_pUI.OnClickConfirmBtn;
                m_NoBtn = m_pUI.ui.GetWidget<Button>("NoBtn");
                if (m_NoBtn) EventTriggerListener.Get(m_NoBtn.gameObject).onClick = m_pUI.OnClickCancelBtn;


                m_Content = m_pUI.ui.GetWidget<Text>("ContentText");
                m_Title = m_pUI.ui.GetWidget<Text>("TitleText");
                m_Num = m_pUI.ui.GetWidget<Text>("Num");
                m_CloseBtn = m_pUI.ui.GetWidget<Button>("CloseBtn");
                if (m_CloseBtn) EventTriggerListener.Get(m_CloseBtn.gameObject).onClick = m_pUI.OnClickCloseBtn;

                m_ConfirmBtn_Text = m_pUI.ui.GetWidget<Text>("ConfirmBtn_Text");

                m_ADIcon = m_pUI.ui.Find("ADIcon");

                if(m_YesBtn) ResetGoList.Add(m_YesBtn.gameObject);
                if(m_NoBtn) ResetGoList.Add(m_NoBtn.gameObject);
                //ResetGoList.Add(m_Content.gameObject);
                //ResetGoList.Add(m_Title.gameObject);
                if(m_CloseBtn) ResetGoList.Add(m_CloseBtn.gameObject);

                Reset();//在第一次打开界面时,重置一下界面
            }
        }
        //------------------------------------------------------
        public void OnHide()
        {
            //关闭界面时,恢复所有界面默认状态
            Reset();
        }
        //------------------------------------------------------
        public override void Update(float fFrame)
        {
            base.Update(fFrame);

            if (m_Content == null)
            {
                return;
            }

            if (m_lineCount != m_Content.cachedTextGenerator.lineCount)
            {
                //Debug.Log(m_Content.cachedTextGenerator.lineCount);
                m_lineCount = m_Content.cachedTextGenerator.lineCount;
                SetContentTextAlignWithLineCount();
            }
        }
        //------------------------------------------------------
        void Reset()
        {
            foreach (var item in ResetGoList)
            {
                item.SetActive(false);
            }
            if (m_Content)
            {
                m_Content.rectTransform.anchoredPosition = Vector2.zero;
            }
            Util.SetActive(m_ADIcon, false);
            m_lineCount = 0;
        }
        //------------------------------------------------------
        public void ShowTip(string title,string content,System.Action<ETipAction> confirmAction,bool isShowDimond = false,int dimondCnt = 0)
        {
            Reset();

            UIUtil.SetLabel(m_Title, title);
            SetContent(content);
            m_pUI.AddAction(confirmAction);
            if (isShowDimond) UIUtil.SetLabel(m_Num, GlobalUtil.GetShortNum(dimondCnt));

            
            SetConfirmBtnActive(true);
        }
        //------------------------------------------------------
        public void ShowTip_Yes(string content,string title,string btnText)
        {
            Reset();

            SetConfirmBtnActive(true);
            UIUtil.SetLabel(m_ConfirmBtn_Text, btnText);
            SetContent(content);
            SetTitle(title);
        }
        //------------------------------------------------------
        public void ShowTip_Yes_No(string content, string title, string yesBtnText, string noBtnText)
        {
            Reset();

            SetConfirmBtnActive(true);

            UIUtil.SetLabel(m_ConfirmBtn_Text, yesBtnText);

            SetContent(content);
            SetTitle(title);
        }
        //------------------------------------------------------
        public void ShowTip_Yes_No_Close(string content, string title, string yesBtnText, string noBtnText)
        {
            Reset();

            SetConfirmBtnActive(true);
            SetCloseBtnActive(true);

            UIUtil.SetLabel(m_ConfirmBtn_Text, yesBtnText);

            SetContent(content);
            SetTitle(title);
        }
        //------------------------------------------------------
        public void ShowTip_Yes_Close(string content, string title, string yesBtnText)
        {
            Reset();

            SetConfirmBtnActive(true);
            SetCloseBtnActive(true);

            UIUtil.SetLabel(m_ConfirmBtn_Text, yesBtnText);

            SetContent(content);
            SetTitle(title);
        }
        //------------------------------------------------------
        public void ShowTip_Close(string content, string title)
        {
            Reset();

            SetCloseBtnActive(true);

            SetContent(content);
            SetTitle(title);
        }
        
        //------------------------------------------------------
        void SetGameObjectActive(GameObject go, bool isActive)
        {
            if (go == null) return;
            go.SetActive(isActive);
        }
        //------------------------------------------------------
        void SetConfirmBtnActive(bool isActive)
        {
            if(m_YesBtn) SetGameObjectActive(m_YesBtn.gameObject,isActive);
        }
        //------------------------------------------------------
        void SetNoBtnActive(bool isActive)
        {
            if(m_NoBtn) SetGameObjectActive(m_NoBtn.gameObject, isActive);
        }
        //------------------------------------------------------
        void SetCloseBtnActive(bool isActive)
        {
            if(m_CloseBtn) SetGameObjectActive(m_CloseBtn.gameObject, isActive);
        }
        //------------------------------------------------------
        void SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                title = Core.LocalizationManager.ToLocalization(80010041);
            UIUtil.SetLabel(m_Title, title);
        }
        //------------------------------------------------------
        void SetContent(string content)
        {
            m_lineCount = m_Content.cachedTextGenerator.lineCount;
            UIUtil.SetLabel(m_Content, content);
            //Debug.Log(m_Content.cachedTextGenerator.lineCount);

        }
        //------------------------------------------------------
        void SetContentTextAlignWithLineCount()
        {
            if (m_Content)
            {
                m_Content.alignment = m_Content.cachedTextGenerator.lineCount == 1 ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft;
            }
        }
        //------------------------------------------------------
        public void SetADIconActive(bool active)
        {
            if (m_ADIcon)
            {
                m_ADIcon.gameObject.SetActive(active);
            }
        }
    }
}
