/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GuideStepHandler
作    者:	HappLI
描    述:	
*********************************************************************/

using System;
using System.Collections.Generic;
using TopGame;
using Framework.Module;
using TopGame.UI;
using UnityEngine;
using TopGame.Logic;
using UnityEngine.Playables;
using TopGame.SvrData;
using TopGame.Data;
using Framework.Core;
using Framework.Logic;
using Framework.BattlePlus;
namespace Framework.Plugin.Guide
{
    public class GuideStepHandler
    {
        static Vector2 m_TouchBeginPos;
        static Vector2 m_BeginMousePos;
        //------------------------------------------------------
        public static bool OnGuideSuccssedListener(StepNode pNode)
        {
            switch ((GuideStepType)pNode.type)
            {
                case GuideStepType.ClickUI:
                    {
                        GuidePanel guidePanel = GetGuidePanel();
                        if (guidePanel == null) return false;
                        if(guidePanel.GetListenGuideOriWiget()!=null)
                        {
                            GuideGuid guid = GuideGuidUtl.FindGuide(pNode._Ports[0].fillValue);
                            if (guid == null || !GuideUtility.IsCheckInView(guid.transform) || !guid.gameObject.activeInHierarchy)//找不到目标,或者不在视野范围内,或者隐藏了
                            {
                                ResetGuidePanel();
                                return false;
                            }
                        }
                    }
                    break;
            }
            return true;
        }
        //------------------------------------------------------

        private static int m_nowGuid = 0;
        public static bool OnGuideSign(StepNode pNode, CallbackParam param)
		{
            if (param.touchType == ETouchType.Begin)
                m_BeginMousePos = param.mousePos;
            if (m_nowGuid != pNode.Guid)
            {
                // Framework.Plugin.Logger.Warning("OnGuideSign pNode.type= " + pNode.type);
                //打点
                TopGame.Core.AUserActionManager.AddActionKV("guide_id", pNode.guideGroupGUID);
                TopGame.Core.AUserActionManager.AddActionKV("step_id", pNode.Guid);
                AddEventUtil.LogEvent("rookie_guide_step",true);
                m_nowGuid = pNode.Guid;
            }
            var guidePanel = GetGuidePanel();
            switch ((GuideStepType)pNode.type)
            {
                case GuideStepType.ClickUI:
                    return OnClickUISign(pNode, param);
                case GuideStepType.Slide:
                    bool result = OnSlideSign(pNode, param); 
                    if (result)
                    {
                        ResetGuidePanel();
                    }
                    return result;
                case GuideStepType.ClickZoom:
                    result = OnClickZoomSign(pNode, param);
                    if (result)
                    {
                        ResetGuidePanel();
                    }
                    return result;
                case GuideStepType.ClickAnywhere:
                    result = OnClickAnywhereSign(pNode, param);
                    if (guidePanel != null)
                    {
                        guidePanel.bDoing = false;
                    }
                    if (result)
                    {
                        ResetGuidePanel();
                    }
                    return result;
                case GuideStepType.WaitAnimPathEnd:
                    //Debug.LogError("isPlaying:" + GameInstance.getInstance().animPather.IsPlaying(pNode._Ports[0].fillValue));
                    return !GameInstance.getInstance().animPather.IsPlaying(pNode._Ports[0].fillValue);
                case GuideStepType.WaitNetEnd:
                    return pNode._Ports[0].fillValue == param.userData;
                case GuideStepType.WaitPanelOpen:
                    return GameInstance.getInstance().uiManager.IsShow((ushort)pNode._Ports[0].fillValue);
                case GuideStepType.SlideCheckDirection:
                    result = OnSlideCheckDirectionSign(pNode, param);
                    if (result)
                    {
                        ResetGuidePanel();
                    }
                    return result;
                case GuideStepType.SlideCheckDirectionImmediately:
                    result = OnSlideCheckDirectionImmediatelySign(pNode, param);
                    if (result)
                    {
                        ResetGuidePanel();
                    }
                    return result;
                case GuideStepType.WaitTweenEnd:
                    return !PositionTweenController.IsPlaying(pNode._Ports[0].fillValue);
                case GuideStepType.WaitUIClose:
                    return !GameInstance.getInstance().uiManager.IsShow((ushort)pNode._Ports[0].fillValue);
                case GuideStepType.WaitAnimPathPlay:
                    return GameInstance.getInstance().animPather.IsPlaying(pNode._Ports[0].fillValue);
                case GuideStepType.WaitUIComponentActive:
                    return OnWaitUIComponentActiveSign(pNode);
                case GuideStepType.WaitTimeLineEnd:
                    return OnWaitTimeLineEndSign(pNode);
                case GuideStepType.WaitUnlockAnimPlayEnd:
                    return !GameInstance.getInstance().unlockMgr.IsPlayingUnLockTipsTween();
                case GuideStepType.WaitComponentLoadCompleted:
                    return OnWaitComponentLoadCompletedSign(pNode);
                case GuideStepType.WaitDialogEnd:
                    return WaitDialogEnd();
                case GuideStepType.WaitCameraTweenEnd:
                    return WaitCameraTweenEnd(pNode);
                case GuideStepType.WaitEnemyInVisible:
                    return WaitEnemyInVisible(pNode);
                case GuideStepType.WaitBattleBegin:
                    return WaitBattleBegin(pNode);
                case GuideStepType.WaitBattleEnd:
                    return WaitBattleEnd(pNode);
                case GuideStepType.WaitBattleResult:
                    return WaitBattleResult(pNode);
                case GuideStepType.WaitHeroSkillCanUse:
                    return WaitHeroSkillCanUse(pNode);
                case GuideStepType.CheckOnlyShowUI:
                    return CheckOnlyShowUI(pNode);
                case GuideStepType.WaitGameobjectActive:
                    return WaitGameobjectActive(pNode);
                case GuideStepType.WaitGameobjectCanClick:
                    return WaitGameobjectCanClick(pNode);
                default:
                    break;
            }
			return false;
		}
        //------------------------------------------------------
        /// <summary>
        /// 控件点击判断逻辑
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static bool OnClickUISign(StepNode pNode, CallbackParam param)
        {
            if (param.triggerType == EUIWidgetTriggerType.Click)
            {
                //按任意键功能
                if (pNode._Ports.Count > 8)
                {
                    bool isPressAnyKey = pNode._Ports[8].fillValue == 1;
                    if (isPressAnyKey)
                    {
                        ResetGuidePanel();
                        return true;
                    }
                }
                //动态列表点击
                if (param.listIndex != -1)
                {
                    if (pNode._Ports[0].fillValue == param.widgetGuid && pNode._Ports[1].fillValue -1 == param.listIndex)
                    {
                        ResetGuidePanel();
                        return true;
                    }
                }
                else
                {
                    if (pNode._Ports[0].fillValue == param.widgetGuid)//点击到指定guidUI
                    {
                        GuideGuid widget = GuideGuidUtl.FindGuide(param.widgetGuid);

                        ResetGuidePanel();
                        return true;
                    }
                }
            }
            return false;
        }

