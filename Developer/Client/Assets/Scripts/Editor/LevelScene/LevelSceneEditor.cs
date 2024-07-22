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
    [CustomEditor(typeof(LevelScene), true)]
    public class LevelSceneEditor : Editor
    {
        enum SplineType
        {
            [Framework.Data.DisplayNameGUI("路径点曲线")]LinePoint,
            [Framework.Data.DisplayNameGUI("相机跟随曲线")] CameraPoint,
        }
        Material m_pDrawMat;
        Mesh m_pMesh = null;
        int m_nSelectEvent = -1;

        float m_fExternLine = 0;

        EEventType m_AddEventType;

        GameObject m_pLineFbx = null;

        SplineType m_SpineEditorType = SplineType.LinePoint;
        Framework.ED.Spline3DEditor m_pSplineEditor = new Spline3DEditor();
        Framework.ED.Spline3DEditor m_pCameraSplineEditor = new Spline3DEditor();

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

            m_pSplineEditor.EnableGUI = false;
            m_pSplineEditor.EnableTan = false;
            m_pSplineEditor.ShowPositionHandle = false;
            LevelScene levelScene = target as LevelScene;
            m_pSplineEditor.OffsetStart = Vector3.zero;
            if (levelScene.LinePoints != null)
            {
                for (int i = 0; i < levelScene.LinePoints.Count; ++i)
                {
                    m_pSplineEditor.AddPoint(i, levelScene.LinePoints[i].position, levelScene.LinePoints[i].rotate, Vector3.zero, Vector3.zero);
                }
            }

            if (levelScene.CameraPoints != null)
            {
                for (int i = 0; i < levelScene.CameraPoints.Count; ++i)
                {
                    m_pCameraSplineEditor.AddPoint(i, levelScene.CameraPoints[i].position, levelScene.CameraPoints[i].inTan, levelScene.CameraPoints[i].outTan);
                }
            }

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
            SaveCureLine();
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
        void SaveCureLine()
        {
            LevelScene levelScene = target as LevelScene;
            levelScene.LinePoints = new List<LevelScene.LinePoint>();
            foreach (var db in m_pSplineEditor.Points)
            {
                LevelScene.LinePoint curve = new LevelScene.LinePoint();
                curve.position = db.point + m_pSplineEditor.OffsetStart;
                curve.rotate = db.rotation;
                levelScene.LinePoints.Add(curve);
            }

            levelScene.CameraPoints = new List<LevelScene.CameraPoint>();
            foreach (var db in m_pCameraSplineEditor.Points)
            {
                LevelScene.CameraPoint curve = new LevelScene.CameraPoint();
                curve.position = db.point + m_pCameraSplineEditor.OffsetStart;
                curve.inTan = db.inTan;
                curve.outTan = db.outTan;
                levelScene.CameraPoints.Add(curve);
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

            m_SpineEditorType = (SplineType)Framework.ED.HandleUtilityWrapper.PopEnum("曲线编辑", m_SpineEditorType);
            if(m_SpineEditorType == SplineType.CameraPoint)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("路径点个数：" + m_pCameraSplineEditor.Points.Count);
                if (GUILayout.Button("清除"))
                {
                    m_pCameraSplineEditor.ClearPoints();
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("路径点个数：" + m_pSplineEditor.Points.Count);
                EditorGUI.BeginDisabledGroup(m_pSplineEditor.Points.Count <= 1);
                if (GUILayout.Button("路径反转"))
                {
                    m_pSplineEditor.Revert();
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                m_pLineFbx = EditorGUILayout.ObjectField(m_pLineFbx, typeof(GameObject), true) as GameObject;
                EditorGUI.BeginDisabledGroup(m_pLineFbx == null);
                m_fExternLine = EditorGUILayout.Slider(m_fExternLine, -10, 10);
                if (GUILayout.Button("路径曲线烘焙"))
                {
                    BakeLinePoints(levelScene, m_pLineFbx, m_fExternLine);
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
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
            worldBox.Set(new ExternEngine.FVector3(-levelScene.BoxSize.x / 2.0f, -1.0f, 0.0f), new ExternEngine.FVector3(levelScene.BoxSize.x / 2.0f, levelScene.BoxSize.y+1.0f, levelScene.BoxSize.z));
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
            if (m_SpineEditorType == SplineType.LinePoint)
            {
                if (m_pSplineEditor.Points != null && m_pSplineEditor.Points.Count > 1)
                {
                    m_pSplineEditor.DrawPointHandle(0, levelScene.transform.position, levelScene.transform.eulerAngles);
                    m_pSplineEditor.DrawPointHandle(m_pSplineEditor.Points.Count - 1, levelScene.transform.position, levelScene.transform.eulerAngles);
                }
                m_pSplineEditor.OnSceneGUI(levelScene.transform.position, levelScene.transform.eulerAngles);

                if (m_pSplineEditor.Points != null && m_pSplineEditor.Points.Count > 1)
                {
                    m_pSplineEditor.DrawPointHandleGUI(0, levelScene.transform.position, levelScene.transform.eulerAngles);
                    m_pSplineEditor.DrawPointHandleGUI(m_pSplineEditor.Points.Count - 1, levelScene.transform.position, levelScene.transform.eulerAngles);
                }
            }
            else
            {
                m_pCameraSplineEditor.OnSceneGUI(levelScene.transform.position, levelScene.transform.eulerAngles);
            }
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
        //------------------------------------------------------
        void BakeLinePoints(LevelScene levelScene, GameObject lineRoot, float externLine)
        {
            if (lineRoot == null) return;
            levelScene.LinePoints = new List<LevelScene.LinePoint>();
            for(int i =0; i < lineRoot.transform.childCount; ++i)
            {
                LevelScene.LinePoint lp = new LevelScene.LinePoint();
                Transform point = lineRoot.transform.GetChild(i);
                lp.position = point.position + (point.up+point.right)*externLine;
                lp.rotate = point.eulerAngles;
                levelScene.LinePoints.Add(lp);
            }
            m_pSplineEditor.ClearPoints();
            for (int i = 0; i < levelScene.LinePoints.Count; ++i)
            {
                m_pSplineEditor.AddPoint(i, levelScene.LinePoints[i].position, levelScene.LinePoints[i].rotate, Vector3.zero, Vector3.zero);
            }
        }
    }
#endif
}
