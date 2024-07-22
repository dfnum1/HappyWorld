using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using TopGame.Core;
using TopGame.ED;
using Framework.ED;
using Framework.Core;

namespace TopGame.Timeline
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(EventPlayableAsset), true)]		
    public class EventPlayableAssetEditor : Editor
    {
        private EEventType m_AddType;
        private List<string> m_vPopSlots = new List<string>();
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

            EventPlayableAsset asset = target as EventPlayableAsset;
            List<BaseEventParameter> vEvents = asset.GetEvents();
            GUILayout.BeginHorizontal();
            m_AddType = EventPopDatas.DrawEventPop(m_AddType, "添加事件");
            if(GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(80) }))
            {
                BaseEventParameter pEvent = BuildEventUtl.BuildEventByType(null, m_AddType);
                if (pEvent != null)
                    vEvents.Add(pEvent);
            }
            GUILayout.EndHorizontal();

            m_vPopSlots.Clear();
            m_vPopSlots.Add("None");
            m_vPopSlots.Add("Root");
            if (asset.pParentSlot != null)
            {
                TransformRef[] slots = asset.pParentSlot.GetComponentsInChildren<TransformRef>();
                for (int i = 0; i < slots.Length; ++i)
                {
                    if(string.IsNullOrEmpty(slots[i].strSymbole)) continue;
                    m_vPopSlots.Add(slots[i].strSymbole);
                }
            }

            Color color = GUI.color;
            for (int i=0;i < vEvents.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                GUI.color = Color.green;
                GUILayout.Box(vEvents[i].GetEventType().ToString());
                GUI.color = color;
                if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    vEvents.RemoveAt(i);
                    break;
                }
                if (GUILayout.Button("复制"))
                {
                    DrawEventCore.AddCopyEvent( vEvents[i]);
                }
                if (DrawEventCore.CanCopyEvent(vEvents[i]) && GUILayout.Button("黏贴"))
                {
                    DrawEventCore.CopyEvent(vEvents[i]);
                }
                GUILayout.EndHorizontal();
                vEvents[i] = DrawEventCore.DrawUnAction(vEvents[i], m_vPopSlots);
            }

            asset.vEvents.Clear();
            for (int i = 0; i < vEvents.Count; ++i)
            {
                asset.vEvents.Add(vEvents[i].ToString() );
            }

            serializedObject.ApplyModifiedProperties();
            if(GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
		}
    }
}