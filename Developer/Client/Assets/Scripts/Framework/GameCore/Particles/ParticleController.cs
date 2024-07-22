/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ParticleController
作    者:	HappLI
描    述:	特效控制器
*********************************************************************/
using DG.Tweening;
using ExternEngine;
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
namespace TopGame.Core
{
    public class ParticleController : AParticleController
    {
        protected struct ParSystemRender
        {
            public ParticleSystemRenderer render;
            public int sorting_order;
        }
        protected struct ParSystem
        {
            public ParticleSystem system;
            public ParticleSystem.MinMaxCurve speed;
            public ParticleSystem.MinMaxCurve startLife;
        }
        public AnimationCurve scaleLife = null;
        TrailRenderer[] m_arrTrails = null;
        protected ParSystem[] m_arrSystems = null;
        List<DOTweenAnimation> m_arrDoTweens = null;
        List<ParSystemRender> m_arrRenders = null;
        bool m_bEnable = false;
        bool m_bStopChecing = false;
        float m_fPlayingStatCheck = 0;

        float m_fCheckTime = 0;
        bool m_bFreezed = false;

        bool m_bResetDirty = false;
        bool m_bCheckedTrail = false;

        private Framework.Plugin.AT.IUserData m_pOwner = null;

        private Transform m_pSlot = null;
        private byte m_BindBit = (byte)(ESlotBindBit.Position | ESlotBindBit.Rotation);
        private Vector3 m_SlotOffset = Vector3.zero;
        private Vector3 m_SlotEulerOffset = Vector3.zero;
        protected float m_fLiftTime = -1;

        [Header("是否关闭特效检测")]
        public bool bIsDisableCheck = false;

