/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ActionTimelinePanel
作    者:	HappLI
描    述:	动作事件轴
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using TopGame.Data;
using TopGame.Core;
using System.Reflection;
using Framework.Core;
using Framework.ED;

namespace TopGame.ED
{
    public class ActionTimelinePanel
    {
        public struct TrackData
        {
            public BaseEventParameter parameter;

            public int actionStatePropertyIndex;
        }
        //------------------------------------------------------
        class DragTrackData
        {
            public TrackData trackData = new TrackData();
            public float time;
            public Rect rect;
            public Vector2 offset;
        }
        DragTrackData m_pDragTrack = null;

        ActionEditor m_pEditor;
        ActionEditorLogic m_pLogic;

        GUIContent m_pContext = new GUIContent();

        static float ROW_HEIGHT = 20;
        float m_fRowHeightOffset = 0;
        GUIStyle m_RowStyle = null;

        float m_fCurTime = 0;
        float m_fMaxTime = 0;
        Rect m_ContextRect = new Rect();
        Vector2 m_Scroll = Vector2.zero;
        List<BaseEventParameter> m_vEvents = new List<BaseEventParameter>();
        TimelinePanel m_Timeline = new TimelinePanel();
        public ActionTimelinePanel()
        {
        }
        //------------------------------------------------------
        public void OnGUI(ActionEditor editor, Rect rect)
        {
            if(m_RowStyle == null)
            {
                m_RowStyle = GUI.skin.button;
            }
            m_pEditor = editor;
            m_pLogic = m_pEditor.logic;
            m_fMaxTime = m_pLogic.GetDurationTime();
            if (m_fMaxTime <= 0) m_fMaxTime = 1;
            m_Timeline.DrawBG(new Rect(rect.x - 5, rect.y, rect.width + 10, 37), new Color(0,0,0,0.6f));
            m_fCurTime = m_Timeline.Draw(new Rect(rect.x, rect.y, rect.width, 50), m_fCurTime, m_fMaxTime, m_fMaxTime * 0.01f);
            m_ContextRect.x = rect.x;
            m_ContextRect.y = rect.y + 50;
            m_ContextRect.width = rect.width;
            m_ContextRect.height = rect.height - 50;
            DrawTimeGap();
            GUILayout.BeginArea(new Rect(m_ContextRect.x, m_ContextRect.y, m_ContextRect.width+5, m_ContextRect.height));
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);
            DrawTimeTriggers();
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();

