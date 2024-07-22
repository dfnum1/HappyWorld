#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GameSystem
作    者:	HappLI
描    述:	UI 模版
*********************************************************************/

using Framework.ED;
using System.Collections.Generic;
using System.IO;
using TopGame.ED;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TopGame.UI
{
    [System.Serializable]
    public class ThemeUIData
    {
        [System.Serializable]
        public class Item
        {
            public string strDesc;
            public string prefabID;
            [System.NonSerialized]
            public GameObject prefab;
        }

        public List<Item> themes = new List<Item>();
        public int AddPrefab(GameObject pPrefab)
        {
            if (pPrefab == null) return 0;
            string path = AssetDatabase.GetAssetPath(pPrefab.GetInstanceID());
            if (path == null)
            {
                Debug.LogWarning(pPrefab.name + "不是有效的预制体!");
                return 0;
            }
            string guid = AssetDatabase.AssetPathToGUID(path);
            for(int i = 0; i < themes.Count; ++i)
            {
                if(guid.CompareTo(themes[i].prefabID) == 0)
                {
                    return 1;
                }
            }
            Item item = new Item();
            item.prefabID = guid;
            item.strDesc = pPrefab.name;
            themes.Add(item);
            return 2;
        }

        public static ThemeUIData Load(string strFile = null)
        {
            if(strFile == null)
                strFile = Application.dataPath + "/../EditorConfigs/Editor/UIThemes.json";
            if (System.IO.File.Exists(strFile))
            {
                return UnityEngine.JsonUtility.FromJson<ThemeUIData>(System.IO.File.ReadAllText(strFile));
            }
            return null;
        }
        public void Save(string strFile = null)
        {
            if(strFile == null)
                strFile = Application.dataPath + "/../EditorConfigs/Editor/UIThemes.json";
            string strDir = System.IO.Path.GetDirectoryName(strFile);
            if(!System.IO.Directory.Exists(strDir))
            {
                System.IO.Directory.CreateDirectory(strDir);
            }
            FileStream fs = new FileStream(strFile, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            fs.Position = 0;
            fs.SetLength(0);
            writer.Write(JsonUtility.ToJson(this, true));
            writer.Close();
        }
    }
    class ThemeWidgetWindow : EditorWindow
    {
        class ElementItem : TreeAssetView.ItemData
        {
            public ThemeUIData.Item themeItem;
            public override Color itemColor()
            {
                if (string.IsNullOrEmpty(themeItem.prefabID))
                    return Color.red;
                if(themeItem.prefab == null)
                {
                    themeItem.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(themeItem.prefabID));
                }
                if (themeItem.prefab == null) return Color.red;
                return Color.white;
            }
        }
        static ThemeWidgetWindow ms_pInstance;
        public static void OpenWindow(Transform parent)
        {
            if (ms_pInstance != null)
            {
                ms_pInstance.m_SelectParent = parent;
                return;
            }
            ms_pInstance = EditorWindow.GetWindow<ThemeWidgetWindow>();
            ms_pInstance.m_SelectParent = parent;
            ms_pInstance.titleContent = new GUIContent("ui 模版控件");
#if UNITY_2019_4_OR_NEWER
            ms_pInstance.ShowModal();
#else
            ms_pInstance.ShowUtility();
#endif
        }
        Transform m_SelectParent;
        TreeAssetView m_pTreeView;
        ElementItem m_SelectItem = null;
        ThemeUIData m_ThemeData = null;
        //------------------------------------------------------
        void OnEnable()
        {
            MultiColumnHeaderState.Column[] colums = new MultiColumnHeaderState.Column[]
                            {
                    new MultiColumnHeaderState.Column(),
                            };

            colums[0].headerContent = new GUIContent("描述", "");
            colums[0].minWidth = position.width/2;
            colums[0].width = position.width / 2;
            colums[0].maxWidth = position.width / 2;
            colums[0].headerTextAlignment = TextAlignment.Center;
            colums[0].canSort = false;
            colums[0].autoResize = true;


            var headerState = new MultiColumnHeaderState(colums);

            TreeViewState pTreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
            m_pTreeView = new TreeAssetView(pTreeState, headerState);
            m_pTreeView.Reload();

            m_pTreeView.OnItemDoubleClick = OnElementDSelect;
            m_pTreeView.OnSelectChange = OnElementSelect;
            m_pTreeView.OnCellDraw += OnElementCellGUI;

            m_ThemeData = ThemeUIData.Load();
            if(m_ThemeData==null)
                m_ThemeData = new ThemeUIData();

            RefreshList();
        }
        //------------------------------------------------------
        void OnDisable()
        {
            m_ThemeData.Save();
        }
        //------------------------------------------------------
        void RefreshList()
        {
            if (m_pTreeView == null) return;
            m_pTreeView.BeginTreeData();
            for(int i = 0; i < m_ThemeData.themes.Count; ++i)
            {
                if (string.IsNullOrEmpty(m_ThemeData.themes[i].prefabID))
                    continue;
                ElementItem item = new ElementItem();
                item.id = i;
                item.name = m_ThemeData.themes[i].strDesc;
                item.path = AssetDatabase.GUIDToAssetPath(m_ThemeData.themes[i].prefabID);
                item.themeItem = m_ThemeData.themes[i];
                item.themeItem.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(item.path);
                m_pTreeView.AddData(item);
            }
            m_pTreeView.EndTreeData();
        }
        //------------------------------------------------------
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, position.width / 2, position.height));
            GUILayout.BeginVertical();
            if(m_pTreeView!=null)
            {
                m_pTreeView.searchString = EditorGUILayout.TextField("搜索", m_pTreeView.searchString, new GUILayoutOption[] { GUILayout.Height(20) });
                m_pTreeView.OnGUI(new Rect(0, 0, position.width / 2, position.height - 20));
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();


            GUILayout.BeginArea(new Rect(position.width / 2, 0, position.width / 2, position.height));
            GUILayout.BeginVertical();
            if(m_SelectItem!=null)
            {
                EditorGUILayout.LabelField("描述:");
                m_SelectItem.themeItem.strDesc = EditorGUILayout.TextArea( m_SelectItem.themeItem.strDesc);
                if(m_SelectItem.themeItem.prefab == null && !string.IsNullOrEmpty(m_SelectItem.themeItem.prefabID))
                    m_SelectItem.themeItem.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(m_SelectItem.themeItem.prefabID));
                m_SelectItem.themeItem.prefab = EditorGUILayout.ObjectField("模版预制", m_SelectItem.themeItem.prefab,typeof(GameObject), false) as GameObject;
                if (m_SelectItem.themeItem.prefab != null)
                    m_SelectItem.themeItem.prefabID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(m_SelectItem.themeItem.prefab));

                if(GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(position.width/2) }))
                {
                    if(EditorUtility.DisplayDialog("提示", "是否确认删除", "删除", "取消"))
                    {
                        m_ThemeData.themes.Remove(m_SelectItem.themeItem);
                        RefreshList();
                    }
                }
                if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(position.width / 2) }))
                {
                    if (EditorUtility.DisplayDialog("提示", "是否确认删除", "删除", "取消"))
                    {
                        m_ThemeData.themes.Add(new ThemeUIData.Item() { strDesc ="", prefabID = null });
                        RefreshList();
                    }
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        void OnElementSelect(TreeAssetView.ItemData data)
        {
            m_SelectItem = data as ElementItem;
        }
        //-----------------------------------------------------
        void OnElementDSelect(TreeAssetView.ItemData data)
        {
            m_SelectItem = data as ElementItem;
            if (string.IsNullOrEmpty(m_SelectItem.themeItem.prefabID))
            {
                EditorUtility.DisplayDialog("提示", "为无效预制模版", "好的");
                return;
            }
            if(m_SelectItem.themeItem.prefab == null)
            {
                m_SelectItem.themeItem.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(m_SelectItem.themeItem.prefabID));
            }
            if (m_SelectItem.themeItem.prefab == null)
            {
                EditorUtility.DisplayDialog("提示", "为无效预制模版", "好的");
                return;
            }
            GameObject uiInstance = PrefabUtility.InstantiatePrefab(m_SelectItem.themeItem.prefab) as GameObject;
            uiInstance.transform.position = Vector3.zero;
            uiInstance.transform.rotation = Quaternion.identity;
            if (m_SelectParent != null)
                uiInstance.transform.SetParent(m_SelectParent);
            uiInstance.transform.localScale = Vector3.one;
            uiInstance.transform.position = Vector3.zero;
            uiInstance.transform.rotation = Quaternion.identity;
            Selection.activeObject = uiInstance;
            this.Close();
        }
        //-----------------------------------------------------
        bool OnElementCellGUI(Rect cellRect, TreeAssetView.TreeItemData item, int column, bool bSelected, bool focused)
        {
            ElementItem data = item.data as ElementItem;
            if (column == 0)
            {
                TreeView.DefaultGUI.Label(cellRect, data.name + "[" + data.id + "]", bSelected, focused);
            }
            return true;
        }
    }
}
#endif