/********************************************************************
生成日期:	3:23:2024  20:39
类    名: 	PVEPlayer
作    者:	HappLI
描    述:	PVE 玩家操控对象
*********************************************************************/
using ExternEngine;
using Framework.Core;
using TopGame.Core;
using TopGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.Logic
{
    [ModeLogic(EMode.PVE)]
    public class PVEUILogic : AModeLogic
    {
        protected override void OnStart()
        {
            base.OnStart();

            BattlePanel panel = UIKits.CastGetUI<BattlePanel>();
            if(panel!=null)
            {
                BubbleJoystick joystick = panel.GetWidget<BubbleJoystick>("joystick");
                if(joystick!=null)
                {
                    var player = GetModeLogic<PVEPlayer>();
                    if (player != null) joystick.AddCallback(player.OnJoystick);
                }
                var attackBtn = panel.GetWidget<Image>("Attack");
                if (attackBtn != null)
                {
                    var player = GetModeLogic<PVEPlayer>();
                    if(player!=null)
                        EventTriggerListener.Get(attackBtn).onClick += player.DoAttack;

                }
                panel.Show();
            }
        }
        //------------------------------------------------------
        protected override void OnClear()
        {
            base.OnClear();
            BattlePanel panel = UIKits.CastGetUI<BattlePanel>();
            if (panel != null)
            {
                BubbleJoystick joystick = panel.GetWidget<BubbleJoystick>("joystick");
                if (joystick != null)
                {
                    joystick.ClearCallbacks();
                }
            }
        }
    }
}