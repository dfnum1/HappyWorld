/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	SimCityConfig
作    者:	HappLI
描    述:	模拟经营主城配置
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.Core;
using TopGame.Core;
namespace TopGame.Logic
{
    [System.Serializable]
    public struct CityRoadMesh
    {
        public Material useMaterial;
        public Material gridCell;
        public Mesh[] roadMeshs;
    }
    [System.Serializable]
    public struct CityCameraSetting
    {
        public float cameraFov;// = 20;
      //  public float cameraDistance;// = 220;
        public Vector3 cameraPosition;// = Vector3.zero;
        public Vector3 cameraEulerAngle;// = new Vector3(45, 45, 0);
        public Vector3 farCameraEulerAngle;// = new Vector3(45, 45, 0);

        public Vector3 cameraClampAngleX;// = new Vector2(-360, 360);
        public Vector3 cameraClampAngleY;// = new Vector2(-360, 360);
        public static CityCameraSetting  DEF = new CityCameraSetting()
        {
            cameraFov = 20,
        //    cameraDistance = 220,
            cameraPosition = Vector3.zero,
            cameraEulerAngle = new Vector3(45, 45, 0),
            farCameraEulerAngle = new Vector3(45, 45, 0),

            cameraClampAngleX = new Vector2(-360, 360),
            cameraClampAngleY = new Vector2(-360, 360),
        };
#if UNITY_EDITOR
        public void Draw()
        {
            cameraPosition = UnityEditor.EditorGUILayout.Vector3Field("位置", cameraPosition);
            cameraEulerAngle = UnityEditor.EditorGUILayout.Vector3Field("角度", cameraEulerAngle);
            farCameraEulerAngle = UnityEditor.EditorGUILayout.Vector3Field("拉远角度", farCameraEulerAngle);
            cameraFov = UnityEditor.EditorGUILayout.Slider("FOV", cameraFov, 10, 170);
            cameraClampAngleX = UnityEditor.EditorGUILayout.Vector2Field("ClampAngleX", cameraClampAngleX);
            cameraClampAngleY = UnityEditor.EditorGUILayout.Vector2Field("ClampAngleY", cameraClampAngleY);
        }
#endif
    }

    public class SimCityConfig : MonoBehaviour
    {
        static SimCityConfig ms_Instnace = null;
        public float limitZoomRange = 10;
        public float farLimitZoomRange = 5;

        public float lerpTweenSpeed = -1;

        public bool cameraOverrid = false;
        public CityCameraSetting cityCamera = CityCameraSetting.DEF;
        public CityCameraSetting diyCamera = CityCameraSetting.DEF;

        public bool fixedCameraSlide = false;
        public float fixedCameraSlideThreshold = 1;
        public float fixedCameraSlideScale = 1;
        public int defaultFixedCameraIndex = 0;

        [Framework.Data.DisplayNameGUI("diy地表文件"), Framework.Data.SelectFileGUI("/DatasRef/DIY/BlockTerrains", "BlockTerrains", "blocks")]
        public string diyBlockFile = "";
        public InstanceAbleHandler diyDragArrow;
        public Color diyCanDragColor = new Color(0.0f, 0.8f,0.0f,0.8f);
        public Color diyForbidDragColor = new Color(1.0f, 0.0f, 0.0f, 0.8f);
        public float diyFocusCameraDistance = 100;
        public CityRoadMesh cityRoad = new CityRoadMesh();

        [System.Serializable]
        public class WanderNpc
        {
            public uint configId = 0;
            [Range(0,20)]public float speedLower = 10;
            [Range(1, 20)] public float speedUpper = 20;
            public float scale = 2;
            public byte group;
            [System.NonSerialized]
            public bool bExpand;
        }
        public List<WanderNpc> wanderNpcConfigIDs;// = 61000101;
        public float wanderHeroScale = 2;
        public float wanderSpeedLower = 10;
        public float wanderSpeedUpper = 50;
        public int wanderHeroMaxCnt = 6;
        public int wanderNpcMaxCnt = 20;
        public int wanderNpcEachPathCnt = 1;

        public Material GridMaterial;

        public List<CityZoom> cityZooms = null;

