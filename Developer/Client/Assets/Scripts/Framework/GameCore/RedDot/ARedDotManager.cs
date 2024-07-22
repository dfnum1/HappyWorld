/********************************************************************
生成日期:	2020-11-24
类    名: 	RedDotManager
作    者:	
描    述:	红点管理器
*********************************************************************/


using Framework.Core;
using UnityEngine;

namespace TopGame.Core
{
    public abstract class ARedDotManager : Framework.Module.AModule
    {
        public delegate bool IsSatisfyCondition();
        public abstract void BindRedDotGameObject(int uiType, int dotType, GameObject reddot);
        public abstract void UnBindRedDotGameObject(int ui, int type, GameObject reddot);
        public abstract void OnRedDotClickedCB(int ui, int type);
        public abstract void UpdateRedDot(int ui, int type);
        public abstract void Register(int ui, int type, IsSatisfyCondition condition, bool isIgnoreClick = false);
    }
}