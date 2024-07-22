
using UnityEngine;
namespace Framework.Plugin.Guide
{
    public enum EGuideCustomType
    {
        None = 0,
        NetCallback = 1,
        AnimPathCallback = 2,
        WaitPanelOpen = 3,
        WaitTweenCompleted = 4,
        WaitPanelClose = 5,
    }
    public enum EFingerType
    {
        [GuideDisplay("点击手势")]
        Click = 0,
        [GuideDisplay("滑动手势")]
        Slide,
        [GuideDisplay("箭头手势")]
        Arrow,
        [GuideDisplay("无手势,有特效")]
        Effect,
        [GuideDisplay("无手势")]
        None,
    }

    public enum EDescBGType
    {
        [GuideDisplay("背景框")]
        Box = 0,
        [GuideDisplay("无背景")]
        None,
    }

    public enum EMaskType
    {
        [GuideDisplay("无")]
        None = 0,
        [GuideDisplay("方框")]
        Box ,
        [GuideDisplay("圆形")]
        Circel,
        [GuideDisplay("菱形")]
        Diamond,
        [GuideDisplay("圆形(小)")]
        SmallCircel,
    }

    public enum EModeBit
    {
        [GuideDisplay("关键步骤(服务器记录)")]
        MasterStep = 1 << 0,
        [GuideDisplay("自动跳转到下一步骤")]
        AutoNext = 1 << 1,
        [GuideDisplay("自动执行")]
        AutoAction = 1 << 2,
    }
}
