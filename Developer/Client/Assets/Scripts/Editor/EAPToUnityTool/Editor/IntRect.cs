#if UNITY_EDITOR
using System;

namespace TopGame.AEPToUnity
{
	public class IntRect
	{
		public int x;

		public int y;

		public int width;

		public int height;

		public IntRect()
		{
		}

		public IntRect(int _x, int _y, int _w, int _h)
		{
			this.x = _x;
			this.y = _y;
			this.width = _w;
			this.height = _h;
		}

		public string ToText()
		{
			return string.Concat(new object[]
			{
				"{",
				this.x,
				",",
				this.y,
				",",
				this.width,
				",",
				this.height,
				"}"
			});
		}
	}
}
#endif