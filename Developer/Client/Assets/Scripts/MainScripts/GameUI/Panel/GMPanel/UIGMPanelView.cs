#if UNITY_EDITOR
#define USE_GMCONSOLE
#endif
/********************************************************************
生成日期:	6:19:2020 10:06
类    名: 	UIGMPanelView
作    者:	JaydenHe
描    述:	控制台界面
*********************************************************************/

using Proto3;
using System;
using System.Collections.Generic;
using TopGame.Base;
using TopGame.Data;
using UnityEngine;
using UnityEngine.UI;
using TopGame.SvrData;
using TopGame.Logic;
using Framework.Core;
using Framework.Logic;
using Framework.BattlePlus;

namespace TopGame.UI
{
    [UI((ushort)EUIType.GMPanel, UI.EUIAttr.View)]
    public class UIGMPanelView : UIView
    {
#if USE_GMCONSOLE
        public struct GMItemData
        {
            public int ItemIdx;
            public int ItemID;
            public int ItemCount;

            public int itemType;

            public string ItemName;

            public GMItemData(int itemID,string itemName, int itemCount = 0, int itemType = 0, int itemIdx = 0)
            {
                ItemIdx = itemIdx;
                ItemID = itemID;
                ItemCount = itemCount;
                this.itemType = itemType;
                ItemName = itemName;
            }
        }

        UIGMPanel m_pUI = null;
        InputField m_InputField = null;
        RectTransform m_StageSelectPanel = null;
        RectTransform m_ItemAddPanel = null;
//         Button m_StageBtn;
//         Button m_ItemAddBtn;
//         Button m_SubmitBtn;
        Button m_ExitBtn;

        int m_showCnt = 1;
        Dictionary<int, GMItemData> m_tempDataMap = new Dictionary<int, GMItemData>();

        private InputField m_Titile_Input;
        private InputField m_Content_Input;
        private InputField m_ExpireTime_Input;
        private Toggle m_Player_Toggle;
        private Toggle m_All_Toggle;
        private InputField m_PlayerID_Input;
        private Button m_MailSubmitBtn;
        private GameObject m_SendMailPanel;

        private InputField m_RandomEventIdInput;
        private Button m_SendEventBtn;
        private Button pvpBtn;

        /// <summary>
        /// 要添加物品的附件列表
        /// </summary>
        public List<GMItemData> EnclosureItemList = new List<GMItemData>();
        /// <summary>
        /// 所有可以添加的附件物品列表
        /// </summary>
        public List<GMItemData> ALLEnclosureItemList = new List<GMItemData>();
        private Toggle m_Invincible_Toggle;
        private UISerialized m_SendMailPanelUI;
        private InputField m_ID_Input;
        private Toggle m_ShowBattleToggle_Toggle;
        private GameObject m_CheatCodePanel;


        private InputField m_BoardcastUserId;
        private InputField m_BoardcastLooptime;
        private InputField m_BoardcastCfg;
        private InputField m_BoardcastContent;
        private RectTransform m_BoardcastSendBtn;

        private InputField m_HeroConfig;
        private InputField m_HeroLevel;
        private InputField m_Herostar;
        private RectTransform m_HeroLevelConfirmBtn;
        private InputField m_SimulationDrawCardType;
        private InputField m_SimulationDrawCardCount;

