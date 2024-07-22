using Framework;
using Framework.Core;
using Framework.Plugin.AI;
using Framework.Plugin.AT;
using System;
using TopGame.Base;
using TopGame.Core;
using TopGame.Data;
using TopGame.Logic;
using UnityEngine;
namespace TopGame.ED
{
    public class BattleEditorInstance : GameInstance
    {
        //------------------------------------------------------
        protected override bool OnInitResource(Framework.IFrameworkCore plusSetting)
        {
            bool bSucceed = base.OnInitResource(plusSetting);
            if(bSucceed)
            {
                m_pCameraController.RegisterCameraMode("editor", new EditorCameraMode());
            }
            return bSucceed;
        }
        //------------------------------------------------------
        protected override void OnStart(IFrameworkCore plusSetting)
        {
            base.OnStart(plusSetting);
            BattleEditorMain editorMain = plusSetting as BattleEditorMain; 

            m_pCameraController.ActiveRoot(true);
            m_pCameraController.Enable(true);
            CameraMode cameraMode = m_pCameraController.SwitchMode("editor");
            if(editorMain && cameraMode !=null)
            {
                EditorCameraMode editor = cameraMode as EditorCameraMode;
                editor.SetCurrentEulerAngle(editorMain.DefaultCameraEulerAngle);
                editor.SetFollowDistance(editorMain.DefaultCameraDistance,0,100);
                editor.Start();
                editor.SetMoveSpeed(editorMain.CameraEditorSpeed);
            }

            ASceneTheme themer = Core.SceneMgr.GetThemer();
            if (themer != null)
            {
                themer.UseTheme(1000, true, false);
                themer.EnableFog(false);
            }
        }
    }
}