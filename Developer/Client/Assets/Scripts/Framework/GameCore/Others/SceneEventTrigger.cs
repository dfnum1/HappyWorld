/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	Scene
作    者:	HappLI
描    述:	场景触发器
*********************************************************************/
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Core
{
    public class SceneEventTrigger : MonoBehaviour
    {
        [System.Serializable]
        public struct EventTrigger
        {
            public string           name;
            public byte             triggetCnt;
            public Vector3          position;
            public Vector3          eulerAngle;
            public Vector3          aabb_min;
            public Vector3          aabb_max;
            public List<string>     events;
#if UNITY_EDITOR
            [System.NonSerialized]
            public List<BaseEventParameter> buildEvents;
#endif
        }

        public EventTrigger[] events;

        //------------------------------------------------------
        protected void OnDestroy()
        {
        }
    } 
}
