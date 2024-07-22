#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace TopGame.AEPToUnity
{
	public class AEPBoneAnimationElement
	{
		public string name;

		public List<AEPAnimationTranslate> translate;

		public List<AEPAnimationScale> scale;

		public List<AEPAnimationRotate> rotate;
	}
}
#endif