        //------------------------------------------------------
        public override void Init(UIBase pBase)
        {
            base.Init(pBase);
            m_pUI = pBase as UIGMPanel;
        }
        //------------------------------------------------------
        public bool GetShowBattleToggle()
        {
            if (m_ShowBattleToggle_Toggle== null)
            {
                return true;
            }
            return m_ShowBattleToggle_Toggle.isOn;
        }
        //------------------------------------------------------
        public Dictionary<int, GMItemData> GetGMItem()
        {
            return m_tempDataMap;
        }
        //------------------------------------------------------
        public void ShowCheatCodePanel(bool isShow)
        {
            Util.SetActive(m_CheatCodePanel, isShow);
        }
        //------------------------------------------------------
        public void ShowSendMailPanel(bool isShow)
        {
            m_SendMailPanel.SetActive(isShow);
        }
        //------------------------------------------------------
        public void ShowSelectPanel(bool isShow)
        {
//             m_StageSelectPanel.gameObject.SetActive(isShow);
//             if(isShow)
//                 m_stageCellMgr.Init(m_pUI.ShowStageDatas.Count, OnShowStagePanel);
        }
        //------------------------------------------------------
        public void ShowItemPanel(bool isShow)
        {
            m_ItemAddPanel.gameObject.SetActive(isShow);
            if(isShow)
            {
                if (m_showCnt <= 0) m_showCnt = 1;
          //      m_itemCellMgr.Init(m_showCnt, OnShowItemPanel);
            }
        }
        //------------------------------------------------------
        private void DelItem(int key)
        {
            m_showCnt--;
            m_tempDataMap.Remove(key);

            int cnt = 0;
            Dictionary<int, GMItemData> newData = new Dictionary<int, GMItemData>();
            foreach (var data in m_tempDataMap)
            {
                newData.Add(cnt,data.Value);
                cnt++;
            }
            m_tempDataMap = newData;

          //  m_itemCellMgr.Init(m_tempDataMap.Count, OnShowItemPanel);
        }
        //------------------------------------------------------
        private void AddItem(GameObject go, params VariablePoolAble[] param)
        {
            m_showCnt++;
         //   m_itemCellMgr.Init(m_showCnt, OnShowItemPanel);
        }
        //------------------------------------------------------
        private void OnDropDownValueChange(int dropValue,int key,Dropdown drop)
        {
            if (m_pUI == null || dropValue >= m_pUI.m_ItemList.Count)
            {
                return;
            }
            GMItemData itemData = m_tempDataMap[key];
            itemData.ItemID = m_pUI.m_ItemList[dropValue].ItemID;
            m_tempDataMap[key] = itemData;
        }
        //------------------------------------------------------
        private void OnInputTextChange(string count,int key)
        {
            GMItemData itemData = m_tempDataMap[key];
            itemData.ItemCount = int.Parse(count);
            m_tempDataMap[key] = itemData;
        }
        //------------------------------------------------------
        public void OnShowItemPanel(GameObject go,int i,int idx)
        {
            bool isContain = m_tempDataMap.ContainsKey(i);
            if (!isContain) m_tempDataMap.Add(i, new GMItemData());

            int index = i+1;
            Dropdown drop = go.transform.Find("Dropdown").GetComponent<Dropdown>();
            drop.ClearOptions();
            drop.options = new List<Dropdown.OptionData>();
            drop.options.Add(new Dropdown.OptionData("1:金币"));
            drop.options.Add(new Dropdown.OptionData("2:经验"));
            drop.options.Add(new Dropdown.OptionData("3:魔尘"));
            drop.options.Add(new Dropdown.OptionData("4:钻石"));
            drop.options.Add(new Dropdown.OptionData("竞技场挑战券"));
            drop.options.Add(new Dropdown.OptionData("水元素引兽粉尘（仙女龙）"));
            drop.options.Add(new Dropdown.OptionData("团魂引兽粉尘（治疗）"));
            drop.options.Add(new Dropdown.OptionData("团魂经验道具"));
            drop.options.Add(new Dropdown.OptionData("感悟材料石"));
            drop.options.Add(new Dropdown.OptionData("感悟突破石"));

            drop.onValueChanged.RemoveAllListeners();
            drop.onValueChanged.AddListener((selectIdx) =>
            {
                OnDropDownValueChange(selectIdx, i, drop);
            });

            drop.value = isContain ? m_tempDataMap[i].ItemID:0;
            drop.RefreshShownValue();

            InputField input = go.transform.Find("InputField").GetComponent<InputField>();
            input.onValueChanged.RemoveAllListeners();
            input.onValueChanged.AddListener((s) => {
                OnInputTextChange(s, i);
            });

            input.text = isContain ? m_tempDataMap[i].ItemCount.ToString(): "1";

            Text text = go.transform.Find("Text").GetComponent<Text>();
            text.text = index.ToString();
            Button delBtn = go.transform.Find("Delbtn").GetComponent<Button>();
            if (i != 0) EventTriggerListener.Get(delBtn.gameObject).onClick = (g, obj) => { DelItem(i); };

            Button addBtn = go.transform.Find("AddBtn").GetComponent<Button>();
            EventTriggerListener.Get(addBtn.gameObject).onClick = AddItem;

            addBtn.gameObject.SetActive(index == m_showCnt);
        }

