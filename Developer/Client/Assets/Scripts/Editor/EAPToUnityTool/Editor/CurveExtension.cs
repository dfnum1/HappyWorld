#if UNITY_EDITOR
using System;
using UnityEngine;

namespace TopGame.AEPToUnity
{
	public static class CurveExtension
	{
		public static void UpdateAllLinearTangents(this AnimationCurve curve)
		{
			for (int i = 0; i < curve.keys.Length; i++)
			{
				CurveExtension.UpdateTangentsFromMode(curve, i);
			}
		}

		public static void UpdateTangentsFromMode(AnimationCurve curve, int index)
		{
			if (index >= 0 && index < curve.length)
			{
				Keyframe keyframe = curve[index];
				if (KeyframeUtil.GetKeyTangentMode(keyframe, 0) == TangentMode.Linear && index >= 1)
				{
					keyframe.inTangent=(CurveExtension.CalculateLinearTangent(curve, index, index - 1));
					curve.MoveKey(index, keyframe);
				}
				if (KeyframeUtil.GetKeyTangentMode(keyframe, 1) == TangentMode.Linear && index + 1 < curve.length)
				{
					keyframe.outTangent=(CurveExtension.CalculateLinearTangent(curve, index, index + 1));
					curve.MoveKey(index, keyframe);
				}
				if (KeyframeUtil.GetKeyTangentMode(keyframe, 0) == TangentMode.Smooth || KeyframeUtil.GetKeyTangentMode(keyframe, 1) == TangentMode.Smooth)
				{
					curve.SmoothTangents(index, 0f);
				}
			}
		}

		private static float CalculateLinearTangent(AnimationCurve curve, int index, int toIndex)
		{
			return (float)(((double)curve[index].value - (double)curve[toIndex].value) / ((double)curve[index].time - (double)curve[toIndex].time));
		}
	}
}
#endif