using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;
using UnityEngine.UI;
using static TopGame.ED.PublishPanel;
using System.Text;
using TopGame.UI;

namespace TopGame.ED
{
    public class GuideAnalysis : EditorWindow
    {
        [MenuItem("Tools/UI/新手引导组件统计")]
        private static void GuideAnalysisTools()
        {
            if (!EditorUtility.DisplayDialog("提示", "UI是否检查所有预置体及场景GuidGuid组件?", "确定", "取消"))
                return;

            string savePath = "";
            if (EditorUtility.DisplayDialog("提示", "请选择新手引导组件检测CSV导出路径", "确定", "取消"))
            {
                savePath = EditorUtility.SaveFilePanel("保存",Application.dataPath.Replace("Assets", "Publishs"), "", "csv");
                if (savePath == "") return;
            }

            PublishSetting m_BuildSetting = PublishPanel.LoadSetting();
            List<string> buildDirs = m_BuildSetting.buildDirs;
            List<string> unbuildDirs = m_BuildSetting.unbuildDirs;

            List<string> vFilesss = new List<string>();
            string RootParticlePath = "Assets/Datas/uis";

            List<string> withoutExtensions = new List<string>() { ".prefab" };
            vFilesss = new List<string>(Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray());

            RootParticlePath = "Assets/DatasRef/UI";
            vFilesss.AddRange(new List<string>(Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray()));

            withoutExtensions = new List<string>() { ".unity" };
            RootParticlePath = "Assets/DatasRef/UI/UIScenes";
            vFilesss.AddRange(new List<string>(Directory.GetFiles(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + RootParticlePath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray()));

            int startIndex = 0;

            List<string> lines = new List<string>();
            lines.Add("fullname,name,guid");

            EditorApplication.update = delegate ()
            {
                string file = vFilesss[startIndex];

                file = file.Substring(Application.dataPath.Length - "Assets".Length, file.Length - Application.dataPath.Length + "Assets".Length);

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("UI文件", file, (float)startIndex / (float)vFilesss.Count);

                if (file.Contains(".unity"))
                {
                    UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(file);
                    if (scene.IsValid())
                    {
                        for (int i = 0; i < scene.GetRootGameObjects().Length; ++i)
                        {
                            GameObject rootObject = scene.GetRootGameObjects()[i];
                            var guidguids = rootObject.GetComponentsInChildren<Framework.Plugin.Guide.GuideGuid>(true);
                            foreach (var guid in guidguids)
                            {
                                string getnodePath = "";
                                GetNodePath(guid.transform,ref getnodePath);
                                string name = guid.name;
                                string guidnum = guid.Guid.ToString();
                                string line = getnodePath + "," + name + "," + guidnum;
                                lines.Add(line);
                            }                                                    
                        }
                    }
                }

                startIndex++;
                if (isCancel || startIndex >= vFilesss.Count)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;
                    Debug.LogError(savePath);
                    WriteCsv(lines.ToArray(), savePath);
                }
            };
        }

        public static void WriteCsv(string[] strs, string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            //UTF-8方式保存
            using (StreamWriter stream = new StreamWriter(path, false, Encoding.UTF8))
            {
                for (int i = 0; i < strs.Length; i++)
                {
                    if (strs[i] != null)
                        stream.WriteLine(strs[i]);
                }
            }

            Debug.LogError("保存结束");
        }

        private static void GetNodePath(Transform trans, ref string path)
        {

            if (path == "")
            {
                if (trans.name != "Canvas" && trans.name != "UIRoot")
                    path = trans.name;
            }
            else
            {
                if (trans.name != "Canvas" && trans.name != "UIRoot")
                    path = trans.name + "/" + path;
            }

            if (trans.parent != null)
            {
                Canvas canvas = trans.parent.GetComponent<Canvas>();
                GraphicRaycaster caster = trans.parent.GetComponent<GraphicRaycaster>();
                UserInterface temper = trans.parent.GetComponent<UserInterface>();
                if (!(temper && caster && temper))
                    GetNodePath(trans.parent, ref path);
                else
                {
                    if(trans.parent.name != "Canvas" && trans.parent.name != "UIRoot")
                    path = trans.parent.name + "/" + path;
                }
            }

        }
    }


}