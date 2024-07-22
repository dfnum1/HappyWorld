#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TopGame.AEPToUnity
{
	public class TexturePacker
	{
		public static bool UpdateAtlasSpriteInfo(string pathOutput, List<DataAnimAnalytics> listAnim, float scale)
		{
			bool result;
			if (listAnim.Count < 1)
			{
				result = false;
			}
			else
			{
				Dictionary<string, EAPInfoAttachment> dictionary = new Dictionary<string, EAPInfoAttachment>();
				for (int i = 0; i < listAnim.Count; i++)
				{
					DataAnimAnalytics dataAnimAnalytics = listAnim[i];
					foreach (KeyValuePair<string, EAPInfoAttachment> current in dataAnimAnalytics.jsonFinal.dicPivot)
					{
						dictionary[current.Value.spriteName] = current.Value;
					}
				}
				Dictionary<string, List<EAPInfoAttachment>> dictionary2 = new Dictionary<string, List<EAPInfoAttachment>>();
				for (int j = 0; j < listAnim.Count; j++)
				{
					DataAnimAnalytics dataAnimAnalytics2 = listAnim[j];
					foreach (KeyValuePair<string, EAPInfoAttachment> current2 in dataAnimAnalytics2.jsonFinal.dicPivot)
					{
						List<EAPInfoAttachment> list = null;
						dictionary2.TryGetValue(current2.Value.spriteName, out list);
						if (list == null)
						{
							list = new List<EAPInfoAttachment>();
						}
						bool flag = false;
						for (int k = 0; k < list.Count; k++)
						{
							if (list[k].spriteName == current2.Key)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							list.Add(current2.Value);
						}
						dictionary2[current2.Value.spriteName] = list;
					}
				}
				TextureImporter textureImporter = AssetImporter.GetAtPath(pathOutput) as TextureImporter;
				TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
				textureImporter.ReadTextureSettings(textureImporterSettings);
				SpriteMetaData[] array = textureImporter.spritesheet;
				Dictionary<string, SpriteMetaData> dictionary3 = new Dictionary<string, SpriteMetaData>();
				bool flag2 = false;
				for (int l = 0; l < array.Length; l++)
				{
					SpriteMetaData value = array[l];
					if (Mathf.Abs(scale - 1f) > Mathf.Epsilon)
					{
						Rect rect = value.rect;
						rect.x = (rect.x * scale);
						rect.y = (rect.y * scale);
						rect.width = (rect.width * scale);
						rect.height = (rect.height * scale);
						value.rect = rect;
						flag2 = true;
					}
					dictionary3[array[l].name] = value;
				}
				foreach (KeyValuePair<string, List<EAPInfoAttachment>> current3 in dictionary2)
				{
					List<EAPInfoAttachment> value2 = current3.Value;
					for (int m = 0; m < value2.Count; m++)
					{
						if (!dictionary3.ContainsKey(value2[m].spriteName))
						{
							if (dictionary3.ContainsKey(value2[m].spriteName))
							{
								SpriteMetaData spriteMetaData = dictionary3[value2[m].spriteName];
								SpriteMetaData value3 = default(SpriteMetaData);
								value3.name = value2[m].spriteName;
								value3.rect = spriteMetaData.rect;
								value3.alignment = spriteMetaData.alignment;
								EAPInfoAttachment eAPInfoAttachment = null;
								dictionary.TryGetValue(value2[m].spriteName, out eAPInfoAttachment);
								if (eAPInfoAttachment == null)
								{
									eAPInfoAttachment = value2[m];
								}
								if (!eAPInfoAttachment.isOptimze)
								{
									value3.pivot = new Vector2(value2[m].x, value2[m].y);
								}
								else
								{
									float num = value2[m].x * (float)eAPInfoAttachment.originalRect.width * scale;
									float num2 = value2[m].y * (float)eAPInfoAttachment.originalRect.height * scale;
									float num3 = (float)eAPInfoAttachment.optimizeRect.width * scale;
									float num4 = (float)eAPInfoAttachment.optimizeRect.height * scale;
									if (num3 < 1f)
									{
										num3 = 1f;
									}
									if (num4 < 1f)
									{
										num4 = 1f;
									}
									num -= (float)eAPInfoAttachment.startX * scale;
									num2 -= (float)eAPInfoAttachment.startY * scale;
									num /= num3;
									num2 /= num4;
									value3.pivot = new Vector2(num, num2);
								}
								dictionary3[value3.name] = value3;
								flag2 = true;
							}
						}
					}
				}
				if (flag2)
				{
					Texture2D dirty = AssetDatabase.LoadAssetAtPath(pathOutput, typeof(Texture2D)) as Texture2D;
					array = new SpriteMetaData[dictionary3.Count];
					int num5 = 0;
					foreach (KeyValuePair<string, SpriteMetaData> current4 in dictionary3)
					{
						array[num5] = current4.Value;
						num5++;
					}
					textureImporter.isReadable = (true);
					textureImporter.mipmapEnabled = (false);
					textureImporter.spritesheet = (array);
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.spriteImportMode = SpriteImportMode.Multiple;
                    textureImporter.spritePixelsPerUnit = (100f);
                    textureImporterSettings.textureFormat = TextureImporterFormat.ARGB32;
                    textureImporterSettings.npotScale = TextureImporterNPOTScale.None;
					textureImporterSettings.alphaIsTransparency = (true);
					textureImporter.SetTextureSettings(textureImporterSettings);
					textureImporter.maxTextureSize = (4096);
					textureImporter.mipmapEnabled = false;
					textureImporter.spriteImportMode = SpriteImportMode.Multiple;
					AssetDatabase.ImportAsset(pathOutput);
					EditorUtility.SetDirty(dirty);
					AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
					AssetDatabase.ImportAsset(pathOutput);
				}
				result = true;
			}
			return result;
		}

		public static bool BuildToEachTexture(List<Texture2D> listTexture, List<DataAnimAnalytics> listJsonAnim, TrimType trimType, string folderOutPut)
		{
			float num = 0f;
			EditorUtility.DisplayCancelableProgressBar("Collecting Textures", "Process...", num);
			bool result;
			try
			{
				Dictionary<string, EAPInfoAttachment> dictionary = new Dictionary<string, EAPInfoAttachment>();
				for (int i = 0; i < listJsonAnim.Count; i++)
				{
					DataAnimAnalytics dataAnimAnalytics = listJsonAnim[i];
					foreach (KeyValuePair<string, EAPInfoAttachment> current in dataAnimAnalytics.jsonFinal.dicPivot)
					{
						dictionary[current.Value.spriteName] = current.Value;
					}
				}
				bool flag = false;
				for (int j = 0; j < listTexture.Count; j++)
				{
					List<SpriteElement> list = new List<SpriteElement>();
					Object @object = listTexture[j];
					if (@object is Texture2D)
					{
						Texture2D texture2D = (Texture2D)@object;
						SpriteElement spriteElement = new SpriteElement(texture2D, true);
						if (trimType == TrimType.Trim2nTexture || trimType == TrimType.TrimMinimum)
						{
							if (!spriteElement.TrimTexture(false))
							{
								spriteElement.CloneFromOriginTexture();
							}
						}
						else
						{
							spriteElement.CloneFromOriginTexture();
						}
						foreach (KeyValuePair<string, EAPInfoAttachment> current2 in dictionary)
						{
							if (current2.Value.spriteName == texture2D.name)
							{
								spriteElement.SetPivot(new Vector2(current2.Value.x, current2.Value.y));
								break;
							}
						}
						string texturePath = folderOutPut + "/" + spriteElement.name + ".png";
						if (trimType == TrimType.Trim2nTexture || trimType == TrimType.TrimMinimum)
						{
							list.Add(spriteElement);
							if (list.Count > 0)
							{
								bool flag2 = TexturePacker.BuildAtlas(trimType, list, dictionary, texturePath, 0, false);
								if (!flag)
								{
									flag = flag2;
								}
								for (int k = 0; k < list.Count; k++)
								{
									Object.DestroyImmediate(list[k].texture);
									list[k] = null;
								}
							}
						}
						else
						{
							TexturePacker.JustSaveNew(spriteElement, texturePath);
							Object.DestroyImmediate(spriteElement.texture);
							if (!flag)
							{
								flag = true;
							}
						}
						num = (float)(j + 1) / (float)listTexture.Count;
						if (EditorUtility.DisplayCancelableProgressBar("Build Textures", "Process...", num))
						{
							result = false;
							return result;
						}
					}
				}
				result = flag;
			}
			catch (Exception ex)
			{
				Debug.LogError("Error:" + ex.Message);
				EditorUtility.ClearProgressBar();
				result = false;
			}
			catch
			{
				EditorUtility.ClearProgressBar();
				result = false;
			}
			return result;
		}

		public static void JustSaveNew(SpriteElement spriteElement, string texturePath)
		{
			byte[] bytes = ImageConversion.EncodeToPNG(spriteElement.texture);
			if (texturePath != "")
			{
				File.WriteAllBytes(texturePath, bytes);
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			}
			AssetDatabase.ImportAsset(texturePath);
			Texture2D dirty = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D)) as Texture2D;
			TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
			TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
			textureImporter.ReadTextureSettings(textureImporterSettings);
			SpriteMetaData[] spritesheet = new SpriteMetaData[]
			{
				new SpriteMetaData
				{
					name = spriteElement.name,
					rect = spriteElement.GetSpriteRect(),
					pivot = spriteElement.pivot,
					alignment = spriteElement.alignment
				}
			};
			textureImporter.isReadable = (true);
			textureImporter.mipmapEnabled = (false);
			textureImporter.spritesheet = (spritesheet);
			textureImporter.textureType = TextureImporterType.Sprite;
			textureImporter.spriteImportMode = SpriteImportMode.Multiple;
			textureImporter.spritePixelsPerUnit = (100f);
			textureImporterSettings.textureFormat = TextureImporterFormat.ARGB32;
			textureImporterSettings.npotScale = TextureImporterNPOTScale.None;
			textureImporterSettings.alphaIsTransparency = (true);
			textureImporter.SetTextureSettings(textureImporterSettings);
			textureImporter.maxTextureSize=(4096);
			textureImporter.mipmapEnabled = (false);
			textureImporter.spriteImportMode = SpriteImportMode.Multiple;
			AssetDatabase.ImportAsset(texturePath);
			EditorUtility.SetDirty(dirty);
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			AssetDatabase.ImportAsset(texturePath);
		}

		public static bool AutoBuildAtlasFromListTexture(List<Texture2D> listTexture, List<DataAnimAnalytics> listJsonAnim, TrimType trimType, string texturePath, int pading)
		{
			float num = 0f;
			EditorUtility.DisplayCancelableProgressBar("Collecting Textures", "Process...", num);
			bool result;
			try
			{
				Dictionary<string, EAPInfoAttachment> dictionary = new Dictionary<string, EAPInfoAttachment>();
				for (int i = 0; i < listJsonAnim.Count; i++)
				{
					DataAnimAnalytics dataAnimAnalytics = listJsonAnim[i];
					foreach (KeyValuePair<string, EAPInfoAttachment> current in dataAnimAnalytics.jsonFinal.dicPivot)
					{
						dictionary[current.Value.spriteName] = current.Value;
					}
				}
				List<SpriteElement> list = new List<SpriteElement>();
				for (int j = 0; j < listTexture.Count; j++)
				{
					Object @object = listTexture[j];
					if (@object is Texture2D)
					{
						Texture2D texture2D = (Texture2D)@object;
						SpriteElement spriteElement = new SpriteElement(texture2D, true);
						if (trimType == TrimType.Trim2nTexture || trimType == TrimType.TrimMinimum)
						{
							if (!spriteElement.TrimTexture(false))
							{
								spriteElement.CloneFromOriginTexture();
							}
						}
						else
						{
							spriteElement.CloneFromOriginTexture();
						}
						foreach (KeyValuePair<string, EAPInfoAttachment> current2 in dictionary)
						{
							if (current2.Value.spriteName == texture2D.name)
							{
								spriteElement.SetPivot(new Vector2(current2.Value.x, current2.Value.y));
								break;
							}
						}
						list.Add(spriteElement);
						num = (float)(j + 1) / (float)listTexture.Count;
						EditorUtility.DisplayCancelableProgressBar("Collecting Textures", "Process...", num);
					}
				}
				if (list.Count > 0)
				{
					bool flag = TexturePacker.BuildAtlas(trimType, list, dictionary, texturePath, pading, false);
					for (int k = 0; k < list.Count; k++)
					{
						Object.DestroyImmediate(list[k].texture);
						list[k] = null;
					}
					result = flag;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Error:" + ex.Message);
				EditorUtility.ClearProgressBar();
				result = false;
			}
			catch
			{
				EditorUtility.ClearProgressBar();
				result = false;
			}
			return result;
		}

		public static bool BuildAtlas(TrimType trimType, List<SpriteElement> listSprite, Dictionary<string, EAPInfoAttachment> dicPivot, string texturePath, int padingSize, bool append = false)
		{
			bool result;
			try
			{
				float num = 0.2f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", num);
				Texture2D[] array = new Texture2D[listSprite.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = listSprite[i].texture;
				}
				Texture2D texture2D = new Texture2D(8192, 8192);
				Rect[] array2 = texture2D.PackTextures(array, padingSize, 8192, false);
				texture2D.Apply();
				int num2 = 0;
				int num3 = 0;
				int width = texture2D.width;
				int height = texture2D.height;
				int num4 = width;
				int num5 = height;
				num = 0.4f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", num);
				if (trimType == TrimType.TrimMinimum && array2.Length > 0)
				{
					float xMin = array2[0].xMin;
					float yMin = array2[0].yMin;
					float xMax = array2[0].xMax;
					float yMax = array2[0].yMax;
					for (int j = 1; j < array2.Length; j++)
					{
						if (array2[j].xMin < xMin)
						{
							xMin = array2[j].xMin;
						}
						if (array2[j].yMin < yMin)
						{
							yMin = array2[j].yMin;
						}
						if (array2[j].xMax < xMax)
						{
							xMax = array2[j].xMax;
						}
						if (array2[j].yMax < yMax)
						{
							yMax = array2[j].yMax;
						}
					}
					int num6 = (int)(xMin * (float)width);
					int num7 = (int)(yMin * (float)height);
					int num8 = (int)(xMax * (float)width);
					int num9 = (int)(yMax * (float)height);
					Color32[] pixels = texture2D.GetPixels32();
					num2 = texture2D.width;
					int num10 = 0;
					num3 = texture2D.height;
					int num11 = 0;
					int width2 = texture2D.width;
					int height2 = texture2D.height;
					int k = 0;
					int num12 = height2;
					while (k < num12)
					{
						int l = 0;
						int num13 = width2;
						while (l < num13)
						{
							Color32 color = pixels[k * num13 + l];
							if (color.a != 0)
							{
								if (k < num3)
								{
									num3 = k;
								}
								if (k > num11 - 1)
								{
									num11 = k + 1;
								}
								if (l < num2)
								{
									num2 = l;
								}
								if (l > num10 - 1)
								{
									num10 = l + 1;
								}
							}
							l++;
						}
						k++;
					}
					if (num2 > num6)
					{
						num2 = num6;
					}
					if (num3 > num7)
					{
						num3 = num7;
					}
					if (num10 < num8)
					{
						num10 = num8;
					}
					if (num11 < num9)
					{
						num11 = num9;
					}
					if (num10 - num2 > 0 && num11 - num3 > 0)
					{
						num4 = num10 - num2;
						num5 = num11 - num3;
						Texture2D texture2D2 = new Texture2D(num10 - num2, num11 - num3);
						texture2D2.SetPixels(texture2D.GetPixels(num2, num3, num10 - num2, num11 - num3));
						texture2D2.Apply();
						Object.DestroyImmediate(texture2D);
						texture2D = texture2D2;
					}
				}
				num = 0.5f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", num);
				byte[] bytes = ImageConversion.EncodeToPNG(texture2D);
				if (texturePath != "")
				{
					File.WriteAllBytes(texturePath, bytes);
					AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
				}
				AssetDatabase.ImportAsset(texturePath);
				EditorUtility.ClearProgressBar();
				num = 0.6f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", num);
				texture2D = (AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D)) as Texture2D);
				TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
				TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
				textureImporter.ReadTextureSettings(textureImporterSettings);
				SpriteMetaData[] array3 = new SpriteMetaData[listSprite.Count];
				if (append)
				{
					if (textureImporter.spritesheet != null && textureImporter.spritesheet.Length > 0)
					{
						append = true;
						array3 = textureImporter.spritesheet;
					}
					else
					{
						append = false;
					}
				}
				for (int m = 0; m < array3.Length; m++)
				{
					if (m < array2.Length)
					{
						SpriteMetaData spriteMetaData = default(SpriteMetaData);
						if (append)
						{
							spriteMetaData = array3[m];
						}
						spriteMetaData.name = listSprite[m].name;
						Rect spriteRect = listSprite[m].GetSpriteRect();
						Rect rect = new Rect(array2[m].x * (float)width - (float)num2, array2[m].y * (float)height - (float)num3, spriteRect.width, spriteRect.height);
						if (rect.x + rect.width > (float)num4)
						{
							rect.width =((float)num4 - rect.x);
						}
						if (rect.y + rect.height > (float)num5)
						{
							rect.height = ((float)num5 - rect.y);
						}
						spriteMetaData.rect = rect;
						int width3 = listSprite[m].originalRect.width;
						int height3 = listSprite[m].originalRect.height;
						if (width3 < 1)
						{
						}
						if (height3 < 1)
						{
						}
						int startX = listSprite[m].startX;
						int startY = listSprite[m].startY;
						if (listSprite[m].IsOptimize())
						{
							float num14 = listSprite[m].pivot.x * (float)listSprite[m].originalRect.width;
							float num15 = listSprite[m].pivot.y * (float)listSprite[m].originalRect.height;
							num14 -= (float)startX;
							num15 -= (float)startY;
							num14 /= (float)listSprite[m].optimizeRect.width;
							num15 /= (float)listSprite[m].optimizeRect.height;
							listSprite[m].SetPivot(new Vector2(num14, num15));
							spriteMetaData.pivot = new Vector2(num14, num15);
							spriteMetaData.alignment = ImportTextureUtil.GetAlignment(spriteMetaData.pivot);
							if (dicPivot != null)
							{
								foreach (KeyValuePair<string, EAPInfoAttachment> current in dicPivot)
								{
									if (current.Value.spriteName == spriteMetaData.name)
									{
										current.Value.SetCache(startX, startY, listSprite[m].originalRect, listSprite[m].optimizeRect);
									}
								}
							}
						}
						else
						{
							spriteMetaData.pivot = listSprite[m].pivot;
							spriteMetaData.alignment = ImportTextureUtil.GetAlignment(spriteMetaData.pivot);
						}
						array3[m] = spriteMetaData;
					}
				}
				num = 0.7f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", num);
				textureImporter.isReadable = (true);
				textureImporter.mipmapEnabled = (false);
				textureImporter.spritesheet = (array3);
				textureImporter.textureType = TextureImporterType.Sprite;
				textureImporter.spriteImportMode = SpriteImportMode.Multiple;
				textureImporter.spritePixelsPerUnit = (100f);
				textureImporterSettings.textureFormat = TextureImporterFormat.ARGB32;
				textureImporterSettings.npotScale = TextureImporterNPOTScale.None;
				textureImporterSettings.alphaIsTransparency = (true);
				textureImporter.SetTextureSettings(textureImporterSettings);
				textureImporter.maxTextureSize=(4096);
				textureImporter.mipmapEnabled = (false);
				textureImporter.spriteImportMode = SpriteImportMode.Multiple;
				AssetDatabase.ImportAsset(texturePath);
				EditorUtility.SetDirty(texture2D);
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
				AssetDatabase.ImportAsset(texturePath);
				num = 1f;
				EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", num);
				EditorUtility.ClearProgressBar();
				GC.Collect();
				result = true;
			}
			catch (UnityException ex)
			{
				Debug.LogError("Error:" + ex.Message);
				EditorUtility.ClearProgressBar();
				result = false;
			}
			catch (Exception ex2)
			{
				Debug.LogError("Error:" + ex2.Message);
				EditorUtility.ClearProgressBar();
				result = false;
			}
			catch
			{
				EditorUtility.ClearProgressBar();
				result = false;
			}
			return result;
		}
	}
}
#endif