/********************************************************************
生成日期:	3:23:2024  20:39
类    名: 	PVEPlayer
作    者:	HappLI
描    述:	PVE 玩家操控对象
*********************************************************************/
using ExternEngine;
using Framework.Core;
using TopGame.Core;
using UnityEngine;

namespace TopGame.Logic
{
    [ModeLogic(EMode.PVE)]
    public class PVEPlayer : AModeLogic
    {
        Player m_pPlayer;

        int m_nKeyDir = 0;
        bool m_bDirectionPressing = false;
        Vector3 m_LastControllerPosition = Vector3.zero;
        Vector3 m_ControllerPosition = Vector3.zero;
        Vector3 m_ControllerDirection = Vector3.forward;
        Vector3 m_PressDirection = Vector3.zero;
        protected override void OnPreStart()
        {
            base.OnPreStart();
            var csvPlayer = Data.DataManager.getInstance().GetCsv<Data.CsvData_Player>();
            var data = csvPlayer.GetData(4030041);
            m_pPlayer = GetWorld().SyncCreateNode<Player>( Framework.Core.EActorType.Player, data);
            m_pPlayer.EnableLogic(true);
        }
        //------------------------------------------------------
        public Vector3 GetPosition()
        {
            if (m_pPlayer != null && !m_pPlayer.IsKilled()) return m_pPlayer.GetPosition();
            return m_ControllerPosition;
        }
        //------------------------------------------------------
        void UpdateKey()
        {
            int lastKey = m_nKeyDir;
            if (Input.GetKeyDown(KeyCode.W)) m_nKeyDir |= (int)Base.EControllerDirFlag.Up;
            else if (Input.GetKeyUp(KeyCode.W)) m_nKeyDir = (m_nKeyDir | (int)Base.EControllerDirFlag.Up) - (int)Base.EControllerDirFlag.Up;

            if (Input.GetKeyDown(KeyCode.S)) m_nKeyDir |= (int)Base.EControllerDirFlag.Down;
            else if (Input.GetKeyUp(KeyCode.S)) m_nKeyDir = (m_nKeyDir | (int)Base.EControllerDirFlag.Down) - (int)Base.EControllerDirFlag.Down;

            if (Input.GetKeyDown(KeyCode.A)) m_nKeyDir |= (int)Base.EControllerDirFlag.Left;
            else if (Input.GetKeyUp(KeyCode.A)) m_nKeyDir = (m_nKeyDir | (int)Base.EControllerDirFlag.Left) - (int)Base.EControllerDirFlag.Left;

            if (Input.GetKeyDown(KeyCode.D)) m_nKeyDir |= (int)Base.EControllerDirFlag.Right;
            else if (Input.GetKeyUp(KeyCode.D)) m_nKeyDir = (m_nKeyDir | (int)Base.EControllerDirFlag.Right) - (int)Base.EControllerDirFlag.Right;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_pPlayer.StartActionByTag(EActionStateType.AttackGround, 100, 0, 1, true);
            }
            if (m_nKeyDir != lastKey)
            {
                if (m_nKeyDir != 0)
                {
                    m_bDirectionPressing = true;
                    m_PressDirection.z = (((m_nKeyDir & (int)Base.EControllerDirFlag.Left) != 0) ? -1f : (((m_nKeyDir & (int)Base.EControllerDirFlag.Right) != 0) ? 1f : 0f));
                    m_PressDirection.y = 0.0f;
                    m_PressDirection.x = (((m_nKeyDir & (int)Base.EControllerDirFlag.Up) != 0) ? -1f : (((m_nKeyDir & (int)Base.EControllerDirFlag.Down) != 0) ? 1f : 0f));
                    m_PressDirection.Normalize();
                    m_ControllerDirection = m_PressDirection;
                }
                else
                {
                    m_bDirectionPressing = false;
                }
            }
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            base.InnerUpdate(fFrame);

#if UNITY_EDITOR
            if (CameraKit.IsEditorMode)
                return;
#endif

            UpdateKey();

            if (m_pPlayer == null)
                return;
            if(m_pPlayer.CanDoGroundAction())
            {
                if (m_bDirectionPressing)
                {
                    m_ControllerPosition += m_PressDirection * m_pPlayer.GetRunSpeed() * Time.deltaTime;
                    //m_pPlayer.SetPosition(m_ControllerPosition);
                    m_pPlayer.SetSpeedXZ(m_PressDirection * m_pPlayer.GetRunSpeed());
                    m_pPlayer.SetDirection(m_ControllerDirection);
                    m_pPlayer.StartActionByType(EActionStateType.Run, 0, 1, false, false, true);
                }
                else
                {
                    m_pPlayer.SetSpeedXZ(FVector3.zero);
                    m_pPlayer.StartActionByType(EActionStateType.Idle, 0, 1, false, false, true);
                }
            }
        }
        //------------------------------------------------------
        protected override void OnClear()
        {
            base.OnClear();
            m_LastControllerPosition = Vector3.zero;
            m_ControllerPosition = Vector3.zero;
            m_ControllerDirection = Vector3.forward;
            m_PressDirection = Vector3.zero;
        }
    }
}