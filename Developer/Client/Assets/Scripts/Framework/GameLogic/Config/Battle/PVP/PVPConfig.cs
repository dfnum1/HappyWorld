/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	PVPConfig
作    者:	HappLI
描    述:	PVP 配置
*********************************************************************/
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
#if USE_SERVER
using MonoBehaviour = ExternEngine.MonoBehaviour;
using Transform = ExternEngine.Transform;
#endif

namespace TopGame.Logic
{
    [ExternEngine.ConfigPath("sos/pvpConfig.mono",true)]
    public class PVPConfig : MonoBehaviour
    {
        static PVPConfig ms_Instance = null;
        public float ActiveRadius = 12;
        public ushort battleSceneCameaID = 14;
        public Vector3 teamGap = new Vector3(5, 0, 5);
        public Vector3 team0Position = new Vector3(0,0,-26);
        public Vector3 team1Position = new Vector3(0, 0, 26);
        public Vector3 team1PreparePosition = new Vector3(0, 0, 26);
        public Vector3 limitZoomMin = new Vector3(-8, 0, -31);
        public Vector3 limitZoomMax = new Vector3(8, 0, 31);
        //------------------------------------------------------
        private void Awake()
        {
            ms_Instance = this;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_Instance = null;
        }
        //------------------------------------------------------
        public static float activeRadius
        {
            get
            {
                if (ms_Instance == null) return 12;
                return ms_Instance.ActiveRadius;
            }
        }
        //------------------------------------------------------
        public static Vector3 activeRegion
        {
            get
            {
                if (ms_Instance == null) return new Vector3(0,0,32);
                return ms_Instance.transform.position;
            }
        }
        //------------------------------------------------------
        public static ushort nBattleSceneCameaID
        {
            get
            {
                if (ms_Instance == null) return 14;
                return ms_Instance.battleSceneCameaID;
            }
        }
        //------------------------------------------------------
        public static Vector3 zoomMin
        {
            get
            {
                if (ms_Instance == null) return new Vector3(-8, 0, -31);
                return Vector3.Min(ms_Instance.limitZoomMin, ms_Instance.limitZoomMax) + ms_Instance.transform.position;
            }
        }
        //------------------------------------------------------
        public static Vector3 zoomMax
        {
            get
            {
                if (ms_Instance == null) return new Vector3(8,0,31);
                return Vector3.Max(ms_Instance.limitZoomMin, ms_Instance.limitZoomMax)+ms_Instance.transform.position;
            }
        }
        //------------------------------------------------------
        public static Vector3 TeamGap
        {
            get
            {
                if (ms_Instance == null) return new Vector3(5, 0, 5);
                return ms_Instance.teamGap;
            }
        }
        //------------------------------------------------------
        public static Vector3 Team0Position
        {
            get
            {
                if (ms_Instance == null) return new Vector3(0, 0, 5);
                return ms_Instance.team0Position + ms_Instance.transform.position;
            }
        }
        //------------------------------------------------------
        public static Vector3 Team1Position
        {
            get
            {
                if (ms_Instance == null) return new Vector3(0, 0, 20);
                return ms_Instance.team1Position + ms_Instance.transform.position;
            }
        }
        //------------------------------------------------------
        public static Vector3 Team1PreparePosition
        {
            get
            {
                if (ms_Instance == null) return new Vector3(0, 0, 20);
                return ms_Instance.team1PreparePosition + ms_Instance.transform.position;
            }
        }
    }
#if UNITY_EDITOR
    //------------------------------------------------------
    [CustomEditor(typeof(PVPConfig), true)]
    [CanEditMultipleObjects]
    public class PVPConfigEditor : Editor
    {
        private void OnSceneGUI()
        {
            PVPConfig pvp = target as PVPConfig;
            //Handles.CircleHandleCap(0, pvp.transform.position+Vector3.up*0.1f, Quaternion.Euler(90, 0, 0), pvp.ActiveRadius, EventType.Repaint);
            UnityEditor.Handles.DrawWireCube(pvp.transform.position, new Vector3(32, 0.1f, pvp.ActiveRadius));

            Handles.Label(pvp.limitZoomMin + pvp.transform.position, "Min");
            Handles.Label(pvp.limitZoomMax + pvp.transform.position, "Max");
            pvp.limitZoomMin = Handles.PositionHandle(pvp.limitZoomMin + pvp.transform.position, Quaternion.identity) - pvp.transform.position;
            pvp.limitZoomMax = Handles.PositionHandle(pvp.limitZoomMax + pvp.transform.position, Quaternion.identity) - pvp.transform.position;

            Handles.Label(pvp.team0Position + pvp.transform.position, "玩家1队伍位置");
            Handles.Label(pvp.team1Position + pvp.transform.position, "玩家2队伍位置");
            Handles.Label(pvp.team1PreparePosition + pvp.transform.position, "玩家2队伍备战位置");
            pvp.team0Position = Handles.PositionHandle(pvp.team0Position + pvp.transform.position, Quaternion.identity) - pvp.transform.position;
            pvp.team1Position = Handles.PositionHandle(pvp.team1Position + pvp.transform.position, Quaternion.identity) - pvp.transform.position;
            pvp.team1PreparePosition = Handles.PositionHandle(pvp.team1PreparePosition + pvp.transform.position, Quaternion.identity) - pvp.transform.position;

            Color color = Handles.color;
            Handles.color = Color.red;
            float posY = pvp.transform.position.y + 0.5f;
            Vector3 min = new Vector3(pvp.limitZoomMin.x + pvp.transform.position.x, 0, pvp.limitZoomMin.z + pvp.transform.position.z);
            Vector3 max = new Vector3(pvp.limitZoomMax.x + pvp.transform.position.x, 0, pvp.limitZoomMax.z + pvp.transform.position.z);
            Handles.DrawLine(new Vector3(min.x, posY, min.z), new Vector3(min.x, posY, max.z));
            Handles.DrawLine(new Vector3(min.x, posY, max.z), new Vector3(max.x, posY, max.z));
            Handles.DrawLine(new Vector3(max.x, posY, max.z), new Vector3(max.x, posY, min.z));
            Handles.DrawLine(new Vector3(max.x, posY, min.z), new Vector3(min.x, posY, min.z));
            Handles.color = color;
        }
    }
#endif
}

