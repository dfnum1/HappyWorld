/********************************************************************
生成日期:	17:9:2019   16:19
类    名: 	UIKits
作    者:	HappLI
描    述:	UI全局通用工具集
*********************************************************************/
using Framework.Core;
using System;
using TopGame.Base;
using UnityEngine;

namespace TopGame.UI
{
    public enum EFitScreenType
    {
        AutoFull = 0,
        AutoNearly,
        FitWidth,
        FitHeight,
    }
    public static class UIKits
    {
        //-----------------------------------------------------
        public static Framework.UI.UIBaseFramework GetUIFramework()
        {
            Framework.UI.UIBaseFramework pThis = null;
            if (Framework.Module.ModuleManager.mainModule != null)
            {
                pThis = Framework.Module.ModuleManager.mainModule.uiFramework;
            }
            return pThis;
        }
        //-----------------------------------------------------
        public static bool IsShowUI(int uiType) 
        {
            if (uiType <= 0) return false;
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return false;
            return pThis.IsShow((ushort)uiType);
        }
        //-----------------------------------------------------
        public static T ShowUI<T>(int uiType = 0) where T : Framework.UI.UIHandle
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return null;
            return pThis.CastShowUI<T>((ushort)uiType);
        }
        //-----------------------------------------------------
        public static Framework.UI.UIHandle ShowUI(int uiType)
        {
            if (uiType <= 0) return null;
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return null;
            return pThis.ShowUI((ushort)uiType);
        }
        //-----------------------------------------------------
        public static void HideUI(int uiType)
        {
            if (uiType <= 0) return;
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return;
            pThis.HideUI((ushort)uiType);
        }
        //-----------------------------------------------------
        public static T CastGetUI<T>(bool bAuto = true, int uiType = 0) where T : Framework.UI.UIHandle
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return null;
            return pThis.CastGetUI<T>((ushort)uiType, bAuto);
        }
        //-----------------------------------------------------
        public static void SetScreenOrientation(ScreenOrientation orientation)
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return;
            pThis.SetScreenOrientation(orientation);
        }
#if USE_FAIRYGUI
        //-----------------------------------------------------
        public static Transform GetUICanvasRoot()
        {
            Framework.UI.UIBaseFramework pThis= GetUIFramework();
            if (pThis == null) return null;
            return pThis.GetUICanvasRoot();
        }
        //-----------------------------------------------------
        public static Transform GetDynamicUIRoot()
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return null;
            return pThis.GetDynamicUIRoot();
        }
        //-----------------------------------------------------
        public static Transform GetStaticUIRoot()
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return null;
            return pThis.GetStaticUIRoot();
        }
        //-----------------------------------------------------
        public static Transform GetAutoUIRoot()
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return null;
            Transform root = pThis.GetDynamicUIRoot();
            if (root == null) root = pThis.GetStaticUIRoot();
            return root;
        }
#else
        //-----------------------------------------------------
        public static Canvas GetUICanvasRoot()
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return null;
            return pThis.GetUICanvasRoot();
        }
        //-----------------------------------------------------
        public static RectTransform GetDynamicUIRoot(int order =0)
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return null;
            return pThis.GetDynamicUIRoot(order);
        }
        //-----------------------------------------------------
        public static RectTransform GetStaticUIRoot()
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return null;
            return pThis.GetStaticUIRoot();
        }
        //-----------------------------------------------------
        public static RectTransform GetAutoUIRoot(int order =0)
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return null;
            RectTransform root = pThis.GetDynamicUIRoot(order);
            if (root == null) root = pThis.GetStaticUIRoot();
            return root;
        }
