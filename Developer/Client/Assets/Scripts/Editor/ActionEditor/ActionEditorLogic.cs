/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ActionEditorLogic
作    者:	HappLI
描    述:	动作编辑器
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;
using System.IO;
using TopGame.Data;
using TopGame.Core;
using Framework.Core;
using Framework.ED;
using Framework.Data;
using Framework.Module;
using ExternEngine;

namespace TopGame.ED
{
    public class ActionEditorLogic : IWorldNodeCallback
    {
        struct JumpParam
        {
            public bool bValid;
            public float JumpHorizenSpeed;
            public float JumpVerticalSpeed;
            public float Gravity;
            public float FallingSpeed;
            public float MaxJumpHeight;
        }
        class JumpTest
        {
            public FFloat fFallingMomentum;
            public FFloat fJumpInitVelocity;
            public FFloat fCurrentGravity;
            public FVector3 controllSpeed;

            public FVector3 controllerPosition;

            public JumpParam CopyJumpParam = new JumpParam() { bValid = false };
            public float fJumpMostHeight = 0;
            public bool bSimulateJump = false;
            public List<FVector3> SimulateJumpPoints = new List<FVector3>();
            public Vector3 SimulateSpeed = Vector3.forward * 5;
            public float fFallAcc = 1;
            public float fMaxJumpHeight = 50;

            public void StartJump(Actor pActor )
            {
                controllerPosition = pActor.GetPosition();
                SimulateJumpPoints.Clear();
                fMaxJumpHeight = pActor.GetMaxJumpHeight();
                FFloat fArgvJumpSpeed = pActor.GetJumpVerticalSpeed();
                fCurrentGravity = pActor.GetGravity();
                fFallingMomentum = pActor.GetFallingSpeed();
                if (fCurrentGravity <= FFloat.zero) return;

                controllSpeed = Vector3.zero;
                controllSpeed.z = SimulateSpeed.z;
                fFallingMomentum += Data.GlobalSetting.Instance.fFallingVelocity;
                controllSpeed.y = Data.GlobalSetting.Instance.fJumpSpeed * fArgvJumpSpeed;
                fJumpInitVelocity = Data.GlobalSetting.Instance.fJumpInitVelocity;

                SimulateJumpPoints.Add(pActor.GetPosition());
            }

            public void Update(Actor pActor, FFloat fTime)
            {
                if (!bSimulateJump) return;
                FFloat falling = 0;
                if (controllSpeed.y <= FFloat.zero)
                {
                    fJumpInitVelocity = FFloat.zero;
                    fFallingMomentum = FMath.Lerp(fFallingMomentum, FFloat.zero, fTime);
                    falling = fFallingMomentum;
                }
                fJumpInitVelocity = FMath.Lerp(fJumpInitVelocity, FFloat.zero, fTime);
                if (fCurrentGravity < FFloat.zero)
                    fCurrentGravity = Framework.Core.CommonUtility.GTRAVITY_VALUE;

                controllSpeed.y -= (fCurrentGravity + fJumpInitVelocity + falling) * fTime;

                //! apply fraction
                controllSpeed.x = Actor.APPLY_FRACTION(controllSpeed.x, FFloat.one, fTime);
                controllSpeed.z = Actor.APPLY_FRACTION(controllSpeed.z, FFloat.one, fTime);

                controllerPosition += controllSpeed.y * fTime * pActor.GetFinalUp();
                controllerPosition += controllSpeed.z * fTime * pActor.GetFinalDirection();

                if (controllSpeed.y < FFloat.zero)
                {
                    if (controllerPosition.y <= FFloat.zero)
                        bSimulateJump = false;
                }
                pActor.SetFinalPosition(controllerPosition);
                SimulateJumpPoints.Add(controllerPosition);
            }

            public void Draw()
            {
                if (SimulateJumpPoints.Count <= 1) return;
                Color color = Handles.color;
                FFloat topMos = FFloat.zero;
                FVector3 topCenter = FVector3.zero;
                for (int i = 0; i < SimulateJumpPoints.Count; ++i)
                {
                    if(SimulateJumpPoints[i].y > topMos)
                    {
                        topMos = SimulateJumpPoints[i].y;
                        topCenter = SimulateJumpPoints[i];
                    }
                    if (i + 1 < SimulateJumpPoints.Count)
                        Handles.DrawLine(SimulateJumpPoints[i], SimulateJumpPoints[i + 1]);
                }
                Handles.color = Color.red;
                Handles.DrawLine(topCenter, new Vector3(topCenter.x, 0, topCenter.z));
                Handles.Label(topCenter, new GUIContent(topMos.ToString()));
                fJumpMostHeight = topMos;
                Handles.color = color;
            }
        }
        static string[] BLEND_PARAMES_NAMES = new string[] { null, "DirSign" };
        static string[] BLEND_PARAMES_POP_NAMES = new string[] { "无", "方向因子" };
        public enum ETabType
        {
            BaseInfo,
            Avatar,
            Stance,
            StateFrames,
        }
        public ETabType curTab = ETabType.Stance;
        static string[] TABS = { "基础信息", "Avatar", "动作姿势", "全局碰撞框"};
        public struct PopData
        {
            public uint ID;
            public IContextData pData;
            public string strFile;
            public string graphFile;
            public bool bHigh;
        }

        ActionEditor m_pEditor;

        TargetPreview m_preview;

        int m_curGraphStateIndex = 0;
        string m_pCopyActionState = null;

        bool m_bExpandAction = true;
        bool m_bEpxandActionCore= false;
        bool m_bEpxandAnimationSeq = true;
        bool m_bExpandActionFrameParameter = false;
        bool m_bEpxandActionEvent = false;
        bool m_bEpxandActionProperty = false;

        int m_nPlayActionCnt = 0;
        ActionState m_pBaseCrossState = null;
        ActionState m_pCurActionState = null;
        int m_nSelectFrame = -1;
        bool m_bCopyVolume = false;
        StateFrame.Volume m_pCopyVolume;
        List<string> m_vActionStateNames = new List<string>();
        List<string> m_vActionStatePopNames = new List<string>();
        bool m_bExpandActionStateParameter = true;
        bool m_bExpandStand = true;
        string m_curStandName = "";
        string m_strAddStandName = "";

        int m_nAddSucceedActionID = 0;
        bool m_bExpandSucceedActionList = false;
        Dictionary<ActionState, bool> m_bExpandSucceedChecks = new Dictionary<ActionState, bool>();

        string m_addActionName = "";
        string m_curActionName = "";

        EEventType m_AddEventType = EEventType.Count;

        List<string> m_vStandPop = new List<string>();

        List<string> m_vParameters = new List<string>();
        List<int> m_vParameterHashs = new List<int>();

        class ParSimulate
        {
            public ParticleSystem parSystem;
            public Animator parAnimator;
            public float simulateTime;
            public float durationTime;
        }
        List<ParSimulate> m_vParSimulates = new List<ParSimulate>();

        EditorModule m_pGameModuel = null;

        AniStatePlayback m_AniPlayback = null;

        int m_nDummyAttackGroup = 1;
        Vector3 m_DummyOffset = Vector3.forward * 10;
        Actor m_pDummyActor = null;
        AniStatePlayback m_pDummyAniPrevew = new AniStatePlayback();

        Actor m_pActor = null;
        JumpTest m_JumpTest = new JumpTest();

        public float fActorEulerAngle = 0;

        ActionGraphBinder m_pGraphBinder = null;
        GameObject m_pActorPrefab = null;
        List<string> m_vAnimations = new List<string>();

        bool m_bExpandSlot = false;
        int m_nAddSlotIndex = -1;
        List<string> m_vSlotPop = new List<string>();
        string m_bExpandListenSlot = "";
        List<string> m_vChildTransformSlotsPop = new List<string>();
        List<string> m_vFullNameChildTransformSlotsPop = new List<string>();
        List<Transform> m_vChildTransformSlots = new List<Transform>();


        List<AnimatorStateData> m_vAnimationState = new List<AnimatorStateData>();
        AvatarMask[]    m_AvatarMasks = null;
        private AnimatorController m_AnimatorController;
        private Animator m_Animator;
        private string m_strModeFile = "";

        private bool m_bExpandSlotAvatar = false;
        private int m_nSelectAvatarIndex = -1;

