/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	AInstanceAble
作    者:	HappLI
描    述:	
*********************************************************************/

#if UNITY_EDITOR
using Framework.Core;
using UnityEngine;

namespace TopGame.Core
{
    //------------------------------------------------------
    [Framework.Plugin.PluginDisiableExport]
    public class DebugEmptyInstanceAble : AInstanceAble
    {
        public override bool CanRecyle()
        {
            return false;
        }
        public override void OnRecyle()
        {
            if (!Application.isPlaying)
                GameObject.DestroyImmediate(gameObject);
            else
                GameObject.Destroy(gameObject);
        }
        public override void RecyleDestroy(int recyleMax = 2)
        {
            if (!Application.isPlaying)
                GameObject.DestroyImmediate(gameObject);
            else
                GameObject.Destroy(gameObject);
        }
        public override void Destroy()
        {
            if (!Application.isPlaying)
                GameObject.DestroyImmediate(gameObject);
            else
                GameObject.Destroy(gameObject);
        }
    }
}
#endif