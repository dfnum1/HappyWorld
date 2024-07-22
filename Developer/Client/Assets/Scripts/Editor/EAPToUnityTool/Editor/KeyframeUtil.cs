#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEngine;

namespace TopGame.AEPToUnity
{
	public class KeyframeUtil
	{
		public static TangentMode GetTangleMode(string str)
		{
			TangentMode result;
			if (str.ToLower().Contains("stepped"))
			{
				result = TangentMode.Stepped;
			}
			else if (str.ToLower().Contains("editable"))
			{
				result = TangentMode.Editable;
			}
			else if (str.ToLower().Contains("Smooth"))
			{
				result = TangentMode.Smooth;
			}
			else
			{
				result = TangentMode.Linear;
			}
			return result;
		}

		public static Keyframe GetNew(float time, float value, TangentMode leftAndRight)
		{
			return KeyframeUtil.GetNew(time, value, leftAndRight, leftAndRight);
		}

		public static Keyframe GetNew(float time, float value, TangentMode left, TangentMode right)
		{
			object obj = new Keyframe(time, value);
			KeyframeUtil.SetKeyBroken(obj, true);
			KeyframeUtil.SetKeyTangentMode(obj, 0, left);
			KeyframeUtil.SetKeyTangentMode(obj, 1, right);
			Keyframe result = (Keyframe)obj;
			if (left == TangentMode.Stepped)
			{
				result.inTangent=(float.PositiveInfinity);
			}
			if (right == TangentMode.Stepped)
			{
				result.outTangent=(float.PositiveInfinity);
			}
			return result;
		}

		public static void SetKeyTangentMode(object keyframe, int leftRight, TangentMode mode)
		{
			Type typeFromHandle = typeof(Keyframe);
			FieldInfo field = typeFromHandle.GetField("m_TangentMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			int num = (int)field.GetValue(keyframe);
			if (leftRight == 0)
			{
				num &= -7;
				num |= (int)((int)mode << 1);
			}
			else
			{
				num &= -25;
				num |= (int)((int)mode << 3);
			}
			field.SetValue(keyframe, num);
			if (KeyframeUtil.GetKeyTangentMode(num, leftRight) != mode)
			{
				Debug.Log("bug");
			}
		}

		public static TangentMode GetKeyTangentMode(int tangentMode, int leftRight)
		{
			TangentMode result;
			if (leftRight == 0)
			{
				result = (TangentMode)((tangentMode & 6) >> 1);
			}
			else
			{
				result = (TangentMode)((tangentMode & 24) >> 3);
			}
			return result;
		}

		public static TangentMode GetKeyTangentMode(Keyframe keyframe, int leftRight)
		{
			Type typeFromHandle = typeof(Keyframe);
			FieldInfo field = typeFromHandle.GetField("m_TangentMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			int num = (int)field.GetValue(keyframe);
			TangentMode result;
			if (leftRight == 0)
			{
				result = (TangentMode)((num & 6) >> 1);
			}
			else
			{
				result = (TangentMode)((num & 24) >> 3);
			}
			return result;
		}

		public static void SetKeyBroken(object keyframe, bool broken)
		{
			Type typeFromHandle = typeof(Keyframe);
			FieldInfo field = typeFromHandle.GetField("m_TangentMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			int num = (int)field.GetValue(keyframe);
			if (broken)
			{
				num |= 1;
			}
			else
			{
				num &= -2;
			}
			field.SetValue(keyframe, num);
		}
	}
}
#endif