        //------------------------------------------------------
        public void OnShowStagePanel(GameObject go, int i, int idx)
        {
//             Button btn = go.transform.Find("Bg").GetComponent<Button>();
//             ChapterData data = m_pUI.ShowStageDatas[i].Value;            
//             Text id = go.transform.Find("IDText").GetComponent<Text>();
//             Text des = go.transform.Find("StageDes").GetComponent<Text>();
//             id.text = "关卡Id:" + m_pUI.ShowStageDatas[i].Key;
//             string desContent = Core.LocalizationManager.ToLocalization(m_pUI.ShowStageDatas[i].Value.nameId);
//             des.text = desContent;
// 
//             EventTriggerListener.Get(btn.gameObject).onClick = (g, obj) => {
//                 m_pUI.GoToStage(m_pUI.ShowStageDatas[i].Key);
//             };
        }
        //------------------------------------------------------
        public void InitView()
        {
//             m_stageCellMgr.Init(m_pUI.ShowStageDatas.Count, OnShowStagePanel);
//             m_itemCellMgr.Init(m_showCnt, OnShowItemPanel);
//             //EnclosureItemCellMgr.Init(EnclosureItemList.Count,OnEnclosureItemShow);
//             m_ItemAddPanel.gameObject.SetActive(false);
//             m_SendMailPanel.SetActive(false);
        }
        //------------------------------------------------------
        /// <summary>
        /// 邮件附件物品格子显示
        /// </summary>
        /// <param name="item"></param>
        /// <param name="wrapIndex"></param>
        /// <param name="realIndex"></param>
        private void OnEnclosureItemShow(GameObject go, int i, int realIndex)
        {
            if (i >= EnclosureItemList.Count)
            {
                return;
            }
            GMItemData data = EnclosureItemList[i];
            //添加英雄类型选择

            UISerialized ui = go.GetComponent<UISerialized>();
            if (ui == null)
            {
                return;
            }

            Text itemName = ui.GetWidget<Text>("ItemName");
            Text itemCount = ui.GetWidget<Text>("ItemCount");
            Text index = ui.GetWidget<Text>("Index");

//             Util.SetLabel(itemName,data.ItemName);
// 
//             Util.SetLabel(itemCount, data.ItemCount.ToString());
// 
//             Util.SetLabel(index, i.ToString());
            //根据index 进行删除
            Button delBtn = go.transform.Find("Delbtn").GetComponent<Button>();
            if (delBtn)
            {
                EventTriggerListener.Get(delBtn.gameObject).onClick = (g, obj) =>
                {
                    m_pUI.OnDeleteEnclosureItem(i);
                };
            }
            
        }

