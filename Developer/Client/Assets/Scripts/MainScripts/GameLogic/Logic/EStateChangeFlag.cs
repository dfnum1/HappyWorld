
namespace TopGame.Logic
{
    public enum EStateChangeFlag
    {
        [Framework.Plugin.DisableGUI] None = 0,
        [Framework.Plugin.PluginDisplay("Nothing")] Nothing = 1 << 0,
        [Framework.Plugin.PluginDisplay("场景加载")] SceneID = 1 << 2,
        [Framework.Plugin.PluginDisplay("关闭所有UI")] HideAllUI = 1 << 3,
        [Framework.Plugin.PluginDisplay("延迟切换")] DelayChange = 1 << 4,
        [Framework.Plugin.PluginDisplay("释放缓冲池")] PoolFree = 1 << 5,

        [Framework.Plugin.DisableGUI] All = SceneID | HideAllUI | DelayChange| PoolFree,
        [Framework.Plugin.DisableGUI] AllNoHideAllUI = SceneID | DelayChange | PoolFree,
    }
}
