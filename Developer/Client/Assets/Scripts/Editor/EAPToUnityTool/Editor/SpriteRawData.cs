#if UNITY_EDITOR
using System;
using UnityEngine;

namespace TopGame.AEPToUnity
{
	public class SpriteRawData
	{
		public Sprite sprite = null;

		public int hashId = 0;

		public Vector2 pivot;

		public int alignment;

		public string name;

		public bool isAttact = true;

		public SpriteBuildStatus spriteStaus = SpriteBuildStatus.OLD;

		public SpriteRawData(Sprite _sprite, string _name, Vector2 _pivot, int _alignment)
		{
			this.name = _name;
			this.sprite = _sprite;
			if (this.sprite != null)
			{
				this.hashId = this.sprite.GetInstanceID();
			}
			this.pivot = _pivot;
			this.alignment = _alignment;
			this.spriteStaus = SpriteBuildStatus.OLD;
			this.isAttact = true;
		}
	}
}
#endif