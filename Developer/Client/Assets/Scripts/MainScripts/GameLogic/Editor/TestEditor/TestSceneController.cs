using System;
using System.Collections;
using System.Collections.Generic;
using TopGame.Base;
using TopGame.Core;
using TopGame.Data;
using TopGame.UI;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework.Core;

namespace TopGame.Test
{
    public class TestSceneController : MonoBehaviour
    {
        public GameObject playerBtn;
        public GameObject monsterBtn;
        public GameObject sceneBtn;

        public GameObject clearPlayerBtn;
        public GameObject clearMonsterBtn;
        public GameObject clearModelsBtn;
        public GameObject clearSceneBtn;

        public GameObject toggleBtn;
        public RectTransform movementNode;
        bool m_IsOpen = true;


        List<ScrollListData> m_ShowScrollDatas = new List<ScrollListData>();
        List<GameObject> m_CellCache = new List<GameObject>();

        public GameObject ScrollCell;
        public Transform ScrollCellParent;

        public Transform SpawnPlayerRoot;
        public Transform SpawnMonsterRoot;
        public Transform SpawnSceneRoot;

        [Header("模型生成的X间隔")]
        public float SpawnModelSpaceX = 10;

        [Header("模型生成的Y间隔")]
        public float SpawnModelSpaceZ = 10;

        [Header("一行生成几个模型")]
        public int SpawnModelRowCount = 3;

        [Header("生成模型缩放")]
        public Vector3 SpawnModelScale = new Vector3(2,2,2);

        int m_SpawnModelIndex = 0;

        public UnityEngine.UI.Text m_pFps = null;
        int m_nFps;

        Vector3 m_SceneSpawnNextPos = Vector3.zero;

        Dictionary<SpawnType, List<AInstanceAble>> m_SpawnInstances = new Dictionary<SpawnType, List<AInstanceAble>>();



