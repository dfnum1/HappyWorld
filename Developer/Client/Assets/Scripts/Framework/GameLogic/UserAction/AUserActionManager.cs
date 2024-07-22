/********************************************************************
生成日期:	2020-06-12
类    名: 	UserActionManager
作    者:	zdq
描    述:	用户行为管理器
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopGame.Net;
using System.IO;
using Framework.Core;

namespace TopGame.Core
{
    public abstract class AUserActionManager : Framework.Module.AModule, Framework.Module.IStartUp, Framework.Module.IUpdate
    {
        [System.Serializable]
        public struct LogEventParam
        {
            public string key;
            public string value;
        }
        [System.Serializable]
        public struct SvrUserAction
        {
            public bool syncServer;
            public string eventName;
            public long time;
            public List<LogEventParam> propertys;
            public static SvrUserAction DEF = new SvrUserAction() { eventName = null, propertys = null, syncServer = false, time= 0 };
            public void AddKV(string key, string value)
            {
                if (string.IsNullOrEmpty(key)) return;
                if (propertys == null) propertys = new List<LogEventParam>(2);
                propertys.Add(new LogEventParam() { key = key, value = value });
            }
            public void Clear()
            {
                syncServer = false;
                eventName = null;
                time = 0;
                if (propertys != null) propertys.Clear();
            }
        }
        [System.Serializable]
        struct AllSaveDataStruct
        {
            public List<SvrUserAction> datas;

            public AllSaveDataStruct(List<SvrUserAction> userActionCache)
            {
                datas = userActionCache;
            }
        }

        protected List<SvrUserAction> m_UserActionCache = new List<SvrUserAction>(15);

        protected string m_LocalCacheFilePath = "";

        protected float m_nSendInterval = 10f;
        protected float m_nTimer = 0;

        private static SvrUserAction ms_TempAction;
        protected override void Awake()
        {
            m_UserActionCache.Clear();
            m_nTimer = 0; 

            string filePath = FileSystemUtil.PersistenDataPath + "UserActionCache.json";
            m_LocalCacheFilePath = filePath;

            //读取本地是否有缓存数据,有的话读取到列表中,发送给服务器后进行删除
            ReadLocalData();
        }
        //------------------------------------------------------
        public void StartUp()
        {
            //这边是所有配置初始化完后,调用,在读取配置的时候使用
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            SaveDataToLocal();
        }
        //------------------------------------------------------
        public void Update(float fFrame)
        {
            //间隔一段时间发送缓存中的消息,并清空
            m_nTimer += fFrame;
            if (m_nTimer >= m_nSendInterval)
            {
                SendToServer();
                m_nTimer = 0;
            }
        }
        //------------------------------------------------------
        public void SaveDataToLocal()
        {
            //在崩溃,或者关闭时,保存当前缓存列表中的数据到本地,下次打开进行发送
            if (string.IsNullOrWhiteSpace(m_LocalCacheFilePath) || m_UserActionCache.Count == 0)
            {
                return;
            }
            if (FileSystemUtil.GetFileSystem() == null) return;
            if(!Directory.Exists(FileSystemUtil.PersistenDataPath))
            {
                Directory.CreateDirectory(FileSystemUtil.PersistenDataPath);
            }

            string json = JsonUtility.ToJson(new AllSaveDataStruct(m_UserActionCache), true);
            FileStream fs = new FileStream(m_LocalCacheFilePath, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            fs.Position = 0;
            fs.SetLength(0);
            writer.Write(json);
            writer.Close();
        }
        //------------------------------------------------------
        protected void ReadLocalData()
        {
            //读取消息
            if (!File.Exists(m_LocalCacheFilePath))
            {
                return;
            }
            string json = File.ReadAllText(m_LocalCacheFilePath);
            AllSaveDataStruct cache = JsonUtility.FromJson<AllSaveDataStruct>(json);
            if (cache.datas == null)
            {
                return;
            }
            foreach (var data in cache.datas)
            {
                AddSvrAction(data);
            }
            
        }
        //------------------------------------------------------
        protected void AddAction(UI.UserActionData userAction)
        {
            AddAction(userAction.eventName, userAction.strProperty, userAction.strValue, userAction.bImmelaySendSvr);
        }
        //------------------------------------------------------
        protected virtual void AddAction(string eventName, string propertyName = null, string strValue = null, bool bImmelaySendSvr = false)
        {
            if (string.IsNullOrEmpty(eventName))
                return;
            SvrUserAction action = new SvrUserAction();
            action.AddKV(propertyName, strValue);
            action.eventName = eventName;
            AddSvrAction(action);
            if (bImmelaySendSvr)
                SendToServer();
        }
        //------------------------------------------------------
        protected void AddSvrAction(SvrUserAction userAction, bool isFirst = false)
        {
            if (string.IsNullOrEmpty(userAction.eventName))
                return;
            if (userAction.syncServer)
            {
                var data = new SvrUserAction();
                data.eventName = userAction.eventName;
                data.time = userAction.time;
                foreach (LogEventParam vLogEventParam in userAction.propertys)
                {
                    data.AddKV(vLogEventParam.key, vLogEventParam.value);
                }
                if (isFirst)
                {
                    m_UserActionCache.Insert(0,data);
                }
                else
                {
                    m_UserActionCache.Add(data);
                }
            }
            OnRecordAction(userAction);
        }
        //------------------------------------------------------
        protected virtual void OnRecordAction(SvrUserAction userAction) { }
        //------------------------------------------------------
        public void AddActionAndSendToServer(UI.UserActionData userAction)
        {
            AddAction(userAction);
            SendToServer();
            m_nTimer = 0;
        }
        //------------------------------------------------------
        public abstract void SendToServer();
        //------------------------------------------------------
        public static void AddRecordAction(string eventName, string propertyName, int value, bool bImmelaySendSvr = false)
        {
            if (Framework.Module.ModuleManager.mainModule == null) return;
            var framework = Framework.Module.ModuleManager.mainModule as GameFramework;
            if (framework == null || framework.userActionMgr == null) return;
            framework.userActionMgr.AddAction(eventName, propertyName, value.ToString(), bImmelaySendSvr);
        }
        //------------------------------------------------------
        public static void AddRecordAction(string eventName, string propertyName, string strProperty, bool bImmelaySendSvr = false)
        {
            if (Framework.Module.ModuleManager.mainModule == null) return;
            var framework = Framework.Module.ModuleManager.mainModule as GameFramework;
            if (framework == null || framework.userActionMgr == null) return;
            framework.userActionMgr.AddAction(eventName, propertyName, strProperty, bImmelaySendSvr);
        }
        //------------------------------------------------------
        public static void AddRecordAction(UI.UserActionData userAction)
        {
            if (string.IsNullOrEmpty(userAction.eventName)) return;
            if (Framework.Module.ModuleManager.mainModule == null) return;
            var framework = Framework.Module.ModuleManager.mainModule as GameFramework;
            if (framework == null || framework.userActionMgr == null) return;
            framework.userActionMgr.AddAction(userAction);
        }
        //------------------------------------------------------
        public static void AddActionKV(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;
            ms_TempAction.AddKV(key, value);
        }
        //------------------------------------------------------
        public static void AddActionKV(string key, int value)
        {
            AddActionKV(key, value.ToString());
        }
        //------------------------------------------------------
        public static void AddActionKV(string key, float value)
        {
            AddActionKV(key, value.ToString());
        }
        //------------------------------------------------------
        public static void AddActionKV(string key, long value)
        {
            AddActionKV(key, value.ToString());
        }
        //------------------------------------------------------
        public static void AddActionKV(string key, bool value)
        {
            AddActionKV(key, value?"1":"0");
        }
        //------------------------------------------------------
        public static void LogActionEvent(string eventName, bool syncServer = false, bool bImmde = false, bool isFirst = false)
        {
            if (string.IsNullOrEmpty(eventName) || Framework.Module.ModuleManager.mainModule == null) return;
            var framework = Framework.Module.ModuleManager.mainModule as GameFramework;
            if (framework == null || framework.userActionMgr == null) return;
            ms_TempAction.eventName = eventName;
            ms_TempAction.time = Framework.Base.TimeHelper.Now();
            ms_TempAction.syncServer = syncServer;
            framework.userActionMgr.AddSvrAction(ms_TempAction, isFirst);
            if (bImmde) framework.userActionMgr.SendToServer();
            ms_TempAction.Clear();
        }
    }
}

