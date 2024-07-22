/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GameInstance
作    者:	HappLI
描    述:	游戏主线程
*********************************************************************/

using System.Collections.Generic;
using TopGame.Base;
using TopGame.Core;
using TopGame.Data;
using TopGame.Logic;
using Framework.Module;
using UnityEngine;
using TopGame.Net;
using System;
using Framework.Plugin.AT;
using Framework.Plugin.AI;
using Framework.Plugin.Guide;
using TopGame.UI;
using TopGame.SvrData;

namespace TopGame
{
    public partial class GameInstance
    {
        //------------------------------------------------------
        public override void OnAnimPathBegin(IPlayableBase playAble)
        {
            base.OnAnimPathBegin(playAble);
            if (playAble.CanSkip() == false)
            {
                return;
            }
        }
        //------------------------------------------------------
        public override void OnAnimPathEnd(IPlayableBase playAble)
        {
            base.OnAnimPathEnd(playAble);
        }
        //------------------------------------------------------
        public override void OnAnimPathUpdate(IPlayableBase playAble)
        {
            base.OnAnimPathUpdate(playAble);
            if (playAble == null)
            {
                return;
            }
        }
    }
}
