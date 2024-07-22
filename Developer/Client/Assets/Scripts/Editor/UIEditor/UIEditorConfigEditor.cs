#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIEditorConfigEditor
作    者:	HappLI
描    述:	UI 配置编辑器
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TopGame.ED;
using UnityEditor.IMGUI.Controls;
using Framework.ED;

namespace TopGame.UI
{
    public class UIEditorConfigEditor : EditorWindow
    {
        public class ElementItem : TopGame.ED.TreeAssetView.ItemData
        {
            public UIConfig.UI pData;
            public override Color itemColor()
            {
                GameObject preFab = AssetDatabase.LoadAssetAtPath<GameObject>(pData.prefab);
                if (preFab == null) return Color.red;
                return Color.white;
            }
        }
        [MenuItem("Tools/UI/UI 配置器")]
        static void Open()
        {
            m_AssetConfig = null;
            if (ms_Instance == null)
                ms_Instance = EditorWindow.GetWindow<UIEditorConfigEditor>();
            ms_Instance.titleContent = new GUIContent("ui 配置器");
        }
        static UIEditorConfigEditor ms_Instance = null;
        public static void Open(UIConfig assetConfig)
        {
            m_AssetConfig = assetConfig;
            if (ms_Instance == null)
                ms_Instance = EditorWindow.GetWindow<UIEditorConfigEditor>();
            ms_Instance.titleContent = new GUIContent("ui 配置器");
            ms_Instance.SetConfig(assetConfig);
        }
        TreeAssetView m_pTreeView = null;

