#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace TopGame.Core.Brush
{
    public class BrushRreferences
    {
        private static Dictionary<string, Settings> settings = new Dictionary<string, Settings>();
        private static string lastKey = "Brush.Settings";

        [System.Serializable]
        public class Settings : ISerializationCallbackReceiver
        {
            public Color guiBrushColor = Color.white;
            public Color guiBrushDelColor = new Color(1f, 0f, 0f);
            public Vector2 brushBeginAngle = new Vector2(0,360);
            public Vector2 brushScale = new Vector2(1,1);
            public float brsuhFlow = 100;
            public float brushSpacing = 0.5f;
            public string brushDataFile="";
            public float dummyBrushSize = 1;

            public float guiBrushThickness = 4;
            public int guiBrushNumCorners = 32;

            public List<int> brushMaskTextures = new List<int>();
            public void OnAfterDeserialize()
            {

            }

            public void OnBeforeSerialize()
            {

            }
        }

        /// <summary> Get settings of current active editor </summary>
        public static Settings GetSettings()
        {
            if (!settings.ContainsKey(lastKey)) VerifyLoaded();
            return settings[lastKey];
        }

#if UNITY_2019_1_OR_NEWER
        [SettingsProvider]
        public static SettingsProvider CreateNodeSettingsProvider() {
            SettingsProvider provider = new SettingsProvider("Preferences/BrushSetting", SettingsScope.User) {
                guiHandler = (searchContext) => { PreferencesGUI(); },
            };
            return provider;
        }
#endif

#if !UNITY_2019_1_OR_NEWER
        [PreferenceItem("AT Editor")]
#endif
        private static void PreferencesGUI()
        {
            VerifyLoaded();
            Settings settings = BrushRreferences.settings[lastKey];

            SystemSettingsGUI(lastKey, settings);
            if (GUILayout.Button(new GUIContent("Set Default", "Reset all values to default"), GUILayout.Width(120)))
            {
                ResetPrefs();
            }
        }
        
        private static void SystemSettingsGUI(string key, Settings settings)
        {
            //Label
            EditorGUILayout.LabelField("System", EditorStyles.boldLabel);
			settings.brushSpacing = EditorGUILayout.Slider("画笔间隔", settings.brushSpacing, 0, 1);
            settings.brushBeginAngle.x = EditorGUILayout.Slider("随机朝向-Min", settings.brushBeginAngle.x, 0,360);
            settings.brushBeginAngle.y = EditorGUILayout.Slider("随机朝向-Max", settings.brushBeginAngle.y, 0, 360);
            settings.brushScale.x = EditorGUILayout.Slider("随机缩放-Min", settings.brushScale.x, 0.1f, 5f);
            settings.brushScale.y = EditorGUILayout.Slider("随机缩放-Max", settings.brushScale.y, 0.1f, 5f);
            settings.guiBrushColor = EditorGUILayout.ColorField("画笔颜色", settings.guiBrushColor);
            settings.guiBrushDelColor = EditorGUILayout.ColorField("移除画显示笔颜色", settings.guiBrushDelColor);
            settings.dummyBrushSize = EditorGUILayout.Slider("dummy画笔区域半径", settings.dummyBrushSize, 0.1f, 10f);
            //    settings.guiBrushThickness = EditorGUILayout.Slider("画笔粗细", settings.guiBrushThickness, 1, 50);
            TerrainFoliageDatas brushDatas = AssetDatabase.LoadAssetAtPath<TerrainFoliageDatas>(settings.brushDataFile);
            brushDatas = EditorGUILayout.ObjectField("缺省植被材质", brushDatas, typeof(TerrainFoliageDatas), true) as TerrainFoliageDatas;
            if (brushDatas != null)
                settings.brushDataFile = AssetDatabase.GetAssetPath(brushDatas);

            for (int i = 0; i < settings.brushMaskTextures.Count; ++i)
            {
                string path = AssetDatabase.GetAssetPath(settings.brushMaskTextures[i]);
                Texture2D prefab =  AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                prefab = EditorGUILayout.ObjectField("Mask[" + i + "]", prefab, typeof(Texture2D), false) as Texture2D;
                if (prefab != null)
                    settings.brushMaskTextures[i] = prefab.GetInstanceID();
            }
            if(GUILayout.Button("添加Mask"))
            {
                settings.brushMaskTextures.Add(0);
            }
            if (GUI.changed)
            {
                TerrainBrushEditor.RefreshBrush();
                SavePrefs(key, settings);
            }
            EditorGUILayout.Space();
        }
        
        /// <summary> Load prefs if they exist. Create if they don't </summary>
        private static Settings LoadPrefs()
        {
            // Create settings if it doesn't exist
            if (!EditorPrefs.HasKey(lastKey))
            {
                EditorPrefs.SetString(lastKey, JsonUtility.ToJson(new Settings()));
            }
            return JsonUtility.FromJson<Settings>(EditorPrefs.GetString(lastKey));
        }

        /// <summary> Delete all prefs </summary>
        public static void ResetPrefs()
        {
            if (EditorPrefs.HasKey(lastKey)) EditorPrefs.DeleteKey(lastKey);
            if (settings.ContainsKey(lastKey)) settings.Remove(lastKey);
            VerifyLoaded();
        }

        /// <summary> Save preferences in EditorPrefs </summary>
        private static void SavePrefs(string key, Settings settings)
        {
            EditorPrefs.SetString(key, JsonUtility.ToJson(settings));
        }

        /// <summary> Check if we have loaded settings for given key. If not, load them </summary>
        private static void VerifyLoaded()
        {
            if (!settings.ContainsKey(lastKey)) settings.Add(lastKey, LoadPrefs());
        }
    }
}
#endif