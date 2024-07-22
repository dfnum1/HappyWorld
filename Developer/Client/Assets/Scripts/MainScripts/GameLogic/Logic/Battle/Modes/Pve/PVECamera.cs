/********************************************************************
生成日期:	3:23:2024  20:37
类    名: 	PVECamera
作    者:	HappLI
描    述:	pve 相机逻辑
*********************************************************************/
using UnityEngine;

namespace TopGame.Logic
{
    [ModeLogic(EMode.PVE)]
    public class PVECamera : BattleCameraLogic
    {
        //------------------------------------------------------
        public override ushort GetOverrideCameraID(EStateType state, ushort defaultID = 0)
        {
            return 100;
        }
        //------------------------------------------------------
        protected override Vector3 GetPosition()
        {
            return GetModeLogic<PVEPlayer>().GetPosition();
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            if (m_BattleCameraMode == null)
                m_BattleCameraMode = Core.CameraKit.GetCurrentMode() as Core.BattleCameraMode;
            if (m_BattleCameraMode == null) return;
            m_LookAtPosition = GetPosition();

            m_BattleCameraMode.SetFollowLookAtPosition(m_LookAtPosition);

            m_BattleCameraMode.SetFollowSpeed(m_CameraFollowSpeed);

        }
    }
}