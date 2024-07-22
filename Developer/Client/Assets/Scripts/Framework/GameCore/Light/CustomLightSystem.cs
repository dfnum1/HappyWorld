using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Core
{
    [ExecuteInEditMode]
	public class CustomLightSystem : MonoBehaviour
	{
		static CustomLightSystem ms_Instance = null;
        public Behaviour[] Lights;
        bool m_bSyncRotate = false;
        bool m_bSyncLightDirection = false;
        Transform m_pTranform;
        Transform m_pFollow = null;

#if UNITY_EDITOR
        public Transform CurrentFollow
        {
            get { return m_pFollow; }
        }
#endif
        private Vector3[] m_arrBackupLightDirections = null;
        //------------------------------------------------------
        private void Awake()
        {
            m_bSyncLightDirection = false;
            m_bSyncRotate = false;
            m_pTranform = transform;
            ms_Instance = this;
            gameObject.name = "_CustomLightSystem";
            if(Lights!=null && Lights.Length>0)
            {
                m_arrBackupLightDirections = new Vector3[Lights.Length];
                for(int i =0; i < Lights.Length; ++i)
                {
                    if(Lights[i]) m_arrBackupLightDirections[i] = Lights[i].transform.eulerAngles;
                }
            }
#if UNITY_EDITOR
            if(Framework.Module.ModuleManager.startUpGame)
                DontDestroyOnLoad(gameObject);
#else
            DontDestroyOnLoad(gameObject);
#endif
        }
        //------------------------------------------------------
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
        private void LateUpdate()
        {
            if(ms_Instance == null) ms_Instance = this;
            if (m_pTranform )
            {
                if (m_pFollow)
                {
                    m_pTranform.position = m_pFollow.position;
                    if(m_bSyncRotate)  m_pTranform.eulerAngles = m_pFollow.eulerAngles;
                    else if(m_bSyncLightDirection)
                    {
                        if(Lights!=null)
                        {
                            for (int i = 0; i < Lights.Length; ++i)
                            {
                               if (Lights[i]) Lights[i].transform.eulerAngles = m_pFollow.eulerAngles;
                            }
                        }
                    }
                }
                else
                {
                    EnableLight(false);
                }
            }
        }
        //------------------------------------------------------
        private void EnableLight(bool bEnable)
        {
            if (this.enabled == bEnable) return;
            this.enabled = bEnable;
            if (!bEnable) m_bSyncRotate = false;
            if (Lights == null) return;
            for(int i =0; i < Lights.Length; ++i)
            {
                if (Lights[i]) Lights[i].enabled = bEnable;
            }
        }
        //------------------------------------------------------
        public static void SetSyncSlot(CustomLightSlot follow)
        {
            if (ms_Instance == null) return;
            SetFollow(follow ? follow.transform : null);
            if (follow)
            {
                if (ms_Instance.Lights != null)
                {
                    for (int i = 0; i < ms_Instance.Lights.Length; ++i)
                    {
                        if(ms_Instance.Lights[i] is Light)
                        {
                            Light light = ms_Instance.Lights[i] as Light;
                            light.cullingMask = follow.nCullingMask;
                            light.shadows = follow.shadowTypes;
                            if (follow.syncLightDirection)
                            {
                                light.transform.eulerAngles = follow.transform.eulerAngles;
                            }
                            else light.transform.eulerAngles = ms_Instance.m_arrBackupLightDirections[i];
                            ms_Instance.m_bSyncLightDirection = follow.syncLightDirection;
                            ms_Instance.m_bSyncRotate = follow.bSyncRotate;
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        public static void SetFollow(Transform follow)
        {
            if (ms_Instance == null) return;
            ms_Instance.m_pFollow = follow;
            ms_Instance.m_bSyncRotate = false;
            ms_Instance.m_bSyncLightDirection = false;
            if (ms_Instance.m_pFollow == null)
            {
                ms_Instance.EnableLight(false);
            }
            else
            {
                ms_Instance.EnableLight(true);
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(CustomLightSystem))]
    public class CustomLightSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CustomLightSystem system = target as CustomLightSystem;
            EditorGUILayout.ObjectField(system.CurrentFollow, typeof(Transform), true);
            base.OnInspectorGUI();
        }
    }

#endif
}
