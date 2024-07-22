/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	图形脚本模块配置
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Data;
using UnityEngine;
namespace TopGame.Data
{
    [System.Serializable]
    public class SystemSettingData : AConfig
    {
        public int id;
        /// <summary>
        /// 背景音乐,默认开
        /// </summary>
        public int BGM;
        /// <summary>
        /// 音效,默认开
        /// </summary>
        public int SoundEffect;
        /// <summary>
        /// 语言,默认汉语
        /// </summary>
        public SystemLanguage Language;
        /// <summary>
        /// 画质,默认最高品质 0,1,2 低,中,高
        /// </summary>
        public int GameQuality;

        public float BGVolumn;
        public float SoundEffectVolumn;

        public int bVibrateDevice;
        //------------------------------------------------------
        public void Set(int bGM, int soundEffect, SystemLanguage language, int GameQuality = 2, float bgVolunm = 1, float soundEffectVolumn = 1, int bVibrateDevice = 1)
        {
            this.id = 1;
            BGM = bGM;
            SoundEffect = soundEffect;
            Language = language;
            this.GameQuality = GameQuality;
            BGVolumn = bgVolunm;
            SoundEffectVolumn = soundEffectVolumn;
            this.bVibrateDevice = bVibrateDevice;
        }
        //------------------------------------------------------
        public void Apply()
        {
        }
        //------------------------------------------------------
        public void Init(Framework.Module.AFrameworkBase pFramework)
        {
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public void OnInspector(System.Object param = null)
        {
        }
        //------------------------------------------------------
        public void Save() { }
#endif
    }
}
