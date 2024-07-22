#if UNITY_EDITOR
#define USE_GMCONSOLE
#endif
/********************************************************************
生成日期:	6:19:2020 10:06
类    名: 	UIGMPanel
作    者:	JaydenHe
描    述:	控制台界面
*********************************************************************/

using System.Collections.Generic;
using TopGame.Data;
using System.Linq;
using UnityEngine;
using System;
using TopGame.Core;
using TopGame.Base;
using Proto3;
using TopGame.SvrData;
using TopGame.Logic;
using Framework.Plugin.AT;
using static TopGame.UI.UIGMPanelView;
using Framework.Core;

namespace TopGame.UI
{
    [Framework.Plugin.AT.ATExportMono("UI系统/GM面板/UIBase")]
    [UI((ushort)EUIType.GMPanel, UI.EUIAttr.UI)]
    public class UIGMPanel : UIBase
    {
#if USE_GMCONSOLE
        UIGMPanelView m_GMView = null;

        public int RandomEventId;
        public List<UIGMPanelView.GMItemData> m_ItemList = new List<UIGMPanelView.GMItemData>();
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            m_GMView = m_pView as UIGMPanelView;
        }
        //------------------------------------------------------
        public void GoToStage(uint stageId)
        {
//             CsvData_Chapter.ChapterData data = DataManager.getInstance().Chapter.GetData(stageId);
//             if (data == null)
//             {
//                 Framework.Plugin.Logger.Error("章节id[" + stageId + "]不存在！");
//                 //! TODO..popmessage
//                 return;
//             }
//             BattleDB battleDb = TopGame.SvrData.UserManager.getInstance().mySelf.ProxyDB<BattleDB>(Data.EDBType.Battle);
//             
//             GameInstance.getInstance().ChangeLocation(ELocationState.DungonPVE, ELoadingType.Loading);
//             battleDb.SetCurrentlevelType(Proto3.LevelTypeCode.Gm);
//             battleDb.SetCurrentLevel(Proto3.LevelTypeCode.Gm, data.id);

            Hide();
        }
        //------------------------------------------------------
        public void RefreshShowStage(string content)
        {
//             ShowStageDatas.Clear();
//             FullStageTable.ForEach((stageData)=> {
//                 if (stageData.Key.ToString().Contains(content)) ShowStageDatas.Add(stageData);
//             });

            m_GMView.UpdateStagePanel();
        }

