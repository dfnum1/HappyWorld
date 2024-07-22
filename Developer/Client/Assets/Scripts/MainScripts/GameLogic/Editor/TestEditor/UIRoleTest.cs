using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TopGame.Core;
using Framework.Core;
using Framework.Module;
using Framework;

namespace TopGame
{
    public class RoleTestFamework : AFrameworkModule
    {
        protected override bool OnAwake(Framework.IFrameworkCore plusSetting)
        {
            base.OnAwake(plusSetting);

            if (m_pDataMgr == null)
                m_pDataMgr = RegisterModule<Data.DataManager>();

            m_pFileSystem = RegisterModule(Logic.FrameworkStartUp.GetFileSystem());
            return true;
        }
        //------------------------------------------------------
        public override BufferState OnCreateBuffer(Actor pActor, AWorldNode trigger, BufferState buff, uint dwBufferID, uint nLevel, IContextData pData, IBuffParam param = null)
        {
            return null;
        }
        //------------------------------------------------------
        public override void OnLaunchProjectile(AProjectile pProjectile)
        {
        }
        //------------------------------------------------------
        public override void OnProjectileHit(AProjectile pProjectile, ActorAttackData attackData, bool bHitScene, bool bExplode)
        {
        }
        //------------------------------------------------------
        public override void OnProjectileUpdate(AProjectile pProjectile)
        {
        }
    }

    public class UIRoleTest : MonoBehaviour
    {
        public FrameworkMainEditor frameworkMain;
        [System.Serializable]
        public struct RoleData
        {
            public int type;
            public GameObject btn;
            public GameObject playList;
            public GameObject dropDownList;
            public RoleTest roleMgr;

            public RoleData(int type)
            {
                this.type = type;
                this.btn = null;
                this.playList = null;
                this.dropDownList = null;
                this.roleMgr = null;
            }
        }

        public GameObject[] Cameras;
        [System.NonSerialized]
        public int curCamIdx = 0;
        [System.NonSerialized]
        public Tweener curTweener;

        private bool bToLeft = true;
        public bool bRotate = false;

        public RoleData[] roleDatas;

        public RoleTest roleDropMenu;
        public RoleTest monsterDropMenu;
        public RoleTest bossDropMenu;
        public RoleTest sceneDropMenu;

        [System.NonSerialized]
        public int curSelectPanel = 0; //0 角色 1 boss 2 小怪

        public struct AssetFile
        {
            public string name;
            public string file;
            public string height_file;
        }
        Dictionary<string, List<AssetFile>> m_vPlayers = new Dictionary<string, List<AssetFile>>();

        Data.CsvData_Player m_RoleCsv = null;
        Data.CsvData_Monster m_MonsterCsv = null;

