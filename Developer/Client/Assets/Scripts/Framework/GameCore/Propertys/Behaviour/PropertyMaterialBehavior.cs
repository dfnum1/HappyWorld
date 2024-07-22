/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	PropertyMaterialBehavior
作    者:	HappLI
描    述:	属性挂件
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core
{
    public class PropertyMaterialBehavior : PropertyBehavior
    {
        public Material start;
        public Material end;
        //------------------------------------------------------
        public override void Reset(float fPlayTime = 0)
        {
            base.Reset(fPlayTime);
            if (m_nPropertyNameID == 0)
            {
                m_nPropertyNameID = -1;
                for (int i = 0; i < m_pRenders.Length; ++i)
                {
                    if (m_pRenders[i])
                    {
                        m_pRenders[i].material = start;
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_pRenders.Length; ++i)
                {
                    if (m_pRenders[i])
                    {
                        m_pRenders[i].materials = new Material[] { start, end };
                    }
                }
            }
        }
        //------------------------------------------------------
        protected override void DoExcude(float fFactor)
        {
            if(m_nPropertyNameID>0)
            {
           //     if (start != null) start.SetFloat(m_nPropertyNameID, fFactor);
                if (end != null) end.SetFloat(m_nPropertyNameID, fFactor);
            }
            else
            {
                for (int i = 0; i < m_pRenders.Length; ++i)
                {
                    if (m_pRenders[i])
                    {
                        m_pRenders[i].material.Lerp(start, end, fFactor);
                    }
                }
            }

        }
    }
}

