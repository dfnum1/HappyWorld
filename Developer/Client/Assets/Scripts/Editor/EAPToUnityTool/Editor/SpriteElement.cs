#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
namespace TopGame.AEPToUnity
{
	public class SpriteElement
	{
		public Texture2D texture;

		public IntRect originalRect;

		public IntRect optimizeRect;

		public int startX;

		public int startY;

		public string name;

		public int alignment;

		public Vector2 pivot;

		private bool isOptimize;

		private bool needSetAuthority;

		public SpriteElement(Texture2D _texture, bool _needSetAuthority = true)
		{
			this.needSetAuthority = _needSetAuthority;
			if (this.needSetAuthority)
			{
				ImportTextureUtil.MaxImportSettings(_texture);
			}
			this.texture = _texture;
			this.name = this.texture.name;
			this.originalRect = new IntRect(0, 0, this.texture.width, this.texture.height);
			this.optimizeRect = new IntRect(0, 0, this.texture.width, this.texture.height);
			this.isOptimize = false;
			this.startX = 0;
			this.startY = 0;
			this.alignment = 0;
			this.pivot = new Vector2(0.5f, 0.5f);
			if (this.needSetAuthority)
			{
				string assetPath = AssetDatabase.GetAssetPath(this.texture);
				TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
				if (textureImporter.textureType == TextureImporterType.Sprite)
				{
					if (textureImporter.spriteImportMode == SpriteImportMode.Single)
					{
						this.pivot = textureImporter.spritePivot;
						this.alignment = ImportTextureUtil.GetAlignment(this.pivot);
					}
					else if (textureImporter.spritesheet != null && textureImporter.spritesheet.Length > 0)
					{
						this.alignment = textureImporter.spritesheet[0].alignment;
						this.pivot = textureImporter.spritesheet[0].pivot;
					}
				}
			}
		}

		public bool IsOptimize()
		{
			return this.isOptimize;
		}

		public void SetPivot(Vector2 _vecPivot)
		{
			this.pivot = _vecPivot;
			this.alignment = ImportTextureUtil.GetAlignment(this.pivot);
		}

		public void CloneFromOriginTexture()
		{
			if (this.needSetAuthority)
			{
				ImportTextureUtil.MaxImportSettings(this.texture);
				ImportTextureUtil.ReadAndUnScale(this.texture);
				Texture2D texture2D = new Texture2D(this.texture.width, this.texture.height);
				texture2D.SetPixels(this.texture.GetPixels(0, 0, this.texture.width, this.texture.height));
				texture2D.Apply();
				this.texture = texture2D;
			}
		}

		public bool TrimTexture(bool deleteOld = false)
		{
			if (this.needSetAuthority)
			{
				ImportTextureUtil.MakeReadable(this.texture);
			}
			Color32[] pixels = this.texture.GetPixels32();
			int num = this.texture.width;
			int num2 = 1;
			int num3 = this.texture.height;
			int num4 = 1;
			int width = this.texture.width;
			int height = this.texture.height;
			int i = 0;
			int num5 = height;
			while (i < num5)
			{
				int j = 0;
				int num6 = width;
				while (j < num6)
				{
					Color32 color = pixels[i * num6 + j];
					if (color.a != 0)
					{
						if (i < num3)
						{
							num3 = i;
						}
						if (i > num4 - 1)
						{
							num4 = i + 1;
						}
						if (j < num)
						{
							num = j;
						}
						if (j > num2 - 1)
						{
							num2 = j + 1;
						}
					}
					j++;
				}
				i++;
			}
			bool result;
			if (num > 0 || num3 > 0 || num2 < this.originalRect.width || num4 < this.originalRect.height)
			{
				this.isOptimize = true;
				int num7 = num2 - num;
				int num8 = num4 - num3;
				bool flag = false;
				if (num7 < 0)
				{
					num = 0;
					num7 = 1;
					flag = true;
				}
				if (num8 < 0)
				{
					num3 = 0;
					num8 = 1;
					flag = true;
				}
				Texture2D texture2D = new Texture2D(num2 - num, num8);
				texture2D.SetPixels(this.texture.GetPixels(num, num3, num7, num8));
				texture2D.Apply();
				if (deleteOld)
				{
					Object.DestroyImmediate(this.texture);
				}
				this.texture = texture2D;
				if (!flag)
				{
					this.optimizeRect = new IntRect(0, 0, this.texture.width, this.texture.height);
				}
				else
				{
					this.optimizeRect = new IntRect(num, num3, num7, num8);
				}
				this.startX = num;
				this.startY = num3;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public void FreeMemory()
		{
			this.texture = null;
		}

		public Rect GetSpriteRect()
		{
			return new Rect((float)this.optimizeRect.x, (float)this.optimizeRect.y, (float)this.optimizeRect.width, (float)this.optimizeRect.height);
		}
	}
}
#endif