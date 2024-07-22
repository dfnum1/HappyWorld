/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	PermanentAssets
作    者:	HappLI
描    述:	常驻资源
*********************************************************************/
using UnityEngine;
using Framework.Core;
namespace TopGame.Data
{
    public enum EPermanentAssetType : byte
    {
        [Framework.Plugin.AT.ATField("怪伤害跳字")]
        Blood = 0,
        [Framework.Plugin.AT.ATField("血条")]
        HudHp = 1,
        [Framework.Plugin.AT.ATField("玩家伤害跳字")]
        PlayerBlood = 2,
        [Framework.Plugin.AT.ATField("怪暴击伤害跳字")]
        Crit = 3,
        [Framework.Plugin.AT.ATField("玩家暴击伤害跳字")]
        PlayerCrit = 4,
        [Framework.Plugin.AT.ATField("飘字")]
        PopText = 5,
        [Framework.Plugin.AT.ATField("UI置灰")]
        GrayMat = 6,
        [Framework.Plugin.AT.ATField("飘自愈")]
        Cure = 7,
        [Framework.Plugin.AT.ATField("玩家飘自愈")]
        PlayerCure =8,
        [Framework.Plugin.AT.ATField("通用伤害特效")]
        CommonHurtPar = 9,
        [Framework.Plugin.AT.ATField("掉血屏幕特效")]
        DropHpScreenEffect= 10,
        [Framework.Plugin.AT.ATField("加速屏幕特效")]
        AddSpeedScreenEffect = 11,
        [Framework.Plugin.AT.ATField("弱点飘血")]
        WeakBlood = 12,
        [Framework.Plugin.AT.ATField("吸铁石特效")]
        MagnetEffect = 13,
        [Framework.Plugin.AT.ATField("弱点飘血暴击")]
        WeakBloodCrit = 14,
        [Framework.Plugin.AT.ATField("元素克制伤害")]
        RestraintDamage = 15,
        [Framework.Plugin.AT.ATField("玩家血条")]
        HeroHudHp = 16,
        [Framework.Plugin.AT.ATField("架势伤害")]
        PostureDamage = 17,
        [Framework.Plugin.AT.ATField("元素克制暴击伤害")]
        RestraintCritiDamage = 18,
        [Framework.Plugin.AT.ATField("元素克制伤害水")]
        RestraintDamageWater = 19,
        [Framework.Plugin.AT.ATField("元素克制伤害火")]
        RestraintDamageFire = 20,
        [Framework.Plugin.AT.ATField("元素克制伤害木")]
        RestraintDamageWood =21,
        [Framework.Plugin.AT.ATField("元素克制伤害土")]
        RestraintDamageSoil = 22,
        [Framework.Plugin.AT.ATField("元素克制伤害电")]
        RestraintDamageElectric = 23,
        [Framework.Plugin.AT.ATField("元素克制暴击伤害水")]
        RestraintCritiDamageWater = 24,
        [Framework.Plugin.AT.ATField("元素克制暴击伤害火")]
        RestraintCritiDamageFire = 25,
        [Framework.Plugin.AT.ATField("元素克制暴击伤害木")]
        RestraintCritiDamageWood = 26,
        [Framework.Plugin.AT.ATField("元素克制暴击伤害土")]
        RestraintCritiDamageSoil = 27,
        [Framework.Plugin.AT.ATField("元素克制暴击伤害电")]
        RestraintCritiDamageElectric = 28,

        [Framework.Plugin.AT.ATField("怪技能伤害跳字")]
        SkillBlood = 29,
        [Framework.Plugin.AT.ATField("玩家技能伤害跳字")]
        PlayerSkillBlood = 30,
        [Framework.Plugin.AT.ATField("技能伤害暴击跳字")]
        SkillCritBlood = 31,
        [Framework.Plugin.AT.ATField("团魂伤害跳字")]
        TeamSoulBlood = 32,
        [Framework.Plugin.AT.ATField("团魂伤害暴击跳字")]
        TeamSoulCrilBlood = 33,
        [Framework.Plugin.AT.ATField("团魂自愈跳字")]
        TeamSoulCure = 34,
        [Framework.Plugin.AT.ATField("我方能量恢复")]
        SpRec = 35,
        [Framework.Plugin.AT.ATField("敌方能量恢复")]
        EmmySpRec = 36,
        [Framework.Plugin.AT.ATField("我方掉能量")]
        SpDrop = 37,
        [Framework.Plugin.AT.ATField("敌方掉能量")]
        EmmySpDrop = 38,

        [Framework.Plugin.AT.ATField("加速拖尾特效")]
        AddSpeedTailEffect = 40,
        [Framework.Plugin.AT.ATField("UI飞行发射器")]
        UIFlyEmitterSprite = 41,

