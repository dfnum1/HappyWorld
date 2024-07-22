/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	CameraShake
作    者:	HappLI
描    述:	相机路径动画
*********************************************************************/
using System;
using System.Collections.Generic;
using Framework.Core;
using Framework.Module;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TopGame.Core
{
    public class CameraAnimPath : ACameraEffect, IPlayableCallback
    {
        public enum EATEventType
        {
            AnimPathEnd = 4000,
            AnimPathUpdate = 4001,
        }
        private enum EFlag
        {
            Position = 1<<0,
            EulerAngle = 1<<1,
            Lookat = 1<<2,
            Up = 1<<3,
            Fov = 1<<4,
            Follow = 1<<5,
        }
        GameObject m_pTrigger = null;
        int m_nPathID = 0;
        bool m_bForcePlay = false;
        IPlayableBase m_pPlayAble = null; 
        private bool m_bEnabled = false;
        private bool m_bAbs = true;

        private byte m_nFlag = 0;
        private Vector3 m_CurPosition = Vector3.zero;
        private Vector3 m_FollowPosition = Vector3.zero;
        private Vector3 m_CurEulerAngle = Vector3.zero;
        private Vector3 m_CurUp = Vector3.up;
        private Vector3 m_CurLookAt = Vector3.zero;
        private float m_CurFov = 0;

        private Vector3 m_TriggerPosition = Vector3.zero;
        //------------------------------------------------------
        public void SetPathID(int nID, bool bAbs = true, GameObject trigger = null, bool bForce = true)
        {
            m_nPathID = nID;
            m_bAbs = bAbs;
            m_pTrigger = trigger;
            m_bForcePlay = bForce;
        }
        //------------------------------------------------------
        public void SetTriggerPosition(Vector3 pos)
        {
            m_TriggerPosition = pos;
        }
        //------------------------------------------------------
        public override bool CanDo()
        {
            return m_nPathID >0 && m_pPlayAble != null && !m_pPlayAble.isOver();
        }
        //------------------------------------------------------
        public override void Start(EType type)
        {
            if (m_nPathID <= 0)
                return;
            base.Start(type);

            if (m_pPlayAble != null)
                GameInstance.getInstance().animPather.Stop(m_pPlayAble.GetGuid());
            m_pPlayAble = null;

            m_nFlag = 0;
            m_CurPosition = Vector3.zero;
            m_CurEulerAngle = Vector3.zero;
            m_FollowPosition = Vector3.zero;
            m_CurLookAt = Vector3.zero;
            m_CurUp = Vector3.up;
            m_CurFov = 0;

            Transform target = CameraKit.cameraController!=null? CameraKit.cameraController.GetTransform():null;
            if (target == null) return;
            if (m_pTrigger)
            {
                m_pPlayAble = GameInstance.getInstance().animPather.PlayAnimPathImmediate(m_nPathID, m_pTrigger, target.gameObject, null, m_bForcePlay);
            }
            else
            {
                m_pPlayAble = GameInstance.getInstance().animPather.PlayAnimPathImmediate(m_nPathID, null, target.gameObject, null, m_bForcePlay);
            }
            if (m_pPlayAble!=null)
            {
                m_pPlayAble.Register(this);
                m_pPlayAble.SetOffsetPosition(m_TriggerPosition);
            }
        }
        //------------------------------------------------------
        public override void Stop()
        {
            if (m_pPlayAble == null || GameInstance.getInstance() == null)
                return;
            GameInstance.getInstance().animPather.Stop(m_pPlayAble.GetGuid());
            m_pPlayAble = null;
            m_nPathID = 0;
            m_bForcePlay = false;
            m_TriggerPosition = Vector3.zero;
            m_nFlag = 0;
            m_CurPosition = Vector3.zero;
            m_CurEulerAngle = Vector3.zero;
            m_FollowPosition = Vector3.zero;
            m_CurLookAt = Vector3.zero;
            m_CurUp = Vector3.up;
            m_CurFov = 0;
            m_pTrigger = null;
            base.Stop();
        }
        //------------------------------------------------------
        public bool IsPlaying()
        {
            if (m_pPlayAble == null) return false;
            return !m_pPlayAble.isOver();
        }
        //------------------------------------------------------
        public IPlayableBase playable
        {
            get { return m_pPlayAble; }
        }
        //------------------------------------------------------
        public override bool Update(ICameraController pController, float fFrame)
        {
            if (m_pPlayAble == null) return false;
            if(!m_pPlayAble.isOver())
            {
                Framework.Plugin.AT.AgentTreeManager.getInstance().ExecuteEvent((ushort)EATEventType.AnimPathUpdate, 0, m_pPlayAble);
            }
            else
            {
                m_nPathID = 0;
            }
            if((m_pPlayAble.getUseEnd()&(int)EPathUseEndType.SyncMode)!=0)
            {
                if(pController.GetCurrentMode()!=null)
                {
                    pController.GetCurrentMode().SetCurrentTrans(m_pPlayAble.GetPosition());
                    pController.GetCurrentMode().SetCurrentEulerAngle(m_pPlayAble.GetEulerAngle());
                }
            }
            return true;
        }
        //------------------------------------------------------
        public Camera GetCamera()
        {
            if (m_pPlayAble == null) return null;
            return m_pPlayAble.GetCamera();
        }
        //------------------------------------------------------
        public bool OnTimelineCallback(IPlayableBase plabable, EPlayableCallbackType type, Variable3 value)
        {
            switch(type)
            {
                case EPlayableCallbackType.Position:
                    {
                        m_nFlag |= (byte)EFlag.Position;
                        m_CurPosition = value.ToVector3();
                    }
                    break;
                case EPlayableCallbackType.EuleraAngle:
                    {
                        m_nFlag |= (byte)EFlag.EulerAngle;
                        m_CurEulerAngle = value.ToVector3();
                    }
                    break;
                case EPlayableCallbackType.Lookat:
                    {
                        m_nFlag |= (byte)EFlag.Lookat;
                        m_CurLookAt = value.ToVector3();
                    }
                    break;
                case EPlayableCallbackType.VecUp:
                    {
                        m_nFlag |= (byte)EFlag.Up;
                        m_CurUp = value.ToVector3();
                    }
                    break;
                case EPlayableCallbackType.FOV:
                    {
                        m_nFlag |= (byte)EFlag.Fov;
                        m_CurFov = value.floatVal0;
                    }
                    break;
                case EPlayableCallbackType.FollowPosition:
                    {
                        m_nFlag |= (byte)EFlag.Follow;
                        m_FollowPosition = value.ToVector3();
                    }
                    break;
                case EPlayableCallbackType.End:
                    {
                        OnEnd();
                    }
                    break;
            }
            return true;
        }
        //------------------------------------------------------
        public void OnEnd()
        {
            
            if(m_pPlayAble != null)
            {
                Framework.Plugin.AT.AgentTreeManager.getInstance().ExecuteEvent((ushort)EATEventType.AnimPathEnd, m_pPlayAble.GetGuid(), m_pPlayAble);
                if (m_pPlayAble.getUseEnd() != 0)
                {
                    CameraMode mode = CameraKit.cameraController!=null? CameraKit.cameraController.GetCurrentMode(): null;
                    if (mode == null) return;

                    mode.ResetLockOffsets();

                    if ((m_pPlayAble.getUseEnd() & (int)EPathUseEndType.SyncMode) != 0)
                    {
                        mode.SetCurrentTrans(m_pPlayAble.GetPosition());
                        mode.SetCurrentEulerAngle(m_pPlayAble.GetEulerAngle());
                        return;
                    }
                    if ( (m_pPlayAble.getUseEnd() & (ushort)(EPathUseEndType.Fov)) !=0 )
                    {
                        if ((m_nFlag & (int)EFlag.Fov) != 0)
                            mode.SetCurrentFov(m_CurFov);
                    }

                    Vector3 curDir = Framework.Core.CommonUtility.EulersAngleToDirection(m_CurEulerAngle);
                    Vector3 curLookat = Framework.Core.CommonUtility.RayHitPos(m_CurPosition, curDir);

                    if ((m_pPlayAble.getUseEnd() & (ushort)(EPathUseEndType.EulerAngle)) != 0)
                    {
                        mode.SetCurrentUp(m_CurUp);
                        mode.SetCurrentEulerAngle(m_CurEulerAngle);
                    }
                    if ((m_pPlayAble.getUseEnd() & (ushort)(EPathUseEndType.Position)) != 0)
                    {
                        mode.SetCurrentLookAtOffset(curLookat - mode.GetFollowLookAtPosition());
                        mode.SetFollowDistance(Vector3.Distance(curLookat, m_CurPosition), true);
                    }
                    if (Framework.Module.ModuleManager.mainModule != null && (m_pPlayAble.getUseEnd() & (ushort)(EPathUseEndType.ReplaceRunPosition)) != 0)
                    {
                        mode.SetCurrentLookAtOffset(curLookat - m_FollowPosition);
                        AFrameworkModule framework = Framework.Module.ModuleManager.GetMainFramework<AFrameworkModule>();
                        if(framework!=null) framework.SyncPlayerPosition(m_FollowPosition);
                        mode.SetFollowLookAtPosition(m_FollowPosition);
                        mode.SetFollowDistance(Vector3.Distance(curLookat, m_CurPosition), true);
                        mode.Start();
                    }
                    if (Framework.Module.ModuleManager.mainModule != null && (m_pPlayAble.getUseEnd() & (ushort)(EPathUseEndType.UseRunPosition)) != 0)
                    {
                        mode.SetCurrentLookAtOffset(curLookat - m_FollowPosition);
                        mode.SetFollowDistance(Vector3.Distance(curLookat, m_CurPosition), true);
                        mode.Start();
                    }
                }
                m_pPlayAble = null;
                m_nPathID = 0;
            }

        }
    }
}