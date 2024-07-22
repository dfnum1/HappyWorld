/********************************************************************
生成日期:	25:7:2019   9:51
类    名: 	PackagePanel
作    者:	HappLI
描    述:	包体信息
*********************************************************************/

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEditor.Build;
using System.Text;
using TopGame.Core;
using UnityEditor.IMGUI.Controls;
using Excel;

namespace TopGame.ED
{
    public class PackagePanel
    {
        public enum EItemType
        {
            Root,
            File,
            Dir,
            EncrptyPak,
        }
        public class FileItem : TreeAssetView.ItemData
        {
            public EItemType itemType;
            public FileInfo fileInfo = null;
            public int fileCnt = 0;
            public long totalSize = 0;
            public string shortPath="";
            public string shortFile="";

            public System.IntPtr packageHandler = System.IntPtr.Zero;

            public int refCnt = 0;

            public AssetBundle assetBundle = null;

            public Color color = Color.white;
            public override Color itemColor()
            {
                return color;
            }

            public override Texture2D itemIcon()
            {
                if (itemType == EItemType.Dir)
                    return EditorGUIUtility.IconContent("Folder Icon").image as Texture2D;
                else if (itemType == EItemType.File)
                    return EditorGUIUtility.IconContent("DefaultAsset Icon").image as Texture2D;
                else if (itemType == EItemType.EncrptyPak)
                    return EditorGUIUtility.IconContent("DefaultAsset Icon").image as Texture2D;
                return null;
            }
        }
        public class FileList
        {
            public FileItem item = new FileItem();
            public List<FileList> childs = new List<FileList>();

            public FileList( string root, int depth=-1)
            {
                item.name = root;
                item.itemType = EItemType.Root;
                item.depth = depth;
            }
        }

        Vector2 m_ScrollPos = Vector2.zero;

        UnityEditor.IMGUI.Controls.TreeViewState m_FilesTreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_FilesListMCHState;
        bool m_bMutiFilesTreeView = false;
        TreeAssetView m_pFilesTreeView = null;
        TreeAssetView m_pFilesTreeViewMultiColums = null;

        UnityEditor.IMGUI.Controls.TreeViewState m_BundAssetTreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_BundleAssetsListMCHState;
        TreeAssetView m_pBundAssetTreeView = null;

        UnityEditor.IMGUI.Controls.TreeViewState m_BundAssetDetialTreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_BundleAssetDetialsListMCHState;
        TreeAssetView m_pBundAssetAssetDetialTreeView = null;

        UnityEditor.IMGUI.Controls.TreeViewState m_AssetRefDetialTreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_AssetRefDetialsListMCHState;
        TreeAssetView m_pAssetRefDetialTreeView = null;

        UnityEditor.IMGUI.Controls.TreeViewState m_PakagesTreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_PakagesTreeStateMCHState;
        TreeAssetView m_pPakagesTreeView = null;

        BuildTarget m_SelectTarget = BuildTarget.Android;
        PublishPanel.PublishSetting m_PublishSetting;
        HashSet<string> m_vIngoreSuffixs = new HashSet<string>();
        FileItem m_SelectFile = null;
        bool m_bChangeSelectRefreshBundleAssetView = false;

        Dictionary<string, int> m_vRefCnt = new Dictionary<string, int>();

        int m_nTotalFileCnt = 0;
        long m_nTotalSize = 0;

