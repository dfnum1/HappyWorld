/********************************************************************
生成日期:	2:6:2023   13:46
类    名: 	WaitView
作    者:	zdq
描    述:	等待网络界面
*********************************************************************/

using TopGame.Base;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    [UI((ushort)EUIType.WaitPanel, UI.EUIAttr.View)]
    public class WaitView : UIView
    {
		//TopGame.UI.EventTriggerListener	m_Mask;

        //------------------------------------------------------
        public override void Init(UIBase pBase)
        {
            base.Init(pBase);
        }
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            if (m_pBaseUI == null) return;
            if (m_pBaseUI.ui)
            {
				//m_Mask = m_pBaseUI.GetWidget<TopGame.UI.EventTriggerListener>("Mask");

            }
        }
    }
}
