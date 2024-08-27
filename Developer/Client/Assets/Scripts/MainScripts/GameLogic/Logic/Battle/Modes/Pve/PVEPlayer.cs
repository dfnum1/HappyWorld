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
            m_pPlayer.SetActived(true);
            m_pPlayer.EnableLogic(true);
            m_pPlayer.EnableSkill(true);
            m_pPlayer.SetVisible(true);
            m_pPlayer.EnableRVO(false);
            m_pPlayer.SetAttackGroup(0);
            m_pPlayer.EnableAI(true);
            m_pPlayer.GetActorParameter().SetLevel(10000);
            m_pPlayer.GetActorParameter().AppendHP(100000);
            m_pPlayer.GetActorParameter().AddpendBaseAttr(EAttrType.Attack, 10000);
			
			m_pPlayer.SetFinalPosition(new FVector3(14.0f,0.0f,6.0f));

            Framework.BattlePlus.RunerAgent agent = m_pPlayer.GetActorAgent() as Framework.BattlePlus.RunerAgent;
            if (agent != null)
            {
                agent.EnableAILogic(false);
                agent.EnableAIType(Framework.BattlePlus.EAILogicType.GoTarget, false);
                agent.EnableAIType(Framework.BattlePlus.EAILogicType.Search, false);
            }

            SkillInformation skillSys = m_pPlayer.GetSkill();
            if (skillSys != null)
            {
              //  skillSys.AutoSkill(true);
                skillSys.EnableSkill(true);
            }
        }
        //------------------------------------------------------
        public Vector3 GetPosition()
        {
            if (m_pPlayer != null && !m_pPlayer.IsKilled()) return m_pPlayer.GetPosition();
            return m_ControllerPosition;
        }
        //------------------------------------------------------
        public void DoAttack(GameObject go, params VariablePoolAble[] param)
        {
            if (m_pPlayer == null) return;
            m_pPlayer.GetSkill().ForceDoSkillByTag(100);
            m_pPlayer.GetSkill().Update(0);
        }
        //------------------------------------------------------
        public void OnJoystick(bool bPress, Vector3 vCurPos, Vector3 vLastPos)
        {
            m_bDirectionPressing = bPress;
            if (bPress)
            {
                Vector3 world0 = CameraKit.ScreenRayWorldPos(vLastPos);
                Vector3 world1 = CameraKit.ScreenRayWorldPos(vCurPos);
                m_PressDirection = (world1 - world0).normalized;
                m_ControllerDirection = m_PressDirection;
                m_ControllerDirection.y = 0;
            }
            else m_PressDirection = Vector3.zero;
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

            if (Input.GetKeyDown(KeyCode.J))
            {
                DoAttack(null);
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_pPlayer != null)
                {
                    if(m_pPlayer.StartActionByType(EActionStateType.JumpStart, 0, 1, true, false, true))
                        m_pPlayer.SetJumpSpeed(10);
                }
            }
            if (m_nKeyDir != lastKey)
            {
                if (m_nKeyDir != 0)
                {
                    m_bDirectionPressing = true;
                    m_PressDirection.z = (((m_nKeyDir & (int)Base.EControllerDirFlag.Left) != 0) ? 1f : (((m_nKeyDir & (int)Base.EControllerDirFlag.Right) != 0) ? -1f : 0f));
                    m_PressDirection.y = 0.0f;
                    m_PressDirection.x = (((m_nKeyDir & (int)Base.EControllerDirFlag.Up) != 0) ? 1f : (((m_nKeyDir & (int)Base.EControllerDirFlag.Down) != 0) ? -1f : 0f));
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
          //  if(m_pPlayer.CanDoGroundAction())
            {
                if (m_bDirectionPressing)
                {
                    m_ControllerPosition += m_PressDirection * m_pPlayer.GetRunSpeed() * Time.deltaTime;
                    //m_pPlayer.SetPosition(m_ControllerPosition);
                    m_pPlayer.SetSpeedXZ(m_PressDirection * m_pPlayer.GetRunSpeed());
                    m_pPlayer.SetDirection(m_ControllerDirection);
                    m_pPlayer.StartActionByType(EActionStateType.Run, 0, 1, false, false, true);
                }
                else if (m_pPlayer.CanDoGroundAction())
                {
                    m_pPlayer.SetSpeedXZ(FVector3.zero);
                    m_pPlayer.StartActionByType(EActionStateType.Idle, 0, 1, false, false, true);
                }
            }
//             else
//             {
//               //  m_pPlayer.SetSpeedXZ(FVector3.zero);
//             }
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