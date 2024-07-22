
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;
using UnityEngine.Playables;
using TopGame.Data;
using TopGame.Core;
using TopGame.Base;
using Framework.Module;
using System.IO;
using UnityEditor.IMGUI.Controls;
using Framework.ED;

namespace TopGame.ED
{
    public class TextureCliperEditorLogic
    {
        enum EDragType
        {
            DragPointL_T = 0,
            DragPointR_T,
            DragPointL_B,
            DragPointR_B,
            DragGrid,
            DragRect,
            None,
        }
        private static float POINT_DOT_SIZE = 10;

        public class AssetItem : TreeAssetView.ItemData
        {
            public System.Object pData;
            public override Color itemColor()
            {
                if(pData is TextureCliperAssets.ClipGroup)
                    return Color.green;
                return Color.white;
            }

            public override Texture2D itemIcon()
            {
                return EditorGUIUtility.IconContent("DefaultAsset Icon").image as Texture2D;
            }
        }

        bool m_bDirtyList = false;
        List<TextureCliperAssets.ClipGroup> m_vGroups = new List<TextureCliperAssets.ClipGroup>();
        AssetItem m_pCurItem = null;

        TextureCliperEditor m_pEditor;
        Texture m_zoomDot = null;

        Data.TextureCliperAssets m_Assets = null;

        Rect m_PreView = new Rect();
        Vector2 m_Scroll = Vector2.zero;
        float m_zoom = 1;
        EDragType m_bDraging = EDragType.None;
        private Vector2 m_dragBoxStart;
        private Rect m_selectionBox;
        private Rect m_ClipBox = new Rect();
        private bool m_bMouseDown = false;

        private Rect m_TextureSize = new Rect();

        UnityEditor.IMGUI.Controls.TreeViewState m_TreeState;
        MultiColumnHeaderState m_AssetListMCHState;
        TreeAssetView m_pTreeView = null;

