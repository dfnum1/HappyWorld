/********************************************************************
生成日期:	6:8:2020 10:42
类    名: 	UIPreTip
作    者:	Happli
描    述:	预提示
*********************************************************************/

using Framework.Core;
using System;
using TopGame.Core;
using UnityEngine;

namespace TopGame.UI
{
    public class UIPreTip : MonoBehaviour
    {
        static UIPreTip ms_pInstance = null;
        public UI.EventTriggerListener checkBtn;
        public LocalizationText tipText;
        private Action m_CheckCallback = null;
        private bool m_bShow = false;

        //------------------------------------------------------
        public static void InitInstance(UIPreTip instnace)
        {
            if (instnace == null) return;
            ms_pInstance = instnace;
            instnace.checkBtn.onClick += instnace.OnButtonClick;
            instnace.m_bShow = false;
            instnace.m_CheckCallback = null;
            //    DontDestroyOnLoad(this);
            Hide();
            instnace.gameObject.SetActive(false);
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_pInstance = null;
        }
        //------------------------------------------------------
        void OnButtonClick(GameObject go, params VariablePoolAble[] param)
        {
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                return;
            }
            if (m_CheckCallback!=null)
            {
                m_CheckCallback();
            }
            Hide();
        }
        //------------------------------------------------------
        public static void Show(Action OnCallback, uint localTextId=0)
        {
            if (ms_pInstance == null) return;
            if (ms_pInstance.m_bShow)
                return;
            ms_pInstance.m_CheckCallback = OnCallback;
            ms_pInstance.m_bShow = true;
            ms_pInstance.gameObject.SetActive(true);
            if (localTextId != 0 && ms_pInstance.tipText)
                ms_pInstance.tipText.UpdateLocalization(localTextId);
            if(ms_pInstance.checkBtn != null) ms_pInstance.checkBtn.gameObject.SetActive(OnCallback != null);
        }
        //------------------------------------------------------
        public static void Hide()
        {
            if (ms_pInstance == null) return;
            if (!ms_pInstance.m_bShow)
                return;
            ms_pInstance.m_bShow = false;
            ms_pInstance.gameObject.SetActive(false);
            ms_pInstance.m_CheckCallback = null;
        }
    }
}
