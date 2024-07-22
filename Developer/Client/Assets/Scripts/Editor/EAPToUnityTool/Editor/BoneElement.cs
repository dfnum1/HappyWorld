#if UNITY_EDITOR
using System;

namespace TopGame.AEPToUnity
{
	public class BoneElement
	{
		public string name;

		public string parent;

		public float x;

		public float y;

		public float scaleX = 1f;

		public float scaleY = 1f;

		public float length = 0f;

		public float rotation;

		public int index = 0;
	}
}
#endif