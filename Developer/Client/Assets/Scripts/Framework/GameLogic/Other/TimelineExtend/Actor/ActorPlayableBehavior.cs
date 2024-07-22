using Framework.Core;
using System;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace TopGame.Timeline
{
    public class ActorPlayableBehavior : PlayableBehaviour
    {
        struct UpdateAble
        {
            public AInstanceAble pAble;
            public Transform pSlot;
            public Vector3 offset;
            public Vector3 euler;
        }
        private bool m_bDestroyed = false;
        private Transform m_pBinder;
        private ActorTrackAsset m_pTrackAsset = null;
        private BasePlayableParam[] m_vParams = null;
        private List<UpdateAble> m_vUpdateLists = null;

        private float m_fFrameTime = 0;
        //------------------------------------------------------
        public void SetParams(BasePlayableParam[] Params)
        {
            if (Params == null) return;
            m_vParams = Params;
        }
        //------------------------------------------------------
        public ActorTrackAsset GetTrackAsset()
        {
            return m_pTrackAsset;
        }
        //------------------------------------------------------
        public void SetTrackAsset(ActorTrackAsset pPointer)
        {
            m_pTrackAsset = pPointer;
        }
        //------------------------------------------------------
        public void SetBinder(Transform pBinder)
        {
            m_pBinder = pBinder;
        }
        //------------------------------------------------------
        public void Destroy()
        {
            if (m_vUpdateLists != null)
            {
                for (int i = 0; i < m_vUpdateLists.Count; ++i)
                {
                    FileSystemUtil.DeSpawnInstance(m_vUpdateLists[i].pAble);
                }
                m_vUpdateLists.Clear();
            }
            m_bDestroyed = true;
        }
        //------------------------------------------------------
        public override void OnPlayableCreate(Playable playable)
        {
            base.OnPlayableCreate(playable);
            Destroy();
            m_fFrameTime = 0;
        }
        //------------------------------------------------------
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            Destroy();
            m_bDestroyed = false;
            base.OnBehaviourPlay(playable, info);
            m_fFrameTime = 0;
        }
        //------------------------------------------------------
        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
            if (m_vParams != null)
            {
                for (int i = 0; i < m_vParams.Length; ++i)
                {
                    if (m_vParams[i] != null) m_vParams[i].Destroy();
                }
            }
            Destroy();
     //       m_pTrackAsset = null;
        }
        //------------------------------------------------------
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
            //       m_pTrackAsset = null;
            if (m_vParams != null && m_pTrackAsset)
            {
                if (playable.GetTime() >= playable.GetDuration() - 0.03f)
                {
                    for (int i = 0; i < m_vParams.Length; ++i)
                    {
                        if (m_vParams[i] != null) m_vParams[i].OnStop(m_pTrackAsset);
                    }
                }
            }
        }
        //------------------------------------------------------
        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
            Destroy();
            if (m_vParams != null && m_pTrackAsset)
            {
                for (int i = 0; i < m_vParams.Length; ++i)
                {
                    if(m_vParams[i]!=null) m_vParams[i].OnStart(m_pTrackAsset);
                }
            }
        }
        //------------------------------------------------------
        public override void OnGraphStop(Playable playable)
        {
            base.OnGraphStop(playable);
            Destroy();
            if (m_vParams != null && m_pTrackAsset)
            {
                if(playable.GetTime() >= playable.GetDuration()-0.03f)
                {
                    for (int i = 0; i < m_vParams.Length; ++i)
                    {
                        if (m_vParams[i] != null) m_vParams[i].OnStop(m_pTrackAsset);
                    }
                }

            }
        }
        //------------------------------------------------------
        public void OnSpawnSign(InstanceOperiaon pCb)
        {
            pCb.bUsed = !m_bDestroyed;
        }
        //------------------------------------------------------
        public void OnSpawnBehaviour(InstanceOperiaon pCb)
        {
            if (pCb.pPoolAble == null) return;
            if (m_vUpdateLists == null) m_vUpdateLists = new List<UpdateAble>(2);
            UpdateAble able = new UpdateAble();


             Variable3 offsetPos = (Variable3)pCb.userData0;
            Variable3 offsetEuler = (Variable3)pCb.userData1;
            VariableString strSlot = (VariableString)pCb.userData2;

            able.euler = offsetEuler.ToVector3();
            able.offset = offsetPos.ToVector3();

            Transform pBind = m_pBinder;
            if (!string.IsNullOrEmpty(strSlot.strValue))
            {
                pBind = DyncmicTransformCollects.FindTransformByName(strSlot.strValue);
            }
            able.pSlot = pBind;
            able.pAble = pCb.pPoolAble;
            able.pAble.SetPosition(pBind!=null?(pBind.position+able.offset):able.offset);
            able.pAble.SetEulerAngle(pBind != null ? (pBind.eulerAngles + able.euler) : able.euler);
            m_vUpdateLists.Add(able);
        }
        //------------------------------------------------------
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            m_fFrameTime += info.deltaTime;
            if (m_pTrackAsset == null)
            {
                base.ProcessFrame(playable, info, playerData);
                return;
            }
            if (m_vParams != null && m_pTrackAsset)
            {
                if(m_pTrackAsset.GetBinder() == null)
                    m_pTrackAsset.SetBinder(playerData as Transform);

                bool bEditorSnap = false;
                if (m_pTrackAsset.GetDirector())
                {
                    bEditorSnap = m_pTrackAsset.GetDirector().state != PlayState.Playing;
                }
                for (int i = 0; i < m_vParams.Length; ++i)
                {
                    m_vParams[i].OnExcude(m_pTrackAsset, (float)playable.GetTime(), (float)playable.GetDuration(), bEditorSnap);
                }
            }

            if(m_vUpdateLists!=null)
            {
                for(int i = 0; i < m_vUpdateLists.Count; ++i)
                {
                    UpdateAble able = m_vUpdateLists[i];
                    if(able.pAble == null) continue;
                    able.pAble.SetPosition(able.offset);
                    able.pAble.SetEulerAngle(able.euler);

                    if (able.pSlot == null) continue;
                    able.pAble.SetPosition(able.pSlot.position+able.offset);
                    able.pAble.SetEulerAngle(able.pSlot.eulerAngles + able.euler);
                    able.pAble.SetScale(able.pSlot.lossyScale);
                }
            }
            base.ProcessFrame(playable, info, playerData);
        }
    }
}