        public float markerLOD = -1;
        public bool crowdCameraView = true;
        public bool limitRotate = true;
        Transform m_pTransform;
        //------------------------------------------------------
        private void Awake()
        {
            m_pTransform = transform;
            ms_Instnace = this;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            m_pTransform = null;
            ms_Instnace = null;
        }
        //------------------------------------------------------
        public static Color DiyCanDragColor
        {
            get { return ms_Instnace != null ? ms_Instnace.diyCanDragColor : new Color(0.0f, 0.8f, 0.0f, 0.8f); }
        }
        //------------------------------------------------------
        public static Color DiyForbidDragColor
        {
            get { return ms_Instnace != null ? ms_Instnace.diyForbidDragColor : new Color(1.0f, 0.0f, 0.0f, 0.8f); }
        }
        //------------------------------------------------------
        public static float DiyFocusCameraDistance
        {
            get { return ms_Instnace != null ? ms_Instnace.diyFocusCameraDistance : 100; }
        }
        //------------------------------------------------------
        public static string DiyBlockFile
        {
            get { return ms_Instnace != null ? ms_Instnace.diyBlockFile : null; }
        }
        //------------------------------------------------------
        public static Material DiyGridMaterial
        {
            get { return ms_Instnace != null ? ms_Instnace.GridMaterial : null; }
        }
        //------------------------------------------------------
        public static CityRoadMesh CityRoadMeshs
        {
            get { return ms_Instnace != null ? ms_Instnace.cityRoad : default; }
        }
        //------------------------------------------------------
        public static InstanceAbleHandler DIYDragArrow
        {
            get { return ms_Instnace != null ? ms_Instnace.diyDragArrow : null; }
        }
        //------------------------------------------------------
        public static WanderNpc RandomWanderNpc
        {
            get 
            {
                if (ms_Instnace == null || ms_Instnace.wanderNpcConfigIDs==null || ms_Instnace.wanderNpcConfigIDs.Count<=0) return null;
                return ms_Instnace.wanderNpcConfigIDs[UnityEngine.Random.Range(0, ms_Instnace.wanderNpcConfigIDs.Count)]; 
            }
        }
        //------------------------------------------------------
        public static List<WanderNpc> RandomWanderNpcs
        {
            get
            {
                if (ms_Instnace == null) return null;
                return ms_Instnace.wanderNpcConfigIDs;
            }
        }
        //------------------------------------------------------
        public static float WanderSpeedLower
        {
            get { return ms_Instnace != null ? Mathf.Min(ms_Instnace.wanderSpeedLower, ms_Instnace.wanderSpeedUpper) : 0; }
        }
        //------------------------------------------------------
        public static float WanderSpeedUpper
        {
            get { return ms_Instnace != null ? Mathf.Max(ms_Instnace.wanderSpeedLower, ms_Instnace.wanderSpeedUpper) : 0; }
        }
        //------------------------------------------------------
        public static float WanderHeroScale
        {
            get { return ms_Instnace != null ? ms_Instnace.wanderHeroScale : 2; }
        }
        //------------------------------------------------------
        public static int WanderHeroMaxCnt
        {
            get { return ms_Instnace != null ? ms_Instnace.wanderHeroMaxCnt : 6; }
        }    
        //------------------------------------------------------
        public static int WanderNpcMaxCnt
        {
            get { return ms_Instnace != null ? ms_Instnace.wanderNpcMaxCnt : 20; }
        }    
        //------------------------------------------------------
        public static int WanderNpcEachPathCnt
        {
            get { return ms_Instnace != null ? ms_Instnace.wanderNpcEachPathCnt : 1; }
        }
        //------------------------------------------------------
        public static float WanderRandomSpeed()
        {
            if (ms_Instnace == null || (ms_Instnace.wanderSpeedLower <= 0 && ms_Instnace.wanderSpeedUpper <= 0)) return -1;
            return UnityEngine.Random.Range(Mathf.Min(ms_Instnace.wanderSpeedLower, ms_Instnace.wanderSpeedUpper), Mathf.Max(ms_Instnace.wanderSpeedLower, ms_Instnace.wanderSpeedUpper));
        }
        //------------------------------------------------------
        public static float LerpTweenSpeed
        {
            get
            {
                if (ms_Instnace == null) return -1;
                return ms_Instnace.lerpTweenSpeed;
            }
        }
        //------------------------------------------------------
        public static bool IsFixedCameraSlide
        {
            get
            {
                if (ms_Instnace == null) return false;
                return ms_Instnace.fixedCameraSlide && ms_Instnace.cityZooms != null && ms_Instnace.cityZooms.Count > 0;
            }
            set
            {
                if (ms_Instnace == null) return;
                ms_Instnace.fixedCameraSlide = value;
            }
        }
        //------------------------------------------------------
        public static int NextFixedCameraIndex(int curIndex)
        {
            if (ms_Instnace == null || !IsFixedCameraSlide || ms_Instnace.cityZooms == null) return curIndex;
            return (curIndex + 1) % ms_Instnace.cityZooms.Count;
        }
        //------------------------------------------------------
        public static int DefaultFixedCameraIndex
        {
            get
            {
                if (ms_Instnace == null) return 0;
                return ms_Instnace.defaultFixedCameraIndex;
            }
        }
        //------------------------------------------------------
        public static float FixedCameraSlideThreshold
        {
            get
            {
                if (ms_Instnace == null) return 1;
                return ms_Instnace.fixedCameraSlideThreshold;
            }
        }
        //------------------------------------------------------
        public static float FixedCameraSlideScale
        {
            get
            {
                if (ms_Instnace == null) return 1;
                return ms_Instnace.fixedCameraSlideScale;
            }
        }
        //------------------------------------------------------
        public static CityZoom GetCityZoom(int index)
        {
            if (ms_Instnace == null || index < 0 || ms_Instnace.cityZooms == null || index >= ms_Instnace.cityZooms.Count) return null;
            return ms_Instnace.cityZooms[index];
        }
        //------------------------------------------------------
        public static CityZoom.FixdeCamera GetFixedCamera(int index)
        {
                if (ms_Instnace == null || index<0 || ms_Instnace.cityZooms == null || index>= ms_Instnace.cityZooms.Count) return null;
            if (ms_Instnace.cityZooms[index] == null) return null;
                return ms_Instnace.cityZooms[index].fixedCamera;
        }
        //------------------------------------------------------
        public static float GetFixedCameraLOD(int index)
        {
            if (ms_Instnace == null) return -1;
            if (!ms_Instnace.fixedCameraSlide) return MarkerLOD;
            if (index < 0 || ms_Instnace.cityZooms == null || index >= ms_Instnace.cityZooms.Count) return MarkerLOD;
            if (ms_Instnace.cityZooms[index] == null) return MarkerLOD;
            float lod = ms_Instnace.cityZooms[index].fixedCamera.markerLOD;
            if (lod <= 0) return ms_Instnace.markerLOD;
            return lod;
        }
        //------------------------------------------------------
        public static float LimitZoomRange
        {
            get
            {
                if (ms_Instnace == null) return 10;
                return ms_Instnace.limitZoomRange;
            }
        }
        //------------------------------------------------------
        public static float FarLimitZoomRange
        {
            get
            {
                if (ms_Instnace == null) return 10;
                return ms_Instnace.farLimitZoomRange;
            }
        }
        //------------------------------------------------------
        public static bool LimitRotate
        {
            get
            {
                if (ms_Instnace == null) return false;
                return ms_Instnace.limitRotate;
            }
        }
        //------------------------------------------------------
        public static float MarkerLOD
        {
            get
            {
                if (ms_Instnace == null) return -1;
                return ms_Instnace.markerLOD;
            }
        }
        //------------------------------------------------------
        public static bool CrowdCameraView
        {
            get
            {
                if (ms_Instnace == null) return true;
                return ms_Instnace.crowdCameraView;
            }
        }
        //------------------------------------------------------
        public static CityCameraSetting GetCityCamera(bool bDiy = false)
        {
            if (ms_Instnace == null) return CityCameraSetting.DEF;
            if (bDiy) return ms_Instnace.diyCamera;
            return ms_Instnace.cityCamera;
        }
        //------------------------------------------------------
        public static bool CameraOverrid
        {
            get
            {
                if (ms_Instnace == null) return false;
                return ms_Instnace.cameraOverrid;
            }
        }
        //------------------------------------------------------
        public static Vector3 ZoomCenter
        {
            get
            {
                if (ms_Instnace == null || ms_Instnace.m_pTransform == null) return Vector3.zero;
                return ms_Instnace.m_pTransform.position;
            }
        }
        //------------------------------------------------------
        public static float GetCurZoomRange(float fFactor)
        {
            if (ms_Instnace == null) return 0;
            if (ms_Instnace.limitZoomRange < 0 && ms_Instnace.farLimitZoomRange < 0) return 0;
            return ms_Instnace.limitZoomRange * (1 - fFactor) + ms_Instnace.farLimitZoomRange * fFactor;
        }
        //------------------------------------------------------
        public static int CheckCityZoomIndex(Vector3 position)
        {
            if (ms_Instnace == null || ms_Instnace.cityZooms == null) return -1;
            for(int i =0; i < ms_Instnace.cityZooms.Count;++i)
            {
                if (ms_Instnace.cityZooms[i] && ms_Instnace.cityZooms[i].IsInRegion(position)) return i;
            }
            return -1;
        }
        //------------------------------------------------------
        public static void AjustDistanceEulerAngle(bool bDIY, ref Vector3 eulerAngle, float fFactor = 0)
        {
            if (ms_Instnace == null) return;
            if(bDIY)
                eulerAngle = ms_Instnace.diyCamera.farCameraEulerAngle* fFactor +ms_Instnace.diyCamera.cameraEulerAngle * (1 - fFactor);
            else
                eulerAngle = ms_Instnace.cityCamera.farCameraEulerAngle * fFactor + ms_Instnace.cityCamera.cameraEulerAngle * (1 - fFactor);
        }
        //------------------------------------------------------
        public static void LimitInZoom(ref Vector3 poistion, float fFactor = 0,float offset =0)
        {
            if (ms_Instnace == null) return;
            if (ms_Instnace.fixedCameraSlide) return;
            if (ms_Instnace.limitZoomRange < 0 && ms_Instnace.farLimitZoomRange < 0) return;
            fFactor = Mathf.Clamp01(fFactor);
            Vector3 center = Vector3.zero;
            float range = GetCurZoomRange(fFactor) + offset;
            if (ms_Instnace.m_pTransform!=null)
            {
                center = ms_Instnace.m_pTransform.position;
            }
            center.y = poistion.y;
            if ((poistion - center).sqrMagnitude > range * range)
            {
                poistion = center + (poistion - center).normalized * range;
            }
        }
    }
}

