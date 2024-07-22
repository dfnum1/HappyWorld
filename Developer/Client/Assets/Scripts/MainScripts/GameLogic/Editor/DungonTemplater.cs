//#if UNITY_EDITOR
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TopGame.Data;
//using TopGame.Core;
//using UnityEditor;
//using TopGame.Logic;
//using Framework.Core;
//using Framework.Base;
//using Framework.Data;
//using Framework.Logic;

//namespace TopGame.ED
//{
//    [ExecuteInEditMode, Framework.Plugin.PluginDisiableExport]
//    public class DungonThemeRoot : MonoBehaviour
//    {
//        public Bounds runtime_bounds;
//        GridWireBounds m_pWire = new GridWireBounds();
//        private void Awake()
//        {
//            EditorHelp.BuildBounds(transform, ref runtime_bounds);
//            m_pWire.Init(Color.yellow);
//            Selection.activeTransform = this.transform;

//            for (int i = 0; i < transform.childCount; ++i)
//            {
//                HideUnTemplater(transform.GetChild(i), true);
//            }
//        }
//        private void Update()
//        {
//            if(Selection.activeTransform == this.transform)
//            {
//                m_pWire.AddBox(this.transform, runtime_bounds.size, new Vector3(0,0, runtime_bounds.size.z)*0.5f);
//                m_pWire.Render();
//            }
//        }
//        //------------------------------------------------------
//        public static void HideUnTemplater(Transform pTrans, bool bHide)
//        {
//            if (pTrans.GetComponent<ED.DungonTemplater>() == null)
//                pTrans.gameObject.SetActive(!bHide);
//            for(int i = 0; i < pTrans.childCount; ++i)
//            {
//                HideUnTemplater(pTrans.GetChild(i), bHide);
//            }
//        }
//        //------------------------------------------------------
//        private void OnDestroy()
//        {
//            m_pWire.Destroy();
////             ThemeRunerScene themeScene = AState.CastCurrentModeLogic<ThemeRunerScene>();
////             if (themeScene != null) themeScene.RemoveTheme(GetInstanceID());
//        }
//        //------------------------------------------------------
////         public void BuildRuntime(ThemeRunerScene scene)
////         {
////             if (scene == null) return;
////             scene.RemoveTheme(this.gameObject.GetInstanceID());
////             ED.DungonTemplater[] templaters = this.GetComponentsInChildren<ED.DungonTemplater>(true);
////             for (int i = 0; i < templaters.Length; ++i)
////             {
////                 ED.DungonTemplater temp = templaters[i] as ED.DungonTemplater;
////                 if (temp.type == Logic.ESceneEleType.BattleObject)
////                 {
////                     if (temp.BestConfigID == 0xffffffff)
////                     {
////                         temp.BestConfigID = temp.GetBestConfigID();
////                     }
////                     ObsElement element = scene.AddObsObject(gameObject.GetInstanceID(), temp.gameObject, temp.BestConfigID, Vector3.zero);
////                  //   temp.pSceneRuntimeEle = element;
////                     if (element != null)
////                     {
////                         temp.gameObject.SetActive(true);
////                     }
////                 }
////                 else if (temp.type == Logic.ESceneEleType.Monster)
////                 {
////                     temp.gameObject.SetActive(true);
//// //                     ObsMonster element = scene.AddMonster(gameObject.GetInstanceID(), temp.gameObject, temp.BestConfigID, Vector3.zero);
//// //                     temp.pSceneRuntimeEle = element;
////                 }
////             }
////         }
//        //------------------------------------------------------
//        [MenuItem("Assets/关卡元素模板导出")]
//        public static void ExportElementThemes()
//        {
//            if (Selection.gameObjects == null || Selection.gameObjects.Length<=0) return;
//            string saveFile = EditorUtility.SaveFolderPanel("保存模版", Application.dataPath + "/DatasRef/Config/RunDatas/", "EleThemes");
//            saveFile = saveFile.Replace("\\", "/");
//            if (!saveFile.EndsWith("/")) saveFile += "/";
//            EditorUtility.DisplayProgressBar("关卡元素模板导出", "", 0);
//            for(int i =0; i < Selection.gameObjects.Length; ++i)
//            {
//                EditorUtility.DisplayProgressBar("关卡元素模板导出", "", (float)i/ (float)Selection.gameObjects.Length);
//                if (!ExportDungonThemeInf(Selection.gameObjects[i], saveFile))
//                {
//                    break;
//                }
//            }
//            EditorUtility.ClearProgressBar();
//        }
//        //------------------------------------------------------
//        static bool ExportDungonThemeInf(GameObject prefab, string strDir)
//        {
//            Bounds bounds = new Bounds();
//            if (!EditorHelp.BuildBounds(prefab.transform, ref bounds))
//            {
//                EditorUtility.DisplayDialog("提示", "包围盒大小无效", "请检测");
//                return false;
//            }

