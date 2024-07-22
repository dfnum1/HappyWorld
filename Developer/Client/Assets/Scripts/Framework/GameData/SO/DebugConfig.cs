/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	DebugConfig
作    者:	HappLI
描    述:	调试信息输出
*********************************************************************/
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using System.IO;
using Framework.Base;
#if UNITY_EDITOR
using System.Reflection;
#endif
#if USE_SERVER
using ScriptableObject = ExternEngine.ScriptableObject;
#endif
namespace TopGame.Core
{
    //   [CreateAssetMenu]
    [ExternEngine.ConfigPath("sos/DebugConfig.asset")]
    public class DebugConfig : ScriptableObject
    {
        [SerializeField]
        [Framework.Data.DisplayNameGUI("日志过滤")]
        [Framework.Data.DisplayEnumBitGUI(typeof(Framework.Plugin.ELogType), false)]
        uint m_LogLevel = 0xfffffff;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("战斗报告")]
        bool m_bBatlleReport = false;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("技能调试输出")]
        bool m_bSkillDebug = false;
        [SerializeField]
        [Framework.Data.DisplayNameGUI("AI 调试输出")]
        bool m_bAIDebug = false;
        [SerializeField]
        [Framework.Data.DisplayNameGUI("伤害计算输出")]
        bool m_bDamageDebug = false;
        [SerializeField]
        [Framework.Data.DisplayNameGUI("事件触发信息输出")]
        bool m_bEventDebug = false;
        [SerializeField]
        [Framework.Data.DisplayNameGUI("锁信息输出")]
        bool m_bLockDebug = false;
        [SerializeField]
        [Framework.Data.DisplayNameGUI("开启新手引导")]
        bool m_bEnableGuide = true;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("快速自动注册")]
        bool m_bAutoAccountRegister = false;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("新手引导Log开关")]
        bool m_bGuideLogEnable = false;


        [SerializeField]
        [Framework.Data.DisplayNameGUI("引导Guid Log开关")]
        bool m_bGuideGuidLog = false;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("是否显示主界面锁")]
        bool m_bIsShowLock = true;
        [SerializeField]
        [Framework.Data.DisplayNameGUI("是否一直显示副本动画")]
        bool m_bIsShowAdventureAniamtion = false;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("开启引导卡住跳过功能")]
        bool m_bEnableGuideSkip = false;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("记录酷跑点")]
        bool m_bRunPointRecord = false;
        [SerializeField]
        [Framework.Data.DisplayNameGUI("酷跑点记录精度")]
        [Framework.Data.StateGUIByField("m_bRunPointRecord", "true")]
        float m_fRunPointRecordPrecision = 0.95f;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("测试开关")]
        bool m_bTestController = false;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("消耗分析")]
        bool m_bProfilerTick = false;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("功能模块锁开关")]
        bool m_bEnableModuleLock = true;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("功能模块锁Log开关")]
        bool m_bEnableModuleLockLog = false;


        [SerializeField]
        [Framework.Data.DisplayNameGUI("Buff飘字开关")]
        bool m_bEnableBuffFont = false;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("是否显示空间树")]
        bool m_bShowSpatial = false;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("是否显示节点碰撞框")]
        bool m_bShowNodeDebugFrame = false;

        [SerializeField]
        [Framework.Data.DisplayNameGUI("是否显示格子地图")]
        bool m_bShowGridMap = false;

        private static DebugConfig ms_Instance = null;
        private void OnEnable()
        {
            ms_Instance = this;
        }
        private void OnDestroy()
        {
            ms_Instance = null;
        }
        public static bool AutoAccountRegister { get { if (ms_Instance) return ms_Instance.m_bAutoAccountRegister; return false; } }
        public static bool bSkillDebug { get { if (ms_Instance) return ms_Instance.m_bSkillDebug; return false; } }
        public static bool bBatlleReport { get { if (ms_Instance) return ms_Instance.m_bBatlleReport; return false; } }
        public static bool bAIDebug { get { if (ms_Instance) return ms_Instance.m_bAIDebug; return false; } }
        public static bool bDamageDebug { get { if (ms_Instance) return ms_Instance.m_bDamageDebug; return false; } }
        public static uint logLevel { get { if (ms_Instance) return ms_Instance.m_LogLevel; return 0xffffffff; } }
        public static bool eventDebug { get { if (ms_Instance) return ms_Instance.m_bEventDebug; return false; } }
        public static bool lockDebug { get { if (ms_Instance) return ms_Instance.m_bLockDebug; return false; } }
        public static bool bEnableGuide { get { if (ms_Instance) return ms_Instance.m_bEnableGuide; return true; } }
        public static bool bIsShowLock { get { if (ms_Instance) return ms_Instance.m_bIsShowLock; return true; } }
        public static bool bIsShowAdventureAnimation { get { if (ms_Instance) return ms_Instance.m_bIsShowAdventureAniamtion; return true; } }

        public static bool bEnableGuideSkip { get { if (ms_Instance) return ms_Instance.m_bEnableGuideSkip; return true; } }
        public static bool bRunPointRecord { get { if (ms_Instance) return ms_Instance.m_bRunPointRecord; return false; } }
        public static float fRunPointRecordPrecision { get { if (ms_Instance) return Mathf.Min(1, ms_Instance.m_fRunPointRecordPrecision); return 1; } }
        public static bool bTestController { get { if (ms_Instance) return ms_Instance.m_bTestController; return false; } }
        public static bool bGuideLogEnable { get { if (ms_Instance) return ms_Instance.m_bGuideLogEnable; return false; } }
        public static bool bGuideGuidLog { get { if (ms_Instance) return ms_Instance.m_bGuideGuidLog; return false; } }
        public static bool bEnableModuleLock { get { if (ms_Instance) return ms_Instance.m_bEnableModuleLock; return false; } }
        public static bool bEnableModuleLockLog { get { if (ms_Instance) return ms_Instance.m_bEnableModuleLockLog; return false; } }
        public static bool bEnableBuffFont { get { if (ms_Instance) return ms_Instance.m_bEnableBuffFont; return false; } }
        public static bool bShowSpatial { get { if (ms_Instance) return ms_Instance.m_bShowSpatial; return false; } }
        public static bool bShowNodeDebugFrame { get { if (ms_Instance) return ms_Instance.m_bShowNodeDebugFrame; return false; } }
        public static bool bShowGridMap { get { if (ms_Instance) return ms_Instance.m_bShowGridMap; return false; } }
        public static bool bProfilerTick { get { if (ms_Instance) return ms_Instance.m_bProfilerTick; return false; } }
        public static void Update()
        {
            var framework = Framework.Module.ModuleManager.GetMainFramework<Framework.Module.AFramework>();
            if (framework != null)
            {
                framework.aiSystem.EnableLog(bAIDebug);
            }
            Framework.Plugin.Logger.getInstance().SetFlag(logLevel);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(DebugConfig), true)]
    public class DebugConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DebugConfig controller = target as DebugConfig;
            UnityEditor.EditorGUI.BeginChangeCheck();
            Framework.ED.HandleUtilityWrapper.DrawProperty(controller, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly, null);
            bool bDirty = UnityEditor.EditorGUI.EndChangeCheck();
            if (GUILayout.Button("刷新"))
            {
                UnityEditor.EditorUtility.SetDirty(target);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.ForceSynchronousImport);
            }
            if (bDirty || serializedObject.ApplyModifiedProperties())
            {
                ED.IDataEditorUti.RefreshData(controller);
            }
        }
    }
#endif
}