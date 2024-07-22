/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	AnimPathData
作    者:	HappLI
描    述:   模拟数据
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;
using TopGame.Core;
using TopGame.Data;
using Framework.Core;
using TopGame.Base;

namespace TopGame.SvrData
{
    public class LocalServerData
    {
        public static User Load(string accountId)
        {
            bool bHasLocal = false;
            string strPath = FileSystemUtil.PersistenDataPath;
            strPath = Framework.Core.CommonUtility.stringBuilder.Append(strPath).Append("user_dbs_").Append(accountId).ToString();
            User pUser = UserManager.getInstance().mySelf;
            if (System.IO.Directory.Exists(strPath))
            {
                try
                {
                    //user file
                    {
                        string userFile = Framework.Core.CommonUtility.stringBuilder.Append(strPath).Append("/user.json").ToString();
                        if(System.IO.File.Exists(userFile))
                        {
                            JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(userFile, System.Text.Encoding.UTF8), pUser);
                            for(int i =0; i < (int)EDBType.Count; ++i)
                            {
                                EDBType type = (EDBType)i;
                                string dbFile = Framework.Core.CommonUtility.stringBuilder.Append(strPath).Append("/").Append(type).Append(".json").ToString();
                                if(System.IO.File.Exists(dbFile))
                                {
                                    pUser.UnSerializeProxyDB(type, System.IO.File.ReadAllText(dbFile, System.Text.Encoding.UTF8));
                                }
                            }
                            bHasLocal = true;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning(ex.ToString());
                }
            }

            BaseDB baseDb = pUser.ProxyDB<BaseDB>(EDBType.BaseInfo);

            Net.PacketBase msgItem = Net.MessagePool.CreatePB<Proto3.ItemInfosResponse>();
            var pbItemMsg = msgItem.GetMsg<Proto3.ItemInfosResponse>();
            pbItemMsg.UserID = pUser.userID;

            if (!bHasLocal)
            {
                pUser.userID = 0;
                pUser.SetSDKUid(accountId);
                //初始道具
                var initItems = CsvData_SystemConfig.sysConfig.initialItem;
                var initItemsCount = CsvData_SystemConfig.sysConfig.initialItemValue;
                for (int i = 0; i < initItems.Length; i++)
                {
                    var id = initItems[i];
                    uint count = 0;
                    if (i < initItemsCount.Length)
                    {
                        count = initItemsCount[i];
                    }
                    Proto3.ItemData itemData = new Proto3.ItemData();
                    itemData.ConfigId = (int)id;
                    itemData.Value = count;
                    itemData.Guid = i + 1;
                    pbItemMsg.Items.Add(itemData);
                }
            }

            Net.PacketBase msgHero = Net.MessagePool.CreatePB<Proto3.HeroInfoResponse>();
            var pbHeroMsg = msgHero.GetMsg<Proto3.HeroInfoResponse>();
            pbHeroMsg.UserID = pUser.userID;
            {
            }
            return pUser;
        }
        //------------------------------------------------------
        public static void SaveUserData()
        {
            if (UserManager.getInstance() == null)
            {
                return;
            }
            string strPath = FileSystemUtil.PersistenDataPath;
            var user = UserManager.getInstance().mySelf;

            
            strPath = Framework.Core.CommonUtility.stringBuilder.Append(strPath).Append("user_dbs_").Append(user.GetSdkUid()).ToString();
            if (!System.IO.Directory.Exists(strPath))
                System.IO.Directory.CreateDirectory(strPath);

            //! user
            {
                string userFile = Framework.Core.CommonUtility.stringBuilder.Append(strPath).Append("/user.json").ToString();
                System.IO.File.WriteAllText(userFile, JsonUtility.ToJson(user, true));
            }
            AProxyDB[] dbs = user.GetProxyDBs();
            if (dbs == null) return;
            for(int i =0; i < dbs.Length; ++i)
            {
                AProxyDB db = dbs[i];
                if (db == null) continue;
                EDBType dbType = (EDBType)DBTypeMapping.GetTypeIndex(db.GetType());
                string dbFile = Framework.Core.CommonUtility.stringBuilder.Append(strPath).Append("/").Append(dbType).Append(".json").ToString();
                string strContent = db.SerializeDB();
                if(!string.IsNullOrEmpty(strContent))
                    System.IO.File.WriteAllText(dbFile, strContent);
            }
        }
    }
}

