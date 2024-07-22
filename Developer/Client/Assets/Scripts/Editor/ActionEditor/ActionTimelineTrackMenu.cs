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
    public class ActionTimelineTrackMenu
    {
        public struct TrackMenuCatch
        {
            public ETrackCtlType type;
            public ActionTimelinePanel.TrackData trackData;
            public TrackMenuCatch(ETrackCtlType type, ActionTimelinePanel.TrackData trackData)
            {
                this.type = type;
                this.trackData = trackData;
            }
        }
        public struct ActionMenuCatch
        {
            public ETriggerCtlType type;
            public EEventType evtType;
            public ActionState actionState;
            public ActionMenuCatch(ETriggerCtlType type, ActionState actionState, EEventType evtType= EEventType.Base)
            {
                this.type = type;
                this.actionState = actionState;
                this.evtType = evtType;
            }
        }
        public enum ETrackCtlType
        {
            Remove,
            Editor,
            EventLayer,
        }
        public enum ETriggerCtlType
        {
            AddEvent,
            AddProperty,
            Reset,
        }
        //------------------------------------------------------
        public static void BuildEventLayerTarackMenu(string partent, ActionTimelinePanel.TrackData trackData, GenericMenu.MenuFunction2 OnCallback, int start = 0, int count = -1)
        {
            if (ActionEditor.Instance == null) return;
            for (int i = 0; i < ActionEditor.Instance.m_vEventLayers.Count; ++i)
            {
                string strDisplay = ActionEditor.Instance.m_vEventLayers[i].strDisplay;
                if ((trackData.parameter.IsEventBit(ActionEditor.Instance.m_vEventLayers[i].eventLayerBit)))
                    strDisplay += "√";
                else strDisplay += "×";
                trackData.actionStatePropertyIndex = (int)ActionEditor.Instance.m_vEventLayers[i].eventLayerBit;
                ContextMenu.AddItemWithArge(strDisplay, false, OnCallback, new TrackMenuCatch(ETrackCtlType.EventLayer, trackData));
            }
        }
        //------------------------------------------------------
        public static void BuildTrackMenu(string partent, ActionTimelinePanel.TrackData trackData, GenericMenu.MenuFunction2 OnCallback, int start = 0, int count = -1)
        {
            if (string.IsNullOrEmpty(partent))
            {
                ContextMenu.AddItemWithArge("编辑", false, OnCallback, new TrackMenuCatch(ETrackCtlType.Editor, trackData));
                ContextMenu.AddItemWithArge("移除", false, OnCallback, new TrackMenuCatch(ETrackCtlType.Remove, trackData));
            }
            else
            {
                ContextMenu.AddItemWithArge(partent + "/编辑", false, OnCallback, new TrackMenuCatch(ETrackCtlType.Editor, trackData));
                ContextMenu.AddItemWithArge(partent + "/移除", false, OnCallback, new TrackMenuCatch(ETrackCtlType.Remove, trackData));
            }
            ContextMenu.Show();
        }
        //------------------------------------------------------
        public static void BuildActionMenu(string partent, ActionState trackData, GenericMenu.MenuFunction2 OnCallback, int start = 0, int count = -1)
        {
            if (string.IsNullOrEmpty(partent))
            {
                List<string> vEvents = EventPopDatas.GetEventPops();
                List<EEventType> vEventTypes = EventPopDatas.GetEventTypes();
                for (int i = 0; i < vEvents.Count; ++i)
                {
                    ContextMenu.AddItemWithArge("事件/"+vEvents[i], false, OnCallback, new ActionMenuCatch(ETriggerCtlType.AddEvent, trackData, vEventTypes[i]));
                }

                ContextMenu.AddItemWithArge("添加动作属性", false, OnCallback, new ActionMenuCatch(ETriggerCtlType.AddProperty, trackData));
                ContextMenu.AddItemWithArge("重置触发", false, OnCallback, new ActionMenuCatch(ETriggerCtlType.Reset, trackData));
            }
            else
            {
                List<string> vEvents = EventPopDatas.GetEventPops();
                List<EEventType> vEventTypes = EventPopDatas.GetEventTypes();
                for (int i = 0; i < vEvents.Count; ++i)
                {
                    ContextMenu.AddItemWithArge(partent + "/事件/" + vEvents[i], false, OnCallback, new ActionMenuCatch(ETriggerCtlType.AddEvent, trackData, vEventTypes[i]));
                }
                ContextMenu.AddItemWithArge(partent + "/添加动作属性", false, OnCallback, new ActionMenuCatch(ETriggerCtlType.AddProperty, trackData));
                ContextMenu.AddItemWithArge(partent + "/重置触发", false, OnCallback, new ActionMenuCatch(ETriggerCtlType.Reset, trackData));
            }
            ContextMenu.Show();
        }
    }
}
