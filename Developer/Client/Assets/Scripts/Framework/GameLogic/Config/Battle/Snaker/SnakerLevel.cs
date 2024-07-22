/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	SnakerLevel
作    者:	HappLI
描    述:	蛇形玩法关卡
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Logic
{
    public class SnakerLevel : Framework.Core.AInstanceAble
    {
        public Transform startPos;
        public List<Transform> vNps = new List<Transform>();
    }
}

