/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	基础播放类型
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core
{
    [Framework.Plugin.AT.ATExportMono("路径动画系统/动画剪辑")]
    public class AnimPathData : IPlayableBase
    {
        protected List<PlayableBindSlot> m_vBindingSlots = null;
        protected Transform[] m_arrSlots = null;
        protected Dictionary<string, Transform> m_Slots = null;
        public void SetSlots(Transform[] slots)
        {
            m_arrSlots = slots;
            if (slots == null || slots.Length <= 0) return;
            if (m_Slots == null) m_Slots = new Dictionary<string, Transform>(slots.Length);
            m_Slots.Clear();
            for (int i = 0; i < slots.Length; ++i)
            {
                if(slots[i] == null) continue;
                m_Slots[slots[i].name] = slots[i];
            }
        }

        public int guid = 0;
        public string AssetName = "";
        public bool bEnableEvent = true;
        public AnimationPathDirector director = null;

        protected bool m_bCanSkip = false;
        protected float m_fSkipTime = 0;

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
            return director.fTweenTime;
        }

        public float GetDuration()
        {
            if (director == null) return 0;
            return director.GetDuration();
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
            if (director != null) return director.bAnimPathUseEnd;
            return 0;
        }

        public bool isOver()
        {
            return director == null || !director.IsPlaying();
        }
        public void EnableEvent(bool bEnable)
        {
            bEnableEvent = bEnable;
        }

        public void TriggerEvent()
        {
            if (director != null) director.TriggerEvent();
        }
        public List<BaseEventParameter> GetEvents()
        {
            if (director != null) return director.GetEvents();
            return null;
        }
        public Transform GetTarget()
        {
            return director != null ? director.GetTarget() : null;
        }
        public void SetTarget(Transform pTransform)
        {
            if (director == null) return;
            director.SetTraget(pTransform);
        }
        public Transform GetTrigger()
        {
            if (director == null) return null;
            return director.GetTrigger();
        }
        public void SetTrigger(Transform pTrans)
        {
            if (director == null) return;
            director.SetTrigger(pTrans);
        }

        [Framework.Plugin.AT.ATMethod("获取相机")]
        public Camera GetCamera()
        {
            if (director == null) return null;
            return director.GetCamera();
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
            if (string.IsNullOrEmpty(strName) || m_Slots == null) return null;
            Transform pSlot = null;
            if (m_Slots.TryGetValue(strName, out pSlot)) return pSlot;
            return null;
        }

        [Framework.Plugin.AT.ATMethod("绑定轨道对象数据")]
        public void BindTrackObject(string trackName, AInstanceAble pObj, bool bGenericBinding, Framework.Plugin.AT.IUserData pAT = null)
        {
            Transform slot = GetSlot(trackName);
            if (slot == null) return;
            if (m_vBindingSlots == null) m_vBindingSlots = new List<PlayableBindSlot>();
            for (int i = 0; i < m_vBindingSlots.Count; ++i)
            {
                if (m_vBindingSlots[i].pAble == pObj)
                {
                    PlayableBindSlot bindSlot = m_vBindingSlots[i];
                    bindSlot.pSlot = slot;
                    bindSlot.pUserAT = pAT;
                    m_vBindingSlots[i] = bindSlot;
                    return;
                }
            }
            PlayableBindSlot newSlot = new PlayableBindSlot();
            newSlot.pSlot = slot;
            newSlot.pAble = pObj;
            newSlot.pUserAT = pAT;
            m_vBindingSlots.Add(newSlot);
            return;
        }
        public PlayableBindSlot GetPlayaleSlot(string strName)
        {
            if (m_vBindingSlots == null) return PlayableBindSlot.DEFAULT;
            for(int i =0; i < m_vBindingSlots.Count; ++i)
            {
                if(m_vBindingSlots[i].pSlot == null) continue;
                if(m_vBindingSlots[i].pSlot.name.CompareTo(strName) == 0)
                {
                    return m_vBindingSlots[i];
                }
            }
            return PlayableBindSlot.DEFAULT;
        }
        public bool HasBinding(Framework.Plugin.AT.IUserData pAT, string strName = null)
        {
            if (m_vBindingSlots == null) return false;
            if(!string.IsNullOrEmpty(strName))
            {
                for (int i = 0; i < m_vBindingSlots.Count; ++i)
                {
                    if (m_vBindingSlots[i].pSlot == null) continue;
                    if (m_vBindingSlots[i].pSlot.name.CompareTo(strName) == 0 && m_vBindingSlots[i].pUserAT == pAT)
                    {
                        return true;
                    }
                }
                return false;
            }
            for (int i = 0; i < m_vBindingSlots.Count; ++i)
            {
                if (m_vBindingSlots[i].pUserAT == pAT)
                {
                    return true;
                }
            }
            return false;
        }
        public bool HasBinding(AInstanceAble pAble, string strName = null)
        {
            if (m_vBindingSlots == null) return false;
            if (!string.IsNullOrEmpty(strName))
            {
                for (int i = 0; i < m_vBindingSlots.Count; ++i)
                {
                    if (m_vBindingSlots[i].pSlot == null) continue;
                    if (m_vBindingSlots[i].pSlot.name.CompareTo(strName) == 0 && m_vBindingSlots[i].pAble == pAble)
                    {
                        return true;
                    }
                }
                return false;
            }
            for (int i = 0; i < m_vBindingSlots.Count; ++i)
            {
                if (m_vBindingSlots[i].pAble == pAble)
                {
                    return true;
                }
            }
            return false;
        }

        public List<PlayableBindSlot> GetPlayableSlots()
        {
            return m_vBindingSlots;
        }

        public void Stop()
        {
            if (director != null) director.Stop();
            bEnableEvent = true;
            if (m_vBindingSlots != null) m_vBindingSlots.Clear();
            SetSkipTitle(false, 0);
        }
        public void Pause()
        {
            if (director != null) director.PausePlay(true);
        }
        public void Resume()
        {
            if (director != null) director.PausePlay(false);
        }
        public void Register(IPlayableCallback callback)
        {
            if (director != null) director.Register(callback);
        }
        public void UnRegister(IPlayableCallback callback)
        {
            if (director != null) director.UnRegister(callback);
        }

        public void SetOffsetPosition(Vector3 pos)
        {
            if (director != null) director.SetOffsetPosition(pos);
        }
        public void SetOffsetEulerAngle(Vector3 eulerAngle)
        {
            if (director != null) director.SetOffsetEulerAngle(eulerAngle);
        }
        public Vector3 GetOffsetPosition()
        {
            if (director != null) return director.GetOffsetPosition();
            return Vector3.zero;
        }
        public Vector3 GetOffsetEulerAngle()
        {
            if (director != null) director.GetOffsetEulerAngle();
            return Vector3.zero;
        }

        [Framework.Plugin.AT.ATMethod("当前位置(GetPosition)")]
        public Vector3 GetPosition()
        {
            if (director == null) return Vector3.zero;
            if (director.GetFollow()) return director.GetFollow().position;
            return director.curPosition;
        }
        [Framework.Plugin.AT.ATMethod("当前角度(GetEulerAngle)")]
        public Vector3 GetEulerAngle()
        {
            if (director == null) return Vector3.zero;
            if (director.GetFollow()) return director.GetFollow().eulerAngles;
            return director.curEulerAngle;
        }
        [Framework.Plugin.AT.ATMethod("当前视点(GetLookAt)")]
        public Vector3 GetLookAt()
        {
            if (director == null) return Vector3.zero;
            return director.curLookat;
        }
        [Framework.Plugin.AT.ATMethod("当前广角(GetFov)")]
        public float GetFov()
        {
            if (director == null) return 45f;
            return director.curFov;
        }
        public void Destroy()
        {
            AssetName = "";
            if (director!=null) director.Clear();
            bEnableEvent = true;
            if (m_vBindingSlots != null) m_vBindingSlots.Clear();
            SetSkipTitle(false, 0);
            m_bEndCallbacked = false;
        }
        [Framework.Plugin.AT.ATMethod("跳指定位置开始播放(SkipTo)")]
        public void SkipTo(float fSkipTime)
        {
            if (director == null) return;
            director.SkipTo(fSkipTime);
            Update(0);
        }
        [Framework.Plugin.AT.ATMethod("跳位置播放(SkipDo)")]
        public void SkipDo()
        {
            if (director == null) return;
            if (director.bCanSkip)
                SkipTo(Mathf.Max(director.fSkipToTime, director.fTweenTime));
        }
        public void SetCamera(Camera pCamea)
        {
            if (director == null) return;
            director.SetCamera(pCamea);
        }
        public void SetFollow(Transform pFollow)
        {
            if (director == null) return;
            director.SetFollow(pFollow);
        }
        public void SetMirror(bool bMirror)
        {
            if (director == null) return;
            director.bMirror = bMirror;
        }
        public KeyFrame GetLastKey()
        {
            if (director == null) return KeyFrame.Epsilon;
            return director.GetLasyKey();
        }
        public bool Update(float fFrame)
        {
            if (director == null)
            {
                Stop();
                return false;
            }
            else
            {
                if (m_vBindingSlots != null)
                {
                    for (int i = 0; i < m_vBindingSlots.Count; ++i)
                    {
                        if (m_vBindingSlots[i].pAble != null && m_vBindingSlots[i].pSlot)
                        {
                            m_vBindingSlots[i].pAble.SetPosition(m_vBindingSlots[i].pSlot.position);
                            m_vBindingSlots[i].pAble.SetEulerAngle(m_vBindingSlots[i].pSlot.eulerAngles);
                            m_vBindingSlots[i].pAble.SetScale(m_vBindingSlots[i].pSlot.lossyScale);
                        }
                    }
                }
                return director.ForceUpdate(fFrame, bEnableEvent);
            }
        }
    }
}