        bool m_bShowAssetsRef = false;
        //------------------------------------------------------
        public void OnDisable()
        {
            AssetBundle.UnloadAllAssetBundles(false);
        }
        //------------------------------------------------------
        public void OnEnable(PublishPanel.PublishSetting setting)
        {            
            m_PublishSetting = setting;
            m_SelectTarget = m_PublishSetting.buildTarget;
            {
                MultiColumnHeaderState.Column[] colums = 
                {
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                };
                colums[0].headerContent = new GUIContent("文件", "");
                colums[0].minWidth = 100;
                colums[0].width = 100;
                colums[0].maxWidth = 100;
                colums[0].headerTextAlignment = TextAlignment.Left;
                colums[0].canSort = true;
                colums[0].autoResize = true;

                colums[1].headerContent = new GUIContent("大小", "");
                colums[1].minWidth = 300;
                colums[1].width = 100;
                colums[1].maxWidth = 200;
                colums[1].headerTextAlignment = TextAlignment.Center;
                colums[1].canSort = true;
                colums[1].autoResize = true;

                colums[2].headerContent = new GUIContent("类型", "");
                colums[2].minWidth = 50;
                colums[2].width = 50;
                colums[2].maxWidth = 50;
                colums[2].headerTextAlignment = TextAlignment.Center;
                colums[2].canSort = false;
                colums[2].autoResize = true;
                m_FilesListMCHState = new MultiColumnHeaderState(colums);
                m_FilesTreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pFilesTreeView = new TreeAssetView(m_FilesTreeState);
                m_pFilesTreeViewMultiColums = new TreeAssetView(m_FilesTreeState, m_FilesListMCHState);
                m_pFilesTreeViewMultiColums.Reload();
                m_pFilesTreeView.Reload();

                m_pFilesTreeView.OnItemDoubleClick = OnFilesSelect;
                m_pFilesTreeView.OnSelectChange = OnFilesSelect;
                m_pFilesTreeView.OnCellDraw = OnFileDrawGUI;

                m_pFilesTreeViewMultiColums.OnItemDoubleClick = OnFilesSelect;
                m_pFilesTreeViewMultiColums.OnSelectChange = OnFilesSelect;
                m_pFilesTreeViewMultiColums.OnCellDraw = OnFileDrawGUIMulti;
                m_pFilesTreeViewMultiColums.OnSortColum = OnFileSlotColums;
            }
            {
                MultiColumnHeaderState.Column[] colums =
                {
                    new MultiColumnHeaderState.Column(),
                };
                colums[0].headerContent = new GUIContent("资源", "");
                colums[0].minWidth = 100;
                colums[0].width = 100;
                colums[0].maxWidth = 100;
                colums[0].headerTextAlignment = TextAlignment.Left;
                colums[0].canSort = true;
                colums[0].autoResize = true;

                m_BundleAssetsListMCHState = new MultiColumnHeaderState(colums);
                m_BundAssetTreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pBundAssetTreeView = new TreeAssetView(m_BundAssetTreeState, m_BundleAssetsListMCHState);
                m_pBundAssetTreeView.OnCellDraw = OnBundleAssetDrawGUI;
                m_pBundAssetTreeView.Reload();
            }
            {
                MultiColumnHeaderState.Column[] colums =
                {
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                };
                colums[0].headerContent = new GUIContent("详细引用资源", "");
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

                m_BundleAssetDetialsListMCHState = new MultiColumnHeaderState(colums);
                m_BundAssetDetialTreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pBundAssetAssetDetialTreeView = new TreeAssetView(m_BundAssetDetialTreeState, m_BundleAssetDetialsListMCHState);
                m_pBundAssetAssetDetialTreeView.OnCellDraw = OnBundleAssetDetialDrawGUI;
                m_pBundAssetAssetDetialTreeView.SetRowHeight(20);
                m_pBundAssetAssetDetialTreeView.Reload();
            }

            {
                MultiColumnHeaderState.Column[] colums =
                {
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                };
                colums[0].headerContent = new GUIContent("详细引用资源", "");
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

                m_AssetRefDetialsListMCHState = new MultiColumnHeaderState(colums);
                m_AssetRefDetialTreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pAssetRefDetialTreeView = new TreeAssetView(m_AssetRefDetialTreeState, m_AssetRefDetialsListMCHState);
                m_pAssetRefDetialTreeView.OnCellDraw = OnBundleAssetDetialDrawGUI;
                m_pAssetRefDetialTreeView.SetRowHeight(20);
                m_pAssetRefDetialTreeView.Reload();
            }

            {
                MultiColumnHeaderState.Column[] colums =
                {
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                };
                colums[0].headerContent = new GUIContent("加固包", "");
                colums[0].minWidth = 100;
                colums[0].width = 100;
                colums[0].maxWidth = 100;
                colums[0].headerTextAlignment = TextAlignment.Left;
                colums[0].canSort = false;
                colums[0].autoResize = true;

                colums[1].headerContent = new GUIContent("大小", "");
                colums[1].minWidth = 100;
                colums[1].width = 100;
                colums[1].maxWidth = 100;
                colums[1].headerTextAlignment = TextAlignment.Left;
                colums[1].canSort = true;
                colums[1].autoResize = true;

                m_PakagesTreeStateMCHState = new MultiColumnHeaderState(colums);
                m_PakagesTreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pPakagesTreeView = new TreeAssetView(m_PakagesTreeState, m_PakagesTreeStateMCHState);
                m_pPakagesTreeView.OnItemDoubleClick = OnEncrptyPakSelect;
                m_pPakagesTreeView.OnSelectChange = OnEncrptyPakSelect;
                m_pPakagesTreeView.OnCellDraw = OnEncrptyPackDrawGUI;
                m_pPakagesTreeView.Reload();
            }

          //  Refresh();
          //  RefreshEncrptyPacks();
        }
        //------------------------------------------------------
        public void Refresh(string strVersion = null)
        {
            RefreshEncrptyPacks(strVersion);
            return;
            AssetBundle.UnloadAllAssetBundles(false);
            if (m_bMutiFilesTreeView)
                RefreshAssetBundleFiles(m_pFilesTreeViewMultiColums, false);
            else
                RefreshAssetBundleFiles(m_pFilesTreeView);

            RefreshEncrptyPacks();
        }
        //------------------------------------------------------
        public void RefreshEncrptyPacks(string strVersion = null)
        {
            GameDelegate.DeleteAllPackages();
            m_pPakagesTreeView.BeginTreeData();
            string encrpty_dir = m_PublishSetting.GetBuildTargetEncrtpyDir(m_SelectTarget);
            string strSuffix = m_PublishSetting.GetEncrtpyPackSuffix(m_SelectTarget);
            if (Directory.Exists(encrpty_dir))
            {
                string[] encrtpy_packages = Directory.GetFiles(encrpty_dir, "*."+strSuffix, SearchOption.AllDirectories);
                int extern_id = encrtpy_packages.Length + 1;
                for (int i = 0; i < encrtpy_packages.Length; ++i)
                {
                    System.IO.FileInfo fileInfo = fileInfo = new System.IO.FileInfo(encrtpy_packages[i]);
                    string name = System.IO.Path.GetFileName(encrtpy_packages[i]);
                    int idx = 0;
                    if(!int.TryParse(System.IO.Path.GetFileNameWithoutExtension(encrtpy_packages[i]).Replace("package_",""), out idx))
                    {
                        idx = extern_id++;
                    }
                    m_pPakagesTreeView.AddData(new FileItem()
                    {
                        path = encrtpy_packages[i].Replace("\\", "/"),
                        name = name,
                        id = idx,
                        itemType = EItemType.EncrptyPak,
                        fileInfo = fileInfo,
                        totalSize = fileInfo.Length

                    });
                }
                if(!string.IsNullOrEmpty(strVersion))
                {
                    string outPut = Application.dataPath + m_PublishSetting.GetBuildOutputRoot(m_PublishSetting.buildTarget);
                    if(Directory.Exists(outPut + "/" + strVersion + "/updates"))
                    {
                        string[] update_encrtpy_packages = Directory.GetFiles(outPut + "/" + strVersion + "/updates", "*." + strSuffix, SearchOption.AllDirectories);
                        for (int i = 0; i < update_encrtpy_packages.Length; ++i)
                        {
                            System.IO.FileInfo fileInfo = fileInfo = new System.IO.FileInfo(update_encrtpy_packages[i]);
                            string name = System.IO.Path.GetFileName(update_encrtpy_packages[i]);
                            int idx = 0;
                            if (!int.TryParse(System.IO.Path.GetFileNameWithoutExtension(update_encrtpy_packages[i]).Replace("package_", ""), out idx))
                            {
                                idx = extern_id++;
                            }
                            m_pPakagesTreeView.AddData(new FileItem()
                            {
                                path = update_encrtpy_packages[i].Replace("\\", "/"),
                                name = name,
                                id = idx,
                                itemType = EItemType.EncrptyPak,
                                fileInfo = fileInfo,
                                totalSize = fileInfo.Length

                            });
                        }
                    }

                }
                m_pPakagesTreeView.GetDatas().Sort( (TreeAssetView.ItemData f1, TreeAssetView.ItemData f2) => { return f1.id - f2.id; });
            }
            m_pPakagesTreeView.EndTreeData();
        }
        //------------------------------------------------------
        void BuildTreeFile(TreeAssetView treeView, FileList root, bool IncludeDir)
        {
            if (root.item.itemType == EItemType.Dir)
            {
                if(IncludeDir)  treeView.AddData(root.item);
            }
            else
                treeView.AddData(root.item);
            if (root.childs!=null)
            {
                for (int i = 0; i < root.childs.Count; ++i)
                {
                    BuildTreeFile(treeView, root.childs[i], IncludeDir);
                }
            }
        }
        //------------------------------------------------------
        void RefreshAssetBundleFiles(TreeAssetView treeView, bool IncludeDir = true)
        {
            FileList root = new FileList("root");
            string dir = m_PublishSetting.GetBuildTargetOutputDir(m_SelectTarget);

            m_vIngoreSuffixs.Clear();
            PublishPanel.PublishSetting.Platform platform = m_PublishSetting.GetPlatform(m_PublishSetting.buildTarget);
            for (int i = 0; i < platform.ignore_suffix.Count; ++i)
                m_vIngoreSuffixs.Add("."+platform.ignore_suffix[i].ToLower());
            int guid = 0;
            m_nTotalFileCnt = 0;
            m_nTotalSize = 0;
            BuildDirFiles(root, dir, dir, 1, ref guid, ref m_nTotalFileCnt, ref m_nTotalSize);

            treeView.BeginTreeData();
            if (root.childs != null)
            {
                for (int i = 0; i < root.childs.Count; ++i)
                {
                    BuildTreeFile(treeView, root.childs[i], IncludeDir);
                }
            }
            treeView.EndTreeData();

            m_vRefCnt.Clear();
            foreach (var db in treeView.GetDatas())
            {
                if((db as FileItem).itemType == EItemType.File )
                {
                    AssetBundle bundle = FindAssetBunlde((db as FileItem).path, (db as FileItem).shortFile);
                    (db as FileItem).assetBundle = bundle;

                    if(bundle!=null)
                    {
                        string[] assets = bundle.GetAllAssetNames();
                        for (int i = 0; i < assets.Length; ++i)
                        {
                            int refCnt = 0;
                            if (!m_vRefCnt.TryGetValue(assets[i], out refCnt))
                                m_vRefCnt[assets[i]] = 1;
                            else
                                m_vRefCnt[assets[i]] = refCnt+1;
                        }
                    }
                }
            }

            guid = 0;
            m_pAssetRefDetialTreeView.BeginTreeData();
            foreach (var db in m_vRefCnt)
            {
                m_pAssetRefDetialTreeView.AddData(new FileItem() { refCnt = db.Value, id = guid++, path = db.Key, name = System.IO.Path.GetFileNameWithoutExtension(db.Key) } );
            }
            m_pAssetRefDetialTreeView.EndTreeData();
        }
        //------------------------------------------------------
        void BuildDirFiles(FileList root, string dir, string rootDir, int depth, ref int guid, ref int fileCnt, ref long totalSize)
        {
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            int subIndex = dir.Replace("\\", "/").Length;
            string[] dirs = Directory.GetDirectories(dir);

            string[] files = Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly);
            for(int i = 0; i < files.Length; ++i)
            {
                if(m_vIngoreSuffixs.Contains(System.IO.Path.GetExtension(files[i]).ToLower())) continue;
                string strCur = dir.Replace("\\", "/").Substring(subIndex);

                FileList item = new FileList(strCur, depth);
                item.item.name = System.IO.Path.GetFileName(files[i]);
                item.item.path = files[i].Replace("\\", "/");
                item.item.shortPath = System.IO.Path.GetDirectoryName(item.item.path).Replace("\\", "/").Replace(rootDir, "");
                item.item.shortFile = item.item.path.Replace("\\", "/").Replace(rootDir+"/", "");
                item.item.id = guid++;
                item.item.itemType = EItemType.File;

                item.item.fileInfo = new System.IO.FileInfo(files[i]);
                item.item.totalSize = item.item.fileInfo.Length;
                item.item.parent = root.item;
                root.childs.Add(item);
                fileCnt++;
                totalSize += item.item.fileInfo.Length;
            }

