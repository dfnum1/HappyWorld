/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	UIAnimatorGroup
作    者:	HappLI
描    述:	UI 动画
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    public enum UIAnimatorElementType
    {
        [Framework.Plugin.PluginDisplay("位置")] POSITION,
        [Framework.Plugin.PluginDisplay("旋转")] ROTATE,
        [Framework.Plugin.PluginDisplay("缩放")] SCALE,
        [Framework.Plugin.PluginDisplay("锚点")] PIVOT,
        [Framework.Plugin.PluginDisplay("颜色")] COLOR,
        [Framework.Plugin.PluginDisplay("透明度")] ALPAH,
        [Framework.Plugin.DisableGUI] COUNT
    };
    //------------------------------------------------------
    public static class UIAnimatorUtil
    {
        public static int GetElementTypeTrackCount(UIAnimatorElementType type)
        {
            switch (type)
            {
                case UIAnimatorElementType.POSITION: return 3;
                case UIAnimatorElementType.SCALE: return 3;
                case UIAnimatorElementType.ROTATE: return 3;
                case UIAnimatorElementType.PIVOT: return 2;
                case UIAnimatorElementType.ALPAH: return 1;
                case UIAnimatorElementType.COLOR: return 4;
            }
            return 0;
        }
    }
    //------------------------------------------------------
    public interface UIAnimationElement
    {
        void SetController(UnityEngine.Object pPointer);
        UnityEngine.Object GetController();
        UIAnimatorElementType GetEleType();
        bool IsValidTrack();
        void Start(bool bLocal=false);
        void End(bool bRecover);
        void Update(float fRuntime);
    }
}

