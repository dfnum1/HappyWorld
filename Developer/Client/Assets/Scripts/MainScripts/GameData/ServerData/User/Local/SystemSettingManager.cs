/********************************************************************
生成日期:	2020-6-16
类    名: 	SystemSettingManager
作    者:	zdq
描    述:	管理第一次登录后,进行配置读取加载的情况
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TopGame.Data;
using UnityEngine;
using static TopGame.Core.LocalizationManager;

namespace TopGame.Core
{

    [Framework.Plugin.AT.ATExportMono("系统设置管理", Framework.Plugin.AT.EGlobalType.Single)]
    public class SystemSettingManager : TopGame.Base.Singleton<SystemSettingManager>, Framework.Plugin.AT.IUserData
    {
        SystemSettingData m_SystemSettingData = null;

        public const string SD_BGM = "SD_BGM";
        public const string SD_SOUNDEFFECT = "SD_SOUNDEFFECT";
        public const string SD_QUALITY = TopGame.Data.GameQuality.QUALITY_KEY;
        public const string SD_BGMVOLUMN = "SD_BGMVOLUMN";
        public const string SD_SOUNDEFFECTVOLUMN = "SD_SOUNDEFFECTVOLUMN";
        public const string SD_VIBRATE = "SD_VIBRATE";

        //------------------------------------------------------
        public SystemSettingData GetSystemSettingData()
        {
            int soundEffect = 1;
            int bGM = 1;
            int GameQuality = (int)EGameQulity.High;
            SystemLanguage language = Application.systemLanguage;
            float bgVolumn = 1;
            float soundEffectVolumn = 1;
            int bVibrateDevice = 1;
            if (PlayerPrefs.HasKey(SD_BGM))
            {
                bVibrateDevice = PlayerPrefs.GetInt(SD_VIBRATE);
            }
            if (PlayerPrefs.HasKey(SD_BGM))
            {
                bGM = PlayerPrefs.GetInt(SD_BGM);
            }
            if (PlayerPrefs.HasKey(SD_SOUNDEFFECT))
            {
                soundEffect = PlayerPrefs.GetInt(SD_SOUNDEFFECT);
            }
            if (PlayerPrefs.HasKey(TopGame.Data.GameQuality.QUALITY_KEY))
            {
                GameQuality = PlayerPrefs.GetInt(TopGame.Data.GameQuality.QUALITY_KEY);
            }
            if (PlayerPrefs.HasKey(ALocalizationManager.SD_LANGUAGE_KEY))
            {
                language = (SystemLanguage)PlayerPrefs.GetInt(ALocalizationManager.SD_LANGUAGE_KEY);
            }
            if (PlayerPrefs.HasKey(SD_BGMVOLUMN))
            {
                bgVolumn = PlayerPrefs.GetFloat(SD_BGMVOLUMN);
            }
            if (PlayerPrefs.HasKey(SD_SOUNDEFFECTVOLUMN))
            {
                soundEffectVolumn = PlayerPrefs.GetFloat(SD_SOUNDEFFECTVOLUMN);
            }
            if (m_SystemSettingData == null) m_SystemSettingData = new SystemSettingData();
            m_SystemSettingData.Set(bGM, soundEffect, language, GameQuality, bgVolumn, soundEffectVolumn, bVibrateDevice);
            return m_SystemSettingData;
        }
        //------------------------------------------------------
        public void Destroy()
        {
            m_SystemSettingData = null;
        }
        //------------------------------------------------------
        public void Save(SystemSettingData data)
        {
            PlayerPrefs.SetInt(SD_BGM, data.BGM);
            PlayerPrefs.SetInt(ALocalizationManager.SD_LANGUAGE_KEY, (int)data.Language);
            PlayerPrefs.SetInt(SD_QUALITY, data.GameQuality);
            PlayerPrefs.SetInt(SD_SOUNDEFFECT, data.SoundEffect);
            PlayerPrefs.SetFloat(SD_BGMVOLUMN, data.BGVolumn);
            PlayerPrefs.SetFloat(SD_SOUNDEFFECTVOLUMN, data.SoundEffectVolumn);
            PlayerPrefs.SetInt(SD_VIBRATE, data.bVibrateDevice);
        }
    }
}