        void Start()
        {
            //初始化FileSystem
            InitFileSystem();
            //加载DataManager
            InitDataManager();
            //设置按钮点击功能
            InitBtnClickEvenet();
            //设置默认Player表
            OnPlayerBtnClick(null);

            Base.FpsStat.OnFps = OnFps;
        }
        //------------------------------------------------------
        private void Update()
        {
            FileSystemUtil.Update(Time.deltaTime, false);
            Base.FpsStat.getInstance().Update();
        }
        //------------------------------------------------------
        void InitFileSystem()
        {
            EFileSystemType type = EFileSystemType.AssetData;
#if !UNITY_EDITOR
            type = EFileSystemType.AssetBundle;
#endif
            VersionData.Parser(type,"version");

            FileSystemUtil.Init(type, "base_pkg", VersionData.version, VersionData.base_pack_cnt, VersionData.assetbundleEncryptKey);

        }
        //------------------------------------------------------
        void InitDataManager()
        {
            Data.DataManager.ReInit();
        }
        //------------------------------------------------------
        void InitBtnClickEvenet()
        {
            EventTriggerListener.Get(playerBtn).onClick = OnPlayerBtnClick;
            EventTriggerListener.Get(monsterBtn).onClick = OnMonsterBtnClick;
            EventTriggerListener.Get(sceneBtn).onClick = OnSceneBtnClick;
            EventTriggerListener.Get(clearPlayerBtn).onClick = OnClearPlayerBtnClick;
            EventTriggerListener.Get(clearMonsterBtn).onClick = OnClearMonsterBtnClick;
            EventTriggerListener.Get(clearSceneBtn).onClick = OnClearSceneBtnClick;
            EventTriggerListener.Get(toggleBtn).onClick = OnToggleBtnClick;
            EventTriggerListener.Get(clearModelsBtn).onClick = OnClearModelsBtnClick;
        }
        #region BtnClick
        //------------------------------------------------------
        void OnPlayerBtnClick(GameObject go, params VariablePoolAble[] param)
        {
            LoadPlayerList();
        }
        //------------------------------------------------------
        void OnMonsterBtnClick(GameObject go, params VariablePoolAble[] param)
        {
            LoadMonsterList();
        }
        //------------------------------------------------------
        void OnSceneBtnClick(GameObject go, params VariablePoolAble[] param)
        {
            LoadSceneList();
        }
        //------------------------------------------------------
        void OnClearPlayerBtnClick(GameObject go, params VariablePoolAble[] param)
        {
            ClearPlayerModels();
        }
        //------------------------------------------------------
        void OnClearMonsterBtnClick(GameObject go, params VariablePoolAble[] param)
        {
            ClearMonsterModels();
        }
        //------------------------------------------------------
        void OnClearSceneBtnClick(GameObject go, params VariablePoolAble[] param)
        {
            ClearSceneModels();
        }
        //------------------------------------------------------
        void OnToggleBtnClick(GameObject go, params VariablePoolAble[] param)
        {
            m_IsOpen = !m_IsOpen;
            if (m_IsOpen)
            {
                movementNode.DOAnchorPosX(100, 0.5f);
            }
            else
            {
                movementNode.DOAnchorPosX(-100, 0.5f);
            }
        }
        //------------------------------------------------------
        void OnClearModelsBtnClick(GameObject go, params VariablePoolAble[] param)
        {
            ClearModels();
        }
        #endregion
        //------------------------------------------------------
        void SpawnInstance(string strFile, SpawnType spawnType)
        {
            InstanceOperiaon pInstCB = FileSystemUtil.SpawnInstance(strFile,false);
            if (pInstCB != null)
            {
                //pInstCB.limitCheckCnt = 1;
                pInstCB.OnCallback = OnSpawnCallback;
                //pInstCB.OnSign = pUI.OnSign;
                pInstCB.userData0 = new Variable1() { intVal = (int)spawnType };
                pInstCB.Refresh();
            }
        }
        //------------------------------------------------------
        void OnSpawnCallback(InstanceOperiaon operiaon)
        {
            SpawnType modelType = (SpawnType)((Variable1)operiaon.userData0).intVal;

            switch (modelType)
            {
                case SpawnType.Player:
                    //设置父级
                    operiaon.pPoolAble.SetParent(SpawnPlayerRoot);
                    //设置位置
                    float posX = (m_SpawnModelIndex % SpawnModelRowCount) * SpawnModelSpaceX - SpawnModelSpaceX;
                    float posZ = Mathf.FloorToInt(m_SpawnModelIndex / SpawnModelRowCount) * SpawnModelSpaceZ;
                    Vector3 pos = new Vector3(posX,0, posZ);
                    operiaon.pPoolAble.SetPosition(pos);
                    operiaon.pPoolAble.SetScale(SpawnModelScale);

                    m_SpawnModelIndex++;
                    break;
                case SpawnType.Monster:
                    //设置父级
                    operiaon.pPoolAble.SetParent(SpawnMonsterRoot);
                    //设置位置
                    posX = (m_SpawnModelIndex % SpawnModelRowCount) * SpawnModelSpaceX - SpawnModelSpaceX;
                    posZ = Mathf.FloorToInt(m_SpawnModelIndex / SpawnModelRowCount) * SpawnModelSpaceZ;
                    pos = new Vector3(posX,0, posZ);
                    operiaon.pPoolAble.SetPosition(pos);
                    operiaon.pPoolAble.SetScale(SpawnModelScale);

                    m_SpawnModelIndex++;
                    break;
                case SpawnType.Scene:
                    //设置父级
                    operiaon.pPoolAble.SetParent(SpawnSceneRoot);
                    //设置位置
                    operiaon.pPoolAble.SetPosition(m_SceneSpawnNextPos);

                    break;
                default:
                    break;
            }


            //存储生成的资源,按照玩家,怪物,场景进行分类存储,方便后面清除
            if (!m_SpawnInstances.ContainsKey(modelType))
            {
                m_SpawnInstances.Add(modelType, new List<AInstanceAble>() { operiaon.pPoolAble });
            }
            else
            {
                m_SpawnInstances[modelType].Add(operiaon.pPoolAble);
            }
        }
        //------------------------------------------------------
        void LoadPlayerList()
        {
            m_ShowScrollDatas.Clear();

            var players = DataManager.getInstance().Player.datas;
            foreach (var player in players)
            {
                if (player.Value.nextId != 0)//过滤掉重复模型
                {
                    continue;
                }

                uint modelID = player.Value.nModelID;
                var textCfg = DataManager.getInstance().Text.GetData(player.Value.nameText);
                string name = "找不到名字";
                if (textCfg != null)
                {
                    name = textCfg.textCN;
                }
                m_ShowScrollDatas.Add(new ScrollListData(name, modelID, SpawnType.Player));
            }

            LoadScrollData();
        }
        //------------------------------------------------------
        void LoadMonsterList()
        {
            m_ShowScrollDatas.Clear();

            Dictionary<uint, CsvData_Monster.MonsterData> finalModels = new Dictionary<uint, CsvData_Monster.MonsterData>();
            var monsters = DataManager.getInstance().Monster.datas;
            foreach (var monster in monsters)
            {
                //过滤掉重复模型

                finalModels[monster.Value.modelId] = monster.Value;
            }

            foreach (var monster in finalModels)
            {
                uint modelID = monster.Value.modelId;
                var textCfg = DataManager.getInstance().Text.GetData(monster.Value.nameText);
                string name = "找不到名字";
                if (textCfg != null)
                {
                    name = textCfg.textCN;
                }
                m_ShowScrollDatas.Add(new ScrollListData(name, modelID, SpawnType.Monster));
            }           

            LoadScrollData();
        }
        //------------------------------------------------------
        void LoadSceneList()
        {
            m_ShowScrollDatas.Clear();

//             var scenes = DataManager.getInstance().RunScenes.datas;
//             foreach (var scene in scenes)
//             {
//                 //过滤掉重复模型?
// 
//                 uint modelID = scene.Value.nID;
// 
//                 string name = "找不到名字";
// #if UNITY_EDITOR
//                 name = scene.Value.strName;
// #else
//                 name = scene.Key.ToString();//由于场景名称只再编辑器下才有
// #endif
//                 m_ShowScrollDatas.Add(new ScrollListData(name, modelID, SpawnType.Scene));
//             }

            LoadScrollData();
        }
        //------------------------------------------------------
        void LoadScrollData()
        {
            HideCacheCells();
            foreach (var data in m_ShowScrollDatas)
            {
                GameObject cell = GetCell();
                if (cell == null)
                {
                    continue;
                }

                var ui = cell.GetComponent<UISerialized>();
                Text name = ui.GetWidget<Text>("Text");
                UIUtil.SetLabel(name, data.name);
                EventTriggerListener.Get(cell).onClick = (g,p) =>
                {
                    OnClickCell(data.modelID,data.modelType);
                };
            }
        }
        //------------------------------------------------------
        void OnClickCell(uint modelID,SpawnType modelType)
        {
            if (modelType == SpawnType.Player || modelType == SpawnType.Monster)
            {
                var modelData = DataManager.getInstance().Models.GetData(modelID);
                if (modelData == null)
                {
                    return;
                }

                //可以同个模型多次点击?
                SpawnInstance(modelData.strFile, modelType);
            }
            else
            {
//                 var sceneCfg = DataManager.getInstance().RunScenes.GetData(modelID);
//                 if (sceneCfg != null)
//                 {
//                     SpawnInstance(sceneCfg.strFile, modelType);
//                     m_SceneSpawnNextPos += sceneCfg.nextLinkPos;
//                 }
            }
        }
        //------------------------------------------------------
        GameObject GetCell()
        {
            foreach (var item in m_CellCache)
            {
                if (item.activeInHierarchy == false)
                {
                    item.SetActive(true);
                    return item;
                }
            }

            if (ScrollCell == null)
            {
                return null;
            }

            GameObject cell = Instantiate(ScrollCell, ScrollCellParent);
            cell.SetActive(true);
            m_CellCache.Add(cell);
            return cell;
        }
        //------------------------------------------------------
        void HideCacheCells()
        {
            foreach (var item in m_CellCache)
            {
                item.SetActive(false);
            }
        }
        //------------------------------------------------------
        //------------------------------------------------------
        void ClearModels()
        {
            ClearPlayerModels();
            ClearMonsterModels();
        }
        void ClearPlayerModels()
        {
            if (m_SpawnInstances.ContainsKey(SpawnType.Player) == false)
            {
                return;
            }
            var players = m_SpawnInstances[SpawnType.Player];

            m_SpawnModelIndex -= players.Count;

            foreach (var player in players)
            {
                FileSystemUtil.DeSpawnInstance(player);
            }

            m_SpawnInstances[SpawnType.Player].Clear();
        }
        //------------------------------------------------------
        void ClearMonsterModels()
        {
            if (m_SpawnInstances.ContainsKey(SpawnType.Monster) == false)
            {
                return;
            }
            var monsters = m_SpawnInstances[SpawnType.Monster];

            m_SpawnModelIndex -= monsters.Count;

            foreach (var monster in monsters)
            {
                FileSystemUtil.DeSpawnInstance(monster);
            }

            m_SpawnInstances[SpawnType.Monster].Clear();
        }
        //------------------------------------------------------
        void ClearSceneModels()
        {
            m_SceneSpawnNextPos = Vector3.zero;
            if (m_SpawnInstances.ContainsKey(SpawnType.Scene) == false)
            {
                return;
            }
            var scenes = m_SpawnInstances[SpawnType.Scene];
            foreach (var scene in scenes)
            {
                FileSystemUtil.DeSpawnInstance(scene);
            }
            m_SpawnInstances[SpawnType.Scene].Clear();
        }
        //------------------------------------------------------
        void OnFps(float fps)
        {
            if (m_pFps == null) return;
            int fps_temp = Mathf.CeilToInt(fps);
            if (m_nFps != fps_temp)
            {
                m_nFps = fps_temp;
                UIUtil.SetLabel(m_pFps, m_nFps.ToString());
            }
        }
    }


    public struct ScrollListData
    {
        public string name;
        public uint modelID;
        public SpawnType modelType;

        public ScrollListData(string name, uint modelID, SpawnType spawnType)
        {
            this.name = name;
            this.modelID = modelID;
            modelType = spawnType;
        }
    }

    public enum SpawnType
    {
        Player,
        Monster,
        Scene
    }
}

