/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AppMain
作    者:	HappLI
描    述:	启动入口
*********************************************************************/

using UnityEngine;
using TopGame.Data;
using Framework.Core;
using System.Collections;

namespace TopGame
{
    public class AppMain
    {
        public static bool Main()
        {
            Framework.Module.ModuleManager.getInstance().RegisterModule<GameInstance>();
            if (Framework.Module.ModuleManager.getInstance().Awake(Logic.FrameworkStartUp.GetFrameworkMain()))
            {
                Data.DataManager.OnLoaded += DoInitStartup;
                Data.DataManager.ReInit();
                return true;
            }
            return false;
        }       
        //------------------------------------------------------
        static void DoInitStartup()
        {
            if (Framework.Module.ModuleManager.getInstance().StartUp(Logic.FrameworkStartUp.GetFrameworkMain()))
            {
                Logic.FrameworkStartUp.getInstance().SetSection(Logic.EStartUpSection.AppStartUp);
            }
        }
    }

}

