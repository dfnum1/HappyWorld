using Framework.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using TopGame.Core;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace TopGame.ED
{
    public class PoolWindow : EditorWindow , IFileSystemCallBack
    {
        public static PoolWindow Instance;

        public class AssetItem : TreeAssetView.ItemData
        {
            public Asset asset;
            public bool bPermanent;

            public override Color itemColor()
            {
                if (asset.Status == Asset.EStatus.Failed)
                {
                    if (asset.IsValid()) return Color.yellow;
                    return Color.red;
                }
                if (asset.Status == Asset.EStatus.None)
                {
                    if (asset.IsValid()) return Color.yellow;
                    return Color.grey;
                }
                return Color.white;
            }
        }

        public class ChildItem : TreeAssetView.ItemData
        {
            public override Color itemColor()
            {
                return Color.cyan;
            }
        }
        public class BundleAssetItem : TreeAssetView.ItemData
        {
            public AssetBundleInfo asset;
            public bool bPermanent;

            public override Color itemColor()
            {
                if (asset.assetbundle == null) return Color.red;
                return Color.white;
            }
        }

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

        UnityEditor.IMGUI.Controls.TreeViewState m_TreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_AssetListMCHState;
        TreeAssetView m_pAssetTreeView = null;

        UnityEditor.IMGUI.Controls.TreeViewState m_AssetBundleTreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_AssetBundleListMCHState;
        TreeAssetView m_pAssetBundleTreeView = null;
        //--------------------------------------------------
        [MenuItem("Tools/Profiler/运行时资源分析器")]
        public static void ShowWindowProfiler()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            if (Instance == null)
            {
                Instance = CreateInstance<PoolWindow>();
            }
            Instance.titleContent = new GUIContent("运行时资源分析器");
            Instance.Show();
        }
        //--------------------------------------------------
        void InitProfile()
        {
            m_vRefDatas.Clear();
            {
                MultiColumnHeaderState.Column[] colums =
                {
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                };
                colums[0].headerContent = new GUIContent("资源", "");
                colums[0].minWidth = 100;
                colums[0].width = 100;
                colums[0].maxWidth = 100;
                colums[0].headerTextAlignment = TextAlignment.Left;
                colums[0].canSort = true;
                colums[0].autoResize = true;

                colums[1].headerContent = new GUIContent("引用Bundle", "");
                colums[1].minWidth = 80;
                colums[1].width = 80;
                colums[1].maxWidth = 80;
                colums[1].headerTextAlignment = TextAlignment.Center;
                colums[1].canSort = false;
                colums[1].autoResize = false;

                colums[2].headerContent = new GUIContent("引用次数", "");
                colums[2].minWidth = 80;
                colums[2].width = 80;
                colums[2].maxWidth = 80;
                colums[2].headerTextAlignment = TextAlignment.Center;
                colums[2].canSort = true;
                colums[2].autoResize = false;

                colums[3].headerContent = new GUIContent("Bundle引用次数", "");
                colums[3].minWidth = 160;
                colums[3].width = 160;
                colums[3].maxWidth = 160;
                colums[3].headerTextAlignment = TextAlignment.Center;
                colums[3].canSort = false;
                colums[3].autoResize = false;

                colums[4].headerContent = new GUIContent("状态", "");
                colums[4].minWidth = 80;
                colums[4].width = 80;
                colums[4].maxWidth = 80;
                colums[4].headerTextAlignment = TextAlignment.Center;
                colums[4].canSort = true;
                colums[4].autoResize = false;

                colums[5].headerContent = new GUIContent("常驻?", "");
                colums[5].minWidth = 80;
                colums[5].width = 80;
                colums[5].maxWidth = 80;
                colums[5].headerTextAlignment = TextAlignment.Center;
                colums[5].canSort = false;
                colums[5].autoResize = false;

                m_AssetListMCHState = new MultiColumnHeaderState(colums);

                m_TreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pAssetTreeView = new TreeAssetView(m_TreeState, m_AssetListMCHState);
                m_pAssetTreeView.OnCellDraw += OnDrawAssetGUI;
                m_pAssetTreeView.OnSortColum += OnAssetSort;
                m_pAssetTreeView.Reload();
            }


            
            {
                MultiColumnHeaderState.Column[] colums =
                {
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                     new MultiColumnHeaderState.Column(),
                };
                colums[0].headerContent = new GUIContent("Bundle资源", "");
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

                colums[2].headerContent = new GUIContent("状态", "");
                colums[2].minWidth = 80;
                colums[2].width = 80;
                colums[2].maxWidth = 80;
                colums[2].headerTextAlignment = TextAlignment.Center;
                colums[2].canSort = true;
                colums[2].autoResize = false;

                colums[3].headerContent = new GUIContent("常驻?", "");
                colums[3].minWidth = 80;
                colums[3].width = 80;
                colums[3].maxWidth = 80;
                colums[3].headerTextAlignment = TextAlignment.Center;
                colums[3].canSort = false;
                colums[3].autoResize = false;
                m_AssetBundleListMCHState = new MultiColumnHeaderState(colums);

                m_AssetBundleTreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pAssetBundleTreeView = new TreeAssetView(m_AssetBundleTreeState, m_AssetBundleListMCHState);
                m_pAssetBundleTreeView.OnCellDraw += OnDrawAssetBundleGUI;
                m_pAssetBundleTreeView.OnSortColum += OnAssetBundleSort;
                m_pAssetBundleTreeView.OnSelectChange += OnAssetBundleSelect;
                m_pAssetBundleTreeView.Reload();
            }
        }
        //--------------------------------------------------
        private void OnEnable()
        {
            Instance = this;
            InitProfile();
            if (FileSystemUtil.GetFileSystem()!=null)
                FileSystemUtil.GetFileSystem().RegisterCallback(this);
            RefreshAssets();
            RefreshBundleAssets();
        }
        //--------------------------------------------------
        private void OnDisable()
        {
            if(FileSystemUtil.GetFileSystem() != null)
                FileSystemUtil.GetFileSystem().UnRegisterCallback(this);
        }
        //--------------------------------------------------
        private void OnGUI()
        {
            bool hasAB = false;
            if (FileSystemUtil.GetStreamType() > EFileSystemType.AssetData)
                hasAB = true;
            Rect assetRect = new Rect(0,0, hasAB?(position.width/2): position.width, position.height);
            GUILayout.BeginArea(assetRect);
            if (m_pAssetTreeView != null)
            {
                float gap = 0;
                for (int i = 1; i < m_AssetListMCHState.columns.Length; ++i)
                    gap += m_AssetListMCHState.columns[i].width;
                m_AssetListMCHState.columns[0].width = assetRect.width - gap;
                m_pAssetTreeView.searchString = EditorGUILayout.TextField("搜索", m_pAssetTreeView.searchString, new GUILayoutOption[] { GUILayout.Width(assetRect.width - 60), GUILayout.Height(20) });
                m_pAssetTreeView.OnGUI(new Rect(0, 30, assetRect.width, assetRect.height-5));
            }
            GUILayout.EndArea();

            if(hasAB)
            {
                Rect bundleRect = new Rect(position.width / 2, 0, position.width / 2, position.height);
                GUILayout.BeginArea(bundleRect);
                if (m_pAssetBundleTreeView != null)
                {
                    float gap = 0;
                    for (int i = 1; i < m_AssetBundleListMCHState.columns.Length; ++i)
                        gap += m_AssetBundleListMCHState.columns[i].width;
                    m_AssetBundleListMCHState.columns[0].width = bundleRect.width - gap;
                    m_pAssetBundleTreeView.searchString = EditorGUILayout.TextField("搜索", m_pAssetBundleTreeView.searchString, new GUILayoutOption[] { GUILayout.Width(bundleRect.width - 60), GUILayout.Height(20) });
                    m_pAssetBundleTreeView.OnGUI(new Rect(0, 30, bundleRect.width, bundleRect.height - 5));
                }
                GUILayout.EndArea();
            }

        }
        //------------------------------------------------------
        List<TreeAssetView.TreeItemData> OnAssetSort(List<TreeAssetView.TreeItemData> vDatas, MultiColumnHeader header, int c)
        {
            bool ascending = header.IsSortedAscending(c);
            if (c == 0)
            {
                vDatas.Sort((TreeAssetView.TreeItemData l, TreeAssetView.TreeItemData r) => { return l.displayName.CompareTo(r.displayName) * (ascending ? 1 : -1); });
            }
            else if (c == 1)
            {
                vDatas.Sort((TreeAssetView.TreeItemData l, TreeAssetView.TreeItemData r) => { return (int)(((AssetItem)l.data).asset.RefCnt - ((AssetItem)r.data).asset.RefCnt) * (ascending ? 1 : -1); });
            }
            else if (c == 2)
            {
                vDatas.Sort((TreeAssetView.TreeItemData l, TreeAssetView.TreeItemData r) => { return (int)((((AssetItem)l.data).asset.Status) - (((AssetItem)r.data).asset.Status)) * (ascending ? 1 : -1); });
            }
            return vDatas;
        }
        //------------------------------------------------------
        bool OnDrawAssetGUI(Rect r, TreeAssetView.TreeItemData t, int c, bool s, bool f)
        {
            AssetItem item = t.data as AssetItem;
            if(c == 0)
            {
                GUI.Label(r, item.path);
            }
            else if (c == 1)
            {
                GUI.Label(r, item.asset.assetbundle!=null?System.IO.Path.GetFileNameWithoutExtension(item.asset.assetbundle.abName):"");
            }
            else if (c == 2)
            {
                GUI.Label(r, item.asset.RefCnt.ToString());
            }
            else if (c == 3)
            {
                GUI.Label(r, item.asset.assetbundle != null ? item.asset.assetbundle.refCnt.ToString() : "0");
            }
            else if (c == 4)
            {
                GUI.Label(r, item.asset.Status.ToString());
            }
            else if (c == 5)
            {
                GUI.Label(r, item.bPermanent.ToString());
            }
            return true;
        }
        //------------------------------------------------------
        void OnAssetBundleSelect(TreeAssetView.ItemData data)
        {

        }
        //------------------------------------------------------
        List<TreeAssetView.TreeItemData> OnAssetBundleSort(List<TreeAssetView.TreeItemData> vDatas, MultiColumnHeader header, int c)
        {
            bool ascending = header.IsSortedAscending(c);
            if (c == 0)
            {
                vDatas.Sort((TreeAssetView.TreeItemData l, TreeAssetView.TreeItemData r) => { return l.displayName.CompareTo(r.displayName) * (ascending ? 1 : -1); });
            }
            else if (c == 1)
            {
                vDatas.Sort((TreeAssetView.TreeItemData l, TreeAssetView.TreeItemData r) => { return (int)(((BundleAssetItem)l.data).asset.refCnt - ((BundleAssetItem)r.data).asset.refCnt) * (ascending ? 1 : -1); });
            }
            else if (c == 2)
            {
                vDatas.Sort((TreeAssetView.TreeItemData l, TreeAssetView.TreeItemData r) => { return (int)((((BundleAssetItem)l.data).asset.assetbundle!=null?1:0) - (((BundleAssetItem)r.data).asset.assetbundle!=null?1:0)) * (ascending ? 1 : -1); });
            }
            return vDatas;
        }
        //------------------------------------------------------
        bool OnDrawAssetBundleGUI(Rect r, TreeAssetView.TreeItemData t, int c, bool s, bool f)
        {
            if (t.data is BundleAssetItem)
            {
                BundleAssetItem item = t.data as BundleAssetItem;
                if (c == 0)
                {
                    if(item.asset.RefBy.Count>0)
                        GUI.Label(new Rect(r.x+10, r.y,r.width-10,r.height), item.asset.abName);
                    else
                        GUI.Label(r, item.asset.abName);
                }
                else if (c == 1)
                {
                    GUI.Label(r, item.asset.refCnt.ToString());
                }
                else if (c == 2)
                {
                    GUI.Label(r, item.asset.assetbundle != null ? "Loaded" : "none");
                }
                else if (c == 3)
                {
                    GUI.Label(r, item.bPermanent.ToString());
                }
            }
            else if (t.data is ChildItem)
            {
                ChildItem item = t.data as ChildItem;
                if (c == 0)
                {
                    GUI.Label(new Rect(r.x + 10, r.y, r.width - 10, r.height), item.path);
                }
            }
            return true;
        }
        //------------------------------------------------------
        void RefreshAssets()
        {
            if (FileSystemUtil.GetFileSystem() == null) return;
            if (m_pAssetTreeView!=null)
            {
                m_pAssetTreeView.BeginTreeData();

                int guid = 0;
                foreach (var db in FileSystemUtil.GetFileSystem().permanentResources)
                {
                    AssetItem item = new AssetItem();
                    item.asset = (Asset)db.Value;
                    item.id = guid++;
                    item.path = db.Key;
                    item.name = db.Key;
                    item.bPermanent = true;
                    m_pAssetTreeView.AddData(item);
                }

                foreach (var db in FileSystemUtil.GetFileSystem().resources)
                {
                    AssetItem item = new AssetItem();
                    item.asset = (Asset)db.Value;
                    item.id = guid++;
                    item.path = db.Key;
                    item.name = db.Key;
                    item.bPermanent = false;
                    m_pAssetTreeView.AddData(item);
                }

                m_pAssetTreeView.EndTreeData();
            }
        }

        //------------------------------------------------------
        void RefreshBundleAssets()
        {
            if (FileSystemUtil.GetFileSystem() == null) return;
            if (m_pAssetBundleTreeView != null)
            {
                m_pAssetBundleTreeView.BeginTreeData();
                m_pAssetBundleTreeView.buildMutiColumnDepth = true;
                int guid = 0;
                foreach (var db in FileSystemUtil.GetFileSystem().assetbunds)
                {
                    BundleAssetItem item = new BundleAssetItem();
                    item.asset = db.Value;
                    item.depth = 1;
                    item.id = guid++;
                    item.path = db.Key;
                    item.name = db.Key;
                    item.bPermanent = db.Value.permanent;
                    m_pAssetBundleTreeView.AddData(item);
                    for(int i = 0; i < db.Value.RefBy.Count; ++i)
                    {
                        ChildItem child = new ChildItem();
                        child.depth = 2;
                        child.id = guid++;
                        child.path = db.Value.RefBy[i];
                        child.name = db.Value.RefBy[i];
                        child.parent = item;
                        m_pAssetBundleTreeView.AddData(child);
                    }
                }

                m_pAssetBundleTreeView.EndTreeData();
            }
        }
        //------------------------------------------------------
        public void OnAssetCallback(Asset asset)
        {
            RefreshAssets();
            RefreshBundleAssets();
        }
        //------------------------------------------------------
        public void OnAssetGrab(Asset asset)
        {
            RefreshAssets();
            RefreshBundleAssets();
        }
        //------------------------------------------------------
        public void OnAssetRelease(Asset asset)
        {
            RefreshAssets();
            RefreshBundleAssets();
        }
        //------------------------------------------------------
        public void OnAssetDispose(Asset asset)
        {
            RefreshAssets();
            RefreshBundleAssets();
        }
    }
}