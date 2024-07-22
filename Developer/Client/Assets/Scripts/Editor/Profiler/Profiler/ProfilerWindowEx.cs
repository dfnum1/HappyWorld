using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace TopGame.ED
{
    public class ProfilerWindowEx : EditorWindow
    {
        enum ProfilerDataType
        {
            Other,
            Assets,
            NotSaved,
            BuiltinResources,
            SceneMemory,
            All,
        }
        ProfilerDataType m_DataType = ProfilerDataType.All;
        UnityEditor.IMGUI.Controls.TreeViewState m_TreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_AssetListMCHState;
        TreeAssetView m_pTreeView = null;
        private float m_memorySize = 0;
        private int m_memoryDepth = 100;
        private ProfilerMemoryElement m_memoryElementRoot;

        GUIStyle m_TitleStyle;
        GUIStyle m_TitleGridStyle;
        GUIStyle m_FoldoutStyle;
        private GUIContent m_ContentText = new GUIContent();

        int TexHeightGap = 20;
        int TileTab = 20;
        int TopGap = 60;
        int BoardGap = 5;
        int ExpandGap = 10;

        Texture2D RedBG;
        Texture2D BlueBG;
        Texture2D CyanBG;
        public Texture2D BG = null;
        int ScrollHeight = 0;
        int ExpandHeight = 0;

        Vector2 m_AssetScrollPos = Vector2.zero;

        public HashSet<string> vChecked = new HashSet<string>();
        public static ProfilerWindowEx Instance;

        public class AssetRefData : TreeAssetView.ItemData
        {
            public bool bExpand=false;
            public long memortSize  = 0;
            public int repaceCnt = 0;
            public int refCnt=0;
            public List<int> referencedBy = new List<int>();
            public HashSet<string> referencedStrBy = new HashSet<string>();
            public HashSet<string> referencedClassBy = new HashSet<string>();
        }
        Dictionary<int, AssetRefData> m_vRefDatas = new Dictionary<int, AssetRefData>();

        int m_ShowPage = 0;
        public string m_filter = "";
        bool m_bAllAssets = false;
        class MemoryInfo
        {
            public long memory = 0;
            public long lastMemory = 0;
            public string tag = "";
            public string ToString()
            {
                if(lastMemory< memory)
                    return tag + ": " + EditorUtility.FormatBytes(memory) + " +" +EditorUtility.FormatBytes(memory- lastMemory);
                else if (lastMemory == memory)
                    return tag + ": " + EditorUtility.FormatBytes(memory);
                else
                    return tag + ": " + EditorUtility.FormatBytes(memory) + " -" + EditorUtility.FormatBytes(lastMemory-memory);
            }
            public Color GetColor()
            {
                if (lastMemory < memory) return Color.red;
                return Color.white;
            }
        }
        MemoryInfo[] m_nCurMemorys = new MemoryInfo[(int)(ProfilerDataType.All+1)]; 
        //--------------------------------------------------
        [MenuItem("Tools/Profiler/资源数据分析器")]
        public static void ShowWindowProfiler()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            EditorApplication.ExecuteMenuItem("Window/Analysis/Profiler");
            if (Instance == null)
            {
                Instance = CreateInstance<ProfilerWindowEx>();
            }
            Instance.titleContent = new GUIContent("资源数据分析器");
            Instance.Show();
        }
        //--------------------------------------------------
        void InitProfile()
        {
            m_vRefDatas.Clear();
            MultiColumnHeaderState.Column[] colums =
                {
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                };
            colums[0].headerContent = new GUIContent("资源ID", "");
            colums[0].minWidth = 100;
            colums[0].width = 100;
            colums[0].maxWidth = 100;
            colums[0].headerTextAlignment = TextAlignment.Left;
            colums[0].canSort = true;
            colums[0].autoResize = true;
            colums[1].headerContent = new GUIContent("引用次数", "");
            colums[1].minWidth = 80;
            colums[1].width = 80;
            colums[1].maxWidth = 80;
            colums[1].headerTextAlignment = TextAlignment.Center;
            colums[1].canSort = true;
            colums[1].autoResize = false;
            m_AssetListMCHState = new MultiColumnHeaderState(colums);

            m_TreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
            m_pTreeView = new TreeAssetView(m_TreeState, m_AssetListMCHState);
            m_pTreeView.Reload();
        }
        //--------------------------------------------------
        private void OnEnable()
        {
            Instance = this;
            InitProfile();
            RedBG = new Texture2D(1, 1);
            RedBG.SetPixel(0, 0, Color.red);
            RedBG.Apply();
            RedBG.hideFlags = HideFlags.HideAndDontSave;

            BlueBG = new Texture2D(1, 1);
            BlueBG.SetPixel(0, 0, Color.blue);
            BlueBG.Apply();
            BlueBG.hideFlags = HideFlags.HideAndDontSave;

            CyanBG = new Texture2D(1, 1);
            CyanBG.SetPixel(0, 0, new Color(29f / 255f, 169f / 255f, 54f / 255f, 1f));
            CyanBG.Apply();
            CyanBG.hideFlags = HideFlags.HideAndDontSave;

            BG = new Texture2D(1, 1);
            BG.SetPixel(0, 0, new Color(0.25f, 0.25f, 0.25f, 1f));
            BG.Apply();
            BG.hideFlags = HideFlags.HideAndDontSave;

            if (m_FoldoutStyle == null)
            {
                m_FoldoutStyle = new GUIStyle();
                m_FoldoutStyle.fixedWidth = ExpandGap;
                m_FoldoutStyle.stretchWidth = true;
                m_FoldoutStyle.fontSize = 15;
                m_FoldoutStyle.fixedHeight = TexHeightGap;
                m_FoldoutStyle.alignment = TextAnchor.MiddleCenter;
                m_FoldoutStyle.normal.textColor = Color.yellow;
            }
            if (m_TitleStyle == null)
            {
                m_TitleStyle = new GUIStyle();
                //    m_TitleStyle.fixedWidth = 250;
                m_TitleStyle.stretchWidth = true;
                m_TitleStyle.fontSize = 15;
                m_TitleStyle.fixedHeight = TexHeightGap;
                m_TitleStyle.alignment = TextAnchor.MiddleCenter;
                m_TitleStyle.normal.textColor = Color.yellow;
            }
            if (m_TitleGridStyle == null)
            {
                m_TitleGridStyle = new GUIStyle();
                //    m_TitleStyle.fixedWidth = 250;
                m_TitleGridStyle.stretchWidth = true;
                m_TitleGridStyle.fontSize = 15;
                m_TitleGridStyle.fixedHeight = TexHeightGap;
                m_TitleGridStyle.alignment = TextAnchor.MiddleLeft;
                m_TitleGridStyle.normal.textColor = Color.white;
            }


            string[] tags = new string[] { "Other", "Assets", "Not Saved", "Builtin Resources", "Scene Memory", "All", "Res_Textures", "Res_Materials" };
            for(int i = 0; i < m_nCurMemorys.Length; ++i)
            {
                m_nCurMemorys[i] = new MemoryInfo();
                m_nCurMemorys[i].tag = tags[i];
            }
        }
        //-----------------------------------------------------
        bool isFilter(string name)
        {
            if (m_filter.Length > 0)
            {
                return !name.Contains(m_filter);
            }
            return false;
        }
        //--------------------------------------------------
        private void OnGUI()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Height(TopGap) });
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Height(TopGap) });
                GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(120), GUILayout.Height(TopGap) });
                m_DataType = (ProfilerDataType)EditorGUILayout.EnumPopup(m_DataType, new GUILayoutOption[] { GUILayout.Width(200) });
                m_memoryDepth = EditorGUILayout.IntField("深度", m_memoryDepth, new GUILayoutOption[] { GUILayout.Width(200) });
                m_memorySize = EditorGUILayout.FloatField("内存大小", m_memorySize, new GUILayoutOption[] { GUILayout.Width(200) });
                GUILayout.EndVertical();
                if (GUILayout.Button("分析", new GUILayoutOption[] { GUILayout.Width(80), GUILayout.Height(TopGap) }))
                {
                    TakeSample(m_DataType);
                    if (m_DataType == ProfilerDataType.Assets) ExportData(m_memorySize, m_memoryDepth, "Assets");
                    else if (m_DataType == ProfilerDataType.BuiltinResources) ExportData(m_memorySize, m_memoryDepth, "Builtin Resources");
                    else if (m_DataType == ProfilerDataType.NotSaved) ExportData(m_memorySize, m_memoryDepth, "Not Saved");
                    else if (m_DataType == ProfilerDataType.Other) ExportData(m_memorySize, m_memoryDepth, "Other");
                    else if (m_DataType == ProfilerDataType.SceneMemory) ExportData(m_memorySize, m_memoryDepth, "Scene Memory");
                    else ExportData(m_memorySize, m_memoryDepth);
                }
                GUILayout.Space(10);
                if(!m_bAllAssets)
                {
                    if (GUILayout.Button("看全部", new GUILayoutOption[] { GUILayout.Width(80), GUILayout.Height(TopGap) }))
                    {
                        m_bAllAssets = true;
                    }
                }
                else
                {
                    if (GUILayout.Button("只看新增", new GUILayoutOption[] { GUILayout.Width(80), GUILayout.Height(TopGap) }))
                    {
                        m_bAllAssets = false;
                    }
                }

                GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Height(TopGap) });
                DrawMemoryGUI(m_nCurMemorys[0], 120, TopGap / 2);
                DrawMemoryGUI(m_nCurMemorys[1], 120, TopGap / 2);
                GUILayout.EndVertical();

                GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Height(TopGap) });
                DrawMemoryGUI(m_nCurMemorys[2], 120, TopGap / 2);
                DrawMemoryGUI(m_nCurMemorys[3], 120, TopGap / 2);
                GUILayout.EndVertical();

                GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Height(TopGap) });
                DrawMemoryGUI(m_nCurMemorys[4], 120, TopGap / 2);
                DrawMemoryGUI(m_nCurMemorys[5], 120, TopGap / 2);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }            
            GUILayout.EndHorizontal();

            if (m_pTreeView != null)
            {
          //      EditorGUILayout.LabelField("数量:" + m_pTreeView.assetCount.ToString());
                m_pTreeView.searchString = EditorGUILayout.TextField("搜索", m_pTreeView.searchString, new GUILayoutOption[] { GUILayout.Width(position.width - 60), GUILayout.Height(20) });
                m_pTreeView.OnGUI(new Rect(0, TopGap + 60, position.width, position.height - TopGap-70));
            }
        }
        //--------------------------------------------------
        void DrawMemoryGUI(MemoryInfo info, float gapWidth, float Gap)
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(gapWidth), GUILayout.Height(Gap) });

            Color color = GUI.color;
            GUI.color = info.GetColor();
            EditorGUILayout.LabelField(info.ToString(), new GUILayoutOption[] { GUILayout.Width(300) });
            GUI.color = color;
            GUILayout.EndHorizontal();
        }
        //--------------------------------------------------
        private void ExportData(float memSize, int memDepth, string FilterName = "", Type filerType = null)
        {
            var filterSize = memSize * 1024 * 1024;
            ProfilerMemoryElement memoryElementRoot = ProfilerHelper.GetMemoryDetailRoot(memDepth, filterSize);

            if (null != memoryElementRoot)
            {
                Dictionary<string, string> vAbMaping = new Dictionary<string, string>();
                IEnumerable<AssetBundle> bundles = AssetBundle.GetAllLoadedAssetBundles();
                foreach (var db in bundles)
                {
                    string[] assets = db.GetAllAssetNames();
                    for (int k = 0; k < assets.Length; ++k)
                        vAbMaping.Add(assets[k], db.name);
                }

                foreach (var memoryElement in memoryElementRoot.children)
                {
                    if (null != memoryElement)
                    {
                        BuildTreeView("", memoryElement, m_vRefDatas, filerType);
                    }
                }

                long total = 0;
                foreach (var memoryElement in memoryElementRoot.children)
                {
                    total += memoryElement.totalMemory;
                }
                for (int i = 0; i < m_nCurMemorys.Length-1; ++i)
                {
                    foreach (var memoryElement in memoryElementRoot.children)
                    {
                        if (null != memoryElement && memoryElement.name.CompareTo(m_nCurMemorys[i].tag) == 0)
                        {
                            m_nCurMemorys[i].lastMemory = m_nCurMemorys[i].memory;
                            m_nCurMemorys[i].memory = memoryElement.totalMemory;
                            break;
                        }
                    }
                }

                m_nCurMemorys[m_nCurMemorys.Length - 1].memory = m_nCurMemorys[m_nCurMemorys.Length - 1].lastMemory;
                m_nCurMemorys[m_nCurMemorys.Length - 1].lastMemory = total;
            }

            m_pTreeView.BeginTreeData();
            foreach (var db in m_vRefDatas)
            {
                m_pTreeView.AddData(db.Value);
            }
            m_pTreeView.EndTreeData();
        }
        //--------------------------------------------------
        void BuildTreeView(string parentName, ProfilerMemoryElement ele, Dictionary<int, AssetRefData> vAssets, System.Type filerType = null)
        {
            if (ele == null) return;
            {
                string path = "";
                bool bFind = filerType == null;
                if (ele.memoryInfo != null)
                {
                    path = UnityEditor.AssetDatabase.GetAssetPath(ele.memoryInfo.instanceId);
                    if (filerType != null)
                    {

                        UnityEngine.Object retObj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                        if (retObj != null)
                        {
                            bFind = bCommonType(retObj.GetType(), filerType);
                        }
                        else
                            bFind = true;
                    }
                    else
                    {
                        bFind = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) != null;
                    }
                }
                if (bFind && ele.memoryInfo.instanceId!=0)
                {
                    AssetRefData asset;
                    if(vAssets.TryGetValue(ele.memoryInfo.instanceId, out asset))
                    {
                        asset.repaceCnt++;
                        return;
                    }
                    asset = new AssetRefData();
                    if (parentName.Length > 0)
                        asset.name = parentName + "/" + ele.name;
                    else
                        asset.name = ele.name;
                    asset.path = path;
                    asset.repaceCnt = 1;
                    asset.id = ele.memoryInfo.instanceId;
                    asset.memortSize = ele.memoryInfo.memorySize;
                    asset.referencedBy = ele.memoryInfo.referencedBy;
                    asset.referencedStrBy = ele.memoryInfo.referencedStrBy;
                    asset.referencedClassBy = ele.memoryInfo.referencedClassBy;
                    asset.refCnt = ele.memoryInfo.referencedBy.Count + ele.memoryInfo.referencedStrBy.Count + ele.memoryInfo.referencedClassBy.Count;
                    vAssets[asset.id] = asset;
                }

            }
            //    if(ele.children.Count>0)
            {
                if (parentName.Length > 0)
                    parentName = parentName + "/" + ele.name;
                else
                    parentName = ele.name;
            }

            foreach (var memoryElement in ele.children)
            {
                if (null != memoryElement)
                {
                    BuildTreeView(parentName, memoryElement, vAssets, filerType);
                }
            }
        }
        //--------------------------------------------------
        private static void ExtractMemory(float memSize, int memDepth, string FilterName="")
        {
            var filterSize = memSize * 1024 * 1024;
            var parent = Directory.GetParent(Application.dataPath);
            var outputPath = string.Format("{0}/profiler/MemoryDetailed{1:yyyy_MM_dd_HH_mm_ss}.txt", parent.FullName, DateTime.Now);
            File.Create(outputPath).Dispose();
            ProfilerMemoryElement memoryElementRoot = ProfilerHelper.GetMemoryDetailRoot(memDepth, filterSize);

            if (null != memoryElementRoot)
            {
                var writer = new StreamWriter(outputPath);
                writer.WriteLine("Memory Size: >= {0}MB", memSize);
                writer.WriteLine("Memory Depth: {0}", memDepth);
                writer.WriteLine("Current Target: {0}", ProfilerDriver.GetConnectionIdentifier(ProfilerDriver.connectedProfiler));
                writer.WriteLine("**********************");
                if (FilterName.Length > 0)
                {
                    foreach (var memoryElement in memoryElementRoot.children)
                    {
                        if (null != memoryElement && memoryElement.name.CompareTo(FilterName)==0)
                        {
                            ProfilerHelper.WriteMemoryDetail(writer, memoryElement);
                            break;
                        }
                    }
                }
                else
                {
                    ProfilerHelper.WriteMemoryDetail(writer, memoryElementRoot);
                }
                writer.Flush();
                writer.Close();
            }

            Process.Start(outputPath);
        }
        //--------------------------------------------------
        private static void ExtractRefrences(float memSize, int memDepth, string FilterName = "")
        {
            var filterSize = memSize * 1024 * 1024;
            var parent = Directory.GetParent(Application.dataPath);
            var outputPath = string.Format("asset_unab_ref.xml", parent.FullName, DateTime.Now);
            File.Create(outputPath).Dispose();
            ProfilerMemoryElement memoryElementRoot = ProfilerHelper.GetMemoryDetailRoot(memDepth, filterSize);

            if (null != memoryElementRoot)
            {
                var writer = new StreamWriter(outputPath);
                if(FilterName.Length>0)
                {
                    foreach (var memoryElement in memoryElementRoot.children)
                    {
                        if (null != memoryElement && memoryElement.name.CompareTo(FilterName) == 0)
                        {
                            ProfilerHelper.WriteReferenceDetail(writer, memoryElement);
                            break;
                        }
                    }
                }
                else
                {
                    ProfilerHelper.WriteReferenceDetail(writer, memoryElementRoot);
                }
                writer.Flush();
                writer.Close();
            }

            Process.Start(outputPath);
        }
        //--------------------------------------------------
        private static void TakeSample(ProfilerDataType DataType)
        {
            ProfilerHelper.RefreshMemoryData();
        }
        //--------------------------------------------------
        bool bCommonType(System.Type obj, System.Type filerType)
        {
            if (obj == null) return false;
            if (obj == filerType)
            {
                return true;
            }
            if (obj.BaseType != null)
            {
                if (bCommonType(obj.BaseType, filerType))
                    return true;
            }
            return false;
        }
    }
}