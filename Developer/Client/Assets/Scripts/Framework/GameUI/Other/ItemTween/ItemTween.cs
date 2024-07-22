
/********************************************************************
生成日期:	5:30:2022 10:57
类    名: 	ItemTween
作    者:	hjc
描    述:	道具收集效果
*********************************************************************/
using UnityEngine;
using System;
using TopGame.Core;
using UnityEngine.UI;
using System.Collections.Generic;
namespace TopGame.UI
{
    public class ItemTween : MonoBehaviour
    {
        // public Vector3 StartPos;
        // public Vector3 EndPos;
        [Header("随机初始速度最小值")]
        public float MinSpeed;
        [Header("随机初始速度最大值")]
        public float MaxSpeed;
        [Header("随机时长最小值")]
        public float MinDuration;
        [Header("随机时长最大值")]
        public float MaxDuration;
        public float Delay;
        public AnimationCurve SpeedCurve;
        public AnimationCurve ScaleCurve;
        public Transform PreviewEndPos;
        public int PreviewNumber = 0;
        public UISerialized Icon;

        private List<UISerialized> m_vList = new List<UISerialized>();

        //------------------------------------------------------
        public float RandomDuration()
        {
            return UnityEngine.Random.Range(MinDuration, MaxDuration);
        }
        //------------------------------------------------------
        public Vector3 RandomSpeed()
        {
            var scale = this.transform.lossyScale;
            float speed = UnityEngine.Random.Range(MinSpeed, MaxSpeed);
            float angle = UnityEngine.Random.Range((float)-Math.PI, (float)Math.PI);
            return new Vector3(speed * (float)Math.Cos(angle) * scale.x, 
                        speed * (float)Math.Sin(angle) * scale.y, 0);
        }
        //------------------------------------------------------
        public List<UISerialized> GetChildren(int count)
        {
            if (m_vList.Count < count)
            {
                int createNum = count - m_vList.Count;
                for (int i = 0; i < createNum; i++)
                {
                    UISerialized obj = GameObject.Instantiate(Icon, this.transform);
                    m_vList.Add(obj);
                }
            }
            for (int i = 0; i < m_vList.Count; i++)
            {
                // 这里不激活，在Manager激活
                m_vList[i].gameObject.SetActive(false);
            }
            return m_vList.GetRange(0, count);
        }

    }
}