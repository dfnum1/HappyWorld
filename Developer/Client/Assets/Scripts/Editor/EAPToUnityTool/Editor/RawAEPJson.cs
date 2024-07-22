#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace TopGame.AEPToUnity
{
	public class RawAEPJson
	{
		public List<BoneElement> bones;

		public List<SlotElelement> slots;

		public Dictionary<string, object> skins;

		public AEPAnimationRaw animations;
	}
}
#endif