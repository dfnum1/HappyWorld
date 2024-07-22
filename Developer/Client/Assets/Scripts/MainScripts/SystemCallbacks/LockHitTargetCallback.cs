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
using Framework.Core;
using Framework.Base;
using Framework.Data;

namespace TopGame
{
    public partial class GameInstance
    {
        public override bool OnLockHitTargetCondition(AWorldNode pTrigger, AWorldNode pTarget, ELockHitType hitType, ELockHitCondition condition, Vector3Int conditionParam)
        {
            if (m_States != null && m_States.GetCurrentState()!=null)
            {
                bool bSucceed = m_States.GetCurrentState().OnLockHitTargetCondition(pTrigger, pTarget, hitType, condition, conditionParam);
                if (!bSucceed) return false;
            }
            return base.OnLockHitTargetCondition(pTrigger, pTarget, hitType, condition, conditionParam);
        }
    }
}
