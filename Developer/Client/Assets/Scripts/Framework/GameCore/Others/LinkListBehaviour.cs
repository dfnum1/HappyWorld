/********************************************************************
生成日期:	10:7:2019   12:11
类    名: 	LinkListBehaviour
作    者:	HappLI
描    述:	Behaviour 链表
*********************************************************************/
using UnityEngine;

namespace TopGame.Core
{
	public class LinkListBehaviour<T> : MonoBehaviour where T : MonoBehaviour 
	{
		static LinkListBehaviour<T>			ms_Header = null;
		static LinkListBehaviour<T>			ms_Tailer = null;
        //------------------------------------------------------
        public static LinkListBehaviour<T>	Header
		{
			get
			{
				return ms_Header;
			}
		}
        //------------------------------------------------------
        public static LinkListBehaviour<T>	Tailer
		{
			get
			{
				return ms_Tailer;
			}
		}
        //------------------------------------------------------
        LinkListBehaviour<T>				m_Prev		= null;
        LinkListBehaviour<T>				m_Next		= null;
		bool								m_IsInList	= false;
        //------------------------------------------------------
        public LinkListBehaviour<T>			Prev
		{
			get
			{
				return m_Prev;
			}
		}
        //------------------------------------------------------
        public LinkListBehaviour<T>			Next
		{
			get
			{
				return m_Next;
			}
		}
        //------------------------------------------------------
        public bool							IsInList
		{
			get
			{
				return m_IsInList;
			}
		}
        //------------------------------------------------------
        public virtual void Insert()
		{
			if (IsInList)
				return;

			if (ms_Header == null)
			{
				ms_Header = this;
			}

			if (ms_Tailer == null)
			{
				ms_Tailer = this;
			}
			else
			{
				ms_Tailer.m_Next = this;
				m_Prev = ms_Tailer;
			}
			
			ms_Tailer = this;

			m_IsInList = true;
		}
        //------------------------------------------------------
        public virtual void Remove()
		{
			if (!IsInList)
				return;

			if (Prev != null)
			{
				Prev.m_Next = Next;
			}

			if (Next != null)
			{
				Next.m_Prev = Prev;
			}

			if (ms_Header == this)
			{
				ms_Header = m_Next;
			}

			if (ms_Tailer == this)
			{
				ms_Tailer = m_Prev;
			}

			m_Prev = null;
			m_Next = null;
			m_IsInList = false;
		}
        //------------------------------------------------------
		void OnDestroy()
		{
			Remove();	
		}
		//------------------------------------------------------
		protected virtual void Awake()
        {
            Insert();
        }
    }
}