using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TopGame.Core;
using TopGame.Data;
using TopGame.ED;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class BattleServerConfigBuilder
    {
        private static string SVR_PROJECT_PATH = "/../../Server/work_spaces/BattleServer/";
        private static string SVR_BIN_PATH = "/../../Server/server_bin/";
        //------------------------------------------------------
        [MenuItem("Tools/战斗服配置数据同步", true)]
        public static bool BuilderCheck()
        {
            string destDir = Application.dataPath + SVR_BIN_PATH;
            return Directory.Exists(destDir);
        }
        //------------------------------------------------------
        [MenuItem("Tools/战斗服配置数据同步")]
        static void Builder()
        {
            string dataPath = Application.dataPath + SVR_BIN_PATH + "datas/";
            EditorUtility.DisplayProgressBar("", "", 0);
            {
                Dictionary<string, string> vSOPaths = new Dictionary<string, string>();
                //  List<string> vTextPaths = new List<string>();

                foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    System.Type[] types = ass.GetTypes();
                    for (int i = 0; i < types.Length; ++i)
                    {
                        if (types[i].IsDefined(typeof(ExternEngine.ConfigPathAttribute), false))
                        {
                            ExternEngine.ConfigPathAttribute attr = types[i].GetCustomAttribute<ExternEngine.ConfigPathAttribute>(false);
                            if (attr.bUseServer)
                            {
                                if (types[i].IsSubclassOf(typeof(ScriptableObject)))
                                {
                                    string[] assets = AssetDatabase.FindAssets("t:" + types[i].Name);
                                    if (assets != null && assets.Length > 0)
                                        vSOPaths[AssetDatabase.GUIDToAssetPath(assets[0])] = attr.strPath;
                                }
                            }
                        }

                    }
                }
                float prossgress = 0;
                foreach (var db in vSOPaths)
                {
                    ScriptableObject setting = AssetDatabase.LoadAssetAtPath<ScriptableObject>(db.Key);
                    File.WriteAllText(dataPath + db.Value, JsonUtility.ToJson(setting, true));
                    EditorUtility.DisplayProgressBar("ScriptObjects数据", db.Key, prossgress / (float)vSOPaths.Count);
                    prossgress++;
                }
            }

            EditorUtility.DisplayProgressBar("Csv数据", "Csv 数据代码自动生成", 0);
            CsvBuilderTool.BuildBattleServerColde(dataPath, Directory.Exists(Application.dataPath + SVR_PROJECT_PATH), true);

            //! ai datas
            EditorUtility.DisplayProgressBar("AI 数据", "AI 数据", 0);
            {
                Framework.Plugin.AI.AIDatas.Refresh();
                    string[] assets = AssetDatabase.FindAssets("t:AIDatas");
                if (assets != null && assets.Length > 0)
                {
                    Framework.Plugin.AI.AIServerDatas serverDatas = new Framework.Plugin.AI.AIServerDatas();
                    var aiData = AssetDatabase.LoadAssetAtPath<Framework.Plugin.AI.AIDatas>(AssetDatabase.GUIDToAssetPath(assets[0]));
                    if (aiData != null)
                    {
                        aiData.Init();
                        List<Framework.Plugin.AI.AIData> vdatas = new List<Framework.Plugin.AI.AIData>();
                        serverDatas.globalVariables = new List<Framework.Plugin.AI.AIVariable>(aiData.globalVariables.ToArray());
                        foreach (var db in aiData.allDatas)
                        {
                            vdatas.Add(db.Value);
                        }
                        serverDatas.datas = vdatas.ToArray();
                        File.WriteAllText(dataPath + "sos/AIDatas.json", JsonUtility.ToJson(serverDatas, true));
                    }
                }
            }

            //! pvp config
            UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/PVP_battle.unity");
            if (scene.IsValid())
            {
                var objs = scene.GetRootGameObjects();
                for (int i = 0; i < objs.Length; ++i)
                {
                    Logic.PVPConfig pvpConfig = objs[i].GetComponentInChildren<Logic.PVPConfig>();
                    if (pvpConfig != null && pvpConfig.GetType().IsDefined(typeof(ExternEngine.ConfigPathAttribute), false))
                    {
                        ExternEngine.ConfigPathAttribute attr = pvpConfig.GetType().GetCustomAttribute<ExternEngine.ConfigPathAttribute>(false);
                        if (attr.bUseServer)
                        {
                            ExternEngine.MonoScriptMeta mono = new ExternEngine.MonoScriptMeta();
                            mono.position = pvpConfig.transform.position;
                            mono.rotation = pvpConfig.transform.rotation;
                            mono.scale = pvpConfig.transform.lossyScale;
                            mono.mono = JsonUtility.ToJson(pvpConfig, true);
                            File.WriteAllText(dataPath + attr.strPath, JsonUtility.ToJson(mono, true));
                        }
                        break;
                    }
                }
            }

            EditorUtility.DisplayProgressBar("动作脚本", "动作脚本数据", 0);
            {
                string[] assets = AssetDatabase.FindAssets("t:ActionGraphAssets");
                if (assets != null && assets.Length > 0)
                {
                    Framework.Core.ActionGraphAssetsEditor.RefreshSync(AssetDatabase.LoadAssetAtPath<Framework.Core.ActionGraphAssets>(AssetDatabase.GUIDToAssetPath(assets[0])));
                }
            }

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("提示", "数据同步成功", "OK");
        }
    }
}