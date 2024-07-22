/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UILoginView
作    者:	HappLI
描    述:	UI管理器
*********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TopGame.SvrData;
using TopGame.Base;
using TopGame.Data;
using TopGame.Core;
using Framework.Core;

namespace TopGame.UI
{
    [Framework.Plugin.AT.ATExportMono("UI系统/登录/视图")]
    [UI((ushort)EUIType.Login, UI.EUIAttr.View)]
    public class UILoginView : UIView
    {
        UILogin m_pUI = null;

        UnityEngine.UI.InputField m_pUsr = null;
        UnityEngine.UI.InputField m_pPwd = null;
        UnityEngine.UI.Button m_Login = null;
        private Text m_userName;
        UnityEngine.UI.Toggle m_OfflineToggle = null;
        UnityEngine.UI.Toggle m_ShowPwdToggle = null;
        UnityEngine.UI.Dropdown m_DropdownOption = null;
        UnityEngine.UI.InputField m_PortInput = null;
        List<SvrData.ServerList.Item> m_ServerItems;
        private Text m_Tips_Text;
        private GameObject m_NotCachedPanel;
        private GameObject m_CachedPanel;
        private EventTriggerListener m_Logout_Btn;
        private InputField m_IP_InputField;
        private InputField m_Port_InputField;
        private Image m_Another_Login;
        private Image m_AgeTipImg;
        public bool bHasCacheAccount = false;

        private Text m_Company_Text;

