/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GuideExcudesHandler
作    者:	HappLI
描    述:	
*********************************************************************/

using Framework.Core;
using System;
using System.Collections.Generic;
using TopGame;
using TopGame.Core;
using TopGame.Data;
using TopGame.Logic;
using TopGame.Net;
using TopGame.SvrData;
using TopGame.UI;
using UnityEngine;

namespace Framework.Plugin.Guide
{
    public class GuideExcudesHandler
    {

        public static bool OnNodeAutoNext(ExcudeNode pNode)
        {
            return true;
        }
        //-------------------------------------------
        public static bool OnGuideSign(ExcudeNode pNode, CallbackParam param)
		{
            return true;
		}
        //-------------------------------------------
        public static bool OnGuideNode(ExcudeNode pNode)
        {
            if (DebugConfig.bGuideLogEnable)
            {
                Framework.Plugin.Logger.Warning(CommonUtility.stringBuilder.Append("OnGuideNode GuideExcudesHandler " ).Append(",类型:").Append(pNode.type).ToString());
            }
            switch ((GuideExcudeType)pNode.type)
            {
                case GuideExcudeType.MaskAble:
                    ExcudeMaskNode(pNode);
                    break;
                case GuideExcudeType.Tips:
                    ExcudeTipsNode(pNode);
                    break;
                case GuideExcudeType.TipsByGUI:
                    ExcudeTipsNodeByGUI(pNode);
                    break;
                case GuideExcudeType.TestWidget:
                    ExcudeTestWidget(pNode);
                    break;
                case GuideExcudeType.IsTriggerGuide:
                    ExcudeIsTriggerGuideNode(pNode);
                    break;
                case GuideExcudeType.UpdateGuideFlag:
                    ExcudeUpdateGuideFlagNode(pNode);
                    break;
                case GuideExcudeType.GetPlayerLevel:
                    ExcudeGetPlayerLevelNode(pNode);
                    break;
                case GuideExcudeType.SetHighlightRect:
                    ExcudeSetHighlightRectNode(pNode);
                    break;
                default:
                    break;
            }
            return true;
        }
        //------------------------------------------------------
        private static void ExcudeGetPlayerLevelNode(ExcudeNode pNode)
        {
            if (pNode == null)
            {
                return;
            }


            var level = UserManager.Current.PlayerLevel;
            pNode._Ports[0].fillValue = level;
        }

        //------------------------------------------------------
        private static void ExcudeUpdateGuideFlagNode(ExcudeNode pNode)
        {
            if (pNode == null)
            {
                return;
            }
            var tag = pNode.GetTag();

            GuideSystem.getInstance().UpdateFlag(tag);
//            NetGuideHandler.Req_SetGuideRequest(tag);
            if (GuideSystem.getInstance().bGuideLogEnable)
            {
                Debug.LogError("保存引导tag:" + tag);
            }
            
        }
        //-------------------------------------------
        public static GuidePanel GetGuidePanel()
        {
            return GameInstance.getInstance().uiManager.CastGetUI<GuidePanel>((ushort)EUIType.GuidePanel);
        }
        //-------------------------------------------
        public static void ExcudeMaskNode(ExcudeNode pNode)
        {
            if (pNode._Ports == null || pNode._Ports.Count == 0)
            {
                Framework.Plugin.Logger.Error("引导组:" + pNode.guideGroup.Guid + ",节点:" + pNode.Guid + ",的端口数据为空!");
                return;
            }
            //显示引导界面
            //打开遮罩
            GuidePanel guidePanel = GetGuidePanel();
            guidePanel.Show();
            int isMask = pNode._Ports[0].fillValue;
            int maskColor = pNode._Ports[1].fillValue;
            bool penetrateEnable = false;
            if (pNode._Ports.Count > 2)
            {
                penetrateEnable = pNode._Ports[2].fillValue == 1;
            }

            int shapeType = 0;
            if (pNode._Ports.Count > 3)
            {
                shapeType = pNode._Ports[3].fillValue;
            }

            int TargetGuid = 0;
            if (pNode._Ports.Count > 4)
            {
                TargetGuid = pNode._Ports[4].fillValue;
            }

            float speed = 0;
            if(pNode._Ports.Count>5)
            {
                speed = pNode._Ports[5].fillValue*0.001f;
            }

            Color col = new Color(
            ((maskColor & 0x00ff0000) >> 16) / 255f,
            ((maskColor & 0x0000ff00) >> 8) / 255f,
            ((maskColor & 0x000000ff)) / 255f,
            ((maskColor & 0xff000000) >> 24) / 255f);
            if (guidePanel != null)
            {
                guidePanel.SetMaskSpeed(speed);
                guidePanel.SetMaskShape((EMaskType)shapeType);
                guidePanel.SetMaskActive(isMask == 1);
                guidePanel.SetMaskColor(col);
                guidePanel.SetPenetrateEnable(penetrateEnable, TargetGuid);
            }
        }
        //-------------------------------------------
        public static void ExcudeTipsNode(ExcudeNode pNode)
        {
            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }
            int bgType = pNode._Ports[0].fillValue;
            int contentID = pNode._Ports[1].fillValue;
            int color_Int = pNode._Ports[2].fillValue;
            bool is3D = pNode._Ports[3].fillValue == 1;

