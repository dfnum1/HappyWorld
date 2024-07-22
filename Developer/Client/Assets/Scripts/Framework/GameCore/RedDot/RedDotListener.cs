/********************************************************************
生成日期:	2020-11-24
类    名: 	RedDotListener
作    者:	JaydenHe
描    述:	红点监听器
*********************************************************************/

using Framework.Module;
using System.Collections;
using System.Collections.Generic;
using TopGame;
using TopGame.Logic;
using TopGame.UI;
using UnityEngine;

namespace TopGame.Core
{

    [RequireComponent(typeof(EmptyImage))]
    public class RedDotListener : MonoBehaviour
    {
#if UNITY_EDITOR
        [Framework.ED.DisplayDrawType("TopGame.Logic.RedDotType")]
#endif
        public int RedType;
#if UNITY_EDITOR
        [Framework.ED.DisplayDrawType("TopGame.UI.EUIType")]
#endif
        public int UIType;
        public GameObject RedObj;
        //------------------------------------------------------
        private void Awake()
        {
            ForceBind();
        }
        //------------------------------------------------------
        public void ForceBind()
        {
            if (ModuleManager.mainModule != null && ModuleManager.mainModule is Core.GameFramework)
            {
                GameFramework frameWork = ModuleManager.mainModule as Core.GameFramework;
                if(frameWork.redDotManager != null)
                    frameWork.redDotManager.BindRedDotGameObject(UIType, RedType, RedObj);
            }
        }
    }
}