        //------------------------------------------------------
        public void OnClickStageSelectBtn(GameObject go, params VariablePoolAble[] param)
        {
            m_GMView.ShowSelectPanel(true);
            m_GMView.ShowSendMailPanel(false);
            m_GMView.ShowItemPanel(false);
            m_GMView.ShowCheatCodePanel(false);
        }
        //------------------------------------------------------
        public void OnClickAddItemBtn(GameObject go, params VariablePoolAble[] param)
        {
            m_GMView.ShowSelectPanel(false);
            m_GMView.ShowItemPanel(true);
            m_GMView.ShowSendMailPanel(false);
            m_GMView.ShowCheatCodePanel(false);

            m_ItemList.Clear();
            m_ItemList.Add(new GMItemData(1, "金币"));//金币
            m_ItemList.Add(new GMItemData(2, "经验"));//经验
            m_ItemList.Add(new GMItemData(3, "魔尘"));//魔尘
            m_ItemList.Add(new GMItemData(4, "钻石"));//钻石
            m_ItemList.Add(new GMItemData(35, "竞技场挑战券"));//竞技场挑战券
            m_ItemList.Add(new GMItemData(6001, "水元素引兽粉尘（仙女龙）"));
            m_ItemList.Add(new GMItemData(6006, "团魂引兽粉尘（治疗）"));
            m_ItemList.Add(new GMItemData(7001, "团魂经验道具"));
            m_ItemList.Add(new GMItemData(8001, "感悟材料石"));
            m_ItemList.Add(new GMItemData(8002, "感悟突破石"));
        }
        //------------------------------------------------------
        public void OnClickSubmitBtn(GameObject go, params VariablePoolAble[] param)
        {
//             Proto3.GMCommandRequest gmReq = new Proto3.GMCommandRequest();
//             foreach (var item in m_GMView.GetGMItem())
//             {
//                 Proto3.GMCommandInfo req = new Proto3.GMCommandInfo();
//                 req.Id = item.Value.ItemID;
//                 req.Parameter = item.Value.ItemCount;
//                 req.Command = Proto3.GMCommandType.AddThing;
//                 gmReq.Info.Add(req);
//             }
//             Net.NetWork.getInstance().SendMessage((int)Proto3.MID.GmcommandReq, gmReq);
        }
        //------------------------------------------------------
        public void OnClickSendEventBtn(GameObject go, params VariablePoolAble[] param)
        {
//             if (RandomEventId == 0)
//             {
//                 Framework.Plugin.Logger.Error("请输入随机事件ID");
//                 return;
//             }
// 
//             List<Proto3.GMCommandInfo> reqs = new List<Proto3.GMCommandInfo>();
// 
//             Proto3.GMCommandInfo req = new Proto3.GMCommandInfo();
//             req.Id = RandomEventId;
//             req.Command = Proto3.GMCommandType.EventTrigger;
//             reqs.Add(req);
// 
//             Net.GMCommandHandler.Req_GMCommands(reqs);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("添加随机装备")]
        public void OnClickAddEquip()
        {
//             GMCommandRequest reqs = new GMCommandRequest();
//             Proto3.GMCommandInfo req = new Proto3.GMCommandInfo();
//             req.Command = Proto3.GMCommandType.SendEquipment;
//             reqs.Info.Add(req);
//             Net.NetWork.getInstance().SendMessage((int)Proto3.MID.GmcommandReq, reqs);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("添加随机元素装备")]
        public void OnClickAddElementEquip()
        {
//             GMCommandRequest reqs = new GMCommandRequest();
//             Proto3.GMCommandInfo req = new Proto3.GMCommandInfo();
//             req.Command = Proto3.GMCommandType.SendEquipment;
//             req.Parameter = 1;
//             reqs.Info.Add(req);
//             Net.NetWork.getInstance().SendMessage((int)Proto3.MID.GmcommandReq, reqs);
        }
        //------------------------------------------------------
        public void OnClickExitBtn(GameObject go, params VariablePoolAble[] param)
        {
            this.Hide();
        }
        //------------------------------------------------------
        internal void OnFpsBtn(GameObject go, VariablePoolAble[] param)
        {
            var fps = GetWidget<UnityEngine.UI.InputField>("target_fps");
            if (fps == null) return;
            int targetFps;
            if (int.TryParse(fps.text, out targetFps))
                Application.targetFrameRate = targetFps;
        }
        //------------------------------------------------------
        internal void OnClickMailSubmitBtn(GameObject go, VariablePoolAble[] param)
        {
            //Proto3.GMCommandRequest gmReq = new Proto3.GMCommandRequest();
            //Proto3.GMCommandInfo req = new Proto3.GMCommandInfo();
            //CommandMailInfo mail = new CommandMailInfo();
            //mail.Title = m_GMView.GetMailTitle();
            //mail.Content = m_GMView.GetMailContent();
            //mail.RemainingTime = m_GMView.GetMailExpireTime();

            //Util.Log("提交邮件,Title=" + mail.Title + ",Content=" + mail.Content + ",RemainingTime=" + mail.RemainingTime);

            ////发送目标
            //if (m_GMView.GetIsAll())
            //{
            //    //全服
            //}
            //else
            //{
            //    List<long> idList = m_GMView.GetSendMailPlaysID();
            //    foreach (var id in idList)
            //    {
            //        mail.UserID.Add(id);
            //        Util.Log("发送的玩家id:" + id);
            //    }
            //}

            ////附件物品添加
            //foreach (var item in m_GMView.EnclosureItemList)
            //{
            //    Util.Log("附件物品:" + item.ItemID + ",count=" + item.ItemCount + ",type=" + item.itemType);
            //    mail.Reward.Add(new ItemInfoResponse() { Id = item.ItemID, Count  = item.ItemCount, Type = item.itemType});
            //}
                


            //req.MailInfo.Add(mail);
            //req.Command = GMCommandType.SendMail;
            //gmReq.Info.Add(req);

            //Net.NetWork.getInstance().SendMessage((int)Proto3.MID.GmcommandReq, gmReq);
        }
        //------------------------------------------------------
        internal void OnClicksendMailBtn(GameObject go, VariablePoolAble[] param)
        {
            m_GMView.ShowSendMailPanel(true);
            m_GMView.ShowSelectPanel(false);
            m_GMView.ShowItemPanel(false);
            m_GMView.ShowCheatCodePanel(false);
        }
        //------------------------------------------------------
        internal void OnClickInputCodeBtn(GameObject go, VariablePoolAble[] param)
        {
            m_GMView.ShowSendMailPanel(false);
            m_GMView.ShowSelectPanel(false);
            m_GMView.ShowItemPanel(false);
            m_GMView.ShowCheatCodePanel(true);
        }
        //------------------------------------------------------
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="i"></param>
        internal void OnDeleteEnclosureItem(int i)
        {
            if (i >= m_GMView.EnclosureItemList.Count)
            {
                return;
            }
            m_GMView.EnclosureItemList.RemoveAt(i);
            m_GMView.InitEnclosureGridMgr();
        }
        //------------------------------------------------------
        public UIGMPanelView GetView()
        {
            return m_GMView;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("输入代码提交")]
        public void OnCommitInputCode(string code)
        {
            Debug.Log("输入的代码:" + code);
            if (code == "guide.enable=false")
            {
                Framework.Plugin.Guide.GuideSystem.getInstance().Enable(false);
                Debug.Log("关闭引导");
            }
            else if (code == "guide.enable=true")
            {
                Framework.Plugin.Guide.GuideSystem.getInstance().Enable(true);
                Debug.Log("开启引导");
            }
            else if (code == "guide.enableLog=true")
            {
                Framework.Plugin.Guide.GuideSystem.getInstance().bGuideLogEnable = true;
                Debug.Log("开启引导Log");
            }
            else if (code == "guide.enableLog=false")
            {
                Framework.Plugin.Guide.GuideSystem.getInstance().bGuideLogEnable = false;
                Debug.Log("关闭引导Log");
            }
            else if (code == "gift")
            {
                GetGift();
                Debug.Log("获得钻石,英雄和随机装备");
            }
        }
        //------------------------------------------------------
        void GetGift()
        {
            //Proto3.GMCommandInfo req = new Proto3.GMCommandInfo();
            //Proto3.GMCommandRequest gmReq = new Proto3.GMCommandRequest();
            //req.Id = 4;
            //req.Parameter = 100000;
            //req.Command = Proto3.GMCommandType.AddThing;
            //gmReq.Info.Add(req);

            ////获取最高级的英雄
            //foreach (var item in DataManager.getInstance().Player.datas)
            //{
            //    if (item.Value.nextId == 0)
            //    {
            //        req = new Proto3.GMCommandInfo();
            //        req.Id = (int)item.Value.ID;
            //        req.Parameter = 1;
            //        req.Command = Proto3.GMCommandType.AddThing;
            //        gmReq.Info.Add(req);
            //        //Debug.Log("添加英雄:" + req.Id);
            //    }
            //}

            ////添加随机装备
            //req = new Proto3.GMCommandInfo();
            //req.Command = Proto3.GMCommandType.SendEquipment;
            //gmReq.Info.Add(req);

            //Net.NetWork.getInstance().SendMessage((int)Proto3.MID.GmcommandReq, gmReq);
        }
        //------------------------------------------------------
        public void AIEnable(bool enable)
        {
            //先去除
            //             AutoRuner auto = AState.CastCurrentModeLogic<AutoRuner>();
            //             if (auto != null)
            //             {
            //                 auto.EnableAuto(enable);
            //             }
        }
        //------------------------------------------------------
        public bool GetAIEnable()
        {
            //先去除
//             AutoRuner auto = AState.CastCurrentModeLogic<AutoRuner>();
//             if (auto != null)
//             {
//                 return auto.IsAuto();
//             }
            return false;
        }
        //------------------------------------------------------
        public bool GetBattleSliderTypeEnable()
        {
            //获取初始状态
            return true;
        }
        //------------------------------------------------------
        public void BattleSliderTypeEnable(bool enable)
        {
            //设置开关
        }
        //------------------------------------------------------
        public void OnSimulationDrawCardBtn(int type,int count)
        {
//             Proto3.GMCommandRequest gmReq = new Proto3.GMCommandRequest();
//             Proto3.GMCommandInfo req = new Proto3.GMCommandInfo();
//             req.Command = GMCommandType.TavernLottery;
//             req.Id = type;
//             req.Parameter = count;
//             gmReq.Info.Add(req);
// 
//             Net.NetWork.getInstance().SendMessage((int)Proto3.MID.GmcommandReq, gmReq);
        }
#else
        [Framework.Plugin.AT.ATMethod("输入代码提交")]
        public void OnCommitInputCode(string code) { }

        [Framework.Plugin.AT.ATMethod("添加随机元素装备")]
        public void OnClickAddElementEquip() { }

        [Framework.Plugin.AT.ATMethod("添加随机装备")]
        public void OnClickAddEquip() { }
#endif
    }
}