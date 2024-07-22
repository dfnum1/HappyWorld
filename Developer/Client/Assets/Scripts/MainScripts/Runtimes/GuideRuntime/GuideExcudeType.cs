/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GuideTriggerType
作    者:	HappLI
描    述:	
*********************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Plugin.Guide
{
    [GuideExport("执行器")]
    public enum GuideExcudeType
    {
        [GuideExcude("启用Mask",true)]
        [GuideArgv("蒙版", "mask", "", typeof(bool), EArgvFalg.All)]
        [GuideArgv("蒙版颜色", "color", "", typeof(Color), EArgvFalg.All)]
        [GuideArgv("启动拖拽穿透", "penetrateEnable", "拖拽穿透,默认关闭", typeof(bool), EArgvFalg.All)]
        [GuideArgv("型状", "maskType", "", typeof(EMaskType), EArgvFalg.All)]
        [GuideArgv("穿透的控件ID", "widgetID", "控件ID,需要在控件上绑定GuideGuid组件", typeof(GuideGuid), EArgvFalg.All)]
        [GuideArgv("过渡", "tweenSpeed", "从无到有的过渡速度,值越大过渡的越快", typeof(float), EArgvFalg.All)]
        MaskAble = 1,

        [GuideExcude("提示框")]
        [GuideArgv("底框类型", "BgType", "", typeof(EDescBGType), EArgvFalg.All)]
        [GuideArgv("文本内容", "contextID", "", null, EArgvFalg.All)]
        [GuideArgv("文本颜色", "color", "", typeof(Color), EArgvFalg.All)]
        [GuideArgv("是否3D", "Is3DPos", "是否为3D坐标,如果没勾选，则为屏幕比率", typeof(bool), EArgvFalg.All)]
        [GuideArgv("PositionX", "posx", "百分位", null, EArgvFalg.All)]
        [GuideArgv("PositionY", "posy", "百分位", null, EArgvFalg.All)]
        [GuideArgv("PositionZ", "posz", "百分位", null, EArgvFalg.All)]
        [GuideArgv("逐字播放", "isTransition", "是否逐字播放", typeof(bool), EArgvFalg.All)]
        [GuideArgv("播放速度", "speed", "", null, EArgvFalg.All)]
        [GuideArgv("显示对话箭头", "enableArrow", "", typeof(bool), EArgvFalg.All)]
        [GuideArgv("人物背景框", "avatarEnable", "", typeof(bool), EArgvFalg.All)]
        Tips = 2,

        [GuideExcude("提示框-跟随控件")]
        [GuideArgv("底框类型", "BgType", "", typeof(EDescBGType), EArgvFalg.All)]
        [GuideArgv("文本内容", "contextID", "", null, EArgvFalg.All)]
        [GuideArgv("文本颜色", "color", "", typeof(Color), EArgvFalg.All)]
        [GuideArgv("跟随控件", "widgetID", "控件ID,需要在控件上绑定GuideGuid组件", typeof(GuideGuid), EArgvFalg.All)]
        [GuideArgv("OffsetX", "offsetX", "百分位", null, EArgvFalg.All)]
        [GuideArgv("OffsetY", "offsetY", "百分位", null, EArgvFalg.All)]
        [GuideArgv("逐字播放", "isTransition", "是否逐字播放", typeof(bool), EArgvFalg.All)]
        [GuideArgv("播放速度", "speed", "", null, EArgvFalg.All)]
        [GuideArgv("控件索引", "widgetIndex", "控件索引", null, EArgvFalg.All)]
        [GuideArgv("自动隐藏时间(毫秒)", "autoHideTime", "控件索引", null, EArgvFalg.All)]
        [GuideStrArgv("控件名称", "searchName", "", null, EArgvFalg.All)]
        [GuideArgv("显示对话箭头", "enableArrow", "", typeof(bool), EArgvFalg.All)]
        [GuideArgv("人物背景框", "avatarEnable", "", typeof(bool), EArgvFalg.All)]
        TipsByGUI = 3,

        [GuideExcude("模式控件事件")]
        [GuideArgv("控件ID", "widgetID", "", typeof(GuideGuid), EArgvFalg.Get)]
        [GuideArgv("事件类型", "evtType", "", typeof(EUIWidgetTriggerType), EArgvFalg.Get)]
        TestWidget = 4,

        [GuideExcude("事件触发器")]
        [GuideArgv("事件ID", "eventID", "", null, EArgvFalg.All)]
        EventTriggers = 5,

        [GuideExcude("当前金币")]
        [GuideArgv("金币", "money", "", null, EArgvFalg.GetAndPort)]
        MoneyInfo = 100,

        [GuideExcude("相机路径动画")]
        [GuideArgv("相机动画ID", "cameraPathID", "参数提示", null, EArgvFalg.All)]
        [GuideArgv("挂载物体GUID", "guid", "参数提示", typeof(GameObject), EArgvFalg.All)]
        PlayCameraAni = 102,

        [GuideExcude("设置对话框箭头位置")]
        [GuideArgv("跟随控件", "widgetID", "控件ID,需要在控件上绑定GuideGuid组件", typeof(GuideGuid), EArgvFalg.All)]
        [GuideArgv("OffsetX", "offsetX", "", null, EArgvFalg.All)]
        [GuideArgv("OffsetY", "offsetY", "", null, EArgvFalg.All)]
        [GuideArgv("角度", "rotate", "")]
        [GuideArgv("是否图片反转", "isReverse", "", typeof(bool), EArgvFalg.All)]
        SetDialogArrow = 105,

        [GuideExcude("引导图片节点")]
        [GuideStrArgv("图片路径", "strPath", "", null, EArgvFalg.All)]
        [GuideArgv("是否显示", "isActive", "", typeof(bool), EArgvFalg.All)]
        [GuideArgv("是否图片反转", "isReverse", "", typeof(bool), EArgvFalg.All)]
        [GuideArgv("是否使用原图片大小", "isSetNativeSize", "", typeof(bool), EArgvFalg.All)]
        [GuideArgv("位置跟随控件ID", "widgetID", "如果有跟随控件ID,就以控件ID坐标为准", typeof(GuideGuid), EArgvFalg.All)]
        [GuideArgv("位置X", "posX", "如果有跟随控件ID,那么就是相对位置X", null, EArgvFalg.All)]
        [GuideArgv("位置Y", "posY", "如果有跟随控件ID,那么就是相对位置Y", null, EArgvFalg.All)]
        [GuideArgv("图片大小X", "sizeX", "如果使用原图片大小,这边就不进行设置,", null, EArgvFalg.All)]
        [GuideArgv("图片大小Y", "sizeY", "如果使用原图片大小,这边就不进行设置,", null, EArgvFalg.All)]
        [GuideArgv("旋转角度", "rotate", "")]
        [GuideArgv("颜色", "color", "", typeof(Color), EArgvFalg.All)]
        SetGuideImage = 106,

        [GuideExcude("设置物体高亮状态")]
        [GuideArgv("高亮guid", "guid", "高亮的guid", null, EArgvFalg.All)]
        [GuideArgv("是否高亮", "isHighLight", "", typeof(bool), EArgvFalg.All)]
        SetHighlightUI = 108,

        [GuideExcude("进入战斗")]
        EnterBattle = 110,

        [GuideExcude("文字节点")]
        [GuideArgv("是否显示", "isEnable", "", typeof(bool), EArgvFalg.All)]
        [GuideArgv("文本id", "id", "", null, EArgvFalg.All)]
        [GuideArgv("位置X", "posX", "", null, EArgvFalg.All)]
        [GuideArgv("位置Y", "posY", "", null, EArgvFalg.All)]
        [GuideArgv("大小X", "sizeX", "", null, EArgvFalg.All)]
        [GuideArgv("大小Y", "sizeY", "", null, EArgvFalg.All)]
        [GuideArgv("颜色", "color", "", typeof(Color), EArgvFalg.All)]
        [GuideArgv("字体大小", "fontSize", "默认23", null, EArgvFalg.All)]
        [GuideArgv("逐字播放", "isTransition", "是否逐字播放", typeof(bool), EArgvFalg.All)]
        [GuideArgv("播放速度", "speed", "", null, EArgvFalg.All)]
        SetGuideText = 111,

        [GuideExcude("点击继续图片节点")]
        [GuideArgv("是否显示", "isEnable", "", typeof(bool), EArgvFalg.All)]
        [GuideArgv("位置X", "posX", "", null, EArgvFalg.All)]
        [GuideArgv("位置Y", "posY", "", null, EArgvFalg.All)]
        [GuideArgv("大小X", "sizeX", "", null, EArgvFalg.All)]
        [GuideArgv("大小Y", "sizeY", "", null, EArgvFalg.All)]
        SetContinueImage = 112,

        [GuideExcude("输入操作过滤开关")]
        [GuideArgv("是否过滤", "isEnable", "", typeof(bool), EArgvFalg.All)]
        TouchInputEnable = 113,

        [GuideExcude("设置AnchorPosition")]
        [GuideArgv("设置的控件guid", "widgetID", "控件ID,需要在控件上绑定GuideGuid组件", typeof(GuideGuid), EArgvFalg.All)]
        [GuideArgv("位置X", "posX", "", null, EArgvFalg.All)]
        [GuideArgv("位置Y", "posY", "", null, EArgvFalg.All)]
        SetAnchorPosition = 115,


        [GuideExcude("获取游戏位置状态")]
        [GuideArgv("状态", "state", "", typeof(TopGame.SvrData.ELocationState), EArgvFalg.GetAndPort)]
        GetLocationState = 120,


        [GuideExcude("获取界面显示状态")]
        [GuideArgv("界面", "ui", "", typeof(TopGame.UI.EUIType), EArgvFalg.SetAndPort)]
        [GuideArgv("是否显示", "isShow", "", null, EArgvFalg.GetAndPort)]
        GetUIState = 128,

        [GuideExcude("获取Gameobject激活状态")]
        [GuideArgv("控件", "widgetID", "控件ID,需要在控件上绑定GuideGuid组件", typeof(GuideGuid), EArgvFalg.All)]
        [GuideArgv("是否显示", "isShow", "", null, EArgvFalg.GetAndPort)]
        GetGameobjectAcrive = 129,

        [GuideExcude("设置Mask高亮区域")]
        [GuideArgv("guid", "guid", "指定Rect大小", null, EArgvFalg.All)]
        [GuideArgv("控件索引", "widgetIndex", "控件索引", null, EArgvFalg.All)]
        [GuideStrArgv("控件名称", "searchName", "", null, EArgvFalg.All)]
        [GuideArgv("是否能够点击", "bClick", "", typeof(bool), EArgvFalg.All)]
        SetHighlightRect = 132,


        [GuideExcude("获取目标是否可以点击")]
        [GuideArgv("guid", "guid", "", null, EArgvFalg.SetAndPort)]
        [GuideArgv("状态", "state", "", null, EArgvFalg.GetAndPort)]
        GetTargetCanClick = 137,

        [GuideExcude("获取玩家等级")]
        [GuideArgv("玩家等级", "level", "", typeof(int), EArgvFalg.GetAndPort)]
        GetPlayerLevel = 138,

        [GuideExcude("记录引导标志")]
        UpdateGuideFlag = 200,

        [GuideExcude("引导打点")]
        [GuideStrArgv("打点描述", "desc", "", null, EArgvFalg.All)]
        GuideLog = 201,

        [GuideExcude("是否执行过引导组")]
        [GuideArgv("tagID", "tagID", "引导组id", null, EArgvFalg.All)]
        [GuideArgv("判断结果", "result", "如果为1,就说明执行过,0未执行", null, EArgvFalg.GetAndPort)]
        IsTriggerGuide = 202,
    }
}