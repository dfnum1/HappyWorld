/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	UISerialized
作    者:	HappLI
描    述:	UI 界面序列化
*********************************************************************/

using Framework.Core;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;

namespace TopGame.UI
{
    [Framework.Plugin.AT.ATExportMono("序列化/UI组件序列化器")]
    public class UISerialized : ComSerialized, UIAnimatorCallback
    {
#if UNITY_EDITOR
        [System.NonSerialized]
        public string PrefabAsset = "";
#endif

        public UIEventData[] UIEventDatas;
        public GameObject[] Elements;

        //   public string[] innerAssetRefs;

        [System.NonSerialized]
        UIAnimatorPlayable m_pUIAnimatoring = null;

        Dictionary<string, GameObject> m_vFinder = null;
        //------------------------------------------------------
        protected override void OnSerialized()
        {
            if (Elements != null && Elements.Length > 0)
            {
                m_vFinder = new Dictionary<string, GameObject>(Elements.Length);
                for (int i = 0; i < Elements.Length; ++i)
                {
                    if (Elements[i] == null) continue;
                    if (m_vFinder.ContainsKey(Elements[i].name)) continue;
                    m_vFinder.Add(Elements[i].name, Elements[i]);
                }
            }
        }
        ////------------------------------------------------------
        //private void Start()
        //{
        //    if (innerAssetRefs != null)
        //    {
        //        for (int i = 0; i < innerAssetRefs.Length; ++i)
        //        {
        //           FileSystemUtil.InnerAssetGrab(innerAssetRefs[i]);
        //        }
        //    }
        //}
        ////------------------------------------------------------
        //private void OnDestroy()
        //{
        //    if (innerAssetRefs != null)
        //    {
        //        for (int i = 0; i < innerAssetRefs.Length; ++i)
        //        {
        //           FileSystemUtil.InnerAssetRelease(innerAssetRefs[i]);
        //        }
        //    }
        //}
        //------------------------------------------------------
        public virtual void SetEventLogic(IUIEventLogic pLogic) { }
        //------------------------------------------------------
        public virtual IUIEventLogic GetEventLogic() { return null; }
        void ClearAnimation()
        {
            if (m_pUIAnimatoring != null)
            {
                UIAnimatorFactory.getInstance().RemovePlayAble(m_pUIAnimatoring, false);
                m_pUIAnimatoring = null;
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public virtual void Visible()
        {
            gameObject.SetActive(true);
            ClearAnimation();
            if(this is UserInterface)
            {
                return;
            }
            UIEventData evtData = GetEventData(UIEventType.Show);
            if (evtData != null)
            {
                if (evtData.tweenGroup != null)
                    evtData.tweenGroup.Play((short)evtData.animationID);
                else if (evtData.animationID > 0)
                {
                    m_pUIAnimatoring = UIAnimatorFactory.getInstance().CreatePlayAble(evtData.animationID, evtData.ApplyRoot ? evtData.ApplyRoot : transform);
                    if (m_pUIAnimatoring != null)
                    {
                        m_pUIAnimatoring.SetUserData(new Variable1() { intVal = 1 });
                        m_pUIAnimatoring.AddCallback(this);
                        m_pUIAnimatoring.SetSpeedScale(evtData.speedScale);
                        m_pUIAnimatoring.SetReverse(evtData.bReverse);
                        m_pUIAnimatoring.Start();
                    }
                }
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod]
        public virtual void Hidden()
        {
            ClearAnimation();
            UIEventData evtData = GetEventData(UIEventType.Hide);
            if (evtData != null)
            {
                if (evtData.tweenGroup != null)
                    evtData.tweenGroup.Play((short)evtData.animationID, OnHiddenTweenOver);
                else if (evtData.animationID > 0)
                {
                    m_pUIAnimatoring = UIAnimatorFactory.getInstance().CreatePlayAble(evtData.animationID, evtData.ApplyRoot ? evtData.ApplyRoot : transform);
                    if (m_pUIAnimatoring != null)
                    {
                        m_pUIAnimatoring.SetUserData(new Variable1() { intVal = -1 });
                        m_pUIAnimatoring.AddCallback(this);
                        m_pUIAnimatoring.SetSpeedScale(evtData.speedScale);
                        m_pUIAnimatoring.SetReverse(evtData.bReverse);
                        m_pUIAnimatoring.Start();
                    }
                }
                else
                    gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        //------------------------------------------------------
        void OnHiddenTweenOver(RtgTween.RtgTweenerParam param, RtgTween.ETweenStatus status)
        {
            if(status == RtgTween.ETweenStatus.Complete)
                gameObject.SetActive(false);
        }
        //------------------------------------------------------
        protected virtual bool DoUIEvent(UIEventType type) { return false; }
        public virtual bool OnShow() { return false; }
        public virtual bool OnHide() { return false; }
        public virtual bool OnMoveOut() { return false; }
        public virtual bool OnMoveIn() { return false; }
        //------------------------------------------------------
        public UIEventData GetEventData(UIEventType type)
        {
            if (UIEventDatas == null) return null;
            if ((int)type >= UIEventDatas.Length) return null;
            return UIEventDatas[(int)type];
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取引用对象")]
        public GameObject Find(string strName)
        {
            Serialized();
            if (m_vFinder == null || m_vFinder.Count <= 0) return null;
            GameObject pOut = null;
            if (m_vFinder.TryGetValue(strName, out pOut))
                return pOut;
            return null;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ClearAnimation();   
        }
        //------------------------------------------------------
        void OnEnable()
        {
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            ClearAnimation();
        }
        //------------------------------------------------------
        public void PlayTween(short group, bool bForce = true)
        {
            if (Widgets == null) return;
            RtgTween.TweenerGroup tween;
            for (int i = 0; i < Widgets.Length; ++i)
            {
                if (Widgets[i].widget == null) continue;
                tween = Widgets[i].widget as RtgTween.TweenerGroup;
                if (tween != null) tween.Play(group, bForce);
            }
        }
        //------------------------------------------------------
        public void OnUIAnimatorPlayableBegin(VariablePoolAble userData)
        {

        }
        //------------------------------------------------------
        public void OnUIAnimatorPlayableEnd(VariablePoolAble userData)
        {
            if(userData!=null && userData is Variable1)
            {
                Variable1 var = (Variable1)userData;
                if(gameObject) gameObject.SetActive(var.intVal > 0);
            }
            m_pUIAnimatoring = null;
        }
    }
}
