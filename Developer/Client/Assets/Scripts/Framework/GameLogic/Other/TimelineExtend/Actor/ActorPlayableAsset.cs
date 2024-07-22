using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
namespace TopGame.Timeline
{
    public class ActorPlayableAsset : PlayableAsset
    {
        [System.NonSerialized]
        ActorTrackAsset m_pTrackAssets = null;
        ActorPlayableBehavior m_pBehaviour = null;
        public List<string> vParams = new List<string>();
        public string displayName="";
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ActorPlayableBehavior>.Create(graph);
            m_pBehaviour = playable.GetBehaviour();
#if UNITY_EDITOR
            vParamEditors.Clear();
#endif
            BasePlayableParam[] vInsParams = new BasePlayableParam[vParams.Count];
            for (int i = 0; i < vParams.Count; ++i)
            {
                BasePlayableParam pParam = PlayableParamUtl.NewParam(vParams[i]);
                if (pParam == null) continue;
                vInsParams[i] = pParam;
#if UNITY_EDITOR
                vParamEditors.Add(pParam);
#endif
            }
            m_pBehaviour.SetParams(vInsParams);

            if (m_pBehaviour != null) m_pBehaviour.SetTrackAsset(m_pTrackAssets);

            return playable;
        }
        //------------------------------------------------------
        public void Reset(ActorTrackAsset pTrackAssets)
        {
            m_pTrackAssets = pTrackAssets;
            if(m_pBehaviour!=null) m_pBehaviour.SetTrackAsset(m_pTrackAssets);
        }
        //------------------------------------------------------
#if UNITY_EDITOR
        [System.NonSerialized]
        List<BasePlayableParam> vParamEditors = new List<BasePlayableParam>();
        public List<BasePlayableParam> GetParams()
        {
            return vParamEditors;
        }
#endif
    }
}