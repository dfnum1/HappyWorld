/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	MonsterObjectEditorLogic
作    者:	HappLI
描    述:	关卡编辑器
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;
using System.IO;
using TopGame.Data;
using TopGame.Core;
using UnityEditor.IMGUI.Controls;
using System.Reflection;
using Framework.Core;
using Framework.Base;
using Framework.ED;

namespace TopGame.ED
{
    public class MonsterObjectEditorLogic
    {
        public class ObjectItem : TreeAssetView.ItemData
        {
            public CsvData_Monster.MonsterData pData;
            public Vector4 userData0;
            public Vector4 userData1;
            public Vector4 userData2;
            public override Color itemColor()
            {
                return Color.white;
            }
        }

        UnityEditor.IMGUI.Controls.TreeViewState m_TreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_AssetListMCHState;
        TreeAssetView m_pTreeView = null;

        CsvData_Monster.MonsterData m_pCurItem;

        Vector2 m_Scroll = Vector2.zero;

        GridWireBounds m_pGridWireBox = new GridWireBounds();
        GameObject m_pPreveObject = null;

        TargetPreview m_preview;
        GUIStyle m_previewStyle;

        DungonEditorLogic m_pLogic;
        //-----------------------------------------------------
        public void Enable(DungonEditorLogic pEditor)
        {
            m_pLogic = pEditor;
            {
                MultiColumnHeaderState.Column[] colums = new MultiColumnHeaderState.Column[]
                {
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column()
                };

                colums[0].headerContent = new GUIContent("ID", "");
                colums[0].minWidth = 100;
                colums[0].width = 100;
                colums[0].maxWidth = 100;
                colums[0].headerTextAlignment = TextAlignment.Left;
                colums[0].canSort = true;
                colums[0].autoResize = true;

                colums[1].headerContent = new GUIContent("描述", "");
                colums[1].minWidth = 100;
                colums[1].width = pEditor.Editor.position.width - 85;
                colums[1].maxWidth = pEditor.Editor.position.width - 85;
                colums[1].headerTextAlignment = TextAlignment.Left;
                colums[1].canSort = false;
                colums[1].autoResize = true;

                var headerState = new MultiColumnHeaderState(colums);
                m_AssetListMCHState = headerState;

                m_TreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pTreeView = new TreeAssetView(m_TreeState, m_AssetListMCHState);
                m_pTreeView.Reload();

                m_pTreeView.OnItemDoubleClick = OnSelect;
                m_pTreeView.OnSelectChange = OnSelect;
                m_pTreeView.OnCellDraw += OnCellGUI;
                m_pTreeView.OnDragItem += OnDragItem;

                RefreshList();
            }

            setUpPreview();
            m_pGridWireBox.Init(Color.red);
        }
        //-----------------------------------------------------
        public void setUpPreview()
        {
            GameObject[] roots = new GameObject[1];
            roots[0] = new GameObject("EditorRoot");
            TargetPreview.InitInstantiatedPreviewRecursive(roots[0], m_preview);

            if (m_preview == null)
                m_preview = new TargetPreview(m_pLogic.Editor);

            TargetPreview.PreviewCullingLayer = 0;
            m_preview.SetCamera(0.01f, 10000f, 60f);
            m_preview.Initialize(roots);
            m_preview.SetPreviewInstance(roots[0]);

            m_preview.OnDrawAfterCB = OnDrawPreview;
        }
        //-----------------------------------------------------
        public void Disable()
        {
            Clear();

            if (m_preview != null)
                m_preview.OnDestroy();

            m_pGridWireBox.Destroy();
        }
        //-----------------------------------------------------
        public void Clear()
        {
            ClearTarget();
        }
        //-----------------------------------------------------
        void ClearTarget()
        {
            if (m_pPreveObject != null) GameObject.DestroyImmediate(m_pPreveObject);
            m_pPreveObject = null;
        }
        //-----------------------------------------------------
        void OnSelect(TreeAssetView.ItemData data)
        {
            ObjectItem item = data as ObjectItem;
            OnChangeSelect(item.pData);
        }
        //-----------------------------------------------------
        void OnChangeSelect(CsvData_Monster.MonsterData item)
        {
            if (item == m_pCurItem)
                return;
            ClearTarget();
            m_pCurItem = item;

            if (m_pCurItem == null) return;
            if (m_pCurItem.Models_modelId_data != null)
            {
                GameObject pObj = AssetDatabase.LoadAssetAtPath<GameObject>(m_pCurItem.Models_modelId_data.strFile);
                if (pObj != null)
                {
                    m_pPreveObject = GameObject.Instantiate<GameObject>(pObj);
                    m_preview.AddSingleObject(m_pPreveObject);
                }
            }
        }
        //-----------------------------------------------------
        public void Update(float fFrameTime)
        {
        }
        //-----------------------------------------------------
        public void Realod()
        {
            RefreshList();
        }
        //------------------------------------------------------
        public void Check()
        {

        }
        //-----------------------------------------------------
        void RefreshList()
        {
            m_pCurItem = null;
            Clear();
            CsvData_Monster projectile = DungonEditor.GetDataer().Monster;
            if (m_pTreeView == null) return;
            m_pTreeView.BeginTreeData();

            if (projectile != null)
            {
                Framework.Data.DataEditorUtil.MappingTable(projectile);
                foreach (var db in projectile.datas)
                {
                    ObjectItem item = new ObjectItem();
                    item.pData = db.Value;
                    item.id = (int)db.Key;
                    item.name = db.Value.desc;
                    m_pTreeView.AddData(item);
                }
            }

            m_pTreeView.EndTreeData();
        }
        //-----------------------------------------------------
        bool OnCellGUI(Rect cellRect, TreeAssetView.TreeItemData item, int column, bool bSelected, bool focused)
        {
            ObjectItem data = item.data as ObjectItem;
            item.displayName = data.pData.desc;
            if (column == 0)
            {
                GUI.Label(cellRect, data.id.ToString());
            }
            else if (column == 1)
            {
                GUI.Label(cellRect, data.pData.desc);
            }
            return true;
        }
        //-----------------------------------------------------
        bool OnDragItem(TreeAssetView.ItemData pData)
        {
            ObjectItem objData = pData as ObjectItem;

            DragAndDrop.PrepareStartDrag();
            DragAndDrop.SetGenericData("level_editor", objData);
            DragAndDrop.StartDrag(objData.name);
   //         m_bDraging = true;

            OnSelect(pData);
            m_pTreeView.SetSelection(new List<int>(new int[] { objData.id }));
            return true;
        }
        //-----------------------------------------------------
        public void OnGUI()
        {          
        }
        //-----------------------------------------------------
        public void OnSceneGUI(SceneView view)
        {

        }
        //-----------------------------------------------------
        void OnDrawPreview(int controll, Camera camera, Event evt)
        {
            if (m_pCurItem != null)
            {
                Quaternion rot = Quaternion.identity;
                Vector3 pos = Vector3.zero;
                if (m_pPreveObject != null)
                {
                    pos = m_pPreveObject.transform.position;
                    m_pPreveObject.transform.localScale = Vector3.one;
                    rot = m_pPreveObject.transform.rotation;
                }
            }
        }
        //-----------------------------------------------------
        public void OnDrawInspecPanel(Vector2 size)
        {
            float view = size.y / 2;
            GUILayout.BeginArea(new Rect(0, 0, size.x - 10, view));
            if(m_pCurItem!=null)
            {
                m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);
                System.Reflection.FieldInfo[] publicMembers = m_pCurItem.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                for (int i = 0; i < publicMembers.Length; ++i)
                {
                    if (publicMembers[i].Name.CompareTo("boxType") == 0 ||
                        publicMembers[i].Name.CompareTo("boxParameter") == 0)
                        continue;

                    Framework.ED.HandleUtilityWrapper.DrawPropertyField(m_pCurItem, publicMembers[i]);
                    //    EditorKits.DrawProperty(m_pCurItem, ref publicMembers[i], bCanSet);
                }
                EditorGUILayout.EndScrollView();
            }
            
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(0, view, size.x - 10, size.y - view));
            DrawPreview(new Rect(0, 0, size.x - 10, size.y - view));
            GUILayout.EndArea();
        }
        //-----------------------------------------------------
        private void DrawPreview(Rect previewRect)
        {
            GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(previewRect.width), GUILayout.Height(previewRect.height) });
            if (m_preview != null)
            {
                if (m_previewStyle == null)
                    m_previewStyle = new GUIStyle(EditorStyles.textField);

                m_preview.OnPreviewGUI(previewRect, m_previewStyle);
            }
            GUILayout.EndVertical();
        }
        //-----------------------------------------------------
        public void OnDrawLayerPanel(Vector2 size)
        {
            if (m_pTreeView == null) return;
            m_pTreeView.multiColumnHeader.GetColumn(1).width = size.x - m_pTreeView.multiColumnHeader.GetColumn(0).width;
            m_pTreeView.searchString = EditorGUILayout.TextField("过滤", m_pTreeView.searchString);
            m_pTreeView.OnGUI(new Rect(0, 40, size.x, size.y - 40));
        }
    }
}