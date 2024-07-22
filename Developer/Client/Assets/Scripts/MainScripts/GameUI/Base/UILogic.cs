/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UILogic
作    者:	HappLI
描    述:	
*********************************************************************/

using Framework.Core;
using Framework.Plugin.AT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    [Framework.Plugin.AT.ATExportMono("UI系统/逻辑脚本")]
    public abstract class UILogic : Framework.Plugin.AT.IUserData, Framework.Core.ITimerTicker
    {
        private UIView m_pView = null;
        private bool m_bActive = true;
        public virtual void OnInit(UIBase pBase) { }
        public virtual void Awake(UIBase pBase) { m_pView = pBase.view; }
        public virtual void OnShowEnd() { }
        public virtual void OnShow() { }
        public virtual void OnHide() { }
        public virtual void OnClear() { }
        public virtual void OnClose() { }
        public virtual void OnDestroy() { m_pView = null; }
        protected virtual void OnUpdate(float fFrame) { }
        protected virtual void OnSecondUpdate() { }

        protected virtual AssetOperiaon LoadObjectAsset(UnityEngine.Object pObj, string strPath, bool bPermanent = false, bool bAysnc = false, Sprite defaultSprite = null)
        {
            if (m_pView != null)
            {
               return m_pView.LoadObjectAsset(pObj, strPath, bPermanent, bAysnc, defaultSprite);
            }
            return null;
        }

        protected virtual AssetOperiaon LoadObjectAsset(UnityEngine.Object pObj, uint key, bool bPermanent = false, bool bAysnc = false, Sprite defaultSprite = null)
        {
            string strPath = UIUtil.GetAssetPath(key);
            
            return LoadObjectAsset(pObj,strPath,bPermanent,bAysnc,defaultSprite);
        }

        protected virtual InstanceOperiaon LoadInstance(string strPrefab, Transform pParent, bool bAsync = true, System.Action<InstanceOperiaon> OnCallback = null, Framework.Plugin.AT.IUserData userPtr = null)
        {
            if (m_pView == null || string.IsNullOrEmpty(strPrefab)) return null;
            return m_pView.LoadInstance(strPrefab, pParent, bAsync, OnCallback, userPtr);
        }
        protected virtual InstanceOperiaon LoadInstance(GameObject prefab, Transform pParent, bool bAsync = true, System.Action<InstanceOperiaon> OnCallback = null, Framework.Plugin.AT.IUserData userPtr = null)
        {
            if (m_pView == null || prefab == null) return null;
            return m_pView.LoadInstance(prefab, pParent, bAsync, OnCallback, userPtr);
        }
        protected virtual InstanceOperiaon LoadInstance(uint langueKey, Transform pParent, bool bAsync = true, System.Action<InstanceOperiaon> OnCallback = null, Framework.Plugin.AT.IUserData userPtr = null)
        {
            if (m_pView == null || langueKey == 0) return null;
            return m_pView.LoadInstance(langueKey, pParent, bAsync, OnCallback, userPtr);
        }

        public void Update(float fFrame) 
        {
            if (m_bActive) OnUpdate(fFrame);
        }

        public void SecondUpdate()
        {
            if (m_bActive) OnSecondUpdate();
        }

        public virtual void Active(bool bActive)  {  m_bActive = bActive;  }
        public void Destroy() { OnDestroy(); }
        //-------------------------------------------------
        public virtual bool OnTimerTick(IBaseTimerEvent hHandle, IUserData param)
        {
            return false;
        }
        //-------------------------------------------------
        public virtual bool IsTimerValid()
        {
            return m_bActive;
        }
        //-------------------------------------------------
        public T GetWidget<T>(string name) where T : Component
        {
            if (m_pView == null) return null;
            return m_pView.GetWidget<T>(name);
        }
    }
}