        private string m_strFiler = "";
        List<StateFrame.Volume> m_vTestVolumes = new List<StateFrame.Volume>();
        List<string> m_vPopStateFrames = new List<string>();
        List<string> m_vPopStatePropertysName = new List<string>();
        int m_nStateFrameIndex = -1;
        int m_nSelectIndex = -1;
        List<string> m_PopDatas = new List<string>();
        List<PopData> m_PopUserDatas = new List<PopData>();
        List<string> m_PopSlots = new List<string>();
        Camera m_pCamera = null;
        //-----------------------------------------------------
        public void OnEnable(ActionEditor pEditor, EditorModule modele)
        {
            m_pEditor = pEditor;
            m_AniPlayback = new AniStatePlayback();
            m_pGameModuel = modele;
            OnReLoadAssetData();
            GameObject camera = new GameObject("camera");
            camera.hideFlags = HideFlags.HideAndDontSave;

            if(m_pGameModuel.world!=null)
                m_pGameModuel.world.RegisterCallback(this);

            EditorApplication.hierarchyChanged += OnHierarchyDirtyChange;
           // m_pCamera = camera.AddComponent<Camera>();

            //             m_preview = new TargetPreview();
            //             m_preview.Initialize(new GameObject[] { camera });
            //             m_preview.SetCamera(0.1f, 10000f, 60f);
            //             m_preview.SetPreviewInstance(camera);
            //             m_preview.OnDrawAfterCB = OnSceneGUI;
        }
        //-----------------------------------------------------
        public void OnDisable()
        {
            Clear();
            EditorApplication.hierarchyChanged -= OnHierarchyDirtyChange;
            if (m_pGameModuel.world != null)
            {
                m_pGameModuel.world.ClearWorld();
                m_pGameModuel.world.UnRegisterCallback(this);
            }
            m_pGameModuel.Destroy();

            if (m_preview != null)
                m_preview.Destroy();

            if (m_pCamera) GameObject.DestroyImmediate(m_pCamera.gameObject);
            m_pCamera = null;

            if (m_pDummyActor != null)
            {
                m_pDummyActor.SetDestroy();
            }
            m_pDummyActor = null;
        }
        //-----------------------------------------------------
        public EditorModule GetModule()
        {
            return m_pGameModuel;
        }
        //-----------------------------------------------------
        void ClearTarget()
        {
            if (m_AniPlayback != null) m_AniPlayback.Clear();
            m_pActorPrefab = null;
            m_pGraphBinder = null;
            m_bExpandSucceedChecks.Clear();
            m_bExpandListenSlot = "";
            m_vSlotPop.Clear();
            m_vAnimations.Clear();
            m_vChildTransformSlots.Clear();
            m_vChildTransformSlotsPop.Clear();
            m_vFullNameChildTransformSlotsPop.Clear();
            m_vAnimationState.Clear();
            m_AvatarMasks = null;
            m_AnimatorController = null;
            m_Animator = null;
            ClearStateFrames();
        }
        //-----------------------------------------------------
        void ClearStateFrames()
        {
            m_nStateFrameIndex = -1;
            m_vPopStateFrames.Clear();
        }
        //-----------------------------------------------------
        public void Clear()
        {
            if (m_pActor != null)
            {
                m_pActor.Destroy();
            }
            m_pActor = null;
            ClearTarget();
            DestroyParticles();
        }
        //-----------------------------------------------------
        public void Load()
        {
            string path = EditorUtility.OpenFilePanel("打开", Application.dataPath + ActionStateManager.SAVE_PATH_ROOT, "json");
            if (string.IsNullOrEmpty(path)) return;
            path = path.Replace("\\", "/");

            path = path.Replace(Application.dataPath, "Assets");

            ActionGraphData pGraph = LoadGraph(path);
            Load(pGraph, true);
        }
        //-----------------------------------------------------
        ActionGraphData LoadGraph(string strFilePath)
        {
            TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(strFilePath);
            if (asset == null)
            {
                EditorUtility.DisplayDialog("提示", "不是一个有效的动作脚本你资源！", "请确认");
                return null;
            }
            try
            {
                ActionGraphData pGraph = JsonUtility.FromJson<ActionGraphData>(asset.text);
                pGraph.pAsset = asset;

                ActionStateManager.getInstance().AddActionGraph(asset.name, pGraph);
                return pGraph;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
                EditorUtility.DisplayDialog("提示", "不是一个有效的动作脚本你资源！", "请确认");
                return null;
            }
        }
        //-----------------------------------------------------
        public void Load(ActionGraphData pGrah, bool bClear = true)
        {
            if (m_pActor == null || pGrah == null) return;

            if (m_AniPlayback != null) m_AniPlayback.Clear();

            m_pActor.SetActionStateGraph(ActionStateManager.getInstance().CreateActionStateGraph(pGrah, m_pGameModuel, true));
        }
        //-----------------------------------------------------
        AnimatorState FindAnimatorState(string stateName, List<AnimatorStateData> vStates = null, string prefabName = null)
        {
            if (vStates == null) vStates = m_vAnimationState;
            for (int i = 0; i < vStates.Count; ++i)
            {
                if (vStates[i].state == null) continue;

                if (vStates[i].state.name.Equals(stateName, StringComparison.OrdinalIgnoreCase))
                {
                    return vStates[i].state;
                }
                if (!string.IsNullOrEmpty(prefabName))
                {
                    if (vStates[i].state.name.Equals(prefabName + "_" + stateName, StringComparison.OrdinalIgnoreCase))
                    {
                        return vStates[i].state;
                    }
                }
            }
            return null;
        }
        //-----------------------------------------------------
        public void BuildPlayableGraph()
        {
            if (m_pActor == null || m_pActor.GetActionStateGraph() == null) return;
            if (m_pActorPrefab != null)
            {
                Animator animator = m_pActorPrefab.GetComponent<Animator>();
                if (animator == null)
                    animator = m_pActorPrefab.GetComponentInChildren<Animator>();
                if (animator == null) return;

                ActionGraphBinder pBinder = m_pActorPrefab.GetComponent<ActionGraphBinder>();
                if (pBinder == null)
                    pBinder = m_pActorPrefab.AddComponent<ActionGraphBinder>();
          //      pBinder.avatarMasks = null;
           //     pBinder.states = null;
           //     List<ActionGraphBinder.State> vAnimatorStates = new List<ActionGraphBinder.State>();
                List<AvatarMask> vMasks = new List<AvatarMask>();

                AnimatorController controller = (AnimatorController)animator.runtimeAnimatorController;
                if (controller == null)
                    controller = m_AnimatorController;
                if(controller == null)
                {
                    EditorUtility.DisplayDialog("提示", "请在【基础信息】面板中先绑定控制器后继续", "好的");
                    return;
                }
                for (int i = 0; i < controller.layers.Length; ++i)
                {
                    vMasks.Add(controller.layers[i].avatarMask);
                }
                //m_pActor.GetActionStateGraph().SetUsedPlayableGraph(true);
                //HashSet<uint> vSets = new HashSet<uint>();
                //foreach ( var db in m_pActor.GetActionStateGraph().GetActionStateIDMap())
                //{
                //    if (!db.Value.IsActionOverride()) continue;
                //    string animation = db.Value.GetCore().animation;
                //    if (vSets.Contains(db.Key)) continue;
                //    vSets.Add(db.Key);
                //    AnimatorState pState= FindAnimatorState(animation);
                //    if (pState != null)
                //    {
                //        ActionGraphBinder.State state = new ActionGraphBinder.State();
                //        state.name = db.Value.GetCore().name;
                //        state.actionId = db.Value.GetCore().id;
                //        state.avatarMask = db.Value.IsActionOverride();
                //        state.layer = db.Value.GetLayer();
                //        state.blendParameter = null;
                //        List<ActionClipTrack> tracks = new List<ActionClipTrack>();
                //        if (pState.motion is BlendTree)
                //        {
                //            BlendTree tree = pState.motion as BlendTree;
                //            state.blendParameter = tree.blendParameter;
                //            for (int j = 0; j < tree.children.Length; ++j)
                //            {
                //                if (tree.children[j].motion == null || !(tree.children[j].motion is AnimationClip)) continue;
                //                ActionClipTrack track = new ActionClipTrack();
                //                track.threshold = tree.children[j].threshold;
                //                track.clip = tree.children[j].motion as AnimationClip;
                //                if (tree.children.Length == 3)
                //                {
                //                    if(Mathf.Abs(tree.children[j].threshold)<=0.001f) track.main = true;
                //                    else track.main = false;
                //                }
                //                else
                //                {
                //                    if (j == 0)
                //                        track.main = true;
                //                    else
                //                        track.main = false;
                //                }
 
                //                tracks.Add(track);
                //            }
                //            tracks.Sort((ActionClipTrack l, ActionClipTrack r) => { return (int)(100 * (l.threshold - r.threshold)); });
                //        }
                //        else if (pState.motion is AnimationClip)
                //        {
                //            ActionClipTrack track = new ActionClipTrack();
                //            track.main = true;
                //            track.clip = pState.motion as AnimationClip;
                //            tracks.Add(track);
                //        }
                //        if (tracks.Count > 0)
                //        {
                //            state.clipTracks = tracks.ToArray();
                //            vAnimatorStates.Add(state);

                //            db.Value.playableState = state;
                //        }
                //    }
                //}

             //   pBinder.states = vAnimatorStates.ToArray();
            //    pBinder.avatarMasks = vMasks.ToArray();
                EditorUtility.SetDirty(m_pActorPrefab);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
        //-----------------------------------------------------
        public void New()
        {
            string path = EditorUtility.SaveFilePanel("新建", Application.dataPath + ActionStateManager.SAVE_PATH_ROOT, "AG", "json");
            if (string.IsNullOrEmpty(path)) return;
            path = path.Replace("\\", "/");

            path = path.Replace(Application.dataPath, "Assets");
            if(!path.Contains(ActionStateManager.SAVE_PATH_ROOT))
            {
                UnityEditor.EditorUtility.DisplayDialog("提示", "存储路径必须是在" + ActionStateManager.SAVE_PATH_ROOT + " 下", "请确认");
                return;
            }
            string fileName = Path.GetFileNameWithoutExtension(path);
            if (fileName.Length <= 0)
            {
                EditorUtility.DisplayDialog("警告", "请输入一个有效文件名", "ok");
                return;
            }
            if (File.Exists(path))
            {
                if (!EditorUtility.DisplayDialog("警告", "改文件已存在，是否确定替换它", "是", "否"))
                {
                    return;
                }
            }
            if(m_pActor!=null)
                m_pActor.SetActionStateGraph(ActionStateManager.getInstance().CreateEmptyActionStateGraph(path));
            else
            {
                ActionStateManager.getInstance().SaveActionStateGraph(ActionStateManager.getInstance().CreateEmptyActionStateGraph(path));
            }
        }
        //-----------------------------------------------------
        public void Select()
        {
            string path = EditorUtility.OpenFilePanel("选择", Application.dataPath + ActionStateManager.SAVE_PATH_ROOT, "json");
            if (string.IsNullOrEmpty(path)) return;
            path = path.Replace("\\", "/");

            path = path.Replace(Application.dataPath, "Assets");
            string fileName = Path.GetFileNameWithoutExtension(path);
            if (fileName.Length <= 0)
            {
                EditorUtility.DisplayDialog("警告", "请选择有效文件名", "ok");
                return;
            }
            if (!path.Contains(ActionStateManager.SAVE_PATH_ROOT))
            {
                UnityEditor.EditorUtility.DisplayDialog("提示", "存储路径必须是在" + ActionStateManager.SAVE_PATH_ROOT + " 下", "请确认");
                return;
            }
            ActionGraphData pGraph = LoadGraph(path);
            m_pActor.SetActionStateGraph(ActionStateManager.getInstance().CreateActionStateGraph(pGraph, m_pGameModuel));
        }
        //-----------------------------------------------------
        void OnSelectChange()
        {
            ClearStateFrames();
            OnChangeGraphStand();
            m_pActor = null;
            m_pDummyActor = null;
            m_strModeFile = "";

            m_pGameModuel.world.ClearWorld();
            CsvData_Player PlayerCsv = DataEditorUtil.GetTable<CsvData_Player>(false);
            CsvData_Player.PlayerData pDummyData = PlayerCsv.datas.ElementAt(0).Value;

            m_pDummyActor = m_pGameModuel.world.CreateNode(EActorType.Player, pDummyData, 0, null, true) as Actor;
            if (m_pDummyActor != null)
            {
                m_pDummyActor.SetAttackGroup(1);
                m_pDummyActor.GetActorParameter().SetBasseAttr(EAttrType.AttackSpeedRate, 1);
                if (m_pDummyActor.GetActionStateGraph() != null && m_pDummyActor.GetObjectAble() != null)
                {
                    AActionGraphBinder varBinder = m_pDummyActor.GetObjectAble().GetComponent<AActionGraphBinder>();
                    if (varBinder != null) m_pDummyAniPrevew.bUseGraphPlayable = varBinder.usePlayableGraph;
                    else m_pDummyAniPrevew.bUseGraphPlayable = false;
                    m_pDummyAniPrevew.SetTarget(m_pDummyActor.GetObjectAble().gameObject, m_pDummyActor.GetActionStateGraph().GetActionState(EActionStateType.Idle, 0), null);
                }
                m_pDummyActor.SetFinalPosition(m_DummyOffset);
                m_pDummyActor.SetDirectionImmediately(Vector3.back);
                m_pDummyActor.StartActionByType(EActionStateType.Idle, 0, 1, true, false, true);
            }
            if (m_nSelectIndex >= 0 && m_nSelectIndex < m_PopUserDatas.Count)
            {
                PopData popData = m_PopUserDatas[m_nSelectIndex];
                m_strModeFile = popData.strFile;
                if (string.IsNullOrEmpty(m_strModeFile))
                {
                    m_pEditor.ShowNotification(new GUIContent("角色预制文件不存在"), 1);
                    //EditorKits.PopMessageBox("提示", "角色预制文件不存在", "请确认");
                }
              //  else
                {
                    if (popData.pData is CsvData_Player.PlayerData)
                    {
                        m_pActor = m_pGameModuel.world.CreateNode(popData.bHigh ? EActorType.PreviewActor : EActorType.Player, popData.pData,0, null, true) as Actor;
                        if (m_pActor != null)
                        {
                            if (m_pActor.GetObjectAble()!=null)
                            {
                                if (SceneView.lastActiveSceneView != null)
                                {
                                    EditorKits.SceneViewLookat(m_pActor.GetObjectAble().GetTransorm());
                                }
                            }
                            SetTarget(AssetDatabase.LoadAssetAtPath<GameObject>(m_strModeFile));
                        }
                    }
                    else if (popData.pData is CsvData_Monster.MonsterData)
                    {
                        m_pActor = m_pGameModuel.world.CreateNode(EActorType.Monster, popData.pData, 0, null, true) as Actor;
                        if (m_pActor != null)
                        {
                            m_pActor.SetAttackGroup(0);
                            if (m_pActor.GetObjectAble()!=null)
                            {
                                if (SceneView.lastActiveSceneView != null)
                                {
                                    EditorKits.SceneViewLookat(m_pActor.GetObjectAble().GetTransorm());
                                }
                            }
                            SetTarget(AssetDatabase.LoadAssetAtPath<GameObject>(m_strModeFile));
                        }
                    }
                    else if (popData.pData is CsvData_Summon.SummonData)
                    {
                        m_pActor = m_pGameModuel.world.CreateNode(EActorType.Summon, popData.pData, 0, null, true) as Actor;
                        if (m_pActor != null)
                        {
                            if ( m_pActor.GetObjectAble() != null)
                            {
                                if (SceneView.lastActiveSceneView != null)
                                {
                                    EditorKits.SceneViewLookat(m_pActor.GetObjectAble().GetTransorm());
                                }
                            }
                            SetTarget(AssetDatabase.LoadAssetAtPath<GameObject>(m_strModeFile));
                        }
                    }
                }
            }
            if (m_pActor != null)
            {
                m_pActor.SetDirection(Vector3.forward);
                m_pActor.GetActorParameter().SetBasseAttr(EAttrType.AttackSpeedRate, 1);
                m_pActor.EnableTerrainHit(false);
            }
        }
        //-----------------------------------------------------
        public void OnWorldNodeStatus(AWorldNode pNode, EWorldNodeStatus status, VariablePoolAble userVariable = null)
        {
            if (status == EWorldNodeStatus.Destroy)
            {
                if (pNode == m_pActor)
                {
                    m_pActor = null;
                }
            }
            else if(status == EWorldNodeStatus.Create)
            {
                if(m_pActor == pNode)
                {
                    if (pNode.GetObjectAble() != null)
                    {
                        if (SceneView.lastActiveSceneView != null)
                        {
                            EditorKits.SceneViewLookat(pNode.GetObjectAble().GetTransorm());
                        }
                    }
                    SetTarget(AssetDatabase.LoadAssetAtPath<GameObject>(m_strModeFile));
                }
            }
        }
        //-----------------------------------------------------
        public void Play(bool bPlay)
        {
            if (m_AniPlayback == null) return;
            if (m_pActor != null)
            {
                m_pActor.ResetMomentum();
                m_pActor.SetPosition(Vector3.zero);
                m_pActor.SetDirection(Vector3.forward);

                Framework.ED.DrawEventCore.PlayPreviews(false);
                Framework.ED.DrawEventCore.PreviewUpdates(0, m_pActor.GetObjectAble().transform);
            }
            if (m_pDummyActor != null)
            {
                m_pDummyActor.ResetMomentum();
                m_pDummyActor.SetPosition(m_pActor.GetPosition()+ m_DummyOffset);
                m_pDummyActor.SetDirectionImmediately(Vector3.back);
                m_pDummyActor.StartActionByType(EActionStateType.Idle, 0, 1, true, false, true);
            }
            if (m_pCurActionState != null)
            {
                m_pCurActionState.ResetRuntime();
                m_pCurActionState.isEditorPreview = true;
                if (bPlay)
                {
                    DestroyParticles();
                    if (m_pActor != null && m_pActor.GetActorAgent() != null)
                    {
                        m_pActor.GetActorAgent().AddLockTarget(m_pDummyActor, true);
                    }
                    List<BaseEventParameter> vGlobalEvents;
                    ActionStateManager.getInstance().GlobalStateEvents.TryGetValue(ActionStateManager.BuildActionKey(m_pCurActionState.GetCore().type, m_pCurActionState.GetCore().tag), out vGlobalEvents);
                    m_pCurActionState.RefreshEvent(vGlobalEvents);
                    m_pCurActionState.RebuildActionStatePropertyMap();
                    m_pCurActionState.ReplaceDoCnt(m_nPlayActionCnt);
                    m_pActor.StartActionState(m_pCurActionState, 0, 1, true, false, true);
                }
                else
                    m_pActor.StopCurrentActionState();
            }
            m_AniPlayback.Play(bPlay);
        }
        //-----------------------------------------------------
        public void DrawPlaySwitch()
        {
            if (m_pCurActionState == null || m_pCurActionState.GetCore().seqType != EAnimSeqType.CountSwitch) return;
            m_nPlayActionCnt = EditorGUILayout.IntField("次数分支", m_nPlayActionCnt);
        }
        //-----------------------------------------------------
        public bool isPlay()
        {
            if (m_AniPlayback == null) return false;
            return m_AniPlayback.bPlay;
        }
        //-----------------------------------------------------
        public void SetPlayTime(float time)
        {
            if (m_AniPlayback == null) return;
            m_AniPlayback.SetPlayTime(time);
        }
        //-----------------------------------------------------
        public float GetPlayTime()
        {
            if (m_AniPlayback == null) return 0;
            return m_AniPlayback.RuningTime;
        }
        //------------------------------------------------------
        public ActionState GetCurAction()
        {
            return m_pCurActionState;
        }
        //-----------------------------------------------------
        public float GetMaxFrameTime()
        {
            if (m_AniPlayback == null) return 0;
            return m_AniPlayback.nTotalFrame;
        }
        //-----------------------------------------------------
        public float GetDurationTime()
        {
            if (m_pCurActionState == null) return 0;
            return m_pCurActionState.GetDuration();
        }
        //-----------------------------------------------------
        void RefreshLocalAnimation(AActionGraphBinder binder, string prefabName)
        {
            if(!binder.usePlayableGraph)
            {
                return;
            }
            m_vAnimations.Clear();
            m_vAnimationState.Clear();

            m_vAnimationState = ActionBatchCreateDefault.RefreshLocalAnimation(binder, prefabName);
            foreach(var db in m_vAnimationState)
            {
                string name = db.state.name;
                if(db.state.motion!=null && db.state.motion is AnimationClip)
                {
                    name += "-" + ((AnimationClip)db.state.motion).length;
                }
                m_vAnimations.Add(name);
            }
            m_vAnimationState.Add(new AnimatorStateData() { state = null });
            m_vAnimations.Add("None");
        }
        //-----------------------------------------------------
        void SetTarget(GameObject target)
        {
            ClearTarget();
            m_pActorPrefab = target;
            if (m_pActorPrefab == null) return;
            m_pGraphBinder = m_pActorPrefab.GetComponent<ActionGraphBinder>();
            if(m_pGraphBinder == null)
                m_pGraphBinder = m_pActorPrefab.AddComponent<ActionGraphBinder>();
            m_AniPlayback.bUseGraphPlayable = m_pGraphBinder.usePlayableGraph;

            m_vAnimations.Clear();
            m_vAnimationState.Clear();
            m_AvatarMasks = null;

            m_vChildTransformSlotsPop.Clear();
            m_vFullNameChildTransformSlotsPop.Clear();
            m_vChildTransformSlots.Clear();

            m_vParameters.Clear();
            m_vParameterHashs.Clear();
            m_Animator = m_pActorPrefab.GetComponent<Animator>();
            if (m_Animator == null)
                m_Animator = m_pActorPrefab.GetComponentInChildren<Animator>();
            if (m_pGraphBinder.usePlayableGraph)
            {
                RefreshLocalAnimation(m_pGraphBinder, m_pActorPrefab.name);
            }
            else
            {
                if (m_Animator != null)
                {
                    m_AnimatorController = (AnimatorController)m_Animator.runtimeAnimatorController;
                    if (m_AnimatorController == null)
                    {
                        EditorUtility.DisplayDialog("提示", "控制器为空！", "好的");
                    }
                    if (m_AnimatorController != null)
                    {
                        BuildAnimatorStates(m_AnimatorController, ref m_vAnimationState, true);

                        for (int i = 0; i < m_AnimatorController.parameters.Length; ++i)
                        {
                            m_vParameters.Add(m_AnimatorController.parameters[i].name);
                            m_vParameterHashs.Add(m_AnimatorController.parameters[i].nameHash);
                        }
                    }
                }
                m_vAnimationState.Add(new AnimatorStateData() { state = null });
                m_vAnimations.Add("None");
            }

            if (m_pActor.GetObjectAble() != null)
                EditorKits.FindTransforms(m_pActor.GetObjectAble().GetTransorm(), ref m_vChildTransformSlots, ref m_vChildTransformSlotsPop,ref m_vFullNameChildTransformSlotsPop, "");

            m_vSlotPop.Add("None");
            if(m_pActor.GetActionStateGraph()!=null)
            {
                Dictionary<string, Slot> vSlots = m_pActor.GetActionStateGraph().GetSlots();
                foreach (var db in vSlots)
                {
                    if (db.Value.IsValid)
                        m_vSlotPop.Add(db.Value.name);
                }
            }

            m_bExpandListenSlot = "";

            RefreshGraphsStand(m_pGraphBinder);
        }
        //-----------------------------------------------------
        void BuildAnimatorStates(AnimatorController controller, ref List<AnimatorStateData> vStates, bool bBuildPop= true)
        {
            for (int i = 0; i < controller.layers.Length; ++i)
            {
                List<AnimatorState> states;
                if (controller.layers[i].syncedLayerIndex >= 0 && controller.layers[i].syncedLayerIndex < controller.layers.Length)
                {
                    states = EditorKits.GetStatesRecursive(controller.layers[controller.layers[i].syncedLayerIndex].stateMachine, true);
                }
                else
                    states = EditorKits.GetStatesRecursive(controller.layers[i].stateMachine, true);
                foreach (var st in states)
                {
                    if (st == null || st.motion == null) continue;
                    AnimatorStateData db = new AnimatorStateData();
                    db.layer = i;
                    db.layerPtr = controller.layers[i];
                    db.layerName = db.layerPtr.name;
                    db.state = st;
                    if (st.motion != null)
                        vStates.Add(db);
                }
            }
            m_vAnimationState.Sort((AnimatorStateData l, AnimatorStateData r) => { return l.layer - r.layer; });
            if (bBuildPop)
            {
                foreach (var db in m_vAnimationState)
                {
                    if (db.state.motion)
                    {
                        if (db.state.motion is AnimationClip)
                            m_vAnimations.Add("["+db.layerName + "]" + db.state.name + "---" + (db.state.motion as AnimationClip).length);
                        else if (db.state.motion is BlendTree)
                            m_vAnimations.Add("[" + db.layerName + "]" + db.state.name + "---" + (db.state.motion as BlendTree).averageDuration);
                    }
                    else
                        m_vAnimations.Add("["+db.layerName + "]" + db.state.name + "---null");
                }
            }
        }
        //-----------------------------------------------------
        public void Update(float fFrameTime)
        {
            if(!m_JumpTest.bSimulateJump)
            {
                if (m_AniPlayback != null) m_AniPlayback.Update(0.0333f*m_pEditor.GetCurrentFrameScale());
            }
            else
            {
                m_JumpTest.Update(m_pActor, 0.0333f * m_pEditor.GetCurrentFrameScale());
            }

            // m_pGameModuel.Update(fFrameTime);

            if (m_pActor != null && m_pActor.GetActionStateGraph()!=null)
            {
                m_pActor.SetScale(m_pActor.GetActionStateGraph().GetObjectScale()*Vector3.one);
            }

            if (m_pDummyActor != null)
            {
                m_pDummyActor.SetDirectionImmediately(Vector3.back);
                m_pDummyActor.SetAttackGroup((byte)m_nDummyAttackGroup);
                m_pDummyAniPrevew.SetCurState(m_pDummyActor.GetCurrentActionState());
                if(!m_pDummyAniPrevew.bPlay)
                {
                    m_pDummyAniPrevew.Play(true);
                    m_pDummyAniPrevew.bPlay = true;
                }
                m_pDummyAniPrevew.Update(fFrameTime);
                if (m_pDummyActor.IsKilled())
                {
                    m_pDummyActor.GetActorParameter().ResetRunTimeParameter();
                    m_pDummyActor.StartActionByType(EActionStateType.Revive, 0, 1, true, false, true);
                    m_pDummyActor.SetKilled(false);
                }
            }

            if (m_pActor != null)
            {
                float eulerOffset = 0;
                if (m_pCurActionState!=null && m_pCurActionState.GetCore().face_front_back)
                    eulerOffset = m_pCurActionState.GetCore().face_front_back_angle;
                m_pActor.SetEulerAngleImmediately(new Vector3(0, fActorEulerAngle+ eulerOffset, 0));
                ActorEditorAgent agent = m_pActor.GetActorAgent() as ActorEditorAgent;
                if (agent != null)
                {
                    agent.SetDummyTarget(m_pDummyActor);
                }

                if (Framework.Module.ModuleManager.mainModule == null ||
                    !(Framework.Module.ModuleManager.mainModule is AFrameworkModule) ||
                    ((AFrameworkModule)Framework.Module.ModuleManager.mainModule).cameraController == null)
                {
                    if(m_pActor.GetObjectAble() != null)
                        Framework.ED.DrawEventCore.PreviewUpdates(fFrameTime, m_pActor.GetObjectAble().transform);
                }
            }

            if (SceneView.lastActiveSceneView != null)
                SceneView.lastActiveSceneView.Repaint();
            //     ((EditorCameraController)m_pGameModuel.cameraController).SetCamera(m_preview.GetCamera());

            //  m_preview.SetCameraPositionAndEulerAngle(SceneView.lastActiveSceneView.camera.transform.position, SceneView.lastActiveSceneView.camera.transform.eulerAngles);
            if (m_JumpTest.bSimulateJump && curTab == ETabType.BaseInfo)
            {
                if(m_pActor.GetGroundType() == EActorGroundType.Ground)
                {
                    if(m_pActor.GetPosition().y > 0)
                        m_pActor.SetGroundType(EActorGroundType.Air);
                }
                else if (m_pActor.GetGroundType() == EActorGroundType.Air)
                {
                    if (m_pActor.GetPosition().y <= 0)
                    {
                        m_pActor.EnableCollisionFlag(ECollisionFlag.DOWN,true);
                    }
                }
                if (m_pActor.CanDoGroundAction())
                    m_pActor.StartActionByType(EActionStateType.Run, 0, 1, false, false, true);
            }

            if (m_pCurActionState != null)
                m_pCurActionState.isEditorPreview = true;

            UpdateParticle(fFrameTime);
        }
        //-----------------------------------------------------
        void OnHierarchyDirtyChange()
        {
            if (Application.isPlaying) return;
            EditorApplication.hierarchyChanged -= OnHierarchyDirtyChange;
            ParticleController[] parCtls = GameObject.FindObjectsOfType<ParticleController>();
            for(int i = 0; i < parCtls.Length; ++i)
            {
                if(parCtls[i].transform.childCount <=0)
                {
                    GameObject.DestroyImmediate(parCtls[i].gameObject);
                }
            }
            EditorApplication.hierarchyChanged += OnHierarchyDirtyChange;

            ParticleSystem[] systems = GameObject.FindObjectsOfType<ParticleSystem>();
            if (systems == null) return;

            HashSet<Animator> vAnimtorTest = new HashSet<Animator>();
            HashSet<ParticleSystem> vTest = new HashSet<ParticleSystem>();
            foreach (var db in m_vParSimulates)
            {
                if (db.parSystem)
                    vTest.Add(db.parSystem);
                if (db.parAnimator) vAnimtorTest.Add(db.parAnimator);
            }
            HashSet<GameObject> vObjs = new HashSet<GameObject>();
            for (int i = 0; i < systems.Length; ++i)
            {
                if (!vTest.Contains(systems[i]))
                {
                    ParSimulate parSim = new ParSimulate();
                    parSim.parSystem = systems[i];
                    parSim.simulateTime = 0;
                    parSim.durationTime = ParticleUtils.GetParticleStartDelay(systems[i]) + systems[i].main.duration * 1.5f;
                    m_vParSimulates.Add(parSim);
                }
                Transform temp = EditorUtil.GetTransformTopParent(systems[i].transform);
                if(temp!=null && !vObjs.Contains(temp.gameObject))
                {
                    vObjs.Add(temp.gameObject);
                }
            }
            foreach(var db in vObjs)
            {
                Animator[] animators= db.GetComponentsInChildren<Animator>();
                for(int i=0; i < animators.Length; ++i)
                {
                    if (!vAnimtorTest.Contains(animators[i]))
                    {
                        ParSimulate parSim = new ParSimulate();
                        parSim.parSystem = null;
                        parSim.parAnimator = animators[i];
                        parSim.simulateTime = 0;
                        parSim.durationTime = 20;
                        m_vParSimulates.Add(parSim);
                    }
                }
            }
            for (int i = 0; i < systems.Length; ++i)
            {
                vTest.Remove(systems[i]);
            }
            EditorApplication.hierarchyChanged -= OnHierarchyDirtyChange;
            foreach (var db in vTest)
            {
                GameObject.DestroyImmediate(db.gameObject);
            }
            EditorApplication.hierarchyChanged += OnHierarchyDirtyChange;
        }
        //-----------------------------------------------------
        void UpdateParticle(float fFrame)
        {
            if (Application.isPlaying) return;
            fFrame = 0.03333f*1.08f;
            ParticleSystem[] systems = GameObject.FindObjectsOfType<ParticleSystem>();
            if (systems == null) return;
            ParSimulate parSim;
            for (int i =0; i < m_vParSimulates.Count; )
            {
                parSim = m_vParSimulates[i];
                if(parSim.parSystem == null && parSim.parAnimator == null)
                {
                    m_vParSimulates.RemoveAt(i);
                    continue;
                }
                if(parSim.parSystem!=null)
                {
                    if (parSim.simulateTime >= parSim.durationTime && parSim.simulateTime > m_AniPlayback.RuningTime * 2)
                    {
                        EditorApplication.hierarchyChanged -= OnHierarchyDirtyChange;
                        GameObject.DestroyImmediate(parSim.parSystem.gameObject);
                        EditorApplication.hierarchyChanged += OnHierarchyDirtyChange;
                        m_vParSimulates.RemoveAt(i);
                        continue;
                    }
                    parSim.simulateTime += fFrame;
                    parSim.parSystem.Play();
                    parSim.parSystem.Simulate(parSim.simulateTime);
                }
                else
                {
                    parSim.simulateTime += fFrame;
                    if (parSim.simulateTime >= parSim.durationTime && parSim.simulateTime > m_AniPlayback.RuningTime * 2)
                    {
                        m_vParSimulates.RemoveAt(i);
                        continue;
                    }
                    parSim.parAnimator.Update(fFrame);
                }
                ++i;
            }
        }
        //-----------------------------------------------------
        void DestroyParticles()
        {
            EditorApplication.hierarchyChanged -= OnHierarchyDirtyChange;
            HashSet<GameObject> vTest = new HashSet<GameObject>();
            foreach (var db in m_vParSimulates)
            {
                if (db.parSystem)
                {
                    Transform transform = EditorUtil.GetTransformTopParent(db.parSystem.transform);
                    if (transform) vTest.Add(transform.gameObject);
                }
                else if(db.parAnimator)
                {
                    Transform transform = EditorUtil.GetTransformTopParent(db.parAnimator.transform);
                    if(transform) vTest.Add(transform.gameObject);
                }
            }
            m_vParSimulates.Clear();
            foreach (var db in vTest)
            {
                if (m_pActor!=null && m_pActor.GetObjectAble() != null && db.gameObject == m_pActor.GetObjectAble().gameObject) continue;
                if (Application.isPlaying)
                    GameObject.Destroy(db.gameObject);
                else GameObject.DestroyImmediate(db.gameObject);
            }
            ParticleController[] parCtls = GameObject.FindObjectsOfType<ParticleController>();
            for (int i = 0; i < parCtls.Length; ++i)
            {
                GameObject.DestroyImmediate(parCtls[i].gameObject);
            }
            EditorApplication.hierarchyChanged += OnHierarchyDirtyChange;
        }
        //-----------------------------------------------------
        public void OnEvent(Event evt)
        {
            if (evt.type == EventType.KeyDown)
            {
                if(evt.keyCode == KeyCode.Escape)
                {
                    m_bCopyVolume = false;
                }
            }
        }
        //-----------------------------------------------------
        public void SaveAlls()
        {
            EditorUtility.DisplayProgressBar("save", "", 0);
            for(int a =0; a < m_PopUserDatas.Count; ++a)
            {
                EditorUtility.DisplayProgressBar("save", "", (float)a / (float)m_PopUserDatas.Count);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(m_PopUserDatas[a].strFile);
                if (prefab == null) continue;

                var pStateGraph = ActionStateManager.getInstance().CreateActionStateGraph(m_PopUserDatas[a].graphFile, m_pGameModuel);
                if (pStateGraph == null) continue;
                ActionGraphBinder pBinder = prefab.GetComponent<ActionGraphBinder>();
                if (pBinder != null && !(pBinder is ActionGraphBinder))
                {
                    GameObject.DestroyImmediate(pBinder, true);
                    pBinder = null;
                }
                if (pBinder == null)
                    pBinder = prefab.AddComponent<ActionGraphBinder>();
                pBinder.usePlayableGraph = true;
                RefreshLocalAnimation(pBinder, prefab.name);

                pBinder.playables = null;
                HashSet<int> vHashs = new HashSet<int>();
                List<PlayableState> vStates = pBinder.playables != null ? new List<PlayableState>(pBinder.playables) : new List<PlayableState>();
                foreach (var db in vStates)
                {
                    vHashs.Add(db.nameHash);
                }
                foreach (var db in pStateGraph.GetActionStateIDMap())
                {
                    if (db.Value.GetCore().animations == null)
                        continue;
                    for (int i = 0; i < db.Value.GetCore().animations.Length; ++i)
                    {
                        var anim = db.Value.GetCore().animations[i];
                        var state = FindAnimatorState(anim.animation, null, prefab.name);
                        if (state != null)
                        {
                            if (vHashs.Contains(anim.hashState))
                                continue;
                            vHashs.Add(anim.hashState);
                            PlayableState playableState = new PlayableState();
                            playableState.nameHash = anim.hashState;
                            playableState.clipTracks = new ActionClipTrack[] { new ActionClipTrack() { clip = state.motion as AnimationClip } };
                            playableState.name = state.name;
                            playableState.layer = anim.layer;
                            playableState.loadType = db.Value.GetCore().animLoadType;
                            playableState.canClamp = db.Value.GetCore().can_clamp;
                            string stateName = state.name.ToLower();
                            if (prefab.name.Contains("_H"))
                            {
                                //! 如果是高模
                                if (stateName.Contains("standby") || stateName.Contains("idle"))
                                {
                                    playableState.loadType = EAnimLoadType.BinderRef;
                                    playableState.isDefault = true;
                                    playableState.clipTracks = new ActionClipTrack[] { new ActionClipTrack() { clip = state.motion as AnimationClip } };
                                }
                                else
                                {
                                    playableState.loadType = EAnimLoadType.RequireLoad;
                                    playableState.clipTracks = new ActionClipTrack[] { new ActionClipTrack() { clipFile = AssetDatabase.GetAssetPath(state.motion) } };
                                }
                            }
                            else
                            {
                                if (stateName.Contains("standby") || stateName.Contains("idle") || stateName.Contains("run"))
                                {
                                    playableState.loadType = EAnimLoadType.BinderRef;
                                    if (stateName.Contains("standby") || stateName.Contains("idle"))
                                        playableState.isDefault = true;
                                    playableState.clipTracks = new ActionClipTrack[] { new ActionClipTrack() { clip = state.motion as AnimationClip } };
                                }
                                else
                                {
                                    if (db.Value.GetCore().type == EActionStateType.AttackAir || db.Value.GetCore().type == EActionStateType.AttackGround)
                                        playableState.loadType = EAnimLoadType.ActiveLoad;
                                    else
                                        playableState.loadType = EAnimLoadType.RequireLoad;
                                    playableState.clipTracks = new ActionClipTrack[] { new ActionClipTrack() { clipFile = AssetDatabase.GetAssetPath(state.motion) } };
                                }
                            }

                            vStates.Add(playableState);
                        }
                    }
                }
                pBinder.playables = vStates.ToArray();
                pBinder.layerAvatarMasks = m_AvatarMasks;
                Animator animator = prefab.GetComponent<Animator>();
                if (animator == null)
                    animator = prefab.GetComponentInChildren<Animator>();
                if (animator != null)
                {
                    animator.runtimeAnimatorController = null;
                }

                EditorUtility.SetDirty(prefab);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            EditorUtility.ClearProgressBar();
        }
        //-----------------------------------------------------
        public void SaveData(bool bSaveGraphFile=true, bool bSyncSaveRefresh = true)
        {
            if (m_pActor == null || m_pActor.GetActionStateGraph() == null)
            {
                ActionGraphAssetsEditor.RefreshSync(AssetDatabase.LoadAssetAtPath<ActionGraphAssets>(ActionStateManager.ACTION_GRAPH_ASSETS),false);
                return;
            }
            if (bSaveGraphFile)
            {
                ActionGraphData pData = ActionStateManager.getInstance().SaveActionStateGraph(m_pActor.GetActionStateGraph());
            }
            if(m_pActorPrefab != null)
            {
                List<Transform> vTransSlot = new List<Transform>();
                List<string> vTransSlotNames = new List<string>();
                List<string> vFullTransSlotNames = new List<string>();
                EditorKits.FindTransforms(m_pActorPrefab.transform, ref vTransSlot, ref vTransSlotNames, ref vFullTransSlotNames, "");

                ActionGraphBinder pBinder = m_pActorPrefab.GetComponent<ActionGraphBinder>();
                if(pBinder!=null && !(pBinder is ActionGraphBinder))
                {
                    GameObject.DestroyImmediate(pBinder, true);
                    pBinder = null;
                }
                if (pBinder == null)
                    pBinder = m_pActorPrefab.AddComponent<ActionGraphBinder>();
                pBinder.usePlayableGraph = true;
                RefreshLocalAnimation(pBinder, m_pActorPrefab.name);
//                 InstanceAbleHandler handler = null;
//                 AInstanceAble pAble = m_pActorPrefab.GetComponent<AInstanceAble>();
//                 if (pAble == null)
//                 {
//                     handler = pAble.GetComponent<InstanceAbleHandler>();
//                     if (handler == null)
//                     {
//                         handler = m_pActorPrefab.AddComponent<InstanceAbleHandler>();
//                     }
//                 }
//                 else handler = pAble as InstanceAbleHandler;
//                 if (handler) handler.AddWidget(pBinder, "binder");

                List<Slot> vSlots = new List<Slot>();
                foreach(var db in m_pActor.GetActionStateGraph().GetSlots())
                {
                    if (db.Value.IsValid)
                    {
                        Slot slot = db.Value;
                        if (slot.transform)
                        {
                            int idnex = vTransSlotNames.IndexOf(slot.transform.name);
                            if (idnex >= 0 && idnex < vTransSlot.Count)
                                slot.transform = vTransSlot[idnex];
                        }
                        if (slot.centroid)
                        {
                            int idnex = vTransSlotNames.IndexOf(slot.centroid.name);
                            if (idnex >= 0 && idnex < vTransSlot.Count)
                                slot.centroid = vTransSlot[idnex];
                        }
                        vSlots.Add(slot);
                    }
                }
                pBinder.Slots = vSlots.ToArray();
                List<Core.BodyPart> vParts = new List<Core.BodyPart>();
                for(int i = 0; i < vSlots.Count; ++i)
                {
                    if(!vSlots[i].IsValid) continue;
                    if (vSlots[i].transform)
                    {
                        bool IsBodyPart = false;
                        Core.BodyPart part = vSlots[i].transform.GetComponent<Core.BodyPart>();
                        List<StateFrame> frames = m_pActor.GetActionStateGraph().GetStateFrames();
                        for (int j = 0; j < frames.Count; ++j)
                        {
                            StateFrame frame = frames[j];
                            if (frame != null && frame.bodyPartID != 0xffffffff && frame.bind_slot.CompareTo(vSlots[i].name) == 0)
                            {
                                if (part == null) part = vSlots[i].transform.gameObject.AddComponent<Core.BodyPart>();
                                vParts.Add(part);
                                IsBodyPart = true;
                            }
                        }
                        if(!IsBodyPart && part)
                        {
                            UnityEngine.Object.DestroyImmediate(part, true);
                        }
                    }
                }
                pBinder.bodyParts = vParts.ToArray();

                SaveCatch();
                if(pBinder.usePlayableGraph)
                {
                    List<PlayableState> vBackStates = new List<PlayableState>();
                    if (pBinder.playables != null) vBackStates.AddRange(pBinder.playables);
                    HashSet<int> vSets = new HashSet<int>();
                    List<PlayableState> vStates = new List<PlayableState>();
                    foreach (var db in m_pActor.GetActionStateGraph().GetActionStateIDMap())
                    {
                        if (db.Value.GetCore().animations == null)
                            continue;
                        for(int i =0; i < db.Value.GetCore().animations.Length; ++i)
                        {
                            var anim = db.Value.GetCore().animations[i];
                            if(pBinder.usePlayableGraph)
                            {
                                if (db.Value.GetCore().bOverride)
                                {
                                    if (anim.layer <= 0) anim.layer = 1;
                                }
                            }
     
                             var state = FindAnimatorState(anim.animation);
                            if (state != null)
                            {
                                if(!vSets.Contains(anim.hashState))
                                {
                                    vSets.Add(anim.hashState);
                                    PlayableState playableState = new PlayableState();
                                    playableState.nameHash = anim.hashState;
                                    playableState.clipTracks = new ActionClipTrack[] { new ActionClipTrack() { clip = state.motion as AnimationClip } };
                                    playableState.name = anim.animation;
                                    playableState.layer = anim.layer;
                                    playableState.loadType = db.Value.GetCore().animLoadType;
                                    playableState.canClamp = db.Value.GetCore().can_clamp;
                                    if (m_pActorPrefab.name.Contains("_H"))
                                    {
                                        //! 如果是高模
                                        if (db.Value.GetCore().type == EActionStateType.Idle || db.Value.GetCore().type == EActionStateType.Run || playableState.loadType == EAnimLoadType.BinderRef)
                                        {
                                            playableState.loadType = EAnimLoadType.BinderRef;
                                            playableState.clipTracks = new ActionClipTrack[] { new ActionClipTrack() { clip = state.motion as AnimationClip } };
                                        }
                                        else
                                        {
                                            playableState.loadType = EAnimLoadType.RequireLoad;
                                            playableState.clipTracks = new ActionClipTrack[] { new ActionClipTrack() { clipFile = AssetDatabase.GetAssetPath(state.motion) } };
                                        }
                                    }
                                    else
                                    {
                                        if (db.Value.GetCore().type <= EActionStateType.Enter || playableState.loadType == EAnimLoadType.BinderRef)
                                        {
                                            playableState.loadType = EAnimLoadType.BinderRef;
                                            playableState.clipTracks = new ActionClipTrack[] { new ActionClipTrack() { clip = state.motion as AnimationClip } };
                                        }
                                        else
                                        {
                                            playableState.clipTracks = new ActionClipTrack[] { new ActionClipTrack() { clipFile = AssetDatabase.GetAssetPath(state.motion) } };
                                        }
                                    }

                                    if (db.Value.GetCore().type == EActionStateType.Idle)
                                        playableState.isDefault = true;
                                    vStates.Add(playableState);
                                }
                            }
                        }
                    }
                    foreach(var db in vBackStates)
                    {
                        if (vSets.Contains(db.nameHash)) continue;
                        vStates.Add(db);
                    }
                    pBinder.playables = vStates.ToArray();
                    pBinder.layerAvatarMasks = m_AvatarMasks;
                    Animator animator = m_pActorPrefab.GetComponent<Animator>();
                    if (animator == null) animator = m_pActorPrefab.GetComponentInChildren<Animator>();
                    if (animator!=null)
                    {
                        animator.runtimeAnimatorController = null;
                    }
                }      
                EditorUtility.SetDirty(m_pActorPrefab);
            }
            if(bSyncSaveRefresh)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }

            GUI.FocusControl("");
            if(bSaveGraphFile)
                ActionGraphAssetsEditor.RefreshSync(AssetDatabase.LoadAssetAtPath<ActionGraphAssets>(ActionStateManager.ACTION_GRAPH_ASSETS), false);
        }
        //------------------------------------------------------
        void SaveCatch()
        {
            if (m_pActorPrefab == null) return;
            string strTempFile = Application.dataPath + "/../EditorData/ActorCatchs/"+ m_pActorPrefab.name +".json";
            if (!Directory.Exists(Application.dataPath + "/../EditorData/ActorCatchs/"))
                Directory.CreateDirectory(Application.dataPath + "/../EditorData/ActorCatchs");

            ActionGraphBinder pBinder = m_pActorPrefab.GetComponent<ActionGraphBinder>();

            if(pBinder)
            {
                if (File.Exists(strTempFile))
                    File.Delete(strTempFile);
                string strCatch = JsonUtility.ToJson(pBinder, true);

                FileStream fs = new FileStream(strTempFile, FileMode.OpenOrCreate);
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
                writer.Write(JsonUtility.ToJson(strCatch, true));
                writer.Close();
            }
        }
        //------------------------------------------------------
        ActionGraphBinder ReadCatch()
        {
            if (m_pActorPrefab == null) return null;
//             string strTempFile = Application.dataPath + "/../EditorData/ActorCatchs/" + m_pActorPrefab.name + ".json";
//             if(File.Exists(strTempFile))
//             {
//                 return JsonUtility.FromJson<ActionGraphBinder>(File.ReadAllText(strTempFile));
//             }
            return null;
        }
        //-----------------------------------------------------
        public void RefreshGraphsStand(ActionGraphBinder pBinder)
        {
            m_AvatarMasks = pBinder? pBinder.layerAvatarMasks:null;
            m_PopSlots.Clear();
            m_PopSlots.Add("None");
            if (m_pActor == null || m_pActor.GetActionStateGraph() == null) return;

            if (pBinder == null)
                pBinder = ReadCatch();

            m_vStandPop.Clear();
            m_vStandPop.Add("None");
            foreach (var db in m_pActor.GetActionStateGraph().GetActionStateStanceMap())
            {
                m_vStandPop.Add(db.Key);
            }
            if (m_pActor.GetObjectAble() != null)
            {
                pBinder = (m_pActor.GetObjectAble() as AInstanceAble).GetComponent<ActionGraphBinder>();
            }
            if(pBinder!=null && pBinder.Slots != null)
            {
                m_pActor.GetActionStateGraph().SetSlots(pBinder.Slots);
                m_pActor.GetActionStateGraph().SetBodyParts(pBinder);
                foreach (var db in m_pActor.GetActionStateGraph().GetSlots())
                {
                    m_PopSlots.Add(db.Key);
                }
            }

            //ActionStateGraph pGraph = m_pActor.GetActionStateGraph();
            //if (pGraph != null)
            //{
            //    if (pBinder != null && pBinder.states != null)
            //    {
            //        for (int i = 0; i < pBinder.states.Length; ++i)
            //        {
            //            ActionState pState = pGraph.FindActionState(pBinder.states[i].actionId);
            //            if (pState != null)
            //            {
            //                pState.playableState = pBinder.states[i];
            //            }
            //        }
            //    }
            //}

            //if (pBinder != null)
            //    m_AvatarMasks = pBinder.avatarMasks;
        }
        //-----------------------------------------------------
        public void OnGUI()
        {
        }
        //-----------------------------------------------------
        public void DrawPreview(Rect rc)
        {
            if (m_preview != null)
                m_preview.OnPreviewGUI(rc, null);

            if (m_pActorPrefab == null)
            {
         //       m_pEditor.ShowNotification(new GUIContent("预制体为空,请检查资源"));
                return;
            }

            if (m_AniPlayback != null)
            {
                Color back = GUI.color;
                GUI.color = Color.red;
                EditorGUI.DropShadowLabel(new Rect(rc.x, rc.yMin, rc.width/2, 20f),
                    (rc.width > 140f ? "Play Time:" : string.Empty) + String.Format("{0:F}", m_AniPlayback.RuningTime));
                EditorGUI.DropShadowLabel(new Rect(rc.xMax- rc.width / 2, rc.yMin, rc.width/2, 20f),
                   (rc.width > 140f ? "Cur Frame:" : string.Empty) + String.Format("{0}", (int)(m_AniPlayback.GetPlayFrame())));

                EditorGUI.DropShadowLabel(new Rect(rc.xMin, rc.yMax - 30f, rc.width, 20f),
                    (rc.width > 140f ? "Cur Action:" : string.Empty) + String.Format("{0}", m_AniPlayback.GetStateName()));

                GUI.color = back;
            }
            EditorGUI.DropShadowLabel(new Rect(rc.xMax - rc.width / 2, rc.yMin+20, rc.width / 2, 20f),
                   (rc.width > 140f ? "跳跃最高高度:" : string.Empty) + String.Format("{0}", m_JumpTest.fJumpMostHeight));
        }
        //-----------------------------------------------------
        public void OnSceneGUI(int control, Camera camera)
        {
#if !UNITY_5_1
            UnityEngine.Rendering.CompareFunction zTest = Handles.zTest;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
#endif
            if (m_pActor != null)
            {
                Color color = Handles.color;

                if (m_pActor.GetActionStateGraph()!=null && m_pActor.GetActionStateGraph().GetPhysicsRadius() > 0)
                {
                    Handles.color = Color.yellow;
                    Handles.CircleHandleCap(control, m_pActor.GetPosition(), Quaternion.Euler(90, 0, 0), m_pActor.GetPhysicRadius(), EventType.Repaint);
                    Handles.color = color;
                }
                if (m_pCurActionState!=null)
                {
                    List<ActionEvent> vEvents = m_pCurActionState.GetActiveEvents();
                    if(vEvents!=null)
                    {
                        for(int i= 0; i < vEvents.Count; ++i)
                        {
                            if(vEvents[i].m_pCore== null || !vEvents[i].m_pCore.bExpand) continue;
                            vEvents[i].m_pCore.OnPreview(false, m_pActor);
                        }
                    }
                }

                if (m_pActor.GetActionStateGraph() != null)
                {
                    Quaternion rot = Quaternion.identity;
                    Dictionary<string, Slot> slots = m_pActor.GetActionStateGraph().GetSlots();
                    foreach (var db in slots)
                    {
                        Slot slot = db.Value;
                        if (slot.IsValid)
                        {
                            if (m_bExpandListenSlot !=null && m_bExpandListenSlot.CompareTo(slot.name) == 0)
                            {
                                Handles.color = Color.red;
                            }
                            else
                                Handles.color = Color.white;
                            Vector3 position = m_pActor.GetPosition();
                            Vector3 vRight = m_pActor.GetRight();
                            Vector3 vUp = m_pActor.GetUp();
                            Vector3 vDir = m_pActor.GetDirection();
                            Transform trans = slot.transform;
                            if (slot.centroid) trans = slot.centroid;
                            if (trans)
                            {
                                vRight = trans.right;
                                vUp = trans.up;
                                vDir = trans.forward;
                                position = trans.position;
                            }
                            position += slot.offset.x * vRight + slot.offset.y * vUp + slot.offset.z * vDir;
                            if (m_bExpandSlot)
                                Handles.SphereHandleCap(control, position, rot, Mathf.Min(2, HandleUtility.GetHandleSize(position)), EventType.Repaint);
                            Handles.Label(position, slot.name);
                        }
                    }
                    Handles.color = color;
                }
                if (m_pActor.GetActionStateGraph()!=null)
                {
                    Vector3 pos = m_pActor.GetPosition();
                    if(m_pActor.GetActionStateGraph().GetMaxAttackDistance()>0)
                    {
                        Handles.color = Color.red;
                        Handles.Label(pos + Vector3.forward * (m_pActor.GetActionStateGraph().GetMaxAttackDistance()), "最大攻击范围");
                        Handles.CircleHandleCap(control, pos, Quaternion.Euler(90, 0, 0), m_pActor.GetActionStateGraph().GetMaxAttackDistance(), EventType.Repaint);
                        Handles.color = color;
                    }
                    if (m_pActor.GetActionStateGraph().GetMinAttackDistance() > 0)
                    {
                        Handles.color = Color.red;
                        Handles.Label(pos + Vector3.forward * (m_pActor.GetActionStateGraph().GetMinAttackDistance() ), "最小攻击范围");
                        Handles.CircleHandleCap(control, pos, Quaternion.Euler(90, 0, 0), m_pActor.GetActionStateGraph().GetMinAttackDistance(), EventType.Repaint);
                        Handles.color = color;
                    }

                    if (m_pActor.GetActionStateGraph().GetMaxTargetDistance() > 0)
                    {
                        Handles.color = Color.green;
                        Handles.Label(pos + Vector3.up * 0.01f + Vector3.forward * m_pActor.GetActionStateGraph().GetMaxTargetDistance(), "激活范围");
                        Handles.CircleHandleCap(control, pos + Vector3.up * 0.01f, Quaternion.Euler(90, 0, 0), m_pActor.GetActionStateGraph().GetMaxTargetDistance(), EventType.Repaint);
                        Handles.color = color;
                    }
                    if (m_pActor.GetActionStateGraph().GetMaxTargetZoom() > 0)
                    {
                        float target = m_pActor.GetActionStateGraph().GetMaxTargetDistance();
                        if (target <= 0) target = 1;
                        Framework.Core.CommonUtility.DrawBoundingBox(Vector3.zero,
                            new Vector3(m_pActor.GetActionStateGraph().GetMaxTargetZoom(), 0.1f, target),
                            m_pActor.GetMatrix(),
                            Color.green, false);
                    }

                    if(curTab == ETabType.BaseInfo)
                    {
                        if (m_pActor.GetActionStateGraph().GetCollisionType() == EActorCollisionType.BOX)
                        {
                            Vector3 size = m_pActor.GetActionStateGraph().GetBoxCollisionExtrude();
                            Vector3 realSize = new Vector3(size.x * m_pActor.GetScale().x, size.y * m_pActor.GetScale().y, size.z * m_pActor.GetScale().z);
                            UnityEditor.Handles.DrawWireCube(m_pActor.GetPosition() + Vector3.up * realSize.y / 2, realSize);
                        }
                        else if (m_pActor.GetActionStateGraph().GetCollisionType() == EActorCollisionType.CAPSULE)
                        {
                            Vector3 realSize = m_pActor.GetScale() * m_pActor.GetActionStateGraph().GetCapsuleCollisionRadius();
                            UnityEditor.Handles.DrawWireCube(m_pActor.GetPosition() + Vector3.up * m_pActor.GetScale().y * m_pActor.GetActionStateGraph().GetCapsuleCollisionHeight() / 2, realSize);
                        }
                    }
                }

                m_JumpTest.Draw();                  
                ActionState pCur = null;
                if (m_AniPlayback.bPlay) pCur = m_pActor.GetCurrentActionState();
                else pCur = m_pCurActionState;
                if(pCur!=null)
                {
                    bool bMirror = pCur.IsMirrorVolume();
                    List<ActionFrame> actionFrameArray = new List<ActionFrame>();
                    if (!m_AniPlayback.bPlay)
                    {
                        pCur.UpdateActionFrameByTime(Mathf.Max( m_AniPlayback.RuningTime, 0.033f));
                    }
                    pCur.GetCurrentFrameArray(actionFrameArray);
                    AInstanceAble pTransAble = m_pActor.GetObjectAble() as AInstanceAble;
                    Transform pTrans = null;
                    if(pTransAble!=null) pTrans = pTransAble.GetTransorm();
                    for (int i = 0; i < actionFrameArray.Count; i++)
                    {
                        ActionFrame pFrame = actionFrameArray[i];
                        Matrix4x4 mtWorld = m_pActor.GetEventBindSlot(pFrame.bind_slot, pFrame.bindSlotBit);

                        RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.Target), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.Target), 1.0f);
                        RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.PartTarget), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.PartTarget), 1.0f);
                        RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.Attack), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.Attack), 1.001f);
                        RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.AttackPre), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.AttackPre), 1.002f);
                        RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.AttackInvert), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.AttackInvert), 1.004f);
                        RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.GrabAttack), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.GrabAttack), 1.004f);
                        RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.GrabTarget), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.GrabTarget), 1.004f);
                        RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.AttackGrabingTarget), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.AttackGrabingTarget), 1.004f);
                    }
                }
                else if(m_pActor.GetActionStateGraph()!=null && m_pActor.GetActionStateGraph().GetStateFrames() != null &&
                    m_nStateFrameIndex >=0 && m_nStateFrameIndex < m_pActor.GetActionStateGraph().GetStateFrames().Count )
                {
                    AInstanceAble pTransAble = m_pActor.GetObjectAble();
                    Transform pTrans = null;
                    if (pTransAble != null) pTrans = pTransAble.GetTransorm();
                    bool bMirror = false;
                    StateFrame pFrame = m_pActor.GetActionStateGraph().GetStateFrames()[m_nStateFrameIndex];
                    Matrix4x4 mtWorld = m_pActor.GetEventBindSlot(pFrame.bind_slot, pFrame.bindslotBit);


                    RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.Target, m_vTestVolumes), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.Target), 1.0f);
                    RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.PartTarget, m_vTestVolumes), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.PartTarget), 1.0f);
                    RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.Attack, m_vTestVolumes), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.Attack), 1.001f);
                    RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.AttackPre, m_vTestVolumes), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.AttackPre), 1.002f);
                    RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.AttackInvert, m_vTestVolumes), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.AttackInvert), 1.004f);
                    RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.GrabAttack, m_vTestVolumes), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.GrabAttack), 1.004f);
                    RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.GrabTarget, m_vTestVolumes), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.GrabTarget), 1.004f);
                    RenderVolumeByColor(pFrame.GetVolumeArrayByType(EVolumeType.AttackGrabingTarget, m_vTestVolumes), bMirror, mtWorld, EditorKits.GetVolumeToColor(EVolumeType.AttackGrabingTarget), 1.004f);
                }

                DrawPropertyCore.OnSceneGUI(m_pActor, control, camera);
                if(m_pActor.GetObjectAble()!=null) DrawEventCore.OnSceneGUI(Rect.zero, m_pActor.GetObjectAble().transform);
            }

            if (m_pDummyActor != null && m_pActor != null)
            {
                Vector3 preOffset = m_DummyOffset;
                m_DummyOffset = Handles.DoPositionHandle(m_DummyOffset + m_pActor.GetPosition(), Quaternion.identity) - m_pActor.GetPosition();
                if(!m_AniPlayback.bPlay && !Framework.Core.CommonUtility.Equal(preOffset, m_DummyOffset))
                {
                    m_pDummyActor.SetFinalPosition(m_DummyOffset + m_pActor.GetPosition());
                }
                Vector2 dummyGUi = HandleUtility.WorldToGUIPoint(m_DummyOffset + m_pActor.GetPosition() + (m_pDummyActor.GetModelHeight() * 1.5f) * Vector3.up);
                GUILayout.BeginArea(new Rect(dummyGUi.x, dummyGUi.y, 100, 25));
                float labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 50;
                m_nDummyAttackGroup = EditorGUILayout.Toggle("敌方", m_nDummyAttackGroup != 0) ? 1 : 0;
                EditorGUIUtility.labelWidth = labelWidth;
                GUILayout.EndArea();
            }
