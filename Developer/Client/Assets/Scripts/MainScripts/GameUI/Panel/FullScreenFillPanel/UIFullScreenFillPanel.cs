/********************************************************************
生成日期:	2020-6-16
类    名: 	UIFullScreenFillPanel
作    者:	zdq
描    述:	UIFullScreenFillPanel界面
//设置遮罩颜色
//分两个阶段,开始阶段和结算阶段,当加载完,收到通知后再播放结算阶段渐变
*********************************************************************/

using Framework.Core;
using Framework.Plugin.AT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TopGame.UI
{
    public interface IFullScreenFillBlack
    {
        void OnFullScreenFillBlack(VariablePoolAble userParam);
    }

    [ATExportMono("UI系统/全屏填充界面/界面")]
    [UI((ushort)EUIType.FullScreenFillPanel, UI.EUIAttr.UI)]
    public class UIFullScreenFillPanel : UIBase
    {
        struct Callback
        {
            public IFullScreenFillBlack callback;
            public VariablePoolAble userParam;
            public Callback(IFullScreenFillBlack callback, VariablePoolAble userParam)
            {
                this.callback = callback;
                this.userParam = userParam;
            }
        }
        public enum State
        {
            None,
            Begin,
            End
        }
        UIFullScreenFillView m_view = null;
        State state = State.None;
        float BeginTweenTime = 0.7f;
        float EndTweenTime = 0.85f;
        float m_Timer = 0f;

        List<Callback> m_vCallbacks = null;

        public System.Action OnHideCallBack;
        int m_OriginOrderValue = 0;
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();

            m_view = m_pView as UIFullScreenFillView;
            m_OriginOrderValue = GetOrder();
        }
        //------------------------------------------------------
        protected override void DoShow()
        {
            base.DoShow();
            
            Dictionary<ushort, UIBase> uis = GameInstance.getInstance().uiManager.GetUIS();
            foreach(var db in uis)
            {
                if (!db.Value.IsVisible()) continue;
                if(db.Value is IFullScreenFillBlack)
                {
                    AddCallbck(db.Value as IFullScreenFillBlack, null );
                }
            }
        }
        //------------------------------------------------------
        public override void Hide()
        {
            if(state == State.Begin)
            {
                if (m_Timer <= 0) m_Timer = 0.7f;
                state = State.End;
                return;
            }
            base.Hide();
            m_Timer = 0f;
            state = State.None;

            if (m_vCallbacks != null) m_vCallbacks.Clear();
            SetOrder(m_OriginOrderValue);//关闭界面时,恢复原本层级

            OnHideCallBack?.Invoke();
        }
        //------------------------------------------------------
        public override bool CanHide()
        {
            return state == State.None;
        }
        //------------------------------------------------------
        public void AddCallbck(IFullScreenFillBlack call_back, VariablePoolAble userParam = null)
        {
            if (call_back == null) return;
            if (m_vCallbacks == null) m_vCallbacks = new List<Callback>(2);
            Callback callBack;
            for(int i =0; i < m_vCallbacks.Count; ++i)
            {
                callBack = m_vCallbacks[i];
                if (callBack.callback == call_back)
                {
                    callBack.userParam = userParam;
                    return;
                }
            }
            callBack = new Callback(call_back, userParam);
            m_vCallbacks.Add(callBack);
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            base.InnerUpdate(fFrame);
            if (state == State.None || m_view == null)
            {
                return;
            }
            m_Timer += fFrame;
            //Framework.Plugin.Logger.Warning("m_Timer:" + m_Timer);
            switch (state)
            {
                case State.Begin:
                    {
                        if (m_Timer < BeginTweenTime)
                        {
                            m_view.SetAlpha(Mathf.Lerp(0, 1, m_Timer / BeginTweenTime));
                        }
                        if (m_Timer >= BeginTweenTime)
                        {
                            m_view.SetAlpha(1);
                            if(m_vCallbacks!=null && m_vCallbacks.Count>0)
                            {
                                Callback callBack;
                                for(int i = 0; i < m_vCallbacks.Count; ++i)
                                {
                                    callBack = m_vCallbacks[i];
                                    callBack.callback.OnFullScreenFillBlack(callBack.userParam);
                                }
                                m_vCallbacks.Clear();
                            }
                        }
                    }
                    break;
                case State.End:
                    {
                        if (m_Timer < EndTweenTime)
                        {
                            m_view.SetAlpha(Mathf.Lerp(1, 0, m_Timer / EndTweenTime));
                        }
                        if (m_Timer >= EndTweenTime)
                        {
                            m_view.SetAlpha(0);
                            Hide();
                        }
                    }
                    break;
            }
        }
        //------------------------------------------------------
        [ATMethod("设置渐变颜色")]
        public void SetFillColor(Color color)
        {
            if (m_view != null)
            {
                m_view.SetFillColor(color);
            }
        }
        //------------------------------------------------------
        [ATMethod("开始渐变")]
        public void SetBeginTween()
        {
            m_Timer = 0f;
            state = State.Begin;
        }
        //------------------------------------------------------
        [ATMethod("开始渐变(指定时间)")]
        public void SetBeginTween(float time)
        {
            if (time >= 0)
            {
                BeginTweenTime = time;
            }
            
            SetBeginTween();
        }
        //------------------------------------------------------
        [ATMethod("结束渐变")]
        public void SetEndTween()
        {
            m_Timer = 0;
            state = State.End;
        }
        //------------------------------------------------------
        [ATMethod("结束渐变(指定时间)")]
        public void SetEndTween(float time)
        {
            if (time >= 0)
            {
                EndTweenTime = time;
            }
            SetEndTween();
        }
        //------------------------------------------------------
        [ATMethod("获取当前状态")]
        public State GetState()
        {
            return state;
        }
    }
}

