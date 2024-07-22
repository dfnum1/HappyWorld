/********************************************************************
生成日期:   28:1:2023   19:0
类    名:   UILoginSDKLogic
作    者:  HappLI
描    述:  UILoginSDKLogic
*********************************************************************/

using Framework.Core;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    [UI((ushort)EUIType.Login, UI.EUIAttr.Logic,true)]
    public class UILoginSDKLogic : UILogic
    {
        static List<string> ms_vSDKAssets = new List<string>();
        public static void AddSDKAssets(string asset)
        {
            if (string.IsNullOrEmpty(asset)) return;
            ms_vSDKAssets.Add(asset);
            if (!ms_vSDKAssets.Contains(asset)) ms_vSDKAssets.Add(asset);
        }
        //------------------------------------------------------
        public override void OnShow()
        {
            base.OnShow();

            UIUtil.SetActive(GetWidget<RectTransform>("CommonLogin"), ms_vSDKAssets==null || ms_vSDKAssets.Count<=0 );
            if (ms_vSDKAssets != null)
            {
                RectTransform logins = null;
                UISerialized sdks = GetWidget<UISerialized>("SDKs");
                if (sdks != null)
                {
                    logins = sdks.GetWidget<RectTransform>("Logins");
                }
                if (logins != null)
                {
                    for (int i = 0; i < ms_vSDKAssets.Count; ++i)
                    {
                        LoadInstance(ms_vSDKAssets[i], logins, true);
                    }
                }
            }
        }
    }
}