#if !UNITY_5_1
            Handles.zTest = zTest;
#endif
        }
        //-----------------------------------------------------
        public void OnDrawInspecPanel(Vector2 size)
        {
            if (m_pActor == null || m_pActor.GetActionStateGraph() == null) return;
            if (m_pCurActionState == null || m_pCurActionState.GetCore().isNullAnimation())
            {
                EditorGUILayout.HelpBox("为空动作", MessageType.Warning);
            }
            using (new UnityEngine.GUILayout.HorizontalScope("box"))
            {
                EditorUtil.BeginHorizontal();
                if (m_pCurActionState != null && GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20) }))
                {
                    if (EditorUtility.DisplayDialog("Tip", "delete action state?", "ok", "cancel"))
                    {
                        DeleteActionState(m_pCurActionState);
                        m_pCurActionState = null;
                        //EditorUtil.EndHorizontal();
                        return;
                    }
                }
                m_bExpandAction = EditorGUILayout.Foldout(m_bExpandAction, "Action State", true);
                EditorUtil.EndHorizontal();
            }
            if (m_bExpandAction)
            {
                EditorGUI.indentLevel++;
                if (m_pCurActionState != null)
                    DrawActionState(m_pCurActionState, new Vector2(size.x-20, size.y));
                EditorGUI.indentLevel--;
            }
           if (m_pActor != null && m_pActor.GetActionStateGraph() != null)
           {
               if (m_pCurActionState != null)
                   DrawActionFrameParameter(m_pCurActionState, size);
           }
        }
        //-----------------------------------------------------
        public void OnDrawFramePanel(Rect size)
        {
            if (m_preview != null)
            {
                DrawPreview(size);
            }
        }
        //-----------------------------------------------------
        public void OnDrawLayerPanel(Vector2 size)
        {
            int preIndex = m_nSelectIndex;
            
            EditorUtil.BeginHorizontal();
            using (new EditorKits.EditorGUIUtilityLabelWidth(80))
            {
                m_nSelectIndex = EditorGUILayout.Popup("选择角色", m_nSelectIndex, m_PopDatas.ToArray());
            }
            string strFiler = m_strFiler;
                m_strFiler = EditorGUILayout.TextField(m_strFiler, new GUILayoutOption[] { GUILayout.Width(100) });
            if (m_strFiler.CompareTo(strFiler) != 0)
            {
                RefreshPop(false);
            }
            EditorUtil.EndHorizontal();
            if(m_nSelectIndex != preIndex)
            {
                OnSelectChange();
            }
            if(m_pActorPrefab == null)
            {
                m_pEditor.ShowNotification(new GUIContent("预制体对象丢失[" + m_strModeFile + "]"));
         //       return;
            }
            if (m_pActor == null) return;

            if(m_pActor.GetActionStateGraph() == null)
            {
                if(GUILayout.Button("创建动作脚本"))
                {
                    New();
                }
                if (GUILayout.Button("选择动作脚本"))
                {
                    Select();
                }
                return;
            }

            Color color = GUI.color;
            float gap = (size.x-10) / TABS.Length;
            EditorUtil.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(size.x-10) });
            for(int i = 0; i < TABS.Length; ++i)
            {
                if (curTab == (ETabType)i) GUI.color = Color.green;
                else GUI.color = color;
                if (GUILayout.Button( TABS[i], new GUILayoutOption[] { GUILayout.Width(gap) } ))
                {
                    curTab = (ETabType)i;
                }
            }
            GUI.color = color;
            EditorUtil.EndHorizontal();

            ActionStateGraph pGrah = m_pActor.GetActionStateGraph();
            if (pGrah == null)
                return;

            size.x -= 10;
            Framework.ED.EditorUtil.BeginVertical(new GUILayoutOption[] { GUILayout.MaxWidth(size.x) });
            if (curTab == ETabType.BaseInfo) DrawBaseInfo(pGrah, size);
            else if (curTab == ETabType.Avatar) DrawAvatar(pGrah, size);
            else if (curTab == ETabType.Stance) DrawStance(pGrah, size);
            else if (curTab == ETabType.StateFrames) DrawStateFrames(pGrah, size);
            Framework.ED.EditorUtil.EndVertical();
        }
        //-----------------------------------------------------
        void OnChangeGraphStand()
        {
            m_nPlayActionCnt = 0;
            m_pCurActionState = null;
            RefreshGraphsStand(null);
        }
        //-----------------------------------------------------
        void OnChangeActionState()
        {
            m_nPlayActionCnt = 0;
            m_curActionName = "";
            if (m_pCurActionState == null)
            {
                m_nSelectFrame = -1;
                if (m_pActor != null)
                {
                    if(m_pActor.GetObjectAble() != null)
                        m_AniPlayback.SetTarget(m_pActor.GetObjectAble().GetObject(), null, null);
                    m_pActor.StopCurrentActionState();
                }
                return;
            }
            m_curActionName = m_pCurActionState.GetCore().name;
            if (m_pCurActionState.GetCore().animations!=null)
            {
                for(int j = 0; j < m_pCurActionState.GetCore().animations.Length; ++j)
                {
                    AnimationSeq seq = m_pCurActionState.GetCore().animations[j];
                    for (int i = 0; i < m_vAnimationState.Count; ++i)
                    {
                        if (m_vAnimationState[i].state == null) continue;
                        if (m_vAnimationState[i].state.name.CompareTo(seq.animation) == 0)
                        {
                            seq.state = m_vAnimationState[i].state;
                            m_pCurActionState.GetCore().animations[j] = seq;
                            break;
                        }
                    }

                }
            }
            m_pActor.StartActionState(m_pCurActionState, 0, 1f, true, false, true);
            m_nSelectFrame = -1;
            if(m_pActor.GetObjectAble() != null)
                m_AniPlayback.SetTarget(m_pActor.GetObjectAble().GetObject(), m_pCurActionState, m_pBaseCrossState);
        }
        //-----------------------------------------------------
        void DrawAvatar(ActionStateGraph pGrah, Vector2 size)
        {
            DrawSlotAvatarDatas(pGrah, size);

            //   if (pGrah.IsUsedPlayableGraph())
            {
                if (GUILayout.Button("添加AvatarMask"))
                {
                    List<AvatarMask> vMasks = m_AvatarMasks != null ? new List<AvatarMask>(m_AvatarMasks) : new List<AvatarMask>();
                    vMasks.Add(null);
                    m_AvatarMasks = vMasks.ToArray();
                }

                if (m_AvatarMasks != null)
                {
                    for (int i = 0; i < m_AvatarMasks.Length; ++i)
                    {
                        EditorUtil.BeginHorizontal();
                        m_AvatarMasks[i] = EditorGUILayout.ObjectField("Layer[" + i + "]", m_AvatarMasks[i], typeof(AvatarMask), false) as AvatarMask;
                        if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(40) }))
                        {
                            if (EditorUtility.DisplayDialog("提示", "确定删除?", "删除", "取消"))
                            {
                                List<AvatarMask> vMasks = m_AvatarMasks != null ? new List<AvatarMask>(m_AvatarMasks) : new List<AvatarMask>();
                                vMasks.RemoveAt(i);
                                m_AvatarMasks = vMasks.ToArray();
                                break;
                            }
                        }
                        EditorUtil.EndHorizontal();
                    }
                }
            }
        }
        //-----------------------------------------------------
        List<Slot> m_vSlots = new List<Slot>();
        void DrawBaseInfo(ActionStateGraph pGrah, Vector2 size)
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            AnimatorController controller = EditorGUILayout.ObjectField("控制器", m_AnimatorController, typeof(AnimatorController), false) as AnimatorController;
            if(controller)
            {
                if (m_AnimatorController != controller)
                {
                    m_AnimatorController = controller;
                    m_vAnimationState.Clear();
                    BuildAnimatorStates(controller, ref m_vAnimationState, true);
                }
            }

            m_bExpandSlot = EditorGUILayout.Foldout(m_bExpandSlot, "绑点", true);
            if (m_bExpandSlot)
            {
                if (m_pActor.GetActionStateGraph() != null && m_pActor.GetActionStateGraph().GetCurrentStance() == null)
                    m_pActor.GetActionStateGraph().SwitchFisrtStance();

                EditorGUI.indentLevel++;
                m_vSlots.Clear();
                Dictionary<string, Slot> vSlots = m_pActor.GetActionStateGraph().GetSlots();
                foreach (var db in vSlots)
                {
                    m_vSlots.Add(db.Value);
                }
                for (int i =0; i < m_vSlots.Count; ++i)
                {
                    Slot slot = m_vSlots[i];
                    bool bExpand = false;
                    EditorUtil.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(size.x-10) });
                    Core.BodyPart part = null;

                    string strLabel = "[" + i.ToString() + "]";
                    EditorGUIUtility.labelWidth = 80;
                    bExpand = EditorGUILayout.Foldout(m_bExpandListenSlot.CompareTo(slot.name)==0, strLabel, true);
                    
                    if (slot.transform)
                    {
                        part = slot.transform.GetComponent<Core.BodyPart>();
                        if (part != null)
                        {
                            strLabel = null;
                        }
                    }
                    string name = EditorGUILayout.TextField(slot.name);
                    if(!string.IsNullOrEmpty(name) && name.CompareTo(slot.name)!=0)
                    {
                        if(!vSlots.ContainsKey(name))
                        {
                            slot.name = name;
                        }
                    }
                    if (GUILayout.Button("删除"))
                    {
                        if(EditorUtility.DisplayDialog("提示", "确定删除绑点?", "删除", "取消"))
                        {
                            m_vSlots.RemoveAt(i);
                            //EditorUtil.EndHorizontal();
                            break;
                        }
                    }
                    EditorUtil.EndHorizontal();

                    if(bExpand)
                    {
                        EditorGUI.indentLevel++;
                        m_bExpandListenSlot = slot.name;
                        EditorUtil.BeginHorizontal();
                        slot.transform = EditorGUILayout.ObjectField("绑点",slot.transform, typeof(Transform), true) as Transform;
                        int tranIndex = EditorGUILayout.Popup(m_vChildTransformSlots.IndexOf(slot.transform), m_vChildTransformSlotsPop.ToArray());
                        if (tranIndex >= 0 && tranIndex < m_vChildTransformSlots.Count)
                            slot.transform = m_vChildTransformSlots[tranIndex];
                        EditorUtil.EndHorizontal();

                        EditorUtil.BeginHorizontal();
                        slot.centroid = EditorGUILayout.ObjectField("质心",slot.centroid, typeof(Transform), true) as Transform;
                        int centIndex = EditorGUILayout.Popup(m_vChildTransformSlots.IndexOf(slot.centroid), m_vChildTransformSlotsPop.ToArray());
                        if (centIndex >= 0 && centIndex < m_vChildTransformSlots.Count)
                            slot.centroid = m_vChildTransformSlots[centIndex];
                        EditorUtil.EndHorizontal();
                        slot.offset = EditorGUILayout.Vector3Field("偏移", slot.offset, new GUILayoutOption[] { GUILayout.Width((size.x - 80) ) });
                        EditorGUI.indentLevel--;
                    }
                    else if (m_bExpandListenSlot.CompareTo(slot.name) == 0)
                        m_bExpandListenSlot = "";

                    EditorGUIUtility.labelWidth = labelWidth;
                    m_vSlots[i] = slot;
                }
                m_pActor.GetActionStateGraph().SetSlots(m_vSlots);
            //    m_pActor.GetActionStateGraph().SetSlots(m_vSlots);
                EditorGUIUtility.labelWidth = labelWidth;
                EditorUtil.BeginHorizontal(new GUILayoutOption[] { GUILayout.MaxWidth(size.x-80) });
                m_nAddSlotIndex = EditorGUILayout.Popup(m_nAddSlotIndex, m_vChildTransformSlotsPop.ToArray());
                if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.MaxWidth(40) }))
                {
                    if (m_nAddSlotIndex >= 0 && m_nAddSlotIndex < m_vChildTransformSlots.Count)
                    {
                        bool bHad = false;
                        foreach(var db in vSlots)
                        {
                            if(m_vChildTransformSlots[m_nAddSlotIndex] == db.Value.transform)
                            {
                                bHad = true;
                                break;
                            }
                        }
                        if (!bHad && !vSlots.ContainsKey(m_vChildTransformSlots[m_nAddSlotIndex].name))
                        {
                            vSlots.Add(m_vChildTransformSlots[m_nAddSlotIndex].name, new Slot(m_vChildTransformSlots[m_nAddSlotIndex].name, m_vChildTransformSlots[m_nAddSlotIndex]));
                        }
                    }
                    m_nAddSlotIndex = -1;
                }
                EditorUtil.EndHorizontal();
                EditorGUI.indentLevel--;

                m_vSlotPop.Clear();
                m_vSlotPop.Add("None");
                foreach (var db in vSlots)
                {
                    if(db.Value.IsValid)
                        m_vSlotPop.Add(db.Key);
                }
            }

            size.x = size.x - 10;
            GUILayoutOption[] op = new GUILayoutOption[] { GUILayout.Width(size.x) };

            EditorUtil.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(size.x-120) });
            EditorGUIUtility.labelWidth = 80;
            m_JumpTest.SimulateSpeed.z = EditorGUILayout.Slider("模拟前进速度", m_JumpTest.SimulateSpeed.z, 0, 100);
            EditorGUIUtility.labelWidth = labelWidth;
            if (GUILayout.Button(m_JumpTest.bSimulateJump ? "停止跳跃模拟" : "模拟跳跃"))
            {
                m_JumpTest.bSimulateJump = !m_JumpTest.bSimulateJump;
                m_pActor.SetFinalPosition(Vector3.zero);
                m_pActor.ResetMomentum();
                m_pActor.EnableLogic(true);
                if (m_pActor.GetActorAgent() is ActorEditorAgent)
                    (m_pActor.GetActorAgent() as ActorEditorAgent).bUseActionDo = m_JumpTest.bSimulateJump;
                if (m_JumpTest.bSimulateJump)
                {
                    m_pActor.SetSpeed(Vector3.zero);
                    m_pActor.StartActionByType(EActionStateType.JumpStart, 0, 1, true, false, true);
                    m_JumpTest.StartJump(m_pActor);
                }
            }
            if (GUILayout.Button("复制跳跃参数"))
            {
                m_JumpTest.CopyJumpParam.bValid = true;
                m_JumpTest.CopyJumpParam.JumpHorizenSpeed = m_pActor.GetActionStateGraph().GetJumpHorizenSpeed();
                m_JumpTest.CopyJumpParam.JumpVerticalSpeed = m_pActor.GetActionStateGraph().GetJumpVerticalSpeed();
                m_JumpTest.CopyJumpParam.Gravity = m_pActor.GetActionStateGraph().GetPhysicsGravity();
                m_JumpTest.CopyJumpParam.FallingSpeed = m_pActor.GetActionStateGraph().GetFallingSpeed();
                m_JumpTest.CopyJumpParam.MaxJumpHeight = m_pActor.GetActionStateGraph().GetMaxJumpHeight();
            }
            if (m_JumpTest.CopyJumpParam.bValid && GUILayout.Button("黏贴跳跃参数"))
            {
                m_JumpTest.CopyJumpParam.bValid = false;
                m_pActor.GetActionStateGraph().SetJumpHorizenSpeed(m_JumpTest.CopyJumpParam.JumpHorizenSpeed);
                m_pActor.GetActionStateGraph().SetJumpVerticalSpeed(m_JumpTest.CopyJumpParam.JumpVerticalSpeed);
                m_pActor.GetActionStateGraph().SetPhysicsGravity(m_JumpTest.CopyJumpParam.Gravity);
                m_pActor.GetActionStateGraph().SetFallingSpeed(m_JumpTest.CopyJumpParam.FallingSpeed);
                m_pActor.GetActionStateGraph().SetMaxJumpHeight(m_JumpTest.CopyJumpParam.MaxJumpHeight);

            }
            EditorUtil.EndHorizontal();
            if (Data.GlobalSetting.Instance != null)
            {
                float fSpeed = EditorGUILayout.FloatField("全局模拟跳跃速度因子", Data.GlobalSetting.Instance.fJumpSpeed, op);
                float fJumpInitV = EditorGUILayout.Slider("全局跳跃初始速度", Data.GlobalSetting.Instance.fJumpInitVelocity, 0, 20, op);
                if (fSpeed != Data.GlobalSetting.Instance.fJumpSpeed || fJumpInitV != Data.GlobalSetting.Instance.fJumpInitVelocity)
                {
                    Data.GlobalSetting.Instance.fJumpSpeed = fSpeed;
                    Data.GlobalSetting.Instance.fJumpInitVelocity = fJumpInitV;
                    EditorUtility.SetDirty(Data.GlobalSetting.Instance);
                }
            }
            pGrah.SetObjectScale(EditorGUILayout.FloatField("Object Scale", pGrah.GetObjectScale(), op));
            pGrah.SetFootShadowScale(EditorGUILayout.FloatField("Foot Shadow Scale", pGrah.GetFootShadowScale(), op));
            pGrah.SetGlobalSpeedScale(EditorGUILayout.FloatField("Action Speed Scale", pGrah.GetGlobalSpeedScale(), op));
            pGrah.SetRunSpeed(EditorGUILayout.FloatField("Run Speed", pGrah.GetRunSpeed(), op));
            pGrah.SetFastRunSpeed(EditorGUILayout.FloatField("Fast Run Speed", pGrah.GetFastRunSpeed(), op));
            pGrah.SetPhysicsRadius(EditorGUILayout.FloatField("Physics Radius", pGrah.GetPhysicsRadius(), op));
            pGrah.SetPhysicsPushForce(EditorGUILayout.FloatField("Physics Push Force", pGrah.GetPhysicsPushForce(), op));
            pGrah.SetPhysicsGravity(Mathf.Max(0, EditorGUILayout.FloatField("Physics Gravity", pGrah.GetPhysicsGravity(), op)));
            pGrah.SetFallingSpeed(EditorGUILayout.FloatField("Falling Speed", pGrah.GetFallingSpeed(), op));
            pGrah.SetMaxJumpHeight(Mathf.Max(0, EditorGUILayout.FloatField("Jump Limit Height", pGrah.GetMaxJumpHeight(), op)));
            pGrah.SetStepHeight(Mathf.Max(0, EditorGUILayout.FloatField("Step Limit Height", pGrah.GetStepHeight(), op)));
            pGrah.SetUnBlowUp(EditorGUILayout.Toggle("UnBlowUp", pGrah.IsUnBlowUp(), op));
            pGrah.SetHardBody(EditorGUILayout.Toggle("HardBody", pGrah.IsHardBody(), op));
            pGrah.SetFixRotation(EditorGUILayout.Toggle("FixRotation", pGrah.IsFixRotation(), op));
            pGrah.SetCollisionType((EActorCollisionType)EditorGUILayout.EnumPopup("CollisionType", pGrah.GetCollisionType(), op));
            if (pGrah.GetCollisionType() == EActorCollisionType.BOX)
            {
                pGrah.SetBoxCollisionExtrude(EditorGUILayout.Vector3Field("BoxExtrude", pGrah.GetBoxCollisionExtrude(), op));
            }
            else if (pGrah.GetCollisionType() == EActorCollisionType.CAPSULE)
            {
                pGrah.SetCapsuleCollisionRadius(EditorGUILayout.FloatField("CapsuleRadius", pGrah.GetCapsuleCollisionRadius(), op));
                pGrah.SetCapsuleCollisionHeight(EditorGUILayout.FloatField("CapsuleHeight", pGrah.GetCapsuleCollisionHeight(), op));
            }
            pGrah.SetMinAttackDistance(EditorGUILayout.FloatField("MinAttackDistance", pGrah.GetMinAttackDistance(), op));
            pGrah.SetMaxAttackDistance(EditorGUILayout.FloatField("MaxAttackDistance", pGrah.GetMaxAttackDistance(), op));
            pGrah.SetMaxTargetDistance(EditorGUILayout.FloatField("MaxTargetDistance", pGrah.GetMaxTargetDistance(), op));
            pGrah.SetMaxTargetZoom(EditorGUILayout.FloatField("MaxTargetZoom", pGrah.GetMaxTargetZoom(), op));

            EditorUtil.BeginHorizontal();

            pGrah.SetAIID(EditorGUILayout.IntField("AI_ID", pGrah.GetAIID(), new GUILayoutOption[] { GUILayout.Width(size.x-20) }));

            EditorUtil.EndHorizontal();         
        }
        //------------------------------------------------------
        string PopSlot(string slot)
        {
            int indexof = -1;
            if (string.IsNullOrEmpty(slot))
            {
                indexof = 0;
            }
            else indexof = m_vSlotPop.IndexOf(slot);
            indexof = EditorGUILayout.Popup(indexof, m_vSlotPop.ToArray());
            if (indexof >= 1 && indexof < m_vSlotPop.Count)
                slot = m_vSlotPop[indexof];
            else
                slot = "";
            return slot;
        }
        //-----------------------------------------------------
        void DrawSlotAvatarDatas(ActionStateGraph pGrash, Vector2 size)
        {
            SlotAvatar[] avatars = pGrash.GetSlotAvatars();
            EditorUtil.BeginHorizontal();
            m_bExpandSlotAvatar = EditorGUILayout.Foldout(m_bExpandSlotAvatar, "Avatar部件", true);
            if(GUILayout.Button("新建", new GUILayoutOption[] { GUILayout.Width(50) }))
            {
                List<SlotAvatar> avatars_list = avatars!=null?new List<SlotAvatar>(avatars):new List<SlotAvatar>();
                SlotAvatar avart = new SlotAvatar();
                avatars_list.Add(avart);
                avatars = avatars_list.ToArray();
                pGrash.SetSlotAvatars(avatars);

                avatars = pGrash.GetSlotAvatars();

                if (m_pActor != null && m_pActor.GetActorAgent() != null)
                    m_pActor.GetActorAgent().CreateSlotAvatars(avatars);

            }
            EditorUtil.EndHorizontal();
            if (m_bExpandSlotAvatar)
            {
                EditorGUI.indentLevel++;
                float width = size.x - 10;
                bool bRefreshAvatar = false;
                if (avatars!=null)
                {
                    for(int i = 0; i < avatars.Length; ++i)
                    {
                        SlotAvatar avatar = avatars[i];
                        EditorUtil.BeginHorizontal();
                        bool bExpand = EditorGUILayout.Foldout(m_nSelectAvatarIndex == i, "Avatar[" + i + 1 + "]");
                        if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(50) }))
                        {
                            if (EditorUtility.DisplayDialog("提示", "确定删除部位信息？", "删除", "取消"))
                            {
                                List<SlotAvatar> avatars_list = new List<SlotAvatar>(avatars);
                                avatars_list.RemoveAt(i);
                                avatars = avatars_list.ToArray();

                                if (m_pActor != null && m_pActor.GetActorAgent() != null)
                                    m_pActor.GetActorAgent().CreateSlotAvatars(avatars);
                               // EditorUtil.EndHorizontal();
                                break;
                            }
                        }
                        EditorUtil.EndHorizontal();
                        if (bExpand)
                        {
                            EditorGUI.indentLevel++;
                            m_nSelectAvatarIndex = i;
                            EditorUtil.BeginChangeCheck();
                            GameObject prefab = EditorGUILayout.ObjectField(AssetDatabase.LoadAssetAtPath<GameObject>(avatar.prefab), typeof(GameObject), false) as GameObject;
                            if (prefab != null)
                            {
                                avatar.prefab = AssetDatabase.GetAssetPath(prefab);
                            }
                            else
                                avatar.prefab = "";

                            EditorUtil.BeginHorizontal();
                            avatar.parent_slot = PopSlot(avatar.parent_slot);
                            EditorUtil.EndHorizontal();

                            avatar.lifeTime = EditorGUILayout.FloatField(avatar.lifeTime);

                            avatar.visible = EditorGUILayout.Toggle("初始显示", avatar.visible);
                            avatar.scale = EditorGUILayout.FloatField("缩放", avatar.scale);
                            avatar.offset_pos = EditorGUILayout.Vector3Field("位置偏移", avatar.offset_pos);
                            avatar.offset_rot = EditorGUILayout.Vector3Field("旋转偏移", avatar.offset_rot);
                            avatar = (SlotAvatar)HandleUtilityWrapper.DrawPropertyByFieldName(avatar, "bindBit");
                            avatar.bindSymbolID = EditorGUILayout.IntField("标识", avatar.bindSymbolID);


                            EditorUtil.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(size.x - 20) });
                            avatar.bExpandHideStates = EditorGUILayout.Foldout(avatar.bExpandHideStates, "跟随动作隐藏");
                            if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(40) }))
                            {
                                List<uint> vTems = avatar.hideInState != null ? new List<uint>(avatar.hideInState) : new List<uint>();
                                vTems.Add(0);
                                avatar.hideInState = vTems.ToArray();
                            }
                            EditorUtil.EndHorizontal();
                            if (avatar.bExpandHideStates)
                            {
                                GUILayoutOption[] subOp = new GUILayoutOption[] { GUILayout.Width(size.x - 40) };

                                EditorGUI.indentLevel++;
                                if (avatar.hideInState != null)
                                {
                                    for (int j = 0; j < avatar.hideInState.Length; ++j)
                                    {
                                        EditorUtil.BeginHorizontal(subOp);
                                        EActionStateType eType;
                                        uint tag;
                                        ActionStateManager.GetActionTypeAndTag(avatar.hideInState[j], out eType, out tag);
                                        ActionState pState = m_pActor.GetActionStateGraph().GetActionState(eType, tag);
                                        pState = PopState(m_pActor.GetActionStateGraph().GetCurrentStance(), pState);
                                        if (pState != null)
                                        {
                                            avatar.hideInState[j] = ActionStateManager.BuildActionKey(pState);
                                        }
                                        if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(50) }))
                                        {
                                            List<uint> vTems = new List<uint>(avatar.hideInState);
                                            vTems.RemoveAt(i);
                                            avatar.hideInState = vTems.ToArray();
                                          //  EditorUtil.EndHorizontal();
                                            break;
                                        }
                                        EditorUtil.EndHorizontal();
                                    }
                                }
                                EditorGUI.indentLevel--;
                            }
                            if (EditorUtil.EndChangeCheck())
                                bRefreshAvatar = true;
                            EditorGUI.indentLevel--;
                        }
                        else
                        {
                            if (m_nSelectAvatarIndex == i) m_nSelectAvatarIndex = -1;
                        }
                        avatars[i] = avatar;

                        if (bRefreshAvatar && m_pActor != null && m_pActor.GetActorAgent() != null)
                        {
                            m_pActor.GetActorAgent().UpdateSlotAvatar(i, avatar);
                        }
                    }
                }
                pGrash.SetSlotAvatars(avatars);
                EditorGUI.indentLevel--;
            }
        }
        //-----------------------------------------------------
        void DrawStateFrames(ActionStateGraph pGrah, Vector2 size)
        {
            size.x = size.x - 10;
            GUILayoutOption[] op = new GUILayoutOption[] { GUILayout.Width(size.x) };

            List<StateFrame> frames = pGrah.GetStateFrames();
            if(frames == null)
            {
                frames = new List<StateFrame>();
            }
            m_vPopStateFrames.Clear();
            for (int i= 0; i < frames.Count; ++i)
            {
                m_vPopStateFrames.Add(frames[i].name + "[" + frames[i].id + "]");
            }
            using (new UnityEngine.GUILayout.HorizontalScope("box"))
            {
                EditorUtil.BeginHorizontal();
                if (m_nStateFrameIndex >= 0 && m_nStateFrameIndex < frames.Count && GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20) }))
                {
                    if (EditorUtility.DisplayDialog("Tip", "delete state frame?", "ok", "cancel"))
                    {
                        foreach(var state in m_pActor.GetActionStateGraph().GetActionStateMap())
                        {
                            if(state.Value.GetCore().frame == null) continue;
                            for(int i=0;i < state.Value.GetCore().frame.Count; ++i)
                            {
                                if(state.Value.GetCore().frame[i].pFrame != null && state.Value.GetCore().frame[i].pFrame == frames[m_nStateFrameIndex])
                                {
                                    state.Value.GetCore().frame[i].pFrame = null;
                                    state.Value.GetCore().frame[i].frameID = 0xff;
                                }
                            }
                        }
                        frames.RemoveAt(m_nStateFrameIndex);
                        m_nSelectIndex = -1;
                       // EditorUtil.EndHorizontal();
                        return;
                    }
                }
                m_nStateFrameIndex = EditorGUILayout.Popup("StateFrame", m_nStateFrameIndex, m_vPopStateFrames.ToArray(), new GUILayoutOption[] { GUILayout.Width(size.x-40) });
                if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
                {
                    m_nStateFrameIndex = frames.Count;
                    short newID = -1;
                    for (int i = 0; i < frames.Count; ++i)
                    {
                        newID = (short)Mathf.Max(newID, (short)frames[i].id);
                    }
                    newID++;
                    StateFrame newFrame = new StateFrame();
                    newFrame.id = (byte)newID;
                    frames.Add(newFrame);
                }
                EditorUtil.EndHorizontal();
            }


            if (m_nStateFrameIndex >=0 && m_nStateFrameIndex < frames.Count)
            {
                DrawStateFrame(frames[m_nStateFrameIndex], size.x, new GUILayoutOption[] { GUILayout.Width(size.x) });
            }

            pGrah.SetStateFrames(frames);
        }
        //-----------------------------------------------------
        void DrawStance(ActionStateGraph pGrah, Vector2 size)
        {
            string curStand = "";
            if (pGrah.GetCurrentStance() != null)
                curStand = pGrah.GetCurrentStance()._strName;
            int index = m_vStandPop.IndexOf(curStand);
            index = EditorGUILayout.Popup("ActionStands", index, m_vStandPop.ToArray(), new GUILayoutOption[] { GUILayout.Width(size.x-10) });
            if (index >= 0 && index < m_vStandPop.Count)
            {
                string stranceName = m_vStandPop[index];
                if (pGrah.SwitchStance(stranceName))
                {
                    m_curStandName = pGrah.GetCurrentStance()._strName;
                    OnChangeGraphStand();
                }
            }
            if (pGrah.GetCurrentStance() == null)
            {
                using (new UnityEngine.GUILayout.HorizontalScope("box"))
                {
                    EditorUtil.BeginHorizontal();
                    m_strAddStandName = EditorGUILayout.TextField("Add ActionStance...", m_strAddStandName, new GUILayoutOption[] { GUILayout.Width(size.x - 20) });
                    EditorUtil.BeginDisabledGroup(m_strAddStandName.Length <= 0 || pGrah.GetActionStateStanceMap().ContainsKey(m_strAddStandName));
                    if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
                    {
                        pGrah.NewStance(m_strAddStandName, null);
                        pGrah.SwitchStance(m_strAddStandName);
                        m_strAddStandName = "";
                        m_curStandName = pGrah.GetCurrentStance()._strName;
                        OnChangeGraphStand();
                    }
                    EditorUtil.EndDisabledGroup();
                    EditorUtil.EndHorizontal();
                }

                return;
            }
            m_vActionStatePopNames.Clear();
            m_vActionStateNames.Clear();
            m_vActionStateNames.Add("None");
            m_vActionStatePopNames.Add("None");
            foreach (var db in pGrah.GetCurrentStance()._mActionStateId)
            {
                if (string.IsNullOrEmpty(db.Value.GetCore().name))
                    db.Value.GetCore().name = db.Value.GetCore().id.ToString();
                {
                    string groupName = ED.EditorHelp.GetDisplayName(db.Value.GetCore().type);
                    if (groupName == null || groupName.CompareTo(db.Value.GetCore().type.ToString()) == 0)
                        groupName = "";
                    if (string.IsNullOrEmpty(groupName))
                        m_vActionStatePopNames.Add(db.Value.GetCore().name + "[id:" + db.Value.GetCore().id + "]" + "[tag:" + db.Value.GetCore().tag + "]");
                    else
                    {
                        int split = groupName.IndexOf('/');
                        if (split>0)
                            groupName = groupName.Substring(0, split);
                        m_vActionStatePopNames.Add(groupName + "/" + db.Value.GetCore().name + "[id:" + db.Value.GetCore().id + "]" + "[tag:" + db.Value.GetCore().tag + "]");
                    }
                    m_vActionStateNames.Add(db.Value.GetCore().name );
                }
            }


            using (new UnityEngine.GUILayout.HorizontalScope("box"))
            {
                EditorUtil.BeginHorizontal();
                m_strAddStandName = EditorGUILayout.TextField("Add ActionStance...", m_strAddStandName, new GUILayoutOption[] { GUILayout.Width(size.x - 20) });
                EditorUtil.BeginDisabledGroup(m_strAddStandName.Length <= 0);
                if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
                {
                    if (!pGrah.GetActionStateStanceMap().ContainsKey(m_strAddStandName))
                    {
                        pGrah.NewStance(m_strAddStandName, null);
                        pGrah.SwitchStance(m_strAddStandName);
                        m_strAddStandName = "";
                        m_curStandName = pGrah.GetCurrentStance()._strName;
                        OnChangeGraphStand();
                    }

                }
                EditorUtil.EndDisabledGroup();
                EditorUtil.EndHorizontal();
            }


            if (pGrah.GetCurrentStance() == null) return;
            using (new UnityEngine.GUILayout.HorizontalScope("box"))
            {
                EditorUtil.BeginHorizontal();
                if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20) }))
                {
                    if (EditorUtility.DisplayDialog("Tip", "delete action stance?", "ok", "cancel"))
                    {
                        DeleteStance(pGrah.GetCurrentStance());
                       // EditorUtil.EndHorizontal();
                        return;
                    }
                }
                m_bExpandStand = EditorGUILayout.Foldout(m_bExpandStand, "ActionStance", true);
                EditorUtil.EndHorizontal();
            }
            if (m_bExpandStand)
            {
                EditorGUI.indentLevel++;

                GUILayoutOption[] sub_op = new GUILayoutOption[] { GUILayout.Width(size.x - 40) };

                EditorUtil.BeginHorizontal();
                m_curStandName = EditorGUILayout.TextField("StanceName", m_curStandName, new GUILayoutOption[] { GUILayout.Width(size.x - 100) });
                EditorUtil.BeginDisabledGroup(m_curStandName == pGrah.GetCurrentStance()._strName);
                if (GUILayout.Button("Applay", new GUILayoutOption[] { GUILayout.Width(60) }))
                {
                    if (!pGrah.GetActionStateStanceMap().ContainsKey(m_curStandName))
                    {
                        pGrah.ReNameStance(pGrah.GetCurrentStance()._strName, m_curStandName);
                        OnChangeGraphStand();
                    }
                }
                EditorUtil.EndDisabledGroup();
                EditorUtil.EndHorizontal();
                int stanceIndex = 0;
                if (!string.IsNullOrEmpty(pGrah.GetCurrentStance()._strInheritStance))
                    stanceIndex = m_vStandPop.IndexOf(pGrah.GetCurrentStance()._strInheritStance);
                stanceIndex  = EditorGUILayout.Popup("InheritStance", stanceIndex, m_vStandPop.ToArray(), new GUILayoutOption[] { GUILayout.Width(size.x - 100) });
                if(stanceIndex >=0 && stanceIndex < m_vStandPop.Count)
                {
                    if (stanceIndex <= 0 || m_vStandPop[stanceIndex].CompareTo(pGrah.GetCurrentStance()._strName) ==0) pGrah.GetCurrentStance()._strInheritStance = null;
                    else pGrah.GetCurrentStance()._strInheritStance = m_vStandPop[stanceIndex];
                }

                pGrah.GetCurrentStance()._fRunSpeed = EditorGUILayout.FloatField("RunSpeed", pGrah.GetCurrentStance()._fRunSpeed, sub_op);
                pGrah.GetCurrentStance()._fFastRunSpeed = EditorGUILayout.FloatField("FastRunSpeed", pGrah.GetCurrentStance()._fFastRunSpeed, sub_op);
                pGrah.GetCurrentStance()._fTurnSpeed = Mathf.Clamp(EditorGUILayout.FloatField("TurnSpeed", pGrah.GetCurrentStance()._fTurnSpeed, sub_op), 0, 10);
                pGrah.GetCurrentStance()._fJumpHorizenSpeed = EditorGUILayout.FloatField("JumpHorizenSpeed", pGrah.GetCurrentStance()._fJumpHorizenSpeed, sub_op);
                pGrah.GetCurrentStance()._fJumpVerticalSpeed = EditorGUILayout.FloatField("JumpVerticalSpeed", pGrah.GetCurrentStance()._fJumpVerticalSpeed, sub_op);

                DrawActionStateParameter( pGrah.GetCurrentStance(), size);
                EditorGUI.indentLevel--;
            }

            using (new UnityEngine.GUILayout.HorizontalScope("box"))
            {
                EditorUtil.BeginHorizontal();
                m_bExpandSucceedActionList = EditorGUILayout.Foldout(m_bExpandSucceedActionList, "SucceedActionList", true);
                EditorUtil.EndHorizontal();
            }
            if(m_bExpandSucceedActionList)
            {
                EditorGUI.indentLevel++;
                DrawSucceedActionList(pGrah, size);
                EditorUtil.BeginHorizontal();
                ActionState pState = PopState(pGrah.GetCurrentStance(),ref m_nAddSucceedActionID, "添加", new GUILayoutOption[] { GUILayout.Width(size.x - 40) });
                bool bExist = pState!=null? pGrah.GetSucceedActionStateMap().ContainsKey(pState):false;
                if (pState!=null && (!bExist || (bExist && ActionStateManager.getInstance().IsGolbalSucceed(pState))))
                {
                    if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
                    {
                        m_bExpandSucceedChecks[pState] = false;
                        if(!bExist)
                        {
                            List<SucceedAction> vSucceeds = new List<SucceedAction>();
                            for (int i = 0; i < (int)ESucceedActionState.Count; ++i)
                                vSucceeds.Add(new SucceedAction(-1));
                            pGrah.GetSucceedActionStateMap().Add(pState, vSucceeds);
                        }

                        m_nAddSucceedActionID = 0;
                    }
                }
                EditorUtil.EndHorizontal();
                EditorGUI.indentLevel--;
            }
        }
        //-----------------------------------------------------
        ActionState PopState(ActionStateStance pStance, ActionState current, string label=null, GUILayoutOption[] op = null)
        {
            if (pStance == null) return current;
            int index = 0;
            if (current != null)
                index = m_vActionStateNames.IndexOf(current.GetCore().name);
            if(string.IsNullOrEmpty(label))
            index = EditorGUILayout.Popup(index, m_vActionStatePopNames.ToArray(), op);
            else
                index = EditorGUILayout.Popup(label, index, m_vActionStatePopNames.ToArray(), op);

            if (index >= 1 && index - 1 < pStance._mActionStateId.Count)
            {
                return pStance._mActionStateId.ElementAt(index - 1).Value;
            }
            return null;
        }
        //-----------------------------------------------------
        ActionState PopState(ActionStateStance pStance, ref int index, string label = null, GUILayoutOption[] op = null)
        {
            if (pStance == null) return null;
            ActionState current = null;
            if (index >= 1 && index - 1 < pStance._mActionStateId.Count)
            {
                if (pStance._mActionStateId.ElementAt(index - 1).Value != m_pCurActionState)
                {
                    current = pStance._mActionStateId.ElementAt(index - 1).Value;
                }
            }
            ActionState action = PopState(pStance, current, label, op);
            if(action!=null)
                index = m_vActionStateNames.IndexOf(action.GetCore().name);
            return action;
        }
        //-----------------------------------------------------
        int PopParameter(int param, string label = null, GUILayoutOption[] op = null)
        {
            int index = 0;
            if(param != 0)
            {
                index = m_vParameterHashs.IndexOf(param);
            }
            if(string.IsNullOrEmpty(label))
                index = EditorGUILayout.Popup(index, m_vParameters.ToArray(), op);
            else
                index = EditorGUILayout.Popup(label, index, m_vParameters.ToArray(), op);
            if (index > 0 && index < m_vParameterHashs.Count)
                param = m_vParameterHashs[index];
            return param;
        }
        //-----------------------------------------------------
        void DrawSucceedActionList(ActionStateGraph pGraph, Vector2 size)
        {
            ActionStateStance pStance = pGraph.GetCurrentStance();
            if (pStance == null) return;
            GUILayoutOption[] sub_op = new GUILayoutOption[] { GUILayout.Width(size.x - 40) };
            Dictionary<ActionState, List<SucceedAction>> succeeds = pGraph.GetSucceedActionStateMap();
            foreach(var db in succeeds)
            {
                if (!m_bExpandSucceedChecks.ContainsKey(db.Key) && ActionStateManager.getInstance().IsGolbalSucceed(db.Key))
                    continue;
                bool bExpand = false;
                m_bExpandSucceedChecks.TryGetValue(db.Key, out bExpand);
                EditorUtil.BeginHorizontal(sub_op);
                bExpand = EditorGUILayout.Foldout(bExpand, db.Key.GetCore().name, true);
                if(GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20) }))
                {
                    succeeds.Remove(db.Key);
                    m_bExpandSucceedChecks.Remove(db.Key);
                    //EditorUtil.EndHorizontal();
                    break;
                }
                m_bExpandSucceedChecks[db.Key] = bExpand;
                EditorUtil.EndHorizontal();

                if(bExpand)
                {
                    EditorGUI.indentLevel++;
                    GUILayoutOption[] sub_sub_op = sub_op;// new GUILayoutOption[] { GUILayout.Width(size.x - 60) };

                    if(db.Value ==null || db.Value.Count != (int)ESucceedActionState.Count)
                    {
                        List<SucceedAction> vBack = db.Value!=null? new List<SucceedAction>(db.Value.ToArray()):new List<SucceedAction>();
                        for(int i = 0; i < (int)ESucceedActionState.Count; ++i)
                        {
                            if (vBack.Count > 0)
                            {
                                db.Value.Add(vBack[0]);
                                vBack.RemoveAt(0);
                            }
                            else
                                db.Value.Add(new SucceedAction(-1));
                        }
                    }
                    float titleGap = (size.x - 10) / 5;
                    GUILayoutOption[] titleLy = new GUILayoutOption[] { GUILayout.Width(titleGap) };
                    EditorUtil.BeginHorizontal(sub_sub_op);
                    GUILayout.Label("Type", titleLy);
                    GUILayout.Label("loop", titleLy);
                    GUILayout.Label("random", titleLy);
                    GUILayout.Label("override", titleLy);
                    GUILayout.Label("", titleLy);
                    EditorUtil.EndHorizontal();
                    for (int i = 0; i < (int)ESucceedActionState.Count; ++i)
                    {
                        SucceedAction suceed = db.Value[i];
                        EditorUtil.BeginHorizontal(titleLy);
                        suceed.expand = EditorGUILayout.Foldout(suceed.expand, ((ESucceedActionState)i).ToString(), true);
                        EditorUtil.EndHorizontal();
                        suceed.loop = (short)EditorGUILayout.IntField(suceed.loop, titleLy);
                        suceed.random = EditorGUILayout.Toggle(suceed.random, titleLy);
                        if (!EditorGUILayout.Toggle(suceed.loop>0, titleLy))
                            suceed.loop = -1;
                        if(GUILayout.Button("Add", titleLy))
                        {
                            var actions = suceed.GetStance(pGraph.GetCurrentStance(), true);
                            if(actions!=null) actions.Add(new SucceedAction.Item() {  pState = null, random = 100 });
                        }
                        EditorUtil.EndHorizontal();

                        if (suceed.expand)
                        {
                            var actions = suceed.GetStance(pGraph.GetCurrentStance());
                            if(actions!=null)
                            {
                                for (int j = 0; j < actions.Count; ++j)
                                {
                                    SucceedAction.Item item = actions[j];
                                    EditorUtil.BeginHorizontal();
                                    item.pState = PopState(pStance, item.pState, null, new GUILayoutOption[] { GUILayout.Width(size.x - 160) });
                                    item.random = (byte)Mathf.Clamp(EditorGUILayout.IntField(item.random, new GUILayoutOption[] { GUILayout.Width(80) }), 0, 100);
                                    if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(40) }))
                                    {
                                        actions.RemoveAt(j);
                                      //  EditorUtil.EndHorizontal();
                                        break;
                                    }
                                    EditorUtil.EndHorizontal();
                                    actions[j] = item;
                                }
                            }
                        }
                        
                        db.Value[i] = suceed;
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }
        //-----------------------------------------------------
        void DrawActionStateParameter(ActionStateStance stance, Vector2 size)
        {
            float remainWidth = size.x - 30;
            using (new UnityEngine.GUILayout.HorizontalScope("box"))
            {
                m_bExpandActionStateParameter = EditorGUILayout.Foldout(m_bExpandActionStateParameter, "ActionStateParamter", true);
            }
            if (m_bExpandActionStateParameter)
            {
                GUILayoutOption[] op = new GUILayoutOption[] { GUILayout.Width(remainWidth) };

                EditorGUI.indentLevel++;

                ActionState returnAction= PopState(stance, m_pCurActionState, "ActionStateList", op);
                if (returnAction!=null)
                {
                    if (returnAction != m_pCurActionState)
                    {
                        m_pCurActionState = returnAction;
                        OnChangeActionState();
                    }
                }
                else
                {
                    m_pCurActionState = null;
                    OnChangeActionState();
                }
                using (new UnityEngine.GUILayout.HorizontalScope("box"))
                {
                    EditorUtil.BeginHorizontal();
                    m_addActionName = EditorGUILayout.TextField("Add ActionState...", m_addActionName, new GUILayoutOption[] { GUILayout.Width(remainWidth - 20) });
                    bool hasName = false;
                    if (m_pActor.GetActionStateGraph()!=null && m_pActor.GetActionStateGraph().GetActionStateMap().ContainsKey(m_addActionName)) hasName = true;
                    EditorUtil.BeginDisabledGroup(m_addActionName.Length <= 0 || m_vActionStateNames.Contains(m_strAddStandName) || hasName);
                    if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
                    {
                        var newAction = NewAcionState(m_addActionName);
                        if(newAction !=null)
                        {
                            m_pCurActionState = newAction;
                            OnChangeActionState();
                            m_addActionName = "";
                        }
                    }
                    EditorUtil.EndDisabledGroup();
                    EditorUtil.EndHorizontal();
                }


                EditorGUI.indentLevel--;
            }
        }
        //-----------------------------------------------------
        AnimatorState DrawAnimation(ref string strName,ref int layer, string label, float labelWidth = -1, GUILayoutOption[] op = null )
        {
            bool usePlayableGraph = false;
            if(m_pGraphBinder!=null) usePlayableGraph = m_pGraphBinder.usePlayableGraph;
            int index = m_vAnimationState.Count-1;
            if (!string.IsNullOrEmpty(strName))
            {
                for (int i = 0; i < m_vAnimationState.Count; ++i)
                {
                    if (m_vAnimationState[i].state == null) continue;
                    if(usePlayableGraph)
                    {
                        if (m_vAnimationState[i].state.name.Equals(strName, StringComparison.OrdinalIgnoreCase))
                        {
                            index = i;
                            break;
                        }
                    }
                    else
                    {
                        if (m_vAnimationState[i].state.name.Equals(strName, StringComparison.OrdinalIgnoreCase) && m_vAnimationState[i].layer == layer)
                        {
                            index = i;
                            break;
                        }
                    }
                }
            }
            float lastLabelWidth = EditorGUIUtility.labelWidth;
            if(labelWidth != -1)
                EditorGUIUtility.labelWidth = labelWidth;
            if(string.IsNullOrEmpty(label))
                index = EditorGUILayout.Popup(index, m_vAnimations.ToArray(), op);
            else
                index = EditorGUILayout.Popup(label, index, m_vAnimations.ToArray(), op);
            if (labelWidth != -1)
                EditorGUIUtility.labelWidth = lastLabelWidth;
            if (index <0 || index >= m_vAnimationState.Count-1)
            {
                strName = "";
                return null;
            }
            else if ((index) < m_vAnimationState.Count-1)
            {
                if (m_vAnimationState[index].state)
                {
                    if (strName != m_vAnimationState[index].state.name)
                    {
                        if (m_vAnimationState[index].state.motion is AnimationClip)
                        {
                            AnimationClip clip = (m_vAnimationState[index].state.motion as AnimationClip);
                            strName = m_vAnimationState[index].state.name;
                            if(!usePlayableGraph)
                            layer = m_vAnimationState[index].layer;
                        }
                        else if (m_vAnimationState[index].state.motion is BlendTree)
                        {
                            BlendTree clip = (m_vAnimationState[index].state.motion as BlendTree);
                            strName = m_vAnimationState[index].state.name;
                            if(!usePlayableGraph)
                                layer = m_vAnimationState[index].layer;
                        }
                    }
                    return m_vAnimationState[index].state;
                }
                else
                {
                    strName = "";
                //    layer = 0;
                    return null;
                }
            }
            return null;
        }
        //-----------------------------------------------------
        void DrawActionState(ActionState actionState, Vector2 size)
        {
            if (m_pActor == null || m_pActor.GetActionStateGraph() == null) return;

            if (actionState == null) return;
            EditorUtil.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(size.x-20) });
            if(GUILayout.Button("复制"))
            {
                m_pCopyActionState = JsonUtility.ToJson(actionState.GetCore());
            }
            if (m_pCopyActionState!= null && GUILayout.Button("黏贴"))
            {
                actionState.Copy(m_pActor.GetActionStateGraph(), m_pCopyActionState);
            }
            if(m_pGraphBinder!=null && m_pActorPrefab && GUILayout.Button("刷新本地动作"))
            {
                RefreshLocalAnimation(m_pGraphBinder, m_pActorPrefab.name);
            }
            EditorUtil.EndHorizontal();

            m_bEpxandActionCore = EditorGUILayout.Foldout(m_bEpxandActionCore, "基础信息", true);
            if(m_bEpxandActionCore)
            {
                EditorGUI.indentLevel++;
                {
                    float remainWidth = size.x - 5;
                    GUILayoutOption[] op = new GUILayoutOption[] { GUILayout.Width(remainWidth) };

                    ActionStateCore pCore = actionState.GetCore();
                    string strName = pCore.name;
                    pCore.name = EditorGUILayout.TextField("Name", pCore.name, op);
                    pCore.forbidJump = EditorGUILayout.Toggle("禁止跳跃", pCore.forbidJump, op);
                    pCore.forbidMove = EditorGUILayout.Toggle("禁止移动", pCore.forbidMove, op);
                    pCore.hard_body = EditorGUILayout.Toggle("霸体", pCore.hard_body, op);
                    pCore.turnSpeed = EditorGUILayout.FloatField("转向时长(<0不覆盖)", pCore.turnSpeed, op);

                    float backlabelWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = size.x - 120;
                    pCore.bOverride = EditorGUILayout.Toggle("动作部位覆盖", pCore.bOverride, op);
                    pCore.bDummy = EditorGUILayout.Toggle("虚拟动作(用于只触发绑定的事件和动作属性)", pCore.bDummy, op);
                    EditorGUIUtility.labelWidth = backlabelWidth;
                    if (strName.CompareTo(pCore.name) != 0)
                    {
                        if (!ActionStateManager.getInstance().ChangeActionStateName(m_pActor.GetActionStateGraph(), actionState, pCore.name))
                            pCore.name = strName;
                    }

                    EActionStateType preType = pCore.type;
                    uint preTag = pCore.tag;

                    pCore.type = (EActionStateType)HandleUtilityWrapper.PopEnum("动作类型", pCore.type, null, op);
                    ushort preIndex = pCore.id;
                    pCore.id = (ushort)EditorGUILayout.IntField("ID", (int)pCore.id, op);
                    if (pCore.id != preIndex)
                    {
                        if (!ActionStateManager.getInstance().ChangeActionStateID(m_pActor.GetActionStateGraph(), actionState, pCore.id))
                            pCore.id = preIndex;
                    }
                    //    pCore.priority = (byte)EditorGUILayout.IntField("Priority", (int)pCore.priority, op);
//                     if(actionState.State!=null)
//                     {
//                         if (actionState.State != null && actionState.State.speedParameterActive)
//                             pCore.multiParameter = Animator.StringToHash(actionState.State.speedParameter);
//                     }
                    pCore.tag = (ushort)EditorGUILayout.IntField("tag", (int)pCore.tag, op);

                    if (preType != pCore.type || preTag != pCore.tag)
                    {
                        ChangeActionStateTypeOrTag(preType, preTag, actionState);
                    }

                    //if (m_pActor.GetActionStateGraph().IsUsedPlayableGraph())
                    //{
                    //    if (actionState.playableState == null)
                    //    {
                    //        actionState.playableState = new ActionGraphBinder.State();
                    //    }
                    //    DrawPlayableState(actionState, size);
                    //}
                    {
                        if (!pCore.bDummy)
                        {
                            DrawAnimationSeq(actionState, new Vector2(remainWidth, size.y));
                        }
                    }
                    pCore.animLoadType = (EAnimLoadType)Framework.ED.HandleUtilityWrapper.PopEnum("动作资源加载模式", pCore.animLoadType, null, op);

                    pCore.duration = EditorGUILayout.FloatField("Duration", pCore.duration, op);
                    pCore.scale_animation_speed = EditorGUILayout.Toggle("CanScaleSpeed", pCore.scale_animation_speed, op);
                    pCore.loop = (short)Mathf.Clamp(EditorGUILayout.IntField("Loop", (int)pCore.loop, op), 0, 255);
                    //     pCore.can_dir = EditorGUILayout.Toggle("CanDir", pCore.can_dir, op);
                    pCore.can_clamp = EditorGUILayout.Toggle("CanClamp", pCore.can_clamp, op);
                    pCore.cancle_priority = (short)EditorGUILayout.IntField("CanclePriority", pCore.cancle_priority, op);
                    pCore.cancle_on_hit = EditorGUILayout.Toggle("CancleOnHit", pCore.cancle_on_hit, op);
                    pCore.cancle_begin = EditorGUILayout.FloatField("CancleBegin", pCore.cancle_begin, op);
                    pCore.cancle_end = EditorGUILayout.FloatField("CancleEnd", pCore.cancle_end, op);
                    pCore.switch_stance = EditorGUILayout.TextField("状态器", pCore.switch_stance, op);
                    pCore.revert_direction = EditorGUILayout.Toggle("反向朝向", pCore.revert_direction, op);
                    pCore.face_front_back = EditorGUILayout.Toggle("指定朝向", pCore.face_front_back, op);
                    if (pCore.face_front_back)
                    {
                        if (pCore.face_front_back)
                        {
                            pCore.face_front_back_angle = EditorGUILayout.Slider("面朝角度偏移", pCore.face_front_back_angle, -90, 90, op);
                        }
                    }
                    else
                    {
                        pCore.face_target = EditorGUILayout.Toggle("面朝目标", pCore.face_target, op);
                    }

                    if (pCore.type == EActionStateType.AttackGround || pCore.type == EActionStateType.AttackAir)
                        pCore.attack_speed = EditorGUILayout.Toggle("AttackSpeed", pCore.attack_speed, op);
                    pCore.invisible_begin = EditorGUILayout.FloatField("InvisibleBegin", pCore.invisible_begin, op);
                    pCore.invisible_end = EditorGUILayout.FloatField("InvisibleEnd", pCore.invisible_end, op);
                    pCore.actor_name_offset = EditorGUILayout.Vector3Field("ActorNameOffset", pCore.actor_name_offset, op);

                    EditorUtil.BeginHorizontal();
                    actionState.bExpandBlend = EditorGUILayout.Foldout(actionState.bExpandBlend, "Blends", true);
                    EditorUtil.EndHorizontal();
                    if (actionState.bExpandBlend)
                    {
                        EditorGUI.indentLevel++;
                        float remainWidth1 = size.x - 10;
                        GUILayoutOption[] op1 = new GUILayoutOption[] { GUILayout.Width(remainWidth) };
                        pCore.blend_offset_time = EditorGUILayout.FloatField("BlendOffsetTime", pCore.blend_offset_time, op1);
                        pCore.blend_animation_time = EditorGUILayout.FloatField("BlendAnimationTime", pCore.blend_animation_time, op1);
                        EditorGUI.indentLevel--;
                    }

                    actionState.SetCore(pCore);
                }
                EditorGUI.indentLevel--;
            }
            DrawActionEvents(actionState, size);
            DrawActionPropertys(actionState, size);
        }
        //-----------------------------------------------------
        void DrawAnimationSeq(ActionState pState, Vector2 size)
        {
            size.x -= 30;
            ActionStateCore pCore = pState.GetCore();
            GUILayoutOption[] op = new GUILayoutOption[] { GUILayout.Width(size.x-2) };
            EditorUtil.BeginHorizontal(op);
            m_bEpxandAnimationSeq = EditorGUILayout.Foldout(m_bEpxandAnimationSeq, "动作序列");
            if(GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(50) }))
            {
                List<AnimationSeq> vReqs = pCore.animations!=null?new List<AnimationSeq>(pCore.animations): new List<AnimationSeq>();
                vReqs.Add(new AnimationSeq() { animation = "", hashState = 0, duration = 0 });
                pCore.animations = vReqs.ToArray();
                m_bEpxandAnimationSeq = true;
            }
            if (pCore.animations!=null)
            {
                float duration = 0;
                if(pCore.seqType == EAnimSeqType.Sequence)
                {
                    for (int i = 0; i < pCore.animations.Length; ++i)
                        duration += pCore.animations[i].duration;
                }
                else if (pCore.seqType == EAnimSeqType.Random)
                {
                    for (int i = 0; i < pCore.animations.Length; ++i)
                    {
                        duration = Mathf.Max(duration, pCore.animations[i].duration);
                    }
                }
                else if (pCore.seqType == EAnimSeqType.CountSwitch)
                {
                    for (int i = 0; i < pCore.animations.Length; ++i)
                    {
                        duration = Mathf.Max(duration, pCore.animations[i].duration);
                    }
                }
                else if (pCore.seqType == EAnimSeqType.BlendTreeSwitch)
                {
                    for (int i = 0; i < pCore.animations.Length; ++i)
                    {
                        duration = Mathf.Max(duration, pCore.animations[i].duration);
                    }
                }
                pCore.duration = duration;
            }
            if (m_pGraphBinder!=null && m_pGraphBinder.usePlayableGraph)
            {
                if (pCore.bOverride)
                {
                    for (int i = 0; i < pCore.animations.Length; ++i)
                    {
                        if (pCore.animations[i].layer <= 0)
                            pCore.animations[i].layer = 1;
                    }
                }
            }
            pCore.seqType = (EAnimSeqType)HandleUtilityWrapper.PopEnum(null, pCore.seqType, null, new GUILayoutOption[] { GUILayout.MaxWidth(100) });
            if (pCore.isNullAnimation()) EditorGUILayout.HelpBox("动作为空", MessageType.Warning);
            EditorUtil.EndHorizontal();

            if (m_bEpxandAnimationSeq)
            {
                float actionWidth = 250;
                float updwWidth = 25;
                float ctlWidth = 20;
                int headCnt = pCore.scale_animation_speed ? 4 : 3;
                if (pCore.seqType == EAnimSeqType.CountSwitch || pCore.seqType == EAnimSeqType.Random || pCore.seqType == EAnimSeqType.BlendTreeSwitch)
                    headCnt++;
                else if(pCore.seqType == EAnimSeqType.Sequence && pCore.loop !=1)
                {
                    headCnt++;
                }
                float aniWidth = (size.x - 5 - updwWidth- ctlWidth- actionWidth) / headCnt;
                GUILayoutOption[] actionWidthOp = new GUILayoutOption[] { GUILayout.Width(actionWidth) };
                GUILayoutOption[] titleOp = new GUILayoutOption[] { GUILayout.Width(aniWidth) };
                GUILayoutOption[] updwWidthOp = new GUILayoutOption[] { GUILayout.Width(updwWidth) };
                GUILayoutOption[] ctlWidthOp = new GUILayoutOption[] { GUILayout.Width(ctlWidth) };

                //   EditorGUI.indentLevel++;
                EditorUtil.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(size.x-10) });
                GUILayout.Space(30);
                GUILayout.Label("动作名", actionWidthOp);
                GUILayout.Label("Layer", titleOp);
                GUILayout.Label("MultiSpeed", titleOp);
                if (pCore.scale_animation_speed)
                    GUILayout.Label("ScaleSpeed", titleOp);
                GUILayout.Label("时长", titleOp);
                if (pCore.seqType == EAnimSeqType.Random)
                {
                    GUILayout.Label("权重", titleOp);
                }
                else if (pCore.seqType == EAnimSeqType.CountSwitch)
                {
                    GUILayout.Label("达成次数", titleOp);
                }
                else if (pCore.seqType == EAnimSeqType.Sequence)
                {
                    if(pCore.loop != 1) GUILayout.Label("播放次数", titleOp);
                }
                else if (pCore.seqType == EAnimSeqType.BlendTreeSwitch)
                {
                    GUILayout.Label("融合因子", titleOp);
                }
                GUILayout.Label("", updwWidthOp);
                GUILayout.Label("", ctlWidthOp);
                EditorUtil.EndHorizontal();


                if (pCore.animations!=null)
                {
                    Color bakColor = GUI.color;
                    for (int i = 0; i < pCore.animations.Length; ++i)
                    {
                        AnimationSeq seq = pCore.animations[i];
                        GUI.color = bakColor;
                        if (pCore.seqType == EAnimSeqType.CountSwitch)
                        {
                            if (m_nPlayActionCnt == seq.externParam)
                                GUI.color = Color.red;
                        }
                        EditorUtil.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(size.x-10) });
                        int layer = seq.layer;
                        AnimatorState editState = DrawAnimation(ref seq.animation, ref layer, null, actionWidth, actionWidthOp);
                        bool bChange = false;
                        if (seq.state != editState)
                        {
                            if (editState != null && editState.speedParameterActive)
                                seq.multiParameter = Animator.StringToHash(editState.speedParameter);
                            else
                                seq.multiParameter = 0;
                            bChange = true;
                            seq.state = editState;
                            if (m_AniPlayback != null && m_pActor.GetObjectAble()!=null)
                                m_AniPlayback.SetTarget(m_pActor.GetObjectAble().GetObject(), pState, m_pBaseCrossState);
                        }

                        if (seq.state != null)
                        {
                            if (seq.state.motion is AnimationClip)
                            {
                                AnimationClip clip = (seq.state.motion as AnimationClip);
                                if (seq.duration <= 0) seq.duration = clip.length;
                                if (bChange)
                                {
                                    seq.scale_speed = seq.state.speed;
                                }
                            }
                            else if (seq.state.motion is BlendTree)
                            {
                                BlendTree clip = seq.state.motion as BlendTree;
                                if (seq.duration <= 0) seq.duration = clip.averageDuration;
                                if (bChange)
                                {
                                    seq.scale_speed = seq.state.speed;
                                }
                            }
                        }
                        seq.layer = (byte)layer;
                        seq.hashState = Animator.StringToHash(seq.animation);

                        seq.layer = (byte)EditorGUILayout.IntField((int)seq.layer, titleOp);

                        seq.multiParameter = PopParameter(seq.multiParameter, null, titleOp);

                        if (pCore.scale_animation_speed)
                            seq.scale_speed = EditorGUILayout.Slider( seq.scale_speed, 0, 5, titleOp);
                        else
                            seq.scale_speed = 1;
                        seq.duration = EditorGUILayout.FloatField(seq.duration, titleOp);

                        if (pCore.seqType == EAnimSeqType.Random)
                        {
                            seq.externParam = EditorGUILayout.IntField(seq.externParam, titleOp);
                        }
                        else if (pCore.seqType == EAnimSeqType.CountSwitch)
                        {
                            seq.externParam = EditorGUILayout.IntField(seq.externParam, titleOp);
                        }
                        else if (pCore.seqType == EAnimSeqType.Sequence)
                        {
                            if (pCore.loop != 1)
                                seq.externParam = EditorGUILayout.IntField(seq.externParam, titleOp);
                        }
                        else if (pCore.seqType == EAnimSeqType.BlendTreeSwitch)
                        {
                            seq.externParam = (int)(EditorGUILayout.FloatField(seq.externParam*0.01f, titleOp)*100);
                        }

                        if(i > 0 && GUILayout.Button("↑"))
                        {
                            List<AnimationSeq> vReqs = new List<AnimationSeq>(pCore.animations);
                            vReqs.RemoveAt(i);
                            vReqs.Insert(i-1, seq);
                            pCore.animations = vReqs.ToArray();
                           // EditorUtil.EndHorizontal();
                            break;
                        }
                        if (i < pCore.animations.Length-1 && GUILayout.Button("↓"))
                        {
                            List<AnimationSeq> vReqs = new List<AnimationSeq>(pCore.animations);
                            vReqs.RemoveAt(i);
                            vReqs.Insert(i+1, seq);
                            pCore.animations = vReqs.ToArray();
                           // EditorUtil.EndHorizontal();
                            break;
                        }
                        if (i == 0 && i == pCore.animations.Length - 1)
                            GUILayout.Label("--", updwWidthOp);
                       // EditorUtil.EndHorizontal();

                        if (GUILayout.Button("-", ctlWidthOp))
                        {
                            if(EditorUtility.DisplayDialog("提示", "确定删除?","删", "等等"))
                            {
                                List<AnimationSeq> vReqs = new List<AnimationSeq>(pCore.animations);
                                vReqs.RemoveAt(i);
                                pCore.animations = vReqs.ToArray();
                            //    EditorUtil.EndHorizontal();
                                break;
                            }
                        }
                        GUI.color = bakColor;
                        EditorUtil.EndHorizontal();
                    }
                }
           //     EditorGUI.indentLevel--;
            }
            if(pCore.seqType == EAnimSeqType.BlendTreeSwitch)
            {
                int selextIndex = -1;
                if (!string.IsNullOrEmpty(pCore.actionParameter))
                {
                    for (int i = 0; i < BLEND_PARAMES_NAMES.Length; ++i)
                    {
                        if (BLEND_PARAMES_NAMES[i] != null && BLEND_PARAMES_NAMES[i].CompareTo(pCore.actionParameter) == 0)
                        {
                            selextIndex = i;
                        }
                    }
                }
                else
                    selextIndex = 0;

                selextIndex = EditorGUILayout.Popup("融合因子", selextIndex, BLEND_PARAMES_POP_NAMES);
                if (selextIndex >= 0 && selextIndex < BLEND_PARAMES_NAMES.Length)
                {
                    pCore.actionParameter = BLEND_PARAMES_NAMES[selextIndex];
                }
                else
                    pCore.actionParameter = null;
            }
        }
        //-----------------------------------------------------
        Vector2 m_fFrameScroll = Vector2.zero;
        void DrawActionFrameParameter(ActionState actionState, Vector2 size)
        {
            if (actionState == null) return;

            ActionStateCore pCore = actionState.GetCore();
            using (new UnityEngine.GUILayout.HorizontalScope("box"))
            {
                EditorUtil.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(size.x-2) });
                if (m_nSelectFrame >=0 && m_nSelectFrame < pCore.frame.Count && GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20) }))
                {
                    if (EditorUtility.DisplayDialog("tips", "确定删除?", "删除", "取消"))
                    {
                        DelActionFrame(pCore.frame[m_nSelectFrame]);
                        m_nSelectFrame = -1;
                     //   EditorUtil.EndHorizontal();
                        return;
                    }
                }
                m_bExpandActionFrameParameter = EditorGUILayout.Foldout(m_bExpandActionFrameParameter, "ActionFrames", true);
                EditorUtil.EndHorizontal();
            }
            if (m_bExpandActionFrameParameter)
            {
                float remainWidth = size.x - 60;
                GUILayoutOption[] op = new GUILayoutOption[] { GUILayout.Width(remainWidth) };
                EditorGUI.indentLevel++;

                List<string> frames = new List<string>();
                if (pCore.frame == null)
                {
                    pCore.frame = new List<ActionFrame>();
                }
                for (int i = 0; i < pCore.frame.Count; ++i)
                {
                    frames.Add("Frame[" + pCore.frame[i].id + "]");
                }

                ActionFrame frame = null;
                if (m_nSelectFrame < 0 && pCore.frame.Count > 0)
                    m_nSelectFrame = 0;

                m_nSelectFrame = EditorGUILayout.Popup(m_nSelectFrame, frames.ToArray(), op);
                if (m_nSelectFrame >= 0 && m_nSelectFrame < pCore.frame.Count)
                    frame = pCore.frame[m_nSelectFrame];
                if (frame != null)
                {
                    DrawActionFrame(frame, remainWidth, op);
                }
                using (new UnityEngine.GUILayout.HorizontalScope("box"))
                {
                    EditorUtil.BeginHorizontal();
                    EditorGUILayout.LabelField("Add ActionFrame...", new GUILayoutOption[] { GUILayout.Width(size.x - 60) });
                    if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
                    {
                        AddActionFrame();
                    }
                    EditorUtil.EndHorizontal();
                }


                EditorGUI.indentLevel--;
            }

            actionState.SetCore(pCore);
        }
        //-----------------------------------------------------
        void DrawStateFrame(StateFrame frame, float gapWidth, GUILayoutOption[] op)
        {
            if (frame == null) return;
            frame.id = (byte)EditorGUILayout.IntField("id", frame.id, op);
            frame.name = EditorGUILayout.TextField("name", frame.name, op);
            frame.property_id_launch = (uint)EditorGUILayout.FloatField("PropertyOnLaunch", frame.property_id_launch, op);
            if(frame.IsPartTargetFrame())
            {
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "bodyPartID");
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "bodyPartPriority");
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "bodyPartState");
            }

            if (frame.IsAttackFrame())
            {
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "damage");
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "buff");
                frame.attack_layer_id = (byte)EditorGUILayout.IntField("AttackLayerID", (int)frame.attack_layer_id, op);
                frame.multiple_hit = EditorGUILayout.Toggle("MultipleHit", frame.multiple_hit, op);
            }
            if(frame.IsTargetFrame())
                frame.penetrable = EditorGUILayout.Toggle("穿透", frame.penetrable, op);

            if(frame.IsGrabAttackFrame())
            {
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "grab_slot", null, "抓方绑点");
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "grab_offset", null, "抓方绑点偏移");
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "target_grab_slot", null, "被抓方绑点");
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "target_grab_offset", null, "被抓方绑点偏移");
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "use_grab_slot_direction", null, "使用抓点作为被抓方朝向");
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "grab_hold_action", null, "被抓方被抓时保持动作");
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "grab_lerp_time", null, "抓取快慢因子");
            }

            EditorUtil.BeginHorizontal(op);
            frame.bind_slot = PopSlot(frame.bind_slot);
            EditorUtil.EndHorizontal();
            if(!string.IsNullOrEmpty(frame.bind_slot))
            {
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "bindslotBit");
            }
            if (frame.IsPartTargetFrame())
            {
                EditorUtil.BeginHorizontal(op);
                ActionState pState = m_pActor.GetActionStateGraph().FindActionState(frame.listenActionHide);
                pState = PopState(m_pActor.GetActionStateGraph().GetCurrentStance(), pState, "动作监听类型");
                if (pState != null)
                {
                    frame.listenActionHide = pState.GetCore().id;
                }
                if (GUILayout.Button("清除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    frame.listenActionHide = 0;
                }
                EditorUtil.EndHorizontal();
            }
            if (frame.total_volume == null) frame.total_volume = new List<StateFrame.Volume>();
            for (int i = 0; i < frame.total_volume.Count; ++i)
            {
                StateFrame.Volume pVolume = frame.total_volume[i];
                EditorUtil.BeginHorizontal();
                EditorGUILayout.Foldout(true, "Volume[" + (i + 1) + "]", true);
                if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    frame.total_volume.RemoveAt(i);
                    break;
                }
                if (GUILayout.Button("复制", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    m_pCopyVolume = pVolume;
                    m_bCopyVolume = true;
                    break;
                }
                if (m_bCopyVolume && GUILayout.Button("粘贴", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    pVolume.flag = m_pCopyVolume.flag;
                    pVolume.min = Vector3.Min(m_pCopyVolume.min, m_pCopyVolume.max);
                    pVolume.max = Vector3.Max(m_pCopyVolume.min, m_pCopyVolume.max);
                }
                EditorUtil.EndHorizontal();
                {
                    EditorGUI.indentLevel++;
                    bool IsTarget = EditorGUILayout.Toggle("受击", (pVolume.flag & (int)EVolumeFlag.Target) != 0, op);
                    bool IsAttack = EditorGUILayout.Toggle("攻击", (pVolume.flag & (int)EVolumeFlag.Attack) != 0, op);
                    bool IsAttackInvert = EditorGUILayout.Toggle("攻击反射", (pVolume.flag & (int)EVolumeFlag.AttackInvert) != 0, op);
                    bool IsAttackPre = EditorGUILayout.Toggle("预判", (pVolume.flag & (int)EVolumeFlag.AttackPre) != 0, op);
                    bool isPartTarget = EditorGUILayout.Toggle("部位", (pVolume.flag & (int)EVolumeFlag.PartTarget) != 0, op);
                    bool isGrabTarget = EditorGUILayout.Toggle("被抓框", (pVolume.flag & (int)EVolumeFlag.GrabTarget) != 0, op);
                    bool isGrabAttack = EditorGUILayout.Toggle("抓取框", (pVolume.flag & (int)EVolumeFlag.GrabAttack) != 0, op);
                    bool isAttackGrabingTarget = EditorGUILayout.Toggle("抓物攻击框", (pVolume.flag & (int)EVolumeFlag.AttackGrabingTarget) != 0, op);

                    Vector3 vMin = EditorGUILayout.Vector3Field("VolumeMix", pVolume.min, op);
                    Vector3 vMax = EditorGUILayout.Vector3Field("VolumeMax", pVolume.max, op);
                    pVolume.min = Vector3.Min(vMin, vMax);
                    pVolume.max = Vector3.Max(vMin, vMax);

                    EditorGUI.indentLevel--;

                    if (IsTarget)
                    {
                        pVolume.flag |= (int)EVolumeFlag.Target;
                    }
                    else
                    {
                        pVolume.flag &= ~((uint)EVolumeFlag.Target);
                    }
                    if (IsAttack)
                    {
                        pVolume.flag |= (int)EVolumeFlag.Attack;
                    }
                    else
                    {
                        pVolume.flag &= ~((uint)EVolumeFlag.Attack);
                    }
                    if (IsAttackPre)
                    {
                        pVolume.flag |= (int)EVolumeFlag.AttackPre;
                    }
                    else
                    {
                        pVolume.flag &= ~((uint)EVolumeFlag.AttackPre);
                    }
                    if (IsAttackInvert)
                    {
                        pVolume.flag |= (int)EVolumeFlag.AttackInvert;
                    }
                    else
                    {
                        pVolume.flag &= ~((uint)EVolumeFlag.AttackInvert);
                    }
                    if (isPartTarget)
                    {
                        pVolume.flag |= (int)EVolumeFlag.PartTarget;
                    }
                    else
                    {
                        pVolume.flag &= ~((uint)EVolumeFlag.PartTarget);
                    }
                    if (isGrabTarget)
                    {
                        pVolume.flag |= (int)EVolumeFlag.GrabTarget;
                    }
                    else
                    {
                        pVolume.flag &= ~((uint)EVolumeFlag.GrabTarget);
                    }
                    if (isGrabAttack)
                    {
                        pVolume.flag |= (int)EVolumeFlag.GrabAttack;
                    }
                    else
                    {
                        pVolume.flag &= ~((uint)EVolumeFlag.GrabAttack);
                    }
                    if(isAttackGrabingTarget)
                    {
                        pVolume.flag |= (int)EVolumeFlag.AttackGrabingTarget;
                    }
                    else
                    {
                        pVolume.flag &= ~((uint)EVolumeFlag.AttackGrabingTarget);
                    }
                    frame.total_volume[i] = pVolume;
                }
            }
            using (new UnityEngine.GUILayout.HorizontalScope("box"))
            {
                EditorUtil.BeginHorizontal();
                EditorGUILayout.LabelField("Add Volume...", new GUILayoutOption[] { GUILayout.Width(gapWidth - 60) });
                if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
                {
                    StateFrame.Volume newFrame = new StateFrame.Volume();
                    frame.total_volume.Add(newFrame);
                }
                EditorUtil.EndHorizontal();
            }
        }
        //-----------------------------------------------------
        void DrawActionFrame(ActionFrame frame, float gapWidth, GUILayoutOption[] op)
        {
            if (frame == null) return;

            int selectFrameIndex = -1;
            m_vPopStateFrames.Clear();
            List<StateFrame> frames = null;
            ActionStateGraph graph = m_pActor.GetActionStateGraph();
            if(graph != null)
            {
                frames = graph.GetStateFrames();
                if (frames != null)
                {
                    for (int i = 0; i < frames.Count; ++i)
                    {
                        m_vPopStateFrames.Add(frames[i].name + "[" + frames[i].id + "]");
                        if(frame.pFrame!=null && frames[i].id == frame.pFrame.id)
                        {
                            selectFrameIndex = i;
                        }
                    }
                }
            }


            frame.start = EditorGUILayout.FloatField("Start", frame.start, op);
            frame.duration = EditorGUILayout.FloatField("Duration", frame.duration, op);
            if(frames!=null)
            {
                EditorUtil.BeginHorizontal(op);
                selectFrameIndex = EditorGUILayout.Popup("frameID", selectFrameIndex, m_vPopStateFrames.ToArray());
                if (selectFrameIndex >= 0 && selectFrameIndex < frames.Count)
                {
                    frame.pFrame = frames[selectFrameIndex];
                    frame.frameID = frame.pFrame.id;
                }
                if (GUILayout.Button("清除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    frame.frameID = 0xff;
                    frame.pFrame = null;
                }
                if (frame.pFrame == null)
                    EditorGUILayout.HelpBox("Lost", MessageType.Error);
                EditorUtil.EndHorizontal();
            }
            else
            {
                EditorUtil.BeginHorizontal(op);
                EditorGUILayout.HelpBox("frameID Lost", MessageType.Error);
                if (GUILayout.Button("清除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    frame.frameID = 0xff;
                    frame.pFrame = null;
                }
                if(frame.pFrame == null)
                    EditorGUILayout.HelpBox("Lost", MessageType.Error);
                EditorUtil.EndHorizontal();
            }
            if (frame.IsAttackFrame())
            {
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "damage", null, "技能组ID");
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "buff");

                frame.stuck_time_hit = EditorGUILayout.FloatField("StuckTimeOnHit", frame.stuck_time_hit, op);
                frame.hit_rate_base = EditorGUILayout.FloatField("RateOnHit", frame.hit_rate_base, op);
                frame.target_direction_postion = EditorGUILayout.Toggle("TargetDirectionByPostion", frame.target_direction_postion, op);
                frame.target_duration_hit = EditorGUILayout.FloatField("TargetDurationOnHit", frame.target_duration_hit, op);
                frame.target_effect_hit = EditorKits.DrawUIObjectByPathNoLayout<GameObject>("击中特效", frame.target_effect_hit);
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "effect_hit_slot", null, "击中特效绑点");
                frame.target_effect_hit_offset = EditorGUILayout.Vector3Field("EffectOnHitOffset", frame.target_effect_hit_offset, op);
                HandleUtilityWrapper.DrawPropertyByFieldName(frame, "target_hit_flag");
                if(frame.sound_hit_id<=0)
                    HandleUtilityWrapper.DrawPropertyByFieldName(frame, "sound_hit");
                if (string.IsNullOrEmpty(frame.sound_hit))
                    HandleUtilityWrapper.DrawPropertyByFieldName(frame, "sound_hit_id");

                EditorGUILayout.Foldout(true, "目标受击反馈", true);
                {
                    m_vPopStatePropertysName.Clear();
                    m_vPopStatePropertysName.Add("None");
                    if (m_pCurActionState!=null)
                    {
                        for (int i = 0; i < m_pCurActionState.GetCore().action_property_data.Count; ++i)
                            m_vPopStatePropertysName.Add(m_pCurActionState.GetCore().action_property_data[i].strName + "[" + i + "]");
                    }


                    EditorGUI.indentLevel++;
                    EditorGUILayout.Foldout(true, "动作反馈", true);
                    frame.target_action_hit_ground = (EActionStateType)HandleUtilityWrapper.PopEnum("对地", frame.target_action_hit_ground, null, op);
                    frame.target_action_hit_ground_back = (EActionStateType)HandleUtilityWrapper.PopEnum("对地-背后受击", frame.target_action_hit_ground_back, null, op);
                    frame.target_action_hit_air = (EActionStateType)HandleUtilityWrapper.PopEnum("对空", frame.target_action_hit_air, null, op);
                    frame.target_action_hit_air_back = (EActionStateType)HandleUtilityWrapper.PopEnum("对空-背后受击", frame.target_action_hit_air_back, null, op);

                    EditorGUILayout.Foldout(true, "物理反馈", true);
                    frame.target_property_hit_ground = (uint)EditorGUILayout.Popup("对地", (int)frame.target_property_hit_ground, m_vPopStatePropertysName.ToArray(), op);
                    frame.target_property_hit_ground_back = (uint)EditorGUILayout.Popup("对地-背后受击", (int)frame.target_property_hit_ground_back, m_vPopStatePropertysName.ToArray(), op);
                    frame.target_property_hit_air = (uint)EditorGUILayout.Popup("对空", (int)frame.target_property_hit_air, m_vPopStatePropertysName.ToArray(), op);
                    frame.target_property_hit_air_back = (uint)EditorGUILayout.Popup("对空-背后受击", (int)frame.target_property_hit_air_back, m_vPopStatePropertysName.ToArray(), op);
                    EditorGUI.indentLevel--;
                }
            }
            frame.RefreshVolume();
        }
        //-----------------------------------------------------
        public void EditEventBit(BaseEventParameter evtParameter, EEventBit layer)
        {
            if (evtParameter == null) return;
            evtParameter.SetEventBit(layer, !evtParameter.IsEventBit(layer));
        }
        //-----------------------------------------------------
        public void EditEvent(ActionState actionState, BaseEventParameter evtParameter)
        {
            if (actionState == null) return;
            if(actionState != m_pCurActionState)
            {
                m_pCurActionState = actionState;
                OnChangeActionState();
            }
            if (!m_bEpxandActionEvent) m_bEpxandActionEvent = true;
            List<BaseEventParameter> vEvnets = new List<BaseEventParameter>();
            m_pCurActionState.GetCore().action_event_core.BuildEvent(m_pGameModuel, vEvnets);
            for(int i = 0; i < vEvnets.Count; ++i)
            {
                vEvnets[i].bExpand = false;
            }
            evtParameter.bExpand = true;

            for (int i = 0; i < m_pCurActionState.GetCore().action_property_data.Count; ++i)
            {
                ActionStatePropertyData propertyData = m_pCurActionState.GetCore().action_property_data[i];
                propertyData.bExpand = false;
                m_pCurActionState.GetCore().action_property_data[i] = propertyData;
            }
            m_bEpxandActionProperty = false;
        }
        //-----------------------------------------------------
        void DrawActionEvents(ActionState actionState, Vector2 size)
        {
            if (actionState == null) return;
            ActionStateCore pCore = actionState.GetCore();
            m_bEpxandActionEvent = EditorGUILayout.Foldout(m_bEpxandActionEvent, "EventList", true);
            if (m_bEpxandActionEvent)
            {
                EditorGUI.indentLevel++;

                //! draw event
                DrawEventCore.Draw(pCore.action_event_core,m_PopSlots, OnDrawEventCallback, -1);


                float remainWidth = size.x - 60;
                GUILayoutOption[] op = new GUILayoutOption[] { GUILayout.Width(remainWidth) };

                using (new UnityEngine.GUILayout.HorizontalScope("box"))
                {
                    EditorUtil.BeginHorizontal();
                    EditorGUILayout.LabelField("Add Event...");
                    m_AddEventType = EventPopDatas.DrawEventPop(m_AddEventType, "");
                    EditorUtil.BeginDisabledGroup(m_AddEventType == EEventType.Base || m_AddEventType == EEventType.Count);
                    if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
                    {
                        BaseEventParameter parame = BuildEventUtl.BuildEventByType(null, m_AddEventType);
                        if(parame!=null)
                        {
                            pCore.action_event_core.AddEvent(parame);
                            m_AddEventType = EEventType.Count;
                        }
                    }
                    EditorUtil.EndDisabledGroup();
                    EditorUtil.EndHorizontal();
                }

                GUILayoutOption[] sub_op = new GUILayoutOption[] { GUILayout.Width(size.x - 45) };

                EditorGUI.indentLevel--;
            }
            actionState.SetCore(pCore);
        }
        //-----------------------------------------------------
        void OnDrawEventCallback(BaseEventParameter evt, string strField)
        {
            if (string.IsNullOrEmpty(strField)) return;
            if(m_AniPlayback !=null && strField.CompareTo("triggetTime") == 0)
            {
                if (GUILayout.Button("设置当前时间", new GUILayoutOption[] { GUILayout.Width(100) }))
                {
                    evt.triggetTime = m_AniPlayback.RuningTime;
                }
            }
        }
        //-----------------------------------------------------
        public void EditActionStateProperty(ActionState actionState, int index)
        {
            if (actionState == null) return;
            if (actionState != m_pCurActionState)
            {
                m_pCurActionState = actionState;
                OnChangeActionState();
            }
            if (m_pCurActionState.GetCore().action_property_data == null) return;
            if (!m_bEpxandActionProperty) m_bEpxandActionProperty = true;

            for(int i = 0; i < m_pCurActionState.GetCore().action_property_data.Count; ++i)
            {
                ActionStatePropertyData propertyData= m_pCurActionState.GetCore().action_property_data[i];
                propertyData.bExpand = index == i;
                m_pCurActionState.GetCore().action_property_data[i] = propertyData;
            }

            m_bEpxandActionEvent = false;
            List<BaseEventParameter> vEvnets = new List<BaseEventParameter>();
            m_pCurActionState.GetCore().action_event_core.BuildEvent(m_pGameModuel, vEvnets);
            for (int i = 0; i < vEvnets.Count; ++i)
            {
                vEvnets[i].bExpand = false;
            }
        }
        //-----------------------------------------------------
        void DrawActionPropertys(ActionState actionState, Vector2 size)
        {
            if (actionState == null) return;
            ActionStateCore pCore = actionState.GetCore();
            m_bEpxandActionProperty = EditorGUILayout.Foldout(m_bEpxandActionProperty, "PropertyList", true);
            if (m_bEpxandActionProperty)
            {
                EditorGUI.indentLevel++;

                //! draw event
                DrawPropertyCore.Draw(actionState,pCore.action_property_data);


                float remainWidth = size.x - 60;
                GUILayoutOption[] op = new GUILayoutOption[] { GUILayout.Width(remainWidth) };

                using (new UnityEngine.GUILayout.HorizontalScope("box"))
                {
                    EditorUtil.BeginHorizontal();
                    EditorGUILayout.LabelField("Add Property...");
                    if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20) }))
                    {
                        ActionStatePropertyData parame = new ActionStatePropertyData();
                        if (pCore.action_property_data == null) pCore.action_property_data = new List<ActionStatePropertyData>();
                        pCore.action_property_data.Add(parame);
                        m_AddEventType = EEventType.Count;
                        actionState.RebuildActionStatePropertyMap();
                    }
                }
                EditorUtil.EndHorizontal();

                GUILayoutOption[] sub_op = new GUILayoutOption[] { GUILayout.Width(size.x - 45) };

                EditorGUI.indentLevel--;
            }
            actionState.SetCore(pCore);
        }
        //-----------------------------------------------------
        void DeleteStance(ActionStateStance pStance)
        {
            m_pActor.GetActionStateGraph().DelStance(pStance);
            OnChangeGraphStand();
        }
        //------------------------------------------------------
        void AddActionFrame()
        {
            ActionStateCore pCore = m_pCurActionState.GetCore();
            ActionFrame pNewFrame = new ActionFrame();
            pCore.frame.Add(pNewFrame);

            m_nSelectFrame = pCore.frame.Count - 1;
        }
        //------------------------------------------------------
        void DelActionFrame(ActionFrame pFrame)
        {
            ActionStateCore pCore = m_pCurActionState.GetCore();
            for(int i = 0; i < pCore.frame.Count; ++i)
            {
                if(pCore.frame[i] == pFrame)
                {
                    pCore.frame.RemoveAt(i);
                    break;
                }
            }
            m_pCurActionState.Begin(0.0f, 1.0f);
        }
        //-----------------------------------------------------
        void DeleteActionState(ActionState pActionState)
        {
            if (pActionState == m_pActor.GetCurrentActionState())
                m_pActor.StopCurrentActionState();
            ActionStateManager.getInstance().DeleteActionStateFromGraph(m_pActor.GetActionStateGraph(), pActionState);
        }
        //------------------------------------------------------
        void ChangeActionStateTypeOrTag(EActionStateType preState, uint preTag, ActionState pState)
        {
            if (m_pActor.GetActionStateGraph() == null) return;
        }
        //-----------------------------------------------------
        ActionState NewAcionState(string strActionState)
        {
            if (m_pActor.GetActionStateGraph() == null) return null;
            if (m_pActor.GetActionStateGraph().GetActionStateMap().ContainsKey(strActionState))
                return null;
            int dwActionStateID = 0;
            foreach (var db in m_pActor.GetActionStateGraph().GetActionStateIDMap())
            {
                dwActionStateID = Mathf.Max(dwActionStateID, (int)db.Key);
            }
            dwActionStateID++;

            ActionState actionState = ActionStateManager.getInstance().CreateActionStateInGraph(m_pActor.GetActionStateGraph(), strActionState, (ushort)dwActionStateID);
            if(actionState == null)
            {
                return null;
            }
            actionState.RegisterCB(m_pActor.GetActorAgent());
            return actionState;
        }
        //-----------------------------------------------------
        public void OnReLoadAssetData()
        {
            RefreshPop();
        }
        //------------------------------------------------------
        void RefreshPop(bool bReload= true)
        {
            List<string> filers = new List<string>(m_strFiler.Split(' '));
            for(int i = 0; i < filers.Count;)
            {
                if (string.IsNullOrWhiteSpace(filers[i]))
                    filers.RemoveAt(i);
                else
                    ++i;
            }

            bool bFiler = filers.Count > 0;
            m_PopDatas.Clear();
            m_PopUserDatas.Clear();
            CsvData_Player playerCsv = DataEditorUtil.GetTable<CsvData_Player>(bReload);
            if(bReload)
                DataEditorUtil.MappingTable(playerCsv);
            foreach (var db in playerCsv.datas)
            {
                bool bValid = db.Value.Models_nModelID_data != null && !string.IsNullOrEmpty(db.Value.Models_nModelID_data.strFile);
                string strItem = "角色/Battle/" + db.Value.strName.Replace("-", "/") + "[" + db.Value.ID + "]";
                if(bFiler)
                {
                    string filterItem = strItem;
                    if (bValid) filterItem += db.Value.Models_nModelID_data.strFile;
                    bool bInclude = false;
                    for(int i =0; i < filers.Count; ++i)
                    {
                        if (filterItem.Contains(filers[i]))
                        {
                            bInclude = true;
                            break;
                        }
                    }
                    if(!bInclude) continue;
                }
                m_PopUserDatas.Add(new PopData()
                {
                    ID = db.Value.ID,
                    pData = db.Value,
                    graphFile = db.Value.actionGraph,
                    strFile = !bValid ? "" : db.Value.Models_nModelID_data.strFile,
                    bHigh = false,
                }); ;
                if (!bValid) strItem += "--无效";
                m_PopDatas.Add(strItem);
            }

            foreach (var db in playerCsv.datas)
            {
                bool bValid = db.Value.Models_hModelID_data != null && !string.IsNullOrEmpty(db.Value.Models_hModelID_data.strFile);
                string strItem = "角色/Show/" + db.Value.strName.Replace("-", "/") + "[" + db.Value.ID + "]";
                if (bFiler)
                {
                    string filterItem = strItem;
                    if (bValid) filterItem += db.Value.Models_nModelID_data.strFile;

                    bool bInclude = false;
                    for (int i = 0; i < filers.Count; ++i)
                    {
                        if (filterItem.Contains(filers[i]))
                        {
                            bInclude = true;
                            break;
                        }
                    }
                    if (!bInclude) continue;
                }
                m_PopUserDatas.Add(new PopData()
                {
                    ID = db.Value.ID,
                    pData = db.Value,
                    graphFile = db.Value.showActionGraph,
                    strFile = !bValid ? "" : db.Value.Models_hModelID_data.strFile,
                    bHigh = true,
                });
                if (!bValid) strItem += "--无效";
                m_PopDatas.Add(strItem);
            }
            CsvData_Monster monsterCsv = DataEditorUtil.GetTable<CsvData_Monster>(bReload);
            if (bReload) DataEditorUtil.MappingTable(monsterCsv);
            foreach (var db in monsterCsv.datas)
            {
                bool bValid = db.Value.Models_modelId_data != null && !string.IsNullOrEmpty(db.Value.Models_modelId_data.strFile);
                string strItem = "怪物/" + db.Value.desc.Replace("-", "/") + "[" + db.Value.id + "]";
                if (bFiler)
                {
                    string filterItem = strItem;
                    if (bValid) filterItem += db.Value.Models_modelId_data.strFile;

                    bool bInclude = false;
                    for (int i = 0; i < filers.Count; ++i)
                    {
                        if (filterItem.Contains(filers[i]))
                        {
                            bInclude = true;
                            break;
                        }
                    }
                    if (!bInclude) continue;
                }
                m_PopUserDatas.Add(new PopData()
                {
                    ID = db.Value.id,
                    pData = db.Value,
                    graphFile = db.Value.actionGraph,
                    strFile = !bValid ? "" : db.Value.Models_modelId_data.strFile,
                });
                if (!bValid) strItem += "--无效";

                m_PopDatas.Add(strItem);
            }
            CsvData_Summon summonCsv = DataEditorUtil.GetTable<CsvData_Summon>(bReload);
            if (bReload) DataEditorUtil.MappingTable(summonCsv);
            foreach (var db in summonCsv.datas)
            {
                bool bValid = db.Value.Models_nModelID_data != null && !string.IsNullOrEmpty(db.Value.Models_nModelID_data.strFile);
                string strItem = "召唤物/" + db.Value.strName.Replace("-", "/") + "[" + db.Value.id + "]";
                if (bFiler)
                {
                    string filterItem = strItem;
                    if (bValid) filterItem += db.Value.Models_nModelID_data.strFile;

                    bool bInclude = false;
                    for (int i = 0; i < filers.Count; ++i)
                    {
                        if (filterItem.Contains(filers[i]))
                        {
                            bInclude = true;
                            break;
                        }
                    }
                    if (!bInclude) continue;
                }
                m_PopUserDatas.Add(new PopData()
                {
                    ID = db.Value.id,
                    pData = db.Value,
                    graphFile = db.Value.actionGraph,
                    strFile = !bValid ? "" : db.Value.Models_nModelID_data.strFile,
                });
                if (!bValid) strItem += "--无效";

                m_PopDatas.Add(strItem);
            }
        }
        //------------------------------------------------------
        void RenderVolumeByColor(List<StateFrame.Volume> pVolume, bool bMirror, Matrix4x4 mtWorld, Color dwColor, float fScale, bool bEditor = false)
        {
            if (pVolume == null) return;
            for (int i = 0; i < pVolume.Count; i++)
            {
                StateFrame.Volume volume = pVolume[i];
                Vector3 vMin = volume.min;
                Vector3 vMax = volume.max;
                if (bMirror)
                {
                    float fTemp = vMin.x;
                    vMin.x = -vMax.x;
                    vMax.x = -fTemp;
                }
                Vector3 vCenter;
                Vector3 vHalf;
                vCenter = (vMax + vMin) * 0.5f;
                vHalf = vMax - vCenter;
                Framework.Core.CommonUtility.DrawBoundingBox(vCenter, vHalf, mtWorld, dwColor, false);

                if(bEditor)
                {
                    Vector3 position = mtWorld.GetColumn(3);
                    Quaternion rotation = Quaternion.LookRotation(mtWorld.GetColumn(2), mtWorld.GetColumn(1));
                    vMin = Handles.DoPositionHandle(vMin + position, rotation) - position;
                    vMax = Handles.DoPositionHandle(vMax + position, rotation) - position;
                    volume.min = Vector3.Min(vMin, vMax);
                    volume.max = Vector3.Max(vMin, vMax);
                }
                pVolume[i] = volume;
            }
        }
    }
}