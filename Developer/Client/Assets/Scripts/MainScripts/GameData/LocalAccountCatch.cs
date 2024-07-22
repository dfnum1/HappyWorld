/********************************************************************
生成日期:	7:2:2019   18:28
类    名: 	LocalAccountCatch
作    者:	Happli
描    述:	本地临时数据管理
*********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TopGame.Data
{
    public static class LocalAccountCatch
    {
        public static void SetAccountData(string user, string userName, string password, string serverName)
        {
            UnityEngine.PlayerPrefs.SetString("OW_PlayerUser", user);
            UnityEngine.PlayerPrefs.SetString("OW_PlayerUserName", userName);
            UnityEngine.PlayerPrefs.SetString("OW_PlayerPassword", password);
            UnityEngine.PlayerPrefs.SetString("OW_ServerName", serverName);
        }
        //------------------------------------------------------
        public static void SetAccount(string userName)
        {
            PlayerPrefs.SetString("OW_PlayerUser", userName);
        }
        //------------------------------------------------------
        public static string GetAccount()
        {
            if (HasKey("OW_PlayerUser")) return PlayerPrefs.GetString("OW_PlayerUser");
            return "";
        }
        //------------------------------------------------------
        public static void SetUserName(string userName)
        {
            PlayerPrefs.SetString("OW_PlayerUserName", userName);
        }
        //------------------------------------------------------
        public static string GetUserName()
        {
            return PlayerPrefs.GetString("OW_PlayerUserName");
        }
        //------------------------------------------------------
        public static void SetPassword(string password)
        {
            PlayerPrefs.SetString("OW_PlayerPassword", password);
        }
        //------------------------------------------------------
        public static string GetPassword()
        {
            if(HasKey("OW_PlayerPassword")) return PlayerPrefs.GetString("OW_PlayerPassword");
            return "";
        }
        //------------------------------------------------------
        public static void SetLinkServer(string server)
        {
            PlayerPrefs.SetString("OW_ServerName", server);
        }
        //------------------------------------------------------
        public static string GetLinkServer()
        {
            if (HasKey("OW_ServerName")) return PlayerPrefs.GetString("OW_ServerName");
            return "";
        }
        //------------------------------------------------------
        public static bool HasAccountCache()
        {
            if (string.IsNullOrEmpty(GetAccount())) return false;
            return true;
        }
        //------------------------------------------------------
        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
        //------------------------------------------------------
        public static int GetLoginSceneIndex()
        {
            return PlayerPrefs.GetInt("OW_LoginSceneIndex", 0);
        }
        //------------------------------------------------------
        public static void SetLoginSceneIndex(int index)
        {
            PlayerPrefs.SetInt("OW_LoginSceneIndex", index);
        }
    }
}