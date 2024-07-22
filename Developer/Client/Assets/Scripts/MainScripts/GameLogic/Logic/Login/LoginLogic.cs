/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	LoginLogic
作    者:	HappLI
描    述:	登录状态逻辑
*********************************************************************/

using Framework.Core;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;

namespace TopGame.Logic
{
    [StateLogic(EGameState.Login)]
    public class LoginLogic : AStateLogic
    {
        List<AInstanceAble> m_vScenes = new List<AInstanceAble>(1);
        //------------------------------------------------------
        protected override void OnPreStart()
        {
            TimelineController timelineController = null;
            Transform tranform = DyncmicTransformCollects.FindTransformByName("LoginVirtualScene");
            if(tranform)
            {
                LoginVirtualScene virtualScene = tranform.GetComponent<LoginVirtualScene>();
                timelineController = virtualScene?.timelineController;
                if (virtualScene!=null && virtualScene.themes.Count>0)
                {
                    int index = Data.LocalAccountCatch.GetLoginSceneIndex();
                    int preIndex = index;
                    if(GameInstance.getInstance().GetPreState() < EGameState.Login  || GameInstance.getInstance().GetPreState() == EGameState.Count)
                        index++;
                    
                    if (index < 0 || index >= virtualScene.themes.Count)
                        index = 0;
                    if(preIndex != index)
                        Data.LocalAccountCatch.SetLoginSceneIndex(index);

                    LoginVirtualScene.Theme theme = virtualScene.themes[index];
                    if(theme.scenes!=null && theme.scenes.Count>0)
                    {
                        for(int i = 0; i < theme.scenes.Count; ++i)
                        {
                            var data = theme.scenes[i];
                            InstanceOperiaon callback = FileSystemUtil.SpawnInstance(data.strFile);
                            if(callback!=null)
                            {
                                callback.OnCallback = OnSpawnScene;
                                callback.OnSign = OnSpawnSceneSign;
                                callback.userData0 = new Variable3() { floatVal0 = data.position.x, floatVal1 = data.position.y, floatVal2 = data.position.z };
                                callback.userData1 = new Variable3() { floatVal0 = data.rotation.x, floatVal1 = data.rotation.y, floatVal2 = data.rotation.z };
                            }
                        }
                    }
                    ASceneTheme themer = Core.SceneMgr.GetThemer();
                    if (themer != null)
                        themer.UseTheme(theme.theme, true, false);
                }
            }
            if(timelineController == null)
                ApplayCamera(m_pState.GetBindSceneID());
            else
            {
                ICameraController cameraCtl = CameraController.getInstance();
                cameraCtl.Enable(false);
            }
        }
        //------------------------------------------------------
        void OnSpawnScene(InstanceOperiaon callback)
        {
            if (callback.pPoolAble == null) return;
            callback.pPoolAble.SetPosition(((Variable3)callback.userData0).ToVector3() );
            callback.pPoolAble.SetEulerAngle(((Variable3)callback.userData1).ToVector3());
            callback.pPoolAble.SetScale(Vector3.one);
            m_vScenes.Add(callback.pPoolAble);
        }
        //------------------------------------------------------
        void OnSpawnSceneSign(InstanceOperiaon callback)
        {
            callback.bUsed = m_pState.IsEnable();
        }
        //------------------------------------------------------
        public void ApplayCamera(ushort sceneCamera, bool bTween = true)
        {
            ICameraController cameraCtl = CameraController.getInstance();
            cameraCtl.Enable(true);
            FreeCameraMode cameraMode = cameraCtl.SwitchMode("free") as FreeCameraMode;

            if (cameraMode == null) return;
            cameraMode.Reset();
            cameraMode.SetCurrentEulerAngle(new Vector3(15.5f, 2.5f,0), false, 0);
            cameraMode.SetCurrentTransOffset(new Vector3(-1.0f,16.9f, -9.2f), false, 0);
            cameraCtl.ForceUpdate(0);
            Data.CsvData_Scene.SceneData scene = Data.DataManager.getInstance().Scene.GetData(sceneCamera);
            if (scene != null)
            {
                float fLerp = bTween ? scene.fLerp : 0;

                cameraMode.SetCurrentLookAtOffset(scene.LookAtOffset, false, fLerp);
                cameraMode.SetCurrentEulerAngle(scene.EulerAngles, false, fLerp);
                cameraMode.SetCurrentTransOffset(scene.PositionOffset, false, fLerp);
                cameraMode.SetCurrentFov(scene.FOV, false, fLerp);
                cameraMode.AppendFollowDistance(scene.FollowDistance, true, false, fLerp);
                if (fLerp <= 0)
                {
                    cameraMode.Start();
                    cameraMode.Blance();
                    CameraController.getInstance().ForceUpdate(0);
                }
            }
            cameraMode.SetLookFocusScatter(Vector3.zero, 0,0);
            cameraMode.ReStartFocusScatter();
        }
        //------------------------------------------------------
        protected override void OnClear()
        {
            for(int i = 0; i < m_vScenes.Count; ++i)
            {
                m_vScenes[i].RecyleDestroy(1);
            }
            m_vScenes.Clear();
        }
    }
}

