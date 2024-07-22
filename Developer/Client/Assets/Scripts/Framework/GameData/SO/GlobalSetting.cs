/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	全局参数设置
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Base;
using UnityEngine;
#if USE_SERVER
using ScriptableObject = ExternEngine.ScriptableObject;
using AnimationCurve = ExternEngine.AnimationCurve;
#endif
namespace TopGame.Data
{
    [System.Serializable]
    public struct LookFocusScatter
    {
        public int lableID;
        [Framework.Data.DisplayNameGUI("相机目标点分散抖动轴向随机区间(-v,v)")]
        public Vector3 LookFocusScatterParam;
        [Framework.Data.DisplayNameGUI("相机目标点分散抖动强度")]
        public float LookFocusScatterIntensity;
        [Framework.Data.DisplayNameGUI("相机目标点分散抖动间隔")]
        public float LookFocusScatterFrequency;
        public static LookFocusScatter DEFAULT = new LookFocusScatter() { LookFocusScatterParam = Vector3.zero, LookFocusScatterIntensity =0, LookFocusScatterFrequency = 0 };
    }
    //  [CreateAssetMenu]
    [ExternEngine.ConfigPath("sos/GlobalSetting.asset")]
    public class GlobalSetting : ScriptableObject
    {
        [Framework.Data.DisplayNameGUI("UI滑动操作敏感度")]
        [Range(0.01f, 100f)]
        public float fTouchUISensitivity = 0.01f;

        [Framework.Data.DisplayNameGUI("下落初始冲量")]
        public float fFallingVelocity = 0;
        [Framework.Data.DisplayNameGUI("跳跃速度因子")]
        public float fJumpSpeed = 1;
        [Range(-40f, 40f)]
        [Framework.Data.DisplayNameGUI("跳跃初始冲量")]
        public float fJumpInitVelocity = 1;

        [Framework.Data.DisplayNameGUI("掉落表现速度")]
        public float fDropSpeed = 5.0f;

        [Framework.Data.DisplayNameGUI("战斗最低倍速")]
        public float lowerBattleSpeed = 1;

        [Framework.Data.DisplayNameGUI("战斗帧数倍数")]
        public float frameFpsScale = 1.5f;

        [Framework.Data.DisplayNameGUI("飞行道具碰撞过滤")]
        [Framework.Data.DisplayEnumBitGUI(typeof(EObstacleType), true)]
        public ushort bitProjectileIgnoreFilter = (1<<(int)EObstacleType.Food | 1 << (int)EObstacleType.Gold | 1 << (int)EObstacleType.Transfer | 
            1 << (int)EObstacleType.Trigger | 1 << (int)EObstacleType.Trap);

        [Framework.Data.DisplayNameGUI("是否显示激励视频按钮")]
        public bool bAlwayShowADBtn = true;

#if !USE_SERVER
        [Framework.Data.DisplayNameGUI("LOD 配置")]
        [Framework.Plugin.DisableGUI]
        public Core.LODRank[] Lods = null;
#endif

        [Framework.Data.DisplayNameGUI("动态避障刷新频率"), Range(0.1f, 10)]
        public float RVOTimeHorizon = 5;
        [Framework.Data.DisplayNameGUI("静态避障刷新频率"),Range(0.1f, 10)]
        public float RVOTimeHorizonObst = 5;

        [Framework.Plugin.DisableGUI]
        public LookFocusScatter[] LookFocusScatters = null;
        [Framework.Plugin.DisableGUI]
        public LookFocusScatter[] PrepareLookFocusScatters = null;

        static GlobalSetting ms_Instance = null;
        public static GlobalSetting Instance
        {
            get 
            {
#if UNITY_EDITOR
                if(ms_Instance == null)
                {
                    ms_Instance = UnityEditor.AssetDatabase.LoadAssetAtPath<GlobalSetting>("Assets/Datas/Config/GlobalSetting.asset");
                }
#endif
                return ms_Instance; 
            }
        }
        private void OnEnable()
        {
            ms_Instance = this;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_Instance = null;
        }
        //------------------------------------------------------
        public static float fLowerBattleSpeed
        {
            get
            {
                if (Instance == null) return 1;
                return ms_Instance.lowerBattleSpeed;
            }
        }
        //------------------------------------------------------
        public static float fFrameFpsScale
        {
            get
            {
                if (Instance == null) return 1.5f;
                return ms_Instance.frameFpsScale;
            }
        }
        //------------------------------------------------------
        public static float fRVOTimeHorizon
        {
            get
            {
                if (Instance == null) return 5;
                return ms_Instance.RVOTimeHorizon;
            }
        }
        //------------------------------------------------------
        public static float fRVOTimeHorizonObst
        {
            get
            {
                if (Instance == null) return 5;
                return ms_Instance.RVOTimeHorizonObst;
            }
        }
        //------------------------------------------------------
        public static float DropSpeed
        {
            get
            {
                if (Instance == null) return 5.0f;
                return ms_Instance.fDropSpeed;
            }
        }
        //------------------------------------------------------
        public static LookFocusScatter GetPrepareLookFocusScatter(int mode)
        {
            if (Instance == null || ms_Instance.PrepareLookFocusScatters == null) return LookFocusScatter.DEFAULT;
            for(int i=0; i < ms_Instance.PrepareLookFocusScatters.Length; ++i)
            {
                if (ms_Instance.PrepareLookFocusScatters[i].lableID == mode) return ms_Instance.PrepareLookFocusScatters[i];
            }
            return LookFocusScatter.DEFAULT;
        }
        //------------------------------------------------------
        public static LookFocusScatter GetLookFocusScatter(int mode)
        {
            if (Instance == null || ms_Instance.LookFocusScatters == null) return LookFocusScatter.DEFAULT;
            for (int i = 0; i < ms_Instance.LookFocusScatters.Length; ++i)
            {
                if (ms_Instance.LookFocusScatters[i].lableID == mode) return ms_Instance.LookFocusScatters[i];
            }
            return LookFocusScatter.DEFAULT;
        }
#if !USE_SERVER
        //------------------------------------------------------
        public static Core.LODRank[] LODS
        {
            get
            {
                if (Instance == null) return null;
                return ms_Instance.Lods;
            }
        }
#endif
    }
}
