/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UISystem
作    者:	HappLI
描    述:	UISystem
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    public class UISystem : MonoBehaviour
    {
        public string uiConfig = "Assets/Datas/Config/UIConfig.asset";
#if USE_FAIRYGUI
        public Camera uiCamera;
        public Transform CanvasRoot;
        public Transform Root;
        public Transform DynamicRoot;
        public FairyGUI.UIContentScaler contentScaler;
        public FairyGUI.StageCamera stageCamera;
#else
        public Canvas rootCanvas;
        public RectTransform Root;
        public RectTransform[] DynamicRoot;
        public UnityEngine.UI.CanvasScaler contentScaler;
        public Camera uiCamera;
#endif
    }
}

