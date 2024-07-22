/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	TrackUI
作    者:	Happli
描    述:	UI 连锁数据
*********************************************************************/
using Framework.Core;
using TopGame.Core;

namespace TopGame.UI
{
    public struct TrackUI : VariablePoolAble
    {
        public UI.EUIType uiType;
        public VariablePoolAble userData;
        public TrackUI(UI.EUIType ui, VariablePoolAble userData = null)
        {
            this.userData = userData;
            uiType = ui;
        }
        public void Destroy()
        {
            this.userData = null;
        }
    }
}
