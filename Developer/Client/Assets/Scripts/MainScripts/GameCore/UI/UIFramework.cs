/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIFramework
作    者:	HappLI
描    述:	UI框架类
*********************************************************************/

using UnityEngine.UI;
using Framework.UI;

namespace TopGame.UI
{
    public interface UIFramework : UIBaseFramework, Framework.Module.IUpdate
    {
#if USE_FAIRYGUI
        FairyGUI.UIContentScaler GetUICanvasScalerRoot();
#else
        CanvasScaler GetUICanvasScalerRoot();
#endif
    }
}
