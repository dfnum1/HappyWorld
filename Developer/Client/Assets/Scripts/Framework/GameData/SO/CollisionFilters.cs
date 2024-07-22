/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	CollisionConfig
作    者:	HappLI
描    述:	碰撞标志
*********************************************************************/
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using System.IO;
using Framework.Core;
using Framework.Base;
#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif
#if USE_SERVER
using ScriptableObject = ExternEngine.ScriptableObject;
#endif
namespace TopGame.Core
{
    //[CreateAssetMenu]
    [ExternEngine.ConfigPath("sos/CollisonFilters.asset")]
    public class CollisionFilters : ScriptableObject
    {
        public ushort[] bitObstacleIgnoreFilter = null;
        [Framework.Data.DisplayNameGUI("吸铁石可吸收"), Framework.Data.DisplayEnumBitGUI("Framework.Base.EObstacleType", true) ]
        public uint magentFlilter = 0;
        static CollisionFilters ms_Instance = null;
        private void OnEnable()
        {
            ms_Instance = this;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_Instance = null;
        }
        //------------------------------------------------------
        public static ushort[] GetObstacleFilters()
        {
            if (ms_Instance == null) return null;
            return ms_Instance.bitObstacleIgnoreFilter;
        }
        //------------------------------------------------------
        public static ushort GetObstacleFilter(int type)
        {
            if (ms_Instance == null || ms_Instance.bitObstacleIgnoreFilter == null || type >= ms_Instance.bitObstacleIgnoreFilter.Length)
                return 0;
            return ms_Instance.bitObstacleIgnoreFilter[type];
        }
        //------------------------------------------------------
        public static uint GetMagentFilter()
        {
            if (ms_Instance == null)
                return 0;
            return ms_Instance.magentFlilter;
        }
    }
}