            Vector3 pos = new Vector3(pNode._Ports[4].fillValue, pNode._Ports[5].fillValue, pNode._Ports[6].fillValue);
            bool isTransition = pNode._Ports[7].fillValue == 1;
            int speed = pNode._Ports[8].fillValue;
            bool enableArrow = true;
            if (pNode._Ports.Count > 9)
            {
                enableArrow = pNode._Ports[9].fillValue == 1;
            }


            //人物背景
            bool enableAvatar = true;
            if (pNode._Ports.Count > 10)
            {
                enableAvatar = pNode._Ports[10].fillValue == 1;
            }

            Color color = new Color(
            ((color_Int & 0x00ff0000) >> 16) / 255f,
            ((color_Int & 0x0000ff00) >> 8) / 255f,
            ((color_Int & 0x000000ff)) / 255f,
            ((color_Int & 0xff000000) >> 24) / 255f);

            guidePanel.Show();
            guidePanel.AddTipDock((EDescBGType)bgType, contentID, color, isTransition ? speed : 0, 0, pos, is3D, 0, 0,"", enableArrow);
            guidePanel.SetAvatarEnable(enableAvatar);
        }
        //-------------------------------------------
        public static void ExcudeTipsNodeByGUI(ExcudeNode pNode)
        {
            int bgType = pNode._Ports[0].fillValue;
            int contentID = pNode._Ports[1].fillValue;
            int color_Int = pNode._Ports[2].fillValue;
            int guid = pNode._Ports[3].fillValue;
            float offsetX = pNode._Ports[4].fillValue;
            float offsetY = pNode._Ports[5].fillValue;
            bool isTransition = pNode._Ports[6].fillValue == 1;
            int speed = pNode._Ports[7].fillValue;
            int widgetIndex = pNode._Ports[8].fillValue;
            int autoHideTime = 0;
            if (pNode._Ports.Count >9)
            {
                autoHideTime = pNode._Ports[9].fillValue;
            }
            string searchListenerName = "";
            if (pNode._Ports.Count > 10)
            {
                searchListenerName = pNode._Ports[10].strValue;
            }

            //设置对话箭头状态
            bool enableArrow = true;
            if (pNode._Ports.Count > 11)
            {
                enableArrow = pNode._Ports[11].fillValue == 1;
            }

            //人物背景
            bool enableAvatar = true;
            if (pNode._Ports.Count > 12)
            {
                enableAvatar = pNode._Ports[12].fillValue == 1;
            }

            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }

            Color color = new Color(
            ((color_Int & 0x00ff0000) >> 16) / 255f,
            ((color_Int & 0x0000ff00) >> 8) / 255f,
            ((color_Int & 0x000000ff)) / 255f,
            ((color_Int & 0xff000000) >> 24) / 255f);

