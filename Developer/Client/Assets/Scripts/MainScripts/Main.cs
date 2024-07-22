/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	Main
作    者:	HappLI
描    述:	入口
*********************************************************************/
using UnityEngine;
using TopGame.Data;
using Framework.Core;
using System.Collections;

namespace TopGame
{
#if USE_HYBRIDCLR
    [HideInInspector]
#endif
    public class Main : MonoBehaviour
    {
#if !USE_HYBRIDCLR
        public FrameworkMain pFrameMain;
        private void Awake()
        {
            Logic.FrameworkStartUp.OnStartupSectionChange += OnStartupSectionChange;
        }
        //------------------------------------------------------
        public void Start()
        {
            
        }
        //------------------------------------------------------
        void OnStartupSectionChange(Logic.EStartUpSection section)
        {
            if(section == Logic.EStartUpSection.AppAwake)
            {
                AppMain.Main();
            }
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            Logic.FrameworkStartUp.OnStartupSectionChange -= OnStartupSectionChange;
        }
        //------------------------------------------------------
        //private void OnGUI()
        //{
        //   // Logic.TestController.OnGUI();
        //}
#endif
    }
}