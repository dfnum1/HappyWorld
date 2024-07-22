#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopGame.Data;
using TopGame.Logic;
using UnityEditor;
using Framework.Core;
using TopGame.Core;
using Framework.Base;

namespace TopGame.ED
{
    [DataEditor(typeof(DebugConfig))]
    public class DebugConfigEditorRefresh : IDataEditorRefresh
    {
        public void OnDityRefresh(object pData)
        {
            if (Framework.Module.ModuleManager.startUpGame)
            {
                Framework.Plugin.Guide.GuideSystem.getInstance().Enable(DebugConfig.bEnableGuide);
                Framework.Plugin.Guide.GuideSystem.getInstance().EnableSkip(DebugConfig.bEnableGuideSkip);
                Framework.Plugin.Guide.GuideSystem.getInstance().bGuideLogEnable = DebugConfig.bGuideLogEnable;
                Framework.Plugin.Guide.GuideGuidUtl.bGuideGuidLog = DebugConfig.bGuideGuidLog;
                var framework = Framework.Module.ModuleManager.GetMainFramework<AFrameworkModule>();
                if (framework != null)
                    framework.aiSystem.EnableLog(DebugConfig.bAIDebug);
                Framework.Plugin.Logger.getInstance().SetFlag(DebugConfig.logLevel);

                ConfigUtil.bDamageDebug = DebugConfig.bDamageDebug;
                ConfigUtil.bSkillDebug = DebugConfig.bSkillDebug;
                ConfigUtil.bShowSpatial = DebugConfig.bShowSpatial;
                ConfigUtil.bShowNodeDebugFrame = DebugConfig.bShowNodeDebugFrame;
                ConfigUtil.bEventTriggerDebug = DebugConfig.eventDebug;
                ConfigUtil.bDamageDebug = DebugConfig.bDamageDebug;
                ConfigUtil.bProfilerDebug = DebugConfig.bProfilerTick;
                ConfigUtil.bBattleFameWriteLogFile = DebugConfig.bBatlleReport;
                if (GameInstance.getInstance() != null && GameInstance.getInstance().unlockMgr != null)
                {
                    GameInstance.getInstance().unlockMgr.SetEnable(DebugConfig.bEnableModuleLock);
                }
            }
        }
    }
}
#endif