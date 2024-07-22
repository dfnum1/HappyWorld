/********************************************************************
生成日期:	2020-6-16
类    名: 	BuildingStateMarker
作    者:	happli
描    述:	建筑标记
*********************************************************************/
using Framework.Core;
using Framework.Plugin.AT;
using UnityEngine;
namespace TopGame.Logic
{
    public class BuildingStateMarker : Framework.Core.AInstanceAble
    {
        public static System.Action<BuildingStateMarker> OnClick;
        public static System.Action<BuildingStateMarker> OnRecyled;
        public enum ELocation
        {
            Bottom = 0,
            Top,
        }
        public bool CityPanelRoot = true;
        public ELocation location = ELocation.Bottom;
#if UNITY_EDITOR
        [Framework.ED.DisplayDrawType("TopGame.Logic.EBuidingMarkerType")]
#endif
        public int markerType = 0;
        public UI.UISerialized uiSerializer;
        public int sortingOrder = 0;

        public float lod = -1;
        public GameObject[] lodControllers;
        public GameObject[] lodShowControllers;

        [Framework.Data.DisplayNameGUI("点击特效"), Framework.Data.StringViewGUI(typeof(GameObject))]
        public string clickEffect = "";
        public float clickEffectScale = 1;

        public Framework.Plugin.Guide.GuideGuid guide;

        private IUserData m_pUserData = null;
        public IUserData userData
        {
            get
            {
                return m_pUserData;
            }
            set
            {
                m_pUserData = value;
            }
        }
        //------------------------------------------------------
        void Start()
        {
            if (uiSerializer && uiSerializer.Widgets != null)
            {
                for (int i = 0; i < uiSerializer.Widgets.Length; ++i)
                {
                    UI.EventTriggerListener listener = uiSerializer.Widgets[i].widget as UI.EventTriggerListener;
                    if (listener)
                    {
                        listener.onClickEvent += OnClickBtn;
                    }
                }
            }
        }
        //------------------------------------------------------
        void OnClickBtn(GameObject go, UnityEngine.EventSystems.BaseEventData evtData, params VariablePoolAble[] param)
        {
            if (OnClick != null) OnClick(this);
        }
        //------------------------------------------------------
        public override void OnRecyle()
        {
            if (OnRecyled != null) OnRecyled(this);
            m_pUserData = null;
            base.OnRecyle();
        }
    }
}
