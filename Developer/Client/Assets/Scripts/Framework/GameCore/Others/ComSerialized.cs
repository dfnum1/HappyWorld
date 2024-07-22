/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ComSerialized
作    者:	HappLI
描    述:	组件序列化
*********************************************************************/

using Framework.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TopGame.Core
{

    [Framework.Plugin.AT.ATExportMono("序列化/组件序列化器")]
    public class ComSerialized : AComSerialized
    {
        //------------------------------------------------------
        [Framework.Plugin.AT.ATMethod("获取组件")]
        public Object GetWidget(string name)
        {
            return GetWidget<Component>(name);
        }
    }
}
