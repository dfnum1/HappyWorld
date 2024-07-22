/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	Timeline
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TopGame.Core
{
    [Framework.Plugin.AT.ATExportMono("路径动画系统/Timeline")]
    public class TimelineData : IPlayableBase
    {
        public int guid = 0;
        public string AssetName = "";

        public Transform trigger = null;
        public bool enableEvent = true;
        public bool bMirror = false;
        public List<BaseEventParameter> events = null;
        public bool bCloseGameCamera = false;
        public ushort bUseEnd = 0;
        public Camera camera = null;
        public Transform target = null;
        public Transform follow = null;
        public AInstanceAble instance = null;
        public PlayableDirector m_director = null;
        public Cinemachine.CinemachineBrain cinemachine = null;
        private bool m_bEnable = false;

        RectTransform m_RectTrans = null;
        public PlayableDirector director
        {
            get
            {
                return m_director;
            }
            set
            {
                m_director = value;
                if (m_director != null)
                    m_director.stopped += OnStopTimeline;
            }
        }

        protected Transform[] m_arrSlots = null;
        protected List<PlayableBindSlot> m_TackSlots = null;
        public void SetSlots(Transform[] slots)
        {
            m_arrSlots = slots;
            if (slots == null || slots.Length <= 0) return;
            if(m_TackSlots == null) m_TackSlots = new List<PlayableBindSlot>(slots.Length);
            m_TackSlots.Clear();
            for (int i = 0; i < slots.Length; ++i)
            {
                if (slots[i] == null) continue;
                PlayableBindSlot bindSlot = new PlayableBindSlot();
                bindSlot.pSlot = slots[i];
                bindSlot.strName = slots[i].name;
                bindSlot.pAble = null;
                bindSlot.pUserAT = null;
                bindSlot.source = null;
                bindSlot.bGenericBinding = false;
                m_TackSlots.Add(bindSlot);
            }
        }


        Vector3 m_OffsetEulerAngle = Vector3.zero;
        Vector3 m_OffsetPosition = Vector3.zero;

        protected bool m_bCanSkip = false;
        protected float m_fSkipTime = 0;

        private List<IPlayableCallback> m_vCallback;
        protected bool m_bEndCallbacked = false;
        public bool IsEndCallbacked() { return m_bEndCallbacked; }
        public void SetEndCallbacked(bool bEndCallbacked) { m_bEndCallbacked = bEndCallbacked; }

        public int GetGuid()
        {
            return guid;
        }

        public void SetSkipTitle(bool bCanSkip, float skipTime)
        {
            m_bCanSkip = bCanSkip;
            m_fSkipTime = skipTime;
        }

        public bool CanSkip()
        {
            return m_bCanSkip;
        }

        public float GetPlayTime()
        {
            if (director == null) return 0;
            return (float)director.time;
        }

        public float GetDuration()
        {
            if (director == null) return 0;
            return (float)director.duration;
        }
        public float GetCanSkipTime()
        {
            return m_fSkipTime;
        }

        public string GetAssetName()
        {
            return AssetName;
        }
        public void SetAssetName(string strAssetName)
        {
            AssetName = strAssetName;
        }

        public ushort getUseEnd()
        {
            return bUseEnd;
        }
        public bool isOver()
        {
            if (!m_bEnable) return false;
            return director == null || director.time >= director.duration;
        }
        public void EnableEvent(bool bEnable)
        {
            enableEvent = bEnable;
        }
        public void Enable(bool bEnable)
        {
            m_bEnable = bEnable;
        }
        public List<BaseEventParameter> GetEvents()
        {
            return events;
        }
        public Transform GetTarget()
        {
            return target;
        }
        public void SetTarget(Transform pTransform)
        {
            target = pTransform;
        }
        public Transform GetTrigger()
        {
            return trigger;
        }
        public void SetTrigger(Transform pTrans)
        {
            trigger = pTrans;
        }
        public void Register(IPlayableCallback callback)
        {
            if (m_vCallback == null) m_vCallback = new List<IPlayableCallback>(2);
            if (!m_vCallback.Contains(callback))
                m_vCallback.Add(callback);
        }
        public void UnRegister(IPlayableCallback callback)
        {
            if (m_vCallback == null) return;
            m_vCallback.Remove(callback);
        }
        public void SetOffsetPosition(Vector3 pos)
        {
            m_OffsetPosition = pos;
            if (instance != null) instance.transform.position += pos;
        }
        public void SetOffsetEulerAngle(Vector3 eulerAngle)
        {
            m_OffsetEulerAngle = eulerAngle;
            if (instance != null) instance.transform.eulerAngles += eulerAngle;
        }

        public Vector3 GetOffsetPosition()
        {
            return m_OffsetPosition;
        }
        public Vector3 GetOffsetEulerAngle()
        {
            return m_OffsetEulerAngle;
        }

        [Framework.Plugin.AT.ATMethod("当前位置(GetPosition)")]
        public Vector3 GetPosition()
        {
            if (follow == null || target == null) return Vector3.zero;
            if (follow) return follow.position;
            return target.position;
        }
        [Framework.Plugin.AT.ATMethod("当前角度(GetEulerAngle)")]
        public Vector3 GetEulerAngle()
        {
            if (follow == null || target == null) return Vector3.zero;
            if (follow) return follow.eulerAngles;
            return target.eulerAngles;
        }
        [Framework.Plugin.AT.ATMethod("当前视点(GetLookAt)")]
        public Vector3 GetLookAt()
        {
            return Vector3.zero;
        }
        [Framework.Plugin.AT.ATMethod("当前广角(GetFov)")]
        public float GetFov()
        {
            if (camera == null) return 45f;
            return camera.fieldOfView;
        }

        [Framework.Plugin.AT.ATMethod("获取相机")]
        public Camera GetCamera()
        {
            return camera;
        }
        [Framework.Plugin.AT.ATMethod("获取绑点")]
        public Transform GetSlot(int index)
        {
            if (index < 0 || m_arrSlots == null || index >= m_arrSlots.Length) return null;
            return m_arrSlots[index];
        }

        [Framework.Plugin.AT.ATMethod("获取绑点-Name")]
        public Transform GetSlot(string strName)
        {
            if (string.IsNullOrEmpty(strName) || m_TackSlots == null) return null;
            PlayableBindSlot pSlot = PlayableBindSlot.DEFAULT;
            for(int i = 0; i < m_TackSlots.Count;++i)
            {
                if (m_TackSlots[i].strName.CompareTo(strName) == 0) return m_TackSlots[i].pSlot;
            }
            return null;
        }

        public KeyFrame GetLastKey() { return KeyFrame.Epsilon; }
        public void Stop()
        {
            if (director != null)
            {
                if (enableEvent && events != null)
                {
                    AFrameworkModule framework = Framework.Module.ModuleManager.GetMainFramework<AFrameworkModule>();
                    if(framework!=null)
                    {
                        for (int j = 0; j < events.Count; ++j)
                        {
                            //if ((events[j].Event.triggerRate & ((int)ERateBit.AnimpathStopSkipEvent)) != 0)
                            //    continue;

                            framework.OnTriggerEvent(events[j]);
                        }
                    }

                    events.Clear();
                }

                if (m_TackSlots != null)
                {
                    foreach (var db in m_TackSlots)
                    {
                        if(db.binding.sourceObject ==null ) continue;
                        director.SetGenericBinding(db.binding.sourceObject, db.source);
                    }
                    m_TackSlots.Clear();
                }

                if (bCloseGameCamera)
                {
                    CameraKit.ActiveRoot(true);
                }
                bCloseGameCamera = false;
                if (m_vCallback != null)
                {
                    for (int i = 0; i < m_vCallback.Count;)
                    {
                        if (m_vCallback[i].OnTimelineCallback(this, EPlayableCallbackType.End, new Variable3())) ++i;
                        else m_vCallback.RemoveAt(i);
                    }
                }
                director.Stop();

                m_director = null;
            }
            if (instance != null)
                instance.RecyleDestroy(1);
            instance = null;
            m_RectTrans = null;
            if (m_vCallback!=null) m_vCallback.Clear();
            m_arrSlots = null;

            SetSkipTitle(false, 0);
        }
        public void Pause()
        {
            if (director != null) director.Pause();
        }
        public void Resume()
        {
            if (director != null) director.Resume();
        }
        public void Destroy()
        {
            if (m_TackSlots != null)
            {
                if(director)
                {
                    foreach (var db in m_TackSlots)
                    {
                        if (db.binding.sourceObject == null) continue;
                        director.SetGenericBinding(db.binding.sourceObject, db.source);
                    }
                }

                m_TackSlots.Clear();
            }
            if(director)
                director.stopped -= OnStopTimeline;
            bMirror = false;
            m_bCanSkip = false;
            m_fSkipTime = 0;
            events = null;
            bCloseGameCamera = false;
            bUseEnd = 0;
            camera = null;
            director = null;
            trigger = null;
            follow = null;
            target = null;
            m_arrSlots = null;
            m_RectTrans = null;
            if (instance != null)
                instance.RecyleDestroy(1);
            instance = null;
            if (m_vCallback != null) m_vCallback.Clear();

            m_OffsetEulerAngle = Vector3.zero;
            m_OffsetPosition = Vector3.zero;

            AssetName = "";
            SetSkipTitle(false, 0);

            m_bEndCallbacked = false;
        }
        [Framework.Plugin.AT.ATMethod("跳指定位置开始播放(SkipTo)")]
        public void SkipTo(float fSkipTime)
        {
            if (director == null) return;
            if (fSkipTime < 0) fSkipTime = (float)director.duration - Time.deltaTime;
            float skip = Mathf.Clamp(fSkipTime, 0, (float)director.duration);
            director.time = skip;
            Update(0);
        }
        [Framework.Plugin.AT.ATMethod("跳位置播放(SkipDo)")]
        public void SkipDo()
        {
            if (director == null) return;
            if (m_bCanSkip)
                SkipTo(Mathf.Max(m_fSkipTime, (float)director.time));
        }
        public void SetCamera(Camera pCamea) { }
        public void SetMirror(bool bMirror) { this.bMirror = bMirror; }

        [Framework.Plugin.AT.ATMethod("绑定轨道对象数据")]
        public void BindTrackObject(string trackName, AInstanceAble pObj, bool bGenericBinding, Framework.Plugin.AT.IUserData pAT)
        {
            if (director == null || m_TackSlots == null) return;

            for(int i= 0; i < m_TackSlots.Count; ++i)
            {
                if(m_TackSlots[i].strName.CompareTo(trackName) == 0)
                {
                    PlayableBindSlot bindSlot = m_TackSlots[i];
                    bindSlot.pUserAT = pAT;
                    bindSlot.pAble = pObj;
                    bindSlot.bGenericBinding = bGenericBinding;
                    if(bindSlot.playableAsset != null)
                    {
                        bindSlot.playableAsset.SetUserPointer(pAT);
                    }
                    if (bGenericBinding)
                    {
                        if (bindSlot.binding.outputTargetType == typeof(Animator))
                        {
                            director.SetGenericBinding(bindSlot.binding.sourceObject, pObj.GetBehaviour<Animator>());
                        }
                        else if (bindSlot.binding.outputTargetType == typeof(GameObject))
                        {
                            director.SetGenericBinding(bindSlot.binding.sourceObject, pObj.gameObject);
                        }
                        else
                        {
                            director.SetGenericBinding(bindSlot.binding.sourceObject, pObj.gameObject);
                        }
                    }
                    m_TackSlots[i] = bindSlot;
                }
            }
        }

        public PlayableBindSlot GetPlayaleSlot(string strName)
        {
            if (m_TackSlots == null) return PlayableBindSlot.DEFAULT;
            for (int i = 0; i < m_TackSlots.Count; ++i)
            {
                if (m_TackSlots[i].strName.CompareTo(strName) == 0)
                    return m_TackSlots[i];
            }
            return PlayableBindSlot.DEFAULT;
        }

        public void InitTrackBinding()
        {
            if (m_TackSlots == null || m_TackSlots.Count<=0) return;
            foreach (var bind in director.playableAsset.outputs)
            {
                if (bind.sourceObject == null) continue;
                UnityEngine.Object source = director.GetGenericBinding(bind.sourceObject);
                if (source == null) continue;
                for (int i = 0; i < m_TackSlots.Count; ++i)
                {
                    if (m_TackSlots[i].strName.CompareTo(source.name) == 0)
                    {
                        PlayableBindSlot bindSlot = m_TackSlots[i];
                        bindSlot.bGenericBinding = false;
                        bindSlot.binding = bind;
                        bindSlot.source = source;
                        if (bind.sourceObject is Timeline.IUserTrackAsset)
                        {
                            bindSlot.playableAsset = bind.sourceObject as Timeline.IUserTrackAsset;
                            bindSlot.playableAsset.Reset(director);
                        }
                        m_TackSlots[i] = bindSlot;
                    }
                }
            }
        }


        public bool HasBinding(Framework.Plugin.AT.IUserData pAT, string strName = null)
        {
            if (m_TackSlots == null) return false;
            if (string.IsNullOrEmpty(strName))
            {
                for (int i = 0; i < m_TackSlots.Count; ++i)
                {
                    if (m_TackSlots[i].pUserAT == pAT) return true;
                }
                return false;
            }
            for (int i = 0; i < m_TackSlots.Count; ++i)
            {
                if (m_TackSlots[i].pUserAT == pAT && strName.CompareTo(m_TackSlots[i].strName) == 0) return true;
            }
            return false;
        }
        public bool HasBinding(AInstanceAble pAble, string strName = null)
        {
            if (m_TackSlots == null) return false;
            if (string.IsNullOrEmpty(strName))
            {
                for (int i = 0; i < m_TackSlots.Count; ++i)
                {
                    if (m_TackSlots[i].pAble == pAble) return true;
                }
                return false;
            }
            for (int i = 0; i < m_TackSlots.Count; ++i)
            {
                if (m_TackSlots[i].pAble == pAble && strName.CompareTo(m_TackSlots[i].strName)==0) return true;
            }
            return false;
        }

        public List<PlayableBindSlot> GetPlayableSlots()
        {
            return m_TackSlots;
        }

        void OnStopTimeline(PlayableDirector director)
        {
            if(m_director == director)
            {
                Stop();
            }
        }

        public void OnSpawnInstanceSign(InstanceOperiaon callback)
        {
            if (Framework.Module.ModuleManager.mainModule != null && GameInstance.getInstance().animPather != null)
            {
                callback.bUsed = GameInstance.getInstance().animPather.IsPlaying(guid);
            }
            else
                callback.bUsed = false;
        }

        public void OnSpawnInstanceCallback(InstanceOperiaon callback)
        {
            m_bEnable = true;
            if (callback.pPoolAble == null)
            {
                Stop();
                return;
            }
            GameObject pTimelineAsset = callback.pPoolAble.gameObject;

            PlayableDirector playableDirector = null;
            TimelineController controller =  callback.pPoolAble as TimelineController;
            if(controller!=null)
            {
                playableDirector = controller.playableDirector;
            }
            if(playableDirector == null)
                playableDirector = pTimelineAsset.GetComponent<PlayableDirector>();
            if (playableDirector == null)
            {
                callback.pPoolAble.RecyleDestroy(1);
                return;
            }
            director = playableDirector;
            if(controller)
            {
                follow = controller.follow;
                camera = controller.mainCamera;
                cinemachine = controller.cinemachine;
                SetSlots(controller.slots);
            }
            SetAssetName(pTimelineAsset.name);
            instance = callback.pPoolAble;

            instance.SetPosition(m_OffsetPosition);
            instance.SetEulerAngle(m_OffsetEulerAngle);
            if (bMirror)
                instance.SetScale(new Vector3(-1, 1, 1));
            else
                instance.SetScale(Vector3.one);

            m_RectTrans = instance.GetTransorm() as RectTransform;

            InitTrackBinding();

            if (director)
                director.Play();
        }

        public void TriggerEvent()
        {
            if (enableEvent && events != null && events.Count > 0)
            {
                float time = 0;
                if(director != null) time = (float)director.time;
                AFrameworkModule framework = Framework.Module.ModuleManager.GetMainFramework<AFrameworkModule>();
                if (framework != null)
                {
                    while (events.Count > 0 && time >= events[0].triggetTime)
                    {
                        framework.OnTriggerEvent(events[0]);
                        events.RemoveAt(0);
                    }
                }
            }
        }

        public bool Update(float fFrame)
        {
            if (!m_bEnable) return true;

            if (m_RectTrans)
            {
                m_RectTrans.localScale = Vector3.one;
                m_RectTrans.anchoredPosition3D = Vector2.zero;
                m_RectTrans.offsetMax = Vector3.zero;
                m_RectTrans.offsetMin = Vector3.zero;
                m_RectTrans.anchorMin = Vector2.zero;
                m_RectTrans.anchorMax = Vector2.one;
            }

            if (director == null)
            {
                Stop();
                return false;
            }
          //  Application.targetFrameRate = 60;// (int)(director.playableAsset as TimelineAsset).editorSettings.fps;

            if (director.time>0)
            {
                if (m_TackSlots != null)
                {
                    for (int i = 0; i < m_TackSlots.Count; ++i)
                    {
                        if (!m_TackSlots[i].bGenericBinding && m_TackSlots[i].pAble && m_TackSlots[i].pSlot)
                        {
                            m_TackSlots[i].pAble.SetPosition(m_TackSlots[i].pSlot.position);
                            m_TackSlots[i].pAble.SetEulerAngle(m_TackSlots[i].pSlot.eulerAngles);
                            m_TackSlots[i].pAble.SetScale(m_TackSlots[i].pSlot.lossyScale);
                        }
                    }
                }
                if(m_vCallback !=null)
                {
                    if (camera)
                    {
                        Transform cameraTrans = camera.transform;
                        for (int i = 0; i < m_vCallback.Count;)
                        {
                            bool bCall = true;
                            if (!m_vCallback[i].OnTimelineCallback(this, EPlayableCallbackType.Position, new Variable3(cameraTrans.position))) bCall = false;
                            if (!m_vCallback[i].OnTimelineCallback(this, EPlayableCallbackType.EuleraAngle, new Variable3(cameraTrans.eulerAngles))) bCall = false;
                            if (!m_vCallback[i].OnTimelineCallback(this, EPlayableCallbackType.FOV, new Variable3() { floatVal0 = camera.fieldOfView })) bCall = false;
                            if (follow != null)
                            {
                                if (!m_vCallback[i].OnTimelineCallback(this, EPlayableCallbackType.FollowPosition, new Variable3(follow.position))) bCall = false;
                            }
                            if (bCall) ++i;
                            else m_vCallback.RemoveAt(i);
                        }
                    }
                    else if (follow)
                    {
                        for (int i = 0; i < m_vCallback.Count;)
                        {
                            bool bCall = true;
                            if (!m_vCallback[i].OnTimelineCallback(this, EPlayableCallbackType.Position, new Variable3(follow.position))) bCall = false;
                            if (!m_vCallback[i].OnTimelineCallback(this, EPlayableCallbackType.EuleraAngle, new Variable3(follow.eulerAngles))) bCall = false;
                            if (camera)
                            {
                                if (!m_vCallback[i].OnTimelineCallback(this, EPlayableCallbackType.FOV, new Variable3() { floatVal0 = camera.fieldOfView })) bCall = false;
                            }
                            if (!m_vCallback[i].OnTimelineCallback(this, EPlayableCallbackType.FollowPosition, new Variable3(follow.position))) bCall = false;

                            if (bCall) ++i;
                            else m_vCallback.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < m_vCallback.Count;)
                    {
                        if (m_vCallback[i].OnTimelineCallback(this, EPlayableCallbackType.Tick, new Variable3())) ++i;
                        else m_vCallback.RemoveAt(i);
                    }
                }
                TriggerEvent();

//                 if (director.extrapolationMode == DirectorWrapMode.Hold && director.time >= director.duration)
//                     director.Stop();
            }
            return true;
        }
    }
}