        string m_EditorCatch="";
        TextureCliperGroupCatch m_Catch = new TextureCliperGroupCatch();
        //-----------------------------------------------------
        public void OnEnable(TextureCliperEditor pEditor)
        {
            m_EditorCatch = Application.dataPath + "/../EditorData/TextureCliperCatch.json";
            m_zoomDot = Resources.Load("zoom_dot") as Texture;
            m_Catch.Load(m_EditorCatch);
            MultiColumnHeaderState.Column[] colums = new MultiColumnHeaderState.Column[]
            {
             new MultiColumnHeaderState.Column(),
             new MultiColumnHeaderState.Column(),
            };

            colums[0].headerContent = new GUIContent("name", "");
            colums[0].minWidth = 100;
            colums[0].width = 100;
            colums[0].maxWidth = 100;
            colums[0].headerTextAlignment = TextAlignment.Left;
            colums[0].canSort = true;
            colums[0].autoResize = true;

            colums[1].headerContent = new GUIContent("", "");
            colums[1].minWidth = 150;
            colums[1].width = 150;
            colums[1].maxWidth = 150;
            colums[1].headerTextAlignment = TextAlignment.Left;
            colums[1].canSort = false;
            colums[1].autoResize = true;

            var headerState = new MultiColumnHeaderState(colums);
            m_AssetListMCHState = headerState;

            m_TreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
            m_pTreeView = new TreeAssetView(m_TreeState);
            m_pTreeView.Reload();

            m_pTreeView.OnItemDoubleClick = OnSelect;
            m_pTreeView.OnSelectChange = OnSelect;
            m_pTreeView.OnCellDraw += OnCellGUI;

            RefreshList();
            m_pEditor = pEditor;
        }
        //-----------------------------------------------------
        private void LoadCatch()
        {

        }
        //-----------------------------------------------------
        public void OnDisable()
        {
            m_Catch.Save(m_vGroups, m_EditorCatch);
            Clear();
            ExportClipTexture.Hide();
        }
        //-----------------------------------------------------
        void RefreshList(bool bSyncAssests = true)
        {
            if(bSyncAssests && m_Assets != null)
            {
                m_vGroups = new List<TextureCliperAssets.ClipGroup>(m_Assets.groups);
            }
            if(m_pTreeView != null)
            {
                m_pTreeView.BeginTreeData();

                int index = 0;
                for (int i = 0; i < m_vGroups.Count; ++i)
                {
                    TextureCliperAssets.ClipGroup clipGroup = m_vGroups[i];
                    if(clipGroup.catchData == null)
                        clipGroup.catchData = m_Catch.GetGroupCatch(clipGroup.name);
                    AssetItem item = new AssetItem();
                    item.pData = clipGroup;
                    item.id = index++;
                    item.name = clipGroup.name;
                    item.depth = 0;
                    m_pTreeView.AddData(item);

                    if (clipGroup.clips != null)
                    {
                        for (int j = 0; j < clipGroup.clips.Length; ++j)
                        {
                            clipGroup.clips[j].group = clipGroup;
                            AssetItem sub_item = new AssetItem();
                            sub_item.pData = clipGroup.clips[j];
                            sub_item.id = index++;
                            sub_item.parent = item;
                            sub_item.name = clipGroup.clips[j].asset;
                            sub_item.depth = 1;
                            m_pTreeView.AddData(sub_item);
                        }
                    }

                }

                m_pTreeView.EndTreeData();
            }
        }
        //-----------------------------------------------------
        public void Clear()
        {
        }
        //-----------------------------------------------------
        public void Update(float fFrameTime)
        {
            if(m_bDirtyList)
            {
                RefreshList(false);
                m_bDirtyList = false;
            }
            CalcSelectBox();
        }
        //-----------------------------------------------------
        public void OnEvent(Event evt)
        {
            if (evt.type == EventType.MouseDown)
            {
                if (m_PreView.Contains(evt.mousePosition))
                {
                    m_bMouseDown = evt.button == 0;
                    Vector2 mousePos = evt.mousePosition;
                    Rect point = new Rect();
                    point.size = new Vector2(POINT_DOT_SIZE*2, POINT_DOT_SIZE*2);
                    if (evt.shift)
                        m_bDraging = EDragType.DragGrid;
                    else m_bDraging = EDragType.None;
                    if(m_selectionBox.Contains(mousePos))
                    {
                        m_bDraging = EDragType.DragRect;
                    }
                    point.center = new Vector2(m_selectionBox.xMin, m_selectionBox.yMin);
                    if (point.Contains(mousePos)) m_bDraging = EDragType.DragPointL_T;

                    point.center = new Vector2(m_selectionBox.xMax, m_selectionBox.yMin);
                    if (point.Contains(mousePos)) m_bDraging = EDragType.DragPointR_T;
                    point.center = new Vector2(m_selectionBox.xMax, m_selectionBox.yMax);
                    if (point.Contains(mousePos)) m_bDraging = EDragType.DragPointR_B;
                    point.center = new Vector2(m_selectionBox.xMin, m_selectionBox.yMax);
                    if (point.Contains(mousePos)) m_bDraging = EDragType.DragPointL_B;
                    m_dragBoxStart = WindowToGridPosition(mousePos);
                }
            }
            else if (evt.type == EventType.MouseDrag)
            {
                if(evt.button == 0)
                {
                    if (m_bDraging == EDragType.DragGrid)
                    {
                        Vector2 boxStartPos = GridToWindowPosition(m_dragBoxStart);
                        Vector2 boxSize = evt.mousePosition - boxStartPos;
                        if (boxSize.x < 0) { boxStartPos.x += boxSize.x; boxSize.x = Mathf.Abs(boxSize.x); }
                        if (boxSize.y < 0) { boxStartPos.y += boxSize.y; boxSize.y = Mathf.Abs(boxSize.y); }

                        Vector2 left = boxStartPos;
                        left.x /= Mathf.Max(1, m_TextureSize.width);
                        left.y /= Mathf.Max(1, m_TextureSize.height);
                        m_selectionBox = new Rect(boxStartPos, boxSize);

                        CalcClipBox(m_selectionBox);
                        m_pEditor.Repaint();
                    }
                    else if(m_bDraging == EDragType.DragRect)
                    {
                        m_selectionBox.center += evt.delta;
                        CalcClipBox(m_selectionBox);
                    }
                    else if (m_bDraging == EDragType.DragPointL_T)
                    {
                        m_selectionBox.xMin += evt.delta.x;
                        m_selectionBox.yMin += evt.delta.y;
                        CalcClipBox(m_selectionBox);
                    }
                    else if (m_bDraging == EDragType.DragPointR_T)
                    {
                        m_selectionBox.xMax += evt.delta.x;
                        m_selectionBox.yMin += evt.delta.y;
                        CalcClipBox(m_selectionBox);
                    }
                    else if (m_bDraging == EDragType.DragPointR_B)
                    {
                        m_selectionBox.xMax += evt.delta.x;
                        m_selectionBox.yMax += evt.delta.y;
                        CalcClipBox(m_selectionBox);
                    }
                    else if (m_bDraging == EDragType.DragPointL_B)
                    {
                        m_selectionBox.xMin += evt.delta.x;
                        m_selectionBox.yMax += evt.delta.y;
                        CalcClipBox(m_selectionBox);
                    }
                }
                else if(evt.button == 2)
                {
                    m_Scroll += evt.delta;
                }
            }
            else if (evt.type == EventType.MouseUp)
            {
                if (m_bDraging != EDragType.None)
                {
                    m_pEditor.Repaint();
                    m_bDraging = EDragType.None;
                }
                if(evt.button == 0)
                    m_bMouseDown = false;
            }
            else if (evt.type == EventType.ScrollWheel)
            {
                if (m_PreView.Contains(evt.mousePosition))
                {
                    float oldZoom = m_zoom;
                    if (evt.delta.y > 0) m_zoom += 0.1f * m_zoom;
                    else m_zoom -= 0.1f * m_zoom;
                }
            }
            else if(evt.type == EventType.KeyDown)
            {
                if(evt.keyCode == KeyCode.Escape)
                {
                    m_selectionBox.size = Vector2.zero;
                }
            }
        }
        //-----------------------------------------------------
        public void SaveData()
        {
            HashSet<string> vName = new HashSet<string>();
            HashSet<string> vSubName = new HashSet<string>();
            HashSet<uint> vClipHash = new HashSet<uint>();
            for (int i = 0; i < m_vGroups.Count; ++i)
            {
                vSubName.Clear();
                if(string.IsNullOrEmpty(m_vGroups[i].name))
                {
                    EditorUtility.DisplayDialog("提示", "组别名称不能为空!", "好的");
                    return;
                }
                if (vName.Contains(m_vGroups[i].name))
                {
                    EditorUtility.DisplayDialog("提示", "组别名称\"" + m_vGroups[i].name + "\"同名,请修改!", "好的");
                    return;
                }
                if (vClipHash.Contains(m_vGroups[i].clipId))
                {
                    EditorUtility.DisplayDialog("提示", "组别\"" + m_vGroups[i].name + "\"同ID,请修改!", "好的");
                    return;
                }
                vClipHash.Add(m_vGroups[i].clipId);

                vName.Add(m_vGroups[i].name);
                if (m_vGroups[i].clips == null) continue; 
                for(int j = 0; j < m_vGroups[i].clips.Length; ++j)
                {
                    if (string.IsNullOrEmpty(m_vGroups[i].clips[j].asset))
                    {
                        EditorUtility.DisplayDialog("提示", "组别\"" + m_vGroups[i].name + "\" 的子节点名称不能为空!", "好的");
                        return;
                    }
                    if (vSubName.Contains(m_vGroups[i].clips[j].asset))
                    {
                        EditorUtility.DisplayDialog("提示", "组别\"" + m_vGroups[i].name + "\" 子节点\""+ m_vGroups[i].clips[j].asset + "\"同名,请修改!", "好的");
                        return;
                    }
                    vSubName.Add(m_vGroups[i].clips[j].asset);
                }
            }
            m_Catch.Save(m_vGroups,m_EditorCatch);
            if (m_Assets!=null)
            {
                m_Assets.groups = m_vGroups.ToArray();
                EditorUtility.SetDirty(m_Assets);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
        //-----------------------------------------------------
        void OnSelect(TreeAssetView.ItemData data)
        {
            AssetItem item = data as AssetItem;
            OnChangeSelect(item);
        }
        //-----------------------------------------------------
        public void Reset()
        {
            m_Scroll = Vector2.zero;
            m_zoom = 1;
        }
        //-----------------------------------------------------
        void OnChangeSelect(AssetItem item)
        {
            if (item == m_pCurItem)
                return;
     //       Reset();
            Clear();
            m_pCurItem = item;

            Rect source = new Rect();
            TextureCliperAssets.ClipGroup clipGroup = m_pCurItem.pData as TextureCliperAssets.ClipGroup;
            TextureCliperAssets.Clip clipItem = m_pCurItem.pData as TextureCliperAssets.Clip;
            if (clipItem != null)
            {
                clipGroup = clipItem.group;
                source = clipItem.source;
            }
            if (clipGroup == null) return;
            clipGroup.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(clipGroup.path);
            if(clipGroup.texture !=null)
            {
                RefreshClipRect(clipGroup.texture, source);
            }
            else
            {
                m_selectionBox = Rect.zero;
            }
        }
        //------------------------------------------------------
        void RefreshClipRect(Texture2D texture, Rect source)
        {
            if (texture != null)
            {
                m_ClipBox.x = source.x / Mathf.Max(1, texture.width);
                m_ClipBox.y = source.y / Mathf.Max(1, texture.height);
                m_ClipBox.width = source.width / Mathf.Max(1, texture.width);
                m_ClipBox.height = source.height / Mathf.Max(1, texture.height);
                CalcSelectBox();
            }
        }
        //-----------------------------------------------------
        bool OnCellGUI(Rect cellRect, TreeAssetView.TreeItemData item, int column, bool bSelected, bool focused)
        {
            AssetItem assetItem = item.data as AssetItem;
            TextureCliperAssets.ClipGroup group = assetItem.pData as TextureCliperAssets.ClipGroup;
            if (group != null)
            {
                item.displayName = group.name;
                if (GUI.Button(new Rect(cellRect.width - 80, cellRect.y, 40, cellRect.height), "添加"))
                {
                    List<TextureCliperAssets.Clip> vClip = group.clips != null ? new List<TextureCliperAssets.Clip>(group.clips) : new List<TextureCliperAssets.Clip>();
                    vClip.Add(new TextureCliperAssets.Clip() { asset = vClip.Count.ToString() });
                    group.clips = vClip.ToArray();
                    m_bDirtyList = true;
                }
                if (GUI.Button(new Rect(cellRect.width - 40, cellRect.y, 40, cellRect.height), "移除"))
                {
                    if (EditorUtility.DisplayDialog("提示", "确实要移除?", "删除", "取消"))
                    {
                        m_vGroups.Remove(group);
                        m_bDirtyList = true;
                    }
                }
            }
            TextureCliperAssets.Clip clipItem = assetItem.pData as TextureCliperAssets.Clip;
            if (clipItem != null && clipItem.group!=null)
            {
                clipItem.group.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(clipItem.group.path);
                item.displayName = clipItem.asset;
                if (clipItem.group.texture != null && GUI.Button(new Rect(cellRect.width - 80, cellRect.y, 40, cellRect.height), "导出"))
                {
                    ExportClipTexture.Show(clipItem, 300);
                }
                if (GUI.Button(new Rect(cellRect.width - 40, cellRect.y, 40, cellRect.height), "移除"))
                {
                    if (EditorUtility.DisplayDialog("提示", "确实要删除?", "删除", "取消"))
                    {
                        List<TextureCliperAssets.Clip> vClip = clipItem.group.clips != null ? new List<TextureCliperAssets.Clip>(clipItem.group.clips) : new List<TextureCliperAssets.Clip>();
                        vClip.Remove(clipItem);
                        clipItem.group.clips = vClip.ToArray();
                        m_bDirtyList = true;
                    }
                }
            }
            return false;
        }
        //-----------------------------------------------------
        public void OnGUI()
        {
            if (m_pEditor == null) return;
            DrawSelectionBox();
        }
        //-----------------------------------------------------
        public TextureCliperAssets.Clip GetCurClip()
        {
            TextureCliperAssets.Clip clipItem = m_pCurItem.pData as TextureCliperAssets.Clip;
            if (clipItem != null)
            {
                return clipItem;
            }
            return null;
        }
        //-----------------------------------------------------
        public TextureCliperAssets.ClipGroup GetCurGroup()
        {
            TextureCliperAssets.ClipGroup clipGroup = m_pCurItem.pData as TextureCliperAssets.ClipGroup;
            if(clipGroup == null)
            {
                TextureCliperAssets.Clip clipItem = m_pCurItem.pData as TextureCliperAssets.Clip;
                if (clipItem != null)
                {
                    clipGroup = clipItem.group;
                }
            }
            return clipGroup;
        }
        //-----------------------------------------------------
        public void DrawInspectorPanel(Rect view)
        {
            if (m_pCurItem == null) return;
            Rect lastRect= new Rect();
            GUILayout.BeginArea(view);
            m_Catch.GetCatch().zoomColor = EditorGUILayout.ColorField("区域颜色", m_Catch.GetCatch().zoomColor);
            TextureCliperAssets.ClipGroup clipGroup =  m_pCurItem.pData as TextureCliperAssets.ClipGroup;
            if(clipGroup!=null)
            {
                clipGroup = (TextureCliperAssets.ClipGroup)HandleUtilityWrapper.DrawProperty(clipGroup, null);
            }
            TextureCliperAssets.Clip clipItem = m_pCurItem.pData as TextureCliperAssets.Clip;
            if (clipItem != null)
            {
                Rect preRect = clipItem.source;
                clipItem = (TextureCliperAssets.Clip)HandleUtilityWrapper.DrawProperty(clipItem, null);
                clipGroup = clipItem.group;
                if (preRect != clipItem.source)
                    RefreshClipRect(clipGroup.texture, clipItem.source);
            }
            if (clipGroup == null)
            {
                GUILayout.EndArea();
                return;
            }

            if (clipGroup.catchData != null)
            {
                TextureCliperGroupCatch.GroupData editData = clipGroup.catchData as TextureCliperGroupCatch.GroupData;
                if (editData != null)
                {
                    editData.group = clipGroup.name;
                    clipGroup.catchData = HandleUtilityWrapper.DrawProperty(editData, null);
                }
            }

            EditorGUI.BeginChangeCheck();
            clipGroup.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(clipGroup.path);
            if (clipGroup.texture)
            {
                GUILayout.Label("图片路径:" + clipGroup.path);
                GUILayout.Label("图片大小:" + clipGroup.texture.width + "x" + clipGroup.texture.height);
            }
            if(EditorGUI.EndChangeCheck())
            {
                
            }
            lastRect = GUILayoutUtility.GetLastRect();
            GUILayout.EndArea();
            if (clipGroup.texture)
            {
                lastRect.y += view.y+20;
                m_PreView = new Rect(view.x+2, lastRect.y, view.width-2, view.height - lastRect.y - 5+50);
                GUI.BeginClip(m_PreView);
                DrawGrid(new Rect(0,0,m_PreView.width, m_PreView.height), m_zoom, m_Scroll);
                DrawTexture(clipGroup.texture, new Rect(0, 0, m_PreView.width, m_PreView.height), m_zoom, m_Scroll);
                GUI.EndClip();
            }
        }
        //-----------------------------------------------------
        public void OnDrawLayerPanel(Rect rc)
        {
            if (m_pTreeView == null) return;
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("新建组", new GUILayoutOption[] { GUILayout.Width(80) }))
            {
                m_vGroups.Add(new TextureCliperAssets.ClipGroup() { name = m_vGroups.Count.ToString() });
                RefreshList(false);
            }
            m_AssetListMCHState.columns[0].width = rc.width / 2;
            m_AssetListMCHState.columns[1].width = rc.width / 2;
            m_pTreeView.searchString = EditorGUILayout.TextField("过滤", m_pTreeView.searchString);
            GUILayout.EndHorizontal();
            m_pTreeView.OnGUI(new Rect(0, 20, rc.size.x, rc.size.y - 20));
        }
        //-----------------------------------------------------
        public void OnReLoadAssetData()
        {
            InitAsset();
        }
        //-----------------------------------------------------
        public void InitAsset()
        {
            m_Assets = AssetDatabase.LoadAssetAtPath<TextureCliperAssets>(TextureCliperEditor.CLIP_PATH);
            RefreshList();
        }
        //-----------------------------------------------------
        public void DrawGrid(Rect rect, float zoom, Vector2 panOffset)
        {
            Vector2 center = rect.size / 2f;
            Texture2D gridTex = EditorUtil.GenerateGridTexture(Color.gray, new Color(0.3f, 0.3f, 0.3f, 1f));
            Texture2D crossTex = EditorUtil.GenerateCrossTexture(new Color(0.45f, 0.45f, 0.45f));

            // Offset from origin in tile units
            float xOffset = -(center.x * zoom) / gridTex.width;
            float yOffset = ((center.y - rect.size.y) * zoom) / gridTex.height;

            Vector2 tileOffset = new Vector2(xOffset, yOffset);

            // Amount of tiles
            float tileAmountX = Mathf.Round(rect.size.x * zoom) / gridTex.width;
            float tileAmountY = Mathf.Round(rect.size.y * zoom) / gridTex.height;

            Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);

            // Draw tiled background
            GUI.DrawTextureWithTexCoords(rect, gridTex, new Rect(tileOffset, tileAmount));
            GUI.DrawTextureWithTexCoords(rect, crossTex, new Rect(tileOffset + new Vector2(0.5f, 0.5f), tileAmount));
        }
        //-----------------------------------------------------
        public void DrawTexture(Texture2D pTexture, Rect rect, float zoom, Vector2 panOffset)
        {
            m_TextureSize.x = rect.x + panOffset.x;
            m_TextureSize.y = rect.y + panOffset.y;
            m_TextureSize.width = Mathf.RoundToInt(pTexture.width / zoom);
            m_TextureSize.height = Mathf.RoundToInt(pTexture.height / zoom);
            GUI.DrawTextureWithTexCoords(m_TextureSize, pTexture, new Rect(0,0,1,1));
        }
        //------------------------------------------------------
        void CalcClipBox(Rect r)
        {
            Vector2 left = new Vector2(r.x, r.y) - new Vector2(m_TextureSize.x + m_PreView.x, m_TextureSize.y + m_PreView.y);
            left.x /= Mathf.Max(1, m_TextureSize.width);
            left.y /= Mathf.Max(1, m_TextureSize.height);
            m_ClipBox = new Rect(left, new Vector2(r.width / Mathf.Max(1, m_TextureSize.width), r.height / Mathf.Max(1, m_TextureSize.height)));
            CalcClipSource();
        }
        //------------------------------------------------------
        void CalcSelectBox()
        {
            m_selectionBox.width = m_ClipBox.width * m_TextureSize.width;
            m_selectionBox.height = m_ClipBox.height * m_TextureSize.height;
            m_selectionBox.x = m_ClipBox.x * m_TextureSize.width + m_TextureSize.x + m_PreView.x;
            m_selectionBox.y = m_ClipBox.y * m_TextureSize.height + m_TextureSize.y + m_PreView.y;
        }
        //------------------------------------------------------
        void CalcClipSource()
        {
            TextureCliperAssets.ClipGroup clipGroup = GetCurGroup();
            TextureCliperAssets.Clip clip = GetCurClip();
            if (clip != null && clipGroup!=null)
            {
                clipGroup.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(clipGroup.path);
                if (clipGroup.texture == null) return;

                clip.source.x = Mathf.FloorToInt(clipGroup.texture.width* m_ClipBox.x);
                clip.source.y = Mathf.FloorToInt(clipGroup.texture.height * m_ClipBox.y);
                clip.source.width = Mathf.FloorToInt(clipGroup.texture.width * m_ClipBox.width);
                clip.source.height = Mathf.FloorToInt(clipGroup.texture.height * m_ClipBox.height);
            }
        }
        //------------------------------------------------------
        void DrawSelectionBox()
        {
         //   if (m_bDraging)
            {
                Rect r = m_ClipBox;
                if (m_bMouseDown && m_bDraging == EDragType.DragGrid)
                {
                    Vector2 curPos = WindowToGridPosition(Event.current.mousePosition);
                    Vector2 size = curPos - m_dragBoxStart;
                    if (size.sqrMagnitude <= 0) return;
                    r = new Rect(m_dragBoxStart, size);
                    r.position = GridToWindowPosition(r.position);
                    r.size /= m_zoom;
                }
                else
                {
                    r.width = r.width * m_TextureSize.width;
                    r.height = r.height * m_TextureSize.height;
                    r.x = r.x* m_TextureSize.width + m_TextureSize.x + m_PreView.x;
                    r.y = r.y * m_TextureSize.width + m_TextureSize.y + m_PreView.y;
                }
                if (r.size.sqrMagnitude <= 0) return;

                GUI.BeginClip(m_PreView);
                r.x -= m_PreView.x;
                r.y -= m_PreView.y;

                Color color = GUI.color;
                GUI.color = m_Catch.GetCatch().zoomColor;
                EditorUtil.DrawColorBox(r, GUI.color);
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0.5f);
                GUI.DrawTexture(new Rect(r.xMin - POINT_DOT_SIZE / 2, r.yMin - POINT_DOT_SIZE / 2, POINT_DOT_SIZE, POINT_DOT_SIZE), m_zoomDot);
                GUI.DrawTexture(new Rect(r.xMax - POINT_DOT_SIZE / 2, r.yMin - POINT_DOT_SIZE / 2, POINT_DOT_SIZE, POINT_DOT_SIZE), m_zoomDot);
                GUI.DrawTexture(new Rect(r.xMax - POINT_DOT_SIZE / 2, r.yMax - POINT_DOT_SIZE / 2, POINT_DOT_SIZE, POINT_DOT_SIZE), m_zoomDot);
                GUI.DrawTexture(new Rect(r.xMin - POINT_DOT_SIZE / 2, r.yMax - POINT_DOT_SIZE / 2, POINT_DOT_SIZE, POINT_DOT_SIZE), m_zoomDot);
                GUI.color = color;
                GUI.EndClip();
            }
        }
        //------------------------------------------------------
        public Vector2 WindowToGridPosition(Vector2 windowPosition)
        {
            return (windowPosition - (m_pEditor.position.size * 0.5f) - (m_Scroll / m_zoom)) * m_zoom;
        }
        //------------------------------------------------------
        public Vector2 GridToWindowPosition(Vector2 gridPosition)
        {
            return (m_pEditor.position.size * 0.5f) + (m_Scroll / m_zoom) + (gridPosition / m_zoom);
        }
    }
}