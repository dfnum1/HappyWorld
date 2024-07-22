/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ActionGraphBinder
作    者:	HappLI
描    述:	动作脚本数据绑定
*********************************************************************/
using UnityEngine;
using Framework.Core;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif
namespace TopGame.Core
{
    public class ActionGraphBinder : AActionGraphBinder
    {
        public BodyPart[] bodyParts;
        public CameraSlots cameraSlot;

        public override int GetBodyPartsCount()
        {
            return bodyParts != null ? bodyParts.Length : 0;
        }
        public override IBaseBodyPart GetBodyParts(int index)
        {
            if (bodyParts == null || index < 0 || index >= bodyParts.Length) return null;
            return bodyParts[index];
        }
        public override ACameraSlots GetCameraSlot()
        {
            return cameraSlot;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ActionGraphBinder))]
    [CanEditMultipleObjects]
    public class ActionGraphBinderEditor : AActionGraphBinderEditor
    {
        protected override void OnInnerInspectorGUI()
        {
            //             EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraSlot"), true);
        }
    }
#endif
}