//            List<BrushElementTheme.Element> vEle = new List<BrushElementTheme.Element>();
//            ED.DungonTemplater[] templaters = prefab.GetComponentsInChildren<ED.DungonTemplater>(true);
//            for (int i = 0; i < templaters.Length; ++i)
//            {
//                ED.DungonTemplater temp = templaters[i] as ED.DungonTemplater;
//                BrushElementTheme.Element ele = new BrushElementTheme.Element();
//                ele.position = temp.transform.position;
//                ele.eulerAngle = temp.transform.eulerAngles;
//                ele.scale = temp.transform.localScale;
//                ele.activeRadius = temp.activeRadius;
//                ele.Limits = new List<BrushElementTheme.Element.Limit>();
//                if (temp.Limits != null)
//                {
//                    for (int j = 0; j < temp.Limits.Count; ++j)
//                    {
//                        BrushElementTheme.Element.Limit limit = new BrushElementTheme.Element.Limit();
//                        if (temp.Limits[j].bRange)
//                        {
//                            limit.value1 = Mathf.Min(temp.Limits[j].value0, temp.Limits[j].value1);
//                            limit.value2 = Mathf.Max(temp.Limits[j].value0, temp.Limits[j].value1);
//                        }
//                        else
//                        {
//                            limit.value1 = temp.Limits[j].value0;
//                            limit.value2 = limit.value1;
//                        }
//                        ele.Limits.Add(limit);
//                    }
//                }
//                ele.sceneEleType = temp.type;
//                ele.objType = temp.objType;
//                ele.moveType = temp.moveType;
//                ele.fSpeedScale = temp.fSpeedScale;
//                ele.targetOffestPos = temp.targetOffestPos;
//                ele.userData = temp.userData;
//                ele.rodeRandom = temp.rodeRandom;
//                ele.bUseGravity = temp.bUseGravity;
//                ele.terrainHeight = temp.terrainHeight;
//                ele.spawnData = temp.spawnData;

//                if (ED.EditorHelp.BuildBounds(temp.transform, ref ele.bounds))
//                {
//                    vEle.Add(ele);
//                }
//            }
//            if (vEle.Count <= 0)
//            {
//                EditorUtility.DisplayDialog("提示", "不存在魔板数据", "请换一个试试");
//                return false;
//            }
//            string saveFile = strDir + prefab.name + ".json";
//            saveFile = saveFile.Replace("\\", "/");
//            if (!saveFile.Contains("DatasRef/Config/RunDatas/EleThemes/"))
//            {
//                EditorUtility.DisplayDialog("提示", "目录必须是在\"DatasRef/Config/RunDatas EleThemes\"下", "好的");
//                return false;
//            }
//            BrushElementTheme theme = new BrushElementTheme();
//            theme.Elements = vEle.ToArray();
//            theme.name = System.IO.Path.GetFileNameWithoutExtension(saveFile);
//            theme.size = bounds.size;
//            theme.SaveToBinaryFile(saveFile);
//            return true;
//        }
//    }
//    //------------------------------------------------------
//    [CustomEditor(typeof(DungonThemeRoot))]
//    [CanEditMultipleObjects]
//    public class DungonThemeRootEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            DungonThemeRoot root = target as DungonThemeRoot;
//            base.OnInspectorGUI();
////             Logic.ThemeRunerScene scene = Logic.AState.CastCurrentModeLogic<Logic.ThemeRunerScene>();
////             if(scene!=null)
////             {
////                 if (GUILayout.Button("从这里开始"))
////                 {
////                     if (scene != null) scene.ResetRunPosition(root.transform.position-Vector3.forward*20);
////                 }
////             }
//            if(GUILayout.Button("保存成预制体"))
//            {
//                string save_path = EditorUtility.SaveFilePanel("模板保存", Application.dataPath + "/EditorDatas/DungonTemplate/", "new_theme", "prefab");
//                save_path = save_path.Replace("\\", "/").Replace(Application.dataPath, "/Assets");
//                if(!save_path.Contains("/EditorDatas/DungonTemplate"))
//                {
//                    EditorUtility.DisplayDialog("提示", "模板保存需要在Assets/EditorDatas/DungonTemplate 子目录下", "我去，还有这种限制");
//                    return;
//                }

