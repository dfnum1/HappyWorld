/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ADBType
作    者:	HappLI
描    述:	服务器数据类型
*********************************************************************/
using System;
namespace TopGame.Data
{
    public class DBTypeAttribute : Attribute
    {
        public EDBType type;
        public DBTypeAttribute(EDBType type)
        {
            this.type = type;
        }
    }
    //------------------------------------------------------
    //! 数据类型
    //------------------------------------------------------
    public enum EDBType
    {
#if USE_SERVER
        Battle,
        Heros,
#else
        [Framework.Plugin.PluginDisplay("基础/基础数据DB")]BaseInfo = 0,
        [Framework.Plugin.PluginDisplay("英雄/英雄数据DB")]Heros,
        [Framework.Plugin.PluginDisplay("战斗玩法/战斗数据DB")]Battle,
        [Framework.Plugin.PluginDisplay("基础/成就数据DB")] Achievement,
        [Framework.Plugin.PluginDisplay("基础/物品数据DB")] Item,
        [Framework.Plugin.PluginDisplay("基础/排行榜数据DB")] Rank,
        [Framework.Plugin.PluginDisplay("基础/装备数据DB")] Equip,
        [Framework.Plugin.PluginDisplay("基础/好友数据DB")] Friend,
        [Framework.Plugin.PluginDisplay("战斗玩法/房间战斗数据DB")] BattleRoom,
        [Framework.Plugin.PluginDisplay("基础/签到DB")] SignIn,
        [Framework.Plugin.PluginDisplay("基础/商店DB")] Shop,
        [Framework.Plugin.PluginDisplay("基础/挂机DB")] HnagUp,
        [Framework.Plugin.PluginDisplay("基础/宠物DB")] Pet,
        [Framework.Plugin.PluginDisplay("基础/系统解锁DB")] SystemUnlock,
        [Framework.Plugin.PluginDisplay("基础/邮件DB")] Mail,
#endif
        [Framework.Plugin.DisableGUI] Count,
    }
}
