#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace TopGame.ED
{
	public class TreeNode<T>
	{
        public delegate bool TraversalDataDelegate(T data);
		public delegate bool TraversalNodeDelegate(TreeNode<T> node);

		private readonly T _data;
		private readonly TreeNode<T> _parent;
		private readonly int _level;
		private readonly List<TreeNode<T>> _children;

		public TreeNode(T data)
		{
			_data = data;
			_children = new List<TreeNode<T>>();
			_level = 0;
		}

		public TreeNode(T data, TreeNode<T> parent) : this(data)
		{
			_parent = parent;
			_level = _parent!=null ? _parent.Level+1 : 0;
		}

		public int Level { get { return _level; } }
		public int Count { get { return _children.Count; }}
		public bool IsRoot { get { return _parent==null; }}
		public bool IsLeaf { get { return _children.Count==0; }}
		public T Data { get { return _data; }}
		public TreeNode<T> Parent { get { return _parent; }}

		public TreeNode<T> this[int key]
		{
			get { return _children[key]; }
		}

		public void Clear()
		{
			_children.Clear();
		}

		public TreeNode<T> AddChild(T value)
		{
			TreeNode<T> node = new TreeNode<T>(value,this);
			_children.Add(node);

			return node;
		}

		public bool HasChild(T data)
		{
			return FindInChildren(data)!=null;
		}

		public TreeNode<T> FindInChildren(T data)
		{
			int i = 0, l = Count;
			for(; i<l; ++i){
				TreeNode<T> child = _children[i];
				if(child.Data.Equals(data)) return child;
			}

			return null;
		}

		public bool RemoveChild(TreeNode<T> node)
		{
			return _children.Remove(node);
		}

		public void Traverse(TraversalDataDelegate handler)
		{
			if(handler(_data)){
				int i = 0, l = Count;
				for(; i<l; ++i) _children[i].Traverse(handler);
			}
		}

		public void Traverse(TraversalNodeDelegate handler)
		{
			if(handler(this)){
				int i = 0, l = Count;
				for(; i<l; ++i) _children[i].Traverse(handler);
			}
		}
        public TreeNode<T> TraverseNeighborSelect(TreeNode<T> data, bool bPrev)
        {
            TreeNode<T> returnNode = null;
            for (int i =0; i < Count; ++i)
            {
                if(_children[i] == data)
                {
                    if(bPrev)
                    {
                        if (i > 0) returnNode = _children[i - 1];
                        else
                        {
                            if (_children[i]._parent != null)
                            {
                                if (_children[i]._parent.Count > 0)
                                {
                                    return _children[i]._parent._children[_children[i]._parent.Count-1];
                                }
                                else
                                    return _children[i]._parent;
                            }
                            else return null;
                        }
                    }
                    else
                    {
                        if(i+1 < Count) returnNode = _children[i+1];
                        else
                        {
                            if (_children[i]._children.Count > 0)
                            {
                                return _children[i]._children[_children[i]._children.Count - 1];
                            }
                            else return null;
                        }
                    }
                }

                returnNode = _children[i].TraverseNeighborSelect(data, bPrev);
                if (returnNode != null) return returnNode;
            }
            return returnNode;
        }
    }
}
#endif