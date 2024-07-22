/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GuideTriggerType
作    者:	HappLI
描    述:	
*********************************************************************/

using System;
using System.Collections.Generic;
using TopGame.UI;
using UnityEngine;

namespace Framework.Plugin.Guide
{
    [GuideExport("步骤")]
    public enum GuideStepType
    {
        [GuideStep("控件点击",true)]
        [GuideArgv("控件", "widgetGUID", "", typeof(GuideGuid), EArgvFalg.PortAll)]
        [GuideArgv("控件索引", "index", "", null, EArgvFalg.PortAll)]
        [GuideArgv("手势类型", "fingerType", "", typeof(EFingerType))]
        [GuideArgv("角度", "rotate", "")]
        [GuideArgv("偏移X", "offsetX", "")]
        [GuideArgv("偏移Y", "offsetY", "")]
        [GuideArgv("克隆到顶层", "MostTop", "", typeof(bool))]
        [GuideStrArgv("控件名称","listenerName","动态加载查找的控件名称")]
        [GuideArgv("按任意键完成", "bPressAnyKey", "", typeof(bool))]
        [GuideArgv("是否自动执行", "bAutoCallNext", "", typeof(bool), EArgvFalg.GetAndPort)]
        ClickUI = 100,

        [GuideStep("滑动", true)]
        [GuideArgv("手势类型", "fingerType", "", typeof(EFingerType))]
        [GuideArgv("角度", "rotate", "")]
        [GuideArgv("起点3D", "IsStart3DPos", "是否为3D坐标,如果没勾选，则为屏幕比率", typeof(bool))]
        [GuideArgv("参数X", "startPosX", "")]
        [GuideArgv("参数Y", "startPosY", "")]
        [GuideArgv("起点大小", "startSize", "")]
        [GuideArgv("终点3D", "IsEnd3DPos", "是否为3D坐标,如果没勾选，则为屏幕比率", typeof(bool))]
        [GuideArgv("参数X", "endPosX", "")]
        [GuideArgv("参数Y", "endPosY", "")]
        [GuideArgv("终点大小", "endSize", "")]
        [GuideArgv("滑动速度(默认100)", "speed", "")]
        Slide = 101,

        [GuideStep("点击区域", true)]
        [GuideArgv("手势类型", "fingerType", "", typeof(EFingerType))]
        [GuideArgv("角度", "rotate", "")]
        [GuideArgv("是否3D", "IsStart3DPos", "是否为3D坐标,如果没勾选，则为屏幕比率", typeof(bool))]
        [GuideArgv("参数X", "startPosX", "")]
        [GuideArgv("参数Y", "startPosY", "")]
        [GuideArgv("区域大小", "Size", "")]
        ClickZoom = 102,

        [GuideStep("任意点击")]
        ClickAnywhere = 103,

        [GuideStep("等待路径动画播放结束")]
        [GuideArgv("路径动画ID", "animPathID", "", null, EArgvFalg.GetAndPort)]
        WaitAnimPathEnd = 104,

        [GuideStep("等待网络回包")]
        [GuideArgv("网络消息类型", "netCode", "", typeof(Proto3.MID), EArgvFalg.GetAndPort)]
        WaitNetEnd = 105,

        [GuideStep("等待界面打开")]
        [GuideArgv("界面ID", "panelID", "", typeof(EUIType), EArgvFalg.GetAndPort)]
        WaitPanelOpen = 106,

        [GuideStep("滑动(只判断方向)", true)]
        [GuideArgv("起点X", "startPosX", "")]
        [GuideArgv("起点Y", "startPosY", "")]
        [GuideArgv("终点X", "endPosX", "")]
        [GuideArgv("终点Y", "endPosY", "")]
        [GuideArgv("速度(默认100)", "speed", "")]
        [GuideArgv("判断滑动成功角度临界值", "checkAngle", "")]
        SlideCheckDirection = 107,

        [GuideStep("滑动(立刻判断滑动方向不用松开滑动手指)", true)]
        [GuideArgv("起点X", "startPosX", "")]
        [GuideArgv("起点Y", "startPosY", "")]
        [GuideArgv("终点X", "endPosX", "")]
        [GuideArgv("终点Y", "endPosY", "")]
        [GuideArgv("速度(默认100)", "speed", "")]
        [GuideArgv("判断滑动成功角度临界值", "checkAngle", "")]
        SlideCheckDirectionImmediately = 108,

        [GuideStep("等待Tween播放结束")]
        [GuideArgv("Tween动画ID", "animPathID", "", null, EArgvFalg.GetAndPort)]
        WaitTweenEnd = 109,