            for (int i = 0; i < dirs.Length; ++i)
            {
                FileList parent = root;
                string strCur = dirs[i].Replace("\\", "/").Substring(subIndex+1);
                if (strCur.Length > 0)
                {
                    parent = new FileList(strCur, depth);
                    parent.item.path = dirs[i].Replace("\\", "/");
                    parent.item.shortPath = System.IO.Path.GetDirectoryName(parent.item.path).Replace("\\", "/").Replace(rootDir, "");
                    parent.item.shortFile = parent.item.shortPath + "/"+ strCur;
                    parent.item.itemType = EItemType.Dir;
                    parent.item.id = guid++;
                    parent.item.parent = root.item;
                    root.childs.Add(parent);

                    int fileSubCnt = 0;
                    long totalSubSize = 0;
                    BuildDirFiles(parent, dirs[i], rootDir, depth + 1, ref guid, ref fileSubCnt, ref totalSubSize);
                    fileCnt += fileSubCnt;
                    totalSize += totalSubSize;
                }
            }
            if (root.item.itemType == EItemType.Dir)
            {
                root.item.fileCnt += fileCnt;
                root.item.totalSize += totalSize;
            }
        }
        //------------------------------------------------------
        public void OnGUI(Rect position)
        {
            if (m_PublishSetting == null) return;
            float viewWidthLeft = 500;
            float viewWidthRight = position.width - viewWidthLeft;

            float lablwidth = EditorGUIUtility.labelWidth;

            float offsetY = 25;
            float viewHeight = position.height - offsetY;

            //! assetbundle
            Rect fileRect = new Rect(0, offsetY, viewWidthLeft, viewHeight / 2);
            GUILayout.BeginArea(fileRect);
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(viewWidthLeft) });
                DrawTile("资源列表", Color.green, viewWidthLeft - 160, 30);
                EditorGUI.BeginChangeCheck();
                EditorGUIUtility.labelWidth = 60;
                m_bMutiFilesTreeView = EditorGUILayout.Toggle("多列表", m_bMutiFilesTreeView, new GUILayoutOption[] { GUILayout.Width(120) });
                if (EditorGUI.EndChangeCheck())
                    Refresh();
                if (GUILayout.Button("导出", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    ExportAssetToCsv.ExportAssets(m_pFilesTreeViewMultiColums.GetDatas());
                }
                GUILayout.EndHorizontal();

                EditorGUIUtility.labelWidth = lablwidth;

                if (m_bMutiFilesTreeView)
                {
                    m_FilesListMCHState.columns[1].width = 120;
                    m_FilesListMCHState.columns[2].width = 80;
                    m_FilesListMCHState.columns[0].width = viewWidthLeft - 200;
                    if (m_pFilesTreeViewMultiColums != null)
                        m_pFilesTreeViewMultiColums.OnGUI(new Rect(0, 30, viewWidthLeft, viewHeight / 2));
                }
                else
                {
                    if (m_pFilesTreeView != null)
                        m_pFilesTreeView.OnGUI(new Rect(0, 30, viewWidthLeft, viewHeight / 2));
                }

            }
            GUILayout.EndArea();


            //! 加固包体
            Rect packRect = new Rect(0, offsetY + viewHeight / 2+5, viewWidthLeft, position.height - (offsetY + viewHeight / 2+5));
            GUILayout.BeginArea(packRect);
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(packRect.width) });
                DrawTile("加固资源包列表", Color.green, packRect.width-270, 30);
                if(GUILayout.Button("清除加固包", new GUILayoutOption[] { GUILayout.Width(90), GUILayout.Height(30) }))
                {
                    ClearEncrptyPaks(m_PublishSetting, m_SelectTarget);
                    RefreshEncrptyPacks();
                }
                if (GUILayout.Button("构建加固包", new GUILayoutOption[] { GUILayout.Width(90), GUILayout.Height(30) }))
                {
                    BuildEncrptyPak(m_PublishSetting, null, m_SelectTarget);
                    RefreshEncrptyPacks();
                }
                if (GUILayout.Button("反解加固包", new GUILayoutOption[] { GUILayout.Width(90), GUILayout.Height(30) }))
                {
                    ExtractPak(m_PublishSetting, m_SelectTarget);
                }
                GUILayout.EndHorizontal();
                if (m_pPakagesTreeView != null)
                {
                    m_PakagesTreeStateMCHState.columns[0].width = viewWidthLeft - 100;
                    m_pPakagesTreeView.OnGUI(new Rect(0, 30, viewWidthLeft, packRect.height -30));
                }
            }
            GUILayout.EndArea();

            float infoOffset = viewWidthLeft + 4;
            Rect inspector = new Rect(infoOffset, offsetY, position.width - infoOffset, position.height - offsetY);

            GUILayout.BeginArea(inspector);
            DrawPackageSetting(new Rect(new Rect(infoOffset, 0, position.width - infoOffset, viewHeight / 2)));
            offsetY = GUILayoutUtility.GetLastRect().yMax;

            DrawFileInfo(new Rect(0, offsetY, inspector.width, inspector.height));
            GUILayout.EndArea();
        }
        //------------------------------------------------------
        void DrawPackageSetting(Rect rect)
        {
            DrawTile("配置信息", Color.green, rect.width - 5);
            PublishPanel.PublishSetting.Platform platform = m_PublishSetting.GetPlatform(m_PublishSetting.buildTarget);
            platform.package_size_kb = EditorGUILayout.IntField("单包大小", platform.package_size_kb);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("明文", new GUILayoutOption[] { GUILayout.Width(80) });
            for (int i = 0; i < m_PublishSetting.pak_encrtpys.Length; ++i)
                m_PublishSetting.pak_encrtpys[i] = EditorGUILayout.IntField(m_PublishSetting.pak_encrtpys[i], new GUILayoutOption[] { GUILayout.Width(100) });
            GUILayout.EndHorizontal();
            float layoutWidth = rect.width - 20;
            //后缀
            {
                string strSuffixs = "";
                for (int i = 0; i < platform.ignore_suffix.Count; ++i)
                    strSuffixs += platform.ignore_suffix[i] + ";";
                GUILayout.BeginHorizontal();
                strSuffixs = EditorGUILayout.TextField("忽略后缀", strSuffixs);
                if(GUILayout.Button("刷新", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    Refresh();
                }
                GUILayout.EndHorizontal();
                platform.ignore_suffix.Clear();
                string[] suffix = strSuffixs.Split(';');
                for (int i = 0; i < suffix.Length; ++i)
                {
                    if (!string.IsNullOrEmpty(suffix[i]))
                        platform.ignore_suffix.Add(suffix[i]);
                }
            }

            {
                GUILayout.BeginHorizontal();
                DrawTile("忽略文件", Color.red, rect.width - 50);
                if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    platform.ignore_file.Add("");
                }
                GUILayout.EndHorizontal();
                {
                    for (int i = 0; i < platform.ignore_file.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        platform.ignore_file[i] = EditorGUILayout.TextField("文件", platform.ignore_file[i]);
                        if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(40) }))
                        {
                            platform.ignore_file.RemoveAt(i);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }

            {
                GUILayout.BeginHorizontal();
                DrawTile("忽略路径", Color.red, rect.width - 50);
                if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    platform.ignore_path.Add("");
                }
                GUILayout.EndHorizontal();
                {
                    for (int i = 0; i < platform.ignore_path.Count; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        platform.ignore_path[i] = EditorGUILayout.TextField("文件", platform.ignore_path[i]);
                        if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(40) }))
                        {
                            platform.ignore_path.RemoveAt(i);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
        }
        //------------------------------------------------------
        void DrawFileInfo(Rect rect)
        {
            DrawTile("总体信息", Color.green, rect.width - 5);
            EditorGUILayout.LabelField("包体资源大小:" + Base.Util.FormBytes(m_nTotalSize));
            EditorGUILayout.LabelField("包体资源个数:" + m_nTotalFileCnt + " 个");
            EditorGUILayout.LabelField("所有资源个数:" + m_vRefCnt.Count + " 个");

            DrawTile("文件信息", Color.green, rect.width - 5);
            if (m_SelectFile == null)
            {
                m_bShowAssetsRef = EditorGUILayout.Toggle("显示所有资源", m_bShowAssetsRef);
            }
            else
            {
                string dir = Application.dataPath;
                float lablwidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 80;

                GUILayout.BeginHorizontal();
                GUILayout.Label("相对路径");
                EditorGUILayout.TextField(m_SelectFile.shortFile);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("相对目录");
                EditorGUILayout.TextField(m_SelectFile.shortPath);
                GUILayout.EndHorizontal();
                EditorGUIUtility.labelWidth = lablwidth;
                if (m_SelectFile.fileInfo != null)
                {
                    EditorGUILayout.LabelField("文件目录=" + m_SelectFile.path.Substring(dir.Length));
                    EditorGUILayout.LabelField("文件名  =" + m_SelectFile.fileInfo.Name);
                    EditorGUILayout.LabelField("文件大小=" + Base.Util.FormBytes(m_SelectFile.fileInfo.Length));

                    m_bShowAssetsRef = EditorGUILayout.Toggle("显示所有资源", m_bShowAssetsRef);
                    if (!m_bShowAssetsRef)
                    {
                        if (m_SelectFile.assetBundle != null)
                        {
                            float leftWidth = rect.width * 1 / 3;
                            float maxy = GUILayoutUtility.GetLastRect().yMax + 5;
                            if (m_pBundAssetTreeView != null)
                            {
                                m_BundleAssetsListMCHState.columns[0].width = leftWidth;
                                m_pBundAssetTreeView.OnGUI(new Rect(0, GUILayoutUtility.GetLastRect().yMax, leftWidth, rect.height - maxy));
                            }
                            if (m_pBundAssetAssetDetialTreeView != null)
                            {
                                m_BundleAssetDetialsListMCHState.columns[0].width = rect.width - leftWidth - m_BundleAssetDetialsListMCHState.columns[1].width;
                                m_pBundAssetAssetDetialTreeView.OnGUI(new Rect(leftWidth, GUILayoutUtility.GetLastRect().yMax, rect.width - leftWidth, rect.height - maxy));
                            }
                        }
                    }

                }
                else
                {
                    EditorGUILayout.LabelField("文件目录=" + m_SelectFile.path.Substring(dir.Length));
                    EditorGUILayout.LabelField("文件数量=" + m_SelectFile.fileCnt);
                    EditorGUILayout.LabelField("总大小  =" + Base.Util.FormBytes(m_SelectFile.totalSize));
                    m_bShowAssetsRef = EditorGUILayout.Toggle("显示所有资源", m_bShowAssetsRef);
                }
            }
            if (m_bShowAssetsRef)
            {
                float leftWidth = rect.width;
                float maxy = GUILayoutUtility.GetLastRect().yMax + 5;
                if (m_pAssetRefDetialTreeView != null)
                {
                    m_AssetRefDetialsListMCHState.columns[0].width = leftWidth - m_AssetRefDetialsListMCHState.columns[1].width;
                    m_pAssetRefDetialTreeView.OnGUI(new Rect(0, GUILayoutUtility.GetLastRect().yMax, leftWidth, rect.height - maxy));
                }
            }

            if (!m_bShowAssetsRef && m_SelectFile!=null && m_SelectFile.packageHandler != System.IntPtr.Zero )
            {
                float leftWidth = rect.width;
                float maxy = GUILayoutUtility.GetLastRect().yMax + 5;
                if (m_pBundAssetTreeView != null)
                {
                    m_BundleAssetsListMCHState.columns[0].width = leftWidth;
                    m_pBundAssetTreeView.OnGUI(new Rect(0, GUILayoutUtility.GetLastRect().yMax, leftWidth, rect.height - maxy));
                }
            }
        }
        //------------------------------------------------------
        public void Update()
        {
        }
        //------------------------------------------------------
        void BuildBundlAssetList(FileItem pakFile)
        {
            m_pBundAssetTreeView.BeginTreeData();
            if(pakFile.assetBundle != null)
            {
                string[] assets = pakFile.assetBundle.GetAllAssetNames();
                for (int i = 0; i < assets.Length; ++i)
                {
                    m_pBundAssetTreeView.AddData(new TreeAssetView.ItemData() { id = i, path = assets[i], name = System.IO.Path.GetFileNameWithoutExtension(assets[i]) });
                }
            }
            m_pBundAssetTreeView.EndTreeData();

            m_pBundAssetAssetDetialTreeView.BeginTreeData();
            if (pakFile.assetBundle != null)
            {
                string[] assets = pakFile.assetBundle.GetAllAssetNames();
                int guid = 0;
                for (int i = 0; i < assets.Length; ++i)
                {
                    string[] depths = AssetDatabase.GetDependencies(assets[i]);
                    m_pBundAssetAssetDetialTreeView.AddData(new FileItem() { refCnt = 1, id = guid++, path = assets[i], name = System.IO.Path.GetFileNameWithoutExtension(assets[i]) });
                    if (depths != null)
                    {
                        for (int j = 0; j < depths.Length; ++j)
                        {
                            m_pBundAssetAssetDetialTreeView.AddData(new FileItem() { refCnt = 1, id = guid++, path = depths[j], name = System.IO.Path.GetFileNameWithoutExtension(depths[j]) });
                        }
                    }
   
                }
            }
            m_pBundAssetAssetDetialTreeView.EndTreeData();
        }
        //------------------------------------------------------
        void OnEncrptyPakSelect(TreeAssetView.ItemData item)
        {
            m_SelectFile = item as FileItem;
            if(m_SelectFile.itemType == EItemType.EncrptyPak)
            {
                GameDelegate.EnableCatchHandle(false);
                m_SelectFile.packageHandler = GameDelegate.LoadPackage(m_SelectFile.path);
                m_pBundAssetTreeView.BeginTreeData();
                if (m_SelectFile.packageHandler != System.IntPtr.Zero)
                {
                    int entrtyCnt = GameDelegate.GetPackageEntryCount(m_SelectFile.packageHandler);
                    for (int i = 0; i < entrtyCnt; ++i)
                    {
                        System.IntPtr enterHanler = GameDelegate.GetEntryPackage(m_SelectFile.packageHandler, i);
                        string name = GameDelegate.GetEntryPackageFileName(enterHanler);
                        if(!string.IsNullOrEmpty(name))
                        {
                            m_pBundAssetTreeView.AddData(new FileItem() { path = name, name = name, id = i });
                        }
                    }
                }
                m_pBundAssetTreeView.EndTreeData();
            }
        }
        //------------------------------------------------------
       public static  AssetBundle FindAssetBunlde(string strAb, string shortFile)
        {
            if (!File.Exists(strAb) || System.IO.Path.GetExtension(strAb).Contains(".manifest") || System.IO.Path.GetExtension(strAb).Contains(".ver") || System.IO.Path.GetExtension(strAb).Contains(".txt")) return null;
            if (strAb.Contains("raws/")) return null;
            IEnumerable<AssetBundle> bundles = AssetBundle.GetAllLoadedAssetBundles();
            foreach (var db in bundles)
            {
                string name = db.name;
                if (!string.IsNullOrEmpty(strAb) && name.Contains(shortFile))
                {
                    return db;
                }
            }

            return AssetBundle.LoadFromFile(strAb);
        }
        //------------------------------------------------------
        void OnFilesSelect(TreeAssetView.ItemData item)
        {
            m_SelectFile = item as FileItem;
            if (m_SelectFile.itemType == EItemType.File && m_SelectFile.assetBundle == null)
            {
                if (m_SelectFile.assetBundle == null)
                {
                    m_SelectFile.assetBundle = FindAssetBunlde(m_SelectFile.path, m_SelectFile.shortFile);
                }

            }
            BuildBundlAssetList(m_SelectFile);
        }
        //------------------------------------------------------
        bool OnFileDrawGUI(Rect r, TreeAssetView.TreeItemData t, int c, bool s, bool f)
        {
            PublishPanel.PublishSetting.Platform platform = m_PublishSetting.GetPlatform(m_PublishSetting.buildTarget);
            FileItem fileItem = t.data as FileItem;

            fileItem.color = Color.white;
            if (fileItem.itemType == EItemType.Dir)
            {
                if (!string.IsNullOrEmpty(fileItem.shortPath) && platform.ignore_path.Contains(fileItem.shortPath))
                    fileItem.color = Color.red;
            }
            else
            {
                if ( (!string.IsNullOrEmpty(fileItem.shortFile) && platform.ignore_file.Contains(fileItem.shortFile)) ||
                    !string.IsNullOrEmpty(fileItem.shortPath) && platform.ignore_path.Contains(fileItem.shortPath))
                    fileItem.color = Color.red;
            }
            return false;
        }
        //------------------------------------------------------
        List<TreeAssetView.TreeItemData> OnFileSlotColums(List<TreeAssetView.TreeItemData> vDatas, MultiColumnHeader multiColumnHeader, int c)
        {
            bool ascending = multiColumnHeader.IsSortedAscending(c);
            if (c == 0)
            {
                vDatas.Sort((TreeAssetView.TreeItemData l, TreeAssetView.TreeItemData r) => { return l.displayName.CompareTo(r.displayName)* (ascending?1:-1); });
            }
            else if (c == 1)
            {
                vDatas.Sort((TreeAssetView.TreeItemData l, TreeAssetView.TreeItemData r) => { return (int)(((FileItem)l.data).totalSize - ((FileItem)r.data).totalSize) * (ascending ? 1 : -1); });
            }
            return vDatas;
        }
        //------------------------------------------------------
        bool OnFileDrawGUIMulti(Rect cellRect, TreeAssetView.TreeItemData t, int c, bool s, bool f)
        {
            FileItem fileItem = t.data as FileItem;
            OnFileDrawGUI(cellRect, t, c, s, f);
            if(c == 0)
            {
                float gap = Mathf.Max(0, 12 * fileItem.depth);
                if (m_pFilesTreeViewMultiColums.buildMutiColumnDepth)
                    GUI.Label(new Rect(cellRect.x+ gap, cellRect.y, cellRect.width- gap, cellRect.height),  fileItem.shortFile);
                else
                    GUI.Label(cellRect, fileItem.shortFile);
            }
            else if (c == 1)
            {
                GUI.Label(cellRect, Base.Util.FormBytes(fileItem.totalSize));
            }
            else if (c == 2)
            {
                GUI.Label(cellRect, fileItem.itemType.ToString());
            }
            return true;
        }
        //------------------------------------------------------
        bool OnBundleAssetDrawGUI(Rect cellRect, TreeAssetView.TreeItemData t, int c, bool s, bool f)
        {
            GUI.color = Color.white;
            if (c == 0)
            {
                GUI.Label(cellRect, t.data.path);
            }
            return true;
        }
        //------------------------------------------------------
        bool OnBundleAssetDetialDrawGUI(Rect cellRect, TreeAssetView.TreeItemData t, int c, bool s, bool f)
        {
            GUI.color = Color.white;
            if (c == 0)
            {
                if(t.icon == null)
                t.icon = AssetDatabase.GetCachedIcon(t.data.path) as Texture2D;
                if(t.icon)
                {
                    GUI.DrawTexture(new Rect(cellRect.x, cellRect.y, cellRect.height, cellRect.height), t.icon);
                    GUI.Label(new Rect(cellRect.x+cellRect.height, cellRect.y, cellRect.width-cellRect.height, cellRect.height), System.IO.Path.GetFileName(t.data.path));
                }
                else
                    GUI.Label(cellRect, t.data.path);
            }
            else if (c == 1)
            {
                int refCnt = 0;
                if(!m_vRefCnt.TryGetValue(t.data.path, out refCnt))
                {
                    refCnt = (t.data as FileItem).refCnt;
                }
                GUI.Label(cellRect, refCnt.ToString() );
            }
            return true;
        }
        //------------------------------------------------------
        bool OnEncrptyPackDrawGUI(Rect cellRect, TreeAssetView.TreeItemData t, int c, bool s, bool f)
        {
            GUI.color = Color.white;
            if (c == 0)
            {
                if (t.icon == null)
                    t.icon = t.data.itemIcon();
                if (t.icon)
                {
                    GUI.DrawTexture(new Rect(cellRect.x, cellRect.y, cellRect.height, cellRect.height), t.icon);
                    GUI.Label(new Rect(cellRect.x + cellRect.height, cellRect.y, cellRect.width - cellRect.height, cellRect.height), System.IO.Path.GetFileName(t.data.path));
                }
                else
                    GUI.Label(cellRect, t.data.path);
            }
            else if (c == 1)
            {
                GUI.Label(cellRect, Base.Util.FormBytes((t.data as FileItem).totalSize));
            }
            return true;
        }
        //------------------------------------------------------
        void DrawTile(string label, Color color, float width = 0, float height = 20)
        {
            Color back = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUILayout.Box(label, new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) });
            GUI.backgroundColor = back;
        }
        //------------------------------------------------------
        public static void ClearEncrptyPaks(PublishPanel.PublishSetting setting, BuildTarget target = BuildTarget.NoTarget)
        {
            string suffixName = setting.GetEncrtpyPackSuffix(target);
            string encrpty_dir = setting.GetBuildTargetEncrtpyDir(target);
            if (!Directory.Exists(encrpty_dir))
                Directory.CreateDirectory(encrpty_dir);

            GameDelegate.DeleteAllPackages();
            string[] encrtpy_packages = Directory.GetFiles(encrpty_dir, "*."+suffixName, SearchOption.AllDirectories);
            for (int i = 0; i < encrtpy_packages.Length; ++i)
            {
                File.Delete(encrtpy_packages[i]);
            }
        }
        //------------------------------------------------------
        public static void ExtractPak(PublishPanel.PublishSetting setting, BuildTarget target = BuildTarget.NoTarget)
        {
            string encrpty_dir = setting.GetBuildTargetEncrtpyDir(target);
            if (!Directory.Exists(encrpty_dir))
            {
                return;
            }


            string suffixName = setting.GetEncrtpyPackSuffix(target);
            string extract_dir = setting.GetBuildTargetExtractDir(target);
            if (!Directory.Exists(extract_dir))
                Directory.CreateDirectory(extract_dir);

            GameDelegate.EnableCatchHandle(false);
            GameDelegate.EnablePackage(true);
            GameDelegate.DeleteAllPackages();
            string[] encrtpy_packages = Directory.GetFiles(encrpty_dir, "*."+suffixName, SearchOption.AllDirectories);
            EditorUtility.DisplayProgressBar("反解包", "", 0);
            uint dwIdStart = 1;
            for (int i = 0; i < encrtpy_packages.Length; ++i)
            {
                EditorUtility.DisplayProgressBar("反解包", encrtpy_packages[i], (float)i / (float)encrtpy_packages.Length);
                string strName = System.IO.Path.GetFileNameWithoutExtension(encrtpy_packages[i]);
                uint nCur = 0;
                if (uint.TryParse(strName.Replace("package_", ""), out nCur))
                {
                    dwIdStart = (uint)Mathf.Max(dwIdStart, nCur);
                }
                System.IntPtr packHandle = GameDelegate.LoadPackage(encrtpy_packages[i]);
                if(packHandle == System.IntPtr.Zero)
                {
                    continue;
                }
                int entryCnt = GameDelegate.GetPackageEntryCount(packHandle);
                for(int j =0; j < entryCnt; ++j)
                {
                    System.IntPtr entryPakHandle =  GameDelegate.GetEntryPackage(packHandle, j);
                    if (entryPakHandle == System.IntPtr.Zero)
                    {
                        continue;
                    }

                    string file = GameDelegate.GetEntryPackageFileName(entryPakHandle);
                    if (string.IsNullOrEmpty(file)) continue;
                    string strAbsFile = extract_dir + "/" + file;
                    string strAbsRoot = System.IO.Path.GetDirectoryName(strAbsFile);
                    if (!Directory.Exists(strAbsRoot)) Directory.CreateDirectory(strAbsRoot);

                    int dataSize = 0;
                    byte[] buff = GameDelegate.ReadFile(file, true, ref dataSize);
                    if(buff!=null && dataSize>0)
                    {
                        System.IO.FileStream fs = new System.IO.FileStream(strAbsFile, System.IO.FileMode.OpenOrCreate);
                        System.IO.BinaryWriter sw = new System.IO.BinaryWriter(fs, System.Text.Encoding.UTF8);
                        fs.SetLength(0);
                        fs.Position = 0;
                        fs.Flush();
                        sw.Write(buff, 0, dataSize);
                        sw.Close();
                    }
                }
            }
            EditorUtility.ClearProgressBar();
            GameDelegate.DeleteAllPackages();
        }
        //------------------------------------------------------
        struct PakTag
        {
            public string dir;
            public string file;
        }
        public static void BuildEncrptyPak(PublishPanel.PublishSetting setting, List<string> vDirs, BuildTarget target = BuildTarget.NoTarget, bool bVersionFile = true, List<string> vEncrtpyPackages = null, string strOutput=null, string strVersion=null)
        {
            if (target == BuildTarget.NoTarget) target = setting.buildTarget;
            if (vDirs == null || vDirs.Count<=0)
            {
                if(vDirs == null)
                    vDirs = new List<string>();
                vDirs.Add(setting.GetBuildTargetOutputDir(target));
//                 for (int i = 0; i < setting.buildDllPakDirs.Count; ++i)
//                 {
//                     vDirs.Add(Application.dataPath + setting.buildDllPakDirs[i]);
//                 }
//                 for (int i = 0; i < setting.copyToStreamDirs.Count; ++i)
//                 {
//                     string strDir = Application.dataPath;
//                     string start = setting.copyToStreamDirs[i].srcDir;
//                     if (start.StartsWith("Assets/"))
//                     {
//                         strDir += start.Substring("Assets".Length);
//                     }
//                     else
//                     {
//                         if (start.StartsWith("/")) strDir += start;
//                         else strDir += "/" + start;
//                     }
//                     vDirs.Add(strDir);
//                 }
            }
            if (vEncrtpyPackages == null) vEncrtpyPackages = new List<string>();
            string suffixName = setting.GetEncrtpyPackSuffix(target);
            string encrpty_dir = strOutput;
            if(string.IsNullOrEmpty(encrpty_dir))
            {
                encrpty_dir = setting.GetBuildTargetEncrtpyDir(target);
                if (!Directory.Exists(encrpty_dir))
                    Directory.CreateDirectory(encrpty_dir);
            }
            HashSet<string> vIngoreSuffixs = new HashSet<string>();
            vIngoreSuffixs.Clear();
            PublishPanel.PublishSetting.Platform platform = setting.GetPlatform(target);
            for (int i = 0; i < platform.ignore_suffix.Count; ++i)
                vIngoreSuffixs.Add("." + platform.ignore_suffix[i].ToLower());

            Debug.Log("begin encrpty packages...");

            uint nVersion = setting.GetBuildTargetVersion(target);
            uint tryVer;
            if (!string.IsNullOrEmpty(strVersion) && uint.TryParse(strVersion.Replace(".", ""), out tryVer))
            {
                nVersion = tryVer;
            }

            GameDelegate.DeleteAllPackages();
            GameDelegate.EnableCatchHandle(false);
            GameDelegate.EnablePackage(true);
            uint dwIdStart = 0;
            if (vEncrtpyPackages!=null && vEncrtpyPackages.Count>0)
            {
                for(int j = 0; j < vEncrtpyPackages.Count; ++j)
                {
                    string[] encrtpy_packages = Directory.GetFiles(vEncrtpyPackages[j], "*." + suffixName, SearchOption.AllDirectories);
                    for (int i = 0; i < encrtpy_packages.Length; ++i)
                    {
                        string strName = System.IO.Path.GetFileNameWithoutExtension(encrtpy_packages[i]);
                        uint nCur = 0;
                        if (uint.TryParse(strName.Replace("package_", ""), out nCur))
                        {
                            dwIdStart = (uint)Mathf.Max(dwIdStart, nCur);
                        }
                        GameDelegate.LoadPackage(encrtpy_packages[i]);
                    }
                }
            }
            else
            {
                string[] encrtpy_packages = Directory.GetFiles(encrpty_dir, "*." + suffixName, SearchOption.AllDirectories);
                for (int i = 0; i < encrtpy_packages.Length; ++i)
                {
                    string strName = System.IO.Path.GetFileNameWithoutExtension(encrtpy_packages[i]);
                    uint nCur = 0;
                    if (uint.TryParse(strName.Replace("package_", ""), out nCur))
                    {
                        dwIdStart = (uint)Mathf.Max(dwIdStart, nCur);
                    }
                    GameDelegate.LoadPackage(encrtpy_packages[i]);
                }
            }


            Queue<PakTag> abs = new Queue<PakTag>();
            for (int d = 0; d < vDirs.Count; ++d)
            {
                if(!Directory.Exists(vDirs[d])) continue;
                string[] assetbunldes = Directory.GetFiles(vDirs[d], "*.*", SearchOption.AllDirectories);
                if (assetbunldes == null)
                {
                    continue;
                }
                for (int i = 0; i < assetbunldes.Length; ++i)
                {
                    string strSuffix = System.IO.Path.GetExtension(assetbunldes[i]);
                    if (vIngoreSuffixs.Contains(strSuffix.ToLower())) continue;

                    string shortFile = assetbunldes[i].Replace("\\", "/").Replace(vDirs[d] + "/", "");
                    string shortPath = System.IO.Path.GetDirectoryName(shortFile);
                    if (platform.ignore_file.Contains(shortFile) || platform.ignore_path.Contains(shortPath))
                        continue;

                    PakTag tag = new PakTag();
                    tag.dir = vDirs[d];
                    tag.file = assetbunldes[i];
                    abs.Enqueue(tag);
                }
            }

            if(abs.Count<=0)
            {
                EditorUtility.DisplayDialog("提示", "没有assetbundle, 请先打Assetbundle 再进行加固", "好的");
                return;
            }


            System.IntPtr pPackage = GameDelegate.CreateEmptyPackage();
            GameDelegate.SetPackageVersion(pPackage, nVersion);
            uint nCurPakSize = 0;
            uint nPackageSize =(uint)(setting.GetPlatform(target).package_size_kb * 1024);
            uint nPackId = dwIdStart+1;

            float nCompleteCount = 0;
            float totalCnt = abs.Count;
            EditorUtility.DisplayProgressBar("加固包","", 0);
            while(abs.Count>0)
            {
                PakTag pakTag = abs.Dequeue();
                string strFile = pakTag.file.Replace("\\", "/");
                string strSubFile = strFile.Replace(pakTag.dir + "/", "");
                string strSuffix = System.IO.Path.GetExtension(strSubFile);
                byte[] pData = File.ReadAllBytes(strFile);
                uint nDataSize = (uint)pData.Length;
                bool bNewPackageEntry = true;
                System.IntPtr pEnterHandler =  GameDelegate.FindEntryPackage(strSubFile);
                if(pEnterHandler!= System.IntPtr.Zero)
                {
                    string md5 = GameDelegate.GetEntryPackageMd5(pEnterHandler);
                    string curMd5 = GameDelegate.Md5(pData, (int)nDataSize);
                    if (!string.IsNullOrEmpty(md5) && md5.CompareTo(curMd5) == 0)
                    {
                        bNewPackageEntry = false;
                    }
                }

                if(bNewPackageEntry)
                {
                    bool bEncrpty = false;
                    if (setting.unEncryptSuffix.Contains(strSuffix))
                        bEncrpty = false;

                    GameDelegate.CreatePackageEntry(pPackage, pData, nDataSize, strSubFile, bEncrpty);
                    nCurPakSize += nDataSize;
                }

                if(nCurPakSize >= nPackageSize || (nCurPakSize>0 && abs.Count<=0))
                {
                    string strPakName = encrpty_dir + "/" + "package_" + nPackId + "." + suffixName;
                    GameDelegate.SetPackageAbsFilePath(pPackage, strPakName);
                    GameDelegate.SavePackage(pPackage, true);

                    nPackId++;
                    pPackage = GameDelegate.CreateEmptyPackage();
                    GameDelegate.SetPackageVersion(pPackage, nVersion);
                    nCurPakSize = 0;
                }

                nCompleteCount++;
                EditorUtility.DisplayProgressBar("加固包", strSubFile, nCompleteCount/totalCnt);
            }
            EditorUtility.ClearProgressBar();

            if(bVersionFile)
            {
                string versionFile = setting.GetBuildTargetOutputDir() + "/version.ver";
                if (File.Exists(versionFile))
                {
                    try
                    {
                        VersionData.Config config = JsonUtility.FromJson<VersionData.Config>(File.ReadAllText(versionFile));
                        config.packCount = (short)(nPackId - 1);

                        StreamWriter sw = new StreamWriter(File.Open(versionFile, FileMode.OpenOrCreate, FileAccess.Write));
                        sw.BaseStream.Position = 0;
                        sw.BaseStream.SetLength(0);
                        sw.BaseStream.Flush();
                        sw.Write(JsonUtility.ToJson(config, true));
                        sw.Close();
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
            }
            

            GameDelegate.DeleteAllPackages();
            Debug.Log("end encrpty packages...");
        }
    }
}
