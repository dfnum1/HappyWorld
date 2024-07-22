using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using TopGame.Core;
using TopGame.ED;
using Framework.ED;

namespace TopGame.Timeline
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(ActorPlayableAsset), true)]		
    public class ActorPlayableAssetEditor : Editor
    {
        private EPlayableParamType m_AddType = EPlayableParamType.Count;
        private List<string> m_vPopSlots = new List<string>();
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

            ActorPlayableAsset asset = target as ActorPlayableAsset;
            List<BasePlayableParam> vEvents = asset.GetParams();
            asset.displayName = EditorGUILayout.TextField("描述", asset.displayName);
            GUILayout.BeginHorizontal();
            m_AddType = (EPlayableParamType)HandleUtilityWrapper.PopEnum("参数类型",m_AddType, typeof(EPlayableParamType));
            if(GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(80) }))
            {
                BasePlayableParam pEvent = PlayableParamUtl.NewParam(m_AddType);
                if (pEvent != null)
                    vEvents.Add(pEvent);
            }
            GUILayout.EndHorizontal();

            m_vPopSlots.Clear();
            m_vPopSlots.Add("None");
            m_vPopSlots.Add("Root");

            Color color = GUI.color;
            for (int i=0;i < vEvents.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                GUI.color = Color.green;
                if(vEvents[i]!=null)
                {
                    GUILayout.Box(vEvents[i].GetParamType().ToString());
                }
                GUI.color = color;
                if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    vEvents.RemoveAt(i);
                    break;
                }
                GUILayout.EndHorizontal();
                if (vEvents[i] != null)
                    vEvents[i] = (BasePlayableParam)HandleUtilityWrapper.DrawProperty(vEvents[i], null);
            }

            asset.vParams.Clear();
            for (int i = 0; i < vEvents.Count; ++i)
            {
                if (vEvents[i] == null) continue;
                asset.vParams.Add(vEvents[i].ToString() );
            }

            serializedObject.ApplyModifiedProperties();
		}
    }
}