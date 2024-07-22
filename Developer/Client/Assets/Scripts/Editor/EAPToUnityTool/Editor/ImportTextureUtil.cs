#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace TopGame.AEPToUnity
{
	public class ImportTextureUtil
	{
		public static Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] array = new Color[width * height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = col;
			}
			Texture2D texture2D = new Texture2D(width, height);
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}

		public static Texture2D[] MaxImportSettings(Texture2D[] imgs)
		{
			for (int i = 0; i < imgs.Length; i++)
			{
				if (AssetDatabase.GetAssetPath(imgs[i]) != null)
				{
					TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(imgs[i])) as TextureImporter;
					textureImporter.isReadable=(true);
					textureImporter.textureFormat= TextureImporterFormat.ARGB32;
					textureImporter.npotScale=(0);
					textureImporter.textureType= TextureImporterType.GUI;
					AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(imgs[i]), ImportAssetOptions.ForceUpdate);
				}
			}
			return imgs;
		}

		public static Texture2D MaxImportSettings(Texture2D img)
		{
			if (AssetDatabase.GetAssetPath(img) != null)
			{
				TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(img)) as TextureImporter;
				textureImporter.isReadable=(true);
				textureImporter.textureFormat= TextureImporterFormat.ARGB32;
				textureImporter.npotScale=(0);
				textureImporter.textureType= TextureImporterType.Sprite;
				textureImporter.maxTextureSize=(4096);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(img), ImportAssetOptions.ForceUpdate);
			}
			return img;
		}

		public static Texture2D MaxImportSettings2(Texture2D img)
		{
			if (AssetDatabase.GetAssetPath(img) != null)
			{
				TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(img)) as TextureImporter;
				textureImporter.textureFormat= TextureImporterFormat.ARGB32;
				textureImporter.npotScale=(0);
				textureImporter.textureType= TextureImporterType.Sprite;
				textureImporter.isReadable=(true);
				textureImporter.maxTextureSize=(4096);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(img), ImportAssetOptions.ForceUpdate);
			}
			return img;
		}

		public static Texture2D ReadAndUnScale(Texture2D img)
		{
			if (AssetDatabase.GetAssetPath(img) != null)
			{
				TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(img)) as TextureImporter;
				textureImporter.isReadable=(true);
				textureImporter.textureFormat= TextureImporterFormat.ARGB32;
				textureImporter.npotScale=(0);
				textureImporter.maxTextureSize=(4096);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(img), ImportAssetOptions.ForceUpdate);
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			}
			return img;
		}

		public static Texture2D MakeReadable(Texture2D img)
		{
			if (AssetDatabase.GetAssetPath(img) != null)
			{
				TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(img)) as TextureImporter;
				textureImporter.isReadable=(true);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(img), ImportAssetOptions.ForceUpdate);
			}
			return img;
		}

		public static Vector2 GetPivotFromMetaSprite(SpriteMetaData meta)
		{
			Vector2 result;
			switch (meta.alignment)
			{
			case 0:
				result = new Vector2(0.5f, 0.5f);
				break;
			case 1:
				result = new Vector2(0f, 1f);
				break;
			case 2:
				result = new Vector2(0.5f, 1f);
				break;
			case 3:
				result = new Vector2(1f, 1f);
				break;
			case 4:
				result = new Vector2(0f, 0.5f);
				break;
			case 5:
				result = new Vector2(1f, 0.5f);
				break;
			case 6:
				result = new Vector2(0f, 0f);
				break;
			case 7:
				result = new Vector2(0.5f, 0f);
				break;
			case 8:
				result = new Vector2(1f, 0f);
				break;
			default:
				result = meta.pivot;
				break;
			}
			return result;
		}

		public static int GetAlignment(Vector2 vec)
		{
			float num = 0.0001f;
			int result;
			if (Mathf.Abs(vec.x - 0.5f) < num && Mathf.Abs(vec.y - 0.5f) < num)
			{
				result = 0;
			}
			else if (Mathf.Abs(vec.x) < num && Mathf.Abs(vec.y - 1f) < num)
			{
				result = 1;
			}
			else if (Mathf.Abs(vec.x - 0.5f) < num && Mathf.Abs(vec.y - 1f) < num)
			{
				result = 2;
			}
			else if (Mathf.Abs(vec.x - 1f) < num && Mathf.Abs(vec.y - 1f) < num)
			{
				result = 3;
			}
			else if (Mathf.Abs(vec.x) < num && Mathf.Abs(vec.y - 0.5f) < num)
			{
				result = 4;
			}
			else if (Mathf.Abs(vec.x - 1f) < num && Mathf.Abs(vec.y - 0.5f) < num)
			{
				result = 5;
			}
			else if (Mathf.Abs(vec.x) < num && Mathf.Abs(vec.y) < num)
			{
				result = 6;
			}
			else if (Mathf.Abs(vec.x - 0.5f) < num && Mathf.Abs(vec.y) < num)
			{
				result = 7;
			}
			else if (Mathf.Abs(vec.x - 1f) < num && Mathf.Abs(vec.y) < num)
			{
				result = 8;
			}
			else
			{
				result = 9;
			}
			return result;
		}
	}
}
#endif