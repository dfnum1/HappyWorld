/********************************************************************
生成日期:	2:6:2023   10:46
类    名: 	AddEventUtil
作    者:	Ywm
描    述:	AddEventUtil
*********************************************************************/


using UnityEngine;

public static class AddEventUtil
{
    //------------------------------------------------------ 添加公共字段
    public static void LogEvent(string eventName, bool syncServer = false, bool bImmde = false, bool isFirst = false)
    {
        if (TopGame.SvrData.UserManager.MySelf != null && TopGame.SvrData.UserManager.MySelf.userID != 0)
        {
            //公共字段
            TopGame.Core.AUserActionManager.AddActionKV("date", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            TopGame.Core.AUserActionManager.AddActionKV("user_level", TopGame.SvrData.UserManager.MySelf.PlayerLevel);
//            TopGame.Core.AUserActionManager.AddActionKV("chapter", TopGame.SvrData.UserManager.MySelf.GetBattleDB().GetPVEChapterId());
            TopGame.Core.AUserActionManager.AddActionKV("user_name", TopGame.SvrData.UserManager.MySelf.GetUserName());
            TopGame.Core.AUserActionManager.LogActionEvent(eventName,syncServer,bImmde,isFirst);
        }
    }
}