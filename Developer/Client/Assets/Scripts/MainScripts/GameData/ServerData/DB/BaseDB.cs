#if !USE_SERVER
/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AProxyDB
作    者:	HappLI
描    述:	服务器数据
*********************************************************************/
using System;
using UnityEngine;
using TopGame.Data;
namespace TopGame.SvrData
{

    [DBType(EDBType.BaseInfo)]
    public partial class BaseDB : AProxyDB
    {
        public string UserName;
        public uint headID = 0;

        public long OnlineTime { get;private set; }

        //------------------------------------------------------
        protected override void OnInit()
        {
            Clear();
        }
        //------------------------------------------------------
        public override void Clear()
        {
            UserName = "";
        }
        //------------------------------------------------------
        public void SetOnlineTime(long time)
        {
            OnlineTime = time;
        }
    }
}
#endif