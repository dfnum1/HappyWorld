
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
using Framework.Core;
using Framework.Data;

namespace TopGame.ED
{
    public class EventDataEditorLogic
    {
        public class EventItem : TreeAssetView.ItemData
        {
            public Data.EventData pData;
            public override Color itemColor()
            {
                return Color.white;
            }

            public void Save()
            {
                if (pData != null) pData.Save();
            }
        }

        EventItem m_pCurItem = null;
        EventData m_pCopyData = null;
        public CsvData_EventDatas m_pCsvData = null;

        EventDataEditor m_pEditor;
        EditorModule m_pGameModuel;
        EEventType m_AddEventType = EEventType.Count;

        int m_nEventID = 0;
        Vector2 m_ScrollEvent;

        Dictionary<System.Object, bool> m_vExpand = new Dictionary<object, bool>();
        UnityEditor.IMGUI.Controls.TreeViewState m_TreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_AssetListMCHState;
        TreeAssetView m_pTreeView = null;
        //-----------------------------------------------------
        public void OnEnable(EventDataEditor pEditor, EditorModule moduel)
        {
            m_pGameModuel = moduel;
            {
                MultiColumnHeaderState.Column[] colums = new MultiColumnHeaderState.Column[]
                {
                new MultiColumnHeaderState.Column(),
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
                colums[1].width = 150;
                colums[1].maxWidth = 1000;
                colums[1].headerTextAlignment = TextAlignment.Left;
                colums[1].canSort = false;
                colums[1].autoResize = true;

                colums[2].headerContent = new GUIContent("", "");
                colums[2].minWidth = 150;
                colums[2].width = 150;
                colums[2].maxWidth = 150;
                colums[2].headerTextAlignment = TextAlignment.Left;
                colums[2].canSort = false;
                colums[2].autoResize = true;

                var headerState = new MultiColumnHeaderState(colums);
                m_AssetListMCHState = headerState;

                m_TreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pTreeView = new TreeAssetView(m_TreeState, m_AssetListMCHState);
                m_pTreeView.Reload();
                m_pTreeView.SetRowHeight(30);

                m_pTreeView.OnItemDoubleClick = OnSelect;
                m_pTreeView.OnSelectChange = OnSelect;
                m_pTreeView.OnCellDraw += OnCellGUI;

                RefreshList();
            }
            m_pEditor = pEditor;
            ED.EditorKits.CheckEventCopy();
        }
        //-----------------------------------------------------
        public void OnDisable()
        {
            Clear();
        }
        //-----------------------------------------------------
        void RefreshList()
        {
            if(m_pTreeView != null)
            {
                m_pTreeView.BeginTreeData();
                if(m_pCsvData!=null)
                {
                    foreach (var db in m_pCsvData.datas)
                    {
                        EventItem item = new EventItem();
                        item.pData = db.Value;
                        item.id = db.Value.nID;
                        item.name = db.Value.desc + db.Value.nID;
                        m_pTreeView.AddData(item);
                    }
                }

                m_pTreeView.EndTreeData();

                if(m_pCurItem != null)
                {
                    m_pTreeView.SetSelection(new List<int>(new int[] { m_pCurItem.pData.nID }));
                }
            }
        }
        //-----------------------------------------------------
        public void Clear()
        {
            m_vExpand.Clear();
        }
        //-----------------------------------------------------
        public void Update(float fFrameTime)
        {
            if (ModuleManager.editorModule == null) return;
        }
        //-----------------------------------------------------
        public void OnEvent(Event evt)
        {
            if (evt.type == EventType.KeyDown)
            {
                m_pCopyData = null;
            }
        }
        //-----------------------------------------------------
        public void SaveData()
        {
            if (m_pCurItem != null) m_pCurItem.pData.Save();

            string strEventFile = EditorHelp.BinaryRootPath + "Config/Csv/Events.csv";
            if(!Directory.Exists(EditorHelp.BinaryRootPath + "Config/Csv"))
            {
                Directory.CreateDirectory(EditorHelp.BinaryRootPath + "Config/Csv");
            }

            if (m_pCsvData == null) return;
            BinaryUtil wirter = new BinaryUtil();
            wirter.WriteByte(0);    //version
            wirter.WriteInt32(m_pCsvData.datas.Count);
            foreach (var db in m_pCsvData.datas)
            {
                db.Value.Save();
                wirter.WriteUint32((uint)db.Key);
                wirter.WriteBool(db.Value.groupRandom);
                if(db.Value.events!=null)
                {
                    wirter.WriteByte((byte)db.Value.events.Count);
                    for(int i =0; i < db.Value.events.Count;++i)
                    {
                        db.Value.events[i].Write(ref wirter);
                    }
                }
                else
                {
                    wirter.WriteByte((byte)0);
                }
            }
            wirter.SaveTo(strEventFile);
            m_pCsvData.Save();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
        //-----------------------------------------------------
        public void CheckLoopRecyleEvent()
        {
            List<int> vCheckers = new List<int>();
            foreach (var db in m_pCsvData.datas)
            {
                vCheckers.Clear();
                if (CheckLoopRecyleEvent(vCheckers, db.Key))
                    break;
            }
            vCheckers.Clear();
            CsvData_Projectile priojTable = DataEditorUtil.GetTable<CsvData_Projectile>();
            if(priojTable!=null)
            {
                foreach (var db in priojTable.datas)
                {
                    vCheckers.Clear(); if (CheckLoopRecyleEvent(vCheckers, db.Value.AttackEventID)) break;
                    vCheckers.Clear(); if (CheckLoopRecyleEvent(vCheckers, db.Value.HitEventID)) break;
                    vCheckers.Clear(); if (CheckLoopRecyleEvent(vCheckers, db.Value.OverEventID)) break;
                    vCheckers.Clear(); if (CheckLoopRecyleEvent(vCheckers, db.Value.StepEventID)) break;
                }
            }

        }
        //-----------------------------------------------------
        private string ListToString(List<int> vCheckers)
        {
            string strText = "";
            for(int i = 0; i < vCheckers.Count; ++i)
            {
                strText += vCheckers[i] + ",";
            }
            return strText;
        }
        //-----------------------------------------------------
        private bool CheckLoopRecyleEvent(List<int> vCheckers, int evnetID)
        {
            EventData evtData = m_pCsvData.GetData(evnetID);
            if (evtData == null) return false;
            if(vCheckers.Contains(evnetID))
            {
                vCheckers.Add(evnetID);
                EditorUtility.DisplayDialog("循环触发事件", ListToString(vCheckers), "请检查");
                m_pTreeView.SetSelection(new List<int>(new int[] { evnetID }));
                return true;
            }
            vCheckers.Add(evnetID);
            if (evtData.events != null)
            {
                for (int i = 0; i < evtData.events.Count; ++i)
                {
                    if (CheckLoopRecyleEvent(vCheckers, evtData.events[i])) return true;
                }
            }
            return false;
        }
        //-----------------------------------------------------
        bool CheckLoopRecyleEvent(List<int> vCheckers, BaseEventParameter eventParam)
        {
            if (eventParam == null) return false;
            if (eventParam is ProjectileEventParameter)
            {
                ProjectileEventParameter projEvet = eventParam as ProjectileEventParameter;
                CsvData_Projectile projectileTable = DataEditorUtil.GetTable<CsvData_Projectile>();
                if(projectileTable!=null)
                {
                    ProjectileData projData = projectileTable.GetData(projEvet.projectileID);
                    if (projData != null)
                    {
                        if (CheckLoopRecyleEvent(vCheckers, projData.AttackEventID)) return true;
                        if (CheckLoopRecyleEvent(vCheckers, projData.HitEventID)) return true;
                        if (CheckLoopRecyleEvent(vCheckers, projData.OverEventID)) return true;
                        if (CheckLoopRecyleEvent(vCheckers, projData.StepEventID)) return true;
                    }
                }

            }
            else if (eventParam is TriggerEventParameter)
            {
                TriggerEventParameter triEvent = eventParam as TriggerEventParameter;
                if (triEvent.dataType == TriggerEventParameter.EDataType.TriggerIDEvent)
                {
                    if (CheckLoopRecyleEvent(vCheckers, triEvent.idValue)) return true;
                }
                else if (triEvent.dataType == TriggerEventParameter.EDataType.Guide)
                {
                    Framework.Plugin.Guide.GuideDatas pGuide = null;
                    string[] guideDatas = AssetDatabase.FindAssets("t:GuideDatas");
                    if (guideDatas != null && guideDatas.Length > 0)
                    {
                        pGuide = AssetDatabase.LoadAssetAtPath<Framework.Plugin.Guide.GuideDatas>(AssetDatabase.GUIDToAssetPath(guideDatas[0]));
                    }
                    if (pGuide != null)
                    {
                        pGuide.Init(true);
                        Framework.Plugin.Guide.GuideGroup group = pGuide.GetGuide(triEvent.idValue);
                        if (group != null && group.vTriggers != null)
                        {
                            for (int j = 0; j < group.vTriggers.Count; ++j)
                            {
                                if (group.vTriggers[j].Events != null)
                                {
                                    for (int k = 0; k < group.vTriggers[j].Events.Length; ++k)
                                    {
                                        if (!string.IsNullOrEmpty(group.vTriggers[j].Events[k]))
                                        {
                                            if (CheckLoopRecyleEvent(vCheckers, BuildEventUtl.BuildEvent(null, group.vTriggers[j].Events[k])))
                                                return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        //-----------------------------------------------------
        void OnSelect(TreeAssetView.ItemData data)
        {
            EventItem item = data as EventItem;
            OnChangeSelect(item);
        }
        //-----------------------------------------------------
        void OnChangeSelect(EventItem item)
        {
            if (item == m_pCurItem)
                return;
            if (m_pCurItem != null) m_pCurItem.Save();
            Clear();
            m_pCurItem = item;
        }
        //-----------------------------------------------------
        bool OnCellGUI(Rect cellRect, TreeAssetView.TreeItemData item, int column, bool bSelected, bool focused)
        {
            EventItem data = item.data as EventItem;
            item.displayName = data.pData.desc;
            if (column == 0)
            {
                int.TryParse(GUI.TextField(new Rect(0, cellRect.y, cellRect.width - 40, cellRect.height), data.id.ToString()), out data.id);
                data.id = Mathf.Clamp(data.id, 0, 65535);
                if (data.id != data.pData.nID && GUI.Button(new Rect(cellRect.width - 40, cellRect.y, 40, cellRect.height), "使用"))
                {
                    GUI.FocusControl(null);
                    if(!m_pCsvData.datas.ContainsKey(data.id))
                    {
                        EventData pData = m_pCsvData.GetData(data.pData.nID);
                        if (pData != null)
                        {
                            m_pCsvData.datas.Remove(data.pData.nID);
                            pData.nID = data.id;
                            m_pCsvData.datas.Add(pData.nID, pData);
                        }
                    }
                    else
                    {
                        EditorKits.PopMessageBox("提示", "该ID 已被使用", "确定");
                    }
                }
            }
            else if (column == 1)
            {
                GUI.Label(cellRect, data.pData.desc);
            }
            else if (column == 2)
            {
                float gap = cellRect.width / 2;
                float posx = 0;
                if(m_pCopyData != null && m_pCopyData != data.pData) gap = cellRect.width / 3;
                if (GUI.Button( new Rect(cellRect.x, cellRect.y+ posx, gap, cellRect.height), "复制" ))
                {
                    m_pCopyData = data.pData;
                }
                posx += gap;
                if (m_pCopyData != null && m_pCopyData != data.pData && GUI.Button(new Rect(cellRect.x+gap, cellRect.y, posx, cellRect.height), "黏贴"))
                {
                    data.pData.Copy(m_pCopyData);
                }
                if (m_pCopyData != null && m_pCopyData != data.pData) posx += gap;
                if(GUI.Button(new Rect(cellRect.x + posx, cellRect.y, gap, cellRect.height), "删除"))
                {
                    if(EditorKits.PopMessageBox("提示", "是否确认删除", "删除", "取消"))
                    {
                        m_pCsvData.datas.Remove(data.pData.nID);
                        RefreshList();
                    }
                }
            }
            return true;
        }
        //-----------------------------------------------------
        public void OnGUI()
        {
            if (m_pEditor == null) return;
        }
        //-----------------------------------------------------
        public void DrawInspectorPanel(Vector2 size)
        {
            if (m_pCurItem == null) return;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("删除"))
            {
                if (EditorKits.PopMessageBox("提示", "是否确认删除", "删除", "取消"))
                {
                    m_pCsvData.datas.Remove(m_pCurItem.pData.nID);
                    m_pCurItem = null;
                    RefreshList();
                }
                return;
            }
            if (GUILayout.Button("复制"))
            {
                m_pCopyData = m_pCurItem.pData;
            }
            if (m_pCopyData != null && m_pCopyData != m_pCurItem.pData &&
                GUILayout.Button("黏贴"))
            {
                m_pCurItem.pData.Copy(m_pCopyData);
                m_pCopyData = null;
            }
            GUILayout.EndHorizontal();

            m_pCurItem.pData.desc = EditorGUILayout.TextField("描述", m_pCurItem.pData.desc);
            m_pCurItem.pData.groupRandom = EditorGUILayout.Toggle("组随机", m_pCurItem.pData.groupRandom);

            GUILayout.BeginHorizontal();
            m_AddEventType = EventPopDatas.DrawEventPop(m_AddEventType, "");
            if (m_AddEventType > EEventType.Base && m_AddEventType < EEventType.Count)
            {
                if (GUILayout.Button("创建"))
                {
                    m_pCurItem.pData.events.Add(BuildEventUtl.BuildEventByType(null, m_AddEventType));
                }
            }
            GUILayout.EndHorizontal();

            Color color = GUI.color;
            m_ScrollEvent = GUILayout.BeginScrollView(m_ScrollEvent);
            if (m_pCurItem.pData.events == null) m_pCurItem.pData.events = new List<BaseEventParameter>();
            for (int i = 0; i < m_pCurItem.pData.events.Count; ++i)
            {
                bool bExpand = false;
                m_vExpand.TryGetValue(m_pCurItem.pData.events[i], out bExpand);

                GUILayout.BeginHorizontal();
                bExpand = EditorGUILayout.Foldout(bExpand, m_pCurItem.pData.events[i].GetEventType().ToString());
                m_vExpand[m_pCurItem.pData.events[i]] = bExpand;
                if (m_pCurItem.pData.events[i].OnEdit(true) && GUILayout.Button("编辑", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    m_pCurItem.pData.events[i].OnEdit(false);
                }
                if (m_pCurItem.pData.events[i].OnPreview(true) && GUILayout.Button("预览", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    m_pCurItem.pData.events[i].OnPreview(false);
                }
                if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    m_pCurItem.pData.events.RemoveAt(i);
                    break;
                }
                if (GUILayout.Button("复制", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    DrawEventCore.AddCopyEvent(m_pCurItem.pData.events[i]);
                }
                if (DrawEventCore.CanCopyEvent(m_pCurItem.pData.events[i]) &&
                    GUILayout.Button("黏贴", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    DrawEventCore.CopyEvent(m_pCurItem.pData.events[i]);
                }
                GUILayout.EndHorizontal();
                if(bExpand)
                    m_pCurItem.pData.events[i] = DrawEventCore.DrawUnAction(m_pCurItem.pData.events[i]);
            }
            GUILayout.EndScrollView();
        }
        //-----------------------------------------------------
        public void OnDrawLayerPanel(Rect rc)
        {
            if (m_pTreeView == null || m_pCsvData == null) return;
            GUILayout.BeginHorizontal();
            m_nEventID = EditorGUILayout.IntField(m_nEventID);
            EditorGUI.BeginDisabledGroup(m_pCsvData.datas.ContainsKey(m_nEventID));
            if(GUILayout.Button("新建"))
            {
                EventData pData = new EventData();
                pData.nID = m_nEventID;
                m_nEventID = 0;
                m_pCsvData.datas.Add(pData.nID, pData);
                RefreshList();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            m_pTreeView.searchString = EditorGUILayout.TextField("过滤", m_pTreeView.searchString);
            m_pTreeView.OnGUI(new Rect(0, 60, rc.size.x, rc.size.y - 60));
        }
        //-----------------------------------------------------
        public void OnReLoadAssetData()
        {
            m_pCsvData = DataEditorUtil.GetTable<CsvData_EventDatas>("EventDatas",true);
            DataEditorUtil.MappingTable(m_pCsvData);
            InitAsset();
        }
        //-----------------------------------------------------
        public void InitAsset()
        {
            m_pCsvData = DataEditorUtil.GetTable<CsvData_EventDatas>("EventDatas", true);
            DataEditorUtil.MappingTable(m_pCsvData);
            RefreshList();
        }
    }
}