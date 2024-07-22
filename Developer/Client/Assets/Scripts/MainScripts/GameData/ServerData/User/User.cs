/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	User
作    者:	HappLI
描    述:   玩家数据
*********************************************************************/

using Proto3;
using System.Collections.Generic;
using System;
using UnityEngine;
using TopGame.Data;
using Framework.Core;
using TopGame.Logic;
using Framework.Plugin.AT;
using TopGame.Net;
using TopGame.Base;
using TopGame.UI;

namespace TopGame.SvrData
{
    public enum ELocationState
    {
        None = 0,
        City = 100,
        InCity = 100,

        OutCity = 200,
        Pve,
        Count
    }
    [System.Serializable]
    [Framework.Plugin.AT.ATExportMono("用户信息/用户")]
    public class User : Framework.Plugin.AT.IUserData
    {
        public static event Action OnNameChange;

        public long userID = 0;
        string m_strSDKUid = null;
        ELocationState m_LocationState = ELocationState.None;

        private AProxyDB[] m_arrProxyDBs = new AProxyDB[(int)EDBType.Count];
        private Framework.Module.AFrameworkBase m_pFramework;
        private long m_nLastLoginTime;
        private int m_nPlayerLevel = 1;
        public int PlayerLevel
        {
            get
            {
                return m_nPlayerLevel;
            }
            set 
            { 
                if (m_nPlayerLevel != value)
                {
                    int before = m_nPlayerLevel;
                    m_nPlayerLevel = value;
                    SDK.GameSDK.UpdateGameData(userID.ToString(), GetUserName(), m_nPlayerLevel, Data.LocalAccountCatch.GetLinkServer(), false, false);
                }
            }
        }
        //------------------------------------------------------
        public string GetSdkUid()
        {
            if (!string.IsNullOrEmpty(m_strSDKUid)) return m_strSDKUid;
            return userID.ToString();
        }
        //------------------------------------------------------
        public void SetSDKUid(string uid)
        {
            m_strSDKUid = uid;
            if (userID == 0) userID = Framework.Core.BaseUtil.StringToHashID64(uid);
        }
        //------------------------------------------------------
        public void ApplayData(IUserData userData)
        {
        }
        //------------------------------------------------------
        public User(Framework.Module.AFrameworkBase pFramework)
        {
            m_pFramework = pFramework;
        }
        //------------------------------------------------------
        public Framework.Module.AFrameworkBase GetFramework()
        {
            return m_pFramework;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod, Framework.Plugin.AT.ATDefaultPointer("TopGame.SvrData.UserManager.getInstance().mySelf")]
        public void SetLocationState(ELocationState location)
        {
            if (m_LocationState == location) return;
            m_LocationState = location;
            Framework.Plugin.AT.AgentTreeManager.Execute((ushort)Base.EATEventType.LocationState, 1, new Variable1() { intVal = (int)location });
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod, Framework.Plugin.AT.ATDefaultPointer("TopGame.SvrData.UserManager.getInstance().mySelf")]
        public ELocationState GetLocationState()
        {
            return m_LocationState;
        }
        //------------------------------------------------------
        public EGameState GetGameStateWithLocationState(ELocationState locationState)
        {
            switch (locationState)
            {
                case ELocationState.InCity:
                    return EGameState.Hall;
                default:
                    return EGameState.Battle;
            }
        }
        //------------------------------------------------------
        public T ProxyDB<T>(EDBType type = EDBType.Count) where T : AProxyDB, new()
        {
            int typeIndex = (int)type;
            if (type == EDBType.Count) typeIndex = DBTypeMapping.GetTypeIndex(typeof(T));
            if (typeIndex<0 || typeIndex >= m_arrProxyDBs.Length ) return null;
            T proxy = m_arrProxyDBs[typeIndex] as T;
            if(proxy == null)
            {
                proxy = new T();
                proxy.Init(this);
                m_arrProxyDBs[typeIndex] = proxy;
            }
            return proxy;
        }
        //------------------------------------------------------
        public void UnSerializeProxyDB(EDBType type, string jsonData)
        {
            System.Type dbType = DBTypeMapping.GetDBType(type);
            if (dbType == null) return;
            int typeIndex = (int)type;
            m_arrProxyDBs[typeIndex] = DBTypeMapping.NewProxyDB(type, this);
            if(!m_arrProxyDBs[typeIndex].UnSerializeDB(jsonData))
                JsonUtility.FromJsonOverwrite(jsonData, m_arrProxyDBs[typeIndex]);
        }
        //------------------------------------------------------
        public string SerializeProxyDB(EDBType type)
        {
            int typeIndex = (int)type;
            if (m_arrProxyDBs == null || typeIndex < 0 || typeIndex >= m_arrProxyDBs.Length) return null;
            string json = m_arrProxyDBs[typeIndex].SerializeDB();
            if (!string.IsNullOrEmpty(json)) return json;
            return JsonUtility.ToJson(m_arrProxyDBs[typeIndex], true);
        }
        //------------------------------------------------------
        public AProxyDB[] GetProxyDBs()
        {
            return m_arrProxyDBs;
        }
        //------------------------------------------------------
        public BaseDB GetBaseDB()
        {
            return ProxyDB<BaseDB>(EDBType.BaseInfo);
        }
        //------------------------------------------------------
        public void Clear()
        {
            userID = 0;
            m_strSDKUid = null;
            for (int i =0; i < m_arrProxyDBs.Length; ++i)
            {
                if (m_arrProxyDBs[i] != null)
                    m_arrProxyDBs[i].Clear();
            }
            m_nLastLoginTime = 0;
        }
        //------------------------------------------------------
        public void Destroy()
        {
            Clear();
        }
        //------------------------------------------------------
        public string GetUserName()
        {
            return ProxyDB<BaseDB>(EDBType.BaseInfo).UserName;
        }
        //------------------------------------------------------
        public void SetUserName(string userName)
        {
            ProxyDB<BaseDB>(EDBType.BaseInfo).UserName = userName;
            OnNameChange?.Invoke();
        }
        //------------------------------------------------------
        public uint GetHead()
        {
            return GetBaseDB().headID;
        }
        //------------------------------------------------------
        public void SetLastLoginTime(long time)
        {
            m_nLastLoginTime= time;
        }
        //------------------------------------------------------
        public long GetLastLoginTime()
        {
            return m_nLastLoginTime;
        }
        //------------------------------------------------------
        public void SetBaseInfo(UserBaseInfoResponse response)
        {
            //更新用户信息
            PlayerLevel = response.PlayerLevel;
            SetUserName(response.NickName);
        }
    }
}

