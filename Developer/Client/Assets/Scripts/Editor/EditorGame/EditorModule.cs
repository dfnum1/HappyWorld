/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	EditorModule
作    者:	HappLI
描    述:	编辑器主模块
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using TopGame.Data;
using TopGame.Core;
using TopGame.Logic;
using Framework.Plugin.AT;
using Framework.Plugin.Guide;
using Framework.Plugin.AI;
using System.Linq;
using TopGame.Post;
using Framework.Core;
using Framework.Logic;
using Framework.Module;
using System.Collections;
using ExternEngine;

namespace TopGame.ED
{
    public class EditorModule : Core.GameModule
    {
        Logic.EditorBattle m_pBattleLogic = new EditorBattle();
        class FramworkSetting : Framework.IFrameworkCore
        {
            public bool IsEditor() { return true; }
            public Coroutine BeginCoroutine(IEnumerator coroutine)
            {
                return null;
            }

            public void EndAllCoroutine()
            {

            }

            public void EndCoroutine(Coroutine cortuine)
            {

            }

            public void EndCoroutine(IEnumerator cortuine)
            {

            }

            public ActionSystemConfig GetActionSystemConfig() { return new ActionSystemConfig(); }

            public int GetMaxThread() { return 0; }
            public EFileSystemType GetFileStreamType() { return EFileSystemType.AssetData; }
        }
        new EditorCameraController m_pCameraController;
        Logic.Battle m_pBattle = new Logic.Battle();

