/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	Scene
作    者:	HappLI
描    述:	镜像检测
*********************************************************************/
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Framework.Core;

namespace TopGame.Core
{
    public class LevelScene : AInstanceAble
    {
        [System.Serializable]
        public struct CameraPoint
        {
            public Vector3 position;
            public Vector3 inTan;
            public Vector3 outTan;
        }

        [System.Serializable]
        public struct LinePoint
        {
            public Vector3 position;
            public Vector3 rotate;
        }

        [HideInInspector]
        public WorldTriggerParamter[] worldTriggers;

        [HideInInspector] public List<LinePoint> LinePoints;
        [HideInInspector] public List<CameraPoint> CameraPoints;

        public bool Mirror = false;
        public int RoadCnt = 3;
        public float RoadWidth = 2.5f;
        public float SizeDepth = 200.0f;
        public Vector3 BoxSize = new Vector3(7.5f,10.0f, 200.0f);

        private int m_nDir = 1;
        //------------------------------------------------------
        public override void OnRecyle()
        {
            base.OnRecyle();
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        //------------------------------------------------------
        private void Update()
        {
            if(Mirror)
            {
                int dir = Vector3.Dot(CameraKit.MainCameraDirection, Vector3.forward)>0?1:-1;
                if(dir != m_nDir)
                {
                    m_nDir = dir;
                    Vector3 scale = m_pTransform.localScale;
                    scale.z = Mathf.Abs(scale.z)* m_nDir;
                    m_pTransform.localScale = scale;
                }
            }
        }
    }

}
