/********************************************************************
生成日期:	2020-11-24
类    名: 	RedDotManager
作    者:	JaydenHe
描    述:	红点管理器
*********************************************************************/

using TopGame.Core;
using TopGame.UI;

namespace TopGame.Logic
{
    public class RedDotUtil
    {
        public static void BindBtnAndRedDot(EUIType type, Logic.RedDotType dotType)
        {
            if (GameInstance.getInstance() == null || GameInstance.getInstance().redDotManager == null)
                return;
            GameInstance.getInstance().redDotManager.OnRedDotClickedCB((int)type, (int)dotType);
        }
        //------------------------------------------------------
        public static void UpdateRedDot(EUIType type, Logic.RedDotType dotType)
        {
            if (GameInstance.getInstance() == null || GameInstance.getInstance().redDotManager == null)
                return;
            GameInstance.getInstance().redDotManager.UpdateRedDot((int)type, (int)dotType);
        }
        //------------------------------------------------------
        public static void RedDotRegister(EUIType uiType, Logic.RedDotType redDotType, ARedDotManager.IsSatisfyCondition condition, bool isIgnoreClick = false)
        {
            if (GameInstance.getInstance() == null || GameInstance.getInstance().redDotManager == null)
                return;
            GameInstance.getInstance().redDotManager.Register((int)uiType, (int)redDotType, condition, isIgnoreClick);
        }
    }
    
}