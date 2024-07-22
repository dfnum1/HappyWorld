/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	BodyPart
作    者:	HappLI
描    述:	部位挂件,处理部位逻辑
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core
{
    public class BodyPart : MonoBehaviour, IBodyPart
    {
        bool m_bChecked = false;
        bool m_bBrokened  = false;
        StateFrame m_pStateFrame = null;

        private bool m_bShowAimParticle = false;
        private AInstanceAble m_pAimParticle = null;

        private List<Renderer> m_pRenders;

        private Transform m_pCentroid = null;

        ColorPropertyLerp m_ColorLerp = null;
        FloatPropertyLerp m_RimLerp = null;
        private void Awake()
        {
            Renderer[] renders = GetComponentsInChildren<Renderer>();
            if(renders != null)
            {
                m_pRenders = new List<Renderer>(1);
                for(int i = 0; i < renders.Length; ++i)
                {
                    if (renders[i] is SkinnedMeshRenderer || renders[i] is MeshRenderer)
                    {
                        m_pRenders.Add(renders[i]);
                    }
                }
            }
        }
        //------------------------------------------------------
        public string GetName()
        {
            return this.name;
        }
        //------------------------------------------------------
        public uint GetBodyID()
        {
            return this.bodyID;
        }
        //------------------------------------------------------
        public uint GetFrameID()
        {
            return this.frameID;
        }
        //------------------------------------------------------
        public EBodyPartState GetState()
        {
            return this.bodyPartState;
        }
        //------------------------------------------------------
        public void SetStateFrame(StateFrame stateFrame)
        {
            m_bBrokened = false;
            m_pStateFrame = stateFrame;
        }
        //------------------------------------------------------
        public StateFrame GetStateFrame()
        {
            return m_pStateFrame;
        }
        //------------------------------------------------------
        public void SetCentroid(Transform centroid)
        {
            m_pCentroid = centroid;
        }
        //------------------------------------------------------
        public void Reset(bool bAimClear = true)
        {
            m_bChecked = false;
            m_bBrokened = false;
            if(bAimClear)
            {
                m_bShowAimParticle = false;
                if (m_pAimParticle)
                    m_pAimParticle.RecyleDestroy(2);
                m_pAimParticle = null;
            }

            gameObject.SetActive(true);
            if(m_ColorLerp!=null)
            {
                m_ColorLerp.Destroy();
                m_ColorLerp = null;
            }
            if(m_RimLerp!=null)
            {
                m_RimLerp.Destroy();
                m_RimLerp = null;
            }
            MaterailBlockUtil.ClearBlock(m_pRenders);
        }
        //------------------------------------------------------
        public void UpdateAmiPoint(string strFile)
        {
            if(string.IsNullOrEmpty(strFile))
            {
                m_bShowAimParticle = false;
                if (m_pAimParticle)
                    m_pAimParticle.RecyleDestroy(1);
                m_pAimParticle = null;
            }
            else
            {
                m_bShowAimParticle = true;
                InstanceOperiaon pCb = FileSystemUtil.SpawnInstance(strFile, true);
                if(pCb!=null)
                {
                    pCb.OnSign = OnSign;
                    pCb.OnCallback = OnSpawnAimPoint;
                }
            }
        }
        //------------------------------------------------------
        public AInstanceAble GetAmiPoint()
        {
            return m_pAimParticle;
        }
        //------------------------------------------------------
        public void Brocken()
        {
            if (m_pAimParticle)
            {
                m_pAimParticle.RecyleDestroy(1);
                m_pAimParticle = null;
            }
            m_bShowAimParticle = false;
            if (m_bBrokened) return;
            if (listenActionHide == 0)
            {
                gameObject.SetActive(false);
                m_bChecked = true;
            }
            else
                m_bChecked = false;
            m_bBrokened = true;
            if (m_ColorLerp != null)
            {
                m_ColorLerp.Destroy();
                m_ColorLerp = null;
            }
            if(m_RimLerp!=null)
            {
                m_RimLerp.Destroy();
                m_RimLerp = null;
            }
            MaterailBlockUtil.ClearBlock(m_pRenders);
        }
        //------------------------------------------------------
        public void OnHit()
        {
        }
        //------------------------------------------------------
        public Vector3 GetPosition()
        {
            if (m_pCentroid) return m_pCentroid.position;
            return transform.position;
        }
        //------------------------------------------------------
        public Vector3 GetEulerAngle()
        {
            if (m_pCentroid) return m_pCentroid.eulerAngles;
            return transform.eulerAngles;
        }
        //------------------------------------------------------
        public Transform GetTransform()
        {
            if (m_pCentroid) return m_pCentroid;
            return transform;
        }
        //------------------------------------------------------
        void OnSign(InstanceOperiaon pCb)
        {
            pCb.bUsed = !m_bBrokened && m_bShowAimParticle;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            if (m_pAimParticle)
                m_pAimParticle.RecyleDestroy(1);
             m_pAimParticle = null;
            m_bShowAimParticle = false;
            MaterailBlockUtil.ClearBlock(m_pRenders);
        }
        //------------------------------------------------------
        void OnSpawnAimPoint(InstanceOperiaon pCb)
        {
            m_pAimParticle = pCb.pPoolAble;
        }
        //------------------------------------------------------
        public void OnActionStateBegin(ActionState state)
        {
            if (m_bChecked) return;
        }
        //------------------------------------------------------
        public void OnActionStateEnd(ActionState state)
        {
            if(m_bBrokened && !m_bChecked)
            {
                if (listenActionHide == state.GetCore().id)
                {
                    gameObject.SetActive(false);
                    m_bChecked = true;

                    if (m_pAimParticle)
                        m_pAimParticle.RecyleDestroy(1);
                    m_pAimParticle = null;
                }
            }
        }
        //------------------------------------------------------
        private void Update()
        {
            if (m_bBrokened)
            {
                return;
            }
            if(m_ColorLerp!=null)
            {
                if (!m_ColorLerp.Update(Time.deltaTime))
                    m_ColorLerp.Clear();
            }
            if(m_RimLerp!=null)
            {
                if (!m_RimLerp.Update(Time.deltaTime))
                    m_RimLerp.Clear();
            }
            if (m_pAimParticle == null) return;
            m_pAimParticle.SetPosition(GetPosition());
            m_pAimParticle.SetEulerAngle(CameraKit.MainCameraEulerAngle);
        }
        //------------------------------------------------------
        public void ApplayEffect(string propertyName, float fDuration, AnimationCurve R, AnimationCurve G, AnimationCurve B, AnimationCurve A)
        {
            if (m_pRenders == null) return;
            if (m_ColorLerp == null)
                m_ColorLerp = ColorPropertyLerp.Malloc();
            m_ColorLerp.binderObject = this;
            m_ColorLerp.renders = m_pRenders;
            m_ColorLerp.propertyName = propertyName;
            m_ColorLerp.fLerp = 0;
            m_ColorLerp.fDuration = fDuration;
            m_ColorLerp.r = R;
            m_ColorLerp.g = G;
            m_ColorLerp.b = B;
            m_ColorLerp.a = A;
        }
        //------------------------------------------------------
        public void ApplayRimEffect(string propertyName, float fDuration, AnimationCurve Rim)
        {
            if (m_pRenders == null) return;
            if (m_RimLerp == null)
                m_RimLerp = FloatPropertyLerp.Malloc();
            m_RimLerp.binderObject = this;
            m_RimLerp.renders = m_pRenders;
            m_RimLerp.propertyName = propertyName;
            m_RimLerp.fLerp = 0;
            m_RimLerp.fDuration = fDuration;
            m_RimLerp.value = Rim;
        }
        //------------------------------------------------------
        public void SetPropertyColor(string propertyName, Color color)
        {
            MaterailBlockUtil.SetRendersColor(m_pRenders, propertyName, color);
        }
        //------------------------------------------------------
        public void SetPropertyFloat(string propertyName, float fValue)
        {
            MaterailBlockUtil.SetRendersFloat(m_pRenders, propertyName, fValue);
        }
        //------------------------------------------------------
        public bool brokened
        {
            get
            {
                return m_bBrokened;
            }
        }
        //------------------------------------------------------
        public uint bodyID
        {
            get
            {
                if (m_pStateFrame == null) return 0xffffffff;
                return m_pStateFrame.bodyPartID;
            }
        }
        //------------------------------------------------------
        public uint frameID
        {
            get
            {
                if (m_pStateFrame == null) return 0xffffffff;
                return m_pStateFrame.id;
            }
        }
        //------------------------------------------------------
        public EBodyPartState bodyPartState
        {
            get
            {
                if (m_pStateFrame != null) return m_pStateFrame.bodyPartState;
                return EBodyPartState.Disable;
            }
        }
        //------------------------------------------------------
        public uint listenActionHide
        {
            get
            {
                if (m_pStateFrame != null) return m_pStateFrame.listenActionHide;
                return 0;
            }
        }
    }
}
