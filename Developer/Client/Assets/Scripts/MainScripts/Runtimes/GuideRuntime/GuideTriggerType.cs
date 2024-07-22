/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GuideTriggerType
作    者:	HappLI
描    述:	
*********************************************************************/

using System;
using System.Collections.Generic;

namespace Framework.Plugin.Guide
{
    [GuideExport("触发器")]
    public enum GuideTriggerType
    {
        [GuideTrigger("常规类型")]
        [GuideArgv("游戏状态", "state", "", typeof(TopGame.Logic.EGameState), EArgvFalg.SetAndPort, EBitGuiType.Offset)]
        Guide = 1,

        [GuideTrigger("打开某个UI")]
        [GuideArgv("ui类型", "uiType", "", typeof(TopGame.UI.EUIType), EArgvFalg.GetAndPort)]
        [GuideArgv("游戏状态", "state", "", typeof(TopGame.Logic.EGameState), EArgvFalg.GetAndPort, EBitGuiType.Offset )]
        OpenUI = 2,

        [GuideTrigger("进入某个状态")]
        [GuideArgv("游戏状态", "state", "", typeof(TopGame.Logic.EGameState), EArgvFalg.SetAndPort)]
        [GuideArgv("游戏模式", "mode", "", typeof(TopGame.Logic.EMode), EArgvFalg.SetAndPort)]
        EnterGameState = 3,

        [GuideTrigger("开始任务(暂不可用)")]
        [GuideArgv("任务ID", "taskID", "", null, EArgvFalg.GetAndPort)]
        [GuideArgv("游戏状态", "state", "", typeof(TopGame.Logic.EGameState), EArgvFalg.GetAndPort, EBitGuiType.Offset)]
        StartTask = 4,

        [GuideTrigger("完成任务")]
        [GuideArgv("任务ID", "taskID", "", null, EArgvFalg.GetAndPort)]
        [GuideArgv("游戏状态", "state", "", typeof(TopGame.Logic.EGameState), EArgvFalg.GetAndPort, EBitGuiType.Offset)]
        CompleteTask = 5,

        [GuideTrigger("关闭某个UI")]
        [GuideArgv("ui类型", "uiType", "", typeof(TopGame.UI.EUIType), EArgvFalg.GetAndPort)]
        [GuideArgv("游戏状态", "state", "", typeof(TopGame.Logic.EGameState), EArgvFalg.GetAndPort, EBitGuiType.Offset)]
        CloseUI = 6,

        [GuideTrigger("引导结束时")]
        [GuideArgv("结束的引导组id", "guideID", "", null, EArgvFalg.GetAndPort)]
        [GuideArgv("游戏状态", "state", "", typeof(TopGame.Logic.EGameState), EArgvFalg.GetAndPort, EBitGuiType.Offset)]
        CompleteGudie = 7,
    }
}