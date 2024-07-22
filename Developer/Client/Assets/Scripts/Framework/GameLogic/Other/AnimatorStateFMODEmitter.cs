/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AnimatorStateFMODEmitter
作    者:	HappLI
描    述:	动画动作fmod 发射脚本
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Logic
{
    public class AnimatorStateFMODEmitter : StateMachineBehaviour

    {
        public bool AllowFadeout = true;
        public bool TriggerOnce = false;

        public FMODUnity.EventReference fmodEvent;
        [System.NonSerialized]
        private Framework.Core.ISound m_pSound;
        [System.NonSerialized]
        private bool m_bTriggerd = false;
        //------------------------------------------------------
        private void OnDisable()
        {
            m_bTriggerd = false;
            StopInstance();
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            m_bTriggerd = false;
            StopInstance();
        }
        //------------------------------------------------------
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (TriggerOnce && m_bTriggerd) return;
            if (fmodEvent.IsNull) return;
#if USE_FMOD
            m_pSound = TopGame.Core.AudioManager.PlayEvent(fmodEvent);
#endif
            if (TriggerOnce) m_bTriggerd = true;
        }
        //------------------------------------------------------
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StopInstance();
        }
        //------------------------------------------------------
        void StopInstance()
        {
            if (m_pSound!=null)
            {
                if (AllowFadeout)
                    m_pSound.Stop();
                else
                    m_pSound.Destroy();
                m_pSound = null;
            }
        }
    }
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AnimatorStateFMODEmitter), true)]
    public class AnimatorStateFMODEmitterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            AnimatorStateFMODEmitter controller = target as AnimatorStateFMODEmitter;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AllowFadeout"), new GUIContent("允许渐变退出"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TriggerOnce"), new GUIContent("只允许播放一次"));
            SerializedProperty eventReference = serializedObject.FindProperty("fmodEvent");
            if (eventReference != null)
                EditorGUILayout.PropertyField(eventReference, new GUIContent("FMOD音效"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
