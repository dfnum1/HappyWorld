//#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ParticleScalerSimulate
作    者:	HappLI
描    述:	特效调试
*********************************************************************/
using UnityEngine;
using UnityEditor;
using Framework.Module;
using TopGame.Logic;
using TopGame.Core;

namespace TopGame.Logic
{
    [ExecuteAlways]
    [ExecuteInEditMode]
    public class ParticleScalerSimulate : MonoBehaviour
    {
        public Transform form;
        public Transform to;

        private ParticleController m_pControl;
        private void Awake()
        {
            hideFlags = HideFlags.DontSave;
            m_pControl = GetComponent<ParticleController>();
            if (m_pControl == null) m_pControl = GetComponentInChildren<ParticleController>();
        }
        //------------------------------------------------------
        private void Update()
        {
            if (m_pControl == null) return;
            if (form == null || to == null) return;
            Vector3 curPos = m_pControl.transform.position;
            float dist = (form.position - to.position).sqrMagnitude;
            if (dist <= 0.01f) return;
            float cur = (curPos - form.position).sqrMagnitude / dist;
            m_pControl.SetPlayLifeTimeScale(Mathf.Clamp01(cur), Vector3.one);
        }
    }
}
//#endif