        //------------------------------------------------------
        public override void Init(UIBase pBase)
        {
            base.Init(pBase);
            m_pUI = pBase as UILogin;
        }
        //------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
            if (m_pUI == null) return;
            if (m_pUI.ui)
            {
                m_pUsr = m_pUI.ui.GetWidget<UnityEngine.UI.InputField>("usr");
                if (m_pUsr)
                    m_pUsr.onValueChanged.AddListener(OnUserAccountChanged);
                m_pPwd = m_pUI.ui.GetWidget<UnityEngine.UI.InputField>("pwd");
                if (m_pPwd)
                    m_pPwd.onValueChanged.AddListener(OnUserPasswordChanged);
                m_Login = m_pUI.ui.GetWidget<UnityEngine.UI.Button>("login");
                m_userName = m_pUI.ui.GetWidget<UnityEngine.UI.Text>("userName");
                if (m_Login != null)
                    EventTriggerListener.Get(m_Login.gameObject).onClick = m_pUI.OnLoginClick;

                m_OfflineToggle = m_pUI.ui.GetWidget<UnityEngine.UI.Toggle>("offline");
                m_DropdownOption = m_pUI.ui.GetWidget<UnityEngine.UI.Dropdown>("Dropdown");
                m_PortInput = m_pUI.ui.GetWidget<UnityEngine.UI.InputField>("InputField");
                m_ShowPwdToggle = m_pUI.ui.GetWidget<UnityEngine.UI.Toggle>("showpassword");
                if (m_ShowPwdToggle)
                {
                    m_pPwd.contentType = InputField.ContentType.Password;
                    m_ShowPwdToggle.isOn = false;
                    m_ShowPwdToggle.onValueChanged.AddListener((isOn) =>
                    {
                        m_pPwd.contentType = isOn ? InputField.ContentType.Standard : InputField.ContentType.Password;
                        m_pPwd.ForceLabelUpdate();
                    });
                }


                m_Tips_Text = m_pUI.ui.GetWidget<Text>("Tips_Text");
                m_NotCachedPanel = m_pUI.ui.Find("NotCachedPanel");
                m_CachedPanel = m_pUI.ui.Find("CachedPanel");

                m_Logout_Btn = m_pUI.ui.GetWidget<TopGame.UI.EventTriggerListener>("Logout_Btn");
                if (m_Logout_Btn)
                {
                    m_Logout_Btn.onClick = OnLogOutClick;
                }
                UIUtil.SetActive(m_Logout_Btn, !SDK.GameSDK.HasLoginSDK());
                var logoutBtn = m_pUI.ui.GetWidget<TopGame.UI.EventTriggerListener>("LogoutBtn");
                if (logoutBtn)
                {
                    logoutBtn.onClick = OnLogOutClick;
                }

                m_IP_InputField = m_pUI.ui.GetWidget<UnityEngine.UI.InputField>("IP_InputField");
                m_Port_InputField = m_pUI.ui.GetWidget<UnityEngine.UI.InputField>("Port_InputField");
                m_Another_Login = m_pUI.ui.GetWidget<Image>("Another_Login");
                if (m_Another_Login != null)
                    EventTriggerListener.Get(m_Another_Login.gameObject).onClick = OnAnotherLoginClick;
                m_AgeTipImg = m_pUI.ui.GetWidget<Image>("AgeTip_Img");
                m_Company_Text = m_pUI.ui.GetWidget<Text>("Company_Text");

                var verText = m_pUI.ui.GetWidget<UnityEngine.UI.Text>("Version_Text");
                UIUtil.SetLabel(verText, Core.LocalizationManager.StringFormatToLocalization(80010036, "1.0.0"));

                SetAgeTipShow();
                SetCompanyText();
                SetListData();
            }
        }
        //------------------------------------------------------
        private void OnAnotherLoginClick(GameObject go, VariablePoolAble[] param)
        {
            if (m_IP_InputField == null || m_Port_InputField == null)
            {
                return;
            }
//             Net.NetWork.getInstance().Connect(m_IP_InputField.text, int.Parse(m_Port_InputField.text));
//             if (m_pUI != null)
//             {
//                 m_pUI.Req_Login(false);
//             }
        }
        //------------------------------------------------------
        private void OnUserPasswordChanged(string value)
        {

//             int maxLength = (int)m_Syscfg.passwordCharacterNum[1];
// 
//             int length = Util.GetStrLength(value);
// 
//             if (length > maxLength)
//             {
//                 Util.ShowCommonTip(ETipType.AutoHide, 10014006);
//                 m_pPwd.text = Util.SubString(value, maxLength);
//             }
        }
        //------------------------------------------------------
        private void OnUserAccountChanged(string value)
        {
//             if (m_Syscfg == null)
//             {
//                 m_Syscfg = CsvData_SystemConfig.sysConfig;
//             }
//             if (m_Syscfg == null || m_Syscfg.accountCharacterNum.Length < 2)
//             {
//                 return;
//             }
//             int maxLength = (int)m_Syscfg.accountCharacterNum[1];
// 
//             int length = Util.GetStrLength(value);
// 
//             if (length > maxLength)
//             {
//                 Util.ShowCommonTip(ETipType.AutoHide, 10014006);
//                 m_pUsr.text = Data.SensitiveWord.FilterWord(Util.SubString(value, maxLength));
//             }
//             else
//                 m_pUsr.text = Data.SensitiveWord.FilterWord(value);
        }
        //------------------------------------------------------
        public string GetUsrInput()
        {
            return m_pUsr.text;
        }
        //------------------------------------------------------
        public string GetPwdInput()
        {
            return m_pPwd.text;
        }
        //------------------------------------------------------
        public string GetLoginServerName()
        {
            if (m_ServerItems.Count == 0)
            {
                return "";
            }
            int index = m_DropdownOption.value;
            if (index < 0 || index > m_ServerItems.Count) return "";
            return m_ServerItems[m_DropdownOption.value].name;
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("是否有缓存账号")]
        public bool HasAccountCache()
        {
            return Data.LocalAccountCatch.HasAccountCache();
        }
        //------------------------------------------------------
        public void SetPanel(bool hasCached, bool isAllHide = false)
        {
            if (hasCached)
            {
                UIUtil.SetLabel(m_Tips_Text, 80010220);
            }
            if (isAllHide)
            {
                Util.SetActive(m_CachedPanel, false);
                Util.SetActive(m_NotCachedPanel, false);
            }
            else
            {
                Util.SetActive(m_CachedPanel, hasCached);
                Util.SetActive(m_NotCachedPanel, !hasCached&&!SDK.GameSDK.HasLoginSDK());
            }
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("点击进入游戏或者登录")]
        public void ReqLogin()
        {
            m_pUI.Login();
            return;
            bHasCacheAccount = HasAccountCache();
            if (bHasCacheAccount)
            {
           //     m_pUI.Login(Proto3.LoginType.Account);
            }
            else
            {
                // quick login TODO...
                //如果没有缓存将使用快速登录,设备唯一码，做请求
                //GameInstance.getInstance().GetDeviceUDID();

                if (DebugConfig.AutoAccountRegister)
                {
                    SetPanel(false, true);
                }
                else
                {
                    if (m_NotCachedPanel.activeInHierarchy)
                    {
                        SetPanel(false, true);
                    }
                    else
                    {
                        SetPanel(false);
                        m_DropdownOption.RefreshShownValue();
                    }
                }
            }
        }
        //------------------------------------------------------
        public void NotCachedAccount(bool bPanel = true)
        {
            if (bPanel) SetPanel(false, false);
            //Util.SetLabel(m_Tips_Text, 80010209);
            bHasCacheAccount = false;
            if (m_Logout_Btn)
            {
                Util.SetActive(m_Logout_Btn.gameObject, false);
            }
        }
        //------------------------------------------------------
        public void HaveCacheAccount()
        {
            SetPanel(true);
        }
        //------------------------------------------------------
        public void SetCompanyText()
        {
            if (m_Company_Text == null)
            {
                return;
            }
#if USE_ANTIADDICTION
            m_Company_Text.text = Core.LocalizationManager.ToLocalization(10017002);
#else
            m_Company_Text.text = Core.LocalizationManager.ToLocalization(80010208);
#endif
        }
        //------------------------------------------------------
        public void SetAgeTipShow()
        {
            if (m_AgeTipImg == null)
            {
                return;
            }
#if USE_ANTIADDICTION
            bool bShow = true;
            string channel = SDK.GameSDK.GetPayChannel(false);
            if(!string.IsNullOrEmpty(channel) && channel.Contains("zssdk"))
            {
                bShow = false;
            }
            Base.Util.SetActive(m_AgeTipImg.gameObject, bShow);
#else
            Base.Util.SetActive(m_AgeTipImg.gameObject, false);
#endif
        }
        //------------------------------------------------------
        private void OnLogOutClick(GameObject go, VariablePoolAble[] param)
        {
            if (SDK.GameSDK.HasLoginSDK())
            {
                return;
            }
            m_pUI?.LogOut();
        }
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("是否离线")]
        public bool IsOffline()
        {
            return m_OfflineToggle.isOn;
        }
        //------------------------------------------------------
        public string GetPath()
        {
           // return "http://192.168.6.99:35530/";
            if (m_ServerItems == null || m_ServerItems.Count <= 0) return null;
            if (m_DropdownOption.value < 0 || m_DropdownOption.value >= m_ServerItems.Count) return null;
            string ip = m_ServerItems[m_DropdownOption.value].ip;
            if (string.IsNullOrEmpty(ip)) return null;
            if (ip.StartsWith("http")) return ip;
            return Base.Util.stringBuilder.Append(ip).Append(":")
                .Append(m_PortInput.text).ToString();
        }
        //------------------------------------------------------
        public void SetListData()
        {
            string user = Data.LocalAccountCatch.GetUserName();
            string password = Data.LocalAccountCatch.GetPassword();
            string serverName = Data.LocalAccountCatch.GetLinkServer();
            if (!string.IsNullOrEmpty(user))
                m_pUsr.SetTextWithoutNotify(user);
            m_pPwd.SetTextWithoutNotify(password);

            if (string.IsNullOrEmpty(user)) user = "";
            if (user.CompareTo("null") == 0) user = "";
            if (string.IsNullOrEmpty(user)) user = Data.LocalAccountCatch.GetAccount();
            UIUtil.SetLabel(m_userName, user);


            m_DropdownOption.ClearOptions();
            m_DropdownOption.options = new List<Dropdown.OptionData>();
            m_ServerItems = SvrData.ServerList.GetServers(EServerType.Login);
            if(m_ServerItems!=null)
            {
                int selectIndex = 0;
                for (int i = 0; i < m_ServerItems.Count; i++)
                {
                    m_DropdownOption.options.Add(new Dropdown.OptionData(m_ServerItems[i].name));
                    if (m_ServerItems[i].name.CompareTo(serverName) == 0)
                        selectIndex = i;
                }
                m_DropdownOption.onValueChanged.AddListener((idx) =>
                {
                    Data.LocalAccountCatch.SetLinkServer(m_ServerItems[idx].name);
                    m_PortInput.text = m_ServerItems[idx].port.ToString();
                });
                m_DropdownOption.value = selectIndex;
                if (selectIndex < m_ServerItems.Count) m_PortInput.text = m_ServerItems[selectIndex].port.ToString();
                m_DropdownOption.RefreshShownValue();
            }
        }
        //------------------------------------------------------
        void OnRefreshServerList()
        {
            ServerList.RefreshServerList((string result, VariablePoolAble userParam) =>
            {
                if (result.CompareTo("Fail") != 0)
                {
                    SetListData();
                }
            });
        }
    }
}
