#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using System;

namespace TopGame.ED
{
    //------------------------------------------------------
    public partial class SearcherMenu
    {
        public interface ParamData
        {
            Texture2D getIcon();
            System.Action<object,bool> getCallback();
            void setCallback(System.Action<object,bool> callback);

            string getName();
        }

        enum EState
        {
            Open,
            Close,
        }
        public Rect inspectorRect = new Rect(1, 22, 120, 50);
        Texture2D m_pBTest = null;

        EState m_nState = EState.Close;
        private Vector2 m_scrollPosition;
        string m_searchString = "";
        private SearchTree m_assetTree;
        private SearchTreeIMGUI m_assetTreeGUI;

        private List<ParamData> m_vDatas = new List<ParamData>();
        private Dictionary<string, ParamData> m_vEvents = new Dictionary<string, ParamData>();
        //------------------------------------------------------
        public bool isOpen()
        {
            return m_nState == EState.Open;
        }
        //------------------------------------------------------
        public void Open(Rect rect)
        {
            inspectorRect = rect;
            if (m_nState == EState.Open) return;
            m_nState = EState.Open;
            Init();
            Search(m_searchString);
        }
        //------------------------------------------------------
        public void Close()
        {
            if (m_nState == EState.Close) return;
            m_nState = EState.Close;
        }
        //--------------------------------------------------
        public bool IsMouseIn(Vector2 mouse)
        {
            if (m_nState == EState.Open && inspectorRect.Contains(mouse)) return true;
            return false;
        }
        //------------------------------------------------------
        void Init()
        {
            if(m_assetTree == null)
            {
                m_assetTree = new SearchTree();
                m_assetTreeGUI = new SearchTreeIMGUI(m_assetTree.Root);
                m_assetTreeGUI.onSelected = OnSelected;
                m_assetTreeGUI.onDoubleClick = OnDoubleSelected;
                m_assetTreeGUI.onDraw = OnDrawItem;
            }
        }
        //------------------------------------------------------
        void OnSelected(AssetData data, bool bSelected)
        {
            EditorGUI.FocusTextInControl(null);
            if (data.guid == null) return;
            ParamData evt;
            if (m_vEvents.TryGetValue(data.guid, out evt) && evt.getCallback() != null)
            {
                evt.getCallback()(evt, false);
            }
            Close();
        }
        //------------------------------------------------------
        void OnDoubleSelected(AssetData data)
        {
            EditorGUI.FocusTextInControl(null);
            if (data.guid == null) return;

            ParamData evt;
            if (m_vEvents.TryGetValue(data.guid, out evt) && evt.getCallback() != null)
            {
                evt.getCallback()(evt, true);
            }
            Close();
        }
        //------------------------------------------------------
        void OnDrawItem(AssetData data)
        {
            ParamData evt;
            if(data.icon ==null && m_vEvents.TryGetValue(data.guid, out evt))
            {
                if(evt.getIcon()!=null)
                    data.icon = evt.getIcon();
            }
        }
        //------------------------------------------------------
        public void OnDraw()
        {
            if (m_nState != EState.Open) return;
            Init();

            GUILayout.Window(0, inspectorRect, OnGUI, "查询");
        }
        //------------------------------------------------------
        public void Update(float fTime)
        {
            if(Event.current!=null)
            {
                m_assetTreeGUI.OnEvent(Event.current);
            }
        }
        //------------------------------------------------------
        private void OnGUI(int id)
        {
            if (m_pBTest == null)
            {
                m_pBTest = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                for (int x = 0; x < 2; ++x)
                {
                    for (int z = 0; z < 2; ++z)
                    {
                        m_pBTest.SetPixel(x, z, new Color(1, 1, 1, 0.25f));
                    }
                }
                m_pBTest.Apply();
            }
            // draw search
            GUI.DrawTexture(new Rect(0,0, inspectorRect.width, inspectorRect.height), m_pBTest);
            GUI.BringWindowToFront(id);
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            EditorGUILayout.LabelField("Search:", EditorStyles.miniLabel, GUILayout.Width(40));

            EditorGUI.BeginChangeCheck();

            //搜索栏聚焦
     //       GUI.SetNextControlName("search");
            m_searchString = EditorGUILayout.TextField(m_searchString, EditorStyles.toolbarTextField, GUILayout.ExpandWidth(true));
     //       GUI.FocusControl("search");
            
            if (EditorGUI.EndChangeCheck())
            {
                Search(m_searchString);
            }

            EditorGUILayout.EndHorizontal();

            // draw tree
            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);

            m_assetTreeGUI.DrawTreeLayout();

            EditorGUILayout.EndScrollView();

     //       GUILayout.EndArea();
        }
        //------------------------------------------------------
        bool IsQuery(string query, string strContext)
        {
            if (string.IsNullOrEmpty(query)) return true;
            if (string.IsNullOrEmpty(strContext)) return false;
            if(strContext.Length > query.Length)
            {
                return strContext.ToLower().Contains(query.ToLower());
            }
            return query.ToLower().Contains(strContext.ToLower());
        }
        //------------------------------------------------------
        public void BeginData()
        {
            m_vDatas.Clear();
        }
        //------------------------------------------------------
        public void AddData(ParamData pData)
        {
            if (string.IsNullOrEmpty(pData.getName())) return;
            m_vDatas.Add(pData);
        }
        //------------------------------------------------------
        public void EndData()
        {
            Search(m_searchString);
        }
        //------------------------------------------------------
        private void Search(string query)
        {
            m_vEvents.Clear();
            m_assetTree.Clear();

            HashSet<char> vQuery = new HashSet<char>();
            for (int i = 0; i < query.Length; ++i)
                vQuery.Add(query[i]);

            int id = 0;
            foreach (var db in m_vDatas)
            {
                bool bQuerty = IsQuery(query, db.getName());
                if (!bQuerty) continue;
                m_assetTree.AddUser(id.ToString(), db.getName(), vQuery.Count > 0);

                m_vEvents.Add(id.ToString(), db);
                id++;
            }
        }
    }
}
#endif