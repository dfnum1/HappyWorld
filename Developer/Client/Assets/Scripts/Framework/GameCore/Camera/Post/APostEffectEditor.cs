#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace TopGame.Post
{
    public abstract class APostEditor : Editor
    {
        //------------------------------------------------------
        public static void DrawFixMeBox(string text, Action action)
        {
            Assert.IsNotNull(action);

            EditorGUILayout.HelpBox(text, MessageType.Warning);

            GUILayout.Space(-32);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Fix", GUILayout.Width(60)))
                    action();

                GUILayout.Space(8);
            }
            GUILayout.Space(11);
        }
    }
}
#endif