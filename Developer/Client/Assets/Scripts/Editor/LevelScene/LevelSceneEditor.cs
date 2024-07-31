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
using Framework.Base;
#if UNITY_EDITOR
using UnityEditor;
using TopGame.ED;
#endif
namespace TopGame.Core
{
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LevelScene), true)]
    public class LevelSceneEditor : Editor
    {
        Material m_pDrawMat;
        Mesh m_pMesh = null;
        int m_nSelectEvent = -1;

        EEventType m_AddEventType;

        bool m_bColliderTest = false;
        List<Collider> m_vMeshCollider = new List<Collider>();

        bool m_bExpandTriggerBase = false;
        bool m_bExpandTriggerEvent = false;
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

            LevelScene levelScene = target as LevelScene;
            if(levelScene.worldTriggers!=null)
            {
                for(int i =0; i < levelScene.worldTriggers.Length; ++i)
                {
                    WorldTriggerParamter worldTrigger = levelScene.worldTriggers[i];
                    worldTrigger.Create(null);
                    levelScene.worldTriggers[i] = worldTrigger;
                }
            }
        }
        //------------------------------------------------------
        void RefreshEventPop()
        {
            LevelScene levelScene = target as LevelScene;
            m_vEventPop.Clear();
            if (levelScene.worldTriggers != null)
            {
                for(int i = 0; i < levelScene.worldTriggers.Length; ++i)
                {
                    m_vEventPop.Add(levelScene.worldTriggers[i].name + "[" + i + "]");
                }
            }
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            foreach (var db in m_vMeshCollider)
                Framework.ED.EditorUtil.Destroy(db);
            m_vMeshCollider.Clear();

            GameObject.DestroyImmediate(m_pMesh);
            GameObject.DestroyImmediate(m_pDrawMat);
          
            Save();
        }
        //------------------------------------------------------
        void Save()
        {
            LevelScene levelScene = target as LevelScene;
            if(levelScene.worldTriggers!=null)
            {
                for(int i =0; i < levelScene.worldTriggers.Length; ++i)
                {
                    WorldTriggerParamter worldTrigger = levelScene.worldTriggers[i];
                    List<string> eventCmds = new List<string>();
                    if (worldTrigger.runtimeEvents!=null)
                    {
                        for(int j =0; j < worldTrigger.runtimeEvents.Count; ++j)
                        {
                            eventCmds.Add(worldTrigger.runtimeEvents[j].WriteCmd());
                        }
                    }
                    worldTrigger.events = eventCmds;
                    levelScene.worldTriggers[i] = worldTrigger;
                }
            }
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LevelScene levelScene = target as LevelScene;
            GUILayout.BeginHorizontal();
            int preIndex = m_nSelectEvent;
            m_nSelectEvent = EditorGUILayout.Popup("触发器", m_nSelectEvent, m_vEventPop.ToArray());
            if(GUILayout.Button("添加"))
            {
                List<WorldTriggerParamter> vTemp;
                if (levelScene.worldTriggers != null) vTemp = new List<WorldTriggerParamter>(levelScene.worldTriggers);
                else vTemp = new List<WorldTriggerParamter>();

                WorldTriggerParamter tri = new WorldTriggerParamter();
                if (Framework.Module.ModuleManager.mainModule != null)
                {
                    Framework.Core.AFrameworkModule framework = Framework.Module.ModuleManager.mainModule as Framework.Core.AFrameworkModule;
                    if(framework!=null) tri.position = framework.GetPlayerPosition() - levelScene.transform.position;
                }
                else
                    tri.position = new Vector3(0, 2, 50);
                tri.triggetCnt = 0;
                tri.aabb_min = new Vector3(-12, -2, -5);
                tri.aabb_max = new Vector3(12, 20, 5);
                vTemp.Add(tri);
                levelScene.worldTriggers = vTemp.ToArray();
                m_nSelectEvent = levelScene.worldTriggers.Length - 1;
            }
            if (levelScene.worldTriggers != null && m_nSelectEvent >=0 && m_nSelectEvent< levelScene.worldTriggers.Length && GUILayout.Button("删除"))
            {
                List<WorldTriggerParamter> vTemp = new List<WorldTriggerParamter>(levelScene.worldTriggers);
                vTemp.RemoveAt(m_nSelectEvent);
                levelScene.worldTriggers = vTemp.ToArray();
                RefreshEventPop();
                m_nSelectEvent = -1;
            }
            GUILayout.EndHorizontal();

            if (m_nSelectEvent>=0 && m_nSelectEvent< levelScene.worldTriggers.Length)
            {
                WorldTriggerParamter evt = levelScene.worldTriggers[m_nSelectEvent];
                if (m_nSelectEvent != preIndex)
                {
                    if (SceneView.lastActiveSceneView != null)
                    {
                        EditorKits.SceneViewLookat(evt.position);
                    }
                }
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginChangeCheck();
                    evt.name = EditorGUILayout.TextField("描述", evt.name);
                    if (EditorGUI.EndChangeCheck())
                    {
                        RefreshEventPop();
                    }
                    m_bExpandTriggerBase = EditorGUILayout.Foldout(m_bExpandTriggerBase, "基本配置");
                    if (m_bExpandTriggerBase)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUI.BeginChangeCheck();
                        evt.name = EditorGUILayout.TextField("描述", evt.name);
                        evt.triggetCnt = (byte)EditorGUILayout.IntField("触发次数(<=0无限制)", evt.triggetCnt);
                        evt.typeFlags = Framework.ED.EditorEnumPop.PopEnumBit("接受触发类型", evt.typeFlags, typeof(EActorType), null, true);
                        evt.triggetCnt = (byte)EditorGUILayout.IntField("触发次数(<=0无限制)", evt.triggetCnt);
                        evt.position = EditorGUILayout.Vector3Field("Position", evt.position);
                        Vector3 aabb_min = EditorGUILayout.Vector3Field("AABB Min", evt.aabb_min);
                        Vector3 aabb_max = EditorGUILayout.Vector3Field("AABB Max", evt.aabb_max);
                        evt.aabb_min = Vector3.Min(aabb_min, aabb_max);
                        evt.aabb_max = Vector3.Max(aabb_min, aabb_max);
                        if (EditorGUI.EndChangeCheck())
                        {
                            if (SceneView.lastActiveSceneView != null)
                                SceneView.lastActiveSceneView.Repaint();
                        }
                        EditorGUI.indentLevel--;
                    }
                    m_bExpandTriggerEvent = EditorGUILayout.Foldout(m_bExpandTriggerEvent, "事件配置");
                    if (m_bExpandTriggerEvent)
                    {
                        EditorGUI.indentLevel++;
                        if (evt.runtimeEvents != null)
                        {
                            for (int i = 0; i < evt.runtimeEvents.Count; ++i)
                            {
                                GUILayout.BeginHorizontal();
                                string tag = string.Format("event#[{0}]", evt.runtimeEvents[i].GetEventType());
                                if (GUILayout.Button("Del", new GUILayoutOption[] { GUILayout.Width(40) }))
                                {
                                    evt.runtimeEvents.RemoveAt(i);
                                    evt.events.RemoveAt(i);
                                    return;
                                }
                                if (GUILayout.Button("复制", new GUILayoutOption[] { GUILayout.Width(40) }))
                                {
                                    DrawEventCore.AddCopyEvent(evt.runtimeEvents[i]);
                                }
                                if (DrawEventCore.CanCopyEvent(evt.runtimeEvents[i]) &&
                                    GUILayout.Button("黏贴", new GUILayoutOption[] { GUILayout.Width(40) }))
                                {
                                    DrawEventCore.CopyEvent(evt.runtimeEvents[i]);
                                }
                                GUILayout.EndHorizontal();


                                GUILayout.BeginHorizontal();
                                GUILayout.Space(15);
                                GUILayout.BeginVertical();
                                float preTime = evt.runtimeEvents[i].triggetTime;
                                evt.runtimeEvents[i] = DrawEventCore.DrawUnAction(evt.runtimeEvents[i], null);
                                GUILayout.EndVertical();
                                GUILayout.EndHorizontal();
                            }
                        }
                        

                        GUILayout.BeginHorizontal();
                        m_AddEventType = EventPopDatas.DrawEventPop(m_AddEventType, "");
                        EditorGUI.BeginDisabledGroup(m_AddEventType == EEventType.Base || m_AddEventType == EEventType.Count);
                        if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(50) }))
                        {
                            BaseEventParameter parame = BuildEventUtl.BuildEventByType(null, m_AddEventType);
                            if (evt.runtimeEvents == null) evt.runtimeEvents = new List<BaseEventParameter>();
                            evt.runtimeEvents.Add(parame);
                            if (evt.events == null) evt.events = new List<string>();
                            evt.events.Add(null);
                            m_AddEventType = EEventType.Count;
                        }
                        EditorGUI.EndDisabledGroup();
                        GUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }

                    EditorGUI.indentLevel--;
                }

                

                levelScene.worldTriggers[m_nSelectEvent] = evt;
            }

            if (!string.IsNullOrEmpty(levelScene.AssetFile))
            {
                if(GUILayout.Button("保存到预制"))
                {
                    Save();
                    GameObject pPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(levelScene.AssetFile);
                    LevelScene lvScene = pPrefab.GetComponent<LevelScene>();
                    if(lvScene!=null)
                    {
                        lvScene.worldTriggers = levelScene.worldTriggers;
                        EditorUtility.SetDirty(lvScene);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
            else
            {
                PrefabAssetType type = PrefabUtility.GetPrefabAssetType(levelScene.gameObject);
                if (type != PrefabAssetType.NotAPrefab)
                {
                    if (GUILayout.Button("保存"))
                    {
                        Save();
                        GameObject pPrefab = PrefabUtility.GetCorrespondingObjectFromSource(levelScene.gameObject) as GameObject;
                        if(pPrefab!=null)
                        {
                            LevelScene lvScene = pPrefab.GetComponent<LevelScene>();
                            if (lvScene != null)
                            {
                                lvScene.worldTriggers = levelScene.worldTriggers;
                                EditorUtility.SetDirty(lvScene);
                            }
                        }

                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    }
                }
                else
                {
                    if (GUILayout.Button("保存"))
                    {
                        Save();
                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    }
                }
            }
            if(!m_bColliderTest)
            {
                if (GUILayout.Button("启用地表碰撞编辑"))
                {
                    RefreshColliderTest();
                }
            }
        }
        //------------------------------------------------------
        private void OnSceneGUI()
        {
            LevelScene levelScene = target as LevelScene;
            WorldBoundBox worldBox = new WorldBoundBox();
            worldBox.Set(new ExternEngine.FVector3(0, -1.0f, 0.0f), new ExternEngine.FVector3(levelScene.BoxSize.x, levelScene.BoxSize.y+1.0f, levelScene.BoxSize.z));
            CommonUtility.DrawBoundingBox(worldBox.GetCenter(), worldBox.GetHalf(), levelScene.transform.localToWorldMatrix, Framework.Core.CommonUtility.GetVolumeToColor(EVolumeType.Target), false);

            if(levelScene.worldTriggers != null)
            {
                WorldTriggerParamter evt;
                for (int i = 0; i < levelScene.worldTriggers.Length; ++i)
                {
                    evt = levelScene.worldTriggers[i];
                    Quaternion rotation = Quaternion.Euler(evt.eulerAngle + levelScene.transform.eulerAngles);
                    if (m_nSelectEvent == i)
                    {
                        if (Tools.current == Tool.Move)
                            evt.position = Handles.DoPositionHandle(evt.position + levelScene.transform.position, rotation) - levelScene.transform.position;
                        else if (Tools.current == Tool.Rotate)
                        {
                            rotation = Quaternion.Euler(evt.eulerAngle);
                            evt.eulerAngle = Handles.DoRotationHandle(rotation, evt.position + levelScene.transform.position).eulerAngles;
                        }
                    }
                    
                    Vector3 center = (evt.aabb_min + evt.aabb_max) / 2;
                    Vector3 half = evt.aabb_max - center;

                    float fSize = HandleUtility.GetHandleSize(evt.position + levelScene.transform.position);

                    if(m_nSelectEvent == i)
                        Framework.Core.CommonUtility.DrawBoundingBox(center, half, Matrix4x4.TRS(evt.position + levelScene.transform.position, Quaternion.Euler(evt.eulerAngle + levelScene.transform.eulerAngles), levelScene.transform.localScale), Color.magenta, false);
                    else
                        Framework.Core.CommonUtility.DrawBoundingBox(center, half, Matrix4x4.TRS(evt.position + levelScene.transform.position, Quaternion.Euler(evt.eulerAngle + levelScene.transform.eulerAngles), levelScene.transform.localScale), Color.cyan, false);

                    Camera camera = null;
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

                    if (camera != null)
                        Graphics.DrawMesh(m_pMesh, Matrix4x4.TRS(evt.position + levelScene.transform.position, billboardRot, Vector3.one * fSize), m_pDrawMat, 0, camera);

                    levelScene.worldTriggers[i] = evt;
                }
            }

//             if (levelScene.navMesh!=null)
//             {
//                 NavMeshTriangulation tris = NavMesh.CalculateTriangulation();
//                 if (tris.vertices != null)
//                 {
//                     ms_pDebugMesh.Clear();
//                     Color[] colors = new Color[tris.vertices.Length];
//                     for (int i = 0; i < ms_pDebugMesh.vertices.Length; ++i)
//                         colors[i] = new Color(0, 1, 0, 0.25f);
//                     ms_pDebugMesh.vertices = tris.vertices;
//                     ms_pDebugMesh.colors = colors;
//                     ms_pDebugMesh.triangles = tris.indices;
//                 }
//                 ms_CalculateDelta = 1;
//             }
//             Graphics.DrawMesh(ms_pDebugMesh, Matrix4x4.Translate(Vector3.up * 0.5f), Framework.Core.CommonUtility.debugMaterial, LayerMask.NameToLayer(GlobalDef.ms_foregroundLayerName));

        }
        //------------------------------------------------------
        void RefreshColliderTest()
        {
            if (m_bColliderTest) return;
            m_bColliderTest = true;
            LevelScene levelScene = target as LevelScene;
            Renderer[] renders = levelScene.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; ++i)
            {
                if (renders[i].GetComponent<MeshCollider>() == null)
                {
                    MeshCollider collider = renders[i].gameObject.AddComponent<MeshCollider>();
                    collider.hideFlags |= HideFlags.DontSave;
                    m_vMeshCollider.Add(collider);
                }
            }
        }
    }
#endif
}
