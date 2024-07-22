/********************************************************************
生成日期:	1:11:2020 10:05
类    名: 	DataManager
作    者:	HappLI
描    述:	
*********************************************************************/

using Framework.Core;
using Framework.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Data
{
    [Framework.Plugin.AT.ATExportMono("配置数据", Framework.Plugin.AT.EGlobalType.Single)]
    public partial class DataManager : ADataManager
    {
        private static System.IO.MemoryStream m_pMemoryStream = null;
        private static System.IO.BinaryReader m_pBinaryReader = null;
        private static DataManager ms_pInstance = null;
#if UNITY_EDITOR
        private static DataManager ms_pEditorInstance = null;
        public static void SetEditor(DataManager pInstance)
        {
            ms_pEditorInstance = pInstance;
        }
#endif

        Dictionary<string, BaseData> m_vBinaryDatas = null;
        //-------------------------------------------
        public static DataManager getInstance()
        {
#if UNITY_EDITOR
            if (ms_pInstance != null) return ms_pInstance;
            if (!Application.isPlaying)
            {
                return ms_pEditorInstance;
            }
#endif
            return ms_pInstance;
        }
        //-------------------------------------------
        protected override void OnAwake()
        {
            ms_pInstance = this;
        }
        //-------------------------------------------
        protected override void OnDestroy()
        {
            ms_pInstance = null;
        }
        //-------------------------------------------
        public static bool ReCheckInit()
        {
            if (ms_pInstance == null) return false;
            return StartInit();
        }
        //-------------------------------------------
        public static bool StartInit(string dataFile = "Assets/Datas/Config/GameDatas.asset")
        {
            if (ms_pInstance == null) return false;
            return ms_pInstance.Init(dataFile);
        }
        //-------------------------------------------
        public static bool ReInit()
        {
            if (ms_pInstance == null) return false;
            ms_pInstance.bInited = false;
            return ms_pInstance.Init();
        }
        //-------------------------------------------
        protected override void OnParserOver()
        {
            base.OnParserOver();
        }
        //-------------------------------------------
        public void UnloadBinary(string strFile)
        {
            if (string.IsNullOrEmpty(strFile)) return;
            if (m_vBinaryDatas != null) m_vBinaryDatas.Remove(strFile);
        }
        //-------------------------------------------
        public static T GetBinary<T>(string strFile) where T : BaseData
        {
            if (getInstance() == null) return null;
            if (string.IsNullOrEmpty(strFile)) return null;
            BaseData getData = null;
            if (getInstance().m_vBinaryDatas != null && getInstance().m_vBinaryDatas.TryGetValue(strFile, out getData))
            {
                return getData as T;
            }

            BaseData newData = null;
            if(newData!=null)
            {
                if (getInstance().m_vBinaryDatas == null) getInstance().m_vBinaryDatas = new Dictionary<string, BaseData>();
                getInstance().m_vBinaryDatas[strFile] = newData;
                return newData as T;
            }
            return null;
        }
        //-------------------------------------------
        public static System.IO.BinaryReader BeginBinary(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0) return null;
            if (m_pBinaryReader == null)
            {
                m_pMemoryStream = new System.IO.MemoryStream();
                m_pBinaryReader = new System.IO.BinaryReader(m_pMemoryStream);
            }
            m_pMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            m_pMemoryStream.SetLength(bytes.Length);
            m_pMemoryStream.Write(bytes, 0, bytes.Length);
            m_pMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            return m_pBinaryReader;
        }
        //-------------------------------------------
        public static void EndBinary()
        {
            if (m_pMemoryStream != null)
                m_pMemoryStream.SetLength(0);
        }
    }
}
