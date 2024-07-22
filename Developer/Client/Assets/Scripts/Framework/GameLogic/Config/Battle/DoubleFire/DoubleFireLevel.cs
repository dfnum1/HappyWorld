/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	DoubleFireLevel
作    者:	HappLI
描    述:	对射玩法关卡
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Logic
{
    public class DoubleFireLevel : Framework.Core.AInstanceAble
    {
        public Transform player0;
        public Transform player1;

        public static System.Action<DoubleFireLevel> OnLevelLoaded;
        //------------------------------------------------------
        public override void OnPoolStart()
        {
            base.OnPoolStart();
            if (OnLevelLoaded != null) OnLevelLoaded(this);
        }
        //------------------------------------------------------
        public override void OnRecyle()
        {
            base.OnRecyle();
            OnLevelLoaded = null;
        }
    }
}

