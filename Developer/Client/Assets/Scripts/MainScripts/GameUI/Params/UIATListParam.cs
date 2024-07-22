/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIATListParam
作    者:	HappLI
描    述:	UI 列表项数据AT回调参数
*********************************************************************/

using Framework.Core;

namespace TopGame.UI
{
    [Framework.Plugin.AT.ATExportMono("UI通用工具")]
    public struct UIATListParam : VariablePoolAble
    {
        [Framework.Plugin.AT.ATField("序列化组件", null, "", 1)]
        public UISerialized pSerialized;

        [Framework.Plugin.AT.ATField("列表回调数据", null, "", 1)]
        public Framework.Plugin.AT.IUserData pUserData;
        
        public void Destroy()
        {
            pSerialized = null;
            pUserData = null;
        }
        //------------------------------------------------------
        public UIATListParam(UISerialized pSerialized, Framework.Plugin.AT.IUserData pUserData)
        {
            this.pSerialized = pSerialized;
            this.pUserData = pUserData;
        }
    }
}
