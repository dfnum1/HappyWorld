/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	LoginVirtualScene
作    者:	HappLI
描    述:	登录虚拟场景
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using TopGame.Data;
namespace TopGame.Logic
{
    public class LoginVirtualScene : MonoBehaviour
    {
        [System.Serializable]
        public struct SceneNode
        {
            public string strFile;
            public Vector3 position;
            public Vector3 rotation;
#if UNITY_EDITOR
            [System.NonSerialized]
            public bool expand;
#endif
        }
        [System.Serializable]
        public struct Theme
        {
            public string name;
            public List<SceneNode> scenes;
            public Data.SceneThemeData theme;
#if UNITY_EDITOR
            [System.NonSerialized]
            public bool expand;
            [System.NonSerialized]
            public bool expandTheme;
#endif
        }

        public List<Theme> themes = new List<Theme>();
        public Core.TimelineController timelineController;
#if UNITY_EDITOR
        [System.NonSerialized]
        public bool expandPaths;
#endif
    }
}

