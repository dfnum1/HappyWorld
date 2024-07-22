/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	GameInstance
作    者:	HappLI
描    述:	游戏主线程
*********************************************************************/
using ExternEngine;
using Framework.BattlePlus;
using Framework.Core;
using Framework.Data;
using Framework.Logic;
using System.Collections.Generic;
using TopGame.Core;
using TopGame.Data;
using TopGame.Logic;
using UnityEngine;

namespace TopGame
{
    public partial class GameInstance
    {
#if UNITY_EDITOR
        float m_nLastWorldNodeMallocTime = 0;
        int m_nWorldNodeMallocCount = 0;
#endif
        //------------------------------------------------------
        protected override AWorldNode ExcudeWorldNodeMalloc(EActorType type)
        {
#if UNITY_EDITOR
            if (Mathf.Min(m_nLastWorldNodeMallocTime - Time.time) <= 0.1f)
            {
                m_nWorldNodeMallocCount++;
                m_nLastWorldNodeMallocTime = Time.time;
                if (m_nWorldNodeMallocCount >= 500)
                {
                    m_nWorldNodeMallocCount = 0;
                    Debug.Log("及短时间内创建大量单位:" + m_nWorldNodeMallocCount);
               //     UnityEditor.EditorUtility.DisplayDialog("提示", "一直在创建世界单位，可能会引发内存爆炸，检测代码是否有逻辑问题！", "好的");
                }
            }
            else m_nWorldNodeMallocCount = 0;
#endif
            switch (type)
            {
                case EActorType.Player: return new Logic.Player(this);
                case EActorType.Mecha: return new Logic.Mecha(this);
                case EActorType.Monster: return new Logic.Monster(this);
                case EActorType.Summon: return new Logic.Summon(this);
                case EActorType.Projectile: return new Core.Projectile(this);
                case EActorType.Element: return new Logic.ObsElement(this);
                case EActorType.Wingman: return new Logic.Wingman(this);
#if !USE_SERVER
                case EActorType.Scene: return new Logic.WorldSceneNode(this);
                case EActorType.PreviewActor: return new Logic.PreviewActor(this);
#endif
            }
            return null;
        }
        //------------------------------------------------------
        public override void OnWorldNodeStatus(AWorldNode pNode, EWorldNodeStatus status, VariablePoolAble userVariable = null)
        {
#if !USE_SERVER
            if (status == EWorldNodeStatus.Killed)
            {
                if (m_pAgentTree != null)
                {
                    Actor pActor = pNode as Actor;
                    m_pAgentTree.ExecuteEvent((ushort)Framework.Base.EBaseATEventType.OnActorKilled, 1, pActor);
                }
                //dynamicTexter.OnKillActor(pNode as Actor);
                return;
            }
#endif
            if (status != EWorldNodeStatus.Create) return;

            ActionStateGraph pStateGraph = null;

            Framework.Plugin.AT.IUserData userData = null;
            bool bAysnc = true;
            bool bRefreshOp = false;
            if (userVariable != null && userVariable is VariableMultiData)
            {
                VariableMultiData mutiData = (VariableMultiData)userVariable;
                Variable1 flag = (Variable1)mutiData.pData1;
                bAysnc = flag.byteVal0 != 0;
                bRefreshOp = flag.byteVal1 != 0;
                userData = mutiData.pData2;
            }
            else userData = userVariable;
#if !USE_SERVER
            if (IsLoading())
            {
                bAysnc = false;
                bRefreshOp = false;
            }
#endif

            IContextData pActorData = pNode.GetContextData();
            CsvData_Models.ModelsData model = null;
            string strActionGraph = "";
            int[] carrerCoefficient = null;
            byte carrerID = 0;
            byte dutyID = 0;
            ushort configLevel = 1;
            string strModelFiler = null;
            ushort careerWeakness = 0;
            ushort nElementFlag = 0;
            byte classify = 0;
            uint weight = 0;
            uint b_weight = 0;
            byte camp = 0;
            byte damageType = 0;
#if !USE_SERVER
            Transform byParentTransform = null;
#endif
            ActionStateGraph stateGraph = null;
            ActorParameter actorParameter = null;
            if (pNode is Actor)
            {
                Actor pActor = pNode as Actor;
                actorParameter = pActor.GetActorParameter();
                stateGraph = pActor.GetActionStateGraph();
#if !USE_SERVER
                byParentTransform = RootsHandler.FindActorRoot((int)pNode.GetActorType());
                if (byParentTransform == null)
                    byParentTransform = RootsHandler.ActorsRoot;
#endif
                CsvData_Summon.SummonData pSummonData = pActorData as CsvData_Summon.SummonData;
                CsvData_Monster.MonsterData pMonsterData = pActorData as CsvData_Monster.MonsterData;
                CsvData_Player.PlayerData pData = pActorData as CsvData_Player.PlayerData;
                if (pData != null)
                {
                    damageType = pData.damageType;
                    camp = pData.camp;
                    //weight = pData.Weight;
                    //b_weight = pData.B_Weight;
                    nElementFlag = (ushort)pData.element;
                    carrerID = pData.genre;
                    //carrerCoefficient = pData.genreCoefficient;
                    dutyID = (byte)pData.duty;
                    classify = (byte)pData.ballisticType;
#if !USE_SERVER
                    if (pNode is PreviewActor)
                    {
                        strActionGraph = pData.showActionGraph;

                        model = pData.Models_hModelID_data;
                        if (model != null)
                            strModelFiler = model.strFile;
                    }
                    else
#endif
                    {
                        strActionGraph = pData.actionGraph;

                        model = pData.Models_nModelID_data;
                        if (model != null)
                            strModelFiler = model.strFile;
                    }

                    RunerAgent pAgent = pActor.GetActorAgent() as RunerAgent;
                    if (pAgent == null) pAgent = new RunerAgent();
                    pAgent.SetActor(pActor);
                    pAgent.EnableAILogic(true);
                }
                else if (pMonsterData != null)
                {
                    camp = pMonsterData.camp;
                    careerWeakness = pMonsterData.genreWeakness;
                    MonsterAgent pAgent = pActor.GetActorAgent() as MonsterAgent;
                    if (pAgent == null) pAgent = new MonsterAgent();
                    pAgent.SetActor(pActor);
                    pAgent.EnableAILogic(true);
                    strActionGraph = pMonsterData.actionGraph;

                    model = pMonsterData.Models_modelId_data;
                    if (model != null)
                        strModelFiler = model.strFile;
                }
                else if(pSummonData!=null)
                {
                    damageType = pSummonData.damageType;
                    SummonAgent pAgent = pActor.GetActorAgent() as SummonAgent;
                    if (pAgent == null) pAgent = new SummonAgent();
                    pAgent.SetActor(pActor);
                    pAgent.EnableAILogic(true);
                    strActionGraph = pSummonData.actionGraph;

                    model = pSummonData.Models_nModelID_data;
                    if (model != null)
                        strModelFiler = model.strFile;
                }
                else
                {
                    RunerAgent pAgent = pActor.GetActorAgent() as RunerAgent;
                    if (pAgent == null) pAgent = new RunerAgent();
                    pAgent.EnableAILogic(true);
                    pAgent.SetActor(pActor);
                }
                if (!string.IsNullOrEmpty(strActionGraph))
                {
                    if (stateGraph == null || stateGraph.GetName().CompareTo(strActionGraph) != 0)
                    {
                        pStateGraph = ActionStateManager.getInstance().CreateActionStateGraph(strActionGraph, this);
                        pActor.SetActionStateGraph(pStateGraph);
                    }
                }
                pNode.SetSpatial(true);
            }
            else if(pNode is ObsElement)
            {
                CsvData_BattleObject.BattleObjectData pData = pActorData as CsvData_BattleObject.BattleObjectData;
                if (pData != null && pData.Models_modelId_data!=null)
                {
                    strModelFiler = pData.Models_modelId_data.strFile;
                }
                CsvData_Models.ModelsData pModelData = pActorData as CsvData_Models.ModelsData;
                if (pModelData != null)
                {
                    strModelFiler = pModelData.strFile;
                }
#if !USE_SERVER
                byParentTransform = RootsHandler.FindActorRoot((int)EActorType.Element);
                if (byParentTransform == null)
                    byParentTransform = RootsHandler.ActorsRoot;
#endif
            }

            if(actorParameter!=null)
                actorParameter.SetSvrData(userData as VariablePoolAble);


#if !USE_SERVER
            if (userData != null && userData is VariableObj)
            {
                AInstanceAble outInInstance = null;
                VariableObj pObj = (VariableObj)userData;
                if (pObj.pGO != null)
                {
                    outInInstance = pObj.pGO as AInstanceAble;
                }
                pNode.SetObjectAble(outInInstance);
            }
            else
            {
                if(string.IsNullOrEmpty(strModelFiler))
                {
                    InstanceOperiaon prefabOp = null;
#if UNITY_EDITOR
                    prefabOp = new InstanceOperiaon();
                    GameObject pDebugTest = new GameObject(pNode.GetInstanceID().ToString());
                    prefabOp.pPoolAble = pDebugTest.AddComponent<DebugEmptyInstanceAble>();
                    prefabOp.pPoolAble.SetParent(byParentTransform);
#endif
                    pNode.OnSpawnCallback(prefabOp);
                }
                else
                {
                    if (!pNode.IsObjected())
                    {
                        InstanceOperiaon prefabOp = FileSystemUtil.SpawnInstance(strModelFiler, bAysnc);
                        if (prefabOp != null)
                        {
                            pNode.SetInstanceOperiaon(prefabOp);
                            prefabOp.OnCallback=pNode.OnSpawnCallback;
                            prefabOp.OnSign =pNode.OnSpawnSign;
                            prefabOp.SetByParent(byParentTransform);
                            if (bRefreshOp)
                            {
                                prefabOp.Refresh();
                            }

                            if (pStateGraph != null && GetState() == EGameState.Battle)
                            {
                                List<string> vPreLoad = shareParams.stringCatchList;
                                HashSet<string> vHassSets = shareParams.stringCatchSet;
                                vPreLoad.Clear();
                                vHassSets.Clear();
                                pStateGraph.CollectPreload(vPreLoad, vHassSets);
                                PreloadAssets(vHassSets,true);
                                PreloadInstances(vPreLoad);
                                vPreLoad.Clear();
                                vHassSets.Clear();
                            }
                        }
                        else
                        {
                            pNode.OnSpawnCallback(null);
                        }
                    }
                    else
                    {
                        AInstanceAble pAble = pNode.GetObjectAble();
                        if(pAble)
                        {
                            pAble.ClearMaterialBlock();
                            pAble.ResetLayer();
                        }
                    }
                }
            }
#else
             pNode.OnSpawnCallback(null);
#endif
            if (actorParameter !=null)
            {
                actorParameter.SetCamp(camp);
                actorParameter.SetDamageType(damageType);
                actorParameter.SetCareerWeakness(careerWeakness);
                actorParameter.SetCareerCoefficient(carrerCoefficient);
                if (model!=null)
                {
                    actorParameter.SetName(model.strName);
                    actorParameter.SetModelHeight(model.fHeight);
                }
                actorParameter.SetElementFlag(nElementFlag);
                actorParameter.SetClassify(classify);

                actorParameter.SetDutyID(dutyID);
                actorParameter.SetCareerID(carrerID);
                actorParameter.SetWeight(weight, b_weight);
                //pActor.GetActorParameter().UpdataAttr();
#if USE_SERVER
                if (userData != null && userData is Proto3.BattleHeroInfo)
                {
                    Proto3.BattleHeroInfo hero = userData as Proto3.BattleHeroInfo;
                    actorParameter.SetLevel((ushort)(hero.GetLevel() + hero.GetExternLevel()), false);
                    if(hero.GetConfig()!=null) actorParameter.SetQuality((byte)hero.Quality, false);
                    actorParameter.SetRoleGUID(0);
                    actorParameter.UpdataAttr();
                }
                else
                    actorParameter.SetLevel((ushort)Mathf.Max(1, configLevel));
#else
                if (userData != null && userData is Framework.Data.ISvrActorData)
                {
                    Framework.Data.ISvrActorData hero = (Framework.Data.ISvrActorData)userData;
                    actorParameter.SetLevel((ushort)(hero.GetLevel()), false);
                    actorParameter.SetRoleGUID(0);
                    actorParameter.UpdataAttr();
                }
                else
                    actorParameter.SetLevel((ushort)Mathf.Max(1, configLevel));
#endif

                if (actorParameter.GetSkill() == null)
                    actorParameter.SetSkill(new BattleSkillInformations());
            }
            pNode.OnCreated();
            if(actorParameter!=null)
                actorParameter.ResetRunTimeParameter();

            //             if (m_pAgentTree != null)
            //             {
            //                 m_pAgentTree.ExecuteEvent((ushort)Base.EATEventType.OnActorCreate, 1, pNode);
            //             }
        }
        //------------------------------------------------------
        public override void UpdateActorAttr(Actor pActor)
        {
            ActorParameter actorParam = pActor.GetActorParameter();
            if (actorParam == null) return;
#if UNITY_EDITOR
            actorParam.attrDebuger.Clear();
#endif
            IContextData pData = pActor.GetContextData();
            if (pData == null) return;

            CalcExternAttrParam extarnParam = new CalcExternAttrParam();
            extarnParam.Clear();
            SvrData.ISvrHero hero = actorParam.GetSvrData() as SvrData.ISvrHero;
            ushort maxLevel = actorParam.GetLevel();
            if (hero != null)
            {
                extarnParam.SystemAttrs = hero.GetAllSystemAttrParams();
            }

          //  actorParam.SetQualityData(qualityData);

            int level = actorParam.GetLevel();
            CsvData_Level.LevelData lvData = Data.DataManager.getInstance().Level.GetData((uint)level);
            float growAbilityRate = 1;
            if (lvData != null)
            {
                maxLevel = (ushort)lvData.levelMax;
                if (pActor.GetActorType() == EActorType.Monster)
                    growAbilityRate = lvData.monsterGrowAbilityRate;
                else if (pActor.GetActorType() == EActorType.Summon)
                    growAbilityRate = lvData.summonGrowAbilityRate;
                else if (pActor.GetActorType() == EActorType.Player || pActor.GetActorType() == EActorType.PreviewActor)
                    growAbilityRate = lvData.growAbilityRate;
            }
            actorParam.SetMaxLevel(maxLevel);

            actorParam.GetAttr().ResetExtern();
            actorParam.SetLevelData(lvData);

            BaseAttrArrayParam baseArrayAttr = AttrUtil.BuildBaseAttrParam(pData, growAbilityRate);
            extarnParam.arrayAttrs = baseArrayAttr;
#if UNITY_EDITOR
            CalcAttrDebuger.SetCurrentDebuger(actorParam.attrDebuger);
#endif
            FFloat[] attrs = CalcActorAtribute.CalcAttribute(shareParams,  pData, baseArrayAttr, CalcExternAttrParam.DEF);
            if (attrs != null)
                actorParam.ResetBaseAttr(attrs);
            CalcActorAtribute.CalcExternAttr(actorParam, extarnParam);
#if UNITY_EDITOR
            CalcAttrDebuger.SetCurrentDebuger(null);
#endif
        }
        //------------------------------------------------------
        public override void ResetActorRuntimeAttr(Actor pActor)
        {
            ActorParameter pActorParameter = pActor.GetActorParameter();
            if (pActorParameter == null) return;
            base.ResetActorRuntimeAttr(pActor);
            switch(pActor.GetActorType())
            {
                case EActorType.Player:
                    {
                        CsvData_Player.PlayerData pCsvData = pActor.GetContextData() as CsvData_Player.PlayerData;
                        if (pCsvData != null)
                        {
                            pActorParameter.SetAttackSpcCoefficient(pCsvData.atkSpCoefficient);
                            pActorParameter.SetHittedSpcCoefficient(pCsvData.defSpCoefficient);
                            pActorParameter.SetAttackerKillSpecValue((int)pCsvData.deadSpValue);
                        }
                    }
                    break;
                case EActorType.Monster:
                    {
                        CsvData_Monster.MonsterData pCsvData = pActor.GetContextData() as CsvData_Monster.MonsterData;
                        if (pCsvData != null)
                        {
                            ActorRuntimeParameter runtime = pActorParameter.GetRuntimeParam();
                            runtime.posture_damage_rate = FMath.Clamp01(pCsvData.postureReduction);
                            runtime.posture_recDuration = pCsvData.postureTime;
                            runtime.posture_recTime = 0;
                            runtime.frenzy_count = (int)pCsvData.postureRageNum;
                            runtime.freezy_buffs = pCsvData.postureRageBuffId;
                            runtime.exhaustionBuffs = pCsvData.postureTimeBuffId;
                            runtime.posture_recDuration = pCsvData.postureRecoverTime;
                            runtime.posture_idleTime = 0;
                            runtime.posture_idleDuration = pCsvData.postureTime;

                            pActorParameter.SetAttackSpcCoefficient(pCsvData.atkSpCoefficient);
                            pActorParameter.SetHittedSpcCoefficient(pCsvData.defSpCoefficient);
                            pActorParameter.SetAttackerKillSpecValue((int)pCsvData.deadSpValue);
                        }
                    }
                    break;
                case EActorType.Summon:
                    {
                        CsvData_Summon.SummonData pCsvData = pActor.GetContextData() as CsvData_Summon.SummonData;
                        if (pCsvData != null)
                        {
                            pActorParameter.SetAttackSpcCoefficient(pCsvData.atkSpCoefficient);
                            pActorParameter.SetHittedSpcCoefficient(pCsvData.defSpCoefficient);
                            pActorParameter.SetAttackerKillSpecValue((int)pCsvData.deadSpValue);
                        }
                    }
                    break;
            }


            // calc fight power
            int fightPower = 0;
            SvrData.ISvrHero svrHero = pActorParameter.GetSvrData() as SvrData.ISvrHero;
            if(svrHero!=null && svrHero.GetSkills() != null)
            {
                List<SvrSkillData> vSkills = svrHero.GetSkills();
                SvrSkillData svrSkill;
                for (int i = 0; i < vSkills.Count; ++i)
                {
                    svrSkill = vSkills[i];
                    if (svrSkill.configData != null && svrSkill.configData is CsvData_Skill.SkillData)
                        fightPower += (int)((CsvData_Skill.SkillData)svrSkill.configData).fightPower;
                }
            }
            else
            {
                // skill
                if (pActor.GetSkill() != null)
                {
                    List<Skill> vSkills = pActor.GetSkill().GetLearned();
                    if (vSkills != null)
                    {
                        for (int i = 0; i < vSkills.Count; ++i)
                        {
                            if (vSkills[i].skillData != null)
                                fightPower += (int)(vSkills[i].skillData as CsvData_Skill.SkillData).fightPower;
                        }
                    }
                }
            }


//             Dictionary<uint, CsvData_Power.PowerData> powerDatas = Data.DataManager.getInstance().Power.datas;
//             if(powerDatas!=null )
//             {
//                 foreach(var db in powerDatas)
//                 {
//                     float fValue = pActorParameter.GetAttr((EAttrType)db.Key);
//                     if (fValue > 0)
//                         fightPower += (int)(((fValue + db.Value.powerReset )* db.Value.powerParam) );
//                 }
//             }
            fightPower = Mathf.Max(0, fightPower);
            pActorParameter.SetFightPower(fightPower);
        }
    }
}