        [Framework.Plugin.AT.ATField("水元素材质")]
        [Framework.Data.ObjectTypeGUI(typeof(Material))]
        WaterEleEffect = 50,
        [Framework.Plugin.AT.ATField("火元素材质")]
        [Framework.Data.ObjectTypeGUI(typeof(Material))]
        FireEleEffect = 51,
        [Framework.Plugin.AT.ATField("木元素材质")]
        [Framework.Data.ObjectTypeGUI(typeof(Material))]
        WoodEleEffect = 52,
        [Framework.Plugin.AT.ATField("土元素材质")]
        [Framework.Data.ObjectTypeGUI(typeof(Material))]
        SoilEleEffect = 53,
        [Framework.Plugin.AT.ATField("电元素材质")]
        [Framework.Data.ObjectTypeGUI(typeof(Material))]
        ThunderEffect = 54,

        [Framework.Plugin.AT.ATField("伤害buff飘字")]
        DamageBuffText = 60,
        [Framework.Plugin.AT.ATField("治疗buff飘字")]
        CureBuffText = 61,
        [Framework.Plugin.AT.ATField("增益buff飘字")]
        GainBuffText = 62,
        [Framework.Plugin.AT.ATField("减益buff飘字")]
        DebuffText = 63,
        [Framework.Plugin.AT.ATField("护盾buff飘字")]
        ShieldBuffText = 64,
        [Framework.Plugin.AT.ATField("词条飘字")]
        WordTip = 65,
        [Framework.Plugin.AT.ATField("技能飘字")]
        SkillText = 66,
        [Framework.Plugin.AT.ATField("UIGhost")]
        UIGhost = 67,
        [Framework.Plugin.AT.ATField("UIGhostGray")]
        UIGhostGray = 68,
        [Framework.Plugin.AT.ATField("UIMaterial")]
        UIMaterial = 69,
        [Framework.Plugin.AT.ATField("UIMaterialGray")]
        UIMaterialGray = 70,
        [Framework.Plugin.AT.ATField("UI3D角色阴影")]
        UIRecvPreviewTransparentShadow = 71,

        [Framework.Plugin.AT.ATField("精英怪血条")]
        EliteHudHp = 80,
        [Framework.Plugin.AT.ATField("敌方召唤物血条")]
        EnemySummonHudHp = 81,
        [Framework.Plugin.AT.ATField("己方召唤物血条")]
        SummonHudHp = 82,

        [Framework.Plugin.AT.ATField("闪避跳字")]
        Dodge = 90,
        [Framework.Plugin.AT.ATField("分数飘字")]
        ScoreText = 91,
        Count,     
    }

    public enum EPathAssetType : byte
    {
        [Framework.Plugin.AT.ATField("部位准心")]
        PartAimPoint = 0,
        [Framework.Plugin.AT.ATField("传送门")]
        TransferDoor = 1,
        [Framework.Plugin.AT.ATField("吃金币特效")]
        GetGoldEffect = 2,
        [Framework.Plugin.AT.ATField("迷宫传送门蓝")]
        MazeDoorBlue = 3,
        [Framework.Plugin.AT.ATField("迷宫传送门红")]
        MazeDoorRed = 4,
        [Framework.Plugin.AT.ATField("迷宫传送门黄")]
        MazeDoor = 5,
        [Framework.Plugin.AT.ATField("迷宫传送门紫")]
        MazeDoorPurple = 6,
        [Framework.Plugin.AT.ATField("迷宫传送门彩")]
        MazeDoorRainbow = 7,
        [Framework.Plugin.AT.ATField("迷宫绿色标记")]
        MazeGreenFlag = 8,
        [Framework.Plugin.AT.ATField("迷宫红色标记")]
        MazeRedFlag = 9,
        [Framework.Plugin.AT.ATField("迷宫黄色标记")]
        MazeYellowFlag = 10,
        [Framework.Plugin.AT.ATField("迷宫卷轴")]
        MazeJuanzhou = 11,
        [Framework.Plugin.AT.ATField("迷宫卡牌")]
        MazeKapai = 12,
        [Framework.Plugin.AT.ATField("迷宫面具")]
        MazeMainju = 13,
        [Framework.Plugin.AT.ATField("迷宫水晶")]
        MazeShuijin = 14,
        [Framework.Plugin.AT.ATField("装备合成特效1")]
        EquipRefineEffect1 = 15,
        [Framework.Plugin.AT.ATField("装备合成特效2")]
        EquipRefineEffect2 = 16,
        [Framework.Plugin.AT.ATField("装备合成特效3")]
        EquipRefineEffect3 = 17,
        [Framework.Plugin.AT.ATField("装备合成特效4")]
        EquipRefineEffect4 = 18,
        [Framework.Plugin.AT.ATField("装备合成特效5")]
        EquipRefineEffect5 = 19,
        [Framework.Plugin.AT.ATField("备战界面Buff特效")]
        BuffInfo = 20,
        [Framework.Plugin.AT.ATField("宝箱特效1")]
        OpenBoxEffect1 = 21,
        [Framework.Plugin.AT.ATField("宝箱特效2")]
        OpenBoxEffect2 = 22,
        [Framework.Plugin.AT.ATField("宝箱特效3")]
        OpenBoxEffect3 = 23,

