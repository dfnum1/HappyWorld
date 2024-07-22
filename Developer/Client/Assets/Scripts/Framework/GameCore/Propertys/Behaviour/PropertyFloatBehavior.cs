/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	PropertyBehavior
作    者:	HappLI
描    述:	属性挂件
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
namespace TopGame.Core
{
    public class PropertyFloatBehavior : PropertyBehavior
    {
        public float start;
        public float end;

        //------------------------------------------------------
        protected override void DoExcude(float fFactor)
        {
            MaterailBlockUtil.SetRendersFloat(m_pRenders, strPropertyName, start * (1 - fFactor) + end * fFactor, bUseBlock, materialShare, materialIndex);
        }
    }
}

