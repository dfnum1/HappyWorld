#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	LoginVirtualScene
作    者:	HappLI
描    述:	登录虚拟场景
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using TopGame.Data;
using UnityEditor;
namespace TopGame.Logic
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LoginVirtualScene), true)]
    public class LoginVirtualSceneEditor : Editor
    {
        List<string> m_vDrawThemeIngore = new List<string>();
        List<string> m_vThemesPop = new List<string>();
        List<Data.SceneThemeData> m_vThemesDatas = new List<Data.SceneThemeData>();
        public void OnEnable()
        {
            m_vDrawThemeIngore.Add("nID");
            m_vThemesPop.Clear();
            m_vThemesDatas.Clear();
            CsvData_DungonThemes pDungonTheme = new CsvData_DungonThemes();
            if (System.IO.File.Exists("Assets/DatasRef/Config/Other/DungonThemes.json"))
            {
                if(pDungonTheme.LoadJson(System.IO.File.ReadAllText("Assets/DatasRef/Config/Other/DungonThemes.json")))
                {
                    foreach(var db in pDungonTheme.datas)
                    {
                        m_vThemesPop.Add(db.Value.strName + "[" + db.Key + "]");
                        m_vThemesDatas.Add(db.Value);
                    }
                }
            }
        }
        public override void OnInspectorGUI()
        {
            LoginVirtualScene virSce = target as LoginVirtualScene;

            virSce.timelineController = EditorGUILayout.ObjectField("timeline", virSce.timelineController, typeof(Core.TimelineController), true) as Core.TimelineController;

            LoginVirtualScene.Theme theme;
            for (int i  =0; i < virSce.themes.Count; ++i)
            {
                theme = virSce.themes[i];
                GUILayout.BeginHorizontal();
                theme.expand = EditorGUILayout.Foldout(theme.expand, theme.name + "[" + (i+1) +"]");
                if(GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(40) }))
                {
                    if(EditorUtility.DisplayDialog("提示", "确定删除? 删除后不能还原，没有做回退哦!", "删除", "再想想"))
                    {
                        virSce.themes.RemoveAt(i);
                        break;
                    }
                }
                GUILayout.EndHorizontal();
                if (theme.expand)
                {
                    theme.name = EditorGUILayout.TextField("描述", theme.name);
                    EditorGUI.indentLevel++;

                    if (theme.scenes!=null)
                    {
                        LoginVirtualScene.SceneNode sce;
                        for (int j =0; j< theme.scenes.Count; ++j)
                        {
                            sce = theme.scenes[j];
                            GUILayout.BeginHorizontal();
                            sce.expand = EditorGUILayout.Foldout(sce.expand,"场景段" + (j+1));
                            if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(40) }))
                            {
                                if (EditorUtility.DisplayDialog("提示", "确定删除? 删除后不能还原，没有做回退哦!", "删除", "再想想"))
                                {
                                    theme.scenes.RemoveAt(j);
                                    break;
                                }
                            }
                            GUILayout.EndHorizontal();
                            if (sce.expand)
                            {
                                EditorGUI.indentLevel++;
                                GameObject prefab = null;
                                if(!string.IsNullOrEmpty(sce.strFile))
                                    prefab = AssetDatabase.LoadAssetAtPath<GameObject>(sce.strFile);
                                prefab = EditorGUILayout.ObjectField("预制体", prefab, typeof(GameObject), false) as GameObject;
                                sce.strFile = AssetDatabase.GetAssetPath(prefab);
                                sce.position = EditorGUILayout.Vector3Field("位置", sce.position);
                                sce.rotation = EditorGUILayout.Vector3Field("旋转", sce.rotation);
                                EditorGUI.indentLevel--;
                            }
                            theme.scenes[j] = sce;
                        }
                    }
                    if(GUILayout.Button("添加场景段"))
                    {
                        if (theme.scenes == null) theme.scenes = new List<LoginVirtualScene.SceneNode>();
                        theme.scenes.Add(new LoginVirtualScene.SceneNode() { position = Vector3.zero, rotation = Vector3.zero, expand = true });
                    }

                    GUILayout.BeginHorizontal();
                    theme.expandTheme = EditorGUILayout.Foldout(theme.expandTheme, "主题");
                    if (GUILayout.Button("清除", new GUILayoutOption[] { GUILayout.Width(40) }))
                    {
                        if (EditorUtility.DisplayDialog("提示", "确定清除? 清除后不能还原，没有做回退哦!", "清除", "再想想"))
                        {
                            theme.theme = new DungonTheme();
                        }
                    }
                    GUILayout.EndHorizontal();
                    if (theme.expandTheme)
                    {
                        int index = -1;
                        for (int j = 0; j < m_vThemesDatas.Count; ++j)
                        {
                            if (m_vThemesDatas[j].nID == theme.theme.nID)
                            {
                                index = j;
                                break;
                            }
                        }
                        EditorGUI.indentLevel++;
                        int preIndex = index;
                        index = EditorGUILayout.Popup("主题", index, m_vThemesPop.ToArray());
                        if (preIndex != index && index >= 0 && index < m_vThemesDatas.Count)
                        {
                            theme.theme.Copy(m_vThemesDatas[index]);
                            theme.theme.nID = m_vThemesDatas[index].nID;
                        }
                        theme.theme = (Data.SceneThemeData)Framework.ED.HandleUtilityWrapper.DrawProperty(theme.theme, m_vDrawThemeIngore);
                        if (Framework.Module.ModuleManager.startUpGame && Application.isPlaying)
                        {
                            Logic.SceneTheme SceneBG = null;
                            if (Framework.Module.ModuleManager.startUpGame)
                            {
                                SceneBG = Core.SceneMgr.GetThemer() as Logic.SceneTheme;
                            }
                            Logic.AState pState = Logic.StateFactory.CurrentState();
                            if (Core.SceneMgr.GetThemer() != null)
                            {
                                if (SceneBG != null)
                                {
                                    SceneBG.UpdateFog(theme.theme.EnableFog, theme.theme.FogColor, theme.theme.FogStart, theme.theme.FogEnd, theme.theme.FogDensity, theme.theme.fLerp);
                                    SceneBG.UpdateSkyBox(theme.theme.SkyBoxMat);
                                    SceneBG.UpdateBG(theme.theme.FarBGScene);
                                    SceneBG.UpdateNearEffect(theme.theme.NearEffect);
                                    SceneBG.SetEnvironmentColor(theme.theme.EnvironmentColor, theme.theme.fLerp);
                                    SceneBG.SetFarDistance(theme.theme.BGKeepDistance, theme.theme.FarOffset, theme.theme.FarRotate);
                                    SceneBG.SetNearDistance(theme.theme.NearKeepDistance, theme.theme.NearOffset, theme.theme.NearRotate);
                                    if (pState != null && pState.IsEnable())
                                        SceneBG.UpdateMusic(theme.theme.BattleMusic, theme.theme.MusicFade);
                                    else
                                        SceneBG.UpdateMusic(theme.theme.PrefabMusic, theme.theme.MusicFade);
                                }
                            }
                            if (Base.GlobalShaderController.Instance)
                                Base.GlobalShaderController.SetBlend(theme.theme.CurveBlendAixs, theme.theme.CurveBlendOffset);
                        }
                        EditorGUI.indentLevel--;

                    }

                    EditorGUI.indentLevel--;
                }
                virSce.themes[i] = theme;
            }

            
            if (GUILayout.Button("添加"))
            {
                virSce.themes.Add(new LoginVirtualScene.Theme());
            }
            if (GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(target);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif

