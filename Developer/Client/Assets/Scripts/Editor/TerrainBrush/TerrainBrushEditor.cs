#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TopGame.Core.Brush
{
    [System.Serializable]
    public struct TempCatchFoliageData
    {
        public List<Foliage> datas;
    }
     public  class EditBrsuhData
    {
        public Brush brush;
        public Material useMatrial;
        public Texture icon = null;
        public Rect uv_tile_offset = new Rect(0,0,0,0);
    }
    public enum EEdtitMode
    {
        None,
        Brush,
    }
    public enum EBrushMode
    {
        Brush = 0,
        BrushColor,
        Remove,
        RandomSize,
    }
    public class TerrainBrushEditor
    {
        class TerrainCatch
        {
            public bool bEnabled;
            public MeshCollider collider;
            public Texture2D lightMap;

            public void Destory()
            {
                if (collider)
                {
                    if(!bEnabled)
                        GameObject.DestroyImmediate(collider);
                }
                if (lightMap)
                {
                    AssetImporter asset = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(lightMap));
                    if (asset)
                    {
                        if ((asset as TextureImporter).isReadable)
                        {
                            (asset as TextureImporter).isReadable = false;
                            asset.SaveAndReimport();
                        }
                    }
                }
            }
        }
        Rect m_InspectorSize = new Rect(0,0,0,0);
        Vector2 m_LastMosePos = Vector2.zero;
        Rect m_DrawBrushRect = new Rect(0,0, 300,100);
        Vector2 m_curLightMap;
        Color m_curLightMapColor;
        Vector3 m_CurBrushPos = Vector3.zero;
        bool m_bDrawBrushSetting = false;
        bool m_bDrawPortalCell = true;
        bool m_bRemoveBrushWhenPop = false;
        EBrushMode m_BrushMode = EBrushMode.Brush;
        EEdtitMode m_eEditMode = EEdtitMode.None;
        Texture2D m_pSelectMask = null;
        Material m_pMaskMeshMatrial = null;
        Mesh m_pMaskTextureMesh = null;
        EditBrsuhData m_SelectBrush = null;
        Vector2 m_BrushMaskDetial = Vector2.zero;
        Vector2 m_BrushDetial = Vector2.zero;
        bool m_bExpandBrushBuffer = false;
        Dictionary<GameObject, TerrainCatch> m_vMeshCollider = new Dictionary<GameObject, TerrainCatch>();

        List<EditBrsuhData> m_vBrush = new List<EditBrsuhData>();

        BrushDrawer m_pDrawer = new BrushDrawer();
        SerializedObject m_pSerialized = null;
        bool m_bUpdateSerializedObject = true;

        public bool bEnableRightOpenMenu = true;

        public delegate Vector3 GetTerrainHeightDelegate(Ray aimRay, out Vector2 lightMap, out Color lightColor);
        public GetTerrainHeightDelegate OnGetTerrainHeightEvent = null;

        List<List<Foliage>> m_vUndoFoliages = new List<List<Foliage>>();

        TerrainBrushEditorRuntime m_pRuntimeData = new TerrainBrushEditorRuntime();
        static List<TerrainBrushEditor> ms_vInstnaces = new List<TerrainBrushEditor>();
        //------------------------------------------------------
        public TerrainBrushEditor(TerrainBrush brush, bool bUpdate = true, SerializedObject serialzedObject = null, bool bUseCommandBuffer = false)
        {
            if(bUpdate)
            {
                UnityEditor.EditorApplication.update -= EditorUpdate;
                UnityEditor.EditorApplication.update += EditorUpdate;
            }

            RefreshTerrainMeshCollision(brush);

            m_bUpdateSerializedObject = false;
            if (serialzedObject == null)
            {
                m_pSerialized = new SerializedObject(brush);
                m_bUpdateSerializedObject = true;
            }
            else m_pSerialized = serialzedObject;

            m_pRuntimeData.Init(brush);
            m_pRuntimeData.useCommandBuffer = bUseCommandBuffer;
            Refresh();
            m_pDrawer.Destroy();
            if (m_pMaskTextureMesh == null)
                m_pMaskTextureMesh = new Mesh();
            if (m_pMaskMeshMatrial == null) m_pMaskMeshMatrial = new Material(Shader.Find("Mobile/Particles/Additive"));

            if (m_pDrawer != null)
                m_pDrawer.m_pEditor = this;
        }
        //------------------------------------------------------
        ~TerrainBrushEditor()
        {
            ms_vInstnaces.Remove(this);
        }
        //------------------------------------------------------
        public bool drawPortalCell
        {
            get { return m_bDrawPortalCell; }
            set { m_bDrawPortalCell = value; }
        }
        //------------------------------------------------------
        public TerrainBrushEditorRuntime GetRuntime()
        {
            return m_pRuntimeData;
        }
        //------------------------------------------------------
        public EEdtitMode GetEditorMode()
        {
            return m_eEditMode;
        }
        //------------------------------------------------------
        void Refresh()
        {
            BrushUtil.FindAllBrush(m_vBrush);
        }
        //------------------------------------------------------
        public static void RefreshBrush()
        {
            foreach (var db in ms_vInstnaces)
                db.Refresh();
        }
        //------------------------------------------------------
        public void Clear()
        {
            foreach (var db in m_vMeshCollider)
            {
                if (db.Value.bEnabled) continue;
                db.Value.Destory();
            }
            m_vMeshCollider.Clear();
            m_pDrawer.Clear();
            m_pRuntimeData.Clear();
        }
        //------------------------------------------------------
        public void Save()
        {
            m_pRuntimeData.Save();
        }
        //------------------------------------------------------
        public void Destroy()
        {
            UnityEditor.EditorApplication.update -= EditorUpdate;
            foreach (var db in m_vMeshCollider)
            {
                if (db.Value.bEnabled) continue;
                db.Value.Destory();
            }
            m_vMeshCollider.Clear();
            ms_vInstnaces.Remove(this);
            m_pDrawer.Destroy();
            if(m_pMaskTextureMesh!=null)
            {
                m_pMaskTextureMesh.Clear();
                GameObject.DestroyImmediate(m_pMaskTextureMesh);
            }
            m_pRuntimeData.Save();
            m_pRuntimeData.Clear();
        }
        //------------------------------------------------------
        void SaveToCatch(TerrainBrush brush)
        {
            string strTempFile = Application.dataPath + "/../EditorData/TempTerrainBrushCopy.json";
            if (Directory.Exists(Application.dataPath + "/../EditorData/"))
                Directory.CreateDirectory(Application.dataPath + "/../EditorData");
            if (File.Exists(strTempFile))
                File.Delete(strTempFile);
            TempCatchFoliageData ff = new TempCatchFoliageData();
            ff.datas = m_pRuntimeData.GetFoliages();
            FileStream fs = new FileStream(strTempFile, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(JsonUtility.ToJson(ff, true));
            writer.Close();
        }
        //------------------------------------------------------
        public void RecordUndo(List<Foliage> vFolis)
        {
            if (vFolis != null)
            {
                if (m_vUndoFoliages.Count >= 1024)
                {
                    m_vUndoFoliages.RemoveAt(0);
                }
                m_vUndoFoliages.Add(new List<Foliage>(vFolis.ToArray()));
            }
           // Undo.RecordObject(target, "BrushRes");
        }
        //------------------------------------------------------
        public void OnInspectorGUI(bool bShowPortalSetting=true)
        {
            if (m_pSerialized == null)  return;
            TerrainBrush brush = m_pRuntimeData.GetTerrainBrush();
            if (brush == null) return;
            m_pSerialized.Update();
            if (m_pDrawer!=null)
                m_pDrawer.m_pEditor = this;
            EditorGUILayout.HelpBox("1:按住Shift键可进行擦除操作\n2:按F4键可快速定位刷子\n3:进行刷子操作时，最好锁定面板", MessageType.Info);

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(m_pSerialized.FindProperty("Portals"), new GUIContent("Portals"), true);
            if(GUILayout.Button("刷新保存"))
            {
                m_pRuntimeData.Save();
            }
            GUILayout.EndHorizontal();
            if(bShowPortalSetting)
            {
                Framework.ED.HandleUtilityWrapper.DrawPropertyByFieldName(brush, "portalStart");
                Framework.ED.HandleUtilityWrapper.DrawPropertyByFieldName(brush, "portalChunk");
                Framework.ED.HandleUtilityWrapper.DrawPropertyByFieldName(brush, "portalSizeH");
                Framework.ED.HandleUtilityWrapper.DrawPropertyByFieldName(brush, "portalSizeV");
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_pSerialized.FindProperty("Terrains"), new GUIContent("编辑地表"), true);
            if (EditorGUI.EndChangeCheck() || (brush.Terrains != null && m_vMeshCollider.Count != brush.Terrains.Count) || (brush.Terrains == null && m_vMeshCollider.Count != 0))
            {
                RefreshTerrainMeshCollision(brush);
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_pSerialized.FindProperty("VisiableCheckDepth"), new GUIContent("深度可见"), true);
            EditorGUILayout.PropertyField(m_pSerialized.FindProperty("VisiableOnlyDepth"), new GUIContent("仅检测深度"), true);
            if (EditorGUI.EndChangeCheck())
            {
              //  brush.SetDirtyCmd();
            }
            GUILayout.BeginHorizontal();
            m_bExpandBrushBuffer = EditorGUILayout.Foldout(m_bExpandBrushBuffer, "植被数量:" + m_pRuntimeData.GetFoliagesCount());
            string strTempFile = Application.dataPath + "/../EditorData/TempTerrainBrushCopy.json";
            if (Application.isPlaying && GUILayout.Button("拷贝"))
            {
                SaveToCatch(brush);
            }
            if(File.Exists(strTempFile) && GUILayout.Button("黏贴"))
            {
                try
                {
                    TempCatchFoliageData data = JsonUtility.FromJson<TempCatchFoliageData>(File.ReadAllText(strTempFile)) ;
                    if(data.datas != null)
                    {
                        RecordUndo(m_pRuntimeData.GetFoliages());
                        m_pRuntimeData.SetFoliages(data.datas);

                        EditorUtility.SetDirty(brush);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
                if(!Application.isPlaying)
                    File.Delete(strTempFile);
            }
            if (GUILayout.Button("清除"))
            {
                if(EditorUtility.DisplayDialog("提示", "是否清理?", "是", "否"))
                {
                    SaveToCatch(brush);
                    RecordUndo(m_pRuntimeData.GetFoliages());
                    m_pRuntimeData.SetFoliages(null);
                }
            }
            if (GUILayout.Button("刷新"))
            {
                m_pRuntimeData.SetDirtyCmd();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if(GUILayout.Button(m_eEditMode == EEdtitMode.Brush?"退出画刷模式":"进入画刷模式"))
            {
                if (m_eEditMode == EEdtitMode.Brush) m_eEditMode = EEdtitMode.None;
                else
                    m_eEditMode =  EEdtitMode.Brush;
            }
            //             if (GUILayout.Button("xx"))
            //             {
            //                 MeshFilter mesh = brush.GetComponent<MeshFilter>();
            //                 BrushUtil.ExportBuffBrushData(brush.transform.localToWorldMatrix, mesh.sharedMesh, brush.name);
            //             }
            GUILayout.EndHorizontal();

            if(m_eEditMode == EEdtitMode.Brush)
            {
                GUILayout.Label("笔刷资源");
                m_BrushDetial = GUILayout.BeginScrollView(m_BrushDetial);
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Height(64) });
                Color color = GUI.color;
                float posX = 0;
                for(int i = 0; i < m_vBrush.Count; ++i)
                {
                    Texture icon = m_vBrush[i].icon;
                    if(icon == null)
                    {
                        if(GUILayout.Button(m_vBrush[i].brush.name, new GUILayoutOption[] { GUILayout.Width(64), GUILayout.Height(64) }))
                        {
                            if (m_SelectBrush == m_vBrush[i])
                                m_SelectBrush = null;
                            else
                                m_SelectBrush = m_vBrush[i];
                            m_pDrawer.Select(m_SelectBrush);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("", new GUILayoutOption[] { GUILayout.Width(64), GUILayout.Height(64) }))
                        {
                            if (m_SelectBrush == m_vBrush[i])
                                m_SelectBrush = null;
                            else
                                m_SelectBrush = m_vBrush[i];

                            m_pDrawer.Select(m_SelectBrush);
                        }
                    }
                    Rect rc = GUILayoutUtility.GetLastRect();
                    if(icon)
                        GUI.DrawTextureWithTexCoords(new Rect(rc.x, rc.y, 64,64), icon, m_vBrush[i].uv_tile_offset);
                    if (m_SelectBrush == m_vBrush[i])
                    {
                        GUI.color = Color.red;
                        GUILayout.BeginArea(new Rect(rc.x+ posX + 64-10, rc.yMin, 20, 20));
                        GUILayout.Toggle(true,"", new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) });
                        GUILayout.EndArea();
                    }
                    GUI.color = color;
                    posX += 64;
                }
                GUILayout.EndHorizontal();
                GUILayout.EndScrollView();
            }

            m_InspectorSize = GUILayoutUtility.GetLastRect();

            if (m_pSerialized.ApplyModifiedProperties())
            {
                m_pRuntimeData.SetDirtyCmd();
            }
        }
        //------------------------------------------------------
        public void DrawSetting(bool bDrawSetting = true)
        {
            if (m_eEditMode == EEdtitMode.None) return;
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80;
            m_BrushMode = (EBrushMode)EditorGUILayout.EnumPopup("模式", m_BrushMode);
            if(bDrawSetting)
            {
                BrushRreferences.GetSettings().brushSpacing = EditorGUILayout.Slider("画笔间隔", BrushRreferences.GetSettings().brushSpacing, 0, 10);
                BrushRreferences.GetSettings().dummyBrushSize = EditorGUILayout.Slider("笔刷大小", BrushRreferences.GetSettings().dummyBrushSize, 0.1f, 10f);
                if (m_BrushMode == EBrushMode.Remove || m_BrushMode == EBrushMode.Brush)
                    BrushRreferences.GetSettings().brsuhFlow = EditorGUILayout.Slider("流量", BrushRreferences.GetSettings().brsuhFlow, 0, 100);

                BrushRreferences.GetSettings().brushBeginAngle.x = EditorGUILayout.Slider("随机朝向-Min", BrushRreferences.GetSettings().brushBeginAngle.x, 0, 360);
                BrushRreferences.GetSettings().brushBeginAngle.y = EditorGUILayout.Slider("随机朝向-Max", BrushRreferences.GetSettings().brushBeginAngle.y, 0, 360);
                BrushRreferences.GetSettings().brushScale.x = EditorGUILayout.Slider("随机缩放-Min", BrushRreferences.GetSettings().brushScale.x, 0.1f, 5f);
                BrushRreferences.GetSettings().brushScale.y = EditorGUILayout.Slider("随机缩放-Max", BrushRreferences.GetSettings().brushScale.y, 0.1f, 5f);
                BrushRreferences.GetSettings().guiBrushColor = EditorGUILayout.ColorField("画笔颜色", BrushRreferences.GetSettings().guiBrushColor);
            }
            else
            {
                if (m_BrushMode == EBrushMode.Remove || m_BrushMode == EBrushMode.Brush)
                    BrushRreferences.GetSettings().brsuhFlow = EditorGUILayout.Slider("流量", BrushRreferences.GetSettings().brsuhFlow, 0, 100);
            }
            EditorGUIUtility.labelWidth = labelWidth;
        }
        //------------------------------------------------------
        public void OnSceneGUI(Camera camera, Ray aimRay, Event curEvent)
        {
            TerrainBrush brush = m_pRuntimeData.GetTerrainBrush();
            if (brush == null) return;
            if(m_bDrawPortalCell)
            {
                if (brush.portalSizeH > 0 && brush.portalSizeV > 0 && brush.portalChunk > 0)
                {
                    Handles.Label(brush.portalStart + brush.transform.position, "空间划分起始位置");
                    brush.portalStart = Handles.DoPositionHandle(brush.portalStart + brush.transform.position, Quaternion.identity) - brush.transform.position;
                    Color color = Handles.color;
                    Handles.color = Color.yellow;
                    Bounds bounds = new Bounds();
                    for (int i = 0; i < brush.portalChunk * brush.portalChunk; ++i)
                    {
                        if (m_pRuntimeData.GetPortalBound(i, ref bounds))
                        {
                            Handles.DrawWireCube(bounds.center, bounds.size);
                        }
                    }
                    Handles.color = color;
                }
            }


            if (m_bDrawBrushSetting)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }

            if (m_pDrawer != null)
                m_pDrawer.m_pEditor = this;

            Vector2 mousePos = curEvent.mousePosition;
            Camera cam = camera;
            if (cam == null) return;

            Vector3 curBrush = GetTerrainHeight(aimRay, out m_curLightMap, out m_curLightMapColor);

            if(m_eEditMode == EEdtitMode.Brush)
            {
                Color guiBrushColor = BrushRreferences.GetSettings().guiBrushColor;
                float brushSpacing = BrushRreferences.GetSettings().brushSpacing;

                float guiBrushThickness = BrushRreferences.GetSettings().guiBrushThickness;
                int guiBrushNumCorners = BrushRreferences.GetSettings().guiBrushNumCorners;

                if (!m_bDrawBrushSetting)
                {
                    //disabling selection
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                    m_CurBrushPos = curBrush;
                }
                if (m_SelectBrush!=null)
                {
                    float brushSize = m_SelectBrush.brush.GetSize();
                    //drawing brush
                    Color color = Handles.color;
                    Handles.color = guiBrushColor;
                    Handles.ArrowHandleCap(0, m_CurBrushPos, Quaternion.FromToRotation(Vector3.down, Vector3.forward), 1, EventType.Repaint);
                    Handles.color = color;
                    if (m_BrushMode == EBrushMode.BrushColor)
                        DrawBrush(m_CurBrushPos, BrushRreferences.GetSettings().dummyBrushSize, BrushRreferences.GetSettings().guiBrushColor, guiBrushThickness, guiBrushNumCorners);
                    else if (curEvent.shift || (m_bDrawBrushSetting && m_bRemoveBrushWhenPop) || m_BrushMode == EBrushMode.BrushColor || m_BrushMode == EBrushMode.Remove)
                        DrawBrush(m_CurBrushPos, BrushRreferences.GetSettings().dummyBrushSize, BrushRreferences.GetSettings().guiBrushDelColor, guiBrushThickness, guiBrushNumCorners);
                    else
                        DrawBrush(m_CurBrushPos, BrushRreferences.GetSettings().dummyBrushSize, guiBrushColor, guiBrushThickness, guiBrushNumCorners);
                    //     DrawBrush(m_CurBrushPos, brushSize * brushFallof, guiBrushColor / 2, guiBrushThickness, guiBrushNumCorners);
                    //       DrawMask(m_pSelectMask, m_CurBrushPos, brushSize, guiBrushColor, guiBrushThickness, guiBrushNumCorners);
                    UnityEditor.SceneView.lastActiveSceneView.Repaint();
                }
                else
                {
                    if (m_BrushMode == EBrushMode.BrushColor
                        || m_BrushMode == EBrushMode.RandomSize)
                        DrawBrush(m_CurBrushPos, BrushRreferences.GetSettings().dummyBrushSize, BrushRreferences.GetSettings().guiBrushColor, guiBrushThickness, guiBrushNumCorners);
                    else if (curEvent.shift || (m_bDrawBrushSetting && m_bRemoveBrushWhenPop) || m_BrushMode == EBrushMode.Remove)
                        DrawBrush(m_CurBrushPos, BrushRreferences.GetSettings().dummyBrushSize, BrushRreferences.GetSettings().guiBrushDelColor, guiBrushThickness, guiBrushNumCorners);
                    UnityEditor.SceneView.lastActiveSceneView.Repaint();
                }

                if (m_bDrawBrushSetting)
                {
                    {
                        m_DrawBrushRect.height = 160;
                        if (m_BrushMode == EBrushMode.Remove || m_BrushMode == EBrushMode.Brush)
                            m_DrawBrushRect.height += 30;
                        GUILayout.BeginArea(m_DrawBrushRect);
                        GUI.DrawTexture(new Rect(0, 0, m_DrawBrushRect.width, m_DrawBrushRect.height), Texture2D.blackTexture);
                        //EditorGUI.BeginChangeCheck();
                        DrawSetting();
                        GUILayout.EndArea();
                    }
                }

                if (curEvent.type == EventType.MouseUp)
                {
                    if(bEnableRightOpenMenu)
                    {
                        if ((m_LastMosePos - curEvent.mousePosition).sqrMagnitude <= 0.1f)
                        {
                            if (curEvent.button == 1)
                            {
                                if (m_bDrawBrushSetting)
                                {
                                    m_bDrawBrushSetting = false;
                                    m_bRemoveBrushWhenPop = false;
                                }
                                else
                                {
                                    m_DrawBrushRect.x = curEvent.mousePosition.x;
                                    m_DrawBrushRect.y = curEvent.mousePosition.y;
                                    m_bDrawBrushSetting = true;
                                    m_bRemoveBrushWhenPop = curEvent.shift;
                                }
                            }
                            else if (curEvent.button != 1)
                            {
                                m_bDrawBrushSetting = false;
                                m_bRemoveBrushWhenPop = false;
                            }
                        }
                    }
                }
                else if (curEvent.type == EventType.MouseDown)
                {
                    m_LastMosePos = curEvent.mousePosition;
                }
            }
            if (curEvent.type == EventType.KeyDown)
            {
                if (curEvent.control && curEvent.keyCode == KeyCode.Z)
                {
                    if(m_vUndoFoliages.Count>0)
                    {
                        m_pRuntimeData.SetFoliages(m_vUndoFoliages[m_vUndoFoliages.Count-1]);
                        m_vUndoFoliages.RemoveAt(m_vUndoFoliages.Count - 1);
                    }
                }
            }

//             Vector3 mousePoint = GUIUtility.GUIToScreenPoint(curEvent.mousePosition);
//             if (!UnityEditor.SceneView.lastActiveSceneView.position.Contains(mousePoint))
//                 return;
            m_pDrawer.OnSceneGUI(this, camera, m_BrushMode, m_CurBrushPos, m_curLightMap, m_curLightMapColor, curEvent);
        }
        //------------------------------------------------------
        public Vector3 GetCurBrushPos()
        {
            return m_CurBrushPos;
        }
        //------------------------------------------------------
        public bool GetLightColor(Camera camera, ref Vector3 worldPos, out Vector2 lightMap, out Color lightColor)
        {
            lightColor = Color.white;
            lightMap = Vector2.one;
            Camera cam = camera? camera:UnityEditor.SceneView.lastActiveSceneView.camera;
            if (cam == null) return false;

            Ray animRay = cam.ScreenPointToRay(cam.WorldToScreenPoint(worldPos));
            if (m_vMeshCollider.Count > 0)
            {
                foreach (var db in m_vMeshCollider)
                {
                    if (db.Value.collider == null)
                    {
                        db.Value.Destory();
                        return false;
                    }
                    RaycastHit hit;
                    if (db.Value.collider.Raycast(animRay, out hit, Mathf.Infinity))
                    {
                        lightMap = hit.textureCoord2;
                        if (db.Value.lightMap == null)
                        {
                            Renderer render = db.Value.collider.GetComponent<Renderer>();
                            if (render != null && render.sharedMaterial)
                            {
                                if (render.sharedMaterial.HasProperty("_LightMap"))
                                {
                                    db.Value.lightMap = render.sharedMaterial.GetTexture("_LightMap") as Texture2D;
                                    if (db.Value.lightMap)
                                    {
                                        AssetImporter asset = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(db.Value.lightMap));
                                        if (asset)
                                        {
                                            if (!(asset as TextureImporter).isReadable)
                                            {
                                                (asset as TextureImporter).isReadable = true;
                                                asset.SaveAndReimport();
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (db.Value.lightMap)
                                m_curLightMapColor = db.Value.lightMap.GetPixel(Mathf.FloorToInt(lightMap.x * db.Value.lightMap.width), Mathf.FloorToInt((lightMap.y * db.Value.lightMap.height)));
                            else
                                m_curLightMapColor = Color.white;
                        }
                        worldPos.y = hit.point.y;
                        return true;
                    }
                }
            }
            return false;
        }
        //------------------------------------------------------
        void EditorUpdate()
        {
            PreviewDraw(null);
            Update(0);
        }
        //------------------------------------------------------
        public void Update(float fDelta, Event pEvt = null)
        {
            if (pEvt == null) pEvt = Event.current;
            if (pEvt != null && pEvt.type == EventType.KeyDown)
            {
                if (pEvt.keyCode == KeyCode.Escape)
                    m_bDrawBrushSetting = false;
            }
            if (m_bUpdateSerializedObject) m_pSerialized.Update();
        }
        //------------------------------------------------------
        public void PreviewDraw(Camera camera)
        {
            if (camera == null) camera = SceneView.lastActiveSceneView.camera;
            if (m_pDrawer != null)
            {
                m_pDrawer.m_pEditor = this;

                m_pDrawer.Update(camera,m_pRuntimeData);
            }
        }
        //------------------------------------------------------
        void DrawMask(Texture2D mask, Vector3 pos, float radius, Color color, float thickNess = 3f, int numCorners = 32)
        {
            return;
            Vector2[] uvs = new Vector2[numCorners + 2];
            Vector3[] corners = new Vector3[numCorners + 2];
            uvs[0] = new Vector2(0.5f,0.5f);
            List<int> tris = new List<int>();
            corners[0] = pos +Vector3.up*0.1f;
            float step = 360f / numCorners;
            for (int i = 1; i <= corners.Length - 1; i++)
            {
                Vector2 lightMap;
                Color lightColor;
                corners[i] = new Vector3(Mathf.Sin(step * i * Mathf.Deg2Rad), 0, Mathf.Cos(step * i * Mathf.Deg2Rad)) * radius + pos;
                corners[i].y = GetTerrainHeight(new Ray(corners[i] + Vector3.up * 10, Vector3.down), out lightMap, out lightColor).y + 0.1f;
                uvs[i] = new Vector2(Mathf.Sin(step * i * Mathf.Deg2Rad), Mathf.Cos(step * i * Mathf.Deg2Rad));

                if(i +1 < corners.Length)
                {
                    tris.Add(0);
                    tris.Add(i);
                    tris.Add(i + 1);
                }

            }
            if (m_pMaskTextureMesh == null)
                m_pMaskTextureMesh = new Mesh();
            if(m_pMaskMeshMatrial == null) m_pMaskMeshMatrial = new Material(Shader.Find("Mobile/Particles/Additive"));


            m_pMaskTextureMesh.vertices = corners;
            m_pMaskTextureMesh.uv = uvs;
            m_pMaskTextureMesh.triangles = tris.ToArray();
            m_pMaskMeshMatrial.mainTexture = mask;
            Graphics.DrawMesh(m_pMaskTextureMesh, Matrix4x4.identity, m_pMaskMeshMatrial,0);
        }
        //------------------------------------------------------
        void DrawBrush(Vector3 pos, float radius, Color color, float thickNess = 3f, int numCorners = 32)
        {
            //incline is the height delta in one unit distance
            Handles.color = color;

            Vector3[] corners = new Vector3[numCorners + 1];
            float step = 360f / numCorners;
            for (int i = 0; i <= corners.Length - 1; i++)
            {
                Vector2 lightMap;
                Color lightColor;
                corners[i] = new Vector3(Mathf.Sin(step * i * Mathf.Deg2Rad), 0, Mathf.Cos(step * i * Mathf.Deg2Rad)) * radius + pos;
                corners[i].y = GetTerrainHeight(new Ray(corners[i]+Vector3.up*10, Vector3.down), out lightMap, out lightColor).y;
            }
            Handles.DrawAAPolyLine(thickNess, corners);
        }
        //------------------------------------------------------
        Vector3 GetTerrainHeight(Ray aimRay, out Vector2 lightMap, out Color lightColor)
        {
            if(OnGetTerrainHeightEvent!=null)
            {
                Vector3 hitPos = OnGetTerrainHeightEvent(aimRay, out lightMap, out lightColor);
                m_curLightMapColor = lightColor;
                return hitPos;
            }
            lightColor = Color.white;
            lightMap = Vector2.one;
            if(m_vMeshCollider.Count>0)
            {
                foreach (var db in m_vMeshCollider)
                {
                    if(db.Value.collider == null)
                    {
                        db.Value.Destory();
                        return Framework.Core.CommonUtility.RayHitPos(aimRay.origin, aimRay.direction, 0);
                    }
                    RaycastHit hit;
                    if (db.Value.collider.Raycast(aimRay, out hit, Mathf.Infinity))
                    {
                        lightMap = hit.textureCoord2;
                        if(db.Value.lightMap == null)
                        {
                            Renderer render = db.Value.collider.GetComponent<Renderer>();
                            if (render != null && render.sharedMaterial)
                            {
                                if(render.sharedMaterial.HasProperty("_LightMap"))
                                {
                                    db.Value.lightMap = render.sharedMaterial.GetTexture("_LightMap") as Texture2D;
                                    if (db.Value.lightMap)
                                    {
                                        AssetImporter asset = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(db.Value.lightMap));
                                        if (asset)
                                        {
                                            if (!(asset as TextureImporter).isReadable)
                                            {
                                                (asset as TextureImporter).isReadable = true;
                                                asset.SaveAndReimport();
                                            }
                                        }
                                    }
                                }
      
                            }
                        }
                        else
                        {
                            if (db.Value.lightMap)
                                m_curLightMapColor = db.Value.lightMap.GetPixel(Mathf.FloorToInt(lightMap.x * db.Value.lightMap.width), Mathf.FloorToInt((lightMap.y * db.Value.lightMap.height)));
                            else
                                m_curLightMapColor = Color.white;
                        }

                        return hit.point;
                    }
                }
            }
            return Framework.Core.CommonUtility.RayHitPos(aimRay.origin, aimRay.direction, 0);
        }
        //------------------------------------------------------
        public void RefreshTerrainMeshCollision(UnityEngine.Object target)
        {
            if (OnGetTerrainHeightEvent != null) return;
            TerrainBrush brush = (TerrainBrush)target;

            foreach (var db in m_vMeshCollider)
            {
                if (db.Value.bEnabled) continue;
                MeshCollider collider = db.Key.transform.GetComponent<MeshCollider>();
                if (collider != null)
                    GameObject.DestroyImmediate(collider);
            }
            m_vMeshCollider.Clear();
            if (brush.Terrains != null)
            {
                for (int i = 0; i < brush.Terrains.Count; ++i)
                {
                    if(brush.Terrains[i] == null) continue;

                    MeshRenderer[] meshRenders = brush.Terrains[i].GetComponentsInChildren<MeshRenderer>();
                    if (meshRenders == null) continue;
                    for(int j =0; j< meshRenders.Length; ++j)
                    {
                        bool bEnabled = true;
                        MeshCollider collider = meshRenders[j].GetComponent<MeshCollider>();
                        if (collider == null)
                        {
                            bEnabled = false;
                            collider = meshRenders[j].gameObject.AddComponent<MeshCollider>();
                        }
                        collider.hideFlags |= HideFlags.DontSave;
                        TerrainCatch oldCollider = null;
                        if (m_vMeshCollider.TryGetValue(meshRenders[j].gameObject, out oldCollider) && oldCollider != null)
                        {
                            oldCollider.Destory();
                        }
                        if (oldCollider == null)
                        {
                            oldCollider = new TerrainCatch();
                        }
                        oldCollider.lightMap = null;
                        oldCollider.collider = collider;
                        oldCollider.bEnabled = bEnabled;
                        m_vMeshCollider[meshRenders[j].gameObject] = oldCollider;
                    }
                }
            }
        }
    }
}
#endif