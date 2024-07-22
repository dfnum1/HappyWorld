/********************************************************************
生成日期:	2020-06-12
类    名: 	UserActionManager
作    者:	zdq
描    述:	用户行为管理器
*********************************************************************/

namespace TopGame.Core
{
    public class UserActionManager : AUserActionManager
    {

        protected override void Awake()
        {
            base.Awake();
        }
        //------------------------------------------------------
        public override void SendToServer()
        {
         //   return;//暂时不发
            if (m_UserActionCache.Count == 0)
                return;
//            NetLogHandler.LogSvrEvent(m_UserActionCache, 1);
            m_UserActionCache.RemoveAt(0);
        }
        //------------------------------------------------------
        protected override void OnRecordAction(SvrUserAction userAction)
        {
            if (!string.IsNullOrEmpty(userAction.eventName))
            {
                //! log to sdk
                if (TopGame.SvrData.UserManager.MySelf != null && TopGame.SvrData.UserManager.MySelf.userID != 0)
                {
                    if(userAction.propertys!=null)
                    {
                        for (int i = 0; i < userAction.propertys.Count; ++i)
                            SDK.GameSDK.AddEventKV(userAction.propertys[i].key, userAction.propertys[i].value);
                    }
                    SDK.GameSDK.AddEventKV("uid", TopGame.SvrData.UserManager.MySelf.userID.ToString());
                    SDK.GameSDK.LogEvent(userAction.eventName);
                }
            }
        }
    }
}

