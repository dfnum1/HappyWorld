/********************************************************************
生成日期:	6:8:2020 10:42
类    名: 	UIPreTip
作    者:	Happli
描    述:	预提示
*********************************************************************/

using Framework.Core;
using System;
using TopGame.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    public class WebViewPanel : UserInterface
    {
        static WebViewPanel ms_pInstance = null;
        bool m_bShow = false;
        System.Action<string> OnAction = null;
        Text m_YesText = null;
        Text m_NoText = null;
        Image m_Yes;
        Image m_No;
        UniWebView m_WebView;
        //------------------------------------------------------
        public static void InitInstance(WebViewPanel instnace)
        {
            if (instnace == null) return;
            ms_pInstance = instnace;
            instnace.m_WebView = instnace.GetWidget<UniWebView>("Web");
            if(instnace.m_WebView) instnace.m_WebView.Init();
            instnace.m_Yes = instnace.GetWidget<Image>("Yes");
            if (instnace.m_Yes)
            {
                var listener = UI.EventTriggerListener.Get(instnace.m_Yes.gameObject);
                listener.onClick += instnace.OnYesClick;
            }
            instnace.m_No = instnace.GetWidget<Image>("No");
            if (instnace.m_No)
            {
                var listener = UI.EventTriggerListener.Get(instnace.m_No.gameObject);
                listener.onClick += instnace.OnNoClick;
            }
            instnace.m_bShow = false;
            instnace.OnAction = null;
            Hide();
            instnace.gameObject.SetActive(false);
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_pInstance = null;
        }
        //------------------------------------------------------
        void OnYesClick(GameObject go, params VariablePoolAble[] param)
        {
            if (OnAction != null)
            {
                OnAction("yes");
            }
            Hide();
        }
        //------------------------------------------------------
        void OnNoClick(GameObject go, params VariablePoolAble[] param)
        {
            if (OnAction != null)
            {
                OnAction("no");
            }
            Hide();
        }
        //------------------------------------------------------
        public static void Show(string url, Action<string> OnCallback, int yesId, int noId)
        {
            if (ms_pInstance == null) return;
            if (ms_pInstance.m_bShow)
                return;
            ms_pInstance.OnAction = OnCallback;
            ms_pInstance.m_bShow = true;
            ms_pInstance.gameObject.SetActive(true);
            if (ms_pInstance.m_YesText)
                ms_pInstance.m_YesText.text = Base.GlobalUtil.ToLocalization(yesId, "Yes");
            if ( ms_pInstance.m_NoText)
                ms_pInstance.m_NoText.text = Base.GlobalUtil.ToLocalization(noId, "No");
            if(ms_pInstance.m_Yes) Base.GlobalUtil.SetActive(ms_pInstance.m_Yes.gameObject, yesId != 0);
            if(ms_pInstance.m_No) Base.GlobalUtil.SetActive(ms_pInstance.m_No.gameObject, noId != 0);

            if (ms_pInstance.m_WebView)
            {
                ms_pInstance.m_WebView.Load(url);
                ms_pInstance.m_WebView.SetHorizontalScrollBarEnabled(true);
                ms_pInstance.m_WebView.SetVerticalScrollBarEnabled(true);
                ms_pInstance.m_WebView.SetShowSpinnerWhileLoading(true);
                ms_pInstance.m_WebView.SetAllowFileAccessFromFileURLs(true);
                ms_pInstance.m_WebView.SetBackButtonEnabled(yesId == 0 && noId == 0);

                ms_pInstance.m_WebView.OnMessageReceived += ms_pInstance.OnMessageReceived;
                ms_pInstance.m_WebView.OnPageStarted += ms_pInstance.OnPageStarted;
                ms_pInstance.m_WebView.OnPageFinished += ms_pInstance.OnPageFinished;
                ms_pInstance.m_WebView.OnKeyCodeReceived += ms_pInstance.OnKeyCodeReceived;
                ms_pInstance.m_WebView.OnPageErrorReceived += ms_pInstance.OnPageErrorReceived;

                ms_pInstance.m_WebView.OnShouldClose += ms_pInstance.OnShouldClose;

                ms_pInstance.m_WebView.BackgroundColor = new Color(0, 0, 0, 0.5f);
                ms_pInstance.m_WebView.UpdateFrame();
                ms_pInstance.m_WebView.Show();
            }
        }
        //------------------------------------------------------
        public static void Hide()
        {
            if (ms_pInstance == null) return;
            if (!ms_pInstance.m_bShow)
                return;
            ms_pInstance.m_bShow = false;
            ms_pInstance.gameObject.SetActive(false);
            ms_pInstance.OnAction = null;
        }
        //------------------------------------------------------
        private void OnPageErrorReceived(UniWebView webView, int errorCode, string errorMessage)
        {
            //MyAppUtil.MakeToastOffic_(string.Format("errorCode:{0},errorMessage{1}", errorCode, errorMessage));//提示。比如网络连接失败
            Debug.Log("UniWebView----OnPageErrorReceived ：" + string.Format("errorCode:{0},errorMessage{1}", errorCode, errorMessage));
        }
        //------------------------------------------------------
        private void OnKeyCodeReceived(UniWebView webView, int keyCode)
        {
            Debug.Log("UniWebView----OnKeyCodeReceived keycode:" + keyCode);
        }
        //------------------------------------------------------
        private void OnPageFinished(UniWebView webView, int statusCode, string url)
        {
            //暂停音乐
            Debug.Log("UniWebView----OnPageFinished statusCode:" + string.Format("statusCode:{0},url{1}", statusCode, url));
        }
        //------------------------------------------------------
        private void OnPageStarted(UniWebView webView, string url)
        {
            Debug.Log("UniWebView----OnPageStarted " + url);
        }
        //------------------------------------------------------
        private void OnMessageReceived(UniWebView webView, UniWebViewMessage message)
        {
            Debug.Log("UniWebView----OnMessageReceived :" + message.RawMessage);
            Debug.Log("UniWebView----OnMessageReceived :" + message.Path);
//             if (message.Path.Equals("action"))
//             {
//                 var key = message.Args["key"];
//                 var anotherKey = message.Args["anotherKey"];
//             }

        }
        //------------------------------------------------------
        private bool OnShouldClose(UniWebView webView)
        {
            webView.Hide();
            //webView.CleanCache();//清除缓存
            webView = null;
            return true;
        }
    }
}
