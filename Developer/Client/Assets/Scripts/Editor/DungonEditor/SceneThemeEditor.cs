/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	SceneThemeEditor
作    者:	HappLI
描    述:	关卡主题编辑器
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using TopGame.Data;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using TopGame.Core;
using System.Reflection;
using TopGame.Base;
using Framework.ED;
using Framework.Data;

namespace TopGame.ED
{
    public class SceneThemeEditor : EditorWindow
    {
        static SceneThemeEditor ms_Instance = null;
        [MenuItem("Tools/场景主题")]
        public static SceneThemeEditor Open()
        {
            if (ms_Instance == null)
                ms_Instance = EditorWindow.GetWindow<SceneThemeEditor>();
            if (ms_Instance != null)
            {
                ms_Instance.titleContent = new GUIContent("场景主题");
                ms_Instance.ShowUtility();
            }
            return ms_Instance;
        }

        CsvData_DungonThemes m_DungonThemes = new CsvData_DungonThemes();
        Vector2 m_Scroll = Vector2.zero;

        List<string> m_vPop = new List<string>();
        List<DungonTheme> m_vThemes = new List<DungonTheme>();

        string m_strBackupCheck = "";
        int m_nEditorID = -1;
        DungonTheme m_pCurThere = null;
        //------------------------------------------------------
        private void OnDisable()
        {
            ms_Instance = null;

            string temp = "";
            if (m_DungonThemes != null)
            {
                foreach (var db in m_DungonThemes.datas)
                {
                    temp += JsonUtility.ToJson(db.Value);
                }
            }
            if(temp.CompareTo(m_strBackupCheck) != 0)
            {
                if (EditorKits.PopMessageBox("提示", "数据有修改，是否要保存后，再关闭", "保存", "取消"))
                {
                    m_DungonThemes.Save();
                }
            }
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            minSize = maxSize = new Vector2(800,600);

            string strFile = DungonThemes.FilePath;
            if (File.Exists(strFile))
            {
                m_DungonThemes.LoadJson(File.ReadAllText(strFile));
            }
            m_DungonThemes.strFilePath = "Assets/DatasRef/Config/Other/DungonThemes.json";
            SyncCheck();

            m_pCurThere = null;
            m_nEditorID = -1;
            RefreshPop();
        }
        //------------------------------------------------------
        void SyncCheck()
        {
            m_strBackupCheck = "";
            if (m_DungonThemes != null)
            {
                foreach (var db in m_DungonThemes.datas)
                {
                    m_strBackupCheck += JsonUtility.ToJson(db.Value);
                }
            }
        }
        //------------------------------------------------------
        int NewID()
        {
            int id = 0;
            foreach (var db in m_DungonThemes.datas)
            {
                id = Mathf.Max(id, db.Key);
            }
            id++;
            return id;
        }
        //------------------------------------------------------
        void RefreshPop()
        {
            m_vThemes.Clear();
            m_vPop.Clear();
            foreach (var db in m_DungonThemes.datas)
            {
                m_vThemes.Add(db.Value);
                m_vPop.Add(db.Value.strName + "[" + db.Value.nID + "]");
            }
        }
        //------------------------------------------------------
        private void OnGUI()
        {
            Logic.SceneTheme SceneBG = null;
            if (Framework.Module.ModuleManager.startUpGame)
            {
                SceneBG = Core.SceneMgr.GetThemer() as Logic.SceneTheme;
            }

            DungonTheme preTheme = m_pCurThere;
            EditorGUILayout.BeginHorizontal();
            int index = EditorGUILayout.Popup("主题", m_vThemes.IndexOf(m_pCurThere), m_vPop.ToArray());
            if (SceneBG!=null && m_pCurThere != null && GUILayout.Button("刷新预览"))
            {
                SceneBG.UseTheme(m_pCurThere, true);
            }
            if (GUILayout.Button("新建"))
            {
                m_pCurThere = new DungonTheme();
                m_pCurThere.nID = NewID();
                m_DungonThemes.datas.Add(m_pCurThere.nID, m_pCurThere);
                RefreshPop();
            }
            if (m_pCurThere !=null && GUILayout.Button("移除"))
            {
                if(EditorKits.PopMessageBox("提示", "确定移除该主题?", "移除", "取消"))
                {
                    m_DungonThemes.datas.Remove(m_pCurThere.nID);
                    RefreshPop();
                    m_pCurThere = null;
                    m_nEditorID = -1;
                }
            }
            if (m_pCurThere!=null && GUILayout.Button("同步RenderSettings Fog"))
            {
                m_pCurThere.EnableFog = RenderSettings.fog;
                m_pCurThere.FogColor = RenderSettings.fogColor;
                m_pCurThere.FogDensity = RenderSettings.fogDensity;
                m_pCurThere.FogStart = RenderSettings.fogStartDistance;
                m_pCurThere.FogEnd = RenderSettings.fogEndDistance;
            }
            EditorGUILayout.EndHorizontal();
            if (index>=0 && index< m_vThemes.Count)
            {
                m_pCurThere = m_vThemes[index];
            }
            if (m_pCurThere == null)
            {
                if (SceneBG != null)
                {
                    m_pCurThere = m_DungonThemes.GetData(SceneBG.GetCurTheme());
                }
            }
            if(preTheme != m_pCurThere)
            {
                m_nEditorID = -1;
            }
            if (m_pCurThere == null) return;
            if (m_nEditorID < 0) m_nEditorID = m_pCurThere.nID;

            GUILayout.BeginHorizontal();
            m_nEditorID = EditorGUILayout.IntField("ID", m_nEditorID);
            if(m_pCurThere.nID != m_nEditorID && !m_DungonThemes.datas.ContainsKey(m_nEditorID) && GUILayout.Button("修改", new GUILayoutOption[] { GUILayout.Width(50) }))
            {
                m_DungonThemes.datas.Remove(m_pCurThere.nID);
                m_pCurThere.nID = m_nEditorID;
                m_DungonThemes.datas.Add(m_nEditorID, m_pCurThere);
                RefreshPop();
            }
            GUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            string strName = m_pCurThere.strName;

            m_Scroll = GUILayout.BeginScrollView(m_Scroll, new GUILayoutOption[] { GUILayout.Height(position.height - 160) });
            m_pCurThere = (DungonTheme)HandleUtilityWrapper.DrawProperty(m_pCurThere, null);
            GUILayout.EndScrollView();

            if (m_pCurThere.strName.CompareTo(strName) != 0)
                RefreshPop();
            if(EditorGUI.EndChangeCheck())
            {
                if (Framework.Module.ModuleManager.startUpGame && Application.isPlaying)
                {
                    Logic.AState pState = Logic.StateFactory.CurrentState();
                    if (SceneMgr.GetThemer() != null)
                    {
                        if (SceneBG != null)
                        {
                            SceneBG.UpdateFog(m_pCurThere.EnableFog, m_pCurThere.FogColor, m_pCurThere.FogStart, m_pCurThere.FogEnd, m_pCurThere.FogDensity, m_pCurThere.fLerp);
                            SceneBG.UpdateSkyBox(m_pCurThere.SkyBoxMat);
                            SceneBG.UpdateBG(m_pCurThere.FarBGScene);
                            SceneBG.UpdateNearEffect(m_pCurThere.NearEffect);
                            SceneBG.SetEnvironmentColor(m_pCurThere.EnvironmentColor, m_pCurThere.fLerp);
                            SceneBG.SetFarDistance(m_pCurThere.BGKeepDistance, m_pCurThere.FarOffset, m_pCurThere.FarRotate);
                            SceneBG.SetNearDistance(m_pCurThere.NearKeepDistance, m_pCurThere.NearOffset, m_pCurThere.NearRotate);
                            if (pState!=null && pState.IsEnable())
                                SceneBG.UpdateMusic(m_pCurThere.BattleMusic, m_pCurThere.MusicFade);
                            else
                                SceneBG.UpdateMusic(m_pCurThere.PrefabMusic, m_pCurThere.MusicFade);
                        }
                    }
                    if(Base.GlobalShaderController.Instance)
                        Base.GlobalShaderController.SetBlend(m_pCurThere.CurveBlendAixs, m_pCurThere.CurveBlendOffset);
                }
            }

            GUILayout.BeginArea(new Rect(0, position.height - 80, position.width, 80));
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("保存", new GUILayoutOption[] { GUILayout.Width(position.width-100), GUILayout.Height(80) }))
            {
                if(EditorKits.PopMessageBox("提示", "确定保存","确定", "取消"))
                {
                    m_DungonThemes.Save();
                    SyncCheck();
                }
            }
            if (GUILayout.Button("关联文件", new GUILayoutOption[] { GUILayout.Width( 100), GUILayout.Height(80) }))
            {
                EditorKits.OpenPathInExplorer(m_DungonThemes.strFilePath);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}
