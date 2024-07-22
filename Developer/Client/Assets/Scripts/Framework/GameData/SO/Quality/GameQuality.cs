/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GameQuality
作    者:	HappLI
描    述:	动态绑点
*********************************************************************/
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TopGame.Data
{
    public enum EGameQulity 
    {
        [Framework.Plugin.PluginDisplay("低")]
        Low = 0,
        [Framework.Plugin.PluginDisplay("中")]
        Middle,
        [Framework.Plugin.PluginDisplay("高")]
        High,
        [Framework.Plugin.DisableGUI]
        None
    }

    //    [CreateAssetMenu]
    public class GameQuality : ScriptableObject
    {
        static GameQuality ms_pInstnace = null;

        public const string QUALITY_KEY = "SD_QUALITY";
        [System.NonSerialized]
        public EGameQulity gameQulity = EGameQulity.None;
        public QualityConfig[] Configs;

        private int m_nStatFps = 0;
        private float m_fLowerFpsCheck = 0;
        [System.NonSerialized]
        Vector2Int m_Resolution = new Vector2Int(1334, 750);
        public static Vector2Int DefaultResolution
        {
            get
            {
                if (ms_pInstnace) return ms_pInstnace.m_Resolution;
                return new Vector2Int(1334, 750);
            }
        }
        //------------------------------------------------------
        void OnEnable()
        {
            ms_pInstnace = this;
            m_Resolution.x = Screen.width;
            m_Resolution.y = Screen.height;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_pInstnace = null;
        }
        //------------------------------------------------------
        public static void Init()
        {
            if (ms_pInstnace == null)
            {
                Framework.Plugin.Logger.Error("游戏品质参数数据未加载...");
                return;
            }
            long remainMemory = Core.JniPlugin.GetRemainMemory();
            Debug.Log("可用内存:"+Base.GlobalUtil.FormBytes(remainMemory));
            EGameQulity quality = EGameQulity.High;
            if (PlayerPrefs.HasKey(QUALITY_KEY))
            {
                quality = (EGameQulity)PlayerPrefs.GetInt(QUALITY_KEY);
            }
            else
            {
                if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2)
                {
                    quality = EGameQulity.Low;
                }
                else if (ms_pInstnace.Configs != null)
                {
                }
                PlayerPrefs.SetInt(QUALITY_KEY, (int)quality);
            }
            ms_pInstnace.SetQuality(quality,false);
        }
        //-------------------------------------------------
        public static void Update(float fFrame, float lowerFps = -1)
        {
            if (ms_pInstnace == null) return;
            if (ms_pInstnace.gameQulity <= EGameQulity.Low) return;
            if (fFrame <= 0.001f) return;
            ms_pInstnace.m_fLowerFpsCheck -= Time.fixedDeltaTime;
            if (ms_pInstnace.m_fLowerFpsCheck <= 0)
            {
                if (lowerFps <= 0) lowerFps = LowerFpsThreshold;
                ms_pInstnace.m_fLowerFpsCheck = 2;
                if (Mathf.CeilToInt(Base.FpsStat.getInstance().fFps) <= lowerFps)
                {
                    ms_pInstnace.m_nStatFps++;
                }
                else ms_pInstnace.m_nStatFps = 0;
                if (ms_pInstnace.m_nStatFps > 5)
                {
                    ms_pInstnace.m_nStatFps = 0;
                    GameQuality.DemotionQuality();
                }
            }
        }
        //-------------------------------------------------
        public static void SetQuality(EGameQulity level)
        {
            if (ms_pInstnace == null) return;
            ms_pInstnace.SetQuality(level, true);
        }
        //-------------------------------------------------
        public static void DemotionQuality()
        {
            return;
            if (ms_pInstnace == null) return;
            if (ms_pInstnace.gameQulity <= EGameQulity.Low) return;
            ms_pInstnace.SetQuality(ms_pInstnace.gameQulity-1, true);
        }
        //-------------------------------------------------
        public static void OnAppResume()
        {
            if (ms_pInstnace == null) return;
            if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].OnResetResolution(ms_pInstnace.gameQulity, DefaultResolution);
        }
        //-------------------------------------------------
        void SetQuality(EGameQulity level,bool isCheckEqual = true)
        {
            if (isCheckEqual && level == this.gameQulity)
                return;
            PlayerPrefs.SetInt(QUALITY_KEY, (int)level);
            gameQulity = level;
            if (Configs != null && (int)level < Configs.Length)
            {
                Configs[(int)level].Used(level, m_Resolution);
            }
            m_nStatFps = 0;
            m_fLowerFpsCheck = 2;
        }
        //-------------------------------------------------
        public static UniversalRenderPipelineAsset GetCurrentURPAsset()
        {
            return QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
        }
        //-------------------------------------------------
        public static void SetTierTo(GraphicsTier tier)
        {
            Graphics.activeTier = tier;
        }
        //-------------------------------------------------
        public static void SetDefaultTier()
        {
            if (ms_pInstnace == null) return;
            int index = (int)ms_pInstnace.gameQulity;
            if (ms_pInstnace.Configs == null || index >= ms_pInstnace.Configs.Length) return;
            Graphics.activeTier = ms_pInstnace.Configs[index].Tier;
        }
        //------------------------------------------------------
        public static int targetFrameRate
        {
            get
            {
                if (ms_pInstnace == null) return 30;
                else
                {
                    if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                        return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].TargetFrameRate;
                }
                return 30;
            }
        }
        public static bool enableFog
        {
            get
            {
                if (ms_pInstnace == null) return false;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].EnableFog;
                return false;
            }
        }
        public static QualityConfig Config
        {
            get
            {
                if (ms_pInstnace == null) return QualityConfig.DEFAULT;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity];
                return QualityConfig.DEFAULT;
            }
        }
        public static uint urpPassFlags
        {
            get
            {
                if (ms_pInstnace == null) return 0xffffffff;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].nURPPassFlags;
                return 0xffffffff;
            }
        }
        public static bool bLimitParticle
        {
            get
            {
                if (ms_pInstnace == null) return false;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].LimitParticleMaxCount;
                return false;
            }
        }
        public static int nMaxParticleCount
        {
            get
            {
                if (ms_pInstnace == null) return 0;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].nMaxParticleCount;
                return 0;
            }
        }
        public static bool bForceLOWLOD
        {
            get
            {
                if (ms_pInstnace == null) return false;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].bForceLOWLOD;
                return false;
            }
        }

        public static float LowerFpsThreshold
        {
            get
            {
                if (ms_pInstnace == null) return 20;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return Mathf.Max(20, ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].LowerFpsThreshold);
                return 20;
            }
        }

        public static bool bInstanceBrush
        {
            get
            {
                if (ms_pInstnace == null) return true;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].bInstanceBrush;
                return true;
            }
        }
        public static int vSyncCount
        {
            get
            {
                if (ms_pInstnace == null) return 0;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].SyncCount;
                return 0;
            }
        }

        public static float TerrainBrushCheckVisible
        {
            get
            {
                if (ms_pInstnace == null) return 0;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].TerrainBrushCheckVisible;
                return 0;
            }
        }
        public static float ProjectorLightCheckGap
        {
            get
            {
                if (ms_pInstnace == null) return 0.5f;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].ProjectorLightCheckGap;
                return 0.5f;
            }
        }
        
        public static int LODValue
        {
            get
            {
                if (ms_pInstnace == null) return 100;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].LOD;
                return 100;
            }
        }
        public static QualityConfig.ETextureQuality TextureQulity
        {
            get
            {
                if (ms_pInstnace == null) return QualityConfig.ETextureQuality.Full;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].textureQuality;
                return QualityConfig.ETextureQuality.Full;
            }
            set
            {
                QualitySettings.masterTextureLimit = (int)value;
            }
        }
        public static EGameQulity Qulity
        {
            get
            {
                if (ms_pInstnace == null) return EGameQulity.Middle;
                return ms_pInstnace.gameQulity;
            }
            set
            {
                if (ms_pInstnace != null)
                    ms_pInstnace.SetQuality(value);
            }
        }
        public static int OneFrameCost
        {
            get
            {
                if (ms_pInstnace == null) return 300;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].OneFrameCost;
                return 300;
            }
        }
        public static int MaxInstanceCount
        {
            get
            {
                if (ms_pInstnace == null) return 30;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].MaxInstanceCount;
                return 30;
            }
        }
        public static int DestroyDelayTime
        {
            get
            {
                if (ms_pInstnace == null) return 60;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].DestroyDelayTime;
                return 60;
            }
        }

        public static int ThresholdSystemMemory
        {
            get
            {
                if (ms_pInstnace == null) return 1024;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].ThresholdSystemMemory;
                return 1024;
            }
        }

        public static UnityEngine.Rendering.VolumeProfile volumeProfile
        {
            get
            {
                if (ms_pInstnace == null) return null;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].postProcess;
                return null;
            }
        }

        public static UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset
        {
            get
            {
                if (ms_pInstnace == null) return null;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                    return ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].urpAsset;
                return null;
            }
        }

        public static float ShadowDistance
        {
            get
            {
                if (ms_pInstnace == null) return 18;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                {
                    var cfg = ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity];
                    var urpAsset = cfg.urpAsset;
                    if(urpAsset!=null)
                    {
                        return urpAsset.shadowDistance;
                    }
                    return cfg.ShadowDistance;
                }
                return 18;
            }
            set
            {
                if (ms_pInstnace == null) return;
                if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                {
                    var cfg = ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity];
                    var urpAsset = cfg.urpAsset;
                    if (urpAsset != null)
                    {
                        urpAsset.shadowDistance = value;
                    }
                    QualitySettings.shadowDistance = value;
                }
            }
        }

        public static void DemotionResolution(float fFactor)
        {
            if (ms_pInstnace == null) return;
#if UNITY_STANDALONE
            return;
#endif
            Vector2Int SourceResolution = ms_pInstnace.m_Resolution;
            bool bPortrait = (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown);
            int finalW = Mathf.RoundToInt(SourceResolution.x * fFactor);
            int finalH = Mathf.RoundToInt(SourceResolution.y * fFactor);
            if (bPortrait)
            {
                if (finalW < 720)
                {
                    fFactor = 720.0f / (float)SourceResolution.x;
                    finalH = Mathf.RoundToInt(SourceResolution.y * fFactor);
                    finalW = 720;
                }
                else if (finalW > 1080)
                {
                    fFactor = 1080.0f / (float)SourceResolution.x;
                    finalH = Mathf.RoundToInt(SourceResolution.y * fFactor);
                    finalW = 1080;
                }
                if (ms_pInstnace.gameQulity >= EGameQulity.High)
                {
                    if (finalW < 1080)
                    {
                        fFactor = 1080.0f / (float)SourceResolution.x;
                        finalH = Mathf.RoundToInt(SourceResolution.y * fFactor);
                        finalW = 1080;
                    }
                }
                else
                {
                    if (finalW < 720)
                    {
                        fFactor = 720.0f / (float)SourceResolution.x;
                        finalH = Mathf.RoundToInt(SourceResolution.y * fFactor);
                        finalW = 720;
                    }
                }
            }
            else
            {
                if (finalH < 720)
                {
                    fFactor = 720.0f / (float)SourceResolution.y;
                    finalW = Mathf.RoundToInt(SourceResolution.x * fFactor);
                    finalH = 720;
                }
                else if (finalH > 1080)
                {
                    fFactor = 1080.0f / (float)SourceResolution.y;
                    finalW = Mathf.RoundToInt(SourceResolution.x * fFactor);
                    finalH = 1080;
                }
                if (ms_pInstnace.gameQulity >= EGameQulity.High)
                {
                    if (finalH < 1080)
                    {
                        fFactor = 1080.0f / (float)SourceResolution.y;
                        finalW = Mathf.RoundToInt(SourceResolution.x * fFactor);
                        finalH = 1080;
                    }
                }
                else
                {
                    if (finalH < 720)
                    {
                        fFactor = 720.0f / (float)SourceResolution.y;
                        finalW = Mathf.RoundToInt(SourceResolution.x * fFactor);
                        finalH = 720;
                    }
                }
            }
            Screen.SetResolution(finalW, finalH, true);
        }

        public static void RestoreQualityResolution()
        {
            if (ms_pInstnace == null) return;
            if (ms_pInstnace.Configs != null && (int)ms_pInstnace.gameQulity < ms_pInstnace.Configs.Length)
                ms_pInstnace.Configs[(int)ms_pInstnace.gameQulity].OnResetResolution(ms_pInstnace.gameQulity, ms_pInstnace.m_Resolution);
        }
    }
}
