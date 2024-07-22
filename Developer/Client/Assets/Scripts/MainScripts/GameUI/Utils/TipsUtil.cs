/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	TipsUtil
作    者:	HappLI
描    述:	UI 弹窗提示
*********************************************************************/

using UnityEngine;

namespace TopGame.UI
{
    public enum ETipAction
    {
        Confirm,
        Cancel,
        Close,
        ResCheck,
    }
    //------------------------------------------------------
    public enum ETipType
    {
        Yes,
        Yes_No,
        Yes_No_Close,
        Yes_Close,
        Close,
        AutoHide,
    }
    public class TipsUtil
    {
        public static void ShowCommonTip(ETipType type, uint text, System.Action<ETipAction> onCallback = null, uint titleText = 80010041, uint yesBtnText = 80010015, uint noBtnText = 80010014)
        {
            UICommonTip tipUI = UIKits.CastGetUI<UICommonTip>();
            if (tipUI != null)
            {
                tipUI.ShowTip(type, Core.LocalizationManager.ToLocalization(text), Core.LocalizationManager.ToLocalization(titleText), onCallback, Core.LocalizationManager.ToLocalization(yesBtnText), Core.LocalizationManager.ToLocalization(noBtnText));
            }
        }
        //--------------------------------------------------------
        public static void ShowCommonTip(ETipType type, string text, System.Action<ETipAction> onCallback = null, uint titleText = 80010041, uint yesBtnText = 80010015, uint noBtnText = 80010014)
        {
            UICommonTip tipUI = UIKits.CastGetUI<UICommonTip>();
            if (tipUI != null)
            {
                tipUI.ShowTip(type, text, Core.LocalizationManager.ToLocalization(titleText), onCallback, Core.LocalizationManager.ToLocalization(yesBtnText), Core.LocalizationManager.ToLocalization(noBtnText));
            }
        }
        //--------------------------------------------------------
        public static void ShowItemTip(uint itemId, Vector3 position)
        {

        }
    }
}