        bool m_bValidPosCheck = false;
        int m_nAutoDestroyFlag = 1;
        //------------------------------------------------------
        public override void SetOwner(Framework.Plugin.AT.IUserData pNode)
        {
            m_pOwner = pNode;
            if(m_pOwner!=null && m_pOwner is AWorldNode)
            {
                AWorldNode worldNode = m_pOwner as AWorldNode;
            }
        }
        //------------------------------------------------------
        public override void SetDisableCheck(bool bDisableCheck)
        {
            bIsDisableCheck = bDisableCheck;
        }
        //------------------------------------------------------
        public override void BindSlot(string strSlot, Vector3 offset, Vector3 euler, ESlotBindBit bitType = ESlotBindBit.All)
        {
            m_pSlot = DyncmicTransformCollects.FindTransformByName(strSlot);
            if (m_pSlot == null) return;
            m_BindBit = (byte)bitType;
            m_SlotOffset = offset;
            m_SlotEulerOffset = euler;
        }
        //------------------------------------------------------
        public override void BindSlot(Transform pSlot, Vector3 offset, Vector3 euler, ESlotBindBit bitType = ESlotBindBit.All)
        {
            m_pSlot = pSlot;
            if (m_pSlot == null) return;
            m_BindBit = (byte)bitType;
            m_SlotOffset = offset;
            m_SlotEulerOffset = euler;
        }
        //------------------------------------------------------
        public override void SetLifeTime(float liftTime)
        {
            m_fLiftTime = liftTime;
        }
        //------------------------------------------------------
        public override void SetPlayLifeTimeScale(float fLifeTime, Vector3 scale)
        {
            if (scaleLife == null || scaleLife.length <= 0) return;
            SetScale((scaleLife.Evaluate(fLifeTime) + 1) * scale);
        }
        //------------------------------------------------------
        public void SetRenderOrder(int order, bool bAmount = false)
        {
            if (m_arrRenders == null)
            {
                ParticleSystemRenderer[] renders = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();
                m_arrRenders = new List<ParSystemRender>(renders.Length);
                for (int i = 0; i < renders.Length; ++i)
                {
                    ParSystemRender par = new ParSystemRender();
                    par.render = renders[i];
                    par.sorting_order = par.render.sortingOrder;
                    m_arrRenders.Add(par);
                }
            }
            if (m_arrRenders == null) return;
            for (int i = 0; i < m_arrRenders.Count; ++i)
            {
                if(m_arrRenders[i].render) m_arrRenders[i].render.sortingOrder = bAmount?(order+ m_arrRenders[i].sorting_order):order;
            }
        }
        //------------------------------------------------------
        public void BackupRenderOrder()
        {
            if (m_arrRenders == null) return;
            for (int i = 0; i < m_arrRenders.Count; ++i)
            {
                if (m_arrRenders[i].render) m_arrRenders[i].render.sortingOrder = m_arrRenders[i].sorting_order;
            }
        }
        //------------------------------------------------------
        void CheckParticleSystems()
        {
            if(m_arrSystems == null)
            {
                ParticleSystem[] pars = gameObject.GetComponentsInChildren<ParticleSystem>();
                if (pars != null && pars.Length > 0)
                {
                    m_arrSystems = new ParSystem[pars.Length];
                    for (int i = 0; i < pars.Length; ++i)
                    {
                        ParSystem par = new ParSystem();
                        par.system = pars[i];
                        par.speed = pars[i].main.startSpeed;
                        par.startLife = pars[i].main.startLifetime;
                        m_arrSystems[i] = par;
                    }
                }
            }

            m_bStopChecing = true;
            if (m_arrSystems != null)
            {
                for (int i = 0; i < m_arrSystems.Length; ++i)
                {
                    if (m_arrSystems[i].system.main.loop)
                    {
                        m_bStopChecing = false;
                        break;
                    }
                }
            }
        }
        //------------------------------------------------------
        protected override void Awake()
        {
            base.Awake();
            m_bFreezed = false;
            m_arrTrails = gameObject.GetComponentsInChildren<TrailRenderer>();
            m_bStopChecing = true;
            m_bEnable = true;
            CheckParticleSystems();
            DOTweenAnimation[] tweens = gameObject.GetComponentsInChildren<DOTweenAnimation>();
            if (tweens != null && tweens.Length > 0)
            {
                if (m_arrDoTweens == null)
                    m_arrDoTweens = new List<DOTweenAnimation>(tweens);
            }
            DOTweenAnimation tween = gameObject.GetComponent<DOTweenAnimation>();
            if (tween)
            {
                if (m_arrDoTweens == null) m_arrDoTweens = new List<DOTweenAnimation>(1);
                m_arrDoTweens.Add(tween);
            }
            SetDirty();
        }
        //------------------------------------------------------
        public override bool IsLooping()
        {
#if UNITY_EDITOR
            CheckParticleSystems();
#endif
            return !m_bStopChecing;
        }
        //------------------------------------------------------
        public override void OnRecyle()
        {
            base.OnRecyle();
            if (m_arrSystems != null)
            {
                ParSystem par;
                for (int i = 0; i < m_arrSystems.Length; ++i)
                {
                    par = m_arrSystems[i];
                    if (par.system == null) continue;
                    par.system.Pause(true);

                    ParticleSystem.MainModule main = par.system.main;
                    if(!main.prewarm)
                    {
                        par.system.time = 0;
                        par.system.Clear(true);
                    }
                    main.startSpeed = par.speed;
                    main.startLifetime = par.startLife;
                }
            }
            if (m_arrTrails != null)
            {
                for (int i = 0; i < m_arrTrails.Length; ++i)
                {
                    if(m_arrTrails[i]) m_arrTrails[i].Clear();
                }
            }
            EnableTrailEmitting(false);
            m_bEnable = false;
            m_bResetDirty = false;
            m_fPlayingStatCheck = 0;

            m_pSlot = null;
            m_SlotEulerOffset = Vector3.zero;
            m_SlotOffset = Vector3.zero;
            m_fLiftTime = -1;

            m_bFreezed = false;
            m_pOwner = null;
            m_nAutoDestroyFlag = 1;
            BackupRenderOrder();
        }
        //------------------------------------------------------
        protected void SetDirty()
        {
            m_bResetDirty = true;
            m_fPlayingStatCheck = 0;
            if (m_arrSystems != null)
            {
                ParSystem par;
                for (int i = 0; i < m_arrSystems.Length; ++i)
                {
                    par = m_arrSystems[i];
                    if (par.system == null) continue;
                    if (!par.system.main.prewarm)
                    {
                        par.system.Clear(true);
                        par.system.time = 0;
                    }
                }
            }
            ResetTrails();
            m_bCheckedTrail = true;
        }
        //------------------------------------------------------
        public override void ResetTrails()
        {
            if (m_arrTrails != null)
            {
                for (int i = 0; i < m_arrTrails.Length; ++i)
                {
                    if(m_arrTrails[i]) m_arrTrails[i].Clear();
                }
            }
        }
        //------------------------------------------------------
        public override void EnableTrailEmitting(bool bEnable)
        {
            if (m_arrTrails != null)
            {
                Vector3 scale = GetTransorm().localScale;
                for (int i = 0; i < m_arrTrails.Length; ++i)
                {
                    if (m_arrTrails[i]) m_arrTrails[i].emitting= bEnable;
                }
            }
        }
        //------------------------------------------------------
        void ResetDirty()
        {
            if (m_bResetDirty)
            {
                m_fPlayingStatCheck = 0;
                if (m_arrSystems != null)
                {
                    ParSystem par;
                    for (int i = 0; i < m_arrSystems.Length; ++i)
                    {
                        par = m_arrSystems[i];
                        if (par.system == null) continue;

                        m_fPlayingStatCheck = Mathf.Max(par.system.startDelay+ par.system.main.duration, m_fPlayingStatCheck);
                        //                         par.system.Clear(true);
                        //                         par.system.time = 0;
                        par.system.Play(true);
                    }
                }
                m_fPlayingStatCheck += Time.time+2;
                ResetTrails();
                if (m_arrDoTweens != null)
                {
                    for (int i = 0; i < m_arrDoTweens.Count; ++i)
                    {
                        if (m_arrDoTweens[i].tween == null)
                            m_arrDoTweens[i].CreateTween();
                        m_arrDoTweens[i].DORestart();
                    }
                }
                m_bResetDirty = false;
                m_bFreezed = false;
            }
            else if(m_bCheckedTrail && m_arrTrails != null && m_arrTrails.Length>0)
            {
                bool bReset = false;
                for (int i = 0; i < m_arrTrails.Length; ++i)
                {
                    if (m_arrTrails[i])
                    {
                        for (int j =0; j < m_arrTrails[i].positionCount; ++j)
                        {
                            if(Framework.Core.CommonUtility.Equal(m_arrTrails[i].GetPosition(j), Framework.Core.CommonUtility.INVAILD_POS, 10))
                            {
                                bReset = true;
                                break;
                            }
                        }
                    }
                    if (bReset) break;
                }
                if (bReset)
                {
                    ResetTrails();
                }
                m_bCheckedTrail = false;
            }
        }
        //------------------------------------------------------
        public override void OnFreezed(bool bFreezed)
        {
            base.OnFreezed(bFreezed);
            m_bFreezed = bFreezed;
            if (bFreezed) Pause();
            else Resume();
        }
        //------------------------------------------------------
        private void Update()
        {
            if (!m_bEnable)
            {
                if(m_bValidPosCheck)
                {
                    if (!Framework.Core.CommonUtility.Equal(GetPosition(), Framework.Core.CommonUtility.INVAILD_POS, 10))
                    {
                        SetRealStart();
                    }
                }
                return;
            }
            if (m_pSlot)
            {
                if ((m_BindBit & (byte)ESlotBindBit.Position) != 0)
                    SetPosition(m_pSlot.position + m_SlotOffset);
                if ((m_BindBit & (byte)ESlotBindBit.Rotation) != 0)
                    SetEulerAngle(m_pSlot.eulerAngles + m_SlotEulerOffset);
                if ((m_BindBit & (byte)ESlotBindBit.Scale) != 0)
                    SetScale(m_pSlot.lossyScale);
                if ((m_BindBit & (byte)ESlotBindBit.TerrainY) != 0)
                {
                    if(Framework.Module.ModuleManager.mainModule!=null && Framework.Module.ModuleManager.mainModule is Framework.Module.AFramework)
                    {
                        Vector3 pos = GetTransorm().position;
                        FFloat fTerrainHeight = FFloat.zero;
                        if (TerrainManager.SampleTerrainHeight((Framework.Module.AFramework)Framework.Module.ModuleManager.mainModule, pos, GetTransorm().up, ref fTerrainHeight, 100))
                        {
                            pos.y = fTerrainHeight;
                            SetPosition(pos);
                        }
                    }
                }
             }

            ResetDirty();

            bool bDestroy = false;
            if (m_bStopChecing && !m_bFreezed)
            {
                if (m_fPlayingStatCheck>0 && Time.time > m_fPlayingStatCheck && m_arrSystems != null)
                {
                    bool bAllStop = true;
                    ParSystem par;
                    for (int i = 0; i < m_arrSystems.Length; ++i)
                    {
                        par = m_arrSystems[i];
                        if (par.system == null) continue;
                        if (par.system.isPlaying)
                        {
                            bAllStop = false;
                            break;
                        }
                    }
                    if (bAllStop)
                    {
                        if (Framework.Module.ModuleManager.startUpGame && Framework.Module.ModuleManager.mainModule != null && Application.isPlaying)
                        {
                            RecyleDestroy();
                            bDestroy = true;
                        }
                        m_bEnable = false;
                    }
                }
            }
            if (m_nAutoDestroyFlag !=0 && !bIsDisableCheck && Time.time - m_fCheckTime > 2)
            {
                bool bCheck = m_pOwner == null;
                if(!bCheck)
                {
                    AWorldNode worldNode = m_pOwner as AWorldNode;
                    if (worldNode == null) bCheck = true;
                    else bCheck = false;// worldNode.IsVisible() && worldNode.IsActived();
                }
                if(bCheck)
                {
                    if (Framework.Module.ModuleManager.mainModule != null && CameraKit.cameraController != null)
                    {
                        Vector3 dir = CameraKit.cameraController.GetDir();
                        Vector3 toDir = (m_pTransform.position - CameraKit.cameraController.GetPosition()).normalized;
                        if (Vector3.Dot(dir, toDir) < -0.05f)
                        {
                            RecyleDestroy();
                            bDestroy = true;
                        }
                    }
                }

                m_fCheckTime = Time.time;
            }

            if(!bDestroy && m_fLiftTime > 0 && !m_bFreezed)
            {
                m_fLiftTime -= Time.deltaTime;
                if(m_fLiftTime <=0)
                {
                    //??????
                    //  Framework.Module.ModuleManager.mainModule.OnParticleStop(this);
                    RecyleDestroy();
                    bDestroy = true;
                }
            }
            if(m_pOwner!=null)
            {
                if(m_pOwner is AWorldNode )
                {
                    AWorldNode node = m_pOwner as AWorldNode;
                    if (node.IsDestroy())
                    {
                        if (m_fLiftTime <= 0) m_fLiftTime = 10;
                        OnFreezed(false);
                        m_pOwner = null;
                    }
                    else
                    {
                        bool bFreezed = node.IsFreezed();
                        if (bFreezed != m_bFreezed)
                            OnFreezed(bFreezed);
                    }
                }
  
            }
        }
        //------------------------------------------------------
        public override void Pause()
        {
            if (m_arrSystems != null)
            {
                ParSystem par;
                for (int i = 0; i < m_arrSystems.Length; ++i)
                {
                    par = m_arrSystems[i];
                    if (par.system == null) continue;
                    par.system.Pause();
                }
            }
            if (m_arrDoTweens != null)
            {
                for (int i = 0; i < m_arrDoTweens.Count; ++i)
                {
                    m_arrDoTweens[i].DOPause();
                }
            }
        }
        //------------------------------------------------------
        public override void Resume()
        {
            if (m_arrSystems != null)
            {
                ParSystem par;
                for (int i = 0; i < m_arrSystems.Length; ++i)
                {
                    par = m_arrSystems[i];
                    if (par.system == null) continue;
                    par.system.Play();
                }
            }
            if (m_arrDoTweens != null)
            {
                for (int i = 0; i < m_arrDoTweens.Count; ++i)
                {
                    m_arrDoTweens[i].DOPlay();
                }
            }
        }
        //------------------------------------------------------
        public override void Play()
        {
            SetDirty();
        }
        //------------------------------------------------------
        public override void SetActive()
        {
            base.SetActive();
            SetDirty();
        }
        //------------------------------------------------------
        public override void SetSpeed(float fSpeed)
        {
            if (m_arrSystems != null)
            {
                ParSystem par;
                for (int i = 0; i < m_arrSystems.Length; ++i)
                {
                    par = m_arrSystems[i];
                    if (par.system == null) continue;
                    ParticleSystem.MainModule main = par.system.main;
                    main.startSpeed = fSpeed;
                }
            }
        }
        //------------------------------------------------------
        public void SetAutoDestroy(bool bAuto)
        {
            if (bAuto) m_nAutoDestroyFlag = 1;
            else m_nAutoDestroyFlag = 0;
        }
        //------------------------------------------------------
        public override void OnPoolReady()
        {
            base.OnPoolReady();

            m_pSlot = null;
            m_SlotEulerOffset = Vector3.zero;
            m_SlotOffset = Vector3.zero;
            m_fLiftTime = -1;
            m_nAutoDestroyFlag = 1;
            m_bFreezed = false;
            m_bValidPosCheck = false;
            EnableTrailEmitting(false);
        }
        //------------------------------------------------------
        public override void OnPoolStart()
        {
            base.OnPoolStart();
            m_bFreezed = false;

            if (!Framework.Core.CommonUtility.Equal(GetPosition(), Framework.Core.CommonUtility.INVAILD_POS, 10))
            {
                SetRealStart();
            }
            else
            {
                m_bEnable = false;
                m_bValidPosCheck = true;
            }
        }
        //------------------------------------------------------
        void SetRealStart()
        {
            m_bEnable = true;
            m_bValidPosCheck = false;
            SetDirty();
            m_fPlayingStatCheck = 0;
            EnableTrailEmitting(true);
        }
    }
}
