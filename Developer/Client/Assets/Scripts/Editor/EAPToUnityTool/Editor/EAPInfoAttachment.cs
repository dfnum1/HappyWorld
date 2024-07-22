#if UNITY_EDITOR
using System;

namespace TopGame.AEPToUnity
{
	public class EAPInfoAttachment
	{
		public string slotName;

		public string spriteName;

		public float x;

		public float y;

		public int depth = -100;

		public float width;

		public float height;

		public float scaleX = 1f;

		public float scaleY = 1f;

		public float rotation = 0f;

		public int startX;

		public int startY;

		public IntRect originalRect;

		public IntRect optimizeRect;

		public bool isOptimze = false;

		public float offsetX = 0f;

		public float offsetY = 0f;

		public EAPInfoAttachment()
		{
		}

		public EAPInfoAttachment(string _spriteName, string _Slotname, int _depth, float x, float y, float _offsetX, float _offsetY)
		{
			this.slotName = _Slotname;
			this.spriteName = _spriteName;
			this.x = x;
			this.y = y;
			this.depth = _depth;
			this.offsetX = _offsetX;
			this.offsetY = _offsetY;
		}

		public void SetCache(int _startX, int _startY, IntRect _originalRect, IntRect _optimizeRect)
		{
			this.isOptimze = true;
			this.startX = _startX;
			this.startY = _startY;
			this.originalRect = _originalRect;
			this.optimizeRect = _optimizeRect;
		}
	}
}

#endif