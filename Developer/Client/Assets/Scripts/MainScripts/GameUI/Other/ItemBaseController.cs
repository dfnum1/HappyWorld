/********************************************************************
生成日期:	3:2:2023   16:51
类    名: 	ItemBaseController
作    者:	zdq
描    述:	管理item加载
*********************************************************************/
using Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TopGame.Base;
using TopGame.Data;
using TopGame.SvrData;
using TopGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{


    public class ItemBaseController : MonoBehaviour
    {
        public struct ItemDataSt
        {
            public int guid;
            public uint configID;
            public long count;
            public int Lv;

            public bool IsCheckCountShowRed;

            private CsvData_Item.ItemData m_ItemCfg;
            public CsvData_Item.ItemData ItemCfg
            {
                get
                {
                    if (m_ItemCfg == null || m_ItemCfg.ID != configID)
                    {
                        m_ItemCfg = DataManager.getInstance().Item.GetData(configID);
                    }
                    return m_ItemCfg;
                }
            }
            
        }

        public delegate AssetOperiaon LoadObjectAsset(UnityEngine.Object pObj, string strPath, bool bPermanent = false, bool bAysnc = true, Sprite defaultSprite = null);

        public UISerialized CommonItem;
        public UISerialized ChestItem;
        public UISerialized HeroItem;
        public UISerialized EquipItem;
        public UISerialized PetItem;

        private Dictionary<int, UISerialized> m_vInstantiateItems = null;


        //------------------------------------------------------
        public UISerialized LoadItem(EItemType itemType, ItemDataSt data, LoadObjectAsset action)
        {
            Clear();
            var item = GetItem(itemType);
            if (item == null)
            {
                Debug.LogError($"获取不到类型:{itemType}的预制体!请检查!");
                return null;
            }
            var mark = item.GetWidget<ItemMark>("ItemMark");
            if (mark)
            {
                mark.type = itemType;
            }
            switch (itemType)
            {
                case EItemType.Money:
                    LoadCommonItem(item, data, action);
                    break;
                case EItemType.Hero:
                    LoadHero(item, data, action);
                    break;
                case EItemType.Chest:
                    LoadChestItem(item, data, action);
                    break;
                case EItemType.Equip:
                    LoadEquip(item, data.configID,action);
                    break;
                default:
                    LoadCommonItem(item, data, action);
                    break;
            }

            return item;
        }
        //------------------------------------------------------
        public UISerialized LoadBagItem(ItemDataSt data, LoadObjectAsset action, int fontSize = 25)
        {
            Clear();
            var item = GetItem(EItemType.Money);
            if (item == null)
            {
                Debug.LogError($"获取不到类型:{EItemType.Money}的预制体!请检查!");
                return null;
            }
            LoadCommonItem(item, data, action,fontSize);

            return item;
        }
        //------------------------------------------------------
        public UISerialized LoadItem(ItemDataSt data, LoadObjectAsset action)
        {
            var cfg = DataManager.getInstance().Item.GetData(data.configID);
            if (cfg == null)
            {
                return null;
            }

            return LoadItem((EItemType)cfg.type,data, action);
        }
        //------------------------------------------------------
        void Clear()
        {
            if (transform.childCount > 0)//有子物体情况,获取子物体
            {
                foreach (Transform child in transform)
                {
                    var ui = child.GetComponent<UISerialized>();
                    if (!ui)
                    {
                        continue;
                    }

                    TopGame.UI.ItemMark itemMark = ui.GetWidget<TopGame.UI.ItemMark>("ItemMark");
                    if (!itemMark)
                    {
                        continue;
                    }

                    int key = (int)itemMark.type;
                    if (m_vInstantiateItems == null)
                    {
                        m_vInstantiateItems = new Dictionary<int, UISerialized>();
                    }
                    m_vInstantiateItems[key] = ui;
                }
            }
            
            if (m_vInstantiateItems != null && m_vInstantiateItems.Count > 0)
            {
                foreach (var item in m_vInstantiateItems)
                {
                    UIUtil.SetActive(item.Value, false);
                }
            }
        }
        //------------------------------------------------------
        UISerialized GetItem(EItemType itemType)
        {
            int key = (int)itemType;
            if (m_vInstantiateItems == null)
            {
                m_vInstantiateItems = new Dictionary<int, UISerialized>();
            }

            if (m_vInstantiateItems.TryGetValue(key, out UISerialized item))//获取缓存
            {
                UIUtil.SetActive(item,true);
                return item;
            }else{
                switch (itemType)
                {
                    case EItemType.Money:
                    case EItemType.Chest:
                        item = Instantiate<UISerialized>(CommonItem, transform);
                        break;
                    case EItemType.Hero:
                        item = Instantiate<UISerialized>(HeroItem, transform);
                        break;
                    case EItemType.Equip:
                        item = Instantiate<UISerialized>(EquipItem, transform);
                        break;
                    default:
                        item = Instantiate<UISerialized>(CommonItem, transform);
                        break;
                }
            }
            UIUtil.SetActive(item, true);
            return item;
        }
        //------------------------------------------------------
        public static void LoadCommonItem(UISerialized ui, ItemDataSt data, LoadObjectAsset action, int fontSize = 25)
        {
            //if (ui == null || action == null || data.ItemCfg == null)
            //{
            //    Debug.LogError("数据有误,无法加载item");
            //    return;
            //}

            //UnityEngine.UI.Image icon = ui.GetWidget<UnityEngine.UI.Image>("icon");
            //UnityEngine.UI.Image frame = ui.GetWidget<UnityEngine.UI.Image>("frame");
            //UnityEngine.UI.Text num_Text = ui.GetWidget<UnityEngine.UI.Text>("num_Text");
            //TopGame.UI.EventTriggerListener iconBtn = ui.GetWidget<TopGame.UI.EventTriggerListener>("icon");

            //if (icon)
            //{
            //    action(icon, UIUtil.GetAssetPath(data.ItemCfg.icon));
            //}
            //if (frame)
            //{
            //    if (data.ItemCfg.qualityFrame != 0)
            //    {
            //        UIUtil.SetActive(frame,true);
            //        action(frame, UIUtil.GetAssetPath(data.ItemCfg.qualityFrame));
            //    }
            //    else
            //    {
            //        UIUtil.SetActive(frame,false);
            //    }
            //}

            //var count = data.count;
            //if (data.ItemCfg.type == (byte)EItemType.HangUpGlod)
            //{
            //    count = data.count * PlayerHangUpLogic.GetHangUpGlodByTime((int)data.ItemCfg.param * 60);
            //}
            //UIUtil.SetFontSize(num_Text, fontSize);
            //if (data.IsCheckCountShowRed)
            //{
            //    var bagCount = UserManager.Current.GetItemDB().GetItemCount(data.configID);
            //    UIUtil.SetGraphicColor(num_Text, data.count > bagCount ? ColorUtil.RedColor : ColorUtil.WhiteColor);
            //    UIUtil.SetLabel(num_Text, $"{GlobalUtil.GetNumFormat(bagCount)}/{GlobalUtil.GetNumFormat(count)}");
            //}
            //else
            //{
            //    UIUtil.SetLabel(num_Text, $"X{GlobalUtil.GetNumFormat(count)}");
            //}

            //if (iconBtn)
            //{
            //    iconBtn.param1 = new Variable1() { uintVal = data.ItemCfg.ID }; ;
            //    iconBtn.onClick = OnClickShowItemTip;
            //}
        }
        //------------------------------------------------------
        public static void LoadHero(UISerialized ui, ItemDataSt data, LoadObjectAsset action)
        {
            if (ui == null ||action == null)
            {
                Debug.LogError("数据有无,无法加载item");
                return;
            }

            CsvData_Player.PlayerData playerData = DataManager.getInstance().Player.GetData(data.configID);
            if (playerData == null)
            {
                Debug.LogError("playerData == null,无法加载Hero");
                return;
            }


            UnityEngine.UI.Image bg = ui.GetWidget<UnityEngine.UI.Image>("bg");

            action(bg, UIUtil.GetAssetPath(playerData.quality));

            UnityEngine.UI.Image icon = ui.GetWidget<UnityEngine.UI.Image>("icon");
            action(icon, UIUtil.GetAssetPath(playerData.icon));
            //UnityEngine.UI.Text lv = ui.GetWidget<UnityEngine.UI.Text>("lv");
            //UIUtil.SetLabel(lv, data.Lv.ToString());
        }
        //------------------------------------------------------
        public static void LoadChestItem(UISerialized ui, ItemDataSt data, LoadObjectAsset action)
        {
            LoadCommonItem(ui, data, action);
        }
        //------------------------------------------------------
        public static void LoadEquip(UISerialized ui,uint configID, LoadObjectAsset action,int level = 1)
        {
            //var cfg = DataManager.getInstance().Equip.GetData(configID);
            //if (ui == null || action == null || cfg == null)
            //{
            //    Debug.LogError($"数据有误,无法加载装备:{configID}");
            //    return;
            //}

            //UnityEngine.UI.Image frame = ui.GetWidget<UnityEngine.UI.Image>("Frame");
            //UnityEngine.UI.Image icon = ui.GetWidget<UnityEngine.UI.Image>("Icon");
            //UnityEngine.UI.Text weaponName = ui.GetWidget<UnityEngine.UI.Text>("WeaponName");
            //UnityEngine.UI.Text levelText = ui.GetWidget<UnityEngine.UI.Text>("level");

            //action(frame, UIUtil.GetAssetPath(cfg.qualityFrame));
            //action(icon, UIUtil.GetAssetPath(cfg.icon));

            //if (weaponName)
            //{
            //    weaponName.color = ColorUtil.GetColor(cfg.qualityColor);
            //    UIUtil.SetLabel(weaponName, cfg.name);
            //}
            //int showLv = 0;

            //showLv = level % 9;
            //if (showLv == 0)
            //{
            //    showLv = 9;
            //}
            //UIUtil.SetLabel(levelText, $"+{showLv}");
        }
        //------------------------------------------------------
        private static void OnClickShowItemTip(GameObject go, VariablePoolAble[] param)
        {
            uint itemId = ((Variable1)param[0]).uintVal;
            TipsUtil.ShowItemTip(itemId, go.transform.position);
        }

    }
}