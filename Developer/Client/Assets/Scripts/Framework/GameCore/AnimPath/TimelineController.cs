/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	TimelineController
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Framework.Core;
using Framework.UI;
using TopGame.UI;

namespace TopGame.Core
{
    public class TimelineController : AInstanceAble
    {
        public bool autoDestroy = false;
        public Camera mainCamera;
        public bool syncGameCamera = true;
        public Transform follow;
        public Transform[] slots;

        public bool holdEndCheck = false;
        public bool overUseCamera = false;
        public float lerpToGameCamera = 0;
        public ECameraType trackCameraType = ECameraType.Effect;
        public bool urpCameraTrack = true;
        public bool useRenderCurve = false;
        public PlayableDirector playableDirector;

        public Cinemachine.CinemachineBrain cinemachine;

        public bool gameGlobalVolumeActive = false;
        public UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset;
        public UnityEngine.Rendering.VolumeProfile volumeProfile;

        public UserActionData userActionData;

        protected List<PlayableBindSlot> m_TackSlots = null;

        private Framework.Core.VariablePoolAble m_CallbackVariable = null;
        protected System.Action<bool, Framework.Core.VariablePoolAble> m_Callback = null;
        private bool m_bDealCameraActive = true;
        private bool m_bSyncCameraStatus = false;
        private bool m_bHoldModePlayEnd = false;
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ClearGameCamera();
        }
        //------------------------------------------------------
        protected override void Awake()
        {
            base.Awake();
            m_bSyncCameraStatus = false;
            if (playableDirector)
            {
                playableDirector.stopped += OnStopTimeline;
                playableDirector.played += OnPlayTimeline;
            }
        }
        //------------------------------------------------------
        protected override void OnEnable()
        {
            CameraKit.OnCameraEnable += OnCameraEnable;
            if(playableDirector !=null && playableDirector.playOnAwake)
                SyncGameCamera();
            base.OnEnable();
        }
        //------------------------------------------------------
        protected override void OnDisable()
        {
            CameraKit.OnCameraEnable -= OnCameraEnable;
            ClearGameCamera();
            base.OnDisable();
        }
        //------------------------------------------------------
        protected override void LateUpdate()
        {
            base.LateUpdate();
            if(!m_bHoldModePlayEnd)
            {
                if (holdEndCheck && playableDirector != null && playableDirector.extrapolationMode == DirectorWrapMode.Hold)
                {
                    if (playableDirector.time + 0.1f >= playableDirector.duration)
                    {
                        m_bHoldModePlayEnd = true;
                        ClearGameCamera();
                    }
                }
            }
            if (m_TackSlots != null && playableDirector && playableDirector.time>0 && playableDirector.time < playableDirector.duration-0.1f)
            {
                PlayableBindSlot bindSlot;
                for (int i = 0; i < m_TackSlots.Count; ++i)
                {
                    bindSlot = m_TackSlots[i];
                    if (!bindSlot.bGenericBinding && bindSlot.pAble && bindSlot.pSlot)
                    {
                        bindSlot.pAble.SetPosition(bindSlot.pSlot.position);
                        bindSlot.pAble.SetEulerAngle(bindSlot.pSlot.eulerAngles);
                        bindSlot.pAble.SetScale(bindSlot.pSlot.lossyScale);
                        if(bindSlot.pUserAT !=null)
                        {
                            if(bindSlot.pUserAT is AWorldNode)
                            {
                                AWorldNode worldNode = bindSlot.pUserAT as AWorldNode;
                                worldNode.SetPosition(bindSlot.pSlot.position);
                                worldNode.SetEulerAngle(bindSlot.pSlot.eulerAngles);
                                worldNode.SetScale(bindSlot.pSlot.lossyScale);
                            }
                            Framework.Core.Actor monster = bindSlot.pUserAT as Framework.Core.Actor;
                            if (monster != null && monster.GetActorType() == EActorType.Monster)
                                monster.SetTerrainType(ETerrainHeightType.TerrainH);
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        void OnCameraEnable(bool bEnable)
        {
            if (bEnable) m_bDealCameraActive = false;
        }
        //------------------------------------------------------
        public override void OnPoolStart()
        {
            base.OnPoolStart();
            m_bHoldModePlayEnd = false;
            if (playableDirector && playableDirector.playOnAwake)
            {
                SyncGameCamera();
                playableDirector.Play();
            }
        }
        //------------------------------------------------------
        public override void OnRecyle()
        {
            m_bHoldModePlayEnd = false;
            RestoreTracks();
            ClearGameCamera();
            base.OnRecyle();
        }
        //------------------------------------------------------
        public bool Play(System.Action<bool,Framework.Core.VariablePoolAble> OnCallback = null, Framework.Core.VariablePoolAble useData = null)
        {
            if (playableDirector)
            {
                if (OnCallback != null)
                {
                    m_CallbackVariable = useData;
                    m_Callback += OnCallback;
                }
                playableDirector.Play();
                m_bHoldModePlayEnd = false;
                return true;
            }
            return false;
        }
        //------------------------------------------------------
        public void Stop()
        {
            if (playableDirector)
            {
                playableDirector.Stop();
                m_bHoldModePlayEnd = true;
                ClearGameCamera();
            }
            m_CallbackVariable = null;
            m_Callback = null;
        }
        //------------------------------------------------------
        void OnPlayTimeline(PlayableDirector playAble)
        {
            if (playableDirector == playAble)
            {
                if (m_Callback != null) m_Callback(true, m_CallbackVariable);
                SyncGameCamera();
                m_bHoldModePlayEnd = playableDirector.time+0.1f >= playableDirector.duration;
                if (userActionData.IsValid)
                {
                    AUserActionManager.AddRecordAction(userActionData);
                }
            }
        }
        //------------------------------------------------------
        void OnStopTimeline(PlayableDirector playAble)
        {
            if(Framework.Module.ModuleManager.mainModule!=null)
            {
                GameFramework pMoudle = Framework.Module.ModuleManager.mainModule as GameFramework;
                if(pMoudle!=null) pMoudle.OnTimelineStop(this);
            }
            if (playableDirector == playAble)
            {
#if UNITY_EDITOR
                if(autoDestroy)
                {
                    if(Framework.Module.ModuleManager.startUpGame)
                        RecyleDestroy(1);
                }
#else
                if(autoDestroy)
                {
                    RecyleDestroy(1);
                }
#endif
                    AUserActionManager.AddRecordAction(userActionData);
            }
            if (m_Callback != null)
                m_Callback(false, m_CallbackVariable);
            m_Callback = null;
            m_CallbackVariable = null;
        }
        //------------------------------------------------------
        void SyncGameCamera()
        {
            if (m_bSyncCameraStatus) return;
            m_bSyncCameraStatus = true;
            UseProfile();
            m_bDealCameraActive = true;
            if (syncGameCamera)
            {
                if (mainCamera)
                {
                    CatchMainCameraUtil.MainCamera = mainCamera;
                    CatchMainCameraUtil.MainTransform = mainCamera.transform;
                }
            }
            if(mainCamera)
            {
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData cameraDataStack = mainCamera.GetUniversalAdditionalCameraData();
                if (Framework.Module.ModuleManager.mainModule !=null && cameraDataStack!=null)
                {
                    if (cameraDataStack.renderType == CameraRenderType.Base)
                    {
                        if(urpCameraTrack)
                        {
                            List<Camera> vStacks = cameraDataStack.cameraStack;
                            UIBaseFramework pUI = UI.UIKits.GetUIFramework();
                            if (pUI != null && pUI.GetUICamera() != null)
                            {
                                vStacks.Add(pUI.GetUICamera());
                            }
                        }

                        CameraKit.ActiveRoot(false);
                        CameraKit.ActiveVolume(gameGlobalVolumeActive);
                    }
                    else
                        CameraKit.AddCameraStack(mainCamera, trackCameraType);
                }
            }

            Base.GlobalShaderController.EnableCurve(useRenderCurve);
        }
        //------------------------------------------------------
        void ClearGameCamera()
        {
            if (!m_bSyncCameraStatus) return;
            m_bSyncCameraStatus = false;
            RestoreProfile();

            if (syncGameCamera)
            {
                if (mainCamera)
                {
                    CatchMainCameraUtil.MainCamera = null;
                    CatchMainCameraUtil.MainTransform = null;
                }
            }
            if (mainCamera)
            {
                UnityEngine.Rendering.Universal.UniversalAdditionalCameraData cameraDataStack = mainCamera.GetUniversalAdditionalCameraData();
                if (Framework.Module.ModuleManager.mainModule != null && cameraDataStack != null)
                {
                    if (cameraDataStack.renderType == CameraRenderType.Base)
                    {
                        if(urpCameraTrack)
                        {
                            List<Camera> vStacks = cameraDataStack.cameraStack;
                            UIBaseFramework pUI = UI.UIKits.GetUIFramework();
                            if (pUI != null && pUI.GetUICamera() != null)
                            {
                                vStacks.Remove(pUI.GetUICamera());
                            }
                        }
 
                        if(m_bDealCameraActive)
                        {
                            CameraKit.ActiveRoot(true);
                            CameraKit.ActiveVolume(!gameGlobalVolumeActive);
                        }

                    }
                    else
                        CameraKit.RemoveCameraStack(mainCamera);
                }

                if(overUseCamera)
                {
                    CameraMode curCameraMode = CameraKit.GetCurrentMode();
                    if(curCameraMode!=null)
                    {
                        Transform cameraTran = mainCamera.transform;
                        Vector3 curLookAt = Framework.Core.BaseUtil.RayHitPos(cameraTran.position, cameraTran.forward, 0);
                        if (Vector3.Dot(cameraTran.forward, curLookAt - cameraTran.position) < 0)
                        {
                            float dist = Vector3.Distance(curLookAt, cameraTran.position);
                            curLookAt = cameraTran.position + (cameraTran.position - curLookAt).normalized * dist;
                        }

                        curCameraMode.Reset();
                        curCameraMode.SetLockCameraLookAtOffset(Vector3.zero);
                        curCameraMode.SetCurrentTransOffset(Vector3.zero);
                        curCameraMode.SetCurrentEulerAngle(mainCamera.transform.eulerAngles, false);
                        curCameraMode.AppendFollowDistance(Vector3.Distance(curLookAt, cameraTran.position),true, false);
                        curCameraMode.SetCurrentFov(mainCamera.fieldOfView);
                        curCameraMode.Start();
                    }
                }
                else if(mainCamera)
                {
                    if (lerpToGameCamera > 0)
                    {
                        CameraKit.SetLerpFormToMode(mainCamera.transform.position, mainCamera.transform.eulerAngles, mainCamera.fieldOfView, lerpToGameCamera);
                    }
                }

            }
            if (!useRenderCurve)
                Base.GlobalShaderController.EnableCurve(true);

            RestoreTracks();

            if (m_Callback != null)
                m_Callback(false, m_CallbackVariable);
            m_Callback = null;
            m_CallbackVariable = null;
        }
        //------------------------------------------------------
        public void UseProfile()
        {
            if (volumeProfile || urpAsset)
            {
                if (volumeProfile) CameraKit.SetPostProcess(volumeProfile);
                if (urpAsset) CameraKit.SetURPAsset(urpAsset);
            }
        }
        //------------------------------------------------------
        public void RestoreProfile()
        {
            if(volumeProfile || urpAsset)
            {
                ICameraController ctl = CameraKit.cameraController;
                if (ctl != null)
                {
                    if(!gameGlobalVolumeActive)
                        if (volumeProfile) ctl.SetPostProcess(Data.GameQuality.volumeProfile);
                    if (urpAsset) ctl.SetURPAsset(Data.GameQuality.urpAsset);
                }
            }
        }
        //------------------------------------------------------
        public Transform GetBindSlot(string slot)
        {
            if (string.IsNullOrEmpty(slot) || slot == null) return null;
            for(int i =0; i < slots.Length; ++i)
            {
                if (slots[i] && slots[i].name.CompareTo(slot) == 0) return slots[i];
            }
            return null;
        }
        //------------------------------------------------------
        void RestoreTracks()
        {
            if (m_TackSlots != null && playableDirector)
            {
                PlayableBindSlot bindSlot;
                for (int i = 0; i < m_TackSlots.Count; ++i)
                {
                    bindSlot = m_TackSlots[i];
                    bindSlot.pUserAT = null;
                    bindSlot.pAble = null;
                    m_TackSlots[i] = bindSlot;
                    if (bindSlot.binding.sourceObject == null) continue;
                    playableDirector.SetGenericBinding(bindSlot.binding.sourceObject, bindSlot.source);
                }
            }
        }
        //------------------------------------------------------
        void InitTracks()
        {
            if (playableDirector == null) return;
            if (slots == null || slots.Length <= 0) return;
            if (m_TackSlots == null)
            {
                m_TackSlots = new List<PlayableBindSlot>(slots.Length);
                for (int i = 0; i < slots.Length; ++i)
                {
                    if (slots[i] == null) continue;
                    PlayableBindSlot bindSlot = new PlayableBindSlot();
                    bindSlot.pSlot = slots[i];
                    bindSlot.strName = slots[i].name;
                    bindSlot.pAble = null;
                    bindSlot.pUserAT = null;
                    bindSlot.source = null;
                    bindSlot.bGenericBinding = false;
                    m_TackSlots.Add(bindSlot);
                }
                foreach (var bind in playableDirector.playableAsset.outputs)
                {
                    if (bind.sourceObject == null) continue;
                    UnityEngine.Object source = playableDirector.GetGenericBinding(bind.sourceObject);
                    if (source == null) continue;
                    for (int i = 0; i < m_TackSlots.Count; ++i)
                    {
                        if (m_TackSlots[i].strName.CompareTo(source.name) == 0)
                        {
                            PlayableBindSlot bindSlot = m_TackSlots[i];
                            bindSlot.bGenericBinding = false;
                            bindSlot.binding = bind;
                            bindSlot.source = source;
                            if (bind.sourceObject is Timeline.IUserTrackAsset)
                            {
                                bindSlot.playableAsset = bind.sourceObject as Timeline.IUserTrackAsset;
                                bindSlot.playableAsset.Reset(playableDirector);
                            }
                            m_TackSlots[i] = bindSlot;
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        public void BindTrackObject(string trackName, AInstanceAble pObject, Framework.Plugin.AT.IUserData pUserData, bool bGenericBinding = true)
        {
            if (playableDirector == null) return;
            InitTracks();
            if (m_TackSlots == null || m_TackSlots.Count<=0) return;
            for(int i =0; i < m_TackSlots.Count; ++i)
            {
                if (m_TackSlots[i].strName.CompareTo(trackName) == 0)
                {
                    PlayableBindSlot bindSlot = m_TackSlots[i];
                    bindSlot.pUserAT = pUserData;
                    bindSlot.pAble = pObject;
                    bindSlot.bGenericBinding = bGenericBinding;
                    if (bindSlot.playableAsset != null)
                    {
                        bindSlot.playableAsset.SetUserPointer(pObject);
                    }
                    if (bindSlot.binding.outputTargetType == typeof(Animator))
                    {
                        playableDirector.SetGenericBinding(bindSlot.binding.sourceObject, pObject.GetBehaviour<Animator>());
                    }
                    else if (bindSlot.binding.outputTargetType == typeof(GameObject))
                    {
                        playableDirector.SetGenericBinding(bindSlot.binding.sourceObject, pObject.gameObject);
                    }
                    else
                    {
                        playableDirector.SetGenericBinding(bindSlot.binding.sourceObject, pObject.gameObject);
                    }
                    if(bindSlot.pSlot)
                    {
                        pObject.SetPosition(bindSlot.pSlot.position);
                        pObject.SetScale(bindSlot.pSlot.lossyScale);
                        pObject.SetEulerAngle(bindSlot.pSlot.eulerAngles);
                        if(pUserData is AWorldNode)
                        {
                            AWorldNode node = pUserData as AWorldNode;
                            node.SetFinalPosition(bindSlot.pSlot.position);
                            node.SetEulerAngle(bindSlot.pSlot.eulerAngles);
                            node.SetScale(bindSlot.pSlot.lossyScale);
                        }
                    }
                    m_TackSlots[i] = bindSlot;
                    break;
                }
            }
        }
    }
}