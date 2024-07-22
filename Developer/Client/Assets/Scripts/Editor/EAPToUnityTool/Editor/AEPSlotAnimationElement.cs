#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace TopGame.AEPToUnity
{
	public class AEPSlotAnimationElement
	{
		public string name;

		public List<AEPAnimationAttachment> attachment;

		public List<AEPAnimationColor> color;
	}
}
#endif