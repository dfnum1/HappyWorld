/********************************************************************
生成日期:	1:11:2020 10:08
类    名: 	BaseDef
作    者:	HappLI
描    述:	
*********************************************************************/

using UnityEngine;

namespace TopGame.Base
{
    enum EControllerDirFlag
    {
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        Jump = 1<<4,
    };
    public enum ELoadingType
    {
        [Framework.Plugin.PluginDisplay("无")]
        Nonthing = 0,
        [Framework.Plugin.PluginDisplay("进度条加载")]
        Loading,
        [Framework.Plugin.PluginDisplay("黑屏加载")]
        BlackFull,
        [Framework.Plugin.PluginDisplay("模式过渡加载")]
        ModeTransition,
    }

//     public enum EClearStatType : byte
//     {
//         [Framework.Plugin.PluginDisplay("所有")]
//         All = 0,
//         [Framework.Plugin.PluginDisplay("加")]
//         Add = 1,
//         [Framework.Plugin.PluginDisplay("减")]
//         Sub = 2,
//    

    [Framework.Plugin.AT.ATEvent("AT事件")]
    public enum EATEventType
    {
        None = 0,

        [Framework.Plugin.AT.ATEvent("逻辑坐落位置")]
        LocationState = 50,

        [Framework.Plugin.AT.ATEvent("HTTP回包")]
        HTTPCallback = 100,
        [Framework.Plugin.AT.ATEvent("TCP回包", typeof(Proto3.MID))]
        TCPCallback = 101,

        [Framework.Plugin.AT.ATEvent("障碍碰撞")]
        OnElementCollision = 201,
        [Framework.Plugin.AT.ATEvent("酷跑速度变化")]
        OnRunSpeedChange = 202,

        [Framework.Plugin.AT.ATEvent("路径动画结束")]
        OnAnimPathEnd = Core.CameraAnimPath.EATEventType.AnimPathEnd,
        [Framework.Plugin.AT.ATEvent("路径动画帧回调")]
        OnAnimPathUpdate = Core.CameraAnimPath.EATEventType.AnimPathUpdate,

        [Framework.Plugin.AT.ATEvent("自定义类型")]
        UserDef = Framework.Base.EBaseATEventType.UserDef,
    }
    public enum EDropItemType
    {
        Rate = 1,// 比率
    }

    //------------------------------------------------------
    public enum EAssetKey
    {
        None = 0,
        EmptyBox = 50011,
    }
    //------------------------------------------------------
    public enum EItemType
    {
        Money = 0,
        Diamond = 1,
        Equip = 2,
        Chest = 3,
        Hero = 4,
        Common = 5,
        Exp = 6,
    }
    //------------------------------------------------------
    public enum EShopType
    {
        Item = 1,
        Glod = 2,
    }
    //------------------------------------------------------
    public enum EItemID
    {
        Gold = 1,
        Diamond=2,
        Exp=3,
        ActionPoint = 4,
        PetFood = 5,
        Egg1 = 1001,
        Egg2 = 1002,
        Egg3 = 1003,
        Egg4 = 1004,
        Ticket = 2005,
    }
    //------------------------------------------------------
    /// <summary>
    /// 关卡类型
    /// </summary>
    public enum EChapterBattleType
    {
        Run,
        Reward,
        Battle,
        Boss,
        /// <summary>
        /// 消耗类型
        /// </summary>
        Consume
    }
    //------------------------------------------------------
    public enum ECareerType
    {
        Tank = 1,
        Warrior = 2,
        ADC = 3,
        Auxiliary = 4,
        Cure = 5,
        Count = 6,
    }
    //------------------------------------------------------
    public enum EEquipType
    {
        /// <summary>
        /// 武器
        /// </summary>
        Weapon=1,
        /// <summary>
        /// 帽子
        /// </summary>
        Hat,
        /// <summary>
        /// 衣服
        /// </summary>
        Clothing,
        /// <summary>
        /// 鞋子
        /// </summary>
        Shoe,
        /// <summary>
        /// 滑板
        /// </summary>
        Skate,
        /// <summary>
        /// 项链
        /// </summary>
        Necklace,
        /// <summary>
        /// 戒指
        /// </summary>
        Ring,
        Count,
    }
    //------------------------------------------------------
    public enum EDutyType
    {
        /// <summary>
        /// 群攻
        /// </summary>
        AOE=1,
        /// <summary>
        /// 刺客
        /// </summary>
        Assassin,
        /// <summary>
        /// 爆发输出
        /// </summary>
        BurstOutput,
        /// <summary>
        /// 持续输出
        /// </summary>
        ContinuousOutput,
        /// <summary>
        /// 肉盾
        /// </summary>
        Tank,
        /// <summary>
        /// 增益
        /// </summary>
        Buff,
        /// <summary>
        /// 减益
        /// </summary>
        Debuff,
        /// <summary>
        /// 控制
        /// </summary>
        Control,
        /// <summary>
        /// 治疗
        /// </summary>
        Cure,
    }
    //------------------------------------------------------
    public enum EQuality
    {
        None,
        R = 2,
        SR = 3,
        SSR = 4,
        UR = 5,
        Count,
    }
    //------------------------------------------------------
    /// <summary>
    /// 英雄解锁类型
    /// </summary>
    public enum EHeroUnlockType
    {
        /// <summary>
        /// 道具解锁
        /// </summary>
        Item=0,
    }
}
