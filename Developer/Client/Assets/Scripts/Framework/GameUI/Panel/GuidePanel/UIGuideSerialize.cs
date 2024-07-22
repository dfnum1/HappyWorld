/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UISerialize
作    者:	HappLI
描    述:	UI 序列化对象容器，用于界面绑定操作对象
*********************************************************************/

using Framework.Plugin.Guide;
using UnityEngine;

namespace TopGame.UI
{
    public class UIGuideSerialize : UserInterface
    {
        public Transform[] Fingers = new Transform[4]; //EFingerType.None
        public Transform[] DescBgs = new Transform[1]; //EDescBGType.None
        public Framework.Plugin.Guide.GuideHighlightMask BgMask;
        public UIPenetrate uiPenetrate;
        public Transform TargetContainer;
        public UnityEngine.UI.Button VirtualButton;
        public UnityEngine.UI.Text TipLabel;
        public UnityEngine.RectTransform DialogArrow;
        public UnityEngine.UI.Image GuideImage;
        public UnityEngine.UI.Text GuideText;
        public UnityEngine.UI.Image ContinueImage;
        public UnityEngine.UI.Image SkipBtn;
        public UnityEngine.RectTransform SimulationClickImage;
        public UnityEngine.UI.Image Avatar;
    }
}
