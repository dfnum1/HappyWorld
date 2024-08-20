/********************************************************************
生成日期:	20:8:2024   11:37
类    名: 	BattleView
作    者:	lqg
描    述:	战斗界面
*********************************************************************/

using TopGame.Base;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    [UI((ushort)EUIType.BattlePanel, UI.EUIAttr.View)]
    public class BattleView : UIView
    {
		//TopGame.UI.BubbleJoystick	m_joystick;

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
				//m_joystick = m_pBaseUI.GetWidget<TopGame.UI.BubbleJoystick>("joystick");

            }
        }
    }
}
