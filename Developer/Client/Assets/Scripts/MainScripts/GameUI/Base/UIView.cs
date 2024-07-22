/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIBase
作    者:	HappLI
描    述:	UI管理器
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    [Framework.Plugin.AT.ATExportMono("UI系统/界面视图")]
    public abstract class UIView : Core.ADyanmicLoader, Framework.Plugin.AT.IUserData
    {
        protected UIBase m_pBaseUI = null;
        public virtual void Init(UIBase pBase) { m_pBaseUI = pBase; }
        public virtual void Awake() { }
        public virtual void Show() { }
        public virtual void Hide() { }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("清理")]
        public virtual void Clear(bool bUnloadDynamic = true)
        {
            if(bUnloadDynamic)
                ClearLoaded();
            Core.RenderHudManager.getInstance().OnClearCallback(this);
        }
        //------------------------------------------------------
        public virtual void Destroy()
        {
            ClearLoaded();
            Core.RenderHudManager.getInstance().OnClearCallback(this);
        }
        //------------------------------------------------------
        protected override bool LoadSignCheck()
        {
            return m_pBaseUI != null && m_pBaseUI.IsInstanced();// && m_pBaseUI.IsVisible();
        }
        //------------------------------------------------------
        protected override bool LoadInstanceSignCheck()
        {
            return m_pBaseUI != null && m_pBaseUI.IsInstanced() && m_pBaseUI.IsVisible();
        }
        //------------------------------------------------------
        protected override void OnSpawInstanced(Framework.Core.AInstanceAble pAble)
        {
            var controller = pAble as Core.ParticleController;
            if(controller)
            {
                controller.bIsDisableCheck = true;
                controller.SetRenderOrder(m_pBaseUI.GetOrder(), true);
            }
        }
        //------------------------------------------------------
        public T GetWidget<T>(string name) where T : Component
        {
            if (m_pBaseUI == null) return null;
            return m_pBaseUI.GetWidget<T>(name);
        }
        //------------------------------------------------------
        public virtual void Update(float fFrame) { }

    }
}
