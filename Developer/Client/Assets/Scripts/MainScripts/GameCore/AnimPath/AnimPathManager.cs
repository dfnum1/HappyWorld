/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	AnimPathManager
作    者:	HappLI
描    述:	
*********************************************************************/
using System;
using System.Collections.Generic;
using Framework.Core;
using Framework.Module;
using UnityEngine;
using UnityEngine.Playables;

namespace TopGame.Core
{
    [Framework.Plugin.AT.ATExportMono("路径动画系统", "TopGame.GameInstance.getInstance().animPather")]
    public class AnimPathManager : Framework.Module.AModule, Framework.Module.IUpdate
    {
        public static System.Action OnCutSceneEnd = null;
        
        private List<IPlayableBase> m_vDirectors = null;

        private List<IAnimPathCallback> m_vCallback = new List<IAnimPathCallback>();

        List<IPlayableBase> m_vRecyles = null;
        private bool m_bPauseAniPath = false;
        protected override void Awake()
        {
            m_bPauseAniPath = false;
        }
        //------------------------------------------------------
        protected override void OnDestroy()
        {
            m_vCallback.Clear();
            Clear();
        }
        //---------------------------------------------
        public void Clear()
        {
            m_bPauseAniPath = false;
            if (m_vDirectors != null)
            {
                for(int i = 0; i < m_vDirectors.Count; ++i)
                {
                    m_vDirectors[i].Destroy();
                }
                m_vDirectors.Clear();
            }
        }
        //---------------------------------------------
        public void RegisterCallback(IAnimPathCallback callback)
        {
            if (callback == null) return;
            if (m_vCallback.Contains(callback)) return;
            m_vCallback.Add(callback);
        }
        //---------------------------------------------
        public void UnRegisterCallback(IAnimPathCallback callback)
        {
            m_vCallback.Remove(callback);
        }
        //---------------------------------------------
        public bool HasPlaying(int guid)
        {
            if (m_vDirectors == null) return false;
            //PlayableData ret = m_vDirectors.Find(able => { return able.guid == guid; });
            for(int i = 0; i < m_vDirectors.Count; ++i)
            {
                if (m_vDirectors[i].GetGuid() == guid)
                    return true;
            }
            return false;
        }
        //---------------------------------------------
        public IPlayableBase GetPlayAble(int guid)
        {
            if (m_vDirectors == null) return null;
            //PlayableData ret = m_vDirectors.Find(able => { return able.guid == guid; });
            for (int i = 0; i < m_vDirectors.Count; ++i)
            {
                if (m_vDirectors[i].GetGuid() == guid)
                    return m_vDirectors[i];
            }
            return null;
        }
        //---------------------------------------------
        public IPlayableBase GetPlayAble(string strAssetName)
        {
            if (m_vDirectors == null || string.IsNullOrEmpty(strAssetName)) return null;
            //PlayableData ret = m_vDirectors.Find(able => { return able.guid == guid; });
            for (int i = 0; i < m_vDirectors.Count; ++i)
            {
                if (m_vDirectors[i].GetAssetName().CompareTo(strAssetName) == 0)
                    return m_vDirectors[i];
            }
            return null;
        }
        //---------------------------------------------
        public IPlayableBase  PlayAnimPathImmediate(int id, GameObject pTrigger, GameObject pTarget, GameObject lookatTarget = null, bool bForce = true)
        {
            if (pTarget == null) return null;

            if(!bForce)
            {
               IPlayableBase playing = GetPlayAble(id);
                if (playing != null)
                    return playing;
            }

            Data.TargetPathData path = Data.DataManager.getInstance().TargetPaths.GetData(id);
            if (path == null) return null;
            if (!string.IsNullOrEmpty(path.animClip))
            {
                if (pTarget == null && RootsHandler.CameraControllRoot)
                {
                    pTarget = RootsHandler.CameraControllRoot.gameObject;
                }
                Asset transCurve = (Asset)FileSystemUtil.LoadAsset(path.animClip,false, true);
                return PlayAnimPath(id, pTrigger, pTarget, path.OffsetPosition, path.OffsetRotate, transCurve, path.keys, path.events, null, false, Vector3.zero, path.MirrorReference, path.useEndType, true, lookatTarget);
            }
            if (path.timeline != null && path.timeline.Length > 0)
            {
                if (HasPlaying(id))
                    return null;
               return Play(id, pTarget, path.timeline, path.OffsetPosition, path.OffsetRotate, path.bCloseGameCamera, path.events, path.useEndType, path.isUIPath);
            }
            return null;
        }
        //---------------------------------------------
        AnimPathData PlayAnimPath(int Guid, GameObject pTrigger, GameObject pTarget, Vector3 OffsetPos, Vector3 OffsetRot, SplineCurve.KeyFrame[] framekeys, BaseEventParameter[] events, AnimationCurve speedCurve,
            bool bLookAt, Vector3 lookatPos, Vector3 mirrorReference, ushort bUseEnd, bool bForce, GameObject lookatTarget = null)
        {
            if (pTarget == null) return null;
            AnimPathData playAble = null;
            if (bForce)
            {
                playAble = GetPlayAble(Guid) as AnimPathData;
            }
            else if (HasPlaying(Guid))
            {
                return null;
            }
            if (m_vDirectors == null) m_vDirectors = new List<IPlayableBase>();
            if(playAble == null)
            {
                playAble = PopNewPlayAble<AnimPathData>();
                if (playAble == null) playAble = new AnimPathData();
                m_vDirectors.Add(playAble);
            }
            if (playAble.director == null)
                playAble.director = new AnimationPathDirector();
            playAble.director.Clear();
            playAble.director.SetPlayAble(playAble);

            playAble.guid = Guid;
            playAble.SetAssetName(Guid.ToString());
            playAble.SetTarget(pTarget.transform);

            if (pTrigger != null)
            {
                TimelineController pCmera = pTrigger.GetComponent<TimelineController>();
                if (pCmera)
                {
                    playAble.SetFollow(pCmera.follow);
                    playAble.SetCamera(pCmera.mainCamera);
                    playAble.SetSlots(pCmera.slots);
                }
            }

            playAble.director.PlayAnimPath(pTrigger, pTarget, framekeys, events, speedCurve, bLookAt, lookatPos, mirrorReference, bUseEnd, lookatTarget);
            CheckSkipToEvent(playAble);
            for(int i =0; i< m_vCallback.Count; ++i)
            {
                m_vCallback[i].OnAnimPathBegin(playAble);
            }
            playAble.TriggerEvent();
            return playAble;
        }
        //---------------------------------------------
        AnimPathData PlayAnimPath(int Guid, GameObject pTrigger, GameObject target, Vector3 OffsetPos, Vector3 OffsetRot, Asset clip, SplineCurve.KeyFrame[] fov, BaseEventParameter[] events, AnimationCurve speedCurve,
            bool bLookAt, Vector3 lookatPos, Vector3 mirrorReference, ushort bUseEnd, bool bForce, GameObject lookatTarget = null)
        {
            if (clip == null) return null;
            if(target == null)
            {
                clip.Release();
                return null;
            }
            AnimationClip clipAsset = clip.GetOrigin<AnimationClip>();
            if (clipAsset == null)
            {
                clip.Release();
                return null;
            }
            AnimPathData playAble = null;
            if (bForce)
            {
                playAble = GetPlayAble(Guid) as AnimPathData;
            }
            else if (HasPlaying(Guid))
            {
                clip.Release();
                return null;
            }
            if (m_vDirectors == null) m_vDirectors = new List<IPlayableBase>();
            if (playAble == null)
            {
                playAble = PopNewPlayAble<AnimPathData>();
                if (playAble == null) playAble = new AnimPathData();
                m_vDirectors.Add(playAble);
            }
            if (playAble.director == null)
                playAble.director = new AnimationPathDirector();
            playAble.director.Clear();
            playAble.director.SetPlayAble(playAble);

            playAble.guid = Guid;
            playAble.SetAssetName(Guid.ToString());
            playAble.SetTarget(target.transform);
			
            if (pTrigger != null)
            {
                TimelineController pCmera = pTrigger.GetComponent<TimelineController>();
                if (pCmera)
                {
                    playAble.SetFollow(pCmera.follow);
                    playAble.SetCamera(pCmera.mainCamera);
                    playAble.SetSlots( pCmera.slots);
                }
            }

            playAble.director.PlayAnimPath(pTrigger, target, clip, fov, events, speedCurve, bLookAt, lookatPos, mirrorReference, bUseEnd, lookatTarget);
            CheckSkipToEvent(playAble);
            for (int i = 0; i < m_vCallback.Count; ++i)
            {
                m_vCallback[i].OnAnimPathBegin(playAble);
            }
            playAble.TriggerEvent();
            return playAble;
        }
        //---------------------------------------------
        TimelineData Play(int guid, GameObject pTarget, string timeline, Vector3 OffsetPos, Vector3 OffsetRot, bool bCloseGameCamera = false, BaseEventParameter[] events = null,
            ushort bUseEnd = 0, bool bUIPath = false)
        {
            if (timeline == null) return null;
            if (HasPlaying(guid))
            {
                return null;
            }

            if (m_vDirectors == null) m_vDirectors = new List<IPlayableBase>();

            TimelineData playAble = PopNewPlayAble<TimelineData>();
            if (playAble == null) playAble = new TimelineData();

            playAble.target = pTarget != null? pTarget.transform:null;
            playAble.guid = guid;
            playAble.SetSkipTitle(false, 0);
            if (events != null && events.Length > 0)
            {
                if(playAble.events == null) playAble.events = new List<BaseEventParameter>();
                for (int i = 0; i < events.Length; ++i)
                {
                    playAble.events.Add(events[i]);
                }
                Framework.Plugin.SortUtility.QuickSortUp<BaseEventParameter>(ref playAble.events);
            }
            else
                playAble.events = null;

            CheckSkipToEvent(playAble);
            playAble.bUseEnd = bUseEnd;
            playAble.Enable(false);
            playAble.bCloseGameCamera = bCloseGameCamera;
            if(bCloseGameCamera)
            {
                if(CameraKit.cameraController!=null) CameraKit.cameraController.ActiveRoot(false);
            }
            for (int i = 0; i < m_vCallback.Count; ++i)
            {
                m_vCallback[i].OnAnimPathBegin(playAble);
            }

            m_vDirectors.Add(playAble);

            InstanceOperiaon callback = FileSystemUtil.SpawnInstance(timeline, false);
            if (callback != null)
            {
                callback.OnSign = playAble.OnSpawnInstanceSign;
                callback.OnCallback = playAble.OnSpawnInstanceCallback;
                if(bUIPath)
                    callback.pByParent = UI.UIManager.GetAutoUIRoot();
                else
                    callback.pByParent = RootsHandler.ScenesRoot;
                callback.Refresh();
            }
            playAble.TriggerEvent();
            return playAble;
        }
        //---------------------------------------------
        void CheckSkipToEvent(IPlayableBase playAble)
        {
            if (playAble == null || playAble.GetEvents()==null) return;
            float fSkipTime = 0;
            bool bSkipEvent = false;
            List<BaseEventParameter> vEvents = playAble.GetEvents();
            for (int i = 0; i < vEvents.Count;)
            {
                if (vEvents[i].GetEventType() == EEventType.TargetPath)
                {
                    TargetPathEventParameter paramer = vEvents[i] as TargetPathEventParameter;
                    if (paramer.ctlType == TargetPathEventParameter.EType.Skip)
                    {
                        float temp;
                        if (float.TryParse(paramer.userParam, out temp))
                            fSkipTime = Mathf.Max(fSkipTime, temp);
                        vEvents.RemoveAt(i);
                        bSkipEvent = true;
                    }
                    else
                        ++i;
                }
                else ++i;
            }
            bool bCanSkip = bSkipEvent && fSkipTime > 0;
            if (bCanSkip)
            {
                playAble.SetSkipTitle(true, fSkipTime);
            }
            else
                playAble.SetSkipTitle(false, 0);
        }
        //---------------------------------------------
        [Framework.Plugin.AT.ATMethod("暂停/继续播放路径动画")]
        public void PausePlay(bool bPause)
        {
            if (m_vDirectors == null) return;
            if (m_bPauseAniPath == bPause) return;
            m_bPauseAniPath = bPause;
            for (int i = 0; i < m_vDirectors.Count;++i)
            {
                IPlayableBase able = m_vDirectors[i];
                if (bPause) able.Pause();
                else able.Resume();
            }
        }
        //---------------------------------------------
        [Framework.Plugin.AT.ATMethod("执行跳过事件")]
        public void DoSkipEvent()
        {
            if (m_vDirectors == null) return;
            for (int i = 0; i < m_vDirectors.Count; ++i)
            {
                IPlayableBase able = m_vDirectors[i];
                able.SkipDo();
            }
        }
        //---------------------------------------------
        [Framework.Plugin.AT.ATMethod("轨道绑定")]
        public void TrackBind(string strTrackName, AInstanceAble pObject, Framework.Plugin.AT.IUserData pUserData = null, int pathID = -1, bool bGenericBinding = false)
        {
            if (m_vDirectors == null) return;
            if (pathID <= 0)
            {
                //! tack bind all
                foreach (var db in m_vDirectors)
                {
                    db.BindTrackObject(strTrackName, pObject, bGenericBinding, pUserData);
                }
            }
            else
            {
                for (int i = 0; i < m_vDirectors.Count; ++i)
                {
                    if (m_vDirectors[i].GetGuid() == pathID)
                    {
                        m_vDirectors[i].BindTrackObject(strTrackName, pObject, bGenericBinding, pUserData);
                        break;
                    }
                }
            }
        }
        //---------------------------------------------
        [Framework.Plugin.AT.ATMethod("是否已经绑定轨道上")]
        public bool HadBindTrack(string strTrackName, AInstanceAble pObject, int pathID = -1)
        {
            if (m_vDirectors == null) return false;
            if (pathID <= 0)
            {
                //! tack bind all
                foreach (var db in m_vDirectors)
                {
                    if(db.HasBinding(pObject, strTrackName))
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_vDirectors.Count; ++i)
                {
                    if (m_vDirectors[i].GetGuid() == pathID)
                    {
                        return m_vDirectors[i].HasBinding(pObject, strTrackName);
                    }
                }
            }
            return false;
        }
        //---------------------------------------------
        [Framework.Plugin.AT.ATMethod("是否已经绑定轨道上-UserData")]
        public bool HadBindTrackByUserData(string strTrackName, Framework.Plugin.AT.IUserData pUserData, int pathID = -1)
        {
            if (m_vDirectors == null) return false;
            if (pathID <= 0)
            {
                //! tack bind all
                foreach (var db in m_vDirectors)
                {
                    if (db.HasBinding(pUserData, strTrackName))
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_vDirectors.Count; ++i)
                {
                    if (m_vDirectors[i].GetGuid() == pathID)
                    {
                        return m_vDirectors[i].HasBinding(pUserData, strTrackName);
                    }
                }
            }
            return false;
        }
        //---------------------------------------------
        public PlayableBindSlot GetPlayableBindSlot(string strTrackName, int pathID = -1)
        {
            if (m_vDirectors == null || string.IsNullOrEmpty(strTrackName)) return PlayableBindSlot.DEFAULT;
            if (pathID <= 0)
            {
                //! tack bind all
                foreach (var db in m_vDirectors)
                {
                    List<PlayableBindSlot> vSlots = db.GetPlayableSlots();
                    if (vSlots != null)
                    {
                        foreach (var sub in vSlots)
                        {
                            if (sub.pUserAT == null) continue;
                            if ((sub.pSlot && sub.strName.CompareTo(strTrackName) == 0))
                            {
                                return sub;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_vDirectors.Count; ++i)
                {
                    if (m_vDirectors[i].GetGuid() == pathID)
                    {
                        List<PlayableBindSlot> vSlots = m_vDirectors[i].GetPlayableSlots();
                        if (vSlots != null)
                        {
                            foreach (var sub in vSlots)
                            {
                                if (sub.pUserAT == null) continue;
                                if ((sub.pSlot && sub.strName.CompareTo(strTrackName) == 0))
                                {
                                    return sub;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return PlayableBindSlot.DEFAULT;
        }
        //---------------------------------------------
        public PlayableBindSlot GetPlayableBindSlot(string strTrackName, string pathName=null)
        {
            if (m_vDirectors == null || string.IsNullOrEmpty(strTrackName)) return PlayableBindSlot.DEFAULT;
            if (string.IsNullOrEmpty(pathName))
            {
                //! tack bind all
                foreach (var db in m_vDirectors)
                {
                    List<PlayableBindSlot> vSlots = db.GetPlayableSlots();
                    if (vSlots != null)
                    {
                        foreach (var sub in vSlots)
                        {
                            if (sub.pUserAT == null) continue;
                            if ((sub.pSlot && sub.strName.CompareTo(strTrackName) == 0))
                            {
                                return sub;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_vDirectors.Count; ++i)
                {
                    if (m_vDirectors[i].GetAssetName().CompareTo(pathName)==0)
                    {
                        List<PlayableBindSlot> vSlots = m_vDirectors[i].GetPlayableSlots();
                        if (vSlots != null)
                        {
                            foreach (var sub in vSlots)
                            {
                                if (sub.pUserAT == null) continue;
                                if ((sub.pSlot && sub.strName.CompareTo(strTrackName) == 0))
                                {
                                    return sub;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return PlayableBindSlot.DEFAULT;
        }
        //---------------------------------------------
        public void CollectTrackBind(HashSet<Framework.Plugin.AT.IUserData> vCollects, string strTrackName, int pathID= -1)
        {
            vCollects.Clear();
            if (m_vDirectors == null) return;
            if (pathID <= 0)
            {
                //! tack bind all
                foreach (var db in m_vDirectors)
                {
                    List<PlayableBindSlot> vSlots = db.GetPlayableSlots();
                    if(vSlots!=null)
                    {
                        foreach (var sub in vSlots)
                        {
                            if (sub.pUserAT == null) continue;
                            if(strTrackName ==null || sub.strName.CompareTo(strTrackName)==0)
                            {
                                vCollects.Add(sub.pUserAT);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_vDirectors.Count; ++i)
                {
                    if (m_vDirectors[i].GetGuid() == pathID)
                    {
                        List<PlayableBindSlot> vSlots = m_vDirectors[i].GetPlayableSlots();
                        if (vSlots != null)
                        {
                            foreach (var sub in vSlots)
                            {
                                if (sub.pUserAT == null) continue;
                                if (strTrackName == null || sub.strName.CompareTo(strTrackName) == 0)
                                {
                                    vCollects.Add(sub.pUserAT);
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }
        //---------------------------------------------
        [Framework.Plugin.AT.ATMethod("跳过指定位置播放")]
        public void SkipTo(float fSkipTime, int pathID=-1)
        {
            if (m_vDirectors == null) return;
            if (pathID <= 0)
            {
                //! stop all
                foreach (var db in m_vDirectors)
                {
                    db.SkipTo(fSkipTime);
                }
            }
            else
            {
                for (int i = 0; i < m_vDirectors.Count; ++i)
                {
                    if (m_vDirectors[i].GetGuid() == pathID)
                    {
                        IPlayableBase playAble = m_vDirectors[i];
                        playAble.SkipTo(fSkipTime);
                        break;
                    }
                }
            }
        }
        //---------------------------------------------
        [Framework.Plugin.AT.ATMethod("事件开关(-1为全部)")]
        public void EnableEvent(bool bEvnet, int pathId = -1)
        {
            if (m_vDirectors == null) return;
            if (pathId <= 0)
            {
                foreach (var db in m_vDirectors)
                {
                    db.EnableEvent(bEvnet);
                }
            }
            else
            {
                for (int i = 0; i < m_vDirectors.Count; ++i)
                {
                    if (m_vDirectors[i].GetGuid() == pathId)
                    {
                        m_vDirectors[i].EnableEvent(bEvnet);
                    }
                }
            }
        }
        //---------------------------------------------
        [Framework.Plugin.AT.ATMethod("停止路径动画(-1为全部)")]
        public void Stop(int pathID = -1)
        {
            if (m_vDirectors == null) return;
            if (pathID <= 0)
            {
                //! stop all
                foreach (var db in m_vDirectors)
                {
                    for (int j = 0; j < m_vCallback.Count; ++j)
                    {
                        m_vCallback[j].OnAnimPathEnd(db);
                    }
                    db.Stop();
                    Recycle(db);
                }
                m_vDirectors.Clear();
            }
            else
            {
                IPlayableBase playAble;
                for (int i = 0; i < m_vDirectors.Count; ++i)
                {
                    playAble = m_vDirectors[i];
                    if (playAble.GetGuid() == pathID)
                    {
                        m_vDirectors.RemoveAt(i);
                        for (int j = 0; j < m_vCallback.Count; ++j)
                        {
                            m_vCallback[j].OnAnimPathEnd(playAble);
                        }
                        playAble.Stop();
                        Recycle(playAble);
                        break;
                    }
                }
            }
        }
        //---------------------------------------------
        public void OnTimelineStop(AInstanceAble able)
        {
            if (m_vDirectors == null) return;
            TimelineController playable = able as TimelineController;
            if (playable == null) return;
            for (int i = 0; i < m_vDirectors.Count;)
            {
                bool bRemove = false;
                if(m_vDirectors[i].GetTrigger() == playable.GetTransorm())
                {
                    bRemove = true;
                }
                if(!bRemove)
                {
                    if (m_vDirectors[i] is TimelineData)
                    {
                        TimelineData timeline = m_vDirectors[i] as TimelineData;
                        if (timeline.director == playable.playableDirector || timeline.trigger == playable.GetTransorm())
                        {
                            bRemove = true;
                        }
                    }
                }
                if (bRemove)
                {
                    for (int j = 0; j < m_vCallback.Count; ++j)
                    {
                        m_vCallback[j].OnAnimPathEnd(m_vDirectors[i]);
                    }
                    m_vDirectors[i].Stop();
                    Recycle(m_vDirectors[i]);
                    m_vDirectors.RemoveAt(i);
                }
                else
                    ++i;
            }
        }
        //---------------------------------------------
        [Framework.Plugin.AT.ATMethod("是否正在播放镜头动画")]
        public bool IsPlaying()
        {
            if (m_vDirectors ==null || m_vDirectors.Count <= 0) return false;
            for(int i = 0; i < m_vDirectors.Count; ++i)
            {
                if (!m_vDirectors[i].isOver()) return true;
            }
            return false;
        }
        //---------------------------------------------
        [Framework.Plugin.AT.ATMethod("是否播放路径动画")]
        public bool IsPlaying(int pathID = -1)
        {
            if (m_vDirectors == null) return false;
            if (pathID <= 0) return IsPlaying();
            for (int i = 0; i < m_vDirectors.Count; ++i)
            {
                if (m_vDirectors[i].GetGuid() == pathID)
                {
                    return !m_vDirectors[i].isOver();
                }
            }
            return false;
        }
        //------------------------------------------------------
        public float GetPlayingTimeLineRemainingTime()
        {
            float time = 0;
            if (m_vDirectors == null || m_vDirectors.Count == 0)
            {
                return time;
            }
            
            for (int i = 0; i < m_vDirectors.Count; i++)
            {
                if (m_vDirectors[i].GetDuration() > time)
                {
                    time = m_vDirectors[i].GetDuration();
                }
            }
            return time;
        }
        //---------------------------------------------
        void Recycle(IPlayableBase pAble)
        {
            if(m_vRecyles == null) m_vRecyles = new List<IPlayableBase>(4);
            if (m_vRecyles.Count >= 4) return;
            pAble.Destroy();
            m_vRecyles.Add(pAble);
        }
        //---------------------------------------------
        T PopNewPlayAble<T>() where T : IPlayableBase
        {
            if (m_vRecyles == null) m_vRecyles = new List<IPlayableBase>(4);
            for (int i = 0; i < m_vRecyles.Count; ++i)
            {
                if(m_vRecyles[i].GetType() == typeof(T))
                {
                    T able = (T)m_vRecyles[i];
                    able.SetEndCallbacked(false);
                    m_vRecyles.RemoveAt(i);
                    return able;
                }
            }

            IPlayableBase nullAble = null;
            return (T)nullAble;
        }
        //---------------------------------------------
        public void Update(float fFrametime)
        {
            if (m_vDirectors == null || m_vDirectors.Count<=0) return;
            Core.TouchInput input = m_pFramework.Get<Core.TouchInput>();
            if (m_bPauseAniPath)
            {
                if (input != null) input.ReleaseTouchCnt();
                return;
            }

            for (int i = 0; i < m_vDirectors.Count; )
            {
                IPlayableBase able = m_vDirectors[i];
                if (!able.Update(fFrametime))
                {
                    m_vDirectors.RemoveAt(i);
                    if (!able.IsEndCallbacked())
                    {
                        able.SetEndCallbacked(true);
                        for (int j = 0; j < m_vCallback.Count; ++j)
                        {
                            m_vCallback[j].OnAnimPathEnd(able);
                        }
                        Framework.Plugin.Guide.GuideWrapper.OnCustomCallback((int)Framework.Plugin.Guide.EGuideCustomType.AnimPathCallback, able.GetGuid());
                    }

                    Recycle(able);
                }
                else
                {
                    if(!able.IsEndCallbacked())
                    {
                        if (able.isOver())
                        {
                            able.SetEndCallbacked(true);
                            for (int j = 0; j < m_vCallback.Count; ++j)
                            {
                                m_vCallback[j].OnAnimPathEnd(able);
                            }
                            Framework.Plugin.Guide.GuideWrapper.OnCustomCallback((int)Framework.Plugin.Guide.EGuideCustomType.AnimPathCallback, able.GetGuid());
                        }
                    }

                    for (int j = 0; j < m_vCallback.Count; ++j)
                    {
                        m_vCallback[j].OnAnimPathUpdate(able);
                    }
                    ++i;
                }
            }
        }
    }
}