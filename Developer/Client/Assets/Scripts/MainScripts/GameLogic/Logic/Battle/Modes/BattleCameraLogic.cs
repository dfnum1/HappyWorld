/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	BattleCameraLogic
作    者:	HappLI
描    述:	战斗相机逻辑
*********************************************************************/
using UnityEngine;
using TopGame.Core;
using TopGame.Data;
using Framework.Base;
using Framework.Logic;
using Framework.Core;
using Framework.BattlePlus;

namespace TopGame.Logic
{
    public abstract class BattleCameraLogic : AModeLogic
    {
        struct EventCameraParam
        {
            public int level;
            public Vector3 lookatOffset;
            public Vector3 positionOffset;
            public Vector3 eulerAngle;
            public float followDistance;
            public float fov;
            public float fLerp;
            public void Clear()
            {
                level = -1;
            }
            //------------------------------------------------------
            public bool IsValid()
            {
                return level >= 0;
            }
        }
        public enum EStateType
        {
            [Framework.Plugin.PluginDisplay("备战状态")] PrepareBattle =0,
            [Framework.Plugin.PluginDisplay("编队状态")] PrepareTeam,
            [Framework.Plugin.PluginDisplay("开始战斗状态")] StartBattle,
            [Framework.Plugin.PluginDisplay("进入战斗状态")] InBattle,
            [Framework.Plugin.DisableGUI]Count,
        }
        protected  Vector3 m_LookAtPosition = Vector3.zero;

        protected Vector4 m_CameraFollowSpeed = Vector4.one;
        protected Core.BattleCameraMode m_BattleCameraMode = null;

        protected EStateType m_CameraState = EStateType.Count;
        protected ushort m_nSceneCamera = 0xffff;
        protected ushort[] m_arrOverrideCameraID = new ushort[(int)EStateType.Count];

