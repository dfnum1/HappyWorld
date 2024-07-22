/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIConfig
作    者:	HappLI
描    述:	UI配置
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    public enum EUIBackupFlag
    {
        [Framework.Data.DisplayNameGUI("开启")] Toggle = 1<<0,
        [Framework.Data.DisplayNameGUI("通用继承")] InheritCommon = 1<<1,
        [Framework.Data.DisplayNameGUI("移至屏幕外")] MoveOutside = 1 << 2,
        [Framework.Data.DisplayNameGUI("备份所有已开界面")] BackupAllShow = 1 << 3,
    }
    public class UIConfig : ScriptableObject
    {
        [System.Serializable]
        public class UI
        {
            public ushort type;
            public string prefab;
            public bool     permanent;
            public bool     alwayShow;
            public int      Order;
            public bool     fullUI = false;
            public int      uiZValue;
            public bool     showLog;
            public bool     trackAble=true;

            //打开该UI时，备份之前的ui，且关闭备份列表中的ui, 直到界面关闭时还原
            [Framework.Data.DisplayNameGUI("备份还原标志"),Framework.Data.DisplayEnumBitGUI(typeof(EUIBackupFlag))]
            public byte canBackupFlag = 0;
            public List<ushort> backups;
            public bool IsCanBackup()
            {
                return (canBackupFlag & (int)EUIBackupFlag.Toggle) != 0;
            }
            public bool IsInheritCommonBackup()
            {
                return (canBackupFlag & (int)EUIBackupFlag.InheritCommon) != 0;
            }

            public bool IsContainBasckup(ushort ui)
            {
                if (backups == null || ui == 0) return false;
                return backups.Contains(ui  );
            }
        }

        public UI[] UIS = null;
        public ushort[] CommonBackupUIs = null;

        public UIAnimatorAssets uiAnimators = null;

        public Sprite DefaultSpr = null;

        private Dictionary<ushort, UI> m_vData = null;
        private void OnEnable()
        {
            if (UIS == null || UIS.Length <=0) return;
            if (m_vData == null) m_vData = new Dictionary<ushort, UI>(UIS.Length);
            m_vData.Clear();
            for (int i =0; i < UIS.Length; ++i)
            {
                if (UIS[i] == null || UIS[i].type == 0) continue;
                if (m_vData.ContainsKey(UIS[i].type))
                {
                    Framework.Plugin.Logger.Break("请检查UIConfig 配置文件是否有红色标记，请解决后再操作!");
                    continue;
                }
                m_vData.Add(UIS[i].type, UIS[i]);
            }
            if(DefaultSpr!=null)
                Data.GlobalDefaultResources.TransparencySprite = DefaultSpr;
        }
        //------------------------------------------------------
        public  UI GetUI(ushort type)
        {
            if (m_vData == null || m_vData.Count<=0) return null;
            UI pOut = null;
            if (m_vData.TryGetValue(type, out pOut))
                return pOut;
            return null;
        }
    }
}

