#if UNITY_EDITOR
using System;
using UnityEngine;

namespace TopGame.AEPToUnity
{
	public class AEPInfoDataImage
	{
		public string name;

		public Rect rect;

		public Vector2 pivot;

		public AEPInfoDataImage(string _name, Rect _rect, float x, float y)
		{
			this.name = _name;
			this.rect = _rect;
			this.pivot = new Vector2(x, y);
		}
	}
}
#endif