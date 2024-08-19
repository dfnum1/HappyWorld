/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	DungonEditorLogic
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
    public class DungonEditorLogic
    {
        public enum ETab
        {
            Element,
            Monster,
            TerrainScene,
        }
        static string[] tabs = { "障碍列表", "怪物列表", "地表场景"};
        public ETab m_eTab = ETab.Element;


        DungonEditor m_pEditor;
        public DungonEditor Editor { get { return m_pEditor; } }
        public BattleObjectEditorLogic m_pBattleObject = new BattleObjectEditorLogic();
        public MonsterObjectEditorLogic m_pMonsterObject = new MonsterObjectEditorLogic();
        public TerrainSceneLogic m_pSceneLogic = new TerrainSceneLogic();

        Vector2 m_Scroll = Vector2.zero;

        bool m_bDraging = false;

        //-----------------------------------------------------
        public void OnEnable(DungonEditor pEditor)
        {
            m_pEditor = pEditor;
            m_pBattleObject.Enable(this);
            m_pMonsterObject.Enable(this);
            m_pSceneLogic.Enable(this);
            m_bDraging = false;
        }
        //-----------------------------------------------------
        public void OnDisable()
        {
            Clear();

            m_pBattleObject.Disable();
            m_pMonsterObject.Disable();
            m_pSceneLogic.Disable();
            m_bDraging = false;
        }
        //-----------------------------------------------------
        public void Clear()
        {
            m_bDraging = false;
            ClearTarget();
        }
        //-----------------------------------------------------
        void ClearTarget()
        {
        }
        //-----------------------------------------------------
        public void Load()
        {

        }
        //-----------------------------------------------------
        public void Play(bool bPlay)
        {
        }
        //-----------------------------------------------------
        public bool isPlay()
        {
            return false;
        }
        //-----------------------------------------------------
        public void Update(float fFrameTime)
        {

        }
        //-----------------------------------------------------
        public void OnEvent(Event evt)
        {
            if (m_bDraging)
            {
                if (evt.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                }
                else if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    DragAndDrop.AcceptDrag();
                }
                else if (evt.type == EventType.DragExited)
                {
                }
                //  evt.Use();
            }
            if(m_eTab == ETab.TerrainScene)
            {
                if (m_pSceneLogic != null)
                {
                    m_pSceneLogic.OnEvent(evt);
                }
            }

        }
        //------------------------------------------------------
        void OnSceneEvent(Event evt)
        {
            ShowMenu(evt);
        }
        //-----------------------------------------------------
        public bool ShowMenu(Event evt, System.Action onCallback =null)
        {
            if (evt.button == 1 && evt.control && evt.type == EventType.MouseUp)
            {
                return true;
            }
            return false;
        }
        //-----------------------------------------------------
        public void SaveData()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
        //-----------------------------------------------------
        public void Realod()
        {
            Framework.Data.DataEditorUtil.ClearTables();
            m_pBattleObject.Realod();
            m_pMonsterObject.Realod();
        }
        //------------------------------------------------------
        public void Check()
        {

        }
        //-----------------------------------------------------
        public void OnToolBarGUI()
        {

        }
        //-----------------------------------------------------
        public bool HasCtlTips()
        {
            return false;
        }
        //-----------------------------------------------------
        public void ShowCtlTips()
        {

        }
        //-----------------------------------------------------
        public void ShowNotification(string content, float showTime)
        {
            m_pEditor.ShowNotification(new GUIContent(content), showTime);
        }
        //-----------------------------------------------------
        public void OnGUI()
        {
            if (m_eTab == ETab.Element)
            {
                if (m_pBattleObject != null) m_pBattleObject.OnGUI();
            }
            else if (m_eTab == ETab.Monster)
            {
                if(m_pMonsterObject!=null) m_pMonsterObject.OnGUI();
            }
        }
        //-----------------------------------------------------
        public void OnSceneGUI(SceneView view)
        {
            if (m_eTab == ETab.Element)
            {
                if (m_pBattleObject != null) m_pBattleObject.OnSceneGUI(view);
            }
            else if (m_eTab == ETab.Monster)
            {
                if (m_pMonsterObject != null) m_pMonsterObject.OnSceneGUI(view);
            }
            else if(m_eTab == ETab.TerrainScene)
            {
                if (m_pSceneLogic != null) m_pSceneLogic.OnSceneGUI(view);
            }
            OnSceneEvent(Event.current);
        }
        //-----------------------------------------------------
        public void OnDrawInspecPanel(Vector2 size)
        {
            if (m_eTab == ETab.Element)
                m_pBattleObject.OnDrawInspecPanel(size);
            else if (m_eTab == ETab.Monster)
                m_pMonsterObject.OnDrawInspecPanel(size);
            else if (m_eTab == ETab.TerrainScene)
            {
                m_pSceneLogic.OnDrawInspecPanel(size);
            }
        }
        //-----------------------------------------------------
        public void New()
        {

        }
        //-----------------------------------------------------
        public void SelectTab(ETab tab)
        {
            if (m_eTab != tab)
            {

            }
            m_eTab = tab;
        }
        //-----------------------------------------------------
        public void OnDrawLayerPanel(Vector2 size)
        {
            Color color = GUI.color;
            GUILayout.BeginHorizontal();
            for (int i = 0; i < tabs.Length; ++i)
            {
                if (i == (int)m_eTab)
                    GUI.color = Color.green;
                else GUI.color = color;
                if (GUILayout.Button(tabs[i]))
                {
                    SelectTab((ETab)i);
                }
            }
            GUILayout.EndHorizontal();
            GUI.color = color;

            if (m_eTab == ETab.Element)
            {
                m_pBattleObject.OnDrawLayerPanel(size);
            }
            else if (m_eTab == ETab.Monster)
                m_pMonsterObject.OnDrawLayerPanel(size);
            else if (m_eTab == ETab.TerrainScene)
            {
                m_pSceneLogic.OnDrawLayerPanel(size);
            }
        }
    }
}