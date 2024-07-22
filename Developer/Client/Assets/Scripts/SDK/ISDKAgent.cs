/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ISDKAgent
作    者:	HappLI
描    述:	SDK 代理
*********************************************************************/
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct ShareParam
    {
        public string title;   //分享标题
        public string content;   //分享内容
        public string imgPath;   //分享图片本地地址
        public string imgUrl;   //分享图片网络地址
        public string url;   //分享链接
        public string type;   //分享类型
        public string shareTo;   //分享到哪里
        public string extenal;   //额外备注
    }
    public enum EAdType
    {
        Banner,
        Reward,
        FullScreen,
    }
    [System.Serializable]
    public struct AdCallbackParam : ISDKParam
    {
        public enum EAction
        {
            Loaded,
            Played,
            PlayEnd,
            LoadFailed,
            PlayFailed,
            Clicked,
            Closed,
            Reward,
        }
        public EAction action;
        public EAdType type;
        public string error;
        public string posId;
        public string rewards;
    }


    public struct SDKStatisticRank
    {
        public long rank;
        public long userId;
        public string uid;
        public string name;
        public double value;
        public SDKStatisticRank(string uid, string name, double value, long rank, long userId = 0)
        {
            this.uid = uid;
            this.name = name;
            this.value = value;
            this.userId = userId;
            this.rank = rank;
            if (this.userId == 0)
                this.userId = Framework.Core.BaseUtil.StringToHashID64(uid);
        }
    }
    //------------------------------------------------------
    public enum ESDKActionType
    {
        InitSucces,
        InitFail,
        LoginSucces,
        LoginFail,
        LogoutSucces,
        LogoutFail,
        SwitchAccount,
        PayBegin,
        PaySucces,
        PayFail,
        PayCancel,
        RePay,
        Message,
        StatisticsRank,
        StatisticsRankUpdate,
        Exit,
        FCMOut,
    }
    //------------------------------------------------------
    public struct SDKCallbackParam : ISDKParam
    {
        public ESDKActionType action;
        public string uid;
        public string cpOrderId;
        public string orderId;
        public string name;
        public string msg;
        public string channel;
        public System.Collections.Generic.List<SDKStatisticRank> statisticRanks;
        public SDKCallbackParam(ESDKActionType action)
        {
            this.action = action;
            this.uid = null;
            this.name = null;
            this.msg = null;
            this.statisticRanks = null;
            this.channel = null;
            this.cpOrderId = null;
            this.orderId = null;
        }

        public override string ToString()
        {
            return string.Format("action:{0}, uid:{1}, name:{2},channel{3}, msg:{4}", action, uid, name, channel, msg);
        }
    }
    //------------------------------------------------------
    public interface ISDKParam
    {

    }
    //------------------------------------------------------
    public interface ISDKConfig
    {
    }
    //------------------------------------------------------
    public interface ISDKCallback
    {
        void OnStartUp(ISDKAgent agent);
        void OnSDKAction(ISDKAgent agent, ISDKParam param);
    }
    //------------------------------------------------------
    public abstract class ISDKAgent
    {
        protected static ISDKAgent ms_Agent = null;
        protected ISDKCallback m_pCallback = null;
        public ISDKAgent()
        {
            ms_Agent = this;
        }
        //------------------------------------------------------
        ~ISDKAgent()
        {
            ms_Agent = null;
            m_pCallback = null;
        }
        //------------------------------------------------------
        public void SetCallback(ISDKCallback callback)
        {
            m_pCallback = callback;
        }
        //------------------------------------------------------
        public ISDKCallback GetCallback()
        {
            return m_pCallback;
        }
        //------------------------------------------------------
        public virtual void Update(float fTime) { }
        //------------------------------------------------------
        protected abstract bool Init(ISDKConfig config);

        //------------------------------------------------------
        public void OnStartUp()
        {
            if(m_pCallback!=null) m_pCallback.OnStartUp(this);
        }
        //------------------------------------------------------
        public void OnSDKAction(ISDKParam param)
        {
            if (m_pCallback != null) m_pCallback.OnSDKAction(this,param);
            else
            {
                GameSDK.DoCallback(this, param);
            }
        }
    }
}