        [GuideStep("等待界面关闭")]
        [GuideArgv("界面ID", "panelID", "", typeof(EUIType), EArgvFalg.GetAndPort)]
        WaitUIClose = 110,

        [GuideStep("等待控件激活")]
        [GuideArgv("控件", "widgetGUID", "", typeof(GuideGuid), EArgvFalg.All)]
        [GuideArgv("控件索引", "index", "", null, EArgvFalg.PortAll)]
        [GuideArgv("是否检测激活", "isCheckEnable", "", typeof(bool))]
        [GuideArgv("是否检测缩放", "isCheckScale", "", typeof(bool))]
        [GuideArgv("缩放X", "scaleX", "")]
        [GuideArgv("缩放Y", "scaleY", "")]
        [GuideArgv("缩放Z", "scaleZ", "")]
        [GuideArgv("是否检测屏幕外", "isCheckPos", "", typeof(bool))]
        [GuideArgv("是否设置检测等待间隔(未实现)", "isCheckInterval", "", typeof(bool))]
        [GuideArgv("检测等待间隔时间", "intervalTime", "")]
        [GuideArgv("检测等待间隔次数", "intervalCount", "")]
        WaitUIComponentActive = 111,

        [GuideStep("等待路径动画播放")]
        [GuideArgv("路径动画ID", "animPathID", "", null, EArgvFalg.GetAndPort)]
        WaitAnimPathPlay = 112,

        [GuideStep("等待TimeLine播放完成")]
        [GuideArgv("guid", "guid", "", null, EArgvFalg.GetAndPort)]
        WaitTimeLineEnd = 113,

        [GuideStep("等待解锁动画播放完成")]
        WaitUnlockAnimPlayEnd = 114,

        [GuideStep("等待动态组件加载完成")]
        [GuideArgv("guid", "guid", "", null, EArgvFalg.GetAndPort)]
        WaitComponentLoadCompleted = 115,

        [GuideStep("等待对话完成")]
        WaitDialogEnd = 116,

        [GuideStep("等待相机过渡完成")]
        [GuideArgv("误差(百分位)", "factor", "", null, EArgvFalg.GetAndPort)]
        WaitCameraTweenEnd = 117,

        [GuideStep("等待敌方出现视野中")]
        WaitEnemyInVisible = 118,

        [GuideStep("等待战斗激活(开始)")]
        WaitBattleBegin = 119,

        [GuideStep("等待波怪战斗结束")]
        [GuideArgv("第几波", "region", "", null, EArgvFalg.GetAndPort)]
        WaitBattleEnd = 120,

        [GuideStep("等待战斗结束")]
        [GuideArgv("战斗结果", "result", "", typeof(TopGame.Logic.EBttleExitType), EArgvFalg.GetAndPort)]
        WaitBattleResult = 121,

        [GuideStep("等待英雄技能可释放")]
        [GuideArgv("configID", "configID", "", null, EArgvFalg.All)]
        [GuideArgv("skillTag", "skillTag", "", null, EArgvFalg.All)]
        WaitHeroSkillCanUse = 122,

        [GuideStep("检测只有限定界面显示")]
        [GuideArgv("UI1", "ui1", "", typeof(TopGame.UI.EUIType), EArgvFalg.Get)]
        [GuideArgv("UI2", "ui2", "", typeof(TopGame.UI.EUIType), EArgvFalg.Get)]
        [GuideArgv("UI3", "ui3", "", typeof(TopGame.UI.EUIType), EArgvFalg.Get)]
        [GuideArgv("UI4", "ui4", "", typeof(TopGame.UI.EUIType), EArgvFalg.Get)]
        [GuideArgv("UI5", "ui5", "", typeof(TopGame.UI.EUIType), EArgvFalg.Get)]
        [GuideArgv("UI6", "ui6", "", typeof(TopGame.UI.EUIType), EArgvFalg.Get)]
        CheckOnlyShowUI = 124,

        [GuideStep("等待Gameobject激活状态")]
        [GuideArgv("控件", "widgetID", "控件ID,需要在控件上绑定GuideGuid组件", typeof(GuideGuid), EArgvFalg.GetAndPort)]
        [GuideArgv("是否显示", "isShow", "", null, EArgvFalg.GetAndPort)]
        WaitGameobjectActive = 127,

        [GuideStep("等待目标可以点击")]
        [GuideArgv("guid", "widgetID", "控件ID,需要在控件上绑定GuideGuid组件", typeof(GuideGuid), EArgvFalg.GetAndPort)]
        WaitGameobjectCanClick = 128,
    }
}