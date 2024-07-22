using Framework.Core;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
namespace TopGame.Timeline
{
    [System.Serializable]
    public struct ActorDestroyParam : BasePlayableParam
    {
        [System.NonSerialized]
        private bool m_bTrigged;

        public void Reset()
        {
            m_bTrigged = false;
        }
        public EPlayableParamType GetParamType()
        {
            return EPlayableParamType.ActorDestroy;
        }

        public bool FormString(string strCmd)
        {
            m_bTrigged = false;
            return true;
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            return GetParamType().ToString()+":";
        }
#endif

        public void OnStart(IUserTrackAsset track) { }
        public void OnStop(IUserTrackAsset track) { }
        public bool OnExcude(IUserTrackAsset track,float time, float fDuration, bool bEditor)
        {
            if (m_bTrigged) return false;
            if(track.GetUserPointer()!=null)
            {
                if (track.GetUserPointer() is Actor)
                {
                    Actor pActor = track.GetUserPointer() as Actor;
                    pActor.Destroy();
                }
            }

            m_bTrigged = true;
            return true;
        }

        public void Destroy()
        {

        }
    }
}