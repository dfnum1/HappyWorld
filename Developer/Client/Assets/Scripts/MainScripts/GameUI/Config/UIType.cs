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
    [Framework.Plugin.PluginBinderType("UI.EUIType")]
    public enum EUIType
    {
        [Framework.Data.DisplayNameGUI("None/无")] None = 0,
        [Framework.Data.DisplayNameGUI("Loading/Loading界面")] Loading = 1,
        [Framework.Data.DisplayNameGUI("Loading/全屏填充界面")] FullScreenFillPanel = 2,
        [Framework.Data.DisplayNameGUI("Loading/过渡效果界面")] TransitionPanel = 3,
        


        [Framework.Data.DisplayNameGUI("基础/登录界面")] Login = 10,
        [Framework.Data.DisplayNameGUI("基础/游戏信息界面")]  GameInfo = 11,
        [Framework.Data.DisplayNameGUI("通用/对话界面")] DialogPanel = 12,
        [Framework.Data.DisplayNameGUI("基础/控制台指令界面")] GMPanel = 13,

        [Framework.Data.DisplayNameGUI("战斗/战斗界面")] BattlePanel = 20,

        [Framework.Data.DisplayNameGUI("基础/等待网络界面")] WaitPanel = 200,
        [Framework.Data.DisplayNameGUI("基础/公告界面")] NoticeTip = 201,
        [Framework.Data.DisplayNameGUI("通用/通用等待界面")] CommonWaitPanel = 202,


        [Framework.Data.DisplayNameGUI("通用/引导界面")] GuidePanel = 254,
        [Framework.Data.DisplayNameGUI("通用/通用提示界面")] CommonTip = 255,
        [Framework.Data.DisplayNameGUI("通用/视频界面"),Framework.Plugin.DisableGUI] VideoPanel = 256,

        [Framework.Plugin.DisableGUI]
        Count,
    }
}