            guidePanel.Show();
            guidePanel.AddTipDock((EDescBGType)bgType, contentID, color,isTransition?speed:0, autoHideTime, new Vector3(offsetX, offsetY, 0), false, guid, widgetIndex, searchListenerName, enableArrow);
            guidePanel.SetAvatarEnable(enableAvatar);
        }
        
        //-------------------------------------------
        public static Vector3 GetGuideGuidPos(int guid)
        {
            GuideGuid guide = GuideGuidUtl.FindGuide(guid);
            if (guide == null)
            {
                return Vector3.zero;
            }

            return guide.transform.localPosition;
        }
        //-------------------------------------------
        public static void ExcudeTestWidget(ExcudeNode pNode)
        {
            int guid = pNode._Ports[0].fillValue;
            GuideGuid guide = GuideGuidUtl.FindGuide(guid);
            if (guide == null)
            {
                return;
            }
            if (!GuideUtility.IsCheckInView(guide.transform) || GuideUtility.IsCheckTweening(guide.transform))
                return;

            EventTriggerListener.Get(guide.gameObject).onClick?.Invoke(guide.gameObject,null);
        }
        //-------------------------------------------
        public static void ExcudePlayCameraAniNode(ExcudeNode pNode)
        {
            int id = pNode._Ports[0].fillValue;
            int guid = pNode._Ports[1].fillValue;
            Vector3 offset = GameInstance.getInstance().GetPlayerPosition();
            Transform trs = DyncmicTransformCollects.FindTransformByGUID(guid);
            if (trs)
            {
                TopGame.Core.CameraController.getInstance().PlayAniPath(id, trs.gameObject, true, offset.x, offset.y,offset.z);
            }
            else
            {
                CameraController.getInstance().PlayAniPath(id, null, true, offset.x, offset.y, offset.z);
            }
            
        }
        //------------------------------------------------------
        static uint GetMainHeroBodyID(uint configID)
        {
            var cfg = DataManager.getInstance().Player.GetData(configID);
            if (cfg == null)
            {
                return 0;
            }

            return cfg.heroNoumenon;
        }
        //------------------------------------------------------
        public static void ExcudeSetDialogArrowNode(ExcudeNode pNode)
        {
            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }
            int widgetID = pNode._Ports[0].fillValue;
            int offsetX = pNode._Ports[1].fillValue;
            int offsetY = pNode._Ports[2].fillValue;
            int angle = pNode._Ports[3].fillValue;
            bool isReverse = pNode._Ports[4].fillValue == 1;

            guidePanel.SetDialogArrow(widgetID,new Vector2(offsetX,offsetY),angle, isReverse);
        }
        //------------------------------------------------------
        public static void ExcudeEventTriggersNode(ExcudeNode pNode)
        {
            int eventID = pNode._Ports[0].fillValue;
            GameInstance.getInstance().OnTriggerEvent(eventID);
        }
        //------------------------------------------------------
        public static void ExcudeSetGuideImageNode(ExcudeNode pNode)
        {
            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }

            string strPath = pNode._Ports[0].strValue;
            bool isActive = pNode._Ports[1].fillValue == 1;
            bool isReverse = pNode._Ports[2].fillValue == 1;
            bool isSetNativeSize = pNode._Ports[3].fillValue == 1;
            int widgetID = pNode._Ports[4].fillValue;
            int posX = pNode._Ports[5].fillValue;
            int posY = pNode._Ports[6].fillValue;
            int sizeX = pNode._Ports[7].fillValue;
            int sizeY = pNode._Ports[8].fillValue;
            int rotate = pNode._Ports[9].fillValue;

            Color col = Color.white;
            if (pNode._Ports.Count > 10)
            {
                int maskColor = pNode._Ports[10].fillValue;
                col = new Color(
                ((maskColor & 0x00ff0000) >> 16) / 255f,
                ((maskColor & 0x0000ff00) >> 8) / 255f,
                ((maskColor & 0x000000ff)) / 255f,
                ((maskColor & 0xff000000) >> 24) / 255f);
            }
            
            

            Vector2 pos = Vector2.zero;
            if (widgetID >= 0)
            {
                Vector3 guidPos = GetGuideGuidPos(widgetID);
                pos = new Vector2(guidPos.x, guidPos.y) + new Vector2(posX,posY);
            }
            else
            {
                pos = new Vector2(posX, posY);
            }

            Vector2 size = new Vector2(sizeX, sizeY);

            guidePanel.Show();
            guidePanel.SetGuideImage(strPath, isSetNativeSize, pos, rotate, isReverse, size, isActive, col);
        }
        //-------------------------------------------
        public static void ExcudeIsTriggerGuideNode(ExcudeNode pNode)
        {
            if (pNode._Ports == null || pNode._Ports.Count == 0)
            {
                return;
            }
            int tag = pNode._Ports[0].fillValue;
#if UNITY_EDITOR
            GuideSystem.getInstance().SetTestFlag(false);//节点执行去掉测试开关
#endif
            bool isTrigger = GuideSystem.getInstance().IsTrigged((ushort)tag);
            pNode._Ports[1].fillValue = isTrigger ? 1 : 0;
        }
        //-------------------------------------------
        public static void ExcudeSetHighlightUINode(ExcudeNode pNode)
        {
            int guid = pNode._Ports[0].fillValue;
            bool isHighLight = pNode._Ports[1].fillValue == 1;

            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }

            GuideGuid guideUI = GuideGuidUtl.FindGuide(guid);
            if (guideUI == null)
            {
                return;
            }

            guidePanel.MoveUI(guideUI.gameObject, isHighLight);
        }
        //------------------------------------------------------
        private static void ExcudeSetGuideTextNode(ExcudeNode pNode)
        {
            bool isEnable = pNode._Ports[0].fillValue == 1;
            int textID = pNode._Ports[1].fillValue;
            int posX = pNode._Ports[2].fillValue;
            int posY = pNode._Ports[3].fillValue;
            int sizeX = pNode._Ports[4].fillValue;
            int sizeY = pNode._Ports[5].fillValue;

            int color = pNode._Ports[6].fillValue;
            int fontSize = pNode._Ports[7].fillValue;
            if (fontSize <= 0)
            {
                fontSize = 23;
            }

            Color col = new Color(
            ((color & 0x00ff0000) >> 16) / 255f,
            ((color & 0x0000ff00) >> 8) / 255f,
            ((color & 0x000000ff)) / 255f,
            ((color & 0xff000000) >> 24) / 255f);

            bool isTransition = pNode._Ports[8].fillValue == 1;
            int speed = pNode._Ports[9].fillValue;

            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }

            guidePanel.SetGuideText(new Vector2(sizeX, sizeY), col, new Vector2(posX, posY), fontSize, isEnable, textID, isTransition, speed);
        }
        //------------------------------------------------------
        private static void ExcudeSetContinueImageNode(ExcudeNode pNode)
        {
            bool isEnable = pNode._Ports[0].fillValue == 1;
            int posX = pNode._Ports[1].fillValue;
            int posY = pNode._Ports[2].fillValue;
            int sizeX = pNode._Ports[3].fillValue;
            int sizeY = pNode._Ports[4].fillValue;

            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }

            guidePanel.SetContinueImageEnable(isEnable);
            if (posX != 0 || posY != 0)
            {
                guidePanel.SetContinueImagePos(new Vector2(posX, posY));
            }
            if (sizeX != 0 || sizeY != 0)
            {
                guidePanel.SetContinueImageSize(new Vector2(sizeX, sizeY));
            }
        }
        //------------------------------------------------------
        private static void TouchInputEnableNode(ExcudeNode pNode)
        {
            bool isEnable = false;
            if (pNode._Ports.Count > 0)
            {
                isEnable = pNode._Ports[0].fillValue == 1;
            }
            GuideSystem.getInstance().bIgnoreTouchInput = isEnable;
        }
        //------------------------------------------------------
        private static void ExcudeIsPassAllChapterConditionNode(ExcudeNode pNode)
        {
            //bool isPass = false;
            //BattleDB battleDb = TopGame.SvrData.UserManager.getInstance().mySelf.ProxyDB<BattleDB>(TopGame.Data.EDBType.Battle);
            //isPass = battleDb.IsPassAllChapterCondition();
            //pNode._Ports[0].fillValue = isPass ? 1 : 0;
        }
        //------------------------------------------------------
        private static void ExcudeSetAnchorPositionNode(ExcudeNode pNode)
        {
            int guideID = pNode._Ports[0].fillValue;
            int anchorPosX = pNode._Ports[1].fillValue;
            int anchorPosY = pNode._Ports[2].fillValue;
            GuideGuid widget = GuideGuidUtl.FindGuide(guideID);
            if (widget)
            {
                RectTransform rect = widget.GetComponent<RectTransform>(); ;
                if (rect)
                {
                    rect.anchoredPosition = new Vector2(anchorPosX,anchorPosY);
                }
            }
        }
        //------------------------------------------------------
        private static void ExcudeIsHeroUseSkillNode(ExcudeNode pNode)
        {
            bool canUse = false;
            int id = pNode._Ports[0].fillValue;

            AbsMode mode = Battle.CastCurrentMode<AbsMode>();
            if (mode != null)
            {
                var players = mode.GetPlayers();
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

                        var currentSkill = runskill.GetPowerSkill();
                        if (currentSkill != null)
                        {
                            canUse = currentSkill.GetCDRate(players[i]) <= 0;
                        }
                    }
                }
            }

            pNode._Ports[1].fillValue = canUse ? 1 : 0;
        }
        //------------------------------------------------------
        private static void ExcudeGetLocationStateNode(ExcudeNode pNode)
        {
            pNode._Ports[0].fillValue = (int)UserManager.Current.GetLocationState();
        }
        //------------------------------------------------------
        private static void ExcudeGetUIStateNode(ExcudeNode pNode)
        {
            int id = 0;
            id = pNode._Ports[0].fillValue;

            pNode._Ports[1].fillValue = GameInstance.getInstance().uiManager.IsShow((ushort)id) ? 1:0;
        }
        //------------------------------------------------------
        private static void ExcudeGetGameobjectAcriveNode(ExcudeNode pNode)
        {
            int id = 0;
            id = pNode._Ports[0].fillValue;
            bool bAvtive = false;

            GuideGuid guide = GuideGuidUtl.FindGuide(id);
            if (guide == null)
            {
                bAvtive = false;
            }
            else
            {
                bAvtive = guide.gameObject.activeInHierarchy;//是否显示
            }

            pNode._Ports[1].fillValue = bAvtive ? 1 : 0;
        }
        //------------------------------------------------------
        private static void ExcudeSetHighlightRectNode(ExcudeNode pNode)
        {
            int guide = pNode._Ports[0].fillValue;
            int clickIndex = pNode._Ports[1].fillValue;
            string searchName = pNode._Ports[2].strValue;
            bool bClick = false;
            if (pNode._Ports.Count > 3)
            {
                bClick = pNode._Ports[3].fillValue == 1;
            }

            GuidePanel guidePanel = GetGuidePanel();
            if (guidePanel == null)
            {
                return;
            }

            guidePanel.SetMaskHighLightRect(guide,clickIndex,searchName, bClick);
        }
        //------------------------------------------------------
        /// <summary>
        /// 用来检测目标是否能够被点击到
        /// </summary>
        /// <param name="pNode"></param>
        private static void ExcudeGetTargetCanClickNode(ExcudeNode pNode)
        {
            int guid = pNode._Ports[0].fillValue;
            bool bClick = false;


            GuideGuid guide = GuideGuidUtl.FindGuide(guid);
            if (guide == null)
            {
                bClick = false;
            }
            else
            {
                Vector3 screenPos = Vector2.zero;
                Camera cam = TopGame.Core.CameraController.MainCamera;

                if (cam != null)
                {
                    screenPos = cam.WorldToScreenPoint(guide.transform.position);
                }

                //UI检测
                bClick = !UIUtil.IsPointOverUI(screenPos);//射线要能检测到UI才行
            }

            Debug.Log("guid:" + guid + ",能否点击:" + bClick);

            pNode._Ports[1].fillValue = bClick ? 1 : 0;
        }
    }
}