using Framework.Core;
using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
namespace TopGame.Timeline
{
    public class EventPlayableAsset : PlayableAsset
    {
        public List<string> vEvents= new List<string>();
#if UNITY_EDITOR

        [System.NonSerialized]
        public Transform pParentSlot = null;
        [System.NonSerialized]
        public List<BaseEventParameter> vEventHandles = null;
#endif

        [System.NonSerialized]
        public EventTrackAsset pTrackAssets = null;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<EventPlayableBehavior>.Create(graph);

#if UNITY_EDITOR
            vEventHandles = new List<BaseEventParameter>();
            pParentSlot = null;
#endif
            var behavor = playable.GetBehaviour();
            bool bClear = true;
            for(int i = 0; i < vEvents.Count; ++i)
            {
                BaseEventParameter pEvent = BaseEventParameter.NewEvent(Framework.Module.ModuleManager.mainModule, vEvents[i]);
                if (pEvent == null) continue;
                behavor.AddEvent(pEvent, bClear);
#if UNITY_EDITOR
                vEventHandles.Add(pEvent);
#endif
                bClear = false;
            }
            if (pTrackAssets != null)
            {
                behavor.SetBinder(pTrackAssets.GetBinder());
                behavor.SetTrackAsset(pTrackAssets);
            }
#if UNITY_EDITOR
            if(owner != null)
                pParentSlot = owner.transform;
#endif
            return playable;
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        public List<BaseEventParameter> GetEvents()
        {
            return vEventHandles;
        }
#endif
    }
}