            OnController();
        }
        //------------------------------------------------------
        void OnController()
        {
            m_pEditor.wantsMouseMove = true;
            Event e = Event.current;
            if (e.type == EventType.MouseDrag)
            {
                if (e.button == 0)
                {
                    if (m_pDragTrack != null)
                    {
                        m_pDragTrack.rect.x = Mathf.Max(0, e.mousePosition.x - m_ContextRect.x - m_pDragTrack.offset.x);
                        if(m_pDragTrack.trackData.parameter !=  null)
                        {
                            m_pDragTrack.trackData.parameter.triggetTime =Mathf.Clamp(m_fMaxTime * m_pDragTrack.rect.x / m_ContextRect.width,0, m_fMaxTime);
                            m_fCurTime = m_pDragTrack.trackData.parameter.triggetTime;
                        }
                        else
                        {
                            List<ActionStatePropertyData> vPropertys = m_pLogic.GetCurAction().GetCore().action_property_data;
                            if (vPropertys != null && m_pDragTrack.trackData.actionStatePropertyIndex>=0 && m_pDragTrack.trackData.actionStatePropertyIndex < vPropertys.Count)
                            {
                                ActionStatePropertyData propertyData = vPropertys[m_pDragTrack.trackData.actionStatePropertyIndex];
                                propertyData.fTriggerTime = Mathf.Clamp(m_fMaxTime * m_pDragTrack.rect.x / m_ContextRect.width, 0, m_fMaxTime);
                                m_fCurTime = propertyData.fTriggerTime;
                                vPropertys[m_pDragTrack.trackData.actionStatePropertyIndex] = propertyData;
                            }
                            else
                            {
                                m_pDragTrack = null;
                            }
                        }
                    }
                }
            }
            else if (e.type == EventType.MouseDown)
            {
                if (m_pDragTrack == null)
                {
                    if(e.button == 1)
                    {
                        ActionTimelineTrackMenu.BuildActionMenu("", m_pLogic.GetCurAction(), OnActionMenu);
                    }
                }
                m_pDragTrack = null;
  
            }
            else if (e.type == EventType.MouseUp)
            {
                m_pDragTrack = null;
            }
        }
        //------------------------------------------------------
        public void SetCurTime(float fTime)
        {
            m_fCurTime = fTime;
        }
        //------------------------------------------------------
        public float GetCurTime()
        {
            return m_fCurTime;
        }
        //------------------------------------------------------
        void DrawTimeTriggers()
        {
            ActionState curState = m_pLogic.GetCurAction();
            if (curState == null) return;

            m_fRowHeightOffset = 0;

            m_vEvents.Clear();
            ActionEventCore eventCore = curState.GetCore().action_event_core;
            if (eventCore == null)
                return;
            eventCore.BuildEvent(m_pLogic.GetModule(), m_vEvents);
            //events
            for(int i = 0; i < m_vEvents.Count; ++i)
            {
                DrawEvent(m_vEvents[i]);
            }

            for (int i = 0; i < m_vEvents.Count; ++i)
            {
                if(m_vEvents[i].bDeling)
                    eventCore.DelEvent(m_vEvents[i]);
            }

            //global event
            List<BaseEventParameter> vGlobalEvents;
            if (ActionStateManager.getInstance().GlobalStateEvents.TryGetValue(ActionStateManager.BuildActionKey(curState.GetCore().type, curState.GetCore().tag), out vGlobalEvents))
            {
                for (int i = 0; i < vGlobalEvents.Count; ++i)
                {
                    DrawEvent(vGlobalEvents[i], true);
                }
            }

            // actionpropertys
            if (curState.GetCore().action_property_data == null) curState.GetCore().action_property_data = new List<ActionStatePropertyData>();
            for (int i = 0; i < curState.GetCore().action_property_data.Count; ++i)
            {
                DrawActionProperty(i, curState.GetCore().action_property_data[i]);
            }

            for(int i = 0; i < curState.GetCore().action_property_data.Count;)
            {
                if (curState.GetCore().action_property_data[i].bDeling)
                {
                    curState.GetCore().action_property_data.RemoveAt(i);
                }
                else
                    ++i;
            }
        }
        //------------------------------------------------------
        void DrawEvent(BaseEventParameter evtParameter, bool bGlobal = false)
        {
            if(!evtParameter.IsEventBit(m_pEditor.EditorEventMask))
                return;

            Color color = GUI.color;
            string strName = evtParameter.GetType().Name;
            EventDeclarationAttribute attr = (EventDeclarationAttribute)evtParameter.GetType().GetCustomAttribute(typeof(EventDeclarationAttribute));
            if (attr != null)
            {
                if (!string.IsNullOrEmpty(attr.EventName))
                    strName = attr.EventName;
            }
            Color eventTimeColor = EventPreferences.GetTypeColor(strName);
            if (bGlobal)
            {
                strName += "[全局]";
            }
            m_pContext.text = strName;
            Vector2 size = m_RowStyle.CalcSize(m_pContext);

            float rowHeight = EventPreferences.GetSettings().rowHeight;

            Rect rect = new Rect(m_ContextRect.width*evtParameter.triggetTime/m_fMaxTime, m_fRowHeightOffset, size.x+5, rowHeight);
            Rect drawRect = rect;
            GUI.color = eventTimeColor;
            if (GUI.RepeatButton(drawRect, m_pContext, m_RowStyle))
            {
                if(!bGlobal)
                {
                    if (Event.current.button == 1)
                    {
                        m_pDragTrack = null;
                        ActionTimelineTrackMenu.BuildEventLayerTarackMenu("", new TrackData() { parameter = evtParameter, actionStatePropertyIndex = -1 }, OnEventTrackMenu);
                        ActionTimelineTrackMenu.BuildTrackMenu("", new TrackData() { parameter = evtParameter, actionStatePropertyIndex = -1 }, OnEventTrackMenu);
                    }
                    else
                    {
                        Vector2 mousepos = Event.current.mousePosition;
                        m_pDragTrack = new DragTrackData();
                        m_pDragTrack.trackData.parameter = evtParameter;
                        m_pDragTrack.trackData.actionStatePropertyIndex = -1;
                        m_pDragTrack.rect = rect;
                        m_pDragTrack.time = evtParameter.triggetTime;
                        m_pDragTrack.offset = mousepos - new Vector2(rect.x, rect.y);
                    }
                }

            }
            GUI.color = color;

            m_fRowHeightOffset += rowHeight;
        }
        //------------------------------------------------------
        void DrawActionProperty(int index, ActionStatePropertyData statePropertyData)
        {
            Color color = GUI.color;
            string strName = statePropertyData.strName;
            if (string.IsNullOrEmpty(strName))
                strName = "ActionStatePropertyData";
            Color eventTimeColor = EventPreferences.GetTypeColor("ActionStatePropertyDataType");
            m_pContext.text = strName;
            Vector2 size = m_RowStyle.CalcSize(m_pContext);

            float rowHeight = EventPreferences.GetSettings().rowHeight;

            float durationWidth = m_ContextRect.width * statePropertyData.FrameDuration / m_fMaxTime;
            Rect rect = new Rect(m_ContextRect.width * statePropertyData.fTriggerTime / m_fMaxTime, m_fRowHeightOffset, durationWidth, rowHeight);
            Rect drawRect = rect;
            GUI.color = eventTimeColor;
            if (durationWidth < size.x)
            {
                GUI.Box(drawRect, "", m_RowStyle);
                m_pContext.text = strName;
                eventTimeColor.a = 0.5f;
                GUI.color = eventTimeColor;
                if(GUI.RepeatButton(new Rect(rect.x, rect.y, size.x, rect.height), m_pContext))
                {
                    if (Event.current.button == 1)
                    {
                        m_pDragTrack = null;
                        ActionTimelineTrackMenu.BuildTrackMenu("", new TrackData() { parameter = null, actionStatePropertyIndex = index }, OnEventActionPropertyDataMenu);
                    }
                    else
                    {
                        Vector2 mousepos = Event.current.mousePosition;
                        m_pDragTrack = new DragTrackData();
                        m_pDragTrack.trackData.parameter = null;
                        m_pDragTrack.trackData.actionStatePropertyIndex = index;
                        m_pDragTrack.rect = rect;
                        m_pDragTrack.time = statePropertyData.fTriggerTime;
                        m_pDragTrack.offset = mousepos - new Vector2(rect.x, rect.y);
                    }
                }
            }
            else
            {
                if (GUI.RepeatButton(drawRect, m_pContext, m_RowStyle))
                {
                    if (Event.current.button == 1)
                    {
                        m_pDragTrack = null;
                        ActionTimelineTrackMenu.BuildTrackMenu("", new TrackData() { parameter = null, actionStatePropertyIndex = index }, OnEventActionPropertyDataMenu);
                    }
                    else
                    {
                        Vector2 mousepos = Event.current.mousePosition;
                        m_pDragTrack = new DragTrackData();
                        m_pDragTrack.trackData.parameter = null;
                        m_pDragTrack.trackData.actionStatePropertyIndex = index;
                        m_pDragTrack.rect = rect;
                        m_pDragTrack.time = statePropertyData.fTriggerTime;
                        m_pDragTrack.offset = mousepos - new Vector2(rect.x, rect.y);
                    }
                }
            }
            GUI.color = color;

            m_fRowHeightOffset += rowHeight;
        }
        //------------------------------------------------------
        void DrawTimeGap()
        {
            float gap = m_fMaxTime * 0.01f;
            int gapCnt = 0;
            float fCur = 0;
            for (float i = 0; i <= m_fMaxTime; i += gap)
            {
                fCur = i;
                if (fCur > m_fMaxTime) fCur = m_fMaxTime;
                if ((gapCnt % 5) == 0 || (gapCnt % 10) == 0 || fCur == m_fMaxTime)
                {
                    float x = m_ContextRect.x + ((m_ContextRect.width * fCur) / m_fMaxTime);
                    EditorUtil.DrawColorLine( new Vector2(x, m_ContextRect.y-12), new Vector2(x, m_ContextRect.yMax+12), new Color(0.5f,0.5f,0.5f,0.5f)  );
                }
                gapCnt++;
            }

            ActionState curState = m_pLogic.GetCurAction();
            if (curState != null && curState.GetCore().animations!=null && curState.GetCore().animations.Length>0)
            {
                if(curState.GetCore().seqType == EAnimSeqType.Sequence)
                {
                    float fTime = 0;
                    for (int i = 0; i < curState.GetCore().animations.Length; ++i)
                    {
                        fTime += curState.GetCore().animations[i].duration;
                        float x = m_ContextRect.x + ((m_ContextRect.width * fTime) / m_fMaxTime);
                        EditorUtil.DrawColorLine(new Vector2(x, m_ContextRect.y - 12), new Vector2(x, m_ContextRect.yMax + 12), new Color(1f, 0f, 0f, 0.8f));
                    }
                }

            }


            fCur = 0;
            gap = EventPreferences.GetSettings().rowHeight;
            float fOffset = m_ContextRect.y;
            for (float i = 0; i <= m_ContextRect.height; i+= gap)
            {
                EditorUtil.DrawColorLine(new Vector2(m_ContextRect.x, fOffset), new Vector2(m_ContextRect.xMax, fOffset), new Color(0.1f, 0.1f, 0.1f, 0.05f));
                fOffset += gap;
            }
        }
        //------------------------------------------------------
        void OnActionMenu(object argv)
        {
            ActionTimelineTrackMenu.ActionMenuCatch action = (ActionTimelineTrackMenu.ActionMenuCatch)argv;
            if (action.actionState == null) return;
            if(action.type == ActionTimelineTrackMenu.ETriggerCtlType.AddEvent)
            {
                if(action.evtType != EEventType.Base)
                {
                    BaseEventParameter parame = BuildEventUtl.BuildEventByType(null, action.evtType);
                    action.actionState.GetCore().action_event_core.AddEvent(parame);
                }
                return;
            }
            if (action.type == ActionTimelineTrackMenu.ETriggerCtlType.AddProperty)
            {
                if (action.actionState.GetCore().action_property_data == null) action.actionState.GetCore().action_property_data = new List<ActionStatePropertyData>();
                action.actionState.GetCore().action_property_data.Add(new ActionStatePropertyData());
                return;
            }
            if (action.type == ActionTimelineTrackMenu.ETriggerCtlType.Reset)
            {
                action.actionState.RefreshEvent();
                action.actionState.RebuildActionStatePropertyMap();
                return;
            }
        }
        //------------------------------------------------------
        void OnEventTrackMenu(object argv)
        {
            ActionTimelineTrackMenu.TrackMenuCatch menuData = (ActionTimelineTrackMenu.TrackMenuCatch)argv;
            if (menuData.trackData.parameter == null) return;
            if(menuData.type == ActionTimelineTrackMenu.ETrackCtlType.Remove)
            {
                if(EditorUtility.DisplayDialog("提示", "确定移除该事件?", "移除", "取消"))
                {
                    menuData.trackData.parameter.bDeling = true;
                }
                return;
            }
            if (menuData.type == ActionTimelineTrackMenu.ETrackCtlType.Editor)
            {
                m_pLogic.EditEvent(m_pLogic.GetCurAction(), menuData.trackData.parameter);
                return;
            }
            if (menuData.type == ActionTimelineTrackMenu.ETrackCtlType.EventLayer)
            {
                m_pLogic.EditEventBit(menuData.trackData.parameter, (EEventBit)menuData.trackData.actionStatePropertyIndex);
                return;
            }
        }
        //------------------------------------------------------
        void OnEventActionPropertyDataMenu(object argv)
        {
            ActionTimelineTrackMenu.TrackMenuCatch menuData = (ActionTimelineTrackMenu.TrackMenuCatch)argv;
            if (menuData.trackData.actionStatePropertyIndex <0 || m_pLogic.GetCurAction() == null) return;

            List<ActionStatePropertyData> vPropertys = m_pLogic.GetCurAction().GetCore().action_property_data;
            if (vPropertys == null || menuData.trackData.actionStatePropertyIndex >= vPropertys.Count) return;
            if (menuData.type == ActionTimelineTrackMenu.ETrackCtlType.Remove)
            {
                if (EditorUtility.DisplayDialog("提示", "确定移除该动作属性?", "移除", "取消"))
                {
                    ActionStatePropertyData propertyData= vPropertys[menuData.trackData.actionStatePropertyIndex];
                    propertyData.bDeling = true;
                    vPropertys[menuData.trackData.actionStatePropertyIndex] = propertyData;
                }
                return;
            }
            if (menuData.type == ActionTimelineTrackMenu.ETrackCtlType.Editor)
            {
                m_pLogic.EditActionStateProperty(m_pLogic.GetCurAction(), menuData.trackData.actionStatePropertyIndex);
                return;
            }
        }
    }
}