        [Framework.Plugin.AT.ATField("禁锢连线特效")]
        ImprisonLinkEffect = 24,

        [Framework.Plugin.AT.ATField("连线Buff特效")]
        BuffLinkEffect = 25,

        [Framework.Plugin.AT.ATField("泡泡对话")]
        BubbleTalk = 29,

        [Framework.Plugin.AT.ATField("水元素光圈")]
        WaterHalo = 30,
        [Framework.Plugin.AT.ATField("火元素光圈")]
        FireHalo = 31,
        [Framework.Plugin.AT.ATField("木元素光圈")]
        WoodHalo = 32,
        [Framework.Plugin.AT.ATField("土元素光圈")]
        SoilHalo = 33,
        [Framework.Plugin.AT.ATField("电元素光圈")]
        ThunderHalo = 34,
        [Framework.Plugin.AT.ATField("抽卡蓝色品质特效")]
        DrawBlueEffect = 35,
        [Framework.Plugin.AT.ATField("抽卡蓝色加品质特效")]
        DrawBluePlusEffect = 36,
        [Framework.Plugin.AT.ATField("抽卡紫色品质特效")]
        DrawPurpleEffect = 37,
        [Framework.Plugin.AT.ATField("抽卡紫色加品质特效")]
        DrawPurplePlusEffect = 38,
        [Framework.Plugin.AT.ATField("得到蓝色品质特效")]
        GetBlueEffect = 39,
        [Framework.Plugin.AT.ATField("得到蓝色加品质特效")]
        GetBluePlusEffect = 40,
        [Framework.Plugin.AT.ATField("得到紫色品质特效")]
        GetPurpleEffect = 41,
        [Framework.Plugin.AT.ATField("得到紫色加品质特效")]
        GetPurplePlusEffect = 42,
        [Framework.Plugin.AT.ATField("抽卡绿色品质特效")]
        DrawGreenEffect = 43,
        [Framework.Plugin.AT.ATField("抽卡金品质特效")]
        DrawGoldenEffect = 44,
        [Framework.Plugin.AT.ATField("架势条特效1")]
        PostureEffect1 = 60,
        [Framework.Plugin.AT.ATField("架势条特效2")]
        PostureEffect2 = 61,
        [Framework.Plugin.AT.ATField("架势条特效3")]
        PostureEffect3 = 62,

        [Framework.Plugin.AT.ATField("铁匠铺特效")]
        FoundryEffect = 70,
        [Framework.Plugin.AT.ATField("铁匠铺Buff1激活特效")]
        FoundryBuff1Effect = 71,
        [Framework.Plugin.AT.ATField("铁匠铺Buff2激活特效")]
        FoundryBuff2Effect = 72,

        [Framework.Plugin.AT.ATField("建筑解锁特效")]
        BuidlingUnLockEffect = 80,

        [Framework.Plugin.AT.ATField("通用物品获得界面特效")]
        CommonGetRewardPanelEffect = 85,

        [Framework.Plugin.AT.ATField("角色复活特效")]
        PlayerReciveEffect = 100,

        [Framework.Plugin.AT.ATField("新装备合成特效1")]
        UIElement01 = 101,
        [Framework.Plugin.AT.ATField("新装备合成特效2")]
        UIElement02 = 102,
        [Framework.Plugin.AT.ATField("新装备合成特效3")]
        UIElement03 = 103,
        [Framework.Plugin.AT.ATField("新装备合成特效4")]
        UIElement04= 104,
        [Framework.Plugin.AT.ATField("新装备合成特效5")]
        UIElement05 = 105,

        [Framework.Plugin.AT.ATField("成就完成特效")]
        AchievementCompletedEffect = 110,

        [Framework.Plugin.AT.ATField("清理关卡障碍物完成特效")]
        ClearObstacleCompletedEffect = 111,

        [Framework.Plugin.AT.ATField("英雄背包UR品质特效")]
        UREffect = 120,
        [Framework.Plugin.AT.ATField("英雄背包SSR品质特效")]
        SSREffect = 121,
        [Framework.Plugin.AT.ATField("英雄背包SR品质特效")]
        SREffect = 122,
        [Framework.Plugin.AT.ATField("英雄背包R品质特效")]
        REffect = 123,

        [Framework.Plugin.AT.ATField("英雄背包UR星星特效")]
        URStarEffect = 124,
        [Framework.Plugin.AT.ATField("英雄背包SSR星星特效")]
        SSRStarEffect = 125,
        [Framework.Plugin.AT.ATField("英雄背包无星星特效")]
        NoneStarEffect = 126,

