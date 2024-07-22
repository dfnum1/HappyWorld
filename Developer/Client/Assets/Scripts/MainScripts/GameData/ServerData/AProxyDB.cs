/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AProxyDB
作    者:	HappLI
描    述:	服务器数据
*********************************************************************/
using UnityEngine;
namespace TopGame.Data
{
    public abstract class AProxyDB : Framework.Plugin.AT.IUserData
    {
        protected SvrData.User m_pUser;
        public void Init(SvrData.User user)
        {
            m_pUser = user;
            OnInit();
        }
        //------------------------------------------------------
        public string SerializeDB()
        {
            return OnSerializeDB();
        }
        //------------------------------------------------------
        public bool UnSerializeDB(string jsonContent)
        {
            return OnUnSerializeDB(jsonContent);
        }
        protected virtual void OnInit() { }
        protected virtual bool OnUnSerializeDB(string jsonContent) { return false; }
        protected virtual string OnSerializeDB() { return null; }
        //------------------------------------------------------
        public virtual void Clear() { }
        //------------------------------------------------------
        public virtual void ApplayData(Framework.Plugin.AT.IUserData userData)
        {

        }
        //------------------------------------------------------
        public virtual void Destroy()
        {
            m_pUser = null;
            Clear();
        }
    }
}
