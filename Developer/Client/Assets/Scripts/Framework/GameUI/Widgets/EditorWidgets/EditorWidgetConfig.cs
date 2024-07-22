
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.UI
{
    [CreateAssetMenu]
    public class EditorWidgetConfig : ScriptableObject
    {
        public UISerialized[] Widgets = new UISerialized[(int)EditorWidgetType.Count];
    }
#if UNITY_EDITOR
    //------------------------------------------------------
    [CustomEditor(typeof(EditorWidgetConfig), true)]
    [CanEditMultipleObjects]
    public class EditorWidgetConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorWidgetConfig assets = target as EditorWidgetConfig;
            if(assets.Widgets == null)
                assets.Widgets = new UISerialized[(int)EditorWidgetType.Count];
            else if(assets.Widgets.Length != (int)EditorWidgetType.Count)
            {
                List<UISerialized> list = new List<UISerialized>(assets.Widgets);
                if(list.Count > (int)EditorWidgetType.Count)
                {
                    list.RemoveRange((int)EditorWidgetType.Count, list.Count - (int)EditorWidgetType.Count);
                }
                else
                {
                    for (int i = list.Count; i < (int)EditorWidgetType.Count; ++i)
                        list.Add(null);
                }
                assets.Widgets = list.ToArray();
            }
            for (int i =0; i < assets.Widgets.Length; ++i)
            {
                assets.Widgets[i] = EditorGUILayout.ObjectField(((EditorWidgetType)i).ToString(), assets.Widgets[i], typeof(UISerialized), false) as UISerialized;
            }

            if (GUILayout.Button("Ë¢ÐÂ±£´æ"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
#endif
}