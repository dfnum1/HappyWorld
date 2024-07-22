/********************************************************************
生成日期:	6:17:2020 15:16
类    名: 	VariableMono
作    者:	zdq
描    述:	挂载在界面上,方便调试界面效果的参数
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    public class VariableMono : MonoBehaviour
    {
        [Range(0, 1)]
        public float FloatValue01;

        public float FloatValue;
        public bool BoolValue;
        public int IntValue;
    }
}