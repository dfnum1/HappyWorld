/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	BasePropertyLerp
作    者:	HappLI
描    述:	基础属性过渡
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core
{
    public abstract class BasePropertyLerp
    {
        public float fLerp;
        public float fDuration;
        public string propertyName = null;

        public bool materialBlock = true;
        public bool materialShare = true;
        public int materialIndex = -1;

        public List<Renderer> renders = null;
        protected UnityEngine.Object m_binderObject = null;
        public UnityEngine.Object binderObject
        {
            get { return m_binderObject; }
            set
            {
                m_binderObject = value;
                m_pAble = null;
                if (value!=null)
                {
                    if (m_binderObject is AInstanceAble)
                        m_pAble = m_binderObject as AInstanceAble;
                }
            }
        }
        protected AInstanceAble m_pAble;
        public virtual bool Update(float fFrameTime)
        {
            if (renders == null || string.IsNullOrEmpty(propertyName)) return false;
            fLerp += fFrameTime;
            InnerUpdate();
            if (fLerp >= fDuration)
            {
                Clear();
                return false;
            }
            return true;
        }

        public virtual void Clear()
        {
            fLerp = 0;
            fDuration = 0;
            propertyName = null;
            binderObject = null;

            materialBlock = true;
            materialShare = true;
            materialIndex = -1;
        }
        protected abstract void InnerUpdate();
        public virtual void Destroy()
        {
            Clear();
        }
    }
}

