#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ActorDebugTransorm
作    者:	HappLI
描    述:	调试数据
*********************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using Framework.Core;

namespace TopGame.Logic
{
    public class ActorDebugTransormEditor : IWorldNodeDebugerEditor
    {
        bool m_bExpandSkill = false;
        bool m_bExpandExternAttr = false;
        bool m_bExpandAttrDebuger = false;
        System.Collections.Generic.List<string> m_vActions = new System.Collections.Generic.List<string>();
        System.Collections.Generic.List<ActionState> m_vActionStates = new System.Collections.Generic.List<ActionState>();
        ActionState m_pDoAction = null;
        uint m_nAddBuff = 0;
        uint m_nAddBufflevel = 1;
        float m_fInvincibleTime = 0;

        EElementType m_nElementFlag = 0;

        Dictionary<EBuffAttrType, string> BuffNames = new Dictionary<EBuffAttrType, string>();

        public void OnDisable(AWorldNode pNode) { }
        public void OnEnable(AWorldNode pNode)
        {
            Actor pActor = pNode as Actor;
            if (pActor == null) return;
            m_nElementFlag = 0;// Logic.BattleUtil.GetFirstElementType(pActor.GetElementFlags());
            if (pActor == null || pActor.GetActionStateGraph() == null) return;
            foreach (var db in pActor.GetActionStateGraph().GetActionStateMap())
            {
                m_vActions.Add(db.Value.GetCore().name);
                m_vActionStates.Add(db.Value);
            }
            foreach (System.Enum v in System.Enum.GetValues(typeof(EBuffAttrType)))
            {
                System.Reflection.FieldInfo fi = typeof(EBuffAttrType).GetField(v.ToString());
                string strTemName = v.ToString();
                if (fi != null && fi.IsDefined(typeof(Framework.Plugin.DisableGUIAttribute)))
                    continue;
                if (fi != null && fi.IsDefined(typeof(Framework.Plugin.PluginDisplayAttribute)))
                {
                    strTemName = fi.GetCustomAttribute<Framework.Plugin.PluginDisplayAttribute>().displayName;
                }
                BuffNames[(EBuffAttrType)v] = strTemName;
            }
        }
        public void OnInspectorGUI(AWorldNode pNode)
        {
            Vector3 pos = pNode.GetPosition();
            if (GUILayout.Button("自杀"))
            {
                pNode.SetFlag(EWorldNodeFlag.Killed, true);
                if(pNode is Actor)
                {
                    Actor pTarget = pNode as Actor;
                    pTarget.GetActorParameter().GetRuntimeParam().hp = 0;
                    AState curState = StateFactory.CurrentState();
                    if (curState != null && curState.GetCurrentActor() != null && Framework.Core.AttackFrameUtil.CU_IsAttack(curState.GetCurrentActor(), pTarget))
                    {
                        curState.GetBattleLogic<Framework.BattlePlus.BattleStats>().OnActorHit(null, curState.GetCurrentActor(), pTarget, EDamageType.Count, 0);
                    }
                }

            }

            EditorGUILayout.LabelField("基础数据", pNode.ToString());
            EditorGUILayout.Vector3Field("位置", pos);
            EditorGUILayout.LabelField("速度", pNode.GetSpeed().ToString());
            EditorGUILayout.LabelField("逻辑静止", pNode.IsFreezed().ToString());
            EditorGUILayout.LabelField("激活", pNode.IsActived().ToString());
            EditorGUILayout.LabelField("显示", pNode.IsVisible().ToString());
            InspectorActor(pNode as Actor);
            InspectorObsElement(pNode as ObsElement);
        }
        //------------------------------------------------------
        void InspectorObsElement(ObsElement pObs)
        {
            if (pObs == null) return;
            EditorGUILayout.FloatField("可视半径", pObs.visibleRadius);
            EditorGUILayout.FloatField("激活半径", pObs.activeRadius);
            EditorGUILayout.LabelField("terrainType", pObs.GetTerrainHeightType().ToString());
            EditorGUILayout.LabelField("moveType", pObs.GetMoveType().ToString());
        }
        //------------------------------------------------------
        void InspectorMonster(Monster pMonster)
        {
            if (pMonster == null) return;
            EditorGUILayout.FloatField("可视半径", pMonster.GetVisibleRadius());
            EditorGUILayout.FloatField("激活半径", pMonster.GetActiveRadius());
            EditorGUILayout.LabelField("terrainType", pMonster.GetTerrainHeightType().ToString());
            EditorGUILayout.LabelField("moveType", pMonster.GetMoveType().ToString());
        }
        //------------------------------------------------------
        void InspectorActor(Actor pActor)
        {
            if (pActor == null) return;
            InspectorMonster(pActor as Monster);
            EditorGUILayout.LabelField("站位排", pActor.GetActorParameter().GetTeamRow().ToString());
            if (pActor.GetActionStateGraph() == null)
            {
                EditorGUILayout.HelpBox("动作脚本缺失", MessageType.Error);
            }
            EditorGUILayout.LabelField("技能开关", (pActor.GetSkill()!=null && pActor.GetSkill().IsCanDriveSkill(null))? "开" :"关");
            EditorGUILayout.LabelField("攻击组", pActor.GetAttackGroup().ToString());
            EditorGUILayout.LabelField("Level", pActor.GetActorParameter().GetLevel().ToString());
            EditorGUILayout.LabelField("Combo", pActor.GetActorParameter().GetRuntimeParam().combo.ToString());
            EditorGUILayout.LabelField("摩擦力", pActor.GetFraction().ToString());
            EditorGUILayout.LabelField("重力", pActor.GetGravity().ToString());
            EditorGUILayout.LabelField("GroundType", pActor.GetGroundType().ToString());
            EditorGUILayout.LabelField("战斗力", pActor.GetActorParameter().GetFightPower().ToString());
            EditorGUILayout.LabelField("队伍索引", pActor.GetActorParameter().GetTeamIndex().ToString());
            EditorGUILayout.LabelField("队伍所在排", pActor.GetActorParameter().GetTeamRow().ToString());
            EditorGUILayout.LabelField("阵营", pActor.GetActorParameter().GetCamp().ToString());
            EditorGUILayout.LabelField("移速", pActor.GetRunSpeed().ToString());
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("攻速", pActor.GetAttackSpeed().ToString());
            if (pActor.GetAttackSpeed() > 2) EditorGUILayout.HelpBox("攻速>2 将取2计算", MessageType.Warning);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            pActor.EnableAI(EditorGUILayout.Toggle("AI开关", pActor.IsEnableAI()));
            if(pActor.GetAI()!=null && GUILayout.Button("调试"))
            {
                Framework.Plugin.AI.AIEditor.Editor(pActor.GetAI().aiData);
            }
            GUILayout.EndHorizontal();
            Vector3 pos = pActor.GetPosition();
            if (GUILayout.Button("属性刷新"))
            {
                pActor.GetActorParameter().UpdataAttr();
                pActor.GetActorParameter().ResetRunTimeParameter(false);
            }
            ActorAgent pAgent = pActor.GetActorAgent();

            if( pActor.IsObjected())
            {
                Animator pAnimator = pActor.GetObjectAble().GetBehaviour<Animator>();
                if(pAnimator!=null)
                    EditorGUILayout.LabelField("表现层动作播放速度", pAnimator.speed.ToString());
            }

            EElementType nElementFlag = (EElementType)EditorGUILayout.EnumPopup("主元素", m_nElementFlag);
            if (nElementFlag != m_nElementFlag)
            {
                pActor.SetElementFlags((uint)(1 << (int)nElementFlag));
                m_nElementFlag = nElementFlag;
            }

            GUILayout.BeginHorizontal();
            m_fInvincibleTime = EditorGUILayout.FloatField("无敌剩余" + pActor.GetInvincibleDuration().ToFloat().ToString("F0") + "s", m_fInvincibleTime);
            if (GUILayout.Button("设置"))
            {
                pActor.SetInvincibleByDuration(m_fInvincibleTime, true);
            }
            GUILayout.EndHorizontal();
            ActorParameter param = pActor.GetActorParameter();

            EditorGUILayout.BeginHorizontal();
            int sel = EditorGUILayout.Popup("播放动作", m_vActionStates.IndexOf(m_pDoAction), m_vActions.ToArray());
            if (sel >= 0 && sel < m_vActionStates.Count)
            {
                m_pDoAction = m_vActionStates[sel];
            }
            if (m_pDoAction != null && GUILayout.Button("播放"))
            {
                pActor.StartActionState(m_pDoAction, 0, 1, true, false, true);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            m_nAddBuff = (uint)EditorGUILayout.IntField("测试buff", (int)m_nAddBuff);
            m_nAddBufflevel = (uint)EditorGUILayout.IntField("Lv",(int)m_nAddBufflevel);
            if (sel >= 0 && sel < m_vActionStates.Count)
            {
                m_pDoAction = m_vActionStates[sel];
            }
            if (m_nAddBuff > 0 && GUILayout.Button("添加"))
            {
                pActor.BeginBuffer(m_nAddBuff, m_nAddBufflevel, pActor);
            }
            EditorGUILayout.EndHorizontal();
            m_bExpandSkill = EditorGUILayout.Foldout(m_bExpandSkill, "技能列表");
            if (m_bExpandSkill && param.GetSkill() != null)
            {
                EditorGUILayout.LabelField("已学技能:");
                BattleSkillInformations battleSkill = param.GetSkill() as BattleSkillInformations;
                if (battleSkill != null)
                {
                    EditorGUILayout.LabelField("技能公共CD:" + Mathf.Max(0, battleSkill.GetGlobalCD()));
                }
                GlobalBattleSkillInfomation globalCmd = pActor.GetGameModule().Get<GlobalBattleSkillInfomation>(false);
                if(globalCmd!=null)
                    EditorGUILayout.LabelField("大招公共CD:" + Mathf.Max(0, globalCmd.GetCmdCD(pActor.GetAttackGroup())));
                foreach (var db in param.GetSkill().GetLearned())
                {
                    string skillName = db.configId.ToString();
                    if (db.skillData != null)
                        skillName = (db.skillData as Data.CsvData_Skill.SkillData).Text_name_data?.textCN + " Lv." + (db.skillData as Data.CsvData_Skill.SkillData).level;
                    string lockTarget = "";
                    System.Collections.Generic.List<AWorldNode> vLocks = param.GetSkill().GetCurLockTargets(false);
                    if (vLocks != null)
                    {
                        lockTarget += "Lock[";
                        for (int i = 0; i < vLocks.Count; ++i)
                        {
                            lockTarget += vLocks[i].GetActorType() + "-" + vLocks[i].GetConfigID() + ",";
                        }
                        lockTarget += "]";
                    }
                    string label = db.nGuid.ToString() + " [" + skillName + "]" + lockTarget;
                    if (!string.IsNullOrEmpty(db.gotLabel))
                        label += "  来源:" + db.gotLabel;
                    EditorGUILayout.LabelField(label);
                }
                EditorGUILayout.LabelField("拥有的技能:");
                foreach (var db in param.GetSkill().GetSkills())
                {
                    string skillName = db.Key.ToString();
                    if (db.Value.skillData != null)
                    {
                        skillName = (db.Value.skillData as Data.CsvData_Skill.SkillData).Text_name_data?.textCN + " Lv." + (db.Value.skillData as Data.CsvData_Skill.SkillData).level;
                        skillName += " UnlockLv." + (db.Value.skillData as Data.CsvData_Skill.SkillData).unLock;
                    }
                    if(!string.IsNullOrEmpty(db.Value.gotLabel))
                        skillName += "  来源:" + db.Value.gotLabel;

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(db.Value.nGuid.ToString() + " [" + skillName + "]" + "   CD:" + Mathf.Max(0, db.Value.runingCD).ToString());
                    if (GUILayout.Button("强制释放"))
                    {
                        param.GetSkill().ForceDoSkill(db.Value.nGuid);
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.LabelField("当前释放技能:");
                if (param.GetSkill().GetCurrentSkill() != null)
                {
                    Skill skill = param.GetSkill().GetCurrentSkill();
                    float duration = 0;
                    if (pActor.GetCurrentActionState() != null && pActor.GetCurrentActionState().GetCore().tag == skill.Tag)
                        duration = pActor.GetCurrentActionState().GetDelta();
                    string skillName = "";
                    if (skill.skillData != null)
                        skillName = (skill.skillData as Data.CsvData_Skill.SkillData).Text_name_data?.textCN + " Lv." + (skill.skillData as Data.CsvData_Skill.SkillData);
                    EditorGUILayout.LabelField(skill.nGuid.ToString() + " [" + skillName + "]    " + duration);
                    EditorGUILayout.TextArea(skill.GetLockParamToString()); ;
                }
                if (pActor.GetCurrentActionState() != null)
                {
                    EditorGUILayout.LabelField("当前动作：" + pActor.GetCurrentActionState().GetCore().name + ":" + pActor.GetCurrentActionState().GetDelta());
                }
                if (pActor.GetCurrentOverrideActionState() != null)
                {
                    EditorGUILayout.LabelField("当前分离动作：" + pActor.GetCurrentOverrideActionState().GetCore().name + ":" + pActor.GetCurrentOverrideActionState().GetDelta());
                }
                EditorGUILayout.LabelField("释放队列:");
                foreach (var db in param.GetSkill().GetCommands())
                {
                    string skillName = "";
                    if (db.skillData != null)
                        skillName = (db.skillData as Data.CsvData_Skill.SkillData).Text_name_data?.textCN + " Lv." + (db.skillData as Data.CsvData_Skill.SkillData).level;
                    EditorGUILayout.LabelField(db.nGuid.ToString() + " [" + skillName + "]");
                }
            }

            m_bExpandSkill = EditorGUILayout.Foldout(m_bExpandSkill, "属性详情");

            if (m_bExpandSkill)
            {
                //! 运行时属性
                EditorGUILayout.LabelField("runtime hp:" + param.GetRuntimeParam().hp.ToString());
                EditorGUILayout.LabelField("runtime maxhp:" + param.GetRuntimeParam().max_hp.ToString());
                EditorGUILayout.LabelField("runtime sp:" + param.GetRuntimeParam().sp.ToString());
                EditorGUILayout.LabelField("runtime maxsp:" + param.GetRuntimeParam().max_sp.ToString());
                EditorGUILayout.LabelField("runtime 护盾:" + param.GetRuntimeParam().shield.ToString() + "护盾时长:" + param.GetShieldedTime().ToString());
                EditorGUILayout.LabelField("血转盾:" + param.GetRuntimeParam().shield_extern.ToString());

                EditorGUILayout.LabelField("攻击回能:" + param.GetAttackSpcCoefficient().ToString());
                EditorGUILayout.LabelField("受击回能:" + param.GetHittedSpcCoefficient().ToString());
                EditorGUILayout.LabelField("死亡击杀者回能:" + param.GetAttackerKillSpecValue().ToString());

                SvrData.ISvrHero svrHero = pActor.GetActorParameter().GetSvrData() as SvrData.ISvrHero;
                if(svrHero!=null)
                {
                    EditorGUILayout.LabelField("附加等级:" + svrHero.GetExternLevel());
                }

                if (pActor.GetActorAgent() != null && pActor.GetActorAgent().GetBufferData() != null)
                {
                    foreach (var db in pActor.GetActorAgent().GetBufferData())
                    {
                        var buffData = db.Value.data as Data.CsvData_Buff.BuffData;
                        if (buffData == null) continue;
                        EditorGUILayout.LabelField("Buff:" + buffData.id + "  group:" + db.Key.ToString() +
                            "  lv:" + db.Value.level + "  layer:" + db.Value.GetLayer() + " reamineTime:" +
                            (db.Value.running_duration - db.Value.running_delta).ToString() + 
                            "  tickCnt:" + db.Value.tick_count + "  actived:" + db.Value.isActived().ToString());
                    }
                }

                EditorGUILayout.LabelField("属性：");
                for (EAttrType i = EAttrType.MaxHp; i < EAttrType.Num; ++i)
                {
                    EditorGUILayout.LabelField(i.ToString() + "->base:" + param.GetBaseAttr(i).ToString() + "    final:" + param.GetAttr(i));
                }
                EditorGUILayout.LabelField("Buff属性：");
                for (EBuffAttrType i = EBuffAttrType.MaxHp; i < EBuffAttrType.Count; ++i)
                {
                    string text = i.ToString() + "->value:" + param.GetBufferValueCache()[(int)i] + "   factor:" + param.GetBufferRateCache()[(int)i]
                        + " | global::value:" + param.GetGlobalBufferValueCache()[(int)i] + "   factor:" + param.GetGlobalBufferRateCache()[(int)i];
                    EditorGUILayout.LabelField(text);
                }

                m_bExpandExternAttr = EditorGUILayout.Foldout(m_bExpandExternAttr, "扩展属性");
                if (m_bExpandExternAttr)
                {
                    ExternAttr[] externAttr = param.GetAttr().arExternAttr;
                    for (int i = 0; i < externAttr.Length; ++i)
                    {
                        if (!BuffNames.ContainsKey((EBuffAttrType)i)) continue;
                        EditorGUILayout.LabelField("extern " + BuffNames[(EBuffAttrType)i] + ":" + externAttr[i].value + "   rate:" + externAttr[i].rate);
                        EditorGUILayout.LabelField("extern元素" + BuffNames[(EBuffAttrType)i] + ":" + GetElementArrayToString(externAttr[i].element));
                    }
                }
            }
            m_bExpandAttrDebuger = EditorGUILayout.Foldout(m_bExpandAttrDebuger, "属性计算Debug");
            if (m_bExpandAttrDebuger)
            {
                param.attrDebuger.Draw();
            }
        }
        //------------------------------------------------------
        string GetElementArrayToString(ExternEngine.FVector2[] eleValues)
        {
            string strTemp = "";
            if (eleValues.Length > (int)EElementType.Water) strTemp += "水:" + eleValues[(int)EElementType.Water].x + "  " + eleValues[(int)EElementType.Water].y;
            if (eleValues.Length > (int)EElementType.Fire) strTemp += "火:" + eleValues[(int)EElementType.Fire].x + "  " + eleValues[(int)EElementType.Fire].y;
            if (eleValues.Length > (int)EElementType.Wood) strTemp += "木:" + eleValues[(int)EElementType.Wood].x + "  " + eleValues[(int)EElementType.Wood].y;
            if (eleValues.Length > (int)EElementType.Soil) strTemp += "土:" + eleValues[(int)EElementType.Soil].x + "  " + eleValues[(int)EElementType.Soil].y;
            if (eleValues.Length > (int)EElementType.Thunder) strTemp += "电:" + eleValues[(int)EElementType.Thunder].x + "  " + eleValues[(int)EElementType.Thunder].y;
            return strTemp;
        }
        //------------------------------------------------------
        public void OnSceneGUI(AWorldNode pNode)
        {
            float activeRadius = 0;
            Actor pActor = pNode as Actor;
            if (pActor != null)
            {
                //attack distance
                Color color = Handles.color;
                Handles.color = Color.red;
                Handles.CircleHandleCap(0, pActor.GetPosition()+Vector3.up*0.1f, Quaternion.Euler(90, 0, 0), pActor.GetMaxAttackDistance(), EventType.Repaint);
                Handles.CircleHandleCap(0, pActor.GetPosition() + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0), pActor.GetMinAttackDistance(), EventType.Repaint);
                Handles.color = color;

                //target distance
                Handles.color = Color.green;
                Handles.CircleHandleCap(0, pActor.GetPosition() + Vector3.up * 0.1f, Quaternion.Euler(90,0,0), pActor.GetMaxTargetDistance(), EventType.Repaint);
                Handles.color = color;

                ActorAgent pActorAgent = pActor.GetActorAgent();
                if(pActorAgent!=null)
                {
                    Vector3Track track = pActorAgent.GetRuningPath();
                    if(track!=null)
                    {
                        track.DrawDebug();
                    }
                }

                if(pActor is Monster)
                {
                    activeRadius = (pActor as Monster).GetActiveRadius();
                }
            }
            if (pNode is ObsElement)
            {
                activeRadius = (pNode as ObsElement).activeRadius;
            }
            if(activeRadius>0 && !pNode.IsActived())
            {
                Color color = Handles.color;
                Handles.color = Color.white;
                Handles.CircleHandleCap(0, pNode.GetPosition(), Quaternion.Euler(pNode.GetEulerAngle()), activeRadius, EventType.Repaint);
                Handles.color = color;
            }
        }
    }
}
#endif
