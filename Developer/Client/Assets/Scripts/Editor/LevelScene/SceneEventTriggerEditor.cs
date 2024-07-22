/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	Scene
作    者:	HappLI
描    述:	镜像检测
*********************************************************************/
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Framework.Core;
using Framework.ED;
#if UNITY_EDITOR
using UnityEditor;
using TopGame.ED;
#endif
namespace TopGame.Core
{
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SceneEventTrigger), true)]
    public class SceneEventTriggerEditor : Editor
    {
        Material m_pDrawMat;
        Mesh m_pMesh = null;
        int m_nSelectEvent = -1;

        EEventType m_AddEventType;

        List<string> m_vEventPop = new List<string>();
        private void OnEnable()
        {
            ED.EditorKits.CheckEventCopy();
            m_nSelectEvent = -1;
            m_pMesh = new Mesh();
            m_pMesh.hideFlags = HideFlags.DontSave;
            m_pMesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0) };
            m_pMesh.triangles = new int[]{ 0, 1, 2, 0, 2, 3};
            m_pMesh.uv = new Vector2[] { new Vector2(0,0), new Vector2(0,1), new Vector2(1,1), new Vector2(1,0) };
            m_pDrawMat = new Material(Shader.Find("SD/Particles/SD_Alpha_Blended"));
            m_pDrawMat.hideFlags = HideFlags.DontSave;
            m_pDrawMat.SetTexture("_MainTex", Resources.Load("event_trigger") as Texture2D);

            RefreshEventPop();
        }
        //------------------------------------------------------
        void RefreshEventPop()
        {
            SceneEventTrigger levelScene = target as SceneEventTrigger;
            m_vEventPop.Clear();
            if (levelScene.events!=null)
            {
                for(int i = 0; i < levelScene.events.Length; ++i)
                {
                    m_vEventPop.Add(levelScene.events[i].name + "[" + i + "]");
                }
            }
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            GameObject.DestroyImmediate(m_pMesh);
            GameObject.DestroyImmediate(m_pDrawMat);
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
        //    base.OnInspectorGUI();
            SceneEventTrigger levelScene = target as SceneEventTrigger;
            GUILayout.BeginHorizontal();
            int preIndex = m_nSelectEvent;
            m_nSelectEvent = EditorGUILayout.Popup("事件", m_nSelectEvent, m_vEventPop.ToArray());
            if(GUILayout.Button("添加"))
            {
                List<SceneEventTrigger.EventTrigger> vTemp;
                if (levelScene.events != null) vTemp = new List<SceneEventTrigger.EventTrigger>(levelScene.events);
                else vTemp = new List<SceneEventTrigger.EventTrigger>();

                SceneEventTrigger.EventTrigger tri = new SceneEventTrigger.EventTrigger();
                if (Framework.Module.ModuleManager.mainModule != null)
                {
                    AFrameworkModule tempFramework = Framework.Module.ModuleManager.mainModule as AFrameworkModule;
                    if(tempFramework!=null) tri.position = tempFramework.GetPlayerPosition() - levelScene.transform.position;
                }
                else
                    tri.position = new Vector3(0, 2, 50);
                tri.triggetCnt = 0;
                tri.aabb_min = new Vector3(-12, -2, -5);
                tri.aabb_max = new Vector3(12, 20, 5);
                vTemp.Add(tri);
                levelScene.events = vTemp.ToArray();
                m_nSelectEvent = levelScene.events.Length - 1;
            }
            if (levelScene.events!=null && m_nSelectEvent >=0 && m_nSelectEvent< levelScene.events.Length && GUILayout.Button("删除"))
            {
                List<SceneEventTrigger.EventTrigger> vTemp = new List<SceneEventTrigger.EventTrigger>(levelScene.events);
                vTemp.RemoveAt(m_nSelectEvent);
                levelScene.events = vTemp.ToArray();
                m_nSelectEvent = -1;
            }
            GUILayout.EndHorizontal();

            if (m_nSelectEvent>=0 && m_nSelectEvent< levelScene.events.Length)
            {
                SceneEventTrigger.EventTrigger evt = levelScene.events[m_nSelectEvent];
                if (m_nSelectEvent != preIndex)
                {
                    if (SceneView.lastActiveSceneView != null)
                    {
                        EditorKits.SceneViewLookat(evt.position);
                    }
                }
                if (evt.events == null || evt.events.Count <= 0) evt.buildEvents = null;
                else
                {
                    if (evt.buildEvents == null || evt.events.Count != evt.buildEvents.Count)
                    {
                        if (evt.buildEvents == null) evt.buildEvents = new List<BaseEventParameter>();
                        for (int j = 0; j < evt.events.Count; ++j)
                        {
                            BaseEventParameter param = BuildEventUtl.BuildEvent(null, evt.events[j]);
                            if(param!=null) evt.buildEvents.Add(param);
                        }
                    }
                }

                if(evt.buildEvents!=null)
                {
                    EditorGUI.BeginChangeCheck();
                    evt.name = EditorGUILayout.TextField("描述", evt.name);
                    evt.triggetCnt = (byte)EditorGUILayout.IntField("触发次数(<=0无限制)", evt.triggetCnt);
                    evt.position = EditorGUILayout.Vector3Field("Position", evt.position);
                    Vector3 aabb_min = EditorGUILayout.Vector3Field("AABB Min", evt.aabb_min);
                    Vector3 aabb_max = EditorGUILayout.Vector3Field("AABB Max", evt.aabb_max);
                    evt.aabb_min = Vector3.Min(aabb_min, aabb_max);
                    evt.aabb_max = Vector3.Max(aabb_min, aabb_max);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (SceneView.lastActiveSceneView != null)
                        {
                            SceneView.lastActiveSceneView.Repaint();
                        }
                    }
                    EditorGUI.BeginChangeCheck();
                    evt.name = EditorGUILayout.TextField("描述", evt.name);
                    if (EditorGUI.EndChangeCheck())
                    {
                        RefreshEventPop();
                    }
                    Color colro = GUI.color;
                    for (int i = 0; i < evt.buildEvents.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        GUI.color = Color.green;
                        GUILayout.Label("Event[" + i + "]" + evt.buildEvents[i].GetEventType());
                        GUI.color = colro;
                        string tag =string.Format("event#[{0}]", evt.buildEvents[i].GetEventType());
                        if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(40) }))
                        {
                            evt.buildEvents.RemoveAt(i);
                            evt.events.RemoveAt(i);
                            return;
                        }
                        if (GUILayout.Button("复制", new GUILayoutOption[] { GUILayout.Width(40) }))
                        {
                            DrawEventCore.AddCopyEvent(evt.buildEvents[i]);
                        }
                        if (DrawEventCore.CanCopyEvent(evt.buildEvents[i]) &&
                            GUILayout.Button("黏贴", new GUILayoutOption[] { GUILayout.Width(40) }))
                        {
                            DrawEventCore.CopyEvent(evt.buildEvents[i]);
                        }
                        GUILayout.EndHorizontal();


                        GUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        GUILayout.BeginVertical();
                        float preTime = evt.buildEvents[i].triggetTime;
                        evt.buildEvents[i] = DrawEventCore.DrawUnAction(evt.buildEvents[i], null);
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        evt.events[i] = evt.buildEvents[i].WriteCmd();
                    }
                }

                GUILayout.BeginHorizontal();
                m_AddEventType = EventPopDatas.DrawEventPop(m_AddEventType, "");
                EditorGUI.BeginDisabledGroup(m_AddEventType == EEventType.Base || m_AddEventType == EEventType.Count);
                if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    BaseEventParameter parame = BuildEventUtl.BuildEventByType(null, m_AddEventType);
                    if(parame!=null)
                    {
                        if (evt.buildEvents == null) evt.buildEvents = new List<BaseEventParameter>();
                        evt.buildEvents.Add(parame);
                        if (evt.events == null) evt.events = new List<string>();
                        evt.events.Add(null);
                        m_AddEventType = EEventType.Count;
                    }

                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();

                levelScene.events[m_nSelectEvent] = evt;
            }
            if (GUILayout.Button("保存"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
        //------------------------------------------------------
        private void OnSceneGUI()
        {
            SceneEventTrigger levelScene = target as SceneEventTrigger;
            SceneEventTrigger.EventTrigger evt;
            for(int i =0; i < levelScene.events.Length; ++i)
            {
                evt = levelScene.events[i];
                Quaternion rotation = Quaternion.Euler(evt.eulerAngle+ levelScene.transform.eulerAngles);
                if (Tools.current == Tool.Move)
                    evt.position = Handles.DoPositionHandle(evt.position + levelScene.transform.position, rotation) - levelScene.transform.position;
                else if (Tools.current == Tool.Rotate)
                {
                    rotation = Quaternion.Euler(evt.eulerAngle);
                    evt.eulerAngle = Handles.DoRotationHandle(rotation, evt.position + levelScene.transform.position).eulerAngles;
                }
                Vector3 center = (evt.aabb_min + evt.aabb_max) / 2;
                Vector3 half = evt.aabb_max - center;

                float fSize = Mathf.Max(1, HandleUtility.GetHandleSize(evt.position + levelScene.transform.position));
                
                Framework.Core.CommonUtility.DrawBoundingBox(center, half, Matrix4x4.TRS(evt.position + levelScene.transform.position, Quaternion.Euler(evt.eulerAngle + levelScene.transform.eulerAngles), levelScene.transform.localScale), Color.magenta, false);

                Camera camera=null;
                Quaternion billboardRot = rotation;
                if (SceneView.lastActiveSceneView != null)
                {
                    billboardRot = SceneView.lastActiveSceneView.camera.transform.rotation;
                    camera = SceneView.lastActiveSceneView.camera;
                }
                else if (SceneView.currentDrawingSceneView != null)
                {
                    billboardRot = SceneView.currentDrawingSceneView.camera.transform.rotation;
                    camera = SceneView.currentDrawingSceneView.camera;
                }

                if (camera!=null)
                    Graphics.DrawMesh(m_pMesh, Matrix4x4.TRS(evt.position + levelScene.transform.position, billboardRot, Vector3.one* fSize) , m_pDrawMat, 0, camera);

                levelScene.events[i] = evt;
            }
        }
    }
#endif
}