//                GameObject newPrefab = GameObject.Instantiate(root.gameObject);
//                DungonThemeRoot.HideUnTemplater(newPrefab.transform, false);
//                Base.Util.ResetGameObject(newPrefab, Base.EResetType.All);
//                newPrefab.name = System.IO.Path.GetFileNameWithoutExtension(save_path);
//                bool bSuccess = false;
//                PrefabUtility.SaveAsPrefabAsset(newPrefab, save_path, out bSuccess);
//                Base.Util.Desytroy(newPrefab);
//                if(!bSuccess)
//                {
//                    EditorUtility.DisplayDialog("提示", "我擦，保存失败", "我去");
//                }
//            }
//            if (GUILayout.Button("刷新"))
//            {
//                EditorHelp.BuildBounds(root.transform, ref root.runtime_bounds);

//              //  root.BuildRuntime(scene);
//            }
//        }
//        private void OnSceneGUI()
//        {
//            DungonThemeRoot root = target as DungonThemeRoot;
//            Matrix4x4 mt = Matrix4x4.TRS(root.transform.position, root.transform.rotation, Vector3.one);
//            Framework.Core.CommonUtility.DrawBoundingBox(root.runtime_bounds.center, root.runtime_bounds.size, mt, new Color(1, 0, 0, 0.8f), false);
////             ThemeRunerScene themeScene = AState.CastCurrentModeLogic<ThemeRunerScene>();
////             if(themeScene!=null)
////             {
////                 if (Tools.current == Tool.Move)
////                 {
////                     Vector3 pos = Handles.DoPositionHandle(root.transform.position, root.transform.rotation);
////                     themeScene.AutoAdsorb(ref pos);
////                     root.transform.position = pos;
////                 }
////             }
//        }
//    }

//    [ExecuteInEditMode, Framework.Plugin.PluginDisiableExport]
//    public class DungonEleRuntimeAble  : AInstanceAble
//    {
//        [System.NonSerialized]
//     //   public ASceneElement pSceneRuntimeEle;

//        public DungonTemplater templater;

//        protected override void OnDestroy()
//        {
//            base.OnDestroy();
//       //     if (pSceneRuntimeEle != null) pSceneRuntimeEle.Destroy();
//        }
//        //------------------------------------------------------
//        public override bool CanRecyle()
//        {
//            return false;
//        }
//        //------------------------------------------------------
//        public override void OnRecyle()
//        {
//            base.OnRecyle();
//            Base.Util.Desytroy(this.gameObject);
//        }
//        //------------------------------------------------------
//        private void LateUpdate()
//        {
////             if( (pSceneRuntimeEle!=null && (pSceneRuntimeEle.IsFlag(EWorldNodeFlag.Killed) || pSceneRuntimeEle.IsDestroy())))
////             {
////                 Base.Util.Desytroy(this.gameObject);
////                 pSceneRuntimeEle = null;
////             }
//        }
//    }

//    [ExecuteInEditMode, Framework.Plugin.PluginDisiableExport]
//    public class DungonTemplater : AInstanceAble
//    {
//        [System.NonSerialized]
//        public bool needConvertCube = false;
//      //  [System.NonSerialized]
//      //  public ASceneElement pSceneRuntimeEle;
//        [System.NonSerialized]
//        public uint BestConfigID = 0;
//        [System.Serializable]
//        public class Limit
//        {
//            public bool bRange = false;
//            public int value0 = 0;
//            public int value1 = 0;
//        }

//        [Framework.Data.DisplayNameGUI("类型")]
//        public BrushElementTheme.ESceneEleType type = BrushElementTheme.ESceneEleType.BattleObject;

//        [Framework.Data.DisplayNameGUI("激活半径(<=0为立马激活)")]
//        public float activeRadius = 0;

//        [Framework.Plugin.DisableGUI]
//        public bool bLimitID = false;
//        [Framework.Plugin.DisableGUI]
//        public List<Limit> Limits = new List<Limit>();

//        [Framework.Plugin.DisableGUI]
//        public EObstacleType objType = EObstacleType.None;

//        [Framework.Plugin.DisableGUI]
//        public EMoveType moveType = EMoveType.None;

//        [Framework.Plugin.DisableGUI]
//        public float fSpeedScale = 1;

//        [Framework.Plugin.DisableGUI]
//        public Vector3 targetOffestPos = Vector3.zero;

//        [Framework.Plugin.DisableGUI]
//        public Vector4 userData = Vector4.zero;

//        [Framework.Plugin.DisableGUI]
//        public Vector3Int rodeRandom = Vector3Int.zero;

//        [Framework.Plugin.DisableGUI]
//        public bool bUseGravity = true;

//        [Framework.Plugin.DisableGUI]
//        public ETerrainHeightType terrainHeight = ETerrainHeightType.None;

//        [Framework.Plugin.DisableGUI]
//        public SpawnSplineData spawnData;