        //------------------------------------------------------
        public void UpdateStagePanel()
        {
          //  m_stageCellMgr.Init(m_pUI.ShowStageDatas.Count, OnShowStagePanel);
        }
        //------------------------------------------------------
        public string GetMailTitle()
        {
            return m_Titile_Input.text;
        }
        //------------------------------------------------------
        public string GetMailContent()
        {
            return m_Content_Input.text;
        }
        //------------------------------------------------------
        public int GetMailExpireTime()
        {
            int expireTime = 0;
            if (int.TryParse(m_ExpireTime_Input.text,out expireTime))
            {
                return expireTime;
            }
       //     Base.Util.ShowCommonTip(ETipType.AutoHide, "输入的时间不是整数类型");
            return 0;
        }
        //------------------------------------------------------
        public bool GetIsAll()
        {
            return m_All_Toggle.isOn;
        }
        //------------------------------------------------------
        /// <summary>
        /// 获取发生邮件的玩家id列表
        /// </summary>
        /// <returns></returns>
        public List<long> GetSendMailPlaysID()
        {
            List<long> IDList = new List<long>();

            if (string.IsNullOrEmpty(m_PlayerID_Input.text))
            {
                return IDList;
            }
            string[] split = m_PlayerID_Input.text.Split(',');
            for (int i = 0; i < split.Length; i++)
            {
                IDList.Add(long.Parse(split[i]));
            }

            return IDList;
        }
        public void InitEnclosureGridMgr()
        {
        //    EnclosureItemCellMgr.Init(EnclosureItemList.Count, OnEnclosureItemShow);
        }
        //------------------------------------------------------
        public void OnShow()
        {
            if (m_PlayerID_Input)
            {
                m_PlayerID_Input.text = UserManager.getInstance().mySelf.userID.ToString();
            }
            InitView();
        }
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            if (m_pUI == null) return;
            if (m_pUI.ui != null)
            {
                m_InputField = m_pUI.ui.GetWidget<InputField>("InputStage");
                if (m_InputField != null) m_InputField.onValueChanged.AddListener(m_pUI.RefreshShowStage);
                 m_StageSelectPanel = m_pUI.ui.GetWidget<RectTransform>("StageSelect");
                m_ItemAddPanel = m_pUI.ui.GetWidget<RectTransform>("ItemAdd");
//                 m_StageBtn = m_pUI.ui.GetWidget<Button>("StageSelectBtn");
//                 if (m_StageBtn != null) EventTriggerListener.Get(m_StageBtn.gameObject).onClick = m_pUI.OnClickStageSelectBtn; 
//                 m_ItemAddBtn = m_pUI.ui.GetWidget<Button>("ItemAddBtn");
//                 if (m_ItemAddBtn != null) EventTriggerListener.Get(m_ItemAddBtn.gameObject).onClick = m_pUI.OnClickAddItemBtn;
//                 m_ExitBtn = m_pUI.ui.GetWidget<Button>("ExitBtn");
//                 if (m_ExitBtn != null) EventTriggerListener.Get(m_ExitBtn.gameObject).onClick = m_pUI.OnClickExitBtn; 
//                 m_SubmitBtn = m_pUI.ui.GetWidget<Button>("SubmitBtn");
//                 if (m_SubmitBtn != null) EventTriggerListener.Get(m_SubmitBtn.gameObject).onClick = m_pUI.OnClickSubmitBtn;

             //   m_stageCellMgr = m_pUI.ui.GetWidget<GridBoxMgr>("StageContent");
                //m_stageCellMgr.CreateType = GridBoxMgr.CreateCellType.UseCellPool;
            //    m_itemCellMgr = m_pUI.ui.GetWidget<GridBoxMgr>("ItemContent");
                //m_itemCellMgr.CreateType = GridBoxMgr.CreateCellType.UseCellPool;
                m_RandomEventIdInput = m_pUI.ui.GetWidget<InputField>("EventIdInput");
                if (m_RandomEventIdInput != null)
                {
                    m_RandomEventIdInput.onEndEdit.AddListener((str) =>
                    {
                        if (str == "") return;
                        m_pUI.RandomEventId = int.Parse(str);
                    });
                }
                m_SendEventBtn = m_pUI.ui.GetWidget<Button>("RandomEventBtn");
                if (m_SendEventBtn != null) EventTriggerListener.Get(m_SendEventBtn.gameObject).onClick = m_pUI.OnClickSendEventBtn;

                #region 邮件

       //         EnclosureItemCellMgr = m_pUI.ui.GetWidget<GridBoxMgr>("EnclosureItemContent");
       //         if(EnclosureItemCellMgr) EnclosureItemCellMgr.CreateType = GridBoxMgr.CreateCellType.CreatAllCell;

                m_Titile_Input = m_pUI.ui.GetWidget<InputField>("Titile_Input");
                if (m_Titile_Input)
                {
                    m_Titile_Input.text = "标题";
                }
                m_Content_Input = m_pUI.ui.GetWidget<InputField>("Content_Input");
                if (m_Content_Input)
                {
                    m_Content_Input.text = "内容";
                }

                m_ExpireTime_Input = m_pUI.ui.GetWidget<InputField>("ExpireTime_Input");

                m_Player_Toggle = m_pUI.ui.GetWidget<Toggle>("Player_Toggle");
                m_All_Toggle = m_pUI.ui.GetWidget<Toggle>("All_Toggle");

                m_PlayerID_Input = m_pUI.ui.GetWidget<InputField>("PlayerID_Input");
                if (m_PlayerID_Input)
                {
                    m_PlayerID_Input.text = UserManager.getInstance().mySelf.userID.ToString();
                }

                var FpsBtn = m_pUI.ui.GetWidget<Button>("fps_set");
                if(FpsBtn) EventTriggerListener.Get(FpsBtn.gameObject).onClick = m_pUI.OnFpsBtn;

                m_MailSubmitBtn = m_pUI.ui.GetWidget<Button>("MailSubmitBtn");
                if (m_MailSubmitBtn != null) EventTriggerListener.Get(m_MailSubmitBtn.gameObject).onClick = m_pUI.OnClickMailSubmitBtn;

                Image sendMailBtn = m_pUI.ui.GetWidget<Image>("SendMailBtn");
                if (sendMailBtn != null) EventTriggerListener.Get(sendMailBtn.gameObject).onClick = m_pUI.OnClicksendMailBtn;

                Image inputCodeBtn = m_pUI.ui.GetWidget<Image>("InputCodeBtn");
                if (inputCodeBtn != null) EventTriggerListener.Get(inputCodeBtn.gameObject).onClick = m_pUI.OnClickInputCodeBtn;

                m_SendMailPanel = m_pUI.ui.Find("SendMailPanel");
                if (m_SendMailPanel)
                {
                    m_SendMailPanelUI = m_SendMailPanel.GetComponent<UISerialized>();
                    if (m_SendMailPanelUI)
                    {
                        m_ID_Input = m_SendMailPanelUI.GetWidget<InputField>("ID_InputField");
                    }
                }
                m_CheatCodePanel = m_pUI.ui.Find("CheatCodePanel");


                //获取下拉框,增加列表

                ALLEnclosureItemList.Add(new GMItemData(1, "金币"));//金币
                ALLEnclosureItemList.Add(new GMItemData(2, "经验"));//经验
                ALLEnclosureItemList.Add(new GMItemData(3, "魔尘"));//魔尘
                ALLEnclosureItemList.Add(new GMItemData(4, "钻石"));//钻石
                ALLEnclosureItemList.Add(new GMItemData(7, "普通抽奖券"));//普通抽奖券
                ALLEnclosureItemList.Add(new GMItemData(35, "竞技场挑战券"));//竞技场挑战券
                ALLEnclosureItemList.Add(new GMItemData(6001, "水元素引兽粉尘（仙女龙）"));
                ALLEnclosureItemList.Add(new GMItemData(6006, "团魂引兽粉尘（治疗）"));
                ALLEnclosureItemList.Add(new GMItemData(7001, "团魂经验道具"));
                ALLEnclosureItemList.Add(new GMItemData(8001, "感悟材料石"));
                ALLEnclosureItemList.Add(new GMItemData(8002, "感悟突破石"));
                ALLEnclosureItemList.Add(new GMItemData(9999, "RMB"));//人民币

//                 //英雄数据添加
//                 foreach (var player in DataManager.getInstance().Player.datas)
//                 {
//                     ALLEnclosureItemList.Add(new GMItemData((int)player.Value.ID,player.Value.strName,1, ItemTypeCode.Hero));
//                 }
// 
//                 //装备数据添加
//                 foreach (var equip in DataManager.getInstance().Equip.datas)
//                 {
//                     if (equip.Value != null)
//                     {
//                         string equipName = equip.Value.Text_equipName_data == null? "没有装备名字": equip.Value.Text_equipName_data.textCN;
//                         ALLEnclosureItemList.Add(new GMItemData((int)equip.Value.id, equipName, 1, ItemTypeCode.Equip));
//                     }
//                 }

                Dropdown sendMailItemDropdown = m_pUI.ui.GetWidget<Dropdown>("sendMailItemDropdown");
                if (sendMailItemDropdown)
                {
                    sendMailItemDropdown.options = new List<Dropdown.OptionData>();

                    //添加货币
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("1:金币"));
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("2:经验"));
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("3:魔尘"));
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("4:钻石"));
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("7:普通抽奖券"));
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("35:竞技场挑战券"));
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("6001:水元素引兽粉尘（仙女龙）"));
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("6006:团魂引兽粉尘（治疗）"));
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("7001:团魂经验道具"));
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("8001:感悟材料石"));
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("8002:感悟突破石"));
                    sendMailItemDropdown.options.Add(new Dropdown.OptionData("9999:人民币"));

                    //添加英雄
                    foreach (var player in DataManager.getInstance().Player.datas)
                    {
                        sendMailItemDropdown.options.Add(new Dropdown.OptionData(player.Value.strName));
                    }

