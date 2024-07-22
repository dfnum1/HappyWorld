#if UNITY_EDITOR
using System;

namespace TopGame.AEPToUnity
{
	public class RawAEPJsonAttachment
	{
		public string name;

		public float x;

		public float y;

		public float width;

		public float height;

		public float scaleX = 1f;

		public float scaleY = 1f;

		public float rotation = 0f;
	}
}

#endif