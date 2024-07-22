/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	SensitiveNode
作    者:	HappLI
描    述:	敏感词节点
*********************************************************************/

using System;
using System.Collections.Generic;

namespace TopGame.Data
{
    //------------------------------------------------------
    public class SensitiveNode
    {
        private bool m_isEnd = false;
        private Dictionary<char, SensitiveNode> m_Childs = new Dictionary<char, SensitiveNode>();
        public void AddSubNode(char key, SensitiveNode node)
        {
            m_Childs.Add(key, node);
        }
        //------------------------------------------------------
        public SensitiveNode GetChid(char key)
        {
            SensitiveNode sN;
            if (m_Childs.TryGetValue(key, out sN)) return sN;
            return null;
        }
        //------------------------------------------------------
        public bool isWordEnd
        {
            get { return m_isEnd; }
            set
            {
                m_isEnd = value;
            }
        }
    }
}