        RoleTestFamework m_pFramework = null;
        void Start()
        {
            if (GameInstance.getInstance()!=null)
            {
                if(m_RoleCsv == null) m_RoleCsv = Data.DataManager.getInstance().Player;
                if (m_MonsterCsv == null) m_MonsterCsv = Data.DataManager.getInstance().Monster;
            }
            else
            {
                m_pFramework = ModuleManager.getInstance().RegisterModule<RoleTestFamework>();
                ModuleManager.mainModule = m_pFramework;

                ModuleManager.getInstance().Awake(frameworkMain);
                ModuleManager.getInstance().StartUp(frameworkMain);
                Logic.FrameworkStartUp.getInstance().SetSection(Logic.EStartUpSection.AppStartUp);

#if UNITY_EDITOR
                m_RoleCsv = Framework.Data.DataEditorUtil.GetTable<Data.CsvData_Player>(true);
                if (m_RoleCsv != null) Framework.Data.DataEditorUtil.MappingTable(m_RoleCsv);

                m_MonsterCsv = Framework.Data.DataEditorUtil.GetTable<Data.CsvData_Monster>(true);
                if (m_MonsterCsv != null) Framework.Data.DataEditorUtil.MappingTable(m_MonsterCsv);
#endif
            }

            m_vPlayers.Clear();
            if (m_RoleCsv!=null)
            {
                HashSet<string> vSets = new HashSet<string>();
                List<AssetFile> assetFiles = new List<AssetFile>();
                m_vPlayers.Add("英雄", assetFiles);
                foreach (var db in m_RoleCsv.datas)
                {
                    AssetFile asset = new AssetFile();
                    if (db.Value.Models_nModelID_data != null)
                    {
                        if(vSets.Contains(db.Value.Models_nModelID_data.strFile))
                            continue;
                        vSets.Add(db.Value.Models_nModelID_data.strFile);
                        asset.file = db.Value.Models_nModelID_data.strFile;
                    }
                    if (db.Value.Models_hModelID_data != null)
                        asset.height_file = db.Value.Models_hModelID_data.strFile;
                    if (db.Value.Text_nameText_data != null)
                        asset.name = db.Value.strName;
                    else asset.name = db.Key.ToString();
                    assetFiles.Add(asset);
                }
                roleDropMenu.AddDropItem(assetFiles);
            }
            if (m_MonsterCsv != null)
            {
                List<AssetFile> monsterFiles = new List<AssetFile>();
                m_vPlayers.Add("小怪", monsterFiles);
                List<AssetFile> bossFiles = new List<AssetFile>();
                m_vPlayers.Add("BOSS", bossFiles);
                foreach (var db in m_MonsterCsv.datas)
                {
                    AssetFile asset = new AssetFile();
                    if (db.Value.Models_modelId_data != null)
                        asset.file = db.Value.Models_modelId_data.strFile;
                    if (db.Value.Text_nameText_data != null)
                        asset.name = db.Value.Text_nameText_data.textCN;
                    else asset.name = db.Key.ToString();
                    if (db.Value.monsterType == Framework.Base.EMonsterType.Boss)
                        bossFiles.Add(asset);
                    else monsterFiles.Add(asset);
                }
                monsterDropMenu.AddDropItem(monsterFiles);
                bossDropMenu.AddDropItem(bossFiles);
            }
//             if (m_SceneCsv != null)
//             {
//                 List<AssetFile> sceneFiles = new List<AssetFile>();
//                 m_vPlayers.Add("场景", sceneFiles);
//                 foreach (var db in m_SceneCsv.datas)
//                 {
//                     AssetFile asset = new AssetFile();
//                     asset.file = db.Value.strFile;
//                     asset.name = System.IO.Path.GetFileNameWithoutExtension(asset.file);
// #if UNITY_EDITOR
//                     if (db.Value.strName != null)
//                         asset.name = db.Value.strName;
// #endif   
//                     sceneFiles.Add(asset);
//                 }
//                 sceneDropMenu.AddDropItem(sceneFiles);
//             }

            //初始化全特效
            OnCameraClick(0);

            //设置角色组
            for (int i = 0; i < roleDatas.Length; i++)
            {
                int idx = i;
                if (roleDatas[idx].btn && roleDatas[idx].playList)
                {
                    roleDatas[idx].btn.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        SetModeActiveByType(roleDatas[idx]);
                    });
                }
            }
        }

        /// <summary>
        /// 获取到对应界面的RoleData
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public RoleData GetRoleDataByType(int type)
        {
            for (int i = 0; i < roleDatas.Length; i++)
            {
                if (roleDatas[i].type == type)
                {
                    return roleDatas[i];
                }
            }
            return new RoleData(-1);
        }
        /// <summary>
        /// 摄像机按钮点击
        /// </summary>
        /// <param name="idx"></param>
        public void OnCameraClick(int idx)
        {
            for (int i = 0; i < Cameras.Length; ++i)
            {
                if (Cameras[i]) Cameras[i].SetActive(i == idx);
            }
            curCamIdx = idx;
            if(idx == 0)
            {
                //! 全视角
                for (int i = 0; i < roleDatas.Length; i++)
                {
                    roleDatas[i].roleMgr?.SetHighLowModel(1);
                }
                return;
            }

            if (idx == 3 || idx == 2)
            {
                RoleData roleData = GetRoleDataByType(0);
                if (roleData.type != -1)
                {
                    roleData.roleMgr?.SetHighLowModel(idx);
                }
            }
        }

        /// <summary>
        /// 设置侧边栏滚动
        /// </summary>
        public void SetSliderBarTween(RectTransform sliderBar)
        {
            if (curTweener != null)
            {
                DOTween.Kill(curTweener);
            }
            curTweener = sliderBar.DOLocalMove(new Vector3(bToLeft ? -800 : -500, 0, 0), 1);
            curTweener.SetAutoKill();
            bToLeft = !bToLeft;
        }

        //设置对应模型隐藏
        public void SetModeActiveByType(RoleData roleData) // 0 role 1 boss 2 monster
        {
            if (!roleData.btn || !roleData.playList) return;

            bool bActive = !roleData.playList.activeSelf;
            roleData.btn.transform.Find("Image")?.gameObject.SetActive(bActive);
            roleData.playList.SetActive(bActive);
        }

        // Update is called once per frame
        void Update()
        {
            if (GameInstance.getInstance() == null)
                ModuleManager.getInstance().Update(Time.deltaTime);
        }
        void LateUpdate()
        {
            if (GameInstance.getInstance() == null)
                ModuleManager.getInstance().LateUpdate(Time.deltaTime);
        }
        void FixedUpdate()
        {
            if (GameInstance.getInstance() == null)
                ModuleManager.getInstance().FixedUpdate(Time.deltaTime);
        }
    }
}