#endif
        //-----------------------------------------------------
        public static bool IsInView(Transform transform)
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return false;
            return pThis.IsInView(transform);
        }
        //-----------------------------------------------------
        public static Camera GetUICamera()
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return null;
            return pThis.GetUICamera();
        }
        //-----------------------------------------------------
        public static void SetAnchor(this RectTransform source, AnchorPresets allign, int offsetX = 0, int offsetY = 0)
        {
            source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

            switch (allign)
            {
                case (AnchorPresets.TopLeft):
                    {
                        source.anchorMin = new Vector2(0, 1);
                        source.anchorMax = new Vector2(0, 1);
                        break;
                    }
                case (AnchorPresets.TopCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 1);
                        source.anchorMax = new Vector2(0.5f, 1);
                        break;
                    }
                case (AnchorPresets.TopRight):
                    {
                        source.anchorMin = new Vector2(1, 1);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }

                case (AnchorPresets.MiddleLeft):
                    {
                        source.anchorMin = new Vector2(0, 0.5f);
                        source.anchorMax = new Vector2(0, 0.5f);
                        break;
                    }
                case (AnchorPresets.MiddleCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0.5f);
                        source.anchorMax = new Vector2(0.5f, 0.5f);
                        break;
                    }
                case (AnchorPresets.MiddleRight):
                    {
                        source.anchorMin = new Vector2(1, 0.5f);
                        source.anchorMax = new Vector2(1, 0.5f);
                        break;
                    }

                case (AnchorPresets.BottomLeft):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(0, 0);
                        break;
                    }
                case (AnchorPresets.BottonCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0);
                        source.anchorMax = new Vector2(0.5f, 0);
                        break;
                    }
                case (AnchorPresets.BottomRight):
                    {
                        source.anchorMin = new Vector2(1, 0);
                        source.anchorMax = new Vector2(1, 0);
                        break;
                    }

                case (AnchorPresets.HorStretchTop):
                    {
                        source.anchorMin = new Vector2(0, 1);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }
                case (AnchorPresets.HorStretchMiddle):
                    {
                        source.anchorMin = new Vector2(0, 0.5f);
                        source.anchorMax = new Vector2(1, 0.5f);
                        break;
                    }
                case (AnchorPresets.HorStretchBottom):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(1, 0);
                        break;
                    }

                case (AnchorPresets.VertStretchLeft):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(0, 1);
                        break;
                    }
                case (AnchorPresets.VertStretchCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0);
                        source.anchorMax = new Vector2(0.5f, 1);
                        break;
                    }
                case (AnchorPresets.VertStretchRight):
                    {
                        source.anchorMin = new Vector2(1, 0);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }

                case (AnchorPresets.StretchAll):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }
            }
        }
        //-----------------------------------------------------
        public static void SetPivot(this RectTransform source, PivotPresets preset)
        {

            switch (preset)
            {
                case (PivotPresets.TopLeft):
                    {
                        source.pivot = new Vector2(0, 1);
                        break;
                    }
                case (PivotPresets.TopCenter):
                    {
                        source.pivot = new Vector2(0.5f, 1);
                        break;
                    }
                case (PivotPresets.TopRight):
                    {
                        source.pivot = new Vector2(1, 1);
                        break;
                    }

                case (PivotPresets.MiddleLeft):
                    {
                        source.pivot = new Vector2(0, 0.5f);
                        break;
                    }
                case (PivotPresets.MiddleCenter):
                    {
                        source.pivot = new Vector2(0.5f, 0.5f);
                        break;
                    }
                case (PivotPresets.MiddleRight):
                    {
                        source.pivot = new Vector2(1, 0.5f);
                        break;
                    }

                case (PivotPresets.BottomLeft):
                    {
                        source.pivot = new Vector2(0, 0);
                        break;
                    }
                case (PivotPresets.BottomCenter):
                    {
                        source.pivot = new Vector2(0.5f, 0);
                        break;
                    }
                case (PivotPresets.BottomRight):
                    {
                        source.pivot = new Vector2(1, 0);
                        break;
                    }
            }
        }
        //------------------------------------------------------
        public static bool WorldPosToUIPos(Vector3 worldPos, bool bLocal, ref Vector3 point, Camera cam = null)
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return false;
            return pThis.ConvertWorldPosToUIPos(worldPos, bLocal, ref point, cam);
        }
        //------------------------------------------------------
        public static bool ScreenPosToUIPos(Vector3 screenPos, bool bLocal, ref Vector3 point)
        {
            Framework.UI.UIBaseFramework pThis = GetUIFramework();
            if (pThis == null) return false;
            return pThis.ConvertScreenToUIPos(screenPos, bLocal, ref point);
        }
        //------------------------------------------------------
        public static float GetCanvasScaler()
        {
            if (Framework.Module.ModuleManager.mainModule != null)
            {
                Framework.UI.UIBaseFramework pThis = GetUIFramework();
                if (pThis != null && pThis.GetUICanvasScaler())
                {
                    UnityEngine.UI.CanvasScaler canvaScaler = pThis.GetUICanvasScaler() as UnityEngine.UI.CanvasScaler;
                    Vector2 referenceResolution = canvaScaler?canvaScaler.referenceResolution: new Vector2(1334, 750);
                    UnityEngine.UI.CanvasScaler.ScreenMatchMode matchMode = canvaScaler ? canvaScaler.screenMatchMode : UnityEngine.UI.CanvasScaler.ScreenMatchMode.Expand;
                    Vector2 screenSize = new Vector2(Screen.width, Screen.height);
                    float scaleFactor = 1;
                    switch (matchMode)
                    {
                        case UnityEngine.UI.CanvasScaler.ScreenMatchMode.MatchWidthOrHeight:
                            {
                                // We take the log of the relative width and height before taking the average.
                                // Then we transform it back in the original space.
                                // the reason to transform in and out of logarithmic space is to have better behavior.
                                // If one axis has twice resolution and the other has half, it should even out if widthOrHeight value is at 0.5.
                                // In normal space the average would be (0.5 + 2) / 2 = 1.25
                                // In logarithmic space the average is (-1 + 1) / 2 = 0
                                float logWidth = Mathf.Log(screenSize.x / referenceResolution.x, 2);
                                float logHeight = Mathf.Log(screenSize.y / referenceResolution.y, 2);
                                float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, canvaScaler?canvaScaler.matchWidthOrHeight:1);
                                scaleFactor = Mathf.Pow(2, logWeightedAverage);
                                break;
                            }
                        case UnityEngine.UI.CanvasScaler.ScreenMatchMode.Expand:
                            {
                                scaleFactor = Mathf.Min(screenSize.x / referenceResolution.x, screenSize.y / referenceResolution.y);
                                break;
                            }
                        case UnityEngine.UI.CanvasScaler.ScreenMatchMode.Shrink:
                            {
                                scaleFactor = Mathf.Max(screenSize.x / referenceResolution.x, screenSize.y / referenceResolution.y);
                                break;
                            }
                    }
                    return scaleFactor;
                }
            }
            return 1;
        }
        //------------------------------------------------------
        public static float ScaleWithScreenSize(EFitScreenType eType)
        {
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 ReferenceResolution = new Vector2(1334, 750);
            if (Framework.Module.ModuleManager.mainModule != null)
            {
                Framework.UI.UIBaseFramework pThis = GetUIFramework();
                if (pThis != null && pThis.GetUICanvasScaler())
                {
                    UnityEngine.UI.CanvasScaler canvaScaler = pThis.GetUICanvasScaler() as UnityEngine.UI.CanvasScaler;
                    if (canvaScaler) ReferenceResolution = canvaScaler.referenceResolution;
                }
            }
            float fMatchWidthOrHeight = 1;
            if (eType == EFitScreenType.FitWidth) fMatchWidthOrHeight = 0;

            float logWidth = screenSize.x / ReferenceResolution.x;
            float logHeight = screenSize.y / ReferenceResolution.y;
            float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, fMatchWidthOrHeight);
            float scaleFactor = logWeightedAverage;
            return scaleFactor;
        }
        //------------------------------------------------------
        public static float ScaleWithScreenSize(EFitScreenType eType, ref Vector2 size)
        {
            Vector2 screenSize = GetReallyResolution();// new Vector2(Screen.width, Screen.height);
            float ratio = 1;

            if (eType == UI.EFitScreenType.AutoNearly || eType == UI.EFitScreenType.AutoFull)
            {
                float ratioX = screenSize.x / size.x;
                float ratioY = screenSize.y / size.y;

                float gapH = Mathf.Abs(Screen.height - ratioX * size.y);
                float gapW = Mathf.Abs(Screen.width - ratioY * size.x);

                if (eType == UI.EFitScreenType.AutoNearly)
                {
                    if (gapW < gapH)
                        ratio = ratioY;
                    else ratio = ratioX;
                }
                else
                {
                    if (gapW > gapH)
                        ratio = ratioY;
                    else ratio = ratioX;
                }
            }
            else
            {
                if (eType == EFitScreenType.FitWidth)
                    ratio = screenSize.x / size.x;
                else if (eType == EFitScreenType.FitHeight)
                    ratio = screenSize.y / size.y;
            }
            size.x = ratio * size.x;
            size.y = ratio * size.y;
            return ratio;
        }
        //------------------------------------------------------
        public static void ScaleWithScreenScale(ref Vector3 scale)
        {
            Vector2 screenSize = GetReallyResolution();// new Vector2(Screen.width, Screen.height);
            Vector2 ReferenceResolution = new Vector2(750, 1334);
            if (Framework.Module.ModuleManager.mainModule != null)
            {
                Framework.UI.UIBaseFramework pThis = GetUIFramework();
                if (pThis != null && pThis.GetUICanvasScaler())
                {
                    UnityEngine.UI.CanvasScaler canvaScaler = pThis.GetUICanvasScaler() as UnityEngine.UI.CanvasScaler;
                    if (canvaScaler) ReferenceResolution = canvaScaler.referenceResolution;
                }
            }
            scale = Vector3.one * Mathf.Max(ReferenceResolution.x / screenSize.x, ReferenceResolution.y / screenSize.y);
        }
        //------------------------------------------------------
        public static Vector2 GetReallyResolution()
        {
            Vector2 ReferenceResolution = new Vector2(750, 1334);
            if (Framework.Module.ModuleManager.mainModule != null)
            {
                Framework.UI.UIBaseFramework pThis = GetUIFramework();
                if (pThis != null && pThis.GetUICanvasScaler())
                {
                    UnityEngine.UI.CanvasScaler canvaScaler = pThis.GetUICanvasScaler() as UnityEngine.UI.CanvasScaler;
                    if (canvaScaler) ReferenceResolution = canvaScaler.referenceResolution;
                }
            }
            return ReferenceResolution;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public static bool PlayTween(UI.UISerialized uiSerializeds, bool bRewind)
        {
            if (uiSerializeds == null || uiSerializeds.Widgets == null) return false;
            bool bHasTween = false;
            for (int i = 0; i < uiSerializeds.Widgets.Length; ++i)
            {
                DG.Tweening.DOTweenAnimation tween = uiSerializeds.Widgets[i].widget as DG.Tweening.DOTweenAnimation;
                if (tween is DG.Tweening.DOTweenAnimation)
                {
                    DG.Tweening.DOTweenAnimation doTween = (DG.Tweening.DOTweenAnimation)tween;
                    if (bRewind) doTween.DOPlayBackwards();
                    else doTween.DOPlayForward();
                    bHasTween = true;
                }
            }
            return bHasTween;
        }
    }
}