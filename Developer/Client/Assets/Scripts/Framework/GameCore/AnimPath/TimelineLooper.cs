/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	TimelineLooper
作    者:	HappLI
描    述:	
*********************************************************************/
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

namespace TopGame.Core
{
    public class TimelineLooper : MonoBehaviour
    {
        PlayableDirector timeliner;
        public float LoopBegin;
        public float LoopEnd;
        private void Awake()
        {
            timeliner = GetComponent<PlayableDirector>();
        }
        private void Update()
        {
            if(LoopEnd> LoopBegin)
            {
                if (timeliner == null) return;
                if (timeliner.time >= LoopEnd)
                    timeliner.time = LoopBegin;
            }
        }
    }
}