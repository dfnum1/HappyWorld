using System;
using System.Collections.Generic;
using UnityEngine;
namespace SDK
{
    public interface IPlatformAdSDK
    {
        //------------------------------------------------------
        bool Init(ISDKAgent agent, ISDKConfig config);
        void Update(float fTime);
        bool AdCheck(Advertising.AdParam param);
        bool IsChecking();
    }
}