/********************************************************************
生成日期:	1:11:2020 13:07
类    名: 	GrassBaker
作    者:	HappLI
描    述:	草数据烘焙
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Core
{
    [ExecuteAlways]
    public class GrassBaker : MonoBehaviour
    {
        public string MeshSaveToPath = "Assets/Datas/Scenes/Meshes/Grasses";
        public Texture2D maskImage = null;
        public Material matrial = null;
        public Material matrial_instance = null;
        public GameObject BakeZoom = null;
        public int intersection = 1;
        public bool BakeZoomNormal = false;
        [Range(0,1)]
        public float density = 1;
        public float maxVisibleDistance = 200f;
        public float maxFadeDistance = 200f;
        public float sizeX = 256f;
        public float sizeZ = 256f;
        public float step = 64;
        public Vector4 aspectMean = Vector4.one;
        public Vector4 aspectSpread = Vector4.one*0.5f;
        public Vector4 heightMean = Vector4.one*4;
        public Vector4 heightSpread = Vector4.one ;
        public Vector4 scaleMean = Vector4.one*4;
        public Vector4 scaleSpread = Vector4.one;

        public List<Mesh> Meshs;

        public bool bDrawInstance = false;

        public Camera camera;
        EditGrass m_pGrass;
        GrassInstance m_pGrassInstance = null;
        //------------------------------------------------------
        private void Awake()
        {
            Application.targetFrameRate = 128;
            m_pGrass = new EditGrass(this);
            m_pGrass.SetTransform(transform.localToWorldMatrix);
        }
        //------------------------------------------------------
        public void Bake()
        {
#if UNITY_EDITOR
            GrassObject[] grass = GameObject.FindObjectsOfType<GrassObject>();
            if (grass.Length > 0)
            {
                if (EditorUtility.DisplayDialog("提示", "是否删除保存好的节点，后继续烘焙?", "删除", "取消"))
                {
                    for (int i = 0; i < grass.Length; ++i)
                    {
                        GameObject.DestroyImmediate(grass[i]);
                    }
                }
            }
#endif
            if (m_pGrass == null) m_pGrass = new EditGrass(this);
            if (m_pGrassInstance == null) m_pGrassInstance = new GrassInstance();
            m_pGrass.Clear();
            m_pGrass.setMaxVisibleDistance(maxVisibleDistance);
            m_pGrass.setMaxFadeDistance(maxFadeDistance);
            m_pGrass.setSizeX(sizeX);
            m_pGrass.setSizeY(sizeZ);
            m_pGrass.setStep(step);
            m_pGrass.setDensity(density);
            m_pGrass.setAspect(aspectMean, aspectSpread);
            m_pGrass.setHeight(heightMean, heightSpread);
            m_pGrass.setScale(scaleMean, scaleSpread);
            m_pGrass.SetMaterial(matrial);
            m_pGrass.Bake(camera.transform.position);

            List<GrassBladeData> vDatas = new List<GrassBladeData>();
            m_pGrass.GetDatas(vDatas);
            m_pGrassInstance.SetTransform(transform.localToWorldMatrix);
            m_pGrassInstance.SetMaterial(matrial_instance);
            m_pGrassInstance.SetDatas(vDatas);
        }
        //------------------------------------------------------
        public void SaveToGameMesh()
        {
            if(!MeshSaveToPath.Contains("Assets/"))
            {
#if UNITY_EDITOR

                MeshSaveToPath = EditorUtility.OpenFolderPanel("请选择保存目录", Application.dataPath, "");
                MeshSaveToPath = MeshSaveToPath.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                if (!MeshSaveToPath.Contains("Assets/"))
                {
                    EditorUtility.DisplayDialog("提示", "目录不正确，需要在工程内路径Assets/", "好的");
                    return;
                }
#else
                return;
#endif
            }
#if UNITY_EDITOR

#endif

            List<Mesh> meshs = new List<Mesh>();
            m_pGrass.GetMeshs(meshs);
            if (meshs == null || meshs.Count<=0) return;
            GrassObject[] grass = GameObject.FindObjectsOfType<GrassObject>();

            GameObject grassRoot = null;
            for (int i = 0; i < grass.Length; ++i)
            {
                if (grassRoot == null && grass[i].transform.parent != null)
                {
                    if (grassRoot == null)
                        grassRoot = grass[i].transform.parent.gameObject;
                }
                GameObject.DestroyImmediate(grass[i]);
            }
            if (grassRoot == null)
                grassRoot = new GameObject("grasses");

            int index = 0;
            for (; index < meshs.Count; ++index)
            {
#if UNITY_EDITOR
                meshs[index].name = "grass_" + index.ToString();
                AssetDatabase.CreateAsset(meshs[index], MeshSaveToPath + "/" + meshs[index].name+".mesh");
#endif
                GameObject grassgo = new GameObject("grass");
                grassgo.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                GrassObject grassO = grassgo.AddComponent<GrassObject>();
                grassO.SetShareMesh(meshs[index]);
                grassO.SetShareMaterial(matrial);
                grassO.transform.SetParent(grassRoot.transform);
            }
        }
        //------------------------------------------------------
        public bool IsInZoom(Vector3 position, ref float fHeight, ref Vector3 normal)
        {
            fHeight = 0;
            if (BakeZoom == null) return true;
            RaycastHit hit;
            bool bHit = Physics.Raycast(position, Vector3.down, out hit, 10000f, 1<<BakeZoom.layer);
            if(bHit)
            {
                if(BakeZoomNormal) normal = hit.normal;
                fHeight = hit.point.y;
            }
            return bHit;
        }
        //------------------------------------------------------
        public float GetDensity(Vector3 position, int index)
        {
            if (maskImage == null) return 1;
            float x = (position.x / m_pGrass.sizeX)*maskImage.width;
            float z = (position.z / m_pGrass.sizeY) *maskImage.height;
            if (maskImage.format == TextureFormat.R8)
            {
                if(index == 0) return maskImage.GetPixel((int)x, (int)z).r;
            }
            else if (maskImage.format == TextureFormat.RGHalf || maskImage.format == TextureFormat.RG16)
            {
                if (index == 0) return maskImage.GetPixel((int)x, (int)z).r;
                if (index == 1) return maskImage.GetPixel((int)x, (int)z).g;
            }
            else if (maskImage.format == TextureFormat.RGB24)
            {
                if (index == 0) return maskImage.GetPixel((int)x, (int)z).r;
                if (index == 1) return maskImage.GetPixel((int)x, (int)z).g;
                if (index == 2) return maskImage.GetPixel((int)x, (int)z).b;
            }
            else
            {
                return maskImage.GetPixel((int)x, (int)z)[index];
            }
            return 1;
        }
        //------------------------------------------------------
        public Vector4 GetDensity(Vector3 min, Vector3 max)
        {
            if (maskImage == null) return Vector4.one;
            Vector4 density = Vector4.zero;
            int x0 = Mathf.FloorToInt((min.x / m_pGrass.sizeX) * maskImage.width);
            int y0 = Mathf.FloorToInt((min.z / m_pGrass.sizeY) * maskImage.height);
            int x1 = Mathf.FloorToInt((max.x / m_pGrass.sizeX) * maskImage.width);
            int y1 = Mathf.FloorToInt((max.z / m_pGrass.sizeY) * maskImage.height);
            if(maskImage.format == TextureFormat.R8)
            {
                for (int y = y0; y < y1; y++)
                {
                    for (int x = x0; x < x1; x++)
                    {
                        Color pixel = maskImage.GetPixel(x, y);
                        density.x += pixel.r;
                    }
                }
            }
            else if (maskImage.format == TextureFormat.RGHalf || maskImage.format == TextureFormat.RG16)
            {
                for (int y = y0; y < y1; y++)
                {
                    for (int x = x0; x < x1; x++)
                    {
                        Color pixel = maskImage.GetPixel(x, y);
                        density.x += pixel.r;
                        density.y += pixel.g;
                    }
                }
            }
            else if (maskImage.format == TextureFormat.RGB24)
            {
                for (int y = y0; y < y1; y++)
                {
                    for (int x = x0; x < x1; x++)
                    {
                        Color pixel = maskImage.GetPixel(x, y);
                        density.x += pixel.r;
                        density.y += pixel.g;
                        density.z += pixel.b;
                    }
                }
            }
            else
            {
                for (int y = y0; y < y1; y++)
                {
                    for (int x = x0; x < x1; x++)
                    {
                        Color pixel = maskImage.GetPixel(x, y);
                        density.x += pixel.r;
                        density.y += pixel.g;
                        density.z += pixel.b;
                        density.w += pixel.a;
                    }
                }
            }
            return density/(float)((x1-x0)*(y1-y0));
        }
        //------------------------------------------------------
        public void Update()
        {
            if (m_pGrass == null || camera == null) return;
           // Base.FpsStat.getInstance().Update();

            if(bDrawInstance)
            {
                if (m_pGrassInstance != null)
                {
                    m_pGrassInstance.Update(camera);
                    m_pGrassInstance.Render(camera);
                }
            }
            else
            {
                m_pGrass.Update(camera);
                m_pGrass.Render(camera);

                Meshs.Clear();
                m_pGrass.GetMeshs(Meshs);
                m_pGrass.SetTransform(transform.localToWorldMatrix);
            }
        }
        ////------------------------------------------------------

        //public UnityEngine.UI.Slider DensityUI;
        //public UnityEngine.UI.InputField SizeXUI;
        //public UnityEngine.UI.InputField SizeYUI;
        //public UnityEngine.UI.InputField StepUI;
        //private void OnGUI()
        //{
        //    GUI.Label(new Rect(Screen.width - 200, 0, 200, 20), Base.FpsStat.getInstance().fFps.ToString());
        //    if (GUI.Button(new Rect(Screen.width - 200, 50, 200, 100), "烘焙"))
        //    {
        //        density = DensityUI.value;
        //        float temp;
        //        if (float.TryParse(SizeXUI.text, out temp)) sizeX = temp;
        //        if (float.TryParse(SizeYUI.text, out temp)) sizeZ = temp;
        //        if (float.TryParse(StepUI.text, out temp)) step = temp;
        //        Bake();
        //    }
        //    if (GUI.Button(new Rect(Screen.width - 200, 170, 200, 100), bDrawInstance?"Instancing":"MeshBatch"))
        //    {
        //        bDrawInstance = !bDrawInstance;
        //    }
        //}
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(GrassBaker))]
    [CanEditMultipleObjects]
    public class GrassBakerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GrassBaker baker = target as GrassBaker;
//             EditorGUILayout.PropertyField(serializedObject.FindProperty("DensityUI"), new GUIContent("DensityUI"));
//             EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeXUI"), new GUIContent("SizeXUI"));
//             EditorGUILayout.PropertyField(serializedObject.FindProperty("SizeYUI"), new GUIContent("SizeZUI"));
//             EditorGUILayout.PropertyField(serializedObject.FindProperty("StepUI"), new GUIContent("StepUI"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("MeshSaveToPath"), new GUIContent("草网格保存目录"));
            baker.camera = EditorGUILayout.ObjectField("相机", baker.camera, typeof(Camera), true) as Camera;
            baker.maskImage = EditorGUILayout.ObjectField("烘焙蒙版强度图", baker.maskImage, typeof(Texture2D), false) as Texture2D;
            baker.BakeZoom = EditorGUILayout.ObjectField("根据地表烘焙", baker.BakeZoom, typeof(GameObject), true) as GameObject;
            if(baker.BakeZoom)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BakeZoomNormal"), new GUIContent("是否使用地表法线朝向"));
            baker.matrial = EditorGUILayout.ObjectField("使用材质", baker.matrial, typeof(Material), false) as Material;
            baker.matrial_instance = EditorGUILayout.ObjectField("实例化材质", baker.matrial_instance, typeof(Material), false) as Material;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("density"), new GUIContent("强度"));
            //          EditorGUILayout.PropertyField(serializedObject.FindProperty("maxVisibleDistance"), new GUIContent("最大显示距离"));
            //          EditorGUILayout.PropertyField(serializedObject.FindProperty("maxFadeDistance"), new GUIContent("最大过渡距离"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sizeX"), new GUIContent("烘焙宽度"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sizeZ"), new GUIContent("烘焙深度"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("step"), new GUIContent("烘焙步距"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("aspectMean"), new GUIContent("体型均值"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("aspectSpread"), new GUIContent("体型随机范围"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("heightMean"), new GUIContent("高度均值"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("heightSpread"), new GUIContent("高度随机范围"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleMean"), new GUIContent("缩放均值"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleSpread"), new GUIContent("缩放范围"));


            EditorGUILayout.PropertyField(serializedObject.FindProperty("bDrawInstance"), new GUIContent("实例化渲染模式"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Meshs"), new GUIContent("Meshs"));
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("烘焙"))
            {
                baker.Bake();
            }
            if (GUILayout.Button("保存到节点"))
            {
                baker.SaveToGameMesh();
            }
        }
        //------------------------------------------------------
        private void OnSceneGUI()
        {
            GrassBaker baker = target as GrassBaker;
         //   baker.Update();
        }
    }
#endif
}