        //-------------------------------------------
        public static void OnNodeAutoNext(StepNode pNode)
        {
            if (pNode._Ports == null || pNode._Ports.Count <= 0) return;
            if (pNode.IsAutoNext() && pNode.GetAutoNextTime() > 0)
            {
                switch ((GuideStepType)pNode.type)
                {
                    case GuideStepType.ClickUI:
                        {
                            if (pNode._Ports.Count > 9)
                            {
                                pNode._Ports[9].fillValue = 1;//如果自动跳转了,那么设置节点状态为1
                            }
                        }
                        break;
                }

                //步骤节点完成后,要跳转到下一个节点时,都进行guide界面的清理
                ResetGuidePanel();
            }
        }
        //-------------------------------------------
        public static void OnGuideNode(StepNode pNode)
        {
            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }
            if (guidePanel.bDoing)
            {
                return;
            }
            
            if (TopGame.Core.DebugConfig.bGuideLogEnable)
            {
                Framework.Plugin.Logger.Warning(CommonUtility.stringBuilder.Append("OnGuideNode GuideStepHandler pNode.type= ").Append(pNode.type).ToString());
            }
            
            switch ((GuideStepType)pNode.type)
            {
                case GuideStepType.ClickUI:
                    OnClickUIExcude(pNode);
                    //重置步骤节点是否自动跳转状态
                    if (pNode._Ports!=null && pNode._Ports.Count > 9)
                    {
                        pNode._Ports[9].fillValue = 0;
                    }
                    guidePanel.bDoing = true;
                    break;
                case GuideStepType.Slide:
                    OnSliderExcude(pNode);
                    guidePanel.bDoing = true;
                    break;
                case GuideStepType.ClickZoom:
                    OnSlickZoomExcude(pNode);
                    guidePanel.bDoing = true;
                    break;
                case GuideStepType.ClickAnywhere:
                    OnClickAnywhereExcude(pNode);
                    guidePanel.bDoing = true;
                    break;
                case GuideStepType.SlideCheckDirection:
                    OnSliderCheckDirectionExcude(pNode);
                    guidePanel.bDoing = true;
                    break;
                case GuideStepType.SlideCheckDirectionImmediately:
                    OnSliderCheckDirectionImmediatelyExcude(pNode);
                    guidePanel.bDoing = true;
                    break;
                default:
                    break;
            }
            
        }
        //-------------------------------------------
        public static bool OnClickZoomSign(StepNode pNode, CallbackParam param)
        {
            bool is3D = pNode._Ports[2].fillValue == 1;
            int x = pNode._Ports[3].fillValue;
            int y = pNode._Ports[4].fillValue;
            int radius = pNode._Ports[5].fillValue;
            Vector2 mousePos = MousePosToScreenPos(param.mousePos);//鼠标点击是以左下角为坐标原点,图标显示是以屏幕中心为原点,这边将鼠标原点从左下角,偏移到屏幕中心
            if (TopGame.Core.DebugConfig.bGuideLogEnable)
                Framework.Plugin.Logger.Warning(CommonUtility.stringBuilder.Append("区域点击,鼠标位置").Append(mousePos ).Append(",点击位置:").Append(x).Append(",").Append(y).ToString());
            if (is3D)
            {
                
                int z = y;
                y = 0;
                Vector3 worldPos = GameInstance.getInstance().uiManager.GetUICamera().ScreenToWorldPoint(mousePos);
                if ((worldPos.x - x) * (worldPos.x - x) + (worldPos.y - y)* (worldPos.y - y) + (worldPos.z - z)* (worldPos.z - z) <= radius * radius)
                {
                    GameInstance.getInstance().OnTouchBegin(new ATouchInput.TouchData() { touchID = 1, position = m_BeginMousePos });
                    GameInstance.getInstance().OnTouchEnd(new ATouchInput.TouchData() { touchID = 1, position = param.mousePos });
                    return true;
                }
            }
            else
            {
                if ((mousePos.x - x) * (mousePos.x - x) + (mousePos.y - y) * (mousePos.y - y) <= radius * radius)
                {
                    GameInstance.getInstance().OnTouchBegin(new ATouchInput.TouchData() { touchID = 1, position = m_BeginMousePos });
                    GameInstance.getInstance().OnTouchEnd(new ATouchInput.TouchData() { touchID = 1, position = param.mousePos });
                    return true;
                }
            }
            return false;
        }
        //-------------------------------------------
        public static bool OnSlideSign(StepNode pNode, CallbackParam param)
        {
            bool isStartPos3D = pNode._Ports[2].fillValue == 1;
            int startPosX = pNode._Ports[3].fillValue;
            int startPosY = pNode._Ports[4].fillValue;
            int startRadius = pNode._Ports[5].fillValue;

            bool isEndPos3D = pNode._Ports[6].fillValue == 1;
            int endPosX = pNode._Ports[7].fillValue;
            int endPosY = pNode._Ports[8].fillValue;
            int endRadius = pNode._Ports[9].fillValue;

            Vector2 mousePos = MousePosToScreenPos(param.mousePos);


            //Framework.Plugin.Logger.Warning("param.touchType=" + param.touchType);

            if (param.touchType == ETouchType.Begin)
            {
                m_TouchBeginPos = MousePosToScreenPos(param.mousePos);
            }
            else if(param.touchType == ETouchType.End)
            {
                bool isStartTouchPass = false;

                if (isStartPos3D)
                {
                    int z = startPosY;
                    startPosY = 0;
                    Vector3 worldPos = GameInstance.getInstance().uiManager.GetUICamera().ScreenToWorldPoint(m_TouchBeginPos);
                    if ((worldPos.x - startPosX) * (worldPos.x - startPosX) + (worldPos.y - startPosY) * (worldPos.y - startPosY) + (worldPos.z - z) * (worldPos.z - z) <= startRadius * startRadius)
                    {
                        isStartTouchPass = true;
                    }
                }
                else
                {
                    if (TopGame.Core.DebugConfig.bGuideLogEnable)
                        Framework.Plugin.Logger.Warning(CommonUtility.stringBuilder.Append("start click value =").Append(",(").Append(startPosX ).Append("," ).Append(startPosY ).Append(")," ).Append( ((m_TouchBeginPos.x - startPosX) * (m_TouchBeginPos.x - startPosX))).Append((m_TouchBeginPos.y - startPosY) * (m_TouchBeginPos.y - startPosY)).Append(",radius=").Append(startRadius * startRadius).ToString());
                    if ((m_TouchBeginPos.x - startPosX) * (m_TouchBeginPos.x - startPosX) + (m_TouchBeginPos.y - startPosY) * (m_TouchBeginPos.y - startPosY) <= startRadius * startRadius)
                    {
                        isStartTouchPass = true;
                    }
                }
                if (isStartTouchPass)
                {
                    if (isEndPos3D)
                    {
                        int z = endPosY;
                        endPosY = 0;
                        Vector3 worldPos = GameInstance.getInstance().uiManager.GetUICamera().ScreenToWorldPoint(mousePos);
                        if ((worldPos.x - endPosX) * (worldPos.x - endPosX) + (worldPos.y - endPosY) * (worldPos.y - endPosY) + (worldPos.z - z) * (worldPos.z - z) <= endRadius * endRadius)
                        {
                            GameInstance.getInstance().OnTouchBegin(new ATouchInput.TouchData() { touchID = 1, position = m_BeginMousePos });
                            GameInstance.getInstance().OnTouchEnd(new ATouchInput.TouchData() { touchID = 1, position = param.mousePos });
                            return true;
                        }
                    }
                    else
                    {
                        //if (TopGame.Core.DebugConfig.bGuideLogEnable)
                            //Framework.Plugin.Logger.Warning("end click value =" + mousePos + "," + ((mousePos.x - endPosX) * (mousePos.x - endPosX) + (mousePos.y - endPosY) * (mousePos.y - endPosY)) + ",radius=" + endRadius * endRadius);
                        if ((mousePos.x - endPosX) * (mousePos.x - endPosX) + (mousePos.y - endPosY) * (mousePos.y - endPosY) <= endRadius * endRadius)
                        {
                            GameInstance.getInstance().OnTouchBegin(new ATouchInput.TouchData() { touchID = 1, position = m_BeginMousePos });
                            GameInstance.getInstance().OnTouchEnd(new ATouchInput.TouchData() { touchID = 1, position = param.mousePos });
                            return true;
                        }
                    }
                    
                }
                return false;
            }
            return false;
        }
        //-------------------------------------------
        /// <summary>
        /// UI屏幕以中心点为0,0点,鼠标以左下角为0,0点,将鼠标位置转换到以屏幕坐标系为准
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        public static Vector2 MousePosToScreenPos(Vector2 mousePos)
        {
            return mousePos - new Vector2(Screen.width / 2f, Screen.height / 2f);
        }
        //-------------------------------------------
        public static bool OnSlideCheckDirectionSign(StepNode pNode, CallbackParam param)
        {
            //bool isStartPos3D = pNode._Ports[2].fillValue == 1;
            int startPosX = pNode._Ports[0].fillValue;
            int startPosY = pNode._Ports[1].fillValue;

            //bool isEndPos3D = pNode._Ports[6].fillValue == 1;
            int endPosX = pNode._Ports[2].fillValue;
            int endPosY = pNode._Ports[3].fillValue;

            int checkAngle = pNode._Ports[5].fillValue;

            Vector2 mousePos = MousePosToScreenPos(param.mousePos);//UI屏幕以中心点为0,0点,鼠标以左下角为0,0点,将鼠标位置转换到以屏幕坐标系为准

            if (param.touchType == ETouchType.Begin)
            {
                m_TouchBeginPos = MousePosToScreenPos(param.mousePos);
            }
            else if (param.touchType == ETouchType.End)
            {
                Vector2 endPos = MousePosToScreenPos(param.mousePos);
                Vector2 sourceDirection = new Vector2(endPosX, endPosY) - new Vector2(startPosX,startPosY);
                Vector2 destinationDirection = endPos - m_TouchBeginPos;

                float angle = Vector2.Angle(sourceDirection, destinationDirection);
                //float dot = Vector2.Dot(sourceDirection.normalized, destinationDirection.normalized);
                //float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                //if (TopGame.Core.DebugConfig.bGuideLogEnable)
                    //Framework.Plugin.Logger.Warning("angle=" + angle + ",m_TouchBeginPos:" + m_TouchBeginPos + ",param.mousePos:" + endPos + ",setStart:" + new Vector2(startPosX, startPosY) + ",setEnd:" + new Vector2(endPosX, endPosY));
                if (angle <= checkAngle)
                {
                    return true;
                }

                return false;
            }
            return false;
        }
        //-------------------------------------------
        public static void OnSliderCheckDirectionExcude(StepNode pNode)
        {
            int startPosX = pNode._Ports[0].fillValue;
            int startPosY = pNode._Ports[1].fillValue;

            int endPosX = pNode._Ports[2].fillValue;
            int endPosY = pNode._Ports[3].fillValue;

            int checkAngle = pNode._Ports[5].fillValue;

            int sliderSpeed = 100;
            if (pNode._Ports.Count >= 5)
            {
                sliderSpeed = pNode._Ports[4].fillValue;
                if (sliderSpeed == 0)
                {
                    sliderSpeed = 100;
                }
            }

            Vector2 startPos = new Vector2(startPosX, startPosY);
            Vector2 endPos = new Vector2(endPosX, endPosY);

            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }
            guidePanel.Show();
            guidePanel.SlideFinger(startPos, endPos, sliderSpeed);
        }
        //-------------------------------------------
        public static bool OnSlideCheckDirectionImmediatelySign(StepNode pNode, CallbackParam param)
        {
            //bool isStartPos3D = pNode._Ports[2].fillValue == 1;
            int startPosX = pNode._Ports[0].fillValue;
            int startPosY = pNode._Ports[1].fillValue;

            //bool isEndPos3D = pNode._Ports[6].fillValue == 1;
            int endPosX = pNode._Ports[2].fillValue;
            int endPosY = pNode._Ports[3].fillValue;

            int checkAngle = pNode._Ports[5].fillValue;

            Vector2 mousePos = MousePosToScreenPos(param.mousePos);//UI屏幕以中心点为0,0点,鼠标以左下角为0,0点,将鼠标位置转换到以屏幕坐标系为准

            if (param.touchType == ETouchType.Begin)
            {
                m_TouchBeginPos = MousePosToScreenPos(param.mousePos);
            }
            else if (param.touchType == ETouchType.Move)
            {
                Vector2 endPos = MousePosToScreenPos(param.mousePos);
                Vector2 sourceDirection = new Vector2(endPosX, endPosY) - new Vector2(startPosX, startPosY);
                Vector2 destinationDirection = endPos - m_TouchBeginPos;

                float angle = Vector2.Angle(sourceDirection, destinationDirection);
                //float dot = Vector2.Dot(sourceDirection.normalized, destinationDirection.normalized);
                //float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                //if (TopGame.Core.DebugConfig.bGuideLogEnable)
                    //Framework.Plugin.Logger.Warning("angle=" + angle + ",m_TouchBeginPos:" + m_TouchBeginPos + ",param.mousePos:" + endPos + ",setStart:" + new Vector2(startPosX, startPosY) + ",setEnd:" + new Vector2(endPosX, endPosY));
                if (angle <= checkAngle)
                {
                    return true;
                }

                return false;
            }
            return false;
        }
        //-------------------------------------------
        public static void OnSliderCheckDirectionImmediatelyExcude(StepNode pNode)
        {
            OnSliderCheckDirectionExcude(pNode);
        }
        //-------------------------------------------
        public static void OnClickUIExcude(StepNode pNode)
        {
            if (pNode._Ports.Count == 0)
            {
                return;
            }
            int guid = pNode._Ports[0].fillValue;
            GuideGuid guideGuid = GuideGuidUtl.FindGuide(guid);
            int touchType = pNode._Ports[2].fillValue;
            int angle = pNode._Ports[3].fillValue;
            int offsetX = pNode._Ports[4].fillValue;
            int offsetY = pNode._Ports[5].fillValue;
            int clickIndex = pNode._Ports[1].fillValue;

            bool bMostTop = true;
            if (pNode._Ports.Count > 6)
                bMostTop = pNode._Ports[6].fillValue != 0;
            string listenerName = "";
            if (pNode._Ports.Count > 7)
                listenerName = pNode._Ports[7].strValue;

            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }

            //如果是3d,那么设置点击组件位置到手指位置,点击组件挂载 EventTriggerListener ,并且调用 SetGuideGuid 设置guid对象进行模拟点击guid的功能
            guidePanel.SetCurStepNode(pNode);
            guidePanel.WidgetListen(pNode,(EFingerType)touchType, new Vector3(0, 0, angle), new Vector2(offsetX, offsetY), guid, clickIndex, bMostTop, listenerName);
        }
        //-------------------------------------------
        public static GuidePanel GetGuidePanel()
        {
            GuidePanel uIBase = UIKits.CastGetUI<GuidePanel>(); 
            if (uIBase == null)
            {
                return null;
            }
            return uIBase;
        }
        //------------------------------------------------------
        public static void ResetGuidePanel()
        {
            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }
            guidePanel.ClearData();
        }
        //-------------------------------------------
        public static void OnSliderExcude(StepNode pNode)
        {
            bool isStartPos3D = pNode._Ports[2].fillValue == 1;
            int startPosX = pNode._Ports[3].fillValue;
            int startPosY = pNode._Ports[4].fillValue;
            int startRadius = pNode._Ports[5].fillValue;

            bool isEndPos3D = pNode._Ports[6].fillValue == 1;
            int endPosX = pNode._Ports[7].fillValue;
            int endPosY = pNode._Ports[8].fillValue;
            int endRadius = pNode._Ports[9].fillValue;

            int sliderSpeed = 100;
            if (pNode._Ports.Count >= 11)
            {
                sliderSpeed = pNode._Ports[10].fillValue;
            }
            

            Vector2 startPos = new Vector2(startPosX, startPosY);
            Vector2 endPos = new Vector2(endPosX, endPosY);

            if (isStartPos3D)
            {
                Vector3 start3DPos = new Vector3(startPos.x,0, startPos.y);
                start3DPos = GameInstance.getInstance().uiManager.GetUICamera().WorldToScreenPoint(start3DPos);
                startPos = new Vector2(start3DPos.x, start3DPos.y);
            }

            if (isEndPos3D)
            {
                Vector3 end3DPos = new Vector3(endPos.x, 0, endPos.y);
                end3DPos = GameInstance.getInstance().uiManager.GetUICamera().WorldToScreenPoint(end3DPos);
                endPos = new Vector2(end3DPos.x, end3DPos.y);
            }

            
            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }
            guidePanel.Show();
            guidePanel.SlideFinger(startPos, endPos, sliderSpeed);
        }
        //-------------------------------------------
        public static void OnSlickZoomExcude(StepNode pNode)
        {
            int touchType = pNode._Ports[0].fillValue;
            int angle = pNode._Ports[1].fillValue;
            bool is3D = pNode._Ports[2].fillValue == 1;
            int posX = pNode._Ports[3].fillValue;
            int posY = pNode._Ports[4].fillValue;
            int radius = pNode._Ports[5].fillValue;

            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }
            guidePanel.Show();
            guidePanel.ClickZoom((EFingerType)touchType, new Vector3(0, 0, angle), new Vector3(posX, posY, 0), is3D, radius);
        }
        //-------------------------------------------
        public static bool OnClickAnywhereSign(StepNode pNode, CallbackParam param)
        {
            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return false;
            }

            if (param.triggerType == EUIWidgetTriggerType.Click /*|| param.touchType == ETouchType.End*/)//为了防止对话跳过问题,这边只监听点击操作,否则会在松开手指后,触发两次
            {
                if (guidePanel.IsShowingDialog)
                {
                    guidePanel.OnDialogClick(); 
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        //-------------------------------------------
        public static void OnClickAnywhereExcude(StepNode pNode)
        {
            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }
            guidePanel.Show();
        }
        //------------------------------------------------------
        public static bool OnWaitUIComponentActiveSign(StepNode pNode)
        {
            int guid = pNode._Ports[0].fillValue;
            bool isCheckEnable = pNode._Ports[2].fillValue == 1;
            bool isCheckScale = pNode._Ports[3].fillValue == 1;
            int scaleX = pNode._Ports[4].fillValue;
            int scaleY = pNode._Ports[5].fillValue;
            int scaleZ = pNode._Ports[6].fillValue;
            bool isCheckOutScreen = pNode._Ports[7].fillValue == 1;
            bool isCheckInterval = pNode._Ports[8].fillValue == 1;
            int intervalTime = pNode._Ports[9].fillValue;
            int intervalCount = pNode._Ports[10].fillValue;

            GuideGuid guideGuid = GuideGuidUtl.FindGuide(guid);
            if (guideGuid == null)
            {
                Framework.Plugin.Logger.Warning(CommonUtility.stringBuilder.Append("找不到控件id:").Append(guid).ToString());
                if (isCheckInterval)
                {
                    //todo:找不到的时候,根据是否检测和检测时间和次数,进行等待执行
                }
                return false;
            }

            int scaleFlag = -1;
            if (isCheckScale)
            {
                Vector3 scale = guideGuid.gameObject.transform.localScale;//由于UI的canvas缩放设置的很小的原因,导致全局缩放值接近与0,所以只使用本地缩放
                //Debug.LogError("scale:" + scale);
                if (Mathf.Abs(scale.x - scaleX) < 0.001f && Mathf.Abs(scale.y - scaleY) < 0.001f && Mathf.Abs(scale.z - scaleZ) < 0.001f)
                {
                    scaleFlag = 1;
                }
                else
                {
                    scaleFlag = 0;
                }
            }
            int enableFlag = -1;
            if (isCheckEnable)
            {
                enableFlag = guideGuid.gameObject.activeInHierarchy ? 1 :0;
            }

            int outScreenFlag = -1;
            if (isCheckOutScreen)
            {
                Vector2 resolution = GameInstance.getInstance().uiManager.GetReallyResolution();
                Vector3 pos = guideGuid.gameObject.transform.localPosition;
                if (pos.x < resolution.x && pos.x > 0 && pos.y > 0 && pos.y < resolution.y)
                {
                    outScreenFlag = 1;
                }
                else
                {
                    outScreenFlag = 0;
                }
            }


            return scaleFlag != 0 && enableFlag != 0 && outScreenFlag != 0;
        }
        //------------------------------------------------------
        public static bool OnWaitTimeLineEndSign(StepNode pNode)
        {
            if (pNode == null)
            {
                return false;
            }

            int guid = pNode._Ports[0].fillValue;
            GuideGuid guideGuid = GuideGuidUtl.FindGuide(guid);
            if (guideGuid == null)
            {
                Framework.Plugin.Logger.Warning(CommonUtility.stringBuilder.Append("找不到控件id:").Append(guid).ToString());
                //todo:找不到的时候,根据是否检测和检测时间和次数,进行等待执行
                return false;
            }
            PlayableDirector playable = guideGuid.GetComponent<PlayableDirector>();
            if (playable)
            {
                //Debug.Log("playable.time:" + playable.time);
                return playable.time >= playable.duration;
            }
            return false;
        }
        //------------------------------------------------------
        private static bool OnWaitComponentLoadCompletedSign(StepNode pNode)
        {
            if (pNode == null)
            {
                return false;
            }

            int guid = pNode._Ports[0].fillValue;
            GuideGuid guideGuid = GuideGuidUtl.FindGuide(guid);
            if (guideGuid == null)
            {
                Framework.Plugin.Logger.Warning(CommonUtility.stringBuilder.Append("找不到控件id:").Append(guid).Append(", 跳过等待组件加载完成步骤").ToString());
                return false;
            }

            ListView listView = guideGuid.GetComponent<ListView>();
            if (listView)
            {
                return listView.GetIsLoadCompleted();
            }

            return false;
        }
        //------------------------------------------------------
        private static bool WaitDialogEnd()
        {
            DialogPanel dialog = GameInstance.getInstance().uiManager.CastGetUI<DialogPanel>((ushort)EUIType.DialogPanel, false);
            if (dialog != null)
            {
                return !dialog.bDialoging;
            }
            return true;
        }
        //------------------------------------------------------
        private static bool WaitCameraTweenEnd(StepNode pNode)
        {
            if (pNode == null)
            {
                return false;
            }

            float factor = 0.1f;
            if(pNode._Ports.Count>0) factor = pNode._Ports[0].fillValue / 100f;
            if (TopGame.Core.CameraController.getInstance() == null) return true;
            return !TopGame.Core.CameraController.getInstance().IsTweenEffecting(factor);
        }
        //------------------------------------------------------
        private static bool WaitEnemyInVisible(StepNode pNode)
        {
            BattleRegions region = BattleKits.GetBattleLogic<BattleRegions>(GameInstance.getInstance());
            if(region!=null)
            {
                return region.IsVisibleInRegion();
            }
            return true;
        }
        //------------------------------------------------------
        private static bool WaitBattleBegin(StepNode pNode)
        {
            Battle battle = StateFactory.CurrentState() as Battle;
            if (battle == null) return true;
            if (!battle.IsEnable()) return false;
            return BattleKits.IsStartingAndActive(GameInstance.getInstance());
        }
        //------------------------------------------------------
        private static bool WaitBattleEnd(StepNode pNode)
        {
            Battle battle = StateFactory.CurrentState() as Battle;
            if (battle == null) return true;
            if (!battle.IsEnable()) return false;
            IMode mode = battle.GetActivedMode();
            if (mode == null) return true;
            BattleRegions region = BattleKits.GetBattleLogic<BattleRegions>(GameInstance.getInstance());
            if (region != null) pNode._Ports[0].fillValue = region.GetCurWave();
            else pNode._Ports[0].fillValue = 0;
            return BattleKits.IsOvered(GameInstance.getInstance());
        }
        //------------------------------------------------------
        private static bool WaitBattleResult(StepNode pNode)
        {
            Battle battle = StateFactory.CurrentState() as Battle;
            if (battle == null) return true;
            if (!battle.IsEnable()) return false;
            IMode mode = battle.GetActivedMode();
            if (mode == null) return true;
            if (battle.GetNetBattleResult() == EBttleExitType.None) return false;
            pNode._Ports[0].fillValue = (int)battle.GetNetBattleResult();
            return mode.IsEnabled() && battle.GetNetBattleResult() != EBttleExitType.None;
        }
        //------------------------------------------------------
        private static bool WaitHeroSkillCanUse(StepNode pNode)
        {
            int id = pNode._Ports[0].fillValue;
            int skillTag = 0;
            if (pNode._Ports.Count > 1)
            {
                skillTag = pNode._Ports[1].fillValue; 
            }
            
            AbsMode mode = Battle.CastCurrentMode<AbsMode>();
            if (mode != null)
            {
                var players = mode.GetPlayers();
                if (players == null)
                {
                    return false;
                }
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i] == null || players[i].GetActorParameter() == null)
                    {
                        continue;
                    }
                    if (players[i].GetConfigID() == id)
                    {
                        BattleSkillInformations runskill = players[i].GetActorParameter().GetSkill() as BattleSkillInformations;
                        if (runskill == null)
                        {
                            continue;
                        }
                        Skill currentSkill = null;
                        if (skillTag != 0)
                            currentSkill = runskill.GetSkillByTag((uint)skillTag);
                        else currentSkill = runskill.GetPowerSkill();
                        if (currentSkill == null) return false;
                           
                        return runskill.CanSkill(currentSkill.nGuid, false, false);
                    }
                }
            }
            return false;
        }
        //------------------------------------------------------
        private static bool CheckOnlyShowUI(StepNode pNode)
        {
            if (!ModuleManager.startUpGame) return true;
            if (GameInstance.getInstance().uiManager == null) return true;
            bool bHas = false;
            Dictionary<ushort, UIBase> uis = GameInstance.getInstance().uiManager.GetUIS();
            foreach (var db in uis)
            {
                if (db.Key == (ushort)EUIType.GuidePanel || db.Key == (ushort)EUIType.GameInfo) continue;
                if (!db.Value.IsVisible() || db.Value.GetAlwayShow())
                    continue;
                bHas = false;
                for(int i = 0; i < pNode._Ports.Count; ++i)
                {
                    if(pNode._Ports[i].fillValue>0 && pNode._Ports[i].fillValue == db.Key)
                    {
                        bHas = true;
                        break;
                    }
                }
                if (!bHas)
                {
                    return false;
                }
            }
            return true;
        }
        //------------------------------------------------------
        private static bool WaitGameobjectActive(StepNode pNode)
        {
            GuideGuid guide = GuideGuidUtl.FindGuide(pNode._Ports[0].fillValue);
            if (guide == null)//找不到情况,结束当前等待
            {
                return true;
            }
            bool bActive = guide.gameObject.activeInHierarchy;
            bool waitState = pNode._Ports[1].fillValue == 1;
            return bActive == waitState;
        }
        //------------------------------------------------------
        private static bool WaitGameobjectCanClick(StepNode pNode)
        {
            GuideGuid guide = GuideGuidUtl.FindGuide(pNode._Ports[0].fillValue);
            if (guide == null)//找不到情况,结束当前等待
            {
                return true;
            }

            Vector3 screenPos = Vector2.zero;
            Camera cam = TopGame.Core.CameraController.MainCamera;

            if (cam != null)
            {
                screenPos = cam.WorldToScreenPoint(guide.transform.position);
            }

            bool bClick = !UIUtil.IsPointOverUI(screenPos);
            return bClick;
        }
    }
}