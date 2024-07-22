/********************************************************************
生成日期:	19:11:2021   14:4
类    名: 	UIPreviewActorHudRTLogic
作    者:	happli
描    述:	3d模型显示
*********************************************************************/
using System.Collections.Generic;
using Framework.Core;
using TopGame.Data;
using Unity.Animations.SpringBones;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    public class UIPreviewActorHudRTLogic : UILogic
    {
        struct ModelHudData
        {
            public Core.HudCamera m_pRTCamera;

            public string m_PlayAciton;
            public Vector3 m_vModelScale;
            public Vector3 m_vModelPos;
            public Vector3 m_vModelEulerAngle;
            public Framework.Core.Actor m_pActor;

            public string m_strRtImage;
            public HUDImage m_SyncRT;

            public AInstanceAble m_pShadow;

            public void Clear()
            {
                if (m_pRTCamera) Core.RenderHudManager.getInstance().Destroy(m_pRTCamera);
                m_pRTCamera = null;
                if (m_pActor != null) m_pActor.Destroy();
                m_pActor = null;
                m_strRtImage = null;
                m_PlayAciton = null;
                m_vModelScale = Vector3.one;
                m_vModelPos = Vector3.zero;
                m_vModelEulerAngle = Vector3.zero;
                m_SyncRT = null;
                if (m_pShadow != null) m_pShadow.Destroy();
                m_pShadow = null;
            }
        };

        List<ModelHudData> m_vDatas = new List<ModelHudData>();

        UIBase m_pBase;
        public override void Awake(UIBase pBase)
        {
            base.Awake(pBase);
            m_pBase = pBase;
        }
        //-------------------------------------------
        public override void OnDestroy()
        {
            base.OnDestroy();
            m_pBase = null;
        }
        //-------------------------------------------
        public override void OnHide()
        {
            base.OnHide();
            Clear();
        }
        //-------------------------------------------
        public override void OnShow()
        {
            base.OnShow();
            //! if show before set hud param,check set agin
            // if(m_pBase !=null && !string.IsNullOrEmpty(m_strRtImage))
            // {
            //     if(m_pRTCamera ==null)
            //         SetHudParam(m_strModelFile, m_strRtImage);
            //     m_strRtImage = null;
            // }
        }
        //-------------------------------------------
        void Clear()
        {
            for (int i = 0; i < m_vDatas.Count; i++)
            {
                m_vDatas[i].Clear();
            }
            m_vDatas.Clear();
        }
        //------------------------------------------------------
        int GetIndex(string rtImage)
        {
            int index = -1;
            for (int i = 0; i < m_vDatas.Count; i++)
            {
                if (m_vDatas[i].m_strRtImage.CompareTo(rtImage) == 0)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }
        //-------------------------------------------
        public bool SetHudParam(SvrData.ISvrHero svrData, string rtImage, bool bAsync= false, EActionStateType playAction = EActionStateType.None)
        {
            if (svrData == null) return false;
            if (Framework.Module.ModuleManager.mainModule == null)
                return false;
            AFrameworkModule frameworkModule = Framework.Module.ModuleManager.mainModule as AFrameworkModule;
            if (frameworkModule == null) return false;
            int index = GetIndex(rtImage);
            ModelHudData data;
            if (index < 0)
            {
                data = new ModelHudData();
                m_vDatas.Add(data);
                index = m_vDatas.Count - 1;
            }
            else
            {
                data = m_vDatas[index];
            }

            if (m_pBase == null)
            {
                m_vDatas.RemoveAt(index);
                return false;
            }
            if (svrData.GetContextData() == null)
            {
                data.Clear();
                m_vDatas.RemoveAt(index);
                return false;
            }
            if(data.m_pActor!=null && data.m_pActor.GetContextData() == svrData.GetContextData())
            {
                return true;
            }

            data.Clear();
            HUDImage hudRT = m_pBase.GetWidget<HUDImage>(rtImage);
            if (hudRT == null)
            {
                m_vDatas.RemoveAt(index);
                return false;
            }
#if UNITY_EDITOR
            data.m_SyncRT = hudRT;
#endif
            data.m_strRtImage = rtImage;
            data.m_vModelScale = hudRT.ModelScale;
            data.m_vModelPos = hudRT.ModelPos + Vector3.right * 100 * index;
            data.m_vModelEulerAngle = hudRT.ModelRotate;
            data.m_PlayAciton = hudRT.playAction;

            if(bAsync)
                data.m_pActor = frameworkModule.world.CreateNode<Logic.PreviewActor>( EActorType.PreviewActor, svrData.GetContextData(), 0, svrData);
            else
                data.m_pActor = frameworkModule.world.SyncCreateNode<Logic.PreviewActor>(EActorType.PreviewActor, svrData.GetContextData(), 0, svrData);
            if (data.m_pActor!=null)
            {
                data.m_pRTCamera = Core.RenderHudManager.getInstance().CreateRenderHud(m_pBase, hudRT, hudRT.CameraPos + Vector3.right * 100 * index, (int)hudRT.rectTransform.sizeDelta.x, (int)hudRT.rectTransform.sizeDelta.y);
                if (data.m_pRTCamera == null) return false;
                data.m_pRTCamera.SetEulerAngle(hudRT.CameraRotate);
                data.m_pRTCamera.SetFov(hudRT.CameraFOV);


                data.m_pActor.SetRenderLayer(Core.RenderHudManager.RenderLayer);
                data.m_pActor.SetPosition(data.m_vModelPos);
                data.m_pActor.SetScale(data.m_vModelScale);
                data.m_pActor.SetEulerAngle(data.m_vModelEulerAngle);

                if (playAction != EActionStateType.None)
                    data.m_pActor.StartActionByType(playAction, 0, 1, true, false, true);
                else if (!string.IsNullOrEmpty(data.m_PlayAciton)) data.m_pActor.StartActionByName(data.m_PlayAciton, 0, 1, true);
            }
            m_vDatas[index] = data;

            if (hudRT.hasTransparentShadow)
            {
                var instanceCB = LoadInstance(PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.UIRecvPreviewTransparentShadow), ARootsHandler.ScenesRoot, true, OnShadowInstance);
                if (instanceCB != null)
                {
                    instanceCB.userData0 = new Framework.Core.Variable1() { intVal = index };
                }
            }
            return true;
        }
        //-------------------------------------------
        public bool SetHudParam(IContextData pConfigData, string rtImage, bool bAsync = false, EActionStateType playAction = EActionStateType.None)
        {
            if (pConfigData == null) return false;
            if (Framework.Module.ModuleManager.mainModule == null)
                return false;
            AFrameworkModule frameworkModule = Framework.Module.ModuleManager.mainModule as AFrameworkModule;
            if (frameworkModule == null) return false;
            int index = GetIndex(rtImage);
            for (int i = 0; i < m_vDatas.Count; i++)
            {
                if (m_vDatas[i].m_strRtImage.CompareTo(rtImage) == 0)
                {
                    index = i;
                    break;
                }
            }
            ModelHudData data;
            if (index < 0)
            {
                data = new ModelHudData();
                m_vDatas.Add(data);
                index = m_vDatas.Count - 1;
            }
            else
            {
                data = m_vDatas[index];
            }

            if (m_pBase == null)
            {
                m_vDatas.RemoveAt(index);
                return false;
            }
            if (pConfigData == null)
            {
                data.Clear();
                m_vDatas.RemoveAt(index);
                return false;
            }

            

            if (data.m_pActor != null)
            {
                var pOldPlayerCfg = data.m_pActor.GetContextData() as CsvData_Player.PlayerData;
                var pNewPlayerCfg = pConfigData as CsvData_Player.PlayerData;

                if (pOldPlayerCfg != null && pNewPlayerCfg != null && pOldPlayerCfg.ID == pNewPlayerCfg.ID)
                {
                    return true;
                }
            }

            data.Clear();
            HUDImage hudRT = m_pBase.GetWidget<HUDImage>(rtImage);
            if (hudRT == null)
            {
                m_vDatas.RemoveAt(index);
                return false;
            }
#if UNITY_EDITOR
            data.m_SyncRT = hudRT;
#endif
            data.m_strRtImage = rtImage;
            data.m_vModelScale = hudRT.ModelScale;
            data.m_vModelPos = hudRT.ModelPos + Vector3.right * 100 * index;
            data.m_vModelEulerAngle = hudRT.ModelRotate;
            data.m_PlayAciton = hudRT.playAction;
            if (bAsync)
                data.m_pActor = frameworkModule.world.CreateNode<Logic.PreviewActor>(EActorType.PreviewActor, pConfigData);
            else
                data.m_pActor = frameworkModule.world.SyncCreateNode<Logic.PreviewActor>(EActorType.PreviewActor, pConfigData);

            if (data.m_pActor != null)
            {
                data.m_pRTCamera = Core.RenderHudManager.getInstance().CreateRenderHud(m_pBase, hudRT, hudRT.CameraPos + Vector3.right * 100 * index, (int)hudRT.rectTransform.sizeDelta.x, (int)hudRT.rectTransform.sizeDelta.y);
                if (data.m_pRTCamera == null) return false;
                data.m_pRTCamera.SetEulerAngle(hudRT.CameraRotate);
                data.m_pRTCamera.SetFov(hudRT.CameraFOV);


                data.m_pActor.SetRenderLayer(Core.RenderHudManager.RenderLayer);
                data.m_pActor.SetPosition(data.m_vModelPos);
                data.m_pActor.SetScale(data.m_vModelScale);
                data.m_pActor.SetEulerAngle(data.m_vModelEulerAngle);

                if (playAction != EActionStateType.None)data.m_pActor.StartActionByType(playAction, 0, 1, true, false, true);
                else if (!string.IsNullOrEmpty(data.m_PlayAciton)) data.m_pActor.StartActionByName(data.m_PlayAciton, 0, 1, true);
            }
            m_vDatas[index] = data;

            if (hudRT.hasTransparentShadow)
            {
                var instanceCB = LoadInstance(PermanentAssetsUtil.GetAsset<GameObject>(EPermanentAssetType.UIRecvPreviewTransparentShadow), ARootsHandler.ScenesRoot, true, OnShadowInstance);
                if (instanceCB != null)
                {
                    instanceCB.userData0 = new Framework.Core.Variable1() { intVal = index };
                }
            }
            return true;
        }
        //------------------------------------------------------
        public void ClearHud(string rtImage)
        {
            int index = GetIndex(rtImage);

            ModelHudData data;
            if (index < 0)
            {
                return;
            }

            data = m_vDatas[index];
            if (data.m_pRTCamera == null)
            {
                return;
            }
            Core.RenderHudManager.getInstance().Destroy(data.m_pRTCamera);
            data.Clear();
            m_vDatas.Remove(data);
        }
        //-------------------------------------------
        void OnShadowInstance(Framework.Core.InstanceOperiaon instanceOp)
        {
            if (instanceOp.userData0 == null || !instanceOp.HasData<Framework.Core.Variable1>(0))
            {
                if(instanceOp.pPoolAble!=null) instanceOp.pPoolAble.RecyleDestroy();
                return;
            }
            var userVar = instanceOp.GetUserData<Framework.Core.Variable1>(0);
            if (userVar.intVal < 0 || userVar.intVal >= m_vDatas.Count)
            {
                if (instanceOp.pPoolAble != null) instanceOp.pPoolAble.RecyleDestroy();
                return;
            }
            ModelHudData hudParam = m_vDatas[userVar.intVal];
            if(hudParam.m_pShadow!= instanceOp.pPoolAble)
            {
                if(hudParam.m_pShadow!=null) hudParam.m_pShadow.RecyleDestroy();
            }
            hudParam.m_pShadow = instanceOp.pPoolAble;
            if(hudParam.m_pShadow!=null)
            {
                Vector3 pos = hudParam.m_pActor.GetFinalPosition();
                pos.y = 0;
                hudParam.m_pShadow.SetPosition(pos);
            //    hudParam.m_pShadow.SetEulerAngle(hudParam.m_pActor.GetFinalEulerAngle());
            }
            m_vDatas[userVar.intVal] = hudParam;
        }
        //-------------------------------------------
        public void PlayAction(EActionStateType stateType)
        {
            for (int i = 0; i < m_vDatas.Count; i++)
            {
                var data = m_vDatas[i];

                if (data.m_pActor != null)
                {
                    data.m_pActor.StartActionByType(stateType, 0, 1, true, false, true);
                }
            }
        }
        //-------------------------------------------
#if UNITY_EDITOR
        protected override void OnUpdate(float fFrame)
        {
            for (int i = 0; i < m_vDatas.Count; i++)
            {
                var data = m_vDatas[i];
                if (data.m_SyncRT)
                {
                    if(data.m_pRTCamera)
                    {
                        data.m_pRTCamera.SetPosition(data.m_SyncRT.CameraPos + Vector3.right * 100 * i);
                        data.m_pRTCamera.SetEulerAngle(data.m_SyncRT.CameraRotate);
                        data.m_pRTCamera.SetFov(data.m_SyncRT.CameraFOV);
                    }
                    if (data.m_pActor!=null)
                    {
                        data.m_pActor.SetPosition(data.m_SyncRT.ModelPos + Vector3.right * 100 * i);
                        data.m_pActor.SetScale(data.m_SyncRT.ModelScale);
                        data.m_pActor.SetEulerAngle(data.m_SyncRT.ModelRotate);
                        if (data.m_pShadow != null)
                        {
                            Vector3 pos = data.m_pActor.GetFinalPosition();
                            pos.y = 0;
                            data.m_pShadow.SetPosition(pos);
                        }
                    }
                }
            }
        }
#endif
    }
}
