/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	RootsHandler
作    者:	HappLI
描    述:	根节点挂点
*********************************************************************/

using Framework.Core;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using TopGame.ED;
using System.Collections.Generic;
#endif
namespace Framework.Core
{
    public class RootsHandler : ARootsHandler
    {
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(RootsHandler), true)]
    public class RootsHandlerEditor : Editor
    {
        bool m_bEpxandActorTypeRoots = false;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            RootsHandler rootHandler = target as RootsHandler;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("actorsRoot"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("particlesRoot"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scenesRoot"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("themeRoot"));
            SerializedProperty actorTypeRoots = serializedObject.FindProperty("actorTypeRoots");
            if (actorTypeRoots != null && actorTypeRoots.isArray)
            {
                if(actorTypeRoots.arraySize == 0)
                {
                    actorTypeRoots.arraySize = (int)EActorType.Count;
                }
                else
                {
                    while (actorTypeRoots.arraySize > (int)EActorType.Count)
                    {
                        actorTypeRoots.DeleteArrayElementAtIndex(actorTypeRoots.arraySize - 1);
                    }
                    for (int i = actorTypeRoots.arraySize; i < (int)EActorType.Count; ++i)
                    {
                        actorTypeRoots.InsertArrayElementAtIndex(actorTypeRoots.arraySize);
                    }
                }
                m_bEpxandActorTypeRoots = EditorGUILayout.Foldout(m_bEpxandActorTypeRoots, "ActorTypeRoots");
                if(m_bEpxandActorTypeRoots)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < (int)EActorType.Count; ++i)
                    {
                        EditorGUILayout.PropertyField(actorTypeRoots.GetArrayElementAtIndex(i), new GUIContent(((EActorType)i).ToString()));
                    }
                    EditorGUI.indentLevel--;
                }
            }
            serializedObject.ApplyModifiedProperties();
            if (GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(rootHandler);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
#endif
}

