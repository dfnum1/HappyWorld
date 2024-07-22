/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	NetHandlerAttribute
作    者:	HappLI
描    述:	消息句柄
*********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TopGame.Net
{
    public class NetHandlerAttribute : Attribute
    {
    }
    public class NetResponseAttribute : Attribute
    {
        public Proto3.MID mid;
        public NetResponseAttribute( Proto3.MID mid)
        {
            this.mid = mid;
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class NetLockAttribute : Attribute
    {
        public Proto3.MID lockID;
        public Proto3.MID unlockID;
        public NetLockAttribute(Proto3.MID lockID, Proto3.MID unlockID)
        {
            this.lockID = lockID;
            this.unlockID = unlockID;
        }
    }
}
