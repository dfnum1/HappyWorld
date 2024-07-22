/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIBase
作    者:	HappLI
描    述:	UI管理器
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TopGame.UI
{
    public enum EUIAttr
    {
        UI,
        Logic,
        View,
    }
    public class UIAttribute : Attribute
    {
        public ushort uiType = 0;
        public EUIAttr attri = EUIAttr.View;
        public bool isAuto = false;
        public UIAttribute(ushort type, EUIAttr attr, bool bAuto = false)
        {
            uiType = type;
            this.attri = attr;
            isAuto = bAuto;
        }
    }
}
