/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	StartUpUpdateUI
作    者:	HappLI
描    述:	启动更新版本检测界面
*********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.UI;
using TopGame.Base;
using UnityEngine.Video;
using Framework.Core;

namespace TopGame.UI
{
    public class StartUpUpdateUI : MonoBehaviour
    {
        long m_lTotalSize = 0;
        long m_lCurSize = 0;

        public FitRawImage bgRawImage;
        public UnityEngine.UI.Text pLoadText = null;
        public UnityEngine.UI.Text pLoadSpeedText = null;
        public UnityEngine.UI.Image pScrollBar = null;
        public UISerialized uISerialized = null;
        public GameObject CommonTipsUI = null;
        public RectTransform DogTectTransform = null;
        public Text ScrollingText = null;
        public List<uint> ScrolIDList;
        public float ScrolTextDelay = 5;
        /// <summary>
        /// 下载资源提示窗弹窗大小临界值,单位kb
        /// </summary>
        public long DownloadTipsSize = 20000;

        private static StartUpUpdateUI ms_Instance;
        private Text m_ContentText;
        private Button m_ConfirmBtn;
        private Button m_CancelBtn;

        Action m_ConfirmAction = null;
        Action m_CancelAction = null;

        long m_LastSize = 0;
        float m_Timer = 0;

        bool m_bShow = false;
        Coroutine m_ScrollingCot = null;
        List<uint> m_UsedScrolIDList = new List<uint>();

