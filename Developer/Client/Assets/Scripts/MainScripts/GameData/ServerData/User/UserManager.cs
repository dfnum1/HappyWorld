/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	AnimPathData
作    者:	HappLI
描    述:   路径动画数据
*********************************************************************/

using System;
using System.Collections.Generic;

namespace TopGame.SvrData
{
    [Framework.Plugin.AT.ATExportMono("用户信息", "TopGame.GameInstance.getInstance().userManager")]
    public class UserManager : Framework.Module.AModule
    {
        List<User> m_vOthers = null;

#if !USE_SERVER
        User m_pMySelf = null;
        [Framework.Plugin.AT.ATField("我自己")]
        public User mySelf
        {
            get { return m_pMySelf; }
            set { m_pMySelf = value; }
        }
        private static UserManager ms_pInstance = null;
        //------------------------------------------------------
        public static User Current
        {
            get
            {
                if (ms_pInstance == null) return null;
                return ms_pInstance.mySelf;
            }
        }
        //------------------------------------------------------
        public static User MySelf
        {
            get
            {
                if (ms_pInstance == null) return null;
                return ms_pInstance.mySelf;

            }
        }
        public static UserManager getInstance()
        {
            return ms_pInstance;
        }     
#endif
        //------------------------------------------------------
        public UserManager()
        {
        }
        //------------------------------------------------------
        protected override void Awake()
        {
#if !USE_SERVER
            ms_pInstance = this;
            m_pMySelf = new User(GetFramework());
#endif
            m_vOthers = new List<User>(4);
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取用户")]
        public User GetUser(long userID)
        {
#if !USE_SERVER
            if (m_pMySelf.userID == userID) return m_pMySelf;
#endif
            for (int i = 0; i < m_vOthers.Count; ++i)
            {
                if (m_vOthers[i].userID == userID)
                    return m_vOthers[i];
            }
            return null;
        }
#if !USE_SERVER
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取当前用户")]
        public User GetCurUser()
        {
            return mySelf;
        }
#endif
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("添加用户")]
        public User AddUser(long userID)
        {
            for (int i = 0; i < m_vOthers.Count; ++i)
            {
                if (m_vOthers[i].userID == userID)
                    return m_vOthers[i];
            }
            User newUser = new User(GetFramework());
            newUser.userID = userID;
            m_vOthers.Add(newUser);
            return newUser;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("清理用户列表")]
        public void ClearUser(bool bIncludeMyself = true)
        {
#if !USE_SERVER
            if (bIncludeMyself) m_pMySelf.Clear();
#endif
            for (int i = 0; i < m_vOthers.Count; ++i)
                m_vOthers[i].Clear();
            m_vOthers.Clear();
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            ClearUser();
#if !USE_SERVER
            ms_pInstance = null;
#endif
        }
    }
}

