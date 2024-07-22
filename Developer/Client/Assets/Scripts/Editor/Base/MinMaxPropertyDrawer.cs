using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxAttributeInt))]
public class MinMaxIntPropertyDrawer :PropertyDrawer
{
	public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
	{
		MinMaxAttributeInt attribute = (MinMaxAttributeInt)base.attribute;

		property.intValue = Mathf.Min(Mathf.Max(EditorGUI.IntField(position, label.text, property.intValue), attribute.min), attribute.max);
	}
}

[CustomPropertyDrawer(typeof(MinMaxAttributeFloat))]
public class MinMaxFloatPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
	{
		MinMaxAttributeFloat attribute = (MinMaxAttributeFloat)base.attribute;

		property.floatValue = Mathf.Min(Mathf.Max(EditorGUI.FloatField(position, label.text, property.floatValue), attribute.min), attribute.max);
	}
}