//                     //添加装备
//                     string equipName = "找不到装备名称";
//                     foreach (var equip in DataManager.getInstance().Equip.datas)
//                     {
//                         if (equip.Value.Text_equipName_data != null)
//                         {
//                             equipName = equip.Value.Text_equipName_data.textCN;
//                         }
//                         sendMailItemDropdown.options.Add(new Dropdown.OptionData(equipName));
//                     }
                }
                InputField sendMailItemNumInputField = m_pUI.ui.GetWidget<InputField>("sendMailItemNumInputField");
                Button addEnclosureItemBtn = m_pUI.ui.GetWidget<Button>("AddEnclosureItemBtn");
                //if (addEnclosureItemBtn)
                //{
                //    EventTriggerListener.Get(addEnclosureItemBtn.gameObject).onClick = (go,param) => {
                //        if (m_ID_Input != null && !string.IsNullOrWhiteSpace(m_ID_Input.text))
                //        {
                //            int id = int.Parse(m_ID_Input.text);
                //            int count = int.Parse(sendMailItemNumInputField.text);
                //            ItemTypeCode itemType;

                //            var cfg = DataManager.getInstance().Item.GetData((uint)id);
                //            if (cfg != null)
                //            {
                //                itemType = (ItemTypeCode)cfg.type;
                //            }
                //            else
                //            {
                //                itemType = ItemTypeCode.Hero;
                //            }

                //            //货币类型直接通过下拉框添加,或者货币界面添加

                //            EnclosureItemList.Add(new GMItemData(id, m_ID_Input.text, count, itemType));
                //            m_ID_Input.text = "";
                //        }
                //        else
                //        {
                //            GMItemData item = new GMItemData(ALLEnclosureItemList[sendMailItemDropdown.value]);
                //            int.TryParse(sendMailItemNumInputField.text, out item.ItemCount);
                //            EnclosureItemList.Add(item);
                //        }
                //        EnclosureItemCellMgr.Init(EnclosureItemList.Count, OnEnclosureItemShow);
                //    };
                //}