        public static StartUpUpdateUI getInstance()
        {
            return ms_Instance;
        }
        //------------------------------------------------------
        public static long GetDownloadTipsSize()
        {
            if (ms_Instance == null) return 20000;
            return ms_Instance.DownloadTipsSize;
        }
        //------------------------------------------------------
        private void Awake()
        {
            m_bShow = false;
            ms_Instance = this;
            if (pScrollBar != null) pScrollBar.transform.parent.gameObject.SetActive(false);
            if (DogTectTransform)
            {
                DogTectTransform.gameObject.SetActive(false);
            }
            if (pLoadSpeedText)
            {
                pLoadSpeedText.gameObject.SetActive(false);
            }
            name = "VersionCheckUpdate";
         //   DontDestroyOnLoad(this.gameObject);
            this.gameObject.SetActive(false);

            m_ContentText = uISerialized.GetWidget<Text>("ContentText");
            m_ConfirmBtn = uISerialized.GetWidget<Button>("ConfirmBtn");
            if (m_ConfirmBtn)
            {
                EventTriggerListener.Get(m_ConfirmBtn.gameObject).onClick = OnConfirmClick;
            }
            m_CancelBtn = uISerialized.GetWidget<Button>("CancelBtn");
            if (m_CancelBtn)
            {
                EventTriggerListener.Get(m_CancelBtn.gameObject).onClick = OnCancelClick;
            }

            Base.GlobalUtil.SetActive(CommonTipsUI, false);

            if (ScrollingText)
            {
                ScrollingText.gameObject.SetActive(false);
            }
            if (bgRawImage) bgRawImage.SetDirtyFit();
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_Instance = null;
            m_LastSize = 0;
            m_Timer = 0;
            m_bShow = false;
            OnUpdate = null;
        }
        //------------------------------------------------------
        public static Action<float> OnUpdate;
        //------------------------------------------------------
        private void Update()
        {
            if (OnUpdate != null) OnUpdate(Time.deltaTime);
        }
        //------------------------------------------------------
        public static bool IsShow()
        {
            if (ms_Instance == null) return false;
            return ms_Instance.m_bShow;
        }
        //------------------------------------------------------
        public static void Show()
        {
            if (ms_Instance == null) return;
            if(ms_Instance.m_bShow)
            {
                return;
            }
            ms_Instance.m_bShow = true;
            ms_Instance.gameObject.SetActive(true);

            if (ms_Instance.pScrollBar != null) ms_Instance.pScrollBar.transform.parent.gameObject.SetActive(true);
            if (ms_Instance.DogTectTransform)
            {
                ms_Instance.DogTectTransform.gameObject.SetActive(true);
            }
            if(ms_Instance.pLoadSpeedText)
            {
                ms_Instance.pLoadSpeedText.gameObject.SetActive(true);
            }

            if (ms_Instance.ScrollingText)
            {
                ms_Instance.ScrollingText.gameObject.SetActive(true);
            }
            if (ms_Instance.bgRawImage) ms_Instance.bgRawImage.enabled = false;

            ms_Instance.StartScrollingText();

            SetCurLoadSize(0);
            SetTotalSize(0);

            SetProgressTip("");
        }
        //------------------------------------------------------
        public static void Hide()
        {
            if (ms_Instance == null) return;
            if (!ms_Instance.m_bShow)
                return;
            ms_Instance.m_bShow = false;
            ms_Instance.gameObject.SetActive(false);
            if (ms_Instance.bgRawImage) ms_Instance.bgRawImage.enabled = false;
            SetCurLoadSize(0);
            SetTotalSize(0);

            SetProgressTip("");
        }
        //------------------------------------------------------
        public static void ShowProgressBar(bool bShow)
        {
            if (ms_Instance == null) return;
            if (ms_Instance.pScrollBar != null) ms_Instance.pScrollBar.transform.parent.gameObject.SetActive(bShow);
            if (ms_Instance.DogTectTransform)
            {
                ms_Instance.DogTectTransform.gameObject.SetActive(bShow);
            }
        }
        //------------------------------------------------------
        public static void SetTotalSize(long Size, bool bLoadText = true)
        {
            if (ms_Instance == null) return;
            if (ms_Instance.m_lTotalSize != Size)
            {
                ms_Instance.m_lTotalSize = Size;
                ms_Instance.UpdateUI(bLoadText);
            }
        }
        //------------------------------------------------------
        public static void SetCurLoadSize(long Size, bool bLoadText = true)
        {
            if (ms_Instance == null) return;
            if (ms_Instance.m_lCurSize != Size)
            {
                ms_Instance.m_lCurSize = Size;
                ms_Instance.UpdateUI(bLoadText);
            }
            ms_Instance.m_Timer += Time.deltaTime;
            if (ms_Instance.m_Timer >= 1f)
            {
                if(bLoadText)
                    ms_Instance.SetDownloadSpeed();
                ms_Instance.m_Timer--;
            }
        }
        //------------------------------------------------------
        public static void SetProgressTip(string tips, TextAnchor alignment = TextAnchor.MiddleCenter)
        {
            if (ms_Instance == null || ms_Instance.uISerialized == null) return;
            if (tips == null) tips = "";
            UnityEngine.UI.Text text = ms_Instance.uISerialized.GetWidget<UnityEngine.UI.Text>("progressTip");
            if (text)
            {
                text.alignment = alignment;
                text.text = tips;
            }
        }
        //------------------------------------------------------
        public static void SetProgressTip(int id, TextAnchor alignment = TextAnchor.MiddleCenter)
        {
            if (ms_Instance == null || ms_Instance.uISerialized == null) return;
            SetProgressTip(GlobalUtil.ToLocalization(id), alignment);
        }
        //------------------------------------------------------
        void UpdateUI(bool bLoadText = true)
        {
            if (pLoadText)
            {
                if (bLoadText) pLoadText.text = Base.GlobalUtil.stringBuilder.Append(Base.GlobalUtil.FormBytes(m_lCurSize)).Append("/").Append(Base.GlobalUtil.FormBytes(m_lTotalSize)).ToString();
                else pLoadText.text = "";
            }
            if (pScrollBar && m_lTotalSize > 0)
            {
                pScrollBar.fillAmount = Mathf.Clamp01(((float)m_lCurSize / (float)m_lTotalSize));

                if (DogTectTransform)
                {
                    DogTectTransform.anchoredPosition = new Vector2(pScrollBar.fillAmount * 590 - 280, DogTectTransform.anchoredPosition.y);
                }
            }
            
        }
        //------------------------------------------------------
        public static void ShowTips(bool isActive,uint contentID,Action confirmAction = null,Action cancelAction = null)
        {
            if (ms_Instance == null) return;
            Base.GlobalUtil.SetActive(ms_Instance.CommonTipsUI, isActive);
            if(ms_Instance.m_ContentText) ms_Instance.m_ContentText.text = GlobalUtil.ToLocalization((int)contentID);
            ms_Instance.m_ConfirmAction = confirmAction;
            ms_Instance.m_CancelAction = cancelAction;
        }
        //------------------------------------------------------
        public static void ShowTips(bool isActive, string content, Action confirmAction = null, Action cancelAction = null)
        {
            if (ms_Instance == null) return;
            Base.GlobalUtil.SetActive(ms_Instance.CommonTipsUI, isActive);
            if (ms_Instance.m_ContentText) ms_Instance.m_ContentText.text = content;
            ms_Instance.m_ConfirmAction = confirmAction;
            ms_Instance.m_CancelAction = cancelAction;
        }
        //------------------------------------------------------
        public static void BeginDownloadBaseFile()
        {

        }
        //------------------------------------------------------
        public static void EndDownloadBaseFile()
        {

        }
        //------------------------------------------------------
        void OnConfirmClick(GameObject go, VariablePoolAble[] param)
        {
            m_ConfirmAction?.Invoke();
            Base.GlobalUtil.SetActive(CommonTipsUI, false);
        }
        //------------------------------------------------------
        void OnCancelClick(GameObject go, VariablePoolAble[] param)
        {
            m_CancelAction?.Invoke();
            Base.GlobalUtil.SetActive(CommonTipsUI, false);
        }
        //------------------------------------------------------
        void SetDownloadSpeed()
        {
            if (pLoadSpeedText == null)
            {
                return;
            }

            long delta = m_lCurSize - m_LastSize;
            pLoadSpeedText.text = Base.GlobalUtil.FormBytes(delta) + "/S";
            m_LastSize = m_lCurSize;
        }
        //------------------------------------------------------
        void StartScrollingText()
        {
            if (ScrollingText == null)
            {
                return;
            }
            StopScrollingText();
            m_ScrollingCot = StartCoroutine(UpdateScrollingText());
        }
        //------------------------------------------------------
        IEnumerator UpdateScrollingText()
        {
            while (true)
            {
                ScrollingText.text = GetRandomUnRepeatText();
                yield return new WaitForSeconds(ScrolTextDelay);
            }
        }
        //------------------------------------------------------
        string GetRandomUnRepeatText()
        {
            if (ScrolIDList.Count == 0)
            {
                uint curID = m_UsedScrolIDList[m_UsedScrolIDList.Count - 1];
                foreach (var item in m_UsedScrolIDList)
                {
                    ScrolIDList.Add(item);
                }
                m_UsedScrolIDList.Clear();
                m_UsedScrolIDList.Add(curID);
                ScrolIDList.Remove(curID);
            }
            int index = UnityEngine.Random.Range(0, ScrolIDList.Count);
            string text = Base.GlobalUtil.ToLocalization((int)ScrolIDList[index]);
            m_UsedScrolIDList.Add(ScrolIDList[index]);
            ScrolIDList.RemoveAt(index);
            return text;
        }
        //------------------------------------------------------
        public static void StopScrollingText()
        {
            if (ms_Instance == null) return;
            if (ms_Instance.m_ScrollingCot != null)
            {
                ms_Instance.StopCoroutine(ms_Instance.m_ScrollingCot);
                ms_Instance.m_ScrollingCot = null;
                if (ms_Instance.ScrollingText) ms_Instance.ScrollingText.text = "";
            }
        }
        //------------------------------------------------------
        public static void ShowBG(bool bShowBG)
        {
            if (ms_Instance == null) return;
            if (ms_Instance.bgRawImage)
            {
                ms_Instance.bgRawImage.enabled = bShowBG;
                ms_Instance.bgRawImage.SetDirtyFit();
            }
        }
    }
}
