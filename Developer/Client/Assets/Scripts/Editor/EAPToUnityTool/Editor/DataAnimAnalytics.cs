#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace TopGame.AEPToUnity
{
	public class DataAnimAnalytics
	{
		public Dictionary<string, string> objShowWhenStartAnim = new Dictionary<string, string>();

		public Dictionary<string, string> objHideWhenStartAnim = new Dictionary<string, string>();

		public AEPJsonFinal jsonFinal;

		public string filename;

		public AnimationStyle animationStyle;

		public DataAnimAnalytics(AEPJsonFinal _jsonFinal, string _filename, AnimationStyle _animationStyle)
		{
			this.jsonFinal = _jsonFinal;
			this.filename = _filename;
			this.animationStyle = _animationStyle;
		}

		public void AddObjectShowWhenStartAnim(Dictionary<string, string> _objShowWhenStartAnim)
		{
			foreach (KeyValuePair<string, string> current in _objShowWhenStartAnim)
			{
				this.objShowWhenStartAnim[current.Key] = current.Value;
			}
		}

		public void AddObjectHideWhenStartAnim(Dictionary<string, string> _objHideWhenStartAnim)
		{
			foreach (KeyValuePair<string, string> current in _objHideWhenStartAnim)
			{
				this.objHideWhenStartAnim[current.Key] = current.Value;
			}
		}
	}
}
#endif