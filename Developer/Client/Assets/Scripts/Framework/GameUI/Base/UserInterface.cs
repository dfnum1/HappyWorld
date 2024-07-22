/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	UserInterface
作    者:	HappLI
描    述:	UI 界面
*********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UserInterface : UISerialized
    {
        [System.NonSerialized]
        public ushort uiType = 0;

        [Framework.Data.DisplayNameGUI("图形脚本")]
        public Framework.Plugin.AT.AgentTreeCoreData ATData;


        [System.NonSerialized]
        protected IUIEventLogic m_uLogic;

        [System.NonSerialized]
        public Action<bool> OnApplicationPauseCB;
        //------------------------------------------------------
        public override void SetEventLogic(IUIEventLogic pLogic)
        {
            m_uLogic = pLogic;
        }
        //------------------------------------------------------
        public override IUIEventLogic GetEventLogic()
        {
            return m_uLogic;
        }
        //------------------------------------------------------
        protected override bool DoUIEvent(UIEventType type)
        {
            if ((int)type >= UIEventDatas.Length) return false;
            UIEventData data = UIEventDatas[(int)type];
            if (data == null) return false;
#if !USE_FMOD
            if (data.Audio != null) { Core.AudioManager.PlayEffect(data.Audio); }
#endif
            return m_uLogic.ExcudeEvent(transform, type, data);
        }
        //------------------------------------------------------
        public override bool OnShow()
        {
            bool bEvent = DoUIEvent(UIEventType.Show);
            return bEvent;
        }
        //------------------------------------------------------
        public override bool OnHide()
        {
            bool bEvent = DoUIEvent(UIEventType.Hide);
            return bEvent;
        }
        //------------------------------------------------------
        public override bool OnMoveOut()
        {
            bool bEvent = DoUIEvent(UIEventType.MoveOut);
            return bEvent;
        }
        //------------------------------------------------------
        public override bool OnMoveIn()
        {
            bool bEvent = DoUIEvent(UIEventType.MoveIn);
            return bEvent;
        }
        //------------------------------------------------------
        private void OnApplicationPause(bool pause)
        {
            OnApplicationPauseCB?.Invoke(pause);
        }
        //------------------------------------------------------
        public override void Visible()
        {
        }
        //------------------------------------------------------
        public override void Hidden()
        {
        }
    }


}
