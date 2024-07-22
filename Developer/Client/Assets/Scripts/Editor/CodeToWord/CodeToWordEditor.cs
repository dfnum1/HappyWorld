using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditorInternal;
using System.IO;
using NPOI.XWPF.UserModel;
// using NPOI.XWPF.Extractor;
// using NPOI;
// using NPOI.XWPF.UserModel;
namespace TopGame.ED
{
    public class CodeToWordEditor : EditorWindow
    {
        [System.Serializable]
        class CatchData
        {
            public string project="";
            public List<string> vFiles = new List<string>();

            [System.NonSerialized]
            public List<UnityEngine.Object> vObjFiles = new List<UnityEngine.Object>();
            [System.NonSerialized]
            public ReorderableList list;
        }
        [System.Serializable]
        class ProjectDatas
        {
            public List<CatchData> projects;
        }
        //------------------------------------------------------
        static CodeToWordEditor ms_Instance = null;
        [MenuItem("Tools/代码输出文档", false, 10)]
        static void Open()
        {
            if (ms_Instance == null)
            {
                ms_Instance = EditorWindow.GetWindow<CodeToWordEditor>();
            }
            ms_Instance.Show();
            ms_Instance.titleContent = new GUIContent("代码输出文档");
            ms_Instance.minSize = new Vector2(600, 600);
        }

        ProjectDatas m_Project = new ProjectDatas();