        static UIConfig m_AssetConfig = null;
        bool m_bExpandBackups = false;
        bool m_bExpandCommonBackups = false;
        List<ushort> m_vCommonBackups = new List<ushort>();
        List<UIConfig.UI> m_vConfigs = new List<UIConfig.UI>();
        ElementItem m_Select = null;
        //------------------------------------------------------
        private void OnEnable()
        {
            minSize = new Vector2(800, 600);
            if(m_AssetConfig!=null)
            {
                SetConfig(m_AssetConfig);
            }
            else
            {
                string[] assets = AssetDatabase.FindAssets("t:UIConfig", new string[] { "Assets/Datas/Config" });
                if (assets != null)
                {
                    for (int i = 0; i < assets.Length; ++i)
                    {
                        UIConfig config = AssetDatabase.LoadAssetAtPath<UIConfig>(AssetDatabase.GUIDToAssetPath(assets[i]));
                        if (config)
                        {
                            SetConfig(config);
                            break;
                        }
                    }
                }
            }


            ms_Instance = this;
            {
                MultiColumnHeaderState.Column[] colums = new MultiColumnHeaderState.Column[]
                {
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                    new MultiColumnHeaderState.Column(),
                };

                colums[0].headerContent = new GUIContent("描述", "");
                colums[0].minWidth = 260;
                colums[0].width = 260;
                colums[0].maxWidth = 260;
                colums[0].headerTextAlignment = TextAlignment.Center;
                colums[0].canSort = false;
                colums[0].autoResize = true;

                colums[1].headerContent = new GUIContent("层级", "");
                colums[1].minWidth = 100;
                colums[1].width = 100;
                colums[1].maxWidth = 100;
                colums[1].headerTextAlignment = TextAlignment.Left;
                colums[1].canSort = true;
                colums[1].autoResize = true;

                colums[2].headerContent = new GUIContent("深度", "");
                colums[2].minWidth = 100;
                colums[2].width = 100;
                colums[2].maxWidth = 100;
                colums[2].headerTextAlignment = TextAlignment.Left;
                colums[2].canSort = true;
                colums[2].autoResize = false;

                var headerState = new MultiColumnHeaderState(colums);

                TreeViewState  pTreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pTreeView = new TreeAssetView(pTreeState, headerState);
                m_pTreeView.Reload();

                m_pTreeView.OnItemDoubleClick = OnElementSelect;
                m_pTreeView.OnSelectChange = OnElementSelect;
                m_pTreeView.OnCellDraw += OnElementCellGUI;

                RefreshList();
            }
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            if(m_AssetConfig!=null)
            {
                for (int i = 0; i < m_vCommonBackups.Count;)
                {
                    if (m_vCommonBackups[i] <= 0)
                        m_vCommonBackups.RemoveAt(i);
                    else ++i;
                }
                m_AssetConfig.CommonBackupUIs = m_vCommonBackups.ToArray();
                m_AssetConfig.UIS = m_vConfigs.ToArray();

                EditorUtility.SetDirty(m_AssetConfig);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
            m_AssetConfig = null;
            ms_Instance = null;
        }
        //------------------------------------------------------
        public void SetConfig(UIConfig uiConfig)
        {
            m_vCommonBackups.Clear();
            m_vConfigs.Clear();
            m_AssetConfig = uiConfig;
            if(m_AssetConfig!=null)
            {
                for (int j = 0; j < uiConfig.UIS.Length; ++j)
                {
                    m_vConfigs.Add(uiConfig.UIS[j]);
                }
                if(m_AssetConfig.CommonBackupUIs!=null)
                {
                    m_vCommonBackups = new List<ushort>(m_AssetConfig.CommonBackupUIs);
                }
            }
            RefreshList();
        }
        //------------------------------------------------------
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, 460, position.height-30));
            GUILayout.BeginVertical();
            if (m_pTreeView != null)
            {
                m_pTreeView.searchString = EditorGUILayout.TextField("搜索", m_pTreeView.searchString);
                m_pTreeView.OnGUI(new Rect(0, 20, 460, position.height-52));
            }
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(0, position.height - 30, 460, 30));
            if (m_AssetConfig && GUILayout.Button("关联文件"))
            {
                EditorKits.OpenPathInExplorer(AssetDatabase.GetAssetPath(m_AssetConfig));
            }
            GUILayout.EndArea();
            GUILayout.EndVertical();

            GUILayout.BeginArea(new Rect(460, 0, position.width - 460, position.height));
            GUILayout.BeginVertical();
            DrawProperty();

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        void DrawProperty()
        {
            GUILayout.BeginHorizontal();
            m_bExpandCommonBackups = EditorGUILayout.Foldout(m_bExpandCommonBackups, "通用的UI备份还原列表");
            if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(40) }))
            {
                m_vCommonBackups.Add(0);
            }
            GUILayout.EndHorizontal();
            if (m_bExpandCommonBackups)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < m_vCommonBackups.Count; ++i)
                {
                    GUILayout.BeginHorizontal();
                    m_vCommonBackups[i] = (ushort)(EUIType)HandleUtilityWrapper.PopEnum("UI类型", (EUIType)m_vCommonBackups[i]);
                    if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(40) }))
                    {
                        m_vCommonBackups.RemoveAt(i);
                        break;
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }

            if (m_Select!=null)
            {
                UIConfig.UI uiData = m_Select.pData;
                GUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                uiData.type = (ushort)(EUIType)HandleUtilityWrapper.PopEnum("UI类型", (EUIType)uiData.type);
                EditorGUILayout.LabelField(uiData.type.ToString(), new GUILayoutOption[] { GUILayout.MaxWidth(50) });
                if(EditorGUI.EndChangeCheck())
                {
                    m_Select.name = TopGame.ED.EditorHelp.GetDisplayName((EUIType)uiData.type);
                }
                GUILayout.EndHorizontal();
                Object asset = AssetDatabase.LoadAssetAtPath<GameObject>(uiData.prefab);
                asset = EditorGUILayout.ObjectField(asset, typeof(GameObject), false) as GameObject;
                if (asset != null)
                {
                    uiData.prefab = AssetDatabase.GetAssetPath(asset);
                }
                uiData.permanent = EditorGUILayout.Toggle("常驻", uiData.permanent);
                uiData.alwayShow = EditorGUILayout.Toggle("一直显示", uiData.alwayShow);
                uiData.trackAble = EditorGUILayout.Toggle("信号栈", uiData.trackAble);
                uiData.fullUI = EditorGUILayout.Toggle("全屏", uiData.fullUI);
                uiData.Order = EditorGUILayout.IntField("层级", uiData.Order);
                uiData.uiZValue = EditorGUILayout.IntField("Z轴深度", uiData.uiZValue);
                uiData.showLog = EditorGUILayout.Toggle("开启UI显示关闭打点", uiData.showLog);
                HandleUtilityWrapper.DrawPropertyByFieldName(uiData, "canBackupFlag");

                if (uiData.IsCanBackup())
                {
                    GUILayout.BeginHorizontal();
                    m_bExpandBackups = EditorGUILayout.Foldout(m_bExpandBackups, "UI备份还原列表");
                    if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(40) }))
                    {
                        if (uiData.backups == null) uiData.backups = new List<ushort>();
                        uiData.backups.Add(0);
                    }
                    GUILayout.EndHorizontal();
                    if (m_bExpandBackups)
                    {
                        EditorGUI.indentLevel++;
                        if (uiData.backups != null)
                        {
                            for (int i = 0; i < uiData.backups.Count; ++i)
                            {
                                GUILayout.BeginHorizontal();
                                uiData.backups[i] = (ushort)(EUIType)HandleUtilityWrapper.PopEnum("UI类型", (EUIType)uiData.backups[i]);
                                if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(40) }))
                                {
                                    uiData.backups.RemoveAt(i);
                                    break;
                                }
                                GUILayout.EndHorizontal();
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                }
                

                if (GUILayout.Button("删除"))
                {
                    if (EditorUtility.DisplayDialog("提示", "确定删除", "删除", "取消"))
                    {
                        var selects = m_pTreeView.GetSelection();
                        if (selects !=null && selects.Count>1)
                        {
                            for(int i =0; i < selects.Count; ++i)
                            {
                                var data = m_pTreeView.GetItem(selects[i]) as ElementItem;
                                if(data!=null)
                                {
                                    m_vConfigs.Remove(data.pData);
                                }
                            }
                            RefreshList();
                        }
                        else
                            m_vConfigs.Remove(uiData);
                        RefreshList();
                    }
                }
            }
        }
        //-----------------------------------------------------
        void RefreshList()
        {
            if (m_pTreeView == null) return;
            m_pTreeView.BeginTreeData();
            for(int i = 0; i < m_vConfigs.Count; ++i)
            {
                ElementItem item = new ElementItem();
                item.pData = m_vConfigs[i];
                item.id = i;
                item.name = TopGame.ED.EditorHelp.GetDisplayName((EUIType)item.pData.type);
                m_pTreeView.AddData(item);
            }
            m_pTreeView.EndTreeData();
        }
        //-----------------------------------------------------
        void OnElementSelect(TreeAssetView.ItemData data)
        {
            m_Select = data as ElementItem;
        }
        //-----------------------------------------------------
        bool OnElementCellGUI(Rect cellRect, TreeAssetView.TreeItemData item, int column, bool bSelected, bool focused)
        {
            ElementItem data = item.data as ElementItem;
            if (column == 0)
            {
                TreeView.DefaultGUI.Label(cellRect, data.name + "[" + data.pData.type + "]", bSelected, focused);
            }
            else if (column == 1)
            {
                GUI.Label(cellRect, data.pData.Order.ToString());
            }
            else if (column == 2)
            {
                GUI.Label(cellRect, data.pData.uiZValue.ToString());
            }
            return true;
        }
    }
}
#endif