/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	FileSystem
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TopGame.Core
{
#if UNITY_EDITOR
    [System.Serializable]
    internal class FileSystemDebug
    {
        public List<string> buildDirs = new List<string>();
        public List<string> unbuildDirs = new List<string>();
    }
#endif
    //------------------------------------------------------
    public class FileSystem : AFileSystem
    {
        //------------------------------------------------------
        protected override void OnPreBuild()
        {
            m_bCoroutines = true;
#if UNITY_EDITOR
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath = Application.streamingAssetsPath;
            m_strStreamRawPath = Application.streamingAssetsPath + "/raws/";
            m_strStreamBinaryPath = Application.dataPath + "/../Binarys/";
            if (!Application.isPlaying) m_bCoroutines = false;
#elif UNITY_STANDALONE
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath = Application.streamingAssetsPath + "/packages/";
            m_strStreamRawPath = Application.streamingAssetsPath + "/packages/raws/";
            m_strStreamBinaryPath = m_strStreamRawPath + "Binarys/";
#elif UNITY_ANDROID
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath = Application.streamingAssetsPath + "/packages/";
            m_strStreamRawPath = "packages/raws/";
            m_strStreamBinaryPath = m_strStreamRawPath + "Binarys/";
#elif UNITY_IOS
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath =  Application.streamingAssetsPath + "/packages/";
            m_strStreamRawPath = Application.streamingAssetsPath + "/packages/raws/";
            m_strStreamBinaryPath = m_strStreamRawPath + "Binarys/";
#elif UNITY_WEBGL
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath =  Application.streamingAssetsPath + "/packages/";
            m_strStreamRawPath = Application.streamingAssetsPath + "/packages/raws/";
            m_strStreamBinaryPath = m_strStreamRawPath + "Binarys/";
#else
            m_strStreamPath = Application.streamingAssetsPath;
            m_strStreamPackagesPath =  Application.streamingAssetsPath + "/packages/";
            m_strStreamRawPath = Application.streamingAssetsPath + "/raws/";
            m_strStreamBinaryPath = Application.dataPath + "/../Binarys/";
#endif
#if UNITY_EDITOR
            bEditorMode = false;
#endif
            m_strAssetPath = Application.dataPath;
            m_strPersistentDataPath = Application.persistentDataPath + "/";
#if UNITY_EDITOR
            m_strPersistentDataPath = Application.dataPath + "/../Local/";
#endif
            m_strUpdateDataPath = Framework.Core.BaseUtil.stringBuilder.Append(m_strPersistentDataPath).Append("updates/").ToString();
            m_strLocalUpdateVersionFile = Framework.Core.BaseUtil.stringBuilder.Append(m_strPersistentDataPath).Append("localversion.txt").ToString();
            m_strLocalUpdateFile = Framework.Core.BaseUtil.stringBuilder.Append(m_strUpdateDataPath).Append("updates.json").ToString();
#if UNITY_EDITOR
            FileSystemDebug setting = null;
            string strPath = Application.dataPath + "/../Publishs/Setting.json";
            if (File.Exists(Application.dataPath + "/../Publishs/Setting_temp.json"))
            {
                strPath = Application.dataPath + "/../Publishs/Setting_temp.json";
            }
            if (System.IO.File.Exists(strPath))
            {
                try
                {
                    string strCode = File.ReadAllText(strPath, System.Text.Encoding.Default);
                    setting = JsonUtility.FromJson<FileSystemDebug>(strCode);
                    for(int i =0; i < setting.buildDirs.Count; ++i)
                        AddDynamicPath(setting.buildDirs[i]);

                    AddDynamicPath("Assets/DatasRef/Role/xxx/animations/");

                }
                catch (System.Exception ex)
                {
                }
            }
#endif
        }
        //------------------------------------------------------
        protected override void OnInnerAwake()
        {
        }
        //------------------------------------------------------
        protected override void OnInit()
        {
            m_nSreamReadBufferSize = 32 * 1024;
            if (m_eType == EFileSystemType.EncrptyPak)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                m_strStreamPackagesPath = "packages/";
                m_strStreamRawPath = "raws/";
                m_strStreamBinaryPath = "raws/Binarys/";
#elif UNITY_IOS
                m_strStreamRawPath = "raws/";
                m_strStreamBinaryPath = "raws/Binarys/";
#elif UNITY_WEBGL && !UNITY_EDITOR
                m_strStreamRawPath = "raws/";
                m_strStreamBinaryPath = "raws/Binarys/";
#elif UNITY_EDITOR
                m_strStreamPackagesPath = Application.dataPath + "/../Publishs/Packages/" + UnityEditor.EditorUserBuildSettings.activeBuildTarget + "/defualt/encrpty_packages/";
#endif
            }
        }
        //------------------------------------------------------
        protected override void OnShutdown()
        {

        }
        //------------------------------------------------------
        public override int GetFileSize(string strFile, bool bAbs)
        {
            return GameDelegate.GetFileSize(strFile, bAbs);
        }
        //------------------------------------------------------
        public override int ReadBuffer(string strFile, byte[] buffer, int dataSize, int bufferOffset, int offsetRead, bool bAbs)
        {
            return GameDelegate.ReadBuffer(strFile, buffer, dataSize, bufferOffset, offsetRead, bAbs);
        }
        //------------------------------------------------------
        public override byte[] ReadFile(string strFile, bool bAbs, ref int dataSize)
        {
            return GameDelegate.ReadFile(strFile, bAbs, ref dataSize);
        }
        //------------------------------------------------------
        public override void EnableCatchHandle(bool bFileCatch,int catchCount=64)
        {
            GameDelegate.EnableCatchHandle(bFileCatch, catchCount);
        }
        //------------------------------------------------------
        public override void DeleteAllPackages()
        {
            GameDelegate.DeleteAllPackages();
        }
        //------------------------------------------------------
        public override IntPtr LoadPackage(string strPakFile)
        {
            return GameDelegate.LoadPackage(strPakFile);
        }
        //------------------------------------------------------
        public override void UnloadPackage(string strPakFile)
        {
            GameDelegate.UnloadPackage(strPakFile);
        }
    }
}

