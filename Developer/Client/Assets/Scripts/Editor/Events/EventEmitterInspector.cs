#if UNITY_EDITOR
using Framework.Core;
using Framework.ED;
using System;
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Logic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace TopGame.Timeline
{
    [CustomEditor(typeof(EventEmitter), true)]
    [CanEditMultipleObjects]
    public class EventEmitterInspector : Editor
    {
        EEventType m_AddType = EEventType.Base;
        private void OnEnable()
        {
            EventEmitter emitter = target as EventEmitter;
            Transform pBind = null;
            if (emitter.parent != null)
            {
                if(emitter.parent is IUserTrackAsset)
                {
                    IUserTrackAsset asset = emitter.parent as IUserTrackAsset;
                    if (asset.GetBinder())
                    {
                        pBind = asset.GetBinder();
                    }
                }
                else
                {
                    if(UnityEditor.Timeline.TimelineEditor.masterDirector != null)
                    {
                        UnityEngine.Object pObject = UnityEditor.Timeline.TimelineEditor.masterDirector.GetGenericBinding(emitter.parent);
                        if(pObject)
                        {
                            if(pObject is MonoBehaviour)
                            {
                                pBind = (pObject as MonoBehaviour).transform;
                            }
                            else if (pObject is GameObject)
                            {
                                pBind = (pObject as GameObject).transform;
                            }
                        }
                    }
                }

                if(pBind)
                {
                    EventEmitterReciver reciver = pBind.GetComponent<EventEmitterReciver>();
                    if (reciver == null) reciver = pBind.gameObject.AddComponent<EventEmitterReciver>();
                }
            }
        }
        public override void OnInspectorGUI()
        {
            EventEmitter emitter = target as EventEmitter;
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            emitter.time = EditorGUILayout.DoubleField("Time",emitter.time);
            EditorGUILayout.EndHorizontal();
            List<BaseEventParameter>  vEvents = emitter.GetEvents();
            GUILayout.BeginHorizontal();
            m_AddType = EventPopDatas.DrawEventPop(m_AddType, "添加事件");
            if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(80) }))
            {
                BaseEventParameter pEvent = BuildEventUtl.BuildEventByType(null, m_AddType);
                if (pEvent != null)
                    vEvents.Add(pEvent);
            }
            GUILayout.EndHorizontal();

            Color color = GUI.color;
            for (int i = 0; i < vEvents.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                GUI.color = Color.green;
                GUILayout.Box(vEvents[i].GetEventType().ToString());
                GUI.color = color;
                if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    vEvents.RemoveAt(i);
                    break;
                }
                if (GUILayout.Button("复制"))
                {
                    DrawEventCore.AddCopyEvent(vEvents[i]);
                }
                if (DrawEventCore.CanCopyEvent(vEvents[i])  &&  GUILayout.Button("黏贴"))
                {
                    DrawEventCore.CopyEvent(vEvents[i]);
                }
                GUILayout.EndHorizontal();
                vEvents[i] = DrawEventCore.DrawUnAction(vEvents[i]);
            }

            emitter.SyncEvents();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif