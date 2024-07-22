#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace TopGame.AEPToUnity
{
	public class EditorUtil
	{
		public class AnimationClipSettings
		{
			private SerializedProperty m_Property;

			public float startTime
			{
				get
				{
					return this.Get("m_StartTime").floatValue;
				}
				set
				{
					this.Get("m_StartTime").floatValue=(value);
				}
			}

			public float stopTime
			{
				get
				{
					return this.Get("m_StopTime").floatValue;
				}
				set
				{
					this.Get("m_StopTime").floatValue=(value);
				}
			}

			public float orientationOffsetY
			{
				get
				{
					return this.Get("m_OrientationOffsetY").floatValue;
				}
				set
				{
					this.Get("m_OrientationOffsetY").floatValue=(value);
				}
			}

			public float level
			{
				get
				{
					return this.Get("m_Level").floatValue;
				}
				set
				{
					this.Get("m_Level").floatValue=(value);
				}
			}

			public float cycleOffset
			{
				get
				{
					return this.Get("m_CycleOffset").floatValue;
				}
				set
				{
					this.Get("m_CycleOffset").floatValue=(value);
				}
			}

			public bool loopTime
			{
				get
				{
					return this.Get("m_LoopTime").boolValue;
				}
				set
				{
					this.Get("m_LoopTime").boolValue=(value);
				}
			}

			public bool loopBlend
			{
				get
				{
					return this.Get("m_LoopBlend").boolValue;
				}
				set
				{
					this.Get("m_LoopBlend").boolValue=(value);
				}
			}

			public bool loopBlendOrientation
			{
				get
				{
					return this.Get("m_LoopBlendOrientation").boolValue;
				}
				set
				{
					this.Get("m_LoopBlendOrientation").boolValue=(value);
				}
			}

			public bool loopBlendPositionY
			{
				get
				{
					return this.Get("m_LoopBlendPositionY").boolValue;
				}
				set
				{
					this.Get("m_LoopBlendPositionY").boolValue=(value);
				}
			}

			public bool loopBlendPositionXZ
			{
				get
				{
					return this.Get("m_LoopBlendPositionXZ").boolValue;
				}
				set
				{
					this.Get("m_LoopBlendPositionXZ").boolValue=(value);
				}
			}

			public bool keepOriginalOrientation
			{
				get
				{
					return this.Get("m_KeepOriginalOrientation").boolValue;
				}
				set
				{
					this.Get("m_KeepOriginalOrientation").boolValue=(value);
				}
			}

			public bool keepOriginalPositionY
			{
				get
				{
					return this.Get("m_KeepOriginalPositionY").boolValue;
				}
				set
				{
					this.Get("m_KeepOriginalPositionY").boolValue=(value);
				}
			}

			public bool keepOriginalPositionXZ
			{
				get
				{
					return this.Get("m_KeepOriginalPositionXZ").boolValue;
				}
				set
				{
					this.Get("m_KeepOriginalPositionXZ").boolValue=(value);
				}
			}

			public bool heightFromFeet
			{
				get
				{
					return this.Get("m_HeightFromFeet").boolValue;
				}
				set
				{
					this.Get("m_HeightFromFeet").boolValue=(value);
				}
			}

			public bool mirror
			{
				get
				{
					return this.Get("m_Mirror").boolValue;
				}
				set
				{
					this.Get("m_Mirror").boolValue=(value);
				}
			}

			public AnimationClipSettings(SerializedProperty prop)
			{
				this.m_Property = prop;
			}

			private SerializedProperty Get(string property)
			{
				return this.m_Property.FindPropertyRelative(property);
			}
		}

		public static Color HexToColor(string hex)
		{
			Color result;
			try
			{
				byte b = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
				byte b2 = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
				byte b3 = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
				byte b4 = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
				result = new Color((float)b / 255f, (float)b2 / 255f, (float)b3 / 255f, (float)b4 / 255f);
			}
			catch (Exception var_5_7A)
			{
				result = new Color(1f, 1f, 1f, 1f);
			}
			return result;
		}

		public static List<string> LoadStringByLine(string text)
		{
			List<string> list = new List<string>();
			string[] collection = text.Split(new string[]
			{
				"\n"
			}, StringSplitOptions.RemoveEmptyEntries);
			list.AddRange(collection);
			return list;
		}

		public static List<string> Load(string fileName)
		{
			List<string> list = new List<string>();
			try
			{
				StreamReader streamReader = new StreamReader(fileName, Encoding.Default);
				using (streamReader)
				{
					string text;
					do
					{
						text = streamReader.ReadLine();
						if (text != null)
						{
							list.Add(text);
						}
					}
					while (text != null);
					streamReader.Close();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("{0}\n", ex.Message);
			}
			return list;
		}

		public static string LoadAllFileToString(string fileName)
		{
			string result = "";
			try
			{
				StreamReader streamReader = new StreamReader(fileName, Encoding.Default);
				using (streamReader)
				{
					result = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("{0}\n", ex.Message);
			}
			return result;
		}

		public static string GetFullPathBone(Transform boneRoot, Transform current)
		{
			string text = current.name;
			Transform transform = current;
			while (!(transform.parent == null) && !(transform.parent == boneRoot))
			{
				text = transform.parent.name + "/" + text;
				transform = transform.parent;
			}
			return text;
		}

		public static string AnimatorInfoJson()
		{
			return "{\"rootPosition\":{\"x\":0,\"y\":0,\"z\":0},\"rootRotation\":{\"eulerAngles\":{\"x\":0,\"y\":0,\"z\":0},\"x\":0,\"y\":0,\"z\":0,\"w\":1},\"applyRootMotion\":false,\"linearVelocityBlending\":false,\"animatePhysics\":false,\"updateMode\":\"Normal\",\"bodyPosition\":{\"x\":0,\"y\":0,\"z\":0},\"bodyRotation\":{\"eulerAngles\":{\"x\":270,\"y\":null,\"z\":0},\"x\":0,\"y\":0,\"z\":null,\"w\":9.183409E-41},\"stabilizeFeet\":false,\"feetPivotActive\":1,\"speed\":1,\"cullingMode\":\"AlwaysAnimate\",\"playbackTime\":-1,\"recorderStartTime\":-1,\"recorderStopTime\":-1,\"runtimeAnimatorController\":{\"layers\":[{\"name\":\"Base Layer\",\"stateMachine\":{\"states\":[],\"stateMachines\":[],\"defaultState\":null,\"anyStatePosition\":{\"x\":50,\"y\":20,\"z\":0},\"entryPosition\":{\"x\":50,\"y\":120,\"z\":0},\"exitPosition\":{\"x\":800,\"y\":120,\"z\":0},\"parentStateMachinePosition\":{\"x\":800,\"y\":20,\"z\":0},\"anyStateTransitions\":[],\"entryTransitions\":[],\"behaviours\":[],\"name\":\"Base Layer\",\"hideFlags\":\"HideInHierarchy\"},\"avatarMask\":null,\"blendingMode\":\"Override\",\"syncedLayerIndex\":-1,\"iKPass\":false,\"defaultWeight\":0,\"syncedLayerAffectsTiming\":false}],\"parameters\":[],\"name\":\"AEP Animation\",\"hideFlags\":\"None\"},\"avatar\":null,\"layersAffectMassCenter\":false,\"logWarnings\":true,\"fireEvents\":true,\"enabled\":true,\"tag\":\"Untagged\",\"name\":\"AEP Animation\",\"hideFlags\":\"None\"}";
		}
	}
}
#endif