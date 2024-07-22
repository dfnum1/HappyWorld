using UnityEngine;
using UnityEngine.Serialization;

namespace Linear_Height_Fog
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [ExecuteInEditMode]
    public class HeightFogGlobal : MonoBehaviour
    {
        public Camera mainCamera;

        [Space(10)]
        [FormerlySerializedAs("fogColor")]

        [Header("                                    Fog")]

        [Tooltip("雾效总体强度")]
        [Range(0, 10)]
        public float FogIntensity = 2.0f;
        [Tooltip("暗调明调切换")]
        [Range(0, 1)]
        public float ContrastTone = 1.0f;


        [Space(10)]
        [Tooltip("雾效颜色")]
        [ColorUsage(false, false)]
        public Color fogColorStart = new Color(0.5f, 0.75f, 1.0f, 1.0f);

        [Space(10)]
        [Tooltip("雾衰减开始距离")]
        public float FogDistanceStart = 0.0f;
        [Tooltip("雾衰减结束距离")]
        
        public float FogDistanceEnd = 1000.0f;
        [Tooltip("距离雾衰减")]
        [Range(0, 1)]
        public float FogDistanceFalloff = 0.15f;

        [Space(10)]
        [Tooltip("地面基准高度")]
        public float CamearRangeMin = 80.0f;
        [Tooltip("摄像机最大高度")]
        public float CamearRangeMax = 150.0f;
        [Tooltip("薄雾结束高度")]
        public float FogHeightStart = 1000.0f;
        [Tooltip("厚雾开始高度")]
        public float FogHeightEnd = 0.0f;
        [Tooltip("高度雾衰减")]
        [Range(1, 10)]
        public float FogHeightFallOff = 10.0f;

        [Space(10)]
        [Header("                                 Skybox")]

        [Range(0, 1)]
        [Tooltip("天空雾强度")]
        public float skyboxFogIntensity = 0.55f;
        [Range(-1.0f, 0.5f)]
        [Tooltip("天空雾高度")]
        public float skyboxFogHeight = 0.0f;
        [Range(0, 1)]
        [Tooltip("天空雾透明度")]
        public float skyboxFogFill = 0.0f;

        [Space(10)]
        [Header("                               Direction")]

        [Range(0, 1)]
        [Tooltip("太阳光对雾影响强度")]
        public float directionalIntensity = 1.0f;
        [Range(1, 64)]
        [Tooltip("太阳光对雾影响衰减")]
        public float directionalFalloff = 32.0f;
        [Tooltip("天空雾散射强度")]
        [Range(0, 1.0f)]
        public float directionalOcclusion = 0.15f;
        [ColorUsage(false, false)]
        [Tooltip("太阳光颜色")]
        public Color directionalColor = new Color(1f, 0.98f, 0.97f, 1f);

        [Space(10)]
        [Header("                                   Noise")]
        [Tooltip("雾效扰动开关")]
        public bool noiseMode = true;
        [Range(0, 1)]
        [Tooltip("雾效扰动强度")]
        public float noiseIntensity = 1.0f;
        [Tooltip("雾效扰动开始距离")]
        public float noiseDistanceEnd = 300.0f;
        [Tooltip("雾效扰动大小比例")]
        public float noiseScale = 70.0f;
        [Tooltip("雾效扰动速度")]
        public Vector3 noiseSpeed = new Vector3(0.25f, 0f, 0.2f);


        //[Space(10)]
        //[Header("                                 Advanced")]
        //public Texture2D HeightMapTex;
        //public EnvironmentProfile envProfile;
        //[Tooltip("排序开关,TA用")]
        private int renderPriority = -1;

        private Material globalMaterial;


        void Awake()
        {
            gameObject.name = "Height Fog Global";

            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;

            GetCamera();

            if (mainCamera != null)
            {
                if (mainCamera.depthTextureMode != DepthTextureMode.Depth || mainCamera.depthTextureMode != DepthTextureMode.DepthNormals)
                {
                    mainCamera.depthTextureMode = DepthTextureMode.Depth;
                }
                FogDistanceStart = mainCamera.nearClipPlane;
                FogDistanceEnd = mainCamera.farClipPlane;
            }
            else
            {
                Debug.Log("[Atmospheric Height Fog] Camera not found! Make sure you have a camera in the scene or your camera has the MainCamera tag!");
            }

            var sphereMeshGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var sphereMesh = sphereMeshGO.GetComponent<MeshFilter>().sharedMesh;
            DestroyImmediate(sphereMeshGO);

            gameObject.GetComponent<MeshFilter>().sharedMesh = sphereMesh;

            globalMaterial = new Material(Shader.Find("Hidden/Rex/HeightFogGlobal"));
            globalMaterial.name = "Atmosphere_Height_Fog";
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = globalMaterial;
        }

        void OnEnable()
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        void OnDisable()
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        void OnDestroy()
        {
        }

        void Update()
        {
            if (mainCamera == null)
            {
                Debug.Log("[Atmospheric Height Fog] " + "Make sure you set scene camera tag to Main Camera for the fog to work!");
                return;
            }
            SetFogSphereSize();
            SetFogSpherePosition();

            SetGlobalMaterials();
            SetRenderQueue();
        }

        void GetCamera()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        void SetGlobalMaterials()
        {
            globalMaterial.SetColor("_DFogColorStart", fogColorStart);

            globalMaterial.SetFloat("_FogDistanceStart", FogDistanceStart);
            globalMaterial.SetFloat("_FogDistanceEnd", FogDistanceEnd);
            globalMaterial.SetFloat("_DistanceFallOff", FogDistanceFalloff);
            globalMaterial.SetFloat("_FogDensity", FogIntensity);
            globalMaterial.SetFloat("_ContrastTone", ContrastTone);
            
            globalMaterial.SetFloat("_CamearRangeMin", CamearRangeMin);
            globalMaterial.SetFloat("_CamearRangeMax", CamearRangeMax);

            globalMaterial.SetFloat("_FogHeightThinOff", FogHeightStart);
            globalMaterial.SetFloat("_FogHeightThickOff", FogHeightEnd);
            globalMaterial.SetFloat("_FogHeightFallOff", FogHeightFallOff);
            //globalMaterial.SetTexture("_HeightMapTex", HeightMapTex);
            //globalMaterial.SetVector("_SceneHeightCamPos", new Vector4(envProfile.overall.CameraWorldStart.x, envProfile.overall.CameraWorldStart.y,envProfile.overall.CameraWorldEnd.x, envProfile.overall.CameraWorldEnd.y));
            //globalMaterial.SetVector("_SceneHeightCamPara", new Vector4(envProfile.overall.SceneCamHeight, envProfile.overall.SceneCamNear,envProfile.overall.SceneCamFar, 0));

            globalMaterial.SetFloat("_DirectionalIntensity", directionalIntensity);
            globalMaterial.SetFloat("_DirectionalFalloff", directionalFalloff);
            globalMaterial.SetFloat("_DirectionalOcclusion", directionalOcclusion);
            globalMaterial.SetVector("_DirectionalColor", directionalColor);

            globalMaterial.SetFloat("_NoiseIntensity", noiseIntensity);
            globalMaterial.SetFloat("_NoiseDistanceEnd", noiseDistanceEnd);
            globalMaterial.SetFloat("_NoiseScale", noiseScale);
            globalMaterial.SetVector("_NoiseSpeed", noiseSpeed);

            globalMaterial.SetFloat("_SkyboxFogIntensity", skyboxFogIntensity);
            globalMaterial.SetFloat("_SkyboxFogHeight", skyboxFogHeight);
            globalMaterial.SetFloat("_SkyboxFogFill", skyboxFogFill);
            if (noiseMode)
            {
                globalMaterial.SetFloat("_NoiseModeBlend", 1.0f);
                globalMaterial.DisableKeyword("AHF_NOISEMODE_OFF");
                globalMaterial.EnableKeyword("AHF_NOISEMODE_PROCEDURAL3D");
            }
            else
            {
                globalMaterial.SetFloat("_NoiseModeBlend", 0.0f);
                globalMaterial.DisableKeyword("AHF_NOISEMODE_PROCEDURAL3D");
                globalMaterial.EnableKeyword("AHF_NOISEMODE_OFF");
            }
        }

        void SetFogSphereSize()
        {
            var cameraFar = mainCamera.farClipPlane - 1;
            gameObject.transform.localScale = new Vector3(cameraFar, cameraFar, cameraFar);
        }

        void SetFogSpherePosition()
        {
            transform.position = mainCamera.transform.position;
        }

        void SetRenderQueue()
        {
            globalMaterial.renderQueue = 4000 + renderPriority;
        }
    }
}