        private float m_fUpdateViewDelta = 0;
        private int m_bUpdateView = 0;
        EventCameraParam m_EventCameraParam = new EventCameraParam() { level = -1 };
        //------------------------------------------------------
        protected abstract Vector3 GetPosition();
        //------------------------------------------------------
        protected override void OnAwake()
        {
            System.Array.Clear(m_arrOverrideCameraID, 0, m_arrOverrideCameraID.Length);
            m_nSceneCamera = 0xffff;

            ICameraController ctl = CameraController.getInstance();
            m_BattleCameraMode = ctl.SwitchMode("battle") as BattleCameraMode;
            m_BattleCameraMode.EnableTween(true);
            m_EventCameraParam.Clear();
        }
        //------------------------------------------------------
        protected override void OnPreStart()
        {
            m_bUpdateView = 0;
            m_fUpdateViewDelta = 0;
            m_EventCameraParam.Clear();
            m_CameraFollowSpeed = Vector4.one;
            PrepareBattleCamera(true, true, false);
        }
        //------------------------------------------------------
        protected override void OnStart()
        {
            CameraKit.Enable(true);
            m_bUpdateView = 0;
            m_fUpdateViewDelta = 0;
            m_EventCameraParam.Clear();
            if(m_CameraState < EStateType.PrepareTeam || m_CameraState == EStateType.Count)
                PrepareBattleCamera(true, true, false);
        }
        //------------------------------------------------------
        public void ClearOverrideCameras()
        {
            System.Array.Clear(m_arrOverrideCameraID, 0, m_arrOverrideCameraID.Length);
            m_nSceneCamera = 0xffff;
        }
        //------------------------------------------------------
        protected override void OnClear()
        {
            m_CameraState = EStateType.Count;
            m_EventCameraParam.Clear();
            ClearOverrideCameras();
            m_nSceneCamera = 0xffff;
            if (m_BattleCameraMode != null) m_BattleCameraMode.ResetLockOffsets();
            m_BattleCameraMode = null;
            m_LookAtPosition = Vector3.zero;
            m_bUpdateView = 0;
            m_fUpdateViewDelta = 0;
        }
        //------------------------------------------------------
        public void ChangeCamera(ushort sceneCamera, bool bPrepare = true, bool bFocusScatter = true, bool bTween = true)
        {
            if (m_nSceneCamera == sceneCamera) return;
            Data.CsvData_Scene.SceneData scene = Data.DataManager.getInstance().Scene.GetData(sceneCamera);
            if (scene == null) return;
            ApplayCamera(sceneCamera, bPrepare, bFocusScatter, bTween);
        }
        //------------------------------------------------------
        public void ApplayEventCamera(int level, Vector3 lookatOffset, Vector3 positionOffset, Vector3 eulerAngle, float followDistance, float fov, float fLerp)
        {
            if (m_EventCameraParam.level > level || level<0) return;
            m_EventCameraParam.level = level;
            m_EventCameraParam.lookatOffset = lookatOffset;
            m_EventCameraParam.positionOffset = positionOffset;
            m_EventCameraParam.eulerAngle = eulerAngle;
            m_EventCameraParam.followDistance = followDistance;
            m_EventCameraParam.fov = fov;
            m_EventCameraParam.fLerp = fLerp;
            if(m_EventCameraParam.IsValid())
            {
                m_BattleCameraMode.ResetLockOffsets();
                m_BattleCameraMode.SetCurrentLookAtOffset(lookatOffset, false, fLerp);
                m_BattleCameraMode.SetCurrentEulerAngle(eulerAngle, false, fLerp);
                m_BattleCameraMode.SetCurrentTransOffset(positionOffset, false, fLerp);
                m_BattleCameraMode.SetCurrentFov(fov, false, fLerp);
                m_BattleCameraMode.AppendFollowDistance(followDistance, true, false, fLerp);
                if (fLerp <= 0)
                {
                    m_BattleCameraMode.Start();
                    m_BattleCameraMode.Blance();
                    CameraController.getInstance().ForceUpdate(0);
                }
            }
        }
        //------------------------------------------------------
        public void ApplayCamera(ushort sceneCamera, bool bPrepare = true, bool bFocusScatter = true, bool bTween = true)
        {
            if (m_nSceneCamera == sceneCamera) return;
#if UNITY_EDITOR
            Debug.Log("当前相机:" + sceneCamera);
#endif
            m_nSceneCamera = sceneCamera;
            m_BattleCameraMode = CameraController.getInstance().SwitchMode("battle") as BattleCameraMode;

            if (m_BattleCameraMode == null) return;
            if (!m_EventCameraParam.IsValid())
            {
                if (!bTween) m_BattleCameraMode.Reset();
            }
            if (GetBattleWorld() == null || !GetBattleWorld().IsStarting())
            {
                m_LookAtPosition = GetPosition();
                m_BattleCameraMode.SetFollowLookAtPosition(m_LookAtPosition);
            }
            m_BattleCameraMode.ClearAllLockZoomFlags();
            if(!m_EventCameraParam.IsValid())
            {
                if (Data.DataManager.getInstance() == null) return;
                Data.CsvData_Scene.SceneData scene = Data.DataManager.getInstance().Scene.GetData(sceneCamera);
                if (scene != null)
                {
                    m_BattleCameraMode.SetFollowLimit(scene.FollowMinDistance, scene.FollowMaxDistance);
                    m_BattleCameraMode.SetFollowSpeed(scene.FollowSpeed);
                    float fLerp = bTween ? scene.fLerp : 0;

                    m_BattleCameraMode.SetCurrentLookAtOffset(scene.LookAtOffset, false, fLerp);
                    m_BattleCameraMode.SetCurrentEulerAngle(scene.EulerAngles, false, fLerp);
                    m_BattleCameraMode.SetCurrentTransOffset(scene.PositionOffset, false, fLerp);
                    m_BattleCameraMode.SetCurrentFov(scene.FOV, false, fLerp);
                    m_BattleCameraMode.AppendFollowDistance(scene.FollowDistance, true, false, fLerp);
                    if (fLerp <= 0)
                    {
                        m_BattleCameraMode.Start();
                        m_BattleCameraMode.Blance();
                        CameraController.getInstance().ForceUpdate(0);
                    }
                }
            }
            
            m_CameraFollowSpeed = m_BattleCameraMode.GetFollowSpeed();
            if (bFocusScatter)
            {
                Data.LookFocusScatter focusScatter;
                if (bPrepare)
                {
                    focusScatter = Data.GlobalSetting.GetPrepareLookFocusScatter((int)m_pAMode.GetMode());
                }
                else
                {
                    focusScatter = Data.GlobalSetting.GetLookFocusScatter((int)m_pAMode.GetMode());
                }
                m_BattleCameraMode.SetLookFocusScatter(focusScatter.LookFocusScatterParam, focusScatter.LookFocusScatterIntensity, focusScatter.LookFocusScatterFrequency);
            }
            else
            {
                m_BattleCameraMode.SetLookFocusScatter(Vector3.zero, 0, 0);
            }
            m_BattleCameraMode.ReStartFocusScatter();
        }
        //------------------------------------------------------
        public void SetOverrideCameraID(EStateType state, ushort cameraID)
        {
            if (state >= EStateType.Count) return;
            m_arrOverrideCameraID[(int)state] = cameraID;
        }
        //------------------------------------------------------
        public virtual ushort GetOverrideCameraID(EStateType state, ushort defaultID = 0)
        {
            if (state >= EStateType.Count) return defaultID;
            if (m_arrOverrideCameraID[(int)state] != 0) return m_arrOverrideCameraID[(int)state];
            return defaultID;
        }
        //------------------------------------------------------
        public void PrepareTeamCamera(bool bPrepare = true, bool bFocusScatter = true, bool bTween = true)
        {
            m_CameraState = EStateType.PrepareTeam;
            AudioManager.SetBGParameter(1);
            ApplayCamera(GetOverrideCameraID(EStateType.PrepareTeam, 7), bPrepare, bFocusScatter, bTween);
        }
        //------------------------------------------------------
        public void PrepareBattleCamera(bool bPrepare = true, bool bFocusScatter = true, bool bTween = true)
        {
            m_CameraState = EStateType.PrepareBattle;
            AudioManager.SetBGParameter(1);
            ApplayCamera(GetOverrideCameraID(EStateType.PrepareBattle, 6), bPrepare, bFocusScatter, bTween);
        }
        //------------------------------------------------------
        public void StartBattleCamera(bool bPrepare = true, bool bFocusScatter = true, bool bTween = true)
        {
            m_CameraState = EStateType.StartBattle;
            AudioManager.SetBGParameter(1);
            ApplayCamera(GetOverrideCameraID(EStateType.StartBattle, 3), bPrepare, bFocusScatter, bTween);
        }
        //------------------------------------------------------
        public void StartInBattleCamera(bool bPrepare = true, bool bFocusScatter = true, bool bTween = true)
        {
            m_CameraState = EStateType.InBattle;
            if(BattleKits.FindRegionBoss(GetFramework()) !=null) AudioManager.SetBGParameter(3);
            else AudioManager.SetBGParameter(4);
            ApplayCamera(GetOverrideCameraID(EStateType.InBattle, 300), bPrepare, bFocusScatter, bTween);
        }
        //------------------------------------------------------
        public override void OnBattleWorldCallback(BattleWorld pWorld, EBattleWorldCallbackType type, VariablePoolAble takeData = null)
        {
            switch(type)
            {
                case EBattleWorldCallbackType.StartBattle:
                    {
                        StartBattleCamera(false, false);
                    }
                    break;
                case EBattleWorldCallbackType.StartRegion:
                    {
                        bool bHasBoss = false;
                        if (takeData != null && takeData is Variable1)
                        {
                            bHasBoss = ((Variable1)takeData).boolVal;
                        }
                        BattleCameraLogic battleCamera = GetModeLogic<BattleCameraLogic>();
                        if (battleCamera != null)
                        {
                            if (bHasBoss) battleCamera.SetOverrideCameraID(BattleCameraLogic.EStateType.InBattle, 301);
                            else battleCamera.SetOverrideCameraID(BattleCameraLogic.EStateType.InBattle, 300);
                        }
                    }
                    break;
                case EBattleWorldCallbackType.ActiveBattle:
                    {
                        StartInBattleCamera(false, false, true);
                    }
                    break;
                case EBattleWorldCallbackType.UnActiveBattle:
                    {
                        StartBattleCamera(false, false, true);
                    }
                    break;
            }
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            if(CanCameraAutoView())
            UpdateCameraView(Time.fixedDeltaTime);
        }
        //------------------------------------------------------
        protected virtual bool CanCameraAutoView()
        {
            return true;
        }
        //------------------------------------------------------
        protected virtual bool CheckActorView(AWorldNode pNode)
        {
            return true;
        }
        //------------------------------------------------------
        protected void UpdateCameraView(float fFrame)
        {
            if (m_BattleCameraMode == null) return;
            if (GetBattleWorld() == null || !GetBattleWorld().IsStartingAndActive())
                return;
            if (m_fUpdateViewDelta > 0)
            {
                m_fUpdateViewDelta -= fFrame;
                return;
            }
            m_fUpdateViewDelta = 0.5f;
            bool bHasFind = false;
            Vector3 minPos = Vector3.one*float.MaxValue;
            Vector3 maxPos = Vector3.one * float.MinValue;
            var node = GetWorld().GetRootNode();
            while(node != null)
            {
                if(node!=null && CheckActorView(node))
                {
                    bHasFind = true;
                    minPos = Vector3.Min(node.GetPosition(), minPos);
                    maxPos = Vector3.Max(node.GetPosition(), maxPos);
                }
                node = node.GetNext();
            }
            if (!bHasFind)
                return;

            int view = 0;
            minPos.y = maxPos.y = 0;
            if (CameraKit.IsInView(minPos,0) && CameraKit.IsInView(maxPos,0))
            {
                view = 1;
            }
            else
            {
                view = 2;
            }
            if(view == m_bUpdateView)
            {
                if(view == 1)
                {
                    if (CameraKit.IsInView(minPos, -0.1f) && CameraKit.IsInView(maxPos, -0.1f))
                    {
                        return;
                    }
                    else
                    {
                        view = 2;
                    }
                }
            }

            m_bUpdateView = view;
            if (view == 1)
                m_BattleCameraMode.AppendFollowDistance(-2, false, true, 1);
            else
                m_BattleCameraMode.AppendFollowDistance(2, false, true, 1);
        }
        //------------------------------------------------------
        public ushort GetSceneCameraID()
        {
            return m_nSceneCamera;
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            m_nSceneCamera = 0xffff;
            Clear();
        }
    }
}