        int m_nSelectIndex = -1;
        List<string> m_vPop = new List<string>();
        HashSet<string> m_vCodeFiles = new HashSet<string>();
        string m_catchFile = "";
        //------------------------------------------------------
        private void OnEnable()
        {
            m_catchFile = Application.dataPath + "/../EditorData/CodeToWorldCatch.json";
            if(File.Exists(m_catchFile))
            {
                try
                {
                    m_Project = JsonUtility.FromJson<ProjectDatas>(File.ReadAllText(m_catchFile));
                }
                catch
                {
                    m_Project = new ProjectDatas();
                }
            }
            m_nSelectIndex = -1;
            m_vPop.Clear();
            for (int i =0; i < m_Project.projects.Count; ++i)
            {
                CatchData catchData = m_Project.projects[i];
                for (int j = 0; j < catchData.vFiles.Count; ++j)
                {
                    catchData.vObjFiles.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(catchData.vFiles[j]));
                    m_vCodeFiles.Add(catchData.vFiles[j]);
                }
                catchData.list = new ReorderableList(catchData.vObjFiles, typeof(UnityEngine.Object),true,true,true,true);
                catchData.list.drawElementCallback = DrawElement;
                m_vPop.Add(catchData.project);
            }
            Refresh();
        }
        //------------------------------------------------------
        void Refresh()
        {
            m_vPop.Clear();
            for (int i = 0; i < m_Project.projects.Count; ++i)
            {
                CatchData catchData = m_Project.projects[i];
                if (string.IsNullOrEmpty(catchData.project))
                    catchData.project = i.ToString();

                m_vPop.Add(catchData.project);
            }
        }
        //------------------------------------------------------
        void SaveCatch()
        {
            m_vCodeFiles.Clear();
            for (int i = 0; i < m_Project.projects.Count; ++i)
            {
                CatchData catchData = m_Project.projects[i];
                catchData.vFiles.Clear();
                for (int j = 0; j < catchData.vObjFiles.Count; ++j)
                {
                    if (catchData.vObjFiles[j])
                    {
                        string file = AssetDatabase.GetAssetPath(catchData.vObjFiles[j]);
                        m_vCodeFiles.Add(file);
                        catchData.vFiles.Add(file);
                    }
                }
            }

            File.WriteAllText(m_catchFile, JsonUtility.ToJson(m_Project, true));
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            SaveCatch();
            EditorUtility.ClearProgressBar();
        }
        //------------------------------------------------------
        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            m_nSelectIndex = EditorGUILayout.Popup("项目", m_nSelectIndex, m_vPop.ToArray());
            if(GUILayout.Button("新建", new GUILayoutOption[] { GUILayout.MaxWidth(50) }))
            {
                CatchData proj = new CatchData();
                proj.project = "TEST";
                proj.list = new ReorderableList(proj.vObjFiles, typeof(UnityEngine.Object), true, true, true, true);
                proj.list.drawElementCallback = DrawElement;
                m_Project.projects.Add(proj);
                m_nSelectIndex = m_Project.projects.Count - 1;
                Refresh();
            }
            GUILayout.EndHorizontal();
            if(m_nSelectIndex>=0 && m_nSelectIndex< m_Project.projects.Count)
            {
                var project = m_Project.projects[m_nSelectIndex];
                project.project = EditorGUILayout.TextField("名称", project.project);

                if(Event.current.type == EventType.Layout)
                {
                    if (project.list == null)
                    {
                        project.list = new ReorderableList(project.vObjFiles, typeof(UnityEngine.Object), true, true, true, true);
                        project.list.drawElementCallback = DrawElement;
                    }
                }

                if(project.list!=null) project.list.DoLayoutList();

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(project.vObjFiles.Count <= 0);
                if (GUILayout.Button("导出"))
                {
                    ExportDot(project);
                }
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("移除"))
                {
                    if(EditorUtility.DisplayDialog("提示", "确定移除?", "移除", "取消"))
                    {
                        m_Project.projects.RemoveAt(m_nSelectIndex);
                        m_nSelectIndex = -1;
                        Refresh();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        //------------------------------------------------------
        void ExportDot(CatchData catchData)
        {
            SaveCatch();
            string path = EditorUtility.SaveFilePanel("导出排版代码", Application.dataPath, catchData.project, "docx");
             XWPFDocument doc = new XWPFDocument();
             XWPFParagraph para = doc.CreateParagraph();
             XWPFRun run = para.CreateRun();
            int maxLine = 5000;// 3000;

            HashSet<string> vHasets = new HashSet<string>();
            List<FileInfo> vFiles = new List<FileInfo>();
            List<string> vTexts = new List<string>();
            int fileIndex = 1;
            for (int i = 0; i < catchData.vObjFiles.Count; ++i)
            {
                if (catchData.vObjFiles[i] == null) continue;
                string filePath = AssetDatabase.GetAssetPath(catchData.vObjFiles[i]);
                if (vHasets.Contains(filePath))
                    continue;
                vHasets.Add(filePath);
                if (catchData.vObjFiles[i] is MonoScript)
                {
                    vTexts.Add((fileIndex++) +".  " + System.IO.Path.GetFileNameWithoutExtension(filePath));
                    ReplaceTile(vTexts,File.ReadAllLines(filePath));
                    continue;
                }
                if (AssetDatabase.IsValidFolder(filePath))
                {
                    vFiles.Clear();
                    EditorKits.FindDirFiles(Application.dataPath + filePath.Replace("Assets/", "/"), vFiles);
                    for(int j =0; j < vFiles.Count; ++j)
                    {
                        if(vFiles[j].Extension.ToLower().CompareTo(".cs") != 0) continue;
                        if (vHasets.Contains(vFiles[j].FullName))
                            continue;
                        vHasets.Add(vFiles[j].FullName);
                        vTexts.Add((fileIndex++) + ".  " + System.IO.Path.GetFileNameWithoutExtension(vFiles[j].FullName));
                        ReplaceTile(vTexts,File.ReadAllLines(vFiles[j].FullName));
                        if (vTexts.Count >= maxLine) break;
                    }
                }
                if (vTexts.Count >= maxLine) break;
            }

            run.SetText("");
            for (int i = 0; i < vTexts.Count; ++i)
            {
                run.AppendText(vTexts[i]);
                run.AddCarriageReturn();
            }

              FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
              doc.Write(file);
              file.Close();
        }
        //------------------------------------------------------
        void ReplaceTile(List<string> vCodes, string[] text)
        {
            bool bHead = false;
            for(int i =0; i < text.Length; ++i)
            {
                if (bHead)
                {
                    if (text[i].Contains("**********************/"))
                    {
                        bHead = false;
                    }
                    continue;
                }
                if (text[i].Contains("/*********************"))
                {
                    bHead = true;
                    continue;
                }
               
                if (text[i].Contains("//--------")) continue;
                text[i] = text[i].Replace('\t', ' ');
                if (string.IsNullOrEmpty(text[i]))
                    continue;
                vCodes.Add(text[i]);
            }
        }
        //------------------------------------------------------
        void DrawElement(Rect rect, int index, bool selected, bool focused)
        {
            if (m_nSelectIndex >= 0 && m_nSelectIndex < m_Project.projects.Count)
            {
                var project = m_Project.projects[m_nSelectIndex];
                var preFile = project.vObjFiles[index];


                project.vObjFiles[index] = EditorGUI.ObjectField(rect, project.vObjFiles[index], typeof(UnityEngine.Object), true);
                string filePath = "";
                if (project.vObjFiles[index] != null) filePath = AssetDatabase.GetAssetPath(project.vObjFiles[index]);
                if (project.vObjFiles[index] != preFile)
                {
                    if (!string.IsNullOrEmpty(filePath) && m_vCodeFiles.Contains(filePath))
                    {
                        project.vObjFiles[index] = null;
                    }

                    if (project.vObjFiles[index]!= null) m_vCodeFiles.Add(AssetDatabase.GetAssetPath(project.vObjFiles[index]));
                    if (preFile != null) m_vCodeFiles.Remove(AssetDatabase.GetAssetPath(preFile));
                }
            }
        }
    }
}
