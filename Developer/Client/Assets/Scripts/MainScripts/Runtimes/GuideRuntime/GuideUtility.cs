
using System.Collections.Generic;
using TopGame.UI;
using UnityEngine;
namespace Framework.Plugin.Guide
{
    public class GuideUtility
    {
        public static bool IsCheckInView(Transform pObj)
        {
            if(pObj is RectTransform)
            {
                RectTransform rectTrans = pObj as RectTransform;
                var uiFramework = TopGame.UI.UIKits.GetUIFramework();
                if(uiFramework != null)
                {
                    return uiFramework.IsInView(rectTrans);
                }
            }
            else
            {
                TopGame.Core.ICameraController camera = TopGame.Core.CameraController.getInstance();
                if(camera!=null)
                {
                    if (camera.IsInView(pObj.position)) return true;
                }
            }
            return false;
        }
        //------------------------------------------------------
        public static bool IsCheckTweening(Transform pObj)
        {
            return IsRecursiveCheckTweening(pObj);
        }
        //------------------------------------------------------
        static bool IsRecursiveCheckTweening(Transform pObj)
        {
            if (pObj == null) return false;

            if (TopGame.RtgTween.RtgTweenerManager.getInstance().IsTweening(pObj))
                return true;

            List<DG.Tweening.Tween> vTweens = DG.Tweening.DOTween.TweensByTarget(pObj);
            if (vTweens != null && vTweens.Count > 0) return true;

            if (UIAnimatorFactory.getInstance().IsTweening(pObj))
                return true;
                
            return IsRecursiveCheckTweening(pObj.parent);
        }
    }
}
