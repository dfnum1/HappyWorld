#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace TopGame.AEPToUnity
{
	public class SlotElelement
	{
		public string name;

		public string bone;

		public string color = "ffffffff";

		public string attachment = "";

		public List<string> listAcceptAttachment = new List<string>();

		public List<EAPInfoAttachment> listAcceptObj = new List<EAPInfoAttachment>();

		public int index = 0;
	}
}
#endif