/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GuideStepHandler
作    者:	HappLI
描    述:	
*********************************************************************/

using System;
using System.Collections.Generic;

namespace Framework.Plugin.Guide
{
    public class GuideTriggerHandler
    {
		public static bool OnGuideSign(TriggerNode pNode, CallbackParam param)
		{
			return true;
		}
        //-------------------------------------------
        public static void OnGuideNode(TriggerNode pNode)
        {
            
        }
        //-------------------------------------------
        public static void OnNodeAutoNext(TriggerNode pNode)
        {

        }
    }
}