//                 m_Invincible_Toggle = m_pUI.ui.GetWidget<Toggle>("Invincible_Toggle");
//                 if (m_Invincible_Toggle)
//                 {
//                     m_Invincible_Toggle.isOn = BattleStatus.bInvincible_Player;
//                     m_Invincible_Toggle.onValueChanged.AddListener((value) => {
//                         BattleStatus.bInvincible_Player = value;
//                         if (value)
//                         {
//                             Framework.Plugin.Logger.Info("你无敌了");
//                         }
//                     });
//                 }


        #endregion


                m_ShowBattleToggle_Toggle = m_pUI.ui.GetWidget<Toggle>("ShowBattleToggle_Toggle");
                if (m_ShowBattleToggle_Toggle)
                {
                    m_ShowBattleToggle_Toggle.isOn = false;
                }

                Toggle ai_Toggle = m_pUI.ui.GetWidget<Toggle>("AI_Toggle");
                if (ai_Toggle && m_pUI != null)
                {
                    ai_Toggle.isOn = m_pUI.GetAIEnable();
                    ai_Toggle.onValueChanged.AddListener((value)=> {
                        m_pUI.AIEnable(value);
                    });
                }

                Toggle battleSliderType_Toggle = m_pUI.ui.GetWidget<Toggle>("BattleSliderType_Toggle");
                if (battleSliderType_Toggle && m_pUI != null)
                {
                    battleSliderType_Toggle.isOn = m_pUI.GetBattleSliderTypeEnable();
                    battleSliderType_Toggle.onValueChanged.AddListener((value) =>
                    {
                        m_pUI.BattleSliderTypeEnable(value);
                    });
                }

                m_BoardcastUserId = m_pUI.ui.GetWidget<InputField>("UserIdInput");
                m_BoardcastLooptime = m_pUI.ui.GetWidget<InputField>("LoopTimeInput");
                m_BoardcastCfg = m_pUI.ui.GetWidget<InputField>("CfgInput");
                m_BoardcastContent = m_pUI.ui.GetWidget<InputField>("ContentInput");
                m_BoardcastSendBtn = m_pUI.ui.GetWidget<RectTransform>("SendBtn");

                m_HeroConfig = m_pUI.ui.GetWidget<InputField>("herocfgid");
                m_HeroLevel = m_pUI.ui.GetWidget<InputField>("herolevel");
                m_Herostar = m_pUI.ui.GetWidget<InputField>("herostar");
                m_HeroLevelConfirmBtn = m_pUI.ui.GetWidget<RectTransform>("LevelSubmitBtn");
                GameObject starSubmitbtn = m_pUI.ui.Find("StarSubmitBtn");

                m_SimulationDrawCardType = m_pUI.ui.GetWidget<InputField>("SimulationDrawCardType");
                m_SimulationDrawCardCount = m_pUI.ui.GetWidget<InputField>("SimulationDrawCardCount");
                GameObject simulationDrawCardBtn = m_pUI.ui.Find("SimulationDrawCardBtn");
                if (simulationDrawCardBtn)
                {
                    EventTriggerListener.Get(simulationDrawCardBtn).onClick = (g,p) =>
                    {
                        if (m_pUI != null && m_SimulationDrawCardType && m_SimulationDrawCardCount)
                        {
                            int type = 0;
                            int count = 1;
                            if (int.TryParse(m_SimulationDrawCardType.text,out type))
                            {
                                
                            }
                            if (int.TryParse(m_SimulationDrawCardCount.text, out count))
                            {

                            }
                            m_pUI.OnSimulationDrawCardBtn(type,count);
                        }
                    };
                }

                Button button = m_pUI.GetWidget<Button>("FileSystemDebug");
                if(button)
                {
                    Text text = button.GetComponentInChildren<Text>();
                    if (text) text.text = FileSystemUtil.isEnableDebug() ? "关闭文件系统调试" : "开启文件系统调试";
                    EventTriggerListener.Get(button.gameObject).onClick = (g, p) =>
                    {
                        FileSystemUtil.EnableDebug(!FileSystemUtil.isEnableDebug());
                        if (text) text.text = FileSystemUtil.isEnableDebug() ? "关闭文件系统调试" : "开启文件系统调试";
                    };
                }
                button = m_pUI.GetWidget<Button>("GuideToggle");
                if (button)
                {
                    Text text = button.GetComponentInChildren<Text>();
                    if (text) text.text = Framework.Plugin.Guide.GuideSystem.getInstance().IsEnable ? "关闭引导" : "开启引导";
                    EventTriggerListener.Get(button.gameObject).onClick = (g, p) =>
                    {
                        if(Framework.Plugin.Guide.GuideSystem.getInstance().IsEnable)
                        {
                            Framework.Plugin.Guide.GuideSystem.getInstance().Enable(false);
                        }
                        else
                        {
                            Framework.Plugin.Guide.GuideSystem.getInstance().Enable(true);
                        }
                        if (text) text.text = Framework.Plugin.Guide.GuideSystem.getInstance().IsEnable ? "关闭引导" : "开启引导";
                    };
                }

                EventTriggerListener buttonListener = m_pUI.GetWidget<EventTriggerListener>("Portait");
                if (buttonListener)
                {
                    buttonListener.onClick = (g, p) =>
                    {
                        if(Screen.orientation >= ScreenOrientation.LandscapeLeft && Screen.orientation<= ScreenOrientation.LandscapeLeft)
                        {
                            UI.UIKits.SetScreenOrientation(ScreenOrientation.Portrait);
                            
                        }
                        else
                        {
                            UI.UIKits.SetScreenOrientation(ScreenOrientation.LandscapeLeft);
                        }

                    };
                }

                GameObject printLockLog = m_pUI.ui.Find("PrintLockLog");
                if (printLockLog)
                {
#if UNITY_EDITOR
                    EventTriggerListener.Get(printLockLog).onClick = (g, p) =>
                    {
                        if (GameInstance.getInstance() != null)
                            GameInstance.getInstance().unlockMgr.PrintDebug();
                    };
#endif
                }
            }
        }
#endif
    }
}