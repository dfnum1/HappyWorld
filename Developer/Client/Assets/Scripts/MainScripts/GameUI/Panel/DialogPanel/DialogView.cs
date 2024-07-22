/********************************************************************
生成日期:	10:12:2021   11:27
类    名: 	DialogView
作    者:	happli
描    述:	
*********************************************************************/

using Framework.Core;
using TopGame.Base;
using Unity.Animations.SpringBones;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    [UI((ushort)EUIType.DialogPanel, UI.EUIAttr.View)]
    public class DialogView : UIView
    {
        //TopGame.UI.UISerialized	m_Player0;
        //TopGame.UI.UISerialized	m_Player1;
        //TopGame.UI.UISerialized	m_Box;

        int m_nDialogCount = 0;
        private Vector3 m_vPosition = Vector3.zero;
        private UISerialized m_PlayerSay = null;
        private UISerialized m_pPlayerHud = null;
        private Core.HudCamera m_pPlayerHudCamera = null;
        private uint m_nPlayerModel = 0;
        private AnimatorUpdateMode m_backupUpdateModel = AnimatorUpdateMode.Normal;
        private AInstanceAble m_pPlayerModel = null;
        private InstanceOperiaon m_pInstaceModelOp = null;

        UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset m_BackupUrpAsset=null;
        //------------------------------------------------------
        public override void Init(UIBase pBase)
        {
            base.Init(pBase);
        }
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            if (m_pBaseUI == null) return;
            if (m_pBaseUI.ui)
            {
				//m_Player0 = m_pBaseUI.ui.GetWidget<TopGame.UI.UISerialized>("Player0");
				//m_Player1 = m_pBaseUI.ui.GetWidget<TopGame.UI.UISerialized>("Player1");
				//m_Box = m_pBaseUI.ui.GetWidget<TopGame.UI.UISerialized>("Box");
            }
        }
        //------------------------------------------------------
        public override void Show()
        {
            base.Show();
            if(Data.GameQuality.Qulity >= Data.EGameQulity.Middle)
            {
                DialogUserInterface pUI = m_pBaseUI.ui as DialogUserInterface;
                if (pUI && pUI.urpAsset)
                {
                    Core.ICameraController ctl = Core.CameraController.getInstance();
                    if (ctl != null)
                    {
                        m_BackupUrpAsset = Data.GameQuality.GetCurrentURPAsset();
                        ctl.SetURPAsset(pUI.urpAsset);
                    }
                }
            }

        }
        //------------------------------------------------------
        public override void Hide()
        {
            base.Hide();
            if (m_BackupUrpAsset!=null)
            {
                DialogUserInterface pUI = m_pBaseUI.ui as DialogUserInterface;
                if (pUI)
                {
                    Core.ICameraController ctl = Core.CameraController.getInstance();
                    if (ctl != null)
                    {
                        ctl.SetURPAsset(m_BackupUrpAsset);
                    }
                }
            }
            m_BackupUrpAsset = null;

            m_nDialogCount = 0;
            m_vPosition = Vector3.zero;
            CreateHudModel(null, Vector3.zero, 0);
            // if (m_pPlayerHud != null) m_pPlayerHud.gameObject.SetActive(false);
            GetWidget<UISerialized>("Player0").gameObject.SetActive(false);
            GetWidget<UISerialized>("Player1").gameObject.SetActive(false);
            m_pPlayerHud = null;
            // if (m_PlayerSay != null) m_PlayerSay.gameObject.SetActive(false);
            GetWidget<UISerialized>("talk0").gameObject.SetActive(false);
            GetWidget<UISerialized>("talk1").gameObject.SetActive(false);
            m_PlayerSay = null;
        }
        //------------------------------------------------------
        protected Data.CsvData_Dialog.DialogData GetCurDialogData()
        {
            return m_pBaseUI != null ? ((DialogPanel)m_pBaseUI).GetCurDialogData():null;
        }
        //------------------------------------------------------
        public void SetPlayerSay(Data.CsvData_Dialog.DialogData dialogData)
        {
            UISerialized playerSay = null;
            if (dialogData.fSeat <= 0)
            {
                playerSay = GetWidget<UISerialized>("talk0");
            }
            else if (dialogData.fSeat >= 1)
            {
                playerSay = GetWidget<UISerialized>("talk1");
            }
            if (playerSay != null)
            {
                if (playerSay != m_PlayerSay)
                {
                    if (m_PlayerSay != null)
                    {
                        PlayTween(m_PlayerSay, false);
                    }
                }
                m_PlayerSay = playerSay;
            }
            if (m_PlayerSay != null)
            {
                PlayTween(m_PlayerSay, true);
            }
        }
        #region HUD_VIEW
        //------------------------------------------------------
        public void SetPlayerHud(Data.CsvData_Dialog.DialogData dialogData)
        {
            m_nDialogCount++;
            m_vPosition = new Vector3(100*m_nDialogCount,-100,0);
            UISerialized playerHud = null;
            if (dialogData.fSeat <= 0)
            {
                playerHud = GetWidget<UISerialized>("Player0");
            }
            else if (dialogData.fSeat >= 1)
            {
                playerHud = GetWidget<UISerialized>("Player1");
            }
            bool bChange = false;
            if (playerHud != null)
            {
                if (playerHud != m_pPlayerHud)
                {
                    bChange = true;
                    if (m_pPlayerHud != null)
                    {
                        PlayTween(m_pPlayerHud, false);
                    }
                }
                m_pPlayerHud = playerHud;
            }
            Data.CsvData_Models.ModelsData modelData = null;
            if (m_pPlayerHud != null)
            {
                if(bChange) PlayTween(m_pPlayerHud, true);
                modelData = dialogData.Models_nModel_data;
            }
            else
            {
                m_nPlayerModel = 0;
            }
            CreateHudModel(dialogData, m_vPosition);
        }
        //------------------------------------------------------
        void CreateHudModel(Data.CsvData_Dialog.DialogData dialogData, Vector3 cameraPos,float fDelay= 0.5f)
        {
            Data.CsvData_Models.ModelsData modelData = dialogData!=null?dialogData.Models_nModel_data:null;
            if (modelData != null && Core.RenderHudManager.IsHUDLowerModel()) modelData = Data.DataManager.getInstance().Models.GetData(modelData.ID-1000);
            if (modelData == null || m_nPlayerModel != modelData.ID)
            {
                if (m_pPlayerHudCamera != null)
                    Core.RenderHudManager.getInstance().Destroy(m_pPlayerHudCamera, fDelay);
                m_pPlayerHudCamera = null;

                if (m_pInstaceModelOp != null) m_pInstaceModelOp.Earse();
                m_pInstaceModelOp = null;
                if (m_pPlayerModel != null)
                {
                    Animator animator = m_pPlayerModel.GetBehaviour<Animator>();
                    if (animator) animator.updateMode = m_backupUpdateModel;
                    m_pPlayerModel.DelayDestroy(fDelay);
                }
                m_pPlayerModel = null;
            }
            if(m_pPlayerHudCamera!=null)
            {
                //! nochange
                return;
            }
            m_nPlayerModel = 0;
            if (modelData == null) return;
            m_nPlayerModel = modelData.ID;
            RawImage hud = m_pPlayerHud.GetWidget<RawImage>("Hud");
            if (hud)
            {
                m_pPlayerHudCamera = Core.RenderHudManager.getInstance().CreateRenderHud(m_pPlayerHud, hud, cameraPos, dialogData.vHudSize.x, dialogData.vHudSize.y, 30);
                if (m_pPlayerHudCamera != null)
                {
                    m_pPlayerHudCamera.SetEulerAngle(new Vector3(0, 0, 0));
                    m_pPlayerHudCamera.SetPosition(m_vPosition + dialogData.vModelCameraOffset + new Vector3(0,1.8f,-4.0f));
                }
                m_pInstaceModelOp = FileSystemUtil.SpawnInstance(modelData.strFile, true);
                if (m_pInstaceModelOp != null)
                {
                    m_pInstaceModelOp.OnCallback = OnLoadModelHud;
                    m_pInstaceModelOp.OnSign = OnSignModelSign;
                    m_pInstaceModelOp.userData0 = dialogData;
                    m_pInstaceModelOp.pByParent = RootsHandler.FindActorRoot((int)EActorType.PreviewActor);
                    //   m_pInstaceModelOp.Refresh();
                }
            }
        }
        //------------------------------------------------------
        void OnLoadModelHud(InstanceOperiaon opCallback)
        {
            m_pInstaceModelOp = null;
            m_pPlayerModel = opCallback.pPoolAble;
            if(m_pPlayerModel != null)
            {
                Data.CsvData_Dialog.DialogData modelData = opCallback.userData0 as Data.CsvData_Dialog.DialogData;
                m_pPlayerModel.SetPosition(m_vPosition+modelData.vModelCameraOffset, true);
                m_pPlayerModel.SetEulerAngle(new Vector3(0, modelData.fModelRotate, 0));
                m_pPlayerModel.SetRenderLayer(Core.RenderHudManager.RenderLayer);
                m_pPlayerModel.SetScale(Vector3.one);
                Animator animator = m_pPlayerModel.GetBehaviour<Animator>();
                if (animator)
                {
                    m_backupUpdateModel = animator.updateMode;
                    animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                    animator.Play(modelData.playAction);
                }
                SpringManager springManager = m_pPlayerModel.GetBehaviour<SpringManager>();
                if (springManager)
                {
                    springManager.enabled = false;
                }

                CameraSlots cameraSlot = m_pPlayerModel.GetBehaviour<CameraSlots>();
                if(cameraSlot)
                {
                    CameraSlots.Slot cameraSolt = cameraSlot.GetSlot(2);
                    if (!cameraSolt.IsValid())
                        cameraSolt = cameraSlot.GetDefault();
                    if(cameraSolt.IsValid())
                    {
                        if(m_pPlayerHudCamera!=null)
                        {
                            m_pPlayerHudCamera.SetPosition(m_vPosition + modelData.vModelCameraOffset + cameraSolt.position);
                            m_pPlayerHudCamera.SetEulerAngle(cameraSolt.eulerAngle);
                            m_pPlayerHudCamera.SetFov(cameraSolt.fov);
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        void OnSignModelSign(InstanceOperiaon opCallback)
        {
            opCallback.bUsed = m_pBaseUI != null && m_pBaseUI.IsVisible() && opCallback.userData0 == GetCurDialogData();
        }
        //------------------------------------------------------
        void PlayTween(UISerialized serialze, bool bIn)
        {
            for (int i = 0; i < serialze.Widgets.Length; ++i)
            {
                if (serialze.Widgets[i].widget is RtgTween.TweenerGroup)
                {
                    RtgTween.TweenerGroup gp = serialze.Widgets[i].widget as RtgTween.TweenerGroup;
                    if (gp != null)
                    {
                        gp.Play((short)(bIn ? 0 : 1));
                    }
                }
            }
        }
        #endregion
    }
}
