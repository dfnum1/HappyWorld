/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Hall
作    者:	HappLI
描    述:	大厅状态
*********************************************************************/

using System;
using Framework.Module;
using TopGame.UI;
using UnityEngine;
using TopGame.SvrData;
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Data;
using Framework.Plugin.AT;
using TopGame.Base;

namespace TopGame.Logic
{
    [State(EGameState.Hall, 2)]
    public class Hall : AState
    {
        //------------------------------------------------------
        public override void OnInit(EGameState state)
        {
            base.OnInit(state);
        }
        //------------------------------------------------------
        public override void OnPreStart()
        {
            CameraKit.Enable(true);

            ASceneTheme themer = Core.SceneMgr.GetThemer();
            if (themer != null) themer.Clear();
            base.OnPreStart();
            Transform pTransform = Framework.Core.DyncmicTransformCollects.FindTransformByName("main_city");
            if (pTransform != null) pTransform.gameObject.SetActive(true);
            if(MagicaCloth.MagicaPhysicsManager.IsInstance())
                MagicaCloth.MagicaPhysicsManager.Instance.enabled = true;
        }
        //------------------------------------------------------
        public override void OnStart()
        {
            base.OnStart();
            MagicaCloth.MagicaPhysicsManager.Enable(true);
        }
        //------------------------------------------------------
        public override void OnExit()
        {
            base.OnExit();
            Transform pTransform = Framework.Core.DyncmicTransformCollects.FindTransformByName("main_city");
            if (pTransform != null) pTransform.gameObject.SetActive(false);
        }
        //------------------------------------------------------
#if UNITY_EDITOR
        //------------------------------------------------------
        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
        }
#endif

        
    }
}
