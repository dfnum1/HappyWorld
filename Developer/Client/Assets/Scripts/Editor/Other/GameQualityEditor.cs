using Framework.ED;
using System.Collections.Generic;
using TopGame.ED;
using UnityEditor;
using UnityEngine;

namespace TopGame.Data
{
    [CustomEditor(typeof(GameQuality))]
    [CanEditMultipleObjects]
    public class GameQualityEditor : Editor
    {
        List<string> vIngoreList = new List<string>();
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GameQuality assets = target as GameQuality;
            if (assets.Configs == null || assets.Configs.Length != (int)EGameQulity.None)
            {
                List<QualityConfig> vAssets = assets.Configs!=null?new List<QualityConfig>(assets.Configs):new List<QualityConfig>();
                for (int i = vAssets.Count; i < (int)EGameQulity.None; ++i)
                    vAssets.Add(QualityConfig.DEFAULT);
                assets.Configs = vAssets.ToArray();
            }
            assets.gameQulity = (EGameQulity)HandleUtilityWrapper.PopEnum("品质等级", assets.gameQulity);

            if(assets.gameQulity >= EGameQulity.Low && assets.gameQulity < EGameQulity.None)
            {
                QualityConfig config = assets.Configs[(int)assets.gameQulity];
                config.nQualityLevel = EditorGUILayout.Popup("渲染等级", config.nQualityLevel, QualitySettings.names);

                vIngoreList.Clear();
                if (UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset!=null)
                {
                    vIngoreList.Add("eShadowQuality");
                    vIngoreList.Add("ShadowLevel");
                    vIngoreList.Add("ShadowDistance");
                    vIngoreList.Add("AntiAliasing");
                    vIngoreList.Add("HDR");
                }
                config = (QualityConfig)HandleUtilityWrapper.DrawProperty(config, vIngoreList);
                assets.Configs[(int)assets.gameQulity] = config;
            }

            if(serializedObject.ApplyModifiedProperties())
                EditorUtility.SetDirty(target);
            if (GUILayout.Button("刷新保存"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }

        }
    }
}