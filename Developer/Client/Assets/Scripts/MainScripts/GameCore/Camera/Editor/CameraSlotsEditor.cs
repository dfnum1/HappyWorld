/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	CameraSlots
作    者:	HappLI
描    述:	camera slot
*********************************************************************/
using UnityEngine;
using TopGame.Core;

#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
#endif
namespace Framework.Core
{
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(CameraSlots), true)]
    public class CameraSlotsEditor : UnityEditor.Editor
    {
        bool m_bSyncing = false;
        ED.EditorTimer m_pTimer = new ED.EditorTimer();
        List<CameraSlots.Slot> m_vSlots = new List<CameraSlots.Slot>();
        int m_nSelect = -1;
        int m_nLerpTo = -1;
        CameraSlots.Slot m_Lerping = new CameraSlots.Slot();
        private void OnEnable()
        {
            EditorApplication.update += OnUpdate;
            CameraSlots slot = target as CameraSlots;
            if (slot.slots != null)
                m_vSlots = new List<CameraSlots.Slot>(slot.slots);
            ActionGraphBinder binder = slot.GetComponent<ActionGraphBinder>();
            if (binder) binder.cameraSlot = slot;

            Camera parentCamera = slot.GetComponentInParent<Camera>();
            if (slot.pPreviewCamera != parentCamera)
            {
                if(parentCamera!=null)
                {
                    slot.pPreviewCamera = parentCamera;
                    if (slot.pPreviewCamera != null)
                        slot.transform.SetParent(slot.pPreviewCamera.transform.parent);
                }
            }
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            EditorApplication.update -= OnUpdate;
            CameraSlots slot = target as CameraSlots;
            if (slot == null) return;
            slot.slots = m_vSlots.ToArray();
            ActionGraphBinder binder = slot.GetComponent<ActionGraphBinder>();
            if (binder) binder.cameraSlot = slot;
        }
        //------------------------------------------------------
        Camera GetCamera()
        {
            CameraSlots slot = target as CameraSlots;
            if (slot.pPreviewCamera != null) return slot.pPreviewCamera;
            if(CameraController.getInstance() != null && CameraController.getInstance().IsEditorMode())
            {
                return CameraController.MainCamera;
            }
            if (SceneView.currentDrawingSceneView) return SceneView.currentDrawingSceneView.camera;
            if (SceneView.lastActiveSceneView) return SceneView.lastActiveSceneView.camera;
            return null;
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Camera camera = GetCamera();
            CameraSlots slot = target as CameraSlots;
            m_bSyncing = EditorGUILayout.Toggle("时时更新", m_bSyncing);
            slot.pPreviewCamera = EditorGUILayout.ObjectField("预览相机", slot.pPreviewCamera, typeof(Camera), true) as Camera;
            slot.defaultIndex = EditorGUILayout.IntField("缺省", slot.defaultIndex);
            slot.slotLerp = EditorGUILayout.FloatField("过度时长(<=0无过度)", slot.slotLerp);
            for(int i = 0; i < m_vSlots.Count; ++i)
            {
                CameraSlots.Slot item = m_vSlots[i];
                GUILayout.BeginHorizontal();
                bool bEpxand = EditorGUILayout.Foldout(m_nSelect == i, "slot[" + i + "]");
                if(camera)
                {
                    if (m_nSelect >= 0 && m_nSelect < m_vSlots.Count && m_nSelect != i && GUILayout.Button("LerpTo", new GUILayoutOption[] { GUILayout.Width(60) }))
                    {
                        m_Lerping = m_vSlots[m_nSelect];
                        m_nLerpTo = i;
                    }
                    if (GUILayout.Button("相机参数提取", new GUILayoutOption[] { GUILayout.Width(120) }))
                    {
                        item.position = camera.transform.position - slot.transform.position;
                        item.eulerAngle = camera.transform.eulerAngles;
                        if (SceneView.lastActiveSceneView && camera == SceneView.lastActiveSceneView.camera)
                            item.fov = SceneView.lastActiveSceneView.cameraSettings.fieldOfView;
                        else
                            item.fov = camera.fieldOfView;
                    }
                    if (GUILayout.Button("设置到相机", new GUILayoutOption[] { GUILayout.Width(100) }))
                    {
                        camera.transform.position = slot.transform.position + item.position;
                        camera.transform.eulerAngles = item.eulerAngle;
                        if (SceneView.lastActiveSceneView && camera == SceneView.lastActiveSceneView.camera)
                            SceneView.lastActiveSceneView.cameraSettings.fieldOfView = item.fov;
                        else
                            camera.fieldOfView = item.fov;

                        UpdateGameCamera(slot.transform, item);
                    }
                    if (GUILayout.Button("复制", new GUILayoutOption[] { GUILayout.Width(50) }))
                    {
                        TextEditor t = new TextEditor();
                        t.text = "cameraslot:" + JsonUtility.ToJson(item);
                        t.OnFocus();
                        t.Copy();
                    }
                    if (!string.IsNullOrEmpty(GUIUtility.systemCopyBuffer) && GUIUtility.systemCopyBuffer.StartsWith("cameraslot:") && GUILayout.Button("黏贴", new GUILayoutOption[] { GUILayout.Width(50) }))
                    {
                        string strData = GUIUtility.systemCopyBuffer.Replace("cameraslot:", "");
                        try
                        {
                            item = JsonUtility.FromJson<CameraSlots.Slot>(strData);
                        }
                        catch (System.Exception ex)
                        {
                        	
                        }
                    }
                }
                if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(60) }))
                {
                    if(EditorUtility.DisplayDialog("tips","确定移除?", "移除", "取消"))
                    {
                        m_vSlots.RemoveAt(i);
                        if (i == m_nSelect)
                            m_nSelect = -1;
                        break;
                    }
                }
                GUILayout.EndHorizontal();
                if (bEpxand)
                {
                    m_nSelect = i;
                    EditorGUI.indentLevel++;
                    item.position = EditorGUILayout.Vector3Field("position", item.position);
                    item.eulerAngle = EditorGUILayout.Vector3Field("rotation", item.eulerAngle);
                    item.fov = EditorGUILayout.Slider("fov", item.fov, 10, 160);
                    item.shadowDistance = EditorGUILayout.FloatField("shdowDistance", item.shadowDistance);

                    EditorGUILayout.HelpBox("批量作用只会将该组的参数,设置到带有CameraSlots挂件对应组号中，如果没有改组，将自动生成改组\r\n", MessageType.Info);
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("批量作用该项"))
                    {
                        string strDir = EditorUtility.OpenFolderPanel("批量作用目录", Application.dataPath, "");
                        SetBatchRoot(strDir, slot, i);
                    }
                    if (GUILayout.Button("批量移除"))
                    {
                        string strDir = EditorUtility.OpenFolderPanel("批量作用目录", Application.dataPath, "");
                        DelBatchRoot(strDir, slot, i);
                    }
                    GUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }
                else
                {
                    if (m_nSelect == i)
                    {
                        m_nLerpTo = -1;
                        m_nSelect = -1;
                    }
                }
                m_vSlots[i] = item;
            }
            if(GUILayout.Button("添加机位"))
            {
                CameraSlots.Slot item = new CameraSlots.Slot();
                item.fov = 60;
                m_vSlots.Add(item);
            }
            slot.slots = m_vSlots.ToArray();
            serializedObject.ApplyModifiedProperties();
            if (GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(target);
                Framework.Core.AInstanceAble ables = slot.GetComponent<Framework.Core.AInstanceAble>();
                if(ables)
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(ables.AssetFile);
                    if(prefab)
                    {
                        CameraSlots prefabSlot = prefab.GetComponent<CameraSlots>();
                        if(prefabSlot == null)
                            prefabSlot = prefab.AddComponent<CameraSlots>();
                        if(UnityEditorInternal.ComponentUtility.CopyComponent(slot))
                        {
                            UnityEditorInternal.ComponentUtility.PasteComponentValues(prefabSlot);
                            EditorUtility.SetDirty(prefab);
                            if (SceneView.lastActiveSceneView)
                                SceneView.lastActiveSceneView.ShowNotification(new GUIContent("save " + ables.AssetFile + "  ok!"), 1);
                        }
                    }
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }

        }
        //------------------------------------------------------
        private void OnSceneGUI()
        {
            if (!m_bSyncing) return;
            Camera camera = GetCamera();
            if (camera == null) return;
            CameraSlots slot = target as CameraSlots;
            if (m_nSelect <= -1 || m_nSelect >= m_vSlots.Count) return;
            CameraSlots.Slot item = m_vSlots[m_nSelect];
            UpdateGameCamera(slot.transform, item);

            ED.EditorUtil.RepaintPlayModeView();
            SceneView.RepaintAll();
        }
        //------------------------------------------------------
        void UpdateGameCamera(Transform transform, CameraSlots.Slot item)
        {
            CameraSlots slot = target as CameraSlots;
            if (slot.pPreviewCamera != null)
            {
                slot.pPreviewCamera.transform.position = transform.position + item.position;
                slot.pPreviewCamera.transform.eulerAngles = item.eulerAngle;
                SetGameFov(item.fov);
                return;
            }
            Transform cameraTrans = GetGameTranform();
            if (cameraTrans)
            {
                cameraTrans.position = transform.position + item.position;
                cameraTrans.eulerAngles = item.eulerAngle;

                if (CameraController.getInstance() != null && CameraController.getInstance().IsEditorMode())
                {
                    (CameraController.getInstance() as CameraController).SyncEdit(cameraTrans.position, cameraTrans.eulerAngles);
                }
            }
            SetGameFov(item.fov);
        }
        //------------------------------------------------------
        void SetBatchRoot(string strDir, CameraSlots slot, int index)
        {
            strDir = strDir.Replace("\\", "/");
            if (!strDir.Contains(Application.dataPath))
            {
                EditorUtility.DisplayDialog("提示", "必须是工程内部目录", "好的");
                return;
            }
            strDir = strDir.Replace(Application.dataPath + "/", "Assets/");
            EditorUtility.DisplayProgressBar("批量", "", 0);
            string[] assets = AssetDatabase.FindAssets("t:Prefab", new string[] { strDir });
            for (int j = 0; j < assets.Length; ++j)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assets[j]));
                EditorUtility.DisplayProgressBar("批量", prefab.name, (float)j / (float)assets.Length);
                CameraSlots cameraSlot = prefab.GetComponent<CameraSlots>();
                if (cameraSlot == null)
                    cameraSlot = prefab.GetComponentInChildren<CameraSlots>();
                if (cameraSlot == null) continue;
                if (cameraSlot == slot) continue;
                cameraSlot.slotLerp = slot.slotLerp;
                if (cameraSlot.slots == null || cameraSlot.slots.Length <= 0)
                {
                    cameraSlot.slots = new CameraSlots.Slot[slot.slots.Length];
                    System.Array.Copy(slot.slots, cameraSlot.slots, slot.slots.Length);
                }
                else
                {
                    if (index < cameraSlot.slots.Length)
                    {
                        cameraSlot.slots[index] = slot.slots[index];
                    }
                    else if (index >= cameraSlot.slots.Length)
                    {
                        List<CameraSlots.Slot> temps = new List<ACameraSlots.Slot>(cameraSlot.slots);
                        for(int i= cameraSlot.slots.Length; i <= index; ++i)
                            temps.Add(slot.slots[index]);
                        cameraSlot.slots = temps.ToArray();
                    }
                }
                EditorUtility.SetDirty(prefab);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            EditorUtility.ClearProgressBar();
        }
        //------------------------------------------------------
        void DelBatchRoot(string strDir, CameraSlots slot, int index)
        {
            strDir = strDir.Replace("\\", "/");
            if (!strDir.Contains(Application.dataPath))
            {
                EditorUtility.DisplayDialog("提示", "必须是工程内部目录", "好的");
                return;
            }
            strDir = strDir.Replace(Application.dataPath + "/", "Assets/");
            EditorUtility.DisplayProgressBar("批量", "", 0);
            string[] assets = AssetDatabase.FindAssets("t:Prefab", new string[] { strDir });
            for (int j = 0; j < assets.Length; ++j)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assets[j]));
                EditorUtility.DisplayProgressBar("批量", prefab.name, (float)j / (float)assets.Length);
                CameraSlots cameraSlot = prefab.GetComponent<CameraSlots>();
                if (cameraSlot == null)
                    cameraSlot = prefab.GetComponentInChildren<CameraSlots>();
                if (cameraSlot == null) continue;
                if (cameraSlot == slot) continue;
                if (cameraSlot.slots == null) continue;
                if (index < cameraSlot.slots.Length)
                {
                    List<CameraSlots.Slot> temps = new List<ACameraSlots.Slot>(cameraSlot.slots);
                    temps.RemoveAt(index);
                    cameraSlot.slots = temps.ToArray();
                    EditorUtility.SetDirty(prefab);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            EditorUtility.ClearProgressBar();
        }
        //------------------------------------------------------
        Transform GetGameTranform()
        {
            if(CameraController.getInstance()!=null && CameraController.getInstance().IsEditorMode() )
            {
                return CameraController.getInstance().GetTransform();
            }
            Camera mainCamera = Camera.main;
            if (mainCamera == null) return null;
            return mainCamera.transform;
        }
        //------------------------------------------------------
        void SetGameFov(float fov)
        {
            CameraSlots slot = target as CameraSlots;
            if (slot.pPreviewCamera != null)
            {
                slot.pPreviewCamera.fieldOfView = fov;
                return;
            }
                
            if (CameraController.getInstance() != null && CameraController.getInstance().IsEditorMode())
            {
                CameraController.getInstance().UpdateFov(fov);
                return;
            }
            Camera mainCamera = Camera.main;
            if (mainCamera == null) return;
            mainCamera.fieldOfView = fov;
        }
        //------------------------------------------------------
        void OnUpdate()
        {
            m_pTimer.Update();
            CameraSlots slot = target as CameraSlots;
            if (m_nSelect < 0 || m_nSelect >= m_vSlots.Count)
                return;
            if (!m_bSyncing) return;

            if(m_nLerpTo >= 0 && m_nLerpTo < m_vSlots.Count)
            {
                if (m_nLerpTo == m_nSelect) return;
                CameraSlots.Slot lerpTo = m_vSlots[m_nLerpTo];
                if (slot.slotLerp > 0)
                {
                    m_Lerping.position = Vector3.Lerp(m_Lerping.position, lerpTo.position, m_pTimer.deltaTime * slot.slotLerp);
                    m_Lerping.eulerAngle = Vector3.Lerp(m_Lerping.eulerAngle, lerpTo.eulerAngle, m_pTimer.deltaTime * slot.slotLerp);
                    m_Lerping.fov = Mathf.Lerp(m_Lerping.fov, lerpTo.fov, m_pTimer.deltaTime * slot.slotLerp);
                }
                else
                {
                    m_Lerping = lerpTo;
                }
                UpdateGameCamera(slot.transform, m_Lerping);

                if (Framework.Core.CommonUtility.Equal(m_Lerping.position, lerpTo.position, 0.01f) && Framework.Core.CommonUtility.Equal(m_Lerping.eulerAngle, lerpTo.eulerAngle, 0.01f) &&
                    Mathf.Abs(m_Lerping.fov - lerpTo.fov) <= 0.01f)
                {
                    m_nSelect = m_nLerpTo;
                    m_nLerpTo = -1;
                }
            }
            else
            {
                UpdateGameCamera(slot.transform, m_vSlots[m_nSelect]);
            }
        }
    }
#endif
}