/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	EGameState
作    者:	HappLI
描    述:	游戏状态
*********************************************************************/

namespace TopGame.Logic
{
    public enum EGameState : byte
    {
        [Framework.Plugin.PluginDisplay("加载")]
        Loading = 0,
        [Framework.Plugin.PluginDisplay("版本验证")]
        VersionCheck = 1,
        [Framework.Plugin.PluginDisplay("Logo")]
        Logo = 2,
        [Framework.Plugin.PluginDisplay("登录")]
        Login = 3,
        [Framework.Plugin.PluginDisplay("城市")]
        Hall =4,
        [Framework.Plugin.PluginDisplay("战斗")]
        Battle = 5,
        [Framework.Plugin.PluginDisplay("大世界")]
        BigWorld = 6,
        [Framework.Plugin.DisableGUI]
        Count,
    }
    //------------------------------------------------------
    public enum EMode : byte
    {
        None = 0,
        City = SvrData.ELocationState.City,
        PVE = SvrData.ELocationState.Pve,
        Count,
    }
}
