/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	ICameraEffect
作    者:	HappLI
描    述:	相机效果
*********************************************************************/
using UnityEngine;

namespace TopGame.Core
{
    public interface ICameraEffectCallback
    {
        void OnEffectPosition(Vector3 pos);
        void OnEffectEulerAngle(Vector3 eulerAngle);
        void OnEffectLookAt(Vector3 lookAt);
        void OnEffectFov(float fov);
        void OnFollowPosition(Vector3 pos);
    }

    public abstract class ACameraEffect
    {
        public enum EType
        {
            ModeSet,
            LockSet,
            Offset,
            Direct,
        }
        protected bool m_bFov = false;
        protected EType m_Type = EType.Offset;
        protected System.Collections.Generic.List<ICameraEffectCallback> m_vCallback = null;
        public virtual void Start(EType type) { m_Type = type; }
        public virtual void Stop()
        {
            m_bFov = false;
            m_Type = EType.Offset;
        }
        //------------------------------------------------------
        public EType GetModeType()
        {
            return m_Type;
        }
        //------------------------------------------------------
        public void EnableFov(bool bFov)
        {
            m_bFov = bFov;
        }
        //------------------------------------------------------
        public virtual bool CanDo() { return false; }
        //------------------------------------------------------
        public virtual bool Update(ICameraController pController, float fFrame) { return false; }
        //------------------------------------------------------
        public void Register(ICameraEffectCallback callback)
        {
            if (m_vCallback == null) m_vCallback = new System.Collections.Generic.List<ICameraEffectCallback>(2);
            if (m_vCallback.Contains(callback)) return;
            m_vCallback.Add(callback);
        }
        //------------------------------------------------------
        public void UnRegister(ICameraEffectCallback callback)
        {
            if (m_vCallback == null) return;
            m_vCallback.Remove(callback);
        }
    }
}