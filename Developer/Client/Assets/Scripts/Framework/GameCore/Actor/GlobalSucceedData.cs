/********************************************************************
生成日期:	2:14:2020 10:29
类    名: 	GlobalSucceedAction
作    者:	HappLI
描    述:	全局动作衔接序列
*********************************************************************/
using Framework.Core;
using Framework.Data;
using System.Collections.Generic;
using UnityEngine;
namespace TopGame.Core
{
    // [CreateAssetMenu]
    [ExternEngine.ConfigPath("sos/GlobalSucceedActions.asset")]
    public class GlobalSucceedData : AGlobalSucceedData
    {

    }

#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(GlobalSucceedData), true)]
    public class GlobalSucceedDataEditor : AGlobalSucceedActionEditor
    {
        public override AConfig CreateOriData()
        {
            string jsonConent = ((GlobalSucceedData)target).jsonConent;
            if(string.IsNullOrEmpty(jsonConent))
                return new GlobalSucceedAction();
            return JsonUtility.FromJson<GlobalSucceedAction>(jsonConent);
        }
    }
#endif
}

