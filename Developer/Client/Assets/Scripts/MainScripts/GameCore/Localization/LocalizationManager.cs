/********************************************************************
生成日期:	2020-06-12
类    名: 	LocalizationManager
作    者:	zdq
描    述:	多语言管理器
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using TopGame.Data;
using UnityEngine;

namespace TopGame.Core
{
    public class LocalizationManager : ALocalizationManager, Framework.Module.IStartUp
    {
        public SystemLanguage CurrentLanguage
        {
            get
            {
                return m_eLanguageType;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if(m_vLanguageDic.Count<=0)
            {
                //加载登录前的多语言
                TextAsset text = Resources.Load<TextAsset>("language");
                if (text != null)
                {
                    CsvData_Text csv = new CsvData_Text();
                    if (csv.LoadData(text.text))
                    {
                        foreach (var db in csv.datas)
                        {
                            AddLanguage(db.Value);
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        public void StartUp()
        {
            var setting = SystemSettingManager.getInstance().GetSystemSettingData();
            if (setting != null)
            {
                m_eLanguageType = setting.Language;
            }
            else
            {
                m_eLanguageType = GetLocalSaveLanguage();
            }
            

            var datas = DataManager.getInstance().Text.datas;

            
            SetCurrentLanguage(m_eLanguageType);

            CsvData_Text.TextData data;
            foreach (var dataID in datas.Keys)
            {
                data = DataManager.getInstance().Text.GetData(dataID);
                if (data != null)
                {
                    AddLanguage(data);
                }
            }

            UpdateFont();
        }
        //------------------------------------------------------
        public void AddLanguage(CsvData_Text.TextData text)
        {
            if (text == null) return;
            AddLanguage(text.id, SystemLanguage.Chinese, text.textCN);
            AddLanguage(text.id, SystemLanguage.English, text.textEN);
        }
        //------------------------------------------------------
        protected override void OnLanguageChanged()
        {

        }
        //------------------------------------------------------
        public static string StringFormatToLocalization(uint key,object arg)
        {
            if (arg == null)
            {
                return ToLocalization(key);
            }

            var content = ToLocalization(key);
            if (content == null || content.Contains("{0}") == false)
            {
                return string.Empty;
            }

            return string.Format(content, arg);
        }
    }

}
