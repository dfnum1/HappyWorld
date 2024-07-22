/********************************************************************
生成日期:	10:12:2021   11:27
类    名: 	DialogPanel
作    者:	happli
描    述:	
*********************************************************************/

using TopGame.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    [UI((ushort)EUIType.DialogPanel, UI.EUIAttr.UI)]
    [Framework.Plugin.AT.ATExportMono("UI系统/对话界面")]
    public class DialogPanel : UIBase
    {
        private Data.CsvData_Dialog.DialogData m_DialogData = null;

        public bool bDialoging = false;

        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
        }
        //------------------------------------------------------
        protected override void DoShow()
        {
            base.DoShow();
            if(m_UI)
                OnDialog(m_DialogData);
        }
        //------------------------------------------------------
        protected override void DoHide()
        {
            if (m_DialogData != null)
            {
                if ( m_DialogData.bPause)
                    GameInstance.getInstance().Pause();

                if (m_DialogData.endEvents != null)
                {
                    for (int i = 0; i < m_DialogData.endEvents.Length; ++i)
                        GameInstance.getInstance().OnTriggerEvent((int)m_DialogData.endEvents[i]);
                }
            }
            m_DialogData = null;
            base.DoHide();
            bDialoging = false;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public Data.CsvData_Dialog.DialogData GetCurDialogData()
        {
            return m_DialogData;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public bool GoNextDialog()
        {
            if (m_DialogData == null)
            {
                Hide();
                return false;
            }
            Data.CsvData_Dialog.DialogData dialogData = Data.DataManager.getInstance().Dialog.GetData(m_DialogData.nNextID);
            return  OnDialog(dialogData);
        }
        //------------------------------------------------------
        bool OnDialog(Data.CsvData_Dialog.DialogData data)
        {
            if(m_DialogData != null)
            {
                if (m_DialogData.bPause)
                    GameInstance.getInstance().Resume();
                if(m_DialogData.endEvents!=null)
                {
                    for(int i = 0; i < m_DialogData.endEvents.Length; ++i)
                        GameInstance.getInstance().OnTriggerEvent((int)m_DialogData.endEvents[i]);
                }
            }
            m_DialogData = data;
            if (m_UI == null) return false;
            if(m_DialogData == null)
            {
                Hide();
                return false;
            }
            this.ExecuteCustom("DialogStart", m_DialogData);
            SetPlayerHud();
            if (m_DialogData.bPause)
                GameInstance.getInstance().Pause();

            if (m_DialogData.beiginEvents != null)
            {
                for (int i = 0; i < m_DialogData.beiginEvents.Length; ++i)
                    GameInstance.getInstance().OnTriggerEvent((int)m_DialogData.beiginEvents[i]);
            }
       //     AUserActionManager.AddRecordAction((int)Proto3.UserActionTypeCode.NewPlayer, 5, 1, 0, true);
            return true;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public void SetPlayerHud()
        {
            DialogView view = m_pView as DialogView;
            if (view == null) return;
            view.SetPlayerHud(m_DialogData);
            view.SetPlayerSay(m_DialogData);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("显示对话")]
        public static bool ShowDialog(uint dialog, float delay =0)
        {
            Data.CsvData_Dialog.DialogData dialogData = Data.DataManager.getInstance().Dialog.GetData(dialog);
            if (dialogData == null) return false;
            DialogPanel dialogUI = UIManager.CastUI<DialogPanel>(EUIType.DialogPanel, false);
            if(dialogUI!=null && dialogUI.IsVisible())
            {
                if(dialogUI.m_DialogData == dialogData)
                {
                    return true;
                }
            }
            dialogUI = UIManager.CastUI<DialogPanel>(EUIType.DialogPanel);
            if (dialogUI == null) return false;
            dialogUI.m_DialogData = dialogData;
            dialogUI.Show(delay);
            dialogUI.bDialoging = true;
            return true;
        }
        //------------------------------------------------------
        public static bool IsDialoging()
        {
            DialogPanel dialogUI = UIManager.CastUI<DialogPanel>(EUIType.DialogPanel, false);
            return dialogUI != null && dialogUI.IsVisible();
        }
    }
}
