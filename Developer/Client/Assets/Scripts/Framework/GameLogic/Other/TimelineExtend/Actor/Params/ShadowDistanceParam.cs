using Framework.Core;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
namespace TopGame.Timeline
{
    [System.Serializable]
    public struct ShadowDistanceParam : BasePlayableParam
    {
        [Framework.Data.DisplayNameGUI("ShadowDistance")]
        public AnimationCurve distance;

        float m_BackupDistance;
        public EPlayableParamType GetParamType()
        {
            return EPlayableParamType.ShadowDistance;
        }
        //------------------------------------------------------
        public void Reset()
        {
            m_BackupDistance = -1;
        }
        //------------------------------------------------------
        public bool FormString(string strCmd)
        {
            m_BackupDistance = -1;
            if (strCmd == null) return false;
            distance = EventHelper.ReadCurve(strCmd);
            return true;
        }
        //------------------------------------------------------
#if UNITY_EDITOR
        public override string ToString()
        {
            string strParam = ((int)GetParamType()).ToString()+":";
            strParam += EventHelper.SaveCurve(distance);
            return strParam;
        }
#endif
        //------------------------------------------------------
        public void OnStart(IUserTrackAsset track)
        {
            m_BackupDistance = -1;
            UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset = GetUrpAsset();
            if (urpAsset == null) return;
            m_BackupDistance = urpAsset.shadowDistance;
        }
        //------------------------------------------------------
        public void OnStop(IUserTrackAsset track)
        {
        }
        //------------------------------------------------------
        UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset GetUrpAsset()
        {
            return UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
        }
        //------------------------------------------------------
        public bool OnExcude(IUserTrackAsset userTrack, float time, float fDuration, bool bEditor)
        {
            if (fDuration<=0) return false;
            if (distance == null ) return false;
            UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset = GetUrpAsset();
            if (urpAsset == null) return false;
            if (m_BackupDistance < 0)
                m_BackupDistance = urpAsset.shadowDistance;

            float fFade = Mathf.Clamp01(time / fDuration);
            float shadowDistance = distance.Evaluate(fFade);
            if(shadowDistance>0) urpAsset.shadowDistance = shadowDistance;
            return true;
        }
        //------------------------------------------------------
        public void Destroy()
        {
            if (m_BackupDistance < 0) return;
            UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset = GetUrpAsset();
            if (urpAsset == null) return;
            urpAsset.shadowDistance = m_BackupDistance;

        }
    }
}