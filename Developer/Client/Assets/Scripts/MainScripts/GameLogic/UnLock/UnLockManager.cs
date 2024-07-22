// /********************************************************************
// 生成日期:	2020-6-16
// 类    名: 	UnLockManager
// 作    者:	zdq
// 描    述:	控制解锁功能的判断和解锁过渡状态的判断,管理所有挂载在功能按钮上的mono
// *********************************************************************/
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TopGame.Data;
// using TopGame.SvrData;
// using Proto3;
// using System;
// using TopGame.UI;
// using Framework.Core;
// using Framework.Data;
// using TopGame.Base;
// 
// namespace TopGame.Logic
// {
//     /// <summary>
//     /// 锁定表现状态
//     /// </summary>
//     public enum LockView
//     {
//         None,
//         Gray,
//         Hide
//     }
//     enum UnLockModuleType
//     {
//         None,
//         /// <summary>
//         /// 主线关卡
//         /// </summary>
//         DefaultLevel,
//         /// <summary>
//         /// 跑酷距离
//         /// </summary>
//         InfiniteParkourDistance,
//         /// <summary>
//         /// 建筑等级
//         /// </summary>
//         BuildingLevel,
//         /// <summary>
//         /// 章节进度
//         /// </summary>
//         ChapterID,
//     }
// 
//     public enum EUnlockID
//     {
//         None = 0,
//         Fabrication4030 = 4030,
//         Count,
//     }
// 
//     [Framework.Plugin.AT.ATExportMono("模块锁系统", "TopGame.GameInstance.getInstance().unlockMgr"), Framework.Plugin.AT.ATExportGUID(1573028666)]
//     public class UnLockManager : AUnLockManager, Framework.Module.IStartUp,Framework.Module.IUpdate
//     {
//         struct Unlock
//         {
//             public CsvData_Unlock.UnlockData unLockData;
//             public List<UnLockListener> listeners;
//             public void OnClear()
//             {
//                 if (listeners == null) return;
//                 for (int i = 0; i < listeners.Count; ++i)
//                 {
//                     if(listeners[i]) listeners[i].OnClear();
//                 }
//             }
//             public void OnLock()
//             {
//                 if (listeners == null) return;
//                 for (int i = 0; i < listeners.Count; ++i)
//                 {
//                     if (listeners[i])
//                     {
//                         listeners[i].OnLock();
//                     }
//                 } 
//             }
// 
//             public bool IsContain(GameObject pGO)
//             {
//                 if (listeners == null) return false;
//                 for (int i = 0; i < listeners.Count; ++i)
//                 {
//                     if (listeners[i] && listeners[i].IsContain(pGO)) return true;
//                 }
//                 return false;
//             }
// 
//             public bool OnUnlock()
//             {
//                 if (listeners == null) return true;
//                 for (int i = 0; i < listeners.Count; ++i)
//                 {
//                     if (listeners[i])
//                     {
//                         if (!listeners[i].OnUnlock())
//                             return false;
//                     }
//                 }
//                 return true;
//             }
//             public bool IsValid()
//             {
//                 return unLockData != null && listeners!=null && listeners.Count > 0;
//             }
//         }
//         Dictionary<uint, Unlock> m_vAllListener = new Dictionary<uint, Unlock>();
//         Dictionary<uint, Unlock> m_LockInfos = new Dictionary<uint, Unlock>();
// 
//         float m_fDirtyLock = 0;
//         private Dictionary<uint, byte> m_vUnLockeds = new Dictionary<uint, byte>(32);
//         /// <summary>
//         /// 当游戏刚运行初始化的时候
//         /// </summary>
//         protected override void Awake()
//         {
//             
//         }
//         //------------------------------------------------------
//         /// <summary>
//         /// 当配置初始化完的时候
//         /// </summary>
//         public void StartUp()
//         {
//             BattleDB battleDb = TopGame.SvrData.UserManager.getInstance().mySelf.ProxyDB<BattleDB>(Data.EDBType.Battle);
//             battleDb.OnChapterDataChange += OnChapterDataChange;
// 
//             BaseDB baseDB = UserManager.getInstance().mySelf.ProxyDB<BaseDB>(EDBType.BaseInfo);
//             baseDB.OnInfiniteDistanceChange += OnInfiniteDistanceChange;
// 
//             BuildingDB buildingDb = UserManager.MySelf.ProxyDB<BuildingDB>(EDBType.Building);
//             buildingDb.OnDirtyBuildingStateChange += OnBuildingChunkStateChange;
// 
//             ChapterTaskDB.OnChapterChange += OnChapterChange;
// 
//             m_bEnable = Core.DebugConfig.bEnableModuleLock;
//         }
//         //------------------------------------------------------
//         void LoadUnLocked()
//         {
//             m_fDirtyLock = 0;
//             m_vUnLockeds.Clear();
//             string strFile = Framework.Core.CommonUtility.stringBuilder.Append(FileSystemUtil.PersistenDataPath).Append("Unlock_").Append(UserManager.MySelf.userID).ToString();
//             Log("开始加载本地缓存解锁数据!");
//             if (System.IO.File.Exists(strFile))
//             {
//                 BinaryUtil reader = new BinaryUtil();
//                 byte[] binarys = System.IO.File.ReadAllBytes(strFile);
//                 if (binarys != null && binarys.Length > 0)
//                 {
//                     reader.Set(binarys, binarys.Length);
//                     int cnt = (int)reader.ToUshort();
//                     for (int i = 0; i < cnt; ++i)
//                     {
//                         uint id = reader.ToUint32();
//                         byte state = reader.ToByte();
//                         if (state <= 0)//容错处理,存档中可能出现状态不对的情况
//                         {
//                             state = (byte)EModuleLockState.Unlock;
//                         }
//                         m_vUnLockeds[id] = state;//:模块锁在从解锁的账号切换到未解锁的账号时,由于界面使用了缓存,并且被之前的账号隐藏掉UI,导致无法出现按钮,在加载解锁列表的时候也刷新
//                         Log(Util.stringBuilder.Append("id:").Append(id).Append("state:").Append((EModuleLockState)state).ToString());
//                         //这边加载完后,需要手动刷新一次状态(主要针对切换账号的情况,第一次进入游戏,这个列表中应该没有数据)
//                         if (m_vAllListener.ContainsKey(id))
//                         {
//                             Log(Util.stringBuilder.Append("刷新缓存listenner id:").Append(id).ToString());
//                             var item = m_vAllListener[id];
//                             if (item.listeners != null)
//                             {
//                                 for (int j = 0; j < item.listeners.Count; j++)
//                                 {
//                                     item.listeners[j].moduleLockState = EModuleLockState.Unlock;
//                                     RefreshState(item.listeners[j]);
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//             Log(Util.stringBuilder.Append("读取本地解锁文件,个数:").Append(m_vUnLockeds.Count).ToString());
//         }
//         //------------------------------------------------------
//         public void SaveUnLocked()
//         {
//             m_fDirtyLock = 0;
//             if (UserManager.Current != UserManager.MySelf)
//                 return;
//             if (m_vUnLockeds.Count <= 0) return;
//             if (!System.IO.Directory.Exists(FileSystemUtil.PersistenDataPath))
//                 System.IO.Directory.CreateDirectory(FileSystemUtil.PersistenDataPath);
//             string strFile = Framework.Core.CommonUtility.stringBuilder.Append(FileSystemUtil.PersistenDataPath).Append("Unlock_").Append(UserManager.MySelf.userID).ToString();
//             BinaryUtil writer = new BinaryUtil();
//             writer.WriteUshort((ushort)m_vUnLockeds.Count);
//             foreach (var db in m_vUnLockeds)
//             {
//                 writer.WriteUint32(db.Key);
//                 writer.WriteByte(db.Value);
//             }
//             writer.SaveTo(strFile);
//         }
//         //------------------------------------------------------
//         public override void RefreshState(UnLockListener listener)
//         {
//             if (listener == null || !Core.DebugConfig.bEnableModuleLock) return;
//             CsvData_Unlock.UnlockData unlockData = listener.cfg as CsvData_Unlock.UnlockData;
//             if (unlockData == null) return;
//             switch ((LockView)unlockData.lockType)
//             {
//                 case LockView.None:
//                     break;
//                 case LockView.Gray:
//                     SetControlGoGray(listener, listener.moduleLockState <= EModuleLockState.Lock);
//                     break;
//                 case LockView.Hide:
//                     if (listener.controlGo != null && listener.controlGo.Count > 0)
//                     {
//                         foreach (var item in listener.controlGo)
//                         {
//                             if (item == null)
//                             {
//                                 continue;
//                             }
//                             item.SetActive(listener.moduleLockState > EModuleLockState.Lock);
//                         }
//                     }
//                     else
//                     {
//                         listener.gameObject.SetActive(listener.moduleLockState > EModuleLockState.Lock);
//                     }
//                     break;
//                 default:
//                     break;
//             }
//         }
//         //------------------------------------------------------
//         void RefreshLocks(uint id, byte state)
//         {
//             byte oldState;
//             if(m_vUnLockeds.TryGetValue(id, out oldState))
//             {
//                 if (state <= (byte)EModuleLockState.Lock)
//                 {
//                     m_vUnLockeds.Remove(id);
//                     m_fDirtyLock = Time.time + 30;
//                 }
//                 if (oldState == state) return;
//             }
//             else
//             {
//                 if (state <= (byte)EModuleLockState.Lock)
//                     return;
//             }
//             m_vUnLockeds[id] = state;
//             m_fDirtyLock = Time.time+30;
//         }
//         //------------------------------------------------------
//         /// <summary>
//         /// 当UI初始化的时候
//         /// </summary>
//         public override void Init()
//         {
//             UI.UIBase.OnGlobalShowUI += OnShowUI;
//             UI.UIBase.OnGlobalMoveInUI += OnShowUI;
//             ////uibase;todo:这边锁功能需要一个界面打开刷新Ui后的回调,然后才进行锁功能刷新
//         }
//         //------------------------------------------------------
//         private void OnShowUI(UIBase ui)
//         {
//             if (!ui.IsVisible())
//                 return;
//             if (ui.GetUIType() == (ushort)EUIType.UnLockPanel)//显示某些界面不触发模块锁检测
//             {
//                 return;
//             }
//             CheckUnlockWaitShow();
//             //检查未解锁列表
//             CheckModuleUnlock(0, UnLockModuleType.None);
//         }
//         //------------------------------------------------------
//         public void OnChangeState(Logic.EGameState lastState, Logic.EGameState state)
//         {
//             if (lastState > Logic.EGameState.Login && state == Logic.EGameState.Login)
//                 SaveUnLocked();
//         }
//         //------------------------------------------------------
//         void CheckUnlockWaitShow()
//         {
//             if (!m_bEnable) return;
//             Unlock unlock;
//             foreach (var db in m_vUnLockeds)
//             {
//                 if (db.Value == (byte)EModuleLockState.UnlockWaitShow)
//                 {
//                     if (m_LockInfos.TryGetValue(db.Key, out unlock))
//                     {
//                         if (unlock.OnUnlock())
//                             RefreshLocks(db.Key, (byte)EModuleLockState.UnlockWaitShow);
//                     }
//                 }
//             }     
//         }
//         //------------------------------------------------------
//         protected override void OnDestroy()
//         {
//             if (TopGame.SvrData.UserManager.getInstance() != null)
//             {
//                 BattleDB battleDb = TopGame.SvrData.UserManager.getInstance().mySelf.ProxyDB<BattleDB>(Data.EDBType.Battle);
//                 battleDb.OnChapterDataChange -= OnChapterDataChange;
// 
//                 BaseDB baseDB = UserManager.getInstance().mySelf.ProxyDB<BaseDB>(EDBType.BaseInfo);
//                 baseDB.OnInfiniteDistanceChange -= OnInfiniteDistanceChange;
// 
//                 BuildingDB buildingDb = UserManager.MySelf.ProxyDB<BuildingDB>(EDBType.Building);
//                 buildingDb.OnDirtyBuildingStateChange -= OnBuildingChunkStateChange;
// 
//                 ChapterTaskDB.OnChapterChange -= OnChapterChange;
//             }
// 
//             Clear();
// 
//             UI.UIBase.OnGlobalShowUI -= OnShowUI;
//             UI.UIBase.OnGlobalMoveInUI -= OnShowUI;
//         }
//         //------------------------------------------------------
//         public override void Clear()
//         {
//             if (UserManager.Current == UserManager.MySelf && UserManager.MySelf != null)
//                 SaveUnLocked();
// 
//             Unlock unlocks;
//             foreach (var item in m_LockInfos)
//             {
//                 unlocks = item.Value;
//                 unlocks.OnClear();
//             }
//             m_vUnLockeds.Clear();
//             m_LockInfos.Clear();
// 
//             //clear时有两种情况,一个是切换账号,另一个是关闭游戏
//             //切换账号时,不进行清理 m_vAllListener 所有记录的listener,再下个账号进入时,进行UI的刷新判断
//         }
//         //------------------------------------------------------
//         public void Update(float fFrame)
//         {
//             if (!m_bEnable)
//             {
//                 return;
//             }
//             if(m_fDirtyLock>1)
//             {
//                 if(Time.time > m_fDirtyLock)
//                     SaveUnLocked();
//             }
//         }
//         //------------------------------------------------------
//         public override bool CanShowTweenEffect() 
//         {
//             if (GameInstance.getInstance() == null) return true;
//             return !GameInstance.getInstance().IsLoading(); 
//         }
//         //------------------------------------------------------
//         public override Framework.Core.IContextData GetUnlockData(uint id)
//         {
//             return DataManager.getInstance().Unlock.GetData(id);
//         }
//         //------------------------------------------------------
//         public override bool IsPlayingUnLockTipsTween()
//         {
//             //如果UnLockPanel在显示状态就是在播放,否则就不在播放状态
//             return GameInstance.getInstance().uiManager.IsShow((ushort)TopGame.UI.EUIType.UnLockPanel);
//         }
//         //------------------------------------------------------
//         public override void SetUnLockState(UnLockListener listener, EModuleLockState state = EModuleLockState.Unlock)
//         {
//             if (listener == null) return;
// 
//             if (listener.info.id == 0)
//             {
//                 listener.OnUnlock();
//                 return;
//             }
//             m_vUnLockeds[listener.info.id] = (byte)state;
//             listener.OnUnlock();
//             Unlock unlock;
//             if(m_LockInfos.TryGetValue(listener.info.id, out unlock))
//             {
//                 if(unlock.listeners!=null) unlock.listeners.Remove(listener);
//             }
//         }
//         //------------------------------------------------------
//         public override void AddUnLockUIItem(UnLockListener listener)
//         {
//             if (listener == null || listener.cfg == null)
//             {
// #if UNITY_EDITOR
//                 if (listener)
//                     Log(listener.info.id + "  无效锁配置");
// #endif
//                 return;
//             }
//             uint id = listener.info.id;
// 
//             AddItemToAllListener(id,listener);
// 
//             byte lockState;
//             if (m_vUnLockeds.TryGetValue(id, out lockState))
//             {
//                 listener.moduleLockState = (EModuleLockState)lockState;
//                 switch (listener.moduleLockState)//刷新状态后,调用一次对应状态函数
//                 {
//                     case EModuleLockState.Lock:
//                         listener.OnLock();
//                         break;
//                     case EModuleLockState.UnlockWaitShow:
//                     case EModuleLockState.Unlock:
//                         listener.OnUnlock();
//                         break;
//                     default:
//                         break;
//                 }
//                 return;
//             }
//             if(m_bEnable)
//                 listener.moduleLockState = EModuleLockState.Lock;
// 
//             Unlock unlocks;
//             if(!m_LockInfos.TryGetValue(id, out unlocks))//判断是否存在已有id
//             {
//                 unlocks = new Unlock();
//                 unlocks.unLockData = listener.cfg as CsvData_Unlock.UnlockData;
//                 m_LockInfos.Add(id, unlocks);
//                 Log(Util.stringBuilder.Append("添加模块锁id:" + id).Append("state:").Append(listener.moduleLockState).ToString());
//             }
//             if (unlocks.listeners == null) unlocks.listeners = new List<UnLockListener>(2);
//             //过滤一样的listener
//             if (!unlocks.listeners.Contains(listener))
//             {
//                 unlocks.listeners.Add(listener);//一个id对应多个listener
//             }
//             
//             m_LockInfos[id] = unlocks;
//             //添加完一次,就刷新一次判断是否解锁
//             CheckModule(unlocks);
//             Log(Util.stringBuilder.Append("添加完刷新后模块锁id:" + id).Append("state:").Append(listener.moduleLockState).ToString());
//         }
//         //------------------------------------------------------
//         void AddItemToAllListener(uint id, UnLockListener listener)
//         {
//             if (m_vAllListener.ContainsKey(id))
//             {
//                 var item = m_vAllListener[id];
//                 if (item.listeners == null)
//                 {
//                     item.listeners = new List<UnLockListener>();
//                 }
//                 if (!item.listeners.Contains(listener))//过滤重复添加情况
//                 {
//                     item.listeners.Add(listener);//一个id对应多个listener
//                 }
//             }
//             else
//             {
//                 var data = new Unlock();
//                 data.unLockData = listener.cfg as CsvData_Unlock.UnlockData;
// 
//                 data.listeners = new List<UnLockListener>();
//                 data.listeners.Add(listener);
// 
//                 m_vAllListener[id] = data;
//             }
//         }
//         //------------------------------------------------------
//         public override void RemoveUnlockListener(UnLockListener listener)
//         {
//             Unlock unlocks;
//             if (m_LockInfos.TryGetValue(listener.info.id, out unlocks))
//             {
//                 unlocks.listeners.Remove(listener);
//                 m_LockInfos[listener.info.id] = unlocks;
//             }
// 
//             if (m_vAllListener.TryGetValue(listener.info.id, out unlocks))
//             {
//                 unlocks.listeners.Remove(listener);
//                 m_vAllListener[listener.info.id] = unlocks;
//             }
//         }
//         //------------------------------------------------------
//         /// <summary>
//         /// 当新增解锁item的时候刷新
//         /// 当有界面打开的时候刷新
//         /// 当关卡进度或者无限跑酷距离变动的时候刷新
//         /// </summary>
//         void CheckModuleUnlock(uint key, UnLockModuleType unlockType)
//         {
//             if (!m_bEnable)
//             {
//                 return;
//             }
// 
//             if (key == 0)
//             {
//                 CheckModule(unlockType);
//             }
//             else
//             {
//                 CheckModule(key);
//             }
//         }
//         //------------------------------------------------------
//         void CheckModule(UnLockModuleType unlockType = UnLockModuleType.None)
//         {
//             if (!m_bEnable) return;
//             var keys = ListPool<uint>.Get();
//             keys.AddRange(m_LockInfos.Keys);
//             if (unlockType == UnLockModuleType.None)
//             {
//                 foreach (var item in keys)
//                 {
//                     CheckModule(m_LockInfos[item]);
//                 }
//             }
//             else
//             {
//                 Unlock unlock;
//                 foreach (var item in keys)
//                 {
//                     unlock = m_LockInfos[item];
//                     if (unlock.unLockData.lockType != (int)unlockType) continue;
//                     CheckModule(unlock);
//                 }
//             }
//             ListPool<uint>.Release(keys);
//         }
//         //------------------------------------------------------
//         void CheckModule(Unlock unlock)
//         {
//             if (!m_bEnable || UserManager.Current == null) return;
//             if (!unlock.IsValid())
//             {
//                 //Log("id:" + id + ", is null!!!");
//                 return;
//             }
// 
//             if (!IsLocked(unlock.unLockData.id))
//             {
//                 if (unlock.OnUnlock()) RefreshLocks(unlock.unLockData.id, (byte)EModuleLockState.Unlock);
//                 else RefreshLocks(unlock.unLockData.id, (byte)EModuleLockState.UnlockWaitShow);
//             }
//             else
//             {
//                 unlock.OnLock();
//                 RefreshLocks(unlock.unLockData.id, (byte)EModuleLockState.Lock);
//             }
//         }
//         //------------------------------------------------------
//         void CheckModule(uint id)
//         {
//             Unlock unlock;
//             if (m_LockInfos.TryGetValue(id, out unlock))
//             {
//                 CheckModule(unlock);
//             }
//         }
//         //------------------------------------------------------
//         bool CanPlayUnlockAnimator(UnLockListener listener)
//         {
//             bool canPlay = false;
// 
//             if (listener == null)
//             {
//                 return canPlay;
//             }
// 
//             //如果界面打开,但是按钮被隐藏,要加个判断
//             //根据当前挂载的界面是否显示,判断是否能够播放
//             if (GameInstance.getInstance().uiManager.IsShow((ushort)listener.info.binderUI) && listener.gameObject.activeInHierarchy)
//             {
//                 canPlay = true;
// 
//                 //Log("能够播放按钮名字:" + listener.name + ",binderUI:" + listener.info.binderUI);
//             }
// 
//             return canPlay;
//         }
//         //------------------------------------------------------
//         private void OnChapterDataChange(LevelTypeCode levelType, uint id)
//         {
//             if (levelType == LevelTypeCode.Default)
//             {
//                 CheckModuleUnlock(0, UnLockModuleType.DefaultLevel);
//             }
//         }
//         //------------------------------------------------------
//         private void OnBuildingChunkStateChange(BuildingInfo buildingChunk)
//         {
//             if (buildingChunk == null) return;
//             if(buildingChunk.buildingState ==  EBuildingState.BuildingUpgradConfirm)
//             {
//                 CheckModuleUnlock(0, UnLockModuleType.BuildingLevel);
//             }
//         }
//         //------------------------------------------------------
//         private void OnInfiniteDistanceChange(int distance)
//         {
//             CheckModuleUnlock(0, UnLockModuleType.InfiniteParkourDistance);
//         }
//         //------------------------------------------------------
//         private void OnChapterChange(int chapterID)
//         {
//             CheckModuleUnlock(0, UnLockModuleType.ChapterID);
//         }
//         //------------------------------------------------------
//         public override void OnChangeAccount()
//         {
//             LoadUnLocked();
//             foreach (var item in m_LockInfos)
//             {
//                 item.Value.OnClear();
//                 CheckModule(item.Value);
//             }
//         }
//         //------------------------------------------------------
//         bool Contains(GameObject item,ref UnLockListener listener)
//         {
//             if (item == null)
//             {
//                 return false;
//             }
// 
//             foreach (var info in m_LockInfos)
//             {
//                 if (info.Value.IsContain(item)) return true;
//             }
//             return false;
//         }
//         //------------------------------------------------------
//         [Framework.Plugin.AT.ATMethod("显示GameObjects(模块锁)")]
//         public void ShowGameObjectsWithCheckLock(List<GameObject> items)
//         {
//             if (items == null) return;
//             
//             foreach (var item in items)
//             {
//                 UnLockListener listener = null;
//                 if (item)
//                 {
//                     if (Contains(item, ref listener))
//                     {
//                         if (listener.moduleLockState == EModuleLockState.Lock)//显示的话,只有锁定状态才跳过,
//                         {
//                             //至于按钮的置灰还是隐藏,由自己的UnLockListener进行管理
//                             continue;
//                         }
//                     }
//                     item.SetActive(true);
//                 }
//             }
//         }
//         //------------------------------------------------------
//         [Framework.Plugin.AT.ATMethod("刷新并且显示GameObjects(模块锁)")]
//         public void RefreshAndShowGameObjectsWithCheckLock(List<GameObject> items)
//         {
//             if (items == null) return;
//             CheckModuleUnlock(0, UnLockModuleType.None);//刷新状态
// 
//             foreach (var item in items)
//             {
//                 UnLockListener listener = null;
//                 if (item && Contains(item, ref listener))
//                 {
//                     if (listener.moduleLockState == EModuleLockState.Lock)//显示的话,只有锁定状态才跳过,
//                     {
//                         //至于按钮的置灰还是隐藏,由自己的UnLockListener进行管理
//                         continue;
//                     }
//                     item.SetActive(true);
//                     //Log("显示解锁物体:" + item.name);
//                 }
//             }
// 
//         }
//         //------------------------------------------------------
//         [Framework.Plugin.AT.ATMethod("隐藏GameObjects(模块锁)")]
//         public void HideGameObjectsWithCheckLock(List<GameObject> items)
//         {
//             //目前暂时没用到
//             if (items == null) return;
//             
//             foreach (var item in items)
//             {
//                 if (item)
//                 {
//                     //隐藏的话,不管是否解锁,直接隐藏
//                     item.SetActive(false);
//                 }
//             }
// 
//         }
//         //------------------------------------------------------
//         [Framework.Plugin.AT.ATMethod("显示UI(模块锁)")]
//         public bool ShowUIWithCheckLock(EUIType ui)
//         {
//             if (m_bEnable)
//             {
//                 //过滤还未解锁界面
//                 if (IsUILocked((uint)ui))
//                 {
//                     return false;
//                 }
//             }
//             
//             GameInstance.getInstance().uiManager.ShowUI((ushort)ui);
//             return true;
//         }
//         //------------------------------------------------------
//         public void Log(string log)
//         {
//             if (Core.DebugConfig.bEnableModuleLockLog)
//             {
//                 Debug.Log(log);
//             }
//         }
//         //------------------------------------------------------
//         public override bool IsUILockAndTip(uint uiType)
//         {
//             var datas = DataManager.getInstance().Unlock.datas;
// 
//             var tempList = ListPool<CsvData_Unlock.UnlockData>.Get();
//             
//             foreach (var data in datas)
//             {
//                 if (data.Value.uiType == (uint)uiType)//多个界面ID一样的情况处理
//                 {
//                     tempList.Add(data.Value);
//                 }
//             }
// 
//             bool isLock = true;
//             if (tempList.Count == 0)
//             {
//                 isLock = false;
//             }
//             else
//             {
//                 foreach (var item in tempList)
//                 {
//                     if (IsLocked(item.id) == false)
//                     {
//                         isLock = false;
//                         break;//同类型只要有一个解锁,就解锁
//                     }
//                 }
//             }
//             
// 
//             ListPool<CsvData_Unlock.UnlockData>.Release(tempList);
// 
//             return isLock;
//         }
//         //------------------------------------------------------
//         public override bool IsLockAndTip(uint id, Framework.Core.IContextData cfg = null)
//         {
//             CsvData_Unlock.UnlockData unlockData = null;
//             if (cfg == null)
//                 unlockData = DataManager.getInstance().Unlock.GetData(id);
//             else unlockData = cfg as CsvData_Unlock.UnlockData;
//             if (id == 0 && unlockData!=null) id = unlockData.id;
//             if (unlockData != null && IsLocked(id) && m_bEnable)
//             {
//                 switch ((UnLockModuleType)unlockData.unlockType)
//                 {
//                     case UnLockModuleType.DefaultLevel:
//                         {
//                             var chapterCfg = DataManager.getInstance().Chapter.GetData((uint)unlockData.tipsChapter);
//                             if (chapterCfg != null)
//                             {
//                                 string chapterName = Core.LocalizationManager.ToLocalization(chapterCfg.nameId);
//                                 string strTipsFormat = Core.LocalizationManager.ToLocalization(unlockData.tips);
//                                 string tips = "--";
//                                 if (!string.IsNullOrEmpty(strTipsFormat) && strTipsFormat.Contains("{0}"))
//                                     tips = string.Format(strTipsFormat, chapterName);
//                                 Base.Util.ShowCommonTip(ETipType.AutoHide, tips);
//                             }
//                             else
//                             {
//                                 Base.Util.ShowCommonTip(UI.ETipType.AutoHide, 10016000);//找不到关卡配置表的情况
//                             }
//                         }
//                         break;
//                     case UnLockModuleType.BuildingLevel:
//                         //建筑解锁提示
//                         string buildName = "";
// 
//                         var building = UserManager.Current.GetBuildingDB().GetBuildInfoByBuildingType((uint)unlockData.unlock);
//                         if (building != null)
//                         {
//                             buildName = BuildingUtil.GetBuildingName(building.ConfigData);
//                         }
//                         
//                         string limitLevel = unlockData.unlock1.ToString();
//                         string format = Core.LocalizationManager.ToLocalization(unlockData.tips);
//                         
//                         //判断是否可以替换
//                         if (format != null && buildName != null && format.Contains("{0}") && format.Contains("{1}"))
//                         {
//                             string lockTips = string.Format(format, buildName, limitLevel);
//                             Base.Util.ShowCommonTip(ETipType.AutoHide, lockTips);
//                         }
//                         else
//                         {
//                             Base.Util.ShowCommonTip(UI.ETipType.AutoHide, 10016000);
//                         }
//                         break;
//                     case UnLockModuleType.ChapterID:
//                         string tipsFormat = Core.LocalizationManager.ToLocalization(unlockData.tips);
//                         var chapterDb = UserManager.Current.GetChapterTaskDB();
//                         if (chapterDb != null)
//                         {
//                             if (!string.IsNullOrEmpty(tipsFormat) && tipsFormat.Contains("{0}"))
//                             Base.Util.ShowCommonTip(ETipType.AutoHide, string.Format(tipsFormat, chapterDb.CurChapterID));
//                         }
//                         break;
//                     default:
//                         Base.Util.ShowCommonTip(UI.ETipType.AutoHide, 10016000);//找不到关卡配置表的情况
//                         break;
//                 }
// 
//                 return true;
//             }
//             return false;
//         }
//         //------------------------------------------------------
//         /// <summary>
//         /// 判断某个UI是否已经锁住
//         /// </summary>
//         /// <param name="ui"></param>
//         /// <returns></returns>
//         public override bool IsUILocked(uint uiType)
//         {
//             if (uiType == (uint)EUIType.None || !m_bEnable) return false;
//             var datas = DataManager.getInstance().Unlock.datas;
//             foreach (var data in datas)
//             {
//                 if (data.Value.uiType == uiType && IsLocked(data.Value.id))
//                 {
//                     return true;
//                 }
//             }
// 
//             return false;
//         }
//         //------------------------------------------------------
//         public override bool IsLocked(uint id)
//         {
//             if (!m_bEnable || UserManager.Current == null) return false;
//             var cfg = DataManager.getInstance().Unlock.GetData(id);
//             if (cfg == null)
//             {
//                 //Log("id:" + id + ", is null!!!");
//                 return false;
//             }
// 
//             switch ((UnLockModuleType)cfg.unlockType)
//             {
//                 case UnLockModuleType.DefaultLevel://主线
//                     uint level = UserManager.Current.ProxyDB<BattleDB>().GetCurrentLevelID(LevelTypeCode.Default);
//                     if (level <= 0)//防止因为id不对,导致状态被刷新的问题
//                     {
//                         //Log("当前关卡id不对,暂时不检测解锁条件,btn:" + info.name);
//                         return false;
//                     }
//                     return cfg.unlock > level;
//                 case UnLockModuleType.InfiniteParkourDistance://无限跑酷
//                     int infiniteDistance = UserManager.Current.ProxyDB<BaseDB>().InfiniteDistance;
//                     return cfg.unlock > infiniteDistance;
//                 case UnLockModuleType.BuildingLevel:
//                     BuildingDB buildingDb = Logic.BuildingManager.UserData.ProxyDB<BuildingDB>();
//                     var buildings = buildingDb.GetBuildInfosByBuildingType((uint)cfg.unlock);
// 
//                     //同一个类型,多个建筑的判断
//                     bool unLock = false;
//                     for (int i = 0; i < buildings.Count; i++)
//                     {
//                         if (buildings[i].level >= cfg.unlock1)
//                         {
//                             unLock = true;
//                             break;
//                         }
//                     }
//                     return !unLock;
//                 case UnLockModuleType.ChapterID:
//                     var taskDb = UserManager.Current.GetChapterTaskDB();
//                     if (taskDb == null)
//                     {
//                         return false;
//                     }
//                     var chapterCfg = taskDb.GetCurChapterCfg();
//                     if (chapterCfg == null)
//                     {
// #if UNITY_EDITOR
//                         Util.LogEditor("获取不到当前章节配置!无法进行模块锁更新!");
// #endif
//                         return false;
//                     }
//                     uint chapterID = chapterCfg.id;
//                     if (chapterID <= 0)
//                     {
// #if UNITY_EDITOR
//                         Util.LogEditor("过滤异常的章节ID模块锁更新!");
// #endif
//                         return false;
//                     }
//                     return cfg.unlock > chapterID;
//             }
// 
//             return false;
//         }
//         //------------------------------------------------------
//         public override void PopUnlockTipPanel(UnLockListener listener)
//         {
//             var panel = GameInstance.getInstance().uiManager.CastGetUI<UI.UIUnLockPanel>((ushort)UI.EUIType.UnLockPanel);
//             if (panel != null)
//             {
//                 panel.SetUnLockData(listener);
//                 panel.Show();
//             }
//         }
//         //------------------------------------------------------
//         void SetControlGoGray(UnLockListener listener, bool gray)
//         {
//             if (listener == null) return;
//             if (listener.controlGo != null && listener.controlGo.Count > 0)
//             {
//                 foreach (var item in listener.controlGo)
//                 {
//                     if (item)
//                     {
//                         UIUtil.SetGrayUI(item, gray, listener.grayColor);
//                     }
//                 }
//             }
//             else
//             {
//                 UIUtil.SetGrayUI(listener.gameObject, gray, listener.grayColor);
//             }
//         }
// #if UNITY_EDITOR
//         //------------------------------------------------------
//         public override void PrintDebug()
//         {
//             foreach (var item in m_LockInfos)
//             {
//                 foreach (var listener in item.Value.listeners)
//                 {
//                     if (listener == null)
//                     {
//                         continue;
//                     }
//                     Debug.Log("id:" + listener.info.id + ",state:" + listener.moduleLockState);
//                 }
//             }
// 
//             foreach (var data in m_vUnLockeds)
//             {
//                 Debug.Log("id:" + data.Key + ",state:" + (EModuleLockState)data.Value);
//             }
//         }
// #endif
//     }
// }
// 
// 
