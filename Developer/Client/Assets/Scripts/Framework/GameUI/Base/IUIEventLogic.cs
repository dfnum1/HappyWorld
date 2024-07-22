/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	IUIEventLogic
作    者:	HappLI
描    述:	
*********************************************************************/

using UnityEngine;
namespace TopGame.UI
{
    //------------------------------------------------------
    public enum UIEventType
    {
        [Framework.Plugin.PluginDisplay("显示")] Show,
        [Framework.Plugin.PluginDisplay("隐藏")] Hide,
        [Framework.Plugin.PluginDisplay("移出屏")] MoveOut,
        [Framework.Plugin.PluginDisplay("移进屏")] MoveIn,
        [Framework.Plugin.DisableGUI] Count,
    }
    //------------------------------------------------------
    [System.Serializable]
    public class UIEventData
    {
        public int animationID;
        public RtgTween.TweenerGroup tweenGroup;
        public Transform ApplyRoot;

        public AnimationClip Animation;
#if USE_FMOD
        public FMODUnity.EventReference fmodEvent;
#else
        public AudioClip Audio;
#endif
        public bool bReverse;
        public float speedScale = 1;
    }
    //------------------------------------------------------
    public interface IUIEventLogic
    {
        bool ExcudeEvent(Transform pTransform, UIEventType eventType, UIEventData uiEvent);
    }
}