        [Framework.Plugin.AT.ATField("英雄背包碎片进度条特效1")]
        SliderEffect1 = 130,
        [Framework.Plugin.AT.ATField("英雄背包碎片进度条特效2")]
        SliderEffect2 = 131,
        [Framework.Plugin.AT.ATField("道具Tween")]
        ItemTween = 132,
        [Framework.Plugin.AT.ATField("小订单NPC图标")]
        WanderOrderIcon = 133,
        [Framework.Plugin.AT.ATField("小订单NPC点赞图标")]
        WanderOrderIconLike = 134,
        [Framework.Plugin.AT.ATField("备战英雄头顶信息")]
        HeroTopInfo = 135,
        [Framework.Plugin.AT.ATField("npc任务标识")]
        WanderNpcTaskMark = 136,

        Count,
    }

    [Framework.Plugin.AT.ATExportMono("常驻资源")]
    public static class PermanentAssetsUtil
    {
        [Framework.Plugin.AT.ATMethod("获取资源"),Framework.Plugin.AT.ATExportGUID(871160379)]
        public static UnityEngine.Object GetAsset(EPermanentAssetType type)
        {
            if (PermanentAssets.Instance == null || PermanentAssets.Instance.Paths == null || (int)type >= PermanentAssets.Instance.Paths.Length) return null;
            return PermanentAssets.Instance.Assets[(int)type];
        }
        //------------------------------------------------------
        public static bool GetBuffEffectMaterial(Framework.Core.EBuffEffectBit buffEffectBit, out Material material, out string propertyName)
        {
            material = null;
            propertyName = null;
            if (PermanentAssets.Instance == null || PermanentAssets.Instance.BuffMaterials == null || (int)buffEffectBit >= PermanentAssets.Instance.BuffMaterials.Length) return false;
            material = PermanentAssets.Instance.BuffMaterials[(int)buffEffectBit].asset;
            propertyName = PermanentAssets.Instance.BuffMaterials[(int)buffEffectBit].propertyName;
            return material != null;
        }
        //------------------------------------------------------
        public static bool GetAssetPath(EPathAssetType type, out string strPath, out bool bPermanent)
        {
            strPath = null;
            bPermanent = false;
            if (PermanentAssets.Instance == null || PermanentAssets.Instance.Paths == null || (int)type >= PermanentAssets.Instance.Paths.Length) return false;
            strPath = PermanentAssets.Instance.Paths[(int)type].asset;
            bPermanent = PermanentAssets.Instance.Paths[(int)type].permanent;
            return true;
        }
        //------------------------------------------------------
        public static string GetAssetPath(EPathAssetType type)
        {
            if (PermanentAssets.Instance == null || PermanentAssets.Instance.Paths == null || (int)type >= PermanentAssets.Instance.Paths.Length) return null;
            return PermanentAssets.Instance.Paths[(int)type].asset;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取元素效果材质")]
        public static Material GetElementEffectMaterial(EElementType type)
        {
            if (PermanentAssets.Instance == null || PermanentAssets.Instance.Assets == null) return null;
            switch (type)
            {
                case EElementType.Water: return GetAsset(EPermanentAssetType.WaterEleEffect) as Material;
                case EElementType.Fire: return GetAsset(EPermanentAssetType.FireEleEffect) as Material;
                case EElementType.Wood: return GetAsset(EPermanentAssetType.WoodEleEffect) as Material;
                case EElementType.Soil: return GetAsset(EPermanentAssetType.SoilEleEffect) as Material;
                case EElementType.Thunder: return GetAsset(EPermanentAssetType.ThunderEffect) as Material;
            }
            return null;
        }
        //------------------------------------------------------
        public static string GetElementHaloAssetPath(EElementType type)
        {
            if (PermanentAssets.Instance == null || PermanentAssets.Instance.Paths == null) return null;
            switch(type)
            {
                case EElementType.Water: return GetAssetPath(EPathAssetType.WaterHalo);
                case EElementType.Fire: return GetAssetPath(EPathAssetType.FireHalo);
                case EElementType.Wood: return GetAssetPath(EPathAssetType.WoodHalo);
                case EElementType.Soil: return GetAssetPath(EPathAssetType.SoilHalo);
                case EElementType.Thunder: return GetAssetPath(EPathAssetType.ThunderHalo);
            }
            return null;
        }
        //------------------------------------------------------
        public static T GetAsset<T>(EPermanentAssetType type) where T : UnityEngine.Object
        {
            if (PermanentAssets.Instance == null || PermanentAssets.Instance.Assets ==null || (int)type >= PermanentAssets.Instance.Assets.Length) return null;
            return PermanentAssets.Instance.Assets[(int)type] as T;
        }
    }
}