        ~EditorModule()
        {

        }
        public void StartUp()
        {
            FramworkSetting plusSetting = new FramworkSetting();
            Framework.Data.DataEditorUtil.ClearTables();
            if (UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline == null)
            UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline = AssetDatabase.LoadAssetAtPath<UnityEngine.Rendering.RenderPipelineAsset>("Assets/DatasRef/Config/RenderURP/Default/UniversalRenderPipelineAsset.asset");

            Framework.Module.ModuleManager.editorModule = this;
            FileSystem pFileSystem = new FileSystem();
            pFileSystem.PreBuild(plusSetting);
            m_pFileSystem = RegisterModule<FileSystem>(pFileSystem);
            m_pFileSystem.EnableCoroutines(false);
            if (Data.DataManager.getInstance() == null)
                m_pDataMgr = RegisterModule<Data.DataManager>();

            System.Reflection.MethodInfo awakeMethod = this.GetType().GetMethod("Awake",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic);
            awakeMethod.Invoke(this, new object[] { plusSetting });

            ActionStateManager.getInstance().Init(this);

            m_pCameraController = new EditorCameraController();

            BattleCameraMode pBattleCameraMode = new BattleCameraMode();
            m_pCameraController.RegisterCameraMode("battle", pBattleCameraMode);
            m_pCameraController.SwitchMode("battle");
            m_pCameraController.SetCamera(Camera.main);

            bEditorMode = true;
            Core.FileSystem.bEditorMode = bEditorMode;
            GetProjectileManager().SetDatas(Framework.Data.DataEditorUtil.GetTable<CsvData_Projectile>(true).datas);

            System.Reflection.MethodInfo startUpMethod = this.GetType().GetMethod("StartUp", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            startUpMethod.Invoke(this, new object[] { plusSetting });

            m_pBattleLogic.Awake(this);
            m_pBattleLogic.Start();

            float regionWidth = 100;// TopGame.Data.GlobalSetting.DefaultBattleRegionWidth;
            world.SetLimitZoom(new Vector3(-regionWidth, 0, 0), new Vector3(regionWidth, 0, 0));

            ATerrainManager.EnableTerrainLowerLimit(this, true);
            ATerrainManager.SetTerrainLowerLimit(this, 0);
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Framework.Data.DataEditorUtil.ClearTables();
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            //             if (Framework.Module.ModuleManager.mainModule == GameInstance.getInstance())
            //                 return;
            Framework.Module.ModuleManager.editorModule = this;
            short logicLock = m_nLogicLock;
            m_nLogicLock = 0;
            base.InnerUpdate(fFrame);
            m_nLogicLock = logicLock;

            Core.FileSystem.bEditorMode = true;
            if (m_pCameraController != null) m_pCameraController.ForceUpdate(fFrame);
            if (m_pBattle != null)
            {
                m_pBattle.SetFramework(this);
                m_pBattle.OnUpdate(fFrame);
            }
            if (m_pFileSystem != null)
            {
                m_pFileSystem.EnableCoroutines(false);
                m_pFileSystem.Update(fFrame, false);
            }
        }
        //------------------------------------------------------
        protected override AWorldNode ExcudeWorldNodeMalloc(EActorType type)
        {
            switch (type)
            {
                case EActorType.Player: return new Logic.Player(this);
                case EActorType.PreviewActor: return new Logic.PreviewActor(this);
                case EActorType.Mecha: return new Logic.Mecha(this);
                case EActorType.Monster: return new Logic.Monster(this);
                case EActorType.Summon: return new Logic.Summon(this);
                case EActorType.Scene: return new Logic.WorldSceneNode(this);
                case EActorType.Projectile: return new Core.Projectile(this);
                case EActorType.Element: return new Logic.ObsElement(this);
                case EActorType.Wingman: return new Logic.Wingman(this);
            }
            return null;
        }
        //------------------------------------------------------
        public override void OnWorldNodeStatus(AWorldNode pNode, EWorldNodeStatus status, VariablePoolAble userVariable = null)
        {
            if (status != EWorldNodeStatus.Create) return;

            Framework.Plugin.AT.IUserData userData = null;
            if (userVariable != null && userVariable is VariableMultiData)
            {
                VariableMultiData mutiData = (VariableMultiData)userVariable;
                userData = mutiData.pData2;
            }
            else userData = userVariable;

            IContextData pActorData = pNode.GetContextData();
            CsvData_Models.ModelsData model = null;
            string strActionGraph = "";
            byte carrerID = 0;
            byte dutyID = 0;
            ushort configLevel = 1;
            string strModelFiler = null;
            ushort careerWeakness = 0;
            ushort nElementFlag = 0;
            byte classify = 0;
            Transform byParentTransform = null;
            ActionStateGraph stateGraph = null;
            ActorParameter actorParameter = null;
            if (pNode is Actor)
            {
                Actor pActor = pNode as Actor;
                actorParameter = pActor.GetActorParameter();
                stateGraph = pActor.GetActionStateGraph();
                byParentTransform = RootsHandler.FindActorRoot((int)pNode.GetActorType());
                if (byParentTransform == null)
                    byParentTransform = RootsHandler.ActorsRoot;
                CsvData_Summon.SummonData pSummonData = pActorData as CsvData_Summon.SummonData;
                CsvData_Monster.MonsterData pMonsterData = pActorData as CsvData_Monster.MonsterData;
                CsvData_Player.PlayerData pData = pActorData as CsvData_Player.PlayerData;
                if (pData != null)
                {
                    nElementFlag = (ushort)pData.element;
                    carrerID = pData.genre;
                    dutyID = (byte)pData.duty;
                    classify = (byte)pData.ballisticType;

                    if (pNode is PreviewActor)
                    {
                        strActionGraph = pData.showActionGraph;

                        model = pData.Models_hModelID_data;
                        if (model != null)
                            strModelFiler = model.strFile;
                    }
                    else
                    {
                        strActionGraph = pData.actionGraph;

                        model = pData.Models_nModelID_data;
                        if (model != null)
                            strModelFiler = model.strFile;
                    }


                    ActorEditorAgent pAgent = pActor.GetActorAgent() as ActorEditorAgent;
                    if (pAgent == null) pAgent = new ActorEditorAgent();
                    pAgent.SetActor(pActor);
                }
                else if (pMonsterData != null)
                {
                    careerWeakness = pMonsterData.genreWeakness;
                    ActorEditorAgent pAgent = pActor.GetActorAgent() as ActorEditorAgent;
                    if (pAgent == null) pAgent = new ActorEditorAgent();
                    pAgent.SetActor(pActor);
                    strActionGraph = pMonsterData.actionGraph;

                    model = pMonsterData.Models_modelId_data;
                    if (model != null)
                        strModelFiler = model.strFile;
                }
                else if (pSummonData != null)
                {
                    ActorEditorAgent pAgent = pActor.GetActorAgent() as ActorEditorAgent;
                    if (pAgent == null) pAgent = new ActorEditorAgent();
                    pAgent.SetActor(pActor);
                    strActionGraph = pSummonData.actionGraph;

                    model = pSummonData.Models_nModelID_data;
                    if (model != null)
                        strModelFiler = model.strFile;
                }
                else
                {
                    ActorEditorAgent pAgent = pActor.GetActorAgent() as ActorEditorAgent;
                    if (pAgent == null) pAgent = new ActorEditorAgent();
                    pAgent.SetActor(pActor);
                }
                if (!string.IsNullOrEmpty(strActionGraph))
                {
                    if (stateGraph == null || stateGraph.GetName().CompareTo(strActionGraph) != 0)
                    {
                        pActor.SetActionStateGraph(ActionStateManager.getInstance().CreateActionStateGraph(strActionGraph, this));
                    }
                }
                pNode.SetSpatial(true);
            }
            else if (pNode is ObsElement)
            {
                CsvData_BattleObject.BattleObjectData pData = pActorData as CsvData_BattleObject.BattleObjectData;
                if (pData != null && pData.Models_modelId_data != null)
                {
                    strModelFiler = pData.Models_modelId_data.strFile;
                }
                byParentTransform = RootsHandler.FindActorRoot((int)EActorType.Element);
                if (byParentTransform == null)
                    byParentTransform = RootsHandler.ActorsRoot;
            }
            else if (pNode is WorldSceneNode)
            {
                byParentTransform = RootsHandler.ScenesRoot;
            }

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
                if (!pNode.IsObjected())
                {
                    InstanceOperiaon prefabOp = FileSystemUtil.SpawnInstance(strModelFiler, false);
                    if (prefabOp != null)
                    {
                        prefabOp.OnCallback = pNode.OnSpawnCallback;
                        prefabOp.OnSign = pNode.OnSpawnSign;
                        prefabOp.pByParent = byParentTransform;
                        prefabOp.Refresh();
                    }
                }
            }
            pNode.EnableLogic(true);
            pNode.EnableAI(false);
            pNode.SetSpatial(true);
            pNode.SetActived(true);
            pNode.SetVisible(true);
            if (actorParameter != null)
            {
                actorParameter.SetCareerWeakness(careerWeakness);
                if(model!=null)
                {
                    actorParameter.SetName(model.strName);
                    actorParameter.SetModelHeight(model.fHeight);
                }

                actorParameter.SetElementFlag(nElementFlag);
                actorParameter.SetClassify(classify);

                actorParameter.SetDutyID(dutyID);
                actorParameter.SetCareerID(carrerID);
                //pActor.GetActorParameter().UpdataAttr();
                actorParameter.SetLevel((ushort)Mathf.Max(1, configLevel));
                actorParameter.ResetRunTimeParameter();

                actorParameter.SetSkill(new BattleSkillInformations());
            }
            pNode.OnCreated();

            //             if (m_pAgentTree != null)
            //             {
            //                 m_pAgentTree.ExecuteEvent((ushort)Base.EATEventType.OnActorCreate, 1, pNode);
            //             }
        }
        //------------------------------------------------------
        protected static FFloat[] ms_TempAttr = new FFloat[(int)EAttrType.Num];
        public static FFloat[] TempAttr
        {
            get { return ms_TempAttr; }
        }
        public override void UpdateActorAttr(Actor pActor)
        {
            IContextData pData = pActor.GetContextData();
            if (pData == null) return;
            byte quality = pActor.GetActorParameter().GetQuality();
//             CsvData_HeroQuality.HeroQualityData qualityData = Framework.Data.DataEditorUtil.GetTable<CsvData_HeroQuality>().GetData(quality);
//             pActor.GetActorParameter().SetQualityData(qualityData);

            pActor.GetActorParameter().SetMaxLevel((ushort)10000);

            int level = pActor.GetActorParameter().GetLevel();
            CsvData_Level.LevelData lvData = Framework.Data.DataEditorUtil.GetTable<CsvData_Level>().GetData((uint)level);
            float growAbilityRate = 1;
            if (lvData != null) growAbilityRate = lvData.growAbilityRate;
            pActor.GetActorParameter().SetLevelData(lvData);
            System.Array.Clear(ms_TempAttr, 0, ms_TempAttr.Length);
            for (int i = 0; i < ms_TempAttr.Length; ++i)
                ms_TempAttr[i] = 10000;
            pActor.GetActorParameter().ResetBaseAttr(ms_TempAttr);
        }
        //------------------------------------------------------
        public override void OnTriggerEvent(int nEventID, bool bAutoClear = true)
        {
            if (m_pEventTrigger == null) return;
            if (bAutoClear)
            {
                m_pEventTrigger.Begin();
                m_pEventTrigger.TriggerEventPos = GetPlayerPosition();
                m_pEventTrigger.TriggerEventRealPos = GetPlayerPosition();
            }
            EventData pEvent = DataManager.getInstance().EventDatas.GetData(nEventID);
            if (pEvent == null) return;

            if (pEvent.events == null)
            {
                Framework.Data.DataEditorUtil.MappingTable(DataManager.getInstance().EventDatas);
                return;
            }
            if (pEvent.events == null)
                return;

            Vector3 TriggerEventPos = m_pEventTrigger.TriggerEventPos;
            Vector3 TriggerEventRealPos = m_pEventTrigger.TriggerEventRealPos;
            if (pEvent.groupRandom)
            {
                BaseEventParameter evt = pEvent.GetRandomGroup(this);
                if (evt != null && CheckerRandom(evt.triggerRate))
                    OnTriggerEvent(evt, false);
            }
            else
            {
                for (int i = 0; i < pEvent.events.Count; ++i)
                {
                    m_pEventTrigger.TriggerEventPos = TriggerEventPos;
                    m_pEventTrigger.TriggerEventRealPos = TriggerEventRealPos;
                    if (CheckerRandom(pEvent.events[i].triggerRate))
                        OnTriggerEvent(pEvent.events[i], false);
                }
            }

            if (bAutoClear) m_pEventTrigger.End();
        }
        //------------------------------------------------------
        public override void OnProjectileUpdate(AProjectile pProjectile)
        {
            base.OnProjectileUpdate(pProjectile);
//             if(pProjectile.CanSceneTest())
//             {
//                 if (pProjectile.position.y <= 0 && pProjectile.running_time >= 1f)
//                 {
//                     m_pProjectileManager.DoProjectileHit(pProjectile, null, null, pProjectile.remain_hit_count, true);
//                 }
//             }


            if (!EditorApplication.isPlaying)
            {
                if(pProjectile.GetObjectAble() != null)
                {
                    ParticleSystem[] parSys = (pProjectile.GetObjectAble() as AInstanceAble).GetComponentsInChildren<ParticleSystem>();
                    for(int i = 0; i < parSys.Length; ++i)
                    {
                        parSys[i].Simulate(pProjectile.running_time, true);
                    }
                }
            }
        }
        //------------------------------------------------------
        public override void OnLaunchProjectile(AProjectile pProj)
        {
            Projectile pProjectile = pProj as Projectile;
            base.OnLaunchProjectile(pProjectile);
            if (!string.IsNullOrEmpty(pProjectile.launch_effect))
            {
                GameObject pAsset = AssetDatabase.LoadAssetAtPath<GameObject>(pProjectile.launch_effect);
                if (pAsset != null)
                {
                    GameObject pObj = GameObject.Instantiate(pAsset);
                    AInstanceAble pAble = pObj.GetComponent<AInstanceAble>();
                    if(pAble == null) pAble = pObj.AddComponent<DummyInstanceAble>();
                    pProjectile.SetObjectAble(pAble);
                    pProjectile.SetPosition(pProjectile.init_position);
                    pProjectile.SetDirection(pProjectile.direction);
                    pProjectile.SetUp(Vector3.up);
                    pProjectile.SetScale(Vector3.one * pProjectile.projectile.scale);
                }
            }
            //! waring test
            if (pProjectile.TestFinalDropPos())
            {
                GameObject pAsset = AssetDatabase.LoadAssetAtPath<GameObject>(pProjectile.projectile.waring_effect);
                if (pAsset != null)
                {
                    GameObject pObj = GameObject.Instantiate(pAsset);
                    AInstanceAble pAble = pObj.GetComponent<AInstanceAble>();
                    if(pAble == null) pAble = pObj.AddComponent<DummyInstanceAble>();
                    pProjectile.particle_waring_data = pAble;
                    pObj.transform.position = pProjectile.final_drop_pos;
                    pObj.transform.forward = pProjectile.direction;
                    pObj.transform.up = Vector3.up;
                }
            }
            if (pProjectile.launch_sound_id>0)
            {
                List<CsvData_Audio.AudioData> audios = (DataManager.getInstance() != null && DataManager.getInstance().Audio != null) ? DataManager.getInstance().Audio.GetData(pProjectile.launch_sound_id) : null;
                if (audios != null && audios.Count > 0)
                {
                    EditorKits.PlayClip(audios[UnityEngine.Random.Range(0, audios.Count)].location);
                }
            }
            else if (!string.IsNullOrEmpty(pProjectile.launch_sound))
            {
                EditorKits.PlayClip(pProjectile.launch_sound);
            }
        }
        //------------------------------------------------------
        public override void OnStopProjectile(AProjectile pProj)
        {
            Projectile pProjectile = pProj as Projectile;
            if (pProjectile.particle_waring_data!=null)
            {
                pProjectile.particle_waring_data.Destroy();
            }
            pProjectile.Destroy();
        }
        //------------------------------------------------------
        public static IUserData OnBuildEvent(string strEvent)
        {
            if (string.IsNullOrEmpty(strEvent)) return null;
            return BuildEventUtl.BuildEvent(null, strEvent);
        }
        //------------------------------------------------------
        public static string SaveEvent(IUserData pEvent)
        {
            if (pEvent == null || !(pEvent is BaseEventParameter)) return null;
            return (pEvent as BaseEventParameter).WriteCmd();
        }
        //------------------------------------------------------
        public static void SaveTable(string strTable)
        {
            if (!DataManager.getInstance().bInited) return;
            System.Reflection.PropertyInfo field = DataManager.getInstance().GetType().GetProperty(strTable);
            if (field != null)
            {
                var table = field.GetValue(DataManager.getInstance());
                if (table == null) return;
                System.Reflection.MethodInfo save = table.GetType().GetMethod("Save");
                if (save == null) return;
                save.Invoke(table, new object[] { null });
            }
        }
    }
}
