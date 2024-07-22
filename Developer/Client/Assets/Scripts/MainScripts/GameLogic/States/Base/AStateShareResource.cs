/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AStateShareResource
作    者:	HappLI
描    述:	游戏状态逻辑共享资源
*********************************************************************/

using Framework.Plugin.AT;
using System.Collections.Generic;
using System;
using TopGame.UI;
using TopGame.Core;

namespace TopGame.Logic
{
    public abstract class AStateShareResource : Framework.Plugin.AT.IUserData
    {
        //------------------------------------------------------
        public virtual void Awake(AState pState) { }
        //------------------------------------------------------
        public virtual void Start(AState pState) { }
        //------------------------------------------------------
        public virtual void PreStart(AState pState) { }
        //------------------------------------------------------
        public virtual void Exit(AState pState) { }
        //------------------------------------------------------
        public virtual void Enable(AState pState, bool bEnable) { }
        //------------------------------------------------------
        public virtual void Update(AState pState, float fFrameTime) { }
        //------------------------------------------------------
        public void Destroy() { }
        //------------------------------------------------------
        public static T CastGet<T>(bool bAuto =true) where T : AStateShareResource, new()
        {
            return StateFactory.ShareResource<T>(bAuto);
        }
        //------------------------------------------------------
#if UNITY_EDITOR
        public virtual void DrawGizmos(AState pState) { }
        public virtual string Print() { return ""; }
#endif

    }
}
