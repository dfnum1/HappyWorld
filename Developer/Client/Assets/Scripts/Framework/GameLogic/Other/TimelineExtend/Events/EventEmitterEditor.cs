#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace TopGame.Timeline
{
    [CustomTimelineEditor(typeof(EventEmitter))]
    class SignalEmitterEditor : MarkerEditor
    {
      //  static readonly string MissingAssetError = LocalizationDatabase.GetLocalizedString("No signal assigned");

        public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
        {
            var options = base.GetMarkerOptions(marker);
            EventEmitter emitter = (EventEmitter)marker;

            if (emitter.parent != null && emitter.parent is IUserTrackAsset)
            {
                IUserTrackAsset asset = emitter.parent as IUserTrackAsset;
                if (asset.GetBinder())
                {
               //     EventEmitterReciver reciver = asset.GetBinder().GetComponent<EventEmitterReciver>();
                //    if (reciver == null) reciver = asset.GetBinder().gameObject.AddComponent<EventEmitterReciver>();
                }
            }

            return options;
        }
    }
}
#endif