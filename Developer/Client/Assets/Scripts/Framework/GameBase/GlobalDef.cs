/********************************************************************
生成日期:	17:9:2019   16:19
类    名: 	GlobalDef
作    者:	HappLI
描    述:	全局定义
*********************************************************************/
using System;
using UnityEngine;

namespace TopGame.Base
{
    public enum EResetType
    {
        Local = 0,
        World,
        All,
    }
    public enum AnchorPresets
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottonCenter,
        BottomRight,
        BottomStretch,

        VertStretchLeft,
        VertStretchRight,
        VertStretchCenter,

        HorStretchTop,
        HorStretchMiddle,
        HorStretchBottom,

        StretchAll
    }
    public enum PivotPresets
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,
    }
    [Framework.Plugin.AT.ATEvent("AT事件")]
    public enum EUIEventType
    {
        [Framework.Plugin.AT.ATEvent("ui-点击")]                                onClick = 1,
        [Framework.Plugin.AT.ATEvent("ui-按下")]                                onDown = 2,
        [Framework.Plugin.AT.ATEvent("ui-鼠标进入")]                            onEnter = 3,
        [Framework.Plugin.AT.ATEvent("ui-鼠标退出")]                            onExit = 4,
        [Framework.Plugin.AT.ATEvent("ui-鼠标弹起")]                            onUp =5,
        [Framework.Plugin.AT.ATEvent("ui-选中")]                                onSelect = 6,
        [Framework.Plugin.AT.ATEvent("ui-选中更新")]                            onUpdateSelect = 7,
        [Framework.Plugin.AT.ATEvent("ui-拖拽")]                                onDrag = 8,
        [Framework.Plugin.AT.ATEvent("ui-拖拽释放")]                            onDrop = 9,
        [Framework.Plugin.AT.ATEvent("ui-取消选中")]                            onDeselect = 10,
        [Framework.Plugin.AT.ATEvent("ui-列表滚动")]                            onScroll = 11,
        [Framework.Plugin.AT.ATEvent("ui-开始拖拽")]                            onBeginDrag = 12,
        [Framework.Plugin.AT.ATEvent("ui-结束拖拽")]                            onEndDrag = 13,
        [Framework.Plugin.AT.ATEvent("ui-提交")]                                onSubmit = 14,
        [Framework.Plugin.AT.ATEvent("ui-取消")]                                onCancel = 15,
        [Framework.Plugin.AT.ATEvent("ui-打开界面", "TopGame.UI.EUIType")]      OnShowUI = 16,
        [Framework.Plugin.AT.ATEvent("ui-关闭界面", "TopGame.UI.EUIType")]      OnHideUI = 17,
        [Framework.Plugin.AT.ATEvent("ui-列表项更新")]                          OnListItem = 18,
        [Framework.Plugin.AT.ATEvent("ui-打开界面结束", "TopGame.UI.EUIType")]  OnShowUIEnd = 19,
        [Framework.Plugin.AT.ATEvent("ui-移动")]                                onMove = 20,
    }
    public enum EMaskLayer
    {
        [Framework.Plugin.PluginDisplay("背景层")] BackLayer = 8,
        [Framework.Plugin.PluginDisplay("前置层")] ForeLayer = 10,
        [Framework.Plugin.PluginDisplay("特效层")] EffectLayer = 11,
        [Framework.Plugin.PluginDisplay("UI3D层")] UI_3D = 12,
        [Framework.Plugin.PluginDisplay("UI3D背景层")] UIBG_3D = 13,
        [Framework.Plugin.PluginDisplay("UI3D前置层")] UIBGAfter_3D = 14,
        [Framework.Plugin.PluginDisplay("物理碰撞层")] PhysicLayer = 15,
    };
}