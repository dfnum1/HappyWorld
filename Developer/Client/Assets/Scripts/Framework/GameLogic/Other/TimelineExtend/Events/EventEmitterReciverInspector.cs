#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace TopGame.Timeline
{
    [CustomEditor(typeof(EventEmitterReciver))]
    class EventEmitterReciverEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("事件接收器");
            
            if (EditorGUI.EndChangeCheck())
            {

            }
        }
    }
}
#endif