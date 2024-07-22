using Framework.Core;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
namespace TopGame.Timeline
{
    [System.Serializable]
    public struct ActorActionParam : BasePlayableParam
    {
        [Framework.Data.DisplayNameGUI("动作名")]
        public string actionName;
        [Framework.Data.DisplayNameGUI("动作类型")]
        public EActionStateType actionStateType;
        [Framework.Data.DisplayNameGUI("动作Tag")]
        public int nTag;
        [Framework.Data.DisplayNameGUI("执行次数")]
        public int ExcudeTimes;

        [Framework.Data.DisplayNameGUI("同步时间轴")]
        public bool SyncDuration;

        [System.NonSerialized]
        int m_nActionNameID;

        [System.NonSerialized]
        int m_nTriggedTimes;
        public void Reset()
        {
            m_nTriggedTimes = ExcudeTimes;
        }
        public EPlayableParamType GetParamType()
        {
            return EPlayableParamType.ActorAction;
        }

        public bool FormString(string strCmd)
        {
            if (string.IsNullOrEmpty(strCmd)) return false;
            m_nActionNameID = 0;
            actionName = null;
            string[] argvs = strCmd.Split(',');
            if (argvs.Length >= 3)
            {
                int temp;
                if (int.TryParse(argvs[0], out temp))
                    actionStateType = (EActionStateType)temp;
                int.TryParse(argvs[1], out nTag);
                int.TryParse(argvs[2], out ExcudeTimes);

                m_nTriggedTimes = ExcudeTimes;

                if(argvs.Length >= 4)
                {
                    actionName = argvs[3];
                    m_nActionNameID = Animator.StringToHash(actionName);
                }
                SyncDuration = false;
                if (argvs.Length >= 5)
                {
                    SyncDuration = argvs[4].CompareTo("1") == 0;
                }

                return true;
            }
            return false;
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            return ((int)GetParamType()).ToString() + ":" + ((int)actionStateType).ToString() + "," + nTag.ToString() + "," + ExcudeTimes + "," + actionName + "," + (SyncDuration?"1":"0");
        }
#endif

        public void OnStart(IUserTrackAsset track) { }
        public void OnStop(IUserTrackAsset track) { }
        public bool OnExcude(IUserTrackAsset track, float time, float fDuration, bool bEditor)
        {
            if(m_nTriggedTimes <0)
                return false;
            if(track.GetUserPointer()!=null)
            {
                if (track.GetUserPointer() is Actor)
                {
                    Actor pActor = track.GetUserPointer() as Actor;
                    if(!string.IsNullOrEmpty(actionName))
                    {
                        pActor.StartActionByName(actionName, 0, 1, false);
                    }
                    else
                    {
                        if (nTag != 0)
                            pActor.StartActionByTag(actionStateType, nTag, 0, 1, false);
                        else
                            pActor.StartActionByType(actionStateType, 0, 1, false, false, true);

                    }
                }
            }
            if(m_nActionNameID!=0 && track.GetAnimator())
            {
                AnimatorStateInfo state = track.GetAnimator().GetCurrentAnimatorStateInfo(0);
                if(state.fullPathHash != m_nActionNameID)
                {
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                        bEditor = true;
#endif
                    if (SyncDuration || bEditor)
                        track.GetAnimator().PlayInFixedTime(m_nActionNameID, 0, time);
                    else
                        track.GetAnimator().PlayInFixedTime(m_nActionNameID);


                    if (bEditor)
                        track.GetAnimator().Update(0);
                }
            }
            if(ExcudeTimes > 0)
                m_nTriggedTimes--;
            return true;
        }

        public void Destroy()
        {

        }
    }
}