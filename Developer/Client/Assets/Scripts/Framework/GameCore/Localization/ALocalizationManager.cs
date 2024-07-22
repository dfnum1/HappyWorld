/********************************************************************
生成日期:	2020-06-12
类    名: 	LocalizationManager
作    者:	zdq
描    述:	多语言管理器
*********************************************************************/

using Framework.Core;
using Framework.Data;
using System.Collections;
using System.Collections.Generic;
using TopGame.Data;
using UnityEngine;
namespace TopGame.Core
{
    public abstract class ALocalizationManager : Framework.Module.AModule
    {
        protected static HashSet<SystemLanguage> ms_vLanguagesSets = new HashSet<SystemLanguage>();
        protected Dictionary<long, string> m_vLanguageDic;
        protected SystemLanguage m_eLanguageType = SystemLanguage.Unknown;
        static string ms_strLanguageKey = null;

        FileDownload m_pFileDownload = null;

        public static string SD_LANGUAGE_KEY
        {
            get
            {
                if(string.IsNullOrEmpty(ms_strLanguageKey))
                {
                    ms_strLanguageKey = Application.identifier + "_SD_CODE_LANGUAGE";
                }
                return ms_strLanguageKey;
            }
        }
        public delegate void OnLanguageChangeDelegate(SystemLanguage languageType);
        public static OnLanguageChangeDelegate OnLanguageChangeEvent;
        protected override void Awake() 
        {
            m_vLanguageDic = new Dictionary<long, string>();
            if (ms_PreLanguage != null)
            {
                foreach (var db in ms_PreLanguage)
                {
                    m_vLanguageDic[db.Key] = db.Value;
                }
                ms_PreLanguage = null;
            }
            m_eLanguageType = GetLocalSaveLanguage();
        }
        //-----------------------------------------------------
        protected override void OnDestroy()
        {
            if (m_vLanguageDic != null) m_vLanguageDic.Clear();
            OnLanguageChangeEvent = null;
            if (m_pFileDownload != null) m_pFileDownload.Abort();
            m_pFileDownload = null;
        }
        //-----------------------------------------------------
        public bool RefreshLanguage()
        {
            //! another lanauge
            CsvParser csvParser = null;
            bool bSucceed = false;
            string strLanguageFile = CommonUtility.stringBuilder.Append(FileSystemUtil.UpdateDataPath).Append("languages/").Append("language_").Append(m_eLanguageType).ToString();
            if (System.IO.File.Exists(strLanguageFile))
            {
                csvParser = new CsvParser();
                if (!csvParser.LoadTableString(System.IO.File.ReadAllText(strLanguageFile)))
                    csvParser = null;
            }
            else
            {
                strLanguageFile = CommonUtility.stringBuilder.Append(FileSystemUtil.StreamBinaryPath).Append("languages/").Append("language_").Append(m_eLanguageType).ToString();
                string text = JniPlugin.ReadFileAllText(strLanguageFile, true);
                if (!string.IsNullOrEmpty(text))
                {
                    csvParser = new CsvParser();
                    if (!csvParser.LoadTableString(text))
                        csvParser = null;
                }
            }
            try
            {
                if (csvParser != null)
                {
                    int i = csvParser.GetTitleLine();
                    if (i >= 0)
                    {
                        int nLineCnt = csvParser.GetLineCount();
                        for (i++; i < nLineCnt; i++)
                        {
                            if (!csvParser[i]["id"].IsValid()) continue;

                            uint keyId = csvParser[i]["id"].Uint();
                            string text = csvParser[i][1].String();
                            if (text != null)
                            {
                                AddLanguage(keyId, m_eLanguageType, text);
                            }
                        }
                        bSucceed = true;
                    }
                    if(bSucceed)
                    {
                        ms_vLanguagesSets.Add(m_eLanguageType);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Framework.Plugin.Logger.Warning(ex.ToString());
            }
            return bSucceed;
        }
        //-----------------------------------------------------
        public void SetCurrentLanguage(SystemLanguage languageType)
        {
            if (m_eLanguageType != languageType)
            {
                if(!CheckDownloadLanguage(languageType))
                {
                    OnSetLanguage(languageType);
                }
            }
        }
        //-----------------------------------------------------
        bool CheckDownloadLanguage(SystemLanguage language, bool bAutoDownload= true)
        {
            if(ms_vLanguagesSets.Contains(language))
            {
                return false;
            }
            if(m_pFileDownload!=null)
            {
                if(m_pFileDownload.isFailed)
                {
                    m_pFileDownload = null;
                }
                if (!m_pFileDownload.isDone)
                {
                    return true;
                }
            }
            string languageShortName = TopGame.Base.GlobalUtil.GetSystemLanuage(language);
            string strLanguageFile = CommonUtility.stringBuilder.Append(FileSystemUtil.UpdateDataPath).Append("languages/").Append(languageShortName).Append(".txt").ToString();
            if (System.IO.File.Exists(strLanguageFile))
            {
                return false;
            }
            else
            {
                strLanguageFile = CommonUtility.stringBuilder.Append(FileSystemUtil.StreamBinaryPath).Append("languages/").Append(languageShortName).Append(".txt").ToString();
                if (JniPlugin.ExistFile(strLanguageFile, true))
                {
                    return false;
                }
            }
            if(bAutoDownload)
            {
                string versionsUrl = SvrData.ServerList.GetHotAddress();
                if (string.IsNullOrEmpty(versionsUrl))
                    return false;
                string languageDir = CommonUtility.stringBuilder.Append(versionsUrl).Append("languages").ToString();
                string languageFile = CommonUtility.stringBuilder.Append(languageShortName).Append(".txt").ToString();

                if (m_pFileDownload == null) m_pFileDownload = new FileDownload(languageDir, CommonUtility.stringBuilder.Append(FileSystemUtil.StreamBinaryPath).Append("languages/").ToString(), languageFile);
                else m_pFileDownload.Reset(languageDir, CommonUtility.stringBuilder.Append(FileSystemUtil.StreamBinaryPath).Append("languages/").ToString(), languageFile);
                m_pFileDownload.userCallData = new Framework.Core.Variable1() { intVal = (int)language };
                m_pFileDownload.OnDownloaded = OnLoadLanguageFile;
                m_pFileDownload.Start();
            }

            return true;
        }
        //------------------------------------------------------
        void OnLoadLanguageFile(FileDownload file)
        {
            if(file.isDone)
            {
                if(file.userCallData!=null && file.userCallData is Framework.Core.Variable1)
                OnSetLanguage((SystemLanguage)((Framework.Core.Variable1)file.userCallData).intVal);
            }
            m_pFileDownload = null;
        }
        //------------------------------------------------------
        void OnSetLanguage(SystemLanguage languageType)
        {
            if (m_eLanguageType == languageType) return;
            m_eLanguageType = languageType;
            RefreshLanguage();
            UpdateFont();

            if (OnLanguageChangeEvent != null)
            {
                OnLanguageChangeEvent.Invoke(m_eLanguageType);
            }
            OnLanguageChanged();
        }
        //------------------------------------------------------
        public void UpdateFont()
        {
            //设置当前语言的字体
            string strFont = GetLocalization(1);
            if (!string.IsNullOrEmpty(strFont))
            {
                UI.FontSwitch.SwitchFont(strFont);
            }
        }
        //-----------------------------------------------------
        public void AddLanguage(uint id, SystemLanguage languageType, string text)
        {
            if (text == null) return;
            m_vLanguageDic[BuildLanguageKey(id, languageType)] = text;
        }
        //-----------------------------------------------------
        public string GetLocalization(uint id, string strDefault = null)
        {
            if (m_vLanguageDic == null)
            {
                return strDefault;
            }
            long key = BuildLanguageKey(id, m_eLanguageType);
            string strTemp;
            if (m_vLanguageDic.TryGetValue(key, out strTemp))
            {
                return strTemp;
            }
            return strDefault;
        }
        //-----------------------------------------------------
        protected abstract void OnLanguageChanged();
        //------------------------------------------------------
        protected static long BuildLanguageKey(uint id, SystemLanguage languageType)
        {
            if (languageType == SystemLanguage.ChineseSimplified) languageType = SystemLanguage.Chinese;
            return (long)((long)id*1000 + (int)languageType);
        }
        //------------------------------------------------------
        public static SystemLanguage GetLocalSaveLanguage()
        {
            SystemLanguage systemLang = Application.systemLanguage;
            if (PlayerPrefs.HasKey(ALocalizationManager.SD_LANGUAGE_KEY))
            {
                systemLang = (SystemLanguage)PlayerPrefs.GetInt(ALocalizationManager.SD_LANGUAGE_KEY, (int)Application.systemLanguage);
            }
            return systemLang;
        }
        //------------------------------------------------------
        public static string ToLocalization(uint id, string strDefault = null)
        {
            if (Framework.Module.ModuleManager.mainFramework == null) return ToPreLocalization((int)id, strDefault);
            GameFramework gameFramework = Framework.Module.ModuleManager.mainFramework as GameFramework;
            if (gameFramework == null || gameFramework.localizationMgr == null) return strDefault;
            return gameFramework.localizationMgr.GetLocalization(id, strDefault);
        }
        //-----------------------------------------------------
        public static string ToLocalization(int locationId, string strDefault = null)
        {
            return ToLocalization((uint)locationId, strDefault);
        }
        //-----------------------------------------------------
        public static void SetLanguage(SystemLanguage languageType)
        {
            if (Framework.Module.ModuleManager.mainFramework == null) return;
            Core.GameFramework gameFrame = Framework.Module.ModuleManager.mainFramework as Core.GameFramework;
            if (gameFrame == null || gameFrame.localizationMgr == null) return;
            gameFrame.localizationMgr.SetCurrentLanguage(languageType);
        }
        //-----------------------------------------------------
        public static bool NeedLoaddownCheck(SystemLanguage languageType)
        {
            if (Framework.Module.ModuleManager.mainFramework == null) return false;
            Core.GameFramework gameFrame = Framework.Module.ModuleManager.mainFramework as Core.GameFramework;
            if (gameFrame == null || gameFrame.localizationMgr == null) return false;
            return gameFrame.localizationMgr.CheckDownloadLanguage(languageType, false);
        }
        //-----------------------------------------------------
        static Dictionary<long,string> ms_PreLanguage = null;
        internal static string ToPreLocalization(int locationId, string strDefault = null)
        {
            SystemLanguage language = GetLocalSaveLanguage();
            if (ms_PreLanguage == null)
            {
                ms_PreLanguage = new Dictionary<long, string>();
                TextAsset text = Resources.Load<TextAsset>("language");
                if (text != null)
                {
                    CsvParser csvParser = new CsvParser();
                    if(csvParser.LoadTableString(text.text))
                    {
                        int i = csvParser.GetTitleLine();
                        if (i >= 0)
                        {
                            int nLineCnt = csvParser.GetLineCount();
                            for (i++; i < nLineCnt; i++)
                            {
                                if (!csvParser[i]["id"].IsValid()) continue;

                                uint keyId = csvParser[i]["id"].Uint();
                                string textCN = csvParser[i]["textCN"].String();
                                string textEN = csvParser[i]["textEN"].String();
                                ms_PreLanguage[BuildLanguageKey(keyId, SystemLanguage.ChineseSimplified)] = textCN;
                                ms_PreLanguage[BuildLanguageKey(keyId, SystemLanguage.English)] = textEN;
                            }

                            ms_vLanguagesSets.Add(SystemLanguage.Chinese);
                            ms_vLanguagesSets.Add(SystemLanguage.ChineseSimplified);
                            ms_vLanguagesSets.Add(SystemLanguage.English);
                        }
                    }
                }
            }
            string strTemp = null;
            long key = BuildLanguageKey((uint)locationId, language);
            if (ms_PreLanguage.TryGetValue(key, out strTemp) )
            {
                return strTemp;
            }
            return strDefault;
        }
    }

}
