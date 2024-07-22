/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GuidePanel
作    者:	HappLI
描    述:	引导界面面板
*********************************************************************/
using DG.Tweening;
using Framework.Core;
using Framework.Plugin.Guide;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TopGame.Base;
using TopGame.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    [UI((ushort)EUIType.GuidePanel, UI.EUIAttr.UI)]
    public class GuidePanel : UI.UIBase
    {
        int m_nDefaultOrder = 0;
        UIGuideSerialize m_Serialize = null;
        UIGuideView m_View = null;
    //    UIPenetrate m_Penetrate = null;

        private bool m_bSlideFinger = false;
#if UNITY_EDITOR
        Vector4 m_SlidePosParam = Vector4.zero;
#endif
        private int m_SlideStartWidgetGuid = 0;
        private Transform m_SlideStartWidget = null;
        private Vector3 m_SlideStarPos = Vector3.zero;
        private Vector3 m_SlideEndPos = Vector3.zero;
        private Transform m_pSlideFinger = null;
        private Vector3 m_SlideParam = Vector3.zero;
        private Vector3 m_SlideRuning = Vector3.zero;

        private EDescBGType m_TipsType = EDescBGType.None;
        private int m_nTipsDockWidgetGUID = 0;
        private int m_nTipsDockListIndex = -1;
        private Transform m_pTipDockWidget = null;
        //这边的偏移是屏幕坐标的偏移,不是世界坐标的偏移
        private Vector3 m_TipDockOffset = Vector3.zero;
        private bool m_bTipsDockPosUse3D = false;

        public Vector3 SlideStartPos { get { return m_SlideStarPos; } }
        public Vector3 SlideEndPos { get { return m_SlideEndPos; } }

        private bool m_bTopCloneWidget = false;
        private Transform m_pGuideWidget;
        private EventTriggerListener m_pGuideTriggerListen;
        private Transform m_pOriGuideWidget;
        private int m_ListenGuideGuid;
        private bool m_bConvertUIPos = false;
        private int m_nListIndex = -1;
        private Vector3 m_FingerOffset = Vector3.zero;
        private EFingerType m_fingerType = EFingerType.None;

        string m_SearchListenName;
        string m_TipsSearchListenName;

        PositionTween m_tween;

        public bool bDoing = false;

        private bool m_isShowingDialog = false;
        private Coroutine m_DialogCoroutine = null;
        private int m_DialogContentID = 0;
        public bool IsShowingDialog
        {
            get { return m_isShowingDialog; }
        }

        private EventTriggerListener m_SimulationClickListener = null;
        public EventTriggerListener SimulationClickListener
        {
            get
            {
                if (m_SimulationClickListener == null && m_Serialize && m_Serialize.SimulationClickImage)
                {
                    m_SimulationClickListener = m_Serialize.SimulationClickImage.GetComponent<EventTriggerListener>();
                }
                return m_SimulationClickListener;
            }
        }

        /// <summary>
        /// 对话框背景高度最低值
        /// </summary>
        private int m_DescBgMinHeight = 80;

        Dictionary<GameObject, Transform> m_MoveUIDic = new Dictionary<GameObject, Transform>();

        Regex m_Regex = new Regex("(<).+?(>)", RegexOptions.Singleline);//匹配字符中所有<>

        StepNode m_CurStepNode;

        //-------------------------------------------
        public override void Awake()
        {
            base.Awake();
            m_Serialize = m_UI as UIGuideSerialize;
            m_nDefaultOrder = m_nOrder;
            ResetData();
            //Framework.Plugin.Logger.Info("Guide Awake");
            m_View = m_pView as UIGuideView;
        }
        //-------------------------------------------
        protected override void DoShow()
        {
            base.DoShow();
            ResetData();
            if (m_Serialize && m_Serialize.BgMask)
            {
                m_Serialize.BgMask.SetRootCavas(UI.UIKits.GetUICanvasRoot(), UI.UIKits.GetUICamera());
            }
        }
        //-------------------------------------------
        protected override void DoHide()
        {
            ResetData();
            base.DoHide();
            bDoing = false;
        }
        //-------------------------------------------
        public override bool CanHide()
        {
            if (GuideSystem.getInstance().bDoing) return false;
            return base.CanHide();
        }
        //-------------------------------------------
        public void ClearData()
        {
            ResetData();
            bDoing = false;
        }
        //-------------------------------------------
        public void ResetData()
        {
            //Framework.Plugin.Logger.Warning("Guide ResetData");
            m_SlideStartWidget = null;
            m_SlideStartWidgetGuid = 0;
            m_bSlideFinger = false;
            m_pSlideFinger = null;
            m_SlideParam = Vector3.zero;
            m_SlideRuning = Vector3.zero;
            if (m_Serialize == null) return;
            if (m_Serialize.Fingers != null)
            {
                for (int i = 0; i < m_Serialize.Fingers.Length; ++i)
                {
                    if (m_Serialize.Fingers[i])
                    {
                        m_Serialize.Fingers[i].gameObject.SetActive(false);
                        m_Serialize.Fingers[i].rotation = Quaternion.identity;
                    }
                }
            }


            if (m_Serialize.BgMask)
            {
                m_Serialize.BgMask.SetSpeed(0);
                m_Serialize.BgMask.EnablePenetrate(false, 0);
                m_Serialize.BgMask.SetTarget(null);
                m_Serialize.BgMask.SetShape(GuideHighlightMask.EShape.None);
                m_Serialize.BgMask.SetClick(true);
                m_Serialize.BgMask.gameObject.SetActive(false);
            }

            if (m_Serialize.TargetContainer)
            {
                m_Serialize.TargetContainer.DetachChildren();
            }

            if (m_Serialize.DescBgs != null)
            {
                for (int i = 0; i < m_Serialize.DescBgs.Length; ++i)
                {
                    if (m_Serialize.DescBgs[i]) m_Serialize.DescBgs[i].gameObject.SetActive(false);
                }
            }

            ClearWidget();
            ClearTipDock();

            m_isShowingDialog = false;
            m_DialogCoroutine = null;
            m_DialogContentID = 0;

            ResetImage();
            CleatDialogArrow();

            ClearMoveUI();

            if (m_tween != null)
            {
                m_tween.Stop();
            }

            ResetGuideText();
            ResetContinueImage();

            if (m_Serialize.SkipBtn)
            {
                m_Serialize.SkipBtn.gameObject.SetActive(false);
            }
            
        }
        //------------------------------------------------------
        void ClearMoveUI()
        {
            if (m_MoveUIDic == null || m_MoveUIDic.Count == 0)
            {
                return;
            }
            foreach (var item in m_MoveUIDic.Keys)
            {
                if (item == null)
                {
                    continue;
                }
                item.transform.SetParent(m_MoveUIDic[item], true);
            }
            m_MoveUIDic.Clear();
        }
        //------------------------------------------------------
        public void CleatDialogArrow()
        {
            if (DialogArrow == null)
            {
                return;
            }

            DialogArrow.anchoredPosition = new Vector2(0, -5.7f);
            DialogArrow.rotation = Quaternion.identity;
            DialogArrow.localScale = Vector3.one;
            SetDialogArrowEnable(true);
        }
        //------------------------------------------------------
        public Transform GetListenGuideOriWiget()
        {
            return m_pOriGuideWidget;
        }
        //------------------------------------------------------
        public void ClearWidget()
        {
            if(m_bTopCloneWidget)
            {
                if(m_pGuideWidget)
                {
                    GameObject.Destroy(m_pGuideWidget.gameObject);
                }
                if (m_pOriGuideWidget)
                    m_pOriGuideWidget.gameObject.SetActive(true);
            }
            m_bTopCloneWidget = false;
            m_pOriGuideWidget = null;
            m_pGuideWidget = null;
            m_pGuideTriggerListen = null;
            m_ListenGuideGuid = 0;
            m_bConvertUIPos = false;
            m_nListIndex = -1;
            m_fingerType = EFingerType.None;
            m_FingerOffset = Vector3.zero;
            m_SearchListenName = null;
            SetSimulationClickImageActive(false);
            m_CurStepNode = null;
        }
        //------------------------------------------------------
        public Transform TargetContainer
        {
            get
            {
                if (m_Serialize == null) return null;
                return m_Serialize.TargetContainer;
            }
        }
        //------------------------------------------------------
        public UnityEngine.UI.Button VirtualButton
        {
            get
            {
                if (m_Serialize == null) return null;
                return m_Serialize.VirtualButton;
            }
        }
        //------------------------------------------------------
        public GameObject BgMask
        {
            get
            {
                if (m_Serialize == null || m_Serialize.BgMask == null) return null;
                return m_Serialize.BgMask.gameObject;
            }
        }
        //------------------------------------------------------
        public UnityEngine.UI.Text TipLabel
        {
            get
            {
                if (m_Serialize == null) return null;
                return m_Serialize.TipLabel;
            }
        }
        //------------------------------------------------------
        public Transform GetFinger(EFingerType type)
        {
            if (m_Serialize == null || m_Serialize.Fingers == null) return null;
            for (int i = 0; i < m_Serialize.Fingers.Length; ++i)
            {
                if (m_Serialize.Fingers[i])
                    m_Serialize.Fingers[i].gameObject.SetActive(i == (int)type);
            }
            if (type >= 0 && (int)type < m_Serialize.Fingers.Length)
                return m_Serialize.Fingers[(int)type];
            return null;
        }
        //------------------------------------------------------
        public Transform GetDescBG(EDescBGType type)
        {
            if (m_Serialize == null || m_Serialize.DescBgs == null) return null;
            for (int i = 0; i < m_Serialize.DescBgs.Length; ++i)
            {
                if (m_Serialize.DescBgs[i])
                    m_Serialize.DescBgs[i].gameObject.SetActive(i == (int)type);
            }
            if (type >= 0 && (int)type < m_Serialize.DescBgs.Length)
            {
                return m_Serialize.DescBgs[(int)type];
            }
            return null;
        }
        //------------------------------------------------------
        public RectTransform DialogArrow
        {
            get
            {
                if (m_Serialize == null) return null;
                return m_Serialize.DialogArrow;
            }
        }
        //------------------------------------------------------
        public Image GuideImage
        {
            get
            {
                if (m_Serialize == null) return null;
                return m_Serialize.GuideImage;
            }
        }
        //------------------------------------------------------
        public Image ContinueImage
        {
            get
            {
                if (m_Serialize == null) return null;
                return m_Serialize.ContinueImage;
            }
        }
        //------------------------------------------------------
        public Text GuideText
        {
            get
            {
                if (m_Serialize == null) return null;
                return m_Serialize.GuideText;
            }
        }
        //------------------------------------------------------
        public void SetDefaultOrder()
        {
            SetOrder(m_nDefaultOrder);
        }
        //-------------------------------------------
        public void OverClear(bool bReset = true)
        {
            if (bReset) ResetData();
        }
        //-------------------------------------------
        public void SlideArrow(Vector4 slidePos, Vector2 param)
        {

        }
        //------------------------------------------------------
        public void AddTipDock(EDescBGType tipType, int tipsContextID, Color tipColor, float speed, int autoHideTime,Vector3 offset, bool is3D, int dockWidget, int listIndex =-1,string listenerName="",bool enableArrow = true)
        {
            ClearTipDock();
            SetTipsActive(tipsContextID>0);
            SetTipsLabel(tipsContextID, speed);
            SetTipsLabelColor(tipColor);
            CalcDescBgHeight(tipType, tipsContextID);
            SetDialogArrowEnable(enableArrow);

            m_TipsType = tipType;
            m_nTipsDockWidgetGUID = dockWidget;
            m_nTipsDockListIndex = listIndex;
            //这边的偏移是屏幕坐标的偏移,不是世界坐标的偏移
            m_TipDockOffset = offset;
            m_bTipsDockPosUse3D = is3D;
            m_TipsSearchListenName = listenerName;
            ListenTipDockWidget();
            if(m_nTipsDockWidgetGUID<=0)
                GetDescBG(tipType);

            if (autoHideTime>0)
            {
                TopGame.Base.Util.Timer(autoHideTime / 1000, () =>
                {
                    GetDescBG(EDescBGType.None);
                });
            }
        }
        //------------------------------------------------------
        public void ClearTipDock()
        {
            GetDescBG(EDescBGType.None);

            SetTipsActive(false);
            m_pTipDockWidget = null;
            m_TipsType = EDescBGType.None;
            m_nTipsDockWidgetGUID = 0;
            m_nTipsDockListIndex = -1;
            m_TipDockOffset = Vector3.zero;
            m_bTipsDockPosUse3D = false;
            m_TipsSearchListenName = "";
            SetAvatarEnable(true);
        }
        //------------------------------------------------------
        void ListenTipDockWidget()
        {
            Vector3 pos =Vector3.zero;
            bool isFollowGuid = false;
            if (m_nTipsDockWidgetGUID>0)
            {
                GuideGuid guide = GuideGuidUtl.FindGuide(m_nTipsDockWidgetGUID);
                if (guide == null) return;
                if (m_nTipsDockListIndex > 0)
                {
                    m_pTipDockWidget = GetListIndexTransform(guide, m_nTipsDockListIndex - 1);
                    if (!string.IsNullOrWhiteSpace(m_TipsSearchListenName))//如果有输入控件名字,就查找控件名字
                    {
                        var listeners = m_pTipDockWidget.GetComponentsInChildren<EventTriggerListener>();
                        foreach (var item in listeners)
                        {
                            if (item.name.Equals(m_TipsSearchListenName))
                            {
                                m_pTipDockWidget = item.transform;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    m_pTipDockWidget = guide.transform;
                }
                GetDescBG(m_TipsType);
                SetTipsActive(true);
            }

            //对话框累加目标坐标
            if (m_pTipDockWidget)
            {
                pos += m_pTipDockWidget.position;//这边使用目标的世界坐标,偏移用屏幕坐标添加
                isFollowGuid = true;
            }

            SetTipsPos(pos, m_bTipsDockPosUse3D, m_TipsType, isFollowGuid);
        }
        //------------------------------------------------------
        //        public void SlideFinger(Transform finger, StepData step)
        //        {
        //            m_bSlideFinger = false;
        ////             if (step == null || step.FingerOffset.x <= 0) return;
        ////             if (finger == null && step.Slide3DArrow == 0)
        ////                 return;
        ////             m_pSlideFinger = finger;
        ////             m_bSlideFinger = true;
        ////             m_SlideParam = step.FingerOffset;
        ////             m_SlideParam.y = step.ClickZoom.x;
        ////             m_SlideParam.z = step.ClickZoom.y;
        //// 
        ////             m_SlideStartWidget = null;
        ////             Vector3 start3DPos = Vector3.zero;
        ////             GuideSystem guideSystem = GameInstance.getInstance().guideSystem;
        ////             if (step.WidgetGuid!=0)
        ////             {
        ////                 m_SlideStartWidgetGuid = step.WidgetGuid;
        ////                 m_SlideStartWidget = guideSystem.FastFindWidet(step.WidgetGuid);
        ////                 if (m_SlideStartWidget == null)
        ////                 {
        ////                     float startposx = Screen.width * step.FingerSlidePos.x;
        ////                     float startposy = Screen.height * step.FingerSlidePos.y;
        ////                     m_SlideStarPos = guideSystem.ConverScreenToUIPos(new Vector3(startposx, startposy, 0));
        ////                     start3DPos = guideSystem.ConverScreenToWorldPos(new Vector3(startposx, startposy));
        ////                 }
        ////                 else
        ////                 {
        ////                     m_SlideStarPos = m_SlideStartWidget.position;
        ////                     Vector3 screenpos = guideSystem.ConverUIPosToScreen(m_SlideStarPos);
        ////                     start3DPos = guideSystem.ConverScreenToWorldPos(screenpos);
        ////                 }
        ////             }
        ////             else
        ////             {
        ////                 m_SlideStartWidgetGuid = 0;
        ////                 if (step.SlidePosUse3D != 0)
        ////                 {
        ////                     m_SlideStarPos = guideSystem.ConverWorldPosToUIPos(new Vector3(step.FingerSlidePos.x, 0, step.FingerSlidePos.y));
        ////                     start3DPos = new Vector3(step.FingerSlidePos.x, 0, step.FingerSlidePos.y);
        ////                 }
        ////                 else
        ////                 {
        ////                     float startposx = Screen.width * step.FingerSlidePos.x ;
        ////                     float startposy = Screen.height * step.FingerSlidePos.y;
        ////                     m_SlideStarPos = guideSystem.ConverScreenToUIPos(new Vector3(startposx, startposy, 0));
        ////                     start3DPos = guideSystem.ConverScreenToWorldPos(new Vector3(startposx, startposy, 0));
        ////                 }
        ////             }
        //// 
        ////             if (step.SlidePosUse3D == 0)
        ////             {
        ////                 float posx = Screen.width * step.FingerSlidePos.z ;
        ////                 float posy = Screen.height * step.FingerSlidePos.w;
        ////                 m_SlideEndPos = guideSystem.ConverScreenToUIPos(new Vector3(posx, posy, 0));
        ////             }
        ////             else
        ////             {
        ////                 m_SlideEndPos = guideSystem.ConverWorldPosToUIPos(new Vector3(step.FingerSlidePos.z, 0, step.FingerSlidePos.w));
        ////             }
        ////             m_SlideRuning = Vector3.zero;
        ////             InnerUpdate(0f);
        //// #if UNITY_EDITOR
        ////             m_SlidePosParam = step.FingerSlidePos;
        //// #endif
        //        }
        //-------------------------------------------
        protected override void InnerUpdate(float fFrameTime)
        {
            if(m_ListenGuideGuid>0)
            {
                ListenWidget();
            }
            ListenTipDockWidget();
            if (m_fingerType == EFingerType.Click || m_fingerType == EFingerType.Effect)
            {
                if (m_pGuideWidget)
                {
                    if (m_Serialize.uiPenetrate)
                    {
                        if(m_pGuideTriggerListen) m_pGuideTriggerListen.SetParentrate(m_Serialize.uiPenetrate);
                        m_Serialize.uiPenetrate.SearchListenName = m_SearchListenName;
                        m_Serialize.uiPenetrate.TriggerGo = m_pOriGuideWidget ? m_pOriGuideWidget.gameObject : m_pGuideWidget.gameObject;
                    }

                    Transform finger = GetFinger(m_fingerType);
                    if (finger)
                    {
                        if (m_bConvertUIPos)
                        {
                            Vector3 uiPos = Vector3.zero;
                            if (UI.UIKits.WorldPosToUIPos(m_pGuideWidget.transform.position, false, ref uiPos))
                            {
                                finger.position = uiPos;
                                SetSimulationClickImagePosition(uiPos);
                            }
                        }
                        else
                            finger.position = m_pGuideWidget.transform.position;
                        finger.localPosition += m_FingerOffset;
                    }

                    if(m_Serialize.BgMask.isActiveAndEnabled)
                    {
                        if (m_pGuideWidget is RectTransform)
                        {
                            m_Serialize.BgMask.SetTarget(m_pGuideWidget as RectTransform);
                        }
                        else if (finger is RectTransform)//如果是3D建筑坐标,设置遮罩显示位置为手指的坐标
                        {
                            m_Serialize.BgMask.Set3DTarget((finger as RectTransform).anchoredPosition);
                        }
                    }
                    else
                        m_Serialize.BgMask.SetTarget(null);

                    RectTransform widget = m_pOriGuideWidget as RectTransform;
                    if (widget)
                    {
                        RectTransform rect = m_Serialize.TargetContainer as RectTransform;
                        rect.anchoredPosition = widget.anchoredPosition;
                        rect.anchoredPosition3D = widget.anchoredPosition3D;
                        rect.anchorMax = widget.anchorMax;
                        rect.anchorMin = widget.anchorMin;
                        rect.offsetMax = widget.offsetMax;
                        rect.offsetMin = widget.offsetMin;
                        rect.pivot = widget.pivot;
                        rect.sizeDelta = widget.sizeDelta;
                        rect.position = widget.position;
                        rect.rotation = widget.rotation;
                       // rect.localScale = widget.localScale;

                        if(m_pGuideWidget != m_pOriGuideWidget)
                        {
                            rect = m_pGuideWidget as RectTransform;
                            rect.anchoredPosition = widget.anchoredPosition;
                            rect.anchoredPosition3D = widget.anchoredPosition3D;
                            rect.anchorMax = widget.anchorMax;
                            rect.anchorMin = widget.anchorMin;
                            rect.offsetMax = widget.offsetMax;
                            rect.offsetMin = widget.offsetMin;
                            rect.pivot = widget.pivot;
                            rect.sizeDelta = widget.sizeDelta;
                            rect.position = widget.position;
                            rect.rotation = widget.rotation;
                            if (rect.localScale == Vector3.zero)//克隆出来UI缩放为0时,默认设置为1
                            {
                                rect.localScale = Vector3.one;
                            }
                            //rect.localScale = widget.localScale;
                        }

                    }
                }
                else if(m_ListenGuideGuid<=0)
                {
                    ShowSkipBtn(GuideSystem.getInstance().DoingSeqNode);
                }
                else
                {
                    Transform finger = GetFinger(m_fingerType);
                    if(finger) finger.position = Framework.Core.CommonUtility.INVAILD_POS;
                }
            }
            if (!m_bSlideFinger) return;
        }
        //-------------------------------------------
        public void SetMaskActive(bool isActive)
        {
            Util.SetActive(BgMask, isActive);
        }
        //-------------------------------------------
        public void SetMaskColor(Color color)
        {
            if (m_Serialize && m_Serialize.BgMask)
            {
                m_Serialize.BgMask.color = color;
            }
        }
        //-------------------------------------------
        public void SetMaskShape(EMaskType type)
        {
            if (m_Serialize && m_Serialize.BgMask)
            {
                m_Serialize.BgMask.SetShape((GuideHighlightMask.EShape)type);
            }
        }
        //-------------------------------------------
        public void SetMaskSpeed(float speed)
        {
            if (m_Serialize && m_Serialize.BgMask)
            {
                m_Serialize.BgMask.SetSpeed(speed);
            }
        }
        //-------------------------------------------
        public void SetTipsLabel(int contentID)
        {
            string content = Core.LocalizationManager.ToLocalization((uint)contentID);
            UIUtil.SetLabel(TipLabel, content);
        }
        //-------------------------------------------
        public void SetTipsLabel(int contentID, float speed)
        {
            m_DialogContentID = contentID;
            if (speed>0)
            {
                if (TipLabel != null)
                {
                    string content = Core.LocalizationManager.ToLocalization((uint)contentID);
                    m_DialogCoroutine = TargetContainer.GetComponent<Image>().StartCoroutine(LabelTransition(content, speed));
                }
            }
            else
            {
                SetTipsLabel(contentID);
            }
        }
        //-------------------------------------------
        System.Collections.IEnumerator LabelTransition(string content, float speed)
        {
            m_isShowingDialog = true;
            WaitForSeconds wait = new WaitForSeconds(Time.deltaTime / (1f / speed));

            for (int i = 0; i <= content.Length; i++)
            {
                UIUtil.SetLabel(TipLabel, content.Substring(0, i));
                yield return wait;
            }
            m_isShowingDialog = false;
        }
        //-------------------------------------------
        System.Collections.IEnumerator GuideTextTransition(string content, float speed)
        {
            m_isShowingDialog = true;
            WaitForSeconds wait = new WaitForSeconds(1f / speed);

            for (int i = 1; i <= content.Length; i++)
            {
                UIUtil.SetLabel(GuideText, content.Substring(0, i));
                yield return wait;
            }
            m_isShowingDialog = false;
        }
        //-------------------------------------------
        public void SetTipsLabelColor(Color color)
        {
            if (TipLabel == null)
            {
                return;
            }
            TipLabel.color = color;
        }
        //-------------------------------------------
        public void SetTipsPos(Vector3 pos, EDescBGType type,bool isFollowGuid)
        {
            if (TipLabel == null)
            {
                return;
            }
            //TipLabel.transform.localPosition = pos;文字跟随背景框移动,没有背景框,不显示文字
            Transform bg = GetDescBG(type);
            if (bg != null)
            {
                if (isFollowGuid)
                {
                    bg.position = pos;//先设置背景框位置,用世界坐标
                    bg.localPosition += m_TipDockOffset;//再加上偏移,用屏幕坐标
                }
                else
                {
                    bg.localPosition = m_TipDockOffset;//不是跟随控件情况下,直接用屏幕坐标赋值
                }
            }
            LimitTipsPos(type);
        }
        //------------------------------------------------------
        /// <summary>
        /// 限制超出屏幕
        /// 目前只做左右屏幕判断
        /// </summary>
        /// <param name="type"></param>
        void LimitTipsPos(EDescBGType type)
        {
            RectTransform bg = GetDescBG(type) as RectTransform;
            if (bg == null)
            {
                return;
            }

            float screenWidth = bg.rect.size.x/2;

            Vector2 screen = GameInstance.getInstance().uiManager.GetReallyResolution();

            float offset = 0;
            float avatarWidth = 0;
            if (m_Serialize.Avatar != null)
            {
                avatarWidth = m_Serialize.Avatar.rectTransform.rect.x;
            }

            if (((bg.anchoredPosition.x + screenWidth) > screen.x /2))//超出右屏幕
            {
                offset = screen.x / 2 - (bg.anchoredPosition.x + screenWidth);
            }
            else if ((bg.anchoredPosition.x - screenWidth - avatarWidth / 2) < (-screen.x / 2))//超出左屏幕(左屏幕由于有角色图标,所以需要加上角色宽度一半)
            {
                offset = -screen.x / 2 - (bg.anchoredPosition.x - screenWidth - avatarWidth / 2);
            }

            bg.anchoredPosition += new Vector2(offset,0);
        }
        //-------------------------------------------
        public void SetTipsActive(bool isActive)
        {
            if (TipLabel == null)
            {
                return;
            }
            Util.SetActive(TipLabel.gameObject, isActive);
        }
        //-------------------------------------------
        public void SetTipsPos(Vector3 pos, bool is3D, EDescBGType type,bool isFollowGuid)
        {
            if (TipLabel == null)
            {
                return;
            }
            if (is3D)
            {
                pos = GameInstance.getInstance().uiManager.GetUICamera().WorldToScreenPoint(pos);
            }
            SetTipsPos(pos, type, isFollowGuid);
        }
        //-------------------------------------------
        public void SetFinger(EFingerType type, Vector3 angle, Vector2 offset)
        {
            m_fingerType = type;
            Transform finger = GetFinger(type);
            if (finger == null)
            {
                Framework.Plugin.Logger.Warning("获取不到手势类型:" + type);
                return;
            }
            finger.rotation = Quaternion.Euler(angle);
            finger.localPosition += new Vector3(offset.x, offset.y, 0);
        }
        //-------------------------------------------
        public void SetFinger(EFingerType type, Vector3 angle, Vector3 offset)
        {
            m_fingerType = type;
            Transform finger = GetFinger(type);
            if (finger == null)
            {
                Framework.Plugin.Logger.Warning("获取不到手势类型:" + type);
                return;
            }
            m_FingerOffset = offset;
            finger.rotation = Quaternion.Euler(angle);
        }
        //-------------------------------------------
        public void ClickZoom(EFingerType type, Vector3 angle, Vector3 pos, bool is3D, float radius)
        {
            m_fingerType = type;
            Transform finger = GetFinger(type);
            if (finger == null)
            {
                Framework.Plugin.Logger.Warning("获取不到手势类型:" + type);
                return;
            }
            finger.rotation = Quaternion.Euler(angle);
            if (is3D)
            {
                pos = GameInstance.getInstance().uiManager.GetUICamera().WorldToScreenPoint(new Vector3(pos.x, 0, pos.y));
            }
            finger.localPosition = pos;
            //todo:radius 半径大小怎么表现?
        }
        //-------------------------------------------
        public void MoveUI(GameObject ui, bool isHighLight)
        {
            //CleatMoveUI();

            if (isHighLight)
            {
                if (!m_MoveUIDic.ContainsKey(ui))
                {
                    m_MoveUIDic.Add(ui, ui.transform.parent);

                    ui.transform.SetParent(TargetContainer, true);
                }
            }
            else
            {
                if (m_MoveUIDic.ContainsKey(ui))
                {
                    Transform parent = m_MoveUIDic[ui];
                    ui.transform.SetParent(parent);
                    m_MoveUIDic.Remove(ui);
                }
            }
        }
        //-------------------------------------------
        public void SlideFinger(Vector2 startPos, Vector2 endPos, int sliderSpeed)
        {
            Transform finger = GetFinger(EFingerType.Slide);

            if (m_tween == null)
            {
                m_tween = finger.GetComponentInChildren<PositionTween>();
            }

            if (m_tween == null)
            {
                return;
            }
            float time = sliderSpeed / 100f;
            float speed = 1f / time;
            //m_FingerDT.endValueV3 = endPos;

            //m_FingerDT.tween.Rewind();
            //m_FingerDT.tween.Kill();
            //m_FingerDT.tween.active
            //m_FingerDT.duration = speed;

            m_tween.transform.localPosition = startPos;
            m_tween.isLoop = true;
            m_tween.Play(startPos, endPos, speed);

            //if (m_FingerDT.isValid)
            {
                //m_FingerDT.CreateTween();
                //m_FingerDT.tween.fullPosition = 0;
                //m_FingerDT.tween.Play();
                //m_FingerDT.transform.DOPunchPosition(endPos, speed);
                //m_FingerDT.transform.DOPlay();
                //m_FingerDT.tween.Restart();


            }
        }
        //-------------------------------------------
        /// <summary>
        /// 点击步骤
        /// </summary>
        public void WidgetListen(StepNode stepNode, EFingerType type, Vector3 angle, Vector2 offset, int clickUI, int clickIndex, bool bTop = true,string searchName = "")
        {
            m_Serialize.BgMask.SetTarget(null);
            ClearWidget();
            m_CurStepNode = stepNode;
            m_bTopCloneWidget = bTop;
            m_nListIndex = clickIndex;
            m_ListenGuideGuid = clickUI;
            m_bConvertUIPos = false;
            m_SearchListenName = searchName;
            SetFinger(type, angle, new Vector3(offset.x, offset.y, 0));
            Transform trans = GetFinger(type);
            //if (trans) trans.gameObject.SetActive(false);//为啥要隐藏?
            ListenWidget();
            Show();
        }
        //------------------------------------------------------
        public void SetCurStepNode(StepNode stepNode)
        {
            m_CurStepNode = stepNode;
        }
        //-------------------------------------------
        void ListenWidget()
        {
            if (m_ListenGuideGuid <= 0) return;
            GuideGuid widget = GuideGuidUtl.FindGuide(m_ListenGuideGuid);
            if (widget && GuideUtility.IsCheckInView(widget.transform))
            {
                //检测组件状态,如果强制引导,并且没有跳过步骤,则默认显示
                if (widget.gameObject.activeInHierarchy == false && m_CurStepNode != null && m_CurStepNode.bOption == false && m_CurStepNode.bSuccessedListenerBreak == false)
                {
                    widget.gameObject.SetActive(true);
                    var listener = widget.GetComponent<UnLockListener>();
                    if (listener)
                    {
                        if (GameInstance.getInstance() != null && GameInstance.getInstance().unlockMgr != null) GameInstance.getInstance().unlockMgr.SetUnLockState(listener);
                        else listener.OnUnlock();
                    }
                }
                m_bConvertUIPos = widget.ConvertUIPos;
                if (m_nListIndex > 0)//动态列表加载情况
                {
                    m_pOriGuideWidget = GetListIndexTransform(widget, m_nListIndex - 1);
                    if (m_pOriGuideWidget == null)
                    {
                        //滚动到英雄头像位置
                        SetGridBoxMgrScrollToIndex(widget, m_nListIndex);
                    }
                    if (!string.IsNullOrWhiteSpace(m_SearchListenName) && m_pOriGuideWidget != null)//如果有输入控件名字,就查找控件名字
                    {
                        Transform trs = m_pOriGuideWidget.Find(m_SearchListenName);//如果查找子物体那么格式为 父物体/子物体
                        if (trs)
                        {
                            m_pOriGuideWidget = trs;
                        }
                        else
                        {
                            //兼容旧得查找子物体
                            var listeners = m_pOriGuideWidget.GetComponentsInChildren<EventTriggerListener>();
                            foreach (var item in listeners)
                            {
                                if (item.name.Equals(m_SearchListenName))
                                {
                                    m_pOriGuideWidget = item.transform;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                    m_pOriGuideWidget = widget.transform;

                if(m_pOriGuideWidget)
                {
                    if (/*!GuideUtility.IsCheckInView(m_pOriGuideWidget.transform) ||*/ GuideUtility.IsCheckTweening(m_pOriGuideWidget.transform) ||
                        !m_pOriGuideWidget.gameObject.activeSelf) return;
                }

                if (m_bTopCloneWidget)
                {
                    if(m_pOriGuideWidget)
                    {
                        GameObject pClone = GameObject.Instantiate(m_pOriGuideWidget.gameObject, m_Serialize.TargetContainer, true);
                        m_pGuideWidget = pClone.transform;
                        m_pGuideTriggerListen = pClone.GetComponentInChildren<EventTriggerListener>();
                        pClone.AddComponent<UITouchIngore>();
                         GuideGuid guide = pClone.GetComponent<GuideGuid>();
                        if(guide) GameObject.DestroyImmediate(guide);
                     //   m_pOriGuideWidget.gameObject.SetActive(false);
                        GuideGuidUtl.OnAdd(widget);
                    }
                }
                else
                {
                    m_pGuideWidget = m_pOriGuideWidget;
                }
                //找到组件的情况下才清空
                if (m_pOriGuideWidget != null)
                {
                    m_ListenGuideGuid = 0;
                }

                //3d建筑坐标情况下,设置模拟点击得图片状态
                if (m_bConvertUIPos)
                {
                    SetSimulationClickImageActive(true);
                    SetSimulationClickImageEventTriggerListenerGuid(widget);
                }
            }
        }
        //------------------------------------------------------
        public void SetMaskHighLightRect(int clickUI,int clickIndex, string searchName = "",bool click = true)
        {
            m_Serialize.BgMask.SetTarget(null);
            ClearWidget();
            m_ListenGuideGuid = clickUI;
            m_nListIndex = clickIndex;
            m_SearchListenName = searchName;
            ListenWidget();
            m_Serialize.BgMask.SetShape(GuideHighlightMask.EShape.Box);
            m_Serialize.BgMask.SetTarget(m_pGuideWidget as RectTransform);
            m_Serialize.BgMask.SetClick(click);
        }
        //-------------------------------------------
        public Transform GetListIndexTransform(GuideGuid guide,int index)
        {
            if (guide == null || index < 0)
            {
                return null;
            }

            IGuideScroll scroll = null;

            scroll = guide.GetComponent<IGuideScroll>();
            if (scroll != null)
            {
                return scroll.GetItemByIndex(index);
            }
            return null;
        }
        //-------------------------------------------
        /// <summary>
        /// 对话点击
        /// </summary>
        /// <returns></returns>
        public bool OnDialogClick()
        {
            if (m_isShowingDialog)
            {
                if (m_DialogCoroutine != null)
                {
                    TargetContainer.GetComponent<Image>().StopCoroutine(m_DialogCoroutine);
                    m_DialogCoroutine = null;
                    SetTipsLabel(m_DialogContentID);
                    SetGuideText(m_DialogContentID);
                    m_isShowingDialog = false;
                }
                return false;
            }
            return true;
        }
        //-------------------------------------------
        public void SetDialogArrow(int widgetID,Vector2 offset,float angle,bool isReverse)
        {
            if (DialogArrow == null)
            {
                return;
            }
            GuideGuid guide = null;
            if (widgetID > 0)
            {
                guide = GuideGuidUtl.FindGuide(widgetID);
            }
            if (guide != null)
            {
                //todo:这边的guide获取有问题,没有考虑到动态加载组件的情况,并且使用的坐标也是本地坐标,可能会出现位置不对的情况
                DialogArrow.localPosition = guide.transform.localPosition + new Vector3(offset.x,offset.y);
            }
            else
            {
                DialogArrow.localPosition += new Vector3(offset.x, offset.y);
            }
            DialogArrow.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (isReverse)
            {
                DialogArrow.localScale = new Vector3(-1, 1, 1);
            }
        }
        //------------------------------------------------------
        void SetDialogArrowEnable(bool enable)
        {
            if (DialogArrow)
            {
                DialogArrow.gameObject.SetActive(enable);
            }
        }
        //-------------------------------------------
        public void SetGuideImage(string strPath,bool isSetNativeSize,Vector2 pos,float angleZ,bool isReverse,Vector2 size,bool isActive,Color color)
        {
            if (GuideImage == null)
            {
                return;
            }

            GuideImage.gameObject.SetActive(isActive);

            if (m_View != null)
            {
                m_View.LoadObjectAsset(GuideImage, strPath);
            }
            

            if (isSetNativeSize)
            {
                GuideImage.SetNativeSize();
            }
            else
            {
                GuideImage.rectTransform.sizeDelta = size;
            }

            GuideImage.transform.localPosition = pos;

            GuideImage.transform.localScale = isReverse ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
            GuideImage.transform.rotation = Quaternion.Euler(0, 0, angleZ);

            GuideImage.color = color;
        }
        //-------------------------------------------
        void ResetImage()
        {
            if (GuideImage == null)
            {
                return;
            }

            GuideImage.gameObject.SetActive(false);
            GuideImage.transform.localPosition = Vector3.zero;
            GuideImage.transform.rotation = Quaternion.identity;
            GuideImage.transform.localScale = Vector3.one;
        }
        //-------------------------------------------
        public void CalcDescBgHeight(EDescBGType descBg,int contentID)
        {
            Transform trs = GetDescBG(descBg);
            if (trs == null)
            {
                Framework.Plugin.Logger.Warning("trs为null,不计算高度");
                return;
            }
            RectTransform rect = trs as RectTransform;
            string content = Core.LocalizationManager.ToLocalization((uint)contentID);
            if (content == null)
            {
                Framework.Plugin.Logger.Warning("显示的文本内容为null,不计算高度");
                return;
            }

            int fontSize = TipLabel.fontSize;

            int row = Mathf.CeilToInt(rect.sizeDelta.x / fontSize);//每行文字个数,向上取整
            int showLabelCount = 0;
            var matchs = m_Regex.Matches(content);
            if (matchs.Count > 0)//考虑到富文本的使用,去掉富文本的字符长度
            {
                showLabelCount = content.Length;
                foreach (Match item in matchs)
                {
                    //Debug.Log(item);
                    showLabelCount -= item.Length;
                }
            }
            else
            {
                showLabelCount = content.Length;
            }
            
            int rowCount = Mathf.CeilToInt((float)showLabelCount / (float)row) + 2;//计算总共有几行,向上取整 + 上下两行留空
            float height = rowCount * (fontSize + TipLabel.lineSpacing *4) + Mathf.Abs(TipLabel.rectTransform.sizeDelta.y);//4是文字间隔 + 文本上下边距
            height = height > m_DescBgMinHeight ? height : m_DescBgMinHeight;//高度最少值
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
            //Framework.Plugin.Logger.Warning("row:" + row + ",column:" + rowCount + ",height:" + height);
        }
        //------------------------------------------------------
        public void SetGuideText(Vector2 size,Color color,Vector2 pos,int fontSize,bool isEnable,int textID,bool isTransition,int speed)
        {
            if (GuideText == null)
            {
                Framework.Plugin.Logger.Error("找不到GuideText对象");
                return;
            }

            GuideText.enabled = isEnable;

            GuideText.color = color;
            GuideText.rectTransform.sizeDelta = size;
            GuideText.rectTransform.localPosition = pos;
            GuideText.fontSize = fontSize;

            string text = Core.LocalizationManager.ToLocalization((uint)textID);
            if (isTransition && text != null)
            {
                m_DialogCoroutine = TargetContainer.GetComponent<Image>().StartCoroutine(GuideTextTransition(text, speed));
            }
            else
            {
                SetGuideText(textID);
            }

            m_DialogContentID = textID;
        }
        //------------------------------------------------------
        public void SetGuideText(int textID)
        {
            if (GuideText == null)
            {
                Framework.Plugin.Logger.Error("找不到GuideText对象");
                return;
            }
            UIUtil.SetLabel(GuideText, (uint)textID);

            m_DialogContentID = textID;
        }
        //-------------------------------------------
        void ResetGuideText()
        {
            if (GuideText == null)
            {
                return;
            }

            GuideText.enabled = false;

            GuideText.color = Color.white;
            GuideText.rectTransform.sizeDelta = Vector2.zero;
            GuideText.rectTransform.localPosition = Vector2.zero;
            GuideText.fontSize = 23;
            GuideText.text = "";
        }
        //------------------------------------------------------
        public void SetContinueImageEnable(bool isEnable)
        {
            if (ContinueImage == null)
            {
                return;
            }

            ContinueImage.enabled = isEnable;
        }
        //------------------------------------------------------
        public void SetContinueImagePos(Vector2 pos)
        {
            if (ContinueImage == null)
            {
                return;
            }

            ContinueImage.rectTransform.anchoredPosition = pos;
        }
        //------------------------------------------------------
        public void SetContinueImageSize(Vector2 size)
        {
            if (ContinueImage == null)
            {
                return;
            }

            ContinueImage.rectTransform.sizeDelta = size;
        }
        //------------------------------------------------------
        public void ResetContinueImage()
        {
            if (ContinueImage == null)
            {
                return;
            }

            ContinueImage.SetNativeSize();
            Vector2 size = ContinueImage.rectTransform.sizeDelta;
            ContinueImage.rectTransform.anchoredPosition = new Vector2(-size.x/2f, size.y/2f);
            ContinueImage.enabled = false;
        }
        //------------------------------------------------------
        public void OnStuckSkipGuide(SeqNode pNode)
        {
            if (GuideSystem.getInstance().IsEnableStuckSkip == false)
            {
                return;
            }

            GuideSystem.getInstance().OnStuckSkipGuide(pNode.GetTag());
      //      Net.GuideHandler.Req_SetGuideRequest(pNode.GetTag());
        }
        //------------------------------------------------------
        public void ShowSkipBtn(SeqNode pNode)
        {
            if (pNode == null) return;
            if (m_Serialize.SkipBtn)
            {
                m_Serialize.SkipBtn.gameObject.SetActive(true);
                EventTriggerListener.Get(m_Serialize.SkipBtn.gameObject).onClick = (go,param) => {
                    OnStuckSkipGuide(pNode);
                };
            }
        }
        //------------------------------------------------------
        public void SetPenetrateEnable(bool enable,int targetGuid)
        {
            if (m_Serialize && m_Serialize.BgMask)
                m_Serialize.BgMask.EnablePenetrate(enable, targetGuid);
        }
        //-------------------------------------------
        public void SetGridBoxMgrScrollToIndex(GuideGuid guide, int index)
        {
            if (guide == null || index < 0)
            {
                return;
            }
            ListView listView = guide.GetComponent<ListView>();
            if (listView)
            {
                listView.ScrollTo(index,0);
            }
        }
        //------------------------------------------------------
        void SetSimulationClickImageActive(bool active)
        {
            if (m_Serialize&& m_Serialize.SimulationClickImage)
            {
                m_Serialize.SimulationClickImage.gameObject.SetActive(active);
            }
        }
        //------------------------------------------------------
        void SetSimulationClickImagePosition(Vector3 pos)
        {
            if (m_Serialize && m_Serialize.SimulationClickImage)
            {
                m_Serialize.SimulationClickImage.position = pos;
            }
        }
        //------------------------------------------------------
        void SetSimulationClickImageEventTriggerListenerGuid(GuideGuid guid)
        {
            if (SimulationClickListener)
            {
                SimulationClickListener.SetGuideGuid(guid);
            }
        }
        //------------------------------------------------------
        public void SetAvatarEnable(bool enable)
        {
            if (m_Serialize && m_Serialize.Avatar)
            {
                m_Serialize.Avatar.enabled = enable;
            }
        }
    }
}