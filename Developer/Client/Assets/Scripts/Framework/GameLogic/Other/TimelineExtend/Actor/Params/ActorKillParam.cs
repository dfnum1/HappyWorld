using Framework.Core;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
namespace TopGame.Timeline
{
    [System.Serializable]
    public struct ActorKillParam : BasePlayableParam
    {
        [System.NonSerialized]
        private bool m_bTrigged;

        public void Reset()
        {
            m_bTrigged = false;
        }
        public EPlayableParamType GetParamType()
        {
            return EPlayableParamType.ActorKill;
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
        public bool OnExcude(IUserTrackAsset userTrack, float time, float fDuration, bool bEditor)
        {
            if (m_bTrigged) return false;

            if(userTrack.GetUserPointer()!=null)
            {
                if (userTrack.GetUserPointer() is Actor)
                {
                    Actor pActor = userTrack.GetUserPointer() as Actor;
                    pActor.SetFlag(EWorldNodeFlag.Killed, true);
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