#if USE_INJECTFIX
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace IFix
{
    [Configure]
    public class HotTypeConfigs
    {
        [IFix(false)]
        static IEnumerable<Type> hotfix
        {
            get
            {
                return new List<Type>()
                {
                    typeof(TopGame.Base.Util),
                    typeof(TopGame.UI.UIUtil),
                    typeof(TopGame.UI.UIManager),
                    typeof(TopGame.UI.ArtifactUtil),
                    typeof(TopGame.UI.ActivityUtil),
                    typeof(TopGame.UI.GemUtil),
                    typeof(TopGame.UI.ItemUtil),
                    typeof(TopGame.UI.SoulUtil),
                };
            }
        }
        [IFix(true)]
        static IEnumerable<Type> hotfixSub
        {
            get
            {
                return new List<Type>()
                {
                    typeof(TopGame.Data.AProxyDB),
                    typeof(TopGame.UI.UIBase),
                    typeof(TopGame.UI.UIView),
                    typeof(TopGame.UI.UILogic)
                };
            }
        }
    }
}
#endif
#endif