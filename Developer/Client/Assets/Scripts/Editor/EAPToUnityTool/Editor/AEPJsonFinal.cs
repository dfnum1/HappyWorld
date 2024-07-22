#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace TopGame.AEPToUnity
{
	public class AEPJsonFinal
	{
		public List<BoneElement> bones;

		public List<SlotElelement> slots;

		public Dictionary<string, BoneElement> dicBones;

		public Dictionary<string, SlotElelement> dicSlots;

		public Dictionary<string, object> skins;

		public Dictionary<string, EAPInfoAttachment> dicPivot = new Dictionary<string, EAPInfoAttachment>();

		public Dictionary<string, AEPBoneAnimationElement> dicAnimation = new Dictionary<string, AEPBoneAnimationElement>();

		public Dictionary<string, AEPSlotAnimationElement> dicSlotAttactment = new Dictionary<string, AEPSlotAnimationElement>();

		public AEPJsonFinal()
		{
		}

		public AEPJsonFinal(RawAEPJson raw)
		{
			this.bones = raw.bones;
			this.dicBones = new Dictionary<string, BoneElement>();
			for (int i = 0; i < this.bones.Count; i++)
			{
				this.dicBones[this.bones[i].name] = this.bones[i];
			}
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			if (raw.animations != null && raw.animations.animation != null)
			{
				foreach (KeyValuePair<string, object> current in raw.animations.animation)
				{
					if (current.Key == "bones")
					{
						Dictionary<string, object> dictionary2 = (Dictionary<string, object>)current.Value;
						foreach (KeyValuePair<string, object> current2 in dictionary2)
						{
							AEPBoneAnimationElement aEPBoneAnimationElement = JsonFx.Json.JsonReader.Deserialize(JsonFx.Json.JsonWriter.Serialize(current2.Value), typeof(AEPBoneAnimationElement)) as AEPBoneAnimationElement;
							aEPBoneAnimationElement.name = current2.Key;
							this.dicAnimation[aEPBoneAnimationElement.name] = aEPBoneAnimationElement;
						}
					}
					else if (current.Key == "slots")
					{
						Dictionary<string, object> dictionary3 = (Dictionary<string, object>)current.Value;
						foreach (KeyValuePair<string, object> current3 in dictionary3)
						{
							AEPSlotAnimationElement aEPSlotAnimationElement = JsonFx.Json.JsonReader.Deserialize(JsonFx.Json.JsonWriter.Serialize(current3.Value), typeof(AEPSlotAnimationElement)) as AEPSlotAnimationElement;
							aEPSlotAnimationElement.name = current3.Key;
							if (aEPSlotAnimationElement.attachment != null)
							{
								for (int j = 0; j < aEPSlotAnimationElement.attachment.Count; j++)
								{
									string text = aEPSlotAnimationElement.attachment[j].name;
									if (!string.IsNullOrEmpty(text))
									{
										int num = text.LastIndexOf("/");
										text = text.Substring(num + 1, text.Length - num - 1);
									}
									aEPSlotAnimationElement.attachment[j].name = text;
								}
								dictionary[aEPSlotAnimationElement.name] = true;
							}
							this.dicSlotAttactment[aEPSlotAnimationElement.name] = aEPSlotAnimationElement;
						}
					}
				}
			}
			this.slots = raw.slots;
			if (this.slots == null)
			{
				this.slots = new List<SlotElelement>();
			}
			this.dicSlots = new Dictionary<string, SlotElelement>();
			for (int k = 0; k < this.slots.Count; k++)
			{
				string text2 = this.slots[k].attachment;
				if (text2 == null)
				{
					if (dictionary.ContainsKey(this.slots[k].name))
					{
						text2 = "";
					}
				}
				if (text2 != null)
				{
					int num2 = text2.LastIndexOf("/");
					text2 = text2.Substring(num2 + 1, text2.Length - num2 - 1);
					this.slots[k].attachment = text2;
					this.slots[k].index = this.slots.Count - k;
					this.dicSlots[this.slots[k].name] = this.slots[k];
					BoneElement boneElement = null;
					this.dicBones.TryGetValue(this.slots[k].bone, out boneElement);
					if (boneElement != null)
					{
						boneElement.index = this.slots[k].index;
					}
				}
			}
			Dictionary<string, object> dictionary4 = raw.skins;
			foreach (KeyValuePair<string, object> current4 in dictionary4)
			{
				Dictionary<string, object> dictionary5 = (Dictionary<string, object>)current4.Value;
				int num3 = 0;
				foreach (KeyValuePair<string, object> current5 in dictionary5)
				{
					string key = current5.Key;
					SlotElelement slotElelement = null;
					this.dicSlots.TryGetValue(key, out slotElelement);
					Dictionary<string, object> dictionary6 = (Dictionary<string, object>)current5.Value;
					foreach (KeyValuePair<string, object> current6 in dictionary6)
					{
						int depth = -num3;
						string text3 = current6.Key;
						int num4 = text3.LastIndexOf("/");
						text3 = text3.Substring(num4 + 1, text3.Length - num4 - 1);
						RawAEPJsonAttachment rawAEPJsonAttachment = JsonFx.Json.JsonReader.Deserialize(JsonFx.Json.JsonWriter.Serialize(current6.Value), typeof(RawAEPJsonAttachment)) as RawAEPJsonAttachment;
						float x = (rawAEPJsonAttachment.width / 2f - rawAEPJsonAttachment.x) / rawAEPJsonAttachment.width;
						float y = (rawAEPJsonAttachment.height / 2f - rawAEPJsonAttachment.y) / rawAEPJsonAttachment.height;
						if (slotElelement != null)
						{
							depth = -slotElelement.index;
						}
						if (string.IsNullOrEmpty(rawAEPJsonAttachment.name))
						{
							rawAEPJsonAttachment.name = text3;
						}
						EAPInfoAttachment eAPInfoAttachment = new EAPInfoAttachment(text3, key, depth, x, y, rawAEPJsonAttachment.x, rawAEPJsonAttachment.y);
						num3++;
						eAPInfoAttachment.spriteName = eAPInfoAttachment.spriteName.Trim();
						this.dicPivot[eAPInfoAttachment.spriteName] = eAPInfoAttachment;
						if (slotElelement != null && !slotElelement.listAcceptAttachment.Contains(eAPInfoAttachment.spriteName))
						{
							slotElelement.listAcceptAttachment.Add(eAPInfoAttachment.spriteName);
							slotElelement.listAcceptObj.Add(eAPInfoAttachment);
						}
					}
				}
			}
		}

		public List<SlotElelement> GetSlotMappingWithBone(string bone)
		{
			List<SlotElelement> list = new List<SlotElelement>();
			foreach (KeyValuePair<string, SlotElelement> current in this.dicSlots)
			{
				if (current.Value.bone == bone)
				{
					list.Add(current.Value);
				}
			}
			return list;
		}

		public BoneElement GetBoneElement(string boneName)
		{
			BoneElement result = null;
			this.dicBones.TryGetValue(boneName, out result);
			return result;
		}

		public string GetFullPathBone(string boneName)
		{
			Dictionary<string, BoneElement> dictionary = this.dicBones;
			string text = "";
			BoneElement boneElement = null;
			if (dictionary.TryGetValue(boneName, out boneElement))
			{
				while (boneElement.parent != null)
				{
					if (text.Length < 1)
					{
						text = boneElement.name;
					}
					else
					{
						text = boneElement.name + "/" + text;
					}
					if (!dictionary.TryGetValue(boneElement.parent, out boneElement))
					{
						return text;
					}
				}
				text += "";
			}
			return text;
		}

		public void JsonFinal()
		{
		}
	}
}
#endif