//        private Vector3 m_backupPos = Vector3.zero;
//        private Vector3 m_backupRot = Vector3.zero;
//        private Vector3 m_backupScale = Vector3.zero;
//        //------------------------------------------------------
//        public override bool CanRecyle()
//        {
//            return false;
//        }
//        protected override void OnDestroy()
//        {
//        //    if (pSceneRuntimeEle != null) pSceneRuntimeEle.Destroy();
//        }
//        //------------------------------------------------------
//        protected override void Awake()
//        {
//            base.Awake();
//        }
//        //------------------------------------------------------
//        public void Update()
//        {

//        }
//        //------------------------------------------------------
//        public static int CompreArea(CsvData_BattleObject.BattleObjectData oth1, CsvData_BattleObject.BattleObjectData oth2)
//        {
//            Vector3 othSize1 = Framework.Core.CommonUtility.AbsVector3(oth1.aabb_max - oth1.aabb_min);
//            Vector3 othSize2 = Framework.Core.CommonUtility.AbsVector3(oth2.aabb_max - oth2.aabb_min);
//            float area1 = othSize1.x * othSize1.y * othSize1.z;
//            float area2 = othSize2.x * othSize2.y * othSize2.z;

//            return (area1 - area2) > 0 ? -1 : 1;
//        }
//        //------------------------------------------------------
//        public uint GetBestConfigID()
//        {
//            uint nConfigId = 0xffffffff;
//            if (type == BrushElementTheme.ESceneEleType.BattleObject)
//            {
//                Bounds bounds = new Bounds();
//                EditorHelp.BuildBounds(this.transform, ref bounds);
//                bounds.center = Vector3.zero;
//                Dictionary<EObstacleType, List<CsvData_BattleObject.BattleObjectData>> vAllTypes = new Dictionary<EObstacleType, List<CsvData_BattleObject.BattleObjectData>>();
//                foreach (var db in DataManager.getInstance().BattleObject.datas)
//                {
//                    List<CsvData_BattleObject.BattleObjectData> vEle = null;
//                    if (!vAllTypes.TryGetValue(db.Value.objectType, out vEle))
//                    {
//                        vEle = new List<CsvData_BattleObject.BattleObjectData>();
//                        vAllTypes.Add(db.Value.objectType, vEle);
//                    }
//                    vEle.Add(db.Value);
//                }
//                foreach (var db in vAllTypes)
//                {
//                    db.Value.Sort(CompreArea);
//                }
//                List<CsvData_BattleObject.BattleObjectData> vObjs;
//                if (!vAllTypes.TryGetValue(objType, out vObjs))
//                {
//                    return nConfigId;
//                }
//                if (vObjs == null || vObjs.Count <= 0) return nConfigId;

//                if (bLimitID)
//                {
//                    for (int j = 0; j < Limits.Count; ++j)
//                    {
//                        if (Limits[j].bRange)
//                        {
//                            for (int k = Limits[j].value0; k < Limits[j].value1; ++k)
//                            {
//                                if (DataManager.getInstance().BattleObject.GetData((uint)k) != null)
//                                {
//                                    return (uint)k;
//                                }
//                            }
//                        }
//                        else
//                        {
//                            if (DataManager.getInstance().BattleObject.GetData((uint)Limits[j].value0) != null)
//                            {
//                                return (uint)Limits[j].value0;
//                            }
//                        }
//                    }
//                }

//                Bounds obj_bounds = new Bounds();
//                for (int j = 0; j < vObjs.Count; ++j)
//                {
//                    obj_bounds.SetMinMax(vObjs[j].aabb_min, vObjs[j].aabb_max);
//                    obj_bounds.center = bounds.center;
//                    if (EditorHelp.ContainBounds(bounds, obj_bounds))
//                    {
//                        nConfigId = vObjs[j].id;
//                        return nConfigId;
//                    }
//                }
//            }
//            else
//            {
//                if (bLimitID)
//                {
//                    for (int j = 0; j < Limits.Count; ++j)
//                    {
//                        if (Limits[j].bRange)
//                        {
//                            for (int k = Limits[j].value0; k < Limits[j].value1; ++k)
//                            {
//                                if (DataManager.getInstance().Monster.GetData((uint)k) != null)
//                                {
//                                    return (uint)k;
//                                }
//                            }
//                        }
//                    }
//                }
//                foreach (var db in EditorHelp.MonsterTemplaterCsv.datas)
//                {
//                    uint[] vals = null;
//                    if (db.Value.groups.TryGetValue(moveType, out vals) && vals != null && vals.Length > 0)
//                    {
//                        return vals[0];
//                    }
//                }

//            }
//            return 0xffffffff;
//        }
//    }
//}
//#endif