using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
namespace TopGame.Timeline
{
    [System.Serializable]
    public struct ShakeParam : BasePlayableParam
    {

        [Framework.Data.DisplayNameGUI("强度")]
        public Vector3 intense ;
        [Framework.Data.DisplayNameGUI("振幅")]
        public Vector3 hertz;
        [Framework.Data.DisplayNameGUI("还原")]
        public bool revert;

        Transform m_pTransform;
        Vector3 m_BackupPosition;
        public void Reset()
        {
            m_pTransform = null;
            m_BackupPosition = Vector3.zero;
        }
        public EPlayableParamType GetParamType()
        {
            return EPlayableParamType.Shake;
        }

        public bool FormString(string strCmd)
        {
            if (string.IsNullOrEmpty(strCmd)) return false;

            m_pTransform = null;
            m_BackupPosition = Vector3.zero;

            string[] argvs = strCmd.Split(',');
            if (argvs.Length <= 0) return false;
            intense.x = float.Parse(argvs[0]);

            if (argvs.Length <= 1) return false;
            intense.y = float.Parse(argvs[1]);

            if (argvs.Length <= 2) return false;
            intense.z = float.Parse(argvs[2]);

            if (argvs.Length <= 3) return false;
            hertz.x = float.Parse(argvs[3]);

            if (argvs.Length <= 4) return false;
            hertz.y = float.Parse(argvs[4]);

            if (argvs.Length <= 5) return false;
            hertz.z = float.Parse(argvs[5]);

            if (argvs.Length <= 6) return true;
            revert = argvs[6].CompareTo("1") == 0;
            return true;
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            string strParam = ((int)GetParamType()).ToString()+":";
            strParam += intense.x.ToString() + "," + intense.y.ToString() + "," + intense.z.ToString();
            strParam += "," + hertz.x.ToString() + "," + hertz.y.ToString() + "," + hertz.z.ToString();
            strParam += "," + (revert?"1":"0");
            return strParam;
        }
#endif

        public void OnStart(IUserTrackAsset track)
        {
            m_pTransform = null;
            m_BackupPosition = Vector3.zero;
        }
        public void OnStop(IUserTrackAsset track)
        {
            if(m_pTransform && revert)
            {
                m_pTransform.position = m_BackupPosition;
            }
        }
        public bool OnExcude(IUserTrackAsset userTrack, float time, float fDuration, bool bEditor)
        {
            if (fDuration<=0) return false;
            if (userTrack.GetBinder() == null) return false;
            Transform tranfrom = userTrack.GetBinder();
            if(m_pTransform == null)
            {
                m_pTransform = tranfrom;
                m_BackupPosition = m_pTransform.position;
            }

            float fFade = Mathf.Clamp01(time / fDuration);

            float dampping = 1;

            float fShakeX = intense.x * ((float)Mathf.Sin(hertz.x * time)) * dampping;
            float fShakeY = intense.y * ((float)Mathf.Sin(hertz.y * time)) * dampping;
            float fShakeZ = intense.z * ((float)Mathf.Sin(hertz.z * time)) * dampping;

            Vector3 offset = fShakeX * tranfrom.forward + fShakeY * tranfrom.up + fShakeZ * tranfrom.right;
            offset *= fFade;

            tranfrom.position = m_BackupPosition+offset;
            return true;
        }

        public void Destroy()
        {

        }
    }
}