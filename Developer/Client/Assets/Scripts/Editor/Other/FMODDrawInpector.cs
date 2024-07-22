//auto generator
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using TopGame.Data;
using TopGame.Core;
using Framework.Data;
using Framework.ED;
using Framework.Core;

namespace FMODUnity
{
    [Framework.Plugin.PluginExternAudio]
    public class FmodSoundEditorPlayer
    {
        public static void Play(string eventPath)
        {
#if USE_FMOD
            if (eventPath.StartsWith("event:/"))
            {
                FMODUnity.EditorEventRef eventRef = FMODUnity.EventCache.GetEventRef(eventPath);
                if (eventRef != null)
                {
                    FMODUnity.EditorUtils.LoadPreviewBanks();
                    FMODUnity.EditorUtils.PreviewEvent(eventRef, new Dictionary<string, float>());
                }
            }
#endif
        }
    }

    [Framework.Plugin.PluginInspector(typeof(FMODUnity.EventReference), "DrawInspector")]
	public class FMODDrawInpector
    {
        static string[] ms_EventPop = null;
        public static string DrawInspector(string strEvent, string strDisplayName)
        {
            if(ms_EventPop == null || ms_EventPop.Length != EventManager.Events.Count)
            {
                List<EditorEventRef> vEvents = EventManager.Events;
                ms_EventPop = new string[vEvents.Count];
                for (int i =0; i < vEvents.Count; ++i)
                {
                    ms_EventPop[i] = vEvents[i].Path;
                }
            }
            int index = -1;
            for (int i =0; i < ms_EventPop.Length; ++i)
            {
                if(ms_EventPop[i].CompareTo(strEvent) == 0)
                {
                    index = i;
                    break;
                }
            }
            
            EditorGUILayout.BeginHorizontal();
            strEvent = EditorGUILayout.TextField(strDisplayName, strEvent);
            index = EditorGUILayout.Popup(index, ms_EventPop, new GUILayoutOption[] {GUILayout.MaxWidth(120) });
            if (index >= 0 && index < ms_EventPop.Length)
                strEvent = ms_EventPop[index];
            else strEvent = "";
            EditorGUILayout.EndHorizontal();
            return strEvent;
        }
	}
}
