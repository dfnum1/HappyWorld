/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	StateClearUtil
作    者:	HappLI
描    述:	状态数据清理标志 
*********************************************************************/

using Framework.Core;
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Data;
using UnityEngine;

namespace TopGame.Logic
{   
    public class ClearFlagUtil
    {
        public static bool CanModeAPI(uint flag, EAPICallType api)
        {
            if ((flag & (uint)api) != 0) return true;
            return false;
        }
        //------------------------------------------------------
        public static float GetDetaChangeTime(uint flags)
        {
            if (StateChangeFlag(flags, EStateChangeFlag.DelayChange))
                return 0.7f;
            return 0;
        }
        //------------------------------------------------------
        public static bool StateChangeFlag(uint flags, EStateChangeFlag flag)
        {
            if ((flags & (uint)flag) != 0) return true;
            return false;
        }
        //------------------------------------------------------
        public static void Clear(EGameState preState, EGameState state, uint flags)
        {
            ASceneMgr sceneMgr = null;
            UI.UIManager.Free();
            if(Framework.Module.ModuleManager.mainModule!=null)
            {
                Framework.Core.AFrameworkModule framework = Framework.Module.ModuleManager.GetMainFramework<Framework.Core.AFrameworkModule>();
                if(framework!=null)
                {
                    sceneMgr = framework.sceneMgr;
                    framework.world.ClearWorld();
                    framework.globalBuff.Clear();
                }
            }

            if (GameInstance.getInstance() != null && GameInstance.getInstance().terrainManager!=null)
                GameInstance.getInstance().terrainManager.Clear();
            URP.ScreenFadeLogic.Clear();

            if(state > EGameState.Login && preState> EGameState.Login)
            {
                long remainMemory = JniPlugin.GetRemainMemory();
                if (remainMemory > 0) Debug.Log("剩余内存:" + Base.Util.FormBytes(remainMemory));
                if (Data.GameQuality.ThresholdSystemMemory > 0 && remainMemory >= Data.GameQuality.ThresholdSystemMemory)
                {
                    //FileSystemUtil.Free(600);
                    //FileSystemUtil.SetInstanceDelayDestroyParam(600);
                }
                else if (remainMemory > 0 && remainMemory < 512) //128 mb
                {
                    if (sceneMgr != null) sceneMgr.Free();
                    FileSystemUtil.Free(0);
                }
                else
                {
                    if (sceneMgr != null) sceneMgr.Free();
                    FileSystemUtil.Free(300);
                    FileSystemUtil.SetInstanceDelayDestroyParam(300);
                }
            }
            else
            {
                long remainMemory = JniPlugin.GetRemainMemory();
                if (remainMemory > 0) Debug.Log("剩余内存:" + Base.Util.FormBytes(remainMemory));
                if (remainMemory > 0 && remainMemory < 1) //1 mb
                    FileSystemUtil.Free(0);
                else
                {
                    FileSystemUtil.Free(10);
                    FileSystemUtil.SetInstanceDelayDestroyParam(60);
                }
            }


            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }
}

