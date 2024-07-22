
using UnityEngine;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#endif

namespace TopGame.Core.Brush
{
    public class WaterBrush : MonoBehaviour
    {
        [HideInInspector]
        public int depthSize = 64;
        [HideInInspector]
        public float BrushSize = 1;
        [HideInInspector]
        public float SubGapColor = 0.01f;
        public bool SubGapColorAmount = true;
        [HideInInspector]
        public float AddGapColor = 1;
        public bool AddGapColorAmount = true;
    }
#if UNITY_EDITOR
    //------------------------------------------------------

    [CustomEditor(typeof(WaterBrush))]
    public class WaterBrushEditor : Editor
    {
        public Texture2D DepthBaker;
        public Vector2 m_curLightMap;
        Vector3 m_CurBrushPos;

        TextureImporter m_pImport = null;
        Material m_pMaterial;
        public MeshCollider m_MeshColliders = null;

        bool m_bEnterEditor = false;
        //------------------------------------------------------
        private void OnEnable()
        {
            WaterBrush water = target as WaterBrush;
            if(water.GetComponent<MeshRenderer>()!=null)
                m_pMaterial = water.GetComponent<MeshRenderer>().sharedMaterial;

            if(m_pMaterial)
            {
                DepthBaker = m_pMaterial.GetTexture("_Depth") as Texture2D;
                if(DepthBaker!=null)
                    water.depthSize = DepthBaker.width;
                string strPath = AssetDatabase.GetAssetPath(DepthBaker);
                m_pImport = AssetImporter.GetAtPath(strPath) as TextureImporter;
                if(m_pImport != null)
                {
                    if(!m_pImport.isReadable)
                    {
                        m_pImport.isReadable = true;
                        m_pImport.SaveAndReimport();
                    }
                }
            }

            if(DepthBaker == null)
                RefreshDepth(water.depthSize);
            RefreshMeshCollision();
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            if (m_MeshColliders)
                m_MeshColliders.enabled = false;
          //      UnityEngine.Object.DestroyImmediate(m_MeshColliders,true);
            //m_MeshColliders = null;
            if (m_pImport!=null)
            {
                m_pImport.isReadable = false;
                m_pImport.SaveAndReimport();
            }
        }
        //------------------------------------------------------
        void RefreshDepth(int size)
        {
            DepthBaker = new Texture2D(size, size, TextureFormat.R8, false);
            for (int x = 0; x < size; ++x)
            {
                for (int z = 0; z < size; ++z)
                {
                    DepthBaker.SetPixel(x, z, Color.black);
                }
            }
            DepthBaker.Apply(false, false);
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            WaterBrush water = target as WaterBrush;
            m_bEnterEditor = EditorGUILayout.Toggle("进入编辑", m_bEnterEditor);
            if(m_bEnterEditor)
            {
                EditorGUILayout.HelpBox("按Shift 键 拖动鼠标擦除\r\n按Ctrl键 拖动鼠标涂深度", MessageType.Info);
            }
            serializedObject.Update();
            water.BrushSize = Mathf.Max(1, EditorGUILayout.FloatField("笔刷大小", water.BrushSize));
            GUILayout.BeginHorizontal();
            if(water.SubGapColorAmount)
                water.SubGapColor = Mathf.Max(0, EditorGUILayout.Slider("擦除流量强度", water.SubGapColor, 0, 1));
            else
                water.SubGapColor = Mathf.Max(0, EditorGUILayout.Slider("擦除强度", water.SubGapColor, 0,1));
            water.SubGapColorAmount = EditorGUILayout.Toggle(water.SubGapColorAmount);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (water.AddGapColorAmount)
                water.AddGapColor = Mathf.Max(0, EditorGUILayout.Slider("笔刷流量强度", water.AddGapColor, 0, 1));
            else
                water.AddGapColor = Mathf.Max(0, EditorGUILayout.Slider("笔刷强度", water.AddGapColor, 0, 1));

            water.AddGapColorAmount = EditorGUILayout.Toggle(water.AddGapColorAmount);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            water.depthSize = Mathf.Max(32, EditorGUILayout.IntField("深度图大小", water.depthSize));
            if((water.depthSize != DepthBaker.width || DepthBaker == null) && GUILayout.Button("应用"))
            {
                if(EditorUtility.DisplayDialog("提示", "是否确定改变图片大小,改变后数据将丢失,是否应用?", "应用", "否"))
                {
                    RefreshDepth(water.depthSize);
                }
            }
            GUILayout.EndHorizontal();
            if (DepthBaker == null) return;
            if(DepthBaker!=null && GUILayout.Button("保存"))
            {
                string strPath = AssetDatabase.GetAssetPath(DepthBaker);
                if(string.IsNullOrEmpty(strPath))
                    strPath = EditorUtility.SaveFilePanel("保存图片", Application.dataPath, "DepthMask", "png");
                if (!string.IsNullOrEmpty(strPath) && !string.IsNullOrEmpty(System.IO.Path.GetFileName(strPath)))
                {
                    DepthBaker.Apply(false, false);
                    var pngData = DepthBaker.EncodeToPNG();
                    System.IO.File.WriteAllBytes(strPath, pngData);
                    string strAsset = strPath.Replace("\\", "/").Replace(Application.dataPath + "/", "Assets/");
                    if (AssetDatabase.LoadAssetAtPath<Texture>(strAsset) == null)
                    {
                        AssetDatabase.ImportAsset(strAsset);

                        if (m_pImport != null)
                        {
                            m_pImport.isReadable = false;
                            m_pImport.SaveAndReimport();
                        }
                        m_pImport = AssetImporter.GetAtPath(strAsset) as TextureImporter;
                        if(m_pImport!=null)
                        {
                            m_pImport.isReadable = true;
                            m_pImport.SaveAndReimport();
                        }
                        CheckTextureFormation(strAsset);
                        DepthBaker = AssetDatabase.LoadAssetAtPath<Texture2D>(strAsset);
                    }

                        if (m_pMaterial)
                        m_pMaterial.SetTexture("_Depth", DepthBaker);
                    EditorUtility.SetDirty(DepthBaker);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                }
            }
            if (DepthBaker != null && GUILayout.Button("另存为保存"))
            {
                string strPath = EditorUtility.SaveFilePanel("保存图片", Application.dataPath, "DepthMask", "png");
                if (!string.IsNullOrEmpty(strPath) && !string.IsNullOrEmpty(System.IO.Path.GetFileName(strPath)))
                {
                    string strAsset = strPath.Replace("\\", "/").Replace(Application.dataPath+"/", "Assets/");
                    DepthBaker.Apply(false, false);
                    var pngData = DepthBaker.EncodeToPNG();
                    System.IO.File.WriteAllBytes(strPath, pngData);
                    if (AssetDatabase.LoadAssetAtPath<Texture>(strAsset) == null)
                    {
                        AssetDatabase.ImportAsset(strAsset);

                        if (m_pImport != null)
                        {
                            m_pImport.isReadable = false;
                            m_pImport.SaveAndReimport();
                        }
                        m_pImport = AssetImporter.GetAtPath(strAsset) as TextureImporter;
                        if (m_pImport != null)
                        {
                            m_pImport.isReadable = true;
                            m_pImport.SaveAndReimport();
                        }
                        CheckTextureFormation(strAsset);
                        DepthBaker = AssetDatabase.LoadAssetAtPath<Texture2D>(strAsset);
                    }
                    if (m_pMaterial)
                        m_pMaterial.SetTexture("_Depth", DepthBaker);
                }
            }
            if (DepthBaker != null && GUILayout.Button("清除"))
            {
                for (int x = 0; x < DepthBaker.width; ++x)
                {
                    for (int z = 0; z < DepthBaker.height; ++z)
                    {
                        DepthBaker.SetPixel(x, z, Color.black);
                    }
                }
                DepthBaker.Apply(false, false);
                m_pMaterial.SetTexture("_Depth", DepthBaker);

            }
            if (m_pMaterial && DepthBaker != null && GUILayout.Button("应用"))
            {
                DepthBaker.Apply(false, false);
                m_pMaterial.SetTexture("_Depth", DepthBaker);

                if(m_pImport == null)
                {
                    string strPath = AssetDatabase.GetAssetPath(DepthBaker);
                    if (!string.IsNullOrEmpty(strPath))
                    {
                        m_pImport = AssetImporter.GetAtPath(strPath) as TextureImporter;
                    }
                }

                if (m_pImport != null)
                {
                    if (!m_pImport.isReadable)
                    {
                        m_pImport.isReadable = true;
                        m_pImport.SaveAndReimport();
                    }
                }

            }
            serializedObject.ApplyModifiedProperties();
        }
        //------------------------------------------------------
        void CheckTextureFormation(string strPath)
        {
            TextureImporter import = TextureImporter.GetAtPath(strPath) as TextureImporter;
            if (import == null) return;

            {
                TextureImporterPlatformSettings setting = import.GetDefaultPlatformTextureSettings();
                if (setting != null)
                {
                    setting.overridden = true;
                    setting.format = TextureImporterFormat.R8;

                    import.SetPlatformTextureSettings(setting);
                    import.SaveAndReimport();
                    return;
                }
            }

            {
                TextureImporterPlatformSettings setting = import.GetPlatformTextureSettings("Standalone");
                if (setting == null)
                {
                    setting = new TextureImporterPlatformSettings();
                    setting.name = "Standalone";
                }
                setting.format = TextureImporterFormat.R8;
            }
            if (import == null) return;
            {
                TextureImporterPlatformSettings setting = import.GetPlatformTextureSettings("Android");
                if (setting == null)
                {
                    setting = new TextureImporterPlatformSettings();
                    setting.name = "Android";
                }
                setting.format = TextureImporterFormat.R8;
            }
            if (import == null) return;
            {
                TextureImporterPlatformSettings setting = import.GetPlatformTextureSettings("iPhone");
                if (setting == null)
                {
                    setting = new TextureImporterPlatformSettings();
                    setting.name = "iPhone";
                }
                setting.format = TextureImporterFormat.R8;
            }
            import.SaveAndReimport();
        }
        //------------------------------------------------------
        private void OnSceneGUI()
        {
            WaterBrush water = target as WaterBrush;
            Vector2 mousePos = Event.current.mousePosition;
            mousePos.y = Screen.height - mousePos.y - 40;
            Camera cam = UnityEditor.SceneView.lastActiveSceneView.camera;
            if (cam == null) return;
            Ray aimRay = cam.ScreenPointToRay(mousePos);

            if (m_bEnterEditor)
            {
                m_CurBrushPos = GetTerrainHeight(aimRay, out m_curLightMap);
                DrawBrush(m_CurBrushPos, water.BrushSize, Color.red);


                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                Vector3 mousePoint = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                if ((Event.current.control || Event.current.shift) && Event.current.type == EventType.MouseDrag && UnityEditor.SceneView.lastActiveSceneView.position.Contains(mousePoint))
                {
                    int starx = (int)(m_curLightMap.x * DepthBaker.width);
                    int starz = (int)(m_curLightMap.y * DepthBaker.height);
                    int brushPexSize = (int)water.BrushSize / 2;
                    Vector2 center = new Vector2(starx, starz);
                    for (int x = starx - brushPexSize; x <= starx + brushPexSize; ++x)
                    {
                        for (int z = starz - brushPexSize; z <= starz + brushPexSize; ++z)
                        {
                            if (x >= 0 && x < DepthBaker.width && z >= 0 && z < DepthBaker.height)
                            {
                                float fFactor = (new Vector2(x, z) - center).magnitude;
                                if(fFactor <= brushPexSize)
                                {
                                    Color color = DepthBaker.GetPixel(x,z);
                                    if (Event.current.shift)
                                    {
                                        if(water.SubGapColorAmount)
                                            color.r -= water.SubGapColor;
                                        else
                                            color.r = water.SubGapColor;
                                        if (color.r < 0) color.r = 0;
                                        DepthBaker.SetPixel(x, z, color);
                                    }
                                    else
                                    {
                                        if (water.AddGapColorAmount)
                                            color.r += water.AddGapColor;
                                        else
                                            color.r = water.AddGapColor;
                                        DepthBaker.SetPixel(x, z, color);
                                    }
                                }
                            }
                        }
                    }
                    DepthBaker.Apply(false, false);
                }
            }

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
                corners[i] = new Vector3(Mathf.Sin(step * i * Mathf.Deg2Rad), 0, Mathf.Cos(step * i * Mathf.Deg2Rad)) * radius + pos;
                corners[i].y = GetTerrainHeight(new Ray(corners[i] + Vector3.up * 10, Vector3.down), out lightMap).y;
            }
            Handles.DrawAAPolyLine(thickNess, corners);
        }
        //------------------------------------------------------
        Vector3 GetTerrainHeight(Ray aimRay, out Vector2 lightMap)
        {
            lightMap = Vector2.one;
            if (m_MeshColliders!=null)
            {
                RaycastHit hit;
                if (m_MeshColliders.Raycast(aimRay, out hit, Mathf.Infinity))
                {
                    lightMap = hit.textureCoord;
                    return hit.point;
                }
            }
            return Framework.Core.CommonUtility.RayHitPos(aimRay.origin, aimRay.direction, 0);
        }
        //------------------------------------------------------
        void RefreshMeshCollision()
        {
            WaterBrush brush = (WaterBrush)target;

            m_MeshColliders = brush.GetComponent<MeshCollider>();
            if (m_MeshColliders)
                m_MeshColliders.enabled = true;
            if(m_MeshColliders == null)
                m_MeshColliders = brush.gameObject.AddComponent<MeshCollider>();
            m_MeshColliders.hideFlags |= HideFlags.DontSaveInEditor | HideFlags.HideInInspector;
        }
    }
#endif
}