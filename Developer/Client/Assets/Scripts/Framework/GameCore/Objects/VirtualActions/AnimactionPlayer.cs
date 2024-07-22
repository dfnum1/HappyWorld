/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	AnimactionPlayer
作    者:	HappLI
描    述:	AnimactionClip 播放器
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Core
{
    public class AnimactionPlayer : MonoBehaviour
    {
        public AnimationClip clip;
        [Range(0.1f, 10)] public float speed = 1;
        public bool autoPlay;
        float m_fDuraiton = 0;
        float m_fPlayTime = 0;
        bool m_bPlaying = false;
        private void Awake()
        {
            if (autoPlay)
                m_bPlaying = true;
            if (clip) m_fDuraiton = clip.averageDuration;
            else m_fDuraiton = 0;
        }
        //------------------------------------------------------
        protected void LateUpdate()
        {;
            if(m_bPlaying)
            {
                if (m_fDuraiton > 0 && clip)
                {
                    m_fPlayTime += Time.deltaTime* speed;
                    clip.SampleAnimation(this.gameObject, m_fPlayTime);
                    if (m_fPlayTime >= m_fDuraiton)
                    {
                        if (clip.isLooping) m_fPlayTime = 0;
                    }
                }
                else
                    m_bPlaying = false;
            }
#if UNITY_EDITOR
            if (clip) m_fDuraiton = clip.averageDuration;
            else m_fDuraiton = 0;
#endif
        }
    }
}
